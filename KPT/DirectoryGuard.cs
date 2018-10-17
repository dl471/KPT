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
    /// Used to guard against attempting to create files in non-existent directories
    /// </summary>
    static class DirectoryGuard
    {
        /// <summary>
        /// A collection of directories already known to exist by the DirectoryGuard
        /// </summary>
        static HashSet<string> knownDirectories;

        public static void Initalize()
        {
            knownDirectories = new HashSet<string>();
        }

        /// <summary>
        /// Ensures that every directory in the given path exists. Creates them if they do not.
        /// </summary>
        /// <param name="directory">The directory to be checked</param>
        public static void CheckDirectory(string directory)
        {
            string targetDir = Path.GetDirectoryName(directory);
            
            while (!knownDirectories.Contains(targetDir) && !Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                knownDirectories.Add(targetDir);
                CheckDirectory(targetDir);
            }

        }

    }
}
