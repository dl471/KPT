using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instructions
{
    // This appears to be some "fetch X file from Y archive function" based on an initial look at the arguments. Having actually run it, it seems to be an image display. That said, it's still possible it has other functionality yet undiscovered.
    class ShowImage : IInstruction
    {
        Opcode opcode;
        string archiveFile;
        string subFile;

        private const int cpkFileNameLength = 0x20;
        private const int imageFileNameLength = 0x20;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            archiveFile = FileIOHelper.ReadFixedLengthString(br, cpkFileNameLength);
            subFile = FileIOHelper.ReadFixedLengthString(br, imageFileNameLength);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            FileIOHelper.WriteFixedLengthString(bw, archiveFile, cpkFileNameLength);
            FileIOHelper.WriteFixedLengthString(bw, subFile, imageFileNameLength);
            return true;
        }

    }
}
