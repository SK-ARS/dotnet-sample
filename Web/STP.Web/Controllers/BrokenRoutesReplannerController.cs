#region
using STP.Common.Logger;
using STP.Domain.RoutePlannerInterface;
using STP.ServiceAccess.RoutePlannerInterface;
using STP.ServiceAccess.Routes;
using STP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using static STP.Domain.Routes.RouteModel;
using STP.Domain.SecurityAndUsers;
using static STP.Domain.Routes.RouteModelJson;
using Newtonsoft.Json;
using STP.Domain.Routes;
using STP.Common.Constants;
using STP.Domain.MovementsAndNotifications.Notification;

#endregion

namespace STP.Business.Controllers
{
    [AuthorizeUser(Roles = "40000,40001,40002,13003,13004,13005,13006,100001,100002,300000")]
    public class BrokenRoutesReplannerController : Controller
    {
        private readonly IRoutesService routesService;
        private readonly IRoutePlannerInterfaceService routePlannerInterfaceSevice;
        public BrokenRoutesReplannerController(IRoutesService routesService, IRoutePlannerInterfaceService routePlannerSevice)
        {
            this.routesService = routesService;
            routePlannerInterfaceSevice = routePlannerSevice;
        }

        #region ReplanBrokenRoutes
        [HttpPost]
        public ActionResult ReplanBrokenRoutes(int routePartId = 0, int isLib = 0, bool isViewOnly = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("BrokenRoutesReplanner/ReplanBrokenRoutes actionResult method started successfully. RouteId: {0}", routePartId));

                BrokenRouteResponseModel responseModel = ReplanBrokenRoutesCommon(routePartId, isLib, isViewOnly);

                if (isViewOnly)
                {
                    var rson = JsonConvert.SerializeObject(responseModel.RoutePart);
                    var routePartJson1 = JsonConvert.DeserializeObject<RoutePartJson>(rson);
                    return Json(new { result = routePartJson1, isSuccess = responseModel.Result });
                }

                return Json(new { result = responseModel.Result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("BrokenRoutesReplanner/ReplanBrokenRoutes, Exception: {0}", ex));
                return Json(new {error= "ReplanBrokenRoutes error occured" });
            }
        }
        #endregion

        #region GetSessionInfo
        public Domain.SecurityAndUsers.UserInfo GetSessionInfo()
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            return SessionInfo;
        }
        #endregion

        #region GetPlannedRoutePath
        public RoutePart GetPlannedRoutePath(int routePartId = 0, int isLib = 0, string userSchema = "")
        {
            if (isLib == 1)
            {
                return routesService.GetLibraryRoute(routePartId, userSchema);
            }
            else
            {
                return routesService.GetApplicationRoute(routePartId, userSchema);
            }
        }
        #endregion

        #region ProcessRoutePointsAndReplan
        public RouteData ProcessRoutePointsAndReplan(List<RoutePoint> brokenRouteList)
        {
            RouteViaWaypointEx routePoints = new RouteViaWaypointEx();
            List<WayPoint> wayPoints = new List<WayPoint>();
            UInt32 startLinkId = 0;
            UInt32 endLinkId = 0;
            foreach (var points in brokenRouteList)
            {
                if (points.PointType == 239001)
                {
                    routePoints.BeginStartNode = Convert.ToUInt32(points.NewBeginNodeId);
                    routePoints.BeginPointEndNode = Convert.ToUInt32(points.NewEndNodeId);
                    routePoints.BeginPointLinkId = Convert.ToUInt32(points.NewLinkId);
                    startLinkId = Convert.ToUInt32(points.NewLinkId);
                }
                else if (points.PointType == 239002)
                {
                    routePoints.EndPointStartNode = Convert.ToUInt32(points.NewBeginNodeId);
                    routePoints.EndPointEndNode = Convert.ToUInt32(points.NewEndNodeId);
                    routePoints.EndPointLinkId = Convert.ToUInt32(points.NewLinkId);
                    endLinkId = Convert.ToUInt32(points.NewLinkId);
                }
                else
                {
                    WayPoint point = new WayPoint();
                    point.WayPointBeginNode = Convert.ToUInt32(points.NewBeginNodeId);
                    point.WayPointEndNode = Convert.ToUInt32(points.NewEndNodeId);
                    point.WayPointLinkId = Convert.ToUInt32(points.NewLinkId);
                    wayPoints.Add(point);
                }
            }
            routePoints.WayPoints = wayPoints;

            Domain.RoutePlannerInterface.RouteData routeData = routePlannerInterfaceSevice.GetRouteData(routePoints);
            routeData = ProcessRouteData(routeData, startLinkId, endLinkId);
            return routeData;
        }
        #endregion

        #region ProcessRouteData
        public RouteData ProcessRouteData(Domain.RoutePlannerInterface.RouteData routeData, uint startLinkId, uint endLinkId)
        {
            RouteData newrouteData = new RouteData();
            List<UInt32> listSegments = new List<UInt32>();
            if (routeData != null && routeData.ListSegments != null)
            {
                int count = routeData.ListSegments != null ? routeData.ListSegments.Count : 0;
                listSegments.Add(startLinkId);
                for (int i = 0; i < count; i++)
                {
                    if (listSegments[0] != routeData.ListSegments[i])
                        listSegments.Add(routeData.ListSegments[i]);
                }
                if (routeData.ListSegments != null && routeData.ListSegments.Count > 0 && routeData.ListSegments[count - 1] != endLinkId)
                {
                    listSegments.Add(endLinkId);
                }
                newrouteData.ResponseMessage = routeData.ResponseMessage;
            }
            newrouteData.ListSegments = listSegments;
            return newrouteData;
        }
        #endregion

        #region ProcessRoutePaths
        public bool ProcessRoutePaths(RoutePath plannedRoutePath, List<RoutePoint> brokenRouteList, List<RouteAnnotation> brokenAnnotationList, RouteData routeData, int isLib = 0, bool isViewOnly = true, string userSchema = "")
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("BrokenRoutesReplanner/ProcessAndSaveRouteSegments actionResult method started successfully. RoutePathId: {0}", plannedRoutePath.RoutePathId));

            bool result = true;
            var i = 0;
            List<RouteLink> routeLinkList = new List<RouteLink>();
            RouteLink routeLink = new RouteLink();
            try
            {
                foreach (var routePoint in plannedRoutePath.RoutePointList)
                {
                    foreach (var point in brokenRouteList)
                    {
                        if (point.RoutePointId == routePoint.RoutePointId)
                        {
                            routePoint.LinkId = point.NewLinkId;
                            routePoint.PointGeom.sdo_point.X = point.RoadGeometry.OrdinatesArray[0];
                            routePoint.PointGeom.sdo_point.Y = point.RoadGeometry.OrdinatesArray[1];
                            routePoint.Lrs = point.Lrs;
                            break;
                        }
                    }
                }
                if (routeData.ListSegments != null)
                {
                    foreach (var item in routeData.ListSegments)
                    {
                        routeLink = new RouteLink
                        {
                            LinkId = item,
                            LinkNo = i++,
                            Direction = 1,
                            SegmentId = plannedRoutePath.RouteSegmentList[0].SegmentId,
                            SegmentNo = plannedRoutePath.RouteSegmentList[0].SegmentNo
                        };
                        routeLinkList.Add(routeLink);
                    }
                }
                plannedRoutePath.RouteSegmentList[0].RouteLinkList = routeLinkList;

                foreach (var routeAnnotation in plannedRoutePath.RouteSegmentList[0].RouteAnnotationsList)
                {
                    foreach (var annotation in brokenAnnotationList)
                    {
                        if (annotation.AnnotationID == routeAnnotation.AnnotationID)
                        {
                            routeAnnotation.LinkId = annotation.NewLinkId;
                            routeAnnotation.Geometry.sdo_point.X = annotation.RoadGeometry.OrdinatesArray[0];
                            routeAnnotation.Geometry.sdo_point.Y = annotation.RoadGeometry.OrdinatesArray[1];
                            routeAnnotation.Easting = (long)annotation.RoadGeometry.OrdinatesArray[0];
                            routeAnnotation.Northing = (long)annotation.RoadGeometry.OrdinatesArray[1];
                            routeAnnotation.LinearRef = annotation.LinearRef;
                            break;
                        }
                    }
                }

                bool startPointFlag = false, endPointFlag = false;
                foreach (var rPoint in plannedRoutePath.RoutePointList)
                {
                    if (rPoint.PointType == 0)
                    {
                        plannedRoutePath.RouteSegmentList[0].StartLinkId = rPoint.LinkId;
                        plannedRoutePath.RouteSegmentList[0].StartPointGeometry = rPoint.PointGeom;
                        startPointFlag = true;
                    }
                    else if (rPoint.PointType == 1)
                    {
                        plannedRoutePath.RouteSegmentList[0].EndLinkId = rPoint.LinkId;
                        plannedRoutePath.RouteSegmentList[0].EndPointGeometry = rPoint.PointGeom;
                        endPointFlag = true;
                    }
                    if (startPointFlag && endPointFlag)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("BrokenRoutesReplanner/ProcessAndSaveRoutePaths, Exception: {0}", ex));
                throw;
            }

            return result;
        }
        #endregion

        #region Check_Is_BrokenRoute
        [HttpPost]
        public ActionResult Is_BrokenRouteCheck(GetBrokenRouteList getBrokenRouteList)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"BrokenRoutesReplanner/Is_BrokenRouteCheck actionResult method started successfully. RouteId: {getBrokenRouteList.RoutePartId}, VersionId: {getBrokenRouteList.VersionId}, RevisonId: {getBrokenRouteList.AppRevisonId}, LibraryRoute: {getBrokenRouteList.LibraryRouteId}, Content_reference: {getBrokenRouteList.ConteRefNo}");

            UserInfo SessionInfo = GetSessionInfo();
            List<BrokenRouteList> brokenRoutes = new List<BrokenRouteList>();
            int brokenRouteCount = 0;
            int specialManouer = 0;
            int autoReplanSuccess = 0;
            int autoReplanFail = 0;
            if (SessionInfo != null)
            {
                getBrokenRouteList.UserSchema = SessionInfo.UserSchema;
                if (getBrokenRouteList.IsSort)//From SOA/Police movement details need to be extracted from the SORT portal
                    getBrokenRouteList.UserSchema = UserSchema.Sort;
                try
                {
                    getBrokenRouteList.OrganisationId = SessionInfo.OrganisationId;
                    brokenRoutes = routesService.GetBrokenRouteIds(getBrokenRouteList);//PDF-1, API-2
                    if (brokenRoutes != null)
                    {
                        for (var i = 0; i < brokenRoutes.Count; i++)
                        {
                            var brokenRoute = brokenRoutes[i];
                            if (brokenRoute.IsBroken > 0)
                            {
                                brokenRouteCount++;
                                if (brokenRoute.IsReplan > 1)//check in the existing route is broken and there exists special manouers
                                    specialManouer++;
                                else if (brokenRoute.IsReplan < 2)//Replan possible if value is 1
                                {
                                    if (getBrokenRouteList.IsReplanRequired)
                                    {
                                        var replanResult = ReplanBrokenRoutesCommon((int)brokenRoute.PlannedRouteId, 0, getBrokenRouteList.IsViewOnly);
                                        if (replanResult.Result)
                                            autoReplanSuccess++;
                                        else
                                            autoReplanFail++;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("BrokenRoutesReplanner/Is_BrokenRouteCheck, Exception: {0}", ex));
                    throw;
                }
            }
            return Json(new BrokenRouteOutputModel
            {
                Result = brokenRoutes,
                brokenRouteCount = brokenRouteCount,
                specialManouer = specialManouer,
                autoReplanSuccess = autoReplanSuccess,
                autoReplanFail = autoReplanFail
            });
        }

        private BrokenRouteResponseModel ReplanBrokenRoutesCommon(int routePartId = 0, int isLib = 0, bool isViewOnly = false)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("BrokenRoutesReplanner/ReplanBrokenRoutesCommon method started successfully. RouteId: {0}", routePartId));

            bool result = false;
            UserInfo SessionInfo = GetSessionInfo();
            BrokenRouteResponseModel output = new BrokenRouteResponseModel();
            RoutePart routePart = new RoutePart();

            if (SessionInfo != null)
            {
                try
                {
                    routePart = GetPlannedRoutePath(routePartId, isLib, SessionInfo.UserSchema);

                    foreach (var routePath in routePart.RoutePathList)
                    {
                        List<RoutePoint> brokenRouteList = routesService.GetBrokenRoutePoints(routePath.RoutePathId, isLib, SessionInfo.UserSchema);
                        List<RouteAnnotation> brokenAnnotationList = routesService.GetBrokenRouteAnnotations(routePath.RouteSegmentList[0].SegmentId, isLib, SessionInfo.UserSchema);

                        RouteData routeData = ProcessRoutePointsAndReplan(brokenRouteList);

                        if (routeData != null && routeData.ListSegments != null && routeData.ListSegments.Count > 0)
                            result = ProcessRoutePaths(routePath, brokenRouteList, brokenAnnotationList, routeData, isLib, isViewOnly, SessionInfo.UserSchema);
                    }


                    if (!isViewOnly && result)
                    {
                        result = routesService.UpdateBrokenRoutePath(routePart.RoutePathList, isLib, SessionInfo.UserSchema);
                    }
                    if (!isViewOnly)
                    {
                        if (result)
                            routesService.SetVerificationStatus(routePartId, isLib, 921002, SessionInfo.UserSchema);
                        else
                            routesService.SetVerificationStatus(routePartId, isLib, 921003, SessionInfo.UserSchema);
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("BrokenRoutesReplanner/ReplanBrokenRoutes, Exception: {0}", ex));
                    throw;
                }
            }

            return new BrokenRouteResponseModel() { RoutePart = routePart, Result = result };
        }
        #endregion

        #region Broken Routes
        public ActionResult ShowBrokenRoutes(long App_revision_id, long can_revision_id, long mov_version_id, int FlagValue = 0, string ContentRefNo = null)
        {
            string userSchema = UserSchema.Sort;
            if (FlagValue == 7)
            {
                if (ContentRefNo != "0")
                    userSchema = UserSchema.Portal;
                else
                    ContentRefNo = null;
            }
            //commented for allow notify SO with broken route
            if (FlagValue == 3 || FlagValue == 4)
            {
                userSchema = UserSchema.Portal;
            }
            List<string> lstBrokenRoutes = new List<string>();

            List<NotifRouteImport> objlstBrokenRoutes = routesService.ListBrokenRouteDetails(ContentRefNo, userSchema, App_revision_id, can_revision_id, mov_version_id);

            foreach (NotifRouteImport strroute in objlstBrokenRoutes)
            {
                lstBrokenRoutes.Add(strroute.RouteName.ToString());
            }
            if (lstBrokenRoutes.Count == 0)
            {
                return null;
            }
            if ((FlagValue == 5 || FlagValue == 8 || FlagValue == 9) && (lstBrokenRoutes.Count != 0))    // here we dont want the ShowBrokenRoutes view......
            {
                bool data = true;
                return Json(data);     //  9/sendforchecking vr1  5/createcandidate route from application .. 8/create candidate route from last candidate version
            }
            else
            {
                ViewBag.FlagValue = FlagValue;
                ViewBag.BrokenRoutes = lstBrokenRoutes;
                ViewBag.ItemType = "";
                if (FlagValue == 7)//For SOA and Police
                {
                    if (ContentRefNo == "0" || ContentRefNo == null)
                    {
                        ViewBag.ItemType = "SO";
                    }
                    else
                    {
                        ViewBag.ItemType = "Notification";
                    }
                }
                return PartialView("ShowBrokenRoutes");
            }
        }
        #endregion

    }
}