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
            labelDescription.Text = GetDescription(userName);
        }

        private string GetDescription(string userName)
        {
            ClientProcess.SendRequest(string.Format("querydescription {0}", userName));
            ClientResponse response = ClientProcess.ReceiveResponse();
            if (response.Status == ElizaStatus.STATUS_SUCCESS)
            {
                return response.Message;
            }
            return "Eroare plm n-am tratat exceptia asta";
        }
    }
}
