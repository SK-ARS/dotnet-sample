using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using STP.RoadNetwork.Interface;
using STP.Domain.RoadNetwork.RoadDelegation;
using STP.RoadNetwork.Persistance;
using NetSdoGeometry;

namespace STP.RoadNetwork.Providers
{
    public sealed class RoadDelegation : IRoadDelegation
    {
        #region RouteManagerProvider Singleton

        private RoadDelegation()
        {
        }
        public static RoadDelegation Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly RoadDelegation instance = new RoadDelegation();
        }
        #endregion

        #region Logger instance

        private const string PolicyName = "RoadDelegation";

        #endregion

        public List<RoadDelegationData> GetRoadDelegationList(RoadDelegationSearchParam searchParam, int pageSize, int pageNumber, int sortOrder, int presetFilter)
        {
            return RoadDelegationDAO.GetRoadDelegationList(searchParam, pageSize, pageNumber, sortOrder, presetFilter);
        }
        public List<RoadDelegationOrgSummary> GetRoadDelegationOrganisations(string orgName, int page, int pageSize, int searchFlag)
        {
            return RoadDelegationDAO.GetRoadDelegationOrganisations(orgName, page, pageSize, searchFlag);
        }
        public bool IsDelegationAllowed(int orgId)
        {
            return RoadDelegationDAO.IsDelegationAllowed(orgId);
        }
        public RoadDelegationData GetRoadDelegationDetailsWithLinkInfo(int arrangementId)
        {
            return RoadDelegationDAO.GetRoadDelegationDetailsWithLinkInfo(arrangementId);
        }
        public RoadDelegationData GetRoadDelegationDetails(int arrangementId)
        {
            return RoadDelegationDAO.GetRoadDelegationDetails(arrangementId);
        }
        public int DeleteRoadDelegation(long arrangementId)
        {
            return RoadDelegationDAO.DeleteRoadDelegation(arrangementId);
        }
        public List<DelegationArrangementDetails> GetArrangementDetails(int orgId)
        {
            return RoadDelegationDAO.GetArrangementDetails(orgId);
        }
        public RoadDelegationOrgSummary GetOrganisationGeoRegion(int orgId)
        {
            return RoadDelegationDAO.GetOrganisationGeoRegion(orgId);
        }
        public List<long> GetLinksAllowedForDelegation(List<long> linkIdList, int fromOrgId)
        {
            return RoadDelegationDAO.GetLinksAllowedForDelegation(linkIdList, fromOrgId);
        }
        public bool CreateRoadDelegation(RoadDelegationData roadDelegationObj)
        {
            return RoadDelegationDAO.CreateRoadDelegation(roadDelegationObj);
        }
        public List<LinkInfo> FetchRoadInfoToDisplayOnMap(int arrangementId, int zoomLevel, int searchFlag, sdogeometry areaGeom, RoadDelegationSearchParam roadDelegSearchParam)
        {
            return RoadDelegationDAO.FetchRoadInfoToDisplayOnMap(arrangementId, zoomLevel, searchFlag, areaGeom, roadDelegSearchParam);
        }
        public bool EditRoadDelegation(RoadDelegationData roadDelegationObj)
        {
            return RoadDelegationDAO.EditRoadDelegation(roadDelegationObj);
        }
    }
}