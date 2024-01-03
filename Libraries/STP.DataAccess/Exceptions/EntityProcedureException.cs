#region

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using STP.Common.Enums;

#endregion

namespace STP.DataAccess.Exceptions
{
    /// <summary>
    ///     Thrown by Entity Procedure class
    /// </summary>
    [Serializable]
    public class EntityProcedureException : DatabaseExecutionException
    {
        #region Instance Type

        private Type instanceType;

        /// <summary>
        ///     Type of generic instance used in EntityProcedure call
        /// </summary>
        public Type InstanceType
        {
            get { return instanceType; }
            set { instanceType = value; }
        }

        #endregion

        /// <summary>
        ///     Creates instance of <see cref="EntityProcedureException" />
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="procedureName"></param>
        /// <param name="innerException"></param>
        public EntityProcedureException(DatabaseSource databaseSource, string procedureName, Exception innerException)
            : base(databaseSource, procedureName, innerException)
        {
        }

        /// <summary>
        ///     Creates instance of <see cref="EntityProcedureException" />
        /// </summary>
        /// <param name="databaseSource"> </param>
        /// <param name="procedureName"></param>
        /// <param name="instanceType"></param>
        /// <param name="innerException"></param>
        public EntityProcedureException(DatabaseSource databaseSource, string procedureName, Type instanceType,
            Exception innerException)
            : base(databaseSource, procedureName, "Instance type: " + instanceType, innerException)
        {
            this.instanceType = instanceType;
        }

        #region Exception serialization support

        /// <summary>
        ///     Deserializes exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected EntityProcedureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // get the custom property out of the serialization stream and set the object's properties
            instanceType = (Type) info.GetValue("InstanceType", typeof (Type));
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
            info.AddValue("InstanceType", instanceType);

            // call the base exception class to ensure proper serialization
            base.GetObjectData(info, context);
        }

        #endregion
    }
}