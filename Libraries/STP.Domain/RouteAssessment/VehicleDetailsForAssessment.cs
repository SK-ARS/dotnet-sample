using Newtonsoft.Json;
using STP.Domain.RouteAssessment.AssessmentInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.RouteAssessment.VehicleDetailsForAssessment
{
     public partial class VehicleDetailsForAssessment
    {

        [JsonProperty("ConfigurationSummaryListPosition")]
        public ConfigurationSummaryListPositionForAssessment ConfigurationSummaryListPosition { get; set; }

        [JsonProperty("Configuration")]
        public Configuration Configuration { get; set; }
    }
    public partial class VehicleDetailsForAssessment
    {
        public static VehicleDetailsForAssessment FromJson(string json)
        {
            return JsonConvert.DeserializeObject<VehicleDetailsForAssessment>(json);
        }
    }
    public partial class ConfigurationSummaryListPositionForAssessment
    {
        [JsonProperty("ConfigurationSummary")]
        public string ConfigurationSummary { get; set; }

        [JsonProperty("ConfigurationComponentsNo")]
        public long ConfigurationComponentsNo { get; set; }
    }
    public partial class Configuration
    {
        [JsonProperty("ComponentListPosition")]
        public ComponentListPosition ComponentListPosition { get; set; }
    }
}
