namespace Eliza_Desktop_App
{
    partial class FormRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegister));
            this.textUsername = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.buttonCheckAvailability = new System.Windows.Forms.Button();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.labelRepeatPassword = new System.Windows.Forms.Label();
            this.textRepeatPassword = new System.Windows.Forms.TextBox();
            this.buttonAddProfilePic = new System.Windows.Forms.Button();
            this.pictureProfile = new System.Windows.Forms.PictureBox();
            this.textDescription = new System.Windows.Forms.TextBox();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // textUsername
            // 
            this.textUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textUsername.Location = new System.Drawing.Point(173, 19);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(184, 27);
            this.textUsername.TabIndex = 0;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsername.Location = new System.Drawing.Point(12, 19);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(86, 20);
            this.labelUsername.TabIndex = 1;
            this.labelUsername.Text = "Username";
            // 
            // buttonCheckAvailability
            // 
            this.buttonCheckAvailability.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCheckAvailability.Location = new System.Drawing.Point(363, 19);
            this.buttonCheckAvailability.Name = "buttonCheckAvailability";
            this.buttonCheckAvailability.Size = new System.Drawing.Size(160, 27);
            this.buttonCheckAvailability.TabIndex = 2;
            this.buttonCheckAvailability.Text = "Check availability";
            this.buttonCheckAvailability.UseVisualStyleBackColor = true;
            this.buttonCheckAvailability.Click += new System.EventHandler(this.buttonCheckAvailability_Click);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword.Location = new System.Drawing.Point(12, 52);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(83, 20);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Password";
            // 
            // textPassword
            // 
            this.textPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textPassword.Location = new System.Drawing.Point(173, 52);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(184, 27);
            this.textPassword.TabIndex = 3;
            this.textPassword.UseSystemPasswordChar = true;
            // 
            // labelRepeatPassword
            // 
            this.labelRepeatPassword.AutoSize = true;
            this.labelRepeatPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRepeatPassword.Location = new System.Drawing.Point(12, 85);
            this.labelRepeatPassword.Name = "labelRepeatPassword";
            this.labelRepeatPassword.Size = new System.Drawing.Size(141, 20);
            this.labelRepeatPassword.TabIndex = 6;
            this.labelRepeatPassword.Text = "Repeat Password";
            // 
            // textRepeatPassword
            // 
            this.textRepeatPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textRepeatPassword.Location = new System.Drawing.Point(173, 85);
            this.textRepeatPassword.Name = "textRepeatPassword";
            this.textRepeatPassword.Size = new System.Drawing.Size(184, 27);
            this.textRepeatPassword.TabIndex = 5;
            this.textRepeatPassword.UseSystemPasswordChar = true;
            // 
            // buttonAddProfilePic
            // 
            this.buttonAddProfilePic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddProfilePic.Location = new System.Drawing.Point(122, 214);
            this.buttonAddProfilePic.Name = "buttonAddProfilePic";
            this.buttonAddProfilePic.Size = new System.Drawing.Size(211, 30);
            this.buttonAddProfilePic.TabIndex = 7;
            this.buttonAddProfilePic.Text = "Add Profile Picture (optional)";
            this.buttonAddProfilePic.UseVisualStyleBackColor = true;
            // 
            // pictureProfile
            // 
            this.pictureProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureProfile.Image = global::Eliza_Desktop_App.Properties.Resources.default_profile_pic;
            this.pictureProfile.Location = new System.Drawing.Point(16, 144);
            this.pictureProfile.Name = "pictureProfile";
            this.pictureProfile.Size = new System.Drawing.Size(100, 100);
            this.pictureProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureProfile.TabIndex = 8;
            this.pictureProfile.TabStop = false;
            // 
            // textDescription
            // 
            this.textDescription.Location = new System.Drawing.Point(122, 144);
            this.textDescription.Multiline = true;
            this.textDescription.Name = "textDescription";
            this.textDescription.Size = new System.Drawing.Size(401, 64);
            this.textDescription.TabIndex = 9;
            // 
            // buttonRegister
            // 
            this.buttonRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRegister.Location = new System.Drawing.Point(324, 263);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(199, 32);
            this.buttonRegister.TabIndex = 10;
            this.buttonRegister.Text = "Complete Registration";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(119, 124);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(147, 17);
            this.labelDescription.TabIndex = 11;
            this.labelDescription.Text = "Description (optional):";
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(535, 307);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.buttonRegister);
            this.Controls.Add(this.textDescription);
            this.Controls.Add(this.pictureProfile);
            this.Controls.Add(this.buttonAddProfilePic);
            this.Controls.Add(this.labelRepeatPassword);
            this.Controls.Add(this.textRepeatPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.buttonCheckAvailability);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.textUsername);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormRegister";
            this.Text = "Register User";
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textUsername;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Button buttonCheckAvailability;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label labelRepeatPassword;
        private System.Windows.Forms.TextBox textRepeatPassword;
        private System.Windows.Forms.Button buttonAddProfilePic;
        private System.Windows.Forms.PictureBox pictureProfile;
        private System.Windows.Forms.TextBox textDescription;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.Label labelDescription;
    }
}