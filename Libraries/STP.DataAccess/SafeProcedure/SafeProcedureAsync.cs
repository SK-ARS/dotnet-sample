#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using STP.Common;
using STP.Common.Enums;
using STP.Common.Log;
using STP.DataAccess.Command;
using STP.DataAccess.Databases;
using STP.DataAccess.Delegates;
using STP.DataAccess.Exceptions;
using STP.DataAccess.Mappers;

#endregion

namespace STP.DataAccess.SafeProcedure
{
    public class SafeProcedureAsync
    {
        #region Async Begin/End Procedures

        // Function which return results
        /// //////////////////////////////////
        /// <summary>
        ///     Call async BeginExecuteReader
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="procName"> </param>
        /// <param name="parameterValues"> </param>
        /// <param name="callback"> </param>
        /// <returns> IAsyncResult </returns>
        public static IAsyncResult BeginExecuteReader(AsyncCallback callback, DatabaseSource databaseSource,
            string procName,
            params object[] parameterValues)
        {
            using (var connection = new SqlConnection(ConnectionManager.GetAsyncConnectionString(databaseSource)))
            {
                SqlCommand command = CommandFactory.CreateCommand(connection, databaseSource.ToString(), procName,
                    parameterValues);
                IAsyncResult result = null;

                try
                {
                    connection.Open();
                    result = command.BeginExecuteReader(callback, command, CommandBehavior.CloseConnection);
                }
                catch (SafeProcedureDataException e) // Procedure class already wrapped all necessary data
                {
                    CloseAsyncConnection(command);
                    STPConfigurationManager.LogProvider.Log(e);
                }
                finally
                {
                    connection.Close();
                }
                return result;
            }
        }

        /// <summary>
        ///     Async returns a ResultMapper
        /// </summary>
        /// <param name="ar"> </param>
        /// <param name="result"> </param>
        /// <returns> void </returns>
        public static void EndExecuteAndMapResults(IAsyncResult ar, ResultMapper result)
        {
            var command = (SqlCommand) ar.AsyncState;
            try
            {
                SqlDataReader reader = command.EndExecuteReader(ar);

                result(new DataRecord(reader));
            }
            finally
            {
                CloseAsyncConnection(ar);
            }
        }

        /// <summary>
        ///     Async returns a RecordMapper &gt;T&lt;
        /// </summary>
        /// <param name="ar"> </param>
        /// <param name="recordMapper"> </param>
        /// <returns> void </returns>
        public static T EndExecuteAndGetInstance<T>(IAsyncResult ar, RecordMapper<T> recordMapper) where T : new()
        {
            var objectInstance = new T();
            var command = (SqlCommand) ar.AsyncState;

            using (SqlDataReader reader = command.EndExecuteReader(ar))
            {
                recordMapper(new DataRecord(reader), objectInstance);
            }
            return objectInstance;
        }

        // Function which don't return results
        /// //////////////////////////////////
        /// <summary>
        ///     Call async BeginExecuteNonQuery
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="procName"> </param>
        /// <param name="parameterValues"> </param>
        /// <param name="callback"> </param>
        /// <returns> IAsyncResult </returns>
        public static IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, DatabaseSource databaseSource,
            string procName,
            params object[] parameterValues)
        {
            using (var connection = new SqlConnection(ConnectionManager.GetAsyncConnectionString(databaseSource)))
            {
                SqlCommand command = CommandFactory.CreateCommand(connection, databaseSource.ToString(), procName,
                    parameterValues);
                IAsyncResult result = null;

                try
                {
                    connection.Open();
                    result = command.BeginExecuteNonQuery(callback, command);
                }
                catch (SafeProcedureDataException e)
                {
                    CloseAsyncConnection(command);
                    STPConfigurationManager.LogProvider.Log(e);
                }

                return result;
            }
        }

        /// <summary>
        ///     Async returns a status of NonQuery execution
        /// </summary>
        /// <param name="ar"> </param>
        /// <returns> int </returns>
        public static int EndExecuteNonQuery(IAsyncResult ar)
        {
            var command = (SqlCommand) ar.AsyncState;
            int result;
            try
            {
                result = command.EndExecuteNonQuery(ar);
            }
            finally
            {
                CloseAsyncConnection(ar);
            }
            return result;
        }

        // Close ADO.NET async connection
        /// //////////////////////////////////
        /// <summary>
        ///     Call CloseAsyncConnection
        /// </summary>
        /// <param name="command"> </param>
        /// <returns> void </returns>
        public static void CloseAsyncConnection(SqlCommand command)
        {
            command.Connection.Close();
            command.Connection.Dispose();
        }

        /// <summary>
        ///     Call CloseAsyncConnection
        /// </summary>
        /// <param name="ar"> </param>
        /// <returns> void </returns>
        public static void CloseAsyncConnection(IAsyncResult ar)
        {
            CloseAsyncConnection((SqlCommand) ar.AsyncState);
        }

        #endregion

        #region Pseudo Async functions

        /// <summary>
        ///     Assembly-scoped class for returning a DataReader.
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <param name="resultMapper"> </param>
        /// <returns> </returns>
        public static void ExecuteAndMapResults(DatabaseSource databaseSource, string procedureName,
            SqlParameterMapper sqlParameterMapper,
            ResultMapper resultMapper)
        {
            using (var connection = new SqlConnection(ConnectionManager.GetAsyncConnectionString(databaseSource)))
            {
                SqlCommand command = CommandFactory.CreateParameterMappedCommand(connection, databaseSource.ToString(),
                    procedureName, sqlParameterMapper);
                bool isCompleted = false;

                try
                {
                    command.Connection.Open();
                    command.BeginExecuteReader(
                        delegate(IAsyncResult result)
                        {
                            try
                            {
                                SqlDataReader reader = command.EndExecuteReader(result);

                                resultMapper(new DataRecord(reader));
                                STPConfigurationManager.LogProvider.Log(
                                    string.Format("ExecuteAndMapResults async callback on thread: {0}",
                                        Thread.CurrentThread.ManagedThreadId));
                            }
                            finally
                            {
                                CloseAsyncConnection(command);
                                isCompleted = true;
                            }
                        }
                        , command
                        );
                }
                catch (Exception e)
                {
                    CloseAsyncConnection(command);
                    STPConfigurationManager.LogProvider.Log(e);
                }

                while (!isCompleted) Thread.Sleep(200);
            }
        }

        /// <summary>
        ///     Call pseudo async NonQuery execution
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="sqlParameterMapper"> </param>
        /// <returns> </returns>
        public static int ExecuteNonQuery(DatabaseSource databaseSource, string procedureName,
            SqlParameterMapper sqlParameterMapper)
        {
            using (var connection = new SqlConnection(ConnectionManager.GetAsyncConnectionString(databaseSource)))
            {
                SqlCommand command = CommandFactory.CreateParameterMappedCommand(connection, databaseSource.ToString(),
                    procedureName, sqlParameterMapper);
                bool isCompleted = false;
                int result = 0;

                try
                {
                    connection.Open();
                    command.BeginExecuteNonQuery(
                        //Begin delegate
                        delegate(IAsyncResult ar)
                        {
                            var locCommand = ar.AsyncState as SqlCommand;

                            try
                            {
                                if (locCommand != null) result = locCommand.EndExecuteNonQuery(ar);
                            }
                            finally
                            {
                                CloseAsyncConnection(locCommand);
                                isCompleted = true;
                            }
                        }
                        //End delegate
                        , command);
                }
                catch (Exception e) //If an exception is thrown during the connection
                {
                    CloseAsyncConnection(command);
                    STPConfigurationManager.LogProvider.Log(e);
                }
                while (!isCompleted) Thread.Sleep(200);

                return result;
            }
        }

        #endregion
    }
}