using System.Diagnostics;

namespace Eliza_Desktop_App
{
    public class ElizaClient : Process
    {
        public ElizaClient() : base()
        {
            StartInfo.FileName = "python";
            StartInfo.Arguments = @"d:\Projects\Eliza\elizaclient.py";
            StartInfo.UseShellExecute = false;
            StartInfo.RedirectStandardInput = true;
            StartInfo.RedirectStandardOutput = true;
            Start();
        }

        public void SendRequest(string msg)
        {
            StandardInput.Write(msg + "\n");
        }

        public int ReceiveResponse()
        {
            return StandardOutput.Read();
            /*char[] buffer = new char[1024];
            int len = StandardOutput.ReadBlock(buffer, 0, 1024);
            return new string(buffer, 0, len);*/
        }

        public new void Close()
        {
            SendRequest("exit");
            StandardInput.Close();
            //WaitForExit();
            Kill();
            base.Close();
        }
    }
}
