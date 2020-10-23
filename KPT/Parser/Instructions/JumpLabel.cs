using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instructions
{
    class JumpLabel : IInstruction
    {
        // makes a position in a file where a jump may target
        Opcode opcode;
        public short lookUpCode { get; private set; } // lookup code for the jump table to find file number, offset, etc. to actually jump to

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            lookUpCode = br.ReadInt16();
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            bw.Write(lookUpCode);
            return true;
        }
    }
}
