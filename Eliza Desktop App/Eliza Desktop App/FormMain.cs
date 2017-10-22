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
            loginControl.ClientProcess = elizaClient;
            FormClosing += FormMain_FormClosing;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            elizaClient.Close();
        }
    }
}
