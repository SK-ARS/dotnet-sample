using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class AuditLogIdentifiers
    {
        public string NonEsdalUser { get; set; }
        public string NENVehicleName { get; set; }
        public string NENRouteName { get; set; }
        public string NENToScrutinyUser { get; set; }
        public string DateTime { get; set; }
        public string NENNotificationNo { get; set; }
        public string InboxItemId { get; set; }
        public AuditActionType AuditActionType { get; set; }
        public AuditLogIdentifiers()
        {
            AuditActionType = new AuditActionType();
        }

        public string ESDALNotificationNo { get; set; }

        public int HelpDeskUserID { get; set; }

        public long NENID { get; set; }

        public string CollabrationNotes { get; set; }

        public string NoteOnEscort { get; set; }

        public string InternalNotes { get; set; }

        public string HelpDeskUsername { get; set; }

        public long NotificationID { get; set; }

        public string RouteStatus { get; set; }

        public string NENFromUser { get; set; }

        public int MailedCollabration { get; set; }
    }
    public enum AuditActionType
    {
        soauser_opens_nen_notif = 499076,
        user_opens_general_tab,
        user_opens_route_tab,
        user_opens_veh_tab,
        soauser_edit_nen_route,
        soauser_view_nen_route_details,
        soauser_plan_nen_route,
        soauser_replan_nen_route,
        soauser_planing_error_nen_route,
        soauser_set_user_for_scrutiny,
        soauser_confirms_route_veh,
        soauser_save_collaboration_det,
        soauser_download_nen_pdf,
        soauser_set_accepted_collab,
        soauser_set_rejected_collab,
        soauser_set_underasmt_collab_with_scrutiny,
        soauser_opens_esdal_notif,
        Check_as_SOA,
        Check_as_Police,
        Save_Notes_On_Escort,
        Save_Internal_Notes,
        print_reduced_report_doc,
        soauser_assigned_scru_unplanned,
        soauser_set_noaction_collab,
        NEN_notification_confirmed_by_user
    }
}