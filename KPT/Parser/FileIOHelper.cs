using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Instructions
{
    /// <summary>
    /// Provides a set of functions to make reading and writing files easier for instruction and element classes
    /// </summary>
    static class FileIOHelper
    {
        const int NAME_LENGTH = 20; // I am starting to consider that this really should not be part of the element reader and should be more directly attached to the string instructions... if they can be succintly grouped together, that is. Perhaps once there is more complete picture of how things work.

        public static Opcode ReadOpcode(BinaryReader br)
        {
            int opcode = br.ReadInt16();

            if (Enum.IsDefined(typeof(Opcode), opcode))
            {
                return (Opcode)opcode;
            }

            throw new Exception("Invalid opcode");

            return Opcode.INVALID; // perhaps there should be a second "Invalid Opcode" error message here?
        }

        public static string ReadName(BinaryReader br) // consider folding this into ReadFixedLengthString(NAME_LENGTH) since that is basically all it is
        {
            byte[] nameAsBytes = br.ReadBytes(NAME_LENGTH);
            return ActiveEncodings.currentEncoding.GetString(nameAsBytes);
        }

        public static string ReadNullTerminatedString(BinaryReader br)
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

            br.ReadByte(); // consume null terminator
            return readString;
        }
        
        public static string ReadDialogueString(BinaryReader br)
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

        public static string ReadFixedLengthString(BinaryReader br, int length)
        {
            byte[] stringAsBytes = br.ReadBytes(length);
            string readString = ActiveEncodings.currentEncoding.GetString(stringAsBytes);

            return readString;
        }

        public static void WriteStringNullTerminated(BinaryWriter bw, string stringToWrite)
        {
            byte[] stringAsBytes = ActiveEncodings.currentEncoding.GetBytes(stringToWrite);
            bw.Write(stringAsBytes);
            bw.Write((byte)0x00);
        }

        public static void WriteDialogueString(BinaryWriter bw, string stringToWrite)
        {
            byte[] stringAsBytes = ActiveEncodings.currentEncoding.GetBytes(stringToWrite);
            bw.Write(stringAsBytes);
            bw.Write((byte)0x00);
            bw.Write((byte)0x00);
        }

    }
}
