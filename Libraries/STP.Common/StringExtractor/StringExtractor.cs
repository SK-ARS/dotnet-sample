using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace STP.Common.StringExtractor
{
    public class StringExtractor
    {
        #region Utf8StringWriter sub class to overide UTF-16 encoding to UTF-8 encoding
        /// <summary>
        /// class to generate UTF8 encoded xml
        /// </summary>
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
        #endregion

        #region ZipAndBlob
        /// <summary>
        /// function to zip the constraint
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ZipAndBlob(string value)
        {
            //Converting string to byte array
            byte[] XMLPage = System.Text.Encoding.UTF8.GetBytes(value);
            string result = string.Empty;

            string ErrMsg = "No Data Found";

            try
            {
                //compressing the byte array
                MemoryStream ms = new MemoryStream();
                Stream zipStream = new GZipStream(ms, CompressionMode.Compress);

                zipStream.Write(XMLPage, 0, XMLPage.Length);
                zipStream.Close();

                byte[] compressedData = (byte[])ms.ToArray();

                //returing the compressed data
                return compressedData;

            }
            catch (Exception ex)
            {
                ErrMsg = ex.StackTrace;
            }
            return null;
        }
        #endregion

        #region RetransmitDocumentDomain
        public class RetransmitDocumentDomain
        {
            /// <summary>
            /// function to fetch reply to email address for retransmission from document
            /// </summary>
            /// <param name="Doc"></param>
            /// <param name="tagName"></param>
            /// <returns></returns>
            public static string GetHaHaulEmailAddressForRetransmit(XmlDocument Doc, string tagName)
            {
                try
                {
                    string HaEmailAddress = "";
                    string HaulEmailAddress = "";
                    XmlNodeList HaulContactlist = null, HaContactlist = null;

                    switch (tagName)
                    {
                        case "HaulierDetails":
                            HaulContactlist = Doc.GetElementsByTagName(tagName);

                            foreach (XmlElement childrenNode in HaulContactlist)
                            {
                                foreach (XmlElement xmlElement in childrenNode)
                                {
                                    if (xmlElement.Name == "EmailAddress")
                                    {
                                        HaulEmailAddress = xmlElement.InnerText;
                                    }
                                }
                            }

                            break;
                        case "HAContact":
                            HaContactlist = Doc.GetElementsByTagName(tagName);

                            foreach (XmlElement childrenNode in HaContactlist)
                            {
                                foreach (XmlElement xmlElement in childrenNode)
                                {
                                    if (xmlElement.Name == "EmailAddress")
                                    {
                                        HaEmailAddress = xmlElement.InnerText;
                                    }
                                }
                            }


                            break;
                    }
                    return tagName == "HaulierDetails" ? HaulEmailAddress : HaEmailAddress;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        #endregion
        

    }
}
