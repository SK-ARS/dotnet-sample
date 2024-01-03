using AggreedRouteXSD;
using STP.Common.Constants;
using STP.DocumentsAndContents.Common;
using STP.DocumentsAndContents.Document;
using STP.DocumentsAndContents.Interface;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using System.Collections.Generic;
using System.Diagnostics;

namespace STP.DocumentsAndContents.Providers
{
    public class DocumentConsole : IDocumentConsole
    {
        #region DocumentConsole Singleton

        private DocumentConsole()
        {
        }
        public static DocumentConsole Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly DocumentConsole instance = new DocumentConsole();
        }
        #endregion

        #region GenerateHaulierProposedRouteDocument
        public byte[] GenerateHaulierProposedRouteDocument(string ESDALReferenceNo, int organisationId, int contactId, string userSchema = UserSchema.Portal, UserInfo sessionInfo = null)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GenerateHaulierProposedRouteDocument(ESDALReferenceNo,organisationId,contactId,userSchema,sessionInfo);
        }
        #endregion

        #region Generate PDF
        public byte[] GeneratePDF(int notificationId, int docType, string xmlInformation, string fileName, string ESDALReferenceNo, long organisationId, int contactId, string docFileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {
            CommonMethods commonMethods = new CommonMethods();
            return commonMethods.GeneratePDF(notificationId, docType, xmlInformation, fileName, ESDALReferenceNo, organisationId, contactId, docFileName, isHaulier, organisationName, HAReference, routePlanUnits, documentType, userInfo, userType);

        }
        #endregion

        #region Generate PDF
        public string GenerateHTMLPDF(int notificationId, int docType, string xmlInformation, string fileName, string ESDALReferenceNo, long organisationId, int contactId, string docFileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {
            CommonMethods commonMethods = new CommonMethods();
            return commonMethods.GenerateHTMLPDF(notificationId, docType, xmlInformation, fileName, ESDALReferenceNo, organisationId, contactId, docFileName, isHaulier, organisationName, HAReference, routePlanUnits, documentType, userInfo, userType);

        }
        #endregion

        #region GetLoggedInUserAffectedStructureDetailsByESDALReference
        public string GetLoggedInUserAffectedStructureDetailsByESDALReference(string xmlInformation, string esDALRefNo, UserInfo SessionInfo, string userSchema, string type, int organisationId)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esDALRefNo, SessionInfo, userSchema, type, organisationId);

        }
        #endregion

        #region Generate ESDAL Notification
        public ESDALNotificationGetParams GenerateEsdalNotification(int notificationId, int contactId)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GenerateEsdalNotification(notificationId, contactId);

        }
        #endregion

        #region Generate ESDAL ReNotification
        public ESDALNotificationGetParams GenerateEsdalReNotification(int notificationId, int contactId)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GenerateEsdalReNotification(notificationId, contactId);

        }
        #endregion

        #region GetSOAPoliceContactList
        public List<ContactModel> GetSOAPoliceContactList(XMLModel modelSOAPolice)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GetSOAPoliceContactList(modelSOAPolice);

        }
        #endregion

        #region FetchContactPreference
        public string[] FetchContactPreference(int contactId, string userSchema)
        {
            return RouteAssessmentDAO.FetchContactPreference(contactId, userSchema);
        }
        #endregion

        #region GetImminentForCountries
        public bool GetImminentForCountries(int Orgid, string ImminentStatus)
        {
            return ImminentMoveAlertDAO.GetImminentForCountries(Orgid, ImminentStatus);
        }
        #endregion

        #region GenerateSODistributeDocument
        public SODistributeDocumentParams GenerateSODistributeDocument(string esDALRefNo, int organisationID, int contactId, string distributionComments, int versionid, Dictionary<int, int> icaStatusDictionary, string EsdalReference, HAContact hacontact, AgreedRouteStructure agreedroute, string userSchema = UserSchema.Portal, int routePlanUnits = 692001, long ProjectStatus = 0, int versionNo = 0, MovementPrint moveprint = null, decimal PreVersionDistr = 0, UserInfo sessioninfo = null)
        {
            return GenerateDocument.GenerateSODistributeDocument(esDALRefNo, organisationID, contactId, distributionComments, versionid, icaStatusDictionary, EsdalReference, hacontact, agreedroute, userSchema, routePlanUnits, ProjectStatus, versionNo, moveprint, PreVersionDistr, sessioninfo);
        }
        #endregion

        #region GetSoProposalXsltPath
        public SOProposalXsltPath GetSoProposalXsltPath(string ContactType, long ProjectStatus, string FinalReson)
        {
            return GenerateDocument.GetSoProposalXsltPath(ContactType, ProjectStatus, FinalReson);
        }
        #endregion

        #region GetOutboundNotificationStructureData
        public NotificationXSD.OutboundNotificationStructure GetOutboundNotificationStructureData(int NotificationId, bool isHaulier, long ContactId, int OrganisationId=0, int IsNen=0)
        {
            NotificationXSD.OutboundNotificationStructure obns = OutBoundDAO.GetOutboundNotificationDetailsForNotification(NotificationId, isHaulier,  ContactId, OrganisationId, IsNen);
            obns.IsFailedDelegationAlert = false;
            return obns;
        }
        #endregion

        #region GetOutboundReNotificationStructureData
        public NotificationXSD.OutboundNotificationStructure GetOutboundReNotificationStructureData(Enums.PortalType psPortalType, int NotificationID, bool isHaulier, int contactId)
        {
            NotificationXSD.OutboundNotificationStructure obns = OutBoundDAO.GetOutboundNotificationDetailsForNotification(NotificationID, isHaulier, contactId);
            obns.IsFailedDelegationAlert = false;
            obns.Type = NotificationXSD.NotificationTypeType.police;
            return obns;
        }

        #endregion

        #region Generate PDF
        public byte[] GeneratePDFFromHtmlString(string htmlString)
        {
            CommonMethods commonMethods = new CommonMethods();
            return commonMethods.GeneratePDFFromHtmlString(htmlString);

        }
        #endregion

        #region Commented Code by Mahzeer on 12/07/2023
        /*
        public void SaveDetailOutboundNotification(long notificationId, byte[] CompressData)
        {
            CommonMethods.SaveDetailOutboundNotification(notificationId, CompressData);
        }
        public List<byte[]> OldGenerateEsdalReNotification(int notificationId, int contactId, Dictionary<int, int> icaStatusDictionary, string ImminentMovestatus = "No imminent movement", UserInfo userInfo = null)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.OldGenerateEsdalReNotification(notificationId, contactId, icaStatusDictionary, ImminentMovestatus, userInfo);
        }
        public ProposedRouteXSD.ProposalStructure GetProposalDocument(string esDAlRefNo, int organisationID, int contactId, string userSchema)
        {
            ProposedRouteXSD.ProposalStructure ps = ProposalDAO.GetProposalRouteDetails(esDAlRefNo, organisationID, contactId, userSchema);
            ps.IsFailedDelegationAlert = false;
            return ps;
        }

        public ProposedRouteXSD.ProposalStructure GetReProposalDocument(int ProjectID, Enums.PortalType psPortalType, int ContactID, int versionNo, string userSchema)
        {
            ProposedRouteXSD.ProposalStructure ps = ReProposalStillAffectedDAO.GetProposalRouteDetails(ProjectID, psPortalType, ContactID, versionNo, userSchema);
            ps.IsFailedDelegationAlert = false;
            return ps;
        }

        public NoLongerAffectedXSD.NoLongerAffectedStructure GetReProposalNoLongerAffectedFAXSOA(int ProjectID, Enums.PortalType psPortalType, int ContactID, int versionNo, string userSchema = UserSchema.Sort)
        {
            NoLongerAffectedXSD.NoLongerAffectedStructure obns = ReProposalNoLongerAffectedDAO.GetNoLongerAfftectedDetailsForNotification(ProjectID, psPortalType, ContactID, versionNo, userSchema);
            return obns;
        }

        public AggreedRouteXSD.AgreedRouteStructure GetAgreedRoute(string esDAlRefNo, string orderNo = "0", int contactId = 0, string distributionComments = "", string userSchema = UserSchema.Sort)//RM#3646
        {
            AggreedRouteXSD.AgreedRouteStructure ars = RevisedAgreementDAO.GetRevisedAgreementDetails(orderNo, esDAlRefNo, contactId, userSchema);
            ars.DistributionComments = distributionComments;//RM#3646
            ars.IsFailedDelegationAlert = false;
            return ars;
        }
        public string GeneratePDF1(int notificationId, int docType, string xmlInformation, string fileName, string ESDALReferenceNo, long organisationId, int contactId, string docFileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "")
        {
            CommonMethods commonMethods = new CommonMethods();
            return commonMethods.GeneratePDF1(notificationId, docType, xmlInformation, fileName, ESDALReferenceNo, organisationId, contactId, docFileName, isHaulier, organisationName, HAReference, routePlanUnits, documentType, userInfo, userType);

        }
        */
        #endregion
    }
}