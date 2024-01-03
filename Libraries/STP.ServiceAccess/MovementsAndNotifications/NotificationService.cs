using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web;
using STP.Domain.Communications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.NonESDAL;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient httpClient;
        public NotificationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region View Distribution Status - Display Notification

        #region public int InsertQuickLink()
        public int InsertQuickLink(int userId, int projectId, int notificationId, int revisionId, int versionId)
        {
            int linkno = 0;
            try
            {
                InsertQuickLinkParams objInsertQuickLinkParams = new InsertQuickLinkParams
                {
                    UserId = userId,
                    ProjectId = projectId,
                    NotificationId = notificationId,
                    RevisionId = revisionId,
                    VersionId = versionId
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Notification/InsertQuickLink",
                    objInsertQuickLinkParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    linkno = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/InsertQuickLink, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/InsertQuickLink, Exception: {ex}");
            }
            return linkno;
        }
        #endregion

        #region public bool IsAcknowledged()
        public bool IsAcknowledged(string esdalRefernce, int historic)
        {
            bool result = true;
            try
            {
                string esdal_encoded = HttpUtility.UrlEncode(esdalRefernce); //contain spl character #
                string urlParameters = "?esdalRefernce=" + esdal_encoded + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/Notification/IsAcknowledged{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/IsAcknowledged, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/IsAcknowledged, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region public int CheckIfNotificationSubmitted()
        public string CheckIfNotificationSubmitted(int NotificationId)
        {
            string notif_code = "";
            try
            {
                string urlParameters = "?notificationId=" + NotificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/CheckNotificationSubmitted{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    notif_code = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckIfNotificationSubmitted, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckIfNotificationSubmitted, Exception: {ex}");
            }
            return notif_code;
        }
        #endregion

        #region public Object GetUnacknowledgedCollaboration()
        public CollaborationModel GetUnacknowledgedCollaboration(string Notification_Code, int historic)
        {
            CollaborationModel objCollaborationModel = new CollaborationModel();
            try
            {
                string esdal_encoded = HttpUtility.UrlEncode(Notification_Code); //contain spl character #
                string urlParameters = "?notificationCode=" + esdal_encoded + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetUnacknowledgedCollaboration{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objCollaborationModel = response.Content.ReadAsAsync<CollaborationModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetUnacknowledgedCollaboration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetUnacknowledgedCollaboration, Exception: {ex}");
            }
            return objCollaborationModel;
        }
        #endregion

        #region public Object GetNotificationGeneralDetail()
        public NotificationGeneralDetails GetNotificationGeneralDetail(long notificationId, int historic)
        {
            NotificationGeneralDetails objNotificationGeneralDetails = new NotificationGeneralDetails();
            try
            {
                string urlParameters = "?NotificationID=" + notificationId + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetNotificationGeneralDetail{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objNotificationGeneralDetails = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetNotificationGeneralDetail, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetNotificationGeneralDetail, Exception: {ex}");
            }
            return objNotificationGeneralDetails;
        }
        #endregion
        
        #region  public DeleteNotification()
        public int DeleteNotification(int notificationId)
        {
            int result = 0;
            string urlParameters = "?notificationId=" + notificationId;
            try
            {
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/DeleteNotification{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/DeleteNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/DeleteNotification, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #endregion

        #region  public byte[] GetAffectedParties()
        public byte[] GetAffectedParties(int notificationId, string userSchema = UserSchema.Portal)
        {
            byte[] result = null;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                                                                           + $"/Notification/GetAffectedParties?notificationId=" + notificationId + "&userSchema=" + userSchema).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetAffectedParties, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetAffectedParties, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  public byte[] GetResponseMailDetails()
        public MailResponse GetResponseMailDetails(int orgId, string userSchema = UserSchema.Portal)
        {
            MailResponse result = new MailResponse();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                                                                           + $"/Notification/GetResponseMailDetails?organisationId=" + orgId + "&userSchema=" + userSchema).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<MailResponse>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetResponseMailDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetResponseMailDetails, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region ShowImminentMovement
        public int ShowImminentMovement(string moveStartDate, string countryId, int countryIdCount, int vehicleClass)
        {
            int result = 0;
            try
            {
                string urlParameters = "?moveStartDate=" + moveStartDate + "&countryId=" + countryId + "&countryIdCount=" + countryIdCount + "&vehicleClass=" + vehicleClass;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/ShowImminentMovement{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/ShowImminentMovement, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/ShowImminentMovement, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  public bool SaveAffectedNotificationDetails()
        public bool SaveAffectedNotificationDetails(AffectedStructConstrParam affectedParam)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                                                                           + $"/Notification/SaveAffectedNotificationDetails", affectedParam).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/SaveAffectedNotificationDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/SaveAffectedNotificationDetails, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region UpdateNotification
        public int UpdateNotification(NotificationGeneralDetails notificationGeneralDetails)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/UpdateNotification", notificationGeneralDetails).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/UpdateNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/UpdateNotification, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region Notify Application
        public NotificationGeneralDetails NotifyApplication(long versionId)
        {
            NotificationGeneralDetails objNotify = new NotificationGeneralDetails();
            string urlParameters = "?versionId=" + versionId;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                + $"/Notification/NotifyApplication{urlParameters}").Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                objNotify = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Notification/NotifyApplication, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return objNotify;
        }
        #endregion

        #region Get Internal Collaboration
        public NotificationStatusModel GetInternalCollaboration(NotificationStatusModel notificationStatusModel, string userSchema)
        {
            NotificationStatusModel result = new NotificationStatusModel();
            notificationStatusModel.userSchema = userSchema;
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                + $"/Notification/GetInternalCollaboration", notificationStatusModel).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<NotificationStatusModel>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Notification/GetInternalCollaboration, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region Manage Collaboration Internal
        public bool ManageCollaborationInternal(NotificationStatusModel notificationStatusModel, string userSchema)
        {
            bool result = false;
            notificationStatusModel.userSchema = userSchema;
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                + $"/Notification/ManageCollaborationInternal", notificationStatusModel).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Notification/ManageCollaborationInternal, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region CheckNotificationVersion
        public int CheckNotificationVersion(int NotificationId)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/CheckNotificationVersion?notificationId=" + NotificationId).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckNotificationVersion, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckNotificationVersion, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GenerateNotificationCode
        public string GenerateNotificationCode(int OrgId, long NotificationId, int Detail)
        {
            string result = string.Empty;
            try
            {
                string urlParameters = "?organisationId=" + OrgId + "&notificationId=" + NotificationId + "&detail=" + Detail;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/GenerateNotificationCode{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GenerateNotificationCode, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GenerateNotificationCode, Exception: {ex}");
            }
            return result;
        }
        #endregion        

        #region UpdateCollborationAcknowledgement
        public bool UpdateCollborationAck(long docId, int colNo, int userId, string acknowledgeAgainst, int historic)
        {
            bool result = true;
            try
            {
                string urlParameters = "?docId=" + docId + "&colNo=" + colNo + "&userId=" + userId + "&acknowledgeAgainst=" + acknowledgeAgainst + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                              $"/Notification/UpdateCollborationAcknowledgement{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/UpdateCollborationAcknowledgement, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/UpdateCollborationAcknowledgement, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetCollaborationList
        public List<CollaborationModel> GetCollaborationList(int pageNumber, int pageSize, string notificationCode, int notificationId, int historic)
        {
            List<CollaborationModel> result = new List<CollaborationModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&notificationCode=" + notificationCode + "&notificationId=" + notificationId + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetCollaborationList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<CollaborationModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetCollaborationList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetCollaborationList, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetNotificationStatusList
        public List<NotificationStatusModel> GetNotificationStatusList(int pageNumber, int pageSize, string NotificationCode, string userSchema)
        {
            List<NotificationStatusModel> result = new List<NotificationStatusModel>();
            try
            {

                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&NotificationCode=" + NotificationCode + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetNotificationStatusList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<NotificationStatusModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetNotificationStatusList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetNotificationStatusList, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region RenotifyNotification
        public NotificationGeneralDetails RenotifyNotification(int notifId, int VR1)
        {
            NotificationGeneralDetails result = new NotificationGeneralDetails();
            try
            {
                string urlParameters = "?notificationId=" + notifId + "&VR1=" + VR1;
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/RenotifyNotification" + urlParameters, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/RenotifyNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/RenotifyNotification, Exception: {ex}");
            }
            return result;
        }
        #endregion
        
        #region CloneNotification
        public NotificationGeneralDetails CloneNotification(int notificationId)
        {
            NotificationGeneralDetails result = new NotificationGeneralDetails();
            try
            {
                string urlParameters = "?notificationId=" + notificationId;
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/CloneNotification" + urlParameters, null).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/CloneNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/CloneNotification, Exception: {ex}");
            }
            return result;
        }
        public NotificationGeneralDetails CloneHistoricNotification(int notificationId)
        {
            NotificationGeneralDetails result = new NotificationGeneralDetails();
            try
            {
                string urlParameters = "?notificationId=" + notificationId;
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/CloneHistoricNotification" + urlParameters, null).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/CloneHistoricNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/CloneHistoricNotification, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetTransmissionList
        public List<TransmissionModel> GetTransmissionList(GetTransmissionListParams getTransmissionList)
        {
            List<TransmissionModel> result = new List<TransmissionModel>();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetTransmissionList", getTransmissionList).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<TransmissionModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetTransmissionList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetTransmissionList, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetExternalCollaboration
        public List<CollaborationModel> GetExternalCollaboration(int pageNumber, int pageSize, int Document_Id, string userSchema)
        {
            List<CollaborationModel> result = new List<CollaborationModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&Document_Id=" + Document_Id + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetExternalCollaboration{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<CollaborationModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetExternalCollaboration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetExternalCollaboration, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetInboxSubContent
        public List<InboxSubContent> GetInboxSubContent(int pageNumber, int pageSize, int versionId, int orgId, int notifhistory)
        {
            List<InboxSubContent> objInbox = new List<InboxSubContent>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&versionId=" + versionId + "&orgId=" + orgId + "&notifhistory=" + notifhistory;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetInboxSubContent{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objInbox = response.Content.ReadAsAsync<List<InboxSubContent>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetInboxSubContent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetInboxSubContent, Exception: {ex}");
            }
            return objInbox;
        }
        #endregion

        #region GetSORTHistoryDetails
        public List<InboxSubContent> GetSORTHistoryDetails(string esdalref, int versionno)
        {
            List<InboxSubContent> objInbox = new List<InboxSubContent>();
            try
            {
                string urlParameters = "?esdalref=" + esdalref + "&versionno=" + versionno;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetSORTHistoryDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objInbox = response.Content.ReadAsAsync<List<InboxSubContent>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetSORTHistoryDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetInboxSubContent, Exception: {ex}");
            }
            return objInbox;
        }
        #endregion

        #region InsertApplicationType
        public NotifGeneralDetails InsertNotificationType(PlanMovementType saveNotification)
        {
            NotifGeneralDetails notifGeneral = new NotifGeneralDetails();
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/Notification/InsertNotificationType", saveNotification).Result;
            if (response.IsSuccessStatusCode)
            {
                notifGeneral = response.Content.ReadAsAsync<NotifGeneralDetails>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Notification/InsertNotificationType, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return notifGeneral;
        }
        #endregion

        #region UpdateNotificationType
        public NotifGeneralDetails UpdateNotificationType(PlanMovementType updateNotification)
        {
            NotifGeneralDetails notifGeneral = new NotifGeneralDetails();
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/Notification/UpdateNotificationType", updateNotification).Result;
            if (response.IsSuccessStatusCode)
            {
                notifGeneral = response.Content.ReadAsAsync<NotifGeneralDetails>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Notification/InsertNotificationType, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return notifGeneral;
        }
        #endregion

        #region SetLoginStatus
        public int SetLoginStatus(int UserId, int flag)
        {
            int result = 0;
            try
            {
                string urlParameters = "?UserId=" + UserId + "&flag=" + flag;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/SetLoginStatus{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/SetLoginStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/SetLoginStatus, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetHaulierDetails
        public HAContact GetHaulierDetails(long notificationId)
        {
            HAContact contact = new HAContact();
            try
            {
                string urlParameters = "?notificationID=" + notificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/GetHaulierDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    contact = response.Content.ReadAsAsync<HAContact>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetHaulierDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetHaulierDetails, Exception: {ex}");
            }
            return contact;
        }
        #endregion

        public void UpdateNenApiIcaStatus(UpdateNENICAStatusParams updateNENICAStatusParams)
        {
            try
            {
                HttpResponseMessage response = httpClient.PutAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/NENNotification/UpdateIcaStatus", updateNENICAStatusParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/UpdateIcaStatus, {response.Content.ReadAsAsync<int>().Result}");
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/UpdateIcaStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NENNotification/UpdateIcaStatus, Exception: {ex}");
            }
        }

        #region Code commented By Mahzeer on 04-12-2023

        #region  public byte[] GetInboundNotification()
        /*public byte[] GetInboundNotification(int notificationId)
        {
            byte[] result = null;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                                                                           + $"/Notification/GetInboundNotification?notificationId=" + notificationId).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetInboundNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetInboundNotification, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region CheckImminent
        /*public int CheckImminent(int vehicleclass, decimal vehiWidth, decimal vehiLength, decimal rigidLength, decimal GrossWeight, int WorkingDays, decimal FrontPRJ, decimal RearPRJ, decimal LeftPRJ, decimal RightPRJ, GetImminentChkDetailsDomain objImminent, string Notif_type = null)
        {
            int result = 0;
            try
            {
                CheckImminentParam checkImminentParam = new CheckImminentParam()
                {
                    VehicleClass = vehicleclass,
                    VehicleWidth = vehiWidth,
                    VehicleLength = vehiLength,
                    RigidLength = rigidLength,
                    GrossWeight = GrossWeight,
                    WorkingDays = WorkingDays,
                    FrontPRJ = FrontPRJ,
                    RearPRJ = RearPRJ,
                    LeftPRJ = LeftPRJ,
                    RightPRJ = RightPRJ,
                    ImminentCheckDetails = objImminent,
                    NotificationType = Notif_type
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/CheckImminent", checkImminentParam).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckImminent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckImminent, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region GetDetailsToChkImminent
        /*public GetImminentChkDetailsDomain GetDetailsToChkImminent(long notificationId, string Content_Ref_No, long revision_id, string UserSchema)
        {
            GetImminentChkDetailsDomain result = new GetImminentChkDetailsDomain();
            try
            {
                string urlParameters = "?notificationId=" + notificationId + "&contentReferenceNo=" + Content_Ref_No + "&revisionId=" + revision_id + "&userSchema=" + UserSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetDetailsToChkImminent{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<GetImminentChkDetailsDomain>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetDetailsToChkImminent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetDetailsToChkImminent, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region Update Inbound Notification
        /*public int UpdateInboundNotif(int notificationId, byte[] inboundNotif)
        {
            int result = 0;
            UpdateInboundNotifParam updateInboundNotif = new UpdateInboundNotifParam
            {
                NotificationId = notificationId,
                InboundNotification = inboundNotif
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                + $"/Notification/UpdateInboundNotif", updateInboundNotif).Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = 1;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Notification/UpdateInboundNotif, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }*/
        #endregion

        #region GetVehicleTypeForNotification
        /*public int GetVehicleTypeForNotif(int notificationId)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetVehicleTypeForNotification?notificationId=" + notificationId).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetVehicleTypeForNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetVehicleTypeForNotification, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region ListCloneAxelDetails
        /*public List<AxleDetails> ListCloneAxelDetails(int vehicleId)
        {
            List<AxleDetails> result = new List<AxleDetails>();
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/ListCloneAxelDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<AxleDetails>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/ListCloneAxelDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/ListCloneAxelDetails, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region  public IsNotifSubmitCheck()
        /*public int IsNotifSubmitCheck(long notificationID)
        {
            int result = 0;
            try
            {
                string urlParameters = "?notificationID=" + notificationID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/IsNotifSubmitCheck{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/IsNotifSubmitCheck, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/IsNotifSubmitCheck, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region  public NotificationGeneralDetails CheckNotifValidation()
        /* public NotificationGeneralDetails CheckNotifValidation(string contentReferenceNo)
         {
             NotificationGeneralDetails result = new NotificationGeneralDetails();
             try
             {
                 HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                                                                            + $"/Notification/CheckNotifValidation?contentReferenceNo=" + contentReferenceNo).Result;
                 if (response.IsSuccessStatusCode)
                 {
                     // Parse the response body.
                     result = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
                 }
                 else
                 {
                     //do exception handling here
                     Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckNotifValidation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                 }
             }
             catch (Exception ex)
             {
                 Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/CheckNotifValidation, Exception: {ex}");
             }
             return result;
         }*/
        #endregion

        #region  public NotificationGeneralDetails SaveNotifGeneralDetails()
        /*public NotificationGeneralDetails SaveNotifGeneralDetails(NotificationGeneralDetails notificationGeneralDetails)
        {
            NotificationGeneralDetails result = new NotificationGeneralDetails();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/SaveNotifGeneralDetails", notificationGeneralDetails).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<NotificationGeneralDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/SaveNotifGeneralDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/SaveNotifGeneralDetails, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region  public UserRegistration GetNotifHaulierDetails()
        /*public UserRegistration GetNotifHaulierDetails(int userId, int notificationId = 0, int vehicleClassCode = 0)
        {
            UserRegistration getNotifHaulierDetails = new UserRegistration();
            try
            {
                string urlParameters = "?userId=" + userId + "&notificationId=" + notificationId + "&vehicleClassCode=" + vehicleClassCode;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/GetNotifHaulierDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    getNotifHaulierDetails = response.Content.ReadAsAsync<UserRegistration>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetNotifHaulierDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetNotifHaulierDetails, Exception: {ex}");
            }
            return getNotifHaulierDetails;
        }*/
        #endregion

        #region  public bool SetNotificationVRNum()
        /*public bool SetNotificationVRNum(int notificationId)
        {
            bool res = false;
            try
            {
                string urlParameters = "?notificationId=" + notificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/SetNotificationVRNum{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/SetNotificationVRNum, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/SetNotificationVRNum, Exception: {ex}");
            }
            return res;
        }*/
        #endregion

        #region  public string GetHaulierLicence()
        /*public string GetHaulierLicence(int orgId)
        {
            string haulierLicence = "";
            try
            {
                string urlParameters = "?orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/GetHaulierLicence{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    haulierLicence = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetHaulierLicence, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetHaulierLicence, Exception: {ex}");
            }
            return haulierLicence;
        }*/
        #endregion

        #region  public object GetNotificationRouteDetails()
        /*public List<ListRouteVehicleId> GetNotificationRouteDetails(string ContentRefNo)
        {
            List<ListRouteVehicleId> objNotifRouteDetails = new List<ListRouteVehicleId>();
            try
            {
                string urlParameters = "?ContentRefNo=" + ContentRefNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/GetNotifcationRouteDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objNotifRouteDetails = response.Content.ReadAsAsync<List<ListRouteVehicleId>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetNotificationRouteDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetNotificationRouteDetails, Exception: {ex}");
            }
            return objNotifRouteDetails;
        }*/
        #endregion

        #region public decimal GetMaxReduciableHeight()
        /*public decimal GetMaxReduciableHeight(int notificationId)
        {
            decimal result = 0;
            try
            {
                string urlParameters = "?NotificationId=" + notificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/GetMaxReduciableHeight{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<decimal>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetMaxReducibleHeight, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetMaxReducibleHeight, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region GetGrossWeight

        /*public int GetGrossWeight(int notifId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?notificationId=" + notifId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetGrossWeight{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetGrossWeight, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetGrossWeight, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region PrintCollabrationList
        /*public List<CollaborationModel> PrintCollabrationList(string notificationCode, int notificationId)
        {
            List<CollaborationModel> result = new List<CollaborationModel>();
            try
            {
                string urlParameters = "?notificationCode=" + notificationCode + "&notificationId=" + notificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/PrintCollabrationList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<CollaborationModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/PrintCollabrationList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/PrintCollabrationList, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #region GenerateOutboundNotificationStructureData
        /*public NotificationXSD.OutboundNotificationStructure GenerateOutboundNotificationStructureData(long NotificationId)
        {
            NotificationXSD.OutboundNotificationStructure obns = new NotificationXSD.OutboundNotificationStructure();
            try
            {
                string urlParameters = "?NotificationId=" + NotificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GenerateOutboundNotificationStructureData{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    obns = response.Content.ReadAsAsync<NotificationXSD.OutboundNotificationStructure>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GenerateOutboundNotificationStructureData, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GenerateOutboundNotificationStructureData, Exception: {ex}");
            }
            return obns;
        }*/
        #endregion

        #region GenerateOutboundNotificationStructureData
        /*public OutboundDocuments GetOutboundDoc(int notificationID)
        {
            OutboundDocuments outbounddocs = new OutboundDocuments();
            try
            {
                string urlParameters = "?notificationID=" + notificationID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/GetOutboundDoc{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    outbounddocs = response.Content.ReadAsAsync<OutboundDocuments>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetOutboundDoc, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"-Notification/GetOutboundDoc, Exception: {ex}");
            }
            return outbounddocs;
        }*/
        #endregion

        #region  GetPreviousAnalysisId
        /*public long GetPreviousAnalysisId(long notificationId)
        {
            long analysisId = 0;
            try
            {
                string urlParameters = "?notificationID=" + notificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Notification/GetPreviousAnalysisId{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    analysisId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetPreviousAnalysisId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/GetPreviousAnalysisId, Exception: {ex}");
            }
            return analysisId;
        }*/
        #endregion

        #region ListAxelDetails
        /*public List<AxleDetails> ListAxelDetails(int vehicleId)
        {
            List<AxleDetails> result = new List<AxleDetails>();
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/Notification/ListAxelDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<AxleDetails>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/ListCloneAxelDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Notification/ListCloneAxelDetails, Exception: {ex}");
            }
            return result;
        }*/
        #endregion

        #endregion

    }
}
