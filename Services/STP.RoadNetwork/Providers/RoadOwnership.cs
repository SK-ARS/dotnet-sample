using System.Collections.Generic;
using System.Diagnostics;
using STP.RoadNetwork.Interface;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.RoadNetwork.Persistance;
using NetSdoGeometry;
using STP.Common.Constants;

namespace STP.RoadNetwork.Providers
{
    public sealed class RoadOwnership : IRoadOwnership
    {
        #region RouteManagerProvider Singleton

        private RoadOwnership()
        {
        }
        public static RoadOwnership Instance
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
            internal static readonly RoadOwnership instance = new RoadOwnership();
        }
        #endregion

        #region Logger instance

        private const string PolicyName = "RoadOwnership";

        #endregion
        public List<RoadOwnershipOrgSummary> GetRoadOwnershipOrganisations(string orgName, int pageNum, int pageSize, int searchFlag)
        {
            return RoadOwnershipDAO.GetRoadOwnershipOrganisations(orgName, pageNum, pageSize, searchFlag);
        }
        public List<long> GetUnassignedLinks(List<long> linkIds)
        {
            return RoadOwnershipDAO.GetUnassignedLinks(linkIds);
        }
        public List<ArrangementDetails> GetDelegationArrangementDetails(int orgId)
        {
            return RoadOwnershipDAO.GetDelegationArrangementDetails(orgId);
        }
        public List<RoadContactModal> GetRoadOwnerContactList(long linkID, long length, string pageType, string userSchema = UserSchema.Portal)
        {
            return RoadOwnershipDAO.GetRoadOwnerContactList(linkID, length, pageType, userSchema);
        }
        public List<RoadOwnershipData> GetRoadOwnershipDetails(List<long> linkIds)
        {
            return RoadOwnershipDAO.GetRoadOwnershipDetails(linkIds);
        }
        public bool SaveRoadOwnership(RoadOwnerShipDetails roadOwnerObj)
        {
            return RoadOwnershipDAO.SaveRoadOwnership(roadOwnerObj);
        }
        public List<LinkInfo> FetchRoadInfoToDisplayOnMap(int organisationId, int fetchFlag, sdogeometry areaGeom, int zoomLevel)
        {
            return RoadOwnershipDAO.FetchRoadInfoToDisplayOnMap(organisationId, fetchFlag, areaGeom, zoomLevel);
        }
    }
}