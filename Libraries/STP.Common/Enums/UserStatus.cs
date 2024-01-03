#region

using System;
using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    [CLSCompliant(false)]
    public enum UserStatus   
    {
        [Description("Pending")] Pending = 0,

        [Description("Approved")] Approved = 1,

        [Description("Rejected")] Rejected = 2,
    }
}