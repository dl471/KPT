using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Spreadsheet_Interface;
using KPT.Parser.Jump_Label_Manager;

namespace KPT.Parser.Instructions
{
    class IntraFileJump : IInstruction, IHasName, IHasStrings
    {
        // make a jump within the same file - appears to use a lookup table in St000_SldtDat.bin
        Opcode opcode;
        // one of these shorts is a look up code and the other is unknown. sometimes they are the same and sometimes they are different. so there is some currently unknown logic that may affect branching in an unpredictable way.
        public short firstLookUpCode { get; private set; }
        public short secondLookUpCode { get; private set; }

        public VirtualLabel target { get; set; } // the virtual label  that the jump is targeted to after processing

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            firstLookUpCode = br.ReadInt16();
            secondLookUpCode = br.ReadInt16();
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            bw.Write(firstLookUpCode);
            bw.Write(secondLookUpCode);
            return true;
        }

        private String Disassemble() // optimistic name for when the disassembly view is actually finished
        {
            if (this.secondLookUpCode == -1)
            {
                // there is a very specific bug with one very specific jump
                // -1 does not appear during manual traversal of the files when normally parsed so it must be getting introuced somewhere
                return String.Format("INTRAFILE -1 PANIC"); 
            }
            else
            {
                return String.Format("IF FLAG {0} THEN INTRAFILE => {1}.LABEL_{2}", this.firstLookUpCode, target.fileNumber.ToString(), target.fileJumpNumber.ToString());
            }
        }

        // implementing this as IHasStrings and IHasName so that DumpStrings and CSVWriter catch it and dump out the breaks in the game script

        public string GetName()
        {
            return "[disasm]"; // this needs to be a constant somewhere
        }

        public void SetName(string newName)
        {
            throw new NotImplementedException();
        }

        public void AddStrings(StringCollection collection)
        {
            string newID = collection.GenerateNewID();
            collection.AddString(newID, this.Disassemble());
        }

        public void GetStrings(StringCollection collection)
        {

        }

        public List<CSVRecord> GetCSVRecords()
        {
            return new List<CSVRecord> { new CSVRecord(GetName(), Disassemble()) };
        }


    }
}
