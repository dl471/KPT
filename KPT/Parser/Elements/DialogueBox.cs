using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Instructions;
using KPT.Parser.Spreadsheet_Interface;

namespace KPT.Parser.Elements
{
    class DialogueBox : IElement, IHasName, IHasStrings
    {
        string name;
        string dialogue;

        public bool Read(BinaryReader br)
        {
            name = FileIOHelper.ReadName(br);
            dialogue = FileIOHelper.ReadDialogueString(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            FileIOHelper.WriteFixedLengthString(bw, name, Constants.NAME_LENGTH);
            FileIOHelper.WriteDialogueString(bw, dialogue);
            return true;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string newName)
        {
            name = newName;
        }

        public void AddStrings(StringCollection collection)
        {
            string newID = collection.GenerateNewID();
            collection.AddString(newID, dialogue);
            dialogue = newID;
        }

        public void GetStrings(StringCollection collection)
        {
            dialogue = collection.GetString(dialogue);
        }

        public List<CSVRecord> GetCSVRecords()
        {
            return new List<CSVRecord> { new CSVRecord(name, dialogue) };
        }

    }
}
