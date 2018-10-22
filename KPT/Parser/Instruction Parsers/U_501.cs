using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instruction_Parsers
{
    class U_501 : IInstructionParser
    {

        public bool Read(BinaryReader br)
        {
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            return true;
        }

    }
}
