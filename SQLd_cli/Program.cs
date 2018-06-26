using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkModel;
using System.IO;
namespace SQLd_cli
{
    class Program
    {
        static int Main(string[] args)
        {
            String csvPath="";
            if (args.Length == 0)
            {
                Console.WriteLine("Enter csv path for reporting");
                Console.WriteLine("Usage: sqld CsvPath [-e] [export]");
                return 1;
            }
            else if(args[0] == "export" || args[0] == "-e" )
            {
                StreamWriter sr = new StreamWriter("./exported.csv");
                sr.WriteLine("Database Vendor,Database Connection,Report Name,User,Password,SQLPath");
                sr.Close();
                return (0);
            }
            else 
            {
                csvPath = args[0];
            }
            
            ReportController rc = new ReportController();
            WorkerQueue queue = new WorkerQueue();
            WorkerExecutionControl wec = new WorkerExecutionControl();
            rc.CreateReportFromCSV(csvPath);

            List<Report> reports = rc.getReportList();

            foreach (Report r in reports)
            {
                
                QueryExec qe = new QueryExec(r);
                //qe.work();
                queue.push(qe);
            }

            wec.ExecuteQueue(queue, 10);
            return (0);
        }
    }
}
