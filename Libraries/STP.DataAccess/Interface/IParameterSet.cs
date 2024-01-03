#region

using System;
using System.Data;
using RDW.Common.Enums;

#endregion

namespace RDW.DataAccess.Interface
{
    public interface IParameterSet
    {
        /// <summary>
        ///     Adds parameter to <see cref="IParameterSet" />
        /// </summary>
        /// <param name="key"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        void AddWithValue(string key, object value);

        /// <summary>
        ///     Adds parameter to <see cref="IParameterSet" />
        /// </summary>
        /// <param name="key"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="direction">
        ///     Parameter direction (Input, InputOutput, Output, ReturnValue) For output parameters of
        ///     sizeable types (varchar, etc) use overload with proper data size
        /// </param>
        void AddWithValue(string key, object value, ParameterDirectionWrap direction);

        /// <summary>
        ///     Adds parameter to <see cref="IParameterSet" />
        /// </summary>
        /// <param name="key"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="direction">
        ///     Parameter direction (Input, InputOutput, Output, ReturnValue) For output parameters of
        ///     sizeable types (varchar, etc) use overload with proper data size
        /// </param>
        /// <param name="size"> Size of parameter. Will override inferred parameter size </param>
        void AddWithValue(string key, object value, ParameterDirectionWrap direction, int? size);

        /// <summary>
        ///     Adds typed DBNull valued parameter to <see cref="IParameterSet" />
        /// </summary>
        /// <param name="key"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="direction">
        ///     Parameter direction (Input, InputOutput, Output, ReturnValue) For output parameters of
        ///     sizeable types (varchar, etc) use overload with proper data size
        /// </param>
        /// <param name="dbType"> Type of this parameter. Specified by System.Data.DbType enumration </param>
        void AddTypedDbNull(string key, ParameterDirectionWrap direction, DbType dbType);

        /// <summary>
        ///     Get value of specified parameter
        /// </summary>
        /// <param name="key"> Unique parameter key </param>
        /// <returns> Value of parameter </returns>
        object GetValue(string key);

        int GetInt32(string key);
        bool GetBool(string key);
        string GetString(string key);
        byte GetByte(string key);
        decimal GetDecimal(string key);
        float GetFloat(string key);
        long GetLong(string key);
        DateTime GetDateTime(string key);
    }
}