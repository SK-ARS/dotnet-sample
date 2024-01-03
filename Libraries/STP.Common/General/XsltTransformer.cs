//  Name: XSLTTransformer.cs
//  Description: The XSLT Transformer will decompress the compressed xml data
//  Created By: NetWeb / Intellizenz
//  Revision         :   9-Apr-2014
//9-Apr-14 Added code to decompress xml data without XSLT

using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Xsl;

namespace STP.Common.General
{
    public class XsltTransformer
    {
        public XsltTransformer()
        {
            //default constructor
        }

        //function to convert xml
        public static string Trafo(byte[] XMLPage, string XSLStylesheet, out string ErrMsg)
        {
            string result = string.Empty;
            ErrMsg = "No Data Found";

            try
            {
                //read XML 
                Stream stream = new MemoryStream(XMLPage);
                //Try to inzip the input stream may be its a gzip
                using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    XmlReader xmlReader;
                    try
                    {
                        xmlReader = new XmlTextReader(zipStream);
                    }
                    catch (Exception exc)
                    {
                        //The provided input stream is not a valid zip so process as normal xml text file
                        stream.Position = 0; //reset the position of the stream to begining
                        xmlReader = new XmlTextReader(stream);
                    }
                    //read xslt path 
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(XSLStylesheet);
                    TextWriter txtWriter = new StringWriter();
                    xslt.Transform(xmlReader, null, txtWriter);
                    //get result
                    result = txtWriter.ToString();

                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                ErrMsg = ex.Message;
            }
            return result;
        }


        //function to convert zip data to byte stream
        public static byte[] Trafo(byte[] XMLPage)
        {
            var result = new MemoryStream();
            var memoryStream = new MemoryStream();

            if (XMLPage != null)
            {
                memoryStream = new MemoryStream(XMLPage);
            }
           
            try
            {
                var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                zipStream.CopyTo(result);
            }
            catch (Exception exc)
            {
                //The provided input stream is not a valid zip so process as normal xml text file
                memoryStream.Position = 0; //reset the position of the stream to begining
                result = memoryStream;
            }
            return result.ToArray();

        }

        // Function to compress data to gzip format
        public static byte[] CompressData(byte[] content)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzStream = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzStream.Write(content, 0, content.Length);
                }
                return ms.ToArray();
            } 
        }

        public static string Trafo(byte[] XMLPage, string XSLStylesheet, int orgId,int userTypeId, out string ErrMsg)
        {
            
            string result = string.Empty;
            ErrMsg = "No Data Found";

            XsltArgumentList argsList = new XsltArgumentList();
            argsList.AddParam("OrgId", "", orgId);
            argsList.AddParam("UserTypeId", "", userTypeId);

            try
            {
                //read XML 
                Stream stream = new MemoryStream(XMLPage);
                //Try to inzip the input stream may be its a gzip
                using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    XmlReader xmlReader;
                    try
                    {
                        xmlReader = new XmlTextReader(zipStream);
                    }
                    catch (Exception exc)
                    {
                        //The provided input stream is not a valid zip so process as normal xml text file
                        stream.Position = 0; //reset the position of the stream to begining
                        xmlReader = new XmlTextReader(stream);
                    }
                    //read xslt path 
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(XSLStylesheet);
                    TextWriter txtWriter = new StringWriter();
                    xslt.Transform(xmlReader, argsList, txtWriter);
                    //get result
                    result = txtWriter.ToString();

                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                ErrMsg = ex.Message;
            }
            return result;
        }

        public static string TrafoSORT(byte[] XMLPage, string XSLStylesheet, out string ErrMsg)
        {
            string result = string.Empty;
            ErrMsg = "No Data Found";

            XsltArgumentList argsList = new XsltArgumentList();
            argsList.AddParam("SORTType", "", 1);

            try
            {
                //read XML 
                Stream stream = new MemoryStream(XMLPage);
                //Try to inzip the input stream may be its a gzip
                using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    XmlReader xmlReader;
                    try
                    {
                        xmlReader = new XmlTextReader(zipStream);
                    }
                    catch (Exception exc)
                    {
                        //The provided input stream is not a valid zip so process as normal xml text file
                        stream.Position = 0; //reset the position of the stream to begining
                        xmlReader = new XmlTextReader(stream);
                    }
                    //read xslt path 
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(XSLStylesheet);
                    TextWriter txtWriter = new StringWriter();
                    xslt.Transform(xmlReader, argsList, txtWriter);
                    //get result
                    result = txtWriter.ToString();

                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                ErrMsg = ex.Message;
            }
            return result;
        }
    }
}
