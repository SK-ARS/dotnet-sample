using System;
using System.Data;
using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.DataAccess.Interface;

namespace STP.DataAccess.ParameterSet
{
    internal class OracleParameterSet : IOracleParameterSet
    {
        private readonly OracleParameterCollection oracleParams;

        internal OracleParameterSet(OracleParameterCollection oracleParameterCollection)
        {
            oracleParams = oracleParameterCollection;
        }

        internal ParameterDirection GetParameterDirection(ParameterDirectionWrap directionWrap)
        {
            ParameterDirection parameterDirection;
            switch (directionWrap)
            {
                case ParameterDirectionWrap.Input:
                    parameterDirection = ParameterDirection.Input;
                    break;
                case ParameterDirectionWrap.InputOutput:
                    parameterDirection = ParameterDirection.InputOutput;
                    break;
                case ParameterDirectionWrap.Output:
                    parameterDirection = ParameterDirection.Output;
                    break;
                case ParameterDirectionWrap.ReturnValue:
                    parameterDirection = ParameterDirection.ReturnValue;
                    break;
                default:
                    parameterDirection = ParameterDirection.Input;
                    break;
            }
            return parameterDirection;
        }

        #region IOracleParameterSet Members

        public void Add(object obj)
        {
            oracleParams.Add((OracleParameter)obj);
        }

        public void AddWithValue(string name, object value)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Direction = GetParameterDirection(direction),
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Direction = GetParameterDirection(direction);
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction,
            int ? size)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Size = (size.HasValue) ? size.Value : 0,
                Direction = GetParameterDirection(direction),
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Size = (size.HasValue) ? size.Value : 0;
                oracleParams[index].Direction = GetParameterDirection(direction);
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction,
            int ? size, string srcColumn)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Size = (size.HasValue) ? size.Value : 0,
                Direction = GetParameterDirection(direction),
                SourceColumn = srcColumn,
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Size = (size.HasValue) ? size.Value : 0;
                oracleParams[index].Direction = GetParameterDirection(direction);
                oracleParams[index].SourceColumn = srcColumn;
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction,
            int ? size, string srcColumn, byte precision)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Size = (size.HasValue) ? size.Value : 0,
                Direction = GetParameterDirection(direction),
                Precision = precision,
                SourceColumn = srcColumn,
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Size = (size.HasValue) ? size.Value : 0;
                oracleParams[index].Direction = GetParameterDirection(direction);
                oracleParams[index].Precision = precision;
                oracleParams[index].SourceColumn = srcColumn;
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction,
            int ? size, string srcColumn, byte precision, byte scale)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Size = (size.HasValue) ? size.Value : 0,
                Direction = GetParameterDirection(direction),
                Precision = precision,
                Scale = scale,
                SourceColumn = srcColumn,
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Size = (size.HasValue) ? size.Value : 0;
                oracleParams[index].Direction = GetParameterDirection(direction);
                oracleParams[index].Precision = precision;
                oracleParams[index].Scale = scale;
                oracleParams[index].SourceColumn = srcColumn;
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction,
            int ? size, string srcColumn, byte precision, byte scale, DataRowVersion version)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Size = (size.HasValue) ? size.Value : 0,
                Direction = GetParameterDirection(direction),
                Precision = precision,
                Scale = scale,
                SourceColumn = srcColumn,
                SourceVersion = version,
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Size = (size.HasValue) ? size.Value : 0;
                oracleParams[index].Direction = GetParameterDirection(direction);
                oracleParams[index].Precision = precision;
                oracleParams[index].Scale = scale;
                oracleParams[index].SourceColumn = srcColumn;
                oracleParams[index].SourceVersion = version;
                oracleParams[index].Value = value;
            }
        }

        public void AddWithValue(string name, object value, OracleDbType dbType, ParameterDirectionWrap direction,
            int ? size, string srcColumn, byte precision, byte scale, DataRowVersion version, bool isNullable)
        {
            var op = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dbType,
                Size = (size.HasValue) ? size.Value : 0,
                Direction = GetParameterDirection(direction),
                IsNullable = isNullable,
                Precision = precision,
                Scale = scale,
                SourceColumn = srcColumn,
                SourceVersion = version,
                Value = value
            };
            oracleParams.Add(op);

            // Need to prevent adding a duplicate key
            int index = oracleParams.IndexOf(name);
            if (index == -1)
            {
                oracleParams.Add(op);
            }
            else
            {
                oracleParams[index].ParameterName = name;
                oracleParams[index].OracleDbType = dbType;
                oracleParams[index].Size = (size.HasValue) ? size.Value : 0;
                oracleParams[index].Direction = GetParameterDirection(direction);
                oracleParams[index].IsNullable = isNullable;
                oracleParams[index].Precision = precision;
                oracleParams[index].Scale = scale;
                oracleParams[index].SourceColumn = srcColumn;
                oracleParams[index].SourceVersion = version;
                oracleParams[index].Value = value;
            }
        }

        public void AddRange(Array paramArray)
        {
            oracleParams.AddRange(paramArray);
        }

       

        #region Gets

        public object GetValue(string key)
        {
            return oracleParams[key].Value;
        }

        public int GetInt32(string key)
        {
            int i = 0;
            var value = oracleParams[key].Value;
            if (value != null) int.TryParse(value.ToString(), out i);
            return i;
        }

        public bool GetBool(string key)
        {
            bool output = false;
            var value = oracleParams[key].Value;
            if (value != null) bool.TryParse(value.ToString(), out output);
            return output;
        }

        public string GetString(string key)
        {
            var value = oracleParams[key].Value;
            if (value != null) return value.ToString();
            return null;
        }

        public byte GetByte(string key)
        {
            byte output = 0;
            var value = oracleParams[key].Value;
            if (value != null) byte.TryParse(value.ToString(), out output);
            return output;
        }

        public decimal GetDecimal(string key)
        {
            decimal output = 0;
            var value = oracleParams[key].Value;
            if (value != null) decimal.TryParse(value.ToString(), out output);
            return output;
        }

        public float GetFloat(string key)
        {
            float output = 0;
            var value = oracleParams[key].Value;
            if (value != null) float.TryParse(value.ToString(), out output);
            return output;
        }

        public long GetLong(string key)
        {
            long output = 0;
            var value = oracleParams[key].Value;
            if (value != null) long.TryParse(value.ToString(), out output);
            return output;
        }

        public DateTime GetDateTime(string key)
        {
            var output = new DateTime();
            var value = oracleParams[key].Value;
            if (value != null) DateTime.TryParse(value.ToString(), out output);
            return output;
        }

        #endregion
        
        
        #endregion

    }
}