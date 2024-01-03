using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using STP.Domain.RoadNetwork.RoadDelegation;
using STP.Common.Logger;
using NetSdoGeometry;
using Newtonsoft.Json;

namespace STP.ServiceAccess.RoadNetwork
{
    public class RoadDelegationService : IRoadDelegationService
    {
        private readonly HttpClient httpClient;

        public RoadDelegationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region private List<Object> GetRoadDelegationList()
        public List<RoadDelegationData> GetRoadDelegationList(RoadDelegationSearchParam searchParam, int pageSize, int pageNumber, int sortOrder, int sortType)
        {
            List<RoadDelegationData> objRoadDelegationLst = new List<RoadDelegationData>();
            try
            {
                GetRoadDelegationListParams roadDelegationParams = new GetRoadDelegationListParams
                {
                    SearchParam = searchParam,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    SortOrder=sortOrder,
                    SortTye=sortType
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadDelegation/GetRoadDelegationList",
                   roadDelegationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    objRoadDelegationLst = response.Content.ReadAsAsync<List<RoadDelegationData>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/GetRoadDelegationList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetRoadDelegationList, Exception: {0}", ex));
            }
            return objRoadDelegationLst;
        }
        #endregion

        #region List<Object> GetRoadDelegationOrganisations()
        public List<RoadDelegationOrgSummary> GetRoadDelegationOrganisations(string orgName, int pageNum, int pageSize, int searchFlag)
        {
            List<RoadDelegationOrgSummary> objListOrganisations = new List<RoadDelegationOrgSummary>();
            try
            {
                string urlParameters = "?orgName=" + orgName + "&pageNum=" + pageNum + "&pageSize=" + pageSize + "&searchFlag=" + searchFlag;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/RoadDelegation/GetRoadDelegationOrganisations" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objListOrganisations = response.Content.ReadAsAsync<List<RoadDelegationOrgSummary>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/GetRoadDelegationOrganisations, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetRoadDelegationOrganisations, Exception: {0}", ex));
            }
            return objListOrganisations;
        }
        #endregion

        #region bool IsDelegationAllowed()
        public bool IsDelegationAllowed(int orgId)
        {
            bool delegationAllowedFlg = false;
            try
            {
                string urlParameters = "?orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync(
                  $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                  $"/RoadDelegation/IsDelegationAllowed" +
                  urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    delegationAllowedFlg = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/IsDelegationAllowed, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/IsDelegationAllowed, Exception: {0}", ex));
            }
            return delegationAllowedFlg;
        }
        #endregion

        #region Object GetRoadDelegationDetailsWithLinkInfo()
        public RoadDelegationData GetRoadDelegationDetailsWithLinkInfo(int delegationArrId)
        {
            RoadDelegationData roadDelegationData = new RoadDelegationData();
            try
            {
                string urlParameters = "?delArrId=" + delegationArrId;
                HttpResponseMessage response = httpClient.GetAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadDelegation/GetRoadDelegationDetailsWithLinkInfo" +
                   urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    roadDelegationData = response.Content.ReadAsAsync<RoadDelegationData>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/GetRoadDelegationDetailsWithLinkInfo, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetRoadDelegationDetailsWithLinkInfo, Exception: {0}", ex));
            }
            return roadDelegationData;
        }
        #endregion

        #region Object GetRoadDelegationDetails()
        public RoadDelegationData GetRoadDelegationDetails(int delegationArrId)
        {
            RoadDelegationData roadDelegationData = new RoadDelegationData();
            try
            {
                string urlParameters = "?delArrId=" + delegationArrId;
                HttpResponseMessage response = httpClient.GetAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadDelegation/GetRoadDelegationDetails" +
                   urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    roadDelegationData = response.Content.ReadAsAsync<RoadDelegationData>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetRoadDelegationDetails, Error: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetRoadDelegationDetails, Exception: {0}", ex));
            }
            return roadDelegationData;
        }
        #endregion

        #region Object DeleteRoadDelegation()
        public int DeleteRoadDelegation(int delegationArrId)
        {
            int affectedRows = 0;

            try
            {
                string urlParameters = "?delArrId=" + delegationArrId;
                HttpResponseMessage response = httpClient.DeleteAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadDelegation/DeleteRoadDelegation" +
                   urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    affectedRows = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/DeleteRoadDelegation, Error: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/DeleteRoadDelegation, Exception: {0}", ex));
            }
            return affectedRows;
        }
        #endregion
        #region List<Object> GetOrganisations()
        public List<Domain.RoadNetwork.RoadOwnership.RoadOwnershipOrgSummary> GetOrganisations(string orgName, int pageNum, int pageSize, int searchFlag)
        {
            List<Domain.RoadNetwork.RoadOwnership.RoadOwnershipOrgSummary> objListOrganisations = null;
            try
            {
                string urlParameters = "?orgName=" + orgName + "&pageNum=" + pageNum + "&pageSize=" + pageSize + "&searchFlag=" + searchFlag;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/RoadDelegation/GetRoadDelegationOrganisations" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objListOrganisations = response.Content.ReadAsAsync<List<Domain.RoadNetwork.RoadOwnership.RoadOwnershipOrgSummary>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadOwnership/GetRoadOwnerOrganisations, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/GetRoadOwnerOrganisations, Exception: {0}", ex));
            }
            return objListOrganisations;
        }
        #endregion
        #region private List<Object> GetArrangementDetails()
        public List<DelegationArrangementDetails> GetArrangementDetails(int orgId)
        {
            List<DelegationArrangementDetails> objdDelegationArrLst = new List<DelegationArrangementDetails>();
            try
            {
                string urlParameters = "?orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadDelegation/GetArrangementDetails" +
                   urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objdDelegationArrLst = response.Content.ReadAsAsync<List<DelegationArrangementDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetArrangementDetails, Error: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetArrangementDetails, Exception: {0}", ex));
            }
            return objdDelegationArrLst;
        }
        #endregion

        #region private Object GetOrganisationGeoRegion()
        public RoadDelegationOrgSummary GetOrganisationGeoRegion(int orgId)
        {
            RoadDelegationOrgSummary objOrgGeoDetails = new RoadDelegationOrgSummary();
            try
            {
                string urlParameters = "?orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadDelegation/GetOrganisationGeoRegion" +
                   urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objOrgGeoDetails = response.Content.ReadAsAsync<RoadDelegationOrgSummary>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetOrganisationGeoRegion, Error: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetOrganisationGeoRegion, Exception: {0}", ex));
            }
            return objOrgGeoDetails;
        }
        #endregion

        #region List<long> GetLinksAllowedForDelegation()
        public List<long> GetLinksAllowedForDelegation(List<long> linkIds, int fromOrgId)
        {
            List<long> objAllowedLinkIds = new List<long>();
            try
            {
                GetLinksAllowedForDelegationParams reqParams = new GetLinksAllowedForDelegationParams
                {
                    LinkIdList = linkIds,
                    FromOrgId = fromOrgId
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadDelegation/GetLinksAllowedForDelegation",
                   reqParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    objAllowedLinkIds = response.Content.ReadAsAsync<List<long>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/GetLinksAllowedForDelegation, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetLinksAllowedForDelegation, Exception: {0}", ex));
            }
            return objAllowedLinkIds;
        }
        #endregion

        #region bool CreateRoadDelegation()
        public bool CreateRoadDelegation(RoadDelegationData roadDelegationObj)
        {
            bool createFlag = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                  $"/RoadDelegation/CreateRoadDelegation",
                  roadDelegationObj).Result;

                if (response.IsSuccessStatusCode)
                {
                    createFlag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/CreateRoadDelegation, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/CreateRoadDelegation, Exception: {0}", ex));
            }
            return createFlag;
        }
        #endregion

        #region List<Object> FetchRoadInfoToDisplayOnMap()
        public List<LinkInfo> FetchRoadInfoToDisplayOnMap(int arrangementId, int zoomLevel, int searchFlag, sdogeometry areaGeom, RoadDelegationSearchParam roadDelegSearchParam)
        {
            List<LinkInfo> roadLinkInfoLst = new List<LinkInfo>();
            try
            {
                FetchRoadInfoParams reqParams = new FetchRoadInfoParams
                {
                    ArrangementId = arrangementId,
                    SearchFlag = searchFlag,
                    AreaGeometryStr = JsonConvert.SerializeObject(areaGeom),
                    SearchParam = roadDelegSearchParam,
                    ZoomLevel = zoomLevel
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                  $"/RoadDelegation/FetchRoadInfoToDisplayOnMap",
                  reqParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    roadLinkInfoLst = response.Content.ReadAsAsync<List<LinkInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/FetchRoadInfoToDisplayOnMap, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/FetchRoadInfoToDisplayOnMap, Exception: {0}", ex));
            }
            return roadLinkInfoLst;
        }
        #endregion

        #region bool EditRoadDelegation()
        public bool EditRoadDelegation(string roadDelegationObj)
        {
            bool editFlag = false;
            try
            {
                RoadDelegationDataMapperInput obj = new RoadDelegationDataMapperInput() { CompressedRoadDelegationString = roadDelegationObj };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                  $"/RoadDelegation/EditRoadDelegation",
                  obj).Result;

                if (response.IsSuccessStatusCode)
                {
                    editFlag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadDelegation/EditRoadDelegation, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/EditRoadDelegation, Exception: {0}", ex));
            }
            return editFlag;
        }
        #endregion
    }
}
