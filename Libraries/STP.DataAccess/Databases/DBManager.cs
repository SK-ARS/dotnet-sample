#region

using System;
using System.Collections.Generic;
using System.Data;
using STP.Common.Enums;
using STP.DataAccess.Delegates;
using STP.DataAccess.Interface;
using STP.DataAccess.ParameterSet;

#endregion

namespace STP.DataAccess.Databases
{
    public sealed class DBManager : IDBManager
    {
        private string dataSource;
        private IDbCommand idbCommand;
        private IDbConnection idbConnection;
        private IDbDataParameter[] idbParameters;
        private IDbTransaction idbTransaction;
        private string initialCatalog;
        private string password;
        private string persistSecurityInfo;
        private DataProvider providerType;
        private string userId;


        public DBManager()
        {
        }

        public DBManager(DataProvider providerType)
        {
            this.providerType = providerType;
        }

        public DBManager(DataProvider providerType, string
            connectionString)
        {
            this.providerType = providerType;
            ConnectionString = connectionString;
        }

        public DatabaseSource DatabaseSource { get; set; }

        #region IDBManager Members

        public IDbConnection Connection
        {
            get { return idbConnection; }
        }

        public IDataReader DataReader { get; set; }

        public DataProvider ProviderType
        {
            get { return providerType; }
            set { providerType = value; }
        }

        public string ConnectionString { get; set; }

        public string DataSource
        {
            get { return dataSource; }
        }

        public string InitialCatalog
        {
            get { return initialCatalog; }
        }

        public string PersistSecurityInfo
        {
            get { return persistSecurityInfo; }
        }

        public string UserId
        {
            get { return userId; }
        }

        public string Password
        {
            get { return password; }
        }

        public IDbCommand Command
        {
            get { return idbCommand; }
        }

        public IDbTransaction Transaction
        {
            get { return idbTransaction; }
        }

        public IDbDataParameter[] Parameters
        {
            get { return idbParameters; }
        }

        public void Open()
        {
            idbConnection = DBManagerFactory.GetConnection(providerType);
            idbConnection.ConnectionString = ConnectionString;
            if (idbConnection.State != ConnectionState.Open)
                idbConnection.Open();
            GetConnectionData();
            //idbCommand = DBManagerFactory.GetCommand(ProviderType);
        }

        public void Close()
        {
            if (idbConnection.State != ConnectionState.Closed)
                idbConnection.Close();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Close();
            idbCommand = null;
            idbTransaction = null;
            idbConnection = null;
        }

        public void BeginTransaction()
        {
            if (idbTransaction == null)
                idbTransaction = DBManagerFactory.GetTransaction(ProviderType);
            idbCommand.Transaction = idbTransaction;
        }

        public void CommitTransaction()
        {
            if (idbTransaction != null)
                idbTransaction.Commit();
            idbTransaction = null;
        }

        public void CloseReader()
        {
            if (DataReader != null)
                DataReader.Close();
        }

        public void CreateParameters(int paramsCount)
        {
            idbParameters = new IDbDataParameter[paramsCount];
            idbParameters = DBManagerFactory.GetParameters(ProviderType, paramsCount);
        }

        public void AddParameters(int index, string paramName, object objValue)
        {
            if (index < idbParameters.Length)
            {
                idbParameters[index].ParameterName = paramName;
                idbParameters[index].Value = objValue;
            }
        }

        //public object ExecuteScalar(CommandType commandType, string commandText)
        //{
        //    idbCommand = DBManagerFactory.GetCommand(ProviderType);
        //    PrepareCommand(idbCommand, Connection, Transaction, commandType, commandText, Parameters);
        //    object returnValue = idbCommand.ExecuteScalar();
        //    
        //    return returnValue;
        //}

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            idbCommand = DBManagerFactory.GetCommand(ProviderType);
            PrepareCommand(idbCommand, Connection, Transaction, commandType, commandText, Parameters);
            IDbDataAdapter dataAdapter = DBManagerFactory.GetDataAdapter(ProviderType);
            dataAdapter.SelectCommand = idbCommand;
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            return dataSet;
        }

        #endregion

        #region Private Methods

        private void AttachParameters(IDbCommand command, IEnumerable<IDbDataParameter> commandParameters)
        {
            foreach (IDbDataParameter idbParameter in commandParameters)
            {
                if ((idbParameter.Direction == ParameterDirection.InputOutput) && (idbParameter.Value == null))
                {
                    idbParameter.Value = DBNull.Value;
                }
                command.Parameters.Add(idbParameter);
            }
        }

        private void PrepareCommand(IDbCommand command, IDbConnection connection,
            IDbTransaction transaction, CommandType commandType, string commandText,
            IEnumerable<IDbDataParameter> commandParameters)
        {
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        private void GetConnectionData()
        {
            string[] arr = ConnectionString.Split(";".ToCharArray());
            foreach (string s in arr)
            {
                string[] k = s.Split("=".ToCharArray());
                if (k.Length > 1)
                {
                    switch (k[0].ToLower())
                    {
                        case "data source":
                            dataSource = k[1];
                            break;
                        case "initial catalog":
                            initialCatalog = k[1];
                            break;
                        case "persist security info":
                            persistSecurityInfo = k[1];
                            break;
                        case "user id":
                            userId = k[1];
                            break;
                        case "password":
                            password = k[1];
                            break;
                    }
                }
            }
        }

        #endregion

        #region Get ExecuteReaders

        public IDataReader ExecuteReader(string procedureName)
        {
            idbCommand = DBManagerFactory.CreateParameterizedCommand(providerType, idbConnection, DatabaseSource,
                procedureName);
            DataReader = idbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return DataReader;
        }

        public IDataReader ExecuteReader(string procedureName, SqlParameterMapper sqlParameterMapper)
        {
            idbCommand = DBManagerFactory.CreateParameterMappedCommand(providerType, idbConnection, DatabaseSource,
                procedureName, sqlParameterMapper);
            DataReader = idbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return DataReader;
        }

        public IDataReader ExecuteReader<T>(string procedureName, SqlParameterMapper sqlParameterMapper, T objectInstance)
        {
            idbCommand = DBManagerFactory.CreateParameterMappedCommand(providerType, idbConnection, DatabaseSource,
                procedureName, sqlParameterMapper);
            DataReader = idbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return DataReader;
        }

        public IDataReader ExecuteReader<T>(string procedureName, SqlParameterMapper<T> sqlParameterMapper, T objectInstance)
        {
            idbCommand = DBManagerFactory.CreateParameterMappedCommand(providerType, idbConnection, DatabaseSource,
                procedureName, sqlParameterMapper, objectInstance);
            DataReader = idbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return DataReader;
        }

        public IDataReader ExecuteReader(string procedureName, object[] parameterValues)
        {
            idbCommand = DBManagerFactory.CreateCommand(providerType, idbConnection, DatabaseSource, procedureName,
                parameterValues);
            DataReader = idbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return DataReader;
        }

        public IDataReader ExecuteReader(string procedureName, StoredProcedureParameterList parameterList)
        {
            idbCommand = DBManagerFactory.CreateCommand(providerType, idbConnection, DatabaseSource, procedureName,
                parameterList);
            DataReader = idbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return DataReader;
        }

        #endregion

        #region Execute NonQuery

        public int ExecuteNonQuery(string procedureName, SqlParameterMapper sqlParameterMapper)
        {
            idbCommand = DBManagerFactory.CreateParameterMappedCommand(providerType, idbConnection, DatabaseSource,
                procedureName, sqlParameterMapper);
            int returnValue = idbCommand.ExecuteNonQuery();

            return returnValue;
        }

        public int ExecuteNonQuery<T>(string procedureName, SqlParameterMapper<T> sqlParameterMapper, T objectInstance)
        {
            idbCommand = DBManagerFactory.CreateParameterMappedCommand(providerType, idbConnection, DatabaseSource,
                procedureName, sqlParameterMapper, objectInstance);
            int returnValue = idbCommand.ExecuteNonQuery();

            return returnValue;
        }

        public int ExecuteNonQuery(string procedureName, params object[] parameterValues)
        {
            idbCommand = DBManagerFactory.CreateCommand(providerType, idbConnection, DatabaseSource, procedureName,
                parameterValues);
            int returnValue = idbCommand.ExecuteNonQuery();

            return returnValue;
        }

        //public int ExecuteNonQuery(CommandType commandType, string commandText)
        //{
        //    idbCommand = DBManagerFactory.GetCommand(ProviderType);
        //    PrepareCommand(idbCommand, Connection, Transaction, commandType, commandText, Parameters);
        //    int returnValue = idbCommand.ExecuteNonQuery();
        //    
        //    return returnValue;
        //}

        #endregion

        #region Execute Scalar

        public object ExecuteScalar(string procedureName, SqlParameterMapper sqlParameterMapper)
        {
            idbCommand = DBManagerFactory.CreateCommand(providerType, idbConnection, DatabaseSource, procedureName,
                sqlParameterMapper);
            object returnValue = idbCommand.ExecuteScalar();
            return returnValue;
        }

        public object ExecuteScalar(string procedureName, params object[] parameterValues)
        {
            idbCommand = DBManagerFactory.CreateCommand(providerType, idbConnection, DatabaseSource, procedureName,
                parameterValues);
            object returnValue = idbCommand.ExecuteScalar();
            return returnValue;
        }

        #endregion
    }
}