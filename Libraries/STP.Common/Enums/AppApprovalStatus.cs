#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum AppApprovalStatus
    {
        [Description("In Process")] InProcess = 1,

        [Description("Complete")] Complete = 2,
    }
}