#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum MarketingComplianceApprovalStatus
    {
        [Description("Pending")] Pending = 1,

        [Description("Completed")] Completed = 2,
    }
}
