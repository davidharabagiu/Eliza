using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eliza_Desktop_App
{
    public static class MessageDialogs
    {
        public static void Info(string text)
        {
            MessageBox.Show(text, "Eliza", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warning(string text)
        {
            MessageBox.Show(text, "Eliza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string text)
        {
            MessageBox.Show(text, "Eliza", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool YesNo(string text)
        {
            DialogResult res =  MessageBox.Show(
                text,
                "Eliza",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            return res == DialogResult.Yes;
        }
    }
}
