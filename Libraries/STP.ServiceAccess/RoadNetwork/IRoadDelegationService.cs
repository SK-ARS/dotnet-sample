using System;
using System.Collections.Generic;
using STP.Domain.RoadNetwork.RoadDelegation;
using NetSdoGeometry;
using STP.Domain.RoadNetwork.RoadOwnership;

namespace STP.ServiceAccess.RoadNetwork
{
    public interface IRoadDelegationService
    {
        List<RoadDelegationData> GetRoadDelegationList(RoadDelegationSearchParam searchParam, int pageSize, int pageNumber,int sortOrder, int SortType);
        List<RoadDelegationOrgSummary> GetRoadDelegationOrganisations(string orgName, int pageNum, int pageSize, int searchFlag);
        bool IsDelegationAllowed(int orgId);
        RoadDelegationData GetRoadDelegationDetailsWithLinkInfo(int delegationArrId);
        RoadDelegationData GetRoadDelegationDetails(int delegationArrId);
        int DeleteRoadDelegation(int delegationArrId);
        List<DelegationArrangementDetails> GetArrangementDetails(int orgId);
        RoadDelegationOrgSummary GetOrganisationGeoRegion(int orgId);
        List<long> GetLinksAllowedForDelegation(List<long> linkIds, int fromOrgId);
        bool CreateRoadDelegation(RoadDelegationData roadDelegationObj);
        //List<LinkInfo> FetchRoadInfoToDisplayOnMap(int arrangementId, int zoomLevel, int searchFlag, sdogeometry areaGeom, RoadDelegationSearchParam roadDelegSearchParam);
        bool EditRoadDelegation(string roadDelegationObj);
		List<RoadOwnershipOrgSummary> GetOrganisations(string searchString, int page, int pageSize, int searchFlag);
		List<Domain.RoadNetwork.RoadDelegation.LinkInfo> FetchRoadInfoToDisplayOnMap(int arrangementId, int zoomLevel, int fetchFlag, sdogeometry areaGeom, RoadDelegationSearchParam searchParam);
	}
}
