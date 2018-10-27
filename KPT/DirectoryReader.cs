using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT
{
    /// <summary>
    /// Mimics the ISO reader
    /// </summary>
    class DirectoryReader
    {
        private bool initalized = false;
        private List<string> fileList;

        public bool OpenDirectoryStream(string directory)
        {
            if (!Directory.Exists(directory))
            {
                string errorMessage = string.Format("Directory {0} does not exist.", directory);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            fileList = new List<string>();
            GenerateFileList(directory, fileList);

            initalized = true;

            return true;
        }

        private void GenerateFileList(string directory, List<string> fileList)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                fileList.Add(file);
            }

            foreach (var dir in Directory.GetDirectories(directory))
            {
                GenerateFileList(dir, fileList);
            }
        }

        public IEnumerable<EmbeddedFileAccessor> GetGenerator()
        {

            if (!initalized)
            {
                throw new Exception("Directory reader not initalized");
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
                    newFileAccessor.dataStream = new FileStream(file, FileMode.Open);
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
