using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class NotificationGeneralDetails
    {
        public long NotificationID { get; set; }
        public long AnalysisId { get; set; }
        public long VersionId { get; set; }
        public long rev_id { get; set; }

        public string MovementName { get; set; }
        public string ESDALReference { get; set; }
        public int UpdateRouteAnalysis { get; set; }
        // public string ClientName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Classification { get; set; }
        public byte[] NotesFromHaulier { get; set; }
        public string MovemntDateFrom { get; set; }
        public string MovemntDateTo { get; set; }
        public int NoOfMovements { get; set; }
        public int MaxPiecesPerLoad { get; set; }
        public int VehicleCode { get; set; }
        public string ContentRefNo { get; set; }
        public long RoutePartIdS { get; set; }
        public long LibRoutePartID { get; set; }
        public long VehicleId { get; set; }
        public int VehComponantId { get; set; }
        // [Required(ErrorMessage = "Enter total number of movements.")]
        [RegularExpression(@"[0-9]+$", ErrorMessage = "Only numbers are allowed")]
        public int NoOfMoves { get; set; }
        // [Required(ErrorMessage = "Enter max pieces per load.")]
        [RegularExpression(@"[0-9]+$", ErrorMessage = "Only numbers are allowed")]
        public int MaxPieces { get; set; }
        public int OrganisationId { get; set; }
        public int UserId { get; set; }
        public int VehicleCategory { get; set; }
        public int ProjectStatus { get; set; }
        public int NeedsAttention { get; set; }
        public int EnteredBySORT { get; set; }
        public int RequiresVR1 { get; set; }
        public int LatestRevNo { get; set; }
        public int IsNextNotif { get; set; }
        public int IsWithdrawn { get; set; }
        public int IsDeclined { get; set; }
        public int VersionNo { get; set; }
        public int VersionStatus { get; set; }
        public int IsWorkInProgress { get; set; }
        public int IsNotified { get; set; }
        public int IsMostRecent { get; set; }
        public int IsVersionedBySORT { get; set; }
        public int NotificationNo { get; set; }
        public int NotifVersionNo { get; set; }
        public int NotYetAgreed { get; set; }
        public int? RevisionId { get; set; }
        public int UpdateRoute { get; set; }
        public int UpdateVehicle { get; set; }
        public int MaxVersion { get; set; }
        public int MaxVersion1 { get; set; }
        public int FleetId { get; set; }

        // public string PlannedContentRefNo { get; set; }
        public string NotificationCode { get; set; }
        public string MyReference { get; set; }
        public string ClientName { get; set; }
        public string HaulierOprLicence { get; set; }
        //  [Required(ErrorMessage = "Enter from summary.")]
        public string FromSummary { get; set; }
        //  [Required(ErrorMessage = "Enter to summary.")]
        public string ToSummary { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        //  [Required(ErrorMessage = "Enter load description.")]
        public string LoadDescription { get; set; }
        //  [Required(ErrorMessage = "Enter registration number.")]
        public string RegisNo { get; set; }
        public string FleetNos { get; set; }
        public string VehType { get; set; }
        //  [Required(ErrorMessage = "Enter notification notes.")]
        public string Notes { get; set; }
        public string NotesOnEscort { get; set; }
        public string OrderingESDALRefNo { get; set; }
        public string HauliersRef { get; set; }
        public string NotificationDate { get; set; }
        public string VR1Number { get; set; }
        public string SONumbers { get; set; }
        public string VSONumber { get; set; }
        public string VSOType { get; set; }

        public byte[] InboundNotif { get; set; }
        public byte[] OutboundNotif { get; set; }
        public byte[] HauliersNotif { get; set; }

        public DateTime NeedsAttentionDate { get; set; }
        public DateTime NotifCreationDate { get; set; }

        [Required(ErrorMessage = "Enter date time(to).")]
        public string ToDateTime { get; set; }

        [Required(ErrorMessage = "Enter date time(from).")]
        // [MustBeGreaterThanOrEqual("Movement_Date_To", "Start date must be greater than end date")]
        public string FromDateTime { get; set; }



        //[Required(ErrorMessage = "Enter overall length.")]
        // [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public decimal VehLength { get; set; }
        // [Required(ErrorMessage = "Enter overall width.")]
        //  [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public decimal VehWidth { get; set; }
        //  [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public decimal? FrontProjection { get; set; }
        //  [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public decimal? RearProjection { get; set; }

        public decimal? LeftProjection { get; set; }
        public decimal? RightProjection { get; set; }
        // [Required(ErrorMessage = "Enter max height.")]
        // [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public decimal MaxHeight { get; set; }
        //  [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public decimal? ReducibleHeight { get; set; }
        //  [Required(ErrorMessage = "Enter rigid length.")]
        // [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public decimal RigidLength { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public int GrossWeight { get; set; }
        public int? AxelWeight { get; set; }

        public bool PlanRouteYes { get; set; }
        public bool PlanRouteNo { get; set; }
        public bool RetJourny { get; set; }
        public bool CollaborationStatus { get; set; }
        public bool IsAToB { get; set; }

        public string fromdate { get; set; }
        public string todate { get; set; }
        public string notifcrdate { get; set; }
        public string notifdate { get; set; }

        public string OrgUser { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        //for checking validation
        public int CheckRoute { get; set; }
        public int CheckVehicle { get; set; }
        public int CheckVehicleConfig { get; set; }
        public int CheckRouteConfig { get; set; }
        public int CheckVehicleReg { get; set; }
        public int CheckVehicleImp { get; set; }
        public int CheckVehicleWeight { get; set; }
        public int CheckRouteImp { get; set; }
        //Show broken routes
        public int CheckBrokenRoutes { get; set; }

        public int IsSimplified { get; set; }
        public int ShowWarning { get; set; }
        public int PlanRouteOnMapId { get; set; }
        public int PlanRouteOnMapId1 { get; set; }
        public int AddingRouteBy { get; set; }
        public int AxleCounter { get; set; }
        public int IsIndemnity { get; set; }
        public string OnBehalfOf { get; set; }

        //parameters for planning route
        public decimal? StartEasting { get; set; }
        public decimal? StartNorthing { get; set; }
        public string StartDescrip { get; set; }
        public decimal? EndEasting { get; set; }
        public decimal? EndNorthing { get; set; }
        public string EndDescrip { get; set; }

        public string NotifRouteName { get; set; }
        public string NotifRetRouteName { get; set; }
        public long RetRoutePartId { get; set; }
        public int RouteCount { get; set; }

        public int CheckVehicleLen { get; set; }
        public int InValidVhclCount { get; set; }
        public int CheckVhclGrossWgt { get; set; }
        public int InValidWgtVhclCount { get; set; }
        public int CheckVhclWidth { get; set; }
        public int InValidWdhVhclCount { get; set; }
        public int CheckVhclAxleWgt { get; set; }
        public int InValidAxleWgtVhclCount { get; set; }
        public int CheckVhclRigidLen { get; set; }
        public int InValidRigidLenVhclCount { get; set; }
        public int ChkIsReplanCount { get; set; }
        public int ChkIsSpecialManourCount { get; set; }
        ////For Folder Showing DropDownList

        public long FolderID { get; set; }
        public long projectID { get; set; }
        public List<ProjectFolderModel> folderDomainList { get; set; }
        public List<NotifDispensations> NotifDispensationList { get; set; }
        public List<NotifVehicleRegistration> NotifVehicleRegistrList { get; set; }
        public List<NotifVehicleImport> NotifVehicleImportList { get; set; }
        public List<NotifVehicleWeight> NotifVehicleWeightList { get; set; }
        public List<NotifRouteImport> NotifRouteImportList { get; set; }
        //Broken routes
        public List<NotifRouteImport> BrokenRoutes { get; set; }
        public List<NotifVehicleImport> NotifVehicleLenList { get; set; }
        public string[] NotifRouteImpList { get; set; }
        public List<AxelDetails> NotifAxleDetails { get; set; }
        public List<NotifVehicleImport> NotifVehicleGWtList { get; set; }
        public List<NotifVehicleImport> NotifVehicleWidthList { get; set; }
        public List<NotifVehicleImport> NotifVehicleAWtList { get; set; }
        public List<NotifVehicleImport> NotifVehicleRLList { get; set; }
        // for the set of hidden fields
        public decimal MetricVehLength { get; set; }
        public decimal MetricVehWidth { get; set; }
        public decimal? MetricFrontProjection { get; set; }
        public decimal? MetricRearProjection { get; set; }
        public decimal? MetricLeftProjection { get; set; }
        public decimal? MetricRightProjection { get; set; }
        public decimal MetricMaxHeight { get; set; }
        public decimal? MetricReducibleHeight { get; set; }
        public decimal MetricRigidLength { get; set; }

        public NotificationGeneralDetails()
        {
            ProjectFolderModel sofolder = new ProjectFolderModel();
            sofolder.FolderID = 0;
            sofolder.FolderName = "--None--";
            folderDomainList = new List<ProjectFolderModel>();
        }
    }

    public class AxelDetails
    {

        public int NoOfWheels { get; set; }
        public decimal AxelWeight { get; set; }
        public decimal AxelSpacing { get; set; }
        public int compid { get; set; }
        public int AxleNumId { get; set; }
        public string TyreCenters { get; set; }
        public string TyreSize { get; set; }
        public double DistToNextAxle { get; set; }
        public int AxleNo { get; set; }
        public decimal ComponentType { get; set; }
    }

    //public class ListRouteVehicleId
    //{
    //    public long RoutePartId { get; set; }
    //    public long VehicleId { get; set; }
    //    public string PartDescr { get; set; }
    //    public int PointNO { get; set; }
    //    public int RouteCount { get; set; }
    //    public string PartName { get; set; }
    //    public string ComponentIdList { get; set; }
    //    public string ComponentTypeList { get; set; }
    //}

    public class NotifDispensations
    {
        public string DRN { get; set; }
        public string SUMMARY { get; set; }
    }

    public class NotifVehicleRegistration
    {
        public long VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleReg { get; set; }
    }

    public class NotifVehicleImport
    {
        public long VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int VehicleClass { get; set; }
    }

    public class NotifVehicleWeight
    {
        public string VehicleName { get; set; }
    }

    public class NotifRouteImport
    {
        public string RouteName { get; set; }
        public int is_Replan { get; set; }
    }
    public class ProjectFolderModel
    {
        public double OrgId { get; set; }
        public string FolderName { get; set; }
        public long ProjectID { get; set; }
        public long FolderID { get; set; }
    }
}