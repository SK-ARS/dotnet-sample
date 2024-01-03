using STP.Common.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public class SORTDocumentService : ISORTDocumentService
    {
        private readonly HttpClient httpClient;
        public SORTDocumentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        //API method to GetSpecialOrder
        public SORTSpecialOrder GetSpecialOrder(string SOrderId)
        {

            SORTSpecialOrder sporder = new SORTSpecialOrder();
            try
            {
                //api call to new service
                string urlParameters = "?orderId=" + SOrderId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/SORTDocument/GetSpecialOrder{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    sporder = response.Content.ReadAsAsync<SORTSpecialOrder>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetSpecialOrder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetSpecialOrder, Exception: {0}", ex);
            }
            return sporder;
        }

        //API method to GetRouteVehicles
        public List<SOCRouteParts> GetRouteVehicles(int movementversionId, string veh_ID)
        {

            List<SOCRouteParts> objRouteParts = new List<SOCRouteParts>();
            try
            {
                //api call to new service
                string urlParameters = "?movementVersionId=" + movementversionId + "&vehicleId=" + veh_ID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/SORTDocument/GetRouteVehicles{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objRouteParts = response.Content.ReadAsAsync<List<SOCRouteParts>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetRouteVehicles, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetRouteVehicles, Exception: {0}", ex);
            }
            return objRouteParts;
        }

        //API method to GetSpecialOrderCoverages
        public List<SOCoverageDetails> GetSpecialOrderCoverages(int projectid, int state)
        {

            List<SOCoverageDetails> lstcoverages = new List<SOCoverageDetails>();
            try
            {
                //api call to new service
                string urlParameters = "?projectid=" + projectid + "&state=" + state;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/SORTDocument/GetSpecialOrderCoverages{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    lstcoverages = response.Content.ReadAsAsync<List<SOCoverageDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetSpecialOrderCoverages, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetSpecialOrderCoverages, Exception: {0}", ex);
            }
            return lstcoverages;
        }

        //API method to ListSortUser
        public List<GetSORTUserList> ListSortUser(long usertypeid, int checkertype = 0)
        {

            List<GetSORTUserList> objSortUser = new List<GetSORTUserList>();
            try
            {
                //api call to new service
                string urlParameters = "?usertypeid=" + usertypeid + "&checkertype=" + checkertype;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/SORTDocument/ListSortUser{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objSortUser = response.Content.ReadAsAsync<List<GetSORTUserList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/ListSortUser, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/ListSortUser, Exception: {0}", ex);
            }
            return objSortUser;
        }

        //API method to GetSpecialOrderList
        public List<SORTMovementList> GetSpecialOrderList(long ProjectID)
        {

            List<SORTMovementList> SpecialOrderListObj = new List<SORTMovementList>();
            try
            {
                //api call to new service
                string urlParameters = "?ProjectID=" + ProjectID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/SORTDocument/GetSpecialOrderList{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    SpecialOrderListObj = response.Content.ReadAsAsync<List<SORTMovementList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetSpecialOrderList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetSpecialOrderList, Exception: {0}", ex);
            }
            return SpecialOrderListObj;
        }

        //API method to Deletespecialorder
        public bool Deletespecialorder(string Orderno, string userschema)
        {

            bool result = false;
            try
            {
                //api call to new service
                string urlParameters = "?Orderno=" + Orderno + "&userschema=" + userschema;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/SORTDocument/Deletespecialorder{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/Deletespecialorder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/Deletespecialorder, Exception: {0}", ex);
            }
            return result;
        }

        //Api method to SaveSortSpecialOrder
        public string SaveSortSpecialOrder(SORTSpecialOrder apprevisionId, List<string> RCoverages)
        {
            SpecialOrderParams specialOrderParams = new SpecialOrderParams()
            {
                SpecialOrderModel = apprevisionId,
                ListCoverages = RCoverages
            };
            string soNumber = "";

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/SORTDocument/SaveSortSpecialOrder",
                           specialOrderParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    soNumber = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/SaveSortSpecialOrder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/SaveSortSpecialOrder, Exception: {0}", ex);
            }
            return soNumber;
        }

        //Api method to UpdateSortSpecialOrder
        public string UpdateSortSpecialOrder(SORTSpecialOrder model, List<string> removedCovrg)
        {
            SpecialOrderParams specialOrderParams = new SpecialOrderParams()
            {
                SpecialOrderModel = model,
                ListCoverages = removedCovrg
            };
            string soNumber = "";

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/SORTDocument/UpdateSortSpecialOrder",
                           specialOrderParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    soNumber = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/UpdateSortSpecialOrder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/UpdateSortSpecialOrder, Exception: {0}", ex);
            }
            return soNumber;
        }

        //Api method to List NE Broken Route Details
        public List<NotifRouteImport> ListNEBrokenRouteDetails(long NEN_Id, int IUserId, long InboxItemId, int IOrgId)
        {
            NotifRouteImportParams NotifRouteImportParams = new NotifRouteImportParams()
            {
                NENId = NEN_Id,
                IUserId = IUserId,
                InboxItemId = InboxItemId,
                IOrgId = IOrgId
            };
            List<NotifRouteImport> list = new List<NotifRouteImport>();

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/SORTDocument/ListNEBrokenRouteDetails",
                           NotifRouteImportParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    list = response.Content.ReadAsAsync<List<NotifRouteImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/ListNEBrokenRouteDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/ListNEBrokenRouteDetails, Exception: {0}", ex);
            }
            return list;
        }

        //API method to GetTransmissionType
        public List<TransmissionModel> GetTransmissionType(long TransId, string Status, int StatusItemCount, string userSchema)

        {

            List<TransmissionModel> transmissionList = new List<TransmissionModel>(); try
            {
                
                //api call to new service
                string urlParameters = "?TransId=" + TransId + "&Status=" + Status + "&StatusItemCount=" + StatusItemCount + "&userSchema=" + userSchema;
                //HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                //                                                  $"/SORTDocument/GetTransmissionType{ urlParameters}").Result;


                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/Document/GetTransmissionType{ urlParameters}").Result;




                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    transmissionList = response.Content.ReadAsAsync<List<TransmissionModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetTransmissionType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetTransmissionType, Exception: {0}", ex);
            }
            return transmissionList;
        }

        //Api method to GenrateSODocument
        public byte[] GenrateSODocument(Enums.SOTemplateType templatetype, string esdalRefNo, string orderNumber, UserInfo userInfo = null, bool generateFlag = true)
        {
            SODocumentParams soDocumentParams = new SODocumentParams()
            {
                TemplateType = templatetype,
                EsdalReferenceNo = esdalRefNo,
                OrderNumber = orderNumber,
                UserInfo = userInfo,
                GenerateFlag = generateFlag
            };
            byte[] exportByteArrayData = null;
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/SORTDocument/GenrateSODocument",
                           soDocumentParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    exportByteArrayData = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenrateSODocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenrateSODocument, Exception: {0}", ex);
            }
            return exportByteArrayData;
        }
        //API method to GetNoofPages
        public int GetNoofPages(String outputString)
        {

            int result =0;
            try
            {
                //api call to new service
                string urlParameters = "?outputString=" + outputString;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/SORTDocument/GetNoofPages{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetNoofPages, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GetNoofPages, Exception: {0}", ex);
            }
            return result;

        }
    }
}
