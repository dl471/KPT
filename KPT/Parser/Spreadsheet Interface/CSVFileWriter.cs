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

            fileName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".csv");

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
                        if (!(record.speaker == "[disasm]"))
                        {
                            record.originalText = fileStrings.GetString(record.stringID);
                        }
                        if (record.speaker != lastSpeaker)
                        {
                            if (record.speaker != "[disasm]")
                            {
                                lastSpeaker = record.speaker;
                            }
                        }
                        else
                        {
                            record.speaker = ""; // blank out the name of the speaker if it is being repeated to make it easier to note when speaker changes and avoid massive walls of speakers text
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
