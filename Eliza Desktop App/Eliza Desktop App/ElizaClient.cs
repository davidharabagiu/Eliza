using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Collections.Generic;
using System;

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
        private BinaryReader reader;
        private BinaryWriter writer;

        public ElizaClient() : base()
        {
            pipeServerStream = new NamedPipeServerStream("elizapipe", PipeDirection.InOut);
            StartInfo.FileName = Program.DebugMode ? "python" : "pythonw";
            StartInfo.Arguments = @"..\..\..\..\elizaclient.py";
            StartInfo.UseShellExecute = Program.DebugMode;
            Start();
            pipeServerStream.WaitForConnection();
            reader = new BinaryReader(pipeServerStream);
            writer = new BinaryWriter(pipeServerStream);
        }

        private void SendRequest(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            writer.Write((uint)buffer.Length);
            writer.Write(buffer);
        }

        private ClientResponse ReceiveResponse()
        {
            int len = (int)reader.ReadUInt32();
            return new ClientResponse(new string(reader.ReadChars(len)));
        }

        private byte[] ReceiveFile(int fileLength)
        {
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

        private ClientResponse SendFile(byte[] fileData)
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

        public new void Close()
        {
            SendRequest("exit");
            pipeServerStream.Close();
            Kill();
            base.Close();
        }
    }
}
