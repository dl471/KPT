﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Instructions
{
    class U_500 : IInstruction
    {
        Opcode opcode;
        Box box1;
        string name;
        string dialogue;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            box1 = new Box(0x8);
            box1.Read(br);
            name = FileIOHelper.ReadName(br);
            dialogue = FileIOHelper.ReadDialogueString(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}