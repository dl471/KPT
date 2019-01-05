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
            beta_0_0_1,
            beta_1_0_0,
            beta_1_0_1,
        };

        /// <summary>
        /// Current version
        /// </summary>
        const Versions CURRENT_VERSION = Versions.beta_1_0_1;

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
            string newVersion = version.ToString();
            newVersion = newVersion.Replace("_", ".");
            newVersion = newVersion.Replace("beta.", "beta-");
            return newVersion;
        }       

    }
}
