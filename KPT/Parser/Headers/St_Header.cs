using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT.Parser.Instructions;
using KPT.Parser.Elements;
using System.IO;

namespace KPT.Parser.Headers
{
    /// <summary>
    /// Used to read the header of files following the StXXX_CpXXXX naming convention.
    /// </summary>
    class St_Header : IHeader
    {
        int fileNumber;
        int stNumber; // second uint16, not currently handled
        Box box1;

        public bool Read(BinaryReader br)
        {
            fileNumber = br.ReadUInt16();
            box1 = new Box(0x5E); // Header size seems to be 0x60 overall, so we read the first int16 then shove the rest in a Box. There seems to be a bit more to the header that can be used to validate but I'm skipping that at the moment.
            box1.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}
