using NetSdoGeometry;
using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.RoadNetwork.Constraint;
using STP.Domain.RouteAssessment;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.RoadNetwork
{
    public class ConstraintService : IConstraintService
    {
        private readonly HttpClient httpClient;

        public ConstraintService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region List<Object> GetConstraintHistory()
        public List<ConstraintModel> GetConstraintHistory(int pageNumber, int pageSize, long constraintId)
        {
            List<ConstraintModel> constraintModels = new List<ConstraintModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&constraintId=" + constraintId;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetConstraintHistory" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    constraintModels = response.Content.ReadAsAsync<List<ConstraintModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetConstraintHistory, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetConstraintHistory, Exception: {0}", ex));
            }
            return constraintModels;
        }
        #endregion

        #region List<Object> DeleteConstraint()
        public long DeleteConstraint(long ConstraintID, string UserName)
        {
            long res = 0;
            try
            {
                string urlParameters = "?ConstraintID=" + ConstraintID + "&UserName=" + UserName;
                HttpResponseMessage response = httpClient.DeleteAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/DeleteConstraint" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/DeleteConstraint, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/DeleteConstraint, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> DeleteCaution()
        public int DeleteCaution(long cautionId, string userName)
        {
            int affectedRows = 0;
            try
            {
                string urlParameters = "?cautionId=" + cautionId + "&userName=" + userName;
                HttpResponseMessage response = httpClient.DeleteAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/DeleteCaution" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    affectedRows = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/DeleteCaution, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/DeleteCaution, Exception: {0}", ex));
            }
            return affectedRows;
        }
        #endregion

        #region List<Object> GetCautionList()
        public List<ConstraintModel> GetCautionList(int pageNumber, int pageSize, long ConstraintID)
        {
            List<ConstraintModel> res = new List<ConstraintModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&ConstraintID=" + ConstraintID;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetCautionList" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<List<ConstraintModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetCautionList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetCautionList, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> GetCautionDetails()
        public ConstraintModel GetCautionDetails(long cautionID)
        {
            ConstraintModel res = new ConstraintModel();
            try
            {
                string urlParameters = "?cautionID=" + cautionID;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetCautionDetails" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<ConstraintModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetCautionDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetCautionDetails, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> GetConstraintDetails()
        public ConstraintModel GetConstraintDetails(int ConstraintID)
        {
            ConstraintModel constraintModels = new ConstraintModel();
            try
            {
                string urlParameters = "?ConstraintID=" + ConstraintID;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetConstraintDetails" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    constraintModels = response.Content.ReadAsAsync<ConstraintModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetConstraintDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetConstraintDetails, Exception: {0}", ex));
            }
            return constraintModels;
        }
        #endregion

        #region List<Object> CheckLinkOwnerShipForPolice()
        public bool CheckLinkOwnerShipForPolice(int orgID, List<ConstraintReferences> constRefrences, bool allLinks)
        {
            bool res = false;
            try
            {
                ConstraintReferencesParam constraintReferences = new ConstraintReferencesParam
                {
                    ConstraintRefrences = constRefrences,
                    OrganisationId = orgID,
                    AllLinks = allLinks
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/CheckLinkOwnerShipForPolice",
                   constraintReferences).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/CheckLinkOwnerShipForPolice, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/CheckLinkOwnerShipForPolice, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> CheckLinkOwnerShip()
        public bool CheckLinkOwnerShip(int orgID, List<ConstraintReferences> constRefrences, bool allLinks)
        {
            bool res = false;
            try
            {
                ConstraintReferencesParam constraintReferences = new ConstraintReferencesParam
                {
                    ConstraintRefrences = constRefrences,
                    OrganisationId = orgID,
                    AllLinks = allLinks
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/CheckLinkOwnerShip",
                   constraintReferences).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/CheckLinkOwnerShip, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/CheckLinkOwnerShip, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> SaveLinkDetails()
        public bool SaveLinkDetails(long CONSTRAINT_ID, List<ConstraintReferences> constRefrences)
        {
            bool res = false;
            try
            {
                ConstraintReferencesParam constraintReferences = new ConstraintReferencesParam
                {
                    ConstraintRefrences = constRefrences,
                    ConstraintId = CONSTRAINT_ID
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/SaveLinkDetails",
                   constraintReferences).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/SaveLinkDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/SaveLinkDetails, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> CheckLinkOwnerShip()
        public bool CheckLinkOwnerShip(int orgID, List<int> linkIDs, bool allLinks)
        {
            bool res = false;
            try
            {
                string urlParameters = "?orgID=" + orgID + "&linkIDs=" + linkIDs + "&allLinks=" + allLinks;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetConstraintListForOrg" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/CheckLinkOwnerShip, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/CheckLinkOwnerShip, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> SaveConstraints()
        public long SaveConstraints(ConstraintModel constrModel, int userID)
        {
            long res = 0;
            try
            {
                constrModel.AreaGeomStructure = JsonConvert.SerializeObject(constrModel.Geometry);
                constrModel.Geometry = null;
                constrModel.UserId = userID;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/SaveConstraints",
                   constrModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/SaveConstraints, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/SaveConstraints, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> SaveConstraints()
        public long UpdateConstraint(ConstraintModel constrModel, int userID)
        {
            long res = 0;
            try
            {
                constrModel.UserId = userID;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/UpdateConstraint",
                   constrModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/UpdateConstraint, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/UpdateConstraint, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> SaveCautions()
        public bool SaveCautions(ConstraintModel constrModel)
        {
            bool res = false;
            try
            {
                constrModel.AreaGeomStructure = JsonConvert.SerializeObject(constrModel.Geometry);
                constrModel.Geometry = null;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/SaveCautions",
                   constrModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/SaveCautions, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/SaveCautions, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> UpdateConstraintLog()
        public bool UpdateConstraintLog(List<ConstraintLogModel> constrModel)
        {
            bool res = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/UpdateConstraintLog",
                   constrModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/UpdateConstraintLog, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/UpdateConstraintLog, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> GetConstraintContactList()
        public List<ConstraintContactModel> GetConstraintContactList(int pageNumber, int pageSize, long ConstraintID, short ContactNo = 0)
        {
            List<ConstraintContactModel> constraintContacts = new List<ConstraintContactModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&ConstraintID=" + ConstraintID + "&ContactNo=" + ContactNo;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetConstraintContactList" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    constraintContacts = response.Content.ReadAsAsync<List<ConstraintContactModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetConstraintContactList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetConstraintContactList, Exception: {0}", ex));
            }
            return constraintContacts;
        }
        #endregion

        #region List<Object> GetNotificationExceedingConstring()
        public List<ConstraintModel> GetNotificationExceedingConstring(int pageNumber, int pageSize, long ConstraintID, int UserID)
        {
            List<ConstraintModel> constraintModel = new List<ConstraintModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&ConstraintID=" + ConstraintID;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetNotificationExceedingConstring" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    constraintModel = response.Content.ReadAsAsync<List<ConstraintModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetNotificationExceedingConstring, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetNotificationExceedingConstring, Exception: {0}", ex));
            }
            return constraintModel;
        }
        #endregion

        #region List<Object> GetConstraintListForOrg()
        public List<RouteConstraints> GetConstraintListForOrg(int organisationID, string userSchema, int otherOrg, int left, int right, int bottom, int top)
        {
            List<RouteConstraints> constraintModel = new List<RouteConstraints>();
            try
            {
                string urlParameters = "?organisationID=" + organisationID + "&userSchema=" + userSchema + "&otherOrg=" + otherOrg + "&left=" + left + "&right=" + right + "&bottom=" + bottom + "&top=" + top;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetConstraintListForOrg{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    constraintModel = response.Content.ReadAsAsync<List<RouteConstraints>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetConstraintListForOrg, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetConstraintListForOrg, Exception: {0}", ex));
            }
            return constraintModel;
        }
        #endregion

        #region private Object GetConstraints()
        public List<RouteConstraints> GetConstraints()
        {
            HttpResponseMessage response = httpClient.PostAsync(
            $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
            $"/Constraint/GetConstraints", null
            ).Result;

            //api call to new service
            List<RouteConstraints> routeConstraints = new List<RouteConstraints>();
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                routeConstraints = response.Content.ReadAsAsync<List<RouteConstraints>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuthenticationService/GetConstraints, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return routeConstraints;
        }
        #endregion

        #region List<Object> FindLinksOfAreaConstraint()
        public bool FindLinksOfAreaConstraint(sdogeometry polygonGeometry, int orgId, int userType)
        {
            bool res = false;
            try
            {
                string polygonGeometrystring = JsonConvert.SerializeObject(polygonGeometry);
                string urlParameters = "?polygonGeometrystring=" + polygonGeometrystring + "&organisationId=" + orgId + "&userType=" + userType;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/FindLinksOfAreaConstraint" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/FindLinksOfAreaConstraint, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/FindLinksOfAreaConstraint, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> SaveConstraintContact()
        public bool SaveConstraintContact(ConstraintContactModel constrModel)
        {
            bool res = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/Constraint/SaveConstraintContact",
                   constrModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/SaveConstraintContact, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/SaveConstraintContact, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region List<Object> DeleteConstrainContact()
        public int DeleteContact(short contactNo, long constraintId)
        {
            int affectedRows = 0;
            try
            {
                string urlParameters = "?contactNo=" + contactNo + "&constraintId=" + constraintId;
                HttpResponseMessage response = httpClient.DeleteAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/DeleteContact" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    affectedRows = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/DeleteContact, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/DeleteConstrainContact, Exception: {0}", ex));
            }
            return affectedRows;
        }
        #endregion

        #region RouteAssessmentModel GetAffectedStructuresConstraints()
        public RouteAssessmentModel GetAffectedStructuresConstraints(int NotificationId, string EsdalRefNum, string HaulierMnemonic, string VersionNo, string userSchema = UserSchema.Portal, int INBOX_ID = 0)
        {
            RouteAssessmentModel res = new RouteAssessmentModel();
            try
            {
                string urlParameters = "?NotificationId=" + NotificationId + "&EsdalRefNum=" + EsdalRefNum + "&HaulierMnemonic=" + HaulierMnemonic + "&VersionNo=" + VersionNo + "&userSchema=" + userSchema + "&INBOX_ID=" + INBOX_ID;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetAffectedStructuresConstraints" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<RouteAssessmentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ConstraintService/GetAffectedStructuresConstraints, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("ConstraintService/GetAffectedStructuresConstraints, Exception: {0}", ex));
            }
            return res;
        }
        #endregion

        #region  GetNotificationAffectedStructuresConstraint
        public RouteAssessmentModel GetNotificationAffectedStructuresConstraint(int inboxId, int organisationId)
        {
            RouteAssessmentModel res = new RouteAssessmentModel();
            try
            {
                string urlParameters = "?inboxId=" + inboxId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/Constraint/GetNotificationAffectedStructuresConstraint" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<RouteAssessmentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Constraint/GetNotificationAffectedStructuresConstraint, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/GetNotificationAffectedStructuresConstraint, Exception: {0}", ex));
            }
            return res;
        }
        #endregion
        
        public List<ConstraintModel> GetConstraintList(int orgId, int pageNum, int pageSize, SearchConstraintsFilter objSearchConstraints)
        {

            List<ConstraintModel> summaryObjList = new List<ConstraintModel>();
            try
            {
                ConstrainteListParams structureListParams = new ConstrainteListParams
                {
                    OrganisationId = orgId,
                    PageNumber = pageNum,
                    PageSize = pageSize,
                    ObjSearchConstraintsFilter = objSearchConstraints
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                           $"/Constraint/GetConstraintList",
                           structureListParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    summaryObjList = response.Content.ReadAsAsync<List<ConstraintModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Constraints/GetConstraintList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Constraints/GetConstraintList, Exception: {0}", ex);
            }
            return summaryObjList;
        }

    }
}
