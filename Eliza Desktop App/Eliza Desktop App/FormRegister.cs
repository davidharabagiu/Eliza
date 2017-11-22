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
            if (ClientProcess.IsUsernameAvailable(textUsername.Text))
            {
                MessageDialogs.Info("This username is available");
            }
            else
            {
                MessageDialogs.Warning("This username is already taken");
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (textUsername.Text == "")
            {
                MessageDialogs.Error("The username field can't be empty.");
            }
            else if (textPassword.Text == "")
            {
                MessageDialogs.Error("The password field can't be empty");
            }
            else if (textPassword.Text != textRepeatPassword.Text)
            {
                MessageDialogs.Error("Passwords do not match.");
            }
            else
            {
                ElizaStatus status = ClientProcess.Register(textUsername.Text, textPassword.Text);
                switch (status)
                {
                    case ElizaStatus.STATUS_SUCCESS:
                        MessageDialogs.Info("Registration completed successfully.");
                        ClientProcess.Login(textUsername.Text, textPassword.Text);
                        
                        if (textDescription.Text.Length > 1000)
                        {
                            textDescription.Text = textDescription.Text.Substring(0, 1000);
                            ClientProcess.SetDescription(textDescription.Text);
                        }

                        if (profilePictureChanged)
                        {
                            ClientProcess.SetProfilePicture(pictureProfile.Image);
                        }

                        ClientProcess.Logout();
                        break;

                    case ElizaStatus.STATUS_USERNAME_TOO_LONG:
                        MessageDialogs.Error("The username should be at most 30 characters long.");
                        break;

                    case ElizaStatus.STATUS_USERNAME_TOO_SHORT:
                        MessageDialogs.Error("The username should be at least 5 characters long.");
                        break;

                    case ElizaStatus.STATUS_PASSWORD_TOO_LONG:
                        MessageDialogs.Error("The password should be at most 20 characters long.");
                        break;

                    case ElizaStatus.STATUS_PASSWORD_TOO_SHORT:
                        MessageDialogs.Error("The password should be at least 5 characters long.");
                        break;

                    case ElizaStatus.STATUS_USERNAME_NON_ALPHANUMERIC:
                        MessageDialogs.Error("The username should contain only letters and digits.");
                        break;

                    case ElizaStatus.STATUS_USERNAME_ALREADY_EXISTS:
                        MessageDialogs.Error("The username is already taken.");
                        break;
                }
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
