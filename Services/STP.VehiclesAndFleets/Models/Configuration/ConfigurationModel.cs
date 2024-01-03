using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class ConfigurationModel
    {
       
        public string vehicle_purpose { get; set; }
        public int oraganisationid { get; set; }
        public long ConfigurationId { get; set; }
        public int ConfigurationTypeID { get; set; }
        public int MovementClassificationID { get; set; }
        public string FormalName { get; set; }
        public string InternalName { get; set; }
        public int vehicle_id { get; set; }
        public string vehicle_name { get; set; }
        public int vehicle_int_desc { get; set; }
        public string vehicle_type { get; set; }
        public string Description { get; set; }
        public int ComponentType { get; set; }
        public double? Maxheight { get; set; }
        public int? MaxheightUnit { get; set; }
        public double? RigidLength { get; set; }
        public int? RigidLengthUnit { get; set; }
        public double? Width { get; set; }
        public int? WidthUnit { get; set; }
        public double? WheelBase { get; set; }
        public int? WheelBaseUnit { get; set; }
        public double? OverallLength { get; set; }
        public int? OverallLengthUnit { get; set; }
        public double? GrossWeight { get; set; }
        public int? GrossWeightUnit { get; set; }
        public double? MaxAxleWeight { get; set; }
        public int? MaxAxleWeightUnit { get; set; }
        public double? TravellingSpeed { get; set; }
        public int? TravellingSpeedUnit { get; set; }
        public double? TyreSpacing { get; set; }
        public int? TyreSpacingUnit { get; set; }
        public double? ReducedHeight { get; set; }
        public int? ReducedHeightUnit { get; set; }
        public double? MaxReducedHeight { get; set; }
        public double? NotifFrontOverhang { get; set; }
        public double? NotifRearOverhang { get; set; }
        public double? NotifRightOverhang { get; set; }
        public double? NotifLeftOverhang { get; set; }

    }
    
}