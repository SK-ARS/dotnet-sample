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
    public sealed class EditGeneralDetails : IEditGeneralDetails
    {
        #region EditGeneralDetails Singleton
        private EditGeneralDetails()
        {
        }
        public static EditGeneralDetails Instance
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
            internal static readonly EditGeneralDetails instance = new EditGeneralDetails();
        }
        
        #endregion
        #region Edit Stucture General Details implementation
        public bool EditStructureGeneralDetails(StructureGeneralDetails structureGeneralDetails)
        {
            return StructureDAO.EditGeneralDetails(structureGeneralDetails);
        }
        #endregion
        #region Get_SVData
        public List<SVDataList> GetSVData(long structureId, long sectionId)
        {
            return StructureDAO.GetSVData(structureId, sectionId);
        }
        #endregion
        #region UpdateSVData
        public List<SVDataList> UpdateSVData(UpdateSVParams objUpdateSVParams)
        {
            return StructureDAO.UpdateSVData(objUpdateSVParams);
        }
        #endregion
        #region UpdateDefaultBanding
        public bool UpdateDefaultBanding(int OrgId, double? OrgMinWeight, double? OrgMaxWeight, double? OrgMinSV, double? OrgMaxSV, string UserName)
        {
            return StructureDAO.SaveDefaultConfig(OrgId, OrgMinWeight, OrgMaxWeight, OrgMinSV, OrgMaxSV, UserName);
        }
        #endregion

        #region GetDefaultBanding
        public ConfigBandModel GetDefaultBanding(int OrgId, long structId, long sectionId)
        {
            return StructureDAO.GetDefaultConfigData(OrgId, structId, sectionId);
        }
        #endregion
    }
}