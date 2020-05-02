using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;

namespace KPT.Parser.Jump_Label_Manager
{
    /// <summary>
    /// Class that contains an instruction which is the target of a jump
    /// </summary>
    /// <remarks>
    /// Slotted into instruction lists as a replacement for instructions which are the targets of jumps. Allows the jump table to updated as Write calls are made.
    /// </remarks>
    class JumpTargetInstructionWrapper : IInstruction
    {
        int currentAddress;
        IInstruction wrappedInstruction;
        JumpTableEntry pairedEntry;

        public JumpTargetInstructionWrapper(IInstruction toWrap, JumpTableEntry toPair)
        {
            wrappedInstruction = toWrap;
            pairedEntry = toPair;
        }

        public bool Read(BinaryReader br)
        {
            currentAddress = (int)br.BaseStream.Position;
            wrappedInstruction.Read(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            currentAddress = (int)bw.BaseStream.Position;
            pairedEntry.UpdateOffset((short)currentAddress);
            wrappedInstruction.Write(bw);
            return true;
        }

    }
}
