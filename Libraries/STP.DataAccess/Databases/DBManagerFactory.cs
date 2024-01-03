#region

using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using STP.Common;
using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.Common.Log;
using STP.DataAccess.Command;
using STP.DataAccess.Delegates;
using STP.DataAccess.Exceptions;
using STP.DataAccess.ParameterSet;

#endregion

namespace STP.DataAccess.Databases
{
    internal sealed class DBManagerFactory
    {
        #region Constructors

        private DBManagerFactory()
        {
        }

        #endregion

        #region Interface Methods

        internal static IDbConnection GetConnection(DataProvider providerType)
        {
            IDbConnection iDbConnection;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    iDbConnection = new SqlConnection();
                    break;
                case DataProvider.OleDb:
                    iDbConnection = new OleDbConnection();
                    break;
                case DataProvider.Odbc:
                    iDbConnection = new OdbcConnection();
                    break;
                case DataProvider.Oracle:
                    iDbConnection = new OracleConnection();
                    break;
                default:
                    return null;
            }
            return iDbConnection;
        }

        internal static IDbCommand GetCommand(DataProvider providerType)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    var sqlCommand = new SqlCommand {CommandTimeout = CommandFactory.CommandTimeOut};
                    return sqlCommand;
                case DataProvider.OleDb:
                    var oleDbCommand = new OleDbCommand {CommandTimeout = CommandFactory.CommandTimeOut};
                    return oleDbCommand;
                case DataProvider.Odbc:
                    var odbcCommand = new OdbcCommand {CommandTimeout = CommandFactory.CommandTimeOut};
                    return odbcCommand;
                case DataProvider.Oracle:
                    var oracleCommand = new OracleCommand {CommandTimeout = CommandFactory.CommandTimeOut};
                    return oracleCommand;
                default:
                    return null;
            }
        }

        internal static IDbDataAdapter GetDataAdapter(DataProvider providerType)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    return new SqlDataAdapter();
                case DataProvider.OleDb:
                    return new OleDbDataAdapter();
                case DataProvider.Odbc:
                    return new OdbcDataAdapter();
                case DataProvider.Oracle:
                    return new OracleDataAdapter();
                default:
                    return null;
            }
        }

        internal static IDbTransaction GetTransaction(DataProvider providerType)
        {
            IDbConnection iDbConnection = GetConnection(providerType);
            IDbTransaction iDbTransaction = iDbConnection.BeginTransaction();
            return iDbTransaction;
        }

        internal static IDataParameter GetParameter(DataProvider providerType)
        {
            IDataParameter iDataParameter = null;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    iDataParameter = new SqlParameter();
                    break;
                case DataProvider.OleDb:
                    iDataParameter = new OleDbParameter();
                    break;
                case DataProvider.Odbc:
                    iDataParameter = new OdbcParameter();
                    break;
                case DataProvider.Oracle:
                    iDataParameter = new OracleParameter();
                    break;
            }
            return iDataParameter;
        }

        internal static IDbDataParameter[] GetParameters(DataProvider providerType, int paramsCount)
        {
            var idbParams = new IDbDataParameter[paramsCount];

            switch (providerType)
            {
                case DataProvider.SqlServer:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new SqlParameter();
                    }
                    break;
                case DataProvider.OleDb:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new OleDbParameter();
                    }
                    break;
                case DataProvider.Odbc:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new OdbcParameter();
                    }
                    break;
                case DataProvider.Oracle:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new OracleParameter();
                    }
                    break;
                default:
                    idbParams = null;
                    break;
            }
            return idbParams;
        }

        #region Get Commands

        internal static IDbCommand CreateParameterizedCommand(DataProvider providerType, IDbConnection connection,
            DatabaseSource databaseSource, string procedureName)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                //STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }
            return CommandFactory.CreateParameterizedCommand((SqlConnection)connection,
                       databaseSource.ToString(), procedureName);
        }

        /// <summary>
        ///     Creates and prepares SqlCommand object and calls SqlParameterMapper to populate command parameters
        /// </summary>
        /// <param name="providerType"> </param>
        /// <param name="connection"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <returns> </returns>
        internal static IDbCommand CreateParameterMappedCommand(DataProvider providerType, IDbConnection connection,
            DatabaseSource databaseSource, string procedureName,
            SqlParameterMapper sqlParameterMapper)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
               // STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }
            return CommandFactory.CreateParameterMappedCommand((SqlConnection)connection,
                     databaseSource.ToString(), procedureName,
                     sqlParameterMapper);
        }

        /// <summary>
        ///     Creates and prepares SqlCommand object and calls strongly typed SqlParameterMapper to populate command parameters
        /// </summary>
        internal static IDbCommand CreateParameterMappedCommand<T>(DataProvider providerType, IDbConnection connection,
            DatabaseSource databaseSource, string procedureName,
            SqlParameterMapper<T> sqlParameterMapper, T objectInstance)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                //STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }
            return CommandFactory.CreateParameterMappedCommand((SqlConnection)connection,
                        databaseSource.ToString(), procedureName,
                        sqlParameterMapper, objectInstance);
        }

        internal static IDbCommand CreateCommand(DataProvider providerType, IDbConnection connection,
            DatabaseSource databaseSource, string procedureName,
            params object[] parameterValues)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
               // STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }
            return CommandFactory.CreateCommand((SqlConnection)connection, databaseSource.ToString(),
                      procedureName, parameterValues);
        }

        internal static IDbCommand CreateCommand(DataProvider providerType, IDbConnection connection,
            DatabaseSource databaseSource, string procedureName,
            SqlParameterMapper sqlParameterMapper)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
               // STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }
            return CommandFactory.CreateCommand((SqlConnection)connection, databaseSource.ToString(),
                      procedureName, sqlParameterMapper);
        }


        /// <summary>
        ///     Creates and prepares an SqlCommand object and sets parameters from the parameter list either by their index value
        ///     or name.
        /// </summary>
        /// <returns> </returns>
        internal static IDbCommand CreateCommand(DataProvider providerType, IDbConnection connection,
            DatabaseSource databaseSource, string procedureName,
            StoredProcedureParameterList parameterList)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                //STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }
            return CommandFactory.CreateCommand((SqlConnection)connection, databaseSource.ToString(),
                       procedureName, parameterList);
        }

        #endregion

        #endregion
    }
}