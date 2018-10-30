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

        public static string StripTrailingNulls(string toStrip)
        {
            int i;

            for (i = 0; i < toStrip.Length; i++)
            {
                char temp = toStrip[i];
                if (temp == '\0')
                {
                    break;
                }
            }
            
            if (i == toStrip.Length)
            {
                return toStrip;
            }
            else
            {
                return toStrip.Substring(0, i);
            }
            
        }

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
            byte[] nameAsBytes = br.ReadBytes(Constants.NAME_LENGTH);
            string name = ActiveEncodings.currentEncoding.GetString(nameAsBytes);
            return StripTrailingNulls(name);
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

            return StripTrailingNulls(readString);
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

        public static void WriteFixedLengthString(BinaryWriter bw, string stringToWrite, int length)
        {
            byte[] stringAsBytes = ActiveEncodings.currentEncoding.GetBytes(stringToWrite);
            if (stringAsBytes.Length > length)
            {
                throw new Exception(string.Format("String {0} was too long", stringToWrite));
            }
            byte[] fixedLengthArray = new byte[length]; // arrays will initalize to null
            Array.Copy(stringAsBytes, 0, fixedLengthArray, 0, stringAsBytes.Length);
            bw.Write(fixedLengthArray);
        }

    }
}
