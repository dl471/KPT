﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Headers
{
    /// <summary>
    /// A more generic header used by other members of the St000 family - its purpose is entirely unknown
    /// </summary>
    class St_Header : IHeader
    {
        public const int HEADER_SIZE = 0x60;

        short fileNumber;
        short stNumber; // second uint16, not currently handled
        DataBox box1;

        public bool Read(BinaryReader br)
        {
            fileNumber = br.ReadInt16();
            box1 = new DataBox(HEADER_SIZE-2); // Header size seems to be 0x60 overall, so we read the first int16 then shove the rest in a Box. There seems to be a bit more to the header that can be used to validate but I'm skipping that at the moment.
            box1.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write(unknown);
            return true;
        }
    }

}
