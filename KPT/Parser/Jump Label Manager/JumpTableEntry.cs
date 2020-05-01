using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;

namespace KPT.Parser.Jump_Label_Manager
{
    /// <summary>
    /// Marks an entry in the games jump table
    /// </summary>
    class JumpTableEntry : IElement
    {

        StCpNumber fileNumber;
        DataBox firstBlock; // this block contains stuff known to be related to the jumps and will be broken down into named variables shortly
        DataBox secondBlock; // this block is usually but is not always entirey 0xFF ands its purpose is not clear

        public bool Read(BinaryReader br)
        {
            fileNumber.Read(br);
            firstBlock = new DataBox(0x12);
            secondBlock = new DataBox(0x82);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            fileNumber.Write(bw);
            firstBlock.Write(bw);
            secondBlock.Write(bw);
            return true;
        }

    }
}
