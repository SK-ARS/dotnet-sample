#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum Relationship
    {
        [Description("Brother")] Brother = 1,

        [Description("Father")] Father = 2,

        [Description("Mother")] Mother = 3,

        [Description("Sister")] Sister = 4,
    }
}