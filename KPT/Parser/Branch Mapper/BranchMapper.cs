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
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
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

        /// <summary>
        /// A class representing an interfile jump from class A to class B, where the jump will be treated as an edge for graphing purposes
        /// </summary>
        public class InterFileEdge
        {

            public StCpNumber from { get; set; }
            public StCpNumber to { get; set; }
            public int frequency { get; set; }

            public InterFileEdge(StCpNumber from, StCpNumber to)
            {
                this.from = from;
                this.to = to;
                frequency = 1;
            }

            public override string ToString()
            {
                return from.ToString() + "->" + to.ToString();
            }

            public sealed class InterFileEdgeCSVRecord : ClassMap<InterFileEdge>
            {
                public InterFileEdgeCSVRecord()
                {
                    Map(csr => csr.from).Name("From");
                    Map(csr => csr.to).Name("To");
                    Map(csr => csr.frequency).Name("Frequency");
                }
            }

        }

        /// <summary>
        /// Produces a list of interfile jumps between files and the frequency of each jump
        /// </summary>
        /// <param name="files">The files to be examined</param>
        /// <returns>A list of InterFileEdge with the jump data</returns>
        public List<InterFileEdge> CalcuateEdgesInterFile(List<KCFile> files)
        {
            var edgeDict = new Dictionary<string, InterFileEdge>();
            
            foreach (var file in files)
            {
                foreach (var instruction in file.instructions)
                {
                    if (instruction is InterFileJump)
                    {
                        var from = (file.header as StCp_Header).GetFileNumber();
                        var to = (instruction as InterFileJump).fileNumber;
                        var edge = new InterFileEdge(from, to);

                        InterFileEdge fromDict;

                        bool success = edgeDict.TryGetValue(edge.ToString(), out fromDict);

                        if (success)
                        {
                            fromDict.frequency += 1;
                        }
                        else
                        {
                            edgeDict[edge.ToString()] = edge;
                        }

                    }
                }
            }

            return edgeDict.Values.ToList();

        }

        /// <summary>
        /// Export list of edges as a CSV for consumption by graphing scripts
        /// </summary>
        /// <param name="edges">The edges to export to file</param>
        /// <param name="filename">The name of the file to export to</param>
        public void ExportInterFileEdges(List<InterFileEdge> edges, string fileName)
        {

            FileStream fs = new FileStream(fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            var csv = new CsvHelper.CsvWriter(sw);
            csv.Configuration.RegisterClassMap<InterFileEdge.InterFileEdgeCSVRecord>();

            // for some reason writing the header is not working, but it should not matter too much in the end i think
            //csv.WriteHeader(typeof(InterFileEdge.InterFileEdgeCSVRecord));
            //csv.NextRecord();

            foreach (var edge in edges)
            {
                csv.WriteRecord(edge);
                csv.NextRecord();
            }

            csv.Flush();

            sw.Close();
            fs.Close();

        }

    }
}
