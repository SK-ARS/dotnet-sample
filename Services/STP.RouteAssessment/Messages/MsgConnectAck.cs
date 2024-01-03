using STP.RouteAssessment.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.RouteAssessment.Messages
{
    public class MsgConnectAck : Message
    {
        #region constructor
        public MsgConnectAck()
        {
            SetMessageType(MessageTypes.MSG_CONNECT_ACK, "MSG_CONNECT_ACK");
        }
        #endregion

        #region methods

        /// <summary>
        /// Abstract Method EncodeDate  overridden from Message
        /// </summary>
        /// <returns></returns>
        public override byte[] EncodeData()
        {
            return new byte[0];
        }

        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool DecodeData(byte[] data)
        {
            //Nothing to decode for Connection_Ack
            return true;
        }
        #endregion
    }
}