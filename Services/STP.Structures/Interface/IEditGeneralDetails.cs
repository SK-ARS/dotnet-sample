using STP.Domain;
using STP.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Structures.Interface
{
    public interface IEditGeneralDetails
    {
        /// <summary>
        /// Updates vehicle fleet component
        /// </summary>
        /// <param name="StructureGeneralDetails"></param>
        bool EditStructureGeneralDetails(StructureGeneralDetails structureGeneralDetails);
        List<SVDataList> GetSVData(long structureId, long sectionId);
        List<SVDataList> UpdateSVData(UpdateSVParams objUpdateSVParams);
        bool UpdateDefaultBanding(int OrgId, double? OrgMinWeight, double? OrgMaxWeight, double? OrgMinSV, double? OrgMaxSV, string userName);
        ConfigBandModel GetDefaultBanding(int OrgId, long structId, long sectionId);
    }
}
