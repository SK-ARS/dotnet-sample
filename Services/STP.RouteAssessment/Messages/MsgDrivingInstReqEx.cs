using STP.Domain.DrivingInstructionsInterface;
using STP.RouteAssessment.Socket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace STP.RouteAssessment.Messages
{
    public class MsgDrivingInstReqEx : Message
    {
        #region properties
        public byte Schema { get; set; }               //DB Schema
        public byte NumRouteParts { get; set; }           //Number of Route Parts in this message
        public List<UInt64> LstRouteParts { get; set; }   //List of route Parts
        public UInt64 AnalysisID { get; set; }            //Analysis ID
        #endregion

        #region constructor
        public MsgDrivingInstReqEx()
        {
            SetMessageType(MessageTypes.MSG_ROUTE_INSTRUCTION_REQ_EX, "MSG_ROUTE_INSTRUCTION_REQ_EX");
        }
        /// <summary>
        /// Constructor with DrivingInstruction Request Data
        /// </summary>
        /// <param name="DrivingInsReq"></param>
        public MsgDrivingInstReqEx(DrivingInsReq DrivingInsReq, DBSchema schema)
        {
            SetMessageType(MessageTypes.MSG_ROUTE_INSTRUCTION_REQ_EX, "MSG_ROUTE_INSTRUCTION_REQ_EX");
            Schema = (byte)schema;
            NumRouteParts = (byte)DrivingInsReq.ListRouteParts.Count;
            LstRouteParts = DrivingInsReq.ListRouteParts;
            AnalysisID = DrivingInsReq.AnalysisID;
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
            byte[] data;
            memStream.WriteByte(Schema);
            memStream.WriteByte(NumRouteParts); //Write number of route parts
            foreach (UInt64 routePartId in LstRouteParts)
            {
                //Encode the transport parts
                data = BitConverter.GetBytes(routePartId);
                memStream.Write(data, 0, data.Length);
            }

            //Encode Begin Point
            data = BitConverter.GetBytes(AnalysisID);
            memStream.Write(data, 0, data.Length);
            return memStream.ToArray();
        }

        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool DecodeData(byte[] data)
        {
            //Nothing to decode for Connection_Req
            return true;
        }
        #endregion
    }
}