using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using STP.SecurityAndUsers.Interface;
using STP.SecurityAndUsers.Persistance;
using System.Collections.Generic;
using System.Diagnostics;
namespace STP.SecurityAndUsers.Providers
{
    public class OrganizationProvider : IOrganizationProvider
    {
        #region OrganizationProvider Singleton
        private OrganizationProvider()
        {
        }
        public static OrganizationProvider Instance
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
            internal static readonly OrganizationProvider instance = new OrganizationProvider();
        }
        #region Logger instance
        private const string PolicyName = "OrganizationProvider";
        #endregion
        #endregion
        #region GetOrganizationInfo
        public List<OrganizationGridList> GetOrganizationInformation(string searchString, int pageNumber, int pageSize, int userTypeId, string searchOrgCode, int sortOrder, int presetFilter)
        {
            return OrganizationDAO.GetOrganizationInformation(searchString, pageNumber, pageSize, userTypeId, searchOrgCode, sortOrder, presetFilter);
        }
        #endregion
        public List<OrganizationGridList> GetOrganizationInformation(int pageNumber, int pageSize, int userTypeId, int sortOrder, int presetFilter)
        {
            return OrganizationDAO.GetOrganizationInformation(pageNumber, pageSize, userTypeId, sortOrder, presetFilter);
        }
        #region Save Organization
        public int SaveOrganization(Organization orgDet)
        {
            return OrganizationDAO.SaveOrganization(orgDet);
        }
        #endregion
        #region Edit Organization
        public int EditOrganization(Organization orgDet)
        {
            return OrganizationDAO.EditOrganization(orgDet);
        }
        #endregion
        #region GetOrganisationTypeList
        public List<OrganizationTypeList> GetOrganizationTypeList()
        {
            return OrganizationDAO.GetOrganizationTypeList();
        }
        #endregion

        public decimal GetOrganizationByName(string organisationName, int type, string mode, string organisationId)
        {
            return OrganizationDAO.SearchOrganisationByName(organisationName, type, mode, organisationId);
        }
        public ValidateAuthentication ValidateAuthentication(string authenticationKey)
        {
            return OrganizationDAO.ValidateAuthentication(authenticationKey);
        }
        public List<ViewOrganizationByID> ViewOrganisationByIDForSORT(int RevisionId)
        {
            return OrganizationDAO.ViewOrganisationByIDForSORT(RevisionId);
        }
        public List<ContactModel> GetAffectedOrganisationDetails(string affectedParties, int affectedPartiesCount, string userSchema)
        {
            return OrganizationDAO.GetAffectedOrganisationDetails(affectedParties, affectedPartiesCount, userSchema);
        }
        public List<ContactModel> GetNenAffectedOrganisationDetails(int inboxItemId)
        {
            return OrganizationDAO.GetNenAffectedOrganisationDetails(inboxItemId);
        }
    }
}