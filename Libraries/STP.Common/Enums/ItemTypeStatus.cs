using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum ItemTypeStatus
    {
        [Description("proposal")] proposal = 312001,
        [Description("reproposal")] reproposal = 312002,
        [Description("withdrawl")] withdrawl = 312003,
        [Description("declination")] declination = 312004,
        [Description("agreement")] agreement = 312005,
        [Description("amendment to agreement")] amendment_to_agreement = 312006,


        [Description("recleared")] recleared = 312007,
        [Description("nolonger affected")] nolonger_affected = 312008,
        [Description("notification")] notification = 312009,
        [Description("renotification")] renotification = 312010,

        [Description("delivery failure")] delivery_failure = 312011,
        [Description("delegation failure alert")] delegation_failure_alert = 312012,
        [Description("vr1 planned route")] vr1_planned_route = 312013,
        [Description("ne notification")] ne_notification = 312014,
        [Description("ne renotification")] ne_renotification = 312015,
        [Description("ne notification api")] ne_notification_api = 312016,
        [Description("ne renotification api")] ne_re_notification_api = 312017,

        [Description("under assessment")] under_assessment = 327003,
        [Description("agreed")] agreed = 305004,
        [Description("Unplanned")] Unplanned = 911001
    }

    public enum ItemDocType
    {
        [Description("proposal")] doc001 = 322001,
        [Description("agreement")] doc002 = 322002,
        [Description("notification")] doc003 = 322003,
        [Description("daily_digest")] doc004 = 322004,
        [Description("route_alert")] doc005 = 322005,
        [Description("imminent_move_alert")] doc006 = 322006,
        [Description("no_longer_affected")] doc007 = 322007,
        [Description("failed_delegation_alert")] doc008 = 322008,
        [Description("movement_details")] doc009 = 322009,
        [Description("special_order")] doc010 = 322010,
        [Description("vr1_planned_route")] doc011 = 322011
    }

    public enum NeReport
    {
        [Description(null)] All_type = 12,
        [Description("241003")] STGO_CAT_1 = 1,//For 241003
        [Description("241004")] STGO_CAT_2 = 2,//For 241004
        [Description("241005")] STGO_CAT_3 = 3,//For 241005
        [Description("241009,241013,241014,241017,241018,241019,241020,241021,241022,241023,241024,241025")] STGO_Other = 4,//For 241009,241013,241014,241017,241018,241019,241020,241021,241022,241023,241024,241025
        [Description("241011")] C_and_U = 5,//For 241011
        [Description("241002")] Special_Orders = 6,//For 241002
        [Description("241003,241004,241005,241026,241027,241028,241029,241030,241031,241032,241033,241034")] VR1 = 7,//For 241003,241004,241005,241026,241027,241028,241029,241030,241031,241032,241033,241034 and requires vr1
        [Description("241006")] Mobile_Crane_CAT_A = 8,//For 241006
        [Description("241007")] Mobile_Crane_CAT_B = 9,//For 241007
        [Description("241008")] Mobile_Crane_CAT_C = 10,//For 241008
        [Description("241001,241012")] OtherNotifications = 11// For 241001,241012
    }
}