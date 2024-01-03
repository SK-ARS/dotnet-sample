using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    public class System_Events
    {
        public System_Events()
        {
        }

        /// <summary>
        /// GetSysEventString
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ObjMovAct"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static string GetSysEventString(UserInfo userInfo, MovementActionIdentifiers ObjMovAct, out string ErrMsg)
        {
            string sysevent_description = string.Empty;
            string portalName = string.Empty;
            ErrMsg = "No Data Found";
            try
            {
                portalName = GetPortalName(userInfo.userTypeId);
                sysevent_description = GetSystemEvents(portalName, userInfo, ObjMovAct);
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                ErrMsg = ex.Message;
            }
            return sysevent_description;
        }

        private static string GetSystemEvents(string portalName, UserInfo userInfo, MovementActionIdentifiers ObjMovAct)
        {
            string err = string.Empty;
            string Sys_Events_Descp = string.Empty;
            string helpDeskUserstring = "";
            if (userInfo.helpdestLoginAsAnotherUser == true)
            {
                helpDeskUserstring = "(Logged in by helpdesk User " + userInfo.helpdesk_username + "(" + userInfo.Helpdesk_userID + "))";
            }
            try
            {
                switch (ObjMovAct.systemEventType)
                {
                    case SysEventType.portal_startup://portal startup
                        Sys_Events_Descp = "Portal started up, version: Portal " + portalName + " 2-004 startup" + helpDeskUserstring;
                        break;
                    case SysEventType.portal_shutdown://portal shutdown
                        Sys_Events_Descp = "Portal " + portalName + " 2-003 shutdown" + helpDeskUserstring;
                        break;
                    case SysEventType.portal_successful_login://portal successful log in
                        Sys_Events_Descp = "User " + userInfo.UserName + " Logged In" + helpDeskUserstring;
                        break;
                    case SysEventType.portal_failed_login://portal failed log in
                        Sys_Events_Descp = "User Login Failed, UserName: " + userInfo.UserName + ", Reason: Unknown Username" + helpDeskUserstring;
                        break;
                    case SysEventType.portal_user_logged_out://portal user logged out
                        Sys_Events_Descp = "User " + userInfo.UserName + " Logged out" + helpDeskUserstring;
                        break;
                    //------------------------SORT side------------------------------
                    case SysEventType.sort_successful_login://sort successful log in
                        Sys_Events_Descp = "User " + userInfo.UserName + " Logged In" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_failed_login://sort failed log in
                        Sys_Events_Descp = "User Login Failed, UserName: " + userInfo.UserName + ", Reason: Unknown Username" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_user_loggedout://sort user logged out
                        Sys_Events_Descp = "SORT user logged out" + helpDeskUserstring;
                        break;
                    //---------------------------------------------------------------
                    //401009
                    #region Haulier SO and VR1 create application 27-12-2016
                    case SysEventType.password_expired://password expired
                        Sys_Events_Descp = "User " + userInfo.UserName + " Password Expired" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_saved_so_application:
                        Sys_Events_Descp = "(" + userInfo.UserName + ") Haulier saved SO application revision id: " + ObjMovAct.revision_id + "" + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_saved_vr1_application:
                        Sys_Events_Descp = "(" + userInfo.UserName + ") Haulier saved VR1 application revision id: " + ObjMovAct.revision_id + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_submitted_vr1_application://haulier submitted application
                        Sys_Events_Descp = "(" + userInfo.UserName + ") Haulier submitted VR1 ApplicationId: " + ObjMovAct.revision_id + ", Application ESDAL reference number: " + ObjMovAct.esdalRef + "" + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_withdrew_application://haulier withdrew application
                        Sys_Events_Descp = "EMRN:" + ObjMovAct.esdalRef + "" + helpDeskUserstring;
                        break;

                    #endregion Haulier SO and VR1 create application 27-12-2016
                    //----------------------------SORT Side-------------------------------
                    case SysEventType.sort_declined_application://sort declined application
                        Sys_Events_Descp = "EMRN:" + ObjMovAct.esdalRef + ", Reason:Piece is to be scrapped. " + ObjMovAct.Org_name + " does not permit for loads for scrapping to be moved by road under Special Order license." + helpDeskUserstring;
                        break;
                    case SysEventType.sort_submitted_application://sort submitted application
                        Sys_Events_Descp = "ApplicationId: " + ObjMovAct.App_ID + ", Application ESDAL reference number: " + ObjMovAct.esdalRef + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_withdrew_application://sort withdrew application
                        Sys_Events_Descp = "EMRN:" + ObjMovAct.esdalRef + "" + helpDeskUserstring;
                        break;
                    //--------------------------------------------------------------------
                    case SysEventType.nack_message_received://nack message received
                        Sys_Events_Descp = "The message id 0000004585001194351147172 has failed acknowledgement" + helpDeskUserstring;
                        break;
                    case SysEventType.portal_user_accepted_TsAndCs://portal user accepted TsAndCs
                        Sys_Events_Descp = "UserId: " + userInfo.UserID + " Accepted Terms and Conditions" + helpDeskUserstring;
                        break;
                    case SysEventType.content_published://content published
                        Sys_Events_Descp = "High and Heavy Load Grids Map Item, download, Version 2" + helpDeskUserstring;
                        break;
                    case SysEventType.content_rollbacked://content rollbacked
                        Sys_Events_Descp = "What's New in ESDAL (Hauliers) Item, news story, Version 5" + helpDeskUserstring;
                        break;
                    case SysEventType.content_updated://content updated
                        Sys_Events_Descp = "Information on the " + ObjMovAct.Org_name + " team Item, info story, Version 4" + helpDeskUserstring;
                        break;
                    case SysEventType.content_retrieved://content retrieved
                        Sys_Events_Descp = "Operating Guidance Self Escorting Item, download, Version 1" + helpDeskUserstring;
                        break;
                    case SysEventType.content_retracted://content retracted
                        Sys_Events_Descp = "Top Tips for Structure Owners Issue 1 Item, info story, Version 1" + helpDeskUserstring;
                        break;
                    case SysEventType.document_downloaded://document downloaded
                        Sys_Events_Descp = "Downloaded: Form of Notice to Police and to Highway and Bridges Authorities.pdf, Item Id: 6, Version: 3" + helpDeskUserstring;
                        break;
                    case SysEventType.distribution_failure://distribution failure
                        Sys_Events_Descp = "Failed to add the movement version " + ObjMovAct.esdalRef + "" + helpDeskUserstring;
                        break;
                    case SysEventType.access_status_altered://access status altered
                        Sys_Events_Descp = "The following users have been disabled: " + ObjMovAct.UserName + " (" + ObjMovAct.User_ID + ")" + helpDeskUserstring;
                        break;
                    case SysEventType.added_contact://added contact
                        Sys_Events_Descp = "Added contact: " + ObjMovAct.contactName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.added_organisation://added organisation
                        Sys_Events_Descp = "Organisation added: " + ObjMovAct.Org_name + "" + helpDeskUserstring;
                        break;
                    case SysEventType.added_user://added user
                        Sys_Events_Descp = "An account for " + ObjMovAct.UserName + " has been created. The user is associated to contact " + ObjMovAct.contactName + " ( " + ObjMovAct.CONTACT_ID + " )." + helpDeskUserstring;
                        break;
                    case SysEventType.admin_status_altered://admin status altered
                        Sys_Events_Descp = "The following users have been enabled as administrators: " + ObjMovAct.UserName + "* (" + ObjMovAct.User_ID + "), grivers-mis*(23431) and the following users have been disabled as administrators: grivers-ops*(23427)" + helpDeskUserstring;
                        break;
                    case SysEventType.amended_contact://amended contact
                        Sys_Events_Descp = "Contact ID: " + ObjMovAct.CONTACT_ID + ", Name: " + ObjMovAct.contactName + " has been amended" + helpDeskUserstring;
                        break;
                    case SysEventType.amended_organisation://amended organisation
                        Sys_Events_Descp = "Organisation amended: " + ObjMovAct.Org_name + "" + helpDeskUserstring;
                        break;
                    case SysEventType.amended_user://amended user
                        Sys_Events_Descp = "User " + ObjMovAct.UserName + " has been amended " + helpDeskUserstring;
                        break;
                    case SysEventType.decline_preregistration://decline preregistration
                        Sys_Events_Descp = "The following preregistrations have been deleted: '" + ObjMovAct.Org_name + "' (" + ObjMovAct.org_id + "); 0 registration(s) failed to delete" + helpDeskUserstring;
                        break;
                    case SysEventType.decline_registration://decline registration
                        Sys_Events_Descp = "The following registrations have been declined: '" + ObjMovAct.Org_name + "' (" + ObjMovAct.org_id + "); 0 registration(s) failed to delete" + helpDeskUserstring;
                        break;
                    case SysEventType.deleted_contacts://deleted contacts
                        Sys_Events_Descp = "The following contacts have been deleted: " + ObjMovAct.contactName + "(" + ObjMovAct.contactName + ")" + helpDeskUserstring;// - ESDAL Curtis Wilson
                        break;
                    case SysEventType.deleted_organisations://deleted organisations
                        Sys_Events_Descp = "Organisations deleted. (Successes=1 ['" + ObjMovAct.Org_name + "' (" + ObjMovAct.org_id + ")]; Failures=0)" + helpDeskUserstring;
                        break;
                    case SysEventType.deleted_users://deleted users
                        Sys_Events_Descp = "Users: " + ObjMovAct.UserName + " ( " + ObjMovAct.User_ID + " )  have been deleted." + helpDeskUserstring;
                        break;
                    case SysEventType.mis_daily_stats_collected://mis daily stats collected
                        Sys_Events_Descp = "Collected daily MIS statistics" + helpDeskUserstring;
                        break;
                    case SysEventType.mis_stats_collection_failure://mis stats collection failure
                        Sys_Events_Descp = "Collection of MIS statistics failed - Failed to update maps displayed statistics - com.serco.esdal.common.interfaces.exceptions.BusinessLogicException" + helpDeskUserstring;
                        break;
                    case SysEventType.password_changed://password changed
                        Sys_Events_Descp = "The password for user " + userInfo.UserName + " ( " + userInfo.UserID + " ) has been changed" + helpDeskUserstring;
                        break;
                    case SysEventType.portal_migration://portal migration
                        Sys_Events_Descp = "Users have been migrated to Portal hauliers-rf v2-001. (Successes=10; Failures=0)" + helpDeskUserstring;
                        break;
                    case SysEventType.preregistration://preregistration
                        Sys_Events_Descp = "Pre-registration has been created for organisation: " + ObjMovAct.org_id + ", contact: " + ObjMovAct.CONTACT_ID + helpDeskUserstring;
                        break;
                    case SysEventType.registration_request://registration request
                        Sys_Events_Descp = "Registering '" + portalName + "' : " + ObjMovAct.contactName + ", from organisation: " + ObjMovAct.Org_name + "" + helpDeskUserstring;
                        break;
                    case SysEventType.reset_password://reset password
                        Sys_Events_Descp = " Users: " + ObjMovAct.UserName + " ( " + ObjMovAct.User_ID + " )  have had their password succesfully reset. " + helpDeskUserstring;
                        break;
                    case SysEventType.sanction_preregistration://sanction preregistration
                        Sys_Events_Descp = "PreRegistration request id: 3 has been sanctioned, contact id: " + ObjMovAct.CONTACT_ID + ", new username: " + userInfo.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sanction_registration://sanction registration
                        Sys_Events_Descp = "Registration request id: 283 has had his/her registration sanctioned, contact id: " + ObjMovAct.CONTACT_ID + ", new username: " + userInfo.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.publish_full_dataset://publish full dataset
                        Sys_Events_Descp = "Full import from dataset: 524" + helpDeskUserstring;
                        break;
                    case SysEventType.publish_outline_dataset://publish outline dataset
                        Sys_Events_Descp = "Outline import from dataset: 701" + helpDeskUserstring;
                        break;
                    //----------------------------SORT Side-------------------------------Added on 20 Feb 2015
                    #region SORT SO and VR1 create application 20-02-2015
                    case SysEventType.sort_saved_so_application:
                        if (ObjMovAct.AllocateToName != null)
                        {
                            Sys_Events_Descp = "Sort saving SO application revision id: " + ObjMovAct.revision_id + ",Allocate To: " + ObjMovAct.AllocateToName + ", Organisation Name: " + ObjMovAct.Org_name + ", user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        else
                        {
                            Sys_Events_Descp = "Sort saving SO application revision id: " + ObjMovAct.revision_id + ", Organisation Name: " + ObjMovAct.Org_name + ", user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        break;
                    case SysEventType.sort_created_new_vehicle_for_so_application:
                        Sys_Events_Descp = "Sort created new vehicle in SO application revision id: " + ObjMovAct.revision_id + ", vehicle id:" + ObjMovAct.vehicle_id + ", user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_created_new_configuration_for_so_application:
                        Sys_Events_Descp = "Sort created new configuration in SO application revision id: " + ObjMovAct.revision_id + ", vehicle id:" + ObjMovAct.vehicle_id + ", user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_imported_component_for_so_application:
                        Sys_Events_Descp = "Sort imported component for so application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + ", vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_created_component_for_so_application:
                        Sys_Events_Descp = "Sort created new component for SO application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + ", vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_created_axle_for_so_application:
                        Sys_Events_Descp = "Sort created Axle for SO application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + ", user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_added_component_to_fleet_for_so_application:
                        Sys_Events_Descp = "Sort added component in fleet for SO application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_edited_component_for_so_application:
                        Sys_Events_Descp = "Sort edit component for SO application component id:" + ObjMovAct.component_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_replaced_component_for_so_application:
                        Sys_Events_Descp = "Sort replace component in SO application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_edited_vehicle_for_so_application:
                        Sys_Events_Descp = "Sort edit vehicle in SO application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_deleted_vehicle_for_so_application:
                        Sys_Events_Descp = "Sort delete vehicle in SO application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_added_new_vehicle_to_fleet_for_so_application:
                        Sys_Events_Descp = "Sort added new vehicle in fleet for SO application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_copied_vehicle_for_so_application:
                        Sys_Events_Descp = "Sort copied vehicle in SO application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_imported_vehicle_from_fleet_for_so_application:
                        Sys_Events_Descp = "Sort imported vehicle from fleet for so application fleet vehicle id: " + ObjMovAct.fleet_veh_id + " revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_imported_vehicle_from_previous_movement_for_so_application:
                        Sys_Events_Descp = "Sort imported vehicle from previous movement for so application Previous ESDAL Ref No:" + ObjMovAct.esdalRef + " revision id: " + ObjMovAct.revision_id + " , Previous App Vehicle Id:" + ObjMovAct.prev_mov_veh_id + " , Current App Vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_new_route_by_textual_description_for_so_application:
                        Sys_Events_Descp = "Sort created route by textual descruption for so application route id: " + ObjMovAct.route_id + " revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_new_route_by_start_and_end_point_for_so_application:
                        Sys_Events_Descp = "Sort created route by start and end point for so application route id: " + ObjMovAct.route_id + " revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_new_route_by_map_for_sort_so_application:
                        Sys_Events_Descp = "Sort created route by Map for so application route id: " + ObjMovAct.route_id + " revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_chose_existing_route_from_library_for_so_application:
                        Sys_Events_Descp = "Sort chooses existing route from library for so application libaray route id: " + ObjMovAct.lib_route_id + ",route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_imported_route_from_previous_movement_for_so_application:
                        Sys_Events_Descp = "Sort importing route from previous movement for so application Previous ESDAL Ref No:" + ObjMovAct.esdalRef + " , Previous App route id: " + ObjMovAct.prev_mov_route_id + " , Current App route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_edited_route_for_so_application:
                        Sys_Events_Descp = "Sort edit route for so application route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_deleted_route_for_so_application:
                        Sys_Events_Descp = "Sort deleted route for so application route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_added_to_route_library_for_so_application:
                        Sys_Events_Descp = "Sort added route to route library for so application route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_submitted_so_application:
                        if (ObjMovAct.Edit_Flag == 0)
                        {
                            Sys_Events_Descp = "Sort submitted SO application ESDAL no:" + ObjMovAct.esdalRef + " ,revision id:" + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        else
                        {
                            Sys_Events_Descp = "Sort edited and submitted SO application ESDAL no:" + ObjMovAct.esdalRef + " ,revision id:" + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        break;
                    case SysEventType.Sort_saved_vr1_application:
                        Sys_Events_Descp = "Sort saved VR1 application revision id: " + ObjMovAct.revision_id + " ,organisation id:" + ObjMovAct.org_id + ", user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_new_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Sort created new vehicle in VR1 application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_configuration_for_vr1_application:
                        Sys_Events_Descp = "Sort created new configuration in VR1 application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_imported_component_for_vr1_application:
                        Sys_Events_Descp = "Sort imported component for VR1 application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_component_for_vr1_application:
                        Sys_Events_Descp = "Sort created new component for VR1 application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_axle_for_vr1_application:
                        Sys_Events_Descp = "Sort created Axle for VR1 application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_added_component_to_fleet_for_vr1_application:
                        Sys_Events_Descp = "Sort added component in fleet for VR1 application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_edited_component_for_vr1_application:
                        Sys_Events_Descp = "Sort edit component for VR1 application component id:" + ObjMovAct.component_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_replaced_component_for_vr1_application:
                        Sys_Events_Descp = "Sort replace component in VR1 application component id:" + ObjMovAct.component_id + ", revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_edited_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Sort edit vehicle in VR1 application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_deleted_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Sort delete vehicle in VR1 application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_added_vehicle_to_fleet_for_vr1_application:
                        Sys_Events_Descp = "Sort added new vehicle in fleet for VR1 application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_copied_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Sort copied vehicle in VR1 application revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_imported_vehicle_from_fleet_for_vr1_application:
                        Sys_Events_Descp = "Sort imported_vehicle from fleet for VR1 application fleet vehicle id: " + ObjMovAct.fleet_veh_id + " revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_imported_vehicle_from_previous_movement_for_vr1_application:
                        Sys_Events_Descp = "Sort imported_vehicle from previous movement for so application ESDAL no:" + ObjMovAct.esdalRef + " revision id: " + ObjMovAct.revision_id + " , vehicle id:" + ObjMovAct.vehicle_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_new_route_by_textual_description_for_vr1_application:
                        Sys_Events_Descp = "Sort created route by textual descruption for VR1 application route id: " + ObjMovAct.route_id + " revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_a_new_route_by_start_and_end_point_for_vr1_application:
                        Sys_Events_Descp = "Sort created route by start and end point for VR1 application route id: " + ObjMovAct.route_id + " revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_created_a_new_route_by_map_for_vr1_application:
                        Sys_Events_Descp = "Sort created route by Map for VR1 application route id: " + ObjMovAct.route_id + " revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_chose_existing_route_from_library_for_vr1_application:
                        Sys_Events_Descp = "Sort chooses existing route from library for VR1 application libaray route id: " + ObjMovAct.lib_route_id + ",route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring; break;
                    case SysEventType.Sort_imported_route_from_previous_movement_for_vr1_application:
                        Sys_Events_Descp = "Sort importing route from previous movement for VR1 application ESDAL no:" + ObjMovAct.esdalRef + " ,route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_edited_route_for_vr1_application:
                        Sys_Events_Descp = "Sort edit route for VR1 application route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_deleted_route_for_vr_application:
                        Sys_Events_Descp = "Sort deleted route for VR1 application route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_added_route_to_library_for_vr1_application:
                        Sys_Events_Descp = "Sort added route to route library for VR1 application route id: " + ObjMovAct.route_id + ", revision id: " + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Sort_submitted_vr1_application:
                        Sys_Events_Descp = "Sort submitted VR1 application ESDAL no:" + ObjMovAct.esdalRef + " ,revision id:" + ObjMovAct.revision_id + " , user name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.sort_edited_axle_details_for_so_component:
                        Sys_Events_Descp = "Sort edited axle details for so component : " + ObjMovAct.component_id + helpDeskUserstring;
                        break;
                    case SysEventType.sort_edited_axle_details_for_vr1_component:
                        Sys_Events_Descp = "Sort edited axle details for vr1 component : " + ObjMovAct.component_id + helpDeskUserstring;
                        break;

                    #endregion
                    //----------------------------end-------------------------------------20 Feb 2015


                    //----------------------------Haulier Side SO and VR1 applications-------------------------------Added on 23 Feb 2015
                    #region Haulier SO and VR1 Logs for Vehicle and Route
                    case SysEventType.Haulier_created_new_vehicle_for_so_application:
                        Sys_Events_Descp = "Haulier created new vehicle in SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_configuration_for_so_application:
                        Sys_Events_Descp = "Haulier created new configuration in SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_configuration_for_so_application:
                        Sys_Events_Descp = "Haulier edited configuration in SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_component_for_so_application:
                        Sys_Events_Descp = "Haulier imported component for SO application Component Id:" + ObjMovAct.component_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_component_for_so_application:
                        Sys_Events_Descp = "Haulier created new component for SO application Component Id:" + ObjMovAct.component_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_axle_for_so_application:
                        Sys_Events_Descp = "Haulier created Axle for SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_axle_for_so_application:
                        Sys_Events_Descp = "Haulier edited Axle for SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_added_component_to_fleet_for_so_application:
                        Sys_Events_Descp = "Haulier added component in fleet for SO application Component Id:" + ObjMovAct.component_id + " , Revision Id: " + ObjMovAct.revision_id + ", Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_component_for_so_application:
                        Sys_Events_Descp = "Haulier edit component for SO application Component Id:" + ObjMovAct.component_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_replaced_component_for_so_application:
                        Sys_Events_Descp = "Haulier replace component in SO application Component Id:" + ObjMovAct.component_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_vehicle_for_so_application:
                        Sys_Events_Descp = "Haulier edit vehicle in SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_deleted_vehicle_for_so_application:
                        Sys_Events_Descp = "Haulier delete vehicle in SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_added_vehicle_to_fleet_for_so_application:
                        Sys_Events_Descp = "Haulier added new vehicle in fleet for SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_copied_vehicle_for_so_application:
                        Sys_Events_Descp = "Haulier copied vehicle in SO application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_vehicle_from_fleet_for_so_application:
                        Sys_Events_Descp = "Haulier imported_vehicle from fleet for so application fleet Vehicle Id: " + ObjMovAct.fleet_veh_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_vehicle_from_previous_movement_for_so_application:
                        Sys_Events_Descp = "Haulier imported_vehicle from previous movement for so application Previous ESDAL Ref no:" + ObjMovAct.esdalRef + " , Revision Id: " + ObjMovAct.revision_id + " , Current App Vehicle Id:" + ObjMovAct.vehicle_id + " , Previous App Vehicle Id:" + ObjMovAct.prev_mov_veh_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_route_by_textual_description_for_so_application:
                        Sys_Events_Descp = "Haulier created route by textual descruption for so application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_route_by_start_and_end_point_for_so_application:
                        Sys_Events_Descp = "Haulier created route by start and end point for so application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_route_by_map_for_so_application:
                        Sys_Events_Descp = "Haulier created route by Map for so application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_existing_route_from_library:
                        Sys_Events_Descp = "Haulier chooses existing route from library for so application Libaray Route Id: " + ObjMovAct.lib_route_id + " , Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_route_from_previous_movement_for_so_application:
                        Sys_Events_Descp = "Haulier importing route from previous movement for so application Previous ESDAL Ref no:" + ObjMovAct.esdalRef + " , Current App Route Id: " + ObjMovAct.route_id + " , Previous App Route Id: " + ObjMovAct.prev_mov_route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_route_for_so_application:
                        Sys_Events_Descp = "Haulier edit route for so application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_deleted_route_for_so_application:
                        Sys_Events_Descp = "Haulier deleted route for so application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_added_route_to_library_for_so_application:
                        Sys_Events_Descp = "Haulier added route to route library for so application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_submitted_so_application:
                        Sys_Events_Descp = "Haulier submitted SO application ESDAL no:" + ObjMovAct.esdalRef + " , Revision Id:" + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_vehicle_for_vr1_application:
                        if (ObjMovAct.vehicle_id != 0)
                        {
                            Sys_Events_Descp = "Haulier created new vehicle in VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        else if (ObjMovAct.vehicle_id == 0)
                        {
                            Sys_Events_Descp = "Haulier created new vehicle in VR1 application Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        break;
                    case SysEventType.Haulier_created_configuration_for_vr1_application:
                        Sys_Events_Descp = "Haulier created new configuration in VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_configuration_for_vr1_application:
                        Sys_Events_Descp = "Haulier edited configuration in VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_component_for_vr1_application:
                        Sys_Events_Descp = "Haulier imported component for VR1 application Component Id:" + ObjMovAct.component_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_component_for_vr1_application:
                        Sys_Events_Descp = "Haulier created new component for VR1 application Component Id:" + ObjMovAct.component_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_axle_for_vr1_application:
                        Sys_Events_Descp = "Haulier created Axle for VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_axle_for_vr1_application:
                        Sys_Events_Descp = "Haulier edited Axle for VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_added_component_to_fleet_for_vr1_application:
                        Sys_Events_Descp = "Haulier added component in fleet for VR1 application Component Id:" + ObjMovAct.component_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_component_for_vr1_application:
                        Sys_Events_Descp = "Haulier edit component for VR1 application Component Id:" + ObjMovAct.component_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_replaced_component_for_vr1_application:
                        Sys_Events_Descp = "Haulier replace component in VR1 application Component Id:" + ObjMovAct.component_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Haulier edit vehicle in VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_deleted_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Haulier delete vehicle in VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_added_vehicle_to_fleet_for_vr1_application:
                        Sys_Events_Descp = "Haulier added new vehicle in fleet for VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_copied_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Haulier copied vehicle in VR1 application Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_vehicle_for_vr1_application:
                        Sys_Events_Descp = "Haulier imported_vehicle from fleet for VR1 application Fleet Vehicle Id: " + ObjMovAct.fleet_veh_id + " , Revision Id: " + ObjMovAct.revision_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_vehicle_from_previous_movment:
                        Sys_Events_Descp = "Haulier imported vehicle from previous movement for VR1 application Previous ESDAL Ref No:" + ObjMovAct.esdalRef + " , Revision Id: " + ObjMovAct.revision_id + " , Previous App Vehicle Id:" + ObjMovAct.prev_mov_veh_id + " , Current App Vehicle id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_route_by_textual_description_for_vr1_application:
                        Sys_Events_Descp = "Haulier created route by textual descruption for VR1 application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_route_by_start_and_end_point_for_vr1_application:
                        Sys_Events_Descp = "Haulier created route by start and end point for VR1 application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_route_by_map_for_vr1_application:
                        Sys_Events_Descp = "Haulier created route by Map for VR1 application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_chose_existing_route_from_library_for_vr1_application:
                        Sys_Events_Descp = "Haulier chooses existing route from library for VR1 application Libaray Route Id: " + ObjMovAct.lib_route_id + " , Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_route_from_previous_movement_for_vr1_application:
                        Sys_Events_Descp = "Haulier importing route from previous movement for VR1 application Previous ESDAL Ref no:" + ObjMovAct.esdalRef + " , Current App Route id: " + ObjMovAct.route_id + " , Previous App Route id: " + ObjMovAct.prev_mov_route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_route_for_vr1_application:
                        Sys_Events_Descp = "Haulier edit route for VR1 application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_deleted_route_for_vr1_application:
                        Sys_Events_Descp = "Haulier deleted route for VR1 application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_addd_route_to_library_for_vr1_application:
                        Sys_Events_Descp = "Haulier added route to route library for VR1 application Route Id: " + ObjMovAct.route_id + " , Revision Id: " + ObjMovAct.revision_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_added_registration_for_so_vehicle:
                        Sys_Events_Descp = "Haulier added registration for so application vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.sort_added_registration_for_so_vehicle:
                        Sys_Events_Descp = "Sort added registration for vr1 application vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_added_registration_for_vr1_vehicle:
                        Sys_Events_Descp = "Haulier added registration for vr1 application vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.sort_added_registration_for_vr1_vehicle:
                        Sys_Events_Descp = "Sort added registration for vr1 application vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_edited_axle_details_for_so_component:
                        Sys_Events_Descp = "Haulier edited axle details for so component : " + ObjMovAct.component_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_edited_axle_details_for_vr1_component:
                        Sys_Events_Descp = "Haulier edited axle details for vr1 component : " + ObjMovAct.component_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_registration_for_so_component:
                        Sys_Events_Descp = "Haulier deleted registration for so component : " + ObjMovAct.component_id + " with IdNo : " + ObjMovAct.component_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_registration_for_vr1_component:
                        Sys_Events_Descp = "Haulier deleted registration for vr1 component : " + ObjMovAct.component_id + " with IdNo : " + ObjMovAct.component_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.sort_deleted_registration_for_so_component:
                        Sys_Events_Descp = "Sort deleted registration for so component : " + ObjMovAct.component_id + " with IdNo : " + ObjMovAct.component_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.sort_deleted_registration_for_vr1_component:
                        Sys_Events_Descp = "Sort deleted registration for vr1 component : " + ObjMovAct.component_id + " with IdNo : " + ObjMovAct.component_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_registration_for_vr1_vehicle:
                        Sys_Events_Descp = "Haulier deleted registration for vr1 vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.sort_deleted_registration_for_vr1_vehicle:
                        Sys_Events_Descp = "Sort deleted registration for vr1 vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_registration_for_so_vehicle:
                        Sys_Events_Descp = "Haulier deleted registration for so vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.sort_deleted_registration_for_so_vehicle:
                        Sys_Events_Descp = "Sort deleted registration for so vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_notify_so_app://Haulier_notify_so_app
                        Sys_Events_Descp = "Haulier Notified so application, Revision Id: " + ObjMovAct.revision_id + ", Notification Id:" + ObjMovAct.NotificationID + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_revised_so_app://Haulier_revised_so_app
                        Sys_Events_Descp = "Haulier Revised so application, Revision Id: " + ObjMovAct.revision_id + ", New Revision Id:" + ObjMovAct.new_revision_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_cloned_so_app://Haulier_cloned_so_app
                        Sys_Events_Descp = "Haulier Cloned so application, Revision Id: " + ObjMovAct.revision_id + ", New Revision Id:" + ObjMovAct.new_revision_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_notify_vr1_app://Haulier_notify_vr1_app
                        Sys_Events_Descp = "Haulier Notified vr1 application, Revision Id: " + ObjMovAct.revision_id + ", Notification Id:" + ObjMovAct.NotificationID + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_revised_vr1_app://Haulier_revised_vr1_app
                        Sys_Events_Descp = "Haulier Revised vr1 application, Revision Id: " + ObjMovAct.revision_id + ", New Revision Id:" + ObjMovAct.new_revision_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_cloned_vr1_app://Haulier_cloned_vr1_app
                        Sys_Events_Descp = "Haulier Cloned vr1 application, Revision Id: " + ObjMovAct.revision_id + ", New Revision Id:" + ObjMovAct.new_revision_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    #endregion
                    //----------------------------end-------------------------------23 Feb 2015

                    //----------------------------Notification flow logs from haulier Vehicle and Route(Detailed)
                    #region Notification flow logs from haulier Vehicle and Route
                    case SysEventType.Haulier_created_notification://Haulier_created_notification
                        Sys_Events_Descp = "Haulier Created Notification, Notification Id : " + ObjMovAct.NotificationID + ", Project Id: " + ObjMovAct.ProjectId + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_submitted_notification://Haulier_submitted_notification
                        Sys_Events_Descp = "Haulier Submitted Notification, Notification Id : " + ObjMovAct.NotificationID + ", Project Id: " + ObjMovAct.ProjectId + ", Notification Code:" + ObjMovAct.notifCode + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_vehicle://Haulier_created_new_vehicle
                        Sys_Events_Descp = "Haulier Created New Vehicle, Notification Id :" + ObjMovAct.NotificationID + ", Vehicle Id:" + ObjMovAct.vehicle_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_vehicle_from_fleet://Haulier_imported_vehicle_from_fleet
                        Sys_Events_Descp = "Haulier Imported Vehicle from Fleet, Notification Id: " + ObjMovAct.NotificationID + ", Vehicle Id:" + ObjMovAct.vehicle_id + ", Fleet Vehicle Id:" + ObjMovAct.fleet_veh_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_vehicle_from_previous_movement://Haulier_imported_vehicle_from_previous_movement
                        Sys_Events_Descp = "Haulier Imported Vehicle from Previous Movement, Notification Id: " + ObjMovAct.NotificationID + ", Vehicle Id:" + ObjMovAct.vehicle_id + ", Previous Movement Vehicle Id:" + ObjMovAct.prev_mov_veh_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_deleted_vehicle://Haulier_deleted_vehicle
                        Sys_Events_Descp = "Haulier Deleted Vehicle in Notification, Notification Id: " + ObjMovAct.NotificationID + ", Vehicle Id:" + ObjMovAct.vehicle_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_vehicle://haulier editing notification's vehicle config
                        Sys_Events_Descp = "Haulier Edited Vehicle in Notification, Notification Id: " + ObjMovAct.NotificationID + ", Vehicle Id:" + ObjMovAct.vehicle_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_added_vehicle_to_fleet://Haulier_added_vehicle_to_fleet vehicle config
                        Sys_Events_Descp = "Haulier Added Vehicle to Fleet, Notification Id: " + ObjMovAct.NotificationID + ", Vehicle Id:" + ObjMovAct.vehicle_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_copied_vehicle://Haulier_copied_vehicle vehicle config
                        Sys_Events_Descp = "Haulier Copied Vehicle:, Notification Id: " + ObjMovAct.NotificationID + ", Vehicle Id:" + ObjMovAct.vehicle_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_new_route://Haulier_created_new_route
                        if (ObjMovAct.ret_route_id == 0)
                        {
                            Sys_Events_Descp = "Haulier Created New Route, Notification Id: " + ObjMovAct.NotificationID + ", Route Id:" + ObjMovAct.route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        else
                        {
                            Sys_Events_Descp = "Haulier Created Return Route, Notification Id: " + ObjMovAct.NotificationID + ", Route Id:" + ObjMovAct.ret_route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        break;
                    case SysEventType.Haulier_created_configuration_for_notification:
                        Sys_Events_Descp = "Haulier created new configuration for Notification, Notification Id: " + ObjMovAct.NotificationID + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_configuration_for_notification:
                        Sys_Events_Descp = "Haulier edited configuration for Notification, Notification Id: " + ObjMovAct.NotificationID + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_component_for_notification:
                        Sys_Events_Descp = "Haulier imported component for Notification, Component Id:" + ObjMovAct.component_id + " , Notification Id: " + ObjMovAct.NotificationID + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_component_for_notification:
                        Sys_Events_Descp = "Haulier created new component for Notification, Component Id:" + ObjMovAct.component_id + " , Notification Id: " + ObjMovAct.NotificationID + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_axle_for_notification:
                        Sys_Events_Descp = "Haulier created Axle for Notification, Notification Id: " + ObjMovAct.NotificationID + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_axle_for_notification:
                        Sys_Events_Descp = "Haulier eidted Axle for Notification, Notification Id: " + ObjMovAct.NotificationID + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_added_component_to_fleet_for_notification:
                        Sys_Events_Descp = "Haulier added component in fleet for Notification, Component Id:" + ObjMovAct.component_id + " , Notification Id: " + ObjMovAct.NotificationID + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_component_for_notification:
                        Sys_Events_Descp = "Haulier edit component for Notification, Notification Id: " + ObjMovAct.NotificationID + " , Component Id:" + ObjMovAct.component_id + " , Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_replaced_component_for_notification:
                        Sys_Events_Descp = "Haulier replace component for Notification, Notification Id: " + ObjMovAct.NotificationID + " , Component Id:" + ObjMovAct.component_id + " ,  Vehicle Id:" + ObjMovAct.vehicle_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_route_from_library://Haulier_imported_route_from_library
                        Sys_Events_Descp = "Haulier Imported Route from Library, Notification Id: " + ObjMovAct.NotificationID + ", Route Id:" + ObjMovAct.route_id + ", Library Route Id:" + ObjMovAct.lib_route_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_imported_route_from_previous_movement://Haulier_imported_route_from_previous_movement
                        Sys_Events_Descp = "Haulier Imported Route from Previous Movement, Notification Id: " + ObjMovAct.NotificationID + ", Route Id:" + ObjMovAct.route_id + ", Previous Movement Route Id:" + ObjMovAct.prev_mov_route_id + " , User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_deleted_route://Haulier_deleted_route
                        Sys_Events_Descp = "Haulier Deleted Route in Notification, Notification Id: " + ObjMovAct.NotificationID + ", Route Id:" + ObjMovAct.route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_route://Haulier_edited_route
                        if (ObjMovAct.ret_route_id == 0)
                        {
                            Sys_Events_Descp = "Haulier Edited Route in Notification, NotificationID: " + ObjMovAct.NotificationID + " , Route Id:" + ObjMovAct.route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        else
                        {
                            Sys_Events_Descp = "Haulier Edited Return Route in Notification, NotificationID: " + ObjMovAct.NotificationID + " , Route Id:" + ObjMovAct.ret_route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        break;
                    case SysEventType.Haulier_added_route_to_library://Haulier_added_route_to_library
                        if (ObjMovAct.NotificationID != 0)
                        {
                            Sys_Events_Descp = "Haulier Added Route to Library, Notification Id: " + ObjMovAct.NotificationID + ", Route Id:" + ObjMovAct.route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        else
                        {
                            Sys_Events_Descp = "Haulier Added Route to Library from Simple Notification, Route Id:" + ObjMovAct.route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        }
                        break;
                    case SysEventType.Haulier_notification_renotified://Haulier_notification_renotified
                        Sys_Events_Descp = "Haulier Notification Renotified Previous Esdal Ref No:" + ObjMovAct.esdalRef + ", Notification Id: " + ObjMovAct.NotificationID + ", New Notification Id:" + ObjMovAct.NewNotificationID + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_notification_cloned://Haulier_notification_renotified
                        Sys_Events_Descp = "Haulier Notification Cloned, Notification Id: " + ObjMovAct.NotificationID + ", New Notification Id:" + ObjMovAct.NewNotificationID + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_notifcation_revised://Haulier_notifcation_revised
                        Sys_Events_Descp = "Haulier Notification Revised, Notification Id: " + ObjMovAct.NotificationID + ", New Notification Id:" + ObjMovAct.NewNotificationID + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_notifcation_deleted://Haulier_notifcation_deleted
                        Sys_Events_Descp = "Haulier Notification Deleted, Notification Id: " + ObjMovAct.NotificationID + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    #endregion
                    //------------------------------end------------------------------------------------------------------Added on 6-01-17

                    //----------------------------Haulier Side fleet vehicle management-------------------------------Added on 24 Feb 2015
                    #region Haulier Fleet vehicle management
                    case SysEventType.haulier_created_fleet_component:
                        Sys_Events_Descp = "Haulier created fleet component " + ObjMovAct.fleet_comp_name + " with id : " + ObjMovAct.fleet_comp_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_edited_fleet_component:
                        Sys_Events_Descp = "Haulier edited fleet component " + ObjMovAct.fleet_comp_name + " with id : " + ObjMovAct.fleet_comp_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_fleet_component:
                        //Sys_Events_Descp = "Haulier deleted fleet component " + ObjMovAct.fleet_comp_name + " with id : " + ObjMovAct.fleet_comp_id + helpDeskUserstring;
                        Sys_Events_Descp = "Haulier deleted fleet component with id : " + ObjMovAct.fleet_comp_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_added_registration_for_fleet_component:
                        Sys_Events_Descp = "Haulier added registration for fleet component : " + ObjMovAct.fleet_comp_id + " with IdNo : " + ObjMovAct.fleet_comp_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_registration_for_fleet_component:
                        Sys_Events_Descp = "Haulier deleted registration for fleet component : " + ObjMovAct.fleet_comp_id + " with IdNo : " + ObjMovAct.fleet_comp_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_added_axle_details_for_fleet_component:
                        Sys_Events_Descp = "Haulier added axle details for fleet component : " + ObjMovAct.fleet_comp_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_edited_axle_details_for_fleet_component:
                        Sys_Events_Descp = "Haulier edited axle details for fleet component : " + ObjMovAct.fleet_comp_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_created_fleet_vehicle:
                        Sys_Events_Descp = "Haulier created fleet vehicle " + ObjMovAct.fleet_veh_name + " with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_edited_fleet_vehicle:
                        Sys_Events_Descp = "Haulier edited fleet vehicle " + ObjMovAct.fleet_veh_name + " with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_fleet_vehicle:
                        Sys_Events_Descp = "Haulier deleted fleet vehicle " + ObjMovAct.fleet_veh_name + " with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_added_fleet_component_as_config:
                        Sys_Events_Descp = "Haulier added fleet component with id : " + ObjMovAct.fleet_comp_id + " as a fleet vehicle with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_added_new_fleet_component:
                        Sys_Events_Descp = "Hailier added new fleet component with id : " + ObjMovAct.fleet_comp_id + " from fleet vehicle" + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_imported_component:
                        Sys_Events_Descp = "Haulier imported component with id : " + ObjMovAct.fleet_comp_id + "  to fleet vehicle with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_created_component:
                        Sys_Events_Descp = "Haulier created component with id : " + ObjMovAct.fleet_comp_id + "  to fleet vehicle with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_component:
                        Sys_Events_Descp = "Haulier edited component with id : " + ObjMovAct.fleet_comp_id + "  to fleet vehicle with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_component:
                        Sys_Events_Descp = "Haulier deleted component with id : " + ObjMovAct.fleet_comp_id + " from vehicle with id : " + ObjMovAct.fleet_veh_id + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_added_registration_for_fleet_vehicle:
                        Sys_Events_Descp = "Haulier added registration for fleet vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.haulier_deleted_registration_for_fleet_vehicle:
                        Sys_Events_Descp = "Haulier deleted registration for fleet vehicle : " + ObjMovAct.fleet_veh_id + " with IdNo : " + ObjMovAct.fleet_vhcl_id_no + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_replaced_component_for_fleet_vehicle:
                        Sys_Events_Descp = "Haulier replace component in fleet vehicle component id:" + ObjMovAct.component_id + " , vehicle id:" + ObjMovAct.fleet_veh_id + "" + helpDeskUserstring;
                        break;
                    #endregion
                    //----------------------------end-------------------------------24 Feb 2015

                    //----------------------------start-----------------------------Helpdesk distribution event 21-05-2015
                    #region Helpdesk distribution event
                    case SysEventType.Retransmitted_document:
                        Sys_Events_Descp = "Heldesk user :" + ObjMovAct.UserName + " Helpdesk user ID :" + ObjMovAct.User_ID + " retransmitted document :" + ObjMovAct.TransID + " for EsdalRef:" + ObjMovAct.esdalRef + "";
                        break;
                    case SysEventType.Retransmit_failed_document:
                        Sys_Events_Descp = "Heldesk user :" + ObjMovAct.UserName + " Helpdesk user ID :" + ObjMovAct.User_ID + " failed retransmitted document :" + ObjMovAct.TransID + " for EsdalRef:" + ObjMovAct.esdalRef + "";
                        break;
                    case SysEventType.View:
                        Sys_Events_Descp = "Heldesk user :" + ObjMovAct.UserName + " Helpdesk user ID :" + ObjMovAct.User_ID + " sending document from organisation name:" + ObjMovAct.Org_name_sender + " to organisation id :" + ObjMovAct.org_id_reciver + " to contact id :" + ObjMovAct.contact_id_reciver + " for EsdalRef:" + ObjMovAct.esdalRef + " and trans id :" + ObjMovAct.TransID + " and Doc type :" + ObjMovAct.DocType + "";
                        break;
                    case SysEventType.Check_as_Haulier:
                        Sys_Events_Descp = "Heldesk user :" + ObjMovAct.UserName + " Helpdesk user ID :" + ObjMovAct.User_ID + " logged in as username :" + userInfo.UserName + " and user id : " + userInfo.UserID + " for EsdalRef:" + ObjMovAct.esdalRef + "";
                        break;
                    case SysEventType.Check_as_SOA:
                        Sys_Events_Descp = "Heldesk user :" + ObjMovAct.UserName + " Helpdesk user ID :" + ObjMovAct.User_ID + " logged in as username :" + userInfo.UserName + " and user id : " + userInfo.UserID + " for EsdalRef:" + ObjMovAct.esdalRef + "";
                        break;
                    case SysEventType.Check_as_Police:
                        Sys_Events_Descp = "Heldesk user :" + ObjMovAct.UserName + " Helpdesk user ID :" + ObjMovAct.User_ID + " logged in as username :" + userInfo.UserName + " and user id : " + userInfo.UserID + " for EsdalRef:" + ObjMovAct.esdalRef + "";
                        break;
                    #endregion
                    //-----------------------------end--------------------------------

                    //----------------------------start-------------------------------SORT SO Application Workflow
                    #region SORT Application Workflow
                    case SysEventType.show_sort_project_overview_details://show_sort_project_overview_details
                        Sys_Events_Descp = "SORTApplication/SORTProjectOverview with hauliermnemonic: " + ObjMovAct.hauliermnemonic + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",ProjectId: " + ObjMovAct.ProjectId + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.bind_sort_application_revisions://bind_sort_application_revisions
                        Sys_Events_Descp = "SORTApplication/SORTApplRevisions with hauliermnemonic: " + ObjMovAct.hauliermnemonic + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",ProjectId: " + ObjMovAct.ProjectId + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.bind_sort_movment_revisions://bind_sort_movment_revisions
                        Sys_Events_Descp = "SORTApplication/SORTAppMovementVersion with hauliermnemonic: " + ObjMovAct.hauliermnemonic + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",ProjectId: " + ObjMovAct.ProjectId + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.bind_sort_candidate_route_revisions://bind_sort_candidate_route_revisions
                        Sys_Events_Descp = "SORTApplication/SORTAppCandidateRouteVersion with AnalysisId : " + ObjMovAct.AnalysisId + ",hauliermnemonic: " + ObjMovAct.hauliermnemonic + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",ProjectId: " + ObjMovAct.ProjectId + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.bind_sort_special_orders://bind_sort_special_orders
                        Sys_Events_Descp = "SORTApplication/SORTSpecialOrderView with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.allocate_sort_users://allocate_sort_users
                        Sys_Events_Descp = "SORTApplication/AllocateSORTUser with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",Allocate_user_id: " + ObjMovAct.allocateUser + ",ProjectOwner: " + ObjMovAct.UserName + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.save_sort_project_details_from_project_overview_tab://save_sort_project_details_from_project_overview_tab
                        Sys_Events_Descp = "SORTApplication /SaveMovementProjDetails with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.save_sort_new_candidate_route://save_sort_new_candidate_route
                        Sys_Events_Descp = "SORTApplications/SaveCandidateRoute with RouteType: " + ObjMovAct.Routename + ",Routename: " + ObjMovAct.route_id + ",ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",AnalysisId : " + ObjMovAct.AnalysisId + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.create_new_revision_from_last_candidate_evision://create_new_revision_from_last_candidate_evision
                        Sys_Events_Descp = "SORTApplications/CreateCandidateVersion with CandRouteId: " + ObjMovAct.CandRouteId + ",CandVersionId: " + ObjMovAct.CandVersionId + ", esdalref: " + ObjMovAct.esdalRef + ",AnalysisId : " + ObjMovAct.AnalysisId + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_checking_process://sort_checking_process
                        Sys_Events_Descp = "SORTApplication/CheckerUpdation with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",Userid: " + ObjMovAct.User_ID + ",Status: " + ObjMovAct.Status + ",PStatus: " + ObjMovAct.proj_status + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.show_sort_candidate_route://show_sort_candidate_route
                        Sys_Events_Descp = "SORTApplication/ShowCandidateRoutes with RouteRevisionId: " + ObjMovAct.route_id + ",CandVersionId: " + ObjMovAct.CandVersionId + ",ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.show_sort_candidate_route_vehicles://show_sort_candidate_route_vehicles
                        Sys_Events_Descp = "Show Candidate route vehicles with Candidate vehicle: " + ObjMovAct.vehicle_id + ",CandVersionId: " + ObjMovAct.CandVersionId + ",ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.create_new_so_movement_version://create_new_so_movement_version
                        Sys_Events_Descp = "SORTApplication/CreateMovementVersion with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",ApplicationRevisionID : " + ObjMovAct.revision_id + ",AnalysisId : " + ObjMovAct.AnalysisId + ",RouteRevisionId: " + ObjMovAct.route_id + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.show_movement_summary://show_movement_summary
                        Sys_Events_Descp = "SORTApplication/SORTApplicationSummary with ApplicationRevisionID : " + ObjMovAct.revision_id + ",ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OrganisationID: " + ObjMovAct.org_id + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.save_haulier_notes://save_haulier_notes
                        Sys_Events_Descp = "SORTApplication/SaveHaulierNotes with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.get_haulier_notes://get_haulier_notes
                        Sys_Events_Descp = "SORTApplications/HaulierNotes with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_movement_distribution://sort_movement_distribution
                        Sys_Events_Descp = "SORTApplications/SaveDistributionComments with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_movement_agree://sort_movement_agree
                        Sys_Events_Descp = "SORTApplications/MovementAgreeUnagreeWithdraw with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + "Version_Id: " + ObjMovAct.mov_ver + ",flag: " + "Agree" + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_movement_unagree://sort_movement_agree
                        Sys_Events_Descp = "SORTApplications/MovementAgreeUnagreeWithdraw with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + "Version_Id: " + ObjMovAct.mov_ver + ",flag: " + "Unagree" + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_movement_withdraw://sort_movement_agree
                        Sys_Events_Descp = "SORTApplications/MovementAgreeUnagreeWithdraw with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + "Version_Id: " + ObjMovAct.mov_ver + ",flag: " + "Withdraw" + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_created_special_order://sort_movement_agree
                        Sys_Events_Descp = "SORTApplication/CreateSpecialOrder with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",SpecialOrderNo: " + ObjMovAct.Spec_Order_No + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_saved_special_order://sort_movement_agree
                        Sys_Events_Descp = "SORTApplication/SavedSpecialOrder with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",SpecialOrderNo: " + ObjMovAct.Spec_Order_No + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_show_special_order://sort_movement_agree
                        Sys_Events_Descp = "SORTApplication/ShowSpecialOrder with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",SpecialOrderNo: " + ObjMovAct.Spec_Order_No + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_deleted_special_order://sort_movement_agree
                        Sys_Events_Descp = "SORTApplication/DeletedSpecialOrder with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",SpecialOrderNo: " + ObjMovAct.Spec_Order_No + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_generated_special_order_documents://sort_movement_agree
                        Sys_Events_Descp = "SORTApplication/GeneratedSpecialOrder with ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",SpecialOrderNo: " + ObjMovAct.Spec_Order_No + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    case SysEventType.sort_generated_amendment_order://sort_movement_agree
                        Sys_Events_Descp = "SORTApplication/GeneratedAmendmentOrder with  ProjectId: " + ObjMovAct.ProjectId + ", esdalref: " + ObjMovAct.esdalRef + ",OwnerName: " + ObjMovAct.UserName + ",AmendmentOrderNo: " + ObjMovAct.Spec_Order_No + ",VR1App: " + ObjMovAct.VR1App + helpDeskUserstring;
                        break;
                    #endregion
                    //-----------------------------end----------------------------------
                    #region RouteLibraryVehicle Work flow
                    case SysEventType.Haulier_created_library_route://Haulier_created_library_route
                        Sys_Events_Descp = "Haulier created route in route library, Route Id:" + ObjMovAct.lib_route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_edited_library_route://Haulier_edited_library_route
                        Sys_Events_Descp = "Haulier edited route from route library, Previous Route Id: " + ObjMovAct.lib_route_id + ", New Route Id:" + ObjMovAct.new_lib_route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                    case SysEventType.Haulier_deleted_library_route://Haulier_edited_library_route
                        Sys_Events_Descp = "Haulier deleted doute from route library, Route Id: " + ObjMovAct.lib_route_id + ", User Name: " + ObjMovAct.UserName + "" + helpDeskUserstring;
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return Sys_Events_Descp;
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
                        portalName = "police alo portal";
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
    }
}