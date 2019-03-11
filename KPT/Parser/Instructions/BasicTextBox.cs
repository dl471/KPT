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
    class BasicTextBox : IInstruction, IHasName, IHasStrings, IDialogueBox
    {
        Opcode opcode;
        int voiceClip;
        int unknown;
        DialogueBox dialogueBox;

        public BasicTextBox()
        {
            dialogueBox = new DialogueBox();
        }

        /// <summary>
        /// Fill with default contents - used for creating entirely new dialogue boxes
        /// </summary>
        public void InitalizeDefault()
        {
            opcode = Opcode.BASIC_TEXT_BOX;
            voiceClip = 999999; // deliberately non-existent
            unknown = 0;
        }

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            voiceClip = br.ReadInt32();
            unknown = br.ReadInt32();
            dialogueBox.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            bw.Write(voiceClip);
            bw.Write(unknown);
            dialogueBox.Write(bw);
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

        public string GetDialogue()
        {
            return dialogueBox.GetDialogue();
        }

        public void SetDialogue(string newDialogue)
        {
            dialogueBox.SetDialogue(newDialogue);
        }

    }
}
