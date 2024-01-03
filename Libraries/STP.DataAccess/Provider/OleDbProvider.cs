using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text.RegularExpressions;
using STP.Common;
using STP.DataAccess.BaseHelper;
using STP.DataAccess.Interface;

namespace STP.DataAccess.Provider
{
    /// <summary>
    ///     The OleDbHelper class is intended to encapsulate high performance, scalable best practices for
    ///     common uses of OleDbClient.
    /// </summary>
    public sealed class OleDbProvider : IOleDbProvider
    {
         #region Singleton

        private static volatile OleDbProvider instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private OleDbProvider()
        {
        }

        internal static OleDbProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                // Support multithreaded applications through 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each time the method is invoked
                if (instance == null)
                {
                    lock (SyncLock)
                    {
                        if (instance == null)
                        {
                            instance = new OleDbProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "OleDbProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Private Variables
        private const string ForbiddenVarcharsReplacement = "?";

        /// <summary>
        ///     Basically, all non-ascii characters. We're banning anything above 255 in non-unicode fields
        /// </summary>
        private static readonly Regex forbiddenVarchars = new Regex("[^\u0009-\u00FF]", RegexOptions.Compiled);

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

        #region Implementation of IOleDbProvider
        #region ExecuteNonQuery

        /// <summary>
        ///     Execute an OleDbCommand (that returns no resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteNonQuery(connectionString, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns no resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new
        ///     OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParameters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create & open an OleDbConnection, and dispose of it after we are done.
            using (OleDbConnection cn = new OleDbConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns no resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters = OleDbHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OleDbDbCommand (that returns no resultset and takes no parameters) against the provided OleDbConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteNonQuery(connection, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns no resultset) against the specified OleDbConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, connection, (OleDbTransaction) null, commandType, commandText, commandParameters);

            //finally, execute the command.
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns no resultset) against the specified OleDbConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(OleDbConnection connection, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns no resultset and takes no parameters) against the provided OleDbTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteNonQuery(transaction, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns no resultset) against the specified OleDbTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //finally, execute the command.
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns no resultset) against the specified
        ///     OleDbTransaction using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(OleDbTransaction transaction, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteNonQuery

        #region ExecuteDataSet

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteDataset(connectionString, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create & open an OleDbConnection, and dispose of it after we are done.
            using (OleDbConnection cn = new OleDbConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns a resultset) against the database specified in
        ///     the conneciton string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters = OleDbHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(OleDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteDataset(connection, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset) against the specified OleDbConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, connection, (OleDbTransaction) null, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            //return the dataset
            return ds;
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns a resultset) against the specified OleDbConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(OleDbConnection connection, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteDataset(transaction, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset) against the specified OleDbTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            //return the dataset
            return ds;
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns a resultset) against the specified
        ///     OleDbTransaction using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(OleDbTransaction transaction, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteDataSet

        #region ExecuteReader

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteReader(connectionString, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new
        ///     OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParameters used to execute the command</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create & open an OleDbbConnection
            OleDbConnection cn = new OleDbConnection(connectionString);
            cn.Open();

            try
            {
                //call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(cn, null, commandType, commandText, commandParameters,
                    OleDbConnectionOwnership.Internal);
            }
            catch
            {
                //if we fail to return the OleDbDataReader, we neeed to close the connection ourselves
                cn.Close();
                throw;
            }
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns a resultset) against the database specified in
        ///     the connection string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters = OleDbHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(OleDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteReader(connection, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset) against the specified OleDbConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(connection, (OleDbTransaction) null, commandType, commandText, commandParameters,
                OleDbConnectionOwnership.External);
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns a resultset) against the specified OleDbConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(OleDbConnection connection, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

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
        ///     Execute an OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteReader(transaction, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute an OleDbCommand (that returns a resultset) against the specified OleDbTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid",
        ///     24));
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OleDbParameters used to execute the command</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters,
                OleDbConnectionOwnership.External);
        }

        /// <summary>
        ///     Execute a stored procedure via an OleDbCommand (that returns a resultset) against the specified
        ///     OleDbTransaction using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     OleDbDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an OleDbDataReader containing the resultset generated by the command</returns>
        public OleDbDataReader ExecuteReader(OleDbTransaction transaction, string spName,
            params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Create and prepare an OleDbCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// </summary>
        /// <remarks>
        ///     If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        ///     If the caller provided the connection, we want to leave it to them to manage.
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection, on which to execute this command</param>
        /// <param name="transaction">a valid OleDbTransaction, or 'null'</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">
        ///     an array of OleDbParameters to be associated with the command or 'null' if no
        ///     parameters are required
        /// </param>
        /// <param name="connectionOwnership">
        ///     indicates whether the connection parameter was provided by the caller, or created by
        ///     OleDbHelper
        /// </param>
        /// <returns>OleDbDataReader containing the results of the command</returns>
        private static OleDbDataReader ExecuteReader(OleDbConnection connection, OleDbTransaction transaction,
            CommandType commandType, string commandText, OleDbParameter[] commandParameters,
            OleDbConnectionOwnership connectionOwnership)
        {
            //create a command and prepare it for execution
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            //create a reader
            OleDbDataReader dr;

            // call ExecuteReader with the appropriate CommandBehavior
            if (connectionOwnership == OleDbConnectionOwnership.External)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return dr;
        }

        /// <summary>
        ///     this enum is used to indicate weather the connection was provided by the caller, or created by OleDbHelper, so that
        ///     we can set the appropriate CommandBehavior when calling ExecuteReader()
        /// </summary>
        private enum OleDbConnectionOwnership
        {
            /// <summary>Connection is owned and managed by OleDbHelper</summary>
            Internal,

            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }

        #endregion ExecuteReader

        #region ExecuteScalar

        /// <summary>
        ///     Execute a OleDbCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in
        ///     the connection string.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteScalar(connectionString, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute a OleDbCommand (that returns a 1x1 resultset) against the database specified in the connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new
        ///     OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create & open a OleDbConnection, and dispose of it after we are done.
            using (OleDbConnection cn = new OleDbConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        ///     Execute a stored procedure via a OleDbCommand (that returns a 1x1 resultset) against the database specified in
        ///     the conneciton string using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters = OleDbHelperParameterCache.GetSpParameterSet(connectionString,
                    spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a OleDbCommand (that returns a 1x1 resultset and takes no parameters) against the provided OleDbConnection.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteScalar(connection, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute a OleDbCommand (that returns a 1x1 resultset) against the specified OleDbConnection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new
        ///     OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, connection, (OleDbTransaction) null, commandType, commandText, commandParameters);

            //execute the command & return the results
            return cmd.ExecuteScalar();
        }

        /// <summary>
        ///     Execute a stored procedure via a OleDbCommand (that returns a 1x1 resultset) against the specified OleDbConnection
        ///     using the provided parameter values.  This method will query the database to discover the parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connection">a valid OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(OleDbConnection connection, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        ///     Execute a OleDbCommand (that returns a 1x1 resultset and takes no parameters) against the provided
        ///     OleDbTransaction.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of OleDbParameters
            return ExecuteScalar(transaction, commandType, commandText, (OleDbParameter[]) null);
        }

        /// <summary>
        ///     Execute a OleDbCommand (that returns a 1x1 resultset) against the specified OleDbTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new
        ///     OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //execute the command & return the results
            return cmd.ExecuteScalar();
        }

        /// <summary>
        ///     Execute a stored procedure via a OleDbCommand (that returns a 1x1 resultset) against the specified
        ///     OleDbTransaction using the provided parameter values.  This method will query the database to discover the
        ///     parameters for the
        ///     stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        ///     This method provides no access to output parameters or the stored procedure's return value parameter.
        ///     e.g.:
        ///     int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">a valid OleDbTransaction</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(OleDbTransaction transaction, string spName, params object[] parameterValues)
        {
            //if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters =
                    OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of OleDbParameters
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                //otherwise we can just call the SP without params
            else
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteScalar

        #endregion

        #region private utility methods & constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new OleDbHelper()".
        //private OleDbHelper() { }

        /// <summary>
        ///     This method is used to attach array's of OleDbParameters to an OleDbCommand.
        ///     This method will assign a value of DbNull to any parameter with a direction of
        ///     InputOutput and a value of null.
        ///     This behavior will prevent default values from being used, but
        ///     this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ///     where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">an array of OleDbParameters tho be added to command</param>
        private static void AttachParameters(OleDbCommand command, OleDbParameter[] commandParameters)
        {
            foreach (OleDbParameter p in commandParameters)
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
        ///     This method assigns an array of values to an array of OleDbParameters.
        /// </summary>
        /// <param name="commandParameters">array of OleDbParameters to be assigned values</param>
        /// <param name="parameterValues">array of objects holding the values to be assigned</param>
        private static void AssignParameterValues(OleDbParameter[] commandParameters, object[] parameterValues)
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

            //iterate through the OleDbParameters, assigning the values from the corresponding position in the 
            //value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }

        /// <summary>
        ///     This method opens (if necessary) and assigns a connection, transaction, command type and parameters
        ///     to the provided command.
        /// </summary>
        /// <param name="command">the OleDbCommand to be prepared</param>
        /// <param name="connection">a valid OleDbConnection, on which to execute this command</param>
        /// <param name="transaction">a valid OleDbTransaction, or 'null'</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">
        ///     an array of OleDbParameters to be associated with the command or 'null' if no
        ///     parameters are required
        /// </param>
        private static void PrepareCommand(OleDbCommand command, OleDbConnection connection,
            OleDbTransaction transaction, CommandType commandType, string commandText,
            OleDbParameter[] commandParameters)
        {
            //if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //associate the connection with the command
            command.Connection = connection;

            //set the command text (stored procedure name or OleDb statement)
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

        #endregion private utility methods & constructors
    }
}