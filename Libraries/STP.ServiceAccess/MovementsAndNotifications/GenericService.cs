using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public class GenericService : IGenericService
    {
        private readonly HttpClient httpClient;

        public GenericService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region public Object GetQuickLinksList()
        public List<QuickLinks> GetQuickLinksList(int UserId)
        {
            List<QuickLinks> objQuickLinkList = new List<QuickLinks>();
            try
            {
                string urlParameters = "?UserId=" + UserId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Generic/GetQuickLinksList{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objQuickLinkList = response.Content.ReadAsAsync<List<QuickLinks>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Generic/GetQuickLinksList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Generic/GetQuickLinksList, Exception: {ex}");
            }
            return objQuickLinkList;
        }
        #endregion
    }
}
