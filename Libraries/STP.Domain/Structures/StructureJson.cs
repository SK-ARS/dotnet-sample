using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace STP.Domain.Structures.StructureJSON
{

    public partial class EsdalStructureJson
    {
        [JsonProperty("EsdalStructure")]
        public List<EsdalStructure> EsdalStructure { get; set; }
    }

    public partial class EsdalStructure
    {
        [JsonProperty("SeqNumber", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<long> SeqNumber { get; set; }

        [JsonProperty("Status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("ESRN")]
        public string ESRN { get; set; }

        [JsonProperty("StructureKey")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string StructureKey { get; set; }

        [JsonProperty("StructureType")]
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
        public int SkewAngle { get; set; }

        [JsonProperty("LoadRating")]
        public LoadRating LoadRating { get; set; }

        [JsonProperty("SignedWeightConstraints")]
        public SignedWeightConstraints SignedWeightConstraints { get; set; }

        [JsonProperty("Span")]
        public List<Span> Span { get; set; }
    }

    public partial class LoadRating
    {
        [JsonProperty("HBRatingWithLoad")]
        public Nullable<double> HbRatingWithLoad { get; set; }
        [JsonProperty("HBRatingWithoutLoad")]
        public Nullable<double> HbRatingWithoutLoad { get; set; }
        [JsonProperty("SVRating")]
        [JsonConverter(typeof(SVRatingConverter))]
        public SVRating SVRating { get; set; }
        [JsonProperty("SVRatings")]
        public SVRatings SVRatings { get; set; }        
        [JsonProperty("SVParameters")]
        public SVParameters SVParameters { get; set; }
    }

    public partial class SVRatings
    {
        [JsonProperty("SVParameters")]
        public List<SVParameters> SVParameters { get; set; }
    }

    public partial class Span
    {
        [JsonProperty("SpanNumber")]
        public long SpanNumber { get; set; }

        [JsonProperty("SpanPosition")]
        public SpanPosition SpanPosition { get; set; }

        [JsonProperty("Length")]
        public double Length { get; set; }

        [JsonProperty("StructureType")]
        public string StructureType { get; set; }

        [JsonProperty("Construction")]
        public string Construction { get; set; }
    }

    public partial class SpanPosition
    {
        [JsonProperty("SequencePosition")]
        public int SequencePosition { get; set; }

        [JsonProperty("SequenceNumber")]
        public int SequenceNumber { get; set; }
    }
    public partial class SVParameters
    {
        [JsonProperty("VehicleType")]
        [JsonConverter(typeof(VehicleTypeConverter))]
        public VehicleType VehicleType { get; set; }
        public decimal? SVReserveWithLoad { get; set; }
        public decimal? SVReserveWithOutLoad { get; set; }
    }
    public partial class SignedWeightConstraints
    {
        public decimal GrossWeight { get; set; }
        public decimal AxleWeight { get; set; }
        public decimal DoubleAxleWeight { get; set; }
        public decimal TripleAxleWeight { get; set; }
        public decimal AxleGroupWeight { get; set; }
    }
    public enum StructureType
    {
        BoxCulvert, SimplySupportedSpan, cablestayedbridge, multispanbridge,
        continuousspanbridge, integralstructure, masonryarch, portalframe,
        suspensionbridge, levelcrossing, lift, swing, cantilever, proppedcantilever, pipe
    }; //simplysupportedspan

    public enum Construction
    {
        arch, metallicwidenedarch, compositewidenedarch, slabwidenedarch, posttensionedbeamandslab,
        pretensionedbeamandslab, reinforcedconcretebeamandslab, metallic, prestressedconcretebox, prestressedconcreteslab, reinforcedconcreteslab,
        steelandconcretecomposite, archpluselements, archplusslab, beamandslab, box, orthotropic, Steel, slab, filledarch
    }; //Steel, BeamAndSlab

    public enum VehicleType
    {
        svnone, sv80, sv100, sv150, svtrain, svtt, unknown
    };
    public enum SVRating
    {
        svnone, sv80, sv100, sv150, svtrain, svtt, unknown
    };
    public class StructureList
    {
        public long SequenceNumber { get; set; }
        public string Status { get; set; }
        public byte[] StructureBlobs { get; set; }
    }
    public partial class EsdalStructureJson
    {
        public static EsdalStructureJson FromJson(string json)
        {
            return JsonConvert.DeserializeObject<EsdalStructureJson>(json);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this EsdalStructureJson self)
        {
            return JsonConvert.SerializeObject(self);
        }
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
                case "simply supported span":
                    return StructureType.SimplySupportedSpan;
                case "box culvert":
                    return StructureType.BoxCulvert;
                case "continuous span bridge":
                    return StructureType.continuousspanbridge;
                case "cable stayed bridge":
                    return StructureType.cablestayedbridge;
                case "multi span bridge":
                    return StructureType.multispanbridge;
                case "integral structure":
                    return StructureType.integralstructure;
                case "masonry arch":
                    return StructureType.masonryarch;
                case "portal frame":
                    return StructureType.portalframe;
                case "suspension bridge":
                    return StructureType.suspensionbridge;
                case "level crossing":
                    return StructureType.levelcrossing;
                case "propped cantilever":
                    return StructureType.proppedcantilever;
                case "lift":
                    return StructureType.lift;
                case "swing":
                    return StructureType.swing;
                case "cantilever":
                    return StructureType.cantilever;
                case "pipe":
                    return StructureType.pipe;

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
                case StructureType.SimplySupportedSpan:
                    serializer.Serialize(writer, "simply supported span");
                    return;
                case StructureType.BoxCulvert:
                    serializer.Serialize(writer, "box culvert");
                    return;
                case StructureType.continuousspanbridge:
                    serializer.Serialize(writer, "continuous span bridge");
                    return;
                case StructureType.cablestayedbridge:
                    serializer.Serialize(writer, "cable stayed bridge");
                    return;
                case StructureType.multispanbridge:
                    serializer.Serialize(writer, "multi span bridge");
                    return;
                case StructureType.integralstructure:
                    serializer.Serialize(writer, "integral structure");
                    return;
                case StructureType.masonryarch:
                    serializer.Serialize(writer, "masonry arch");
                    return;
                case StructureType.portalframe:
                    serializer.Serialize(writer, "portal frame");
                    return;
                case StructureType.suspensionbridge:
                    serializer.Serialize(writer, "suspension bridge");
                    return;
                case StructureType.levelcrossing:
                    serializer.Serialize(writer, "level crossing");
                    return;
                case StructureType.proppedcantilever:
                    serializer.Serialize(writer, "propped cantilever");
                    return;
                case StructureType.lift:
                    serializer.Serialize(writer, "lift");
                    return;
                case StructureType.swing:
                    serializer.Serialize(writer, "swing");
                    return;
                case StructureType.cantilever:
                    serializer.Serialize(writer, "cantilever");
                    return;
                case StructureType.pipe:
                    serializer.Serialize(writer, "pipe");
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
                    return Construction.beamandslab;
                case "steel":
                    return Construction.Steel;
                case "metallic widened arch":
                    return Construction.metallicwidenedarch;
                case "composite widened arch":
                    return Construction.compositewidenedarch;
                case "slab widened arch":
                    return Construction.slabwidenedarch;
                case "post tensioned beam and slab":
                    return Construction.posttensionedbeamandslab;
                case "pre tensioned beam and slab":
                    return Construction.pretensionedbeamandslab;
                case "reinforced concrete beam and slab":
                    return Construction.reinforcedconcretebeamandslab;
                case "prestressed concrete box":
                    return Construction.prestressedconcretebox;
                case "prestressed concrete slab":
                    return Construction.prestressedconcreteslab;
                case "reinforced concrete slab":
                    return Construction.reinforcedconcreteslab;
                case "steel and concrete composite":
                    return Construction.steelandconcretecomposite;
                case "arch plus elements":
                    return Construction.archpluselements;
                case "arch plus slab":
                    return Construction.archplusslab;
                case "filled arch":
                    return Construction.filledarch;
                case "slab":
                    return Construction.slab;
                case "orthotropic":
                    return Construction.orthotropic;
                case "box":
                    return Construction.box;
                case "arch":
                    return Construction.arch;
                case "metallic":
                    return Construction.metallic;
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
                case Construction.beamandslab:
                    serializer.Serialize(writer, "beam and slab");
                    return;
                case Construction.Steel:
                    serializer.Serialize(writer, "steel");
                    return;
                case Construction.metallicwidenedarch:
                    serializer.Serialize(writer, "metallic widened arch");
                    return;
                case Construction.compositewidenedarch:
                    serializer.Serialize(writer, "composite widened arch");
                    return;
                case Construction.slabwidenedarch:
                    serializer.Serialize(writer, "slab widened arch");
                    return;
                case Construction.posttensionedbeamandslab:
                    serializer.Serialize(writer, "post tensioned beam and slab");
                    return;
                case Construction.pretensionedbeamandslab:
                    serializer.Serialize(writer, "pre tensioned beam and slab");
                    return;
                case Construction.reinforcedconcretebeamandslab:
                    serializer.Serialize(writer, "reinforced concrete beam and slab");
                    return;
                case Construction.prestressedconcretebox:
                    serializer.Serialize(writer, "prestressed concrete box");
                    return;
                case Construction.prestressedconcreteslab:
                    serializer.Serialize(writer, "prestressed concrete slab");
                    return;
                case Construction.reinforcedconcreteslab:
                    serializer.Serialize(writer, "reinforced concrete slab");
                    return;
                case Construction.steelandconcretecomposite:
                    serializer.Serialize(writer, "steel and concrete composite");
                    return;
                case Construction.archpluselements:
                    serializer.Serialize(writer, "arch plus elements");
                    return;
                case Construction.archplusslab:
                    serializer.Serialize(writer, "arch plus slab");
                    return;
                case Construction.filledarch:
                    serializer.Serialize(writer, "filled arch");
                    return;
                case Construction.slab:
                    serializer.Serialize(writer, "slab");
                    return;
                case Construction.orthotropic:
                    serializer.Serialize(writer, "orthotropic");
                    return;
                case Construction.box:
                    serializer.Serialize(writer, "box");
                    return;
                case Construction.arch:
                    serializer.Serialize(writer, "arch");
                    return;
                case Construction.metallic:
                    serializer.Serialize(writer, "metallic");
                    return;
            }
            throw new Exception("Cannot marshal type Construction");
        }

        public static readonly ConstructionConverter Singleton = new ConstructionConverter();
    }

    internal class VehicleTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(VehicleType) || t == typeof(VehicleType?);
        }
        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "svnone":
                    return VehicleType.svnone;
                case "sv80":
                    return VehicleType.sv80;
                case "sv100":
                    return VehicleType.sv100;
                case "sv150":
                    return VehicleType.sv150;
                case "svtrain":
                    return VehicleType.svtrain;
                case "svtt":
                    return VehicleType.svtt;
                case "unknown":
                    return VehicleType.unknown;

            }
            throw new Exception("Cannot unmarshal type VehicleType");
        }
        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (VehicleType)untypedValue;
            switch (value)
            {
                case VehicleType.svnone:
                    serializer.Serialize(writer, "svnone");
                    return;
                case VehicleType.sv80:
                    serializer.Serialize(writer, "sv80");
                    return;
                case VehicleType.sv100:
                    serializer.Serialize(writer, "sv100");
                    return;
                case VehicleType.sv150:
                    serializer.Serialize(writer, "sv150");
                    return;
                case VehicleType.svtrain:
                    serializer.Serialize(writer, "svtrain");
                    return;
                case VehicleType.svtt:
                    serializer.Serialize(writer, "svtt");
                    return;
                case VehicleType.unknown:
                    serializer.Serialize(writer, "unknown");
                    return;
            }
            throw new Exception("Cannot marshal type VehicleType");
        }

        public static readonly VehicleTypeConverter Singleton = new VehicleTypeConverter();
    }

    internal class SVRatingConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(SVRating) || t == typeof(SVRating?);
        }
        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "svnone":
                    return SVRating.svnone;
                case "sv80":
                    return SVRating.sv80;
                case "sv100":
                    return SVRating.sv100;
                case "sv150":
                    return SVRating.sv150;
                case "svtrain":
                    return SVRating.svtrain;
                case "svtt":
                    return SVRating.svtt;
                case "unknown":
                    return SVRating.unknown;

            }
            throw new Exception("Cannot unmarshal type SVRating");
        }
        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (SVRating)untypedValue;
            switch (value)
            {
                case SVRating.svnone:
                    serializer.Serialize(writer, "svnone");
                    return;
                case SVRating.sv80:
                    serializer.Serialize(writer, "sv80");
                    return;
                case SVRating.sv100:
                    serializer.Serialize(writer, "sv100");
                    return;
                case SVRating.sv150:
                    serializer.Serialize(writer, "sv150");
                    return;
                case SVRating.svtrain:
                    serializer.Serialize(writer, "svtrain");
                    return;
                case SVRating.svtt:
                    serializer.Serialize(writer, "svtt");
                    return;
                case SVRating.unknown:
                    serializer.Serialize(writer, "unknown");
                    return;
            }
            throw new Exception("Cannot marshal type SVRating");
        }

        public static readonly SVRatingConverter Singleton = new SVRatingConverter();
    }
}
