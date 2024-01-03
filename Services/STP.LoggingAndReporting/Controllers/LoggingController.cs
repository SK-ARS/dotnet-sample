using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.DocumentsAndContents;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.LoggingAndReporting.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace STP.LoggingAndReporting.Controllers
{
    public class LoggingController : ApiController
    {
        /// <summary>
        ///To save SysEvents in movements.
        /// </summary>
        /// <param name="SaveSystemInputParams">Input values using modal class SaveSystemInputParams </param>        
        /// <returns></returns>
        [HttpPost]
        [Route("Logging/SaveSysEvents")]
        public IHttpActionResult SaveSysEvents(int SystemEventType, string SysDescrp, int userid, string userschema)
        {
            try
            {
                bool result = LoggingProvider.Instance.SaveSysEvents(SystemEventType, SysDescrp, userid, userschema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Logging/SaveSysEvents,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }

        [HttpPost]
        [Route("Logging/SaveMovementAction")]
        public IHttpActionResult SaveMovementAction(SaveMovementActionParam saveMovementActionParam)
        {
            try
            {
                long actionId = LoggingProvider.Instance.SaveMovementAction(saveMovementActionParam.esdalRef, saveMovementActionParam.movementActionType, saveMovementActionParam.movementDescription, saveMovementActionParam.projectId, saveMovementActionParam.revisionNo, saveMovementActionParam.versionNo, saveMovementActionParam.userSchema);

                return Content(HttpStatusCode.OK, actionId);
            }
           
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Logging/SaveMovementAction, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Logging/InsertTransmissionInfoToAction")]
        public IHttpActionResult InsertTransmissionInfoToAction(TransmissionParams inputparams)
        {
            try
            {
                LoggingProvider.Instance.InsertTransmissionInfoToAction(inputparams.ObjectContact, inputparams.UserInfo, inputparams.TransmissionId, inputparams.ESDALReference, inputparams.ActionFlag, inputparams.ErrorMessage, inputparams.DocumentType);
                return Ok();
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Logging/InsertTransmissionInfoToAction, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region Get AuditList Search
        [HttpGet]
        [Route("Logging/GetAuditListSearch")]
        public IHttpActionResult GetAuditListSearch(string searchString, int pageNo, int pageSize, int sortFlag, long organisationId, string searchType,int searchNotificationSource, int presetFilter, int? sortOrder)
        {
            try
            {
                List<NENAuditLogList> auditList = LoggingProvider.Instance.GetAuditListSearch(searchString, pageNo, pageSize, sortFlag, organisationId, searchType,searchNotificationSource, presetFilter, sortOrder);
                return Content(HttpStatusCode.OK, auditList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Logging/GetAuditListSearch, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get NEN Auditlog
        [HttpPost]
        [Route("Logging/GetAuditlogNEN")]
        public IHttpActionResult GetAuditlogNEN(AuditLogParams auditLogParams)
        {
            try
            {
                List<NENAuditGridList> auditList = LoggingProvider.Instance.GetAuditlogNEN(auditLogParams.Page, auditLogParams.PageSize, auditLogParams.NENNotificationNo, auditLogParams.OrganisationId,auditLogParams.sortOrder,auditLogParams.sortType);
                return Content(HttpStatusCode.OK, auditList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Logging/GetAuditlogNEN, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpGet]
        [Route("Logging/GetNotificationHistory")]
        public IHttpActionResult GetNotificationHistory(int pageNumber, int pageSize, long notificationNo,int sortOrder,int sortType, int historic, int userType=0, long projectId = 0)
        {
            try
            {
                List<NotificationHistory> result = LoggingProvider.Instance.GetNotificationHistory(pageNumber, pageSize, notificationNo,sortOrder,sortType, historic, userType, projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Notification/GetNotificationHistory,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
