using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using STP.DataAccess.Command;

namespace STP.DataAccess.Provider
{
    internal class OracleCommandCache : Dictionary<string, OracleCommand>, IDisposable
    {
        internal OracleCommand GetCommandCopy(OracleConnection connection, string databaseInstanceName, string procedureName)
        {
            string commandCacheKey = databaseInstanceName + procedureName;

            if (!ContainsKey(commandCacheKey))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                command.CommandTimeout = CommandFactory.CommandTimeOut;
                OracleCommandBuilder.DeriveParameters(command);

                //if (connection.State != ConnectionState.Open)
                //{
                //    connection.Open();

                //    connection.Close();
                //}
                lock (this)
                {
                    this[commandCacheKey] = command;
                }
            }

            var copiedCommand = (OracleCommand)this[commandCacheKey].Clone();
            copiedCommand.Connection = connection;
            return copiedCommand;
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}