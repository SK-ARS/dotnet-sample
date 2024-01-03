using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.MovementsAndNotifications.Providers
{
    public class DispensationProvider : IDispensation
    {
        #region
        private DispensationProvider()
        {
        }
        internal static DispensationProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly DispensationProvider instance = new DispensationProvider();
        }
        #region Logger instance

        private const string PolicyName = "DispensationProvider";

        #endregion
        #endregion

        #region GetAffDispensationInfo
        public List<DispensationGridList> GetAffDispensationInfo(int organisationId, int granteeId, int pageNumber, int pageSize, int userType)
        {
            return DispensationDAO.GetAffDispensationInfo(organisationId, granteeId, pageNumber, pageSize, userType);
        }
        #endregion

        #region GetSummaryListCount
        public int GetSummaryListCount(int organisationId, int userType)
        {
            return DispensationDAO.DispensationListCount(organisationId, userType);
        }
        #endregion

        #region GetDispensationInfo
        public List<DispensationGridList> GetDispensationInfo(int organisationId, int pageNumber, int pageSize, int userType,int presetFilter,int? sortOrder=null)
        {
            return DispensationDAO.GetDispensationInfo(organisationId, pageNumber, pageSize, userType,presetFilter,sortOrder);
        }
        #endregion

        #region GetDispensationSearchInfo
        public List<DispensationGridList> GetDispensationSearchInfo(int organisationId, int pageNumber, int pageSize, string DRefNo, string summary, string grantedBy, string description, int isValid, int chckcunt, int userType, int presetFilter = 1, int? sortOrder = null)
        {
            return DispensationDAO.GetDispensationSearchList(organisationId, pageNumber, pageSize, DRefNo, summary, grantedBy, description, isValid, chckcunt, userType,presetFilter,sortOrder);
        }
        #endregion

        #region ViewDispensationInfoByDRN
        public DispensationGridList ViewDispensationInfoByDRN(string DRN, int userTypeId)
        {
            return DispensationDAO.ViewDispensationInfoByDRN(DRN, userTypeId);
        }
        #endregion

        #region ViewDispensationInfo
        public DispensationGridList ViewDispensationInfo(int dispensationId, int userTypeId)
        {
            return DispensationDAO.ViewDispensationInfo(dispensationId, userTypeId);
        }
        #endregion

        #region GetDispensationDetailsObjByID
        public DispensationGridList GetDispensationDetailsObjByID(int dispensationId, int userTypeId)
        {
            return DispensationDAO.GetDispensationDetailsObjByID(dispensationId, userTypeId);
        }
        #endregion

        #region UpdateDispensation
        public int UpdateDispensation(UpdateDispensationParams updateDispensation)
        {
            return DispensationDAO.UpdateDispensation(updateDispensation.RegisterDispensation, updateDispensation.UserTypeId);
        }
        #endregion

        #region DeleteDispensation
        public int DeleteDispensation(int dispensationId)
        {
            return DispensationDAO.DeleteDispensation(dispensationId);
        }
        #endregion

        #region SaveDispensation
        public bool SaveDispensation(UpdateDispensationParams updateDispensation)
        {
            return DispensationDAO.SaveDispensation(updateDispensation.RegisterDispensation, updateDispensation.UserTypeId);
        }
        #endregion

        #region GetDispOrganisationInfo
        public List<DispensationGridList> GetDispOrganisationInfo(string organisationName, int pageNumber, int pageSize, int chckcunt, int userType)
        {
            return DispensationDAO.GetDispOrganisationInfo(organisationName, pageNumber, pageSize, chckcunt, userType);

            #endregion
        }
        #region GetDispensationDetailsByID
        public List<DispensationGridList> GetDispensationDetailsByID(int dispensationId)
        {
            return DispensationDAO.GetDispensationDetailsByID(dispensationId);
        }

        #endregion

        public decimal GetDispensationReferenceNumber(string dispensationReferenceNo, int organisationId, string mode, long dispensationId)
        {
            return DispensationDAO.SearchDispensationReferenceNumber(dispensationReferenceNo, organisationId, mode, dispensationId);
        }
    }
}