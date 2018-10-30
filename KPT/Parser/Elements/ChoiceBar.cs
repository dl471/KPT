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
        

        int choiceNumber;
        string choiceText;

        public bool Read(BinaryReader br)
        {
            choiceNumber = br.ReadUInt16();
            choiceText = FileIOHelper.ReadFixedLengthString(br, Constants.CHOICE_STRING_LENGTH);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)choiceNumber);
            FileIOHelper.WriteFixedLengthString(bw, choiceText, Constants.CHOICE_STRING_LENGTH);
            return true;
        }

    }
}
