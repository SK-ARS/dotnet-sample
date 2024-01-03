using STP.Common.Logger;
using STP.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Structures;
namespace STP.ServiceAccess.Structures
{
    public class StructureDeligationService : IStructureDeligationService
    {
        private readonly HttpClient httpClient;
        public StructureDeligationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public List<DelegationList> GetDelegArrangList(long organisationId, int pageNumber, int pageSize, string searchType, string searchValue, int presetFilter, int? sortOrder = null)
        {
            List<DelegationList> result = new List<DelegationList>();
            try
            {
               
                string urlParameters = "?organisationId=" + organisationId + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&searchType=" + searchType + "&SearchValue=" + searchValue + "&presetFilter=" + presetFilter + "&sortOrder=" + sortOrder;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                $"/StructureDeligation/GetDelegationArrangements{ urlParameters}").Result;            
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<DelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetDelegationArrangements, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetDelegationArrangements, Exception:{ex}");
            }
            return result;
        }
        public List<DelegationList> GetDelegArrangList(int organisationId, int pageNumber, int pageSize, string arrangName)
        {
            List<DelegationList> result = new List<DelegationList>();
            try
            {
                //api call to new service
                string urlParameters = "?organizationId=" + organisationId + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&arrangementName=" + arrangName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetDelegateArrangementList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<DelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetDelegateArrangementList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetDelegateArrangementList, Exception: {ex}");
            }
            return result;
        }
        public DelegationList GetArrangement(long arrangementId, int organizationId)
        {
            DelegationList result = new DelegationList();
            try
            {
                //api call to new service
                string urlParameters = "?arrangementId=" + arrangementId + "&organizationId=" + organizationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetArrangement{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<DelegationList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetArrangement, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetArrangement, Exception: {ex}");
            }
            return result;
        }
        public List<DelegationList> GetOrganisationList(int pageNumber, int pageSize, string organizationName)
        {
            List<DelegationList> result = new List<DelegationList>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&organizationName=" + organizationName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetOrganisationList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<DelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetOrganisationList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetOrganisationList, Exception: {ex}");
            }
            return result;
        }
        public List<DropDown> GetDelegationAutoFill(int organisationId, string delegationName)
        {
            List<DropDown> result = new List<DropDown>();
            try
            {
                //api call to new service
                string urlParameters = "?organizationId=" + organisationId + "&delegationName=" + delegationName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetDelegationAutoFill{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<DropDown>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetDelegationAutoFill, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetDelegationAutoFill, Exception: {ex}");
            }
            return result;
        }
        public List<StructureInDelegationList> GetStructuresInDeleg(long arrangementId, long organizationId, int? pageNumber, int? pageSize)
        {
            List<StructureInDelegationList> result = new List<StructureInDelegationList>();
            try
            {
                //api call to new service
                string urlParameters = "?arrangementId=" + arrangementId + "&organizationId=" + organizationId + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetStructuresInDelegation{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureInDelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructuresInDeleg, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructuresInDeleg, Exception: {ex}");
            }
            return result;
        }
        public List<DelegationList> GetContactList(int pageNumber, int pageSize, string contactName, int organizationId)
        {
            List<DelegationList> result = new List<DelegationList>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&contactName=" + contactName + "&organizationId=" + organizationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetContactList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<DelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetContactList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetContactList, Exception: {ex}");
            }
            return result;
        }
        public List<RoadDelegationList> GetRoadDelegationList(int pageNumber, int pageSize, long organisationId,int presetFilter, int? sortOrder = null)
        {
            List<RoadDelegationList> result = new List<RoadDelegationList>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&organisationId=" + organisationId + "&presetFilter=" + presetFilter + "&sortOrder=" + sortOrder;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetRoadDelegationList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<RoadDelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetRoadDelegationList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetRoadDelegationList, Exception: {ex}");
            }
            return result;
        }
        public List<StructureInDelegationList> GetStructureInDelegationList(int pageNumber, int? pageSize, string structurecodes, int OrganisationId, int structurecodecount)
        {
            List<StructureInDelegationList> result = new List<StructureInDelegationList>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&structureCodes=" + structurecodes + "&organizationId=" + OrganisationId + "&structurecodeCount=" + structurecodecount;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetStructureInDelegationList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureInDelegationList>>().Result;
                }
                else
                {

                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureInDelegationList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureInDelegationList, Exception: {ex}");
            }
            return result;
        }
        public List<StructureInDelegationList> GetStructureInDelegationList(string[] structurecodes, int OrganisationId)
        {
            List<StructureInDelegationList> result = new List<StructureInDelegationList>();
            try
            {

                string structurecode = Newtonsoft.Json.JsonConvert.SerializeObject(structurecodes);
                string urlParameters = "?structureCodes=" + structurecode + "&organisationId=" + OrganisationId;
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" + $"/StructureDeligation/GetStructureInDeleList{ urlParameters}").Result;                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureInDelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureInDeleList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureInDelegationList, Exception: {ex}");
            }
            return result;
        }
        public bool ManageStructureDelegation(DelegationList delegationList)
        {
            bool result = false;
            try
            {
              
                //api call to new service

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Structures"]}" + $"/StructureDeligation/ManageStructureDelegation", delegationList).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/ManageStructureDelegation, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/ManageStructureDelegation, Exception: {ex}");

            }
            return result;
        }
        public bool ManageDelegationStructureContact(DelegationList delegationList)
        {
            bool result = false;
            try
            {                
                //api call to new service

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Structures"]}" + $"/StructureDeligation/ManageDelegationStructureContact", delegationList).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/ManageDelegationStructureContact, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/ManageDelegationStructureContact, Exception: {ex}");

            }
            return result;
        }
        public int CheckSubDelegationList(long structureID, long organisationID)
        {
            int result = 0;
            try
            {

                //api call to new service

                string urlParameters = "?structureID=" + structureID + "&organisationId=" + organisationID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/CheckSubDelegationList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/CheckSubDelegationList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/CheckSubDelegationList, Exception: {ex}");

            }
            return result;
        }
        public bool DeleteStructureEdit(long arrangementId)
        {
            bool result = false;
            int output = 0;
            try
            {

                //api call to new service

                string urlParameters = "?arrangementId=" + arrangementId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/DeleteStructureEdit{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    output = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                    if (output > 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    result = false;
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructureEdit, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructureEdit, Exception: {ex}");

            }
            return result;
        }
        public List<StructureContactsList> GetStructureContactList(long arrangementId)
        {
            List<StructureContactsList> result = new List<StructureContactsList>();
            try
            {
                //api call to new service
                string urlParameters = "?arrangementId=" + arrangementId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                              $"/StructureDeligation/GetStructureContactList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureContactsList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureContactList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureContactList, Exception: {ex}");
            }
            return result;
        }
        public bool DeleteStructInDelegation(long structId, long arrangId)
        {
            bool result = false;
            int output = 0;
            try
            {

                //api call to new service

                string urlParameters = "?structId=" + structId + "&arrangementId=" + arrangId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/DeleteStructureInDelegation{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    output = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                    if (output > 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructureInDelegation, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructureInDelegation, Exception: {ex}");

            }
            return result;
        }
        public List<DelegationArrangment> viewDelegationArrangment(long organisationId)
        {
            List<DelegationArrangment> result = new List<DelegationArrangment>();
            try
            {
                //api call to new service
                string urlParameters = "?organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/ViewDelegationArrangment{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<DelegationArrangment>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/viewDelegationArrangment, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/viewDelegationArrangment, Exception: {ex}");
            }
            return result;
        }
        public List<StructureSectionList> viewUnsuitableStructSections(long structureId, long route_part_id, long section_id, string cont_ref_num)
        {
            List<StructureSectionList> result = new List<StructureSectionList>();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&routePartId=" + route_part_id + "&sectionId=" + section_id + "&countReferenceNo=" + cont_ref_num;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/ViewUnsuitableStructSections{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureSectionList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/ViewUnsuitableStructSections, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/viewUnsuitableStructSections, Exception: {ex}");
            }
            return result;
        }
        public long GetStructureId(string structureCode)
        {
            int result = -1;
            try
            {

                //api call to new service

                string urlParameters = "?structureCode=" + structureCode;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetStructureId{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureId, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/GetStructureId, Exception: {ex}");

            }
            return result;
        }
        public DelegationList ManageDelegationArrangement(DelegationList savedelegation, long organisationId)
        {
            DelegationList result = new DelegationList();
            try
            {
                //api call to new service
                string url = "?OrganisationId=" + organisationId;        
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Structures"]}" + $"/StructureDeligation/ManageDelegationArrangement" + url, savedelegation).Result;

                if (response.IsSuccessStatusCode)
                {

                    // Parse the response body.
                    result = response.Content.ReadAsAsync<DelegationList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/ManageDelegationArrangement, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/ManageDelegationArrangement, Exception: {ex}");
            }
            return result;
        }
        public bool DeleteStructureContact(short CONTACT_NO, long CautionId)
        {
            bool result = false;
            int output = 0;
            try
            {
                //api call to new service
                string urlParameters = "?contactNO=" + CONTACT_NO + "&cautionId=" + CautionId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/DeleteStructureContact{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                   
                    output = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                    if (output > 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructureContact, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructInDelegation, Exception: {ex}");

            }
            return result;
        }
        public bool DeleteStructureContact(StructureContactsList structContactList)
        {
            bool result = false;
            int output = 0;
            try
            {
                //api call to new service
                //long structureId, string structureCode, long arrangementId, long ownerID

                string urlParameters = "?structureId=" + structContactList.StructureId + "&structureCode=" + structContactList.StructureCode + "&arrangementId=" + structContactList.ArrangementId + "&ownerID=" + structContactList.OwnerId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/DeleteStructureContact{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.

                    output = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                    if (output > 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructureContact, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteStructureContact, Exception: {ex}");

            }
            return result;
        }
        public bool DeleteDelegationArrangement(long arrangId)
        {
            bool result = false;
            int output = 0;
            try
            {
                //api call to new service
                string urlParameters = "?arrangementId=" + arrangId ;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/DeleteDelegationArrangement{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    output = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                    if (output > 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteDelegationArrangement, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligation/DeleteDelegationArrangement, Exception: {ex}");

            }


            return result;
        }

        public List<StructureSummary> GetNotDelegatedStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct, int? sortOrder = null, int? sortType = null)
        {

            List<StructureSummary> summaryObjList = new List<StructureSummary>();
            try
            {
                StructureListParams structureListParams = new StructureListParams
                {
                    OrganisationId = orgId,
                    PageNumber = pageNum,
                    PageSize = pageSize,
                    ObjSearchStructure = objSearchStruct,
                    sortOrder=(int)sortOrder,
                    presetFilter=(int)sortType
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/StructureDeligation/GetNotDelegatedStructureListSearch",
                           structureListParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    summaryObjList = response.Content.ReadAsAsync<List<StructureSummary>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetNotDelegatedStructureListSearch, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetNotDelegatedStructureListSearch, Exception: {0}", ex);
            }
            return summaryObjList;
        }


        public List<StructureDeleArrList> GetStructureDeleArrg(string structureCode)
        {

            List<StructureDeleArrList> objList = new List<StructureDeleArrList>();
            try
            {

                string urlParameters = "?structureCode=" + structureCode;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/StructureDeligation/GetStructureDeleArrg{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objList = response.Content.ReadAsAsync<List<StructureDeleArrList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureDeleArrg, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureDeligationService/GetStructureDeleArrg, Exception: {0}", ex);
            }
            return objList;
        }


    }
}
