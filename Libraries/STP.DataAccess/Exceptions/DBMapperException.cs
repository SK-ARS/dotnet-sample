#region

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

namespace STP.DataAccess.Exceptions
{
    /// <summary>
    ///     Thrown by Mapper class
    /// </summary>
    [Serializable]
    public class DBMapperException : SafeProcedureDataException
    {
        #region Mapper Connection String

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        #endregion

        public DBMapperException(string instanceName, string connectionString, string statementName,
            Exception innerException)
            :
                base(
                "Mapper Exception Against Database: " + instanceName + " \n "
                + "\n With statement: " + statementName + " \n "
                + "\n With inner exception message: " + innerException.Message,
                innerException
                )
        {
            this.instanceName = instanceName;
            procedureName = statementName;
            this.connectionString = connectionString;
            this.innerException = innerException;
            shortMessage =
                "Database Exception Against Database: " + instanceName +
                "\n With inner exception message: " + innerException.Message;
        }

        #region Exception serialization support

        protected DBMapperException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // get the custom property out of the serialization stream and set the object's properties
            connectionString = info.GetString("ConnectionString");
        }

        /// <summary>
        ///     Serializes exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // add the custom property into the serialization stream
            info.AddValue("ConnectionString", connectionString);

            // call the base exception class to ensure proper serialization
            base.GetObjectData(info, context);
        }

        #endregion
    }
}