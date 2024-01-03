using STP.Domain;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Document
{
    public class MovementActions
    {
        public MovementActions()
        {
            //default constructor
        }
        /// <summary>
        /// GetMovementActionString
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ObjMovAct"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="ESDALRef"></param>
        /// <returns></returns>
        public static string GetMovementActionString(UserInfo userInfo, MovementActionIdentifiers ObjMovAct, out string ErrMsg, string ESDALRef = "")
        {
            string movement_description = string.Empty;
            string portalName = string.Empty;
            ErrMsg = "No Data Found";
            try
            {
                portalName = GetPortalName(userInfo.UserTypeId);
                movement_description = GetMovementDescription(portalName, userInfo, ObjMovAct);
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                ErrMsg = ex.Message;
            }
            return movement_description;
        }

        private static string GetMovementDescription(string portalName, UserInfo userInfo, MovementActionIdentifiers ObjMovAct)
        {
            string err = string.Empty;
            string CollabStatus = string.Empty;
            string MovementDescription = string.Empty;
            string helpDeskUserstring = "";
            if (userInfo.HelpdeskLoginAsAnotherUser == true)
            {
                helpDeskUserstring = "";//"(Logged in by helpdesk User " + userInfo.HelpdeskUserName + "(" + userInfo.HelpdeskUserId + "))";
            }
            try
            {
                switch (ObjMovAct.MovementActionType)
                {
                    case MovementnActionType.haulier_submit_appl: //haulier submits application
                        MovementDescription = "The application '" + ObjMovAct.ESDALRef + "' was submitted by haulier '" + userInfo.UserName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.haulier_withdraw_appl: //haulier withdraws application
                        MovementDescription = "The application was withdrawn by haulier '" + userInfo.UserName + "'. It is a " + ObjMovAct.DocType + " type project." + helpDeskUserstring;
                        break;
                    //----SORT side
                    case MovementnActionType.sort_desktop_submits_appl: //sort desktop submits application
                        MovementDescription = "The application '" + ObjMovAct.ESDALRef + "' was submitted by SORT user '" + ObjMovAct.ContactName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desktop_declines_appl: //sort desktop declines application
                        MovementDescription = "The application was declined by SORT user '" + ObjMovAct.ContactName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desktop_withdraws_appl: //sort desktop withdraws application
                        MovementDescription = "The application was withdrawn by SORT user '" + ObjMovAct.ContactName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desktop_allocates_proj: //sort desktop allocates project
                        MovementDescription = "SORT user '" + ObjMovAct.ContactName + "' allocated ownership of the project to '" + ObjMovAct.AllocateUser + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desktop_reallocates_proj: //sort desktop reallocates project
                        MovementDescription = "SORT user '" + ObjMovAct.ContactName + "' changed the ownership of the project, from '" + ObjMovAct.AllocateUser + "' to '" + ObjMovAct.ReallocateUser + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_receives_appl_from_haul: //sort ss receives application from haulier
                        MovementDescription = "The SORT team received the submitted application from the owning haulier." + helpDeskUserstring;
                        break;
                    //---------------------------
                    case MovementnActionType.sort_acknowledges_appl_tohaul: //sort acknowledges application to haulier
                        MovementDescription = "The owning haulier has received confirmation that the submitted application has been received by the SORT team." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_logs_in: //user logs in
                        MovementDescription = "User " + userInfo.UserName + " has logged in to " + portalName + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_logs_out: //user logs out
                        MovementDescription = "User " + ObjMovAct.ContactName + " has logged out from " + portalName + helpDeskUserstring;
                        break;
                    //SORT side--------------------------
                    case MovementnActionType.hauliers_withdrawal_req_receipt: //hauliers withdrawal request receipt
                        MovementDescription = "A request to withdraw the movement project was received from the owning haulier." + helpDeskUserstring;
                        break;
                    //-----------------------------------
                    case MovementnActionType.sort_withdrawal_request_receipt: //sort withdrawal request receipt
                        MovementDescription = "A request to withdraw the movement project was received from the SORT team." + helpDeskUserstring;
                        break;
                    //SORT side---------------------------
                    case MovementnActionType.sort_desk_sends_candroutever_for_check: //sort desktop sends movement for checking
                        MovementDescription = "SORT user '" + ObjMovAct.ContactName + "' sent candidate route version '" + ObjMovAct.CandVerNo + "' for checking to '" + ObjMovAct.CheckerName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desk_returns_candver_from_check: // sort desktop returns movement from checking
                        MovementDescription = "SORT user '" + ObjMovAct.CheckerName + "' has returned candidate route version '" + ObjMovAct.CandVerNo + "' from checking to '" + ObjMovAct.AllocateUser + "'." + helpDeskUserstring;//not implemented
                        break;
                    case MovementnActionType.sort_desk_distributes_movement_version: // sort desktop distributes movement version
                        MovementDescription = "The movement version has been distributed by SORT user '" + ObjMovAct.ContactName + "' to a state of '" + ObjMovAct.ProjectStatus + "'." + helpDeskUserstring;
                        break;
                    //------------------------------------
                    case MovementnActionType.user_opens_inbox_item: //user opens inbox item
                        MovementDescription = "The inbox item '" + ObjMovAct.ESDALRef + "' has been opened by user '" + userInfo.UserName + "'. It was destined for organisation '" + ObjMovAct.OrganisationNameReceiver + "', recipient '" + ObjMovAct.ReciverContactName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.inbox_item_delivered: //inbox item delivered
                        MovementDescription = "An outbound document has been placed in organisation's '" + ObjMovAct.OrganisationNameReceiver + "' inbox, recipient '" + ObjMovAct.ReciverContactName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.outbound_doc_ready_for_delivery: //outbound document ready for delivery
                        MovementDescription = "The outbound document '" + ObjMovAct.ESDALRef + "' is ready for delivery to '" + ObjMovAct.ReciverContactName + "' at '" + ObjMovAct.OrganisationNameReceiver + "' via '" + ObjMovAct.ContactPreference.ToString() + "'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.outbound_doc_delivery_failure: //outbound document delivery failure
                        MovementDescription = "The outbound document '" + ObjMovAct.ESDALRef + "' has not been delivered to '" + ObjMovAct.ReciverContactName + "' at '" + ObjMovAct.OrganisationNameReceiver + "' because: " + ObjMovAct.TransmissionErrorMsg + "" + helpDeskUserstring;
                        break;
                    //SORT side---------------------------------------
                    case MovementnActionType.user_amends_special_order: //user amends special order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has updated Special Order '" + ObjMovAct.SpecialOrderNo + "' for movement version '" + ObjMovAct.ESDALRef + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_deletes_special_order: //user deletes special order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has deleted Special Order '" + ObjMovAct.SpecialOrderNo + "' for movement version '" + ObjMovAct.ESDALRef + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_revokes_special_order: //user revokes special order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has revoked Special Order '" + ObjMovAct.SpecialOrderNo + "' for movement version '" + ObjMovAct.ESDALRef + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_creates_special_order: //user creates special order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has created Special Order '" + ObjMovAct.SpecialOrderNo + "' for movement version '" + ObjMovAct.ESDALRef + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_reuse_special_order: //user reuse special order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has reuse Special Order '" + ObjMovAct.SpecialOrderNo + "' for movement version '" + ObjMovAct.ESDALRef + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_agrees_movement_version: //user agrees movement version
                        MovementDescription = "The movement version '" + ObjMovAct.ESDALRef + "' has been agreed by SORT user '" + ObjMovAct.ContactName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_unagrees_movement_version: //user unagrees movement version
                        MovementDescription = "The movement version '" + ObjMovAct.ESDALRef + "' has been unagreed by SORT user '" + ObjMovAct.ContactName + "'." + helpDeskUserstring;
                        break;
                    //--------------------------------------------------
                    case MovementnActionType.outbound_doc_delivered: //outbound document delivered
                        MovementDescription = "The outbound document has been delivered to '" + ObjMovAct.ReciverContactName + "' at '" + ObjMovAct.OrganisationNameReceiver + "' via '" + ObjMovAct.ContactPreference.ToString() + "'" + helpDeskUserstring;
                        break;
                    //SORT side----------------------
                    case MovementnActionType.user_adds_mtp: //user adds mtp
                        MovementDescription = "User '" + userInfo.UserName + "' has created an MTP named 'Great Yarmouth Test MTP' at OS grid location '653123,307898' with reference number 'M-TG531078-1'" + helpDeskUserstring;// Not known fully
                        break;
                    case MovementnActionType.user_deletes_mtp: //user deletes mtp
                        MovementDescription = "User 'sandy mac testing' has deleted MTP with reference 'M-ST513784-1'" + helpDeskUserstring;// Not fully known
                        break;
                    //--------------------------------
                    case MovementnActionType.delegation_alert: //delegation alert
                        MovementDescription = "A delegation failure alert has been sent to '" + ObjMovAct.ReciverContactName + "' at '" + ObjMovAct.OrganisationNameReceiver + "' via online inbox due to failing to send a document to '" + ObjMovAct.FailToSendUser + "' at '" + ObjMovAct.FailToSendOrganisation + "' via " + ObjMovAct.ContactPreference.ToString() + "" + helpDeskUserstring;
                        break;
                    case MovementnActionType.notification_attempt: //notification attempt
                        MovementDescription = "User '" + userInfo.UserName + "' at '" + userInfo.OrganisationName + "' has triggered a notification and confirmed indemnity against movement version '" + ObjMovAct.ESDALRef + "'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.admin_create_new_contact: //new contact user
                        MovementDescription = "Administrator '" + userInfo.UserName + "' has created a new contact named ' " + ObjMovAct.ContactName + "'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.admin_changes_users_detail: //amend user
                        MovementDescription = "Administrator '" + userInfo.UserName + "n' has changed user '" + ObjMovAct.HaulierName + "''s details" + helpDeskUserstring;
                        break;
                    case MovementnActionType.admin_deleted_contact_user: //delete contact users
                        MovementDescription = "Administrator '" + userInfo.UserName + "' has deleted the following contacts '" + ObjMovAct.ContactId + "'" + helpDeskUserstring;
                        break;
                    //SORT side------------------------------------
                    case MovementnActionType.sort_desktop_revises_appl: //sort desktop revises application
                        MovementDescription = "The application '" + ObjMovAct.ESDALRef + "' was revised by SORT user '" + ObjMovAct.ContactName + "'." + helpDeskUserstring;
                        break;
                    //---------------------------------------------
                    case MovementnActionType.haulier_revises_application: //haulier revises application
                        MovementDescription = "The application '" + ObjMovAct.ESDALRef + "' was revised by Haulier '" + userInfo.UserName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.haulier_clones_application: //haulier clones application
                        MovementDescription = "The application '" + ObjMovAct.ESDALRef + "' was cloned by Haulier '" + userInfo.UserName + "'." + helpDeskUserstring;
                        break;
                    //SORT side------------------------------------
                    case MovementnActionType.sort_desk_creates_movement_revisions: //user revises movement version
                        //MovementDescription = "The movement version '" + ObjMovAct.ESDALRef + "' has been revised by SORT user '" + ObjMovAct.ContactName + "'.";
                        MovementDescription = "The movement version '" + ObjMovAct.ESDALRef + "' has been created by SORT user '" + ObjMovAct.ContactName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desk_acknowl_proj_revisions: //sort desktop acknowledges projects revisions
                        MovementDescription = "SORT user '" + ObjMovAct.ContactName + "' has acknowledged project '" + ObjMovAct.ESDALRef + "'s application revisions - latest movement version number '" + ObjMovAct.MovementVer + "', acknowledged application revision numbers '[applicationRevisionNumber '" + ObjMovAct.RevisionNo + "', createdBySORT '" + ObjMovAct.ReviseBySort + "']'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_amends_amendment_order: //user amends amendment order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has superseded Special Order '" + ObjMovAct.SpecialOrderNo + "' for movement version '" + ObjMovAct.ESDALRef + "'. Superseding order number is '" + ObjMovAct.SpecialOrderNo + "'" + helpDeskUserstring;
                        //or User 'sarah hollender' has updated Amendment Order 'P195/2008' for movement version 'SORT/S128/4'.
                        break;
                    case MovementnActionType.user_creates_amendment_order: //user creates amendment order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has created Amendment Order '" + ObjMovAct.SpecialOrderNo + "' for movement version '" + ObjMovAct.ESDALRef + "'. It amends Special Order '" + ObjMovAct.SpecialOrderNo + "'" + helpDeskUserstring;
                        break;
                    //---------------------------------------------
                    case MovementnActionType.renotification_attempt: //renotification attempt
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' at '" + userInfo.OrganisationName + "' has triggered a re-notification and confirmed indemnity against movement version '" + ObjMovAct.ESDALRef + "'" + helpDeskUserstring;
                        break;
                    //SORT Side-------------------------------------
                    case MovementnActionType.pre_esdal_move_addition_attempt: //pre esdal movement addition attempt
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has created a Pre-ESDAL Movement, Job File Reference '" + ObjMovAct.JobFileRef + "'." + helpDeskUserstring;
                        break;
                    //-----------------------------------------------
                    case MovementnActionType.collaboration_status_check: //collaboration status amended
                        switch (ObjMovAct.CollaborationStatus)
                        {
                            case 327001:
                                CollabStatus = "ACCEPTED";
                                break;
                            case 327002:
                                CollabStatus = "REJECTED";
                                break;
                            case 327003:
                                CollabStatus = "UNDER ASSESSMENT";
                                break;
                            case 327006:
                                CollabStatus = "NOT APPLICABLE";
                                break;
                        }
                        MovementDescription = "The collaboration status of inbox item '" + ObjMovAct.ESDALRef + "' has been amended by user '" + userInfo.FirstName + "' to '" + ObjMovAct.CollaborationNotes + "'" + helpDeskUserstring;
                        break;
                    //SORT side-------------------------------------------
                    case MovementnActionType.int_collab_details_amended: //internal collaboration details amended
                        switch (ObjMovAct.CollaborationStatus)
                        {
                            case 327001:
                                CollabStatus = "ACCEPTED";
                                break;
                            case 327002:
                                CollabStatus = "REJECTED";
                                break;
                            case 327003:
                                CollabStatus = "UNDER ASSESSMENT";
                                break;
                            case 327006:
                                CollabStatus = "NOT APPLICABLE";
                                break;
                        }
                        MovementDescription = "The internal collaboration details relating to '" + ObjMovAct.ESDALRef + "' has been amended by user '" + ObjMovAct.ContactName + "' to '" + ObjMovAct.CollaborationNotes + "'" + helpDeskUserstring;
                        //or The internal collaboration details relating to 'GCS1/2/1' has been set to 'UNSPECIFIED'
                        break;
                    case MovementnActionType.outbound_document_retransmitted: //outbound document retransmitted
                        MovementDescription = "The outbound document is ready for redelivery to '" + ObjMovAct.OrganisationNameReceiver + "' at '" + ObjMovAct.OrganisationNameSender + "'" + helpDeskUserstring;
                        break;
                    //----------------------------------------------------
                    case MovementnActionType.transmission_delivered: //transmission delivered
                        MovementDescription = "The transmission " + ObjMovAct.TransmissionId + " of type " + ObjMovAct.TransmissionDocType + " has been delivered via " + ObjMovAct.OrganisationNameReceiver + " (started sending " + ObjMovAct.DateTime + ")" + helpDeskUserstring;
                        break;
                    case MovementnActionType.transmission_delivery_failure: //transmission delivery failure
                        MovementDescription = "The transmission " + ObjMovAct.TransmissionId + " of type " + ObjMovAct.TransmissionDocType + " has failed to be delivered via " + ObjMovAct.OrganisationNameReceiver + " due to '" + ObjMovAct.TransmissionErrorMsg + "' (started sending " + ObjMovAct.DateTime + ")" + helpDeskUserstring;
                        break;
                    case MovementnActionType.transmission_ready_for_delivery: //transmission ready for delivery
                        MovementDescription = "The transmission " + ObjMovAct.TransmissionId + " of type " + ObjMovAct.TransmissionDocType + " is ready for delivery via " + ObjMovAct.OrganisationNameReceiver + "" + helpDeskUserstring;
                        break;
                    case MovementnActionType.movement_notified: //movement notified
                        MovementDescription = "Movement '" + ObjMovAct.ESDALRef + "' of type '" + ObjMovAct.DocType + "' has been notified producing notification '" + ObjMovAct.NotificationCode + "'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.outbound_document_delivery_retry: //outbound document delivery retry
                        MovementDescription = "The delivery of the outbound document '" + ObjMovAct.ESDALRef + "' to '" + ObjMovAct.ReciverContactName + "' at '" + ObjMovAct.OrganisationNameReceiver + "' is being retried" + helpDeskUserstring;
                        break;
                    case MovementnActionType.transmission_delivery_retry: //transmission delivery retry
                        MovementDescription = "Retrying delivery of transmission " + ObjMovAct.TransmissionId + " of type " + ObjMovAct.TransmissionDocType + " via " + ObjMovAct.ContactPreference.ToString() + "" + helpDeskUserstring;
                        break;
                    //SORT Side-------------------------------------------
                    case MovementnActionType.outbound_document_suppressed: //outbound document suppressed
                        MovementDescription = "The outbound document delivery to '" + ObjMovAct.ContactName + "' at '" + ObjMovAct.OrganisationNameReceiver + "' for movement version '" + ObjMovAct.ESDALRef + "' has been suppressed" + helpDeskUserstring;
                        break;
                    //-----------------------------------------------------
                    case MovementnActionType.transmission_forwarded: //transmission forwarded
                        MovementDescription = "The transmission " + ObjMovAct.TransmissionId + " of type " + ObjMovAct.TransmissionDocType + " has been forwarded via " + ObjMovAct.ContactPreference.ToString() + " (started sending " + ObjMovAct.DateTime + ")" + helpDeskUserstring;
                        break;
                    //SORT side--------------------------------------------
                    case MovementnActionType.vr1_application_approved: //vr1 application approved
                        MovementDescription = "The application '" + ObjMovAct.ESDALRef + "' was approved by '" + userInfo.UserName + "' and the VR1 number generated is '" + ObjMovAct.VR1GenNum + "'." + helpDeskUserstring;
                        break;
                    //-----------------------------------------------------
                    case MovementnActionType.transmission_sent: //transmission sent
                        MovementDescription = "The transmission " + ObjMovAct.TransmissionId + " of type " + ObjMovAct.TransmissionDocType + " has been sent via " + ObjMovAct.ContactPreference.ToString() + "" + helpDeskUserstring;
                        break;
                    case MovementnActionType.daily_digest_sent: //daily digest sent
                        MovementDescription = "" + ObjMovAct.SenderContactName + " " + ObjMovAct.OrganisationNameSender + " has been sent a Daily Digest containing " + ObjMovAct.ItemTypeNo + " items" + helpDeskUserstring;
                        break;
                    case MovementnActionType.manual_party_added: //manual party added
                        //if delegated user then handle condition using below result
                        //result = "Notifier '" + ObjMovAct.applicant_name + "' ('" + ObjMovAct.OrganisationNameSender + "' notifying on behalf of '" + ObjMovAct.manually_added_org_name + "') has manually added '" + ObjMovAct.manually_added_cont_name + "' ('" + ObjMovAct.OrganisationNameSender + "')";
                        MovementDescription = "Notifier '" + ObjMovAct.SenderContactName + "' (" + ObjMovAct.OrganisationNameSender + ") has manually added '" + ObjMovAct.ManuallyAddedOrgName + "' ('" + ObjMovAct.ManuallyAddedContName + "')" + helpDeskUserstring;
                        break;
                    case MovementnActionType.transmission_status_changed: //transmission status changed
                        MovementDescription = "Movement transmission '" + ObjMovAct.ESDALRef + "', mechanism '" + ObjMovAct.TransmissionModel.InboxOnly + "', status was '" + ObjMovAct.TransmissionModelFilter.Pending + "', is now '" + ObjMovAct.TransmissionModelFilter.Delivered + "', overall '" + ObjMovAct.TransmissionModelFilter.Pending + "'. It was destined for organisation '" + ObjMovAct.OrganisationNameReceiver + "', recipient '" + ObjMovAct.ReciverContactName + "'. " + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_accepted_terms_and_cond: //user accepted terms and conditions
                        MovementDescription = "User '" + userInfo.UserName + "' of organisation '" + userInfo.OrganisationName + "' has accepted portal terms and conditions" + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_accepted_notification_terms_and_cond: //user accepted notification terms and conditions
                        MovementDescription = "'" + userInfo.UserName + "' of organisation '" + userInfo.OrganisationName + "' has accepted the notification terms and conditions when notifying '" + ObjMovAct.NotificationCode + "'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.notification_collaboration_made: //notification collaboration made
                        MovementDescription = "Collaborator '" + userInfo.UserName + "' from '" + userInfo.OrganisationName + "' has added a collaboration note against notification '" + ObjMovAct.NotificationCode + "'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.notification_collaboration_viewed: //notification collaboration viewed
                        MovementDescription = "Haulier '" + userInfo.UserName + "' from '" + userInfo.OrganisationName + "' has viewed a collaboration note from '" + ObjMovAct.OrganisationNameSender + "' against notification '" + ObjMovAct.NotificationCode + "'. Note was added against notification '" + ObjMovAct.NotificationCode + "' at '" + ObjMovAct.DateTime + "'" + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desk_sends_mov_ver_for_QA_check: //sort desktop sends movement for checking
                        MovementDescription = "SORT user '" + ObjMovAct.ContactName + "' sent movement version '" + ObjMovAct.MovementVer + "' for QA checking to '" + ObjMovAct.CheckerName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desk_returns_mov_ver_from_QA_check: // sort desktop returns movement from checking
                        MovementDescription = "SORT user '" + ObjMovAct.CheckerName + "' has returned movement version '" + ObjMovAct.MovementVer + "' from QA checking to '" + ObjMovAct.AllocateUser + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desk_sends_mov_ver_for_final_check: //sort desktop sends movement for checking
                        MovementDescription = "SORT user '" + ObjMovAct.ContactName + "' sent movement version '" + ObjMovAct.MovementVer + "' for final checking to '" + ObjMovAct.CheckerName + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.sort_desk_returns_mov_ver_from_final_check: // sort desktop returns movement from checking
                        MovementDescription = "SORT user '" + ObjMovAct.CheckerName + "' has returned movement version '" + ObjMovAct.MovementVer + "' from final checking to '" + ObjMovAct.AllocateUser + "'." + helpDeskUserstring;
                        break;
                    case MovementnActionType.user_creates_candidate_ver_from_revised_app_ver: //user creates special order
                        MovementDescription = "User '" + ObjMovAct.ContactName + "' has created candidate route version '" + ObjMovAct.CandVerNo + "' from revised application version '" + ObjMovAct.ESDALRef + "'." + helpDeskUserstring;
                        break;
                        /*case MovementnActionType.sort_desk_sends_vr1_for_check: //sort desktop sends vr1 for checking
                            MovementDescription = "SORT user '" + ObjMovAct.ContactName + "' sent movement version '" + ObjMovAct.MovementVer + "' for checking to '" + ObjMovAct.CheckerName + "'.";
                            break;
                        case MovementnActionType.sort_desk_returns_vr1_from_check: // sort desktop returns vr1 from checking
                            MovementDescription = "SORT user '" + ObjMovAct.CheckerName + "' has returned movement version '" + ObjMovAct.MovementVer + "' from checking to '" + ObjMovAct.AllocateUser + "'.";
                            break;*/
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return MovementDescription;
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