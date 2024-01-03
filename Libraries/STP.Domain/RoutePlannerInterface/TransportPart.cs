using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace STP.Domain.RoutePlannerInterface
{
    public class TransportPart
    {
        public UInt32 LinkId { get; set; } //LinkID or segment restricted
        public UInt16 Height { get; set; } //Height in metres
        public UInt32 Weight { get; set; } //Weight in metres
        public UInt16 Length { get; set; } //Length in meters
        public UInt16 Width { get; set; }  //Width in meters
        public UInt32 NormalLoad { get; set; } //Normal Load in Kilograms
        public UInt32 ShuttleLoad { get; set; } //Shuttle Weight in Kg
        /// <summary>
        /// Encodes the Transport part to byte array
        /// </summary>
        public byte[] GetBytes()
        {
            MemoryStream memStream = new MemoryStream();
            byte[] data;
            //Encode LinkID
            data = BitConverter.GetBytes(LinkId);
            memStream.Write(data, 0, data.Length);
            //Encode Height
            data = BitConverter.GetBytes(Height);
            memStream.Write(data, 0, data.Length);
            //Encode Weight
            data = BitConverter.GetBytes(Weight);
            memStream.Write(data, 0, data.Length);
            //Encode Length
            data = BitConverter.GetBytes(Length);
            memStream.Write(data, 0, data.Length);
            //Encode Width
            data = BitConverter.GetBytes(Width);
            memStream.Write(data, 0, data.Length);
            //Encode Normal Load
            data = BitConverter.GetBytes(NormalLoad);
            memStream.Write(data, 0, data.Length);
            //Encode Shuttle Load
            data = BitConverter.GetBytes(ShuttleLoad);
            memStream.Write(data, 0, data.Length);
            return memStream.ToArray();
        }
    }
}