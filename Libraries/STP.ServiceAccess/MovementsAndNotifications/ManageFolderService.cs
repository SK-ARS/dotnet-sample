using STP.Common.Logger;
using STP.Domain.MovementsAndNotifications.Folder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public class ManageFolderService : IManageFolderService
    {
        private readonly HttpClient httpClient;

        public ManageFolderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        //API Method to GetFoldersSearchInfo
        public List<FoldersDomain> GetFoldersSearchInfo(int pageNumber, int pageSize, int organisationId, string searchString)
        {

            List<FoldersDomain> folderObj = new List<FoldersDomain>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&organisationId=" + organisationId + "&searchString=" + searchString;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                                 $"/MovementsFolder/GetFoldersSearchInfo{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    folderObj = response.Content.ReadAsAsync<List<FoldersDomain>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/GetFoldersSearchInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/GetFoldersSearchInfo, Exception: {0}", ex);
            }
            return folderObj;
        }

        //API Method to InsertFolderInfo
        public int InsertFolderInfo(int organisationId, string folderName)
        {

            int result = 0;
            try
            {
                InsertFolderParams insertFolderParams = new InsertFolderParams()
                {
                    OrganisationId = organisationId,
                    FolderName = folderName
                };

                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/MovementsFolder/InsertFolderInfo",
                           insertFolderParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/InsertFolderInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/InsertFolderInfo, Exception: {0}", ex);
            }
            return result;
        }

        //API Method to DeleteFolderInfo
        public int DeleteFolderInfo(int folderId)
        {

            int result = 0;
            try
            {
                //api call to new service
                string urlParameters = "?folderId=" + folderId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                                 $"/MovementsFolder/DeleteFolderInfo{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/DeleteFolderInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/DeleteFolderInfo, Exception: {0}", ex);
            }
            return result;
        }

        //API Method to EditFolderInfo
        public int EditFolderInfo(int folderId, int organisationId, string folderName)
        {

            int result = 0;
            try
            {
                EditFolderParams editFolderParams = new EditFolderParams()
                {
                    FolderId = folderId,
                    OrganisationId = organisationId,
                    FolderName = folderName
                };

                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                           $"/MovementsFolder/EditFolderInfo",
                           editFolderParams).Result;                
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/EditFolderInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/EditFolderInfo, Exception: {0}", ex);
            }
            return result;
        }
    }
}
