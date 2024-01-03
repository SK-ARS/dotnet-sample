using NetSdoGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace STP.Domain.RouteAssessment
{
    public class GenerateRouteAssessmentModel
    {
        public long VersionId { get; set; }
        public string ContentRefNo { get; set; }
        public long RevisionId { get; set; }
        public long NotificationId { get; set; }
        public long AnalysisId { get; set; }
        public long PrevAnalysisId { get; set; }
        public int VSoType { get; set; }
        public bool IsRenotify { get; set; }
        public int AppRevId { get; set; }
        public string PreviousContactName { get; set; }
        public bool NoGenerateAffectedStructures { get; set; }
        public bool NoGenerateAffectedRoads { get; set; }
        public bool NoGenerateAffectedAnnotations { get; set; }
        public bool NoGenerateAffectedCautions { get; set; }
        public bool NoGenerateAffectedConstraints { get; set; }
        public bool NoGenerateAffectedParties { get; set; }
        public bool IsNenApi { get; set; }
    }
    public class RouteAssessmentModel
    {
        public long AnalysisId { get; set; }
        public byte[] DriveInst { get; set; }
        public byte[] Cautions { get; set; }
        public byte[] RouteDescription { get; set; }
        public byte[] AffectedStructure { get; set; }
        public byte[] Annotation { get; set; }
        public byte[] Constraints { get; set; }
        public byte[] AffectedParties { get; set; }
        public byte[] AffectedRoads { get; set; }
    }
    #region RouteAnalysisXml
    /// <summary>
    /// Class to include xml string related variables
    /// </summary>
    public class RouteAnalysisXml
    {
        public string XmlAnalysedStructure { get; set; }
        public string XmlAnalysedConstraints { get; set; }
        public string XmlAnalysedCautions { get; set; }
        public string XmlAnalysedAnnotations { get; set; }
        public RouteAnalysisXml()
        { }
        public string XmlAffectedParties { get; set; }
        public string XmlAffectedRoads { get; set; }
    }
    #endregion
    #region RouteConstraints Class
    /// <summary>
    /// Constraints class to set and get constraints
    /// </summary>
    [DataContract]
    public class RouteConstraints
    {
        [DataMember]
        public long ConstraintId { get; set; }

        [DataMember]
        public string ConstraintType { get; set; }

        [DataMember]
        public long ConstraintTypeCode { get; set; }

        [DataMember]
        public string TopologyType { get; set; }

        [DataMember]
        public long TopologyTypeCode { get; set; }

        [DataMember]
        public string TraversalType { get; set; }

        [DataMember]
        public long TraversalTypeCode { get; set; }

        [DataMember]
        public sdogeometry ConstraintGeometry { get; set; }

        [DataMember]
        public DateRange Date { get; set; }

        [DataMember]
        public List<RouteCautions> CautionList { get; set; }

        [DataMember]
        public string ConstraintName { get; set; }

        [DataMember]
        public string ConstraintCode { get; set; }

        [DataMember]
        public List<ConstraintReferences> ConstraintRefrences { get; set; }

        [DataMember]
        public ConstraintValues ConstraintValue { get; set; }

        [DataMember]
        public List<AssessmentContacts> ConstraintContact { get; set; }

        public RouteConstraints()
        {
            Date = null;
            CautionList = null;
            ConstraintValue = null;
            ConstraintRefrences = null;
            ConstraintContact = null;
        }

        [DataMember]
        public string ConstraintSuitability { get; set; }
    }
    #endregion
    #region DateRange
    /// <summary>
    /// Class to get and set Start Date and End Date
    /// </summary>
    [DataContract]
    public class DateRange
    {
        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }
    }
    #endregion
    #region RouteCautions Class
    /// <summary>
    /// Cautions class
    /// </summary>
    [DataContract]
    public class RouteCautions
    {
        [DataMember]
        public int OrganisationId { get; set; }

        [DataMember]
        public long CautionId { get; set; }

        [DataMember]
        public long CautionConstraintId { get; set; }

        [DataMember]
        public string CautionName { get; set; }

        [DataMember]
        public string cautDescription { get; set; }

        [DataMember]
        public List<AssessmentContacts> CautionContactList { get; set; }

        [DataMember]
        public ConstraintValues CautionConstraintValue { get; set; }

        public RouteCautions()
        {
            CautionContactList = null;
            CautionConstraintValue = null;
        }

        [DataMember]
        public long OwnerOrgId { get; set; }

        [DataMember]
        public string CautionConstraintName { get; set; }

        [DataMember]
        public string CautionConstraintCode { get; set; }

        [DataMember]
        public string CautionConstraintType { get; set; }

        [DataMember]
        public long CautionStructureId { get; set; }

        [DataMember]
        public long CautionSectionId{ get; set; }

        [DataMember]
        public string RoadName { get; set; }

        [DataMember]
        public string Suitability { get; set; }

        [DataMember]
        ///member which determines the type of caution 0 : road caution, 1 : structure caution
        public int CautionType { get; set; }
    }
    #endregion
    #region ConstraintReferences Class
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ConstraintReferences
    {

        [DataMember]
        public long constLink { get; set; }

        [DataMember]
        public long ToNorthing { get; set; }

        [DataMember]
        public long ToEasting { get; set; }

        [DataMember]
        public long FromNorthing { get; set; }

        [DataMember]
        public long FromEasting { get; set; }

        [DataMember]
        public long LinearRef { get; set; }

        [DataMember]
        public long? ToLinearRef { get; set; }

        [DataMember]
        public long? FromLinearRef { get; set; }

        [DataMember]
        public long Easting { get; set; }

        [DataMember]
        public long Northing { get; set; }


        [DataMember]
        public bool IsPoint { get; set; }

        [DataMember]
        public int? Direction { get; set; }

        [DataMember]
        public int? ConstraintLinkNo { get; set; }
    }
    #endregion
    #region ConstraintValues Class
    /// <summary>
    /// Class to store constraint specific values.
    /// </summary>
    [DataContract]
    public class ConstraintValues
    {
        [DataMember]
        public long GrossWeight { get; set; }

        [DataMember]
        public long AxleWeight { get; set; }

        [DataMember]
        public Single MaxHeight { get; set; }

        [DataMember]
        public Single MaxLength { get; set; }

        [DataMember]
        public Single MaxWidth { get; set; }

        [DataMember]
        public float MinSpeed { get; set; }

        public ConstraintValues()
        {
            GrossWeight = 0;
            AxleWeight = 0;
            MaxHeight = 0;
            MaxHeight = 0;
            MaxLength = 0;
            MaxWidth = 0;
        }

    }
    #endregion
    #region CautionContacts Class
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AssessmentContacts
    {

        [DataMember]
        public long ContactId { get; set; }

        [DataMember]
        public string ContactName { get; set; }

        [DataMember]
        public long OrganisationId { get; set; }

        [DataMember]
        public string OrganisationName { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public List<string> AddressLine { get; set; }

        [DataMember]
        public string PostCode { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public int RoleTypeCode { get; set; }

        [DataMember] //Enumerated Role Type Object
        public XmlAnalysedCautions.RoleType RoleName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public object IsAdHoc { get; set; }

        [DataMember]
        public object CountryCode { get; set; }

        [DataMember]
        public object Telephone { get; set; }

        [DataMember]
        public object Extension { get; set; }

        [DataMember]
        public XmlAffectedParties.AffectedPartyReasonType AffectedReasonType { get; set; }

        [DataMember]
        public XmlAffectedParties.DispensationStatusType DispensationStatus { get; set; }

        [DataMember]
        public string DispensationStatusType { get; set; }

        [DataMember]
        public XmlAffectedParties.AffectedPartyReasonExclusionOutcomeType AffectedExclusionOutcome { get; set; }

        [DataMember]
        public long TotalRecords { get; set; }

        [DataMember]
        public string OrganisationType { get; set; }

        [DataMember]
        public List<long> DispensationId { get; set; }

        [DataMember]
        public int UserAddedAffectedContact { get; set; }

        [DataMember]
        public long OwnerOrgId { get; set; }

        [DataMember]
        public long OwnerArrangementId { get; set; }

        [DataMember]
        public string OwnerOrgName { get; set; }

        [DataMember]
        public bool RetainNotification { get; set; }

        [DataMember]
        public bool RecieveFailures { get; set; }

        public AssessmentContacts()
        {
            DispensationStatus = XmlAffectedParties.DispensationStatusType.nonematching;

            AffectedReasonType = XmlAffectedParties.AffectedPartyReasonType.newlyaffected;

            DispensationStatusType = "None Matching";

            AffectedExclusionOutcome = XmlAffectedParties.AffectedPartyReasonExclusionOutcomeType.newlyaffected;
        }
    }
    #endregion

    #region RouteAssessmentInputs
    /// <summary>
    /// Class to store route assessment related input's
    /// </summary>
    public class RouteAssessmentInputs
    {
        /// <summary>
        /// Notification Route of Simplified/Detailed Notification
        /// </summary>
        public string ContentReferenceNo { get; set; }

        public long AnalysisId { get; set; }

        /// <summary>
        /// Candidate Route of SORT linked through revision_id. This is used only in SORT schema
        /// </summary>
        public long RevisionId { get; set; }

        /// <summary>
        /// New change for showing Application planned routes linking through APP_REVISION_ID
        /// </summary>
        public long AppRevisionId { get; set; }

        /// <summary>
        /// Agreed Route of SO and VR1 Application linked through movement version
        /// </summary>
        public long VersionId { get; set; }

        /// <summary>
        /// variable to check whether the route is a condidate route or not 
        /// </summary>
        public bool IsCanditateRoute { get; set; }

        /// <summary>
        /// organisation id for which the assessment is carried out
        /// </summary>
        public long OrganisationId { get; set; }

        public RouteAssessmentInputs()
        {

        }

    }
    #endregion
    #region RouteAnalysisObject
    public class RouteAnalysisObject
    {
        public XmlAffectedParties.AffectedPartiesStructure AffectedPartyList { get; set; }
        public XmlAnalysedAnnotations.AnalysedAnnotations AnnotationList { get; set; }
        public XmlAnalysedCautions.AnalysedCautions AnalysedCautionList { get; set; }
        public XmlAnalysedRoads.AnalysedRoadsRoute AnalysedRoadList { get; set; }
        public XmlAnalysedStructures.AnalysedStructures AnalysedStructureList { get; set; }
        public XmlConstraints.AnalysedConstraints AnalysedConstraintList { get; set; }

        public RouteAnalysisObject()
        {
            AffectedPartyList = null;
            AnnotationList = null;
            AnalysedCautionList = null;
            AnalysedRoadList = null;
            AnalysedStructureList = null;
            AnalysedConstraintList = null;
        }
    }
    #endregion
    #region AffectedRoadDetail
    /// <summary>
    /// 
    /// </summary>
    public class AffectedRoadDetail
    {
        public string RoadName { get; set; }

        public long LinkId { get; set; }

        public long LinkNo { get; set; }

        public long Distance { get; set; }

        public long StartPointLink { get; set; }

        public long EndPointLink { get; set; }

        public long PoliceContactId { get; set; }

        public long PoliceOrgId { get; set; }

        public string PoliceForceName { get; set; }

        public long ManagerContactId { get; set; }

        public long ManagerOrgId { get; set; }

        public string ManagerName { get; set; }

        public long OnBehalOfContId { get; set; }

        public long OnBehalOfArrangId { get; set; }

        public long OnBehalOfOrgId { get; set; }

        public bool RetainNotification { get; set; }

        public bool WantFailure { get; set; }

        public string DelegOrgName { get; set; }

        public List<AffectedRoadResponsibleContact> AffectedRoadContactList { get; set; }

        public AffectedRoadDetail()
        {
            AffectedRoadContactList = new List<AffectedRoadResponsibleContact>();
        }
    }
    #endregion
    public class AffectedRoadResponsibleContact
    {
        public string RespOrgName { get; set; }

        public long RespContId { get; set; }

        public long RespOrgId { get; set; }

        public List<AffectedRoadResponsibleDelegation> AffectedRoadDelegation { get; set; }

        public AffectedRoadResponsibleContact()
        {
            AffectedRoadDelegation = new List<AffectedRoadResponsibleDelegation>();
        }
    }
    public class AffectedRoadResponsibleDelegation
    {
        public long RespOnBehalOfContId { get; set; }

        public long RespOnBehalOfArrangId { get; set; }

        public long RespOnBehalOfOrgId { get; set; }

        public bool RetainNotification { get; set; }

        public bool WantFailure { get; set; }

        public string DelegOrgName { get; set; }
    }
    public class RouteAssementAffectConstraint
    {
        public decimal ConstraintId { get; set; }
        public string ConstraintCode { get; set; }
    }
    #region  AffectedRoadPointDet
    /// <summary>
    /// 
    /// </summary>
    public class AffectedRoadPointDet
    {
        public string GridRef { get; set; }

        public string Description { get; set; }

        public long RoutePointType { get; set; }

        public sdogeometry TruePointGeom { get; set; }
    }
    #endregion

    public class LibraryNotes
    {
        public int LibraryNotesId { get; set; }
        public string Notes { get; set; }
        public long OrganisationId { get; set; }
        public long UserId { get; set; }

    }
}