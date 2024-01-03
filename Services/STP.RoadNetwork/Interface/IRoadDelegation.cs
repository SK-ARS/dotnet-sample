using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.RoadNetwork.RoadDelegation;
using NetSdoGeometry;

namespace STP.RoadNetwork.Interface
{
    interface IRoadDelegation
    {
        List<RoadDelegationData> GetRoadDelegationList(RoadDelegationSearchParam searchParam, int pageSize, int pageNumber, int sortOrder, int presetFilter);
        List<RoadDelegationOrgSummary> GetRoadDelegationOrganisations(string orgName, int page, int pageSize, int searchFlag);
        bool IsDelegationAllowed(int orgId);
        RoadDelegationData GetRoadDelegationDetailsWithLinkInfo(int arrangementId);
        RoadDelegationData GetRoadDelegationDetails(int arrangementId);
        int DeleteRoadDelegation(long arrangementId);
        List<DelegationArrangementDetails> GetArrangementDetails(int orgId);
        RoadDelegationOrgSummary GetOrganisationGeoRegion(int orgId);
        List<long> GetLinksAllowedForDelegation(List<long> linkIdList, int fromOrgId);
        bool CreateRoadDelegation(RoadDelegationData roadDelegationObj);
        List<LinkInfo> FetchRoadInfoToDisplayOnMap(int arrangementId, int zoomLevel, int searchFlag, sdogeometry areaGeom, RoadDelegationSearchParam roadDelegSearchParam);
        bool EditRoadDelegation(RoadDelegationData roadDelegationObj);
    }
}
