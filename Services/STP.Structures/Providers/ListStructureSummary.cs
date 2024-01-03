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
    public sealed class ListStructureSummary : IListStructureSummary
    {
        #region ListStructureSummary Singleton

        private ListStructureSummary()
        {
        }
        public static ListStructureSummary Instance
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
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly ListStructureSummary instance = new ListStructureSummary();
        }

        #region Logger instance

        private const string PolicyName = "ListStructureSummary";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion


        #endregion

        public List<StructureSummary> GetStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct)
        {
            return StructureDAO.GetStructureListSearch(orgId, pageNum, pageSize, objSearchStruct);
        }

    }
}