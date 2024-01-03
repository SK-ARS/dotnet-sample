#region

using STP.Common.Enums;

#endregion

namespace STP.DataAccess.ParameterSet
{
    public class StoredProcedureParameter
    {
        #region Public Properties

        #region Key

        /// <summary>
        ///     This can be null. If null, the position of this argument within the argument list will be used to pass the value.
        /// </summary>
        public string Key { get; set; }

        #endregion

        #region Value

        public object Value { get; set; }

        #endregion

        #region Parameter Direction

        public ParameterDirectionWrap ParameterDirection { get; set; }

        #endregion

        #region Size

        public int? Size { get; set; }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        ///     Create a parameter to be passed by parameter name.
        /// </summary>
        public StoredProcedureParameter(string key, object value, ParameterDirectionWrap parameterDirection, int size)
        {
            Key = key;
            Value = value;
            ParameterDirection = parameterDirection;
            Size = size;
        }

        /// <summary>
        ///     Create a parameter to be passed by parameter name.
        /// </summary>
        public StoredProcedureParameter(string key, object value, ParameterDirectionWrap parameterDirection)
        {
            Key = key;
            Value = value;
            ParameterDirection = parameterDirection;
            Size = null;
        }

        /// <summary>
        ///     Create an parameter to be passed by parameter index.
        /// </summary>
        public StoredProcedureParameter(object value, ParameterDirectionWrap parameterDirection, int size)
        {
            Key = null;
            Value = value;
            ParameterDirection = parameterDirection;
            Size = size;
        }

        /// <summary>
        ///     Create an parameter to be passed by parameter index.
        /// </summary>
        public StoredProcedureParameter(object value, ParameterDirectionWrap parameterDirection)
        {
            Key = null;
            Value = value;
            ParameterDirection = parameterDirection;
            Size = null;
        }

        /// <summary>
        ///     Create an input parameter to be passed by parameter index.
        /// </summary>
        public StoredProcedureParameter(object value)
        {
            Key = null;
            Value = value;
            ParameterDirection = ParameterDirectionWrap.Input;
            Size = null;
        }

        #endregion
    }
}