using NetSdoGeometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace STP.Domain.Routes
{
    public class RouteModelJson
    {
        #region RoutePartDetailsJson
        [DataContract]
        public class RoutePartDetailsJson
        {
            [DataMember]
            [JsonProperty("RouteId")]
            public long routeID { get; set; }

            [DataMember]
            [JsonProperty("RoutePartNo")]
            public int routePartNo { get; set; }

            [DataMember]
            [JsonProperty("RouteName")]
            public string routeName { get; set; }

            [DataMember]
            [JsonProperty("RouteDescr")]
            public string routeDescr { get; set; }

            [DataMember]
            [JsonProperty("IsComplete")]
            public int isComplete { get; set; }

            [DataMember]
            [JsonProperty("PartGeometry")]
            public sdogeometry partGeometry { get; set; }

            [DataMember]
            [JsonProperty("SaveAsNew")]
            public bool SaveAsNew { get; set; }

            [DataMember]
            [JsonProperty("DockCaution")]
            public bool DockCaution { get; set; }

            [DataMember]
            [JsonProperty("WayPointAddress")]
            public bool wayPointAddress { get; set; }

            [DataMember]
            [JsonProperty("WayPointContactDetails")]
            public bool wayPointContactDetails { get; set; }

            //For Library route
            [DataMember]
            [JsonProperty("RouteType")]
            public string routeType { get; set; }

            [DataMember]
            [JsonProperty("StartDesc")]
            public string startDesc { get; set; }

            [DataMember]
            [JsonProperty("EndDesc")]
            public string endDesc { get; set; }

            [DataMember]
            public int IsBrokenLib { get; set; }

            //For NEN
            [DataMember]
            [JsonProperty("AnalysisId")]
            public int analysisId { get; set; }

            [DataMember]
            public int TotalRecord { get; set; }
        }
        #endregion

        #region RoutePathJson
        [DataContract]
        public class RoutePathJson
        {
            [DataMember]
            [JsonProperty("RoutePathId")]
            public long routePathId { get; set; }

            [DataMember]
            [JsonProperty("RouteId")]
            public long routeID { get; set; }

            [DataMember]
            [JsonProperty("RoutePathNo")]
            public int routePathNo { get; set; }

            [DataMember]
            [JsonProperty("RoutePathType")]
            public int routePathType { get; set; }

            [DataMember]
            [JsonProperty("RouteVariant")]
            public long routeVariant { get; set; }

            [DataMember]
            [JsonProperty("PathDescr")]
            public string pathDescr { get; set; }

            [DataMember]
            [JsonProperty("RouteSegmentList")]
            public List<RouteSegmentJson> routeSegmentList { get; set; }

            [DataMember]
            [JsonProperty("RoutePointList")]
            public List<RoutePointJson> routePointList { get; set; }


            [DataMember]
            [JsonProperty("AlternatePointList")]
            public List<RoutePointJson> alternatePointList { get; set; }
            public RoutePathJson()
            {
                routeSegmentList = new List<RouteSegmentJson>();
                routePointList = new List<RoutePointJson>();
                alternatePointList = new List<RoutePointJson>();
            }
        }
        #endregion

        #region RoutePointJson
        [DataContract]
        public class RoutePointJson
        {
            [DataMember]
            [JsonProperty("RoutePointId")]
            public long routePointId { get; set; }

            [DataMember]
            [JsonProperty("RoutePointNo")]
            public int routePointNo { get; set; }

            [DataMember]
            [JsonProperty("PointType")]
            public int? pointType { get; set; }

            [DataMember]
            [JsonProperty("PointDescr")]
            public string pointDescr { get; set; }

            [DataMember]
            [JsonProperty("Lrs")]
            public Int32? lrs { get; set; }

            [DataMember]
            [JsonProperty("Direction")]
            public int? direction { get; set; }

            [DataMember]
            [JsonProperty("LinkId")]
            public long linkId { get; set; }

            [DataMember]
            [JsonProperty("PointGeom")]
            public sdogeometry pointGeom { get; set; }

            [DataMember]
            [JsonProperty("RoadGeometry")]
            public sdogeometry roadGeometry { get; set; }

            [DataMember]
            [JsonProperty("WayText")]
            public string wayText { get; set; }
            [DataMember]
            [JsonProperty("IsAnchorPoint")]
            public int isAnchorPoint { get; set; }

            [DataMember]
            [JsonProperty("RoutePathId")]
            public long routePathId { get; set; }

            [DataMember]
            [JsonProperty("PointAnnotation")]
            public string pointAnnotation { get; set; }

            [DataMember]
            [JsonProperty("ShowRoutePoint")]
            public int showRoutePoint { get; set; }

            //For Broken Route replanner
            [DataMember]
            [JsonProperty("NewLinkId")]
            public long newLinkId { get; set; }

            [DataMember]
            [JsonProperty("NewBeginNodeId")]
            public long newBeginNodeId { get; set; }

            [DataMember]
            [JsonProperty("NewEndNodeId")]
            public long newEndNodeId { get; set; }

            [DataMember]
            [JsonProperty("DistanceToNewLink")]
            public decimal distanceToNewLink { get; set; }

            //For NEN Route
            [DataMember]
            public long Northing { get; set; }

            [DataMember]
            public long Easting { get; set; }

            public RoutePointJson()
            {
                showRoutePoint = 1;
            }
        }

        #endregion

        #region RouteSegmentJson
        /// <summary>
        /// RouteSegment
        /// </summary>
        [DataContract]
        public class RouteSegmentJson
        {
            [DataMember]
            [JsonProperty("SegmentId")]
            public long segmentId { get; set; }

            [DataMember]
            [JsonProperty("SegmentNo")]
            public int segmentNo { get; set; }

            [DataMember]
            [JsonProperty("RoutePathId")]
            public long routePathId { get; set; }

            [DataMember]
            [JsonProperty("SegmentDesc")]
            public string segmentDesc { get; set; }

            [DataMember]
            [JsonProperty("OffRoadGeometry")]
            public sdogeometry offRoadGeometry { get; set; }

            [DataMember]
            [JsonProperty("StartLinkId")]
            public long? startLinkId { get; set; }

            [DataMember]
            [JsonProperty("EndLinkId")]
            public long? endLinkId { get; set; }

            [DataMember]
            [JsonProperty("StartLrs")]
            public int? startLrs { get; set; }

            [DataMember]
            [JsonProperty("StartPointGeometry")]
            public sdogeometry startPointGeometry { get; set; }

            [DataMember]
            [JsonProperty("EndLrs")]
            public int? endLrs { get; set; }

            [DataMember]
            [JsonProperty("EndPointGeometry")]
            public sdogeometry endPointGeometry { get; set; }

            [DataMember]
            [JsonProperty("LinkGeometry")]
            public sdogeometry LinkGeometry { get; set; }
            [DataMember]
            [JsonProperty("StartPointDirection")]
            public int? startPointDirection { get; set; }

            [DataMember]
            [JsonProperty("EndPointDirection")]
            public int? endPointDirection { get; set; }

            [DataMember]
            [JsonProperty("SegmentType")]
            public long segmentType { get; set; }

            [DataMember]
            [JsonProperty("RouteLinkList")]
            public List<RouteLinkJson> routeLinkList { get; set; }

            [DataMember]
            [JsonProperty("RouteAnnotationsList")]
            public List<RouteAnnotationJson> routeAnnotationsList { get; set; }

            public RouteSegmentJson()
            {
                routeLinkList = new List<RouteLinkJson>();
                routeAnnotationsList = new List<RouteAnnotationJson>();
            }
        }
        #endregion

        #region RouteLinkJson
        [DataContract]
        public class RouteLinkJson
        {
            [DataMember]
            [JsonProperty("SegmentId")]
            public long segmentId { get; set; }

            [DataMember]
            [JsonProperty("SegmentNo")]
            public int segmentNo { get; set; }

            [DataMember]
            [JsonProperty("LinkNo")]
            public int linkNo { get; set; }    //Modified to included null values coming for routes having off road

            [DataMember]
            [JsonProperty("LinkId")]
            public long? linkId { get; set; } // Should be of type long

            [DataMember]
            [JsonProperty("Direction")]
            public int? direction { get; set; }
        }
        #endregion

        #region RouteAnnotationJson
        /// <summary>
        /// RouteAnnotation
        /// </summary>
        [DataContract]
        public class RouteAnnotationJson
        {
            [DataMember]
            [JsonProperty("AnnotationId")]
            public long annotationID { get; set; }

            [DataMember]
            [JsonProperty("AnnotType")]
            public int annotType { get; set; }

            [DataMember]
            [JsonProperty("AnnotText")]
            public string annotText { get; set; }

            [DataMember]
            [JsonProperty("AssocType")]
            public long assocType { get; set; }

            [DataMember]
            [JsonProperty("Easting")]
            public long easting { get; set; }

            [DataMember]
            [JsonProperty("Northing")]
            public long northing { get; set; }

            [DataMember]
            [JsonProperty("Geometry")]
            public sdogeometry geometry { get; set; }

            [DataMember]
            [JsonProperty("Direction")]
            public short direction { get; set; }

            [DataMember]
            [JsonProperty("StructureEsrn")]
            public string structureEsrn { get; set; }

            [DataMember]
            [JsonProperty("ConstraintEsrn")]
            public string constraintEsrn { get; set; }

            [DataMember]
            [JsonProperty("LinkId")]
            public long linkId { get; set; }

            [DataMember]
            [JsonProperty("LinearRef")]
            public int linearRef { get; set; }

            [DataMember]
            [JsonProperty("IsBroken")]
            public short isBroken { get; set; }

            [DataMember]
            [JsonProperty("InRouteDescription")]
            public short inRouteDescription { get; set; }

            [DataMember]
            [JsonProperty("SegmentId")]
            public long segmentID { get; set; }

            [DataMember]
            [JsonProperty("SegmentNo")]
            public int segmentNo { get; set; }

            [DataMember]
            [JsonProperty("AnnotationContactList")]
            public List<AnnotationContactJson> annotationContactList { get; set; }

            [DataMember]
            [JsonProperty("NewLinkId")]
            public long newLinkId { get; set; }

            [DataMember]
            [JsonProperty("DistanceToNewLink")]
            public decimal distanceToNewLink { get; set; }

            [DataMember]
            [JsonProperty("RoadGeometry")]
            public sdogeometry roadGeometry { get; set; }

            public RouteAnnotationJson()
            {
                geometry = new sdogeometry();
                annotationContactList = new List<AnnotationContactJson>();
            }
        }
        #endregion

        #region AnnotationContactJson
        /// <summary>
        /// AnnotationContact
        /// </summary>
        [DataContract]
        public class AnnotationContactJson
        {
            [DataMember]
            [JsonProperty("AnnotationId")]
            public long annotationID { get; set; }

            [DataMember]
            [JsonProperty("ContactNo")]
            public long contactNo { get; set; }

            [DataMember]
            [JsonProperty("ContactId")]
            public long contactID { get; set; }

            [DataMember]
            [JsonProperty("PhoneNumber")]
            public string phoneNumber { get; set; }

            [DataMember]
            [JsonProperty("OrganizationId")]
            public long organizationID { get; set; }

            [DataMember]
            [JsonProperty("IsAdhoc")]
            public short isAdhoc { get; set; }

            [DataMember]
            [JsonProperty("FullName")]
            public string fullName { get; set; }

            [DataMember]
            [JsonProperty("OrgName")]
            public string orgName { get; set; }

            [DataMember]
            [JsonProperty("Email")]
            public string email { get; set; }

            [DataMember]
            [JsonProperty("EmailPreference")]
            public long emailPreference { get; set; }
        }
        #endregion

        #region RoutePartJson
        /// <summary>
        /// RoutePart
        /// </summary>
        [DataContract]
        public class RoutePartJson
        {
            [DataMember]
            [JsonProperty("RoutePartDetails")]
            public RoutePartDetailsJson routePartDetails { get; set; }

            [DataMember]
            [JsonProperty("RoutePathList")]
            public List<RoutePathJson> routePathList { get; set; }
            public RoutePartJson()
            {
                routePartDetails = new RoutePartDetailsJson();
                routePathList = new List<RoutePathJson>();
            }

            [DataMember]
            [JsonProperty("LastUpdated")]
            public DateTime lastUpdated { get; set; }

            [DataMember]
            [JsonProperty("OrgId")]
            public long orgId { get; set; }

            [DataMember]
            public long Esdal2Broken { get; set; }

            [DataMember]
            public long LibRtBrok { get; set; }

            [DataMember]
            public long IsAutoReplan { get; set; }
        };
        #endregion
    }
}
