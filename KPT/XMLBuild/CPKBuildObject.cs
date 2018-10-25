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
    /// Used to create an XML element that describes how a CPK should be rebuilt
    /// </summary>
    class CPKBuildObject
    {
        string originalFileLocation;
        Dictionary<uint, string> files;
        
        public CPKBuildObject()
        {
            files = new Dictionary<uint, string>();
        }

        public void SetOriginalFileLocation(string filePath)
        {
            originalFileLocation = filePath;
        }

        public void AddFile(string fileName, uint id)
        {
            files[id] = fileName;
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
                xmlWriter.WriteString(file.Value);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

        }

    }
}
