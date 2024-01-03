using STP.Common.Constants;
using STP.RoadNetwork.Interface;
using STP.Domain.RoadNetwork.Constraint;
using STP.RoadNetwork.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.RouteAssessment;

namespace STP.RoadNetwork.Providers
{
    public sealed class ConstraintProvider: IConstraint
    {
        #region ConstraintProvider Singleton

        private ConstraintProvider()
        {
        }
        public static ConstraintProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly ConstraintProvider instance = new ConstraintProvider();
        }
        #endregion

        public List<ConstraintModel> GetConstraintHistory(int pageNumber, int pageSize, long constraintId)
        {
            return ConstraintDAO.GetConstraintHistory(pageNumber, pageSize, constraintId);
        }
        public bool CheckLinkOwnerShipForPolice(int organisationId, List<ConstraintReferences> constRefrences, bool allLinks)
        {
            return ConstraintDAO.CheckLinkOwnerShipForPolice(organisationId, constRefrences, allLinks);
        }
        public bool CheckLinkOwnerShip(int organisationId, List<ConstraintReferences> constRefrences, bool allLinks)
        {
            return ConstraintDAO.CheckLinkOwnerShip(organisationId, constRefrences, allLinks);
        }
        public bool CheckLinkOwnerShip(int organisationId, List<int> linkIds, bool allLinks)
        {
            return ConstraintDAO.CheckLinkOwnerShip(organisationId, linkIds, allLinks);
        }
        public bool SaveLinkDetails(long constraintId, List<ConstraintReferences> constRefrences)
        {
            return ConstraintDAO.SaveLinkDetails(constraintId, constRefrences);
        }
        public List<RouteConstraints> GetConstraints()
        {
            return ConstraintDAO.GetConstraints();
        }
        public List<ConstraintModel> GetConstraintList(int OrgId, int pageNumber, int pageSize, SearchConstraintsFilter objSearchConstraints)
        {
            return ConstraintDAO.GetConstraintList(OrgId, pageNumber, pageSize, objSearchConstraints);
        }
        public long SaveConstraints(ConstraintModel constrModel ,int userId)
        {
            return ConstraintDAO.SaveConstraint(constrModel,userId);
        }
        public ConstraintModel GetConstraintDetails(int constraintId)
        {
            return ConstraintDAO.GetConstraintDetails(constraintId);
        }
        public long UpdateConstraint(ConstraintModel constraintModel, int userId)
        {
            return ConstraintDAO.UpdateConstraint(constraintModel, userId);
        }
        public long DeleteConstraint(long constraintID, string userName)
        {
            return ConstraintDAO.DeleteConstraint(constraintID, userName);
        }
        /// <summary>
        /// Get Constraint list
        /// </summary>
        /// <param name="pageNumber">Page</param>
        /// <param name="pageSize"> size of page</param>
        /// <param name="constraintId">Constraint Id </param>
        /// <returns>Return list of caution list</returns>
        public List<ConstraintModel> GetCautionList(int pageNumber, int pageSize, long constraintId)
        {
            return ConstraintDAO.GetCautionList(pageNumber, pageSize, constraintId);
        }
        /// <summary>
        /// Deleter caution
        /// </summary>
        /// <param name="cautionId">Caution id</param>
        /// <param name="userName">Login user name</param>
        /// <returns>Deleter caution and make entry in history</returns>
        public int DeleteCaution(long cautionId, string userName)
        {
            return ConstraintDAO.DeleteCaution(cautionId, userName);
        }
        public ConstraintModel GetCautionDetails(long cautionId)
        {
            return ConstraintDAO.GetCautionDetails(cautionId);
        }
        /// <summary>
        /// Save cautions
        /// </summary>
        /// <param name="constraintModel">ConstraintModel</param>
        /// <returns>Save cautions</returns>
        public bool SaveCautions(ConstraintModel constraintModel)
        {
            return ConstraintDAO.SaveCautions(constraintModel);
        }

        /// <summary>
        /// Update constraint log
        /// </summary>
        /// <param name="constraintLogsModel">list of ConstraintLogModel</param>
        /// <returns>Update modification in Constraint_log table</returns>
        public bool UpdateConstraintLog(List<ConstraintLogModel> constraintLogsModel)
        {
            return ConstraintDAO.UpdateConstraintLog(constraintLogsModel);
        }
        /// <summary>
        /// Review contacts list
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="constraintId">Constraint id</param>
        /// <param name="contactNo">Contact no</param>
        /// <returns>Get contact list</returns>
        public List<ConstraintContactModel> GetConstraintContactList(int pageNumber, int pageSize, long constraintId, short contactNo = 0)
        {
            return ConstraintDAO.GetConstraintContactList(pageNumber, pageSize, constraintId, contactNo);
        }
        public List<ConstraintModel> GetNotificationExceedingConstring(int pageNumber, int pageSize, long constraintID, int userID)
        {
            return ConstraintDAO.GetNotificationExceedingConstraint(pageNumber, pageSize, constraintID, userID);
        }
        public List<RouteConstraints> GetConstraintListForOrg(int organisationId, string userSchema, int otherOrganisation, int left, int right, int bottom, int top)
        {
            return ConstraintDAO.getConstraintListForOrg(organisationId, userSchema, otherOrganisation, left, right, bottom, top);
        }
        public bool FindLinksOfAreaConstraint(NetSdoGeometry.sdogeometry polygonGeometry, int organisationId, int userType)
        {
            return ConstraintDAO.FindLinksOfAreaConstraint(polygonGeometry, organisationId, userType);
        }
        public bool SaveConstraintContact(ConstraintContactModel constraintContact)
        {
            return ConstraintDAO.SaveConstraintContact(constraintContact);
        }
        /// <summary>
        /// Deleter constraint contact
        /// </summary>
        /// <param name="contactNo">Contact no</param>
        /// <param name="constraintId">Constraint id</param>
        /// <returns>Delete constrain contact </returns>
        public int DeleteContact(short contactNo, long constraintId)
        {
            return ConstraintDAO.DeleteContact(contactNo, constraintId);
        }

        #region GetAffectedStructuresConstraints
        public RouteAssessmentModel GetAffectedStructuresConstraints(int notificationId, string esdalReferenceNo, string haulierMnemonic, string versionNo, string userSchema = UserSchema.Portal, int inboxId = 0)
        {
            return ConstraintDAO.GetNotifAffectedStructuresConstraint(notificationId, esdalReferenceNo, haulierMnemonic, versionNo, userSchema, inboxId);
        }
        #endregion
        #region GetNotificationAffectedStructuresConstraint
        public RouteAssessmentModel GetNotificationAffectedStructuresConstraint(int inboxId, int organisationId)
        {
            return ConstraintDAO.GetNotifAffectedStructuresConstraint(inboxId, organisationId);
        }
        #endregion
    }
}