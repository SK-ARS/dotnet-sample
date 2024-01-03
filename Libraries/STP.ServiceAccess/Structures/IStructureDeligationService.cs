using STP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Structures;
namespace STP.ServiceAccess.Structures
{
    public interface IStructureDeligationService
    {
        List<DelegationList> GetDelegArrangList(long organisationId, int pageNumber, int pageSize, string searchType, string searchValue, int presetFilter, int? sortOrder = null);
        List<DelegationList> GetDelegArrangList(int organisationId, int pageNumber, int pageSize, string arrangName);
        DelegationList GetArrangement(long arrangementId, int organizationId);
        List<DelegationList> GetOrganisationList(int pageNumber, int pageSize, string organizationName);
        List<DropDown> GetDelegationAutoFill(int organisationId, string delegationName);
        List<StructureInDelegationList> GetStructuresInDeleg(long arrangementId, long organizationId, int? pageNumber, int? pageSize);
        List<DelegationList> GetContactList(int pageNumber, int pageSize, string contactName, int organizationId);
        List<RoadDelegationList> GetRoadDelegationList(int pageNumber, int pageSize, long organisationId,int presetFilter, int? sortOrder = null);
        List<StructureInDelegationList> GetStructureInDelegationList(int pageNumber, int? pageSize, string structurecodes, int OrganisationId, int structurecodecount);
        List<StructureInDelegationList> GetStructureInDelegationList(string[] structurecodes, int OrganisationId);
        bool ManageStructureDelegation(DelegationList delegationList);
        bool ManageDelegationStructureContact(DelegationList delegationList);
        int CheckSubDelegationList(long structureID, long organisationID);
        bool DeleteStructureEdit(long arrangementId);
        List<StructureContactsList> GetStructureContactList(long arrangementId);
        bool DeleteStructInDelegation(long structId, long arrangId);
        List<DelegationArrangment> viewDelegationArrangment(long organisationId);
        List<StructureSectionList> viewUnsuitableStructSections(long structureId, long route_part_id, long section_id, string cont_ref_num);
        long GetStructureId(string structureCode);
        DelegationList ManageDelegationArrangement(DelegationList savedelegation, long organisationId);
        bool DeleteStructureContact(short CONTACT_NO, long CautionId);
        bool DeleteStructureContact(StructureContactsList structContactList);
        bool DeleteDelegationArrangement(long arrangId);
        List<StructureSummary> GetNotDelegatedStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct,int? sortOrder=null,int? sortType=null);

        List<StructureDeleArrList> GetStructureDeleArrg(string structureCode);
    }
}
