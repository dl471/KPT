using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace KPT.Parser.Spreadsheet_Interface
{
    public class CSVRecord
    {
        const string version = "v1.0";
        public string speaker { get; set; }
        public string originalText { get; set; }
        public string translatedText { get; set; }
        public string translatorNotes { get; set; }
        public string editedText { get; set; }
        public string editorNotes { get; set; }
        public string blank1 { get; set; }
        public string objectID { get; set; }
        public string stringID { get; set; }

        public CSVRecord (string speaker, string stringID)
        {
            this.speaker = speaker;
            this.stringID = stringID;
        }

    }

    public sealed class CSVRecordMap : ClassMap<CSVRecord>
    {
        public CSVRecordMap()
        {
            Map(csr => csr.speaker).Name("Speaker");
            Map(csr => csr.originalText).Name("Original Text");
            Map(csr => csr.translatedText).Name("Translated Text");
            Map(csr => csr.translatorNotes).Name("Translator Notes");
            Map(csr => csr.editedText).Name("Edited Text");
            Map(csr => csr.editorNotes).Name("Editor Notes");
            Map(csr => csr.blank1).Name("");
            Map(csr => csr.objectID).Name("Object ID");
            Map(csr => csr.stringID).Name("String ID");
        }
    }
    
}
