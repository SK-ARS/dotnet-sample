using System;
using System.Collections.Generic;
using NetSdoGeometry;

namespace STP.RoadNetwork.Models.RoadDelegation
{
    public class GetRoadDelegationListParams
    {
        public RoadDelegationSearchParam searchParam { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
    public class GetLinksAllowedForDelegationParams
    {
        public List<long> linkIds { get; set; }
        public int fromOrgId { get; set; }
    }
    public class FetchRoadInfoParams
    {
        public int arrangementId { get; set; }
        public int searchFlag { get; set; }
        public string areaGeomStr { get; set; }
        public RoadDelegationSearchParam searchParam { get; set; }
        public int zoomLevel { get; set; }
    }
}