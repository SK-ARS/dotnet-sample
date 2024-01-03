using STP.Common.Configuration;
using STP.Common.Logger;
using STP.Domain.RoutePlannerInterface;
using STP.Routes.Messages;
using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;

namespace STP.RoutePlannerInterface.Socket
{
    public class RoutePlannerConnect
    {
        #region Properties
        private TcpClient m_TCPClient;        
        private NetworkStream m_NetworStream;  
        private bool m_bConnected;
        enum Read_Status
        {
            ReadingHeader = 1,
            ReadingLength,
            ReadBody
        };
        private Read_Status m_ReadStatus; 
        private RPMessageHeader msg_Header; 
        public RoutePlannerConnect()
        {
            m_bConnected = false;
            m_ReadStatus = Read_Status.ReadingHeader;
        }
        ~RoutePlannerConnect()
        {
            if (m_bConnected)
            {
                try
                {
                    if (m_NetworStream != null) m_NetworStream.Close();
                    if (m_TCPClient != null) m_TCPClient.Close();
                    m_NetworStream = null;
                    m_TCPClient = null;
                    m_bConnected = false;
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR,
                         Logger.LogInstance + @" - Unable to connect Route Planner,  Exception:" + ex);
                }
            }
        }
        #endregion

        #region Establish TCP/IP Connection
        public bool Connect(string strHost, int nPort, UInt16 rpClientId)
        {
            int RouteRequestTimeDelay = Settings.GetInstance().GetRouteRequestTimeDelay();
            if (!m_bConnected)
            {
                try
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, 
                        Logger.LogInstance + @" - Connecting to RoutePlanner Host:" + strHost + ",Port:" + nPort);
                    m_TCPClient = new TcpClient(strHost, nPort);
                    m_NetworStream = m_TCPClient.GetStream();
                    m_bConnected = true; 
                    Message message = new MsgConnectReq(rpClientId);
                    m_bConnected = SendMessage(message);
                    m_ReadStatus = Read_Status.ReadingHeader;
                    message = WaitForMessageResponse(MessageTypes.MSG_CONNECT_CONFIRM, RouteRequestTimeDelay);
                    if (message == null)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, 
                            Logger.LogInstance + @" - CONNECTION_ACCEPTED  Message not received from Route Planner");
                        m_bConnected = false;
                    }
                    else if (message.MsgHeader.Message_Type == MessageTypes.MSG_CONNECT_CONFIRM)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL,
                            Logger.LogInstance + @" - CONNECTION_ACCEPTED received from RoutePlanner");
                    }
                }
                catch (SocketException ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR,
                        Logger.LogInstance + @" - Socket Exception in Connecting to RoutePlanner,  Exception:" + ex);
                }
                catch (ArgumentNullException ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR,
                        Logger.LogInstance + @" - Argument Null Exception in Connecting to RoutePlanner,  Exception:" + ex);
                }
                finally
                {
                    if (!m_bConnected)
                    {
                        try
                        {
                            if (m_TCPClient != null) m_TCPClient.Close();
                            if (m_NetworStream != null) m_NetworStream.Close();
                        }
                        catch (Exception ex)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR,
                                Logger.LogInstance + @" - Exception in Connecting to Route Planner,  Exception" + ex);
                        }
                    }
                }
            }
            return m_bConnected;
        }
        #endregion

        #region Send Message
        bool SendMessage(Message message)
        {
            bool bResult = false;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.FUNCTIONAL, Logger.LogInstance + @" - Sending Message:" + message.MessageName);
                Byte[] data = message.GetBytes();
                m_NetworStream.Write(data, 0, data.Length);
                m_NetworStream.FlushAsync();
                bResult = true;
            }
            catch (SocketException ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Socket Exception in Sending message, Exception:" + ex);
                try
                {
                    m_NetworStream.Close();
                    m_TCPClient.Close();
                    m_NetworStream = null;
                    m_TCPClient = null;
                    m_bConnected = false;
                    bResult = false;
                }
                catch (Exception e)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception in Sending Message,  Exception" + e);
                }
            }
            return bResult;
        }
        #endregion

        #region Get Route Data
        public RouteData GetRoute(RouteViaWaypointEx routeViaPointEx)
        {
            RouteData routeData;
            string routePlannerIP = Settings.GetInstance().GetRoutePlannerIP();
            int routePlannerPort = Settings.GetInstance().GetRoutePlannerPort();
            UInt16 rpClientId = Settings.GetInstance().RPClientId;
            bool useExtendedMsg = Settings.GetInstance().UseExtendedMessage;

            if (Connect(routePlannerIP, routePlannerPort, rpClientId))
            {
                if (useExtendedMsg)
                {
                    routeData = GetPlannedRoute(routeViaPointEx);
                }
                else
                {
                    RouteViaWaypoint routeViaWayPoint = routeViaPointEx.GetRouteViaPoint();
                    routeData = GetPlannedRoute(routeViaWayPoint);
                }
            }
            else
            {
                routeData = new RouteData();
                routeData.ResponseMessage = "Connection Failed";
            }
            if (m_bConnected)
            {
                try
                {
                    m_NetworStream.Close();
                    m_TCPClient.Close();
                    m_NetworStream = null;
                    m_TCPClient = null;
                    m_bConnected = false;
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception in Get Route,  Exception:" + ex);
                }
            }
            return routeData;
        }
        #endregion

        #region Get Planned Route

        #region Param RouteViaWaypoint
        private RouteData GetPlannedRoute(RouteViaWaypoint routeViaPoint)
        {
            RouteData routeData = null;
            int routeRequestTimeDelay = Settings.GetInstance().GetRouteRequestTimeDelay();
            MessageRouteReq msgRoute_Req = new MessageRouteReq(routeViaPoint);
            if (SendMessage(msgRoute_Req))
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Send Route Request to RoutePlanner");
                m_ReadStatus = Read_Status.ReadingHeader;
                MessagePlannedRoute msgPlannedRoute = (MessagePlannedRoute)WaitForMessageResponse(MessageTypes.MSG_PLANNED_ROUTE,
                    routeRequestTimeDelay);
                if (msgPlannedRoute != null)
                {
                    routeData = msgPlannedRoute.GetRouteData();
                }
                else
                {
                    routeData = new RouteData();
                    routeData.ResponseMessage = "NO_RESPONSE_FROM_ROUTEPLANNER";
                }
            }
            return routeData;
        }
        #endregion

        #region Param RouteViaWaypointEx
        private RouteData GetPlannedRoute(RouteViaWaypointEx routeViaPointEx)
        {
            RouteData routeData = null;
            int routeRequestTimeDelay = Settings.GetInstance().GetRouteRequestTimeDelay();
            MessageRouteReqEx msgRoute_ReqEx = new MessageRouteReqEx(routeViaPointEx);
            if (SendMessage(msgRoute_ReqEx))
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Send Route Request Extended to RoutePlanner");
                m_ReadStatus = Read_Status.ReadingHeader;
                MessagePlannedRoute msgPlannedRoute = (MessagePlannedRoute)WaitForMessageResponse(MessageTypes.MSG_PLANNED_ROUTE,
                    routeRequestTimeDelay);
                if (msgPlannedRoute != null)
                {
                    routeData = msgPlannedRoute.GetRouteData();
                }
                else
                {
                    routeData = new RouteData();
                    routeData.ResponseMessage = "NO_RESPONSE_FROM_ROUTEPLANNER";
                }
            }
            return routeData;
        }
        #endregion

        #endregion

        #region Wait For Message Response
        public Message WaitForMessageResponse(int nMessageType, int nWaitTime)
        {
            Message message = null;
            bool btimeout = true;
            try
            {
                if (m_NetworStream.CanRead)
                {
                    byte[] myReadBuffer = new byte[1024];
                    MemoryStream memStream = new MemoryStream();
                    int numberOfBytesRead = 0;
                    long starttimeStamp = DateTime.UtcNow.Ticks / 10000; 
                    long finaltimestamp;
                    long streamReadPosition = 0;
                    long streamEndPosition = 0;
                    do
                    {
                        if (m_NetworStream.DataAvailable)
                        {
                            numberOfBytesRead = m_NetworStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                            if (numberOfBytesRead > 0)
                            {
                                memStream.Position = streamEndPosition;
                                memStream.Write(myReadBuffer, 0, numberOfBytesRead);
                                streamEndPosition = memStream.Position; 
                                memStream.Position = streamReadPosition;
                                message = DecodeResponse(memStream);
                                streamReadPosition = memStream.Position;
                                if (message != null)
                                {
                                    if (message.MsgHeader.Message_Type == nMessageType)
                                    {
                                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL,
                                            Logger.LogInstance + @" - Correct Message received from RoutePlanner. Expected Message:" + nMessageType + 
                                            ",Received Message:" + MessageFactory.GetMessgageTypeString(message.MsgHeader.Message_Type));
                                        btimeout = false;
                                        break;
                                    }
                                    else
                                    {
                                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR,
                                            Logger.LogInstance + @" - InCorrect Message received from RoutePlanner. Expected Message:" + nMessageType + 
                                            ",Received Message:" + MessageFactory.GetMessgageTypeString(message.MsgHeader.Message_Type));
                                    }
                                }
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                        finaltimestamp = DateTime.UtcNow.Ticks / 10000;
                    }
                    while (m_NetworStream.DataAvailable || (finaltimestamp - starttimeStamp) < nWaitTime);
                    memStream.Dispose();
                }
            }
            catch (SocketException)
            {
                try
                {
                    m_NetworStream.Close();
                    m_TCPClient.Close();
                    m_NetworStream = null;
                    m_TCPClient = null;
                    m_bConnected = false;
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception in Wait For Message Response,  Exception:" + ex);
                }
            }
            if (btimeout)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Message not received even after waiting for" +nWaitTime + "millis");
            }
            return message;
        }
        #endregion

        #region Decode Response
        private Message DecodeResponse(MemoryStream memStream)
        {
            Message message = null;
            bool bHasMessageToRead = true;
            while (bHasMessageToRead)
            {
                if (m_ReadStatus == Read_Status.ReadingHeader)
                {
                    if (memStream.Length >= RPMessageHeader.HEADER_SIZE)
                    {
                        byte[] headerBytes = new byte[RPMessageHeader.HEADER_SIZE]; 
                        memStream.Read(headerBytes, 0, RPMessageHeader.HEADER_SIZE);
                        RPMessageHeader msgHeader = RPMessageHeader.DecodeHeader(headerBytes);
                        if (msgHeader == null) break; 

                        if (MessageFactory.IsValidMessageType(msgHeader.Message_Type))
                        {
                            msg_Header = msgHeader;
                            m_ReadStatus = Read_Status.ReadBody; 
                        }
                        else
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Invalid Message Type" + msgHeader.Message_Type);
                            msg_Header = null;
                            m_ReadStatus = Read_Status.ReadingHeader;
                        }
                    }
                    else
                    {
                        bHasMessageToRead = false;
                    }
                }
                else if (m_ReadStatus == Read_Status.ReadBody)
                {
                    if (memStream.Length - memStream.Position >= msg_Header.Message_Length)
                    {
                        byte[] data = new byte[msg_Header.Message_Length];
                        memStream.Read(data, 0, msg_Header.Message_Length);
                        message = MessageFactory.generateMessage(msg_Header);
                        if (message != null) message.DecodeData(data);
                    }
                    break;
                }
            }
            return message;
        }
        #endregion
    }
}