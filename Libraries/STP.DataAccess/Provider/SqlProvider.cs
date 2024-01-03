using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using STP.Common;
using STP.Common.Enums;
using STP.Common.Log;
using STP.DataAccess.BaseHelper;
using STP.DataAccess.Command;
using STP.DataAccess.Databases;
using STP.DataAccess.Debug;
using STP.DataAccess.Delegates;
using STP.DataAccess.Exceptions;
using STP.DataAccess.Interface;
using STP.DataAccess.Mappers;
using STP.DataAccess.ParameterSet;

namespace STP.DataAccess.Provider
{
    /// <summary>
    ///     The SqlHelper class is intended to encapsulate high performance, scalable best practices for
    ///     common uses of SqlClient
    /// </summary>
    public sealed class SqlProvider : ISqlProvider
    {
        #region Singleton

        private static volatile SqlProvider instance;
        // Lock synchronization object
        private static readonly object syncLock = new object();

        private string connectionString = string.Empty;
        private DatabaseSource databaseSource = DatabaseSource.SQL;


        private SqlProvider()
        {
            connectionString = ConnectionManager.GetConnectionString(DatabaseSource.SQL);
        }

        internal static SqlProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                // Support multithreaded applications through 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each time the method is invoked
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                        {
                            instance = new SqlProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "SqlProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Private Variables
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

        #endregion

        #region Implementation of ISqlProvider

        #region Check ConnectivityIssue

        public bool CheckDatabaseConnection(DatabaseSource databaseSource, DataProvider dataProvider)
        {
            bool result;
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
            return result;
        }

        public string GetServerIP(DatabaseSource databaseSource, DataProvider dataProvider)
        {
            string result = string.Empty;
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    result = dbManager.DataSource;
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
            return result;
        }

        public string GetConnectionString(DatabaseSource databaseSource)
        {
            return ConnectionManager.GetConnectionString(databaseSource);
        }

        #endregion

        #region Implementing DBManager via Factory Pattern for more efficiency and performance

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="sqlOutputMapper"> </param>
        /// <returns> </returns>
        public  int ExecuteNonQuery(DatabaseSource databaseSource, DataProvider dataProvider, string procedureName,
            SqlParameterMapper sqlParameterMapper, SqlOutputParameterMapper sqlOutputMapper)
        {
            int result = 0;

            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    result = dbManager.ExecuteNonQuery(procedureName, sqlParameterMapper);


                    if (sqlOutputMapper != null)
                    {
                        var sqloutputParams =
                                    new SqlParameterSet((SqlParameterCollection)dbManager.Command.Parameters);
                        sqlOutputMapper(sqloutputParams);
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
            return result;
        }

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <returns> </returns>
        public  int ExecuteNonQuery(DatabaseSource databaseSource, DataProvider dataProvider, string procedureName,
            SqlParameterMapper sqlParameterMapper)
        {
            return ExecuteNonQuery(databaseSource, dataProvider, procedureName, sqlParameterMapper, null);
        }

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="objectInstance"> </param>
        /// <returns> </returns>
        public  int ExecuteNonQuery<T>(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper<T> sqlParameterMapper,
            T objectInstance)
        {
            int result = 0;
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    result = dbManager.ExecuteNonQuery(procedureName, sqlParameterMapper, objectInstance);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
            return result;
        }

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="parameterValues"> </param>
        /// <returns> </returns>
        public  int ExecuteNonQuery(DatabaseSource databaseSource, DataProvider dataProvider, string procedureName,
            params object[] parameterValues)
        {
            int result = 0;
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    result = dbManager.ExecuteNonQuery(procedureName, parameterValues);
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
            return result;
        }

        #endregion

        #region ExecuteScalar's

        /// <summary>
        ///     Executes a command and returns the value of the first column of the first row of the resultset (or null).
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <returns> </returns>
        public  object ExecuteScalar(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper)
        {
            return ExecuteScalar(databaseSource, dataProvider, procedureName, sqlParameterMapper, null);
        }

        /// <summary>
        ///     <para>Executes a stored procedure on the specified databaseSource.</para>
        /// </summary>
        /// <param name="databaseSource">
        ///     <para>
        ///         The
        ///         <see cref="databaseSource" />
        ///         on which the sproc should be executed.
        ///     </para>
        /// </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName">
        ///     <para>The name of the sproc to execute.</para>
        /// </param>
        /// <param name="sqlParameterMapper">
        ///     <para>
        ///         A delegate that will populate the parameters in the sproc call.
        ///         Specify
        ///         <see langword="null" />
        ///         if the sproc does not require parameters.
        ///     </para>
        /// </param>
        /// <param name="sqlOutputMapper">
        ///     <para>
        ///         A delegate that will read the value of the parameters returned
        ///         by the sproc call.  Specify
        ///         <see langword="null" />
        ///         if no output parameters
        ///         have been provided by the
        ///         <paramref name="sqlParameterMapper" />
        ///         delegate.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>The value returned by as part of the result set.</para>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <para>
        ///         The argument
        ///         <paramref name="databaseSource" />
        ///         is
        ///         <see langword="null" />
        ///         .
        ///     </para>
        ///     <para>-or-</para>
        ///     <para>
        ///         The argument
        ///         <paramref name="procedureName" />
        ///         is
        ///         <see langword="null" />
        ///         .
        ///     </para>
        /// </exception>
        /// <exception cref="SafeProcedureException">
        ///     <para>An unexpected exception has been encountered during the sproc call.</para>
        /// </exception>
        public  object ExecuteScalar(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper,
            SqlOutputParameterMapper sqlOutputMapper)
        {
            object result = null;
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        result = dbManager.ExecuteScalar(procedureName, sqlParameterMapper);
                        if (sqlOutputMapper != null)
                        {
                            var sqloutputParams =
                                     new SqlParameterSet((SqlParameterCollection)dbManager.Command.Parameters);
                            sqlOutputMapper(sqloutputParams);

                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
                    }
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
            return result;
        }

        /// <summary>
        ///     Executes a command and returns the value of the first column of the first row of the resultset (or null).
        /// </summary>
        /// <param name="databaseSource"> The databaseSource. </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> The stored procedure name. </param>
        /// <param name="parameterValues"> The parameter values. </param>
        /// <returns> </returns>
        public  object ExecuteScalar(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            params object[] parameterValues)
        {
            object result = null;
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        result = dbManager.ExecuteScalar(procedureName, parameterValues);
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
                    }
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
            return result;
        }

        #endregion

        #region Execute's

        /// <summary>
        ///     Executes a single-result procedure and loads the results into a DataTable named after the procedure.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="parameters"> </param>
        /// <returns> A fully populated datatable </returns>
        /// <remarks>
        ///     Will throw an exception if one occurs, but will close databaseSource connection.
        /// </remarks>
        public  DataTable Execute(DatabaseSource databaseSource, DataProvider dataProvider, string procedureName,
            params object[] parameters)
        {
            var dt = new DataTable(procedureName);
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        using (var reader = dbManager.ExecuteReader(procedureName, parameters))
                        {
                            dt.Load(reader, LoadOption.OverwriteChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
                    }
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
            return dt;
        }

        //public DataTable Execute(DatabaseSource databaseSource, DataProvider dataProvider, string procedureName,
        //    SqlParameterMapper sqlParameterMapper)
        //{
        //    return Execute(databaseSource, dataProvider, procedureName, sqlParameterMapper);
        //}

        /// <summary>
        ///     Executes a single-result procedure and loads the results into a DataTable named after the procedure.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <returns> A fully populated datatable </returns>
        /// <remarks>
        ///     Will throw an exception if one occurs, but will close databaseSource connection.
        /// </remarks>
        public  DataTable Execute(DatabaseSource databaseSource, DataProvider dataProvider, string procedureName,
            SqlParameterMapper sqlParameterMapper)
        {
            var dt = new DataTable(procedureName);
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString = connectionString,
                    DatabaseSource = DatabaseSource.SQL,
                })
                {
                    try
                    {
                        dbManager.Open();
                        using (var reader = dbManager.ExecuteReader(procedureName, sqlParameterMapper))
                        {
                            dt.Load(reader, LoadOption.OverwriteChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
                    }
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
            return dt;
        }

        #endregion

        #region ExecuteAndGetDictionary

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and adds the instance
        ///     to a dictionary. First column returned must be the int key.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <returns> </returns>
        public  Dictionary<int, T> ExecuteAndGetDictionary<T>(DatabaseSource databaseSource,
            DataProvider dataProvider, string procedureName,
            SqlParameterMapper sqlParameterMapper,
            RecordMapper<T> recordMapper) where T : new()
        {
            var dictionaryList = new Dictionary<int, T>();
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        using (
                            IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                dictionaryList[reader.GetInt32(0)] = objectInstance;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
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
            return dictionaryList;
        }

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and adds the instance
        ///     to a dictionary. First column returned must be the int key.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        public  Dictionary<int, T> ExecuteAndGetDictionary<T>(DatabaseSource databaseSource,
            DataProvider dataProvider, string procedureName,
            RecordMapper<T> recordMapper,
            params object[] parameters) where T : new()
        {
            var dictionaryList = new Dictionary<int, T>();
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameters)))
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                dictionaryList[reader.GetInt32(0)] = objectInstance;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
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
            return dictionaryList;
        }

        #endregion

        #region ExecuteAndMapResult's

        /// <summary>
        ///     Executes a procedure and allows the caller to inject a resultset mapper and an output parameter mapper.
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="parameterList"> </param>
        /// <param name="result"> </param>
        /// <param name="sqlOutputMapper"> </param>
        /// <param name="databaseSource"> </param>
        public  void ExecuteAndMapResults(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            StoredProcedureParameterList parameterList, ResultMapper result,
            SqlOutputParameterMapper sqlOutputMapper)
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameterList)))
                    {
                        result(reader);
                    }
                    if (sqlOutputMapper != null)
                    {
                        var sqloutputParams =
                                    new SqlParameterSet((SqlParameterCollection)dbManager.Command.Parameters);
                        sqlOutputMapper(sqloutputParams);
                    }

                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        /// <summary>
        ///     Executes a procedure and allows the caller to inject a resultset mapper.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="result"> </param>
        public  void ExecuteAndMapResults(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper,
            ResultMapper result)
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
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

        /// <summary>
        ///     Executes a procedure and allows the caller to inject a resultset mapper.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="result"> </param>
        /// <param name="parameters"> </param>
        public  void ExecuteAndMapResults(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName, ResultMapper result,
            params object[] parameters)
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameters)))
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

        #region ExecuteAndMapRecord's

        /// <summary>
        ///     Executes a single-result procedure and fires a mapping delegate for each row that is returned.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        public  void ExecuteAndMapRecords(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper,
            RecordMapper recordMapper)
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
                    {
                        while (reader.Read())
                            recordMapper(reader);
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }


        /// <summary>
        ///     Executes a single-result procedure and fires a mapping delegate for each row that is returned and then fires a
        ///     parameter mapping delegate for output params.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="sqlOutputMapper"> </param>
        public  void ExecuteAndMapRecords(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper,
            RecordMapper recordMapper, SqlOutputParameterMapper sqlOutputMapper)
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
                    {
                        while (reader.Read())
                            recordMapper(reader);
                        if (sqlOutputMapper != null)
                        {
                            var sqloutputParams =
                                       new SqlParameterSet((SqlParameterCollection)dbManager.Command.Parameters);
                            sqlOutputMapper(sqloutputParams);
                        }
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        /// <summary>
        ///     Executes a single-result procedure and fires a mapping delegate for each row that is returned.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        public  void ExecuteAndMapRecords(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            RecordMapper recordMapper,
            params object[] parameters)
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameters)))
                    {
                        while (reader.Read())
                            recordMapper(reader);
                    }
                }
                catch (Exception ex)
                {
                    STPConfigurationManager.LogProvider.Log(ex);
                }
            }
        }

        #endregion

        #region ExecuteAndGetInstance for <T>'s

        /// <summary>
        ///     Creates a new T instance, executes a procedure and fires a mapping delegate and returns the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <returns> </returns>
        public  T ExecuteAndGetInstance<T>(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper, RecordMapper<T> recordMapper)
            where T : new()
        {
            var objectInstance = new T();
            if (
                !ExecuteAndHydrateInstance(objectInstance, databaseSource, dataProvider, procedureName, sqlParameterMapper,
                    recordMapper))
                return default(T);

            return objectInstance;
        }

        /// <summary>
        ///     Creates a new T instance, executes a procedure and fires a mapping delegate and returns the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        public  T ExecuteAndGetInstance<T>(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            RecordMapper<T> recordMapper,
            params object[] parameters) where T : new()
        {
            var objectInstance = new T();
            if (
                !ExecuteAndHydrateInstance(objectInstance, databaseSource, dataProvider, procedureName, recordMapper,
                    parameters))
                return default(T);

            return objectInstance;
        }

        #endregion

        #region ExecuteAndGetInstanceList for <T>'s

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to the return List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <returns> </returns>
        public  List<T> ExecuteAndGetInstanceList<T>(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper, RecordMapper<T> recordMapper)
            where T : new()
        {
            var instanceList = new List<T>();
            ExecuteAndHydrateInstanceList(instanceList, databaseSource, dataProvider, procedureName, sqlParameterMapper,
                recordMapper);
            return instanceList;
        }

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to the return List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        public  List<T> ExecuteAndGetInstanceList<T>(DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            RecordMapper<T> recordMapper, params object[] parameters)
            where T : new()
        {
            var instanceList = new List<T>();
            ExecuteAndHydrateInstanceList(instanceList, databaseSource, dataProvider, procedureName, recordMapper,
                parameters);
            return instanceList;
        }

        #endregion

        #region ExecuteAndHydrateGenericInstance for <T>'s

        /// <summary>
        ///     Executes a procedure that one or more multiple recordsets and for each row returned in each record set, call the
        ///     delegates in the delegate array to map the generic entity type
        /// </summary>
        /// <typeparam name="T"> any T type with a default construtor </typeparam>
        /// <param name="objectInstance"> The generic entity instance to be hydrated </param>
        /// <param name="databaseSource"> The database to retrieve the data from if neccesary. </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> The name of the stored procedure to execute. </param>
        /// <param name="sqlParameterMapper"> The paramters values used in the stored procedure. </param>
        /// <param name="recordMappers">
        ///     The array of mapping delegates with parameter IRecord used to populate the generic entity
        ///     instance from the DataReader.
        /// </param>
        /// <returns> An instance of EntityList containing the data retrieved from the database. </returns>
        public  void ExecuteAndHydrateGenericInstance<T>(T objectInstance, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, SqlParameterMapper sqlParameterMapper,
            RecordMapper<T>[] recordMappers)
            where T : new()
        {
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        using (
                            IRecordSet reader =
                                new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper, sqlParameterMapper))
                            )
                        {
                            //Get all the recordsets, call each delegate in the delegate array to map the generic entity instance.
                            //Note: that if the number of delegates in the array does not match up with the number of recordsets returned
                            //Note: the function exit's normally, it will only call the delegates that are in the array.
                            int mapperIndex = 0;
                            do
                            {
                                if (mapperIndex < recordMappers.Length)
                                {
                                    while (reader.Read())
                                    {
                                        recordMappers[mapperIndex](reader, objectInstance);
                                    }
                                    mapperIndex++;
                                }
                                else
                                {
                                    break;
                                }
                            } while (reader.NextResult());
                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
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

        /// <summary>
        ///     Executes a procedure that one or more multiple recordsets and for each row returned in each record set, call the
        ///     delegates in the delegate array to map the generic entity type
        /// </summary>
        /// <typeparam name="T"> any T type with a default construtor </typeparam>
        /// <param name="objectInstance"> The generic entity instance to be hydrated </param>
        /// <param name="databaseSource"> The database to retrieve the data from if neccesary. </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> The name of the stored procedure to execute. </param>
        /// <param name="recordMappers">
        ///     The array of mapping delegates with parameter IRecord used to populate the generic entity
        ///     instance from the DataReader.
        /// </param>
        /// <param name="parameters"> The paramters values used in the stored procedure. </param>
        /// <returns> An instance of EntityList containing the data retrieved from the database. </returns>
        public  void ExecuteAndHydrateGenericInstance<T>(T objectInstance, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName,
            RecordMapper<T>[] recordMappers,
            params object[] parameters)
            where T : new()
        {
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameters)))
                        {
                            //Get all the recordsets, call each delegate in the delegate array to map the generic entity instance.
                            //Note that if the number of delegates in the array does not match up with the number of recordsets returned
                            //the function exit normally, it will only call the delegates that are in the array.
                            int mapperIndex = 0;
                            do
                            {
                                if (mapperIndex < recordMappers.Length)
                                {
                                    while (reader.Read())
                                    {
                                        recordMappers[mapperIndex](reader, objectInstance);
                                    }
                                    mapperIndex++;
                                }
                                else
                                {
                                    break;
                                }
                            } while (reader.NextResult());
                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
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

        #endregion

        #region ExecuteAndHydrateInstance for <T>'s

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        public  bool ExecuteAndHydrateInstance<T>(T objectInstance, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName,
            RecordMapper<T> recordMapper, params object[] parameters)
        {
            bool result = false;
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameters)))
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
                }
            }
            return result;
        }

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        public  bool ExecuteAndHydrateInstance<T>(T objectInstance, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, SqlParameterMapper<T> sqlParameterMapper,
            RecordMapper<T> recordMapper)
        {
            bool result = false;
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (
                        IRecordSet reader =
                            new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper, objectInstance)))
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
                }
            }
            return result;
        }

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        public  bool ExecuteAndHydrateInstance<T>(T objectInstance, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, SqlParameterMapper sqlParameterMapper,
            RecordMapper<T> recordMapper)
        {
            bool result = false;
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
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
                }
            }
            return result;
        }

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        public  bool ExecuteAndHydrateInstance<T>(T objectInstance, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, SqlParameterMapper sqlParameterMapper,
            RecordMapper recordMapper)
        {
            return ExecuteAndHydrateInstance(objectInstance, databaseSource, dataProvider, procedureName,
                (parameterSet, instance) => sqlParameterMapper(parameterSet),
                (reader, instance) => recordMapper(reader));
        }

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        public  bool ExecuteAndHydrateInstance<T>(T objectInstance, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName,
            RecordMapper recordMapper, params object[] parameters)
        {
            return ExecuteAndHydrateInstance(objectInstance, databaseSource, dataProvider, procedureName,
                (reader, instance) => recordMapper(reader), parameters);
        }

        #endregion

        #region ExecuteAndHydrateInstanceList for <T>'s

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        public  void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, SqlParameterMapper sqlParameterMapper,
            RecordMapper<T> recordMapper) where T : new()
        {
            try
            {
                using (IDBManager dbManager = new DBManager(dataProvider)
                {
                    ConnectionString =
                        ConnectionManager.GetConnectionString(databaseSource),
                    DatabaseSource = databaseSource,
                })
                {
                    try
                    {
                        dbManager.Open();
                        using (
                            IRecordSet reader =
                                new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper, recordMapper)))
                        {
                            while (reader.Read())
                            {
                                var objectInstance = new T();
                                recordMapper(reader, objectInstance);
                                instanceList.Add(objectInstance);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        STPConfigurationManager.LogProvider.Log(ex);
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

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="TConcrete"> </typeparam>
        /// <typeparam name="TList"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        public  void ExecuteAndHydrateInstanceList<TConcrete, TList>(ICollection<TList> instanceList,
            DatabaseSource databaseSource, DataProvider dataProvider,
            string procedureName,
            SqlParameterMapper sqlParameterMapper,
            RecordMapper<TConcrete> recordMapper)
            where TConcrete : TList, new()
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
                    {
                        while (reader.Read())
                        {
                            var objectInstance = new TConcrete();
                            recordMapper(reader, objectInstance);
                            instanceList.Add(objectInstance);
                        }
                    }
                }
                catch (SafeProcedureDataException e) // Procedure class already wrapped all necessary data
                {
                    STPConfigurationManager.LogProvider.Log(e);
                }
                catch (Exception e)
                {
                    STPConfigurationManager.LogProvider.Log(new SafeProcedureException(databaseSource, procedureName, typeof(TList), e));
                }
            }
        }

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        public  void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, RecordMapper<T> recordMapper,
            params object[] parameters) where T : new()
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameters)))
                    {
                        while (reader.Read())
                        {
                            var objectInstance = new T();
                            recordMapper(reader, objectInstance);
                            instanceList.Add(objectInstance);
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
                }
            }
        }

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        public  void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, RecordMapper recordMapper,
            params object[] parameters) where T : new()
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameters)))
                    {
                        while (reader.Read())
                        {
                            var objectInstance = new T();
                            recordMapper(reader);
                            instanceList.Add(objectInstance);
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
                }
            }
        }
        
        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="sqlOutputMapper"> </param>
        public  void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, SqlParameterMapper sqlParameterMapper,
            RecordMapper recordMapper,
            SqlOutputParameterMapper sqlOutputMapper) where T : new()
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
                    {
                        while (reader.Read())
                        {
                            var objectInstance = new T();
                            recordMapper(reader);
                            instanceList.Add(objectInstance);
                        }
                        if (sqlOutputMapper != null)
                        {
                            var sqloutputParams =
                                       new SqlParameterSet((SqlParameterCollection)dbManager.Command.Parameters);
                            sqlOutputMapper(sqloutputParams);
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
                }
            }
        }

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="sqlOutputMapper"> </param>
        public  void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName, SqlParameterMapper sqlParameterMapper,
            RecordMapper<T> recordMapper,
            SqlOutputParameterMapper sqlOutputMapper) where T : new()
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, sqlParameterMapper)))
                    {
                        while (reader.Read())
                        {
                            var objectInstance = new T();
                            recordMapper(reader, objectInstance);
                            instanceList.Add(objectInstance);
                        }
                        if (sqlOutputMapper != null)
                        {
                            var sqloutputParams =
                                      new SqlParameterSet((SqlParameterCollection)dbManager.Command.Parameters);
                            sqlOutputMapper(sqloutputParams);

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
                }
            }
        }


        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="dataProvider"></param>
        /// <param name="procedureName"> </param>
        /// <param name="parameterList"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="sqlOutputMapper"> </param>
        public  void ExecuteAndHydrateInstanceList<T>(List<T> instanceList, DatabaseSource databaseSource,
            DataProvider dataProvider,
            string procedureName,
            StoredProcedureParameterList parameterList,
            RecordMapper<T> recordMapper,
            SqlOutputParameterMapper sqlOutputMapper) where T : new()
        {
            using (IDBManager dbManager = new DBManager(dataProvider)
            {
                ConnectionString =
                    ConnectionManager.GetConnectionString(databaseSource),
                DatabaseSource = databaseSource,
            })
            {
                try
                {
                    dbManager.Open();
                    using (IRecordSet reader = new DataRecord(dbManager.ExecuteReader(procedureName, parameterList)))
                    {
                        while (reader.Read())
                        {
                            var objectInstance = new T();
                            recordMapper(reader, objectInstance);
                            instanceList.Add(objectInstance);
                        }
                        if (sqlOutputMapper != null)
                        {
                            var sqloutputParams =
                                       new SqlParameterSet((SqlParameterCollection)dbManager.Command.Parameters);
                            sqlOutputMapper(sqloutputParams);

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
                }
            }
        }

        #endregion

        #region ExecuteDataset

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="databaseSource"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        public void ExecuteDataset<T>(List<T> instanceList, DatabaseSource databaseSource,
            string procedureName, SqlParameterMapper sqlParameterMapper,
            RecordMapper<T> recordMapper) where T : new()
        {
            //SqlHelper.ExecuteDataset()
        }

        #endregion

        #endregion

        #region private utility methods & constructors
        
        /// <summary>
        ///     This method is used to attach array of SqlParameters to a SqlCommand.
        ///     This method will assign a value of DbNull to any parameter with a direction of
        ///     InputOutput and a value of null.
        ///     This behavior will prevent default values from being used, but
        ///     this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ///     where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                             p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        ///     This method assigns dataRow column values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // Do nothing if we get no data
                return;
            }

            int i = 0;
            // Set the parameters values
            foreach (SqlParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format(
                            "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                            i, commandParameter.ParameterName));
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary>
        ///     This method assigns an array of values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter) parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary>
        ///     This method opens (if necessary) and assigns a connection, transaction, command type and parameters
        ///     to the provided command
        /// </summary>
        /// <param name="command">The SqlCommand to be prepared</param>
        /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">
        ///     An array of SqlParameters to be associated with the command or 'null' if no parameters
        ///     are required
        /// </param>
        /// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction,
            CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
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


        #region All SQL Commands
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
                STPConfigurationManager.LogProvider.Log(
                string.Format(
                    "The incorrect number of parameters were supplied to the procedure {0}.  The number supplied was: {1}.  The number expected is: {2}.",
                    procedureName, numPassedParameters, numProcedureParameters - returnValueOffset), (string)null);

                //STPConfigurationManager.LogProvider.LogAndEmail(string.Format(
                //    "Too many parameters parameters were supplied to the procedure {0}.  The number supplied was: {1}.  The number expected is: {2}.",
                //    procedureName, numPassedParameters, numProcedureParameters - returnValueOffset));
            }
#endif
        }

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
                STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, commandName));
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
                STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));
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
            if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
                STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, procedureName));

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
                STPConfigurationManager.LogProvider.Log(new NoDboException(connection.Database, commandName));
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

        #region Helper Methods
        #region ExecuteNonQuery

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in
        ///     the connection string
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns no resultset) against the database specified in the connection string
        ///     using the provided parameters
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored prcedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
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
        private int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
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
        private int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters,
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
        private int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

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
        private int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
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
        private int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
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
        private int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

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

        #region ExecuteDataset

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connectionString, commandType, commandText, null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");

            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                // Return the dataset
                return ds;
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                // Return the dataset
                return ds;
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        ///     SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters
        ///     for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
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
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteDataset

        #region ExecuteReader

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(connection, null, commandType, commandText, commandParameters,
                    SqlConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves
                if (connection != null) connection.Close();
                throw;
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            // Pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters,
                SqlConnectionOwnership.External);
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters,
                SqlConnectionOwnership.External);
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        ///     SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters
        ///     for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// </summary>
        /// <remarks>
        ///     If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        ///     If the caller provided the connection, we want to leave it to them to manage.
        /// </remarks>
        /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">
        ///     An array of SqlParameters to be associated with the command or 'null' if no parameters
        ///     are required
        /// </param>
        /// <param name="connectionOwnership">
        ///     Indicates whether the connection parameter was provided by the caller, or created by
        ///     SqlHelper
        /// </param>
        /// <returns>SqlDataReader containing the results of the command</returns>
        private SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction,
            CommandType commandType, string commandText, SqlParameter[] commandParameters,
            SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters,
                    out mustCloseConnection);

                // Create a reader
                SqlDataReader dataReader;

                // Call ExecuteReader with the appropriate CommandBehavior
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // Detach the SqlParameters from the command object, so they can be used again.
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader can´t set its values. 
                // When this happen, the parameters can´t be used again in other command.
                bool canClear = true;
                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }

        /// <summary>
        ///     This enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
        ///     we can set the appropriate CommandBehavior when calling ExecuteReader()
        /// </summary>
        private enum SqlConnectionOwnership
        {
            /// <summary>Connection is owned and managed by SqlHelper</summary>
            Internal,

            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }

        #endregion ExecuteReader

        #region ExecuteScalar

        /// <summary>
        ///     Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new
        ///     SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(string connectionString, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new
        ///     SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified
        ///     SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters
        ///     for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // PPull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteScalar

        #region ExecuteXmlReader

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters,
                    out mustCloseConnection);

                // Create the DataAdapter & DataSet
                XmlReader retval = cmd.ExecuteXmlReader();

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                return retval;
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure using "FOR XML AUTO"</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText,
            params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Create the DataAdapter & DataSet
            XmlReader retval = cmd.ExecuteXmlReader();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        ///     SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters
        ///     for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteXmlReader

        #region FillDataset

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        private void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet,
            string[] tableNames)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new
        ///     SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        private void FillDataset(string connectionString, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        private void FillDataset(string connectionString, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        private void FillDataset(SqlConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new
        ///     SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        private void FillDataset(SqlConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        private void FillDataset(SqlConnection connection, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        private void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText,
            DataSet dataSet, string[] tableNames)
        {
            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        ///     Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new
        ///     SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        private void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames,
                commandParameters);
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        ///     SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters
        ///     for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        private void FillDataset(SqlTransaction transaction, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }
        
        /// <summary>
        ///     Private helper method that execute a SqlCommand (that returns a resultset) against the specified SqlTransaction and
        ///     SqlConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new
        ///     SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">
        ///     This array will be used to create table mappings allowing the DataTables to be referenced
        ///     by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        private void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            // Create a command and prepare it for execution
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters,
                out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                // Add the table mappings specified by the user
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0)
                            throw new ArgumentException(
                                "The tableNames parameter must contain a list of tables, a value was provided as null or empty string.",
                                "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }

                // Fill the DataSet using default values for DataTable names, etc
                dataAdapter.Fill(dataSet);

                // Detach the SqlParameters from the command object, so they can be used again
                command.Parameters.Clear();
            }

            if (mustCloseConnection)
                connection.Close();
        }

        #endregion

        #region UpdateDataset

        /// <summary>
        ///     Executes the respective command for each inserted, updated, or deleted row in the DataSet.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param name="insertCommand">
        ///     A valid transact-SQL statement or stored procedure to insert new records into the data
        ///     source
        /// </param>
        /// <param name="deleteCommand">A valid transact-SQL statement or stored procedure to delete records from the data source</param>
        /// <param name="updateCommand">
        ///     A valid transact-SQL statement or stored procedure used to update records in the data
        ///     source
        /// </param>
        /// <param name="dataSet">The DataSet used to update the data source</param>
        /// <param name="tableName">The DataTable used to update the data source.</param>
        private static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand,
            DataSet dataSet, string tableName)
        {
            if (insertCommand == null) throw new ArgumentNullException("insertCommand");
            if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
            if (updateCommand == null) throw new ArgumentNullException("updateCommand");
            if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("tableName");

            // Create a SqlDataAdapter, and dispose of it after we are done
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                // Set the data adapter commands
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // Update the dataset changes in the data source
                dataAdapter.Update(dataSet, tableName);

                // Commit all the changes made to the DataSet
                dataSet.AcceptChanges();
            }
        }

        #endregion

        #region CreateCommand

        /// <summary>
        ///     Simplify the creation of a Sql command object by allowing
        ///     a stored procedure and optional parameters to be provided
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     SqlCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="sourceColumns">An array of string to be assigned as the source columns of the stored procedure parameters</param>
        /// <returns>A valid SqlCommand object</returns>
        private SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // Create a SqlCommand
            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // If we receive parameter values, we need to figure out where they go
            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided source columns to these parameters based on parameter order
                for (int index = 0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];

                // Attach the discovered parameters to the SqlCommand object
                AttachParameters(cmd, commandParameters);
            }

            return cmd;
        }

        #endregion

        #region ExecuteNonQueryTypedParams

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in
        ///     the connection string using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified
        ///     SqlTransaction using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="transaction">A valid SqlTransaction object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private int ExecuteNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // Sf the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion

        #region ExecuteDatasetTypedParams

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in
        ///     the connection string using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            //If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the dataRow column values as the store procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="transaction">A valid SqlTransaction object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        private DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion

        #region ExecuteReaderTypedParams

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in
        ///     the connection string using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }


        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="transaction">A valid SqlTransaction object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        private SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion

        #region ExecuteScalarTypedParams

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in
        ///     the connection string using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalarTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="transaction">A valid SqlTransaction object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        private object ExecuteScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion

        #region ExecuteXmlReaderTypedParams

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ///     using the dataRow column values as the stored procedure's parameters values.
        ///     This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="transaction">A valid SqlTransaction object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        private XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion
        #endregion

        #endregion private utility methods & constructors

        
    }
}