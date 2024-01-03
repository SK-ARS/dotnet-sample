using STP.Domain.ExternalAPI;
using STP.Domain.SecurityAndUsers;
using System.Collections.Generic;

namespace STP.SecurityAndUsers.Interface
{
    public interface IOrganizationProvider
    {
        List<OrganizationGridList> GetOrganizationInformation(string searchString, int pageNumber, int pageSize, int userTypeId,string searchOrgCode, int sortOrder, int presetFilter);
        List<OrganizationGridList> GetOrganizationInformation(int pageNumber, int pageSize, int userTypeId, int sortOrder, int presetFilter);
        List<OrganizationTypeList> GetOrganizationTypeList();
        int SaveOrganization(Organization orgDet);
        int EditOrganization(Organization orgDet);
        decimal GetOrganizationByName(string organisationName, int type, string mode, string organisationId);
        List<ViewOrganizationByID> ViewOrganisationByIDForSORT(int RevisionId);
        ValidateAuthentication ValidateAuthentication(string authenticationKey);
        List<ContactModel> GetAffectedOrganisationDetails(string affectedParties, int affectedPartiesCount, string userSchema);
        List<ContactModel> GetNenAffectedOrganisationDetails(int inboxItemId);
    }
}
