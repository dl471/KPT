using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using KPT.Parser.Elements;

namespace KPT.Parser.Branch_Mapper
{
    class JumpLabelMeta
    {
        StCpNumber parentFile;
        HashSet<JumpLabelMeta> goesTo;
        HashSet<JumpLabelMeta> comesFrom;

        public JumpLabelMeta()
        {
            goesTo = new HashSet<JumpLabelMeta>();
            comesFrom = new HashSet<JumpLabelMeta>();
        }

    }
}
