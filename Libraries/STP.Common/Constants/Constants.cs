namespace STP.Common.Constants
{
    public static class Constants
    {
        public const string Haulier = "Haulier Portal";
        public const string Police = "Police Portal";
        public const string OPS = "OPS Portal";
        public const string MIS = "MIS Portal";
        public const string PUBLIC = "Public Portal";
        public const string Admin = "Admin";
        public const string SOA = "SOA Portal";
        public const string SORT = "SORT Portal";
        public const string HelpDesk = "Helpdesk Portal";
    }
    public static class StatusMessage
    {
        public const string Ok = "Success";
        public const string Created = "Created";
        public const string BadRequest = "Bad request";
        public const string NotFound = "No records found";
        public const string InternalServerError = "The request cannot be completed due to an error in the server";
        public const string DeletionFailed = "Deletion failed";
        public const string InsertionFailed = "Insertion failed";
        public const string UpdationFailed = "Updation failed";
        public const string NotificationNotFound = "No notifications found";
        public const string Unauthorized = "Authentication failure";
    }
    public static class ExternalApiStatusMessage
    {
        public const string SubmitOk = "Application submitted successfully";
        public const string Unauthorized = "Authentication failure";
        public const string ValidationFailure = "Validation failure";
        public const string BadRequest = "Malformed request";
        public const string InternalServerError = "The request cannot be completed due to an error in the server";
        public const string NotFound = "No data found";
        public const string VehicleDelete = "Vehicle deleted successfully";
        public const string NotValidEsdalReference = "The provided esdal reference number is not valid";
    }
    public static class UserSchema
    {
        public const string Portal = "portal";
        public const string Sort = "stp_sort";
        public const string Spatial = "spatial";
    }
    public static class UserType
    {
        public const int Haulier = 696001;
        public const int PoliceALO = 696002;
        public const int OPSPORTAL = 696003;
        public const int MISPORTAL = 696004;
        public const int PUBLICPORTAL = 696005;
        public const int Admin = 696006;
        public const int SOA = 696007;
        public const int Sort = 696008;
        public const int AdminSU = 696009;
    }
    public static class ComponentFields
    {
        public const string InternalName = "Internal_Name";
        public const string MakeConfig = "MakeConfig";
    }

    public static class VehiclePurposeType
    {
        public const int ConstructionAndUse = 270001;
        public const int StgoAil = 270002;
        public const int StgoMobileCrane = 270003;
        public const int StgoEngineeringPlant = 270004;
        public const int StgoRoadRecovery = 270005;
        public const int SpecialOrder = 270006;
        public const int VehicleSpecialOrder = 270007;
        public const int Tracked = 270008;
    }

    public static class WorkflowActivityConstants
    {
        public const string Wf_Subject_FleetManagement = "FLEET MANAGEMENT";
        public const string Wf_Subject_ApplicationNotification = "APPLICATION AND NOTIFICATION";
        public const string Wf_Subject_SortSoApplicationProcess = "SORT SO APPLICATION PROCESS";
        public const string Wf_Subject_SortVR1ApplicationProcess = "SORT VR1 APPLICATION PROCESS";
        public const string Wf_Subject_SOAPoliceNotificationProcess = "SOA POLICE NOTIFICATION PROCESS";
        public const string Fm_TotalComponentsCount = "totalComponents";
        public const string Gn_Failed = "Failed";
    }

    public static class RouteExportError
    {
        public const string ExportError = "The route cannot be exported as it doesn't support GPX format due to any of the following reasons - route is broken after a map upgrade activity, route contains alternate paths, route contains special maneuvers.";
    }

    public static class IcaSuitability
    {
        public const string IcaDisabled = "ica options are disabled for structure";
        public const string Suitable = "suitable";
        public const string Unsuitable = "unsuitable";
        public const string MariginalSuitable = "marginally suitable";
        public const string MariginalSuit = "marginal";
        public const string NotApplicable = "assessment not applicable";
        public const string Sidebyside = "cannot be performed for side-by-side configuration";
        public const string MoreComponent = "cannot be performed for 3 or more components";
        public const string Minaxlespace = "minimum axle spacing not found";
        public const string Svtrain = "vehicle not suitable for sv train";
        public const string Stgocat1 = "vehicle does not belong to stgo category 1";
        public const string Axleweight = "axle weight capacity of structure is not available";
        public const string Axleweigthscreening = "cannot perform axle weight screening";
        public const string Structurewidth = "structure width or length is not available";
        public const string Grossweight = "gross weight capacity of structure is not available";
        public const string Grossweightscreen = "cannot perform gross weight screening";
        public const string NotSvVehicle = "not sv vehicle";
        public const string NotSpecified = "not specified";
        public const string NotStructureSpecified = "not structure specified";
    }
}
