using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Collections.Generic;
using System;
using System.Threading;

namespace Eliza_Desktop_App
{
    public enum ElizaStatus
    {
        STATUS_SUCCESS = 0,
        STATUS_INVALID_REQUEST_PARAMETERS = 1,
        STATUS_NOT_LOGGED_IN = 2,
        STATUS_UNKNOWN_REQUEST = 3,
        STATUS_DATABASE_ERROR = 4,
        STATUS_USERNAME_TOO_SHORT = 5,
        STATUS_USERNAME_TOO_LONG = 6,
        STATUS_PASSWORD_TOO_SHORT = 7,
        STATUS_PASSWORD_TOO_LONG = 8,
        STATUS_USERNAME_ALREADY_EXISTS = 9,
        STATUS_USERNAME_NON_ALPHANUMERIC = 10,
        STATUS_ALREADY_LOGGED_IN = 11,
        STATUS_INVALID_CREDENTIALS = 12,
        STATUS_USER_NOT_ONLINE = 13,
        STATUS_ERROR_EMPTY_MESSAGE = 14,
        STATUS_RECEIVER_BLOCKED = 15,
        STATUS_SENDER_BLOCKED = 16,
        STATUS_FRIEND_REQUEST_ALREADY_SENT = 17,
        STATUS_FRIEND_REQUEST_ALREADY_RECEIVED = 18,
        STATUS_ALREADY_FRIENDS = 19,
        STATUS_NON_EXISTENT_USER = 20,
        STATUS_NO_FRIEND_REQUEST = 21,
        STATUS_NO_FRIENDSHIP = 22,
        STATUS_ALREADY_BLOCKED = 23,
        STATUS_USER_NOT_BLOCKED = 24,
        STATUS_INVALID_PASSWORD = 25,
        STATUS_EMPTY_SONG_NAME = 26,
        STATUS_INVALID_SONG = 27,
        STATUS_NOT_ALLOWED = 28,
        QUERYRESPONSE_FALSE = 100,
        QUERYRESPONSE_TRUE = 101
    }

    public struct ClientResponse
    {
        public ElizaStatus Status { get; private set; }
        public string Message { get; private set; }

        public ClientResponse(string responseMessage)
        {
            int messageIndex = responseMessage.IndexOf('\n');
            if (messageIndex == -1)
            {
                Status = (ElizaStatus)int.Parse(responseMessage);
                Message = string.Empty;
            }
            else
            {
                Status = (ElizaStatus)int.Parse(responseMessage.Substring(0, messageIndex));
                if (responseMessage.Length > messageIndex + 1)
                {
                    Message = responseMessage.Substring(messageIndex + 1);
                }
                else
                {
                    Message = string.Empty;
                }
            }
        }
    }

    public partial class ElizaClient : Process
    {
        private NamedPipeServerStream pipeServerStream;
        private NamedPipeServerStream pipeMessagesServerStream;
        private Thread inboundMessagesThread;
        private BinaryReader reader;
        private BinaryWriter writer;
        private WMPLib.WindowsMediaPlayer musicPlayer;

        public delegate void MessageReceivedEventHandler(string username, string message);
        public delegate void BroadcastMessageReceivedEventHandler(int id, string timestamp, string roomName, string username, string message);
        public delegate void ReplyReceivedEventHandler(int replyid, int msgid, string timestamp, string roomName, string username, string message);
        public event MessageReceivedEventHandler MessageReceived;
        public event BroadcastMessageReceivedEventHandler BroadcastMessageReceived;
        public event ReplyReceivedEventHandler ReplyReceived;
        public delegate void CommunicationErrorEventHandler();
        public event CommunicationErrorEventHandler CommunicationError;

        public ElizaClient() : base()
        {
            int instanceId = new Random().Next(10000);

            pipeServerStream = new NamedPipeServerStream("elizapipe" + instanceId.ToString(), PipeDirection.InOut);
            pipeMessagesServerStream = new NamedPipeServerStream("elizapipemsg" + instanceId.ToString(), PipeDirection.InOut);

            StartInfo.FileName = Program.DebugMode ? "python" : "pythonw";
            StartInfo.Arguments = Program.ElizaClientScriptPath + " " + instanceId.ToString();
            StartInfo.UseShellExecute = Program.DebugMode;
            Start();

            pipeServerStream.WaitForConnection();
            reader = new BinaryReader(pipeServerStream);
            writer = new BinaryWriter(pipeServerStream);

            musicPlayer = new WMPLib.WindowsMediaPlayer();
            musicPlayer.URL = "song.mp3";
            musicPlayer.controls.stop();

            inboundMessagesThread = new Thread(new ThreadStart(ListenToInboundMessages));
            inboundMessagesThread.Start();
        }

        private void ListenToInboundMessages()
        {
            pipeMessagesServerStream.WaitForConnection();
            BinaryReader msgReader = new BinaryReader(pipeMessagesServerStream);

            try
            {
                while(true)
                {
                    int len = (int)msgReader.ReadUInt32();
                    string data = new string(msgReader.ReadChars(len));

                    if (data.StartsWith("/song"))
                    {
                        ReceiveSong(msgReader);
                        continue;
                    }

                    int separatorIndex = data.IndexOf(':');
                    if (separatorIndex > 0)
                    {
                        string username = data.Substring(0, separatorIndex);
                        string message = data.Substring(separatorIndex + 1);

                        string[] msgdata = username.Split(new char[] { '#' });
                        if (msgdata.Length >= 4)
                        {
                            string timestamp = msgdata[1];
                            string roomName = msgdata[2];
                            username = msgdata[3];

                            string[] iddata = msgdata[0].Split(new char[] { '/' });

                            if (iddata.Length == 2)
                            {
                                int msgid = Convert.ToInt32(iddata[1]);
                                int replyid = Convert.ToInt32(iddata[0]);
                                ReplyReceived(replyid, msgid, timestamp, roomName, username, message);
                            }
                            else
                            {
                                int msgid = Convert.ToInt32(msgdata[0]);
                                BroadcastMessageReceived(msgid, timestamp, roomName, username, message);
                            }
                        }
                        else
                        {
                            MessageReceived(username, message);
                        }
                    }
                    else
                    {
                        MessageDialogs.Error(string.Format("Invalid message received: {0}.", data));
                    }
                }
            }
            catch
            {
                // pipeServerStream was closed, do nothing
            }
        }

        private void ReceiveSong(BinaryReader msgReader)
        {
            int len = (int)msgReader.ReadUInt32();
            int songLength = int.Parse(new string(msgReader.ReadChars(len)));
            //System.Windows.Forms.MessageBox.Show(songLength.ToString());
            byte[] songData = ReceiveFile(songLength, msgReader);
            musicPlayer.controls.stop();
            string songName = "song" + new Random().Next(10000).ToString() + ".mp3";
            File.WriteAllBytes(songName, songData);
            musicPlayer.URL = songName;
            musicPlayer.controls.play();
        }

        private void SendRequest(string request)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(request);
                writer.Write((uint)buffer.Length);
                writer.Write(buffer);
            }
            catch (IOException ex)
            {
                MessageDialogs.Error(string.Format(
                    "The client has encountered an unexpected problem and was forced to exit.\n" +
                    "Error message: {0}\nSee the log for more details.", ex.Message));
                pipeServerStream.Close();
                CommunicationError();
            }
        }

        private ClientResponse ReceiveResponse()
        {
            try
            {
                int len = (int)reader.ReadUInt32();
                return new ClientResponse(new string(reader.ReadChars(len)));
            }
            catch (IOException ex)
            {
                MessageDialogs.Error(string.Format(
                    "The client has encountered an unexpected problem and was forced to exit.\n" +
                    "Error message: {0}\nSee the log for more details.", ex.Message));
                pipeServerStream.Close();
                CommunicationError();
                return new ClientResponse();
            }
        }

        private byte[] ReceiveFile(int fileLength, BinaryReader binaryReader)
        {
            try
            {
                BinaryReader reader = (binaryReader == null) ? this.reader : binaryReader;

                int bytesReceived = 0;
                int len = 0;

                List<char> base64FileData = new List<char>();
                while (bytesReceived < fileLength)
                {
                    len = (int)reader.ReadUInt32();
                    char[] buffer = reader.ReadChars(len);
                    base64FileData.AddRange(buffer);
                    bytesReceived += len;
                }
                string sBase64FileData = new string(base64FileData.ToArray());
                if (sBase64FileData == "None")
                {
                    return null;
                }
                return Convert.FromBase64String(sBase64FileData);
            }
            catch (IOException ex)
            {
                MessageDialogs.Error(string.Format(
                    "The client has encountered an unexpected problem and was forced to exit.\n" +
                    "Error message: {0}\nSee the log for more details.", ex.Message));
                pipeServerStream.Close();
                CommunicationError();
                return null;
            }
        }

        private ClientResponse SendFile(byte[] fileData)
        {
            try
            {
                char[] base64FileData = Convert.ToBase64String(fileData).ToCharArray();
                SendRequest(string.Format("filetransfer {0}", base64FileData.Length));

                ClientResponse response = ReceiveResponse();
                if (response.Status != ElizaStatus.STATUS_SUCCESS)
                {
                    return response;
                }

                int fileBytesSent = 0;
                while (fileBytesSent < base64FileData.Length)
                {
                    int fileBytesToSend = Math.Min(1024, base64FileData.Length - fileBytesSent);
                    writer.Write(fileBytesToSend);
                    writer.Write(base64FileData, fileBytesSent, fileBytesToSend);
                    fileBytesSent += 1024;

                    response = ReceiveResponse();
                    if (response.Status != ElizaStatus.STATUS_SUCCESS)
                    {
                        return response;
                    }
                }

                return response;
            }
            catch (IOException ex)
            {
                MessageDialogs.Error(string.Format(
                    "The client has encountered an unexpected problem and was forced to exit.\n" +
                    "Error message: {0}\nSee the log for more details.", ex.Message));
                pipeServerStream.Close();
                CommunicationError();
                return new ClientResponse();
            }
        }

        public new void Close()
        {
            SendRequest("exit");
            pipeServerStream.Close();
            Kill();
            base.Close();
        }
    }
}
