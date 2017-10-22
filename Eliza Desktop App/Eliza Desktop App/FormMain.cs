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
            this.elizaClient = elizaClient;
            InitializeComponent(elizaClient);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            elizaClient.Close();
        }
    }
}
