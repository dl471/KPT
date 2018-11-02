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

        private string idPrefix = "$";
        private string idRoot;
        private string idPostfix = "_";
        private int idCounter = 0;

        public int NumberOfKeys
        {
            get
            {
               return stringMap.Keys.Count;
            }
        }

        public StringCollection(string idRoot)
        {
            stringMap = new Dictionary<string, string>();
            this.idRoot = idRoot;
        }

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

        public string GenerateNewID()
        {
            string newID = string.Format("{0}{1}{2}{3}", idPrefix, idRoot, idPostfix, idCounter.ToString());
            idCounter++;
            return newID;
        }

    }
}
