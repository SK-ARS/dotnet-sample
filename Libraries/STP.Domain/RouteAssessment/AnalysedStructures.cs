#region namespace
using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
#endregion

namespace STP.Domain.RouteAssessment.XmlAnalysedStructures
{
    #region AnalysedStructures
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    [XmlRoot(ElementName = "AnalysedStructures", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public class AnalysedStructures
    {

        // ELEMENTS
        [XmlElement("AnalysedStructuresPart")]
        public List<AnalysedStructuresPart> AnalysedStructuresPart { get; set; }

        // CONSTRUCTOR
        public AnalysedStructures()
        { }
    }
    #endregion

    public class AnalysedStructuresPart
    {
        // ATTRIBUTES
        [XmlAttribute("ComparisonId")]
        public int ComparisonId { get; set; }

        [XmlAttribute("Id")]
        public int Id { get; set; }

        // ELEMENTS
        [XmlElement("Name")]
        public string AnalysedStructuresPartName { get; set; }

        [XmlElement("Structure")]
        public List<Structure> Structure { get; set; }

        // CONSTRUCTOR
        public AnalysedStructuresPart()
        { }
    }

    public class Structure
    {
        // ATTRIBUTES
        [XmlAttribute("StructureSectionId")]
        public long StructureSectionId { get; set; }

        [XmlAttribute("TraversalType")]
        public string TraversalType { get; set; }

        // ELEMENTS
        [XmlElement("ESRN")]
        public string ESRN { get; set; }

        [XmlElement("Name")]
        public string StructureName { get; set; }

        [XmlElement("Constraints")]
        public Constraints Constraints { get; set; }

        [XmlElement("StructureResponsibility")]
        public StructureResponsibility StructureResponsibility { get; set; }

        [XmlElement("Appraisal")]
        public List<Appraisal> Appraisal { get; set; }
        [XmlElement("AlsatAppraisal")]
        public AlsatAppraisal AlsatAppraisal { get; set; }

        public List<AnalysedCautionStructure> AnalysedCautions { get; set; }

        // CONSTRUCTOR
        public Structure()
        { }
    }
    public class AlsatAppraisal
    {
        [XmlElement("ESRN")]
        public string ESRN { get; set; }

        [XmlElement("StuctureKey")]
        public string StructureKey { get; set; }

        [XmlElement("StructureCalculationType")]
        public string StructureCalculationType { get; set; }

        [XmlElement("ResultStructure")]
        public string ResultStructure { get; set; }

        [XmlElement("Sf")]
        public double Sf { get; set; }

        [XmlElement("CommentsForHaulier")]
        public string CommentsForHaulier { get; set; }

        [XmlElement("AssessmentComments")]
        public string AssessmentComments { get; set; }

        //Constructor
        public AlsatAppraisal()
        { }
    }

    public class Constraints
    {

        // ELEMENTS
        [XmlElement("UnsignedSpatialConstraint")]
        public UnsignedSpatialConstraint UnsignedSpatialConstraint { get; set; }

        [XmlElement("SignedSpatialConstraints")]
        public SignedSpatialConstraints SignedSpatialConstraints { get; set; }

        // CONSTRUCTOR
        public Constraints()
        { }
    }


    public class StructureResponsibility
    {

        // ELEMENTS
        [XmlElement("StructureResponsibleParty")]
        public List<StructureResponsibleParty> StructureResponsibleParty { get; set; }

        // CONSTRUCTOR
        public StructureResponsibility()
        { }
    }

    public class Appraisal
    {
        // ATTRIBUTES
        [XmlAttribute("OrganisationId")]
        public int OrganisationId { get; set; }

        // ELEMENTS
        [XmlElement("Suitability")]
        public AppraisalSuitability AppraisalSuitability { get; set; }

        [XmlElement("Organisation")]
        public Organisation Organisation { get; set; }

        [XmlElement("IndividualSectionSuitability")]
        public IndividualSectionSuitability IndividualSectionSuitability { get; set; }

        // CONSTRUCTOR
        public Appraisal()
        { }
    }


    public class SignedSpatialConstraints
    {

        // ELEMENTS
        [XmlElement("Height")]
        public SignedSpatialConstraintsHeight SignedSpatialConstraintsHeight { get; set; }

        // CONSTRUCTOR
        public SignedSpatialConstraints()
        { }
    }


    public class UnsignedSpatialConstraint
    {

        // ELEMENTS
        [XmlElement("Height", Namespace = "http://www.esdal.com/schemas/core/structure")]
        public decimal Height { get; set; }

        [XmlElement("Width", Namespace = "http://www.esdal.com/schemas/core/structure")]
        public decimal Width { get; set; }

        // CONSTRUCTOR
        public UnsignedSpatialConstraint()
        { }
    }



    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/structure")]
    public class SignedSpatialConstraintsHeight
    {

        // ELEMENTS
        [XmlElement("SignedDistanceValue")]
        public SignedDistanceValue SignedDistanceValue { get; set; }

        // CONSTRUCTOR
        public SignedSpatialConstraintsHeight()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/structure")]
    public class UnsignedSpatialConstraintHeight
    {

        // ELEMENTS
        [XmlText]
        public decimal Value { get; set; }

        // CONSTRUCTOR
        public UnsignedSpatialConstraintHeight()
        { }
    }


    public class StructureResponsibleParty
    {
        // ATTRIBUTES
        [XmlAttribute("ContactId")]
        public int ContactId { get; set; }

        [XmlAttribute("OrganisationId")]
        public int OrganisationId { get; set; }

        // ELEMENTS
        [XmlElement("OrganisationName")]
        public string StructureResponsiblePartyOrganisationName { get; set; }

        [XmlElement("OnBehalfOf")]
        public StructureResponsiblePartyOnBehalfOf StructureResponsiblePartyOnBehalfOf { get; set; }

        // CONSTRUCTOR
        public StructureResponsibleParty()
        { }
    }

    public class StructureResponsiblePartyOnBehalfOf
    {
        // ATTRIBUTES
        [XmlAttribute("ContactId")]
        public int ContactId { get; set; }

        [XmlAttribute("DelegationId")]
        public int DelegationId { get; set; }

        [XmlAttribute("OrganisationId")]
        public int OrganisationId { get; set; }

        [XmlIgnore]
        public bool RetainNotification { get; set; }
        [XmlAttribute("RetainNotification")]
        public string RetainNotificationString
        {
            get { return RetainNotification ? "true" : "false"; }
            set { RetainNotification = value == "true"; }
        }

        [XmlIgnore]
        public bool WantsFailureAlert { get; set; }
        [XmlAttribute("WantsFailureAlert")]
        public string WantsFailureAlertString
        {
            get { return WantsFailureAlert ? "true" : "false"; }
            set { WantsFailureAlert = value == "true"; }
        }

        // ELEMENTS
        [XmlElement("OrganisationName")]
        public string OrganisationName { get; set; }
        //public StructureResponsiblePartyOnBehalfOfOrganisationName StructureResponsiblePartyOnBehalfOfOrganisationName { get; set; }

        [XmlElement("OnBehalfOf")]
        public OnBehalfOfOnBehalfOf OnBehalfOfOnBehalfOf { get; set; }

        // CONSTRUCTOR
        public StructureResponsiblePartyOnBehalfOf()
        { }
    }

    public class OnBehalfOfOnBehalfOf
    {
        // ATTRIBUTES
        [XmlAttribute("ContactId")]
        public int ContactId { get; set; }

        [XmlAttribute("DelegationId")]
        public int DelegationId { get; set; }

        [XmlAttribute("OrganisationId")]
        public int OrganisationId { get; set; }

        [XmlIgnore]
        public bool RetainNotification { get; set; }
        [XmlAttribute("RetainNotification")]
        public string RetainNotificationString
        {
            get { return RetainNotification ? "true" : "false"; }
            set { RetainNotification = value == "true"; }
        }

        [XmlIgnore]
        public bool WantsFailureAlert { get; set; }
        [XmlAttribute("WantsFailureAlert")]
        public string WantsFailureAlertString
        {
            get { return WantsFailureAlert ? "true" : "false"; }
            set { WantsFailureAlert = value == "true"; }
        }

        // ELEMENTS
        [XmlElement("OrganisationName")]
        public string OrganisationName { get; set; }

        // CONSTRUCTOR
        public OnBehalfOfOnBehalfOf()
        { }
    }

    public class OnBehalfOfOnBehalfOfOrganisationName
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public OnBehalfOfOnBehalfOfOrganisationName()
        { }
    }

    public class StructureResponsiblePartyOnBehalfOfOrganisationName
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public StructureResponsiblePartyOnBehalfOfOrganisationName()
        { }
    }

    public class AppraisalSuitability
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public AppraisalSuitability()
        { }
    }

    public class Organisation
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public Organisation()
        { }
    }

    public class IndividualSectionSuitability
    {
        // ATTRIBUTES
        [XmlAttribute("SectionId")]
        public int SectionId { get; set; }

        // ELEMENTS
        [XmlElement("Suitability")]
        public IndividualSectionSuitabilitySuitability IndividualSectionSuitabilitySuitability { get; set; }

        [XmlElement("SectionDescription")]
        public SectionDescription SectionDescription { get; set; }

        [XmlElement("IndividualResult")]
        public IndividualResult IndividualResult { get; set; }

        // CONSTRUCTOR
        public IndividualSectionSuitability()
        { }
    }


    public class IndividualSectionSuitabilitySuitability
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public IndividualSectionSuitabilitySuitability()
        { }
    }

    public class SectionDescription
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public SectionDescription()
        { }
    }

    public class IndividualResult
    {

        // ELEMENTS
        [XmlElement("Suitability")]
        public IndividualResultSuitability IndividualResultSuitability { get; set; }

        [XmlElement("TestClass")]
        public TestClass TestClass { get; set; }

        [XmlElement("TestIdentity")]
        public TestIdentity TestIdentity { get; set; }

        [XmlElement("ResultDetails")]
        public ResultDetails ResultDetails { get; set; }

        // CONSTRUCTOR
        public IndividualResult()
        { }
    }

    public class TestClass
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public TestClass()
        { }
    }

    public class TestIdentity
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public TestIdentity()
        { }
    }

    public class IndividualResultSuitability
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public IndividualResultSuitability()
        { }
    }


    public class SignedDistanceValue
    {

        // ELEMENTS
        [XmlElement("Metres")]
        public decimal Metres { get; set; }

        [XmlElement("Feet")]
        public decimal Feet { get; set; }

        // CONSTRUCTOR
        public SignedDistanceValue()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/structure")]
    public class Width
    {

        // ELEMENTS
        [XmlText]
        public decimal Value { get; set; }

        // CONSTRUCTOR
        public Width()
        { }
    }

    public class Metres
    {

        // ELEMENTS
        [XmlText]
        public decimal Value { get; set; }

        // CONSTRUCTOR
        public Metres()
        { }
    }

    public class Feet
    {

        // ELEMENTS
        [XmlText]
        public decimal Value { get; set; }

        // CONSTRUCTOR
        public Feet()
        { }
    }

    public class ResultDetails
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ResultDetails()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/caution")]
    public class SpecificAction
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public SpecificAction()
        { }
    }

    #region ArchStructure
    public class ArchStructure
    {
        //ELEMENT
        [XmlElement("SpanOfArch")]
        public decimal SpanOfArch { get; set; }

        //ELEMENT
        [XmlElement("RiseAtCrown")]
        public decimal RiseAtCrown { get; set; }

        //ELEMENT
        [XmlElement("RiseAtQuarter")]
        public decimal RiseAtQuarter { get; set; }

        //ELEMENT
        [XmlElement("DepthOfFill")]
        public decimal DepthOfFill { get; set; }

        //ELEMENT
        [XmlElement("BarrelThickness")]
        public decimal BarrelThickness { get; set; }

        //ELEMENT
        [XmlElement("StructureFactorType")]
        public StructureFactorType StructureFactorType { get; set; }

        [XmlElement("AxleLiftOff")]
        public Boolean AxleLiftOff { get; set; }

        [XmlElement("ProvisionalAxleLoad")]
        public decimal ProvisionalAxleLoad { get; set; }

        [XmlElement("ModifiedAxleLoad")]
        public decimal ModifiedAxleLoad { get; set; }

        public ArchStructure()
        { }
    }
    #endregion

    #region StructureFactorType
    public class StructureFactorType
    {
        //ELEMENT
        [XmlElement("BarrelFactor")]
        public decimal BarrelFactor { get; set; }

        //ELEMENT
        [XmlElement("ConditionFactor")]
        public decimal ConditionFactor { get; set; }

        //ELEMENT
        [XmlElement("DepthFactor")]
        public decimal DepthFactor { get; set; }

        //ELEMENT
        [XmlElement("FillFactor")]
        public decimal FillFactor { get; set; }

        //ELEMENT
        [XmlElement("JointWidthFactor")]
        public decimal JointWidthFactor { get; set; }

        //ELEMENT
        [XmlElement("MortarFactor")]
        public decimal MortarFactor { get; set; }

        //ELEMENT
        [XmlElement("MaterialFactor")]
        public decimal MaterialFactor { get; set; }

        //ELEMENT
        [XmlElement("JointFactor")]
        public decimal JointFactor { get; set; }

        //ELEMENT
        [XmlElement("ProfileFactor")]
        public decimal ProfileFactor { get; set; }

        //ELEMENT
        [XmlElement("SpanRiseFactor")]
        public decimal SpanRiseFactor { get; set; }

        //ELEMENT
        [XmlElement("ModificationFactor")]
        public decimal ModificationFactor { get; set; }

        public StructureFactorType()
        { }
    }
    #endregion

    #region AxleWeightConstraintsStructure
    //[XmlElement("AxleWeightConstraintsStructure")]
    public class AxleWeightConstraintsStructure
    {

        [XmlElement("AxleGroupWeight")]
        public WeightConstraintValueType AxleGroupWeight { get; set; }

        [XmlElement("AxleGroupLength")]
        public decimal AxleGroupLength { get; set; }

        public AxleWeightConstraintsStructure()
        { }
    }
    #endregion

    #region WeightConstraintValueType
    public class WeightConstraintValueType
    {
        [XmlText]
        public decimal Value { get; set; }

        public WeightConstraintValueType()
        { }
    }
    #endregion

    #region BearingStructure
    //[XmlElement("BearingStructure")]
    public class BearingStructure
    {
        [XmlElement("PredefinedType")]
        public PredefinedBearingType PredefinedBearingType { get; set; }

        [XmlElement("UserDefinedType")]
        public SimpleDescriptionType SimpleDescriptionType { get; set; }

        public BearingStructure()
        { }
    }
    #endregion

    public enum PredefinedBearingType
    {
        none,
        elastomeric,
        guided,
        rocker
    }

    public class SimpleDescriptionType
    {
        [XmlText]
        public string Value
        {
            get
            {
                return Value;
            }
            set
            {
                if (Value.Length > 100)
                {
                    Value = Value.Substring(0, 100);
                }
                else
                {
                    Value = Value;
                }
            }
        }

        public SimpleDescriptionType()
        { }
    }
}
