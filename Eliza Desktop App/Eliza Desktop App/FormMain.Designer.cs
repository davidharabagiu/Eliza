namespace Eliza_Desktop_App
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.loginControl = new Eliza_Desktop_App.LoginControl();
            this.mainChatControl = new Eliza_Desktop_App.MainChatControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::Eliza_Desktop_App.Properties.Resources.eliza_logo;
            this.pictureBoxLogo.Location = new System.Drawing.Point(70, 48);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(375, 169);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // loginControl
            // 
            this.loginControl.ClientProcess = null;
            this.loginControl.Location = new System.Drawing.Point(99, 295);
            this.loginControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loginControl.Name = "loginControl";
            this.loginControl.Size = new System.Drawing.Size(325, 137);
            this.loginControl.TabIndex = 1;
            // 
            // mainChatControl
            // 
            this.mainChatControl.Location = new System.Drawing.Point(0, 0);
            this.mainChatControl.Name = "mainChatControl";
            this.mainChatControl.Size = new System.Drawing.Size(507, 692);
            this.mainChatControl.TabIndex = 2;
            this.mainChatControl.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(511, 693);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.loginControl);
            this.Controls.Add(this.mainChatControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Eliza";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LoginControl loginControl;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private MainChatControl mainChatControl;
    }
}