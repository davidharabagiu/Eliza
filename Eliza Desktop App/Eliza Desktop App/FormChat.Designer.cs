namespace Eliza_Desktop_App
{
    partial class FormChat
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChat));
            this.labelUserName = new System.Windows.Forms.Label();
            this.pictureOnlineStatus = new System.Windows.Forms.PictureBox();
            this.pictureProfile = new System.Windows.Forms.PictureBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.timerCheckOnline = new System.Windows.Forms.Timer(this.components);
            this.chatBox = new System.Windows.Forms.WebBrowser();
            this.textSongName = new System.Windows.Forms.TextBox();
            this.buttonSendSong = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOnlineStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserName.Location = new System.Drawing.Point(147, 9);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(183, 38);
            this.labelUserName.TabIndex = 3;
            this.labelUserName.Text = "User Name";
            // 
            // pictureOnlineStatus
            // 
            this.pictureOnlineStatus.BackColor = System.Drawing.Color.Transparent;
            this.pictureOnlineStatus.Image = global::Eliza_Desktop_App.Properties.Resources.Ball_red_64;
            this.pictureOnlineStatus.Location = new System.Drawing.Point(118, 12);
            this.pictureOnlineStatus.Name = "pictureOnlineStatus";
            this.pictureOnlineStatus.Size = new System.Drawing.Size(32, 32);
            this.pictureOnlineStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureOnlineStatus.TabIndex = 4;
            this.pictureOnlineStatus.TabStop = false;
            // 
            // pictureProfile
            // 
            this.pictureProfile.BackColor = System.Drawing.Color.White;
            this.pictureProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureProfile.Image = global::Eliza_Desktop_App.Properties.Resources.default_profile_pic;
            this.pictureProfile.Location = new System.Drawing.Point(12, 12);
            this.pictureProfile.Name = "pictureProfile";
            this.pictureProfile.Size = new System.Drawing.Size(100, 100);
            this.pictureProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureProfile.TabIndex = 2;
            this.pictureProfile.TabStop = false;
            // 
            // labelDescription
            // 
            this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDescription.Location = new System.Drawing.Point(119, 51);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(608, 61);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // textMessage
            // 
            this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textMessage.Location = new System.Drawing.Point(13, 553);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(664, 27);
            this.textMessage.TabIndex = 0;
            this.textMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textMessage_KeyDown);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(683, 552);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(44, 28);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = ">";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // timerCheckOnline
            // 
            this.timerCheckOnline.Enabled = true;
            this.timerCheckOnline.Interval = 1000;
            this.timerCheckOnline.Tick += new System.EventHandler(this.timerCheckOnline_Tick);
            // 
            // chatBox
            // 
            this.chatBox.Location = new System.Drawing.Point(13, 118);
            this.chatBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.chatBox.Name = "chatBox";
            this.chatBox.Size = new System.Drawing.Size(714, 429);
            this.chatBox.TabIndex = 6;
            // 
            // textSongName
            // 
            this.textSongName.Location = new System.Drawing.Point(467, 14);
            this.textSongName.Name = "textSongName";
            this.textSongName.Size = new System.Drawing.Size(162, 22);
            this.textSongName.TabIndex = 7;
            // 
            // buttonSendSong
            // 
            this.buttonSendSong.Location = new System.Drawing.Point(635, 9);
            this.buttonSendSong.Name = "buttonSendSong";
            this.buttonSendSong.Size = new System.Drawing.Size(92, 32);
            this.buttonSendSong.TabIndex = 8;
            this.buttonSendSong.Text = "Send Song";
            this.buttonSendSong.UseVisualStyleBackColor = true;
            this.buttonSendSong.Click += new System.EventHandler(this.buttonSendSong_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(595, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 29);
            this.button1.TabIndex = 9;
            this.button1.Text = "Start Webcam";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(739, 592);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonSendSong);
            this.Controls.Add(this.textSongName);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.pictureOnlineStatus);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.pictureProfile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormChat";
            this.Text = "Eliza";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormChat_FormClosing);
            this.Load += new System.EventHandler(this.FormChat_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureOnlineStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.PictureBox pictureProfile;
        private System.Windows.Forms.PictureBox pictureOnlineStatus;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Timer timerCheckOnline;
        private System.Windows.Forms.WebBrowser chatBox;
        private System.Windows.Forms.TextBox textSongName;
        private System.Windows.Forms.Button buttonSendSong;
        private System.Windows.Forms.Button button1;
    }
}