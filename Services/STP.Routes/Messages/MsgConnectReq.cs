using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Messages
{
    public class MsgConnectReq : Message
    {
        #region properties
        public ushort RP_Client_ID { get; set; }
        #endregion

        #region constructor
        public MsgConnectReq(ushort rpClientID)
        {
            SetMessageType(MessageTypes.MSG_CONNECT_REQUEST, "MSG_CONNECT_REQUEST");
            RP_Client_ID = rpClientID;
        }
        #endregion

        #region methods

        /// <summary>
        /// Abstract Method EncodeDate  overridden from Message
        /// </summary>
        /// <returns></returns>
        public override byte[] EncodeData()
        {
            return BitConverter.GetBytes(RP_Client_ID);
        }

        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool DecodeData(byte[] data)
        {
            RP_Client_ID = BitConverter.ToUInt16(data, 0);
            return true;
        }
        #endregion
    }
}