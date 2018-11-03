using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instructions
{
    /// <summary>
    /// Used as a hands off container for instructions that are not understood. Intended to read and write data without modifying it or handling it in any way.
    /// </summary>
    class InstructionBox : IInstruction
    {
        Opcode opcode;
        byte[] boxContents;
        int contentsSize;

        public InstructionBox(int bytesToRead)
        {
            contentsSize = bytesToRead - Constants.OPCODE_SIZE;
        }

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            boxContents = br.ReadBytes(contentsSize);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            bw.Write(boxContents);
            return true;
        }

    }
}
