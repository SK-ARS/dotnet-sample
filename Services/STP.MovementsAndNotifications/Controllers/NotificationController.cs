using STP.Common.Constants;
using STP.MovementsAndNotifications.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.Routes;
using STP.Domain.Communications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.MovementsAndNotifications.Controllers
{
    public class NotificationController : ApiController
    {
        #region Insert Quick Link
        [HttpPost]
        [Route("Notification/InsertQuickLink")]
        public IHttpActionResult InsertQuickLink(InsertQuickLinkParams objInsertQuickLinkParams)
        {
            try
            {
                int linkno = QuickLinksProvider.Instance.InsertQuickLink(objInsertQuickLinkParams);
                return Ok(linkno);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/InsertQuickLink, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get IsAcknowledge
        [HttpGet]
        [Route("Notification/IsAcknowledged")]
        public IHttpActionResult IsAcknowledged(string esdalRefernce, int historic)
        {
            bool result = true;
            try
            {
                result = NotificationProvider.Instance.IsAcknowledged(esdalRefernce, historic);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/IsAcknowledged, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get unacknowledged Collaboration
        [HttpGet]
        [Route("Notification/GetUnacknowledgedCollaboration")]
        public IHttpActionResult GetUnacknowledgedCollaboration(string notificationCode, int historic)
        {
            CollaborationModel objCollaboration = new CollaborationModel();
            try
            {
                objCollaboration = NotificationProvider.Instance.GetUnacknowledgedCollaboration(notificationCode, historic);
                return Ok(objCollaboration);              
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetUnacknowledgedCollaboration, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Check Notification Submitted
        [HttpGet]
        [Route("Notification/CheckNotificationSubmitted")]
        public IHttpActionResult CheckNotificationSubmitted(int notificationId)
        {
            try
            {
                string notif_code = NotificationProvider.Instance.CheckNotificationSubmitted(notificationId);
                return Ok(notif_code);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/CheckNotificationSubmitted, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Notification General Details
        [HttpGet]
        [Route("Notification/GetNotificationGeneralDetail")]
        public IHttpActionResult GetNotificationGeneralDetail(long notificationId, int historic)
        {
            try
            {
                NotificationGeneralDetails obj = SimpleNotificationProvider.Instance.GetNotificationGeneralDetail(notificationId, historic);               
                return Content(HttpStatusCode.OK, obj);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetNotificationGeneralDetail, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetNotificationAffectedStructures
        [HttpGet]
        [Route("Notification/GetNotificationAffectedStructures")]
        public IHttpActionResult GetNotificationAffectedStructures(int notificationId, string esdalReferenceNumber, string haulierMnemonic, string versionNumber, string userSchema = UserSchema.Portal)
        {
            try
            {
                byte[] result = SimpleNotificationProvider.Instance.GetNotificationAffectedStructures(notificationId, esdalReferenceNumber, haulierMnemonic, versionNumber, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/GetNotificationAffectedStructures,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetOrderNoProjectId
        [HttpGet]
        [Route("Notification/GetOrderNoProjectId")]
        public IHttpActionResult GetOrderNoProjectId(int versionId)
        {
            try
            {
                MovementPrint result = SimpleNotificationProvider.Instance.GetOrderNoProjectId(versionId);
                if (result == null)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
                }
                return Ok(result);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetProjectIdByEsdalReferenceNo
        [HttpGet]
        [Route("Notification/GetProjectIdByEsdalReferenceNo")]
        public IHttpActionResult GetProjectIdByEsdalReferenceNo(string EsdalRefNo)
        {
            try
            {
                MovementPrint result = SimpleNotificationProvider.Instance.GetProjectIdByEsdalReferenceNo(EsdalRefNo);
                if (result == null)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
                }
                return Ok(result);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region DeleteNotification
        [HttpDelete]
        [Route("Notification/DeleteNotification")]
        public IHttpActionResult DeleteNotification(int notificationId)
        {
            try
            {
                int affectedRows = SimpleNotificationProvider.Instance.DeleteNotification(notificationId);
                if (affectedRows > -1)
                    return Content(HttpStatusCode.OK, affectedRows);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/IsNotifSubmitCheck, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Get Affected Parties
        [HttpGet]
        [Route("Notification/GetAffectedParties")]
        public IHttpActionResult GetAffectedParties(int notificationId, string userSchema = UserSchema.Portal)
        {
            try
            {
                byte[] result = SimpleNotificationProvider.Instance.GetAffectedParties(notificationId,userSchema);             
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/CheckNotifValidation, Exception: "+ ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Get Response Mail Details
        [HttpGet]
        [Route("Notification/GetResponseMailDetails")]
        public IHttpActionResult GetResponseMailDetails(int organisationId, string userSchema = UserSchema.Portal)
        {
            try
            {
                MailResponse result = SimpleNotificationProvider.Instance.GetResponseMailDetails(organisationId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetResponseMailDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ShowImminentMovement
        [HttpGet]
        [Route("Notification/ShowImminentMovement")]
        public IHttpActionResult ShowImminentMovement(string moveStartDate, string countryId, int countryIdCount, int vehicleClass)
        {
            try
            {
                int? result = ManageImminentProvider.Instance.ShowImminentMovement(moveStartDate, countryId, countryIdCount, vehicleClass);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/ShowImminentMovement,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Save Affected Notification Details
        [HttpPost]
        [Route("Notification/SaveAffectedNotificationDetails")]
        public IHttpActionResult SaveAffectedNotificationDetails(AffectedStructConstrParam affectedParam)
        {
            try
            {
                bool result = SimpleNotificationProvider.Instance.SaveAffectedNotificationDetails(affectedParam);
                return Content(HttpStatusCode.Created, result);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/SaveAffectedNotificationDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Update Notification
        [HttpPost]
        [Route("Notification/UpdateNotification")]
        public IHttpActionResult UpdateNotification(NotificationGeneralDetails notificationGeneralDetails)
        {
            try
            {
                int result = SimpleNotificationProvider.Instance.UpdateNotification(notificationGeneralDetails);
                if (result >= 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/UpdateNotification, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Notify Application
        [HttpGet]
        [Route("Notification/NotifyApplication")]
        public IHttpActionResult NotifyApplication(long versionId)
        {
            try
            {
                NotificationGeneralDetails objNotify = SimpleNotificationProvider.Instance.NotifyApplication(versionId);
                return Content(HttpStatusCode.OK, objNotify);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/NotifyApplication, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region CheckNotificationVersion
        [HttpGet]
        [Route("Notification/CheckNotificationVersion")]
        public IHttpActionResult CheckNotificationVersion(int notificationId)
        {
            try
            {
                int result = SimpleNotificationProvider.Instance.CheckNotificationVersion(notificationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/CheckNotificationVersion,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GenerateNotificationCode
        [HttpGet]
        [Route("Notification/GenerateNotificationCode")]
        public IHttpActionResult GenerateNotificationCode(int organisationId, long notificationId, int detail)
        {
            try
            {
                string result = SimpleNotificationProvider.Instance.GenerateNotificationCode(organisationId, notificationId, detail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/GenerateNotificationCode,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region UpdateCollborationAcknowledgement
        [HttpGet]
        [Route("Notification/UpdateCollborationAcknowledgement")]
        public IHttpActionResult UpdateCollborationAcknowledgement(long docId, int colNo, int userId, string acknowledgeAgainst, int historic)
        {
            bool result = true;
            try
            {
                result = NotificationProvider.Instance.UpdateCollborationAcknowledgement(docId, colNo, userId, acknowledgeAgainst, historic);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/UpdateCollborationAcknowledgement, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetNotificationStatusList
        [HttpGet]
        [Route("Notification/GetNotificationStatusList")]
        public IHttpActionResult GetNotificationStatusList(int pageNumber, int pageSize, string NotificationCode, string userSchema)
        {
            try
            {
                List<NotificationStatusModel> result = NotificationProvider.Instance.GetNotificationStatusList(pageNumber, pageSize, NotificationCode, userSchema);
                return Content(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/UpdateCollborationAcknowledgement, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetCollaborationList
        [HttpGet]
        [Route("Notification/GetCollaborationList")]
        public IHttpActionResult GetCollaborationList(int pageNumber, int pageSize, string notificationCode, int notificationId, int historic)
        {
            try
            {
                List<CollaborationModel> result = NotificationProvider.Instance.GetCollaborationList(pageNumber, pageSize, notificationCode, notificationId, historic);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetCollaborationList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetInternalCollaboration
        [HttpPost]
        [Route("Notification/GetInternalCollaboration")]
        public IHttpActionResult GetInternalCollaboration(NotificationStatusModel notificationStatusModel)
        {
            try
            {
                NotificationStatusModel result = NotificationProvider.Instance.GetInternalCollaboration(notificationStatusModel, notificationStatusModel.userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetInternalCollaboration, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ManageCollaborationInternal
        [HttpPost]
        [Route("Notification/ManageCollaborationInternal")]
        public IHttpActionResult ManageCollaborationInternal(NotificationStatusModel notificationStatusModel)
        {
            try
            {
                bool result = NotificationProvider.Instance.ManageCollaborationInternal(notificationStatusModel, notificationStatusModel.userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/ManageCollaborationInternal, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetExternalCollaboration
        [HttpGet]
        [Route("Notification/GetExternalCollaboration")]
        public IHttpActionResult GetExternalCollaboration(int pageNumber, int pageSize, int Document_Id, string userSchema)
        {
            try
            {
                List<CollaborationModel> result = NotificationProvider.Instance.GetExternalCollaboration(pageNumber, pageSize, Document_Id, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetExternalCollaboration, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        
        #region  RenotifyNotification
        [HttpPost]
        [Route("Notification/RenotifyNotification")]
        public IHttpActionResult RenotifyNotification(int notificationId, int VR1)
        {
            try
            {
                NotificationGeneralDetails result = SimpleNotificationProvider.Instance.RenotifyNotification(notificationId, VR1);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/RenotifyNotification, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        
        #region  CloneNotification
        [HttpPost]
        [Route("Notification/CloneNotification")]
        public IHttpActionResult CloneNotification(int notificationId)
        {
            try
            {
                NotificationGeneralDetails result = SimpleNotificationProvider.Instance.CloneNotification(notificationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/CloneNotification, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetTransmissionList
        [HttpPost]
        [Route("Notification/GetTransmissionList")]
        public IHttpActionResult GetTransmissionList(GetTransmissionListParams getTransmissionList)
        {
            try
            {
                List<TransmissionModel> result = NotificationProvider.Instance.GetTransmissionList(getTransmissionList);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetCollaborationList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetInboxSubContent
        [HttpGet]
        [Route("Notification/GetInboxSubContent")]
        public IHttpActionResult GetInboxSubContent(int pageNumber, int pageSize,int versionId, int orgId, int notifhistory)
        {
            try
            {
                List<InboxSubContent> result = MovementInboxSubcontent.Instance.GetInboxSubContent(pageNumber,pageSize,versionId, orgId, notifhistory);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetInboxSubContent, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetSORTHistoryDetails
        [HttpGet]
        [Route("Notification/GetSORTHistoryDetails")]
        public IHttpActionResult GetSORTHistoryDetails(string esdalref, int versionno)
        {
            try
            {
                List<InboxSubContent> result = MovementInboxSubcontent.Instance.GetSORTHistoryDetails(esdalref, versionno);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetInboxSubContent, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region InsertApplicationType
        [HttpPost]
        [Route("Notification/InsertNotificationType")]
        public IHttpActionResult InsertNotificationType(PlanMovementType saveNotifType)
        {
            try
            {
                NotifGeneralDetails notifGeneral = NotificationProvider.Instance.InsertNotificationType(saveNotifType);
                return Content(HttpStatusCode.Created, notifGeneral);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/InsertNotificationType, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region UpdateNotificationType
        [HttpPost]
        [Route("Notification/UpdateNotificationType")]
        public IHttpActionResult UpdateNotificationType(PlanMovementType updateNotifType)
        {
            try
            {
                NotifGeneralDetails notifGeneral = NotificationProvider.Instance.UpdateNotificationType(updateNotifType);
                return Content(HttpStatusCode.Created, notifGeneral);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/UpdateNotificationType, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SetLoginStatus
        [HttpGet]
        [Route("Notification/SetLoginStatus")]
        public IHttpActionResult SetLoginStatus(int UserId, int flag)
        {
            try
            {
                int result = NotificationProvider.Instance.SetLoginStatus(UserId,flag);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/SetLoginStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetHaulierDetails
        [HttpGet]
        [Route("Notification/GetHaulierDetails")]
        public IHttpActionResult GetHaulierDetails(long notificationId)
        {
            try
            {
                HAContact contact = NotificationProvider.Instance.GetHaulierDetails(notificationId);
                return Content(HttpStatusCode.Created, contact);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Notification/GetOutboundDoc, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  CloneNotification
        [HttpPost]
        [Route("Notification/CloneHistoricNotification")]
        public IHttpActionResult CloneHistoricNotification(int notificationId)
        {
            try
            {
                NotificationGeneralDetails generalDetails = SimpleNotificationProvider.Instance.CloneHistoricNotification(notificationId);
                return Content(HttpStatusCode.OK, generalDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/CloneNotification, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Code Commented By Mahzeer on 04-12-2023

        #region GetVehicleTypeForNotification
        /*[HttpGet]
        [Route("Notification/GetVehicleTypeForNotification")]
        public IHttpActionResult GetVehicleTypeForNotification(int notificationId)
        {
            try
            {
                long result = SimpleNotificationProvider.Instance.GetVehicleTypeForNotification(notificationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/GetVehicleTypeForNotification,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region Update Inbound Notification
        /*[HttpPost]
        [Route("Notification/UpdateInboundNotif")]
        public IHttpActionResult UpdateInboundNotif(UpdateInboundNotifParam updateInboundNotif)
        {
            try
            {
                int result = SimpleNotificationProvider.Instance.UpdateInboundNotif(updateInboundNotif.NotificationId, updateInboundNotif.InboundNotification);
                if (result >= 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/UpdateInboundNotif, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region GetDetailsToChkImminent
        /*[HttpGet]
        [Route("Notification/GetDetailsToChkImminent")]
        public IHttpActionResult GetDetailsToChkImminent(long notificationId, string contentReferenceNo, long revisionId, string userSchema)
        {
            try
            {
                GetImminentChkDetailsDomain result = ManageImminentProvider.Instance.GetDetailsToChkImminent(notificationId, contentReferenceNo, revisionId, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/GetDetailsToChkImminent,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region CheckImminent
        /*[HttpPost]
        [Route("Notification/CheckImminent")]
        public IHttpActionResult CheckImminent(CheckImminentParam checkImminentParam)
        {
            try
            {
                int? result = ManageImminentProvider.Instance.CheckImminent(checkImminentParam.VehicleClass, checkImminentParam.VehicleWidth, checkImminentParam.VehicleLength, checkImminentParam.RigidLength, checkImminentParam.GrossWeight, checkImminentParam.WorkingDays, checkImminentParam.FrontPRJ, checkImminentParam.RearPRJ, checkImminentParam.LeftPRJ, checkImminentParam.RightPRJ, checkImminentParam.ImminentCheckDetails, checkImminentParam.NotificationType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/CheckImminent,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region GetInboundNotification
        /*[HttpGet]
        [Route("Notification/GetInboundNotification")]
        public IHttpActionResult GetInboundNotification(int notificationId)
        {
            byte[] inbboundNotif = null;
            try
            {
                inbboundNotif = SimpleNotificationProvider.Instance.GetInboundNotification(notificationId);
                return Ok(inbboundNotif);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetInboundNotification, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region ListAxelDetails
        /*[HttpGet]
        [Route("Notification/ListAxelDetails")]
        public IHttpActionResult ListAxelDetails(int vehicleId)
        {
            try
            {
                List<AxleDetails> result = SimpleNotificationProvider.Instance.ListAxelDetails(vehicleId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/ListCloneAxelDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion   

        #region ListCloneAxelDetails
        /*[HttpGet]
        [Route("Notification/ListCloneAxelDetails")]
        public IHttpActionResult ListCloneAxelDetails(int vehicleId)
        {
            try
            {
                List<AxleDetails> result = SimpleNotificationProvider.Instance.ListCloneAxelDetails(vehicleId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/ListCloneAxelDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region  Save Notification General Details
        /*[HttpPost]
        [Route("Notification/SaveNotifGeneralDetails")]
        public IHttpActionResult SaveNotifGeneralDetails(NotificationGeneralDetails notificationGeneralDetails)
        {
            try
            {
                NotificationGeneralDetails result = SimpleNotificationProvider.Instance.SaveNotifGeneralDetails(notificationGeneralDetails);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/SaveNotifGeneralDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region IsNotifSubmitCheck
        /*[HttpGet]
        [Route("Notification/IsNotifSubmitCheck")]
        public IHttpActionResult IsNotifSubmitCheck(long notificationId)
        {
            try
            {
                int result = SimpleNotificationProvider.Instance.IsNotifSubmitCheck(notificationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/IsNotifSubmitCheck, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region  Check Notification Validation
        /*[HttpGet]
        [Route("Notification/CheckNotifValidation")]
        public IHttpActionResult CheckNotifValidation(string contentReferenceNo)
        {
            try
            {
                NotificationGeneralDetails result = SimpleNotificationProvider.Instance.CheckNotifValidation(contentReferenceNo);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/CheckNotifValidation, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region  Get Haulier Detailed Notification
        /*[HttpGet]
        [Route("Notification/GetNotifHaulierDetails")]
        public IHttpActionResult GetNotifHaulierDetails(int userId, int notificationId = 0, int vehicleClassCode = 0)
        {
            try
            {
                UserRegistration result = SimpleNotificationProvider.Instance.GetNotifHaulierDetails(userId, notificationId, vehicleClassCode);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetNotifHaulierDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region Set Notification VR Num
        /*[HttpGet]
        [Route("Notification/SetNotificationVRNum")]
        public IHttpActionResult SetNotificationVRNum(int notificationId)
        {
            try
            {
                bool result = SimpleNotificationProvider.Instance.SetNotificationVRNum(notificationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/SetNotificationVRNum, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region  Get Haulier Licence
        /*[HttpGet]
        [Route("Notification/GetHaulierLicence")]
        public IHttpActionResult GetHaulierLicence(int organisationId)
        {
            try
            {
                string result = NotificationProvider.Instance.GetHaulierLicence(organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetHaulierLicence, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region  Get Notifcation Route Details
        /*[HttpGet]
        [Route("Notification/GetNotifcationRouteDetails")]
        public IHttpActionResult GetNotifcationRouteDetails(string ContentRefNo)
        {
            List<ListRouteVehicleId> objDetails = new List<ListRouteVehicleId>();
            try
            {
                objDetails = NotificationProvider.Instance.GetNotifcationRouteDetails(ContentRefNo);
                return Ok(objDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetNotifcationRouteDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region Get Max Reduciable Height
        /*[HttpGet]
        [Route("Notification/GetMaxReduciableHeight")]
        public IHttpActionResult GetMaxReduciableHeight(int notificationId)
        {
            decimal result = 0;
            try
            {
                result = SimpleNotificationProvider.Instance.GetMaxReduciableHeight(notificationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetMaxReduciableHeight, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region  GetGrossWeight
        /*
        [HttpGet]
        [Route("Notification/GetGrossWeight")]
        public IHttpActionResult GetGrossWeight(int notificationId)
        {
            try
            {
                int result = SimpleNotificationProvider.Instance.GetGrossWeight(notificationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/GetGrossWeight, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region PrintCollabrationList
        /*
        [HttpGet]
        [Route("Notification/PrintCollabrationList")]
        public IHttpActionResult PrintCollabrationList(string notificationCode, int notificationId)
        {
            try
            {
                List<CollaborationModel> result = NotificationProvider.Instance.PrintCollabrationList(notificationCode, notificationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Notification/PrintCollabrationList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region GenerateOutboundNotificationStructureData
        /*
        [HttpGet]
        [Route("Notification/GenerateOutboundNotificationStructureData")]
        public IHttpActionResult GenerateOutboundNotificationStructureData(long NotificationId)
        {
            try
            {
                NotificationXSD.OutboundNotificationStructure obns = NotificationDocumentProvider.Instance.GenerateOutboundNotificationStructureData(NotificationId);
                return Content(HttpStatusCode.Created, obns);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GenerateOutboundNotificationStructureData, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region GetOutboundDoc
        /*
        [HttpGet]
        [Route("Notification/GetOutboundDoc")]
        public IHttpActionResult GetOutboundDoc(int notificationID)
        {
            try
            {
                OutboundDocuments outbounddocs = NotificationDocumentProvider.Instance.GetOutboundDoc(notificationID);
                return Content(HttpStatusCode.Created, outbounddocs);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/GetOutboundDoc, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion

        #region GetParentNotificationAnalysisId
        /*[HttpGet]
        [Route("Notification/GetPreviousAnalysisId")]
        public IHttpActionResult GetPreviousAnalysisId(long notificationID)
        {
            try
            {
                long analysisId = NotificationProvider.Instance.GetPreviousAnalysisId(notificationID);
                return Content(HttpStatusCode.Created, analysisId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Notification/GetPreviousAnalysisId, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }*/
        #endregion 
        
        #endregion
    }
}
