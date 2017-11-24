using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eliza_Desktop_App
{
    public partial class FormMain : Form
    {
        private ElizaClient elizaClient;

        public FormMain(ElizaClient elizaClient)
        {
            InitializeComponent();
            this.elizaClient = elizaClient;
            this.elizaClient.CommunicationError += ElizaClient_CommunicationError;
            mainChatControl.ClientProcess = elizaClient;
            mainChatControl.LogOutPressed += MainChatControl_LogOutPressed;
            mainChatControl.ExitPressed += MainChatControl_ExitPressed;
            loginControl.ClientProcess = elizaClient;
            loginControl.LogInPressed += LoginControl_LogInPressed;
            FormClosing += FormMain_FormClosing;
        }

        private void ElizaClient_CommunicationError()
        {
            ExitProgram();
        }

        private void MainChatControl_ExitPressed()
        {
            ExitProgram();
        }

        private void MainChatControl_LogOutPressed()
        {
            mainChatControl.Hide();
            loginControl.Show();
            pictureBoxLogo.Show();
        }

        private void LoginControl_LogInPressed(ElizaStatus status, string userName)
        {
            switch (status)
            {
                case ElizaStatus.STATUS_SUCCESS:
                    loginControl.Hide();
                    pictureBoxLogo.Hide();
                    mainChatControl.SetUser(userName);
                    mainChatControl.Show();
                    break;
                case ElizaStatus.STATUS_INVALID_CREDENTIALS:
                    MessageDialogs.Error("Invalid username or password.");
                    break;
                case ElizaStatus.STATUS_ALREADY_LOGGED_IN:
                    MessageDialogs.Error(string.Format("{0} is already logged in.", userName));
                    break;
            }
        }

        private void ExitProgram()
        {
            elizaClient.Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitProgram();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }
    }
}
