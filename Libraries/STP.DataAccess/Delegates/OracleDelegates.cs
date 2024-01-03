#region

using STP.DataAccess.Interface;

#endregion

namespace STP.DataAccess.Delegates
{
    #region Oracle Delegates

    /// <summary>
    ///     For injecting parameters into a command.
    /// </summary>
    /// <param name="parameters"> </param>
    public delegate void OracleParameterMapper(IOracleParameterSet parameters);

    /// <summary>
    ///     For injecting parameters from an object instance into a command.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="parameters"> </param>
    /// <param name="objectInstance"> </param>
    public delegate void OracleParameterMapper<in T>(IOracleParameterSet parameters, T objectInstance);

    /// <summary>
    ///     For populating output parameters
    /// </summary>
    /// <param name="outputParameters"> </param>
    public delegate void OracleOutputParameterMapper(IOracleParameterSet outputParameters);

    /// <summary>
    ///     For populating output parameters from an object instance - added for unit testing.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="outputParameters"> </param>
    /// <param name="objectInstance"> </param>
    public delegate void OracleOutputParameterMapper<in T>(IOracleParameterSet outputParameters, T objectInstance);

    #endregion
}