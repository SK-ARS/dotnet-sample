using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;
using STP.Domain.Structures;
namespace STP.Structures.Interface
{
    public interface IListStructureSummary
    {
        List<StructureSummary> GetStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct);

    }
}
