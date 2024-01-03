#region

using STP.DataAccess.Mappers;

#endregion

namespace STP.DataAccess.Delegates
{

    #region SQL Delegates

    /// <summary>
    ///     For mapping individual records from a single-resultset procedure.
    /// </summary>
    /// <param name="record"> </param>
    public delegate void RecordMapper(IRecord record);

    /// <summary>
    ///     For mapping individual records from a single resultset procedure to an object instance.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="record"> </param>
    /// <param name="objectInstance"> </param>
    public delegate void RecordMapper<in T>(IRecord record, T objectInstance);

    /// <summary>
    ///     For mapping individual records from a multiple resultset procedure to an object instance.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="record"> </param>
    /// <param name="objectInstance"> </param>
    /// <param name="recordSetIndex"> </param>
    public delegate void MrsRecordMapper<in T>(IRecord record, int recordSetIndex, T objectInstance);

    /// <summary>
    ///     For mapping entire results from single or multi-resultset procedures.
    /// </summary>
    /// <param name="record"> </param>
    public delegate void ResultMapper(IRecordSet record);

    /// <summary>
    ///     fro Async Non Query Callback
    /// </summary>
    public delegate void AsyncNonQueryCallback();

    #endregion
}