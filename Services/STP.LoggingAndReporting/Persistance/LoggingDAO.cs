using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.DocumentsAndContents;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.LoggingAndReporting.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Persistance
{
    public static class LoggingDAO
    {

        #region InsertTransmissionInfoToAction(NotificationContacts objcontact,long transmissionId, string esdalRef,int actionFlag, string errMessage = "",string docType="")
        public static void InsertTransmissionInfoToAction(NotificationContacts objcontact, UserInfo userInfo, long transmissionId, string esdalRef, int actionFlag, string errMessage = "", string docType = "")
        {

            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                switch (actionFlag)
                {
                    case 1: //499053	499	transmission ready for delivery
                        movactiontype.MovementActionType = MovementnActionType.transmission_ready_for_delivery;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.OrganisationNameReceiver = objcontact.OrganistationName;
                        break;

                    case 2: //499051	499	transmission delivered
                        movactiontype.MovementActionType = MovementnActionType.transmission_delivered;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.OrganisationNameReceiver = objcontact.OrganistationName;
                        movactiontype.DateTime = DateTime.Today;
                        break;

                    case 3: //499052	499	transmission delivery failure
                        movactiontype.MovementActionType = MovementnActionType.transmission_delivery_failure;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.TransmissionErrorMsg = errMessage;
                        movactiontype.OrganisationNameReceiver = objcontact.OrganistationName;
                        movactiontype.DateTime = DateTime.Today;
                        break;

                    case 4: //499058	499	transmission forwarded
                        movactiontype.MovementActionType = MovementnActionType.transmission_forwarded;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.ContactPreference = objcontact.ContactPreference;
                        movactiontype.DateTime = DateTime.Today;
                        break;

                    case 5: //499063	499	transmission sent
                        movactiontype.MovementActionType = MovementnActionType.transmission_sent;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.ContactPreference = objcontact.ContactPreference;
                        break;


                }
               
                GenerateMovementAction(userInfo, esdalRef, movactiontype);
                #region-------------- Release 2 added sys_events loggs for Retransmit
                if (movactiontype.MovementActionType == MovementnActionType.transmission_delivered)
                {
                    movactiontype.SystemEventType = SysEventType.Retransmitted_document;
                    movactiontype.UserId = Convert.ToInt32(userInfo.UserId);
                    movactiontype.UserName = userInfo.UserName;
                    movactiontype.ESDALRef = esdalRef;
                   
                    string sysEventDescp = System_Events.GetSysEventString(userInfo, movactiontype, out errMessage);
                    int user_ID = Convert.ToInt32(userInfo.UserId);
               
                    SaveSysEvents((int)movactiontype.SystemEventType, sysEventDescp, user_ID, userInfo.UserSchema);
                }
                else if (movactiontype.MovementActionType == MovementnActionType.transmission_delivery_failure)
                {
                    movactiontype.SystemEventType = SysEventType.Retransmit_failed_document;
                    movactiontype.UserId = Convert.ToInt32(userInfo.UserId);
                    movactiontype.UserName = userInfo.UserName;
                    movactiontype.ESDALRef = esdalRef;
                 
                    string sysEventDescp = System_Events.GetSysEventString(userInfo, movactiontype, out errMessage);
                    int user_ID = Convert.ToInt32(userInfo.UserId);
                   
                     SaveSysEvents((int)movactiontype.SystemEventType, sysEventDescp, user_ID, userInfo.UserSchema);
                }
                #endregion
           
        }
        #endregion

        public static bool GenerateMovementAction(UserInfo userSessionValue, string esdalRef, MovementActionIdentifiers movActionItem, int movFlagVar = 0, NotificationContacts objContact = null)
        {
            bool status = false;

            MovementActionIdentifiers movactiontype = null;
            string ErrorMsg = string.Empty;

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
                    movactiontype.ESDALRef = esdalRef;
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

            string MovementDescription = MovementActions.GetMovementActionString(userSessionValue, movactiontype, out ErrorMsg);

             SaveMovementAction(movactiontype.ESDALRef, (int) movactiontype.MovementActionType, MovementDescription,0,0,0, userSessionValue.UserSchema);

            return status;
        }

        #region SaveMovementAction
        public static long SaveMovementAction(string esdalRef, int movementActionType, string movementDescription,long? projectId,int? revisionNo,int? versionNo, string userSchema)
        {
            if(projectId==0)
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
                          //parameter.AddWithValue("P_TYPE", ChkDB, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_ESDAL_REF", esdalRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_ACTION_TYPE", movementActionType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_OCCURRED", null, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("P_DESCRIPTION", movementDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
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
        #endregion
        #region SaveSysEvents
        public static bool SaveSysEvents(int systemEventType, string systemDescrp, int userId, string userSchema)
        {
            long result = 0;
            string machinename = Environment.MachineName;

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                      result,
                      userSchema + ".SP_INSERT_SYS_EVENTS ",
                      parameter =>
                      {
                          parameter.AddWithValue("SV_EVENT_TYPE", systemEventType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("SV_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("SV_MACHINE_NAME", machinename, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("SV_DESCRIPTION", systemDescrp, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("SV_SOFT_VERSION", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                          parameter.AddWithValue("SV_PRIORITY", null, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                          if (userSchema == UserSchema.Sort)
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
        #endregion
        #region AuditLog

        public static long SaveNotifAuditLog(AuditLogIdentifiers auditLogType, string logMsg, int userId, long organisationId = 0)
        {
            long result = 0;
            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        result,
                        UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_NEN_INSERT_ESDAL_NOTIF_LOG",
                        parameter =>
                        {
                            parameter.AddWithValue("P_ESDAL_REF_NO", auditLogType.ESDALNotificationNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_INBOX_ITEM_ID", auditLogType.InboxItemId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_LOG_MSG", logMsg, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                        },
                         (records, instance) =>
                         {
                             result = Convert.ToInt64(records["AUDIT_ID"]);
                         }
                );

            return result;
        }
        #endregion

        #region Get AuditLog Search
        public static List<NENAuditLogList> GetAuditListSearch(string searchString, int pageNo, int pageSize, int sortFlag, long organisationId, string searchType, int searchNotificationSource, int presetFilter = 1, int? sortOrder = null)
        {
            List<NENAuditLogList> AuditList = new List<NENAuditLogList>();
            if (!string.IsNullOrEmpty(searchString) && searchType == "3")
            {
                DateTime dt = DateTime.ParseExact(searchString, "d/M/yyyy", CultureInfo.InvariantCulture);
                searchString = dt.ToString("dd-MMM-yyyy");
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                AuditList,
                  UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_NEN_AUDIT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_searchString", searchString, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("sortFlag", sortFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEARCHITEM", searchType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDAL", searchNotificationSource, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {

                        instance.History = records.GetStringOrDefault("LOG").ToString();
                        instance.RecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                        instance.ESDALReferenceNumber = records.GetStringOrDefault("ESDAL_REFERENCE").ToString();
                        instance.HistoryDate = records.GetDateTimeOrDefault("LOG_INSERT_TIME").ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        instance.InboxItemId = records.GetLongOrDefault("INBOX_ITEM_ID");
                        instance.UserName = records.GetStringOrDefault("USER_NAM");
                        instance.NotificationSource = records.GetStringOrDefault("NotificationSource");

                    }
            );
            return AuditList;
        }
        #endregion

        #region Get NEN Auditlog
        public static List<NENAuditGridList> ListNENAuditPerNotification(int? page, int? pageSize, string NENnotificationNo, long organisationId,int? sortOrder,int? sortType)
        {
            List<NENAuditGridList> AuditlogperNEN = new List<NENAuditGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    AuditlogperNEN,
                    UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_AUDIT_PER_NEN",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ESDAL_NOTIF", NENnotificationNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_PAGENUM", page, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("PRESET_FILTER",sortType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                    },
                    (records, instance) =>
                    {
                        DateTime dt;
                        dt = records.GetDateTimeOrDefault("LOG_INSERT_TIME");
                        string insertTime = dt.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture);
                        instance.DateTime = insertTime.Replace('-', '/');
                        instance.User = Convert.ToString(records["USER_NAME"]);
                        instance.Log = Convert.ToString(records["LOG"]);
                        instance.RecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        instance.NotificationSource= Convert.ToString(records["NOTIFICATIONSOURCE"]);
                    }
            );
            return AuditlogperNEN;
        }
        #endregion

        public static List<NotificationHistory> GetNotificationHistory(int pageNumber, int pageSize, long notificationNo, int sortOrder, int sortType, int historic, int userType = 0, long projectId = 0)
        {
            List<NotificationHistory> notificationHistoryList = new List<NotificationHistory>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                notificationHistoryList,
                UserSchema.Portal + ".SP_GET_NOTIFICATION_HISTORY",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_NO", notificationNo, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PROJECT_ID", projectId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", sortType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USERTYPE", userType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {

                        //instance.NotificationVersionNumber = Convert.ToInt32(records.GetShortOrDefault("VERSION_NO"));
                        instance.NotifDate = records.GetDateTimeOrEmpty("OCCURRED");
                        instance.Description = records.GetStringOrDefault("DESCRIPTION");
                        instance.ActionType = records.GetStringOrDefault("ACTION_TYPE");
                        //instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                        instance.TotalCount = Convert.ToInt32(records.GetDecimalOrDefault("TOTAL_ROWS"));
                    }
            );
            return notificationHistoryList;
        }


    }
}