using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Spreadsheet_Interface;

namespace KPT.Parser.Instructions
{
    class U_502 : IInstruction, IHasName, IHasStrings
    {
        Opcode opcode;
        Box box1;
        string name;
        string dialogue;
        Box box2; // in retrospect this seems like a very strange thing to have - i may have to double check it

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            box1 = new Box(0x0C);
            box1.Read(br);
            name = FileIOHelper.ReadName(br);
            dialogue = FileIOHelper.ReadDialogueString(br);
            box2 = new Box(0x04);
            box2.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            box1.Write(bw);
            FileIOHelper.WriteFixedLengthString(bw, name, Constants.NAME_LENGTH);
            FileIOHelper.WriteDialogueString(bw, dialogue);
            box2.Write(bw);
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
