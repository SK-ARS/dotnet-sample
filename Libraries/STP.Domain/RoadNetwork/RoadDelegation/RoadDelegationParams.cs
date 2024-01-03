using System.Collections.Generic;

namespace STP.Domain.RoadNetwork.RoadDelegation
{
    public class GetRoadDelegationListParams
    {
        public RoadDelegationSearchParam SearchParam { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int SortOrder { get; set; }
        public int SortTye { get; set; }
    }
    public class GetLinksAllowedForDelegationParams
    {
        public List<long> LinkIdList { get; set; }
        public int FromOrgId { get; set; }
    }
    public class FetchRoadInfoParams
    {
        public int ArrangementId { get; set; }
        public int SearchFlag { get; set; }
        public string AreaGeometryStr { get; set; }
        public RoadDelegationSearchParam SearchParam { get; set; }
        public int ZoomLevel { get; set; }
    }
}