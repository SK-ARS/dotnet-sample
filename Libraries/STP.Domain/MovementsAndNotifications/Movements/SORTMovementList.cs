using NetSdoGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    #region public class SORTMovementList
    public class SORTMovementList
    {
        /// <summary>
        /// SORT type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// SORT priority
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// ESDAL2 Reference string
        /// </summary>
        public string ESDALRef { get; set; }
        /// <summary>
        /// Work status
        /// </summary>
        public int WorkStatus { get; set; }
        /// <summary>
        /// Project status
        /// </summary>
        public int ProjectStatus { get; set; }
        /// <summary>
        /// Owner name
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// Owner name
        /// </summary>
        public string CheckerName { get; set; }
        /// <summary>
        /// Application data
        /// </summary>
        public string ApplicationDate { get; set; }
        /// <summary>
        /// Due date
        /// </summary>
        public string DueDate { get; set; }
        /// <summary>
        /// From date
        /// </summary>
        public string FromDate { get; set; }
        /// <summary>
        /// To date
        /// </summary>
        public string ToDate { get; set; }
        /// <summary>
        /// Movement from location
        /// </summary>
        public string MoveFrom { get; set; }
        /// <summary>
        /// Movement to location
        /// </summary>
        public string MoveTo { get; set; }
        /// <summary>
        /// Movement to location
        /// </summary>
        public int VehicleClassification { get; set; }
        /// <summary>
        /// Total record count
        /// </summary>
        public int TotalRecordCount { get; set; }
        /// <summary>
        /// MovementVersionId
        /// </summary>
        public long MovementVersionID { get; set; }
        /// <summary>
        /// MovementRevisionId
        /// </summary>
        public long MovementRevisionID { get; set; }
        /// <summary>
        /// ProjectEsdalReference
        /// </summary>
        public int ProjectEsdalReference { get; set; }
        /// <summary>
        /// HaulierMnemonic
        /// </summary>
        public string HaulierMnemonic { get; set; }
        /// <summary>
        /// ApplicationRevisionNum
        /// </summary>
        public int ApplicationRevisionNumber { get; set; }
        /// <summary>
        /// MovementVersionNumber
        /// </summary>
        public int MovementVersionNumber { get; set; }
        /// <summary>
        /// MovementType
        /// </summary>
        public string MovementType { get; set; }
        /// <summary>
        /// VersionStatus
        /// </summary>
        public int VersionStatus { get; set; }
        /// <summary>
        /// ESDALReference
        /// </summary>
        public string ESDALReference { get; set; }
        /// <summary>
        /// MyReference
        /// </summary>
        public string MyReference { get; set; }
        /// <summary>
        /// AppRevId
        /// </summary>
        public long AppRevID { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectID { get; set; }
        /// <summary>
        /// RevisionNo
        /// </summary>
        public int RevisionNo { get; set; }

        public string CommittedDate { get; set; }
        /// <summary>
        /// ApplicationDate1
        /// </summary>        
        public string ApplicationDate1 { get; set; }
        /// <summary>
        /// VersionNo
        /// </summary>
        public long VersionNo { get; set; }
        /// <summary>
        /// version_id
        /// </summary>
        public long VersionID { get; set; }
        /// <summary>
        /// Revision_Id
        /// </summary>
        public long RevisionID { get; set; }
        /// <summary>
        /// Order_No
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// Expiry_Date
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// OrganisationId
        /// </summary>
        public int OrganisationID { get; set; }
        /// <summary>
        /// AppStatus
        /// </summary>
        public int AppStatus { get; set; }
        /// <summary>
        /// Enter_By_SORT
        /// </summary>
        public int EnterBySORT { get; set; }
        public DateTime SOCreateDate { get; set; }

        public int IsWithdrawn { get; set; }
        public int IsDeclined { get; set; }
        public long PlannerID { get; set; }
        public long CheckerID { get; set; }
        //5189
        public string DistributionDate { get; set; }
        public int Days { get; set; }
        public int IsNotified { get; set; }
        public long AnalysisId { get; set; }
    }
    #endregion public class SORTMovementList

    public class SORTMovementListParams
    {
        public int OrganisationID { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public SORTMovementFilter SORTMovementFilter { get; set; }
        public SortAdvancedMovementFilter SORTAdvMovementFilter { get; set; }
        public bool IsCreCandidateOrCreAppl { get; set; }
        public bool PlanMovement { get; set; }
        public SortMapFilter SortObjMapFilter { get; set; }
        public int sortOrder { get; set; }
        public int sortType { get; set; }
    }

    public class SortAdvancedMovementFilter
    {
        #region advanced filter

        #region General
        public int SORTOrder { get; set; }
        
        /// <summary>
        /// Keyword search checkbox
        /// </summary>
        public bool KeyCheck { get; set; }
        /// <summary>
        /// Keyword
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// Load details checkbox
        /// </summary>
        public bool LoadDetailsCheck { get; set; }
        /// <summary>
        /// Load details
        /// </summary>
        public string LoadDetails { get; set; }
        /// <summary>
        /// Include pre-ESDAL movements checkbox
        /// </summary>

        /// <summary>
        /// Job reference number checkbox
        /// </summary>
        public bool JobReferenceCheck { get; set; }
        /// <summary>
        /// Job reference number
        /// </summary>
        public string JobReference { get; set; }

        /// <summary>
        /// structure reference number checkbox
        /// </summary>
        public bool StructReferenceCheck { get; set; }
        /// <summary>
        /// structure reference number
        /// </summary>
        public string StructReference { get; set; }


        /// <summary>
        /// Special order number checkbox
        /// </summary>
        public bool SONumCheck { get; set; }
        /// <summary>
        /// Special order number
        /// </summary>
        public string SONum { get; set; }
        /// <summary>
        /// VR 1 number checkbox
        /// </summary>
        public bool VRNumCheck { get; set; }
        /// <summary>
        /// VR 1 number text
        /// </summary>
        public string VRNum { get; set; }
        /// <summary>
        /// Application type checkbox
        /// </summary>
        public bool ApplicationTypeCheck { get; set; }
        /// <summary>
        /// Application type of type int
        /// </summary>
        public int ApplicationType { get; set; }

        public string ESDALReferenceNumber { get; set; }

        public string HaulierName { get; set; }
        #endregion General

       

        #region Vehicle configuration
        /// <summary>
        /// Width checkbox
        /// </summary>
        public bool WidthCheck { get; set; }
        /// <summary>
        /// Width condition check dropdown
        /// </summary>
        public int WidthEqualDrop { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        public string Width { get; set; }

        public string Width1 { get; set; }
        /// <summary>
        /// Height checkbox
        /// </summary>
        public bool HeightCheck { get; set; }
        /// <summary>
        /// Height condition check dropdown
        /// </summary>
        public int HeightEqualDrop { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public string Height { get; set; }

        public string Height1 { get; set; }
        /// <summary>
        /// Reducible Height checkbox
        /// </summary>
        public bool RedHeightCheck { get; set; }
        /// <summary>
        /// Reducible Height condition check dropdown
        /// </summary>
        public int RedHeightEqualDrop { get; set; }
        /// <summary>
        /// Reducible Height
        /// </summary>
        public string RedHeight { get; set; }
        public string RedHeight1 { get; set; }
        /// <summary>
        /// Overall length checkbox
        /// </summary>
        public bool OverallLengthCheck { get; set; }
        /// <summary>
        /// Overall length condition check dropdown
        /// </summary>
        public int OverallLengthEqualDrop { get; set; }
        /// <summary>
        /// Overall length
        /// </summary>
        public string OverallLength { get; set; }
        public string OverallLength1 { get; set; }
        /// <summary>
        /// Rigid Length checkbox
        /// </summary>
        public bool RigidLengthCheck { get; set; }
        /// <summary>
        /// Rigid Length condition check dropdown
        /// </summary>
        public int RigidLengthEqualDrop { get; set; }
        /// <summary>
        /// Rigid Length
        /// </summary>
        public string RigidLength { get; set; }
        public string RigidLength1 { get; set; }
        /// <summary>
        /// Max Axle Weight checkbox
        /// </summary>
        public bool MaxAxleWeightCheck { get; set; }
        /// <summary>
        /// Max Axle Weight condition check dropdown
        /// </summary>
        public int MaxAxleWeightEqualDrop { get; set; }
        /// <summary>
        /// Max Axle Weight
        /// </summary>
        public string MaxAxleWeight { get; set; }
        public string MaxAxleWeight1 { get; set; }
        /// <summary>
        /// Max Weight checkbox
        /// </summary>
        public bool MaxWeightCheck { get; set; }
        /// <summary>
        /// Max Weight condition check dropdown
        /// </summary>
        public int MaxWeightEqualDrop { get; set; }
        /// <summary>
        /// Max Weight
        /// </summary>
        public string MaxWeight { get; set; }
        public string MaxWeight1 { get; set; }
        public string QueryString { get; set; }

        #endregion Vehicle configuration

        #region Location
        /// <summary>
        /// Start/End Point checkbox
        /// </summary>
        public bool StartEndCheck { get; set; }
        /// <summary>
        /// Start/End point
        /// </summary>
        public string StartPoint { get; set; }

        /// <summary>
        /// End point
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Structure checkbox
        /// </summary>
        public bool StructureCheck { get; set; }
        /// <summary>
        /// Structure
        /// </summary>
        public string Structure { get; set; }
        /// <summary>
        /// Road Segment checkbox
        /// </summary>
        public bool RoadSegmentCheck { get; set; }
        /// <summary>
        /// Road Segment
        /// </summary>
        public string RoadSegment { get; set; }
        /// <summary>
        /// MTP checkbox
        /// </summary>
        public bool MTPCheck { get; set; }
        /// <summary>
        /// MTP
        /// </summary>
        public string MTP { get; set; }
        /// <summary>
        /// Area checkbox
        /// </summary>
        public bool AreaCheck { get; set; }
        /// <summary>
        /// Area
        /// </summary>
        public string Area { get; set; }

        public int LogicOpr { get; set; }

        #endregion Location 
        public long FolderId { get; set; }

        #endregion advanced filter
    }
    public class SortMapFilter
    {
        public Int32 Flag { get; set; }
        public string StructureList { get; set; }
        public sdogeometry Geometry { get; set; }
        public int StructureCount { get; set; }
    }

    public class MapStructLink
    {
        public long StructureId { get; set; }
        public long LinkId { get; set; }
        public string StructureType { get; set; }
       public sdogeometry Geometry { get; set; }
        public sdogeometry PointGeometry { get; set; }
        public string StructureClass { get; set; }

        public string StructureCode { get; set; }

        public string StructureName { get; set; }
        

    }
    #region public class SORTMovementFilter
    public class SORTMovementFilter
    {
        #region normal filters

        #region SORT movement status
        //SORT movement status
        /// <summary>
        /// Unallocated
        /// </summary>
        public bool Unallocated { get; set; }
        /// <summary>
        /// In progress
        /// </summary>
        public bool InProgress { get; set; }
        /// <summary>
        /// Proposed
        /// </summary>
        public bool Proposed { get; set; }
        /// <summary>
        /// Re-proposed
        /// </summary>
        public bool ReProposed { get; set; }
        /// <summary>
        /// Agreed
        /// </summary>
        public bool Agreed { get; set; }
        /// <summary>
        /// Agreed revised
        /// </summary>
        public bool AgreedRevised { get; set; }
        /// <summary>
        /// Agreed recleared
        /// </summary>
        public bool AgreedRecleared { get; set; }
        /// <summary>
        /// Withdrawn
        /// </summary>
        public bool Withdrawn { get; set; }
        /// <summary>
        /// Declined
        /// </summary>
        public bool Declined { get; set; }
        /// <summary>
        /// Planned
        /// </summary>
        public bool Planned { get; set; }
        /// <summary>
        /// Approved
        /// </summary>
        public bool Approved { get; set; }
        /// <summary>
        /// Include historic movement
        /// </summary>
        /// 
        public bool InclPreESDALMov { get; set; }
        public bool InclHistoricMov { get; set; }

        /// <summary>
        /// Revised
        /// </summary>
        public bool Revised { get; set; }
        public bool IsNotified { get; set; }


        #endregion SORT movement status

        #region SORT movement othres
        //SORT movement othres
        /// <summary>
        /// ESDAL reference check box
        /// </summary>
        public bool ESDALReferenceCheck { get; set; }
        /// <summary>
        /// ESDAL reference text
        /// </summary>
        public string ESDALReference { get; set; }
        /// <summary>
        /// Haulier name checkbox
        /// </summary>
        public bool HaulierNameCheck { get; set; }
        /// <summary>
        /// Haulier name text
        /// </summary>
        public string HaulierName { get; set; }
        /// <summary>
        /// Allocated user checkbox
        /// </summary>
        public bool AllocateUserCheck { get; set; }
        /// <summary>
        /// Allocated user text
        /// </summary>
        public string AllocateUser { get; set; }
        /// <summary>
        /// Checking user checkbox
        /// </summary>
        public bool CheckingUserCheck { get; set; }
        /// <summary>
        /// Checking user text
        /// </summary>
        public string CheckingUser { get; set; }
        /// <summary>
        /// Priority checkbox
        /// </summary>
        public bool PriorityCheck { get; set; }
        /// <summary>
        /// Priority text
        /// </summary>
        public string Priority { get; set; }
        /// <summary>
        /// Special Order issued checkbox
        /// </summary>
        public bool SOIssued { get; set; }

        /// <summary>
        /// Show my projects
        /// </summary>
        public bool ShowMyProjects { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public string UserID { get; set; }

        public int SORTOrder { get; set; }

        #endregion SORT movement othres

        #region SORT movement dates
        //SORT movement dates
        /// <summary>
        /// Special Order Signing date checkbox
        /// </summary>
        public bool SOSignDateCheck { get; set; }
        /// <summary>
        /// Special Order Signing start date
        /// </summary>
        public string SOSignFromDate { get; set; }
        public DateTime? SOSignFrom { get; set; }

        /// <summary>
        /// Special Order Signing end date
        /// </summary>
        public string SOSignToDate { get; set; }
        public DateTime? SOSignTo { get; set; }
        /// <summary>
        /// Application date checkbox
        /// </summary>
        public bool ApplicationDateCheck { get; set; }
        /// <summary>
        /// Application start date
        /// </summary>
        public string ApplicationFromDate { get; set; }
        public DateTime? ApplicationFrom { get; set; }
        /// <summary>
        /// Application end date
        /// </summary>
        public string ApplicationToDate { get; set; }
        public DateTime? ApplicationTo { get; set; }
        /// <summary>
        /// Revision date checkbox
        /// </summary>
        public bool RevDateCheck { get; set; }
        /// <summary>
        /// Revision start date
        /// </summary>
        public DateTime? RevFromDate { get; set; }
        /// <summary>
        /// Revision end date
        /// </summary>
        public DateTime? RevToDate { get; set; }
        /// <summary>
        /// Assigned date checkbox
        /// </summary>
        public bool AssignDateCheck { get; set; }
        /// <summary>
        /// Assigned start date
        /// </summary>
        public string AssignFromDate { get; set; }
        public DateTime? AssignFrom { get; set; }
        /// <summary>
        /// Assigned end date
        /// </summary>
        public string AssignToDate { get; set; }
        public DateTime? AssignTo { get; set; }
        /// <summary>
        /// Movement date checkbox
        /// </summary>
        public bool MovDateCheck { get; set; }
        /// <summary>
        /// Movement start date
        /// </summary>
        public string MovFromDate { get; set; }
        public DateTime? MovFrom { get; set; }
        /// <summary>
        /// Movement end date
        /// </summary>
        public string MovToDate { get; set; }
        public DateTime? MovTo { get; set; }
        /// <summary>
        /// Work due date checkbox
        /// </summary>
        public bool WorkDueDateCheck { get; set; }
        /// <summary>
        /// Work due start date
        /// </summary>
        public string WorkDueFromDate { get; set; }
        public DateTime? WorkDueFrom { get; set; }
        /// <summary>
        /// Work due end date
        /// </summary>
        public string WorkDueToDate { get; set; }
        public DateTime? WorkDueTo { get; set; }
        /// <summary>
        /// Declined date checkbox
        /// </summary>
        public bool DeclDateCheck { get; set; }
        /// <summary>
        /// Declined start date
        /// </summary>
        public string DeclFromDate { get; set; }
        /// <summary>
        /// Declined end date
        /// </summary>
        public string DeclToDate { get; set; }
        public int SortOrderValue { get; set; }
        public int SortTypeValue { get; set; }
        public int pagesize { get; set; }

        #endregion SORT movement dates


        #endregion normal filters

    }
    #endregion public class SORTMovementFilter

    #region GetSORTUserList
    public class GetSORTUserList
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long ContactID { get; set; }
        public string UserTypeID { get; set; }
    }
    #endregion
}