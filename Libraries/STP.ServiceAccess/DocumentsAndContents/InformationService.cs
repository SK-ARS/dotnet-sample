using Newtonsoft.Json;
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
using System.Web;
namespace STP.ServiceAccess.DocumentsAndContents
{
    public class InformationService : IInformationService
    {
        private readonly HttpClient httpClient;
        public InformationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public List<InformationModel> GetInformationList(int pageNumber, int pageSize, string pageName, string contentType, int userType, int sortOrder, int presetFilter)
        {
            List<InformationModel> objInformationList = new List<InformationModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&pageName=" + pageName + "&contentType=" + contentType + "&userType=" + userType + "&sortOrder=" + sortOrder + "&presetFilter=" + presetFilter;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" + $"/Information/GetInformationList{urlParameters}").Result;
               
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<InformationModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetInformationList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetInformationList, Exception: {ex}");
            }
            return objInformationList;
        }
        #region DeleteInformation
        public int DeleteInformation(int deletedContactId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?deletedContactId=" + deletedContactId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/DeleteInformation{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/DeleteInformation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/DeleteInformation, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region GetEnumValsListByEnumType
        public List<InformationModel> GetEnumValsListByEnumType(string EnumTypeName)
        {
            List<InformationModel> objInformationList = new List<InformationModel>();
           
            try
            {
                string urlParameters = "?EnumTypeName=" + EnumTypeName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/GetEnumValsListByEnumType{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<InformationModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetEnumValsListByEnumType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetEnumValsListByEnumType, Exception: {ex}");
            }
            return objInformationList;
        }
        #endregion
        #region GetAssociatedFilesByContentId
        public List<WebContentFile> GetAssociatedFilesByContentId(int CONTENT_ID)
        {
            List<WebContentFile> objInformationList = new List<WebContentFile>();
            try
            {
                string urlParameters = "?CONTENT_ID=" + CONTENT_ID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/GetAssociatedFilesByContentId{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<WebContentFile>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetAssociatedFilesByContentId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetEnumValsListByEnumType, Exception: {ex}");
            }
            return objInformationList;
        }
        #endregion
        #region GetPortalContentById
        public List<InformationModel> GetPortalContentById(int CONTENT_ID)
        {
            List<InformationModel> objInformationList = new List<InformationModel>();
            try
            {
                string urlParameters = "?CONTENT_ID=" + CONTENT_ID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/GetPortalContentById{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<InformationModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetPortalContentById, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetPortalContentById, Exception: {ex}");
            }
            return objInformationList;
        }
        #endregion
        #region GetInformationById
        public InformationModel GetInformationById(int managedContentId)
        {
            InformationModel objInformation = new InformationModel();
            try
            {
                string urlParameters = "?managedContentId=" + managedContentId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/GetInformationById{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformation = response.Content.ReadAsAsync<InformationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetInformationById, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetInformationById, Exception: {ex}");
            }
            return objInformation;
        }
        #endregion
        #region PublishInformation
        public InformationModel ManageInformation(InformationModel infoModel)
        {
            InformationModel objInfoModel = new InformationModel();
            
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/Information/PublishInformation",
                  infoModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    objInfoModel = response.Content.ReadAsAsync<InformationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/PublishInformation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, Exception: {0}", ex));
            }
            return objInfoModel;
        }
        public bool ManageInformationFiles(InformationModel webContentFile)
        {
            bool status=false;

            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/Information/PublishInformationFiles",
                  webContentFile).Result;
                if (response.IsSuccessStatusCode)
                {
                    status = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/PublishInformationFiles, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformationFiles, Exception: {0}", ex));
            }
            return status;

        }
        #endregion
        #region GetDownloadList
        public List<InformationModel> GetDownloadList(int pageNum, int pageSize, string pageName, string contentType, int userType, int isAdmin, string downloadType)
        {
            List<InformationModel> objInformationList = new List<InformationModel>();
            try
            {
                string urlParameters = "?pageNum=" + pageNum + "&pageSize=" + pageSize + "&pageName=" + pageName + "&contentType=" + contentType + "&userType=" + userType + "&isAdmin=" + isAdmin + "&downloadType=" + downloadType;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/GetDownloadList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<InformationModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetDownloadList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetDownloadList, Exception: {ex}");
            }
            return objInformationList;
        }
        #endregion
        #region GetAllHotNews
        public InformatinDetail GetAllHotNews(long userTypeId)
        {
            InformatinDetail objInformation = new InformatinDetail();
            try
            {
                string urlParameters = "?userTypeId=" + userTypeId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/GetAllHotNews{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformation = response.Content.ReadAsAsync<InformatinDetail>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetAllHotNews, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetAllHotNews, Exception: {ex}");
            }
            return objInformation;
        }
        #endregion

        #region GetUniqueInfoList
        public List<InformationModel> GetUniqueInfoList(int pageNum, int pageSize, int portalid, string SearchType)
        {
            List<InformationModel> objInformationList = new List<InformationModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNum + "&pageSize=" + pageSize + "&portalId=" + portalid + "&searchType=" + SearchType;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Information/GetUniqueInfoList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<InformationModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetUniqueInfoList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetUniqueInfoList, Exception: {ex}");
            }
            return objInformationList;
        }
        #endregion

        #region GetInformationListPortal
        public List<InformationModel> GetInformationListPortal(int pageNumber, int pageSize, string contentType, long userType)
        {
            List<InformationModel> objInformationList = new List<InformationModel>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&contentType=" + contentType + "&userType=" + userType;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" + $"/Information/GetInformationListPortal{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<InformationModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetInformationListPortal, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetInformationListPortal, Exception: {ex}");
            }
            return objInformationList;
        }
        #endregion

        public List<InformatinDetail> GetHotNewsForAdmin(string SearchType)
        {
            List<InformatinDetail> objInformationList = new List<InformatinDetail>();
            try
            {
                string urlParameters = "?SearchType=" + SearchType;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" + $"/Information/GetHotNewsForAdmin{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objInformationList = response.Content.ReadAsAsync<List<InformatinDetail>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetHotNewsForAdmin, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetHotNewsForAdmin, Exception: {ex}");
            }
            return objInformationList;
        }

        public List<LatestNews> GetLatestNews(int portalId, int timeInterval)
        {
            List<LatestNews> objLatestNewsList = new List<LatestNews>();
            try
            {
                string urlParameters = "?portalId=" + portalId + "&timeInterval=" + timeInterval;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" + $"/Information/GetLatestNews{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objLatestNewsList = response.Content.ReadAsAsync<List<LatestNews>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetLatestNews, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Information/GetLatestNews, Exception: {ex}");
            }
            return objLatestNewsList;
        }

    }
}
