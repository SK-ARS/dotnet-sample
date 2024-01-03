using STP.Domain;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Structures.Interface
{
    public interface IDelegationArrangementProvider
    {
        List<DelegationList> GetDelegArrangList(long organisationId, int pageNumber, int pageSize, string searchType, string searchValue, int presetFilter, int? sortOrder = null);
        DelegationList GetArrangement(long arrangid, int orgId);
        List<DelegationList> GetOrganisationList(int pageNumber, int pageSize, string Organisationname);
        List<DropDown> GetDelegationAutoFill(int organisationId, string delegationName);
        List<StructureInDelegationList> GetStructuresInDeleg(long arrangid, long orgId, int? pageNum, int? pageSize);
        List<DelegationList> GetContactList(int pageNumber, int pageSize, string Contactname, int orgID);
        List<RoadDelegationList> GetRoadDelegationList(int pageNum, int pageSize, long OrganisationId,int presetFilter, int? sortOrder = null);
        List<StructureInDelegationList> GetStructureInDelegationList(int pageNumber, int? pageSize, string structurecodes, int OrganisationId, int structurecodecount);
        List<StructureInDelegationList> GetStructureInDelegationList(string[] structurecodes, int OrganisationId);
        bool ManageStructureDelegation(DelegationList delegationList);
        bool ManageDelegationStructureContact(DelegationList structureContact);
        int CheckSubDelegationList(long structureID, long organisationID);
        int DeleteStructureEdit(long arrangementId);
        List<StructureContactsList> GetStructureContactList(long arrangementId);
        int DeleteStructInDelegation(long structId, long arrangId);
        DelegationList ManageDelegationArrangement(DelegationList savedelegation, long organisationId);
        int DeleteStructureContact(long StructureId, string StructureCode, long ArrangementId, long OwnerID);
        int DeleteDelegationArrangement(long arrangId);
       List<StructureSummary> GetNotDelegatedStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct,int sortOrder,int sortType);
        List<StructureDeleArrList> GetStructureDeleArrg(string StructureCode);


    }
}
