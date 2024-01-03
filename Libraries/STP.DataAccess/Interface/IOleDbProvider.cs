using System.Data;
using System.Data.OleDb;

namespace STP.DataAccess.Interface
{
    public interface IOleDbProvider
    {
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
        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText);

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
        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues);

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
        int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText);

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
        int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        int ExecuteNonQuery(OleDbConnection connection, string spName, params object[] parameterValues);

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
        int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText);

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
        int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        int ExecuteNonQuery(OleDbTransaction transaction, string spName, params object[] parameterValues);

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
        DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText);

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
        DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues);

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
        DataSet ExecuteDataset(OleDbConnection connection, CommandType commandType, string commandText);

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
        DataSet ExecuteDataset(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        DataSet ExecuteDataset(OleDbConnection connection, string spName, params object[] parameterValues);

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
        DataSet ExecuteDataset(OleDbTransaction transaction, CommandType commandType, string commandText);

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
        DataSet ExecuteDataset(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        DataSet ExecuteDataset(OleDbTransaction transaction, string spName, params object[] parameterValues);

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
        OleDbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText);

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
        OleDbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        OleDbDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues);

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
        OleDbDataReader ExecuteReader(OleDbConnection connection, CommandType commandType, string commandText);

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
        OleDbDataReader ExecuteReader(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        OleDbDataReader ExecuteReader(OleDbConnection connection, string spName, params object[] parameterValues);

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
        OleDbDataReader ExecuteReader(OleDbTransaction transaction, CommandType commandType, string commandText);

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
        OleDbDataReader ExecuteReader(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        OleDbDataReader ExecuteReader(OleDbTransaction transaction, string spName, params object[] parameterValues);

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
        object ExecuteScalar(string connectionString, CommandType commandType, string commandText);

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
        object ExecuteScalar(string connectionString, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        object ExecuteScalar(string connectionString, string spName, params object[] parameterValues);

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
        object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText);

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
        object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        object ExecuteScalar(OleDbConnection connection, string spName, params object[] parameterValues);

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
        object ExecuteScalar(OleDbTransaction transaction, CommandType commandType, string commandText);

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
        object ExecuteScalar(OleDbTransaction transaction, CommandType commandType, string commandText,
            params OleDbParameter[] commandParameters);

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
        object ExecuteScalar(OleDbTransaction transaction, string spName, params object[] parameterValues);

        #endregion ExecuteScalar
    }
}