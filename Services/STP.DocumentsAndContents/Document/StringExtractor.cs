using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;

namespace STP.DocumentsAndContents.Document
{
    public class StringExtractor
    {
        #region XmlDeserializerStructures function
        /// <summary>
        /// Function to deserialize the Analysed structure's Xml Data 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static AnalysedStructures XmlDeserializerStructures(string xmlString)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AnalysedStructures));

                StringReader read = new StringReader(xmlString);

                using (XmlReader xmlReader = new XmlTextReader(read))
                {
                    Object obj = serializer.Deserialize(xmlReader);

                    AnalysedStructures structures = (AnalysedStructures)obj;

                    xmlReader.Close();

                    return structures;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

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

        public static string XmlCautionSerializer(List<AnalysedCautionsPart> XmlData)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AnalysedCautionsPart>));

                StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
                serializer.Serialize(outStream, XmlData);
                string str = outStream.ToString();

                return outStream.ToString(); // Output string 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}