using NetSdoGeometry;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace STP.Routes.Models
{
    public class RouteModel
    {
        #region RoutePartDetails
        /// <summary>
        /// PlannedRouteDetails Class
        /// </summary>
        [DataContract]
        public class RoutePartDetails
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
            [DataMember]
            public bool SaveAsNew { get; set; }
            [DataMember]
            public bool DockCaution { get; set; }
            [DataMember]
            public bool wayPointAddress { get; set; }
            [DataMember]
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

            #region Deserializing partGeometry object
            private string _partGeom;
            [DataMember]
            public string partGeom
            {
                set
                {
                    _partGeom = value;
                    _partGeometry = JsonConvert.DeserializeObject<sdogeometry>(_partGeom);
                }
                get { return _partGeom; }
            }
            private sdogeometry _partGeometry;
            [DataMember]
            public sdogeometry partGeometry
            {
                set { _partGeometry = value; }
                get { return _partGeometry; }
            }
            #endregion
        };
        #endregion

        #region RoutePath
        [DataContract]
        public class RoutePath
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
            public List<RouteSegment> routeSegmentList { get; set; }
            [DataMember]
            public List<RoutePoint> routePointList { get; set; }
            public RoutePath()
            {
                routeSegmentList = new List<RouteSegment>();
                routePointList = new List<RoutePoint>();
            }
        };
        #endregion

        #region RoutePoint
        /// <summary>
        /// Planned Route Point class
        /// </summary>
        [DataContract]
        public class RoutePoint
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
            public RoutePoint()
            {
                showRoutePoint = 1;
            }
            //For NEN Route
            [DataMember]
            public long Northing { get; set; }
            [DataMember]
            public long Easting { get; set; }

            #region Deserializing pointGeom object
            private string _routePointGeom;
            [DataMember]
            public string routePointGeom
            {
                set
                {
                    _routePointGeom = value;
                    _pointGeom = JsonConvert.DeserializeObject<sdogeometry>(routePointGeom);
                }
                get { return _routePointGeom; }
            }
            private sdogeometry _pointGeom;
            [DataMember]
            public sdogeometry pointGeom
            {
                set
                { _pointGeom = value; }
                get { return _pointGeom; }
            }
            #endregion

            #region Deserializing roadGeometry object
            private string _roadGeom;
            [DataMember]
            public string roadGeom
            {
                set
                {
                    _roadGeom = value;
                    _roadGeometry = JsonConvert.DeserializeObject<sdogeometry>(roadGeom);
                }
                get { return _roadGeom; }
            }
            private sdogeometry _roadGeometry;
            [DataMember]
            public sdogeometry roadGeometry
            {
                set { _roadGeometry = value; }
                get { return _roadGeometry; }
            }
            #endregion
        };

        #endregion

        #region RouteSegment
        /// <summary>
        /// RouteSegment
        /// </summary>
        [DataContract]
        public class RouteSegment
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
            public List<RouteAnnotation> routeAnnotationsList { get; set; }
            public RouteSegment()
            {
                routeLinkList = new List<RouteLink>();
                routeAnnotationsList = new List<RouteAnnotation>();
            }

            #region Deserializing startPointGeometry object
            private string _startGeom;
            [DataMember]
            public string startGeom
            {
                set
                {
                    _startGeom = value;
                    _startPointGeometry = JsonConvert.DeserializeObject<sdogeometry>(_startGeom);
                }
                get { return _startGeom; }
            }
            private sdogeometry _startPointGeometry;
            [DataMember]
            public sdogeometry startPointGeometry
            {
                set { _startPointGeometry = value; }
                get { return _startPointGeometry; }
            }
            #endregion

            #region Deserializing endPointGeometry object
            private string _endGeom;
            [DataMember]
            public string endGeom
            {
                set
                {
                    _endGeom = value;
                    _endPointGeometry = JsonConvert.DeserializeObject<sdogeometry>(_endGeom);
                }
                get { return _endGeom; }
            }
            private sdogeometry _endPointGeometry;
            [DataMember]
            public sdogeometry endPointGeometry
            {
                set { _endPointGeometry = value; }
                get { return _endPointGeometry; }
            }
            #endregion

            #region Deserializing offRoadGeometry object
            private string _offGeom;
            [DataMember]
            public string offGeom
            {
                set
                {
                    _offGeom = value;
                    _offRoadGeometry = JsonConvert.DeserializeObject<sdogeometry>(_offGeom);
                }
                get { return _offGeom; }
            }
            private sdogeometry _offRoadGeometry;
            [DataMember]
            public sdogeometry offRoadGeometry
            {
                set { _offRoadGeometry = value; }
                get { return _offRoadGeometry; }
            }
            #endregion
        };
        #endregion

        #region RouteLink
        [DataContract]
        public class RouteLink : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMappingAttribute("SEGMENT_ID")]
            public long segmentId { get; set; }
            [DataMember]
            [OracleObjectMappingAttribute("SEGMENT_NO")]
            public int segmentNo { get; set; }
            [DataMember]
            [OracleObjectMappingAttribute("LINK_NO")]
            public int linkNo { get; set; }    //Modified to included null values coming for routes having off road
            [DataMember]
            [OracleObjectMappingAttribute("LINK_ID")]
            public long? linkId { get; set; } // Should be of type long
            [DataMember]
            [OracleObjectMappingAttribute("DIRECTION")]
            public int? direction { get; set; }
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
                    RouteLink p = new RouteLink();
                    p.m_bIsNull = true;
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "SEGMENT_ID", segmentId);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_NO", segmentNo);
                OracleUdt.SetValue(con, pUdt, "LINK_NO", linkNo);
                OracleUdt.SetValue(con, pUdt, "LINK_ID", linkId);
                OracleUdt.SetValue(con, pUdt, "DIRECTION", direction);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                segmentId = (long)OracleUdt.GetValue(con, pUdt, "SEGMENT_ID");
                segmentNo = (int)OracleUdt.GetValue(con, pUdt, "SEGMENT_NO");
                linkNo = (int)OracleUdt.GetValue(con, pUdt, "LINK_NO");
                linkId = (long?)OracleUdt.GetValue(con, pUdt, "LINK_ID");
                direction = (int?)OracleUdt.GetValue(con, pUdt, "DIRECTION");
            }
        }
        [OracleCustomTypeMappingAttribute("PORTAL.ROUTELINK")]
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
            public RouteLink[] RouteLinkObj
            { get; set; }

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
                    RouteLinkArray p = new RouteLinkArray();
                    p.m_bIsNull = true;
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

        [OracleCustomTypeMapping("PORTAL.ROUTELINKARRAY")]
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

        #region RoutePart
        [DataContract]
        public class RoutePart
        {
            [DataMember]
            public RoutePartDetails routePartDetails { get; set; }
            [DataMember]
            public List<RoutePath> routePathList { get; set; }
            public RoutePart()
            {
                routePartDetails = new RoutePartDetails();
                routePathList = new List<RoutePath>();
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

        #region RouteAnnotation
        /// <summary>
        /// RouteAnnotation
        /// </summary>
        [DataContract]
        public class RouteAnnotation : INullable, IOracleCustomType
        {
            private bool m_bIsNull;

            [DataMember]
            public long annotationID { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("ANNOT_TYPE")]
            public int annotType { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("ANNOT_TEXT")]
            public string annotText { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("ASSOC_TYPE")]
            public long assocType { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("EASTING")]
            public long easting { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("NORTHING")]
            public long northing { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("DIRECTION")]
            public short direction { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("STRUCTURE_ESRN")]
            public string structureEsrn { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("CONSTRAINT_ESRN")]
            public string constraintEsrn { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("LINK_ID")]
            public long linkId { get; set; }
            [DataMember]
            [OracleObjectMappingAttribute("LINEAR_REF")]
            public int linearRef { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("IS_BROKEN")]
            public short isBroken { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("IN_ROUTE_DESCRIPTION")]
            public short inRouteDescription { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("SEGMENT_ID")]
            public long segmentID { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("SEGMENT_NO")]
            public int segmentNo { get; set; }

            [DataMember]
            public List<AnnotationContact> annotationContactList { set; get; }

            [DataMember]
            [OracleObjectMappingAttribute("ANNOTATION_CONTACT_ARRAY")]
            public AnnotationContactArray annotationContactArray { set; get; }

            [DataMember]
            public long newLinkId { get; set; }

            [DataMember]
            public decimal distanceToNewLink { get; set; }            

            #region Deserializing geometry object
            private string _geom;
            [DataMember]
            public string geom
            {
                set
                {
                    _geom = value;
                    _geometry = JsonConvert.DeserializeObject<sdogeometry>(_geom);
                }
                get { return _geom; }
            }
            private sdogeometry _geometry;
            [DataMember]
            [OracleObjectMappingAttribute("GEOMETRY")]
            public sdogeometry geometry
            {
                set { _geometry = value; }
                get { return _geometry; }
            }
            #endregion

            #region Deserializing roadGeometry object
            private string _roadGeom;
            [DataMember]
            public string roadGeom
            {
                set
                {
                    _roadGeom = value;
                    _roadGeometry = JsonConvert.DeserializeObject<sdogeometry>(_roadGeom);
                }
                get { return _roadGeom; }
            }
            private sdogeometry _roadGeometry;
            [DataMember]
            public sdogeometry roadGeometry
            {
                set { _roadGeometry = value; }
                get { return _roadGeometry; }
            }
            #endregion            

            public RouteAnnotation()
            {
                geometry = new sdogeometry();
                roadGeometry = new sdogeometry();
                annotationContactList = new List<AnnotationContact>();
                annotationContactArray = new AnnotationContactArray();
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
                    RouteAnnotation p = new RouteAnnotation();
                    p.m_bIsNull = true;
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "ANNOT_TYPE", annotType);
                OracleUdt.SetValue(con, pUdt, "ANNOT_TEXT", annotText);
                OracleUdt.SetValue(con, pUdt, "ASSOC_TYPE", assocType);
                OracleUdt.SetValue(con, pUdt, "EASTING", easting);
                OracleUdt.SetValue(con, pUdt, "NORTHING", northing);
                OracleUdt.SetValue(con, pUdt, "GEOMETRY", geometry);
                OracleUdt.SetValue(con, pUdt, "DIRECTION", direction);
                OracleUdt.SetValue(con, pUdt, "STRUCTURE_ESRN", structureEsrn);
                OracleUdt.SetValue(con, pUdt, "CONSTRAINT_ESRN", constraintEsrn);
                OracleUdt.SetValue(con, pUdt, "LINK_ID", linkId);
                OracleUdt.SetValue(con, pUdt, "LINEAR_REF", linearRef);
                OracleUdt.SetValue(con, pUdt, "IS_BROKEN", isBroken);
                OracleUdt.SetValue(con, pUdt, "IN_ROUTE_DESCRIPTION", inRouteDescription);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_ID", segmentID);
                OracleUdt.SetValue(con, pUdt, "SEGMENT_NO", segmentNo);
                OracleUdt.SetValue(con, pUdt, "ANNOTATION_CONTACT_ARRAY", annotationContactArray);
            }
            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                annotType = (int)OracleUdt.GetValue(con, pUdt, "ANNOT_TYPE");
                annotText = (string)OracleUdt.GetValue(con, pUdt, "ANNOT_TEXT");
                assocType = (long)OracleUdt.GetValue(con, pUdt, "ASSOC_TYPE");
                easting = (long)OracleUdt.GetValue(con, pUdt, "EASTING");
                northing = (long)OracleUdt.GetValue(con, pUdt, "NORTHING");
                geometry = (sdogeometry)OracleUdt.GetValue(con, pUdt, "GEOMETRY");
                direction = (short)OracleUdt.GetValue(con, pUdt, "DIRECTION");
                structureEsrn = (string)OracleUdt.GetValue(con, pUdt, "STRUCTURE_ESRN");
                constraintEsrn = (string)OracleUdt.GetValue(con, pUdt, "CONSTRAINT_ESRN");
                linkId = (long)OracleUdt.GetValue(con, pUdt, "LINK_ID");
                linearRef = (int)OracleUdt.GetValue(con, pUdt, "LINEAR_REF");
                isBroken = (short)OracleUdt.GetValue(con, pUdt, "IS_BROKEN");
                inRouteDescription = (short)OracleUdt.GetValue(con, pUdt, "IN_ROUTE_DESCRIPTION");
                segmentID = (long)OracleUdt.GetValue(con, pUdt, "SEGMENT_ID");
                segmentNo = (int)OracleUdt.GetValue(con, pUdt, "SEGMENT_NO");
                annotationContactArray = (AnnotationContactArray)OracleUdt.GetValue(con, pUdt, "ANNOTATION_CONTACT_ARRAY");
            }
        }

        [OracleCustomTypeMappingAttribute("PORTAL.ROUTEANNOTATION")]
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
            public RouteAnnotation[] routeAnnotationObj { get; set; }

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
                    RouteAnnotationArray p = new RouteAnnotationArray();
                    p.m_bIsNull = true;
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, routeAnnotationObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                routeAnnotationObj = (RouteAnnotation[])OracleUdt.GetValue(con, pUdt, 0);
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
            public long annotationID { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("CONTACT_NO")]
            public long contactNo { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("CONTACT_ID")]
            public long contactID { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("PHONE_NUMBER")]
            public string phoneNumber { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("ORGANIZATION_ID")]
            public long organizationID { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("IS_ADHOC")]
            public short isAdhoc { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("FULL_NAME")]
            public string fullName { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("ORG_NAME")]
            public string orgName { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("EMAIL")]
            public string email { get; set; }

            [DataMember]
            [OracleObjectMappingAttribute("EMAIL_PREFERENCE")]
            public long emailPreference { get; set; }

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
                    AnnotationContact p = new AnnotationContact();
                    p.m_bIsNull = true;
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "CONTACT_NO", contactNo);
                OracleUdt.SetValue(con, pUdt, "CONTACT_ID", contactID);
                OracleUdt.SetValue(con, pUdt, "PHONE_NUMBER", phoneNumber);
                OracleUdt.SetValue(con, pUdt, "ORGANIZATION_ID", organizationID);
                OracleUdt.SetValue(con, pUdt, "IS_ADHOC", isAdhoc);
                OracleUdt.SetValue(con, pUdt, "FULL_NAME", fullName);
                OracleUdt.SetValue(con, pUdt, "ORG_NAME", orgName);
                OracleUdt.SetValue(con, pUdt, "EMAIL", email);
                OracleUdt.SetValue(con, pUdt, "EMAIL_PREFERENCE", emailPreference);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                contactNo = (long)OracleUdt.GetValue(con, pUdt, "CONTACT_NO");
                contactID = (long)OracleUdt.GetValue(con, pUdt, "CONTACT_ID");
                phoneNumber = (string)OracleUdt.GetValue(con, pUdt, "PHONE_NUMBER");
                organizationID = (long)OracleUdt.GetValue(con, pUdt, "ORGANIZATION_ID");
                isAdhoc = (short)OracleUdt.GetValue(con, pUdt, "IS_ADHOC");
                fullName = (string)OracleUdt.GetValue(con, pUdt, "FULL_NAME");
                orgName = (string)OracleUdt.GetValue(con, pUdt, "ORG_NAME");
                email = (string)OracleUdt.GetValue(con, pUdt, "EMAIL");
                emailPreference = (long)OracleUdt.GetValue(con, pUdt, "EMAIL_PREFERENCE");
            }
        }

        [OracleCustomTypeMappingAttribute("PORTAL.ANNOTATIONCONTACT")]
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
            public AnnotationContact[] annotationContactObj { get; set; }

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
                    AnnotationContactArray p = new AnnotationContactArray();
                    p.m_bIsNull = true;
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, annotationContactObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                annotationContactObj = (AnnotationContact[])OracleUdt.GetValue(con, pUdt, 0);
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
    }
}