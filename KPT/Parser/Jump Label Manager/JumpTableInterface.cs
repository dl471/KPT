using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;
using KPT.Parser.Headers;
using KPT.Parser.Footers;
using System.Text.RegularExpressions;

namespace KPT.Parser.Jump_Label_Manager
{
    /// <summary>
    /// Read and write the games jump table
    /// </summary>
    /// <remarks>
    /// The game handles jumps by looking up indexes in a jump table that is stored as a file on disk. 
    /// </remarks>
    class JumpTableInterface
    {
        private string jumpTablePath = string.Empty;

        St_Header header;
        List<JumpTableEntry> jumpTableEntries;
        St_Footer footer;

        public JumpTableInterface(string jumpTableLocation)
        {
            header = new St_Header();
            footer = new St_Footer();
            jumpTableEntries = new List<JumpTableEntry>();

            if (IsJumpTable(jumpTableLocation))
            {
                jumpTablePath = jumpTableLocation;
            }
            else
            {
                throw new Exception("File " + jumpTableLocation + "did not match expected jump table name convention");
            }

        }

        /// <summary>
        /// Find a jumpt table within a list of files
        /// </summary>
        /// <param name="fileList">Array of file paths to check</param>
        /// <returns>Path to jump table</returns>
        /// <remarks>Expects to find exactly one jump table, throws exception on finding multiple</remarks>
        public static string FindJumpTable(string[] fileList)
        {
            var jumpTableArray = 
                fileList
                .Where(file => IsJumpTable(file))
                .ToArray();

            switch (jumpTableArray.Length)
            {
                case 0:
                    throw new Exception("Could not find jump table in specified path");
                    break;
                case 1:
                    return jumpTableArray.First();
                    break;
                default:
                    throw new Exception("Found multiple jump tables in specified path - please process each St directory seperately");
                    break;                
            }

        }

        public static bool IsJumpTable(string fileName)
        {
            Regex regex = new Regex(@"St[0-9A-F]{3}_SldtDat\.bin"); // search for the StXXX_SldtDat.bin format
            Match match = regex.Match(fileName);
            return match.Success;
        }

        public void LoadJumpTable()
        {
            string originalFilePath = Path.Combine(ProjectFolder.rootDir, ProjectFolder.unpackedGameFilesDir, jumpTablePath);            

            FileStream fs = new FileStream(originalFilePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            footer.Read(br);
            header.Read(br);

            long streamEnd = br.BaseStream.Length - ElementHelper.GetElementSize(footer);

            while (br.BaseStream.Position != streamEnd)
            {
                var nextEntry = new JumpTableEntry();
                nextEntry.Read(br);
                jumpTableEntries.Add(nextEntry);
            }

            br.Close();
        }

        public void SaveJumpTable()
        {
            string targetFilePath = Path.Combine(ProjectFolder.rootDir, ProjectFolder.reassembledGameFilesDir, jumpTablePath);

            FileStream fs = new FileStream(targetFilePath, FileMode.Open);
            BinaryWriter bw = new BinaryWriter(fs);

            header.Write(bw);

            foreach (var entry in jumpTableEntries)
            {
                entry.Write(bw);
            }

            footer.Write(bw);

            bw.Close();

        }

        public List<JumpTableEntry> GetJumpTableEntries()
        {
            return jumpTableEntries;
        }

        /// <summary>
        /// Iterate through the jump table entries and verify that each global look up code is unique
        /// </summary>
        public void VerifyJumpTableGlobalCodes()
        {
            HashSet<int> lookUpCodes = new HashSet<int>();

            foreach (var entry in jumpTableEntries)
            {
                var lookupCode = entry.LookUpCode;
                if (lookUpCodes.Contains(lookupCode))
                {
                    throw new Exception(string.Format("Non-unique lookup code: lookup code {0} has been reused", lookupCode));
                }
                lookUpCodes.Add(lookupCode);

            }

        }

    }
}
