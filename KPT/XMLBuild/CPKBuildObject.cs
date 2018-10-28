using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT;
using System.Xml;
using System.Windows.Forms;
using System.IO;

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
    /// Used to create an XML element that describes how a CPK should be rebuilt
    /// </summary>
    class CPKBuildObject
    {
        string originalFileLocation;
        string targetFileLocation;
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

        public void CommitXML(string targetFile)
        {
            string targetPath = Path.Combine(targetFile, GenerateCPKID() + ".xml");
            DirectoryGuard.CheckDirectory(targetPath);

            FileStream fs = new FileStream(targetPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(sw, settings);

            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement(Identifiers.CPK_BUILD_TAG);
            xmlWriter.WriteAttributeString(Identifiers.SHORT_FILE_NAME_TAG, Path.GetFileName(originalFileLocation));

            xmlWriter.WriteStartElement(Identifiers.ORIGINAL_LOCATION_TAG);
            xmlWriter.WriteString(originalFileLocation);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement(Identifiers.TARGET_FILE_PATH_TAG);
            xmlWriter.WriteString(targetFileLocation);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement(Identifiers.FILE_LIST_TAG);
            xmlWriter.WriteAttributeString(Identifiers.FILE_NUM_TAG, files.Count.ToString());

            foreach (var file in files)
            {
                xmlWriter.WriteStartElement(Identifiers.FILE_TAG);
                xmlWriter.WriteAttributeString(Identifiers.FILE_ID_TAG, file.Key.ToString());

                xmlWriter.WriteStartElement(Identifiers.FILE_NAME_TAG);
                xmlWriter.WriteString(file.Value.fileName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(Identifiers.RELATIVE_PATH_TAG);
                xmlWriter.WriteString(file.Value.filePath);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(Identifiers.CHECKSUM_TAG);
                xmlWriter.WriteAttributeString(Identifiers.CHECKSUM_TYPE_TAG, Identifiers.MD5_TAG);
                xmlWriter.WriteString(file.Value.checksumValue);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();

            xmlWriter.Close();
            sw.Close();
            fs.Close();

        }

        private string GenerateCPKID()
        {
            return originalFileLocation.Replace(Path.DirectorySeparatorChar, '-').ToLowerInvariant();
        }

        public bool BuildFromXML(string targetFile)
        {
            if (!File.Exists(targetFile))
            {
                string errorMessage = string.Format("File {0} did not exist.", targetFile);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            XmlReader xr = XmlReader.Create(targetFile, null);

            while (xr.Read())
            {
                MessageBox.Show(xr.NodeType.ToString());
            }

            xr.Close();

            return true;
        }

    }
}
