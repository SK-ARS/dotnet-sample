using STP.Common.Logger;
using STP.Domain;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.NonESDAL;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public class NENNotificationService : INENNotificationService
    {
        private readonly HttpClient httpClient;
        public NENNotificationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region public Object GetNeHaulier()
        public List<NeHaulierList> GetNeHaulier(int pageNo, int pageSize, string searchString, int isVal, int presetFilter,int sortorder)
        {
            List<NeHaulierList> objNeHaulierList = new List<NeHaulierList>();
            try
            {
                string urlParameters = "?pageNo=" + pageNo + "&pageSize=" + pageSize + "&searchString=" + searchString + "&isVal=" + isVal + "&presetFilter=" + presetFilter + "&sortorder=" + sortorder;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNeHaulier{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objNeHaulierList = response.Content.ReadAsAsync<List<NeHaulierList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNeHaulier, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNeHaulier, Exception: {ex}");
            }
            return objNeHaulierList;
        }
        #endregion
        #region public Object EditNeUser()
        public List<NeHaulierList> EditNeUser(long AuthKeyId)
        {
            List<NeHaulierList> objNeHaulierList = new List<NeHaulierList>();
            try
            {
                string urlParameters = "?AuthKeyId=" + AuthKeyId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/EditNENUser{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objNeHaulierList = response.Content.ReadAsAsync<List<NeHaulierList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/EditNeUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/EditNeUser, Exception: {ex}");
            }
            return objNeHaulierList;
        }
        #endregion
        #region public int SaveNeUser()
        public int SaveNeUser(string Haulname, string authKey, string OrgName, long NeLimit, long KeyId)
        {
            int flag = 0;
            try
            {
                NeHaulierParams objProjectFolderModelParams = new NeHaulierParams
                {
                    HaulierName = Haulname,
                    AuthenticationKey = authKey,
                    OrganisationName = OrgName,
                    NeLimit = NeLimit,
                    KeyId = KeyId
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/NENNotification/SaveNeUser",
                    objProjectFolderModelParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    flag = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/SaveNeUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/SaveNeUser, Exception: {ex}");
            }
            return flag;
        }
        #endregion
        #region public int EnableUser()
        public int EnableUser(string authKey, long keyId)
        {
            int success = 0;
            try
            {
                string urlParameters = "?authKey=" + authKey + "&keyId=" + keyId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/EnableUser{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    success = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/EnableUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/EnableUser, Exception: {ex}");
            }
            return success;
        }
        #endregion

        #region public int  HaulierValidation()
        public int HaulierValid(string haulierName, string organisationName)
        {
            int success = 0;
            try
            {
                string urlParameters = "?haulierName=" + haulierName + "&organisationName=" + organisationName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/HaulierValid{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    success = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/HaulierValid, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/HaulierValid, Exception: {ex}");
            }
            return success;
        }
        #endregion
        #region public Object GetNENRouteList()
        public List<NENUpdateRouteDet> GetNENRouteList(long NENInboxId, int organisationId)
        {
            List<NENUpdateRouteDet> Route_List = new List<NENUpdateRouteDet>();
            try
            {
                string urlParameters = "?NENinboxId=" + NENInboxId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENRouteList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    Route_List = response.Content.ReadAsAsync<List<NENUpdateRouteDet>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENRouteList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENRouteList, Exception: {ex}");
            }
            return Route_List;
        }
        #endregion        
        
        public List<OrganisationUser> GetOrg_UserList(long OrgID, int SOA_UserTypeID, long inBoxId = 0, long NEN_ID = 0)
        {
            List<OrganisationUser> result = new List<OrganisationUser>();
            try
            {
                string urlParameters = "?organisationId=" + OrgID + "&SOAuserTypeId=" + SOA_UserTypeID + "&inboxId=" + inBoxId + "&NENId=" + NEN_ID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetOrgUserList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<OrganisationUser>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetOrg_UserList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetOrg_UserList, Exception: {ex}");
            }           
            return result;
        }
        public bool SP_SAVE_NEN_USER_FOR_SCRUTINY(MovementModel movement)
        {
            bool result = false;
            try
            {
                 HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/NENNotification/SAVENENUSERFORSCRUTINY", movement).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/SAVENENUSERFORSCRUTINY, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/SAVENENUSERFORSCRUTINY, Exception: {ex}");
            }
            return result;
        }

        #region public int EnableUser()
        public long GetNENId(int notificationNo)
        {
            long success = 0;
            try
            {
                string urlParameters = "?notificationNo=" + notificationNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENId{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    success = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENId, Exception: {ex}");
            }
            return success;
        }
        #endregion
        #region public Object GetNENSOAReportHistory()
        public List<NENSOAReportModel> GetNENSOAReportHistory(int month, int year, long orgId)
        {
            List<NENSOAReportModel> ReportList = new List<NENSOAReportModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year + "&organisationId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENSOAReportHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    ReportList = response.Content.ReadAsAsync<List<NENSOAReportModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENSOAReportHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENSOAReportHistory, Exception: {ex}");
            }
            return ReportList;
        }
        #endregion
        #region public Object GetNENHelpdeskReportHistory()
        public List<NENHelpdeskReportModel> GetNENHelpdeskReportHistory(int month, int year, string vehicleCat, int requiresVr1, int vehicleCount)
        {
            List<NENHelpdeskReportModel> objHelpdeskReport = new List<NENHelpdeskReportModel>();
            try
            {
                string urlParameters = "?month=" + month + "&year=" + year + "&vehicleCat=" + vehicleCat + "&requiresVr1=" + requiresVr1 + "&vehicleCount=" + vehicleCount;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENHelpdeskReportHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objHelpdeskReport = response.Content.ReadAsAsync<List<NENHelpdeskReportModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENHelpdeskReportHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENHelpdeskReportHistory, Exception: {ex}");
            }
            return objHelpdeskReport;
        }
        #endregion
        #region public long GetNENRouteID()
        public long GetNENRouteID(int NENId, int inboxItemID, int organisationId, char returnVal)
        {
            long Lreturn_val = 0;
            try
            {
                string urlParameters = "?NENId=" + NENId + "&inboxItemID=" + inboxItemID + "&organisationId=" + organisationId + "&returnVal=" + returnVal;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"NENNotification/GetNENRouteID, Starting the GetNENRouteID call for NENId=" + NENId);
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENRouteID{urlParameters}").Result;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"NENNotification/GetNENRouteID, Starting the GetNENRouteID call for NENId=" + NENId);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    Lreturn_val = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENRouteID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENRouteID, Exception: {ex}");
            }
            return Lreturn_val;
        }
        #endregion
        #region public int VerifyRouteIdWithOtherOrg()
        public int VerifyRouteIdWithOtherOrg(int NENId, int organisationId, int routePartId)
        {
            int IsUsing = 0;
            try
            {
                string urlParameters = "?NENId=" + NENId + "&organisationId=" + organisationId + "&routePartId=" + routePartId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"NENNotification/VerifyRouteIdWithOtherOrg, Starting the VerifyRouteIdWithOtherOrg call for organisationId=" + organisationId);
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/VerifyRouteIdWithOtherOrg{urlParameters}").Result;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"NENNotification/VerifyRouteIdWithOtherOrg, Starting the VerifyRouteIdWithOtherOrg call for organisationId=" + organisationId);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    IsUsing = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/VerifyRouteIdWithOtherOrg, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/VerifyRouteIdWithOtherOrg, Exception: {ex}");
            }
            return IsUsing;
        }
        #endregion

        public long SaveNotificationAutoResponseAuditLog(SaveAutoResponseParams saveAutoResponseParams)
        {
            long result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/SaveNotificationAutoResponseAuditLog", saveAutoResponseParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/SaveNotificationAutoResponseAuditLog, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/SaveNotificationAutoResponseAuditLog, Exception: {ex}");
            }
            return result;
        }

        public List<NENRouteStatusList> GetNENBothRouteStatus(int inboxId, int NENId, int userId, long organisationId)
        {
            List<NENRouteStatusList> objList = new List<NENRouteStatusList>();
            try
            {
                string urlParameters = "?inboxId=" + inboxId + "&NENId=" + NENId + "&userId="+ userId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENBothRouteStatus{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objList = response.Content.ReadAsAsync<List<NENRouteStatusList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENBothRouteStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetNENBothRouteStatus, Exception: {ex}");
            }
            return objList;
        }

        public int GetRouteStatus(int inboxItemId, int NENId, int userId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?inboxItemId=" + inboxItemId + "&NENId=" + NENId + "&userId=" + userId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetRouteStatus{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetRouteStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetRouteStatus, Exception: {ex}");
            }
            return result;
        }

        public List<NENGeneralDetails> GetNENRouteDescription(int NENId, long inboxId, long organisationId)
        {
            List<NENGeneralDetails> objList = new List<NENGeneralDetails>();
            try
            {
                string urlParameters = "?NENId=" + NENId + "&inboxId=" + inboxId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENRouteDescription{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objList = response.Content.ReadAsAsync<List<NENGeneralDetails>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENRouteDescription, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetNENRouteDescription, Exception: {ex}");
            }
            return objList;
        }

        public List<NENGeneralDetails> GetRouteFromAndToDescp(long routepartId)
        {
            List<NENGeneralDetails> objList = new List<NENGeneralDetails>();
            try
            {
                string urlParameters = "?routepartId=" + routepartId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetRouteFromAndToDescription{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objList = response.Content.ReadAsAsync<List<NENGeneralDetails>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetRouteFromAndToDescription, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetRouteFromAndToDescp, Exception: {ex}");
            }
            return objList;
        }

        public NENHaulierRouteDesc GetHualierRouteDesc(int NENId, int inboxItemId, string routeDesc = "", string fromAddress = "", string toAddress = "")
        {
            NENHaulierRouteDesc objresult = new NENHaulierRouteDesc();
            try
            {
                string urlParameters = "?NENId=" + NENId + "&inboxItemId=" + inboxItemId + "&routeDesc=" + routeDesc + "&fromAddress=" + fromAddress + "&toAddress=" + toAddress;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetHualierRouteDescription{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objresult = response.Content.ReadAsAsync<NENHaulierRouteDesc>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetHualierRouteDescription, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetHualierRouteDesc, Exception: {ex}");
            }
            return objresult;
        }

        public long GetNENDocumentIdFromInbox(long NENId, long inboxId, long organisationId)
        {
            long documetnId = 0;
            try
            {
                string urlParameters = "?NENId=" + NENId + "&inboxId=" + inboxId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENDocumentIdFromInbox{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    documetnId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENDocumentIdFromInbox, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetNENDocumentIdFromInbox, Exception: {ex}");
            }
            return documetnId;
        }

        public int UpdateInboxTypeFirstTime(long InboxId, long organisationId)
        {
            int UpdateStatus = 0;
            try
            {
                string urlParameters = "?InboxId=" + InboxId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/UpdateInboxTypeFirstTime{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    UpdateStatus = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/UpdateInboxTypeFirstTime, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/UpdateInboxTypeFirstTime, Exception: {ex}");
            }
            return UpdateStatus;
        }

        public NENGeneralDetails GetGeneralDetail(int NENId, int RouteId)
        {
            NENGeneralDetails GDetail = new NENGeneralDetails();
            try
            {
                string urlParameters = "?NENId=" + NENId + "&RouteId=" + RouteId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetGeneralDetail{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    GDetail = response.Content.ReadAsAsync<NENGeneralDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetGeneralDetail, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetGeneralDetail, Exception: {ex}");
            }
            return GDetail;
        }

        public NotificationGeneralDetails GetNENNotifInboundDet(long NotifId, long NENId)
        {
            NotificationGeneralDetails objDetails = new NotificationGeneralDetails();
            try
            {
                string urlParameters = "?NotifId=" + NotifId + "&NENId=" + NENId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENNotifInboundDet{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objDetails = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENNotifInboundDet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetNENNotifInboundDet, Exception: {ex}");
            }
            return objDetails;
        }

        public int UpdateRouteStatus(int InboxId, int UserId, int RouteId, int RouteStatus, long OrganisationId)
        {
            int result = 0;
            try
            {
                NENUpdateParams updateParams = new NENUpdateParams
                {
                    InboxId= InboxId,
                    UserId= UserId,
                    RouteId = RouteId,
                    RouteStatus = RouteStatus,
                    OrganisationId = OrganisationId
                };
                
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/UpdateRouteStatus", updateParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/UpdateRouteStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/UpdateRouteStatus, Exception: {ex}");
            }
            return result;
        }

        public bool InsertInboxEditRouteForNewUser(int InboxId, long NENId, int NotificationId, int NewUserId, long EditedRouteId, long NewRouteId, long OrganisationId)
        {
            bool result = false;
            try
            {
                InboxEditRouteParams editRouteParams = new InboxEditRouteParams
                {
                    InboxId = InboxId,
                    NENId = NENId,
                    NotificationId = NotificationId,
                    NewUserId = NewUserId,
                    EditedRouteId = EditedRouteId,
                    NewRouteId = NewRouteId,
                    OrganisationId = OrganisationId
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/InsertInboxEditRouteForNewUser", editRouteParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/InsertInboxEditRouteForNewUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/InsertInboxEditRouteForNewUser, Exception: {ex}");
            }
            return result;
        }

        public long GetNENReturnRouteID(int InboxItemId, int orgId)
        {
            long ReturnRouteID = 0;
            try
            {
                string urlParameters = "?InboxItemId=" + InboxItemId + "&orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/GetNENReturnRouteID{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    ReturnRouteID = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/GetNENReturnRouteID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/GetNENReturnRouteID, Exception: {ex}");
            }
            return ReturnRouteID;
        }

        public int UpdateNENICAStatusInboxItem(int InboxId, int IcaStatus, long OrganisationId)
        {
            int result = 0;
            try
            {
                UpdateNENICAStatusParams updateNENICA = new UpdateNENICAStatusParams
                {
                    InboxId=InboxId,
                    IcaStatus=IcaStatus,
                    OrganisationId=OrganisationId
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/NENNotification/UpdateNENICAStatusInboxItem", updateNENICA).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/UpdateNENICAStatusInboxItem, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotificationService/UpdateNENICAStatusInboxItem, Exception: {ex}");
            }
            return result;
        }

    }
}