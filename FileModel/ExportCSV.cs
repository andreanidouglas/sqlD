using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileModel
{
    public class ExportCSV : Export
    {
        private StreamWriter writer;
        public bool quotedStrings { get; set; }
        public bool append { get; set; }
        public string _exportPath;
        public override string exportPath
        {
            get
            {
                return _exportPath;
            }
            set
            {
                if (!value.EndsWith(".csv"))
                {
                    _exportPath = value + ".csv";
                }
                else
                {
                    _exportPath = value;
                }
            }
        }

        public override void openStream()
        {
            try
            {
                writer = new StreamWriter(this.exportPath, append); 
            }
            catch(Exception e)
            {
                throw new System.IO.FileNotFoundException("Export path not set");
            }
        }

        public override void export()
        {
            try
            {
                writer.Close();
            }
            catch (Exception e)
            {
                throw new System.IO.FileNotFoundException("Export path not set");
            }
        }

        public String escapeSequence(String sequence)
        {
            String escaped = sequence.Replace("\"", "\"\"");
            return escaped;
        }

        public override void addRow(string[] row)
        {
            try
            {
                for (int i = 0; i < row.Length; i++)
                {
                    String value = this.escapeSequence(row[i]);
                    if (quotedStrings)
                    {
                        writer.Write("\"");
                    }
                    if (i == row.Length - 1)
                    {
                        writer.WriteLine(value + (quotedStrings ? "\"" : ""));
                    }
                    else
                    {
                        writer.Write(value + (quotedStrings ? "\"" : "") + ",");
                    }
                }
            }
            catch (Exception e)
            {
                throw new System.IO.FileNotFoundException("Export path not set");
            }
        }
    }
}
