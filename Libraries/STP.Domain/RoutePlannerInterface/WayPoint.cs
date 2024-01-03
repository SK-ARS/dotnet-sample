using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace STP.Domain.RoutePlannerInterface
{
    public class WayPoint
    {
        public UInt32 WayPointBeginNode { get; set; } //Waypoint Begin Node
        public UInt32 WayPointLinkId { get; set; } //begin point link Id
        public UInt32 WayPointEndNode { get; set; } //beging point end node

        /// <summary>
        /// Encodes the WayPoint to byte array
        /// </summary>
        public byte[] GetBytes()
        {
            MemoryStream memStream = new MemoryStream();
            byte[] data;
            //Encode WayPointbeginNode
            data = BitConverter.GetBytes(WayPointBeginNode);
            memStream.Write(data, 0, data.Length);
            //Encode Way point Link Id
            data = BitConverter.GetBytes(WayPointLinkId);
            memStream.Write(data, 0, data.Length);
            //Encode WayPointEndNode
            data = BitConverter.GetBytes(WayPointEndNode);
            memStream.Write(data, 0, data.Length);
            return memStream.ToArray();
        }
    }
}