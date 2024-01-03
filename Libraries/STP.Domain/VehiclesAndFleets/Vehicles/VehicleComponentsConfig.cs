using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace STP.VehiclesAndFleets.Configurations
{
    public class VehicleComponentsConfig
    {
		[XmlRoot(ElementName = "Classification")]
		public class Classification
		{
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "MovementClassification")]
		public class MovementClassification
		{
			[XmlElement(ElementName = "Classification")]
			public List<Classification> Classification { get; set; }
		}

		[XmlRoot(ElementName = "ComponentType")]
		public class ComponentType
		{
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
			[XmlAttribute(AttributeName = "Tractor")]
			public string Tractor { get; set; }
			[XmlAttribute(AttributeName = "img")]
			public string Img { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "VehicleComponentType")]
		public class VehicleComponentType
		{
			[XmlElement(ElementName = "ComponentType")]
			public List<ComponentType> ComponentType { get; set; }
		}

		[XmlRoot(ElementName = "AxleConfig")]
		public class AxleConfig
		{
			[XmlElement(ElementName = "MaxWheels")]
			public string MaxWheels { get; set; }
			[XmlElement(ElementName = "MinWeight")]
			public string MinWeight { get; set; }
			[XmlElement(ElementName = "MaxWeight")]
			public string MaxWeight { get; set; }
			[XmlElement(ElementName = "MinAxleSpacing")]
			public string MinAxleSpacing { get; set; }
			[XmlElement(ElementName = "MaxAxleSpacing")]
			public string MaxAxleSpacing { get; set; }
			[XmlElement(ElementName = "MinTyreCentre")]
			public string MinTyreCentre { get; set; }
			[XmlElement(ElementName = "MaxTyreCentre")]
			public string MaxTyreCentre { get; set; }
		}

		[XmlRoot(ElementName = "SubComponentType")]
		public class SubComponentType
		{
			[XmlElement(ElementName = "image")]
			public string Image { get; set; }
			[XmlElement(ElementName = "subcompName")]
			public string SubcompName { get; set; }
			[XmlElement(ElementName = "AxleConfig")]
			public AxleConfig AxleConfig { get; set; }
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "VehicleSubComponentType")]
		public class VehicleSubComponentType
		{
			[XmlElement(ElementName = "SubComponentType")]
			public List<SubComponentType> SubComponentType { get; set; }
		}

		[XmlRoot(ElementName = "Configuration")]
		public class Configuration
		{
			[XmlElement(ElementName = "ConfigName")]
			public string ConfigName { get; set; }
			[XmlElement(ElementName = "ConfigId")]
			public string ConfigId { get; set; }
			[XmlElement(ElementName = "ComponentType")]
			public string ComponentType { get; set; }
			[XmlElement(ElementName = "NumberOfComponents")]
			public string NumberOfComponents { get; set; }
			[XmlElement(ElementName = "OnlyTractorInFront")]
			public string OnlyTractorInFront { get; set; }
			[XmlElement(ElementName = "SideBySideRows")]
			public string SideBySideRows { get; set; }
		}

		[XmlRoot(ElementName = "VehicleConfigurationTypes")]
		public class VehicleConfigurationTypes
		{
			[XmlElement(ElementName = "MaxTrailersinConfig")]
			public string MaxTrailersinConfig { get; set; }
			[XmlElement(ElementName = "MaxTractorsinConfig")]
			public string MaxTractorsinConfig { get; set; }
			[XmlElement(ElementName = "Configuration")]
			public List<Configuration> Configuration { get; set; }
		}

		[XmlRoot(ElementName = "ParamNode")]
		public class ParamNode
		{
			[XmlElement(ElementName = "DisplayString")]
			public string DisplayString { get; set; }
			[XmlElement(ElementName = "ParamModel")]
			public string ParamModel { get; set; }
			[XmlElement(ElementName = "ParamType")]
			public List<string> ParamType { get; set; }
			[XmlElement(ElementName = "ParamMaxLength")]
			public List<string> ParamMaxLength { get; set; }
			[XmlElement(ElementName = "StrRange")]
			public List<string> StrRange { get; set; }
			[XmlElement(ElementName = "InputType")]
			public string InputType { get; set; }
			[XmlElement(ElementName = "validRegex")]
			public string ValidRegex { get; set; }
			[XmlElement(ElementName = "IsRequired")]
			public string IsRequired { get; set; }
			[XmlElement(ElementName = "ParamValue")]
			public string ParamValue { get; set; }
			[XmlElement(ElementName = "DropDownList")]
			public DropDownList DropDownList { get; set; }
			[XmlElement(ElementName = "Unit")]
			public Unit Unit { get; set; }
		}

		[XmlRoot(ElementName = "keyValue")]
		public class KeyValue
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "DropDownList")]
		public class DropDownList
		{
			[XmlElement(ElementName = "keyValue")]
			public List<KeyValue> KeyValue { get; set; }
			[XmlAttribute(AttributeName = "class")]
			public string Class { get; set; }
		}

		[XmlRoot(ElementName = "VehicleComponent")]
		public class VehicleComponent
		{
			[XmlElement(ElementName = "ComponentTypeID")]
			public string ComponentTypeID { get; set; }
			[XmlElement(ElementName = "SubComponentTypeid")]
			public string SubComponentTypeid { get; set; }
			[XmlElement(ElementName = "AxleConfiguration")]
			public string AxleConfiguration { get; set; }
			[XmlElement(ElementName = "ConfigureTyreSpacing")]
			public string ConfigureTyreSpacing { get; set; }
			[XmlElement(ElementName = "ParamNode")]
			public List<ParamNode> ParamNode { get; set; }
			[XmlElement(ElementName = "AxleConfig")]
			public AxleConfig AxleConfig { get; set; }
		}

		[XmlRoot(ElementName = "Unit")]
		public class Unit
		{
			[XmlElement(ElementName = "UnitText")]
			public string UnitText { get; set; }
		}

		[XmlRoot(ElementName = "MovementClassificationID")]
		public class MovementClassificationID
		{
			[XmlElement(ElementName = "VehicleComponent")]
			public List<VehicleComponent> VehicleComponent { get; set; }
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "VehicleComponents")]
		public class VehicleComponents
		{
			[XmlElement(ElementName = "MovementClassificationID")]
			public List<MovementClassificationID> MovementClassificationID { get; set; }
		}

		[XmlRoot(ElementName = "VehicleConfiguration")]
		public class VehicleConfiguration
		{
			[XmlElement(ElementName = "ConfigurationType")]
			public string ConfigurationType { get; set; }
			[XmlElement(ElementName = "MovementClassificationID")]
			public string MovementClassificationID { get; set; }
			[XmlElement(ElementName = "ParamNode")]
			public List<ParamNode> ParamNode { get; set; }
		}

		[XmlRoot(ElementName = "VehicleConfigurations")]
		public class VehicleConfigurations
		{
			[XmlElement(ElementName = "VehicleConfiguration")]
			public List<VehicleConfiguration> VehicleConfiguration { get; set; }
		}

		[XmlRoot(ElementName = "VehicleComponentConfiguration")]
		public class VehicleComponentConfiguration
		{
			[XmlElement(ElementName = "MovementClassification")]
			public MovementClassification MovementClassification { get; set; }
			[XmlElement(ElementName = "VehicleComponentType")]
			public VehicleComponentType VehicleComponentType { get; set; }
			[XmlElement(ElementName = "VehicleSubComponentType")]
			public VehicleSubComponentType VehicleSubComponentType { get; set; }
			[XmlElement(ElementName = "VehicleConfigurationTypes")]
			public VehicleConfigurationTypes VehicleConfigurationTypes { get; set; }
			[XmlElement(ElementName = "VehicleComponents")]
			public VehicleComponents VehicleComponents { get; set; }
			[XmlElement(ElementName = "VehicleConfigurations")]
			public VehicleConfigurations VehicleConfigurations { get; set; }
		}

	}
}