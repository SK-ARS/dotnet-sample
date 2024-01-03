using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class SaveLibraryRouteParams
    {
        public RoutePart RoutePart { get; set; }
        public string UserSchema { get; set; }
    }
    public class SaveRouteAnnotationParams
    {
        public RoutePartSerialized routePartSerialized { get; set; }
        public RoutePart routePart { get; set; }
        public int type { get; set; }
        public string userSchema { get; set; }
    }
    public class AddToLibraryParams
    {
        public int OrganisationId { get; set; }
        public long RouteId { get; set; }
        public string RouteType { get; set; }
        public string UserSchema { get; set; }
    }
    public class CheckRouteName
    {
        public string RouteName { get; set; }
        public int OrganisationId { get; set; }
        public string UserSchema { get; set; }
    }

    public class ImportToLibraryParams
    {
        public int OrganisationId { get; set; }
        public string RouteName { get; set; }
        public GPXInputModel GPXInput { get; set; }
        public RoutePart RoutePart { get; set; }
    }
}