#region

using System.Data;
using System.Data.SqlClient;
using STP.Common;
using STP.Common.Configuration;
using STP.Common.Enums;
using STP.Common.Log;

#endregion

namespace STP.DataAccess.Debug
{
    public class LogWrapper
    {
        private const string LogPrefix = "DebugEnabled=True: SafeProcedure Log";

        #region Helper Method

        /*
        public static void LogIfDebugEnabled(Database database, string procedureName, SqlCommand command)
        {
            if (STPConfig.Instance.IsDebugEnabled)
            {
                STPConfigurationManager.LogProvider.Log(string.Format("{0}::Database: {1}, Procedure: {2}, Parameters: {3}", LogPrefix,
                                             database.InstanceName, procedureName,
                                             DebugUtil.GetParameterString(command)));
            }
        }

        public static void LogIfDebugEnabled(Database database, string procedureName, params object[] parameters)
        {
            if (STPConfig.Instance.IsDebugEnabled)
            {
                STPConfigurationManager.LogProvider.Log(string.Format("{0}::Database: {1}, Procedure: {2}, Parameters: {3}", LogPrefix,
                                             database.InstanceName, procedureName,
                                             DebugUtil.GetParameterString(parameters)));
            }
        }

        public static void LogIfDebugEnabled<T>(Database database, string procedureName, object[] parameters)
        {
            if (STPConfig.Instance.IsDebugEnabled)
            {
                STPConfigurationManager.LogProvider.Log(string.Format("{0}::Database: {1}, Procedure: {2}, Parameters: {3}, Instance Type: {4}",
                                             LogPrefix,
                                             database.InstanceName, procedureName,
                                             DebugUtil.GetParameterString(parameters), typeof (T)));
            }
        }
        */

        public static void LogIfDebugEnabled(DatabaseSource databaseSource, string procedureName, IDbCommand command)
        {
            if (STPConfig.Instance.IsDebugEnabled)
            {
                STPConfigurationManager.LogProvider.Log(string.Format("{0}::Database: {1}, Procedure: {2}, Parameters: {3}", LogPrefix,
                    databaseSource, procedureName,
                    DebugUtil.GetParameterString((SqlCommand) command)));
            }
        }

        public static void LogIfDebugEnabled(string databaseSource, string procedureName, IDbCommand command)
        {
            if (STPConfig.Instance.IsDebugEnabled)
            {
                STPConfigurationManager.LogProvider.Log(string.Format("{0}::Database: {1}, Procedure: {2}, Parameters: {3}", LogPrefix,
                    databaseSource, procedureName,
                    DebugUtil.GetParameterString((SqlCommand) command)));
            }
        }

        #endregion
    }
}