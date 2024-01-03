#region

using System;
using System.Data;
using System.Data.SqlClient;
using RDW.Common.Enums;
using RDW.DataAccess.Interface;

#endregion

namespace RDW.DataAccess.ParameterSet
{
    internal class ParameterSet : IParameterSet
    {
        private readonly SqlParameterCollection pm;

        internal ParameterSet(SqlParameterCollection sqlParameterCollection)
        {
            pm = sqlParameterCollection;
        }

        #region IParameterSet Members

        public object GetValue(string key)
        {
            return pm[key].Value;
        }

        public int GetInt32(string key)
        {
            int i = 0;
            var value = pm[key].Value;
            if (value != null) int.TryParse(value.ToString(), out i);
            return i;
        }

        public bool GetBool(string key)
        {
            bool output = false;
            var value = pm[key].Value;
            if (value != null) bool.TryParse(value.ToString(), out output);
            return output;
        }

        public string GetString(string key)
        {
            var value = pm[key].Value;
            if (value != null) return value.ToString();
            return null;
        }

        public byte GetByte(string key)
        {
            byte output = 0;
            var value = pm[key].Value;
            if (value != null) byte.TryParse(value.ToString(), out output);
            return output;
        }

        public decimal GetDecimal(string key)
        {
            decimal output = 0;
            var value = pm[key].Value;
            if (value != null) decimal.TryParse(value.ToString(), out output);
            return output;
        }

        public float GetFloat(string key)
        {
            float output = 0;
            var value = pm[key].Value;
            if (value != null) float.TryParse(value.ToString(), out output);
            return output;
        }

        public long GetLong(string key)
        {
            long output = 0;
            var value = pm[key].Value;
            if (value != null) long.TryParse(value.ToString(), out output);
            return output;
        }

        public DateTime GetDateTime(string key)
        {
            var output = new DateTime();
            var value = pm[key].Value;
            if (value != null) DateTime.TryParse(value.ToString(), out output);
            return output;
        }

        public void AddTypedDbNull(string key, ParameterDirectionWrap direction, DbType dbType)
        {
            AddWithValue(key, DBNull.Value, direction, null, dbType);
        }

        public void AddWithValue(string key, object value)
        {
            // Need to prevent adding a duplicate key
            int index = pm.IndexOf(key);
            if (index == -1)
            {
                pm.AddWithValue(key, value);
            }
            else
            {
                pm[index].Value = value;
            }
        }

        public void AddWithValue(string key, object value, ParameterDirectionWrap direction)
        {
            AddWithValue(key, value, direction, null, null);
        }

        public void AddWithValue(string key, object value, ParameterDirectionWrap direction, int? size)
        {
            AddWithValue(key, value, direction, size, null);
        }

        #endregion

        public void AddWithValue(string key, object value, ParameterDirectionWrap direction, int? size, DbType? dbType)
        {
            var sp = new SqlParameter(key, value);

            if (dbType.HasValue)
            {
                sp.DbType = dbType.Value;
            }

            ParameterDirection pDir;

            switch (direction)
            {
                case ParameterDirectionWrap.Input:
                    pDir = ParameterDirection.Input;
                    break;
                case ParameterDirectionWrap.InputOutput:
                    pDir = ParameterDirection.InputOutput;
                    break;
                case ParameterDirectionWrap.Output:
                    pDir = ParameterDirection.Output;
                    break;
                case ParameterDirectionWrap.ReturnValue:
                    pDir = ParameterDirection.ReturnValue;
                    break;
                default:
                    pDir = ParameterDirection.Input;
                    break;
            }

            // Need to prevent adding a duplicate key
            int index = pm.IndexOf(key);
            if (index == -1)
            {
                sp.Direction = pDir;

                if (size.HasValue)
                    sp.Size = size.Value;

                pm.Add(sp);
            }
            else
            {
                pm[index].Direction = pDir;
                if (size.HasValue)
                {
                    pm[index].Size = size.Value;
                }
                pm[index].Value = value;
            }
        }
    }
}