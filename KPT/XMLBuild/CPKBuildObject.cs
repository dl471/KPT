using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using SharpYaml;

namespace KPT.XMLBuild
{

    
    /// <summary>
    /// The necessary data for keeping track of a file within a CPK
    /// </summary>
    class CPKEmbeddedFileMeta
    {
        public string fileName;
        public string filePath;
        public uint ID;
        public string checksumValue;
        public Checksum checksumType;
        
    }

    /// <summary>
    /// Keeps track of the data necessary to build a CPK
    /// </summary>
    class CPKBuildObject
    {
        [SharpYaml.Serialization.YamlMember] // this makes it write private members
        string originalFileLocation;
        [SharpYaml.Serialization.YamlMember]
        string targetFileLocation;
        [SharpYaml.Serialization.YamlMember]
        Dictionary<uint, CPKEmbeddedFileMeta> files;
        
        public CPKBuildObject()
        {
            files = new Dictionary<uint, CPKEmbeddedFileMeta>();
        }

        public void SetOriginalFileLocation(string filePath)
        {
            originalFileLocation = filePath;
        }

        public void SetTargetFileLocation(string filePath)
        {
            targetFileLocation = filePath;
        }

        public void AddFile(uint id, CPKEmbeddedFileMeta cpk)
        {
            files[id] = cpk;
        }

        public void SerializeToDisk(string targetFile)
        {
            var yamlSerialzer = new SharpYaml.Serialization.Serializer();
            var serializedData = yamlSerialzer.Serialize( this);

            string targetPath = Path.Combine(targetFile, GenerateCPKID() + ".yaml");
            DirectoryGuard.CheckDirectory(targetPath);

            FileStream fs = new FileStream(targetPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(serializedData);

            sw.Close();
            fs.Close();
        }

        private string GenerateCPKID()
        {
            return originalFileLocation.Replace(Path.DirectorySeparatorChar, '-').ToLowerInvariant();
        }

        public bool DeserializeFromDisk(string targetFile)
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
            files = new Dictionary<uint, CPKEmbeddedFileMeta>();

            try
            {
                yamlSerializer.DeserializeInto<CPKBuildObject>(data, this);
            }
            catch
            {
                string errorMessage = string.Format("There was an error when parsing file {0}.\r\n\r\n{1}.", targetFile, e.Message);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // might want to specifically check all fields for being null here even if it is a very unlikely edge case

            sr.Close();
            fs.Close();

            return true;
        }

    }
}
