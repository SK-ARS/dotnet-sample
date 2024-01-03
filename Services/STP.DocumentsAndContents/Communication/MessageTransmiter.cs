using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using STP.Common.Logger;
using System;
using System.IO;

namespace STP.DocumentsAndContents.Communication
{
    public class MessageTransmiter
    {
        public MessageTransmiter()
        {
            
        }
        
        public static byte[] GenerateSOPDF(string outputString)
        {
            byte[] content = null;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "GenerateSOPDF method initiated.");
                int pageCount = GetNoofPages(outputString);

                StringReader sr = new StringReader(outputString.Replace("###Noofpages###", pageCount.ToString()));


                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                    StyleSheet style = new StyleSheet();
                    htmlparser.SetStyleSheet(style);

                    pdfDoc.Open();

                    htmlparser.Parse(sr);

                    HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                    StyleSheet style1 = new StyleSheet();
                    htmlparser1.SetStyleSheet(style1);

                    pdfDoc.Close();

                    content = AddPageNumbers(myMemoryStream.ToArray());

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("GenerateSOPDF method exception : {0} ", ex.Message));
            }
            return content;

        }
        public static int GetNoofPages(String sw)
        {
            try
            {

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "GetNoofPages method initiated.");
                int pageCount = 0;
                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    try
                    {

                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 40f, 10f, 40f, 36);

                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                        pdfDoc.Open();
                        StringReader sr = new StringReader(sw);

                        htmlparser.Parse(sr);
                        sr.Close();
                        pdfDoc.Close();
                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("itextsharp Using statement exception : {0} ", e.Message));
                        return 0;
                    }

                    MemoryStream ms = new MemoryStream();
                    ms.Write(myMemoryStream.ToArray(), 0, myMemoryStream.ToArray().Length);
                    PdfReader reader = new PdfReader(myMemoryStream.ToArray());   // we create a reader for a certain document
                    pageCount = reader.NumberOfPages;            // we retrieve the total number of pages
                    reader.Close();
                    return pageCount;

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("GetNoofPages exception : {0} ", ex.Message));
                return 0;
            }

        }
        public static byte[] AddPageNumbers(byte[] pdf)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "AddPageNumbers method initiated.");
                
                ms.Write(pdf, 0, pdf.Length);

                PdfReader reader = new PdfReader(pdf);   // we create a reader for a certain document

                int n = reader.NumberOfPages;            // we retrieve the total number of pages

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

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("AddPageNumbers exception : {0} ", ex.Message));
            }
            return ms.ToArray();
        }

        #region Commented Code By Mahzeer on 12/07/2023
        /*
        public static void InsertTransmissionInfoToAction(NotificationContacts objcontact, UserInfo userInfo, long transmissionId, string esdalRef, int actionFlag, string errMessage = "", string docType = "",long projectid=0,int revisionNo=0,int versionNo=0)
        {
            try
            {
                movactiontype = new MovementActionIdentifiers();
                switch (actionFlag)
                {
                    case 1: //499053	499	transmission ready for delivery
                        movactiontype.MovementActionType = MovementnActionType.transmission_ready_for_delivery;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.OrganisationNameReceiver = objcontact.OrganistationName;
                        break;

                    case 2: //499051	499	transmission delivered
                        movactiontype.MovementActionType = MovementnActionType.transmission_delivered;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.OrganisationNameReceiver = objcontact.OrganistationName;
                        movactiontype.ESDALRef = esdalRef;
                        movactiontype.DateTime = DateTime.Today;
                        break;

                    case 3: //499052	499	transmission delivery failure
                        movactiontype.MovementActionType = MovementnActionType.transmission_delivery_failure;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.TransmissionErrorMsg = errMessage;
                        movactiontype.OrganisationNameReceiver = objcontact.OrganistationName;
                        movactiontype.DateTime = DateTime.Today;
                        break;

                    case 4: //499058	499	transmission forwarded
                        movactiontype.MovementActionType = MovementnActionType.transmission_forwarded;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.ContactPreference = objcontact.ContactPreference;
                        movactiontype.DateTime = DateTime.Today;
                        break;

                    case 5: //499063	499	transmission sent
                        movactiontype.MovementActionType = MovementnActionType.transmission_sent;
                        movactiontype.TransmissionId = transmissionId;
                        movactiontype.TransmissionDocType = docType;
                        movactiontype.ContactPreference = objcontact.ContactPreference;
                        break;


                }
                OutBoundDocumentDOA.GenerateMovementAction(userInfo, esdalRef, movactiontype,projectid,revisionNo,versionNo);
                #region-------------- Release 2 added sys_events loggs for Retransmit
                if (movactiontype.MovementActionType == MovementnActionType.transmission_delivered)
                {
                    movactiontype.SystemEventType = SysEventType.Retransmitted_document;
                    movactiontype.UserId = Convert.ToInt32(userInfo.UserId);
                    movactiontype.UserName = userInfo.UserName;
                    movactiontype.ESDALRef = esdalRef;
                    string sysEventDescp = System_Events.GetSysEventString(userInfo, movactiontype, out errMessage);
                    int user_ID = Convert.ToInt32(userInfo.UserId);
                    OutBoundDocumentDOA.SaveSysEvents(movactiontype, sysEventDescp, user_ID, userInfo.UserSchema);
                }
                else if (movactiontype.MovementActionType == MovementnActionType.transmission_delivery_failure)
                {
                    movactiontype.SystemEventType = SysEventType.Retransmit_failed_document;
                    movactiontype.UserId = Convert.ToInt32(userInfo.UserId);
                    movactiontype.UserName = userInfo.UserName;
                    movactiontype.ESDALRef = esdalRef;
                    string sysEventDescp = System_Events.GetSysEventString(userInfo, movactiontype, out errMessage);
                    int user_ID = Convert.ToInt32(userInfo.UserId);
                    OutBoundDocumentDOA.SaveSysEvents(movactiontype, sysEventDescp, user_ID, userInfo.UserSchema);
                }
                #endregion
            }
            catch
            {
                //do nothing
            }
        }
        
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
            { return null; }
            else
            { return input.First().ToString().ToUpper() + input.Substring(1); }
        }
        public static byte[] TransmitNotification(NotificationContacts objContact, UserInfo userInfo, string EsdalReference, string notifHtmlString, long transmissionId = 0, bool indemnity = false, byte[] attachXml = null, bool IsImminent = false, int docType = 0, long projectid = 0, int revisionNo = 0, int versionNo = 0)
        {
            byte[] content = null;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("TransmitNotification method initiated for TransmissionId: {0} with Contact Id : {1} and EsdalRef : {2}", transmissionId, objContact.ContactId, EsdalReference));


                byte[] attachment = new byte[0];
                int xmlattach = 0;

                if (attachXml != null)
                {
                    attachment = attachXml;
                    xmlattach = 1;
                }

                string docTypeName = null;

                if (docType != 0)
                {
                    if (docType == 322001)
                    { docTypeName = "proposal"; }
                    else if (docType == 322002)
                    { docTypeName = "agreement"; }
                    else if (docType == 322003)
                    { docTypeName = "notification"; }
                    else if (docType == 322004)
                    { docTypeName = "daily digest"; }
                    else if (docType == 322005)
                    { docTypeName = "route alert"; }
                    else if (docType == 322006)
                    { docTypeName = "imminent move alert"; }
                    else if (docType == 322007)
                    { docTypeName = "no longer affected"; }
                    else if (docType == 322008)
                    { docTypeName = "failed delegation alert"; }
                    else if (docType == 322009)
                    { docTypeName = "movement details"; }
                    else if (docType == 322010)
                    { docTypeName = "special order"; }
                    else if (docType == 322011)
                    { docTypeName = "vr1 planned route"; }
                }

                string tmpFullName = objContact.ContactName;
                string tmpOrgName = objContact.OrganistationName;
                string tmpEmailAddr = objContact.Email;
                string tmpFaxNumbr = objContact.Fax;
                string[] tmpStringSeparator = new string[] { "##**##" };

                //splitting full name
                if (tmpFullName != null && tmpFullName != string.Empty && tmpFullName.IndexOf("##**##") != -1)
                {
                    string[] reasonArray = tmpFullName.Split(tmpStringSeparator, StringSplitOptions.None);

                    if (reasonArray.Length > 1)
                    {
                        objContact.ContactName = reasonArray[1];
                    }
                }
                else if (tmpFullName != null && tmpFullName != string.Empty && tmpFullName.IndexOf("##**##") == -1)
                {
                    objContact.ContactName = tmpFullName;
                }


                //splitting organisation name
                if (tmpOrgName != null && tmpOrgName != string.Empty && tmpOrgName.IndexOf("##**##") != -1)
                {
                    string[] reasonArray = tmpOrgName.Split(tmpStringSeparator, StringSplitOptions.None);

                    if (reasonArray.Length > 1)
                    {
                        objContact.OrganistationName = reasonArray[1];
                    }
                }
                else if (tmpOrgName != null && tmpOrgName != string.Empty && tmpOrgName.IndexOf("##**##") == -1)
                {
                    objContact.OrganistationName = tmpOrgName;
                }

                //splitting email address
                if (tmpEmailAddr != null && tmpEmailAddr != string.Empty && tmpEmailAddr.IndexOf("##**##") != -1)
                {
                    string[] reasonArray = tmpEmailAddr.Split(tmpStringSeparator, StringSplitOptions.None);

                    if (reasonArray.Length > 1)
                    {
                        objContact.Email = reasonArray[1];
                    }
                }
                else if (tmpEmailAddr != null && tmpEmailAddr != string.Empty && tmpEmailAddr.IndexOf("##**##") == -1)
                {
                    objContact.Email = tmpEmailAddr;
                }

                //splitting Fax address
                if (tmpFaxNumbr != null && tmpFaxNumbr != string.Empty && tmpFaxNumbr.IndexOf("##**##") != -1)
                {
                    string[] reasonArray = tmpFaxNumbr.Split(tmpStringSeparator, StringSplitOptions.None);

                    if (reasonArray.Length > 1)
                    {
                        objContact.Fax = reasonArray[1];
                    }
                }
                else if (tmpFaxNumbr != null && tmpFaxNumbr != string.Empty && tmpFaxNumbr.IndexOf("##**##") == -1)
                {
                    objContact.Fax = tmpFaxNumbr;
                }

                if (objContact.ContactPreference == ContactPreference.fax) // Business logic to generate PDF will be done in below code snippet.
                {

                    objContact.Email = null; //only Fax needs to be saved in active transactions table for Fax preference

                    if (!indemnity)
                    {
                        transmissionId = DocumentTransmissionDAO.SaveDistributionStatus(objContact, 0, 0, EsdalReference, transmissionId, IsImminent);
                    }

                    if (transmissionId != 0)
                    {
                        InsertTransmissionInfoToAction(objContact, userInfo, transmissionId, EsdalReference, 1, "", "Fax", projectid, revisionNo, versionNo);
                    }
                    content = GenerateSOPDF(notifHtmlString);

                }
                else if (objContact.ContactPreference == ContactPreference.onlineInboxOnly) // Business logic to generate word document will be done in below code snippet.
                {
                    //setting both null in case its an inbox only preference.
                    objContact.Fax = null;
                    objContact.Email = null;

                }
                else if (objContact.ContactPreference == ContactPreference.emailHtml) // Business logic to generate HTML will be done in below code snippet.
                {
                    objContact.Fax = null; //only email field needs to be saved in active transactions table for email preference

                    content = Encoding.UTF8.GetBytes(notifHtmlString); // Instead of ASCII, changed it to UTF8                    

                }
                else if (objContact.ContactPreference == ContactPreference.OnlineInboxPlusEmail) // Business logic to generate HTML will be done in below code snippet.
                {
                    objContact.Fax = null; //only email field needs to be saved in active transactions table for email preference

                    content = Encoding.UTF8.GetBytes(notifHtmlString); // Instead of ASCII, changed it to UTF8

                }
                //return true;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("TransmitNotification method exception : {0} ", ex.Message));
                //return false;
            }
            return content;
        }
        */
        #endregion
    }
}