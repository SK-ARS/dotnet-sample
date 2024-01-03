using STP.Common.Logger;
using STP.Domain.Communications;
using STP.Domain.SecurityAndUsers;
using System;
using System.Configuration;
using System.Net.Http;

namespace STP.DocumentsAndContents.ServiceAccess
{
    public class CommunicationService: ICommunicationService
    {
        private readonly HttpClient httpClient;
        public CommunicationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public int SendMail(NotificationContacts objContact, UserInfo userInfo, long TransmId, byte[] content, byte[] attachment, string EsdalReference, int xmlAttach = 0, bool isImminent = false, string docTypeName = null, string replyToEmail = "noreply@esdal2.com")
        {
            int status = 0;
            try
            {
                CommunicationParams communication = new CommunicationParams
                {
                    ObjectContact = objContact,
                    UserInfo = userInfo,
                    TransmissionId= TransmId,
                    Content= content,
                    Attachment= attachment,
                    ESDALReference= EsdalReference,
                    XMLAttach= xmlAttach,
                    IsImminent= isImminent,
                    DocumentTypeName= docTypeName,
                    ReplyToEmail= replyToEmail
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["Communications"]}" +
                                $"/Communication/SendMail", communication).Result;

                if (response.IsSuccessStatusCode)
                {
                    status = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendMail, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return status;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendMail, Exception: {ex}");
                throw;
            }
        }

        public int SendFax(NotificationContacts objContact, UserInfo userInfo, long TransmId, byte[] content)
        {
            int status = 0;
            try
            {
                CommunicationParams communication = new CommunicationParams
                {
                    ObjectContact = objContact,
                    UserInfo = userInfo,
                    TransmissionId = TransmId,
                    Content = content
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["Communications"]}" +
                                $"/Communication/SendFax", communication).Result;

                if (response.IsSuccessStatusCode)
                {
                    status = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendFax, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return status;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Communication/SendFax, Exception: {ex}");
                throw;
            }
        }

    }
}