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
        private bool owner;
        private SortedList<string, Message> messagesList;
        private Dictionary<int, SortedList<string, Message>> repliesList;

        private string chatText = "<font face = \"Microsoft Sans Serif\" size = \"3\">";

        public FormChatRoom()
        {
            InitializeComponent();
            chatBoxMutex = new Mutex();
            userColors = new Dictionary<string, Color>();
            messagesList = new SortedList<string, Message>();
            repliesList = new Dictionary<int, SortedList<string, Message>>();
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

            if (!clientProcess.CheckMembership(roomName))
            {
                clientProcess.BroadcastAnnoucement(roomName, String.Format("{0} has joined", myUsername));
                ElizaStatus status = clientProcess.AddToRoom(myUsername, roomName);
                if (status != ElizaStatus.STATUS_SUCCESS)
                {
                    MessageDialogs.Error("Error " + status.ToString());
                }
            }

            CheckUsers();

            owner = clientProcess.CheckOwnership(roomName);
            if (owner)
            {
                labelRoomName.Text = "[My] " + labelRoomName.Text;
            }
            else
            {
                buttonKick.Visible = false;
                buttonDelete.Visible = false;
                button1.Visible = false;
            }

            string messages = clientProcess.GetRoomMessages(roomName);
            string[] messagesArray = messages.Split(new char[] { '\r', '\n' });

            foreach (string msg in messagesArray)
            {
                string[] msgData = msg.Split(new char[] { ' ' });
                if (msgData.Length < 4)
                {
                    continue;
                }

                string msgContent = "";
                for (int i = 3; i < msgData.Length; ++i)
                {
                    msgContent += msgData[i] + " ";
                }

                Message msgObj = new Message(Convert.ToInt32(msgData[0]), msgData[1], msgData[2], msgContent);
                messagesList.Add(msgObj.Timestamp, msgObj);
            }

            messages = clientProcess.GetReplies(roomName);
            messagesArray = messages.Split(new char[] { '\r', '\n' });

            foreach (string msg in messagesArray)
            {
                string[] msgData = msg.Split(new char[] { ' ' });
                if (msgData.Length < 5)
                {
                    continue;
                }

                string msgContent = "";
                for (int i = 4; i < msgData.Length; ++i)
                {
                    msgContent += msgData[i] + " ";
                }

                Message msgObj = new Message(Convert.ToInt32(msgData[1]), msgData[2], msgData[3], msgContent);
                int repliedTo = Convert.ToInt32(msgData[0]);
                if (!repliesList.ContainsKey(repliedTo))
                {
                    repliesList.Add(repliedTo, new SortedList<string, Message>());
                }
                repliesList[repliedTo].Add(msgObj.Timestamp, msgObj);
            }

            messages = clientProcess.GetAnnouncements(roomName);
            messagesArray = messages.Split(new char[] { '\r', '\n' });

            foreach (string msg in messagesArray)
            {
                string[] msgData = msg.Split(new char[] { ' ' });
                if (msgData.Length < 2)
                {
                    continue;
                }

                string msgContent = "";
                for (int i = 1; i < msgData.Length; ++i)
                {
                    msgContent += msgData[i] + " ";
                }

                Message msgObj = new Message(msgData[0], msgContent);
                messagesList.Add(msgObj.Timestamp, msgObj);
            }

            DisplayMessages();

            clientProcess.BroadcastMessageReceived += ClientProcess_MessageReceived;
            clientProcess.ReplyReceived += ClientProcess_ReplyReceived;
            clientProcess.AnnouncementReceived += ClientProcess_AnnouncementReceived;
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

        private void DisplayMessages()
        {
            chatText += "<div style=\" width: 200px\">";
            chatText = "<font face = \"Microsoft Sans Serif\" size = \"3\">";

            foreach (Message msg in messagesList.Values)
            {
                string timestamp = msg.Timestamp.Substring(0, 4) + "." + msg.Timestamp.Substring(4, 2) + "." + msg.Timestamp.Substring(6, 2) + " " +
                    msg.Timestamp.Substring(8, 2) + ":" + msg.Timestamp.Substring(10, 2) + ":" + msg.Timestamp.Substring(12, 2);

                if (msg.IsAnnouncement)
                {
                    chatText += "<div align=\"center\"  style=\"background:#EEEEEE; word-wrap:break-word;\">";
                    chatText += string.Format("<font color = \"#a0a0a0\"><i>{0}</i></font><br><font color=\"#a0a0a0\" size=\"1\">{1}</font><br>",
                                msg.Content,
                                timestamp);
                    chatText += "</div><br/>";
                }
                else
                {
                    if (msg.Username == myUsername)
                    {
                        chatText += "<div align=\"right\"  style=\"background:#EEEEEE; word-wrap:break-word;\">";
                    }
                    else
                    {
                        chatText += "<div align=\"left\"  style=\"background:#EEEEEE;  word-wrap:break-word;\">";
                    }
                    chatText += string.Format("<font color=\"#a0a0a0\" size=\"1\">{0}</font><font color = \"" + GetColor(msg.Username) + "\"><b> {1}: </b></font>{2}<br><font color=\"#a0a0a0\" size=\"1\">{3}</font><br>",
                                    msg.Id,
                                    msg.Username,
                                    msg.Content,
                                    timestamp);
                    if (repliesList.ContainsKey(msg.Id))
                    {
                        chatText += "<div>";
                        foreach (Message reply in repliesList[msg.Id].Values)
                        {
                            string tr = reply.Timestamp.Substring(0, 4) + "." + reply.Timestamp.Substring(4, 2) + "." + reply.Timestamp.Substring(6, 2) +
                              reply.Timestamp.Substring(8, 2) + ":" + reply.Timestamp.Substring(10, 2) + ":" + reply.Timestamp.Substring(12, 2);
                            chatText += string.Format("<font size = \"2\"><font color = \" " + GetColor(reply.Username) + "\"><b>&rarr;{0}: </b></font>{1}</font><br><font color=\"#a0a0a0\" size=\"1\">{2}</font><br>",
                                        reply.Username,
                                        reply.Content,
                                        tr);
                        }
                        chatText += "</div>";
                    }
                    chatText += "</div><br/>";
                }
            }

            chatText += "</font></div>";
            chatBox.DocumentText = chatText;
        }

        private void ClientProcess_MessageReceived(int id, string timestamp, string roomName, string username, string message)
        {
            if (this.roomName == roomName)
            {
                chatBoxMutex.WaitOne();
                messagesList.Add(timestamp, new Message(id, timestamp, username, message));
                DisplayMessages();
                chatBoxMutex.ReleaseMutex();
            }
        }

        private void ClientProcess_ReplyReceived(int replyid, int id, string timestamp, string roomName, string username, string message)
        {
            if (this.roomName == roomName)
            {
                chatBoxMutex.WaitOne();

                if (!repliesList.ContainsKey(replyid))
                {
                    repliesList.Add(replyid, new SortedList<string, Message>());
                }
                repliesList[replyid].Add(timestamp, new Message(id, timestamp, username, message));

                DisplayMessages();
                chatBoxMutex.ReleaseMutex();
            }
        }

        private void ClientProcess_AnnouncementReceived(string timestamp, string roomName, string message)
        {
            if (this.roomName == roomName)
            {
                chatBoxMutex.WaitOne();
                messagesList.Add(timestamp, new Message(timestamp, message));
                DisplayMessages();
                chatBoxMutex.ReleaseMutex();
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
                ElizaStatus status = ElizaStatus.STATUS_SUCCESS;

                if (textBoxReplyTo.Text.Length > 0)
                {
                    status = clientProcess.BroadcastReply(roomName, textMessage.Text, textBoxReplyTo.Text);
                    if (status != ElizaStatus.STATUS_SUCCESS)
                    {
                        MessageDialogs.Error(status.ToString());
                    }
                    else
                    {
                        Message repliedMsg = null;
                        foreach (Message msg in messagesList.Values)
                        {
                            if (!msg.IsAnnouncement && msg.Id.ToString() == textBoxReplyTo.Text)
                            {
                                repliedMsg = msg;
                                break;
                            }
                        }
                        if (repliedMsg != null)
                        {
                            //clientProcess.BroadcastAnnoucement(roomName, String.Format("{0} replied to {1}", myUsername, repliedMsg.Username));
                        }
                    }
                }
                else
                {
                    status = clientProcess.BroadcastMessage(roomName, textMessage.Text);
                }

                if (status == ElizaStatus.STATUS_SUCCESS)
                {
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
            if (owner)
            {
                UserNameDialog dlg = new UserNameDialog();
                dlg.ShowDialog();
                if (dlg.UserName != null && dlg.UserName.Length > 0)
                {
                    ElizaStatus status = clientProcess.TransferOwnership(roomName, dlg.UserName);
                    if (status != ElizaStatus.STATUS_SUCCESS)
                    {
                        MessageDialogs.Error(status.ToString());
                        return;
                    }
                    clientProcess.BroadcastAnnoucement(roomName, String.Format("{0} is now the owner", dlg.UserName));
                }
                else
                {
                    return;
                }   
            }
            clientProcess.BroadcastAnnoucement(roomName, String.Format("{0} left the room", myUsername));
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
                    clientProcess.BroadcastAnnoucement(roomName, String.Format("{0} got kicked", dlg.UserName));
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
                    clientProcess.BroadcastAnnoucement(roomName, String.Format("{0} has joined", dlg.UserName));
                }
                else
                {
                    MessageDialogs.Error(status.ToString());
                }
            }
        }

        private void chatBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            chatBox.Document.Window.ScrollTo(0, 1000000);
        }
    }
}
