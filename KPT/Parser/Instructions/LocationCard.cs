using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Instructions
{
    class LocationCard : IInstruction, IHasStrings
    {
        Opcode opcode;
        string time;
        string location;

        public bool Read(BinaryReader br)
        {
            opcode = FileIOHelper.ReadOpcode(br);
            time = FileIOHelper.ReadNullTerminatedString(br);
            location = FileIOHelper.ReadNullTerminatedString(br);
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)opcode);
            FileIOHelper.WriteStringNullTerminated(bw, time);
            FileIOHelper.WriteStringNullTerminated(bw, location);
            return true;
        }


        public void AddStrings(StringCollection collection)
        {
            string newID;

            newID = collection.GenerateNewID();
            collection.AddString(newID, time);
            time = newID;

            newID = collection.GenerateNewID();
            collection.AddString(newID, location);
            location = newID;
        }

        public void GetStrings(StringCollection collection)
        {
            time = collection.GetString(time);
            location = collection.GetString(location);
        }

    }
}
