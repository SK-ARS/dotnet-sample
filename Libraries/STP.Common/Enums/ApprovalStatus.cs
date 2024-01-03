#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum ApprovalStatus
    {
        [Description("In Queue")] Queue = -2,

        [Description("Pending")] Pending = 0,

        [Description("In Process")] InProcess = 1,

        [Description("On Hold")] OnHold = 2,

        [Description("Rejected")] Rejected = 3,

        [Description("Approved")] Approved = 4,

        [Description("Need More Information")] NeedMoreInformation = 5,

        [Description("Resubmitted")] Resubmitted = 6
    }
}