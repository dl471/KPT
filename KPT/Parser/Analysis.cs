using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT.Parser;
using KPT.Parser.Instructions;
using KPT.Parser.Elements;
using KPT.Parser.Jump_Label_Manager;
using System.Windows.Forms;

namespace KPT.Parser
{
    /// <summary>
    /// Contains various snippets of code used to analyze files and confirm/disprove various hypotheses about the game's logic - can also subsequently function as an integtiry checker
    /// </summary>
    static class Analysis
    {

        /// <summary>
        /// A wrapper around an instruction containing data on its position in a file
        /// </summary>
        /// <remarks>Used to attach data on instruction number/instruction address to an instruction without bloating IInstruction</remarks>
        public class InstructionMetaDataWrapper
        {
            public long instructionNumber { get; private set; }
            public long address { get; private set; }
            public IInstruction wrappedInstruction { get; private set; }

            public InstructionMetaDataWrapper(long instructionNumber, long address, IInstruction wrappedInstruction)
            {
                this.instructionNumber = instructionNumber;
                this.address = address;
                this.wrappedInstruction = wrappedInstruction;
            }

        }

        /// <summary>
        /// List of IInstructon converted to linked list and address->instruction / instruction->address map
        /// </summary>
        /// <remarks>Used to simplify forward/backward traversal of instruction list and instruction look up by address for jump mapping</remarks>
        public class WrappedInstructionList // should be renamed or perhaps outright refactored, though i think the list and the map should stay together
        {
            public Dictionary<LinkedListNode<InstructionMetaDataWrapper>, long> instructionToAddress { get; private set; }
            public Dictionary<long, LinkedListNode<InstructionMetaDataWrapper>> addressToInstruction { get; private set; }
            public LinkedList<InstructionMetaDataWrapper> wrappedInstructions { get; private set; }

            public WrappedInstructionList(List<IInstruction> instructionList)
            {
                instructionToAddress = new Dictionary<LinkedListNode<InstructionMetaDataWrapper>, long>();
                addressToInstruction = new Dictionary<long, LinkedListNode<InstructionMetaDataWrapper>>();
                wrappedInstructions = new LinkedList<InstructionMetaDataWrapper>();

                WrapInstructions(instructionList);
            }

            private void WrapInstructions(List<IInstruction> instructionList)
            {
                long currentFileOffset = 0x60; // starting at 0x60 to account for StCp header
                long instructionCounter = 0;

                foreach (var instruction in instructionList)
                {
                    var instructionSize = instruction.GetElementSize();
                    var instructionAddress = currentFileOffset;

                    var newWrapper = new InstructionMetaDataWrapper(instructionCounter, instructionAddress, instruction);
                    var newNode = wrappedInstructions.AddLast(newWrapper);

                    instructionToAddress[newNode] = instructionAddress;
                    addressToInstruction[instructionAddress] = newNode;

                    currentFileOffset = currentFileOffset + instructionSize; // current address + size of current instruction = address of next instruction
                    instructionCounter += 1;
                }

            }

        }

        /// <summary>
        /// Iterate through the jump table entries and verify that each global look up code is unique
        /// </summary>
        public static void VerifyJumpTableGlobalCodes(JumpTableInterface jumpTable)
        {
            HashSet<int> lookUpCodes = new HashSet<int>();

            foreach (var entry in jumpTable.GetJumpTableEntries())
            {
                var lookupCode = entry.LookUpCode;
                if (lookUpCodes.Contains(lookupCode))
                {
                    throw new Exception(string.Format("Non-unique lookup code: lookup code {0} has been reused", lookupCode));
                }
                lookUpCodes.Add(lookupCode);

            }


        }
    }
}
