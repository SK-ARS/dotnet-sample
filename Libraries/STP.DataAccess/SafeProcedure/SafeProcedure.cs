#region

using STP.DataAccess.Interface;
using STP.DataAccess.Provider;

#endregion

namespace STP.DataAccess.SafeProcedure
{
    public sealed class SafeProcedure
    {
        public class DBProvider
        {

            //// <summary>
            ////     Gets All methods for SQL Provider
            //// </summary>
            //public static ISqlProvider Sql
            //{
            //    get { return SqlProvider.Instance; }
            //}
            /// <summary>
            ///     Gets All methods for SQL Provider
            /// </summary>
            public static IOracleProvider Oracle
            {
                get { return OracleProvider.Instance; }
            }
            //// <summary>
            ////     Gets All methods for SQL Provider
            //// </summary>
            //public static IOleDbProvider Oledb
            //{
            //    get { return OleDbProvider.Instance; }
            //}


        }




        
    }
}