using System.Collections.Generic;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class MovementVehicleConfig
    {
        public long MovementId { get; set; }
        public long VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int VehicleType { get; set; }
        public int VehiclePurpose { get; set; }
        public List<VehicleConfigList> VehicleCompList { get; set; }
        public List<string> VehicleNameList { get; set; }
        public MovementVehicleConfig()
        {
            VehicleCompList = new List<VehicleConfigList>();
            VehicleNameList = new List<string>();
        }
        public bool HighlightVehicle { get; set; }
        public long MainVehicleId { get; set; }
    }
    public class InsertMovementVehicle
    {
        public long MovementId { get; set; }
        public long VehicleId { get; set; }
        public long NotificationId { get; set; }
        public long RevisionId { get; set; }
        public int IsVr1 { get; set; }
        public int Flag { get; set; }
        public string UserSchema { get; set; }
    }
    public class DeleteMovementVehicle
    {
        public long MovementId { get; set; }
        public long VehicleId { get; set; }
        public string UserSchema { get; set; }
    }
    public class MovementVehicleList
    {
        public long RoutePartId { get; set; }
        public int RoutePartNo { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int IsSort { get; set; }
        public string RoutePartName { get; set; }
        public List<VehicleDetails> VehicleList { get; set; }
        public MovementVehicleList()
        {
            VehicleList = new List<VehicleDetails>();
        }
    }
    public class GetRouteVehicleList
    {
        public long RevisionId { get; set; }
        public long VersionId { get; set; }
        public string ContentRefNum { get; set; }
        public string UserSchema { get; set; }
        public int IsHistoric { get; set; }
    }
    public class VehicleDetails
    {
        public long VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int VehicleType { get; set; }
        public int VehiclePurpose { get; set; }
        public long ParentVehicleId { get; set; }
        public List<VehicleConfigList> VehicleCompList { get; set; }
        public List<string> VehicleNameList { get; set; }
        public VehicleDetails()
        {
            VehicleCompList = new List<VehicleConfigList>();
            VehicleNameList = new List<string>();
        }
    }

    public class VehicleAssignment
    {
        public long RoutePartId { get; set; }
        public List<long> VehicleIds { get; set; }
    }
    public class VehicleAssignementParams
    {
        public List<VehicleAssignment> VehicleAssignments { get; set; }
        public long VersionId { get; set; }
        public long RevsionId { get; set; }
        public string ContentRefNum { get; set; }
        public string UserSchema { get; set; }
    }

    public class PerformVehicelAssessment
    {
        public List<MovementVehicleConfig> VehicleList { get; set; }
        public List<AssessedVehicleList> AssessedVehicles { get; set; }
        public VehicleMovementType MovementType { get; set; }
        public int VehicleError { get; set; }
        public PerformVehicelAssessment()
        {
            VehicleList = new List<MovementVehicleConfig>();
            AssessedVehicles = new List<AssessedVehicleList>();
            MovementType = new VehicleMovementType();
        }
    }
    public class AssessedVehicleList
    {
        public long VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int MovementType { get; set; }
        public int VehicleClass { get; set; }
        public int VehiclePurpose { get; set; }
    }

    public class AssessMoveTypeParams
    {
        public int VehicleId { get; set; }
        public int IsRoute { get; set; }
        public string UserSchema { get; set; }
        public bool ForceApplication { get; set; }
        public VehicleMovementType PreviousMovementType { get; set; }
        public ConfigurationModel configuration { get; set; }
    }
    public class AutoFillModel
    {
        public long VehicleId { get; set; }
        public string RecordType { get; set; }
        public double? WheelBase { get; set; }
        public double? OverallLength { get; set; }
        public double? GrossWeight { get; set; }
        public double? RigidLength { get; set; }
        public double? MaxHeight { get; set; }
        public double? OverallWidth { get; set; }
    }
}
