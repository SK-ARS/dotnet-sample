#region Using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
#endregion

#region
/*
 * 
 * classes generated using XSD.exe the classes seen below are part of caution related classes 
 * Added on 05-May-2014
 * 
 * */
#endregion

namespace STP.Domain.RouteAssessment.XmlAnalysedCautions
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    [XmlRootAttribute("AnalysedCautions", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
    public partial class AnalysedCautions
    {

        /// <remarks/>
        [XmlElementAttribute("AnalysedCautionsPart")]
        public List<AnalysedCautionsPart> AnalysedCautionsPart
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedCautionsPart : AnalysedCautionsPartStructure
    {

        public AnalysedCautionsPart()
        {
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ModeOfTransportType.road)]
        public ModeOfTransportType ModeOfTransport
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
        [XmlAttributeAttribute()]
        public int ComparisonId
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedCautionsPartStructure
    {


        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("Caution")]
        public List<AnalysedCautionStructure> Caution
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedCautionStructure : ResolvedCautionStructure
    {

        public AnalysedCautionStructure()
        {
        }

        /// <remarks/>
        public RoadIdentificationStructure Road
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("CautionedEntity")]
        public AnalysedCautionChoiceStructure CautionedEntity1
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("Vehicle", DataType = "token")]
        public string[] Vehicle
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsSuppressed
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public partial class RoadIdentificationStructure
    {

        public RoadIdentificationStructure()
        {
        }
        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Number
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Unidentified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedCautionChoiceStructure
    {
        /// <remarks/>
        [XmlElementAttribute("Constraint", typeof(AnalysedCautionConstraintStructure))]
        public AnalysedCautionConstraintStructure AnalysedCautionConstraintStructure
        {
            get;
            set;
        }

        [XmlElementAttribute("Structure", typeof(AnalysedCautionStructureStructure))]
        public AnalysedCautionStructureStructure AnalysedCautionStructureStructure
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedCautionConstraintStructure
    {

        /// <remarks/>
        public string ECRN
        {
            get;
            set;
        }

        /// <remarks/>
        public ConstraintType Type
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("Annotation")]
        public List<ResolvedAnnotationStructure> Annotation
        {
            get;
            set;
        }

        public AnalysedCautionConstraintStructure()
        {
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
    public enum ConstraintType
    {

        /// <remarks/>
        generic,

        /// <remarks/>
        height,

        /// <remarks/>
        width,

        /// <remarks/>
        length,

        /// <remarks/>
        weight,

        /// <remarks/>
        oneway,

        /// <remarks/>
        roadworks,

        /// <remarks/>
        incline,

        /// <remarks/>
        tram,

        /// <remarks/>
        [XmlEnumAttribute("tight bend")]
        tightbend,

        /// <remarks/>
        @event,

        /// <remarks/>
        [XmlEnumAttribute("risk of grounding")]
        riskofgrounding,

        /// <remarks/>
        unmade,

        /// <remarks/>
        [XmlEnumAttribute("natural void")]
        naturalvoid,

        /// <remarks/>
        [XmlEnumAttribute("manmade void")]
        manmadevoid,

        /// <remarks/>
        tunnel,

        /// <remarks/>
        [XmlEnumAttribute("tunnel void")]
        tunnelvoid,

        /// <remarks/>
        [XmlEnumAttribute("pipes and ducts")]
        pipesandducts,

        /// <remarks/>
        [XmlEnumAttribute("retaining wall")]
        retainingwall,

        /// <remarks/>
        [XmlEnumAttribute("traffic calming")]
        trafficcalming,

        /// <remarks/>
        [XmlEnumAttribute("overhead building")]
        overheadbuilding,

        /// <remarks/>
        [XmlEnumAttribute("overhead pipes and utilities")]
        overheadpipesandutilities,

        /// <remarks/>
        [XmlEnumAttribute("adjacent retaining wall")]
        adjacentretainingwall,

        /// <remarks/>
        [XmlEnumAttribute("power cable")]
        powercable,

        /// <remarks/>
        [XmlEnumAttribute("electrification cable")]
        electrificationcable,

        /// <remarks/>
        [XmlEnumAttribute("telecomms cable")]
        telecommscable,

        /// <remarks/>
        [XmlEnumAttribute("gantry road furniture")]
        gantryroadfurniture,

        /// <remarks/>
        [XmlEnumAttribute("cantilever road furniture")]
        cantileverroadfurniture,

        /// <remarks/>
        [XmlEnumAttribute("catenary road furniture")]
        catenaryroadfurniture,

        /// <remarks/>
        bollard,

        /// <remarks/>
        [XmlEnumAttribute("removable bollard")]
        removablebollard,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedCautionStructureStructure
    {

        /// <remarks/>
        public string ESRN
        {
            get;
            set;
        }

        public long SECTION_ID
        {
            get;
            set;
        }
        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        public StructureType Type
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("Annotation")]
        public List<ResolvedAnnotationStructure> Annotation
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [XmlIncludeAttribute(typeof(AnalysedCautionStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    [XmlRootAttribute("ResolvedCaution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
    public partial class ResolvedCautionStructure : CautionStructure
    {

        public ResolvedCautionStructure()
        {
        }

        /// <remarks/>
        [XmlElementAttribute("Contact")]
        public List<ResolvedContactStructure> Contact
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("ConstrainingAttribute")]
        public List<CautionConditionType> ConstrainingAttribute
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool IsApplicable
        {
            get;
            set;
        }
    }

    [System.SerializableAttribute()]
    public enum StructureType
    {
        [XmlEnumAttribute("Underbridge")]
        underbridge,

        [XmlEnumAttribute("Overbridge")]
        overbridge,

        [XmlEnumAttribute("Level crossing")]
        levelcrossing,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public enum CautionConditionType
    {

        /// <remarks/>
        [XmlEnumAttribute("gross weight")]
        grossweight,

        /// <remarks/>
        [XmlEnumAttribute("axle weight")]
        axleweight,

        /// <remarks/>
        height,

        /// <remarks/>
        [XmlEnumAttribute("overall length")]
        overalllength,

        /// <remarks/>
        width,

        /// <remarks/>
        speed,
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(ResolvedAnnotationStructure))]
    [XmlIncludeAttribute(typeof(AnalysedAnnotationStructure))]
    [XmlIncludeAttribute(typeof(UnresolvedAnnotationStructure))]
    [XmlIncludeAttribute(typeof(RouteAnnotationStructure))]
    [XmlIncludeAttribute(typeof(ConstraintAnnotationContactReasonStructure))]
    [XmlIncludeAttribute(typeof(StructureAnnotationContactReasonStructure))]
    [XmlIncludeAttribute(typeof(AnnotationContactReasonStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
    public abstract partial class AnnotationStructure
    {

        public AnnotationStructure()
        {
        }
        /// <remarks/>
        public SimpleTextStructure Text
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int AnnotationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool AnnotationIdSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(AnnotationType.generic)]
        public AnnotationType AnnotationType
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class ConstraintAnnotationContactReasonStructure : AnnotationStructure
    {

        /// <remarks/>
        public string ECRN
        {
            get;
            set;
        }

        /// <remarks/>
        public ConstraintType Type
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class StructureAnnotationContactReasonStructure : AnnotationStructure
    {

        /// <remarks/>
        public string ESRN
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnnotationContactReasonStructure : AnnotationStructure
    {

        /// <remarks/>
        public RoadIdentificationStructure Road
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
    public enum AnnotationType
    {

        /// <remarks/>
        generic,

        /// <remarks/>
        caution,

        /// <remarks/>
        [XmlEnumAttribute("special manouevre")]
        specialmanouevre,
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(RouteAnnotationStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
    [XmlRootAttribute("UnresolvedAnnotation", Namespace = "http://www.esdal.com/schemas/core/annotation", IsNullable = false)]
    public partial class UnresolvedAnnotationStructure : AnnotationStructure
    {

        /// <remarks/>
        [XmlElementAttribute("Contact")]
        public List<ContactReferenceStructure> Contact
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
    [XmlRootAttribute("RouteAnnotation", Namespace = "http://www.esdal.com/schemas/core/annotation", IsNullable = false)]
    public partial class RouteAnnotationStructure : UnresolvedAnnotationStructure
    {

        /// <remarks/>
        public SegmentPointStructure SegmentPoint
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public AnnotationAssociationType Association
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool AssociationSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Structure
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Constraint
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
    public partial class SegmentPointStructure
    {

        public SegmentPointStructure()
        {
        }

        /// <remarks/>
        public long RoadSectionId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool RoadSectionIdSpecified
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
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsBroken
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedAnnotationStructure : ResolvedAnnotationStructure
    {

        /// <remarks/>
        public RoadIdentificationStructure Road
        {
            get;
            set;
        }

        /// <remarks/>
        public AnalysedAnnotationChoiceStructure AnnotatedEntity
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class CautionOwnerStructure
    {
        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Organisation
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
        [XmlIgnoreAttribute()]
        public bool OrganisationIdSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
    public enum AnnotationAssociationType
    {

        /// <remarks/>
        road,

        /// <remarks/>
        structure,

        /// <remarks/>
        constraint,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class ContactReferenceStructure
    {

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Description
        {
            get;
            set;
        }

        /// <remarks/>
        public ContactReferenceChoiceStructure Contact
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class ContactReferenceChoiceStructure
    {
        /// <remarks/>
        [XmlElementAttribute("AdhocReference", typeof(AdhocContactReferenceStructure))]
        [XmlElementAttribute("RoleReference", typeof(RoleContactReferenceStructure))]
        [XmlElementAttribute("SimpleReference", typeof(SimpleContactReferenceStructure))]
        public object Item
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class AdhocContactReferenceStructure
    {

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string FullName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string OrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        public AddressStructure Address
        {
            get;
            set;
        }

        /// <remarks/>
        public string TelephoneNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string TelephoneExtension
        {
            get;
            set;
        }

        /// <remarks/>
        public string MobileNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string FaxNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string EmailAddress
        {
            get;
            set;
        }

        /// <remarks/>
        public EmailFormatType EmailFormatPreference
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool EmailFormatPreferenceSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class RoleContactReferenceStructure
    {

        /// <remarks/>
        public RoleType RoleWithinOganisation
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string OrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string ResolvedFullName
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
        public int ResolvedContactId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool ResolvedContactIdSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class SimpleContactReferenceStructure
    {

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string FullName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string OrganisationName
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
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedAnnotationChoiceStructure
    {

        /// <remarks/>
        [XmlElementAttribute("Constraint", typeof(AnalysedAnnotationConstraintStructure))]
        [XmlElementAttribute("Road", typeof(AnalysedAnnotationRoadStructure))]
        [XmlElementAttribute("Structure", typeof(AnalysedAnnotationStructureStructure))]
        public object Item
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedAnnotationConstraintStructure
    {

        /// <remarks/>
        public string ECRN
        {
            get;
            set;
        }

        /// <remarks/>
        public ConstraintType Type
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedAnnotationRoadStructure
    {

        /// <remarks/>
        public string OSGridRef
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedAnnotationStructureStructure
    {
        /// <remarks/>
        public string ESRN
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(AnalysedAnnotationStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
    public partial class ResolvedAnnotationStructure : AnnotationStructure
    {

        public ResolvedAnnotationStructure()
        {
        }

        /// <remarks/>
        [XmlElementAttribute("Contact")]
        public List<ResolvedContactStructure> Contact
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsDrivingInstructionAnnotation
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public partial class ResolvedContactStructure
    {

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Description
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string FullName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string OrganisationName
        {
            get;
            set;
        }

        /// <remarks/>
        public RoleType Role
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool RoleSpecified
        {
            get;
            set;
        }

        /// <remarks/>
        public AddressStructure Address
        {
            get;
            set;
        }

        /// <remarks/>
        public string TelephoneNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string TelephoneExtension
        {
            get;
            set;
        }

        /// <remarks/>
        public string MobileNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string FaxNumber
        {
            get;
            set;
        }

        /// <remarks/>
        public string EmailAddress
        {
            get;
            set;
        }

        /// <remarks/>
        public EmailFormatType EmailFormatPreference
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool EmailFormatPreferenceSpecified
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
        [XmlIgnoreAttribute()]
        public bool ContactIdSpecified
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
        [XmlIgnoreAttribute()]
        public bool OrganisationIdSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public partial class AddressStructure : UKPostalAddressStructure
    {
        /// <remarks/>
        public CountryType Country
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool CountrySpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public enum CountryType
    {

        /// <remarks/>
        england,

        /// <remarks/>
        wales,

        /// <remarks/>
        scotland,

        /// <remarks/>
        [XmlEnumAttribute("northern ireland")]
        northernireland,
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(AddressStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/AddressAndPersonalDetails")]
    public partial class UKPostalAddressStructure
    {

        /// <remarks/>
        [XmlElementAttribute("Line")]
        public List<string> Line
        {
            get;
            set;
        }

        /// <remarks/>
        public string PostCode
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public enum EmailFormatType
    {

        /// <remarks/>
        html,

        /// <remarks/>
        text,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
    public enum RoleType
    {

        /// <remarks/>
        [XmlEnumAttribute("data holder")]
        dataholder,

        /// <remarks/>
        [XmlEnumAttribute("notification contact")]
        notificationcontact,

        /// <remarks/>
        [XmlEnumAttribute("official contact")]
        officialcontact,

        /// <remarks/>
        [XmlEnumAttribute("police alo")]
        policealo,

        /// <remarks/>
        haulier,

        /// <remarks/>
        [XmlEnumAttribute("it contact")]
        itcontact,

        /// <remarks/>
        [XmlEnumAttribute("default contact")]
        defaultcontact,

        /// <remarks/>
        [XmlEnumAttribute("data owner")]
        dataowner,
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(SimpleTextStructure))]
    [XmlIncludeAttribute(typeof(LevelTwoLetteredStructure))]
    [XmlIncludeAttribute(typeof(LevelTwoNumberedStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public partial class LevelTwoTextStructure
    {

        private LevelTwoTextStructure[] ItemsField;

        private ItemsChoiceType[] itemsElementNameField;

        private string[] TextField;

        /// <remarks/>
        [XmlElementAttribute("Bold", typeof(LevelTwoTextStructure))]
        [XmlElementAttribute("Italic", typeof(LevelTwoTextStructure))]
        [XmlElementAttribute("Underscore", typeof(LevelTwoTextStructure))]
        [XmlChoiceIdentifierAttribute("ItemsElementName")]
        public LevelTwoTextStructure[] Items
        {
            get
            {
                return this.ItemsField;
            }
            set
            {
                this.ItemsField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("ItemsElementName")]
        [XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        Bold,

        /// <remarks/>
        Italic,

        /// <remarks/>
        Underscore,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public partial class SimpleTextStructure : LevelTwoTextStructure
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public abstract partial class LevelTwoLetteredStructure : LevelTwoTextStructure
    {

        private string letterField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Letter
        {
            get
            {
                return this.letterField;
            }
            set
            {
                this.letterField = value;
            }
        }
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(LevelOneLetteredStructure))]
    [XmlIncludeAttribute(typeof(LevelOneNumberedStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public partial class LevelOneTextStructure
    {

        private object[] itemsField;

        private ItemsChoiceType1[] itemsElementNameField;

        private string[] textField;

        /// <remarks/>
        [XmlElementAttribute("Bold", typeof(LevelOneTextStructure))]
        [XmlElementAttribute("BulletedText", typeof(LevelTwoTextStructure))]
        [XmlElementAttribute("Italic", typeof(LevelOneTextStructure))]
        [XmlElementAttribute("LetteredText", typeof(LevelTwoLetteredStructure))]
        [XmlElementAttribute("NumberedText", typeof(LevelTwoNumberedStructure))]
        [XmlElementAttribute("Underscore", typeof(LevelOneTextStructure))]
        [XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("ItemsElementName")]
        [XmlIgnoreAttribute()]
        public ItemsChoiceType1[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public abstract partial class LevelTwoNumberedStructure : LevelTwoTextStructure
    {

        private string numberField;

        /// <remarks/>
        [XmlAttributeAttribute(DataType = "positiveInteger")]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {

        /// <remarks/>
        Bold,

        /// <remarks/>
        BulletedText,

        /// <remarks/>
        Italic,

        /// <remarks/>
        LetteredText,

        /// <remarks/>
        NumberedText,

        /// <remarks/>
        Underscore,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public abstract partial class LevelOneLetteredStructure : LevelOneTextStructure
    {

        private string letterField;

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Letter
        {
            get
            {
                return this.letterField;
            }
            set
            {
                this.letterField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public abstract partial class LevelOneNumberedStructure : LevelOneTextStructure
    {

        private string NumberField;

        /// <remarks/>
        [XmlAttributeAttribute(DataType = "positiveInteger")]
        public string Number
        {
            get
            {
                return this.NumberField;
            }
            set
            {
                this.NumberField = value;
            }
        }
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(ComplexTextStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public partial class LevelZeroTextStructure
    {

        private object[] ItemsField;

        private ItemsChoiceType2[] ItemsElementNameField;

        private string[] TextField;

        /// <remarks/>
        [XmlElementAttribute("Bold", typeof(LevelZeroTextStructure))]
        [XmlElementAttribute("BulletedText", typeof(LevelOneTextStructure))]
        [XmlElementAttribute("Italic", typeof(LevelZeroTextStructure))]
        [XmlElementAttribute("LetteredText", typeof(LevelOneLetteredStructure))]
        [XmlElementAttribute("NumberedText", typeof(LevelOneNumberedStructure))]
        [XmlElementAttribute("Underscore", typeof(LevelZeroTextStructure))]
        [XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.ItemsField;
            }
            set
            {
                this.ItemsField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("ItemsElementName")]
        [XmlIgnoreAttribute()]
        public ItemsChoiceType2[] ItemsElementName
        {
            get
            {
                return this.ItemsElementNameField;
            }
            set
            {
                this.ItemsElementNameField = value;
            }
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IncludeInSchema = false)]
    public enum ItemsChoiceType2
    {

        /// <remarks/>
        Bold,

        /// <remarks/>
        BulletedText,

        /// <remarks/>
        Italic,

        /// <remarks/>
        LetteredText,

        /// <remarks/>
        NumberedText,

        /// <remarks/>
        Underscore,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
    public partial class ComplexTextStructure : LevelZeroTextStructure
    {
    }

    /// <remarks/>
    [XmlIncludeAttribute(typeof(EntityCautionStructure))]
    [XmlIncludeAttribute(typeof(ResolvedCautionStructure))]
    [XmlIncludeAttribute(typeof(AnalysedCautionStructure))]
    [XmlIncludeAttribute(typeof(UnresolvedCautionStructure))]
    [XmlIncludeAttribute(typeof(CautionContactReasonStructure))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public abstract partial class CautionStructure
    {

        /// <remarks/>
        public CautionActionStructure Action
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        public CautionedEntityChoiceStructure CautionedEntity
        {
            get;
            set;
        }

        /// <remarks/>
        public CautionConditionStructure Conditions
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int CautionId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool CautionIdSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class CautionContactReasonStructure : CautionStructure
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
    public partial class VerticalAlignmentPointStructure
    {
        /// <remarks/>
        public decimal Distance
        {
            get;
            set;
        }

        /// <remarks/>
        public decimal Height
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    [XmlRootAttribute("UnresolvedCaution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
    public partial class UnresolvedCautionStructure : CautionStructure
    {

        /// <remarks/>
        public CautionOwnerStructure Owner
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("Contact")]
        public List<ContactReferenceStructure> Contact
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class EntityCautionStructure : CautionStructure
    {

        /// <remarks/>
        public EntityCautionContactChoiceStructure Contacts
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public bool IsApplicable
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool IsApplicableSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class EntityCautionContactChoiceStructure
    {

        /// <remarks/>
        [XmlElementAttribute("ResolvedContact", typeof(ResolvedContactStructure))]
        public List<ResolvedContactStructure> ResolvedContactItems
        {
            get;
            set;
        }

        [XmlElementAttribute("UnresolvedContact", typeof(ContactReferenceStructure))]
        public List<ContactReferenceStructure> ContactReferenceStructureItems
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class CautionActionStructure
    {

        /// <remarks/>
        [XmlElementAttribute("SpecificAction")] //, typeof(SimpleTextStructure))]
        public string SpecificAction
        {
            get;
            set;
        }

        [XmlElementAttribute("Standard", typeof(object))]
        public object Item
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class CautionedEntityChoiceStructure
    {

        /// <remarks/>
        [XmlElementAttribute("Constraint", typeof(CautionedConstraintStructure))]
        public CautionedConstraintStructure CautionedConstraintStructure
        {
            get;
            set;
        }

        [XmlElementAttribute("Structure", typeof(CautionedStructureStructure))]
        public CautionedStructureStructure CautionedStructureStructure
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class CautionedConstraintStructure
    {

        /// <remarks/>
        public string ECRN
        {
            get;
            set;
        }

        /// <remarks/>
        public ConstraintType Type
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string ConstraintName
        {
            get;
            set;
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class CautionedStructureStructure
    {

        /// <remarks/>
        public string ESRN
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "token")]
        public string StructureName
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int SectionId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool SectionIdSpecified
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
    public partial class CautionConditionStructure
    {

        /// <remarks/>
        public WeightStructure MaxGrossWeight
        {
            get;
            set;
        }

        /// <remarks/>
        public WeightStructure MaxAxleWeight
        {
            get;
            set;
        }

        /// <remarks/>
        public DistanceStructure MaxHeight
        {
            get;
            set;
        }

        /// <remarks/>
        public DistanceStructure MaxOverallLength
        {
            get;
            set;
        }

        /// <remarks/>
        public DistanceStructure MaxWidth
        {
            get;
            set;
        }

        /// <remarks/>
        public SpeedStructure MinSpeed
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public partial class WeightStructure
    {

        public WeightStructure()
        {
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(WeightUnitType.kilogram)]
        public WeightUnitType Unit
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public enum WeightUnitType
    {

        /// <remarks/>
        kilogram,

        /// <remarks/>
        ton,

        /// <remarks/>
        pound,

        /// <remarks/>
        hundredweight,

        /// <remarks/>
        tonne,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public partial class DistanceStructure
    {

        public DistanceStructure()
        {
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(DistanceUnitType.metre)]
        public DistanceUnitType Unit
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public enum DistanceUnitType
    {

        /// <remarks/>
        metre,

        /// <remarks/>
        kilometre,

        /// <remarks/>
        millimetre,

        /// <remarks/>
        centimetre,

        /// <remarks/>
        inch,

        /// <remarks/>
        foot,

        /// <remarks/>
        yard,

        /// <remarks/>
        mile,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public partial class SpeedStructure
    {

        public SpeedStructure()
        {
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SpeedUnitType.mph)]
        public SpeedUnitType Unit
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
    public enum SpeedUnitType
    {

        /// <remarks/>
        mph,

        /// <remarks/>
        kph,
    }
}
