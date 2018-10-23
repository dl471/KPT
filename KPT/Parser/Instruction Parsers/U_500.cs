using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instruction_Parsers
{
    class U_500 : IInstructionParser
    {
        Opcode opcode;
        Box box1;
        string name;
        string dialogue;

        public bool Read(BinaryReader br)
        {
            opcode = ElementReader.ReadOpcode(br);
            box1 = new Box(0x8);
            box1.Read(br);
            name = ElementReader.ReadName(br);
            dialogue = ElementReader.ReadDialogue(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            return true;
        }
    }
}
