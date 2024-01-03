using STP.Common.Constants;
using STP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Structures;
namespace STP.Structures.Interface
{
    public interface IStructure
    {
        List<StructureModel> GetCautionList(int pageNumber, int pageSize, long structureID, long SectionID);
        StructureModel GetCautionDetails(long cautionID);
        List<RoadDelegationList> GetRoadDelegationList(int pageNum, int pageSize, long organisationId);
        bool SaveCautions(StructureModel Structuremodelalter);
        int DeleteCaution(long cautionId, string userName);
        bool UpdateStructureLog(List<StructureLogModel> structureLogsModel);
        List<StructureModel> GetStructureHistory(int pageNumber, int pageSize, long StructureID);
        List<StructureContactModel> GetStructureContactList(int pageNumber, int pageSize, long cautionId, short contactNo = 0);
        bool SaveStructureContact(StructureContactModel structureContact);
        int DeleteStructureContact(short CONTACT_NO, long CautionId);
        List<StructureNotification> GetAllStructureNotification(string structureId, int pageNum, int pageSize);
        List<StructureNotification> GetAllStructureOnerousVehicles(string structureId, int pageNumber, int pageSize, string searchCriteria, string searchStatus, DateTime? startDate, DateTime? endDate, int statusCount, int sort,long organisationId);
    }

}
