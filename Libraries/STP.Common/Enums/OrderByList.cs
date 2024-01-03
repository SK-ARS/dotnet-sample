#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum OrderByList
    {
        [Description("Name")] Name = 1,

        [Description("Number")] Number = 2,

        [Description("Description")] Description = 3
    }
}