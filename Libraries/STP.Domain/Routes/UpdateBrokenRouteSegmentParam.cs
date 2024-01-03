using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class UpdateBrokenRouteSegmentParam
    {
        public RouteSegment routeSegment { get; set; }
        public int is_lib { get; set; }
        public string userSchema { get; set; }
    }
}