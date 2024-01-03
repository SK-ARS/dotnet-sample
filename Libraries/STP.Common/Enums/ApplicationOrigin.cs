#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum ApplicationOrigin
    {
        [Description("NTSOnline Web Application")] NTSOnlineWeb = 0,

        [Description("Advertisement")] Advertisement = 1,

        [Description("Email")] Email = 2,

        [Description("Friend")] Friend = 3,

        [Description("RIAFinancial")] RIAFinancial = 4
    }
}