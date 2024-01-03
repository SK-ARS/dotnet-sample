using STP.Common.Logger;
using STP.Domain;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public class FeedbackService : IFeedbackService
    {
        private readonly HttpClient httpClient;
        public FeedbackService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
       
        public int InsertFeedbackDetails(InsertFeedbackDomain objInsertFeedback)
        {
            int result = 0;
            try
            {
                
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/Contents/InsertFeedbackDetails",objInsertFeedback).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Home/InsertFeedbackDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Home/InsertFeedbackDetails, Exception: {0}", ex));
            }
            return result;

        }

        #region GetFeedBackList
        public List<FeedbackDomain> GetFeedbackSearchInfo(int pageNumber, int pageSize, string searchtype, int flag, string searchString, int sortOrder, int presetFilter)
        {
            List<FeedbackDomain> feedBackList = new List<FeedbackDomain>();
            try
            {
                string urlParameter = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&searchtype=" + searchtype + "&flag=" + flag + "&searchString=" + searchString + "&sortOrder=" + sortOrder + "&presetFilter=" + presetFilter; ;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/FeedBack/GetFeedbackSearchInfo{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    feedBackList = response.Content.ReadAsAsync<List<FeedbackDomain>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"FeedBack/GetFeedbackSearchInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"FeedbackService/GetFeedbackSearchInfo, Exception: {ex}");
            }
            return feedBackList;
        }



        #endregion

        #region  GetFeedbackInfo
        public FeedbackDomain GetFeedbackInfo(long feedBackId, int openChk)
        {
            FeedbackDomain feedBack = new FeedbackDomain();
            try
            {
                string urlParameter = "?feedBackId=" + feedBackId + "&openChk=" + openChk;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/FeedBack/GetFeedbackInfo{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    feedBack = response.Content.ReadAsAsync<FeedbackDomain>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"FeedBack/GetFeedbackInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"FeedbackService/GetFeedbackSearchInfo, Exception: {ex}");
            }
            return feedBack;
        }

        #endregion

        #region Delete FeedBack

        public int DeleteFeedbackDetails(int feedBackId)
        {

            int output = 0;
            try
            {

                //api call to new service

                string urlParameters = "?feedBackId=" + feedBackId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/FeedBack/DeleteFeedbackDetails{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    output = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);

                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"FeedBack/DeleteFeedbackDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"FeedbackService/DeleteFeedbackDetails, Exception: {ex}");

            }
            return output;
        }
        #endregion        
    }
}
