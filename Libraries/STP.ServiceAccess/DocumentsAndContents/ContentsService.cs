using STP.Common.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.DocumentsAndContents
{
  public class ContentsService:IContentsService
    {
        private readonly HttpClient httpClient;
        public ContentsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public int ManageFavourites(int categoryId, int categoryType, int isFavourite)
        {
            int result = 0;
            try
            {
                
                string urlParams = "?categoryId=" + categoryId + "&categoryType=" + categoryType + "&isFavourite=" + isFavourite;
                HttpResponseMessage response = httpClient.GetAsync(
                        $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                        $"/Contents/ManageFavourites" +
                        urlParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ContentsService/ManageFavourites, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ContentsService/ManageFavourites, Exception: {ex}");
            }
            return result;
        }
    }
}
