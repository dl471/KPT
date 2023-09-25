using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;
using KPT.Parser.Headers;

namespace KPT.Parser.Jump_Label_Manager
{
    /// <summary>
    /// Marks an entry in the games jump table
    /// </summary>
    class JumpTableEntry : IElement
    {

        StCpNumber fileNumber;
        short sequentialJumpNumber; // increments by one with each registered jump in a given file - every file has its own counter. may be referred to as sequentialChoiceNumber (SCN) as a legacy name.
        short unknown1; // this appears to always be zero
        short offset; // jump to this offset within the file - note that this is the offset from the end of the header not the start of the file
        short unknown2; // this appears to also always be zero
        short jumpFileNumber; // increments by one with each file registered in the jump table, which means every file has its own unique ID - St000Cp0001 = 0, St000Cp0101 = 1, St000Cp02A1 = 2 , etc. - whether or not this genericizes outside of the domain of the jump table is unknown
        short lookupCode { get; set;  } // increments by one with each jump registered in the jump table, giving each jumpTableEntry its own unique ID - the look up code used by IntraFileJump and possibly also InterFileJump 
        short unknown4;
        short unknown5;
        DataBox trailingBlock; // this block is usually but is not always entirey 0xFF ands its purpose is not clear

        public StCpNumber FileNumber
        {
            get => fileNumber;
        }

        public short SequentialJumoNumber
        {
            get => sequentialJumpNumber;
        }

        public short LookUpCode
        {
            get => lookupCode;
        }

        public short Offset
        {
            get => offset;
        }

        public JumpTableEntry()
        {
            fileNumber = new StCpNumber();
        }

        public bool Read(BinaryReader br)
        {
            fileNumber.Read(br);
            sequentialJumpNumber = br.ReadInt16();
            unknown1 = br.ReadInt16();
            offset = br.ReadInt16();
            offset = HandleOffsetOnRead(offset); // since the offset is counted from the end of the header not the start of the file we make our own adjustment to it to turn into a real offset from the start of the file. since we will be dealing with full file streams this makes it easier to reason about and recalcuate the offets.
            unknown2 = br.ReadInt16();
            jumpFileNumber = br.ReadInt16();
            lookupCode = br.ReadInt16();
            unknown4 = br.ReadInt16();
            unknown5 = br.ReadInt16();
            trailingBlock = new DataBox(0x82);
            trailingBlock.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            fileNumber.Write(bw);
            bw.Write(sequentialJumpNumber);
            bw.Write(unknown1);
            offset = HandleOffsetOnWrite(offset);
            bw.Write(offset);
            bw.Write(unknown2);
            bw.Write(jumpFileNumber);
            bw.Write(lookupCode);
            bw.Write(unknown4);
            bw.Write(unknown5);
            trailingBlock.Write(bw);
            return true;
        }

        private short HandleOffsetOnRead(short offset)
        {
            return (short)(offset + StCp_Header.HEADER_SIZE);
        }

        private short HandleOffsetOnWrite(short offset)
        {
           return (short)(offset - StCp_Header.HEADER_SIZE);
        }

        public static string GenerateJumpID(StCpNumber fileNumber, int jumpTargetAddress)
        {
            return fileNumber.ToString() + ":" + jumpTargetAddress.ToString("X");
        }

        public string GetJumpID()
        {
            return GenerateJumpID(fileNumber, offset);
        }

        public void UpdateOffset(short newOffset)
        {
            offset = newOffset;
        }

    }
}
