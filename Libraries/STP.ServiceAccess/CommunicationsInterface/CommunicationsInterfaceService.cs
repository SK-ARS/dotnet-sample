using STP.Common.General;
using STP.Common.Logger;
using STP.Domain.Communications;
using STP.Domain.SecurityAndUsers;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;

namespace STP.ServiceAccess.CommunicationsInterface
{
    public class CommunicationsInterfaceService : ICommunicationsInterfaceService
    {
        private readonly HttpClient httpClient;
        public CommunicationsInterfaceService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public bool SendFax(NotificationContacts objContact, UserInfo userInfo, long TransmId, byte[] content)
        {
            bool result = false;
            CommunicationParams communicationParams = new CommunicationParams()
            {
                ObjectContact = objContact,
                UserInfo = userInfo,
                TransmissionId = TransmId,
                Content = content
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["Communications"]}" +
                    $"/Communication/SendFax", communicationParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendFax, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendFax, Exception: {ex}");
                throw;
            }
        }
        public bool SendGeneralmail(CommunicationParams communicationParams)
        {
            bool mailStatus = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["Communications"]}" +
                    $"/Communication/SendGeneralmail", communicationParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    mailStatus = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendGeneralmail, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return mailStatus;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendGeneralmail, Exception: {ex}");
                throw;
            }
        }

        #region Send Auto Response Mai
        public bool SendAutoResponseMail(string EsdalReference, string HaulierEmailAddress, string organisationName, MailResponse responseMailDetails)
        {
            string fileType = ".pdf";
            string xmlHtmlString;
            byte[] attachment = new byte[0];
            byte[] content;
            
            //mail subject
            string mailSubject = "Automated response for " + EsdalReference + " from " + organisationName + "";

            //mail content          
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
            xmlHtmlString = XsltTransformer.Trafo(responseMailDetails.ReplyMailText, path, out string errormsg);
            if (responseMailDetails.ReplyMailPdf != "" && responseMailDetails.ReplyMailPdf != null)
            {
                string serverAddress = Convert.ToString(ConfigurationManager.AppSettings["ServerAddress"]);
                string PathWithFileName = responseMailDetails.ReplyMailPdf;
                string FileName = Path.GetFileName(PathWithFileName);
                string pdfPath = serverAddress + "/Account/ViewResponseMailPdf?FileName=" + FileName;
                xmlHtmlString = xmlHtmlString.Replace("</body>", "<p>Please <a href=" + pdfPath + "  >click here</a> to see Automated Response PDF.</p></body>");
            }
            content = Encoding.UTF8.GetBytes(xmlHtmlString);

            string docName = attachment.Length == 0 ? string.Empty : EsdalReference + fileType;
            CommunicationParams communicationParams = new CommunicationParams()
            {
                UserEmail = HaulierEmailAddress,
                Subject = mailSubject,
                Content = content,
                Attachment = attachment,
                ESDALReference = EsdalReference,
                XMLAttach = 1,
                DocumentTypeName = docName,
                IsImminent = false,
                DocumentType = 0
            };
            bool mailStatus = SendGeneralmail(communicationParams);
            return mailStatus;
        }
        #endregion

    }
}
