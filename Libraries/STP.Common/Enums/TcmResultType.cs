using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum TcmResultType
    {
        [Description("Pass")]
        Pass = 2,
        [Description("Fail")]
        Fail = 3,
        [Description("N/A")]
        NotApplicable = 1,
    }
}