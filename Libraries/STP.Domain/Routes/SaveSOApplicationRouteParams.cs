using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class SaveSOApplicationRouteParams
    {
        public RoutePart RoutePart { get; set; }

        public int? VersionId { get; set; }

        public int? RevisionId { get; set; }

        public string ContRefNumber { get; set; }

        public int DockFlag { get; set; }

        public long? RouteRevisionId { get; set; }

        public string UserSchema { get; set; }
    }
}