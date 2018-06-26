using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using DatabaseModel.Generic;

namespace DatabaseModel.Oracle
{
    public class Oracle : Database
    {
        private readonly string _connectionString = @"Provider=OraOLEDB.Oracle";
        private OleDbConnection connection;
        private OleDbDataReader dataReader;
        private String dataSource;
        
        public Oracle() : base ("Oracle")
        {
            this.connectionString = _connectionString;
        }

        public Oracle(string dataSource)
            : base("Oracle")
        {
            this.connectionString = _connectionString + ";Data Source=" + dataSource;
            this.dataSource = dataSource;
        }

        public Oracle(string userId, string password, string dataSource)
            : base("Oracle")
        {
            this.connectionString = _connectionString + ";User ID=" + userId + ";Password=" + password + ";Data Source=" + dataSource;
            this.userName = userId;
            this.setPassword( password);
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Set/Replace User Id for the Oracle connection
        /// </summary>
        /// <param name="userId"></param>
        public void setUserId(string userId)
        {
            Regex newRegex = new Regex(";User Id=[\\-0-9A-Za-z]+");
            if (newRegex.Match(this.connectionString).Success)
            {
                newRegex.Replace(this.connectionString, ";User Id=" + userId);
            }
            else
            {
                this.connectionString += ";User Id=" + userId;
            }
            this.userName = userId;
        }

        /// <summary>
        /// Set/Replace the datasource for the Oracle connection
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
        /// Set/Replace the password for the Oracle connection
        /// </summary>
        /// <param name="password"></param>
        public override void setPassword(string password)
        {
            Regex newRegex = new Regex(";Password=[A-Za-z0-9]+");
            if (newRegex.Match(this.connectionString).Success)
            {
                this.connectionString = newRegex.Replace(this.connectionString, ";Password=" + password);
            }
            else
            {
                this.connectionString += ";Password=" + password;
            }
            this.setPassword(password);
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
        /// Verify wheater the connection is open
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
