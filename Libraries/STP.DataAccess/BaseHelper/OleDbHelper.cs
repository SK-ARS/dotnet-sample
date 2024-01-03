using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace STP.DataAccess.BaseHelper
{
    /// <summary>
    ///     OleDbHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
    ///     ability to discover parameters for stored procedures at run-time.
    /// </summary>
    public sealed class OleDbHelperParameterCache
    {
        #region private methods, variables, and constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new OleDbHelperParameterCache()".

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        private OleDbHelperParameterCache()
        {
        }

        /// <summary>
        ///     resolve at run-time the appropriate set of OleDbParameters for a stored procedure
        /// </summary>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <param name="includeReturnValueParameter">weather or not to onclude ther return value parameter</param>
        /// <returns></returns>
        private static OleDbParameter[] DiscoverSpParameterSet(string connectionString, string spName,
            bool includeReturnValueParameter)
        {
            using (OleDbConnection cn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(spName, cn))
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                OleDbCommandBuilder.DeriveParameters(cmd);

                if (!includeReturnValueParameter)
                {
                    if (ParameterDirection.ReturnValue == cmd.Parameters[0].Direction)
                        cmd.Parameters.RemoveAt(0);
                }

                OleDbParameter[] discoveredParameters = new OleDbParameter[cmd.Parameters.Count];

                cmd.Parameters.CopyTo(discoveredParameters, 0);

                return discoveredParameters;
            }
        }

        //deep copy of cached OleDbParameter array
        private static OleDbParameter[] CloneParameters(OleDbParameter[] originalParameters)
        {
            OleDbParameter[] clonedParameters = new OleDbParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (OleDbParameter) ((ICloneable) originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion private methods, variables, and constructors

        #region caching functions

        /// <summary>
        ///     add parameter array to the cache
        /// </summary>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <param name="commandParameters">an array of OleDbParamters to be cached</param>
        public static void CacheParameterSet(string connectionString, string commandText,
            params OleDbParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        ///     retrieve a parameter array from the cache
        /// </summary>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="commandText">the stored procedure name or T-OleDb command</param>
        /// <returns>an array of OleDbParameters</returns>
        public static OleDbParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;

            OleDbParameter[] cachedParameters = (OleDbParameter[]) paramCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion caching functions

        #region Parameter Discovery Functions

        /// <summary>
        ///     Retrieves the set of OleDbParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        ///     This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OleDbConnection</param>
        /// <param name="spName">the name of the stored prcedure</param>
        /// <returns>an array of OleDbParameters</returns>
        public static OleDbParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, true);
        }

        /// <summary>
        ///     Retrieves the set of OleDbParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        ///     This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">a valid connection string for an OleDbConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">
        ///     a bool value indicating weather the return value parameter should be included
        ///     in the results
        /// </param>
        /// <returns>an array of OleDbParameters</returns>
        public static OleDbParameter[] GetSpParameterSet(string connectionString, string spName,
            bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + spName +
                             (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            OleDbParameter[] cachedParameters;

            cachedParameters = (OleDbParameter[]) paramCache[hashKey];

            if (cachedParameters == null)
            {
                cachedParameters =
                    (OleDbParameter[])
                        (paramCache[hashKey] =
                            DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
            }

            return CloneParameters(cachedParameters);
        }

        #endregion Parameter Discovery Functions
    }
}