using STP.Common.Logger;
using STP.Domain.Routes;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.SecurityAndUsers;
using AggreedRouteXSD;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.ServiceAccess.Applications
{
    public class SORTApplicationService : ISORTApplicationService
    {
        private readonly HttpClient httpClient;
        const string m_RouteName = "-SORTApplication";
        public SORTApplicationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region Commented Code by Mahzeer on 13/07/2023
        /*
        public bool UpdateCheckerDetails(UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams)
        {
            bool responseData = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/UpdateCheckerDetails",
                           updateCheckerDetailsInsertParams).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/UpdateCheckerDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                else
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/UpdateCheckerDetails, Exception: {0}", ex);
            }
            return responseData;
        }
        public bool GetCandRouteVehicleDetails(long routeRevisionID, string userSchema)
        {
            bool responseData = false;
            string urlParameters = "?routeRevisionID=" + routeRevisionID + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetCandRouteVehicleDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetCandRouteVehicleDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public ApplyForVR1 SaveSORTVR1Application(SORTVR1ApplicationInsertParams sortVR1ApplicationInsertParams)
        {
            ApplyForVR1 responseData = new ApplyForVR1();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveSORTVR1Application",
                           sortVR1ApplicationInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSORTVR1Application, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSORTVR1Application, Exception: {0}", ex);
            }
            return responseData;
        }
        public SOApplication SaveSOApplication(SOApplication soApplication)
        {
            SOApplication responseData = new SOApplication();

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveSOApplication",
                           soApplication).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<SOApplication>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSOApplication, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSOApplication, Exception: {0}", ex);
            }
            return responseData;
        }
        public string UpdateSortSpecialOrder(UpdateSORTSpecialOrderParams updateSortSpecialOrderParams)
        {
            string responseData = string.Empty;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/UpdateSortSpecialOrder",
                           updateSortSpecialOrderParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateSortSpecialOrder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateSortSpecialOrder, Exception: {0}", ex);
            }
            return responseData;
        }
        public bool DeleteSpecialOrder(DeleteSpecialOrderParams deleteSpecialOrderParams)
        {
            bool responseData = false;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/DeleteSpecialOrder",
                           deleteSpecialOrderParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/DeleteSpecialOrder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/DeleteSpecialOrder, Exception: {0}", ex);
            }
            return responseData;
        }
        public int GenerateSOProposalDocument(string EsdalReference, int OrganisationId, int ContactId, string DistributionComments, int VersionId, Dictionary<int, int> ICAStatusDictionary, string Esdalreference, HAContact HaContactDetail, AgreedRouteStructure Agreedroute, string UserSchema, int RoutePlanUnits, long ProjectStatus, int VersionNo, MovementPrint Moveprint, decimal PreVersionDistr, UserInfo SessionInfo)
        {
            int result = 0;
            try
            {
                SOProposalDocumentParams sOProposalDocument = new SOProposalDocumentParams
                {
                    EsdalReferenceNo = Esdalreference,
                    OrganisationId = OrganisationId,
                    ContactId = ContactId,
                    DistributionComments = DistributionComments,
                    VersionId = VersionId,
                    ICAStatusDictionary = ICAStatusDictionary,
                    Esdalreference = Esdalreference,
                    HaContactDetail = HaContactDetail,
                    Agreedroute = Agreedroute,
                    UserSchema = UserSchema,
                    RoutePlanUnits = RoutePlanUnits,
                    ProjectStatus = ProjectStatus,
                    VersionNo = VersionNo,
                    Moveprint = Moveprint,
                    PreVersionDistr = PreVersionDistr,
                    SessionInfo = SessionInfo
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/SORTDocument/GenerateSOProposalDocument", sOProposalDocument).Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateSOProposalDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateSOProposalDocument, Exception: {0}", ex);
            }
            return result;
        }
        */
        #endregion

        public List<SORTMovementList> GetHaulierAppRevision(long projectID)
        {

            List<SORTMovementList> responseList = new List<SORTMovementList>();
            string urlParameters = "?projectID=" + projectID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetHaulierAppRevision{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<SORTMovementList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetHaulierAppRevision, Error: {(int)response.StatusCode}:{response.ReasonPhrase}");
            }
            return responseList;
        }
        public List<SORTUserList> ListSORTUser(long userTypeID, int checkerType = 0)
        {

            List<SORTUserList> responseList = new List<SORTUserList>();
            string urlParameters = "?userTypeID=" + userTypeID + "&checkerType=" + checkerType;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
              $"/SORTApplication/ListSORTUser{urlParameters}").Result;

            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<SORTUserList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/SORTApplication/ListSORTUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseList;
        }
        public List<string> SaveSORTAllocateUser(AllocateSORTUserInsertParams allocateSortUserParams)
        {
            List<string> responseList = new List<string>();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveSORTAllocateUser",
                           allocateSortUserParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseList = response.Content.ReadAsAsync<List<string>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSORTAllocateUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSORTAllocateUser, Exception: {0}", ex);
            }
            return responseList;
        }
        public List<SORTMovementList> GetMovmentVersion(long projectID)
        {
            List<SORTMovementList> responseList = new List<SORTMovementList>();
            string urlParameters = "?projectID=" + projectID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetMovmentVersion{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<SORTMovementList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetMovmentVersion, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseList;
        }
        public List<CandidateRTModel> GetCandidateRTDetails(long projectID)
        {
            List<CandidateRTModel> responseList = new List<CandidateRTModel>();
            string urlParameters = "?projectID=" + projectID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetCandidateRTDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<CandidateRTModel>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetCandidateRTDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseList;
        }
        public SOApplicationRelatedMov GetRelatedMovement(long applicationID, string type)
        {
            SOApplicationRelatedMov responseData = new SOApplicationRelatedMov();
            string urlParameters = "?applicationID=" + applicationID + "&type=" + type;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetRelatedMovement{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<SOApplicationRelatedMov>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetRelatedMovement, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public bool CheckVehicleOnRoute(int projectID, int revisionID)
        {
            bool responseData = false;
            string urlParameters = "?projectID=" + projectID + "&revisionID=" + revisionID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/CheckVehicleOnRoute{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/CheckVehicleOnRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public long GetRevIDFromApplication(long projectID)
        {
            long responseData = 0;
            string urlParameters = "?projectID=" + projectID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetRevIDFromApplication{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetRevIDFromApplication, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public List<MovementHistory> GetMovementHistory(int pageNumber, int pageSize, string haulierNumber, int esdalReference, int versionNumber, long projectID,int? sortOrder=null,int?sortType=null)
        {
            List<MovementHistory> responseList = new List<MovementHistory>();
            string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&haulierNumber=" + haulierNumber + "&esdalReference=" + esdalReference + "&versionNumber=" + versionNumber + "&projectID=" + projectID + "&sortOrder=" +sortOrder + "&sortType="+sortType;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetMovementHistory{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<MovementHistory>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetMovementHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseList;
        }
        public bool GetCandRouteVehicleAssignDetails(long routeRevisionID, string userSchema)
        {
            bool responseData = false;
            string urlParameters = "?routeRevisionID=" + routeRevisionID + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetCandRouteVehicleAssignDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetCandRouteVehicleAssignDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public string GetCandidateRouteNM(long candidateRouteID)
        {
            string responseData = string.Empty;
            string urlParameters = "?candidateRouteID=" + candidateRouteID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetCandidateRouteNM{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetCandidateRouteNM, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public string GetVR1ApprovalDate(long projectID)
        {
            string responseData = string.Empty;
            string urlParameters = "?projectID=" + projectID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetVR1ApprovalDate{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetVR1ApprovalDate, Error: {(int)response.StatusCode}​​​​​​ - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public SOApplication GetProjOverviewDetails(long revisionID)
        {
            SOApplication responseData = new SOApplication();
            string urlParameters = "?revisionID=" + revisionID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetProjOverviewDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<SOApplication>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetProjOverviewDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public string GetSORTNotifiCode(int revisionID)
        {
            string responseData = string.Empty;
            string urlParameters = "?revisionID=" + revisionID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetSORTNotifiCode{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetSORTNotifiCode, Error: {(int)response.StatusCode}​​​​​​ - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public List<CRDetails> GetRouteType(int revisionID, string userSchema)
        {
            List<CRDetails> responseList = new List<CRDetails>();
            string urlParameters = "?revisionID=" + revisionID + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetRouteType{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<CRDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetRouteType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseList;
        }
        public List<AppVehicleConfigList> CandidateRouteVehicleConfiguration(int revisionID, string userSchema, char rListType = ApplicationConstants.RListTypeC)
        {
            List<AppVehicleConfigList> responseList = new List<AppVehicleConfigList>();
            string urlParameters = "?revisionID=" + revisionID + "&userSchema=" + userSchema+ "&rListType="+ rListType;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/CandidateRouteVehicleConfiguration{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<AppVehicleConfigList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/CandidateRouteVehicleConfiguration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseList;
        }
        public int SORTAppWithdrawandDecline(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams)
        {
            int responseData = 0;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SORTAppWithdrawandDecline",
                           sortApplnWithdrawandDeclineParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SORTAppWithdrawandDecline, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SORTAppWithdrawandDecline, Exception: {0}", ex);
            }
            return responseData;
        }
        public int SORTUnwithdraw(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams)
        {
            int responseData = 0;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SORTUnwithdraw",
                           sortApplnWithdrawandDeclineParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SORTUnwithdraw, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SORTUnwithdraw, Exception: {0}", ex);
            }
            return responseData;
        }
        public int CheckCandIsModified(int analysisID)
        {
            int responseData = 0;
            string urlParameters = "?analysisID=" + analysisID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/CheckCandIsModified{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/CheckCandIsModified, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public SORTLatestAppDetails GetSortProjectDetails(long projectID)
        {
            SORTLatestAppDetails responseData = new SORTLatestAppDetails();
            string urlParameters = "?projectID=" + projectID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/GetSortProjectDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<SORTLatestAppDetails>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetSortProjectDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseData;
        }
        public List<AppRouteList> CandRouteList(int routeRevisionID, string userSchema, char rListType = ApplicationConstants.RListTypeC)
        {
            List<AppRouteList> responseList = new List<AppRouteList>();
            string urlParameters = "?routeRevisionID=" + routeRevisionID + "&userSchema=" + userSchema + "&rListType=" + rListType;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
            $"/SORTApplication/CandRouteList{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<AppRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/CandRouteList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return responseList;
        }
        public SOApplication SubmitSORTSoApplication(SORTSOApplicationParams sortSoApplicationParams)
        {
            SOApplication responseData = new SOApplication();

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SubmitSORTSoApplication",
                           sortSoApplicationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<SOApplication>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SubmitSORTSoApplication, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SubmitSORTSoApplication, Exception: {0}", ex);
            }
            return responseData;
        }
        public CandidateRouteInsertResponse SaveCandidateRoute(CandidateRouteInsertParams candidateRouteSaveParams)
        {
            CandidateRouteInsertResponse responseData = new CandidateRouteInsertResponse();

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveCandidateRoute",
                           candidateRouteSaveParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<CandidateRouteInsertResponse>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveCandidateRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveCandidateRoute, Exception: {0}", ex);
            }
            return responseData;
        }
        public RouteRevision SaveRouteRevision(RouteRevisionInsertParams routeRevisionInsertParams)
        {
            RouteRevision responseData = new RouteRevision();

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveRouteRevision",
                           routeRevisionInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<RouteRevision>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SaveRouteRevision, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SaveRouteRevision, Exception: {0}", ex);
            }
            return responseData;
        }
        public int SaveSORTMovProjDetail(SORTMvmtProjectDetailsInsertParams sortMvmtProjectDetailsInsertParams)
        {
            int responseData = 0;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveSORTMovProjDetail",
                           sortMvmtProjectDetailsInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSORTMovProjDetail, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveSORTMovProjDetail, Exception: {0}", ex);
            }
            return responseData;
        }
        public ApplyForVR1 SubmitSORTVR1Application(SubmitSORTVR1Params submitSORTParams)
        {
            ApplyForVR1 responseData = new ApplyForVR1();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SubmitSORTVR1Application",
                           submitSORTParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SubmitSORTVR1Application, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SubmitSORTVR1Application, Exception: {0}", ex);
            }
            return responseData;
        }
        public int SaveVR1Approval(VR1ApprovalInsertParams vr1ApprovalInsertParams)
        {
            int responseData = 0;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveVR1Approval",
                           vr1ApprovalInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveVR1Approval, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveVR1Approval, Exception: {0}", ex);
            }
            return responseData;
        }
        public bool SaveMovHaulierNotes(HaulierMovNotesInsertParams haulierMovNotesInsertParams)
        {
            bool responseData = false;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveMovHaulierNotes",
                           haulierMovNotesInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveMovHaulierNotes, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveMovHaulierNotes, Exception: {0}", ex);
            }
            return responseData;
        }
        public ApplyForVR1 ReviseVR1Application(ReviseVR1Params reviseVR1Params)
        {
            ApplyForVR1 objReviseVR1 = new ApplyForVR1();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/ReviseVR1Application",
                           reviseVR1Params).Result;

                if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    objReviseVR1 = response.Content.ReadAsAsync<ApplyForVR1>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/ReviseVR1Application, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/ReviseVR1Application, Exception: {0}", ex);
            }
            return objReviseVR1;
        }
        public long UpdateCandidateRouteNM(UpdateCandidateRouteNMInsertParams updateCandidateRouteNMInsertParams)
        {
            long responseData = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/UpdateCandidateRoute",
                           updateCandidateRouteNMInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateCandidateRouteNM, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateCandidateRouteNM, Exception: {0}", ex);
            }
            return responseData;
        }
        public decimal UpdateProjectDetails(UpdateProjectDetailsInsertParams updateProjectDetailsInsertParams)
        {
            long responseData = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/UpdateProjectDetails",
                           updateProjectDetailsInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateProjectDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateProjectDetails, Exception: {0}", ex);
            }
            return responseData;
        }
        public bool UpdateCollaborationView(int documentID)
        {
            bool responseData = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/UpdateCollaborationView?documentID="+documentID, documentID).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateCollaborationView, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateCollaborationView, Exception: {0}", ex);
            }
            return responseData;
        }
        public int UpdateSpecialOrder(UpdateSpecialOrderInsertParams updateSpecialOrderInsertParams)
        {
            int responseData = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/UpdateSpecialOrder",
                           updateSpecialOrderInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateSpecialOrder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/UpdateSpecialOrder, Exception: {0}", ex);
            }
            return responseData;
        }
        public bool Deletequicklinks(long projectID)
        {
            bool responseData = false;
            try
            {

                string urlParameters = "?ProjectId=" + projectID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                $"/SORTApplication/Deletequicklinks{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/Deletequicklinks, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/Deletequicklinks, Exception: {0}", ex);
            }
            return responseData;
        }
        public void CloneRouteParts(CloneRTPartsInsertParams cloneRTPartsInsertParams)
        {

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/CloneRouteParts",
                           cloneRTPartsInsertParams).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CloneRouteParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CloneRouteParts, Exception: {0}", ex);
            }

        }
        public bool CheckerDetailsUpdation(UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams)
        {
            bool responseData = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/CheckerDetailsUpdation",
                           updateCheckerDetailsInsertParams).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CheckerDetailsUpdation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                else
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CheckerDetailsUpdation, Exception: {0}", ex);
            }
            return responseData;
        }
        public string SaveVR1Number(VR1NumberInsertParams vr1NumberInsertParams)
        {
            string responseData = string.Empty;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveVR1Number",
                           vr1NumberInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SaveVR1Number. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SaveVR1Number. Exception: {0}", ex);
            }
            return responseData;
        }
        public bool SpecialOrderUpdation(SpecialOrderUpdationInsertParams specialOrderUpdationInsertParams)
        {
            bool responseData = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SpecialOrderUpdation",
                           specialOrderUpdationInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SpecialOrderUpdation. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SpecialOrderUpdation. Exception: {0}", ex);
            }
            return responseData;
        }
        public object SaveMovementVersion(InsertMovementVersionInsertParams insertMovementVersionInsertParams)
        {
            object responseData = null;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/InsertMovementVersion",
                           insertMovementVersionInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<object>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SaveMovementVersion. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SpecialOrderUpdation. Exception: {0}", ex);
            }
            return responseData;
        }
        public void CloneApplicationParts(CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams)
        {
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/CloneApplicationParts",
                           cloneApplicationRTPartsInsertParams).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CloneApplicationParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CloneApplicationParts, Exception: {0}", ex);
            }
        }
        public bool SaveResponseMessage(SaveResponseMessageParams saveResponseMessageParams)
        {
            bool responseData = false;
            try
            {
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(saveResponseMessageParams);
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/SaveResponseMessage",
                           saveResponseMessageParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveResponseMessage. StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/SaveResponseMessage. Exception: {0}", ex);
            }
            return responseData;
        }
        public byte[] GetMovHaulierNotes(long VersionId, string UserSchema)
        {
            byte[] result = null;
            try
            {
                string urlParameters = "?movementVersionID=" + VersionId + "&userSchema=" + UserSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                  $"/SORTApplication/GetMovHaulierNotes{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetMovHaulierNotes, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetMovHaulierNotes, Exception: {0}", ex);
            }
            return result;
        }
        public SOHaulierApplication GetSORTSOHaulierDetails(long revisionId)
        {
            SOHaulierApplication sOApplicationObj = new SOHaulierApplication();
            try
            {
                string urlParameters = "?revisionId=" + revisionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/SORTApplication/GetSORTSOHaulierDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    sOApplicationObj = response.Content.ReadAsAsync<SOHaulierApplication>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"SORTApplication/GetSORTSOHaulierDetails, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"SORTApplication/GetSORTSOHaulierDetails, Exception: " + ex);
            }
            return sOApplicationObj;
        }
        public void UpdateCandIsModified(int analysisID)
        {
            try
            {
                string urlParameters = "?analysisID=" + analysisID;
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                                                                  $"/SORTApplication/UpdateCandIsModified{urlParameters}",null).Result;
                if (response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"SORTApplication/UpdateCandIsModified, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"SORTApplication/UpdateCandIsModified, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"SORTApplication/UpdateCandIsModified, Exception: " + ex);
            }
        }
        public int MovementVersionAgreeUnagreeWith(MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams)
        {
            int responseData = 0;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Applications"]}" +
                           $"/SORTApplication/MovementVersionAgreeUnagreeWith",
                           movementVersionAgreeUnagreeWithInsertParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/MovementVersionAgreeUnagreeWith, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/MovementVersionAgreeUnagreeWith, Exception: {0}", ex);
            }
            return responseData;
        }
        public byte[] GenerateFormVR1Document(string haulierMnemonic, string esdalRefNumber, int Version_No, bool generateFlag = true)
        {
            byte[] result = null;
            try
            {
                string urlParameters = "?haulierMnemonic=" + haulierMnemonic + "&esdalRefNumber=" + esdalRefNumber + "&Version_No=" + Version_No + "&generateFlag=" + generateFlag;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/SORTDocument/GenerateFormVR1Document{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateFormVR1Document, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateFormVR1Document, Exception: {0}", ex);
            }
            return result;
        }
        public byte[] GenerateHaulierAgreedRouteDocument(string esDALRefNo = "GCS1/25/2", string order_no = "P21/2012", int contactId = 8866, int UserTypeId=0)
        {
            byte[] result = null;
            try
            {
                string urlParameters = "?esDALRefNo=" + esDALRefNo + "&order_no=" + order_no + "&contactId=" + contactId + "&UserTypeId=" + UserTypeId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/SORTDocument/GenerateHaulierAgreedRouteDocument{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateFormVR1Document, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateFormVR1Document, Exception: {0}", ex);
            }
            return result;
        }
        public byte[] GetAffectedStructures(int notificationId, string esdalReferenceNumber, string haulierMnemonic, string versionNumber, string userSchema)
        {
            byte[] result = null;
            try
            {
                string urlParameters = "?notificationId=" + notificationId + "&esdalReferenceNumber=" + esdalReferenceNumber + "&haulierMnemonic=" + haulierMnemonic + "&versionNumber=" + versionNumber + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                  $"/Notification/GetNotificationAffectedStructures{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetNotificationAffectedStructures, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetNotificationAffectedStructures, Exception: {0}", ex);
            }
            return result;
        }
        public MovementPrint GetOrderNoProjectId(int versionId)
        {
            MovementPrint result = null;
            try
            {
                string urlParameters = "?versionId=" + versionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                  $"/Notification/GetOrderNoProjectId{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<MovementPrint>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetOrderNoProjectId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetOrderNoProjectId, Exception: {0}", ex);
            }
            return result;
        }
        public MovementPrint GetProjectIdByEsdalReferenceNo(string EsdalRefNo)
        {
            MovementPrint result = null;
            try
            {
                string urlParameters = "?EsdalRefNo=" + EsdalRefNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                  $"/Notification/GetProjectIdByEsdalReferenceNo{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<MovementPrint>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetProjectIdByEsdalReferenceNo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetProjectIdByEsdalReferenceNo, Exception: {0}", ex);
            }
            return result;
        }
        public Dictionary<int, int> GetNotifICAstatus(string xmlaffectedStructures)
        {
            Dictionary<int, int> result = null;
            try
            {
                NotificationICAstatusParans notificationICAstatus = new NotificationICAstatusParans
                {
                    XmlaffectedStructures = xmlaffectedStructures
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/SORTDocument/GetNotifICAstatus", notificationICAstatus).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<Dictionary<int, int>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetNotifICAstatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetNotifICAstatus, Exception: {0}", ex);
            }
            return result;
        }
        public bool SaveAffectedMovementDetails(AffectedStructConstrParam affectedParam)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}"
                                                                           + $"/Movements/SaveAffectedMovementDetails", affectedParam).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/SaveAffectedMovementDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/SaveAffectedMovementDetails, Exception: {ex}");
            }
            return result;
        }
        public byte[] GenerateAmendmentDocument(string SOnumber, Enums.DocumentType doctype, int organisationId, bool generateFlag)
        {
            byte[] result = null;
            try
            {
                string urlParameters = "?SOnumber=" + SOnumber + "&doctype=" + doctype + "&organisationId=" + organisationId + "&generateFlag=" + generateFlag;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/SORTDocument/GenerateAmendmentDocument{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateAmendmentDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateSOProposalDocument, Exception: {0}", ex);
            }
            return result;
        }
        public List<AffectedStructures> GetAgreedRouteParts(int revisionid, string userschema)
        {
            List<AffectedStructures> list = new List<AffectedStructures>();
            try
            {
                string urlParameters = "?revisionid=" + revisionid + "&userschema=" + userschema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Applications"]}" +
                  $"/SORTApplication/GetAgreedRouteParts{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    list = response.Content.ReadAsAsync<List<AffectedStructures>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetAgreedRouteParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTApplication/GetAgreedRouteParts, Exception: {0}", ex);
            }
            return list;
        }
    }
}
