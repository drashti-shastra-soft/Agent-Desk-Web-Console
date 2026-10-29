using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace AgentDeskWebConsole.Service
{
    public class FileReaderService : IFileReaderService
    {

        private static readonly string[] RequiredExcelHeaders =
        {
            "policyno", "fupdate", "premium"
        };


        public List<Domain.ImportLI> ReadExcel(string filePath)
        {
            List<Dictionary<int, string>> rows;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                rows = ReadXlsxRows(fs);
            }

            if (rows.Count == 0)
                throw new Exception("Excel sheet is empty.");

            int headerIndex = -1;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Values.Any(v => NormalizeHeader(v) == "policyno"))
                {
                    headerIndex = i;
                    break;
                }
            }
            if (headerIndex == -1)
                throw new Exception("Header row not found. 'Policy No' column is missing.");

            var map = new Dictionary<string, int>();
            foreach (var kv in rows[headerIndex])
            {
                var key = NormalizeHeader(kv.Value);
                if (key != "" && !map.ContainsKey(key))
                    map.Add(key, kv.Key);
            }

            var missing = RequiredExcelHeaders.Where(h => !map.ContainsKey(h)).ToList();
            if (missing.Any())
                throw new Exception("Missing column(s): " + string.Join(", ", missing)
                    + ". Headings found in file: " + string.Join(", ", map.Keys));

            var list = new List<Domain.ImportLI>();
            int sno = 1;

            foreach (var r in rows.Skip(headerIndex + 1))
            {
                if (r.Values.All(v => string.IsNullOrWhiteSpace(v))) continue;

                list.Add(new Domain.ImportLI
                {
                    SNo = sno++,
                    Family = Col(map, r, "family"),
                    Name = Col(map, r, "name", "policyholdername", "customername"),
                    Mobile = Col(map, r, "mobile", "mobileno", "phone"),
                    Email = Col(map, r, "email", "emailid"),
                    Address = Col(map, r, "address"),
                    DOB = ToDate(Col(map, r, "dob", "dateofbirth")),
                    PolicyNo = Col(map, r, "policyno", "policynumber"),
                    AgCode = Col(map, r, "agcode", "agentcode"),
                    ComDate = ToDate(Col(map, r, "comdate", "commencementdate")),
                    PlanTermPpt = Col(map, r, "plantermppt"),
                    SumAssured = ToDecimal(Col(map, r, "sumassured")),
                    Mode = Col(map, r, "mode"),
                    FupDate = ToDate(Col(map, r, "fupdate")),
                    Premium = ToDecimal(Col(map, r, "premium")),
                    Nominee = Col(map, r, "nominee", "nomineename"),
                    NomineeAge = Col(map, r, "nomineeage", "nomage"),
                    Relation = Col(map, r, "relation", "relationship")
                });
            }

            if (list.Count == 0)
                throw new Exception("No data rows found in the Excel file.");

            return list;
        }

        private static string Col(Dictionary<string, int> map, Dictionary<int, string> row, params string[] aliases)
        {
            foreach (var a in aliases)
            {
                int col;
                if (map.TryGetValue(a, out col))
                    return Cell(row, col);
            }
            return "";
        }

        private static List<Dictionary<int, string>> ReadXlsxRows(Stream stream)
        {
            XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
            var result = new List<Dictionary<int, string>>();

            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                // 1) shared strings
                var shared = new List<string>();
                var ssEntry = zip.GetEntry("xl/sharedStrings.xml");
                if (ssEntry != null)
                {
                    using (var s = ssEntry.Open())
                    {
                        var doc = XDocument.Load(s);
                        foreach (var si in doc.Root.Elements(ns + "si"))
                            shared.Add(string.Concat(si.Descendants(ns + "t").Select(t => t.Value)));
                    }
                }

                var sheetEntry = zip.Entries
                    .Where(e => e.FullName.StartsWith("xl/worksheets/sheet") && e.FullName.EndsWith(".xml"))
                    .OrderBy(e => e.FullName)
                    .FirstOrDefault();
                if (sheetEntry == null) return result;

                using (var s = sheetEntry.Open())
                {
                    var doc = XDocument.Load(s);
                    var sheetData = doc.Root.Element(ns + "sheetData");
                    if (sheetData == null) return result;

                    foreach (var rowEl in sheetData.Elements(ns + "row"))
                    {
                        var row = new Dictionary<int, string>();

                        foreach (var c in rowEl.Elements(ns + "c"))
                        {
                            var addr = (string)c.Attribute("r");
                            if (string.IsNullOrEmpty(addr)) continue;
                            int col = ColumnIndex(addr);

                            var type = (string)c.Attribute("t");
                            string value = "";

                            if (type == "s")                       
                            {
                                var v = c.Element(ns + "v");
                                int idx;
                                if (v != null && int.TryParse(v.Value, out idx) && idx < shared.Count)
                                    value = shared[idx];
                            }
                            else if (type == "inlineStr")          
                            {
                                value = string.Concat(c.Descendants(ns + "t").Select(t => t.Value));
                            }
                            else                                   
                            {
                                var v = c.Element(ns + "v");
                                if (v != null) value = v.Value;
                            }

                            row[col] = (value ?? "").Trim();
                        }

                        result.Add(row);
                    }
                }
            }

            return result;
        }

        private static int ColumnIndex(string cellRef)
        {
            int col = 0;
            foreach (char ch in cellRef)
            {
                if (!char.IsLetter(ch)) break;
                col = col * 26 + (char.ToUpper(ch) - 'A' + 1);
            }
            return col;
        }

        private static string NormalizeHeader(string s)
        {
            return new string((s ?? "").ToLower().Where(char.IsLetterOrDigit).ToArray());
        }

        private static string Cell(Dictionary<int, string> row, int col)
        {
            string v;
            return row.TryGetValue(col, out v) ? v : "";
        }

        private static DateTime? ToDate(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            double oa;
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out oa)
                && oa > 1 && oa < 2958466)
                return DateTime.FromOADate(oa);

            DateTime dt;
            string[] formats =
            {
                "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "d-M-yyyy",
                "yyyy-MM-dd", "dd/MM/yy", "dd-MMM-yyyy"
            };
            if (DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;
            if (DateTime.TryParse(text, new CultureInfo("en-GB"), DateTimeStyles.None, out dt))
                return dt;

            return null;
        }

        private static decimal? ToDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            decimal d;
            return decimal.TryParse(text.Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out d)
                ? d
                : (decimal?)null;
        }
    }
}


