#region

using STP.DataAccess.Interface;
using STP.DataAccess.Mappers;

#endregion

namespace STP.DataAccess.Delegates
{
    #region SQL Delegates

    /// <summary>
    ///     For injecting parameters into a command.
    /// </summary>
    /// <param name="parameters"> </param>
    public delegate void SqlParameterMapper(ISqlParameterSet parameters);

    /// <summary>
    ///     For injecting parameters from an object instance into a command.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="parameters"> </param>
    /// <param name="objectInstance"> </param>
    public delegate void SqlParameterMapper<in T>(ISqlParameterSet parameters, T objectInstance);

    /// <summary>
    ///     For populating output parameters
    /// </summary>
    /// <param name="outputParameters"> </param>
    public delegate void SqlOutputParameterMapper(ISqlParameterSet outputParameters);

    /// <summary>
    ///     For populating output parameters from an object instance - added for unit testing.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="outputParameters"> </param>
    /// <param name="objectInstance"> </param>
    public delegate void SqlOutputParameterMapper<in T>(ISqlParameterSet outputParameters, T objectInstance);
  
    #endregion
}