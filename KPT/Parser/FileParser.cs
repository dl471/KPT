using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KPT.Parser.Instruction_Parsers;
using System.Windows.Forms;

namespace KPT.Parser
{
    class FileParser
    {

        List<IInstructionParser> instructions;

        public FileParser()
        {
            instructions = new List<IInstructionParser>();
        }

        private Box MakeBox(Opcode opcode)
        {
            int instructionSize = OpcodeInfo.GetInstructionSize(opcode);
            return new Box(instructionSize);
        }

        private Opcode ReadOpcode(BinaryReader br)
        {
            int opcode = br.ReadInt16();

            if (Enum.IsDefined(typeof(Opcode), opcode))
            {
                return (Opcode)opcode;
            }
            
            return Opcode.INVALID;            
        }

        public List<IInstructionParser> ParseFile(BinaryReader br, string fileName)
        {

            while (br.BaseStream.Position != br.BaseStream.Length) // will need to check this for accuracy as it has been unreliable in some cases in the past
            {
                Opcode opcode = ReadOpcode(br);
                br.BaseStream.Position -= OpcodeInfo.OPCODE_SIZE; // set the position back by 2, the size of the opcodes, as the instruction parsers will expect to given a position starting with an opcode

                if (opcode == Opcode.INVALID)
                {
                    string errorMessage = string.Format("There was an unexpected opcode when reading file {0} at position {1}", fileName, br.BaseStream.Position.ToString("X"));
                    MessageBox.Show(errorMessage);
                    Environment.Exit(1);
                }

                IInstructionParser newInstruction;
                Type instructionParserType = OpcodeInfo.GetInstructionParserType(opcode);
                
                if (instructionParserType == typeof(Box))
                {
                    newInstruction = MakeBox(opcode);
                }
                else
                {
                    newInstruction = (IInstructionParser)Activator.CreateInstance(instructionParserType, br);
                }

                instructions.Add(newInstruction);

            }


            return instructions;
            
        }


    }
}
