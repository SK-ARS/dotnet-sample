using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;

namespace STP.MovementsAndNotifications.Providers
{
    public sealed class MovementProvider : IMovement
    {
        #region MovementProvider Singleton
        private MovementProvider()
        {
        }
        public static MovementProvider Instance
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
            internal static readonly MovementProvider instance = new MovementProvider();
        }
        #endregion
        #region GetSORTMovementRelatedToStructList
        public List<SORTMovementList> GetSORTMovementRelatedToStructList(int organisationId, int pageNumber, int pageSize, long structID)
        {
            return MovementsDAO.GetSORTMovementRelatedToStructList(organisationId, pageNumber,pageSize,structID);
        }
        #endregion
        #region GetSortMovementsList
        public List<SORTMovementList> GetSortMovementsList(int pageNumber, int pageSize, SORTMovementFilter sortMovementFilter, SortAdvancedMovementFilter SORTMovementFilterAdvanced, bool IsCreCandidateOrCreAppl,SortMapFilter SortObjMapFilter, bool planMovement,int sortOrder,int sortType)
        {
            return MovementsDAO.GetListSortMovement(pageNumber, pageSize, sortMovementFilter, SORTMovementFilterAdvanced, IsCreCandidateOrCreAppl, SortObjMapFilter, planMovement, sortOrder,sortType);
        }
        #endregion
        #region Edit inbox items open status
        public int EditInboxItemOpenStatus(long inboxId, long organisationId)
        {
            return MovementsDAO.EditInboxItemOpenStatus(inboxId, organisationId);
        }
        #endregion
        #region Get Auhorize movement general proposed
        public MovementModel GetAuthorizeMovementGeneralProposed(MovementModelParams objMovementModelParams)
        {
            return MovementsDAO.GetAuthorizeMovementGeneralProposed(objMovementModelParams);
        }
        #endregion
        #region Get vehicle details
        public List<VehicleConfigration> GetVehiclesList(string mnemonic, string ESDALReferenceNo, string version, long notificationId, int isSimplified)
        {
            return MovementsDAO.GetVehicleList(mnemonic, ESDALReferenceNo, version, notificationId, isSimplified);
        }
        #endregion
        #region Get NH And HaulierContactId By Name
        public MovementModel GetHAAndHaulierContactIdByName(MovementModel movement)
        {
            return MovementsDAO.GetHAAndHaulierContactIdByName(movement);
        }
        #endregion
        #region Get collaboration notes list
        public List<CollaborationNotes> GetCollaborationNotes(long DocumentId, long OrganisationId)
        {
            return MovementsDAO.GetCollaborationNotes(DocumentId, OrganisationId);
        }
        #endregion
        #region Get Authorized movement general detail
        public MovementModel GetAuthorizeMovementGeneral(long notificationId, long inboxId, long contactId, string ESDALReferenceNo, long organisationId)
        {
            return MovementsDAO.GetAuthorizeMovementGeneral(notificationId, inboxId, contactId, ESDALReferenceNo, organisationId);
        }
        #endregion
        #region Get haulier contact id from haulier name
        public MovementModel GetHaulierContactId(MovementModel movement)
        {
            return MovementsDAO.GetHaulierContactId(movement);
        }
        #endregion
        #region Update status of Inbox item
        public long UpdateInboxItemStatus(InboxItemStatusParams objInboxItemStatusParams)
        {
            return MovementsDAO.UpdateInboxItemStatus(objInboxItemStatusParams);
        }
        #endregion
        #region Get Special order by Notification code
        public List<SpecialOrder> GetSpecialOrders(string notificationCode)
        {
            return MovementsDAO.GetSpecialOrders(notificationCode);
        }

		public bool SaveAffectedMovementDetails(AffectedStructConstrParam affectedParam)
		{
            return MovementsDAO.SaveAffectedMovementDetails(affectedParam);
        }
		#endregion
		#region Get VR1 list
		public List<VR1> GetVR1s(string VR1No)
        {
            return MovementsDAO.GetVR1s(VR1No);
        }
        #endregion
        #region Get Notification details by code
        public List<RelatedCommunication> GetNotificationDetailsByCode(string notificationCode, string route, long organisationId, long projectId)
        {
            return MovementsDAO.GetNotificationDetailsByCode(notificationCode, route, organisationId, projectId);
        }
        #endregion
        #region Get NEN VehicleList
        public List<VehicleConfigration> GetNENVehicleList(long NENId, long inboxId, long organisationId)
        {
            return MovementsDAO.GetNENVehicleList(NENId, inboxId, organisationId);
        }
        #endregion
        #region Get PrintReport
        public string PrintReport(long notificationId)
        {
            return MovementsDAO.GetNotificationOutboundDocsXML(notificationId);
        }
        #endregion
        #region ViewMovementDocument
        public DocumentInfo ViewMovementDocument(long documentId, long organisationId, string userSchema)
        {
            return MovementsDAO.GetOutboundDocsXMLwithUserType(documentId, organisationId, userSchema);
        }
        #endregion
        /// <summary>
        /// Get collaboration notes list
        /// </summary>
        /// <param name="DOCUMENT_ID">Document Id</param>
        /// <returns>Returns collaboration notes list</returns>
        public MovementModel GetCollaborationStatus(long INBOX_ID)
        {
            return MovementsDAO.GetCollaborationStatus(INBOX_ID);
        }
        /// <summary>
        /// Update movement status
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        public bool ManageCollaborationStatus(MovementModel movement)
        {
            return MovementsDAO.ManageCollaborationStatus(movement);
        }
        /// <summary>
        /// Get XML by Notificationcode
        /// </summary>
        /// <param name="Notificationid">Notificationcode</param>
        /// <param name="organisationId">Organisation Id</param>
        /// <returns>xml</returns>
        public string PrintAgreedReport(string Notificationcode, long organisationId)
        {
            return MovementsDAO.GetOutboundDocsXML(Notificationcode, organisationId);
        }

        public string GetProposalOutboundDocsXML(long documentNumber)
        {
            return MovementsDAO.GetProposalOutboundDocsXML(documentNumber);
        }

        /// <summary>
        /// Get Inbox item details
        /// </summary>
        /// <param name="esdalRefNumber">Esdal reference number</param>
        /// <param name="organisationId">Organisation Id</param>
        /// <returns>xml</returns>
        public MovementModel GetInboxItemDetails(string esdalRefNumber, long organisationId)
        {
            return MovementsDAO.GetInboxItemDetails(esdalRefNumber, organisationId);
        }
        /// <summary>
        /// Manage Notes on Escort
        /// </summary>
        /// <param name="movement">MovementModel model</param>
        /// <returns>Update notes on escort</returns>
        public bool ManageNotesOnEscort(MovementModel movement)
        {
            return MovementsDAO.ManageNotesOnEscort(movement);
        }
        public string GetHaulierUserId(string firstName, string surName, int organisationId)
        {
            return MovementsDAO.GetHaulierUserId(firstName, surName, organisationId);
        }
       
        /// <summary>
        /// Update internal notes
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        public bool ManageInternalNotes(MovementModel movement)
        {
            return MovementsDAO.ManageInternalNotes(movement);
        }

        public int GetContactDetailsForDefault(int organisationId)// For RM#4547
        {
            return MovementsDAO.GetContactDetailsForDefault(organisationId);
        }
        public long CopyMovementSortToPortal(MovementCopyDetails movementCopyDetails, int movementCloneStatus, int versionID, string esdalReference, byte[] haContactBytes, int organizationID, string userSchema)
        {
            return MovementsDAO.CopyMovementSortToPortal(movementCopyDetails, movementCloneStatus, versionID, esdalReference, haContactBytes, organizationID, userSchema);
        }
        public List<MapStructLink> GetStructLinkId(SortMapFilter objSortMapFilterParams)
        {
            return MovementsDAO.GetStructLinkId(objSortMapFilterParams);
        }
        #region get affected paries of proposed and agreed movement
        public MovementContactModel GetContactedPartiesDetail(long analysisId)
        {
            return MovementsDAO.GetContactedPartiesDetail(analysisId);
        }
        #endregion

        public int ReturnRouteAutoAssignVehicle(long movementId, int flag, long notificationId, long organisationId)
        {
            return MovementsDAO.ReturnRouteAutoAssignVehicle(movementId, flag, notificationId, organisationId);
        }

        public DateTime CalcualteMovementDate(int noticePeriod,int vehicleClass, string userSchema)
        {
            return MovementsDAO.CalcualteMovementDate(noticePeriod, vehicleClass, userSchema);
        }

        public List<ContactModel> GetNENAffectedContactDetails(string esdalRefNumber, string userSchema)
        {
            return MovementsDAO.GetNENAffectedContactDetails(esdalRefNumber, userSchema);
        }
    }
}