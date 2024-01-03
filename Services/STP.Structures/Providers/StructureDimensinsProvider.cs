using STP.Domain;
using STP.Structures.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Domain.Structures;
namespace STP.Structures.Providers
{
    public static class StructureDimensinsProvider
    {        
        public static List<StucDDList> GetSTRUCT_DD(int type)
        {
            return StructureDAO.STRUCT_DD(type);
        }

        public static bool GetEditStructureImposed(ImposedConstraints objStructureImpoConst, int structureId, int sectionId, int structType)
        {
            return StructureDAO.EditStructureImposed(objStructureImpoConst, structureId, sectionId, structType);
        }
        public static bool EditDimensionConstraints(DimensionConstruction objStructureDimension, int structureId, int sectionId)
        {
            return StructureDAO.EditDimensionConstraints(objStructureDimension, structureId, sectionId);
        }
        public static int SaveStructureSpan(SpanData objSpanData, long StructureID, long SectionID, int editSaveFlag)
        {
            return StructureDAO.SaveStructureSpan(objSpanData, StructureID, SectionID, editSaveFlag);
        }
        public static int DeleteStructureSpan(long StructureID, long SectionID, long spanNo, string userName)
        {
            return StructureDAO.DeleteStructureSpan(StructureID, SectionID, spanNo, userName);
        }


    }
}