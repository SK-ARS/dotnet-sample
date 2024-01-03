using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using NetSdoGeometry;
using Newtonsoft.Json;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.RoadNetwork.Providers;
using STP.Common.Constants;
using STP.Common.Logger;

namespace STP.RoadNetwork.Controllers
{
    public class RoadOwnershipController : ApiController
    {
        [HttpGet]
        [Route("RoadOwnership/GetRoadOwnershipOrganisations")]
        public IHttpActionResult GetRoadOwnershipOrganisations(string orgName, int pageNum, int pageSize, int searchFlag)
        {
            try
            {
                List<RoadOwnershipOrgSummary> objRoadOwnershipOrgLst = RoadOwnership.Instance.GetRoadOwnershipOrganisations(orgName, pageNum, pageSize, searchFlag);
                return Ok(objRoadOwnershipOrgLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/GetRoadOwnershipOrganisations, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }            
        }

        [HttpPost]
        [Route("RoadOwnership/GetUnassignedLinks")]
        public IHttpActionResult GetUnassignedLinks(List<long> linkIds)
        {
            try
            {
                List<long> unassignedLinkLst = RoadOwnership.Instance.GetUnassignedLinks(linkIds);
                return Ok(unassignedLinkLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/GetUnassignedLinks, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }                         
        }

        [HttpGet]
        [Route("RoadOwnership/GetDelegationArrangementDetails")]
        public IHttpActionResult GetDelegationArrangementDetails(int orgId)
        {
            try
            {
                List<ArrangementDetails> arrDetailsLst = RoadOwnership.Instance.GetDelegationArrangementDetails(orgId);
                return Ok(arrDetailsLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/GetDelegationArrangementDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }             
        }

        [HttpGet]
        [Route("RoadOwnership/GetRoadOwnerContactList")]
        public IHttpActionResult GetRoadOwnerContactList(long linkId, long length, string pageType, string userSchema = UserSchema.Portal)
        {
            try
            {
                List<RoadContactModal> objRoadOwnerContactLst = RoadOwnership.Instance.GetRoadOwnerContactList(linkId, length, pageType, userSchema);
                return Ok(objRoadOwnerContactLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/GetRoadOwnerContactList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }             
        }

        [HttpPost]
        [Route("RoadOwnership/GetRoadOwnershipDetails")]
        public IHttpActionResult GetRoadOwnershipDetails(List<long> linkIds)
        {
            try
            {
                List<RoadOwnershipData> objRoadOwnershipDetails = RoadOwnership.Instance.GetRoadOwnershipDetails(linkIds);
                return Ok(objRoadOwnershipDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/GetRoadOwnershipDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }            
        }

        [HttpPost]
        [Route("RoadOwnership/SaveRoadOwnership")]
        public IHttpActionResult SaveRoadOwnership(RoadOwnerShipDetails roadOwnerObj)
        {
            bool saveFlag = false;
            try
            {
                saveFlag = RoadOwnership.Instance.SaveRoadOwnership(roadOwnerObj);                
                return Content(HttpStatusCode.Created, saveFlag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/SaveRoadOwnership, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RoadOwnership/FetchRoadInfoToDisplayOnMap")]
        public IHttpActionResult FetchRoadInfoToDisplayOnMap(FetchRoadInfoParams fetchRoadInfoParams)
        {
            try
            {
                sdogeometry areaGeomObj = JsonConvert.DeserializeObject<sdogeometry>(fetchRoadInfoParams.AreaGeometryStr.ToString());
                List<LinkInfo> roadLinkInfoLst = RoadOwnership.Instance.FetchRoadInfoToDisplayOnMap(fetchRoadInfoParams.OrganisationId, fetchRoadInfoParams.FetchFlag, areaGeomObj, fetchRoadInfoParams.ZoomLevel);
                return Ok(roadLinkInfoLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/FetchRoadInfoToDisplayOnMap, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}