using STP.RouteAssessment.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.RouteAssessment.Messages
{
    public class MsgConnectReq : Message
    {
        #region constructor
        public MsgConnectReq()
        {
            SetMessageType(MessageTypes.MSG_CONNECT_REQUEST, "MSG_CONNECT_REQUEST");
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
            //Nothing to decode for Connection_Req
            return true;
        }
        #endregion
    }
}