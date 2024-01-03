#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using STP.Common;
using STP.Common.Enums;
using STP.Common.Log;
using STP.DataAccess.Debug;
using STP.DataAccess.Delegates;
using STP.DataAccess.Exceptions;
using STP.DataAccess.ParameterSet;

#endregion

namespace STP.DataAccess.Command
{
    internal class CommandFactory
    {
        private const string ForbiddenVarcharsReplacement = "?";

        /// <summary>
        ///     Basically, all non-ascii characters. We're banning anything above 255 in non-unicode fields
        /// </summary>
        private static readonly Regex forbiddenVarchars = new Regex("[^\u0009-\u00FF]", RegexOptions.Compiled);

        #region Command Parameters

        public static int CommandTimeOut
        {
            get
            {
                int tCommandTimeOut = 180;

                try
                {
                    tCommandTimeOut =
                        STPConfigurationManager.ConfigProvider.DatabaseConfigProvider.CommandTimeout;
                }
                catch
                {
                }
                return tCommandTimeOut;
            }
        }

        public static int MaxAllowedCharForString
        {
            get
            {
                int tMaxAllowedCharForString = 8000;
                try
                {
                    tMaxAllowedCharForString =
                        STPConfigurationManager.ConfigProvider.DatabaseConfigProvider.VarcharMax;
                }
                catch
                {
                }
                return tMaxAllowedCharForString;
            }
        }

        #endregion

        #region Helper Methods

        

        private static void ApplySecurity(OracleCommand command, OracleParameterCollection parameterTypes)
        {
            command.CommandTimeout = CommandTimeOut;
            foreach (OracleParameter parameter in command.Parameters)
            {
                try
                {
                    switch (parameter.DbType)
                    {
                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                            if (parameter.Size > 8000)
                            {
                                parameter.Size = MaxAllowedCharForString;
                            }
                            if (parameter.Value == null || string.IsNullOrEmpty(parameter.Value.ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            string parameterName = parameter.ParameterName.Replace("@", "").ToLower();
                            foreach (OracleParameter commandParameter in command.Parameters)
                            {
                                if ((commandParameter.ParameterName.Replace("@", "").ToLower() == parameterName) &&
                                    ((commandParameter.Value != null) && (commandParameter.Value != DBNull.Value)))
                                {
                                    commandParameter.Value = forbiddenVarchars.Replace(
                                        commandParameter.Value.ToString(),
                                        ForbiddenVarcharsReplacement);
                                }
                            }
                            break;
                        case DbType.Boolean:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Binary:
                        case DbType.Byte:
                        case DbType.Object:
                        case DbType.Xml:
                            if (parameter.Value == null || string.IsNullOrEmpty(parameter.Value.ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Currency:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Date:
                        case DbType.DateTime:
                        case DbType.DateTime2:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                DateTime dt;
                                DateTime.TryParse(parameter.Value.ToString(), out dt);
                                OracleDate sqlDateTime;
                                try
                                {
                                    sqlDateTime = new OracleDate(dt);
                                }
                                catch
                                {
                                    sqlDateTime = OracleDate.MinValue;
                                }
                                if (sqlDateTime == OracleDate.MinValue || sqlDateTime == OracleDate.MaxValue)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            break;
                        case DbType.DateTimeOffset:
                        case DbType.Decimal:
                        case DbType.Double:
                        case DbType.Guid:
                        case DbType.Int16:
                        case DbType.Int32:
                        case DbType.Int64:
                        case DbType.UInt16:
                        case DbType.UInt32:
                        case DbType.UInt64:
                        case DbType.SByte:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Single:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.String:
                        case DbType.StringFixedLength:
                            if (parameter.Size > 8000)
                            {
                                parameter.Size = MaxAllowedCharForString;
                            }
                            if (parameter.Value == null || string.IsNullOrEmpty(parameter.Value.ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Time:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.VarNumeric:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                    }
                }
                catch
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }

        private static void AssertParameterCount(int numProcedureParameters, int numPassedParameters,
            int returnValueOffset, string procedureName)
        {
            // putting this back as compiler directives because
            // the assertion should be for local test code only
#if DEBUG
            if (numProcedureParameters != numPassedParameters + returnValueOffset)
            {
                STPConfigurationManager.LogProvider.Log(
                    string.Format(
                        "The incorrect number of parameters were supplied to the procedure {0}.  The number supplied was: {1}.  The number expected is: {2}.",
                        procedureName, numPassedParameters, numProcedureParameters - returnValueOffset), (string)null);
            }

#else
            if (numProcedureParameters < numPassedParameters + returnValueOffset)
            {
                //STPConfigurationManager.LogProvider.LogAndEmail(string.Format(
                //    "Too many parameters parameters were supplied to the procedure {0}.  The number supplied was: {1}.  The number expected is: {2}.",
                //    procedureName, numPassedParameters, numProcedureParameters - returnValueOffset));

                    STPConfigurationManager.LogProvider.Log(
        string.Format(
            "The incorrect number of parameters were supplied to the procedure {0}.  The number supplied was: {1}.  The number expected is: {2}.",
            procedureName, numPassedParameters, numProcedureParameters - returnValueOffset), (string)null);
            }
#endif
        }

        #endregion

        //private static Regex forbiddenVarchars = new Regex("[\u0100-\u0180\u019F-\u01A1\u0197\u019A\u01AB\u01AE-\u01B0\u01B6\u01CD-\u01F0\u0261\u02B9-\u02C8\u030E\u0393\u0398\u03A3\u03A6\u03A9\u03B1\u03B4\u03B5\u03C0\u03C3\u03C4\u03C6\u04BB\u2017\u2032\u2044\u207F\u20A7\u2102\u2107\u210A-\u2134\u2215\u221A\u2229\u2261\u2264\u2265\u2320\u2321\u2329\u232A\u3008\u3009\uFF02-\uFF09\uFF0F\uFF1C\uFF1D\uFF1E\u0268\u0275\u0288\u03B3\u03B8\u03C9]", RegexOptions.Compiled);

        #region All SQL Commands

        private static void MapParameters(SqlCommand command, object[] parameters)
        {
            const int returnValueOffset = 1;
            if (parameters == null)
            {
                AssertParameterCount(command.Parameters.Count, 0, returnValueOffset, command.CommandText);
                return;
            }
            AssertParameterCount(command.Parameters.Count, parameters.Length, returnValueOffset, command.CommandText);

            int j = parameters.Length;
            for (int i = 0 + returnValueOffset; i <= j; i++)
            {
                object parameterValue = parameters[i - 1] ?? DBNull.Value;
                command.Parameters[i].Value = parameterValue;
            }
        }

        private static void MapParameters(SqlCommand command, StoredProcedureParameterList parameters)
        {
            const int returnValueOffset = 1;

            int parameterCount = parameters.Count;
            if (parameters.Any(spp => spp.ParameterDirection == ParameterDirectionWrap.ReturnValue))
            {
                parameterCount--;
            }

            AssertParameterCount(command.Parameters.Count, parameterCount, returnValueOffset, command.CommandText);

            int j = parameters.Count;
            for (int i = 0 + returnValueOffset; i <= j; i++)
            {
                StoredProcedureParameter spp = parameters[i - 1];
                SqlParameter sqlParameter = spp.Key != null ? command.Parameters[spp.Key] : command.Parameters[i];

                sqlParameter.Value = spp.Value ?? DBNull.Value;

                switch (spp.ParameterDirection)
                {
                    case ParameterDirectionWrap.Input:
                        sqlParameter.Direction = ParameterDirection.Input;
                        break;
                    case ParameterDirectionWrap.Output:
                        sqlParameter.Direction = ParameterDirection.Output;
                        break;
                    case ParameterDirectionWrap.InputOutput:
                        sqlParameter.Direction = ParameterDirection.InputOutput;
                        break;
                    case ParameterDirectionWrap.ReturnValue:
                        sqlParameter.Direction = ParameterDirection.ReturnValue;
                        break;
                    default:
                        STPConfigurationManager.LogProvider.Log(
                            new ArgumentException(string.Format("Unknown parameter direction specified: {0}",
                                spp.ParameterDirection)));
                        break;
                }

                if (spp.Size.HasValue)
                    sqlParameter.Size = spp.Size.Value;
            }
        }

        internal static SqlCommand CreateParameterizedCommand(SqlConnection connection, string databaseInstanceName,
            string commandName)
        {
            if (commandName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                //STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, commandName));
            }
            SqlCommand command;
            using (var commandCache = new SqlCommandCache())
            {
                command = commandCache.GetCommandCopy(connection, databaseInstanceName, commandName);
            }
            return command;
        }

        /// <summary>
        ///     Creates and prepares SqlCommand object and calls SqlParameterMapper to populate command parameters
        /// </summary>
        /// <param name="connection"> </param>
        /// <param name="databaseInstanceName"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <returns> </returns>
        internal static SqlCommand CreateParameterMappedCommand(SqlConnection connection, string databaseInstanceName,
            string procedureName, SqlParameterMapper sqlParameterMapper)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                //STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }

            SqlCommand command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = CommandTimeOut;

            if (sqlParameterMapper != null)
            {
                var pSet = new SqlParameterSet(command.Parameters);
                sqlParameterMapper(pSet);
            }
            using (var commandCache = new SqlCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, procedureName).Parameters);
            }

            return command;
        }

        /// <summary>
        ///     Creates and prepares SqlCommand object and calls strongly typed SqlParameterMapper to populate command parameters
        /// </summary>
        internal static SqlCommand CreateParameterMappedCommand<T>(SqlConnection connection, string databaseInstanceName,
            string procedureName,
            SqlParameterMapper<T> sqlParameterMapper, T objectInstance)
        {
            //if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
              //  STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));

            SqlCommand command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = CommandTimeOut;

            if (sqlParameterMapper != null)
            {
                var pSet = new ParameterSet.SqlParameterSet(command.Parameters);
                sqlParameterMapper(pSet, objectInstance);
            }
            using (var commandCache = new SqlCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, procedureName).Parameters);
            }
            return command;
        }

        internal static SqlCommand CreateCommand(SqlConnection connection, string databaseInstanceName,
            string commandName, params object[] parameterValues)
        {
            using (var commandCache = new SqlCommandCache())
            {
                SqlCommand command = CreateParameterizedCommand(connection, databaseInstanceName, commandName);
                command.CommandTimeout = CommandTimeOut;
                MapParameters(command, parameterValues);
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, commandName).Parameters);
                LogWrapper.LogIfDebugEnabled(databaseInstanceName, commandName, command);
                return command;
            }
        }

        internal static SqlCommand CreateCommand(SqlConnection connection, string databaseInstanceName,
            string commandName, SqlParameterMapper sqlParameterMapper)
        {
            if (commandName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                //STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, commandName));
            }

            SqlCommand command = connection.CreateCommand();
            command.CommandText = commandName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = CommandTimeOut;

            if (sqlParameterMapper != null)
            {
                var pSet = new ParameterSet.SqlParameterSet(command.Parameters);
                sqlParameterMapper(pSet);
            }
            using (var commandCache = new SqlCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, commandName).Parameters);
                LogWrapper.LogIfDebugEnabled(databaseInstanceName, commandName, command);
            }

            return command;
        }

        /// <summary>
        ///     Creates and prepares an SqlCommand object and sets parameters from the parameter list either by their index value
        ///     or name.
        /// </summary>
        /// <returns> </returns>
        internal static SqlCommand CreateCommand(SqlConnection connection, string databaseInstanceName,
            string commandName, StoredProcedureParameterList parameterList)
        {
            using (var commandCache = new SqlCommandCache())
            {
                SqlCommand command = CreateParameterizedCommand(connection, databaseInstanceName, commandName);
                command.CommandTimeout = CommandTimeOut;
                MapParameters(command, parameterList);
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, commandName).Parameters);
                LogWrapper.LogIfDebugEnabled(databaseInstanceName, commandName, command);
                return command;
            }
        }

        #region Helper Methods
        private static void ApplySecurity(SqlCommand command, SqlParameterCollection parameterTypes)
        {
            command.CommandTimeout = CommandTimeOut;
            foreach (SqlParameter parameter in command.Parameters)
            {
                try
                {
                    switch (parameter.DbType)
                    {
                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                            if (parameter.Size > 8000)
                            {
                                parameter.Size = MaxAllowedCharForString;
                            }
                            if (parameter.Value == null || string.IsNullOrEmpty(parameter.Value.ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            string parameterName = parameter.ParameterName.Replace("@", "").ToLower();
                            foreach (SqlParameter commandParameter in command.Parameters)
                            {
                                if ((commandParameter.ParameterName.Replace("@", "").ToLower() == parameterName) &&
                                    ((commandParameter.Value != null) && (commandParameter.Value != DBNull.Value)))
                                {
                                    commandParameter.Value = forbiddenVarchars.Replace(
                                        commandParameter.Value.ToString(),
                                        ForbiddenVarcharsReplacement);
                                }
                            }
                            break;
                        case DbType.Boolean:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Binary:
                        case DbType.Byte:
                        case DbType.Object:
                        case DbType.Xml:
                            if (parameter.Value == null || string.IsNullOrEmpty(parameter.Value.ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Currency:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Date:
                        case DbType.DateTime:
                        case DbType.DateTime2:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                DateTime dt;
                                DateTime.TryParse(parameter.Value.ToString(), out dt);
                                SqlDateTime sqlDateTime;
                                try
                                {
                                    sqlDateTime = new SqlDateTime(dt);
                                }
                                catch
                                {
                                    sqlDateTime = SqlDateTime.MinValue;
                                }
                                if (sqlDateTime == SqlDateTime.MinValue || sqlDateTime == SqlDateTime.MaxValue)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            break;
                        case DbType.DateTimeOffset:
                        case DbType.Decimal:
                        case DbType.Double:
                        case DbType.Guid:
                        case DbType.Int16:
                        case DbType.Int32:
                        case DbType.Int64:
                        case DbType.UInt16:
                        case DbType.UInt32:
                        case DbType.UInt64:
                        case DbType.SByte:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Single:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.String:
                        case DbType.StringFixedLength:
                            if (parameter.Size > 8000)
                            {
                                parameter.Size = MaxAllowedCharForString;
                            }
                            if (parameter.Value == null || string.IsNullOrEmpty(parameter.Value.ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Time:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                        case DbType.VarNumeric:
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            break;
                    }
                }
                catch
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }
        #endregion

        #endregion


 

    }
}