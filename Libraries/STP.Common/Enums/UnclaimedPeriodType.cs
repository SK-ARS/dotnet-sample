
#region "NameSpace"

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum UnclaimedPeriodType
    {
        [Description("Time Span")]
        TimeSpan = 1,
        [Description("Life Time")]
        LifeTime = 2,
        [Description("Calendar Year")]
        CalendarYear = 3
    }
}
