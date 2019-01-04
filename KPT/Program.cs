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

            //ImageHandler.GIMToPNG(@"H:\kokoro_connect\ImageTest\input.gim", @"H:\kokoro_connect\ImageTest\output.png");
            //ImageHandler.GIMToPNG(@"H:\kokoro_connect\ImageTest\Logo_Banpre.gim", @"H:\kokoro_connect\ImageTest\logo_test.png");
            ImageHandler.ConvertImage(@"H:\kokoro_connect\ImageTest\logo_test.png", @"H:\kokoro_connect\ImageTest\Logo_Banpre.gim");

            Application.Run(new Form1());
            Environment.Exit(0);

            //StringBuilder target = new StringBuilder();
            //var xmlTest = XmlWriter.Create(target);

            //FileStream target = new FileStream("xmlTest.xml", FileMode.Create);
            //var xmlSettings = new XmlWriterSettings();
            //xmlSettings.Indent = true;
            //XmlWriter xmlTest = XmlWriter.Create(target, xmlSettings);


            //xmlTest.WriteStartDocument();

            //xmlTest.WriteStartElement("ISO");

            //xmlTest.WriteStartElement("desu");
            //xmlTest.WriteString("dolls");
            //xmlTest.WriteEndElement();

            //xmlTest.WriteEndElement();

            //xmlTest.WriteEndDocument();

            //xmlTest.Close();

            ////FileStream 
            ////StreamWriter sw = new StreamWriter(xmlFile);

            ////sw.Write(target.ToString());

            ////sw.Close();
            //target.Close();

            //Environment.Exit(0);


            const string testFileDirectory = @"C:\Sandbox\kay\desubox\drive\H\kpt_test\test files";
            string testFileLocation = Path.Combine(testFileDirectory, "test_file_1.cpk");
            string testInLocation = Path.Combine(testFileDirectory, "test_in");
            string testOutLocation = Path.Combine(testFileDirectory, "test_out");

            string testSourceDirectory = @"C:\Sandbox\kay\desubox\drive\H\kpt_test\test_in";
            string testTargetDirectory = @"C:\Sandbox\kay\desubox\drive\H\kpt_test\test_out";

            string parserTestInDir = @"C:\Sandbox\kay\desubox\drive\H\kpt_test\test_out\Editable\PSP_GAME\USRDIR\St000";
            //string parserTestInName = "St000_Cp0101.bin";
            //string parserTestInName = "St109_Cp0002.bin";
            //string parserTestInName = "St000_Cp02A1.bin"; // has the very interesting opcode that we need the paste for
            string parserTestInName = "St105_cp1301.bin";

            string parserTestIn;

            int fileNum = 0;
            int roughDialogueEstimate = 0;

            

            ////foreach (string file in new List<string> { Path.Combine(parserTestInDir, parserTestInName) })
            //foreach (string file in Directory.GetFiles(parserTestInDir))
            //{

            //    //parserTestIn = Path.Combine(parserTestInDir, parserTestInName);
            //    parserTestIn = file;

            //    if (!File.Exists(parserTestIn))
            //    {
            //        MessageBox.Show("File " + parserTestInName + " does not exist.");
            //        Environment.Exit(1);
            //    }

            //    FileStream fs = new FileStream(parserTestIn, FileMode.Open);
            //    BinaryReader br = new BinaryReader(fs);

            //    //br.BaseStream.Position = 0x20A;

            //    var testParser = new FileParser();
            //    var parsedFile = testParser.ParseFile(br, Path.GetFileName(file));

            //    foreach (IElement element in parsedFile.instructions)
            //    {
            //        if (element is U_501 || element is U_502 || element is U_500)
            //        {
            //            roughDialogueEstimate += 1;
            //        }
            //    }

            //    br.Close();
            //    fs.Close();

            //    fileNum += 1;
            //}

            //MessageBox.Show("Parsed " + fileNum.ToString() + " files with roughly " + roughDialogueEstimate.ToString() + " lines of dialogue!");

            //Environment.Exit(0);

           // var dumpTest = new Dumper();
            //dumpTest.ProcessDirectory(testSourceDirectory, testTargetDirectory);

            Environment.Exit(0);

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
;