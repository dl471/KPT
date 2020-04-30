using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instructions
{
    class InterFileJump : IInstruction
    {
        // make a jump to a different file - possibly also uses lookup table in St000_SldtDat.bin
        Opcode opcode;
        short cpNumber;
        short stNumber;
        // same deal as the look up codes in InterFileJump
        short firstLookUpCode;
        short secondLookUpCode;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            stNumber = br.ReadInt16();
            cpNumber = br.ReadInt16();
            firstLookUpCode = br.ReadInt16();
            secondLookUpCode = br.ReadInt16();
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            bw.Write(cpNumber);
            bw.Write(stNumber);
            bw.Write(firstLookUpCode);
            bw.Write(secondLookUpCode);
            return true;
        }
    }
}
