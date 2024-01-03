using STP.Domain.RoutePlannerInterface;
using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Messages
{
    public class MessagePlannedRoute : Message
    {
        #region properties
        public Byte Response { get; set; }   
        public UInt16 SegmentCount { get; set; } 
        public List<UInt32> Segments { get; set; } 
        #endregion

        #region constructor
        public MessagePlannedRoute()
        {
            SetMessageType(MessageTypes.MSG_PLANNED_ROUTE, "MSG_PLANNED_ROUTE");
        }
        #endregion

        #region methods
        public override byte[] EncodeData()
        {
            return new byte[0];
        }
        public override bool DecodeData(byte[] data)
        {
            int nDataRead = 0;
            int nLength = data.Length;
            if (nLength < 3)
            {
                return false; 
            }

            Response = data[nDataRead++];
            SegmentCount = BitConverter.ToUInt16(data, nDataRead);
            nDataRead += 2; 
            if (nLength < (3 + 4 * SegmentCount))
            {
                return false;
            }
            Segments = new List<UInt32>();
            for (int nCount = 0; nCount < SegmentCount; ++nCount)
            {
                Segments.Add(BitConverter.ToUInt32(data, nDataRead));
                nDataRead += 4;
            }

            return true;
        }
        #endregion
        internal RouteData GetRouteData()
        {
            RouteData routeData = new RouteData();
            routeData.ListSegments = Segments;
            if (Response == 0)
            {
                routeData.ResponseMessage = "ROOT_SEGMTS";
            }
            else
            {
                routeData.ResponseMessage = "ROUTE_PLAN_FAILED";
            }
            return routeData;
        }
    }
}