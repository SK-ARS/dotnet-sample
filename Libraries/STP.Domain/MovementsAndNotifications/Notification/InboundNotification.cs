using System.Xml.Serialization;
using System.Collections.Generic;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/notification", IsNullable = false)]
    public partial class InboundNotification
    {      
        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public ESDALReferenceNumber ESDALReferenceNumber { get; set; }

        /// <remarks/>
        public string Classification { get; set; }

        /// <remarks/>
        public HaulierDetails HaulierDetails { get; set; }

        /// <remarks/>
        public string HauliersReference { get; set; }

        /// <remarks/>
        public JourneyFromToSummary JourneyFromToSummary { get; set; }

        /// <remarks/>
        public JourneyFromTo JourneyFromTo { get; set; }

        /// <remarks/>
        public JourneyTiming JourneyTiming { get; set; }

        /// <remarks/>
        public LoadDetails LoadDetails { get; set; }

        ///<remarks/>
        public string NotificationNotesFromHaulier { get; set; }

        /// <remarks/>
        [XmlElementAttribute("Part")]
        public List<Part> Part { get; set; }

        [XmlElementAttribute("Dispensations")]
        public Dispensations Dispensation { get; set; }

        /// <remarks/>
        public bool IndemnityConfirmation { get; set; }

        /// <remarks/>
        public string OnBehalfOf { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long AnalysedRouteId { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int Id { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsA2B { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsRenotification { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsSimplified { get; set; }

        /// <remarks/>
        public string VSONotificationType { get; set; }

        public string DftReference { get; set; }
    }

    public partial class Dispensations
    {
        [XmlElementAttribute("InboundDispensation")]
        public List<InboundDispensation> InboundDispensation { get; set; }
    }

    public partial class InboundDispensation
    {
        [XmlAttributeAttribute()]
        public bool IsAdhoc { get; set; }

        [XmlAttributeAttribute()]
        public int GrantedById { get; set; }

        public string DRN { get; set; }

        public string Summary { get; set; }

        public string GrantedBy { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class ESDALReferenceNumber
    {

        /// <remarks/>
        public string Mnemonic { get; set; }

        /// <remarks/>
        public long MovementProjectNumber { get; set; }

        /// <remarks/>
        public MovementVersion MovementVersion { get; set; }

        /// <remarks/>
        public long NotificationNumber { get; set; }

        /// <remarks/>
        public long NotificationVersion { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    public partial class MovementVersion
    {

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool CreatedBySort { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public long Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class HaulierDetails
    {

        private string HaulierContactField;

        private string HaulierNameField;

        private HaulierAddress HaulierAddressField;

        private string TelephoneNumberField;

        private string FaxNumberField;

        private string EmailAddressField;

        private string LicenceField;

        private long OrganisationIdField;

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string HaulierContact { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string HaulierName { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public HaulierAddress HaulierAddress { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string TelephoneNumber { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string FaxNumber { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string EmailAddress { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string Licence { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long OrganisationId { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
    public partial class HaulierAddress
    {

        private string[] LineField;

        private string PostCodeField;

        private string CountryField;

        /// <remarks/>
        [XmlElementAttribute("Line", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string[] Line { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string PostCode { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string Country { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class JourneyFromToSummary
    {

        private string FromField;

        private string ToField;

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string From { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string To { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class JourneyFromTo
    {

        private string FromField;

        private string ToField;

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string From { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string To { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class JourneyTiming
    {

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string FirstMoveDate { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string LastMoveDate { get; set; }

        /// <remarks/>
        public string StartTime { get; set; }

        /// <remarks/>
        public string EndTime { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class LoadDetails
    {

        private string DescriptionField;

        private long TotalMovesField;

        private long MaxPiecesPerMoveField;

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public string Description { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public long TotalMoves { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public long MaxPiecesPerMove { get; set; }
    }


    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class Part
    {

        private PartRoute RouteField;

        private PartVehicleConfigurations VehicleConfigurationsField;

        /// <remarks/>
        public PartRoute Route { get; set; }

        /// <remarks/>
        public PartVehicleConfigurations VehicleConfigurations { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class PartRoute
    {

        private string NameField;

        private string DescriptionField;

        private string ModeOfTransportField;

        private StartPoints StartPointsField;

        private EndPoints EndPointsField;

        private PartTraversal PartTraversalField;

        private int ComparisonIdField;

        private string ConsistencyField;

        private int IdField;

        private bool IsSpatiallyCompleteField;

        private bool RequiresAttentionField;

        private string SourceField;

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public string Name { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public string Description { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public string ModeOfTransport { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public StartPoints StartPoints { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public EndPoints EndPoints { get; set; }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public PartTraversal PartTraversal { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ComparisonId { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Consistency { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int Id { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsSpatiallyComplete { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool RequiresAttention { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Source { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/route", IsNullable = false)]
    public partial class StartPoints
    {

        private StartPointsStartPoint StartPointField;

        /// <remarks/>
        public StartPointsStartPoint StartPoint { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class StartPointsStartPoint
    {

        private string DescriptionField;

        private string GridRefField;

        private StartPointsStartPointPosition PositionField;

        private int IdField;

        /// <remarks/>
        public string Description { get; set; }

        /// <remarks/>
        public string GridRef { get; set; }

        /// <remarks/>
        public StartPointsStartPointPosition Position { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int Id { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class StartPointsStartPointPosition
    {

        private long RoadSectionIDField;

        private long LinearReferenceField;

        private bool PositiveDirectionField;

        private string GridRefField;

        /// <remarks/>
        public long RoadSectionID { get; set; }

        /// <remarks/>
        public long LinearReference { get; set; }

        /// <remarks/>
        public bool PositiveDirection { get; set; }

        /// <remarks/>
        public string GridRef { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/route", IsNullable = false)]
    public partial class EndPoints
    {

        private EndPointsEndPoint EndPointField;

        /// <remarks/>
        public EndPointsEndPoint EndPoint { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class EndPointsEndPoint
    {

        private string DescriptionField;

        private string GridRefField;

        private EndPointsEndPointPosition PositionField;

        private int IdField;

        /// <remarks/>
        public string Description { get; set; }

        /// <remarks/>
        public string GridRef { get; set; }

        /// <remarks/>
        public EndPointsEndPointPosition Position { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int Id { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class EndPointsEndPointPosition
    {

        private long RoadSectionIDField;

        private long LinearReferenceField;

        private bool PositiveDirectionField;

        private string GridRefField;

        /// <remarks/>
        public long RoadSectionID { get; set; }

        /// <remarks/>
        public long LinearReference { get; set; }

        /// <remarks/>
        public bool PositiveDirection { get; set; }

        /// <remarks/>
        public string GridRef { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/route", IsNullable = false)]
    public partial class PartTraversal
    {

        private PartTraversalRoadPart RoadPartField;

        /// <remarks/>
        public PartTraversalRoadPart RoadPart { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPart
    {

        private PartTraversalRoadPartSubPart SubPartField;

        /// <remarks/>
        public PartTraversalRoadPartSubPart SubPart { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPart
    {

        private PartTraversalRoadPartSubPartPaths PathsField;

        /// <remarks/>
        public PartTraversalRoadPartSubPartPaths Paths { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPartPaths
    {

        private PartTraversalRoadPartSubPartPathsPath PathField;

        /// <remarks/>
        public PartTraversalRoadPartSubPartPathsPath Path { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPartPathsPath
    {

        private PartTraversalRoadPartSubPartPathsPathContiguousSegments ContiguousSegmentsField;

        /// <remarks/>
        public PartTraversalRoadPartSubPartPathsPathContiguousSegments ContiguousSegments { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPartPathsPathContiguousSegments
    {

        private PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegment ContiguousSegmentField;

        /// <remarks/>
        public PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegment ContiguousSegment { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegment
    {

        private PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentStartPoint StartPointField;

        private PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentEndPoint EndPointField;

        private PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentSegmentTraversal SegmentTraversalField;

        private string TypeField;

        /// <remarks/>
        public PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentStartPoint StartPoint { get; set; }

        /// <remarks/>
        public PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentEndPoint EndPoint { get; set; }

        /// <remarks/>
        public PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentSegmentTraversal SegmentTraversal { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Type { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentStartPoint
    {

        private long RoadSectionIDField;

        private long LinearReferenceField;

        private bool PositiveDirectionField;

        private string GridRefField;

        /// <remarks/>
        public long RoadSectionID { get; set; }

        /// <remarks/>
        public long LinearReference { get; set; }

        /// <remarks/>
        public bool PositiveDirection { get; set; }

        /// <remarks/>
        public string GridRef { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentEndPoint
    {

        private long RoadSectionIDField;

        private long LinearReferenceField;

        private bool PositiveDirectionField;

        private string GridRefField;

        /// <remarks/>
        public long RoadSectionID { get; set; }

        /// <remarks/>
        public long LinearReference { get; set; }

        /// <remarks/>
        public bool PositiveDirection { get; set; }

        /// <remarks/>
        public string GridRef { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class PartTraversalRoadPartSubPartPathsPathContiguousSegmentsContiguousSegmentSegmentTraversal
    {

        private string OnRoadSectionsField;

        /// <remarks/>
        public string OnRoadSections { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/notification")]
    public partial class PartVehicleConfigurations
    {

        private VehicleConfiguration VehicleConfigurationField;

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public VehicleConfiguration VehicleConfiguration { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
    public partial class VehicleConfiguration
    {

        private string NameField;

        private string DescriptionField;

        private string SummaryField;

        private string ConfigurationTypeField;

        private VehicleConfigurationConfigurationLength ConfigurationLengthField;

        private VehicleConfigurationRigidLength RigidLengthField;

        private VehicleConfigurationWidth WidthField;

        private VehicleConfigurationGrossWeight GrossWeightField;

        private VehicleConfigurationMaxHeight MaxHeightField;

        private VehicleConfigurationMaxAxleWeight MaxAxleWeightField;

        private VehicleConfigurationOverallWheelbase OverallWheelbaseField;

        private VehicleConfigurationConfigurationInstance ConfigurationInstanceField;

        private VehicleConfigurationConfigurationComponent[] ConfigurationComponentField;

        private long ConfigurationNoField;

        private int IdField;

        private string IntentField;

        /// <remarks/>
        public string Name { get; set; }

        /// <remarks/>
        public string Description { get; set; }

        /// <remarks/>
        public string Summary { get; set; }

        /// <remarks/>
        public string ConfigurationType { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationLength ConfigurationLength { get; set; }

        /// <remarks/>
        public VehicleConfigurationRigidLength RigidLength { get; set; }

        /// <remarks/>
        public VehicleConfigurationWidth Width { get; set; }

        /// <remarks/>
        public VehicleConfigurationGrossWeight GrossWeight { get; set; }

        /// <remarks/>
        public VehicleConfigurationMaxHeight MaxHeight { get; set; }

        /// <remarks/>
        public VehicleConfigurationMaxAxleWeight MaxAxleWeight { get; set; }

        /// <remarks/>
        public VehicleConfigurationOverallWheelbase OverallWheelbase { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationInstance ConfigurationInstance { get; set; }

        /// <remarks/>
        [XmlElementAttribute("ConfigurationComponent")]
        public VehicleConfigurationConfigurationComponent[] ConfigurationComponent { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long ConfigurationNo { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int Id { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Intent { get; set; }

        /// <remarks/>
        public VehicleConfigurationTravellingSpeed TravellingSpeed { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationLength
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationTravellingSpeed
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationRigidLength
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationWidth
    {

        private string unitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationGrossWeight
    {

        private string UnitField;

        private int ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public int Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationMaxHeight
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationMaxAxleWeight
    {

        private string UnitField;

        private long ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public long Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationOverallWheelbase
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationInstance
    {

        private string PlateNoField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string PlateNo { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponent
    {

        private VehicleConfigurationConfigurationComponentVehicleComponent VehicleComponentField;

        private string ComponentTypeField;

        private long LatitudeField;

        private long LongitudeField;

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponent VehicleComponent { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string ComponentType { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long Latitude { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long Longitude { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponent
    {

        private string InformalNameField;

        private string DescriptionField;

        private string SummaryField;

        private string ComponentSubTypeField;

        private string CouplingTypeField;

        private VehicleConfigurationConfigurationComponentVehicleComponentMaxHeight MaxHeightField;

        private VehicleConfigurationConfigurationComponentVehicleComponentRigidLength RigidLengthField;

        private VehicleConfigurationConfigurationComponentVehicleComponentWidth WidthField;

        private VehicleConfigurationConfigurationComponentVehicleComponentFrontOverhang FrontOverhangField;

        private VehicleConfigurationConfigurationComponentVehicleComponentRearOverhang RearOverhangField;

        private VehicleConfigurationConfigurationComponentVehicleComponentGroundClearance GroundClearanceField;

        private object ComponentInstanceField;

        private VehicleConfigurationConfigurationComponentVehicleComponentAxleSpacingToFollowingComponent AxleSpacingToFollowingComponentField;

        private VehicleConfigurationConfigurationComponentVehicleComponentAxleDetails AxleDetailsField;

        private int ComponentNoField;

        /// <remarks/>
        public string InformalName { get; set; }

        /// <remarks/>
        public string Description { get; set; }

        /// <remarks/>
        public string Summary { get; set; }

        /// <remarks/>
        public string ComponentSubType { get; set; }

        /// <remarks/>
        public string CouplingType { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentMaxHeight MaxHeight { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentRigidLength RigidLength { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentWidth Width { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentFrontOverhang FrontOverhang { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentRearOverhang RearOverhang { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentGroundClearance GroundClearance { get; set; }

        /// <remarks/>
        public object ComponentInstance { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentAxleSpacingToFollowingComponent AxleSpacingToFollowingComponent { get; set; }

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentAxleDetails AxleDetails { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ComponentNo { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentMaxHeight
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentRigidLength
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentWidth
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentFrontOverhang
    {

        private string UnitField;

        private long ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public long Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentRearOverhang
    {

        private string UnitField;

        private long ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public long Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentGroundClearance
    {

        private string UnitField;

        private long ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public long Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentAxleSpacingToFollowingComponent
    {

        private string UnitField;

        private decimal ValueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Unit { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public decimal Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentAxleDetails
    {

        private VehicleConfigurationConfigurationComponentVehicleComponentAxleDetailsAxleList AxleListField;

        /// <remarks/>
        public VehicleConfigurationConfigurationComponentVehicleComponentAxleDetailsAxleList AxleList { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentAxleDetailsAxleList
    {

        private VehicleConfigurationConfigurationComponentVehicleComponentAxleDetailsAxleListAxle[] AxleField;

        private string AxleSpacingUnitField;

        private string AxleWeightUnitField;

        private long NumberOfAxlesField;

        private string WheelSpacingUnitField;

        /// <remarks/>
        [XmlElementAttribute("Axle")]
        public VehicleConfigurationConfigurationComponentVehicleComponentAxleDetailsAxleListAxle[] Axle { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string AxleSpacingUnit { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string AxleWeightUnit { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long NumberOfAxles { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string WheelSpacingUnit { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class VehicleConfigurationConfigurationComponentVehicleComponentAxleDetailsAxleListAxle
    {

        private long AxleCountField;

        private long AxleNumberField;

        private long AxleWeightField;

        private decimal DistanceToNextAxleField;

        private bool DistanceToNextAxleFieldSpecified;

        private long NumberOfWheelsField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int AxleCount { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int AxleNumber { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public long AxleWeight { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public decimal DistanceToNextAxle { get; set; }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DistanceToNextAxleSpecified { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int NumberOfWheels { get; set; }
    }

    /// <remarks/>


    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/notification")]
    public enum VSONotificationTypeType
    {

        /// <remarks/>
        soa,

        /// <remarks/>
        police,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("soa and police")]
        soaandpolice,
    }

    /// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
    public partial class AxleStructure
    {

        private string TyreSizeField;

        private string WheelSpacingField;

        private decimal AxleWeightField;

        private bool AxleWeightFieldSpecified;

        private string NumberOfWheelsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
        public string TyreSize { get; set; }

        /// <remarks/>
        public string WheelSpacing { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal AxleWeight { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AxleWeightSpecified { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public int NumberOfWheels { get; set; }
    }
}