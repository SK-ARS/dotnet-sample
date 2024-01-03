using System;
using System.Data;
using Oracle.DataAccess.Client;
using STP.Common.Enums;

namespace STP.DataAccess.Interface
{
    public interface IOracleParameterSet
    {
        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="obj"></param>
        void Add(object obj);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="paramArray"></param>
        void AddRange(Array paramArray);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        void AddWithValue(string name, object value);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="dbType"></param>
        void AddWithValue(string name, object value, OracleDbType dbType);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction, int? size);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="srcColumn"></param>
        void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction, int? size, string srcColumn);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="srcColumn"></param>
        /// <param name="precision"></param>
        void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction, int? size, string srcColumn, byte precision);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="srcColumn"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction, int? size, string srcColumn, byte precision, byte scale);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">
        ///     Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure
        /// </param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="srcColumn"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="version"></param>
        void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction, int? size, string srcColumn, byte precision, byte scale, DataRowVersion version);

        /// <summary>
        ///     Adds parameter to <see cref="IOracleParameterSet" />
        /// </summary>
        /// <param name="name"> Unique key corresponding to stored procedure parameter </param>
        /// <param name="value">Value for stored procedure parameter. If null, the DBNull value will be used as parameter value
        ///     for stored procedure</param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="srcColumn"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="version"></param>
        /// <param name="isNullable"></param>
        void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction, int? size, string srcColumn, byte precision, byte scale, DataRowVersion version, bool isNullable);

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