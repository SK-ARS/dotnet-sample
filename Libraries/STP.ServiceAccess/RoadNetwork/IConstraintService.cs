using NetSdoGeometry;
using STP.Common.Constants;
using STP.Domain.RoadNetwork.Constraint;
using STP.Domain.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.RoadNetwork
{
    public interface IConstraintService
    {
        List<ConstraintModel> GetConstraintHistory(int pageNumber, int pageSize, long constraintId);
        bool CheckLinkOwnerShipForPolice(int orgID, List<ConstraintReferences> constRefrences, bool allLinks);
        bool CheckLinkOwnerShip(int orgID, List<ConstraintReferences> constRefrences, bool allLinks);
        long SaveConstraints(ConstraintModel constrModel, int userID);
        ConstraintModel GetConstraintDetails(int ConstraintID);
        long UpdateConstraint(ConstraintModel constrModel, int userID);
        long DeleteConstraint(long ConstraintID, string UserName);
        int DeleteCaution(long CAUTION_ID, string UserName);
        List<ConstraintModel> GetCautionList(int pageNumber, int pageSize, long ConstraintID);
        ConstraintModel GetCautionDetails(long cautionID);
        bool SaveCautions(ConstraintModel constrModel);
        bool UpdateConstraintLog(List<ConstraintLogModel> constrModel);
        List<ConstraintContactModel> GetConstraintContactList(int pageNumber, int pageSize, long ConstraintID, short ContactNo = 0);
        List<ConstraintModel> GetNotificationExceedingConstring(int pageNumber, int pageSize, long ConstraintID, int UserID);
        List<RouteConstraints> GetConstraintListForOrg(int organisationID, string userSchema, int otherOrg, int left, int right, int bottom, int top);
        bool CheckLinkOwnerShip(int orgID, List<int> linkIDs, bool allLinks);
        bool SaveLinkDetails(long CONSTRAINT_ID, List<ConstraintReferences> constRefrences);
        List<RouteConstraints> GetConstraints();
        bool FindLinksOfAreaConstraint(sdogeometry polygonGeometry, int orgId, int userType);
        bool SaveConstraintContact(ConstraintContactModel constrModel);
        int DeleteContact(short contactNo, long constraintId);
        RouteAssessmentModel GetAffectedStructuresConstraints(int NotificationId, string EsdalRefNum, string HaulierMnemonic, string VersionNo, string userSchema = UserSchema.Portal, int INBOX_ID = 0);
        RouteAssessmentModel GetNotificationAffectedStructuresConstraint(int inboxId, int organisationId);
        List<ConstraintModel> GetConstraintList(int OrgId, int pageNumber, int pageSize , SearchConstraintsFilter objSearch);


    }
}
