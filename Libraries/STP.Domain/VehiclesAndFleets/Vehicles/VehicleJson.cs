using Newtonsoft.Json;
using System.Collections.Generic;

namespace STP.Domain.VehiclesAndFleets.Vehicles
{
    public partial class VehicleJson
    {
        [JsonProperty("Vehicle")]
        public Vehicle Vehicle { get; set; }

    }
    public partial class EsdalVehiclesJson
    {
        [JsonProperty("Vehicles")]
        public Vehicles Vehicles { get; set; }
    }
    public partial class Vehicles
    {
        [JsonProperty("ConfigurationSummaryListPosition")]
        public ConfigurationSummaryListPosition ConfigurationSummaryListPosition { get; set; }

        [JsonProperty("Configuration")]
        public Configuration Configuration { get; set; }
    }
    public partial class ConfigurationSummaryListPosition
    {
        [JsonProperty("ConfigurationSummary")]
        public string ConfigurationSummary { get; set; }

        [JsonProperty("ConfigurationComponentsNo")]
        public long ConfigurationComponentsNo { get; set; }
    }
    public partial class Configuration
    {
        [JsonProperty("ComponentListPosition")]
        public ComponentListPosition ComponentListPosition { get; set; }
    }
    public partial class ComponentListPosition
    {
        [JsonProperty("Component")]
        public List<Component> Component { get; set; }
    }
    public partial class Component
    {
        [JsonProperty("ComponentType")]
        public string ComponentType { get; set; }

        [JsonProperty("ComponentSubType")]
        public string ComponentSubType { get; set; }

        [JsonProperty("Longitude")]
        public long Longitude { get; set; }

        [JsonProperty("AxleConfiguration")]
        public AxleConfiguration AxleConfiguration { get; set; }
    }
    public partial class Vehicle
    {
        [JsonProperty("VehicleId")]
        public long VehicleId { get; set; }
        [JsonProperty("OrganisationId")]
        public long OrganisationId { get; set; }
        [JsonProperty("UnitSystem")]
        public string UnitSystem { get; set; }
        [JsonProperty("VehicleConfiguration")]
        public VehicleConfiguration VehicleConfiguration { get; set; }
        [JsonProperty("VehicleComponents")]
        public List<VehicleComponent> VehicleComponents { get; set; }
        
    }
    public partial class VehicleConfiguration
    {
        [JsonIgnore]
        [JsonProperty("OrganisationId")]
        public long OrganisationId { get; set; }
        [JsonProperty("FormalName")]
        public string FormalName { get; set; }
        [JsonProperty("InternalName")]
        public string InternalName { get; set; }
        [JsonProperty("MovementClassification")]
        public string MovementClassification { get; set; }
        [JsonProperty("VehicleType")]
        public string VehicleType { get; set; }
        [JsonProperty("GrossWeight")]
        public double? GrossWeight { get; set; }
      
       [JsonProperty("OverallLength")]
        public double? OverallLength { get; set; }
        [JsonProperty("VehicleRegistration")]
        public List<VehicleRegistration> VehicleRegistration { get; set; }
        [JsonProperty("Notes")]
        public string Description { get; set; }
        [JsonProperty("RigidLength")]
        public double? RigidLength { get; set; }
        [JsonProperty("Width")]
        public double? Width { get; set; }
        [JsonProperty("Speed")]
        public double? Speed { get; set; }
        [JsonProperty("MaxHeight")]
        public double? MaxHeight { get; set; }

        [JsonProperty("MaxAxleWeight")]
        public double? MaxAxleWeight { get; set; }
        [JsonProperty("WheelBase")]
        public double? WheelBase { get; set; }
        [JsonProperty("TyreSpacing")]
        public double? TyreSpacing { get; set; }
        
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
    public class ComponentRegistration
    {
        public int SerialNumber { get; set; }
        public string FleetId { get; set; }
        public string Registration { get; set; }
    }
    public partial class VehicleComponent
    {   
        public int ComponentNumber { get; set; }
        public string ComponentType { get; set; }
        public string ComponentSubType { get; set; }
        public string FormalName { get; set; }
        public string InternalName { get; set; }
        public double? MaxHeight { get; set; }
        public double? RigidLength { get; set; }
        public double? Width { get; set; }
        public int? AxleCount { get; set; }
        public double? OutsideTrack { get; set; }
        public double? AxleSpacingToFollowing { get; set; }
        public List<Axle> Axles { get; set; }
        public string CouplingType { get; set; }
        public string RearSteerable { get; set; }
        public string Notes { get; set; }
        public double? ReducibleHeight { get; set; }
        [JsonIgnore]
        public double? GrossWeight { get; set; }
        [JsonIgnore]
        public double? MaxAxleWeight { get; set; }
        [JsonIgnore]
        public double? WheelBase { get; set; }
        public double? LeftOverhang { get; set; }
        public double? RightOverhang { get; set; }
        public double? FrontOverhang { get; set; }
        public double? RearOverhang { get; set; }
        public double? GroundClearance { get; set; }
        public double? ReducibleGroundClearance { get; set; }
        
        public List<ComponentRegistration> ComponentRegistrations { get; set; }
    }
    public partial class Axle
    {
        public int AxleNumber { get; set; }
        public int NoOfWheels { get; set; }
        public double AxleWeight { get; set; }
        public double DistanceToNextAxle { get; set; }
        public string TyreSize { get; set; }
        public List<double> WheelSpacing { get; set; }
    }
    public partial class AxleConfiguration
    {
        [JsonProperty("NumberOfAxles")]
        public long NumberOfAxles { get; set; }

        [JsonProperty("AxleWeightListPosition")]
        public AxleWeightListPosition AxleWeightListPosition { get; set; }

        [JsonProperty("AxleSpacingListPosition")]
        public AxleSpacingListPosition AxleSpacingListPosition { get; set; }

        [JsonProperty("AxleSpacingToFollowing")]
        public double AxleSpacingToFollowing { get; set; }
    }
    public partial class AxleSpacingListPosition
    {
        [JsonProperty("AxleSpacing")]
        public List<Axles> AxleSpacing { get; set; }
    }
    public partial class AxleWeightListPosition
    {
        [JsonProperty("AxleWeight")]
        public List<Axles> AxleWeight { get; set; }
    }
    public partial class VehicleImportJson
    {
        [JsonProperty("UnitSystem")]
        public string UnitSystem { get; set; }
        [JsonProperty("VehicleConfiguration")]
        public VehicleConfiguration VehicleConfiguration { get; set; }
        [JsonProperty("VehicleComponents")]
        public List<VehicleComponent> VehicleComponents { get; set; }
        [JsonProperty("OrganisationId")]
        public long OrganisationId { get; set; }
        [JsonProperty("AuthenticationKey")]
        public string AuthenticationKey { get; set; }
    }


}