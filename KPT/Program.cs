using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using LibCPK;
using KPT.Parser;
using KPT.Parser.Instructions;
using System.Xml;
using System.Xml.Serialization;

namespace KPT
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ActiveEncodings.Initalize();
            DirectoryGuard.Initalize();

            Application.Run(new Form1());
            Environment.Exit(0);
        }
    }
}
