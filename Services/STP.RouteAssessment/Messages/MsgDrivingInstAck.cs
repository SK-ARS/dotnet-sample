using STP.RouteAssessment.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.RouteAssessment.Messages
{
    public class MsgDrivingInstAck : Message
    {
        #region properties
        public byte Result { get; set; }           //Result of RouteInstruction Request 1 means success 2 means failed
        
        public enum ENUM_RESULT
        {
            DRIVING_INST_GEN_SUCCESS = 0,
            DRIVING_INST_GEN_FAILED = 2
        }
        #endregion

        #region constructor
        public MsgDrivingInstAck()
        {
            SetMessageType(MessageTypes.MSG_ROUTE_INSTRUCTION_ACK, "MSG_ROUTE_INSTRUCTION_ACK");
        }
        #endregion

        #region methods
        /// <summary>
        /// Abstract Method EncodeDate  overridden from Message
        /// </summary>
        /// <returns></returns>
        public override byte[] EncodeData()
        {
            return new byte[0]; //NO need to encode this message we are only receiving this message
        }

        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool DecodeData(byte[] data)
        {
            Result = data[0]; //First byte contains the data
            return true;
        }
        #endregion
    }
}