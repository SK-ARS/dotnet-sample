using STP.DocumentsAndContents.Common;
using STP.DocumentsAndContents.Document;
using STP.DocumentsAndContents.Interface;
using STP.DocumentsAndContents.Persistance;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Providers
{
    public class SORTDocumentProvider : ISORTDocument
    {
        #region ListMovement Singleton

        private SORTDocumentProvider()
        {
        }
        public static SORTDocumentProvider Instance
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
            internal static readonly SORTDocumentProvider instance = new SORTDocumentProvider();
        }
        #endregion
        public SORTSpecialOrder GetSpecialOrder(string orderId)
        {
            return SpecialOrderDAO.GetSORTSpecialOrder(orderId);
        }
        public List<SOCRouteParts> GetRouteVehicles(int movementVersionId, string vehicleId)
        {
            return SpecialOrderDAO.GetRouteVehicles(movementVersionId, vehicleId);
        }
        public List<SOCoverageDetails> GetSpecialOrderCoverages(int projectid, int state)
        {
            return SpecialOrderDAO.GetSpecialOrderCoverages(projectid, state);
        }
        #region ListSortUser
        public List<GetSORTUserList> ListSortUser(long userTypeId, int checkerType = 0)
        {
            return SORTApplicationDAO.ListSortUserAppl(userTypeId, checkerType);
        }
        #endregion
        public List<SORTMovementList> GetSpecialOrderList(long projectId)
        {
            return SORTApplicationDAO.GetSpecialOrderList(projectId);
        }
        //Delete sort special order
        public int DeleteSpecialOrder(string orderNo, string userSchema)
        {
            return SpecialOrderDAO.DeleteSpecialOrder(orderNo, userSchema);
        }
        public string SaveSortSpecialOrder(SORTSpecialOrder apprevisionId, List<string> RCoverages)
        {
            return SpecialOrderDAO.SaveNewSortSpecialOrder(apprevisionId, RCoverages);
        }

        public string UpdateSortSpecialOrder(SORTSpecialOrder model, List<string> removedCovrg)
        {
            return SpecialOrderDAO.UpdateSortSpecialOrder(model,removedCovrg);
        }
        public byte[] GenrateSODocument(Enums.SOTemplateType templatetype, string esdalRefNo, string orderNumber, UserInfo userInfo = null, bool generateFlag = true)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GenrateSODocument(templatetype, esdalRefNo, orderNumber,userInfo,generateFlag);
        }

        public byte[] GenerateHaulierAgreedRouteDocument(string esDALRefNo = "GCS1/25/2", string order_no = "P21/2012", int contactId = 8866, UserInfo SessionInfo = null)
        {
            GenerateDocument generateDocument = new GenerateDocument();
            return generateDocument.GenerateHaulierAgreedRouteDocument(Enums.DocumentType.PDF, esDALRefNo, order_no, contactId, SessionInfo);
        }

        public List<NotifRouteImport> ListNEBrokenRouteDetails(NotifRouteImportParams objNotifRouteImportParams)
        {
            return SORTApplicationDAO.ListNEBrokenRouteDetails(objNotifRouteImportParams);
        }
        public int GetNoofPages(String outputString)
        {
            return CommonMethods.GetNoofPages(outputString);
        }
    }
}