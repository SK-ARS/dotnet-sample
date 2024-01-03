using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;

namespace STP.Applications.Persistance
{
    public static class SORTApplicationDAO
    {


        #region READ
        /// <summary>
        /// ListSORTUser
        /// </summary>
        /// <param name="userTypeID"></param>
        /// <param name="checkerType"></param>
        /// <returns></returns>
        public static List<SORTUserList> ListSortUserAppl(long userTypeID, int checkerType = 0)
        {
            List<SORTUserList> sortUserList = new List<SORTUserList>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            sortUserList,
            UserSchema.Sort + ".LIST_SORT_USER",
            parameter =>
            {
                parameter.AddWithValue("p_USERTYPE_ID", userTypeID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_CHCEKING_TYPE", checkerType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("SORT_ORDER", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("PRESET_FILTER", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
             (records, instance) =>
             {
                 instance.UserID = records.GetLongOrDefault("user_id");
                 instance.UserName = records.GetStringOrDefault("SORT_USER");
                 instance.ContactID = records.GetLongOrDefault("contact_id");
             }

        );
            return sortUserList;
        }
        /// <summary>
        /// GetHaulierAppRevision
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static List<SORTMovementList> GetHaulierAppRevision(long projectID)
        {

            List<SORTMovementList> haulierApplicationRevisionList = new List<SORTMovementList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                haulierApplicationRevisionList,
                UserSchema.Sort + ".SP_GET_SORT_HAUL_APP_REV_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_PRODUCTID", projectID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RevisionNo = records.GetShortOrDefault("revision_no");
                        instance.RevisionID = records.GetLongOrDefault("REVISION_ID");
                        instance.VersionNo = records.GetLongOrDefault("move_ver_no");
                        instance.ApplicationDate = records.GetStringOrDefault("application_date");
                        instance.AppStatus = records.GetInt32OrDefault("APPLICATION_STATUS"); 
                        instance.ESDALReference = records.GetStringOrDefault("esdal_ref_no");
                    }
          );
            return haulierApplicationRevisionList;
        }
        /// <summary>
        /// GetMovmentVersion
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static List<SORTMovementList> GetMovmentVersion(long projectID)
        {
            List<SORTMovementList> movementVersionList = new List<SORTMovementList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementVersionList,
                UserSchema.Sort + ".SP_GET_SORT_MOVEMENT_VER_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_PRODUCTID", projectID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VersionNo = records.GetShortOrDefault("VERSION_NO");
                        instance.VersionID = records.GetLongOrDefault("VERSION_ID");
                        instance.RevisionNo = records.GetShortOrDefault("REVISION_NO");
                        instance.RevisionID = records.GetLongOrDefault("REVISION_ID");
                        instance.OrganisationID = (int)records.GetLongOrDefault("HAULIER_ORG_ID");
                        instance.CommittedDate = records.GetStringOrDefault("COMMITTED_DATE");
                        instance.DistributionDate = records.GetStringOrDefault("DISTRIBUTED_DATE");
                        instance.IsNotified = records.GetShortOrDefault("IS_NOTIFIED");
                        instance.ESDALReference = records.GetStringOrDefault("esdal_ref_no");
                        instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                    }
               );
            return movementVersionList;
        }
        /// <summary>
        /// GetCandidateRoutes
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static List<CandidateRTModel> GetCandidateRoutes(long projectID)
        {

            List<CandidateRTModel> candidateRouteList = new List<CandidateRTModel>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                candidateRouteList,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.GET_CANDIDATE_ROUTE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RouteID = records.GetLongOrDefault("ROUTE_ID");
                        instance.Name = records.GetStringOrDefault("ROUTE_NAME");
                        instance.AnalysisID = records.GetLongOrDefault("ANALYSIS_ID");
                        instance.RevisionID = records.GetLongOrDefault("REVISION_ID");
                        instance.RevisionNo = records.GetShortOrDefault("REVISION_NO");
                        instance.CandidateDate = records.GetStringOrDefault("ANALYSIS_DATE");
                    }
          );
            return candidateRouteList;
        }
        /// <summary>
        /// GetRelatedMovement
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SOApplicationRelatedMov GetRelatedMovement(long applicationID, string type)
        {
            SOApplicationRelatedMov soApplicationRelatedMov = new SOApplicationRelatedMov();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            soApplicationRelatedMov,
            UserSchema.Sort + ".SP_RELATED_MOVEMENTS",
            parameter =>
            {
                parameter.AddWithValue("P_APPLICATION_ID", applicationID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_TYPE", type, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
           (records, instance) =>
           {
               instance.ApplicationRevisionId = records.GetLongOrDefault("REVISION_ID");
               instance.ProjectID = records.GetLongOrDefault("PROJECT_ID");
               instance.VersionID = records.GetLongOrDefault("VERSION_ID");
               instance.VersionNo = records.GetShortOrDefault("VERSION_NO");
               instance.RevisionNo = records.GetShortOrDefault("REVISION_NO");
               instance.LastVersionNo = (int)records.GetDecimalOrDefault("LAST_VER_NO");
               instance.HaulierMnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
               instance.ReferenceNo = records.GetInt32OrDefault("ESDAL_REF_NUMBER");
               instance.EnteredBySORT = records.GetInt16OrDefault("ENTERED_BY_SORT");
               instance.OrganisationID = (int)records.GetLongOrDefault("HAULIER_ORG_ID");
               instance.OwnerName = records.GetStringOrDefault("OWNER_NAME");
               instance.ESDALReference = records.GetStringOrDefault("V_ESDAL_REF");
               if (type == ApplicationConstants.CandidateType)
               {
                   instance.CandidateAnalysisID = records.GetLongOrDefault("ANALYSIS_ID");
                   instance.CandidateRevisionID = records.GetLongOrDefault("CAND_REVISION_ID");
                   instance.CandidateRevisionNo = records.GetShortOrDefault("CAND_REVISION_NO");
                   instance.LastCandidateRevisionNo = (int)records.GetDecimalOrDefault("MAX_CAND_REV");
                   instance.CandidateRouteName = records.GetStringOrDefault("ROUTE_NAME");
                   instance.CandidateRouteID = records.GetLongOrDefault("ROUTE_ID");
               }
           }
           );
            return soApplicationRelatedMov;
        }
        /// <summary>
        /// CandidateRouteList
        /// </summary>
        /// <param name="routeRevisionID"></param>
        /// <param name="userSchema"></param>
        /// <param name="rListType"></param>
        /// <returns></returns>
        public static List<AppRouteList> CandidateRouteList(int routeRevisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC)
        {

            List<AppRouteList> appSORouteList = new List<AppRouteList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           appSORouteList,
               userSchema + ".STP_CANDIDATE_ROUTE_PKG.CANDIDATE_ROUTES_LIST",
           parameter =>
           {
               parameter.AddWithValue("CR_REVISION_ID", routeRevisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("RLIST_TYPE", rListType, OracleDbType.Char, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("CR_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   instance.RouteID = records.GetLongOrDefault("PART_ID");
                   instance.RouteName = records.GetStringOrDefault("PART_NAME");
                   instance.RouteDescription = records.GetStringOrDefault("DESCRIPTION");
                   instance.TransportMode = records.GetStringOrDefault("TRANSPORT_MODE");
                   instance.PartNo = records.GetInt16OrDefault("PART_NO");
                   instance.NewPartNo = records.GetInt16OrDefault("PART_NO");
                   instance.RouteType = records.GetStringOrDefault("flag");
                   instance.FromAddress = records.GetStringOrDefault("START_ADDRESS");
                   instance.ToAddress = records.GetStringOrDefault("END_ADDRESS");
               }
          );
            return appSORouteList;
        }
        /// <summary>
        /// CandidateVehicleDetails
        /// </summary>
        /// <param name="revisionID"></param>
        /// <param name="userSchema"></param>
        /// <param name="rListType"></param>
        /// <returns></returns>
        public static List<AppVehicleConfigList> CandidateVehicleDetails(int revisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT, char rListType = ApplicationConstants.RListTypeC)
        {

            List<AppVehicleConfigList> appVehicleConfigList = new List<AppVehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           appVehicleConfigList,
              userSchema + ".STP_CANDIDATE_ROUTE_PKG.SP_LIST_VEHICLES",
           parameter =>
           {
               parameter.AddWithValue("CR_REVISION_ID", revisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("RLIST_TYPE", rListType, OracleDbType.Char, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("CR_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   instance.VehicleId = records.GetLongOrDefault("vehicle_id");
                   instance.VehicleName = records.GetStringOrDefault("vehicle_name");
                   instance.RoutePartId = (int)records.GetLongOrDefault("ROUTE_PART_ID");
                   instance.RoutePart = records.GetStringOrDefault("ROUTE_PART_NAME");
               }
          );
            return appVehicleConfigList;
        }
        /// <summary>
        /// CheckCandIsModified
        /// </summary>
        /// <param name="analysisID"></param>
        /// <returns></returns>
        public static int CheckCandIsModified(int analysisID)
        {

            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Sort + ".CHECK_CAND_IS_MODIFY",
                parameter =>
                {
                    parameter.AddWithValue("P_ANALYSIS_ID", analysisID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = (int)records.GetShortOrDefault("IS_MODIFIED");
                }
          );
            return result;
        }
        /// <summary>
        /// GetCandRouteVehicleDetails
        /// </summary>
        /// <param name="routeRevisionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static bool GetCandRouteVehicleDetails(long routeRevisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {

            bool result = true;
            List<CandRouteVehicle> candidateRouteVehicleList = new List<CandRouteVehicle>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            candidateRouteVehicleList,
              userSchema + ".STP_CANDIDATE_ROUTE_PKG.SP_GET_RT_VEH_COUNT",
           parameter =>
           {
               parameter.AddWithValue("R_REVISION_ID", routeRevisionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("R_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   instance.RouteID = records.GetLongOrDefault("ROUTE_PART_ID");
                   instance.VehicleCount = records.GetDecimalOrDefault("VEH_COUNT");
               }
          );
            if (candidateRouteVehicleList.Count != 0)
                result = candidateRouteVehicleList.Exists(c => c.VehicleCount == 0);
            return result;
        }
        /// <summary>
        /// GetSpecialOrderList
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static List<SORTMovementList> GetSpecialOrderList(long projectID)
        {

            List<SORTMovementList> sortMovementList = new List<SORTMovementList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                sortMovementList,
                UserSchema.Sort + ".SP_GET_SORT_SPECIAL_ORD_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_PRODUCTID", projectID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.OrderNumber = records.GetStringOrDefault("ORDER_NO");
                        instance.ExpiryDate = records.GetStringOrDefault("EXPIRY_DATE");
                        instance.SOCreateDate = records.GetDateTimeOrDefault("CREATED_DATE");
                    }
          );
            return sortMovementList;
        }
        /// <summary>
        /// GetCandidateRouteNM
        /// </summary>
        /// <param name="candidateRouteID"></param>
        /// <returns></returns>
        public static string GetCandidateRouteNM(long candidateRouteID)
        {

            string name = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                name,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.GET_CANDIDATE_ROUTE_NM",
                parameter =>
                {
                    parameter.AddWithValue("CR_ROUTEID", candidateRouteID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        name = records.GetStringOrDefault("ROUTE_NAME");
                    }
          );
            return name;
        }
        /// <summary>
        /// GetVR1AppDate
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static string GetVR1AppDate(long projectID)
        {

            string result = null;
            string vr1Number = null;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Sort + ".SP_GET_VR1_APPROVAL_DATE",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        result = record.GetStringOrDefault("APPROVAL_DATE");
                        vr1Number = Convert.ToString(record.GetDecimalOrDefault("V_VR1_NUMBER"));
                    }
          );
            return result != null && vr1Number != null ? result + "," + vr1Number : "False";
        }
        /// <summary>
        /// GetRevIDFromApplication
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static long GetRevIDFromApplication(long projectID)
        {

            long revID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                revID,
                UserSchema.Sort + ".SP_CHECK_MAX_APP_REV_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        revID = Convert.ToInt64(record.GetDecimalOrDefault("Rev_ID"));
                    }
          );
            return revID;
        }
        /// <summary>
        /// GetMovementHistory
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="haulierNumber"></param>
        /// <param name="esdalReference"></param>
        /// <param name="versionNumber"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static List<MovementHistory> GetMovementHistory(int pageNumber, int pageSize, string haulierNumber, int esdalReference, int versionNumber, long projectID, int? sortOrder = null, int? sortType = null)
        {


            List<MovementHistory> movementHistoryList = new List<MovementHistory>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementHistoryList,
                UserSchema.Sort + ".SP_GET_SORT_HISTORY",
                parameter =>
                {
                    parameter.AddWithValue("P_HAUL_NUM", haulierNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("P_ESDAL_REF", esdalReference, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("P_VERSION_NO", versionNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PROJECT_ID", projectID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SORT_ORDER",sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER",sortType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {

                        instance.LatestRevisionNumber = records.GetShortOrDefault("REVISION_NO");
                        instance.NotificationVersionNumber =  Convert.ToInt32(records.GetShortOrDefault("VERSION_NO"));
                        instance.NotificationDate = records.GetStringOrDefault("Mov_DATE");
                        instance.Description = records.GetStringOrDefault("DESCRIPTION");
                        instance.ActionType = records.GetStringOrDefault("ACTION_TYPE");
                        instance.TotalCount = Convert.ToInt32(records.GetDecimalOrDefault("TOTAL_ROWS"));
                    }
            );
            return movementHistoryList;
        }
        /// <summary>
        /// GetMovHaulierNotes
        /// </summary>
        /// <param name="movementVersionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        internal static Byte[] GetMovHaulierNotes(long movementVersionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {


            Byte[] result = null;
            decimal isCompressedFile = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_GET_NOTES_FOR_HAULIER",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", movementVersionID, OracleDbType.Long, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        result = records.GetByteArrayOrNull("NOTES_FOR_HAULIER");
                        isCompressedFile = records.GetDecimalOrDefault("ISCOMPRESSED");
                    }
          );
            return result;
        }
        /// <summary>
        /// GetSortProjectDetails
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static SORTLatestAppDetails GetSortProjectDetails(long projectID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {

            SORTLatestAppDetails sortLatestAppDetails = new SORTLatestAppDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sortLatestAppDetails,
                userSchema + ".SP_GET_SORT_PROJECT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("S_PROJECT_ID", projectID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("S_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                        instance.ApplicationRevisionId = long.Parse(records.GetDecimalOrDefault("REVISION_ID").ToString());
                        instance.ApplicationRevisionNo = int.Parse(records.GetShortOrDefault("REVISION_NO").ToString());
                        instance.VersionId = long.Parse(records.GetDecimalOrDefault("VERSION_ID").ToString());
                        instance.VersionNo = int.Parse(records.GetDecimalOrDefault("VERSION_NO").ToString());
                        instance.MovIsDistributed = records.GetDecimalOrDefault("IS_DISTRIBUTED");
                    }
          );
            return sortLatestAppDetails;
        }
        /// <summary>
        /// GetCandRouteVehicleAssignDetails
        /// </summary>
        /// <param name="routeRevisionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static bool GetCandRouteVehicleAssignDetails(long routeRevisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {
            bool result = true;

            List<CandRouteVehicle> candidateRouteVehicleList = new List<CandRouteVehicle>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            candidateRouteVehicleList,
              userSchema + ".STP_CANDIDATE_ROUTE_PKG.SP_CHK_RT_ASGN_VEH_CNT",
           parameter =>
           {
               parameter.AddWithValue("R_REVISION_ID", routeRevisionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("R_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   instance.RouteID = records.GetLongOrDefault("ROUTE_PART_ID");
                   instance.VehicleCount = records.GetDecimalOrDefault("VEH_COUNT");
               }
          );
            if (candidateRouteVehicleList.Count != 0)
            {
                result = candidateRouteVehicleList.Exists(c => c.VehicleCount == 1);
            }
            return result;
        }
        /// <summary>
        /// GetRouteType
        /// </summary>
        /// <param name="revisionID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static List<CRDetails> GetRouteType(int revisionID, string userSchema = ApplicationConstants.DbUserSchemaSTPSORT)
        {
            List<CRDetails> routeDetailsList = new List<CRDetails>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeDetailsList,
                userSchema + ".STP_CANDIDATE_ROUTE_PKG.SP_GET_ROUTE_TYPES",
                parameter =>
                {
                    parameter.AddWithValue("R_REVISION_ID", revisionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("R_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RoutePartID = records.GetLongOrDefault("ROUTE_PART_ID");
                        instance.RouteName = records.GetStringOrDefault("R_NAME");
                        instance.SegmentNumber = records.GetDecimalOrDefault("S_NUMBER");
                    }
          );
            return routeDetailsList;
        }
        /// <summary>
        /// GetProjOverviewDetails
        /// </summary>
        /// <param name="revisionID"></param>
        /// <returns></returns>
        public static SOApplication GetProjOverviewDetails(long revisionID)
        {
            SOApplication soApplication = new SOApplication();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                soApplication,
                UserSchema.Sort + ".SP_SORT_PROJ_OVER_DET",
                parameter =>
                {
                    parameter.AddWithValue("p_REV_ID", revisionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.HaulierReference = records.GetStringOrDefault("HAULIERS_REF");
                        instance.ProjectStatus = records.GetStringOrDefault("PROJECT_STATUS");
                        instance.CheckingStatus = records.GetStringOrDefault("CHECKING_STATUS");
                        instance.ApplicationDate = records.GetStringOrDefault("APPLICATION_DATE");
                        instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                        instance.FromAddress = records.GetStringOrDefault("FROM_DESCR");
                        instance.ToAddress = records.GetStringOrDefault("TO_DESCR");
                        instance.Load = records.GetStringOrDefault("LOAD_DESCR");
                        instance.HAJobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");
                        instance.MovementName = records.GetStringOrDefault("project_name");
                        string date = records.GetStringOrDefault("DUE_DATE");
                        instance.ApplicationDueDate = date.Length == 0 ? null : date;
                        instance.PlannerUserId = records.GetLongOrDefault("PLANNER_USERID");
                        instance.ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                        instance.CheckerName = records.GetStringOrDefault("checkername");
                    }
          );
            return soApplication;
        }
        /// <summary>
        /// GetSORTNotifiCode
        /// </summary>
        /// <param name="revisionID"></param>
        /// <returns></returns>
        public static string GetSORTNotifiCode(int revisionID)
        {
            string response = string.Empty;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                response,
                UserSchema.Sort + ".SP_GET_SORT_NOTIF_CODE",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", revisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        response = records.GetStringOrDefault("NOTIF_CODE");
                    }
          );
            return response;
        }
        /// <summary>
        /// CheckVehicleOnRoute
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="revisionID"></param>
        /// <returns></returns>
        internal static bool CheckVehicleOnRoute(int projectID, int revisionID)
        {
            bool result = false;

            decimal response = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                response,
                UserSchema.Sort + ".SP_CHK_VEH_ASSIGN_TO_ROUTE",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJ_ID", projectID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REV_ID", revisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        response = records.GetDecimalOrDefault("RES");
                    }
          );
            if (response > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion READ

        #region WRITE
        /// <summary>
        /// SORTAppWithdrawandDecline
        /// </summary>
        /// <param name="sortApplnWithdrawandDeclineParams"></param>
        /// <returns></returns>
        public static int SORTAppWithdrawandDecline(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Sort + ".SP_SORT_WITHDRAW_DECLINE_APP",
                parameter =>
                {
                    parameter.AddWithValue("p_PROJECT_ID", sortApplnWithdrawandDeclineParams.ProjectId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Flag", sortApplnWithdrawandDeclineParams.Flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetDecimalOrDefault("CNT"));
                 }
            );
            return result;
        }
        /// <summary>
        /// SORTUnwithdraw
        /// </summary>
        /// <param name="sortApplnWithdrawandDeclineParams"></param>
        /// <returns></returns>
        public static int SORTUnwithdraw(SORTAppWithdrawAndDeclineParams sortApplnWithdrawandDeclineParams)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Sort + ".SP_SORT_UNWITHDRAW_APP",
                parameter =>
                {
                    parameter.AddWithValue("p_PROJECT_ID", sortApplnWithdrawandDeclineParams.ProjectId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetDecimalOrDefault("CNT"));
                 }
            );
            return result;
        }

        #region SubmitSORTSOApplication
        public static SOApplication SubmitSORTSOApplication(SORTSOApplicationParams sortSOApplicationParams)
        {
            SOApplication sOApplication = new SOApplication();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sOApplication,
                UserSchema.Sort + ".SP_SUBMIT_SORT_SPEC_ORDER",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", sortSOApplicationParams.ApplicationRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EDIT_FLAG", sortSOApplicationParams.EditFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     sOApplication.HaulierMneu = record.GetStringOrDefault("V_HAULIER_MNEMONIC");
                     sOApplication.EsdalRefNo = (int)record.GetDecimalOrDefault("V_ESDAL_REF");
                     sOApplication.ESDALReference = record.GetStringOrDefault("ESDAL_REF_NO");
                     sOApplication.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                     sOApplication.LastRevisionNo = record.GetInt16OrDefault("REVISION_NO");
                     sOApplication.VersionNo= (int)record.GetDecimalOrDefault("VERSION_NO");
                 }
            );
            return sOApplication;
        }
        #endregion

        #region SubmitSORTVR1Application
        public static ApplyForVR1 SubmitSORTVR1Application(SubmitSORTVR1Params submitSORTParams)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                applyForVR1,
                UserSchema.Sort + ".SP_SUBMIT_SORT_VR1",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", submitSORTParams.ApplicationRevisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                  record =>
                  {
                      applyForVR1.HaulierMnemonic = record.GetStringOrDefault("V_HAULIER_MNEMONIC");
                      applyForVR1.EsdalRefNo = (int)record.GetDecimalOrDefault("V_ESDAL_REF");
                      applyForVR1.ESDALReference = record.GetStringOrDefault("ESDAL_REF_NO");
                      applyForVR1.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                      applyForVR1.RevisionNumber = record.GetInt16OrDefault("REVISION_NO");
                      applyForVR1.VersionNumber = (int)record.GetDecimalOrDefault("VERSION_NO");
                  }
             );
            return applyForVR1;
        }
        #endregion

        #region Save Sort App
        public static SOApplication SaveSOApplication(SOApplication soApplication)
        {

            DateTime fromDate = Convert.ToDateTime(soApplication.MovementDateFrom);
            DateTime toDate = Convert.ToDateTime(soApplication.MovementDateTo);
            DateTime appDate = Convert.ToDateTime(soApplication.ApplicationDate);
            DateTime appDueDate = Convert.ToDateTime(soApplication.ApplicationDueDate);

            decimal revisionID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                revisionID,
                UserSchema.Sort + ".SP_INSERT_SORT_SPEC_ORDER",
                parameter =>
                {
                    parameter.AddWithValue("APP_REVID", soApplication.ApplicationRevId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_ORG", soApplication.OrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("L_DESC ", soApplication.Load, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("F_DESCR", soApplication.FromAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("T_DESCR ", soApplication.ToAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CLIENT", soApplication.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("DESCR", soApplication.HaulierDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("APP_START", TimeZoneInfo.ConvertTimeToUtc(appDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("APP_DUE", TimeZoneInfo.ConvertTimeToUtc(appDueDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_START", TimeZoneInfo.ConvertTimeToUtc(fromDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_END", TimeZoneInfo.ConvertTimeToUtc(toDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES", soApplication.ApplicationNotesFromHA, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("NO_OF_MOV", soApplication.NumberOfMovements, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("No_OF_PIECES", soApplication.NumberofPieces, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REF", soApplication.HaulierReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES_ON_ESCORT", soApplication.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("AGENTNAME", soApplication.AgentName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ORG_HAULIER_CONTACT_NAME", soApplication.OrgHaulierContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ORG_EMAIL", soApplication.OrgEmailId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ORG_FAX", soApplication.OrgFax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PLANNER_USER_ID", soApplication.AllocateTo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SAVEALLOCATEFLAG", soApplication.SaveAllocateFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EditBySORT", soApplication.EditBySORT, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ContactId", soApplication.HAContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     soApplication.ApplicationRevId = Convert.ToInt64(record.GetDecimalOrDefault("REVISION_ID"));
                     soApplication.ApplicationStatus = record.GetInt32OrDefault("APPLICATION_STATUS");
                     soApplication.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                 }
            );
            return soApplication;
        }
        public static ApplyForVR1 SaveSORTVR1Application(SORTVR1ApplicationInsertParams sortVR1ApplicationInsertParams)
        {

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sortVR1ApplicationInsertParams.VR1Application,
                UserSchema.Sort + ".SP_INSERT_SORT_VR1_APPL",
                parameter =>
                {

                    parameter.AddWithValue("APP_REVID", sortVR1ApplicationInsertParams.ApplicationRevisionId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_ORG", sortVR1ApplicationInsertParams.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("L_DESC ", sortVR1ApplicationInsertParams.VR1Application.LoadDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("F_DESCR", sortVR1ApplicationInsertParams.VR1Application.FromSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("T_DESCR ", sortVR1ApplicationInsertParams.VR1Application.ToSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CLIENT", sortVR1ApplicationInsertParams.VR1Application.ClientName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("DESCR", sortVR1ApplicationInsertParams.VR1Application.DescriptionWithApplication, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_START", sortVR1ApplicationInsertParams.VR1Application.MovementDateFrom, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_END", sortVR1ApplicationInsertParams.VR1Application.MovementDateTo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES", sortVR1ApplicationInsertParams.VR1Application.ApplicationNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("NO_OF_MOV", sortVR1ApplicationInsertParams.VR1Application.NoOfMovements, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("No_OF_PIECES", sortVR1ApplicationInsertParams.VR1Application.MaxPiecesPerLoad, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REF", sortVR1ApplicationInsertParams.VR1Application.MyReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("USERID", sortVR1ApplicationInsertParams.UserId, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES_ON_ESCORT", sortVR1ApplicationInsertParams.VR1Application.ApplicationNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("AGENTNAME", sortVR1ApplicationInsertParams.VR1Application.ContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("VEHICLE_CLASSIFICATION", sortVR1ApplicationInsertParams.VR1Application.SubMovementClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("VEHICLE_DESC", sortVR1ApplicationInsertParams.VR1Application.VehicleDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("REDUCED_DETAILS", 1, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("HAULIER_CONT", sortVR1ApplicationInsertParams.VR1Application.ContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_NAME", sortVR1ApplicationInsertParams.VR1Application.HaulierOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_ADD_1", sortVR1ApplicationInsertParams.VR1Application.HaulierAddress1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_ADD_2", sortVR1ApplicationInsertParams.VR1Application.HaulierAddress2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_ADD_3", sortVR1ApplicationInsertParams.VR1Application.HaulierAddress3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_ADD_4", sortVR1ApplicationInsertParams.VR1Application.HaulierAddress4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_ADD_5", sortVR1ApplicationInsertParams.VR1Application.HaulierAddress5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_POST", sortVR1ApplicationInsertParams.VR1Application.OnBehalfOfPostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_COUNTRY", sortVR1ApplicationInsertParams.VR1Application.CountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_TEL", sortVR1ApplicationInsertParams.VR1Application.HaulierTelephoneNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_EMAIL", sortVR1ApplicationInsertParams.VR1Application.OnBehalfOfEmailId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAUL_FAX_NO", sortVR1ApplicationInsertParams.VR1Application.HaulierFaxNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("HAULIER_LICENCE_NO", sortVR1ApplicationInsertParams.VR1Application.HaulierOperatorLicence, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("Applicant_CONT", sortVR1ApplicationInsertParams.VR1Application.ApplicantContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_NAME", sortVR1ApplicationInsertParams.VR1Application.ApplicantName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_ADD_1", sortVR1ApplicationInsertParams.VR1Application.ApplicantAddress1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_ADD_2", sortVR1ApplicationInsertParams.VR1Application.ApplicantAddress2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_ADD_3", sortVR1ApplicationInsertParams.VR1Application.ApplicantAddress3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_ADD_4", sortVR1ApplicationInsertParams.VR1Application.ApplicantAddress4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_ADD_5", sortVR1ApplicationInsertParams.VR1Application.ApplicantAddress5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_POST", sortVR1ApplicationInsertParams.VR1Application.ApplicationPostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_COUNTRY", sortVR1ApplicationInsertParams.VR1Application.ApplicantCountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_TEL", sortVR1ApplicationInsertParams.VR1Application.ApplicantTelephoneNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_EMAIL", sortVR1ApplicationInsertParams.VR1Application.ApplicantEmailId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_FAX_NO", sortVR1ApplicationInsertParams.VR1Application.ApplicantFaxNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("Applicant_LICENCE_NO", sortVR1ApplicationInsertParams.VR1Application.ApplicantOperatorLicence, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("WIDTH_MAX_MTR", sortVR1ApplicationInsertParams.VR1Application.OverallWidth, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("LEN_MAX_MTR", sortVR1ApplicationInsertParams.VR1Application.OverallLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MAX_HEIGHT_MAX_MTR", sortVR1ApplicationInsertParams.VR1Application.OverallHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("GROSS_WEIGHT_MAX_KG", sortVR1ApplicationInsertParams.VR1Application.GrossWeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_PLANNER_USER_ID", sortVR1ApplicationInsertParams.VR1Application.AllocateTo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SAVEALLOCATEFLAG", sortVR1ApplicationInsertParams.VR1Application.SaveAllocateFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_EditBySORT", sortVR1ApplicationInsertParams.VR1Application.EditBySORT, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     sortVR1ApplicationInsertParams.VR1Application.ApplicationRevisionId = Convert.ToInt64(record.GetDecimalOrDefault("REVISION_ID"));
                     sortVR1ApplicationInsertParams.VR1Application.AnalysisId = (int)record.GetDecimalOrDefault("ANALYSIS_ID");
                     sortVR1ApplicationInsertParams.VR1Application.VR1ContentRefNo = record.GetStringOrDefault("CONTENT_REF_NO");
                     sortVR1ApplicationInsertParams.VR1Application.VersionId = (int)record.GetDecimalOrDefault("VERSION_ID");
                 }

            );
            return sortVR1ApplicationInsertParams.VR1Application;
        }

        #endregion

        /// <summary>
        /// InsertCandidateRoute
        /// </summary>
        /// <param name="candidateRouteSaveParams"></param>
        /// <returns></returns>
        public static CandidateRouteInsertResponse InsertCandidateRoute(CandidateRouteInsertParams candidateRouteSaveParams)
        {

            long candidateRouteID = 0;
            long candidateAnalysisID = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                candidateRouteID,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.INSERT_CANDIDATE_ROUTE",
                parameter =>
                {
                    parameter.AddWithValue("CR_NAME", candidateRouteSaveParams.CandidateName, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_PROJECTID", candidateRouteSaveParams.ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        candidateRouteID = records.GetLongOrDefault("ROUTE_ID");
                        candidateAnalysisID = records.GetLongOrDefault("ANALYSIS_ID");
                    }
          );

            CandidateRouteInsertResponse routeDetails = new CandidateRouteInsertResponse()
            {
                RouteId = candidateRouteID,
                AnalysisId = candidateAnalysisID
            };
            return routeDetails;
        }
        
        /// <summary>
        /// SaveSORTMovProjDetail
        /// </summary>
        /// <param name="sortMvmtProjectDetailsInsertParams"></param>
        /// <returns></returns>
        public static int SaveSORTMovProjDetail(SORTMvmtProjectDetailsInsertParams sortMvmtProjectDetailsInsertParams)
        {

            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Sort + ".SP_SORT_UPDATE_MOV_PROJ",
                parameter =>
                {
                    parameter.AddWithValue("p_PROJECT_ID", sortMvmtProjectDetailsInsertParams.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PROJECT_NAME", sortMvmtProjectDetailsInsertParams.MovementName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_HA_JOB_REF", sortMvmtProjectDetailsInsertParams.HaulierJobReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FROM_ADD", sortMvmtProjectDetailsInsertParams.FromAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TO_ADD", sortMvmtProjectDetailsInsertParams.ToAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LOAD", sortMvmtProjectDetailsInsertParams.Load, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                  record =>
                  {
                      result = (int)record.GetDecimalOrDefault("PROJ_CNT");
                  }

             );
            return result;
        }
        
        /// <summary>
        /// InsertMovHaulierNotes
        /// </summary>
        /// <param name="haulierMovNotesInsertParams"></param>
        /// <returns></returns>
        internal static bool InsertMovHaulierNotes(HaulierMovNotesInsertParams haulierMovNotesInsertParams)
        {
            bool result = false;
            decimal response = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                response,
                haulierMovNotesInsertParams.UserSchema + ".SP_SAVE_NOTES_FOR_HAULIER",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", haulierMovNotesInsertParams.MovementVersionID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTES_FOR_HAUL", haulierMovNotesInsertParams.HaulierNote, OracleDbType.Blob, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        response = records.GetDecimalOrDefault("VAR_IS_UPDATED");
                    }
          );
            if (response == 1)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// ReviseVR1Application
        /// </summary>
        /// <param name="reviseVR1Params"></param>
        /// <returns></returns>
        public static ApplyForVR1 ReviseVR1Application(ReviseVR1Params reviseVR1Params)
        {
            ApplyForVR1 obj = new ApplyForVR1();
            long revisionID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                revisionID,
                reviseVR1Params.UserSchema + ".SP_VR1_APPL_REVISE",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", reviseVR1Params.ApplicationRevisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     //revisionID = record.GetLongOrDefault("REVISION_ID");
                     obj.ApplicationRevisionId = record.GetLongOrDefault("REVISION_ID");
                     obj.VersionId = (int)record.GetDecimalOrDefault("VERSION_ID");
                     obj.SubMovementClass = record.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                     obj.MovementId = (long)record.GetDecimalOrDefault("MOVEMENT_ID");
                     obj.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                     obj.RevisionNumber= record.GetInt16OrDefault("REVISION_NO");
                 }
            );
            return obj;
        }
        /// <summary>
        /// UpdateCandidateRouteNM
        /// </summary>
        /// <param name="updateCandidateRouteNMInsertParams"></param>
        /// <returns></returns>
        public static long UpdateCandidateRouteNM(UpdateCandidateRouteNMInsertParams updateCandidateRouteNMInsertParams)
        {

            long candidateRouteID = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                candidateRouteID,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.UPDATE_CANDIDATE_ROUTE_NM",
                parameter =>
                {
                    parameter.AddWithValue("CR_NAME", updateCandidateRouteNMInsertParams.Name, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_PROJECTID", updateCandidateRouteNMInsertParams.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_ROUTEID", updateCandidateRouteNMInsertParams.CandidateRouteId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        candidateRouteID = records.GetLongOrDefault("ROUTE_ID");
                    }
          );

            return candidateRouteID;
        }
        /// <summary>
        /// UpdateCandIsModified
        /// </summary>
        /// <param name="analysisID"></param>
        public static void UpdateCandIsModified(int analysisID)
        {

            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Sort + ".UPDATE_CAND_IS_MODIFY",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ANALYSIS_ID", analysisID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    },
                    records =>
                    {
                    }
              );
        }
        /// <summary>
        /// UpdateProjectDetails
        /// </summary>
        /// <param name="updateProjectDetailsInsertParams"></param>
        /// <returns></returns>
        public static decimal UpdateProjectDetails(UpdateProjectDetailsInsertParams updateProjectDetailsInsertParams)
        {

            decimal result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
              updateProjectDetailsInsertParams.UserSchema + ".SP_UPDATE_PROJECT_DETAILS",
           parameter =>
           {
               parameter.AddWithValue("P_PROJECTID", updateProjectDetailsInsertParams.ProjectId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("P_SO_APP", updateProjectDetailsInsertParams.IsSO, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   result = records.GetDecimalOrDefault("STATUS");
               }
          );
            return result;
        }
        /// <summary>
        /// UpdateCollaborationView
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public static bool UpdateCollaborationView(int documentID)
        {

            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Sort + ".UPDATE_COLLAB_VIEW",
                    parameter =>
                    {
                        parameter.AddWithValue("P_DOCUMENT_ID", documentID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        records =>
                        {
                            result = true;
                        }
              );
            return result;
        }
        /// <summary>
        /// UpdateSpecialOrder
        /// </summary>
        /// <param name="updateSpecialOrderInsertParams"></param>
        /// <returns></returns>
        public static int UpdateSpecialOrder(UpdateSpecialOrderInsertParams updateSpecialOrderInsertParams)
        {

            int response = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                response,
                UserSchema.Sort + ".UPDATE_SPEC_ORDER",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", updateSpecialOrderInsertParams.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PROJECT_ID", updateSpecialOrderInsertParams.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ESDAL_REF", updateSpecialOrderInsertParams.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        response = Convert.ToInt32(records.GetDecimalOrDefault("V_FLAG"));
                    }
          );
            return response;
        }
        /// <summary>
        /// CloneApplicationRTParts
        /// </summary>
        /// <param name="cloneApplicationRTPartsInsertParams"></param>
        public static void CloneApplicationRTParts(CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams)
        {

            bool status = false;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                status,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.CLONE_APPROUTE_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("CA_OLD_REVISION_ID", cloneApplicationRTPartsInsertParams.OldRevisionID, OracleDbType.Long, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CA_ROUTE_REVISION_ID", cloneApplicationRTPartsInsertParams.RTRevisionID, OracleDbType.Long, ParameterDirectionWrap.Input);
                },
                    records =>
                    {
                    }
          );

        }
        /// <summary>
        /// InsertMovementVersion
        /// </summary>
        /// <param name="insertMovementVersionInsertParams"></param>
        /// <returns></returns>
        public static object InsertMovementVersion(InsertMovementVersionInsertParams insertMovementVersionInsertParams)
        {

            long movmentVersionID = 0;
            long movementAnalysisID = 0;
            int versionNo = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                movmentVersionID,
                insertMovementVersionInsertParams.UserSchema + ".STP_CANDIDATE_ROUTE_PKG.SP_INSERT_MOVEMENT_VERSION",
                parameter =>
                {
                    parameter.AddWithValue("CR_PROJECTID", insertMovementVersionInsertParams.ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_APP_REF", insertMovementVersionInsertParams.ApplicationReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("Cr_From_Revious", insertMovementVersionInsertParams.FromPrevious, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        movmentVersionID = records.GetLongOrDefault("VERSION_ID");
                        movementAnalysisID = records.GetLongOrDefault("ANALYSIS_ID");
                        versionNo = records.GetInt16OrDefault("VERSION_NO");
                    }
                );
            object newMovement = new { MovVersionId = movmentVersionID, MovAnalysisId = movementAnalysisID, VerNo=versionNo };
            return newMovement;
        }
        /// <summary>
        /// InsertRouteRevision
        /// </summary>
        /// <param name="routeRevisionInsertParams"></param>
        /// <returns></returns>
        public static RouteRevision InsertRouteRevision(RouteRevisionInsertParams routeRevisionInsertParams)
        {
            RouteRevision routerevision = new RouteRevision();


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routerevision,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.INSERT_ROUTE_REVISION",
                parameter =>
                {
                    parameter.AddWithValue("RR_ROUTEID", routeRevisionInsertParams.RouteID, OracleDbType.Long, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("RR_ROUTE_TYPE", routeRevisionInsertParams.RouteType, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("RR_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        routerevision.RevisionId = records.GetLongOrDefault("REVISION_ID");
                        routerevision.RevisionNumber = records.GetShortOrDefault("REVISION_NO");
                        routerevision.NewAnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                    }
                );
            return routerevision;
        }
        /// <summary>
        /// UpdateCheckerDetails
        /// </summary>
        /// <param name="updateCheckerDetailsInsertParams"></param>
        /// <returns></returns>
        public static bool UpdateCheckerDetails(UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams)
        {
            bool result = false;


            decimal response = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.CHECKER_ALLOCATION",

                parameter =>
                {
                    parameter.AddWithValue("CA_PROJECT_ID", updateCheckerDetailsInsertParams.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CA_CHECKER_ID", updateCheckerDetailsInsertParams.CheckerId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CA_CHECKER_STATUS", updateCheckerDetailsInsertParams.CheckerStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CA_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     response = record.GetDecimalOrDefault("CA_STATUS");
                 }
            );
            result = response == 1 ? true : false;
            return result;
        }
        /// <summary>
        /// CloneRTParts
        /// </summary>
        /// <param name="cloneRTPartsInsertParams"></param>
        public static void CloneRTParts(CloneRTPartsInsertParams cloneRTPartsInsertParams)
        {
            bool status = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                status,
                UserSchema.Sort + ".STP_CANDIDATE_ROUTE_PKG.CLONE_RTPARTS_RTVEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("CR_OLD_REV_ID", cloneRTPartsInsertParams.OldRevisionID, OracleDbType.Long, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_ROUTE_REV_ID", cloneRTPartsInsertParams.RtRevisionID, OracleDbType.Long, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("CR_FLAG", cloneRTPartsInsertParams.Flag, OracleDbType.Decimal, ParameterDirectionWrap.Input);
                },
                    records =>
                    {
                    }
          );

        }
        /// <summary>
        /// SpecialOrderUpdation
        /// </summary>
        /// <param name="specialOrderUpdationInsertParams"></param>
        /// <returns></returns>
        public static bool SpecialOrderUpdation(SpecialOrderUpdationInsertParams specialOrderUpdationInsertParams)
        {
            bool status = false;
            decimal response = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                status,
                UserSchema.Sort + ".SP_UPDATE_SO_STATE",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", specialOrderUpdationInsertParams.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOV_VER_NO", specialOrderUpdationInsertParams.MovmentVersionNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STATUS", specialOrderUpdationInsertParams.Status, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        response = records.GetDecimalOrDefault("V_FLAG");
                    }
          );
            if (response == 1)
            {
                status = true;
            }
            return status;
        }
        #endregion WRITE

        #region DELETE
        /// <summary>
        /// For deleting quick links.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static int DeleteQuickLinks(long projectID)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".SP_DELETE_FROM_QUICK_LINKS",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                  record =>
                  {
                      result=record.GetInt32("P_AFFECTED_ROWS");
                      //result = Convert.ToInt32("p_AFFECTED_ROWS");
                  }

             );
            return result;
        }
        #endregion DELETE

        #region InserResponseMessageNotes
        internal static bool InsertResponseMessageNotes(long movementVersionId, int autoResponseId, long organisationId, byte[] haulierNote, string responsePdf)
        {
            bool result = false;
            decimal res = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                res,
                 "STP_USER_PREFERENCES.SP_SAVE_RESPONSE_MESSAGE",
                parameter =>
                {
                    parameter.AddWithValue("P_USER_ID", movementVersionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ENABLE_AUTO_RESPONSE", autoResponseId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REPLY_MAIL_TEXT", haulierNote, OracleDbType.Blob, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REPLY_MAIL_PDF", responsePdf, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        res = records.GetDecimalOrDefault("VAR_IS_UPDATED");
                    }
          );
            if (res > 0)
                result = true;
            return result;
        }
        #endregion

        #region GetSORTSOHaulApplDetails(revisionId)
        public static SOHaulierApplication GetSORTSOHaulApplDetails(long revisionId)
        {
            SOHaulierApplication sOApplicationObj = new SOHaulierApplication();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sOApplicationObj,
                   UserSchema.Sort + ".SP_SORT_HAULIER_APP_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_RevisionID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RevisionId = records.GetLongOrDefault("REVISION_ID");
                        instance.HaulierESDALReference = records.GetStringOrDefault("ESDAL_REF");
                        instance.HaulierReference = records.GetStringOrDefault("HAULIERS_REF");
                        instance.HaulierFromSummary = records.GetStringOrDefault("FROM_DESCR");
                        instance.HaulierToSummary = records.GetStringOrDefault("TO_DESCR");
                        instance.HaulierClientName = records.GetStringOrDefault("CLIENT_DESCR");
                        instance.HaulierDescription = records.GetStringOrDefault("APPLICATION_NAME");
                        instance.HaulierLoad = records.GetStringOrDefault("LOAD_DESCR");
                        instance.HaulierApplicationDate = records.GetDateTimeOrDefault("APPLICATION_DATE");
                        instance.HaulierMovementDateFrom = records.GetDateTimeOrDefault("MOVEMENT_START_DATE");
                        instance.HaulierMovementDateTo = records.GetDateTimeOrDefault("MOVEMENT_END_DATE");
                        instance.HaulierContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                        instance.HaulierApplicantName = records.GetStringOrDefault("HAULIER_NAME");
                        instance.HaulierApplicantAddress1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                        instance.HaulierApplicantAddress2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                        instance.HaulierApplicantAddress3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                        instance.HaulierApplicantAddress4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                        instance.HaulierApplicantAddress5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                        instance.HaulierPostCode = records.GetStringOrDefault("HAULIER_POST_CODE");
                        instance.HaulierCountry = records.GetStringOrDefault("HAULIER_COUNTRY");
                        instance.HaulierTelephone = records.GetStringOrDefault("HAULIER_TEL_NO");
                        instance.HaulierFaxNumber = records.GetStringOrDefault("HAULIER_FAX_NO");
                        instance.HaulierEmailId = records.GetStringOrDefault("HAULIER_EMAIL");
                        instance.HaulierOperatorLicence = records.GetStringOrDefault("HAULIER_LICENCE_NO");
                        instance.HaulierNumberOfMovements = records.GetShortOrDefault("TOTAL_MOVES");
                        instance.HaulierNumberOfPieces = records.GetShortOrDefault("MAX_PARTS_PER_MOVE");
                        instance.HaulierApplicationNotes = records.GetStringOrDefault("APPLICATION_NOTES");
                        instance.AgentName = records.GetStringOrDefault("agent_name");
                        instance.NotesOnEscort = records.GetStringOrDefault("notesonescort");
                        instance.VehicleDescription = records.GetStringOrDefault("REDUCED_VEHICLE_DESCR");
                        instance.SubMovementClass = records.GetInt32OrDefault("vehicle_classification");

                        instance.OnBehalOfContactName = records.GetStringOrDefault("ON_BEHALF_OF_CONTACT");
                        instance.OnBehalOfHaulierOrgName = records.GetStringOrDefault("ON_BEHALF_OF_NAME");
                        instance.OnBehalOfHaulierAddress1 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_1");
                        instance.OnBehalOfHaulierAddress2 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_2");
                        instance.OnBehalOfHaulierAddress3 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_3");
                        instance.OnBehalOfHaulierAddress4 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_4");
                        instance.OnBehalOfHaulierAddress5 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_5");
                        instance.OnBehalOfHaulPostCode = records.GetStringOrDefault("ON_BEHALF_OF_POST_CODE");
                        instance.OnBehalOfCountryID = records.GetInt32OrDefault("ON_BEHALF_OF_COUNTRY");
                        instance.OnBehalOfCountryName = records.GetStringOrDefault("ON_BEHALF_OF_COUNTRYNAME");
                        instance.OnBehalOfHaulTelephoneNo = records.GetStringOrDefault("ON_BEHALF_OF_TEL_NO");
                        instance.OnBehalOfHaulFaxNo = records.GetStringOrDefault("ON_BEHALF_OF_FAX_NO");
                        instance.OnBehalOfHaulEmailID = records.GetStringOrDefault("ON_BEHALF_OF_EMAIL");
                        instance.OnBehalOfHaulOperatorLicens = records.GetStringOrDefault("ON_BEHALF_OF_LICENCE_NO");
                    }
            );
            return sOApplicationObj;
        }
        #endregion

        //Candiddate route view
        public static List<AffectedStructures> GetCandidateAgreedRoutePart(int revisionid, string userSchema)
        {
            try
            {
                List<AffectedStructures> objAffectedAtructures = new List<AffectedStructures>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objAffectedAtructures,
                  userSchema + ".SP_GET_HAULIER_CANDIDATE_APP",
               parameter =>
               {
                   parameter.AddWithValue("REVISION_ID", revisionid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("PRESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.RoutePartID = records.GetLongOrDefault("ROUTE_PART_ID");
                       instance.RoutePartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                       instance.RoutePartName = records.GetStringOrDefault("PART_NAME");
                   }
              );
                return objAffectedAtructures;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}