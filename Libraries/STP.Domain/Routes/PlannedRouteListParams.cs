namespace STP.Domain.Routes
{
    public class PlannedRouteListParams
    {
        public int organisationID { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int routeType { get; set; }
        public string serchString { get; set; }
        public string userSchema { get; set; }
    }

    public class RetunRouteObject
    {
        public string StartDescr { get; set; }
        public decimal? StartEasting { get; set; }
        public decimal? StartNorthing { get; set; }
        public long RouteId { get; set; }
        public string EndDescr { get; set; }
        public decimal? EndEasting { get; set; }
        public decimal? EndNorthing { get; set; }
    }
    public class ReturnRouteMapping
    {
        public long MainRouteId { get; set; }
        public long ReturnRouteId { get; set; }
    }
}