using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Instruction_Parsers
{
    class U_504 : IInstructionParser
    {

        Opcode opcode;
        Box box1;

        public bool Read(BinaryReader br)
        {
            opcode = ElementReader.ReadOpcode(br);
            box1 = new Box(0x2);
            box1.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            return true;
        }

    }
}
