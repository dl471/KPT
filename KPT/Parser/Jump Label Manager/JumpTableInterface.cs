using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;

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
        private static readonly string JUMP_TABLE_FILE = Path.Combine("PSP_GAME", "USRDIR", "St000", "St000", "St000_SldtDat.bin");

        private short firstNumber;
        List<JumpTableEntry> jumpTableEntries;
        DataBox footer;

        public JumpTableInterface()
        {
            jumpTableEntries = new List<JumpTableEntry>();
        }

        public void LoadJumpTable()
        {
            string originalFilePath = Path.Combine(ProjectFolder.rootDir, ProjectFolder.unpackedGameFilesDir, JUMP_TABLE_FILE);
        }

        public void SaveJumpTable()
        {
            string targetFilePath = Path.Combine(ProjectFolder.rootDir, ProjectFolder.reassembledGameFilesDir, JUMP_TABLE_FILE);
        }
    }
}
