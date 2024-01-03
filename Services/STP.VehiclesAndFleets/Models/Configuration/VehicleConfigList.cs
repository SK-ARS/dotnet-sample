using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class VehicleConfigList
    {
        public long vehicle_id { get; set; }
        public long component_id { get; set; }
        public int sub_type { get; set; }
        public int lat_posn { get; set; }
        public int long_posn { get; set; }
        public double space_to_following { get; set; }
        public int space_to_following_unit { get; set; }
        public long route_part_id { get; set; }
        public long componentType { get; set; }
        public string component_Type { get; set; }
        public string component_subType { get; set; }
        public double? ground_clearance { get; set; }
        public double? Red_Height { get; set; }
        public double? front_overhang { get; set; }
        public double? rear_overhang { get; set; }
        public decimal is_steerable_at_rear { get; set; }
        public double? wheelbase { get; set; }
        public double? RedGroundClearance { get; set; }
        public double? outside_track { get; set; }
        public double? left_overhang { get; set; }
        public double? right_overhang { get; set; }
        public double? len { get; set; }
        public double? width { get; set; }
        public string vehicle_desc { get; set; }
        public double? gross_weight { get; set; }
        public double? Rigid_Len { get; set; }
    }
}