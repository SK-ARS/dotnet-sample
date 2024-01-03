using STP.Domain;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Structures.Interface
{
    public interface IGetStructureDetailsModel
    {
        List<DropDown> GetDelegationArrangement(int organisationId, string delegationArrangement);
        int CheckStructAgainstOrg(long structureId, long organisationId);
        List<StructureGeneralDetails> ViewGeneralDetails(long structureId);
        SpanData ViewSpanDataByNo(long structureId, long sectionId, long? spanNo);
        DimensionConstruction ViewDimensionConstruction(int structureId, int sectionId);
        List<SpanData> ViewSpanData(int structureId, int sectionId);
        List<StructureSectionList> ViewStructureSections(long structureId);
        ManageStructureICA ViewEnabledICA(int structureId, int sectionId, long OrgID);
        List<SvReserveFactors> viewSVData(int structureId, int sectionId);
        List<DelegationArrangment> viewDelegationArrangment(long organisationId);
        List<StructureSectionList> viewUnsuitableStructSections(long structureId, long route_part_id, long section_id, string cont_ref_num);
        long GetStructureId(string structureCode);

    }
}
