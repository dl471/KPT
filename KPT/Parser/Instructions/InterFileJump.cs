using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;
using KPT.Parser.Spreadsheet_Interface;

namespace KPT.Parser.Instructions
{
    class InterFileJump : IInstruction, IHasName, IHasStrings
    {
        // make a jump to a different file - possibly also uses lookup table in St000_SldtDat.bin
        Opcode opcode;
        public StCpNumber fileNumber { get; private set; }
        // same deal as the look up codes in InterFileJump
        public short firstLookUpCode { get; private set; }
        public short secondLookUpCode { get; private set; }

        public InterFileJump()
        {
            fileNumber = new StCpNumber();
        }

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            fileNumber.Read(br);
            firstLookUpCode = br.ReadInt16();
            secondLookUpCode = br.ReadInt16();
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            fileNumber.Write(bw);
            bw.Write(firstLookUpCode);
            bw.Write(secondLookUpCode);
            return true;
        }

        private String Disassemble() // optimistic name for when the disassembly view is actually finished
        {
            return String.Format("IF FLAG {0} THEN INTERFILE => {1}", this.secondLookUpCode, this.fileNumber.ToString());
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