using System.ComponentModel;
namespace STP.Common.Enums
{
    public enum VersionStatus
    {
        [Description("work in progress")]
        wip = 305001,
        [Description("proposed")]
        proposed = 305002,
        [Description("reproposed")]
        reproposed = 305003,
        [Description("agreed")]
        agreed = 305004,
        [Description("agreed revised")]
        agreed_revised = 305005,
        [Description("agreed recleared")]
        agreed_recleared = 305006,
        [Description("withdrawn")]
        withdrawn = 305007,
        [Description("declined")]
        declined = 305008,
        [Description("historical")]
        historical = 305009,
        [Description("revised")]
        revised = 305010,
        [Description("agreement work in progress")]
        agreement_wip = 305011,
        [Description("agreement reclearance work in progress")]
        agreement_reclearance_wip = 305012,
        [Description("planned")]
        planned = 305013,
        [Description("approved")]
        approved = 305014
    }
}
