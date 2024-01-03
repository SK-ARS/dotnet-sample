#region

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

namespace STP.DataAccess.Exceptions
{
    /// <summary>
    ///     Encapsulates a basic database-centric exception.
    /// </summary>
    [Serializable]
    public class SafeProcedureDataException : Exception
    {
        #region DBInstanceName

        protected string instanceName;

        /// <summary>
        ///     Database instance name
        /// </summary>
        public string DBInstanceName
        {
            get { return instanceName; }
            set { instanceName = value; }
        }

        #endregion

        #region ProcedureName

        protected string procedureName;

        /// <summary>
        ///     Stored procedure or statement name
        /// </summary>
        public string ProcedureName
        {
            get { return procedureName; }
            set { procedureName = value; }
        }

        #endregion

        #region ShortMessage

        protected string shortMessage;

        /// <summary>
        ///     Short message used for hashing and grouping of database exceptions
        /// </summary>
        public string ShortMessage
        {
            get { return shortMessage; }
            set { shortMessage = value; }
        }

        #endregion

        #region InnerException

        protected Exception innerException;

        public new Exception InnerException
        {
            get { return innerException; }
            set { innerException = value; }
        }

        #endregion

        internal SafeProcedureDataException(string message)
            : base(message)
        {
        }

        internal SafeProcedureDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #region Exception serialization support

        protected SafeProcedureDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // get the custom property out of the serialization stream and set the object's properties
            instanceName = info.GetString("DBInstanceName");
            procedureName = info.GetString("ProcedureName");
            shortMessage = info.GetString("ShortMessage");
            innerException = (Exception) info.GetValue("_InnerException", typeof (Exception));
        }

        /// <summary>
        ///     Serializes exception
        /// </summary>
        /// <param name="info"> </param>
        /// <param name="context"> </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // add the custom property into the serialization stream
            info.AddValue("DBInstanceName", instanceName);
            info.AddValue("ProcedureName", procedureName);
            info.AddValue("ShortMessage", shortMessage);
            info.AddValue("_InnerException", innerException);

            // call the base exception class to ensure proper serialization
            base.GetObjectData(info, context);
        }

        #endregion
    };
}