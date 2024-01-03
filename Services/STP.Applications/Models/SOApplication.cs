using STP.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STP.Applications.Models
{
    public class SOApplication
    {
        public string ESDAL_Reference { get; set; }
        public string ESDAL_Reference_SORT { get; set; }
        public string HA_Reference { get; set; }
        public string Haulier_Reference { get; set; }
        [Required(ErrorMessage = "From summary is required")]
        public string From_Address { get; set; }
        [Required(ErrorMessage = "To summary is required")]
        public string To_Address { get; set; }
        public string Haulier { get; set; }
        public string Haulier_Contact_Address1 { get; set; }
        public string Haulier_Contact_Address2 { get; set; }
        public string Haulier_Contact_Address3 { get; set; }
        public string Haulier_Contact_Address4 { get; set; }
        public string Haulier_Contact_Address5 { get; set; }
        public string Haulier_FaxNo { get; set; }
        public string Haulier_EmailId { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Operator_License { get; set; }
        public string App_Notes_From_HA { get; set; }
        public byte[] App_Notes_To_HA { get; set; }
        public string Desc { get; set; }
        [Required(ErrorMessage = "Load description summary is required")]
        public string Load { get; set; }
        public string HA_Job_Reference { get; set; }
        [MustBeGreaterThanOrEqual("Movement_Date_To", "Start date must be greater than end date")]
        public string Movement_Date_From { get; set; }
        public string Movement_Date_To { get; set; }
        public string ApplicationDueDate { get; set; }
        public string ApplicationDate { get; set; }
        public string Mov_Agreed_date { get; set; }
        public string VR1_Approval_date { get; set; }
        public int? NumberofMovement { get; set; }
        public int? NumberofPieces { get; set; }
        public string HAContactName { get; set; }
        public decimal HAContact_id { get; set; }
        public int DestributionNote { get; set; }
        public long AnalysisID { get; set; }
        public int ApplicationStatus { get; set; }
        public string Mov_Status { get; set; }
        public int VehicleClassification { get; set; }
        public string AgentName { get; set; }
        public int VersionStatus { get; set; }
        public long VersionID { get; set; }
        public string TabStatus { get; set; }
        public string NotesOnEscort { get; set; }
        public int IsNotified { get; set; }
        public string Haul_Description { get; set; }
        public long ProjectID { get; set; }
        public long ApplicationrevId { get; set; }
        public string AllocateTo { get; set; }
        public string AllocateToName { get; set; }
        public decimal OrgID { get; set; }
        public int EditBySORT { get; set; }
        public string OrgEmailId { get; set; }
        public string OrgFax { get; set; }
        public string OrgHaulierContactName { get; set; }
        public string OrgName { get; set; }
        public string Proj_Status { get; set; }
        public string Checking_Status { get; set; }
        public string haulier_name { get; set; }
        public string Haulier_Notes { get; set; }
        public int SaveAllocateFlag { get; set; }
        public string Mov_Name { get; set; }
        public int CheckingStatus_Code { get; set; }
        public long CheckerUserId { get; set; }
        public long PlannerUserId { get; set; }
        public int AppStatus_Code { get; set; }
        public string LastSpecialOrderNo { get; set; }
        public string CheckerName { get; set; }
        public long LastCandidateRouteId { get; set; }
        public long LastRevisionId { get; set; }
        public int LastRevisionNo { get; set; }
        public string Vr1Number { get; set; }
        public long RouteAnalysisId { get; set; }
        public decimal MovIsDistributed { get; set; }
        public string SONumber { get; set; }
        public int VersionNo { get; set; }
        public decimal Enteredbysort { get; set; }
        public string MovementFromDesc { get; set; }
        public string MovementToDesc { get; set; }
        public string MovementLoadDesc { get; set; }
        public decimal PreVerDistributed { get; set; }
        public long DistributedMovAnalysisId { get; set; }
        public int Reference_no { get; set; }
        public string Hauli_Mneu { get; set; }
        public long FolderID { get; set; }
        public List<ProjectFolderModel> folderDomainList { get; set; }
        public SOApplication()
        {
            ProjectFolderModel sofolder = new ProjectFolderModel();
            sofolder.FolderID = 0;
            sofolder.FolderName = "--None--";
            folderDomainList = new List<ProjectFolderModel>();
        }
    }
    public class ApplicationWithdraw
    {
        public bool result { get; set; }
        public string projectStatus { get; set; }
        public string checkingStatus { get; set; }
    }

    public class SOHaulierApplication
    {
        public long RevisionID { get; set; }
        public string Haul_ESDALReference { get; set; }
        public string Haul_Reference { get; set; }
        public string Haul_ClientName { get; set; }
        public string Haul_FromSummary { get; set; }
        public string Haul_ToSummary { get; set; }
        public string Haul_Description { get; set; }
        public string Haul_Load { get; set; }
        public DateTime Haul_ApplicationDate { get; set; }
        public DateTime Haul_MovementDateFrom { get; set; }
        public DateTime Haul_MovementDateTo { get; set; }
        public int Haul_NumberOfMovements { get; set; }
        public int Haul_NumberOfPieces { get; set; }
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
        public string Haul_ApplicationNotes { get; set; }
        public string HaulierContactName { get; set; }
        public string AgentName { get; set; }
        public string NotesOnEscort { get; set; }
        public string VehicleDescription { get; set; }
        public int SubMovementClass { get; set; }
    }
    public class SubmitSoParams
    {
        public int ApprevisionId { get; set; }
        public int UserId { get; set; }
    }
}