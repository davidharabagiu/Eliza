using System.Diagnostics;
using System.IO.Pipes;

namespace Eliza_Desktop_App
{
    public class ElizaClient : Process
    {
        private NamedPipeServerStream pipeServerStream;
        private StringStream ioStream;

        public ElizaClient() : base()
        {
            pipeServerStream = new NamedPipeServerStream("elizapipe", PipeDirection.In);
            StartInfo.FileName = "pythonw";
            StartInfo.Arguments = @"d:\Projects\Eliza\elizaclient.py";
            StartInfo.UseShellExecute = false;
            Start();
            pipeServerStream.WaitForConnection();
        }

        public void SendRequest(string msg)
        {
            ioStream.Write(msg);
        }

        public string ReceiveResponse()
        {
            return ioStream.Read();
        }

        public new void Close()
        {
            SendRequest("exit");
            StandardInput.Close();
            Kill();
            base.Close();
        }
    }
}
