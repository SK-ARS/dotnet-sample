using STP.Domain.MovementsAndNotifications.HaulierMovementsAPI;
using STP.Domain.MovementsAndNotifications.SOAPoliceMovementsAPI;
using STP.Domain.MovementsAndNotifications.SORTMovementsAPI;

namespace STP.MovementsAndNotifications.Interface
{
   public  interface IMovementExternalApi
    {
        HaulierMovementDetails GetHaulierMovementList(int organisationId, int historicData, int movementType, int pageNo, int pageSize);
        SoaPoliceDetails GetSOAPoliceMovementList(int organisationId, int historicData, int pageNo, int pageSize, bool isPolice);
        SORTMovementDetails GetSORTMovementList(int organisationId, int historicData, int pageNo, int pageSize);
    }
}
