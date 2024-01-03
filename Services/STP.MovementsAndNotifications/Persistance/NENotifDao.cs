using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.NonESDAL;
using System;
using System.Collections.Generic;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class NENotifDao
    {
        #region NEN Notification 
        public static NotificationGeneralDetails SaveNENotification(NENotifGeneralDetails notifGeneralDetails)
        {
            NotificationGeneralDetails notificationGeneralDetails = new NotificationGeneralDetails();
            
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                notificationGeneralDetails,
               UserSchema.Portal + ".STP_NE_GENERAL.SP_INSERT_NE_NOTIFICATION",
                parameter =>
                {
                    parameter.AddWithValue("P_CLIENT", notifGeneralDetails.Client, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_REF", notifGeneralDetails.HauliersReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTES_ON_ESCORT", notifGeneralDetails.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);

                    parameter.AddWithValue("P_NON_ESDAL_KEY", notifGeneralDetails.NonEsdalKeyId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ORG", notifGeneralDetails.HaulierOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_CONT", notifGeneralDetails.HaulierContact, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_EMAIL", notifGeneralDetails.HaulierEmail, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_TEL", notifGeneralDetails.HaulierTelephoneNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_FAX", notifGeneralDetails.HaulierFaxNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_LIC", notifGeneralDetails.HaulierLicence, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_HAUL_ADDRESS", notifGeneralDetails.HaulierAddressLine, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_POSTCODE", notifGeneralDetails.HaulierPostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_COUNTRY", notifGeneralDetails.HaulierCountry, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_LOAD_DESC ", notifGeneralDetails.LoadDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NO_OF_MOV", notifGeneralDetails.TotalMoves, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NO_OF_PIECES", notifGeneralDetails.MaxPiecesPerMove, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_FROM_DESCR", notifGeneralDetails.FromSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_DESCR ", notifGeneralDetails.ToSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOV_START", TimeZoneInfo.ConvertTimeToUtc(notifGeneralDetails.MovementStart), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOV_END", TimeZoneInfo.ConvertTimeToUtc(notifGeneralDetails.MovementEnd), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_INDEMNITY", notifGeneralDetails.Indemnity, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_REQUIRE_VR1", notifGeneralDetails.RequireVR1, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REQUIRE_SO", notifGeneralDetails.RequireSo , OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VSO_TYPE", notifGeneralDetails.VSOType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_APP_REFERENCE", notifGeneralDetails.ApplicationReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_VEHICLE_CLASS", notifGeneralDetails.Classification, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTES_FROM_HAULIER", notifGeneralDetails.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);

                    parameter.AddWithValue("P_PREV_ESDAL_REF", notifGeneralDetails.PrevEsdalRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (record,instance) =>
                 {
                     instance.NotificationId = record.GetLongOrDefault("NOTIFICATION_ID");
                     instance.ContentReferenceNo = record.GetStringOrDefault("CONTENT_REF_NUM");
                     instance.AnalysisId = record.GetLongOrDefault("ANALYSIS_ID");
                 }
            );
            return notificationGeneralDetails;
        }

        public static ValidNERenotif IsNenRenotified(string prevEsdalRef)
        {
            ValidNERenotif validNERenotif = new ValidNERenotif();
            DateTime now = DateTime.Now;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance
            (
                validNERenotif,
                UserSchema.Portal + ".STP_NE_GENERAL.SP_CHECK_NEN_NOTIF",
                parameter =>
                {
                    parameter.AddWithValue("P_PREV_ESDAL_REF", prevEsdalRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.IsNotExist = (int)records.GetDecimalOrDefault("IS_EXIST") == 0;
                    instance.IsRenotified = (int)records.GetDecimalOrDefault("IS_RENOTIFY") == 1;
                    instance.NonEsdalKeyId = (int)records.GetDecimalOrDefault("NON_ESDAL_KEY");
                }
            );
            return validNERenotif;
        }

        #region GenerateNotifCode
        public static string GenerateNENotifCode(long notificationId, int isIndementiy)
        {
            string NotificationCode = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   notificationId,
                  UserSchema.Portal + ".STP_NE_GENERAL.SP_UPDATE_NE_NOTIF",
                   parameter =>
                   {
                       parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_INDEMENITY", isIndementiy, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                   record =>
                   {
                       NotificationCode = record.GetStringOrDefault("NOTIF_CODE");
                   }
                   );
            return NotificationCode;
        }
        #endregion

        public static int GetWorkingDays(DateTime moveStartDate, int countryId)
        {
            int result = 0;
            DateTime now = DateTime.Now;
            if (moveStartDate.Date == now.Date)
            {
                result = 0;
            }
            else
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance
                (
                    result,
                   UserSchema.Portal + ".SP_GET_WORKING_DAYS",
                     parameter =>
                     {
                         parameter.AddWithValue("P_Current_date", now.Date, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_moveStartDate", moveStartDate.Date, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_countryId", countryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         result = (int)records.GetDecimalOrDefault("VAL_CNT");
                     }
                );
            }
            return result;
        }

        #endregion

        #region GetNENotificationStatus
        public static List<NENotificationStatusOutput> GetNENotificationStatus(string ESDALReferenceNumber)
        {
            List<NENotificationStatusOutput> NENotificationStatusList = new List<NENotificationStatusOutput>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                NENotificationStatusList,
               UserSchema.Portal + ".STP_EXPORT_DETAILS.SP_GET_NE_NOTIF_STATUS",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDAL_REF_NO", ESDALReferenceNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                     string notes = records.GetStringOrDefault("collab_notes");
                     List<string> collabs = null;
                     if (!string.IsNullOrWhiteSpace(notes))
                     {
                         string[] collabnotes = notes.Split(';');
                         collabs = new List<string>();
                         foreach (var item in collabnotes)
                         {
                             if (!string.IsNullOrWhiteSpace(item))
                                 collabs.Add(item);
                         }
                     }
                     instance.CollaborationNotes = collabs;
                 }
            );

            return NENotificationStatusList;
        }
        #endregion
        
    }
}