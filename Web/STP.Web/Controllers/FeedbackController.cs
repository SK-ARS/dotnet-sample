using System.Threading.Tasks;
using System.Web.Mvc;
using STP.Domain;
using System.Net.Http;
using System.Configuration;
using STP.Web.Filters;
using System.Collections.Generic;
using System;
using PagedList;
using STP.Domain.DocumentsAndContents;
using STP.ServiceAccess.DocumentsAndContents;

namespace STP.Web.Controllers
{
    [AuthorizeUser(Roles = "11003")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService feedbackService;
        public FeedbackController()
        {

        }
        public FeedbackController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        #region ActionResult ListFeedback(int? page, int? pageSize, string searchType = null, int flag = 0, string searchString = null)

        public ActionResult ListFeedback(int page = 1, int? pageSize = 10, string searchType = null, int flag = 0, string searchString = null, FeedbackDomain objfeedSearch = null, int? sortType = null, int? sortOrder = null, bool isClear = false) // flag=1 then it for search flag=0 then it is for showing entire list
        {
            List<FeedbackDomain> feedbackObj;
            int pageNumber = page;
            if (Session["PageSize"] == null)
            {
                Session["PageSize"] = 10;
            }
            if ((pageSize != null)&&(searchType==null))
            {
                pageSize = (int)Session["PageSize"];
            }
                if (pageSize == null)
            {
                pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
            }
            
            //else
            //{
            //    pageSize = (int)Session["PageSize"];
            //}
            if (!isClear)
            {
                objfeedSearch = objfeedSearch != null && (string.IsNullOrEmpty(objfeedSearch.SearchType) || objfeedSearch.SearchType == "0")
                    && Session["g_FeedbackSearch"] != null ? (FeedbackDomain)Session["g_FeedbackSearch"] : objfeedSearch;
                sortOrder = sortOrder == null && Session["SortOrder"] != null ? (int)Session["SortOrder"] : sortOrder;
                sortType = sortType == null && Session["SortType"] != null ? (int)Session["SortType"] : sortType;
                pageNumber = pageNumber == 0 && Session["PageNumber"] != null ? (int)Session["PageNumber"] : pageNumber;
            }
            else
            {
                objfeedSearch = new FeedbackDomain();
            }

            //Setting Pagination
            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;

            sortOrder = sortOrder != null ?(int)sortOrder :2; //name
            int presetFilter = sortType != null ?(int)sortType :1; // asc
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortType = presetFilter;

            int totalCount = 0;
            if (objfeedSearch.SearchType != "0" && objfeedSearch.SearchType != null)//to searching data
            {
                feedbackObj = feedbackService.GetFeedbackSearchInfo(pageNumber, (int)pageSize, objfeedSearch.SearchType, 1, objfeedSearch.SearchString, sortOrder.Value, presetFilter);
                if (feedbackObj.Count > 0)
                {
                    totalCount = feedbackObj[0].ListCount;

                }
            }
            else//to show entire data
            {
                feedbackObj = feedbackService.GetFeedbackSearchInfo(pageNumber, (int)pageSize, objfeedSearch.SearchType, 0, objfeedSearch.SearchString, sortOrder.Value, presetFilter);
                if (feedbackObj.Count > 0)
                {
                    totalCount = feedbackObj[0].ListCount;

                }
            }
            ViewBag.SearchFlag = flag;
            ViewBag.Searchtype = objfeedSearch.SearchType;
            ViewBag.searchString = objfeedSearch.SearchString;

            ViewBag.TotalCount = totalCount;
            var feedAsIPagedList = new StaticPagedList<FeedbackDomain>(feedbackObj, pageNumber, (int)pageSize, totalCount);
            Session["g_FeedbackSearch"] = objfeedSearch;
            return View(feedAsIPagedList);
        }


        #endregion

        #region ActionResult MostRecentFeedbacks(int page=1, int pageSize=10, string searchType = null, int flag = 0, string searchString = null)

        public ActionResult MostRecentFeedbacks(string searchType = null, int flag = 0, string searchString = null, FeedbackDomain objfeedSearch = null, int? sortType = null, int? sortOrder = null) // flag=1 then it for search flag=0 then it is for showing entire list
        {
            List<FeedbackDomain> feedbackObj;
            int pageNumber = 1;
            int pageSize = 4;
            int presetFilter = 0;
            int SORTOrder = 1;
            Session["g_FeedbackSearch"] = objfeedSearch;
            if (Session["g_FeedbackSearch"] != null)
            {
                objfeedSearch = (FeedbackDomain)Session["g_FeedbackSearch"];
            }

            int totalCount = 0;
            SORTOrder = sortOrder != null ? (int)sortOrder : 2;
            presetFilter = sortType != null ? (int)sortType : 1;

            if (objfeedSearch.SearchType != "0" && objfeedSearch.SearchType != null)//to searching data
            {
                feedbackObj = feedbackService.GetFeedbackSearchInfo(pageNumber, pageSize, objfeedSearch.SearchType, 1, objfeedSearch.SearchString, SORTOrder, presetFilter);
                if (feedbackObj.Count > 0)
                {
                    totalCount = feedbackObj[0].ListCount;

                }
            }
            else//to show entire data
            {
                feedbackObj = feedbackService.GetFeedbackSearchInfo(pageNumber, pageSize, searchType, 0, objfeedSearch.SearchString, SORTOrder, presetFilter);
                if (feedbackObj.Count > 0)
                {
                    totalCount = feedbackObj[0].ListCount;

                }
            }

            var feedAsIPagedList = new StaticPagedList<FeedbackDomain>(feedbackObj, pageNumber, pageSize, totalCount);
            return PartialView(feedAsIPagedList);
        }


        #endregion

        #region ActionResult ViewFeedbackPopup(long feedid,int openChk=0)
        public ActionResult ViewFeedbackPopup(long feedid, int openChk = 0)//to checke wheather the feedback is read or unread
        {

            FeedbackDomain ObjFeedback = feedbackService.GetFeedbackInfo(feedid, openChk);//openChk is 0 if feedback unread and 1 if read
            return PartialView(ObjFeedback);
        }
        #endregion


        #region JsonResult DeleteFeedbackDetails(int feedBackId, string FeedbackTypeName)

        public JsonResult DeleteFeedbackDetails(int feedBackId, string FeedbackTypeName)
        {
            

            int result =feedbackService.DeleteFeedbackDetails(feedBackId);
                                                                          
            if (result == 1)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

           
        }
        #endregion
    }
}