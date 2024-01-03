using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.DocumentsAndContents;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using System;

namespace STP.DocumentsAndContents.Persistance
{
    public static class OutBoundDocumentDOA
    {
        #region AddManageDocument
        public static long AddManageDocument(OutboundDocuments obdc, string userSchema)
        {
            OutboundDocuments outbounddocs = new OutboundDocuments();
            long documentId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                outbounddocs,
                    userSchema + ".MANAGE_DOCUMENTS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_DOCUMENT_ID", 0, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_DOCUMENT_TYPE", obdc.DocType, OracleDbType.Int32, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_DOCUMENT", obdc.DocumentInBytes, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                        if (obdc.NotificationID == 0)
                        {
                            parameter.AddWithValue("P_NOTIFICATION_ID", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        }
                        else
                        {
                            parameter.AddWithValue("P_NOTIFICATION_ID", obdc.NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        }
                        parameter.AddWithValue("P_ORGANISATION_ID", obdc.OrganisationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ESDAL_REF", obdc.EsdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONTACT_ID", obdc.ContactID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"AddManageDocument started successfully with parameters esdalRef: {obdc.EsdalReference}, Notifid: {1}, DocType: {obdc.NotificationID}, OrgId: {obdc.DocType}, contact: {obdc.ContactID}");
                    },
                        records =>
                        {
                            documentId = records.GetLongOrDefault("MAXID");
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"AddManageDocument completed successfully with parameters DocId : {documentId}");
                        }
                );
            if (obdc.OrganisationID != 0) //manually added parties are not inserted in  Outbound document metadata
            {
                ManageMetaDataDocument(obdc, documentId, userSchema);
            }
            return documentId;
        }
        #endregion

        #region ManageMetaDataDocument
        public static void ManageMetaDataDocument(OutboundDocuments obdc, long documentId, string userSchema)
        {
            OutboundDocuments outbounddocs = new OutboundDocuments();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                outbounddocs,
                    userSchema + ".MANAGE_DOCUMETADATA",
                    parameter =>
                    {
                        parameter.AddWithValue("P_DOCUMENT_ID", documentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORGANISATION_ID", obdc.OrganisationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ESDAL_REF", obdc.EsdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONTACT_ID", obdc.ContactID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                    },
                        records =>
                        {
                        }
                );
        }
        #endregion

        #region SaveInboxItems
        public static long SaveInboxItems(int NotificationID, long documentId, int OrganisationID, string esDAlRefNo, string userSchema = UserSchema.Portal, int icaStatus = 277001, bool ImminentMovestatus = false)
        {

            long inboxId = 0;
            esDAlRefNo = esDAlRefNo.Replace("~", "#");
            esDAlRefNo = esDAlRefNo.Replace("-", "/");
            esDAlRefNo = esDAlRefNo.Replace("#", "/");
            string[] esdalRefPro = esDAlRefNo.Split('/');
            string haulierMnemonic = string.Empty;
            string esdalrefnum = string.Empty;
            int versionNo = 0;
            //----for NEN project---
            int immStatus = 0;
            if (ImminentMovestatus)
            {
                immStatus = 1;
            }
            //----------------------
            if (esdalRefPro.Length > 0)
            {
                haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               inboxId,
               userSchema + ".STP_ROUTE_ASSESSMENT.SP_INSERT_INBOX_ITEMS",
               parameter =>
               {
                   parameter.AddWithValue("P_NOTIF_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ESDAL_REF", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_HAUL_NEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ESDAL_REF_NO", esdalrefnum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_VERSION_NO", versionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_TRANS_ID", documentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ORG_ID", OrganisationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ICA_STATUS", icaStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   if (userSchema != UserSchema.Sort && userSchema!= UserSchema.Sort)      //RM#10827
                       {
                       parameter.AddWithValue("P_IMMINENT_STATUS", immStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//---Added for NEN project
                       }
                   parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SaveInboxItems started successfully with parameters esdalRef : {0}, Notifid : {1}, DocId : {2}, OrgId : {3}, ICAStat : {4}, VersionNo : {5}", esDAlRefNo, NotificationID, documentId, OrganisationID, icaStatus, versionNo));
               },
               records =>
               {
                   inboxId = records.GetLongOrDefault("INBOX_ITEM_ID");
                   Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("AddManageDocument completed successfully with parameters InboxId : {0}", inboxId));
               });
            return inboxId;
        }
        #endregion

        #region Commented Code By mahzeer on 12/07/2023
        /*
        public static long SaveMovementActionForDistTrans(MovementActionIdentifiers movactiontype, string MovDescrp, long? projectId, int? revisionNo, int? versionNo, string userSchema)
        {
            if (projectId == 0)
            {
                projectId = null;
            }
            if (revisionNo == 0)
            {
                revisionNo = null;
            }
            if (versionNo == 0)
            {
                versionNo = null;
            }
            long result = 0;
            int ChkDB = 0;
            if (userSchema == UserSchema.Sort)
            {
                ChkDB = 1;
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                      result,
                      userSchema + ".SP_INSERT_MOVEMENT_ACTION",
                      parameter =>
                      {

                         // parameter.AddWithValue("P_TYPE", ChkDB, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_ESDAL_REF", movactiontype.ESDALRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_ACTION_TYPE", movactiontype.MovementActionType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_OCCURRED", null, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_DESCRIPTION", MovDescrp, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_METADATA", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_PROJECT_ID", projectId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_VERSION_NO", versionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_REVISION_NO", revisionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                      },
                      record =>
                      {
                          result = record.GetLongOrDefault("ACTION_ID");
                      }
                 );
            return result;
        }
        public static bool GenerateMovementAction(UserInfo UserSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem,long ProjectId,int revisionNo, int VersionNo, int movFlagVar = 0, NotificationContacts objContact = null)
        {
            bool status = false;
            MovementActionIdentifiers movactiontype = null;
            string ErrMsg = string.Empty;

            //Movement action of allocation
            movactiontype = new MovementActionIdentifiers();
            if (movActionItem != null)
            {
                movactiontype = movActionItem;
            }

            switch (movFlagVar)
            {
                case 0:
                    break;
                case 1: //499018	499	inbox item delivered
                    movactiontype.MovementActionType = MovementnActionType.inbox_item_delivered;
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.ReciverContactName = "";
                    break;
                case 2: //499019	499	outbound document ready for delivery
                    movactiontype.MovementActionType = MovementnActionType.outbound_doc_ready_for_delivery;
                    movactiontype.ESDALRef = "";
                    movactiontype.ReciverContactName = "";
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.ContactPreference = ContactPreference.emailHtml;
                    break;
                case 3: //499020	499	outbound document delivery failure
                    movactiontype.MovementActionType = MovementnActionType.outbound_doc_delivery_failure;
                    movactiontype.ESDALRef = "";
                    movactiontype.ReciverContactName = "";
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.TransmissionErrorMsg = "";
                    break;
                case 4: //499027	499	outbound document delivered
                    movactiontype.MovementActionType = MovementnActionType.outbound_doc_delivered;
                    movactiontype.ReciverContactName = "";
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.ContactPreference = ContactPreference.emailHtml;
                    break;
                case 5: //499051	499	transmission delivered
                    movactiontype.MovementActionType = MovementnActionType.transmission_delivered;
                    movactiontype.TransmissionId = 0;
                    movactiontype.TransmissionDocType = "";
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.DateTime = DateTime.Today;
                    break;
                case 6: //499052	499	transmission delivery failure
                    movactiontype.MovementActionType = MovementnActionType.transmission_delivery_failure;
                    movactiontype.TransmissionId = 0;
                    movactiontype.TransmissionDocType = "";
                    movactiontype.TransmissionErrorMsg = "";
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.DateTime = DateTime.Today;
                    break;
                case 7: //499053	499	transmission ready for delivery
                    movactiontype.MovementActionType = MovementnActionType.transmission_ready_for_delivery;
                    movactiontype.TransmissionId = 0;
                    movactiontype.TransmissionDocType = "";
                    movactiontype.OrganisationNameReceiver = "";
                    break;
                case 8: //499055	499	outbound document delivery retry
                    movactiontype.MovementActionType = MovementnActionType.outbound_document_delivery_retry;
                    movactiontype.ESDALRef = "";
                    movactiontype.ReciverContactName = "";
                    movactiontype.OrganisationNameReceiver = "";
                    break;
                case 9: //499056	499	transmission delivery retry
                    movactiontype.MovementActionType = MovementnActionType.transmission_delivery_retry;
                    movactiontype.TransmissionId = 0;
                    movactiontype.TransmissionDocType = "";
                    movactiontype.ContactPreference = ContactPreference.emailHtml;
                    break;
                case 10: //499058	499	transmission forwarded
                    movactiontype.MovementActionType = MovementnActionType.transmission_forwarded;
                    movactiontype.TransmissionId = 0;
                    movactiontype.TransmissionDocType = "";
                    movactiontype.ContactPreference = ContactPreference.emailHtml;
                    movactiontype.DateTime = DateTime.Today;
                    break;
                case 11: //499063	499	transmission sent
                    movactiontype.MovementActionType = MovementnActionType.transmission_sent;
                    movactiontype.TransmissionId = 0;
                    movactiontype.TransmissionDocType = "";
                    movactiontype.ContactPreference = ContactPreference.emailHtml;
                    break;
                case 12://499064	499	daily digest sent
                    movactiontype.MovementActionType = MovementnActionType.daily_digest_sent;
                    movactiontype.SenderContactName = "";
                    movactiontype.OrganisationNameSender = "";
                    movactiontype.ItemTypeNo = 0;

                    break;
                case 13://499065	499	manual party added
                    movactiontype.MovementActionType = MovementnActionType.manual_party_added;
                    movactiontype.SenderContactName = "";
                    movactiontype.OrganisationNameSender = "";
                    movactiontype.ManuallyAddedOrgName = "";
                    movactiontype.ManuallyAddedContName = "";
                    break;
                case 14://499066	499	transmission status changed
                    movactiontype.MovementActionType = MovementnActionType.transmission_status_changed;
                    movactiontype.ESDALRef = EsdalRef;
                    movactiontype.TransmissionModel = new TransmissionModel();
                    movactiontype.TransmissionModel.InboxOnly = 0;
                    movactiontype.TransmissionModelFilter.Pending = false;
                    movactiontype.TransmissionModelFilter.Delivered = true;
                    movactiontype.TransmissionModelFilter.Pending = false;
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.ReciverContactName = "";
                    break;
                case 15://499050	499	outbound document retransmitted
                    movactiontype.MovementActionType = MovementnActionType.outbound_document_retransmitted;
                    movactiontype.OrganisationNameReceiver = "";
                    movactiontype.OrganisationNameSender = "";
                    break;
            }

            string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);

            SaveMovementActionForDistTrans(movactiontype, MovementDescription, ProjectId, revisionNo, VersionNo, UserSessionValue.UserSchema);

            return status;
        }
        public static bool SaveSysEvents(MovementActionIdentifiers action, string SysDescrp, int userid, string userschema)// override method for saving sys_events in case of retransmit delivered or failed For Release 2
        {
            long result = 0;
            string machinename = Environment.MachineName;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  result,
                  userschema + ".SP_INSERT_SYS_EVENTS ",
                  parameter =>
                  {
                      parameter.AddWithValue("SV_EVENT_TYPE", action.SystemEventType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("SV_USER_ID", userid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("SV_MACHINE_NAME", machinename, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("SV_DESCRIPTION", SysDescrp, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("SV_SOFT_VERSION", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("SV_PRIORITY", null, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                      if (userschema == UserSchema.Sort)
                          parameter.AddWithValue("SV_USER_SCHEMA", 2, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                      else
                          parameter.AddWithValue("SV_USER_SCHEMA", null, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("SV_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                  record =>
                  {
                      result = record.GetLongOrDefault("EVENT_ID");
                  }
             );
            if (result != 0)
                return true;
            else
                return false;
        }
        public static void SaveXMLOutboundNotification(OutboundDocuments obdc)
        {
            OutboundDocuments outbounddocs = new OutboundDocuments();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                outbounddocs,
                    UserSchema.Portal + ".UPDATE_OUTBOUNDNOTIFICATIONXML",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", obdc.NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CORENOTIFICATIONXML", obdc.DocumentInBytes, OracleDbType.Blob, ParameterDirectionWrap.Input, 100000000);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        records =>
                        {
                        }
                );
        }
        */
        #endregion

        #region InsertCollaboration
        public static int InsertCollaboration(OutboundDocuments obdc, long documentId, string userSchema, int status = 0)
        {

            int? contactId;
            int isUpdated = 0;

            if (obdc.ContactID == 0)
            {
                contactId = null;
            }
            else
            {
                contactId = obdc.ContactID;
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               isUpdated,
               userSchema + ".SP_INSERT_COLLABORATION",
               parameter =>
               {
                   parameter.AddWithValue("P_DOC_ID", documentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_CONTACT_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_STATUS", status, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               records =>
               {
                   isUpdated = (int)records.GetDecimalOrDefault("IS_UPDATED");
               });

            return isUpdated;

        }
        #endregion

    }
}