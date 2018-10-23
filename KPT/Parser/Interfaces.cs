using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser
{
    /// <summary>
    /// Public interface for instruction parsers
    /// </summary>
    interface IInstructionParser : IElement
    {
        /// <summary>
        /// Read an instruction and its arguments from the given stream
        /// </summary>
        /// <param name="br">The stream from which to read</param>
        /// <returns>True on successful read, false on failed read</returns>
        new bool Read(BinaryReader br);
        /// <summary>
        /// Write an instruction and its arguments to the given stream
        /// </summary>
        /// <param name="bw">The stream to write to</param>
        /// <returns>True on successful write, false on failed write</returns>
        new bool Write(BinaryWriter bw);
    }

    interface IHeader : IElement
    {
        new bool Read(BinaryReader br);
        new bool Write(BinaryWriter bw);
    }

    interface IElement
    {
        bool Read(BinaryReader br);
        bool Write(BinaryWriter bw);
    }

}
