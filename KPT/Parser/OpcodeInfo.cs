using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT.Parser.Instruction_Parsers;
using System.Windows.Forms;

namespace KPT.Parser
{  

    /// <summary>
    /// A list of opcodes known to the program
    /// </summary>
    enum Opcode
    {
        INVALID = -1,
        EOF = -2,
        U_191 = 0x191,
        U_303 = 0x303,
        U_501 = 0x501,
        U_504 = 0x504,

        U_214 = 0x214,
        U_210 = 0x210,
        U_404 = 0x404,
        U_402 = 0x402,

        U_131 = 0x131,
        U_403 = 0x403,
        U_601 = 0x601,
        U_130 = 0x130,
        U_132 = 0x132,
        U_205 = 0x205,
        U_500 = 0x500,

        U_603 = 0x603,
        U_192 = 0x192,
        U_401 = 0x401,
        U_405 = 0x405,

        U_173 = 0x173,
        U_174 = 0x174,
        U_160 = 0x160,
        U_120 = 0x120,
        U_30C = 0x30C,
        U_00C = 0x00C,
        U_1C0 = 0x1C0,
        U_170 = 0x170,

        U_100 = 0x100,
        U_111 = 0x111,
        U_190 = 0x190,
        U_171 = 0x171,
        U_1000 = 0x1000,
        U_121 = 0x121,
        U_008 = 0x008,
        LOCATION_CARD = 0x1001,
        CHOICE_DIALOG = 0x701,
        U_113 = 0x0113,
    }


    static class OpcodeInfo
    {

        public static int OPCODE_SIZE = 2;

        /// <summary>
        /// A dictionary of instructions and their sizes. Size is defined as the combined size of the opcode and any arguments it takes. Mainly used for Boxes.
        /// </summary>
        static Dictionary<Opcode, int> instructionSize = new Dictionary<Opcode, int>
        {
            { Opcode.U_191, 6 },
            { Opcode.U_303, 2 },
            { Opcode.U_214, 4 },
            { Opcode.U_210, 4 },
            { Opcode.U_404, 7 },
            { Opcode.U_402, 2 },
            { Opcode.U_131, 4 },
            { Opcode.U_403, 2 },
            { Opcode.U_601, 8 },
            { Opcode.U_130, 4 },
            { Opcode.U_132, 2 },
            { Opcode.U_205, 4 },
            { Opcode.U_603, 2 },
            { Opcode.U_192, 4 },
            { Opcode.U_401, 4 },
            { Opcode.U_405, 4 },
            { Opcode.U_173, 4 },
            { Opcode.U_174, 2 },
            { Opcode.U_160, 2 },
            { Opcode.U_120, 4 },
            { Opcode.U_30C, 2 }, // only had one example of this so far
            { Opcode.U_00C, 2 },
            { Opcode.U_1C0, 2 },
            { Opcode.U_170, 4 },
            { Opcode.U_100, 2 }, // educated guess
            { Opcode.U_111, 4 }, // educated guess
            { Opcode.U_190, 4 }, // educated guess
            { Opcode.U_171, 4 },
            { Opcode.U_1000, 2 },
            { Opcode.U_121, 6 }, // educated guess, extremely unconfirmed
            { Opcode.U_008, 2 }, // educated guess
            { Opcode.U_113, 10 }, // educated guess, extremely unconfirmed


        };

        static Dictionary<Opcode, Type> instructionParserMap = new Dictionary<Opcode, Type>
        {
            { Opcode.U_008, typeof(Box) },
            { Opcode.U_00C, typeof(Box) },

            { Opcode.U_100, typeof(Box) },
            { Opcode.U_111, typeof(Box) },
            { Opcode.U_113, typeof(Box) },
            { Opcode.U_120, typeof(Box) },
            { Opcode.U_121, typeof(Box) },
            { Opcode.U_130, typeof(Box) },
            { Opcode.U_131, typeof(Box) },
            { Opcode.U_132, typeof(Box) },
            { Opcode.U_160, typeof(Box) },
            { Opcode.U_170, typeof(Box) },
            { Opcode.U_171, typeof(Box) },
            { Opcode.U_173, typeof(Box) },
            { Opcode.U_174, typeof(Box) },
            { Opcode.U_190, typeof(Box) },
            { Opcode.U_191, typeof(Box) },
            { Opcode.U_192, typeof(Box) },
            { Opcode.U_1C0, typeof(Box) },

            { Opcode.U_205, typeof(Box) },
            { Opcode.U_210, typeof(Box) },
            { Opcode.U_214, typeof(Box) },

            { Opcode.U_303, typeof(Box) },
            { Opcode.U_30C, typeof(Box) },  

            { Opcode.U_401, typeof(Box) },
            { Opcode.U_402, typeof(Box) },
            { Opcode.U_403, typeof(Box) },
            { Opcode.U_404, typeof(Box) },
            { Opcode.U_405, typeof(Box) },

            { Opcode.U_500, typeof(U_500) },
            { Opcode.U_501, typeof(U_501) },
            { Opcode.U_504, typeof(U_504) }, 

            { Opcode.U_601, typeof(Box) },
            { Opcode.U_603, typeof(Box) },

            { Opcode.U_1000, typeof(Box) },
            { Opcode.LOCATION_CARD, typeof(LocationCard) },
            { Opcode.CHOICE_DIALOG, typeof(ChoiceDialog) },
        };


        public static int GetInstructionSize(Opcode opcode)
        {
            int size;
            bool success;

            success = instructionSize.TryGetValue(opcode, out size);

            if (!success)
            {
                string errorMessage = string.Format("No instruction size specified for opcode {0}.", opcode.ToString());
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            return size;
        }

        public static Type GetInstructionParserType(Opcode opcode)
        {

            Type type;
            bool success;

            success = instructionParserMap.TryGetValue(opcode, out type);

            if (!success)
            {
                string errorMessage = string.Format("No instruction parser type specified for opcode {0}.", opcode.ToString());
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            return type;
        }

    }

}
