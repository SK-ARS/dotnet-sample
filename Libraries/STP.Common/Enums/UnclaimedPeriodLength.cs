
#region "Namespace"

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum UnclaimedPeriodLength
    {
        [Description("Day(s)")]
        Days = 1,
        [Description("Month(s)")]
        Months = 2,
        [Description("Year(s)")]
        Years = 3,
        [Description("Calendar Year")]
        CalendarYear = 4
    }
}
