using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Elements
{
    // A block of bytes indicating or referencing a Cp and St number together - appears in headers and jumps
    class StCpNumber : IElement
    {

        short cpNumber;
        short stNumber;

        short ST_NUM_LEN = 3;
        short CP_NUM_LEN = 4;

        public bool Read(BinaryReader br)
        {
            cpNumber = br.ReadInt16();
            stNumber = br.ReadInt16();            
            return true;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write(cpNumber);
            bw.Write(stNumber);
            return true;
        }

        private string formatStNumber()
        {
            string stNumString = stNumber.ToString("X");
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < ST_NUM_LEN - stNumString.Length; i++)
            {
                sb.Append("0");
            }

            sb.Append(stNumString);

            return sb.ToString();
        }

        private string formatCpNumber()
        {
            string cpNumString = cpNumber.ToString("X");
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < CP_NUM_LEN - cpNumString.Length; i++)
            {
                sb.Append("0");
            }

            sb.Append(cpNumString);

            return sb.ToString();
        }

        public override string ToString()
        {
            return "St" + formatStNumber() + "Cp" + formatCpNumber();
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types - modified from https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=net-6.0
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                StCpNumber number = (StCpNumber)obj;
                return (stNumber == number.stNumber) && (cpNumber == number.cpNumber);
            }
        }


        public override int GetHashCode()
        {
            return stNumber << 16 + cpNumber;
        }

    }
}
