using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using ProposedRouteXSD;
using SpecialOrderXSD;
using STP.Common.Logger;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace STP.Web.Document
{
    public class GenerateXML
    {
        /// <summary>
        /// function to generate notification related xml
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="obns"></param>
        /// <param name="psPortalType"></param>
        /// <returns></returns>
        public XMLModel GenerateNotificationXML(int notificationId, NotificationXSD.OutboundNotificationStructure obns, NotificationXSD.NotificationTypeType psPortalType)
        {
            string retunXML = string.Empty;
            XMLModel model = new XMLModel();
            obns.Type = psPortalType;

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(NotificationXSD.OutboundNotificationStructure));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, obns);
                    }
                    retunXML = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            model.ReturnXML = retunXML;
            model.NotificationID = notificationId;
            return model;
        }

        #region public static byte[] GenerateInstrPDF(string outputString)
        public static byte[] GenerateInstrPDF(string outputString)//Generate PDF for print functionality
        {
            int pageCount = GetNoofPages(outputString);

            StringReader sr = new StringReader(outputString.Replace("###Noofpages###", pageCount.ToString()));

            byte[] content = null;
            string savedfile = string.Empty;


            using (MemoryStream myMemoryStream = new MemoryStream())
            {

                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                StyleSheet style = new StyleSheet();
                htmlparser.SetStyleSheet(style);
                PdfWriter myPDFWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();

                htmlparser.Parse(sr);

                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                StyleSheet style1 = new StyleSheet();
                htmlparser1.SetStyleSheet(style1);

                pdfDoc.Close();

                content = AddPageNumbers(myMemoryStream.ToArray());
            }

            return content;
        }

        #region public static byte[] AddPageNumbers(byte[] pdf)
        public static byte[] AddPageNumbers(byte[] pdf)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);

            PdfReader reader = new PdfReader(pdf);   // we create a reader for a certain document

            int n = reader.NumberOfPages;            // we retrieve the total number of pages

            Rectangle psize = reader.GetPageSize(1);  // we retrieve the size of the first page

            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);    // step 1: creation of a document-object

            PdfWriter writer = PdfWriter.GetInstance(document, ms);      // step 2: we create a writer that listens to the document

            document.Open();     // step 3: we open the document

            PdfContentByte cb = writer.DirectContent;  // step 4: we add content

            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;

                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
                cb.AddTemplate(importedPage, 0, 0, false);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Page " + p + " of " + n, 525, 15, 0);
                cb.EndText();
            }
            document.Close();      // step 5: we close the document

            return ms.ToArray();
        }
        #endregion

        public static byte[] GeneratePrintablePDF(string outputString)
        {
            int pageCount = GetNoofPages(outputString);
            StringReader stringReader = new StringReader(outputString.Replace("###Noofpages###", pageCount.ToString()));
            byte[] content = null;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);
                PdfWriter myPDFWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(myPDFWriter, pdfDoc, stringReader);
                pdfDoc.Close();
                content = AddPageNumbers(myMemoryStream.ToArray());
            }

            return content;
        }

        #endregion

        #region public static int GetNoofPages(String sw)
        public static int GetNoofPages(String sw)
        {
            int pageCount = 0;

            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                try
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                    PdfWriter myPDFWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                    pdfDoc.Open();

                    StringReader sr = new StringReader(sw);

                    htmlparser.Parse(sr);

                    sr.Close();
                    pdfDoc.Close();

                    MemoryStream ms = new MemoryStream();
                    ms.Write(myMemoryStream.ToArray(), 0, myMemoryStream.ToArray().Length);
                    PdfReader reader = new PdfReader(myMemoryStream.ToArray());   // we create a reader for a certain document
                    pageCount = reader.NumberOfPages;            // we retrieve the total number of pages
                    reader.Close();
                    return pageCount;
                }
                catch (Exception e)
                {
                    return pageCount;
                }
            }

        }
        #endregion

    }
}