using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Instructions;
using System.Windows.Forms;

namespace KPT.Parser.Elements
{
    class ChoiceBar : IElement
    {
        const int CHOICE_STRING_LENGTH = 0x38;

        int choiceNumber;
        string choiceText;

        public bool Read(BinaryReader br)
        {
            choiceNumber = br.ReadUInt16();
            choiceText = FileIOHelper.ReadFixedLengthString(br, CHOICE_STRING_LENGTH);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

    }
}
