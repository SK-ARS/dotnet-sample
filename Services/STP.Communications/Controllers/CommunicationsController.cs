using STP.Common.Constants;
using STP.Common.Logger;
using STP.Communications.Communication;
using STP.Domain.Communications;
using System;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace STP.Communications.Controllers
{
    public class CommunicationsController : ApiController
    {
        private static string LogInstance = ConfigurationManager.AppSettings["Instance"];
        
        #region Send Fax
        [HttpPost]
        [Route("Communication/SendFax")]
        public IHttpActionResult SendFax(CommunicationParams communicationParams)
        {
            try
            {
                int result = MessageTransmiter.SendFax(communicationParams.ObjectContact, communicationParams.UserInfo, communicationParams.TransmissionId, communicationParams.Content);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- Communication/SendFax, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Send Mail
        [HttpPost]
        [Route("Communication/SendGeneralmail")]
        public IHttpActionResult SendGeneralmail(CommunicationParams communicationParams)
        {
            try
            {
                bool mailStatus = MessageTransmiter.SendGeneralmail(communicationParams.UserEmail, communicationParams.Subject, communicationParams.Content,communicationParams.Attachment, communicationParams.ESDALReference,communicationParams.XMLAttach, communicationParams.DocumentTypeName, communicationParams.IsImminent,communicationParams.DocumentType);
                return Content(HttpStatusCode.OK, mailStatus);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- Communication/SendGeneralmail, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}
