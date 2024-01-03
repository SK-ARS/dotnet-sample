using AggreedRouteXSD;
using STP.Domain;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.DocumentsAndContents.Interface
{
    public interface ISORTDocument
    {
        SORTSpecialOrder GetSpecialOrder(string orderId);
        List<SOCRouteParts> GetRouteVehicles(int movementVersionId, string vehicleId);
        List<SOCoverageDetails> GetSpecialOrderCoverages(int projectid, int state);
        List<GetSORTUserList> ListSortUser(long userTypeId, int checkerType = 0);
        List<SORTMovementList> GetSpecialOrderList(long projectId);
        int DeleteSpecialOrder(string orderNo, string userSchema);
        string SaveSortSpecialOrder(SORTSpecialOrder apprevisionId, List<string> RCoverages);
        byte[] GenrateSODocument(Enums.SOTemplateType templatetype, string esdalRefNo, string orderNumber, UserInfo userInfo = null, bool generateFlag = true);
        List<NotifRouteImport> ListNEBrokenRouteDetails(NotifRouteImportParams objNotifRouteImportParams);
        byte[] GenerateHaulierAgreedRouteDocument(string esDALRefNo = "GCS1/25/2", string order_no = "P21/2012", int contactId = 8866, UserInfo SessionInfo = null);
        int GetNoofPages(String outputString);
       
    }
}
