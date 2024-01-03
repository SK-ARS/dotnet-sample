using STP.Domain.RoutePlannerInterface;
using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace STP.Routes.Messages
{
    public class MessageRouteReq : Message
    {
        #region properties
        public UInt32 SentTime { get; set; }   //Time in which the message was sent
        public UInt32 BeginPoint { get; set; } //From junction or Node ID
        public UInt32 EndPoint { get; set; }   //End junction or Node Id
        public List<TransportPart> TransportParts { get; set; }
        public List<UInt32> WayPoints { get; set; }
        #endregion

        #region constructor
        public MessageRouteReq()
        {
            SetMessageType(MessageTypes.MSG_ROUTE_REQUEST, "MSG_ROUTE_REQUEST");
        }

        public MessageRouteReq(RouteViaWaypoint routeViaPoint)
        {
            this.SentTime = (uint)Utilities.GetCurrentTime();
            this.BeginPoint = Convert.ToUInt32(routeViaPoint.StartPoint);
            this.EndPoint = Convert.ToUInt32(routeViaPoint.EndPoint);
            TransportParts = new List<TransportPart>();
            WayPoints = new List<uint>();
            if (routeViaPoint.WayPoints != null)
            {
                foreach (string strWayPoint in routeViaPoint.WayPoints)
                {
                    WayPoints.Add(Convert.ToUInt32(strWayPoint));
                }
            }
            SetMessageType(MessageTypes.MSG_ROUTE_REQUEST, "MSG_ROUTE_REQUEST");

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
            //Encode Begin Point
            data = BitConverter.GetBytes(BeginPoint);
            memStream.Write(data, 0, data.Length);
            //Encode End Point
            data = BitConverter.GetBytes(EndPoint);
            memStream.Write(data, 0, data.Length);
            //Encode Transport Parts 
            Byte nTransportPart = (Byte)TransportParts.Count;  //Encode number of transport parts
            memStream.WriteByte(nTransportPart);
            //Write each transport part objects to stream
            foreach (TransportPart transPart in TransportParts)
            {
                //Encode the transport parts
                data = transPart.GetBytes();
                memStream.Write(data, 0, data.Length);
            }
            //Encode Way Points
            Byte nWayPoints = (Byte)WayPoints.Count;          //Encode number of way points
            memStream.WriteByte(nWayPoints);
            foreach (int nWayPoint in WayPoints.OfType<int>())
            {
                //Encode the way points
                data = BitConverter.GetBytes(nWayPoint);
                memStream.Write(data, 0, data.Length);
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