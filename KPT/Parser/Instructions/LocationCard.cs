using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Instructions
{
    class LocationCard : IInstruction
    {
        Opcode opcode;
        string time;
        string location;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            time = FileIOHelper.ReadNullTerminatedString(br);
            location = FileIOHelper.ReadNullTerminatedString(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            FileIOHelper.WriteStringNullTerminated(bw, time);
            FileIOHelper.WriteStringNullTerminated(bw, location);
            return true;
        }
    }
}
