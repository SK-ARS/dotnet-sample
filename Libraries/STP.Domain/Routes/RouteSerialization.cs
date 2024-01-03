using NetSdoGeometry;
using Newtonsoft.Json;
using System.Collections.Generic;
using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class RouteSerialization
    {
        public class Serialization
        {
            #region Serailize RoutePart
            public RoutePart SerializeRoutePart(RoutePart routePart)
            {
                routePart.RoutePathList = SerializeRoutePath(routePart.RoutePathList);
                routePart.RoutePartDetails.PartGeom = JsonConvert.SerializeObject(routePart.RoutePartDetails.PartGeometry);
                routePart.RoutePartDetails.PartGeometry = null;
                return routePart;
            }
            public List<RoutePath> SerializeRoutePath(List<RoutePath> routePathList)
            {
                routePathList.ForEach(
                    routePath =>
                    {
                        routePath.RouteSegmentList.ForEach(routeSegment =>
                        {
                            routeSegment.EndGeom = JsonConvert.SerializeObject(routeSegment.EndPointGeometry);
                            routeSegment.OffGeom = JsonConvert.SerializeObject(routeSegment.OffRoadGeometry);
                            routeSegment.StartGeom = JsonConvert.SerializeObject(routeSegment.StartPointGeometry);
                            routeSegment.EndPointGeometry = null;
                            routeSegment.OffRoadGeometry = null;
                            routeSegment.StartPointGeometry = null;
                            if (routeSegment.RouteAnnotationsList != null)
                            {
                                routeSegment.RouteAnnotationsList.ForEach(
                                    routeAnnotation =>
                                    {
                                        routeAnnotation.Geom = JsonConvert.SerializeObject(routeAnnotation.Geometry);
                                        routeAnnotation.RoadGeom = JsonConvert.SerializeObject(routeAnnotation.RoadGeometry);
                                        routeAnnotation.Geometry = null;
                                        routeAnnotation.RoadGeometry = null;
                                    });
                            }
                        });
                        routePath.RoutePointList.ForEach(routePoint =>
                        {
                            routePoint.RoutePointGeom = JsonConvert.SerializeObject(routePoint.PointGeom);
                            routePoint.TruePointGeom = JsonConvert.SerializeObject(routePoint.RoadGeometry);
                            routePoint.PointGeom = null;
                            routePoint.RoadGeometry = null;
                        });
                    });
                return routePathList;
            }
            #endregion

            #region Deserialize RoutePart
            public RoutePart DeserializeRoutePart(RoutePart routePart)
            {
                routePart.RoutePathList = DeserializeRoutePath(routePart.RoutePathList);
                if (routePart.RoutePartDetails.PartGeom != null)
                    routePart.RoutePartDetails.PartGeometry = JsonConvert.DeserializeObject<sdogeometry>(routePart.RoutePartDetails.PartGeom);
                routePart.RoutePartDetails.PartGeom = null;

                return routePart;
            }
            public List<RoutePath> DeserializeRoutePath(List<RoutePath> routePathList)
            {
                routePathList.ForEach(
                    routePath =>
                    {
                        routePath.RouteSegmentList.ForEach(routeSegment =>
                        {
                            if (routeSegment.EndGeom != null)
                                routeSegment.EndPointGeometry = JsonConvert.DeserializeObject<sdogeometry>(routeSegment.EndGeom);
                            if (routeSegment.OffGeom != null)
                                routeSegment.OffRoadGeometry = JsonConvert.DeserializeObject<sdogeometry>(routeSegment.OffGeom);
                            if (routeSegment.StartGeom != null)
                                routeSegment.StartPointGeometry = JsonConvert.DeserializeObject<sdogeometry>(routeSegment.StartGeom);
                            routeSegment.EndGeom = null;
                            routeSegment.OffGeom = null;
                            routeSegment.StartGeom = null;
                            if (routeSegment.RouteAnnotationsList != null)
                            {
                                routeSegment.RouteAnnotationsList.ForEach(
                                    routeAnnotation =>
                                    {
                                        if (routeAnnotation.Geom != null)
                                            routeAnnotation.Geometry = JsonConvert.DeserializeObject<sdogeometry>(routeAnnotation.Geom);
                                        if (routeAnnotation.RoadGeom != null)
                                            routeAnnotation.RoadGeometry = JsonConvert.DeserializeObject<sdogeometry>(routeAnnotation.RoadGeom);
                                        routeAnnotation.Geom = null;
                                        routeAnnotation.RoadGeom = null;
                                    });
                            }
                        });
                        routePath.RoutePointList.ForEach(routePoint =>
                        {
                            if (routePoint.RoutePointGeom != null)
                                routePoint.PointGeom = JsonConvert.DeserializeObject<sdogeometry>(routePoint.RoutePointGeom);
                            if (routePoint.TruePointGeom != null)
                                routePoint.RoadGeometry = JsonConvert.DeserializeObject<sdogeometry>(routePoint.TruePointGeom);
                            routePoint.RoutePointGeom = null;
                            routePoint.TruePointGeom = null;
                        });
                    });
                return routePathList;
            }
            #endregion
        }
    }
}
