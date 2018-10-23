using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Elements;
using KPT.Parser.Instruction_Parsers;
using System.Windows.Forms;

namespace KPT.Parser
{

    class KCFile
    {
        public IHeader header;
        public List<IInstructionParser> instructions;
        public Box footer;
    }



    class FileParser
    {

        List<IElement> instructions;

        public FileParser()
        {
            instructions = new List<IElement>();
        }

        private Box MakeBox(Opcode opcode)
        {
            int instructionSize = OpcodeInfo.GetInstructionSize(opcode);
            return new Box(instructionSize);
        }

        public KCFile ParseFile(BinaryReader br, string fileName)
        {
            KCFile workingFile = new KCFile();
            List<IInstructionParser> instructions = new List<IInstructionParser>();

            Box footer = ReadFooter(br);
            workingFile.footer = footer;
            workingFile.header = ReadHeader(br);

            long streamEnd = br.BaseStream.Length - footer.GetContentsSize();

            while (br.BaseStream.Position != streamEnd) // will need to check this for accuracy as it has been unreliable in some cases in the past
            {
                Opcode opcode = ElementReader.ReadOpcode(br);
                br.BaseStream.Position -= OpcodeInfo.OPCODE_SIZE; // set the position back by 2, the size of the opcodes, as the instruction parsers will expect to given a position starting with an opcode

                if (opcode == Opcode.INVALID)
                {
                    string errorMessage = string.Format("There was an unexpected opcode when reading file {0} at position {1} after reading {2} instructions", fileName, br.BaseStream.Position.ToString("X"), instructions.Count.ToString());
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }

                IInstructionParser newInstruction;
                Type instructionParserType = OpcodeInfo.GetInstructionParserType(opcode);
                
                if (instructionParserType == typeof(Box))
                {
                    newInstruction = MakeBox(opcode);
                    newInstruction.Read(br);
                }
                else
                {
                    newInstruction = (IInstructionParser)Activator.CreateInstance(instructionParserType);
                    newInstruction.Read(br);
                }

                instructions.Add(newInstruction);

            }

            workingFile.instructions = instructions;
            return workingFile;
            
        }

        private IHeader ReadHeader(BinaryReader br)
        {
            St_Header header = new St_Header();
            if (!header.Read(br))
            {
                string errorMessage = "Failed to read header of file {0}. Corrupt or invalid header?";
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            return header;
        }

        private Box ReadFooter(BinaryReader br)
        {
            br.BaseStream.Seek(-1, SeekOrigin.End);

            int footerSize = 0;
            Box footer;

            if (br.ReadByte() != 0x88)
            {
                footer = new Box(0);
                footer.Read(br);
                return footer;
            }

            br.BaseStream.Seek(-1, SeekOrigin.End);

            while (br.ReadByte() == 0x88)
            {
                br.BaseStream.Seek(-2, SeekOrigin.Current);
                footerSize += 1;
            }

            footer = new Box(footerSize);
            footer.Read(br);

            br.BaseStream.Seek(0, SeekOrigin.Begin);

            return footer;
        }

    }
}
