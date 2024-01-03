using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.General;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.DocumentsAndContents.Common;
using STP.DocumentsAndContents.Communication;
using STP.DocumentsAndContents.Document;
using STP.DocumentsAndContents.Providers;
//using STP.DocumentsAndContents.ServiceAccess;
using STP.Domain.DocumentsAndContents;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using static STP.DocumentsAndContents.Document.StringExtractor;

namespace STP.DocumentsAndContents.Persistance
{
    public static class DocumentTransmissionDAO
    {
        #region Get UserDetails for Notification
        public static UserInfo GetUserDetailsForNotification(string ESDALReference = null)
        {
            var info = new UserInfo();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   info,
                   UserSchema.Portal + ".SP_GET_NOTIF_USER_DET",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ESDAL_REF", ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       info.UserName = records.GetStringOrDefault("USERNAME");
                       info.IsAdmin = Convert.ToInt32(records.GetStringOrDefault("IS_ADMINISTRATOR"));
                       info.IsDeleted = Convert.ToInt32(records.GetStringOrDefault("DELETED"));
                   });
            return info;
        }
        #endregion

        #region Get UserDetails for Haulier
        public static UserInfo GetUserDetailsForHaulier(string mnemonic = null, string ESDALReference = null)
        {
            var info = new UserInfo();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   info,
                   UserSchema.Portal + ".SP_GET_HAUL_USER_DET",
                   parameter =>
                   {
                       parameter.AddWithValue("P_HAUL_MNEMONIC", mnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF", ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       info.UserName = records.GetStringOrDefault("USERNAME");
                       info.IsAdmin = Convert.ToInt32(records.GetStringOrDefault("IS_ADMINISTRATOR"));
                       info.IsDeleted = Convert.ToInt32(records.GetStringOrDefault("DELETED"));
                   });
            return info;
        }
        #endregion

        #region Get UserName
        public static UserInfo GetUserName(long organizationId = 0, long contactId = 0)
        {
            var info = new UserInfo();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   info,
                   UserSchema.Portal + ".SP_GET_SOA_POLICE_USERDET",
                   parameter =>
                   {
                       parameter.AddWithValue("P_OrgID", organizationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_Contact_ID", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       info.UserName = records.GetStringOrDefault("USERNAME");
                       info.IsAdmin = Convert.ToInt32(records.GetStringOrDefault("IS_ADMINISTRATOR"));
                       info.IsDeleted = Convert.ToInt32(records.GetStringOrDefault("DELETED"));
                   });
            return info;
        }
        #endregion

        #region Get SOA,Police Details
        public static DistributionAlerts GetSOAPoliceDetails(string ESDALReference = null, int transmissionId = 0)
        {
            var DistributionObj = new DistributionAlerts();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   DistributionObj,
                    UserSchema.Portal + ".SP_GET_SOA_POLICE_APPDET",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ESDAL_REF", ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_TRANS_ID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       DistributionObj.InboxId = Convert.ToInt32(records.GetLongOrDefault("INBOX_ITEM_ID"));
                       DistributionObj.NotificationId = Convert.ToInt32(records.GetLongOrDefault("NOTIFICATION_ID"));
                       DistributionObj.ItemType = records.GetStringOrDefault("TYPE");
                   });
            return DistributionObj;
        }
        #endregion

        #region Get Notification Details

        public static DistributionAlerts GetNotifDetails(string ESDALReference = null, int transmissionId = 0)
        {
            var DistributionObj = new DistributionAlerts();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   DistributionObj,
                   UserSchema.Portal + ".SP_GET_NOTIF_CHECKASHAULIER",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ESDAL_REF", ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_TRANS_ID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       instance.NotificationId = Convert.ToInt32(records.GetLongOrDefault("NOTIFICATION_ID"));
                       instance.ESDALReference = records.GetStringOrDefault("NOTIFICATION_CODE");
                       instance.VehicleType = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                       // instance.OutboundDocument = records.GetByteArrayOrNull("DOC");

                   });
            return DistributionObj;
        }
        #endregion

        #region Get Haulier Details
        public static DistributionAlerts GetHaulierDetails(string mnemonic = null, string ESDALReference = null, string versionNo = null)
        {
            var DistributionObj = new DistributionAlerts();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   DistributionObj,
                   UserSchema.Portal + ".SP_GET_HAUL_APP_DET",
                   parameter =>
                   {
                       parameter.AddWithValue("P_HAUL_MNEMONIC", mnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF", ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VERSION_NO", versionNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       DistributionObj.ProjectId = Convert.ToInt32(records.GetLongOrDefault("PROJECT_ID"));
                       DistributionObj.VersionId = Convert.ToInt32(records.GetLongOrDefault("VERSION_ID"));
                       DistributionObj.VersionNo = records.GetInt16OrDefault("VERSION_NO");
                       DistributionObj.RevisionId = Convert.ToInt32(records.GetLongOrDefault("REVISION_ID"));
                       DistributionObj.RevisionNo = records.GetInt16OrDefault("REVISION_NO");
                       DistributionObj.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                       DistributionObj.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                   });
            return DistributionObj;
        }
        #endregion

        #region  Get Agreed proposed and notification details XML for Haulier
        public static byte[] GetAgreedProposedNotificationXML(string docType, string ESDALReference, int notificationId)
        {
            byte[] docBytes = null;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                docBytes,
                 UserSchema.Portal + ".GET_AGREED_PROP_NOTI_XML",
                parameter =>
                {
                    parameter.AddWithValue("P_DOCTYPE", docType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDALREF", ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                   (records, instance) =>
                   {
                       docBytes = records.GetByteArrayOrNull("document_xml");
                   }
            );
            return docBytes;
        }
        #endregion

        #region Save DistributionStatus
        public static long SaveDistributionStatus(SaveDistributionStatusParams saveDistributionStatus)
        {
            NotificationContacts objContact = saveDistributionStatus.NotificationContacts;
            int status = saveDistributionStatus.Status;
            int inboxOnly = saveDistributionStatus.InboxOnly; 
            string EsdalReference = saveDistributionStatus.EsdalReference;
            long transmissionId = saveDistributionStatus.TransmissionId;
            bool IsImminent = saveDistributionStatus.IsImminent;
            string username = saveDistributionStatus.Username;
            //function saves some default value's into the
            try
            {
                if (objContact != null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Insert in DistributionStatus orgID: {0},contactId: {1} ", objContact.OrganisationId, objContact.ContactId));
                }
                #region 
                string email = null, fax = null; //variables are assigned null values.
                //the check is added for retransmitting an application with an updated mail / fax. such that the distribution status is updated with the values
                switch (status)
                {
                    case 8: // updating distribution status with latest email / fax in case of retransmitting condition's
                    case 12:
                        email = objContact.Email;
                        fax = objContact.Fax;
                        break;

                }
                #endregion

                long distrStatus = 0; // default set for "initiated"
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   distrStatus,
                    UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_DISTRIBUTION_STATUS_OP",
                   parameter =>
                   {
                       parameter.AddWithValue("P_STATUS", status, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // 0 for inserting
                       parameter.AddWithValue("P_NOTIF_ID", status != 0 ? 0 : objContact.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); // not required
                       parameter.AddWithValue("P_TRANS_ID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); //0 for insertion and gt& for updating
                       parameter.AddWithValue("P_ORG_ID", status != 0 ? 0 : objContact.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); //org id of Generated affected parties
                       parameter.AddWithValue("P_IS_MANUAL", status != 0 ? 0 : (objContact.ContactId == 0 ? 1 : 0), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_CONT_ID", status != 0 ? 0 : objContact.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_FULL_NAME", status != 0 ? status == 8 || status == 12 ? username : null : objContact.ContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ORG_NAME", status != 0 ? null : objContact.OrganistationName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_EMAIL", status != 0 ? email : objContact.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_FAX", status != 0 ? fax : objContact.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_INBOX_ONLY", status != 0 ? 0 : inboxOnly, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF", status != 0 ? null : EsdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   records =>
                   {
                       if (status == 0)
                           transmissionId = records.GetLongOrDefault("TRANSMISSION_ID");
                       else
                           distrStatus = (int)records.GetDecimalOrDefault("ROWCNT");
                   });

                if (status == 0)
                {
                    //only Generated affected parties are saved into active transactions table
                    if (objContact.ContactId != 0 && objContact.OrganisationId != 0 && !IsImminent)
                    {
                        //saving into active transactions table.
                        SaveActiveTransmission(objContact, EsdalReference, transmissionId, inboxOnly);
                    }
                    return transmissionId;
                }
                else
                {
                    return distrStatus;
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Insert in DistributionStatus exception : {0},orgID: {1} ", e.Message, objContact.OrganisationId));
                throw;
            }
        }
        #endregion

        #region Save ActiveTransmission
        public static long SaveActiveTransmission(NotificationContacts objContact, string EsdalReference, long transmissionId, int inboxOnly = 0)
        {
            //function saves some default value's into the
            int transType = 798003; // default set for "notification distribution"
            long tmpTransId = 0;
            int inboxStatus = 799002; // default set for "initiated"
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               tmpTransId,
                UserSchema.Portal + ".STP_ROUTE_ASSESSMENT.SP_INSERT_ACTIVE_TRANSC",
               parameter =>
               {
                   parameter.AddWithValue("P_TRANS_ID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_TRANS_TYPE", transType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ESDAL_REF", EsdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_EMAIL", objContact.Email, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_EMAIL_FOR", 247001, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_FAX", objContact.Fax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_INBOX_ONLY", inboxOnly, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ORG_ID", objContact.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_CONT_ID", objContact.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_ATTEMPT", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_INBOX_STATUS", inboxStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_INBOX_ATTEMPT", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_HAS_BEEN_QUE", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SaveActiveTransmission started successfully with parameters TransId : {0}, Transtype : {1}, EsdaRef : {2}, OrgId : {3}", transmissionId, transType, EsdalReference, objContact.OrganisationId));
               },
               records =>
               {
                   tmpTransId = records.GetLongOrDefault("TRANSMISSION_ID");
                   Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SaveActiveTransmission completed successfully with parameters TransId : {0}", tmpTransId));
               });
            return tmpTransId;
        }
        #endregion

        #region CopyMovementSortToPortal(MovementCopyDetails moveCopyDet)
        public static long CopyMovementSortToPortal(MovementCopyDetails moveCopyDet, int movementCloneStatus, int versionid = 0, string EsdalReference = "", byte[] hacontactbytes = null, int organizationid = 0, string userSchema = UserSchema.Portal)
        {

            int pstatus = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   pstatus,
                   userSchema + ".STP_MOVEMENT_TRANS_DISTR.SP_MOVMNT_SORT_PORTAL",
                   parameter =>
                   {

                       parameter.AddWithValue("P_PROJ_ID", moveCopyDet.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VER_NO", moveCopyDet.VersionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_HAUL_NEMONIC", moveCopyDet.HaulMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF_NO", moveCopyDet.ESDALRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VER_STATUS", moveCopyDet.MovementStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_USE_FLAG", movementCloneStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VERSION_ID", versionid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF", EsdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_HA_DOCUMENT", hacontactbytes, OracleDbType.Blob, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ORG_ID", organizationid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   records =>
                   {
                       pstatus = records.GetInt32OrDefault("PROJECT_STATUS");
                   });
            return pstatus;
        }
        #endregion

        #region public static List<TransmissionModel> GetTransmissionType(long TransId, string Status, int StatusItemCount, string userSchema)

        public static List<TransmissionModel> GetTransmissionType(long TransId, string Status, int StatusItemCount, string userSchema)
        {
            List<TransmissionModel> transmissionList = new List<TransmissionModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                transmissionList,
               userSchema + ".GET_DELIVERY_TYPE",
                parameter =>
                {
                    parameter.AddWithValue("P_TRANS_ID", TransId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STATUS", Status, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STATUS_ITEMS_COUNT", StatusItemCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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

        #region SortSideCheckDoctype(int transmissionId, UserInfo userInfo)
        public static TransmittingDocumentDetails SortSideCheckDoctype(int transmissionId, string userSchema = UserSchema.Portal)
        {
            TransmittingDocumentDetails transmittingDetail = new TransmittingDocumentDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   transmittingDetail,
                   userSchema + ".STP_MOVEMENT_TRANS_DISTR.SP_RESEND_TRANSMISSION_DATA",
                   parameter =>
                   {
                       parameter.AddWithValue("P_TRANS_ID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                        instance.DocumentId = records.GetLongOrDefault("DOCUMENT_ID");
                        instance.DocumentType = records.GetStringOrDefault("DOCUMENTTYPE");
                        instance.EsdalReference = records.GetStringOrDefault("ESDAL_REF");

                        instance.InboxItemId = records.GetFieldType("INBOX_ITEM_ID").Name == "Decimal" ? Convert.ToInt64(records.GetDecimalOrDefault("INBOX_ITEM_ID"))
                                    : records.GetLongOrDefault("INBOX_ITEM_ID");

                        instance.ContactId = records.GetFieldType("CONTACT_ID").Name == "Decimal" ? Convert.ToInt64(records.GetDecimalOrDefault("CONTACT_ID"))
                                    : records.GetLongOrDefault("CONTACT_ID");

                        instance.OrganisationId = records.GetFieldType("ORGANISATION_ID").Name == "Decimal" ? Convert.ToInt64(records.GetDecimalOrDefault("ORGANISATION_ID"))
                                    : records.GetLongOrDefault("ORGANISATION_ID");
                       if (records.GetFieldType("NOTIFICATION_ID") != null)
                       {
                           instance.NotificationId = records.GetFieldType("NOTIFICATION_ID").Name == "Decimal" ? Convert.ToInt64(records.GetDecimalOrDefault("NOTIFICATION_ID"))
                                       : records.GetLongOrDefault("NOTIFICATION_ID");
                       }

                        instance.ItemType = records.GetFieldType("ITEM_TYPE").Name == "Decimal" ? Convert.ToInt32(records.GetDecimalOrDefault("ITEM_TYPE"))
                                    : records.GetInt32OrDefault("ITEM_TYPE");
                   });
            return transmittingDetail;
        }
        #endregion

        #region Code Commetned By Mahzeer on 17/08/2023 SortSideRetransmitApplication(int transmissionId, RetransmitDetails retransmitDetails, string userSchema)
        /*public static int SortSideRetransmitApplication(int transmissionId, RetransmitDetails retransmitDetails, UserInfo userInfo)
        {
            string userSchema = "";
            if (userInfo != null)
            {
                userSchema = userInfo.UserSchema;
            }
            else
            {
                userSchema = UserSchema.Portal;
            }

            byte[] content = null;
            string xmlHtmlString = "", xsltPath = "", xmlString = "";
            TransmittingDocumentDetails transmittingDetail = new TransmittingDocumentDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   transmittingDetail,
                   userSchema + ".STP_MOVEMENT_TRANS_DISTR.SP_RESEND_TRANSMISSION_DATA",
                   parameter =>
                   {
                       parameter.AddWithValue("P_TRANS_ID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       //instance.OutboundDocument = records.GetByteArrayOrNull("DOCU");
                       instance.DocumentId = records.GetLongOrDefault("DOCUMENT_ID");
                       instance.DocumentType = records.GetStringOrDefault("DOCUMENTTYPE");
                       instance.EsdalReference = records.GetStringOrDefault("ESDAL_REF");

                       instance.InboxItemId = records.GetLongOrDefault("INBOX_ITEM_ID");
                       instance.ContactId = records.GetLongOrDefault("CONTACT_ID");
                       instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");

                   });
            if (userInfo.UserSchema == UserSchema.Portal && retransmitDetails.distFlag == true)
            {
                transmissionId = (int)getNewInsertedTransForDist(transmittingDetail, transmissionId);//RM#4966
            }
            NotificationContacts objContact = new NotificationContacts();

            string[] contactDet = null;

            if (transmittingDetail.ContactId != 0)
            {
                //function that returns contact's details in a string array
                contactDet = RouteAssessmentDAO.FetchContactPreference((int)transmittingDetail.ContactId, UserSchema.Portal);

                objContact.OrganisationId = Convert.ToInt32(contactDet[4]);
                objContact.ContactId = (int)transmittingDetail.ContactId;
            }

            xmlString = Encoding.UTF8.GetString(XsltTransformer.Trafo(transmittingDetail.OutboundDocument));

            XmlDocument Doc = new XmlDocument();

            try
            {
                Doc.LoadXml(xmlString);
            }
            catch
            {
                return 0;
            }
            XmlElement root = Doc.DocumentElement;

            string documentRootName = root.Name;

            XmlNodeList parentNode = Doc.GetElementsByTagName("AgreedRoute");


            string tagName = "", replyToEmail = "noreply@esdal2.com";
            bool isImminent = false;

            #region 
            //splitting up the esdal reference number to find notification  or renotification
            string esDAlRefNo = transmittingDetail.EsdalReference;
            esDAlRefNo = esDAlRefNo.Replace("~", "#");
            esDAlRefNo = esDAlRefNo.Replace("-", "/");
            esDAlRefNo = esDAlRefNo.Replace("#", "/");
            string[] esdalRefPro = esDAlRefNo.Split('/');
            string haulierMnemonic = string.Empty;
            string esdalrefnum = string.Empty;
            int versionNo = 0;

            if (esdalRefPro.Length > 0)
            {
                haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
            }
            #endregion

            switch (transmittingDetail.DocumentType)
            {
                case "proposal":
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderProposal.xslt";

                    tagName = "HAContact";
                    replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    break;
                case "agreement":
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";

                    tagName = "HAContact";
                    replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    break;
                case "notification":
                    try
                    {
                        if (contactDet[3] == "police")
                        {
                            if (versionNo > 1)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Notification_Fax_Police.xslt";

                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Fax_Police.xslt";
                            }
                        }
                        else
                        {
                            if (versionNo > 1)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReNotification.xslt";
                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_FAX_SOA_PDF.xsl";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (contactDet == null || contactDet.Length == 0)
                        {
                            if (versionNo > 1)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Notification_Fax_Police.xslt";

                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Fax_Police.xslt";
                            }
                        }
                    }

                    tagName = "HaulierDetails";
                    replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    break;
                case "daily digest":
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\DailyDigestFax.xslt";
                    break;
                case "route alert":
                    break;
                case "imminent move alert":
                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Alert_Fax.xslt";
                    isImminent = true;

                    tagName = "HaulierDetails";
                    replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    break;
                case "no longer affected":
                    try
                    {

                        if (contactDet[3] == "police")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalNoLongerFAXPolice.xslt";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                        }
                    }
                    catch (Exception ex)
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalNoLongerFAXPolice.xslt";
                    }
                    break;
                case "failed delegation alert":
                    break;
                case "movement details":
                    if (documentRootName == "AgreedRoute")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";
                    }
                    else if (documentRootName == "Proposal")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderProposal.xslt";
                    }

                    tagName = "HAContact";
                    replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    break;
                case "special order":
                    if (documentRootName == "AgreedRoute")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";

                        tagName = "HAContact";
                        replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    }
                    else if (documentRootName == "Proposal")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderProposal.xslt";

                        tagName = "HAContact";
                        replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    }
                    else if (documentRootName == "OutboundNotification") //special order notification
                    {
                        try
                        {
                            if (contactDet[3] == "police")
                            {
                                if (versionNo > 1)
                                {
                                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Notification_Fax_Police.xslt";

                                }
                                else
                                {
                                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Fax_Police.xslt";
                                }
                            }
                            else
                            {
                                if (versionNo > 1)
                                {
                                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReNotification.xslt";
                                }
                                else
                                {
                                    xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_FAX_SOA_PDF.xsl";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (versionNo > 1)
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Notification_Fax_Police.xslt";

                            }
                            else
                            {
                                xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Notification_Fax_Police.xslt";
                            }
                        }
                        tagName = "HaulierDetails";
                        replyToEmail = RetransmitDocumentDomain.GetHaHaulEmailAddressForRetransmit(Doc, tagName);

                    }
                    break;
                case "vr1 planned route":
                    break;
            }

            int transmissionStatus = 0;
            if (xsltPath != "")
            {
                try
                {
                    if (retransmitDetails.efax == "fax")
                    {
                        objContact.Fax = retransmitDetails.Fax; 
                        xmlString = Encoding.UTF8.GetString(XsltTransformer.Trafo(transmittingDetail.OutboundDocument));
                        xmlString = CommonMethods.AttachAffecetedStructure(xmlString, esDAlRefNo, userInfo, userSchema, (int)objContact.OrganisationId);
                        xmlHtmlString = CommonMethods.RetransmissionDocument(xmlString, objContact.ContactId, xsltPath, "PDF", userInfo);
                        content = MessageTransmiter.GenerateSOPDF(xmlHtmlString);
                    }
                    else if (retransmitDetails.efax == "email")
                    {
                        byte[] attachment = new byte[0];

                        objContact.Email = retransmitDetails.Email;  
                        xmlString = Encoding.UTF8.GetString(XsltTransformer.Trafo(transmittingDetail.OutboundDocument));
                        xmlString = CommonMethods.AttachAffecetedStructure(xmlString, esDAlRefNo, userInfo, userSchema, (int)objContact.OrganisationId);
                        xmlHtmlString = CommonMethods.RetransmissionDocument(xmlString, objContact.ContactId, xsltPath, "EMAIL", userInfo);
                        content = Encoding.UTF8.GetBytes(xmlHtmlString);
                        /// to send xml attchment in mail to SOA users if xmlattached user preference is set.
                        int varXmlAttached = 0;
                        if (contactDet != null)// added condition to satify redmine issue #5410
                        {
                            if (contactDet[3] == "soa")
                            {
                                if (contactDet[5] == "1")
                                {
                                    attachment = XsltTransformer.Trafo(transmittingDetail.OutboundDocument);
                                    varXmlAttached = 1; //send 1 for xmlattach parameter to attach xml in mail.
                                }
                            }
                        }
                    }
                    
                    if (transmissionStatus == 0)
                        SaveDistributionStatus(objContact, 8, 0, transmittingDetail.EsdalReference, transmissionId, false, userInfo.UserName);
                    else
                        SaveDistributionStatus(objContact, 12, 0, transmittingDetail.EsdalReference, transmissionId, false, userInfo.UserName);
                }
                catch
                {
                    //do nothing
                }
            }
            return transmissionStatus;
        }*/
        #endregion

        #region getNewInsertedTransForDist
        //For distribution status view
        public static long getNewInsertedTransForDist(TransmittingDocumentDetails transDetails, int transmissionId)
        {
            try
            {
                long newtransID = DocumentTransmissionDAO.InsertNewTransmissionForDistribution(transmissionId);
                if (newtransID != 0)
                {
                    transDetails.DocumentId = newtransID;
                }
                else
                {
                    newtransID = transmissionId;
                }
                return newtransID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertNewTransmissionForDistribution(int transmissionId)
        public static long InsertNewTransmissionForDistribution(int transmissionId)
        {
            long newTransmissionID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   newTransmissionID,
                   UserSchema.Portal + ".SP_CLONE_TRANSM_DIST",
                   parameter =>
                   {
                       parameter.AddWithValue("P_DOCID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   records =>
                   {
                       newTransmissionID = records.GetLongOrDefault("NEWTRANID");
                   });
            return newTransmissionID;
        }
        #endregion

        #region RetransmitSortSideApplications(long transmissionId,string userSchema)
        public static RetransmitDetails GetRetransmitDetails(long transmissionId, string userSchema)
        {
            string[] splitSeperator = { "##**##" };

            RetransmitDetails retransmitDetails = new RetransmitDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   retransmitDetails,
                   userSchema + ".STP_MOVEMENT_TRANS_DISTR.SP_FETCH_TRANSMISSION_DATA",
                   parameter =>
                   {
                       parameter.AddWithValue("P_TRANS_ID", transmissionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, instance) =>
                   {
                       if (records.GetStringOrDefault("EMAIL").IndexOf("##**##") != -1)
                       {
                           string[] email = records.GetStringOrDefault("EMAIL").Split(splitSeperator, StringSplitOptions.None);

                           if (email.Length == 2)
                           {
                               instance.Email = email[1];
                           }
                           else
                           {
                               instance.Email = email[0];
                           }
                       }
                       else
                       {
                           instance.Email = records.GetStringOrDefault("EMAIL");
                       }

                       instance.Fax = records.GetStringOrDefault("FAX");

                       if (records.GetStringOrDefault("FULL_NAME").IndexOf("##**##") != -1)
                       {
                           string[] fullName = records.GetStringOrDefault("FULL_NAME").Split(splitSeperator, StringSplitOptions.None);

                           if (fullName.Length == 2)
                           {
                               instance.ContactName = fullName[1];
                           }
                           else
                           {
                               instance.ContactName = fullName[0];
                           }
                       }
                       else
                       {
                           instance.ContactName = records.GetStringOrDefault("FULL_NAME");
                       }

                       if (records.GetStringOrDefault("ORG_NAME").IndexOf("##**##") != -1)
                       {
                           string[] orgName = records.GetStringOrDefault("ORG_NAME").Split(splitSeperator, StringSplitOptions.None);

                           if (orgName.Length == 2)
                           {
                               instance.ContactName = orgName[1];
                           }
                           else
                           {
                               instance.ContactName = orgName[0];
                           }
                       }
                       else
                       {
                           instance.OrganisationName = records.GetStringOrDefault("ORG_NAME");
                       }

                       instance.efax = instance.Email == "NA" ? "fax" : "email";
                       instance.ContactMethod = instance.Email == "NA" ? "Fax" : "Email";
                       if (instance.Fax == "0")
                       {
                           instance.Fax = null;
                       }
                       else if (instance.Email == "NA")
                       {
                           instance.Email = null;
                       }
                   });

            return retransmitDetails;
        }
        #endregion

        #region GetDocument
        public static XMLModel GetDocument(SOProposalDocumentParams documentParams)
        {
            GenerateXML gxml = new GenerateXML();
            XMLModel model = new XMLModel();
            switch (documentParams.ItemTypeStatus)
            {
                /*agreed                    agreement    - Done                  vrl planned route - Done -need to check
                amendment to agreement                    recleared                    proposal  - Done                  reproposal
                nolonger affected                    notification - Done                    renotification - Done
                ne agreed notification                    ne notification  - Done                  ne renotification- Done
                ne agreed renotification*/
                case (int)ItemTypeStatus.proposal://proposed
                    model = gxml.GenerateProposalXML(documentParams.EsdalReferenceNo, documentParams.OrganisationId, documentParams.ContactId, documentParams.UserSchema);
                    break;

                case (int)ItemTypeStatus.reproposal://re-proposed
                                                    //version
                                                    //versionNo - 1
                    model = gxml.GenerateReProposalStillAffectedFAXSOAXML((int)documentParams.Moveprint.ProjectId, documentParams.UserType, documentParams.ContactId, documentParams.VersionNo,
                          documentParams.UserSchema);
                    break;

                case (int)ItemTypeStatus.nolonger_affected://nolonger_affected
                    model = gxml.GenerateReProposalNoLongerAffectedFAXSOAXML((int)documentParams.Moveprint.ProjectId, documentParams.UserType, documentParams.ContactId, documentParams.VersionNo,
                          documentParams.UserSchema);
                    break;

                case (int)ItemTypeStatus.agreement://agreed
                                                   //SOA
                                                   //Police
                    model = gxml.GenerateAgreedRoutetXML(documentParams.UserType, documentParams.EsdalReferenceNo, documentParams.Moveprint.OrderNumber,
                         documentParams.ContactId, documentParams.UserSchema);
                    break;

                case (int)ItemTypeStatus.vr1_planned_route://vr1_planned_route
                    model = gxml.GenerateAgreedRoutetXML(documentParams.UserType, documentParams.EsdalReferenceNo, documentParams.Moveprint.OrderNumber,
                         documentParams.ContactId, documentParams.UserSchema);
                    break;

                case (int)ItemTypeStatus.amendment_to_agreement://agreed revised
                                                                //SOA
                                                                //Police
                                                                //GetRevisedAgreementDetails(orderNo
                                                                //GetRevisedAgreementDetails(""
                    model = gxml.GenerateRevisedAgreementXML(documentParams.UserType, documentParams.EsdalReferenceNo, documentParams.Moveprint.OrderNumber, documentParams.ContactId, documentParams.UserSchema, documentParams.DistributionComments);
                    break;

                case (int)ItemTypeStatus.recleared://agreed recleared
                                                   //SOA
                                                   //Police
                    model = gxml.GenerateAgreedRoutetXML(documentParams.UserType, documentParams.EsdalReferenceNo, documentParams.Moveprint.OrderNumber, documentParams.ContactId, documentParams.UserSchema, documentParams.DistributionComments);
                    break;

                case (int)ItemTypeStatus.notification://notification
                    NotificationXSD.OutboundNotificationStructure obns = DocumentConsole.Instance.GetOutboundNotificationStructureData(documentParams.NotificationId, false, documentParams.ContactId);
                    model = gxml.GenerateNotificationXML(documentParams.NotificationId, obns, documentParams.NotificationType);
                    break;
                case (int)ItemTypeStatus.ne_notification://ne notification
                    NotificationXSD.OutboundNotificationStructure obnsne = DocumentConsole.Instance.GetOutboundNotificationStructureData(documentParams.NotificationId, false, documentParams.ContactId,documentParams.OrganisationId,documentParams.IsNen);
                    model = gxml.GenerateNotificationXML(documentParams.NotificationId, obnsne, documentParams.NotificationType);
                    break;

                case (int)ItemTypeStatus.renotification://renotification
                                                        //NotificationId-1
                    NotificationXSD.OutboundNotificationStructure obnsRe = DocumentConsole.Instance.GetOutboundReNotificationStructureData(Enums.PortalType.SOA, documentParams.NotificationId, false, documentParams.ContactId);
                    model = gxml.GenerateNotificationXML(documentParams.NotificationId, obnsRe, documentParams.NotificationType);
                    break;

                case (int)ItemTypeStatus.ne_renotification://ne renotification
                                                           //NotificationId-1
                    NotificationXSD.OutboundNotificationStructure obnsNeRe = DocumentConsole.Instance.GetOutboundReNotificationStructureData(Enums.PortalType.SOA, documentParams.NotificationId, false, documentParams.ContactId);
                    model = gxml.GenerateNotificationXML(documentParams.NotificationId, obnsNeRe, documentParams.NotificationType);
                    break;
            }
            return model;
        }
        #endregion

        #region GetRetransmitDocument
        public static RetransmitEmailgetParams GetRetransmitDocument(TransmittingDocumentDetails transmittingDetail, RetransmitDetails retransmitDetails, int transmissionId, UserInfo userInfo, string userSchema)
        {
            string xmlHtmlString, xsltPath = string.Empty, xmlString;
            string[] contactDet = new string[6];
            byte[] content = null;
            byte[] attachment = new byte[0];
            int varXmlAttached = 0;
            NotificationContacts objContact = new NotificationContacts();
            if (transmittingDetail.ContactId != 0)
            {
                //function that returns contact's details in a string array
                contactDet = RouteAssessmentDAO.FetchContactPreference((int)transmittingDetail.ContactId, UserSchema.Portal);
                objContact.OrganisationId = Convert.ToInt32(contactDet[4]);
                objContact.ContactId = (int)transmittingDetail.ContactId;
            }
            else
            {
                contactDet[0] = (!string.IsNullOrWhiteSpace(retransmitDetails.Email)) ? "Email" : "Online Inbox Only";
                contactDet[1] = retransmitDetails.Email;
                contactDet[2] = retransmitDetails.Fax;
                contactDet[3] = "soa";
                contactDet[5] = "0";
            }
            #region-------splitting up the esdal reference number to find notification  or renotification
            string esDAlRefNo = transmittingDetail.EsdalReference;
            esDAlRefNo = esDAlRefNo.Replace("~", "#");
            esDAlRefNo = esDAlRefNo.Replace("-", "/");
            esDAlRefNo = esDAlRefNo.Replace("#", "/");
            string[] esdalRefPro = esDAlRefNo.Split('/');
            int versionNo = 0;

            if (esdalRefPro.Length > 0)
                versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
            #endregion

            var itemTypeStat = transmittingDetail.ItemType;
            var NotificationType = userInfo.UserTypeId == UserType.PoliceALO ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
            MovementPrint moveprint = GetProjectIdByEsdalReferenceNo(esDAlRefNo);
            var UserTypeE = userInfo.UserTypeId == UserType.PoliceALO ? Enums.PortalType.POLICE : Enums.PortalType.SOA;

            var documentParams = new SOProposalDocumentParams()
            {
                ContactId = Convert.ToInt32(transmittingDetail.ContactId),
                NotificationId = transmittingDetail.NotificationId != null ? (int)transmittingDetail.NotificationId : 0,
                EsdalReferenceNo = transmittingDetail.EsdalReference,
                OrganisationId = Convert.ToInt32(transmittingDetail.OrganisationId),
                ItemTypeStatus = itemTypeStat,
                NotificationType = NotificationType,
                UserSchema = userSchema,
                UserType = UserTypeE,
                Moveprint = moveprint,
                VersionNo = versionNo
            };
            var output = GetDocument(documentParams);
            xmlString = output.ReturnXML;
            XmlDocument Doc = new XmlDocument();

            try
            {
                Doc.LoadXml(xmlString);
            }
            catch
            {
                return null;
            }
            XmlElement root = Doc.DocumentElement;

            string documentRootName = root.Name;

            bool isImminent = false;
            switch (transmittingDetail.DocumentType)
            {
                case "proposal":
                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderProposal.xslt";
                    break;
                case "agreement":
                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderAgreedRoute.xslt";
                    break;
                case "notification":
                    try
                    {
                        if (contactDet[3] == "police")
                        {
                            if (versionNo > 1)
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Notification_Fax_Police.xslt";
                            else
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Fax_Police.xslt";
                        }
                        else
                        {
                            if (versionNo > 1)
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReNotification.xslt";
                            else
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_FAX_SOA_PDF.xsl";
                        }
                    }
                    catch (Exception ex)
                    {
                        if (contactDet == null || contactDet.Length == 0)
                        {
                            if (versionNo > 1)
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Notification_Fax_Police.xslt";
                            else
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Fax_Police.xslt";
                        }
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"GetRetransmitDocument, Exception: {ex}");
                    }
                    break;
                case "daily digest":
                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\DailyDigestFax.xslt";
                    break;
                case "route alert":
                    break;
                case "imminent move alert":
                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Alert_Fax.xslt";
                    isImminent = true;
                    break;
                case "no longer affected":
                    try
                    {
                        if (contactDet[3] == "police")
                            xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalNoLongerFAXPolice.xslt";
                        else
                            xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                    }
                    catch (Exception ex)
                    {
                        xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\ReProposalNoLongerFAXPolice.xslt";
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"GetRetransmitDocument, Exception: {ex}");
                    }
                    break;
                case "failed delegation alert":
                    break;
                case "movement details":
                    if (documentRootName == "AgreedRoute")
                        xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderAgreedRoute.xslt";
                    else if (documentRootName == "Proposal")
                        xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderProposal.xslt";
                    break;
                case "special order":
                    if (documentRootName == "AgreedRoute")
                        xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderAgreedRoute.xslt";
                    else if (documentRootName == "Proposal")
                        xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\SpecialOrderProposal.xslt";
                    else if (documentRootName == "OutboundNotification") //special order notification
                    {
                        try
                        {
                            if (contactDet[3] == "police")
                            {
                                if (versionNo > 1)
                                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Notification_Fax_Police.xslt";
                                else
                                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Fax_Police.xslt";
                            }
                            else
                            {
                                if (versionNo > 1)
                                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReNotification.xslt";
                                else
                                    xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_FAX_SOA_PDF.xsl";
                            }
                        }
                        catch (Exception ex)
                        {
                            if (versionNo > 1)
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Re_Notification_Fax_Police.xslt";
                            else
                                xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Fax_Police.xslt";
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"GetRetransmitDocument, Exception: {ex}");
                        }
                    }
                    break;
                case "vr1 planned route":
                    break;
            }
            if (!string.IsNullOrWhiteSpace(xsltPath))
            {
                try
                {
                    if (retransmitDetails.efax == "email" || !string.IsNullOrEmpty(retransmitDetails.Email))
                    {
                        objContact.Email = retransmitDetails.Email;
                        xmlString = CommonMethods.AttachAffecetedStructure(xmlString, esDAlRefNo, userInfo, userSchema, (int)objContact.OrganisationId);
                        xmlHtmlString = CommonMethods.RetransmissionDocument(xmlString, objContact.ContactId, xsltPath, "EMAIL", userInfo);
                        content = Encoding.UTF8.GetBytes(xmlHtmlString);
                        /// to send xml attchment in mail to SOA users if xmlattached user preference is set.

                        if (contactDet != null && contactDet[3] == "soa" && contactDet[5] == "1")// added condition to satify redmine issue #5410
                        {
                            attachment = XsltTransformer.Trafo(content);
                            varXmlAttached = 1;
                        }
                    }
                    else if (retransmitDetails.efax == "fax")
                    {
                        objContact.Fax = retransmitDetails.Fax;
                        xmlString = Encoding.UTF8.GetString(XsltTransformer.Trafo(transmittingDetail.OutboundDocument));
                        xmlString = CommonMethods.AttachAffecetedStructure(xmlString, esDAlRefNo, userInfo, userSchema, (int)objContact.OrganisationId);
                        xmlHtmlString = CommonMethods.RetransmissionDocument(xmlString, objContact.ContactId, xsltPath, "PDF", userInfo);
                        content = MessageTransmiter.GenerateSOPDF(xmlHtmlString);
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"GetRetransmitDocument, Exception: {ex}");
                    throw;
                }
            }
            RetransmitEmailgetParams retransmit = new RetransmitEmailgetParams
            {
                Content = content,
                AttachmentData = attachment,
                XmlAttached = varXmlAttached,
                IsImminent = isImminent
            };
            return retransmit;
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
    }
}