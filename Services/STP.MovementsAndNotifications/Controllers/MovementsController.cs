using STP.Common.Constants;
using STP.Common.Logger;
using STP.MovementsAndNotifications.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.LoggingAndReporting;
using STP.ServiceAccess.SecurityAndUsers;
using STP.Domain.MovementsAndNotifications.HaulierMovementsAPI;
using STP.Domain.MovementsAndNotifications.SOAPoliceMovementsAPI;
using STP.Domain.MovementsAndNotifications.SORTMovementsAPI;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;

namespace STP.MovementsAndNotifications.Controllers
{
    public class MovementsController : ApiController
    {
        private readonly IAuthenticationService authenticationService;
        public MovementsController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        #region GetInboxMovements
        [HttpPost]
        [Route("Movements/GetInboxMovements")]
        public IHttpActionResult GetInboxMovements(GetInboxMovementsParams inboxMovementsParams)
        {
            List<MovementsInbox> movementList = new List<MovementsInbox>();
            try
            {
                movementList = MovementInbox.Instance.GetInboxMovements(inboxMovementsParams);
                return Content(HttpStatusCode.OK, movementList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetInboxMovements,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSORTMovementRelatedToStructList
        [HttpGet]
        [Route("Movements/GetSORTMovementRelatedToStructList")]
        public IHttpActionResult GetSORTMovementRelatedToStructList(int organisationId, int pageNumber, int pageSize, long structID)
        {
            List<SORTMovementList> SORTMovementList = new List<SORTMovementList>();
            try
            {
                SORTMovementList = MovementProvider.Instance.GetSORTMovementRelatedToStructList(organisationId, pageNumber, pageSize, structID);
                if (SORTMovementList.Count == 0)
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
                else
                    return Ok(SORTMovementList);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSORTMovementList
        [HttpPost]
        [Route("Movements/GetSORTMovementList")]
        public IHttpActionResult GetSORTMovementList(SORTMovementListParams SORTMovementListParams)
        {
            List<SORTMovementList> SORTMovementList = new List<SORTMovementList>();
            try
            {
                SORTMovementList = MovementProvider.Instance.GetSortMovementsList(SORTMovementListParams.PageNum, SORTMovementListParams.PageSize, SORTMovementListParams.SORTMovementFilter, SORTMovementListParams.SORTAdvMovementFilter, SORTMovementListParams.IsCreCandidateOrCreAppl,SORTMovementListParams.SortObjMapFilter, SORTMovementListParams.PlanMovement, SORTMovementListParams.sortOrder, SORTMovementListParams.sortType);
                return Content(HttpStatusCode.OK, SORTMovementList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetSORTMovementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Save Affected Movement Details
        [HttpPost]
        [Route("Movements/SaveAffectedMovementDetails")]
        public IHttpActionResult SaveAffectedMovementDetails(AffectedStructConstrParam affectedParam)
        {
            try
            {
                bool result = MovementProvider.Instance.SaveAffectedMovementDetails(affectedParam);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/SaveAffectedMovementDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Contact Id
        [HttpGet]
        [Route("Movements/GetContactDetails")]
        public IHttpActionResult GetContactDetails(int userId)
        {
            int contactId;
            try
            {
                contactId = MovementInbox.Instance.GetContactDetails(userId);
                return Ok(contactId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetContactDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Edit inbox items open status
        [HttpGet]
        [Route("Movements/EditInboxItemOpenStatus")]
        public IHttpActionResult EditInboxItemOpenStatus(long inboxId, long organisationId)
        {
            int editFlag = 0;
            try
            {
                editFlag = MovementProvider.Instance.EditInboxItemOpenStatus(inboxId, organisationId);
                return Ok(editFlag);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/EditInboxItemOpenStatus, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Auhorize movement general proposed
        [HttpPost]
        [Route("Movements/GetAuthorizeMovementGeneralProposed")]
        public IHttpActionResult GetAuthorizeMovementGeneralProposed(MovementModelParams objMovementModelParams)
        {
            MovementModel objMovement = new MovementModel();
            try
            {
                objMovement = MovementProvider.Instance.GetAuthorizeMovementGeneralProposed(objMovementModelParams);
                return Ok(objMovement);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetAuthorizeMovementGeneralProposed, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get order no by esdal ref
        [HttpGet]
        [Route("Movements/GetSpecialOrderNo")]
        public IHttpActionResult GetSpecialOrderNo(string ESDALreferenceNo)
        {
            string orderNo = string.Empty;
            try
            {
                orderNo = MovementInbox.Instance.GetSpecialOrderNo(ESDALreferenceNo);
                return Ok(orderNo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetSpecialOrderNo, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get vehicle details
        [HttpGet]
        [Route("Movements/GetVehiclesList")]
        public IHttpActionResult GetVehiclesList(string mnemonic, string ESDALreferenceNo, string version, long notificationId, int isSimplified)
        {
            try
            {
                List<VehicleConfigration> vehicleConfigList   = MovementProvider.Instance.GetVehiclesList(mnemonic, ESDALreferenceNo, version, notificationId, isSimplified);
                return Ok(vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetVehiclesList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get NH And HaulierContactId By Name
        [HttpPost]
        [Route("Movements/GetHAAndHaulierContactIdByName")]
        public IHttpActionResult GetHAAndHaulierContactIdByName(MovementModel movement)
        {
            MovementModel objMovement = new MovementModel();
            try
            {
                objMovement = MovementProvider.Instance.GetHAAndHaulierContactIdByName(movement);
                return Ok(objMovement);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetHAAndHaulierContactIdByName, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get document id based on esdalref and contactid
        [HttpGet]
        [Route("Movements/GetDocumentID")]
        public IHttpActionResult GetDocumentID(string ESDALReferenceNo, long organisationID)
        {
            long documentId = 0;
            try
            {
                documentId = MovementInbox.Instance.GetDocumentID(ESDALReferenceNo, organisationID);
                return Ok(documentId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetDocumentID, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get collaboration notes list
        [HttpGet]
        [Route("Movements/GetCollaborationNotes")]
        public IHttpActionResult GetCollaborationNotes(long documentId, long organisationId)
        {
            List<CollaborationNotes> collaborationNotesList = new List<CollaborationNotes>();
            try
            {
                collaborationNotesList = MovementProvider.Instance.GetCollaborationNotes(documentId, organisationId);
                return Ok(collaborationNotesList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetCollaborationNotes, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Authorized movement general detail
        [HttpPost]
        [Route("Movements/GetAuthorizeMovementGeneral")]
        public IHttpActionResult GetAuthorizeMovementGeneral(MovementModelParams objMovementModelParams)
        {
            MovementModel objMovement = new MovementModel();
            try
            {
                objMovement = MovementProvider.Instance.GetAuthorizeMovementGeneral(objMovementModelParams.Notificationid, objMovementModelParams.InboxId, objMovementModelParams.ContactId, objMovementModelParams.ESDALReference, objMovementModelParams.OrganisationId);
                return Ok(objMovement);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetAuthorizeMovementGeneral, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get haulier contact id from haulier name
        [HttpPost]
        [Route("Movements/GetHaulierContactId")]
        public IHttpActionResult GetHaulierContactId(MovementModel movement)
        {
            MovementModel objMovement = new MovementModel();
            try
            {
                objMovement = MovementProvider.Instance.GetHaulierContactId(movement);
                return Ok(objMovement);                
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetHaulierContactId, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Update status of Inbox item
        [HttpPost]
        [Route("Movements/UpdateInboxItemStatus")]
        public IHttpActionResult UpdateInboxItemStatus(InboxItemStatusParams objInboxItemStatusParams)
        {
            long transmissionId = 0;
            try
            {
                transmissionId = MovementProvider.Instance.UpdateInboxItemStatus(objInboxItemStatusParams);
                return Ok(transmissionId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/UpdateInboxItemStatus, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Special order by Notification code
        [HttpGet]
        [Route("Movements/GetSpecialOrders")]
        public IHttpActionResult GetSpecialOrders(string notificationCode)
        {
            List<SpecialOrder> specialOrderList = new List<SpecialOrder>();
            try
            {
                specialOrderList = MovementProvider.Instance.GetSpecialOrders(notificationCode);
                return Ok(specialOrderList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetSpecialOrders, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Notification details by code
        [HttpGet]
        [Route("Movements/GetNotificationDetailsByCode")]
        public IHttpActionResult GetNotificationDetailsByCode(string notificationCode, string route, long organisationId, long projectId)
        {
            List<RelatedCommunication> relatedCommunicationList = new List<RelatedCommunication>();
            try
            {
                relatedCommunicationList = MovementProvider.Instance.GetNotificationDetailsByCode(notificationCode, route, organisationId, projectId);
                return Ok(relatedCommunicationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetNotificationDetailsByCode, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get VR1 list
        [HttpGet]
        [Route("Movements/GetVR1s")]
        public IHttpActionResult GetVR1s(string VR1_NUMBER)
        {
            List<VR1> VR1List = new List<VR1>();
            try
            {
                VR1List = MovementProvider.Instance.GetVR1s(VR1_NUMBER);
                    return Ok(VR1List);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetVR1s, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Get NEN VehicleList
        [HttpGet]
        [Route("Movements/GetNENVehicleList")]
        public IHttpActionResult GetNENVehicleList(long NENId, long inboxId, long organisationId)
        {
            List<VehicleConfigration> VehicleConfigrationList = new List<VehicleConfigration>();
            try
            {
                VehicleConfigrationList = MovementProvider.Instance.GetNENVehicleList(NENId, inboxId, organisationId);
                return Ok(VehicleConfigrationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetNENVehicleList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Content Ref No
        [HttpGet]
        [Route("Movements/GetContentReferenceNo")]
        public IHttpActionResult GetContentReferenceNo(int notificationNo)
        {
            string contentReferenceNo = "0";
            try
            {
                contentReferenceNo = QuickLinksProvider.Instance.GetContentReferenceNo(notificationNo);
                return Ok(contentReferenceNo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetContentReferenceNo, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Insert Quick Links SOA
        [HttpGet]
        [Route("Movements/InsertQuickLinkSOA")]
        public IHttpActionResult InsertQuickLinkSOA(int organisationId, int inboxId, int userId)
        {
            int linkNo = 0;
            try
            {
                linkNo = QuickLinksProvider.Instance.InsertQuickLinkSOA(organisationId, inboxId, userId);
                return Ok(linkNo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/InsertQuickLinkSOA, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Save Notification AuditLog
        [HttpPost]
        [Route("Movements/SaveNotificationAuditLog")]
        public IHttpActionResult SaveNotificationAuditLog(AuditLogIdentifiersParams objAuditLogIdentifiersParams)
        {
            long result = 0;
            try
            {
                result = NENNotificationProvider.Instance.SaveNotificationAuditLog(objAuditLogIdentifiersParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/SaveNotificationAuditLog, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Delegation Arrangement List
        [HttpGet]
        [Route("Movements/GetArrangementList")]
        public IHttpActionResult GetArrangementList(int organisationId)
        {
            List<DelegArrangeNameList> objDelegArrangeNameList = new List<DelegArrangeNameList>();
            try
            {
                objDelegArrangeNameList = MovementInbox.Instance.GetArrangementList(organisationId);
                return Ok(objDelegArrangeNameList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetArrangementList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get QuickLinks SOA List
        [HttpGet]
        [Route("Movements/GetQuickLinksSOAList")]
        public IHttpActionResult GetQuickLinksSOAList(int userId)
        {
            List<QuickLinksSOA> objQuickLinkSOAList = new List<QuickLinksSOA>();
            try
            {
                objQuickLinkSOAList = QuickLinksProvider.Instance.GetQuickLinksSOAList(userId);
                return Ok(objQuickLinkSOAList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetQuickLinksSOAList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Haulier Movement List
        [HttpPost]
        [Route("Movements/GetMovementsList")]
        public IHttpActionResult GetMovementsList(HaulierMovementsListParams objHaulierMovementsListParams)
        {
            
            List<MovementsList> haulierMovementList = new List<MovementsList>();
            try
            {
                haulierMovementList = MovementInbox.Instance.GetMovementsList(objHaulierMovementsListParams);
                return Content(HttpStatusCode.OK, haulierMovementList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetMovementsList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Folder List
        [HttpGet]
        [Route("Movements/GetFolderList")]
        public IHttpActionResult GetFolderList(long organisationId, string userSchema)
        {
            List<FolderNameList> folderNameLists = new List<FolderNameList>();
            try
            {
                folderNameLists = MovementInbox.Instance.GetFolderList(organisationId, userSchema);
                return Ok(folderNameLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetFolderList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region PrintReport
        [HttpGet]
        [Route("Movements/PrintReport")]
        public IHttpActionResult PrintReport(long notificationId)
        {
            string recipientXMLInformation = string.Empty;
            try
            {
                recipientXMLInformation = MovementProvider.Instance.PrintReport(notificationId);
                return Content(HttpStatusCode.OK, recipientXMLInformation);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/PrintReport, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region View Movement Document
        [HttpGet]
        [Route("Movements/ViewMovementDocument")]
        public IHttpActionResult ViewMovementDocument(long documentId, long organisationId, string userSchema)
        {
            try
            {
                DocumentInfo XmlOutboundDoc = MovementProvider.Instance.ViewMovementDocument(documentId, organisationId, userSchema);
                return Content(HttpStatusCode.OK, XmlOutboundDoc);
                            }
            catch (Exception ex)

            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/ViewMovementDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetCollaborationStatus
        /// <summary>
        /// Getcollabration status
        /// </summary>
        /// <param name="inboxID">INBOX  Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Movements/GetCollaborationStatus")]
        public IHttpActionResult GetCollaborationStatus(long inboxId)
        {           
            try
            {
                MovementModel movementModel = MovementProvider.Instance.GetCollaborationStatus(inboxId);
                return Ok(movementModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetCollaborationStatus, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ManageCollaborationStatus
        /// <summary>
        /// ManageCollaborationStatus
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        [HttpPost]
        [Route("Movements/ManageCollaborationStatus")]
        public IHttpActionResult ManageCollaborationStatus(MovementModel movement)
        {
            try
            {
                bool result = MovementProvider.Instance.ManageCollaborationStatus(movement);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/ManageCollaborationStatus, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// Get XML by Notificationcode
        /// </summary>
        /// <param name="notificationCode">Notificationcode</param>
        /// <param name="organisationId">Organisation Id</param>
        /// <returns>xml</returns>
        #endregion

        #region PrintAgreedReport
        [HttpGet]
        [Route("Movements/PrintAgreedReport")]
        public IHttpActionResult PrintAgreedReport(string notificationCode, long organisationId)
        {
            try
            {
                string result = MovementProvider.Instance.PrintAgreedReport(notificationCode, organisationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/PrintAgreedReport, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetProposalOutboundDocsXML
        [HttpGet]
        [Route("Movements/GetProposalOutboundDocsXML")]
        public IHttpActionResult GetProposalOutboundDocsXML(long documentNumber)
        {
            try
            {
                string result = MovementProvider.Instance.GetProposalOutboundDocsXML(documentNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetProposalOutboundDocsXML, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetInboxItemDetails
        /// <summary>
        /// Get Inbox item details
        /// </summary>
        /// <param name="esdalRefNumber">Esdal reference number</param>
        /// <param name="organisationId">Organisation Id</param>
        /// <returns>xml</returns>
        [HttpGet]
        [Route("Movements/GetInboxItemDetails")]
        public IHttpActionResult GetInboxItemDetails(string esdalRefNumber, long organisationId)
        {
            try
            {
                MovementModel inboxDetails = MovementProvider.Instance.GetInboxItemDetails(esdalRefNumber, organisationId);
                return Ok(inboxDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetInboxItemDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ManageNotesOnEscort
        /// <summary>
        /// Manage Notes on Escort
        /// </summary>
        /// <param name="movement">MovementModel model</param>
        /// <returns>Update notes on escort</returns>
        [HttpPost]
        [Route("Movements/ManageNotesOnEscort")]
        public IHttpActionResult ManageNotesOnEscort(MovementModel movement)
        {
            try
            {
                bool result = MovementProvider.Instance.ManageNotesOnEscort(movement);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/ManageNotesOnEscort, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetHaulierUserId
        [HttpGet]
        [Route("Movements/GetHaulierUserId")]
        public IHttpActionResult GetHaulierUserId(string firstName, string surName, int organisationId)
        {
            try
            {
                string result = string.Empty;
                result = MovementProvider.Instance.GetHaulierUserId(firstName, firstName, organisationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetHaulierUserId, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ManageInternalNotes
        /// <summary>
        /// Update internal notes
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        [HttpPost]
        [Route("Movements/ManageInternalNotes")]
        public IHttpActionResult ManageInternalNotes(MovementModel movement)
        {
            try
            {
                bool result = MovementProvider.Instance.ManageInternalNotes(movement);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/ManageInternalNotes, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetContactDetailsForDefault
        /// <summary>
        /// Get the default contact details of a organisation
        /// </summary>
        /// <param name="OrganisationId">Organisation Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Movements/GetContactDetailsForDefault")]
        public IHttpActionResult GetContactDetailsForDefault(int organisationId)
        {
            try
            {
                int result = MovementProvider.Instance.GetContactDetailsForDefault(organisationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetContactDetailsForDefault, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region CopyMovementSortToPortal   
        /// <summary>
        /// Copy movement from sort to portal.
        /// </summary>
        /// <param name="copyMovementSortToPortalInsertParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Movements/CopyMovementSortToPortal")]
        public IHttpActionResult CopyMovementSortToPortal(CopyMovementSortToPortalInsertParams copyMovementSortToPortalInsertParams)
        {
            try
            {
                long projectStatus = MovementProvider.Instance.CopyMovementSortToPortal(copyMovementSortToPortalInsertParams.MovementCopyDetail, copyMovementSortToPortalInsertParams.MovementCloneStatus, copyMovementSortToPortalInsertParams.VersionID, copyMovementSortToPortalInsertParams.EsdalReference, copyMovementSortToPortalInsertParams.HAContactBytes, copyMovementSortToPortalInsertParams.OrganizationID, copyMovementSortToPortalInsertParams.ModelUserSchema);
                return Ok(projectStatus);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SortSideRetransmitApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Haulier Movement List For RouteImport
        [HttpPost]
        [Route("Movements/GetPlanMovementList")]
        public IHttpActionResult GetPlanMovementList(HaulierMovementsListParams objHaulierMovementsListParams)
        {
            try
            {
                List<MovementsList> haulierMovementList = MovementInbox.Instance.GetPlanMovementList(objHaulierMovementsListParams);
                return Content(HttpStatusCode.OK, haulierMovementList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetPlanMovementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetStructLinkId
        [HttpPost]
        [Route("Movements/GetStructLinkId")]
        public IHttpActionResult GetStructLinkId(SortMapFilter objSortMapFilterParams)
        {
            List<MapStructLink> StructLinkIdList = new List<MapStructLink>();
            try
            {
                StructLinkIdList = MovementProvider.Instance.GetStructLinkId(objSortMapFilterParams);
                return Content(HttpStatusCode.OK, StructLinkIdList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetStructLinkId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetContactedPartiesDetail
        [HttpGet]
        [Route("Movements/GetContactedPartiesDetail")]
        public IHttpActionResult GetContactedPartiesDetail(long analysisId)
        {
            MovementContactModel contactDetail = new MovementContactModel();
            try
            {
                contactDetail = MovementProvider.Instance.GetContactedPartiesDetail(analysisId);
                return Content(HttpStatusCode.OK, contactDetail);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetContactedPartiesDetail,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region List Movement Api-External

        #region List Haulier Movement
        [HttpGet]
        [Route("Movements/ListHaulierMovements")]
        public IHttpActionResult ListHaulierMovements(int MovementType = 1, int IncludeHistoricData = 0, string AuthenticationKey = null, int PageNumber = 0, int PageSize = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey);

                if (authentication.OrganisationId != 0)
                {
                    PageNumber = PageNumber > 0 ? PageNumber : 1;
                    PageSize = PageSize > 0 ? PageSize : 20;
                    HaulierMovementDetails haulierMovements = MovementExternalApiProvider.Instance.GetHaulierMovementList(authentication.OrganisationId, IncludeHistoricData, MovementType, PageNumber, PageSize);

                    if (haulierMovements.TotalRecords > 0)
                        return Content(HttpStatusCode.OK, haulierMovements);
                    else
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - MovementsAPI/GetHauliarMovementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SOA/Police Movement List
        [HttpGet]
        [Route("Movements/ListSOAPoliceMovements")]
        public IHttpActionResult ListSOAPoliceMovements(int IncludeHistoricData = 0, string AuthenticationKey = null, int PageNumber = 0, int PageSize = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey);
                if (authentication.OrganisationId != 0)
                {
                    PageNumber = PageNumber > 0 ? PageNumber : 1;
                    PageSize = PageSize > 0 ? PageSize : 20;
                    SoaPoliceDetails soapoliceMovements = MovementExternalApiProvider.Instance.GetSOAPoliceMovementList(authentication.OrganisationId, IncludeHistoricData, PageNumber, PageSize, authentication.UserTypeId == UserType.PoliceALO);

                    if (soapoliceMovements.TotalRecords > 0)
                        return Content(HttpStatusCode.OK, soapoliceMovements);
                    else
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetSOAPoliceMovementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Sort Movement List
        [HttpGet]
        [Route("Movements/ListSORTMovements")]
        public IHttpActionResult ListSORTMovements(int IncludeHistoricData = 0, string AuthenticationKey = null, int PageNumber = 0, int PageSize = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey); 
                if (authentication.OrganisationId != 0)
                {
                    PageNumber = PageNumber != 0 ? PageNumber : 1;
                    PageSize = PageSize != 0 ? PageSize : 20;

                    SORTMovementDetails sortMovements = MovementExternalApiProvider.Instance.GetSORTMovementList(authentication.OrganisationId, IncludeHistoricData, PageNumber, PageSize);

                    if (sortMovements.TotalRecords > 0)
                        return Content(HttpStatusCode.OK, sortMovements);
                    else
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetSORTMovementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #endregion

        #region GetHomePageMovements
        [HttpPost]
        [Route("Movements/GetHomePageMovements")]
        public IHttpActionResult GetHomePageMovements(GetInboxMovementsParams inboxMovementsParams)
        {
            List<MovementsInbox> movementList = new List<MovementsInbox>();
            try
            {
                movementList = MovementInbox.Instance.GetHomePageMovements(inboxMovementsParams);
                return Content(HttpStatusCode.OK, movementList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetHomePageMovements,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ReturnRouteAutoAssignVehicle
        [HttpGet]
        [Route("Movements/ReturnRouteAutoAssignVehicle")]
        public IHttpActionResult ReturnRouteAutoAssignVehicle(long movementId, int flag, long notificationId, long organisationId)
        {
            try
            {
                int iCount = MovementProvider.Instance.ReturnRouteAutoAssignVehicle(movementId, flag, notificationId, organisationId);
                return Content(HttpStatusCode.OK, iCount);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/ReturnRouteAutoAssignVehicle,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region CalcualteMovementDate
        [HttpGet]
        [Route("Movements/CalcualteMovementDate")]
        public IHttpActionResult CalcualteMovementDate(int noticePeriod, int vehicleClass, string userSchema)
        {
            try
            {
                DateTime dateTime = MovementProvider.Instance.CalcualteMovementDate(noticePeriod, vehicleClass, userSchema);
                return Content(HttpStatusCode.OK, dateTime);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/CalcualteMovementDate,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetNENAffectedContactDetails
        [HttpGet]
        [Route("Movements/GetNENAffectedContactDetails")]
        public IHttpActionResult GetNENAffectedContactDetails(string esdalRefNumber, string userSchema)
        {
            try
            {
                List<ContactModel> contacts = MovementProvider.Instance.GetNENAffectedContactDetails(esdalRefNumber, userSchema);
                return Content(HttpStatusCode.OK, contacts);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetNENAffectedContactDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}