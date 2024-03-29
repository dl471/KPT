﻿using System;
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
    class JumpLabelManager
    {
        /// <summary>
        /// Map jump ids to entries in the jump table
        /// </summary>
        private Dictionary<string, JumpTableEntry> jumpLabelMap;
        /// <summary>
        /// Track the labels in a specific file
        /// </summary>
        private Dictionary<StCpNumber, List<VirtualLabel>> fileJumpTargets;
        /// <summary>
        /// Maps a global lookup code to a specific virtual label in an already processed file
        /// </summary>
        private Dictionary<int, VirtualLabel> globalLookUpCodeMap;

        public JumpLabelManager(List<JumpTableEntry> jumpTableEntries)
        {

            jumpLabelMap = new Dictionary<string, JumpTableEntry>();
            fileJumpTargets = new Dictionary<StCpNumber, List<VirtualLabel>>();
            globalLookUpCodeMap = new Dictionary<int, VirtualLabel>();

            foreach (var entry in jumpTableEntries)
            {
                string jumpID = entry.GetJumpID();
                jumpLabelMap[jumpID] = entry;
            }
        
        }

        public bool IsJumpTarget(StCpNumber fileNumber, int address)
        {
            var tempLabel = JumpTableEntry.GenerateJumpID(fileNumber, address);
            return jumpLabelMap.Keys.Contains(tempLabel);
        }

        public VirtualLabel CreateVirtualLabel(StCpNumber fileNumber, int address, short lookUpCode)
        {

            // so that we can rebuild the games jump table wit ths info
            var jumpLabel = JumpTableEntry.GenerateJumpID(fileNumber, address);
            var jumpTableEntry = jumpLabelMap[jumpLabel];

            // so that we can replace the global lookup code with a local label in disassembly and CSV
            // e.g. INTERFILE_JUMP 111 => INTERFILE_JUMP StCpSt006_Cp0701.LABEL_5 (fake, not the translaton)
            List<VirtualLabel> labelList = null;
            bool success = fileJumpTargets.TryGetValue(fileNumber, out labelList);

            if (!success)
            {
                labelList = new List<VirtualLabel>();
                fileJumpTargets[fileNumber] = labelList;
            }
            var jumpNumber = labelList.Count + 1;
            var virtualLabel = new VirtualLabel(jumpTableEntry, fileNumber, jumpNumber);
            labelList.Add(virtualLabel);
            globalLookUpCodeMap[lookUpCode] = virtualLabel;          

            return virtualLabel;
        }

        public VirtualLabel GetVirtualLabelByGlobalLookUpCode(int globalLookUpCode)
        {

            VirtualLabel virtualLabel = null;
            bool success = globalLookUpCodeMap.TryGetValue(globalLookUpCode, out virtualLabel);

            if(!success && globalLookUpCode != -1)
            {
                Console.WriteLine(String.Format("Virtual label with global lookup code {0} not be found", globalLookUpCode));
            }

            return virtualLabel;
        }

    }
}
