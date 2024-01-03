using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum Category
    {
        [Description("Functional")] Functional = 1,
        [Description("UI")] UI = 2,
        [Description("Validation")] Validation = 3,
        [Description("Performance")] Performance = 4,
    }
}