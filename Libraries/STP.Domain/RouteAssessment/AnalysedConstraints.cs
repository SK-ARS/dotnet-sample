#region Using Namespace
using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
#endregion


namespace STP.Domain.RouteAssessment.XmlConstraints
{
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    [XmlRoot(ElementName = "AnalysedConstraints", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public class AnalysedConstraints
    {

        // ELEMENTS
        [XmlElement("AnalysedConstraintsPart")]
        public List<AnalysedConstraintsPart> AnalysedConstraintsPart { get; set; }

        // CONSTRUCTOR
        public AnalysedConstraints()
        { }
    }

    public class AnalysedConstraintsPart
    {
        // ATTRIBUTES
        [XmlAttribute("Id")]
        public int Id { get; set; }

        // ELEMENTS
        [XmlElement("Name")]
        public string AnalysedConstraintsPartName { get; set; }
        //public AnalysedConstraintsPartName AnalysedConstraintsPartName { get; set; }


        [XmlElement("Constraint")]
        public List<Constraint> Constraint { get; set; }

        // CONSTRUCTOR
        public AnalysedConstraintsPart()
        { }
    }

    public class AnalysedConstraintsPartName
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public AnalysedConstraintsPartName()
        { }
    }

    public class ActionToBeTaken
    {

        // ELEMENTS
        [XmlElement("SpecificAction", Namespace = "http://www.esdal.com/schemas/core/caution")]
        public SpecificAction SpecificAction { get; set; }

        // CONSTRUCTOR
        public ActionToBeTaken()
        { }
    }

    public class Appraisal
    {

        // ELEMENTS
        [XmlElement("Suitability")]
        public Suitability Suitability { get; set; }

        // CONSTRUCTOR
        public Appraisal()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/caution")]
    public class Caution
    {
        // ATTRIBUTES
        [XmlAttribute("CautionId")]
        public int CautionId { get; set; }

        [XmlIgnore]
        public bool IsApplicable { get; set; }
        [XmlAttribute("IsApplicable")]
        public string IsApplicableString
        {
            get { return IsApplicable ? "true" : "false"; }
            set { IsApplicable = value == "true"; }
        }

        // ELEMENTS
        [XmlElement("Action")]
        public ActionToBeTaken Action { get; set; }

        [XmlElement("Conditions")]
        public Conditions Conditions { get; set; }

        [XmlElement("Contacts")]
        public Contacts Contacts { get; set; }

        // CONSTRUCTOR
        public Caution()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/constraint")]
    public class Cautions
    {

        // ELEMENTS
        [XmlElement("Caution", Namespace = "http://www.esdal.com/schemas/core/caution")]
        public Caution Caution { get; set; }

        // CONSTRUCTOR
        public Cautions()
        { }
    }

    public class Conditions
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public Conditions()
        { }
    }

    public class Constraint
    {
        // ATTRIBUTES
        [XmlIgnore]
        public bool IsApplicable { get; set; }
        [XmlAttribute("IsApplicable")]
        public string IsApplicableString
        {
            get { return IsApplicable ? "true" : "false"; }
            set { IsApplicable = value == "true"; }
        }

        // ELEMENTS
        [XmlElement("ECRN", Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public string ECRN { get; set; }

        //public ECRN ECRN { get; set; }

        [XmlElement("Type", Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public string ConstraintType { get; set; }

        //public ConstraintType ConstraintType { get; set; }

        [XmlElement("Name", Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public string ConstraintName { get; set; }

        //public ConstraintName ConstraintName { get; set; }

        [XmlElement("Cautions", Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public Cautions Cautions { get; set; }

        [XmlElement("Contact", Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public Contact Contact { get; set; }

        [XmlElement("Appraisal")]
        public Appraisal Appraisal { get; set; }

        [XmlElement("EntryPoint")]
        public string EntryPoint { get; set; }
        //public EntryPoint EntryPoint { get; set; }

        [XmlElement("ExitPoint")]
        public string ExitPoint { get; set; }
        //public ExitPoint ExitPoint { get; set; }

        [XmlElement("Road")]
        public Road Road { get; set; }

        [XmlElement("Owner")]
        public Owner Owner { get; set; }
        public List<AnalysedCautionStructure> AnalysedCautions { get; set; }

        // CONSTRUCTOR
        public Constraint()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/constraint")]
    public class ConstraintName
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ConstraintName()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/constraint")]
    public class Contact
    {
        // ATTRIBUTES
        [XmlAttribute("ContactId")]
        public int ContactId { get; set; }

        [XmlAttribute("OrganisationId")]
        public int OrganisationId { get; set; }

        // ELEMENTS
        [XmlElement("FullName", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactFullName ContactFullName { get; set; }

        [XmlElement("OrganisationName", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactOrganisationName ContactOrganisationName { get; set; }

        [XmlElement("Address", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactAddress ContactAddress { get; set; }

        [XmlElement("TelephoneNumber", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactTelephoneNumber ContactTelephoneNumber { get; set; }

        [XmlElement("FaxNumber", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactFaxNumber ContactFaxNumber { get; set; }

        [XmlElement("EmailAddress", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactEmailAddress ContactEmailAddress { get; set; }

        // CONSTRUCTOR
        public Contact()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/contact")]
    public class ContactAddress
    {

        // ELEMENTS
        [XmlElement("Line", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public List<ContactAddressLine> ContactAddressLine { get; set; }

        [XmlElement("PostCode", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public ContactAddressPostCode ContactAddressPostCode { get; set; }

        [XmlElement("Country", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public ContactAddressCountry ContactAddressCountry { get; set; }

        // CONSTRUCTOR
        public ContactAddress()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public class ContactAddressLine
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactAddressLine()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public class ContactAddressPostCode
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactAddressPostCode()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public class ContactAddressCountry
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactAddressCountry()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/contact")]
    public class ContactEmailAddress
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactEmailAddress()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/contact")]
    public class ContactFaxNumber
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactFaxNumber()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/contact")]
    public class ContactFullName
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactFullName()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/contact")]
    public class ContactOrganisationName
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactOrganisationName()
        { }
    }

    public class Contacts
    {

        // ELEMENTS
        [XmlElement("ResolvedContact")]
        public ResolvedContact ResolvedContact { get; set; }

        // CONSTRUCTOR
        public Contacts()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/contact")]
    public class ContactTelephoneNumber
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ContactTelephoneNumber()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/constraint")]
    public class ECRN
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ECRN()
        { }
    }

    public class EntryPoint
    {

        // ELEMENTS
        //[XmlText]
        //public int Value { get; set; }

        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public EntryPoint()
        { }
    }

    public class ExitPoint
    {

        // ELEMENTS
        //[XmlText]

        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ExitPoint()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public class Number
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public Number()
        { }
    }

    public class Owner
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public Owner()
        { }
    }

    public class ResolvedContact
    {
        // ATTRIBUTES
        [XmlAttribute("ContactId")]
        public int ContactId { get; set; }

        [XmlAttribute("OrganisationId")]
        public int OrganisationId { get; set; }

        // ELEMENTS
        [XmlElement("FullName", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactFullName ResolvedContactFullName { get; set; }

        [XmlElement("OrganisationName", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactOrganisationName ResolvedContactOrganisationName { get; set; }

        [XmlElement("Address", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactAddress ResolvedContactAddress { get; set; }

        [XmlElement("TelephoneNumber", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactTelephoneNumber ResolvedContactTelephoneNumber { get; set; }

        [XmlElement("FaxNumber", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactFaxNumber ResolvedContactFaxNumber { get; set; }

        [XmlElement("EmailAddress", Namespace = "http://www.esdal.com/schemas/core/contact")]
        public ContactEmailAddress ResolvedContactEmailAddress { get; set; }

        // CONSTRUCTOR
        public ResolvedContact()
        { }
    }

    public class Road
    {

        [XmlElement("Name", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public RoadName RoadName { get; set; }

        // ELEMENTS
        [XmlElement("Number", Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public Number Number { get; set; }

        // CONSTRUCTOR
        public Road()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public class RoadName
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public RoadName()
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

    public class Suitability
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public Suitability()
        { }
    }

    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/constraint")]
    public class ConstraintType
    {

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public ConstraintType()
        {
        }
    }
}
