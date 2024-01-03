using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace STP.Domain.Applications
{

	[XmlRoot(ElementName = "ESDALReferenceNumber", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class ESDALReferenceNumber1
	{
		[XmlElement(ElementName = "Mnemonic", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Mnemonic { get; set; }
		[XmlElement(ElementName = "MovementProjectNumber", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string MovementProjectNumber { get; set; }
		[XmlElement(ElementName = "MovementVersion", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string MovementVersion { get; set; }
		[XmlElement(ElementName = "NotificationNumber", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string NotificationNumber { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	public class HaulierContact
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class HaulierName
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class Line
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class PostCode
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}
	public class Country1
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}


	public class HaulierAddress
	{
		public List<Line> Line { get; set; }
		public PostCode PostCode { get; set; }
		public Country1 Country { get; set; }
		public string Xmlns { get; set; }
	}

	public class TelephoneNumber
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class FaxNumber
	{
		public string Xmlns { get; set; }
	}

	public class EmailAddress
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class Licence
	{
		public string Xmlns { get; set; }
	}

	public class HaulierDetails1
	{
		public HaulierContact HaulierContact { get; set; }
		public HaulierName HaulierName { get; set; }
		public HaulierAddress HaulierAddress { get; set; }
		public TelephoneNumber TelephoneNumber { get; set; }
		public FaxNumber FaxNumber { get; set; }
		public EmailAddress EmailAddress { get; set; }
		public Licence Licence { get; set; }
		public string OrganisationId { get; set; }
	}

	public class From
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class To
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class JourneyFromToSummary1
	{
		public From From { get; set; }
		public To To { get; set; }
	}

	public class JourneyFromTo1
	{
		public From From { get; set; }
		public To To { get; set; }
	}

	public class FirstMoveDate
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class LastMoveDate
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class JourneyTiming1
	{
		public FirstMoveDate FirstMoveDate { get; set; }
		public LastMoveDate LastMoveDate { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
	}

	public class Description
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class TotalMoves
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class MaxPiecesPerMove
	{
		public string Xmlns { get; set; }
		public string Text { get; set; }
	}

	public class LoadDetails1
	{
		public Description Description { get; set; }
		public TotalMoves TotalMoves { get; set; }
		public MaxPiecesPerMove MaxPiecesPerMove { get; set; }
	}

	public class Scottish
	{
		public string Xmlns { get; set; }
	}

	public class Numbers
	{
		public Scottish Scottish { get; set; }
	}


	public class VR1Information
	{
		[XmlElement(ElementName = "Status", Namespace = "http://www.esdal.com/schemas/core/notification")]
		public string Status { get; set; }
		[XmlElement(ElementName = "Numbers", Namespace = "http://www.esdal.com/schemas/core/notification")]
		public Numbers Numbers { get; set; }
	}

	[XmlRoot(ElementName = "Contact", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Contact
	{
		[XmlElement(ElementName = "ContactName", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string ContactName { get; set; }
		[XmlElement(ElementName = "OrganisationName", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string OrganisationName { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Email { get; set; }
		[XmlAttribute(AttributeName = "ContactId")]
		public string ContactId { get; set; }
		[XmlAttribute(AttributeName = "OrganisationId")]
		public string OrganisationId { get; set; }
		[XmlAttribute(AttributeName = "Reason")]
		public string Reason { get; set; }
		[XmlAttribute(AttributeName = "IsRecipient")]
		public string IsRecipient { get; set; }
		[XmlAttribute(AttributeName = "IsPolice")]
		public string IsPolice { get; set; }
		[XmlAttribute(AttributeName = "IsHaulier")]
		public string IsHaulier { get; set; }
		[XmlAttribute(AttributeName = "IsRetainedNotificationOnly")]
		public string IsRetainedNotificationOnly { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "Recipients", Namespace = "http://www.esdal.com/schemas/core/notification")]
	public class Recipients1
	{
		[XmlElement(ElementName = "Contact", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public List<Contact> Contact { get; set; }
	}

	[XmlRoot(ElementName = "Description", Namespace = "http://www.esdal.com/schemas/core/route")]
	public class Description2
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "GridRef", Namespace = "http://www.esdal.com/schemas/core/route")]
	public class GridRef
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "StartPoint", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class StartPoint
	{
		[XmlElement(ElementName = "Description", Namespace = "http://www.esdal.com/schemas/core/route")]
		public Description2 Description2 { get; set; }
		[XmlElement(ElementName = "GridRef", Namespace = "http://www.esdal.com/schemas/core/route")]
		public GridRef GridRef { get; set; }
	}

	[XmlRoot(ElementName = "StartPointListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class StartPointListPosition
	{
		[XmlElement(ElementName = "StartPoint", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public StartPoint StartPoint { get; set; }
	}

	[XmlRoot(ElementName = "EndPoint", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class EndPoint
	{
		[XmlElement(ElementName = "Description", Namespace = "http://www.esdal.com/schemas/core/route")]
		public Description2 Description2 { get; set; }
		[XmlElement(ElementName = "GridRef", Namespace = "http://www.esdal.com/schemas/core/route")]
		public GridRef GridRef { get; set; }
	}

	[XmlRoot(ElementName = "EndPointListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class EndPointListPosition
	{
		[XmlElement(ElementName = "EndPoint", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public EndPoint EndPoint { get; set; }
	}

	[XmlRoot(ElementName = "Metric", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Metric
	{
		[XmlElement(ElementName = "Distance", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Distance { get; set; }
	}

	[XmlRoot(ElementName = "Imperial", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Imperial
	{
		[XmlElement(ElementName = "Distance", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Distance { get; set; }
	}

	[XmlRoot(ElementName = "Distance", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Distance
	{
		[XmlElement(ElementName = "Metric", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Metric Metric { get; set; }
		[XmlElement(ElementName = "Imperial", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Imperial Imperial { get; set; }
	}

	[XmlRoot(ElementName = "ConfigurationSummaryListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class ConfigurationSummaryListPosition1
	{
		[XmlElement(ElementName = "ConfigurationSummary", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string ConfigurationSummary { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "OverallLength", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class OverallLength
	{
		[XmlElement(ElementName = "IncludingProjections", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string IncludingProjections { get; set; }
	}

	[XmlRoot(ElementName = "OverallLengthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class OverallLengthListPosition1
	{
		[XmlElement(ElementName = "OverallLength", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public OverallLength OverallLength { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "RigidLengthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class RigidLengthListPosition
	{
		[XmlElement(ElementName = "RigidLength", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string RigidLength { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "RearOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class RearOverhangListPosition1
	{
		[XmlElement(ElementName = "RearOverhang", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string RearOverhang { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "FrontOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class FrontOverhangListPosition1
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "LeftOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class LeftOverhangListPosition
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "RightOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class RightOverhangListPosition
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "OverallWidthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class OverallWidthListPosition1
	{
		[XmlElement(ElementName = "OverallWidth", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string OverallWidth { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "OverallHeight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class OverallHeight
	{
		[XmlElement(ElementName = "MaxHeight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string MaxHeight { get; set; }
	}

	[XmlRoot(ElementName = "OverallHeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class OverallHeightListPosition1
	{
		[XmlElement(ElementName = "OverallHeight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public OverallHeight OverallHeight { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "GrossWeight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class GrossWeight
	{
		[XmlElement(ElementName = "Weight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string Weight { get; set; }
	}

	[XmlRoot(ElementName = "GrossWeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class GrossWeightListPosition1
	{
		[XmlElement(ElementName = "GrossWeight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public GrossWeight GrossWeight { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "MaximumAxleWeight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class MaximumAxleWeight
	{
		[XmlElement(ElementName = "Weight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string Weight { get; set; }
	}

	[XmlRoot(ElementName = "MaxAxleWeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class MaxAxleWeightListPosition1
	{
		[XmlElement(ElementName = "MaximumAxleWeight", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public MaximumAxleWeight MaximumAxleWeight { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "VehicleSummary", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class VehicleSummary
	{
		[XmlElement(ElementName = "WeightConformance", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string WeightConformance { get; set; }
		[XmlElement(ElementName = "Configuration", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string Configuration { get; set; }
		[XmlElement(ElementName = "ConfigurationType", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public string ConfigurationType { get; set; }
	}

	[XmlRoot(ElementName = "VehicleSummaryListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
	public class VehicleSummaryListPosition1
	{
		[XmlElement(ElementName = "VehicleSummary", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public VehicleSummary VehicleSummary { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "Vehicles", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Vehicles
	{
		[XmlElement(ElementName = "ConfigurationSummaryListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public ConfigurationSummaryListPosition ConfigurationSummaryListPosition { get; set; }
		[XmlElement(ElementName = "OverallLengthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public OverallLengthListPosition OverallLengthListPosition { get; set; }
		[XmlElement(ElementName = "RigidLengthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public RigidLengthListPosition RigidLengthListPosition { get; set; }
		[XmlElement(ElementName = "RearOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public RearOverhangListPosition RearOverhangListPosition { get; set; }
		[XmlElement(ElementName = "FrontOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public FrontOverhangListPosition FrontOverhangListPosition { get; set; }
		[XmlElement(ElementName = "LeftOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public LeftOverhangListPosition LeftOverhangListPosition { get; set; }
		[XmlElement(ElementName = "RightOverhangListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public RightOverhangListPosition RightOverhangListPosition { get; set; }
		[XmlElement(ElementName = "OverallWidthListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public OverallWidthListPosition OverallWidthListPosition { get; set; }
		[XmlElement(ElementName = "OverallHeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public OverallHeightListPosition OverallHeightListPosition { get; set; }
		[XmlElement(ElementName = "GrossWeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public GrossWeightListPosition GrossWeightListPosition { get; set; }
		[XmlElement(ElementName = "MaxAxleWeightListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public MaxAxleWeightListPosition MaxAxleWeightListPosition { get; set; }
		[XmlElement(ElementName = "VehicleSummaryListPosition", Namespace = "http://www.esdal.com/schemas/core/vehicle")]
		public VehicleSummaryListPosition VehicleSummaryListPosition { get; set; }
	}

	[XmlRoot(ElementName = "Name", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
	public class Name
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "RoadIdentity", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RoadIdentity
	{
		[XmlElement(ElementName = "Name", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
		public Name Name { get; set; }
	}

	[XmlRoot(ElementName = "RoadTraversal", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RoadTraversal
	{
		[XmlElement(ElementName = "RoadIdentity", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public RoadIdentity RoadIdentity { get; set; }
		[XmlElement(ElementName = "Distance", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Distance Distance { get; set; }
	}

	[XmlRoot(ElementName = "RoadTraversalListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RoadTraversalListPosition
	{
		[XmlElement(ElementName = "RoadTraversal", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public RoadTraversal RoadTraversal { get; set; }
	}

	[XmlRoot(ElementName = "Path", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Path1
	{
		[XmlElement(ElementName = "RoadTraversalListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public List<RoadTraversalListPosition> RoadTraversalListPosition { get; set; }
	}

	[XmlRoot(ElementName = "PathListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class PathListPosition
	{
		[XmlElement(ElementName = "Path", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Path1 Path { get; set; }
	}

	[XmlRoot(ElementName = "RouteSubPart", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RouteSubPart
	{
		[XmlElement(ElementName = "PathListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public PathListPosition PathListPosition { get; set; }
	}

	[XmlRoot(ElementName = "RouteSubPartListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RouteSubPartListPosition
	{
		[XmlElement(ElementName = "RouteSubPart", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public RouteSubPart RouteSubPart { get; set; }
	}

	[XmlRoot(ElementName = "Roads", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Roads
	{
		[XmlElement(ElementName = "RouteSubPartListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public RouteSubPartListPosition RouteSubPartListPosition { get; set; }
	}

	[XmlRoot(ElementName = "Suitability", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
	public class Suitability
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Organisation", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
	public class Organisation
	{
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Appraisal", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Appraisal
	{
		[XmlElement(ElementName = "Suitability", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
		public Suitability Suitability { get; set; }
		[XmlElement(ElementName = "Organisation", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
		public Organisation Organisation { get; set; }
	}

	[XmlRoot(ElementName = "Structure", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Structure
	{
		[XmlElement(ElementName = "ESRN", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string ESRN { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Name2 { get; set; }
		[XmlElement(ElementName = "Appraisal", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Appraisal Appraisal { get; set; }
		[XmlAttribute(AttributeName = "IsMyResponsibility")]
		public string IsMyResponsibility { get; set; }
		[XmlAttribute(AttributeName = "TraversalType")]
		public string TraversalType { get; set; }
	}

	[XmlRoot(ElementName = "Structures", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class Structures
	{
		[XmlElement(ElementName = "Structure", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public List<Structure> Structure { get; set; }
	}

	[XmlRoot(ElementName = "Navigation", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class Navigation
	{
		[XmlElement(ElementName = "Instruction", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public string Instruction { get; set; }
		[XmlElement(ElementName = "Distance", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public Distance2 Distance2 { get; set; }
	}

	[XmlRoot(ElementName = "RoutePoint", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class RoutePoint1
	{
		[XmlElement(ElementName = "Description", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public string Description3 { get; set; }
		[XmlAttribute(AttributeName = "PointType")]
		public string PointType { get; set; }
	}

	[XmlRoot(ElementName = "Content", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class Content
	{
		[XmlElement(ElementName = "RoutePoint", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public RoutePoint1 RoutePoint { get; set; }
		[XmlElement(ElementName = "MotorwayCaution", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public MotorwayCaution MotorwayCaution { get; set; }
	}

	[XmlRoot(ElementName = "Note", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class Note
	{
		[XmlElement(ElementName = "Content", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public Content Content { get; set; }
	}

	[XmlRoot(ElementName = "NoteListPosition", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class NoteListPosition
	{
		[XmlElement(ElementName = "Note", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public Note Note { get; set; }
	}

	[XmlRoot(ElementName = "Instruction", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class Instruction
	{
		[XmlElement(ElementName = "Navigation", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public Navigation Navigation { get; set; }
		[XmlElement(ElementName = "NoteListPosition", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public NoteListPosition NoteListPosition { get; set; }
	}

	[XmlRoot(ElementName = "InstructionListPosition", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class InstructionListPosition
	{
		[XmlElement(ElementName = "Instruction", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public Instruction Instruction { get; set; }
	}

	[XmlRoot(ElementName = "Alternative", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class Alternative
	{
		[XmlElement(ElementName = "InstructionListPosition", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public InstructionListPosition InstructionListPosition { get; set; }
	}

	[XmlRoot(ElementName = "AlternativeListPosition", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class AlternativeListPosition
	{
		[XmlElement(ElementName = "Alternative", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public Alternative Alternative { get; set; }
	}

	[XmlRoot(ElementName = "SubPart", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class SubPart
	{
		[XmlElement(ElementName = "AlternativeListPosition", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public AlternativeListPosition AlternativeListPosition { get; set; }
	}

	[XmlRoot(ElementName = "SubPartListPosition", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class SubPartListPosition
	{
		[XmlElement(ElementName = "SubPart", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public SubPart SubPart { get; set; }
	}

	[XmlRoot(ElementName = "Distance", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class Distance2
	{
		[XmlElement(ElementName = "MeasuredMetric", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public string MeasuredMetric { get; set; }
		[XmlElement(ElementName = "DisplayMetric", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public string DisplayMetric { get; set; }
		[XmlElement(ElementName = "DisplayImperial", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public string DisplayImperial { get; set; }
	}

	[XmlRoot(ElementName = "MotorwayCaution", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class MotorwayCaution
	{
		[XmlAttribute(AttributeName = "type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Type { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "DrivingInstructions", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
	public class DrivingInstructions1
	{
		public string LegNumber { get; set; }
		public string Name2 { get; set; }
		public List<SubPartListPosition> SubPartListPosition { get; set; }
		[XmlAttribute(AttributeName = "Id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "ComparisonId")]
		public string ComparisonId { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "RoadPart", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RoadPart
	{
		[XmlElement(ElementName = "StartPointListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public StartPointListPosition StartPointListPosition { get; set; }
		[XmlElement(ElementName = "EndPointListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public EndPointListPosition EndPointListPosition { get; set; }
		[XmlElement(ElementName = "Distance", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Distance Distance { get; set; }
		[XmlElement(ElementName = "Vehicles", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Vehicles Vehicles { get; set; }
		[XmlElement(ElementName = "Roads", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Roads Roads { get; set; }
		[XmlElement(ElementName = "Structures", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public Structures Structures { get; set; }
		[XmlElement(ElementName = "DrivingInstructions", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
		public DrivingInstructions DrivingInstructions { get; set; }
	}

	[XmlRoot(ElementName = "RoutePart", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RoutePart1
	{
		[XmlElement(ElementName = "LegNumber", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string LegNumber { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Name { get; set; }
		[XmlElement(ElementName = "RoadPart", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public RoadPart RoadPart { get; set; }
		[XmlElement(ElementName = "Mode", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Mode { get; set; }
		[XmlAttribute(AttributeName = "Id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "RoutePartListPosition", Namespace = "http://www.esdal.com/schemas/core/movement")]
	public class RoutePartListPosition
	{
		[XmlElement(ElementName = "Route", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string Route { get; set; }
		[XmlElement(ElementName = "RouteImperial", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public string RouteImperial { get; set; }
		[XmlElement(ElementName = "RoutePart", Namespace = "http://www.esdal.com/schemas/core/movement")]
		public RoutePart1 RoutePart { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	public class RouteParts1
	{
		public RoutePartListPosition RoutePartListPosition { get; set; }
	}


}
