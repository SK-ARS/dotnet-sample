using System.Collections.Generic;
using System.Data;
using STP.DataAccess.Delegates;
using STP.DataAccess.ParameterSet;

namespace STP.DataAccess.Interface
{
    public interface IOracleProvider
    {
        #region Implementing Singleton Pattern for more efficiency and performance

        #region Check ConnectivityIssue

        bool CheckDatabaseConnection();

        string GetServerIP();

        //string GetConnectionString();

        #endregion

        #region ExecuteNonQuery's

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="oracleOutputMapper"> </param>
        /// <returns> </returns>
        int ExecuteNonQuery(string procedureName, OracleParameterMapper oracleParameterMapper,
            OracleOutputParameterMapper oracleOutputMapper);

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <returns> </returns>
        int ExecuteNonQuery(string procedureName,
            OracleParameterMapper oracleParameterMapper);

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="objectInstance"> </param>
        /// <returns> </returns>
        int ExecuteNonQuery<T>(
            string procedureName,
            OracleParameterMapper<T> oracleParameterMapper,
            T objectInstance);

        /// <summary>
        ///     Executes a query and returns the number of affected rows.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="parameterValues"> </param>
        /// <returns> </returns>
        int ExecuteNonQuery(string procedureName,
            params object[] parameterValues);

        int ExecuteBulkNonQuery(string insertQuery, OracleParameterMapper oracleParameterMapper, int count);

        bool ExecuteBulkProcedure(string procedureName, OracleParameterMapper oracleParameterMapper, int count);//, RecordMapper<T> recordMapper);
        #endregion

        #region ExecuteScalar's

        /// <summary>
        ///     Executes a command and returns the value of the first column of the first row of the resultset (or null).
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <returns> </returns>
        object ExecuteScalar(
            string procedureName,
            OracleParameterMapper oracleParameterMapper);

        /// <summary>
        ///     <para>Executes a stored procedure on the specified databaseSource.</para>
        /// </summary>
        ///     <para>
        ///         The
        ///         on which the sproc should be executed.
        ///     </para>
        /// <param name="procedureName">
        ///     <para>The name of the sproc to execute.</para>
        /// </param>
        /// <param name="oracleParameterMapper">
        ///     <para>
        ///         A delegate that will populate the parameters in the sproc call.
        ///         Specify
        ///         <see langword="null" />
        ///         if the sproc does not require parameters.
        ///     </para>
        /// </param>
        /// <param name="oracleOutputMapper">
        ///     <para>
        ///         A delegate that will read the value of the parameters returned
        ///         by the sproc call.  Specify
        ///         <see langword="null" />
        ///         if no output parameters
        ///         have been provided by the
        ///         <paramref name="oracleParameterMapper" />
        ///         delegate.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>The value returned by as part of the result set.</para>
        /// </returns>
        ///     <para>
        ///         The argument
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
        ///     <para>An unexpected exception has been encountered during the sproc call.</para>
        object ExecuteScalar(
            string procedureName,
            OracleParameterMapper oracleParameterMapper,
            OracleOutputParameterMapper oracleOutputMapper);

        /// <summary>
        ///     Executes a command and returns the value of the first column of the first row of the resultset (or null).
        /// </summary>
        /// <param name="procedureName"> The stored procedure name. </param>
        /// <param name="parameterValues"> The parameter values. </param>
        /// <returns> </returns>
        object ExecuteScalar(
            string procedureName,
            params object[] parameterValues);

        #endregion

        #region Execute's

        /// <summary>
        ///     Executes a single-result procedure and loads the results into a DataTable named after the procedure.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="parameters"> </param>
        /// <returns> A fully populated datatable </returns>
        /// <remarks>
        ///     Will throw an exception if one occurs, but will close databaseSource connection.
        /// </remarks>
        DataTable Execute(string procedureName,
            params object[] parameters);

        /// <summary>
        ///     Executes a single-result procedure and loads the results into a DataTable named after the procedure.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <returns> A fully populated datatable </returns>
        /// <remarks>
        ///     Will throw an exception if one occurs, but will close databaseSource connection.
        /// </remarks>
        DataTable Execute(string procedureName,
            OracleParameterMapper oracleParameterMapper);

        #endregion

        #region ExecuteAndGetDictionary

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and adds the instance
        ///     to a dictionary. First column returned must be the int key.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <returns> </returns>
        Dictionary<int, T> ExecuteAndGetDictionary<T>(
             string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper) where T : new();

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and adds the instance
        ///     to a dictionary. First column returned must be the int key.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        Dictionary<int, T> ExecuteAndGetDictionary<T>(
            string procedureName,
            RecordMapper<T> recordMapper,
            params object[] parameters) where T : new();

        #endregion

        #region ExecuteAndMapResult's

        /// <summary>
        ///     Executes a procedure and allows the caller to inject a resultset mapper and an output parameter mapper.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="parameterList"> </param>
        /// <param name="result"> </param>
        /// <param name="oracleOutputMapper"> </param>
        void ExecuteAndMapResults(
            string procedureName, StoredProcedureParameterList parameterList, ResultMapper result,
            OracleOutputParameterMapper oracleOutputMapper);

        /// <summary>
        ///     Executes a procedure and allows the caller to inject a resultset mapper.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="result"> </param>
        void ExecuteAndMapResults(
            string procedureName,
            OracleParameterMapper oracleParameterMapper,
            ResultMapper result);

        /// <summary>
        ///     Executes a procedure and allows the caller to inject a resultset mapper.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="result"> </param>
        /// <param name="parameters"> </param>
        void ExecuteAndMapResults(
            string procedureName, ResultMapper result,
            params object[] parameters);

        #endregion

        #region ExecuteAndMapRecord's

        /// <summary>
        ///     Executes a single-result procedure and fires a mapping delegate for each row that is returned.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        void ExecuteAndMapRecords(
            string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper recordMapper);


        /// <summary>
        ///     Executes a single-result procedure and fires a mapping delegate for each row that is returned and then fires a
        ///     parameter mapping delegate for output params.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="oracleOutputMapper"> </param>
        void ExecuteAndMapRecords(
            string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper recordMapper, OracleOutputParameterMapper oracleOutputMapper);

        /// <summary>
        ///     Executes a single-result procedure and fires a mapping delegate for each row that is returned.
        /// </summary>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        void ExecuteAndMapRecords(
            string procedureName,
            RecordMapper recordMapper,
            params object[] parameters);

        #endregion

        #region ExecuteAndGetInstance for <T>'s

        /// <summary>
        ///     Creates a new T instance, executes a procedure and fires a mapping delegate and returns the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <returns> </returns>
        T ExecuteAndGetInstance<T>(
            string procedureName,
            OracleParameterMapper oracleParameterMapper, RecordMapper<T> recordMapper)
            where T : new();

        /// <summary>
        ///     Creates a new T instance, executes a procedure and fires a mapping delegate and returns the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        T ExecuteAndGetInstance<T>(
            string procedureName,
            RecordMapper<T> recordMapper,
            params object[] parameters) where T : new();

        #endregion

        #region ExecuteAndGetInstanceList for <T>'s

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to the return List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <returns> </returns>
        List<T> ExecuteAndGetInstanceList<T>(
            string procedureName,
            OracleParameterMapper oracleParameterMapper, RecordMapper<T> recordMapper)
            where T : new();

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to the return List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        List<T> ExecuteAndGetInstanceList<T>(
            string procedureName,
            RecordMapper<T> recordMapper, params object[] parameters)
            where T : new();

        #endregion

        #region ExecuteAndHydrateGenericInstance for <T>'s

        /// <summary>
        ///     Executes a procedure that one or more multiple recordsets and for each row returned in each record set, call the
        ///     delegates in the delegate array to map the generic entity type
        /// </summary>
        /// <typeparam name="T"> any T type with a default construtor </typeparam>
        /// <param name="objectInstance"> The generic entity instance to be hydrated </param>
        /// <param name="procedureName"> The name of the stored procedure to execute. </param>
        /// <param name="oracleParameterMapper"> The paramters values used in the stored procedure. </param>
        /// <param name="recordMappers">
        ///     The array of mapping delegates with parameter IRecord used to populate the generic entity
        ///     instance from the DataReader.
        /// </param>
        /// <returns> An instance of EntityList containing the data retrieved from the database. </returns>
        void ExecuteAndHydrateGenericInstance<T>(T objectInstance,
            
            string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper<T>[] recordMappers)
            where T : new();

        /// <summary>
        ///     Executes a procedure that one or more multiple recordsets and for each row returned in each record set, call the
        ///     delegates in the delegate array to map the generic entity type
        /// </summary>
        /// <typeparam name="T"> any T type with a default construtor </typeparam>
        /// <param name="objectInstance"> The generic entity instance to be hydrated </param>
        /// <param name="procedureName"> The name of the stored procedure to execute. </param>
        /// <param name="recordMappers">
        ///     The array of mapping delegates with parameter IRecord used to populate the generic entity
        ///     instance from the DataReader.
        /// </param>
        /// <param name="parameters"> The paramters values used in the stored procedure. </param>
        /// <returns> An instance of EntityList containing the data retrieved from the database. </returns>
        void ExecuteAndHydrateGenericInstance<T>(T objectInstance,
            
            string procedureName,
            RecordMapper<T>[] recordMappers,
            params object[] parameters)
            where T : new();

        #endregion

        #region ExecuteAndHydrateInstance for <T>'s

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        bool ExecuteAndHydrateInstance<T>(T objectInstance,
            
            string procedureName,
            RecordMapper<T> recordMapper, params object[] parameters);

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        bool ExecuteAndHydrateInstance<T>(T objectInstance,
            
            string procedureName, OracleParameterMapper<T> oracleParameterMapper,
            RecordMapper<T> recordMapper);

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        bool ExecuteAndHydrateInstance<T>(T objectInstance,
            
            string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper);

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        bool ExecuteAndHydrateInstance<T>(T objectInstance,
            
            string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper recordMapper);

        /// <summary>
        ///     Executes a procedure and fires a mapping delegate and hydrates the T instance
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objectInstance"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        bool ExecuteAndHydrateInstance<T>(T objectInstance,
            
            string procedureName,
            RecordMapper recordMapper, params object[] parameters);

        #endregion

        #region ExecuteAndHydrateInstanceList for <T>'s

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        void ExecuteAndHydrateInstanceList<T>(List<T> instanceList,
            string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper) where T : new();

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="TConcrete"> </typeparam>
        /// <typeparam name="TList"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        void ExecuteAndHydrateInstanceList<TConcrete, TList>(ICollection<TList> instanceList,
            string procedureName,
            OracleParameterMapper oracleParameterMapper,
            RecordMapper<TConcrete> recordMapper)
            where TConcrete : TList, new();

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        void ExecuteAndHydrateInstanceList<T>(List<T> instanceList,
            
            string procedureName, RecordMapper<T> recordMapper,
            params object[] parameters) where T : new();

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="parameters"> </param>
        void ExecuteAndHydrateInstanceList<T>(List<T> instanceList,
            
            string procedureName, RecordMapper recordMapper,
            params object[] parameters) where T : new();


        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="oracleOutputMapper"> </param>
        void ExecuteAndHydrateInstanceList<T>(List<T> instanceList,
            
            string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper recordMapper,
            OracleOutputParameterMapper oracleOutputMapper) where T : new();

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="oracleOutputMapper"> </param>
        void ExecuteAndHydrateInstanceList<T>(List<T> instanceList,
            
            string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper,
            OracleOutputParameterMapper oracleOutputMapper) where T : new();


        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="parameterList"> </param>
        /// <param name="recordMapper"> </param>
        /// <param name="oracleOutputMapper"> </param>
        void ExecuteAndHydrateInstanceList<T>(List<T> instanceList,
            
            string procedureName,
            StoredProcedureParameterList parameterList,
            RecordMapper<T> recordMapper,
            OracleOutputParameterMapper oracleOutputMapper) where T : new();

        #endregion

        #region ExecuteDataset

        /// <summary>
        ///     Executes a procedure and for each row returned creates a T instance, fires a mapping delegate and add the instance
        ///     to a List.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="instanceList"> </param>
        /// <param name="procedureName"> </param>
        /// <param name="oracleParameterMapper"> </param>
        /// <param name="recordMapper"> </param>
        void ExecuteDataset<T>(List<T> instanceList,
            string procedureName, OracleParameterMapper oracleParameterMapper,
            RecordMapper<T> recordMapper) where T : new();

        #endregion

        #endregion
    }
}