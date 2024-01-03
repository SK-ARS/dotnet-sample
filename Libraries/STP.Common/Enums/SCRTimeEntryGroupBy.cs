#region

using System;
using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    [CLSCompliant(false)]
    public enum SCRTimeEntryGroupBy
    {
        [Description("Date Logged")]
        DateLogged = 0,

        [Description("Employee")]
        Employee = 1,

        [Description("SCR")]
        SCR = 2,
    }
}
