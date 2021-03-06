﻿using System;
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
        public ElizaClient ClientProcess { get; set; }
        public delegate void LogInPressedEventHandler(ElizaStatus status, string userName);
        public event LogInPressedEventHandler LogInPressed;

        public LoginControl()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textUsername.Text.Length == 0)
            {
                MessageDialogs.Error("Username field can't be empty.");
            }
            if (textPassword.Text.Length == 0)
            {
                MessageDialogs.Error("Password field can't be empty.");
            }

            ElizaStatus status = ClientProcess.Login(textUsername.Text, textPassword.Text);
            LogInPressed(status, textUsername.Text);
            textUsername.Text = "";
            textPassword.Text = "";
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            FormRegister frm = new FormRegister();
            frm.ClientProcess = ClientProcess;
            frm.Show();
        }
    }
}
