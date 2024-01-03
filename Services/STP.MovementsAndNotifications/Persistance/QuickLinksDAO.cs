using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Common.Constants;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class QuickLinksDAO
    {
        #region Get Content Reference No
        internal static string Get_CONTENT_REF_NO(int notificationNo)
        {
            string ContentReferenceNo = "0";
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    ContentReferenceNo,
                    UserSchema.Portal + ".STP_NOTIFICATION.SP_Get_CONTENT_REF_NO",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", notificationNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        ContentReferenceNo = Convert.ToString(records.GetStringOrDefault("PLANNED_CONTENT_REF_NO"));
                    }
                    );
            return ContentReferenceNo;
        }
        #endregion

        #region Insert Quick Links SOA
        internal static int InsertQuickLinksSOA(int organisationId, int inboxId, int userId)
        {
            int linkNo = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    linkNo,
                    UserSchema.Portal + ".SP_INSERT_SOA_QUICKLINKS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ITEM_ID", inboxId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        linkNo = Convert.ToInt32(records.GetLongOrDefault("QUICK_REF"));
                    }
                    );
            return linkNo;
        }
        #endregion

        #region Insert Quick Links
        internal static int InsertQuickLinks(InsertQuickLinkParams objInsertQuickLinkParams)
        {
            int linkno = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    linkno,
                    UserSchema.Portal + ".sp_insert_quick_links",
                    parameter =>
                    {
                        parameter.AddWithValue("p_user_id", objInsertQuickLinkParams.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VER_ID", objInsertQuickLinkParams.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_REV_ID", objInsertQuickLinkParams.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_notif_id", objInsertQuickLinkParams.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_resultset", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        linkno = Convert.ToInt32(records.GetLongOrDefault("link_no"));
                    }
                    );
            return linkno;
        }
        #endregion

        #region Get Quick Links SOA
        public static List<QuickLinksSOA> GetQuickLinksSOA(int userId)
        {
            var objQuickLinks = new List<QuickLinksSOA>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objQuickLinks,
                UserSchema.Portal + ".SP_GET_SOA_QUICKLINKS",
                parameter =>
                {
                    parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.NotificationId = Convert.ToInt32(records["NOTIFICATION_ID"]);
                        instance.ESDALReferenceNo = records.GetStringOrDefault("ESDAL_REFERENCE");
                        instance.InboxId = Convert.ToInt32(records["INBOX_ITEM_ID"]);
                        int Notif_VerNo = 0;
                        Notif_VerNo = Convert.ToInt16(records["NOTIFICATION_VERSION_NO"]);
                        instance.Route = ConvertMessage(records.GetInt32OrDefault("ITEM_TYPE"), Notif_VerNo);
                        instance.ItemStatus = Convert.ToInt32(records["ITEM_STATUS"]);
                        instance.NENId = Convert.ToInt64(records["NEN_ID"]);
                    }
            );
            return objQuickLinks;

        }
        #endregion

        #region Private Methods
        #region private static string ConvertMessage(int messageCode)
        private static string ConvertMessage(int messageCode, int Notif_VerNo = 0)
        {
            string message = string.Empty;
            //------FOR NEN only----------
            if (Notif_VerNo > 1 && (messageCode == 911001 || messageCode == 911002 || messageCode == 911003 || messageCode == 911004 || messageCode == 911005 || messageCode == 911010 || messageCode == 911011))// Case is for NE Renotification for NEN processing thru all planning and assigning scrutiny
            {
                messageCode = 312015;
            }
            //----------------------------
            switch (messageCode)
            {
                case 312001:
                    message = "Proposal";
                    break;
                case 312002:
                    message = "Reproposal";
                    break;
                case 312003:
                    message = "Withdrawal";
                    break;
                case 312004:
                    message = "Declination";
                    break;
                case 312005:
                    message = "Agreement";
                    break;
                case 312006:
                    message = "Amendment to agreement";
                    break;
                case 312007:
                    message = "Recleared";
                    break;
                case 312008:
                    message = "No longer affected";
                    break;
                case 312009:
                    message = "Notification";
                    break;
                case 312010:
                    message = "Renotification";
                    break;
                case 312011:
                    message = "Delivery failure";
                    break;
                case 312012:
                    message = "Delegation failure alert";
                    break;
                case 312013:
                    message = "VRL planned route";
                    break;
                case 312014://New NEN came from Email service to DB
                case 911001://Unplanned
                case 911002://Planned
                case 911003://Planning error
                case 911004://Replanned
                case 911005://Assigned for scrutiny unplanned
                case 911010://Assigned for scrutiny planned
                case 911011://Assigned for scrutiny replanned
                    message = "NE Notification";
                    break;
                case 312015://NEN Renotification came from Email service to DB
                    message = "NE Renotification";
                    break;
                case 911006://Agreed
                case 911007://Accepted
                case 911008://Rejected
                case 911009://Under assessment
                    message = "NE agreed notification";
                    break;
                default:
                    break;
            }
            return message;
        }
        #endregion
        #endregion

        #region Get QuickLinks
        public static List<QuickLinks> GetQuickLinks(int userId)
        {
            var objQuickLinks = new List<QuickLinks>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objQuickLinks,
                UserSchema.Portal + ".SP_GET_QUICK_LINKS",
                parameter =>
                {
                    parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                        instance.VehicleClassification = Convert.ToInt32(records.GetInt32OrDefault("VEHICLE_CLASSIFICATION"));
                        instance.NotificationNumber = Convert.ToInt32(records.GetInt16Nullable("NOTIFICATION_NO"));
                        instance.NotificationVersionNumber = Convert.ToInt32(records.GetInt16Nullable("NOTIFICATION_VERSION_NO"));
                        instance.VersionNumber = Convert.ToInt32(records.GetInt16Nullable("VERSION_NO"));
                        instance.LinkNumber = Convert.ToInt64(records.GetLongOrDefault("LINK_NO"));

                        instance.VersionId = Convert.ToInt64(records.GetLongOrDefault("VERSION_ID"));
                        instance.RevisionNumber = Convert.ToInt32(records.GetInt16Nullable("REVISION_NO"));
                        instance.HaulierMnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                        instance.ESDALReferenceNumber = Convert.ToInt32(records.GetInt32OrDefault("ESDAL_REF_NUMBER"));
                        instance.ESDALReference = records.GetStringOrDefault("ESDAL_REF");
                        instance.NotificationId = Convert.ToInt32(records.GetLongOrDefault("NOTIFICATION_ID"));

                        instance.RevisionId = Convert.ToInt32(records.GetLongOrDefault("REVISION_ID"));
                        instance.ProjectId = Convert.ToInt32(records.GetLongOrDefault("PROJECT_ID"));
                        instance.ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                        instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                        instance.IsNotified = records.GetShortOrDefault("IS_NOTIFIED");
                    }
            );

            return objQuickLinks;
        }
        #endregion 
    }
}
