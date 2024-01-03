using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class SaveAppRouteParams
    {
        public RoutePart RoutePart { get; set; }

        public long VersionId { get; set; }

        public long RevisionId { get; set; }

        public string ContRefNumber { get; set; }

        public int DockFlag { get; set; }

        public long RouteRevisionId { get; set; }

        public string UserSchema { get; set; }

        public  bool IsReturnRoute { get; set; }
        public bool IsAutoPlan { get; set; }
    }

    public class SaveNERouteParams
    {
        public GPXInputModel GPXInput { get; set; }
        public RoutePart RoutePart { get; set; }
        public long RevisionId { get; set; }
        public long NotificationId { get; set; }
        public bool IsVr1 { get; set; }
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }

    }

    public class UpdateHistoricCloneRoute
    {
        public long VersionId { get; set; }
        public long RevisionId { get; set; }
        public string ContRefNumber { get; set; }
        public string UserSchema { get; set; }
    }

    public class CloneNenRoute
    {
        public long NotificationId { get; set; }
        public string ContentRefNo { get; set; }
        public long AnalysisId { get; set; }
        public string Organisations { get; set; }
        public int OrgCount { get; set; }
    }

    public class NenRouteList
    {
        public long AnalysisId { get; set; }
        public long RoutePartId { get; set; }
        public string RouteName { get; set; }
        public string RouteType { get; set; }
        public long OrganisationId { get; set; }
    }
}