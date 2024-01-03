using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace XmlAnalysedRoads
{
    #region new
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
    public partial class AnalysedRoadsRoute
    {
        /// <remarks/>
        [XmlElementAttribute("AnalysedRoadsPart")]
        public List<AnalysedRoadsPart> AnalysedRoadsPart
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedRoadsPart
    {
        /// <remarks/>
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("SubPart")]
        public List<SubPart> SubPart
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(ModeOfTransportType.road)]
        public ModeOfTransportType ModeOfTransport
        {
            get;
            set;
        }

        //public string ModeOfTransport
        //{
        //    get;
        //    set;
        //}

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long Id
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ComparisonId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsBroken
        {
            get;
            set;
        }
    }

    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public enum ModeOfTransportType
    {

        /// <remarks/>
        road,

        /// <remarks/>
        rail,

        /// <remarks/>
        sea,

        /// <remarks/>
        air,
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class SubPart
    {

        /// <remarks/>
        //[XmlElementAttribute("Path")] PathRoadsPathSegment[][]
        ////[XmlArrayItemAttribute("Path", IsNullable = false)]
        //public List<List<PathRoadsPathSegment>> Path { get; set; }

        //public SubPart()
        //{
        //}
        [XmlArrayItemAttribute("Path", IsNullable = false)]
        [XmlArrayItemAttribute("RoadsPathSegment", IsNullable = false, NestingLevel = 1)]

        public List<List<PathRoadsPathSegment>> Roads
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class PathRoadsPathSegment
    {
        /// <remarks/>
        [XmlElementAttribute("Road")]
        public Road Road
        {
            get;
            set;
        }

        [XmlElementAttribute("RoutePoint")]
        public RoutePointStructure RoutePoint
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class Road
    {
        /// <remarks/>
        public RoadIdentity RoadIdentity
        {
            get;
            set;
        }

        /// <remarks/> 
        public Distance Distance
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlArrayItemAttribute("RoadResponsibleParty", IsNullable = false)]
        public List<RoadResponsibleParty> RoadResponsibility
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Constraint", IsNullable = true)]
        public List<RoadConstraint> Constraints
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class RoadIdentity
    {
        public RoadIdentity()
        {
            this.Unidentified = false;
        }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string Number
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool Unidentified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class Distance
    {
        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class RoadResponsibleParty
    {


        /// <remarks/>
        public string OrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        public ResponsiblePartyOnBehalfOf OnBehalfOf
        {
            get;
            set;
        }

        /// <remarks/>
        public PartialResponsibility PartialResponsibility
        {
            get;
            set;
        }


        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int OrganisationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool RetainNotification
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool WantsFailureAlert
        {
            get;
            set;
        }

        public RoadResponsibleParty()
        {
            this.RetainNotification = false;
            this.WantsFailureAlert = false;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class ResponsiblePartyOnBehalfOf
    {

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long ContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long DelegationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long OrganisationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool RetainNotification
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool WantsFailureAlert
        {
            get;
            set;
        }


        /// <remarks/>
        public OnBehalfOf OnBehalfOf
        {
            get;
            set;
        }

        /// <remarks/>
        public string OrganisationName
        {
            get;
            set;
        }


        public ResponsiblePartyOnBehalfOf()
        {
            this.RetainNotification = false;
            this.WantsFailureAlert = false;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class OnBehalfOf
    {


        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int DelegationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int OrganisationId
        {
            get;
            set;
        }


        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool RetainNotification
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool WantsFailureAlert
        {
            get;
            set;
        }

        /// <remarks/>
        public string OrganisationName
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class PartialResponsibility
    {

        /// <remarks/>
        public EncounteredAt EncounteredAt
        {
            get;
            set;
        }

        /// <remarks/>
        public LastingFor LastingFor
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class EncounteredAt
    {

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class LastingFor
    {
        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class RoadConstraint
    {
        /// <remarks/>
        public string ECRN
        {
            get;
            set;
        }

        /// <remarks/>
        public string Type
        {
            get;
            set;
        }

        /// <remarks/>
        public RoadConstraintAppraisal Appraisal
        {
            get;
            set;
        }

        /// <remarks/>
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        public ConstraintEncounteredAt EncounteredAt
        {
            get;
            set;
        }

        /// <remarks/>
        public ConstraintLastingFor LastingFor
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int OwnerId
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class RoadConstraintAppraisal
    {
        /// <remarks/>
        public string Suitability
        {
            get;
            set;
        }

        /// <remarks/>
        public string ResultDetails
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class ConstraintEncounteredAt
    {
        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class ConstraintLastingFor
    {
        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value
        {
            get;
            set;
        }
    }
    #endregion


    [XmlTypeAttribute(TypeName = "RoutePointStructure", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class RoutePointStructure
    {

        public RoutePointStructure()
        {
            this.IsMTP = false;
            this.IsBroken = false;
        }

        /// <remarks/>
        public Point Point
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public RoutePointType PointType
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool IsMTP
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool IsBroken
        {
            get;
            set;
        }
    }

    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public enum RoutePointType
    {

        /// <remarks/>
        start,

        /// <remarks/>
        end,

        /// <remarks/>
        way,

        /// <remarks/>
        intermediate,
    }

    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class Point //: SimplifiedRoutePointStructure
    {


        public Point()
        {
            this.IsBroken = false;
        }

        /// <remarks/>
        public string Description
        {
            get;
            set;
        }

        /// <remarks/>
        public string GridRef
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/mtp")]
        public string MTPRN
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool IsBroken
        {
            get;
            set;
        }

        /// <remarks/>
        public SegmentPointStructure Position
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int Id
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool IdSpecified
        {
            get;
            set;
        }

    }

    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class SimplifiedRoutePointStructure
    {

        public SimplifiedRoutePointStructure()
        {
            this.IsBroken = false;
        }

        /// <remarks/>
        public string Description
        {
            get;
            set;
        }

        /// <remarks/>
        public string GridRef
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/mtp")]
        public string MTPRN
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool IsBroken
        {
            get;
            set;
        }
    }

    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class SegmentPointStructure
    {

        public SegmentPointStructure()
        {
            this.IsBroken = false;
        }

        /// <remarks/>
        public long RoadSectionID
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool RoadSectionIDSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "integer")]
        public string LinearReference
        {
            get;
            set;
        }

        /// <remarks/>
        public bool PositiveDirection
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool PositiveDirectionSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        public string GridRef
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [DefaultValueAttribute(false)]
        public bool IsBroken
        {
            get;
            set;
        }
    }
}