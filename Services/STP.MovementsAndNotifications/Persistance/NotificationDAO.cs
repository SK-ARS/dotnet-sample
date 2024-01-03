using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.Configuration;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Routes;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class NotificationDAO
    {
        #region Get IsAcknowledge
        internal static bool GetIsAcknowledge(string esdalReference, int historic)
        {
            bool result = true;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                UserSchema.Portal + ".SP_GET_ACKNOWLEDGE",
                parameter =>
                {
                    parameter.AddWithValue("V_ESDAL_REF", esdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    count = Convert.ToInt32(records.GetDecimalOrDefault("ACKNOWLEDGE_COUNT"));
                }
            );
            if (count >= 1)
                result = false;

            return result;
        }
        #endregion

        #region Get Unacknowledged Collaboration
        public static CollaborationModel GetUnacknowledgedCollaboration(string notificationCode, int historic)
        {
            CollaborationModel CollaborationList = new CollaborationModel();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    CollaborationList,
                      UserSchema.Portal + ".GET_UNACK_COLLABORATION",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_CODE", notificationCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 50);
                        parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.Title = records.GetStringOrDefault("TITLE");
                            instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
                            instance.SurName = records.GetStringOrDefault("SUR_NAME");
                            instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                            instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                            // instance.DateAndTime = records.GetDateTimeOrDefault("WHEN");
                            DateTime dt = records.GetDateTimeOrDefault("WHEN");
                            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                            instance.DateAndTime = TimeZoneInfo.ConvertTimeFromUtc(dt, ukTimeZone);
                            instance.PhoneNumber = records.GetStringOrDefault("phonenumber");
                            instance.Notes = records.GetStringOrDefault("notes");
                            instance.DocumentId = records.GetLongOrDefault("document_id");
                            instance.CollaborationNo = records.GetInt16OrDefault("collaboration_no");
                        }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{ConfigurationManager.AppSettings["Instance"] } - NotificationDAO/GetUnacknowledgedCollaboration, Exception: {ex}");
                throw;
            }
            return CollaborationList;
        }
        #endregion

        #region Check Notification Submitted
        public static string CheckIfNotificationSubmitted(int notificationId)
        {
            string notificationcode = "";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                notificationcode,
                UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_SUBMIT_NOTIFICATION",
                parameter =>
                {
                    parameter.AddWithValue("SB_NOTIFICATIONID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("SB_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    notificationcode = records.GetStringOrDefault("NOTIFICATION_CODE");
                }
            );
            return notificationcode;
        }
        #endregion

        #region UpdateCollborationAck
        public static bool UpdateCollborationAck(long DocID, int ColNo, int UserID, string AcknowledgeAgainst, int historic)
        {
            bool result = false;
            string Res = "";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".MANAGE_COLLABORATION_ACK",
                parameter =>
                {
                    parameter.AddWithValue("P_DOCUMENT_ID", DocID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COLLABORATION_NO", ColNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", UserID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_acknowledged_against", AcknowledgeAgainst, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT", null, OracleDbType.Varchar2, ParameterDirectionWrap.Output, 32767);
                },
               (records, instance) =>
               {
                   Res = records.GetStringOrDefault("P_RESULT");
               }
              );
            if (Res == "Sucess")
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region GetNotificationStatusList
        /// <summary>
        /// Collaboration status list
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">size of page</param>
        /// <param name="RefNo">Status code</param>
        /// <returns>List of collaboration status list</returns>
        public static List<NotificationStatusModel> GetNotificationStatusList(int pageNumber, int pageSize, string NotificationCode, string userSchema)
        {
            try
            {
                List<NotificationStatusModel> notificationStatusList = new List<NotificationStatusModel>();
                string spName = string.Empty;
                if (userSchema.ToLower() == UserSchema.Sort)
                {
                    spName = "GET_SORT_COLLABORATION_LIST";
                }
                else
                {
                    spName = "GET_COLLABORATION_STATUS_LIST";
                }
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    notificationStatusList,
                      userSchema + "." + spName,
                    parameter =>
                    {
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NOTIFICATION_CODE", NotificationCode.Trim(), OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                    },
                        (records, instance) =>
                        {
                            instance.DOCUMENT_ID = records.GetLongOrDefault("DOCUMENT_ID");
                            instance.FIRST_NAME = records.GetStringOrDefault("FIRST_NAME");
                            instance.SUR_NAME = records.GetStringOrDefault("SUR_NAME");
                            instance.FAX = records.GetStringOrDefault("FAX");
                            instance.EMAIL = records.GetStringOrDefault("EMAIL");
                            instance.ORGANISATION_ID = records.GetLongOrDefault("ORGANISATION_ID");
                            instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                            instance.COMMUNICATION_METHOD = records.GetStringOrDefault("COMMUNICATION_METHOD");
                            instance.WHEN = records.GetDateTimeOrDefault("WHEN");
                            instance.COLLABORATION_NO = records.GetInt16OrDefault("COLLABORATION_NO");
                            instance.CONTACT_ID = records.GetLongOrDefault("CONTACT_ID");
                            instance.StatusName = records.GetStringOrDefault("StatusName");
                            instance.NOTES = records.GetStringOrDefault("NOTES");
                            instance.InternalNotes = records.GetStringOrDefault("INT_NOTES");
                            instance.InternalStatusName = records.GetStringOrDefault("InternalStatusName");
                            instance.SeenBySort = records.GetInt16OrDefault("SEEN_BY_SORT");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");

                        }
                );
                return notificationStatusList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ManageCollaborationInternal
        /// <summary>
        /// Manage collaboration internal status
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        internal static bool ManageCollaborationInternal(NotificationStatusModel notificationStatus, string userSchema)
        {
            bool result = false;
            string spName = string.Empty;
            if (userSchema.ToLower() == UserSchema.Sort)
            {
                spName = "MANAGE_SORT_COLLABORATION_INT";
            }
            else
            {
                spName = "MANAGE_COLLABORATION_INTERNAL";
            }


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + "." + spName,
                parameter =>
                {
                    parameter.AddWithValue("P_DOCUMENT_ID", notificationStatus.DOCUMENT_ID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STATUS", notificationStatus.STATUS, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_NOTES", notificationStatus.NOTES, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                },
                (records, instance) =>
                {
                    result = true;
                }
            );
            return result;
        }

        #endregion

        #region GetExternalCollaboration
        /// <summary>
        /// Display collaboration history list
        /// </summary>
        /// <param name="page">Page </param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="DocumentId">Document id</param>
        /// <returns>Return collaboration history list</returns>
        public static List<CollaborationModel> GetExternalCollaboration(int pageNumber, int pageSize, int Document_Id, string userSchema)
        {
            try
            {
                string spName = string.Empty;
                if (userSchema.ToLower() == UserSchema.Sort)
                {
                    spName = "GET_SORT_COLLABORATION_EXT";
                }
                else
                {
                    spName = "GET_COLLABORATION_EXTERNAL";
                }

                List<CollaborationModel> collaborationModelList = new List<CollaborationModel>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    collaborationModelList,
                      userSchema + "." + spName,
                    parameter =>
                    {
                        parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_DOCUMENT_ID", Document_Id, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                    },
                        (records, instance) =>
                        {
                            instance.DocumentId = records.GetLongOrDefault("DOCUMENT_ID");
                            instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
                            instance.SurName = records.GetStringOrDefault("SUR_NAME");
                            instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                            instance.WHEN = records.GetDateTimeOrDefault("WHEN");
                            instance.ExternalCollaboratonStatus = records.GetStringOrDefault("ExternalCollaboratonStatus");
                            instance.ExternalNotes = records.GetStringOrDefault("ExternalNotes");
                            instance.InternalCollaborationStatus = records.GetStringOrDefault("InternalCollaborationStatus");
                            instance.InternalNotes = records.GetStringOrDefault("InternalNotes");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");

                        }
                );
                return collaborationModelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetInternalCollaboration
        /// <summary>
        /// Create internal collaboration
        /// </summary>
        /// <returns>Return data for internal collaboraton</returns>
        public static NotificationStatusModel GetInternalCollaboration(NotificationStatusModel notificationStatusModel, string userSchema)
        {
            try
            {
                string spName = string.Empty;
                if (userSchema.ToLower() == UserSchema.Sort)
                {
                    spName = "GET_SORT_COLLABORATION_INT";
                }
                else
                {
                    spName = "GET_COLLABORATION_INTERNAL";
                }

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    notificationStatusModel,
                      userSchema + "." + spName,
                    parameter =>
                    {

                        parameter.AddWithValue("P_COLLABORATION_NO", notificationStatusModel.COLLABORATION_NO, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_DOCUMENT_ID", notificationStatusModel.DOCUMENT_ID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                    },
                        (records, instance) =>
                        {
                            instance.FIRST_NAME = records.GetStringOrDefault("FIRST_NAME");
                            instance.SUR_NAME = records.GetStringOrDefault("SUR_NAME");
                            instance.ORGANISATION_ID = records.GetLongOrDefault("ORGANISATION_ID");
                            instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                            instance.NotificationStatus = records.GetStringOrDefault("NotificationStatus");
                            instance.STATUS = records.GetInt32OrDefault("STATUS");
                            instance.StatusName = records.GetStringOrDefault("StatusName");
                            instance.NOTES = records.GetStringOrDefault("NOTES");
                            instance.DOCUMENT_ID = records.GetLongOrDefault("DOCUMENT_ID");
                            instance.COLLABORATION_NO = records.GetInt16OrDefault("COLLABORATION_NO");
                            instance.CONTACT_ID = records.GetLongOrDefault("CONTACT_ID");
                            instance.WHEN = records.GetDateTimeOrDefault("WHEN");
                        }
                );
                return notificationStatusModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetCollaborationList

        /// <summary>
        /// The GetCollaborationList fetches the list of collaboration from the database
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="VersionID"></param>
        /// <returns></returns>
        public static List<CollaborationModel> GetCollaborationList(int pageNumber, int pageSize, string RefNo, int Notification_Id, int historic)
        {
            RefNo += "%";
            List<CollaborationModel> CollaborationList = new List<CollaborationModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                CollaborationList,
                UserSchema.Portal + ".GET_COLLABORATION_LIST",
                parameter =>
                {
                    parameter.AddWithValue("p_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RefNo", RefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Notification_Id", Notification_Id, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.Title = records.GetStringOrDefault("TITLE");
                    instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
                    instance.SurName = records.GetStringOrDefault("SUR_NAME");
                    instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                    instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                    DateTime dt = records.GetDateTimeOrDefault("WHEN");
                    var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                    instance.DateAndTime = TimeZoneInfo.ConvertTimeFromUtc(dt, ukTimeZone);
                    instance.Notes = records.GetStringOrDefault("NOTES");
                    instance.RecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    instance.ContactDetail = records.GetStringOrDefault("CONTACTDETAIL");
                }
            );
            return CollaborationList;
        }
        #endregion

        #region GetTransmissionList
        /// <summary>
        /// The GetTransmissionList, feteched the records for transmission from the database
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="RefNo"></param>
        /// <param name="Status"></param>
        /// <param name="StatusItemCount"></param>
        /// <returns></returns>
        public static List<TransmissionModel> GetTransmissionList(GetTransmissionListParams getTransmissionList)
        {
            List<TransmissionModel> transmissionList = new List<TransmissionModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                transmissionList,
                getTransmissionList.UserSchema + ".GET_TRANSMISSION_LIST",
                parameter =>
                {
                    parameter.AddWithValue("p_pageNumber", getTransmissionList.PageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_pageSize", getTransmissionList.PageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDALREFNO", getTransmissionList.ESDALRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STATUS", getTransmissionList.Status, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STATUS_ITEMS_COUNT", getTransmissionList.StatusItemCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", getTransmissionList.IsHistoric, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ContactID = Convert.ToInt32(records.GetLongOrDefault("CONTACT_ID"));
                    instance.FullName = records.GetStringOrDefault("FULL_NAME");
                    instance.OrganisationName = records.GetStringOrDefault("ORG_NAME");
                    instance.Fax = records.GetStringOrDefault("FAX_NUMBER");
                    instance.Email = records.GetStringOrDefault("EMAIL_ADDRESS");
                    instance.SentOnDate = records.GetDateTimeOrDefault("INBOX_STATUS_UPDATE_TIME");
                    instance.StatusUpdateTime = records.GetDateTimeOrDefault("STATUS_UPDATE_TIME");
                    instance.TransmissionStatus = Convert.ToInt32(records.GetDecimalOrDefault("TRANSMISSION_STATUS"));
                    instance.InboxStatusCode = records.GetInt32OrDefault("INBOX_STATUS");
                    instance.TransmissionStatusName = records.GetStringOrDefault("TRANSMISSION_STATUS_NAME");
                    instance.RecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    instance.TransmissionId = Convert.ToInt32(records.GetLongOrDefault("TRANSMISSION_ID"));
                    instance.OrganisationID = Convert.ToInt32(records.GetLongOrDefault("ORGANISATION_ID"));
                    instance.IsManuallyAdded = records.GetInt16OrDefault("IS_MANUALLY_ADDED");
                }
            );
            return transmissionList;
           

        }
        #endregion

        #region InsertNotificationType
        public static NotifGeneralDetails InsertNotificationType(PlanMovementType saveNotifType)
        {
            NotifGeneralDetails notifGeneral = new NotifGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                notifGeneral,
                saveNotifType.UserSchema + ".STP_MOVEMENT.SP_INSERT_NOTIFICATION",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", saveNotifType.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_ID", saveNotifType.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOVEMENT_ID", saveNotifType.MovementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_CLASS", saveNotifType.VehicleClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FROM_DESC", saveNotifType.FromDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_DESC", saveNotifType.ToDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_START_DATE", saveNotifType.MovementStart, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DATE", saveNotifType.MovementEnd, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_REF ", saveNotifType.HaulierRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", saveNotifType.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (record, instance) =>
                {
                    instance.NotificationId = record.GetLongOrDefault("NOTIFICATION_ID");
                    instance.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                    instance.AnalysisId = record.GetLongOrDefault("ANALYSIS_ID");
                    instance.VersionId = record.GetLongOrDefault("VERSION_ID");
                    instance.ContentRefNum = record.GetStringOrDefault("CONTENT_REF_NUM");
                }
            );
            return notifGeneral;
        }
        #endregion

        #region UpdateNotificationType
        public static NotifGeneralDetails UpdateNotificationType(PlanMovementType updateNotifType)
        {
            NotifGeneralDetails notifGeneral = new NotifGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                notifGeneral,
                updateNotifType.UserSchema + ".STP_MOVEMENT.SP_UPDATE_NOTIFICATION",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", updateNotifType.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_ID", updateNotifType.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOVEMENT_ID", updateNotifType.MovementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_CLASS", updateNotifType.VehicleClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FROM_DESC", updateNotifType.FromDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_DESC", updateNotifType.ToDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_START_DATE", updateNotifType.MovementStart, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DATE", updateNotifType.MovementEnd, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_REF", updateNotifType.HaulierRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_ID", updateNotifType.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_EDIT", updateNotifType.IsVehicleEdit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_AMEND", updateNotifType.IsVehicleAmended, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (record, instance) =>
                {
                    instance.NotificationId = record.GetLongOrDefault("NOTIFICATION_ID");
                    instance.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                    instance.AnalysisId = record.GetLongOrDefault("ANALYSIS_ID");
                    instance.VersionId = record.GetLongOrDefault("VERSION_ID");
                    instance.ContentRefNum = record.GetStringOrDefault("CONTENT_REF_NUM");
                }
            );
            return notifGeneral;
        }
        #endregion

        #region SetLoginStatus

        internal static int SetLoginStatus(int UserId, int flag)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_LOGIN_PKG.SP_SETLOGINSTATUS",
                //"PORTAL.SP_SETLOGINSTATUS",
                parameter =>
                {
                    parameter.AddWithValue("p_UserId", UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_flag", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = Convert.ToInt32(records.GetDecimalOrDefault("FLAG"));
                }
                );
            return result;
        }
        #endregion

        #region GetHaulierDetails
        public static HAContact GetHaulierDetails(long notificationId)
        {
            HAContact contact = new HAContact();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                contact,
                UserSchema.Portal + ".SP_GET_NOTIF_HAULIER_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                    instance.OrganisationName = records.GetStringOrDefault("HAULIER_ORG_NAME");
                    instance.Telephone = records.GetStringOrDefault("HAULIER_TEL_NO");
                    instance.Fax = records.GetStringOrDefault("HAULIER_Fax_NO");
                    instance.Email = records.GetStringOrDefault("HAULIER_Email");
                    instance.PostCode= records.GetStringOrDefault("haulier_post_code"); 
                    instance.HAAddress1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                    instance.HAAddress2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                    instance.HAAddress3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                    instance.HAAddress4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                    instance.HAAddress5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                    instance.Country = records.GetStringOrDefault("HAULIER_COUNTRY");
                }
                );
            return contact;
        }

        #endregion

        #region Get Non Esdal Notification general Details
        public static Domain.ExternalAPI.ExportNotifGeneralDetails ExportNotifGeneralDetails(string esdalRefNumber)
        {
            Domain.ExternalAPI.ExportNotifGeneralDetails notifGeneralDetails = new Domain.ExternalAPI.ExportNotifGeneralDetails();
            string notifJson = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               notifJson,
               UserSchema.Portal + ".STP_EXPORT_DETAILS.SP_GET_NOTIF_GEN_DETAILS",
               parameter =>
               {
                   parameter.AddWithValue("p_ESDAL_REF_NO", esdalRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               records =>
               {
                   notifJson = records.GetStringOrDefault("NOTIF_JSON");
               });
            if (!string.IsNullOrWhiteSpace(notifJson))
                notifGeneralDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.ExternalAPI.ExportNotifGeneralDetails>(notifJson);
            return notifGeneralDetails;
        }
        #endregion

        #region Removed Unwanted code by Mahzeer on 04-12-2023

        #region Get Notification Route Details
        //public static List<ListRouteVehicleId> GetNotifRouteDetails(string contentReferenceNo)
        //{
        //    List<ListRouteVehicleId> objDetails = new List<ListRouteVehicleId>();
        //    try
        //    {
        //        SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //            objDetails,
        //               UserSchema.Portal + ".SP_GET_NOTIF_CLONE_ROUTEID",
        //            parameter =>
        //            {
        //                parameter.AddWithValue("P_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
        //                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //            },
        //                (records, instance) =>
        //                {
        //                    instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
        //                    instance.PartDescr = records.GetStringOrDefault("PART_NAME");
        //                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
        //                    instance.RouteCount = (int)records.GetDecimalOrDefault(":B2");
        //                }
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{ConfigurationManager.AppSettings["Instance"] } - NotificationDAO/GetNotifRouteDetails, Exception: {ex}");
        //        throw;
        //    }
        //    return objDetails;
        //}
        #endregion

        #region Get Haulier Licence
        //public static string GETHAULIERLICENCEDAO(int organisationId)
        //{
        //    string haulierLicence = "";
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance
        //    (
        //        haulierLicence,
        //        UserSchema.Portal + ".SP_GETHAULIERLICENCE",
        //         parameter =>
        //         {
        //             parameter.AddWithValue("P_Organisation_id", organisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //             parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //         },
        //         records =>
        //         {
        //             haulierLicence = records.GetStringOrDefault("Licence");
        //         }
        //    );
        //    return haulierLicence;
        //}
        #endregion

        #region GetPreviousAnalysisId
        /// <summary>
        /// Return parent notification analysis id.
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        //public static long GetPreviousAnalysisId(long notificationId)
        //{
        //    long analysisId = 0;
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        analysisId,
        //        UserSchema.Portal + ".STP_NOTIFICATION.SP_GET_PARENT_ANALYSISID_RENOTIFY",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //        records =>
        //        {
        //            analysisId = records.GetLongOrDefault("P_ANALYSIS_ID");
        //        }
        //        );
        //    return analysisId;
        //}
        #endregion

        #region GetPrintCollabList
        //public static List<CollaborationModel> GetPrintCollabList(string RefNo, int notificationId)
        //{
        //    List<CollaborationModel> CollaborationList = new List<CollaborationModel>();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //        CollaborationList,
        //        UserSchema.Portal + ".GET_PRINT_COLLAB_LIST",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_RefNo", RefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_Notification_Id", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //            (records, instance) =>
        //            {
        //                instance.Title = records.GetStringOrDefault("TITLE");
        //                instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
        //                instance.SurName = records.GetStringOrDefault("SUR_NAME");
        //                instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
        //                instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
        //                instance.DateAndTime = records.GetDateTimeOrDefault("WHEN");
        //                instance.Notes = records.GetStringOrDefault("NOTES");
        //                instance.RecordCount = records.GetDecimalOrDefault("TOTALRECORDCOUNT");
        //                instance.ContactDetail = records.GetStringOrDefault("CONTACTDETAIL");
        //            }
        //    );
        //    return CollaborationList;
        //}
        #endregion

        #endregion

    }
}
