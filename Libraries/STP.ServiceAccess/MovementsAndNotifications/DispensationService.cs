using STP.Common.Logger;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public class DispensationService:IDispensationService
    {
        private readonly HttpClient httpClient;

        public DispensationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region GetAffDispensationInfo
      public  List<DispensationGridList> GetAffDispensationInfo(int organisationId, int Grantee_ID, int pageNum, int pageSize, int usertype)
        {
            List<DispensationGridList> objAffDispensionInfo = new List<DispensationGridList>();
            try
            {
                string urlParameters = "?organisationId=" + organisationId + "&granteeId=" + Grantee_ID + "&pageNumber=" + pageNum + "&pageSize=" + pageSize + "&userType=" + usertype;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetAffDispensationInfo{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    objAffDispensionInfo = response.Content.ReadAsAsync<List<DispensationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetAffDispensationInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetAffDispensationInfo, Exception: {ex}");
            }
            return objAffDispensionInfo;
        }
        #endregion

        #region GetSummaryListCount
        public int GetSummaryListCount(int orgId, int usertype)
        {
            int result = 0;
            try
            {
                string urlParameters = "?organisationId=" + orgId + "&userType=" + usertype;
               
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetSummaryListCount{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetSummaryListCount, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetSummaryListCount, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetDispensationInfo
        public List<DispensationGridList> GetDispensationInfo(int organisationId, int pageNum,  int pageSize, int usertype, int presetFilter, int? sortOrder = null )
        {
            List<DispensationGridList> objDispenstionGrid = new List<DispensationGridList>();
            try
            {
                string urlParameters = "?organisationId=" + organisationId + "&pageNumber=" + pageNum + "&pageSize=" + pageSize + "&userType=" + usertype + "&presetFilter=" + presetFilter +"&sortOrder="+sortOrder ;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetDispensationInfo{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    objDispenstionGrid = response.Content.ReadAsAsync<List<DispensationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationInfo, Exception: {ex}");
            }
            return objDispenstionGrid;
        }
        #endregion

        #region GetDispensationSearchInfo
        public List<DispensationGridList> GetDispensationSearchInfo(int orgId, int pageNumber, int pageSize, string DRefNo, string Summary, string GrantedBy, string description, int isValid, int chckcunt, int usertype, int presetFilter, int? sortOrder = null)
        {
            List<DispensationGridList> objDispenstionGrid = new List<DispensationGridList>();
            try
            {
                string urlParameters = "?organisationId=" + orgId + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&DRefNo=" + DRefNo + "&summary=" + Summary + "&grantedBy=" + GrantedBy + "&description=" + description + "&isValid=" + isValid + "&chckcunt=" + chckcunt + "&userType=" + usertype + "&presetFilter=" + presetFilter +"&sortOrder="+sortOrder;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetDispensationSearchInfo{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    objDispenstionGrid = response.Content.ReadAsAsync<List<DispensationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationSearchInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationSearchInfo, Exception: {ex}");
            }
            return objDispenstionGrid;
        }
        #endregion

        #region ViewDispensationInfoByDRN
        public DispensationGridList ViewDispensationInfoByDRN(string DRN, int userTypeID)
        {
            DispensationGridList dispObjList = new DispensationGridList();
            try
            {
                string urlParameters = "?DRN=" + DRN + "&userTypeID=" + userTypeID;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/ViewDispensationInfoByDRN{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    dispObjList = response.Content.ReadAsAsync<DispensationGridList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/ViewDispensationInfoByDRN, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/ViewDispensationInfoByDRN, Exception: {ex}");
            }
            return dispObjList;
        }
        #endregion

        #region ViewDispensationInfo
        public DispensationGridList ViewDispensationInfo(int dispId, int userTypeID)
        {
            DispensationGridList dispObjList = new DispensationGridList();
            try
            {
                string urlParameters = "?dispensationId=" + dispId + "&userTypeId=" + userTypeID;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/ViewDispensationInfo{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    dispObjList = response.Content.ReadAsAsync<DispensationGridList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/ViewDispensationInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/ViewDispensationInfo, Exception: {ex}");
            }
            return dispObjList;
        }
        #endregion

        #region GetDispensationDetailsObjByID
        public DispensationGridList GetDispensationDetailsObjByID(int dispid, int userTypeID)
        {
            DispensationGridList dispObjList = new DispensationGridList();
            try
            {
                string urlParameters = "?dispensationId=" + dispid + "&userTypeId=" + userTypeID;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetDispensationDetailsObjByID{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    dispObjList = response.Content.ReadAsAsync<DispensationGridList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationDetailsObjByID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationDetailsObjByID, Exception: {ex}");
            }
            return dispObjList;
        }
        #endregion

        #region UpdateDispensation
        public int UpdateDispensation(UpdateDispensationParams updateDispensation)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/UpdateDispensation", updateDispensation).Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/UpdateDispensation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/UpdateDispensation, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region DeleteDispensation
        public int DeleteDispensation(int dispId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?dispensationId=" + dispId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/Dispensation/DeleteDispensation{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/DeleteDispensation, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/DeleteDispensation, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region SaveDispensation
        public bool SaveDispensation(UpdateDispensationParams updateDispensation)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/SaveDispensation", updateDispensation).Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/SaveDispensation Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/SaveDispensation, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetDispOrganisationInfo
        public List<DispensationGridList> GetDispOrganisationInfo(string org_name, int page, int pageSize, int chckcunt, int usertype)
        {
            List<DispensationGridList> objDispenstionGrid = new List<DispensationGridList>();
            try
            {
                string urlParameters = "?organisationName=" + org_name + "&pageNumber=" + page + "&pageSize=" + pageSize + "&chckcunt=" + chckcunt + "&userType=" + usertype;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetDispOrganisationInfo{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    objDispenstionGrid = response.Content.ReadAsAsync<List<DispensationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispOrganisationInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispOrganisationInfo, Exception: {ex}");
            }
            return objDispenstionGrid;
        }
        #endregion

        #region GetDispensationDetailsByID
        public List<DispensationGridList> GetDispensationDetailsByID(int dispid)
        {
            List<DispensationGridList> objDispenstionGrid = new List<DispensationGridList>();
            try
            {
                string urlParameters = "?dispid=" + dispid;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetDispensationDetailsByID{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    objDispenstionGrid = response.Content.ReadAsAsync<List<DispensationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationDetailsByID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationDetailsByID, Exception: {ex}");
            }
            return objDispenstionGrid;
        }
        #endregion

        public decimal GetDispensationReferenceNumber(string dispensationReferenceNo, int organisationId, string mode, long dispensationId)
        {
            decimal iResult = 0;
            try
            {
                string urlParameters = "?dispensationReferenceNo=" + dispensationReferenceNo+ "&organisationId="+organisationId+ "&mode="+ mode + "&dispensationId="+ dispensationId;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
               $"/Dispensation/GetDispensationReferenceNumber{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    iResult = response.Content.ReadAsAsync<decimal>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationReferenceNumber, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Dispensation/GetDispensationReferenceNumber, Exception: {ex}");
            }
            return iResult;
        }
    }
}
