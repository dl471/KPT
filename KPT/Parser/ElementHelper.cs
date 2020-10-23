using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPT.Parser.Elements;
using KPT.Parser.Instructions;
using System.IO;

namespace KPT.Parser
{
    internal static class ElementHelper
    {
        // there is no reason for an element to be anywhere near as large as nevermind larger than 4096 in normal usage so i am using that as the buffer size
        static byte[] buffer = new byte[4096];
        static MemoryStream bufferStream = new MemoryStream(buffer);
        static BinaryWriter bufferWriter = new BinaryWriter(bufferStream);

        // instead of adding this to the the IElement interface and propogating it through IElement classes we'll just make an extension function here
        // this is also a rather cheeky / odd function in that instead of writing the code for each part of the elements or using reflection we just get it to write itself to a fake buffer then record the write length
        // can be depreceated if the performance implications are too bad
        internal static long GetElementSize(this IElement element)
        {
            bufferWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            if (element is Jump_Label_Manager.VirtualLabel)
            {
                return 0; // remember when i decided it would be a cool idea to have the virtuallabels update the offset of their paired jumptableentry automatically on write? turns out calling getelementsize on it causes it to zero the offsets, which is bad if you're trying to figure out what they are after calling a function that uses getelementsize.
            }
            element.Write(bufferWriter);
            return bufferWriter.BaseStream.Position;
        }

        /// <summary>
        /// A helper function for creating Boxes
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        /// <remarks>
        /// Intended to make Box creating code cleaner by delegating the getting of instruction size and construction of object to a seperate function
        /// </remarks>
        internal static InstructionBox MakeInstructionBox(Opcode opcode)
        {
            int instructionSize = OpcodeInfo.GetInstructionSize(opcode);
            return new InstructionBox(instructionSize);
        }
    }
}
