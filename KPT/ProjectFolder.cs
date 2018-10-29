using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.XMLBuild;
using System.Windows.Forms;

namespace KPT
{
    // this is just a shell of a class at the moment that will become an important reference point for the UI later - it is possible that the directory structure from the Dumper will be moved here as well
    static class ProjectFolder
    {

        /// <summary>
        /// The location of the project / the root directory from which the paths of all other directories will be derived
        /// </summary>
        public static string rootDir = @"H:\kokoro_connect\BuildingTest";
        /// <summary>
        /// Dump of ISO contents - CPKs etc.
        /// </summary>
        public const string extractedISODir = "Extracted ISO Files";
        /// <summary>
        /// Game files extracted from CPKs in extracted ISO folder or standalone files copied over directly from the extracted ISO folder - bin files etc.
        /// </summary>
        public const string unpackedGameFilesDir = "Unpacked Game Files";
        /// <summary>
        /// Disassembled game files
        /// </summary>
        public const string disassemblyDir = "Disassembled Files";
        /// <summary>
        /// Files presented in a more readily editable format - spreadsheets of strings etc.
        /// </summary>
        public const string editableGameFiesDir = "Editable Game Files";
        /// <summary>
        /// Game files that have been reassembled - bins etc.
        /// </summary>
        public const string reassembledGameFilesDir = "Reassembled Game Files";
        /// <summary>
        /// Game files that have been repacked where necessary - CPKs etc.
        /// </summary>
        public const string repackedGameFilesDir = "Repacked Game Files";
        /// <summary>
        /// XML files, scripts, etc. used to direct the reassembly or repacking of files
        /// </summary>
        public const string buildScriptsDir = "Build Scripts";

        public static string GetRootDir()
        {
            return rootDir;
        }

        public static bool CheckProjectFile(string fileName)
        {
            return true;
        }

        public static void RebuildCPKs()
        {
            string buildDir = Path.Combine(rootDir, buildScriptsDir);
            
            if (!Directory.Exists(buildDir))
            {
                string errorMessage = string.Format("Could not find build scripts directory at {0}.", buildDir);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] fileList = Directory.GetFiles(buildDir);

            foreach (var file in fileList)
            {
                if (file.EndsWith(".cpk.yaml"))
                {
                    CPKBuildObject cpk = new CPKBuildObject();
                    cpk.BuildCPK(file);
                }
            }
        }

    }
}
