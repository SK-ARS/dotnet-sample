using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using STP.Domain;
using STP.Domain.SecurityAndUsers;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;

using NetSdoGeometry;
using STP.Domain.VehiclesAndFleets.Configuration;
using Newtonsoft.Json;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class MovementsDAO
    {
        const string m_RouteName = "Movements";
        public static List<MovementsInbox> GetMovementInbox(GetInboxMovementsParams inboxMovementsParams)
        {
            //for passing values as a string to the db.eg:"a,s,d,f,g"
            string itemStatus = null; 
            string[] itemStatusArray = null;
            int itemStatusCount = 0;

            string vehicleType = null;
            int vehicleTypeOperator = 1;
            string[] vehicleTypeArray = null;
            int vehicleTypeCount = 0;

            string assignedUsers = null;
            string[] assignedUsersArray = null;
            int assignedUsersCount = 0;

            string ICAStatusFlag = null;
            string[] ICAStatusArray = null;
            int ICAStatusCount = 0;

            int? requiresVR1Flag = null;
            int? mostRecent = null;
            int IsImminent = 0;
            int IsImminentOpr = 0;
            int UnderAssessmentFlag = 0;
            int UnderAssessmentOpr = 0;
            int includeHistoricalData = 0;
            
            //Creating new object for GetMovementList
            List<MovementsInbox> movementInboxObj = new List<MovementsInbox>();

            var MovementFromDate = inboxMovementsParams.InboxFilter.MovementFromDate;
            var MovementToDate = inboxMovementsParams.InboxFilter.MovementToDate;
            if (!string.IsNullOrEmpty(MovementFromDate) && !string.IsNullOrEmpty(MovementToDate))
            {
                inboxMovementsParams.InboxFilter.MovementFrom = Convert.ToDateTime(MovementFromDate);
                inboxMovementsParams.InboxFilter.MovementTo = Convert.ToDateTime(MovementToDate);
            }
            if (!string.IsNullOrEmpty(MovementFromDate) && string.IsNullOrEmpty(MovementToDate))
            {
                inboxMovementsParams.InboxFilter.MovementFrom = Convert.ToDateTime(MovementFromDate);
                inboxMovementsParams.InboxFilter.MovementTo = DateTime.Now;
            }
            if (string.IsNullOrEmpty(MovementFromDate) && !string.IsNullOrEmpty(MovementToDate))
            {
                inboxMovementsParams.InboxFilter.MovementFrom = DateTime.Now;
                inboxMovementsParams.InboxFilter.MovementTo = Convert.ToDateTime(MovementToDate);
            }

            var FromReceiptDateOfCommn = inboxMovementsParams.InboxFilter.FromReceiptDateOfCommn;
            var ToReceiptDateOfCommn = inboxMovementsParams.InboxFilter.ToReceiptDateOfCommn;
            if (!string.IsNullOrEmpty(FromReceiptDateOfCommn) && !string.IsNullOrEmpty(ToReceiptDateOfCommn))
            {
                inboxMovementsParams.InboxFilter.FromReceipt = Convert.ToDateTime(FromReceiptDateOfCommn);
                inboxMovementsParams.InboxFilter.ToReceipt = Convert.ToDateTime(ToReceiptDateOfCommn);
            }
            if (!string.IsNullOrEmpty(FromReceiptDateOfCommn) && string.IsNullOrEmpty(ToReceiptDateOfCommn))
            {
                inboxMovementsParams.InboxFilter.FromReceipt = Convert.ToDateTime(FromReceiptDateOfCommn);
                inboxMovementsParams.InboxFilter.ToReceipt = DateTime.Now;
            }
            if (string.IsNullOrEmpty(FromReceiptDateOfCommn) && !string.IsNullOrEmpty(ToReceiptDateOfCommn))
            {
                inboxMovementsParams.InboxFilter.FromReceipt = DateTime.Now;
                inboxMovementsParams.InboxFilter.ToReceipt = Convert.ToDateTime(ToReceiptDateOfCommn);
            }

            if (!string.IsNullOrEmpty(inboxMovementsParams.InboxFilter.ESDALReference))
                inboxMovementsParams.InboxFilter.ESDALReference = inboxMovementsParams.InboxFilter.ESDALReference.Trim();
            if (!string.IsNullOrEmpty(inboxMovementsParams.InboxFilter.HaulierName))
                inboxMovementsParams.InboxFilter.HaulierName = inboxMovementsParams.InboxFilter.HaulierName.Trim();
            if (!string.IsNullOrEmpty(inboxMovementsParams.InboxFilter.HaulierReference))
                inboxMovementsParams.InboxFilter.HaulierReference = inboxMovementsParams.InboxFilter.HaulierReference.Trim();
            if (!string.IsNullOrEmpty(inboxMovementsParams.InboxAdvancedFilter.StructureReferenceNo))
                inboxMovementsParams.InboxAdvancedFilter.StructureReferenceNo = inboxMovementsParams.InboxAdvancedFilter.StructureReferenceNo.Trim();
           
            if (inboxMovementsParams.InboxFilter.IncludeHistorical)
                includeHistoricalData = 1;
            if (inboxMovementsParams.InboxFilter.ShowMostRecentVersion)
                mostRecent = 1;

            #region pass item status as a string of values to the DAO
            if (inboxMovementsParams.InboxFilter.Withdrawn)
                itemStatus = string.Concat(itemStatus, "313002,");
            if (inboxMovementsParams.InboxFilter.Declined)
                itemStatus = string.Concat(itemStatus, "313003,");
            if (inboxMovementsParams.InboxFilter.Accepted)
                itemStatus = string.Concat(itemStatus, "313005,");
            if (inboxMovementsParams.InboxFilter.Rejected)
                itemStatus = string.Concat(itemStatus, "313006,");
            if (inboxMovementsParams.InboxFilter.Opened)
                itemStatus = string.Concat(itemStatus, "313007,");
            if (inboxMovementsParams.InboxFilter.Unopened)
                itemStatus = string.Concat(itemStatus, "313009,");

            if (!string.IsNullOrEmpty(itemStatus))
            {
                itemStatus = itemStatus.TrimEnd(',');//To remove "," at the end of string
                itemStatusArray = itemStatus.Split(',');
                itemStatusCount = itemStatusArray.Length;
            }
            #endregion

            #region pass Imminent Flag
            if (inboxMovementsParams.InboxFilter.ImminentMovement)
            {
                IsImminent = 1;
                if (!string.IsNullOrEmpty(itemStatus))//OR operator
                    IsImminentOpr = 2;
                else //AND Operator
                    IsImminentOpr = 1;
            }
            #endregion

            #region pass Under Assessment flag
            if (inboxMovementsParams.InboxFilter.UnderAssessmentbyMe)
            {
                UnderAssessmentFlag = 1;
                assignedUsers = string.Concat(assignedUsers, Convert.ToString(inboxMovementsParams.UserId) + ",");
            }

            if (inboxMovementsParams.InboxFilter.UnderAssessmentbyOtherUser)
            {
                UnderAssessmentFlag = 1;
                assignedUsers = string.Concat(assignedUsers, Convert.ToString(inboxMovementsParams.InboxFilter.UserID) + ",");
            }

            if (inboxMovementsParams.InboxFilter.UnderAssessment)
                UnderAssessmentFlag = 2;

            if (UnderAssessmentFlag > 0)
            {
                if (!string.IsNullOrEmpty(itemStatus) || IsImminentOpr > 0)//OR operator
                    UnderAssessmentOpr = 2;
                else //AND Operator
                    UnderAssessmentOpr = 1;
            }
            if (!string.IsNullOrEmpty(assignedUsers))
            {
                assignedUsers = assignedUsers.TrimEnd(',');//To remove "," at the end of string
                assignedUsersArray = assignedUsers.Split(',');
                assignedUsersCount = assignedUsersArray.Length;
            }
            #endregion

            #region pass ICA Status as a string of values to the DAO
            if (inboxMovementsParams.InboxFilter.IncludeFailedDelivery)
                ICAStatusFlag = string.Concat(ICAStatusFlag, "277005,");
            if (inboxMovementsParams.InboxAdvancedFilter.Suitable)
                ICAStatusFlag = string.Concat(ICAStatusFlag, "277002,");
            if (inboxMovementsParams.InboxAdvancedFilter.MarginallySuitable)
                ICAStatusFlag = string.Concat(ICAStatusFlag, "277003,");
            if (inboxMovementsParams.InboxAdvancedFilter.UnSuitable)
                ICAStatusFlag = string.Concat(ICAStatusFlag, "277004,");
            if (!string.IsNullOrEmpty(ICAStatusFlag))
            {
                ICAStatusFlag = ICAStatusFlag.TrimEnd(',');//To remove "," at the end of string
                ICAStatusArray = ICAStatusFlag.Split(',');
                ICAStatusCount = ICAStatusArray.Length;
            }
            #endregion

            #region pass Vehicle Type as a string of values to the DAO
            if (inboxMovementsParams.InboxFilter.SO)
                vehicleType = string.Concat(vehicleType, "241002,");
            if (inboxMovementsParams.InboxFilter.VSO)
                vehicleType = string.Concat(vehicleType, "241001,");
            if (inboxMovementsParams.InboxFilter.STGO)
                vehicleType = string.Concat(vehicleType, "241003,241004,241005,241006,241007,241008,241009,241010,241012,241013,241014,");
            if (inboxMovementsParams.InboxFilter.CandU)
                vehicleType = string.Concat(vehicleType, "241011,");
            if (inboxMovementsParams.InboxFilter.Tracked)
                vehicleType = string.Concat(vehicleType, "241012,");
            if (inboxMovementsParams.InboxFilter.STGOVR1)
            {
                if (string.IsNullOrEmpty(vehicleType))
                {
                    //vehicleType = string.Concat(vehicleType, "241003,241004,241005,"); not required to pass, handled in SP
                    vehicleTypeOperator = 2;
                }
                else
                {
                    vehicleTypeOperator = 1;
                }
                
                requiresVR1Flag = 1;
            }

            if (!string.IsNullOrEmpty(vehicleType))
            {
                vehicleType = vehicleType.TrimEnd(',');//To remove "," at the end of string
                vehicleTypeArray = vehicleType.Split(',');
                vehicleTypeCount = vehicleTypeArray.Length;
            }
            #endregion

            //Setup Procedure LIST_MOVEMENT_INBOX
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementInboxObj,
                inboxMovementsParams.UserSchema + ".SP_GET_MOVEMENT_INBOX",
                parameter =>
                {
                    parameter.AddWithValue("ORG_ID", inboxMovementsParams.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("STRUCT_ID", inboxMovementsParams.InboxAdvancedFilter.StructureId == 0 ? null : inboxMovementsParams.InboxAdvancedFilter.StructureId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("STRUCT_REF_NO", inboxMovementsParams.InboxAdvancedFilter.StructureReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pagenumber", inboxMovementsParams.PageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pagesize", inboxMovementsParams.PageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ITEM_STAT", itemStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("STATUS_COUNT", itemStatusCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IMMINENT_FLAG", IsImminent, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IMMINENT_OPR", IsImminentOpr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_UNDER_ASSESSMENT", UnderAssessmentFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_UNDER_ASSESS_OPR", UnderAssessmentOpr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ASSIGNED_USERS", assignedUsers, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ASSIGN_USERS_CNT", assignedUsersCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ICA_STAT", ICAStatusFlag, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ICA_STAT_COUNT", ICAStatusCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_TYPE", vehicleType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("TYPE_COUNT", vehicleTypeCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOVE_TYPE_OPERATOR", vehicleTypeOperator, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("REQUIRES_VR1_FLAG", requiresVR1Flag, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ESDAL_REF", inboxMovementsParams.InboxFilter.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_NAME", inboxMovementsParams.InboxFilter.HaulierName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_REF", inboxMovementsParams.InboxFilter.HaulierReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ST_DATE", inboxMovementsParams.InboxFilter.MovementFrom, OracleDbType.Date, ParameterDirectionWrap.Input, 32737);
                    parameter.AddWithValue("EN_DATE", inboxMovementsParams.InboxFilter.MovementTo, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("REC_DATE_1", inboxMovementsParams.InboxFilter.FromReceipt, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("REC_DATE_2", inboxMovementsParams.InboxFilter.ToReceipt, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOST_REC", mostRecent, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("DELEG_ARRANG", inboxMovementsParams.InboxAdvancedFilter.ObjectDelegationList, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("sort_order", inboxMovementsParams.InboxAdvancedFilter.SortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", inboxMovementsParams.presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("START_POINT", inboxMovementsParams.InboxAdvancedFilter.StartPoint, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("END_POINT", inboxMovementsParams.InboxAdvancedFilter.EndPoint, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_isnen", inboxMovementsParams.InboxFilter.IsNen, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//Nen Filteration.
                    parameter.AddWithValue("P_HISTORIC", includeHistoricalData, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_query_string", inboxMovementsParams.InboxAdvancedFilter.QueryString, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FOLDER_ID", inboxMovementsParams.InboxAdvancedFilter.FolderId , OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_logic_opr", inboxMovementsParams.InboxAdvancedFilter.LogicOpr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("presult_set", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     // Sort Movements
                    if (inboxMovementsParams.UserSchema == UserSchema.Sort)
                    {
                        instance.VehicleClassification = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        instance.TotalRecord = (int)records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                        instance.Type = records.GetStringOrDefault("TYPE");
                        instance.ESDALReference = records.GetStringOrDefault("ESDAL_REF");
                        instance.ProjectStatus = records.GetInt32OrDefault("PROJECT_STATUS");
                        instance.ApplicationDate = instance.ApplicationDate = records.GetStringOrEmpty("APPLICATION_DATE");
                        instance.WorkStatus = records.GetInt32OrDefault("CHECKING_STATUS");
                        instance.DueDate = records.GetStringOrEmpty("DUE_DATE");
                        instance.MoveFrom = records.GetStringOrDefault("APP_FROM_DESCR");
                        instance.MoveTo = records.GetStringOrDefault("APP_TO_DESCR");
                        instance.Owner = records.GetStringOrDefault("OWNER");
                        instance.CheckerName = records.GetStringOrDefault("CHECKER_NAME");
                        instance.ApplicationRevisionId = records.GetLongOrDefault("REVISION_ID");
                        if (instance.ProjectStatus == 307014 || instance.ProjectStatus == 307016)
                            instance.Days = 0;
                        else
                            instance.Days = GetDifferenceDays(instance.ApplicationRevisionId, instance.DueDate);
                        instance.HaulierMnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                        instance.ESDALRefNum = records.GetInt32OrDefault("ESDAL_REF_NUMBER");
                        instance.ProjectId = records.GetLongOrDefault("Project_id");
                     }
                     else
                     {
                         instance.Type = records.GetStringOrDefault("MOVE_TYPE");
                         //if((inboxMovementsParams.UserType == UserType.SOA || inboxMovementsParams.UserType == UserType.PoliceALO) && records["REQUIRES_VR1"].ToString() == "1")
                         //    instance.Type = instance.Type + " VR1";
                         instance.VehicleMaxWidth = Convert.ToDouble(records.GetDecimalOrDefault("WIDTH_MAX_MTR"));
                         instance.ICAStatus = Convert.ToString(records.GetInt32OrDefault("ICA_STATUS"));
                         instance.Status = Convert.ToString(records.GetInt32OrDefault("ITEM_STATUS"));
                         instance.ReceivedDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("RECEIVED_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("RECEIVED_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                         instance.FromDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("START_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                         instance.ToDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("END_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("END_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                         instance.ESDALReference = records.GetStringOrDefault("ESDAL_REFERENCE");
                         int Notif_VerNo = 0;
                         var type = records.GetFieldType("NOTIFICATION_VERSION_NO");
                         if (type != null)   // check added for RM #10775
                             Notif_VerNo = records.GetInt16OrDefault("NOTIFICATION_VERSION_NO");
                         instance.MessageType = records.GetStringOrDefault("MOVEMENT_TYPE");
                         instance.InboxItemStatus = records.GetInt32OrDefault("ITEM_TYPE");
                         //instance.MessageType = ConvertMessage(records.GetInt32OrDefault("ITEM_TYPE"), Notif_VerNo);
                         instance.StructName = records.GetStringOrDefault("NAME");
                         instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
                         instance.ApplicationRevisionId = records.GetLongOrDefault("REVISION_ID");
                         instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                         instance.TotalRecord = (int)records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                         instance.InboxItemId = records.GetLongOrDefault("INBOX_ITEM_ID"); // Added by NetWeb for further use.
                         instance.StatuatoryPeriod = CheckStatuatory(instance.ReceivedDate, instance.FromDate);
                         instance.IsOpened = records.GetShortOrDefault("OPENED");
                         instance.IsUnopened = records.GetShortOrDefault("UNOPENED");
                         instance.IsWithdrawn = records.GetShortOrDefault("IS_WITHDRAWN");
                         instance.IsDeclined = records.GetShortOrDefault("IS_DECLINED");
                         instance.ImminentMovement = records.GetShortOrDefault("IS_IMMINENT");
                         instance.NENId = records.GetLongOrDefault("NEN_ID");
                         instance.UserAssignId = Convert.ToString(records.GetLongOrDefault("ASSIGN_TO_USER"));
                         instance.IsHistoric = (int)records.GetDecimalOrDefault("historical");
                         instance.AssignedUserName = records.GetStringOrDefault("ASSIGNED_USER_NAME");
                     }
                 }
            );
           
           
            return movementInboxObj;
        }

       

        public static bool SaveAffectedMovementDetails(AffectedStructConstrParam affectedParam)
		{
            var sectionListJson = JsonConvert.SerializeObject(affectedParam.AffectedSections);
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
            UserSchema.Sort + ".SP_SAVE_APP_STRUCTURES",
            parameter =>
            {
                parameter.AddWithValue("P_VERSION_ID", affectedParam.NotificationId, OracleDbType.Long, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_STRUCTURE_SECTIONS", sectionListJson, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                result = records.GetDataTypeName("VERSION_ID") == "Int16" ? records.GetInt16OrDefault("VERSION_ID") : (int)records.GetDecimalOrDefault("VERSION_ID");
            });

            if (result > 0)
                return true;
            else
                return false;
        }

        private static string ConvertType(int typeCode)
        {
            string type = string.Empty;
            switch (typeCode)
            {
                case 241001:
                    type = "VSO";
                    break;
                case 241002:
                    type = "SO";
                    break;
                case 241003:
                    type = "STGO Cat 1";
                    break;
                case 241004:
                    type = "STGO Cat 2";
                    break;
                case 241005:
                    type = "STGO Cat 3";
                    break;
                case 241006:
                    type = "STGO mobile crane Cat A";
                    break;
                case 241007:
                    type = "STGO mobile crane Cat B";
                    break;
                case 241008:
                    type = "STGO mobile crane Cat C";
                    break;
                case 241009:
                    type = "STGO engg plant";
                    break;
                case 241010:
                    type = "STGO road recovery";
                    break;
                case 241011:
                    type = "Wheeled construction & use";
                    break;
                case 241012:
                    type = "Tracked";
                    break;
                case 241013:
                    type = "STGO engg plant wheeled";
                    break;
                case 241014:
                    type = "STGO engg plant tracked";
                    break;
                default:
                    break;
            }
            return type;
        }

        private static string ConvertMessage(int messageCode, int notificationVerNo = 0)
        {
            string message = string.Empty;
            //------FOR NEN only----------
            if (notificationVerNo > 1 && (messageCode == 911001 || messageCode == 911002 || messageCode == 911003 || messageCode == 911004 || messageCode == 911005 || messageCode == 911010 || messageCode == 911011))// Case is for NE Renotification for NEN processing thru all planning and assigning scrutiny
            {
                messageCode = 312015;
            }
            else if (notificationVerNo > 1 && (messageCode == 911006 || messageCode == 911007 || messageCode == 911008 || messageCode == 911009))
            {
                return "NE agreed renotification";
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
                case 312016://NEN Notification VIA API
                    message = "NE Notification API";
                    break;
                case 312017://NEN Notification VIA API
                    message = "NE ReNotification API";
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

        private static bool CheckStatuatory(string receivedDate, string fromDate)
        {
            bool isStatuatory = false;
            int statuatoryPeriod = 5;
            CultureInfo provider = CultureInfo.InvariantCulture;

            DateTime recDate = DateTime.ParseExact(receivedDate, "dd/MM/yyyy", provider);
            DateTime frmDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", provider);
            int dateDifference = (frmDate.Date - recDate.Date).Days;
            if (dateDifference < statuatoryPeriod)
            {
                isStatuatory = true;
            }
            return isStatuatory;
        }

        public static int GetDifferenceDays(long appRevId, string dueDate)
        {
            CultureInfo culture = new CultureInfo("en-GB");
            dueDate = dueDate.Replace("/", "-");
            int result = 0;
            DateTime now = DateTime.Now;
            string currentDate = now.ToString("dd-MM-yyyy");
            DateTime due; 
            if (dueDate != "" && dueDate != "01-01-0001")
            {

                due = DateTime.ParseExact(dueDate, "dd-MM-yyyy", culture);
                
            }
			else
			{
                due= DateTime.Now;
            }
            if (due.Date < now.Date)
            {
                result = 0;
            }
            else if (due.Date == now.Date)
            {
                result = 1;
            }
            else
            {
                List<int> CountryIDList = new List<int>();
                List<long> routePartDet;
                routePartDet = GetRoutePartid(appRevId);
                foreach (long routePart in routePartDet)
                {
                    if (routePart != 0)
                        CountryIDList = GetCountryId((int)routePart);
                }
                if (CountryIDList.Count == 0)
                {
                    result = result + GetDays(currentDate, dueDate, 0);
                }
                else
                {
                    foreach (int CountryID in CountryIDList)
                    {
                        if (CountryID != 0)
                            result = result + GetDays(currentDate, dueDate, CountryID);
                    }
                }
            }
            return result;
        }

        private static int GetDays(string currentDate, string dueDate, int countryId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance
                (
                    result,
                    UserSchema.Sort + ".SP_CALC_DAYS_DIFF",
                     parameter =>
                     {
                         parameter.AddWithValue("P_Current_date", currentDate, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_Due_Date", dueDate, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_CountryId", countryId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         result = (int)records.GetDecimalOrDefault("VAL_CNT");
                     }
                );
            return result;
        }

        public static List<int> GetCountryId(int routeID = 0)
        {
            List<int> result = new List<int>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
              result,
              UserSchema.Sort + ".SP_GET_COUNTRYNAME",
               parameter =>
               {
                   parameter.AddWithValue("P_ROUTE_ID", routeID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               (records, instance) =>
               {

                   result.Add(records.GetInt32OrDefault("COUNTRY_ID"));
               }
            );
            return result;
        }

        public static List<long> GetRoutePartid(long appRevId)
        {
            List<long> result = new List<long>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
              result,
              UserSchema.Sort + ".SP_GET_ROUTEPART_ID",
               parameter =>
               {
                   parameter.AddWithValue("P_APP_REV_ID", appRevId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
               (records, instance) =>
               {

                   result.Add(records.GetLongOrDefault("ROUTE_PART_ID"));
               }
            );
            return result;
        }


        #region Get Contact Id
        public static int GetContactDetails(int userId)
        {
            ContactModel Contact = new ContactModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    Contact,
                     UserSchema.Portal + ".GET_CONTACT_ID_BY_USERID",
                    parameter =>
                    {
                        parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.ContactId = Convert.ToInt32(records.GetLongOrDefault("contact_id"));
                        }
                );
            return Contact.ContactId;
        }
        #endregion

        #region Get orderno by esdal reference
        public static string GetSpecialOrderNo(string ESDALReferenceNo)
        {
            string orderNo = string.Empty;
            MovementModel Movement = new MovementModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                Movement,
                 UserSchema.Portal + ".GET_SO_NUMBER",
                parameter =>
                {
                    parameter.AddWithValue("p_ESDAL_REF_NUMBER", ESDALReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        orderNo = records.GetStringOrDefault("order_no");
                    }
                );
            return orderNo;
        }
        #endregion

        #region Get documentId based on esdal reference and contactId
        public static long GetDocumentID(string ESDALReferenceNo, long organisationId)
        {
            long documentId = 0;
            MovementModel Movement = new MovementModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                Movement,
                 UserSchema.Portal + ".GET_DOCUMENT_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_ESDAL_REF_NUMBER", ESDALReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        documentId = records.GetLongOrDefault("DOCUMENT_ID");
                    }
                );
            return documentId;
        }
        #endregion

        #region Edit inbox items open status
        public static int EditInboxItemOpenStatus(long inboxItemId, long organisationId)
        {
            int editFlag = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    editFlag,
                    UserSchema.Portal + ".SP_EDIT_INBOXITEMS_OPENSTATUS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_INBOX_ITEM_ID", inboxItemId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORGID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_resultset", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     (records, instance) =>
                     {
                         editFlag = (int)records.GetDecimalOrDefault("FLAG");
                     }
            );
            return editFlag;
        }
        #endregion

        #region Get Auhorize movement general proposed
        public static MovementModel GetAuthorizeMovementGeneralProposed(MovementModelParams objMovementModelParams)
        {
            MovementModel movement = new MovementModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                movement,
                UserSchema.Portal + ".GET_MOVE_AUTH_GEN_PROJECT",
                parameter =>
                {
                    parameter.AddWithValue("P_MNEMONIC", objMovementModelParams.Mnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDALREFNUM", objMovementModelParams.ESDALReferenceNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSIONNO", objMovementModelParams.Version, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE", objMovementModelParams.Route, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ITEMID", objMovementModelParams.InboxId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDALREF", objMovementModelParams.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACTID", objMovementModelParams.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATIONID", objMovementModelParams.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ProjectId = records.GetLongOrDefault("project_id");
                        instance.RevisionId = records.GetLongOrDefault("REVISION_ID");
                        instance.VehicleClassification = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        instance.VehicleClassificationName = records.GetStringOrDefault("VEHICLE_CLASSIFICATION_NAME");
                        instance.FromDescription = records.GetStringOrDefault("FROM_DESCR");
                        instance.ToDescription = records.GetStringOrDefault("TO_DESCR");
                        instance.IsMostRecent = records.GetInt16OrDefault("IS_MOST_RECENT");
                        instance.HAJobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");
                        instance.HauliersReference = records.GetStringOrDefault("HAULIERS_REF");
                        instance.ApplicationDate = records.GetDateTimeOrDefault("APPLICATION_DATE");
                        instance.LoadDescription = records.GetStringOrDefault("LOAD_DESCR");
                        instance.MoveStartDate = records.GetDateTimeOrDefault("MOVEMENT_START_DATE");
                        instance.MoveEndDate = records.GetDateTimeOrDefault("MOVEMENT_END_DATE");
                        instance.StartTime = instance.MoveStartDate;
                        instance.EndTime = instance.MoveEndDate;
                        instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                        instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                        instance.NotesOnEscort = records.GetStringOrDefault("NOTESONESCORT");
                        instance.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");
                        instance.VR1Number = records.GetStringOrDefault("VR1_NUMBER");
                        instance.InternalNotes = records.GetStringOrDefault("APPLICATION_NOTES");
                        instance.Status = records.GetDecimalOrDefault("ITEM_STATUS");
                        instance.StatusName = records.GetStringOrDefault("STATUS_NAME");
                        instance.IncludeDockCaution = records.GetDecimalOrDefault("INCLUDE_DOCK_CAUTION");
                        instance.DocumentId = Convert.ToInt64(records.GetDecimalOrDefault("DOCUMENT_ID"));
                        instance.Licence = records.GetStringOrDefault("HAULIER_LICENCE_NO");
                        instance.NotesFromHaulier = records.GetStringOrDefault("NOTESFROMHAULIER");
                        instance.TotalMoves = records.GetInt16OrDefault("TOTAL_MOVES");
                        instance.MaxPartPerMove = records.GetInt16OrDefault("MAX_PARTS_PER_MOVE");
                        instance.HAContactDetails = records.GetByteArrayOrNull("INBOUND_DOC");
                        if (instance.VersionId > 0)
                            instance.AuthenticationNotesToHaulier = GetAuthNotesToHaulier((int)instance.VersionId);
                    }
            );
            return movement;
        }
        #endregion

        public static byte[] GetAuthNotesToHaulier(int version_ID)
        {
            byte[] AuthenticationNotesToHaulier = null;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                 AuthenticationNotesToHaulier,
                "STP_ROUTE_ASSESSMENT.SP_Get_AuthNotesToHaulier",
                 parameter =>
                 {
                     parameter.AddWithValue("p_version_no", version_ID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                 },
                 record =>
                 {
                     AuthenticationNotesToHaulier = record.GetByteArrayOrNull("NOTES_FOR_HAULIER");

                 });
            return AuthenticationNotesToHaulier;
        }

        #region Get vehicle details
        public static List<VehicleConfigration> GetVehicleList(string mnemonic, string ESDALReferenceNo, string version, long notificationId, int isSimplified)
        {
            List<VehicleConfigration> vehicleList = new List<VehicleConfigration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleList,
                UserSchema.Portal + ".SP_GET_AUTH_MOVMT_VHCLS",

                 parameter =>
                 {
                     parameter.AddWithValue("P_MNEMONIC", mnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ESDAL_REF", Convert.ToInt32(ESDALReferenceNo), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_VER_NO", Convert.ToInt32(version), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("SIMPLIFIED_FLAG", isSimplified, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },

                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
					instance.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                    instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                    instance.VehicleCompList = GetVR1VehicleConfigPosn(Convert.ToInt32(instance.VehicleId), UserSchema.Portal);
                }
            );
            return vehicleList;
        }
		public static List<VehicleConfigList> GetVR1VehicleConfigPosn(int vhclID, string userSchema = UserSchema.Portal)
        {
            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".SP_ROUTE_GET_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    result.SubType = records.GetInt32OrDefault("SUB_TYPE");
                    result.ComponentTypeId = records.GetInt32OrDefault("COMPONENT_TYPE");
                    result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                    result.LongPosn = records.GetInt16OrDefault("LAT_POSN");
                    result.ComponentSubTypeId = records.GetInt32OrDefault("SUB_TYPE");
                }
            );
            return listVehclRegObj;
        }
        #endregion

        #region  Get NH and Haulier ContactId By Name
        public static MovementModel GetHAAndHaulierContactIdByName(MovementModel movement)
        {
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                movement,
                UserSchema.Portal + ".GET_HA_HAULIER_CONTACT_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_HAULIER_CONTACT", movement.HaulierContact, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HA_CONTACT", movement.HAContact, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.HaulierContactId = records.GetDecimalOrDefault("HaulierContactId");
                        instance.HAContactId = records.GetDecimalOrDefault("HAContactId");
                    }
            );
            return movement;
        }
        #endregion

        #region Get collaboration notes list
        public static List<CollaborationNotes> GetCollaborationNotes(long DocumentId, long OrganisationId)
        {
            List<CollaborationNotes> Collaborationnotes = new List<CollaborationNotes>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                Collaborationnotes,
                UserSchema.Portal + ".GET_COLLABORATION_NOTES",
                parameter =>
                {
                    parameter.AddWithValue("P_DOCUMENT_ID", DocumentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATIONID", OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.DocumentId = records.GetLongOrDefault("DOCUMENT_ID");
                        //string whenUK = "8/25/2022";
                        instance.When = records.GetDateTimeOrDefault("WHEN").ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).Replace('-', '/');
                        instance.Notes = records.GetStringOrDefault("Notes");
                        instance.AcknowledgedAgainst = records.GetStringOrDefault("ACKNOWLEDGED_AGAINST");
                        instance.AcknowledgedWhen = records.GetDateTimeOrDefault("ACKNOWLEDGED_WHEN");
                        instance.Acknowledged = records.GetInt32OrDefault("ACKNOWLEDGED");
                    }
            );
            return Collaborationnotes;
        }
        #endregion

        #region Get Authorized movement general detail
        public static MovementModel GetAuthorizeMovementGeneral(long notificationId, long inboxId, long contactId, string ESDALReferenceNo, long organisationId)
        {
            MovementModel movement = new MovementModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                movement,
                UserSchema.Portal + ".GET_MOVEMENT_AUTH_GENERAL",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ITEMID", inboxId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDALREF", ESDALReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACTID", contactId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATIONID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
                        instance.VehicleClassification = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        instance.VehicleClassificationName = records.GetStringOrDefault("VEHICLE_CLASSIFICATION_NAME");
                        instance.FromDescription = records.GetStringOrDefault("FROM_DESCR");
                        instance.ToDescription = records.GetStringOrDefault("TO_DESCR");
                        try
                        {
                            instance.IsMostRecent = records.GetInt16OrDefault("IS_MOST_RECENT");
                        }
                        catch
                        {
                            instance.IsMostRecent = short.Parse(records.GetDecimalOrDefault("IS_MOST_RECENT").ToString());
                        }
                        instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                        instance.HAJobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");
                        instance.HauliersReference = records.GetStringOrDefault("HAULIERS_REF");
                        instance.NotificationDate = records.GetDateTimeOrDefault("NOTIFICATION_DATE");
                        instance.ReceivedDate = records.GetFieldType("RECEIVED_DATE") !=null?records.GetDateTimeOrDefault("RECEIVED_DATE"):DateTime.MinValue;
                        instance.LoadDescription = records.GetStringOrDefault("LOAD_DESCR");
                        instance.InternalNotes = records.GetStringOrDefault("INTERNAL_NOTES");

                        instance.AnalysisId = Convert.ToInt64(records["ANALYSIS_ID"]);// Modified for NEN and Existing ESDAL2
                        instance.NotesOnEscort = records.GetStringOrDefault("NOTESONESCORT");
                        instance.VR1Number = records.GetStringOrDefault("VR1_NUMBER");
                        instance.RequiresVR1 = records.GetInt16OrDefault("REQUIRES_VR1");

                        instance.Status = records.GetDecimalOrDefault("ITEM_STATUS");
                        instance.StatusName = records.GetStringOrDefault("STATUS_NAME");
                        instance.DocumentId = Convert.ToInt64(records.GetDecimalOrDefault("DOCUMENT_ID"));
                        instance.SoNumbers = records.GetStringOrDefault("SO_NUMBERS");
                        instance.RouteType = records.GetStringOrDefault("ROUTE_TYPE");
                        instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                        //Adding data to list after BLOB removed
                        instance.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");
                        instance.EmailAddress = records.GetStringOrDefault("EMAIL_ADDRESS");
                        instance.Licence = records.GetStringOrDefault("LICENSE");
                        instance.MoveStartDate = records.GetDateTimeOrDefault("MOVE_START_DATE");
                        instance.MoveEndDate = records.GetDateTimeOrDefault("MOVE_END_DATE");
                        instance.StartTime = instance.MoveStartDate;
                        instance.EndTime = instance.MoveEndDate;
                        instance.TotalMoves = (int)records.GetDecimalOrDefault("TOTAL_MOVES");
                        instance.MaxPartPerMove = records.GetInt32OrDefault("MAX_PIECES_PER_MOVE");
                        instance.Notes = instance.LoadDescription;
                        instance.NotesFromHaulier = records.GetStringOrDefault("HAUL_NOTES");
                        instance.IndemnityConfirmation = records.GetInt16OrDefault("INDEMNITY_CONFIRMATION") > 0 ? "Yes" : "No";
                        instance.OnBehalfOf = records.GetStringOrDefault("ON_BEHALF_OF");
                        instance.IsSimplified = records.GetStringOrDefault("IS_SIMPLIFIED") != "0";
                        instance.DispensationIds = records.GetStringOrDefault("DISPENSATION_ID");
                        instance.OtherContactDetails = records.GetStringOrDefault("OTHER_CONTACT_DETAILS");
                        instance.HAContactDetails = records.GetByteArrayOrNull("INBOUND_DOC");
                        instance.VSONo = records.GetStringOrDefault("VSO_NUMBER");
                    }
            );
            return movement;
        }
        #endregion

        #region Get haulier contactId from haulier name
        public static MovementModel GetHaulierContactId(MovementModel movement)
        {
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                movement,
                UserSchema.Portal + ".GET_HAULIER_CONTACT_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_HAULIER_CONTACT", movement.HaulierContact, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.HaulierContactId = records.GetDecimalOrDefault("CONTACT_ID");
                    }
            );
            return movement;
        }
        #endregion

        #region Update status of Inbox item
        public static long UpdateInboxItemStatus(InboxItemStatusParams objInboxItemStatusParams)
        {
            long transmissionId = 0;
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".UPDATE_MOVEMENT_OPENSTATUS",
                parameter =>
                {
                    parameter.AddWithValue("P_ORGANISATION_ID", objInboxItemStatusParams.OrganisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDAL_REF", objInboxItemStatusParams.ESDALRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        transmissionId = records.GetLongOrDefault("TRANSMISSION_ID");
                    }
            );
            return transmissionId;
        }
        #endregion

        #region Get Special order by Notification code
        public static List<SpecialOrder> GetSpecialOrders(string ESDALReference)
        {
            List<SpecialOrder> specialOrder = new List<SpecialOrder>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                specialOrder,
                UserSchema.Sort + ".GET_SPECIAL_ORDERS",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDAL_REF", ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.OrderNo = records.GetStringOrDefault("ORDER_NO");
                        instance.SignedDate = records.GetDateTimeOrDefault("SIGNED_DATE");
                        instance.ExpiryDate = records.GetDateTimeOrDefault("EXPIRY_DATE");
                    }
            );
            return specialOrder;
        }
        #endregion

        #region Get VR1 list
        public static List<VR1> GetVR1s(string VR1No)
        {
            List<VR1> vr1 = new List<VR1>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vr1,
                UserSchema.Portal + ".GET_VR1_BY_VR1_NUMBER",
                parameter =>
                {
                    parameter.AddWithValue("P_VR1_NUMBER", VR1No, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VR1Id = records.GetInt32OrDefault("VR1_ID");
                        instance.VR1Number = records.GetStringOrDefault("VR1_NUMBER");
                        instance.ScottishVR1Number = records.GetStringOrDefault("SCOTTISH_VR1_NUMBER");
                    }
            );
            return vr1;
        }

        #endregion

        #region Get Notification details by code

        public static List<RelatedCommunication> GetNotificationDetailsByCode(string notificationCode, string route, long organisationId, long projectId)
        {
            List<RelatedCommunication> specialOrder = new List<RelatedCommunication>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                specialOrder,
                UserSchema.Portal + ".GET_NOTIFICATION_LIST_BY_CODE",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIFICATION_CODE", notificationCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTETYPE", route, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.NotificationId = Convert.ToInt64(records.GetDecimalOrDefault("NOTIFICATION_ID"));
                        instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                        instance.PreviousNotificationESDALReference = records.GetStringOrDefault("PREV_NOTIFICATION_ESDAL_REF");

                        instance.NotificationDate = records.GetDateTimeOrDefault("NOTIFICATION_DATE");
                        instance.ItemStatus = records.GetStringOrDefault("ITEM_TYPE");
                    }
            );
            return specialOrder;
        }
        #endregion

        #region Get NEN VehicleList
        public static List<VehicleConfigration> GetNENVehicleList(long NENId, long inboxId, long organisationId = 0)
        {
            List<VehicleConfigration> vehicleList = new List<VehicleConfigration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleList,
                UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_NEN_VEH_LIST",
                 parameter =>
                 {
                     parameter.AddWithValue("P_NEN_ID", NENId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_INBOX_ID", inboxId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },

                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
					instance.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                    instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                    instance.VehicleCompList = GetVR1VehicleConfigPosn((int)instance.VehicleId, UserSchema.Portal);
                }
            );
            return vehicleList;
        }
        #endregion

        public static List<SORTMovementList> GetSORTMovementRelatedToStructList(int organisationId, int pageNumber, int pageSize, long structID)//, SORTAdvancedMovementFilter SORTMovementFilterAdvanced   
        {
            List<SORTMovementList> movementListObj = new List<SORTMovementList>();
            //Setup Procedure LIST_MOVEMENT
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementListObj,
                UserSchema.Sort + ".SP_GET_MOV_RELATED_TO_STRUCT",
                parameter =>
                {
                    parameter.AddWithValue("PAGENUMBER", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCT_ID", structID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FOLDERID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ProjectID = records.GetLongOrDefault("Project_id");
                        instance.Type = records.GetStringOrDefault("TYPE");
                        instance.Priority = records.GetInt32OrDefault("PRIORITY");
                        instance.FromDate = (string.IsNullOrEmpty(records.GetStringOrDefault("START_DATE"))) ? string.Empty : Convert.ToDateTime(records.GetStringOrDefault("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.ToDate = (string.IsNullOrEmpty(records.GetStringOrDefault("END_DATE"))) ? string.Empty : Convert.ToDateTime(records.GetStringOrDefault("END_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.ApplicationDate = records.GetStringOrEmpty("APPLICATION_DATE");
                        instance.ProjectStatus = records.GetInt32OrDefault("PROJECT_STATUS");
                        instance.AppStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                        instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                        instance.ESDALRef = records.GetStringOrDefault("ESDAL");
                        instance.WorkStatus = records.GetInt32OrDefault("CHECKING_STATUS");
                        instance.VehicleClassification = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        instance.DueDate = records.GetStringOrEmpty("DUE_DATE");
                        instance.MoveFrom = records.GetStringOrDefault("APP_FROM_DESCR");
                        instance.MoveTo = records.GetStringOrDefault("APP_TO_DESCR");
                        instance.HaulierMnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                        instance.ProjectEsdalReference = records.GetInt32OrDefault("ESDAL_REF_NUMBER");
                        instance.ApplicationRevisionNumber = records.GetShortOrDefault("REVISION_NO");
                        instance.MovementVersionNumber = Convert.ToInt32(records.GetDecimalOrDefault("VERSION_NO"));
                        instance.MovementVersionID = records.GetLongOrDefault("VERSION_ID");
                        instance.MovementRevisionID = records.GetLongOrDefault("REVISION_ID");
                        instance.AppRevID = records.GetLongOrDefault("REVISION_ID");
                        instance.ProjectID = records.GetLongOrDefault("APP_PRO_ID");
                        instance.EnterBySORT = records.GetInt16OrDefault("ENTERED_BY_SORT");
                        instance.OrganisationID = (int)records.GetLongOrDefault("ORGANISATION_ID");
                        instance.Owner = records.GetStringOrDefault("OWNER");
                        instance.CheckerName = records.GetStringOrDefault("CHECKER_NAME");
                        instance.IsWithdrawn = (int)records.GetShortOrDefault("IS_WITHDRAWN");
                        instance.IsDeclined = (int)records.GetShortOrDefault("IS_DECLINED");
                        instance.TotalRecordCount = (int)records.GetDecimalOrDefault("ROW_COUNT");
                        if (instance.ProjectStatus == 307014 || instance.ProjectStatus == 307016)
                            instance.Days = 0;
                        else
                            instance.Days = GetDifferenceDays(instance.AppRevID, instance.DueDate);
                    }
            );
            return movementListObj;
        }

        public static List<SORTMovementList> GetListSortMovement(int pageNumber, int pageSize, SORTMovementFilter movementFilter, SortAdvancedMovementFilter SORTMovementFilterAdvanced, bool IsCreCandidateOrCreAppl,SortMapFilter sortobjMapFilter, bool planMovement,int sortOrder,int sortType)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "GetListSortMovement actionResult method started successfully");
            if(SORTMovementFilterAdvanced.LogicOpr == null)
            {
                SORTMovementFilterAdvanced.LogicOpr = 0;
            }
            if (planMovement)
            {
                if (SORTMovementFilterAdvanced.ESDALReferenceNumber != null)
                {
                    movementFilter.ESDALReference = SORTMovementFilterAdvanced.ESDALReferenceNumber.Trim();
                }
            }
            else
            {
                if (movementFilter.ESDALReference != null)
                {
                    movementFilter.ESDALReference = movementFilter.ESDALReference.Trim();
                }
            }
            if (movementFilter.HaulierName != null)
            {
                movementFilter.HaulierName = movementFilter.HaulierName.Trim();
            }
            if (movementFilter.AllocateUser != null)
            {
                movementFilter.AllocateUser = movementFilter.AllocateUser.Trim();
            }
            if (movementFilter.CheckingUser != null)
            {
                movementFilter.CheckingUser = movementFilter.CheckingUser.Trim();
            }
            if (SORTMovementFilterAdvanced.JobReference != null)
            {
                SORTMovementFilterAdvanced.JobReference = SORTMovementFilterAdvanced.JobReference.Trim();
            }
            if (SORTMovementFilterAdvanced.SONum != null)
            {
                SORTMovementFilterAdvanced.SONum = SORTMovementFilterAdvanced.SONum.Trim();
            }
            if (SORTMovementFilterAdvanced.VRNum != null)
            {
                SORTMovementFilterAdvanced.VRNum = SORTMovementFilterAdvanced.VRNum.Trim();
            }
            if (SORTMovementFilterAdvanced.Keywords != null)
            {
                SORTMovementFilterAdvanced.Keywords = SORTMovementFilterAdvanced.Keywords.Trim();
            }
            if (SORTMovementFilterAdvanced.LoadDetails != null)
            {
                SORTMovementFilterAdvanced.LoadDetails = SORTMovementFilterAdvanced.LoadDetails.Trim();
            }
            if (SORTMovementFilterAdvanced.StartPoint != null)
            {
                SORTMovementFilterAdvanced.StartPoint = SORTMovementFilterAdvanced.StartPoint.Trim();
            }
            if (SORTMovementFilterAdvanced.EndPoint != null)
            {
                SORTMovementFilterAdvanced.EndPoint = SORTMovementFilterAdvanced.EndPoint.Trim();
            }

            String status = null;
            String versionStatus = null;
            String[] statusCount = null;
            String[] versionStatusCount = null;
            int statCount = 0;
            int versionStatCount = 0;
            int historicDataFlag = 0;
            String application_Type = null;
            int withdrawnFlag = 0, declinedFlag = 0, myPrjects = 0, notifyFlag=0;
            int applicationTypeCount = 0;
            List<SORTMovementList> movementListObj = new List<SORTMovementList>();

            #region date conversion
            if (!string.IsNullOrEmpty(movementFilter.SOSignFromDate))
            {
                movementFilter.SOSignFrom = Convert.ToDateTime(movementFilter.SOSignFromDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.SOSignToDate))
            {
                movementFilter.SOSignTo = Convert.ToDateTime(movementFilter.SOSignToDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.MovFromDate))
            {
                movementFilter.MovFrom = Convert.ToDateTime(movementFilter.MovFromDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.MovToDate))
            {
                movementFilter.MovTo = Convert.ToDateTime(movementFilter.MovToDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.ApplicationFromDate))
            {
                movementFilter.ApplicationFrom = Convert.ToDateTime(movementFilter.ApplicationFromDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.ApplicationToDate))
            {
                movementFilter.ApplicationTo = Convert.ToDateTime(movementFilter.ApplicationToDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.WorkDueFromDate))
            {
                movementFilter.WorkDueFrom = Convert.ToDateTime(movementFilter.WorkDueFromDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.WorkDueToDate))
            {
                movementFilter.WorkDueTo = Convert.ToDateTime(movementFilter.WorkDueToDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.AssignFromDate))
            {
                movementFilter.AssignFrom = Convert.ToDateTime(movementFilter.AssignFromDate);
            }
            if (!string.IsNullOrEmpty(movementFilter.AssignToDate))
            {
                movementFilter.AssignTo = Convert.ToDateTime(movementFilter.AssignToDate);
            }
            #endregion

            //to pass Application Status as a string of values to the DAO
            if (movementFilter.Unallocated)
            {
                status = String.Concat(status, "307001,");
            }
            if (movementFilter.InProgress)
            {
                status = String.Concat(status, "307002,");
            }
            if (movementFilter.Proposed)
            {
                status = String.Concat(status, "307003,");
            }
            if (movementFilter.ReProposed)
            {
                status = String.Concat(status, "307004,");
            }
            if (movementFilter.Agreed)
            {
                status = String.Concat(status, "307005,");
            }

            if (movementFilter.AgreedRevised)
            {
                status = String.Concat(status, "307006,");
            }
            if (movementFilter.AgreedRecleared)
            {
                status = String.Concat(status, "307007,");
            }
            if (movementFilter.Withdrawn)
            {
                status = String.Concat(status, "307008,");
                withdrawnFlag = 1;
            }
            if (movementFilter.Declined)
            {
                status = String.Concat(status, "307009,");
                declinedFlag = 1;
            }
            if (movementFilter.Planned)
            {
                status = String.Concat(status, "307014,");
            }
            if (movementFilter.Approved)
            {
                status = String.Concat(status, "307016,");
            }
            if (movementFilter.Revised)
            {
                status = String.Concat(status, "307011,");
            }
            if (movementFilter.IsNotified)
            {
                notifyFlag = 1;
            }

            if (!string.IsNullOrEmpty(status))
            {
                status = status.TrimEnd(',');//To remove "," at the end of string
                statusCount = status.Split(',');
                statCount = statusCount.Length;
            }

            if (!string.IsNullOrEmpty(versionStatus))
            {
                versionStatus = versionStatus.TrimEnd(',');//To remove "," at the end of string
                versionStatusCount = versionStatus.Split(',');
                versionStatCount = versionStatusCount.Length;
            }

            //check for historic records
            if (movementFilter.InclHistoricMov)
                {
                    historicDataFlag = 1;
                }

            if (movementFilter.ShowMyProjects)
            {
                myPrjects = 1;
            }

            if (SORTMovementFilterAdvanced.ApplicationType == 1)
            {
                application_Type = String.Concat(application_Type, "241002,");
            }
            else if (SORTMovementFilterAdvanced.ApplicationType == 2)
            {

                application_Type = String.Concat(application_Type, "241003,241004,241005");
            }

            if (!string.IsNullOrEmpty(application_Type))
            {
                application_Type = application_Type.TrimEnd(',');//To remove "," at the end of string
                applicationTypeCount = application_Type.Split(',').Length;
            }
            if (IsCreCandidateOrCreAppl)
            {
                status = "307002,307003,307004,307005,307006,307007,307014,307011";
                statCount = 8;
                application_Type = "241002";
                applicationTypeCount = 1;
                movementFilter.ESDALReference = SORTMovementFilterAdvanced.ESDALReferenceNumber;
                movementFilter.HaulierName = SORTMovementFilterAdvanced.HaulierName;
            }

            if (planMovement)
            {
                if (SORTMovementFilterAdvanced.ApplicationType == 1)
                    application_Type = "241002";
                else if (SORTMovementFilterAdvanced.ApplicationType == 2)
                    application_Type = "241003,241004,241005";
                else if (SORTMovementFilterAdvanced.ApplicationType == 3)
                    application_Type = "241002,241003,241004,241005";
                status = "307002,307003,307004,307005,307006,307007,307014,307011,307016";
                statCount = status.Split(',').Length;
                applicationTypeCount = application_Type.Split(',').Length;
            }

            #region Geomerty
            OracleCommand cmd = new OracleCommand();
            OracleParameter oraclePointGeo = cmd.CreateParameter();
            oraclePointGeo.OracleDbType = OracleDbType.Object;
            oraclePointGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oraclePointGeo.Value = sortobjMapFilter==null? null: sortobjMapFilter.Geometry; //Saving Route point geometry 
            oraclePointGeo.ParameterName = "p_geometry";

            OracleCommand cmd1 = new OracleCommand();
            OracleParameter structList = cmd1.CreateParameter();
            structList.ParameterName = "P_STRUCTURE_LIST";
            structList.OracleDbType = OracleDbType.Int32;
            structList.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            structList.Value = new int[5] { 1, 2, 3, 4, 5 };


            //string structList = "1,2,34,44"; //sortobjMapFilter==null ? 0 : sortobjMapFilter.StructureList;
            #endregion
            string SortProcName = ".SP_GET_SORT_MOV_LIST_FILTER";//SP_GET_SORT_MOV_LIST_FILTER
            if (IsCreCandidateOrCreAppl || planMovement)
            {
                SortProcName = ".SP_GET_SORT_MOV_LIST_FILTER_T";//SP_GET_SORT_MOV_LIST_FILTER_T
            }
            //Setup Procedure LIST_MOVEMENT
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementListObj,
                UserSchema.Sort + SortProcName,
                parameter =>
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "SP SP_GET_SORT_MOV_LIST_FILTER call started successfully");
                    parameter.AddWithValue("P_USER_ID", movementFilter.UserID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("STATUS", status, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("STATUS_COUNT", statCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("VER_STATUS", versionStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("VER_STATUS_COUNT", versionStatCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ESDAL_REF_NUM", movementFilter.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAULIER_NAME", movementFilter.HaulierName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    if(string.IsNullOrEmpty(movementFilter.SOSignFromDate))
                    {
                        parameter.AddWithValue("P_FROM_SIGN_DATE", movementFilter.SOSignFromDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("P_FROM_SIGN_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.SOSignFromDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    if (string.IsNullOrEmpty(movementFilter.SOSignToDate))
                    {
                        parameter.AddWithValue("P_TO_SIGN_DATE", movementFilter.SOSignToDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);                     
                    }
                    else
                    {
                        parameter.AddWithValue("P_TO_SIGN_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.SOSignToDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }


                    if (string.IsNullOrEmpty(movementFilter.ApplicationFromDate))
                    {
                        parameter.AddWithValue("FROM_APP_DATE", movementFilter.ApplicationFromDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("FROM_APP_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.ApplicationFromDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    if (string.IsNullOrEmpty(movementFilter.ApplicationToDate))
                    {
                        parameter.AddWithValue("TO_APP_DATE", movementFilter.ApplicationToDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }                   
                    else
                    {
                        parameter.AddWithValue("TO_APP_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.ApplicationToDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }

                    if (string.IsNullOrEmpty(movementFilter.AssignFromDate))
                    {
                        parameter.AddWithValue("FROM_ASSIGNED_DATE", movementFilter.AssignFromDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("FROM_ASSIGNED_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.AssignFromDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }

                    if (string.IsNullOrEmpty(movementFilter.AssignToDate))
                    {
                        parameter.AddWithValue("TO_ASSIGNED_DATE", movementFilter.AssignToDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("TO_ASSIGNED_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.AssignToDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }

                    if (string.IsNullOrEmpty(movementFilter.WorkDueFromDate))
                    {
                        parameter.AddWithValue("FROM_DUE_DATE", movementFilter.WorkDueFromDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("FROM_DUE_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.WorkDueFromDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    if (string.IsNullOrEmpty(movementFilter.WorkDueToDate))
                    {
                        parameter.AddWithValue("TO_DUE_DATE", movementFilter.WorkDueToDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("TO_DUE_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.WorkDueToDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }

                    if (string.IsNullOrEmpty(movementFilter.MovFromDate))
                    {
                        parameter.AddWithValue("START_MOV_DATE", movementFilter.MovFromDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("START_MOV_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.MovFromDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    if (string.IsNullOrEmpty(movementFilter.MovToDate))
                    {
                        parameter.AddWithValue("END_MOV_DATE", movementFilter.MovToDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("END_MOV_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.MovToDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }

                    if (string.IsNullOrEmpty(movementFilter.DeclFromDate))
                    {
                        parameter.AddWithValue("FROM_DECLINATION_DATE", movementFilter.DeclFromDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("FROM_DECLINATION_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.DeclFromDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    if (string.IsNullOrEmpty(movementFilter.DeclToDate))
                    {
                        parameter.AddWithValue("TO_DECLINATION_DATE", movementFilter.DeclToDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }
                    else
                    {
                        parameter.AddWithValue("TO_DECLINATION_DATE", TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(movementFilter.DeclToDate)), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32737);
                    }


                    parameter.AddWithValue("P_PLANNER_USER", movementFilter.AllocateUser, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CHECKER_USER", movementFilter.CheckingUser, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("WITHDRAWN_FLAG", withdrawnFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("DECLINED_FLAG", declinedFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    if (!IsCreCandidateOrCreAppl && !planMovement)
                    {
                        parameter.AddWithValue("p_is_notified", notifyFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    }
                    parameter.AddWithValue("P_MY_PROJECTS", myPrjects, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_JOB_REF_NUM", SORTMovementFilterAdvanced.JobReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_SPECIAL_ORDER_NUM", SORTMovementFilterAdvanced.SONum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VR1_NUMBER", SORTMovementFilterAdvanced.VRNum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("APPLICATION_TYPE", application_Type, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("APPLICATION_TYPE_COUNT", applicationTypeCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_KEYWORDS", SORTMovementFilterAdvanced.Keywords, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LOAD_DESCR", SORTMovementFilterAdvanced.LoadDetails, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_START_DESCR", SORTMovementFilterAdvanced.StartPoint, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DESCR", SORTMovementFilterAdvanced.EndPoint, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);


                    if (!IsCreCandidateOrCreAppl && !planMovement)
                    {
                        if (sortobjMapFilter != null)
                        {
                            if (sortobjMapFilter.StructureList != null)
                            {
                                parameter.AddWithValue("p_structure_list", sortobjMapFilter.StructureList, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                                parameter.AddWithValue("p_struct_count", sortobjMapFilter.StructureCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                            }
                            else
                            {
                                parameter.AddWithValue("p_structure_list", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                                parameter.AddWithValue("p_struct_count", 0, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                            }
                        }
                        else
                        {
                            parameter.AddWithValue("p_structure_list", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_struct_count", 0, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        }
                        parameter.Add(oraclePointGeo);
                    }
                    parameter.AddWithValue("P_HISTORIC", historicDataFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("SHOW_HISTORIC",1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("preset_filter",sortType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_query_string", SORTMovementFilterAdvanced.QueryString, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FOLDER_ID", SORTMovementFilterAdvanced.FolderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_logic_opr", SORTMovementFilterAdvanced.LogicOpr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ProjectID = records.GetLongOrDefault("Project_id");
                        instance.Type = records.GetStringOrDefault("TYPE");
                        instance.Priority = records.GetInt32OrDefault("PRIORITY");
                        instance.FromDate= (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("START_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.ToDate= (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("END_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("END_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //instance.FromDate = (string.IsNullOrEmpty(records.GetDateTimeOrEmpty("START_DATE"))) ? string.Empty : Convert.ToDateTime(records.GetStringOrDefault("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //instance.ToDate = (string.IsNullOrEmpty(records.GetStringOrDefault("END_DATE"))) ? string.Empty : Convert.ToDateTime(records.GetStringOrDefault("END_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.ApplicationDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("APPLICATION_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("APPLICATION_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //instance.ApplicationDate = records.GetStringOrEmpty("APPLICATION_DATE");
                        instance.ProjectStatus = records.GetInt32OrDefault("PROJECT_STATUS");
                        instance.AppStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                        instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                        instance.ESDALRef = records.GetStringOrDefault("ESDAL");
                        instance.WorkStatus = records.GetInt32OrDefault("CHECKING_STATUS");
                        instance.VehicleClassification = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        instance.DueDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("DUE_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("DUE_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //instance.DueDate = records.GetStringOrEmpty("DUE_DATE");
                        instance.MoveFrom = records.GetStringOrDefault("APP_FROM_DESCR");
                        instance.MoveTo = records.GetStringOrDefault("APP_TO_DESCR");
                        instance.HaulierMnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                        instance.ProjectEsdalReference = records.GetInt32OrDefault("ESDAL_REF_NUMBER");
                        instance.ApplicationRevisionNumber = records.GetShortOrDefault("REVISION_NO");
                        instance.MovementVersionNumber = Convert.ToInt32(records.GetDecimalOrDefault("VERSION_NO"));
                        instance.MovementVersionID = records.GetLongOrDefault("VERSION_ID");
                        instance.MovementRevisionID = records.GetLongOrDefault("REVISION_ID");
                        instance.AppRevID = records.GetLongOrDefault("REVISION_ID");
                        instance.ProjectID = records.GetLongOrDefault("APP_PRO_ID");
                        instance.EnterBySORT = records.GetInt16OrDefault("ENTERED_BY_SORT");
                        instance.OrganisationID = (int)records.GetLongOrDefault("ORGANISATION_ID");
                        instance.Owner = records.GetStringOrDefault("OWNER");
                        instance.CheckerName = records.GetStringOrDefault("CHECKER_NAME");
                        instance.IsWithdrawn = (int)records.GetShortOrDefault("IS_WITHDRAWN");
                        instance.IsDeclined = (int)records.GetShortOrDefault("IS_DECLINED");
                        instance.TotalRecordCount = (int)records.GetDecimalOrDefault("ROW_COUNT");

                        instance.PlannerID = records.GetLongOrDefault("PLANNER_USER_ID");
                        instance.CheckerID = records.GetLongOrDefault("CHECKER_USER_ID");
                        if (!IsCreCandidateOrCreAppl && !planMovement)
                        {
                            instance.IsNotified = (int)records.GetDecimalOrDefault("SORT_NOTIFIED");
                        }
                        if (instance.ProjectStatus == 307014 || instance.ProjectStatus == 307016)
                            instance.Days = 0;
                        else
                            instance.Days = GetDifferenceDays(instance.AppRevID, instance.DueDate);



                    }
            );
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "SP SP_GET_SORT_MOV_LIST_FILTER call endeed successfully");

            return movementListObj;
        }


        #region Get Delegation Arrangement List
        public static List<DelegArrangeNameList> GetDelegationList(int organisationId)
        {
            List<DelegArrangeNameList> Objdeleg = new List<DelegArrangeNameList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                Objdeleg,
                 UserSchema.Portal +".STP_STRUCTURE_LIST.SP_GET_DELEGATION_LIST",
                parameter =>
                {
                    parameter.AddWithValue("D_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("D_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                        instance.ArrangementName = records.GetStringOrDefault("NAME");
                    }
            );
            return Objdeleg;
        }
        #endregion

        #region  Get Haulier Movement List

        public static List<MovementsList> GetListMovement(HaulierMovementsListParams objHaulierMovementsListParams)
        {
            if (objHaulierMovementsListParams.AdvancedMovementFilter.MyReference != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.MyReference = objHaulierMovementsListParams.AdvancedMovementFilter.MyReference.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference = objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.Client != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.Client = objHaulierMovementsListParams.AdvancedMovementFilter.Client.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.FleetId != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.FleetId = objHaulierMovementsListParams.AdvancedMovementFilter.FleetId.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration = objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.StartOrEnd != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.StartOrEnd = objHaulierMovementsListParams.AdvancedMovementFilter.StartOrEnd.Trim();
            }
            if(objHaulierMovementsListParams.AdvancedMovementFilter.LogicOpr == null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.LogicOpr = 0;
            }
            String status = null;
            String versionStatus = null;
            String vehicleType = null;
            String vehicleTypeNotif = null;
            String[] statusCount = null;
            String[] versionStatusCount = null;
            String[] vehicleCount = null;
            String[] vehicleCountNotif = null;
            int statCount = 0;
            int vhclCount = 0;
            int vhclCountNotif = 0;
            int versionStatCount = 0;
           int Historic = 0;
            int IsWithdrawn = 0;
            int IsDeclined = 0;
            int? ApplnWIP = null;
            string NotifWIP = null;
            int ApplnWIPCount = 0;
            int NotifWIPCount = 0;
            bool hasApplicationFilters = false; //Flag indicating whether Application any filter is selected
            bool hasNotificaionFilters = false; //Flag indicating whether Notification any filter is selected
            int? requiresVR1Flag = null;
            /*
             * Flag determining whether notification or Application need to be searched 
             * 1- Only Application
             * 2- Only Notification
             * 3- Both Application and NOtification
             */
            int nSearchFlag = 3;
            List<MovementsList> movementListObj = new List<MovementsList>();
            #region date conversion
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.MovementFromDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.MovementFrom = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.MovementFromDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.MovementToDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.MovementTo = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.MovementToDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFromDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFrom = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFromDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationToDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationTo = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationToDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFromDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFrom = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFromDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationToDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.NotificationTo = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationToDate);
            }
            #endregion

            //to pass Application Status as a string of values to the DAO
            if (objHaulierMovementsListParams.MovementFilter.Submitted)
            {
                status = String.Concat(status, "308002,");
            }
            if (objHaulierMovementsListParams.MovementFilter.ReceivedByHA)
            {
                status = String.Concat(status, "308003,");
            }
            if (objHaulierMovementsListParams.MovementFilter.WithdrawnApplications)
            {
                IsWithdrawn = 1;
            }
            if (objHaulierMovementsListParams.MovementFilter.DeclinedApplications)
            {
                IsDeclined = 1;
            }
            if (objHaulierMovementsListParams.MovementFilter.ApprovedVR1)
            {
                //status = String.Concat(status, "308007,");
                versionStatus = String.Concat(versionStatus, "305014,");
            }

            if (objHaulierMovementsListParams.MovementFilter.WorkInProgress)
            {
                status = String.Concat(status, "308001,");
            }
            if (!string.IsNullOrEmpty(status))
            {
                status = status.TrimEnd(',');//To remove "," at the end of string
                statusCount = status.Split(',');
                statCount = statusCount.Length;
            }

            //to pass Version Status as a string of values to the DAO
            if (objHaulierMovementsListParams.MovementFilter.Agreed)
            {
                status = String.Concat(status, "308009,");
                versionStatus = String.Concat(versionStatus, "305004,305005,305006,");
            }
            if (objHaulierMovementsListParams.MovementFilter.ProposedRoute)
            {
                status = String.Concat(status, "308009,");
                versionStatus = String.Concat(versionStatus, "305002,305003,");
            }
            if (!string.IsNullOrEmpty(versionStatus))
            {
                versionStatus = versionStatus.TrimEnd(',');//To remove "," at the end of string
                versionStatusCount = versionStatus.Split(',');
                versionStatCount = versionStatusCount.Length;
            }

            if (objHaulierMovementsListParams.MovementFilter.WorkInProgressNotification || objHaulierMovementsListParams.MovementFilter.Notifications)
            {
                if (objHaulierMovementsListParams.MovementFilter.SO)
                {
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241002,");
                }
                if (objHaulierMovementsListParams.MovementFilter.VSO)
                {
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241001,");
                }
                if (objHaulierMovementsListParams.MovementFilter.STGO)
                {
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241003,241004,241005,241006,241007,241008,241009,241010,241012,241013,241014,");
                }
                if (objHaulierMovementsListParams.MovementFilter.CandU)
                {
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241011,");
                }
                if (objHaulierMovementsListParams.MovementFilter.Tracked)
                {
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241012,");
                }
                if (objHaulierMovementsListParams.MovementFilter.STGOVR1)
                {
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241003,241004,241005,");
                    requiresVR1Flag = 1;
                }

                if (!string.IsNullOrEmpty(vehicleTypeNotif))
                {
                    vehicleTypeNotif = vehicleTypeNotif.TrimEnd(',');//To remove "," at the end of string
                    vehicleCountNotif = vehicleTypeNotif.Split(',');
                    vhclCountNotif = vehicleCountNotif.Length;
                }
            }
            else if (objHaulierMovementsListParams.MovementFilter.ReceivedByHA || objHaulierMovementsListParams.MovementFilter.ProposedRoute || objHaulierMovementsListParams.MovementFilter.DeclinedApplications || objHaulierMovementsListParams.MovementFilter.ApprovedVR1 || objHaulierMovementsListParams.MovementFilter.Submitted || objHaulierMovementsListParams.MovementFilter.Agreed || objHaulierMovementsListParams.MovementFilter.WithdrawnApplications || objHaulierMovementsListParams.MovementFilter.WorkInProgressApplication)
            {
                if (objHaulierMovementsListParams.MovementFilter.SO)
                {
                    vehicleType = String.Concat(vehicleType, "241002,");
                }
                if (objHaulierMovementsListParams.MovementFilter.VSO)
                {
                    vehicleType = String.Concat(vehicleType, "241001,");
                }
                if (objHaulierMovementsListParams.MovementFilter.STGO)
                {
                    vehicleType = String.Concat(vehicleType, "241003,241004,241005,241006,241007,241008,241009,241010,241012,241013,241014,");
                }
                if (objHaulierMovementsListParams.MovementFilter.CandU)
                {
                    vehicleType = String.Concat(vehicleType, "241011,");
                }
                if (objHaulierMovementsListParams.MovementFilter.Tracked)
                {
                    vehicleType = String.Concat(vehicleType, "241012,");
                }
                if (objHaulierMovementsListParams.MovementFilter.STGOVR1)
                {
                    vehicleType = String.Concat(vehicleType, "241003,241004,241005,");
                }

                if (!string.IsNullOrEmpty(vehicleType))
                {
                    vehicleType = vehicleType.TrimEnd(',');//To remove "," at the end of string
                    vehicleCount = vehicleType.Split(',');
                    vhclCount = vehicleCount.Length;
                }
            }
            else
            {
                if (objHaulierMovementsListParams.MovementFilter.SO)
                {
                    vehicleType = String.Concat(vehicleType, "241002,");
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241002,");
                }
                if (objHaulierMovementsListParams.MovementFilter.VSO)
                {
                    vehicleType = String.Concat(vehicleType, "241001,");
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241001,");
                }
                if (objHaulierMovementsListParams.MovementFilter.STGO)
                {
                    vehicleType = String.Concat(vehicleType, "241003,241004,241005,241006,241007,241008,241009,241010,241013,241014,");
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241003,241004,241005,241006,241007,241008,241009,241010,241013,241014,");
                }
                if (objHaulierMovementsListParams.MovementFilter.CandU)
                {
                    vehicleType = String.Concat(vehicleType, "241011,");
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241011,");
                }
                if (objHaulierMovementsListParams.MovementFilter.Tracked)
                {
                    vehicleType = String.Concat(vehicleType, "241012,");
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241012,");
                }
                if (objHaulierMovementsListParams.MovementFilter.STGOVR1)
                {
                    vehicleType = String.Concat(vehicleType, "241003,241004,241005,");
                    vehicleTypeNotif = String.Concat(vehicleTypeNotif, "241003,241004,241005,");
                    requiresVR1Flag = 1;
                }

                if (!string.IsNullOrEmpty(vehicleType))
                {
                    vehicleType = vehicleType.TrimEnd(',');//To remove "," at the end of string
                    vehicleCount = vehicleType.Split(',');
                    vhclCount = vehicleCount.Length;
                }
                if (!string.IsNullOrEmpty(vehicleTypeNotif))
                {
                    vehicleTypeNotif = vehicleTypeNotif.TrimEnd(',');//To remove "," at the end of string
                    vehicleCountNotif = vehicleTypeNotif.Split(',');
                    vhclCountNotif = vehicleCountNotif.Length;
                }
            }

            if (objHaulierMovementsListParams.MovementFilter.WorkInProgressApplication)
            {
                ApplnWIP = 1;
                ApplnWIPCount = 1;
            }
            if ((objHaulierMovementsListParams.MovementFilter.WorkInProgressNotification && objHaulierMovementsListParams.MovementFilter.Notifications)) //|| (!movementFilter.WorkInProgressNotif && !movementFilter.Notifications))
            {
                NotifWIP = "0,1";
                NotifWIPCount = 2;
            }

            if ((objHaulierMovementsListParams.MovementFilter.WorkInProgressNotification && !objHaulierMovementsListParams.MovementFilter.Notifications))
            {
                NotifWIP = "1";
                NotifWIPCount = 1;
            }

            if ((!objHaulierMovementsListParams.MovementFilter.WorkInProgressNotification && objHaulierMovementsListParams.MovementFilter.Notifications))
            {
                NotifWIP = "0";
                NotifWIPCount = 1;
            }
            if ((!objHaulierMovementsListParams.MovementFilter.WorkInProgressNotification && !objHaulierMovementsListParams.MovementFilter.Notifications))
            {
                NotifWIP = null;
                NotifWIPCount = 0;
            }
            if (status == null && (objHaulierMovementsListParams.MovementFilter.WorkInProgressNotification || objHaulierMovementsListParams.MovementFilter.Notifications))
            {
                status = null;
                statCount = 0;
            }

            //Determine the search flag
            hasApplicationFilters = status != null || versionStatus != null || ApplnWIP != null || objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFrom !=null;
            hasNotificaionFilters = objHaulierMovementsListParams.MovementFilter.Notifications || objHaulierMovementsListParams.MovementFilter.WorkInProgressNotification || objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFrom != null;
            if (hasApplicationFilters && !hasNotificaionFilters)
            {
                nSearchFlag = 1; //Only Applications
            }
            else if (!hasApplicationFilters && hasNotificaionFilters)
            {
                nSearchFlag = 2; //Only Notificaion
            }

            if(objHaulierMovementsListParams.MovementFilter.NewCollabration|| objHaulierMovementsListParams.MovementFilter.ReadCollaboration)
            {
                nSearchFlag = 2; //Only Notificaion
            }
            else if (!hasNotificaionFilters && (objHaulierMovementsListParams.MovementFilter.DeclinedApplications || objHaulierMovementsListParams.MovementFilter.WithdrawnApplications))
            {
                nSearchFlag = 1;
            }
            // check for historic records
            if (objHaulierMovementsListParams.AdvancedMovementFilter.IncludeHistoricalData)
                {
                    Historic = 1;
                }

            string procName = "";
            if (string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.Keyword) || string.IsNullOrWhiteSpace(objHaulierMovementsListParams.AdvancedMovementFilter.Keyword))
            {
                if (objHaulierMovementsListParams.ShowPreviousSORTRoute == 2)
                {
                    if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.SONum))
                    {
                        procName = ".SP_GET_PREV_MOVE_ROUTE_SO";
                    }
                    else
                    {
                        procName = ".SP_GET_PREV_MOVE_ROUTE_LIST";
                    }
                }
                else if (objHaulierMovementsListParams.ShowPreviousSORTRoute == 1)
                {
                    if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.SONum))
                    {
                        procName = ".SP_GET_PREV_MOVE_VEH_SO";
                    }
                    else
                    {
                        procName = ".SP_GET_PREV_MOVE_VEH_LIST";
                    }
                }
                else if (objHaulierMovementsListParams.PrevMovImport&& objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationType == 3)
                {
                    procName = ".SP_GET_PREV_MOVE_LIST_FILTER";
                }
                else
                {
                    procName = ".SP_GET_MOVEMENT_LIST_FILTER";
                }
            }
            else
            {

                if (objHaulierMovementsListParams.ShowPreviousSORTRoute == 2 || objHaulierMovementsListParams.ShowPreviousSORTRoute == 1)
                {
                    procName = ".SP_GET_MOVE_CAND_ROUTE_K";
                }
                else
                {
                    procName = ".SP_GET_MOVEMENT_LIST_FILTER_K";
                }
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationType == 1)
            {
                nSearchFlag = 1;
                vehicleType = objHaulierMovementsListParams.AdvancedMovementFilter.VehicleClass;
                versionStatus = "0,305002,305003,305004,305005,305006,305013,305014";
                vhclCount = vehicleType != null ? vehicleType.Split(',').Length:0;
                versionStatCount = versionStatus.Split(',').Length;
                status = "308002,308003,308007,308009";
                statCount = status.Split(',').Length;
                procName = ".SP_GET_MOVEMENT_LIST_FILTER";
              //  Historic = 0;
            }
            else if (objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationType == 2)
            {
                nSearchFlag = 2;
                vehicleTypeNotif = objHaulierMovementsListParams.AdvancedMovementFilter.VehicleClass;
                versionStatus = "305013";
                versionStatCount = versionStatus.Split(',').Length;
                vhclCountNotif = vehicleTypeNotif != null? vehicleTypeNotif.Split(',').Length:0;
                NotifWIP = "0";
                NotifWIPCount = 1;
                procName = ".SP_GET_MOVEMENT_LIST_FILTER";
                //Historic = 0;
            }
            else if (objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationType == 3)
            {
                nSearchFlag = 3;
                if(objHaulierMovementsListParams.AdvancedMovementFilter.SORTOrder == 1)
                {
                    objHaulierMovementsListParams.AdvancedMovementFilter.SORTOrder = 4;
                    objHaulierMovementsListParams.PresetFilter = 4;
                }
                vehicleTypeNotif = "241002,241003,241004,241005,241006,241007,241008,241009,241010,241011,241012,241013,241014";
                vehicleType = "241002,241003,241004,241005";
                versionStatus = "0,305002,305003,305004,305005,305006,305013,305014";
                vhclCount = vehicleType.Split(',').Length;
                vhclCountNotif = vehicleTypeNotif.Split(',').Length;
                versionStatCount = versionStatus.Split(',').Length;
                status = "308002,308003,308007,308009";
                statCount = status.Split(',').Length;
                NotifWIP = "0";
                NotifWIPCount = 1;
                procName = ".SP_GET_MOVEMENT_LIST_FILTER";
               // Historic = 0;
            }
            else if (objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationType == 4)
            {
                nSearchFlag = 1;
                vehicleType = objHaulierMovementsListParams.AdvancedMovementFilter.VehicleClass;
                versionStatus = "305002,305003,305004,305005,305006,305014";
                vhclCount = vehicleType != null ? vehicleType.Split(',').Length : 0;
                versionStatCount = versionStatus.Split(',').Length;
                procName = ".SP_GET_MOVEMENT_LIST_FILTER";
            }
            if (Historic == 0 && status == "308005")
            {
                return movementListObj;
            }
            else
            {

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    movementListObj,
                    objHaulierMovementsListParams.UserSchema + procName,
                    parameter =>
                    {

                        parameter.AddWithValue("ORG_ID", objHaulierMovementsListParams.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageNumber", objHaulierMovementsListParams.PageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", objHaulierMovementsListParams.PageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("STATUS", status, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("STATUS_COUNT", statCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("VER_STATUS", versionStatus, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("VER_STATUS_COUNT", versionStatCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("W_IN_PROG_APP", ApplnWIP, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("W_IN_PROG_APP_COUNT", ApplnWIPCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("W_IN_PROG_NOTIF", NotifWIP, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("W_IN_PROG_NOTIF_COUNT", NotifWIPCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NEEDS_ATTN", objHaulierMovementsListParams.MovementFilter.NeedsAttention ? "1" : null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SHOW_REC", objHaulierMovementsListParams.MovementFilter.MostRecentVersion ? "1" : null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("TYPE_CODE_NOTIF", vehicleTypeNotif, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("TYPE_COUNT_NOTIF", vhclCountNotif, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("TYPE_CODE_APPL", vehicleType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("TYPE_COUNT_APPL", vhclCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("REQUIRES_VR1_FLAG", requiresVR1Flag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ESDAL_REF", objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MY_REF", objHaulierMovementsListParams.AdvancedMovementFilter.MyReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("CLIENT", objHaulierMovementsListParams.AdvancedMovementFilter.Client, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WEIGHT", objHaulierMovementsListParams.AdvancedMovementFilter.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WEIGHT_1", objHaulierMovementsListParams.AdvancedMovementFilter.GrossWeight1, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WEIGHT_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.WeightCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WIDTH", objHaulierMovementsListParams.AdvancedMovementFilter.OverallWidth, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WIDTH_1", objHaulierMovementsListParams.AdvancedMovementFilter.OverallWidth1, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WIDTH_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.WidthCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("LENGTH_IN", objHaulierMovementsListParams.AdvancedMovementFilter.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("LENGTH_IN_1", objHaulierMovementsListParams.AdvancedMovementFilter.Length1, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("LEN_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.LengthCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MAX_HIEGHT", objHaulierMovementsListParams.AdvancedMovementFilter.Height, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MAX_HIEGHT_1", objHaulierMovementsListParams.AdvancedMovementFilter.Height1, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("HEIGHT_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.HeightCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MAX_AXLE", objHaulierMovementsListParams.AdvancedMovementFilter.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MAX_AXLE_1", objHaulierMovementsListParams.AdvancedMovementFilter.AxleWeight1, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("AXLE_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.AxleCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("APP_DATE_1", objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFrom, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("APP_DATE_2", objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationTo, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("START_MOV_DATE", objHaulierMovementsListParams.AdvancedMovementFilter.MovementFrom, OracleDbType.Date, ParameterDirectionWrap.Input, 32737);
                        if (objHaulierMovementsListParams.AdvancedMovementFilter.MovementFrom != null && objHaulierMovementsListParams.AdvancedMovementFilter.MovementTo == null)
                            objHaulierMovementsListParams.AdvancedMovementFilter.MovementTo = DateTime.Now.AddYears(5);
                        parameter.AddWithValue("END_MOV_DATE", objHaulierMovementsListParams.AdvancedMovementFilter.MovementTo, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("NOTIF_DATE_1", objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFrom, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NOTIF_DATE_2", objHaulierMovementsListParams.AdvancedMovementFilter.NotificationTo, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("KEYWORD", objHaulierMovementsListParams.AdvancedMovementFilter.Keyword, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("FLEET_NO", objHaulierMovementsListParams.AdvancedMovementFilter.FleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("START_POINT", objHaulierMovementsListParams.AdvancedMovementFilter.StartPoint, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("END_POINT", objHaulierMovementsListParams.AdvancedMovementFilter.EndPoint, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);


                        parameter.AddWithValue("VEH_REG", objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SORT_ORDER", objHaulierMovementsListParams.AdvancedMovementFilter.SORTOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("PRESET_FILTER", objHaulierMovementsListParams.PresetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("FLAG", nSearchFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);




                        //   parameter.AddWithValue("SHOW_HISTORIC", Historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("SHOW_HISTORIC", Historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("WITHDRAWN_FLAG", IsWithdrawn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("DECLINED_FLAG", IsDeclined, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_FOLDER_ID", objHaulierMovementsListParams.MovementFilter.FolderId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SPECIAL_ORDER_NUM", objHaulierMovementsListParams.AdvancedMovementFilter.SONum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VR1_NUMBER", objHaulierMovementsListParams.AdvancedMovementFilter.VRNum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_HAULIER_NAME", objHaulierMovementsListParams.AdvancedMovementFilter.HaulierName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_LOAD_DETAILS", objHaulierMovementsListParams.AdvancedMovementFilter.LoadDetails, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_NOTIFY_VSO", objHaulierMovementsListParams.MovementFilter.NotifyVSO, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_query_string_1", objHaulierMovementsListParams.AdvancedMovementFilter.QueryString1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_query_string_2", objHaulierMovementsListParams.AdvancedMovementFilter.QueryString2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_logic_opr", objHaulierMovementsListParams.AdvancedMovementFilter.LogicOpr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_IS_NEW_COLLABORATION", objHaulierMovementsListParams.MovementFilter.NewCollabration ? "1" : "0", OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_IS_READ_COLLABORATION", objHaulierMovementsListParams.MovementFilter.ReadCollaboration ? "1" : "0", OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.ApplicationRevisionNo = records.GetDataTypeName("REVISION_NO") == "Int16" ? records.GetInt16OrDefault("REVISION_NO") : (int)records.GetDecimalOrDefault("REVISION_NO");
                            instance.MovementRevisionId = records.GetDataTypeName("REVISION_ID") == "" ? 0 : records.GetLongOrDefault("REVISION_ID");
                            instance.ProjectId = records.GetDataTypeName("APP_PRO_ID") == "Int64" ? records.GetLongOrDefault("APP_PRO_ID") : (long)records.GetDecimalOrDefault("APP_PRO_ID");
                            instance.Attention = records.GetDataTypeName("NEEDS_ATTENTION") == "Int16" ? (Int32)records.GetInt16OrDefault("NEEDS_ATTENTION") : Convert.ToInt32(records.GetDecimalOrDefault("NEEDS_ATTENTION"));
                            instance.NotificationId = records.GetDataTypeName("NOTIFICATION_ID") == "Int64" ? Convert.ToInt32(records.GetLongOrDefault("NOTIFICATION_ID")) : Convert.ToInt32(records.GetDecimalOrDefault("NOTIFICATION_ID"));
                            instance.NotificationStatus = records.GetDataTypeName("WORK_IN_PROGRESS") == "Int16" ? records.GetInt16OrDefault("WORK_IN_PROGRESS") : Convert.ToInt16(records.GetDecimalOrDefault("WORK_IN_PROGRESS"));
                            instance.MovementType = records.GetStringOrDefault("VEHICLE_CLASS_TYPE");
                            try
                            {
                                instance.FromDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("START_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch(Exception ex)
                            {

                            }
                            try
                            {
                                instance.ToDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("END_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("END_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex)
                            {

                            }
                            instance.Status = records.GetStringOrDefault("STATUS");
                            if (objHaulierMovementsListParams.MovementFilter.ReceivedByHA && versionStatCount == 0)
                                instance.VersionStatus = 0;
                            else
                                instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                            instance.ESDALReference = records.GetStringOrDefault("ESDAL");
                            instance.MyReference = records.GetStringOrDefault("MY_REF");
                            instance.HaulierMnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                            instance.ProjectESDALReference = records.GetInt32OrDefault("ESDAL_REF_NUMBER");
                            instance.MovementVersionNumber = Convert.ToInt32(records.GetDecimalOrDefault("VERSION_NO"));
                            instance.MovementVersionId = records.GetLongOrDefault("VERSION_ID");
                            instance.ColloborationStatus = Convert.ToInt32(records.GetDecimalOrDefault("COLLABORATION_STATUS"));
                            instance.FromDescription = records.GetStringOrDefault("APP_FROM_DESCR");
                            instance.ToDescription = records.GetStringOrDefault("APP_TO_DESCR");
                            instance.NotificationFromDesc = records.GetStringOrDefault("FROM_DESCR");
                            instance.NotificationToDesc = records.GetStringOrDefault("TO_DESCR");
                            instance.ClientDescription = records.GetStringOrDefault("CLIENT_DESCR");
                            instance.ApplicationRevisionId = instance.MovementRevisionId;
                            instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                            instance.NotificationMyReference = records.GetStringOrDefault("HAULIERS_REF");
                            instance.TotalRowCount = (int)records.GetDecimalOrDefault("ROW_COUNT");
                            instance.IsHistoric = Convert.ToInt16(records.GetDecimalOrDefault("HISTORICAL"));
                            instance.IsNotified = records.GetShortOrDefault("IS_NOTIFY_FLAG");
                            instance.IsWithdrawn = records.GetShortOrDefault("IS_WITHDRAWN_FLAG");
                            instance.IsDeclined = records.GetShortOrDefault("IS_DECLINED_FLAG");
                            instance.EnteredBySort = records.GetDataTypeName("ENTERED_BY_SORT") =="" ? 0 :  records.GetInt16OrDefault("ENTERED_BY_SORT");
                            instance.ColloborationStatus = (int)records.GetDecimalOrDefault("COLLABORATION_STATUS_COUNT");
                            instance.SearchFlag = nSearchFlag;
                            instance.NeedAttentionFilterFlag = objHaulierMovementsListParams.MovementFilter.NeedsAttention;
                            instance.ContentRefNum = records.GetStringOrDefault("CONTENT_REF_NUM");
                            //if (!objHaulierMovementsListParams.PrevMovImport || objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationType != 3)
                               // instance.ContentRefNum = records.GetStringOrDefault("CONTENT_REF_NUM");
                            if (objHaulierMovementsListParams.UserSchema.ToLower() == UserSchema.Portal)
                                instance.MaximumVersion = records.GetDataTypeName("MAX_STAT") == "Int16" ? records.GetInt16OrDefault("MAX_STAT") : (int)records.GetDecimalOrDefault("MAX_STAT");

                        }

                );
            }
            return movementListObj;
        }
        #endregion

        #region GetFolderList
        internal static List<FolderNameList> GetListOfFolders(long organisationId, string userSchema)
        {
            var getList = new List<FolderNameList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                getList,
                userSchema + ".SP_LIST_PROJECTFOLDER",
                parameter =>
                {
                    parameter.AddWithValue("P_org_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.FolderId = records.GetLongOrDefault("FOLDER_ID");
                    instance.FolderName = records.GetStringOrDefault("FOLDER_NAME");
                });
            return getList;
        }
        #endregion

        /// <summary>
        /// Get outbound document xml by notification id
        /// </summary>
        /// <param name="notificationId">notification id</param>
        /// <param name="ContactId">ContactId id</paramGetNotificationOutboundDocsXML
        /// <returns>returns xml</returns>
        public static string GetNotificationOutboundDocsXML(long notificationId)
        {
            MovementModel Movement = new MovementModel();
            Byte[] OutboundDocumentsXMLByteArray = null;
            string recipientXMLInformation = string.Empty;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  Movement,
                  UserSchema.Portal + ".GET_OUTBOUNDDOCUMENTSXML",
                  parameter =>
                  {
                      parameter.AddWithValue("P_NOTIFICATION_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                      (records, instance) =>
                      {
                          OutboundDocumentsXMLByteArray = records.GetByteArrayOrNull("OUTBOUND_NOTIFICATION");
                      }
              );
            if (OutboundDocumentsXMLByteArray != null)
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(OutboundDocumentsXMLByteArray);

                    recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
                }
                catch (System.Xml.XmlException)
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(OutboundDocumentsXMLByteArray, 0, OutboundDocumentsXMLByteArray.Length);
                }
            }
            return recipientXMLInformation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="organisationId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static DocumentInfo GetOutboundDocsXMLwithUserType(long documentId, long organisationId, string userSchema)
        {
            DocumentInfo DocumentInfo = new DocumentInfo();

            Byte[] OutboundDocumentsXMLByteArray = null;
            string recipientXMLInformation = string.Empty;
            string userType = string.Empty;

            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  DocumentInfo,
                  userSchema + ".SP_GET_OUTBOUNDDOCS_WITH_USER",
                  parameter =>
                  {
                      parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_DOC_ID", documentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                      (records, instance) =>
                      {
                          //OutboundDocumentsXMLByteArray = records.GetByteArrayOrNull("DOCUMENT");
                          userType = records.GetStringOrDefault("USER_TYPE"); // police  / soa
                      }
              );

                if (OutboundDocumentsXMLByteArray != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    try
                    {
                        byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(OutboundDocumentsXMLByteArray);

                        recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
                    }
                    catch (System.Xml.XmlException XE)
                    {
                        recipientXMLInformation = Encoding.UTF8.GetString(OutboundDocumentsXMLByteArray, 0, OutboundDocumentsXMLByteArray.Length);
                    }

                    //filling the xml document information.
                    DocumentInfo.XMLDocument = recipientXMLInformation;
                    DocumentInfo.DocumentId = documentId;
                    DocumentInfo.OrganisationId = organisationId;
                    DocumentInfo.UserType = userType;
                }


                return DocumentInfo;
            }
            catch (Exception ex)
            {
                throw ex;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/GetOutboundDocsXMLwithUserType, Exception: {0}", ex));
            }
        }

        public static MovementModel GetCollaborationStatus(long DOCUMENT_ID)
        {
            //Creating new object for CollaborationStatus
            MovementModel CollaborationStatus = new MovementModel();
            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                CollaborationStatus,
                 UserSchema.Portal + ".GET_COLLABORATION_STATUS",
                parameter =>
                {
                    parameter.AddWithValue("P_DOCUMENT_ID", DOCUMENT_ID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.CollaborationNo = records.GetInt16OrDefault("COLLABORATION_NO");
                        instance.Status = records.GetInt32OrDefault("STATUS");
                        instance.UserId = records.GetLongOrDefault("ASSIGN_TO_USER");
                    }
            );
            return CollaborationStatus;
        }

        /// <summary>
        /// Update movement status
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        public static bool ManageCollaborationStatus(MovementModel movement)
        {
            // For SO version
            string StringEsdalRef = string.Empty;
            string mnemonic = string.Empty;
            string esdalrefnum = string.Empty;
            string version = string.Empty;
            StringEsdalRef = movement.ESDALReference;
            if (!string.IsNullOrEmpty(StringEsdalRef) && !StringEsdalRef.Contains("#"))
            {
                string[] esdalRefPro = StringEsdalRef.Split('/');

                if (esdalRefPro.Length > 0)
                {
                    mnemonic = Convert.ToString(esdalRefPro[0]);
                    esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    version = Convert.ToString(esdalRefPro[2].ToUpper().Replace("S", ""));
                }
            }
            else
            {
                esdalrefnum= StringEsdalRef ;
            }
            
            //End
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
              UserSchema.Portal + ".MANAGE_COLLABORATION_EXTERNAL",
            parameter =>
            {
                parameter.AddWithValue("P_DOCUMENT_ID", movement.DocumentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_STATUS", movement.Status, OracleDbType.Int32, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_NOTES", movement.Notes, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_INBOX_ID", movement.InboxId, OracleDbType.Long, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_CONTACT_ID", movement.ContactId, OracleDbType.Long, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_USER_ID", movement.UserId, OracleDbType.Long, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_MNEMONIC", mnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_ESDALREFNUM", esdalrefnum, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_VERSIONNO", version, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_ACKNOW", movement.MailedCollab, OracleDbType.Int32, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            record =>
            {
                result = true;
            }
        );

            return result;
        }

        public static string GetProposalOutboundDocsXML(long documentNumber)
        {
            MovementModel Movement = new MovementModel();
            Byte[] OutboundDocumentsXMLByteArray = null;
            string recipientXMLInformation = string.Empty;

            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  Movement,
                  UserSchema.Sort + ".GET_PROPOSALDOCUMENTSXML",
                  parameter =>
                  {
                      parameter.AddWithValue("P_DOCUMENT_ID", documentNumber, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                  },
                      (records, instance) =>
                      {
                          OutboundDocumentsXMLByteArray = records.GetByteArrayOrNull("OUTBOUND_DOCUMENT");
                      }
              );
                if (OutboundDocumentsXMLByteArray != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    try
                    {
                        byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(OutboundDocumentsXMLByteArray);

                        recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
                    }
                    catch (System.Xml.XmlException XE)
                    {
                        recipientXMLInformation = Encoding.UTF8.GetString(OutboundDocumentsXMLByteArray, 0, OutboundDocumentsXMLByteArray.Length);
                    }
                }
                return recipientXMLInformation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get outbound document xml by esdal ref and organisation id
        /// </summary>
        /// <param name="Notificationid">notification id</param>
        /// <param name="organisationId">ContactId id</param>
        /// <returns>returns xml</returns>
        public static string GetOutboundDocsXML(string NotificationCode, long organisationId)
        {
            MovementModel Movement = new MovementModel();
            Byte[] OutboundDocumentsXMLByteArray = null;
            string recipientXMLInformation = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                 Movement,
                  UserSchema.Portal + ".GET_OUTBOUNDDOCSXML",
                 parameter =>
                 {
                     parameter.AddWithValue("P_ESDAL_REFERENCE_NO", NotificationCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                     (records, instance) =>
                     {
                         OutboundDocumentsXMLByteArray = records.GetByteArrayOrNull("DOCUMENT");
                     }
             );

            if (OutboundDocumentsXMLByteArray != null)
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(OutboundDocumentsXMLByteArray);

                    recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
                }
                catch (System.Xml.XmlException)
                {
                    recipientXMLInformation = Encoding.UTF8.GetString(OutboundDocumentsXMLByteArray, 0, OutboundDocumentsXMLByteArray.Length);
                }
            }

            return recipientXMLInformation;
        }

        public static MovementModel GetInboxItemDetails(string esdalRefNumber, long organisationId)
        {
            MovementModel inboxDetails = new MovementModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                inboxDetails,
                UserSchema.Portal + ".GET_INBOXITEMDETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_CODE", esdalRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.InboxId = records.GetLongOrDefault("INBOX_ITEM_ID");
                        instance.StatusName = records.GetStringOrDefault("STATUS_NAME");
                        int Notif_VerNo = 0;
                        Notif_VerNo = records.GetInt16OrDefault("NOTIFICATION_VERSION_NO");
                        instance.Route = ConvertMessage(records.GetInt32OrDefault("ITEM_TYPE"), Notif_VerNo);
                        instance.ItemStatus = records.GetInt32OrDefault("ITEM_STATUS");
                        instance.NenId = records.GetLongOrDefault("NEN_ID");
                    }
            );

            return inboxDetails;
        }

        /// <summary>
        /// Manage Notes on Escort
        /// </summary>
        /// <param name="movement">MovementModel model</param>
        /// <returns>Update notes on escort</returns>
        public static bool ManageNotesOnEscort(MovementModel movement)
        {
            bool result = false;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".MANAGE_MOVE_NOTES_ON_ESCORT",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", movement.NotificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTES_ON_ESCORT", movement.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_REVISION_ID", movement.RevisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = true;
                }
            );
            return result;
        }

        public static string GetHaulierUserId(string Firstname, string Surname, Int32 organisationId)
        {
            UserInfo UserInfo = new UserInfo();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                UserInfo,
               UserSchema.Portal + ".GET_HAULIERUSERID",
                parameter =>
                {
                    parameter.AddWithValue("P_FIRSTNAME", Firstname, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SURNAME", Surname, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.UserId = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
                    }
            );
            return UserInfo.UserId;
        }
        /// <summary>
        /// SaveMovementAction
        /// </summary>
        /// <param name="movementActionInsertParams"></param>
        /// <returns></returns>
       
        #region public static List<InboxSubContent> GetMovementInboxSubContent(int movementVersionId,int orgId)
        /// <summary>
        /// Get SubContent within Movement list
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static List<InboxSubContent> GetMovementInboxSubContent(int pageNumber, int pageSize, int movementVersionId, int orgId, int notifhistory)
        {
            try
            {
                //Creating new object for GetMovementList
                List<InboxSubContent> inboxSubContentObj = new List<InboxSubContent>();
                //Setup Procedure LIST_MOVEMENT
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    inboxSubContentObj,
                   UserSchema.Portal + ".SP_GET_MOVMENT_NOTIF",
                    parameter =>
                    {

                        parameter.AddWithValue("VER_ID", movementVersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_FLAG", notifhistory, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.NotificationId = records.GetLongOrDefault("NOTIFID");
                            instance.ESDALReference = records.GetStringOrDefault("CODE");
                            instance.NotificationDate = (string.IsNullOrEmpty(records.GetStringOrDefault("NOTIF_DATE"))) ? string.Empty : Convert.ToDateTime(records.GetStringOrDefault("NOTIF_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //instance.NotificationDate = records.GetStringOrDefault ("NOTIF_DATE");
                            instance.CollaborationStatus = Convert.ToInt32(records.GetDecimalOrDefault("COLLABORATION_STATUS"));
                            instance.FromSummary = records.GetStringOrDefault("FROM_DESCR");
                            instance.ToSummary = records.GetStringOrDefault("TO_DESCR");
                            instance.Description = records.GetStringOrDefault("CLIENT_DESCR");
                            //   int a =  records.GetShortOrDefault("NOTIFICATION_VERSION_NO");
                            instance.NotificationVersionNum = records.GetShortOrDefault("NOTIFICATION_VERSION_NO");
                            instance.VersionNum = records.GetShortOrDefault("VERSION_NO");
                            instance.LatestRevisionNum = records.GetShortOrDefault("LATEST_REVISION_NO");
                            instance.NotificationStatus = Convert.ToInt16(records.GetDecimalOrDefault("NN"));
                            //instance.NotificationId = records.GetInt32OrDefault("NOTIFID");
                        }
                );
                return inboxSubContentObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region public static List<InboxSubContent> GetSORTHistoryDetails(string esdalref)
        /// <summary>
        /// GetSORTHistoryDetails
        /// </summary>
        /// <param name="esdalref"></param>
        /// <returns></returns>
        public static List<InboxSubContent> GetSORTHistoryDetails(string esdalref, int versionno)
        {
            try
            {
                //Creating new object for GetMovementList
                List<InboxSubContent> inboxSubContentObj = new List<InboxSubContent>();
                //Setup Procedure LIST_MOVEMENT
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    inboxSubContentObj,
                    UserSchema.Sort + ".SP_GET_SORT_HISTORY",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ESDAL_REF", esdalref, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VERSION_NO", versionno, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {

                            instance.LatestRevisionNum = records.GetShortOrDefault("REVISION_NO");
                            instance.NotificationVersionNum = Convert.ToInt32(records.GetShortOrDefault("VERSION_NO"));
                            instance.NotificationDate = records.GetStringOrDefault("Mov_DATE");
                            instance.Description = records.GetStringOrDefault("DESCRIPTION");
                            instance.actiontype = records.GetStringOrDefault("ACTION_TYPE");
                        }
                );
                return inboxSubContentObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Update internal notes
        /// </summary>
        /// <param name="movement">MovementModel object</param>
        /// <returns>Return true or false</returns>
        public static bool ManageInternalNotes(MovementModel movement)
        {
            bool result = false;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".MANAGE_MOVEMENT_NOTES",
                parameter =>
                {
                    parameter.AddWithValue("P_DOCUMENT_ID", movement.DocumentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", movement.RevisionId, OracleDbType.Long, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_INTERNAL_NOTES", movement.InternalNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = true;
                }
            );

            return result;
        }

        public static int GetContactDetailsForDefault(int OrganisationId) // For RM#4547
        {
            ContactModel Contact = new ContactModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                Contact,
               UserSchema.Portal + ".GET_DEFAULT_CONTACT_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_ORGA_ID", OrganisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContactId = Convert.ToInt32(records.GetDecimalOrDefault("contact_id"));
                    }
            );
            return Contact.ContactId;
        }
        #region CopyMovementSortToPortal(MovementCopyDetails moveCopyDet)
        /// <summary>
        /// DAO call to copy from sort movement to portal movement 
        /// </summary>
        /// <param name="movementCopyDetails"></param>
        /// <param name="movementCloneStatus">0 By default to copy in case of distribution</param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static long CopyMovementSortToPortal(MovementCopyDetails movementCopyDetails, int movementCloneStatus, int versionID = 0, string esdalReference = "", byte[] haContactBytes = null, int organizationID = 0, string userSchema = UserSchema.Portal)
        {

            int projectStatus = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   projectStatus,
                   userSchema + ".STP_MOVEMENT_TRANS_DISTR.SP_MOVMNT_SORT_PORTAL",
                   parameter =>
                   {

                       parameter.AddWithValue("P_PROJ_ID", movementCopyDetails.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VER_NO", movementCopyDetails.VersionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_HAUL_NEMONIC", movementCopyDetails.HaulMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF_NO", movementCopyDetails.ESDALRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VER_STATUS", movementCopyDetails.MovementStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_USE_FLAG", movementCloneStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VERSION_ID", versionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ESDAL_REF", esdalReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_HA_DOCUMENT", haContactBytes, OracleDbType.Blob, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ORG_ID", organizationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   records =>
                   {
                       projectStatus = records.GetInt32OrDefault("PROJECT_STATUS");
                   });
            return projectStatus;
        }
        #endregion

        #region  Get Haulier Movement List
        public static List<MovementsList> GetPlanMovementList(HaulierMovementsListParams objHaulierMovementsListParams)
        {
            if (objHaulierMovementsListParams.AdvancedMovementFilter.MyReference != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.MyReference = objHaulierMovementsListParams.AdvancedMovementFilter.MyReference.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference = objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.Client != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.Client = objHaulierMovementsListParams.AdvancedMovementFilter.Client.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.FleetId != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.FleetId = objHaulierMovementsListParams.AdvancedMovementFilter.FleetId.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration = objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration.Trim();
            }
            if (objHaulierMovementsListParams.AdvancedMovementFilter.StartOrEnd != null)
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.StartOrEnd = objHaulierMovementsListParams.AdvancedMovementFilter.StartOrEnd.Trim();
            }
            
            List<MovementsList> movementListObj = new List<MovementsList>();

            #region date conversion
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.MovementFromDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.MovementFrom = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.MovementFromDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.MovementToDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.MovementTo = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.MovementToDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFromDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFrom = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFromDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationToDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationTo = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationToDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFromDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFrom = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFromDate);
            }
            if (!string.IsNullOrEmpty(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationToDate))
            {
                objHaulierMovementsListParams.AdvancedMovementFilter.NotificationTo = Convert.ToDateTime(objHaulierMovementsListParams.AdvancedMovementFilter.NotificationToDate);
            }
            #endregion

            int nSearchFlag;
            if (objHaulierMovementsListParams.MovementType == (int)MovementType.special_order || objHaulierMovementsListParams.MovementType == (int)MovementType.vr_1)
                nSearchFlag = 1; //Only Applications
            else if (objHaulierMovementsListParams.MovementType == (int)MovementType.notification)
                nSearchFlag = 2; //Only Notificaion
            else
                nSearchFlag = 3;
            int Historic = 0;
            if (objHaulierMovementsListParams.AdvancedMovementFilter.IncludeHistoricalData)
                Historic = 1;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementListObj,
                objHaulierMovementsListParams.UserSchema + ".SP_GET_PLAN_MOVE_LIST_FILTER",
                parameter =>
                {
                    parameter.AddWithValue("ORG_ID", objHaulierMovementsListParams.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", objHaulierMovementsListParams.PageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", objHaulierMovementsListParams.PageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_class", objHaulierMovementsListParams.VehicleClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ESDAL_REF", objHaulierMovementsListParams.AdvancedMovementFilter.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MY_REF", objHaulierMovementsListParams.AdvancedMovementFilter.MyReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CLIENT", objHaulierMovementsListParams.AdvancedMovementFilter.Client, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("WEIGHT", objHaulierMovementsListParams.AdvancedMovementFilter.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("WEIGHT_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.WeightCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("WIDTH", objHaulierMovementsListParams.AdvancedMovementFilter.OverallWidth, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("WIDTH_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.WidthCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("LENGTH_IN", objHaulierMovementsListParams.AdvancedMovementFilter.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("LEN_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.LengthCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MAX_HIEGHT", objHaulierMovementsListParams.AdvancedMovementFilter.Height, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HEIGHT_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.HeightCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MAX_AXLE", objHaulierMovementsListParams.AdvancedMovementFilter.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("AXLE_COUNT", objHaulierMovementsListParams.AdvancedMovementFilter.AxleCount, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("APP_DATE_1", objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationFrom, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("APP_DATE_2", objHaulierMovementsListParams.AdvancedMovementFilter.ApplicationTo, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("START_MOV_DATE", objHaulierMovementsListParams.AdvancedMovementFilter.MovementFrom, OracleDbType.Date, ParameterDirectionWrap.Input, 32737);
                    parameter.AddWithValue("END_MOV_DATE", objHaulierMovementsListParams.AdvancedMovementFilter.MovementTo, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTIF_DATE_1", objHaulierMovementsListParams.AdvancedMovementFilter.NotificationFrom, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTIF_DATE_2", objHaulierMovementsListParams.AdvancedMovementFilter.NotificationTo, OracleDbType.Date, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("KEYWORD", objHaulierMovementsListParams.AdvancedMovementFilter.Keyword, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("FLEET_NO", objHaulierMovementsListParams.AdvancedMovementFilter.FleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("START_END_POINT", objHaulierMovementsListParams.AdvancedMovementFilter.StartOrEnd, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("VEH_REG", objHaulierMovementsListParams.AdvancedMovementFilter.VehicleRegistration, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", objHaulierMovementsListParams.AdvancedMovementFilter.SORTOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", objHaulierMovementsListParams.PresetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("FLAG", nSearchFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SHOW_HISTORIC", Historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPECIAL_ORDER_NUM", objHaulierMovementsListParams.AdvancedMovementFilter.SONum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VR1_NUMBER", objHaulierMovementsListParams.AdvancedMovementFilter.VRNum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAULIER_NAME", objHaulierMovementsListParams.AdvancedMovementFilter.HaulierName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LOAD_DETAILS", objHaulierMovementsListParams.AdvancedMovementFilter.LoadDetails, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ApplicationRevisionNo = records.GetDataTypeName("REVISION_NO") == "Int16" ? records.GetInt16OrDefault("REVISION_NO") : (int)records.GetDecimalOrDefault("REVISION_NO");
                        instance.MovementRevisionId = records.GetDataTypeName("REVISION_ID") == "" ? 0 : records.GetLongOrDefault("REVISION_ID");
                        instance.ProjectId = records.GetDataTypeName("APP_PRO_ID") == "Int64" ? records.GetLongOrDefault("APP_PRO_ID") : (long)records.GetDecimalOrDefault("APP_PRO_ID");
                        instance.Attention = records.GetDataTypeName("NEEDS_ATTENTION") == "Int16" ? (Int32)records.GetInt16OrDefault("NEEDS_ATTENTION") : Convert.ToInt32(records.GetDecimalOrDefault("NEEDS_ATTENTION"));
                        instance.NotificationId = records.GetDataTypeName("NOTIFICATION_ID") == "Int64" ? Convert.ToInt32(records.GetLongOrDefault("NOTIFICATION_ID")) : Convert.ToInt32(records.GetDecimalOrDefault("NOTIFICATION_ID"));
                        instance.NotificationStatus = records.GetDataTypeName("WORK_IN_PROGRESS") == "Int16" ? records.GetInt16OrDefault("WORK_IN_PROGRESS") : Convert.ToInt16(records.GetDecimalOrDefault("WORK_IN_PROGRESS"));
                        instance.MovementType = records.GetStringOrDefault("VEHICLE_CLASS_TYPE");
                        instance.FromDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("START_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.ToDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("END_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("END_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.Status = records.GetStringOrDefault("STATUS");
                        instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                        instance.ESDALReference = records.GetStringOrDefault("ESDAL");
                        instance.MyReference = records.GetStringOrDefault("MY_REF");
                        instance.HaulierMnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                        instance.ProjectESDALReference = records.GetInt32OrDefault("ESDAL_REF_NUMBER");
                        instance.MovementVersionNumber = Convert.ToInt32(records.GetDecimalOrDefault("VERSION_NO"));
                        instance.MovementVersionId = records.GetLongOrDefault("VERSION_ID");
                        instance.ColloborationStatus = Convert.ToInt32(records.GetDecimalOrDefault("COLLABORATION_STATUS"));
                        instance.FromDescription = records.GetStringOrDefault("APP_FROM_DESCR");
                        instance.ToDescription = records.GetStringOrDefault("APP_TO_DESCR");
                        instance.NotificationFromDesc = records.GetStringOrDefault("FROM_DESCR");
                        instance.NotificationToDesc = records.GetStringOrDefault("TO_DESCR");
                        instance.ClientDescription = records.GetStringOrDefault("CLIENT_DESCR");
                        instance.ApplicationRevisionId = instance.MovementRevisionId;
                        instance.NotificationCode = records.GetStringOrDefault("NOTIFICATION_CODE");
                        instance.NotificationMyReference = records.GetStringOrDefault("HAULIERS_REF");
                        instance.TotalRowCount = (int)records.GetDecimalOrDefault("ROW_COUNT");
                        instance.IsHistoric = Convert.ToInt16(records.GetDecimalOrDefault("HISTORICAL"));
                        instance.IsNotified = records.GetShortOrDefault("IS_NOTIFY_FLAG");
                        instance.IsWithdrawn = records.GetShortOrDefault("IS_WITHDRAWN_FLAG");
                        instance.IsDeclined = records.GetShortOrDefault("IS_DECLINED_FLAG");
                        instance.ColloborationStatus = (int)records.GetDecimalOrDefault("COLLABORATION_STATUS_COUNT");
                        instance.SearchFlag = nSearchFlag;
                        instance.NeedAttentionFilterFlag = objHaulierMovementsListParams.MovementFilter.NeedsAttention;
                        instance.ContentRefNum = records.GetStringOrDefault("CONTENT_REF_NUM");
                    }
            );
            return movementListObj;
        }
        #endregion

        #region  Get Structure and Linked Id 
        public static List<MapStructLink> GetStructLinkId(SortMapFilter objSortMapFilterParams)
        {
            List<MapStructLink> movementLinkObj = new List<MapStructLink>();
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "GetStructLinkId actionResult method started successfully");
            string SortProcName = ".SP_GET_STRUCT_FROM_GEOMETRY";
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oraclePointGeo = cmd.CreateParameter();
            oraclePointGeo.OracleDbType = OracleDbType.Object;
            oraclePointGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oraclePointGeo.Value = objSortMapFilterParams.Geometry; //Saving Route point geometry 
            oraclePointGeo.ParameterName = "P_GEOM";
            #endregion


            //Setup Procedure LIST_MOVEMENT
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementLinkObj,
                UserSchema.Sort + SortProcName,
                parameter =>
                {
                    parameter.Add(oraclePointGeo);
                    parameter.AddWithValue("P_FLAG", objSortMapFilterParams.Flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultset", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        if (objSortMapFilterParams.Flag == 1)
                        {
                            instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                            instance.PointGeometry = records.GetGeometryOrNull("POINT_GEOMETRY") as sdogeometry;
                            instance.StructureType = instance.StructureType = records.GetStringOrDefault("STRUCTURE_TYPE");
                            instance.StructureClass = records.GetStringOrDefault("TYPE_NAME");
                            instance.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");
                            instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                        }
                        if (objSortMapFilterParams.Flag == 2)
                        {
                            instance.LinkId = (long)Convert.ToInt32(records["LINK_ID"]);
                            instance.Geometry = records.GetGeometryOrNull("GEOM") as sdogeometry;
                        }

                    }
            );
            return movementLinkObj;
          
        }
        #endregion


        #region get affected paries of proposed and agreed movement
        public static MovementContactModel GetContactedPartiesDetail(long analysisId)
        {
            try
            {
                MovementContactModel contactDetail = new MovementContactModel();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    contactDetail,
                   UserSchema.Portal + ".GET_CONTACTED_PARTIES_DETAIL",
                    parameter =>
                    {
                        parameter.AddWithValue("p_Analysis_ID", analysisId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {
                           instance.AffectedParties = records.GetByteArrayOrNull("AFFECTED_PARTIES");
                       }
                );
                return contactDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public static List<MovementsInbox> GetHomePageMovements(GetInboxMovementsParams inboxMovementsParams)
        {
           List<MovementsInbox> movementInboxObj = new List<MovementsInbox>();            
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementInboxObj,
                inboxMovementsParams.UserSchema + ".SP_GET_MOVEMENT_HOMEPAGE",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", inboxMovementsParams.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", inboxMovementsParams.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 { 
                     instance.Type = ConvertType(records.GetInt32OrDefault("MOVE_TYPE"));
                     if ((inboxMovementsParams.UserType == UserType.SOA || inboxMovementsParams.UserType == UserType.PoliceALO) && records["REQUIRES_VR1"].ToString() == "1")
                     {
                         instance.Type = instance.Type + " VR1";
                     }
                     instance.VehicleMaxWidth = records.GetDoubleNullable("WIDTH_MAX_MTR");
                     instance.ICAStatus = Convert.ToString(records.GetInt32OrDefault("ICA_STATUS"));
                     instance.Status = Convert.ToString(records.GetInt32OrDefault("ITEM_STATUS"));
                     instance.ReceivedDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("RECEIVED_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("RECEIVED_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                     instance.FromDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("START_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                     instance.ToDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("END_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("END_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                     instance.ESDALReference = records.GetStringOrDefault("ESDAL_REFERENCE");
                     int Notif_VerNo = 0;
                     var type = records.GetFieldType("NOTIFICATION_VERSION_NO");
                     if (type != null)   // check added for RM #10775
                     {
                         Notif_VerNo = records.GetInt16OrDefault("NOTIFICATION_VERSION_NO");
                     }
                     instance.MessageType = ConvertMessage(records.GetInt32OrDefault("ITEM_TYPE"), Notif_VerNo);
                     instance.StructName = records.GetStringOrDefault("NAME");
                     instance.NotificationId = records.GetLongOrDefault("NOTIFICATION_ID");
                     instance.InboxItemId = records.GetLongOrDefault("INBOX_ITEM_ID"); // Added by NetWeb for further use.
                     instance.StatuatoryPeriod = CheckStatuatory(instance.ReceivedDate, instance.FromDate);
                     instance.IsOpened = records.GetShortOrDefault("OPENED");
                     instance.IsUnopened = records.GetShortOrDefault("UNOPENED");
                     instance.IsWithdrawn = records.GetShortOrDefault("IS_WITHDRAWN");
                     instance.IsDeclined = records.GetShortOrDefault("IS_DECLINED");
                     instance.ImminentMovement = records.GetShortOrDefault("IS_IMMINENT");
                     instance.NENId = records.GetLongOrDefault("NEN_ID");
                     instance.InboxItemStatus = records.GetInt32OrDefault("ITEM_TYPE");
                     instance.UserId = Convert.ToString(records.GetLongOrDefault("USER_ID"));
                     instance.UserAssignId = Convert.ToString(records.GetLongOrDefault("ASSIGN_TO_USER"));
                 }
            );
            return movementInboxObj;
        }

        public static int ReturnRouteAutoAssignVehicle(long movementId, int flag, long notificationId, long organisationId)
        {
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal+".STP_MOVEMENT_RETURN_ROUTE.SP_RETURN_ROUTE_VEHICLE_ASSIGNMENT",
                parameter =>
                {

                    parameter.AddWithValue("P_MOVEMENT_ID", movementId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FLAG", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);

                },
                record =>
                {
                    count = record.GetInt32("P_AFFECTED_ROWS");
                }
            );
            return count;
        }
        public static DateTime CalcualteMovementDate(int noticePeriod, int vehicleClass, string userSchema)
        {
            DateTime dateTime = DateTime.Now;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                dateTime,
               UserSchema.Portal + ".SP_CALC_MOVMENT_DATE",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTICE_PERIOD", noticePeriod, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_CLASS", vehicleClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    dateTime = records.GetDateTimeOrDefault("MOVE_START_DATE");
                }
            );
            return dateTime;
        }

        public static List<ContactModel> GetNENAffectedContactDetails(string esdalRefNumber, string userSchema)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                contacts,
                userSchema + ".SP_GET_NEN_AFFECT_PARTIES",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDAL_REF", esdalRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    var type = records.GetFieldType("ORGANISATION_ID");
                    instance.ContactId = (int)records.GetLongOrDefault("CONTACT_ID");
                    instance.FullName = records.GetStringOrDefault("FULL_NAME");
                    instance.Email = records.GetStringOrDefault("EMAIL_ADDRESS");
                    instance.Organisation = records.GetStringOrDefault("ORG_NAME");
                    instance.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");
                });

            return contacts;
        }
    }
}