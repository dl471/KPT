using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using LibCPK;

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


            const string testFileDirectory = @"C:\Sandbox\kay\desubox\drive\H\kpt_test\test files";
            string testFileLocation = Path.Combine(testFileDirectory, "test_file_1.cpk");
            string testInLocation = Path.Combine(testFileDirectory, "test_in");
            string testOutLocation = Path.Combine(testFileDirectory, "test_out");

            string testDirectory = @"C:\Sandbox\kay\desubox\drive\H\kpt_test\test_in";

            var dumpTest = new Dumper();
            dumpTest.ProcessDirectory(testDirectory);
                


            var test = new CPK(new Tools());

            Encoding sj = Encoding.GetEncoding("shift-jis");


            foreach (FileEntry file in test.FileTable)
            {
                string fileName = file.FileName.ToString();
                MessageBox.Show(fileName);
                if (fileName.EndsWith(".bin"))
                {
                    int filePos = (int)(file.FileOffset);
                    uint fileSize = (uint)(file.FileSize);
                    MessageBox.Show(fileSize.ToString("X"));
                }
            }

            Environment.Exit(0);

            Application.Run(new Form1());
        }
    }
}
