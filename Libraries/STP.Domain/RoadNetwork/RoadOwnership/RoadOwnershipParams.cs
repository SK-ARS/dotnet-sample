
namespace STP.Domain.RoadNetwork.RoadOwnership
{    
    public class FetchRoadInfoParams
    {
        public int OrganisationId { get; set; }
        public int FetchFlag { get; set; }
        public string AreaGeometryStr { get; set; }
        public int ZoomLevel { get; set; }

    }    
}