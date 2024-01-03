using System;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Text;
using STP.Common.Logger;
using System.Configuration;

namespace GpxLibrary
{
    //GPX(the GPS eXchange Format) is a data format for exchanging GPS data between programs, and for sharing GPS data with other users. 
    //This GPX class provide converts  GPS data(waypoints, routes, and tracks) to be compatible with GPX format file.
     
    public class ConvertGpx
    {
        private static string LogInstance = ConfigurationManager.AppSettings["Instance"];
        
        public class Route
        {
            public long RouteId { get; set; }
            public string RouteName { get; set; }
            public string RouteType { get; set; }
            public string Description { get; set; }
        }
        public class WayPoints
        {

            /* [Required] - Latitude of the waypoint. */
            public string Lat { get; set; } = "";

            /*[Required] - Longitude of the waypoint.*/
            public string Lon { get; set; } = "";

            /* [Optional Description Information] - GPS waypoint name of the waypoint. */
            public string Name { get; set; } = "";

            /*[Optional Description Information] - Descriptive description of the waypoint.*/
            public string Desc { get; set; } = "";
        }

        public class Tracks
        {

            /*[Required] - List of trackpoints(trackpoints). */
            public TrackPoints[] TrackPoints;

            /* [Required] - GPS trackpoint name. */
            public string Name { get; set; } = "";

            /*[Optional] - Description of the trackpoint.*/
            public string Desc { get; set; } = "";

        }

        public class TrackPoints
        {

            /*[Required] - Latitude of the trackpoint.*/
            public string Lat { get; set; } = "";

            /*[Required] - Longitude of the trackpoint.*/
            public string Lon { get; set; } = "";

            /* [Optional Description Information]</B> GPS trackpoint name */
            public string Name { get; set; } = "";

            /* [Optional Description Information]</B> Descriptive description of the trackpoint. */
            public string Desc { get; set; } = "";

            /* [Optional TrackName Information]</B> Descriptive TrackName of the route. */
            public string TrackName { get; set; } = "";

        }
        public static string Base64Encode(string plainString)
        {
            var plainStringInBytes = Encoding.UTF8.GetBytes(plainString);
            return System.Convert.ToBase64String(plainStringInBytes);
        }

        /* GPS Data convert to GPX format file. */
        public string GPSConvertToGPX(WayPoints[] vWaypoints, Tracks[] vTracks)
        {
            var localTime = DateTime.Now.ToString();
            StringBuilder builder = new StringBuilder();
            try
            {                
                using (StringWriter stringWriter = new StringWriter(builder))
                {
                    using (XmlTextWriter myXmlTextWriter = new XmlTextWriter(stringWriter))
                    {
                        myXmlTextWriter.Formatting = Formatting.Indented;

                        myXmlTextWriter.WriteStartDocument();

                        myXmlTextWriter.WriteStartElement("gpx");

                        myXmlTextWriter.WriteAttributeString("version", "1.0");
                        myXmlTextWriter.WriteAttributeString("creator", "ESDAL4");
                        myXmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                        myXmlTextWriter.WriteAttributeString("xmlns", "http://www.topografix.com/GPX/1/1");
                        myXmlTextWriter.WriteAttributeString("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1/gpx.xsd");

                        myXmlTextWriter.WriteElementString("time", localTime);

                        if (vWaypoints != null)
                        {
                            for (int countWP = 0; countWP < vWaypoints.Length; countWP++)
                            {
                                myXmlTextWriter.WriteStartElement("wpt");

                                WayPoints WP = vWaypoints[countWP];

                                //Required Informations
                                myXmlTextWriter.WriteAttributeString("lat", WP.Lat);
                                myXmlTextWriter.WriteAttributeString("lon", WP.Lon);

                                //Optional Information
                                Type t = WP.GetType();
                                PropertyInfo[] PIS = t.GetProperties();

                                foreach (PropertyInfo pi in PIS)
                                {
                                    if ((!pi.Name.Equals("Lat")) && (!pi.Name.Equals("Lon")) && (pi.GetValue(WP, null).ToString().Length != 0))
                                    {
                                        myXmlTextWriter.WriteElementString(pi.Name.ToLower(), pi.GetValue(WP, null).ToString());
                                    }
                                }

                                myXmlTextWriter.WriteEndElement();
                            }
                        }

                        if (vTracks != null)
                        {
                            for (int countTP = 0; countTP < vTracks.Length; countTP++)
                            {
                                myXmlTextWriter.WriteStartElement("trk");                                
                                //Optional Information
                                Tracks TS = vTracks[countTP];
                                Type t = TS.GetType();
                                PropertyInfo[] PIS = t.GetProperties();

                                foreach (PropertyInfo pi in PIS)
                                {
                                    if (pi.GetValue(TS, null).ToString().Length != 0)
                                    {
                                        myXmlTextWriter.WriteElementString(pi.Name.ToLower(), pi.GetValue(TS, null).ToString());
                                    }
                                }
                                myXmlTextWriter.WriteStartElement("trkseg");
                                //<trkpt>----------------------------------------------					
                                if (TS.TrackPoints != null)
                                {
                                    for (int countTrackpoints = 0; countTrackpoints < TS.TrackPoints.Length; countTrackpoints++)
                                    {
                                        myXmlTextWriter.WriteStartElement("trkpt");

                                        TrackPoints TP = TS.TrackPoints[countTrackpoints];

                                        //Required Informations
                                        myXmlTextWriter.WriteAttributeString("lat", TP.Lat);
                                        myXmlTextWriter.WriteAttributeString("lon", TP.Lon);

                                        //Optional Information
                                        Type tTP = TP.GetType();
                                        PropertyInfo[] PIS_TP = tTP.GetProperties();

                                        foreach (PropertyInfo pi in PIS_TP)
                                        {
                                            if ((!pi.Name.Equals("Lat")) && (!pi.Name.Equals("Lon")) && (!pi.Name.Equals("TrackName")) && (pi.GetValue(TP, null).ToString().Length != 0))
                                            {
                                                myXmlTextWriter.WriteElementString(pi.Name.ToLower(), pi.GetValue(TP, null).ToString());
                                            }
                                        }

                                        myXmlTextWriter.WriteEndElement();
                                    }
                                }
                                //</trkpt>---------------------------------------------

                                myXmlTextWriter.WriteEndElement();
                                myXmlTextWriter.WriteEndElement();

                            }
                        }

                        myXmlTextWriter.WriteEndElement();

                        myXmlTextWriter.WriteEndDocument();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $" - RouteExport/GetRouteDetails, Exception: {e}​​​​​​​");
            }

            return builder.ToString();
        }
    }
}
