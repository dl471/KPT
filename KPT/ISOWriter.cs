using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using DiscUtils.Iso9660;

namespace KPT
{
    class ISOWriter
    {
        public bool BuildISOFromDirectory(string sourceDirectory, string outputFileName)
        {

            if (!Directory.Exists(sourceDirectory))
            {
                string errorMessage = string.Format("Directory {0} does not exist.", sourceDirectory);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string outputDirectory = Path.GetDirectoryName(outputFileName);

            if (!Directory.Exists(outputDirectory))
            {
                string errorMessage = string.Format("Directory {0} does not exist.", outputDirectory); // could perhaps use directory guard to at least try and resolve this? or is it not worth it?
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            CDBuilder builder = new CDBuilder();
            builder.UseJoliet = true;
            builder.VolumeIdentifier = "TEST_ISO";
            

            List<string> fileList = new List<string>();
            GenerateFileList(sourceDirectory, fileList);

            foreach (var file in fileList)
            {
                string pathOnDisk = file;
                string pathInISO = GetSubPath(file, sourceDirectory);
                builder.AddFile(pathInISO, pathOnDisk);
            }
            
            builder.Build(outputFileName);

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

        private string GetSubPath(string path, string root)
        {
            int rootLen = root.Length + Path.DirectorySeparatorChar.ToString().Length;
            int subPathLen = path.Length - rootLen;
            return path.Substring(rootLen, subPathLen);
        }

    }
}
