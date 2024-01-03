using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class VehicleGlobalConfig
    {
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class VehicleComponentConfiguration
        {

            private VehicleComponentConfigurationClassification[] movementClassificationField;

            private VehicleComponentConfigurationComponentType[] vehicleComponentTypeField;

            private VehicleComponentConfigurationSubComponentType[] vehicleSubComponentTypeField;

            private VehicleComponentConfigurationVehicleConfigurationTypes vehicleConfigurationTypesField;

            private VehicleComponentConfigurationMovementClassificationID[] vehicleComponentsField;

            private VehicleComponentConfigurationVehicleConfiguration[] vehicleConfigurationsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Classification", IsNullable = false)]
            public VehicleComponentConfigurationClassification[] MovementClassification
            {
                get
                {
                    return this.movementClassificationField;
                }
                set
                {
                    this.movementClassificationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ComponentType", IsNullable = false)]
            public VehicleComponentConfigurationComponentType[] VehicleComponentType
            {
                get
                {
                    return this.vehicleComponentTypeField;
                }
                set
                {
                    this.vehicleComponentTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("SubComponentType", IsNullable = false)]
            public VehicleComponentConfigurationSubComponentType[] VehicleSubComponentType
            {
                get
                {
                    return this.vehicleSubComponentTypeField;
                }
                set
                {
                    this.vehicleSubComponentTypeField = value;
                }
            }

            /// <remarks/>
            public VehicleComponentConfigurationVehicleConfigurationTypes VehicleConfigurationTypes
            {
                get
                {
                    return this.vehicleConfigurationTypesField;
                }
                set
                {
                    this.vehicleConfigurationTypesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("MovementClassificationID", IsNullable = false)]
            public VehicleComponentConfigurationMovementClassificationID[] VehicleComponents
            {
                get
                {
                    return this.vehicleComponentsField;
                }
                set
                {
                    this.vehicleComponentsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("VehicleConfiguration", IsNullable = false)]
            public VehicleComponentConfigurationVehicleConfiguration[] VehicleConfigurations
            {
                get
                {
                    return this.vehicleConfigurationsField;
                }
                set
                {
                    this.vehicleConfigurationsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationClassification
        {

            private uint idField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint id
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationComponentType
        {

            private uint idField;

            private bool tractorField;

            private string imgField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint id
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
            public bool Tractor
            {
                get
                {
                    return this.tractorField;
                }
                set
                {
                    this.tractorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string img
            {
                get
                {
                    return this.imgField;
                }
                set
                {
                    this.imgField = value;
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationSubComponentType
        {

            private string imageField;

            private string subcompNameField;

            private VehicleComponentConfigurationSubComponentTypeAxleConfig axleConfigField;

            private uint idField;

            /// <remarks/>
            public string image
            {
                get
                {
                    return this.imageField;
                }
                set
                {
                    this.imageField = value;
                }
            }

            /// <remarks/>
            public string subcompName
            {
                get
                {
                    return this.subcompNameField;
                }
                set
                {
                    this.subcompNameField = value;
                }
            }

            /// <remarks/>
            public VehicleComponentConfigurationSubComponentTypeAxleConfig AxleConfig
            {
                get
                {
                    return this.axleConfigField;
                }
                set
                {
                    this.axleConfigField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint id
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationSubComponentTypeAxleConfig
        {

            private byte maxWheelsField;

            private ushort minWeightField;

            private ushort maxWeightField;

            private decimal minAxleSpacingField;

            private byte maxAxleSpacingField;

            private string minTyreCentreField;

            private string maxTyreCentreField;

            /// <remarks/>
            public byte MaxWheels
            {
                get
                {
                    return this.maxWheelsField;
                }
                set
                {
                    this.maxWheelsField = value;
                }
            }

            /// <remarks/>
            public ushort MinWeight
            {
                get
                {
                    return this.minWeightField;
                }
                set
                {
                    this.minWeightField = value;
                }
            }

            /// <remarks/>
            public ushort MaxWeight
            {
                get
                {
                    return this.maxWeightField;
                }
                set
                {
                    this.maxWeightField = value;
                }
            }

            /// <remarks/>
            public decimal MinAxleSpacing
            {
                get
                {
                    return this.minAxleSpacingField;
                }
                set
                {
                    this.minAxleSpacingField = value;
                }
            }

            /// <remarks/>
            public byte MaxAxleSpacing
            {
                get
                {
                    return this.maxAxleSpacingField;
                }
                set
                {
                    this.maxAxleSpacingField = value;
                }
            }

            /// <remarks/>
            public string MinTyreCentre
            {
                get
                {
                    return this.minTyreCentreField;
                }
                set
                {
                    this.minTyreCentreField = value;
                }
            }

            /// <remarks/>
            public string MaxTyreCentre
            {
                get
                {
                    return this.maxTyreCentreField;
                }
                set
                {
                    this.maxTyreCentreField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationVehicleConfigurationTypes
        {

            private byte maxTrailersinConfigField;

            private byte maxTractorsinConfigField;

            private VehicleComponentConfigurationVehicleConfigurationTypesConfiguration[] configurationField;

            /// <remarks/>
            public byte MaxTrailersinConfig
            {
                get
                {
                    return this.maxTrailersinConfigField;
                }
                set
                {
                    this.maxTrailersinConfigField = value;
                }
            }

            /// <remarks/>
            public byte MaxTractorsinConfig
            {
                get
                {
                    return this.maxTractorsinConfigField;
                }
                set
                {
                    this.maxTractorsinConfigField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Configuration")]
            public VehicleComponentConfigurationVehicleConfigurationTypesConfiguration[] Configuration
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationVehicleConfigurationTypesConfiguration
        {

            private string configNameField;

            private uint configIdField;

            private string componentTypeField;

            private string numberOfComponentsField;

            private bool onlyTractorInFrontField;

            private byte sideBySideRowsField;

            /// <remarks/>
            public string ConfigName
            {
                get
                {
                    return this.configNameField;
                }
                set
                {
                    this.configNameField = value;
                }
            }

            /// <remarks/>
            public uint ConfigId
            {
                get
                {
                    return this.configIdField;
                }
                set
                {
                    this.configIdField = value;
                }
            }

            /// <remarks/>
            public string ComponentType
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
            public string NumberOfComponents
            {
                get
                {
                    return this.numberOfComponentsField;
                }
                set
                {
                    this.numberOfComponentsField = value;
                }
            }

            /// <remarks/>
            public bool OnlyTractorInFront
            {
                get
                {
                    return this.onlyTractorInFrontField;
                }
                set
                {
                    this.onlyTractorInFrontField = value;
                }
            }

            /// <remarks/>
            public byte SideBySideRows
            {
                get
                {
                    return this.sideBySideRowsField;
                }
                set
                {
                    this.sideBySideRowsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationMovementClassificationID
        {

            private VehicleComponentConfigurationMovementClassificationIDVehicleComponent[] vehicleComponentField;

            private uint idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("VehicleComponent")]
            public VehicleComponentConfigurationMovementClassificationIDVehicleComponent[] VehicleComponent
            {
                get
                {
                    return this.vehicleComponentField;
                }
                set
                {
                    this.vehicleComponentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint id
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationMovementClassificationIDVehicleComponent
        {

            private uint componentTypeIDField;

            private string subComponentTypeidField;

            private bool axleConfigurationField;

            private VehicleComponentConfigurationMovementClassificationIDVehicleComponentAxleConfig axleConfigField;

            private bool configureTyreSpacingField;

            private bool configureTyreSpacingFieldSpecified;

            private VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNode[] paramNodeField;

            private string[] textField;

            /// <remarks/>
            public uint ComponentTypeID
            {
                get
                {
                    return this.componentTypeIDField;
                }
                set
                {
                    this.componentTypeIDField = value;
                }
            }

            /// <remarks/>
            public string SubComponentTypeid
            {
                get
                {
                    return this.subComponentTypeidField;
                }
                set
                {
                    this.subComponentTypeidField = value;
                }
            }

            /// <remarks/>
            public bool AxleConfiguration
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
            public VehicleComponentConfigurationMovementClassificationIDVehicleComponentAxleConfig AxleConfig
            {
                get
                {
                    return this.axleConfigField;
                }
                set
                {
                    this.axleConfigField = value;
                }
            }

            /// <remarks/>
            public bool ConfigureTyreSpacing
            {
                get
                {
                    return this.configureTyreSpacingField;
                }
                set
                {
                    this.configureTyreSpacingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ConfigureTyreSpacingSpecified
            {
                get
                {
                    return this.configureTyreSpacingFieldSpecified;
                }
                set
                {
                    this.configureTyreSpacingFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ParamNode")]
            public VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNode[] ParamNode
            {
                get
                {
                    return this.paramNodeField;
                }
                set
                {
                    this.paramNodeField = value;
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationMovementClassificationIDVehicleComponentAxleConfig
        {

            private byte maxWheelsField;

            private ushort minWeightField;

            private ushort maxWeightField;

            private decimal minAxleSpacingField;

            private byte maxAxleSpacingField;

            /// <remarks/>
            public byte MaxWheels
            {
                get
                {
                    return this.maxWheelsField;
                }
                set
                {
                    this.maxWheelsField = value;
                }
            }

            /// <remarks/>
            public ushort MinWeight
            {
                get
                {
                    return this.minWeightField;
                }
                set
                {
                    this.minWeightField = value;
                }
            }

            /// <remarks/>
            public ushort MaxWeight
            {
                get
                {
                    return this.maxWeightField;
                }
                set
                {
                    this.maxWeightField = value;
                }
            }

            /// <remarks/>
            public decimal MinAxleSpacing
            {
                get
                {
                    return this.minAxleSpacingField;
                }
                set
                {
                    this.minAxleSpacingField = value;
                }
            }

            /// <remarks/>
            public byte MaxAxleSpacing
            {
                get
                {
                    return this.maxAxleSpacingField;
                }
                set
                {
                    this.maxAxleSpacingField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNode
        {

            private object[] itemsField;

            private ItemsChoiceType[] itemsElementNameField;

            private string[] textField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("DisplayString", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("DropDownList", typeof(VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNodeDropDownList))]
            [System.Xml.Serialization.XmlElementAttribute("InputType", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("IsRequired", typeof(byte))]
            [System.Xml.Serialization.XmlElementAttribute("ParamMaxLength", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("ParamModel", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("ParamType", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("ParamValue", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("StrRange", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Unit", typeof(VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNodeUnit))]
            [System.Xml.Serialization.XmlElementAttribute("validRegex", typeof(object))]
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNodeDropDownList
        {

            private VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNodeDropDownListKeyValue[] keyValueField;

            private string classField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("keyValue")]
            public VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNodeDropDownListKeyValue[] keyValue
            {
                get
                {
                    return this.keyValueField;
                }
                set
                {
                    this.keyValueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string @class
            {
                get
                {
                    return this.classField;
                }
                set
                {
                    this.classField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNodeDropDownListKeyValue
        {

            private byte valueField;

            private string valueField1;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte value
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

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField1;
                }
                set
                {
                    this.valueField1 = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationMovementClassificationIDVehicleComponentParamNodeUnit
        {

            private string unitTextField;

            /// <remarks/>
            public string UnitText
            {
                get
                {
                    return this.unitTextField;
                }
                set
                {
                    this.unitTextField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
        public enum ItemsChoiceType
        {

            /// <remarks/>
            DisplayString,

            /// <remarks/>
            DropDownList,

            /// <remarks/>
            InputType,

            /// <remarks/>
            IsRequired,

            /// <remarks/>
            ParamMaxLength,

            /// <remarks/>
            ParamModel,

            /// <remarks/>
            ParamType,

            /// <remarks/>
            ParamValue,

            /// <remarks/>
            StrRange,

            /// <remarks/>
            Unit,

            /// <remarks/>
            validRegex,
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationVehicleConfiguration
        {

            private uint configurationTypeField;

            private string movementClassificationIDField;

            private VehicleComponentConfigurationVehicleConfigurationParamNode[] paramNodeField;

            /// <remarks/>
            public uint ConfigurationType
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
            public string MovementClassificationID
            {
                get
                {
                    return this.movementClassificationIDField;
                }
                set
                {
                    this.movementClassificationIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ParamNode")]
            public VehicleComponentConfigurationVehicleConfigurationParamNode[] ParamNode
            {
                get
                {
                    return this.paramNodeField;
                }
                set
                {
                    this.paramNodeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationVehicleConfigurationParamNode
        {

            private object[] itemsField;

            private ItemsChoiceType1[] itemsElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("DisplayString", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("DropDownList", typeof(VehicleComponentConfigurationVehicleConfigurationParamNodeDropDownList))]
            [System.Xml.Serialization.XmlElementAttribute("InputType", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("IsRequired", typeof(byte))]
            [System.Xml.Serialization.XmlElementAttribute("ParamMaxLength", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("ParamModel", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("ParamType", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("ParamValue", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("StrRange", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Unit", typeof(VehicleComponentConfigurationVehicleConfigurationParamNodeUnit))]
            [System.Xml.Serialization.XmlElementAttribute("validRegex", typeof(object))]
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
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationVehicleConfigurationParamNodeDropDownList
        {

            private VehicleComponentConfigurationVehicleConfigurationParamNodeDropDownListKeyValue[] keyValueField;

            private string classField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("keyValue")]
            public VehicleComponentConfigurationVehicleConfigurationParamNodeDropDownListKeyValue[] keyValue
            {
                get
                {
                    return this.keyValueField;
                }
                set
                {
                    this.keyValueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string @class
            {
                get
                {
                    return this.classField;
                }
                set
                {
                    this.classField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationVehicleConfigurationParamNodeDropDownListKeyValue
        {

            private byte valueField;

            private string valueField1;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte value
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

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField1;
                }
                set
                {
                    this.valueField1 = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class VehicleComponentConfigurationVehicleConfigurationParamNodeUnit
        {

            private string unitTextField;

            /// <remarks/>
            public string UnitText
            {
                get
                {
                    return this.unitTextField;
                }
                set
                {
                    this.unitTextField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
        public enum ItemsChoiceType1
        {

            /// <remarks/>
            DisplayString,

            /// <remarks/>
            DropDownList,

            /// <remarks/>
            InputType,

            /// <remarks/>
            IsRequired,

            /// <remarks/>
            ParamMaxLength,

            /// <remarks/>
            ParamModel,

            /// <remarks/>
            ParamType,

            /// <remarks/>
            ParamValue,

            /// <remarks/>
            StrRange,

            /// <remarks/>
            Unit,

            /// <remarks/>
            validRegex,
        }
    }
}
