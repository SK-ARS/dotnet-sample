using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace STP.Domain.Structures.VehicleJSON
{
    public partial class EsdalVehiclesJSON
    {
        [JsonProperty("Vehicles")]
        public Vehicles Vehicles { get; set; }
    }
    public partial class Vehicles
    {
        [JsonProperty("ConfigurationSummaryListPosition")]
        public ConfigurationSummaryListPosition ConfigurationSummaryListPosition { get; set; }

        [JsonProperty("Configuration")]
        public Configuration Configuration { get; set; }
    }

    public partial class Configuration
    {
        [JsonProperty("ComponentListPosition")]
        public ComponentListPosition ComponentListPosition { get; set; }
    }

    public partial class ComponentListPosition
    {
        [JsonProperty("Component")]
        public List<Component> Component { get; set; }
    }

    public partial class Component
    {
        [JsonProperty("ComponentType")]
        public string ComponentType { get; set; }

        [JsonProperty("ComponentSubType")]
        public string ComponentSubType { get; set; }

        [JsonProperty("Longitude")]
        public long Longitude { get; set; }

        [JsonProperty("AxleConfiguration")]
        public AxleConfiguration AxleConfiguration { get; set; }
    }

    public partial class AxleConfiguration
    {
        [JsonProperty("NumberOfAxles")]
        public long NumberOfAxles { get; set; }

        [JsonProperty("AxleWeightListPosition")]
        public AxleWeightListPosition AxleWeightListPosition { get; set; }

        [JsonProperty("AxleSpacingListPosition")]
        public AxleSpacingListPosition AxleSpacingListPosition { get; set; }

        [JsonProperty("AxleSpacingToFollowing")]
        public double AxleSpacingToFollowing { get; set; }
    }

    public partial class AxleSpacingListPosition
    {
        [JsonProperty("AxleSpacing")]
        public List<Axles> AxleSpacing { get; set; }
    }

    public partial class Axles
    {
        [JsonProperty("AxleCount")]
        public long AxleCount { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

    public partial class AxleWeightListPosition
    {
        [JsonProperty("AxleWeight")]
        public List<Axles> AxleWeight { get; set; }
    }

    public partial class ConfigurationSummaryListPosition
    {
        [JsonProperty("ConfigurationSummary")]
        public string ConfigurationSummary { get; set; }

        [JsonProperty("ConfigurationComponentsNo")]
        public long ConfigurationComponentsNo { get; set; }
    }

    public partial class Vehicles
    {
        public static Vehicles FromJson(string json)
        {
           return JsonConvert.DeserializeObject<Vehicles>(json);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this Vehicles self)
        {
            return JsonConvert.SerializeObject(self);
        }
    }

    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters =
    //        {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}
}