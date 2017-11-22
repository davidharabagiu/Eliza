using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eliza_Desktop_App
{
    static class Program
    {
        private static bool debugMode;

        public static bool DebugMode
        {
            get
            {
                return debugMode;
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            bool result;
            var mutex = new System.Threading.Mutex(true, "ElizaUniqueAppId", out result);

            if (!result)
            {
                MessageDialogs.Error("Another instance of Eliza is already running.");
                return;
            }

            debugMode = false;
            if (args.Length > 0 && args[0] == "-debug")
            {
                debugMode = true;
            }

            ElizaClient elizaClient = new ElizaClient();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(elizaClient));
        }
    }
}
