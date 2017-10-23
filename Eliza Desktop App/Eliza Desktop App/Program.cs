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
