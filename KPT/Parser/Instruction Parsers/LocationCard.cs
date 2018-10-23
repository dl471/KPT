using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Instruction_Parsers
{
    class LocationCard : IInstruction
    {
        Opcode opcode;
        string time;
        string location;

        public bool Read(BinaryReader br)
        {
            opcode = ElementReader.ReadOpcode(br);
            time = ElementReader.ReadNullTerminatedString(br);
            location = ElementReader.ReadNullTerminatedString(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            return true;
        }
    }
}
