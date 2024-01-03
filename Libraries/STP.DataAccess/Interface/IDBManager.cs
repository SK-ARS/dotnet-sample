#region

using System;
using System.Data;
using STP.Common.Enums;
using STP.DataAccess.Delegates;
using STP.DataAccess.ParameterSet;

#endregion

namespace STP.DataAccess.Interface
{
    public interface IDBManager : IDisposable
    {
        DataProvider ProviderType { get; set; }

        string ConnectionString { get; set; }
        string DataSource { get; }
        string InitialCatalog { get; }
        string PersistSecurityInfo { get; }
        string UserId { get; }
        string Password { get; }


        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        IDataReader DataReader { get; }
        IDbCommand Command { get; }

        IDbDataParameter[] Parameters { get; }

        void Open();
        void BeginTransaction();
        void CommitTransaction();
        void CreateParameters(int paramsCount);
        void AddParameters(int index, string paramName, object objValue);
        //IDataReader ExecuteReader(CommandType commandType, string commandText);

        DataSet ExecuteDataSet(CommandType commandType, string
            commandText);

        //object ExecuteScalar(CommandType commandType, string commandText);
        //int ExecuteNonQuery(CommandType commandType, string commandText);
        void CloseReader();
        void Close();
        new void Dispose();

        #region New Methods Added

        #region ExecuteNonQuery

        int ExecuteNonQuery(string procedureName, SqlParameterMapper sqlParameterMapper);
        int ExecuteNonQuery(string procedureName, params object[] parameterValues);
        int ExecuteNonQuery<T>(string procedureName, SqlParameterMapper<T> sqlParameterMapper, T objectInstance);

        #endregion

        #region ExecuteReader

        IDataReader ExecuteReader(string procedureName);

        IDataReader ExecuteReader(string procedureName, SqlParameterMapper sqlParameterMapper);

        IDataReader ExecuteReader<T>(string procedureName, SqlParameterMapper sqlParameterMapper, T objectInstance);

        IDataReader ExecuteReader<T>(string procedureName, SqlParameterMapper<T> sqlParameterMapper, T objectInstance);

        IDataReader ExecuteReader(string procedureName, object[] parameterValues);

        IDataReader ExecuteReader(string procedureName, StoredProcedureParameterList parameterList);

        #endregion

        #region ExecuteScalar

        object ExecuteScalar(string procedureName, SqlParameterMapper sqlParameterMapper);
        object ExecuteScalar(string procedureName, params object[] parameterValues);

        #endregion

        #endregion
    }
}