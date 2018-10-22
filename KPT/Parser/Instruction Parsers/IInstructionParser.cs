using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instruction_Parsers
{
    /// <summary>
    /// Public interface for instruction parsers
    /// </summary>
    interface IInstructionParser
    {
        /// <summary>
        /// Read an instruction and its arguments from the given stream
        /// </summary>
        /// <param name="br">The stream from which to read</param>
        /// <returns>True on successful read, false on failed read</returns>
        bool Read(BinaryReader br);
        /// <summary>
        /// Write an instruction and its arguments to the given stream
        /// </summary>
        /// <param name="bw">The stream to write to</param>
        /// <returns>True on successful write, false on failed write</returns>
        bool Write(BinaryWriter bw);
    }
}
