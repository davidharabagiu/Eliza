using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Eliza_Desktop_App
{
    public partial class MainChatControl : UserControl
    {
        public ElizaClient ClientProcess { get; set; }
        private string userName;
        private bool emptyDescription;
        public delegate void LogOutPressedEventHandler(ElizaStatus status);
        public event LogOutPressedEventHandler LogOutPressed;

        public MainChatControl()
        {
            InitializeComponent();
            emptyDescription = true;
        }

        public void SetUser(string userName)
        {
            this.userName = userName;
            labelUserName.Text = userName;
            string description = GetDescription(userName);
            if (description != null)
            {
                labelDescription.Text = description.Substring(0, description.Length - 1);
                emptyDescription = false;
            }
            Image profilePicture = GetProfilePicture(userName);
            if (profilePicture != null)
            {
                pictureProfile.Image = profilePicture;
            }
        }

        private string GetDescription(string userName)
        {
            try
            {
                ClientProcess.SendRequest(string.Format("querydescription {0}", userName));
                ClientResponse response = ClientProcess.ReceiveResponse();
                if (response.Status == ElizaStatus.STATUS_SUCCESS)
                {
                    if (response.Message == "None\n")
                    {
                        return null;
                    }
                    return response.Message;
                }
                else
                {
                    throw new ElizaClientException(response.Status);
                }
            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
                return null;
            }
        }

        private Image GetProfilePicture(string userName)
        {
            try
            {
                ClientProcess.SendRequest(string.Format("queryprofilepic {0}", userName));
                ClientResponse response = ClientProcess.ReceiveResponse();
                if (response.Status == ElizaStatus.STATUS_SUCCESS)
                {
                    int imageLength = int.Parse(response.Message);
                    byte[] imageData = ClientProcess.ReceiveFile(imageLength);
                    if (imageData == null)
                    {
                        return null;
                    }
                    using (var ms = new MemoryStream(imageData))
                    {
                        return Image.FromStream(ms);
                    }
                }
                else
                {
                    throw new ElizaClientException(response.Status);
                }
            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
                return null;
            }
        }

        private void signOutMenuButton_Click(object sender, EventArgs e)
        {
            ClientProcess.SendRequest("logout");
            ElizaStatus status = ClientProcess.ReceiveResponse().Status;
            LogOutPressed(status);
            pictureProfile.Image = Eliza_Desktop_App.Properties.Resources.default_profile_pic;
            labelDescription.Text = "Click to add description...";
            emptyDescription = true;
        }

        private void labelDescription_Click(object sender, EventArgs e)
        {
            if (emptyDescription)
            {
                textDescription.Text = "";
            }
            else
            {
                textDescription.Text = labelDescription.Text;
            }
            labelDescription.Visible = false;
            textDescription.Visible = true;
            buttonSaveDescription.Visible = true;
            buttonDiscardDescription.Visible = true;
        }

        private void buttonSaveDescription_Click(object sender, EventArgs e)
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
            finally
            {
                labelDescription.Visible = true;
                textDescription.Visible = false;
                buttonSaveDescription.Visible = false;
                buttonDiscardDescription.Visible = false;
                textDescription.Text = "";
            }
        }

        private void buttonDiscardDescription_Click(object sender, EventArgs e)
        {

            labelDescription.Visible = true;
            textDescription.Visible = false;
            buttonSaveDescription.Visible = false;
            buttonDiscardDescription.Visible = false;
            textDescription.Text = "";
        }

        private void pictureProfile_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Change Profile Picture";
                dialog.FileName = "";
                dialog.Filter = "JPEG files(*.jpg;*.jpeg)|*.jpg;*.jpeg";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Image newProfilePic = new Bitmap(Image.FromFile(dialog.FileName), new Size(100, 100));
                    byte[] fileData;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        newProfilePic.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
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
                    pictureProfile.Image = newProfilePic;
                }
            }
            catch (ElizaClientException ex)
            {
                Program.ErrorMessage(ex.Message);
            }
        }
    }
}
