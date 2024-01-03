using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class AppVehicleConfigList
    {
        public long VehicleId { get; set; }
        public int RoutePartId { get; set; }
        public string VehicleName { get; set; }
        public string RoutePart { get; set; }
        public int ApplicationPartId { get; set; }
        public string RouteType { get; set; }
        public string VehicleDescription { get; set; }
        public int VehicleType { get; set; }
        public int VehiclePurpose { get; set; }
        public long ParentVehicleId { get; set; }
        public List<VehicleConfigList> VehicleCompList { get; set; }
        public List<string> VehicleNameList { get; set; }
        public AppVehicleConfigList()
        {
            VehicleCompList = new List<VehicleConfigList>();
            VehicleNameList = new List<string>();
        }
    }
    public class VehicleList
    {
        public long VehicleId { get; set; }
        public string FormalName { get; set; }
        public string InternalName { get; set; }
        public string MovementClassification { get; set; }
        public string OrganisationName { get; set; }
        public string VehicleType { get; set; }
        public string IndendedUse { get; set; }
    }
}