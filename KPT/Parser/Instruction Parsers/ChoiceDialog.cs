using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;

namespace KPT.Parser.Instruction_Parsers
{
    class ChoiceDialog : IInstructionParser
    {

        Opcode opcode;
        int numberOfChoices;
        int unknownInt; // possible type of dialog? untested.
        List<ChoiceBar> choices;

        public ChoiceDialog()
        {
            choices = new List<ChoiceBar>();
        }

        public bool Read(BinaryReader br)
        {
            opcode = ElementReader.ReadOpcode(br);
            unknownInt = br.ReadUInt16();
            numberOfChoices = br.ReadUInt16();
            for (int i = 0; i < numberOfChoices; i++)
            {
                var newChoice = new ChoiceBar();
                newChoice.Read(br);
                choices.Add(newChoice);
            }
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            return true;
        }

    }
}
