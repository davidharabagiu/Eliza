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
        private string userName1;
        private string userName2;
        private ElizaClient clientProcess;

        public FormChat()
        {
            InitializeComponent();
        }

        private void FormChat_Load(object sender, EventArgs e)
        {
            
        }

        public void Setup(ElizaClient clientProcess, string userName1, string userName2)
        {
            this.clientProcess = clientProcess;
            this.userName1 = userName1;
            this.userName2 = userName2;
        }
    }
}
