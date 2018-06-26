using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DatabaseModel.Generic;


namespace SQLd_cli
{
    class ReportController
    {
        List<Report> reports = new List<Report>();

        public Report getReportByName(String name)
        {
            foreach (Report r in reports)
            {
                if (r.Name == name)
                {
                    return r;
                }
            }
            return null;
        }

        public List<Report> getReportList() 
        {
            return reports;
        }

        public void CreateReportFromCSV(String csvFilePath)
        {
            try
            {
                StreamReader sr = new StreamReader(csvFilePath);
                while (!sr.EndOfStream)
                {
                    String[] reportLine = sr.ReadLine().Split(new Char[] { ',' });
                    Report r = new Report();
                    r.DatabaseVendor = reportLine[0];
                    r.Database = reportLine[1];
                    r.Name = reportLine[2];
                    r.Login = reportLine[3];
                    r.Password = reportLine[4];
                    r.QueryPath = reportLine[5];
                    generateReportDatabase(r);
                    r.readFile();
                    reports.Add(r);
                }
            }
            catch (System.IO.IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
                return;
            }
        }

        private void generateReportDatabase(Report report)
        {
            DatabaseConstructor dbc = new DatabaseConstructor(report.DatabaseVendor, report.Database);
            dbc.setLogin(report.Login);
            dbc.setPassword(report.Password);
            
            report.ReportDatabase = dbc.Database();
        }

    }
}
