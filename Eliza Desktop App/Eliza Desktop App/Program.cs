using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Eliza_Desktop_App
{
    static class Program
    {
        private static bool debugMode;
        private static string elizaClientScriptPath;

        public static bool DebugMode
        {
            get
            {
                return debugMode;
            }
        }

        public static string ElizaClientScriptPath
        {
            get
            {
                return elizaClientScriptPath;
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            debugMode = false;
            elizaClientScriptPath = @".\elizaclient.pyc";

            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == "-debug")
                {
                    debugMode = true;
                }
                else if (args[i] == "-clientpath" && i + 1 < args.Length)
                {
                    elizaClientScriptPath = args[i + 1];
                }
            }

            if (!File.Exists(elizaClientScriptPath))
            {
                MessageDialogs.Error(string.Format("Cannot find {0}", elizaClientScriptPath));
                return;
            }

            ElizaClient elizaClient = new ElizaClient();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(elizaClient));
        }
    }
}
