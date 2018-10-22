using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPT
{
    static class ActiveEncodings
    {
        public static Encoding currentEncoding;

        public static void Initalize()
        {
            currentEncoding = Encoding.GetEncoding("shift-jis");
        }

    }

}
