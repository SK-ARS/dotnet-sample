using STP.Common.Logger;
using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.DocumentsAndContents
{
   public class ContactService:IContactService
    {
        private readonly HttpClient httpClient;
        public ContactService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
       public List<ContactListModel> GetContactListSearch(int orgId, int pageNum, int pageSize, int searchCriteria, string searchValue, int sortFlag, int presetFilter, int? sortOrder = null)
        {
            string contactDomain = "?organizationId=" + orgId + "&pageNumber=" + pageNum + "&pageSize=" + pageSize + "&searchCriteria=" + searchCriteria + "&searchValue=" + searchValue + "&sortFlag="+ sortFlag + "&presetFilter=" + presetFilter + "&sortOrder=" + sortOrder;
            HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                    $"/Contents/ContactList" +
                    contactDomain).Result;

            //api call to new service
            List<ContactListModel> contactInfo = new List<ContactListModel>();

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                contactInfo = response.Content.ReadAsAsync<List<ContactListModel>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ContactService/GetContactListSearch, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return contactInfo;
        }
        public List<HaulierContactModel> GetHaulierContactList(int orgId, int pageNum, int pageSize, string searchCriteria, string searchValue, int presetFilter, int? sortOrder = null)
        {
            string haulierContactDomain = "?organizationId=" + orgId + "&pageNumber=" + pageNum + "&pageSize=" + pageSize + "&searchCriteria=" + searchCriteria + "&searchValue=" + searchValue + "&presetFilter=" + presetFilter + "&sortOrder=" + sortOrder;
            HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                    $"/Contents/AddressList" +
                    haulierContactDomain).Result;

            //api call to new service
            List<HaulierContactModel> contactInfo = new List<HaulierContactModel>();

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                contactInfo = response.Content.ReadAsAsync<List<HaulierContactModel>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ContactService/GetHaulierContactList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");


            }

            return contactInfo;
        }
        public HaulierContactModel GetHaulierContactById(double haulierContactId)
        {
            HaulierContactModel objHauliercontactModel = new HaulierContactModel();
            try
            {
                string urlParameters = "?haulierContactId=" + haulierContactId;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Contents/GetHaulierContactById{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objHauliercontactModel = response.Content.ReadAsAsync<HaulierContactModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ContactService/GetHaulierContactById, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ContactService/GetHaulierContactById, Exception: {ex}");
            }
            return objHauliercontactModel;
        }
        public bool ManageHaulierContact(HaulierContactModel haulierContactModel)
        {
            bool result = false;
            try
            {

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/Contents/ManageHaulierContact",haulierContactModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/ManageHaulierContact, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Contents/ManageHaulierContact, Exception: {0}", ex));
            }
            return result;
           }
        public int DeleteHaulierContact(double haulierContactId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?haulierContactId=" + haulierContactId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Contents/DeleteHaulierContact{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contact/DeleteHaulierContact, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contact/DeleteHaulierContact, Exception: {ex}");
            }
            return result;
        }

    }
}
