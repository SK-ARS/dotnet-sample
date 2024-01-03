using STP.Common.Constants;
using STP.DocumentsAndContents.Interface;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Providers
{
    public class OutBoundProvider:IOutBound
    {

        #region OutBoundProvider Singleton

        private OutBoundProvider()
        {
        }
        public static OutBoundProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly OutBoundProvider instance = new OutBoundProvider();
        }
        #endregion

        public long SaveInboxItems(int NotificationID, long documentId, int OrganisationID, string esDAlRefNo, string userSchema = UserSchema.Portal, int icaStatus = 277001, bool ImminentMovestatus = false)
        {
            return OutBoundDocumentDOA.SaveInboxItems(NotificationID, documentId, OrganisationID, esDAlRefNo, userSchema, icaStatus,ImminentMovestatus);
        }

        #region Commented Code By Mahzeer on 12/07/2023
        /*
        public bool GenerateMovementAction(UserInfo UserSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem,long projectId=0,int revisionNo=0,int versionNo=0, int movFlagVar = 0, NotificationContacts objContact = null)
        {
            return OutBoundDocumentDOA.GenerateMovementAction(UserSessionValue, EsdalRef, movActionItem, projectId, revisionNo, versionNo, movFlagVar, objContact);
        }
        public long SaveMovementActionForDistTrans(MovementActionIdentifiers movactiontype, string MovDescrp,long projectId,int revisionNo,int versionNo, string userSchema)
        {
            return OutBoundDocumentDOA.SaveMovementActionForDistTrans(movactiontype, MovDescrp,projectId,revisionNo,versionNo, userSchema);
        }*/
        #endregion
    }
}