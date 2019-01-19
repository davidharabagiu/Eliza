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
    public partial class RoomNameDialog : Form
    {
        public string RoomName { get; set; }
        public bool IsPublic { get; set; }

        public RoomNameDialog()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RoomName = textBox1.Text;
                IsPublic = checkBox1.Checked;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
