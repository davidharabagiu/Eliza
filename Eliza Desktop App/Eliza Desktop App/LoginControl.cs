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
        private ElizaClient elizaClient;

        public LoginControl(ElizaClient elizaClient)
        {
            this.elizaClient = elizaClient;
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            elizaClient.SendRequest(String.Format(
                "login {0} {1}",
                textUsername.Text,
                textPassword.Text));
            MessageBox.Show(elizaClient.ReceiveResponse().ToString());
        }
    }
}
