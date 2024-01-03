using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    public class AuditLogIdentifiers
    {
        public string nonEsdalUser { get; set; }
        public string nenVehicleName { get; set; }
        public string nenRouteName { get; set; }
        public string nenToScrutinyUser { get; set; }
        public string datetime { get; set; }
        public string nenNotifNo { get; set; }
        public string inboxItemId { get; set; }
        public AuditActionType auditActionType { get; set; }
        public AuditLogIdentifiers()
        {
            auditActionType = new AuditActionType();
        }

        public string esdalNotifNo { get; set; }

        public int helpDeskUser_ID { get; set; }

        public long NEN_ID { get; set; }

        public string collabNotes { get; set; }

        public string noteOnEscort { get; set; }

        public string internalNotes { get; set; }

        public string helpDeskUsername { get; set; }

        public long notificationID { get; set; }

        public string RouteStatus { get; set; }

        public string nenFromUser { get; set; }

        public int MAILED_COLLAB { get; set; }
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