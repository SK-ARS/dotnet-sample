#region

using System;
using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    [CLSCompliant(false)]
    public enum ApplicationType
    {
        [Description("Default")] Default = 0,

        [Description("New Applicant")] NewApplicant = 1,

        [Description("Change Of Ownership")] ChangeOfOwnership = 2,

        [Description("Change Of Location")] ChangeOfLocation = 3,

        [Description("Adding Location")] AddingLocation = 4
    }
}