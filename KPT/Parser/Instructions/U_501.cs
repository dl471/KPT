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
    class U_501 : IInstruction, IHasName, IHasStrings, IDialogueBox
    {

        Opcode opcode;
        SpriteInfo firstSprite;
        SpriteInfo secondSprite;
        SpriteInfo thirdSprite;
        int voiceClip;
        int unknown;
        DialogueBox dialogueBox;

        public U_501()
        {
            dialogueBox = new DialogueBox();
        }

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            firstSprite = new SpriteInfo();
            firstSprite.Read(br);
            secondSprite = new SpriteInfo();
            secondSprite.Read(br);
            thirdSprite = new SpriteInfo();
            thirdSprite.Read(br);
            voiceClip = br.ReadInt32();
            unknown = br.ReadInt32();
            dialogueBox.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            firstSprite.Write(bw);
            secondSprite.Write(bw);
            thirdSprite.Write(bw);
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
