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
    /// Marks an entry in the games jump table
    /// </summary>
    class JumpTableEntry : IElement
    {

        StCpNumber fileNumber;
        short sequentialChoiceNumber; // suspected to the sequential number of the choice within the file but not 100% guaranteed
        short unknown1;
        short offset;
        short unknown2;
        short unknown3;
        short lookupCode; // the look up code used by IntraFileJump and possibly also InterFileJump
        short unknown4;
        short unknown5;
        DataBox trailingBlock; // this block is usually but is not always entirey 0xFF ands its purpose is not clear

        public JumpTableEntry()
        {
            fileNumber = new StCpNumber();
        }

        public bool Read(BinaryReader br)
        {
            fileNumber.Read(br);
            sequentialChoiceNumber = br.ReadInt16();
            unknown1 = br.ReadInt16();
            offset = br.ReadInt16();
            unknown2 = br.ReadInt16();
            unknown3 = br.ReadInt16();
            lookupCode = br.ReadInt16();
            unknown4 = br.ReadInt16();
            unknown5 = br.ReadInt16();
            trailingBlock = new DataBox(0x82);
            trailingBlock.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            fileNumber.Write(bw);
            bw.Write(sequentialChoiceNumber);
            bw.Write(unknown1);
            bw.Write(offset);
            bw.Write(unknown2);
            bw.Write(unknown3);
            bw.Write(lookupCode);
            bw.Write(unknown4);
            bw.Write(unknown5);
            trailingBlock.Write(bw);
            return true;
        }

    }
}
