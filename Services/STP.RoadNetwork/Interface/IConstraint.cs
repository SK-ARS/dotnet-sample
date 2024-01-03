using STP.Common.Constants;
using STP.Domain.RoadNetwork.Constraint;
using STP.Domain.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.RoadNetwork.Interface
{
    interface IConstraint
    {
        List<ConstraintModel> GetConstraintHistory(int pageNumber, int pageSize, long constraintId);
        bool CheckLinkOwnerShipForPolice(int organisationId, List<ConstraintReferences> constRefrences, bool allLinks);
        bool CheckLinkOwnerShip(int organisationId, List<ConstraintReferences> constRefrences, bool allLinks);
        bool CheckLinkOwnerShip(int organisationId, List<int> linkIDs, bool allLinks);
        long SaveConstraints(ConstraintModel constrModel, int userId);
        ConstraintModel GetConstraintDetails(int constraintId);
        long UpdateConstraint(ConstraintModel constraintModel, int userId);
        long DeleteConstraint(long constraintId, string UserName);
        List<ConstraintModel> GetCautionList(int pageNumber, int pageSize, long constraintId);
        ConstraintModel GetCautionDetails(long cautionId);
        bool SaveCautions(ConstraintModel constraintModel);
        bool UpdateConstraintLog(List<ConstraintLogModel> constraintLogsModel);
        List<ConstraintContactModel> GetConstraintContactList(int pageNumber, int pageSize, long constraintId, short ContactNo = 0);
        List<ConstraintModel> GetNotificationExceedingConstring(int pageNumber, int pageSize, long constraintId, int userId);
        List<RouteConstraints> GetConstraintListForOrg(int organisationID, string userSchema, int otherOrg, int left, int right, int bottom, int top);
        bool SaveLinkDetails(long constraintId, List<ConstraintReferences> constRefrences);
        List<RouteConstraints> GetConstraints();
        bool FindLinksOfAreaConstraint(NetSdoGeometry.sdogeometry polygonGeometry, int organisationId, int userType);
        bool SaveConstraintContact(ConstraintContactModel constraintContact);
        int DeleteContact(short contactNo, long constraintId);
        RouteAssessmentModel GetAffectedStructuresConstraints(int notificationId, string EsdalRefNum, string HaulierMnemonic, string VersionNo, string userSchema = UserSchema.Portal, int inboxId = 0);
        int DeleteCaution(long cautionId, string userName);
        RouteAssessmentModel GetNotificationAffectedStructuresConstraint(int inboxId, int organisationId);
        List<ConstraintModel> GetConstraintList(int OrgId, int pageNumber, int pageSize, SearchConstraintsFilter objSearchConstraints);
    }
}
