using System;

namespace DatabaseModel.Generic
{
    /// <summary>
    /// Use this class to construct Database connectors without needing to use each vendor specific class
    /// 
    /// The DatabaseVendor static class provide the list of available vendors
    /// </summary>
    public class DatabaseConstructor
    {
        private Database builder;

        /// <summary>
        /// Construct the Database per vendor and datasource
        /// </summary>
        /// 
        /// <param name="vendor"><see cref="DatabaseVendor"/></param>
        /// 
        /// <param name="dataSource">Data Source that you will connect</param>
        public DatabaseConstructor(String vendor, String dataSource)
        {
            switch (vendor)
            {
                case "Oracle":
                    builder = new Oracle.Oracle(dataSource);
                    
                    break;
                case "SQL Server Analysis":
                    builder = new SQLServer.SQLServerAnalysis(dataSource);
                    ((SQLServer.SQLServer)builder).build();
                    break;
                case "SQL Server Database":
                    builder = new SQLServer.SQLServerDatabase(dataSource);
                    ((SQLServer.SQLServer)builder).build();
                    break;
            }
        }

        public Database Database()
        {
            return this.builder;
        }

        /// <summary>
        /// Set the login for the Database
        /// </summary>
        /// <param name="login"></param>
        public void setLogin(string login)
        {
            this.builder.connectionString += ";User ID=" + login;
        }

        /// <summary>
        /// Set the password for the Database
        /// </summary>
        /// <param name="password"></param>
        public void setPassword(string password)
        {
            this.builder.connectionString += ";Password=" + password;
        }

        /// <summary>
        /// Set an additional parameter to the Database
        /// </summary>
        /// <param name="parameter">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void setParameter(string parameter, string value)
        {
            this.builder.connectionString += ";" + parameter + "=" + value;
        }
    }
}
