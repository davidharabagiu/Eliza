using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eliza_Desktop_App
{
    class Message
    {
        public int Id;
        public string Timestamp;
        public string Username;
        public string Content;

        public Message(int id, string timestamp, string username, string content)
        {
            Id = id;
            Timestamp = timestamp;
            Username = username;
            Content = content;
        }
    }
}
