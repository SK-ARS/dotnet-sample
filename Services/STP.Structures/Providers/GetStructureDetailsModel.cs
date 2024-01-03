using STP.Structures.Interface;
using STP.Domain;
using STP.Structures.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.Structures;

namespace STP.Structures.Providers
{
    public sealed class GetStructureDetailsModel : IGetStructureDetailsModel
    {
        #region GetStructureDetailsModel Singleton
        private GetStructureDetailsModel()
        {
        }
        public static GetStructureDetailsModel Instance
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
            internal static readonly GetStructureDetailsModel instance = new GetStructureDetailsModel();
        }
        
        #endregion
        public List<DropDown> GetDelegationArrangement(int organisationId, string delegationArrangement)
        {
            return StructureDAO.GetDelegationArrangementList(organisationId, delegationArrangement);
        }
        public int CheckStructAgainstOrg(long structureId, long organisationId)
        {
            return StructureDAO.CheckStructAgainstOrg(structureId, organisationId);
        }
        public int CheckStructOrg(int organisationId, long structureId)
        {
            return StructureDAO.CheckStructOrg(structureId, organisationId);
        }
        public DimensionConstruction ViewDimensionConstruction(int structureId, int sectionId)
        {
            return StructureDAO.viewDimensionConstruction(structureId, sectionId);
        }
        public List<SpanData> ViewSpanData(int structureId, int sectionId)
        {
            return StructureDAO.ViewSpanData(structureId, sectionId);
        }
        public List<StructureGeneralDetails> ViewGeneralDetails(long structureId)
        {
            return StructureDAO.ViewGeneralDetails(structureId);
        }
        public List<StructType> getStructType(int type)
        {
            return StructureDAO.getStructType(type);
        }
        public List<StructCategory> getStructCategory(int type)
        {
            return StructureDAO.getStructCategory(type);
        }
        public ImposedConstraints ViewimposedConstruction(int structureId, int sectionId)
        {
            return StructureDAO.ViewImposedConstruction(structureId, sectionId);
        }
        public SpanData ViewSpanDataByNo(long structureId, long sectionId, long? spanNo)
        {
            return StructureDAO.ViewSpanDataByNo(structureId, sectionId, spanNo);
        }
        public List<StructureSectionList> ViewStructureSections(long structureId)
        {
            return StructureDAO.ViewStructureSections(structureId);
        }
        public ManageStructureICA ViewEnabledICA(int structureId, int sectionId, long OrgID)
        {
            return StructureDAO.viewEnabledICA(structureId, sectionId, OrgID);
        }
        public List<SvReserveFactors> viewSVData(int structureId, int sectionId)
        {
            return StructureDAO.viewSVData(structureId, sectionId);
        }
        public List<DelegationArrangment> viewDelegationArrangment(long organisationId)
        {
            return StructureDAO.viewDelegationArrangment(organisationId);
        }
        public List<StructureSectionList> viewUnsuitableStructSections(long structureId, long route_part_id, long section_id, string cont_ref_num)
        {
            return StructureDAO.viewUnsuitableStructSections(structureId, route_part_id, section_id, cont_ref_num);
        }
        public long GetStructureId(string structureCode)
        {
            return StructureDAO.GetStructureId(structureCode);
        }
    }
}