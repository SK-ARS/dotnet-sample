using STP.Structures.Interface;
using STP.Domain;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.Structures.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.Structures;
using STP.Common.Constants;


using static STP.Structures.Persistance.StructureManager;

namespace STP.Structures.Providers
{
    public class StructureManagerProvider : IStructureManager
    {
        #region RouteManagerProvider Singleton

        private StructureManagerProvider()
        {
        }
        public static StructureManagerProvider Instance
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
            internal static readonly StructureManagerProvider instance = new StructureManagerProvider();
        }
        #endregion
        public List<StructureInfo> MyStructureInfoList(int organisationId, int otherOrganisation, int left, int right, int bottom, int top)
        {
            return Structures.Persistance.StructureManager.GetMyStructureInfoList(organisationId, otherOrganisation, left, right, bottom, top);
        }

        public List<StructureContact> GetStructureContactListInfo(long structureId, string userSchema = UserSchema.Portal)
        {
            return Structures.Persistance.StructureManager.GetMyStructureContactList(structureId, userSchema);
        }

        public List<RoadContactModal> GetRoadContactList(long linkId, long length, string userSchema = UserSchema.Portal)
        {
            return Structures.Persistance.StructureManager.GetRoadContactList(linkId, length, userSchema);
        }

        public List<StructureInfo> AgreedAppStructureInfo(string StructureCode)
        {
            return Structures.Persistance.StructureManager.getAgreedAppStructureInfo(StructureCode);
        }
        public int GetStructureOwner(long structId, long orgId)
        {
            return Structures.Persistance.StructureManager.GetStructureOwner(structId, orgId);
        }

        public int GetStructureId(string structCode)
        {
            return Structures.Persistance.StructureManager.GetStructureId(structCode);
        }

    }
}