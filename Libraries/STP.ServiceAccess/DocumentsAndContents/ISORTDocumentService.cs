using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public interface ISORTDocumentService
    {
        SORTSpecialOrder GetSpecialOrder(string SOrderId);
        List<SOCRouteParts> GetRouteVehicles(int movementversionId, string veh_ID);
        List<SOCoverageDetails> GetSpecialOrderCoverages(int projectid, int state);
        List<GetSORTUserList> ListSortUser(long usertypeid, int checkertype = 0);
        List<SORTMovementList> GetSpecialOrderList(long ProjectID);
        bool Deletespecialorder(string Orderno, string userschema);
        string SaveSortSpecialOrder(SORTSpecialOrder apprevisionId, List<string> RCoverages);
        string UpdateSortSpecialOrder(SORTSpecialOrder model, List<string> removedCovrg);
        byte[] GenrateSODocument(Enums.SOTemplateType templatetype, string esdalRefNo, string orderNumber, UserInfo userInfo = null, bool generateFlag = true);
        List<NotifRouteImport> ListNEBrokenRouteDetails(long NEN_Id, int IUserId, long InboxItemId, int IOrgId);
        List<TransmissionModel> GetTransmissionType(long TransId, string Status, int StatusItemCount, string userSchema);
        int GetNoofPages(String outputString);
    }
}
