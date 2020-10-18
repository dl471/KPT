using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instructions
{
    class U_111 : IInstruction
    {
        // seems to be related to jumps, but the exact purpose is still unclear
        Opcode opcode;
        short lookUpCode; // most likely, this will tie into a lookup code in the jumptable

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
