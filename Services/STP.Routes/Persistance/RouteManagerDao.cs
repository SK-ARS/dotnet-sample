using NetSdoGeometry;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.Custom;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.RouteAssessment;
using STP.Domain.Routes;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using static STP.Domain.Routes.RouteModel;
using static STP.Domain.Routes.RouteSerialization;

namespace STP.Routes.Persistance
{
    #region RouteManagerDao
    public static class RouteManagerDao
    {
        public static bool RouteSave { get; set; }
        public static long GlobalOrgId { get; set; }

        #region Library Route Scenarios

        #region Library Route List
        public static List<RoutePartDetails> GetLibraryRouteList(int organisationID, int pageNumber, int pageSize, int routeType, string serchString, string userSchema = UserSchema.Portal, int filterFavouritesRoutes = 0, int presetFilter = 1, int? sortOrder = null)
        {
            List<RoutePartDetails> routeDetailsList = new List<RoutePartDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            routeDetailsList,
            userSchema + ".SP_R_GET_PLAN_ROUTE_NAME",
            parameter =>
            {
                parameter.AddWithValue("P_ORGANISATION_ID", organisationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_routeType", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ROUTE_NAME", serchString, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_FILTER_FAVOURITES_FLAG", filterFavouritesRoutes, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                instance.RouteId = records.GetLongOrDefault("PLANNED_ROUTE_ID"); //Id for the Route
                instance.RouteName = records.GetStringOrDefault("ROUTE_NAME");   //Route Name
                instance.RouteDescr = records.GetStringOrDefault("ROUTE_DESCR");        //Route Type
                instance.RouteType = records.GetStringOrDefault("NAME");
                instance.IsBrokenLib = (int)records.GetDecimalOrDefault("IS_BROKEN");
                instance.TotalRecord = (int)records.GetDecimalOrDefault("TOTAL_ROWS");
                instance.IsFavourites = records.GetInt16OrDefault("IS_FAVOURITE");
                instance.LastUpdate = records.GetDateTimeOrDefault("LASTUPDATE");
            });
            return routeDetailsList;
        }


        #endregion

        #region Get Library Route

        #region Get Library Route New Method
        public static RoutePart GetLibraryRoute(long routeId, string userSchema)
        {
            RoutePart routePart = new RoutePart();
            var routeJson = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routePart,
               userSchema + ".STP_GET_ROUTE_PART.SP_LIBRARY_ROUTE_JSON",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    routeJson = records.GetStringOrDefault("ROUTE_JSON");
                });
            routePart = JsonConvert.DeserializeObject<RoutePart>(routeJson);
            routePart = GetInnerText(routePart);
            return routePart;
        }


        #endregion

        //These method can be removed after QC verification in QC environment

        #region Library Route Path
        /*public static RoutePart GetLibraryRoute(long plannedRouteID, string userSchema = UserSchema.Portal)
        {
            List<RoutePart> routeList = new List<RoutePart>();
            RoutePart routePart = null;
            RouteSegment routeSegment = null;
            RoutePath routePath = new RoutePath();
            int nSegNo = 0;
            long nRoutePath = 0;
            long routeType = 0;
            int segmentType = 0;

            Dictionary<long, RoutePath> routePathList = new Dictionary<long, RoutePath>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeList,
                userSchema + ".SP_GET_LINK_IDS",
                parameter =>
                {
                    parameter.AddWithValue("P_PLANNED_ROUTE_ID", plannedRouteID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    long linkID;
                    if (routePart == null)
                    {
                        routePart = new RoutePart();
                        routePart.RoutePartDetails.RouteId = records.GetLongOrDefault("LIBRARY_ROUTE_ID");
                        routePart.RoutePartDetails.RouteName = records.GetStringOrDefault("ROUTE_NAME");
                        routePart.RoutePartDetails.RouteDescr = records.GetStringOrDefault("PATH_DESCR");
                        routePart.OrgId = records.GetLongOrDefault("ORGANISATION_ID");
                        routeType = records.GetInt32OrDefault("ROUTE_TYPE");
                        if (routeType == 615001)
                            routePart.RoutePartDetails.RouteType = "outline";
                        else if (routeType == 615002)
                            routePart.RoutePartDetails.RouteType = "planned";
                        routePart.RoutePartDetails.StartDesc = records.GetStringOrDefault("START_DESCRIPTION");
                        routePart.RoutePartDetails.EndDesc = records.GetStringOrDefault("END_DESCRIPTION");
                    }
                    if (routePart.RoutePartDetails.RouteType == "planned")
                    {
                        long nTempRoutePath = records.GetLongOrDefault("ROUTE_PATH_ID");
                        int segNo = records.GetInt16OrDefault("SEGMENT_NO");
                        if (nRoutePath == 0 || nTempRoutePath != nRoutePath || routeSegment == null)
                        {
                            routePath = new RoutePath();
                            routePart.RoutePathList.Add(routePath);
                            routePath.RouteId = records.GetLongOrDefault("LIBRARY_ROUTE_ID");
                            routePath.RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID");
                            routePath.RoutePathNo = records.GetInt16OrDefault("ROUTE_PATH_NO");
                            routePath.PathDescr = records.GetStringOrDefault("ROUTE_PATH_DESCR");
                            if (segNo == 1 && nRoutePath != nTempRoutePath)
                            {
                                routeSegment = new RouteSegment();
                                routeSegment.SegmentNo = records.GetInt16OrDefault("SEGMENT_NO");
                                routeSegment.RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID");
                                routeSegment.SegmentId = records.GetLongOrDefault("SEGMENT_ID");
                                routeSegment.OffRoadGeometry = records.GetGeometryOrNull("OFF_ROAD_GEOMETRY"); //Fetching Off Road geometry
                                routeSegment.StartPointGeometry = records.GetGeometryOrNull("START_POINT_GEOMETRY"); //Fetching start geometry
                                routeSegment.EndPointGeometry = records.GetGeometryOrNull("END_POINT_GEOMETRY"); //Fetching end geometry
                                linkID = ConvertLinkID(records.GetLongOrDefault("START_LINK_ID"));
                                routeSegment.StartLinkId = linkID == 0 ? (long?)null : linkID; //Fetching start link id
                                linkID = ConvertLinkID(records.GetLongOrDefault("END_LINK_ID"));
                                routeSegment.EndLinkId = linkID == 0 ? (long?)null : linkID; //Fetching end link id                                
                                segmentType = records.GetInt32OrDefault("SEGMENT_TYPE");
                                routeSegment.SegmentType = GetSegmentType(segmentType);
                                routeSegment.StartLrs = records.GetInt32OrDefault("START_LINEAR_REF");
                                routeSegment.EndLrs = records.GetInt32OrDefault("END_LINEAR_REF");
                                routeSegment.StartPointDirection = records.GetInt16Nullable("START_POINT_DIRECTION");
                                routeSegment.EndPointDirection = records.GetInt16Nullable("END_POINT_DIRECTION");
                                routeSegment.RouteAnnotationsList = GetAnnotation(routeSegment.SegmentNo, routeSegment.SegmentId, 1);
                                routePath.RouteSegmentList.Add(routeSegment);
                                nSegNo = segNo;
                            }
                            routePath.RoutePointList = GetLibraryRoutePoints(routePath.RoutePathId, userSchema);
                            routePathList.Add(routePath.RoutePathId, routePath);
                            nRoutePath = nTempRoutePath;
                        }
                        if (segNo != nSegNo)
                        {
                            routeSegment = new RouteSegment();
                            routeSegment.SegmentNo = records.GetInt16OrDefault("SEGMENT_NO");
                            routeSegment.RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID");
                            routeSegment.SegmentId = records.GetLongOrDefault("SEGMENT_ID");
                            routeSegment.StartPointGeometry = records.GetGeometryOrNull("START_POINT_GEOMETRY"); //Fetching start geometry
                            routeSegment.EndPointGeometry = records.GetGeometryOrNull("END_POINT_GEOMETRY"); //Fetching end geometry
                            linkID = ConvertLinkID(records.GetLongOrDefault("START_LINK_ID"));
                            routeSegment.StartLinkId = linkID == 0 ? (long?)null : linkID; //Fetching start link id
                            linkID = ConvertLinkID(records.GetLongOrDefault("END_LINK_ID"));
                            routeSegment.EndLinkId = linkID == 0 ? (long?)null : linkID; //Fetching end link id
                            segmentType = records.GetInt32OrDefault("SEGMENT_TYPE");
                            routeSegment.SegmentType = GetSegmentType(segmentType);
                            routeSegment.StartLrs = records.GetInt32OrDefault("START_LINEAR_REF");
                            routeSegment.EndLrs = records.GetInt32OrDefault("END_LINEAR_REF");
                            routeSegment.StartPointDirection = records.GetInt16Nullable("START_POINT_DIRECTION");
                            routeSegment.EndPointDirection = records.GetInt16Nullable("END_POINT_DIRECTION");
                            routeSegment.OffRoadGeometry = records.GetGeometryOrNull("OFF_ROAD_GEOMETRY"); // Fetching Off Road Geometry
                            routeSegment.RouteAnnotationsList = GetAnnotation(routeSegment.SegmentNo, routeSegment.SegmentId, 1);
                            routePath.RouteSegmentList.Add(routeSegment);
                            nSegNo = segNo;
                        }
                        RouteLink routelink = new RouteLink();
                        if (routeSegment.OffRoadGeometry == null) //fetching link_ids for segments without offroad geometry
                        {
                            routelink.SegmentId = records.GetLongOrDefault("SEGMENT_ID");
                            routelink.SegmentNo = records.GetInt16OrDefault("SEGMENT_NO");
                            routelink.LinkNo = Convert.ToInt16(records["LINK_NO"]);
                            routelink.LinkId = ConvertLinkID(records.GetLongOrDefault("LINK_ID"));
                            routelink.Direction = records.GetInt16Nullable("DIRECTION");
                        }
                        if (routelink.SegmentId != 0)
                            routeSegment.RouteLinkList.Add(routelink);
                    }
                    else
                    {
                        if (routePath == null)
                            routePath = new RoutePath();
                    }
                }
                );
            if (routeType == 615001)
            {
                routePath = new RoutePath();
                routePath.RouteId = routePart.RoutePartDetails.RouteId;
                routePath.RoutePointList = GetOutlineLibraryRoutePoint(routePath.RouteId);
                routePart.RoutePathList.Add(routePath);
            }
            if (routeType == 615002)
            {
                routePathList = routePathList.OrderBy(kyp => kyp.Key).ToDictionary(d => d.Key, d => d.Value);
                List<long> dictKeyList = routePathList.Keys.ToList();
                foreach (long key in dictKeyList)
                {
                    switch (routePathList[key].RoutePathNo)
                    {
                        case 1:
                            if (key == dictKeyList.Min())
                                routePathList[key].RoutePathType = 0;        //Main Route
                            else
                                routePathList[key].RoutePathType = 1;        //Alternate Start
                            break;
                        case 2:
                            routePathList[key].RoutePathType = 2;            //Alternate Middle
                            break;
                        case 3:
                            routePathList[key].RoutePathType = 3;            //Alternate End
                            break;
                    }
                }
                List<RoutePath> updatedRoutePath = new List<RoutePath>();

                foreach (KeyValuePair<long, RoutePath> entry in routePathList)
                {
                    updatedRoutePath.Add(entry.Value);
                }

                routePart.RoutePathList = updatedRoutePath;
            }
            return routePart;
        }*/




        #endregion

        #region Library Route Points
        public static List<RoutePoint> GetLibraryRoutePoints(long routePathId, string userSchema)
        {
            List<RoutePoint> plannedRoutePoint = new List<RoutePoint>();
            long pointtype = 0;
            int rpPtNo = 1;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            plannedRoutePoint,
            userSchema + ".SP_GET_ROUTE_POINTS",
            parameter =>
            {
                parameter.AddWithValue("P_ROUTE_PATH_ID", routePathId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                instance.RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID");
                instance.RoutePointNo = rpPtNo > 2 ? rpPtNo - 2 : rpPtNo;
                instance.RoutePointId = Convert.ToInt32(records.GetLongOrDefault("ROUTE_POINT_ID"));
                instance.PointDescr = records.GetStringOrDefault("DESCR");
                string direct = records["DIRECTION"].ToString();
                if (string.IsNullOrEmpty(direct))
                    instance.Direction = -1;
                else
                    instance.Direction = Convert.ToInt32(direct);
                instance.LinkId = ConvertLinkID(records.GetLongOrDefault("DATALINKS_LINK_ID"));
                instance.Lrs = records.GetInt32OrDefault("LINEAR_REF");
                pointtype = records.GetInt32OrDefault("ROUTE_POINT_TYPE");
                instance.PointGeom = records.GetGeometryOrNull("ROAD_POINT_GEOMETRY");
                instance.IsAnchorPoint = records.GetInt16OrDefault("IS_ANCHOR_POINT");
                // Setting Type of route point based on flag variable (0 start point 1 end point 2 way point 3 via point)
                instance.PointType = (int)pointtype - 239001;
                instance.WayText = GetWayText(records, "WAY_TEXT");
                rpPtNo++;
            });
            return plannedRoutePoint;
        }

        




        #endregion

        #region Outline Library Route Point
        /*private static List<RoutePoint> GetOutlineLibraryRoutePoint(long routeId)
        {
            List<RoutePoint> plannedRoutePoint = new List<RoutePoint>();
            long pointtype = 0; // flag variable
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                plannedRoutePoint,
                UserSchema.Portal + ".SP_R_GET_OUTLINE_ROUTE_POINTS",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RoutePointNo = records.GetInt16OrDefault("POINT_NO");
                    instance.PointAnnotation = records.GetStringOrDefault("ANNOTATION"); //Route-Point annotation 
                    instance.PointGeom = records.GetGeometryOrNull("GEOMETRY");
                    instance.PointDescr = records.GetStringOrDefault("DESCRIPTION"); // Route- point description for outline routes actually stores the point location details
                    pointtype = records.GetInt32OrDefault("POINT_TYPE");    // flag variable
                    instance.IsAnchorPoint = 0;
                    // Setting Type of route point based on flag variable (0 start point 1 end point 2 way point 3 via point)
                    instance.PointType = (int)pointtype - 239001;
                    if (instance.PointType == 3)
                        instance.IsAnchorPoint = 1;
                    //Setting the pointno's for way points/via point from 1 since it is saved in database from 3 due to Unique index
                    if ((instance.PointType == 3 || instance.PointType == 2) && instance.RoutePointNo > 2)
                        instance.RoutePointNo -= 2;
                });
            return plannedRoutePoint;
        }*/
        #endregion

        #endregion

        #region Save Update Delete Library Route

        #region Save Library Route
        public static long SaveLibraryRoute(RoutePart routePart, string userSchema = UserSchema.Portal)
        {
            RouteSave = true;
            long result = 0;
            GlobalOrgId = routePart.OrgId;
            long routeType = 0;
            Serialization serialization = new Serialization();
            routePart = serialization.DeserializeRoutePart(routePart);
            routePart.RoutePathList = ProcessRoutePath(routePart.RoutePathList);

            RoutePathArray routePathObj = new RoutePathArray()
            {
                RoutePathObj = routePart.RoutePathList.ToArray()
            };

            OracleCommand cmd = new OracleCommand();
            OracleParameter routePathObjParam = cmd.CreateParameter();
            routePathObjParam.OracleDbType = OracleDbType.Object;
            routePathObjParam.UdtTypeName = "PORTAL.ROUTEPATHARRAY";
            routePathObjParam.Value = routePart.RoutePathList.Count > 0 ? routePathObj : null;
            routePathObjParam.ParameterName = "P_ROUTE_PATH_ARRAY";
            if (routePart.RoutePartDetails.RouteType == "planned")
                routeType = 615002;
            else
                routeType = 615001;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/SaveLibraryRoute , Starting saving library route : " + routePart.RoutePartDetails.RouteName);
            //Function needs to be updated with SP variables
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_SAVE.SP_SAVE_LIBRARY_ROUTE",
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTE_NAME", routePart.RoutePartDetails.RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_DESCR", routePart.RoutePartDetails.RouteDescr, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_START_DESCR", routePart.RoutePartDetails.StartDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_END_DESCR", routePart.RoutePartDetails.EndDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_ID", routePart.OrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", routePart.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(routePathObjParam);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = (long)records.GetDecimalOrDefault("LIBRARY_ROUTE_ID");
                    if (result == 0)
                        RouteSave = false;
                });
            if (!RouteSave)
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/SaveLibraryRoute , Failed To Save library route ");
            else
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/SaveLibraryRoute , Saving library route " + routePart.RoutePartDetails.RouteName + " completed");
            return result;
        }
        #endregion

        #region Update Library Route
        public static long UpdateLibraryRoute(RoutePart routePart, string userSchema = UserSchema.Portal)
        {
            RouteSave = true;
            long result = 0;
            GlobalOrgId = routePart.OrgId;
            long routeType = 0;

            Serialization serialization = new Serialization();
            routePart = serialization.DeserializeRoutePart(routePart);
            routePart.RoutePathList = ProcessRoutePath(routePart.RoutePathList);

            RoutePathArray routePathObj = new RoutePathArray()
            {
                RoutePathObj = routePart.RoutePathList.ToArray()
            };

            OracleCommand cmd = new OracleCommand();
            OracleParameter routePathObjParam = cmd.CreateParameter();
            routePathObjParam.OracleDbType = OracleDbType.Object;
            routePathObjParam.UdtTypeName = "PORTAL.ROUTEPATHARRAY";
            routePathObjParam.Value = routePart.RoutePathList.Count > 0 ? routePathObj : null;
            routePathObjParam.ParameterName = "P_ROUTE_PATH_ARRAY";

            if (routePart.RoutePartDetails.RouteType == "planned")
                routeType = 615002;
            else
                routeType = 615001;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/UpdateLibraryRoute , Starting updating library route : " + routePart.RoutePartDetails.RouteName);
            //Function needs to be updated with SP variables
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_SAVE.SP_UPDATE_LIBRARY_ROUTE",
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTE_ID", routePart.RoutePartDetails.RouteId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_NAME", routePart.RoutePartDetails.RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_DESCR", routePart.RoutePartDetails.RouteDescr, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_START_DESCR", routePart.RoutePartDetails.StartDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_END_DESCR", routePart.RoutePartDetails.EndDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORG_ID", routePart.OrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", routePart.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(routePathObjParam);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = (long)records.GetDecimalOrDefault("LIBRARY_ROUTE_ID");
                    if (result == 0)
                        RouteSave = false;
                });
            if (!RouteSave)
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/UpdateLibraryRoute , Failed To Update library route ");
            else
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/UpdateLibraryRoute , Updating library route " + routePart.RoutePartDetails.RouteName + " completed");
            return result;
        }
        #endregion

        #region Delete Library Route
        public static int DeleteLibraryRoute(long PlannedRouteID, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 userSchema + ".STP_ROUTE_SAVE.SP_DELETE_LIBRARY_ROUTE",
                 parameter =>
                 {
                     parameter.AddWithValue("p_LIB_ROUTE_ID", PlannedRouteID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_Affected_Rows", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = records.GetInt32("p_Affected_Rows");
                 });
            return result;
        }
        #endregion

        #endregion

        #region Add Route To Library
        public static long AddRouteToLibrary(long routePartId, int orgId, string rtType, string userSchema = UserSchema.Portal)
        {
            long routeId = 0;
            long routeType = 615002;
            if (rtType == "outline")
                routeType = 615001;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routeId,
                userSchema + ".SP_ADD_ROUTE_TO_LIBRARY",
                parameter =>
                {
                    parameter.AddWithValue("P_PART_ID", routePartId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", orgId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 records =>
                 {
                     routeId = records.GetLongOrDefault("LIBRARY_ROUTE_ID");
                 });
            return routeId;
        }

        public static int CheckRouteName(string RouteName, int organisationId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
           userSchema + ".SP_CHECK_ROUTE_NAME",
            parameter =>
            {
                parameter.AddWithValue("ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ROUTE_NAME", RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
             record =>
             {
                 result = Convert.ToInt32(record.GetDecimalOrDefault("CNT"));
             });
            return result;
        }
        #endregion

        #endregion

        #region Application/Notification Route Scenarios

        #region Get Application Route

        #region Get Application Route New Method
        public static RoutePart GetApplicationRoute(long routeId, string userSchema)
        {
            RoutePart routePart = new RoutePart();
            var routeJson = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routePart,
               userSchema + ".STP_GET_ROUTE_PART.SP_APPLICATION_ROUTE_JSON",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    routeJson = records.GetStringOrDefault("ROUTE_JSON");
                });
            routePart = JsonConvert.DeserializeObject<RoutePart>(routeJson);
            routePart = GetInnerText(routePart);

            return routePart;
        }
        #endregion

        #region Get Application Route Old Method
        //These method can be removed after QC verification in QC environment

        #region Application Route Path
        /*public static RoutePart GetApplicationRoute(long routePartId, string userSchema)
        {
            List<RoutePart> routeList = new List<RoutePart>();
            RoutePart routePart = null;
            RouteSegment routeSegment = null;
            RoutePath routePath = new RoutePath();

            int nSegNo = 0;
            long nRoutePath = 0;
            int tempSegType = 0;
            int dock_caut = 0;

            Dictionary<long, RoutePath> routePathList = new Dictionary<long, RoutePath>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeList,
               userSchema + ".SP_APP_ROUTE_LINKS",
                parameter =>
                {
                    parameter.AddWithValue("p_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    long startLinkId;
                    long endLinkId;

                    if (routePart == null)
                    {
                        routePart = new RoutePart();
                        routePart.RoutePartDetails.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                        routePart.RoutePartDetails.RouteName = records.GetStringOrDefault("PART_NAME");
                        routePart.RoutePartDetails.RouteDescr = records.GetStringOrDefault("PART_DESCR");
                        routePart.RoutePartDetails.RoutePartNo = records.GetInt16OrDefault("ROUTE_PART_NO"); //route part no of route part id
                        if (userSchema.ToLower() == UserSchema.Sort)
                        {
                            dock_caut = Convert.ToInt16(records["INCLUDE_DOCK_CAUTION"]);
                            if (dock_caut == 1)
                                routePart.RoutePartDetails.DockCaution = true;
                            else
                                routePart.RoutePartDetails.DockCaution = false;
                        }
                        routePart.RoutePartDetails.RouteType = "planned";
                    }
                    long nTempRoutePath = records.GetLongOrDefault("ROUTE_PATH_ID");
                    int segNo = records.GetInt16OrDefault("SEGMENT_NO");
                    if (nRoutePath == 0 || nTempRoutePath != nRoutePath || routeSegment == null)
                    {
                        routePath = new RoutePath();
                        routePart.RoutePathList.Add(routePath);
                        routePath.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                        
                        routePath.RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID");
                       
                        //Route Path id for a route is fetched here (required)
                        routePath.RoutePathNo = records.GetInt16OrDefault("ROUTE_PATH_NO");
                       
                        routePath.PathDescr = records.GetStringOrDefault("ROUTE_PATH_DESCR");           //route path description is fetched (required)
                        if (segNo == 1 && nRoutePath != nTempRoutePath)
                        {
                            tempSegType = records.GetInt32OrDefault("SEGMENT_TYPE");
                            startLinkId = ConvertLinkID(records.GetLongOrDefault("START_LINK_ID"));
                            endLinkId = ConvertLinkID(records.GetLongOrDefault("END_LINK_ID"));
                            routeSegment = new RouteSegment
                            {
                                SegmentNo = records.GetInt16OrDefault("SEGMENT_NO"),
                                RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID"),
                                SegmentId = records.GetLongOrDefault("SEGMENT_ID"),
                                SegmentDesc = records.GetStringOrDefault("SEGMENT_DESCR"), //segment descirption
                                SegmentType = GetSegmentType(tempSegType), //converting to segment Type 1 2 3 4 5 6 7 8 
                                OffRoadGeometry = records.GetGeometryOrNull("OFF_ROAD_GEOMETRY"), //Fetching Off Road geometry
                                StartLinkId = startLinkId == 0 ? (long?)null : startLinkId,//Fetching start link id
                                EndLinkId = endLinkId == 0 ? (long?)null : endLinkId,//Fetching end link id
                                StartPointGeometry = records.GetGeometryOrNull("START_POINT_GEOMETRY"), //Fetching start geometry
                                EndPointGeometry = records.GetGeometryOrNull("END_POINT_GEOMETRY"), //Fetching end geometry
                                StartLrs = records.GetInt32OrDefault("START_LINEAR_REF"),
                                EndLrs = records.GetInt32OrDefault("END_LINEAR_REF"),
                                StartPointDirection = records.GetInt16Nullable("START_POINT_DIRECTION"),
                                EndPointDirection = records.GetInt16Nullable("END_POINT_DIRECTION")
                            };
                            routeSegment.RouteAnnotationsList = GetAnnotation(routeSegment.SegmentNo, routeSegment.SegmentId, 0, userSchema);
                            routePath.RouteSegmentList.Add(routeSegment);
                            nSegNo = segNo;
                        }
                        routePath.RoutePointList = GetApplicationRoutePoints(routePath.RoutePathId, userSchema);// Function to get route-points based on route-variants or route-paths 's :?
                        routePathList.Add(routePath.RoutePathId, routePath);                           //Adding the route path variant to route variant list Obj
                        nRoutePath = nTempRoutePath;
                    }
                    if (segNo != nSegNo)
                    {
                        tempSegType = records.GetInt32OrDefault("SEGMENT_TYPE");
                        startLinkId = ConvertLinkID(records.GetLongOrDefault("START_LINK_ID"));
                        endLinkId = ConvertLinkID(records.GetLongOrDefault("END_LINK_ID"));
                        routeSegment = new RouteSegment
                        {
                            SegmentId = records.GetLongOrDefault("SEGMENT_ID"),
                            SegmentNo = records.GetInt16OrDefault("SEGMENT_NO"),
                            RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID"),
                            SegmentDesc = records.GetStringOrDefault("PART_DESCR"),
                            SegmentType = GetSegmentType(tempSegType),     //converting to segment Type 1 2 3 4 5 6 7 8 
                            OffRoadGeometry = records.GetGeometryOrNull("OFF_ROAD_GEOMETRY"), //Fetching Off Road geometry
                            StartPointGeometry = records.GetGeometryOrNull("START_POINT_GEOMETRY"), //Fetching start geometry
                            EndPointGeometry = records.GetGeometryOrNull("END_POINT_GEOMETRY"), //Fetching end geometry
                            StartLinkId = startLinkId == 0 ? (long?)null : startLinkId, //Fetching start link id
                            EndLinkId = endLinkId == 0 ? (long?)null : endLinkId, //Fetching end link id
                            StartLrs = records.GetInt32OrDefault("START_LINEAR_REF"),
                            EndLrs = records.GetInt32OrDefault("END_LINEAR_REF"),
                            StartPointDirection = records.GetInt16Nullable("START_POINT_DIRECTION"),
                            EndPointDirection = records.GetInt16Nullable("END_POINT_DIRECTION")
                        };
                        routeSegment.RouteAnnotationsList = GetAnnotation(routeSegment.SegmentNo, routeSegment.SegmentId, 0, userSchema);
                        routePath.RouteSegmentList.Add(routeSegment);
                        nSegNo = segNo;
                    }
                    RouteLink routelink = new RouteLink();
                    if (routeSegment.OffRoadGeometry == null) //fetching link_ids for segments without offroad geometry
                    {
                        routelink.SegmentId = records.GetLongOrDefault("SEGMENT_ID");
                        routelink.SegmentNo = records.GetInt16OrDefault("SEGMENT_NO");
                        if (records["LINK_NO"].ToString() != "")
                        {
                            routelink.LinkNo = Convert.ToInt16(records["LINK_NO"]);
                        }
                        else
                        {
                            routelink.LinkNo = records.GetInt16Nullable("LINK_NO");
                            if (routelink.LinkNo == null) { routelink.LinkNo = 0; }
                        }
                        if (records["LINK_ID"].ToString() != "") //check added for NEN related routes 
                        {
                            routelink.LinkId = Convert.ToInt64(records["LINK_ID"].ToString());
                        }
                        else
                        {
                            routelink.LinkId = records.GetInt16Nullable("LINK_ID");
                            if (routelink.LinkId == null) { routelink.LinkId = 0; }
                        }
                        routelink.Direction = records.GetInt16Nullable("DIRECTION");
                        routeSegment.RouteLinkList.Add(routelink);
                    }
                });
            //Sorting dictionary value's
            routePathList = routePathList.OrderBy(kyp => kyp.Key).ToDictionary(d => d.Key, d => d.Value);

            //Retreiving key list of the dictionary variable
            List<long> dictKeyList = routePathList.Keys.ToList();

            foreach (long key in dictKeyList)
            {
                switch (routePathList[key].RoutePathNo)
                {
                    case 1:
                        if (key == dictKeyList.Min())
                            routePathList[key].RoutePathType = 0;        //Main Route
                        else
                            routePathList[key].RoutePathType = 1;        //Alternate Start
                        break;
                    case 2:
                        routePathList[key].RoutePathType = 2;            //Alternate Middle
                        break;
                    case 3:
                        routePathList[key].RoutePathType = 3;            //Alternate End
                        break;
                }
            }

            List<RoutePath> updatedRoutePath = new List<RoutePath>();

            foreach (KeyValuePair<long, RoutePath> entry in routePathList)
            {
                updatedRoutePath.Add(entry.Value);
            }
            if (routePart != null)
                routePart.RoutePathList = updatedRoutePath;
            return routePart;
        }*/
        #endregion

        #region Application Route Points
        /// <summary>
        /// Function to retrieve Application route point's
        /// </summary>
        /// <param name="routePartId"></param>
        /// <returns></returns>
        /*public static List<RoutePoint> GetApplicationRoutePoints(long? routePathId, string userSchema)
        {
            List<RoutePoint> plannedRoutePoint = new List<RoutePoint>();
            long pointtype = 0;
            int rpPtNo = 1;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                plannedRoutePoint,
                userSchema + ".SP_APP_ROUTE_LINK_POINTS",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PATH_ID", routePathId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID");
                    instance.RoutePointNo = records.GetInt16OrDefault("ROUTE_POINT_NO");
                    instance.RoutePointId = Convert.ToInt32(records.GetLongOrDefault("ROUTE_POINT_ID"));
                    instance.PointDescr = records.GetStringOrDefault("DESCR");
                    string direct = records["DIRECTION"].ToString();
                    if (string.IsNullOrEmpty(direct))
                        instance.Direction = -1;
                    else
                        instance.Direction = Convert.ToInt32(direct);
                    instance.LinkId = ConvertLinkID(records.GetLongOrDefault("LINK_ID"));
                    instance.Lrs = records.GetInt32OrDefault("LINEAR_REF");
                    pointtype = records.GetInt32OrDefault("ROUTE_POINT_TYPE");
                    instance.PointGeom = records.GetGeometryOrNull("ROAD_POINT_GEOMETRY");
                    // Setting Type of route point based on flag variable (0 start point 1 end point 2 way point 3 via point)
                    instance.PointType = (int)pointtype - 239001;
                    instance.IsAnchorPoint = records.GetInt16OrDefault("IS_ANCHOR_POINT");
                    if (userSchema != UserSchema.Sort)
                        instance.WayText = GetWayText(records, "WAY_TEXT");
                    rpPtNo++;
                }
                );
            return plannedRoutePoint;
        }*/
        #endregion

        #endregion

        #region Application Outline Route Part Geometry
        /// <summary>
        /// Function to retrieve application route parts from APPLICATION PARTS Table
        /// </summary>
        /// <param name="revId"></param>
        /// <returns></returns>
        public static RoutePart GetApplicationRoutePartGeometry(long partId, string userSchema)
        {
            RoutePart routePart = new RoutePart();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routePart,
                userSchema + ".SP_R_APP_GET_ROUTE",
                parameter =>
                {
                    parameter.AddWithValue("p_PART_ID", partId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    routePart.RoutePartDetails.PartGeometry = records.GetGeometryOrNull("PART_GEOMETRY");
                    routePart.RoutePartDetails.RouteDescr = records.GetStringOrDefault("part_descr");
                    routePart.RoutePartDetails.RouteId = records.GetLongOrDefault("PART_ID");
                    routePart.RoutePartDetails.RouteName = records.GetStringOrDefault("PART_NAME");
                });
            if (routePart.RoutePartDetails.RouteId != 0)    // PART_ID as routeId
            {
                RoutePath routePath = new RoutePath()
                {
                    RoutePointList = GetApplicationRoutePointGeometry(routePart.RoutePartDetails.RouteId, userSchema) // Getting ApplicationRoutePointGeometry
                };
                routePart.RoutePathList.Add(routePath); //Adding the routePath list to routePart Object
            }
            return routePart;
        }
        #endregion

        #region  Application Outline Route Point Geometry
        /// <summary>
        /// Function to retrieve outline route points from APPLICATION POINTS Table
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public static List<RoutePoint> GetApplicationRoutePointGeometry(long partId, string userSchema)
        {
            List<RoutePoint> routePoint = new List<RoutePoint>();
            long pointType = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routePoint,
                userSchema + ".SP_R_APP_GET_ROUTE_POINT",
                parameter =>
                {
                    parameter.AddWithValue("p_PartId", partId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RoutePointNo = records.GetInt16OrDefault("POINT_NO");
                    pointType = records.GetInt32OrDefault("POINT_TYPE");
                    instance.PointDescr = records.GetStringOrDefault("description");
                    instance.IsAnchorPoint = 0;
                    // Setting Type of route point based on flag variable (0 start point 1 end point 2 way point 3 via point)
                    instance.PointType = (int)pointType - 239001;
                    if (instance.PointType == 3)
                        instance.IsAnchorPoint = 1;
                    //Setting the pointno's for way points/via point from 1 since it is saved in database from 3 due to Unique index
                    if ((instance.PointType == 3 || instance.PointType == 2) && instance.RoutePointNo > 2)
                        instance.RoutePointNo -= 2;
                    instance.PointGeom = records.GetGeometryOrNull("POINT_GEOMETRY");
                });
            return routePoint;
        }
        #endregion

        #region HistoricApplicationRoute
        public static RoutePart GetHistoricAppRoute(long routeId, string userSchema)
        {
            RoutePart routePart = new RoutePart();
            var routeJson = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routePart,
               userSchema + ".STP_HISTORIC_ROUTE.SP_GET_ROUTE_PART_JSON",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    routeJson = records.GetStringOrDefault("ROUTE_JSON");
                });
            routePart = JsonConvert.DeserializeObject<RoutePart>(routeJson);
            routePart = GetInnerText(routePart);
            return routePart;
        }


        public static List<RouteLinkModel> GetHistoricRouteLinkModel(long segmentId, int segmentNo, int tolerance, string userSchema)
        {
            List<RouteLinkModel> routeLinkModel = new List<RouteLinkModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    routeLinkModel,
                    userSchema + ".STP_HISTORIC_ROUTE.SP_GET_CLONE_ROUTE_LINKS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_SEGMENT_ID", segmentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SEGMENT_NO", segmentNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
                    });
            return routeLinkModel;
        }
        #endregion

        #endregion

        #region Save Update Delete Application Route

        #region Save Application/Notification Route
        /// Function to create and save a route for Application and Notification
        public static long SaveApplicationRoute(SaveAppRouteParams saveAppRouteParams)
        {
            RouteSave = true;
            long result = 0;
            GlobalOrgId = saveAppRouteParams.RoutePart.OrgId;
            long routeType = 0;

            Serialization serialization = new Serialization();
            saveAppRouteParams.RoutePart = serialization.DeserializeRoutePart(saveAppRouteParams.RoutePart);
            saveAppRouteParams.RoutePart.RoutePathList = ProcessRoutePath(saveAppRouteParams.RoutePart.RoutePathList);

            RoutePathArray routePathObj = new RoutePathArray()
            {
                RoutePathObj = saveAppRouteParams.RoutePart.RoutePathList.ToArray()
            };

            if (saveAppRouteParams.RoutePart.RoutePartDetails.RouteType == "planned")
                routeType = 0;
            else
                routeType = 1;
            //Function needs to be updated with SP variables
            OracleCommand cmd = new OracleCommand();

            OracleParameter routePathObjParam = cmd.CreateParameter();
            routePathObjParam.OracleDbType = OracleDbType.Object;
            routePathObjParam.UdtTypeName = "PORTAL.ROUTEPATHARRAY";
            routePathObjParam.Value = saveAppRouteParams.RoutePart.RoutePathList.Count > 0 ? routePathObj : null;
            routePathObjParam.ParameterName = "P_ROUTE_PATH_ARRAY";

            OracleParameter oracleParameterGeo = cmd.CreateParameter();
            oracleParameterGeo.OracleDbType = OracleDbType.Object;
            oracleParameterGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleParameterGeo.Value = saveAppRouteParams.RoutePart.RoutePartDetails.PartGeometry; //Variable storing Geometry details for outline , route's 
            oracleParameterGeo.ParameterName = "P_GEOMETRY";
            string procedure = ".STP_ROUTE_SAVE.SP_SAVE_PORTAL_ROUTE";
            if (saveAppRouteParams.UserSchema == UserSchema.Sort)
                procedure = ".STP_ROUTE_SAVE.SP_SAVE_SORT_ROUTE";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + procedure,
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTE_NAME", saveAppRouteParams.RoutePart.RoutePartDetails.RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_DESCR", saveAppRouteParams.RoutePart.RoutePartDetails.RouteDescr, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VER_ID", saveAppRouteParams.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REV_ID", saveAppRouteParams.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(routePathObjParam);
                    parameter.Add(oracleParameterGeo);
                    parameter.AddWithValue("P_CONT_REF_NUM", saveAppRouteParams.ContRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DOCK_FLAG", saveAppRouteParams.DockFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_REVID", saveAppRouteParams.RouteRevisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RETURN_ROUTE", saveAppRouteParams.IsReturnRoute, OracleDbType.Boolean, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", saveAppRouteParams.RoutePart.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    switch (routeType)
                    {
                        case 0:
                            result = records.GetLongOrDefault("ROUTE_PART_ID");
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/SaveApplicationRoute , Output Parameters for Saving Route ROUTE_PART_ID:" + result);
                            break;
                        case 1:
                            result = records.GetLongOrDefault("PART_ID");
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteManager/SaveApplicationRoute , Output Parameters for Saving Route PART_ID:" + result);
                            break;
                    }
                    if (result != 0)
                        saveAppRouteParams.RoutePart.RoutePartDetails.RouteId = result;
                    else
                        RouteSave = false;
                });
            if (!RouteSave)         //deleting if not saved properly.
                DeleteApplicationRoute(saveAppRouteParams.RoutePart.RoutePartDetails.RouteId, saveAppRouteParams.RoutePart.RoutePartDetails.RouteType, saveAppRouteParams.UserSchema);
            return result;
        }
        #endregion

        #region Update Application/Notification Route
        /// Function to create and save a route for Application and Notification
        public static long UpdateApplicationRoute(SaveAppRouteParams updateAppRouteParams)
        {
            RouteSave = true;
            long result = 0;
            GlobalOrgId = updateAppRouteParams.RoutePart.OrgId;
            long routeType = 0;
            int isAutoPlanned = updateAppRouteParams.IsAutoPlan ? 1 : 0;
            Serialization serialization = new Serialization();
            updateAppRouteParams.RoutePart = serialization.DeserializeRoutePart(updateAppRouteParams.RoutePart);
            updateAppRouteParams.RoutePart.RoutePathList = ProcessRoutePath(updateAppRouteParams.RoutePart.RoutePathList);

            RoutePathArray routePathObj = new RoutePathArray()
            {
                RoutePathObj = updateAppRouteParams.RoutePart.RoutePathList.ToArray()
            };

            if (updateAppRouteParams.RoutePart.RoutePartDetails.RouteType == "planned")
            {
                routeType = 0;
            }
            else
            {
                routeType = 1;
            }
            //Function needs to be updated with SP variables
            OracleCommand cmd = new OracleCommand();

            OracleParameter routePathObjParam = cmd.CreateParameter();
            routePathObjParam.OracleDbType = OracleDbType.Object;
            routePathObjParam.UdtTypeName = "PORTAL.ROUTEPATHARRAY";
            routePathObjParam.Value = updateAppRouteParams.RoutePart.RoutePathList.Count > 0 ? routePathObj : null;
            routePathObjParam.ParameterName = "P_ROUTE_PATH_ARRAY";

            OracleParameter oracleParameterGeo = cmd.CreateParameter();
            oracleParameterGeo.OracleDbType = OracleDbType.Object;
            oracleParameterGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleParameterGeo.Value = updateAppRouteParams.RoutePart.RoutePartDetails.PartGeometry; //Variable storing Geometry details for outline , route's 
            oracleParameterGeo.ParameterName = "P_GEOMETRY";

            string procedure = ".STP_ROUTE_SAVE.SP_UPDATE_PORTAL_ROUTES";

            if (updateAppRouteParams.UserSchema == UserSchema.Sort)
                procedure = ".STP_ROUTE_SAVE.SP_UPDATE_SORT_ROUTES";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + procedure,
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTE_ID", updateAppRouteParams.RoutePart.RoutePartDetails.RouteId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_NAME", updateAppRouteParams.RoutePart.RoutePartDetails.RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_DESCR", updateAppRouteParams.RoutePart.RoutePartDetails.RouteDescr, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(routePathObjParam);
                    parameter.Add(oracleParameterGeo);
                    parameter.AddWithValue("P_CONT_REF_NUM", updateAppRouteParams.ContRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DOCK_FLAG", updateAppRouteParams.DockFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", updateAppRouteParams.RoutePart.UserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_AUTO_PLAN", isAutoPlanned, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    switch (routeType)
                    {
                        case 0:
                            result = records.GetLongOrDefault("ROUTE_PART_ID");
                            break;
                        case 1:
                            result = records.GetLongOrDefault("PART_ID");
                            break;
                    }
                    if (result != 0)
                        updateAppRouteParams.RoutePart.RoutePartDetails.RouteId = result;
                    else
                        RouteSave = false;
                });
            if (!RouteSave)         //deleting if not saved properly.
                DeleteApplicationRoute(updateAppRouteParams.RoutePart.RoutePartDetails.RouteId, updateAppRouteParams.RoutePart.RoutePartDetails.RouteType, updateAppRouteParams.UserSchema);
            return result;
        }
        #endregion

        #region Delete Application Route
        /// <summary>
        /// Function to delete an existing planned route
        /// returns 1 if route is deleted else 0
        /// </summary>
        /// <param name="PlannedRouteID"></param>
        /// <returns></returns>
        public static int DeleteApplicationRoute(long routeId, string routeType, string userSchema)
        {
            int result = 0;
            int rtType = 0;
            if (routeType == "planned")
                rtType = 1;
            string procedure = ".STP_ROUTE_SAVE.SP_DELETE_PORTAL_ROUTES";
            if (userSchema == UserSchema.Sort)
                procedure = ".STP_ROUTE_SAVE.SP_DELETE_SORT_ROUTES";

            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + procedure,
                 parameter =>
                 {
                     parameter.AddWithValue("p_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ROUTE_TYPE", rtType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_Affected_Rows", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = records.GetInt32("p_Affected_Rows");
                 });
            return result;
        }
        #endregion

        #region UpdateCloneHistoricAppRoute
        public static List<RoutePart> UpdateCloneHistoricAppRoute(UpdateHistoricCloneRoute updateHistoricClone)
        {
            List<RoutePart> routePartList;
            var routeJson = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routeJson,
                updateHistoricClone.UserSchema + ".STP_HISTORIC_ROUTE.SP_GET_ROUTE_LIST_JSON",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_REF_NO", updateHistoricClone.ContRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", updateHistoricClone.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID", updateHistoricClone.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    routeJson = records.GetStringOrDefault("ROUTE_JSON");
                });
            routePartList = JsonConvert.DeserializeObject<List<RoutePart>>(routeJson);
            List<RoutePart> routePartListnew = new List<RoutePart>();
            foreach (var routePart in routePartList)
            {
                routePartListnew.Add(GetInnerText(routePart));
            }
            return routePartListnew;
        }
        #endregion

        #endregion

        #endregion

        #region Common Methods

        #region ProcessRoutePath
        /// <summary>
        /// add route segment and route point list in an array
        /// </summary>
        /// <param name="routePathList"></param>
        /// <returns></returns>
        private static List<RoutePath> ProcessRoutePath(List<RoutePath> routePathList)
        {
            string str1 = null, str2 = null, str3 = null;

            routePathList.ForEach(
                routePath =>
                {
                    routePath.RouteSegmentList.ForEach(
                        routeSegment =>
                        {
                            routeSegment.SegmentType = GetSegmentType(routeSegment.SegmentType);
                            routeSegment.RouteLinkArray.RouteLinkObj = (routeSegment.RouteLinkList.Count > 0) ? routeSegment.RouteLinkList.ToArray() : null;
                            if (routeSegment.RouteAnnotationsList != null)
                            {
                                routeSegment.RouteAnnotationsList.ForEach(
                                    routeAnnotation =>
                                    {
                                        routeAnnotation.AnnotText = DecodeXml(routeAnnotation.AnnotText);
                                        routeAnnotation.AnnotText = EncodeXml(routeAnnotation.AnnotText);
                                        routeAnnotation.AnnotText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <annotation:Text xmlns:annotation=\"http://www.esdal.com/schemas/core/annotation\">" + routeAnnotation.AnnotText + "</annotation:Text>";
                                        routeAnnotation.AnnotationContactArray.AnnotationContactObj =
                                        (routeAnnotation.AnnotationContactList != null && routeAnnotation.AnnotationContactList.Count > 0) ? routeAnnotation.AnnotationContactList.ToArray() : null;
                                    });
                                routeSegment.RouteAnnotationArray.RouteAnnotationObj =
                                (routeSegment.RouteAnnotationsList.Count > 0) ? routeSegment.RouteAnnotationsList.ToArray() : null;
                            }
                        });
                    routePath.RoutePointList.ForEach(
                        routePoint =>
                        {
                            str1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<route:Text xmlns:route=\"http://www.esdal.com/schemas/core/route\">";
                            str2 = routePoint.WayText;
                            str3 = "</route:Text>";
                            routePoint.WayText = str1 + str2 + str3;
                            routePoint.IsAnchorPoint = 0;
                            routePoint.PointType += 239001;
                            if (routePoint.PointType >= 3)  //Via Points
                                routePoint.IsAnchorPoint = 1;
                        });
                    routePath.RouteSegmentArray.SegmentObj = (routePath.RouteSegmentList.Count > 0) ? routePath.RouteSegmentList.ToArray() : null;
                    routePath.RoutePointArray.RoutePointObj = (routePath.RoutePointList.Count > 0) ? routePath.RoutePointList.ToArray() : null;
                });
            return routePathList;
        }
        #endregion

        #region Convert LinkId
        /// <summary>
        /// linkId > 300000000000 ? 300000000000 : linkId > 200000000000 ? 200000000000 : linkId > 100000000000 ? 100000000000 : 0;
        private static long ConvertLinkID(long linkId)
        {
            long valToSub = 0;
            if (linkId > 300000000000)
                valToSub = 300000000000;
            else if (linkId > 200000000000 && linkId <= 300000000000)
                valToSub = 200000000000;
            else if (linkId > 100000000000 && linkId <= 200000000000)
                valToSub = 100000000000;
            return valToSub > 0 ? (linkId - valToSub) / 10 : linkId;
        }
        #endregion

        #region Get Segment Type
        public static int GetSegmentType(int segmentType)
        {
            /*
             *  1 - 231001 normal
                2 - 231002 override
                3 - 231003 offroad
                4 - 231004 shunt
                5 - 231005 uturn
                6 - 231006 broken
                7 - 231007 assumed
                8 - 231008 confirmed
             * */
            if (segmentType < 231001)
            {
                switch (segmentType)
                {
                    case 1:
                        segmentType = 231001;
                        break;
                    case 2:
                        segmentType = 231002;
                        break;
                    case 3:
                        segmentType = 231003;
                        break;
                    case 4:
                        segmentType = 231004;
                        break;
                    case 5:
                        segmentType = 231005;
                        break;
                    case 6:
                        segmentType = 231006;
                        break;
                    case 7:
                        segmentType = 231007;
                        break;
                    case 8:
                        segmentType = 231008;
                        break;
                }
            }
            else if (segmentType >= 231001)
            {
                switch (segmentType)
                {
                    case 231001:
                        segmentType = 1;
                        break;
                    case 231002:
                        segmentType = 2;
                        break;
                    case 231003:
                        segmentType = 3;
                        break;
                    case 231004:
                        segmentType = 4;
                        break;
                    case 231005:
                        segmentType = 5;
                        break;
                    case 231006:
                        segmentType = 6;
                        break;
                    case 231007:
                        segmentType = 7;
                        break;
                    case 231008:
                        segmentType = 8;
                        break;
                }
            }
            return segmentType;
        }
        #endregion

        #region Get Inner Text
        private static RoutePart GetInnerText(RoutePart routePart)
        {
            routePart.RoutePathList.ForEach(
                routePath =>
                {
                    routePath.RoutePointList = routePath.RoutePointList.OrderBy(x => x.PointType).ThenBy(item => item.RoutePointNo).ToList();
                    routePath.RouteSegmentList.ForEach(
                        routeSegment =>
                        {
                            if (routeSegment.RouteAnnotationsList != null)
                            {
                                routeSegment.RouteAnnotationsList.ForEach(
                                    routeAnnotation =>
                                    {
                                        routeAnnotation.AnnotText = GetXmlInnerText(routeAnnotation.AnnotText, "annotation");
                                    });
                            }
                            else
                            {
                                routeSegment.RouteAnnotationsList = new List<RouteAnnotation>();
                            }
                            if (routeSegment.RouteLinkList == null)
                            {
                                routeSegment.RouteLinkList = new List<RouteLink>();
                            }
                        });
                    routePath.RoutePointList.ForEach(
                        routePoint =>
                        {
                            routePoint.WayText = GetXmlInnerText(routePoint.WayText, "route");
                        });
                });
            return routePart;
        }

        private static string GetXmlInnerText(string xmlString, string type)
        {
            string innerText = string.Empty;
            if (!string.IsNullOrWhiteSpace(xmlString))
            {
                xmlString = xmlString.Trim('"').Replace("\\\"", "\"");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);
                var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nsManager.AddNamespace(type, xmlDoc.DocumentElement.NamespaceURI);
                innerText = xmlDoc.InnerText;
            }
            return innerText;
        }

        #endregion

        #region Get Way Text
        private static string GetWayText(DataAccess.Mappers.IRecord records, string field)
        {
            string strRes = "";
            string xmlWayText = records.GetStringOrDefault(field);

            if (xmlWayText != null && xmlWayText != "")
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(xmlWayText);

                var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);

                nsManager.AddNamespace("route", xmlDoc.DocumentElement.NamespaceURI);

                strRes = xmlDoc.InnerText;
            }
            return strRes;
        }
        #endregion

        #endregion

        #region Annotation related functions

        #region Save Annotation
        public static bool SaveRouteAnnotation(RoutePart routePart, int type, string userSchema = UserSchema.Portal)
        {
            bool deleteFlag = false;
            bool saveFlag = false;
            Serialization serialization = new Serialization();
            routePart = serialization.DeserializeRoutePart(routePart);
            deleteFlag = DeleteAllAnnotation(routePart.RoutePartDetails.RouteId, userSchema);
            if (routePart.RoutePathList != null)
            {
                routePart.RoutePathList.ForEach(
                routePath =>
                {
                    routePath.RouteSegmentList.ForEach(
                    routeSegment =>
                    {
                        routeSegment.RouteAnnotationsList.ForEach(
                        routeAnnotation =>
                        {
                            routeAnnotation.AnnotText = DecodeXml(routeAnnotation.AnnotText);
                            routeAnnotation.AnnotText = EncodeXml(routeAnnotation.AnnotText);
                            routeAnnotation.AnnotText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <annotation:Text xmlns:annotation=\"http://www.esdal.com/schemas/core/annotation\">" + routeAnnotation.AnnotText + "</annotation:Text>";
                            routeAnnotation.SegmentId = routeSegment.SegmentId;
                            routeAnnotation.SegmentNo = routeSegment.SegmentNo;
                        });
                        saveFlag = InsertAnnotation(routeSegment.RouteAnnotationsList, type, userSchema);
                    });
                });
            }
            return saveFlag;
        }
        #endregion

        #region Get Annotation
        /*private static List<RouteAnnotation> GetAnnotation(int segmentNo, long segmentId, int type, string userSchema = UserSchema.Portal)
        {
            List<RouteAnnotation> routeAnnotationsList = new List<RouteAnnotation>();
            string schema = userSchema + ".STP_ANNOTATIONS.SP_GET_ROUTE_ANNOTATION";
            if (type == 0)
            {
                schema = userSchema + ".STP_ANNOTATIONS.SP_GET_ROUTE_ANNOTATION";
            }
            long annotationID = -1;
            List<RouteAnnotation> routeAnnotationsListDummy = new List<RouteAnnotation>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeAnnotationsListDummy,
                schema,
                parameter =>
                {
                    parameter.AddWithValue("P_Segment_ID", segmentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEGMENT_NO", segmentNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    long nAnotID = records.GetLongOrDefault("ANNOTATION_ID");
                    if (annotationID != nAnotID)
                    {
                        instance.AnnotationID = nAnotID;
                        instance.AnnotType = records.GetInt32OrDefault("ANNOT_TYPE");
                        instance.AnnotText = records.GetStringOrDefault("ANNOT_TEXT");
                        instance.AssocType = (long)records.GetInt32OrDefault("ASSOC_TYPE");
                        instance.Northing = records.GetInt32OrDefault("NORTHING");
                        instance.Easting = records.GetInt32OrDefault("EASTING");
                        instance.Geometry = records.GetGeometryOrNull("GEOMETRY");
                        string direct = records["DIRECTION"].ToString();
                        if (string.IsNullOrEmpty(direct))
                            instance.Direction = -1;
                        else
                            instance.Direction = Convert.ToInt16(direct);
                        instance.StructureEsrn = records.GetStringOrDefault("STRUCTURE_CODE");
                        instance.ConstraintEsrn = records.GetStringOrDefault("CONSTRAINT_CODE");
                        instance.LinkId = records.GetLongOrDefault("LINK_ID");
                        instance.LinearRef = records.GetInt32OrDefault("LINEAR_REF");
                        instance.IsBroken = records.GetShortOrDefault("IS_BROKEN");
                        instance.InRouteDescription = records.GetShortOrDefault("IN_ROUTE_DESCR");
                        instance.SegmentId = records.GetLongOrDefault("SEGMENT_ID");
                        annotationID = nAnotID;
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(instance.AnnotText);
                        var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                        nsManager.AddNamespace("annotation", xmlDoc.DocumentElement.NamespaceURI);
                        instance.AnnotText = EncodeXml(xmlDoc.InnerText);
                    }
                    if (segmentId != 0)
                    {
                        instance.AnnotationContactList = GetAnnotationContacts(nAnotID, type, userSchema);
                        routeAnnotationsList.Add(instance);
                    }
                });
            return routeAnnotationsList;
        }*/
        #endregion

        #region Get Annotation Contact
        /*private static List<AnnotationContact> GetAnnotationContacts(long annotationID, int type, string userSchema = UserSchema.Portal)
        {
            List<AnnotationContact> annotationContacts = new List<AnnotationContact>();
            List<AnnotationContact> annotationContactsDummy = new List<AnnotationContact>();
            string schema = userSchema + ".STP_ANNOTATIONS.SP_GET_ROUTE_ANNOTATION_CTACT";
            if (type == 0)
            {
                schema = userSchema + ".STP_ANNOTATIONS.SP_GET_ROUTE_ANNOTATION_CTACT";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                annotationContactsDummy,
                schema,
                parameter =>
                {
                    parameter.AddWithValue("P_ANNOT_ID", annotationID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.AnnotationId = annotationID;
                    instance.PhoneNumber = records.GetStringOrDefault("PHONENUMBER");
                    instance.ContactId = (long)Convert.ToInt32(records["CONTACT_ID"]);
                    instance.ContactNo = (long)records.GetShortOrDefault("CONTACT_NO");
                    instance.OrgName = Convert.ToString(records["ORGNAME"]);
                    instance.OrganizationId = (long)Convert.ToInt32(records["ORGANISATION_ID"]);
                    if (instance.ContactId > 0)
                        annotationContacts.Add(instance);
                });
            return annotationContacts;
        }*/
        #endregion

        #region Encode/Decode XML
        private static string DecodeXml(string txt)
        {
            return System.Net.WebUtility.HtmlDecode(txt);
        }
        private static string EncodeXml(string txt)
        {
            return System.Net.WebUtility.HtmlEncode(txt);
        }
        #endregion

        #region Delete All Annotations
        public static bool DeleteAllAnnotation(long routeId, string userSchema = UserSchema.Sort)
        {
            bool result = false;
            decimal res = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            routeId,
            userSchema + ".STP_ANNOTATIONS.SP_DELETE_ALL_ROUTE_ANNOTATION",
            parameter =>
            {
                parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_Resultset", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            records =>
            {
                res = records.GetDecimalOrDefault("AFFECTEDROW");
            });
            if (res > 0)
                result = true;
            return result;
        }
        #endregion

        #region Insert Annotation
        private static bool InsertAnnotation(List<RouteAnnotation> routeAnnotationLst, int type, string userSchema)
        {
            bool saveFlag = false;
            string schema = UserSchema.Portal + ".SP_INSERT_ROUTE_ANNOTATION";

            decimal result = 0;
            routeAnnotationLst.ForEach(annotation => { annotation.AnnotationContactArray.AnnotationContactObj = (annotation.AnnotationContactList.Count > 0) ? annotation.AnnotationContactList.ToArray() : null; });
            RouteAnnotationArray routeAnnotArrObj = new RouteAnnotationArray();
            routeAnnotArrObj.RouteAnnotationObj = routeAnnotationLst.ToArray();

            OracleCommand cmd = new OracleCommand();
            OracleParameter routeAnnotObjParam = cmd.CreateParameter();
            routeAnnotObjParam.OracleDbType = OracleDbType.Object;
            routeAnnotObjParam.UdtTypeName = "PORTAL.ROUTEANNOTATIONARRAY";
            routeAnnotObjParam.Value = routeAnnotationLst.Count > 0 ? routeAnnotArrObj : null;
            routeAnnotObjParam.ParameterName = "P_ROUTE_ANNOTATION_ARRAY";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
            schema,
                parameter =>
                {
                    parameter.AddWithValue("P_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SCHEMA", userSchema, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(routeAnnotObjParam);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    result = records.GetDecimalOrDefault("AFFECTEDROW");
                    if (result > 0)
                        saveFlag = true;
                });
            return saveFlag;
        }
        #endregion

        #endregion

        #region Save Map Usage
        public static int SaveMapUsageInfo(int userID, int organizationID, int type)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 UserSchema.Portal + ".UPDATE_USAGE_COUNT",
                 parameter =>
                 {
                     parameter.AddWithValue("P_USER_ID ", userID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ORGANIZATION_ID", organizationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_TYPE", type, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 (records) =>
                 {
                     result = (int)records.GetDecimalOrDefault("CNT");
                 });
            return result;
        }
        #endregion

        #region ListBrokenRouteDetails
        public static List<NotifRouteImport> ListBrokenRouteDetails(string contentReferenceNumber, string userSchema, long appRevisionID, long revisionID, long movementVersionID)
        {
            List<NotifRouteImport> listNotifRouteImport = new List<NotifRouteImport>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               listNotifRouteImport,
             userSchema + ".STP_BROKEN_ROUTES_REPLANNER.SP_GET_BROKEN_ROUTE_NAME",
               parameter =>
               {
                   parameter.AddWithValue("P_CONTENT_REFNO ", contentReferenceNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input);// For notification
                   parameter.AddWithValue("P_APP_REVISION_ID ", appRevisionID, OracleDbType.Long, ParameterDirectionWrap.Input);//For SO application of sort and portal
                   parameter.AddWithValue("P_REVISION_ID ", revisionID, OracleDbType.Long, ParameterDirectionWrap.Input);//For SO application of sort and portal
                   parameter.AddWithValue("P_VERSION_ID ", movementVersionID, OracleDbType.Long, ParameterDirectionWrap.Input);//For SO application of sort and portal
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.RouteName = records.GetStringOrDefault("PART_NAME");
                       instance.IsReplan = records.GetFieldType("IS_REPLAN").Name == "Decimal" ? Convert.ToInt32(records.GetDecimalOrDefault("IS_REPLAN")) : records.GetInt32OrDefault("IS_REPLAN");
                   }
           );
            return listNotifRouteImport;
        }
        #endregion

        #region ListRouteImportDetails
        public static List<NotifRouteImport> ListRouteImportDetails(string contentReferenceNo)
        {
            List<NotifRouteImport> objListRt = new List<NotifRouteImport>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                   objListRt,
                     UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_ROUTE_IMPORT",
                   parameter =>
                   {
                       parameter.AddWithValue("RI_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                       parameter.AddWithValue("RI_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                    (records, instance) =>
                    {
                        instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    }
               );
            return objListRt;
        }
        #endregion

        #region GetRoutePathId
        public static long GetRoutePathId(long routeId, int isLib, string userSchema)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   result,
                  userSchema + ".SP_GET_ROUTE_PATH_ID",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ROUTE_ID", routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_IS_LIB", isLib, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                    record =>
                    {
                        result = record.GetLongOrDefault("ROUTE_PATH_ID");
                    }
               );
            return result;
        }
        #endregion

        #region Affected Constraints
        internal static List<RouteConstraints> GetAffectedConstraintList(int routePartId, RoutePart routePart, string userSchema)
        {
            try
            {

                List<long?> routePointLinkId = new List<long?>();

                Dictionary<long, int[]> linkList = new System.Collections.Generic.Dictionary<long, int[]>();

                List<long?> routeLinkId = new List<long?>();

                long linkId = 0;
                int? dir, ptlrs, fLrs;
                int ptType = 0;

                bool result = false;

                int[] idInfo = null;

                foreach (RoutePath routePathObj in routePart.RoutePathList)
                {
                    //Start 
                    foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 0).ToList())
                    {
                        routePointLinkId.Add(routePointObj.LinkId);

                        idInfo = new int[3];

                        idInfo[0] = (int)routePointObj.Lrs;      //LRS measure of point
                        idInfo[1] = (int)routePointObj.Direction; //direction of link Id
                        idInfo[2] = 0;  //point type 0 for start

                        linkList.Add(routePointObj.LinkId, idInfo);
                    }

                    //Middle portion
                    foreach (RouteSegment routeSegObj in routePathObj.RouteSegmentList)
                    {
                        foreach (RouteLink routeLinkObj in routeSegObj.RouteLinkList.OrderBy(t => t.LinkNo))
                        {
                            routeLinkId.Add(routeLinkObj.LinkId);
                        }
                    }

                    //End 
                    foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 1).ToList())
                    {
                        routePointLinkId.Add(routePointObj.LinkId);

                        idInfo = new int[3];

                        idInfo[0] = (int)routePointObj.Lrs; //LRS measure of point
                        idInfo[1] = (int)routePointObj.Direction;   //direction of link id
                        idInfo[2] = 1;  // point type 1 for end

                        linkList.Add(routePointObj.LinkId, idInfo);
                    }

                }

                var tmpLinkId = routeLinkId.Except(routePointLinkId);

                routeLinkId = tmpLinkId.ToList();

                OracleCommand cmd = new OracleCommand();

                #region Portion to check all constraints at all other point's
                //creating associative array parameter to pass to stored procedure
                OracleParameter param = new OracleParameter(); // cmd.CreateParameter();
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = routeLinkId.ToArray(); // change when the testing is completed
                param.Size = routeLinkId.ToArray().Length;

                List<RouteConstraints> routeConst = getConstraintInfoList(routePartId, param, userSchema);

                #endregion

                #region find constraints at start and end point's
                //Portion to check whether the structure's at point are affected or not.
                OracleParameter pointParam = new OracleParameter();
                pointParam.OracleDbType = OracleDbType.Int32;
                pointParam.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                pointParam.Value = linkList.Keys.ToArray();
                pointParam.Size = linkList.Keys.ToArray().Length;

                List<RouteConstraints> routePointConst = getConstraintInfoList(routePartId, pointParam, userSchema);

                #endregion


                List<RouteConstraints> routePointConstList = new List<RouteConstraints>();

                foreach (RouteConstraints routeConstObj in routePointConst)
                {
                    foreach (ConstraintReferences constRef in routeConstObj.ConstraintRefrences)
                    {
                        linkId = constRef.constLink;
                        if (linkList.ContainsKey(linkId))
                        {
                            fLrs = (int)constRef.FromLinearRef;

                            ptlrs = linkList[linkId][0];
                            dir = linkList[linkId][1];
                            ptType = linkList[linkId][2];

                            result = IsAffectedPortion(dir, ptlrs, fLrs, ptType);

                            if (result)
                            {
                                routePointConstList.Add(routeConstObj);
                            }
                        }
                    }
                }

                if (routePointConstList.Count > 0)
                {
                    routeConst.AddRange(routePointConstList);
                }

                return routeConst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static List<RouteConstraints> getConstraintInfoList(int routePartId, OracleParameter param, string userSchema)
        {

            List<RouteConstraints> routeConst = new List<RouteConstraints>();


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeConst,
                userSchema + ".STP_LINK_ID_ARRAY.SP_SELECT_CNSTRT_INSTANT",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(param);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                        instance.ConstraintCode = records.GetStringOrDefault("CONSTRAINT_CODE");
                        instance.ConstraintName = records.GetStringOrDefault("CONSTRAINT_NAME");
                        instance.ConstraintType = records.GetStringOrDefault("CONSTRAINT_TYPE");
                        instance.TopologyType = records.GetStringOrDefault("TOPOLOGY_TYPE");
                        instance.TraversalType = records.GetStringOrDefault("TRAVERSAL_TYPE");
                        //retrieving geometric references related to constraint's
                        instance.ConstraintRefrences = new List<ConstraintReferences>();

                        instance.ConstraintRefrences = getConstraintGeoDetails(instance.ConstraintId, userSchema);

                        instance.ConstraintGeometry = records.GetGeometryOrNull("GEOMETRY");

                        instance.ConstraintValue = new ConstraintValues();

                        instance.ConstraintSuitability = records.GetStringOrDefault("SUITABILITY"); //variable to store constraint suitability

                        instance.ConstraintValue.GrossWeight = (int)records.GetInt32OrDefault("GROSS_WEIGHT");
                        instance.ConstraintValue.AxleWeight = (int)records.GetInt32OrDefault("AXLE_WEIGHT");
                        instance.ConstraintValue.MaxHeight = records.GetSingleOrDefault("MAX_HEIGHT_MTRS");
                        instance.ConstraintValue.MaxLength = records.GetSingleOrDefault("MAX_LEN_MTRS");
                        instance.ConstraintValue.MaxWidth = records.GetSingleOrDefault("MAX_WIDTH_MTRS");

                        instance.CautionList = getCautionList(instance.ConstraintId, userSchema);
                    }
            );

            return routeConst;
        }

        public static List<RouteCautions> getCautionList(long constraintId, string userSchema)
        {
            try
            {
                List<RouteCautions> routeCaution = new List<RouteCautions>();

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    routeCaution,
                    userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_CAUTIONS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ROUTE_PART_ID", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONSTRAINT_ID", constraintId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ROUTE_TYPE", null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.CautionId = records.GetLongOrDefault("CAUTION_ID");
                            instance.CautionConstraintId = records.GetLongOrDefault("CONSTRAINT_ID");
                            instance.CautionName = records.GetStringOrDefault("CAUTION_NAME");
                            instance.cautDescription = StringExtraction.XmlStringExtractor(records.GetStringOrDefault("SPECIFIC_ACTION"), "SpecificAction");

                            instance.CautionConstraintValue = new ConstraintValues();

                            instance.CautionConstraintValue.GrossWeight = (long)records.GetDoubleOrDefault("GROSS_WEIGHT");
                            instance.CautionConstraintValue.AxleWeight = (long)records.GetDoubleOrDefault("AXLE_WEIGHT");
                            instance.CautionConstraintValue.MaxHeight = (Single)records.GetDoubleOrDefault("MAX_HEIGHT");
                            instance.CautionConstraintValue.MaxLength = (Single)records.GetDoubleOrDefault("MAX_LENGTH");
                            instance.CautionConstraintValue.MaxWidth = (Single)records.GetDoubleOrDefault("MAX_WIDTH");
                            instance.CautionConstraintValue.MinSpeed = (Single)records.GetSingleOrDefault("MIN_SPEED");
                        }
                );

                return routeCaution;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static List<ConstraintReferences> getConstraintGeoDetails(long constId, string userSchema = UserSchema.Portal)
        {
            int temp1 = 0;

            List<ConstraintReferences> routeConstraintRef = new List<ConstraintReferences>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeConstraintRef,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_R_GET_CONSTRAINT_LINKS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONSTRAINT_ID", constId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        temp1 = (int)records.GetDecimalOrDefault("CONT_ONE");
                        if (temp1 != 0)
                        {
                            instance.constLink = records.GetLongOrDefault("LINK_ID");

                            instance.ToEasting = records.GetInt32OrDefault("TO_EASTING");

                            instance.ToNorthing = records.GetInt32OrDefault("TO_NORTHING");

                            instance.FromEasting = records.GetInt32OrDefault("FROM_EASTING");

                            instance.FromNorthing = records.GetInt32OrDefault("FROM_NORTHING");

                            instance.ToLinearRef = records.GetInt32OrDefault("TO_LINEAR_REF");

                            instance.FromLinearRef = records.GetInt32OrDefault("FROM_LINEAR_REF");

                            try
                            {
                                instance.IsPoint = records.GetInt16OrDefault("IS_POINT") == 1 ? true : false;
                            }
                            catch
                            {
                                instance.IsPoint = records.GetInt16Nullable("IS_POINT") == 1 ? true : false;
                            }

                            instance.Direction = records.GetInt16Nullable("DIRECTION");
                        }
                    }
            );

            return routeConstraintRef;

        }
        #endregion

        #region Affected structures

        public static List<StructureInfo> GetAffectedStructureList(int routePartId, RoutePart routePart, string userSchema)
        {
            List<long?> routePointLinkId = new List<long?>();

            Dictionary<long, int[]> linkList = new System.Collections.Generic.Dictionary<long, int[]>();

            List<long?> routeLinkId = new List<long?>();


            int[] idInfo = null;

            foreach (RoutePath routePathObj in routePart.RoutePathList)
            {
                //Start 
                foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 0).ToList())
                {
                    routePointLinkId.Add(routePointObj.LinkId);

                    idInfo = new int[3];

                    idInfo[0] = (int)routePointObj.Lrs;      //LRS measure of point
                    idInfo[1] = (int)routePointObj.Direction; //direction of link Id
                    idInfo[2] = 0;  //point type 0 for start

                    linkList.Add(routePointObj.LinkId, idInfo);
                }

                //Middle portion
                foreach (RouteSegment routeSegObj in routePathObj.RouteSegmentList)
                {
                    foreach (RouteLink routeLinkObj in routeSegObj.RouteLinkList.OrderBy(t => t.LinkNo))
                    {
                        routeLinkId.Add(routeLinkObj.LinkId);
                    }
                }

                //End 
                foreach (RoutePoint routePointObj in routePathObj.RoutePointList.Where(t => t.PointType == 1).ToList())
                {
                    routePointLinkId.Add(routePointObj.LinkId);

                    idInfo = new int[3];

                    idInfo[0] = (int)routePointObj.Lrs; //LRS measure of point
                    idInfo[1] = (int)routePointObj.Direction;   //direction of link id
                    idInfo[2] = 1;  // point type 1 for end

                    linkList.Add(routePointObj.LinkId, idInfo);
                }

            }

            var linkId = routeLinkId.Except(routePointLinkId);

            routeLinkId = linkId.ToList();

            List<StructureInfo> structAtFlags = new List<StructureInfo>();

            OracleCommand cmd = new OracleCommand();

            #region Portion to check all structures at all other point's
            //creating associative array parameter to pass to stored procedure
            OracleParameter param = new OracleParameter(); // cmd.CreateParameter();
            param.OracleDbType = OracleDbType.Int32;
            param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            param.Value = routeLinkId.ToArray(); // change when the testing is completed
            param.Size = routeLinkId.ToArray().Length;

            List<StructureInfo> structInfoList = GetStructureInfoList(routePartId, param, userSchema);

            #endregion

            #region find structures at start and end point's
            //Portion to check whether the structure's at point are affected or not.
            OracleParameter pointParam = new OracleParameter();
            pointParam.OracleDbType = OracleDbType.Int32;
            pointParam.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            pointParam.Value = linkList.Keys.ToArray();
            pointParam.Size = linkList.Keys.ToArray().Length;

            List<StructureInfo> structAtPoints = GetStructureInfoAtPoint(routePartId, pointParam, linkList, userSchema);

            #endregion

            if (structAtPoints.Count != 0)
            {
                structInfoList.AddRange(structAtPoints);
            }



            return structInfoList;
        }
        private static List<StructureInfo> GetStructureInfoList(int routePartId, OracleParameter param, string userSchema)
        {
            List<StructureInfo> tmpStructInfoList = new List<StructureInfo>();
            List<StructureInfo> structInfoList = new List<StructureInfo>();

            long tmpOrgId1 = 0, tmpOrgId2 = 0;
            long structId = 0, structIdTemp = 0, sectionId = 0;
            string appraisalTemp = null;
            string str = null;

            StructureInfo StructInfo = null;


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    tmpStructInfoList,
                    userSchema + ".STP_LINK_ID_ARRAY.SP_SELECT_STRUCT_INSTANT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ROUTE_PATH_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.Add(param);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        structId = records.GetLongOrDefault("STRUCTURE_ID");
                        sectionId = records.GetLongOrDefault("SECTION_ID");
                        tmpOrgId2 = records.GetLongOrDefault("OWNER_ID"); //owner of the structure

                        if (structId != structIdTemp)
                        {
                            StructInfo = new StructureInfo();

                            structInfoList.Add(StructInfo);

                            StructInfo.StructureId = structId; // saving structure id

                            StructInfo.SectionId = sectionId; //saving section id

                            StructInfo.SectionNo = records.GetLongOrDefault("SECTION_NO");

                            StructInfo.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");

                            StructInfo.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");

                            StructInfo.StructureClass = records.GetStringOrDefault("SECTION_CLASS");

                            StructInfo.StructureDescr = records.GetStringOrDefault("DESCRIPTION");

                            StructInfo.PointGeometry = records.GetGeometryOrNull("POINT_GEOMETRY");

                            StructInfo.LineGeometry = records.GetGeometryOrNull("LINE_GEOMETRY");

                            str = StructInfo.PointGeometry.AsText;

                            StructInfo.Point = StructInfo.PointGeometry.sdo_point;

                            StructInfo.Suitability.Add(records.GetStringOrDefault("SECTION_SUITABILITY"));

                            tmpOrgId1 = tmpOrgId2;

                            structIdTemp = structId;

                        }
                        if (structId == structIdTemp && tmpOrgId1 != tmpOrgId2)
                        {
                            tmpOrgId1 = records.GetLongOrDefault("OWNER_ID");

                            appraisalTemp = records.GetStringOrDefault("SECTION_SUITABILITY");

                            StructInfo.Suitability.Add(appraisalTemp);
                        }

                    }
                );

            return structInfoList;
        }

        private static List<StructureInfo> GetStructureInfoAtPoint(int routePartId, OracleParameter param, Dictionary<long, int[]> linkList, string userSchema)
        {
            List<StructureInfo> tmpStructInfoList = new List<StructureInfo>();
            List<StructureInfo> structInfoList = new List<StructureInfo>();
            bool result = false;
            long tmpOrgId1 = 0, tmpOrgId2 = 0;
            long structId = 0, structIdTemp = 0, sectionId = 0, linkId = 0;
            int? dir, ptlrs, fLrs;
            int ptType = 0;

            string appraisalTemp = null;
            string str = null;

            StructureInfo StructInfo = null;


            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    tmpStructInfoList,
                    userSchema + ".STP_LINK_ID_ARRAY.SP_SELECT_STRUCT_INSTANT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_ROUTE_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.Add(param);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        structId = records.GetLongOrDefault("STRUCTURE_ID");
                        sectionId = records.GetLongOrDefault("SECTION_ID");
                        tmpOrgId2 = records.GetLongOrDefault("OWNER_ID"); //owner of the structure

                        linkId = records.GetLongOrDefault("LINK_ID");
                        fLrs = records.GetInt32OrDefault("FROM_LINEAR_REF");
                        if (linkList.ContainsKey(linkId))
                        {
                            ptlrs = linkList[linkId][0];
                            dir = linkList[linkId][1];
                            ptType = linkList[linkId][2];

                            result = IsAffectedPortion(dir, ptlrs, fLrs, ptType);
                        }
                        if (result)
                        {
                            if (structId != structIdTemp)
                            {
                                StructInfo = new StructureInfo();

                                structInfoList.Add(StructInfo);

                                StructInfo.StructureId = structId; // saving structure id

                                StructInfo.SectionId = sectionId; //saving section id

                                StructInfo.SectionNo = records.GetLongOrDefault("SECTION_NO");

                                StructInfo.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");

                                StructInfo.StructureCode = records.GetStringOrDefault("STRUCTURE_CODE");

                                StructInfo.StructureClass = records.GetStringOrDefault("SECTION_CLASS");

                                StructInfo.StructureDescr = records.GetStringOrDefault("DESCRIPTION");

                                StructInfo.PointGeometry = records.GetGeometryOrNull("POINT_GEOMETRY");

                                StructInfo.LineGeometry = records.GetGeometryOrNull("LINE_GEOMETRY");

                                str = StructInfo.PointGeometry.AsText;

                                StructInfo.Point = StructInfo.PointGeometry.sdo_point;

                                StructInfo.Suitability.Add(records.GetStringOrDefault("SECTION_SUITABILITY"));

                                tmpOrgId1 = tmpOrgId2;

                                structIdTemp = structId;

                            }
                            if (structId == structIdTemp && tmpOrgId1 != tmpOrgId2)
                            {
                                tmpOrgId1 = records.GetLongOrDefault("OWNER_ID");

                                appraisalTemp = records.GetStringOrDefault("SECTION_SUITABILITY");

                                StructInfo.Suitability.Add(appraisalTemp);
                            }
                        }

                    }
                );

            return structInfoList;
        }

        private static bool IsAffectedPortion(int? direction, int? ptLRS, int? StrCnstrFLRS, int pointType)
        {
            if (direction == 0)
            {
                if (pointType == 0)
                {
                    if (ptLRS >= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (ptLRS <= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (direction == 1)
            {
                if (pointType == 0)
                {
                    if (ptLRS <= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (ptLRS >= StrCnstrFLRS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region UpdateNotifPlanRoute
        public static bool UpdateNotifPlanRoute(int RoutePartId, string contentrefno, int RoutePartNo, int ImportVeh, int Flag)
        {
            bool status = false;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   RoutePartId,
                  UserSchema.Portal + ".STP_NOTIFICATION.SP_UPDATE_PLAN_ROUTE",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ROUTEPARTID", RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_CONTENTREFNO", contentrefno, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_IMPORTVEH", ImportVeh, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ROUTEPART_NO", RoutePartNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_FLAG", Flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                   record =>
                   {
                       status = true;
                   }
                   );
            return status;
        }
        #endregion

        #region Verify Application RouteName
        public static int VerifyApplicationRouteName(ApplicationRouteNameParams objApplicationRouteNameParams)
        {
            int count = 1;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                 count,
                 objApplicationRouteNameParams.UserSchema + ".STP_ROUTE_GENERATOR.SP_APPL_ROUTE_NAME_VALIDATION",
                 parameter =>
                 {
                     parameter.AddWithValue("p_ROUTE_NAME", objApplicationRouteNameParams.RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_REVISION_ID", objApplicationRouteNameParams.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_CONTENT_REF_NO", objApplicationRouteNameParams.ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ROUTE_FOR", objApplicationRouteNameParams.RouteFor, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     count = Convert.ToInt32(records.GetDecimalOrDefault("COUNT"));
                 });
            return count;

        }
        #endregion

        #region GetRouteDetails
        public static List<RoutePoint> GetRouteDetails(string contentReferenceNo)
        {
            List<RoutePoint> objDetails = new List<RoutePoint>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                   objDetails,
                  UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_GET_ROUTE_ADDR_DETAILS",
                   parameter =>
                   {
                       parameter.AddWithValue("P_CONTENT_REF_NO", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                  (records, instance) =>
                  {
                      instance.PointType = records.GetInt32OrDefault("ROUTE_POINT_TYPE");
                      instance.PointDescr = records.GetStringOrDefault("DESCR");
                      instance.RoutePointNo = records.GetInt16OrDefault("ROUTE_POINT_NO");
                      instance.RoutePointId = Convert.ToInt32(records.GetLongOrDefault("ROUTE_POINT_ID"));
                  }
                   );
            return objDetails;

        }
        #endregion

        #region GetRoutePartsCount
        public static int GetRoutePartsCount(string contentReferenceNo)
        {
            int count = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   count,
                  UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_GET_ROUTEPARTCOUNT_RC",
                   parameter =>
                   {
                       parameter.AddWithValue("RC_CONTENT_REF_NO", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("RC_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                   record =>
                   {
                       count = (int)record.GetDecimalOrDefault("COUNT(ROUTE_PART_ID)");
                   }
                   );

            return count;
        }
        #endregion

        #region DeleteOldRoute
        public static int DeleteOldRoute(long newRoutePartId, string contentRefNo, int oldRoutePartId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  result,
                  UserSchema.Portal + ".STP_NOTIFICATION.SP_DEL_OLD_CLN_NOTIF_ROU_ID",
                   parameter =>
                   {
                       parameter.AddWithValue("p_NewRoutePart_ID", newRoutePartId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ContentRef", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_OldRoutePart_ID", oldRoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_Affected_Rows", null, OracleDbType.Int32, ParameterDirectionWrap.Output, 32767);
                   },
                   record =>
                   {
                       result = record.GetInt32OrDefault("p_Affected_Rows");
                   }
                   );
            return result;
        }
        #endregion

        #region UpdateRoutePartId
        public static int UpdateRoutePartId(int newRoutePartId, int oldRoutePartId, string contentRefNo)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_DELETE_SNROUTE",
                parameter =>
                {
                    parameter.AddWithValue("DL_NEW_ROUTEPART_ID", newRoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("DL_OLD_ROUTEPART_ID", oldRoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("DL_CONTENT_REF", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                },
                 record =>
                 {

                 });
            return result;
        }
        #endregion

        #region GetNotifRouteDetails
        public static List<ListRouteVehicleId> GetNotifRouteDetails(string contentReferenceNo)
        {
            List<ListRouteVehicleId> objDetails = new List<ListRouteVehicleId>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objDetails,
                  UserSchema.Portal + ".SP_GET_NOTIF_CLONE_ROUTEID",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                        instance.PartDescr = records.GetStringOrDefault("PART_NAME");
                        instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                        instance.RouteCount = (int)records.GetDecimalOrDefault(":B2");
                    }
            );
            return objDetails;
        }

        #endregion

        #region DeleteOldReturnLeg
        public static int DeleteOldReturnLeg(string contentReferenceNo)
        {
            int result = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                ".STP_NOTIFICATION_ROUTE.SP_DELETE_OLD_RET_ROUTE",
                parameter =>
                {
                    parameter.AddWithValue("DEL_CONTENT_REF_NO", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_AFFECTEDROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);

                },
                record =>
                {
                    result = record.GetInt32("P_AFFECTEDROWS");
                }
            );

            return result;
        }
        #endregion

        #region GetRoutePointsForReturnLeg
        public static List<RoutePoint> GetRoutePointsForReturnLeg(int libraryRouteId, long planRouteId)
        {
            List<RoutePoint> RLObj = new List<RoutePoint>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                RLObj,
               UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_GET_ROUTEVARIANT_RL",
                parameter =>
                {
                    parameter.AddWithValue("RL_LIB_ROUTE_ID ", libraryRouteId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("RL_PLAN_ROUTE_ID ", planRouteId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("RL_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.PointType = records.GetInt32OrDefault("ROUTE_POINT_TYPE");
                    instance.PointDescr = records.GetStringOrDefault("DESCR");
                    instance.PointGeom = records.GetGeometryOrNull("ROAD_POINT_GEOMETRY");
                }
            );
            return RLObj;
        }
        #endregion

        #region DeleteOldRouteForImport
        public static int DeleteOldRouteForImport(long newRoutePartId, string contentReferenceNo, int routePartNo)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  result,
                  UserSchema.Portal + ".SP_DEL_OLD_CLN_NOTIF_ROU_IMP",
                   parameter =>
                   {
                       parameter.AddWithValue("p_RoutePart_ID", newRoutePartId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ContentRef", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_RoutePart_NO", routePartNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);

                   },
                   record =>
                   {
                       result = Convert.ToInt32(record["FLAG"]);
                   }
                   );

            return result;
        }
        #endregion

        #region Route Import Scenarios

        #region SaveNotifRoute
        public static long SaveNotifRoute(int routePartId, int versionId, string contentRefNo, int routeType)
        {
            long routePartID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   routePartID,
                  UserSchema.Portal + ".STP_ROUTE_IMPORT.SP_LIB_TO_NOTIF_IMP",
                   parameter =>
                   {
                       parameter.AddWithValue("P_LIB_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_APPREV_ID ", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_CONTENT_REF", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                   record =>
                   {
                       routePartID = record.GetLongOrDefault("ROUTE_PART_ID");
                   });
            return routePartID;
        }
        #endregion

        #region SaveSNotifRoute
        public static long SaveSNotifRoute(int LIB_routepartId, string ContentRefNo)
        {
            long RoutePartID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   RoutePartID,
                  UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_IMPORT_SN_ROUTE",
                   parameter =>
                   {
                       parameter.AddWithValue("SN_CONTENT_REF_NO", ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("SN_LIB_ROUTE_ID", LIB_routepartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("SN_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                   record =>
                   {
                       RoutePartID = record.GetLongOrDefault("ROUTE_PART_ID");
                   }
                   );
            return RoutePartID;
        }
        #endregion

        #region SaveSOApplicationRoute
        public static long SaveSOAppImportRoute(int routePartId, int appRevId, int routeType, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".STP_ROUTE_IMPORT.SP_SO_OUTL_TO_APPL_IMP",
                parameter =>
                {
                    parameter.AddWithValue("P_LIB_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID ", appRevId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_TYPE ", 615001, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = record.GetLongOrDefault("PART_ID");
                 }
            );
            return result;
        }
        #endregion

        #region SaveVR1ApplicationRoute
        public static long ImportRouteFromLibrary(int routePartId, int versionId, int appRevId, int routeType, string contentRef, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            int? p_VersionId = null;
            if (versionId != 0)
            {
                p_VersionId = versionId;
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                           result,
                             userSchema + ".STP_ROUTE_IMPORT.SP_LIB_TO_NOTIF_IMP",
                           parameter =>
                           {
                               parameter.AddWithValue("P_LIB_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                               parameter.AddWithValue("P_VERSION_ID ", p_VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                               parameter.AddWithValue("P_APPREV_ID ", appRevId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                               parameter.AddWithValue("P_CONTENT_REF ", contentRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                               parameter.AddWithValue("P_ROUTE_TYPE ", 615002, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                               parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                           },
                            record =>
                            {
                                result = record.GetLongOrDefault("ROUTE_PART_ID");

                            }

                       );
            return result;
        }
        #endregion

        #region SaveRouteInRouteParts
        public static long SaveRouteInRouteParts(int routePartId, int appRevId, int versionId, string contentRef, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            int? p_VersionId = null;
            if (versionId != 0)
            {
                p_VersionId = versionId;
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_IMPORT.SP_ROUTEPART_TO_ROUTEPART_IMP",
                parameter =>
                {
                    parameter.AddWithValue("P_RPART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID ", appRevId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID ", p_VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENTREF", contentRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = record.GetLongOrDefault("ROUTE_PART_ID");

                 }

            );
            return result;
        }
        #endregion

        #region SaveRouteInAppParts
        public static long SaveRouteInAppParts(int routePartId, int appRevId, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_IMPORT.SP_R_APPL_TO_APPL_IMP",
                parameter =>
                {
                    parameter.AddWithValue("P_APPL_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID ", appRevId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = record.GetLongOrDefault("PART_ID");
                 }
            );
            return result;
        }
        #endregion

        #endregion

        #region GetRoutePartId
        public static long GetRoutePartId(string conRefNumber, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".SP_GET_ROUTE_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_CONT_REF_NUM", conRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },

                 record =>
                 {
                     result = record.GetLongOrDefault("ROUTE_PART_ID");
                 }
            );
            return result;
        }

        #endregion

        #region Get Route Points
        public static List<RoutePoint> GetRoutePointsDetails(int PlanRouteID)
        {
            List<RoutePoint> RLObj = new List<RoutePoint>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                RLObj,
               UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_GET_ROUTE_POINTS",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID ", PlanRouteID, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("RL_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.PointType = records.GetInt32OrDefault("ROUTE_POINT_TYPE");
                    instance.PointDescr = records.GetStringOrDefault("DESCR");
                    instance.PointGeom = records.GetGeometryOrNull("ROAD_POINT_GEOMETRY");
                }
            );
            return RLObj;
        }
        #endregion

        #region Authorized Route Part List fo SOA/Police
        /// <summary>
        /// SP reterives values from Route_parts table
        /// </summary>
        /// <param name="apprevisionId"></param>
        /// <returns></returns>
        public static List<AppRouteList> GetAuthorizedRoutePartList(long versionId, string userSchema)
        {
            List<AppRouteList> AppSORouteList = new List<AppRouteList>();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               AppSORouteList,
                   userSchema + ".STP_ROUTE_IMPORT.SP_AUTHORIZED_ROUTE_PART_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
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
                }
              );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Routes/GetAuthorizedRoutePartList,  Exception: " + ex​​​​);
            }
            return AppSORouteList;
        }
        #endregion

        #region Get Planned NEN Route List for SOA/Police
        public static List<AppRouteList> GetPlannedNenRouteList(long nenId, int userId, long inboxItemId, int orgId)
        {
            List<AppRouteList> nenRouteList = new List<AppRouteList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                nenRouteList,
                 UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_LIST_NENOTIFICATION_RPART",
                parameter =>
                {
                    parameter.AddWithValue("P_NEN_ID", nenId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_INBOX_ITEM_ID", inboxItemId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteID = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteDescription = records.GetStringOrDefault("PART_DESCR");
                    instance.RouteType = records.GetInt32OrDefault("ROUTE_STATUS").ToString();
                    instance.FromAddress = records.GetStringOrDefault("From_ADDRE");
                    instance.ToAddress = records.GetStringOrDefault("TO_ADDRE");
                }
            );
            return nenRouteList;
        }
        #endregion

        #region SO App Route List
        /// <summary>
        /// SP reterives values from Application parts table
        /// </summary>
        /// <param name="apprevisionId"></param>
        /// <returns></returns>
        public static List<AppRouteList> GetSoAppRouteList(long revisionId, string userSchema)
        {
            List<AppRouteList> soRouteList = new List<AppRouteList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           soRouteList,
               userSchema + ".STP_ROUTE_IMPORT.SP_APPL_LIST_ROUTE_PART",
            parameter =>
            {
                parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
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
                instance.FromAddress = records.GetStringOrDefault("FROM_ADDRESS");
                instance.ToAddress = records.GetStringOrDefault("TO_ADDRESS");
            }
          );
            return soRouteList;
        }
        #endregion

        #region ListImportedRouteForVR1
        public static List<AppRouteList> NotifVR1RouteList(long revisionId, string contRefNum, long versionId, string userSchema)
        {
            List<AppRouteList> routeList = new List<AppRouteList>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           routeList,
             userSchema + ".STP_ROUTE_IMPORT.SP_LIST_IMPORTED_RPART",
           parameter =>
           {
               parameter.AddWithValue("P_REV_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("P_CONT_REF_NUM", contRefNum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("P_VER_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   instance.RouteID = records.GetLongOrDefault("ROUTE_PART_ID");
                   instance.RouteName = records.GetStringOrDefault("PART_NAME");
                   instance.RouteDescription = records.GetStringOrDefault("PART_DESCR");
                   instance.FromAddress = records.GetStringOrDefault("FROM_ADDRESS");
                   instance.ToAddress = records.GetStringOrDefault("TO_ADDRESS");
                   instance.RouteType = "planned";
                   instance.PartNo = records.GetInt16OrDefault("route_part_no");
                   instance.ReturnRouteType = records.GetInt32OrDefault("ROUTE_TYPE");
               }
          );
            return routeList;
        }

        #endregion

        #region Candidate Route

        #region Candiadte Outline Route
        public static RoutePart GetCandidateOutlineRoute(long routePartId, string userSchema)
        {
            RoutePart routePart = new RoutePart();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routePart,
                userSchema + ".STP_CANDIDATE_ROUTE_PKG.SP_GET_CANDOUTLINERT",
                parameter =>
                {
                    parameter.AddWithValue("p_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    routePart.RoutePartDetails.PartGeometry = records.GetGeometryOrNull("PART_GEOMETRY");
                    routePart.RoutePartDetails.RouteDescr = records.GetStringOrDefault("part_descr");
                    routePart.RoutePartDetails.RouteId = records.GetLongOrDefault("PART_ID");
                    routePart.RoutePartDetails.RouteName = records.GetStringOrDefault("PART_NAME");
                });
            if (routePart.RoutePartDetails.RouteId != 0)    // PART_ID as routeId
            {
                RoutePath routePath = new RoutePath()
                {
                    RoutePointList = GetCandidateOutlineRoutePoint(routePart.RoutePartDetails.RouteId, userSchema)
                };
                routePart.RoutePathList.Add(routePath);
            }
            return routePart;
        }
        #endregion

        # region Candiadte Outline Route Point
        public static List<RoutePoint> GetCandidateOutlineRoutePoint(long? partId, string userSchema = UserSchema.Sort)
        {
            List<RoutePoint> routePoint = new List<RoutePoint>();
            long pointType = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routePoint,
                userSchema + ".STP_CANDIDATE_ROUTE_PKG.SP_GET_CANDOUTPOINTS",
                parameter =>
                {
                    parameter.AddWithValue("p_PartId", partId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RoutePointNo = records.GetInt16OrDefault("POINT_NO");
                    pointType = records.GetInt32OrDefault("POINT_TYPE");
                    instance.PointDescr = records.GetStringOrDefault("description");
                    instance.IsAnchorPoint = 0;
                    instance.PointType = (int)pointType - 239001;   // Setting Type of route point based on flag variable
                    if (instance.PointType == 3)
                        instance.IsAnchorPoint = 1;
                    //generating route-point-no's for way/via points which starts from 3 onwards in table way/via points starts from 1,2,3, which in database will be saved as 3,4,5
                    if ((instance.PointType == 3 || instance.PointType == 2) && instance.RoutePointNo > 2)
                        instance.RoutePointNo -= 2;
                    instance.PointGeom = records.GetGeometryOrNull("POINT_GEOMETRY");
                });
            return routePoint;
        }
        #endregion

        #endregion

        #region Broken Route Scenarios

        #region  Get Broken Route Points
        public static List<RoutePoint> GetBrokenRoutePoints(long routePathId, int is_lib, string userSchema)
        {
            List<RoutePoint> plannedRoutePoint = new List<RoutePoint>();
            int pointCount = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                plannedRoutePoint,
                userSchema + ".STP_BROKEN_ROUTES_REPLANNER.SP_GET_BROKEN_ROUTE_POINTS",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PATH_ID", routePathId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_LIB", is_lib, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RoutePointId = Convert.ToInt32(records.GetLongOrDefault("ROUTE_POINT_ID"));
                    instance.RoutePointNo = Convert.ToInt16(records.GetInt16OrDefault("ROUTE_POINT_NO"));
                    instance.PointType = records.GetInt32OrDefault("ROUTE_POINT_TYPE");
                    instance.RoutePathId = records.GetLongOrDefault("ROUTE_PATH_ID");
                    instance.LinkId = ConvertLinkID(records.GetLongOrDefault("DATALINKS_LINK_ID"));
                    instance.NewLinkId = ConvertLinkID(Convert.ToInt64(records.GetDecimalOrDefault("LINK_ID")));
                    instance.NewBeginNodeId = Convert.ToInt64(records.GetDecimalOrDefault("REF_IN_ID"));
                    instance.NewEndNodeId = Convert.ToInt64(records.GetDecimalOrDefault("NREF_IN_ID"));
                    instance.DistanceToNewLink = records.GetDecimalOrDefault("DISTANCE");
                    instance.RoadGeometry = records.GetGeometryOrNull("ROAD_GEOMETRY");
                    instance.Lrs = Convert.ToInt32(records.GetDecimalOrDefault("LINEAR_REF"));
                    pointCount++;
                }
                );
            return plannedRoutePoint;
        }
        #endregion

        #region  Get Broken Route Annotations
        public static List<RouteAnnotation> GetBrokenRouteAnnotations(long segmentId, int is_lib, string userSchema)
        {
            List<RouteAnnotation> plannedRouteAnnotations = new List<RouteAnnotation>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                plannedRouteAnnotations,
                userSchema + ".STP_BROKEN_ROUTES_REPLANNER.SP_GET_BROKEN_ROUTE_ANNOTATIONS",
                parameter =>
                {
                    parameter.AddWithValue("P_SEGMENT_ID", segmentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_LIB", is_lib, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.AnnotationID = Convert.ToInt16(records.GetLongOrDefault("ANNOTATION_ID"));
                    instance.LinkId = ConvertLinkID(records.GetLongOrDefault("LINK_ID"));
                    instance.NewLinkId = ConvertLinkID(Convert.ToInt64(records.GetDecimalOrDefault("NEW_LINK_ID")));
                    instance.DistanceToNewLink = records.GetDecimalOrDefault("DISTANCE");
                    instance.RoadGeometry = records.GetGeometryOrNull("ROAD_GEOMETRY");
                    instance.LinearRef = Convert.ToInt32(records.GetDecimalOrDefault("LINEAR_REF"));
                }
                );
            return plannedRouteAnnotations;
        }
        #endregion

        #region  GetBrokenRouteIds
        public static List<BrokenRouteList> GetBrokenRouteIds(GetBrokenRouteList getBrokenRouteList)
        {
            List<BrokenRouteList> brokenRouteList = new List<BrokenRouteList>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                brokenRouteList,
                getBrokenRouteList.UserSchema + (getBrokenRouteList.IsNen == 0 ?
                ".STP_BROKEN_ROUTES_REPLANNER.SP_GET_BROKEN_ROUTES_MAP" :
                ".STP_BROKEN_ROUTES_REPLANNER.SP_GET_NEN_BROKEN_ROUTES_MAP"),
                parameter =>
                {
                    if (getBrokenRouteList.IsNen == 0)
                    {
                        parameter.AddWithValue("p_ROUTE_PART_ID", getBrokenRouteList.RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_VERSION_ID", getBrokenRouteList.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_REVISION_ID", getBrokenRouteList.AppRevisonId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_LIB_ROUTE_ID", getBrokenRouteList.LibraryRouteId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_CONTENT_REF_NO", getBrokenRouteList.ConteRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_CAND_REVISION_ID", getBrokenRouteList.CandRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    }
                    else
                    {
                        parameter.AddWithValue("p_CONTENT_REF_NO", getBrokenRouteList.ConteRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ORG_ID", getBrokenRouteList.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_NEN_TYPE", getBrokenRouteList.IsNen, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//PDF-1, API-2
                    }
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.PlannedRouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.IsBroken = (int)records.GetDecimalOrDefault("IS_BROKEN");
                    instance.IsReplan = (int)records.GetDecimalOrDefault("IS_REPLAN");
                }
                );
            return brokenRouteList;
        }
        #endregion


        #region  GetBrokenRouteIds
        public static int CheckIsBroken(GetBrokenRouteList getBrokenRouteList)
        {
            int result = 0;
            try
            {


                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    getBrokenRouteList.UserSchema + ".STP_BROKEN_ROUTES_REPLANNER.SP_CHECK_BROKEN_ROUTE",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ROUTE_PART_ID", getBrokenRouteList.RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_CAND_REVISION_ID", getBrokenRouteList.CandRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {

                        result = (int)records.GetDecimalOrDefault("v_BROKEN_ROUTE_EXISTS");
                    }
                    );
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region Update Broken Route Path 
        public static bool UpdateBrokenRoutePath(UpdateBrokenRoutePathParam objUpdateBrokenRoutePathParam)
        {
            RouteSave = true;
            Serialization serialization = new Serialization();
            objUpdateBrokenRoutePathParam.RoutePathList = serialization.DeserializeRoutePath(objUpdateBrokenRoutePathParam.RoutePathList);
            objUpdateBrokenRoutePathParam.RoutePathList = ProcessRoutePath(objUpdateBrokenRoutePathParam.RoutePathList);
            RoutePathArray routePathObj = new RoutePathArray()
            {
                RoutePathObj = objUpdateBrokenRoutePathParam.RoutePathList.ToArray()
            };
            try
            {
                OracleCommand cmd = new OracleCommand();
                OracleParameter routePathObjParam = cmd.CreateParameter();
                routePathObjParam.OracleDbType = OracleDbType.Object;
                routePathObjParam.UdtTypeName = "PORTAL.ROUTEPATHARRAY";
                routePathObjParam.Value = objUpdateBrokenRoutePathParam.RoutePathList.Count > 0 ? routePathObj : null;
                routePathObjParam.ParameterName = "P_ROUTE_PATH_ARRAY";
                decimal result = 0;

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    UserSchema.Portal + ".STP_BROKEN_ROUTES_REPLANNER.SP_UPDATE_BROKEN_ROUTE_PATH",
                     parameter =>
                     {
                         parameter.AddWithValue("P_IS_LIB", objUpdateBrokenRoutePathParam.IsLib, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_SCHEMA", objUpdateBrokenRoutePathParam.UserSchema, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                         parameter.Add(routePathObjParam);
                         parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                     },
                     records =>
                     {
                         result = records.GetDecimalOrDefault("FLAG");
                     });
                if (result == 0)
                {
                    RouteSave = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RouteSave;
        }
        #endregion

        #region SetVerificationStatus
        public static int SetVerificationStatusBrknRts(VerificationStatusParams objVerificationStatusParams)
        {
            int res = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               res,
               objVerificationStatusParams.userSchema + ".STP_BROKEN_ROUTES_REPLANNER.SP_BROKEN_ROUTE_VERIFICATION",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", objVerificationStatusParams.routeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_LIB", objVerificationStatusParams.isLib, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REPLAN_STATUS", objVerificationStatusParams.replanStatus, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    res = Convert.ToInt32(records.GetDecimalOrDefault("v_result"));
                }
                );
            return res;
        }
        #endregion

        #endregion

        #region GetFavouriteRoutes
        public static List<RoutePartDetails> GetFavouriteRoutes(int organisationId, string userSchema)
        {
            List<RoutePartDetails> routeList = new List<RoutePartDetails>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeList,
                userSchema + ".SP_GET_ROUTE_FAV_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.RouteId = records.GetLongOrDefault("LIBRARY_ROUTE_ID");
                        instance.RouteName = records.GetStringOrDefault("ROUTE_NAME");
                        instance.RouteType = records.GetStringOrDefault("ROUTE_TYPE");
                    }
            );

            return routeList;
        }

        #endregion

        #region Save Non ESDAL Route
        public static long SaveNERoute(SaveNERouteParams saveNERoute, bool isAutoPlan)
        {
            RouteSave = true;
            long result = 0;
            int isVr1 = saveNERoute.IsVr1 ? 1 : 0;
            int isAutoPlanned = isAutoPlan ? 1 : 0;

            Serialization serialization = new Serialization();
            saveNERoute.RoutePart = serialization.DeserializeRoutePart(saveNERoute.RoutePart);

            saveNERoute.RoutePart.RoutePathList = ProcessRoutePath(saveNERoute.RoutePart.RoutePathList);

            RoutePathArray routePathObj = new RoutePathArray()
            {
                RoutePathObj = saveNERoute.RoutePart.RoutePathList.ToArray()
            };
            //Function needs to be updated with SP variables
            OracleCommand cmd = new OracleCommand();

            OracleParameter routePathObjParam = cmd.CreateParameter();
            routePathObjParam.OracleDbType = OracleDbType.Object;
            routePathObjParam.UdtTypeName = "PORTAL.ROUTEPATHARRAY";
            routePathObjParam.Value = saveNERoute.RoutePart.RoutePathList.Count > 0 ? routePathObj : null;
            routePathObjParam.ParameterName = "P_ROUTE_PATH_ARRAY";

            OracleParameter oracleParameterGeo = cmd.CreateParameter();
            oracleParameterGeo.OracleDbType = OracleDbType.Object;
            oracleParameterGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleParameterGeo.Value = saveNERoute.RoutePart.RoutePartDetails.PartGeometry; //Variable storing Geometry details for outline , route's 
            oracleParameterGeo.ParameterName = "P_GEOMETRY";

            OracleParameter oracleParameterGPXGeo = cmd.CreateParameter();
            oracleParameterGPXGeo.OracleDbType = OracleDbType.Object;
            oracleParameterGPXGeo.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            oracleParameterGPXGeo.Value = saveNERoute.RoutePart.GPXGeometry; //Variable storing Geometry details for outline , route's 
            oracleParameterGPXGeo.ParameterName = "P_GPX_GEOMETRY";

            string procedure = ".STP_NE_GENERAL.SP_SAVE_NON_ESDAL_ROUTE";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + procedure,
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTE_NAME", saveNERoute.RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_DESCR", saveNERoute.RouteDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REV_ID", saveNERoute.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NOTIF_ID", saveNERoute.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_VR1", isVr1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_AUTO_PLAN", isAutoPlanned, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(routePathObjParam);
                    parameter.Add(oracleParameterGeo);
                    parameter.Add(oracleParameterGPXGeo);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    saveNERoute.RoutePart.RoutePartDetails.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                    result = saveNERoute.RoutePart.RoutePartDetails.RouteId;
                });
            return result;
        }
        #endregion

        #region SORT Previous/Current Movement RouteList
        public static List<AppRouteList> GetSortMovementRoute(long revisionId, int rListType)
        {
            List<AppRouteList> appRouteList = new List<AppRouteList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            appRouteList,
                UserSchema.Sort + ".STP_MOVEMENT.SP_PREVIOUS_MOVE_ROUTES",
                parameter =>
                {
                    parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TYPE", rListType, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteID = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.PartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                    instance.FromAddress = records.GetStringOrDefault("START_ADDRESS");
                    instance.ToAddress = records.GetStringOrDefault("END_ADDRESS");
                    instance.RouteType = records.GetStringOrDefault("ROUTE_TYPE");
                }
            );
            return appRouteList;
        }
        #endregion

        #region GetRouteDetailForAnalysis
        public static List<RoutePartDetails> GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema)
        {
            List<RoutePartDetails> routeList = new List<RoutePartDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                routeList,
                userSchema + ".STP_ROUTE_ASSESSMENT.SP_GET_ROUTE_FOR_ANALYSIS",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_REF", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CAND_FLAG", isCandidate, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                });
            return routeList;
        }
        #endregion

        #region CheckRouteVehicleAttach
        public static int CheckRouteVehicleAttach(long routePartId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
            UserSchema.Portal + ".SP_CHECK_ROUTE_VEHICLE_ATTACH",
            parameter =>
            {
                parameter.AddWithValue("P_RouteId", routePartId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            record =>
            {
                result = Convert.ToInt32(record.GetDecimalOrDefault("CNT"));
            });
            return result;
        }
        #endregion

        #region SaveAnnotationInLibrary
        public static long SaveAnnotationInLibrary(int organisationId, int userId, long annotationType, string annotationText, long structureId = 0, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            int output = 0;
            //int oracleOutputMapper = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_INSERT_INTO_ANNOTATIONS_LIBRARY",
                parameter =>
                {
                    parameter.AddWithValue("P_ORGANISATION_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USER_ID ", (int)userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_STRUCTURE_ID", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ANNOT_TYPE", (int)annotationType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ANNOT_TEXT ", annotationText, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LAST_MODIFIED_TIME", DateTime.UtcNow, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_RECORD_INSERTED", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);


                },
                 record =>
                 {
                     result = record.GetLongOrDefault("ANNOTATIONS_LIBRARY_ID");
                 }

            );
            return result;
        }
        #endregion

        #region GetAnnotationsFromLibrary
        public static List<AnnotationTextLibrary> GetAnnotationsFromLibrary(int organisationId, int userId, int pageNumber, int pageSize, long annotationType, string annotationText, long structureId = 0, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            List<AnnotationTextLibrary> annotTextList = new List<AnnotationTextLibrary>();


            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    annotTextList,
                    userSchema + ".SP_GET_ANNOTATION_FROM_LIBRARY",
                    parameter =>
                    {

                        parameter.AddWithValue("p_PageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ANNOT_TYPE", annotationType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_ID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ANNOT_TEXT", annotationText, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);


                    },
                     (records, instance) =>
                     {
                         var data = records.GetFieldType("TOTALRECORDCOUNT");
                         instance.AnnotationTextId = records.GetLongOrDefault("ANNOTATIONS_LIBRARY_ID");
                         instance.AnnotType = records.GetInt32OrDefault("ANNOT_TYPE");
                         instance.AnnotationText = records.GetStringOrDefault("ANNOT_TEXT");
                         instance.UserName = records.GetStringOrDefault("USER_NAME");
                         instance.totalRecords = records.GetDecimalOrDefault("TOTALRECORDCOUNT");

                         //result = record.GetLongOrDefault("P_IS_RECORD_INSERTED");
                     }

                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return annotTextList;
        }
        #endregion

        #region ReOrderRoutePart
        public static int ReOrderRoutePart(string routePartIds, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            var count = routePartIds.Split(',').Count();
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               userSchema + ".SP_UPDATE_ROUTE_PART_NO",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_IDS", routePartIds, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_ROUTE_IDS_COUNT", count, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_AFFECT_ROW", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    result = records.GetInt32("P_AFFECT_ROW");
                }
            );
            return result;
        }
        #endregion

        #region GetRoutePartDetails
        public static List<RoutePartDetails> GetRoutePartDetails(string notificationidVal, int? isNenViaPdf, int? isHistoric, int orgId, string userSchema = UserSchema.Portal)
        {

            List<RoutePartDetails> routePartDetails = new List<RoutePartDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                   routePartDetails,
                  UserSchema.Portal + ".SP_GET_ROUTE_PART_DETAILS",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ORGANISATION_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_NOTIF_ID", notificationidVal, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_TYPE", isNenViaPdf, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_IS_HISTORIC", isHistoric, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                  (records, instance) =>
                  {
                      instance.RouteName = records.GetStringOrDefault("PART_NAME");
                      instance.RouteDescr = records.GetStringOrDefault("PART_DESCR");

                  }
                   );
            return routePartDetails;




        }

        #endregion

        #region NEN Via API route functions
        /// <summary>
        /// To Clone the NEN route for organisation specific created by NEN via API
        /// </summary>
        /// <param name="cloneNenRoute"></param>
        /// <returns></returns>
        public static List<NenRouteList> CloneNenRoute(CloneNenRoute cloneNenRoute)
        {
            List<NenRouteList> nenRouteLists = new List<NenRouteList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                nenRouteLists,
                UserSchema.Portal + ".STP_NON_ESDAL_ROUTES.SP_CLONE_ROUTE_PARTS",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID", cloneNenRoute.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ANALYSIS_ID", cloneNenRoute.AnalysisId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_REF", cloneNenRoute.ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION", cloneNenRoute.Organisations, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_COUNT", cloneNenRoute.OrgCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                    instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                    instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                });
            return nenRouteLists;
        }
        /// <summary>
        /// To fetch the NEN route details for organisation to show the route in map created using via API
        /// </summary>
        /// <param name="contRefNum"></param>
        /// <param name="orgId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static List<AppRouteList> GetPlannedNenAPIRouteList(string contRefNum, int orgId, string userSchema)
        {
            List<AppRouteList> nenRouteList = new List<AppRouteList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                nenRouteList,
                 UserSchema.Portal + ".STP_NON_ESDAL_ROUTES.SP_GET_ROUTE_PARTS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_REF_NO", contRefNum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteID = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteDescription = records.GetStringOrDefault("PART_DESCR");
                    instance.RouteType = "planned";
                    instance.FromAddress = records.GetStringOrDefault("FROM_ADDRESS");
                    instance.ToAddress = records.GetStringOrDefault("TO_ADDRESS");
                    instance.PartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                }
            );
            return nenRouteList;
        }
        /// <summary>
        /// To fetch the NEN route details for organisation to generate routeassessment created using via API
        /// </summary>
        /// <param name="contRefNum"></param>
        /// <param name="orgId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public static List<RoutePartDetails> GetNenApiRoutesForAnalysis(string contRefNum, int orgId, string userSchema)
        {
            List<RoutePartDetails> nenRouteList = new List<RoutePartDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                nenRouteList,
                 UserSchema.Portal + ".STP_NON_ESDAL_ROUTES.SP_GET_NEN_ROUTE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_REF_NO", contRefNum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                    instance.AnalysisId = (int)records.GetLongOrDefault("ANALYSIS_ID");
                }
            );
            return nenRouteList;
        }
        #endregion

        #region NEN PDF Route details
        public static List<RoutePartDetails> GetNenPdfRoutesForAnalysis(int inboxItemId, int orgId, string userSchema)
        {
            List<RoutePartDetails> nenRouteList = new List<RoutePartDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                nenRouteList,
                 UserSchema.Portal + ".STP_NON_ESDAL_ROUTES.SP_GET_NEN_PDF_ROUTE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_INBOX_ITEM_ID", inboxItemId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGANISATION_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RouteId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RouteName = records.GetStringOrDefault("PART_NAME");
                    instance.RouteType = records.GetStringOrDefault("TRANSPORT_MODE");
                    instance.AnalysisId = (int)records.GetLongOrDefault("ANALYSIS_ID");
                }
            );
            return nenRouteList;
        }
        #endregion
    }
    #endregion
}