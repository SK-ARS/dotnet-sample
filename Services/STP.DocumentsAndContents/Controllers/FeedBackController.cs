using STP.Common.Constants;
using STP.Common.Logger;
using STP.DocumentsAndContents.Providers;
using STP.Domain.DocumentsAndContents;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.DocumentsAndContents.Controllers
{
    public class FeedBackController : ApiController
    {
        [HttpGet]
        [Route("FeedBack/GetFeedbackSearchInfo")]
        public IHttpActionResult GetFeedbackSearchInfo(int pageNumber, int pageSize, string searchtype, int flag, string searchString, int sortOrder, int presetFilter)
        {
            //Note : Flag =0 ---list, Flag = 1---search
            //Search type 1 - Complaint, 2 - Suggestions, 3 - General Complaint , 4 - Fault
            try
            {
                List<FeedbackDomain> feedBacks = FeedbackProvider.Instance.GetFeedbackSearchInfo(pageNumber,pageSize, searchtype, flag, searchString, sortOrder, presetFilter);
                return Content(HttpStatusCode.OK, feedBacks);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - FeedBack/GetFeedbackSearchInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("FeedBack/DeleteFeedbackDetails")]
        public IHttpActionResult  DeleteFeedbackDetails(int feedBackId)
        {
          
            try
            {
               int  result = FeedbackProvider.Instance.DeleteFeedbackDetails( feedBackId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - FeedBack/DeleteFeedbackDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("FeedBack/GetFeedbackInfo")]
        public IHttpActionResult GetFeedbackInfo(long feedBackId, int openChk)
        {

            try
            {
                FeedbackDomain result = FeedbackProvider.Instance.GetFeedbackInfo( feedBackId,  openChk);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - FeedBack/GetFeedbackInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
