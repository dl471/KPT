using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KPT.XMLBuild
{
    interface IXMLBuildObject
    {
        void WriteXML(XmlWriter xmlWriter);
    }
}
