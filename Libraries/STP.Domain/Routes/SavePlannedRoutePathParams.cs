using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using NetSdoGeometry;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class SavePlannedRoutePathParams
    {
        public RoutePart routePart { get; set; }
        public string userSchema { get; set; }
    }
    #region Serialized RoutePart
    /// <summary>
    /// RoutePart
    /// </summary>
    [DataContract]
    public class RoutePartSerialized
    {
        [DataMember]
        public RoutePartDetailsSerialized routePartDetails { get; set; }
        [DataMember]
        public List<RoutePathSerialized> routePathList { get; set; }
        public RoutePartSerialized()
        {
            routePartDetails = new RoutePartDetailsSerialized();
            routePathList = new List<RoutePathSerialized>();
        }
        [DataMember]
        public DateTime lastUpdated { get; set; }
        [DataMember]
        public long orgId { get; set; }
        [DataMember]
        public long Esdal2Broken { get; set; }
        [DataMember]
        public long LibRtBrok { get; set; }
        [DataMember]
        public long IsAutoReplan { get; set; }
    };
    #endregion
    #region Serialized RoutePartDetails
    /// <summary>
    /// PlannedRouteDetails Class
    /// </summary>
    [DataContract]
    public class RoutePartDetailsSerialized
    {
        [DataMember]
        public long routeID { get; set; }
        [DataMember]
        public int routePartNo { get; set; }
        [DataMember]
        public string routeName { get; set; }
        [DataMember]
        public string routeDescr { get; set; }
        [DataMember]
        public int isComplete { get; set; }
        public bool SaveAsNew { get; set; }
        public bool DockCaution { get; set; }
        public bool wayPointAddress { get; set; }
        public bool wayPointContactDetails { get; set; }
        //For Library route
        [DataMember]
        public string routeType { get; set; }
        [DataMember]
        public string startDesc { get; set; }
        [DataMember]
        public string endDesc { get; set; }
        [DataMember]
        public int IsBrokenLib { get; set; }
        //For NEN
        [DataMember]
        public int analysisId { get; set; }
        [DataMember]
        public int TotalRecord { get; set; }

        private sdogeometry _partGeometry;
        [DataMember]
        public sdogeometry partGeometry
        {
            set
            {
                if (value != null)
                {
                    _partGeometry = value;
                    _partGeom = JsonConvert.SerializeObject(_partGeometry);
                    _partGeometry = null;
                }
            }
            get { return _partGeometry; }
        }
        private string _partGeom;
        [DataMember]
        public string partGeom
        {
            get { return _partGeom; }
        }
    };
    #endregion

    #region Serialized RoutePath
    [DataContract]
    public class RoutePathSerialized
    {
        [DataMember]
        public long routePathId { get; set; }
        [DataMember]
        public long routeID { get; set; }
        [DataMember]
        public int routePathNo { get; set; }
        [DataMember]
        public int routePathType { get; set; }
        [DataMember]
        public long routeVariant { get; set; }
        [DataMember]
        public string pathDescr { get; set; }
        [DataMember]
        public List<RouteSegmentSerialized> routeSegmentList { get; set; }
        [DataMember]
        public List<RoutePointSerialized> routePointList { get; set; }
        public RoutePathSerialized()
        {
            routeSegmentList = new List<RouteSegmentSerialized>();
            routePointList = new List<RoutePointSerialized>();
        }
    };
    #endregion

    #region Serialized RoutePoint
    /// <summary>
    /// Planned Route Point class
    /// </summary>
    [DataContract]
    public class RoutePointSerialized
    {
        [DataMember]
        public long routePointId { get; set; }
        [DataMember]
        public int routePointNo { get; set; }
        [DataMember]
        public int pointType { get; set; }
        [DataMember]
        public string pointDescr { get; set; }
        [DataMember]
        public Int32 lrs { get; set; }
        [DataMember]
        public int? direction { get; set; }
        [DataMember]
        public long linkId { get; set; }
        [DataMember]
        public string wayText { get; set; }
        [DataMember]
        public int isAnchorPoint { get; set; }
        [DataMember]
        public long routePathId { get; set; }
        [DataMember]
        public string pointAnnotation { get; set; }
        [DataMember]
        public int showRoutePoint { get; set; }
        //For Broken Route replanner
        [DataMember]
        public long newLinkId { get; set; }
        [DataMember]
        public long newBeginNodeId { get; set; }
        [DataMember]
        public long newEndNodeId { get; set; }
        [DataMember]
        public decimal distanceToNewLink { get; set; }
        public RoutePointSerialized()
        {
            showRoutePoint = 1;
        }
        //For NEN Route
        public long Northing { get; set; }
        public long Easting { get; set; }

        #region Serializing pointGeom object
        private sdogeometry _pointGeom;
        [DataMember]
        public sdogeometry pointGeom
        {
            set
            {
                if (value != null)
                {
                    _pointGeom = value;
                    _routePointGeom = JsonConvert.SerializeObject(_pointGeom);
                    _pointGeom = null;
                }
            }
            get { return _pointGeom; }
        }

        private string _routePointGeom;
        [DataMember]
        public string routePointGeom
        {
            get { return _routePointGeom; }
        }
        #endregion

        #region Serializing roadGeometry object
        private sdogeometry _roadGeometry;
        [DataMember]
        public sdogeometry roadGeometry
        {
            set
            {
                if (value != null)
                {
                    _roadGeometry = value;
                    _roadGeom = JsonConvert.SerializeObject(_roadGeometry);
                    _roadGeometry = null;
                }
            }
            get { return _roadGeometry; }
        }
        private string _roadGeom;
        [DataMember]
        public string roadGeom
        {
            get { return _roadGeom; }
        }
        #endregion
    };

    #endregion

    #region Serialized RouteSegment
    /// <summary>
    /// RouteSegment
    /// </summary>
    [DataContract]
    public class RouteSegmentSerialized
    {
        [DataMember]
        public long segmentId { get; set; }
        [DataMember]
        public int segmentNo { get; set; }
        [DataMember]
        public long routePathId { get; set; }
        [DataMember]
        public string segmentDesc { get; set; }
        [DataMember]
        public long? startLinkId { get; set; }
        [DataMember]
        public long? endLinkId { get; set; }
        [DataMember]
        public Int32 startLrs { get; set; }
        [DataMember]
        public Int32 endLrs { get; set; }
        [DataMember]
        public int? startPointDirection { get; set; }
        [DataMember]
        public int? endPointDirection { get; set; }
        [DataMember]
        public long segmentType { get; set; }
        [DataMember]
        public List<RouteLink> routeLinkList { get; set; }
        [DataMember]
        public List<RouteAnnotationSerialized> routeAnnotationsList { get; set; }
        public RouteSegmentSerialized()
        {
            routeLinkList = new List<RouteLink>();
            routeAnnotationsList = new List<RouteAnnotationSerialized>();
        }

        #region Serializing startPointGeometry object
        private sdogeometry _startPointGeometry;
        [DataMember]
        public sdogeometry startPointGeometry
        {
            set
            {
                if (value != null)
                {
                    _startPointGeometry = value;
                    _startGeom = JsonConvert.SerializeObject(_startPointGeometry);
                    _startPointGeometry = null;
                }
            }
            get { return _startPointGeometry; }
        }
        private string _startGeom;
        [DataMember]
        public string startGeom
        {
            get { return _startGeom; }
        }
        #endregion

        #region Serializing endPointGeometry object
        private sdogeometry _endPointGeometry;
        [DataMember]
        public sdogeometry endPointGeometry
        {
            set
            {
                if (value != null)
                {
                    _endPointGeometry = value;
                    _endGeom = JsonConvert.SerializeObject(_endPointGeometry);
                    _endPointGeometry = null;
                }
            }
            get { return _endPointGeometry; }
        }
        private string _endGeom;
        [DataMember]
        public string endGeom
        {
            get { return _endGeom; }
        }
        #endregion

        #region Serializing offRoadGeometry object
        private sdogeometry _offRoadGeometry;
        [DataMember]
        public sdogeometry offRoadGeometry
        {
            set
            {
                if (value != null)
                {
                    _offRoadGeometry = value;
                    _offGeom = JsonConvert.SerializeObject(_offRoadGeometry);
                    _offRoadGeometry = null;
                }
            }
            get { return _offRoadGeometry; }
        }
        private string _offGeom;
        [DataMember]
        public string offGeom
        {
            get { return _offGeom; }
        }
        #endregion
    };
    #endregion

    #region Serialized RouteAnnotation
    /// <summary>
    /// RouteAnnotation
    /// </summary>
    [DataContract]
    public class RouteAnnotationSerialized
    {
        [DataMember]
        public long annotationID { get; set; }
        [DataMember]
        public int annotType { get; set; }
        [DataMember]
        public string annotText { get; set; }
        [DataMember]
        public long assocType { get; set; }
        [DataMember]
        public long easting { get; set; }
        [DataMember]
        public long northing { get; set; }
        [DataMember]
        public short direction { get; set; }
        [DataMember]
        public string structureEsrn { get; set; }
        [DataMember]
        public string constraintEsrn { get; set; }
        [DataMember]
        public long linkId { get; set; }
        [DataMember]
        public int linearRef { get; set; }
        [DataMember]
        public short isBroken { get; set; }
        [DataMember]
        public short inRouteDescription { get; set; }
        [DataMember]
        public long segmentID { get; set; }
        [DataMember]
        public int segmentNo { get; set; }
        [DataMember]
        public List<AnnotationContact> annotationContactList { get; set; }
        [DataMember]
        public long newLinkId { get; set; }
        [DataMember]
        public decimal distanceToNewLink { get; set; }
        public RouteAnnotationSerialized()
        {
            geometry = new sdogeometry();
            annotationContactList = new List<AnnotationContact>();
        }

        #region Serializing geometry object
        private sdogeometry _geometry;
        [DataMember]
        public sdogeometry geometry
        {
            set
            {
                if (value != null)
                {
                    _geometry = value;
                    _geom = JsonConvert.SerializeObject(_geometry);
                    _geometry = null;
                }
            }
            get { return _geometry; }
        }

        private string _geom;
        [DataMember]
        public string geom
        {
            get { return _geom; }
        }
        #endregion

        #region Serializing roadGeometry object
        private sdogeometry _roadGeometry;
        [DataMember]
        public sdogeometry roadGeometry
        {
            set
            {
                if (value != null)
                {
                    _roadGeometry = value;
                    _roadGeom = JsonConvert.SerializeObject(_roadGeometry);
                    _roadGeometry = null;
                }
            }
            get { return _roadGeometry; }
        }

        private string _roadGeom;
        [DataMember]
        public string roadGeom
        {
            get { return _roadGeom; }
        }
        #endregion        
    }
    #endregion
}