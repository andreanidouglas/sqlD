using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkModel;
using DatabaseModel.Generic;
using System.IO;

namespace TestConsoleApplication
{
    class QueryExecution : Worker 
    {
        Database database;
        int counter;
        public QueryExecution(int i) : base("process " + i)
        {
            DatabaseConstructor db = new DatabaseConstructor(DatabaseVendor.Oracle, "p599.noa.alcoa.com");
            db.setLogin("andredr");
            db.setPassword("D5022a25");
            database = db.Database();
           
            counter = i;
        }


        public override void work()
        {
            database.Connect();
            database.RunSQLReader("Select * from apps.ap_suppliers where creation_date >= add_months(to_date('01-MAY-2016', 'DD-MON-YYYY'), " + counter * -1 + ") ");
            StreamWriter sw = new StreamWriter(@"c:\temp\suppliers" + counter + ".csv");

            foreach (String h in database.GetColumnNames())
            {
                sw.Write("\"" + h.Trim() + "\"" + ",");
            }
            sw.WriteLine("");

            while (database.Read())
            {
                String[] row = database.GetNextRow();
                foreach (String s in row)
                {
                    sw.Write("\"" + s.Trim() + "\"" + ",");
                }
                sw.WriteLine("");
            }
            sw.Close();

        }
    }
}
