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
            mainChatControl.ClientProcess = elizaClient;
            mainChatControl.LogOutPressed += MainChatControl_LogOutPressed;
            loginControl.ClientProcess = elizaClient;
            loginControl.LogInPressed += LoginControl_LogInPressed;
            FormClosing += FormMain_FormClosing;
        }

        private void MainChatControl_LogOutPressed(ElizaStatus status)
        {
            try
            {
                if (status != ElizaStatus.STATUS_SUCCESS)
                {
                    throw new ElizaClientException(status);
                }
            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
            }
            finally
            {
                mainChatControl.Hide();
                loginControl.Show();
                pictureBoxLogo.Show();
            }
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
                case ElizaStatus.STATUS_INVALID_REQUEST_PARAMETERS:
                    Program.ErrorMessage("Invalid username or password.");
                    break;
                case ElizaStatus.STATUS_ALREADY_LOGGED_IN:
                    Program.ErrorMessage(string.Format("{0} is already logged in.", userName));
                    break;
                default:
                    Program.ErrorMessage(string.Format("An unknown error occured: {0}.", (int)status));
                    break;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            elizaClient.Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }
    }
}
