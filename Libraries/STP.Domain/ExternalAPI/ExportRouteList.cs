namespace STP.Domain.ExternalAPI
{
    public class ExportRouteList
    {
        public long RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
    }

    public class Route
    {
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
        public string GPX { get; set; }
    }

    public class CheckRouteExportable
    {
        public int RouteCount { get; set; }
        public int IsBroken { get; set; }
        public int IsMultiPath { get; set; }
        public int IsMultiSegment { get; set; }
    }
    public class GetRouteExportList
    {
        public long RevisionId { get; set; }
        public long VersionId { get; set; }
        public string ContentRefNo { get; set; }
        public int NotificationType { get; set; }
        public long AnalysisId { get; set; }
        public long OrganisationId { get; set; }
        public string UserSchema { get; set; }
        public byte[] RouteDescription { get; set; }
    }
}
