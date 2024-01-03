using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class ApplyForVR1
    {
        public int userId { get; set; }
        public int organisationId { get; set; }
        public long apprevid { get; set; }
        public long ApplicationrevId { get; set; }
        public long AnalysisId { get; set; }
        public int SubMovementClass { get; set; }
        public string MyReference { get; set; }
        public string ClientName { get; set; }
        public string DescriptionWithAppl { get; set; }
        public int VR1ApplStatus { get; set; }
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
        public decimal HAContact_id { get; set; }
        public string ContactName { get; set; }
        public string HaulierOrgName { get; set; }
        public string HaulierAddress1 { get; set; }
        public string HaulierAddress2 { get; set; }
        public string HaulierAddress3 { get; set; }
        public string HaulierAddress4 { get; set; }
        public string HaulierAddress5 { get; set; }
        public string HaulOperatorLicens { get; set; }
        public string HaulEmailID { get; set; }
        public string HaulFaxNo { get; set; }
        public string HaulTelephoneNo { get; set; }
        public string NotesWithAppl { get; set; }
        public string LoadDescription { get; set; }
        public Int16? ReducedDetails { get; set; }

        public int IsDistributed { get; set; }
        public int IsNotified { get; set; }
        public string VR1_Number { get; set; }

        public int CheckRoute { get; set; }
        public int CheckVehicle { get; set; }
        public int CheckVehicleConfig { get; set; }
        public int CheckSupplInfo { get; set; }
        public int CheckRouteConfig { get; set; }
        public List<VR1RouteImport> VR1RouteImportList { get; set; }
        public List<SORouteImport> SORouteImportList { get; set; }
        public int CheckVehicleAxleAndGrossWgt { get; set; }

        public string ApplicantContactName { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress1 { get; set; }
        public string ApplicantAddress2 { get; set; }
        public string ApplicantAddress3 { get; set; }
        public string ApplicantAddress4 { get; set; }
        public string ApplicantAddress5 { get; set; }
        public string ApplicantOperatorLicens { get; set; }
        public string ApplicantEmailID { get; set; }
        public string ApplicantFaxNo { get; set; }
        public string ApplicantTelephoneNo { get; set; }
        public string ApplicantCountryName { get; set; }
        public string ApplicantCountryID { get; set; }
        public string AppPostCode { get; set; }
        public string VR1ContentRef { get; set; }

        public string AllocateTo { get; set; }
        public string AllocateToName { get; set; }
        public int SaveAllocateFlag { get; set; }
        public int VersionId { get; set; }
        public int LatestVersion { get; set; }

        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string Country { get; set; }

        public int EditBySORT { get; set; }
        public string CountryID { get; set; }
        public string HaulPostCode { get; set; }

        public int VR1ProjStatus { get; set; }
        public string Haul_ContactName { get; set; }
        public string Haul_ApplicantName { get; set; }
        public string Haul_ApplicantAddress1 { get; set; }
        public string Haul_ApplicantAddress2 { get; set; }
        public string Haul_ApplicantAddress3 { get; set; }
        public string Haul_ApplicantAddress4 { get; set; }
        public string Haul_ApplicantAddress5 { get; set; }
        public string Haul_PostCode { get; set; }
        public string Haul_Country { get; set; }
        public string Haul_Telephone { get; set; }
        public string Haul_FaxNumber { get; set; }
        public string Haul_EmailID { get; set; }
        //
        ////For Folder Showing DropDownList
        public long FolderID { get; set; }
        public long ProjectID { get; set; }
        public string vappDate { get; set; }

        public List<ProjectFolderModel> folderDomainList { get; set; }
        public ApplyForVR1()
        {
            ProjectFolderModel sofolder = new ProjectFolderModel();
            sofolder.FolderID = 0;
            sofolder.FolderName = "--None--";
            folderDomainList = new List<ProjectFolderModel>();
        }

        public int STATUS { get; set; }

        public List<ApplVehiclComponents> ApplVehiclComponentsList { get; set; }
        public int CheckVehComp { get; set; }

        public List<ApplVehiclComponents> ApplLenVehicleList { get; set; }
        public int CheckVehicleLen { get; set; }
        public int CheckRouteImp { get; set; }

        public List<ApplVehiclComponents> ApplWgtVehicleList { get; set; }

        public IEnumerable<object> objBrocken { get; set; }
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
        public int ApprevisionId { get; set; }
        public int ReducedDet { get; set; }
    }
}