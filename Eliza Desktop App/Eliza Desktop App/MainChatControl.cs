using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Eliza_Desktop_App
{
    public partial class MainChatControl : UserControl
    {
        public ElizaClient ClientProcess { get; set; }
        private string userName;
        private bool emptyDescription;
        public delegate void LogOutPressedEventHandler();
        public event LogOutPressedEventHandler LogOutPressed;

        public MainChatControl()
        {
            InitializeComponent();
            emptyDescription = true;
        }

        public void SetUser(string userName)
        {
            this.userName = userName;
            labelUserName.Text = userName;

            string description = ClientProcess.GetDescription(userName);
            if (description.Length > 0)
            {
                labelDescription.Text = description.Substring(0, description.Length - 1);
                emptyDescription = false;
            }

            Image profilePicture = ClientProcess.GetProfilePicture(userName);
            if (profilePicture != null)
            {
                pictureProfile.Image = profilePicture;
            }

            UpdateFriendList();
            UpdateFriendRequestsMenu();

            timerRefresh.Start();
        }

        private void UpdateFriendList()
        {
            listViewFriends.Items.Clear();
            Dictionary<string, bool> friendsList = ClientProcess.GetFriends();
            foreach (string k in friendsList.Keys)
            {
                ListViewItem friendItem = new ListViewItem(new string[] { k, friendsList[k] ? "Yes" : "No" });
                listViewFriends.Items.Add(friendItem);
            }
        }

        private void UpdateFriendRequestsMenu()
        {
            friendRequestsMenu.DropDownItems.Clear();
            List<string> friendRequests = ClientProcess.GetFriendRequests();
            if (friendRequests.Count > 0)
            {
                foreach (string f in friendRequests)
                {
                    ToolStripMenuItem fItem = new ToolStripMenuItem(f);
                    fItem.Click += FriendRequestMenuItem_Click;
                    friendRequestsMenu.DropDownItems.Add(fItem);
                }
                friendRequestsMenu.Visible = true;
            }
            else
            {
                friendRequestsMenu.Visible = false;
            }
        }

        private void FriendRequestMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (MessageDialogs.YesNo(string.Format("Accept {0} as friend?", menuItem.Text)))
            {
                ClientProcess.AcceptFriendRequest(menuItem.Text);
            }
            else
            {
                ClientProcess.DeclineFriendRequest(menuItem.Text);
            }

            UpdateFriendList();
            UpdateFriendRequestsMenu();
        }

        private void signOutMenuButton_Click(object sender, EventArgs e)
        {
            timerRefresh.Stop();
            ClientProcess.Logout();
            LogOutPressed();
            pictureProfile.Image = Eliza_Desktop_App.Properties.Resources.default_profile_pic;
            labelDescription.Text = "Click to add description...";
            emptyDescription = true;
            listViewFriends.Items.Clear();
        }

        private void labelDescription_Click(object sender, EventArgs e)
        {
            if (emptyDescription)
            {
                textDescription.Text = "";
            }
            else
            {
                textDescription.Text = labelDescription.Text;
            }
            labelDescription.Visible = false;
            textDescription.Visible = true;
            buttonSaveDescription.Visible = true;
            buttonDiscardDescription.Visible = true;
        }

        private void buttonSaveDescription_Click(object sender, EventArgs e)
        {
            if (textDescription.Text.Length > 1000)
            {
                MessageDialogs.Error("Description too long.");
                return;
            }
            ClientProcess.SetDescription(textDescription.Text);
            labelDescription.Text = textDescription.Text;

            labelDescription.Visible = true;
            textDescription.Visible = false;
            buttonSaveDescription.Visible = false;
            buttonDiscardDescription.Visible = false;
            textDescription.Text = "";
        }

        private void buttonDiscardDescription_Click(object sender, EventArgs e)
        {

            labelDescription.Visible = true;
            textDescription.Visible = false;
            buttonSaveDescription.Visible = false;
            buttonDiscardDescription.Visible = false;
            textDescription.Text = "";
        }

        private void pictureProfile_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Change Profile Picture";
            dialog.FileName = "";
            dialog.Filter = "JPEG files(*.jpg;*.jpeg)|*.jpg;*.jpeg";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Image newProfilePic = new Bitmap(Image.FromFile(dialog.FileName), new Size(100, 100));
                ClientProcess.SetProfilePicture(newProfilePic);
                pictureProfile.Image = newProfilePic;
            }
        }

        private void addFriendMenuButton_Click(object sender, EventArgs e)
        {
            SendFriendRequestDialog dlg = new SendFriendRequestDialog();
            if (dlg.ShowDialog() != DialogResult.OK || dlg.UserName.Length == 0)
            {
                return;
            }

            ElizaStatus status = ClientProcess.SendFriendRequest(dlg.UserName);
            switch (status)
            {
                case ElizaStatus.STATUS_SUCCESS:
                    MessageDialogs.Info("Friend request sent succesfully.");
                    break;

                case ElizaStatus.STATUS_NON_EXISTENT_USER:
                    MessageDialogs.Warning("This user does not exist.");
                    break;

                case ElizaStatus.STATUS_ALREADY_FRIENDS:
                    MessageDialogs.Warning("You are already friends with this user.");
                    break;

                case ElizaStatus.STATUS_FRIEND_REQUEST_ALREADY_SENT:
                    MessageDialogs.Warning("You already sent a friend request to this user.");
                    break;

                case ElizaStatus.STATUS_FRIEND_REQUEST_ALREADY_RECEIVED:
                    ClientProcess.AcceptFriendRequest(dlg.UserName);
                    UpdateFriendList();
                    UpdateFriendRequestsMenu();
                    break;
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            UpdateFriendList();
            UpdateFriendRequestsMenu();
        }

        private void listViewFriends_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFriends.SelectedItems.Count > 0)
            {
                FormChat chat = new FormChat();
                chat.Setup(ClientProcess, listViewFriends.SelectedItems[0].Text);
                chat.Show();
            }
        }
    }
}
