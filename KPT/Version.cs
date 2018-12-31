using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPT
{
    static class Version
    {
        /// <summary>
        /// List of known versions
        /// </summary>
        public enum Versions
        {
            v0_0_1,
        };

        /// <summary>
        /// Current version
        /// </summary>
        const Versions CURRENT_VERSION = Versions.v0_0_1;

        public static Versions CurrentVersion
        {
            get
            {
                return CURRENT_VERSION;
            }

        }
        
        /// <summary>
        /// Convert the current version to a more user friendly string
        /// </summary>
        /// <returns>Current version as string formatted to current specifications</returns>
        public static string ToFormattedString(this Versions version)
        {
            return version.ToString().Replace("_", ".");
        }       

    }
}
