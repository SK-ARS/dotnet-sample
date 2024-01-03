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
using System.Web;

namespace STP.ServiceAccess.LoggingAndReporting
{
   public class LoggingService : ILoggingService
    {
        private readonly HttpClient httpClient;

        public LoggingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region public Object SaveMovementAction()
        public long SaveMovementAction(string esdalRef, int movementActionType, string movementDescription,long projectid,int revisionNo,int versionNo, string userSchema)
        {
            long result = 0;
           
            try
            {
                SaveMovementActionParam saveMovementActionParam = new SaveMovementActionParam()
                {
                    esdalRef = esdalRef,
                    movementActionType = movementActionType,
                    movementDescription = movementDescription,
                    projectId = projectid,
                    revisionNo = revisionNo,
                    versionNo = versionNo,
                    userSchema = userSchema
                };
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SaveMovementAction, Error: {"1-" + esdalRef + ",2-" + movementActionType + ",3-" + movementDescription + ",4-" + userSchema}");
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                $"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                $"/Logging/SaveMovementAction",
               saveMovementActionParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/SaveMovementAction, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/SaveMovementAction, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region SaveSysEvents
        public bool SaveSysEventsMovement(int SystemEventType, string SysDescrp, int userid, string userschema)
        {
            bool result = false;
            try
            {
               
                //api call to new service             
                string urlParameters = "?SystemEventType=" + SystemEventType + "&SysDescrp=" + SysDescrp + "&userid=" + userid + "&userschema=" + userschema ;
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" + $"/Logging/SaveSysEvents"+ urlParameters,null).Result;

              
                
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Logging/SaveSysEventsMovement, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Logging/SaveSysEventsMovements, Exception: {0}", ex);

            }
            return result;
        }
        #endregion

        #region public Object GetAuditListSearch()
        public List<NENAuditLogList> GetAuditListSearch(string searchString, int pageNum, int pageSize, int sortFlag, long organisationId,int searchNotificationSource, string searchType = "0", int presetFilter=1, int? sortOrder = null)
        {
            List<NENAuditLogList> auditList = new List<NENAuditLogList>();
            try
            {
                string searchString_encoded = HttpUtility.UrlEncode(searchString); //contain spl character #
                string urlParameters = "?searchString=" + searchString_encoded + "&pageNo=" + pageNum + "&pageSize=" + pageSize + "&sortFlag=" + sortFlag + "&organisationId=" + organisationId + "&searchType=" + searchType + "&searchNotificationSource=" + searchNotificationSource + "&presetFilter=" + presetFilter + "&sortOrder=" + sortOrder;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                              $"/Logging/GetAuditListSearch{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    auditList = response.Content.ReadAsAsync<List<NENAuditLogList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"LoggingService/GetAuditListSearch, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"LoggingService/GetAuditListSearch, Exception: {ex}");
            }
            return auditList;
        }
        #endregion

        #region public Object GetAuditlogNEN()
        public List<NENAuditGridList> GetAuditlogNEN(int? page, int? pageSize, string NENnotificationNo, long organisationId,int? sortOrder,int? sortType)
        {
            List<NENAuditGridList> auditList = new List<NENAuditGridList>();
            try
            {
                AuditLogParams auditLogParams = new AuditLogParams()
                {
                    Page = page,
                    PageSize= pageSize,
                    NENNotificationNo = NENnotificationNo,
                    OrganisationId = organisationId,
                    sortOrder=sortOrder,
                    sortType=sortType
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                           $"/Logging/GetAuditlogNEN",
                           auditLogParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    auditList = response.Content.ReadAsAsync<List<NENAuditGridList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"LoggingService/GetAuditlogNEN, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"LoggingService/GetAuditlogNEN, Exception: {ex}");
            }
            return auditList;
        }
        #endregion

        public List<NotificationHistory> GetNotificationHistory(int pageNumber, int pageSize, long notificationNo, int sortOrder, int sortType, int historic, int userType=0, long projectId = 0)
        {
            List<NotificationHistory> notificationList = new List<NotificationHistory>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&notificationNo=" + notificationNo + "&sortOrder=" + sortOrder + "&sortType=" + sortType + "&historic=" + historic + "&userType=" + userType + "&projectId=" + projectId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" +
                              $"/Logging/GetNotificationHistory{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    notificationList = response.Content.ReadAsAsync<List<NotificationHistory>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"LoggingService/GetNotificationHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"LoggingService/GetNotificationHistory, Exception: {ex}");
            }
            return notificationList;
        }
    }
}
