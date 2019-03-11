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
    class U_502 : IInstruction, IHasName, IHasStrings, IDialogueBox
    {
        Opcode opcode;
        short unknown1;
        byte unknown2;
        byte unknown3;
        int voiceClip;
        int unknown4;
        DialogueBox dialogueBox;
        int unknown5;

        public U_502()
        {
            dialogueBox = new DialogueBox();
        }

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            unknown1 = br.ReadInt16();
            unknown2 = br.ReadByte();
            unknown3 = br.ReadByte();
            voiceClip = br.ReadInt32();
            unknown4 = br.ReadInt32();
            dialogueBox.Read(br);
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
            dialogueBox.Write(bw);
            bw.Write(unknown5);
            return true;
        }

        public string GetName()
        {
            return dialogueBox.GetName();
        }

        public void SetName(string newName)
        {
            dialogueBox.SetName(newName);
        }

        public void AddStrings(StringCollection collection)
        {
            dialogueBox.AddStrings(collection);
        }

        public void GetStrings(StringCollection collection)
        {
            dialogueBox.GetStrings(collection);
        }

        public List<CSVRecord> GetCSVRecords()
        {
            return dialogueBox.GetCSVRecords();
        }

        public bool isTranslated
        {
            get { return dialogueBox.isTranslated; }
            set { dialogueBox.isTranslated = value; }
        }

    }
}
