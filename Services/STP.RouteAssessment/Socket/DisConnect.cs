using STP.Common.Configuration;
using STP.Common.Logger;
using STP.Domain.DrivingInstructionsInterface;
using STP.RouteAssessment.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace STP.RouteAssessment.Socket
{
    public class DisConnect
    {
        #region Properties
        private TcpClient m_TCPClient;        //TCP Client member
        private NetworkStream m_NetworStream;  //NetworkStream
        private bool m_bConnected;    //Whether Connected to TCP
        enum Read_Status
        {
            ReadingHeader = 1,
            ReadingLength,
            ReadBody
        };
        private Read_Status m_ReadStatus; //Variable to keep stream read status
        private DisMessageHeader msg_Header; //The Header of the message being read
        public DisConnect()
        {
            m_bConnected = false;
            m_ReadStatus = Read_Status.ReadingHeader;
        }
        ~DisConnect()
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
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - DisConnect, Exception: " + ex​​​​);
                }
            }
        }
        #endregion

        #region Establish TCP/IP Connection
        public bool Connect(string strHost, int nPort)
        {
            if (!m_bConnected)
            {
                try
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, 
                        Logger.LogInstance + @" - Connecting to DrivingInstructor Host: " + strHost + ",Port:" + nPort);
                    m_TCPClient = new TcpClient(strHost, nPort);
                    m_NetworStream = m_TCPClient.GetStream();
                    m_bConnected = true; //If we are here means connection is success

                    Message message = new MsgConnectReq();
                    m_bConnected = SendMessage(message); //Send Connect Request
                    m_ReadStatus = Read_Status.ReadingHeader;

                    message = WaitForMessageResponse(MessageTypes.MSG_CONNECT_ACK, 10000); //Get Connection Accept Message
                    if (message == null)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, 
                            Logger.LogInstance + @" - CONNECTION_ACCEPTED  Message not received from Driving Instructor");
                        m_bConnected = false;
                    }
                    else if (message.MsgHeader.Message_Type == MessageTypes.MSG_CONNECT_ACK)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, Logger.LogInstance + @" - CONNECTION_ACCEPTED received from Driving Instructor");
                    }
                }
                catch (SocketException e)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Socket Exception in Connecting to Driving Instructor" + e);
                }
                catch (ArgumentNullException ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Socket Exception in Connecting to Driving Instructor" + ex);
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
                        catch (Exception  ex)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Connecting to Driving Instructor, Exception:" + ex​​​​);
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
                m_NetworStream.FlushAsync();//Flush data to server
                bResult = true;
            }
            catch (SocketException e)
            {
                //Some Connection Issue Connection Lost
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Socket Exception in writing data" + e);
                try
                {
                    m_NetworStream.Close();
                    m_TCPClient.Close();
                    m_NetworStream = null;
                    m_TCPClient = null;
                    m_bConnected = false;
                    bResult = false;
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SendMessage, Exception: " + ex​​​​);
                }
            }
            return bResult;
        }
        #endregion

        #region Generate DI and RD
        /// Gets Route via way point
        public int GenerateDrivingInstnRouteDesc(DrivingInsReq drivingInsReq, DBSchema schema = DBSchema.portal)
        {
            bool bResult = false;
            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DI Service Log, schema: {0},AnalysisID:{1}", schema, drivingInsReq.AnalysisID));
            string routeInstructorIP = Settings.GetInstance().RouteInstructorIP;
            int routeInstructorPort = Settings.GetInstance().RouteInstructorport;
            int DICode = 0;
            if (Connect(routeInstructorIP, routeInstructorPort))
            {
                MsgDrivingInstReqEx msgDrivingInsReq = new MsgDrivingInstReqEx(drivingInsReq, schema);
                if (SendMessage(msgDrivingInsReq))
                {
                    MsgDrivingInstAck response = (MsgDrivingInstAck)WaitForMessageResponse(MessageTypes.MSG_ROUTE_INSTRUCTION_ACK, 120000);
                    if (response != null && response.Result == (byte)MsgDrivingInstAck.ENUM_RESULT.DRIVING_INST_GEN_SUCCESS)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FUNCTIONAL, Logger.LogInstance + @" - Driving instruction generated for:", drivingInsReq.AnalysisID);
                        bResult = true;
                        DICode = 5;
                    }
                    else if (response != null && response.Result == (byte)MsgDrivingInstAck.ENUM_RESULT.DRIVING_INST_GEN_FAILED)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FUNCTIONAL, Logger.LogInstance + @" - Driving instruction not generated for:", drivingInsReq.AnalysisID);
                        DICode = 3;//– Request send, but return FALSE 
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FUNCTIONAL, Logger.LogInstance + @" - Request send but timed out !! No response");
                        DICode = 2;
                    }
                }
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FUNCTIONAL, Logger.LogInstance + @" - Socket connection failed");
                DICode = 1; 
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
                    Logger.GetInstance().LogMessage(Log_Priority.FUNCTIONAL, Logger.LogInstance + @" - TCP Client and Network stream cannot be closed" + ex);
                    DICode = 4; 
                }
            }
            return bResult ? 5 : DICode;
        }
        #endregion

        #region Response Message
        //Wait for receiving the passed messagetype for nWaitTime
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
                    long starttimeStamp = DateTime.UtcNow.Ticks / 10000; //Get milliseconds
                    long finaltimestamp;
                    long streamReadPosition = 0;
                    long streamEndPosition = 0;
                    m_ReadStatus = Read_Status.ReadingHeader;
                    do
                    {
                        if (m_NetworStream.DataAvailable)
                        {
                            numberOfBytesRead = m_NetworStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                            if (numberOfBytesRead > 0)
                            {
                                memStream.Position = streamEndPosition;// Set position to end position where data is already there
                                memStream.Write(myReadBuffer, 0, numberOfBytesRead);
                                streamEndPosition = memStream.Position; //Set the stream end position to temp variable
                                memStream.Position = streamReadPosition;
                                message = DecodeResponse(memStream);
                                streamReadPosition = memStream.Position;
                                if (message != null)
                                {
                                    if (message.MsgHeader.Message_Type == nMessageType)
                                    {
                                        //We have some error
                                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL,
                                            Logger.LogInstance + @" - Correct Message received from Driving Instructor. Expected Message:" + nMessageType + 
                                            ",Received Message:" + MessageFactory.GetMessgageTypeString(message.MsgHeader.Message_Type));
                                        btimeout = false;
                                        break;
                                    }
                                    else
                                    {
                                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR,
                                            Logger.LogInstance + $" - InCorrect Message received from Driving Instructor. Expected Message:" + nMessageType+
                                            ",Received Message:" + MessageFactory.GetMessgageTypeString(message.MsgHeader.Message_Type));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Just wait 100 ms to see data coming in other wise this loop will kill the cpu
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
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - WaitForMessageResponse, Exception: " + ex​​​​);
                }
            }
            if (btimeout)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Message not received even after waiting for " + nWaitTime + "millis");
            }
            return message;
        }
        #endregion

        #region Decode response
        //Decode the Message received in String Builder
        private Message DecodeResponse(MemoryStream memStream)
        {
            Message message = null;
            bool bHasMessageToRead = true;
            while (bHasMessageToRead)
            {
                if (m_ReadStatus == Read_Status.ReadingHeader)
                {
                    if (memStream.Length >= DisMessageHeader.HEADER_SIZE)
                    {
                        byte[] headerBytes = new byte[DisMessageHeader.HEADER_SIZE]; //Read header to byte array
                        memStream.Read(headerBytes, 0, DisMessageHeader.HEADER_SIZE);
                        DisMessageHeader msgHeader = DisMessageHeader.DecodeHeader(headerBytes);
                        if (msgHeader == null) break; //Incorrect Header
                        if (MessageFactory.IsValidMessageType(msgHeader.Message_Type))
                        {
                            msg_Header = msgHeader;
                            m_ReadStatus = Read_Status.ReadBody; //Read Body
                        }
                        else
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Invalid Message Type :" + msgHeader.Message_Type);
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
                    //We have to read the body
                    //First check whether the message really have a body if not nothing to read on the body
                    if (MessageFactory.hasMessageBody(msg_Header.Message_Type))
                    {
                        if (memStream.Length - memStream.Position >= (long)msg_Header.Message_Length)
                        {
                            //Proceed to read message
                            byte[] data = new byte[msg_Header.Message_Length];
                            memStream.Read(data, 0, (int)msg_Header.Message_Length);
                            message = MessageFactory.generateMessage(msg_Header);
                            if (message != null) message.DecodeData(data);
                        }
                        break;
                    }
                    else
                    {
                        message = MessageFactory.generateMessage(msg_Header); //Generate the message object and return
                        break;
                    }
                }
            }
            return message;
        }
        #endregion
    }
}