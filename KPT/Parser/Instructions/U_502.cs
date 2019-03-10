using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Spreadsheet_Interface;
using KPT.Parser.Elements;

namespace KPT.Parser.Instructions
{
    class U_502 : IInstruction, IHasName, IHasStrings
    {
        Opcode opcode;
        short unknown1;
        byte unknown2;
        byte unknown3;
        int voiceClip;
        int unknown4;
        string name;
        string dialogue;
        int unknown5;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            unknown1 = br.ReadInt16();
            unknown2 = br.ReadByte();
            unknown3 = br.ReadByte();
            voiceClip = br.ReadInt32();
            unknown4 = br.ReadInt32();
            name = FileIOHelper.ReadName(br);
            dialogue = FileIOHelper.ReadDialogueString(br);
            unknown5 = br.ReadInt32();
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            bw.Write(unknown1);
            bw.Write(unknown2);
            bw.Write(unknown3);
            bw.Write(voiceClip);
            bw.Write(unknown4);
            FileIOHelper.WriteFixedLengthString(bw, name, Constants.NAME_LENGTH);
            FileIOHelper.WriteDialogueString(bw, dialogue);
            bw.Write(unknown5);
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
