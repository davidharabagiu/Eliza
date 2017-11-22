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
    public partial class FormChat : Form
    {
        private string username;
        private bool online;
        private ElizaClient clientProcess;

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
        }

        private void FormChat_Load(object sender, EventArgs e)
        {
            
        }

        public void Setup(ElizaClient clientProcess, string username)
        {
            this.clientProcess = clientProcess;
            this.username = username;

            this.Text = string.Format("Eliza - {0}", username);
            labelUserName.Text = username;
            labelDescription.Text = clientProcess.GetDescription(username);
            Online = clientProcess.IsUserOnline(username);

            Image profilePicture = clientProcess.GetProfilePicture(username);
            if (profilePicture != null)
            {
                pictureProfile.Image = profilePicture;
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
    }
}
