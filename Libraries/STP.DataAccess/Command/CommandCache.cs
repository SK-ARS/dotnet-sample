#region

using STP.DataAccess.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace RDW.DataAccess.Command
{
    internal class CommandCache : Dictionary<string, SqlCommand>, IDisposable
    {
       
        // Lock synchronization object
        private  readonly object objCmdCaches = new object();
        internal SqlCommand GetCommandCopy(SqlConnection connection, string databaseInstanceName, string procedureName)
        {
            string commandCacheKey = databaseInstanceName + procedureName;

            if (!ContainsKey(commandCacheKey))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                command.CommandTimeout = CommandFactory.CommandTimeOut;
                SqlCommandBuilder.DeriveParameters(command);

                //if (connection.State != ConnectionState.Open)
                //{
                //    connection.Open();

                //    connection.Close();
                //}
                lock (objCmdCaches)
                {
                    this[commandCacheKey] = command;
                }
            }

            SqlCommand copiedCommand = this[commandCacheKey].Clone();
            copiedCommand.Connection = connection;
            return copiedCommand;
        }

        #region Implementation of IDisposable

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}