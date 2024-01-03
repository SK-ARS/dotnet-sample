using STP.Domain.MovementsAndNotifications.HaulierMovementsAPI;
using STP.Domain.MovementsAndNotifications.SOAPoliceMovementsAPI;
using STP.Domain.MovementsAndNotifications.SORTMovementsAPI;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System.Diagnostics;

namespace STP.MovementsAndNotifications.Providers
{
    public class MovementExternalApiProvider : IMovementExternalApi
    {
        #region
        private MovementExternalApiProvider()
        {
        }
        internal static MovementExternalApiProvider Instance
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
            internal static readonly MovementExternalApiProvider instance = new MovementExternalApiProvider();
        }
        #region Logger instance

        private const string PolicyName = "MovementsAPIProvider";

        #endregion
        #endregion

        #region GetHaulierMovementList
        public HaulierMovementDetails GetHaulierMovementList(int organisationId, int historicData, int movementType, int pageNo, int pageSize)
        {
            return MovementExternalApiDao.GetHaulierMovementList(organisationId, historicData, movementType, pageNo, pageSize);
        }

        #endregion

        #region GetSOAPoliceMovementList
        public SoaPoliceDetails GetSOAPoliceMovementList(int organisationId, int historicData ,int pageNo, int pageSize, bool isPolice)
        {
            return MovementExternalApiDao.GetSOAPoliceMovementList(organisationId, historicData, pageNo, pageSize, isPolice);
        }

        #endregion

        #region GetSORTMovementList
        public SORTMovementDetails GetSORTMovementList(int organisationId, int historicData, int pageNo, int pageSize)
        {
            return MovementExternalApiDao.GetSORTMovementList(organisationId, historicData, pageNo, pageSize);
        }

        #endregion
    }
}