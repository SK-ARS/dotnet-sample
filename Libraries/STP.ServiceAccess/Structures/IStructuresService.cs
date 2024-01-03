using STP.Common.Constants;
using STP.Domain;
using STP.Domain.RoadNetwork.RoadOwnership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Structures;
namespace STP.ServiceAccess.Structures
{
    public interface IStructuresService
    {
        List<DropDown> GetDelegationArrangement(int orgId, string delegationArrangement);
        List<StructureSummary> GetStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct,int presetFilter);
        int CheckStructureAgainstOrganisation(long structureId, long organisationId);
        int CheckStructureOrganisation(int organisationId, long structureId);
        List<StructureGeneralDetails> ViewGeneralDetails(long structureId);
        bool EditStructureGeneralDetails(StructureGeneralDetails structureGeneralDetails);
        ImposedConstraints ViewimposedConstruction(int structureId, int sectionId);
        bool GetEditStructureImposed(StuctImposedParams stuctImposedParams);
        List<StructType> GetStructType(int type);
        List<StructCategory> GetStructCategory(int type);
        SpanData ViewSpanDataByNo(long structureId, long sectionId, long? spanNo);
        List<StucDDList> GetSTRUCT_DD(int TYPE);
        DimensionConstruction ViewDimensionConstruction(int structureId, int sectionId);
        List<SpanData> ViewSpanData(int structureId, int sectionId);
        int SaveStructureSpan(StructureSpanParams SpanParams);
        List<SVDataList> GetSVData(long structureId, long sectionId);
        List<SVDataList> UpdateSVData(UpdateSVParams objUpdateSVParams);
        List<double?> GetHBRatings(long structureId, long sectionId);
        List<SvReserveFactors> GetCalculatedHBToSV(long structureId, long sectionId, double? hbWithLoad, double? hbWithoutLoad, int saveFlag, string userName);
        List<StructureModel> GetCautionList(CautionListParams cautionListParams);
        StructureModel GetCautionDetails(long cautionID);
        List<RoadDelegationList> GetRoadDelegationList(int pageNum, int pageSize, long orgId);
        bool SaveCautions(StructureModel Structuremodelalter);
        long DeleteCaution(long cautionId, string userName);
        bool UpdateStructureLog(List<StructureLogModel> structureLogsModel);
        List<StructureModel> GetStructureHistory(int pageNumber, int pageSize, long StructureID);
        int GetStructureHistoryCount(long StructureID);
        List<StructureHistoryList> GetStructureHistoryById(int pageNumber, int pageSize, long StructureID);
        List<StructureContactModel> GetStructureContactList(int pageNumber, int pageSize, long CautionID, short ContactNo = 0);
        List<StructureSectionList> ViewStructureSections(long structureId);
        ManageStructureICA ViewEnabledICA(int structureId, int sectionId, long OrgID);
        List<SvReserveFactors> ViewSVData(int structureId, int sectionId);
        bool SaveStructureContact(StructureContactModel constraintContact);
        bool DeleteStructureContact(short contactNo, long cautionId);
        List<StructureInfo> MyStructureInfoList(int organisationId, int otherOrganisation, int left, int right, int bottom, int top);
        List<StructureContact> GetStructureContactListInfo(long structureId, string userSchema = UserSchema.Portal);
        List<RoadContactModal> GetRoadContactList(long linkId, long length, string userSchema = UserSchema.Portal);

        List<StructureInfo> AgreedAppStructureInfo(string StructureCode);
        byte[] GetAffectedParties(int NotificationId, string userSchema = UserSchema.Portal);
        List<StructureNotification> GetAllStructureNotification(string structureId, int pageNumber, int pageSize);
        bool UpdateDefaultBanding(SaveDefaultConfigParams configparams);
        ConfigBandModel GetDefaultBanding(int organisationId, int structureId, int sectionId);
        string GetICAVehicleResult(ICAVehicleResult ICAVehicleParams);
        ManageStructureICA GetManageICAUsage(int orgId, int structureId, int sectionId);
        bool UpdateStructureICAUsage(UpdateICAUsageParams ICAUsageparams);
        bool EditDimensionConstraints(DimensionConstruction objStructureDimension, int structureId, int sectionId);
        int DeleteStructureSpan(long structureId, long sectionId, long spanNo, string userName);
        List<StructureNotification> GetAllStructureOnerousVehicles(string structureId, int pageNumber, int pageSize, string searchCriteria, string searchStatus, DateTime? startDate, DateTime? endDate, int statusCount, int sort, long organisationId);
        int GetStructureOwner(long structId, long orgId);
		int GetStructureId(string structurecode);
	}
}
