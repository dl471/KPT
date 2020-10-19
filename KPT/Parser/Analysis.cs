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

            MessageBox.Show(lookUpCodes.Count.ToString());

        }
    }
}
