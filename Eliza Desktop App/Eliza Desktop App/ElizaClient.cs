using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;

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

    public class ElizaClient : Process
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

        public void SendRequest(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            writer.Write((uint)buffer.Length);
            writer.Write(buffer);
        }

        public string ReceiveResponse()
        {
            int len = (int)reader.ReadUInt32();
            return new string(reader.ReadChars(len));
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
