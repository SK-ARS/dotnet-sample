using STP.Common.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace STP.RoutePlannerInterface.Socket
{
    public class RPMessageHeader
    {
        #region properties
        /// <summary>
        /// Contains the message start sequence heading bytes
        /// </summary>
        public static readonly byte[] MESSAGE_HEAD = new byte[] { 0xEF, 0xEF, 0xFE, 0xFE };
        /// <summary>
        /// Property Message Type
        /// </summary>
        public byte Message_Type { get; set; }
        /// <summary>
        /// Message Length
        /// </summary>
        public UInt16 Message_Length { get; set; }
        /// <summary>
        /// CheckSum field
        /// </summary>
        public byte CheckSum { get; set; }
        /// <summary>
        /// Message Sequence Number
        /// </summary>
        public byte MessageNumber { get; set; }
        #endregion
        //Header messages size 9 bytes
        public const int HEADER_SIZE = 9;
        public static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];
        #region methods
        /// <summary>
        /// Returns the encoded header message as byte Array
        /// Ensure that all header properties are filled before calling this method
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] dataBytes;
            MemoryStream memStream = new MemoryStream();
            memStream.Write(MESSAGE_HEAD, 0, MESSAGE_HEAD.Length); //Encode header with magic number
            memStream.WriteByte(Message_Type);                     //Encode the message type
            dataBytes = BitConverter.GetBytes(Message_Length);     //Encode the message length
            memStream.Write(dataBytes, 0, dataBytes.Length);
            memStream.WriteByte(CheckSum);                         //Encode Checksum
            memStream.WriteByte(MessageNumber);                    //Encode Message seq number 
            return memStream.ToArray();
        }
        #endregion
        /// <summary>
        /// Decodes the Message Header and returns the Header object
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static RPMessageHeader DecodeHeader(byte[] buffer)
        {
            if (IsHeaderCorrect(buffer))
            {
                RPMessageHeader msgHeader = new RPMessageHeader();
                msgHeader.Message_Type = buffer[4];
                msgHeader.Message_Length = BitConverter.ToUInt16(buffer, 5);
                msgHeader.CheckSum = buffer[7];
                msgHeader.MessageNumber = buffer[8];
                return msgHeader;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR,
                    LogInstance + $" - Incorrect Header in message received");
                return null;
            }
        }
        /// <summary>
        /// Checks whether the header in the messsage is correct or not
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static bool IsHeaderCorrect(byte[] buffer)
        {
            for (int nIndex = 0; nIndex < MESSAGE_HEAD.Length; ++nIndex)
            {
                if (buffer[nIndex] != MESSAGE_HEAD[nIndex])
                {
                    //Header is wrong
                    return false;
                }
            }
            return true;
        }
    }
}