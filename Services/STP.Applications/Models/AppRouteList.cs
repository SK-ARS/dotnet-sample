namespace STP.Applications.Models
{
    public class AppRouteList
    {
        public long RouteID { get; set; }
        public string RouteName { get; set; }
        public string RouteType { get; set; }
        public string RouteDescription { get; set; }
        public string TransportMode { get; set; }
        public string RoutePart { get; set; }
        public long PartNo { get; set; }
        public int RID { get; set; }
        public int PNo { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public long NewPartNo { get; set; }
        public string NENRouteStatus { get; set; }
    }
}