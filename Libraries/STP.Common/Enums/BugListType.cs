using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum BugListType
    {
        [Description("With Bugs")]
        WithBugs = 1,
        [Description("Without Bugs")]
        WithoutBugs = 2
    }
}