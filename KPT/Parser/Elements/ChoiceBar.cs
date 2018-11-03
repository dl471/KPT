using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Instructions;
using System.Windows.Forms;
using KPT.Parser.Spreadsheet_Interface;

namespace KPT.Parser.Elements
{
    class ChoiceBar : IElement, IHasStrings
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

        public void AddStrings(StringCollection collection)
        {
            string newID = collection.GenerateNewID();
            collection.AddString(newID, choiceText);
            choiceText = newID;
        }

        public void GetStrings(StringCollection collection)
        {
            choiceText = collection.GetString(choiceText);
        }

        public List<CSVRecord> GetCSVRecords()
        {
            return new List<CSVRecord> { new CSVRecord("Choice", choiceText) };
        }
    }
}
