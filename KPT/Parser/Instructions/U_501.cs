using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using KPT.Parser.Spreadsheet_Interface;
using KPT.Parser.Elements;

namespace KPT.Parser.Instructions
{
    class U_501 : IInstruction, IHasName, IHasStrings
    {

        Opcode opcode;
        byte unknown1;
        byte unknown2;
        byte unknown3;
        byte unknown4;
        byte unknown5;
        byte unknown6;
        byte unknown7;
        byte unknown8;
        byte unknown9;
        byte unknown10;
        byte unknown11;
        byte unknown12;
        int voiceClip;
        int unknown13;
        string name;
        string dialogue;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            unknown1 = br.ReadByte();
            unknown2 = br.ReadByte();
            unknown3 = br.ReadByte();
            unknown4 = br.ReadByte();
            unknown5 = br.ReadByte();
            unknown6 = br.ReadByte();
            unknown7 = br.ReadByte();
            unknown8 = br.ReadByte();
            unknown9 = br.ReadByte();
            unknown10 = br.ReadByte();
            unknown11 = br.ReadByte();
            unknown12 = br.ReadByte();
            voiceClip = br.ReadInt32();
            unknown13 = br.ReadInt32();
            name = FileIOHelper.ReadName(br);
            dialogue = FileIOHelper.ReadDialogueString(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            bw.Write(unknown1);
            bw.Write(unknown2);
            bw.Write(unknown3);
            bw.Write(unknown4);
            bw.Write(unknown5);
            bw.Write(unknown6);
            bw.Write(unknown7);
            bw.Write(unknown8);
            bw.Write(unknown9);
            bw.Write(unknown10);
            bw.Write(unknown11);
            bw.Write(unknown12);
            bw.Write(voiceClip);
            bw.Write(unknown13);
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
