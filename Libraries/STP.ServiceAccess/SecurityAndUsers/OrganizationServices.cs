using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace STP.ServiceAccess.SecurityAndUsers
{
     public  class OrganizationServices : IOrganizationService
    {
        private readonly HttpClient httpClient;
        public OrganizationServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region ViewOrganizationByID
        public List<ViewOrganizationByID> ViewOrganizationByID(int orgId)
        {
            List<ViewOrganizationByID> objUserInfo = new List<ViewOrganizationByID>();
            try
            {
                string urlParameters = "?orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/ViewOrganizationByID{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objUserInfo = response.Content.ReadAsAsync<List<ViewOrganizationByID>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/ViewOrganisationByID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/ViewOrganisationByID, Exception: {ex}");
            }
            return objUserInfo;
        }
        #endregion
        #region Save Organization
        public int SaveOrganization(Organization orgDet)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/SaveOrganization", orgDet).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/SaveOrganization, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/SaveOrganization, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region Edit Organization
        public int EditOrganization(Organization orgDet)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/EditOrganization", orgDet).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/EditOrganization, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/EditOrganization, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region GetOrganisationTypeList
        public List<OrganizationTypeList> GetOrganisationTypeList()
        {
            List<OrganizationTypeList> typeList = new List<OrganizationTypeList>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/GetOrganizationTypeList").Result;
                if (response.IsSuccessStatusCode)
                {
                    typeList = response.Content.ReadAsAsync<List<OrganizationTypeList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationTypeList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationTypeList, Exception: {ex}");
            }
            return typeList;
        }
        #endregion
        #region GetOrganizationInformation(string searchString, int pageNumber, int pageSize)
        public List<OrganizationGridList> GetOrganizationInformation(string searchString, int pageNumber, int pageSize, int userTypeId, string searchOrgCode, int sortOrder, int presetFilter)
        {
            List<OrganizationGridList> orgList = new List<OrganizationGridList>();
            try
            {
                string urlParameter = "?searchString=" + searchString + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&userTypeId=" + userTypeId + "&searchOrgCode=" + searchOrgCode + "&sortOrder=" + sortOrder + "&presetFilter=" + presetFilter;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/GetOrganizationInformation{urlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    orgList = response.Content.ReadAsAsync<List<OrganizationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationInformation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationInformation, Exception: {ex}");
            }
            return orgList;
        }
        #endregion
        #region List<OrganizationGridList> GetOrganizationInformation(int pageNumber, int pageSize)
        //Pass parameter 0,0 for list of organisation.
        public List<OrganizationGridList> GetOrganizationInformation(int pageNumber, int pageSize,int userTypeId, int sortOrder, int presetFilter)
        {
            List<OrganizationGridList> orgList = new List<OrganizationGridList>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize+ "&userTypeId="+ userTypeId + "&sortOrder=" + sortOrder + "&presetFilter=" + presetFilter;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/GetOrganizationInformation{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    orgList = response.Content.ReadAsAsync<List<OrganizationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationInformation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationInformation, Exception: {ex}");
            }
            return orgList;
        }
        #endregion

        public decimal GetOrganizationByName(string organisationName, int type, string mode, string organisationId)
        {
            decimal orgCount=0;
            try
            {
                CheckOrganisationExists checkOrganisationExists = new CheckOrganisationExists()
                {
                    OrganisationName = organisationName,
                    Type = type,
                    Mode = mode,
                    OrganisationId = organisationId
                };                 
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/GetOrganizationByName", checkOrganisationExists).Result;
                if (response.IsSuccessStatusCode)
                {
                    orgCount = response.Content.ReadAsAsync<decimal>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationByName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Organization/GetOrganizationByName, Exception: {ex}");
            }
            return orgCount;
        }

        public List<ViewOrganizationByID> ViewOrganisationByIDForSORT(int RevisionId)
        {
            List<ViewOrganizationByID> objUserInfo = new List<ViewOrganizationByID>();
            try
            {
                string urlParameters = "?RevisionId=" + RevisionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
               $"/Organization/ViewOrganisationByIDForSORT{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objUserInfo = response.Content.ReadAsAsync<List<ViewOrganizationByID>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/ViewOrganisationByIDForSORT, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"User/ViewOrganisationByIDForSORT, Exception: {ex}");
            }
            return objUserInfo;
        }

        public List<ContactModel> GetAffectedOrganisationDetails(string affectedParties, int affectedPartiesCount, string userSchema)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            try
            {
                string urlParameters = "?affectedParties=" + Uri.EscapeDataString(affectedParties) + "&affectedPartiesCount=" + affectedPartiesCount + "&userSchema=" + userSchema; 
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                    $"/Organization/GetAffectedOrganisationDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    contacts = response.Content.ReadAsAsync<List<ContactModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Organization/GetAffectedOrganisationDetails, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Organization/GetAffectedOrganisationDetails, Exception:" + ex);
            }
            return contacts;

        }

        public List<ContactModel> GetNenAffectedOrganisationDetails(int inboxItemId)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            try
            {
                string urlParameters = "?inboxItemId=" + inboxItemId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["SecurityAndUsers"]}" +
                    $"/Organization/GetNenAffectedOrganisationDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    contacts = response.Content.ReadAsAsync<List<ContactModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Organization/GetNenAffectedOrganisationDetails, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Organization/GetNenAffectedOrganisationDetails, Exception:" + ex);
            }
            return contacts;

        }
    }
}
