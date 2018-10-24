using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Instructions
{
    class U_504 : IInstruction
    {

        Opcode opcode;
        Box box1;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
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
