#region

using System;
using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    [CLSCompliant(false)]
    public enum ApplicationMode   
    {
        [Description("Add")] Add = 1,
        [Description("Edit")] Edit = 2,
        [Description("Update")] Update = 3,

    }
}