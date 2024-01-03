namespace STP.Domain.Routes
{
    public class ListRouteVehicleId
    {
        public long RoutePartId { get; set; }
        public long VehicleId { get; set; }
        public string PartDescr { get; set; }
        public int PointNO { get; set; }
        public int RouteCount { get; set; }
        public string PartName { get; set; }
        public string ComponentIdList { get; set; }
        public string ComponentTypeList { get; set; }
    }
}