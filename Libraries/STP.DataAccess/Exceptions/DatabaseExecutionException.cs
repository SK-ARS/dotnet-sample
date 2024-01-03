#region

using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Security.Permissions;
using STP.Common.Enums;

#endregion

namespace STP.DataAccess.Exceptions
{
    /// <summary>
    ///     Thrown by Low level database access classes (Procedure class)
    /// </summary>
    [Serializable]
    public class DatabaseExecutionException : SafeProcedureDataException
    {
        #region Database

        //protected Database database;
        protected DatabaseSource databaseSource;

        ///// <summary>
        /////   Affected database
        ///// </summary>
        //public Database Database
        //{
        //    get { return database; }
        //    set { database = value; }
        //}

        /// <summary>
        ///     Affected database
        /// </summary>
        public DatabaseSource DatabaseSource
        {
            get { return databaseSource; }
            set { databaseSource = value; }
        }

        #endregion

        #region Command

        private SqlCommand command;
/*
        private DatabaseSource databaseSource_2;
*/
/*
        private string p;
*/

        public SqlCommand Command
        {
            get { return command; }
            set { command = value; }
        }

        #endregion

        /*
        public DatabaseExecutionException(Database database, string procedureName, Exception innerException)
            :
                base(
                "Database Exception Against Database: " + database.InstanceName + " \n "
                + "\n With procedure: " + procedureName + " \n "
                + "\n With inner exception message: " + innerException.Message,
                innerException
                )
        {
            this.database = database;
            this.procedureName = procedureName;
            this.innerException = innerException;
            instanceName = database.InstanceName;
            shortMessage =
                "Database Exception Against Database: " + database.InstanceName +
                "\n With inner exception message: " + innerException.Message;
        }

        public DatabaseExecutionException(Database database, string procedureName, SqlCommand command,
                                          Exception innerException)
            :
                base(
                "Database Exception Against Database: " + database.InstanceName + " \n "
                + "\n With procedure: " + procedureName + " \n "
                + "\n With parameters: " + GetParameterString(command)
                + "\n With inner exception message: " + innerException.Message,
                innerException
                )
        {
            this.database = database;
            this.procedureName = procedureName;
            this.command = command;
            this.innerException = innerException;
            instanceName = database.InstanceName;
            shortMessage =
                "Database Exception Against Database: " + database.InstanceName +
                "\n With inner exception message: " + innerException.Message;
        }

        public DatabaseExecutionException(Database database, string procedureName, string contextInfo,
                                          Exception innerException)
            :
                base(
                "Database Exception Against Database: " + database.InstanceName + " \n "
                + "\n With procedure: " + procedureName + " \n "
                + "\n With context info: " + contextInfo + " \n "
                + "\n With inner exception message: " + innerException.Message,
                innerException
                )
        {
            this.database = database;
            this.procedureName = procedureName;
            this.innerException = innerException;
            instanceName = database.InstanceName;
            shortMessage =
                "Database Exception Against Database: " + database.InstanceName +
                "\n With inner exception message: " + innerException.Message;
        }
        */

        public DatabaseExecutionException(DatabaseSource databaseSource, string procedureName, Exception innerException)
            :
                base(
                string.Format(
                    "Database Exception Against Database: {0}  \n \n With procedure: {1}  \n\n \n With inner exception message: {2}::{3}",
                    databaseSource, procedureName, innerException.Message, innerException))
        {
            this.procedureName = procedureName;
            this.innerException = innerException;
            instanceName = databaseSource.ToString();
            shortMessage =
                "Database Exception Against Database: " + instanceName +
                "\n With inner exception message: " + innerException.Message;
        }

        public DatabaseExecutionException(DatabaseSource databaseSource, string procedureName, string contextInfo,
            Exception innerException)
            : base(
                string.Format(
                    "Database Exception Against Database: {0}  \n \n With procedure: {1}  \n\n With context info:  {2}\n With inner exception message: {3}::{4}",
                    databaseSource, procedureName, contextInfo, innerException.Message, innerException))
        {
            this.databaseSource = databaseSource;
            this.procedureName = procedureName;
            this.innerException = innerException;
            instanceName = databaseSource.ToString();
            shortMessage = string.Format(
                "Database Exception Against Database: {0} \n With inner exception message: {1}", databaseSource,
                innerException.Message);
        }

        #region Exception serialization support

        protected DatabaseExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // get the custom property out of the serialization stream and set the object's properties
            //database = (Database)info.GetValue("Database", typeof(databaseSource));
        }


        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // add the custom property into the serialization stream
            info.AddValue("Database", databaseSource);

            // call the base exception class to ensure proper serialization
            base.GetObjectData(info, context);
        }

        #endregion

        private static string GetParameterString(SqlCommand c)
        {
            string ret = string.Empty;
            try
            {
                if (c == null) return string.Empty;
                foreach (SqlParameter p in c.Parameters)
                {
                    ret += string.Format("{{@{0}}}={{{1}}}", p.ParameterName, p.Value);
                }
            }
            catch (Exception)
            {
            } // Ignore these

            return ret;
        }
    }
}