using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Web.General
{
    public class AuditLog
    {
        AuditLog()
        {
            //default constructor
        }

        public static string GetNENNotifAuditLog(UserInfo userInfo, AuditLogIdentifiers ObjAudLog, out string ErrMsg)
        {
            string audit_message = string.Empty;
            string portalName = string.Empty;
            ErrMsg = "No Data Found";
            try
            {
                portalName = GetPortalName(userInfo.UserTypeId);
                audit_message = GetAuditDescription(portalName, userInfo, ObjAudLog);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
            return audit_message;
        }
        private static string GetPortalName(int UsertypeId)
        {
            string err = string.Empty;
            string portalName = string.Empty;
            try
            {
                switch (UsertypeId)
                {
                    case 696001:
                        portalName = "hauliers portal";
                        break;
                    case 696002:
                        portalName = "police portal";
                        break;
                    case 696003:
                        portalName = "ops portal";
                        break;
                    case 696004:
                        portalName = "mis portal";
                        break;
                    case 696005:
                        portalName = "public portal";
                        break;
                    case 696006:
                        portalName = "cm admin portal";
                        break;
                    case 696007:
                        portalName = "soa portal";
                        break;
                    case 696008:
                        portalName = "sort portal";
                        break;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return portalName;
        }
        private static string GetAuditDescription(string portalName, UserInfo userInfo, AuditLogIdentifiers ObjAudLog)
        {
            string err = string.Empty;
            string AuditDescription = string.Empty;
            string helpDeskUserstring = "";
            string mailedColaboration = "";
            if (userInfo.HelpdeskLoginAsAnotherUser == true)
            {
                //helpDeskUserstring = "(Logged in by helpdesk User " + ObjAudLog.HelpDeskUsername + ")";
            }
            if (ObjAudLog.MailedCollabration == 2 || ObjAudLog.MailedCollabration == 3)
            {
                mailedColaboration = "(mailed collaboration)";
            }
            try
            {
                switch (ObjAudLog.AuditActionType)
                {
                    case AuditActionType.Check_as_SOA:
                        AuditDescription = "ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' is opened by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        break;
                    case AuditActionType.Check_as_Police:
                        AuditDescription = "ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' is opened by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        break;
                    case AuditActionType.Save_Notes_On_Escort:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has saved notes on escort '" + ObjAudLog.NoteOnEscort + "' for NE notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has saved notes on escort '" + ObjAudLog.NoteOnEscort + "' for ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has saved notes on escort '" + ObjAudLog.NoteOnEscort + "' for NE notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has saved notes on escort '" + ObjAudLog.NoteOnEscort + "' for ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.Save_Internal_Notes:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has saved internal notes '" + ObjAudLog.InternalNotes + "' for NE notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has saved internal notes '" + ObjAudLog.InternalNotes + "' for ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has saved internal notes '" + ObjAudLog.InternalNotes + "' for NE notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has saved internal notes '" + ObjAudLog.InternalNotes + "' for ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.soauser_opens_nen_notif:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "NE notification '" + ObjAudLog.NENNotificationNo + "' is opened by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "NE notification '" + ObjAudLog.NENNotificationNo + "' is opened by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.NEN_notification_confirmed_by_user:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "NE notification '" + ObjAudLog.NENNotificationNo + "'s route and vehicle details are confirmed by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "NE notification '" + ObjAudLog.NENNotificationNo + "'s route and vehicle details are confirmed by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.soauser_edit_nen_route:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route is edited inside NE notification '" + ObjAudLog.NENNotificationNo + "' by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route is edited inside NE notification '" + ObjAudLog.NENNotificationNo + "' by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.soauser_view_nen_route_details:
                        AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route is viewed inside NE notification '" + ObjAudLog.NENNotificationNo + "' by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        break;
                    case AuditActionType.soauser_plan_nen_route:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route is planned inside NE notification '" + ObjAudLog.NENNotificationNo + "' when opened by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route is planned inside NE notification '" + ObjAudLog.NENNotificationNo + "' when opened by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.soauser_replan_nen_route:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route is replanned inside NE notification '" + ObjAudLog.NENNotificationNo + "' when opened by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route is replanned inside NE notification '" + ObjAudLog.NENNotificationNo + "' when opened by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.soauser_assigned_scru_unplanned:
                        AuditDescription = "NE notification '" + ObjAudLog.NENNotificationNo + "' with route status '" + ObjAudLog.RouteStatus + "' is sent for scrutiny from '" + ObjAudLog.NENFromUser + "' to  '" + ObjAudLog.NENToScrutinyUser + "' by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        break;
                    case AuditActionType.soauser_planing_error_nen_route:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route planning error inside NE notification '" + ObjAudLog.NENNotificationNo + "' when opened by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "'" + ObjAudLog.NENRouteName + "' Route planning error inside NE notification '" + ObjAudLog.NENNotificationNo + "' when opened by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.soauser_set_user_for_scrutiny:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "NE notification '" + ObjAudLog.NENNotificationNo + "' with route status '" + ObjAudLog.RouteStatus + "' is sent for scrutiny from '" + ObjAudLog.NENFromUser + "' to '" + ObjAudLog.NENToScrutinyUser + "' by soa user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "NE notification '" + ObjAudLog.NENNotificationNo + "' with route status '" + ObjAudLog.RouteStatus + "' is sent for scrutiny from '" + ObjAudLog.NENFromUser + "' to '" + ObjAudLog.NENToScrutinyUser + "' by police user '" + userInfo.UserName + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.soauser_confirms_route_veh:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "SOA user '" + userInfo.UserName + "' has confirmed route and vehicle details for NE notification '" + ObjAudLog.NENNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "Police user '" + userInfo.UserName + "' has confirmed route and vehicle details for NE notification '" + ObjAudLog.NENNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.soauser_set_accepted_collab:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' to accepted  and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' to accepted  and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' to accepted and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' to accepted and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.soauser_set_rejected_collab:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' to rejected and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' to rejected and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' to rejected and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' to rejected and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.soauser_set_underasmt_collab_with_scrutiny:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' to under assessment and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' and assigned for scrutiny to '" + ObjAudLog.NENToScrutinyUser + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' to under assessment and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' and assigned for scrutiny to '" + ObjAudLog.NENToScrutinyUser + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' to under assessment and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' and assigned for scrutiny to '" + ObjAudLog.NENToScrutinyUser + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' to under assessment and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' and assigned for scrutiny to '" + ObjAudLog.NENToScrutinyUser + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.soauser_set_noaction_collab:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' no action status for collaboration and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' no action status for collaboration and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set NE notification '" + ObjAudLog.ESDALNotificationNo + "' no action status for collaboration and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' set ESDAL notification '" + ObjAudLog.ESDALNotificationNo + "' no action status for collaboration and saved collaboration notes '" + ObjAudLog.CollabrationNotes + "' on '" + ObjAudLog.DateTime + "'" + mailedColaboration + " ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.soauser_download_nen_pdf:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has downloaded pdf document for NE notification '" + ObjAudLog.NENNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has downloaded pdf document for ESDAL notification '" + ObjAudLog.NENNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has downloaded pdf document for NE notification '" + ObjAudLog.NENNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has downloaded pdf document for ESDAL notification '" + ObjAudLog.NENNotificationNo + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.print_reduced_report_doc:
                        if (userInfo.UserTypeId == 696007)
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has printed reduced document report for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "SOA user '" + userInfo.UserName + "' has printed reduced document report for ESDAL notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        else
                        {
                            if (ObjAudLog.NENID > 0)
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has printed reduced document report for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                            else
                            {
                                AuditDescription = "Police user '" + userInfo.UserName + "' has printed reduced document report for ESDAL notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                            }
                        }
                        break;
                    case AuditActionType.user_opens_general_tab:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "SOA user '" + userInfo.UserName + "' has opened general tab for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "Police user '" + userInfo.UserName + "' has opened general tab for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.user_opens_route_tab:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "SOA user '" + userInfo.UserName + "' has opened route tab for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "Police user '" + userInfo.UserName + "' has opened route tab for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                    case AuditActionType.user_opens_veh_tab:
                        if (userInfo.UserTypeId == 696007)
                        {
                            AuditDescription = "SOA user '" + userInfo.UserName + "' has opened vehicle tab for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        else
                        {
                            AuditDescription = "Police user '" + userInfo.UserName + "' has opened vehicle tab for NE notification '" + ObjAudLog.NENNotificationNo + "' notification Id :'" + ObjAudLog.NotificationID + "' on '" + ObjAudLog.DateTime + "' ." + helpDeskUserstring;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return AuditDescription;
        }
    }

}