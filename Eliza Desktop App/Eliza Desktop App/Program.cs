using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eliza_Desktop_App
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ElizaClient elizaClient = new ElizaClient();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(elizaClient));
        }
    }
}
