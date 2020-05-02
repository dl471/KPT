using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT.Parser.Elements;

namespace KPT.Parser.Jump_Label_Manager
{

    /// <summary>
    /// Class to help manage jump labels
    /// </summary>
    static class JumpLabelManager
    {
        /// <summary>
        /// Map jump ids to entries in the jump table
        /// </summary>
        private static Dictionary<string, JumpTableEntry> jumpLabelMap;

        private static bool initalized = false;

        public static void Initalize(List<JumpTableEntry> jumpTableEntries)
        {
            jumpLabelMap = new Dictionary<string, JumpTableEntry>();

            foreach (var entry in jumpTableEntries)
            {
                string jumpID = entry.GetJumpID();
                jumpLabelMap[jumpID] = entry;
            }

            initalized = true;
        
        }

        public static bool IsJumpTarget(StCpNumber fileNumber, int address)
        {
            if (!initalized)
            {
                throw new Exception("JumpLabelManager not initalized");
            }

            var tempLabel = JumpTableEntry.GenerateJumpID(fileNumber, address);
            return jumpLabelMap.Keys.Contains(tempLabel);
        }

        public static JumpTargetInstructionWrapper wrapInstruction(StCpNumber fileNumber, int address, IInstruction instruction)
        {
            if (!initalized)
            {
                throw new Exception("JumpLabelManager not initalized");
            }

            var jumpLabel = JumpTableEntry.GenerateJumpID(fileNumber, address);
            var jumpTableEntry = jumpLabelMap[jumpLabel];
            var wrappedInstruction = new JumpTargetInstructionWrapper(instruction, jumpTableEntry);
            return wrappedInstruction;
        }

    }
}
