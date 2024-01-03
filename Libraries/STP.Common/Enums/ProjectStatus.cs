using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum ProjectStatus
    {
        [Description("unallocated")] unallocated = 307001,
        [Description("work in progress")] wip = 307002,
        [Description("proposed")] proposed = 307003,
        [Description("reproposed")] reproposed = 307004,
        [Description("agreed")] agreed = 307005,
        [Description("agreed revised")] agreed_revised = 307006,
        [Description("agreed recleared")] agreed_recleared = 307007,
        [Description("withdrawn")] withdrawn = 307008,
        [Description("declined")] declined = 307009,
        [Description("historical")] historical = 307010,
        [Description("revised")] revised = 307011,
        [Description("agreement work in progress")] agreement_wip = 307012,
        [Description("agreement reclearance work in progress")] agreement_reclearance_wip = 307013,
        [Description("planned")] planned = 307014,
        [Description("allocated")] allocated = 307015,
        [Description("approved")] approved = 307016
    }

    public enum ProjectCheckingStatus
    {
        [Description("not checking")] not_checking = 301001,
        [Description("checking")] checking = 301002,
        [Description("checked positively")] checked_positive = 301003,
        [Description("checked negatively")] checked_negatively = 301004,
        [Description("Checking final")] checking_final = 301005,
        [Description("Checked final positively")] checking_final_positive = 301006,
        [Description("Checked final negatively")] checking_final_negative = 301007,
        [Description("QA Checking")] qa_checking = 301008,
        [Description("QA Checked Positively")] qa_check_positive = 301009,
        [Description("QA Checked Negatively")] qa_check_negative = 301010
    }
}
