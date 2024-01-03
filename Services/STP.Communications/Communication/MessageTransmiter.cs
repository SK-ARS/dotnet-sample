using Newtonsoft.Json;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using STP.Common.Enums;

namespace STP.Communications.Communication
{
    public static class MessageTransmiter
    {

        #region SendFax(NotificationContacts objContact, long TransmId, byte[] content)
        /// <summary>
        /// function to send notification fax
        /// </summary>
        /// <param name="objContact"></param>
        /// <param name="TransmId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int SendFax(NotificationContacts objContact, UserInfo userInfo, long TransmId, byte[] content)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("SendFax method initiated for TransmissionId: {0} with Contact Id : {1}", TransmId, objContact.ContactId));

                string output = null;
                string status = null;
                int count = 0;
                int faxStatus = 0;

                var faxObj = new
                {
                    FaxNumber = objContact.Fax,
                    SourceFile = Convert.ToBase64String(content),
                    TypeOfFile = "pdf",
                    TransmissionId = TransmId
                };

                //string jsonString = JavaScriptSerializer
                string jsonString = JsonConvert.SerializeObject(faxObj);

                WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["FaxService"]);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";
                // Connecting to the server. Sending request and receiving response
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        output = streamReader.ReadToEnd();
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Fax server response :{0}", output));
                        output = Regex.Replace(output, @"[^0-9a-zA-Z]+", ",");
                        string[] words = output.Split(',');
                        foreach (string word in words)
                        {
                            count = count + 1;
                            if (word == "StatusCode")
                            {
                                status = words[count];
                            }
                        }

                        faxStatus = Convert.ToInt16(status);
                        if (faxStatus != 1)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Exception occured while sending fax, Exception: {0}", ex));
                return -1;
            }
        }
        #endregion

        #region MailSubjectGenerator
        /// <summary>
        /// Function to generate the subject for outgoing mail.
        /// </summary>
        /// <param name="docTypeName"></param>
        /// <param name="esdalReference"></param>
        /// <param name="docType"></param>
        /// <returns></returns>
        public static string MailSubjectGenerator(string esdalReference, int docType)
        {
            string docTypeName="";
            try
            {
                switch (docType)
                {
                    case (int)ItemDocType.doc001:
                        docTypeName = "Special Order Proposal ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc002:
                        docTypeName = "Special Order Agreement ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc003:
                        docTypeName = "Movement notification alert ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc004:
                        docTypeName = "Daily digest alert ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc005:
                        docTypeName = "Route alert ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc006:
                        docTypeName = "Imminent Move Alert ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc007:
                        docTypeName = "No Longer Affected ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc008:
                        docTypeName = "Failed delegation alert ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc009:
                        docTypeName = "Movement alert ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc010:
                        docTypeName = "Special Order ( " + esdalReference + " ) ";
                        break;
                    case (int)ItemDocType.doc011:
                        docTypeName = "Planned route alert ( " + esdalReference + " ) ";
                        break;
                    default:
                        docTypeName = "Movement notification alert ( " + esdalReference + " ) ";
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("MailSubjectGenerator exception : {0} ", ex.Message));
            }
            return docTypeName;

        }
        #endregion

        #region SendGeneralmail
        public static bool SendGeneralmail(string userEmail, string subject, byte[] content, byte[] attachment, string EsdalReference, int xmlAttach = 0, string docTypeName = null, bool isImminent = false, int docType = 0)
        {
            try
            {
                string fileType = null;
                string FileName = null;

                var ReceiverMailId = ConfigurationManager.AppSettings["Receiver"];
                var IsLiveSystem = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveSystem"]);
                var SenderEmailID = WebConfigurationManager.AppSettings["Sender"];
                var ToMailAdd = !IsLiveSystem ? ReceiverMailId : userEmail ?? ReceiverMailId;
                var CredentialUsername = ConfigurationManager.AppSettings["CredentialUsername"];
                var CredentialPassword = ConfigurationManager.AppSettings["CredentialPassword"];
                var Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                var Host = ConfigurationManager.AppSettings["Host"];
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(SenderEmailID);
                message.To.Add(new MailAddress(ToMailAdd));

                if (subject == "") 
                    message.Subject = isImminent ? "Imminent Move Alert(" + EsdalReference + ")" : MailSubjectGenerator(EsdalReference, docType);
                else
                    message.Subject = subject;

                if (xmlAttach == 1)
                    fileType = ".xml";
                else
                    fileType = ".pdf";

                FileName = attachment.Length == 0 ? string.Empty : EsdalReference + fileType;

                message.IsBodyHtml = true; //to make message body as html
                message.Body = Encoding.UTF8.GetString(content);
                if (attachment.Length > 0 && xmlAttach > 0)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(attachment), FileName));
                }
                smtp.Port = Port;
                smtp.Host = Host; //for gmail host
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(CredentialUsername, CredentialPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SendGeneralmail exception : {0} ", ex.Message));
                return false;
            }
            return true;
        }
        #endregion
    }
}