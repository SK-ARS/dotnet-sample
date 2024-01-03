using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class UpdateBrokenRoutePointsParam
    {
        public RoutePoint routePoint { get; set; }
        public int is_lib { get; set; }
        public string userSchema { get; set; }
    }
}