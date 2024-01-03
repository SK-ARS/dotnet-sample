using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.SecurityAndUsers
{
   public  interface IOrganizationService
    {
      
        List<ViewOrganizationByID> ViewOrganizationByID(int orgId);
        int SaveOrganization(Organization orgDet);
        int EditOrganization(Organization orgDet);
        List<OrganizationTypeList> GetOrganisationTypeList();
        List<OrganizationGridList> GetOrganizationInformation(string searchString, int pageNumber, int pageSize, int userTypeId, string searchOrgCode, int sortOrder, int presetFilter);
        List<OrganizationGridList> GetOrganizationInformation(int pageNumber, int pageSize, int userTypeId, int sortOrder, int presetFilter);
        decimal GetOrganizationByName(string organisationName, int type, string mode, string organisationId);
        List<ViewOrganizationByID> ViewOrganisationByIDForSORT(int RevisionId);
        List<ContactModel> GetAffectedOrganisationDetails(string affectedParties, int affectedPartiesCount, string userSchema);
        List<ContactModel> GetNenAffectedOrganisationDetails(int inboxItemId);
    }
}
