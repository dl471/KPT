using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT.Parser;
using KPT.Parser.Instructions;
using KPT.Parser.Headers;
using KPT.Parser.Elements;
using KPT.Parser.Jump_Label_Manager;
using System.Windows.Forms;
using System.Diagnostics;

namespace KPT.Parser.Branch_Mapper
{

    class BranchMapper
    {

        public class BranchComplexity
        {
            public KCFile file;
            public int labels { get; set; }
            public int interFileJumps { get; set; }
            public int intraFileJumps { get; set; }
            public int dialogueBoxes { get; set; }

            public void PrintBranchComplexity()
            {
                Debug.WriteLine("File: " + (file.header as StCp_Header).GetFileNumber());
                Debug.WriteLine("Labels: " + labels);
                Debug.WriteLine("Interfile jumps: " + interFileJumps);
                Debug.WriteLine("Intrafile jumps: " + intraFileJumps);
                Debug.WriteLine("Text boxes: " + dialogueBoxes);
            }

        }

        public BranchComplexity CalculateBranchComplexity(KCFile file)
        {
            var complexity = new BranchComplexity();

            complexity.file = file;

            foreach (var instruction in file.instructions)
            {
                if (instruction is JumpLabel)
                {
                    complexity.labels += 1;
                }
                else if (instruction is InterFileJump)
                {
                    complexity.interFileJumps += 1;
                }
                else if (instruction is IntraFileJump)
                {
                    complexity.intraFileJumps += 1;
                }
                else if (instruction is IDialogueBox)
                {
                    complexity.dialogueBoxes += 1;
                }
            }

            return complexity;

        }

        

    }
}
