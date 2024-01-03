using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum AppEnvironment
    {
        [Description("QualityAssurance")] QA = 1,
        [Description("Staging")] Staging = 2,
        [Description("Maintanence")] Maintanence = 3,
        [Description("Pilot")] Pilot = 4,
        [Description("Production")] Production = 5,
    }
}