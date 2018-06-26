using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkModel;
using DatabaseModel.Generic;
using System.IO;
using FileModel;
namespace SQLd_cli
{
    class QueryExec : Worker
    {
        private Report r;
        public QueryExec(Report report)
            : base(report.Name)
        {
            this.r = report;
        }

        public override void work()
        {
            try
            {
                r.ReportDatabase.Connect();
                r.ReportDatabase.RunSQLReader(r.query);
                String path = Path.GetDirectoryName(r.QueryPath);

                ExportCSV export = new ExportCSV();
                
                export.exportPath = path + "\\" + this.r.Name + DateTime.Now.ToString(" dd_M_yyyy HH_m_s") + ".csv";
                export.append = true;
                export.quotedStrings = true;
                export.openStream();
                export.addRow(r.ReportDatabase.GetColumnNames());
                while (r.ReportDatabase.Read())
                {
                    export.addRow(r.ReportDatabase.GetNextRow());
                }
                export.export();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " " + r.Name);

            }
        }
    }
}
