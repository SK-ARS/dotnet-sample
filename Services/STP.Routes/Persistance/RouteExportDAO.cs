using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Common.Constants;
using System.Collections.Generic;
using static GpxLibrary.ConvertGpx;
using STP.Domain.Routes;

namespace STP.Routes.Persistance
{
    public static class RouteExportDao
    {
        #region GetRouteList
        public static RouteExportList GetRouteList(int organisationId, int pageNumber, int pageSize)
        {
            RouteExportList routeExportList = new RouteExportList();
            List<Route> routeList = new List<Route>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeList,
                UserSchema.Portal + ".SP_GET_ROUTE_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pagesize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteId = records.GetLongOrDefault("LIBRARY_ROUTE_ID");
                    instance.RouteName = records.GetStringOrDefault("ROUTE_NAME");
                    instance.Description = records.GetStringOrDefault("ROUTE_DESCR");
                    instance.RouteType = records.GetStringOrDefault("ROUTE_TYPE");
                    routeExportList.TotalRecords = (long)records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                });

            //To get the Page Count
            if (routeExportList.TotalRecords > 0)
            {
                long Pages = routeExportList.TotalRecords / (pageSize);
                long RemainingRecords = routeExportList.TotalRecords % (pageSize);

                if (RemainingRecords >= 1)
                {
                    routeExportList.NumberOfPages = (int)Pages + 1;

                }
                else
                {
                    routeExportList.NumberOfPages = (int)Pages;
                }
            }
            routeExportList.Routes = routeList;
            routeExportList.PageNumber = pageNumber;
            routeExportList.PageSize = pageSize;
            return routeExportList;
        }
        #endregion

        #region CheckIsRouteExportable
        public static Domain.ExternalAPI.CheckRouteExportable CheckIsRouteExportable(long routeId, bool isApp, string userSchema)
        {
            Domain.ExternalAPI.CheckRouteExportable checkRouteExportable = new Domain.ExternalAPI.CheckRouteExportable();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   checkRouteExportable,
                   userSchema + ".SP_CHECK_ROUTE_EXPORTABLE",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_IS_APP", isApp ? 1 : 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, result) =>
                   {
                       result.RouteCount = (int)records.GetDecimalOrDefault("ROUTE_COUNT");
                       result.IsBroken = (int)records.GetDecimalOrDefault("IS_BROKEN");
                       result.IsMultiPath = (int)records.GetDecimalOrDefault("IS_MULTI_PATH");
                       result.IsMultiSegment = (int)records.GetDecimalOrDefault("IS_MULTI_SEGMENT");
                   }
               );
            return checkRouteExportable;
        }
        #endregion

        #region GetWayPoints(int plannedRouteID)
        public static List<WayPoints> GetWayPoints(long plannedRouteID, int organisationId, bool isApp, string userSchema)
        {
            List<WayPoints> wayPoints = new List<WayPoints>();
            int i = 1;
            string wayPointName = "WP";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                wayPoints,
                userSchema + ".SP_GET_ROUTE_POINTS_GPX",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", plannedRouteID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_APP", isApp ? 1 : 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.Name = wayPointName + i.ToString().PadLeft(3, '0');
                    result.Desc = records.GetStringOrDefault("DESCR");
                    result.Lat = records.GetDecimalOrDefault("LATITUDE").ToString();
                    result.Lon = records.GetDecimalOrDefault("LONGITUDE").ToString();
                    i++;
                });

            return wayPoints;
        }
        #endregion

        #region GetTrackPoints(int plannedRouteID)
        public static List<TrackPoints> GetTrackPoints(long plannedRouteID, int organisationId, bool isApp, string userSchema)
        {
            List<TrackPoints> trackPoints = new List<TrackPoints>();
            int i = 1;
            string trackPointName = "TP";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                trackPoints,
                userSchema + ".SP_GET_ROUTE_LINKS_GPX",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", plannedRouteID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_APP", isApp ? 1 : 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                (records, result) =>
                {
                    result.Name = trackPointName + i.ToString().PadLeft(3, '0');
                    result.Lat = records.GetDecimalOrDefault("LATITUDE").ToString();
                    result.Lon = records.GetDecimalOrDefault("LONGITUDE").ToString();
                    result.TrackName = records.GetStringOrDefault("PART_NAME").ToString();
                    i++;
                });
            return trackPoints;
        }
        #endregion

        #region ExportRouteList
        public static List<Domain.ExternalAPI.ExportRouteList> ExportRouteList(Domain.ExternalAPI.GetRouteExportList routeExportList)
        {
            List<Domain.ExternalAPI.ExportRouteList> routeList = new List<Domain.ExternalAPI.ExportRouteList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList
            (
                routeList,
                routeExportList.UserSchema + ".STP_EXPORT_DETAILS.SP_GET_ROUTE_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_REVISION_ID", routeExportList.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_REF_NO", routeExportList.ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID", routeExportList.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", routeExportList.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_TYPE", routeExportList.NotificationType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteDescription = records.GetStringOrDefault("PART_DESCR");
                }
            );
            return routeList;
        }
        #endregion
    }
}