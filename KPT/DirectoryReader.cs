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
        private string rootDirectory;

        public bool OpenDirectoryStream(string directory)
        {
            if (!Directory.Exists(directory))
            {
                string errorMessage = string.Format("Directory {0} does not exist.", directory);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            fileList = new List<string>();
            rootDirectory = directory;
            GenerateFileList(directory, fileList);
            

            initalized = true;

            return true;
        }

        private void GenerateFileList(string directory, List<string> fileList)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                string newFileName = GetSubPath(file, rootDirectory); // since the directory is intended to mimic the ISO reader, it must transform the file names to how they would appear in the ISO
                fileList.Add(newFileName);
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
                string filePath = Path.Combine(rootDirectory, file);

                try
                {
                    newFileAccessor.dataStream = new FileStream(filePath, FileMode.Open);
                }
                catch (Exception e)
                {
                    string errorMessage = string.Format("There was an error when attempting to open file {0}.\r\n\r\n{1}", file, e.Message);
                    MessageBox.Show(errorMessage);
                    Environment.Exit(1);
                }

                yield return newFileAccessor;

                newFileAccessor.dataStream.Close();

            }
        }

        private string GetSubPath(string path, string root)
        {
            int rootLen = root.Length + Path.DirectorySeparatorChar.ToString().Length;
            int subPathLen = path.Length - rootLen;
            return path.Substring(rootLen, subPathLen);
        }

    }
}
