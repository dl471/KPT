using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Instruction_Parsers
{
    static class ElementReader
    {

        const int NAME_LENGTH = 20;

        public static Opcode ReadOpcode(BinaryReader br)
        {
            int opcode = br.ReadInt16();

            if (Enum.IsDefined(typeof(Opcode), opcode))
            {
                return (Opcode)opcode;
            }

            return Opcode.INVALID;
        }

        public static string ReadName(BinaryReader br)
        {
            byte[] nameAsBytes = br.ReadBytes(NAME_LENGTH);
            return ActiveEncodings.currentEncoding.GetString(nameAsBytes);
        }
        
        public static string ReadDialogue(BinaryReader br)
        {
            long start = br.BaseStream.Position;
            long end = 0;

            while (br.ReadByte() != 0x00)
            {
                end++;
            }

            br.BaseStream.Position = start;

            byte[] stringAsBytes = br.ReadBytes((int)end);
            string readString = ActiveEncodings.currentEncoding.GetString(stringAsBytes);

            br.ReadBytes(2); // the strings seem to be followed by 2 nulls - not sure why. first could be a null terminator, second could be some kind of alignnment thing. keep an eye on this if any bugs start happening.
            return readString;
        }
    }
}
