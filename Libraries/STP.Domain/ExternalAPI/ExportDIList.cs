using System.Collections.Generic;

namespace STP.Domain.ExternalAPI
{
    public class ExportDIList
    {
        public string EsdalReferenceNumber { get; set; }
        public string MovementStatus { get; set; }
        public List<DIRoute> Routes { get; set; }
        public ExportDIList()
        {
            Routes = new List<DIRoute>();
        }
    }
    public class DIList
    {
        public string MovementStatus { get; set; }
        public string ContentRefNum { get; set; }
        public long VersionId { get; set; }
        public long AnalysisId { get; set; }
        public int NotificationType { get; set; }
    }
    public class DIRoute
    {
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
        public string GPX { get; set; }
        public string DrivingInstructions { get; set; }
    }
}
