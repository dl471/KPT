using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KPT.Parser.Spreadsheet_Interface
{
    class CSVFileWriter
    {
        public void WriteCSVFile(string fileName, List<IInstruction> fileContents, StringCollection fileStrings)
        {

            DirectoryGuard.CheckDirectory(fileName);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            var csv = new CsvHelper.CsvWriter(sw);
            csv.Configuration.RegisterClassMap<CSVRecordMap>();

            csv.WriteHeader<CSVRecord>();
            csv.NextRecord();

            string lastSpeaker = "";

            foreach (var instruction in fileContents)
            {
                if (instruction is IHasStrings)
                {
                    var temp = instruction as IHasStrings;
                    List<CSVRecord> csvRecords = temp.GetCSVRecords();
                    foreach (var record in csvRecords)
                    {
                        record.originalText = fileStrings.GetString(record.stringID);
                        if (record.speaker != lastSpeaker)
                        {
                            lastSpeaker = record.speaker;
                        }
                        else
                        {
                            record.speaker = "";
                        }
                        csv.WriteRecord(record);
                        csv.NextRecord();
                    }
                }
            }

            csv.Flush();

            sw.Close();
            fs.Close();

        }
    }
}
