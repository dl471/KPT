using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

    }
}
