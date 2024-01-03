﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.DocumentsAndContents
{
    public class ReProposalParams
    {
        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        [System.Xml.Serialization.XmlRootAttribute("BS7666Address", Namespace = "http://www.govtalk.gov.uk/people/bs7666", IsNullable = false)]
        public partial class BSaddressStructure
        {

            private AONstructure sAONField;

            private AONstructure pAONField;

            private string streetDescriptionField;

            private string uniqueStreetReferenceNumberField;

            private string[] itemsField;

            private ItemsChoiceType1[] itemsElementNameField;

            private string postTownField;

            private string postCodeField;

            private string uniquePropertyReferenceNumberField;

            /// <remarks/>
            public AONstructure SAON
            {
                get
                {
                    return this.sAONField;
                }
                set
                {
                    this.sAONField = value;
                }
            }

            /// <remarks/>
            public AONstructure PAON
            {
                get
                {
                    return this.pAONField;
                }
                set
                {
                    this.pAONField = value;
                }
            }

            /// <remarks/>
            public string StreetDescription
            {
                get
                {
                    return this.streetDescriptionField;
                }
                set
                {
                    this.streetDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string UniqueStreetReferenceNumber
            {
                get
                {
                    return this.uniqueStreetReferenceNumberField;
                }
                set
                {
                    this.uniqueStreetReferenceNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AdministrativeArea", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Locality", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Town", typeof(string))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
            public string[] Items
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
            [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
            [System.Xml.Serialization.XmlIgnoreAttribute()]
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
            public string PostTown
            {
                get
                {
                    return this.postTownField;
                }
                set
                {
                    this.postTownField = value;
                }
            }

            /// <remarks/>
            public string PostCode
            {
                get
                {
                    return this.postCodeField;
                }
                set
                {
                    this.postCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string UniquePropertyReferenceNumber
            {
                get
                {
                    return this.uniquePropertyReferenceNumberField;
                }
                set
                {
                    this.uniquePropertyReferenceNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class AONstructure
        {

            private object[] itemsField;

            private ItemsChoiceType[] itemsElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Description", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("EndRange", typeof(AONrangeStructure))]
            [System.Xml.Serialization.XmlElementAttribute("StartRange", typeof(AONrangeStructure))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
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
            [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
            [System.Xml.Serialization.XmlIgnoreAttribute()]
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
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class AONrangeStructure
        {

            private string numberField;

            private string suffixField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
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

            /// <remarks/>
            public string Suffix
            {
                get
                {
                    return this.suffixField;
                }
                set
                {
                    this.suffixField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class BLPUpolygonStructure
        {

            private string polygonIDField;

            private BLPUpolygonStructurePolygonType polygonTypeField;

            private bool polygonTypeFieldSpecified;

            private object[] itemsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
            public string PolygonID
            {
                get
                {
                    return this.polygonIDField;
                }
                set
                {
                    this.polygonIDField = value;
                }
            }

            /// <remarks/>
            public BLPUpolygonStructurePolygonType PolygonType
            {
                get
                {
                    return this.polygonTypeField;
                }
                set
                {
                    this.polygonTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool PolygonTypeSpecified
            {
                get
                {
                    return this.polygonTypeFieldSpecified;
                }
                set
                {
                    this.polygonTypeFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ExternalRef", typeof(ulong))]
            [System.Xml.Serialization.XmlElementAttribute("Vertices", typeof(CoordinateStructure))]
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
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public enum BLPUpolygonStructurePolygonType
        {

            /// <remarks/>
            H,
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(GridReferenceStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class CoordinateStructure
        {

            private string xField;

            private string yField;

            /// <remarks/>
            public string X
            {
                get
                {
                    return this.xField;
                }
                set
                {
                    this.xField = value;
                }
            }

            /// <remarks/>
            public string Y
            {
                get
                {
                    return this.yField;
                }
                set
                {
                    this.yField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class GridReferenceStructure : CoordinateStructure
        {
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class BLPUextentStructure
        {

            private string sourceDescriptionField;

            private string extentEntryDateField;

            private string extentSourceDateField;

            private string extentStartDateField;

            private string extentEndDateField;

            private bool extentEndDateFieldSpecified;

            private string extentLastUpdateDateField;

            private BLPUpolygonStructure[] extentDefinitionField;

            /// <remarks/>
            public string SourceDescription
            {
                get
                {
                    return this.sourceDescriptionField;
                }
                set
                {
                    this.sourceDescriptionField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ExtentEntryDate
            {
                get
                {
                    return this.extentEntryDateField;
                }
                set
                {
                    this.extentEntryDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ExtentSourceDate
            {
                get
                {
                    return this.extentSourceDateField;
                }
                set
                {
                    this.extentSourceDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ExtentStartDate
            {
                get
                {
                    return this.extentStartDateField;
                }
                set
                {
                    this.extentStartDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ExtentEndDate
            {
                get
                {
                    return this.extentEndDateField;
                }
                set
                {
                    this.extentEndDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ExtentEndDateSpecified
            {
                get
                {
                    return this.extentEndDateFieldSpecified;
                }
                set
                {
                    this.extentEndDateFieldSpecified = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ExtentLastUpdateDate
            {
                get
                {
                    return this.extentLastUpdateDateField;
                }
                set
                {
                    this.extentLastUpdateDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ExtentDefinition")]
            public BLPUpolygonStructure[] ExtentDefinition
            {
                get
                {
                    return this.extentDefinitionField;
                }
                set
                {
                    this.extentDefinitionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class ProvenanceStructure
        {

            private ProvenanceCodeType provenanceCodeField;

            private string annotationField;

            private string provEntryDateField;

            private string provStartDateField;

            private string provEndDateField;

            private bool provEndDateFieldSpecified;

            private string provLastUpdateDateField;

            private BLPUextentStructure bLPUextentField;

            /// <remarks/>
            public ProvenanceCodeType ProvenanceCode
            {
                get
                {
                    return this.provenanceCodeField;
                }
                set
                {
                    this.provenanceCodeField = value;
                }
            }

            /// <remarks/>
            public string Annotation
            {
                get
                {
                    return this.annotationField;
                }
                set
                {
                    this.annotationField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ProvEntryDate
            {
                get
                {
                    return this.provEntryDateField;
                }
                set
                {
                    this.provEntryDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ProvStartDate
            {
                get
                {
                    return this.provStartDateField;
                }
                set
                {
                    this.provStartDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ProvEndDate
            {
                get
                {
                    return this.provEndDateField;
                }
                set
                {
                    this.provEndDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ProvEndDateSpecified
            {
                get
                {
                    return this.provEndDateFieldSpecified;
                }
                set
                {
                    this.provEndDateFieldSpecified = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ProvLastUpdateDate
            {
                get
                {
                    return this.provLastUpdateDateField;
                }
                set
                {
                    this.provLastUpdateDateField = value;
                }
            }

            /// <remarks/>
            public BLPUextentStructure BLPUextent
            {
                get
                {
                    return this.bLPUextentField;
                }
                set
                {
                    this.bLPUextentField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public enum ProvenanceCodeType
        {

            /// <remarks/>
            T,

            /// <remarks/>
            L,

            /// <remarks/>
            F,

            /// <remarks/>
            R,

            /// <remarks/>
            P,

            /// <remarks/>
            O,

            /// <remarks/>
            U,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class ElementaryStreetUnitStructure
        {

            private string eSUidentityField;

            private string eSUversionField;

            private string eSUentryDateField;

            private string eSUclosureDateField;

            private bool eSUclosureDateFieldSpecified;

            private CoordinateStructure startCoordinateField;

            private CoordinateStructure endCoordinateField;

            private string toleranceField;

            private CoordinateStructure[] intermediateCoordField;

            /// <remarks/>
            public string ESUidentity
            {
                get
                {
                    return this.eSUidentityField;
                }
                set
                {
                    this.eSUidentityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
            public string ESUversion
            {
                get
                {
                    return this.eSUversionField;
                }
                set
                {
                    this.eSUversionField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ESUentryDate
            {
                get
                {
                    return this.eSUentryDateField;
                }
                set
                {
                    this.eSUentryDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string ESUclosureDate
            {
                get
                {
                    return this.eSUclosureDateField;
                }
                set
                {
                    this.eSUclosureDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ESUclosureDateSpecified
            {
                get
                {
                    return this.eSUclosureDateFieldSpecified;
                }
                set
                {
                    this.eSUclosureDateFieldSpecified = value;
                }
            }

            /// <remarks/>
            public CoordinateStructure StartCoordinate
            {
                get
                {
                    return this.startCoordinateField;
                }
                set
                {
                    this.startCoordinateField = value;
                }
            }

            /// <remarks/>
            public CoordinateStructure EndCoordinate
            {
                get
                {
                    return this.endCoordinateField;
                }
                set
                {
                    this.endCoordinateField = value;
                }
            }

            /// <remarks/>
            public string Tolerance
            {
                get
                {
                    return this.toleranceField;
                }
                set
                {
                    this.toleranceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("IntermediateCoord")]
            public CoordinateStructure[] IntermediateCoord
            {
                get
                {
                    return this.intermediateCoordField;
                }
                set
                {
                    this.intermediateCoordField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class StreetDescriptiveIdentifierStructure
        {

            private string streetDescriptionField;

            private string[] itemsField;

            private ItemsChoiceType2[] itemsElementNameField;

            /// <remarks/>
            public string StreetDescription
            {
                get
                {
                    return this.streetDescriptionField;
                }
                set
                {
                    this.streetDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AdministrativeArea", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Locality", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Town", typeof(string))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
            public string[] Items
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
            [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemsChoiceType2[] ItemsElementName
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
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666", IncludeInSchema = false)]
        public enum ItemsChoiceType2
        {

            /// <remarks/>
            AdministrativeArea,

            /// <remarks/>
            Locality,

            /// <remarks/>
            Town,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class StreetStructure
        {

            private StreetReferenceTypeType streetReferenceTypeField;

            private CoordinateStructure startCoordinateField;

            private CoordinateStructure endCoordinateField;

            private string toleranceField;

            private string streetVersionNumberField;

            private string streetEntryDateField;

            private string streetClosureDateField;

            private bool streetClosureDateFieldSpecified;

            private string responsibleAuthorityField;

            private StreetDescriptiveIdentifierStructure descriptiveIdentifierField;

            private StreetDescriptiveIdentifierStructure streetAliasField;

            private StreetStructureStreetCrossReferences streetCrossReferencesField;

            private string usrnField;

            /// <remarks/>
            public StreetReferenceTypeType StreetReferenceType
            {
                get
                {
                    return this.streetReferenceTypeField;
                }
                set
                {
                    this.streetReferenceTypeField = value;
                }
            }

            /// <remarks/>
            public CoordinateStructure StartCoordinate
            {
                get
                {
                    return this.startCoordinateField;
                }
                set
                {
                    this.startCoordinateField = value;
                }
            }

            /// <remarks/>
            public CoordinateStructure EndCoordinate
            {
                get
                {
                    return this.endCoordinateField;
                }
                set
                {
                    this.endCoordinateField = value;
                }
            }

            /// <remarks/>
            public string Tolerance
            {
                get
                {
                    return this.toleranceField;
                }
                set
                {
                    this.toleranceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
            public string StreetVersionNumber
            {
                get
                {
                    return this.streetVersionNumberField;
                }
                set
                {
                    this.streetVersionNumberField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string StreetEntryDate
            {
                get
                {
                    return this.streetEntryDateField;
                }
                set
                {
                    this.streetEntryDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string StreetClosureDate
            {
                get
                {
                    return this.streetClosureDateField;
                }
                set
                {
                    this.streetClosureDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool StreetClosureDateSpecified
            {
                get
                {
                    return this.streetClosureDateFieldSpecified;
                }
                set
                {
                    this.streetClosureDateFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string ResponsibleAuthority
            {
                get
                {
                    return this.responsibleAuthorityField;
                }
                set
                {
                    this.responsibleAuthorityField = value;
                }
            }

            /// <remarks/>
            public StreetDescriptiveIdentifierStructure DescriptiveIdentifier
            {
                get
                {
                    return this.descriptiveIdentifierField;
                }
                set
                {
                    this.descriptiveIdentifierField = value;
                }
            }

            /// <remarks/>
            public StreetDescriptiveIdentifierStructure StreetAlias
            {
                get
                {
                    return this.streetAliasField;
                }
                set
                {
                    this.streetAliasField = value;
                }
            }

            /// <remarks/>
            public StreetStructureStreetCrossReferences StreetCrossReferences
            {
                get
                {
                    return this.streetCrossReferencesField;
                }
                set
                {
                    this.streetCrossReferencesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string usrn
            {
                get
                {
                    return this.usrnField;
                }
                set
                {
                    this.usrnField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public enum StreetReferenceTypeType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("1")]
            Item1,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("2")]
            Item2,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("3")]
            Item3,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("4")]
            Item4,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class StreetStructureStreetCrossReferences
        {

            private object[] itemsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ElementaryStreetUnit", typeof(ElementaryStreetUnitStructure))]
            [System.Xml.Serialization.XmlElementAttribute("UniqueStreetReferenceNumbers", typeof(string), DataType = "integer")]
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
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public partial class LandAndPropertyIdentifierStructure
        {

            private AONstructure pAONField;

            private AONstructure sAONField;

            private string postTownField;

            private string postCodeField;

            private string levelField;

            private LogicalStatusType logicalStatusField;

            private bool officialAddressMarkerField;

            private bool officialAddressMarkerFieldSpecified;

            private string lPIstartDateField;

            private string lPIentryDateField;

            private string lPIendDateField;

            private bool lPIendDateFieldSpecified;

            private string lPIlastUpdateDateField;

            private object itemField;

            /// <remarks/>
            public AONstructure PAON
            {
                get
                {
                    return this.pAONField;
                }
                set
                {
                    this.pAONField = value;
                }
            }

            /// <remarks/>
            public AONstructure SAON
            {
                get
                {
                    return this.sAONField;
                }
                set
                {
                    this.sAONField = value;
                }
            }

            /// <remarks/>
            public string PostTown
            {
                get
                {
                    return this.postTownField;
                }
                set
                {
                    this.postTownField = value;
                }
            }

            /// <remarks/>
            public string PostCode
            {
                get
                {
                    return this.postCodeField;
                }
                set
                {
                    this.postCodeField = value;
                }
            }

            /// <remarks/>
            public string Level
            {
                get
                {
                    return this.levelField;
                }
                set
                {
                    this.levelField = value;
                }
            }

            /// <remarks/>
            public LogicalStatusType LogicalStatus
            {
                get
                {
                    return this.logicalStatusField;
                }
                set
                {
                    this.logicalStatusField = value;
                }
            }

            /// <remarks/>
            public bool OfficialAddressMarker
            {
                get
                {
                    return this.officialAddressMarkerField;
                }
                set
                {
                    this.officialAddressMarkerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OfficialAddressMarkerSpecified
            {
                get
                {
                    return this.officialAddressMarkerFieldSpecified;
                }
                set
                {
                    this.officialAddressMarkerFieldSpecified = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string LPIstartDate
            {
                get
                {
                    return this.lPIstartDateField;
                }
                set
                {
                    this.lPIstartDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string LPIentryDate
            {
                get
                {
                    return this.lPIentryDateField;
                }
                set
                {
                    this.lPIentryDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string LPIendDate
            {
                get
                {
                    return this.lPIendDateField;
                }
                set
                {
                    this.lPIendDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LPIendDateSpecified
            {
                get
                {
                    return this.lPIendDateFieldSpecified;
                }
                set
                {
                    this.lPIendDateFieldSpecified = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string LPIlastUpdateDate
            {
                get
                {
                    return this.lPIlastUpdateDateField;
                }
                set
                {
                    this.lPIlastUpdateDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Street", typeof(StreetStructure))]
            [System.Xml.Serialization.XmlElementAttribute("USRN", typeof(string), DataType = "integer")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public enum LogicalStatusType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("1")]
            Item1,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("2")]
            Item2,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("3")]
            Item3,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("5")]
            Item5,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("6")]
            Item6,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("8")]
            Item8,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("9")]
            Item9,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666", IncludeInSchema = false)]
        public enum ItemsChoiceType
        {

            /// <remarks/>
            Description,

            /// <remarks/>
            EndRange,

            /// <remarks/>
            StartRange,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666", IncludeInSchema = false)]
        public enum ItemsChoiceType1
        {

            /// <remarks/>
            AdministrativeArea,

            /// <remarks/>
            Locality,

            /// <remarks/>
            Town,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        [System.Xml.Serialization.XmlRootAttribute("BS7666BLPU", Namespace = "http://www.govtalk.gov.uk/people/bs7666", IsNullable = false)]
        public partial class BasicLandAndPropertyUnitStructure
        {

            private string uniquePropertyReferenceNumberField;

            private string custodianCodeField;

            private RepresentativePointCodeType representativePointCodeField;

            private LogicalStatusType logicalStatusField;

            private CoordinateStructure gridReferenceField;

            private string bLPUentryDateField;

            private string bLPUstartDateField;

            private string bLPUendDateField;

            private bool bLPUendDateFieldSpecified;

            private string bLPUlastUpdateDateField;

            private LandAndPropertyIdentifierStructure[] landAndPropertyIdentifierField;

            private ProvenanceStructure[] provenanceField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string UniquePropertyReferenceNumber
            {
                get
                {
                    return this.uniquePropertyReferenceNumberField;
                }
                set
                {
                    this.uniquePropertyReferenceNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string CustodianCode
            {
                get
                {
                    return this.custodianCodeField;
                }
                set
                {
                    this.custodianCodeField = value;
                }
            }

            /// <remarks/>
            public RepresentativePointCodeType RepresentativePointCode
            {
                get
                {
                    return this.representativePointCodeField;
                }
                set
                {
                    this.representativePointCodeField = value;
                }
            }

            /// <remarks/>
            public LogicalStatusType LogicalStatus
            {
                get
                {
                    return this.logicalStatusField;
                }
                set
                {
                    this.logicalStatusField = value;
                }
            }

            /// <remarks/>
            public CoordinateStructure GridReference
            {
                get
                {
                    return this.gridReferenceField;
                }
                set
                {
                    this.gridReferenceField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string BLPUentryDate
            {
                get
                {
                    return this.bLPUentryDateField;
                }
                set
                {
                    this.bLPUentryDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string BLPUstartDate
            {
                get
                {
                    return this.bLPUstartDateField;
                }
                set
                {
                    this.bLPUstartDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string BLPUendDate
            {
                get
                {
                    return this.bLPUendDateField;
                }
                set
                {
                    this.bLPUendDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool BLPUendDateSpecified
            {
                get
                {
                    return this.bLPUendDateFieldSpecified;
                }
                set
                {
                    this.bLPUendDateFieldSpecified = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string BLPUlastUpdateDate
            {
                get
                {
                    return this.bLPUlastUpdateDateField;
                }
                set
                {
                    this.bLPUlastUpdateDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("LandAndPropertyIdentifier")]
            public LandAndPropertyIdentifierStructure[] LandAndPropertyIdentifier
            {
                get
                {
                    return this.landAndPropertyIdentifierField;
                }
                set
                {
                    this.landAndPropertyIdentifierField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Provenance")]
            public ProvenanceStructure[] Provenance
            {
                get
                {
                    return this.provenanceField;
                }
                set
                {
                    this.provenanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.govtalk.gov.uk/people/bs7666")]
        public enum RepresentativePointCodeType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("1")]
            Item1,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("2")]
            Item2,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("3")]
            Item3,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("4")]
            Item4,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("5")]
            Item5,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("9")]
            Item9,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/mtp")]
        [System.Xml.Serialization.XmlRootAttribute("UnresolvedMTP", Namespace = "http://www.esdal.com/schemas/core/mtp", IsNullable = false)]
        public partial class UnresolvedMTPStructure : MTPStructure
        {

            private ContactReferenceStructure[] contactField;

            private bool isDeletedField;

            public UnresolvedMTPStructure()
            {
                this.isDeletedField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ContactReferenceStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsDeleted
            {
                get
                {
                    return this.isDeletedField;
                }
                set
                {
                    this.isDeletedField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class ContactReferenceStructure
        {

            private string descriptionField;

            private ContactReferenceChoiceStructure contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public ContactReferenceChoiceStructure Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class ContactReferenceChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AdhocReference", typeof(AdhocContactReferenceStructure))]
            [System.Xml.Serialization.XmlElementAttribute("RoleReference", typeof(RoleContactReferenceStructure))]
            [System.Xml.Serialization.XmlElementAttribute("SimpleReference", typeof(SimpleContactReferenceStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class AdhocContactReferenceStructure
        {

            private string fullNameField;

            private string organisationNameField;

            private AddressStructure addressField;

            private string telephoneNumberField;

            private string telephoneExtensionField;

            private string mobileNumberField;

            private string faxNumberField;

            private string emailAddressField;

            private EmailFormatType emailFormatPreferenceField;

            private bool emailFormatPreferenceFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string FullName
            {
                get
                {
                    return this.fullNameField;
                }
                set
                {
                    this.fullNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            public AddressStructure Address
            {
                get
                {
                    return this.addressField;
                }
                set
                {
                    this.addressField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            public string TelephoneExtension
            {
                get
                {
                    return this.telephoneExtensionField;
                }
                set
                {
                    this.telephoneExtensionField = value;
                }
            }

            /// <remarks/>
            public string MobileNumber
            {
                get
                {
                    return this.mobileNumberField;
                }
                set
                {
                    this.mobileNumberField = value;
                }
            }

            /// <remarks/>
            public string FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            public EmailFormatType EmailFormatPreference
            {
                get
                {
                    return this.emailFormatPreferenceField;
                }
                set
                {
                    this.emailFormatPreferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool EmailFormatPreferenceSpecified
            {
                get
                {
                    return this.emailFormatPreferenceFieldSpecified;
                }
                set
                {
                    this.emailFormatPreferenceFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class AddressStructure : UKPostalAddressStructure
        {

            private CountryType countryField;

            private bool countryFieldSpecified;

            /// <remarks/>
            public CountryType Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool CountrySpecified
            {
                get
                {
                    return this.countryFieldSpecified;
                }
                set
                {
                    this.countryFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public enum CountryType
        {

            /// <remarks/>
            england,

            /// <remarks/>
            wales,

            /// <remarks/>
            scotland,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("northern ireland")]
            northernireland,
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AddressStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class UKPostalAddressStructure
        {

            private string[] lineField;

            private string postCodeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Line")]
            public string[] Line
            {
                get
                {
                    return this.lineField;
                }
                set
                {
                    this.lineField = value;
                }
            }

            /// <remarks/>
            public string PostCode
            {
                get
                {
                    return this.postCodeField;
                }
                set
                {
                    this.postCodeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public enum EmailFormatType
        {

            /// <remarks/>
            html,

            /// <remarks/>
            text,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class RoleContactReferenceStructure
        {

            private RoleType roleWithinOganisationField;

            private string organisationNameField;

            private string resolvedFullNameField;

            private string organisationIdField;

            private string resolvedContactIdField;

            private bool resolvedContactIdFieldSpecified;

            /// <remarks/>
            public RoleType RoleWithinOganisation
            {
                get
                {
                    return this.roleWithinOganisationField;
                }
                set
                {
                    this.roleWithinOganisationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string ResolvedFullName
            {
                get
                {
                    return this.resolvedFullNameField;
                }
                set
                {
                    this.resolvedFullNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ResolvedContactId
            {
                get
                {
                    return this.resolvedContactIdField;
                }
                set
                {
                    this.resolvedContactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ResolvedContactIdSpecified
            {
                get
                {
                    return this.resolvedContactIdFieldSpecified;
                }
                set
                {
                    this.resolvedContactIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public enum RoleType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("data holder")]
            dataholder,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("notification contact")]
            notificationcontact,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("official contact")]
            officialcontact,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("police alo")]
            policealo,

            /// <remarks/>
            haulier,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("it contact")]
            itcontact,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("default contact")]
            defaultcontact,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("data owner")]
            dataowner,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class SimpleContactReferenceStructure
        {

            private string fullNameField;

            private string organisationNameField;

            private string contactIdField;

            private string organisationIdField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string FullName
            {
                get
                {
                    return this.fullNameField;
                }
                set
                {
                    this.fullNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactId
            {
                get
                {
                    return this.contactIdField;
                }
                set
                {
                    this.contactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolvedMTPStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnresolvedMTPStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/mtp")]
        public abstract partial class MTPStructure
        {

            private string mTPRNField;

            private MTPModeType modeField;

            private string locationField;

            private string nameField;

            private string descriptionField;

            /// <remarks/>
            public string MTPRN
            {
                get
                {
                    return this.mTPRNField;
                }
                set
                {
                    this.mTPRNField = value;
                }
            }

            /// <remarks/>
            public MTPModeType Mode
            {
                get
                {
                    return this.modeField;
                }
                set
                {
                    this.modeField = value;
                }
            }

            /// <remarks/>
            public string Location
            {
                get
                {
                    return this.locationField;
                }
                set
                {
                    this.locationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/mtp")]
        public enum MTPModeType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("road-water")]
            roadwater,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("road-rail")]
            roadrail,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("road-air")]
            roadair,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/mtp")]
        [System.Xml.Serialization.XmlRootAttribute("ResolvedMTP", Namespace = "http://www.esdal.com/schemas/core/mtp", IsNullable = false)]
        public partial class ResolvedMTPStructure : MTPStructure
        {

            private ResolvedContactStructure[] contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ResolvedContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class ResolvedContactStructure
        {

            private string descriptionField;

            private string fullNameField;

            private string organisationNameField;

            private RoleType roleField;

            private bool roleFieldSpecified;

            private AddressStructure addressField;

            private string telephoneNumberField;

            private string telephoneExtensionField;

            private string mobileNumberField;

            private string faxNumberField;

            private string emailAddressField;

            private EmailFormatType emailFormatPreferenceField;

            private bool emailFormatPreferenceFieldSpecified;

            private string contactIdField;

            private bool contactIdFieldSpecified;

            private string organisationIdField;

            private bool organisationIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string FullName
            {
                get
                {
                    return this.fullNameField;
                }
                set
                {
                    this.fullNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            public RoleType Role
            {
                get
                {
                    return this.roleField;
                }
                set
                {
                    this.roleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RoleSpecified
            {
                get
                {
                    return this.roleFieldSpecified;
                }
                set
                {
                    this.roleFieldSpecified = value;
                }
            }

            /// <remarks/>
            public AddressStructure Address
            {
                get
                {
                    return this.addressField;
                }
                set
                {
                    this.addressField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            public string TelephoneExtension
            {
                get
                {
                    return this.telephoneExtensionField;
                }
                set
                {
                    this.telephoneExtensionField = value;
                }
            }

            /// <remarks/>
            public string MobileNumber
            {
                get
                {
                    return this.mobileNumberField;
                }
                set
                {
                    this.mobileNumberField = value;
                }
            }

            /// <remarks/>
            public string FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            public EmailFormatType EmailFormatPreference
            {
                get
                {
                    return this.emailFormatPreferenceField;
                }
                set
                {
                    this.emailFormatPreferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool EmailFormatPreferenceSpecified
            {
                get
                {
                    return this.emailFormatPreferenceFieldSpecified;
                }
                set
                {
                    this.emailFormatPreferenceFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactId
            {
                get
                {
                    return this.contactIdField;
                }
                set
                {
                    this.contactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ContactIdSpecified
            {
                get
                {
                    return this.contactIdFieldSpecified;
                }
                set
                {
                    this.contactIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIdSpecified
            {
                get
                {
                    return this.organisationIdFieldSpecified;
                }
                set
                {
                    this.organisationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
        [System.Xml.Serialization.XmlRootAttribute("UnresolvedAnnotations", Namespace = "http://www.esdal.com/schemas/core/annotation", IsNullable = false)]
        public partial class UnresolvedAnnotationsStructure
        {

            private UnresolvedAnnotationStructure[] unresolvedAnnotationField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("UnresolvedAnnotation")]
            public UnresolvedAnnotationStructure[] UnresolvedAnnotation
            {
                get
                {
                    return this.unresolvedAnnotationField;
                }
                set
                {
                    this.unresolvedAnnotationField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(RouteAnnotationStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
        [System.Xml.Serialization.XmlRootAttribute("UnresolvedAnnotation", Namespace = "http://www.esdal.com/schemas/core/annotation", IsNullable = false)]
        public partial class UnresolvedAnnotationStructure : AnnotationStructure
        {

            private ContactReferenceStructure[] contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ContactReferenceStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintAnnotationContactReasonStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureAnnotationContactReasonStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnnotationContactReasonStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolvedAnnotationStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnalysedAnnotationStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnresolvedAnnotationStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(RouteAnnotationStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
        public abstract partial class AnnotationStructure
        {

            private SimpleTextStructure textField;

            private string annotationIdField;

            private bool annotationIdFieldSpecified;

            private AnnotationType annotationTypeField;

            public AnnotationStructure()
            {
                this.annotationTypeField = AnnotationType.generic;
            }

            /// <remarks/>
            public SimpleTextStructure Text
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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AnnotationId
            {
                get
                {
                    return this.annotationIdField;
                }
                set
                {
                    this.annotationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AnnotationIdSpecified
            {
                get
                {
                    return this.annotationIdFieldSpecified;
                }
                set
                {
                    this.annotationIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(AnnotationType.generic)]
            public AnnotationType AnnotationType
            {
                get
                {
                    return this.annotationTypeField;
                }
                set
                {
                    this.annotationTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public partial class SimpleTextStructure : LevelTwoTextStructure
        {
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleTextStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelTwoLetteredStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelTwoNumberedStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public partial class LevelTwoTextStructure
        {

            private LevelTwoTextStructure[] itemsField;

            private ItemsChoiceType3[] itemsElementNameField;

            private string[] textField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Bold", typeof(LevelTwoTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Italic", typeof(LevelTwoTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Underscore", typeof(LevelTwoTextStructure))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
            public LevelTwoTextStructure[] Items
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
            [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemsChoiceType3[] ItemsElementName
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
            [System.Xml.Serialization.XmlTextAttribute()]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IncludeInSchema = false)]
        public enum ItemsChoiceType3
        {

            /// <remarks/>
            Bold,

            /// <remarks/>
            Italic,

            /// <remarks/>
            Underscore,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public abstract partial class LevelTwoLetteredStructure : LevelTwoTextStructure
        {

            private string letterField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public abstract partial class LevelTwoNumberedStructure : LevelTwoTextStructure
        {

            private string numberField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
        public enum AnnotationType
        {

            /// <remarks/>
            generic,

            /// <remarks/>
            caution,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("special manouevre")]
            specialmanouevre,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class ConstraintAnnotationContactReasonStructure : AnnotationStructure
        {

            private string eCRNField;

            private ConstraintType typeField;

            private string nameField;

            /// <remarks/>
            public string ECRN
            {
                get
                {
                    return this.eCRNField;
                }
                set
                {
                    this.eCRNField = value;
                }
            }

            /// <remarks/>
            public ConstraintType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
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
            [System.Xml.Serialization.XmlEnumAttribute("tight bend")]
            tightbend,

            /// <remarks/>
            @event,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("risk of grounding")]
            riskofgrounding,

            /// <remarks/>
            unmade,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("natural void")]
            naturalvoid,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("manmade void")]
            manmadevoid,

            /// <remarks/>
            tunnel,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("tunnel void")]
            tunnelvoid,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("pipes and ducts")]
            pipesandducts,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("retaining wall")]
            retainingwall,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("traffic calming")]
            trafficcalming,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("overhead building")]
            overheadbuilding,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("overhead pipes and utilities")]
            overheadpipesandutilities,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("adjacent retaining wall")]
            adjacentretainingwall,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("power cable")]
            powercable,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("electrification cable")]
            electrificationcable,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("telecomms cable")]
            telecommscable,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("gantry road furniture")]
            gantryroadfurniture,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("cantilever road furniture")]
            cantileverroadfurniture,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("catenary road furniture")]
            catenaryroadfurniture,

            /// <remarks/>
            bollard,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("removable bollard")]
            removablebollard,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class StructureAnnotationContactReasonStructure : AnnotationStructure
        {

            private string eSRNField;

            private string nameField;

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnnotationContactReasonStructure : AnnotationStructure
        {

            private RoadIdentificationStructure roadField;

            /// <remarks/>
            public RoadIdentificationStructure Road
            {
                get
                {
                    return this.roadField;
                }
                set
                {
                    this.roadField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class RoadIdentificationStructure
        {

            private string nameField;

            private string numberField;

            private bool unidentifiedField;

            public RoadIdentificationStructure()
            {
                this.unidentifiedField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool Unidentified
            {
                get
                {
                    return this.unidentifiedField;
                }
                set
                {
                    this.unidentifiedField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnalysedAnnotationStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
        public partial class ResolvedAnnotationStructure : AnnotationStructure
        {

            private ResolvedContactStructure[] contactField;

            private bool isDrivingInstructionAnnotationField;

            public ResolvedAnnotationStructure()
            {
                this.isDrivingInstructionAnnotationField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ResolvedContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsDrivingInstructionAnnotation
            {
                get
                {
                    return this.isDrivingInstructionAnnotationField;
                }
                set
                {
                    this.isDrivingInstructionAnnotationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedAnnotationStructure : ResolvedAnnotationStructure
        {

            private RoadIdentificationStructure roadField;

            private AnalysedAnnotationChoiceStructure annotatedEntityField;

            /// <remarks/>
            public RoadIdentificationStructure Road
            {
                get
                {
                    return this.roadField;
                }
                set
                {
                    this.roadField = value;
                }
            }

            /// <remarks/>
            public AnalysedAnnotationChoiceStructure AnnotatedEntity
            {
                get
                {
                    return this.annotatedEntityField;
                }
                set
                {
                    this.annotatedEntityField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedAnnotationChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Constraint", typeof(AnalysedAnnotationConstraintStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Road", typeof(AnalysedAnnotationRoadStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Structure", typeof(AnalysedAnnotationStructureStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedAnnotationConstraintStructure
        {

            private string eCRNField;

            private ConstraintType typeField;

            private string nameField;

            /// <remarks/>
            public string ECRN
            {
                get
                {
                    return this.eCRNField;
                }
                set
                {
                    this.eCRNField = value;
                }
            }

            /// <remarks/>
            public ConstraintType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedAnnotationRoadStructure
        {

            private string oSGridRefField;

            /// <remarks/>
            public string OSGridRef
            {
                get
                {
                    return this.oSGridRefField;
                }
                set
                {
                    this.oSGridRefField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedAnnotationStructureStructure
        {

            private string eSRNField;

            private string nameField;

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        [System.Xml.Serialization.XmlRootAttribute("RouteAnnotation", Namespace = "http://www.esdal.com/schemas/core/annotation", IsNullable = false)]
        public partial class RouteAnnotationStructure : UnresolvedAnnotationStructure
        {

            private SegmentPointStructure segmentPointField;

            private AnnotationAssociationType associationField;

            private bool associationFieldSpecified;

            private string structureField;

            private string constraintField;

            /// <remarks/>
            public SegmentPointStructure SegmentPoint
            {
                get
                {
                    return this.segmentPointField;
                }
                set
                {
                    this.segmentPointField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public AnnotationAssociationType Association
            {
                get
                {
                    return this.associationField;
                }
                set
                {
                    this.associationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AssociationSpecified
            {
                get
                {
                    return this.associationFieldSpecified;
                }
                set
                {
                    this.associationFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Structure
            {
                get
                {
                    return this.structureField;
                }
                set
                {
                    this.structureField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Constraint
            {
                get
                {
                    return this.constraintField;
                }
                set
                {
                    this.constraintField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class SegmentPointStructure
        {

            private string roadSectionIDField;

            private bool roadSectionIDFieldSpecified;

            private string linearReferenceField;

            private bool positiveDirectionField;

            private bool positiveDirectionFieldSpecified;

            private string gridRefField;

            private bool isBrokenField;

            public SegmentPointStructure()
            {
                this.isBrokenField = false;
            }

            /// <remarks/>
            public string RoadSectionID
            {
                get
                {
                    return this.roadSectionIDField;
                }
                set
                {
                    this.roadSectionIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RoadSectionIDSpecified
            {
                get
                {
                    return this.roadSectionIDFieldSpecified;
                }
                set
                {
                    this.roadSectionIDFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string LinearReference
            {
                get
                {
                    return this.linearReferenceField;
                }
                set
                {
                    this.linearReferenceField = value;
                }
            }

            /// <remarks/>
            public bool PositiveDirection
            {
                get
                {
                    return this.positiveDirectionField;
                }
                set
                {
                    this.positiveDirectionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool PositiveDirectionSpecified
            {
                get
                {
                    return this.positiveDirectionFieldSpecified;
                }
                set
                {
                    this.positiveDirectionFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string GridRef
            {
                get
                {
                    return this.gridRefField;
                }
                set
                {
                    this.gridRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
        [System.Xml.Serialization.XmlRootAttribute("UnresolvedConstraint", Namespace = "http://www.esdal.com/schemas/core/constraint", IsNullable = false)]
        public partial class UnresolvedConstraintStructure : ConstraintStructure
        {

            private ConstraintOwnerStructure ownerField;

            private ContactReferenceStructure[] contactField;

            private object ownerIsContactField;

            private string directionField;

            private ConstraintTopologyType topologyTypeField;

            private ConstraintTraversalType traversalTypeField;

            public UnresolvedConstraintStructure()
            {
                this.topologyTypeField = ConstraintTopologyType.point;
                this.traversalTypeField = ConstraintTraversalType.link;
            }

            /// <remarks/>
            public ConstraintOwnerStructure Owner
            {
                get
                {
                    return this.ownerField;
                }
                set
                {
                    this.ownerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ContactReferenceStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            public object OwnerIsContact
            {
                get
                {
                    return this.ownerIsContactField;
                }
                set
                {
                    this.ownerIsContactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string Direction
            {
                get
                {
                    return this.directionField;
                }
                set
                {
                    this.directionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ConstraintTopologyType.point)]
            public ConstraintTopologyType TopologyType
            {
                get
                {
                    return this.topologyTypeField;
                }
                set
                {
                    this.topologyTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ConstraintTraversalType.link)]
            public ConstraintTraversalType TraversalType
            {
                get
                {
                    return this.traversalTypeField;
                }
                set
                {
                    this.traversalTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public partial class ConstraintOwnerStructure
        {

            private string organisationField;

            private string organisationIdField;

            private bool organisationIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Organisation
            {
                get
                {
                    return this.organisationField;
                }
                set
                {
                    this.organisationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIdSpecified
            {
                get
                {
                    return this.organisationIdFieldSpecified;
                }
                set
                {
                    this.organisationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public enum ConstraintTopologyType
        {

            /// <remarks/>
            point,

            /// <remarks/>
            linear,

            /// <remarks/>
            area,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public enum ConstraintTraversalType
        {

            /// <remarks/>
            node,

            /// <remarks/>
            link,
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnresolvedConstraintStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintContactReasonStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolvedConstraintStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnalysedConstraintStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(VehicleSpecificConstraintStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public abstract partial class ConstraintStructure
        {

            private string eCRNField;

            private ConstraintType typeField;

            private string nameField;

            private ConstraintRestrictionStructure restrictionsField;

            private EntityCautionStructure[] cautionsField;

            /// <remarks/>
            public string ECRN
            {
                get
                {
                    return this.eCRNField;
                }
                set
                {
                    this.eCRNField = value;
                }
            }

            /// <remarks/>
            public ConstraintType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public ConstraintRestrictionStructure Restrictions
            {
                get
                {
                    return this.restrictionsField;
                }
                set
                {
                    this.restrictionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Caution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
            public EntityCautionStructure[] Cautions
            {
                get
                {
                    return this.cautionsField;
                }
                set
                {
                    this.cautionsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public partial class ConstraintRestrictionStructure
        {

            private string heightField;

            private bool heightFieldSpecified;

            private string imperialHeightField;

            private bool imperialHeightFieldSpecified;

            private string widthField;

            private bool widthFieldSpecified;

            private string imperialWidthField;

            private bool imperialWidthFieldSpecified;

            private string lengthField;

            private bool lengthFieldSpecified;

            private string imperialLengthField;

            private bool imperialLengthFieldSpecified;

            private string grossWeightField;

            private bool grossWeightFieldSpecified;

            private string axleWeightField;

            private bool axleWeightFieldSpecified;

            /// <remarks/>
            public string Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool HeightSpecified
            {
                get
                {
                    return this.heightFieldSpecified;
                }
                set
                {
                    this.heightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ImperialHeight
            {
                get
                {
                    return this.imperialHeightField;
                }
                set
                {
                    this.imperialHeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ImperialHeightSpecified
            {
                get
                {
                    return this.imperialHeightFieldSpecified;
                }
                set
                {
                    this.imperialHeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool WidthSpecified
            {
                get
                {
                    return this.widthFieldSpecified;
                }
                set
                {
                    this.widthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ImperialWidth
            {
                get
                {
                    return this.imperialWidthField;
                }
                set
                {
                    this.imperialWidthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ImperialWidthSpecified
            {
                get
                {
                    return this.imperialWidthFieldSpecified;
                }
                set
                {
                    this.imperialWidthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ImperialLength
            {
                get
                {
                    return this.imperialLengthField;
                }
                set
                {
                    this.imperialLengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ImperialLengthSpecified
            {
                get
                {
                    return this.imperialLengthFieldSpecified;
                }
                set
                {
                    this.imperialLengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GrossWeightSpecified
            {
                get
                {
                    return this.grossWeightFieldSpecified;
                }
                set
                {
                    this.grossWeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleWeightSpecified
            {
                get
                {
                    return this.axleWeightFieldSpecified;
                }
                set
                {
                    this.axleWeightFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class EntityCautionStructure : CautionStructure
        {

            private EntityCautionContactChoiceStructure contactsField;

            private bool isApplicableField;

            private bool isApplicableFieldSpecified;

            /// <remarks/>
            public EntityCautionContactChoiceStructure Contacts
            {
                get
                {
                    return this.contactsField;
                }
                set
                {
                    this.contactsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsApplicable
            {
                get
                {
                    return this.isApplicableField;
                }
                set
                {
                    this.isApplicableField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IsApplicableSpecified
            {
                get
                {
                    return this.isApplicableFieldSpecified;
                }
                set
                {
                    this.isApplicableFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class EntityCautionContactChoiceStructure
        {

            private object[] itemsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ResolvedContact", typeof(ResolvedContactStructure))]
            [System.Xml.Serialization.XmlElementAttribute("UnresolvedContact", typeof(ContactReferenceStructure))]
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
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnresolvedCautionStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(CautionContactReasonStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolvedCautionStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnalysedCautionStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityCautionStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public abstract partial class CautionStructure
        {

            private CautionActionStructure actionField;

            private string nameField;

            private CautionedEntityChoiceStructure cautionedEntityField;

            private CautionConditionStructure conditionsField;

            private string cautionIdField;

            private bool cautionIdFieldSpecified;

            /// <remarks/>
            public CautionActionStructure Action
            {
                get
                {
                    return this.actionField;
                }
                set
                {
                    this.actionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public CautionedEntityChoiceStructure CautionedEntity
            {
                get
                {
                    return this.cautionedEntityField;
                }
                set
                {
                    this.cautionedEntityField = value;
                }
            }

            /// <remarks/>
            public CautionConditionStructure Conditions
            {
                get
                {
                    return this.conditionsField;
                }
                set
                {
                    this.conditionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string CautionId
            {
                get
                {
                    return this.cautionIdField;
                }
                set
                {
                    this.cautionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool CautionIdSpecified
            {
                get
                {
                    return this.cautionIdFieldSpecified;
                }
                set
                {
                    this.cautionIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionActionStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SpecificAction", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Standard", typeof(object))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionedEntityChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Constraint", typeof(CautionedConstraintStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Structure", typeof(CautionedStructureStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionedConstraintStructure
        {

            private string eCRNField;

            private ConstraintType typeField;

            private string constraintNameField;

            /// <remarks/>
            public string ECRN
            {
                get
                {
                    return this.eCRNField;
                }
                set
                {
                    this.eCRNField = value;
                }
            }

            /// <remarks/>
            public ConstraintType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string ConstraintName
            {
                get
                {
                    return this.constraintNameField;
                }
                set
                {
                    this.constraintNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionedStructureStructure
        {

            private string eSRNField;

            private string structureNameField;

            private string sectionIdField;

            private bool sectionIdFieldSpecified;

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string StructureName
            {
                get
                {
                    return this.structureNameField;
                }
                set
                {
                    this.structureNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string SectionId
            {
                get
                {
                    return this.sectionIdField;
                }
                set
                {
                    this.sectionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SectionIdSpecified
            {
                get
                {
                    return this.sectionIdFieldSpecified;
                }
                set
                {
                    this.sectionIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionConditionStructure
        {

            private WeightStructure maxGrossWeightField;

            private WeightStructure maxAxleWeightField;

            private DistanceStructure maxHeightField;

            private DistanceStructure maxOverallLengthField;

            private DistanceStructure maxWidthField;

            private SpeedStructure minSpeedField;

            /// <remarks/>
            public WeightStructure MaxGrossWeight
            {
                get
                {
                    return this.maxGrossWeightField;
                }
                set
                {
                    this.maxGrossWeightField = value;
                }
            }

            /// <remarks/>
            public WeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure MaxOverallLength
            {
                get
                {
                    return this.maxOverallLengthField;
                }
                set
                {
                    this.maxOverallLengthField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure MaxWidth
            {
                get
                {
                    return this.maxWidthField;
                }
                set
                {
                    this.maxWidthField = value;
                }
            }

            /// <remarks/>
            public SpeedStructure MinSpeed
            {
                get
                {
                    return this.minSpeedField;
                }
                set
                {
                    this.minSpeedField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class WeightStructure
        {

            private WeightUnitType unitField;

            private string valueField;

            public WeightStructure()
            {
                this.unitField = WeightUnitType.kilogram;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(WeightUnitType.kilogram)]
            public WeightUnitType Unit
            {
                get
                {
                    return this.unitField;
                }
                set
                {
                    this.unitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class DistanceStructure
        {

            private DistanceUnitType unitField;

            private string valueField;

            public DistanceStructure()
            {
                this.unitField = DistanceUnitType.metre;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(DistanceUnitType.metre)]
            public DistanceUnitType Unit
            {
                get
                {
                    return this.unitField;
                }
                set
                {
                    this.unitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class SpeedStructure
        {

            private SpeedUnitType unitField;

            private string valueField;

            public SpeedStructure()
            {
                this.unitField = SpeedUnitType.mph;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(SpeedUnitType.mph)]
            public SpeedUnitType Unit
            {
                get
                {
                    return this.unitField;
                }
                set
                {
                    this.unitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public enum SpeedUnitType
        {

            /// <remarks/>
            mph,

            /// <remarks/>
            kph,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        [System.Xml.Serialization.XmlRootAttribute("UnresolvedCaution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
        public partial class UnresolvedCautionStructure : CautionStructure
        {

            private CautionOwnerStructure ownerField;

            private ContactReferenceStructure[] contactField;

            /// <remarks/>
            public CautionOwnerStructure Owner
            {
                get
                {
                    return this.ownerField;
                }
                set
                {
                    this.ownerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ContactReferenceStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public partial class CautionOwnerStructure
        {

            private string organisationField;

            private string organisationIdField;

            private bool organisationIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Organisation
            {
                get
                {
                    return this.organisationField;
                }
                set
                {
                    this.organisationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIdSpecified
            {
                get
                {
                    return this.organisationIdFieldSpecified;
                }
                set
                {
                    this.organisationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class CautionContactReasonStructure : CautionStructure
        {
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnalysedCautionStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        [System.Xml.Serialization.XmlRootAttribute("ResolvedCaution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
        public partial class ResolvedCautionStructure : CautionStructure
        {

            private ResolvedContactStructure[] contactField;

            private CautionConditionType[] constrainingAttributeField;

            private bool isApplicableField;

            public ResolvedCautionStructure()
            {
                this.isApplicableField = true;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ResolvedContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ConstrainingAttribute")]
            public CautionConditionType[] ConstrainingAttribute
            {
                get
                {
                    return this.constrainingAttributeField;
                }
                set
                {
                    this.constrainingAttributeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(true)]
            public bool IsApplicable
            {
                get
                {
                    return this.isApplicableField;
                }
                set
                {
                    this.isApplicableField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/caution")]
        public enum CautionConditionType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("gross weight")]
            grossweight,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("axle weight")]
            axleweight,

            /// <remarks/>
            height,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("overall length")]
            overalllength,

            /// <remarks/>
            width,

            /// <remarks/>
            speed,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedCautionStructure : ResolvedCautionStructure
        {

            private RoadIdentificationStructure roadField;

            private AnalysedCautionChoiceStructure cautionedEntityField;

            private string[] vehicleField;

            private bool isSuppressedField;

            public AnalysedCautionStructure()
            {
                this.isSuppressedField = false;
            }

            /// <remarks/>
            public RoadIdentificationStructure Road
            {
                get
                {
                    return this.roadField;
                }
                set
                {
                    this.roadField = value;
                }
            }

            /// <remarks/>
            public AnalysedCautionChoiceStructure AnalyzedCautionedEntity
            {
                get
                {
                    return this.cautionedEntityField;
                }
                set
                {
                    this.cautionedEntityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Vehicle", DataType = "token")]
            public string[] Vehicle
            {
                get
                {
                    return this.vehicleField;
                }
                set
                {
                    this.vehicleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsSuppressed
            {
                get
                {
                    return this.isSuppressedField;
                }
                set
                {
                    this.isSuppressedField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedCautionChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Constraint", typeof(AnalysedCautionConstraintStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Structure", typeof(AnalysedCautionStructureStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedCautionConstraintStructure
        {

            private string eCRNField;

            private ConstraintType typeField;

            private string nameField;

            private ResolvedAnnotationStructure[] annotationField;

            /// <remarks/>
            public string ECRN
            {
                get
                {
                    return this.eCRNField;
                }
                set
                {
                    this.eCRNField = value;
                }
            }

            /// <remarks/>
            public ConstraintType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation")]
            public ResolvedAnnotationStructure[] Annotation
            {
                get
                {
                    return this.annotationField;
                }
                set
                {
                    this.annotationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedCautionStructureStructure
        {

            private string eSRNField;

            private string nameField;

            private ResolvedAnnotationStructure[] annotationField;

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation")]
            public ResolvedAnnotationStructure[] Annotation
            {
                get
                {
                    return this.annotationField;
                }
                set
                {
                    this.annotationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class ConstraintContactReasonStructure : ConstraintStructure
        {
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnalysedConstraintStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(VehicleSpecificConstraintStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/constraint")]
        public partial class ResolvedConstraintStructure : ConstraintStructure
        {

            private ResolvedContactStructure[] contactField;

            private bool isApplicableField;

            public ResolvedConstraintStructure()
            {
                this.isApplicableField = true;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ResolvedContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(true)]
            public bool IsApplicable
            {
                get
                {
                    return this.isApplicableField;
                }
                set
                {
                    this.isApplicableField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(VehicleSpecificConstraintStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedConstraintStructure : ResolvedConstraintStructure
        {

            private ResolvedAnnotationStructure[] annotationField;

            private ConstraintSuitabilityStructure appraisalField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation")]
            public ResolvedAnnotationStructure[] Annotation
            {
                get
                {
                    return this.annotationField;
                }
                set
                {
                    this.annotationField = value;
                }
            }

            /// <remarks/>
            public ConstraintSuitabilityStructure Appraisal
            {
                get
                {
                    return this.appraisalField;
                }
                set
                {
                    this.appraisalField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class ConstraintSuitabilityStructure
        {

            private SuitabilityType suitabilityField;

            private string resultDetailsField;

            /// <remarks/>
            public SuitabilityType Suitability
            {
                get
                {
                    return this.suitabilityField;
                }
                set
                {
                    this.suitabilityField = value;
                }
            }

            /// <remarks/>
            public string ResultDetails
            {
                get
                {
                    return this.resultDetailsField;
                }
                set
                {
                    this.resultDetailsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public enum SuitabilityType
        {

            /// <remarks/>
            unknown,

            /// <remarks/>
            suitable,

            /// <remarks/>
            marginal,

            /// <remarks/>
            unsuitable,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class VehicleSpecificConstraintStructure : AnalysedConstraintStructure
        {

            private RoadIdentificationStructure roadField;

            private string[] vehicleField;

            private DistanceStructure extentField;

            private string ownerField;

            private bool isMyConstraintField;

            public VehicleSpecificConstraintStructure()
            {
                this.isMyConstraintField = false;
            }

            /// <remarks/>
            public RoadIdentificationStructure Road
            {
                get
                {
                    return this.roadField;
                }
                set
                {
                    this.roadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Vehicle", DataType = "token")]
            public string[] Vehicle
            {
                get
                {
                    return this.vehicleField;
                }
                set
                {
                    this.vehicleField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure Extent
            {
                get
                {
                    return this.extentField;
                }
                set
                {
                    this.extentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Owner
            {
                get
                {
                    return this.ownerField;
                }
                set
                {
                    this.ownerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsMyConstraint
            {
                get
                {
                    return this.isMyConstraintField;
                }
                set
                {
                    this.isMyConstraintField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute("VehicleComponent", Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class VehicleComponentStructure
        {

            private string informalNameField;

            private string descriptionField;

            private string summaryField;

            private VehicleComponentSubType componentSubTypeField;

            private object conformsToCandUField;

            private VehicleCouplingType couplingTypeField;

            private bool isSteerableAtRearField;

            private WeightStructure weightField;

            private DistanceStructure maxHeightField;

            private DistanceStructure reducibleHeightField;

            private DistanceStructure rigidLengthField;

            private DistanceStructure widthField;

            private DistanceStructure wheelbaseField;

            private DistanceStructure leftOverhangField;

            private DistanceStructure rightOverhangField;

            private DistanceStructure frontOverhangField;

            private DistanceStructure rearOverhangField;

            private DistanceStructure groundClearanceField;

            private DistanceStructure reducedGroundClearanceField;

            private DistanceStructure outsideTrackField;

            private WeightStructure maxAxleWeightField;

            private VehicleComponentStructureComponentInstance[] componentInstanceField;

            private DistanceStructure axleSpacingToFollowingComponentField;

            private VehicleComponentStructureAxleDetails axleDetailsField;

            private string componentNoField;

            private bool componentNoFieldSpecified;

            private VehicleIntentType intentField;

            private bool intentFieldSpecified;

            public VehicleComponentStructure()
            {
                this.isSteerableAtRearField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string InformalName
            {
                get
                {
                    return this.informalNameField;
                }
                set
                {
                    this.informalNameField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public VehicleComponentSubType ComponentSubType
            {
                get
                {
                    return this.componentSubTypeField;
                }
                set
                {
                    this.componentSubTypeField = value;
                }
            }

            /// <remarks/>
            public object ConformsToCandU
            {
                get
                {
                    return this.conformsToCandUField;
                }
                set
                {
                    this.conformsToCandUField = value;
                }
            }

            /// <remarks/>
            public VehicleCouplingType CouplingType
            {
                get
                {
                    return this.couplingTypeField;
                }
                set
                {
                    this.couplingTypeField = value;
                }
            }

            /// <remarks/>
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsSteerableAtRear
            {
                get
                {
                    return this.isSteerableAtRearField;
                }
                set
                {
                    this.isSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            public WeightStructure Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure ReducibleHeight
            {
                get
                {
                    return this.reducibleHeightField;
                }
                set
                {
                    this.reducibleHeightField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure Wheelbase
            {
                get
                {
                    return this.wheelbaseField;
                }
                set
                {
                    this.wheelbaseField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure OutsideTrack
            {
                get
                {
                    return this.outsideTrackField;
                }
                set
                {
                    this.outsideTrackField = value;
                }
            }

            /// <remarks/>
            public WeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ComponentInstance")]
            public VehicleComponentStructureComponentInstance[] ComponentInstance
            {
                get
                {
                    return this.componentInstanceField;
                }
                set
                {
                    this.componentInstanceField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure AxleSpacingToFollowingComponent
            {
                get
                {
                    return this.axleSpacingToFollowingComponentField;
                }
                set
                {
                    this.axleSpacingToFollowingComponentField = value;
                }
            }

            /// <remarks/>
            public VehicleComponentStructureAxleDetails AxleDetails
            {
                get
                {
                    return this.axleDetailsField;
                }
                set
                {
                    this.axleDetailsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComponentNo
            {
                get
                {
                    return this.componentNoField;
                }
                set
                {
                    this.componentNoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ComponentNoSpecified
            {
                get
                {
                    return this.componentNoFieldSpecified;
                }
                set
                {
                    this.componentNoFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public VehicleIntentType Intent
            {
                get
                {
                    return this.intentField;
                }
                set
                {
                    this.intentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IntentSpecified
            {
                get
                {
                    return this.intentFieldSpecified;
                }
                set
                {
                    this.intentFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public enum VehicleComponentSubType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("ballast tractor")]
            ballasttractor,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("conventional tractor")]
            conventionaltractor,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("other tractor")]
            othertractor,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("semi trailer")]
            semitrailer,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("semi low loader")]
            semilowloader,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("trombone trailer")]
            trombonetrailer,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("other semi trailer")]
            othersemitrailer,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("drawbar trailer")]
            drawbartrailer,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("other drawbar trailer")]
            otherdrawbartrailer,

            /// <remarks/>
            bogie,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("twin bogies")]
            twinbogies,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("tracked vehicle")]
            trackedvehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("rigid vehicle")]
            rigidvehicle,

            /// <remarks/>
            spmt,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("girder set")]
            girderset,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("wheeled load")]
            wheeledload,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("recovery vehicle")]
            recoveryvehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("recovered vehicle")]
            recoveredvehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("mobile crane")]
            mobilecrane,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("engineering plant")]
            engineeringplant,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public enum VehicleCouplingType
        {

            /// <remarks/>
            none,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("fifth wheel")]
            fifthwheel,

            /// <remarks/>
            drawbar,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("tow hitch")]
            towhitch,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleComponentStructureComponentInstance
        {

            private string plateNoField;

            private string fleetNoField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
            public string PlateNo
            {
                get
                {
                    return this.plateNoField;
                }
                set
                {
                    this.plateNoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FleetNo
            {
                get
                {
                    return this.fleetNoField;
                }
                set
                {
                    this.fleetNoField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleComponentStructureAxleDetails
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleList", typeof(VehicleComponentStructureAxleDetailsAxleList))]
            [System.Xml.Serialization.XmlElementAttribute("StandardCAndUVehicle", typeof(VehicleComponentStructureAxleDetailsStandardCAndUVehicle))]
            [System.Xml.Serialization.XmlElementAttribute("TrackedVehicle", typeof(object))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleComponentStructureAxleDetailsAxleList
        {

            private VehicleComponentStructureAxleDetailsAxleListAxle[] axleField;

            private string numberOfAxlesField;

            private WeightUnitType axleWeightUnitField;

            private bool axleWeightUnitFieldSpecified;

            private DistanceUnitType axleSpacingUnitField;

            private bool axleSpacingUnitFieldSpecified;

            private DistanceUnitType wheelSpacingUnitField;

            private bool wheelSpacingUnitFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Axle")]
            public VehicleComponentStructureAxleDetailsAxleListAxle[] Axle
            {
                get
                {
                    return this.axleField;
                }
                set
                {
                    this.axleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string NumberOfAxles
            {
                get
                {
                    return this.numberOfAxlesField;
                }
                set
                {
                    this.numberOfAxlesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public WeightUnitType AxleWeightUnit
            {
                get
                {
                    return this.axleWeightUnitField;
                }
                set
                {
                    this.axleWeightUnitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleWeightUnitSpecified
            {
                get
                {
                    return this.axleWeightUnitFieldSpecified;
                }
                set
                {
                    this.axleWeightUnitFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DistanceUnitType AxleSpacingUnit
            {
                get
                {
                    return this.axleSpacingUnitField;
                }
                set
                {
                    this.axleSpacingUnitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleSpacingUnitSpecified
            {
                get
                {
                    return this.axleSpacingUnitFieldSpecified;
                }
                set
                {
                    this.axleSpacingUnitFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DistanceUnitType WheelSpacingUnit
            {
                get
                {
                    return this.wheelSpacingUnitField;
                }
                set
                {
                    this.wheelSpacingUnitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool WheelSpacingUnitSpecified
            {
                get
                {
                    return this.wheelSpacingUnitFieldSpecified;
                }
                set
                {
                    this.wheelSpacingUnitFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleComponentStructureAxleDetailsAxleListAxle : AxleStructure
        {

            private string axleNumberField;

            private string axleCountField;

            private string distanceToNextAxleField;

            private bool distanceToNextAxleFieldSpecified;

            public VehicleComponentStructureAxleDetailsAxleListAxle()
            {
                this.axleCountField = "1";
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string AxleNumber
            {
                get
                {
                    return this.axleNumberField;
                }
                set
                {
                    this.axleNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            [System.ComponentModel.DefaultValueAttribute("1")]
            public string AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DistanceToNextAxle
            {
                get
                {
                    return this.distanceToNextAxleField;
                }
                set
                {
                    this.distanceToNextAxleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DistanceToNextAxleSpecified
            {
                get
                {
                    return this.distanceToNextAxleFieldSpecified;
                }
                set
                {
                    this.distanceToNextAxleFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class AxleStructure
        {

            private string tyreSizeField;

            private string wheelSpacingField;

            private string axleWeightField;

            private bool axleWeightFieldSpecified;

            private string numberOfWheelsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string TyreSize
            {
                get
                {
                    return this.tyreSizeField;
                }
                set
                {
                    this.tyreSizeField = value;
                }
            }

            /// <remarks/>
            public string WheelSpacing
            {
                get
                {
                    return this.wheelSpacingField;
                }
                set
                {
                    this.wheelSpacingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleWeightSpecified
            {
                get
                {
                    return this.axleWeightFieldSpecified;
                }
                set
                {
                    this.axleWeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string NumberOfWheels
            {
                get
                {
                    return this.numberOfWheelsField;
                }
                set
                {
                    this.numberOfWheelsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleComponentStructureAxleDetailsStandardCAndUVehicle
        {

            private string numberOfAxlesField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string NumberOfAxles
            {
                get
                {
                    return this.numberOfAxlesField;
                }
                set
                {
                    this.numberOfAxlesField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public enum VehicleIntentType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("C AND U")]
            candu,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("STGO AIL")]
            stgoail,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("STGO mobile crane")]
            stgomobilecrane,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("STGO engineering plant wheeled")]
            stgoengineeringplantwheeled,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("STGO road recovery")]
            stgoroadrecovery,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("Special order")]
            specialorder,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("Vehicle special order")]
            vehiclespecialorder,

            /// <remarks/>
            tracked,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute("VehicleConfiguration", Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class VehicleConfigurationStructure
        {

            private string nameField;

            private string descriptionField;

            private string summaryField;

            private VehicleConfigurationType configurationTypeField;

            private object conformsToCandUField;

            private DistanceStructure configurationLengthField;

            private DistanceStructure rigidLengthField;

            private DistanceStructure widthField;

            private WeightStructure grossWeightField;

            private DistanceStructure maxHeightField;

            private WeightStructure maxAxleWeightField;

            private DistanceStructure overallWheelbaseField;

            private VehicleConfigurationStructureConfigurationInstance[] configurationInstanceField;

            private VehicleConfigurationStructureConfigurationComponent[] configurationComponentField;

            private SpeedStructure travellingSpeedField;

            private DistanceStructure sideBySideSpacingField;

            private string configurationNoField;

            private bool configurationNoFieldSpecified;

            private string idField;

            private bool idFieldSpecified;

            private VehicleIntentType intentField;

            private bool intentFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public VehicleConfigurationType ConfigurationType
            {
                get
                {
                    return this.configurationTypeField;
                }
                set
                {
                    this.configurationTypeField = value;
                }
            }

            /// <remarks/>
            public object ConformsToCandU
            {
                get
                {
                    return this.conformsToCandUField;
                }
                set
                {
                    this.conformsToCandUField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure ConfigurationLength
            {
                get
                {
                    return this.configurationLengthField;
                }
                set
                {
                    this.configurationLengthField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public WeightStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            public WeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure OverallWheelbase
            {
                get
                {
                    return this.overallWheelbaseField;
                }
                set
                {
                    this.overallWheelbaseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ConfigurationInstance")]
            public VehicleConfigurationStructureConfigurationInstance[] ConfigurationInstance
            {
                get
                {
                    return this.configurationInstanceField;
                }
                set
                {
                    this.configurationInstanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ConfigurationComponent")]
            public VehicleConfigurationStructureConfigurationComponent[] ConfigurationComponent
            {
                get
                {
                    return this.configurationComponentField;
                }
                set
                {
                    this.configurationComponentField = value;
                }
            }

            /// <remarks/>
            public SpeedStructure TravellingSpeed
            {
                get
                {
                    return this.travellingSpeedField;
                }
                set
                {
                    this.travellingSpeedField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure SideBySideSpacing
            {
                get
                {
                    return this.sideBySideSpacingField;
                }
                set
                {
                    this.sideBySideSpacingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ConfigurationNo
            {
                get
                {
                    return this.configurationNoField;
                }
                set
                {
                    this.configurationNoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ConfigurationNoSpecified
            {
                get
                {
                    return this.configurationNoFieldSpecified;
                }
                set
                {
                    this.configurationNoFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IdSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public VehicleIntentType Intent
            {
                get
                {
                    return this.intentField;
                }
                set
                {
                    this.intentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IntentSpecified
            {
                get
                {
                    return this.intentFieldSpecified;
                }
                set
                {
                    this.intentFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public enum VehicleConfigurationType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("drawbar vehicle")]
            drawbarvehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("semi vehicle")]
            semivehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("rigid vehicle")]
            rigidvehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("tracked vehicle")]
            trackedvehicle,

            /// <remarks/>
            spmt,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("other in line")]
            otherinline,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("other side by side")]
            othersidebyside,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleConfigurationStructureConfigurationInstance
        {

            private string plateNoField;

            private string fleetNoField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
            public string PlateNo
            {
                get
                {
                    return this.plateNoField;
                }
                set
                {
                    this.plateNoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FleetNo
            {
                get
                {
                    return this.fleetNoField;
                }
                set
                {
                    this.fleetNoField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleConfigurationStructureConfigurationComponent
        {

            private object itemField;

            private string longitudeField;

            private string latitudeField;

            private VehicleComponentType componentTypeField;

            private bool componentTypeFieldSpecified;

            public VehicleConfigurationStructureConfigurationComponent()
            {
                this.longitudeField = "1";
                this.latitudeField = "1";
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ComponentID", typeof(int))]
            [System.Xml.Serialization.XmlElementAttribute("VehicleComponent", typeof(VehicleComponentStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            [System.ComponentModel.DefaultValueAttribute("1")]
            public string Longitude
            {
                get
                {
                    return this.longitudeField;
                }
                set
                {
                    this.longitudeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            [System.ComponentModel.DefaultValueAttribute("1")]
            public string Latitude
            {
                get
                {
                    return this.latitudeField;
                }
                set
                {
                    this.latitudeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public VehicleComponentType ComponentType
            {
                get
                {
                    return this.componentTypeField;
                }
                set
                {
                    this.componentTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ComponentTypeSpecified
            {
                get
                {
                    return this.componentTypeFieldSpecified;
                }
                set
                {
                    this.componentTypeFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public enum VehicleComponentType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("ballast tractor")]
            ballasttractor,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("conventional tractor")]
            conventionaltractor,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("rigid vehicle")]
            rigidvehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("tracked vehicle")]
            trackedvehicle,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("semi trailer")]
            semitrailer,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("drawbar trailer")]
            drawbartrailer,

            /// <remarks/>
            spmt,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        [System.Xml.Serialization.XmlRootAttribute("VehicleFleet", Namespace = "http://www.esdal.com/schemas/core/vehicle", IsNullable = false)]
        public partial class VehicleFleetStructure
        {

            private object[] itemsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Component", typeof(VehicleComponentStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Configuration", typeof(VehicleConfigurationStructure))]
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
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/dispensation")]
        [System.Xml.Serialization.XmlRootAttribute("Dispensation", Namespace = "http://www.esdal.com/schemas/core/dispensation", IsNullable = false)]
        public partial class DispensationStructure
        {

            private string dRNField;

            private string summaryField;

            private string descriptionField;

            private VehicleCharacteristicsStructure vehicleCharacteristicsField;

            private ValidityStructure validityField;

            private OwningPartyStructure ownedByField;

            private PartyStructure grantedToHaulierField;

            private PartyStructure grantedByNotifiableAuthorityField;

            private string idField;

            private bool idFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string DRN
            {
                get
                {
                    return this.dRNField;
                }
                set
                {
                    this.dRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public VehicleCharacteristicsStructure VehicleCharacteristics
            {
                get
                {
                    return this.vehicleCharacteristicsField;
                }
                set
                {
                    this.vehicleCharacteristicsField = value;
                }
            }

            /// <remarks/>
            public ValidityStructure Validity
            {
                get
                {
                    return this.validityField;
                }
                set
                {
                    this.validityField = value;
                }
            }

            /// <remarks/>
            public OwningPartyStructure OwnedBy
            {
                get
                {
                    return this.ownedByField;
                }
                set
                {
                    this.ownedByField = value;
                }
            }

            /// <remarks/>
            public PartyStructure GrantedToHaulier
            {
                get
                {
                    return this.grantedToHaulierField;
                }
                set
                {
                    this.grantedToHaulierField = value;
                }
            }

            /// <remarks/>
            public PartyStructure GrantedByNotifiableAuthority
            {
                get
                {
                    return this.grantedByNotifiableAuthorityField;
                }
                set
                {
                    this.grantedByNotifiableAuthorityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IdSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/dispensation")]
        public partial class VehicleCharacteristicsStructure
        {

            private string maxGrossWeightField;

            private string maxAxleWeightField;

            private string maxHeightField;

            private bool maxHeightFieldSpecified;

            private string maxOverallLengthField;

            private bool maxOverallLengthFieldSpecified;

            private string maxWidthField;

            private bool maxWidthFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string MaxGrossWeight
            {
                get
                {
                    return this.maxGrossWeightField;
                }
                set
                {
                    this.maxGrossWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public string MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaxHeightSpecified
            {
                get
                {
                    return this.maxHeightFieldSpecified;
                }
                set
                {
                    this.maxHeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string MaxOverallLength
            {
                get
                {
                    return this.maxOverallLengthField;
                }
                set
                {
                    this.maxOverallLengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaxOverallLengthSpecified
            {
                get
                {
                    return this.maxOverallLengthFieldSpecified;
                }
                set
                {
                    this.maxOverallLengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string MaxWidth
            {
                get
                {
                    return this.maxWidthField;
                }
                set
                {
                    this.maxWidthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaxWidthSpecified
            {
                get
                {
                    return this.maxWidthFieldSpecified;
                }
                set
                {
                    this.maxWidthFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/dispensation")]
        public partial class ValidityStructure
        {

            private string startField;

            private string endField;

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string Start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string End
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/dispensation")]
        public partial class OwningPartyStructure : PartyStructure
        {

            private bool isHaulierField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsHaulier
            {
                get
                {
                    return this.isHaulierField;
                }
                set
                {
                    this.isHaulierField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(OwningPartyStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/dispensation")]
        public partial class PartyStructure
        {

            private string nameField;

            private string idField;

            private bool idFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IdSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        [System.Xml.Serialization.XmlRootAttribute("PlannedRoutePart", Namespace = "http://www.esdal.com/schemas/core/route", IsNullable = false)]
        public partial class PlannedRoutePartStructure
        {

            private string nameField;

            private string descriptionField;

            private object includeDockCautionField;

            private ModeOfTransportType modeOfTransportField;

            private RoutePointStructure[] startPointsField;

            private RoutePointStructure[] endPointsField;

            private PlannedRoutePartStructurePartTraversal partTraversalField;

            private string idField;

            private bool idFieldSpecified;

            private string comparisonIdField;

            private bool comparisonIdFieldSpecified;

            private bool isSpatiallyCompleteField;

            private ConsistencyType consistencyField;

            private bool isAssumedConnectivityField;

            public PlannedRoutePartStructure()
            {
                this.isSpatiallyCompleteField = false;
                this.consistencyField = ConsistencyType.consistent;
                this.isAssumedConnectivityField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public object IncludeDockCaution
            {
                get
                {
                    return this.includeDockCautionField;
                }
                set
                {
                    this.includeDockCautionField = value;
                }
            }

            /// <remarks/>
            public ModeOfTransportType ModeOfTransport
            {
                get
                {
                    return this.modeOfTransportField;
                }
                set
                {
                    this.modeOfTransportField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("StartPoint", IsNullable = false)]
            public RoutePointStructure[] StartPoints
            {
                get
                {
                    return this.startPointsField;
                }
                set
                {
                    this.startPointsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("EndPoint", IsNullable = false)]
            public RoutePointStructure[] EndPoints
            {
                get
                {
                    return this.endPointsField;
                }
                set
                {
                    this.endPointsField = value;
                }
            }

            /// <remarks/>
            public PlannedRoutePartStructurePartTraversal PartTraversal
            {
                get
                {
                    return this.partTraversalField;
                }
                set
                {
                    this.partTraversalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IdSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComparisonId
            {
                get
                {
                    return this.comparisonIdField;
                }
                set
                {
                    this.comparisonIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ComparisonIdSpecified
            {
                get
                {
                    return this.comparisonIdFieldSpecified;
                }
                set
                {
                    this.comparisonIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsSpatiallyComplete
            {
                get
                {
                    return this.isSpatiallyCompleteField;
                }
                set
                {
                    this.isSpatiallyCompleteField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ConsistencyType.consistent)]
            public ConsistencyType Consistency
            {
                get
                {
                    return this.consistencyField;
                }
                set
                {
                    this.consistencyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsAssumedConnectivity
            {
                get
                {
                    return this.isAssumedConnectivityField;
                }
                set
                {
                    this.isAssumedConnectivityField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
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
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(WayPointStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnresolvedWayPointStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolvedWayPointStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class RoutePointStructure : SimplifiedRoutePointStructure
        {

            private SegmentPointStructure positionField;

            private string idField;

            private bool idFieldSpecified;

            /// <remarks/>
            public SegmentPointStructure Position
            {
                get
                {
                    return this.positionField;
                }
                set
                {
                    this.positionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IdSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(RoutePointStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(WayPointStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnresolvedWayPointStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolvedWayPointStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class SimplifiedRoutePointStructure
        {

            private string descriptionField;

            private string gridRefField;

            private string mTPRNField;

            private bool isBrokenField;

            public SimplifiedRoutePointStructure()
            {
                this.isBrokenField = false;
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public string GridRef
            {
                get
                {
                    return this.gridRefField;
                }
                set
                {
                    this.gridRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/mtp")]
            public string MTPRN
            {
                get
                {
                    return this.mTPRNField;
                }
                set
                {
                    this.mTPRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnresolvedWayPointStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolvedWayPointStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public abstract partial class WayPointStructure : RoutePointStructure
        {

            private SimpleTextStructure textField;

            /// <remarks/>
            public SimpleTextStructure Text
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class UnresolvedWayPointStructure : WayPointStructure
        {

            private ContactReferenceStructure[] contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ContactReferenceStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class ResolvedWayPointStructure : WayPointStructure
        {

            private ResolvedContactStructure[] contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ResolvedContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class PlannedRoutePartStructurePartTraversal
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("NonRoadPart", typeof(PlannedRoutePartStructurePartTraversalNonRoadPart))]
            [System.Xml.Serialization.XmlElementAttribute("RoadPart", typeof(PlannedRoutePartStructurePartTraversalRoadPart))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class PlannedRoutePartStructurePartTraversalNonRoadPart
        {

            private string nonRoadGeometryField;

            /// <remarks/>
            public string NonRoadGeometry
            {
                get
                {
                    return this.nonRoadGeometryField;
                }
                set
                {
                    this.nonRoadGeometryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class PlannedRoutePartStructurePartTraversalRoadPart
        {

            private PlannedRouteSubPartStructure[] subPartField;

            private UnresolvedWayPointStructure[] wayPointsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SubPart")]
            public PlannedRouteSubPartStructure[] SubPart
            {
                get
                {
                    return this.subPartField;
                }
                set
                {
                    this.subPartField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("WayPoint", IsNullable = false)]
            public UnresolvedWayPointStructure[] WayPoints
            {
                get
                {
                    return this.wayPointsField;
                }
                set
                {
                    this.wayPointsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class PlannedRouteSubPartStructure
        {

            private PlannedRoutePathStructure[] pathsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Path", IsNullable = false)]
            public PlannedRoutePathStructure[] Paths
            {
                get
                {
                    return this.pathsField;
                }
                set
                {
                    this.pathsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class PlannedRoutePathStructure
        {

            private string descriptionField;

            private ContiguousRouteSegmentStructure[] contiguousSegmentsField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ContiguousSegment", IsNullable = false)]
            public ContiguousRouteSegmentStructure[] ContiguousSegments
            {
                get
                {
                    return this.contiguousSegmentsField;
                }
                set
                {
                    this.contiguousSegmentsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class ContiguousRouteSegmentStructure
        {

            private SegmentPointStructure startPointField;

            private SegmentPointStructure endPointField;

            private RouteAnnotationStructure[] annotationField;

            private UnresolvedWayPointStructure[] wayPointField;

            private string descriptionField;

            private ContiguousRouteSegmentStructureSegmentTraversal segmentTraversalField;

            private SegmentType typeField;

            public ContiguousRouteSegmentStructure()
            {
                this.typeField = SegmentType.normal;
            }

            /// <remarks/>
            public SegmentPointStructure StartPoint
            {
                get
                {
                    return this.startPointField;
                }
                set
                {
                    this.startPointField = value;
                }
            }

            /// <remarks/>
            public SegmentPointStructure EndPoint
            {
                get
                {
                    return this.endPointField;
                }
                set
                {
                    this.endPointField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation")]
            public RouteAnnotationStructure[] Annotation
            {
                get
                {
                    return this.annotationField;
                }
                set
                {
                    this.annotationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("WayPoint")]
            public UnresolvedWayPointStructure[] WayPoint
            {
                get
                {
                    return this.wayPointField;
                }
                set
                {
                    this.wayPointField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public ContiguousRouteSegmentStructureSegmentTraversal SegmentTraversal
            {
                get
                {
                    return this.segmentTraversalField;
                }
                set
                {
                    this.segmentTraversalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(SegmentType.normal)]
            public SegmentType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/route")]
        public partial class ContiguousRouteSegmentStructureSegmentTraversal
        {

            private string itemField;

            private ItemChoiceType2 itemElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OffRoadGeometry", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("OnRoadSections", typeof(string))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
            public string Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemChoiceType2 ItemElementName
            {
                get
                {
                    return this.itemElementNameField;
                }
                set
                {
                    this.itemElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route", IncludeInSchema = false)]
        public enum ItemChoiceType2
        {

            /// <remarks/>
            OffRoadGeometry,

            /// <remarks/>
            OnRoadSections,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public enum SegmentType
        {

            /// <remarks/>
            normal,

            /// <remarks/>
            @override,

            /// <remarks/>
            offroad,

            /// <remarks/>
            shunt,

            /// <remarks/>
            uturn,

            /// <remarks/>
            broken,

            /// <remarks/>
            assumed,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/route")]
        public enum ConsistencyType
        {

            /// <remarks/>
            consistent,

            /// <remarks/>
            unknown,

            /// <remarks/>
            inconsistent,

            /// <remarks/>
            broken,

            /// <remarks/>
            brokenorworse,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        [System.Xml.Serialization.XmlRootAttribute("RouteDrivingInstructions", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction", IsNullable = false)]
        public partial class RouteDrivingInstructionsStructure
        {

            private DrivingInstructionsStructure[] routePartsField;

            private PredefinedCautionsDescriptionsStructure predefinedCautionsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("DrivingInstructions", IsNullable = false)]
            public DrivingInstructionsStructure[] RouteParts
            {
                get
                {
                    return this.routePartsField;
                }
                set
                {
                    this.routePartsField = value;
                }
            }

            /// <remarks/>
            public PredefinedCautionsDescriptionsStructure PredefinedCautions
            {
                get
                {
                    return this.predefinedCautionsField;
                }
                set
                {
                    this.predefinedCautionsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        [System.Xml.Serialization.XmlRootAttribute("DrivingInstructions", Namespace = "http://www.esdal.com/schemas/core/drivinginstruction", IsNullable = false)]
        public partial class DrivingInstructionsStructure
        {

            private string legNumberField;

            private string nameField;

            private DrivingInstructionsStructureSubPartListPosition[] subPartListPositionField;

            private string idField;

            private string comparisonIdField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string LegNumber
            {
                get
                {
                    return this.legNumberField;
                }
                set
                {
                    this.legNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SubPartListPosition")]
            public DrivingInstructionsStructureSubPartListPosition[] SubPartListPosition
            {
                get
                {
                    return this.subPartListPositionField;
                }
                set
                {
                    this.subPartListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComparisonId
            {
                get
                {
                    return this.comparisonIdField;
                }
                set
                {
                    this.comparisonIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsStructureSubPartListPosition
        {

            private DrivingInstructionSubPartStructureAlternativeListPosition[] subPartField;

            private DrivingInstructionsStructureSubPartListPositionOldSubPart oldSubPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("AlternativeListPosition", IsNullable = false)]
            public DrivingInstructionSubPartStructureAlternativeListPosition[] SubPart
            {
                get
                {
                    return this.subPartField;
                }
                set
                {
                    this.subPartField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionsStructureSubPartListPositionOldSubPart OldSubPart
            {
                get
                {
                    return this.oldSubPartField;
                }
                set
                {
                    this.oldSubPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionSubPartStructureAlternativeListPosition
        {

            private DrivingInstructionListStructure alternativeField;

            private DrivingInstructionSubPartStructureAlternativeListPositionOldAlternative oldAlternativeField;

            /// <remarks/>
            public DrivingInstructionListStructure Alternative
            {
                get
                {
                    return this.alternativeField;
                }
                set
                {
                    this.alternativeField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionSubPartStructureAlternativeListPositionOldAlternative OldAlternative
            {
                get
                {
                    return this.oldAlternativeField;
                }
                set
                {
                    this.oldAlternativeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionListStructure
        {

            private AlternativeDescriptionStructure alternativeDescriptionField;

            private DrivingInstructionListStructureOldAlternativeDescription oldAlternativeDescriptionField;

            private DrivingInstructionListStructureInstructionListPosition[] instructionListPositionField;

            /// <remarks/>
            public AlternativeDescriptionStructure AlternativeDescription
            {
                get
                {
                    return this.alternativeDescriptionField;
                }
                set
                {
                    this.alternativeDescriptionField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionListStructureOldAlternativeDescription OldAlternativeDescription
            {
                get
                {
                    return this.oldAlternativeDescriptionField;
                }
                set
                {
                    this.oldAlternativeDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("InstructionListPosition")]
            public DrivingInstructionListStructureInstructionListPosition[] InstructionListPosition
            {
                get
                {
                    return this.instructionListPositionField;
                }
                set
                {
                    this.instructionListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class AlternativeDescriptionStructure
        {

            private string alternativeNoField;

            private string descriptionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
            public string AlternativeNo
            {
                get
                {
                    return this.alternativeNoField;
                }
                set
                {
                    this.alternativeNoField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionListStructureOldAlternativeDescription
        {

            private AlternativeDescriptionStructure alternativeDescriptionField;

            /// <remarks/>
            public AlternativeDescriptionStructure AlternativeDescription
            {
                get
                {
                    return this.alternativeDescriptionField;
                }
                set
                {
                    this.alternativeDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionListStructureInstructionListPosition
        {

            private DrivingInstructionStructure instructionField;

            private DrivingInstructionListStructureInstructionListPositionOldInstruction oldInstructionField;

            /// <remarks/>
            public DrivingInstructionStructure Instruction
            {
                get
                {
                    return this.instructionField;
                }
                set
                {
                    this.instructionField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionListStructureInstructionListPositionOldInstruction OldInstruction
            {
                get
                {
                    return this.oldInstructionField;
                }
                set
                {
                    this.oldInstructionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionStructure
        {

            private NavigationInstructionStructure navigationField;

            private DrivingInstructionStructureOldNavigation oldNavigationField;

            private DrivingInstructionStructureNoteListPosition[] noteListPositionField;

            /// <remarks/>
            public NavigationInstructionStructure Navigation
            {
                get
                {
                    return this.navigationField;
                }
                set
                {
                    this.navigationField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionStructureOldNavigation OldNavigation
            {
                get
                {
                    return this.oldNavigationField;
                }
                set
                {
                    this.oldNavigationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("NoteListPosition")]
            public DrivingInstructionStructureNoteListPosition[] NoteListPosition
            {
                get
                {
                    return this.noteListPositionField;
                }
                set
                {
                    this.noteListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class NavigationInstructionStructure
        {

            private SimpleTextStructure instructionField;

            private DrivingInstructionDistanceStructure distanceField;

            /// <remarks/>
            public SimpleTextStructure Instruction
            {
                get
                {
                    return this.instructionField;
                }
                set
                {
                    this.instructionField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionDistanceStructure Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionDistanceStructure
        {

            private string measuredMetricField;

            private string displayMetricField;

            private string displayImperialField;

            /// <remarks/>
            public string MeasuredMetric
            {
                get
                {
                    return this.measuredMetricField;
                }
                set
                {
                    this.measuredMetricField = value;
                }
            }

            /// <remarks/>
            public string DisplayMetric
            {
                get
                {
                    return this.displayMetricField;
                }
                set
                {
                    this.displayMetricField = value;
                }
            }

            /// <remarks/>
            public string DisplayImperial
            {
                get
                {
                    return this.displayImperialField;
                }
                set
                {
                    this.displayImperialField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionStructureOldNavigation
        {

            private NavigationInstructionStructure navigationField;

            /// <remarks/>
            public NavigationInstructionStructure Navigation
            {
                get
                {
                    return this.navigationField;
                }
                set
                {
                    this.navigationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionStructureNoteListPosition
        {

            private DrivingInstructionNoteStructure noteField;

            private DrivingInstructionStructureNoteListPositionOldNote oldNoteField;

            /// <remarks/>
            public DrivingInstructionNoteStructure Note
            {
                get
                {
                    return this.noteField;
                }
                set
                {
                    this.noteField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionStructureNoteListPositionOldNote OldNote
            {
                get
                {
                    return this.oldNoteField;
                }
                set
                {
                    this.oldNoteField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionNoteStructure
        {

            private NoteChoiceStructure contentField;

            private GridReferenceStructure gridReferenceField;

            private DrivingInstructionDistanceStructure encounteredAtField;

            private DrivingInstructionDistanceStructure lastsForField;

            private ResolvedContactStructure[] contactField;

            /// <remarks/>
            public NoteChoiceStructure Content
            {
                get
                {
                    return this.contentField;
                }
                set
                {
                    this.contentField = value;
                }
            }

            /// <remarks/>
            public GridReferenceStructure GridReference
            {
                get
                {
                    return this.gridReferenceField;
                }
                set
                {
                    this.gridReferenceField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionDistanceStructure EncounteredAt
            {
                get
                {
                    return this.encounteredAtField;
                }
                set
                {
                    this.encounteredAtField = value;
                }
            }

            /// <remarks/>
            public DrivingInstructionDistanceStructure LastsFor
            {
                get
                {
                    return this.lastsForField;
                }
                set
                {
                    this.lastsForField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ResolvedContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class NoteChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation", typeof(ResolvedAnnotationStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Caution", typeof(ResolvedCautionStructure))]
            [System.Xml.Serialization.XmlElementAttribute("MotorwayCaution", typeof(MotorwayCautionStructure))]
            [System.Xml.Serialization.XmlElementAttribute("RoutePoint", typeof(RoutePointDescriptionStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class RoutePointDescriptionStructure
        {

            private string descriptionField;

            private SimpleTextStructure textField;

            private RoutePointType pointTypeField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public SimpleTextStructure Text
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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public RoutePointType PointType
            {
                get
                {
                    return this.pointTypeField;
                }
                set
                {
                    this.pointTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class MotorwayCautionStructure
        {
            private string descriptionField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
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

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionStructureNoteListPositionOldNote
        {

            private DrivingInstructionNoteStructure noteField;

            /// <remarks/>
            public DrivingInstructionNoteStructure Note
            {
                get
                {
                    return this.noteField;
                }
                set
                {
                    this.noteField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionListStructureInstructionListPositionOldInstruction
        {

            private DrivingInstructionStructure instructionField;

            /// <remarks/>
            public DrivingInstructionStructure Instruction
            {
                get
                {
                    return this.instructionField;
                }
                set
                {
                    this.instructionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionSubPartStructureAlternativeListPositionOldAlternative
        {

            private DrivingInstructionListStructure alternativeField;

            /// <remarks/>
            public DrivingInstructionListStructure Alternative
            {
                get
                {
                    return this.alternativeField;
                }
                set
                {
                    this.alternativeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class DrivingInstructionsStructureSubPartListPositionOldSubPart
        {

            private DrivingInstructionSubPartStructureAlternativeListPosition[] subPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("AlternativeListPosition", IsNullable = false)]
            public DrivingInstructionSubPartStructureAlternativeListPosition[] SubPart
            {
                get
                {
                    return this.subPartField;
                }
                set
                {
                    this.subPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(PredefinedCautionsDescriptionsStructure1))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class PredefinedCautionsDescriptionsStructure
        {

            private object dockCautionDescriptionField;

            private PredefinedCautionsDescriptionsStructureOldDockCautionDescription oldDockCautionDescriptionField;

            private object motorwayCautionDescriptionField;

            private PredefinedCautionsDescriptionsStructureOldMotorwayCautionDescription oldMotorwayCautionDescriptionField;

            private object heightCautionDescriptionField;

            private PredefinedCautionsDescriptionsStructureOldHeightCautionDescription oldHeightCautionDescriptionField;

            private object standardCautionDescriptionField;

            private PredefinedCautionsDescriptionsStructureOldStandardCautionDescription oldStandardCautionDescriptionField;

            /// <remarks/>
            public object DockCautionDescription
            {
                get
                {
                    return this.dockCautionDescriptionField;
                }
                set
                {
                    this.dockCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            public PredefinedCautionsDescriptionsStructureOldDockCautionDescription OldDockCautionDescription
            {
                get
                {
                    return this.oldDockCautionDescriptionField;
                }
                set
                {
                    this.oldDockCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            public object MotorwayCautionDescription
            {
                get
                {
                    return this.motorwayCautionDescriptionField;
                }
                set
                {
                    this.motorwayCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            public PredefinedCautionsDescriptionsStructureOldMotorwayCautionDescription OldMotorwayCautionDescription
            {
                get
                {
                    return this.oldMotorwayCautionDescriptionField;
                }
                set
                {
                    this.oldMotorwayCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            public object HeightCautionDescription
            {
                get
                {
                    return this.heightCautionDescriptionField;
                }
                set
                {
                    this.heightCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            public PredefinedCautionsDescriptionsStructureOldHeightCautionDescription OldHeightCautionDescription
            {
                get
                {
                    return this.oldHeightCautionDescriptionField;
                }
                set
                {
                    this.oldHeightCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            public object StandardCautionDescription
            {
                get
                {
                    return this.standardCautionDescriptionField;
                }
                set
                {
                    this.standardCautionDescriptionField = value;
                }
            }

            /// <remarks/>
            public PredefinedCautionsDescriptionsStructureOldStandardCautionDescription OldStandardCautionDescription
            {
                get
                {
                    return this.oldStandardCautionDescriptionField;
                }
                set
                {
                    this.oldStandardCautionDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class PredefinedCautionsDescriptionsStructureOldDockCautionDescription
        {

            private object dockCautionDescriptionField;

            /// <remarks/>
            public object DockCautionDescription
            {
                get
                {
                    return this.dockCautionDescriptionField;
                }
                set
                {
                    this.dockCautionDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class PredefinedCautionsDescriptionsStructureOldMotorwayCautionDescription
        {

            private object motorwayCautionDescriptionField;

            /// <remarks/>
            public object MotorwayCautionDescription
            {
                get
                {
                    return this.motorwayCautionDescriptionField;
                }
                set
                {
                    this.motorwayCautionDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class PredefinedCautionsDescriptionsStructureOldHeightCautionDescription
        {

            private object heightCautionDescriptionField;

            /// <remarks/>
            public object HeightCautionDescription
            {
                get
                {
                    return this.heightCautionDescriptionField;
                }
                set
                {
                    this.heightCautionDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
        public partial class PredefinedCautionsDescriptionsStructureOldStandardCautionDescription
        {

            private object standardCautionDescriptionField;

            /// <remarks/>
            public object StandardCautionDescription
            {
                get
                {
                    return this.standardCautionDescriptionField;
                }
                set
                {
                    this.standardCautionDescriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(TypeName = "PredefinedCautionsDescriptionsStructure", Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PredefinedCautionsDescriptionsStructure1 : PredefinedCautionsDescriptionsStructure
        {
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        [System.Xml.Serialization.XmlRootAttribute("AnalysedStructures", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
        public partial class AnalysedStructuresStructure
        {

            private AnalysedStructuresStructureAnalysedStructuresPart[] analysedStructuresPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedStructuresPart")]
            public AnalysedStructuresStructureAnalysedStructuresPart[] AnalysedStructuresPart
            {
                get
                {
                    return this.analysedStructuresPartField;
                }
                set
                {
                    this.analysedStructuresPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedStructuresStructureAnalysedStructuresPart : AnalysedStructuresPartStructure
        {

            private ModeOfTransportType modeOfTransportField;

            private string idField;

            private string comparisonIdField;

            public AnalysedStructuresStructureAnalysedStructuresPart()
            {
                this.modeOfTransportField = ModeOfTransportType.road;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ModeOfTransportType.road)]
            public ModeOfTransportType ModeOfTransport
            {
                get
                {
                    return this.modeOfTransportField;
                }
                set
                {
                    this.modeOfTransportField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComparisonId
            {
                get
                {
                    return this.comparisonIdField;
                }
                set
                {
                    this.comparisonIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedStructuresPartStructure
        {

            private string nameField;

            private AnalysedStructureStructure[] structureField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Structure")]
            public AnalysedStructureStructure[] Structure
            {
                get
                {
                    return this.structureField;
                }
                set
                {
                    this.structureField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedStructureStructure
        {

            private string eSRNField;

            private string nameField;

            private object isConstrainedField;

            private AnalysedStructureConstraintsStructure constraintsField;

            private ResolvedAnnotationStructure[] annotationField;

            private ResolvedCautionStructure[] cautionField;

            private ResponsiblePartyStructure[] structureResponsibilityField;

            private StructureSuitabilityStructure[] appraisalField;

            private StructureTraversalType traversalTypeField;

            private string structureSectionIdField;

            private bool structureSectionIdFieldSpecified;

            public AnalysedStructureStructure()
            {
                this.traversalTypeField = StructureTraversalType.underbridge;
            }

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public object IsConstrained
            {
                get
                {
                    return this.isConstrainedField;
                }
                set
                {
                    this.isConstrainedField = value;
                }
            }

            /// <remarks/>
            public AnalysedStructureConstraintsStructure Constraints
            {
                get
                {
                    return this.constraintsField;
                }
                set
                {
                    this.constraintsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation")]
            public ResolvedAnnotationStructure[] Annotation
            {
                get
                {
                    return this.annotationField;
                }
                set
                {
                    this.annotationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Caution")]
            public ResolvedCautionStructure[] Caution
            {
                get
                {
                    return this.cautionField;
                }
                set
                {
                    this.cautionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("StructureResponsibleParty", IsNullable = false)]
            public ResponsiblePartyStructure[] StructureResponsibility
            {
                get
                {
                    return this.structureResponsibilityField;
                }
                set
                {
                    this.structureResponsibilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Appraisal")]
            public StructureSuitabilityStructure[] Appraisal
            {
                get
                {
                    return this.appraisalField;
                }
                set
                {
                    this.appraisalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(StructureTraversalType.underbridge)]
            public StructureTraversalType TraversalType
            {
                get
                {
                    return this.traversalTypeField;
                }
                set
                {
                    this.traversalTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string StructureSectionId
            {
                get
                {
                    return this.structureSectionIdField;
                }
                set
                {
                    this.structureSectionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool StructureSectionIdSpecified
            {
                get
                {
                    return this.structureSectionIdFieldSpecified;
                }
                set
                {
                    this.structureSectionIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedStructureConstraintsStructure
        {

            private SignedSpatialConstraintsStructure signedSpatialConstraintsField;

            private SpatialConstraintsStructure unsignedSpatialConstraintField;

            private SignedWeightConstraintsStructure signedWeightConstraintsField;

            private WeightConstraintsStructure unsignedWeightConstraintsField;

            /// <remarks/>
            public SignedSpatialConstraintsStructure SignedSpatialConstraints
            {
                get
                {
                    return this.signedSpatialConstraintsField;
                }
                set
                {
                    this.signedSpatialConstraintsField = value;
                }
            }

            /// <remarks/>
            public SpatialConstraintsStructure UnsignedSpatialConstraint
            {
                get
                {
                    return this.unsignedSpatialConstraintField;
                }
                set
                {
                    this.unsignedSpatialConstraintField = value;
                }
            }

            /// <remarks/>
            public SignedWeightConstraintsStructure SignedWeightConstraints
            {
                get
                {
                    return this.signedWeightConstraintsField;
                }
                set
                {
                    this.signedWeightConstraintsField = value;
                }
            }

            /// <remarks/>
            public WeightConstraintsStructure UnsignedWeightConstraints
            {
                get
                {
                    return this.unsignedWeightConstraintsField;
                }
                set
                {
                    this.unsignedWeightConstraintsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SignedSpatialConstraintsStructure
        {

            private SignedSpatialConstraintChoiceStructure heightField;

            private SignedSpatialConstraintChoiceStructure widthField;

            private SignedSpatialConstraintChoiceStructure lengthField;

            /// <remarks/>
            public SignedSpatialConstraintChoiceStructure Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }

            /// <remarks/>
            public SignedSpatialConstraintChoiceStructure Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public SignedSpatialConstraintChoiceStructure Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SignedSpatialConstraintChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("None", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("SignedDistanceValue", typeof(SignedSpatialConstraintChoiceStructureSignedDistanceValue))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SignedSpatialConstraintChoiceStructureSignedDistanceValue : ImperialMetricPairStructure
        {

            private SigningStatusType signingStatusField;

            public SignedSpatialConstraintChoiceStructureSignedDistanceValue()
            {
                this.signingStatusField = SigningStatusType.PhysicallySigned;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(SigningStatusType.PhysicallySigned)]
            public SigningStatusType SigningStatus
            {
                get
                {
                    return this.signingStatusField;
                }
                set
                {
                    this.signingStatusField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum SigningStatusType
        {

            /// <remarks/>
            PhysicallySigned,

            /// <remarks/>
            PublicViaESDAL,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class ImperialMetricPairStructure
        {

            private string feetField;

            private bool feetFieldSpecified;

            private string metresField;

            private bool metresFieldSpecified;

            /// <remarks/>
            public string Feet
            {
                get
                {
                    return this.feetField;
                }
                set
                {
                    this.feetField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool FeetSpecified
            {
                get
                {
                    return this.feetFieldSpecified;
                }
                set
                {
                    this.feetFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Metres
            {
                get
                {
                    return this.metresField;
                }
                set
                {
                    this.metresField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MetresSpecified
            {
                get
                {
                    return this.metresFieldSpecified;
                }
                set
                {
                    this.metresFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SpatialConstraintsStructure
        {

            private string heightField;

            private bool heightFieldSpecified;

            private string widthField;

            private bool widthFieldSpecified;

            private string lengthField;

            private bool lengthFieldSpecified;

            /// <remarks/>
            public string Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool HeightSpecified
            {
                get
                {
                    return this.heightFieldSpecified;
                }
                set
                {
                    this.heightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool WidthSpecified
            {
                get
                {
                    return this.widthFieldSpecified;
                }
                set
                {
                    this.widthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SignedWeightConstraintsStructure
        {

            private SignedWeightConstraintChoiceStructure grossWeightField;

            private SignedWeightConstraintChoiceStructure axleWeightField;

            private SignedWeightConstraintChoiceStructure doubleAxleWeightField;

            private SignedWeightConstraintChoiceStructure tripleAxleWeightField;

            /// <remarks/>
            public SignedWeightConstraintChoiceStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }

            /// <remarks/>
            public SignedWeightConstraintChoiceStructure AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }

            /// <remarks/>
            public SignedWeightConstraintChoiceStructure DoubleAxleWeight
            {
                get
                {
                    return this.doubleAxleWeightField;
                }
                set
                {
                    this.doubleAxleWeightField = value;
                }
            }

            /// <remarks/>
            public SignedWeightConstraintChoiceStructure TripleAxleWeight
            {
                get
                {
                    return this.tripleAxleWeightField;
                }
                set
                {
                    this.tripleAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SignedWeightConstraintChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("None", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("SignedWeightValue", typeof(SignedWeightConstraintChoiceStructureSignedWeightValue))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SignedWeightConstraintChoiceStructureSignedWeightValue
        {

            private SigningStatusType signingStatusField;

            private string valueField;

            public SignedWeightConstraintChoiceStructureSignedWeightValue()
            {
                this.signingStatusField = SigningStatusType.PhysicallySigned;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(SigningStatusType.PhysicallySigned)]
            public SigningStatusType SigningStatus
            {
                get
                {
                    return this.signingStatusField;
                }
                set
                {
                    this.signingStatusField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class WeightConstraintsStructure
        {

            private string grossWeightField;

            private bool grossWeightFieldSpecified;

            private string axleWeightField;

            private bool axleWeightFieldSpecified;

            private string doubleAxleWeightField;

            private bool doubleAxleWeightFieldSpecified;

            private string tripleAxleWeightField;

            private bool tripleAxleWeightFieldSpecified;

            private AxleWeightConstraintsStructure[] axleWeightLimitsField;

            /// <remarks/>
            public string GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GrossWeightSpecified
            {
                get
                {
                    return this.grossWeightFieldSpecified;
                }
                set
                {
                    this.grossWeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleWeightSpecified
            {
                get
                {
                    return this.axleWeightFieldSpecified;
                }
                set
                {
                    this.axleWeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string DoubleAxleWeight
            {
                get
                {
                    return this.doubleAxleWeightField;
                }
                set
                {
                    this.doubleAxleWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DoubleAxleWeightSpecified
            {
                get
                {
                    return this.doubleAxleWeightFieldSpecified;
                }
                set
                {
                    this.doubleAxleWeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string TripleAxleWeight
            {
                get
                {
                    return this.tripleAxleWeightField;
                }
                set
                {
                    this.tripleAxleWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool TripleAxleWeightSpecified
            {
                get
                {
                    return this.tripleAxleWeightFieldSpecified;
                }
                set
                {
                    this.tripleAxleWeightFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleWeightLimits")]
            public AxleWeightConstraintsStructure[] AxleWeightLimits
            {
                get
                {
                    return this.axleWeightLimitsField;
                }
                set
                {
                    this.axleWeightLimitsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class AxleWeightConstraintsStructure
        {

            private string axleGroupWeightField;

            private string axleGroupLengthField;

            /// <remarks/>
            public string AxleGroupWeight
            {
                get
                {
                    return this.axleGroupWeightField;
                }
                set
                {
                    this.axleGroupWeightField = value;
                }
            }

            /// <remarks/>
            public string AxleGroupLength
            {
                get
                {
                    return this.axleGroupLengthField;
                }
                set
                {
                    this.axleGroupLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(RoadResponsiblePartyStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class ResponsiblePartyStructure
        {

            private string organisationNameField;

            private ResponsiblePartyStructure onBehalfOfField;

            private string organisationIdField;

            private string contactIdField;

            private bool retainNotificationField;

            private bool wantsFailureAlertField;

            private string delegationIdField;

            private bool delegationIdFieldSpecified;

            public ResponsiblePartyStructure()
            {
                this.retainNotificationField = false;
                this.wantsFailureAlertField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            public ResponsiblePartyStructure OnBehalfOf
            {
                get
                {
                    return this.onBehalfOfField;
                }
                set
                {
                    this.onBehalfOfField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactId
            {
                get
                {
                    return this.contactIdField;
                }
                set
                {
                    this.contactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool RetainNotification
            {
                get
                {
                    return this.retainNotificationField;
                }
                set
                {
                    this.retainNotificationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool WantsFailureAlert
            {
                get
                {
                    return this.wantsFailureAlertField;
                }
                set
                {
                    this.wantsFailureAlertField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegationId
            {
                get
                {
                    return this.delegationIdField;
                }
                set
                {
                    this.delegationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegationIdSpecified
            {
                get
                {
                    return this.delegationIdFieldSpecified;
                }
                set
                {
                    this.delegationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class RoadResponsiblePartyStructure : ResponsiblePartyStructure
        {

            private PartialRoadStructure partialResponsibilityField;

            /// <remarks/>
            public PartialRoadStructure PartialResponsibility
            {
                get
                {
                    return this.partialResponsibilityField;
                }
                set
                {
                    this.partialResponsibilityField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class PartialRoadStructure
        {

            private DistanceStructure encounteredAtField;

            private DistanceStructure lastingForField;

            /// <remarks/>
            public DistanceStructure EncounteredAt
            {
                get
                {
                    return this.encounteredAtField;
                }
                set
                {
                    this.encounteredAtField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure LastingFor
            {
                get
                {
                    return this.lastingForField;
                }
                set
                {
                    this.lastingForField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class StructureSuitabilityStructure
        {

            private SuitabilityType suitabilityField;

            private string organisationField;

            private StructureSectionSuitabilityStructure[] individualSectionSuitabilityField;

            private string organisationIdField;

            private bool organisationIdFieldSpecified;

            /// <remarks/>
            public SuitabilityType Suitability
            {
                get
                {
                    return this.suitabilityField;
                }
                set
                {
                    this.suitabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Organisation
            {
                get
                {
                    return this.organisationField;
                }
                set
                {
                    this.organisationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("IndividualSectionSuitability")]
            public StructureSectionSuitabilityStructure[] IndividualSectionSuitability
            {
                get
                {
                    return this.individualSectionSuitabilityField;
                }
                set
                {
                    this.individualSectionSuitabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIdSpecified
            {
                get
                {
                    return this.organisationIdFieldSpecified;
                }
                set
                {
                    this.organisationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class StructureSectionSuitabilityStructure
        {

            private SuitabilityType suitabilityField;

            private string sectionDescriptionField;

            private SuitabilityResultStructure[] individualResultField;

            private string sectionIdField;

            private bool sectionIdFieldSpecified;

            /// <remarks/>
            public SuitabilityType Suitability
            {
                get
                {
                    return this.suitabilityField;
                }
                set
                {
                    this.suitabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string SectionDescription
            {
                get
                {
                    return this.sectionDescriptionField;
                }
                set
                {
                    this.sectionDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("IndividualResult")]
            public SuitabilityResultStructure[] IndividualResult
            {
                get
                {
                    return this.individualResultField;
                }
                set
                {
                    this.individualResultField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string SectionId
            {
                get
                {
                    return this.sectionIdField;
                }
                set
                {
                    this.sectionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SectionIdSpecified
            {
                get
                {
                    return this.sectionIdFieldSpecified;
                }
                set
                {
                    this.sectionIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class SuitabilityResultStructure
        {

            private SuitabilityType suitabilityField;

            private SuitabilityTestClassType testClassField;

            private string testIdentityField;

            private string resultDetailsField;

            /// <remarks/>
            public SuitabilityType Suitability
            {
                get
                {
                    return this.suitabilityField;
                }
                set
                {
                    this.suitabilityField = value;
                }
            }

            /// <remarks/>
            public SuitabilityTestClassType TestClass
            {
                get
                {
                    return this.testClassField;
                }
                set
                {
                    this.testClassField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string TestIdentity
            {
                get
                {
                    return this.testIdentityField;
                }
                set
                {
                    this.testIdentityField = value;
                }
            }

            /// <remarks/>
            public string ResultDetails
            {
                get
                {
                    return this.resultDetailsField;
                }
                set
                {
                    this.resultDetailsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public enum SuitabilityTestClassType
        {

            /// <remarks/>
            ICA,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("dimensional constraint")]
            dimensionalconstraint,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public enum StructureTraversalType
        {

            /// <remarks/>
            underbridge,

            /// <remarks/>
            overbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("level crossing")]
            levelcrossing,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("arched overbridge")]
            archedoverbridge,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        [System.Xml.Serialization.XmlRootAttribute("AnalysedRoute", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
        public partial class AnalysedRouteStructure
        {

            private AnalysedPartStructure[] analysedRoutePartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedRoutePart")]
            public AnalysedPartStructure[] AnalysedRoutePart
            {
                get
                {
                    return this.analysedRoutePartField;
                }
                set
                {
                    this.analysedRoutePartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedPartStructure
        {

            private string nameField;

            private AnalysedRoadPartStructure roadPartField;

            private ModeOfTransportType modeOfTransportField;

            private bool isBrokenField;

            public AnalysedPartStructure()
            {
                this.modeOfTransportField = ModeOfTransportType.road;
                this.isBrokenField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public AnalysedRoadPartStructure RoadPart
            {
                get
                {
                    return this.roadPartField;
                }
                set
                {
                    this.roadPartField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ModeOfTransportType.road)]
            public ModeOfTransportType ModeOfTransport
            {
                get
                {
                    return this.modeOfTransportField;
                }
                set
                {
                    this.modeOfTransportField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadPartStructure
        {

            private AnalysedSubPartChoiceStructure[] subpartsField;

            private VariableDistanceStructure distanceField;

            private VehiclesSummaryStructure vehiclesField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Subparts")]
            public AnalysedSubPartChoiceStructure[] Subparts
            {
                get
                {
                    return this.subpartsField;
                }
                set
                {
                    this.subpartsField = value;
                }
            }

            /// <remarks/>
            public VariableDistanceStructure Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructure Vehicles
            {
                get
                {
                    return this.vehiclesField;
                }
                set
                {
                    this.vehiclesField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedSubPartChoiceStructure
        {

            private object itemField;

            private ItemChoiceType4 itemElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedSubPart", typeof(AnalysedSubPartStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Broken", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Discontinuity", typeof(object))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemChoiceType4 ItemElementName
            {
                get
                {
                    return this.itemElementNameField;
                }
                set
                {
                    this.itemElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedSubPartStructure
        {

            private AnalysedPathStructure[] analysedPathField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedPath")]
            public AnalysedPathStructure[] AnalysedPath
            {
                get
                {
                    return this.analysedPathField;
                }
                set
                {
                    this.analysedPathField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedPathStructure
        {

            private AnalysedTraversalChoiceStructure[] pathSegmentField;

            private string descriptionField;

            private SimplifiedRoutePointStructure startPointField;

            private SimplifiedRoutePointStructure endPointField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PathSegment")]
            public AnalysedTraversalChoiceStructure[] PathSegment
            {
                get
                {
                    return this.pathSegmentField;
                }
                set
                {
                    this.pathSegmentField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public SimplifiedRoutePointStructure StartPoint
            {
                get
                {
                    return this.startPointField;
                }
                set
                {
                    this.startPointField = value;
                }
            }

            /// <remarks/>
            public SimplifiedRoutePointStructure EndPoint
            {
                get
                {
                    return this.endPointField;
                }
                set
                {
                    this.endPointField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedTraversalChoiceStructure
        {

            private object itemField;

            private ItemChoiceType3 itemElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Assumed", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Broken", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Discontinuity", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("OffRoad", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Road", typeof(AnalysedRoadTraversalStructure))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemChoiceType3 ItemElementName
            {
                get
                {
                    return this.itemElementNameField;
                }
                set
                {
                    this.itemElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadTraversalStructure
        {

            private RoadIdentificationStructure roadIdentityField;

            private DistanceStructure distanceField;

            private TraversedEntityStructure[] entityTraversalField;

            private SegmentType typeField;

            public AnalysedRoadTraversalStructure()
            {
                this.typeField = SegmentType.normal;
            }

            /// <remarks/>
            public RoadIdentificationStructure RoadIdentity
            {
                get
                {
                    return this.roadIdentityField;
                }
                set
                {
                    this.roadIdentityField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("EntityTraversal")]
            public TraversedEntityStructure[] EntityTraversal
            {
                get
                {
                    return this.entityTraversalField;
                }
                set
                {
                    this.entityTraversalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(SegmentType.normal)]
            public SegmentType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class TraversedEntityStructure
        {

            private TraversedEntityChoiceStructure entityField;

            private DistanceStructure encounteredAtField;

            private DistanceStructure lastingForField;

            /// <remarks/>
            public TraversedEntityChoiceStructure Entity
            {
                get
                {
                    return this.entityField;
                }
                set
                {
                    this.entityField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure EncounteredAt
            {
                get
                {
                    return this.encounteredAtField;
                }
                set
                {
                    this.encounteredAtField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure LastingFor
            {
                get
                {
                    return this.lastingForField;
                }
                set
                {
                    this.lastingForField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class TraversedEntityChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation", typeof(ResolvedAnnotationStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Constraint", typeof(AnalysedConstraintStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Structure", typeof(AnalysedStructureStructure))]
            [System.Xml.Serialization.XmlElementAttribute("WayPoint", typeof(AnalysedWayPointStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedWayPointStructure
        {

            private string descriptionField;

            private SimpleTextStructure textField;

            private ResolvedContactStructure[] contactField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public SimpleTextStructure Text
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

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public ResolvedContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IncludeInSchema = false)]
        public enum ItemChoiceType3
        {

            /// <remarks/>
            Assumed,

            /// <remarks/>
            Broken,

            /// <remarks/>
            Discontinuity,

            /// <remarks/>
            OffRoad,

            /// <remarks/>
            Road,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IncludeInSchema = false)]
        public enum ItemChoiceType4
        {

            /// <remarks/>
            AnalysedSubPart,

            /// <remarks/>
            Broken,

            /// <remarks/>
            Discontinuity,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class VariableDistanceStructure
        {

            private object itemField;

            private DistanceUnitType unitField;

            public VariableDistanceStructure()
            {
                this.unitField = DistanceUnitType.metre;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Distance", typeof(decimal))]
            [System.Xml.Serialization.XmlElementAttribute("DistanceRange", typeof(VariableDistanceStructureDistanceRange))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(DistanceUnitType.metre)]
            public DistanceUnitType Unit
            {
                get
                {
                    return this.unitField;
                }
                set
                {
                    this.unitField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public partial class VariableDistanceStructureDistanceRange
        {

            private string minDistanceField;

            private string maxDistanceField;

            /// <remarks/>
            public string MinDistance
            {
                get
                {
                    return this.minDistanceField;
                }
                set
                {
                    this.minDistanceField = value;
                }
            }

            /// <remarks/>
            public string MaxDistance
            {
                get
                {
                    return this.maxDistanceField;
                }
                set
                {
                    this.maxDistanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructure
        {

            private VehiclesSummaryStructureConfigurationSummaryListPosition[] configurationSummaryListPositionField;

            private VehiclesSummaryStructureOverallLengthListPosition[] overallLengthListPositionField;

            private VehiclesSummaryStructureRigidLengthListPosition[] rigidLengthListPositionField;

            private VehiclesSummaryStructureRearOverhangListPosition[] rearOverhangListPositionField;

            //netweb
            private VehiclesSummaryStructureGroundClearanceListPosition[] groundClearanceListPositionField;

            //netweb
            private VehiclesSummaryStructureReducedGroundClearanceListPosition[] reducedGroundClearanceListPositionField;

            //netweb
            private VehiclesSummaryStructureLeftOverhangListPosition[] leftOverhangListPositionField;

            //netweb
            private VehiclesSummaryStructureRightOverhangListPosition[] rightOverhangListPositionField;

            private VehiclesSummaryStructureFrontOverhangListPosition[] frontOverhangListPositionField;

            private VehiclesSummaryStructureOverallWidthListPosition[] overallWidthListPositionField;

            private VehiclesSummaryStructureSideBySideSpacingListPosition[] sideBySideSpacingListPositionField;

            private VehiclesSummaryStructureOverallHeightListPosition[] overallHeightListPositionField;

            private VehiclesSummaryStructureGrossWeightListPosition[] grossWeightListPositionField;

            private VehiclesSummaryStructureMaxAxleWeightListPosition[] maxAxleWeightListPositionField;

            private VehiclesSummaryStructureVehicleSummaryListPosition[] vehicleSummaryListPositionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ConfigurationSummaryListPosition")]
            public VehiclesSummaryStructureConfigurationSummaryListPosition[] ConfigurationSummaryListPosition
            {
                get
                {
                    return this.configurationSummaryListPositionField;
                }
                set
                {
                    this.configurationSummaryListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OverallLengthListPosition")]
            public VehiclesSummaryStructureOverallLengthListPosition[] OverallLengthListPosition
            {
                get
                {
                    return this.overallLengthListPositionField;
                }
                set
                {
                    this.overallLengthListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("RigidLengthListPosition")]
            public VehiclesSummaryStructureRigidLengthListPosition[] RigidLengthListPosition
            {
                get
                {
                    return this.rigidLengthListPositionField;
                }
                set
                {
                    this.rigidLengthListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("RearOverhangListPosition")]
            public VehiclesSummaryStructureRearOverhangListPosition[] RearOverhangListPosition
            {
                get
                {
                    return this.rearOverhangListPositionField;
                }
                set
                {
                    this.rearOverhangListPositionField = value;
                }
            }
            /// netweb<remarks/>
            [System.Xml.Serialization.XmlElementAttribute("GroundClearancegListPosition")]
            public VehiclesSummaryStructureGroundClearanceListPosition[] GroundClearanceListPosition
            {
                get
                {
                    return this.groundClearanceListPositionField;
                }
                set
                {
                    this.groundClearanceListPositionField = value;
                }
            }

            /// netweb<remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ReducedGroundClearanceListPosition")]
            public VehiclesSummaryStructureReducedGroundClearanceListPosition[] ReducedGroundClearanceListPosition
            {
                get
                {
                    return this.reducedGroundClearanceListPositionField;
                }
                set
                {
                    this.reducedGroundClearanceListPositionField = value;
                }
            }

            /// netweb<remarks/>
            [System.Xml.Serialization.XmlElementAttribute("LeftOverhangListPosition")]
            public VehiclesSummaryStructureLeftOverhangListPosition[] LeftOverhangListPosition
            {
                get
                {
                    return this.leftOverhangListPositionField;
                }
                set
                {
                    this.leftOverhangListPositionField = value;
                }
            }

            /// netweb<remarks/>
            [System.Xml.Serialization.XmlElementAttribute("RightOverhangListPosition")]
            public VehiclesSummaryStructureRightOverhangListPosition[] RightOverhangListPosition
            {
                get
                {
                    return this.rightOverhangListPositionField;
                }
                set
                {
                    this.rightOverhangListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("FrontOverhangListPosition")]
            public VehiclesSummaryStructureFrontOverhangListPosition[] FrontOverhangListPosition
            {
                get
                {
                    return this.frontOverhangListPositionField;
                }
                set
                {
                    this.frontOverhangListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OverallWidthListPosition")]
            public VehiclesSummaryStructureOverallWidthListPosition[] OverallWidthListPosition
            {
                get
                {
                    return this.overallWidthListPositionField;
                }
                set
                {
                    this.overallWidthListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SideBySideSpacingListPosition")]
            public VehiclesSummaryStructureSideBySideSpacingListPosition[] SideBySideSpacingListPosition
            {
                get
                {
                    return this.sideBySideSpacingListPositionField;
                }
                set
                {
                    this.sideBySideSpacingListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OverallHeightListPosition")]
            public VehiclesSummaryStructureOverallHeightListPosition[] OverallHeightListPosition
            {
                get
                {
                    return this.overallHeightListPositionField;
                }
                set
                {
                    this.overallHeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("GrossWeightListPosition")]
            public VehiclesSummaryStructureGrossWeightListPosition[] GrossWeightListPosition
            {
                get
                {
                    return this.grossWeightListPositionField;
                }
                set
                {
                    this.grossWeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("MaxAxleWeightListPosition")]
            public VehiclesSummaryStructureMaxAxleWeightListPosition[] MaxAxleWeightListPosition
            {
                get
                {
                    return this.maxAxleWeightListPositionField;
                }
                set
                {
                    this.maxAxleWeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("VehicleSummaryListPosition")]
            public VehiclesSummaryStructureVehicleSummaryListPosition[] VehicleSummaryListPosition
            {
                get
                {
                    return this.vehicleSummaryListPositionField;
                }
                set
                {
                    this.vehicleSummaryListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureConfigurationSummaryListPosition
        {

            private string configurationSummaryField;

            private VehiclesSummaryStructureConfigurationSummaryListPositionOldConfigurationSummary oldConfigurationSummaryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string ConfigurationSummary
            {
                get
                {
                    return this.configurationSummaryField;
                }
                set
                {
                    this.configurationSummaryField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureConfigurationSummaryListPositionOldConfigurationSummary OldConfigurationSummary
            {
                get
                {
                    return this.oldConfigurationSummaryField;
                }
                set
                {
                    this.oldConfigurationSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureConfigurationSummaryListPositionOldConfigurationSummary
        {

            private string configurationSummaryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string ConfigurationSummary
            {
                get
                {
                    return this.configurationSummaryField;
                }
                set
                {
                    this.configurationSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureOverallLengthListPosition
        {

            private SummaryLengthStructure overallLengthField;

            private VehiclesSummaryStructureOverallLengthListPositionOldOverallLength oldOverallLengthField;

            /// <remarks/>
            public SummaryLengthStructure OverallLength
            {
                get
                {
                    return this.overallLengthField;
                }
                set
                {
                    this.overallLengthField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureOverallLengthListPositionOldOverallLength OldOverallLength
            {
                get
                {
                    return this.oldOverallLengthField;
                }
                set
                {
                    this.oldOverallLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryLengthStructure
        {

            private string includingProjectionsField;

            private string excludingProjectionsField;

            private bool excludingProjectionsFieldSpecified;

            /// <remarks/>
            public string IncludingProjections
            {
                get
                {
                    return this.includingProjectionsField;
                }
                set
                {
                    this.includingProjectionsField = value;
                }
            }

            /// <remarks/>
            public string ExcludingProjections
            {
                get
                {
                    return this.excludingProjectionsField;
                }
                set
                {
                    this.excludingProjectionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ExcludingProjectionsSpecified
            {
                get
                {
                    return this.excludingProjectionsFieldSpecified;
                }
                set
                {
                    this.excludingProjectionsFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureOverallLengthListPositionOldOverallLength
        {

            private SummaryLengthStructure overallLengthField;

            /// <remarks/>
            public SummaryLengthStructure OverallLength
            {
                get
                {
                    return this.overallLengthField;
                }
                set
                {
                    this.overallLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureRigidLengthListPosition
        {

            private string rigidLengthField;

            private bool rigidLengthFieldSpecified;

            private VehiclesSummaryStructureRigidLengthListPositionOldRigidLength oldRigidLengthField;

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RigidLengthSpecified
            {
                get
                {
                    return this.rigidLengthFieldSpecified;
                }
                set
                {
                    this.rigidLengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureRigidLengthListPositionOldRigidLength OldRigidLength
            {
                get
                {
                    return this.oldRigidLengthField;
                }
                set
                {
                    this.oldRigidLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureRigidLengthListPositionOldRigidLength
        {

            private string rigidLengthField;

            private bool rigidLengthFieldSpecified;

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RigidLengthSpecified
            {
                get
                {
                    return this.rigidLengthFieldSpecified;
                }
                set
                {
                    this.rigidLengthFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureRearOverhangListPosition
        {

            private string rearOverhangField;

            private bool rearOverhangFieldSpecified;

            private VehiclesSummaryStructureRearOverhangListPositionOldRearOverhang oldRearOverhangField;

            /// <remarks/>
            public string RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RearOverhangSpecified
            {
                get
                {
                    return this.rearOverhangFieldSpecified;
                }
                set
                {
                    this.rearOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureRearOverhangListPositionOldRearOverhang OldRearOverhang
            {
                get
                {
                    return this.oldRearOverhangField;
                }
                set
                {
                    this.oldRearOverhangField = value;
                }
            }
        }

        /// netweb GroundClearance<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureGroundClearanceListPosition
        {

            private string groundClearanceField;

            private bool GroundClearanceFieldSpecified;

            private VehiclesSummaryStructureGroundClearanceListPositionOldGroundClearance oldGroundClearanceField;

            /// <remarks/>
            public string GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GroundClearanceSpecified
            {
                get
                {
                    return this.GroundClearanceFieldSpecified;
                }
                set
                {
                    this.GroundClearanceFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureGroundClearanceListPositionOldGroundClearance OldGroundClearance
            {
                get
                {
                    return this.oldGroundClearanceField;
                }
                set
                {
                    this.oldGroundClearanceField = value;
                }
            }
        }

        /// netweb ReducedGroundClearance<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureReducedGroundClearanceListPosition
        {

            private string reducedGroundClearanceField;

            private bool ReducedGroundClearanceFieldSpecified;

            private VehiclesSummaryStructureReducedGroundClearanceListPositionOldReducedGroundClearance oldReducedGroundClearanceField;

            /// <remarks/>
            public string ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReducedGroundClearanceSpecified
            {
                get
                {
                    return this.ReducedGroundClearanceFieldSpecified;
                }
                set
                {
                    this.ReducedGroundClearanceFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureReducedGroundClearanceListPositionOldReducedGroundClearance OldReducedGroundClearance
            {
                get
                {
                    return this.oldReducedGroundClearanceField;
                }
                set
                {
                    this.oldReducedGroundClearanceField = value;
                }
            }
        }

        /// netweb LeftOverhang<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureLeftOverhangListPosition
        {

            private string leftOverhangField;

            private bool leftOverhangFieldSpecified;

            private VehiclesSummaryStructureLeftOverhangListPositionOldLeftOverhang oldLeftOverhangField;

            /// <remarks/>
            public string LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LeftOverhangSpecified
            {
                get
                {
                    return this.leftOverhangFieldSpecified;
                }
                set
                {
                    this.leftOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureLeftOverhangListPositionOldLeftOverhang OldLeftOverhang
            {
                get
                {
                    return this.oldLeftOverhangField;
                }
                set
                {
                    this.oldLeftOverhangField = value;
                }
            }
        }

        /// netweb<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureRightOverhangListPosition
        {

            private string rightOverhangField;

            private bool rightOverhangFieldSpecified;

            private VehiclesSummaryStructureRightOverhangListPositionOldRightOverhang oldRightOverhangField;

            /// <remarks/>
            public string RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RightOverhangSpecified
            {
                get
                {
                    return this.rightOverhangFieldSpecified;
                }
                set
                {
                    this.rightOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureRightOverhangListPositionOldRightOverhang OldRightOverhang
            {
                get
                {
                    return this.oldRightOverhangField;
                }
                set
                {
                    this.oldRightOverhangField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureRearOverhangListPositionOldRearOverhang
        {

            private string rearOverhangField;

            private bool rearOverhangFieldSpecified;

            /// <remarks/>
            public string RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RearOverhangSpecified
            {
                get
                {
                    return this.rearOverhangFieldSpecified;
                }
                set
                {
                    this.rearOverhangFieldSpecified = value;
                }
            }
        }

        /// netweb<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureGroundClearanceListPositionOldGroundClearance
        {

            private string groundClearanceField;

            private bool groundClearanceFieldSpecified;

            /// <remarks/>
            public string GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GroundClearanceSpecified
            {
                get
                {
                    return this.groundClearanceFieldSpecified;
                }
                set
                {
                    this.groundClearanceFieldSpecified = value;
                }
            }
        }

        /// netweb<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureReducedGroundClearanceListPositionOldReducedGroundClearance
        {

            private string reducedGroundClearanceField;

            private bool reducedGroundClearanceFieldSpecified;

            /// <remarks/>
            public string ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReducedGroundClearanceSpecified
            {
                get
                {
                    return this.reducedGroundClearanceFieldSpecified;
                }
                set
                {
                    this.reducedGroundClearanceFieldSpecified = value;
                }
            }
        }

        /// netweb<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureLeftOverhangListPositionOldLeftOverhang
        {

            private string leftOverhangField;

            private bool leftOverhangFieldSpecified;

            /// <remarks/>
            public string LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GroundClearanceSpecified
            {
                get
                {
                    return this.leftOverhangFieldSpecified;
                }
                set
                {
                    this.leftOverhangFieldSpecified = value;
                }
            }
        }

        /// netweb<remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureRightOverhangListPositionOldRightOverhang
        {

            private string rightOverhangField;

            private bool rightOverhangFieldSpecified;

            /// <remarks/>
            public string RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RightOverhangSpecified
            {
                get
                {
                    return this.rightOverhangFieldSpecified;
                }
                set
                {
                    this.rightOverhangFieldSpecified = value;
                }
            }
        }
        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureFrontOverhangListPosition
        {
            private string frontOverhangFld;

            private FrontOverhangStructure frontOverhangField;

            private VehiclesSummaryStructureFrontOverhangListPositionOldFrontOverhang oldFrontOverhangField;

            private bool frontOverhangFieldSpecified;

            public string FrontOverhangField
            {
                get
                {
                    return this.frontOverhangFld;
                }
                set
                {
                    this.frontOverhangFld = value;
                }
            }

            /// <remarks/>
            public FrontOverhangStructure FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureFrontOverhangListPositionOldFrontOverhang OldFrontOverhang
            {
                get
                {
                    return this.oldFrontOverhangField;
                }
                set
                {
                    this.oldFrontOverhangField = value;
                }
            }

            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool FrontOverhangSpecified
            {
                get
                {
                    return this.frontOverhangFieldSpecified;
                }
                set
                {
                    this.frontOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class FrontOverhangStructure
        {

            private bool infrontOfCabField;

            private string valueField;

            public FrontOverhangStructure()
            {
                this.infrontOfCabField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool InfrontOfCab
            {
                get
                {
                    return this.infrontOfCabField;
                }
                set
                {
                    this.infrontOfCabField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureFrontOverhangListPositionOldFrontOverhang
        {

            private FrontOverhangStructure frontOverhangField;

            /// <remarks/>
            public FrontOverhangStructure FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureOverallWidthListPosition
        {

            private string overallWidthField;

            private bool overallWidthFieldSpecified;

            private VehiclesSummaryStructureOverallWidthListPositionOldOverallWidth oldOverallWidthField;

            /// <remarks/>
            public string OverallWidth
            {
                get
                {
                    return this.overallWidthField;
                }
                set
                {
                    this.overallWidthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OverallWidthSpecified
            {
                get
                {
                    return this.overallWidthFieldSpecified;
                }
                set
                {
                    this.overallWidthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureOverallWidthListPositionOldOverallWidth OldOverallWidth
            {
                get
                {
                    return this.oldOverallWidthField;
                }
                set
                {
                    this.oldOverallWidthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureOverallWidthListPositionOldOverallWidth
        {

            private string overallWidthField;

            private bool overallWidthFieldSpecified;

            /// <remarks/>
            public string OverallWidth
            {
                get
                {
                    return this.overallWidthField;
                }
                set
                {
                    this.overallWidthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OverallWidthSpecified
            {
                get
                {
                    return this.overallWidthFieldSpecified;
                }
                set
                {
                    this.overallWidthFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureSideBySideSpacingListPosition
        {

            private string sideBySideSpacingField;

            private bool sideBySideSpacingFieldSpecified;

            private VehiclesSummaryStructureSideBySideSpacingListPositionOldSideBySideSpacing oldSideBySideSpacingField;

            /// <remarks/>
            public string SideBySideSpacing
            {
                get
                {
                    return this.sideBySideSpacingField;
                }
                set
                {
                    this.sideBySideSpacingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SideBySideSpacingSpecified
            {
                get
                {
                    return this.sideBySideSpacingFieldSpecified;
                }
                set
                {
                    this.sideBySideSpacingFieldSpecified = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureSideBySideSpacingListPositionOldSideBySideSpacing OldSideBySideSpacing
            {
                get
                {
                    return this.oldSideBySideSpacingField;
                }
                set
                {
                    this.oldSideBySideSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureSideBySideSpacingListPositionOldSideBySideSpacing
        {

            private string sideBySideSpacingField;

            private bool sideBySideSpacingFieldSpecified;

            /// <remarks/>
            public string SideBySideSpacing
            {
                get
                {
                    return this.sideBySideSpacingField;
                }
                set
                {
                    this.sideBySideSpacingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SideBySideSpacingSpecified
            {
                get
                {
                    return this.sideBySideSpacingFieldSpecified;
                }
                set
                {
                    this.sideBySideSpacingFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureOverallHeightListPosition
        {

            private SummaryHeightStructure overallHeightField;

            private VehiclesSummaryStructureOverallHeightListPositionOldOverallHeight oldOverallHeightField;

            /// <remarks/>
            public SummaryHeightStructure OverallHeight
            {
                get
                {
                    return this.overallHeightField;
                }
                set
                {
                    this.overallHeightField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureOverallHeightListPositionOldOverallHeight OldOverallHeight
            {
                get
                {
                    return this.oldOverallHeightField;
                }
                set
                {
                    this.oldOverallHeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryHeightStructure
        {

            private string maxHeightField;

            private string reducibleHeightField;

            private bool reducibleHeightFieldSpecified;

            /// <remarks/>
            public string MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            public string ReducibleHeight
            {
                get
                {
                    return this.reducibleHeightField;
                }
                set
                {
                    this.reducibleHeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReducibleHeightSpecified
            {
                get
                {
                    return this.reducibleHeightFieldSpecified;
                }
                set
                {
                    this.reducibleHeightFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureOverallHeightListPositionOldOverallHeight
        {

            private SummaryHeightStructure overallHeightField;

            /// <remarks/>
            public SummaryHeightStructure OverallHeight
            {
                get
                {
                    return this.overallHeightField;
                }
                set
                {
                    this.overallHeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureGrossWeightListPosition
        {

            private GrossWeightStructure grossWeightField;

            private VehiclesSummaryStructureGrossWeightListPositionOldGrossWeight oldGrossWeightField;

            /// <remarks/>
            public GrossWeightStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureGrossWeightListPositionOldGrossWeight OldGrossWeight
            {
                get
                {
                    return this.oldGrossWeightField;
                }
                set
                {
                    this.oldGrossWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class GrossWeightStructure : SummaryWeightStructure
        {

            private bool excludesTractorsField;

            public GrossWeightStructure()
            {
                this.excludesTractorsField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool ExcludesTractors
            {
                get
                {
                    return this.excludesTractorsField;
                }
                set
                {
                    this.excludesTractorsField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(GrossWeightStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryWeightStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("CandUDeclared", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Weight", typeof(string), DataType = "integer")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureGrossWeightListPositionOldGrossWeight
        {

            private GrossWeightStructure grossWeightField;

            /// <remarks/>
            public GrossWeightStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureMaxAxleWeightListPosition
        {

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            private VehiclesSummaryStructureMaxAxleWeightListPositionOldMaxAxleWeight oldMaxAxleWeightField;

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureMaxAxleWeightListPositionOldMaxAxleWeight OldMaxAxleWeight
            {
                get
                {
                    return this.oldMaxAxleWeightField;
                }
                set
                {
                    this.oldMaxAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryMaxAxleWeightStructure
        {

            private object itemField;

            private ItemChoiceType1 itemElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("CandUAxleDeclared", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Tracked", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Weight", typeof(string), DataType = "integer")]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemChoiceType1 ItemElementName
            {
                get
                {
                    return this.itemElementNameField;
                }
                set
                {
                    this.itemElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle", IncludeInSchema = false)]
        public enum ItemChoiceType1
        {

            /// <remarks/>
            CandUAxleDeclared,

            /// <remarks/>
            Tracked,

            /// <remarks/>
            Weight,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureMaxAxleWeightListPositionOldMaxAxleWeight
        {

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureVehicleSummaryListPosition
        {

            private VehicleSummaryStructure vehicleSummaryField;

            private VehiclesSummaryStructureVehicleSummaryListPositionOldVehicleSummary oldVehicleSummaryField;

            /// <remarks/>
            public VehicleSummaryStructure VehicleSummary
            {
                get
                {
                    return this.vehicleSummaryField;
                }
                set
                {
                    this.vehicleSummaryField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructureVehicleSummaryListPositionOldVehicleSummary OldVehicleSummary
            {
                get
                {
                    return this.oldVehicleSummaryField;
                }
                set
                {
                    this.oldVehicleSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryStructure
        {

            private SummaryWeightConformanceType weightConformanceField;

            private VehicleSummaryStructureOldWeightConformance oldWeightConformanceField;

            private VehicleSummaryStructureConfigurationIdentityListPosition[] configurationIdentityListPositionField;

            private VehicleSummaryTypeChoiceStructure configurationField;

            private VehicleSummaryStructureOldConfiguration oldConfigurationField;

            private VehicleConfigurationType configurationTypeField;

            private VehicleSummaryStructureOldConfigurationType oldConfigurationTypeField;

            private string alternativeIdField;

            /// <remarks/>
            public SummaryWeightConformanceType WeightConformance
            {
                get
                {
                    return this.weightConformanceField;
                }
                set
                {
                    this.weightConformanceField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryStructureOldWeightConformance OldWeightConformance
            {
                get
                {
                    return this.oldWeightConformanceField;
                }
                set
                {
                    this.oldWeightConformanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ConfigurationIdentityListPosition")]
            public VehicleSummaryStructureConfigurationIdentityListPosition[] ConfigurationIdentityListPosition
            {
                get
                {
                    return this.configurationIdentityListPositionField;
                }
                set
                {
                    this.configurationIdentityListPositionField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryTypeChoiceStructure Configuration
            {
                get
                {
                    return this.configurationField;
                }
                set
                {
                    this.configurationField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryStructureOldConfiguration OldConfiguration
            {
                get
                {
                    return this.oldConfigurationField;
                }
                set
                {
                    this.oldConfigurationField = value;
                }
            }

            /// <remarks/>
            public VehicleConfigurationType ConfigurationType
            {
                get
                {
                    return this.configurationTypeField;
                }
                set
                {
                    this.configurationTypeField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryStructureOldConfigurationType OldConfigurationType
            {
                get
                {
                    return this.oldConfigurationTypeField;
                }
                set
                {
                    this.oldConfigurationTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string AlternativeId
            {
                get
                {
                    return this.alternativeIdField;
                }
                set
                {
                    this.alternativeIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public enum SummaryWeightConformanceType
        {

            /// <remarks/>
            other,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("Heavy STGO")]
            heavystgo,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("Light STGO")]
            lightstgo,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("AWR schedule 3")]
            awrschedule3,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("C AND U")]
            candu,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryStructureOldWeightConformance
        {

            private SummaryWeightConformanceType weightConformanceField;

            /// <remarks/>
            public SummaryWeightConformanceType WeightConformance
            {
                get
                {
                    return this.weightConformanceField;
                }
                set
                {
                    this.weightConformanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryStructureConfigurationIdentityListPosition
        {

            private SummaryVehicleIdentificationStructure configurationIdentityField;

            private VehicleSummaryStructureConfigurationIdentityListPositionOldConfigurationIdentity oldConfigurationIdentityField;

            /// <remarks/>
            public SummaryVehicleIdentificationStructure ConfigurationIdentity
            {
                get
                {
                    return this.configurationIdentityField;
                }
                set
                {
                    this.configurationIdentityField = value;
                }
            }

            /// <remarks/>
            public VehicleSummaryStructureConfigurationIdentityListPositionOldConfigurationIdentity OldConfigurationIdentity
            {
                get
                {
                    return this.oldConfigurationIdentityField;
                }
                set
                {
                    this.oldConfigurationIdentityField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryVehicleIdentificationStructure
        {

            private string plateNoField;

            private SummaryVehicleIdentificationStructureOldPlateNo oldPlateNoField;

            private string fleetNoField;

            private SummaryVehicleIdentificationStructureOldFleetNo oldFleetNoField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string PlateNo
            {
                get
                {
                    return this.plateNoField;
                }
                set
                {
                    this.plateNoField = value;
                }
            }

            /// <remarks/>
            public SummaryVehicleIdentificationStructureOldPlateNo OldPlateNo
            {
                get
                {
                    return this.oldPlateNoField;
                }
                set
                {
                    this.oldPlateNoField = value;
                }
            }

            /// <remarks/>
            public string FleetNo
            {
                get
                {
                    return this.fleetNoField;
                }
                set
                {
                    this.fleetNoField = value;
                }
            }

            /// <remarks/>
            public SummaryVehicleIdentificationStructureOldFleetNo OldFleetNo
            {
                get
                {
                    return this.oldFleetNoField;
                }
                set
                {
                    this.oldFleetNoField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryVehicleIdentificationStructureOldPlateNo
        {

            private string plateNoField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string PlateNo
            {
                get
                {
                    return this.plateNoField;
                }
                set
                {
                    this.plateNoField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryVehicleIdentificationStructureOldFleetNo
        {

            private string fleetNoField;

            /// <remarks/>
            public string FleetNo
            {
                get
                {
                    return this.fleetNoField;
                }
                set
                {
                    this.fleetNoField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryStructureConfigurationIdentityListPositionOldConfigurationIdentity
        {

            private SummaryVehicleIdentificationStructure configurationIdentityField;

            /// <remarks/>
            public SummaryVehicleIdentificationStructure ConfigurationIdentity
            {
                get
                {
                    return this.configurationIdentityField;
                }
                set
                {
                    this.configurationIdentityField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryTypeChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("NonSemiVehicle", typeof(NonSemiTrailerSummaryStructure))]
            [System.Xml.Serialization.XmlElementAttribute("SemiVehicle", typeof(SemiTrailerSummaryStructure))]
            [System.Xml.Serialization.XmlElementAttribute("TrackedVehicle", typeof(TrackedVehicleSummaryStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class NonSemiTrailerSummaryStructure
        {

            private NonSemiTrailerSummaryStructureComponentListPosition[] componentListPositionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ComponentListPosition")]
            public NonSemiTrailerSummaryStructureComponentListPosition[] ComponentListPosition
            {
                get
                {
                    return this.componentListPositionField;
                }
                set
                {
                    this.componentListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class NonSemiTrailerSummaryStructureComponentListPosition
        {

            private ComponentSummaryStructure componentField;

            private NonSemiTrailerSummaryStructureComponentListPositionOldComponent oldComponentField;

            /// <remarks/>
            public ComponentSummaryStructure Component
            {
                get
                {
                    return this.componentField;
                }
                set
                {
                    this.componentField = value;
                }
            }

            /// <remarks/>
            public NonSemiTrailerSummaryStructureComponentListPositionOldComponent OldComponent
            {
                get
                {
                    return this.oldComponentField;
                }
                set
                {
                    this.oldComponentField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class ComponentSummaryStructure
        {

            private object itemField;

            private VehicleComponentType componentTypeField;

            private VehicleComponentSubType componentSubTypeField;

            private string longitudeField;

            private SummarySideType sideField;

            private bool sideFieldSpecified;

            public ComponentSummaryStructure()
            {
                this.longitudeField = "1";
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("DrawbarTractor", typeof(DrawbarTractorSummaryStructure))]
            [System.Xml.Serialization.XmlElementAttribute("LoadBearing", typeof(LoadBearingSummaryStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public VehicleComponentType ComponentType
            {
                get
                {
                    return this.componentTypeField;
                }
                set
                {
                    this.componentTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public VehicleComponentSubType ComponentSubType
            {
                get
                {
                    return this.componentSubTypeField;
                }
                set
                {
                    this.componentSubTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            [System.ComponentModel.DefaultValueAttribute("1")]
            public string Longitude
            {
                get
                {
                    return this.longitudeField;
                }
                set
                {
                    this.longitudeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public SummarySideType Side
            {
                get
                {
                    return this.sideField;
                }
                set
                {
                    this.sideField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SideSpecified
            {
                get
                {
                    return this.sideFieldSpecified;
                }
                set
                {
                    this.sideFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class DrawbarTractorSummaryStructure
        {

            private string summaryField;

            private DrawbarTractorSummaryStructureOldSummary oldSummaryField;

            private string weightField;

            //private SummaryWeightStructure weightField;

            private DrawbarTractorSummaryStructureOldWeight oldWeightField;

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            private DrawbarTractorSummaryStructureOldMaxAxleWeight oldMaxAxleWeightField;

            private SummaryAxleStructure axleConfigurationField;

            private DrawbarTractorSummaryStructureOldAxleConfiguration oldAxleConfigurationField;

            private string lengthField;

            private bool lengthFieldSpecified;

            private DrawbarTractorSummaryStructureOldLength oldLengthField;

            private string axleSpacingToFollowingField;

            private bool axleSpacingToFollowingFieldSpecified;

            private DrawbarTractorSummaryStructureOldAxleSpacingToFollowing oldAxleSpacingToFollowingField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public DrawbarTractorSummaryStructureOldSummary OldSummary
            {
                get
                {
                    return this.oldSummaryField;
                }
                set
                {
                    this.oldSummaryField = value;
                }
            }

            public string Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }

            /// <remarks/>
            //public SummaryWeightStructure Weight
            //{
            //    get
            //    {
            //        return this.weightField;
            //    }
            //    set
            //    {
            //        this.weightField = value;
            //    }
            //}

            /// <remarks/>
            public DrawbarTractorSummaryStructureOldWeight OldWeight
            {
                get
                {
                    return this.oldWeightField;
                }
                set
                {
                    this.oldWeightField = value;
                }
            }

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public DrawbarTractorSummaryStructureOldMaxAxleWeight OldMaxAxleWeight
            {
                get
                {
                    return this.oldMaxAxleWeightField;
                }
                set
                {
                    this.oldMaxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructure AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }

            /// <remarks/>
            public DrawbarTractorSummaryStructureOldAxleConfiguration OldAxleConfiguration
            {
                get
                {
                    return this.oldAxleConfigurationField;
                }
                set
                {
                    this.oldAxleConfigurationField = value;
                }
            }

            /// <remarks/>
            public string Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public DrawbarTractorSummaryStructureOldLength OldLength
            {
                get
                {
                    return this.oldLengthField;
                }
                set
                {
                    this.oldLengthField = value;
                }
            }

            /// <remarks/>
            public string AxleSpacingToFollowing
            {
                get
                {
                    return this.axleSpacingToFollowingField;
                }
                set
                {
                    this.axleSpacingToFollowingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleSpacingToFollowingSpecified
            {
                get
                {
                    return this.axleSpacingToFollowingFieldSpecified;
                }
                set
                {
                    this.axleSpacingToFollowingFieldSpecified = value;
                }
            }

            /// <remarks/>
            public DrawbarTractorSummaryStructureOldAxleSpacingToFollowing OldAxleSpacingToFollowing
            {
                get
                {
                    return this.oldAxleSpacingToFollowingField;
                }
                set
                {
                    this.oldAxleSpacingToFollowingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class DrawbarTractorSummaryStructureOldSummary
        {

            private string summaryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class DrawbarTractorSummaryStructureOldWeight
        {

            private SummaryWeightStructure weightField;

            /// <remarks/>
            public SummaryWeightStructure Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class DrawbarTractorSummaryStructureOldMaxAxleWeight
        {

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructure
        {

            private string numberOfAxlesField;

            private SummaryAxleStructureOldNumberOfAxles oldNumberOfAxlesField;

            private SummaryAxleStructureAxleWeightListPosition[] axleWeightListPositionField;

            private SummaryAxleStructureWheelsPerAxleListPosition[] wheelsPerAxleListPositionField;

            private SummaryAxleStructureAxleSpacingListPosition[] axleSpacingListPositionField;

            private SummaryAxleStructureTyreSizeListPosition[] tyreSizeListPositionField;

            private SummaryAxleStructureWheelSpacingListPosition[] wheelSpacingListPositionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string NumberOfAxles
            {
                get
                {
                    return this.numberOfAxlesField;
                }
                set
                {
                    this.numberOfAxlesField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructureOldNumberOfAxles OldNumberOfAxles
            {
                get
                {
                    return this.oldNumberOfAxlesField;
                }
                set
                {
                    this.oldNumberOfAxlesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleWeightListPosition")]
            public SummaryAxleStructureAxleWeightListPosition[] AxleWeightListPosition
            {
                get
                {
                    return this.axleWeightListPositionField;
                }
                set
                {
                    this.axleWeightListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("WheelsPerAxleListPosition")]
            public SummaryAxleStructureWheelsPerAxleListPosition[] WheelsPerAxleListPosition
            {
                get
                {
                    return this.wheelsPerAxleListPositionField;
                }
                set
                {
                    this.wheelsPerAxleListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleSpacingListPosition")]
            public SummaryAxleStructureAxleSpacingListPosition[] AxleSpacingListPosition
            {
                get
                {
                    return this.axleSpacingListPositionField;
                }
                set
                {
                    this.axleSpacingListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("TyreSizeListPosition")]
            public SummaryAxleStructureTyreSizeListPosition[] TyreSizeListPosition
            {
                get
                {
                    return this.tyreSizeListPositionField;
                }
                set
                {
                    this.tyreSizeListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("WheelSpacingListPosition")]
            public SummaryAxleStructureWheelSpacingListPosition[] WheelSpacingListPosition
            {
                get
                {
                    return this.wheelSpacingListPositionField;
                }
                set
                {
                    this.wheelSpacingListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureOldNumberOfAxles
        {

            private string numberOfAxlesField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string NumberOfAxles
            {
                get
                {
                    return this.numberOfAxlesField;
                }
                set
                {
                    this.numberOfAxlesField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureAxleWeightListPosition
        {

            private AxleWeightSummaryStructure axleWeightField;

            private SummaryAxleStructureAxleWeightListPositionOldAxleWeight oldAxleWeightField;

            /// <remarks/>
            public AxleWeightSummaryStructure AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructureAxleWeightListPositionOldAxleWeight OldAxleWeight
            {
                get
                {
                    return this.oldAxleWeightField;
                }
                set
                {
                    this.oldAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class AxleWeightSummaryStructure
        {

            private string axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute(DataType = "integer")]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureAxleWeightListPositionOldAxleWeight
        {

            private AxleWeightSummaryStructure axleWeightField;

            /// <remarks/>
            public AxleWeightSummaryStructure AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureWheelsPerAxleListPosition
        {

            private WheelsPerAxleSummaryStructure wheelsPerAxleField;

            private SummaryAxleStructureWheelsPerAxleListPositionOldWheelsPerAxle oldWheelsPerAxleField;

            /// <remarks/>
            public WheelsPerAxleSummaryStructure WheelsPerAxle
            {
                get
                {
                    return this.wheelsPerAxleField;
                }
                set
                {
                    this.wheelsPerAxleField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructureWheelsPerAxleListPositionOldWheelsPerAxle OldWheelsPerAxle
            {
                get
                {
                    return this.oldWheelsPerAxleField;
                }
                set
                {
                    this.oldWheelsPerAxleField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class WheelsPerAxleSummaryStructure
        {

            private string axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute(DataType = "integer")]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureWheelsPerAxleListPositionOldWheelsPerAxle
        {

            private WheelsPerAxleSummaryStructure wheelsPerAxleField;

            /// <remarks/>
            public WheelsPerAxleSummaryStructure WheelsPerAxle
            {
                get
                {
                    return this.wheelsPerAxleField;
                }
                set
                {
                    this.wheelsPerAxleField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureAxleSpacingListPosition
        {

            private AxleSpacingSummaryStructure axleSpacingField;

            private SummaryAxleStructureAxleSpacingListPositionOldAxleSpacing oldAxleSpacingField;

            /// <remarks/>
            public AxleSpacingSummaryStructure AxleSpacing
            {
                get
                {
                    return this.axleSpacingField;
                }
                set
                {
                    this.axleSpacingField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructureAxleSpacingListPositionOldAxleSpacing OldAxleSpacing
            {
                get
                {
                    return this.oldAxleSpacingField;
                }
                set
                {
                    this.oldAxleSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class AxleSpacingSummaryStructure
        {

            private string axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureAxleSpacingListPositionOldAxleSpacing
        {

            private AxleSpacingSummaryStructure axleSpacingField;

            /// <remarks/>
            public AxleSpacingSummaryStructure AxleSpacing
            {
                get
                {
                    return this.axleSpacingField;
                }
                set
                {
                    this.axleSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureTyreSizeListPosition
        {

            private TyreSizeSummaryStructure tyreSizeField;

            private SummaryAxleStructureTyreSizeListPositionOldTyreSize oldTyreSizeField;

            /// <remarks/>
            public TyreSizeSummaryStructure TyreSize
            {
                get
                {
                    return this.tyreSizeField;
                }
                set
                {
                    this.tyreSizeField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructureTyreSizeListPositionOldTyreSize OldTyreSize
            {
                get
                {
                    return this.oldTyreSizeField;
                }
                set
                {
                    this.oldTyreSizeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class TyreSizeSummaryStructure
        {

            private string axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute(DataType = "token")]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureTyreSizeListPositionOldTyreSize
        {

            private TyreSizeSummaryStructure tyreSizeField;

            /// <remarks/>
            public TyreSizeSummaryStructure TyreSize
            {
                get
                {
                    return this.tyreSizeField;
                }
                set
                {
                    this.tyreSizeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureWheelSpacingListPosition
        {

            private WheelSpacingSummaryStructure wheelSpacingField;

            private SummaryAxleStructureWheelSpacingListPositionOldWheelSpacing oldWheelSpacingField;

            /// <remarks/>
            public WheelSpacingSummaryStructure WheelSpacing
            {
                get
                {
                    return this.wheelSpacingField;
                }
                set
                {
                    this.wheelSpacingField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructureWheelSpacingListPositionOldWheelSpacing OldWheelSpacing
            {
                get
                {
                    return this.oldWheelSpacingField;
                }
                set
                {
                    this.oldWheelSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class WheelSpacingSummaryStructure
        {

            private string axleCountField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
            public string AxleCount
            {
                get
                {
                    return this.axleCountField;
                }
                set
                {
                    this.axleCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SummaryAxleStructureWheelSpacingListPositionOldWheelSpacing
        {

            private WheelSpacingSummaryStructure wheelSpacingField;

            /// <remarks/>
            public WheelSpacingSummaryStructure WheelSpacing
            {
                get
                {
                    return this.wheelSpacingField;
                }
                set
                {
                    this.wheelSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class DrawbarTractorSummaryStructureOldAxleConfiguration
        {

            private SummaryAxleStructure axleConfigurationField;

            /// <remarks/>
            public SummaryAxleStructure AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class DrawbarTractorSummaryStructureOldLength
        {

            private string lengthField;

            private bool lengthFieldSpecified;

            /// <remarks/>
            public string Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class DrawbarTractorSummaryStructureOldAxleSpacingToFollowing
        {

            private string axleSpacingToFollowingField;

            private bool axleSpacingToFollowingFieldSpecified;

            /// <remarks/>
            public string AxleSpacingToFollowing
            {
                get
                {
                    return this.axleSpacingToFollowingField;
                }
                set
                {
                    this.axleSpacingToFollowingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleSpacingToFollowingSpecified
            {
                get
                {
                    return this.axleSpacingToFollowingFieldSpecified;
                }
                set
                {
                    this.axleSpacingToFollowingFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructure
        {

            private string summaryField;

            private LoadBearingSummaryStructureOldSummary oldSummaryField;

            private string weightField;

            //private SummaryWeightStructure weightField;

            private LoadBearingSummaryStructureOldWeight oldWeightField;

            private bool isSteerableAtRearField;

            private bool isSteerableAtRearFieldSpecified;

            private LoadBearingSummaryStructureOldIsSteerableAtRear oldIsSteerableAtRearField;

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            private LoadBearingSummaryStructureOldMaxAxleWeight oldMaxAxleWeightField;

            private SummaryAxleStructure axleConfigurationField;

            private LoadBearingSummaryStructureOldAxleConfiguration oldAxleConfigurationField;

            private string rigidLengthField;

            private LoadBearingSummaryStructureOldRigidLength oldRigidLengthField;

            private string widthField;

            private LoadBearingSummaryStructureOldWidth oldWidthField;

            private SummaryHeightStructure heightField;

            private LoadBearingSummaryStructureOldHeight oldHeightField;

            private string wheelbaseField;

            private bool wheelbaseFieldSpecified;

            private LoadBearingSummaryStructureOldWheelbase oldWheelbaseField;

            private string leftOverhangField;

            private bool leftOverhangFieldSpecified;

            private LoadBearingSummaryStructureOldLeftOverhang oldLeftOverhangField;

            private string rightOverhangField;

            private bool rightOverhangFieldSpecified;

            private LoadBearingSummaryStructureOldRightOverhang oldRightOverhangField;

            private string frontOverhangField;

            private bool frontOverhangFieldSpecified;

            private LoadBearingSummaryStructureOldFrontOverhang oldFrontOverhangField;

            private string rearOverhangField;

            private LoadBearingSummaryStructureOldRearOverhang oldRearOverhangField;

            private string groundClearanceField;

            private bool groundClearanceFieldSpecified;

            private LoadBearingSummaryStructureOldGroundClearance oldGroundClearanceField;

            private string reducedGroundClearanceField;

            private bool reducedGroundClearanceFieldSpecified;

            private LoadBearingSummaryStructureOldReducedGroundClearance oldReducedGroundClearanceField;

            private string outsideTrackField;

            private bool outsideTrackFieldSpecified;

            private LoadBearingSummaryStructureOldOutsideTrack oldOutsideTrackField;

            private string axleSpacingToFollowingField;

            private bool axleSpacingToFollowingFieldSpecified;

            private LoadBearingSummaryStructureOldAxleSpacingToFollowing oldAxleSpacingToFollowingField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldSummary OldSummary
            {
                get
                {
                    return this.oldSummaryField;
                }
                set
                {
                    this.oldSummaryField = value;
                }
            }

            public string Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }

            /// <remarks/>
            //public SummaryWeightStructure Weight
            //{
            //    get
            //    {
            //        return this.weightField;
            //    }
            //    set
            //    {
            //        this.weightField = value;
            //    }
            //}

            /// <remarks/>
            public LoadBearingSummaryStructureOldWeight OldWeight
            {
                get
                {
                    return this.oldWeightField;
                }
                set
                {
                    this.oldWeightField = value;
                }
            }

            /// <remarks/>
            public bool IsSteerableAtRear
            {
                get
                {
                    return this.isSteerableAtRearField;
                }
                set
                {
                    this.isSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IsSteerableAtRearSpecified
            {
                get
                {
                    return this.isSteerableAtRearFieldSpecified;
                }
                set
                {
                    this.isSteerableAtRearFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldIsSteerableAtRear OldIsSteerableAtRear
            {
                get
                {
                    return this.oldIsSteerableAtRearField;
                }
                set
                {
                    this.oldIsSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldMaxAxleWeight OldMaxAxleWeight
            {
                get
                {
                    return this.oldMaxAxleWeightField;
                }
                set
                {
                    this.oldMaxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructure AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldAxleConfiguration OldAxleConfiguration
            {
                get
                {
                    return this.oldAxleConfigurationField;
                }
                set
                {
                    this.oldAxleConfigurationField = value;
                }
            }

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldRigidLength OldRigidLength
            {
                get
                {
                    return this.oldRigidLengthField;
                }
                set
                {
                    this.oldRigidLengthField = value;
                }
            }

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldWidth OldWidth
            {
                get
                {
                    return this.oldWidthField;
                }
                set
                {
                    this.oldWidthField = value;
                }
            }

            /// <remarks/>
            public SummaryHeightStructure Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldHeight OldHeight
            {
                get
                {
                    return this.oldHeightField;
                }
                set
                {
                    this.oldHeightField = value;
                }
            }

            /// <remarks/>
            public string Wheelbase
            {
                get
                {
                    return this.wheelbaseField;
                }
                set
                {
                    this.wheelbaseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool WheelbaseSpecified
            {
                get
                {
                    return this.wheelbaseFieldSpecified;
                }
                set
                {
                    this.wheelbaseFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldWheelbase OldWheelbase
            {
                get
                {
                    return this.oldWheelbaseField;
                }
                set
                {
                    this.oldWheelbaseField = value;
                }
            }

            /// <remarks/>
            public string LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LeftOverhangSpecified
            {
                get
                {
                    return this.leftOverhangFieldSpecified;
                }
                set
                {
                    this.leftOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldLeftOverhang OldLeftOverhang
            {
                get
                {
                    return this.oldLeftOverhangField;
                }
                set
                {
                    this.oldLeftOverhangField = value;
                }
            }

            /// <remarks/>
            public string RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RightOverhangSpecified
            {
                get
                {
                    return this.rightOverhangFieldSpecified;
                }
                set
                {
                    this.rightOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldRightOverhang OldRightOverhang
            {
                get
                {
                    return this.oldRightOverhangField;
                }
                set
                {
                    this.oldRightOverhangField = value;
                }
            }

            /// <remarks/>
            public string FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool FrontOverhangSpecified
            {
                get
                {
                    return this.frontOverhangFieldSpecified;
                }
                set
                {
                    this.frontOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldFrontOverhang OldFrontOverhang
            {
                get
                {
                    return this.oldFrontOverhangField;
                }
                set
                {
                    this.oldFrontOverhangField = value;
                }
            }

            /// <remarks/>
            public string RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldRearOverhang OldRearOverhang
            {
                get
                {
                    return this.oldRearOverhangField;
                }
                set
                {
                    this.oldRearOverhangField = value;
                }
            }

            /// <remarks/>
            public string GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GroundClearanceSpecified
            {
                get
                {
                    return this.groundClearanceFieldSpecified;
                }
                set
                {
                    this.groundClearanceFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldGroundClearance OldGroundClearance
            {
                get
                {
                    return this.oldGroundClearanceField;
                }
                set
                {
                    this.oldGroundClearanceField = value;
                }
            }

            /// <remarks/>
            public string ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReducedGroundClearanceSpecified
            {
                get
                {
                    return this.reducedGroundClearanceFieldSpecified;
                }
                set
                {
                    this.reducedGroundClearanceFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldReducedGroundClearance OldReducedGroundClearance
            {
                get
                {
                    return this.oldReducedGroundClearanceField;
                }
                set
                {
                    this.oldReducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            public string OutsideTrack
            {
                get
                {
                    return this.outsideTrackField;
                }
                set
                {
                    this.outsideTrackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OutsideTrackSpecified
            {
                get
                {
                    return this.outsideTrackFieldSpecified;
                }
                set
                {
                    this.outsideTrackFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldOutsideTrack OldOutsideTrack
            {
                get
                {
                    return this.oldOutsideTrackField;
                }
                set
                {
                    this.oldOutsideTrackField = value;
                }
            }

            /// <remarks/>
            public string AxleSpacingToFollowing
            {
                get
                {
                    return this.axleSpacingToFollowingField;
                }
                set
                {
                    this.axleSpacingToFollowingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleSpacingToFollowingSpecified
            {
                get
                {
                    return this.axleSpacingToFollowingFieldSpecified;
                }
                set
                {
                    this.axleSpacingToFollowingFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadBearingSummaryStructureOldAxleSpacingToFollowing OldAxleSpacingToFollowing
            {
                get
                {
                    return this.oldAxleSpacingToFollowingField;
                }
                set
                {
                    this.oldAxleSpacingToFollowingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldSummary
        {

            private string summaryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldWeight
        {

            private SummaryWeightStructure weightField;

            /// <remarks/>
            public SummaryWeightStructure Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldIsSteerableAtRear
        {

            private bool isSteerableAtRearField;

            private bool isSteerableAtRearFieldSpecified;

            /// <remarks/>
            public bool IsSteerableAtRear
            {
                get
                {
                    return this.isSteerableAtRearField;
                }
                set
                {
                    this.isSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IsSteerableAtRearSpecified
            {
                get
                {
                    return this.isSteerableAtRearFieldSpecified;
                }
                set
                {
                    this.isSteerableAtRearFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldMaxAxleWeight
        {

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldAxleConfiguration
        {

            private SummaryAxleStructure axleConfigurationField;

            /// <remarks/>
            public SummaryAxleStructure AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldRigidLength
        {

            private string rigidLengthField;

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldWidth
        {

            private string widthField;

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldHeight
        {

            private SummaryHeightStructure heightField;

            /// <remarks/>
            public SummaryHeightStructure Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldWheelbase
        {

            private string wheelbaseField;

            private bool wheelbaseFieldSpecified;

            /// <remarks/>
            public string Wheelbase
            {
                get
                {
                    return this.wheelbaseField;
                }
                set
                {
                    this.wheelbaseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool WheelbaseSpecified
            {
                get
                {
                    return this.wheelbaseFieldSpecified;
                }
                set
                {
                    this.wheelbaseFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldLeftOverhang
        {

            private string leftOverhangField;

            private bool leftOverhangFieldSpecified;

            /// <remarks/>
            public string LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LeftOverhangSpecified
            {
                get
                {
                    return this.leftOverhangFieldSpecified;
                }
                set
                {
                    this.leftOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldRightOverhang
        {

            private string rightOverhangField;

            private bool rightOverhangFieldSpecified;

            /// <remarks/>
            public string RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RightOverhangSpecified
            {
                get
                {
                    return this.rightOverhangFieldSpecified;
                }
                set
                {
                    this.rightOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldFrontOverhang
        {

            private string frontOverhangField;

            private bool frontOverhangFieldSpecified;

            /// <remarks/>
            public string FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool FrontOverhangSpecified
            {
                get
                {
                    return this.frontOverhangFieldSpecified;
                }
                set
                {
                    this.frontOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldRearOverhang
        {

            private string rearOverhangField;

            /// <remarks/>
            public string RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldGroundClearance
        {

            private string groundClearanceField;

            private bool groundClearanceFieldSpecified;

            /// <remarks/>
            public string GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GroundClearanceSpecified
            {
                get
                {
                    return this.groundClearanceFieldSpecified;
                }
                set
                {
                    this.groundClearanceFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldReducedGroundClearance
        {

            private string reducedGroundClearanceField;

            private bool reducedGroundClearanceFieldSpecified;

            /// <remarks/>
            public string ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReducedGroundClearanceSpecified
            {
                get
                {
                    return this.reducedGroundClearanceFieldSpecified;
                }
                set
                {
                    this.reducedGroundClearanceFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldOutsideTrack
        {

            private string outsideTrackField;

            private bool outsideTrackFieldSpecified;

            /// <remarks/>
            public string OutsideTrack
            {
                get
                {
                    return this.outsideTrackField;
                }
                set
                {
                    this.outsideTrackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OutsideTrackSpecified
            {
                get
                {
                    return this.outsideTrackFieldSpecified;
                }
                set
                {
                    this.outsideTrackFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class LoadBearingSummaryStructureOldAxleSpacingToFollowing
        {

            private string axleSpacingToFollowingField;

            private bool axleSpacingToFollowingFieldSpecified;

            /// <remarks/>
            public string AxleSpacingToFollowing
            {
                get
                {
                    return this.axleSpacingToFollowingField;
                }
                set
                {
                    this.axleSpacingToFollowingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleSpacingToFollowingSpecified
            {
                get
                {
                    return this.axleSpacingToFollowingFieldSpecified;
                }
                set
                {
                    this.axleSpacingToFollowingFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public enum SummarySideType
        {

            /// <remarks/>
            right,

            /// <remarks/>
            left,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class NonSemiTrailerSummaryStructureComponentListPositionOldComponent
        {

            private ComponentSummaryStructure componentField;

            /// <remarks/>
            public ComponentSummaryStructure Component
            {
                get
                {
                    return this.componentField;
                }
                set
                {
                    this.componentField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructure
        {

            private string summaryField;

            private SemiTrailerSummaryStructureOldSummary oldSummaryField;

            private SummaryWeightStructure grossWeightField;

            private SemiTrailerSummaryStructureOldGrossWeight oldGrossWeightField;

            private bool isSteerableAtRearField;

            private bool isSteerableAtRearFieldSpecified;

            private SemiTrailerSummaryStructureOldIsSteerableAtRear oldIsSteerableAtRearField;

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            private SemiTrailerSummaryStructureOldMaxAxleWeight oldMaxAxleWeightField;

            private SummaryAxleStructure axleConfigurationField;

            private SemiTrailerSummaryStructureOldAxleConfiguration oldAxleConfigurationField;

            private string rigidLengthField;

            private SemiTrailerSummaryStructureOldRigidLength oldRigidLengthField;

            private string widthField;

            private SemiTrailerSummaryStructureOldWidth oldWidthField;

            private SummaryHeightStructure heightField;

            private SemiTrailerSummaryStructureOldHeight oldHeightField;

            private string wheelbaseField;

            private bool wheelbaseFieldSpecified;

            private SemiTrailerSummaryStructureOldWheelbase oldWheelbaseField;

            private string leftOverhangField;

            private bool leftOverhangFieldSpecified;

            private SemiTrailerSummaryStructureOldLeftOverhang oldLeftOverhangField;

            private string rightOverhangField;

            private bool rightOverhangFieldSpecified;

            private SemiTrailerSummaryStructureOldRightOverhang oldRightOverhangField;

            private string frontOverhangField;

            private bool frontOverhangFieldSpecified;

            private SemiTrailerSummaryStructureOldFrontOverhang oldFrontOverhangField;

            private string rearOverhangField;

            private SemiTrailerSummaryStructureOldRearOverhang oldRearOverhangField;

            private string groundClearanceField;

            private bool groundClearanceFieldSpecified;

            private SemiTrailerSummaryStructureOldGroundClearance oldGroundClearanceField;

            private string reducedGroundClearanceField;

            private bool reducedGroundClearanceFieldSpecified;

            private SemiTrailerSummaryStructureOldReducedGroundClearance oldReducedGroundClearanceField;

            private string outsideTrackField;

            private bool outsideTrackFieldSpecified;

            private SemiTrailerSummaryStructureOldOutsideTrack oldOutsideTrackField;

            private VehicleComponentSubType tractorSubTypeField;

            private VehicleComponentSubType trailerSubTypeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldSummary OldSummary
            {
                get
                {
                    return this.oldSummaryField;
                }
                set
                {
                    this.oldSummaryField = value;
                }
            }

            /// <remarks/>
            public SummaryWeightStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldGrossWeight OldGrossWeight
            {
                get
                {
                    return this.oldGrossWeightField;
                }
                set
                {
                    this.oldGrossWeightField = value;
                }
            }

            /// <remarks/>
            public bool IsSteerableAtRear
            {
                get
                {
                    return this.isSteerableAtRearField;
                }
                set
                {
                    this.isSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IsSteerableAtRearSpecified
            {
                get
                {
                    return this.isSteerableAtRearFieldSpecified;
                }
                set
                {
                    this.isSteerableAtRearFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldIsSteerableAtRear OldIsSteerableAtRear
            {
                get
                {
                    return this.oldIsSteerableAtRearField;
                }
                set
                {
                    this.oldIsSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldMaxAxleWeight OldMaxAxleWeight
            {
                get
                {
                    return this.oldMaxAxleWeightField;
                }
                set
                {
                    this.oldMaxAxleWeightField = value;
                }
            }

            /// <remarks/>
            public SummaryAxleStructure AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldAxleConfiguration OldAxleConfiguration
            {
                get
                {
                    return this.oldAxleConfigurationField;
                }
                set
                {
                    this.oldAxleConfigurationField = value;
                }
            }

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldRigidLength OldRigidLength
            {
                get
                {
                    return this.oldRigidLengthField;
                }
                set
                {
                    this.oldRigidLengthField = value;
                }
            }

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldWidth OldWidth
            {
                get
                {
                    return this.oldWidthField;
                }
                set
                {
                    this.oldWidthField = value;
                }
            }

            /// <remarks/>
            public SummaryHeightStructure Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldHeight OldHeight
            {
                get
                {
                    return this.oldHeightField;
                }
                set
                {
                    this.oldHeightField = value;
                }
            }

            /// <remarks/>
            public string Wheelbase
            {
                get
                {
                    return this.wheelbaseField;
                }
                set
                {
                    this.wheelbaseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool WheelbaseSpecified
            {
                get
                {
                    return this.wheelbaseFieldSpecified;
                }
                set
                {
                    this.wheelbaseFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldWheelbase OldWheelbase
            {
                get
                {
                    return this.oldWheelbaseField;
                }
                set
                {
                    this.oldWheelbaseField = value;
                }
            }

            /// <remarks/>
            public string LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LeftOverhangSpecified
            {
                get
                {
                    return this.leftOverhangFieldSpecified;
                }
                set
                {
                    this.leftOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldLeftOverhang OldLeftOverhang
            {
                get
                {
                    return this.oldLeftOverhangField;
                }
                set
                {
                    this.oldLeftOverhangField = value;
                }
            }

            /// <remarks/>
            public string RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RightOverhangSpecified
            {
                get
                {
                    return this.rightOverhangFieldSpecified;
                }
                set
                {
                    this.rightOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldRightOverhang OldRightOverhang
            {
                get
                {
                    return this.oldRightOverhangField;
                }
                set
                {
                    this.oldRightOverhangField = value;
                }
            }

            /// <remarks/>
            public string FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool FrontOverhangSpecified
            {
                get
                {
                    return this.frontOverhangFieldSpecified;
                }
                set
                {
                    this.frontOverhangFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldFrontOverhang OldFrontOverhang
            {
                get
                {
                    return this.oldFrontOverhangField;
                }
                set
                {
                    this.oldFrontOverhangField = value;
                }
            }

            /// <remarks/>
            public string RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldRearOverhang OldRearOverhang
            {
                get
                {
                    return this.oldRearOverhangField;
                }
                set
                {
                    this.oldRearOverhangField = value;
                }
            }

            /// <remarks/>
            public string GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GroundClearanceSpecified
            {
                get
                {
                    return this.groundClearanceFieldSpecified;
                }
                set
                {
                    this.groundClearanceFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldGroundClearance OldGroundClearance
            {
                get
                {
                    return this.oldGroundClearanceField;
                }
                set
                {
                    this.oldGroundClearanceField = value;
                }
            }

            /// <remarks/>
            public string ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReducedGroundClearanceSpecified
            {
                get
                {
                    return this.reducedGroundClearanceFieldSpecified;
                }
                set
                {
                    this.reducedGroundClearanceFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldReducedGroundClearance OldReducedGroundClearance
            {
                get
                {
                    return this.oldReducedGroundClearanceField;
                }
                set
                {
                    this.oldReducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            public string OutsideTrack
            {
                get
                {
                    return this.outsideTrackField;
                }
                set
                {
                    this.outsideTrackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OutsideTrackSpecified
            {
                get
                {
                    return this.outsideTrackFieldSpecified;
                }
                set
                {
                    this.outsideTrackFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SemiTrailerSummaryStructureOldOutsideTrack OldOutsideTrack
            {
                get
                {
                    return this.oldOutsideTrackField;
                }
                set
                {
                    this.oldOutsideTrackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public VehicleComponentSubType TractorSubType
            {
                get
                {
                    return this.tractorSubTypeField;
                }
                set
                {
                    this.tractorSubTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public VehicleComponentSubType TrailerSubType
            {
                get
                {
                    return this.trailerSubTypeField;
                }
                set
                {
                    this.trailerSubTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldSummary
        {

            private string summaryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldGrossWeight
        {

            private SummaryWeightStructure grossWeightField;

            /// <remarks/>
            public SummaryWeightStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldIsSteerableAtRear
        {

            private bool isSteerableAtRearField;

            private bool isSteerableAtRearFieldSpecified;

            /// <remarks/>
            public bool IsSteerableAtRear
            {
                get
                {
                    return this.isSteerableAtRearField;
                }
                set
                {
                    this.isSteerableAtRearField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IsSteerableAtRearSpecified
            {
                get
                {
                    return this.isSteerableAtRearFieldSpecified;
                }
                set
                {
                    this.isSteerableAtRearFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldMaxAxleWeight
        {

            private SummaryMaxAxleWeightStructure maxAxleWeightField;

            /// <remarks/>
            public SummaryMaxAxleWeightStructure MaxAxleWeight
            {
                get
                {
                    return this.maxAxleWeightField;
                }
                set
                {
                    this.maxAxleWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldAxleConfiguration
        {

            private SummaryAxleStructure axleConfigurationField;

            /// <remarks/>
            public SummaryAxleStructure AxleConfiguration
            {
                get
                {
                    return this.axleConfigurationField;
                }
                set
                {
                    this.axleConfigurationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldRigidLength
        {

            private string rigidLengthField;

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldWidth
        {

            private string widthField;

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldHeight
        {

            private SummaryHeightStructure heightField;

            /// <remarks/>
            public SummaryHeightStructure Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldWheelbase
        {

            private string wheelbaseField;

            private bool wheelbaseFieldSpecified;

            /// <remarks/>
            public string Wheelbase
            {
                get
                {
                    return this.wheelbaseField;
                }
                set
                {
                    this.wheelbaseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool WheelbaseSpecified
            {
                get
                {
                    return this.wheelbaseFieldSpecified;
                }
                set
                {
                    this.wheelbaseFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldLeftOverhang
        {

            private string leftOverhangField;

            private bool leftOverhangFieldSpecified;

            /// <remarks/>
            public string LeftOverhang
            {
                get
                {
                    return this.leftOverhangField;
                }
                set
                {
                    this.leftOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LeftOverhangSpecified
            {
                get
                {
                    return this.leftOverhangFieldSpecified;
                }
                set
                {
                    this.leftOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldRightOverhang
        {

            private string rightOverhangField;

            private bool rightOverhangFieldSpecified;

            /// <remarks/>
            public string RightOverhang
            {
                get
                {
                    return this.rightOverhangField;
                }
                set
                {
                    this.rightOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RightOverhangSpecified
            {
                get
                {
                    return this.rightOverhangFieldSpecified;
                }
                set
                {
                    this.rightOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldFrontOverhang
        {

            private string frontOverhangField;

            private bool frontOverhangFieldSpecified;

            /// <remarks/>
            public string FrontOverhang
            {
                get
                {
                    return this.frontOverhangField;
                }
                set
                {
                    this.frontOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool FrontOverhangSpecified
            {
                get
                {
                    return this.frontOverhangFieldSpecified;
                }
                set
                {
                    this.frontOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldRearOverhang
        {

            private string rearOverhangField;

            private bool rearOverhangFieldSpecified;

            /// <remarks/>
            public string RearOverhang
            {
                get
                {
                    return this.rearOverhangField;
                }
                set
                {
                    this.rearOverhangField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RearOverhangSpecified
            {
                get
                {
                    return this.rearOverhangFieldSpecified;
                }
                set
                {
                    this.rearOverhangFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldGroundClearance
        {

            private string groundClearanceField;

            private bool groundClearanceFieldSpecified;

            /// <remarks/>
            public string GroundClearance
            {
                get
                {
                    return this.groundClearanceField;
                }
                set
                {
                    this.groundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GroundClearanceSpecified
            {
                get
                {
                    return this.groundClearanceFieldSpecified;
                }
                set
                {
                    this.groundClearanceFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldReducedGroundClearance
        {

            private string reducedGroundClearanceField;

            private bool reducedGroundClearanceFieldSpecified;

            /// <remarks/>
            public string ReducedGroundClearance
            {
                get
                {
                    return this.reducedGroundClearanceField;
                }
                set
                {
                    this.reducedGroundClearanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReducedGroundClearanceSpecified
            {
                get
                {
                    return this.reducedGroundClearanceFieldSpecified;
                }
                set
                {
                    this.reducedGroundClearanceFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class SemiTrailerSummaryStructureOldOutsideTrack
        {

            private string outsideTrackField;

            private bool outsideTrackFieldSpecified;

            /// <remarks/>
            public string OutsideTrack
            {
                get
                {
                    return this.outsideTrackField;
                }
                set
                {
                    this.outsideTrackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OutsideTrackSpecified
            {
                get
                {
                    return this.outsideTrackFieldSpecified;
                }
                set
                {
                    this.outsideTrackFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class TrackedVehicleSummaryStructure
        {

            private string summaryField;

            private TrackedVehicleSummaryStructureOldSummary oldSummaryField;

            private SummaryWeightStructure grossWeightField;

            private TrackedVehicleSummaryStructureOldGrossWeight oldGrossWeightField;

            private string rigidLengthField;

            private TrackedVehicleSummaryStructureOldRigidLength oldRigidLengthField;

            private string widthField;

            private TrackedVehicleSummaryStructureOldWidth oldWidthField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }

            /// <remarks/>
            public TrackedVehicleSummaryStructureOldSummary OldSummary
            {
                get
                {
                    return this.oldSummaryField;
                }
                set
                {
                    this.oldSummaryField = value;
                }
            }

            /// <remarks/>
            public SummaryWeightStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }

            /// <remarks/>
            public TrackedVehicleSummaryStructureOldGrossWeight OldGrossWeight
            {
                get
                {
                    return this.oldGrossWeightField;
                }
                set
                {
                    this.oldGrossWeightField = value;
                }
            }

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }

            /// <remarks/>
            public TrackedVehicleSummaryStructureOldRigidLength OldRigidLength
            {
                get
                {
                    return this.oldRigidLengthField;
                }
                set
                {
                    this.oldRigidLengthField = value;
                }
            }

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public TrackedVehicleSummaryStructureOldWidth OldWidth
            {
                get
                {
                    return this.oldWidthField;
                }
                set
                {
                    this.oldWidthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class TrackedVehicleSummaryStructureOldSummary
        {

            private string summaryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Summary
            {
                get
                {
                    return this.summaryField;
                }
                set
                {
                    this.summaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class TrackedVehicleSummaryStructureOldGrossWeight
        {

            private SummaryWeightStructure grossWeightField;

            /// <remarks/>
            public SummaryWeightStructure GrossWeight
            {
                get
                {
                    return this.grossWeightField;
                }
                set
                {
                    this.grossWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class TrackedVehicleSummaryStructureOldRigidLength
        {

            private string rigidLengthField;

            /// <remarks/>
            public string RigidLength
            {
                get
                {
                    return this.rigidLengthField;
                }
                set
                {
                    this.rigidLengthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class TrackedVehicleSummaryStructureOldWidth
        {

            private string widthField;

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryStructureOldConfiguration
        {

            private VehicleSummaryTypeChoiceStructure configurationField;

            /// <remarks/>
            public VehicleSummaryTypeChoiceStructure Configuration
            {
                get
                {
                    return this.configurationField;
                }
                set
                {
                    this.configurationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehicleSummaryStructureOldConfigurationType
        {

            private VehicleConfigurationType configurationTypeField;

            /// <remarks/>
            public VehicleConfigurationType ConfigurationType
            {
                get
                {
                    return this.configurationTypeField;
                }
                set
                {
                    this.configurationTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/vehicle")]
        public partial class VehiclesSummaryStructureVehicleSummaryListPositionOldVehicleSummary
        {

            private VehicleSummaryStructure vehicleSummaryField;

            /// <remarks/>
            public VehicleSummaryStructure VehicleSummary
            {
                get
                {
                    return this.vehicleSummaryField;
                }
                set
                {
                    this.vehicleSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        [System.Xml.Serialization.XmlRootAttribute("AnalysedRoadsRoute", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
        public partial class AnalysedRoadsRouteStructure
        {

            private AnalysedRoadsRouteStructureAnalysedRoadsPart[] analysedRoadsPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedRoadsPart")]
            public AnalysedRoadsRouteStructureAnalysedRoadsPart[] AnalysedRoadsPart
            {
                get
                {
                    return this.analysedRoadsPartField;
                }
                set
                {
                    this.analysedRoadsPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadsRouteStructureAnalysedRoadsPart : AnalysedRoadsPartStructure
        {

            private ModeOfTransportType modeOfTransportField;

            private string idField;

            private string comparisonIdField;

            private bool isBrokenField;

            public AnalysedRoadsRouteStructureAnalysedRoadsPart()
            {
                this.modeOfTransportField = ModeOfTransportType.road;
                this.isBrokenField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ModeOfTransportType.road)]
            public ModeOfTransportType ModeOfTransport
            {
                get
                {
                    return this.modeOfTransportField;
                }
                set
                {
                    this.modeOfTransportField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComparisonId
            {
                get
                {
                    return this.comparisonIdField;
                }
                set
                {
                    this.comparisonIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadsPartStructure
        {

            private string nameField;

            private AnalysedRoadsSubPartChoiceStructure[] subPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SubPart")]
            public AnalysedRoadsSubPartChoiceStructure[] SubPart
            {
                get
                {
                    return this.subPartField;
                }
                set
                {
                    this.subPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadsSubPartChoiceStructure
        {

            private object itemField;

            private ItemChoiceType6 itemElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Broken", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Discontinuity", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Roads", typeof(AnalysedRoadsSubPartStructure))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemChoiceType6 ItemElementName
            {
                get
                {
                    return this.itemElementNameField;
                }
                set
                {
                    this.itemElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadsSubPartStructure
        {

            private AnalysedRoadsChoiceStructure[][] pathField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoadsPathSegment", typeof(AnalysedRoadsChoiceStructure), IsNullable = false)]
            public AnalysedRoadsChoiceStructure[][] Path
            {
                get
                {
                    return this.pathField;
                }
                set
                {
                    this.pathField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadsChoiceStructure
        {

            private object itemField;

            private ItemChoiceType5 itemElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Assumed", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Broken", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Discontinuity", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("OffRoad", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Road", typeof(AnalysedRoadStructure))]
            [System.Xml.Serialization.XmlElementAttribute("RoutePoint", typeof(RoutePointStructure1))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemChoiceType5 ItemElementName
            {
                get
                {
                    return this.itemElementNameField;
                }
                set
                {
                    this.itemElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadStructure
        {

            private RoadIdentificationStructure roadIdentityField;

            private DistanceStructure distanceField;

            private RoadResponsiblePartyStructure[] roadResponsibilityField;

            private AnalysedRoadsConstraintStructure[] constraintsField;

            /// <remarks/>
            public RoadIdentificationStructure RoadIdentity
            {
                get
                {
                    return this.roadIdentityField;
                }
                set
                {
                    this.roadIdentityField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoadResponsibleParty", IsNullable = false)]
            public RoadResponsiblePartyStructure[] RoadResponsibility
            {
                get
                {
                    return this.roadResponsibilityField;
                }
                set
                {
                    this.roadResponsibilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Constraint", IsNullable = false)]
            public AnalysedRoadsConstraintStructure[] Constraints
            {
                get
                {
                    return this.constraintsField;
                }
                set
                {
                    this.constraintsField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(AffectedRoadConstraintStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedRoadsConstraintStructure
        {

            private string eCRNField;

            private ConstraintType typeField;

            private ConstraintSuitabilityStructure appraisalField;

            private string nameField;

            private MetricImperialDistancePairStructure encounteredAtField;

            private DistanceStructure lastingForField;

            private string ownerIdField;

            private bool ownerIdFieldSpecified;

            /// <remarks/>
            public string ECRN
            {
                get
                {
                    return this.eCRNField;
                }
                set
                {
                    this.eCRNField = value;
                }
            }

            /// <remarks/>
            public ConstraintType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            public ConstraintSuitabilityStructure Appraisal
            {
                get
                {
                    return this.appraisalField;
                }
                set
                {
                    this.appraisalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public MetricImperialDistancePairStructure EncounteredAt
            {
                get
                {
                    return this.encounteredAtField;
                }
                set
                {
                    this.encounteredAtField = value;
                }
            }

            /// <remarks/>
            public DistanceStructure LastingFor
            {
                get
                {
                    return this.lastingForField;
                }
                set
                {
                    this.lastingForField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OwnerId
            {
                get
                {
                    return this.ownerIdField;
                }
                set
                {
                    this.ownerIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OwnerIdSpecified
            {
                get
                {
                    return this.ownerIdFieldSpecified;
                }
                set
                {
                    this.ownerIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadConstraintStructure : AnalysedRoadsConstraintStructure
        {

            private bool isMyResponsibilityField;

            public AffectedRoadConstraintStructure()
            {
                this.isMyResponsibilityField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsMyResponsibility
            {
                get
                {
                    return this.isMyResponsibilityField;
                }
                set
                {
                    this.isMyResponsibilityField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(TypeName = "RoutePointStructure", Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class RoutePointStructure1
        {

            private RoutePointStructure pointField;

            private RoutePointType pointTypeField;

            private bool isMTPField;

            private bool isBrokenField;

            public RoutePointStructure1()
            {
                this.isMTPField = false;
                this.isBrokenField = false;
            }

            /// <remarks/>
            public RoutePointStructure Point
            {
                get
                {
                    return this.pointField;
                }
                set
                {
                    this.pointField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public RoutePointType PointType
            {
                get
                {
                    return this.pointTypeField;
                }
                set
                {
                    this.pointTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsMTP
            {
                get
                {
                    return this.isMTPField;
                }
                set
                {
                    this.isMTPField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IncludeInSchema = false)]
        public enum ItemChoiceType5
        {

            /// <remarks/>
            Assumed,

            /// <remarks/>
            Broken,

            /// <remarks/>
            Discontinuity,

            /// <remarks/>
            OffRoad,

            /// <remarks/>
            Road,

            /// <remarks/>
            RoutePoint,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IncludeInSchema = false)]
        public enum ItemChoiceType6
        {

            /// <remarks/>
            Broken,

            /// <remarks/>
            Discontinuity,

            /// <remarks/>
            Roads,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        [System.Xml.Serialization.XmlRootAttribute("RouteContacts", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
        public partial class RouteContactsStructure
        {

            private RouteContactStructure[] contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Contact")]
            public RouteContactStructure[] Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class RouteContactStructure
        {

            private ResolvedContactStructure contactField;

            private string onBehalfOfField;

            private ContactReasonStructure[] reasonsField;

            /// <remarks/>
            public ResolvedContactStructure Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OnBehalfOf
            {
                get
                {
                    return this.onBehalfOfField;
                }
                set
                {
                    this.onBehalfOfField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Reason", IsNullable = false)]
            public ContactReasonStructure[] Reasons
            {
                get
                {
                    return this.reasonsField;
                }
                set
                {
                    this.reasonsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class ContactReasonStructure
        {

            private object itemField;

            private ItemChoiceType7 itemElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation", typeof(AnnotationContactReasonStructure))]
            [System.Xml.Serialization.XmlElementAttribute("BoundaryBasedStructureResponsibility", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("BoundaryCrossed", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("Caution", typeof(CautionContactReasonStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Constraint", typeof(ConstraintContactReasonStructure))]
            [System.Xml.Serialization.XmlElementAttribute("ConstraintAnnotation", typeof(ConstraintAnnotationContactReasonStructure))]
            [System.Xml.Serialization.XmlElementAttribute("RoadUsage", typeof(object))]
            [System.Xml.Serialization.XmlElementAttribute("StructureAnnotation", typeof(StructureAnnotationContactReasonStructure))]
            [System.Xml.Serialization.XmlElementAttribute("StructureResponsibility", typeof(AffectedStructureStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Waypoint", typeof(WaypointContactReasonStructure))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemChoiceType7 ItemElementName
            {
                get
                {
                    return this.itemElementNameField;
                }
                set
                {
                    this.itemElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AffectedStructureStructure
        {

            private string nameField;

            private string eSRNField;

            private StructureTraversalType traversalTypeField;

            public AffectedStructureStructure()
            {
                this.traversalTypeField = StructureTraversalType.underbridge;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(StructureTraversalType.underbridge)]
            public StructureTraversalType TraversalType
            {
                get
                {
                    return this.traversalTypeField;
                }
                set
                {
                    this.traversalTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class WaypointContactReasonStructure
        {

            private string descriptionField;

            private SimpleTextStructure textField;

            private RoadIdentificationStructure roadField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public SimpleTextStructure Text
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

            /// <remarks/>
            public RoadIdentificationStructure Road
            {
                get
                {
                    return this.roadField;
                }
                set
                {
                    this.roadField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IncludeInSchema = false)]
        public enum ItemChoiceType7
        {

            /// <remarks/>
            Annotation,

            /// <remarks/>
            BoundaryBasedStructureResponsibility,

            /// <remarks/>
            BoundaryCrossed,

            /// <remarks/>
            Caution,

            /// <remarks/>
            Constraint,

            /// <remarks/>
            ConstraintAnnotation,

            /// <remarks/>
            RoadUsage,

            /// <remarks/>
            StructureAnnotation,

            /// <remarks/>
            StructureResponsibility,

            /// <remarks/>
            Waypoint,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        [System.Xml.Serialization.XmlRootAttribute("AnalysedAnnotations", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
        public partial class AnalysedAnnotationsStructure
        {

            private AnalysedAnnotationsStructureAnalysedAnnotationsPart[] analysedAnnotationsPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedAnnotationsPart")]
            public AnalysedAnnotationsStructureAnalysedAnnotationsPart[] AnalysedAnnotationsPart
            {
                get
                {
                    return this.analysedAnnotationsPartField;
                }
                set
                {
                    this.analysedAnnotationsPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedAnnotationsStructureAnalysedAnnotationsPart : AnalysedAnnotationsPartStructure
        {

            private ModeOfTransportType modeOfTransportField;

            private string idField;

            public AnalysedAnnotationsStructureAnalysedAnnotationsPart()
            {
                this.modeOfTransportField = ModeOfTransportType.road;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ModeOfTransportType.road)]
            public ModeOfTransportType ModeOfTransport
            {
                get
                {
                    return this.modeOfTransportField;
                }
                set
                {
                    this.modeOfTransportField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedAnnotationsPartStructure
        {

            private string nameField;

            private AnalysedAnnotationStructure[] annotationField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Annotation")]
            public AnalysedAnnotationStructure[] Annotation
            {
                get
                {
                    return this.annotationField;
                }
                set
                {
                    this.annotationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        [System.Xml.Serialization.XmlRootAttribute("AnalysedCautions", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
        public partial class AnalysedCautionsStructure
        {

            private AnalysedCautionsStructureAnalysedCautionsPart[] analysedCautionsPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedCautionsPart")]
            public AnalysedCautionsStructureAnalysedCautionsPart[] AnalysedCautionsPart
            {
                get
                {
                    return this.analysedCautionsPartField;
                }
                set
                {
                    this.analysedCautionsPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedCautionsStructureAnalysedCautionsPart : AnalysedCautionsPartStructure
        {

            private ModeOfTransportType modeOfTransportField;

            private string idField;

            private string comparisonIdField;

            public AnalysedCautionsStructureAnalysedCautionsPart()
            {
                this.modeOfTransportField = ModeOfTransportType.road;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ModeOfTransportType.road)]
            public ModeOfTransportType ModeOfTransport
            {
                get
                {
                    return this.modeOfTransportField;
                }
                set
                {
                    this.modeOfTransportField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ComparisonId
            {
                get
                {
                    return this.comparisonIdField;
                }
                set
                {
                    this.comparisonIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedCautionsPartStructure
        {

            private string nameField;

            private AnalysedCautionStructure[] cautionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Caution")]
            public AnalysedCautionStructure[] Caution
            {
                get
                {
                    return this.cautionField;
                }
                set
                {
                    this.cautionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        [System.Xml.Serialization.XmlRootAttribute("AnalysedConstraints", Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
        public partial class AnalysedConstraintsStructure
        {

            private AnalysedConstraintsStructureAnalysedConstraintsPart[] analysedConstraintsPartField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AnalysedConstraintsPart")]
            public AnalysedConstraintsStructureAnalysedConstraintsPart[] AnalysedConstraintsPart
            {
                get
                {
                    return this.analysedConstraintsPartField;
                }
                set
                {
                    this.analysedConstraintsPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedConstraintsStructureAnalysedConstraintsPart : AnalysedConstraintsPartStructure
        {

            private ModeOfTransportType modeOfTransportField;

            private string idField;

            public AnalysedConstraintsStructureAnalysedConstraintsPart()
            {
                this.modeOfTransportField = ModeOfTransportType.road;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(ModeOfTransportType.road)]
            public ModeOfTransportType ModeOfTransport
            {
                get
                {
                    return this.modeOfTransportField;
                }
                set
                {
                    this.modeOfTransportField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
        public partial class AnalysedConstraintsPartStructure
        {

            private string nameField;

            private VehicleSpecificConstraintStructure[] constraintField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Constraint")]
            public VehicleSpecificConstraintStructure[] Constraint
            {
                get
                {
                    return this.constraintField;
                }
                set
                {
                    this.constraintField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        [System.Xml.Serialization.XmlRootAttribute("Contact", Namespace = "http://www.esdal.com/schemas/core/contact", IsNullable = false)]
        public partial class ContactStructure
        {

            private ContactStructureName nameField;

            private string organisationNameField;

            private string[] representedOrganisationsField;

            private AddressStructure addressField;

            private string telephoneNumberField;

            private string telephoneExtensionField;

            private string mobileNumberField;

            private string faxNumberField;

            private string emailAddressField;

            private AdditionalContactPhoneNumberDetailsStructure[] additionalTelephoneNumbersField;

            private string notesField;

            private string contactIdField;

            private string organisationIdField;

            /// <remarks/>
            public ContactStructureName Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RepresentedOrganisationName", DataType = "token", IsNullable = false)]
            public string[] RepresentedOrganisations
            {
                get
                {
                    return this.representedOrganisationsField;
                }
                set
                {
                    this.representedOrganisationsField = value;
                }
            }

            /// <remarks/>
            public AddressStructure Address
            {
                get
                {
                    return this.addressField;
                }
                set
                {
                    this.addressField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            public string TelephoneExtension
            {
                get
                {
                    return this.telephoneExtensionField;
                }
                set
                {
                    this.telephoneExtensionField = value;
                }
            }

            /// <remarks/>
            public string MobileNumber
            {
                get
                {
                    return this.mobileNumberField;
                }
                set
                {
                    this.mobileNumberField = value;
                }
            }

            /// <remarks/>
            public string FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("TelephoneNumberDetails", IsNullable = false)]
            public AdditionalContactPhoneNumberDetailsStructure[] AdditionalTelephoneNumbers
            {
                get
                {
                    return this.additionalTelephoneNumbersField;
                }
                set
                {
                    this.additionalTelephoneNumbersField = value;
                }
            }

            /// <remarks/>
            public string Notes
            {
                get
                {
                    return this.notesField;
                }
                set
                {
                    this.notesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactId
            {
                get
                {
                    return this.contactIdField;
                }
                set
                {
                    this.contactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class ContactStructureName
        {

            private string titleField;

            private string forenameField;

            private string surnameField;

            /// <remarks/>
            public string Title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            public string Forename
            {
                get
                {
                    return this.forenameField;
                }
                set
                {
                    this.forenameField = value;
                }
            }

            /// <remarks/>
            public string Surname
            {
                get
                {
                    return this.surnameField;
                }
                set
                {
                    this.surnameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/contact")]
        public partial class AdditionalContactPhoneNumberDetailsStructure
        {

            private string nameField;

            private string telephoneNumberField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute("ESDALReferenceNumber", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class ESDALReferenceNumberStructure
        {

            private string mnemonicField;

            private string movementProjectNumberField;

            private bool enteredBySORTField;

            private bool enteredBySORTFieldSpecified;

            private ApplicationRevisionNumberStructure applicationRevisionField;

            private MovementVersionNumberStructure movementVersionField;

            private string notificationNumberField;

            private bool notificationNumberFieldSpecified;

            private string notificationVersionField;

            private bool notificationVersionFieldSpecified;

            public ESDALReferenceNumberStructure()
            {
                this.enteredBySORTField = true;
            }

            /// <remarks/>
            public string Mnemonic
            {
                get
                {
                    return this.mnemonicField;
                }
                set
                {
                    this.mnemonicField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string MovementProjectNumber
            {
                get
                {
                    return this.movementProjectNumberField;
                }
                set
                {
                    this.movementProjectNumberField = value;
                }
            }

            /// <remarks/>
            public bool EnteredBySORT
            {
                get
                {
                    return this.enteredBySORTField;
                }
                set
                {
                    this.enteredBySORTField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool EnteredBySORTSpecified
            {
                get
                {
                    return this.enteredBySORTFieldSpecified;
                }
                set
                {
                    this.enteredBySORTFieldSpecified = value;
                }
            }

            /// <remarks/>
            public ApplicationRevisionNumberStructure ApplicationRevision
            {
                get
                {
                    return this.applicationRevisionField;
                }
                set
                {
                    this.applicationRevisionField = value;
                }
            }

            /// <remarks/>
            public MovementVersionNumberStructure MovementVersion
            {
                get
                {
                    return this.movementVersionField;
                }
                set
                {
                    this.movementVersionField = value;
                }
            }

            /// <remarks/>
            public string NotificationNumber
            {
                get
                {
                    return this.notificationNumberField;
                }
                set
                {
                    this.notificationNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool NotificationNumberSpecified
            {
                get
                {
                    return this.notificationNumberFieldSpecified;
                }
                set
                {
                    this.notificationNumberFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string NotificationVersion
            {
                get
                {
                    return this.notificationVersionField;
                }
                set
                {
                    this.notificationVersionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool NotificationVersionSpecified
            {
                get
                {
                    return this.notificationVersionFieldSpecified;
                }
                set
                {
                    this.notificationVersionFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class ApplicationRevisionNumberStructure
        {

            private bool createdBySortField;

            private string valueField;

            public ApplicationRevisionNumberStructure()
            {
                this.createdBySortField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool CreatedBySort
            {
                get
                {
                    return this.createdBySortField;
                }
                set
                {
                    this.createdBySortField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class MovementVersionNumberStructure
        {

            private bool createdBySortField;

            private string valueField;

            public MovementVersionNumberStructure()
            {
                this.createdBySortField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool CreatedBySort
            {
                get
                {
                    return this.createdBySortField;
                }
                set
                {
                    this.createdBySortField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute("RecipientsList", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class RecipientContactsStructure
        {

            private RecipientContactsStructureContactListPosition[] contactListPositionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ContactListPosition")]
            public RecipientContactsStructureContactListPosition[] ContactListPosition
            {
                get
                {
                    return this.contactListPositionField;
                }
                set
                {
                    this.contactListPositionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RecipientContactsStructureContactListPosition
        {

            private RecipientContactStructure contactField;

            private RecipientContactsStructureContactListPositionOldContact oldContactField;

            /// <remarks/>
            public RecipientContactStructure Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            public RecipientContactsStructureContactListPositionOldContact OldContact
            {
                get
                {
                    return this.oldContactField;
                }
                set
                {
                    this.oldContactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RecipientContactStructure : BasicRecipientContactStructure
        {

            private string faxField;

            private string emailField;

            private OnBehalfOfStructure onbehalfOfField;

            private bool isRecipientField;

            private bool isPoliceField;

            private bool isHaulierField;

            private bool isRetainedNotificationOnlyField;

            private string reasonField;

            //public RecipientContactStructure()
            //{
            //    this.isRecipientField = false;
            //    this.isPoliceField = false;
            //    this.isHaulierField = false;
            //    this.isRetainedNotificationOnlyField = false;
            //    this.reasonField = string.Empty;
            //}

            /// <remarks/>
            public string Fax
            {
                get
                {
                    return this.faxField;
                }
                set
                {
                    this.faxField = value;
                }
            }

            /// <remarks/>
            public string Email
            {
                get
                {
                    return this.emailField;
                }
                set
                {
                    this.emailField = value;
                }
            }

            /// <remarks/>
            public OnBehalfOfStructure OnbehalfOf
            {
                get
                {
                    return this.onbehalfOfField;
                }
                set
                {
                    this.onbehalfOfField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Reason
            {
                get
                {
                    return this.reasonField;
                }
                set
                {
                    this.reasonField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsRecipient
            {
                get
                {
                    return this.isRecipientField;
                }
                set
                {
                    this.isRecipientField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsPolice
            {
                get
                {
                    return this.isPoliceField;
                }
                set
                {
                    this.isPoliceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsHaulier
            {
                get
                {
                    return this.isHaulierField;
                }
                set
                {
                    this.isHaulierField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsRetainedNotificationOnly
            {
                get
                {
                    return this.isRetainedNotificationOnlyField;
                }
                set
                {
                    this.isRetainedNotificationOnlyField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class OnBehalfOfStructure
        {

            private string delegatorsOrganisationNameField;

            private OnBehalfOfStructure onBehalfOfField;

            private string delegatorsOrganisationIdField;

            private bool delegatorsOrganisationIdFieldSpecified;

            private string delegatorsContactIdField;

            private bool delegatorsContactIdFieldSpecified;

            private bool retainNotificationField;

            private bool wantsFailureAlertField;

            private string delegationIdField;

            private bool delegationIdFieldSpecified;

            public OnBehalfOfStructure()
            {
                this.retainNotificationField = false;
                this.wantsFailureAlertField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string DelegatorsOrganisationName
            {
                get
                {
                    return this.delegatorsOrganisationNameField;
                }
                set
                {
                    this.delegatorsOrganisationNameField = value;
                }
            }

            /// <remarks/>
            public OnBehalfOfStructure OnBehalfOf
            {
                get
                {
                    return this.onBehalfOfField;
                }
                set
                {
                    this.onBehalfOfField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegatorsOrganisationId
            {
                get
                {
                    return this.delegatorsOrganisationIdField;
                }
                set
                {
                    this.delegatorsOrganisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegatorsOrganisationIdSpecified
            {
                get
                {
                    return this.delegatorsOrganisationIdFieldSpecified;
                }
                set
                {
                    this.delegatorsOrganisationIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegatorsContactId
            {
                get
                {
                    return this.delegatorsContactIdField;
                }
                set
                {
                    this.delegatorsContactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegatorsContactIdSpecified
            {
                get
                {
                    return this.delegatorsContactIdFieldSpecified;
                }
                set
                {
                    this.delegatorsContactIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool RetainNotification
            {
                get
                {
                    return this.retainNotificationField;
                }
                set
                {
                    this.retainNotificationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool WantsFailureAlert
            {
                get
                {
                    return this.wantsFailureAlertField;
                }
                set
                {
                    this.wantsFailureAlertField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegationId
            {
                get
                {
                    return this.delegationIdField;
                }
                set
                {
                    this.delegationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegationIdSpecified
            {
                get
                {
                    return this.delegationIdFieldSpecified;
                }
                set
                {
                    this.delegationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(RecipientContactStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class BasicRecipientContactStructure
        {

            private string contactNameField;

            private string organisationNameField;

            private string contactIdField;

            private bool contactIdFieldSpecified;

            private string organisationIdField;

            private bool organisationIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string ContactName
            {
                get
                {
                    return this.contactNameField;
                }
                set
                {
                    this.contactNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactId
            {
                get
                {
                    return this.contactIdField;
                }
                set
                {
                    this.contactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ContactIdSpecified
            {
                get
                {
                    return this.contactIdFieldSpecified;
                }
                set
                {
                    this.contactIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIdSpecified
            {
                get
                {
                    return this.organisationIdFieldSpecified;
                }
                set
                {
                    this.organisationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RecipientContactsStructureContactListPositionOldContact
        {

            private RecipientContactStructure contactField;

            /// <remarks/>
            public RecipientContactStructure Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute("ResolvedAffectedParties", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class ResolvedAffectedPartiesStructure
        {

            private ResolvedAffectedPartyStructure[] generatedAffectedPartiesField;

            private ResolvedAffectedPartyStructure[] manualAffectedPartiesField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ResolvedAffectedParty", IsNullable = false)]
            public ResolvedAffectedPartyStructure[] GeneratedAffectedParties
            {
                get
                {
                    return this.generatedAffectedPartiesField;
                }
                set
                {
                    this.generatedAffectedPartiesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ResolvedAffectedParty", IsNullable = false)]
            public ResolvedAffectedPartyStructure[] ManualAffectedParties
            {
                get
                {
                    return this.manualAffectedPartiesField;
                }
                set
                {
                    this.manualAffectedPartiesField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class ResolvedAffectedPartyStructure
        {

            private ResolvedContactStructure contactField;

            private OnBehalfOfStructure onbehalfOfField;

            private DelegatingToStructure delegatingToField;

            private bool isPoliceField;

            private bool excludeField;

            private AffectedPartyReasonType reasonField;

            private bool reasonFieldSpecified;

            private AffectedPartyReasonExclusionOutcomeType exclusionOutcomeField;

            private bool exclusionOutcomeFieldSpecified;

            private bool isRetainedNotificationOnlyField;

            public ResolvedAffectedPartyStructure()
            {
                this.isPoliceField = false;
                this.excludeField = false;
                this.isRetainedNotificationOnlyField = false;
            }

            /// <remarks/>
            public ResolvedContactStructure Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            public OnBehalfOfStructure OnbehalfOf
            {
                get
                {
                    return this.onbehalfOfField;
                }
                set
                {
                    this.onbehalfOfField = value;
                }
            }

            /// <remarks/>
            public DelegatingToStructure DelegatingTo
            {
                get
                {
                    return this.delegatingToField;
                }
                set
                {
                    this.delegatingToField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsPolice
            {
                get
                {
                    return this.isPoliceField;
                }
                set
                {
                    this.isPoliceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool Exclude
            {
                get
                {
                    return this.excludeField;
                }
                set
                {
                    this.excludeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public AffectedPartyReasonType Reason
            {
                get
                {
                    return this.reasonField;
                }
                set
                {
                    this.reasonField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReasonSpecified
            {
                get
                {
                    return this.reasonFieldSpecified;
                }
                set
                {
                    this.reasonFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public AffectedPartyReasonExclusionOutcomeType ExclusionOutcome
            {
                get
                {
                    return this.exclusionOutcomeField;
                }
                set
                {
                    this.exclusionOutcomeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ExclusionOutcomeSpecified
            {
                get
                {
                    return this.exclusionOutcomeFieldSpecified;
                }
                set
                {
                    this.exclusionOutcomeFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsRetainedNotificationOnly
            {
                get
                {
                    return this.isRetainedNotificationOnlyField;
                }
                set
                {
                    this.isRetainedNotificationOnlyField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class DelegatingToStructure
        {

            private DelegatingToStructure delegatingToField;

            private string delegateesOrganisationIdField;

            private bool delegateesOrganisationIdFieldSpecified;

            private string delegateesContactIdField;

            private bool delegateesContactIdFieldSpecified;

            private string delegationIdField;

            private bool delegationIdFieldSpecified;

            /// <remarks/>
            public DelegatingToStructure DelegatingTo
            {
                get
                {
                    return this.delegatingToField;
                }
                set
                {
                    this.delegatingToField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegateesOrganisationId
            {
                get
                {
                    return this.delegateesOrganisationIdField;
                }
                set
                {
                    this.delegateesOrganisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegateesOrganisationIdSpecified
            {
                get
                {
                    return this.delegateesOrganisationIdFieldSpecified;
                }
                set
                {
                    this.delegateesOrganisationIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegateesContactId
            {
                get
                {
                    return this.delegateesContactIdField;
                }
                set
                {
                    this.delegateesContactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegateesContactIdSpecified
            {
                get
                {
                    return this.delegateesContactIdFieldSpecified;
                }
                set
                {
                    this.delegateesContactIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegationId
            {
                get
                {
                    return this.delegationIdField;
                }
                set
                {
                    this.delegationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegationIdSpecified
            {
                get
                {
                    return this.delegationIdFieldSpecified;
                }
                set
                {
                    this.delegationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public enum AffectedPartyReasonType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("newly affected")]
            newlyaffected,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("no longer affected")]
            nolongeraffected,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("affected by change of route")]
            affectedbychangeofroute,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("still affected")]
            stillaffected,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public enum AffectedPartyReasonExclusionOutcomeType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("newly affected")]
            newlyaffected,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("no longer affected")]
            nolongeraffected,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("affected by change of route")]
            affectedbychangeofroute,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("still affected")]
            stillaffected,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("not affected")]
            notaffected,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute("AffectedParties", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class AffectedPartiesStructure
        {

            private AffectedPartyStructure[] generatedAffectedPartiesField;

            private AffectedPartyStructure[] manualAffectedPartiesField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("AffectedParty", IsNullable = false)]
            public AffectedPartyStructure[] GeneratedAffectedParties
            {
                get
                {
                    return this.generatedAffectedPartiesField;
                }
                set
                {
                    this.generatedAffectedPartiesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("AffectedParty", IsNullable = false)]
            public AffectedPartyStructure[] ManualAffectedParties
            {
                get
                {
                    return this.manualAffectedPartiesField;
                }
                set
                {
                    this.manualAffectedPartiesField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedPartyStructure
        {

            private ContactReferenceStructure contactField;

            private OnBehalfOfStructure onBehalfOfField;

            private DelegatingToStructure delegatingToField;

            private DispensationStatusType dispensationStatusField;

            private bool excludeField;

            private AffectedPartyReasonType reasonField;

            private bool reasonFieldSpecified;

            private AffectedPartyReasonExclusionOutcomeType exclusionOutcomeField;

            private bool exclusionOutcomeFieldSpecified;

            private bool isPoliceField;

            private bool isRetainedNotificationOnlyField;

            public AffectedPartyStructure()
            {
                this.dispensationStatusField = DispensationStatusType.nonematching;
                this.excludeField = false;
                this.isPoliceField = false;
                this.isRetainedNotificationOnlyField = false;
            }

            /// <remarks/>
            public ContactReferenceStructure Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            public OnBehalfOfStructure OnBehalfOf
            {
                get
                {
                    return this.onBehalfOfField;
                }
                set
                {
                    this.onBehalfOfField = value;
                }
            }

            /// <remarks/>
            public DelegatingToStructure DelegatingTo
            {
                get
                {
                    return this.delegatingToField;
                }
                set
                {
                    this.delegatingToField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(DispensationStatusType.nonematching)]
            public DispensationStatusType DispensationStatus
            {
                get
                {
                    return this.dispensationStatusField;
                }
                set
                {
                    this.dispensationStatusField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool Exclude
            {
                get
                {
                    return this.excludeField;
                }
                set
                {
                    this.excludeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public AffectedPartyReasonType Reason
            {
                get
                {
                    return this.reasonField;
                }
                set
                {
                    this.reasonField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReasonSpecified
            {
                get
                {
                    return this.reasonFieldSpecified;
                }
                set
                {
                    this.reasonFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public AffectedPartyReasonExclusionOutcomeType ExclusionOutcome
            {
                get
                {
                    return this.exclusionOutcomeField;
                }
                set
                {
                    this.exclusionOutcomeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ExclusionOutcomeSpecified
            {
                get
                {
                    return this.exclusionOutcomeFieldSpecified;
                }
                set
                {
                    this.exclusionOutcomeFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsPolice
            {
                get
                {
                    return this.isPoliceField;
                }
                set
                {
                    this.isPoliceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsRetainedNotificationOnly
            {
                get
                {
                    return this.isRetainedNotificationOnlyField;
                }
                set
                {
                    this.isRetainedNotificationOnlyField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/dispensation")]
        public enum DispensationStatusType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("in use")]
            inuse,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("none matching")]
            nonematching,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("some matching")]
            somematching,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        [System.Xml.Serialization.XmlRootAttribute("OwnershipDetails", Namespace = "http://www.esdal.com/schemas/core/movement", IsNullable = false)]
        public partial class OwnershipDetailsStructure
        {

            private OwnershipDetailsStructureOwnership[] ownershipField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Ownership")]
            public OwnershipDetailsStructureOwnership[] Ownership
            {
                get
                {
                    return this.ownershipField;
                }
                set
                {
                    this.ownershipField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class OwnershipDetailsStructureOwnership
        {

            private SimpleContactReferenceStructure responsiblePartyField;

            private OnBehalfOfStructure onbehalfOfField;

            /// <remarks/>
            public SimpleContactReferenceStructure ResponsibleParty
            {
                get
                {
                    return this.responsiblePartyField;
                }
                set
                {
                    this.responsiblePartyField = value;
                }
            }

            /// <remarks/>
            public OnBehalfOfStructure OnbehalfOf
            {
                get
                {
                    return this.onbehalfOfField;
                }
                set
                {
                    this.onbehalfOfField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        [System.Xml.Serialization.XmlRootAttribute("EsdalStructure", Namespace = "http://www.esdal.com/schemas/core/structure", IsNullable = false)]
        public partial class EsdalStructureStructure
        {

            private string eSRNField;

            private StructureClassType structureClassField;

            private string nameField;

            private GridReferenceStructure gridReferenceField;

            private EsdalStructureStructureResponsibilityChainContact[][] ownersField;

            private string structureKeyField;

            private string[] alternativeNameField;

            private string descriptionField;

            private string notesField;

            private StructureCategoryStructure structureCategoryField;

            private StructureStructure[] structureTypeField;

            private string lengthField;

            private bool lengthFieldSpecified;

            private OverSectionStructure[] overbridgeSectionsField;

            private EsdalStructureStructureLevelCrossingSections levelCrossingSectionsField;

            private UnderSectionStructure[] underbridgeSectionsField;

            private string structureIdField;

            private bool structureIdFieldSpecified;

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            public StructureClassType StructureClass
            {
                get
                {
                    return this.structureClassField;
                }
                set
                {
                    this.structureClassField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public GridReferenceStructure GridReference
            {
                get
                {
                    return this.gridReferenceField;
                }
                set
                {
                    this.gridReferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ResponsibilityChain", IsNullable = false)]
            [System.Xml.Serialization.XmlArrayItemAttribute("Contact", IsNullable = false, NestingLevel = 1)]
            public EsdalStructureStructureResponsibilityChainContact[][] Owners
            {
                get
                {
                    return this.ownersField;
                }
                set
                {
                    this.ownersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string StructureKey
            {
                get
                {
                    return this.structureKeyField;
                }
                set
                {
                    this.structureKeyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AlternativeName", DataType = "token")]
            public string[] AlternativeName
            {
                get
                {
                    return this.alternativeNameField;
                }
                set
                {
                    this.alternativeNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public string Notes
            {
                get
                {
                    return this.notesField;
                }
                set
                {
                    this.notesField = value;
                }
            }

            /// <remarks/>
            public StructureCategoryStructure StructureCategory
            {
                get
                {
                    return this.structureCategoryField;
                }
                set
                {
                    this.structureCategoryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("StructureType")]
            public StructureStructure[] StructureType
            {
                get
                {
                    return this.structureTypeField;
                }
                set
                {
                    this.structureTypeField = value;
                }
            }

            /// <remarks/>
            public string Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("OverbridgeSection", IsNullable = false)]
            public OverSectionStructure[] OverbridgeSections
            {
                get
                {
                    return this.overbridgeSectionsField;
                }
                set
                {
                    this.overbridgeSectionsField = value;
                }
            }

            /// <remarks/>
            public EsdalStructureStructureLevelCrossingSections LevelCrossingSections
            {
                get
                {
                    return this.levelCrossingSectionsField;
                }
                set
                {
                    this.levelCrossingSectionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("UnderbridgeSection", IsNullable = false)]
            public UnderSectionStructure[] UnderbridgeSections
            {
                get
                {
                    return this.underbridgeSectionsField;
                }
                set
                {
                    this.underbridgeSectionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string StructureId
            {
                get
                {
                    return this.structureIdField;
                }
                set
                {
                    this.structureIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool StructureIdSpecified
            {
                get
                {
                    return this.structureIdFieldSpecified;
                }
                set
                {
                    this.structureIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum StructureClassType
        {

            /// <remarks/>
            underbridge,

            /// <remarks/>
            overbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("under and over bridge")]
            underandoverbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("level crossing")]
            levelcrossing,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("non ail")]
            nonail,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("non esdal")]
            nonesdal,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class EsdalStructureStructureResponsibilityChainContact
        {

            private string organisationIdField;

            private bool organisationIdFieldSpecified;

            private string contactIdField;

            private bool contactIdFieldSpecified;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIdSpecified
            {
                get
                {
                    return this.organisationIdFieldSpecified;
                }
                set
                {
                    this.organisationIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactId
            {
                get
                {
                    return this.contactIdField;
                }
                set
                {
                    this.contactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ContactIdSpecified
            {
                get
                {
                    return this.contactIdFieldSpecified;
                }
                set
                {
                    this.contactIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute(DataType = "token")]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class StructureCategoryStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PredefinedType", typeof(PredefinedStructureCategoryType))]
            [System.Xml.Serialization.XmlElementAttribute("UserDefinedType", typeof(string), DataType = "token")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum PredefinedStructureCategoryType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("road bridge")]
            roadbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("elevated road")]
            elevatedroad,

            /// <remarks/>
            culvert,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("level crossing")]
            levelcrossing,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("tunnel deck")]
            tunneldeck,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("rail bridge")]
            railbridge,

            /// <remarks/>
            aqueduct,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("pedestrian bridge")]
            pedestrianbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("farm access")]
            farmaccess,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("accomodation access")]
            accomodationaccess,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("accomodation access over")]
            accomodationaccessover,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("accomodation access under")]
            accomodationaccessunder,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("non esdal")]
            nonesdal,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class StructureStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PredefinedType", typeof(PredefinedStructureType))]
            [System.Xml.Serialization.XmlElementAttribute("UserDefinedType", typeof(string), DataType = "token")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum PredefinedStructureType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("box culvert")]
            boxculvert,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("cable stayed bridge")]
            cablestayedbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("multi span bridge")]
            multispanbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("continuous span bridge")]
            continuousspanbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("integral structure")]
            integralstructure,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("masonry arch")]
            masonryarch,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("portal frame")]
            portalframe,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("simply supported span")]
            simplysupportedspan,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("suspension bridge")]
            suspensionbridge,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("level crossing")]
            levelcrossing,

            /// <remarks/>
            lift,

            /// <remarks/>
            swing,

            /// <remarks/>
            cantilever,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("propped cantilever")]
            proppedcantilever,

            /// <remarks/>
            pipe,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class OverSectionStructure
        {

            private string descriptionField;

            private string objectCrossedField;

            private string objectCarriedField;

            private SignedSpatialConstraintsStructure signedConstraintsField;

            private SpatialConstraintsStructure unsignedConstraintsField;

            private HeightEnvelopeStructure[] heightEnvelopeField;

            private string directionField;

            private EntityCautionStructure[] cautionsField;

            private string sectionIdField;

            private bool sectionIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public string ObjectCrossed
            {
                get
                {
                    return this.objectCrossedField;
                }
                set
                {
                    this.objectCrossedField = value;
                }
            }

            /// <remarks/>
            public string ObjectCarried
            {
                get
                {
                    return this.objectCarriedField;
                }
                set
                {
                    this.objectCarriedField = value;
                }
            }

            /// <remarks/>
            public SignedSpatialConstraintsStructure SignedConstraints
            {
                get
                {
                    return this.signedConstraintsField;
                }
                set
                {
                    this.signedConstraintsField = value;
                }
            }

            /// <remarks/>
            public SpatialConstraintsStructure UnsignedConstraints
            {
                get
                {
                    return this.unsignedConstraintsField;
                }
                set
                {
                    this.unsignedConstraintsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("HeightEnvelope")]
            public HeightEnvelopeStructure[] HeightEnvelope
            {
                get
                {
                    return this.heightEnvelopeField;
                }
                set
                {
                    this.heightEnvelopeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string Direction
            {
                get
                {
                    return this.directionField;
                }
                set
                {
                    this.directionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Caution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
            public EntityCautionStructure[] Cautions
            {
                get
                {
                    return this.cautionsField;
                }
                set
                {
                    this.cautionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string SectionId
            {
                get
                {
                    return this.sectionIdField;
                }
                set
                {
                    this.sectionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SectionIdSpecified
            {
                get
                {
                    return this.sectionIdFieldSpecified;
                }
                set
                {
                    this.sectionIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class HeightEnvelopeStructure
        {

            private string offsetField;

            private string widthField;

            private string heightField;

            /// <remarks/>
            public string Offset
            {
                get
                {
                    return this.offsetField;
                }
                set
                {
                    this.offsetField = value;
                }
            }

            /// <remarks/>
            public string Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public string Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class EsdalStructureStructureLevelCrossingSections
        {

            private LevelCrossingStructure levelCrossingSectionField;

            /// <remarks/>
            public LevelCrossingStructure LevelCrossingSection
            {
                get
                {
                    return this.levelCrossingSectionField;
                }
                set
                {
                    this.levelCrossingSectionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class LevelCrossingStructure
        {

            private string chainageField;

            private string highwaySkewField;

            private SignedSpatialConstraintsStructure signedSpatialConstraintsField;

            private SpatialConstraintsStructure unsignedSpatialConstraintsField;

            private SignedWeightConstraintsStructure signedWeightConstraintsField;

            private WeightConstraintsStructure unsignedWeightConstraintsField;

            private LevelCrossingAlignmentStructure verticalAlignmentField;

            private EntityCautionStructure[] cautionsField;

            private string sectionIdField;

            private bool sectionIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Chainage
            {
                get
                {
                    return this.chainageField;
                }
                set
                {
                    this.chainageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string HighwaySkew
            {
                get
                {
                    return this.highwaySkewField;
                }
                set
                {
                    this.highwaySkewField = value;
                }
            }

            /// <remarks/>
            public SignedSpatialConstraintsStructure SignedSpatialConstraints
            {
                get
                {
                    return this.signedSpatialConstraintsField;
                }
                set
                {
                    this.signedSpatialConstraintsField = value;
                }
            }

            /// <remarks/>
            public SpatialConstraintsStructure UnsignedSpatialConstraints
            {
                get
                {
                    return this.unsignedSpatialConstraintsField;
                }
                set
                {
                    this.unsignedSpatialConstraintsField = value;
                }
            }

            /// <remarks/>
            public SignedWeightConstraintsStructure SignedWeightConstraints
            {
                get
                {
                    return this.signedWeightConstraintsField;
                }
                set
                {
                    this.signedWeightConstraintsField = value;
                }
            }

            /// <remarks/>
            public WeightConstraintsStructure UnsignedWeightConstraints
            {
                get
                {
                    return this.unsignedWeightConstraintsField;
                }
                set
                {
                    this.unsignedWeightConstraintsField = value;
                }
            }

            /// <remarks/>
            public LevelCrossingAlignmentStructure VerticalAlignment
            {
                get
                {
                    return this.verticalAlignmentField;
                }
                set
                {
                    this.verticalAlignmentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Caution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
            public EntityCautionStructure[] Cautions
            {
                get
                {
                    return this.cautionsField;
                }
                set
                {
                    this.cautionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string SectionId
            {
                get
                {
                    return this.sectionIdField;
                }
                set
                {
                    this.sectionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SectionIdSpecified
            {
                get
                {
                    return this.sectionIdFieldSpecified;
                }
                set
                {
                    this.sectionIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class LevelCrossingAlignmentStructure
        {

            private VerticalAlignmentPointStructure entryField;

            private VerticalAlignmentPointStructure maxHeightField;

            private VerticalAlignmentPointStructure exitField;

            /// <remarks/>
            public VerticalAlignmentPointStructure Entry
            {
                get
                {
                    return this.entryField;
                }
                set
                {
                    this.entryField = value;
                }
            }

            /// <remarks/>
            public VerticalAlignmentPointStructure MaxHeight
            {
                get
                {
                    return this.maxHeightField;
                }
                set
                {
                    this.maxHeightField = value;
                }
            }

            /// <remarks/>
            public VerticalAlignmentPointStructure Exit
            {
                get
                {
                    return this.exitField;
                }
                set
                {
                    this.exitField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class VerticalAlignmentPointStructure
        {

            private string distanceField;

            private string heightField;

            /// <remarks/>
            public string Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            public string Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class UnderSectionStructure
        {

            private string descriptionField;

            private string objectCarriedField;

            private string objectCrossedField;

            private ConstructionStructure constructionField;

            private string skewAngleField;

            private string lengthField;

            private bool lengthFieldSpecified;

            private string maxSpanLengthField;

            private bool maxSpanLengthFieldSpecified;

            private SignedSpatialConstraintsStructure signedSpatialConstraintsField;

            private SpatialConstraintsStructure unsignedSpatialConstraintsField;

            private WeightConstraintsStructure unsignedWeightConstraintsField;

            private SignedWeightConstraintsStructure signedWeightConstraintsField;

            private LoadRatingStructure loadRatingField;

            private string directionField;

            private SpanStructure[] spanField;

            private decimal[] carriagewayWidthField;

            private decimal[] deckWidthField;

            private ArchStructure[] archField;

            private EntityCautionStructure[] cautionsField;

            private InfluenceLineDataStructure influenceLineDataField;

            private string sectionIdField;

            private bool sectionIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public string ObjectCarried
            {
                get
                {
                    return this.objectCarriedField;
                }
                set
                {
                    this.objectCarriedField = value;
                }
            }

            /// <remarks/>
            public string ObjectCrossed
            {
                get
                {
                    return this.objectCrossedField;
                }
                set
                {
                    this.objectCrossedField = value;
                }
            }

            /// <remarks/>
            public ConstructionStructure Construction
            {
                get
                {
                    return this.constructionField;
                }
                set
                {
                    this.constructionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string SkewAngle
            {
                get
                {
                    return this.skewAngleField;
                }
                set
                {
                    this.skewAngleField = value;
                }
            }

            /// <remarks/>
            public string Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string MaxSpanLength
            {
                get
                {
                    return this.maxSpanLengthField;
                }
                set
                {
                    this.maxSpanLengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaxSpanLengthSpecified
            {
                get
                {
                    return this.maxSpanLengthFieldSpecified;
                }
                set
                {
                    this.maxSpanLengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SignedSpatialConstraintsStructure SignedSpatialConstraints
            {
                get
                {
                    return this.signedSpatialConstraintsField;
                }
                set
                {
                    this.signedSpatialConstraintsField = value;
                }
            }

            /// <remarks/>
            public SpatialConstraintsStructure UnsignedSpatialConstraints
            {
                get
                {
                    return this.unsignedSpatialConstraintsField;
                }
                set
                {
                    this.unsignedSpatialConstraintsField = value;
                }
            }

            /// <remarks/>
            public WeightConstraintsStructure UnsignedWeightConstraints
            {
                get
                {
                    return this.unsignedWeightConstraintsField;
                }
                set
                {
                    this.unsignedWeightConstraintsField = value;
                }
            }

            /// <remarks/>
            public SignedWeightConstraintsStructure SignedWeightConstraints
            {
                get
                {
                    return this.signedWeightConstraintsField;
                }
                set
                {
                    this.signedWeightConstraintsField = value;
                }
            }

            /// <remarks/>
            public LoadRatingStructure LoadRating
            {
                get
                {
                    return this.loadRatingField;
                }
                set
                {
                    this.loadRatingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string Direction
            {
                get
                {
                    return this.directionField;
                }
                set
                {
                    this.directionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Span")]
            public SpanStructure[] Span
            {
                get
                {
                    return this.spanField;
                }
                set
                {
                    this.spanField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("CarriagewayWidth")]
            public decimal[] CarriagewayWidth
            {
                get
                {
                    return this.carriagewayWidthField;
                }
                set
                {
                    this.carriagewayWidthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("DeckWidth")]
            public decimal[] DeckWidth
            {
                get
                {
                    return this.deckWidthField;
                }
                set
                {
                    this.deckWidthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Arch")]
            public ArchStructure[] Arch
            {
                get
                {
                    return this.archField;
                }
                set
                {
                    this.archField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Caution", Namespace = "http://www.esdal.com/schemas/core/caution", IsNullable = false)]
            public EntityCautionStructure[] Cautions
            {
                get
                {
                    return this.cautionsField;
                }
                set
                {
                    this.cautionsField = value;
                }
            }

            /// <remarks/>
            public InfluenceLineDataStructure InfluenceLineData
            {
                get
                {
                    return this.influenceLineDataField;
                }
                set
                {
                    this.influenceLineDataField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string SectionId
            {
                get
                {
                    return this.sectionIdField;
                }
                set
                {
                    this.sectionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SectionIdSpecified
            {
                get
                {
                    return this.sectionIdFieldSpecified;
                }
                set
                {
                    this.sectionIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class ConstructionStructure
        {

            private string numberOfSpansField;

            private string numberOfDecksField;

            private ConstructionTypeStructure[] constructionTypeField;

            private DeckMaterialStructure[] deckMaterialField;

            private BearingStructure[] bearingsField;

            private FoundationStructure[] foundationsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string NumberOfSpans
            {
                get
                {
                    return this.numberOfSpansField;
                }
                set
                {
                    this.numberOfSpansField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string NumberOfDecks
            {
                get
                {
                    return this.numberOfDecksField;
                }
                set
                {
                    this.numberOfDecksField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ConstructionType")]
            public ConstructionTypeStructure[] ConstructionType
            {
                get
                {
                    return this.constructionTypeField;
                }
                set
                {
                    this.constructionTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("DeckMaterial")]
            public DeckMaterialStructure[] DeckMaterial
            {
                get
                {
                    return this.deckMaterialField;
                }
                set
                {
                    this.deckMaterialField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Bearings")]
            public BearingStructure[] Bearings
            {
                get
                {
                    return this.bearingsField;
                }
                set
                {
                    this.bearingsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Foundations")]
            public FoundationStructure[] Foundations
            {
                get
                {
                    return this.foundationsField;
                }
                set
                {
                    this.foundationsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class ConstructionTypeStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PredefinedType", typeof(PredefinedConstructionType))]
            [System.Xml.Serialization.XmlElementAttribute("UserDefinedType", typeof(string), DataType = "token")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum PredefinedConstructionType
        {

            /// <remarks/>
            arch,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("metallic widened arch")]
            metallicwidenedarch,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("composite widened arch")]
            compositewidenedarch,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("slab widened arch")]
            slabwidenedarch,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("post tensioned beam and slab")]
            posttensionedbeamandslab,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("pre tensioned beam and slab")]
            pretensionedbeamandslab,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("reinforced concrete beam and slab")]
            reinforcedconcretebeamandslab,

            /// <remarks/>
            metallic,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("prestressed concrete box")]
            prestressedconcretebox,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("prestressed concrete slab")]
            prestressedconcreteslab,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("reinforced concrete slab")]
            reinforcedconcreteslab,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("steel and concrete composite")]
            steelandconcretecomposite,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("arch plus elements")]
            archpluselements,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("arch plus slab")]
            archplusslab,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("beam and slab")]
            beamandslab,

            /// <remarks/>
            box,

            /// <remarks/>
            orthotropic,

            /// <remarks/>
            steel,

            /// <remarks/>
            slab,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("filled arch")]
            filledarch,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class DeckMaterialStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PredefinedType", typeof(PredefinedDeckMaterialType))]
            [System.Xml.Serialization.XmlElementAttribute("UserDefinedType", typeof(string), DataType = "token")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum PredefinedDeckMaterialType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("cast iron")]
            castiron,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("composite fpr")]
            compositefpr,

            /// <remarks/>
            concrete,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("post tensioned concrete")]
            posttensionedconcrete,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("pre stressed concrete")]
            prestressedconcrete,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("reinforced concrete")]
            reinforcedconcrete,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("masonry or brick")]
            masonryorbrick,

            /// <remarks/>
            steel,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("steel and concrete composite")]
            steelandconcretecomposite,

            /// <remarks/>
            metal,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("wrought iron")]
            wroughtiron,

            /// <remarks/>
            timber,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("corrugated steel")]
            corrugatedsteel,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class BearingStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PredefinedType", typeof(PredefinedBearingType))]
            [System.Xml.Serialization.XmlElementAttribute("UserDefinedType", typeof(string), DataType = "token")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum PredefinedBearingType
        {

            /// <remarks/>
            none,

            /// <remarks/>
            elastomeric,

            /// <remarks/>
            guided,

            /// <remarks/>
            rocker,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class FoundationStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PredefinedType", typeof(PredefinedFoundationType))]
            [System.Xml.Serialization.XmlElementAttribute("UserDefinedType", typeof(string), DataType = "token")]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum PredefinedFoundationType
        {

            /// <remarks/>
            pad,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("pile cap")]
            pilecap,

            /// <remarks/>
            piles,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("spread and strip")]
            spreadandstrip,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("spread footing")]
            spreadfooting,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("brick stone masonry")]
            brickstonemasonry,

            /// <remarks/>
            caissons,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("granular fill")]
            granularfill,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("reinforced soild foundation mattress")]
            reinforcedsoildfoundationmattress,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class LoadRatingStructure
        {

            private object haField;

            private string hBRatingWithLoadField;

            private bool hBRatingWithLoadFieldSpecified;

            private string hBRatingWithoutLoadField;

            private bool hBRatingWithoutLoadFieldSpecified;

            private SVVehicleType sVRatingField;

            private bool sVRatingFieldSpecified;

            private SVParametersStructure[] sVParametersField;

            /// <remarks/>
            public object HA
            {
                get
                {
                    return this.haField;
                }
                set
                {
                    this.haField = value;
                }
            }

            /// <remarks/>
            public string HBRatingWithLoad
            {
                get
                {
                    return this.hBRatingWithLoadField;
                }
                set
                {
                    this.hBRatingWithLoadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool HBRatingWithLoadSpecified
            {
                get
                {
                    return this.hBRatingWithLoadFieldSpecified;
                }
                set
                {
                    this.hBRatingWithLoadFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string HBRatingWithoutLoad
            {
                get
                {
                    return this.hBRatingWithoutLoadField;
                }
                set
                {
                    this.hBRatingWithoutLoadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool HBRatingWithoutLoadSpecified
            {
                get
                {
                    return this.hBRatingWithoutLoadFieldSpecified;
                }
                set
                {
                    this.hBRatingWithoutLoadFieldSpecified = value;
                }
            }

            /// <remarks/>
            public SVVehicleType SVRating
            {
                get
                {
                    return this.sVRatingField;
                }
                set
                {
                    this.sVRatingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SVRatingSpecified
            {
                get
                {
                    return this.sVRatingFieldSpecified;
                }
                set
                {
                    this.sVRatingFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SVParameters")]
            public SVParametersStructure[] SVParameters
            {
                get
                {
                    return this.sVParametersField;
                }
                set
                {
                    this.sVParametersField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum SVVehicleType
        {

            /// <remarks/>
            svnone,

            /// <remarks/>
            sv80,

            /// <remarks/>
            sv100,

            /// <remarks/>
            sv150,

            /// <remarks/>
            svtrain,

            /// <remarks/>
            svtt,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SVParametersStructure
        {

            private SVVehicleType vehicleTypeField;

            private string sVReserveWithLoadField;

            private bool sVReserveWithLoadFieldSpecified;

            private string sVReserveWithoutLoadField;

            private bool sVReserveWithoutLoadFieldSpecified;

            private string manualHBtoSVConversionFactorField;

            private bool manualHBtoSVConversionFactorFieldSpecified;

            private string calculatedHBtoSVConversionFactorField;

            private bool calculatedHBtoSVConversionFactorFieldSpecified;

            private SVReserveFactorDerivationType derivationField;

            public SVParametersStructure()
            {
                this.derivationField = SVReserveFactorDerivationType.raw;
            }

            /// <remarks/>
            public SVVehicleType VehicleType
            {
                get
                {
                    return this.vehicleTypeField;
                }
                set
                {
                    this.vehicleTypeField = value;
                }
            }

            /// <remarks/>
            public string SVReserveWithLoad
            {
                get
                {
                    return this.sVReserveWithLoadField;
                }
                set
                {
                    this.sVReserveWithLoadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SVReserveWithLoadSpecified
            {
                get
                {
                    return this.sVReserveWithLoadFieldSpecified;
                }
                set
                {
                    this.sVReserveWithLoadFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string SVReserveWithoutLoad
            {
                get
                {
                    return this.sVReserveWithoutLoadField;
                }
                set
                {
                    this.sVReserveWithoutLoadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SVReserveWithoutLoadSpecified
            {
                get
                {
                    return this.sVReserveWithoutLoadFieldSpecified;
                }
                set
                {
                    this.sVReserveWithoutLoadFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ManualHBtoSVConversionFactor
            {
                get
                {
                    return this.manualHBtoSVConversionFactorField;
                }
                set
                {
                    this.manualHBtoSVConversionFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ManualHBtoSVConversionFactorSpecified
            {
                get
                {
                    return this.manualHBtoSVConversionFactorFieldSpecified;
                }
                set
                {
                    this.manualHBtoSVConversionFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string CalculatedHBtoSVConversionFactor
            {
                get
                {
                    return this.calculatedHBtoSVConversionFactorField;
                }
                set
                {
                    this.calculatedHBtoSVConversionFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool CalculatedHBtoSVConversionFactorSpecified
            {
                get
                {
                    return this.calculatedHBtoSVConversionFactorFieldSpecified;
                }
                set
                {
                    this.calculatedHBtoSVConversionFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(SVReserveFactorDerivationType.raw)]
            public SVReserveFactorDerivationType Derivation
            {
                get
                {
                    return this.derivationField;
                }
                set
                {
                    this.derivationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum SVReserveFactorDerivationType
        {

            /// <remarks/>
            raw,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("manual hb to sv conversion")]
            manualhbtosvconversion,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("calculated hb to sv conversion")]
            calculatedhbtosvconversion,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SpanStructure
        {

            private string spanNumberField;

            private SpanPositionStructure spanPositionField;

            private string lengthField;

            private bool lengthFieldSpecified;

            private string descriptionField;

            private StructureStructure[] structureTypeField;

            private ConstructionTypeStructure[] constructionField;

            private DeckMaterialStructure[] deckMaterialField;

            private BearingStructure[] bearingField;

            private FoundationStructure[] foundationField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string SpanNumber
            {
                get
                {
                    return this.spanNumberField;
                }
                set
                {
                    this.spanNumberField = value;
                }
            }

            /// <remarks/>
            public SpanPositionStructure SpanPosition
            {
                get
                {
                    return this.spanPositionField;
                }
                set
                {
                    this.spanPositionField = value;
                }
            }

            /// <remarks/>
            public string Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("StructureType")]
            public StructureStructure[] StructureType
            {
                get
                {
                    return this.structureTypeField;
                }
                set
                {
                    this.structureTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Construction")]
            public ConstructionTypeStructure[] Construction
            {
                get
                {
                    return this.constructionField;
                }
                set
                {
                    this.constructionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("DeckMaterial")]
            public DeckMaterialStructure[] DeckMaterial
            {
                get
                {
                    return this.deckMaterialField;
                }
                set
                {
                    this.deckMaterialField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Bearing")]
            public BearingStructure[] Bearing
            {
                get
                {
                    return this.bearingField;
                }
                set
                {
                    this.bearingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Foundation")]
            public FoundationStructure[] Foundation
            {
                get
                {
                    return this.foundationField;
                }
                set
                {
                    this.foundationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SpanPositionStructure
        {

            private string sequencePositionField;

            private string sequenceNumberField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string SequencePosition
            {
                get
                {
                    return this.sequencePositionField;
                }
                set
                {
                    this.sequencePositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string SequenceNumber
            {
                get
                {
                    return this.sequenceNumberField;
                }
                set
                {
                    this.sequenceNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class ArchStructure
        {

            private string spanOfArchField;

            private bool spanOfArchFieldSpecified;

            private string riseAtCrownField;

            private bool riseAtCrownFieldSpecified;

            private string riseAtQuarterField;

            private bool riseAtQuarterFieldSpecified;

            private string depthOfFillField;

            private bool depthOfFillFieldSpecified;

            private string barrelThicknessField;

            private bool barrelThicknessFieldSpecified;

            private string barrelFactorField;

            private bool barrelFactorFieldSpecified;

            private string conditionFactorField;

            private bool conditionFactorFieldSpecified;

            private string depthFactorField;

            private bool depthFactorFieldSpecified;

            private string fillFactorField;

            private bool fillFactorFieldSpecified;

            private string jointWidthFactorField;

            private bool jointWidthFactorFieldSpecified;

            private string mortarFactorField;

            private bool mortarFactorFieldSpecified;

            private bool axleLiftOffField;

            private bool axleLiftOffFieldSpecified;

            private string materialFactorField;

            private bool materialFactorFieldSpecified;

            private string jointFactorField;

            private bool jointFactorFieldSpecified;

            private string profileFactorField;

            private bool profileFactorFieldSpecified;

            private string spanRiseFactorField;

            private bool spanRiseFactorFieldSpecified;

            private string provisionalAxleLoadField;

            private bool provisionalAxleLoadFieldSpecified;

            private string modificationFactorField;

            private bool modificationFactorFieldSpecified;

            private string modifiedAxleLoadField;

            private bool modifiedAxleLoadFieldSpecified;

            /// <remarks/>
            public string SpanOfArch
            {
                get
                {
                    return this.spanOfArchField;
                }
                set
                {
                    this.spanOfArchField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SpanOfArchSpecified
            {
                get
                {
                    return this.spanOfArchFieldSpecified;
                }
                set
                {
                    this.spanOfArchFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string RiseAtCrown
            {
                get
                {
                    return this.riseAtCrownField;
                }
                set
                {
                    this.riseAtCrownField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RiseAtCrownSpecified
            {
                get
                {
                    return this.riseAtCrownFieldSpecified;
                }
                set
                {
                    this.riseAtCrownFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string RiseAtQuarter
            {
                get
                {
                    return this.riseAtQuarterField;
                }
                set
                {
                    this.riseAtQuarterField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RiseAtQuarterSpecified
            {
                get
                {
                    return this.riseAtQuarterFieldSpecified;
                }
                set
                {
                    this.riseAtQuarterFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string DepthOfFill
            {
                get
                {
                    return this.depthOfFillField;
                }
                set
                {
                    this.depthOfFillField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DepthOfFillSpecified
            {
                get
                {
                    return this.depthOfFillFieldSpecified;
                }
                set
                {
                    this.depthOfFillFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string BarrelThickness
            {
                get
                {
                    return this.barrelThicknessField;
                }
                set
                {
                    this.barrelThicknessField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool BarrelThicknessSpecified
            {
                get
                {
                    return this.barrelThicknessFieldSpecified;
                }
                set
                {
                    this.barrelThicknessFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string BarrelFactor
            {
                get
                {
                    return this.barrelFactorField;
                }
                set
                {
                    this.barrelFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool BarrelFactorSpecified
            {
                get
                {
                    return this.barrelFactorFieldSpecified;
                }
                set
                {
                    this.barrelFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ConditionFactor
            {
                get
                {
                    return this.conditionFactorField;
                }
                set
                {
                    this.conditionFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ConditionFactorSpecified
            {
                get
                {
                    return this.conditionFactorFieldSpecified;
                }
                set
                {
                    this.conditionFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string DepthFactor
            {
                get
                {
                    return this.depthFactorField;
                }
                set
                {
                    this.depthFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DepthFactorSpecified
            {
                get
                {
                    return this.depthFactorFieldSpecified;
                }
                set
                {
                    this.depthFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string FillFactor
            {
                get
                {
                    return this.fillFactorField;
                }
                set
                {
                    this.fillFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool FillFactorSpecified
            {
                get
                {
                    return this.fillFactorFieldSpecified;
                }
                set
                {
                    this.fillFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string JointWidthFactor
            {
                get
                {
                    return this.jointWidthFactorField;
                }
                set
                {
                    this.jointWidthFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool JointWidthFactorSpecified
            {
                get
                {
                    return this.jointWidthFactorFieldSpecified;
                }
                set
                {
                    this.jointWidthFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string MortarFactor
            {
                get
                {
                    return this.mortarFactorField;
                }
                set
                {
                    this.mortarFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MortarFactorSpecified
            {
                get
                {
                    return this.mortarFactorFieldSpecified;
                }
                set
                {
                    this.mortarFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public bool AxleLiftOff
            {
                get
                {
                    return this.axleLiftOffField;
                }
                set
                {
                    this.axleLiftOffField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool AxleLiftOffSpecified
            {
                get
                {
                    return this.axleLiftOffFieldSpecified;
                }
                set
                {
                    this.axleLiftOffFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string MaterialFactor
            {
                get
                {
                    return this.materialFactorField;
                }
                set
                {
                    this.materialFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaterialFactorSpecified
            {
                get
                {
                    return this.materialFactorFieldSpecified;
                }
                set
                {
                    this.materialFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string JointFactor
            {
                get
                {
                    return this.jointFactorField;
                }
                set
                {
                    this.jointFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool JointFactorSpecified
            {
                get
                {
                    return this.jointFactorFieldSpecified;
                }
                set
                {
                    this.jointFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ProfileFactor
            {
                get
                {
                    return this.profileFactorField;
                }
                set
                {
                    this.profileFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ProfileFactorSpecified
            {
                get
                {
                    return this.profileFactorFieldSpecified;
                }
                set
                {
                    this.profileFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string SpanRiseFactor
            {
                get
                {
                    return this.spanRiseFactorField;
                }
                set
                {
                    this.spanRiseFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SpanRiseFactorSpecified
            {
                get
                {
                    return this.spanRiseFactorFieldSpecified;
                }
                set
                {
                    this.spanRiseFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ProvisionalAxleLoad
            {
                get
                {
                    return this.provisionalAxleLoadField;
                }
                set
                {
                    this.provisionalAxleLoadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ProvisionalAxleLoadSpecified
            {
                get
                {
                    return this.provisionalAxleLoadFieldSpecified;
                }
                set
                {
                    this.provisionalAxleLoadFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ModificationFactor
            {
                get
                {
                    return this.modificationFactorField;
                }
                set
                {
                    this.modificationFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ModificationFactorSpecified
            {
                get
                {
                    return this.modificationFactorFieldSpecified;
                }
                set
                {
                    this.modificationFactorFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string ModifiedAxleLoad
            {
                get
                {
                    return this.modifiedAxleLoadField;
                }
                set
                {
                    this.modifiedAxleLoadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ModifiedAxleLoadSpecified
            {
                get
                {
                    return this.modifiedAxleLoadFieldSpecified;
                }
                set
                {
                    this.modifiedAxleLoadFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class InfluenceLineDataStructure
        {

            private InfluenceLineStructure[] influenceLineField;

            private ComparisonVehicleChoiceStructure[] comparisonVehicleField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("InfluenceLine")]
            public InfluenceLineStructure[] InfluenceLine
            {
                get
                {
                    return this.influenceLineField;
                }
                set
                {
                    this.influenceLineField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ComparisonVehicle")]
            public ComparisonVehicleChoiceStructure[] ComparisonVehicle
            {
                get
                {
                    return this.comparisonVehicleField;
                }
                set
                {
                    this.comparisonVehicleField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class InfluenceLineStructure
        {

            private string nameField;

            private string descriptionField;

            private InfluenceLineEvaluationDirectionType evaluationDirectionField;

            private string evaluationPointsField;

            private InfluenceLineCoordinateStructure[] lineField;

            private string idField;

            private bool idFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public InfluenceLineEvaluationDirectionType EvaluationDirection
            {
                get
                {
                    return this.evaluationDirectionField;
                }
                set
                {
                    this.evaluationDirectionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string EvaluationPoints
            {
                get
                {
                    return this.evaluationPointsField;
                }
                set
                {
                    this.evaluationPointsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Coordinate", IsNullable = false)]
            public InfluenceLineCoordinateStructure[] Line
            {
                get
                {
                    return this.lineField;
                }
                set
                {
                    this.lineField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool IdSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public enum InfluenceLineEvaluationDirectionType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("left to right")]
            lefttoright,

            /// <remarks/>
            both,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class InfluenceLineCoordinateStructure
        {

            private string xField;

            private float yField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string X
            {
                get
                {
                    return this.xField;
                }
                set
                {
                    this.xField = value;
                }
            }

            /// <remarks/>
            public float Y
            {
                get
                {
                    return this.yField;
                }
                set
                {
                    this.yField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class ComparisonVehicleChoiceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("HB", typeof(HBComparisonVehicleStructure))]
            [System.Xml.Serialization.XmlElementAttribute("SV", typeof(SVComparisonVehicleStructure))]
            [System.Xml.Serialization.XmlElementAttribute("UserDefined", typeof(UserDefinedComparisonVehicleStructure))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class HBComparisonVehicleStructure
        {

            private string unitsOfHBField;

            /// <remarks/>
            public string UnitsOfHB
            {
                get
                {
                    return this.unitsOfHBField;
                }
                set
                {
                    this.unitsOfHBField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class SVComparisonVehicleStructure
        {

            private SVVehicleType vehicleTypeField;

            private string reserveFactorField;

            private bool reserveFactorFieldSpecified;

            /// <remarks/>
            public SVVehicleType VehicleType
            {
                get
                {
                    return this.vehicleTypeField;
                }
                set
                {
                    this.vehicleTypeField = value;
                }
            }

            /// <remarks/>
            public string ReserveFactor
            {
                get
                {
                    return this.reserveFactorField;
                }
                set
                {
                    this.reserveFactorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReserveFactorSpecified
            {
                get
                {
                    return this.reserveFactorFieldSpecified;
                }
                set
                {
                    this.reserveFactorFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/structure")]
        public partial class UserDefinedComparisonVehicleStructure
        {

            private string descriptionField;

            private string[] axleWeightField;

            private string[] axleSpacingField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleWeight", DataType = "integer")]
            public string[] AxleWeight
            {
                get
                {
                    return this.axleWeightField;
                }
                set
                {
                    this.axleWeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AxleSpacing", DataType = "integer")]
            public string[] AxleSpacing
            {
                get
                {
                    return this.axleSpacingField;
                }
                set
                {
                    this.axleSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/proposedroute")]
        [System.Xml.Serialization.XmlRootAttribute("Proposal", Namespace = "http://www.esdal.com/schemas/core/proposedroute", IsNullable = false)]
        public partial class ProposalStructure : SpecialOrderRouteStructure
        {

            private bool isReproposalField;

            private bool isFailedDelegationAlertField;

            private string NotificationId;

            public ProposalStructure()
            {
                this.isReproposalField = false;
                this.isFailedDelegationAlertField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsReproposal
            {
                get
                {
                    return this.isReproposalField;
                }
                set
                {
                    this.isReproposalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsFailedDelegationAlert
            {
                get
                {
                    return this.isFailedDelegationAlertField;
                }
                set
                {
                    this.isFailedDelegationAlertField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public string NotificationID
            {
                get
                {
                    return this.NotificationId;
                }
                set
                {
                    this.NotificationId = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProposalStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public abstract partial class SpecialOrderRouteStructure
        {

            private ESDALReferenceNumberStructure eSDALReferenceNumberField;

            private ESDALReferenceNumberStructure lastRouteReceivedReferenceNumberField;

            private string distributionCommentsField;

            private string quickReferenceField;

            private string jobFileReferenceField;

            private SpecialOrderRouteStructureOldJobFileReference oldJobFileReferenceField;

            private string sentDateTimeField;

            private HAContactStructure hAContactField;

            private SpecialOrderRouteStructureOldHAContact oldHAContactField;

            private RecipientContactStructure[] recipientsField;

            private BasicRecipientContactStructure failedNotificationContactField;

            private HaulierDetailsStructure haulierDetailsField;

            private SpecialOrderRouteStructureOldHaulierDetails oldHaulierDetailsField;

            private string hauliersReferenceField;

            private SpecialOrderRouteStructureOldHauliersReference oldHauliersReferenceField;

            private JourneyFromToSummaryStructure journeyFromToSummaryField;

            private SpecialOrderRouteStructureOldJourneyFromToSummary oldJourneyFromToSummaryField;

            private JourneyFromToStructure journeyFromToField;

            private JourneyDateStructure journeyTimingField;

            private SpecialOrderRouteStructureOldJourneyTiming oldJourneyTimingField;

            private string loadSummaryField;

            private SpecialOrderRouteStructureOldLoadSummary oldLoadSummaryField;

            private LoadDetailsStructure loadDetailsField;

            private SpecialOrderRouteStructureOldLoadDetails oldLoadDetailsField;

            private RoutePartsStructureRoutePartListPosition[] routePartsField;

            private SpecialOrderRouteStructureOldRouteParts oldRoutePartsField;

            private string notesFromHaulierField;

            private SpecialOrderRouteStructureOldNotesFromHaulier oldNotesFromHaulierField;

            private PredefinedCautionsDescriptionsStructure1 predefinedCautionsField;

            private ComplexTextStructure notesForHaulierField;

            private SpecialOrderRouteStructureOldNotesForHaulier oldNotesForHaulierField;

            private bool isRedlinedField;

            public SpecialOrderRouteStructure()
            {
                this.isRedlinedField = false;
            }

            /// <remarks/>
            public ESDALReferenceNumberStructure ESDALReferenceNumber
            {
                get
                {
                    return this.eSDALReferenceNumberField;
                }
                set
                {
                    this.eSDALReferenceNumberField = value;
                }
            }

            /// <remarks/>
            public ESDALReferenceNumberStructure LastRouteReceivedReferenceNumber
            {
                get
                {
                    return this.lastRouteReceivedReferenceNumberField;
                }
                set
                {
                    this.lastRouteReceivedReferenceNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string DistributionComments
            {
                get
                {
                    return this.distributionCommentsField;
                }
                set
                {
                    this.distributionCommentsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string QuickReference
            {
                get
                {
                    return this.quickReferenceField;
                }
                set
                {
                    this.quickReferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string JobFileReference
            {
                get
                {
                    return this.jobFileReferenceField;
                }
                set
                {
                    this.jobFileReferenceField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldJobFileReference OldJobFileReference
            {
                get
                {
                    return this.oldJobFileReferenceField;
                }
                set
                {
                    this.oldJobFileReferenceField = value;
                }
            }

            /// <remarks/>
            public string SentDateTime
            {
                get
                {
                    return this.sentDateTimeField;
                }
                set
                {
                    this.sentDateTimeField = value;
                }
            }

            /// <remarks/>
            public HAContactStructure HAContact
            {
                get
                {
                    return this.hAContactField;
                }
                set
                {
                    this.hAContactField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldHAContact OldHAContact
            {
                get
                {
                    return this.oldHAContactField;
                }
                set
                {
                    this.oldHAContactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Contact", IsNullable = false)]
            public RecipientContactStructure[] Recipients
            {
                get
                {
                    return this.recipientsField;
                }
                set
                {
                    this.recipientsField = value;
                }
            }

            /// <remarks/>
            public BasicRecipientContactStructure FailedNotificationContact
            {
                get
                {
                    return this.failedNotificationContactField;
                }
                set
                {
                    this.failedNotificationContactField = value;
                }
            }

            /// <remarks/>
            public HaulierDetailsStructure HaulierDetails
            {
                get
                {
                    return this.haulierDetailsField;
                }
                set
                {
                    this.haulierDetailsField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldHaulierDetails OldHaulierDetails
            {
                get
                {
                    return this.oldHaulierDetailsField;
                }
                set
                {
                    this.oldHaulierDetailsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string HauliersReference
            {
                get
                {
                    return this.hauliersReferenceField;
                }
                set
                {
                    this.hauliersReferenceField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldHauliersReference OldHauliersReference
            {
                get
                {
                    return this.oldHauliersReferenceField;
                }
                set
                {
                    this.oldHauliersReferenceField = value;
                }
            }

            /// <remarks/>
            public JourneyFromToSummaryStructure JourneyFromToSummary
            {
                get
                {
                    return this.journeyFromToSummaryField;
                }
                set
                {
                    this.journeyFromToSummaryField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldJourneyFromToSummary OldJourneyFromToSummary
            {
                get
                {
                    return this.oldJourneyFromToSummaryField;
                }
                set
                {
                    this.oldJourneyFromToSummaryField = value;
                }
            }

            /// <remarks/>
            public JourneyFromToStructure JourneyFromTo
            {
                get
                {
                    return this.journeyFromToField;
                }
                set
                {
                    this.journeyFromToField = value;
                }
            }

            /// <remarks/>
            public JourneyDateStructure JourneyTiming
            {
                get
                {
                    return this.journeyTimingField;
                }
                set
                {
                    this.journeyTimingField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldJourneyTiming OldJourneyTiming
            {
                get
                {
                    return this.oldJourneyTimingField;
                }
                set
                {
                    this.oldJourneyTimingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string LoadSummary
            {
                get
                {
                    return this.loadSummaryField;
                }
                set
                {
                    this.loadSummaryField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldLoadSummary OldLoadSummary
            {
                get
                {
                    return this.oldLoadSummaryField;
                }
                set
                {
                    this.oldLoadSummaryField = value;
                }
            }

            /// <remarks/>
            public LoadDetailsStructure LoadDetails
            {
                get
                {
                    return this.loadDetailsField;
                }
                set
                {
                    this.loadDetailsField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldLoadDetails OldLoadDetails
            {
                get
                {
                    return this.oldLoadDetailsField;
                }
                set
                {
                    this.oldLoadDetailsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoutePartListPosition", IsNullable = false)]
            public RoutePartsStructureRoutePartListPosition[] RouteParts
            {
                get
                {
                    return this.routePartsField;
                }
                set
                {
                    this.routePartsField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldRouteParts OldRouteParts
            {
                get
                {
                    return this.oldRoutePartsField;
                }
                set
                {
                    this.oldRoutePartsField = value;
                }
            }

            /// <remarks/>
            public string NotesFromHaulier
            {
                get
                {
                    return this.notesFromHaulierField;
                }
                set
                {
                    this.notesFromHaulierField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldNotesFromHaulier OldNotesFromHaulier
            {
                get
                {
                    return this.oldNotesFromHaulierField;
                }
                set
                {
                    this.oldNotesFromHaulierField = value;
                }
            }

            /// <remarks/>
            public PredefinedCautionsDescriptionsStructure1 PredefinedCautions
            {
                get
                {
                    return this.predefinedCautionsField;
                }
                set
                {
                    this.predefinedCautionsField = value;
                }
            }

            /// <remarks/>
            public ComplexTextStructure NotesForHaulier
            {
                get
                {
                    return this.notesForHaulierField;
                }
                set
                {
                    this.notesForHaulierField = value;
                }
            }

            /// <remarks/>
            public SpecialOrderRouteStructureOldNotesForHaulier OldNotesForHaulier
            {
                get
                {
                    return this.oldNotesForHaulierField;
                }
                set
                {
                    this.oldNotesForHaulierField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsRedlined
            {
                get
                {
                    return this.isRedlinedField;
                }
                set
                {
                    this.isRedlinedField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldJobFileReference
        {

            private string jobFileReferenceField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string JobFileReference
            {
                get
                {
                    return this.jobFileReferenceField;
                }
                set
                {
                    this.jobFileReferenceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class HAContactStructure
        {

            private string contactField;

            private AddressStructure addressField;

            private string telephoneNumberField;

            private string faxNumberField;

            private string emailAddressField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            public AddressStructure Address
            {
                get
                {
                    return this.addressField;
                }
                set
                {
                    this.addressField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            public string FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldHAContact
        {

            private HAContactStructure hAContactField;

            /// <remarks/>
            public HAContactStructure HAContact
            {
                get
                {
                    return this.hAContactField;
                }
                set
                {
                    this.hAContactField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class HaulierDetailsStructure
        {

            private string haulierContactField;

            private string haulierNameField;

            private AddressStructure haulierAddressField;

            private string telephoneNumberField;

            private string faxNumberField;

            private string emailAddressField;

            private string licenceField;

            private string organisationIdField;

            private bool organisationIdFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string HaulierContact
            {
                get
                {
                    return this.haulierContactField;
                }
                set
                {
                    this.haulierContactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string HaulierName
            {
                get
                {
                    return this.haulierNameField;
                }
                set
                {
                    this.haulierNameField = value;
                }
            }

            /// <remarks/>
            public AddressStructure HaulierAddress
            {
                get
                {
                    return this.haulierAddressField;
                }
                set
                {
                    this.haulierAddressField = value;
                }
            }

            /// <remarks/>
            public string TelephoneNumber
            {
                get
                {
                    return this.telephoneNumberField;
                }
                set
                {
                    this.telephoneNumberField = value;
                }
            }

            /// <remarks/>
            public string FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Licence
            {
                get
                {
                    return this.licenceField;
                }
                set
                {
                    this.licenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationId
            {
                get
                {
                    return this.organisationIdField;
                }
                set
                {
                    this.organisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIdSpecified
            {
                get
                {
                    return this.organisationIdFieldSpecified;
                }
                set
                {
                    this.organisationIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldHaulierDetails
        {

            private HaulierDetailsStructure haulierDetailsField;

            /// <remarks/>
            public HaulierDetailsStructure HaulierDetails
            {
                get
                {
                    return this.haulierDetailsField;
                }
                set
                {
                    this.haulierDetailsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldHauliersReference
        {

            private string hauliersReferenceField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string HauliersReference
            {
                get
                {
                    return this.hauliersReferenceField;
                }
                set
                {
                    this.hauliersReferenceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class JourneyFromToSummaryStructure
        {

            private string fromField;

            private string toField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string From
            {
                get
                {
                    return this.fromField;
                }
                set
                {
                    this.fromField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string To
            {
                get
                {
                    return this.toField;
                }
                set
                {
                    this.toField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldJourneyFromToSummary
        {

            private JourneyFromToSummaryStructure journeyFromToSummaryField;

            /// <remarks/>
            public JourneyFromToSummaryStructure JourneyFromToSummary
            {
                get
                {
                    return this.journeyFromToSummaryField;
                }
                set
                {
                    this.journeyFromToSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class JourneyFromToStructure
        {

            private string fromField;

            private JourneyFromToStructureOldFrom oldFromField;

            private string toField;

            private JourneyFromToStructureOldTo oldToField;

            /// <remarks/>
            public string From
            {
                get
                {
                    return this.fromField;
                }
                set
                {
                    this.fromField = value;
                }
            }

            /// <remarks/>
            public JourneyFromToStructureOldFrom OldFrom
            {
                get
                {
                    return this.oldFromField;
                }
                set
                {
                    this.oldFromField = value;
                }
            }

            /// <remarks/>
            public string To
            {
                get
                {
                    return this.toField;
                }
                set
                {
                    this.toField = value;
                }
            }

            /// <remarks/>
            public JourneyFromToStructureOldTo OldTo
            {
                get
                {
                    return this.oldToField;
                }
                set
                {
                    this.oldToField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class JourneyFromToStructureOldFrom
        {

            private string fromField;

            /// <remarks/>
            public string From
            {
                get
                {
                    return this.fromField;
                }
                set
                {
                    this.fromField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class JourneyFromToStructureOldTo
        {

            private string toField;

            /// <remarks/>
            public string To
            {
                get
                {
                    return this.toField;
                }
                set
                {
                    this.toField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class JourneyDateStructure
        {

            private string firstMoveDateField;

            private string lastMoveDateField;

            private bool lastMoveDateFieldSpecified;

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string FirstMoveDate
            {
                get
                {
                    return this.firstMoveDateField;
                }
                set
                {
                    this.firstMoveDateField = value;
                }
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public string LastMoveDate
            {
                get
                {
                    return this.lastMoveDateField;
                }
                set
                {
                    this.lastMoveDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LastMoveDateSpecified
            {
                get
                {
                    return this.lastMoveDateFieldSpecified;
                }
                set
                {
                    this.lastMoveDateFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldJourneyTiming
        {

            private JourneyDateStructure journeyTimingField;

            /// <remarks/>
            public JourneyDateStructure JourneyTiming
            {
                get
                {
                    return this.journeyTimingField;
                }
                set
                {
                    this.journeyTimingField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldLoadSummary
        {

            private string loadSummaryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string LoadSummary
            {
                get
                {
                    return this.loadSummaryField;
                }
                set
                {
                    this.loadSummaryField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class LoadDetailsStructure
        {

            private string descriptionField;

            private LoadDetailsStructureOldDescription oldDescriptionField;

            private string totalMovesField;

            private LoadDetailsStructureOldTotalMoves oldTotalMovesField;

            private string maxPiecesPerMoveField;

            private bool maxPiecesPerMoveFieldSpecified;

            private LoadDetailsStructureOldMaxPiecesPerMove oldMaxPiecesPerMoveField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public LoadDetailsStructureOldDescription OldDescription
            {
                get
                {
                    return this.oldDescriptionField;
                }
                set
                {
                    this.oldDescriptionField = value;
                }
            }

            /// <remarks/>
            public string TotalMoves
            {
                get
                {
                    return this.totalMovesField;
                }
                set
                {
                    this.totalMovesField = value;
                }
            }

            /// <remarks/>
            public LoadDetailsStructureOldTotalMoves OldTotalMoves
            {
                get
                {
                    return this.oldTotalMovesField;
                }
                set
                {
                    this.oldTotalMovesField = value;
                }
            }

            /// <remarks/>
            public string MaxPiecesPerMove
            {
                get
                {
                    return this.maxPiecesPerMoveField;
                }
                set
                {
                    this.maxPiecesPerMoveField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaxPiecesPerMoveSpecified
            {
                get
                {
                    return this.maxPiecesPerMoveFieldSpecified;
                }
                set
                {
                    this.maxPiecesPerMoveFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LoadDetailsStructureOldMaxPiecesPerMove OldMaxPiecesPerMove
            {
                get
                {
                    return this.oldMaxPiecesPerMoveField;
                }
                set
                {
                    this.oldMaxPiecesPerMoveField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class LoadDetailsStructureOldDescription
        {

            private string descriptionField;

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class LoadDetailsStructureOldTotalMoves
        {

            private string totalMovesField;

            /// <remarks/>
            public string TotalMoves
            {
                get
                {
                    return this.totalMovesField;
                }
                set
                {
                    this.totalMovesField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class LoadDetailsStructureOldMaxPiecesPerMove
        {

            private string maxPiecesPerMoveField;

            private bool maxPiecesPerMoveFieldSpecified;

            /// <remarks/>
            public string MaxPiecesPerMove
            {
                get
                {
                    return this.maxPiecesPerMoveField;
                }
                set
                {
                    this.maxPiecesPerMoveField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaxPiecesPerMoveSpecified
            {
                get
                {
                    return this.maxPiecesPerMoveFieldSpecified;
                }
                set
                {
                    this.maxPiecesPerMoveFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldLoadDetails
        {

            private LoadDetailsStructure loadDetailsField;

            /// <remarks/>
            public LoadDetailsStructure LoadDetails
            {
                get
                {
                    return this.loadDetailsField;
                }
                set
                {
                    this.loadDetailsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsStructureRoutePartListPosition
        {

            private PlannedRoutePartStructure2 routePartField;

            private RoutePartsStructureRoutePartListPositionOldRoutePart oldRoutePartField;

            /// <remarks/>
            public PlannedRoutePartStructure2 RoutePart
            {
                get
                {
                    return this.routePartField;
                }
                set
                {
                    this.routePartField = value;
                }
            }

            /// <remarks/>
            public RoutePartsStructureRoutePartListPositionOldRoutePart OldRoutePart
            {
                get
                {
                    return this.oldRoutePartField;
                }
                set
                {
                    this.oldRoutePartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(TypeName = "PlannedRoutePartStructure", Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoutePartStructure2
        {

            private string legNumberField;

            private string nameField;

            private PlannedRoutePartStructureOldName oldNameField;

            private PlannedRoadRoutePartStructure roadPartField;

            private PlannedRoutePartStructureOldRoadPart oldRoadPartField;

            private ModeOfTransportType modeField;

            private PlannedRoutePartStructureOldMode oldModeField;

            private string idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string LegNumber
            {
                get
                {
                    return this.legNumberField;
                }
                set
                {
                    this.legNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public PlannedRoutePartStructureOldName OldName
            {
                get
                {
                    return this.oldNameField;
                }
                set
                {
                    this.oldNameField = value;
                }
            }

            /// <remarks/>
            public PlannedRoadRoutePartStructure RoadPart
            {
                get
                {
                    return this.roadPartField;
                }
                set
                {
                    this.roadPartField = value;
                }
            }

            /// <remarks/>
            public PlannedRoutePartStructureOldRoadPart OldRoadPart
            {
                get
                {
                    return this.oldRoadPartField;
                }
                set
                {
                    this.oldRoadPartField = value;
                }
            }

            /// <remarks/>
            public ModeOfTransportType Mode
            {
                get
                {
                    return this.modeField;
                }
                set
                {
                    this.modeField = value;
                }
            }

            /// <remarks/>
            public PlannedRoutePartStructureOldMode OldMode
            {
                get
                {
                    return this.oldModeField;
                }
                set
                {
                    this.oldModeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoutePartStructureOldName
        {

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructure
        {

            private PlannedRoadRoutePartStructureStartPointListPosition[] startPointListPositionField;

            private PlannedRoadRoutePartStructureEndPointListPosition[] endPointListPositionField;

            private VariableMetricImperialDistancePairStructure distanceField;

            private PlannedRoadRoutePartStructureOldDistance oldDistanceField;

            private VehiclesSummaryStructure vehiclesField;

            private PlannedRoadRoutePartStructureOldVehicles oldVehiclesField;

            private AffectedRoadsStructure roadsField;

            private PlannedRoadRoutePartStructureOldRoads oldRoadsField;

            private AffectedStructuresStructure structuresField;

            private DrivingInstructionsStructure drivingInstructionsField;

            private PlannedRoadRoutePartStructureOldDrivingInstructions oldDrivingInstructionsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("StartPointListPosition")]
            public PlannedRoadRoutePartStructureStartPointListPosition[] StartPointListPosition
            {
                get
                {
                    return this.startPointListPositionField;
                }
                set
                {
                    this.startPointListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("EndPointListPosition")]
            public PlannedRoadRoutePartStructureEndPointListPosition[] EndPointListPosition
            {
                get
                {
                    return this.endPointListPositionField;
                }
                set
                {
                    this.endPointListPositionField = value;
                }
            }

            /// <remarks/>
            public VariableMetricImperialDistancePairStructure Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            public PlannedRoadRoutePartStructureOldDistance OldDistance
            {
                get
                {
                    return this.oldDistanceField;
                }
                set
                {
                    this.oldDistanceField = value;
                }
            }

            /// <remarks/>
            public VehiclesSummaryStructure Vehicles
            {
                get
                {
                    return this.vehiclesField;
                }
                set
                {
                    this.vehiclesField = value;
                }
            }

            /// <remarks/>
            public PlannedRoadRoutePartStructureOldVehicles OldVehicles
            {
                get
                {
                    return this.oldVehiclesField;
                }
                set
                {
                    this.oldVehiclesField = value;
                }
            }

            /// <remarks/>
            public AffectedRoadsStructure Roads
            {
                get
                {
                    return this.roadsField;
                }
                set
                {
                    this.roadsField = value;
                }
            }

            /// <remarks/>
            public PlannedRoadRoutePartStructureOldRoads OldRoads
            {
                get
                {
                    return this.oldRoadsField;
                }
                set
                {
                    this.oldRoadsField = value;
                }
            }

            /// <remarks/>
            public AffectedStructuresStructure Structures
            {
                get
                {
                    return this.structuresField;
                }
                set
                {
                    this.structuresField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
            public DrivingInstructionsStructure DrivingInstructions
            {
                get
                {
                    return this.drivingInstructionsField;
                }
                set
                {
                    this.drivingInstructionsField = value;
                }
            }

            /// <remarks/>
            public PlannedRoadRoutePartStructureOldDrivingInstructions OldDrivingInstructions
            {
                get
                {
                    return this.oldDrivingInstructionsField;
                }
                set
                {
                    this.oldDrivingInstructionsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureStartPointListPosition
        {

            private SimplifiedRoutePointStructure startPointField;

            private PlannedRoadRoutePartStructureStartPointListPositionOldStartPoint oldStartPointField;

            /// <remarks/>
            public SimplifiedRoutePointStructure StartPoint
            {
                get
                {
                    return this.startPointField;
                }
                set
                {
                    this.startPointField = value;
                }
            }

            /// <remarks/>
            public PlannedRoadRoutePartStructureStartPointListPositionOldStartPoint OldStartPoint
            {
                get
                {
                    return this.oldStartPointField;
                }
                set
                {
                    this.oldStartPointField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureStartPointListPositionOldStartPoint
        {

            private SimplifiedRoutePointStructure startPointField;

            /// <remarks/>
            public SimplifiedRoutePointStructure StartPoint
            {
                get
                {
                    return this.startPointField;
                }
                set
                {
                    this.startPointField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureEndPointListPosition
        {

            private SimplifiedRoutePointStructure endPointField;

            private PlannedRoadRoutePartStructureEndPointListPositionOldEndPoint oldEndPointField;

            /// <remarks/>
            public SimplifiedRoutePointStructure EndPoint
            {
                get
                {
                    return this.endPointField;
                }
                set
                {
                    this.endPointField = value;
                }
            }

            /// <remarks/>
            public PlannedRoadRoutePartStructureEndPointListPositionOldEndPoint OldEndPoint
            {
                get
                {
                    return this.oldEndPointField;
                }
                set
                {
                    this.oldEndPointField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureEndPointListPositionOldEndPoint
        {

            private SimplifiedRoutePointStructure endPointField;

            /// <remarks/>
            public SimplifiedRoutePointStructure EndPoint
            {
                get
                {
                    return this.endPointField;
                }
                set
                {
                    this.endPointField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class VariableMetricImperialDistancePairStructure
        {

            private VariableMetricDistanceStructure metricField;

            private VariableImperialDistanceStructure imperialField;

            /// <remarks/>
            public VariableMetricDistanceStructure Metric
            {
                get
                {
                    return this.metricField;
                }
                set
                {
                    this.metricField = value;
                }
            }

            /// <remarks/>
            public VariableImperialDistanceStructure Imperial
            {
                get
                {
                    return this.imperialField;
                }
                set
                {
                    this.imperialField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class VariableMetricDistanceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Distance", typeof(string), DataType = "integer")]
            [System.Xml.Serialization.XmlElementAttribute("DistanceRange", typeof(VariableMetricDistanceStructureDistanceRange))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class VariableMetricDistanceStructureDistanceRange
        {

            private string minDistanceField;

            private string maxDistanceField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string MinDistance
            {
                get
                {
                    return this.minDistanceField;
                }
                set
                {
                    this.minDistanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string MaxDistance
            {
                get
                {
                    return this.maxDistanceField;
                }
                set
                {
                    this.maxDistanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class VariableImperialDistanceStructure
        {

            private object itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Distance", typeof(string), DataType = "integer")]
            [System.Xml.Serialization.XmlElementAttribute("DistanceRange", typeof(VariableImperialDistanceStructureDistanceRange))]
            public object Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class VariableImperialDistanceStructureDistanceRange
        {

            private string minDistanceField;

            private string maxDistanceField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string MinDistance
            {
                get
                {
                    return this.minDistanceField;
                }
                set
                {
                    this.minDistanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string MaxDistance
            {
                get
                {
                    return this.maxDistanceField;
                }
                set
                {
                    this.maxDistanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureOldDistance
        {

            private VariableMetricImperialDistancePairStructure distanceField;

            /// <remarks/>
            public VariableMetricImperialDistancePairStructure Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureOldVehicles
        {

            private VehiclesSummaryStructure vehiclesField;

            /// <remarks/>
            public VehiclesSummaryStructure Vehicles
            {
                get
                {
                    return this.vehiclesField;
                }
                set
                {
                    this.vehiclesField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsStructure
        {

            private AffectedRoadsStructureRouteSubPartListPosition[] routeSubPartListPositionField;

            private string organisationIDField;

            private bool organisationIDFieldSpecified;

            private string organisationNameField;

            private bool isBrokenField;

            public AffectedRoadsStructure()
            {
                this.isBrokenField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("RouteSubPartListPosition")]
            public AffectedRoadsStructureRouteSubPartListPosition[] RouteSubPartListPosition
            {
                get
                {
                    return this.routeSubPartListPositionField;
                }
                set
                {
                    this.routeSubPartListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationID
            {
                get
                {
                    return this.organisationIDField;
                }
                set
                {
                    this.organisationIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIDSpecified
            {
                get
                {
                    return this.organisationIDFieldSpecified;
                }
                set
                {
                    this.organisationIDFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsStructureRouteSubPartListPosition
        {

            private AffectedRoadsSubPartStructure routeSubPartField;

            private AffectedRoadsStructureRouteSubPartListPositionOldRouteSubPart oldRouteSubPartField;

            /// <remarks/>
            public AffectedRoadsSubPartStructure RouteSubPart
            {
                get
                {
                    return this.routeSubPartField;
                }
                set
                {
                    this.routeSubPartField = value;
                }
            }

            /// <remarks/>
            public AffectedRoadsStructureRouteSubPartListPositionOldRouteSubPart OldRouteSubPart
            {
                get
                {
                    return this.oldRouteSubPartField;
                }
                set
                {
                    this.oldRouteSubPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsSubPartStructure
        {

            private AffectedRoadsSubPartStructurePathListPosition[] pathListPositionField;

            private bool isBrokenField;

            public AffectedRoadsSubPartStructure()
            {
                this.isBrokenField = false;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PathListPosition")]
            public AffectedRoadsSubPartStructurePathListPosition[] PathListPosition
            {
                get
                {
                    return this.pathListPositionField;
                }
                set
                {
                    this.pathListPositionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsSubPartStructurePathListPosition
        {

            private AffectedRoadsPathStructureRoadTraversalListPosition[] pathField;

            private AffectedRoadsSubPartStructurePathListPositionOldPath oldPathField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoadTraversalListPosition", IsNullable = false)]
            public AffectedRoadsPathStructureRoadTraversalListPosition[] Path
            {
                get
                {
                    return this.pathField;
                }
                set
                {
                    this.pathField = value;
                }
            }

            /// <remarks/>
            public AffectedRoadsSubPartStructurePathListPositionOldPath OldPath
            {
                get
                {
                    return this.oldPathField;
                }
                set
                {
                    this.oldPathField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsPathStructureRoadTraversalListPosition
        {

            private AffectedRoadStructure roadTraversalField;

            private AffectedRoadsPathStructureRoadTraversalListPositionOldRoadTraversal oldRoadTraversalField;

            /// <remarks/>
            public AffectedRoadStructure RoadTraversal
            {
                get
                {
                    return this.roadTraversalField;
                }
                set
                {
                    this.roadTraversalField = value;
                }
            }

            /// <remarks/>
            public AffectedRoadsPathStructureRoadTraversalListPositionOldRoadTraversal OldRoadTraversal
            {
                get
                {
                    return this.oldRoadTraversalField;
                }
                set
                {
                    this.oldRoadTraversalField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadStructure
        {

            private RoadIdentificationStructure roadIdentityField;

            private MetricImperialDistancePairStructure distanceField;

            private MetricImperialDistancePairStructure contiguousRoadDistanceField;

            private AffectedRoadConstraintStructure[] constraintsField;

            private bool isMyResponsibilityField;

            private bool isStartOfMyResponsibilityField;

            private bool isEndOfMyResponsibilityField;

            private string delegatorsOrganisationIdField;

            private bool delegatorsOrganisationIdFieldSpecified;

            private string delegatorsContactIdField;

            private bool delegatorsContactIdFieldSpecified;

            private bool retainNotificationField;

            private bool wantsFailureAlertField;

            private string delegationFromIdField;

            private bool delegationFromIdFieldSpecified;

            private string delegationToIdField;

            private bool delegationToIdFieldSpecified;

            private bool isBrokenField;

            public AffectedRoadStructure()
            {
                this.isMyResponsibilityField = false;
                this.isStartOfMyResponsibilityField = false;
                this.isEndOfMyResponsibilityField = false;
                this.retainNotificationField = false;
                this.wantsFailureAlertField = false;
                this.isBrokenField = false;
            }

            /// <remarks/>
            public RoadIdentificationStructure RoadIdentity
            {
                get
                {
                    return this.roadIdentityField;
                }
                set
                {
                    this.roadIdentityField = value;
                }
            }

            /// <remarks/>
            public MetricImperialDistancePairStructure Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            public MetricImperialDistancePairStructure ContiguousRoadDistance
            {
                get
                {
                    return this.contiguousRoadDistanceField;
                }
                set
                {
                    this.contiguousRoadDistanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Constraint", IsNullable = false)]
            public AffectedRoadConstraintStructure[] Constraints
            {
                get
                {
                    return this.constraintsField;
                }
                set
                {
                    this.constraintsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsMyResponsibility
            {
                get
                {
                    return this.isMyResponsibilityField;
                }
                set
                {
                    this.isMyResponsibilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsStartOfMyResponsibility
            {
                get
                {
                    return this.isStartOfMyResponsibilityField;
                }
                set
                {
                    this.isStartOfMyResponsibilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsEndOfMyResponsibility
            {
                get
                {
                    return this.isEndOfMyResponsibilityField;
                }
                set
                {
                    this.isEndOfMyResponsibilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegatorsOrganisationId
            {
                get
                {
                    return this.delegatorsOrganisationIdField;
                }
                set
                {
                    this.delegatorsOrganisationIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegatorsOrganisationIdSpecified
            {
                get
                {
                    return this.delegatorsOrganisationIdFieldSpecified;
                }
                set
                {
                    this.delegatorsOrganisationIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegatorsContactId
            {
                get
                {
                    return this.delegatorsContactIdField;
                }
                set
                {
                    this.delegatorsContactIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegatorsContactIdSpecified
            {
                get
                {
                    return this.delegatorsContactIdFieldSpecified;
                }
                set
                {
                    this.delegatorsContactIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool RetainNotification
            {
                get
                {
                    return this.retainNotificationField;
                }
                set
                {
                    this.retainNotificationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool WantsFailureAlert
            {
                get
                {
                    return this.wantsFailureAlertField;
                }
                set
                {
                    this.wantsFailureAlertField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegationFromId
            {
                get
                {
                    return this.delegationFromIdField;
                }
                set
                {
                    this.delegationFromIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegationFromIdSpecified
            {
                get
                {
                    return this.delegationFromIdFieldSpecified;
                }
                set
                {
                    this.delegationFromIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegationToId
            {
                get
                {
                    return this.delegationToIdField;
                }
                set
                {
                    this.delegationToIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegationToIdSpecified
            {
                get
                {
                    return this.delegationToIdFieldSpecified;
                }
                set
                {
                    this.delegationToIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsBroken
            {
                get
                {
                    return this.isBrokenField;
                }
                set
                {
                    this.isBrokenField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class MetricImperialDistancePairStructure
        {

            private string metricField;

            private string imperialField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string Metric
            {
                get
                {
                    return this.metricField;
                }
                set
                {
                    this.metricField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string Imperial
            {
                get
                {
                    return this.imperialField;
                }
                set
                {
                    this.imperialField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsPathStructureRoadTraversalListPositionOldRoadTraversal
        {

            private AffectedRoadStructure roadTraversalField;

            /// <remarks/>
            public AffectedRoadStructure RoadTraversal
            {
                get
                {
                    return this.roadTraversalField;
                }
                set
                {
                    this.roadTraversalField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsSubPartStructurePathListPositionOldPath
        {

            private AffectedRoadsPathStructureRoadTraversalListPosition[] pathField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoadTraversalListPosition", IsNullable = false)]
            public AffectedRoadsPathStructureRoadTraversalListPosition[] Path
            {
                get
                {
                    return this.pathField;
                }
                set
                {
                    this.pathField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedRoadsStructureRouteSubPartListPositionOldRouteSubPart
        {

            private AffectedRoadsSubPartStructure routeSubPartField;

            /// <remarks/>
            public AffectedRoadsSubPartStructure RouteSubPart
            {
                get
                {
                    return this.routeSubPartField;
                }
                set
                {
                    this.routeSubPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureOldRoads
        {

            private AffectedRoadsStructure roadsField;

            /// <remarks/>
            public AffectedRoadsStructure Roads
            {
                get
                {
                    return this.roadsField;
                }
                set
                {
                    this.roadsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedStructuresStructure
        {

            private AffectedStructureStructure1[] structureField;

            private bool areMyResponsibilityOnlyField;

            private string organisationIDField;

            private bool organisationIDFieldSpecified;

            private string organisationNameField;

            private bool isStructureOwnerField;

            public AffectedStructuresStructure()
            {
                this.areMyResponsibilityOnlyField = true;
                this.isStructureOwnerField = true;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Structure")]
            public AffectedStructureStructure1[] Structure
            {
                get
                {
                    return this.structureField;
                }
                set
                {
                    this.structureField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(true)]
            public bool AreMyResponsibilityOnly
            {
                get
                {
                    return this.areMyResponsibilityOnlyField;
                }
                set
                {
                    this.areMyResponsibilityOnlyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganisationID
            {
                get
                {
                    return this.organisationIDField;
                }
                set
                {
                    this.organisationIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OrganisationIDSpecified
            {
                get
                {
                    return this.organisationIDFieldSpecified;
                }
                set
                {
                    this.organisationIDFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
            public string OrganisationName
            {
                get
                {
                    return this.organisationNameField;
                }
                set
                {
                    this.organisationNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(true)]
            public bool IsStructureOwner
            {
                get
                {
                    return this.isStructureOwnerField;
                }
                set
                {
                    this.isStructureOwnerField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(TypeName = "AffectedStructureStructure", Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class AffectedStructureStructure1
        {

            private string eSRNField;

            private string nameField;

            private OnBehalfOfStructure[] onBehalfOfField;

            private StructureSuitabilityStructure appraisalField;

            private AffectedStructureReasonType reasonField;

            private bool reasonFieldSpecified;

            private bool isRetainNotificationOnlyField;

            private bool isMyResponsibilityField;

            private StructureTraversalType traversalTypeField;

            private bool isInFailedDelegationField;

            private bool isConstrainedField;

            private string structureSectionIdField;

            private bool structureSectionIdFieldSpecified;

            private string delegationToIdField;

            private bool delegationToIdFieldSpecified;

            public AffectedStructureStructure1()
            {
                this.isRetainNotificationOnlyField = false;
                this.isMyResponsibilityField = true;
                // this.traversalTypeField = StructureTraversalType.underbridge;
                this.isInFailedDelegationField = false;
                this.isConstrainedField = false;
            }

            /// <remarks/>
            public string ESRN
            {
                get
                {
                    return this.eSRNField;
                }
                set
                {
                    this.eSRNField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OnBehalfOf")]
            public OnBehalfOfStructure[] OnBehalfOf
            {
                get
                {
                    return this.onBehalfOfField;
                }
                set
                {
                    this.onBehalfOfField = value;
                }
            }

            /// <remarks/>
            public StructureSuitabilityStructure Appraisal
            {
                get
                {
                    return this.appraisalField;
                }
                set
                {
                    this.appraisalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public AffectedStructureReasonType Reason
            {
                get
                {
                    return this.reasonField;
                }
                set
                {
                    this.reasonField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReasonSpecified
            {
                get
                {
                    return this.reasonFieldSpecified;
                }
                set
                {
                    this.reasonFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsRetainNotificationOnly
            {
                get
                {
                    return this.isRetainNotificationOnlyField;
                }
                set
                {
                    this.isRetainNotificationOnlyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(true)]
            public bool IsMyResponsibility
            {
                get
                {
                    return this.isMyResponsibilityField;
                }
                set
                {
                    this.isMyResponsibilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(StructureTraversalType.underbridge)]
            public StructureTraversalType TraversalType
            {
                get
                {
                    return this.traversalTypeField;
                }
                set
                {
                    this.traversalTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsInFailedDelegation
            {
                get
                {
                    return this.isInFailedDelegationField;
                }
                set
                {
                    this.isInFailedDelegationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            [System.ComponentModel.DefaultValueAttribute(false)]
            public bool IsConstrained
            {
                get
                {
                    return this.isConstrainedField;
                }
                set
                {
                    this.isConstrainedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string StructureSectionId
            {
                get
                {
                    return this.structureSectionIdField;
                }
                set
                {
                    this.structureSectionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool StructureSectionIdSpecified
            {
                get
                {
                    return this.structureSectionIdFieldSpecified;
                }
                set
                {
                    this.structureSectionIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DelegationToId
            {
                get
                {
                    return this.delegationToIdField;
                }
                set
                {
                    this.delegationToIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DelegationToIdSpecified
            {
                get
                {
                    return this.delegationToIdFieldSpecified;
                }
                set
                {
                    this.delegationToIdFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/movement")]
        public enum AffectedStructureReasonType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("newly affected")]
            newlyaffected,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("no longer affected")]
            nolongeraffected,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("still affected")]
            stillaffected,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoadRoutePartStructureOldDrivingInstructions
        {

            private DrivingInstructionsStructure drivingInstructionsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/drivinginstruction")]
            public DrivingInstructionsStructure DrivingInstructions
            {
                get
                {
                    return this.drivingInstructionsField;
                }
                set
                {
                    this.drivingInstructionsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoutePartStructureOldRoadPart
        {

            private PlannedRoadRoutePartStructure roadPartField;

            /// <remarks/>
            public PlannedRoadRoutePartStructure RoadPart
            {
                get
                {
                    return this.roadPartField;
                }
                set
                {
                    this.roadPartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class PlannedRoutePartStructureOldMode
        {

            private ModeOfTransportType modeField;

            /// <remarks/>
            public ModeOfTransportType Mode
            {
                get
                {
                    return this.modeField;
                }
                set
                {
                    this.modeField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class RoutePartsStructureRoutePartListPositionOldRoutePart
        {

            private PlannedRoutePartStructure2 routePartField;

            /// <remarks/>
            public PlannedRoutePartStructure2 RoutePart
            {
                get
                {
                    return this.routePartField;
                }
                set
                {
                    this.routePartField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldRouteParts
        {

            private RoutePartsStructureRoutePartListPosition[] routePartsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RoutePartListPosition", IsNullable = false)]
            public RoutePartsStructureRoutePartListPosition[] RouteParts
            {
                get
                {
                    return this.routePartsField;
                }
                set
                {
                    this.routePartsField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldNotesFromHaulier
        {

            private string notesFromHaulierField;

            /// <remarks/>
            public string NotesFromHaulier
            {
                get
                {
                    return this.notesFromHaulierField;
                }
                set
                {
                    this.notesFromHaulierField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public partial class ComplexTextStructure : LevelZeroTextStructure
        {
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComplexTextStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public partial class LevelZeroTextStructure
        {

            private object[] itemsField;

            private ItemsChoiceType6[] itemsElementNameField;

            private string[] textField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Bold", typeof(LevelZeroTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("BulletedText", typeof(LevelOneTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Italic", typeof(LevelZeroTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("LetteredText", typeof(LevelOneLetteredStructure))]
            [System.Xml.Serialization.XmlElementAttribute("NumberedText", typeof(LevelOneNumberedStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Underscore", typeof(LevelZeroTextStructure))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
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
            [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemsChoiceType6[] ItemsElementName
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
            [System.Xml.Serialization.XmlTextAttribute()]
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
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelOneLetteredStructure))]
        [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelOneNumberedStructure))]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public partial class LevelOneTextStructure
        {

            private object[] itemsField;

            private ItemsChoiceType5[] itemsElementNameField;

            private string[] textField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Bold", typeof(LevelOneTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("BulletedText", typeof(LevelTwoTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Italic", typeof(LevelOneTextStructure))]
            [System.Xml.Serialization.XmlElementAttribute("LetteredText", typeof(LevelTwoLetteredStructure))]
            [System.Xml.Serialization.XmlElementAttribute("NumberedText", typeof(LevelTwoNumberedStructure))]
            [System.Xml.Serialization.XmlElementAttribute("Underscore", typeof(LevelOneTextStructure))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
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
            [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemsChoiceType5[] ItemsElementName
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
            [System.Xml.Serialization.XmlTextAttribute()]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IncludeInSchema = false)]
        public enum ItemsChoiceType5
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public abstract partial class LevelOneLetteredStructure : LevelOneTextStructure
        {

            private string letterField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public abstract partial class LevelOneNumberedStructure : LevelOneTextStructure
        {

            private string numberField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.esdal.com/schemas/core/formattedtext", IncludeInSchema = false)]
        public enum ItemsChoiceType6
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/movement")]
        public partial class SpecialOrderRouteStructureOldNotesForHaulier
        {

            private ComplexTextStructure notesForHaulierField;

            /// <remarks/>
            public ComplexTextStructure NotesForHaulier
            {
                get
                {
                    return this.notesForHaulierField;
                }
                set
                {
                    this.notesForHaulierField = value;
                }
            }
        }
    }

}
