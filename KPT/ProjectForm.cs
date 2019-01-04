using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using KPT.Build_Objects;

namespace KPT
{
    public partial class ProjectForm : Form
    {
        public ProjectForm()
        {
            InitializeComponent();
            this.Text = "Working Project";
        }

        public bool CheckFolder(string fileName)
        {
            return true;
        }

        private void RebuildCPKs_Click(object sender, EventArgs e)
        {
            if (ProjectFolder.RebuildCPKs())
            {
                MessageBox.Show("CPKs rebuilt!");
            }
            else
            {
                MessageBox.Show("There was an error while rebuilding the CPKs.");
            }
            
        }

        private void DumpStrings_Click(object sender, EventArgs e)
        {
            if (ProjectFolder.DumpStrings())
            {
                MessageBox.Show("Strings dumped!");
            }
            else
            {
                MessageBox.Show("There was an error while dumping strings.");
            }
        }

        private void LoadStrings_Click(object sender, EventArgs e)
        {
            if (ProjectFolder.LoadStrings())
            {
                MessageBox.Show("Strings loaded!");
            }
            else
            {
                MessageBox.Show("There was an error while loading strings.");
            }
        }

        private void DumpISO_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "ISO|*.iso";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName == "")
            {
                return;
            }

            if (ProjectFolder.DumpISO(openFileDialog1.FileName))
            {
                MessageBox.Show("ISO dumped!");
            }
            else
            {
                MessageBox.Show("There was an error while dumping the ISO.");
            }


        }

        private void UnpackFiles_Click(object sender, EventArgs e)
        {
            var dumper = new Dumper();

            if (dumper.ProcessDirectory(ProjectFolder.GetRootDir()))
            {
                MessageBox.Show("Files unpacked!");
            }
            else
            {
                MessageBox.Show("There was an error while unpacking the files.");
            }
        }

        private void DumpImages_Click(object sender, EventArgs e)
        {
            List<string> imageFiles = new List<string>();
            string filter = ".gim";
            int counter = 0; // used to add a number to each image name, just in case disambiguation is required

            GenerateFileListFiltered(Path.Combine(ProjectFolder.GetRootDir(), ProjectFolder.unpackedGameFilesDir), imageFiles, filter);

            foreach (var file in imageFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string targetFilePath = Path.Combine(ImageHandler.GetImagesDir(), fileName);
                string targetFilePathPostFix = string.Format("_{0}.png", counter.ToString());
                targetFilePath += targetFilePathPostFix;
                ImageHandler.ConvertImage(file, targetFilePath);
                counter++;

                var gimBuildInstructions = new GIMBuildObject();
                gimBuildInstructions.originalFileLocation = Path.Combine(ProjectFolder.editableGameFiesDir, ImageHandler.imagesDir, Path.GetFileName(targetFilePath));
                gimBuildInstructions.targetFileLocation = Path.Combine(ProjectFolder.reassembledGameFilesDir, ProjectFolder.GetSubPath(file, Path.Combine(ProjectFolder.GetRootDir(), ProjectFolder.unpackedGameFilesDir))); // this is a bad line
                gimBuildInstructions.checksumType = Checksum.MD5;

                FileStream fs = new FileStream(targetFilePath, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);

                byte[] imageData = br.ReadBytes((int)br.BaseStream.Length);
                gimBuildInstructions.checksumValue = Checksums.GetMD5(imageData);

                br.Close();
                fs.Close();

                gimBuildInstructions.SerializeToDisk(Path.Combine(ProjectFolder.GetRootDir(), ProjectFolder.buildScriptsDir, Path.GetFileName(targetFilePath)));

            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        // This function and variants keep getting copy and pasted around and I'm not sure what to do with it - might shove it in a utils Class
        private void GenerateFileListFiltered(string directory, List<string> fileList, string filter)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.EndsWith(filter))
                {
                    fileList.Add(file);
                }
            }

            foreach (var dir in Directory.GetDirectories(directory))
            {
                GenerateFileListFiltered(dir, fileList, filter);
            }
        }


    }


}
