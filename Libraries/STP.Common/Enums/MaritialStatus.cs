#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum MaritialStatus
    {
        [Description("Single")] Single = 1,

        [Description("Married")] Married = 2,

        [Description("Divorced")] Divorced = 3,

        [Description("Widow")] Widow = 4,

        [Description("Decline to Provide")] DeclinetoProvide = 5,
    }
}