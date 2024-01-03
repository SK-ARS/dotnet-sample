#region

using System.Configuration;
using System.Data.SqlClient;

#endregion

namespace STP.DataAccess.BaseHelper
{
    public class ConnectionFactory
    {
        //private const string ConnectionString = "data source=localhost;initial catalog=Chinook;integrated security=True;multipleactiveresultsets=True;";
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["MULogs"].ConnectionString;

        public static SqlConnection OpenConnection(string cString)
        {
            var connection = new SqlConnection(cString);
            connection.Open();
            return connection;
        }

        public static SqlConnection OpenConnection()
        {
            return OpenConnection(connectionString);
        }
    }
}