using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Messages
{
    public static class MessageFactory
    {
        public static bool IsValidMessageType(byte msgType)
        {
            switch (msgType)
            {
                case MessageTypes.MSG_CONNECT_REQUEST:
                case MessageTypes.MSG_CONNECT_CONFIRM:
                case MessageTypes.MSG_DISCONNECT_REQUEST:
                case MessageTypes.MSG_MESSAGE_ACK:
                case MessageTypes.MSG_ROUTE_REQUEST:
                case MessageTypes.MSG_PLANNED_ROUTE:
                case MessageTypes.MSG_ROUTE_REQUEST_EX:
                    return true;
                default:
                    return false;
            }
        }
        internal static Message generateMessage(RPMessageHeader msg_Header)
        {
            Message message = null;
            switch (msg_Header.Message_Type)
            {
                case MessageTypes.MSG_CONNECT_CONFIRM:
                    message = new MsgConnectConfirm();
                    message.MsgHeader = msg_Header;
                    break;
                case MessageTypes.MSG_DISCONNECT_REQUEST:
                    message = new MsgDisconnectReq();
                    message.MsgHeader = msg_Header;
                    break;
                case MessageTypes.MSG_PLANNED_ROUTE:
                    message = new MessagePlannedRoute();
                    message.MsgHeader = msg_Header;
                    break;
                case MessageTypes.MSG_ROUTE_REQUEST_EX:
                    message = new MessageRouteReqEx();
                    message.MsgHeader = msg_Header;
                    break;
            }
            return message;
        }
        internal static String GetMessgageTypeString(byte msgType)
        {
            switch (msgType)
            {
                case MessageTypes.MSG_CONNECT_CONFIRM:
                    return "MSG_CONNECT_CONFIRM";
                case MessageTypes.MSG_DISCONNECT_REQUEST:
                    return "MSG_DISCONNECT_REQUEST";
                case MessageTypes.MSG_PLANNED_ROUTE:
                    return "MSG_PLANNED_ROUTE";
                case MessageTypes.MSG_CONNECT_REQUEST:
                    return "MSG_CONNECT_REQUEST";
                case MessageTypes.MSG_MESSAGE_ACK:
                    return "MSG_MESSAGE_ACK";
                case MessageTypes.MSG_ROUTE_REQUEST:
                    return "MSG_ROUTE_REQUEST";
                case MessageTypes.MSG_ROUTE_REQUEST_EX:
                    return "MSG_ROUTE_REQUEST_EX";
                default:
                    return string.Format("UNKNOWN_MESSAGE of type {0}", msgType);
            }
        }
    }
    public static class MessageTypes
    {
        public const byte MSG_CONNECT_REQUEST = 1; //First Message send after establishing a connection send from client
        public const byte MSG_CONNECT_CONFIRM = 2; //Connection Acknowledgment message from server
        public const byte MSG_DISCONNECT_REQUEST = 3; //Message from Client requesting for Route Instruction
        public const byte MSG_MESSAGE_ACK = 11;//Acknowledgement Message from server on Route Planning Request (Or any Message)
        public const byte MSG_ROUTE_REQUEST = 50; //Message from client requesting a Route
        public const byte MSG_PLANNED_ROUTE = 51; //Message from server with the planned route returned (To client)
        public const byte MSG_ROUTE_REQUEST_EX = 52; //Extended Message from client requesting a Route
    }
}