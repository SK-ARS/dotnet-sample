#region

using System;
using STP.Common;
using STP.Common.Enums;
using STP.DataAccess.Exceptions;

#endregion

namespace STP.DataAccess.Databases
{
    public sealed class ConnectionManager
    {
        public static string GetConnectionString(DatabaseSource databaseSource)
        {
            string connectionString;
            string conStr = Enum.GetName(typeof (DatabaseSource), databaseSource);
            try
            {
                connectionString =
                    STPConfigurationManager.ConfigProvider.DatabaseConfigProvider.DatabaseGroupCollectionItems[
                        conStr].Databases["mainserver1"].ConnectionString;
            }
            catch
            {
                throw new DatabaseNotConfiguredException(Enum.GetName(typeof (DatabaseSource), databaseSource));
            }
            return connectionString;
        }


        public static string GetConnectionString(string strDatabaseSource)
        {
            string connectionString;
            try
            {
                connectionString =
                    STPConfigurationManager.ConfigProvider.DatabaseConfigProvider.DatabaseGroupCollectionItems[
                        "SQL"].Databases["mainserver1"].ConnectionString;
            }
            catch
            {
                throw new DatabaseNotConfiguredException(strDatabaseSource);
            }
            return connectionString;
        }

        public static string GetAsyncConnectionString(DatabaseSource databaseSource)
        {
            string connectionStringAsync;
            try
            {
                connectionStringAsync = GetConnectionString(databaseSource) + ";Asynchronous Processing=true;";
            }
            catch
            {
                throw new DatabaseNotConfiguredException(Enum.GetName(typeof (DatabaseSource), databaseSource));
            }
            return connectionStringAsync;
        }

        public static string GetAsyncConnectionString(string databaseSource)
        {
            string connectionStringAsync;
            try
            {
                connectionStringAsync = GetConnectionString(databaseSource) + ";Asynchronous Processing=true;";
            }
            catch
            {
                throw new DatabaseNotConfiguredException(Enum.GetName(typeof (DatabaseSource), databaseSource));
            }
            return connectionStringAsync;
        }
    }
}