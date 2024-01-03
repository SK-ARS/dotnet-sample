#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum PersonalSections
    {
        [Description("PSSection1")] PSSectionMain = 0,

        [Description("PSSection2")] PSSectionOne = 1,

        [Description("PSSection3")] PSSectionTwo = 2,

        [Description("PSSection4")] PSSectionThree = 3
    }
}