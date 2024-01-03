using STP.Common.Constants;
using STP.Common.Logger;
using STP.MovementsAndNotifications.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.LoggingAndReporting;
using STP.Domain.NonESDAL;

namespace STP.MovementsAndNotifications.Controllers
{
    public class NENNotificationController : ApiController
    {
        #region Get NEN Haulier
        [HttpGet]
        [Route("NENNotification/GetNeHaulier")]
        public IHttpActionResult GetNeHaulier(int pageNo = 1, int pageSize = 20, string searchString = null, int isVal = 0, int presetFilter = 1, int sortorder = 1)
        {
            List<NeHaulierList> NeHaulierList = new List<NeHaulierList>();
            try
            {
                NeHaulierList = NENNotificationProvider.Instance.GetNeHaulier(pageNo, pageSize, searchString, isVal, presetFilter, sortorder);
                return Content(HttpStatusCode.OK, NeHaulierList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNeHaulier, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Edit NEN User
        [HttpGet]
        [Route("NENNotification/EditNENUser")]
        public IHttpActionResult EditNENUser(long authKeyId)
        {
            List<NeHaulierList> NeHaulierList = new List<NeHaulierList>();
            try
            {
                NeHaulierList = NENNotificationProvider.Instance.EditNENUser(authKeyId);
                return Content(HttpStatusCode.OK, NeHaulierList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/EditNENUser, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Save Ne User
        [HttpPost]
        [Route("NENNotification/SaveNeUser")]
        public IHttpActionResult SaveNeUser(NeHaulierParams objNeHaulierParams)
        {
            int result = 0;
            try
            {
                result = NENNotificationProvider.Instance.SaveNeuse(objNeHaulierParams);
                return Content(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/SaveNeUser, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Enable/Disable User
        [HttpGet]
        [Route("NENNotification/EnableUser")]
        public IHttpActionResult EnableUser(string authKey, long keyId)
        {
            int success = 0;
            try
            {
                success = NENNotificationProvider.Instance.EnableUser(authKey,keyId);
                return Content(HttpStatusCode.OK, success);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/EnableUser, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Haulier Validation
        [HttpGet]
        [Route("NENNotification/HaulierValid")]
        public IHttpActionResult HaulierValid(string haulierName, string organisationName)
        {
            int success = 0;
            try
            {
                success = NENNotificationProvider.Instance.HaulierValid(haulierName, organisationName);
                return Content(HttpStatusCode.OK,success);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/HaulierValid, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetNENRouteList
        [HttpGet]
        [Route("NENNotification/GetNENRouteList")]
        public IHttpActionResult GetNENRouteList(long NENinboxId, int organisationId)
        {
            List<NENUpdateRouteDet> Route_List = new List<NENUpdateRouteDet>();
            try
            {
                Route_List = NENNotificationProvider.Instance.GetNENRouteList(NENinboxId, organisationId);
                return Ok(Route_List);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NENNotification/GetNENRouteList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion


        /// <summary>
        /// functions to get user list
        /// </summary>
        /// <param name="organisationId">Organisation Id</param>
        /// <param name="SOAuserTypeId">SOA user Id</param>
        /// <param name="inBoxId">Inbox Id</param>
        /// <param name="NENId">NEN  Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("NENNotification/GetOrgUserList")]
        public IHttpActionResult GetOrgUserList(long organisationId, int SOAuserTypeId, long inboxId = 0, long NENId = 0)
        {
            try
            {
                List<OrganisationUser> userlist = NENNotificationProvider.Instance.GetOrgUserList(organisationId, SOAuserTypeId, inboxId, NENId);
                return Content(HttpStatusCode.OK, userlist);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetOrgUserList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// SAVENENUSERFORSCRUTINY
        /// </summary>
        /// <param name="movement">parameter is passed as MovementModel class</param>
        /// <returns></returns>
        [HttpPost]
        [Route("NENNotification/SAVENENUSERFORSCRUTINY")]
        public IHttpActionResult SAVENENUSERFORSCRUTINY(MovementModel movement)
        {
            bool result = NENNotificationProvider.Instance.SAVENENUSERFORSCRUTINY(movement);          
            if (result)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region Get NEN Id
        [HttpGet]
        [Route("NENNotification/GetNENId")]
        public IHttpActionResult GetNENId(int notificationNo)
        {
            long NENId = 0;
            try
            {
                NENId = NENNotificationProvider.Instance.GetNENId(notificationNo);
                return Content(HttpStatusCode.OK, NENId);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENId, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetNENSOAReportHistory
        [HttpGet]
        [Route("NENNotification/GetNENSOAReportHistory")]
        public IHttpActionResult GetNENSOAReportHistory(int month, int year, long organisationId)
        {
            List<NENSOAReportModel> ReportList = new List<NENSOAReportModel>();
            try
            {
                ReportList = NENNotificationProvider.Instance.GetNENSOAReportHistory(month, year, organisationId);
                return Content(HttpStatusCode.OK, ReportList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENSOAReportHistory, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetNENHelpdeskReportHistory
        [HttpGet]
        [Route("NENNotification/GetNENHelpdeskReportHistory")]
        public IHttpActionResult GetNENHelpdeskReportHistory(int month, int year, string vehicleCat, int requiresVr1, int vehicleCount)
        {
            List<NENHelpdeskReportModel> ReportList = new List<NENHelpdeskReportModel>();
            try
            {
                ReportList = NENNotificationProvider.Instance.GetNENHelpdeskReportHistory(month, year, vehicleCat, requiresVr1, vehicleCount);
                return Content(HttpStatusCode.OK, ReportList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENSOAReportHistory, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetNENRouteID
        [HttpGet]
        [Route("NENNotification/GetNENRouteID")]
        public IHttpActionResult GetNENRouteID(int NENId, int inboxItemID, int organisationId, char returnVal)
        {
            long Lreturn_val = 0;
            try
            {
                Lreturn_val = NENNotificationProvider.Instance.GetNENRouteID(NENId, inboxItemID, organisationId, returnVal);
                return Content(HttpStatusCode.OK, Lreturn_val);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENRouteID, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region VerifyRouteIdWithOtherOrg
        [HttpGet]
        [Route("NENNotification/VerifyRouteIdWithOtherOrg")]
        public IHttpActionResult VerifyRouteIdWithOtherOrg(int nenId, int organisationId, int routePartId)
        {
            int IsUsing = 0;
            try
            {
                IsUsing = NENNotificationProvider.Instance.VerifyRouteIdWithOtherOrg(nenId, organisationId, routePartId);
                return Content(HttpStatusCode.OK, IsUsing);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/VerifyRouteIdWithOtherOrg, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveNotificationAutoResponseAuditLog
        [HttpPost]
        [Route("NENNotification/SaveNotificationAutoResponseAuditLog")]
        public IHttpActionResult SaveNotificationAutoResponseAuditLog(SaveAutoResponseParams saveAutoResponseParams)
        {
            try
            {
                long result = NENNotificationProvider.Instance.SaveNotificationAutoResponseAuditLog(saveAutoResponseParams.LogMessage, saveAutoResponseParams.UserId, saveAutoResponseParams.OrganisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTApplication/SaveNotifAutoResponseAuditLog,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion


        [HttpGet]
        [Route("NENNotification/GetNENBothRouteStatus")]
        public IHttpActionResult GetNENBothRouteStatus(int inboxId, int NENId, int userId, long organisationId)
        {
            try
            {
                List<NENRouteStatusList> routeStatusLists = NENNotificationProvider.Instance.GetNENBothRouteStatus(inboxId, NENId, userId, organisationId);
                return Content(HttpStatusCode.OK, routeStatusLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENBothRouteStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/GetRouteStatus")]
        public IHttpActionResult GetRouteStatus(int inboxItemId, int NENId, int userId)
        {
            try
            {
                int routeStatus = NENNotificationProvider.Instance.GetRouteStatus(inboxItemId, NENId, userId);
                return Content(HttpStatusCode.OK, routeStatus);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetRouteStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/GetNENRouteDescription")]
        public IHttpActionResult GetNENRouteDescription(int NENId, long inboxId, long organisationId)
        {
            try
            {
                List<NENGeneralDetails> nenRoutedetails = NENNotificationProvider.Instance.GetNENRouteDescription(NENId, inboxId, organisationId);
                return Content(HttpStatusCode.OK, nenRoutedetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENRouteDescription, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/GetRouteFromAndToDescription")]
        public IHttpActionResult GetRouteFromAndToDescription(long routepartId)
        {
            try
            {
                List<NENGeneralDetails> nenRoutedetails = NENNotificationProvider.Instance.GetRouteFromAndToDescp(routepartId);
                return Content(HttpStatusCode.OK, nenRoutedetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetRouteFromAndToDescription, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/GetHualierRouteDescription")]
        public IHttpActionResult GetHualierRouteDescription(int NENId, int inboxItemId, string routeDesc = "", string fromAddress = "", string toAddress = "")
        {
            try
            {
                NENHaulierRouteDesc nenRoutedetails = null;
                if (!string.IsNullOrWhiteSpace(routeDesc) && !string.IsNullOrWhiteSpace(fromAddress) && !string.IsNullOrWhiteSpace(toAddress))
                {
                    nenRoutedetails = new NENHaulierRouteDesc()
                    {
                        HualierGRouteDescription = routeDesc,
                        MainStartAddress = fromAddress,
                        MainEndAddress = toAddress
                    };
                }
                else
                {
                    nenRoutedetails = NENNotificationProvider.Instance.GetHualierRouteDesc(NENId, inboxItemId);
                }
                return Content(HttpStatusCode.OK, nenRoutedetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetHualierRouteDescription, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/GetNENDocumentIdFromInbox")]
        public IHttpActionResult GetNENDocumentIdFromInbox(long NENId, long inboxId, long organisationId)
        {
            long documetnId = 0;
            try
            {
                documetnId = NENNotificationProvider.Instance.GetNENDocumentIdFromInbox(NENId, inboxId, organisationId);
                return Content(HttpStatusCode.OK, documetnId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENDocumentIdFromInbox, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/UpdateInboxTypeFirstTime")]
        public IHttpActionResult UpdateInboxTypeFirstTime(long InboxId, long organisationId)
        {
            int UpdateStatus = 0;
            try
            {
                UpdateStatus = NENNotificationProvider.Instance.UpdateInboxTypeFirstTime(InboxId, organisationId);
                return Content(HttpStatusCode.OK, UpdateStatus);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/UpdateInboxTypeFirstTime, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/GetGeneralDetail")]
        public IHttpActionResult GetGeneralDetail(int NENId, int RouteId)
        {
            NENGeneralDetails GDetail = new NENGeneralDetails();
            try
            {
                GDetail = NENNotificationProvider.Instance.GetGeneralDetail(NENId, RouteId);
                return Content(HttpStatusCode.OK, GDetail);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetGeneralDetail, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("NENNotification/GetNENNotifInboundDet")]
        public IHttpActionResult GetNENNotifInboundDet(long NotifId, long NENId)
        {
            NotificationGeneralDetails objDetails = new NotificationGeneralDetails();
            try
            {
                objDetails = NENNotificationProvider.Instance.GetNENNotifInboundDet(NotifId, NENId);
                return Content(HttpStatusCode.OK, objDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/GetNENNotifInboundDet, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("NENNotification/UpdateRouteStatus")]
        public IHttpActionResult UpdateRouteStatus(NENUpdateParams updateParams)
        {
            int UpdateStatus = 0;
            try
            {
                UpdateStatus = NENNotificationProvider.Instance.UpdateRouteStatus(updateParams.InboxId, updateParams.UserId, updateParams.RouteId, updateParams.RouteStatus, updateParams.OrganisationId);
                return Content(HttpStatusCode.OK, UpdateStatus);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/UpdateRouteStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpPost]
        [Route("NENNotification/InsertInboxEditRouteForNewUser")]
        public IHttpActionResult InsertInboxEditRouteForNewUser(InboxEditRouteParams editRouteParams)
        {
            bool result =false;
            try
            {
                result = NENNotificationProvider.Instance.InsertInboxEditRouteForNewUser(editRouteParams.InboxId, editRouteParams.NENId, editRouteParams.NotificationId, editRouteParams.NewUserId, editRouteParams.EditedRouteId, editRouteParams.NewRouteId, editRouteParams.OrganisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/UpdateRouteStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        
        [HttpGet]
        [Route("NENNotification/GetNENReturnRouteID")]
        public IHttpActionResult GetNENReturnRouteID(int InboxItemId, int orgId)
        {
            long ReturnRouteID = 0;
            try
            {
                ReturnRouteID = NENNotificationProvider.Instance.GetNENReturnRouteID(InboxItemId, orgId);
                return Content(HttpStatusCode.OK, ReturnRouteID);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/UpdateRouteStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("NENNotification/UpdateNENICAStatusInboxItem")]
        public IHttpActionResult UpdateNENICAStatusInboxItem(UpdateNENICAStatusParams updateNENICA)
        {
            int UpdateStatus = 0;
            try
            {
                UpdateStatus = NENNotificationProvider.Instance.UpdateNENICAStatusInboxItem(updateNENICA.InboxId, updateNENICA.IcaStatus, updateNENICA.OrganisationId);
                return Content(HttpStatusCode.OK, UpdateStatus);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/UpdateNENICAStatusInboxItem, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPut]
        [Route("NENNotification/UpdateIcaStatus")]
        public IHttpActionResult UpdateNenApiIcaStatus(UpdateNENICAStatusParams updateNENICA)
        {
            int UpdateStatus = 0;
            try
            {
                UpdateStatus = NENNotificationProvider.Instance.UpdateNenApiIcaStatus(updateNENICA);
                return Content(HttpStatusCode.OK, UpdateStatus);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENNotification/UpdateNENICAStatusInboxItem, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

    }
}
