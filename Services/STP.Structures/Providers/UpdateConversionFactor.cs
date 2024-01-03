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
    public sealed class UpdateConversionFactor : IUpdateConversionFactor
    {
        #region UpdateConversionFactor Singleton
        private UpdateConversionFactor()
        {
        }
        public static UpdateConversionFactor Instance
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
            internal static readonly UpdateConversionFactor instance = new UpdateConversionFactor();
        }
       
        #endregion
        #region UpdateConversionFactor GetHBRatings
        public List<double?> GetHBRatings(long structureId, long sectionId)
        {
            return StructureDAO.GetHBRatings(structureId, sectionId);
        }
        #endregion
        #region UpdateConversionFactor GetCalculatedHBToSV
        public List<SvReserveFactors> GetCalculatedHBToSV(long structureId, long sectionId, double? hbWithLoad, double? hbWithoutLoad, int saveFlag, string userName)
        {
            return StructureDAO.HbToSvConversion(structureId, sectionId, hbWithLoad, hbWithoutLoad, saveFlag, userName);
        }
        #endregion
    }
}