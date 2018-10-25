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
    class CPKFileMeta
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
        Dictionary<uint, CPKFileMeta> files;
        
        public CPKBuildObject()
        {
            files = new Dictionary<uint, CPKFileMeta>();
        }

        public void SetOriginalFileLocation(string filePath)
        {
            originalFileLocation = filePath;
        }

        public void AddFile(uint id, CPKFileMeta cpk)
        {
            files[id] = cpk;
        }

        public void CommitXML(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(Identifiers.CPK_BUILD_TAG);
            xmlWriter.WriteAttributeString(Identifiers.SHORT_FILE_NAME_TAG, Path.GetFileName(originalFileLocation));

            xmlWriter.WriteStartElement(Identifiers.ORIGINAL_LOCATION_TAG);
            xmlWriter.WriteString(originalFileLocation);
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

                xmlWriter.WriteStartElement(Identifiers.CHECKSUM_TAG);
                xmlWriter.WriteAttributeString(Identifiers.CHECKSUM_TYPE_TAG, Identifiers.MD5_TAG);
                xmlWriter.WriteString(file.Value.checksumValue);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

        }

    }
}
