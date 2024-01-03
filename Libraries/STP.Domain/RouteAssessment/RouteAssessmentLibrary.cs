using NetSdoGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace STP.Domain.RouteAssessment
{
    #region Annotations Class
    /// <summary>
    /// Annotations Class for route annotations
    /// </summary>
    
    public class Annotations
    {
        
        public long AnnotationId { get; set; }

        
        public string AnnotationType { get; set; }

        
        public long AnnotationLinkId { get; set; }

        
        public string AnnotationText { get; set; }

        
        public List<RouteConstraints> ConstraintList { get; set; }

        
        public RouteConstraints ConstraintDetails { get; set; }

        public Annotations()
        {
            ConstraintList = null;
            ConstraintDetails = null;
        }
    }
    #endregion

    #region ConstraintReferencesParam Class
    public class ConstraintReferencesParam
    {
        public List<ConstraintReferences> ConstraintRefrences { get; set; }
        public int OrganisationId { get; set; }
        public bool AllLinks { get; set; }
        public long ConstraintId { get; set; }
    }
    #endregion
   
    public class NenAnalysedData
    {
        public NenAnalysedData(int inboxId, int organisationId, int notificationId, string schema)
        {
            this.InboxId = inboxId;
            this.OrganisationId = organisationId;
            this.NotificationId = notificationId;
            this.Schema = schema;
        }
        public int InboxId { get; set; }
        public int OrganisationId { get; set; }
        public int NotificationId { get; set; }
        public int AnalysisType { get; set; }
        public string Schema { get; set; }

        public string getAnalysisType()
        {
            if (AnalysisType.Equals(1)) { return "Driving instruction and route description"; }
            else if (AnalysisType.Equals(2)) { return "--"; }
            else if (AnalysisType.Equals(3)) { return "Affected structures"; }
            else if (AnalysisType.Equals(4)) { return "Cautions"; }
            else if (AnalysisType.Equals(5)) { return "Constraints"; }
            else if (AnalysisType.Equals(6)) { return "--"; }
            else if (AnalysisType.Equals(8)) { return "Affected roads"; }
            else { return ""; }
        }
    }

    #region  To get notes to the haulier
    public class GetAuthNotesToHaulier
    {
        public byte[] AuthenticationNotesToHaulier { get; set; }
    }
    #endregion

    #region IcaCalculator
    /// <summary>
    /// 
    /// </summary>
    
    public class IcaCalculator
    {
        
        public double WeightLowerDef { get; set; }

        
        public double WeightUpperDef { get; set; }

        
        public double SvLowerDef { get; set; }

        
        public double SvUpperDef { get; set; }


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
        
        public long VehicleId { get; set; }

        
        public string VehicleName { get; set; }

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

    #region AffectedPartyContactDetail
    public class AffectedPartyContactDetail
    {
        public AffectedPartyContactDetail()
        {
            AffectedReason = new XmlAffectedParties.AffectedPartyReasonType();
            FullName = null;
            OrganisationName = null;
            FaxNumber = null;
            Email = null;
            CommonMethodCode = 695003;
            Delegated = false;
        }

        public XmlAffectedParties.AffectedPartyReasonType AffectedReason { get; set; }

        public bool Delegated { get; set; }

        public string FullName { get; set; }

        public string OrganisationName { get; set; }

        public string FaxNumber { get; set; }

        public string Email { get; set; }

        public UserType User { get; set; }

        public long CommonMethodCode { get; set; }
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

}