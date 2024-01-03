using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class NewConfigurationModel
    {
        public int? VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleIntDesc { get; set; }
        public int? VehicleType { get; set; }
        public int? VehiclePurpose { get; set; }
        public int? OrganisationId { get; set; }
        public string VehicleDesc { get; set; }
        public double? Length { get; set; }
        public int? LengthUnit { get; set; }
        public double? LengthMtr { get; set; }
        public double? RigidLength { get; set; }
        public int? RigidLengthUnit { get; set; }
        public int? RigidLengthMtr { get; set; }
        public double? Width { get; set; }
        public int? WidthUnit { get; set; }
        public double? WidthMtr { get; set; }
        public double? GrossWeight { get; set; }
        public int? GrossWeightUnit { get; set; }
        public int? GrossWeightKg { get; set; }
        public double? MaxHeight { get; set; }
        public int? MaxHeightUnit { get; set; }
        public double? MaxHeightMtr { get; set; }
        public double? RedHeightMtr { get; set; }
        public double? MaxAxleWeight { get; set; }
        public int? MaxAxleWeightUnit { get; set; }
        public int? MaxAxleWeightKg { get; set; }
        public double? WheelBase { get; set; }
        public int? WheelBaseUnit { get; set; }
        public double? Speed { get; set; }
        public int? SpeedUnit { get; set; }
        public double? TyreSpacing { get; set; }
        public int? TyreSpacingUnit { get; set; }
        public int? ApplicationRevisionId { get; set; }
        public int? PartId { get; set; }
        public long RoutePartId { get; set; }
        public int RevisionId { get; set; }
        public string ContentRefNo { get; set; }
        public int VersionId { get; set; }
        public int CandRevisionId { get; set; }
        public int MovementId { get; set; }
        public int TractorAxleCount { get; set; }
        public int TrailerAxleCount { get; set; }
    }
    public class CreateConfigurationParams
    {
        public NewConfigurationModel ConfigurationDetails { get; set; }
        public string UserSchema { get; set; }
    }
}
