using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Messages
{
    public class MsgConnectConfirm : Message
    {
        #region properties
        public byte Response { get; set; }
        #endregion

        #region constructor
        public MsgConnectConfirm()
        {
            SetMessageType(MessageTypes.MSG_CONNECT_CONFIRM, "MSG_CONNECT_CONFIRM");
        }
        #endregion

        #region methods
        /// <summary>
        /// Abstract Method EncodeDate  overridden from Message
        /// </summary>
        /// <returns></returns>
        public override byte[] EncodeData()
        {
            byte[] data = new byte[1];
            data[0] = Response;
            return data;
        }

        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool DecodeData(byte[] data)
        {
            Response = data[0];
            return true;
        }
        #endregion

        public const byte RESPONSE_CONNECT_OK = 1; //Connect OK Response Status
        public const byte RESPONSE_CREDENTIALS_NOT_KNOWN = 2; //Credentials not known
        public const byte RESPONSE_SERVER_SHUTTING_DOWN = 3;  //Server Shutting down
        public const byte RESPONSE_ALREADY_CONNECTED = 4;     //Client Already connected
    }
}