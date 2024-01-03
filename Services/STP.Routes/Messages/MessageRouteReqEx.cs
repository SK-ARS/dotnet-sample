using STP.Domain.RoutePlannerInterface;
using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace STP.Routes.Messages
{
    public class MessageRouteReqEx : Message
    {
        #region properties
        public UInt32 SentTime { get; set; }   //Time in which the message was sent
        public UInt32 BeginStartNode { get; set; } //Begin Point Start Node
        public UInt32 BeginPointLinkId { get; set; } //begin point link Id
        public UInt32 BeginPointEndNode { get; set; } //beging point end node
        public UInt32 EndPointStartNode { get; set; }   //End point Start Node
        public UInt32 EndPointLinkId { get; set; } //End POint LinkId
        public UInt32 EndPointEndNode { get; set; } //End POint end Node

        public List<TransportPart> TransportParts { get; set; }
        public List<WayPoint> WayPoints { get; set; }
        #endregion

        #region constructor
        public MessageRouteReqEx()
        {
            SetMessageType(MessageTypes.MSG_ROUTE_REQUEST_EX, "MSG_ROUTE_REQUEST_EX");
        }

        public MessageRouteReqEx(RouteViaWaypointEx routeViaPoint_ex)
        {
            this.SentTime = (uint)Utilities.GetCurrentTime();
            if (routeViaPoint_ex != null)
            {
                this.BeginStartNode = Convert.ToUInt32(routeViaPoint_ex.BeginStartNode);
                this.BeginPointLinkId = Convert.ToUInt32(routeViaPoint_ex.BeginPointLinkId);
                this.BeginPointEndNode = Convert.ToUInt32(routeViaPoint_ex.BeginPointEndNode);
                this.EndPointStartNode = Convert.ToUInt32(routeViaPoint_ex.EndPointStartNode);
                this.EndPointLinkId = Convert.ToUInt32(routeViaPoint_ex.EndPointLinkId);
                this.EndPointEndNode = Convert.ToUInt32(routeViaPoint_ex.EndPointEndNode);
                TransportParts = new List<TransportPart>();
                WayPoints = new List<WayPoint>();
                if (routeViaPoint_ex.WayPoints != null)
                {
                    foreach (WayPoint wayPoint in routeViaPoint_ex.WayPoints)
                    {
                        WayPoints.Add(wayPoint);
                    }
                }

                SetMessageType(MessageTypes.MSG_ROUTE_REQUEST_EX, "MSG_ROUTE_REQUEST_EX");
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Abstract Method EncodeDate  overridden from Message
        /// </summary>
        /// <returns></returns>
        public override byte[] EncodeData()
        {
            MemoryStream memStream = new MemoryStream();
            //Encode Sent Time
            byte[] data = BitConverter.GetBytes(SentTime);
            memStream.Write(data, 0, data.Length);
            //Encode Begin Point StartNode
            data = BitConverter.GetBytes(BeginStartNode);
            memStream.Write(data, 0, data.Length);
            //Encode Begin Point Link ID
            data = BitConverter.GetBytes(BeginPointLinkId);
            memStream.Write(data, 0, data.Length);
            //Encode Begin Point End Node
            data = BitConverter.GetBytes(BeginPointEndNode);
            memStream.Write(data, 0, data.Length);
            //Encode End point start node
            data = BitConverter.GetBytes(EndPointStartNode);
            memStream.Write(data, 0, data.Length);
            //Encode End Point Link Id
            data = BitConverter.GetBytes(EndPointLinkId);
            memStream.Write(data, 0, data.Length);
            //Encode End point End node
            data = BitConverter.GetBytes(EndPointEndNode);
            memStream.Write(data, 0, data.Length);
            //Encode Transport Parts 
            Byte nTransportPart;
            if (TransportParts != null)
            {
                nTransportPart = (Byte)TransportParts.Count;  //Encode number of transport parts
                memStream.WriteByte(nTransportPart);
                //Write each transport part objects to stream
                foreach (TransportPart transPart in TransportParts)
                {
                    //Encode the transport parts
                    data = transPart.GetBytes();
                    memStream.Write(data, 0, data.Length);
                }
            }
            //Encode Way Points
            Byte nWayPoints;
            if (WayPoints != null)
            {
                nWayPoints = (Byte)WayPoints.Count;
                //Encode number of way points
                memStream.WriteByte(nWayPoints);
                foreach (WayPoint waypoint in WayPoints)
                {
                    //Encode the way points
                    data = waypoint.GetBytes();
                    memStream.Write(data, 0, data.Length);
                }
            }
            return memStream.ToArray();
        }

        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool DecodeData(byte[] data)
        {
            //Decoding not expected not so not implementing
            return false;
        }
        #endregion
    }
}