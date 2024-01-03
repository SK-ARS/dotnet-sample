using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using NetSdoGeometry;
using Newtonsoft.Json;
using STP.Domain.RoadNetwork.RoadDelegation;
using STP.RoadNetwork.Providers;
using STP.Common.Constants;
using STP.Common.Logger;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace STP.RoadNetwork.Controllers
{
    public class RoadDelegationController : ApiController
    {

        [HttpPost]
        [Route("RoadDelegation/GetRoadDelegationList")]
        public IHttpActionResult GetRoadDelegationList(GetRoadDelegationListParams roadDelegationListParams)
        {
            try
            {
                List<RoadDelegationData> objRoadDelegationLst = RoadDelegation.Instance.GetRoadDelegationList(roadDelegationListParams.SearchParam, roadDelegationListParams.PageSize, roadDelegationListParams.PageNumber, roadDelegationListParams.SortOrder,roadDelegationListParams.SortTye);
                return Ok(objRoadDelegationLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/GetRoadDelegationList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }            
        }

        [HttpGet]
        [Route("RoadDelegation/GetRoadDelegationOrganisations")]
        public IHttpActionResult GetRoadDelegationOrganisations(string orgName, int pageNum, int pageSize, int searchFlag)
        {
            try
            {
                List<RoadDelegationOrgSummary> objRoadDelegationOrgLst = RoadDelegation.Instance.GetRoadDelegationOrganisations(orgName, pageNum, pageSize, searchFlag);
                return Ok(objRoadDelegationOrgLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/GetRoadDelegationOrganisations, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RoadDelegation/IsDelegationAllowed")]
        public IHttpActionResult IsDelegationAllowed(int orgId)
        {
            bool delegationAllowedFlg = false;
            try
            {                
                delegationAllowedFlg = RoadDelegation.Instance.IsDelegationAllowed(orgId);
                return Ok(delegationAllowedFlg);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/IsDelegationAllowed, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RoadDelegation/GetRoadDelegationDetailsWithLinkInfo")]
        public IHttpActionResult GetRoadDelegationDetailsWithLinkInfo(int delArrId)
        {
            try
            {
                RoadDelegationData objRoadDelegation = RoadDelegation.Instance.GetRoadDelegationDetailsWithLinkInfo(delArrId);
                return Ok(objRoadDelegation);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/GetRoadDelegationDetailsWithLinkInfo, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RoadDelegation/GetRoadDelegationDetails")]
        public IHttpActionResult GetRoadDelegationDetails(int delArrId)
        {
            try
            {
                RoadDelegationData objRoadDelegation = RoadDelegation.Instance.GetRoadDelegationDetails(delArrId);
                return Ok(objRoadDelegation);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/GetRoadDelegationDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("RoadDelegation/DeleteRoadDelegation")]
        public IHttpActionResult DeleteRoadDelegation(int delArrId)
        {
            try
            {
                int affectedRows = RoadDelegation.Instance.DeleteRoadDelegation(delArrId);
                if (affectedRows > -1)
                    return Content(HttpStatusCode.OK, affectedRows);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/DeleteRoadDelegation, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RoadDelegation/GetArrangementDetails")]
        public IHttpActionResult GetArrangementDetails(int orgId)
        {
            try
            {
                List<DelegationArrangementDetails> objdDelegationArrLst = RoadDelegation.Instance.GetArrangementDetails(orgId);
                return Ok(objdDelegationArrLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/GetArrangementDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("RoadDelegation/GetOrganisationGeoRegion")]
        public IHttpActionResult GetOrganisationGeoRegion(int orgId)
        {
            try
            {
                RoadDelegationOrgSummary objOrgGeoDetails = RoadDelegation.Instance.GetOrganisationGeoRegion(orgId);
                return Ok(objOrgGeoDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/GetOrganisationGeoRegion, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RoadDelegation/GetLinksAllowedForDelegation")]
        public IHttpActionResult GetLinksAllowedForDelegation(GetLinksAllowedForDelegationParams getLnksAllwdForDelegParams)
        {            
            try
            {                
                List<long> allowedLinkIdLst = RoadDelegation.Instance.GetLinksAllowedForDelegation(getLnksAllwdForDelegParams.LinkIdList, getLnksAllwdForDelegParams.FromOrgId);
                return Ok(allowedLinkIdLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/GetLinksAllowedForDelegation, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RoadDelegation/CreateRoadDelegation")]
        public IHttpActionResult CreateRoadDelegation(RoadDelegationData roadDelegationObj)
        {
            bool createFlag = false;
            try
            {
                createFlag = RoadDelegation.Instance.CreateRoadDelegation(roadDelegationObj);
                return Content(HttpStatusCode.Created, createFlag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/CreateRoadDelegation, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RoadDelegation/FetchRoadInfoToDisplayOnMap")]
        public IHttpActionResult FetchRoadInfoToDisplayOnMap(FetchRoadInfoParams fetchRoadInfoParams)
        {
            try
            {
                sdogeometry areaGeomObj = JsonConvert.DeserializeObject<sdogeometry>(fetchRoadInfoParams.AreaGeometryStr.ToString());
                List<LinkInfo> roadLinkInfoLst = RoadDelegation.Instance.FetchRoadInfoToDisplayOnMap(fetchRoadInfoParams.ArrangementId, fetchRoadInfoParams.ZoomLevel, fetchRoadInfoParams.SearchFlag, areaGeomObj, fetchRoadInfoParams.SearchParam);
                return Ok(roadLinkInfoLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/FetchRoadInfoToDisplayOnMap, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("RoadDelegation/EditRoadDelegation")]
        public IHttpActionResult EditRoadDelegation(RoadDelegationDataMapperInput inputModel)
        {
            try
            {
                string jsonData = ConvertToModelFromCompressedData(inputModel.CompressedRoadDelegationString);
                bool editFlag;
                try
                {
                    RoadDelegationDataMapper newRoadDelegObj = JsonConvert.DeserializeObject<RoadDelegationDataMapper>(jsonData);
                    editFlag = RoadDelegation.Instance.EditRoadDelegation(newRoadDelegObj.RoadDelegationData);
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveRoute, Exception:" + ex);
                    return Json(new { result = false });
                }
                return Ok(editFlag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/EditRoadDelegation, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        private static string ConvertToModelFromCompressedData(string roadDelegationObj)
        {
            string jsonData = "";
            byte[] compressedBytes = Convert.FromBase64String(roadDelegationObj);

            // Decompress the data
            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (GZipStream decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedStream);
                    }

                    // Convert the decompressed data to a string
                    jsonData = Encoding.UTF8.GetString(decompressedStream.ToArray());
                    // Deserialize the JSON data into the RoutePart object

                }
            }

            return jsonData;
        }
    }
}