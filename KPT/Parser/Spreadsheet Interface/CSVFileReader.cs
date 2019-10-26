using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KPT.Parser.Spreadsheet_Interface
{
    class CSVFileReader
    {
        /// <summary>
        /// Read the contents of a CSV file following the CSVRecord format and place its strings into the provided string collection
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="collection"></param>
        public void ReadCSVFile(string fileName, StringCollection collection)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            var csvReader = new CsvHelper.CsvReader(sr);
            csvReader.Configuration.MissingFieldFound = null;
            csvReader.Configuration.RegisterClassMap<CSVRecordMap>();

            csvReader.Read();
            csvReader.ReadHeader(); // not checking correctness of header atm
            
            while (csvReader.Read())
            {
                var record = csvReader.GetRecord<CSVRecord>();
                var id = record.stringID;
                var translatedText = record.translatedText;
                if (translatedText == "")
                {
                    translatedText = record.originalText; // replace translated text with original text if it's blank
                }
                collection.AddString(id, translatedText);
            }            

            sr.Close();
            fs.Close();
        }
    }
}
