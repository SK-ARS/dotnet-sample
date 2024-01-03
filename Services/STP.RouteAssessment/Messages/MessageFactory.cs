using STP.RouteAssessment.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.RouteAssessment.Messages
{
    public static class MessageFactory
    {
        /// <summary>
        /// Checks whether passed message is a valid message type
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public static bool IsValidMessageType(byte msgType)
        {
            switch (msgType)
            {
                case MessageTypes.MSG_CONNECT_REQUEST:
                case MessageTypes.MSG_CONNECT_ACK:
                case MessageTypes.MSG_ROUTE_INST_DBG_ACK:
                case MessageTypes.MSG_ROUTE_INST_DBG_REQ:
                case MessageTypes.MSG_ROUTE_INSTRUCTION_ACK:
                //case MessageTypes.MSG_ROUTE_INSTRUCTION_REQ: Obsolete will not use
                case MessageTypes.MSG_ROUTE_INSTRUCTION_REQ_EX:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Factory method for generating the Message object from the header
        /// </summary>
        /// <param name="msg_Header"></param>
        /// <returns></returns>
        internal static Message generateMessage(DisMessageHeader msg_Header)
        {
            Message message = null;
            switch (msg_Header.Message_Type)
            {
                case MessageTypes.MSG_CONNECT_REQUEST:
                    message = new MsgConnectReq();
                    message.MsgHeader = msg_Header;
                    break;
                case MessageTypes.MSG_CONNECT_ACK:
                    message = new MsgConnectAck();
                    message.MsgHeader = msg_Header;
                    break;
                case MessageTypes.MSG_ROUTE_INSTRUCTION_REQ_EX:
                    message = new MsgDrivingInstReqEx();
                    message.MsgHeader = msg_Header;
                    break;
                case MessageTypes.MSG_ROUTE_INSTRUCTION_ACK:
                    message = new MsgDrivingInstAck();
                    message.MsgHeader = msg_Header;
                    break;
            }
            return message;
        }

        internal static String GetMessgageTypeString(byte msgType)
        {
            switch (msgType)
            {
                case MessageTypes.MSG_CONNECT_REQUEST:
                    return "MSG_CONNECT_REQUEST";
                case MessageTypes.MSG_CONNECT_ACK:
                    return "MSG_CONNECT_ACK";
                case MessageTypes.MSG_ROUTE_INSTRUCTION_REQ:
                    return "MSG_PLANNED_ROUTE";
                case MessageTypes.MSG_ROUTE_INSTRUCTION_ACK:
                default:
                    return string.Format("UNKNOWN_MESSAGE of type {0}", msgType);
            }
        }

        /// <summary>
        /// Checks whether the passed message type has a message body or not
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        internal static bool hasMessageBody(byte msgType)
        {
            switch (msgType)
            {
                case MessageTypes.MSG_CONNECT_REQUEST:
                case MessageTypes.MSG_CONNECT_ACK:
                    return false;//Above messages dont have any body
                default:
                    return true; //All other messages have body
            }
        }
    }
    public static class MessageTypes
    {
        public const byte MSG_CONNECT_REQUEST = 1; //First Message send after establishing a connection send from client
        public const byte MSG_CONNECT_ACK = 2; //Connection Acknowledgment message from server
        public const byte MSG_ROUTE_INSTRUCTION_REQ = 3; //Message from Client requesting for Route Instruction ###Obsolete No Longer used from 9 Sept 2014####
        public const byte MSG_ROUTE_INSTRUCTION_ACK = 4;//Acknowledgement Message from server on Route Planning Request (Or any Message)
        public const byte MSG_ROUTE_INST_DBG_REQ = 5; //Message from client requesting a Route
        public const byte MSG_ROUTE_INST_DBG_ACK = 6; //Message from server with the planned route returned (To client)
        public const byte MSG_ROUTE_INSTRUCTION_REQ_EX = 7; //Message from Client requesting for Route Instruction This will be replacing old MSG_ROUTE_INSTRUCTION_REQ message
    }
}