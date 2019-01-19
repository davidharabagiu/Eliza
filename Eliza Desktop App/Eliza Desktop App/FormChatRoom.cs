using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eliza_Desktop_App
{
    public partial class FormChatRoom : Form
    {
        public class MyListBoxItem
        {
            public MyListBoxItem(Color c, string m)
            {
                ItemColor = c;
                Message = m;
            }
            public Color ItemColor { get; set; }
            public string Message { get; set; }
        }

        private string roomName;
        private string myUsername;
        private ElizaClient clientProcess;
        private Mutex chatBoxMutex;
        private Dictionary<string, Color> userColors;
        private static Random rand = new Random();

        private string chatText = "<font face = \"Microsoft Sans Serif\" size = \"3\">";

        public FormChatRoom()
        {
            InitializeComponent();
            chatBoxMutex = new Mutex();
            userColors = new Dictionary<string, Color>();
        }

        private string GetColor(string username)
        {
            if (userColors.ContainsKey(username))
            {
                return ColorTranslator.ToHtml(userColors[username]);
            }
            userColors.Add(username, Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
            return ColorTranslator.ToHtml(userColors[username]);
        }

        private void FormChat_Load(object sender, EventArgs e)
        {
            
        }

        public void Setup(ElizaClient clientProcess, string myUsername, string roomName)
        {
            this.clientProcess = clientProcess;
            this.roomName = roomName;
            this.myUsername = myUsername;

            this.Text = string.Format("Eliza - {0}", roomName);
            labelRoomName.Text = roomName;

            ElizaStatus status = clientProcess.AddToRoom(myUsername, roomName);
            if (status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error("Error " + status.ToString());
            }

            CheckUsers();

            bool owner = clientProcess.CheckOwnership(roomName);
            if (owner)
            {
                labelRoomName.Text = "[My] " + labelRoomName.Text;
                buttonLeave.Visible = false;
            }
            else
            {
                buttonKick.Visible = false;
                buttonDelete.Visible = false;
                button1.Visible = false;
            }

            string messages = clientProcess.GetRoomMessages(roomName);
            string[] messagesArray = messages.Split(new char[] { '\r', '\n' });

            chatBoxMutex.WaitOne();
            foreach (string msg in messagesArray)
            {
                string[] msgData = msg.Split(new char[] { ' ' });
                if (msgData.Length < 2)
                {
                    continue;
                }

                string msgContent = "";
                for (int i = 2; i < msgData.Length; ++i)
                {
                    msgContent += msgData[i] + " ";
                }
                if (msgData[1] == myUsername)
                {
                    chatText += "<div  align=\"right\">";
                }
                else
                {
                    chatText += "<div  align=\"left\">";
                }
                chatText += string.Format("<font color = \"" + GetColor(msgData[1]) +"\"><b>{0}: </b></font>{1}<br></div>",
                            msgData[1],
                            msgContent);
            }
            chatBox.DocumentText = chatText;
            chatBoxMutex.ReleaseMutex();

            clientProcess.BroadcastMessageReceived += ClientProcess_MessageReceived;
        }

        private void CheckUsers()
        {
            listBoxUsers.Items.Clear();
            Dictionary<string, bool> users = clientProcess.GetRoomMembers(roomName);
            foreach (string user in users.Keys)
            {
                listBoxUsers.Items.Add(new MyListBoxItem(users[user] ? Color.Green : Color.Red, user));
            }
        }

        private void DisplayMessage(string username, string message)
        {
            chatBoxMutex.WaitOne();

            if (username == myUsername)
            {
                chatText += "<div  align=\"right\">";
            }
            else
            {
                chatText += "<div align=\"left\">";
            }
            chatText += string.Format("<font color = \"" + GetColor(username) + "\"><b>{0}: </b></font>{1}<br></div>",
                            username,
                            message);
            chatBox.DocumentText = chatText;

            chatBoxMutex.ReleaseMutex();
        }

        private void ClientProcess_MessageReceived(string roomName, string username, string message)
        {
            if (this.roomName == roomName)
            {
                DisplayMessage(username, message);
            }
        }

        private void timerCheckOnline_Tick(object sender, EventArgs e)
        {
            CheckUsers();
        }

        private void SendMessage()
        {
            if (textMessage.Text.Length < 1)
            {
                // nothing kek
            }
            else if (textMessage.Text.Length > 100)
            {
                MessageDialogs.Error(string.Format("The message can't be longer than 100 characters.", roomName));
            }
            else
            {
                ElizaStatus status = clientProcess.BroadcastMessage(roomName, textMessage.Text);

                if (status == ElizaStatus.STATUS_SUCCESS)
                {
                    DisplayMessage(myUsername, textMessage.Text);
                    textMessage.Clear();
                }
                else
                {
                    MessageDialogs.Error("Error " + status.ToString());
                    Close();
                }
            }
        }

        private void textMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void FormChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientProcess.BroadcastMessageReceived -= ClientProcess_MessageReceived;
            chatBoxMutex.Close();
        }

        private void listBoxUsers_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBoxUsers.Items.Count)
            {
                return;
            }
            MyListBoxItem item = listBoxUsers.Items[e.Index] as MyListBoxItem; // Get the current item and cast it to MyListBoxItem
            if (item != null)
            {
                e.Graphics.DrawString( // Draw the appropriate text in the ListBox
                    item.Message, // The message linked to the item
                    listBoxUsers.Font, // Take the font from the listbox
                    new SolidBrush(item.ItemColor), // Set the color 
                    0, // X pixel coordinate
                    e.Index * listBoxUsers.ItemHeight // Y pixel coordinate.  Multiply the index by the ItemHeight defined in the listbox.
                );
            }
            else
            {
                // The item isn't a MyListBoxItem, do something about it
            }
        }

        private void buttonLeave_Click(object sender, EventArgs e)
        {
            clientProcess.Kick(myUsername, roomName);
            Close();
        }

        private void buttonKick_Click(object sender, EventArgs e)
        {
            UserNameDialog dlg = new UserNameDialog();
            dlg.ShowDialog();
            if (dlg.UserName.Length > 0 && dlg.UserName != myUsername)
            {
                var status = clientProcess.Kick(dlg.UserName, roomName);
                if (status == ElizaStatus.STATUS_SUCCESS)
                {
                    CheckUsers();
                }
                else
                {
                    MessageDialogs.Error(status.ToString());
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            clientProcess.DeleteRoom(roomName);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserNameDialog dlg = new UserNameDialog();
            dlg.ShowDialog();
            if (dlg.UserName.Length > 0)
            {
                var status = clientProcess.AddToRoom(dlg.UserName, roomName);
                if (status == ElizaStatus.STATUS_SUCCESS)
                {
                    CheckUsers();
                }
                else
                {
                    MessageDialogs.Error(status.ToString());
                }
            }
        }
    }
}
