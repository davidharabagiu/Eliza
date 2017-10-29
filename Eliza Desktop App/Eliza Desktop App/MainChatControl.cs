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

        public MainChatControl()
        {
            InitializeComponent();
        }

        public void SetUser(string userName)
        {
            this.userName = userName;
            labelUserName.Text = userName;
            string description = GetDescription(userName);
            if (description != null)
            {
                labelDescription.Text = description;
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
                    if (response.Message == "None")
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
    }
}
