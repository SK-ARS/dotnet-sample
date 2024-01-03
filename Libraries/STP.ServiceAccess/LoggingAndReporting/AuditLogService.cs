using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NetSdoGeometry;
using Newtonsoft.Json;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.LoggingAndReporting;

namespace STP.ServiceAccess.LoggingAndReporting
{
    public class AuditLogService : IAuditLogService
    {
        private readonly HttpClient httpClient;
        public AuditLogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
      
        public long  SaveNotifAuditLog(AuditLogIdentifiers auditLogType, string logMsg, int User_ID, long Org_ID = 0)
        {
            long result = 0;            
            try
            {
                NENAuditLogInputParams inputParams = new NENAuditLogInputParams()
                {
                   AuditLogIdentifiers = auditLogType,
                   LogMsg = logMsg,
                   OrganisationId = Org_ID,
                   UserId= User_ID
                };
                //api call to new service             
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["LoggingAndReporting"]}" + $"/AuditLog/SaveNotificationAuditLog", inputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result =  response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuditLog/SaveNotificationAuditLog, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"AuditLog/SaveNotificationAuditLog, Exception: {0}", ex);
            }
            return result;
        }
       
    }
}
