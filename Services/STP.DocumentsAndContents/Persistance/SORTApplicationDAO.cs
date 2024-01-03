using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Persistance
{
    public static class SORTApplicationDAO
    {
        #region ListSortUserAppl(usertypeid)
        public static List<GetSORTUserList> ListSortUserAppl(long usertypeid, int checkertype = 0)
        {
            List<GetSORTUserList> objSortUser = new List<GetSORTUserList>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                        objSortUser,
                        UserSchema.Sort + ".LIST_SORT_USER",
                        parameter =>
                        {
                            parameter.AddWithValue("p_USERTYPE_ID", usertypeid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_CHCEKING_TYPE", checkertype, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
            return objSortUser;
        }
        #endregion
        #region GetSpecialOrderList
        public static List<SORTMovementList> GetSpecialOrderList(long projectId)
        {
            List<SORTMovementList> SpecialOrderListObj = new List<SORTMovementList>();

            //Setup Procedure LIST_MOVEMENT
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                SpecialOrderListObj,
                UserSchema.Sort + ".SP_GET_SORT_SPECIAL_ORD_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_PRODUCTID", projectId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.OrderNumber = records.GetStringOrDefault("ORDER_NO");
                        instance.ExpiryDate = records.GetStringOrDefault("EXPIRY_DATE");
                        instance.SOCreateDate = records.GetDateTimeOrDefault("CREATED_DATE");
                    }
          );
            return SpecialOrderListObj;
        }

        #endregion

        #region List NE Broken Route Details
        public static List<NotifRouteImport> ListNEBrokenRouteDetails(NotifRouteImportParams objNotifRouteImportParams)
        {
            List<NotifRouteImport> objlistrt = new List<NotifRouteImport>();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                       objlistrt,
                           UserSchema.Portal + ".SP_FETCH_BROKENNENROUTES",
                       parameter =>
                       {
                           parameter.AddWithValue("P_NEN_ID ", objNotifRouteImportParams.NENId, OracleDbType.Long, ParameterDirectionWrap.Input);
                           parameter.AddWithValue("P_USER_ID ", objNotifRouteImportParams.IUserId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                           parameter.AddWithValue("P_INBOX_ITEM_ID ", objNotifRouteImportParams.InboxItemId, OracleDbType.Long, ParameterDirectionWrap.Input);
                           parameter.AddWithValue("P_ORGANISATION_ID ", objNotifRouteImportParams.IOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                           parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                       },
                           (records, instance) =>
                           {
                               instance.RouteName = records.GetStringOrDefault("PART_NAME");
                           }
                   );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{ConfigurationManager.AppSettings["Instance"]} - SORTApplication/ListNEBrokenRouteDetails, Exception: {ex}");
                throw;
            }
            return objlistrt;
        }
        #endregion
    }
}