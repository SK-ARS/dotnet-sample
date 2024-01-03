using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.RouteAssessment.AssessmentInput
{
    public class AssessmentResponse
    {
        public AssessmentInput AssessmentInput { get; set; }
        public string ExceptionCode { get; set; }
    }

    public class TempProperties
    {
        public long SequenceNumber { get; set; }
        public DateTime? Timestamp { get; set; }
        public string MovementId { get; set; }

        public byte[] Vehicles { get; set; }
        public byte[] EsdalStructure { get; set; }
    }

    public class TempVehicles
    {
        public Vehicles Vehicles { get; set; }
    }

    public class TempStructures
    {
        public List<EsdalStructure> EsdalStructure { get; set; }
    }
  public partial class AssessmentInput
    {
        [JsonProperty("$id")]
        public Uri Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("definitions")]
        public Definitions Definitions { get; set; }

        [JsonProperty("schema")]
        public Uri Schema { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

    }
    public partial class Definitions
    {
    }

    public partial class Properties
    {
        [JsonProperty("sequence_number")]
        public long SequenceNumber { get; set; }

        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("movement_id")]
        public string MovementId { get; set; }

        [JsonProperty("Vehicles")]
        public Vehicles Vehicles { get; set; }

        [JsonProperty("EsdalStructure")]
        public List<EsdalStructure> EsdalStructure { get; set; }
    }

    public partial class EsdalStructure
    {
        [JsonProperty("ESRN")]
        public string Esrn { get; set; }

        [JsonProperty("StructureKey")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string StructureKey { get; set; }

        [JsonProperty("StructureType")]
        //public StructureType StructureType { get; set; }
        public string StructureType { get; set; }

        [JsonProperty("UnderbridgeSections")]
        public UnderbridgeSections UnderbridgeSections { get; set; }
    }

    public partial class UnderbridgeSections
    {
        [JsonProperty("UnderbridgeSection")]
        public UnderbridgeSection UnderbridgeSection { get; set; }
    }

    public partial class UnderbridgeSection
    {
        [JsonProperty("SkewAngle")]
        public long? SkewAngle { get; set; }

        [JsonProperty("LoadRating")]
        public LoadRating LoadRating { get; set; }

        [JsonProperty("Span")]
        public List<Span> Span { get; set; }

        [JsonProperty("SignedWeightConstraints")]
        public SignedWeightConstraints SignedWeightConstraints { get; set; }
    }

    public partial class LoadRating
    {
        [JsonProperty("HBRatingWithLoad", NullValueHandling = NullValueHandling.Ignore)]
        public long? HbRatingWithLoad { get; set; }

        [JsonProperty("HBRatingWithoutLoad", NullValueHandling = NullValueHandling.Ignore)]
        public long? HbRatingWithoutLoad { get; set; }

        [JsonProperty("SVRatings", NullValueHandling = NullValueHandling.Ignore)]
        public SVRatings SVRatings { get; set; }
    }

    public partial class SVRatings
    {
        [JsonProperty("SVParameters")]
        public List<SVParameters> SVParameters { get; set; }

    }

    public partial class SignedWeightConstraints
    {
        [JsonProperty("AxleWeight")]
        public long AxleWeight { get; set; }

        [JsonProperty("GrossWeight")]
        public long GrossWeight { get; set; }

        [JsonProperty("DoubleAxleWeight")]
        public long DoubleAxleWeight { get; set; }

        [JsonProperty("TripleAxleWeight")]
        public long TripleAxleWeight { get; set; }
        [JsonProperty("AxleGroupWeight")]
        public long AxleGroupWeight { get; set; }
    }

    public partial class SVParameters
    {
        [JsonProperty("VehicleType", NullValueHandling = NullValueHandling.Ignore)]
        public string VehicleType { get; set; }

        [JsonProperty("SVReserveWithLoad", NullValueHandling = NullValueHandling.Ignore)]
        public double? SvReserveWithLoad { get; set; }

        [JsonProperty("SVReserveWithOutLoad", NullValueHandling = NullValueHandling.Ignore)]
        public double? SvReserveWithOutLoad { get; set; }
    }

    public partial class Span
    {
        [JsonProperty("SpanNumber")]
        public long SpanNumber { get; set; }

        [JsonProperty("SpanPosition")]
        public SpanPosition SpanPosition { get; set; }

        [JsonProperty("Length")]
        public double? Length { get; set; }

        [JsonProperty("StructureType")]
        //public StructureType StructureType { get; set; }
        public string StructureType { get; set; }

        [JsonProperty("Construction")]
        //public Construction Construction { get; set; }
        public string Construction { get; set; }
    }

    public partial class SpanPosition
    {
        [JsonProperty("SequencePosition")]
        public long SequencePosition { get; set; }

        [JsonProperty("SequenceNumber")]
        public long SequenceNumber { get; set; }
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
        public List<Axle> AxleSpacing { get; set; }
    }

    public partial class Axle
    {
        [JsonProperty("AxleCount")]
        public long AxleCount { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
    
    public partial class AxleWeightListPosition
    {
        [JsonProperty("AxleWeight")]
        public List<Axle> AxleWeight { get; set; }
    }

    public partial class ConfigurationSummaryListPosition
    {
        [JsonProperty("ConfigurationSummary")]
        public string ConfigurationSummary { get; set; }

        [JsonProperty("ConfigurationComponentsNo")]
        public long ConfigurationComponentsNo { get; set; }
    }

    public enum StructureType { BoxCulvert, ContinuousSpanBridge, ContinuousSpanBridges, SimplySupportedSpan };

    public enum Construction { BeamAndSlab, Box, Slab };

    public enum SequenceNumber { UnknownValue };

    public partial class AssessmentInput
    {
        public static AssessmentInput FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AssessmentInput>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this AssessmentInput self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                StructureTypeConverter.Singleton,
                ConstructionConverter.Singleton,
                SequenceNumberConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(long) || t == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class StructureTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(StructureType) || t == typeof(StructureType?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "box culvert":
                    return StructureType.BoxCulvert;
                case "continuous span bridge":
                    return StructureType.ContinuousSpanBridge;
                case "continuous span bridges":
                    return StructureType.ContinuousSpanBridges;
                case "simply supported span":
                    return StructureType.SimplySupportedSpan;
            }
            throw new Exception("Cannot unmarshal type StructureType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (StructureType)untypedValue;
            switch (value)
            {
                case StructureType.BoxCulvert:
                    serializer.Serialize(writer, "box culvert");
                    return;
                case StructureType.ContinuousSpanBridge:
                    serializer.Serialize(writer, "continuous span bridge");
                    return;
                case StructureType.ContinuousSpanBridges:
                    serializer.Serialize(writer, "continuous span bridges");
                    return;
                case StructureType.SimplySupportedSpan:
                    serializer.Serialize(writer, "simply supported span");
                    return;
            }
            throw new Exception("Cannot marshal type StructureType");
        }

        public static readonly StructureTypeConverter Singleton = new StructureTypeConverter();
    }

    internal class ConstructionConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(Construction) || t == typeof(Construction?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "beam and slab":
                    return Construction.BeamAndSlab;
                case "box":
                    return Construction.Box;
                case "slab":
                    return Construction.Slab;
            }
            throw new Exception("Cannot unmarshal type Construction");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Construction)untypedValue;
            switch (value)
            {
                case Construction.BeamAndSlab:
                    serializer.Serialize(writer, "beam and slab");
                    return;
                case Construction.Box:
                    serializer.Serialize(writer, "box");
                    return;
                case Construction.Slab:
                    serializer.Serialize(writer, "slab");
                    return;
            }
            throw new Exception("Cannot marshal type Construction");
        }

        public static readonly ConstructionConverter Singleton = new ConstructionConverter();
    }

    internal class SequenceNumberConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(SequenceNumber) || t == typeof(SequenceNumber?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "unknown value")
            {
                return SequenceNumber.UnknownValue;
            }
            throw new Exception("Cannot unmarshal type SequenceNumber");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (SequenceNumber)untypedValue;
            if (value == SequenceNumber.UnknownValue)
            {
                serializer.Serialize(writer, "unknown value");
                return;
            }
            throw new Exception("Cannot marshal type SequenceNumber");
        }

        public static readonly SequenceNumberConverter Singleton = new SequenceNumberConverter();
    }

}
