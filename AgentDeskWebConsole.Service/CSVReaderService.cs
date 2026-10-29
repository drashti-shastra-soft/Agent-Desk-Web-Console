using AgentDeskWebConsole.Infrastructure.IService;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Text;
using System.IO;
using System;

namespace AgentDeskWebConsole.Service
{
    public class CSVReaderService : ICSVReaderService
    {
        public DataTable ReadFile(string filePath)
        {
            DataTable dataTable = new DataTable();
            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    dataTable = ReadWithSpecialChar(streamReader.ReadToEnd(), ",");
                }
            }
            return dataTable;
        }

        public DataTable ReadWithSpecialChar(string csvString, params string[] args)
        {
            DataTable dataTable = new DataTable();
            using (TextFieldParser textFieldParser = new TextFieldParser(new MemoryStream(Encoding.UTF8.GetBytes(csvString))))
            {
                textFieldParser.HasFieldsEnclosedInQuotes = true;
                textFieldParser.TrimWhiteSpace = true;
                textFieldParser.SetDelimiters(args);
                bool readFirstLine = true;
                try
                {
                    while (!textFieldParser.EndOfData)
                    {
                        string[] fields = textFieldParser.ReadFields();

                        if (readFirstLine)
                        {
                            foreach (var val in fields)
                            {
                                dataTable.Columns.Add(val);
                            }
                            readFirstLine = false;
                        }
                        else
                        {
                            dataTable.Rows.Add(fields);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return dataTable;
        }
    }
}
