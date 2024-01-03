using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.MovementsAndNotifications.Interface
{
   public interface IDispensation
    {    
        List<DispensationGridList> GetAffDispensationInfo(int organisationId, int granteeId, int pageNumber, int pageSize, int userType);
        int GetSummaryListCount(int organisationId, int userType);
        List<DispensationGridList> GetDispensationInfo(int organisationId, int pageNumber, int pageSize, int userType,int presetFilter, int? sortOrder = null);
        List<DispensationGridList> GetDispensationSearchInfo(int organisationId, int pageNumber, int pageSize, string DRefNo, string summary, string grantedBy, string description, int isValid, int chckcunt, int userType,int presetFilter,int? sortOrder=null);
        DispensationGridList ViewDispensationInfoByDRN(string DRN, int userTypeId);
        DispensationGridList ViewDispensationInfo(int dispensationId, int userTypeId);
        DispensationGridList GetDispensationDetailsObjByID(int dispensationId, int userTypeId);
        int UpdateDispensation(UpdateDispensationParams updateDispensation);
        int DeleteDispensation(int dispensationId);
        bool SaveDispensation(UpdateDispensationParams updateDispensation);
        List<DispensationGridList> GetDispOrganisationInfo(string organisationName, int pageNumber, int pageSize, int chckcunt, int userType);
        List<DispensationGridList> GetDispensationDetailsByID(int dispensationId);
        decimal GetDispensationReferenceNumber(string dispensationReferenceNo, int organisationId, string mode, long dispensationId);
    }
}
