using AggreedRouteXSD;
using NotificationXSD;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.General;
using STP.Common.Logger;
using STP.Common.StringExtractor;
using STP.Domain;
using STP.Domain.Communications;
using STP.Domain.Custom;
using STP.Domain.DocumentsAndContents;
using STP.Domain.HelpdeskTools;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets;
using STP.ServiceAccess.CommunicationsInterface;
using STP.ServiceAccess.LoggingAndReporting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using static STP.Common.Enums.ExternalApiEnums;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public class DocumentService : IDocumentService
    {
        private readonly HttpClient httpClient;
        private readonly ICommunicationsInterfaceService communicationsService;
        private readonly ILoggingService loggingService;

        public DocumentService(HttpClient httpClient, ICommunicationsInterfaceService commService, ILoggingService logService)
        {
            this.httpClient = httpClient;
            communicationsService = commService;
            loggingService = logService;
        }

        #region GetNotificationDispensation
        public List<NotifDispensations> GetNotificationDispensation(long notificationId, int historic)
        {
            List<NotifDispensations> notifDispensations = new List<NotifDispensations>();
            try
            {
                string urlParameters = "?notificationId=" + notificationId + "&historic=" + historic;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                               $"/Document/GetDispensation{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    notifDispensations = response.Content.ReadAsAsync<List<NotifDispensations>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Account/GetUserDetailsForNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Account/GetUserDetailsForNotif, Exception: {ex}");
            }
            return notifDispensations;
        }
        #endregion

        #region GetUserDetailsForNotif
        public UserInfo GetUserDetailsForNotification(string ESDALReference)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                string ESDALReferenceEncoded = HttpUtility.UrlEncode(ESDALReference);
                string urlParameters = "?ESDALReference=" + ESDALReferenceEncoded;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                               $"/Document/GetUserDetailsForNotification{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objUserInfo = response.Content.ReadAsAsync<UserInfo>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Account/GetUserDetailsForNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Account/GetUserDetailsForNotif, Exception: {ex}");
            }
            return objUserInfo;
        }
        #endregion

        #region GetUserDetailsForHaulier
        public UserInfo GetUserDetailsForHaulier(string mnemonic, string ESDALReference)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                string urlParameters = "?mnemonic=" + mnemonic + "&ESDALReference=" + ESDALReference;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Document/GetUserDetailsForHaulier{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objUserInfo = response.Content.ReadAsAsync<UserInfo>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetUserDetailsForHaulier, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetUserDetailsForHaulier, Exception: {ex}");
            }
            return objUserInfo;
        }
        #endregion

        #region GetUserName
        public UserInfo GetUserName(long orgId, long contactId)
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                string urlParameters = "?orgId=" + orgId + "&contactId=" + contactId;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Document/GetUserName{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objUserInfo = response.Content.ReadAsAsync<UserInfo>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetUserName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetUserName, Exception: {ex}");
            }
            return objUserInfo;
        }
        #endregion

        #region GetSOAPoliceDetails
        public DistributionAlerts GetSOAPoliceDetails(string ESDALReference, int transmissionId)
        {
            DistributionAlerts objDistributionAlerts = new DistributionAlerts();
            try
            {
                string esdalEncoded = HttpUtility.UrlEncode(ESDALReference); //contain spl character #
                string urlParameters = "?Esdal_ref=" + esdalEncoded + "&trsmission_id=" + transmissionId;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Document/GetSOAPoliceDetails{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objDistributionAlerts = response.Content.ReadAsAsync<DistributionAlerts>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetSOAPoliceDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetSOAPoliceDetails, Exception: {ex}");
            }
            return objDistributionAlerts;
        }
        #endregion

        #region GetNotifDetails
        public DistributionAlerts GetNotifDetails(string ESDALReference, int transmissionId)
        {
            DistributionAlerts objDistributionAlerts = new DistributionAlerts();
            try
            {
                string esdalEncoded = HttpUtility.UrlEncode(ESDALReference); //contain spl character #
                string urlParameters = "?ESDALReference=" + esdalEncoded + "&TransmissionId=" + transmissionId;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                $"/Document/GetNotifDetails{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objDistributionAlerts = response.Content.ReadAsAsync<DistributionAlerts>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetNotifDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetNotifDetails, Exception: {ex}");
            }
            return objDistributionAlerts;
        }
        #endregion

        #region GetHaulierDetails
        public DistributionAlerts GetHaulierDetails(string mnemonic, string ESDALReference, string versionNo)
        {
            DistributionAlerts objDistributionAlerts = new DistributionAlerts();
            try
            {
                string esdalEncoded = HttpUtility.UrlEncode(ESDALReference); //contain spl character #
                string urlParameters = "?mnemonic=" + mnemonic + "&Esdal_ref=" + esdalEncoded + "&ver_no=" + versionNo;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                $"/Document/GetHaulierDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objDistributionAlerts = response.Content.ReadAsAsync<DistributionAlerts>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetHaulierDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetHaulierDetails, Exception: {ex}");
            }
            return objDistributionAlerts;
        }
        #endregion

        #region GetAgreedProposedNotificationXML
        public byte[] GetAgreedProposedNotificationXML(string docType, string ESDALReference, int notificationId)
        {
            byte[] docBytes = null;
            try
            {
                string urlParameters = "?docType=" + docType + "&ESDALReference=" + ESDALReference + "&notificationId=" + notificationId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                $"/Document/GetAgreedProposedNotificationXML{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    docBytes = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetAgreedProposedNotificationXML, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetAgreedProposedNotificationXML, Exception: {ex}");
            }

            return docBytes;
        }
        #endregion

        #region SortSideCheckDoctype
        public TransmittingDocumentDetails SortSideCheckDoctype(int transmissionId, string userSchema)
        {
            TransmittingDocumentDetails transmittingDetail = new TransmittingDocumentDetails();
            try
            {
                string urlParameters = "?transmissionId=" + transmissionId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                $"/Document/SortSideCheckDoctype{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    transmittingDetail = response.Content.ReadAsAsync<TransmittingDocumentDetails>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SortSideCheckDoctype, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SortSideCheckDoctype, Exception: {ex}");
            }

            return transmittingDetail;
        }
        #endregion

        #region GenerateHaulierProposedRouteDocument
        public byte[] GenerateHaulierProposedRouteDocument(string esdalRefNo, int organisationId, int contactId, string userSchema = UserSchema.Portal, UserInfo sessionInfo = null)
        {
            byte[] docBytes = null;
            ProposedDocParams proposedDocParams = new ProposedDocParams()
            {
                EsdalReferenceNo = esdalRefNo,
                OrganisationId = organisationId,
                ContactId = contactId,
                UserSchema = userSchema,
                SessionInfo = sessionInfo
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateHaulierProposedDoc",
                          proposedDocParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    docBytes = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateHaulierProposedDoc, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateHaulierProposedDoc, Exception: {ex}");
            }

            return docBytes;
        }
        #endregion

        #region Generate PDF
        public byte[] GeneratePDF(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, int contactID, string docfileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {
            byte[] content = null;
            GeneratePdfParams generatePdfParams = new GeneratePdfParams()
            {
                NotificationID = notificationID,
                DocType = docType,
                XMLInformation = xmlInformation,
                FileName = fileName,
                ESDALReferenceNo = esDALRefNo,
                OrganisationID = organisationID,
                ContactID = contactID,
                DocumentFileName = docfileName,
                IsHaulier = isHaulier,
                OrganisationName = organisationName,
                HAReference = HAReference,
                RoutePlanUnits = routePlanUnits,
                DocumentType = documentType,
                UserInfo = userInfo,
                UserType = userType

            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateDoc",
                          generatePdfParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    content = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateDoc, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateDoc, Exception: {ex}");
            }

            return content;
        }
        #endregion

        #region Generate PDF
        public string GeneratePDF1(int notificationID, int docType, string xmlInformation, string fileName, string esDALRefNo, long organisationID, int contactID, string docfileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {
            string content = null;
            GeneratePdfParams generatePdfParams = new GeneratePdfParams()
            {
                NotificationID = notificationID,
                DocType = docType,
                XMLInformation = xmlInformation,
                FileName = fileName,
                ESDALReferenceNo = esDALRefNo,
                OrganisationID = organisationID,
                ContactID = contactID,
                DocumentFileName = docfileName,
                IsHaulier = isHaulier,
                OrganisationName = organisationName,
                HAReference = HAReference,
                RoutePlanUnits = routePlanUnits,
                DocumentType = documentType,
                UserInfo = userInfo,
                UserType = userType

            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateDoc1",
                          generatePdfParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    content = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateDoc, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateDoc, Exception: {ex}");
            }

            return content;
        }
        #endregion

        #region Get Logged In User Affected Structure Details By ESDALReference
        public string GetLoggedInUserAffectedStructureDetailsByESDALReference(string xmlInformation, string esDALRefNo, UserInfo SessionInfo, string userSchema, string type, int organisationId)
        {
            string xmlInfo = null;
            AffectedStructureParams affectedStructureParams = new AffectedStructureParams()
            {
                XMLInformation = xmlInformation,
                ESDALReferenceNo = esDALRefNo,
                SessionInfo = SessionInfo,
                UserSchema = userSchema,
                Type = type,
                OrganisationId = organisationId
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/AffectedStructureDetailsByESDALReference",
                          affectedStructureParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body
                    xmlInfo = response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetLoggedInUserAffectedStructureDetailsByESDALReference, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetLoggedInUserAffectedStructureDetailsByESDALReference, Exception: {ex}");
            }

            return xmlInfo;
        }
        #endregion

        #region SortSideRetransmitApplication
        public int SortSideRetransmitApplication(int transmissionId, RetransmitDetails retransmitDetails, UserInfo userInfo)
        {
            int status = 0;
            RetransmitApplicationParams retransmitApplicationParams = new RetransmitApplicationParams()
            {
                TransmissionId = transmissionId,
                RetransmitDetails = retransmitDetails,
                UserInfo = userInfo
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/SortSideRetransmitApplication",
                          retransmitApplicationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    status = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SortSideRetransmitApplication, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SortSideRetransmitApplication, Exception: {ex}");
            }

            return status;
        }
        #endregion

        #region GetVehicleComponentAxles
        public List<VehComponentAxles> GetVehicleComponentAxles(int notificationId, long vehicleId)
        {
            List<VehComponentAxles> objVehComponentAxles = new List<VehComponentAxles>();

            try
            {
                //api call to new service   
                string urlParameters = "?notificationId=" + notificationId + "&vehicleId=" + vehicleId;

                HttpResponseMessage response = new HttpClient().GetAsync(
                    $"{System.Configuration.ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                    $"/Contents/GetVehicleComponentAxles{ urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objVehComponentAxles = response.Content.ReadAsAsync<List<VehComponentAxles>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessment, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RouteAssessment/UpdateRouteAssessment, Exception: {0}", ex);
            }
            return objVehComponentAxles;

        }
        public NotificationXSD.SummaryAxleStructureAxleWeightListPosition[] GetAxleWeightListPositions(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureAxleWeightListPosition> sasawlpList = new List<NotificationXSD.SummaryAxleStructureAxleWeightListPosition>();
            try
            {
                List<VehComponentAxles> objVehicleComponentAxles = new List<VehComponentAxles>();
                //api call to new service   
                HttpResponseMessage response = new HttpClient().PostAsJsonAsync(

                     $"{System.Configuration.ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                     $"/Contents/GetAxleWeightListPositions",
                     objVehicleComponentAxles).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    sasawlpList = response.Content.ReadAsAsync<List<NotificationXSD.SummaryAxleStructureAxleWeightListPosition>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleWeightListPositions, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleWeightListPositions, Exception: {0}", ex);
            }
            return sasawlpList.ToArray();
        }
        public NotificationXSD.SummaryAxleStructureAxleSpacingListPosition[] GetAxleSpacingListPositionAxleSpacings(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition> sasawlpList = new List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition>();
            try
            {
                List<VehComponentAxles> objVehicleComponentAxles = new List<VehComponentAxles>();
                //api call to new service   
                HttpResponseMessage response = new HttpClient().PostAsJsonAsync(

                     $"{System.Configuration.ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                     $"/Contents/GetAxleSpacingListPositionAxleSpacings",
                     objVehicleComponentAxles).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    sasawlpList = response.Content.ReadAsAsync<List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleSpacingListPositionAxleSpacings, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleSpacingListPositionAxleSpacings, Exception: {0}", ex);
            }
            return sasawlpList.ToArray();
        }
        public SummaryAxleStructureAxleSpacingToFollowListPosition[] GetAxleSpacingToFollowListPositionAxleSpacings(List<VehComponentAxles> vehicleComponentAxlesList, double firstComponentAxleSpaceToFollow)
        {
            List<SummaryAxleStructureAxleSpacingToFollowListPosition> summaryAxleStructureAxleSpacingToFollowListPosits = new List<SummaryAxleStructureAxleSpacingToFollowListPosition>();
            try
            {
                Domain.DocumentsAndContents.AxleFollowParams axleFollowParams = new Domain.DocumentsAndContents.AxleFollowParams()
                {
                    vehicleComponentAxlesList = vehicleComponentAxlesList,
                    firstComponentAxleSpaceToFollow = firstComponentAxleSpaceToFollow
                };
                //api call to new service   
                HttpResponseMessage response = new HttpClient().PostAsJsonAsync(

                     $"{System.Configuration.ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                     $"/Contents/GetAxleSpacingToFollowListPositionAxleSpacings",
                     axleFollowParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    summaryAxleStructureAxleSpacingToFollowListPosits = response.Content.ReadAsAsync<List<NotificationXSD.SummaryAxleStructureAxleSpacingToFollowListPosition>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleSpacingListPositionAxleSpacings, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleSpacingListPositionAxleSpacings, Exception: {0}", ex);
            }
            return summaryAxleStructureAxleSpacingToFollowListPosits.ToArray();
        }
        #endregion

        #region GetRetransmitDetails
        public RetransmitDetails GetRetransmitDetails(long transmissionId, string userSchema)
        {
            RetransmitDetails retransmitDetails = new RetransmitDetails();

            try
            {
                //api call to new service   
                string urlParameters = "?transmissionId=" + transmissionId + "&userSchema=" + userSchema;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/Document/GetRetransmitDetails{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    retransmitDetails = response.Content.ReadAsAsync<RetransmitDetails>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetRetransmitDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetRetransmitDetails, Exception: {0}", ex);
            }
            return retransmitDetails;

        }
        #endregion

        #region GetSOAPoliceContactList
        public List<ContactModel> GetSOAPoliceContactList(XMLModel modelSOAPolice)
        {
            List<ContactModel> contactSOAPoliceList = new List<ContactModel>();

            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GetSOAPoliceContactList",
                          modelSOAPolice).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    contactSOAPoliceList = response.Content.ReadAsAsync<List<ContactModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetSOAPoliceContactList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetSOAPoliceContactList, Exception: {0}", ex);
            }
            return contactSOAPoliceList;

        }
        #endregion

        #region GetNewInsertedTransForDist()
        public long GetNewInsertedTransForDist(TransmittingDocumentDetails transDetails)
        {
            long newtransId = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GetNewInsertedTransForDist",
                          transDetails).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    newtransId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetNewInsertedTransForDist, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetNewInsertedTransForDist, Exception: {ex}");
            }

            return newtransId;
        }
        #endregion

        #region GetRetransmitDocument
        public RetransmitEmailgetParams GetRetransmitDocument(GetRetransmitDocumentParams getRetransmit)
        {
            RetransmitEmailgetParams retransmit = new RetransmitEmailgetParams();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GetRetransmitDocument",
                          getRetransmit).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    retransmit = response.Content.ReadAsAsync<RetransmitEmailgetParams>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetRetransmitDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetRetransmitDocument, Exception: {ex}");
            }

            return retransmit;
        }
        #endregion

        #region SaveMovementActionForDistTrans
        public long SaveMovementActionForDistTrans(MovementActionIdentifiers movactiontype, string MovDescrp,long ProjectId,int RevisionNo,int VersionNo, string userSchema)
        {
            long status = 0;
            try
            {
                GenerateMovementActionParams urlParams = new GenerateMovementActionParams
                {
                    MovementActionIdentifier = movactiontype,
                    MovementDesc = MovDescrp,
                    projectId= ProjectId,
                    revisionNo= RevisionNo,
                    versionNo= VersionNo,
                    UserSchema = userSchema
                };
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/SaveMovementActionForDistTrans",
                          urlParams).Result;
                if (response.IsSuccessStatusCode)
                {

                    status = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveMovementActionForDistTrans, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveMovementActionForDistTrans, Exception: {0}", ex);
            }
            return status;

        }
        #endregion

        #region Generate PDF
        public byte[] GeneratePDFFromHtmlString(HtmlDocumentParams model)
        {
            byte[] content = null;
            
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GeneratePDFFromHtmlString",
                          model).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    content = response.Content.ReadAsAsync<byte[]>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GeneratePDFFromHtmlString, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateDoc, Exception: {ex}");
            }

            return content;
        }
        #endregion

        #region Notification/Movement Version Distribution Process
        public void DistributeNotification(NotifDistibutionParams distibutionParams)
        {
            int contactId = 0;
            int organisationId = 0;
            string orgName = string.Empty;
            string contactName = string.Empty;
            try
            {
                bool imminentState = false;
                string docFileName;
                string[] contactDetails = new string[6];
                int docType = (int)ItemDocType.doc003;//322003
                string[] imminentStateArr = null;

                ESDALNotificationGetParams notifParams;
                GenerateEsdalNotificationParams esdalNotificationParams = new GenerateEsdalNotificationParams()
                {
                    NotificationId = distibutionParams.NotificationId,
                    ContactId = distibutionParams.ContactId,
                    ICAStatusDictionary = distibutionParams.ICAStatusDictionary,
                    ImminentMoveStatus = distibutionParams.ImminentMovestatus,
                    UserInfo = distibutionParams.SessionInfo
                };
                if (!distibutionParams.IsRenotify)
                    notifParams = GenerateEsdalNotification(esdalNotificationParams);
                else
                    notifParams = GenerateEsdalReNotification(esdalNotificationParams);

                if (!string.IsNullOrWhiteSpace(distibutionParams.ImminentMovestatus) && distibutionParams.ImminentMovestatus != "No imminent movement")
                    imminentStateArr = distibutionParams.ImminentMovestatus.Split(',');

                List<ContactModel> contactSOAPoliceList = distibutionParams.IsNen ? distibutionParams.ContactModel : StringExtraction.GetRecipientDetails(notifParams.XMLModel.ReturnXML);

                //Soa related code part
                var contactList = contactSOAPoliceList.ToList();
                string xsltPath;

                #region Contact List Loop
                foreach (ContactModel objcontact in contactList)
                {
                    contactId = objcontact.ContactId;
                    organisationId = objcontact.OrganisationId;
                    orgName = objcontact.Organisation;
                    contactName = objcontact.FullName;

                    imminentState = false;
                    if (objcontact.ContactId != 0)
                    {
                        //function that returns contact's details in a string array
                        contactDetails = FetchContactPreference(objcontact.ContactId, UserSchema.Portal);
                        objcontact.OrganisationId = objcontact.OrganisationId == 0 ? Convert.ToInt32(contactDetails[4]) : objcontact.OrganisationId; // saving organisation Id into the NotificationContacts object

                        //Imminent movement, Imminent movement for police, Imminent movement for SOA, Imminent movement for SOA and police
                        if (imminentStateArr != null && imminentStateArr[0].ToLower().StartsWith("imminent movement"))//Get imminent status to send mail to that organisation affected parties
                            imminentState = (objcontact.ISPolice && imminentStateArr[0].ToLower().Contains("police")) || !objcontact.ISPolice ? 
                                GetImminentForCountries(objcontact.OrganisationId, distibutionParams.ImminentMovestatus):
                                false;
                    }
                    else
                    {
                        contactDetails[0] = (objcontact.Email != null && objcontact.Email != "") ? "Email" : "Online Inbox Only";
                        contactDetails[1] = objcontact.Email;
                        contactDetails[2] = objcontact.Fax;
                        contactDetails[5] = "0";
                    }
                    if(distibutionParams.IsNen)
                        contactDetails[0] = "Online Inbox Only";

                    if (!distibutionParams.IsRenotify)
                        docFileName = !objcontact.ISPolice ? "NotificationFaxSOA" : "NotificationFaxPolice";
                    else
                        docFileName = !objcontact.ISPolice ? "RenotificationFaxSOA" : "RenotificationFaxPolice";

                    xsltPath = !objcontact.ISPolice ? notifParams.XsltSOAPath : notifParams.XsltPolicePath;//storing the soa related XSLT path

                    contactDetails[3] = !objcontact.ISPolice ? "soa" : "police";
                    GenerateEmailParams distributionInputParams = new GenerateEmailParams
                    {
                        NotificationId = distibutionParams.NotificationId,
                        Projectid = distibutionParams.ProjectId,
                        DocType = docType,
                        XmlInformation = notifParams.XMLModel.ReturnXML,
                        FileName = xsltPath,
                        ESDALReferenceNo = notifParams.outboundDocuments.EsdalReference,
                        Contact = objcontact,
                        DocumentFileName = docFileName,
                        ImminentMovestatus = imminentState,
                        UserInfo = distibutionParams.SessionInfo,
                        OrganisationId = objcontact.OrganisationId,
                        IcaStatus= (distibutionParams.ICAStatusDictionary != null && distibutionParams.ICAStatusDictionary.ContainsKey(objcontact.OrganisationId))?
                        distibutionParams.ICAStatusDictionary[objcontact.OrganisationId]: (int)ExternalApiSuitability.unknown
                    };
                    DistributeMovements(distributionInputParams, notifParams.outboundDocuments.OrganisationID, contactDetails, objcontact);
                }
                #endregion
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Distribute Notification Exception, NotificationId:" + 
                    distibutionParams.NotificationId + ",ContactId:" + contactId + ",OrgsanisationId:" + organisationId + 
                    ",ContactName:" + contactName + ",OrganisationName:" + orgName + ",Exception Message" + ex.Message + " "+ ex.StackTrace );
            }
        }
        public void DistributeSOMovement(SODistributionParams sODistributionParams, ref int status)
        {
            int contactId = 0;
            int organisationId = 0;
            string orgName = string.Empty;
            string contactName = string.Empty;
            try
            {
                int docType;
                string docFileName;
                string[] contactDet = new string[6];
                SOProposalDocumentParams sOProposalDocument = new SOProposalDocumentParams
                {
                    EsdalReferenceNo = sODistributionParams.EsdalReference,
                    OrganisationId = (int)sODistributionParams.SessionInfo.OrganisationId,
                    ContactId = 0,
                    DistributionComments = sODistributionParams.DistribComments,
                    VersionId = sODistributionParams.Versionid,
                    ICAStatusDictionary = sODistributionParams.IcaStatusDictionary,
                    Esdalreference = sODistributionParams.EsdalReferenceNum,
                    HaContactDetail = sODistributionParams.HaContact,
                    Agreedroute = sODistributionParams.AgreedRoute,
                    UserSchema = sODistributionParams.SessionInfo.UserSchema,
                    RoutePlanUnits = 692001,
                    ProjectStatus = sODistributionParams.ProjectStatus,
                    VersionNo = sODistributionParams.VersionNo,
                    Moveprint = sODistributionParams.Moveprint,
                    PreVersionDistr = sODistributionParams.PreVersionDistr,
                    SessionInfo = sODistributionParams.SessionInfo
                };
                SODistributeDocumentParams sODistribute = GenerateSODistributeDocument(sOProposalDocument);
                sODistributionParams.ProjectStatus = sODistribute.ProjectStatus;
                List<ContactModel> contactList = sODistribute.ContactList;
                docType = sODistribute.DocType;
                docFileName = sODistribute.DocFileName;
                XMLModel model = null;
                if (sODistribute.XmlModel.ReturnXML != null)
                    model = sODistribute.XmlModel;
                else if (sODistribute.ModelStillAfftdSOA.ReturnXML != null)
                    model = sODistribute.ModelStillAfftdSOA;

                #region Contact List Loop
                foreach (ContactModel objcontact in contactList)
                {
                    contactId = objcontact.ContactId;
                    organisationId = objcontact.OrganisationId;
                    orgName = objcontact.Organisation;
                    contactName = objcontact.FullName;
                    if (objcontact.Organisation != null && objcontact.FullName != null && objcontact.Organisation != string.Empty && objcontact.FullName != string.Empty)
                    {
                        if (objcontact.ContactId != 0)
                        {
                            contactDet = FetchContactPreference(objcontact.ContactId, UserSchema.Portal);
                            objcontact.OrganisationId = objcontact.OrganisationId == 0 ? Convert.ToInt32(contactDet[4]) : objcontact.OrganisationId; // saving organisation Id into the NotificationContacts object
                        }
                        else
                        {
                            contactDet[3] = "police";// send the police document to any manually added party
                            contactDet[0] = (objcontact.Email != null && objcontact.Email != "") ? "Email" : "Online Inbox Only";
                            contactDet[1] = objcontact.Email;
                            contactDet[2] = objcontact.Fax;
                            contactDet[5] = "0";
                        }
                        GetDocFileType(sODistributionParams.ProjectStatus, ref docType, ref docFileName, contactDet, sODistribute, ref model, out string xsltPath, out SOProposalXsltPath proposalXsltPath, objcontact.Reason);
                        string returnXml = model.ReturnXML;
                        if (sODistribute.XmlModel.ReturnXML != null)
                            returnXml = sODistribute.XmlModel.ReturnXML;
                        else if (sODistribute.ModelStillAfftdSOA.ReturnXML != null)
                            returnXml = sODistribute.ModelStillAfftdSOA.ReturnXML;
                        else if (sODistribute.ModelPolice.ReturnXML != null)
                            returnXml = sODistribute.ModelPolice.ReturnXML;
                        else if (sODistribute.ModelSOA.ReturnXML != null)
                            returnXml = sODistribute.ModelSOA.ReturnXML;
                        GenerateEmailParams distributionParams = new GenerateEmailParams
                        {
                            NotificationId = Convert.ToInt32(model.NotificationID),
                            DocType = docType,
                            XmlInformation = returnXml,
                            FileName = xsltPath,
                            ESDALReferenceNo = sODistributionParams.EsdalReference,
                            Contact = objcontact,
                            DocumentFileName = docFileName,
                            ImminentMovestatus = false,
                            UserInfo = sODistributionParams.SessionInfo,
                            OrganisationId = objcontact.OrganisationId,
                            Projectid= sODistributionParams.ProjectId,
                            RevisionNo= sODistributionParams.LastRevNo,
                            VersionNo= sODistributionParams.VersionNo,
                            IcaStatus = (sODistributionParams.IcaStatusDictionary != null && sODistributionParams.IcaStatusDictionary.ContainsKey(objcontact.OrganisationId)) ?
                                sODistributionParams.IcaStatusDictionary[objcontact.OrganisationId] : (int)ExternalApiSuitability.suitable
                        };
                        DistributeMovements(distributionParams, distributionParams.OrganisationId, contactDet, objcontact);
                    }
                }
                #endregion

                #region Code part to clone movement details by extracting esdal reference code details
                if (contactList.Count > 0)
                {
                    status = 1;
                    string Esdalref = sODistributionParams.EsdalReference;
                    Esdalref = Esdalref.Replace("~", "#");
                    Esdalref = Esdalref.Replace("-", "/");
                    Esdalref = Esdalref.Replace("#", "/");
                    string[] esdalRefPro = Esdalref.Split('/');

                    MovementCopyDetails moveDetails = new MovementCopyDetails();
                    if (esdalRefPro.Length > 0)
                    {
                        moveDetails.HaulMnemonic = Convert.ToString(esdalRefPro[0]);
                        moveDetails.ESDALRefNo = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                        moveDetails.VersionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                    }
                    byte[] hacontactbytes = GetHAContactDetails(sODistributionParams.HaContact, sODistributionParams.AgreedRoute);
                    CopyMovementSortToPortal(moveDetails, 0, sODistributionParams.Versionid, sODistributionParams.EsdalReference, hacontactbytes, (int)sODistributionParams.SessionInfo.OrganisationId, sODistributionParams.SessionInfo.UserSchema);

                }
                #endregion

                #region movement actions for this action method
                if (contactList.Count != 0)
                {
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.sort_desk_distributes_movement_version;
                    movactiontype.ContactName = sODistributionParams.SessionInfo.FirstName + " " + sODistributionParams.SessionInfo.LastName;
                    switch (sODistributionParams.ProjectStatus)
                    {
                        case (int)ProjectStatus.proposed:
                            movactiontype.ProjectStatus = "PROPOSED";
                            break;
                        case (int)ProjectStatus.reproposed:
                            movactiontype.ProjectStatus = "REPROPOSED";
                            break;
                        case (int)ProjectStatus.agreed:
                            movactiontype.ProjectStatus = "AGREED";
                            break;
                        case (int)ProjectStatus.agreed_revised:
                            movactiontype.ProjectStatus = "AGREED_REVISED";
                            break;
                        case (int)ProjectStatus.agreed_recleared:
                            movactiontype.ProjectStatus = "AGREED_RECLEARED";
                            break;
                    }
                    string MovementDescription = MovementActions.GetMovementActionString(sODistributionParams.SessionInfo, movactiontype, out string ErrMsg);
                    loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, sODistributionParams.ProjectId, sODistributionParams.LastRevNo, sODistributionParams.VersionNo, sODistributionParams.SessionInfo.UserSchema);

                }
                #endregion
            }
            catch(Exception ex)
            {
                status = 2;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Distribute Movement Exception, Versionid:" +
                   sODistributionParams.Versionid + ",ContactId:" + contactId + ",OrgsanisationId:" + organisationId +
                   ",ContactName:" + contactName + ",OrganisationName:" + orgName + ",Exception" + ex.Message + " " + ex.StackTrace);
            }
        }
        
        #region Private Method
        private void GetDocFileType(long ProjectStatus, ref int docType, ref string docFileName, string[] contactDet, SODistributeDocumentParams sODistribute, ref XMLModel model, out string xsltPath, out SOProposalXsltPath proposalXsltPath, string latestReason)
        {
            string[] stringSeparators = new string[] { "##**##" };
            string finalReason = "";
            try
            {
                if (latestReason != null && latestReason != string.Empty && latestReason.IndexOf("##**##") != -1)
                {
                    string[] reasonArray = latestReason.Split(stringSeparators, StringSplitOptions.None);
                    if (reasonArray.Length > 1)
                        finalReason = reasonArray[1];
                }
                else if (latestReason != null && latestReason != string.Empty && latestReason.IndexOf("##**##") == -1)
                    finalReason = latestReason;
            }
            catch
            {
                // do nothing
            }
            switch (ProjectStatus)
            {
                case (int)Common.Enums.ProjectStatus.reproposed: //307004
                    {
                        if (finalReason == "no longer affected") //no longer affected fax soa
                        {
                            docFileName = contactDet[3] == "soa" ? "ReProposalNolongerAffectedFaxSOA" : "ReProposalNolongerAffectedFaxPolice";
                            docType = (int)ItemDocType.doc007;// 322007
                            model = sODistribute.ModelNoLongAfftdSOA;
                        }
                        else //still affected fax soa
                        {
                            docFileName = contactDet[3] == "soa" ? "ReProposalStillAffectedFaxSOA" : "ReProposalStillAffectedFaxPolice";
                            docType = (int)ItemDocType.doc001;//322001
                            model = sODistribute.ModelStillAfftdSOA;
                        }
                        break;
                    }
                case (int)Common.Enums.ProjectStatus.agreed: //307005
                    {
                        docFileName = "AgreedRouteSOA";
                        docType = (int)ItemDocType.doc002;//322002
                        model = contactDet[3] == "soa" ? sODistribute.ModelSOA : sODistribute.ModelPolice;
                    }
                    break;
                case (int)Common.Enums.ProjectStatus.agreed_revised: //307006
                    {
                        if (finalReason == "no longer affected")//no longer affected fax soa
                        {
                            docFileName = "ReProposalNolongerAffectedFaxSOA";
                            docType = (int)ItemDocType.doc007;//322007
                            model = sODistribute.ModelNoLongAfftdSOA;
                        }
                        else //soa
                        {
                            docFileName = contactDet[3] == "soa" ? "RevisedAgreement" : "RevisedAgreementPolice";
                            docType = (int)ItemDocType.doc002;//322002
                            model = contactDet[3] == "soa" ? sODistribute.ModelSOA : sODistribute.ModelPolice;
                        }
                        break;
                    }
                case (int)Common.Enums.ProjectStatus.agreed_recleared: //307007 //soa
                    {
                        docFileName = contactDet[3] == "soa" ? "ReclearedAgreementFaxSOA" : "ReclearedAgreementFaxPolice";
                        docType = (int)ItemDocType.doc002;//322002
                        model = contactDet[3] == "soa" ? sODistribute.ModelSOA : sODistribute.ModelPolice;
                    }
                    break;
            }
            proposalXsltPath = GetSoProposalXsltPath(contactDet[3], ProjectStatus, finalReason);
            xsltPath = proposalXsltPath.XSLTPath != "" ? proposalXsltPath.XSLTPath : sODistribute.XsltPath;
        }
        private byte[] GetHAContactDetails(HAContact haContactDetailObj, AgreedRouteStructure agreedroute)
        {
            HAContactStructure objhacontact = new HAContactStructure();

            AggreedRouteXSD.AddressStructure sddrstructure = new AggreedRouteXSD.AddressStructure();
            objhacontact.TelephoneNumber = haContactDetailObj.Telephone;
            objhacontact.EmailAddress = haContactDetailObj.Email;
            objhacontact.Contact = haContactDetailObj.ContactName;
            objhacontact.FaxNumber = haContactDetailObj.Fax;
            string[] Addstru = new string[5];
            Addstru[0] = haContactDetailObj.HAAddress1;
            Addstru[1] = haContactDetailObj.HAAddress2;
            Addstru[2] = haContactDetailObj.HAAddress3;
            Addstru[3] = haContactDetailObj.HAAddress4;
            Addstru[4] = haContactDetailObj.HAAddress5;
            sddrstructure.Line = Addstru;
            objhacontact.Address = sddrstructure;
            switch (haContactDetailObj.Country)
            {
                case "England":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.england;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Wales":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.wales;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Scotland":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.scotland;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Northern Ireland":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.northernireland;
                    sddrstructure.CountrySpecified = true;
                    break;
            }
            sddrstructure.PostCode = haContactDetailObj.PostCode;
            agreedroute.HAContact = objhacontact;

            XmlSerializer serializer = new XmlSerializer(typeof(AgreedRouteStructure));

            StringWriter outStream = new StringExtractor.Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, agreedroute);
            string str = outStream.ToString();

            byte[] hacont = StringExtractor.ZipAndBlob(str);
            return hacont;
        }
        private void DistributeMovements(GenerateEmailParams distributionParams, long organisationId, string[] contactDetails, ContactModel objcontact)
        {
            switch (contactDetails[0])
            {
                case "Email":
                case "Online inbox plus email (HTML)":
                    {
                        objcontact.Email = contactDetails[1];
                        distributionParams.RoutePlanUnits = 692001;
                        distributionParams.xmlAttach = contactDetails[5] == "1" ? 1 : 0;
                        DistributeEmail(distributionParams);
                        break;
                    }
                case "Fax":
                    {
                        objcontact.Fax = contactDetails[2];
                        DistributePdf(distributionParams);
                        break;
                    }
                case "Online Inbox Only":
                    {
                        distributionParams.OrganisationId = distributionParams.OrganisationId != 0 ? distributionParams.OrganisationId : organisationId;
                        distributionParams.GenerateFlag = true;
                        DistributeWord(distributionParams);
                        break;
                    }
            }
        }
        private void DistributeEmail(GenerateEmailParams emailParams)
        {
            GenerateEmailgetParams emailgetParams = GenerateMovementEMAIL(emailParams);
            string logoImageName1 = ConfigurationManager.AppSettings["LogoImagePath"] + "Content/assets/images/logo.png";
            string logoImageName2 = ConfigurationManager.AppSettings["LogoImagePath"] + "Content/assets/images/National Highways Logo.svg";
            string logoImageTag = "<img align=\"left\" width=\"120\" height=\"80\" src=\"" + logoImageName1 + "\" /><img align=\"left\" width=\"120\" height=\"80\" src=\"" + logoImageName2 + "\" />";
            emailgetParams.HtmlContent = emailgetParams.HtmlContent.Replace("<img />", logoImageTag);

            byte[] CompressXMLString = XsltTransformer.CompressData(emailgetParams.AttachmentData);

            #region NotificationContacts
            NotificationContacts notiContacts = new NotificationContacts
            {
                ContactId = emailParams.Contact.ContactId,
                ContactName = emailParams.Contact.FullName,
                Email = emailParams.Contact.Email,
                Fax = emailParams.Contact.Fax,
                Reason = emailParams.Contact.Reason,
                ContactPreference = ContactPreference.emailHtml, // contactPreference.onlineInboxOnly; the contact preference is changed to send email from TransmitNotification
                OrganisationId = emailParams.Contact.OrganisationId,
                NotificationId = emailParams.NotificationId, //adding notification id to the notification contact object.
                OrganistationName = emailParams.Contact.Organisation // organisation name of contact . Needed in case of a manually added contact.
            };
            #endregion

            #region SaveDocument
            long transId = 0;
            ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivery, emailParams, notiContacts, ref transId);
            SaveDocumentParams saveDocumentParams = new SaveDocumentParams
            {
                NotificationId = emailParams.NotificationId,
                DocType = emailParams.DocType,
                OrganisationId = emailParams.Contact.OrganisationId,
                ESDALReferenceNo = emailParams.ESDALReferenceNo,
                ContactId = emailParams.Contact.ContactId,
                ExportByteArrayData = CompressXMLString,
                UserSchema = emailParams.UserInfo.UserSchema,
                UserInfo = emailParams.UserInfo,
                Contact = notiContacts,
                ProjectId = emailParams.Projectid,
                RevisionNo = emailParams.RevisionNo,
                VersionNo = emailParams.VersionNo
            };
            long docid = SaveDocument(saveDocumentParams);
            long transmissionId = docid;
            if (docid != 0)
            {
                ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivered, emailParams, notiContacts, ref transId, docid);
                transmissionId = transId;
                if (transmissionId != 0)
                {
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivery, emailParams, notiContacts, ref transId, docid, transmissionId);
                    byte[] content = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
                    CommunicationParams communicationParams = new CommunicationParams()
                    {
                        UserEmail = emailParams.Contact.Email,
                        Subject = "",
                        Content = content,
                        Attachment = emailgetParams.AttachmentData,
                        ESDALReference = emailParams.ESDALReferenceNo,
                        XMLAttach = emailParams.xmlAttach,
                        DocumentTypeName = "notification",
                        IsImminent = emailParams.ImminentMovestatus,
                        DocumentType = emailParams.DocType
                    };
                    bool mailStatus = communicationsService.SendGeneralmail(communicationParams);
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_forwarded, emailParams, notiContacts, ref transId, docid, transmissionId);
                    if (mailStatus)
                        ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivered, emailParams, notiContacts, ref transId, docid, transmissionId);
                    else
                        ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivery_fail, emailParams, notiContacts, ref transId, docid, transmissionId);
                }
                else
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivery_fail, emailParams, notiContacts, ref transId, docid, transmissionId);
            }
            else
                ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivery_fail, emailParams, notiContacts, ref transId, docid);
            #endregion

            #region Inbox Items
            //condition to check whether the objcontact is null or not the following code part is to save notification in inbox items table additional conditions can be added to prevent from saving into inbox item's
            if (emailParams.Contact != null && emailParams.Contact.OrganisationId != 0) 
            {
                emailParams.NotificationId = emailParams.DocumentFileName == "SpecialOrderProposal" ? 0 : emailParams.NotificationId;
                long inboxItemId = SaveInboxItems(emailParams.NotificationId, docid, emailParams.Contact.OrganisationId, emailParams.ESDALReferenceNo, emailParams.UserInfo.UserSchema, emailParams.IcaStatus, emailParams.ImminentMovestatus);

                if (inboxItemId != 0) //updating status of inbox items entry with failed or updated 
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.inbox_item_delivered_with_mail, emailParams, notiContacts, ref transId, docid, transmissionId);
                else
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.inbox_item_failed_with_mail, emailParams, notiContacts, ref transId, docid, transmissionId);
            }
            #endregion
        }
        private void DistributePdf(GenerateEmailParams pdfParams)
        {
            GenerateEmailgetParams emailgetParams = GenerateMovementPDF(pdfParams);
            byte[] CompressXMLString = XsltTransformer.CompressData(emailgetParams.AttachmentData);

            #region NotificationContacts
            NotificationContacts notiContacts = new NotificationContacts
            {
                ContactId = pdfParams.Contact.ContactId,
                ContactName = pdfParams.Contact.FullName,
                Email = pdfParams.Contact.Email,
                Fax = pdfParams.Contact.Fax,
                Reason = pdfParams.Contact.Reason,
                ContactPreference = ContactPreference.fax, // contactPreference.onlineInboxOnly; the contact preference is changed to send email from TransmitNotification
                OrganisationId = pdfParams.Contact.OrganisationId,
                NotificationId = pdfParams.NotificationId, //adding notification id to the notification contact object.
                ProjectId = pdfParams.Projectid,
                OrganistationName = pdfParams.Contact.Organisation // organisation name of contact . Needed in case of a manually added contact.
            };
            #endregion

            #region SaveDocument
            long transId = 0;
            ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivery, pdfParams, notiContacts, ref transId);
            SaveDocumentParams saveDocumentParams = new SaveDocumentParams
            {
                NotificationId = pdfParams.NotificationId,
                DocType = pdfParams.DocType,
                OrganisationId = pdfParams.Contact.OrganisationId,
                ESDALReferenceNo = pdfParams.ESDALReferenceNo,
                ContactId = pdfParams.Contact.ContactId,
                ExportByteArrayData = CompressXMLString,
                UserSchema = pdfParams.UserInfo.UserSchema,
                UserInfo = pdfParams.UserInfo,
                Contact = notiContacts,
                ProjectId = pdfParams.Projectid,
                RevisionNo = pdfParams.RevisionNo,
                VersionNo = pdfParams.VersionNo
            };
            long docid = SaveDocument(saveDocumentParams);
            long transmissionId = docid;
            if (docid != 0)
            {
                ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivered, pdfParams, notiContacts, ref transId, docid);
                transmissionId = transId;
                if (transmissionId != 0)
                {
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivery, pdfParams, notiContacts, ref transId, docid, transmissionId);
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_forwarded, pdfParams, notiContacts, ref transId, docid, transmissionId);
                    byte[] content = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
                    bool FaxStatus = communicationsService.SendFax(notiContacts, pdfParams.UserInfo, transmissionId, content);
                    if (FaxStatus)
                        ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivered, pdfParams, notiContacts, ref transId, docid, transmissionId);
                    else
                        ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivery_fail, pdfParams, notiContacts, ref transId, docid, transmissionId);
                }
                else
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.trans_delivery_fail, pdfParams, notiContacts, ref transId, docid, transmissionId);
            }
            else
                ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivery_fail, pdfParams, notiContacts, ref transId, docid);
            #endregion

            #region Inbox Items
            //condition to check whether the objcontact is null or not the following code part is to save notification in inbox items table additional conditions can be added to prevent from saving into inbox item's
            if (pdfParams.Contact != null && pdfParams.Contact.OrganisationId != 0) 
            {
                pdfParams.NotificationId = pdfParams.DocumentFileName == "SpecialOrderProposal" ? 0 : pdfParams.NotificationId;
                //finally saving into inbox items
                long inboxItemId = SaveInboxItems(pdfParams.NotificationId, docid, pdfParams.Contact.OrganisationId, pdfParams.ESDALReferenceNo, pdfParams.UserInfo.UserSchema, pdfParams.IcaStatus, pdfParams.ImminentMovestatus);// ADDED ImminentMovestatus FOR NEN PROJECT
                //updating status of inbox items entry with failed or updated 
                if (inboxItemId != 0) //updating status of inbox items entry with failed or updated 
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.inbox_item_delivered_without_mail, pdfParams, notiContacts, ref transId, docid, transmissionId);
                else
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.inbox_item_failed_without_mail, pdfParams, notiContacts, ref transId, docid, transmissionId);
            }
            #endregion
        }
        private void DistributeWord(GenerateEmailParams wordParams)
        {
            GenerateEmailgetParams emailgetParams = GenerateMovementWord(wordParams);

            #region NotificationContacts
            NotificationContacts notiContacts = new NotificationContacts();
            if (!(wordParams.DocumentFileName.Contains("2D") || wordParams.DocumentFileName.Contains("Amendment") || wordParams.DocumentFileName.Contains("FormVR1")))
            {
                notiContacts.ContactId = wordParams.Contact.ContactId;
                notiContacts.ContactName = wordParams.Contact.FullName;
                notiContacts.Email = wordParams.Contact.Email;
                notiContacts.Fax = wordParams.Contact.Fax;
                notiContacts.Reason = wordParams.Contact.Reason;
                notiContacts.ContactPreference = ContactPreference.onlineInboxOnly;
                notiContacts.OrganisationId = wordParams.Contact.OrganisationId; //adding organisationiD to the object for saving into active transactions folder
                notiContacts.OrganistationName = wordParams.Contact.Organisation; // organisation name of contact . Needed in case of a manually added contact.
            }
            #endregion

            int organisationId = wordParams.Contact.OrganisationId != 0 ? wordParams.Contact.OrganisationId : (int)wordParams.OrganisationId;
            byte[] CompressXMLString = XsltTransformer.CompressData(emailgetParams.AttachmentData);

            #region SaveDocument
            long transId = 0;
            ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivery, wordParams, notiContacts, ref transId);
            SaveDocumentParams saveDocumentParams = new SaveDocumentParams
            {
                NotificationId = wordParams.NotificationId,
                DocType = wordParams.DocType,
                OrganisationId = organisationId,
                ESDALReferenceNo = wordParams.ESDALReferenceNo,
                ContactId = wordParams.Contact.ContactId,
                ExportByteArrayData = CompressXMLString,
                UserSchema = wordParams.UserInfo.UserSchema,
                UserInfo = wordParams.UserInfo,
                Contact = notiContacts,
                ProjectId = wordParams.Projectid,
                RevisionNo = wordParams.RevisionNo,
                VersionNo = wordParams.VersionNo
            };
            long docid = SaveDocument(saveDocumentParams);
            if (docid != 0)
                ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivered, wordParams, notiContacts, ref transId, docid);
            else
                ProcessMovementActionAndDistStatus((int)DocumentDistribute.doc_delivery_fail, wordParams, notiContacts, ref transId);
            #endregion

            #region Inbox Items
            //condition to check whether the objcontact is null or not  the following code part is to save notification in inbox items table
            if (wordParams.Contact != null && wordParams.Contact.OrganisationId != 0) // && ImminentMovestatus == false  REMOVED FOR NEN PROJECT
            {
                //esDALRefNo is passed when docfile name is "SpecialOrderProposal" saving into inbox items table
                wordParams.NotificationId = wordParams.DocumentFileName == "SpecialOrderProposal" ? 0 : wordParams.NotificationId;
                long inboxItemId = SaveInboxItems(wordParams.NotificationId, docid, wordParams.Contact.OrganisationId, wordParams.ESDALReferenceNo, wordParams.UserInfo.UserSchema, wordParams.IcaStatus, wordParams.ImminentMovestatus);
                if (inboxItemId != 0) //updating status of inbox items entry with failed or updated 
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.inbox_item_delivered_without_mail, wordParams, notiContacts, ref transId, docid, docid);
                else
                    ProcessMovementActionAndDistStatus((int)DocumentDistribute.inbox_item_failed_without_mail, wordParams, notiContacts, ref transId, docid, docid);
            }
            #endregion
        }
        #endregion

        #region Proccess Movement Action
        private void ProcessMovementActionAndDistStatus(int actionFlag, GenerateEmailParams emailParams, NotificationContacts notiContacts, ref long transId, long documentId = 0, long transmissionId = 0)
        {
            string docTypeName = emailParams.DocType != 0 ? ((ItemDocType)emailParams.DocType).GetEnumDescription() : string.Empty;
            MovementActionIdentifiers movactiontype;
            SaveDistributionStatusParams saveDistributionStatus;
            switch (actionFlag)
            {
                case (int)DocumentDistribute.doc_delivery:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.outbound_doc_ready_for_delivery,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        ReciverContactName = emailParams.Contact.FullName,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts.ContactPreference,
                        DateTime = DateTime.Now,//HE-8207 - As we are storing it as a string without conversion, we don't need to do utc conversion here
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = null;
                    break;
                case (int)DocumentDistribute.doc_delivered:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.outbound_doc_delivered,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        ReciverContactName = emailParams.Contact.FullName,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts.ContactPreference,
                        DateTime = DateTime.Now,
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = new SaveDistributionStatusParams// Insertion in distirbution staus 
                    {
                        NotificationContacts = notiContacts,
                        Status = (int)DistibuteStat.initializing, //Trans,Overall & Inbox status as  310004(Initializing)
                        InboxOnly = notiContacts.ContactPreference == ContactPreference.onlineInboxOnly ? 1 : 0,
                        EsdalReference = emailParams.ESDALReferenceNo,
                        TransmissionId = documentId,
                        IsImminent = emailParams.ImminentMovestatus,
                        Username = emailParams.UserInfo.UserName
                    };
                    break;
                case (int)DocumentDistribute.trans_delivery:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.transmission_ready_for_delivery,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts!=null?notiContacts.ContactPreference:ContactPreference.onlineInboxOnly,
                        ReciverContactName = emailParams.Contact.FullName,
                        DateTime = DateTime.Now,
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = null;
                    break;
                case (int)DocumentDistribute.trans_forwarded:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.transmission_forwarded,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts != null ? notiContacts.ContactPreference : ContactPreference.onlineInboxOnly,
                        ReciverContactName = emailParams.Contact.FullName,
                        DateTime = DateTime.Now,
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = new SaveDistributionStatusParams //Update distribution status
                    {
                        Status = (int)DistibuteStat.sending, //Trans,Overall status as 310005(Sending)
                        InboxOnly = notiContacts.ContactPreference == ContactPreference.onlineInboxOnly ? 1 : 0,
                        TransmissionId = transmissionId
                    };
                    break;
                case (int)DocumentDistribute.trans_delivered:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.transmission_delivered,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts != null ? notiContacts.ContactPreference : ContactPreference.onlineInboxOnly,
                        ReciverContactName = emailParams.Contact.FullName,
                        DateTime = DateTime.Now,
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = new SaveDistributionStatusParams //Update distribution status
                    {
                        Status = (int)DistibuteStat.trans_delivered, //Trans,Overall status as 310001(Delivered)
                        InboxOnly = notiContacts.ContactPreference == ContactPreference.onlineInboxOnly ? 1 : 0,
                        TransmissionId = transmissionId
                    };
                    break;
                case (int)DocumentDistribute.trans_delivery_fail:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.transmission_delivery_failure,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts != null ? notiContacts.ContactPreference : ContactPreference.onlineInboxOnly,
                        ReciverContactName = emailParams.Contact.FullName,
                        DateTime = DateTime.Now,
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = new SaveDistributionStatusParams //Update distribution status
                    {
                        Status = (int)DistibuteStat.trans_failed,//Trans,Overall status as 310002(Failure)
                        InboxOnly = notiContacts.ContactPreference == ContactPreference.onlineInboxOnly ? 1 : 0,
                        TransmissionId = transmissionId
                    };
                    break;
                case (int)DocumentDistribute.doc_delivery_fail:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.outbound_doc_delivery_failure,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        ReciverContactName = emailParams.Contact.FullName,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts != null ? notiContacts.ContactPreference : ContactPreference.onlineInboxOnly,
                        TransmissionErrorMsg = "cannot save outbound document !! contact helpdesk",
                        DateTime = DateTime.Now,
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = null;
                    break;
                case (int)DocumentDistribute.inbox_item_delivered_with_mail:
                case (int)DocumentDistribute.inbox_item_delivered_without_mail:
                    movactiontype = new MovementActionIdentifiers
                    {
                        MovementActionType = MovementnActionType.inbox_item_delivered,
                        ProjectId = emailParams.Projectid,
                        ESDALRef = emailParams.ESDALReferenceNo,
                        OrganisationNameReceiver = emailParams.Contact.Organisation,
                        ContactPreference = notiContacts != null ? notiContacts.ContactPreference : ContactPreference.onlineInboxOnly,
                        ReciverContactName = emailParams.Contact.FullName,
                        DateTime = DateTime.Now,
                        TransmissionDocType = docTypeName
                    };
                    saveDistributionStatus = new SaveDistributionStatusParams //Update distribution status
                    {
                        Status = actionFlag == (int)DocumentDistribute.inbox_item_delivered_without_mail ? (int)DistibuteStat.without_mail_inbox_delivered : (int)DistibuteStat.with_mail_inbox_delivered, //Withmail Inbox and Without mail both Inbox & Overall status as 310001(Delivered)
                        InboxOnly = notiContacts.ContactPreference == ContactPreference.onlineInboxOnly ? 1 : 0,
                        TransmissionId = transmissionId
                    };
                    break;
                case (int)DocumentDistribute.inbox_item_failed_with_mail:
                case (int)DocumentDistribute.inbox_item_failed_without_mail:
                    movactiontype = null;
                    saveDistributionStatus = new SaveDistributionStatusParams //Update distribution status
                    {
                        Status = actionFlag == (int)DocumentDistribute.inbox_item_failed_without_mail ? (int)DistibuteStat.without_mail_inbox_failed : (int)DistibuteStat.with_mail_inbox_failed, //Withmail Inbox and Without mail both Inbox & Overall status as 310002(Failed)
                        InboxOnly = notiContacts.ContactPreference == ContactPreference.onlineInboxOnly ? 1 : 0,
                        TransmissionId = transmissionId
                    };
                    break;
                default:
                    movactiontype = null;
                    saveDistributionStatus = null;
                    break;
            }
            if (movactiontype != null)
            {
                string MovementDescription = MovementActions.GetMovementActionString(emailParams.UserInfo, movactiontype, out string ErrMsg);
                loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, emailParams.Projectid, emailParams.RevisionNo, emailParams.VersionNo, emailParams.UserInfo.UserSchema);
            }
            if(saveDistributionStatus != null)
                transId = SaveDistributionStatus(saveDistributionStatus);
                
        }
        #endregion

        #region Notifications

        #region Generate Esdal Notification
        private ESDALNotificationGetParams GenerateEsdalNotification(GenerateEsdalNotificationParams esdalNotificationParams)
        {
            ESDALNotificationGetParams getParams = new ESDALNotificationGetParams();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateEsdalNotification",
                          esdalNotificationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    getParams = response.Content.ReadAsAsync<ESDALNotificationGetParams>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateEsdalNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateEsdalNotification, Exception: {ex}");
            }

            return getParams;
        }

        #endregion

        #region Generate Esdal ReNotification
        private ESDALNotificationGetParams GenerateEsdalReNotification(GenerateEsdalNotificationParams esdalNotificationParams)
        {
            ESDALNotificationGetParams getParams = new ESDALNotificationGetParams();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateEsdalReNotification",
                          esdalNotificationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    getParams = response.Content.ReadAsAsync<ESDALNotificationGetParams>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateEsdalReNotification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateEsdalReNotification, Exception: {ex}");
            }

            return getParams;
        }
        #endregion

        #endregion

        #region SO Movements

        #region GenerateSODistributeDocument
        private SODistributeDocumentParams GenerateSODistributeDocument(SOProposalDocumentParams sOProposalDocument)
        {
            SODistributeDocumentParams sODistribute = new SODistributeDocumentParams();
            try
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                  $"/Document/GenerateSODistributeDocument", sOProposalDocument).Result;

                if (response.IsSuccessStatusCode)
                {
                    sODistribute = response.Content.ReadAsAsync<SODistributeDocumentParams>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateSODistributeDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"SORTDocument/GenerateSODistributeDocument, Exception: {0}", ex);
            }
            return sODistribute;
        }
        #endregion

        #region GetSoProposalXsltPath
        private SOProposalXsltPath GetSoProposalXsltPath(string ContactType, long ProjectStatus, string FinalReson)
        {
            SOProposalXsltPath sOProposalXslt = new SOProposalXsltPath();
            try
            {
                string urlParameters = "?ContactType=" + ContactType + "&ProjectStatus=" + ProjectStatus + "&FinalReson=" + FinalReson;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
               $"/Document/GetSoProposalXsltPath{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    sOProposalXslt = response.Content.ReadAsAsync<SOProposalXsltPath>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetSoProposalXsltPath, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetSoProposalXsltPath, Exception: {ex}");
            }
            return sOProposalXslt;
        }
        #endregion

        #region CopyMovementSortToPortal
        private long CopyMovementSortToPortal(MovementCopyDetails moveCopyDet, int movementCloneStatus, int versionid = 0, string EsdalReference = "", byte[] hacontactbytes = null, int organizationid = 0, string userSchema = UserSchema.Portal)
        {
            long pstatus = 0;
            try
            {
                CopyMovementSortToPortalInsertParams urlParams = new CopyMovementSortToPortalInsertParams
                {
                    MovementCopyDetail = moveCopyDet,
                    MovementCloneStatus = movementCloneStatus,
                    VersionID = versionid,
                    EsdalReference = EsdalReference,
                    HAContactBytes = hacontactbytes,
                    OrganizationID = organizationid,
                    ModelUserSchema = userSchema
                };
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/CopyMovementSortToPortal",
                          urlParams).Result;
                if (response.IsSuccessStatusCode)
                {

                    pstatus = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/CopyMovementSortToPortal, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/CopyMovementSortToPortal, Exception: {0}", ex);
            }
            return pstatus;

        }
        #endregion

        #endregion
        
        #region Common

        #region SaveDocument
        private long SaveDocument(SaveDocumentParams saveDocumentParams)
        {
            long docid = 0;
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/SaveDocument",
                          saveDocumentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    docid = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveDocument, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveDocument, Exception: {0}", ex);
            }
            return docid;

        }
        #endregion

        #region SaveDistributionStatus
        public long SaveDistributionStatus(SaveDistributionStatusParams saveDistributionStatus)
        {
            long distrStatus = 0;
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/SaveDistributionStatus",
                          saveDistributionStatus).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    distrStatus = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveDistributionStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveDistributionStatus, Exception: {0}", ex);
            }
            return distrStatus;
        }
        #endregion

        #region FetchContactPreference
        public string[] FetchContactPreference(int contactId, string userSchema)
        {
            string[] contactDet = new string[6];
            try
            {
                //api call to new service   
                string urlParameters = "?contactId=" + contactId + "&userSchema=" + userSchema;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/Document/FetchContactPreference{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    contactDet = response.Content.ReadAsAsync<string[]>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/FetchContactPreference, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/FetchContactPreference, Exception: {0}", ex);
            }
            return contactDet;

        }
        #endregion

        #region GetImminentForCountries
        private bool GetImminentForCountries(int Orgid, string ImminentStatus)
        {
            bool ImminentFlag = false;
            try
            {
                //api call to new service   
                string urlParameters = "?Orgid=" + Orgid + "&ImminentStatus=" + ImminentStatus;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                                                                  $"/Document/GetImminentForCountries{ urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    ImminentFlag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetImminentForCountries, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GetImminentForCountries, Exception: {0}", ex);
            }
            return ImminentFlag;

        }
        #endregion

        #region SaveInboxItems
        private long SaveInboxItems(int NotificationID, long documentId, int OrganisationID, string esDAlRefNo, string userSchema = UserSchema.Portal, int icaStatus = 277001, bool ImminentMovestatus = false)
        {
            long inboxId = 0;
            try
            {
                SaveInboxParams saveInboxParams = new SaveInboxParams
                {
                    NotificationId = NotificationID,
                    DocumentId = documentId,
                    OrganisationId = OrganisationID,
                    ESDALReferenceNo = esDAlRefNo,
                    UserSchema = userSchema,
                    IcaStatus = icaStatus,
                    ImminentMovestatus = ImminentMovestatus
                };
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/SaveInboxItems",
                          saveInboxParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    inboxId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveInboxItems, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/SaveInboxItems, Exception: {0}", ex);
            }
            return inboxId;

        }
        #endregion

        #endregion

        #region GenerateMovementPDF
        private GenerateEmailgetParams GenerateMovementPDF(GenerateEmailParams pdfParams)
        {
            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams();
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateNotificationPDF",
                          pdfParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    emailgetParams = response.Content.ReadAsAsync<GenerateEmailgetParams>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateNotificationPDF, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateNotificationPDF, Exception: {0}", ex);
            }
            return emailgetParams;

        }
        #endregion

        #region GenerateMovementWord
        private GenerateEmailgetParams GenerateMovementWord(GenerateEmailParams wordParams)
        {
            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams();
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateWord",
                          wordParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    emailgetParams = response.Content.ReadAsAsync<GenerateEmailgetParams>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateWord, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateWord, Exception: {0}", ex);
            }
            return emailgetParams;

        }
        #endregion

        #region GenerateEMAIL
        private GenerateEmailgetParams GenerateMovementEMAIL(GenerateEmailParams emailParams)
        {
            GenerateEmailgetParams emailgetParams = new GenerateEmailgetParams();
            try
            {
                //api call to new service   
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                          $"{ConfigurationManager.AppSettings["DocumentsAndContents"]}" +
                          $"/Document/GenerateEMAIL",
                          emailParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    emailgetParams = response.Content.ReadAsAsync<GenerateEmailgetParams>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateEMAIL, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Document/GenerateEMAIL, Exception: {0}", ex);
            }
            return emailgetParams;

        }
        #endregion
        
        #endregion

    }
}
