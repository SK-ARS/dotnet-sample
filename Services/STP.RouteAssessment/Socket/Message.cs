using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace STP.RouteAssessment.Socket
{
    public abstract class Message
    {
        #region Properties
        public DisMessageHeader MsgHeader { get; set; }
        public string MessageName { get; set; }
        #endregion

        #region Constructor
        protected Message()
        {
            MsgHeader = new DisMessageHeader(); //Initialise the Message Header
        }
        #endregion

        #region methods
        /// <summary>
        /// Sets the Message Type
        /// </summary>
        /// <param name="messageType"></param>
        protected void SetMessageType(byte messageType, string messageName)
        {
            MsgHeader.Message_Type = messageType;
            MessageName = messageName;
        }


        /// <summary>
        /// Method returning the message encoded as byte stream including header
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            MemoryStream outStream = new MemoryStream();

            byte[] dataBytes = EncodeData();   //First get the encoded byte stream from the data message

            //Now we will finish the header with check sum length
            MsgHeader.Message_Length = (ulong)dataBytes.LongLength; //Set the message length to header
                                                                    //Message Length and checksum added now compose the message

            byte[] headerBytes = MsgHeader.GetBytes(); //Get encoded header bytes

            outStream.Write(headerBytes, 0, headerBytes.Length);     //write header byte to stream
            outStream.Write(dataBytes, 0, dataBytes.Length);         //write data message to the stream
            return outStream.ToArray();
        }
        #endregion


        /// <summary>
        /// Abstract Method EncodeDate to be overridden by base Messages
        /// </summary>
        /// <returns></returns>
        public abstract byte[] EncodeData();
        /// <summary>
        /// Abstract Method DecodeData to be overridden by Base Messages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool DecodeData(byte[] data);
    }
}