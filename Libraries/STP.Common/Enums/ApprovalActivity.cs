#region

using System;
using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    [CLSCompliant(false)]
    public enum ApprovalActivity
    {
        [Description("None")] None = 0,

        [Description("Change Status")] ChangeStatus = 1,

        [Description("Check List")] CheckList = 2,

        [Description("Document Upload")] DocumentUpload = 3,

        [Description("File Upload")] FileUpload = 4,

        [Description("Log History")] LogHistory = 5,

        [Description("Notes")] Notes = 6,

        [Description("Forms")] Forms = 7,

        [Description("WelcomeKit")] WelcomeKit = 8,
    }
}