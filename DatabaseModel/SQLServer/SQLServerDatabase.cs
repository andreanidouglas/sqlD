using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using DatabaseModel.Generic;
namespace DatabaseModel.SQLServer
{
    public class SQLServerDatabase : SQLServer
    {
        private readonly string _connectionString = @"Provider=SQLOLEDB;Integrated Security=SSPI";
        
        public SQLServerDatabase(string dataSource) : base(dataSource) { }
        
        public SQLServerDatabase(string dataSource, string initialCatalog) : base(dataSource, initialCatalog) { }

        protected override String SQLConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (value.Equals(""))
                { this.connectionString = _connectionString; }
            }
        }
        
        
        public override void build()
        {
            this.connectionString = this.SQLConnectionString + this.connectionString;
        }
    }
}
