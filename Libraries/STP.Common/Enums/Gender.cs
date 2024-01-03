#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum Gender
    {
        [Description("Male")] Male = 1,

        [Description("Female")] Female = 2,

        [Description("Decline to Provide")] DeclinetoProvide = 3,
    }
}
