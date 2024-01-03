using STP.Common.Validation;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STP.Domain.Applications
{
    public class SOApplication
    {
        public string ESDALReference { get; set; }
        public string ESDALReferenceSORT { get; set; }
        public string HAReference { get; set; }
        public string HaulierReference { get; set; }
        [Required(ErrorMessage = "From summary is required")]
        public string FromAddress { get; set; }
        [Required(ErrorMessage = "To summary is required")]
        public string ToAddress { get; set; }
        public string Haulier { get; set; }
        public string HaulierContactAddress1 { get; set; }
        public string HaulierContactAddress2 { get; set; }
        public string HaulierContactAddress3 { get; set; }
        public string HaulierContactAddress4 { get; set; }
        public string HaulierContactAddress5 { get; set; }
        public string HaulierFaxNo { get; set; }
        public string HaulierEmailId { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string OperatorLicence { get; set; }
        public string ApplicationNotesFromHA { get; set; }
        public byte[] ApplicationNotesToHA { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Load description summary is required")]
        public string Load { get; set; }
        public string HAJobFileReference { get; set; }
        [MustBeGreaterThanOrEqual("MovementDateTo", "Start date must be greater than end date")]
        public string MovementDateFrom { get; set; }
        public string MovementDateTo { get; set; }
        public string ApplicationDueDate { get; set; }
        public string ApplicationDate { get; set; }
        public string MovementAgreedDate { get; set; }
        public string VR1ApprovalDate { get; set; }
        public int? NumberOfMovements { get; set; }
        public int? NumberofPieces { get; set; }
        public string HAContactName { get; set; }
        public decimal HAContactId { get; set; }
        public int DestributionNote { get; set; }
        public long AnalysisId { get; set; }
        public int ApplicationStatus { get; set; }
        public string MovementStatus { get; set; }
        public int VehicleClassification { get; set; }
        public string AgentName { get; set; }
        public int VersionStatus { get; set; }
        public long VersionId { get; set; }
        public string TabStatus { get; set; }
        public string NotesOnEscort { get; set; }
        public int IsNotified { get; set; }
        public string HaulierDescription { get; set; }
        public long ProjectId { get; set; }
        public long ApplicationRevId { get; set; }
        public string AllocateTo { get; set; }
        public string AllocateToName { get; set; }
        public decimal OrgId { get; set; }
        public int EditBySORT { get; set; }
        public string OrgEmailId { get; set; }
        public string OrgFax { get; set; }
        public string OrgHaulierContactName { get; set; }
        public string OrgName { get; set; }
        public string ProjectStatus { get; set; }
        public string CheckingStatus { get; set; }
        public string HaulierName { get; set; }
        public string HaulierNotes { get; set; }
        public int SaveAllocateFlag { get; set; }
        public string MovementName { get; set; }
        public int CheckingStatusCode { get; set; }
        public long CheckerUserId { get; set; }
        public long PlannerUserId { get; set; }
        public int ApplicationStatusCode { get; set; }
        public string LastSpecialOrderNo { get; set; }
        public string CheckerName { get; set; }
        public long LastCandidateRouteId { get; set; }
        public long LastRevisionId { get; set; }
        public int LastRevisionNo { get; set; }
        public string VR1Number { get; set; }
        public long RouteAnalysisId { get; set; }
        public decimal IsMovDistributed { get; set; }
        public string SONumber { get; set; }
        public int VersionNo { get; set; }
        public decimal EnteredBySORT { get; set; }
        public string MovementFromDescription { get; set; }
        public string MovementToDescription { get; set; }
        public string MovementLoadDescription { get; set; }
        public decimal PreviousVersionDistributed { get; set; }
        public long DistributedMovAnalysisId { get; set; }
        public int ReferenceNo { get; set; }
        public int EsdalRefNo { get; set; }
        public string HaulierMneu { get; set; }
        public long FolderId { get; set; }
        public List<ProjectFolderModel> ProjectFolderList { get; set; }
        public long MovementId { get; set; }
        public List<SpecialOrder> SpecialOrders { get; set; }
        public decimal LatestVersionNumber { get; set; }
        public SOApplication()
        {
            ProjectFolderModel projFolder = new ProjectFolderModel();
            projFolder.FolderId = 0;
            projFolder.FolderName = "--None--";
            ProjectFolderList = new List<ProjectFolderModel>();
        }

        public string OnBehalOfContactName { get; set; }
        public string OnBehalOfHaulierOrgName { get; set; }
        public string OnBehalOfHaulierAddress1 { get; set; }
        public string OnBehalOfHaulierAddress2 { get; set; }
        public string OnBehalOfHaulierAddress3 { get; set; }
        public string OnBehalOfHaulierAddress4 { get; set; }
        public string OnBehalOfHaulierAddress5 { get; set; }
        public int OnBehalOfCountryID { get; set; }
        public string OnBehalOfHaulPostCode { get; set; }
        public string OnBehalOfHaulOperatorLicens { get; set; }
        public string OnBehalOfHaulEmailID { get; set; }
        public string OnBehalOfHaulFaxNo { get; set; }
        public string OnBehalOfHaulTelephoneNo { get; set; }
        public List<UserRegistration> CountryList { get; set; }
    }
    public class ApplicationWithdraw
    {
        public bool Result { get; set; }
        public string ProjectStatus { get; set; }
        public string CheckingStatus { get; set; }
    }

    public class SOHaulierApplication
    {
        public long RevisionId { get; set; }
        public string HaulierESDALReference { get; set; }
        public string HaulierReference { get; set; }
        public string HaulierClientName { get; set; }
        public string HaulierFromSummary { get; set; }
        public string HaulierToSummary { get; set; }
        public string HaulierDescription { get; set; }
        public string HaulierLoad { get; set; }
        public DateTime HaulierApplicationDate { get; set; }
        public DateTime HaulierMovementDateFrom { get; set; }
        public DateTime HaulierMovementDateTo { get; set; }
        public int HaulierNumberOfMovements { get; set; }
        public int HaulierNumberOfPieces { get; set; }
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
        public string HaulierApplicationNotes { get; set; }
        public string HaulierOperatorLicence { get; set; }
        public string AgentName { get; set; }
        public string NotesOnEscort { get; set; }
        public string VehicleDescription { get; set; }
        public int SubMovementClass { get; set; }
        public string Status { get; set; }

        public string OnBehalOfContactName { get; set; }
        public string OnBehalOfHaulierOrgName { get; set; }
        public string OnBehalOfHaulierAddress1 { get; set; }
        public string OnBehalOfHaulierAddress2 { get; set; }
        public string OnBehalOfHaulierAddress3 { get; set; }
        public string OnBehalOfHaulierAddress4 { get; set; }
        public string OnBehalOfHaulierAddress5 { get; set; }
        public int OnBehalOfCountryID { get; set; }
        public string OnBehalOfCountryName { get; set; }
        public string OnBehalOfHaulPostCode { get; set; }
        public string OnBehalOfHaulOperatorLicens { get; set; }
        public string OnBehalOfHaulEmailID { get; set; }
        public string OnBehalOfHaulFaxNo { get; set; }
        public string OnBehalOfHaulTelephoneNo { get; set; }
    }
    public class SubmitSoParams
    {
        public int ApplicationRevisionId { get; set; }
        public int UserId { get; set; }
    }
    public class VehicleConfigParams {
        public string ESDALRef { get; set; }
        public int VR1Vehicle { get; set; }
    }

    public class VehicleDetail
    {
        public long Veh_Id { get; set; }
        public long Rpart_Id { get; set; }
        public string Veh_Name { get; set; }
        public string Vehicle_Name { get; set; }
        public string Registration { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Reducible_Height { get; set; }
        public double Rigid_Length { get; set; }
        public double Gross_Weight { get; set; }
        public double Max_Axle_Weight { get; set; }
        public double Rear_Overhang { get; set; }
        public double Wheelbase { get; set; }
        public double Ground_Clearence { get; set; }
        public double Outside_Track { get; set; }
    }
    public class AppGeneralDetails
    {
        public long RevisionId { get; set; }
        public long VersionId { get; set; }
        public bool IsVr1 { get; set; }
    }

    public class SORouteAssessment
    {
        public string RouteAnalysis { get; set; }
    }
}