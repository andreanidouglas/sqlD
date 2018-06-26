using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using DatabaseModel.Generic;

namespace DatabaseModel.SQLServer
{
    public class SQLServerAnalysis : SQLServer
    {
        
        public int timeoutTime { get; set; }
        
        public SQLServerAnalysis(string dataSource, string initialCatalog) : base(dataSource, initialCatalog) { }
        public SQLServerAnalysis(string dataSource) : base(dataSource) {}
        private readonly String _connectionString = @"Provider=MSOLAP;Integrated Security=SSPI;Persist Security Info=True";

        protected override String SQLConnectionString { 
            get { return _connectionString; }
            set
            {
                if (value.Equals(""))
                { this.SQLConnectionString = _connectionString + this.connectionString; }
            }
        }

        
        
        public override void build()
        {
            this.connectionString = this._connectionString + this.connectionString;
        }

        public override void RunSQLReader(string sqlCommand)
        {
            try
            {
                if (!this.isConnectionOpen())
                {
                    throw new DatabaseNotOpen();
                }
                OleDbCommand command = new OleDbCommand(sqlCommand);
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = timeoutTime;
                dataReader = command.ExecuteReader(System.Data.CommandBehavior.Default);
            }
            catch (OleDbException ode)
            {
                throw ode;
            }
        }

    }
}
