using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;


namespace STP.Domain.RouteAssessment.AssessmentOutput
{
    public partial class AssessmentOutput
    {
        //[JsonProperty("$id")]
        //public Uri Id { get; set; }

        //[JsonProperty("title")]
        //public string Title { get; set; }

        //[JsonProperty("type")]
        //public string Type { get; set; }

        //[JsonProperty("definitions")]
        //public Definitions Definitions { get; set; }

        //[JsonProperty("schema")]
        //public Uri Schema { get; set; }

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

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Timestamp { get; set; }

        [JsonProperty("timestamp_finish", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? TimestampFinish { get; set; }

        [JsonProperty("movement_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MovementId { get; set; }

        [JsonProperty("global_result", NullValueHandling = NullValueHandling.Ignore)]
        public string GlobalResult { get; set; }

        [JsonProperty("global_comments", NullValueHandling = NullValueHandling.Ignore)]
        public string GlobalComments { get; set; }

        [JsonProperty("EsdalStructure", NullValueHandling = NullValueHandling.Ignore)]
        public List<EsdalStructure> EsdalStructure { get; set; }
    }
    public partial class EsdalStructure
    {
        //[JsonProperty("$id", NullValueHandling = NullValueHandling.Ignore)]
        //public Uri Id { get; set; }

        //[JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        //public string Title { get; set; }

        //[JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        //public string Type { get; set; }

        [JsonProperty("ESRN")]
        public string Esrn { get; set; }

        [JsonProperty("StructureKey")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string StructureKey { get; set; }

        [JsonProperty("StructureCalculationType", NullValueHandling = NullValueHandling.Ignore)]
        public string StructureCalculationType { get; set; }

        [JsonProperty("result_structure")]
        public string ResultStructure { get; set; }

        [JsonProperty("sf")]
        public double Sf { get; set; }

        [JsonProperty("comments_for_haulier")]
        public string CommentsForHaulier { get; set; }

        [JsonProperty("assessment_comments")]
        [JsonConverter(typeof(AssessmentCommentsConverter))]
        public AssessmentComments AssessmentComments { get; set; }

        [JsonProperty("RouteId", NullValueHandling = NullValueHandling.Ignore)]
        public long RouteId { get; set; }
    }
    public enum AssessmentComments
    {
        [Description("Unable to perform assessment - Invalid json schema")] E001,
        [Description("Unable to perform assessment - Structure not assessed due to data issue")] E101,
        [Description("Unable to perform assessment - Structure type not supported by ALSAT")] E102,
        [Description("Unable to perform assessment - Weight restriction applied")] E201,
        [Description("Unable to perform assessment - Data issue - Structure rating missing")] E202,
        [Description("Unable to perform assessment - Data issue - Span length missing")] E301,
        [Description("Unable to perform assessment - Data issue - Span sequence position duplicated")] E302,
        [Description("Unable to perform assessment - Data issue - Span sequence position missing")] E303,
        [Description("")] Empty
    };
    public partial class AssessmentOutput
    {
        public static AssessmentOutput FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AssessmentOutput>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this AssessmentOutput self)
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
            return value;
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

    internal class AssessmentCommentsConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(AssessmentComments) || t == typeof(AssessmentComments?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "E001":
                    return AssessmentComments.E001;
                case "E101":
                    return AssessmentComments.E101;
                case "E102":
                    return AssessmentComments.E102;
                case "E201":
                    return AssessmentComments.E201;
                case "E202":
                    return AssessmentComments.E202;
                case "E301":
                    return AssessmentComments.E301;
                case "E302":
                    return AssessmentComments.E302;
                case "E303":
                    return AssessmentComments.E303;
                case "":
                default:
                    return AssessmentComments.Empty;
            }
            throw new Exception("Cannot unmarshal type AssessmentComments");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AssessmentComments)untypedValue;
            switch (value)
            {
                case AssessmentComments.E001:
                    serializer.Serialize(writer, "E001");
                    return;
                case AssessmentComments.E101:
                    serializer.Serialize(writer, "E101");
                    return;
                case AssessmentComments.E102:
                    serializer.Serialize(writer, "E102");
                    return;
                case AssessmentComments.E201:
                    serializer.Serialize(writer, "E201");
                    return;
                case AssessmentComments.E202:
                    serializer.Serialize(writer, "E202");
                    return;
                case AssessmentComments.E301:
                    serializer.Serialize(writer, "E301");
                    return;
                case AssessmentComments.E302:
                    serializer.Serialize(writer, "E302");
                    return;
                case AssessmentComments.E303:
                    serializer.Serialize(writer, "E303");
                    return;
                case AssessmentComments.Empty:
                default:
                    serializer.Serialize(writer, "");
                    return;

            }
            throw new Exception("Cannot marshal type AssessmentComments");
        }

        public static readonly AssessmentCommentsConverter Singleton = new AssessmentCommentsConverter();
    }

    public static class EnumExtensions
    {
        public static string GetString(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    System.ComponentModel.DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
                    if (attr != null)
                    {
                        //return the description if we have it
                        name = attr.Description;
                    }
                }
            }
            return name;
        }

        public static T ToEnum<T>(this string value)
        {
            T theEnum = default(T);

            Type enumType = typeof(T);

            //check and see if the value is a non attribute value
            try
            {
                theEnum = (T)Enum.Parse(enumType, value);
            }
            catch (System.ArgumentException e)
            {
                bool found = false;
                foreach (T enumValue in Enum.GetValues(enumType))
                {
                    System.Reflection.FieldInfo field = enumType.GetField(enumValue.ToString());

                    System.ComponentModel.DescriptionAttribute attr =
                               Attribute.GetCustomAttribute(field,
                                 typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;

                    if (attr != null && attr.Description.Equals(value))
                    {
                        theEnum = enumValue;
                        found = true;
                        break;

                    }
                }
                if (!found)
                    throw new ArgumentException("Cannot convert " + value + " to " + enumType.ToString());
            }

            return theEnum;
        }
    }
}
