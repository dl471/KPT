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
using System.Linq;

namespace KPT.Parser.Branch_Mapper
{

    class BranchMapper
    {

        private Dictionary<string, KCFile> fileDict = null; // practically global state, because the edge dict being numbers only was a questionable decision later on

        private void BuildFileDict(List<KCFile> files)
        {
            fileDict = new Dictionary<string, KCFile>();
            foreach (var file in files)
            {
                var fileNumber = (file.header as StCp_Header).GetFileNumber().ToString();
                fileDict[fileNumber] = file;
                Debug.WriteLine(fileNumber);
            }
        }

        public BranchMapper()
        {

        }


        public class BranchComplexity
        {
            public KCFile file; // why does branch complexity keep a copy of the file? if anything, branch complexity itsel could be rolled into KCFile as a whole, consider this
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

        public bool IsSimpleInterFileJump(BranchComplexity from, BranchComplexity to)
        {
            return (from.intraFileJumps == 0 && to.intraFileJumps == 0 && from.interFileJumps == 1);
        }

        public bool IsSimpleInterFileJump(KCFile from, KCFile to)
        {
            return IsSimpleInterFileJump(CalculateBranchComplexity(from), CalculateBranchComplexity(to));
        }

        public bool IsSimpleInterFileJump(InterFileEdge edge)
        {
            if (fileDict == null)
            {
                throw new Exception("File dict not built");
            }

            return IsSimpleInterFileJump(fileDict[edge.from.ToString()], fileDict[edge.to.ToString()]);

        }

        /// <summary>
        /// Check whether two BranchComplexities represent a simple interfile jump
        /// </summary>
        /// <param name="from">The source file of the jump</param>
        /// <param name="to">The target file of a jump</param>
        /// <param name="edgeList">The edge list to compare against</param>
        /// <returns></returns>
        /// <remarks>
        /// A simple interfile jump is a jump from a file with no intrafile jumps and one interfile jump to another file with no intrafile jumps which is the target of one jump only.
        /// Such files can be merged by removing the from file's interfile jump appending the to file to the from file for translation purposes with no risk.
        /// Though such files should not be reintroduced into the game proper.
        /// </remarks>
        public bool CheckMergabilitySimple(BranchComplexity from, BranchComplexity to, List<InterFileEdge> edgeList)
        {
            if (!IsSimpleInterFileJump(from, to))
            {
                return false; // we don't want to risk making things more complicated, so use simple files only
            }

            StCpNumber mergeSourceNumber = (from.file.header as StCp_Header).GetFileNumber();
            StCpNumber mergeTargetNumber = (to.file.header as StCp_Header).GetFileNumber();

            foreach (var edge in edgeList)
            {
                var edgeSource = edge.from;
                var edgeTarget = edge.to;

                if (edgeTarget.Equals(mergeTargetNumber))
                {

                    if (!edgeSource.Equals(mergeSourceNumber))
                    //if (!edgeSource.Equals(mergeSourceNumber) && (!IsSimpleInterFileJump(edge))) // TODO: figure out why this doesn't work
                    {
                        return false; // if the to file is a target of any jumps other than that from the merge target and the source of that jump is not itself a simple interfile jump we cannot safely merge
                    }                    
                }
            }

            return true;
               
        }
        
        public bool CheckMergabilitySimple(KCFile from, KCFile to, List<InterFileEdge> edgeList)
        {
            return CheckMergabilitySimple(CalculateBranchComplexity(from), CalculateBranchComplexity(to), edgeList);
        }

        /// <summary>
        /// Merge two files meeting the simple interfile jump conditions together
        /// </summary>
        /// <param name="fromFile">The source file of the jump</param>
        /// <param name="toFile">The target file of the jump</param>
        /// <returns></returns>
        /// <remarks>Check mergability with CheckMergabilitySimple</remarks>
        public KCFile MergeFilesSimple(KCFile fromFile, KCFile toFile, List<InterFileEdge> edgeList)
        {
            // TODO: add virtual start of files and virtual end of files to track metadata on which parts of a merged file come from where later on

            // stitch the files together
            var fromInstructions = fromFile.instructions;
            var toInstructions = toFile.instructions;
            if (!(fromInstructions.Last() is InterFileJump))
            {
                throw new Exception("Expected last instruction of simple merge target to be InterFileJump");
            }
            fromInstructions.RemoveAt(fromInstructions.Count - 1);
            fromInstructions.AddRange(toInstructions);
            toFile.instructions = null; // when functional programmers say they hate side-effects this is probably why

            // update the edge list
            StCpNumber mergeSourceNumber = (fromFile.header as StCp_Header).GetFileNumber();
            StCpNumber mergeTargetNumber = (toFile.header as StCp_Header).GetFileNumber();

            for (int i = 0; i < edgeList.Count; i++)
            {
                var workingEdge = edgeList[i];
                // all of the jumps from the from file becomes jumps from the new merged file
                if (workingEdge.from.Equals(mergeTargetNumber))
                {
                    workingEdge.from = mergeSourceNumber;
                }
                // delete edges linking the old files since these jumps are now treated as non-existent
                if (workingEdge.from.Equals(mergeSourceNumber) && workingEdge.to.Equals(mergeTargetNumber))
                {
                    edgeList[i] = null; // TODO: replace this with RemoveAt() at some point
                }
            }

            // purge the nulls created when updating the edge list
            edgeList.RemoveAll(edge => edge == null);

            return fromFile;
        }

        /// <summary>
        /// Given a list of files and list of jumps between files, merge files that are simple and wholly sequential together
        /// </summary>
        /// <param name="files">The list of files to be checked/updated</param>
        /// <param name="edgeList">The edge list to be used/updated</param>
        /// <remarks>Reduce the number of vertices on the branch graph (and therefore the amount of files that we need to deal with) by elimating vertices that follow the simple interfile jump pattern</remarks>
        public void FlattenSimpleFiles(List<KCFile> files, List<InterFileEdge> edgeList)
        {

            BuildFileDict(files);

            for (int i = 0; i < files.Count; i++)
            {
                var currentFile = files[i];

                if (currentFile != null)

                {
                    if (currentFile.instructions == null)
                    {
                        continue; // remember when i made MergeFilesSimple null something out? yep
                    }

                    var branchComplexity = CalculateBranchComplexity(currentFile);
                    if (branchComplexity.interFileJumps <= 1)

                    {

                        var currentFileNumber = (currentFile.header as StCp_Header).GetFileNumber();
                        var edges = from edge in edgeList where edge.@from == currentFileNumber select edge;
                        var foundEdges = edges.ToList();

                        if (foundEdges.Count() == 0)
                        {
                            continue; // weird case for dead end vertices, not a big problem though since they're not that common
                        }
                        else if (foundEdges.Count() > 1)
                        {
                            throw new Exception("Unexpected number of edges for simple file");
                        }

                        var targetFileNumber = foundEdges[0].to.ToString();
                        var targetFile = fileDict[targetFileNumber];

                        if (CheckMergabilitySimple(currentFile, targetFile, edgeList))
                        {
                            MergeFilesSimple(currentFile, targetFile, edgeList);
                        }

                    }
                }
            }

            files.RemoveAll(file => file == null);

        }

    }
}
