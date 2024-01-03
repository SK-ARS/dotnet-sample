using STP.Common.Constants;
using STP.Common.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using STP.Domain.Structures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly HttpClient httpClient;
        private readonly string Applications;
        public ApplicationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            Applications = ConfigurationManager.AppSettings["Applications"];
        }

        #region public int GetVR1General()
        public ApplyForVR1 GetVR1General(string userSchema, long revisionid = 0, long versionid = 0, int organisationId = 0, int historic = 0)
        {
            ApplyForVR1 objVR1 = new ApplyForVR1();
            try
            {
                string urlParameters = "?userSchema=" + userSchema + "&revisionId=" + revisionid + "&versionId=" + versionid + "&organisationId=" + organisationId + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/GetVR1General{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetVR1General, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetVR1General, Exception: {ex}");
            }
            return objVR1;
        }
        #endregion

        #region public Object GetSOApplicationTabDetails()
        public SOApplicationTabs GetSOApplicationTabDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            SOApplicationTabs objSOApplication = new SOApplicationTabs();
            try
            {
                string urlParameters = "?revisionId=" + revisionId + "&versionId=" + versionId + "&userSchema=" + userSchema + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/GetSOApplicationTabDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objSOApplication = response.Content.ReadAsAsync<SOApplicationTabs>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetSOApplicationTabDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetSOApplicationTabDetails, Exception: {ex}");
            }
            return objSOApplication;
        }
        #endregion              

        #region public int ResetNeedAttention()
        public int ResetNeedAttention(long projectID, long revisionID, long versionID)
        {
            int result = 0;
            try
            {
                string urlParameters = "?projectID=" + projectID + "&revisionID=" + revisionID + "&versionID=" + versionID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/ResetNeedAttention{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/ResetNeedAttention, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/ResetNeedAttention, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region public int Update_Needs_Attention()
        public int UpdateNeedsAttention(int notificationID = 0, int revisionID = 0, int naFlag = 0)
        {
            int result = 0;
            try
            {
                UpdateNeedsAttentionModel updateNeedsAttentionModel = new UpdateNeedsAttentionModel
                {
                    NotificationId = notificationID,
                    RevisionId = revisionID,
                    NAFlag = naFlag
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/UpdateNeedsAttention", updateNeedsAttentionModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/UpdateNeedsAttention, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/UpdateNeedsAttention, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region public int GetProjectFolderList()
        public List<ProjectFolderModel> GetProjectFolderList(int orgId)
        {
            List<ProjectFolderModel> projectFolderModel = new List<ProjectFolderModel>();
            try
            {
                string urlParameters = "?organisationId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/GetProjectFolderList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    projectFolderModel = response.Content.ReadAsAsync<List<ProjectFolderModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetProjectFolderList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetProjectFolderList, Exception: {ex}");
            }
            return projectFolderModel;
        }
        #endregion

        #region public int GetFolderDetails()
        public ProjectFolderModel GetFolderDetails(int flag, long folderID, long projectId, string hauliermnemonic, int esdalref, long notificationId, long revisionID)
        {
            ProjectFolderModel objProjectFolderModel = new ProjectFolderModel();
            try
            {
                ProjectFolderModelParams objProjectFolderModelParams = new ProjectFolderModelParams
                {
                    Flag = flag,
                    FolderId = folderID,
                    ProjectId = projectId,
                    HaulierMnemonic = hauliermnemonic,
                    ESDALReference = esdalref,
                    NotificationId = notificationId,
                    RevisionId = revisionID
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["Applications"]}" +
                    $"/Application/GetProjectFolderDetails",
                    objProjectFolderModelParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objProjectFolderModel = response.Content.ReadAsAsync<ProjectFolderModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetProjectFolderDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetProjectFolderDetails, Exception: {ex}");
            }
            return objProjectFolderModel;
        }
        #endregion

        #region GetStructureDetails
        public List<AffStructureGeneralDetails> GetStructureDetailList(string StructureCode, int SectionId = 0)
        {
            List<AffStructureGeneralDetails> result = new List<AffStructureGeneralDetails>();
            try
            {
                //api call to new service
                string urlParameters = "?StructureCode=" + StructureCode + "&SectionId=" + SectionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application//GetStructureDetailList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<AffStructureGeneralDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application//GetStructureDetailList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application//GetStructureDetailList, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetHAContactDetails
        public HAContact GetHAContactDetails(decimal ContactId)
        {
            HAContact hAContactObj = new HAContact();
            try
            {
                //api call to new service
                string urlParameters = "?ContactId=" + ContactId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                 $"/Application/GetHAContactDetails{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    hAContactObj = response.Content.ReadAsAsync<HAContact>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetHAContactDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetHAContactDetails, Exception: {0}", ex);
            }
            return hAContactObj;
        }
        #endregion

        #region GetStructureDetails
        public List<AffStructureSectionList> ViewAffStructureSections(string structureCode)
        {
            List<AffStructureSectionList> result = new List<AffStructureSectionList>();
            try
            {
                //api call to new service
                string urlParameters = "?structureCode=" + structureCode;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/ViewAffStructureSections{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<AffStructureSectionList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/ViewAffStructureSections, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/ViewAffStructureSections, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region Delete Application
        public bool DeleteApplication(long apprevisionId, string userSchema)
        {
            bool result = false;
            string urlParameters = "?appRevisionId=" + apprevisionId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.DeleteAsync(Applications + $"/Application/DeleteApplication{urlParameters}").Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = true;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/DeleteApplication, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region CheckLatestAppStatus
        public SOApplication CheckLatestAppStatus(long projectId)
        {
            SOApplication sOApplicationObj = new SOApplication();
            //api call to new service
            string urlParameters = "?projectId=" + projectId;
            HttpResponseMessage response = httpClient.GetAsync(Applications + $"/Application/CheckLatestAppStatus{ urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                sOApplicationObj = response.Content.ReadAsAsync<SOApplication>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/CheckLatestAppStatus, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return sOApplicationObj;
        }
        #endregion

        #region Withdraw Application
        public ApplicationWithdraw WithdrawApplication(long projectId, long appRevId)
        {
            ApplicationWithdraw withDrawApp = new ApplicationWithdraw();
            string urlParameters = "?projectId=" + projectId + "&appRevisionId=" + appRevId;
            HttpResponseMessage response = httpClient.GetAsync(Applications + $"/Application/WithdrawApplication{urlParameters}").Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                withDrawApp = response.Content.ReadAsAsync<ApplicationWithdraw>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/WithdrawApplication, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return withDrawApp;
        }
        #endregion

        #region Revise SO Application
        public SOApplication ReviseSOApplication(long apprevId, string userSchema)
        {
            SOApplication sOApplication = new SOApplication();
            string urlParameters = "?apprevId=" + apprevId + "&userSchema=" + userSchema;

            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                               $"/Application/ReviseSOApplication{urlParameters}").Result;


            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                sOApplication = response.Content.ReadAsAsync<SOApplication>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/ReviseSOApplication, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return sOApplication;
        }
        #endregion

        #region Clone SO Application
        public SOApplication CloneSOApplication(long apprevId, int organisationId, int userId)
        {
            SOApplication sOApplication = new SOApplication();
            string urlParameters = "?apprevId=" + apprevId + "&OrganisationId=" + organisationId + "&userId=" + userId;
            HttpResponseMessage response = httpClient.GetAsync(Applications + $"/Application/CloneSOApplication{urlParameters}").Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                sOApplication = response.Content.ReadAsAsync<SOApplication>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/CloneSOApplication, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return sOApplication;
        }
        #endregion

        #region Clone SO History Application
        public SOApplication CloneSOHistoryApplication(long apprevId, int organisationId, int userId, string userSchema)
        {
            SOApplication sOApplication = new SOApplication();
            string urlParameters = "?apprevId=" + apprevId + "&OrganisationId=" + organisationId + "&userId=" + userId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync(Applications + $"/Application/CloneSOHistoryApplication{urlParameters}").Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                sOApplication = response.Content.ReadAsAsync<SOApplication>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/CloneSOHistoryApplication, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return sOApplication;
        }
        #endregion

        #region Revise VR1 Application
        public ApplyForVR1 ReviseVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema)
        {
            ApplyForVR1 objReviseVR1 = new ApplyForVR1();
            CloneReviseVR1Params cloneReviseVR1 = new CloneReviseVR1Params()
            {
                ApplicationRevisionId = apprevId,
                ReducedDet = reducedDet,
                CloneApp = cloneApp,
                VersionId = versionId,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/ReviseVR1Application", cloneReviseVR1).Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                objReviseVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/ReviseVR1Application, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return objReviseVR1;
        }
        #endregion

        #region Clone History VR1 Application
        public ApplyForVR1 CloneHistoryVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema)
        {
            ApplyForVR1 objReviseVR1 = new ApplyForVR1();
            CloneReviseVR1Params cloneReviseVR1 = new CloneReviseVR1Params()
            {
                ApplicationRevisionId = apprevId,
                ReducedDet = reducedDet,
                CloneApp = cloneApp,
                VersionId = versionId,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/CloneHistoryVR1Application", cloneReviseVR1).Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                objReviseVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/CloneHistoryVR1Application, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return objReviseVR1;
        }
        #endregion

        #region GetSOGeneralWorkinProcessbyrevisionid
        public SOApplication GetSOGeneralWorkinProcessbyrevisionid(string userSchema, long revisionId = 0, long versionId = 0, int Org_id = 0)
        {
            SOApplication objSOApplication = new SOApplication();
            try
            {
                string urlParameters = "?userSchema=" + userSchema + "&revisionId=" + revisionId + "&versionId=" + versionId + "&Org_id=" + Org_id;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetSOGeneralWorkinProcessbyrevisionid{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objSOApplication = response.Content.ReadAsAsync<SOApplication>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/GetSOGeneralWorkinProcessbyrevisionid, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetSOGeneralWorkinProcessbyrevisionid, Exception: {ex}");
            }
            return objSOApplication;
        }
        #endregion

        #region GetSOGeneralDetails
        public SOApplication GetSOGeneralDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            SOApplication projectStatus = new SOApplication();
            try
            {
                string urlParameters = "?revisionId=" + revisionId + "&versionId=" + versionId + "&userSchema=" + userSchema + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetSOGeneralDetails{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    projectStatus = response.Content.ReadAsAsync<SOApplication>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/GetSOGeneralDetails, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetSOGeneralDetails, Exception: {ex}");
            }
            return projectStatus;
        }
        #endregion

        #region GetSONumberStatus
        public string GetSONumberStatus(int project_id, string userSchema = UserSchema.Portal)
        {
            string sonum = string.Empty;
            try
            {
                string urlParameters = "?projectId=" + project_id + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetSONumberStatus{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    sonum = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/GetSONumberStatus, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetSONumberStatus, Exception: {ex}");
            }
            return sonum;
        }
        #endregion 

        #region public int UpdateVR1Application()
        public bool UpdateVR1Application(ApplyForVR1 vr1application, int organisationId, int userId, long apprevid)
        {
            bool res = false;
            try
            {
                vr1application.OrganisationId = organisationId;
                vr1application.UserId = userId;
                vr1application.AppRevId = apprevid;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["Applications"]}" +
                    $"/Application/UpdateVR1Application",
                    vr1application).Result;
                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/UpdateVR1Application, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/UpdateVR1Application, Exception: {ex}");
            }
            return res;
        }
        #endregion

        #region public int SaveVR1Application()
        public ApplyForVR1 SaveVR1Application(ApplyForVR1 vr1application, int organisationId, int userId)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            try
            {
                vr1application.OrganisationId = organisationId;
                vr1application.UserId = userId;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["Applications"]}" +
                    $"/Application/SaveVR1Application",
                    vr1application).Result;
                if (response.IsSuccessStatusCode)
                {
                    applyForVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/SaveVR1Application, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/SaveVR1Application, Exception: {ex}");
            }
            return applyForVR1;
        }
        #endregion

        #region SaveAppGeneral
        public bool SaveAppGeneral(SOApplication soapplication, int organisationId, int userId, long applicationrevId, string userSchema)
        {
            bool result = false;
            UpdateSOApplicationParams updateSOApplicationParams = new UpdateSOApplicationParams()
            {
                SOApplication = soapplication,
                OrganisationId = organisationId,
                UserId = userId,
                ApplicationRevisionId = applicationrevId,
                userSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/SaveAppGeneral", updateSOApplicationParams).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/SaveAppGeneral, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region SaveSOApplication
        public long SaveSOApplication(SOApplication soapplication, int organisationId, int userId)
        {
            long result = 0;
            UpdateSOApplicationParams updateSOApplicationParams = new UpdateSOApplicationParams()
            {
                SOApplication = soapplication,
                OrganisationId = organisationId,
                UserId = userId
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/SaveSOApplication", updateSOApplicationParams).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/SaveSOApplication, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region CheckSOValidation
        public ApplyForVR1 CheckSOValidation(int apprevisionId, string userSchema = UserSchema.Portal)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            try
            {
                string urlParameters = "?apprevisionId=" + apprevisionId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/CheckSOValidation{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    applyForVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/CheckSOValidation, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/CheckSOValidation, Exception: {ex}");
            }
            return applyForVR1;
        }
        #endregion

        #region GetApplicationStatus
        public int GetApplicationStatus(int versionNo, int revisionNo, long projectId, string userSchema, int historic)
        {
            int result = 0;
            try
            {
                string urlParameters = "?versionNo=" + versionNo + "&revisionNo=" + revisionNo + "&projectId=" + projectId + "&userSchema=" + userSchema
                    + "&historic="+ historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetApplicationStatus{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetApplicationStatus, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetApplicationStatus, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetHAContDetFromInboundDoc
        public HAContact GetHAContDetFromInboundDoc(string EsdalRef)
        {
            HAContact ObjContactDet = new HAContact();
            try
            {
                string urlParameters = "?esdalReference=" + EsdalRef;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetHAContDetFromInboundDoc{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    ObjContactDet = response.Content.ReadAsAsync<HAContact>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetHAContDetFromInboundDoc, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetHAContDetFromInboundDoc, Exception: {ex}");
            }
            return ObjContactDet;
        }
        #endregion

        #region CheckVR1Validation
        public ApplyForVR1 CheckVR1Validation(int versionId, int showVehicle, string contentRef, int apprevisionId, string userSchema = UserSchema.Portal)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            try
            {
                string urlParameters = "?versionId=" + versionId + "&showVehicle=" + showVehicle + "&contentRef=" + contentRef + "&apprevisionId=" + apprevisionId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/CheckVR1Validation{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    applyForVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/CheckVR1Validation, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/CheckVR1Validation, Exception: {ex}");
            }
            return applyForVR1;
        }
        #endregion

        #region ListVR1RouteDetails
        public List<VR1RouteImport> ListVR1RouteDetails(string contentref)
        {
            List<VR1RouteImport> objlistrt = new List<VR1RouteImport>();
            try
            {
                string urlParameters = "?contentref=" + contentref;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/ListVR1RouteDetails{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistrt = response.Content.ReadAsAsync<List<VR1RouteImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/ListVR1RouteDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/ListVR1RouteDetails, Exception: {ex}");
            }
            return objlistrt;
        }
        #endregion

        #region SubmitSoApplication
        public SOApplication SubmitSoApplication(int apprevisionId, int userId)
        {
            SOApplication soGeneralDetails = new SOApplication();
            try
            {
                SubmitSoParams submitSoParams = new SubmitSoParams()
                {
                    ApplicationRevisionId = apprevisionId,
                    UserId = userId
                };
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(submitSoParams);
                HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/SubmitSoApplication", submitSoParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    soGeneralDetails = response.Content.ReadAsAsync<SOApplication>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/SubmitSoApplication, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/SubmitSoApplication, Exception: {ex}");
            }
            return soGeneralDetails;
        }
        #endregion

        #region SubmitVR1Application
        public ApplyForVR1 SubmitVR1Application(int apprevisionId, int reducedDet)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            try
            {
                SubmitVR1Params submitVR1Params = new SubmitVR1Params()
                {
                    ApplicationRevisionId = apprevisionId,
                    ReducedDet = reducedDet
                };
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(submitVR1Params);
                HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/SubmitVR1Application", submitVR1Params).Result;
                if (response.IsSuccessStatusCode)
                {
                    applyForVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/SubmitVR1Application, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/SubmitVR1Application, Exception: {ex}");
            }
            return applyForVR1;
        }
        #endregion

        #region GetVR1VehicleDetails
        public ApplyForVR1 GetVR1VehicleDEtails(VR1VehicleDetailsParams vr1VehicleDetailsParams)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/GetVR1VehicleDetails", vr1VehicleDetailsParams).Result;
            if (response.IsSuccessStatusCode)
            {
                applyForVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/GetVR1VehicleDetails, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return applyForVR1;
        }
        #endregion

        #region GetSOHaulierDetails
        public SOHaulierApplication GetSOHaulierDetails(long revisionId, long versionId, int historic)
        {
            SOHaulierApplication sOApplicationObj = new SOHaulierApplication();
            try
            {
                string urlParameters = "?revisionId=" + revisionId + "&versionId=" + versionId + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetSOHaulierDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    sOApplicationObj = response.Content.ReadAsAsync<SOHaulierApplication>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetSOHaulierDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetSOHaulierDetails, Exception: {ex}");
            }
            return sOApplicationObj;
        }
        #endregion

        #region SOVR1SupplementaryInfo
        public bool SOVR1SupplementaryInfo(SupplimentaryInfo objSupplimentaryInfo, string userSchema, int applicationRevisionId)
        {
            bool result = false;
            SupplimentaryInfoParams objSupplimentaryInfoParams = new SupplimentaryInfoParams()
            {
                SupplimentaryInfo = objSupplimentaryInfo,
                UserSchema = userSchema,
                ApplicationRevisionId = applicationRevisionId
            };
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/SOVR1SupplementaryInfo", objSupplimentaryInfoParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/SOVR1SupplementaryInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/SOVR1SupplementaryInfo, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region UpdatePartId
        public int UpdatePartId(int VehicleId, int PartId, bool VR1Appl, bool Notif, string RType, bool Iscand, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            UpdatePartIdInputParams updatePartIdInputParams = new UpdatePartIdInputParams()
            {
                Notif = Notif,
                PartId = PartId,
                RType = RType,
                userSchema = userSchema,
                VehicleId = VehicleId,
                VR1Appl = VR1Appl,
                Iscand = Iscand
            };
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/UpdatePartId", updatePartIdInputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/UpdatePartId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/UpdatePartId, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region InsertApplicationType
        public AppGeneralDetails InsertApplicationType(PlanMovementType saveApplication)
        {
            AppGeneralDetails appGeneral = new AppGeneralDetails();
            
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/InsertApplicationType", saveApplication).Result;
            if (response.IsSuccessStatusCode)
            {
                appGeneral = response.Content.ReadAsAsync<AppGeneralDetails>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/InsertApplicationType, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return appGeneral;
        }

        public AppGeneralDetails UpdateApplicationType(PlanMovementType updateApplication)
        {
            AppGeneralDetails appGeneral = new AppGeneralDetails();
            var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(updateApplication);
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Applications + $"/Application/UpdateApplicationType", updateApplication).Result;
            if (response.IsSuccessStatusCode)
            {
                appGeneral = response.Content.ReadAsAsync<AppGeneralDetails>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Application/UpdateApplicationType, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return appGeneral;
        }
        #endregion

        #region VR1GetSupplementaryInfo
        public SupplimentaryInfo VR1GetSupplementaryInfo(int apprevisionId = 0, string userSchema = UserSchema.Portal, int historic = 0)
        {
            SupplimentaryInfo result = new SupplimentaryInfo();

            try
            {
                string urlParameters = "?apprevisionId=" + apprevisionId + "&userSchema=" + userSchema;
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/VR1GetSupplementaryInfo{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<SupplimentaryInfo>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/VR1GetSupplementaryInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/VR1GetSupplementaryInfo, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetSORTHaulierAppRouteParts
        public List<AffectedStructures> GetSORTHaulierAppRouteParts(int versionID, string vr1ContentRefNo, string userSchema)
        {
            List<AffectedStructures> affectedStructures = new List<AffectedStructures>();

            try
            {
                string urlParameters = "?versionID=" + versionID + "&vr1ContentRefNo=" + vr1ContentRefNo + "&userSchema=" + userSchema;
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetSORTHaulierAppRouteParts{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    affectedStructures = response.Content.ReadAsAsync<List<AffectedStructures>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/GetSORTHaulierAppRouteParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/GetSORTHaulierAppRouteParts, Exception: {ex}");
            }
            return affectedStructures;
        }
        #endregion

        #region GetHaulierApplRouteParts
        public List<AffectedStructures> GetHaulierApplRouteParts(int revisionID, string appFlag, string sortRouteVehicleFlag, string userSchema)
        {
            List<AffectedStructures> affectedStructures = new List<AffectedStructures>();

            try
            {
                string urlParameters = "?revisionID=" + revisionID + "&appFlag=" + appFlag + "&sortRouteVehicleFlag=" + sortRouteVehicleFlag + "&userSchema=" + userSchema;
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetHaulierApplRouteParts{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    affectedStructures = response.Content.ReadAsAsync<List<AffectedStructures>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/GetHaulierApplRouteParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/GetHaulierApplRouteParts, Exception: {ex}");
            }
            return affectedStructures;
        }
        #endregion

        #region GetHaulierApplRouteParts
        public List<AffectedStructures> GetNotifRouteParts(int notificationid, int rpFlag)
        {
            List<AffectedStructures> affectedStructures = new List<AffectedStructures>();

            try
            {
                string urlParameters = "?notificationid=" + notificationid + "&rpFlag=" + rpFlag;
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/Application/GetNotifRouteParts{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    affectedStructures = response.Content.ReadAsAsync<List<AffectedStructures>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/GetNotifRouteParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Application/GetNotifRouteParts, Exception: {ex}");
            }
            return affectedStructures;
        }
        #endregion

        #region  IMP_CondidateRoue()
        public long IMP_CondidateRoue(int RoutePartId, int AppRevId, int VersionId, string ContentRef, string userSchema)
        {

            long result = 0;
            try
            {
                string urlParameters = "?RoutePartId=" + RoutePartId + "&AppRevId=" + AppRevId + "&VersionId=" + VersionId + "&ContentRef=" + ContentRef + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/IMP_CondidateRoue{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/IMP_CondidateRoue, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/IMP_CondidateRoue, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetEsdalRefNum
        public string GetEsdalRefNum(int SOVersionID)
        {

            string result = string.Empty;
            try
            {
                string urlParameters = "?SOVersionID=" + SOVersionID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/EsdalRefNum{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/EsdalRefNum, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/EsdalRefNum, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetAgreedRouteParts
        public List<AffectedStructures> GetAgreedRouteParts(int VersionId, int revisionid, string userSchema, string ContentRefNo = "")
        {
            List<AffectedStructures> list = new List<AffectedStructures>();
            try
            {
                string urlParameters = "?VersionId=" + VersionId + "&revisionid=" + revisionid + "&userSchema=" + userSchema + "&ContentRefNo=" + ContentRefNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/GetAgreedRouteParts{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    list = response.Content.ReadAsAsync<List<AffectedStructures>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetAgreedRouteParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetAgreedRouteParts, Exception: {ex}");
            }
            return list;
        }
        #endregion

        #region GetApplicationDetails
        public PlanMovementType GetApplicationDetails(long revisionId, string userSchema)
        {
            PlanMovementType planMovement = new PlanMovementType();
            try
            {
                string urlParameters = "?revisionId=" + revisionId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/GetApplicationDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    planMovement = response.Content.ReadAsAsync<PlanMovementType>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetApplicationDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetApplicationDetails, Exception: {ex}");
            }
            return planMovement;
        }
        #endregion

        #region GetNotificationDetails
        public PlanMovementType GetNotificationDetails(long notificationId, string userSchema)
        {
            PlanMovementType planMovement = new PlanMovementType();
            try
            {
                string urlParameters = "?notificationId=" + notificationId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                               $"/Application/GetNotificationDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    planMovement = response.Content.ReadAsAsync<PlanMovementType>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetNotificationDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Application/GetNotificationDetails, Exception: {ex}");
            }
            return planMovement;
        }
        #endregion
    }
}
