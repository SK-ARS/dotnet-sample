using System.Collections.Generic;

namespace STP.Domain.ExternalAPI
{
    public partial class VehicleImportExternal
    {
        public string UnitSystem { get; set; }
        public VehicleConfiguration VehicleConfiguration { get; set; }
        public List<VehicleComponents> VehicleComponents { get; set; }
        public string AuthenticationKey { get; set; }
    }

    public partial class VehicleExportExternal
    {
        public int VehicleId { get; set; }
        public string UnitSystem { get; set; }
        public VehicleConfiguration VehicleConfiguration { get; set; }
        public List<VehicleComponents> VehicleComponents { get; set; }
    }
    public partial class Vehicle
    {
        public string UnitSystem { get; set; }
        public VehicleConfiguration VehicleConfiguration { get; set; }
        public List<VehicleComponents> VehicleComponents { get; set; }
        public string RouteName { get; set; }
    }
    public partial class VehicleConfiguration
    {
        public string Name { get; set; }
        public string MovementClassification { get; set; }
        public string VehicleType { get; set; }
        public double? GrossWeight { get; set; }
        public double? GrossTrainWeight { get; set; }
        public double? OverallLength { get; set; }
        public List<VehicleRegistration> VehicleRegistration { get; set; }
        public string Notes { get; set; }
        public double? RigidLength { get; set; }
        public double? OverallWidth { get; set; }
        public double? Speed { get; set; }
        public double? MaxHeight { get; set; }
        public double? HeaviestAxleWeight { get; set; }
        public double? WheelBase { get; set; }
    }
    public class VehicleRegistration
    {
        public int SerialNumber { get; set; }
        public string FleetId { get; set; }
        public string Registration { get; set; }
    }
    public partial class VehicleComponents
    {
        public int ComponentNumber { get; set; }
        public string ComponentType { get; set; }
        public string ComponentSubType { get; set; }
        public string Name { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? GrossWeight { get; set; }
        public int? AxleCount { get; set; }
        public double? OutsideTrack { get; set; }
        public double? AxleSpacingToFollowing { get; set; }
        public List<Axle> Axles { get; set; }
        public string CouplingType { get; set; }
        public string RearSteerable { get; set; }
        public string Notes { get; set; }
        public double? ReducibleHeight { get; set; }
        public double? LeftOverhang { get; set; }
        public double? RightOverhang { get; set; }
        public double? FrontOverhang { get; set; }
        public double? RearOverhang { get; set; }
        public double? GroundClearance { get; set; }
        public double? ReducibleGroundClearance { get; set; }
        public List<VehicleRegistration> ComponentRegistrations { get; set; }
    }
    public partial class Axle
    {
        public int AxleNumber { get; set; }
        public int NoOfWheels { get; set; }
        public double? AxleWeight { get; set; }
        public double? DistanceToNextAxle { get; set; }
        public string TyreSize { get; set; }
        public List<double?> WheelSpacing { get; set; }
    }

    public class VehicleImportOutput
    {
        public long MovementId { get; set; }
        public long VehicleId { get; set; }
    }
    public class GetVehicleExportList
    {
        public long RevisionId { get; set; }
        public long VersionId { get; set; }
        public string ContentRefNo { get; set; }
        public int NotificationType { get; set; }
        public long OrganisationId { get; set; }
        public string UserSchema { get; set; }
    }
}
