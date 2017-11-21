using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
                    ClientProcess.SendRequest(string.Format("login {0} {1}", textUsername.Text, textPassword.Text));
                    ElizaStatus status = ClientProcess.ReceiveResponse().Status;
                    if (profilePictureChanged)
                    {
                        UpdateProfilePicture();
                    }
                    UpdateProfileDescription();
                    ClientProcess.SendRequest("logout");
                    status = ClientProcess.ReceiveResponse().Status;
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
            try
            {
                byte[] fileData;
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureProfile.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fileData = ms.ToArray();
                }
                ClientResponse response = ClientProcess.SendFile(fileData);
                if (response.Status != ElizaStatus.STATUS_SUCCESS)
                {
                    throw new ElizaClientException(response.Status);
                }
                ClientProcess.SendRequest("setprofilepic");
                response = ClientProcess.ReceiveResponse();
                if (response.Status != ElizaStatus.STATUS_SUCCESS)
                {
                    throw new ElizaClientException(response.Status);
                }
            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
            }
        }

        private void UpdateProfileDescription()
        {
            try
            {
                if (textDescription.Text.Length > 1000)
                {
                    Program.ErrorMessage("Description too long.");
                    return;
                }
                ClientProcess.SendRequest(string.Format("setdescription {0}", textDescription.Text));
                ElizaStatus status = ClientProcess.ReceiveResponse().Status;
                if (status != ElizaStatus.STATUS_SUCCESS)
                {
                    throw new ElizaClientException(status);
                }
                labelDescription.Text = textDescription.Text;

            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
            }
        }

        private void buttonAddProfilePic_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Add Profile Picture";
            dialog.FileName = "";
            dialog.Filter = "JPEG files(*.jpg;*.jpeg)|*.jpg;*.jpeg";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Image newProfilePic = new Bitmap(Image.FromFile(dialog.FileName), new Size(100, 100));
                pictureProfile.Image = newProfilePic;
                profilePictureChanged = true;
            }
        }
    }
}
