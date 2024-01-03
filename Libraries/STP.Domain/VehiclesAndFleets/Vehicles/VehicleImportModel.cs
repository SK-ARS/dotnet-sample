using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STP.Domain.VehiclesAndFleets.Vehicles
{
    public partial class VehicleImportModel
    {
        [JsonProperty("VehicleId")]
        public long VehicleId { get; set; }
        [JsonProperty("UnitSystem")]
        public string UnitSystem { get; set; }
        [JsonProperty("VehicleConfiguration")]
        public VehicleConfigDetails VehicleConfigDetails { get; set; }
        [JsonProperty("VehicleComponents")]
        public List<VehicleComponentDetails> VehicleComponentDetails { get; set; }
        public ValidationError VehicleError { get; set; }

    }
    public partial class VehicleConfigDetails
    {
        public long OrganisationId { get; set; }
        public string FormalName { get; set; }
        public string InternalName { get; set; }
        public string Description { get; set; }
        public int MovementClassification { get; set; }
        public int VehicleType { get; set; }
        public double? OverallLength { get; set; }
        public double? RigidLength { get; set; }
        public double? Width { get; set; }
        public double? MaxHeight { get; set; }
        public double? GrossWeight { get; set; }
        public double? MaxAxleWeight { get; set; }
        public double? WheelBase { get; set; }
        public double? Speed { get; set; }
        public double? TyreSpacing { get; set; }
        public List<VehicleRegistration> VehicleRegistration { get; set; }
        public ValidationError VehicleConfigError { get; set; }
    }
    public class ValidationError
    {
        public int ErrorCount { get; set; }
        public string ErrorMessage { get; set; }
    }
    public partial class VehicleComponentMapping
    {
        public long VehicleId { get; set; }
        public long ComponentId { get; set; }
        public int ComponentType { get; set; }
        public int LatPosition { get; set; }
        public int LongPosition { get; set; }

    }
    public class VehicleRegistrations
    {
        public int SerialNumber { get; set; }
        public string FleetId { get; set; }
        public string Registration { get; set; }
    }
    public partial class VehicleComponentDetails
    {
        public string FormalName { get; set; }
        public string InternalName { get; set; }
        public int ComponentType { get; set; }
        public int ComponentSubType { get; set; }
        public string Description { get; set; }
        public int CouplingType { get; set; }
        public string RearSteerable { get; set; }

        public double? RigidLength { get; set; }
        public double? Width { get; set; }
        public double? MaxHeight { get; set; }
        public double? ReducibleHeight { get; set; }
        public double? GrossWeight { get; set; }
        public double? MaxAxleWeight { get; set; }
        public int? AxleCount { get; set; }
        public List<Axles> Axles { get; set; }

        public double? AxleSpacingToFollowing { get; set; }
        public double? WheelBase { get; set; }
        public double? LeftOverhang { get; set; }
        public double? RightOverhang { get; set; }
        public double? FrontOverhang { get; set; }
        public double? RearOverhang { get; set; }
        public double? GroundClearance { get; set; }
        public double? ReducibleGroundClearance { get; set; }
        public double? OutsideTrack { get; set; }
        public int VehiclePurpose { get; set; }
        public List<VehicleRegistration> ComponentRegistration { get; set; }
        public ValidationError VehicleComponentError { get; set; }
    }
    public partial class Axles
    {
        public int AxleNumber { get; set; }
        public int NoOfWheels { get; set; }
        public double AxleWeight { get; set; }
        public double DistanceToNextAxle { get; set; }
        public string TyreSize { get; set; }
        public string TyreSpacing { get; set; }
    }   
}