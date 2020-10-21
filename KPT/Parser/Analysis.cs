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
using System.Linq;
using System.Diagnostics;
using System.IO;

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
        /// Instruction close to a jump (source of or target of) and its associated dialogue
        /// </summary>
        /// <remarks>From a user perspective it seems like the jumps are going from one line of dialogue to another not one from instruction to another, so this is a more true representation of what they expect to see</remarks>
        public class JumpAdjacentInstruction
        {
            public StCpNumber fileNumber { get; set; }
            public InstructionMetaDataWrapper instruction { get; set; }
            public string dialogue { get; set; }
            public string dialogueID { get; set; }

            public JumpAdjacentInstruction(StCpNumber fileNumber, InstructionMetaDataWrapper instruction, string dialogue, string dialogueID)
            {
                this.fileNumber = fileNumber;
                this.instruction = instruction;
                this.dialogue = dialogue;
                this.dialogueID = dialogueID;
            }

            public JumpAdjacentInstruction(KCFile file, InstructionMetaDataWrapper wrappedInstruction)
            {
                this.fileNumber = (file.header as Headers.StCp_Header).GetFileNumber();
                this.instruction = wrappedInstruction;

                var workingInstruction = wrappedInstruction.wrappedInstruction as IDialogueBox;
                this.dialogue = workingInstruction.GetDialogue();
                this.dialogueID = "N/A"; // it is at this point i remember that dialogue and dialogueID occupy the same space in the instruction, making it functionally equivalent to a union, making it extremely tedious and/or requiring two passes of the entiretey of the games dialogue to get them both. perhaps in due time.
            }

        }

        /// <summary>
        /// Jump that has been processed to be a reference to choice boxes/text boxes instead of jump instructions and jump labels
        /// </summary>
        public class TranslatedJump
        {
            public JumpAdjacentInstruction source { get; set; }
            public JumpAdjacentInstruction target { get; set; }

            public TranslatedJump(JumpAdjacentInstruction source, JumpAdjacentInstruction target)
            {
                this.source = source;
                this.target = target;
            }
        }

        public static LinkedListNode<InstructionMetaDataWrapper> FindNextDialogueBox(LinkedListNode<InstructionMetaDataWrapper> node)
        {
            for (var currentNode = node; currentNode.Next != null; currentNode = currentNode.Next)
            {
                if (currentNode.Value.wrappedInstruction is IntraFileJump)
                {
                    throw new Exception("Second branch occured before next dialogue box");
                }
                if (currentNode.Value.wrappedInstruction is IDialogueBox)
                {
                    return currentNode;
                }
            }

            return null;  // unresolveable - perhaps connected to an interfile jump further down the chain, but we will investigate this later

        }

        public static JumpAdjacentInstruction ResolveIntraFileJumpTarget(KCFile originFile, WrappedInstructionList wrappedInstructionList, JumpTableInterface jumpTable, IntraFileJump jumpOrigin)
        {
            var jumpTrigger = jumpOrigin;
            var jumpLookUpCode = jumpTrigger.secondLookUpCode;

            JumpAdjacentInstruction resolvedTargetInstruction = null;

            if (jumpLookUpCode < 0)
            {
                // continue; 
                // there is precisely one instruction with a global look up code of -1 and it seems to be related to the endings so i have no idea where it goes
            }
            else
            {
                var targetJump = jumpTable.GetJumpTableEntryByGlobalLookupCode(jumpLookUpCode);

                if (targetJump.FileNumber.ToString() != (originFile.header as Headers.StCp_Header).GetFileNumber().ToString())
                {
                    throw new Exception(string.Format("Intrafile jump tried to peform an interfile jump"));
                }

                var targetAddress = targetJump.Offset;
                var targetInstructionNode = wrappedInstructionList.addressToInstruction[targetAddress];
                var targetInstruction = targetInstructionNode.Value;
                var nextDialogueBox = FindNextDialogueBox(targetInstructionNode);                

                if (nextDialogueBox == null)
                {
                    resolvedTargetInstruction = new JumpAdjacentInstruction(
                        (originFile.header as Headers.StCp_Header).GetFileNumber(),
                        null,
                        "UNRESOLVABLE",
                        null);
                }
                else
                {
                    resolvedTargetInstruction = new JumpAdjacentInstruction(originFile, nextDialogueBox.Value);
                }
            }

            return resolvedTargetInstruction;
        }

        public static void MapIntraFileJumps(KCFile file, JumpTableInterface jumpTable)
        {
            var instructionList = file.instructions;
            var wrappedInstructionList = new WrappedInstructionList(instructionList);
            var wrappedInstructionListNodes = wrappedInstructionList.wrappedInstructions;

            var firstNode = wrappedInstructionListNodes.First;
            var currentNode = firstNode;

            while (currentNode != null)
            {
                var currentInstruction = currentNode.Value.wrappedInstruction;

                if (currentInstruction is ChoiceDialog)
                {
                    var choiceDialog = currentInstruction as ChoiceDialog;
                    foreach (var choice in choiceDialog.GetChoices())
                    {
                        JumpAdjacentInstruction resolvedOrigin = new JumpAdjacentInstruction((file.header as Headers.StCp_Header).GetFileNumber(), currentNode.Value, choice.GetChoiceText(), "N/A");
                        JumpAdjacentInstruction resolvedTarget = null;

                        currentNode = currentNode.Next;
                        var nextInstruction = currentNode.Value.wrappedInstruction;
                        if (nextInstruction is IntraFileJump)
                        {
                            resolvedTarget = ResolveIntraFileJumpTarget(file, wrappedInstructionList, jumpTable, nextInstruction as IntraFileJump);
                        }
                        else if (nextInstruction is InterFileJump)
                        {
                            // we can't resolve these yet
                        }
                        else
                        {
                            throw new Exception("Choice bar without associated jump encountered");
                        }
                       
                    }

                }

                else if (currentInstruction is IntraFileJump)
                {
                    var resolvedTarget = ResolveIntraFileJumpTarget(file, wrappedInstructionList, jumpTable, currentInstruction as IntraFileJump);
                }

                currentNode = currentNode.Next;

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
