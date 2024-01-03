using STP.Common.Constants;
using STP.Common.Logger;
using STP.DocumentsAndContents.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Domain;
using STP.Domain.DocumentsAndContents;

namespace STP.DocumentsAndContents.Controllers
{
    public class InformationController : ApiController
    {
        #region DownloadList
        [HttpGet]
        [Route("Information/DownloadList")]
        public IHttpActionResult DownloadList(int pageNumber, int pageSize, string pageName, string contentType, int userType, int isAdmin, int sortOrder, int presetFilter)
        {
            try
            {
                List<InformationModel> informationList = InformationProvider.Instance.GetInformationList(pageNumber, pageSize, pageName, contentType, userType, sortOrder, presetFilter);
                return Content(HttpStatusCode.OK, informationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Information/GetDownloadList , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
                
            }
        }
        #endregion
        #region DeleteInformation
        [HttpDelete]
        [Route("Information/DeleteInformation")]
        public IHttpActionResult DeleteInformation(int deletedContactId)
        {
            try
            {
                int result = InformationProvider.Instance.DeleteInformation(deletedContactId);
                if(result<0)
                {
                    return Content(HttpStatusCode.InternalServerError,StatusMessage.DeletionFailed);
                }
                else if(result==0)
                {
                    return Content(HttpStatusCode.OK,result);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/DeleteInformation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetEnumValsListByEnumType
        [HttpGet]
        [Route("Information/GetEnumValsListByEnumType")]
        public IHttpActionResult GetEnumValsListByEnumType(string EnumTypeName)
        {
            try
            {
                var result = InformationProvider.Instance.GetEnumValsListByEnumType(EnumTypeName);
                return Content(HttpStatusCode.Created, result);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/GetEnumValsListByEnumType,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAssociatedFilesByContentId
        [HttpGet]
        [Route("Information/GetAssociatedFilesByContentId")]
        public IHttpActionResult GetAssociatedFilesByContentId(int CONTENT_ID)
        {
            try
            {
                var result = InformationProvider.Instance.GetAssociatedFilesByContentId(CONTENT_ID);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/GetAssociatedFilesByContentId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

      
        #endregion
        #region GetPortalContentById
        [HttpGet]
        [Route("Information/GetPortalContentById")]
        public IHttpActionResult GetPortalContentById(int CONTENT_ID)
        {
            try
            {
                var result = InformationProvider.Instance.GetPortalContentById(CONTENT_ID);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/GetPortalContentById,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetInformationById
        [HttpGet]
        [Route("Information/GetInformationById")]
        public IHttpActionResult GetInformationById(int managedContentId)
        {
            try
            {
                var result = InformationProvider.Instance.GetInformationById(managedContentId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/GetInformationById,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CreateEditNewsList
        [HttpPost]
        [Route("Information/PublishInformation")]
        public IHttpActionResult PublishInformation(InformationModel informationModel)
        {
            try
            {
                InformationModel objInformationModel = InformationProvider.Instance.ManageInformation(informationModel);
                return Content(HttpStatusCode.Created, objInformationModel);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/PublishInformation,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("Information/PublishInformationFiles")]
        public IHttpActionResult PublishInformationFiles(InformationModel informationModel)
        {
            try
            {
                bool? status = InformationProvider.Instance.ManageInformationFiles(informationModel);
                return Content(HttpStatusCode.Created, status);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/PublishInformationFiles,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #endregion
        #region GetDownloadList
        [HttpGet]
        [Route("Information/GetDownloadList")]
        public IHttpActionResult GetDownloadList(int pageNum, int pageSize, string pageName, string contentType, int userType, int isAdmin, string downloadType)
        {
            try
            {
                var result = InformationProvider.Instance.GetDownloadList(pageNum, pageSize, pageName, contentType, userType, isAdmin, downloadType);
                return Ok(result);
               
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/GetDownloadList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAllHotNews
        [HttpGet]
        [Route("Information/GetAllHotNews")]
        public IHttpActionResult GetAllHotNews(long userTypeId)
        {          
            try
            {
                InformatinDetail result = InformationProvider.Instance.GetAllHotNews(userTypeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Movements/GetAllHotNews, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetInformationList
        [HttpGet]
        [Route("Information/GetInformationList")]
        public IHttpActionResult GetInformationList(int pageNumber, int pageSize, string pageName, string contentType, int userType, int sortOrder, int presetFilter)
        {
            try
            {
                var result = InformationProvider.Instance.GetInformationList(pageNumber, pageSize, pageName, contentType, userType, sortOrder, presetFilter);
                    return Ok(result);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/GetInformationList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion
        #region GetInformationList By Portal
        [HttpGet]
        [Route("Information/GetInformationListPortal")]
        public IHttpActionResult GetInformationListPortal(int pageNumber, int pageSize, string contentType,long userType)
        {
            try
            {
                List<InformationModel> result = InformationProvider.Instance.GetInformationListPortal(pageNumber, pageSize, contentType, userType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Information/GetInformationListPortal, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion

        #region GetUniqueInfoList
        [HttpGet]
        [Route("Information/GetUniqueInfoList")]
        public IHttpActionResult GetUniqueInfoList(int pageNumber, int pageSize, int portalId, string searchType)
        {
            try
            {
                List<InformationModel> result = InformationProvider.Instance.GetUniqueInfoList(pageNumber, pageSize, portalId, searchType);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Information/GetUniqueInfoList , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion


        [HttpGet]
        [Route("Information/GetHotNewsForAdmin")]
        public IHttpActionResult GetHotNewsForAdmin(string SearchType)
        {
            try
            {
                List<InformatinDetail> informationList = InformationProvider.Instance.GetHotNewsForAdmin(SearchType);
                return Content(HttpStatusCode.OK, informationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Information/GetHotNewsForAdmin , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }
        [HttpGet]
        [Route("Information/GetLatestNews")]
        public IHttpActionResult GetLatestNews(int portalId, int timeInterval)
        {
            try
            {
                List<LatestNews> informationList = InformationProvider.Instance.GetLatestNews(portalId, timeInterval);
                return Content(HttpStatusCode.OK, informationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Information/GetLatestNews , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);

            }
        }
    }

}
