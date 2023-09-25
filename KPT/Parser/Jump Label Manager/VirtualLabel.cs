using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;
using KPT.Parser.Spreadsheet_Interface;

namespace KPT.Parser.Jump_Label_Manager
{
    /// <summary>
    /// A fake instruction representing the target of a jump
    /// </summary>
    /// <remarks>
    /// Slotted into instruction lists in front of instructions that are targets of jumps. Allows the jump table to updated as Write calls are made.
    /// </remarks>
    class VirtualLabel : IInstruction, IHasName, IHasStrings
    {
        int currentAddress;
        JumpTableEntry pairedEntry;
        int globalJumpLookupCode;

        StCpNumber fileNumber { get; set; } // the file the jump belongs to
        int fileJumpNumber { get; set; } // the order in which this jump apppears in its file

        public VirtualLabel(JumpTableEntry toPair, StCpNumber fileNumber, int fileJumpNumber)
        {
            this.pairedEntry = toPair;
            this.fileNumber = fileNumber;
            this.fileJumpNumber = fileJumpNumber;
            this.globalJumpLookupCode = pairedEntry.LookUpCode;
        }

        public bool Read(BinaryReader br)
        {
            currentAddress = (int)br.BaseStream.Position;
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            currentAddress = (int)bw.BaseStream.Position;
            pairedEntry.UpdateOffset((short)currentAddress);
            return true;
        }

        private String Disassemble() // optimistic name for when the disassembly view is actually finished
        {
            return String.Format("{0}.LABEL_{1}", this.fileNumber.ToString(), this.fileJumpNumber.ToString());
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
