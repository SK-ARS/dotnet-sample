using NetSdoGeometry;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using PagedList;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace STP.Domain.Routes
{
    public class RouteModel
    {
        #region RoutePartDetails
        /// <summary>
        /// PlannedRouteDetails Class
        /// </summary>
        public class RoutePartDetailsList
        {
            public List<RoutePartDetails> RoutePartDetails = new List<RoutePartDetails>();
        }

            #endregion

        #region RoutePartDetails
            /// <summary>
            /// PlannedRouteDetails Class
            /// </summary>
            [DataContract]
        public class RoutePartDetails
        {
            [DataMember]
            public long RouteId { get; set; }
            [DataMember]
            public int RoutePartNo { get; set; }
            [DataMember]
            public string RouteName { get; set; }
            [DataMember]
            public string RouteDescr { get; set; }
            [DataMember]
            public int IsComplete { get; set; }
            [DataMember]
            public sdogeometry PartGeometry { get; set; }
            [DataMember]
            public bool SaveAsNew { get; set; }
            [DataMember]
            public bool DockCaution { get; set; }
            [DataMember]
            public bool WayPointAddress { get; set; }
            [DataMember]
            public bool WayPointContactDetails { get; set; }
            //For Library route
            [DataMember]
            public string RouteType { get; set; }
            [DataMember]
            public string StartDesc { get; set; }
            [DataMember]
            public string EndDesc { get; set; }
            [DataMember]
            public int IsBrokenLib { get; set; }
            //For NEN
            [DataMember]
            public int AnalysisId { get; set; }
            [DataMember]
            public int TotalRecord { get; set; }
            [DataMember]
            public string PartGeom { get; set; }
            [DataMember]
            public int IsFavourites { get; set; }
            [DataMember]
            public DateTime LastUpdate { get; set; }
        }
        #endregion

        #region RoutePath
        [DataContract]
        public class RoutePath : INullable, IOracleCustomType
        {
            private bool m_bIsNull;

            [DataMember]
            [OracleObjectMapping("ROUTE_PATH_ID")]
            public long RoutePathId { get; set; }

            [DataMember]
            public long RouteId { get; set; }

            [DataMember]
            [OracleObjectMapping("ROUTE_PATH_NO")]
            public int RoutePathNo { get; set; }

            [DataMember]
            public int RoutePathType { get; set; }

            [DataMember]
            public long RouteVariant { get; set; }

            [DataMember]
            [OracleObjectMapping("PATH_DESCR")]
            public string PathDescr { get; set; }

            [DataMember]
            public List<RouteSegment> RouteSegmentList { get; set; }

            [DataMember]
            [OracleObjectMapping("ROUTE_SEGMENT_ARRAY")]
            public RouteSegmentArray RouteSegmentArray { get; set; }

            [DataMember]
            public List<RoutePoint> RoutePointList { get; set; }
            [DataMember]
            public List<RoutePoint> AlternatePointList { get; set; }

            [DataMember]
            [OracleObjectMapping("ROUTE_POINT_ARRAY")]
            public RoutePointArray RoutePointArray { get; set; }
            public RoutePath()
            {
                RouteSegmentList = new List<RouteSegment>();
                RouteSegmentArray = new RouteSegmentArray();
                RoutePointList = new List<RoutePoint>();
                RoutePointArray = new RoutePointArray();
                AlternatePointList = new List<RoutePoint>();
            }
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RoutePath Null
            {
                get
                {
                    RoutePath p = new RoutePath
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "ROUTE_PATH_ID", RoutePathId);
                OracleUdt.SetValue(con, pUdt, "ROUTE_PATH_NO", RoutePathNo);
                OracleUdt.SetValue(con, pUdt, "PATH_DESCR", PathDescr);
                OracleUdt.SetValue(con, pUdt, "ROUTE_SEGMENT_ARRAY", RouteSegmentArray);
                OracleUdt.SetValue(con, pUdt, "ROUTE_POINT_ARRAY", RoutePointArray);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RoutePathId = (long)OracleUdt.GetValue(con, pUdt, "ROUTE_PATH_ID");
                RoutePathNo = (int)OracleUdt.GetValue(con, pUdt, "ROUTE_PATH_NO");
                PathDescr = (string)OracleUdt.GetValue(con, pUdt, "PATH_DESCR");
                RouteSegmentArray = (RouteSegmentArray)OracleUdt.GetValue(con, pUdt, "ROUTE_SEGMENT_ARRAY");
                RoutePointArray = (RoutePointArray)OracleUdt.GetValue(con, pUdt, "ROUTE_POINT_ARRAY");
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTEPATH")]
        public class RoutePathFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new RoutePath();
            }
        }
        public class RoutePathArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public RoutePath[] RoutePathObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RoutePathArray Null
            {
                get
                {
                    RoutePathArray p = new RoutePathArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, RoutePathObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RoutePathObj = (RoutePath[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTEPATHARRAY")]
        public class RoutePathArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new RoutePathArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new RoutePath[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new RoutePath[numElems];
            }
            #endregion
        }
        #endregion

        #region RoutePoint
        [DataContract]
        public class RoutePoint : INullable, IOracleCustomType
        {
            private bool m_bIsNull;

            [DataMember]
            [OracleObjectMapping("ROUTE_POINT_ID")]
            public long RoutePointId { get; set; }

            [DataMember]
            [OracleObjectMapping("ROUTE_POINT_NO")]
            public int RoutePointNo { get; set; }
            [DataMember]
            [OracleObjectMapping("ROUTE_POINT_TYPE")]
            public int? PointType { get; set; }

            [DataMember]
            [OracleObjectMapping("DESCR")]
            public string PointDescr { get; set; }

            [DataMember]
            [OracleObjectMapping("LINEAR_REF")]
            public int? Lrs { get; set; }

            [DataMember]
            [OracleObjectMapping("DIRECTION")]
            public int? Direction { get; set; }

            [DataMember]
            [OracleObjectMapping("LINK_ID")]
            public long LinkId { get; set; }

            [DataMember]
            [OracleObjectMapping("ROAD_POINT_GEOMETRY")]
            public sdogeometry PointGeom { get; set; }

            [DataMember]
            [OracleObjectMapping("TRUE_POINT_GEOMETRY")]
            public sdogeometry RoadGeometry { get; set; }

            [DataMember]
            [OracleObjectMapping("WAY_TEXT")]
            public string WayText { get; set; }

            [DataMember]
            [OracleObjectMapping("IS_ANCHOR_POINT")]
            public int IsAnchorPoint { get; set; }

            [DataMember]
            public long RoutePathId { get; set; }

            [DataMember]
            public string PointAnnotation { get; set; }

            [DataMember]
            public int ShowRoutePoint { get; set; }

            //For Broken Route replanner
            [DataMember]
            public long NewLinkId { get; set; }

            [DataMember]
            public long NewBeginNodeId { get; set; }

            [DataMember]
            public long NewEndNodeId { get; set; }

            [DataMember]
            public decimal DistanceToNewLink { get; set; }

            //For NEN Route
            [DataMember]
            public long Northing { get; set; }

            [DataMember]
            public long Easting { get; set; }

            [DataMember]
            public string RoutePointGeom { get; set; }
            [DataMember]
            public string TruePointGeom { get; set; }

            public RoutePoint()
            {
                ShowRoutePoint = 1;
            }

            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RoutePoint Null
            {
                get
                {
                    RoutePoint p = new RoutePoint
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "ROUTE_POINT_ID", RoutePointId);
                OracleUdt.SetValue(con, pUdt, "ROUTE_POINT_NO", RoutePointNo);
                OracleUdt.SetValue(con, pUdt, "ROUTE_POINT_TYPE", PointType);
                OracleUdt.SetValue(con, pUdt, "DESCR", PointDescr);
                OracleUdt.SetValue(con, pUdt, "LINEAR_REF", Lrs);
                OracleUdt.SetValue(con, pUdt, "DIRECTION", Direction);
                OracleUdt.SetValue(con, pUdt, "LINK_ID", LinkId);
                OracleUdt.SetValue(con, pUdt, "ROAD_POINT_GEOMETRY", PointGeom);
                OracleUdt.SetValue(con, pUdt, "TRUE_POINT_GEOMETRY", RoadGeometry);
                OracleUdt.SetValue(con, pUdt, "WAY_TEXT", WayText);
                OracleUdt.SetValue(con, pUdt, "IS_ANCHOR_POINT", IsAnchorPoint);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RoutePointId = (long)OracleUdt.GetValue(con, pUdt, "ROUTE_POINT_ID");
                RoutePointNo = (int)OracleUdt.GetValue(con, pUdt, "ROUTE_POINT_NO");
                PointType = (int)OracleUdt.GetValue(con, pUdt, "ROUTE_POINT_TYPE");
                PointDescr = (string)OracleUdt.GetValue(con, pUdt, "DESCR");
                Lrs = (int)OracleUdt.GetValue(con, pUdt, "LINEAR_REF");
                Direction = (int)OracleUdt.GetValue(con, pUdt, "DIRECTION");
                LinkId = (long)OracleUdt.GetValue(con, pUdt, "LINK_ID");
                PointGeom = (sdogeometry)OracleUdt.GetValue(con, pUdt, "ROAD_POINT_GEOMETRY");
                RoadGeometry = (sdogeometry)OracleUdt.GetValue(con, pUdt, "TRUE_POINT_GEOMETRY");
                WayText = (string)OracleUdt.GetValue(con, pUdt, "WAY_TEXT");
                IsAnchorPoint = (int)OracleUdt.GetValue(con, pUdt, "IS_ANCHOR_POINT");
            }
        }
        [OracleCustomTypeMapping("PORTAL.ROUTEPOINT")]
        public class RoutePointFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new RoutePoint();
            }
        }
        public class RoutePointArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public RoutePoint[] RoutePointObj
            { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RoutePointArray Null
            {
                get
                {
                    RoutePointArray p = new RoutePointArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, RoutePointObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RoutePointObj = (RoutePoint[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTEPOINTARRAY")]
        public class RoutePointArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new RoutePointArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new RoutePoint[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new RoutePoint[numElems];
            }
            #endregion
        }
        #endregion

        #region RouteSegment
        [DataContract]
        public class RouteSegment : INullable, IOracleCustomType
        {
            private bool m_bIsNull;

            [DataMember]
            [OracleObjectMapping("SEGMENT_ID")]
            public long SegmentId { get; set; }

            [DataMember]
            [OracleObjectMapping("SEGMENT_NO")]
            public int SegmentNo { get; set; }

            [DataMember]
            public long RoutePathId { get; set; }

            [DataMember]
            [OracleObjectMapping("SEGMENT_DESCR")]
            public string SegmentDesc { get; set; }

            [DataMember]
            [OracleObjectMapping("OFF_ROAD_GEOMETRY")]
            public sdogeometry OffRoadGeometry { get; set; }

            [DataMember]
            [OracleObjectMapping("START_LINK_ID")]
            public long? StartLinkId { get; set; }

            [DataMember]
            [OracleObjectMapping("END_LINK_ID")]
            public long? EndLinkId { get; set; }

            [DataMember]
            [OracleObjectMapping("START_LINEAR_REF")]
            public int? StartLrs { get; set; }

            [DataMember]
            [OracleObjectMapping("START_POINT_GEOMETRY")]
            public sdogeometry StartPointGeometry { get; set; }

            [DataMember]
            [OracleObjectMapping("END_LINEAR_REF")]
            public int? EndLrs { get; set; }

            [DataMember]
            [OracleObjectMapping("END_POINT_GEOMETRY")]
            public sdogeometry EndPointGeometry { get; set; }

            [DataMember]
            [OracleObjectMapping("START_POINT_DIRECTION")]
            public int? StartPointDirection { get; set; }

            [DataMember]
            [OracleObjectMapping("END_POINT_DIRECTION")]
            public int? EndPointDirection { get; set; }

            [DataMember]
            [OracleObjectMapping("SEGMENT_TYPE")]
            public int SegmentType { get; set; }

            [DataMember]
            public List<RouteLink> RouteLinkList { get; set; }

            [DataMember]
            [OracleObjectMapping("ROUTE_LINK_ARRAY")]
            public RouteLinkArray RouteLinkArray { get; set; }

            [DataMember]
            public List<RouteAnnotation> RouteAnnotationsList { get; set; }

            [DataMember]
            [OracleObjectMapping("ROUTE_ANNOTATION_ARRAY")]
            public RouteAnnotationArray RouteAnnotationArray { get; set; }

            [DataMember]
            public string OffGeom { get; set; }

            [DataMember]
            public string StartGeom { get; set; }

            [DataMember]
            public string EndGeom { get; set; }
            
            [DataMember]
            public sdogeometry LinkGeometry { get; set; }

            public string LinkGeom { get; set; }

            public RouteSegment()
            {
                RouteLinkList = new List<RouteLink>();
                RouteLinkArray = new RouteLinkArray();
                RouteAnnotationsList = new List<RouteAnnotation>();
                RouteAnnotationArray = new RouteAnnotationArray();
            }

            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RouteSegment Null
            {
                get
                {
                    RouteSegment p = new RouteSegment
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "SEGMENT_ID", SegmentId);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_NO", SegmentNo);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_DESCR", SegmentDesc);
                OracleUdt.SetValue(con, pUdt, "OFF_ROAD_GEOMETRY", OffRoadGeometry);
                OracleUdt.SetValue(con, pUdt, "START_LINK_ID", StartLinkId);
                OracleUdt.SetValue(con, pUdt, "START_LINEAR_REF", StartLrs);
                OracleUdt.SetValue(con, pUdt, "START_POINT_GEOMETRY", StartPointGeometry);
                OracleUdt.SetValue(con, pUdt, "START_POINT_DIRECTION", StartPointDirection);
                OracleUdt.SetValue(con, pUdt, "END_LINK_ID", EndLinkId);
                OracleUdt.SetValue(con, pUdt, "END_LINEAR_REF", EndLrs);
                OracleUdt.SetValue(con, pUdt, "END_POINT_GEOMETRY", EndPointGeometry);
                OracleUdt.SetValue(con, pUdt, "END_POINT_DIRECTION", EndPointDirection);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_TYPE", SegmentType);
                OracleUdt.SetValue(con, pUdt, "ROUTE_LINK_ARRAY", RouteLinkArray);
                OracleUdt.SetValue(con, pUdt, "ROUTE_ANNOTATION_ARRAY", RouteAnnotationArray);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                SegmentId = (long)OracleUdt.GetValue(con, pUdt, "SEGMENT_ID");
                SegmentNo = (int)OracleUdt.GetValue(con, pUdt, "SEGMENT_NO");
                SegmentDesc = (string)OracleUdt.GetValue(con, pUdt, "SEGMENT_DESCR");
                OffRoadGeometry = (sdogeometry)OracleUdt.GetValue(con, pUdt, "OFF_ROAD_GEOMETRY");
                StartLinkId = (long?)OracleUdt.GetValue(con, pUdt, "START_LINK_ID");
                StartLrs = (int?)OracleUdt.GetValue(con, pUdt, "START_LINEAR_REF");
                StartPointGeometry = (sdogeometry)OracleUdt.GetValue(con, pUdt, "START_POINT_GEOMETRY");
                StartPointDirection = (int?)OracleUdt.GetValue(con, pUdt, "START_POINT_DIRECTION");
                EndLinkId = (long?)OracleUdt.GetValue(con, pUdt, "END_LINK_ID");
                EndLrs = (int?)OracleUdt.GetValue(con, pUdt, "END_LINEAR_REF");
                EndPointGeometry = (sdogeometry)OracleUdt.GetValue(con, pUdt, "END_POINT_GEOMETRY");
                EndPointDirection = (int?)OracleUdt.GetValue(con, pUdt, "END_POINT_DIRECTION");
                SegmentType = (int)OracleUdt.GetValue(con, pUdt, "SEGMENT_TYPE");
                RouteLinkArray = (RouteLinkArray)OracleUdt.GetValue(con, pUdt, "ROUTE_LINK_ARRAY");
                RouteAnnotationArray = (RouteAnnotationArray)OracleUdt.GetValue(con, pUdt, "ROUTE_ANNOTATION_ARRAY");
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTESEGMENT")]
        public class RouteSegmnetFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new RouteSegment();
            }
        }
        public class RouteSegmentArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public RouteSegment[] SegmentObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RouteSegmentArray Null
            {
                get
                {
                    RouteSegmentArray p = new RouteSegmentArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, SegmentObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                SegmentObj = (RouteSegment[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTESEGMENTARRAY")]
        public class RouteSegmentArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new RouteSegmentArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new RouteSegment[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new RouteSegment[numElems];
            }
            #endregion
        }
        #endregion

        #region RouteLink
        [DataContract]
        public class RouteLink : INullable, IOracleCustomType
        {
            private bool m_bIsNull;

            [DataMember]
            public long SegmentId { get; set; }

            [DataMember]
            public int SegmentNo { get; set; }

            [DataMember]
            [OracleObjectMapping("LINK_NO")]
            public int? LinkNo { get; set; }    //Modified to included null values coming for routes having off road

            [DataMember]
            [OracleObjectMapping("LINK_ID")]
            public long? LinkId { get; set; } // Should be of type long

            [DataMember]
            [OracleObjectMapping("DIRECTION")]
            public int? Direction { get; set; }
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RouteLink Null
            {
                get
                {
                    RouteLink p = new RouteLink
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "LINK_NO", LinkNo);
                OracleUdt.SetValue(con, pUdt, "LINK_ID", LinkId);
                OracleUdt.SetValue(con, pUdt, "DIRECTION", Direction);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                LinkNo = (int)OracleUdt.GetValue(con, pUdt, "LINK_NO");
                LinkId = (long?)OracleUdt.GetValue(con, pUdt, "LINK_ID");
                Direction = (int?)OracleUdt.GetValue(con, pUdt, "DIRECTION");
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTELINKS")]
        public class RouteLinksFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new RouteLink();
            }
        }
        public class RouteLinkArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public RouteLink[] RouteLinkObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RouteLinkArray Null
            {
                get
                {
                    RouteLinkArray p = new RouteLinkArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, RouteLinkObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RouteLinkObj = (RouteLink[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTELINKSARRAY")]
        public class RouteLinkArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new RouteLinkArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new RouteLink[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new RouteLink[numElems];
            }
            #endregion
        }
        #endregion

        #region RouteAnnotation
        /// <summary>
        /// RouteAnnotation
        /// </summary>
        [DataContract]
        public class RouteAnnotation : INullable, IOracleCustomType
        {
            private bool m_bIsNull;

            [DataMember]
            [OracleObjectMapping("ANNOTATION_ID")]
            public long AnnotationID { get; set; }

            [DataMember]
            [OracleObjectMapping("ANNOT_TYPE")]
            public int AnnotType { get; set; }

            [DataMember]
            [OracleObjectMapping("ANNOT_TEXT")]
            public string AnnotText { get; set; }

            [DataMember]
            [OracleObjectMapping("ASSOC_TYPE")]
            public long AssocType { get; set; }

            [DataMember]
            [OracleObjectMapping("EASTING")]
            public long Easting { get; set; }

            [DataMember]
            [OracleObjectMapping("NORTHING")]
            public long Northing { get; set; }

            [DataMember]
            [OracleObjectMapping("GEOMETRY")]
            public sdogeometry Geometry { get; set; }

            [DataMember]
            [OracleObjectMapping("DIRECTION")]
            public short Direction { get; set; }

            [DataMember]
            [OracleObjectMapping("STRUCTURE_ESRN")]
            public string StructureEsrn { get; set; }

            [DataMember]
            [OracleObjectMapping("CONSTRAINT_ESRN")]
            public string ConstraintEsrn { get; set; }

            [DataMember]
            [OracleObjectMapping("LINK_ID")]
            public long LinkId { get; set; }
            [DataMember]
            [OracleObjectMapping("LINEAR_REF")]
            public int LinearRef { get; set; }

            [DataMember]
            [OracleObjectMapping("IS_BROKEN")]
            public short IsBroken { get; set; }

            [DataMember]
            [OracleObjectMapping("IN_ROUTE_DESCRIPTION")]
            public short InRouteDescription { get; set; }

            [DataMember]
            [OracleObjectMapping("SEGMENT_ID")]
            public long SegmentId { get; set; }

            [DataMember]
            [OracleObjectMapping("SEGMENT_NO")]
            public int SegmentNo { get; set; }

            [DataMember]
            public List<AnnotationContact> AnnotationContactList { set; get; }

            [DataMember]
            [OracleObjectMapping("ANNOTATION_CONTACT_ARRAY")]
            public AnnotationContactArray AnnotationContactArray { set; get; }

            [DataMember]
            public long NewLinkId { get; set; }

            [DataMember]
            public decimal DistanceToNewLink { get; set; }

            [DataMember]
            public sdogeometry RoadGeometry { get; set; }

            [DataMember]
            public string Geom { get; set; }

            [DataMember]
            public string RoadGeom { get; set; }

          

            public RouteAnnotation()
            {
                Geometry = new sdogeometry();
                AnnotationContactList = new List<AnnotationContact>();
                AnnotationContactArray = new AnnotationContactArray();
            }

            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RouteAnnotation Null
            {
                get
                {
                    RouteAnnotation p = new RouteAnnotation
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "ANNOTATION_ID", AnnotationID);
                OracleUdt.SetValue(con, pUdt, "ANNOT_TYPE", AnnotType); 
                OracleUdt.SetValue(con, pUdt, "ANNOT_TEXT", AnnotText);
                OracleUdt.SetValue(con, pUdt, "ASSOC_TYPE", AssocType);
                OracleUdt.SetValue(con, pUdt, "EASTING", Easting);
                OracleUdt.SetValue(con, pUdt, "NORTHING", Northing);
                OracleUdt.SetValue(con, pUdt, "GEOMETRY", Geometry);
                OracleUdt.SetValue(con, pUdt, "DIRECTION", Direction);
                OracleUdt.SetValue(con, pUdt, "STRUCTURE_ESRN", StructureEsrn);
                OracleUdt.SetValue(con, pUdt, "CONSTRAINT_ESRN", ConstraintEsrn);
                OracleUdt.SetValue(con, pUdt, "LINK_ID", LinkId);
                OracleUdt.SetValue(con, pUdt, "LINEAR_REF", LinearRef);
                OracleUdt.SetValue(con, pUdt, "IS_BROKEN", IsBroken);
                OracleUdt.SetValue(con, pUdt, "IN_ROUTE_DESCRIPTION", InRouteDescription);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_ID", SegmentId);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_NO", SegmentNo);
                OracleUdt.SetValue(con, pUdt, "ANNOTATION_CONTACT_ARRAY", AnnotationContactArray);
            }
            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                AnnotationID = (long)OracleUdt.GetValue(con, pUdt, "ANNOTATION_ID");
                AnnotType = (int)OracleUdt.GetValue(con, pUdt, "ANNOT_TYPE");
                AnnotText = (string)OracleUdt.GetValue(con, pUdt, "ANNOT_TEXT");
                AssocType = (long)OracleUdt.GetValue(con, pUdt, "ASSOC_TYPE");
                Easting = (long)OracleUdt.GetValue(con, pUdt, "EASTING");
                Northing = (long)OracleUdt.GetValue(con, pUdt, "NORTHING");
                Geometry = (sdogeometry)OracleUdt.GetValue(con, pUdt, "GEOMETRY");
                Direction = (short)OracleUdt.GetValue(con, pUdt, "DIRECTION");
                StructureEsrn = (string)OracleUdt.GetValue(con, pUdt, "STRUCTURE_ESRN");
                ConstraintEsrn = (string)OracleUdt.GetValue(con, pUdt, "CONSTRAINT_ESRN");
                LinkId = (long)OracleUdt.GetValue(con, pUdt, "LINK_ID");
                LinearRef = (int)OracleUdt.GetValue(con, pUdt, "LINEAR_REF");
                IsBroken = (short)OracleUdt.GetValue(con, pUdt, "IS_BROKEN");
                InRouteDescription = (short)OracleUdt.GetValue(con, pUdt, "IN_ROUTE_DESCRIPTION");
                SegmentId = (long)OracleUdt.GetValue(con, pUdt, "SEGMENT_ID");
                SegmentNo = (int)OracleUdt.GetValue(con, pUdt, "SEGMENT_NO");
                AnnotationContactArray = (AnnotationContactArray)OracleUdt.GetValue(con, pUdt, "ANNOTATION_CONTACT_ARRAY");
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTEANNOTATION")]
        public class RouteAnnotationsFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                return new RouteAnnotation();
            }
        }

        public class RouteAnnotationArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public RouteAnnotation[] RouteAnnotationObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RouteAnnotationArray Null
            {
                get
                {
                    RouteAnnotationArray p = new RouteAnnotationArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, RouteAnnotationObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RouteAnnotationObj = (RouteAnnotation[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.ROUTEANNOTATIONARRAY")]
        public class RouteAnnotationArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new RouteAnnotationArray();
            }
            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new RouteAnnotation[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new RouteAnnotation[numElems];
            }
            #endregion
        }

        #endregion

        #region AnnotationContact
        /// <summary>
        /// AnnotationContact
        /// </summary>
        [DataContract]
        public class AnnotationContact : INullable, IOracleCustomType
        {
            private bool m_bIsNull;

            [DataMember]
            public long AnnotationId { get; set; }

            [DataMember]
            [OracleObjectMapping("CONTACT_NO")]
            public long ContactNo { get; set; }

            [DataMember]
            [OracleObjectMapping("CONTACT_ID")]
            public long ContactId { get; set; }

            [DataMember]
            [OracleObjectMapping("PHONE_NUMBER")]
            public string PhoneNumber { get; set; }

            [DataMember]
            [OracleObjectMapping("ORGANIZATION_ID")]
            public long OrganizationId { get; set; }

            [DataMember]
            [OracleObjectMapping("IS_ADHOC")]
            public short IsAdhoc { get; set; }

            [DataMember]
            [OracleObjectMapping("FULL_NAME")]
            public string FullName { get; set; }

            [DataMember]
            [OracleObjectMapping("ORG_NAME")]
            public string OrgName { get; set; }

            [DataMember]
            [OracleObjectMapping("EMAIL")]
            public string Email { get; set; }

            [DataMember]
            [OracleObjectMapping("EMAIL_PREFERENCE")]
            public long EmailPreference { get; set; }

            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static AnnotationContact Null
            {
                get
                {
                    AnnotationContact p = new AnnotationContact
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "CONTACT_NO", ContactNo);
                OracleUdt.SetValue(con, pUdt, "CONTACT_ID", ContactId);
                OracleUdt.SetValue(con, pUdt, "PHONE_NUMBER", PhoneNumber);
                OracleUdt.SetValue(con, pUdt, "ORGANIZATION_ID", OrganizationId);
                OracleUdt.SetValue(con, pUdt, "IS_ADHOC", IsAdhoc);
                OracleUdt.SetValue(con, pUdt, "FULL_NAME", FullName);
                OracleUdt.SetValue(con, pUdt, "ORG_NAME", OrgName);
                OracleUdt.SetValue(con, pUdt, "EMAIL", Email);
                OracleUdt.SetValue(con, pUdt, "EMAIL_PREFERENCE", EmailPreference);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                ContactNo = (long)OracleUdt.GetValue(con, pUdt, "CONTACT_NO");
                ContactId = (long)OracleUdt.GetValue(con, pUdt, "CONTACT_ID");
                PhoneNumber = (string)OracleUdt.GetValue(con, pUdt, "PHONE_NUMBER");
                OrganizationId = (long)OracleUdt.GetValue(con, pUdt, "ORGANIZATION_ID");
                IsAdhoc = (short)OracleUdt.GetValue(con, pUdt, "IS_ADHOC");
                FullName = (string)OracleUdt.GetValue(con, pUdt, "FULL_NAME");
                OrgName = (string)OracleUdt.GetValue(con, pUdt, "ORG_NAME");
                Email = (string)OracleUdt.GetValue(con, pUdt, "EMAIL");
                EmailPreference = (long)OracleUdt.GetValue(con, pUdt, "EMAIL_PREFERENCE");
            }
        }

        [OracleCustomTypeMapping("PORTAL.ANNOTATIONCONTACT")]
        public class AnnotationContactsFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new AnnotationContact();
            }
        }

        public class AnnotationContactArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public AnnotationContact[] AnnotationContactObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static AnnotationContactArray Null
            {
                get
                {
                    AnnotationContactArray p = new AnnotationContactArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, AnnotationContactObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                AnnotationContactObj = (AnnotationContact[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.ANNOTATIONCONTACTARRAY")]
        public class AnnotationContactArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new AnnotationContactArray();
            }
            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new AnnotationContact[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new AnnotationContact[numElems];
            }
            #endregion
        }

        #endregion

        #region RoutePartMapper
        public class RoutePartMapper
        {
            [DataMember]
            public RoutePart RoutePart { get; set; }
        }
        #endregion

        #region RoutePart
        [DataContract]
        public class RoutePart
        {
            [DataMember]
            public RoutePartDetails RoutePartDetails { get; set; }
            [DataMember]
            public List<RoutePath> RoutePathList { get; set; }
            
            public RoutePart()
            {
                RoutePartDetails = new RoutePartDetails();
                RoutePathList = new List<RoutePath>();
            }
            [DataMember]
            public DateTime LastUpdated { get; set; }
            [DataMember]
            public long OrgId { get; set; }
            [DataMember]
            public long Esdal2Broken { get; set; }
            [DataMember]
            public long LibRtBrok { get; set; }
            [DataMember]
            public long IsAutoReplan { get; set; }
            [DataMember]
            public int UserId { get; set; }//setting user id for modified by user
            [DataMember]
            public sdogeometry GPXGeometry { get; set; }
        }
        #endregion

        #region BrokenRouteList
        [DataContract]
        public class BrokenRouteList
        {
            [DataMember]
            public long PlannedRouteId { get; set; }
            [DataMember]
            public int IsBroken { get; set; }
            [DataMember]
            public int IsReplan { get; set; }

        };

        #endregion

        #region CheckBrokenRoute
        public class CheckIsBrokenParams
        {
            public long RoutePartId { get; set; }
            public long VersionId { get; set; }
            public long RevisonId { get; set; }
            public long LibraryRouteId { get; set; }
            public string ContentRefNum { get; set; }
            public long CandRevisionId { get; set; }
        }
        #endregion

        #region AnnotationTextLibrary
        [DataContract]
        public class AnnotationTextLibrary
        {
            [DataMember]
            public long AnnotationTextId { get; set; }
            [DataMember]
            public long OrganisationId { get; set; }
            [DataMember]
            public int UserId { get; set; }
            [DataMember]
            public long AnnotType { get; set; }
            [DataMember]
            public string AnnotationText { get; set; }
            [DataMember]
            public string UserName { get; set; }

            [DataMember]
            public decimal totalRecords { get; set; }
        }
        #endregion

        public class AnnotationTextLibraryList
        {
            [DataMember]
            public int AnnotType { get; set; }

            [DataMember]
            StaticPagedList<AnnotationTextLibrary> AnnotationTextLibararyPageList { get; set; }
        }

    }
}