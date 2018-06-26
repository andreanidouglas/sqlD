using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DatabaseModel.Generic;

namespace SQLd_cli
{
    class Report
    {
        public String DatabaseVendor { get; set; }
        public String Database { get; set; }
        public String Name { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String QueryPath { get; set; }
        public String query { get; set; } 
        public Database ReportDatabase { get; set; }
        public String[] Result {get; set;}
        public String SavePath {get; set;}
        private StreamWriter sw;

        public void readFile()
        {
            StreamReader sr = new StreamReader(QueryPath);
            this.query = sr.ReadToEnd();
        }
        public void OpenCSV()
        {
            sw = new StreamWriter(SavePath);
        }
        public void CloseCSV()
        {
            sw.Close();
        }

        public void WriteLineToCSV()
        {
            String buffer = "";
            foreach (String s in Result)
            {
                buffer += s + ",";
            }

            sw.WriteLine(buffer.Substring(0,buffer.Length));
            
        }
    }
}
