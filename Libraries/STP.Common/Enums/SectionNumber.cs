#region

using System;
using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    [CLSCompliant(false)]
    public enum SectionNumber
    {
        [Description("None")] None = 0,

        [Description("One")] BSSectionOne = 1,

        [Description("Two")] BSSectionTwo = 2,

        [Description("Three")] BSSectionThree = 3,

        [Description("Four")] BSSectionFour = 4,

        [Description("Main")] PSSectionMain = 5,

        [Description("One")] PSSectionOne = 6,

        [Description("Two")] PSSectionTwo = 7,

        [Description("Three")] PSSectionThree = 8,
    }
}