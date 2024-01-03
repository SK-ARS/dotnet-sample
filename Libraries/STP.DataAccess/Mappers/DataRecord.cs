#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Xml;
using STP.Common;
using NetSdoGeometry;

#endregion

namespace STP.DataAccess.Mappers
{
    /// <summary>
    ///     Wraps an individual database result row.
    /// </summary>
    public interface IRecord
    {
        #region Custom Calls

        /// <summary>
        ///     Returns the item of type T inhabiting the field index. Will throw exception if null.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="i"> </param>
        /// <returns> </returns>
        T Get<T>(int i);

        /// <summary>
        ///     Returns the item of type T inhabiting the field index. If DBNull, will return the default value.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="i"> </param>
        /// <param name="defaultValue"> </param>
        /// <returns> </returns>
        T GetOrDefault<T>(int i, T defaultValue);

        /// <summary>
        ///     Returns the item of type T inhabiting the field name. Will throw exception if null.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        T Get<T>(string fieldName);

        /// <summary>
        ///     Returns an item of type T inhabiting the field name. If DBNull, will return the default value.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="fieldName"> </param>
        /// <param name="defaultValue"> </param>
        /// <returns> </returns>
        T GetOrDefault<T>(string fieldName, T defaultValue);

        /// <summary>
        ///     Returns an item of type T inhabiting the field index. If DBNull, will return null.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="fieldIndex"> </param>
        /// <returns> </returns>
        T GetOrNull<T>(int fieldIndex) where T : class;

        /// <summary>
        ///     Returns an item of type T inhabiting the field name. If DBNull, will return null.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        T GetOrNull<T>(string fieldName) where T : class;

        /// <summary>
        ///     Returns a string, or empty if the value is null.
        /// </summary>
        /// <param name="i"> </param>
        /// <returns> </returns>
        string GetStringOrEmpty(int i);

        /// <summary>
        ///     Returns a string, or empty if the value is null.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        string GetStringOrEmpty(string fieldName);

        /// <summary>
        ///     Returns a string, or the supplied default if the string is DBNull.
        /// </summary>
        /// <param name="i"> </param>
        /// <param name="defaultString"> </param>
        /// <returns> </returns>
        string GetStringOrDefault(int i, string defaultString);

        /// <summary>
        ///     Returns a char, or the supplied default if the string is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        char GetCharOrDefault(string fieldName);

        /// <summary>
        ///     Returns a string, or the supplied default if the string is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        string GetStringOrDefault(string fieldName);

        /// <summary>
        ///     Returns a string, or null if the string is DBNull.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <returns> </returns>
        string GetStringOrNull(int fieldIndex);


        /// <summary>
        ///     Decompresses a byte array and returns the corresponding string, or null if the byte array is null.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <returns> </returns>
        string GetStringOrNullFromCompressedByteArray(int fieldIndex);

        /// <summary>
        ///     Will return an Int32, or the default value if the Int32 is DBNull.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <param name="defaultValue"> </param>
        /// <returns> </returns>
        int GetInt32OrDefault(int fieldIndex, int defaultValue);
      

        /// <summary>
        ///     Will return an Int32, or the default value if the Int32 is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        int GetInt32OrDefault(string fieldName);

        /// <summary>
        ///     Will return an Int32, or the Null value if the Int32 is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        int? GetInt32Nullable(string fieldName);

        /// <summary>
        ///     Will return an Int32, or the default value if the Int32 is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        int GetInt32OrMinus1(string fieldName);


        /// <summary>
        ///     Gets XmlElement Data from the column
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        XmlElement GetXml(string fieldName);

        /// <summary>
        ///     Will return the initialized value of Int32 if the item is DBNull.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <returns> </returns>
        int GetInt32OrEmpty(int fieldIndex);

        /// <summary>
        ///     Will convert a byte and return an Int32, or the default value if the byte is DBNull.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <param name="defaultValue"> </param>
        /// <returns> </returns>
        int GetInt32OrDefaultFromByte(int fieldIndex, int defaultValue);

        /// <summary>
        ///     Will convert a byte and return an Int32, initialized value of Int32 if the item is DBNull.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <returns> </returns>
        int GetInt32OrEmptyFromByte(int fieldIndex);

        /// <summary>
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <returns> a byte array or null if the item is DBNull. </returns>
        Byte[] GetByteArrayOrNull(int fieldIndex);

        /// <summary>
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> a byte array or null if the item field name is DBNull. </returns>
        Byte[] GetByteArrayOrNull(string fieldName);

        /// <summary>
        ///     Will return an Int16, or the default value if the Int16 is DBNull.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <param name="defaultValue"> </param>
        /// <returns> </returns>
        short GetInt16OrDefault(int fieldIndex, short defaultValue);

        short? GetInt16Nullable(string fieldName);

        /// <summary>
        ///     Will return an Int16, or the default value if the Int16 is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        short GetInt16OrDefault(string fieldName);

        /// <summary>
        ///     Will return a DateTime object, or the default value if the DateTime is DBNull.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <returns> </returns>
        DateTime GetDateTimeOrEmpty(int fieldIndex);

        /// <summary>
        ///     Will return a DateTime object, or the default value if the DateTime is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        DateTime GetDateTimeOrDefault(string fieldName);

        /// <summary>
        ///     Will return a DateTime object, or the default value if the DateTime is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        DateTime GetDateTimeFromString(string fieldName);

        /// <summary>
        ///     Will return a DateTime object, or the default value if the DateTime is DBNull AS PST value.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        DateTime GetPSTDateTimeOrDefault(string fieldName);

         DateTime GetDateTimeOrEmpty(string fieldName);
         

        /// <summary>
        ///     Will return a Decimal object, or the default value if the Decimal is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        decimal GetDecimalOrDefault(string fieldName);

        /// <summary>
        ///     Will return a Decimal object, or the null if the Decimal is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        decimal? GetDecimalNullable(string fieldName);

        /// <summary>
        ///     Will return a Decimal object, or the default value if the Decimal is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        decimal GetDecimalFromMoneyOrDefault(string fieldName);

        /// <summary>
        ///     Will return a Long object, or the default value if the Decimal is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        long GetLongOrDefault(string fieldName);

        /// <summary>
        ///     Will return a Long object, or the Null if the long is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        long? GetLongOrNullable(string fieldName);

        /// <summary>
        ///     Will return a float object, or the default value if the float is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        float GetFloatOrDefault(string fieldName);

        /// <summary>
        ///     Will return a double object, or the default value if the float is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        double GetDoubleOrDefault(string fieldName);

        /// <summary>
        ///     Will return a double object, or the Null value if the float is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        double? GetDoubleNullable(string fieldName);

        /// <summary>
        ///     Will return a short object, or the default value if the short is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        short GetShortOrDefault(string fieldName);

        /// <summary>
        ///     Will return a bool object, or the default value if the bool is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        bool GetBooleanOrFalse(string fieldName);

        /// <summary>
        ///     Will return a bool object
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        bool? GetNullableBoolean(string fieldName);

        /// <summary>
        ///     Will return an SqlXml object or null if value is DBNull or if the underlying data set does not support the XML data
        ///     type.
        /// </summary>
        /// <param name="fieldIndex"> </param>
        /// <returns> </returns>
        SqlXml GetSqlXml(int fieldIndex);


        /// <summary>
        ///     Will return an Int32, or the default value if the Int32 is DBNull.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        int GetInt32OrDefaultFromByte(string fieldName);

        /// <summary>
        ///     GetDataTypeName for the column name
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        string GetDataTypeName(string fieldName);

        /// <summary>
        ///     GetFieldType for the column name
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        Type GetFieldType(string fieldName);


        Guid GetGuidOrEmpty(string fieldName);

        /// <summary>
        ///     Will return a Local DateTime object, or the default value if the DateTime is DBNull and timeZoneName is null or
        ///     empty.
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <param name="timeZoneName"> </param>
        /// <returns> </returns>
        DateTime GetLocalDateTime(string fieldName, string timeZoneName);




        sdogeometry GetGeometryOrNull(string fieldName);

        #endregion

        #region IDataRecord implementation

        // Methods
        int FieldCount { get; }
        object this[string name] { get; }
        object this[int i] { get; }
        bool GetBoolean(int i);
        byte GetByte(int i);
        long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);
        char GetChar(int i);
        long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length);
        IDataReader GetData(int i);
        string GetDataTypeName(int i);
        DateTime GetDateTime(int i);
        decimal GetDecimal(int i);
        double GetDouble(int i);
        Type GetFieldType(int i);
        float GetFloat(int i);
        Guid GetGuid(int i);
        short GetInt16(int i);
        int GetInt32(int i);
        long GetInt64(int i);
        string GetName(int i);
        int GetOrdinal(string name);
        string GetString(int i);
        object GetValue(int i);
        int GetValues(object[] values);

        // Properties
        bool GetBooleanFromByte(int i);
        bool GetBooleanOrFalse(int i);
        byte GetByteOrDefault(int i, byte defaultValue);
        byte GetByteOrDefault(string fieldName);
        int? GetInt32Nullable(int i);
        short? GetInt16Nullable(int i);

        /// <summary>
        ///     Whether or not the value is null in the database.
        /// </summary>
        /// <param name="i"> </param>
        /// <returns> </returns>
        bool IsDBNull(int i);
        Single GetSingleOrDefault(string p);
        Single? GetSingleNullable(string p);
        #endregion


    }

    /// <summary>
    ///     Wraps a resultset from a database call.
    /// </summary>
    public interface IRecordSet : IDisposable, IRecord
    {
        #region Custom Calls

        #endregion

        int Depth { get; }
        bool IsClosed { get; }


        int RecordsAffected { get; }

        /// <summary>
        ///     Returns the database connection to the pool, or closes a non-pooled connection.
        /// </summary>
        void Close();

        /// <summary>
        ///     Returns information on the underlying schema.
        /// </summary>
        /// <returns> </returns>
        DataTable GetSchemaTable();

        /// <summary>
        ///     Move to the next result set.
        /// </summary>
        /// <returns> </returns>
        bool NextResult();

        /// <summary>
        ///     Read in the next record (or read the 1st record if the result is fresh.)
        /// </summary>
        /// <returns> </returns>
        bool Read();
    }


    /// <summary>
    ///     An encapsulation of an IDataReader This class is used to provide an API similar to a SQL data reader except that it
    ///     is possible to navigate through any part of the result set. It is also possible to execute commands while this
    ///     class is still "open" even though the CLR in-proc provider does not support MARS at this time. This implementation
    ///     is highly simplified to make the sample easy to understand. A better implementation would fetch multiple rows to
    ///     avoid a database turnaround per row fetched. Using this class can have a significantly smaller memory footprint
    ///     than filling a dataset with all the results of a query which is very important for server side programming.
    /// </summary>
    internal class DataRecord : IRecordSet
    {
        private readonly IDataReader wr;

        internal DataRecord(IDataReader wrappedReader)
        {
            wr = wrappedReader;
        }

        #region IRecordSet Members

        public int FieldCount
        {
            get { return wr.FieldCount; }
        }

        public bool GetBoolean(int i)
        {
            return wr.GetBoolean(i);
        }

        public bool GetBooleanFromByte(int i)
        {
            return (wr.GetByte(i) == 1);
        }

        public byte GetByte(int i)
        {
            return wr.GetByte(i);
        }

        public byte GetByteOrDefault(int i, byte defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : wr.GetByte(i);
        }

        public byte GetByteOrDefault(string fieldName)
        {
            if (IsDBNull(GetOrdinal(fieldName)))
            {
                return byte.MinValue;
            }
            byte b;
            try
            {
                b = Get<byte>(fieldName);
            }
            catch
            {
                b = byte.MinValue;
            }
            return b;
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return wr.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return wr.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return wr.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return wr.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return wr.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return wr.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return wr.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return wr.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return wr.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return wr.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return wr.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return wr.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return wr.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return wr.GetInt64(i);
        }

        public string GetName(int i)
        {
            return wr.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return wr.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return wr.GetString(i);
        }

        public object GetValue(int i)
        {
            return wr.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return wr.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return wr.IsDBNull(i);
        }

        public object this[string name]
        {
            get { return wr[name]; }
        }

        public object this[int i]
        {
            get { return wr[i]; }
        }

        public string GetStringOrEmpty(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? string.Empty : Get<string>(fieldName).Trim();
        }

        public string GetStringOrDefault(int i, string defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : GetString(i).Trim();
        }

        public char GetCharOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? char.MinValue : Get<char>(fieldName);
        }

        public string GetStringOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? string.Empty : Get<string>(fieldName).Trim();
        }

        public string GetStringOrEmpty(int i)
        {
            return GetStringOrDefault(i, string.Empty);
        }

        public string GetStringOrNull(int i)
        {
            return (IsDBNull(i)) ? null : GetString(i);
        }

        public Guid GetGuidOrEmpty(string fieldName)
        {
            Guid i;
            if (IsDBNull(GetOrdinal(fieldName)))
            {
                i = Guid.Empty;
            }
            else
            {
                try
                {
                    i = Get<Guid>(fieldName);
                }
                catch
                {
                    i = Guid.Empty;
                }
            }
            return i;
        }

        public int GetInt32OrDefaultFromByte(string fieldName)
        {
            int i;
            if (IsDBNull(GetOrdinal(fieldName)))
            {
                i = 0;
            }
            else
            {
                byte b;
                try
                {
                    b = Get<byte>(fieldName);
                }
                catch
                {
                    b = byte.MinValue;
                }
                i = b;
            }
            return i;
        }


        public string GetStringOrNullFromCompressedByteArray(int fieldIndex)
        {
            Byte[] compressedString = GetByteArrayOrNull(fieldIndex);
            if (compressedString == null || compressedString.Length == 0)
            {
                return null;
            }
            //ToDo:Do not change this : Prakash
            return compressedString.ToString();
            //return UnicodeStringCompressor.Decompress(compressedString);
        }


        /// <summary>
        ///     GetDataTypeName for the column name
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        public string GetDataTypeName(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? string.Empty : GetDataTypeName(GetOrdinal(fieldName)).Trim();
        }

        /// <summary>
        ///     GetFieldType for the column name
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        public Type GetFieldType(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? null : GetFieldType(GetOrdinal(fieldName));
        }

        public short? GetInt16Nullable(int fieldIndex)
        {
            if (IsDBNull(fieldIndex)) return null;

            return GetInt16(fieldIndex);
        }

        public int? GetInt32Nullable(int fieldIndex)
        {
            if (IsDBNull(fieldIndex)) return null;

            return GetInt32(fieldIndex);
        }

        public int GetInt32OrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? 0 : Get<int>(fieldName);
        }

        public int GetInt32OrMinus1(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? -1 : Get<int>(fieldName);
        }


        /// <summary>
        ///     Gets and Xml Data from the column
        /// </summary>
        /// <param name="fieldName"> </param>
        /// <returns> </returns>
        public XmlElement GetXml(string fieldName)
        {
            if (IsDBNull(GetOrdinal(fieldName)))
            {
                return null;
            }
            try
            {
                var xmlDocument = new XmlDocument();
                SqlXml sqlXml = GetSqlXml(fieldName);
                if (!sqlXml.IsNull)
                {
                    xmlDocument.LoadXml(sqlXml.Value);
                    XmlElement xmlElement = xmlDocument.DocumentElement;
                    return xmlElement;
                }
                return null;
            }
            catch (Exception exception)
            {
                STPConfigurationManager.LogProvider.Log(exception);
            }
            return null;
        }

        public int GetInt32OrDefault(int fieldIndex, int defaultValue)
        {
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt32(fieldIndex);
        }

        public int? GetInt32Nullable(string fieldName)
        {
            //return (IsDBNull(GetOrdinal(fieldName))) ? null : GetOrNull<Decimal>(fieldName);

            if (IsDBNull(GetOrdinal(fieldName))) return null;

            return Get<int>(fieldName);
        }

        public int GetInt32OrEmpty(int fieldIndex)
        {
            return GetInt32OrDefault(fieldIndex, 0);
        }

        public int GetInt32OrDefaultFromByte(int fieldIndex, int defaultValue)
        {
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt32FromByte(fieldIndex);
        }

        public int GetInt32OrEmptyFromByte(int fieldIndex)
        {
            return GetInt32OrDefaultFromByte(fieldIndex, 0);
        }

        public short GetInt16OrDefault(int fieldIndex, short defaultValue)
        {
            return (IsDBNull(fieldIndex)) ? defaultValue : GetInt16(fieldIndex);
        }

        public short? GetInt16Nullable(string fieldName)
        {
            if (IsDBNull(GetOrdinal(fieldName))) return null;

            return Get<short?>(fieldName);
        }
        public short GetInt16OrDefault(string fieldName)
        {
            return Get<short>(fieldName);
        }

        public Byte[] GetByteArrayOrNull(int fieldIndex)
        {
            if (IsDBNull(fieldIndex))
            {
                return null;
            }
            var bytes = new Byte[(GetBytes(fieldIndex, 0, null, 0, int.MaxValue))];
            GetBytes(fieldIndex, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public Byte[] GetByteArrayOrNull(string fieldName)
        {
            //if (string.IsNullOrEmpty(fieldName))
            //{
            //    return null;
            //}
            //return Get<Byte[]>(fieldName);
            return (IsDBNull(GetOrdinal(fieldName))) ? null : Get<Byte[]>(fieldName);
            //var bytes = new Byte[(GetBytes(fieldIndex, 0, null, 0, int.MaxValue))];
            //GetBytes(fieldIndex, 0, bytes, 0, bytes.Length);
            //return bytes;
        }

        public DateTime GetDateTimeOrEmpty(int fieldIndex)
        {
            return (IsDBNull(fieldIndex)) ? DateTime.MinValue : GetDateTime(fieldIndex);
        }

        /// <summary>
        ///     Converts Db datetime to Local Date time using Session Value of Time Zone
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public DateTime GetDateTimeOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? DateTime.MinValue : GetLocalDateTime(fieldName);
        }

        public DateTime GetDateTimeFromString(string fieldName)
        {
            if (IsDBNull(GetOrdinal(fieldName)))
            {
                return DateTime.MinValue;
            }
            DateTime dt;
            DateTime.TryParse(Get<string>(fieldName), out dt);
            return dt;
        }
        public DateTime GetDateTimeOrEmpty(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? DateTime.MinValue : GetLocalDateTime(fieldName);
        }
           
        public DateTime GetPSTDateTimeOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? DateTime.MinValue : Get<DateTime>(fieldName);
        }

        public decimal GetDecimalOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? decimal.Zero : Get<decimal>(fieldName);
        }


        public double? GetDoubleNullable(string fieldName)
        {
            //return (IsDBNull(GetOrdinal(fieldName))) ? null : GetOrNull<Decimal>(fieldName);

            if (IsDBNull(GetOrdinal(fieldName))) return null;

            return Get<double>(fieldName);
        }

        public Single? GetSingleNullable(string fieldName)
        {
            //return (IsDBNull(GetOrdinal(fieldName))) ? null : GetOrNull<Decimal>(fieldName);

            if (IsDBNull(GetOrdinal(fieldName))) return null;

            return Get<Single>(fieldName);
        }

        public decimal? GetDecimalNullable(string fieldName)
        {
            //return (IsDBNull(GetOrdinal(fieldName))) ? null : GetOrNull<Decimal>(fieldName);

            if (IsDBNull(GetOrdinal(fieldName))) return null;

            return Get<decimal>(fieldName);
        }

        public decimal GetDecimalFromMoneyOrDefault(string fieldName)
        {
            if (IsDBNull(GetOrdinal(fieldName)))
            {
                return decimal.Zero;
            }
            decimal b = decimal.Zero;
            try
            {
                var money = Get<SqlMoney>(fieldName);
                if (!money.IsNull)
                {
                    b = money.ToDecimal();
                }
            }
            catch
            {
                b = decimal.Zero;
            }
            return b;
        }

        public long GetLongOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? 0 : Get<Int64>(fieldName);
        }

        public long? GetLongOrNullable(string fieldName)
        {
            //return (IsDBNull(GetOrdinal(fieldName))) ? null : GetOrNull<Decimal>(fieldName);

            if (IsDBNull(GetOrdinal(fieldName))) return null;

            return Get<Int64>(fieldName);
        }

        public short GetShortOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? (short)0 : Get<short>(fieldName);
        }

        public float GetFloatOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? 0 : (float)Get<double>(fieldName);
        }

        public double GetDoubleOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? 0 : Get<double>(fieldName);
        }
        /// <summary>
        /// Retrieves the Fieldname in Single
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Single GetSingleOrDefault(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? 0 : Get<Single>(fieldName);
        }
        public SqlXml GetSqlXml(int fieldIndex)
        {
            if (IsDBNull(fieldIndex))
                return null;

            var sdr = wr as SqlDataReader;
            if (sdr == null)
                return null;

            return sdr.GetSqlXml(fieldIndex);
        }

        public T Get<T>(int i)
        {
            return (T)this[i];
        }

        public T Get<T>(string fieldName)
        {
            return (T)this[fieldName];
        }


        public T GetOrDefault<T>(int i, T defaultValue)
        {
            return (IsDBNull(i)) ? defaultValue : Get<T>(i);
        }

        public T GetOrDefault<T>(string fieldName, T defaultValue)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? defaultValue : Get<T>(fieldName);
        }

        public T GetOrNull<T>(int fieldIndex) where T : class
        {
            return (IsDBNull(fieldIndex)) ? null : Get<T>(fieldIndex);
        }

        public T GetOrNull<T>(string fieldName) where T : class
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? null : Get<T>(fieldName);
        }

        public bool GetBooleanOrFalse(int fieldIndex)
        {
            return !(IsDBNull(fieldIndex)) && GetBoolean(fieldIndex);
        }

        public bool GetBooleanOrFalse(string fieldName)
        {
            return !(IsDBNull(GetOrdinal(fieldName))) && Get<bool>(fieldName);
        }

        public bool? GetNullableBoolean(string fieldName)
        {
            bool? x;
            if (IsDBNull(GetOrdinal(fieldName)))
            {
                x = null;
            }
            else
            {
                x = Get<bool>(fieldName);
            }

            return x;
        }


        public void Close()
        {
            wr.Close();
        }

        public int Depth
        {
            get { return wr.Depth; }
        }

        public DataTable GetSchemaTable()
        {
            return wr.GetSchemaTable();
        }

        public bool IsClosed
        {
            get { return wr.IsClosed; }
        }

        public bool NextResult()
        {
            return wr.NextResult();
        }

        public bool Read()
        {
            return wr.Read();
        }

        public int RecordsAffected
        {
            get { return wr.RecordsAffected; }
        }

        public void Dispose()
        {
            wr.Dispose();
        }

        #region Local date time logic

        public DateTime GetLocalDateTime(string fieldName, string timeZoneName)
        {
            DateTime dbDate = (IsDBNull(GetOrdinal(fieldName)))
                ? DateTime.MinValue
                : GetLocalDateTime(fieldName, timeZoneName);

            DateTime localDateTime = dbDate;

            if (!string.IsNullOrEmpty(timeZoneName))
            {
                if (dbDate == DateTime.MinValue || dbDate == DateTime.MaxValue)
                {
                    return localDateTime;
                }
                TimeZoneInfo psZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                //string tZoneName = psZone.IsDaylightSavingTime(dbDate) ? psZone.DaylightName : psZone.StandardName;
                TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                localDateTime = TimeZoneInfo.ConvertTime(dbDate, psZone, localZone);
                return localDateTime;
            }
            return localDateTime;
        }

        /// <summary>
        ///     This should be used taking out logically by passing time zone as parameter as strictly System.Web should not be in
        ///     DataAccess projects as its separated
        /// </summary>
        /// <param name="strDbDateTime"></param>
        /// <returns></returns>
        public DateTime GetLocalDateTime(string strDbDateTime)
        {
            //DateTime psTime = new DateTime(2011, 09, 26, 00, 10, 00);
            var dbDateTime = Get<DateTime>(strDbDateTime);
            try
            {
                if (dbDateTime != DateTime.MinValue && dbDateTime != DateTime.MaxValue)
                {
                    TimeZoneInfo psZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    string timeZoneName = psZone.IsDaylightSavingTime(dbDateTime)
                        ? psZone.DaylightName
                        : psZone.StandardName;
                    if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                        HttpContext.Current.Session["TimeZoneName"] != null)
                    {
                        TimeZoneInfo localZone =
                            TimeZoneInfo.FindSystemTimeZoneById(HttpContext.Current.Session["TimeZoneName"].ToString());
                        DateTime localDateTime = TimeZoneInfo.ConvertTime(dbDateTime, psZone, localZone);
                        return localDateTime;
                    }
                }
            }
            catch (TimeZoneNotFoundException)
            {
                STPConfigurationManager.LogProvider.Log("The registry does not define the Pacific Standard Time zone.");
            }
            catch (InvalidTimeZoneException)
            {
                STPConfigurationManager.LogProvider.Log("Registry data on the Pacific Standard Time zone has been corrupted.");
            }
            return dbDateTime;
        }

        #endregion

        #endregion

        #region Local date time logic

        /*
		public DateTime GetLocalDateTime(DateTime dbDateTime)
		{
		   
			//DateTime psTime = new DateTime(2011, 09, 26, 00, 10, 00);
			string strMsg = string.Empty;
			DateTime localDateTime = DateTime.MinValue;
			try
			{
				if (dbDateTime != DateTime.MinValue)
				{
					TimeZoneInfo psZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
					string timeZoneName = psZone.IsDaylightSavingTime(dbDateTime) ? psZone.DaylightName : psZone.StandardName;
					if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["TimeZoneName"]!=null)
					{
						TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById(HttpContext.Current.Session["TimeZoneName"].ToString());
						localDateTime = TimeZoneInfo.ConvertTime(dbDateTime, psZone, localZone);
					}
					//localDateTime = TimeZoneInfo.ConvertTime(dbDateTime, psZone, TimeZoneInfo.Local);
					//TimeSpan psOffset = psZone.GetUtcOffset(dbDateTime);
				}
			}
			catch (TimeZoneNotFoundException)
			{
				strMsg = "The registry does not define the Pacific Standard Time zone.";
			}
			catch (InvalidTimeZoneException)
			{
				strMsg = "Registry data on the Pacific Standard Time zone has been corrupted.";
			}
			
			return dbDateTime;
		}
		*/

        #endregion

        public int GetInt32OrEmpty(string fieldName)
        {
            return GetInt32OrDefault(fieldName);
        }

        public SqlXml GetSqlXml(string fieldName)
        {
            if (IsDBNull(GetOrdinal(fieldName)))
                return null;

            var sdr = wr as SqlDataReader;
            if (sdr == null)
                return null;

            return sdr.GetSqlXml(GetOrdinal(fieldName));
        }

        public int GetInt32FromByte(int i)
        {
            return Convert.ToInt32(wr.GetByte(i));
        }

        public sdogeometry GetGeometryOrNull(string fieldName)
        {
            return (IsDBNull(GetOrdinal(fieldName))) ? null : Get<sdogeometry>(fieldName);
        }
    }
}