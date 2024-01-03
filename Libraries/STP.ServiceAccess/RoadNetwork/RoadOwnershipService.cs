using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.Common.Logger;
using NetSdoGeometry;
using Newtonsoft.Json;

namespace STP.ServiceAccess.RoadNetwork
{
    public class RoadOwnershipService : IRoadOwnershipService
    {
        private readonly HttpClient httpClient;

        public RoadOwnershipService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region List<Object> GetRoadOwnershipOrganisations()
        public List<RoadOwnershipOrgSummary> GetRoadOwnershipOrganisations(string orgName, int pageNum, int pageSize, int searchFlag)
        {
            List<RoadOwnershipOrgSummary> objListOrganisations = null;
            try
            {
                string urlParameters = "?orgName=" + orgName + "&pageNum=" + pageNum + "&pageSize=" + pageSize + "&searchFlag=" + searchFlag;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/RoadOwnership/GetRoadOwnershipOrganisations" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objListOrganisations = response.Content.ReadAsAsync<List<RoadOwnershipOrgSummary>>().Result;
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

        #region List<Object> GetDelegationArrangementDetails()
        public List<ArrangementDetails> GetDelegationArrangementDetails(int orgId)
        {
            List<ArrangementDetails> objListDelegationArrangements = null;
            try
            {
                string urlParameters = "?orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/RoadOwnership/GetDelegationArrangementDetails" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objListDelegationArrangements = response.Content.ReadAsAsync<List<ArrangementDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadOwnership/GetDelegationArrangementDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/GetDelegationArrangementDetails, Exception: {0}", ex));
            }
            return objListDelegationArrangements;
        }
        #endregion

        #region List<Object> GetUnassignedLinks()
        public List<long> GetUnassignedLinks(List<long> linkIds)
        {
            List<long> objUnassignedlinkIds = new List<long>();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadOwnership/GetUnassignedLinks",
                   linkIds).Result;

                if (response.IsSuccessStatusCode)
                {
                    objUnassignedlinkIds = response.Content.ReadAsAsync<List<long>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadOwnership/GetUnassignedLinks, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/GetUnassignedLinks, Exception: {0}", ex));
            }
            return objUnassignedlinkIds;
        }
        #endregion

        #region List<Object> GetRoadOwnerContactList()
        public List<RoadContactModal> GetRoadOwnerContactList(long linkId, long length, string pageType, string userSchema)
        {
            List<RoadContactModal> objRoadContactList = new List<RoadContactModal>();
            try
            {
                string urlParameters = "?linkId=" + linkId + "&length=" + length + "&pageType=" + pageType + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                    $"/RoadOwnership/GetRoadOwnerContactList" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objRoadContactList = response.Content.ReadAsAsync<List<RoadContactModal>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadOwnership/GetRoadOwnerContactList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/GetRoadOwnerContactList, Exception: {0}", ex));
            }
            return objRoadContactList;
        }
        #endregion

        #region List<Object> GetRoadOwnershipDetails()
        public List<RoadOwnershipData> GetRoadOwnershipDetails(List<LinkInfo> linkIdInfo, int page, int pageSize)
        {
            List<RoadOwnershipData> objRoadOwnershipDetails = new List<RoadOwnershipData>();
            List<long> linkIds = new List<long>();
            try
            {
                foreach (LinkInfo item in linkIdInfo)
                {
                    linkIds.Add(item.LinkId);
                }
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                   $"/RoadOwnership/GetRoadOwnershipDetails",
                   linkIds).Result;

                if (response.IsSuccessStatusCode)
                {
                    objRoadOwnershipDetails = response.Content.ReadAsAsync<List<RoadOwnershipData>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadOwnership/GetRoadOwnershipDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/GetRoadOwnershipDetails, Exception: {0}", ex));
            }
            return objRoadOwnershipDetails;
        }
        #endregion

        #region bool SaveRoadOwnership()
        public bool SaveRoadOwnership(RoadOwnerShipDetails roadOwnershipObj)
        {
            bool saveFlag = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                  $"/RoadOwnership/SaveRoadOwnership",
                  roadOwnershipObj).Result;

                if (response.IsSuccessStatusCode)
                {
                    saveFlag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadOwnership/SaveRoadOwnership, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/SaveRoadOwnership, Exception: {0}", ex));
            }
            return saveFlag;
        }
        #endregion

        #region List<Object> FetchRoadInfoToDisplayOnMap()
        public List<LinkInfo> FetchRoadInfoToDisplayOnMap(int organisationId, int fetchFlag, sdogeometry areaGeom, int zoomLevel)
        {
            List<LinkInfo> roadLinkInfoLst = new List<LinkInfo>();
            try
            {
                FetchRoadInfoParams reqParams = new FetchRoadInfoParams
                {
                    OrganisationId = organisationId,
                    FetchFlag = fetchFlag,
                    AreaGeometryStr = JsonConvert.SerializeObject(areaGeom),
                    ZoomLevel = zoomLevel
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["RoadNetwork"]}" +
                  $"/RoadOwnership/FetchRoadInfoToDisplayOnMap",
                  reqParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    roadLinkInfoLst = response.Content.ReadAsAsync<List<LinkInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoadOwnership/FetchRoadInfoToDisplayOnMap, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/FetchRoadInfoToDisplayOnMap, Exception: {0}", ex));
            }
            return roadLinkInfoLst;
        }
        #endregion
    }
}
