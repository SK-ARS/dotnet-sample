using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using System;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.Communications;
using Newtonsoft.Json;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class SimpleNotificationDAO
    {

        #region Get Notification General Details
        public static NotificationGeneralDetails GetNotifGeneralDetails(long notificationId, int historic)
        {
            NotificationGeneralDetails objDetails = new NotificationGeneralDetails();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    objDetails,
                       UserSchema.Portal + ".STP_NOTIFICATION.SP_GET_NOTIF_GEN_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIF_ID ", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.NotificationId = notificationId;
                            instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                            instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                            instance.MovementName = records.GetStringOrDefault("MOVEMENT_NAME");
                            instance.ESDALReference = records.GetStringOrDefault("NOTIFICATION_CODE");
                            instance.MyReference = records.GetStringOrDefault("MY_REF");
                            instance.SONumbers = records.GetStringOrDefault("so_numbers");
                            instance.VehicleLength = (decimal)records.GetDoubleOrDefault("len_max_mtr");
                            instance.RigidLength = (decimal)records.GetDoubleOrDefault("rigid_len_max_mtr");
                            instance.VehicleWidth = (decimal)records.GetDoubleOrDefault("width_max_mtr");
                            instance.MaximamHeight = (decimal)records.GetDoubleOrDefault("max_height_max_mtr");
                            instance.ReducibleHeight = (decimal)records.GetDoubleOrDefault("red_height_max_mtr");
                            instance.GrossWeight = records.GetInt32OrDefault("gross_weight_max_kg");
                            instance.AxelWeight = records.GetInt32OrDefault("max_axle_weight_max_kg");
                            instance.RegisterNo = records.GetStringOrDefault("plate_nos");
                            instance.FleetNos = records.GetStringOrDefault("fleet_nos");
                            instance.ClientName = records.GetStringOrDefault("CLIENT");
                            instance.From = records.GetStringOrDefault("FROM_DESCR");
                            instance.FromSummary = records.GetStringOrDefault("FROM_DESCR");
                            instance.FromAddress = records.GetStringOrDefault("FROM_DESCR");
                            instance.To = records.GetStringOrDefault("TO_DESCR");
                            instance.ToSummary = records.GetStringOrDefault("TO_DESCR");
                            instance.ToAddress = records.GetStringOrDefault("TO_DESCR");
                            instance.LoadDescription = records.GetStringOrDefault("load_descr");
                            instance.VehicleCode = records.GetInt32OrDefault("vehicle_classification");
                            instance.Classification = records.GetStringOrDefault("NAME");
                            instance.FromDateTime =records.GetDateTimeOrDefault("START_DATE");
                            instance.ToDateTime = records.GetDateTimeOrDefault("END_DATE");
                            instance.MovementDateFrom = records.GetDateTimeOrDefault("START_DATE");
                            instance.MovementDateTo = records.GetDateTimeOrDefault("END_DATE");
                            instance.NoOfMovements = records.GetInt16OrDefault("NO_OF_MOVES");
                            instance.MaxPiecesPerLoad = records.GetInt32OrDefault("max_pieces_per_move");
                            instance.HaulierOprLicence = records.GetStringOrDefault("HAUL_LICENCE");
                            instance.ContentReferenceNo = records.GetStringOrDefault("PLANNED_CONTENT_REF_NO");
                            instance.RevId = records.GetLongOrDefault("revision_id");
                            instance.NotesOnEscort = records.GetStringOrDefault("notesonescort");
                            instance.VersionNo = records.GetInt16OrDefault("version_no");
                            instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                            instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                            instance.ProjectStatus = records.GetInt32OrDefault("PROJECT_STATUS");
                            instance.RequiresVR1 = records.GetInt16OrDefault("V_REQVR1");
                            instance.VR1Number = records.GetStringOrDefault("V_VR1NUM");
                            instance.IndemnifyFlag = records.GetInt16OrDefault("INDEMNITY_CONFIRMATION") > 0;
                            instance.Notes = records.GetStringOrDefault("NOTES");
                            instance.ActingOnBehalfOf = records.GetStringOrDefault("ON_BEHALF_OF");
                            instance.VSONumber = records.GetStringOrDefault("vso_number");
                            instance.IsMostRecent = records.GetInt16OrDefault("is_most_recent");
                            instance.ShowWarning = (int)records.GetDecimalOrDefault("V_SHOW_WARN");
                            instance.DispensationId = records.GetStringOrDefault("dispensation_id");
                        }
                );
            return objDetails;
        }
        #endregion

        #region GetNotificationAffectedStructures
        /// <summary>
        /// GetNotificationAffectedStructures
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="esdalReferenceNumber"></param>
        /// <param name="haulierMnemonic"></param>
        /// <param name="versionNumber"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static byte[] GetNotificationAffectedStructures(int notificationId, string esdalReferenceNumber, string haulierMnemonic, string versionNumber, string userSchema)
        {
            byte[] affectedStructure = null;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        affectedStructure,
                        userSchema + ".SP_GET_AFFECTED_STUCTURES",
                        parameter =>
                        {
                            parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_ESDAL_REF_NO", Convert.ToInt32(esdalReferenceNumber), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_HAULIER_MNEMONIC", haulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_VERSION_NO", Convert.ToInt32(versionNumber), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                        },
                        record =>
                        {
                            affectedStructure = record.GetByteArrayOrNull("AFFECTED_STRUCTURES");
                        }
                    );
            return affectedStructure;
        }
        #endregion

        #region GetOrderNoProjectId
        public static MovementPrint GetOrderNoProjectId(int versionId)
        {
            MovementPrint movementPrint = new MovementPrint();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   movementPrint,
                      UserSchema.Sort + ".SP_GET_ORDERNO",
                   parameter =>
                   {
                       parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                       (records, instance) =>
                       {
                           instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                           instance.OrderNumber = records.GetStringOrDefault("V_ORDERNO");
                       }
               );
            return movementPrint;
        }
        #endregion

        #region GetProjectIdByEsdalReferenceNo
        public static MovementPrint GetProjectIdByEsdalReferenceNo(string EsdalRefNo)
        {
            MovementPrint movementPrint = new MovementPrint();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   movementPrint,
                      UserSchema.Sort + ".SP_GET_PROJECT_ID_BY_ESDALREFNO",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ESDALREFNO", EsdalRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                       (records, instance) =>
                       {
                           instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                       }
               );
            return movementPrint;
        }
        #endregion

        #region DeleteNotification
        public static int DeleteNotification(int notificationId)
        {
            int affectedRows = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                            UserSchema.Portal + ".STP_NOTIFICATION.SP_DELETE_NOTIFICATION",
                        parameter =>
                        {
                            parameter.AddWithValue("P_NOTIF_ID‏", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                            parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                        },
                         record =>
                         {
                             affectedRows = record.GetInt32("P_AFFECTED_ROWS");
                         }                       
                        );
            return affectedRows;
        }
        #endregion

        #region GetNotifAffectedParties
        public static byte[] GetNotifAffectedParties(int notificationId, string userSchema = UserSchema.Portal)
        {
            byte[] affectedParties = null;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        affectedParties,
                        userSchema + ".SP_CHECK_AFFECTED_PARTIES",
                        parameter =>
                        {
                            parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                        },
                        record =>
                        {
                            affectedParties = record.GetByteArrayOrNull("AFFECTED_PARTIES");
                        }
                    );
                return affectedParties;
        }
        #endregion

        #region GetResponseMailDetails
        public static MailResponse GetResponseMailDetails(int organisationId, string userSchema = UserSchema.Portal)
        {
            MailResponse responseMailDetails = new MailResponse();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        responseMailDetails,
                        userSchema + ".STP_USER_PREFERENCES.SP_GET_CONTACT_DETAILS",
                        parameter =>
                        {
                            parameter.AddWithValue("C_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("C_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                        },
                        record =>
                        {
                            responseMailDetails.EmailID = record.GetStringOrDefault("EMAIL_ID");
                            responseMailDetails.ReplyMailPdf = record.GetStringOrDefault("REPLY_PDF");
                            responseMailDetails.ReplyMailText = record.GetByteArrayOrNull("REPLY_TEXT");
                        }
                    );
            return responseMailDetails;
        }
        #endregion
        
        #region SaveAffectedNotificationDetails
        public static bool SaveAffectedNotificationDetails(AffectedStructConstrParam affectedParam)
        {
            var sectionListJson = JsonConvert.SerializeObject(affectedParam.AffectedSections);
            var constraintList = string.Join(",", affectedParam.AffectedConstraints);
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Portal + ".SP_SAVE_AFFECTED_NOTIF_DET",//SP_SAVE_AFFECTED_NOTIF_DET_TEST",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", affectedParam.NotificationId, OracleDbType.Long, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_AFFECTED_SECTIONS", sectionListJson, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_AFFECTED_CONSTRAINTS", constraintList, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        result = Convert.ToInt32(records["COUNT"]);
                    });

            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region UpdateNotification
        public static int UpdateNotification(NotificationGeneralDetails notificationGeneralDetails)
        {
            int count = 0;
            int indemnity = notificationGeneralDetails.IndemnifyFlag ? 1 : 0;
            var dispListJson = notificationGeneralDetails.DispensationList != null && notificationGeneralDetails.DispensationList.Count  >0 ? JsonConvert.SerializeObject(notificationGeneralDetails.DispensationList):null;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".STP_MOVEMENT.SP_SAVE_NOTIF_GENERAL",
                    parameter =>
                    {
                        parameter.AddWithValue("P_HAUL_ORG", notificationGeneralDetails.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USERID", notificationGeneralDetails.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NOTIF_ID", notificationGeneralDetails.NotificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_MOVE_START", notificationGeneralDetails.FromDateTime, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_MOVE_END", notificationGeneralDetails.ToDateTime, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_LOAD_DESC", notificationGeneralDetails.LoadDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("P_FROM_DESC", notificationGeneralDetails.FromSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("P_TO_DESC", notificationGeneralDetails.ToSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("P_CLIENT_DESC", notificationGeneralDetails.ClientName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("P_HAUL_REF", notificationGeneralDetails.MyReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("P_PLATE_NO", notificationGeneralDetails.RegisterNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("P_NO_OF_MOVES", notificationGeneralDetails.NoOfMovements, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_MAX_PIECES", notificationGeneralDetails.MaxPiecesPerLoad, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NOTES", notificationGeneralDetails.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999); 
                        parameter.AddWithValue("P_NOTES_ON_ESCORT", notificationGeneralDetails.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("P_INDEMNITY", indemnity, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ACT_BEHALF", notificationGeneralDetails.ActingOnBehalfOf, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_HAUL_LICENCE", notificationGeneralDetails.HaulierOprLicence, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VSO_NUMBER", notificationGeneralDetails.VSONumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VSO_TYPE", notificationGeneralDetails.VSOType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VR1_NUMBER", notificationGeneralDetails.VR1Number, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_REQUIRE_VR1", notificationGeneralDetails.RequiresVR1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SO_NUMBER", notificationGeneralDetails.SONumbers, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_DISP_LIST", dispListJson, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                        parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                    },
                    record =>
                    {
                        count = record.GetInt32("P_AFFECTED_ROWS");
                    }
                    );
            return count;
        }
        #endregion

        #region Notify Application
        public static NotificationGeneralDetails NotifyApplication(long versionId)
        {
            NotificationGeneralDetails objNotify = new NotificationGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            objNotify,
                UserSchema.Portal + ".STP_NOTIFICATION.SP_NOTIFY_APPLICATIONS",
            parameter =>
            {
                parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
                instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                instance.ContentReferenceNo = records.GetStringOrDefault("planned_content_ref_no");
                instance.VehicleCode = records.GetInt32OrDefault("vehicle_classification");
                instance.MovementId = (long)records.GetDecimalOrDefault("V_MOVEMENT_ID");
                instance.PreviousContactName = records.GetStringOrDefault("HAULIER_CONTACT");
            });
            return objNotify;
        }
        #endregion
        
        #region FetchNotifVersion(int notificationId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        internal static int FetchNotifVersion(int notificationId)
        {
                int iNotifVer = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                       iNotifVer,
                      UserSchema.Portal + ".STP_NOTIFICATION.SP_NOTIF_GET_NOTIF_VER",
                       parameter =>
                       {
                           parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                       },
                       record =>
                       {
                           iNotifVer = (int)record.GetDecimalOrDefault("VERSION_NO");
                       }
                       );
                return iNotifVer;
        }
        #endregion

        #region GenerateNotifCode
        public static string GenerateNotifCode(int organisationId, long notificationId, int detail)
        {
            string NotificationCode = string.Empty;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                       notificationId,
                      UserSchema.Portal + ".STP_NOTIFICATION.SP_NOTIF_UPDATE",
                       parameter =>
                       {
                           parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_DETAIL", detail, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        
        #region ReNotifyNotification
        public static NotificationGeneralDetails RenotifyNotification(int notificationId, int VR1_Renotify)
        {
            NotificationGeneralDetails objReNotify = new NotificationGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   objReNotify,
                      UserSchema.Portal + ".STP_NOTIFICATION.SP_RENOTIFY",
                   parameter =>
                   {
                       parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                       parameter.AddWithValue("P_VR1", VR1_Renotify, OracleDbType.Int32, ParameterDirectionWrap.Input);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                       (records, instance) =>
                       {
                           instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
                           instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                           instance.NotificationCode = records.GetStringOrDefault("notification_code");
                           instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                           instance.ContentReferenceNo = records.GetStringOrDefault("planned_content_ref_no");
                           instance.VehicleCode = records.GetInt32OrDefault("vehicle_classification");
                           instance.MovementId = (long)records.GetDecimalOrDefault("MOVEMENT_ID");
                           instance.RequiresVR1 = records.GetInt16OrDefault("V_REQVR1");
                           instance.VSOType = records.GetInt32OrDefault("VSO_TYPE");
                           instance.PreviousContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                       }
               );
            return objReNotify;
        }
        #endregion
        
        #region CloneNotification
        public static NotificationGeneralDetails CloneNotifications(int notificationId)
        {
            NotificationGeneralDetails objCloneNotif = new NotificationGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                 objCloneNotif,
                   UserSchema.Portal + ".STP_NOTIFICATION.SP_NOTIF_REVISE_CLONE",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID‏", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULT_SET‏", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
                        instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                        instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                        instance.VehicleCode = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        instance.ContentReferenceNo = records.GetStringOrDefault("REF_NO");
                        instance.MovementId = (long)records.GetDecimalOrDefault("MOVEMENT_ID");
                        instance.RequiresVR1 = records.GetInt16OrDefault("V_REQVR1");
                        instance.VSOType = records.GetInt32OrDefault("VSO_TYPE");
                        instance.PreviousContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                    }
            );
            return objCloneNotif;
        }

        public static NotificationGeneralDetails CloneHistoricNotification(int notificationId)
        {
            NotificationGeneralDetails objCloneNotif = new NotificationGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                 objCloneNotif,
                 UserSchema.Portal + ".STP_HISTORIC_MOVEMENT.SP_CLONE_NOTIFICATION",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID‏", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULT_SET‏", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
                    instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                    instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                    instance.VehicleCode = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    instance.ContentReferenceNo = records.GetStringOrDefault("REF_NO");
                    instance.MovementId = (long)records.GetDecimalOrDefault("MOVEMENT_ID");
                    instance.RequiresVR1 = records.GetInt16OrDefault("V_REQVR1");
                    instance.VSOType = records.GetInt32OrDefault("VSO_TYPE");
                    instance.PreviousContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                });
            return objCloneNotif;
        }
        #endregion

        #region Removed Unwanted code by Mahzeer on 04-12-2023

        #region Get Max Reduciable Height
        //public static decimal GetMaxReducibleHeight(int notificationId)
        //{
        //    decimal result = 0;
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //           result,
        //              UserSchema.Portal + ".SP_GET_MAX_RED_HGT",
        //           parameter =>
        //           {
        //               parameter.AddWithValue("P_NOTIFICATIONID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //               parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //           },
        //               (records) =>
        //               {
        //                   result = records.GetDecimalOrDefault("MAX_RED_HGT");
        //               }
        //       );
        //    return result;
        //}
        #endregion

        #region Set Notification VR Num
        //public static bool SetNotificationVRNum(int notificationId)
        //{
        //    int result = 0;
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        result,
        //        UserSchema.Portal + ".SP_SET_VRNUM_IN_NOTIF",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //        record =>
        //        {
        //            result = (int)record.GetDecimalOrDefault(":B1");
        //        }
        //    );
        //    if (result == 1)
        //        return true;
        //    else
        //        return false;
        //}
        #endregion

        #region GetNotifHaulierDetails
        //public static UserRegistration GetNotifHaulierDetails(int userId, int vehicleClassCode = 0, int notificationId = 0)
        //{
        //    UserRegistration objDetails = new UserRegistration();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        objDetails,
        //           UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_GET_HAULIER_CONTACT",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("HC_USER_ID ", userId, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //            parameter.AddWithValue("HC_VEHICLECLASS_CODE ", vehicleClassCode, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //            parameter.AddWithValue("HC_NOTFICATION_ID ", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //            parameter.AddWithValue("HC_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //            (records, instance) =>
        //            {
        //                instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
        //                instance.FirstName = records.GetStringOrDefault("FIRST_NAME");
        //                instance.SurName = records.GetStringOrDefault("SUR_NAME");
        //                instance.AddressLine1 = records.GetStringOrDefault("ADDRESSLINE_1");
        //                instance.AddressLine2 = records.GetStringOrDefault("ADDRESSLINE_2");
        //                instance.AddressLine3 = records.GetStringOrDefault("ADDRESSLINE_3");
        //                instance.AddressLine4 = records.GetStringOrDefault("ADDRESSLINE_4");
        //                instance.AddressLine5 = records.GetStringOrDefault("ADDRESSLINE_5");
        //                instance.PostCode = records.GetStringOrDefault("POSTCODE");
        //                instance.Telephone = records.GetStringOrDefault("PHONENUMBER");
        //                instance.Fax = records.GetStringOrDefault("FAX");
        //                instance.Country = records.GetStringOrDefault("HC_COUNTRY");
        //                instance.Email = records.GetStringOrDefault("EMAIL");
        //                instance.FromDescrp = records.GetStringOrDefault("HC_FROM_DESCR");
        //                instance.ToDescrp = records.GetStringOrDefault("HC_TO_DESCR");
        //                instance.LicenceNumber = records.GetStringOrDefault("HC_LICENCE_NR");
        //            }
        //    );
        //    return objDetails;
        //}
        #endregion

        #region SaveGeneralDetails
        //public static NotificationGeneralDetails SaveNotifGeneralDetails(NotificationGeneralDetails notificationGeneralDetails)
        //{
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        notificationGeneralDetails,
        //        UserSchema.Portal + ".STP_NOTIFICATION.SP_INSERT_NOTIFICATION",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_HAUL_ORG", notificationGeneralDetails.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_NOTIF_CODE", notificationGeneralDetails.NotificationCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_USERID", notificationGeneralDetails.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_L_CAT", notificationGeneralDetails.VehicleCategory, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_PROJ_STATUS", notificationGeneralDetails.ProjectStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_NEEDS_ATT", notificationGeneralDetails.NeedsAttention, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_REQ_VR1", notificationGeneralDetails.RequiresVR1, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_VR1_NO", notificationGeneralDetails.VR1Number, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_SO_NO", notificationGeneralDetails.SONumbers, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_WIP", notificationGeneralDetails.IsWorkInProgress, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("NOTIF_DATE", notificationGeneralDetails.NotifDate, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_NOTIF_NO", notificationGeneralDetails.NotificationNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_LENGTH", notificationGeneralDetails.VehicleLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_AXELWEIGHT", notificationGeneralDetails.AxelWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_MAXHEIGHT", notificationGeneralDetails.MaximamHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_REDHEIGHT", notificationGeneralDetails.ReducibleHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_GROSSWEIGHT", notificationGeneralDetails.GrossWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_RIGIDLEN", notificationGeneralDetails.RigidLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_WIDTH", notificationGeneralDetails.VehicleWidth, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("MOVE_START", notificationGeneralDetails.FromDateTime, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("MOVE_END", notificationGeneralDetails.ToDateTime, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_REV_ID", notificationGeneralDetails.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_L_DESC", notificationGeneralDetails.LoadDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_F_DESC", notificationGeneralDetails.FromSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_T_DESC", notificationGeneralDetails.ToSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_C_DESC", notificationGeneralDetails.ClientName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_HAUL_REF", notificationGeneralDetails.MyReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_VEH_CLASS", notificationGeneralDetails.VehicleCategory, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_VEH_FLEETNO", notificationGeneralDetails.FleetNos, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_VEH_REGISTR", notificationGeneralDetails.RegisterNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("P_ORDER_ESDAL_REF", notificationGeneralDetails.OrderingESDALReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
        //            parameter.AddWithValue("NO_OF_MOV", notificationGeneralDetails.NoOfMovements, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("NOTES_ON_ESCORT", notificationGeneralDetails.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("FolderID", notificationGeneralDetails.FolderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
        //        },
        //        (records, instance) =>
        //        {
        //            instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
        //            instance.ContentReferenceNo = records.GetStringOrDefault("PLANNED_CONTENT_REF_NO");
        //            instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
        //            instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
        //        }
        //        );
        //    return notificationGeneralDetails;
        //}
        #endregion

        #region CheckNotifValidation
        //public static NotificationGeneralDetails CheckNotifValidation(string contentReferenceNo)
        //{
        //    NotificationGeneralDetails objCheckValid = new NotificationGeneralDetails();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        objCheckValid,
        //       UserSchema.Portal + ".STP_NOTIFICATION.SP_CHECK_NOTIF_VALIDATION",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_CONTENTREF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //            (records, instance) =>
        //            {
        //                instance.CheckRoute = (int)records.GetDecimalOrDefault("ROUTE");
        //                instance.CheckVehicle = (int)records.GetDecimalOrDefault("VEHICLE");
        //                instance.CheckVehicleConfig = records.GetInt16OrDefault("VEHICLE_CONFIG");
        //                instance.CheckRouteConfig = records.GetInt16OrDefault("ROUTE_CONFIG");
        //            }
        //   );
        //    return objCheckValid;
        //}
        #endregion

        #region IsNotifSubmitCheck
        //public static int IsNotifSubmitCheck(long NotificationID)
        //{
        //    int result = 0;
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //              result,
        //                UserSchema.Portal + ".SP_CHECK_IS_NOTIF_SUBMIT",
        //              parameter =>
        //              {
        //                  parameter.AddWithValue("P_NOTIFICATIONID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //                  parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //              },
        //                  (records) =>
        //                  {
        //                      result = (int)records.GetDecimalOrDefault("V_FLAG");
        //                  }
        //          );
        //    return result;
        //}
        #endregion

        #region GetInboundNotification
        //public static byte[] GetInboundNotification(int notificationId)
        //{
        //    byte[] inbboundNotif = null;
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //            inbboundNotif,
        //           UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_GET_INBOUNDNOTIF",
        //            parameter =>
        //            {
        //                parameter.AddWithValue("IN_NOTIFICATION_ID", notificationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //                parameter.AddWithValue("IN_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
        //            },
        //            record =>
        //            {
        //                inbboundNotif = record.GetByteArrayOrNull("INBOUND_NOTIFICATION");
        //            }
        //        );
        //    return inbboundNotif;
        //}
        #endregion

        #region Update Inbound Notification
        //public static int UpdateInboundNotif(int notificationId, byte[] inboundNotif)
        //{
        //    int result = 0;
        //    SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
        //       UserSchema.Portal + ".STP_NOTIFICATION.SP_UPDATE_INBOUNDNOTIF",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_UPDATENOTIF_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_UPDATEINBOUND_NOTIF", inboundNotif, OracleDbType.Blob, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
        //        },
        //        record =>
        //        {
        //            result = record.GetInt32("P_AFFECTED_ROWS");
        //        }
        //    );
        //    return result;
        //}
        #endregion

        #region ListCloneAxelDetails
        //public static List<AxleDetails> ListCloneAxelDetails(int VehicleID)
        //{
        //    List<AxleDetails> objlistaxel = new List<AxleDetails>();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //           objlistaxel,
        //              UserSchema.Portal + ".STP_NOTIFICATION.SP_GET_NOTIF_CLONE_AXEL",
        //           parameter =>
        //           {
        //               parameter.AddWithValue("p_VHCL_ID ", VehicleID, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //               parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //           },
        //               (records, instance) =>
        //               {
        //                   instance.ComponentId = (int)records.GetLongOrDefault("COMPONENT_ID");
        //                   instance.AxleNo = records.GetInt16OrDefault("AXLE_NO");
        //                   instance.AxleWeight = (decimal)records.GetDoubleOrDefault("WEIGHT");
        //                   instance.AxleSpacing = records.GetDecimalOrDefault("V_NEXT_AXLE_DIST");
        //                   instance.NoOfWheels = records.GetInt16OrDefault("WHEEL_COUNT");
        //                   instance.ComponentType = records.GetDecimalOrDefault("COMP_TYPE");
        //               }
        //       );
        //    return objlistaxel;
        //}
        #endregion

        #region ListAxelDetails
        //public static List<AxleDetails> ListAxelDetails(int VehicleID)
        //{
        //    List<AxleDetails> objlistaxel = new List<AxleDetails>();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //           objlistaxel,
        //             UserSchema.Portal + ".STP_NOTIFICATION.SP_NOTIF_LIST_AXLE",
        //           parameter =>
        //           {
        //               parameter.AddWithValue("p_VHCL_ID ", VehicleID, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //               parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //           },
        //               (records, instance) =>
        //               {
        //                   instance.ComponentId = (int)records.GetLongOrDefault("COMPONENT_ID");
        //                   instance.AxleNo = records.GetInt16OrDefault("AXLE_NO");
        //                   instance.AxleWeight = (decimal)records.GetDoubleOrDefault("WEIGHT");
        //                   instance.AxleSpacing = records.GetDecimalOrDefault("V_NEXT_AXLE_DIST");
        //                   instance.NoOfWheels = records.GetInt16OrDefault("WHEEL_COUNT");
        //                   instance.ComponentType = records.GetDecimalOrDefault("COMP_TYPE");
        //               }
        //       );
        //    return objlistaxel;
        //}
        #endregion

        #region GetSimpleNotifVhclType
        //public static long GetSimpleNotifVhclType(int NotifId)
        //{
        //    long result = 0;
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //           result,
        //             UserSchema.Portal + ".STP_NOTIFICATION.SP_GET_VHCL_TYPE_FOR_NOTIF",
        //           parameter =>
        //           {
        //               parameter.AddWithValue("p_NOTIF_ID", NotifId, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //               parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //           },
        //               (records) =>
        //               {
        //                   result = records.GetInt32OrDefault("VEHICLE_TYPE");
        //               }
        //       );
        //    return result;
        //}
        #endregion

        #region GetGrossWeight
        //public static int GetGrossWeight(int notificationId)
        //{
        //    int result = 0;
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //           result,
        //              UserSchema.Portal + ".SP_GET_GROSS_WEIGHT",
        //           parameter =>
        //           {
        //               parameter.AddWithValue("P_NOTIFICATIONID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
        //               parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //           },
        //               (records) =>
        //               {
        //                   result = records.GetInt32OrDefault("V_GROSS_WEIGHT");
        //               }
        //       );
        //    return result;
        //}
        #endregion

        #endregion
    }
}