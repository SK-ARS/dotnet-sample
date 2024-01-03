using System.Collections.Generic;
using STP.Domain.RoadNetwork.RoadOwnership;
using NetSdoGeometry;
using STP.Common.Constants;


namespace STP.RoadNetwork.Interface
{
    public interface IRoadOwnership
    {
        List<RoadOwnershipOrgSummary> GetRoadOwnershipOrganisations(string orgName, int pageNum, int pageSize, int searchFlag);
        List<long> GetUnassignedLinks(List<long> linkIds);
        List<ArrangementDetails> GetDelegationArrangementDetails(int orgId);
        List<RoadContactModal> GetRoadOwnerContactList(long linkID, long length, string pageType, string userSchema = UserSchema.Portal);
        List<RoadOwnershipData> GetRoadOwnershipDetails(List<long> linkIds);
        bool SaveRoadOwnership(RoadOwnerShipDetails roadOwnerObj);
        List<LinkInfo> FetchRoadInfoToDisplayOnMap(int organisationId, int fetchFlag, sdogeometry areaGeom, int zoomLevel);
    }
}