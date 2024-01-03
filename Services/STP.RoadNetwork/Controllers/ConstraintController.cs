using NetSdoGeometry;
using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;

using STP.Domain.RoadNetwork.Constraint;
using STP.Domain.RouteAssessment;
using STP.RoadNetwork.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.RoadNetwork.Controllers
{
    public class ConstraintController : ApiController
    {

        [HttpPost]
        [Route("Constraint/CheckLinkOwnerShipForPolice")]
        public IHttpActionResult CheckLinkOwnerShipForPolice(ConstraintReferencesParam constraintReferences)
        {
            try
            {
                bool res;
                res = ConstraintProvider.Instance.CheckLinkOwnerShipForPolice(constraintReferences.OrganisationId, constraintReferences.ConstraintRefrences, constraintReferences.AllLinks);
                return Content(HttpStatusCode.OK, res);

            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/CheckLinkOwnerShipForPolice, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Constraint/CheckLinkOwnerShip")]
        public IHttpActionResult CheckLinkOwnerShip(ConstraintReferencesParam constraintReferences)
        {
            try
            {
                bool res;
                res = ConstraintProvider.Instance.CheckLinkOwnerShip(constraintReferences.OrganisationId, constraintReferences.ConstraintRefrences, constraintReferences.AllLinks);
                return Content(HttpStatusCode.OK, res);

            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/CheckLinkOwnerShip, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Constraint/SaveLinkDetails")]
        public IHttpActionResult SaveLinkDetails(ConstraintReferencesParam constraintReferences)
        {
            try
            {
                bool res;
                res = ConstraintProvider.Instance.SaveLinkDetails(constraintReferences.ConstraintId, constraintReferences.ConstraintRefrences);
                return Content(HttpStatusCode.Created, res);

            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/SaveLinkDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/CheckLinkOwnerShip")]
        public IHttpActionResult CheckLinkOwnerShip(int organisationId, List<int> linkIds, bool allLinks)
        {
            try
            {
                bool res = ConstraintProvider.Instance.CheckLinkOwnerShip(organisationId, linkIds, allLinks);
                if (!res)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.InternalServerError);
                }
                else
                {
                    return Content(HttpStatusCode.OK, res);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/CheckLinkOwnerShip, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Constraint/SaveConstraints")]
        public IHttpActionResult SaveConstraints(ConstraintModel constraintModel)
        {
            try
            {
                long res;
                constraintModel.Geometry = JsonConvert.DeserializeObject<sdogeometry>(constraintModel.AreaGeomStructure.ToString());
                res = ConstraintProvider.Instance.SaveConstraints(constraintModel, constraintModel.UserId);
                return Content(HttpStatusCode.Created, res);

            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/SaveConstraints, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpGet]
        [Route("Constraint/GetConstraintHistory")]
        public IHttpActionResult GetConstraintHistory(int pageNumber, int pageSize, long constraintId)
        {
            try
            {
                List<ConstraintModel> constraintModels = ConstraintProvider.Instance.GetConstraintHistory(pageNumber, pageSize, constraintId);
                if (constraintModels != null && constraintModels.Count == 0)
                {
                    return Content(HttpStatusCode.NotFound, "Constrain tHistory not available");
                }
                else
                {
                    return Content(HttpStatusCode.OK, constraintModels);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetConstraintHistory, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/GetConstraintDetails")]
        public IHttpActionResult GetConstraintDetails(int ConstraintId)
        {
            try
            {
                ConstraintModel constraintModels = ConstraintProvider.Instance.GetConstraintDetails(ConstraintId);
                if (constraintModels != null)
                {
                    return Content(HttpStatusCode.OK, constraintModels);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, "Constraint not available");
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetConstraintDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Constraint/UpdateConstraint")]
        public IHttpActionResult UpdateConstraint(ConstraintModel constraintModel)
        {
            try
            {
                long ConstraintId = ConstraintProvider.Instance.UpdateConstraint(constraintModel, constraintModel.UserId);
                if (ConstraintId == 0)
                {
                    return Content(HttpStatusCode.NotFound, "Update Constraint Failed");
                }
                else
                {
                    return Content(HttpStatusCode.OK, ConstraintId);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/UpdateConstraint, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Constraint/DeleteConstraint")]
        public IHttpActionResult DeleteConstraint(long constraintId, string userName)
        {
            try
            {
                long res = ConstraintProvider.Instance.DeleteConstraint(constraintId, userName);
                if (res ==0)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, res);
                }
            }
            catch (SqlException ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{ConfigurationManager.AppSettings["Instance"]} - ConstraintDao/DeleteConstraint, Exception: {ex}​​​​​​​");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/DeleteConstraint, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Constraint/DeleteCaution")]
        public IHttpActionResult DeleteCaution(long cautionId, string userName)
        {
            try
            {
                int affectedRows = ConstraintProvider.Instance.DeleteCaution(cautionId, userName);
                if (affectedRows > -1)
                {                 
                    return Content(HttpStatusCode.OK, affectedRows);
                }
                else
                { 
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/DeleteCaution, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/GetCautionList")]
        public IHttpActionResult GetCautionList(int pageNumber, int pageSize, long constraintId)
        {
            try
            {
                List<ConstraintModel> constraintModels = ConstraintProvider.Instance.GetCautionList(pageNumber, pageSize, constraintId);
                if (constraintModels == null)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.InternalServerError);
                }
                else
                {
                    return Content(HttpStatusCode.OK, constraintModels);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetCautionList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/GetCautionDetails")]
        public IHttpActionResult GetCautionDetails(long cautionId)
        {
            try
            {
                ConstraintModel constraintModels = ConstraintProvider.Instance.GetCautionDetails(cautionId);
                if (constraintModels == null)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.InternalServerError);
                }
                else
                {
                    return Content(HttpStatusCode.OK, constraintModels);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetCautionDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Constraint/SaveCautions")]
        public IHttpActionResult SaveCautions(ConstraintModel constraintModel)
        {
            try
            {
                bool res = ConstraintProvider.Instance.SaveCautions(constraintModel);
                if (!res)
                {
                    return Content(HttpStatusCode.NotFound, "Save Cautions Failed");
                }
                else
                {
                    return Content(HttpStatusCode.Created, res);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/SaveCautions, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Constraint/UpdateConstraintLog")]
        public IHttpActionResult UpdateConstraintLog(List<ConstraintLogModel> constraintLogModels)
        {
            try
            {
                bool res = ConstraintProvider.Instance.UpdateConstraintLog(constraintLogModels);
                if (!res)
                {
                    return Content(HttpStatusCode.NotFound, "Save Cautions Failed");
                }
                else
                {
                    return Content(HttpStatusCode.Created, res);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/UpdateConstraintLog, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/GetConstraintContactList")]
        public IHttpActionResult GetConstraintContactList(int pageNumber, int pageSize, long constraintID, short contactNo = 0)
        {
            try
            {
                List<ConstraintContactModel> constraintModels = ConstraintProvider.Instance.GetConstraintContactList(pageNumber, pageSize, constraintID, contactNo);
                if (constraintModels.Count == 0)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.InternalServerError);
                }
                else
                {
                    return Content(HttpStatusCode.OK, constraintModels);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetConstraintContactList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/GetNotificationExceedingConstring")]
        public IHttpActionResult GetNotificationExceedingConstring(int pageNumber, int pageSize, long constraintID, int userID)
        {
            try
            {
                List<ConstraintModel> constraintModels = ConstraintProvider.Instance.GetNotificationExceedingConstring(pageNumber, pageSize, constraintID, userID);
                if (constraintModels.Count == 0)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.InternalServerError);
                }
                else
                {
                    return Content(HttpStatusCode.OK, constraintModels);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetNotificationExceedingConstring, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/GetConstraintListForOrg")]
        public IHttpActionResult GetConstraintListForOrg(int organisationID, string userSchema, int otherOrg, int left, int right, int bottom, int top)
        {
            try
            {
                List<RouteConstraints> constraint = ConstraintProvider.Instance.GetConstraintListForOrg(organisationID, userSchema, otherOrg, left, right, bottom, top);
                if (constraint.Count == 0)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.InternalServerError);
                }
                else
                {
                    return Content(HttpStatusCode.OK, constraint);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetConstraintListForOrg, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Constraint/GetConstraints")]
        public IHttpActionResult GetConstraints()
        {
            List<RouteConstraints> routeConstraints = new List<RouteConstraints>();
            try
            {
                routeConstraints = ConstraintProvider.Instance.GetConstraints();

            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetConstraints, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
            return Content(HttpStatusCode.OK, routeConstraints);
        }
        [HttpPost]
        [Route("Constraint/GetConstraintList")]
        public IHttpActionResult GetConstraintList(ConstrainteListParams ConstraintListParams)
        {
            List<ConstraintModel> routeConstraints = new List<ConstraintModel>();
            try
            {
                routeConstraints = ConstraintProvider.Instance.GetConstraintList(ConstraintListParams.OrganisationId, ConstraintListParams.PageNumber, ConstraintListParams.PageSize, ConstraintListParams.ObjSearchConstraintsFilter);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/GetConstraintList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
            return Content(HttpStatusCode.OK, routeConstraints);
        }

        [HttpGet]
        [Route("Constraint/FindLinksOfAreaConstraint")]
        public IHttpActionResult FindLinksOfAreaConstraint(string polygonGeometrystring, int organisationId, int userType)
        {
            try
            {
                sdogeometry polygonGeometry = JsonConvert.DeserializeObject<sdogeometry>(polygonGeometrystring);

                bool res = ConstraintProvider.Instance.FindLinksOfAreaConstraint(polygonGeometry, organisationId, userType);
                if (!res)
                {
                    return Content(HttpStatusCode.NotFound, StatusMessage.InternalServerError);
                }
                else
                {
                    return Content(HttpStatusCode.OK, res);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/FindLinksOfAreaConstraint, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Constraint/SaveConstraintContact")]
        public IHttpActionResult SaveConstraintContact(ConstraintContactModel constrModel)
        {
            try
            {
                bool res = ConstraintProvider.Instance.SaveConstraintContact(constrModel);
                if (!res)
                {
                    return Content(HttpStatusCode.NotFound, "Save Cautions Failed");
                }
                else
                {
                    return Content(HttpStatusCode.Created, res);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/SaveConstraintContact, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Constraint/DeleteContact")]
        public IHttpActionResult DeleteContact(short contactNo, long constraintId)
        {
            try
            {
                int affectedRows = ConstraintProvider.Instance.DeleteContact(contactNo, constraintId);
                if (affectedRows > -1)
                {
                    return Content(HttpStatusCode.OK, affectedRows);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Constraint/DeleteContact, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Constraint/GetAffectedStructuresConstraints")]
        public IHttpActionResult GetAffectedStructuresConstraints(int notificationId, string EsdalRefNum, string haulierMnemonic, string versionNo, string userSchema = UserSchema.Portal, int inboxId = 0)
        {
            try
            {
                RouteAssessmentModel res = ConstraintProvider.Instance.GetAffectedStructuresConstraints(notificationId, EsdalRefNum, haulierMnemonic, versionNo, userSchema , inboxId);
                    return Content(HttpStatusCode.OK, res);
            }
           catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Constraint/GetAffectedStructuresConstraints,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region GetNotificationAffectedStructuresConstraint
        [HttpGet]
        [Route("Constraint/GetNotificationAffectedStructuresConstraint")]
        public IHttpActionResult GetNotificationAffectedStructuresConstraint(int inboxId, int organisationId)
        {
            try
            {
                RouteAssessmentModel result = ConstraintProvider.Instance.GetNotificationAffectedStructuresConstraint(inboxId, organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Constraint/GetNotificationAffectedStructuresConstraint,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}
