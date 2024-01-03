using STP.Common.Constants;
using STP.Common.Logger;
using STP.DataAccess.Provider;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.MovementsAndNotifications.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.MovementsAndNotifications.Controllers
{
    public class DispensationController : ApiController
    {
        #region GetAffDispensationInfo
        [HttpGet]
        [Route("Dispensation/GetAffDispensationInfo")]
        public IHttpActionResult GetAffDispensationInfo(int organisationId, int granteeId, int pageNumber, int pageSize, int userType)
        {
            try
            {
                List<DispensationGridList> result = DispensationProvider.Instance.GetAffDispensationInfo(organisationId, granteeId, pageNumber, pageSize, userType);
                    return Ok(result);
            }           
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/GetAffDispensationInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError,StatusMessage.InternalServerError);
            }

        }
        #endregion

        #region GetSummaryListCount
        [HttpGet]
        [Route("Dispensation/GetSummaryListCount")]
        public IHttpActionResult GetSummaryListCount(int organisationId, int userType)
        {
            try
            {
                int result = DispensationProvider.Instance.GetSummaryListCount(organisationId, userType);
                    return Ok(result);
            }            
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/GetSummaryListCount,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetDispensationInfo
        [HttpGet]
        [Route("Dispensation/GetDispensationInfo")]
        public IHttpActionResult GetDispensationInfo(int organisationId, int pageNumber, int pageSize, int userType, int presetFilter,int? sortOrder)
        {
            try
            {
                List<DispensationGridList> result = DispensationProvider.Instance.GetDispensationInfo(organisationId, pageNumber, pageSize, userType,presetFilter,sortOrder);
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/GetDispensationInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetDispensationSearchInfo
        [HttpGet]
        [Route("Dispensation/GetDispensationSearchInfo")]
        public IHttpActionResult GetDispensationSearchInfo(int organisationId, int pageNumber, int pageSize, string DRefNo, string summary, string grantedBy, string description, int isValid, int chckcunt, int userType, int presetFilter,int? sortOrder)
        {
            try
            {
                List<DispensationGridList> result = DispensationProvider.Instance.GetDispensationSearchInfo(organisationId, pageNumber, pageSize, DRefNo, summary, grantedBy, description, isValid, chckcunt, userType,presetFilter,sortOrder);
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/GetDispensationSearchInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ViewDispensationInfoByDRN
        [HttpGet]
        [Route("Dispensation/ViewDispensationInfoByDRN")]
        public IHttpActionResult ViewDispensationInfoByDRN(string DRN, int userTypeId)
        {
            try
            {
                DispensationGridList result = DispensationProvider.Instance.ViewDispensationInfoByDRN(DRN, userTypeId);
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/ViewDispensationInfoByDRN,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ViewDispensationInfo
        [HttpGet]
        [Route("Dispensation/ViewDispensationInfo")]
        public IHttpActionResult ViewDispensationInfo(int dispensationId, int userTypeId)
        {
            try
            {
                DispensationGridList result = DispensationProvider.Instance.ViewDispensationInfo(dispensationId, userTypeId);
                    return Ok(result);
            }           
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/ViewDispensationInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetDispensationDetailsObjByID
        [HttpGet]
        [Route("Dispensation/GetDispensationDetailsObjByID")]
        public IHttpActionResult GetDispensationDetailsObjByID(int dispensationId, int userTypeId)
        {
            try
            {
                DispensationGridList result = DispensationProvider.Instance.GetDispensationDetailsObjByID(dispensationId, userTypeId);
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/GetDispensationDetailsObjByID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region UpdateDispensation
        [HttpPost]
        [Route("Dispensation/UpdateDispensation")]
        public IHttpActionResult UpdateDispensation(UpdateDispensationParams updateDispensation)
        {
            try
            {
                int result = DispensationProvider.Instance.UpdateDispensation(updateDispensation);
                if (result >= 0)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/UpdateDispensation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region DeleteDispensation
        [HttpDelete]
        [Route("Dispensation/DeleteDispensation")]
        public IHttpActionResult DeleteDispensation(int dispensationId)
        {
            try
            {
                int affectedRows = DispensationProvider.Instance.DeleteDispensation(dispensationId);
                if (affectedRows > 0)
                    return Content(HttpStatusCode.OK, affectedRows);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            } catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/DeleteDispensation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SaveDispensation
        [HttpPost]
        [Route("Dispensation/SaveDispensation")]
        public IHttpActionResult SaveDispensation(UpdateDispensationParams updateDispensation)
        {
            try
            {
                bool result = DispensationProvider.Instance.SaveDispensation(updateDispensation);
                return Content(HttpStatusCode.Created, result);
            }catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/SaveDispensation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetDispOrganisationInfo
        [HttpGet]
        [Route("Dispensation/GetDispOrganisationInfo")]
        public IHttpActionResult GetDispOrganisationInfo(string organisationName, int pageNumber, int pageSize, int chckcunt, int userType)
        {
            try
            {
                List<DispensationGridList> result = DispensationProvider.Instance.GetDispOrganisationInfo(organisationName, pageNumber, pageSize, chckcunt, userType);
                    return Ok(result);
            }catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/GetDispOrganisationInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetDispensationDetailsByID
        [HttpGet]
        [Route("Dispensation/GetDispensationDetailsByID")]
        public IHttpActionResult GetDispensationDetailsByID(int dispensationId)
        {
            try
            {
                List<DispensationGridList> result = DispensationProvider.Instance.GetDispensationDetailsByID(dispensationId);
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -Dispensation/GetDispensationDetailsByID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpGet]
        [Route("Dispensation/GetDispensationReferenceNumber")]
        public IHttpActionResult GetDispensationReferenceNumber(string dispensationReferenceNo, int organisationId, string mode, long dispensationId)
        {
            try
            {
                decimal output = DispensationProvider.Instance.GetDispensationReferenceNumber(dispensationReferenceNo, organisationId, mode, dispensationId);
                return Content(HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Organization/GetOrganizationByName,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
