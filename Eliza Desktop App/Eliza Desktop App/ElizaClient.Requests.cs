using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Eliza_Desktop_App
{
    public partial class ElizaClient
    {
        private void UnexpectedError(ElizaStatus status)
        {
            MessageDialogs.Error(
                string.Format("An unexpected error occurred: {0}.\nThe program will now terminate.",
                status.ToString()));
            this.Close();
        }

        public ElizaStatus Login(string username, string password)
        {
            this.SendRequest(string.Format("login {0} {1}", username, password));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS &&
                status != ElizaStatus.STATUS_INVALID_CREDENTIALS &&
                status != ElizaStatus.STATUS_ALREADY_LOGGED_IN)
            {
                UnexpectedError(status);
            }

            return status;
        }

        public void Logout()
        {
            this.SendRequest("logout");
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(status);
            }
        }

        public bool IsUsernameAvailable(string username)
        {
            this.SendRequest(string.Format("queryuserexists {0}", username));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status == ElizaStatus.QUERYRESPONSE_TRUE)
            {
                return false;
            }
            else if (status == ElizaStatus.QUERYRESPONSE_FALSE)
            {
                return true;
            }

            UnexpectedError(status);
            return false;
        }

        public bool IsUserOnline(string username)
        {
            this.SendRequest(string.Format("queryonline {0}", username));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status == ElizaStatus.QUERYRESPONSE_TRUE)
            {
                return true;
            }
            else if (status == ElizaStatus.QUERYRESPONSE_FALSE)
            {
                return false;
            }

            UnexpectedError(status);
            return false;
        }

        public ElizaStatus Register(string username, string password)
        {
            this.SendRequest(string.Format("register {0} {1}", username, password));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS &&
                status != ElizaStatus.STATUS_USERNAME_TOO_LONG &&
                status != ElizaStatus.STATUS_USERNAME_TOO_SHORT &&
                status != ElizaStatus.STATUS_PASSWORD_TOO_LONG &&
                status != ElizaStatus.STATUS_PASSWORD_TOO_SHORT &&
                status != ElizaStatus.STATUS_USERNAME_NON_ALPHANUMERIC &&
                status != ElizaStatus.STATUS_USERNAME_ALREADY_EXISTS)
            {
                UnexpectedError(status);
            }

            return status;
        }

        public ElizaStatus SendMessage(string username, string message)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
            this.SendRequest(string.Format("sendmsg {0} {1} {2}", timestamp, username, message));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS &&
                status != ElizaStatus.STATUS_SENDER_BLOCKED &&
                status != ElizaStatus.STATUS_RECEIVER_BLOCKED &&
                status != ElizaStatus.STATUS_USER_NOT_ONLINE)
            {
                UnexpectedError(status);
            }

            return status;
        }

        public string GetMessages(string username1, string username2)
        {
            this.SendRequest(string.Format("getmessages {0} {1}", username1, username2));
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(response.Status);
            }
            return response.Message;
        }

        public ElizaStatus SendSong(string username, string song)
        {
            this.SendRequest(string.Format("sendsong {0} {1}", username, song));
            ElizaStatus status = this.ReceiveResponse().Status;
            if (status != ElizaStatus.STATUS_SUCCESS &&
                status != ElizaStatus.STATUS_SENDER_BLOCKED &&
                status != ElizaStatus.STATUS_RECEIVER_BLOCKED &&
                status != ElizaStatus.STATUS_USER_NOT_ONLINE &&
                status != ElizaStatus.STATUS_INVALID_SONG)
            {
                UnexpectedError(status);
            }

            return status;
        }

        public string GetDescription(string username)
        {
            this.SendRequest(string.Format("querydescription {0}", username));
            ClientResponse response = this.ReceiveResponse();

            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(response.Status);
                return "";
            }

            if (response.Message == "None\n")
            {
                return "";
            }
            return response.Message;
        }

        public void SetDescription(string description)
        {
            this.SendRequest(string.Format("setdescription {0}", description));
            ElizaStatus status = this.ReceiveResponse().Status;
            if (status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(status);
            }
        }

        public Image GetProfilePicture(string username)
        {
            this.SendRequest(string.Format("queryprofilepic {0}", username));
            ClientResponse response = this.ReceiveResponse();
            if (response.Status == ElizaStatus.STATUS_SUCCESS)
            {
                int imageLength = int.Parse(response.Message);
                byte[] imageData = this.ReceiveFile(imageLength, null);
                if (imageData == null)
                {
                    return null;
                }
                using (var ms = new MemoryStream(imageData))
                {
                    return Image.FromStream(ms);
                }
            }
            else
            {
                UnexpectedError(response.Status);
                return null;
            }
        }

        public void SetProfilePicture(Image profilePicture)
        {
            byte[] fileData;
            using (MemoryStream ms = new MemoryStream())
            {
                profilePicture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                fileData = ms.ToArray();
            }
            ClientResponse response = this.SendFile(fileData);
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(response.Status);
            }
            this.SendRequest("setprofilepic");
            response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(response.Status);
            }
        }

        public ElizaStatus SendFriendRequest(string username)
        {
            this.SendRequest(string.Format("friendrequest {0}", username));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS &&
                status != ElizaStatus.STATUS_FRIEND_REQUEST_ALREADY_SENT &&
                status != ElizaStatus.STATUS_FRIEND_REQUEST_ALREADY_RECEIVED &&
                status != ElizaStatus.STATUS_ALREADY_FRIENDS &&
                status != ElizaStatus.STATUS_NON_EXISTENT_USER)
            {
                UnexpectedError(status);
            }

            return status;
        }

        public void AcceptFriendRequest(string username)
        {
            this.SendRequest(string.Format("acceptfriendrequest {0}", username));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(status);
            }
        }

        public void DeclineFriendRequest(string username)
        {
            this.SendRequest(string.Format("declinefriendrequest {0}", username));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(status);
            }
        }

        public Dictionary<string, bool> GetFriends()
        {
            this.SendRequest("queryfriends");
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(response.Status);
            }
            string[] friends = response.Message.Split(new char[] { '\n' });
            Dictionary<string, bool> friendList = new Dictionary<string, bool>();
            foreach (string s in friends)
            {
                int spaceIndex = s.IndexOf(' ');
                if (spaceIndex > 0)
                {
                    friendList.Add(s.Substring(0, spaceIndex), s.Substring(spaceIndex + 1).StartsWith("1"));
                }
            }
            return friendList;
        }

        public List<string> GetFriendRequests()
        {
            this.SendRequest("queryfriendrequests");
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                UnexpectedError(response.Status);
            }
            string[] friends = response.Message.Split(new char[] { '\n' });
            List<string> friendRequests = new List<string>();
            foreach (string s in friends)
            {
                if (s != "")
                {
                    friendRequests.Add(s);
                }
            }
            return friendRequests;
        }

        public List<string> GetRooms()
        {
            this.SendRequest("getrooms");
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error(response.Status.ToString());
            }
            string[] rooms = response.Message.Split(new char[] { '\n' });
            List<string> roomList = new List<string>();
            foreach (string s in rooms)
            {
                if (s != "")
                {
                    roomList.Add(s);
                }
            }
            return roomList;
        }

        public ElizaStatus CreateRoom(string roomName, bool isPublic)
        {
            this.SendRequest(string.Format("createroom {0} {1}", roomName, isPublic ? "1" : "0"));
            ElizaStatus status = this.ReceiveResponse().Status;
            return status;
        }

        public string GetRoomMessages(string roomName)
        {
            this.SendRequest(string.Format("getroommessages {0}", roomName));
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error(response.Status.ToString());
            }
            return response.Message;
        }

        public ElizaStatus BroadcastMessage(string roomName, string message)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
            this.SendRequest(string.Format("broadcastmsg {0} {1} {2}", timestamp, roomName, message));
            ElizaStatus status = this.ReceiveResponse().Status;

            if (status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error(status.ToString());
            }

            return status;
        }

        public Dictionary<string, bool> GetRoomMembers(string roomName)
        {
            this.SendRequest(string.Format("getroommembers {0}", roomName));
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error(response.Status.ToString());
            }
            string[] membersArray = response.Message.Split(new char[] { '\r', '\n' });
            Dictionary<string, bool> ret = new Dictionary<string, bool>();
            foreach (string member in membersArray)
            {
                string[] memberDetails = member.Split(new char[] { ' ' });
                if (memberDetails.Length < 2)
                {
                    continue;
                }
                ret.Add(memberDetails[0], memberDetails[1] == "1");
            }
            return ret;
        }

        public bool CheckOwnership(string roomName)
        {
            this.SendRequest(string.Format("doiownthis {0}", roomName));
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error(response.Status.ToString());
            }
            if (response.Message.Trim() == "yes")
            {
                return true;
            }
            return false;
        }

        public ElizaStatus AddToRoom(string username, string roomName)
        {
            this.SendRequest(string.Format("addtoroom {0} {1}", roomName, username));
            ElizaStatus status = this.ReceiveResponse().Status;
            return status;
        }

        public ElizaStatus Kick(string username, string roomName)
        {
            this.SendRequest(string.Format("kickfromroom {0} {1}", roomName, username));
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error(response.Status.ToString());
            }
            return response.Status;
        }

        public ElizaStatus DeleteRoom(string roomName)
        {
            this.SendRequest(string.Format("deleteroom {0}", roomName));
            ClientResponse response = this.ReceiveResponse();
            if (response.Status != ElizaStatus.STATUS_SUCCESS)
            {
                MessageDialogs.Error(response.Status.ToString());
            }
            return response.Status;
        }

        public ElizaStatus TransferOwnership(string roomName, string newOwner)
        {
            this.SendRequest(string.Format("transferroomownership {0} {1}", roomName, newOwner));
            ClientResponse response = this.ReceiveResponse();
            return response.Status;
        }
    }
}
