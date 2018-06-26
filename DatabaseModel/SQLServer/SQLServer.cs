using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

using DatabaseModel.Generic;
using System.Text.RegularExpressions;

namespace DatabaseModel.SQLServer
{
    public abstract class SQLServer : Database
    {
        protected OleDbConnection connection;
        protected OleDbDataReader dataReader;
        private String dataSource;
        private String initialCatalog;

        protected virtual String SQLConnectionString { get; set; }

        public SQLServer()
            : base("SQL Server")
        {
        }

        public SQLServer(string dataSource)
            : base("SQL Server")
        {
            this.connectionString +=  ";Data Source=" + dataSource;
            this.dataSource = dataSource;
        }

        public SQLServer(string dataSource, string initialCatalog)
            : base("SQL Server")
        {
            this.connectionString += ";Data Source=" + dataSource + ";Initial Catalog=" + initialCatalog;
            this.initialCatalog = initialCatalog;
            this.build();
        }

        public abstract void build();

        /// <summary>
        /// Set/Replace the datasource for the SQL Server connection
        /// </summary>
        /// <param name="dataSource"></param>
        public void setDataSource(string dataSource)
        {
            Regex newRegex = new Regex(";Data Source=[\\.@0-9a-z_-]+");
            if (newRegex.Match(this.connectionString).Success)
            {
                this.connectionString = newRegex.Replace(this.connectionString, ";Data Source=" + dataSource);
            }
            else
            {
                this.connectionString += ";Data Source=" + dataSource;
            }
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Set/Replace the Initial Catalog for the SQL Server connection
        /// </summary>
        /// <param name="initialCatalog"></param>
        public void setInitialCatalog(string initialCatalog)
        {
            Regex newRegex = new Regex(";Initial Catalog=[\\.@0-9a-z_-]+");
            if (newRegex.Match(this.connectionString).Success)
            {
                this.connectionString = newRegex.Replace(this.connectionString, ";Initial Catalog=" + initialCatalog);
            }
            else
            {
                this.connectionString += ";Initial Catalog=" + initialCatalog;
            }
            this.initialCatalog = initialCatalog;
        }

        public override bool Connect()
        {
            try
            {
                connection = new OleDbConnection(this.connectionString);
                connection.Open();
                return (isConnectionOpen());
            }
            catch (OleDbException ode)
            {
                throw ode;
            }

        }

        /// <summary>
        /// Vefify wheater the connection is open
        /// </summary>
        /// <returns>True if the connection is open</returns>
        public bool isConnectionOpen()
        {
            return (connection != null && connection.State == System.Data.ConnectionState.Open) ? true : false;
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
                dataReader = command.ExecuteReader(System.Data.CommandBehavior.Default);
            }
            catch (OleDbException ode)
            {
                throw ode;
            }
        }

        public override String[] GetNextRow()
        {
            if (dataReader != null)
            {
                String[] returnable = new String[dataReader.FieldCount];
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    returnable[i] = dataReader.GetValue(i).ToString();
                }
                return returnable;
            }
            return (new String[0]);
        }

        public override String[] GetColumnNames()
        {
            if (dataReader != null)
            {
                String[] returnable = new String[dataReader.FieldCount];
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    returnable[i] = dataReader.GetName(i);
                }
                return returnable;
            }
            return (new String[0]);
        }

        public override bool Read()
        {
            return (dataReader.Read());
        }
    }
}
