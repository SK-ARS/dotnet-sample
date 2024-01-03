#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum SarLogType
    {
        [Description("SarLog")] ShowSarLog = 1,

        [Description("BlackList")] ShowBlacklist = 2,

        [Description("Rejected")] ShowRejected = 3,

        [Description("BlackList Attempts")] ShowBlackListAttempts = 4
    }
}