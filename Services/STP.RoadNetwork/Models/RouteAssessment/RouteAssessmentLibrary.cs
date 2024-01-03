using NetSdoGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace STP.RoadNetwork.Models.RouteAssessment
{
    #region Annotations Class
    /// <summary>
    /// Annotations Class for route annotations
    /// </summary>
    
    public class Annotations
    {
        
        public long annotId { get; set; }

        
        public string annotType { get; set; }

        
        public long annotLinkId { get; set; }

        
        public string annotText { get; set; }

        
        //public sdogeometry annotGeom { get; set; }

        
        public List<RouteConstraints> constraintList { get; set; }

        
        public RouteConstraints constDetails { get; set; }

        public Annotations()
        {
            constraintList = null;
            constDetails = null;
        }
    }
    #endregion

    #region DateRange
    /// <summary>
    /// Class to get and set Start Date and End Date
    /// </summary>
    
    public class DateRange
    {
        
        public DateTime startDate { get; set; }

        
        public DateTime endDate { get; set; }
    }
    #endregion

    #region ConstraintValues Class
    /// <summary>
    /// Class to store constraint specific values.
    /// </summary>
    
    public class ConstraintValues
    {
        
        public long grossWeight { get; set; }

        
        public long axleWeight { get; set; }

        
        public Single maxHeight { get; set; }

        
        public Single maxLength { get; set; }

        
        public Single maxWidth { get; set; }

        
        public float minSpeed { get; set; }

        public ConstraintValues()
        {
            grossWeight = 0;
            axleWeight = 0;
            maxHeight = 0;
            maxHeight = 0;
            maxLength = 0;
            maxWidth = 0;
        }

    }
    #endregion

    #region ConstraintReferencesParam Class
    public class ConstraintReferencesParam
    {
        public List<ConstraintReferences> constRefrences { get; set; }
        public int organisationId { get; set; }
        public bool allLinks { get; set; }
        public long CONSTRAINT_ID { get; set; }
    }
    #endregion

    #region ConstraintReferences Class
    /// <summary>
    /// 
    /// </summary>

    public class ConstraintReferences
    {

        
        public long constLink { get; set; }

        
        public long toNorthing { get; set; }

        
        public long toEasting { get; set; }

        
        public long fromNorthing { get; set; }

        
        public long fromEasting { get; set; }

        
        public long linearRef { get; set; }

        
        public long? toLinearRef { get; set; }

        
        public long? fromLinearRef { get; set; }

        
        public long easting { get; set; }

        
        public long northing { get; set; }

        
        
        public bool isPoint { get; set; }

        
        public int? direction { get; set; }

        
        public int? conLinkNo { get; set; }
    }
    #endregion

    #region RouteConstraints Class
    /// <summary>
    /// Constraints class to set and get constraints
    /// </summary>
    
    public class RouteConstraints
    {
        
        public long constId { get; set; }

        
        public string constType { get; set; }

        
        public long constTypeCode { get; set; }

        
        public string topologyType { get; set; }

        
        public long topologyTypeCode { get; set; }

        
        public string traversalType { get; set; }

        
        public long traversalTypeCode { get; set; }

        
        public sdogeometry constGeom { get; set; }

        
        public DateRange date { get; set; }

        
        public List<RouteCautions> cautionList { get; set; }
        
        
        public string consName { get; set; }

        
        public string constCode { get; set; }

        
        public List<ConstraintReferences> constRefrences { get; set; }

        
        public ConstraintValues constraintValue { get; set; }

        
        public List<AssessmentContacts> constrContact { get; set; }

        public RouteConstraints()
        {
            date = null;
            cautionList = null;
            constraintValue = null;
            constRefrences = null;
            constrContact = null;
        }

        
        public string constSuitability { get; set; }
    }
    #endregion

    #region RouteCautions Class
    /// <summary>
    /// Cautions class
    /// </summary>
    
    public class RouteCautions
    {
        
        public int orgId { get; set; }

        
        public long cautId { get; set; }

        
        public long cautConstId { get; set; }

        
        public string cautName { get; set; }
        
        
        public string cautDescription { get; set; }

        
        public List<AssessmentContacts> cautContactList { get; set; }

        
        public ConstraintValues cautConstValue { get; set; }

        public RouteCautions()
        {
            cautContactList = null;
            cautConstValue = null;
        }

        
        public long ownerOrgId { get; set; }

        
        public string cautConstrName { get; set; }

        
        public string cautConstrCode { get; set; }

        
        public string cautConstrType { get; set; }

        
        public long cautStructId { get; set; }

        
        public string roadName { get; set; }

        
        public string suitability { get; set; }

        
        ///member which determines the type of caution 0 : road caution, 1 : structure caution
        public int cautType { get; set; }
    }
    #endregion

    #region CautionContacts Class
    /// <summary>
    /// 
    /// </summary>
    
    public class AssessmentContacts
    {

        
        public long contactId { get; set; }

        
        public string contactName { get; set; }

        
        public long orgId { get; set; }

        
        public string orgName { get; set; }

        
        public string country { get; set; }

        
        public List<string> addressLine { get; set; }

        
        public string postCode { get; set; }

        
        public string fax { get; set; }

        
        public string email { get; set; }

        
        public string mobile { get; set; }

        
        public int roleTypeCode { get; set; }

         //Enumerated Role Type Object
        public XmlAnalysedCautions.RoleType roleName { get; set; }

        
        public string descr { get; set; }

        
        public object isAdHoc { get; set; }

        
        public object countryCode { get; set; }

        
        public object telephone { get; set; }

        
        public object extension { get; set; }

        
        public XmlAffectedParties.AffectedPartyReasonType affectedReasonType { get; set; }

        
        public XmlAffectedParties.DispensationStatusType DispensationStatus { get; set; }

        
        public string dispensationStatusType { get; set; }

        
        public XmlAffectedParties.AffectedPartyReasonExclusionOutcomeType affectedExclusionOutcome { get; set; }

        
        public long totalRecords { get; set; }

        
        public string orgType { get; set; }

        
        public List<long> dispensationId { get; set; }

        
        public int userAddedAffectedContact { get; set; }

        
        public long ownerOrgId { get; set; }

        
        public long ownerArrangementId { get; set; }

        
        public string ownerOrgName { get; set; }

        
        public bool retainNotification { get; set; }

        
        public bool recieveFailures { get; set; }

        public AssessmentContacts()
        {
            DispensationStatus = XmlAffectedParties.DispensationStatusType.nonematching;

            affectedReasonType = XmlAffectedParties.AffectedPartyReasonType.newlyaffected;

            dispensationStatusType = "None Matching";

            affectedExclusionOutcome = XmlAffectedParties.AffectedPartyReasonExclusionOutcomeType.newlyaffected;
        } 
    }
    #endregion

    public class NenAnalysedData
    {
        public NenAnalysedData(int inboxId, int organisationId, int notificationId, string schema)
        {
            this.inboxId = inboxId;
            this.organisationId = organisationId;
            this.notificationId = notificationId;
            this.schema = schema;
        }
        public int inboxId { get; set; }
        public int organisationId { get; set; }
        public int notificationId { get; set; }
        public int analysisType { get; set; }
        public string schema { get; set; }

        public string getAnalysisType()
        {
            if (analysisType.Equals(1)) { return "Driving instruction and route description"; }
            else if (analysisType.Equals(2)) { return "--"; }
            else if (analysisType.Equals(3)) { return "Affected structures"; }
            else if (analysisType.Equals(4)) { return "Cautions"; }
            else if (analysisType.Equals(5)) { return "Constraints"; }
            else if (analysisType.Equals(6)) { return "--"; }
            else if (analysisType.Equals(8)) { return "Affected roads"; }
            else { return ""; }
        }
    }

    #region RouteAssessmentModel
    public class RouteAssessmentModel
    {
        public long analysisId { get; set; }
        public byte[] drive_inst { get; set; }
        public byte[] cautions { get; set; }
        public byte[] routeDescr { get; set; }
        public byte[] affectedstruct { get; set; }
        public byte[] annotation { get; set; }
        public byte[] constraints { get; set; }
        //public StringBuilder instruction { get; set; } 

        public byte[] affectedParties { get; set; }

        public byte[] affectedRoads { get; set; }
    }
    #endregion

    #region  To get notes to the haulier
    public class GetAuthNotesToHaulier
    {
        public byte[] Auth_Notes_To_HA { get; set; }
    }
    #endregion

    #region IcaCalculator
    /// <summary>
    /// 
    /// </summary>
    
    public class IcaCalculator
    {
        
        public double wgtLowerDef { get; set; }

        
        public double wgtUpperDef { get; set; }

        
        public double svLowerDef { get; set; }

        
        public double svUpperDef { get; set; }


        public IcaCalculator()
        { }
    }
    #endregion

    #region RouteVehicles
    /// <summary>
    /// RouteVehicle Related class
    /// </summary>
    public class RouteVehicles
    {
        
        public long vehicleId { get; set; }

        
        public string vehicleName { get; set; }

    }
    #endregion

    #region RouteAnalysisXml
    /// <summary>
    /// Class to include xml string related variables
    /// </summary>
    public class RouteAnalysisXml
    {
        
        public string xmlAnalysedStructure { get; set; }

        
        public string xmlAnalysedConstraints { get; set; }

        
        public string xmlAnalysedCautions { get; set; }

        
        public string xmlAnalysedAnnotations { get; set; }

        public RouteAnalysisXml()
        { }

        
        public string xmlAffectedParties { get; set; }

        
        public string xmlAffectedRoads { get; set; }
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
        public string contentRefNo { get; set; }

        public long analysisId { get; set; }

        /// <summary>
        /// Candidate Route of SORT linked through revision_id. This is used only in SORT schema
        /// </summary>
        public long revisionId { get; set; }

        /// <summary>
        /// New change for showing Application planned routes linking through APP_REVISION_ID
        /// </summary>
        public long appRevisionId { get; set; }

        /// <summary>
        /// Agreed Route of SO and VR1 Application linked through movement version
        /// </summary>
        public long versionId { get; set; }

        /// <summary>
        /// variable to check whether the route is a condidate route or not 
        /// </summary>
        public bool isCanditateRoute { get; set; }

        /// <summary>
        /// organisation id for which the assessment is carried out
        /// </summary>
        public long orgId { get; set; }

        public RouteAssessmentInputs()
        {

        }

    }
    #endregion

    #region AnalysedRoute
    /// <summary>
    /// class to store route analysis related xml's blob and analyisis Id
    /// </summary>
    public class AnalysedRoute
    {
        
        public string routeAnalysisXml { get; set; }

        
        public byte[] routeAnalysisBlob { get; set; }

        
        public long prevAnalysisId { get; set; }

        
        public long newAnalysisId { get; set; }

        
        public analysisType analysisType { get; set; }

        public AnalysedRoute()
        {
            prevAnalysisId = 0;
            routeAnalysisXml = null;
            routeAnalysisBlob = null;
        }
    }
    #endregion

    #region WayTextRoute
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("Text", Namespace = "route:http://www.esdal.com/schemas/core/route")]
    public class WayTextRoute
    {
        public string Value { get; set; }

        public WayTextRoute()
        { }
    }
    #endregion

    #region enum class

    #region analysisType
    /// <summary>
    /// 
    /// </summary>
    public enum analysisType
    {

        /// <remarks/>
        structure,

        /// <remarks/>
        drivinginstructions,

        /// <remarks/>
        affectedparties,

        /// <remarks/>
        resolvedparties,

        /// <remarks/>
        routeanalysis,

        /// <remarks/>
        affectedconstraints,

        /// <remarks/>
        annotations,

        /// <remarks/>
        cautions,

        /// <remarks/>
        affectedroads,

    }
    #endregion

    #endregion

    #region AffectedRoadDetail
    /// <summary>
    /// 
    /// </summary>
    public class AffectedRoadDetail
    {
        public string roadName { get; set; }

        public long linkId { get; set; }

        public long linkNo { get; set; }

        public long distance { get; set; }

        public long startPointLink { get; set; }

        public long endPointLink { get; set; }

        public long policeContactId { get; set; }

        public long policeOrgId { get; set; }

        public string policeForceName { get; set; }

        public long managerContactId { get; set; }

        public long managerOrgId { get; set; }

        public string managerName { get; set; }

        public long onBehalOfContId { get; set; }

        public long onBehalOfArrangId { get; set; }

        public long onBehalOfOrgId { get; set; }

        public bool retainNotif { get; set; }

        public bool wantFailure { get; set; }

        public string delegOrgName { get; set; }

        public List<AffectedRoadResponsibleContact> affectedRoadContactList { get; set; }

        public AffectedRoadDetail()
        {
            affectedRoadContactList = new List<AffectedRoadResponsibleContact>();
        }
    }
    #endregion

    public class AffectedRoadResponsibleContact
    {
        public string RespOrgName { get; set; }

        public long RespContId { get; set; }

        public long RespOrgId { get; set; }

        public List<AffectedRoadResponsibleDelegation> affectedRoadDelegation { get; set; }

        public AffectedRoadResponsibleContact()
        {
            affectedRoadDelegation = new List<AffectedRoadResponsibleDelegation>();
        }
    }

    public class AffectedRoadResponsibleDelegation
    {
        public long RespOnBehalOfContId { get; set; }

        public long RespOnBehalOfArrangId { get; set; }

        public long RespOnBehalOfOrgId { get; set; }

        public bool retainNotif { get; set; }

        public bool wantFailure { get; set; }

        public string delegOrgName { get; set; }
    }

    #region  AffectedRoadPointDet
    /// <summary>
    /// 
    /// </summary>
    public class AffectedRoadPointDet
    {
        public string gridRef { get; set; }

        public string description { get; set; }

        public long routePointType { get; set; }

        //public sdogeometry truePointGeom { get; set; }
    }
    #endregion

    #region AffectedPartyContactDetail
    public class AffectedPartyContactDetail
    {
        public AffectedPartyContactDetail()
        {
            AffectedReason = new XmlAffectedParties.AffectedPartyReasonType();
            fullName = null;
            orgName = null;
            faxNumber = null;
            email = null;
            commnMethodCode = 695003;
            delegated = false;
        }

        public XmlAffectedParties.AffectedPartyReasonType AffectedReason { get; set; }

        public bool delegated { get; set; }

        public string fullName { get; set; }

        public string orgName { get; set; }

        public string faxNumber { get; set; }

        public string email { get; set; }

        public UserType user { get; set; }

        public long commnMethodCode { get; set; }
    }

    #endregion

    #region enum UserType
    public enum UserType
    {
        police,

        soa,

        ha,
    }
    #endregion

    #region RouteAnalysisObject
    public class RouteAnalysisObject
    {
        public XmlAffectedParties.AffectedPartiesStructure affectedPartyListObj { get; set; }
        public XmlAnalysedAnnotations.AnalysedAnnotations annotationListObj { get; set; }
        public XmlAnalysedCautions.AnalysedCautions analysedCautionListObj { get; set; }
        public XmlAnalysedRoads.AnalysedRoadsRoute analysedRoadListObj { get; set; }
        public XmlAnalysedStructures.AnalysedStructures analysedStructureListObj { get; set; }
        public XmlConstraints.AnalysedConstraints analysedConstraintListObj { get; set; }
        
        public RouteAnalysisObject()
        {
            affectedPartyListObj = null;
            annotationListObj = null;
            analysedCautionListObj = null;
            analysedRoadListObj = null;
            analysedStructureListObj = null;
            analysedConstraintListObj = null;
        }
    }
    #endregion
}