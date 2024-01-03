using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class ApplyForVR1
    {
        public int UserId { get; set; }
        public int OrganisationId { get; set; }
        public long AppRevId { get; set; }
        public long ApplicationRevisionId { get; set; }
        public long AnalysisId { get; set; }
        public int SubMovementClass { get; set; }
        public string MyReference { get; set; }
        public string ClientName { get; set; }
        public string DescriptionWithApplication { get; set; }
        public int VR1ApplicationStatus { get; set; }
        public string FromSummary { get; set; }
        public string ToSummary { get; set; }
        public string MovementDateFrom { get; set; }
        public string MovementDateTo { get; set; }
        public string ApplicationDate { get; set; }
        public int? NoOfMovements { get; set; }
        public int? MaxPiecesPerLoad { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double? OverallWidth { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double? OverallLength { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double? OverallHeight { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double? GrossWeight { get; set; }
        public string ESDALReference { get; set; }
        public string VehicleDescription { get; set; }
        public string OnBehalfOf { get; set; }
        public decimal HAContactId { get; set; }
        public string ContactName { get; set; }
        public string HaulierOrgName { get; set; }
        public string HaulierAddress1 { get; set; }
        public string HaulierAddress2 { get; set; }
        public string HaulierAddress3 { get; set; }
        public string HaulierAddress4 { get; set; }
        public string HaulierAddress5 { get; set; }
        public string HaulierOperatorLicence { get; set; }
        public string OnBehalfOfEmailId { get; set; }
        public string HaulierFaxNo { get; set; }
        public string HaulierTelephoneNo { get; set; }
        public string ApplicationNotes { get; set; }
        public string LoadDescription { get; set; }
        public Int16? ReducedDetails { get; set; }
        public int IsDistributed { get; set; }
        public int IsNotified { get; set; }
        public string VR1Number { get; set; }
        public int CheckRoute { get; set; }
        public int CheckVehicle { get; set; }
        public int CheckVehicleConfig { get; set; }
        public int CheckSuppliInfo { get; set; }
        public int CheckRouteConfig { get; set; }
        public List<VR1RouteImport> VR1RouteImportList { get; set; }
        public List<SORouteImport> SORouteImportList { get; set; }
        public int CheckVehicleAxleAndGrossWeight { get; set; }
        public string ApplicantContactName { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress1 { get; set; }
        public string ApplicantAddress2 { get; set; }
        public string ApplicantAddress3 { get; set; }
        public string ApplicantAddress4 { get; set; }
        public string ApplicantAddress5 { get; set; }
        public string ApplicantOperatorLicence { get; set; }
        public string ApplicantEmailId { get; set; }
        public string ApplicantFaxNo { get; set; }
        public string ApplicantTelephoneNo { get; set; }
        public string ApplicantCountryName { get; set; }
        public string ApplicantCountryId { get; set; }
        public string ApplicationPostCode { get; set; }
        public string VR1ContentRefNo { get; set; }
        public string AllocateTo { get; set; }
        public string AllocateToName { get; set; }
        public int SaveAllocateFlag { get; set; }
        public int VersionId { get; set; }
        public int LatestVersion { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string Country { get; set; }
        public int EditBySORT { get; set; }
        public string CountryId { get; set; }
        public string OnBehalfOfPostCode { get; set; }
        public int VR1ProjectStatus { get; set; }
        public string HaulierContactName { get; set; }
        public string HaulierApplicantName { get; set; }
        public string HaulierApplicantAddress1 { get; set; }
        public string HaulierApplicantAddress2 { get; set; }
        public string HaulierApplicantAddress3 { get; set; }
        public string HaulierApplicantAddress4 { get; set; }
        public string HaulierApplicantAddress5 { get; set; }
        public string HaulierPostCode { get; set; }
        public string HaulierCountry { get; set; }
        public string HaulierTelephone { get; set; }
        public string HaulierFaxNumber { get; set; }
        public string HaulierEmailId { get; set; }        
        ////For Folder Showing DropDownList
        public long FolderId { get; set; }
        public long ProjectId { get; set; }
        public string VApprDate { get; set; }
        public string HaulierMnemonic { get; set; }
        public int EsdalRefNo { get; set; }
        public List<ProjectFolderModel> ProjectFolderList { get; set; }
        public ApplyForVR1()
        {
            ProjectFolderModel projFolder = new ProjectFolderModel();
            projFolder.FolderId = 0;
            projFolder.FolderName = "--None--";
            ProjectFolderList = new List<ProjectFolderModel>();
        }
        public int Status { get; set; }
        public List<ApplVehiclComponents> ApplicationVehicleComponentList { get; set; }
        public int CheckVehComp { get; set; }
        public int CheckVehicleLength { get; set; }
        public int CheckRouteImp { get; set; }
        public IEnumerable<object> ObjBroken { get; set; }
        public long MovementId { get; set; }
        public int VersionNumber { get; set; }
        public int RevisionNumber { get; set; }
    }
    public class VR1RouteImport
    {
        public string RouteName { get; set; }
    }
    public class SORouteImport
    {
        public string RouteName { get; set; }
    }
    public class ApplVehiclComponents
    {
        public long VehicleId { get; set; }
        public string VehicleName { get; set; }
    }
    public class SubmitVR1Params
    {
        public int ApplicationRevisionId { get; set; }
        public int ReducedDet { get; set; }
    }
}