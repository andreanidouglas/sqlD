using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseModel.Generic
{
    public abstract class Database
    {
        public String connectionString { get; set; }
        public String userName { get; set; } 
        private string password;
        public String vendor { get; set; }

        public virtual void setPassword(String password)
        {
            this.password = password;
        }

        protected Database(String vendor)
        {
            this.vendor = vendor;
        }

        /// <summary>
        /// Connect to the Database
        /// </summary>
        /// <returns>True if the connection succeeds</returns>
        public abstract bool Connect();

        /// <summary>
        /// Run a Data Reader for the given SQL command
        /// </summary>
        /// <param name="sqlCommand">SQL command to be executed</param>
        public abstract void RunSQLReader(string sqlCommand);

        /// <summary>
        /// Get the name of the returned columns from the DataReader
        /// </summary>
        /// <returns>Array of column names</returns>
        public abstract String[] GetColumnNames();

        /// <summary>
        /// Get the next row of values of the returned columns from the DataReader
        /// </summary>
        /// <returns>Array of values</returns>
        public abstract String[] GetNextRow();

        /// <summary>
        /// Read if the next row of the DataReader is valid and not EOF
        /// </summary>
        /// <returns>True if not EOF</returns>
        public abstract bool Read();
    }

    public class DatabaseNotOpen : Exception
    {
        public DatabaseNotOpen() : base("Database must be opened before execute the operation") { }
        
        public DatabaseNotOpen(string error) : base(error) { }


    }
}
