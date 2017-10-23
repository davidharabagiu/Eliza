using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eliza_Desktop_App
{
    public partial class LoginControl : UserControl
    {
        public ElizaClient ClientProcess { get; set; }

        public LoginControl()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            ClientProcess.SendRequest(string.Format(
                "login {0} {1}",
                textUsername.Text,
                textPassword.Text));

            ElizaStatus status = (ElizaStatus)int.Parse(ClientProcess.ReceiveResponse());
            MessageBoxIcon mboxIcon = MessageBoxIcon.Error;
            string statusMessage;
            switch (status)
            {
                case ElizaStatus.STATUS_SUCCESS:
                    statusMessage = string.Format("Logged in succesfully as {0}.", textUsername.Text);
                    mboxIcon = MessageBoxIcon.Information;
                    break;
                case ElizaStatus.STATUS_INVALID_CREDENTIALS:
                case ElizaStatus.STATUS_INVALID_REQUEST_PARAMETERS:
                    statusMessage = "Invalid username or password.";
                    break;
                case ElizaStatus.STATUS_ALREADY_LOGGED_IN:
                    statusMessage = string.Format("{0} is already logged in.", textUsername.Text);
                    break;
                default:
                    statusMessage = string.Format("An unknown error occured: {0}.", (int)status);
                    break;
            }
            MessageBox.Show(statusMessage, "Eliza", MessageBoxButtons.OK, mboxIcon);
        }
    }
}
