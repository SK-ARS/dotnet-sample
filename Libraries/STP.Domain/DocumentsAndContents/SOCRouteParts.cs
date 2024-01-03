using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class SOCRouteParts
    {
        public long RoutePartId { get; set; }
        public string RPName { get; set; }
        public long VehicleId { get; set; }
        public string RVName { get; set; }
        public string Orderno { get; set; }
        public string RecomTemplate { get; set; }
    }

    public class SOCoverageDetails
    {
        public string OrderNo { get; set; }
        public string Applicability { get; set; }
    }

    public class SOCRoutePartsModel
    {
        public SOCRoutePartsModel()
        {
            this.Vehicles = new List<SOCRouteVehicle>();
        }
        public long RoutePartId { get; set; }
        public string RPName { get; set; }
        public List<SOCRouteVehicle> Vehicles { get; set; }
        public int VCount { get; set; }
    }
    public class SOCRouteVehicle
    {
        public long VehicleId { get; set; }
        public string RVName { get; set; }
        public bool Include { get; set; }
        public bool isExist { get; set; }
        public string OrderNo { get; set; }
    }
}