using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DatabaseModel.Generic;
using WorkModel;


namespace TestConsoleApplication
{
    class Program
    {
        public static void Main(String[] args){
            WorkerQueue reports = new WorkerQueue();
            WorkerExecutionControl wec = new WorkerExecutionControl();
            for (int i = 0; i < 50; i++)
            {
                reports.push(new QueryExecution(i));
            }

            wec.ExecuteQueue(reports, 10);
           
            
        }
    }
}
