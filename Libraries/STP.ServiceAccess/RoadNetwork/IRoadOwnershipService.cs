using System;
using System.Collections.Generic;
using STP.Domain.RoadNetwork.RoadOwnership;
using NetSdoGeometry;

namespace STP.ServiceAccess.RoadNetwork
{
    public interface IRoadOwnershipService
    {/// <summary>
     /// This interface contains api calls related with Road Ownership
     /// defined in the  RoadOwnershipController.cs  in STP.RoadNetwork Service.
     /// </summary>
        List<RoadOwnershipOrgSummary> GetRoadOwnershipOrganisations(string orgName, int pageNum, int pageSize, int searchFlag);
        List<ArrangementDetails> GetDelegationArrangementDetails(int orgId);
        List<long> GetUnassignedLinks(List<long> linkIds);
        List<RoadContactModal> GetRoadOwnerContactList(long linkId, long length, string pageType, string userSchema);
        List<RoadOwnershipData> GetRoadOwnershipDetails(List<LinkInfo> linkIdInfo, int page, int pageSize);
        bool SaveRoadOwnership(RoadOwnerShipDetails roadOwnershipObj);
        List<LinkInfo> FetchRoadInfoToDisplayOnMap(int organisationId, int fetchFlag, sdogeometry areaGeom, int zoomLevel);
    }
}
