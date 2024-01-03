#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using STP.Common;
using STP.Common.Enums;
using STP.Common.Log;
using STP.DataAccess.BaseHelper;
using STP.DataAccess.Databases;
using STP.DataAccess.Debug;
using STP.DataAccess.Delegates;
using STP.DataAccess.Exceptions;
using STP.DataAccess.Interface;
using STP.DataAccess.Mappers;
using STP.DataAccess.ParameterSet;
using STP.Common.Logger;

#endregion

namespace STP.DataAccess.Provider
{
    /// <summary>
    ///     The OracleHelper class is intended to encapsulate high performance, scalable best practices for
    ///     common uses of OracleClient.
    /// </summary>
    public sealed class OracleProvider : IOracleProvider
    {
        #region Private Declaration

        private const DatabaseSource databaseSource = DatabaseSource.ORACLE;
        private static volatile OracleProvider instance;
        // Lock synchronization object
        private static readonly object syncLock = new object();
        private readonly int commandTimeout;
        private readonly string connectionString = string.Empty;
        private readonly int maxAllowedCharForString;

        private const string ForbiddenVarcharsReplacement = "?";

        /// <summary>
        ///     Basically, all non-ascii characters. We're banning anything above 255 in non-unicode fields
        /// </summary>
        private static readonly Regex forbiddenVarchars = new Regex("[^\u0009-\u00FF]", RegexOptions.Compiled);

        #endregion

        #region OracleProvider Singleton
        private OracleProvider()
        {
            connectionString = ConnectionManager.GetConnectionString(DatabaseSource.ORACLE);
            try
            {
                commandTimeout = STPConfigurationManager.ConfigProvider.DatabaseConfigProvider.CommandTimeout;
            }
            catch
            {
                commandTimeout = 180;
            }
            try
            {
                maxAllowedCharForString = STPConfigurationManager.ConfigProvider.DatabaseConfigProvider.VarcharMax;
            }
            catch
            {
                maxAllowedCharForString = 8000;
            }
        }
        internal static OracleProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly OracleProvider instance = new OracleProvider();
        }

        #region Logger instance

        private const string PolicyName = "OracleProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Implementation of IOracleProvider

        public bool CheckDatabaseConnection()
        {
            bool isValidConnection = false;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    isValidConnection = true;
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("OracleProvider/CheckDatabaseConnection, Exception: {0}", ex));
                    OracleConnection.ClearAllPools();
                    //OracleConnection.ClearPool(connection);
                    //throw ex;
                    throw new DBConnectionFailedException();
                }
                finally
                {
                    connection.Close();
                }
                return isValidConnection;
            }
        }

        public string GetServerIP()
        {
            using (var connection = new OracleConnection(connectionString))
            {
            }
            return string.Empty;
        }

        #region ExecuteNonQuery

        public int ExecuteNonQuery(string procedureName, OracleParameterMapper oracleParameterMapper,
            OracleOutputParameterMapper oracleOutputMapper)
        {
            int result = 0;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    result = oracleCommand.ExecuteNonQuery();
                    if (oracleOutputMapper != null)
                    {
                        var outputParams = new OracleParameterSet(oracleCommand.Parameters);
                        oracleOutputMapper(outputParams);
                    }
                }
                catch (OracleException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return result;
        }

        public int ExecuteNonQuery(string procedureName, OracleParameterMapper oracleParameterMapper)
        {
            int result = 0;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    connection.Open();
                    result = oracleCommand.ExecuteNonQuery();
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return result;
        }

        public int ExecuteNonQuery<T>(string procedureName, OracleParameterMapper<T> oracleParameterMapper,
            T objectInstance)
        {
            int result = 0;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper, objectInstance);
                    result = oracleCommand.ExecuteNonQuery();
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return result;
        }

        public int ExecuteBulkNonQuery(string insertQuery, OracleParameterMapper oracleParameterMapper, int count)
        {
            int result = 0;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedBulkCommand(connection, DatabaseSource.ORACLE.ToString(), insertQuery, oracleParameterMapper, count);
                    result = oracleCommand.ExecuteNonQuery();
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, insertQuery, e));
                    throw;
                }
            }
            return result;
        }

        public bool ExecuteBulkProcedure(string procedureName, OracleParameterMapper oracleParameterMapper, int count)
        {
            bool result = false;
            var dt = new DataTable(procedureName);
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedBulkProcedure(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper, count);
                    var output = oracleCommand.ExecuteReader();

                    if (output != null)
                    {
                        result = true;
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return result;
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns no resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="procedureName">>the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(string procedureName, params object[] parameterValues)
        {
            int result = 0;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateCommand(connection, DatabaseSource.ORACLE.ToString(), procedureName,
                        parameterValues);
                    connection.Open();
                    result = oracleCommand.ExecuteNonQuery();
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return result;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        public object ExecuteScalar(string procedureName, OracleParameterMapper oracleParameterMapper)
        {
            return ExecuteScalar(procedureName, oracleParameterMapper, null);
        }

        public object ExecuteScalar(string procedureName, OracleParameterMapper oracleParameterMapper,
            OracleOutputParameterMapper oracleOutputMapper)
        {
            object result = null;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var command = CreateCommand(connection, procedureName, oracleParameterMapper);
                    result = command.ExecuteScalar();
                    if (oracleOutputMapper != null)
                    {
                        var outputParams = new OracleParameterSet(command.Parameters);
                        oracleOutputMapper(outputParams);
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return result;
        }

        public object ExecuteScalar(string procedureName, params object[] parameterValues)
        {
            object result = null;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var command = CreateCommand(connection, DatabaseSource.ORACLE.ToString(), procedureName,
                        parameterValues);
                    result = command.ExecuteScalar();
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return result;
        }

        #endregion ExecuteScalar

        #region Execute

        public DataTable Execute(string procedureName, params object[] parameters)
        {
            var dt = new DataTable(procedureName);
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateCommand(connection, DatabaseSource.ORACLE.ToString(), procedureName,
                        parameters);
                    using (
                        IDataReader reader = ExecuteReader(connection, null, oracleCommand,
                            OracleConnectionOwnership.Internal))
                    {
                        dt.Load(reader, LoadOption.OverwriteChanges);
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return dt;
        }

        public DataTable Execute(string procedureName, OracleParameterMapper oracleParameterMapper)
        {
            var dt = new DataTable(procedureName);
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IDataReader reader = ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal))
                    {
                        dt.Load(reader, LoadOption.OverwriteChanges);
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                }
            }
            return dt;
        }

        #endregion

        #region ExecuteAndGetDictionary

        public Dictionary<int, T> ExecuteAndGetDictionary<T>(string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper) where T : new()
        {
            var dictionaryList = new Dictionary<int, T>();

            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            var objectInstance = new T();
                            recordMapper(reader, objectInstance);
                            dictionaryList[reader.GetInt32(0)] = objectInstance;
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, typeof(T), e));
                }
            }
            return dictionaryList;
        }

        public Dictionary<int, T> ExecuteAndGetDictionary<T>(string procedureName, RecordMapper<T> recordMapper,
            params object[] parameters) where T : new()
        {
            var dictionaryList = new Dictionary<int, T>();
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateCommand(connection, DatabaseSource.ORACLE.ToString(), procedureName,
                        parameters);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                dictionaryList[reader.GetInt32(0)] = objectInstance;
                            }
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, typeof(T), e));
                }
            }
            return dictionaryList;
        }

        #endregion

        #region ExecuteAndMapResults

        public void ExecuteAndMapResults(string procedureName, StoredProcedureParameterList parameterList,
            ResultMapper result,
            OracleOutputParameterMapper oracleOutputMapper)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var oracleCommand = new OracleCommand
                    {
                        CommandText = procedureName,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = commandTimeout
                    };
                    MapParameters(oracleCommand, parameterList);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        result(reader);
                    }
                    if (oracleOutputMapper != null)
                        oracleOutputMapper(new OracleParameterSet(oracleCommand.Parameters));
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndMapResults(string procedureName, OracleParameterMapper oracleParameterMapper,
            ResultMapper result)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName,
                        oracleParameterMapper);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        result(reader);
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndMapResults(string procedureName, ResultMapper result, params object[] parameters)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var oracleCommand = new OracleCommand
                    {
                        CommandText = procedureName,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = commandTimeout
                    };
                    MapParameters(oracleCommand, parameters);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        result(reader);
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        #endregion

        #region ExecuteAndMapRecords

        public void ExecuteAndMapRecords(string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper recordMapper)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName,
                        oracleParameterMapper);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                                recordMapper(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndMapRecords(string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper recordMapper,
            OracleOutputParameterMapper oracleOutputMapper)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName,
                        oracleParameterMapper);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                                recordMapper(reader);
                            if (oracleOutputMapper != null)
                                oracleOutputMapper(
                                    new OracleParameterSet(oracleCommand.Parameters));
                        }
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndMapRecords(string procedureName, RecordMapper recordMapper, params object[] parameters)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var oracleCommand = new OracleCommand
                    {
                        CommandText = procedureName,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = commandTimeout
                    };
                    MapParameters(oracleCommand, parameters);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                                recordMapper(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        #endregion

        #region ExecuteAndGetInstance

        public T ExecuteAndGetInstance<T>(string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper) where T : new()
        {
            var objectInstance = new T();
            if (!ExecuteAndHydrateInstance(objectInstance, procedureName, oracleParameterMapper, recordMapper))
                return default(T);

            return objectInstance;
        }

        public T ExecuteAndGetInstance<T>(string procedureName, RecordMapper<T> recordMapper, params object[] parameters)
            where T : new()
        {
            var objectInstance = new T();
            if (!ExecuteAndHydrateInstance(objectInstance, procedureName, recordMapper, parameters))
                return default(T);

            return objectInstance;
        }

        #endregion

        #region ExecuteAndGetInstanceList

        public List<T> ExecuteAndGetInstanceList<T>(string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper) where T : new()
        {
            var instanceList = new List<T>();
            ExecuteAndHydrateInstanceList(instanceList, procedureName, oracleParameterMapper, recordMapper);
            return instanceList;
        }

        public List<T> ExecuteAndGetInstanceList<T>(string procedureName, RecordMapper<T> recordMapper,
            params object[] parameters) where T : new()
        {
            var instanceList = new List<T>();
            ExecuteAndHydrateInstanceList(instanceList, procedureName, recordMapper, parameters);
            return instanceList;
        }

        #endregion

        #region ExecuteAndHydrateGenericInstance

        public void ExecuteAndHydrateGenericInstance<T>(T objectInstance, string procedureName,
            OracleParameterMapper oracleParameterMapper, RecordMapper<T>[] recordMappers) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        //Get all the recordsets, call each delegate in the delegate array to map the generic entity instance.
                        //Note: that if the number of delegates in the array does not match up with the number of recordsets returned
                        //Note: the function exit's normally, it will only call the delegates that are in the array.
                        int mapperIndex = 0;
                        do
                        {
                            if (mapperIndex < recordMappers.Length)
                            {
                                if (reader.Read())
                                {
                                    while (reader.Read())
                                    {
                                        recordMappers[mapperIndex](reader, objectInstance);
                                    }

                                    mapperIndex++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (reader.NextResult());
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, typeof(T), e));
                }
            }
        }

        public void ExecuteAndHydrateGenericInstance<T>(T objectInstance, string procedureName,
            RecordMapper<T>[] recordMappers,
            params object[] parameters) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var oracleCommand = new OracleCommand
                    {
                        CommandText = procedureName,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = commandTimeout
                    };
                    MapParameters(oracleCommand, parameters);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        //Get all the recordsets, call each delegate in the delegate array to map the generic entity instance.
                        //Note that if the number of delegates in the array does not match up with the number of recordsets returned
                        //the function exit normally, it will only call the delegates that are in the array.
                        int mapperIndex = 0;
                        do
                        {
                            if (mapperIndex < recordMappers.Length)
                            {
                                if (reader.Read())
                                {
                                    while (reader.Read())
                                    {
                                        recordMappers[mapperIndex](reader, objectInstance);
                                    }
                                    mapperIndex++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (reader.NextResult());
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, typeof(T), e));
                }
            }
        }

        #endregion

        #region ExecuteAndHydrateInstance

        public bool ExecuteAndHydrateInstance<T>(T objectInstance, string procedureName, RecordMapper<T> recordMapper,
            params object[] parameters)
        {
            bool result = false;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateCommand(connection, DatabaseSource.ORACLE.ToString(), procedureName,
                        parameters);
                    using (
                        IRecordSet reader =
                            new DataRecord(ExecuteReader(connection, null, oracleCommand,
                                OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            recordMapper(reader, objectInstance);
                            result = true;
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (DBConnectionFailedException dbEx)
                {
                    throw dbEx;
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, e));
                    //throw e;
                }
            }
            return result;
        }

        public bool ExecuteAndHydrateInstance<T>(T objectInstance, string procedureName,
            OracleParameterMapper<T> oracleParameterMapper,
            RecordMapper<T> recordMapper)
        {
            bool result = false;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {

                    if (!CheckDatabaseConnection())
                    {
                        throw new DBConnectionFailedException();
                    }
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper, objectInstance);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            recordMapper(reader, objectInstance);
                            result = true;
                        }
                    }
                }
                catch (SafeProcedureDataException e) // Procedure class already wrapped all necessary data
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (DBConnectionFailedException dbEx)
                {
                    STPConfigurationManager.LogProvider.Log(dbEx);
                    throw dbEx;
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, typeof(T), e));
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Exception : {0},ProcedureName: {1} ", e, procedureName));
                    throw e;
                }
            }
            return result;
        }

        public bool ExecuteAndHydrateInstance<T>(T objectInstance, string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper)
        {
            bool result = false;
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            recordMapper(reader, objectInstance);
                            result = true;
                        }
                    }
                }
                catch (SafeProcedureDataException e) // Procedure class already wrapped all necessary data
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, typeof(T), e));
                  throw;
                }
            }
            return false;
        }

        public bool ExecuteAndHydrateInstance<T>(T objectInstance, string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper recordMapper)
        {
            return ExecuteAndHydrateInstance(objectInstance, procedureName,
                (parameterSet, obj) => oracleParameterMapper(parameterSet),
                (reader, obj) => recordMapper(reader));
        }

        public bool ExecuteAndHydrateInstance<T>(T objectInstance, string procedureName, RecordMapper recordMapper,
            params object[] parameters)
        {
            return ExecuteAndHydrateInstance(objectInstance, procedureName, (reader, obj) => recordMapper(reader),
                parameters);
        }

        #endregion

        #region ExecuteAndHydrateInstanceList

        public void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, string procedureName,
            OracleParameterMapper oracleParameterMapper, RecordMapper<T> recordMapper) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        //if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                instanceList.Add(objectInstance);
                            }
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                    throw;
                  
                }
            }
        }

        public void ExecuteAndHydrateInstanceList<TConcrete, TList>(ICollection<TList> instanceList,
            string procedureName,
            OracleParameterMapper oracleParameterMapper, RecordMapper<TConcrete> recordMapper)
            where TConcrete : TList, new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new TConcrete();
                                recordMapper(reader, objectInstance);
                                instanceList.Add(objectInstance);
                            }
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                    throw;
                }
            }
        }

        public void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, string procedureName,
            RecordMapper<T> recordMapper,
            params object[] parameters) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var oracleCommand = new OracleCommand
                    {
                        CommandText = procedureName,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = commandTimeout
                    };
                    MapParameters(oracleCommand, parameters);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                instanceList.Add(objectInstance);
                            }
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, string procedureName,
            RecordMapper recordMapper,
            params object[] parameters) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var oracleCommand = new OracleCommand
                    {
                        CommandText = procedureName,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = commandTimeout
                    };
                    MapParameters(oracleCommand, parameters);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader);
                                instanceList.Add(objectInstance);
                            }
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, string procedureName,
            OracleParameterMapper oracleParameterMapper, RecordMapper recordMapper,
            OracleOutputParameterMapper oracleOutputMapper) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader);
                                instanceList.Add(objectInstance);
                            }
                        }
                        if (oracleOutputMapper != null)
                        {
                            var outputParams =
                                new OracleParameterSet(oracleCommand.Parameters);
                            oracleOutputMapper(outputParams);
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, string procedureName,
            OracleParameterMapper oracleParameterMapper, RecordMapper<T> recordMapper,
            OracleOutputParameterMapper oracleOutputMapper) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                instanceList.Add(objectInstance);
                            }
                        }
                        if (oracleOutputMapper != null)
                        {
                            var outputParams =
                                new OracleParameterSet(oracleCommand.Parameters);
                            oracleOutputMapper(outputParams);
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        public void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, string procedureName,
            StoredProcedureParameterList parameterList, RecordMapper<T> recordMapper,
            OracleOutputParameterMapper oracleOutputMapper) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var oracleCommand = new OracleCommand
                    {
                        CommandText = procedureName,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = commandTimeout
                    };
                    MapParameters(oracleCommand, parameterList);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                instanceList.Add(objectInstance);
                            }
                        }
                        if (oracleOutputMapper != null)
                        {
                            var outputParams =
                                new OracleParameterSet(oracleCommand.Parameters);
                            oracleOutputMapper(outputParams);
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        #endregion

        #region ExecuteDataset

        public void ExecuteDataset<T>(List<T> instanceList, string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper) where T : new()
        {
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    var oracleCommand = CreateParameterMappedCommand(connection, DatabaseSource.ORACLE.ToString(),
                        procedureName, oracleParameterMapper);
                    using (IRecordSet reader = new DataRecord(ExecuteReader(connection, null, oracleCommand,
                        OracleConnectionOwnership.Internal)))
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                instanceList.Add(objectInstance);
                            }
                        }
                    }
                }
                catch (SafeProcedureDataException e)
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        #endregion

        #endregion Implementation of IOracleProvider

        #region Private Helper Methods

        #region private utility methods & constructors

        /// <summary>
        ///     This method opens (if necessary) and assigns a connection, transaction, command type and parameters
        ///     to the provided command.
        /// </summary>
        /// <param name="command">the OracleCommand to be prepared</param>
        /// <param name="connection">a valid OracleConnection, on which to execute this command</param>
        /// <param name="transaction">a valid OracleTransaction, or 'null'</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">
        ///     an array of OracleParameters to be associated with the command or 'null' if no
        ///     parameters are required
        /// </param>
        private void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction,
            CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            //if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //associate the connection with the command
            command.Connection = connection;

            //set the command text (stored procedure name or Oracle statement)
            command.CommandText = commandText;

            //if we were provided a transaction, assign it.
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            //set the command type
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return;
        }


        private void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction,
            CommandType commandType, string commandText, OracleParameter[] commandParameters,
            out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null)
                    throw new ArgumentException(
                        "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        /// <summary>
        ///     This method is used to attach array's of OracleParameters to an OracleCommand.
        ///     This method will assign a value of DbNull to any parameter with a direction of
        ///     InputOutput and a value of null.
        ///     This behavior will prevent default values from being used, but
        ///     this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ///     where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">an array of OracleParameters tho be added to command</param>
        private void AttachParameters(OracleCommand command, IEnumerable<OracleParameter> commandParameters)
        {
            foreach (OracleParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        /// <summary>
        ///     This method assigns an array of values to an array of OracleParameters.
        /// </summary>
        /// <param name="commandParameters">array of OracleParameters to be assigned values</param>
        /// <param name="parameterValues">array of objects holding the values to be assigned</param>
        private void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                //do nothing if we get no data
                return;
            }

            // we must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            //iterate through the OracleParameters, assigning the values from the corresponding position in the 
            //value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }

        #endregion private utility methods & constructors

        #region ExecuteNonQuery

        #endregion ExecuteNonQuery

        #region Security

        private void ApplySecurity(OracleCommand command, OracleParameterCollection parameterTypes)
        {

            command.CommandTimeout = commandTimeout;
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
                                parameter.Size = maxAllowedCharForString;
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
                                parameter.Size = maxAllowedCharForString;
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
                            //OracleDbType.Number;
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

        private void AssertParameterCount(int numProcedureParameters, int numPassedParameters,
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

        private void MapParameters(OracleCommand command, object[] parameters)
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

        private void MapParameters(OracleCommand command, StoredProcedureParameterList parameters)
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
                OracleParameter oracleParameter = spp.Key != null ? command.Parameters[spp.Key] : command.Parameters[i];

                oracleParameter.Value = spp.Value ?? DBNull.Value;

                switch (spp.ParameterDirection)
                {
                    case ParameterDirectionWrap.Input:
                        oracleParameter.Direction = ParameterDirection.Input;
                        break;
                    case ParameterDirectionWrap.Output:
                        oracleParameter.Direction = ParameterDirection.Output;
                        break;
                    case ParameterDirectionWrap.InputOutput:
                        oracleParameter.Direction = ParameterDirection.InputOutput;
                        break;
                    case ParameterDirectionWrap.ReturnValue:
                        oracleParameter.Direction = ParameterDirection.ReturnValue;
                        break;
                    default:
                        STPConfigurationManager.LogProvider.Log(
                            new ArgumentException(string.Format("Unknown parameter direction specified: {0}",
                                spp.ParameterDirection)));
                        break;
                }

                if (spp.Size.HasValue)
                    oracleParameter.Size = spp.Size.Value;
            }
        }

        internal OracleCommand CreateParameterizedCommand(OracleConnection connection, string databaseInstanceName,
            string commandName)
        {
            if (commandName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {//
                STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, commandName));
            }
            connection.Open();
            OracleCommand command;
            using (var commandCache = new OracleCommandCache())
            {
                command = commandCache.GetCommandCopy(connection, databaseInstanceName, commandName);
            }
            return command;
        }

        /// <summary>
        ///     Creates and prepares OracleCommand object and calls parameterMapper to populate command parameters
        /// </summary>
        /// <param name="connection"> </param>
        /// <param name="databaseInstanceName"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="parameterMapper"> </param>
        /// <returns> </returns>
        internal OracleCommand CreateParameterMappedCommand(OracleConnection connection, string databaseInstanceName,
            string procedureName, OracleParameterMapper parameterMapper)
        {
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                // STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
            }
            connection.Open();
            OracleCommand command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = commandTimeout;

            if (parameterMapper != null)
            {
                var pSet = new OracleParameterSet(command.Parameters);
                parameterMapper(pSet);
#if DEBUG
                PrintStoredProcedureParameters(connection.DatabaseName, procedureName, command);
#endif
            }
            using (var commandCache = new OracleCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, procedureName).Parameters);
            }

            return command;
        }

        /// <summary>
        ///     Creates and prepares OracleCommand object and calls strongly typed parameterMapper to populate command parameters
        /// </summary>
        internal OracleCommand CreateParameterMappedCommand<T>(OracleConnection connection, string databaseInstanceName,
            string procedureName, OracleParameterMapper<T> parameterMapper, T objectInstance)
        {
            connection.Open();
            //if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            // STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));

            OracleCommand command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = commandTimeout;

            if (parameterMapper != null)
            {
                var pSet = new OracleParameterSet(command.Parameters);
                parameterMapper(pSet, objectInstance);
#if DEBUG
                PrintStoredProcedureParameters(connection.DatabaseName, procedureName, command);
#endif
            }
            using (var commandCache = new OracleCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, procedureName).Parameters);
            }
            return command;
        }

        private static void PrintStoredProcedureParameters(string DatabaseName, string procedureName, OracleCommand command)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(DatabaseName + "  *****  " + procedureName + ";");
                System.Diagnostics.Debug.WriteLine("********************************************************************************");
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    var item = command.Parameters[i];
                    string Value = "NULL";
                    if (item.Value != null && item.Value != "")
                    {
                        Value = item.Value.ToString();
                        if (Value.Contains(','))
                        {
                            Value = "'" + Value + "'";
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(item.ParameterName + " := " + Value + ";");
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        }

        // <summary>
        ///     Creates and prepares OracleCommand object and calls strongly typed parameterMapper to populate command parameters
        /// </summary>
        internal OracleCommand CreateParameterMappedBulkCommand(OracleConnection connection, string databaseInstanceName,
            string insertQuery, OracleParameterMapper parameterMapper, int count)
        {
            connection.Open();
            //if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            // STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));

            OracleCommand command = connection.CreateCommand();
            command.CommandText = insertQuery;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = commandTimeout;
            command.BindByName = true;
            command.ArrayBindCount = count;
            if (parameterMapper != null)
            {
                var pSet = new OracleParameterSet(command.Parameters);
                parameterMapper(pSet);
            }
            return command;
        }

        /// <summary>
        ///     Creates and prepares OracleCommand object and calls strongly typed parameterMapper to populate command parameters
        /// </summary>
        internal OracleCommand CreateParameterMappedBulkProcedure(OracleConnection connection, string databaseInstanceName,
            string procedureName, OracleParameterMapper parameterMapper, int count)
        {
            connection.Open();

            OracleCommand command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = commandTimeout;
            command.BindByName = true;
            command.ArrayBindCount = count;
            if (parameterMapper != null)
            {
                var pSet = new OracleParameterSet(command.Parameters);
                parameterMapper(pSet);
            }
            using (var commandCache = new OracleCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, procedureName).Parameters);
            }
            return command;
        }

        internal OracleCommand CreateCommand(OracleConnection connection, string databaseInstanceName,
            string commandName, params object[] parameterValues)
        {
            connection.Open();
            using (var commandCache = new OracleCommandCache())
            {
                OracleCommand command = CreateParameterizedCommand(connection, databaseInstanceName, commandName);
                command.CommandTimeout = commandTimeout;
                MapParameters(command, parameterValues);
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, commandName).Parameters);
                LogWrapper.LogIfDebugEnabled(databaseInstanceName, commandName, command);
                return command;
            }
        }

        internal OracleCommand CreateCommand(OracleConnection connection, string databaseInstanceName,
            string commandName, OracleParameterMapper parameterMapper)
        {
            if (commandName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, commandName));
            }
            connection.Open();
            OracleCommand command = connection.CreateCommand();
            command.CommandText = commandName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = commandTimeout;

            if (parameterMapper != null)
            {
                var pSet = new OracleParameterSet(command.Parameters);
                parameterMapper(pSet);
            }
            using (var commandCache = new OracleCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, commandName).Parameters);
                LogWrapper.LogIfDebugEnabled(databaseInstanceName, commandName, command);
            }

            return command;
        }

        /// <summary>
        ///     Creates and prepares an OracleCommand object and sets parameters from the parameter list either by their index
        ///     value
        ///     or name.
        /// </summary>
        /// <returns> </returns>
        internal OracleCommand CreateCommand(OracleConnection connection, string databaseInstanceName,
            string commandName, StoredProcedureParameterList parameterList)
        {
            connection.Open();
            using (var commandCache = new OracleCommandCache())
            {
                var command = CreateParameterizedCommand(connection, databaseInstanceName, commandName);
                command.CommandTimeout = commandTimeout;
                MapParameters(command, parameterList);
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, databaseInstanceName, commandName).Parameters);
                LogWrapper.LogIfDebugEnabled(databaseInstanceName, commandName, command);
                return command;
            }
        }

        internal OracleCommand CreateCommand(OracleConnection connection, string commandName,
            OracleParameterMapper parameterMapper)
        {

            if (commandName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
            {
                STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, commandName));
            }
            connection.Open();
            OracleCommand command = connection.CreateCommand();
            command.CommandText = commandName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = commandTimeout;

            if (parameterMapper != null)
            {
                var pSet = new OracleParameterSet(command.Parameters);
                parameterMapper(pSet);
            }
            using (var commandCache = new OracleCommandCache())
            {
                ApplySecurity(command,
                    commandCache.GetCommandCopy(connection, DatabaseSource.ORACLE.ToString(), commandName).Parameters);
                LogWrapper.LogIfDebugEnabled(string.Empty, commandName, command);
            }

            return command;
        }

        #endregion

        #endregion

        #region Internals

        /// <summary>
        ///     this enum is used to indicate weather the connection was provided by the caller, or created by OracleHelper, so
        ///     that
        ///     we can set the appropriate CommandBehavior when calling ExecuteReader()
        /// </summary>
        internal enum OracleConnectionOwnership
        {
            /// <summary>Connection is owned and managed by OracleHelper</summary>
            Internal,

            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }

        #region ExecuteReader

        /// <summary>
        ///     Create and prepare an OracleCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// </summary>
        /// <remarks>
        ///     If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        ///     If the caller provided the connection, we want to leave it to them to manage.
        /// </remarks>
        /// <param name="connection">a valid OracleConnection, on which to execute this command</param>
        /// <param name="transaction">a valid OracleTransaction, or 'null'</param>
        /// <param name="oracleCommand"></param>
        /// <param name="connectionOwnership">
        ///     indicates whether the connection parameter was provided by the caller, or created by
        ///     OracleHelper
        /// </param>
        /// <returns>OracleDataReader containing the results of the command</returns>
        internal OracleDataReader ExecuteReader(OracleConnection connection, OracleTransaction transaction,
            OracleCommand oracleCommand, OracleConnectionOwnership connectionOwnership)
        {
            //create a reader
            // call ExecuteReader with the appropriate CommandBehavior
            OracleDataReader dr = connectionOwnership == OracleConnectionOwnership.External
                ? oracleCommand.ExecuteReader()
                : oracleCommand.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return dr;
        }


        /// <summary>
        ///     Create and prepare an OracleCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// </summary>
        /// <remarks>
        ///     If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        ///     If the caller provided the connection, we want to leave it to them to manage.
        /// </remarks>
        /// <param name="connection">a valid OracleConnection, on which to execute this command</param>
        /// <param name="transaction">a valid OracleTransaction, or 'null'</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">
        ///     an array of OracleParameters to be associated with the command or 'null' if no
        ///     parameters are required
        /// </param>
        /// <param name="connectionOwnership">
        ///     indicates whether the connection parameter was provided by the caller, or created by
        ///     OracleHelper
        /// </param>
        /// <returns>OracleDataReader containing the results of the command</returns>
        internal OracleDataReader ExecuteReader(OracleConnection connection, OracleTransaction transaction,
            CommandType commandType, string commandText, OracleParameter[] commandParameters,
            OracleConnectionOwnership connectionOwnership)
        {
            //create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            //create a reader
            OracleDataReader dr;

            // call ExecuteReader with the appropriate CommandBehavior
            dr = connectionOwnership == OracleConnectionOwnership.External ? cmd.ExecuteReader() : cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));

            return (OracleDataReader)dr;
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteReader(connectionString, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new
        ///     OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //create & open an OraclebConnection
            OracleConnection cn = new OracleConnection(connectionString);
            cn.Open();

            try
            {
                //call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(cn, null, commandType, commandText, commandParameters,
                    OracleConnectionOwnership.Internal);
            }
            catch
            {
                //if we fail to return the OracleDataReader, we need to close the connection ourselves
                cn.Close();
                throw;
            }
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OracleParameters
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteReader(connection, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset) against the specified OracleConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(connection, (OracleTransaction)null, commandType, commandText, commandParameters,
                OracleConnectionOwnership.External);
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a resultset) against the specified OracleConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(OracleConnection connection, string spName,
            params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType,
            string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteReader(transaction, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset) against the specified OracleTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType,
            string commandText, params OracleParameter[] commandParameters)
        {
            //pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters,
                OracleConnectionOwnership.External);
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a resultset) against the specified
        ///     OracleTransaction using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     OracleDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an OracleDataReader containing the resultset generated by the command</returns>
        internal OracleDataReader ExecuteReader(OracleTransaction transaction, string spName,
            params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteReader

        #region ExecuteDataSet

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connectionString, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //create & open an OracleConnection, and dispose of it after we are done.
            using (OracleConnection cn = new OracleConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a resultset) against the database specified in
        ///     the conneciton string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OracleParameters
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connection, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset) against the specified OracleConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            //return the dataset
            return ds;
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a resultset) against the specified OracleConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(OracleConnection connection, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OracleParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(transaction, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a resultset) against the specified OracleTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            //return the dataset
            return ds;
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a resultset) against the specified
        ///     OracleTransaction using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        internal DataSet ExecuteDataset(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OracleParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteDataSet

        #region ExecuteScalar

        /// <summary>
        ///     Execute an OracleCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-Oracle command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(string cnString, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(cnString, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a 1x1 resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new
        ///     OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-Oracle command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(string cnString, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //create & open an OracleConnection, and dispose of it after we are done.
            using (OracleConnection cn = new OracleConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a 1x1 resultset) against the database specified in
        ///     the conneciton string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(string cnString, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OracleParameters
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a 1x1 resultset and takes no parameters) against the provided
        ///     OracleConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-Oracle command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(connection, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a 1x1 resultset) against the specified OracleConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new
        ///     OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters);

            //execute the command & return the results
            return cmd.ExecuteScalar();
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a 1x1 resultset) against the specified
        ///     OracleConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connection">a valid OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(OracleConnection connection, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OracleParameters
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a 1x1 resultset and takes no parameters) against the provided
        ///     OracleTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(transaction, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns a 1x1 resultset) against the specified OracleTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new
        ///     OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //execute the command & return the results
            return cmd.ExecuteScalar();
        }

        /// <summary>
        ///     Execute a stored procedure via an OracleCommand (that returns a 1x1 resultset) against the specified
        ///     OracleTransaction using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        internal object ExecuteScalar(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OracleParameters
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the SP without params
            else
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteScalar

        #region ExecuteNonQuery

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in
        ///     the connection string
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="conString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        internal int ExecuteNonQuery(string conString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(conString, commandType, commandText, null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset) against the database specified in the connection string
        ///     using the provided parameters
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new
        ///     OracleParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="conString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(string conString, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new OracleConnection(conString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a OracleCommand (that returns no resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="conString">A valid connection string for a OracleConnection</param>
        /// <param name="spName">The name of the stored prcedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(string conString, string spName, params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(conString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connection, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset) against the specified SqlConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(transaction, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified
        ///     SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters
        ///     for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteNonQuery

        #endregion
    }
}