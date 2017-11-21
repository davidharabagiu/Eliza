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
    public partial class FormRegister : Form
    {
        public ElizaClient ClientProcess { get; set; }
        private bool profilePictureChanged = false;

        public FormRegister()
        {
            InitializeComponent();
        }

        private void buttonCheckAvailability_Click(object sender, EventArgs e)
        {
            try
            {
                ClientProcess.SendRequest(string.Format("queryuserexists {0}", textUsername.Text));
                ClientResponse response = ClientProcess.ReceiveResponse();
                if (response.Status == ElizaStatus.QUERYRESPONSE_TRUE)
                {
                    MessageBox.Show("This username is already taken",
                        "Not available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                else if (response.Status == ElizaStatus.QUERYRESPONSE_FALSE)
                {
                    MessageBox.Show("This username is available",
                        "Available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    throw new ElizaClientException(response.Status);
                }
            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (textUsername.Text == "")
            {
                Program.ErrorMessage("The username field can't be empty.");
                return;
            }
            else if (textPassword.Text == "")
            {
                Program.ErrorMessage("The password field can't be empty");
                return;
            }
            else if (textPassword.Text != textRepeatPassword.Text)
            {
                Program.ErrorMessage("Passwords don't match.");
                return;
            }

            try
            {
                ClientProcess.SendRequest(string.Format("register {0} {1}", textUsername.Text, textPassword.Text));
                ClientResponse response = ClientProcess.ReceiveResponse();
                if (response.Status == ElizaStatus.STATUS_SUCCESS)
                {
                    MessageBox.Show("Registration completed successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    if (profilePictureChanged)
                    {
                        UpdateProfilePicture();
                    }
                    UpdateProfileDescription();
                    this.Close();
                }
                else
                {
                    throw new ElizaClientException(response.Status);
                }
            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
            }
        }

        private void UpdateProfilePicture()
        {

        }

        private void UpdateProfileDescription()
        {

        }
    }
}
