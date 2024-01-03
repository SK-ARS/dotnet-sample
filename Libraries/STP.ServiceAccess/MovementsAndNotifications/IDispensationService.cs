using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.MovementsAndNotifications
{
   public interface IDispensationService
    {
        List<DispensationGridList> GetAffDispensationInfo(int organisationId, int Grantee_ID, int pageNum, int pageSize, int usertype);
        int GetSummaryListCount(int orgId, int usertype);
        List<DispensationGridList> GetDispensationInfo(int organisationId, int pageNumber, int pageSize, int userType, int presetFilter, int? sortOrder = null);
        List<DispensationGridList> GetDispensationSearchInfo(int orgId, int pageNumber, int pageSize, string DRefNo, string Summary, string GrantedBy, string description, int isValid, int chckcunt, int usertype, int presetFilter, int? sortOrder = null);
        DispensationGridList ViewDispensationInfoByDRN(string DRN, int userTypeID);
        DispensationGridList ViewDispensationInfo(int dispId, int userTypeID);
        DispensationGridList GetDispensationDetailsObjByID(int dispid, int userTypeID);
        int UpdateDispensation(UpdateDispensationParams updateDispensation);
        int DeleteDispensation(int dispId);
        bool SaveDispensation(UpdateDispensationParams updateDispensation);
        List<DispensationGridList> GetDispOrganisationInfo(string org_name, int page, int pageSize, int chckcunt, int usertype);
        List<DispensationGridList> GetDispensationDetailsByID(int dispid);
        decimal GetDispensationReferenceNumber(string dispensationReferenceNo, int organisationId, string mode, long dispensationId);
    }
}
