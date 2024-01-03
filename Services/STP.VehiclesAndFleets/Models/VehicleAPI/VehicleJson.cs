using Newtonsoft.Json;
using System.Collections.Generic;

namespace STP.VehiclesAndFleets.Models
{
    public partial class VehicleJson
    {
        [JsonProperty("Vehicle")]
        public Vehicle Vehicle { get; set; }

    }
    public partial class Vehicle
    {
        [JsonProperty("VehicleId")]
        public long VehicleId { get; set; }
        [JsonProperty("UnitSystem")]
        public string UnitSystem { get; set; }
        [JsonProperty("VehicleConfiguration")]
        public VehicleConfiguration VehicleConfiguration { get; set; }
        [JsonProperty("VehicleComponents")]
        public List<VehicleComponents> VehicleComponents { get; set; }

    }
    public partial class VehicleConfiguration
    {
        [JsonProperty("OrganisationId")]
        public long OrganisationId { get; set; }
        [JsonProperty("FormalName")]
        public string FormalName { get; set; }
        [JsonProperty("InternalName")]
        public string InternalName { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("MovementClassification")]
        public string MovementClassification { get; set; }
        [JsonProperty("VehicleType")]
        public string VehicleType { get; set; }
        [JsonProperty("OverallLength")]
        public double? OverallLength { get; set; }
        [JsonProperty("RigidLength")]
        public double? RigidLength { get; set; }
        [JsonProperty("Width")]
        public double? Width { get; set; }
        [JsonProperty("MaxHeight")]
        public double? MaxHeight { get; set; }
        [JsonProperty("GrossWeight")]
        public double? GrossWeight { get; set; }
        [JsonProperty("MaxAxleWeight")]
        public double? MaxAxleWeight { get; set; }
        [JsonProperty("WheelBase")]
        public double? WheelBase { get; set; }
        [JsonProperty("Speed")]
        public double? Speed { get; set; }
        [JsonProperty("TyreSpacing")]
        public double? TyreSpacing { get; set; }
        [JsonProperty("VehicleRegistration")]
        public List<VehicleRegistration> VehicleRegistration { get; set; }
    }
    public partial class VehicleCompList
    {
        public long VehicleId { get; set; }
        public long ComponentId { get; set; }
        public string ComponentType { get; set; }
        public int LatPosition { get; set; }
        public int LongPosition { get; set; }

    }
    public class VehicleRegistration
    {
        public int SerialNumber { get; set; }
        public string FleetId { get; set; }
        public string Registration { get; set; }
    }
    public partial class VehicleComponents
    {
        public string FormalName { get; set; }
        public string InternalName { get; set; }
        public string ComponentType { get; set; }
        public string ComponentSubType { get; set; }
        public string Description { get; set; }
        public string CouplingType { get; set; }
        public string RearSteerable { get; set; }

        public double? RigidLength { get; set; }
        public double? Width { get; set; }
        public double? MaxHeight { get; set; }
        public double? ReducibleHeight { get; set; }
        public double? GrossWeight { get; set; }
        public double? MaxAxleWeight { get; set; }
        public int? AxleCount { get; set; }
        public List<Axle> Axles { get; set; }

        public double? AxleSpacingToFollowing { get; set; }
        public double? WheelBase { get; set; }
        public double? LeftOverhang { get; set; }
        public double? RightOverhang { get; set; }
        public double? FrontOverhang { get; set; }
        public double? RearOverhang { get; set; }
        public double? GroundClearance { get; set; }
        public double? ReducibleGroundClearance { get; set; }
        public double? OutsideTrack { get; set; }
        public List<VehicleRegistration> ComponentRegistration { get; set; }
    }
    public partial class Axle
    {
        public int AxleNumber { get; set; }
        public int NoOfWheels { get; set; }
        public double AxleWeight { get; set; }
        public double DistanceToNextAxle { get; set; }
        public string TyreSize { get; set; }
        public List<double> TyreSpacing { get; set; }
    }

}