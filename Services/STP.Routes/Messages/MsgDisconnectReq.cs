using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Messages
{
    public class MsgDisconnectReq : Message
    {
        #region constructor
        public MsgDisconnectReq()
        {
            SetMessageType(MessageTypes.MSG_DISCONNECT_REQUEST, "MSG_DISCONNECT_REQUEST");
        }
        #endregion

        #region methods
        /// <summary>
        /// Abstract Method EncodeDate  overridden from Message
        /// </summary>
        /// <returns></returns>
        public override byte[] EncodeData()
        {
            //This message doesnt have any data parts
            byte[] data = new byte[0];
            return data;
        }

        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool DecodeData(byte[] data)
        {
            //This message nothing have any data part so nothing to decode
            return true;
        }
        #endregion
    }
}