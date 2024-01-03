using NetSdoGeometry;
using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.RoutePlannerInterface;
using STP.Domain.Routes;
using STP.RoutePlannerInterface.Socket;
using STP.Routes.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using static STP.Domain.Routes.RouteModel;
using static STP.Domain.Routes.RouteSerialization;

namespace STP.Routes.Controllers
{
    public class RouteImportController : ApiController
    {
        private static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];
        private static readonly string TerminateMessage = $", Terminating the process due to excess buffer area.";
        private static readonly string GPXMapping = $"Mapping GPX failed due to absence of valid road segment(s) in ";
        private string InputMessage;
        private static readonly int MinTolerance = Convert.ToInt32(ConfigurationManager.AppSettings["MinTolerance"]);
        private static readonly int MaxTolerance = Convert.ToInt32(ConfigurationManager.AppSettings["MaxTolerance"]);
        private static readonly int IncrementBuffer = Convert.ToInt32(ConfigurationManager.AppSettings["IncrementBuffer"]);
        private static readonly string RetryMessage = $", Retrying by increasing buffer area to {IncrementBuffer}m";
        private static readonly string GPXProcessCompleted = $"GPX Processing completed successfully with road segment(s) in ";
        private static readonly string GPXProcessStart = $"Processing GPX for movement with";
        private static readonly string ProcessRoadSegment = $"Processing road segments within ";

        #region Convert Route
        private RoutePart ConvertRoute(GPXInputModel gpxInput, out bool isAutoReplan)
        {
            RoutePart routePart;
            isAutoReplan = false;
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(gpxType));
                gpxType route;
                using (XmlReader reader = XmlReader.Create(new StringReader(gpxInput.RouteGPX)))
                {
                    route = (gpxType)deserializer.Deserialize(reader);
                }
                routePart = ConvertRouteFromGPX(route);
                sdogeometry gpxGeometry = GetGeometryFromGPX(route);
                if (routePart == null)
                {
                    //Auto Replan
                    isAutoReplan = true;
                    routePart = ConstructAutoPlanRoutePart(route.wpt);
                }
                routePart.GPXGeometry = gpxGeometry;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - RouteImport/ConvertRouteFromGPX,Exception:" + ex);
                routePart = null;
            }
            return routePart;
        }
        #endregion

        #region ConvertRouteFromGPX
        private RoutePart ConvertRouteFromGPX(gpxType route)
        {
            RoutePart RoutePart = null;
            ProcessedRouteModel processedRoute = new ProcessedRouteModel();
            ProcessedWaypointModel processedWaypoint;
            try
            {
                if (route.trk != null)
                {
                    processedRoute = GetRouteFromGPXTrk(route);
                }
                else if (route.rte != null)
                {
                    processedRoute = GetRouteFromGPXRte(route);
                }
                if (processedRoute.RouteComplete)
                {
                    processedWaypoint = GetWaypointsFromGPX(route, processedRoute);
                    if (processedWaypoint.WaypointComplete)
                    {
                        //Construct route point
                        List<RoutePoint> routePointList = ConstructRoutePoint(processedWaypoint);

                        //Construct route path
                        List<RoutePath> routePathList = ConstructRoutePath(routePointList, processedRoute);

                        //Construct route part
                        RoutePart = ConstructRoutePart(routePathList);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - RouteImport/ConvertRouteFromGPX,Exception:" + ex);
                RoutePart = null;
            }
            return RoutePart;
        }
        private ProcessedRouteModel GetRouteFromGPXTrk(gpxType route)
        {
            List<RouteLinkResult> pathResultLinks = new List<RouteLinkResult>();
            List<RouteSegment> RouteSegmentList = new List<RouteSegment>();
            bool routeComplete = false;

            //track
            List<RouteLinkResult> resultLinks = null;

            int segmentCount = 0;
            RouteLinkModel startSegLink = null, endSegLink = null;
            List<RouteLink> routeLinkList;
            foreach (var trkpt in route.trk[0].trkseg.Select(item => item.trkpt))
            {
                startSegLink = null; endSegLink = null;
                resultLinks = ProcessRouteLinkList(null, trkpt, UserSchema.Portal, ref routeComplete, ref startSegLink, ref endSegLink);
                if (routeComplete)
                {
                    //Construct route links
                    routeLinkList = ConstructRouteLink(resultLinks, null);

                    //Construct route segment
                    RouteSegmentList.Add(ConstructRouteSegment(++segmentCount, startSegLink, endSegLink, resultLinks, routeLinkList));
                }
                /*--------------------------------------------------------------------------------------------------*/
                pathResultLinks.AddRange(resultLinks);
            }

            return new ProcessedRouteModel()
            {
                PathResultLinks = pathResultLinks,
                RouteSegmentList = RouteSegmentList,
                RouteComplete = routeComplete
            };
        }
        private ProcessedRouteModel GetRouteFromGPXRte(gpxType route)
        {
            List<RouteLinkResult> pathResultLinks = new List<RouteLinkResult>();
            List<RouteSegment> RouteSegmentList = new List<RouteSegment>();
            //track
            List<RouteLinkResult> resultLinks = null;
            bool routeComplete = false;
            int segmentCount = 0;
            RouteLinkModel startSegLink = null, endSegLink = null;
            List<RouteLink> routeLinkList;
            foreach (var rtept in route.rte.Select(trkseg => trkseg.rtept))
            {
                startSegLink = null; endSegLink = null;
                resultLinks = ProcessRouteLinkList(null, rtept, UserSchema.Portal, ref routeComplete, ref startSegLink, ref endSegLink);

                if (routeComplete)
                {
                    //Construct route links
                    routeLinkList = ConstructRouteLink(resultLinks, null);

                    //Construct route segment
                    RouteSegmentList.Add(ConstructRouteSegment(++segmentCount, startSegLink, endSegLink, resultLinks, routeLinkList));
                }
                /*--------------------------------------------------------------------------------------------------*/
                pathResultLinks.AddRange(resultLinks);
            }

            return new ProcessedRouteModel()
            {
                PathResultLinks = pathResultLinks,
                RouteSegmentList = RouteSegmentList,
                RouteComplete = routeComplete
            };
        }
        private ProcessedWaypointModel GetWaypointsFromGPX(gpxType route, ProcessedRouteModel processedRoute)
        {
            //waypoints
            int wptCount = 0;
            RouteLinkModel startLink = null, endLink = null;
            List<RouteLinkModel> waypointLinks = new List<RouteLinkModel>();
            bool waypointComplete = false;
            RouteLinkModel wptLink;
            foreach (var wpt in route.wpt)
            {
                wptLink = GetNearestLinkFromRouteTrack(GetLinkFromPoint(wpt.lon, wpt.lat, 2001, 4326, wpt.desc), processedRoute.PathResultLinks);
                if (wptLink != null && wptLink.RoadGeometry != null)
                {
                    if (wptCount == 0)   //start point
                    {
                        startLink = wptLink;
                    }
                    else if (wptCount == route.wpt.Length - 1)   //end point
                    {
                        endLink = wptLink;
                        waypointComplete = true;
                    }
                    else    //waypoint
                    {
                        waypointLinks.Add(wptLink);
                    }
                }
                else
                {
                    waypointComplete = false;
                    break;
                }
                wptCount++;
            }

            return new ProcessedWaypointModel()
            {
                StartLink = startLink,
                EndLink = endLink,
                WaypointLinks = waypointLinks,
                WaypointComplete = waypointComplete
            };
        }
        private List<RouteLink> ConstructRouteLink(List<RouteLinkResult> resultLinks, RouteSegment routeSegment)
        {
            List<RouteLink> routeLinkList = new List<RouteLink>();
            if (routeSegment != null)
            {
                foreach (var link in resultLinks)
                {
                    routeLinkList.Add(new RouteLink()
                    {
                        SegmentId = routeSegment.SegmentId,
                        SegmentNo = routeSegment.SegmentNo,
                        LinkId = link.LinkId,
                        LinkNo = link.LinkNo,
                        Direction = link.Direction
                    });
                }
            }
            else
            {
                foreach (var link in resultLinks)
                {
                    routeLinkList.Add(new RouteLink()
                    {
                        LinkId = link.LinkId,
                        LinkNo = link.LinkNo,
                        Direction = link.Direction
                    });
                }
            }

            return routeLinkList;
        }
        private RouteSegment ConstructRouteSegment(int segmentCount, RouteLinkModel startSegLink, RouteLinkModel endSegLink, List<RouteLinkResult> resultLinks, List<RouteLink> routeLinkList)
        {
            return new RouteSegment()
            {
                SegmentNo = segmentCount,
                SegmentType = 1,
                StartLinkId = startSegLink.LinkId,
                StartLrs = startSegLink.Lrs,
                StartGeom = JsonConvert.SerializeObject(GetPointOnlyGeometry(startSegLink.RoadGeometry, 2001, 27700)),
                StartPointDirection = resultLinks[0].Direction,
                EndLinkId = endSegLink.LinkId,
                EndLrs = endSegLink.Lrs,
                EndGeom = JsonConvert.SerializeObject(GetPointOnlyGeometry(endSegLink.RoadGeometry, 2001, 27700)),
                EndPointDirection = resultLinks[resultLinks.Count - 1].Direction,
                RouteLinkList = routeLinkList
            };
        }
        private List<RoutePoint> ConstructRoutePoint(ProcessedWaypointModel processedWaypoint)
        {
            List<RoutePoint> RoutePointList = new List<RoutePoint>
            {
                SetRoutePoint(processedWaypoint.StartLink, 0, 1),
                SetRoutePoint(processedWaypoint.EndLink, 1, 2)
            };
            int routePointNo = 0;
            foreach (var waypoint in processedWaypoint.WaypointLinks)
            {
                RoutePointList.Add(SetRoutePoint(waypoint, 3, ++routePointNo));
            }

            return RoutePointList;
        }
        private List<RoutePath> ConstructRoutePath(List<RoutePoint> routePointList, ProcessedRouteModel processedRoute)
        {
            List<RoutePath> RoutePathList = new List<RoutePath>();
            RoutePathList.Add(new RoutePath()
            {
                RoutePathNo = 1,
                RoutePointList = routePointList,
                RouteSegmentList = processedRoute.RouteSegmentList
            });

            return RoutePathList;
        }
        private RoutePart ConstructRoutePart(List<RoutePath> routePathList)
        {
            return new RoutePart()
            {
                RoutePartDetails = new RoutePartDetails()
                {
                    RouteType = "planned"
                },
                RoutePathList = routePathList
            };
        }
        private RouteLinkResult ConstructRouteLinkResult(long linkId, int linkNo, int direction, long nodeId)
        {
            return new RouteLinkResult()
            {
                LinkId = linkId,
                LinkNo = linkNo,
                Direction = direction,
                LastNode = nodeId
            };
        }
        private List<RouteLinkModel> GetLinkFromPoint(double x, double y, decimal gtype, decimal srid, string description)
        {
            List<RouteLinkModel> linkList;
            try
            {
                sdogeometry point = new sdogeometry()
                {
                    sdo_point = new SDOPOINT()
                    {
                        X = Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(x),
                        Y = Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(y),
                        Z = null
                    },
                    sdo_gtype = gtype,
                    sdo_srid = srid
                };
                linkList = RouteImport.Instance.GetLinkFromGPXTrackPoint(point, description);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GPX/GetRouteLinks, Exception: " + ex​​​​);
                linkList = null;
            }

            return linkList;
        }
        private RouteLinkModel GetNearestLinkFromRouteTrack(List<RouteLinkModel> wptLinks, List<RouteLinkResult> trackLinks = null, List<RouteLinkModel> routeLinks = null)
        {
            var wpt = new RouteLinkModel();
            if (trackLinks != null)
            {
                wpt = wptLinks.Find(wpt1 => trackLinks.Exists(e => e.LinkId == wpt1.LinkId));
            }
            else if (routeLinks != null)
            {
                wpt = wptLinks.Find(wpt1 => routeLinks.Exists(e => e.LinkId == wpt1.LinkId));
            }
            return wpt;
        }
        private List<RouteLinkModel> GetRouteLinks(wptType[] trkseg, decimal[] elemArray, decimal gtype, decimal srid, int tolerance)
        {
            List<RouteLinkModel> routeLinks;
            try
            {
                sdogeometry routeGeom = new sdogeometry
                {
                    ElemArray = elemArray,
                    sdo_gtype = gtype,
                    sdo_srid = srid
                };

                List<decimal> geomOrdinates = new List<decimal>();
                foreach (var trkpt in trkseg)
                {
                    geomOrdinates.Add(Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(trkpt.lon));
                    geomOrdinates.Add(Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(trkpt.lat));
                }
                routeGeom.OrdinatesArray = geomOrdinates.ToArray();

                routeLinks = RouteImport.Instance.GetRouteFromGPXTrack(routeGeom, tolerance);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GPX/GetRouteLinks, Exception: " + ex​​​​);
                routeLinks = null;
            }

            return routeLinks;
        }
        private RoutePoint SetRoutePoint(RouteLinkModel routePointLink, int pointType, int pointNo)
        {
            return new RoutePoint()
            {
                PointType = pointType,
                RoutePointNo = pointNo,
                IsAnchorPoint = pointType == 239004 ? 1 : 0,
                LinkId = routePointLink.LinkId,
                RoutePointGeom = JsonConvert.SerializeObject(GetPointOnlyGeometry(routePointLink.RoadGeometry, 2001, 27700)),
                Lrs = routePointLink.Lrs,
                PointDescr = routePointLink.Description
            };
        }
        #endregion

        #region Common Method
        private sdogeometry GetPointOnlyGeometry(sdogeometry geom, decimal gtype, decimal srid)
        {
            sdogeometry pointGeometry = null;
            if (geom.OrdinatesArray != null && geom.OrdinatesArray.Length > 1)
            {
                pointGeometry = new sdogeometry()
                {
                    sdo_point = new SDOPOINT()
                    {
                        X = geom.OrdinatesArray[0],
                        Y = geom.OrdinatesArray[1],
                        Z = null
                    },
                    sdo_gtype = gtype,
                    sdo_srid = srid
                };
            }
            return pointGeometry;
        }
        private sdogeometry GetGeometryFromGPX(gpxType route)
        {
            sdogeometry gpxGeometry = null;

            foreach (var trkpt in route.trk[0].trkseg.Select(item => item.trkpt))
            {
                gpxGeometry = GetTransformedGeometry(trkpt, new decimal[] { 1, 2, 1 }, 2002, 4326);
                if (gpxGeometry != null)
                    break;
            }
            return gpxGeometry;
        }
        private sdogeometry GetTransformedGeometry(wptType[] trkpt, decimal[] elemArray, int gtype, int srid)
        {
            sdogeometry gpxGeometry;
            sdogeometry routeGeom = new sdogeometry
            {
                ElemArray = elemArray,
                sdo_gtype = gtype,
                sdo_srid = srid
            };

            List<decimal> geomOrdinates = new List<decimal>();
            foreach (var trkseg in trkpt)
            {
                geomOrdinates.Add(Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(trkseg.lon));
                geomOrdinates.Add(Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(trkseg.lat));
            }
            routeGeom.OrdinatesArray = geomOrdinates.ToArray();
            gpxGeometry = RouteImport.Instance.GetTransformedGeometry(routeGeom);
            return gpxGeometry;
        }
        #endregion

        #region Auto Replan Function
        private RoutePart ConstructAutoPlanRoutePart(wptType[] wpt)
        {
            RoutePart routePart = new RoutePart();
            List<RouteLinkModel> routeLinkModel = GetPointForAutoReplan(wpt);
            List<RoutePoint> routePoints = ConstructAutoReplanRoutePoint(routeLinkModel);
            RouteData routeData = GetRouteDataAutoReplan(routePoints);
            RoutePath plannedRoutePath = ConstructAutoReplanRoutePath(routePoints, routeData);
            routePart.RoutePathList.Add(plannedRoutePath);
            routePart.RoutePartDetails.RouteType = "planned";
            return routePart;
        }
        private List<RouteLinkModel> GetPointForAutoReplan(wptType[] wpt)
        {
            List<RouteLinkModel> linkModels = new List<RouteLinkModel>();
            RouteLinkModel linkModel;
            try
            {
                foreach (var item in wpt)
                {
                    sdogeometry point = new sdogeometry()
                    {
                        sdo_point = new SDOPOINT()
                        {
                            X = Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(item.lon),
                            Y = Domain.Custom.StringExtraction.ConvertExponentialValueToDecimal(item.lat),
                            Z = null
                        },
                        sdo_gtype = 2001,
                        sdo_srid = 4326
                    };
                    linkModel = RouteImport.Instance.GetLinkForAutoReplanPoint(point, item.desc);
                    linkModels.Add(linkModel);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GPX/GetAutoReplanLinkFromPoint, Exception: " + ex​​​​);
                linkModels = null;
            }
            return linkModels;
        }
        private List<RoutePoint> ConstructAutoReplanRoutePoint(List<RouteLinkModel> routeLinkModel)
        {
            List<RoutePoint> routePoints = new List<RoutePoint>();
            int wptCount = 0;
            var i = 1;
            foreach (var item in routeLinkModel)
            {
                RoutePoint routePoint;
                if (wptCount == 0)
                {
                    routePoint = new RoutePoint
                    {
                        LinkId = item.LinkId,
                        NewBeginNodeId = item.RefInId,
                        NewEndNodeId = item.NrefInId,
                        RoutePointGeom = JsonConvert.SerializeObject(GetPointOnlyGeometry(item.RoadGeometry, 2001, 27700)),
                        Lrs = item.Lrs,
                        PointType = 0,
                        PointDescr = item.Description,
                        RoutePointNo = 1
                    };
                }
                else if (wptCount == routeLinkModel.Count - 1)   //end point
                {
                    routePoint = new RoutePoint
                    {
                        LinkId = item.LinkId,
                        NewBeginNodeId = item.RefInId,
                        NewEndNodeId = item.NrefInId,
                        RoutePointGeom = JsonConvert.SerializeObject(GetPointOnlyGeometry(item.RoadGeometry, 2001, 27700)),
                        Lrs = item.Lrs,
                        PointType = 1,
                        PointDescr = item.Description,
                        RoutePointNo = 2
                    };
                }
                else    //viapoint
                {
                    routePoint = new RoutePoint
                    {
                        LinkId = item.LinkId,
                        NewBeginNodeId = item.RefInId,
                        NewEndNodeId = item.NrefInId,
                        RoutePointGeom = JsonConvert.SerializeObject(GetPointOnlyGeometry(item.RoadGeometry, 2001, 27700)),
                        Lrs = item.Lrs,
                        PointType = 3,
                        PointDescr = item.Description,
                        RoutePointNo = i++
                    };
                }
                wptCount++;
                routePoints.Add(routePoint);
            }
            return routePoints;
        }
        private RouteData GetRouteDataAutoReplan(List<RoutePoint> routePoints)
        {
            RouteViaWaypointEx routePlannerPoints = new RouteViaWaypointEx();
            List<WayPoint> wayPoints = new List<WayPoint>();
            UInt32 startLinkId = 0;
            UInt32 endLinkId = 0;
            foreach (var points in routePoints)
            {
                if (points.PointType == 0)
                {
                    routePlannerPoints.BeginStartNode = Convert.ToUInt32(points.NewBeginNodeId);
                    routePlannerPoints.BeginPointEndNode = Convert.ToUInt32(points.NewEndNodeId);
                    routePlannerPoints.BeginPointLinkId = Convert.ToUInt32(points.LinkId);
                    startLinkId = Convert.ToUInt32(points.LinkId);
                }
                else if (points.PointType == 1)
                {
                    routePlannerPoints.EndPointStartNode = Convert.ToUInt32(points.NewBeginNodeId);
                    routePlannerPoints.EndPointEndNode = Convert.ToUInt32(points.NewEndNodeId);
                    routePlannerPoints.EndPointLinkId = Convert.ToUInt32(points.LinkId);
                    endLinkId = Convert.ToUInt32(points.LinkId);
                }
                else
                {
                    WayPoint point = new WayPoint
                    {
                        WayPointBeginNode = Convert.ToUInt32(points.NewBeginNodeId),
                        WayPointEndNode = Convert.ToUInt32(points.NewEndNodeId),
                        WayPointLinkId = Convert.ToUInt32(points.LinkId)
                    };
                    wayPoints.Add(point);
                }
            }
            routePlannerPoints.WayPoints = wayPoints;
            RoutePlannerConnect rpConnect = new RoutePlannerConnect();
            RouteData routeData = rpConnect.GetRoute(routePlannerPoints);
            routeData = ProcessRouteData(routeData, startLinkId, endLinkId);
            return routeData;
        }
        private RouteData ProcessRouteData(RouteData routeData, uint startLinkId, uint endLinkId)
        {
            RouteData newrouteData = new RouteData();
            List<UInt32> listSegments = new List<UInt32>();
            int count = routeData.ListSegments.Count;
            listSegments.Add(startLinkId);
            for (int i = 0; i < count; i++)
            {
                if (listSegments[0] != routeData.ListSegments[i])
                    listSegments.Add(routeData.ListSegments[i]);
            }
            if (routeData.ListSegments[count - 1] != endLinkId)
            {
                listSegments.Add(endLinkId);
            }
            newrouteData.ResponseMessage = routeData.ResponseMessage;
            newrouteData.ListSegments = listSegments;

            return newrouteData;
        }
        private RoutePath ConstructAutoReplanRoutePath(List<RoutePoint> routePoints, RouteData routeData)
        {
            RoutePath routePath = new RoutePath();
            var linkNo = 0;
            List<RouteLink> routeLinkList = new List<RouteLink>();
            RouteLink routeLink;
            foreach (var item in routeData.ListSegments)
            {
                routeLink = new RouteLink
                {
                    LinkId = item,
                    LinkNo = linkNo++,
                    Direction = 1
                };
                routeLinkList.Add(routeLink);
            }
            bool startPointFlag = false, endPointFlag = false;
            RouteSegment routeSegment = new RouteSegment();
            List<RouteSegment> routeSegmentList = new List<RouteSegment>();

            foreach (var rPoint in routePoints)
            {
                if (rPoint.PointType == 0)
                {
                    routeSegment.StartLinkId = rPoint.LinkId;
                    routeSegment.StartGeom = rPoint.RoutePointGeom;
                    routeSegment.StartLrs = rPoint.Lrs;
                    startPointFlag = true;
                }
                else if (rPoint.PointType == 1)
                {
                    routeSegment.EndLinkId = rPoint.LinkId;
                    routeSegment.EndGeom = rPoint.RoutePointGeom;
                    routeSegment.EndLrs = rPoint.Lrs;
                    endPointFlag = true;
                }
                if (startPointFlag && endPointFlag)
                {
                    break;
                }
            }
            routeSegment.SegmentNo = 1;
            routeSegment.SegmentType = 1;
            routeSegment.RouteLinkList = routeLinkList;
            routeSegmentList.Add(routeSegment);
            routePath.RouteSegmentList = routeSegmentList;
            routePath.RoutePointList = routePoints;
            routePath.RoutePathNo = 1;
            return routePath;
        }
        #endregion

        #region InsertNERoute
        [HttpPost]
        [Route("RouteImport/InsertNERoute")]
        public IHttpActionResult InsertNERoute(SaveNERouteParams saveNERoute)
        {
            long routeId;
            try
            {
                InputMessage = $", NotificationId:{saveNERoute.NotificationId}, RevisionId:{saveNERoute.RevisionId}";
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXProcessStart}{InputMessage}");
                var routePart = ConvertRoute(saveNERoute.GPXInput, out bool isAutoPlan);
                if (routePart != null)
                {
                    saveNERoute.RoutePart = routePart;
                    routeId = RouteManagerProvider.Instance.SaveNERoute(saveNERoute, isAutoPlan);
                    return Content(HttpStatusCode.Created, routeId);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, "Route cannot be imported to ESDAL.");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteImport/SaveNERoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion

        #region ImportToLibrary
        [HttpPost]
        [Route("RouteImport/ImportToLibrary")]
        public IHttpActionResult ImportToLibrary(ImportToLibraryParams libraryRoute)
        {
            long routeId;
            try
            {
                var routePart = ConvertRoute(libraryRoute.GPXInput, out bool isAutoPlan);
                if (routePart != null)
                {
                    libraryRoute.RoutePart = routePart;
                    libraryRoute.RoutePart.OrgId = libraryRoute.OrganisationId;
                    libraryRoute.RoutePart.RoutePartDetails.RouteName = libraryRoute.RouteName;
                    routeId = RouteManagerProvider.Instance.SaveLibraryRoute(libraryRoute.RoutePart, UserSchema.Portal);
                    return Content(HttpStatusCode.Created, routeId);
                }
                else
                    return Content(HttpStatusCode.InternalServerError, "Route cannot be imported to ESDAL.");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteImport/ImportToLibrary, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Get Historic Notif/ApplicationRoute
        [HttpPost]
        [Route("Routes/UpdateCloneHistoricAppRoute")]
        public IHttpActionResult UpdateCloneHistoricAppRoute(UpdateHistoricCloneRoute updateHistoricClone)
        {
            try
            {
                List<RoutePart> routePartList = RouteManagerProvider.Instance.UpdateCloneHistoricAppRoute(updateHistoricClone);
                RoutePart routePart;
                long routeId = 0;
                List<long> routeIds = new List<long>();
                Serialization serialization = new Serialization();
                bool isAutoPlan = false;
                foreach (var item in routePartList)
                {
                    routePart = ProcessHistoricRoutePath(item, ref isAutoPlan, updateHistoricClone.UserSchema);
                    routePart = serialization.SerializeRoutePart(routePart);
                    SaveAppRouteParams updateAppRouteParams = new SaveAppRouteParams
                    {
                        RoutePart = routePart,
                        VersionId = updateHistoricClone.VersionId,
                        RevisionId = updateHistoricClone.RevisionId,
                        ContRefNumber = updateHistoricClone.ContRefNumber,
                        DockFlag = 0,
                        RouteRevisionId = 0,
                        UserSchema = updateHistoricClone.UserSchema,
                        IsAutoPlan = isAutoPlan
                    };
                    routeId = RouteManagerProvider.Instance.UpdateApplicationRoute(updateAppRouteParams);
                    routeIds.Add(routeId);
                }
                if (routeIds.Count > 0)
                    return Content(HttpStatusCode.OK, routeIds);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"/GetHistoricApplicationRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Routes/GetHistoricAppRoute")]
        public IHttpActionResult GetHistoricAppRoute(long routeId, string userSchema)
        {
            try
            {
                RoutePart routePart = RouteManagerProvider.Instance.GetHistoricAppRoute(routeId, userSchema);
                if (routePart != null)
                    return Content(HttpStatusCode.OK, routePart);
                else
                    return Content(HttpStatusCode.OK, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"/GetHistoricApplicationRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        private RoutePart ProcessHistoricRoutePath(RoutePart routePart, ref bool isAutoPlan, string userSchema)
        {
            bool isAutomaticPlan = false;
            routePart.RoutePathList.ForEach(
                routePath =>
                {
                    routePath.RouteSegmentList.ForEach(
                        routeSegment =>
                        {
                            List<RouteLink> routeLinks = new List<RouteLink>();
                            if (routeSegment.OffRoadGeometry == null)
                            {
                                routeLinks = GetHistoricRouteLink(routeSegment, userSchema);
                                if (routeLinks.Count == 0)
                                {
                                    isAutomaticPlan = true;
                                    routeLinks = GetAutoPlanRouteLink(routeSegment, userSchema);
                                }
                            }
                            routeSegment.RouteLinkList = routeLinks;
                        });
                });
            isAutoPlan = isAutomaticPlan;
            return routePart;
        }

        private List<RouteLink> GetHistoricRouteLink(RouteSegment routeSegment, string userSchema)
        {
            List<RouteLink> routeLinkList = new List<RouteLink>();
            RouteLinkModel startSegLink = null, endSegLink = null;
            bool routeComplete = false;
            List<RouteLinkResult> resultLinks = ProcessRouteLinkList(routeSegment, null, userSchema, ref routeComplete, ref startSegLink, ref endSegLink);
            if (routeComplete)//Construct route links
                routeLinkList = ConstructRouteLink(resultLinks, routeSegment);

            return routeLinkList;
        }
        #endregion

        #region ProcessRouteLinkList
        private List<RouteLinkResult> ProcessRouteLinkList(RouteSegment routeSegment, wptType[] rtept, string userSchema, ref bool routeComplete, ref RouteLinkModel startSegLink, ref RouteLinkModel endSegLink)
        {
            RouteLinkModel startSegmentLink = null, endSegmentLink = null;
            RouteLinkModel secondLink = null, nextLink = null;
            List<String> roundaboutPref = new List<String> { "Y", "N" };
            List<RouteLinkResult> resultLinks = null;
            List<RouteLinkModel> routeLinkModel;
            
            for (int tolerance = MinTolerance; tolerance <= MaxTolerance && !routeComplete; tolerance += IncrementBuffer)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{ProcessRoadSegment}{tolerance}m{InputMessage}");
                resultLinks = new List<RouteLinkResult>();
                if (routeSegment != null)
                {
                    routeLinkModel = RouteManagerProvider.Instance.GetHistoricRouteLinkModel(routeSegment.SegmentId, routeSegment.SegmentNo, tolerance, userSchema);
                    startSegmentLink = routeLinkModel.Find(item => item.LinkId == routeSegment.StartLinkId);
                    endSegmentLink = routeLinkModel.Find(item => item.LinkId == routeSegment.EndLinkId);
                    startSegLink = startSegmentLink;
                    endSegLink = endSegmentLink;
                }
                else
                {
                    //get route ,
                    routeLinkModel = GetRouteLinks(rtept, new decimal[] { 1, 2, 1 }, 2002, 4326, tolerance);

                    //get start point in segment
                    startSegmentLink = GetNearestLinkFromRouteTrack(GetLinkFromPoint(rtept[0].lon, rtept[0].lat, 2001, 4326, rtept[0].name), null, routeLinkModel);

                    //get end point in segment
                    var lastCount = rtept.Count() - 1;
                    endSegmentLink = GetNearestLinkFromRouteTrack(GetLinkFromPoint(rtept[lastCount].lon, rtept[lastCount].lat, 2001, 4326, rtept[lastCount].name), null, routeLinkModel);

                    startSegLink = startSegmentLink;
                    endSegLink = endSegmentLink;
                    if (startSegmentLink == null || endSegmentLink == null)
                    {
                        if (tolerance < MaxTolerance)
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{RetryMessage}{InputMessage}");
                        else
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{TerminateMessage}{InputMessage}");
                        continue;
                    }
                }
                //find second link from the route link list
                while (routeLinkModel.Count > 0)
                {
                    secondLink = routeLinkModel.Where(r => r.LinkId != startSegmentLink.LinkId
                                                && (r.RefInId == startSegmentLink.RefInId || r.RefInId == startSegmentLink.NrefInId
                                                || r.NrefInId == startSegmentLink.RefInId || r.NrefInId == startSegmentLink.NrefInId))
                        .OrderBy(r =>
                        {
                            var index = roundaboutPref.IndexOf(r.IsRoundabout);
                            return index == -1 ? int.MaxValue : index;
                        })
                        .FirstOrDefault();
                    if (secondLink != null)
                    {
                        if (startSegmentLink.RefInId == secondLink.RefInId && secondLink.DirTravel != "T")
                        {
                            resultLinks.Add(ConstructRouteLinkResult(startSegmentLink.LinkId, 0, 0, startSegmentLink.RefInId));
                            resultLinks.Add(ConstructRouteLinkResult(secondLink.LinkId, 1, 1, secondLink.NrefInId));
                            break;
                        }
                        else if (startSegmentLink.RefInId == secondLink.NrefInId && secondLink.DirTravel != "F")
                        {
                            resultLinks.Add(ConstructRouteLinkResult(startSegmentLink.LinkId, 0, 0, startSegmentLink.RefInId));
                            resultLinks.Add(ConstructRouteLinkResult(secondLink.LinkId, 1, 0, secondLink.RefInId));
                            break;
                        }
                        else if (startSegmentLink.NrefInId == secondLink.RefInId && secondLink.DirTravel != "T")
                        {
                            resultLinks.Add(ConstructRouteLinkResult(startSegmentLink.LinkId, 0, 1, startSegmentLink.NrefInId));
                            resultLinks.Add(ConstructRouteLinkResult(secondLink.LinkId, 1, 1, secondLink.NrefInId));
                            break;
                        }
                        else if (startSegmentLink.NrefInId == secondLink.NrefInId && secondLink.DirTravel != "F")
                        {
                            resultLinks.Add(ConstructRouteLinkResult(startSegmentLink.LinkId, 0, 1, startSegmentLink.NrefInId));
                            resultLinks.Add(ConstructRouteLinkResult(secondLink.LinkId, 1, 0, secondLink.RefInId));
                            break;
                        }
                        routeLinkModel.Remove(secondLink);
                    }
                    else
                    {
                        break;
                    }
                }
                if (secondLink != null)
                {
                    //find next link from the route link list until last
                    while (routeLinkModel.Count > 0)
                    {
                        //nextLink = null
                        if (resultLinks.Count > 0)
                        {
                            var lastCount = resultLinks.Count - 1;
                            nextLink = routeLinkModel.Where(r => r.LinkId != resultLinks[lastCount].LinkId
                                                && (r.RefInId == resultLinks[lastCount].LastNode
                                                || r.NrefInId == resultLinks[lastCount].LastNode))
                                        .OrderBy(r => r.FuncClass)
                                        .ThenBy(r =>
                                        {
                                            var index = roundaboutPref.IndexOf(r.IsRoundabout);
                                            return index == -1 ? int.MaxValue : index;
                                        })
                                        //.ThenByDescending(r => r.StreetName?.StartsWith(resultLinks.Last().StreetName) ?? false)
                                        .FirstOrDefault();

                            if (nextLink != null)
                            {
                                if (resultLinks[lastCount].LastNode == nextLink.RefInId && nextLink.DirTravel != "T")
                                {
                                    resultLinks.Add(ConstructRouteLinkResult(nextLink.LinkId, resultLinks.Count, 1, nextLink.NrefInId));
                                }
                                else if (resultLinks[lastCount].LastNode == nextLink.NrefInId && nextLink.DirTravel != "F")
                                {
                                    resultLinks.Add(ConstructRouteLinkResult(nextLink.LinkId, resultLinks.Count, 0, nextLink.RefInId));
                                }

                                if (nextLink.LinkId == endSegmentLink.LinkId)
                                {
                                    routeComplete = true;
                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXProcessCompleted}{tolerance}m{InputMessage}");
                                    break;
                                }
                                routeLinkModel.Remove(nextLink);
                            }
                            else
                            {
                                if (resultLinks.Any())
                                {
                                    resultLinks.RemoveAt(resultLinks.Count - 1);
                                }
                                else
                                {
                                    routeComplete = false;
                                    if (tolerance < MaxTolerance)
                                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{RetryMessage}{InputMessage}");
                                    else
                                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{TerminateMessage}{InputMessage}");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            routeComplete = false;
                            if (tolerance < MaxTolerance)
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{RetryMessage}{InputMessage}");
                            else
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{TerminateMessage}{InputMessage}");
                            break;
                        }
                    }
                }
                else
                {
                    routeComplete = false;
                    if (tolerance < MaxTolerance)
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{RetryMessage}{InputMessage}");
                    else
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXMapping}{tolerance}m{TerminateMessage}{InputMessage}");
                }
            }
            return resultLinks;
        }
        private List<RouteLink> GetAutoPlanRouteLink(RouteSegment routeSegment, string userSchema)
        {
            List<RoutePoint> routePoints = RouteImport.Instance.GetRoutePointForReplan(routeSegment, userSchema);
            RouteData routeData = GetRouteDataAutoReplan(routePoints);
            List<RouteLink> routeLinkList = ConstructAutoReplanRouteLink(routeSegment, routeData);
            return routeLinkList;
        }
        private List<RouteLink> ConstructAutoReplanRouteLink(RouteSegment routeSegment, RouteData routeData)
        {
            var linkNo = 0;
            List<RouteLink> routeLinkList = new List<RouteLink>();
            RouteLink routeLink;
            foreach (var item in routeData.ListSegments)
            {
                routeLink = new RouteLink
                {
                    SegmentId = routeSegment.SegmentId,
                    SegmentNo = routeSegment.SegmentNo,
                    LinkId = item,
                    LinkNo = linkNo++,
                    Direction = 1
                };
                routeLinkList.Add(routeLink);
            }
            return routeLinkList;
        }
        #endregion

        #region TestRouteImport
        [HttpPost]
        [Route("RouteImport/TestRouteImport")]
        public IHttpActionResult TestRouteImport(GPXInputModel GPX)
        {
            try
            {
                InputMessage = $", NotificationId:{GPX.NotificationId}, RevisionId:{GPX.RevisionId}";
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"{GPXProcessStart}{InputMessage}");

                GPXInputModel gPXInput = new GPXInputModel { RouteGPX = Domain.Custom.StringExtraction.Base64Decode(GPX.RouteGPX) };
                var routePart = ConvertRoute(gPXInput, out bool isAutoPlan);
                if (routePart != null)
                    return Content(HttpStatusCode.Created, routePart);
                else
                    return Content(HttpStatusCode.InternalServerError, "Route cannot be imported to ESDAL.");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteImport/ImportToLibrary, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Clone NEN Route
        [HttpPost]
        [Route("RouteImport/CloneNenRoute")]
        public IHttpActionResult CloneNenRoute(CloneNenRoute cloneNenRoute)
        {
            try
            {
                List<NenRouteList> nenRouteLists = RouteManagerProvider.Instance.CloneNenRoute(cloneNenRoute);
                if(nenRouteLists.Count > 0)
                    return Content(HttpStatusCode.Created, nenRouteLists);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteImport/SaveNERoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion

    }
}
