using AggreedRouteXSD;
using STP.Common.Constants;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.DocumentsAndContents.Interface
{
    public interface IDocumentConsole
    {
        byte[] GenerateHaulierProposedRouteDocument(string ESDALReferenceNo, int organisationId, int contactId, string userSchema = UserSchema.Portal, UserInfo sessionInfo = null);
        byte[] GeneratePDF(int notificationId, int docType, string xmlInformation, string fileName, string ESDALReferenceNo, long organisationId, int contactId, string docFileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "");
        string GenerateHTMLPDF(int notificationId, int docType, string xmlInformation, string fileName, string ESDALReferenceNo, long organisationId, int contactId, string docFileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "");
        string[] FetchContactPreference(int contactId, string userSchema);
        bool GetImminentForCountries(int Orgid, string ImminentStatus);
        ESDALNotificationGetParams GenerateEsdalNotification(int notificationId, int contactId);
        ESDALNotificationGetParams GenerateEsdalReNotification(int notificationId, int contactId);
        SODistributeDocumentParams GenerateSODistributeDocument(string esDALRefNo, int organisationID, int contactId, string distributionComments, int versionid, Dictionary<int, int> icaStatusDictionary, string EsdalReference, HAContact hacontact, AgreedRouteStructure agreedroute, string userSchema = UserSchema.Portal, int routePlanUnits = 692001, long ProjectStatus = 0, int versionNo = 0, MovementPrint moveprint = null, decimal PreVersionDistr = 0, UserInfo sessioninfo = null);
        SOProposalXsltPath GetSoProposalXsltPath(string ContactType, long ProjectStatus, string FinalReson);

        #region Commented Code by Mahzeer on 12/07/2023
        //void SaveDetailOutboundNotification(long notificationId, byte[] CompressData);
        //string GeneratePDF1(int notificationId, int docType, string xmlInformation, string fileName, string ESDALReferenceNo, long organisationId, int contactId, string docFileName, bool isHaulier = false, string organisationName = "", string HAReference = "", int routePlanUnits = 692001, string documentType = "PDF", UserInfo userInfo = null, string userType = "");
        #endregion
    }
}
