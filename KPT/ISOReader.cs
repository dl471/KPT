using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscUtils.Iso9660;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace KPT
{
    struct EmbeddedFileAccessor
    {
        public string name;
        public Stream dataStream;
    }

    class ISOReader
    {
        private FileStream isoFile;
        private CDReader isoReader;
        private bool initalized = false;
        private List<string> fileList;

        public bool OpenISOStream(string isoPath, string outputDirectory)
        {

            if (!File.Exists(isoPath))
            {
                string errorMessage = string.Format("Could not open ISO {0}.", isoPath);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;                   
            }

            if(!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception e)
                {
                    string errorMessage = string.Format("There was an error while attempting to create directory {0}.\r\n\r\n{1}", outputDirectory, e.Message);
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            
            if (Directory.GetDirectories(outputDirectory).Length != 0 || Directory.GetFiles(outputDirectory).Length != 0)
            {
                string errorMessage = string.Format("There was an attempt to extraxct an ISO to directory {0} but the directory was not empty. Extracting the ISO will destroy any files in the folder. Please confirm this is your intention by manually deleting any files or folders in the directory and trying again.", outputDirectory);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            FileStream fs = new FileStream(isoPath, FileMode.Open);
            CDReader iso = new CDReader(fs, true);

            isoFile = fs;
            isoReader = iso;

            fileList = new List<string>();
            GenerateFileList(iso, iso.Root.FullName, fileList);

            initalized = true;

            return true;
        }

        public void CloseISOStream()
        {
            // couldn't find documentation on whether or not CDReader needs to be closed or disposed
            isoFile.Close();
        }

        private void GenerateFileList(CDReader iso, string directory, List<string> fileList)
        {
            foreach (var file in iso.GetFiles(directory))
            {
                fileList.Add(file);
            }

            foreach (var dir in iso.GetDirectories(directory))
            {
                GenerateFileList(iso, dir, fileList);
            }

        }

        public IEnumerable<EmbeddedFileAccessor> GetGenerator()
        {

            if(!initalized)
            {
                throw new Exception("ISO reader not initalized");
            }

            return EnumerateFiles();
        }

        private IEnumerable<EmbeddedFileAccessor> EnumerateFiles()
        {
            foreach (string file in fileList)
            {
                var newFileAccessor = new EmbeddedFileAccessor();
                newFileAccessor.name = file;

                try
                {
                    newFileAccessor.dataStream = isoReader.OpenFile(file, FileMode.Open);
                }
                catch (Exception e)
                {
                    string errorMessage = string.Format("There was an error when attempting to open file {0} in the ISO.\r\n\r\n{1}", file, e.Message);
                    MessageBox.Show(errorMessage);
                    Environment.Exit(1);
                }

                yield return newFileAccessor;

                newFileAccessor.dataStream.Close();
                
            }
        }

    }

}
