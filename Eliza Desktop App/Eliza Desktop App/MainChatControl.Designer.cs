﻿namespace Eliza_Desktop_App
{
    partial class MainChatControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.optionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openChatMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.addFriendMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.createRoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.signOutMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.friendRequestsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.textDescription = new System.Windows.Forms.TextBox();
            this.buttonSaveDescription = new System.Windows.Forms.Button();
            this.buttonDiscardDescription = new System.Windows.Forms.Button();
            this.listViewFriends = new System.Windows.Forms.ListView();
            this.columnHeaderUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOnline = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.pictureProfile = new System.Windows.Forms.PictureBox();
            this.listBoxRooms = new System.Windows.Forms.ListBox();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserName.Location = new System.Drawing.Point(92, 28);
            this.labelUserName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(151, 31);
            this.labelUserName.TabIndex = 1;
            this.labelUserName.Text = "User Name";
            // 
            // labelDescription
            // 
            this.labelDescription.ForeColor = System.Drawing.Color.Gray;
            this.labelDescription.Location = new System.Drawing.Point(94, 64);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(274, 50);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Click to add description...";
            this.labelDescription.Click += new System.EventHandler(this.labelDescription_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.BackColor = System.Drawing.Color.Transparent;
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsMenu,
            this.friendRequestsMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(380, 24);
            this.mainMenu.TabIndex = 3;
            this.mainMenu.Text = "menuStrip1";
            // 
            // optionsMenu
            // 
            this.optionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openChatMenuButton,
            this.addFriendMenuButton,
            this.createRoomToolStripMenuItem,
            this.signOutMenuButton,
            this.exitMenuButton});
            this.optionsMenu.Name = "optionsMenu";
            this.optionsMenu.Size = new System.Drawing.Size(61, 20);
            this.optionsMenu.Text = "Options";
            // 
            // openChatMenuButton
            // 
            this.openChatMenuButton.Name = "openChatMenuButton";
            this.openChatMenuButton.Size = new System.Drawing.Size(178, 22);
            this.openChatMenuButton.Text = "Open Chat Window";
            this.openChatMenuButton.Click += new System.EventHandler(this.openChatMenuButton_Click);
            // 
            // addFriendMenuButton
            // 
            this.addFriendMenuButton.Name = "addFriendMenuButton";
            this.addFriendMenuButton.Size = new System.Drawing.Size(178, 22);
            this.addFriendMenuButton.Text = "Add Friend";
            this.addFriendMenuButton.Click += new System.EventHandler(this.addFriendMenuButton_Click);
            // 
            // createRoomToolStripMenuItem
            // 
            this.createRoomToolStripMenuItem.Name = "createRoomToolStripMenuItem";
            this.createRoomToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.createRoomToolStripMenuItem.Text = "Create Room";
            this.createRoomToolStripMenuItem.Click += new System.EventHandler(this.createRoomToolStripMenuItem_Click);
            // 
            // signOutMenuButton
            // 
            this.signOutMenuButton.Name = "signOutMenuButton";
            this.signOutMenuButton.Size = new System.Drawing.Size(178, 22);
            this.signOutMenuButton.Text = "Sign Out";
            this.signOutMenuButton.Click += new System.EventHandler(this.signOutMenuButton_Click);
            // 
            // exitMenuButton
            // 
            this.exitMenuButton.Name = "exitMenuButton";
            this.exitMenuButton.Size = new System.Drawing.Size(178, 22);
            this.exitMenuButton.Text = "Exit";
            this.exitMenuButton.Click += new System.EventHandler(this.exitMenuButton_Click);
            // 
            // friendRequestsMenu
            // 
            this.friendRequestsMenu.Name = "friendRequestsMenu";
            this.friendRequestsMenu.Size = new System.Drawing.Size(102, 20);
            this.friendRequestsMenu.Text = "Friend Requests";
            this.friendRequestsMenu.Visible = false;
            // 
            // textDescription
            // 
            this.textDescription.Location = new System.Drawing.Point(94, 63);
            this.textDescription.Margin = new System.Windows.Forms.Padding(2);
            this.textDescription.Multiline = true;
            this.textDescription.Name = "textDescription";
            this.textDescription.Size = new System.Drawing.Size(276, 52);
            this.textDescription.TabIndex = 4;
            this.textDescription.Visible = false;
            // 
            // buttonSaveDescription
            // 
            this.buttonSaveDescription.Location = new System.Drawing.Point(312, 119);
            this.buttonSaveDescription.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveDescription.Name = "buttonSaveDescription";
            this.buttonSaveDescription.Size = new System.Drawing.Size(56, 22);
            this.buttonSaveDescription.TabIndex = 5;
            this.buttonSaveDescription.Text = "Save";
            this.buttonSaveDescription.UseVisualStyleBackColor = true;
            this.buttonSaveDescription.Visible = false;
            this.buttonSaveDescription.Click += new System.EventHandler(this.buttonSaveDescription_Click);
            // 
            // buttonDiscardDescription
            // 
            this.buttonDiscardDescription.Location = new System.Drawing.Point(251, 119);
            this.buttonDiscardDescription.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDiscardDescription.Name = "buttonDiscardDescription";
            this.buttonDiscardDescription.Size = new System.Drawing.Size(56, 22);
            this.buttonDiscardDescription.TabIndex = 6;
            this.buttonDiscardDescription.Text = "Discard";
            this.buttonDiscardDescription.UseVisualStyleBackColor = true;
            this.buttonDiscardDescription.Visible = false;
            this.buttonDiscardDescription.Click += new System.EventHandler(this.buttonDiscardDescription_Click);
            // 
            // listViewFriends
            // 
            this.listViewFriends.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUsername,
            this.columnHeaderOnline});
            this.listViewFriends.Location = new System.Drawing.Point(12, 119);
            this.listViewFriends.Margin = new System.Windows.Forms.Padding(2);
            this.listViewFriends.MultiSelect = false;
            this.listViewFriends.Name = "listViewFriends";
            this.listViewFriends.Size = new System.Drawing.Size(357, 205);
            this.listViewFriends.TabIndex = 7;
            this.listViewFriends.UseCompatibleStateImageBehavior = false;
            this.listViewFriends.View = System.Windows.Forms.View.Details;
            this.listViewFriends.DoubleClick += new System.EventHandler(this.listViewFriends_DoubleClick);
            // 
            // columnHeaderUsername
            // 
            this.columnHeaderUsername.Text = "User Name";
            this.columnHeaderUsername.Width = 150;
            // 
            // columnHeaderOnline
            // 
            this.columnHeaderOnline.Text = "Online";
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 10000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // pictureProfile
            // 
            this.pictureProfile.BackColor = System.Drawing.Color.White;
            this.pictureProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureProfile.Image = global::Eliza_Desktop_App.Properties.Resources.default_profile_pic;
            this.pictureProfile.Location = new System.Drawing.Point(12, 31);
            this.pictureProfile.Margin = new System.Windows.Forms.Padding(2);
            this.pictureProfile.Name = "pictureProfile";
            this.pictureProfile.Size = new System.Drawing.Size(76, 82);
            this.pictureProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureProfile.TabIndex = 0;
            this.pictureProfile.TabStop = false;
            this.pictureProfile.DoubleClick += new System.EventHandler(this.pictureProfile_DoubleClick);
            // 
            // listBoxRooms
            // 
            this.listBoxRooms.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxRooms.FormattingEnabled = true;
            this.listBoxRooms.ItemHeight = 18;
            this.listBoxRooms.Location = new System.Drawing.Point(13, 335);
            this.listBoxRooms.Name = "listBoxRooms";
            this.listBoxRooms.Size = new System.Drawing.Size(357, 202);
            this.listBoxRooms.TabIndex = 8;
            this.listBoxRooms.DoubleClick += new System.EventHandler(this.listBoxRooms_DoubleClick);
            // 
            // MainChatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxRooms);
            this.Controls.Add(this.buttonDiscardDescription);
            this.Controls.Add(this.buttonSaveDescription);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.pictureProfile);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.textDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.listViewFriends);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainChatControl";
            this.Size = new System.Drawing.Size(380, 555);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureProfile;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem optionsMenu;
        private System.Windows.Forms.ToolStripMenuItem signOutMenuButton;
        private System.Windows.Forms.ToolStripMenuItem exitMenuButton;
        private System.Windows.Forms.TextBox textDescription;
        private System.Windows.Forms.Button buttonSaveDescription;
        private System.Windows.Forms.Button buttonDiscardDescription;
        private System.Windows.Forms.ListView listViewFriends;
        private System.Windows.Forms.ColumnHeader columnHeaderUsername;
        private System.Windows.Forms.ColumnHeader columnHeaderOnline;
        private System.Windows.Forms.ToolStripMenuItem addFriendMenuButton;
        private System.Windows.Forms.ToolStripMenuItem friendRequestsMenu;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.ToolStripMenuItem openChatMenuButton;
        private System.Windows.Forms.ToolStripMenuItem createRoomToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxRooms;
    }
}
