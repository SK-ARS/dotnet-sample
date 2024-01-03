using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.MovementsAndNotifications.Interface
{
    public interface IMovementInbox
    {
        List<MovementsInbox> GetInboxMovements(GetInboxMovementsParams inboxMovementsParams);
        List<MovementsInbox> GetHomePageMovements(GetInboxMovementsParams inboxMovementsParams);
        int GetContactDetails(int UserId);
        string GetSpecialOrderNo(string esdalRefNo);
        long GetDocumentID(string esdalRefNo, long organisationID);
        List<DelegArrangeNameList> GetArrangementList(int organisationId);
        List<MovementsList> GetMovementsList(HaulierMovementsListParams objHaulierMovementsListParams);
        List<MovementsList> GetPlanMovementList(HaulierMovementsListParams objHaulierMovementsListParams);
        List<FolderNameList> GetFolderList(long organisationId, string userSchema);
    }
}
