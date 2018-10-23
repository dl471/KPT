using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instruction_Parsers
{

    /// <summary>
    /// Used as a hands off container for parts of instructions that are not understood. Intended to read and write data without modifying it or handling it in any way.
    /// </summary>
    class Box : IInstructionParser
    {
        byte[] boxContents;
        int contentsSize;

        public Box (int bytesToRead)
        {
            contentsSize = bytesToRead;
        }

        public byte[] GetContents()
        {
            return boxContents;
        }

        public int GetContentsSize()
        {
            return contentsSize;
        }

        public bool Read(BinaryReader br)
        {
            boxContents = br.ReadBytes(contentsSize);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write(boxContents);
            return true;
        }

    }
}
