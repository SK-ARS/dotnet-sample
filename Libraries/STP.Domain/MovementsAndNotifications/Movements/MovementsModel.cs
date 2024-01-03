using STP.Common.Validation;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class MovementsList
    {
        public int TotalRowCount { get; set; }
        public long MovementVersionId { get; set; }
        public long MovementRevisionId { get; set; }
        public int ProjectESDALReference { get; set; } // Esdal Reference from Project Table.
        public string HaulierMnemonic { get; set; }
        public int ApplicationRevisionNo { get; set; }
        public string MinNotificationDate { get; set; }
        public string MaxNotificationDate { get; set; }
        public int MovementVersionNumber { get; set; }
        public string MovementType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public int VersionStatus { get; set; }
        public string ESDALReference { get; set; }
        public string MyReference { get; set; }
        public int Attention { get; set; }
        public int ColloborationStatus { get; set; }
        public string FromDescription { get; set; }
        public string ToDescription { get; set; }
        public string NotificationFromDesc { get; set; }
        public string NotificationToDesc { get; set; }
        public string ClientDescription { get; set; }
        public long ApplicationRevisionId { get; set; }
        public long ProjectId { get; set; }

        public long NotificationId { get; set; }
        public long NotificationStatus { get; set; }
        public string NotificationCode { get; set; }
        public string ContentRefNum { get; set; }
        public string NotificationMyReference { get; set; }
        public int IsNotified { get; set; }
        public string NotificationFromDate { get; set; }
        public string NotificationToDate { get; set; }

        public int IsHistoric { get; set; }
        public int IsWithdrawn { get; set; }
        public int IsDeclined { get; set; }
        public int SearchFlag { get; set; }
        public bool NeedAttentionFilterFlag { get; set; }
        public int EnteredBySort { get; set; }
        public int MaximumVersion { get; set; }
    }

    //model for filtereing of Haulier List Movement
    public class MovementsFilter
    {
        //For Application States
        public bool WorkInProgress { get; set; }
        public bool WorkInProgressApplication { get; set; } // for Application
        public bool WorkInProgressNotification { get; set; }//for Notification
        public bool Submitted { get; set; }
        public bool ReceivedByHA { get; set; }
        public bool WithdrawnApplications { get; set; }
        public bool DeclinedApplications { get; set; }
        public bool Agreed { get; set; }
        public bool ProposedRoute { get; set; }
        public bool Notifications { get; set; }
        public bool ApprovedVR1 { get; set; }

        //For Other Options
        public bool NeedsAttention { get; set; }
        public bool NewCollabration { get; set; }
        public bool ReadCollaboration { get; set; }

        public bool MostRecentVersion { get; set; }
        public bool IncludeHistoric { get; set; }
        public long FolderId { get; set; }

        //For Movement Type
        public bool SO { get; set; }
        public bool VSO { get; set; }
        public bool STGO { get; set; }
        public bool CandU { get; set; }
        public bool Tracked { get; set; }
        public bool STGOVR1 { get; set; }
        public long BtnClearSORTOrder { get; set; }
        public int? NotifyVSO { get; set; }
        public int SortOrderValue { get; set; }
        public int SortTypeValue { get; set; }
        public bool WorkInProgressAppNotif { get; set; }


    }
    //For Advanced Filtering
    public class MovementsAdvancedFilter
    {
        public string ESDALReference { get; set; }
        public string MyReference { get; set; }
        public string StartOrEnd { get; set; }
        public string FleetId { get; set; }
        public string Keyword { get; set; }
        public string Client { get; set; }
        public int? ReceiptOrganisation { get; set; }
        public string VehicleRegistration { get; set; }
        public string AdvancedDimensionFilterString {
            get
            {
                return "";
            }
            set
            {
                if (value != null && value != "")
                {
                    this.AdvancedDimensionFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AdvancedDimensionFilter>>(value);
                    if (AdvancedDimensionFilter != null && AdvancedDimensionFilter.Any())
                    {
                        var queryString1 = "";
                        var queryString2 = "";
                        var filterIndex = 0;
                        var totalLength = AdvancedDimensionFilter.Count;
                        foreach (var item in AdvancedDimensionFilter)
                        {
                            filterIndex++;
                            var operatorVal = item.LogicalOperator != "" && filterIndex < totalLength ? item.LogicalOperator : "";
                            if (!string.IsNullOrWhiteSpace(item.SearchValue))
                            {
                                var strVehiclebtw = (item.ComparisonOperator == "between") ? item.DimensionType + " " + item.ComparisonOperator + " " + item.SearchValue + " and " + item.SearchValueBetween + " " + operatorVal :
                                       item.DimensionType + " " + item.ComparisonOperator + " " + item.SearchValue + " " + operatorVal;
                                queryString1 += " " + item.AppTableAlias + strVehiclebtw;
                                queryString2 += " " + item.NotiTableAlias + strVehiclebtw;
                            }
                        }
                        queryString1 = (queryString1 != "") ? "(" + queryString1 + ")" : "";
                        queryString2 = (queryString2 != "") ? "(" + queryString2 + ")" : "";
                        QueryString1 = queryString1;
                        QueryString2 = queryString2;
                    }
                }
            }
        }
        public List<AdvancedDimensionFilter> AdvancedDimensionFilter { get; set; }
        [DisplayName("gross_weight_max_kg")]
        public double? GrossWeight { get; set; }
        [DisplayName("gross_weight_max_kg1")]
        public double? GrossWeight1 { get; set; }
        public double? OverallWidth { get; set; }
        public double? OverallWidth1 { get; set; }
        public double? Length { get; set; }
        public double? Length1 { get; set; }
        public double? Height { get; set; }
        public double? Height1 { get; set; }
        public bool IncludeHistoricalData { get; set; }
        public double? AxleWeight { get; set; }
        public double? AxleWeight1 { get; set; }
        [MustBeGreaterThanOrEqual("MovementToDate", "Start date must be greater than end date")]
        public string MovementFromDate { get; set; }

        public DateTime? MovementFrom { get; set; }
        public string MovementToDate { get; set; }
        public DateTime? MovementTo { get; set; }
        [MustBeGreaterThanOrEqual("ApplicationToDate", "Start date must be greater than end date")]
        public string ApplicationFromDate { get; set; }
        public DateTime? ApplicationFrom { get; set; }
        public string ApplicationToDate { get; set; }
        public DateTime? ApplicationTo { get; set; }
        [MustBeGreaterThanOrEqual("NotificationToDate", "Start date must be greater than end date")]
        public string NotificationFromDate { get; set; }
        public DateTime? NotificationFrom { get; set; }
        public string NotificationToDate { get; set; }
        public DateTime? NotificationTo { get; set; }
        public int WeightCount { get; set; }
        public int WidthCount { get; set; }
        public int LengthCount { get; set; }
        public int HeightCount { get; set; }
        public int AxleCount { get; set; }

        //dates checkbox
        public bool MovementDate { get; set; }
        public bool ApplicationDate { get; set; }
        public bool NotifyDate { get; set; }

        //For Sorting Order
        public int SORTOrder { get; set; }

        //variables added for ESDAL3 SORT CR
        public string HaulierName { get; set; }
        public string SONum { get; set; }
        /// <summary>
        /// VR 1 number checkbox
        /// </summary>
        public string VRNum { get; set; }
        /// <summary>
        /// Application type checkbox
        /// </summary>
        public int ApplicationType { get; set; }
        public string LoadDetails { get; set; }

        [Display(Name = "Start point")]
        /// <summary>
        /// Start/End point
        /// </summary>
        public string StartPoint { get; set; }

        /// <summary>
        /// End point
        /// </summary>
        [Display(Name = "End point")]
        public string EndPoint { get; set; }
        public string VehicleClass { get; set; }
        public string QueryString1 { get; set; }
        public string QueryString2 { get; set; }
        public int LogicOpr { get; set; }
        public int Operator { get; set; }
    }
    public class AdvancedDimensionFilter
    {
        public AdvancedDimensionFilter()
        {
            AppTableAlias = "AP.";
            NotiTableAlias = "NF.";
        }
        public string DimensionType { get; set; }
        public string ComparisonOperator { get; set; }
        public string SearchValue { get; set; }
        public string SearchValueBetween { get; set; }
        public string LogicalOperator { get; set; }
        public string AppTableAlias { get; set; }
        public string NotiTableAlias { get; set; }
    }

    public class MovementsInbox
    {
        public long InboxItemId { get; set; } // Changed by NetWeb
        public string Status { get; set; }
        public string Type { get; set; }
        public string ReceivedDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ESDALReference { get; set; }
        public string MessageType { get; set; }
        public int StructureId { get; set; }
        public string StructName { get; set; }
        public bool StatuatoryPeriod { get; set; }
        public bool RouteNotAgreed { get; set; }
        public long NotificationId { get; set; }
        public int TotalRecord { get; set; }
        public double? VehicleMaxWidth { get; set; }
        public string ICAStatus { get; set; }
        public int IsOpened { get; set; }
        public int IsUnopened { get; set; }
        public int IsWithdrawn { get; set; }
        public int IsDeclined { get; set; }
        public string UserId { get; set; }
        public int ImminentMovement { get; set; } // added for NEN
        public long NENId { get; set; } // added for NEN
        public long NENRouteStatus { get; set; }// added for NEN
        public int InboxItemStatus { get; set; }// added for NEN
        public string UserAssignId { get; set; }// added for NEN
        public int VehicleClassification { get; set; }
        public int ProjectStatus { get; set; }
        public string Owner { get; set; }
        public string ApplicationDate { get; set; }
        public int WorkStatus { get; set; }
        public string DueDate { get; set; }
        public string MoveFrom { get; set; }
        public string MoveTo { get; set; }
        public string CheckerName { get; set; }
        public int Days { get; set; }
        public long ApplicationRevisionId { get; set; }
        public string HaulierMnemonic { get; set; }
        public int ESDALRefNum { get; set; }
        public long ProjectId { get; set; }
        public long VersionId { get; set; }
        public int IsHistoric { get; set; }
        public string AssignedUserName { get; set; }
    }

    //model for filtering SOA Movement Inbox
    public class MovementsInboxFilter
    {
        //for Show Only options
        public bool Accepted { get; set; }
        public bool Rejected { get; set; }
        
        public bool Unopened { get; set; }
        public bool Opened { get; set; }
        public bool Withdrawn { get; set; }
        public bool Declined { get; set; }
        public bool ImminentMovement { get; set; } // added for NEN
        public bool UnderAssessmentbyMe { get; set; } // added for NEN
        public bool UnderAssessment { get; set; }
        public bool UnderAssessmentbyOtherUser { get; set; }
        public bool IncludeHistorical { get; set; }
        public bool IncludeFailedDelivery { get; set; }
        public int UserID { get; set; }

        //For Filter Options

        public int IsNen { get; set; }
        public bool SO { get; set; }
        public bool STGOVR1 { get; set; }
        public bool STGO { get; set; }
        public bool CandU { get; set; }

        public bool Tracked { get; set; }
        public bool VSO { get; set; }

        //for textbox search
        public string ESDALReference { get; set; }
        public string HaulierName { get; set; }
        public string HaulierReference { get; set; }

        //for Search By Date

        public bool EnableMovementDate { get; set; }

        public bool MovementDateCheck { get; set; }
        [MustBeGreaterThanOrEqual("MovementToDate", "Start date must be greater than end date")]
        public string MovementFromDate { get; set; }
        public DateTime? MovementFrom { get; set; }
        public string MovementToDate { get; set; }
        public DateTime? MovementTo { get; set; }
        public bool EnableReceiptDate { get; set; }
        [MustBeGreaterThanOrEqual("ToReceiptDateOfCommn", "Start date must be greater than end date")]
        public string FromReceiptDateOfCommn { get; set; }
        public DateTime? FromReceipt { get; set; }
        public string ToReceiptDateOfCommn { get; set; }
        public DateTime? ToReceipt { get; set; }

        public bool ShowMostRecentVersion { get; set; }
        public int StructureId { get; set; }

        public bool MovementDate { get; set; }
        public bool ReceiveDate { get; set; }
        public int SortOrderValue { get; set; }
        public int SortTypeValue { get; set; }
        public int SortOrder { get; set; }
        // for textbox and dropdown search
        public string DelegationArrangement { get; set; }
        public string StartOrEnd { get; set; }
        public List<DelegArrangeNameList> DelegationArrangList { get; set; }
        public List<filters> dynamicfilters { get; set; }
        public string StructureReferenceNo { get; set; }
        public double? GrossWeight { get; set; }
        public double? GrossWeight1 { get; set; }
        public double? OverallWidth { get; set; }
        public double? OverallWidth1 { get; set; }
        public double? OverallLength { get; set; }
        public double? OverallLength1 { get; set; }
        public double? RigidLength { get; set; }
        public double? RigidLength1 { get; set; }
        public double? Height { get; set; }
        public double? Height1 { get; set; }
        public double? AxleWeight { get; set; }
        public double? AxleWeight1 { get; set; }
        public int WeightCount { get; set; }
        public int WidthCount { get; set; }
        public int LengthCount { get; set; }
        public int RigidLengthCount { get; set; }
        public int HeightCount { get; set; }
        public int AxleCount { get; set; }

        public int? GrossWeightAndOr { get; set; }
        public int? OverallWidthAndOr { get; set; }
        public int? OverallLengthAndOr { get; set; }
        public int? RigidLengthAndOr { get; set; }
        public int? HeightAndOr { get; set; }

       
        public int ObjectDelegationList { get; set; }
        public bool Suitable { get; set; }
        public bool UnSuitable { get; set; }
        public bool MarginallySuitable { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string QueryString { get; set; }
        public long FolderId { get; set; }
        public int LogicOpr { get; set; }
    }

    //for Advanced Filtering
    public class MovementsInboxAdvancedFilter
    {
        public int SortOrder { get; set; }
        // for textbox and dropdown search
        public string DelegationArrangement { get; set; }
        public string StartOrEnd { get; set; }
        public List<DelegArrangeNameList> DelegationArrangList { get; set; }
        public List<filters> dynamicfilters { get; set; }
        public string StructureReferenceNo { get; set; }
        [DisplayName("gross_weight_max_kg")]
        public double? GrossWeight { get; set; }
        [DisplayName("gross_weight_max_kg1")]
        public double? GrossWeight1 { get; set; }
        public double? OverallWidth { get; set; }
        public double? OverallWidth1 { get; set; }
        public double? OverallLength { get; set; }
        public double? OverallLength1 { get; set; }
        public double? RigidLength { get; set; }
        public double? RigidLength1 { get; set; }
        public double? Height { get; set; }
        public double? Height1 { get; set; }
        public double? AxleWeight { get; set; }
        public double? AxleWeight1 { get; set; }
        public int WeightCount { get; set; }
        public int WidthCount { get; set; }
        public int LengthCount { get; set; }
        public int RigidLengthCount { get; set; }
        public int HeightCount { get; set; }
        public int AxleCount { get; set; }

        public int? GrossWeightAndOr { get; set; }
        public int? OverallWidthAndOr { get; set; }
        public int? OverallLengthAndOr { get; set; }
        public int? RigidLengthAndOr { get; set; }
        public int? HeightAndOr { get; set; }

        public int? StructureId { get; set; }

        public int ObjectDelegationList { get; set; }
        public bool Suitable { get; set; }
        public bool UnSuitable { get; set; }
        public bool MarginallySuitable { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string QueryString { get; set; }
        public long FolderId { get; set; }
        public int LogicOpr { get; set; }

        public int Operator { get; set; }
    }

    public class FolderNameList
    {
        public long FolderId { get; set; }
        public string FolderName { get; set; }

    }

    # region NetWeb code changes
    /// <summary>
    /// Model for movement
    /// </summary>
    public class MovementModel
    {
        public long ProjectId { get; set; }
        public long NotificationId { get; set; }
        public long RevisionId { get; set; }
        public long VersionId { get; set; }
        public int VehicleClassification { get; set; }
        public string VehicleClassificationName { get; set; }

        public decimal Status { get; set; }
        public string StatusName { get; set; }
        public string Notes { get; set; }
        public string NotificationCode { get; set; }
        public string HAJobFileReference { get; set; }

        public string HauliersReference { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string HaulierName { get; set; }
        public string LoadDescription { get; set; }
        public DateTime MoveStartDate { get; set; }
        public DateTime MoveEndDate { get; set; }
        public byte[] HauliersNotification { get; set; }
        public int TotalMoves { get; set; }
        public int MaxPartPerMove { get; set; }
        public byte[] AffectedParties { get; set; }
        public string ApplicationNotes { get; set; }
        public string HAContact { get; set; }
        public string InternalNotes { get; set; }
        public long DocumentId { get; set; }
        public decimal TotalRecordCount { get; set; }
        public List<CollaborationNotes> CollaborationNotes { get; set; }
        public long AnalysisId { get; set; }

        public string VehicleName { get; set; }

        public string FromDescription { get; set; }

        public string ToDescription { get; set; }

        public short IsMostRecent { get; set; }
        public byte[] OutboundNotification { get; set; }
        public byte[] InboundNotification { get; set; }
        public string Licence { get; set; }
        public string IndemnityConfirmation { get; set; }
        public string OnBehalfOf { get; set; }
        public string HaulierContact { get; set; }
        public decimal HaulierContactId { get; set; }

        public string NotesOnEscort { get; set; }

        public int VehicleId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public List<SpecialOrder> SpecialOrders { get; set; }

        public decimal HAContactId { get; set; }

        public List<RelatedCommunication> RelatedCommunications { get; set; }
        public List<VehicleConfigration> VehicleConfigurations { get; set; }
        public DateTime ApplicationDate { get; set; }

        public decimal DrivingId { get; set; }
        public string DrivingInstruction { get; set; }
        public string NotesFromHaulier { get; set; }
        public string Route { get; set; }


        public string VR1Number { get; set; }
        public List<VR1> VR1s { get; set; }
        public byte[] Document { get; set; }

        public string EmailAddress { get; set; }
        public int RequiresVR1 { get; set; }

        public long ContactId { get; set; }

        public long OrganisationId { get; set; }

        public string ESDALReference { get; set; }

        public string OrderNumber { get; set; }

        public long InboxId { get; set; }

        public decimal IncludeDockCaution { get; set; }

        public List<Dispensations> DispensationList { get; set; }

        public byte[] DocumentDetails { get; set; }

        public short CollaborationNo { get; set; }

        public string SoNumbers { get; set; }

        public long UserId { get; set; }

        public bool NenFlag { get; set; }

        public long NenId { get; set; }

        public long RouteStatus { get; set; }

        public int NenProcess { get; set; }

        public long LoginUserId { get; set; }

        public string UserName { get; set; }

        public int WIPNENProcess { get; set; }

        public int MailedCollab { get; set; }

        public NENRequiredFieldList NENFieldObject { get; set; }

        public string FromUserName { get; set; }// For NEN project

        public int ItemStatus { get; set; }

        //added by Poonam on 20Jun18 to fix RM11275
        public string OtherContactDetails { get; set; }

        public long RoutePartId { get; set; }

        public string RouteType { get; set; }
        public bool IsSimplified { get; set; }
        public string DispensationIds { get; set; }
        public byte[] HAContactDetails { get; set; }
        public string VSONo { get; set; }
        public byte[] AuthenticationNotesToHaulier { get; set; }
        public string AuthenticationNotesToHaulierContent { get; set; }
    }

    public class MovementModelPanel
    {
        public long NotificationId { get; set; }
        public string ESDALReference { get; set; }
        public string Route { get; set; }
        public string VehicleClassificationName { get; set; }
        public string DRN { get; set; }
        public int IsNEN { get; set; } // For NEN project
    }

    public class Dispensations
    {
        public string DRN { get; set; }
        public string Summary { get; set; }
    }

    public class CollaborationNotes
    {
        public long DocumentId { get; set; }
        public string When { get; set; }
        public string Notes { get; set; }
        public int Acknowledged { get; set; }
        public string AcknowledgedAgainst { get; set; }
        public DateTime AcknowledgedWhen { get; set; }
    }
    public class SpecialOrder
    {
        public string OrderNo { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
    public class RelatedCommunication
    {
        public long NotificationId { get; set; }
        public DateTime NotificationDate { get; set; }

        public string NotificationCode { get; set; }
        public string PreviousNotificationESDALReference { get; set; }

        public long InboxItemId { get; set; }
        public string ItemStatus { get; set; }

        public string EncrapNotificationId { get; set; }
        public string EncryptESDALReference { get; set; }
        public string EncryptRoute { get; set; }
        public string EncryptInboxId { get; set; }
    }
    public class VehicleConfigration
    {
        public int VehicleClassification { get; set; }
        public string VehicleName { get; set; }
        public decimal VehicleId { get; set; }
        public int VehiclePurpose { get; set; }
        public int VehicleType { get; set; }
        public List<VehicleConfigList> VehicleCompList { get; set; }
        public List<string> VehicleNameList { get; set; }
        public VehicleConfigration()
        {
            VehicleCompList = new List<VehicleConfigList>();
            VehicleNameList = new List<string>();
        }
    }
    public class VR1
    {
        public Int32 VR1Id { get; set; }
        public string VR1Number { get; set; }
        public string ScottishVR1Number { get; set; }
    }

    public class MovementPrint
    {
        public long ContactId { get; set; }

        public long OrganisationId { get; set; }

        public string ESDALReferenceNumber { get; set; }

        public long VersionId { get; set; }

        public string OrderNumber { get; set; }

        public long ProjectId { get; set; }
    }

    #endregion

    #region MovementCopyDetails
    /// <summary>
    /// class to store variables while cloning movement details from sort to portal and vice versa
    /// </summary>
    public class MovementCopyDetails
    {
        public string ESDALReference { get; set; }

        public long ProjectId { get; set; }

        public long RevisionId { get; set; }

        public long VersionId { get; set; }

        public int VersionNo { get; set; }

        public string HaulMnemonic { get; set; }

        public string ESDALRefNo { get; set; }

        public int MovementStatus { get; set; }
    }
    #endregion

    public class DelegArrangeNameList
    {
        public long ArrangementId { get; set; }
        public string ArrangementName { get; set; }
    }
    public class filters
    {
        public string SOAVehicleDimension { get; set; }
        public string OperatorCount { get; set; }
        public string Operator { get; set; }
        public string GrossWeight { get; set; }
        public string GrossWeight1 { get; set; }
    }

    //For NEN project
    public class NENRequiredFieldList
    {
        public long NENInboxItemId { get; set; }
        public long NENRoutePartId { get; set; }
        public long NENId { get; set; }
        public int NENInboxVersionNo { get; set; }
        public long NENUserId { get; set; }
        public long NENAnalysisId { get; set; }
        public int NENRouteStatus { get; set; }
    }
}