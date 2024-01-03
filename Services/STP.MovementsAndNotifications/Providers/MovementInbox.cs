using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.MovementsAndNotifications.Providers
{
    public sealed class MovementInbox : IMovementInbox
    {
        #region MovementInbox Singleton

        private MovementInbox()
        {
        }
        public static MovementInbox Instance
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
            internal static readonly MovementInbox instance = new MovementInbox();
        }
        #endregion

        #region MovementInbox implementation

        public List<MovementsInbox> GetInboxMovements(GetInboxMovementsParams inboxMovementsParams)
        {
            return MovementsDAO.GetMovementInbox(inboxMovementsParams);
        }

        #endregion

        #region Get Contact Id
        public int GetContactDetails(int UserId)
        {
            return MovementsDAO.GetContactDetails(UserId);
        }
        #endregion

        #region Get OrderNo by esdal ref
        public string GetSpecialOrderNo(string esdalRefNo)
        {
            return MovementsDAO.GetSpecialOrderNo(esdalRefNo);
        }
        #endregion

        #region Get document id based on esdalref and contactid
        public long GetDocumentID(string esdalRefNo, long organisationID)
        {
            return MovementsDAO.GetDocumentID(esdalRefNo, organisationID);
        }
        #endregion

        #region Get Delegation Arrangement List
        public List<DelegArrangeNameList> GetArrangementList(int organisationId)
        {
            return MovementsDAO.GetDelegationList(organisationId);
        }
        #endregion

        #region Get Haulier Movement List
        public List<MovementsList> GetMovementsList(HaulierMovementsListParams objHaulierMovementsListParams)
        {
            return MovementsDAO.GetListMovement(objHaulierMovementsListParams);
        }
        #endregion

        #region Get Haulier Movement List For Plan Movement
        public List<MovementsList> GetPlanMovementList(HaulierMovementsListParams objHaulierMovementsListParams)
        {
            return MovementsDAO.GetPlanMovementList(objHaulierMovementsListParams);
        }
        #endregion


        #region Get Folder List
        public List<FolderNameList> GetFolderList(long organisationId, string userSchema)
        {
            return MovementsDAO.GetListOfFolders(organisationId, userSchema);
        }
        #endregion

        public List<MovementsInbox> GetHomePageMovements(GetInboxMovementsParams inboxMovementsParams)
        {
            return MovementsDAO.GetHomePageMovements(inboxMovementsParams);
        }
    }
}