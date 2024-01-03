using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class ConfigurationModel
    {

        public int? VehiclePurpose { get; set; }
        public int OrganisationId { get; set; }
        public long ConfigurationId { get; set; }
        public int ConfigurationTypeId { get; set; }
        public int MovementClassificationId { get; set; }
        public string FormalName { get; set; }
        public string InternalName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int VehicleIntDesc { get; set; }
        public int? VehicleType { get; set; }
        public string Description { get; set; }
        public int ComponentType { get; set; }
        public double? MaxHeight { get; set; }
        public int? MaxHeightUnit { get; set; }
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
        public int? AxleCount { get; set; }
        public double? TrailerWeight { get; set; }
        public double? TractorWeight { get; set; }
        public double? TrainWeight { get; set; }
        public int? TrailerAxleCount { get; set; }
        public int LeadingComponentType { get; set; }
    }

    public class VehicleMovementType
    {
        public int VehicleClass { get; set; }
        public int MovementType { get; set; }
        public int SOANoticePeriod { get; set; }
        public int PoliceNoticePeriod { get; set; }
        public string Message { get; set; }
        public int VehiclePurpose { get; set; }

        public VehicleMovementType()
        {
            SOANoticePeriod = 0;
            PoliceNoticePeriod = 0;
            Message = "";
        }
    }
}