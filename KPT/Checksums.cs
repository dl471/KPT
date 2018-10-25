using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace KPT
{
    /// <summary>
    /// An enum of available checksum functions
    /// </summary>
    enum Checksum { MD5 }

    /// <summary>
    /// Generates checksums for data as needed
    /// </summary>
    static class Checksums
    {
        /// <summary>
        /// Calculates the MD5 hash of a byte array and converts it to a more classic MD5 formatting
        /// </summary>
        /// <param name="fileAsBytes">The array whose MD5 is to be calcluated</param>
        /// <returns>Calculated MD5 hash</returns>
        public static string GetMD5(byte[] fileAsBytes)
        {
            var md5Maker = MD5.Create();
            byte[] md5AsBytes = md5Maker.ComputeHash(fileAsBytes);
            string md5String = BitConverter.ToString(md5AsBytes);
            md5String = md5String.Replace("-", "");
            md5String = md5String.ToLowerInvariant();
            return md5String;
        }

    }
}
