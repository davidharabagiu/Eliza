namespace Eliza_Desktop_App
{
    partial class FormChatRoom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChatRoom));
            this.labelRoomName = new System.Windows.Forms.Label();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.timerCheckOnline = new System.Windows.Forms.Timer(this.components);
            this.chatBox = new System.Windows.Forms.WebBrowser();
            this.listBoxUsers = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // labelRoomName
            // 
            this.labelRoomName.AutoSize = true;
            this.labelRoomName.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRoomName.Location = new System.Drawing.Point(11, 9);
            this.labelRoomName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRoomName.Name = "labelRoomName";
            this.labelRoomName.Size = new System.Drawing.Size(165, 31);
            this.labelRoomName.TabIndex = 3;
            this.labelRoomName.Text = "Room Name";
            // 
            // textMessage
            // 
            this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textMessage.Location = new System.Drawing.Point(10, 478);
            this.textMessage.Margin = new System.Windows.Forms.Padding(2);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(796, 23);
            this.textMessage.TabIndex = 0;
            this.textMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textMessage_KeyDown);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(810, 478);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(33, 23);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = ">";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // timerCheckOnline
            // 
            this.timerCheckOnline.Enabled = true;
            this.timerCheckOnline.Interval = 3000;
            this.timerCheckOnline.Tick += new System.EventHandler(this.timerCheckOnline_Tick);
            // 
            // chatBox
            // 
            this.chatBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBox.Location = new System.Drawing.Point(10, 53);
            this.chatBox.Margin = new System.Windows.Forms.Padding(2);
            this.chatBox.MinimumSize = new System.Drawing.Size(15, 16);
            this.chatBox.Name = "chatBox";
            this.chatBox.Size = new System.Drawing.Size(614, 420);
            this.chatBox.TabIndex = 6;
            // 
            // listBoxUsers
            // 
            this.listBoxUsers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxUsers.FormattingEnabled = true;
            this.listBoxUsers.Location = new System.Drawing.Point(629, 61);
            this.listBoxUsers.Name = "listBoxUsers";
            this.listBoxUsers.Size = new System.Drawing.Size(214, 394);
            this.listBoxUsers.TabIndex = 7;
            this.listBoxUsers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxUsers_DrawItem);
            // 
            // FormChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(855, 510);
            this.Controls.Add(this.listBoxUsers);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.labelRoomName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormChatRoom";
            this.Text = "Eliza";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormChat_FormClosing);
            this.Load += new System.EventHandler(this.FormChat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelRoomName;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Timer timerCheckOnline;
        private System.Windows.Forms.WebBrowser chatBox;
        private System.Windows.Forms.ListBox listBoxUsers;
    }
}