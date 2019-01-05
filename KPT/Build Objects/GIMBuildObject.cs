using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Build_Objects
{

    /// <summary>
    /// Object that keeps track of data required to place an edited image back in the game
    /// </summary>
    class GIMBuildObject
    {

        public string originalFileLocation;
        public string targetFileLocation;
        public string checksumValue;
        public Checksum checksumType;

        public void SerializeToDisk(string targetFile)
        {
            var yamlSerialzer = new SharpYaml.Serialization.Serializer();
            var serializedData = yamlSerialzer.Serialize(this);

            string targetPath = Path.Combine(targetFile + ".yaml");
            DirectoryGuard.CheckDirectory(targetPath);

            FileStream fs = new FileStream(targetPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(serializedData);

            sw.Close();
            fs.Close();

        }

        public bool DeserializeFromDisk(string targetFile)
        {
            {
                if (!File.Exists(targetFile))
                {
                    string errorMessage = string.Format("File {0} did not exist.", targetFile);
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var yamlSerializer = new SharpYaml.Serialization.Serializer();
                FileStream fs;

                try
                {
                    fs = new FileStream(targetFile, FileMode.Open);
                }
                catch (Exception e)
                {
                    string errorMessage = string.Format("There was an error when opening file {0}.\r\n\r\n{1}.", targetFile, e.Message);
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                StreamReader sr = new StreamReader(fs);
                string data = sr.ReadToEnd();

                try
                {
                    yamlSerializer.DeserializeInto<GIMBuildObject>(data, this);
                }
                catch (Exception e)
                {
                    string errorMessage = string.Format("There was an error when parsing file {0}.\r\n\r\n{1}.", targetFile, e.Message);
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                sr.Close();
                fs.Close();

                return true;
            }
        }

    }
}
