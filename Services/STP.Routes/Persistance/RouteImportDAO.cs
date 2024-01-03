using NetSdoGeometry;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.Routes;
using System;
using System.Collections.Generic;
using System.Configuration;
using static STP.Domain.Routes.RouteModel;

namespace STP.Routes.Persistance
{
    public static class RouteImportDao
    {
        private static string LogInstance = ConfigurationManager.AppSettings["Instance"];
        private static string GPXPackage = ".STP_GPX_ROUTE.";
        public static List<RouteLinkModel> GetLinkFromGPXTrackPoint(sdogeometry pointGeom, string description)
        {
            List<RouteLinkModel> routeLink = new List<RouteLinkModel>();
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oracleGeo = cmd.CreateParameter();
            oracleGeo.OracleDbType = OracleDbType.Object;
            oracleGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleGeo.Value = pointGeom;
            #endregion
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    routeLink,
                     UserSchema.Portal + GPXPackage + "SP_GET_WAYPOINTS",
                    parameter =>
                    {
                        parameter.Add(oracleGeo);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.LinkId = (long)records.GetDecimalOrDefault("LINK_ID");
                        instance.RefInId = (long)records.GetDecimalOrDefault("REF_IN_ID");
                        instance.NrefInId = (long)records.GetDecimalOrDefault("NREF_IN_ID");
                        instance.DirTravel = records.GetStringOrDefault("DIR_TRAVEL");
                        instance.FuncClass = short.Parse(records.GetStringOrDefault("FUNC_CLASS"));
                        instance.RoadGeometry = records.GetGeometryOrNull("ROAD_GEOMETRY");
                        instance.Lrs = Convert.ToInt32(records.GetDecimalOrDefault("LINEAR_REF"));
                        instance.Description = description;
                    });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - RouteImport/GetRouteFromGPX, Exception: " + ex​​​​);
                throw;
            }
            return routeLink;
        }

        public static sdogeometry GetTransformedGeometry(sdogeometry routeGeom)
        {
            sdogeometry gpxGeometry = null;
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oracleGeo = cmd.CreateParameter();
            oracleGeo.OracleDbType = OracleDbType.Object;
            oracleGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleGeo.Value = routeGeom;
            #endregion
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                gpxGeometry,
                UserSchema.Portal + GPXPackage + "SP_GET_TRANSFORMED_GEOMETRY",
                parameter =>
                {
                    parameter.Add(oracleGeo);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    gpxGeometry = records.GetGeometryOrNull("GEOMETRY");
                });
            return gpxGeometry;
        }

        public static List<RouteLinkModel> GetRouteFromGPXTrack(sdogeometry routeGeom, int tolerance = 20)
        {
            List<RouteLinkModel> routeLinks = new List<RouteLinkModel>();
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oracleGeo = cmd.CreateParameter();
            oracleGeo.OracleDbType = OracleDbType.Object;
            oracleGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleGeo.Value = routeGeom;
            #endregion
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    routeLinks,
                    UserSchema.Portal + GPXPackage + "SP_GET_ROUTE_LINKS",
                    parameter =>
                    {
                        parameter.Add(oracleGeo);
                        parameter.AddWithValue("P_TOLERANCE", tolerance, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.LinkId = (long)records.GetDecimalOrDefault("LINK_ID");
                            instance.RefInId = (long)records.GetDecimalOrDefault("REF_IN_ID");
                            instance.NrefInId = (long)records.GetDecimalOrDefault("NREF_IN_ID");
                            instance.DirTravel = records.GetStringOrDefault("DIR_TRAVEL");
                            instance.FuncClass = short.Parse(records.GetStringOrDefault("FUNC_CLASS"));
                            instance.IsRoundabout = records.GetStringOrDefault("ROUNDABOUT");
                        }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - RouteImport/GetRouteFromGPX, Exception: " + ex​​​​);
                throw;
            }
            return routeLinks;
        }

        public static RouteLinkModel GetLinkForAutoReplanPoint(sdogeometry pointGeom, string description)
        {
            RouteLinkModel routeLink = new RouteLinkModel();
            #region
            OracleCommand cmd = new OracleCommand();
            OracleParameter oracleGeo = cmd.CreateParameter();
            oracleGeo.OracleDbType = OracleDbType.Object;
            oracleGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleGeo.Value = pointGeom;
            #endregion
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routeLink,
                UserSchema.Portal + GPXPackage + "SP_GET_WAYPOINTS_AUTO_REPLAN",
                parameter =>
                {
                    parameter.Add(oracleGeo);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.LinkId = (long)records.GetDecimalOrDefault("LINK_ID");
                    instance.RefInId = (long)records.GetDecimalOrDefault("REF_IN_ID");
                    instance.NrefInId = (long)records.GetDecimalOrDefault("NREF_IN_ID");
                    instance.DirTravel = records.GetStringOrDefault("DIR_TRAVEL");
                    instance.FuncClass = short.Parse(records.GetStringOrDefault("FUNC_CLASS"));
                    instance.RoadGeometry = records.GetGeometryOrNull("ROAD_GEOMETRY");
                    instance.Lrs = Convert.ToInt32(records.GetDecimalOrDefault("LINEAR_REF"));
                    instance.Description = description;
                });
            return routeLink;
        }

        public static List<RoutePoint> GetRoutePointForReplan(RouteSegment routeSegment, string userSchema)
        {
            List<RoutePoint> routePoints = new List<RoutePoint>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routePoints,
                userSchema + ".STP_HISTORIC_ROUTE.SP_GET_ROUTE_POINT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PATH_ID", routeSegment.RoutePathId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.LinkId = (long)records.GetDecimalOrDefault("LINK_ID");
                    instance.NewBeginNodeId = (long)records.GetDecimalOrDefault("REF_IN_ID");
                    instance.NewEndNodeId = (long)records.GetDecimalOrDefault("NREF_IN_ID");
                    instance.PointType = (int)records.GetDecimalOrDefault("ROUTE_POINT_TYPE");
                });
            return routePoints;
        }
    }
}