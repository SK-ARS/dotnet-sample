using STP.Common.Logger;
using STP.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using STP.Common.Constants;
using STP.Domain.HelpdeskTools;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.LoggingAndReporting;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Folder;
using STP.Domain.SecurityAndUsers;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public class MovementsService : IMovementsService
    {
        private readonly HttpClient httpClient;
        public MovementsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public List<MovementsInbox> GetMovementInbox(GetInboxMovementsParams inboxMovementsParams)
        {
            List<MovementsInbox> objListMoveInbox = new List<MovementsInbox>();
            try
            {
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(inboxMovementsParams);
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                $"/Movements/GetInboxMovements",
                inboxMovementsParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objListMoveInbox = response.Content.ReadAsAsync<List<MovementsInbox>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetInboxMovements, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetInboxMovements, Exception: {ex}");
            }
            return objListMoveInbox;
        }
        public List<SORTMovementList> GetSORTMovementList(int organisationId, int pageNum, int pageSize, SORTMovementFilter objSORTMovementFilter, SortAdvancedMovementFilter objSORTMovementFilterAdvanced, bool IsCreCandidateOrCreAppl = false, SortMapFilter SortMapFilter=null, bool planMovement = false,int sortOrder=1,int sortType=0)
        {
            List<SORTMovementList> result = new List<SORTMovementList>();
            SORTMovementListParams SORTMovementListParams = new SORTMovementListParams()
            {
                OrganisationID = organisationId,
                PageNum = pageNum,
                PageSize = pageSize,
                SORTMovementFilter = objSORTMovementFilter,
                SORTAdvMovementFilter = objSORTMovementFilterAdvanced,
                IsCreCandidateOrCreAppl = IsCreCandidateOrCreAppl,
                PlanMovement = planMovement,
                SortObjMapFilter = SortMapFilter,
                sortOrder = sortOrder,
                sortType= sortType
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                $"/Movements/GetSORTMovementList",
                SORTMovementListParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<SORTMovementList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetSORTMovementList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetSORTMovementList, Exception: {ex}");
            }
            return result;
        }
        #region public Object GetSORTMovementRelatedToStructList()
        public List<SORTMovementList> GetSORTMovementRelatedToStructList(int orgID, int pageNum, int pageSize, long structID)
        {
            List<SORTMovementList> result = new List<SORTMovementList>();
            try
            {
                string urlParameters = "?organisationId=" + orgID + "&pageNumber=" + pageNum + "&pageSize=" + pageSize + "&structID=" + structID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                $"/Movements/GetSORTMovementRelatedToStructList{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<SORTMovementList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetSORTMovementRelatedToStructList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetSORTMovementRelatedToStructList, Exception: {ex}");
            }
            return result;
        }

       
        #endregion

        #region DistributionView Status - AuthorizeMovementGeneral
        #region public int GetContactDetails()
        public int GetContactDetails(int UserId)
        {
            int Contact_Id = 0;
            try
            {
                string urlParameters = "?UserId=" + UserId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetContactDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    Contact_Id = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContactDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContactDetails, Exception: {ex}");
            }
            return Contact_Id;
        }
        #endregion
        #region public int EditInboxItemOpenStatus()
        public int EditInboxItemOpenStatus(long inboxId, long organisationId)
        {
            int editOpenStatusFlag = 0;
            try
            {
                string urlParameters = "?inboxId=" + inboxId + "&OrganisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
             $"/Movements/EditInboxItemOpenStatus{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    editOpenStatusFlag = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/EditInboxItemOpenStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/EditInboxItemOpenStatus, Exception: {ex}");
            }
            return editOpenStatusFlag;
        }
        #endregion
        #region public Object GetAuthorizeMovementGeneralProposed()
        public MovementModel GetAuthorizeMovementGeneralProposed(string route, string mnemonic, string esdalrefnum, string version, long inboxId, string esdal_ref, int contactId, long organisationId)
        {
            MovementModel objMovementModel = new MovementModel();
            try
            {
                MovementModelParams objMovementModelParams = new MovementModelParams
                {
                    Route = route,
                    Mnemonic = mnemonic,
                    ESDALReferenceNumber = esdalrefnum,
                    Version = version,
                    InboxId = inboxId,
                    ESDALReference = esdal_ref,
                    ContactId = contactId,
                    OrganisationId = organisationId
                };


                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/GetAuthorizeMovementGeneralProposed",
                    objMovementModelParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objMovementModel = response.Content.ReadAsAsync<MovementModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetAuthorizeMovementGeneralProposed, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetAuthorizeMovementGeneralProposed, Exception: {ex}");
            }

            return objMovementModel;
        }
        #endregion
        #region public string GetSpecialOrderNo()
        public string GetSpecialOrderNo(string ESDALReferenceNo)
        {
            string OrderNumber = null;
            try
            {
                string esdalEncoded = HttpUtility.UrlEncode(ESDALReferenceNo); //contain spl character #
                string urlParameters = "?ESDALreferenceNo=" + esdalEncoded;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                               $"/Movements/GetSpecialOrderNo{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    OrderNumber = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetSpecialOrderNo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetSpecialOrderNo, Exception: {ex}");
            }
            return OrderNumber;
        }
        #endregion
        #region public Object GetVehiclesList()
        public List<VehicleConfigration> GetVehiclesList(string mnemonic, string ESDALreferenceNo, string version, long notificationId, int IsSimplified)
        {
            List<VehicleConfigration> objVehicleConfigration = new List<VehicleConfigration>();
            try
            {
                string urlParameters = "?mnemonic=" + mnemonic + "&ESDALreferenceNo=" + ESDALreferenceNo + "&version=" + version + "&notificationId=" + notificationId + "&isSimplified=" + IsSimplified;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetVehiclesList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objVehicleConfigration = response.Content.ReadAsAsync<List<VehicleConfigration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetVehiclesList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetVehiclesList, Exception: {ex}");
            }
            return objVehicleConfigration;
        }
        #endregion
        #region public Object GetHAAndHaulierContactIdByName()
        public MovementModel GetHAAndHaulierContactIdByName(MovementModel movement)
        {
            MovementModel objMovementModel = new MovementModel();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/GetHAAndHaulierContactIdByName",
                    movement).Result;
                if (response.IsSuccessStatusCode)
                {
                    objMovementModel = response.Content.ReadAsAsync<MovementModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetHAAndHaulierContactIdByName, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetHAAndHaulierContactIdByName, Exception: {ex}");
            }
            return objMovementModel;
        }
        #endregion
        #region public long GetDocumentID()
        public long GetDocumentID(string ESDALReferenceNo, long organisationId)
        {
            long documentId = 0;
            try
            {
                string esdalEncoded = HttpUtility.UrlEncode(ESDALReferenceNo); //contain spl character #
                string urlParameters = "?ESDALReferenceNo=" + esdalEncoded + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                               $"/Movements/GetDocumentID{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    documentId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetDocumentID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetDocumentID, Exception: {ex}");
            }
            return documentId;
        }
        #endregion
        #region public Object GetCollaborationNotes()
        public List<CollaborationNotes> GetCollaborationNotes(long documentId, long organisationId)
        {
            List<CollaborationNotes> objCollaborationNotes = new List<CollaborationNotes>();
            try
            {
                string urlParameters = "?documentId=" + documentId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                               $"/Movements/GetCollaborationNotes{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objCollaborationNotes = response.Content.ReadAsAsync<List<CollaborationNotes>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetCollaborationNotes, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetCollaborationNotes, Exception: {ex}");
            }
            return objCollaborationNotes;
        }
        #endregion
        #region public Object GetAuthorizeMovementGeneral()
        public MovementModel GetAuthorizeMovementGeneral(long notificationId, long inboxId, long contactId, string ESDALReference, long organisationId)
        {
            MovementModel objMovementModel = new MovementModel();
            try
            {
                MovementModelParams objMovementParams = new MovementModelParams
                {
                    Notificationid = notificationId,
                    InboxId = inboxId,
                    ContactId = contactId,
                    ESDALReference = ESDALReference,
                    OrganisationId = organisationId
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/GetAuthorizeMovementGeneral",
                    objMovementParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objMovementModel = response.Content.ReadAsAsync<MovementModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetAuthorizeMovementGeneral, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetAuthorizeMovementGeneral, Exception: {ex}");
            }
            return objMovementModel;
        }
        #endregion
        #region public Object GetHaulierContactId()
        public MovementModel GetHaulierContactId(MovementModel objMovement)
        {
            MovementModel objMovementModel = new MovementModel();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/GetHaulierContactId",
                    objMovement).Result;
                if (response.IsSuccessStatusCode)
                {
                    objMovementModel = response.Content.ReadAsAsync<MovementModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetHaulierContactId, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetHaulierContactId, Exception: {ex}");
            }
            return objMovementModel;
        }
        #endregion
        #region public Object GetSpecialOrders()
        public List<SpecialOrder> GetSpecialOrders(string notificationCode)
        {
            List<SpecialOrder> SpecialOrderList = new List<SpecialOrder>();
            try
            {
                string esdalEncoded = HttpUtility.UrlEncode(notificationCode); //contain spl character #
                string urlParameters = "?notificationCode=" + esdalEncoded;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                             $"/Movements/GetSpecialOrders{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    SpecialOrderList = response.Content.ReadAsAsync<List<SpecialOrder>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetSpecialOrders, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetSpecialOrders, Exception: {ex}");
            }
            return SpecialOrderList;
        }
        #endregion
        #region public Object GetVR1s()
        public List<VR1> GetVR1s(string VR1_NUMBER)
        {
            List<VR1> VR1List = new List<VR1>();
            try
            {
                string urlParameters = "?VR1_NUMBER=" + VR1_NUMBER;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetVR1s{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    VR1List = response.Content.ReadAsAsync<List<VR1>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetVR1s, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetVR1s, Exception: {ex}");
            }
            return VR1List;
        }
        #endregion
        #region public Object GetNotificationDetailsByCode()
        public List<RelatedCommunication> GetNotificationDetailsByCode(string notificationCode, string route, long organisationId, long ProjectId)
        {
            List<RelatedCommunication> RelatedCommunicationList = new List<RelatedCommunication>();
            try
            {
                string urlParameters = "?notificationCode=" + notificationCode + "&route=" + route + "&organisationId=" + organisationId + "&projectId=" + ProjectId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                               $"/Movements/GetNotificationDetailsByCode{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    RelatedCommunicationList = response.Content.ReadAsAsync<List<RelatedCommunication>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetNotificationDetailsByCode, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetNotificationDetailsByCode, Exception: {ex}");
            }
            return RelatedCommunicationList;
        }
        #endregion
        #region public long UpdateInboxItemStatus()
        public long UpdateInboxItemStatus(long OrganisationId, string ESDALRef)
        {
            long transmissionId = 0;
            try
            {
                InboxItemStatusParams objInboxItemStatusParams = new InboxItemStatusParams
                {
                    OrganisationId = OrganisationId,
                    ESDALRef = ESDALRef
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/UpdateInboxItemStatus",
                    objInboxItemStatusParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    transmissionId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/UpdateInboxItemStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/UpdateInboxItemStatus, Exception: {ex}");
            }
            return transmissionId;
        }
        #endregion
        #region public Object GetNENVehicleList()
        public List<VehicleConfigration> GetNENVehicleList(long NENId, long inboxId, long orgId)
        {
            List<VehicleConfigration> VehicleList = new List<VehicleConfigration>();
            try
            {
                string urlParameters = "?NENId=" + NENId + "&inboxId=" + inboxId + "&organisationId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                               $"/Movements/GetNENVehicleList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    VehicleList = response.Content.ReadAsAsync<List<VehicleConfigration>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetNENVehicleList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetNENVehicleList, Exception: {ex}");
            }
            return VehicleList;
        }
        #endregion
        #region public string GetContentReferenceNo()
        public string GetContentReferenceNo(int notificationNo)
        {
            string V_CONTENT_REF_NO = "0";
            try
            {
                string urlParameters = "?notificationNo=" + notificationNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                               $"/Movements/GetContentReferenceNo{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    V_CONTENT_REF_NO = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContentReferenceNo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContentReferenceNo, Exception: {ex}");
            }
            return V_CONTENT_REF_NO;
        }
        #endregion
        #region public int InsertQuickLinkSOA()
        public int InsertQuickLinkSOA(int orgId, int inboxId, int userId)
        {
            int linkno = 0;
            try
            {
                string urlParameters = "?organisationId=" + orgId + "&inboxId=" + inboxId + "&userId=" + userId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                               $"/Movements/InsertQuickLinkSOA{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    linkno = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/InsertQuickLinkSOA, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/InsertQuickLinkSOA, Exception: {ex}");
            }
            return linkno;
        }
        #endregion
        #region public long  SaveNotificationAuditLog()
        public long SaveNotificationAuditLog(AuditLogIdentifiers auditLogType, string logMsg, int User_ID, long Org_ID = 0)
        {
            long result = 0;
            try
            {
                AuditLogIdentifiersParams objAuditLogIdentifiers = new AuditLogIdentifiersParams
                {
                    LogMsg = logMsg,
                    UserId = User_ID,
                    OrganisationId = Org_ID,                    
                    AuditLogType = new AuditLogIdentifiers()
                    {
                        ESDALNotificationNo = auditLogType.ESDALNotificationNo,
                        InboxItemId = auditLogType.InboxItemId
                    }
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/SaveNotificationAuditLog",
                    objAuditLogIdentifiers).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/SaveNotificationAuditLog, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/SaveNotificationAuditLog, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #endregion
        #region public int GetArrangementList()
        public List<DelegArrangeNameList> GetArrangementList(int orgId)
        {
            List<DelegArrangeNameList> objDelegArrangeNameList = new List<DelegArrangeNameList>();
            try
            {
                string urlParameters = "?organisationId=" + orgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetArrangementList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objDelegArrangeNameList = response.Content.ReadAsAsync<List<DelegArrangeNameList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetArrangementList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetArrangementList, Exception: {ex}");
            }
            return objDelegArrangeNameList;
        }
        #endregion
        #region public int GetQuickLinksSOAList()
        public List<QuickLinksSOA> GetQuickLinksSOAList(int UserId)
        {
            List<QuickLinksSOA> objQuickLinkSOAList = new List<QuickLinksSOA>();
            try
            {
                string urlParameters = "?UserId=" + UserId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetQuickLinksSOAList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objQuickLinkSOAList = response.Content.ReadAsAsync<List<QuickLinksSOA>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetQuickLinksSOAList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetQuickLinksSOAList, Exception: {ex}");
            }

            return objQuickLinkSOAList;
        }
        #endregion
        #region public Object GetFolderList()
        public List<FolderNameList> GetFolderList(long orgId, string userSchema = UserSchema.Portal)
        {
            List<FolderNameList> objFolderNameList = new List<FolderNameList>();
            try
            {
                string urlParameters = "?organisationId=" + orgId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetFolderList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objFolderNameList = response.Content.ReadAsAsync<List<FolderNameList>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetFolderList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetFolderList, Exception: {ex}");
            }
            return objFolderNameList;
        }
        #endregion
        #region public Object GetMovementsList()
        public List<MovementsList> GetMovementsList(int orgId, int pageNum, int pageSize, MovementsFilter movementFilter, MovementsAdvancedFilter advancedMovementFilter, int presetFilter, string userSchema = UserSchema.Portal, int ShowPrevSortRoute = 0, bool prevMovImport = false)
        {
            List<MovementsList> objHaulierMovementList = new List<MovementsList>();
            try
            {
                HaulierMovementsListParams objMovementsListParams = new HaulierMovementsListParams
                {
                    OrganisationId = orgId,
                    PageNo = pageNum,
                    PageSize = pageSize,
                    PresetFilter = presetFilter,
                    UserSchema = userSchema,
                    ShowPreviousSORTRoute = ShowPrevSortRoute,
                    PrevMovImport = prevMovImport,
                    MovementFilter = new MovementsFilter()
                    {
                        WorkInProgress = movementFilter.WorkInProgress,
                        WorkInProgressApplication = movementFilter.WorkInProgressApplication,
                        WorkInProgressNotification = movementFilter.WorkInProgressNotification,
                        Submitted = movementFilter.Submitted,
                        ReceivedByHA = movementFilter.ReceivedByHA,
                        WithdrawnApplications = movementFilter.WithdrawnApplications,
                        DeclinedApplications = movementFilter.DeclinedApplications,
                        Agreed = movementFilter.Agreed,
                        ProposedRoute = movementFilter.ProposedRoute,
                        Notifications = movementFilter.Notifications,
                        ApprovedVR1 = movementFilter.ApprovedVR1,
                        NeedsAttention = movementFilter.NeedsAttention,
                        NewCollabration = movementFilter.NewCollabration,
                        ReadCollaboration = movementFilter.ReadCollaboration,
                        MostRecentVersion = movementFilter.MostRecentVersion,
                        IncludeHistoric = movementFilter.IncludeHistoric,
                        FolderId = movementFilter.FolderId,
                        SO = movementFilter.SO,
                        VSO = movementFilter.VSO,
                        STGO = movementFilter.STGO,
                        CandU = movementFilter.CandU,
                        Tracked = movementFilter.Tracked,
                        STGOVR1 = movementFilter.STGOVR1,
                        BtnClearSORTOrder = movementFilter.BtnClearSORTOrder,
                        NotifyVSO=movementFilter.NotifyVSO
                    },
                    AdvancedMovementFilter = new MovementsAdvancedFilter()
                    {
                        ESDALReference = advancedMovementFilter.ESDALReference,
                        MyReference = advancedMovementFilter.MyReference,
                        StartOrEnd = advancedMovementFilter.StartOrEnd,
                        FleetId = advancedMovementFilter.FleetId,
                        Keyword = advancedMovementFilter.Keyword,
                        Client = advancedMovementFilter.Client,
                        ReceiptOrganisation = advancedMovementFilter.ReceiptOrganisation,
                        VehicleRegistration = advancedMovementFilter.VehicleRegistration,
                        GrossWeight = advancedMovementFilter.GrossWeight,
                        GrossWeight1 = advancedMovementFilter.GrossWeight1,
                        OverallWidth = advancedMovementFilter.OverallWidth,
                        OverallWidth1 = advancedMovementFilter.OverallWidth1,
                        Length = advancedMovementFilter.Length,
                        Length1 = advancedMovementFilter.Length1,
                        Height = advancedMovementFilter.Height,
                        Height1 = advancedMovementFilter.Height1,
                        AxleWeight = advancedMovementFilter.AxleWeight,
                        AxleWeight1 = advancedMovementFilter.AxleWeight1,
                        MovementFromDate = advancedMovementFilter.MovementFromDate,
                        MovementToDate = advancedMovementFilter.MovementToDate,
                        ApplicationFromDate = advancedMovementFilter.ApplicationFromDate,
                        ApplicationToDate = advancedMovementFilter.ApplicationToDate,
                        NotificationFromDate = advancedMovementFilter.NotificationFromDate,
                        NotificationToDate = advancedMovementFilter.NotificationToDate,
                        WeightCount = advancedMovementFilter.WeightCount,
                        WidthCount = advancedMovementFilter.WidthCount,
                        LengthCount = advancedMovementFilter.LengthCount,
                        HeightCount = advancedMovementFilter.HeightCount,
                        AxleCount = advancedMovementFilter.AxleCount,
                        MovementDate = advancedMovementFilter.MovementDate,
                        ApplicationDate = advancedMovementFilter.ApplicationDate,
                        NotifyDate = advancedMovementFilter.NotifyDate,
                        SORTOrder = advancedMovementFilter.SORTOrder,
                        HaulierName = advancedMovementFilter.HaulierName,
                        SONum = advancedMovementFilter.SONum,
                        VRNum = advancedMovementFilter.VRNum,
                        ApplicationType = advancedMovementFilter.ApplicationType,
                        LoadDetails = advancedMovementFilter.LoadDetails,
                        StartPoint = advancedMovementFilter.StartPoint,
                        EndPoint = advancedMovementFilter.EndPoint,
                        IncludeHistoricalData=advancedMovementFilter.IncludeHistoricalData,
                        VehicleClass = advancedMovementFilter.VehicleClass,
                        QueryString1 = advancedMovementFilter.QueryString1,
                        QueryString2 = advancedMovementFilter.QueryString2,
                        LogicOpr= advancedMovementFilter.LogicOpr
                    }
                };
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(objMovementsListParams);
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/GetMovementsList",
                    objMovementsListParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objHaulierMovementList = response.Content.ReadAsAsync<List<MovementsList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetMovementsList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetMovementsList, Exception: {ex}");
            }
            return objHaulierMovementList;
        }
        #endregion
        #region PrintReport
        public string PrintReport(long Notificationid)
        {
            string recipientXMLInformation = string.Empty;
            try
            {
                string urlParameters = "?notificationId=" + Notificationid;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/PrintReport{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    recipientXMLInformation = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/PrintReport, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/PrintReport, Exception: {ex}");
            }
            return recipientXMLInformation;
        }
        #endregion
        #region ViewMovementDocument
        public DocumentInfo ViewMovementDocument(long documentId, long organisationId, string userSchema)
        {
            DocumentInfo XmlOutboundDoc = new DocumentInfo();
            try
            {
                string urlParameters = "?documentId=" + documentId + "&OrganisationId=" + organisationId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/ViewMovementDocument{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    XmlOutboundDoc = response.Content.ReadAsAsync<DocumentInfo>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ViewMovementDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ViewMovementDocument, Exception: {ex}");
            }
            return XmlOutboundDoc;
        }
        #endregion
        public MovementModel GetCollaborationStatus(long INBOX_ID)
        {
            MovementModel result = new MovementModel();
            try
            {
                string urlParameters = "?inboxId=" + INBOX_ID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetCollaborationStatus{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<MovementModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetCollaborationStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetCollaborationStatus, Exception: {ex}");
            }
            return result;
        }
        public bool ManageCollaborationStatus(MovementModel movement)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/Movements/ManageCollaborationStatus", movement).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ManageCollaborationStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ManageCollaborationStatus, Exception: {ex}");
            }
            return result;
        }
        public string PrintAgreedReport(string Notificationcode, long organisationId)
        {
            string recipientXMLInformation = string.Empty;
            try
            {
                string urlParameters = "?Notificationcode=" + Notificationcode + "&OrganisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/PrintAgreedReport{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    recipientXMLInformation = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/PrintAgreedReport, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/PrintAgreedReport, Exception: {ex}");
            }
            return recipientXMLInformation;
        }

        public string GetProposalOutboundDocsXML(long documentNumber)
        {
            string recipientXMLInformation = string.Empty;
            try
            {
                string urlParameters = "?documentNumber=" + documentNumber;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetProposalOutboundDocsXML{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    recipientXMLInformation = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetProposalOutboundDocsXML, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetProposalOutboundDocsXML, Exception: {ex}");
            }
            return recipientXMLInformation;
        }

        public MovementModel GetInboxItemDetails(string esdalRefNumber, long organisationId)
        {
            MovementModel result = new MovementModel();
            try
            {
                string esdal_encoded = HttpUtility.UrlEncode(esdalRefNumber); //contain spl character #
                string urlParameters = "?esdalRefNumber=" + esdal_encoded + "&OrganisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetInboxItemDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<MovementModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetInboxItemDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetInboxItemDetails, Exception: {ex}");
            }
            return result;
        }
        public bool ManageNotesOnEscort(MovementModel movement)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/Movements/ManageNotesOnEscort", movement).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ManageNotesOnEscort, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ManageNotesOnEscort, Exception: {ex}");
            }
            return result;
        }
        public string GetHaulierUserId(string FirstName, string Surname, int OrgId)
        {
            string result = String.Empty;
            try
            {
                string urlParameters = "?firstName=" + FirstName + "&surName=" + Surname + "&organisationId=" + OrgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                               $"/Movements/GetHaulierUserId{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetHaulierUserId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetHaulierUserId, Exception: {ex}");
            }
            return result;
        }
        public bool ManageInternalNotes(MovementModel movement)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/Movements/ManageInternalNotes", movement).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ManageInternalNotes, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ManageInternalNotes, Exception: {ex}");
            }
            return result;
        }
        public List<MapStructLink> GetStructLinkId(SortMapFilter objSortMapFilterParams)
        {
            List<MapStructLink> result = new List<MapStructLink>();
           
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                $"/Movements/GetStructLinkId",
                objSortMapFilterParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<MapStructLink>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetStructLinkId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetStructLinkId, Exception: {ex}");
            }
            return result;
        }
        #region public int GetContactDetailsForDefault()
        public int GetContactDetailsForDefault(int OrganisationId)
        {
            int Contact_Id = 0;
            try
            {
                string urlParameters = "?organisationId=" + OrganisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetContactDetailsForDefault{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    Contact_Id = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContactDetailsForDefault, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContactDetailsForDefault, Exception: {ex}");
            }
            return Contact_Id;
        }

        public List<MovementsList> GetPlanMovementList(int organisationId, int pageNumber, int pageSize, MovementsAdvancedFilter advancedMovementFilter, int presetFilter, int movementType, int vehicleClass, string userSchema)
        {
            List<MovementsList> movementsLists= new List<MovementsList>();
            MovementsFilter movementFilter = new MovementsFilter();
            try
            {
                HaulierMovementsListParams objMovementsListParams = new HaulierMovementsListParams
                {
                    OrganisationId = organisationId,
                    PageNo = pageNumber,
                    PageSize = pageSize,
                    PresetFilter = presetFilter,
                    UserSchema = userSchema,
                    VehicleClass = vehicleClass,
                    MovementType = movementType,
                    MovementFilter = movementFilter,
                    AdvancedMovementFilter = new MovementsAdvancedFilter()
                    {
                        ESDALReference = advancedMovementFilter.ESDALReference,
                        MyReference = advancedMovementFilter.MyReference,
                        StartOrEnd = advancedMovementFilter.StartOrEnd,
                        FleetId = advancedMovementFilter.FleetId,
                        Keyword = advancedMovementFilter.Keyword,
                        Client = advancedMovementFilter.Client,
                        ReceiptOrganisation = advancedMovementFilter.ReceiptOrganisation,
                        VehicleRegistration = advancedMovementFilter.VehicleRegistration,
                        GrossWeight = advancedMovementFilter.GrossWeight,
                        OverallWidth = advancedMovementFilter.OverallWidth,
                        Length = advancedMovementFilter.Length,
                        Height = advancedMovementFilter.Height,
                        AxleWeight = advancedMovementFilter.AxleWeight,
                        MovementFromDate = advancedMovementFilter.MovementFromDate,
                        MovementToDate = advancedMovementFilter.MovementToDate,
                        ApplicationFromDate = advancedMovementFilter.ApplicationFromDate,
                        ApplicationToDate = advancedMovementFilter.ApplicationToDate,
                        NotificationFromDate = advancedMovementFilter.NotificationFromDate,
                        NotificationToDate = advancedMovementFilter.NotificationToDate,
                        WeightCount = advancedMovementFilter.WeightCount,
                        WidthCount = advancedMovementFilter.WidthCount,
                        LengthCount = advancedMovementFilter.LengthCount,
                        HeightCount = advancedMovementFilter.HeightCount,
                        AxleCount = advancedMovementFilter.AxleCount,
                        MovementDate = advancedMovementFilter.MovementDate,
                        ApplicationDate = advancedMovementFilter.ApplicationDate,
                        NotifyDate = advancedMovementFilter.NotifyDate,
                        SORTOrder = advancedMovementFilter.SORTOrder,
                        HaulierName = advancedMovementFilter.HaulierName,
                        SONum = advancedMovementFilter.SONum,
                        VRNum = advancedMovementFilter.VRNum,
                        ApplicationType = advancedMovementFilter.ApplicationType,
                        LoadDetails = advancedMovementFilter.LoadDetails,
                        StartPoint = advancedMovementFilter.StartPoint,
                        EndPoint = advancedMovementFilter.EndPoint,
                        IncludeHistoricalData = advancedMovementFilter.IncludeHistoricalData
                    }
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                    $"/Movements/GetPlanMovementList",
                    objMovementsListParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    movementsLists = response.Content.ReadAsAsync<List<MovementsList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Movements/GetPlanMovementList, Error:" + (int)response.StatusCode + '-' + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Movements/GetPlanMovementList, Exception:" + ex);
            }
            return movementsLists;
        }
        #endregion

        #region get affected paries of proposed and agreed movement
        public MovementContactModel GetContactedPartiesDetail(long analysisId)
        {
            MovementContactModel contactDetail = new MovementContactModel();
            try
            {
                string urlParameters = "?analysisId=" + analysisId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/Movements/GetContactedPartiesDetail{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    contactDetail = response.Content.ReadAsAsync<MovementContactModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContactedPartiesDetail, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetContactedPartiesDetail, Exception: {ex}");
            }
            return contactDetail;
        }
        #endregion

        public List<MovementsInbox> GetHomePageMovements(GetInboxMovementsParams inboxMovementsParams)
        {
            List<MovementsInbox> objListMoveInbox = new List<MovementsInbox>();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                $"/Movements/GetHomePageMovements",
                inboxMovementsParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    objListMoveInbox = response.Content.ReadAsAsync<List<MovementsInbox>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/GetHomePageMovements, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/GetHomePageMovements, Exception: {ex}");
            }
            return objListMoveInbox;
        }

        public int ReturnRouteAutoAssignVehicle(long movementId, int flag, long notificationId, long organisationId)
        {
            int iCount = 0;
            try
            {
                string strUrlParameters = "?movementId=" + movementId + "&flag=" + flag + "&notificationId=" + notificationId + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync(
                $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                $"/Movements/ReturnRouteAutoAssignVehicle"+ strUrlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    iCount = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/ReturnRouteAutoAssignVehicle, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsService/ReturnRouteAutoAssignVehicle, Exception: {ex}");
            }
            return iCount;
        }

        public XMLModel GetXmlDataForPrint(SOProposalDocumentParams documentParams)
        {
            XMLModel result=null;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" + $"/Document/GetDocument", documentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<XMLModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetDocument, Exception: {ex}");
            }
            return result;
        }

        public List<FolderTreeModel> GetFolders(long organisationId)
        {
            List<FolderTreeModel> result = new List<FolderTreeModel>();
            try
            {
                string urlParameters = "?organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                             $"/MovementsFolder/GetFolders{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<FolderTreeModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/GetFolders, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/GetFolders, Exception: {ex}");
            }
            return result;
        }

        public int InsertFolderInfo(Domain.MovementsAndNotifications.Folder.InsertFolderParams model)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/MovementsFolder/InsertFolderInfo", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/InsertFolderInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/InsertFolderInfo, Exception: {ex}");
            }
            return result;
        }

        public int UpdateFolderInfo(Domain.MovementsAndNotifications.Folder.EditFolderParams model)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/MovementsFolder/EditFolderInfo", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/UpdateFolderInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/UpdateFolderInfo, Exception: {ex}");
            }
            return result;
        }

        public int DeleteFolderInfo(EditFolderParams model)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/MovementsFolder/DeleteFolderInfo", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/DeleteFolderInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/DeleteFolderInfo, Exception: {ex}");
            }
            return result;
        }
        public int AddItemToFolder(List<AddItemFolderModel> model)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/MovementsFolder/AddItemToFolder", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/AddItemToFolder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/AddItemToFolder, Exception: {ex}");
            }
            return result;
        }

        public int RemoveItemsFromFolder(List<AddItemFolderModel> model)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/MovementsFolder/RemoveItemsFromFolder", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/RemoveItemsFromFolder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/RemoveItemsFromFolder, Exception: {ex}");
            }
            return result;
        }

        public int MoveFolderToFolder(FolderTreeModel model)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                   $"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" + $"/MovementsFolder/MoveFolderToFolder", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/MoveFolderToFolder, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"MovementsFolder/MoveFolderToFolder, Exception: {ex}");
            }
            return result;
        }
        public DateTime CalcualteMovementDate(int noticePeriod, int vehicleClass, string userSchema)
        {
            DateTime dateTime;
            try
            {
                string urlParameters = "?noticePeriod=" + noticePeriod + "&vehicleClass=" + vehicleClass + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                               $"/Movements/CalcualteMovementDate{urlParameters}").Result; 
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    dateTime = response.Content.ReadAsAsync<DateTime>().Result;
                }
                else
                {
                    //do exception handling here
                    dateTime = DateTime.Now;
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/CalcualteMovementDate, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                dateTime = DateTime.Now;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Movements/CalcualteMovementDate, Exception: {ex}");
            }
            return dateTime;
        }

        #region GetAffectedContacts for NEN notifications
        public List<ContactModel> GetNENAffectedContactDetails(string esdalRefNumber, string userSchema)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            try
            {
                string urlParameters = "?esdalRefNumber=" + Uri.EscapeDataString(esdalRefNumber) + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["MovementsAndNotifications"]}" +
                                               $"/Movements/GetNENAffectedContactDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    contacts = response.Content.ReadAsAsync<List<ContactModel>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Movements/GetNENAffectedContactDetails, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Movements/GetNENAffectedContactDetails, Exception:" + ex);
            }
            return contacts;
        }
        #endregion

        
    }
}
