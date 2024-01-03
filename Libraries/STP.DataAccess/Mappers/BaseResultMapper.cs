#region

using System;
using System.Collections.Generic;
using System.Data;

#endregion

namespace STP.DataAccess.Mappers
{
    /// <summary>
    ///     A set of methods that will check a IDataRecord for a null value before they return.
    /// </summary>
    public class BaseResultMapper
    {
        /// <summary>
        ///     Retrieves a decimal value or 0 for null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static decimal GetDecimal(IDataRecord reader, int ordinal)
        {
            return (reader[ordinal] != DBNull.Value) ? reader.GetDecimal(ordinal) : 0;
        }

        /// <summary>
        ///     Retrieves a String or Empty if null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static string GetString(IDataRecord reader, int ordinal)
        {
            return GetString(reader, ordinal, String.Empty);
        }

        /// <summary>
        ///     Retrieves a string or the specified nullValue if the string is null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        protected static string GetString(IDataRecord reader, int ordinal, string nullValue)
        {
            return (reader[ordinal] != DBNull.Value) ? reader.GetString(ordinal) : nullValue;
        }

        /// <summary>
        ///     Retrieves a Date, or MinValue if null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static DateTime GetDate(IDataRecord reader, int ordinal)
        {
            return (reader[ordinal] != DBNull.Value) ? reader.GetDateTime(ordinal) : DateTime.MinValue;
        }

        /// <summary>
        ///     Retrieves a SmallInt.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static int GetSmallInt(IDataRecord reader, int ordinal)
        {
            return GetSmallInt(reader, ordinal, 0);
        }

        /// <summary>
        ///     Retrieves a SmallInt, or the specified null replacement.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static int GetSmallInt(IDataRecord reader, int ordinal, int defaultValue)
        {
            return (reader[ordinal] != DBNull.Value) ? reader.GetInt16(ordinal) : defaultValue;
        }


        /// <summary>
        ///     Retrieves an integer or 0 if null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static int GetInt(IDataRecord reader, int ordinal)
        {
            return (reader[ordinal] != DBNull.Value) ? reader.GetInt32(ordinal) : 0;
        }

        /// <summary>
        ///     Converts a tinyint to an int, or 0 if null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static int GetIntFromTinyInt(IDataRecord reader, int ordinal)
        {
            return (reader[ordinal] != DBNull.Value) ? Convert.ToInt32(reader.GetByte(ordinal)) : 0;
        }

        /// <summary>
        ///     Retrieves a boolean value, or false if null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static bool GetBool(IDataRecord reader, int ordinal)
        {
            return (reader[ordinal] != DBNull.Value) && reader.GetBoolean(ordinal);
        }

        /// <summary>
        ///     Retrieves bytes from a datarecord, or null if null.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        protected static byte[] GetBytes(IDataRecord reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            var bytes = new List<byte>();

            var buffer = new byte[256];
            long readCount;
            long readTotal = 0;

            do
            {
                readCount = reader.GetBytes(ordinal, readTotal, buffer, 0, buffer.Length);
                readTotal += readCount;

                if (readCount == 0)
                    break;

                bytes.AddRange(buffer);
            } while (readCount > 0);

            bytes = bytes.GetRange(0, (int) readTotal);
            // in case the message byte array is not factorable by buffer.Length

            return bytes.ToArray();
        }
    }
}