using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;
using KPT.Parser.Spreadsheet_Interface;

namespace KPT.Parser.Instructions
{
    class ChoiceDialog : IInstruction, IHasStrings
    {

        Opcode opcode;
        int unknownInt; // possible type of dialog? untested.
        int numberOfChoices;
        List<ChoiceBar> choices;

        public ChoiceDialog()
        {
            choices = new List<ChoiceBar>();
        }

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
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
            bw.Write((short)opcode);
            bw.Write((short)unknownInt);
            bw.Write((short)numberOfChoices);
            foreach (var choice in choices)
            {
                choice.Write(bw);
            }
            return true;
        }

        public void AddStrings(StringCollection collection)
        {
            foreach (var choice in choices)
            {
                choice.AddStrings(collection);
            }
        }

        public void GetStrings(StringCollection collection)
        {
            foreach (var choice in choices)
            {
                choice.GetStrings(collection);
            }
        }

        public List<CSVRecord> GetCSVRecords()
        {
            var records = new List<CSVRecord>();
            foreach (var choice in choices)
            {
                foreach (var entry in choice.GetCSVRecords())
                {
                    records.Add(entry);
                }
            }
            return records;
        }

    }
}
