using STP.Common.Constants;
using STP.Common.Logger;
using STP.DocumentsAndContents.Common;
using STP.DocumentsAndContents.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using STP.Domain.HelpdeskTools;
using STP.Domain.SecurityAndUsers;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.DocumentsAndContents.Communication;
using System.Web;

namespace STP.DocumentsAndContents.Controllers
{
    public class DocumentController : ApiController
    {

        #region Get UserDetails for Notification
        [HttpGet]
        [Route("Document/GetUserDetailsForNotification")]
        public IHttpActionResult GetUserDetailsForNotification(string ESDALReference)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                objUserInfo = DocumentTransmission.Instance.GetUserDetailsForNotification(ESDALReference);
                 return Ok(objUserInfo);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Document/GetUserDetailsForNotification, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get UserDetails for Haulier
        [HttpGet]
        [Route("Document/GetUserDetailsForHaulier")]
        public IHttpActionResult GetUserDetailsForHaulier(string mnemonic, string ESDALReference)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                objUserInfo = DocumentTransmission.Instance.GetUserDetailsForHaulier(mnemonic, ESDALReference);
                return Ok(objUserInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Document/GetUserDetailsForHaul, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get UserName
        [HttpGet]
        [Route("Document/GetUserName")]
        public IHttpActionResult GetUserName(long orgId, long contactId)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                objUserInfo = DocumentTransmission.Instance.GetUserName(orgId, contactId);
                 return Ok(objUserInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Document/GetUserName, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get SOA,Police Details
        [HttpGet]
        [Route("Document/GetSOAPoliceDetails")]
        public IHttpActionResult GetSOAPoliceDetails(string Esdal_ref, int trsmission_id)
        {
            DistributionAlerts objDisbutionAlert = new DistributionAlerts();
            try
            {
                string esdalDecoded = HttpUtility.UrlDecode(Esdal_ref);
                objDisbutionAlert = DocumentTransmission.Instance.GetSOAPoliceDetails(Esdal_ref, trsmission_id);
                return Ok(objDisbutionAlert);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Document/GetSOAPoliceDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Notification Details
        [HttpGet]
        [Route("Document/GetNotifDetails")]
        public IHttpActionResult GetNotifDetails(string ESDALReference, int TransmissionId)
        {
            DistributionAlerts objDisbutionAlert = new DistributionAlerts();
            try
            {
                objDisbutionAlert = DocumentTransmission.Instance.GetNotifDetails(ESDALReference, TransmissionId);
                return Ok(objDisbutionAlert);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Document/GetNotifDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Haulier Details
        [HttpGet]
        [Route("Document/GetHaulierDetails")]
        public IHttpActionResult GetHaulierDetails(string mnemonic, string Esdal_ref, string ver_no)
        {
            DistributionAlerts objDisbutionAlert = new DistributionAlerts();
            try
            {
                objDisbutionAlert = DocumentTransmission.Instance.GetHaulierDetails(mnemonic, Esdal_ref, ver_no);
                return Ok(objDisbutionAlert);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Document/GetHaulierDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Get Agreed proposed and notification details XML for Haulier
        [HttpGet]
        [Route("Document/GetAgreedProposedNotificationXML")]
        public IHttpActionResult GetAgreedProposedNotificationXML(string docType, string ESDALRef, int notificationID)
        {
            byte[] docBytes = null;
            try
            {
                docBytes = DocumentTransmission.Instance.GetAgreedProposedNotificationXML(docType, ESDALRef, notificationID);
                return Ok(docBytes);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Document/GetAgreedProposedNotificationXML, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Save DistributionStatus
        [HttpPost]
        [Route("Document/SaveDistributionStatus")]
        public IHttpActionResult SaveDistributionStatus(SaveDistributionStatusParams saveDistributionStatus)
        {
            long distrStatus = 0;
            try
            {
                distrStatus = DocumentTransmission.Instance.SaveDistributionStatus(saveDistributionStatus);
                if (distrStatus != 0)
                    return Ok(distrStatus);
                else
                    return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region  Save ActiveTransmission
        [HttpPost]
        [Route("Document/SaveActiveTransmission")]
        public IHttpActionResult SaveActiveTransmission(NotificationContacts objContact, int status, int inboxOnly, string EsdalReference, long transmissionId, bool IsImminent, string username)
        {
            long tmpTransId = 0;
            try
            {
                tmpTransId = DocumentTransmission.Instance.SaveActiveTransmission(objContact, EsdalReference, transmissionId, inboxOnly);
                if (tmpTransId != 0)
                    return Ok(tmpTransId);
                else
                    return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetTransmissionType
        [HttpGet]
        [Route("Document/GetTransmissionType")]
        public IHttpActionResult GetTransmissionType(long TransId, string Status, int StatusItemCount, string userSchema)

        {
            try
            {
                List<TransmissionModel> transmissionList = DocumentTransmission.Instance.GetTransmissionType(TransId, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

                return Ok(transmissionList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GetTransmissionType, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SortSideCheckDoctype
        [HttpGet]
        [Route("Document/SortSideCheckDoctype")]
        public IHttpActionResult SortSideCheckDoctype(int transmissionId, string userSchema)
        {
            try
            {
                TransmittingDocumentDetails transmittingDetail = DocumentTransmission.Instance.SortSideCheckDoctype(transmissionId, userSchema);
                return Ok(transmittingDetail);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SortSideCheckDoctype, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GenerateHaulierProposedDoc
        [HttpPost]
        [Route("Document/GenerateHaulierProposedDoc")]
        public IHttpActionResult GenerateHaulierProposedDoc(ProposedDocParams proposedDocParams)

        {
            byte[] byteArrayData = null;
            try
            {
                byteArrayData = DocumentConsole.Instance.GenerateHaulierProposedRouteDocument(proposedDocParams.EsdalReferenceNo, proposedDocParams.OrganisationId, proposedDocParams.ContactId, proposedDocParams.UserSchema, proposedDocParams.SessionInfo);

                return Ok(byteArrayData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateHaulierProposedDoc, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Generate PDF
        [HttpPost]
        [Route("Document/GenerateDoc")]
        public IHttpActionResult GenerateDoc(GeneratePdfParams generatePdfParams)
        {
            byte[] content = null;
            try
            {
                content = DocumentConsole.Instance.GeneratePDF(generatePdfParams.NotificationID, generatePdfParams.DocType, generatePdfParams.XMLInformation, generatePdfParams.FileName, generatePdfParams.ESDALReferenceNo , generatePdfParams.OrganisationID , generatePdfParams.ContactID, generatePdfParams.DocumentFileName, generatePdfParams.IsHaulier, generatePdfParams.OrganisationName , generatePdfParams.HAReference, generatePdfParams.RoutePlanUnits, generatePdfParams.DocumentType, generatePdfParams.UserInfo, generatePdfParams.UserType);
                
                return Ok(content);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateDoc, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Commented Code by Mahzeer on 12/07/2023
        /*
        [HttpPost]
        [Route("Document/SaveMovementActionForDistTrans")]
        public IHttpActionResult SaveMovementActionForDistTrans(GenerateMovementActionParams generateMovement)
        {
            long result = 0;
            try
            {
                result = OutBoundProvider.Instance.SaveMovementActionForDistTrans(generateMovement.MovementActionIdentifier, generateMovement.MovementDesc, generateMovement.projectId, generateMovement.revisionNo, generateMovement.versionNo, generateMovement.UserSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SaveMovementActionForDistTrans, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Document/GenerateMovementAction")]
        public IHttpActionResult GenerateMovementAction(GenerateMovementActionParams generateMovement)
        {
            bool status = false;
            try
            {
                status = OutBoundProvider.Instance.GenerateMovementAction(generateMovement.UserInfo, generateMovement.EsdalReference, generateMovement.MovementActionIdentifier, generateMovement.projectId, generateMovement.revisionNo, generateMovement.versionNo, generateMovement.MovementFlag, generateMovement.NotificationContacts);
                return Ok(status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateMovementAction, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Document/InsertTransmissionInfoToAction")]
        public IHttpActionResult InsertTransmissionInfoToAction(TransmitNotificationParams transmitNotification)
        {
            try
            {
                MessageTransmiter.InsertTransmissionInfoToAction(transmitNotification.NotificationContacts, transmitNotification.UserInfo, transmitNotification.TransmissionId, transmitNotification.EsdalReference, transmitNotification.ActionFlag, transmitNotification.ErorrMessage, transmitNotification.DocTypeName, transmitNotification.ProjectId, transmitNotification.RevisionNo, transmitNotification.VersionNo);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/InsertTransmissionInfoToAction, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Document/TransmitNotification")]
        public IHttpActionResult TransmitNotification(TransmitNotificationParams transmitNotification)
        {
            byte[] content = null;
            try
            {
                content = MessageTransmiter.TransmitNotification(transmitNotification.NotificationContacts, transmitNotification.UserInfo, transmitNotification.EsdalReference, transmitNotification.NotifHtmlString, transmitNotification.TransmissionId, transmitNotification.Indemnity, transmitNotification.AttachXml, transmitNotification.IsImminent, transmitNotification.DocType, transmitNotification.ProjectId, transmitNotification.RevisionNo, transmitNotification.VersionNo);

                return Ok(content);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/TransmitNotification, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Document/GenerateDoc1")]
        public IHttpActionResult GenerateDoc1(GeneratePdfParams generatePdfParams)
        {
            string content = null;
            try
            {
                content = DocumentConsole.Instance.GeneratePDF1(generatePdfParams.NotificationID, generatePdfParams.DocType, generatePdfParams.XMLInformation, generatePdfParams.FileName, generatePdfParams.ESDALReferenceNo, generatePdfParams.OrganisationID, generatePdfParams.ContactID, generatePdfParams.DocumentFileName, generatePdfParams.IsHaulier, generatePdfParams.OrganisationName, generatePdfParams.HAReference, generatePdfParams.RoutePlanUnits, generatePdfParams.DocumentType, generatePdfParams.UserInfo, generatePdfParams.UserType);

                return Ok(content);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateDoc, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Document/SaveOutboundNotification")]
        public IHttpActionResult SaveOutboundNotification(SaveOutboundNotificationParams notificationParams)
        {
            try
            {
                DocumentConsole.Instance.SaveDetailOutboundNotification(notificationParams.NotificationId, notificationParams.CompressData);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SaveDetailOutboundNotification, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        [HttpPost]
        [Route("Document/OldGenerateEsdalReNotification")]
        public IHttpActionResult OldGenerateEsdalReNotification(GenerateEsdalNotificationParams esdalNotificationParams)
        {
            List<byte[]> docarray = null;
            try
            {
                docarray = DocumentConsole.Instance.OldGenerateEsdalReNotification(esdalNotificationParams.NotificationId, esdalNotificationParams.ContactId, esdalNotificationParams.ICAStatusDictionary, esdalNotificationParams.ImminentMoveStatus, esdalNotificationParams.UserInfo);

                return Ok(docarray);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateEsdalReNotification, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Document/SortSideRetransmitApplication")]
        public IHttpActionResult SortSideRetransmitApplication(RetransmitApplicationParams retransmitApplicationParams)
        {
            int status = 0;
            try
            {
                status = DocumentTransmission.Instance.SortSideRetransmitApplication(retransmitApplicationParams.TransmissionId, retransmitApplicationParams.RetransmitDetails, retransmitApplicationParams.UserInfo);

                return Ok(status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SortSideRetransmitApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        */
        #endregion

        #region Generate PDF html
        [HttpPost]
        [Route("Document/GeneratehtmlDoc")]
        public IHttpActionResult GeneratehtmlDoc(GeneratePdfParams generatePdfParams)
        {
            string content = null;
            try
            {
                content = DocumentConsole.Instance.GenerateHTMLPDF(generatePdfParams.NotificationID, generatePdfParams.DocType, generatePdfParams.XMLInformation, generatePdfParams.FileName, generatePdfParams.ESDALReferenceNo, generatePdfParams.OrganisationID, generatePdfParams.ContactID, generatePdfParams.DocumentFileName, generatePdfParams.IsHaulier, generatePdfParams.OrganisationName, generatePdfParams.HAReference, generatePdfParams.RoutePlanUnits, generatePdfParams.DocumentType, generatePdfParams.UserInfo, generatePdfParams.UserType);

                return Ok(content);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateDoc, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetLoggedInUserAffectedStructureDetailsByESDALReference
        [HttpPost]
        [Route("Document/AffectedStructureDetailsByESDALReference")]
        public IHttpActionResult AffectedStructureDetailsByESDALReference(AffectedStructureParams affectedStructureParams)
        {
            string xmlInfo = null;
            try
            {
                xmlInfo = DocumentConsole.Instance.GetLoggedInUserAffectedStructureDetailsByESDALReference(affectedStructureParams.XMLInformation, affectedStructureParams.ESDALReferenceNo, affectedStructureParams.SessionInfo, affectedStructureParams.UserSchema, affectedStructureParams.Type, affectedStructureParams.OrganisationId);

                return Ok(xmlInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/AffectedStructureDetailsByESDALReference, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Generate ESDAL Notification
        [HttpPost]
        [Route("Document/GenerateEsdalNotification")]
        public IHttpActionResult GenerateEsdalNotification(GenerateEsdalNotificationParams esdalNotificationParams)
        {
            try
            {
                ESDALNotificationGetParams getParams = DocumentConsole.Instance.GenerateEsdalNotification(esdalNotificationParams.NotificationId, esdalNotificationParams.ContactId);
                return Content(HttpStatusCode.OK, getParams);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateEsdalNotification, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Generate ESDAL ReNotification
        [HttpPost]
        [Route("Document/GenerateEsdalReNotification")]
        public IHttpActionResult GenerateEsdalReNotification(GenerateEsdalNotificationParams esdalNotificationParams)
        {
            try
            {
                ESDALNotificationGetParams getParams = DocumentConsole.Instance.GenerateEsdalReNotification(esdalNotificationParams.NotificationId, esdalNotificationParams.ContactId);
                return Content(HttpStatusCode.OK, getParams);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateEsdalReNotification, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #endregion

        #region SortSideRetransmitApplication
        [HttpPost]
        [Route("Document/CopyMovementSortToPortal")]
        public IHttpActionResult CopyMovementSortToPortal(CopyMovementSortToPortalInsertParams copyMovementSortToPortalInsertParams)
        {
            long status = 0;
            try
            {

                status = DocumentTransmission.Instance.CopyMovementSortToPortal(copyMovementSortToPortalInsertParams.MovementCopyDetail, copyMovementSortToPortalInsertParams.MovementCloneStatus, copyMovementSortToPortalInsertParams.VersionID, copyMovementSortToPortalInsertParams.EsdalReference, copyMovementSortToPortalInsertParams.HAContactBytes, copyMovementSortToPortalInsertParams.OrganizationID, copyMovementSortToPortalInsertParams.ModelUserSchema);

                return Ok(status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SortSideRetransmitApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetRetransmitDetails
        [HttpGet]
        [Route("Document/GetRetransmitDetails")]
        public IHttpActionResult GetRetransmitDetails(long transmissionId, string userSchema)
        {
            RetransmitDetails retransmitDetails = new RetransmitDetails();
            try
            {
                retransmitDetails = DocumentTransmission.Instance.GetRetransmitDetails(transmissionId, userSchema);
                return Ok(retransmitDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GetRetransmitDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetSOAPoliceContactList
        [HttpPost]
        [Route("Document/GetSOAPoliceContactList")]
        public IHttpActionResult GetSOAPoliceContactList(XMLModel modelSOAPolice)
        {
            List<ContactModel> contactSOAPoliceList = new List<ContactModel>();
            try
            {
                contactSOAPoliceList = DocumentConsole.Instance.GetSOAPoliceContactList(modelSOAPolice);
                return Ok(contactSOAPoliceList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GetSOAPoliceContactList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region fetchContactPreference
        [HttpGet]
        [Route("Document/FetchContactPreference")]
        public IHttpActionResult FetchContactPreference(int contactId, string userSchema)
        {
            string[] contactDet = new string[6];
            try
            {
                contactDet = DocumentConsole.Instance.FetchContactPreference(contactId, userSchema);
                return Ok(contactDet);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/fetchContactPreference, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetImminentForCountries
        [HttpGet]
        [Route("Document/GetImminentForCountries")]
        public IHttpActionResult GetImminentForCountries(int Orgid, string ImminentStatus)
        {
            bool ImminentFlag = false;
            try
            {
                ImminentFlag = DocumentConsole.Instance.GetImminentForCountries(Orgid, ImminentStatus);
                return Ok(ImminentFlag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/fetchContactPreference, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GenerateEMAIL
        [HttpPost]
        [Route("Document/GenerateEMAIL")]
        public IHttpActionResult GenerateEMAIL(GenerateEmailParams emailParams)
        {
            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams();
            try
            {
                emailgetParams = CommonMethods.GenerateEMAIL(emailParams.NotificationId, emailParams.DocType, emailParams.XmlInformation, emailParams.FileName, emailParams.ESDALReferenceNo, emailParams.OrganisationId, emailParams.Contact, emailParams.DocumentFileName, emailParams.TransmitMethodCallReq, emailParams.UserInfo, emailParams.IcaStatus, emailParams.Indemnity, emailParams.xmlAttach, emailParams.ImminentMovestatus, emailParams.RoutePlanUnits, emailParams.Projectstatus);

                return Ok(emailgetParams);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateEMAIL, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveDocument
        [HttpPost]
        [Route("Document/SaveDocument")]
        public IHttpActionResult SaveDocument(SaveDocumentParams saveDocumentParams)
        {
            try
            {
                long docid = CommonMethods.SaveDocument(saveDocumentParams.NotificationId, saveDocumentParams.DocType, saveDocumentParams.OrganisationId, saveDocumentParams.ESDALReferenceNo, saveDocumentParams.ContactId, saveDocumentParams.ExportByteArrayData, saveDocumentParams.UserSchema, saveDocumentParams.UserInfo, saveDocumentParams.Contact, saveDocumentParams.ProjectId, saveDocumentParams.RevisionNo, saveDocumentParams.VersionNo);

                return Ok(docid);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SaveDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveInboxItems
        [HttpPost]
        [Route("Document/SaveInboxItems")]
        public IHttpActionResult SaveInboxItems(SaveInboxParams saveInboxParams)
        {
            long inboxId = 0;
            try
            {
                inboxId = OutBoundProvider.Instance.SaveInboxItems(saveInboxParams.NotificationId, saveInboxParams.DocumentId, saveInboxParams.OrganisationId, saveInboxParams.ESDALReferenceNo, saveInboxParams.UserSchema, saveInboxParams.IcaStatus, saveInboxParams.ImminentMovestatus);
                return Ok(inboxId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/SaveInboxItems, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GenerateNotificationPDF
        [HttpPost]
        [Route("Document/GenerateNotificationPDF")]
        public IHttpActionResult GenerateNotificationPDF(GenerateEmailParams emailParams)
        {
            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams();
            try
            {
                emailgetParams = CommonMethods.GenerateNotificationPDF(emailParams.NotificationId, emailParams.DocType, emailParams.XmlInformation, emailParams.FileName, emailParams.ESDALReferenceNo, emailParams.OrganisationId, emailParams.Contact, emailParams.DocumentFileName,emailParams.UserInfo, emailParams.IcaStatus, emailParams.Indemnity, emailParams.ImminentMovestatus, emailParams.RoutePlanUnits);

                return Ok(emailgetParams);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateEMAIL, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GenerateWord
        [HttpPost]
        [Route("Document/GenerateWord")]
        public IHttpActionResult GenerateWord(GenerateEmailParams emailParams)
        {
            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams();
            try
            {
                emailgetParams = CommonMethods.GenerateWord(emailParams.NotificationId, emailParams.DocType, emailParams.XmlInformation, emailParams.FileName, emailParams.ESDALReferenceNo, emailParams.OrganisationId, emailParams.DocumentFileName, emailParams.Contact, emailParams.UserInfo, emailParams.IcaStatus, emailParams.Indemnity, emailParams.ImminentMovestatus, emailParams.RoutePlanUnits,emailParams.GenerateFlag);

                return Ok(emailgetParams);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateWord, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetNewInsertedTransForDist
        [HttpPost]
        [Route("Document/GetNewInsertedTransForDist")]
        public IHttpActionResult GetNewInsertedTransForDist(TransmittingDocumentDetails transDetails)
        {
            long newtransId = 0;
            try
            {
                newtransId= DocumentTransmission.Instance.GetNewInsertedTransForDist(transDetails);
                return Ok(newtransId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GetNewInsertedTransForDist, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetRetransmitDocument

        [HttpPost]
        [Route("Document/GetRetransmitDocument")]
        public IHttpActionResult GetRetransmitDocument(GetRetransmitDocumentParams getRetransmit)
        {
            RetransmitEmailgetParams retransmit = new RetransmitEmailgetParams();
            try
            {
                retransmit = DocumentTransmission.Instance.GetRetransmitDocument(getRetransmit.TransmittingDetail, getRetransmit.RetransmitDetails, getRetransmit.TransmissionId, getRetransmit.UserInfo, getRetransmit.UserSchema);
                return Ok(retransmit);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GetRetransmitDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GenerateSODistributeDocument
        [HttpPost]
        [Route("Document/GenerateSODistributeDocument")]
        public IHttpActionResult GenerateSODistributeDocument(SOProposalDocumentParams documentParams)
        {
            SODistributeDocumentParams sODistribute = new SODistributeDocumentParams();
            try
            {
                sODistribute=DocumentConsole.Instance.GenerateSODistributeDocument(documentParams.EsdalReferenceNo, documentParams.OrganisationId, documentParams.ContactId, documentParams.DistributionComments, documentParams.VersionId, documentParams.ICAStatusDictionary, documentParams.Esdalreference, documentParams.HaContactDetail, documentParams.Agreedroute, documentParams.UserSchema, documentParams.RoutePlanUnits, documentParams.ProjectStatus, documentParams.VersionNo, documentParams.Moveprint, documentParams.PreVersionDistr, documentParams.SessionInfo);
                return Ok(sODistribute);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GenerateSODistributeDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetSoProposalXsltPath
        [HttpGet]
        [Route("Document/GetSoProposalXsltPath")]
        public IHttpActionResult GetSoProposalXsltPath(string ContactType, long ProjectStatus, string FinalReson)
        {
            SOProposalXsltPath sOProposalXslt = new SOProposalXsltPath();
            try
            {
                sOProposalXslt = DocumentConsole.Instance.GetSoProposalXsltPath(ContactType, ProjectStatus, FinalReson);
                return Ok(sOProposalXslt);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GetSoProposalXsltPath, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetDispensation
        [HttpGet]
        [Route("Document/GetDispensation")]
        public IHttpActionResult GetDispensation(long notificationId, int historic)
        {
            try
            {
                var result = DocumentTransmission.Instance.GetNotificationDispensation(notificationId, historic);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Document/GetDispensation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetDocument
        [HttpPost]
        [Route("Document/GetDocument")]
        public IHttpActionResult GetDocument(SOProposalDocumentParams documentParams)
        {
            try
            {
                var result = DocumentTransmission.Instance.GetDocument(documentParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GetProposalDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region Generate PDF From Html
        [HttpPost]
        [Route("Document/GeneratePDFFromHtmlString")]
        public IHttpActionResult GeneratePDFFromHtmlString(HtmlDocumentParams model)
        {
            byte[] content = null;
            try
            {
                content = DocumentConsole.Instance.GeneratePDFFromHtmlString(model.InputHtmlString);
                
                return Ok(content);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Document/GeneratePDFFromHtmlString, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

    }
}

