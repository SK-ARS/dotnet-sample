using NotificationXSD;
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
using STP.Domain.SecurityAndUsers;
using STP.Domain.RouteAssessment;
using STP.Domain.LoggingAndReporting;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public class NotificationDocService : INotificationDocService
    {
        private readonly HttpClient httpClient;
        public NotificationDocService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region GetRouteDescription
        //API method to GetRouteDescription
        public DrivingInstructionModel GetRouteDescription(int NotificationID)
        {

            DrivingInstructionModel structuresDetail = new DrivingInstructionModel();
            try
            {
                //api call to new service
                string urlParameters = "?NotificationID=" + NotificationID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/NotificationDocument/GetRouteDescription{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structuresDetail = response.Content.ReadAsAsync<DrivingInstructionModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetRouteDescription, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetRouteDescription, Exception: {0}", ex);
            }
            return structuresDetail;
        }
        #endregion

        #region Commented Code by Mahzeer on 13/07/2023
        /*
        //API method to GetVehicleUnits
        public int GetVehicleUnits(int ContactId, Int32 OrgId)
        {

            int vehicleUnits = 0;
            try
            {
                //api call to new service
                string urlParameters = "?ContactId=" + ContactId + "&OrgId=" + OrgId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/NotificationDocument/GetVehicleUnits{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    vehicleUnits = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetVehicleUnits, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetVehicleUnits, Exception: {0}", ex);
            }
            return vehicleUnits;
        }

        //Api method to AddManageDocument
        public long AddManageDocument(OutboundDocuments obdc, string userSchema)
        {
            AddManageDocParams addManageDocParams = new AddManageDocParams()
            {
                OutboundDocuments = obdc,
                UserSchema = userSchema
            };
            long documentId = 0;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/NotificationDocument/AddManageDocument",
                           addManageDocParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    documentId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/AddManageDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/AddManageDocument, Exception: {0}", ex);
            }
            return documentId;
        }

        
        //Api method to GenerateMovementAction
        public bool GenerateMovementAction(UserInfo UserSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem, int movFlagVar = 0, NotificationContacts objContact = null)
        {
            GenerateMovementParams generateMovementParams = new GenerateMovementParams()
            {
                UserInfo = UserSessionValue,
                ESDALReference = EsdalRef,
                Movactiontype = movActionItem,
                MovFlagVar = movFlagVar,
                NotificationContacts = objContact
            };
            bool status = false;

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/NotificationDocument/GenerateMovementAction",
                           generateMovementParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    status  = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GenerateMovementAction, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GenerateMovementAction, Exception: {0}", ex);
            }
            return status;
        }
        
        

        //Api method to InsertCollaboration
        public int InsertCollaboration(OutboundDocuments obdc, long documentId, string userSchema, int status = 0)
        {
            InsertCollabParams insertCollabParams = new InsertCollabParams()
            {
                OutboundDocuments= obdc,
                DocumentId = documentId,
                UserSchema = userSchema,
                Status = status
            };

            int isUpdated = 0;
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/NotificationDocument/InsertCollaboration",
                           insertCollabParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    isUpdated = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/InsertCollaboration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/InsertCollaboration, Exception: {0}", ex);
            }
            return isUpdated;
        }
        public StructuresModel GetStructuresXMLByESDALReference(string esdalReference, string userSchema)
        {
            StructuresModel struInfo = new StructuresModel();
            try
            {
                string urlParameters = "?esdalReference=" + esdalReference + "&userSchema=" + userSchema;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                              $"/NotificationDocument/GetStructuresXMLByESDALReference{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    struInfo = response.Content.ReadAsAsync<StructuresModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetStructuresXMLByESDALReference, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetStructuresXMLByESDALReference, Exception: {ex}");
            }
            return struInfo;
        }
        */
        #endregion

        #region GetNENRouteDescription
        //API method to GetNENRouteDescription
        public DrivingInstructionModel GetNENRouteDescription(long NENInboxId, int organisationId)
        {

            DrivingInstructionModel structuresDetail = new DrivingInstructionModel();
            try
            {
                //api call to new service
                string urlParameters = "?NENInboxId=" + NENInboxId + "&OrganisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/NotificationDocument/GetNENRouteDescription{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structuresDetail = response.Content.ReadAsAsync<DrivingInstructionModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetNENRouteDescription, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetNENRouteDescription, Exception: {0}", ex);
            }
            return structuresDetail;
        }
        #endregion

        #region GetStructuresXML
        public StructuresModel GetStructuresXML(int NotificationID)
        {
            StructuresModel structuresDetail = new StructuresModel();
            try
            {
                string urlParameters = "?NotificationID=" + NotificationID;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                              $"/NotificationDocument/GetStructuresXML{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structuresDetail = response.Content.ReadAsAsync<StructuresModel>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetStructuresXML, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetStructuresXML, Exception: {ex}");
            }
            return structuresDetail;
        }
        #endregion

        #region GenerateHaulierNotificationDocument
        public byte[] GenerateHaulierNotificationDocument(int notificationId, Enums.DocumentType doctype, int contactId, UserInfo SessionInfo = null)
        {
            byte[] byteArrayData = null;
            HaulierNotificationParams haulierNotificationParams = new HaulierNotificationParams()
            {
                NotificationId = notificationId,
                DocumentType = doctype,
                ContactId = contactId,
                SessionInfo = SessionInfo
            };

            try
            {
                //api call to new service
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                           $"/NotificationDocument/GetHaulierNotificationDocument",
                           haulierNotificationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    byteArrayData = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetHaulierNotificationDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetHaulierNotificationDocument, Exception: {0}", ex);
            }
            return byteArrayData;
        }
        #endregion

        #region GetContactDetails(int contactId)
        public ContactModel GetContactDetails(int contactId)
        {
            ContactModel contactModel = new ContactModel();

            try
            {
                string urlParameter = "?contactId=" + contactId;

                //api call to new service   
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
            $"/NotificationDocument/GetContactDetails{urlParameter}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    contactModel = response.Content.ReadAsAsync<ContactModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetContactDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/GetContactDetails, Exception: {0}", ex);
            }
            return contactModel;
        }
        #endregion

        #region GetNENRouteDescription
        //API method to GetNENRouteDescription
        public RouteAnalysisModel GetApiRouteAssessmentDetails(int notificationID, int organisationId, int isNen)
        {

            RouteAnalysisModel structuresDetail = new RouteAnalysisModel();
            try
            {
                //api call to new service
                string urlParameters = "?notificationID=" + notificationID + "&organisationId=" + organisationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/NotificationDocument/GetApiRouteAssessmentDetails{ urlParameters}").Result;


                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    structuresDetail = response.Content.ReadAsAsync<RouteAnalysisModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetApiRouteAssessmentDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"NotificationDocument/GetApiRouteAssessmentDetails, Exception: {0}", ex);
            }
            return structuresDetail;
        }
        #endregion
    }
}
