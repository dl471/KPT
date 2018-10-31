using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPT.Parser
{
    class StringCollection
    {

        private Dictionary<string, string> stringMap;

        public StringCollection()
        {
            stringMap = new Dictionary<string, string>();
        }

        public void AddString(string id, string newValue)
        {
            if (!stringMap.Keys.Contains(id)) {
                stringMap[id] = newValue;
                return;
            }

            string currentValue = stringMap[id];

            if (currentValue != newValue)
            {
                throw new Exception(string.Format("Tried to overwrite existing string value of id {0}", id));
            }

        }

        public string GetString(string id)
        {
            return stringMap[id];
        }

    }
}
