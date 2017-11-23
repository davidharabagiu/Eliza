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
    public partial class FormChat : Form
    {
        private string username;
        private string myUsername;
        private bool online;
        private ElizaClient clientProcess;
        private Mutex chatBoxMutex;

        private string chatText = "<font face = \"Microsoft Sans Serif\" size = \"3\">";

        private bool Online
        {
            get
            {
                return online;
            }
            set
            {
                online = value;
                if (online)
                {
                    pictureOnlineStatus.Image = Eliza_Desktop_App.Properties.Resources.Ball_green_64;
                }
                else
                {
                    pictureOnlineStatus.Image = Eliza_Desktop_App.Properties.Resources.Ball_red_64;
                }
            }
        }

        public FormChat()
        {
            InitializeComponent();
            chatBoxMutex = new Mutex();
        }

        private void FormChat_Load(object sender, EventArgs e)
        {
            
        }

        public void Setup(ElizaClient clientProcess, string myUsername, string username)
        {
            this.clientProcess = clientProcess;
            this.username = username;
            this.myUsername = myUsername;

            this.Text = string.Format("Eliza - {0}", username);
            labelUserName.Text = username;
            labelDescription.Text = clientProcess.GetDescription(username);
            Online = clientProcess.IsUserOnline(username);

            Image profilePicture = clientProcess.GetProfilePicture(username);
            if (profilePicture != null)
            {
                pictureProfile.Image = profilePicture;
            }

            clientProcess.MessageReceived += ClientProcess_MessageReceived;
        }

        private void DisplayMessage(string username, string message)
        {
            chatBoxMutex.WaitOne();

            chatText += string.Format("<font color = \"Blue\"><b>{0}: </b></font>{1}<br>",
                            myUsername,
                            textMessage.Text);
            chatBox.DocumentText = chatText;

            chatBoxMutex.ReleaseMutex();
        }

        private void ClientProcess_MessageReceived(string username, string message)
        {
            if (this.username == username)
            {
                DisplayMessage(username, message);
            }
        }

        private void timerCheckOnline_Tick(object sender, EventArgs e)
        {
            bool newOnlineStatus = clientProcess.IsUserOnline(username);
            if (newOnlineStatus != Online)
            {
                Online = newOnlineStatus;
            }
        }

        private void SendMessage()
        {
            if (!Online)
            {
                MessageDialogs.Error(string.Format("{0} is offline.", username));
            }
            else if (textMessage.Text.Length < 1)
            {
                // nothing kek
            }
            else if (textMessage.Text.Length > 900)
            {
                MessageDialogs.Error(string.Format("The message can't be longer than 900 characters.", username));
            }
            else
            {
                ElizaStatus status = clientProcess.SendMessage(username, textMessage.Text);

                switch (status)
                {
                    case ElizaStatus.STATUS_SUCCESS:
                        DisplayMessage(myUsername, textMessage.Text);
                        textMessage.Clear();
                        break;

                    case ElizaStatus.STATUS_USER_NOT_ONLINE:
                        MessageDialogs.Error(string.Format("{0} is offline.", username));
                        break;

                    case ElizaStatus.STATUS_SENDER_BLOCKED:
                        MessageDialogs.Error(string.Format("{0} blocked you.", username));
                        break;

                    case ElizaStatus.STATUS_RECEIVER_BLOCKED:
                        MessageDialogs.Error(string.Format("You have blocked {0}.", username));
                        break;
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
            chatBoxMutex.Close();
        }
    }
}
