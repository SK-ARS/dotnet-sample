using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.RoadNetwork.RoadOwnership;
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
   public class StructuresService : IStructuresService
    {
        private readonly HttpClient httpClient;
        public StructuresService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region Public int GetDelegationArrangement()
        public List<DropDown> GetDelegationArrangement(int orgId, string delegationArrangement)
        {
            List<DropDown> dropDowns = new List<DropDown>();
            try
            {
                string urlParameters = "?organizationId=" + orgId + "&delegationArrangement=" + delegationArrangement;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                $"/Structures/GetDelegationArrangementList{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    dropDowns = response.Content.ReadAsAsync<List<DropDown>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructuresService/GetContactDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructuresService/GetContactDetails, Exception: {ex}");
            }
            return dropDowns;
        }
        #endregion
        //API method to GetStructureListSearch
        public List<StructureSummary> GetStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct,int presetFilter)
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
                    presetFilter=presetFilter
                    

                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/GetStructureListSearch",
                           structureListParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    summaryObjList = response.Content.ReadAsAsync<List<StructureSummary>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureListSearch, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureListSearch, Exception: {0}", ex);
            }
            return summaryObjList;
        }
        public int CheckStructureAgainstOrganisation(long structureId, long organisationId)
        {
            int recCnt = 0;
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/Structures/CheckStructureAgainstOrganisation{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    recCnt = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/CheckStructureAgainstOrganisation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/CheckStructureAgainstOrganisation, Exception: {0}", ex);
            }
            return recCnt;
        }
        //Api method to checkStructOrg
        public int CheckStructureOrganisation(int organisationId, long structureId)
        {
            int orgCheck = 0;
            try
            {
                //api call to new service
                string urlParameters = "?organisationId=" + organisationId + "&structureId=" + structureId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/CheckStructureOrganisation{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    orgCheck = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/CheckStructureOrganisation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/CheckStructureOrganisation, Exception: {0}", ex);
            }
            return orgCheck;
        }
        //Api method to ViewGeneralDetails
        public List<StructureGeneralDetails> ViewGeneralDetails(long structureId)
        {
            List<StructureGeneralDetails> objStructureGeneralDetails = new List<StructureGeneralDetails>();
            
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/ViewGeneralDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objStructureGeneralDetails = response.Content.ReadAsAsync<List<StructureGeneralDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewGeneralDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewGeneralDetails, Exception: {0}", ex);
            }
            return objStructureGeneralDetails;
        }
        //Api method to EditStructureGeneralDetails
        public bool EditStructureGeneralDetails(StructureGeneralDetails structureGeneralDetails)
        {
            bool saveFlag = false;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/EditStructureGeneralDetails",
                           structureGeneralDetails).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    saveFlag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/EditStructureGeneralDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/EditStructureGeneralDetails, Exception: {0}", ex);
            }
            return saveFlag;
        }

        //Api method to ViewimposedConstruction
        public ImposedConstraints ViewimposedConstruction(int structureId, int sectionId)
        {
            ImposedConstraints objImpoConstr = new ImposedConstraints();            
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/ViewimposedConstruction{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objImpoConstr = response.Content.ReadAsAsync<ImposedConstraints>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewimposedConstruction, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewimposedConstruction, Exception: {0}", ex);
            }
            return objImpoConstr;
        }

        //Api method to GetEditStructImposed
        public bool GetEditStructureImposed(StuctImposedParams stuctImposedParams)
        {
            bool flag = false;
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/GetEditStructureImposed",
                           stuctImposedParams).Result;                

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    flag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetEditStructImposed, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetEditStructImposed, Exception: {0}", ex);
            }
            return flag;
        }
        //Api method to EditDimensionConstraints
        public bool EditDimensionConstraints(DimensionConstruction objStructureDimension, int structureId, int sectionId)
        {
            bool flag = false;
            EditDimensionParams editDimensionParams = new EditDimensionParams()
            {
                StructureDimension = objStructureDimension,
                StructureId = structureId,
                SectionId = sectionId
            };
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/EditDimensionConstraints",
                           editDimensionParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    flag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/EditDimensionConstraints, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/EditDimensionConstraints, Exception: {0}", ex);
            }
            return flag;
        }
        /// <summary>
        /// Method to fill structure type dropdown list
        /// </summary>
        public List<StructType> GetStructType(int type)
        {
            List<StructType> dropdownObjList = new List<StructType>();

            try
            {
                //api call to new service
                string urlParameters = "?type=" + type;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetStructureType{ urlParameters}").Result;

                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    // Parse the response body.
                    dropdownObjList = response.Content.ReadAsAsync<List<StructType>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureType, Exception: {0}", ex);
            }
            return dropdownObjList;
        }
        /// <summary>
        /// Method to fill structure type dropdown list
        /// </summary>
        public List<StructCategory> GetStructCategory(int type)
        {
            List<StructCategory> dropdownObjList = new List<StructCategory>();
            
            try
            {
                //api call to new service
                String urlParameters = "?type=" + type;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetStructureCategory{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    dropdownObjList = response.Content.ReadAsAsync<List<StructCategory>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureCategory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureCategory, Exception: {0}", ex);
            }
            return dropdownObjList;
        }

        //Api method to ViewSpanDataByNo

        public SpanData ViewSpanDataByNo(long structureId, long sectionId, long? spanNo)
        {

            SpanData objListSpanData = new SpanData();
            try
            {
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId + "&spanNo=" + spanNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                 $"/Structures/ViewSpanDataByNo{urlParameters}").Result;

                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    // Parse the response body.
                    objListSpanData = response.Content.ReadAsAsync<SpanData>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewSpanDataByNo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewSpanDataByNo, Exception: {0}", ex);
            }
            return objListSpanData;
        }

        public List<StucDDList> GetSTRUCT_DD(int TYPE)
        {
            List<StucDDList> objGetStruct = new List<StucDDList>();
            try
            {
                string urlParameters = "?TYPE=" + TYPE;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                               $"/Structures/GetSTRUCT_DD{urlParameters}").Result;

                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    objGetStruct = response.Content.ReadAsAsync<List<StucDDList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetSTRUCT_DD, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetSTRUCT_DD, Exception: {0}", ex);
            }
            return objGetStruct;
        }
        //API Method to ViewDimensionConstruction
        public DimensionConstruction ViewDimensionConstruction(int structureId, int sectionId)
        {
            DimensionConstruction objDimentionConstruction = new DimensionConstruction();
            try
            {

                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                               $"/Structures/ViewDimensionConstruction{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objDimentionConstruction = response.Content.ReadAsAsync<DimensionConstruction>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewDimensionConstruction, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewDimensionConstruction, Exception: {0}", ex);
            }
            return objDimentionConstruction;
        }
        public List<SpanData> ViewSpanData(int structureId, int sectionId)
        {
            List<SpanData> objListSpanData = new List<SpanData>();
            try
            {

                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                               $"/Structures/ViewSpanData{urlParameters}").Result;

                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    objListSpanData = response.Content.ReadAsAsync<List<SpanData>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewSpanData, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewSpanData, Exception: {0}", ex);
            }
            return objListSpanData;
        }
        //API Method to SaveStructureSpan
        public  int SaveStructureSpan(StructureSpanParams SpanParams)
        {
            int saveFlag = 0;
            try
            {
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(SpanParams);
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}"  +
                     $"/Structures/SaveStructureSpan",
                     SpanParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    saveFlag = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/SaveStructureSpan, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/SaveStructureSpan, Exception: {0}", ex);
            }
            return saveFlag;
        }

        //API Method to GetSVData
        public List<SVDataList> GetSVData(long structureId, long sectionId)
        {

            List<SVDataList> objSVDataList = new List<SVDataList>();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetSVData{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objSVDataList = response.Content.ReadAsAsync<List<SVDataList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetSVData, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetSVData, Exception: {0}", ex);
            }
            return objSVDataList;
        }

        //API Method to UpdateSVData
        public List<SVDataList> UpdateSVData(UpdateSVParams objUpdateSVParams)
        {
            List<SVDataList> objSVDataList = new List<SVDataList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                     $"/Structures/UpdateSVData",
                     objUpdateSVParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objSVDataList = response.Content.ReadAsAsync<List<SVDataList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateSVData, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateSVData, Exception: {0}", ex);
            }
            return objSVDataList;
        }

        //API Method to GetHBRatings
        public List<double?> GetHBRatings(long structureId, long sectionId)
        {

            List<double?> lstHBRatings = null;
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetHBRatings{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    lstHBRatings = response.Content.ReadAsAsync<List<double?>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetHBRatings, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetHBRatings, Exception: {0}", ex);
            }
            return lstHBRatings;
        }

        //API Method to GetCalculatedHBToSV
        public List<SvReserveFactors> GetCalculatedHBToSV(long structureId, long sectionId, double? hbWithLoad, double? hbWithoutLoad, int saveFlag, string userName)
        {

            List<SvReserveFactors> lstSvReserveFt = new List<SvReserveFactors>();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId + "&hbWithLoad=" + hbWithLoad + "&hbWithoutLoad=" + hbWithoutLoad + "&saveFlag=" + saveFlag + "&userName=" + userName;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetCalculatedHBToSV{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    lstSvReserveFt = response.Content.ReadAsAsync<List<SvReserveFactors>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetCalculatedHBToSV, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetCalculatedHBToSV, Exception: {0}", ex);
            }
            return lstSvReserveFt;
        }

        //API call for GetCautionList
        public List<StructureModel>  GetCautionList(CautionListParams cautionListParams )
        {

            List<StructureModel> structureList = new List<StructureModel>();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/GetCautionList",
                           cautionListParams).Result;

                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    // Parse the response body.
                    structureList = response.Content.ReadAsAsync<List<StructureModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetCautionList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetCautionList, Exception: {0}", ex);
            }
            return structureList;
        }
        //API call for GetCautionDetails
        public StructureModel GetCautionDetails(long cautionID)
        {

            StructureModel StructuremodelBase = new StructureModel();
            try
            {
                string urlParameters = "?cautionID=" + cautionID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetCautionDetails{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    StructuremodelBase = response.Content.ReadAsAsync<StructureModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetCautionDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetCautionDetails, Exception: {0}", ex);
            }
            return StructuremodelBase;
        }

        #region List<Object> GetRoadDelegationList()
        public List<RoadDelegationList> GetRoadDelegationList(int pageNum, int pageSize, long orgId)
        {
            List<RoadDelegationList> objRoadDelegationLst = new List<RoadDelegationList>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNum + "&pageSize=" + pageSize + "&organizationId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync(
                    $"{ConfigurationManager.AppSettings["Structures"]}" +
                    $"/Structures/GetRoadDelegationList" +
                    urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    objRoadDelegationLst = response.Content.ReadAsAsync<List<RoadDelegationList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetRoadDelegationList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/GetRoadDelegationList, Exception: {0}", ex));
            }
            return objRoadDelegationLst;
        }
        #endregion

        //API call for SaveCautions
        public bool SaveCautions(StructureModel Structuremodelalter)
        {

            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["Structures"]}" +
                          $"/Structures/SaveCautions",
                          Structuremodelalter).Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/SaveCautions, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/SaveCautions, Exception: {0}", ex);
            }
            return result;
        }
        public long DeleteCaution(long cautionId, string userName)
        {
            long result = 1;
            try
            {
                string urlParameters = "?cautionId=" + cautionId + "&userName=" + userName;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/DeleteCaution{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/DeleteCaution, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/DeleteCaution, Exception: {0}", ex);
            }

            return result;
        }
        //Api method to UpdateStructureLog
        public bool UpdateStructureLog(List<StructureLogModel> structureLogsModel)
        {
            bool result = false;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/UpdateStructureLog",
                           structureLogsModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateStructureLog, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateStructureLog, Exception: {0}", ex);
            }
            return result;
        }
        //Api method to GetStructureHistory
        public List<StructureModel> GetStructureHistory(int pageNumber, int pageSize, long StructureID)
        {
            List<StructureModel> lstStructureModel = new List<StructureModel>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&structureID=" + StructureID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetStructureHistory{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    lstStructureModel = response.Content.ReadAsAsync<List<StructureModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureHistory, Exception: {0}", ex);
            }
            return lstStructureModel;
        }


        //Api method to GetStructureHistory by id
        public List<StructureHistoryList> GetStructureHistoryById(int pageNumber, int pageSize, long StructureID)
        {
            List<StructureHistoryList> lstStructureModel = new List<StructureHistoryList>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&structureID=" + StructureID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetStructureHistoryById{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    lstStructureModel = response.Content.ReadAsAsync<List<StructureHistoryList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureHistory, Exception: {0}", ex);
            }
            return lstStructureModel;
        }



        //Api method to GetStructureHistory count
        public int GetStructureHistoryCount(long StructureID)
        {
            int lstStructureModel=0;
            try
            {
                //api call to new service
                string urlParameters = "?structureID=" + StructureID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetStructureHistoryCount{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    lstStructureModel = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureHistory, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureHistory, Exception: {0}", ex);
            }
            return lstStructureModel;
        }
        public bool SaveStructureContact(StructureContactModel constraintContact)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                            $"{ConfigurationManager.AppSettings["Structures"]}" +
                            $"/Structures/SaveStructureContact",
                            constraintContact).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;

                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/SaveStructureContact, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/SaveStructureContact, Exception: {ex}");
                throw;
            }
        }
        public bool DeleteStructureContact(short contactNo, long cautionId)
        {
            bool result = false;
            int output = 0;
            try
            {
                //api call to new service
                string urlParameters = "?contactNo=" + contactNo + "?cautionId=" + cautionId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                              $"/Structures/DeleteStructureContact{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)

                {
                    // Parse the response body.
                    output = response.Content.ReadAsAsync<int>().Result;
                    if (output > 0)
                    {
                        result = true;
                    }

                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/DeleteStructureContact, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/DeleteStructureContact, Exception: {ex}");
            }
            return result;
        }

        #region ----------------------------Susmitha------------------------
        public List<StructureContactModel> GetStructureContactList(int pageNumber, int pageSize, long CautionID, short ContactNo = 0)
        {
            List<StructureContactModel> result = new List<StructureContactModel>();
            try
            {
                //api call to new service
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&CautionID=" + CautionID + "&ContactNo=" + ContactNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                              $"/Structures/GetStructureContactList{ urlParameters}").Result;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureContactModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureContactList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureContactList, Exception: {ex}");
            }
            return result;
        }
        public List<StructureSectionList> ViewStructureSections(long structureId)
        {
            List<StructureSectionList> result = new List<StructureSectionList>();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                              $"/Structures/ViewStructureSections{ urlParameters}").Result;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureSectionList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewStructureSections, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewStructureSections, Exception: {ex}");
            }
            return result;
        }
        public ManageStructureICA ViewEnabledICA(int structureId, int sectionId, long OrgID)
        {
            ManageStructureICA result = new ManageStructureICA();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId + "&organisationId=" + OrgID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                              $"/Structures/ViewEnabledICA{ urlParameters}").Result;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<ManageStructureICA>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewEnabledICA, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/ViewEnabledICA, Exception: {ex}");
            }
            return result;
        }
        public List<SvReserveFactors> ViewSVData(int structureId, int sectionId)
        {
            List<SvReserveFactors> result = new List<SvReserveFactors>();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                              $"/Structures/viewSVData{ urlParameters}").Result;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<SvReserveFactors>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/viewSVData, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/viewSVData, Exception: {ex}");
            }
            return result;
        }

        #endregion

        //Api method to StructureInfoList
        public List<StructureInfo> MyStructureInfoList(int organisationId, int otherOrganisation, int left, int right, int bottom, int top)
        {
            List<StructureInfo> structureInfoList = new List<StructureInfo>();
            try
            {
                //api call to new service
                string urlParameters = "?organisationId=" + organisationId + "&otherOrganisation=" + otherOrganisation + "&left=" + left + "&right=" + right + "&bottom=" + bottom + "&top=" + top;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/StructureInfoList{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structureInfoList = response.Content.ReadAsAsync<List<StructureInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/StructureInfoList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/StructureInfoList, Exception: {0}", ex);
            }
            return structureInfoList;
        }

        //Api method to GetStructureContactListInfo
        public List<StructureContact> GetStructureContactListInfo(long structureId, string userSchema = UserSchema.Portal)
        {
            List<StructureContact> StructureContactList = new List<StructureContact>();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetStructureContactList{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    StructureContactList = response.Content.ReadAsAsync<List<StructureContact>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureContactList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureContactList, Exception: {0}", ex);
            }
            return StructureContactList;
        }

        //Api method to getRoadContactList
        public List<RoadContactModal> GetRoadContactList(long linkId, long length, string userSchema = UserSchema.Portal)
        {
            List<RoadContactModal> RoadContactList = new List<RoadContactModal>();
            try
            {
                //api call to new service
                string urlParameters = "?linkId=" + linkId + "&length=" + length + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetRoadContactList{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    RoadContactList = response.Content.ReadAsAsync<List<RoadContactModal>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetRoadContactList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetRoadContactList, Exception: {0}", ex);
            }
            return RoadContactList;
        }


        public List<StructureInfo> AgreedAppStructureInfo(string StructureCode)
        {
            List<StructureInfo> result = new List<StructureInfo>();
            try
            {
                //api call to new service
                string urlParameters = "?StructureCode=" + StructureCode ;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/AgreedAppStructureInfo{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureInfo>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/AgreedAppStructureInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/AgreedAppStructureInfo, Exception: {0}", ex);
            }
            return result;
        }
        #region GetAffectedParties
        public byte[] GetAffectedParties(int NotificationId, string userSchema = UserSchema.Portal)
        {
            byte[] result = null;
            try
            {
                //api call to new service
                string urlParameters = "?NotificationId=" + NotificationId+ "&userSchema="+ userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetAffectedParties{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetAffectedParties, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetAffectedParties, Exception: {0}", ex);
            }
            return result;
        }
        #endregion

        public List<StructureNotification> GetAllStructureNotification(string structureId, int pageNumber, int pageSize)
        {
            List<StructureNotification> StructureContactList = new List<StructureNotification>();
            try
            {
                //api call to new service
                string urlParameters = "?structureId=" + structureId + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetAllStructureNotification{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    StructureContactList = response.Content.ReadAsAsync<List<StructureNotification>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetAllStructureNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetAllStructureNotification, Exception: {0}", ex);
            }
            return StructureContactList;
        }

        public ManageStructureICA GetManageICAUsage(int orgId, int structureId, int sectionId)
        {
            ManageStructureICA result = new ManageStructureICA();
            try
            {
                //api call to new service
                string urlParameters = "?organisationId=" + orgId + "&structureId=" + structureId + "&sectionId=" + sectionId; 
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetManageICAUsage{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<ManageStructureICA>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetManageICAUsage, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetManageICAUsage, Exception: {0}", ex);
            }
            return result;
        }

        public string GetICAVehicleResult(ICAVehicleResult ICAVehicleParams)
        {

            string result = string.Empty;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["Structures"]}" +
                    $"/Structures/GetICAVehicleResult",
                    ICAVehicleParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetICAVehicleResult, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetICAVehicleResult, Exception: {0}", ex);
            }
            return result;
        }
        public ConfigBandModel GetDefaultBanding(int organisationId, int structureId, int sectionId)
        {
            ConfigBandModel result = new ConfigBandModel();
            try
            {
                //api call to new service
                string urlParameters = "?organisationId=" + organisationId + "&structureId=" + structureId + "&sectionId=" + sectionId; 
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/GetDefaultBanding{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<ConfigBandModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetDefaultBanding, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetDefaultBanding, Exception: {0}", ex);
            }
            return result;
        }

        public bool UpdateStructureICAUsage(UpdateICAUsageParams ICAUsageparams)
        {
            bool result = false;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/UpdateStructureICAUsage",
                           ICAUsageparams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateStructureICAUsage, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateStructureICAUsage, Exception: {0}", ex);
            }
            return result;
        }

        public bool UpdateDefaultBanding(SaveDefaultConfigParams configparams)
        {
            bool result = false;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/UpdateDefaultBanding",
                           configparams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateDefaultBanding, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/UpdateDefaultBanding, Exception: {0}", ex);
            }
            return result;
        }
        public int DeleteStructureSpan(long structureId, long sectionId, long spanNo, string userName)
        {
            int result = 0;
            try
            {
                string urlParameters = "?structureId=" + structureId + "&sectionId=" + sectionId + "&spanNo=" + spanNo + "&userName=" + userName;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                 $"/Structures/DeleteStructureSpan{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/DeleteStructureSpan, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/DeleteStructureSpan, Exception: {0}", ex);
            }

            return result;
        }

        public List<StructureNotification> GetAllStructureOnerousVehicles(string structureId, int pageNumber, int pageSize, string searchCriteria, string searchStatus, DateTime? startDate, DateTime? endDate, int statusCount, int sort,long organisationId)
        {
            List<StructureNotification> result = new List<StructureNotification>();
            try
            {
                if (startDate != null)
                {
                    startDate = startDate.Value;
                }
                if (endDate != null)
                {
                    endDate = endDate.Value;
                }
                OnerousVehicleListParams onerousVehicleListParams = new OnerousVehicleListParams()
                {
                    StructureId = structureId,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchCriteria = searchCriteria,
                    SearchStatus = searchStatus,
                    StartDate= startDate,
                    EndDate= endDate,
                    StatusCount = statusCount,
                    Sort = sort,
                    OrganisationId = organisationId
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["Structures"]}" +
                           $"/Structures/GetAllStructureOnerousVehicles",
                           onerousVehicleListParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<StructureNotification>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetAllStructureOnerousVehicles, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetAllStructureOnerousVehicles, Exception: {0}", ex);
            }

            return result;
        }

        public int GetStructureOwner(long structId, long orgId)
        {
            int recCnt = 0;
            try
            {
                //api call to new service
                string urlParameters = "?structId=" + structId + "&orgId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/Structures/GetStructureOwner{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    recCnt = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureOwner, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureOwner, Exception: {0}", ex);
            }
            return recCnt;
        }
        #region GetStructureId
        public int GetStructureId(string structurecode)
        {
            int StructureId = 0;
            try
            {
                //api call to new service
                string urlParameters = "?structurecode=" + structurecode;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Structures"]}" +
                                                                  $"/Structures/GetStructureId{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    StructureId = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Structures//GetStructureId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"/Structures//GetStructureId, Exception: {ex}");
            }
            return StructureId;
        }
        #endregion




    }
}
