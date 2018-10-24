using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instruction_Parsers
{
    class U_502 : IInstruction
    {
        Opcode opcode;
        Box box1;
        string name;
        string dialogue;
        Box box2;

        public bool Read(BinaryReader br)
        {
            opcode = ElementReader.ReadOpcode(br);
            box1 = new Box(0x0C);
            box1.Read(br);
            name = ElementReader.ReadName(br);
            dialogue = ElementReader.ReadDialogue(br);
            box2 = new Box(0x04);
            box2.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            return true;
        }

    }
}
