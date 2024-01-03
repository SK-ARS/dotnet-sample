using System.ComponentModel;
namespace STP.Common.Enums
{
    public enum ApplicationStatus
    {
        [Description("work in progress")]
        app_wip = 308001,
        [Description("submitted")]
        submitted = 308002,
        [Description("received by ha")]
        received_by_NH = 308003,
        [Description("declined")]
        declined = 308004,
        [Description("withdrawn")]
        withdraw = 308005,
        [Description("historical")]
        historica = 308006,
        [Description("approved")]
        approved = 308007
    }
}