using STP.Common.Logger;
using STP.Domain.Routes.QAS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.Routes
{
    public class QasService: IQasService
    {
        private readonly HttpClient httpClient;
        public QasService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public List<AddrDetails> Search(string searchKeyword)
        {
            List<AddrDetails> addrList = new List<AddrDetails>();
            string urlParameters = "?searchKeyword=" + searchKeyword;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                $"/QAS/Search{ urlParameters}").Result;

            if (response.IsSuccessStatusCode)
            {
                addrList = response.Content.ReadAsAsync<List<AddrDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"QAS/Search. StatusCode: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
            }
            return addrList;
        }
        public AddrDetails GetAddress(string moniker)
        {
            AddrDetails addDetails = new AddrDetails();
            string urlParameters = "?moniker=" + moniker;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                $"/QAS/GetAddress{ urlParameters}").Result;

            if (response.IsSuccessStatusCode)
            {
                addDetails = response.Content.ReadAsAsync<AddrDetails>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"QAS/GetAddress. StatusCode: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
            }
            return addDetails;
        }
    }
}
