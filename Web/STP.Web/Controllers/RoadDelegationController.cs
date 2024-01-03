using System;
using System.Collections.Generic;
using STP.Common.Logger;
using System.Web.Mvc;
using STP.Domain.SecurityAndUsers;
using STP.Domain.RoadNetwork.RoadDelegation;
using PagedList;
using STP.ServiceAccess.RoadNetwork;
using STP.Domain.RoadNetwork.RoadOwnership;
using NetSdoGeometry;
using STP.Web.WorkflowProvider;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace STP.Web.Controllers
{
    public class RoadDelegationController : Controller
    {
        private readonly IRoadDelegationService roadDelegationService;


        public RoadDelegationController(IRoadDelegationService roadDelegationService)
        {
            this.roadDelegationService = roadDelegationService;
        }
        public ActionResult GetRoadDelegationList(RoadDelegationSearchParam searchParam, int? page, int? pageSize, int? sortOrder = 0, int? sortType = 0,bool isClear=false)
        {
            try
            {
                #region Session Check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                {
                    return RedirectToAction("Login", "Account");
                }                
                #endregion

                #region Paging Part
                int pageNumber = (page ?? 1);

                if (pageSize == null)
                {
                    if (Session["PageSize"] == null)
                    {
                        Session["PageSize"] = 10;
                    }
                    pageSize = (int)Session["PageSize"];
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }
                if(!isClear)
                {
                    searchParam =searchParam!=null && string.IsNullOrEmpty(searchParam.SearchText) && Session["SearchParam"] != null ? (RoadDelegationSearchParam)Session["SearchParam"]:searchParam;
                    sortOrder =sortOrder==null && Session["SortOrder"] != null ? (int)Session["SortOrder"]:sortOrder;
                    sortType =sortType==null && Session["SortType"] != null ? (int)Session["SortType"]:sortType;
                    pageNumber =pageNumber==0 && Session["PageNumber"] != null ? (int)Session["PageNumber"]:pageNumber;
                }

                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;
                #endregion

                if (searchParam.SearchText != null)
                    ViewBag.SearchString = searchParam.SearchText;
                else
                    ViewBag.SearchString = null;
                
                sortOrder = sortOrder != null && sortOrder!=0 ? (int)sortOrder : 1; //arrangement name
                sortType = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = sortType;
                
                long totalCount = 0;
                List<RoadDelegationData> roadDelegationList = roadDelegationService.GetRoadDelegationList(searchParam, (int)pageSize, pageNumber, (int)sortOrder,(int)sortType);
                totalCount =roadDelegationList != null && roadDelegationList.Count > 0 ? roadDelegationList[0].TotalRecordCount:0;
                ViewBag.TotalCount = totalCount;
                var pagedRoadDelegationList = new StaticPagedList<RoadDelegationData>(roadDelegationList, pageNumber, (int)pageSize, (int)totalCount);
                Session["SearchParam"] = searchParam;
                Session["SortOrder"] = sortOrder;
                Session["SortType"] = sortType;
                Session["PageNumber"] = pageNumber;
                return View("RoadDelegationList", pagedRoadDelegationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetRoadDelegationList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }            
        }

        #region DeleteRoadDelegation
        [HttpPost]
        public ActionResult DeleteRoadDelegation(int arrangementId)
        {
            try
            {
                #region Session Check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                {
                    return RedirectToAction("Login", "Account");
                }               
                #endregion

                int affectedRows = roadDelegationService.DeleteRoadDelegation(arrangementId);
                bool deleteFlag = (affectedRows > 0);
                return Json(new { value = deleteFlag });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetRoadDelegationList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion
        #region CreateRoadDelegation
        public ActionResult CreateRoadDelegation(RoadDelegationData roadDelegationObject)
        {
            try
            {
                #region Session Check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}

                if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                {
                    return RedirectToAction("Login", "Account");
                }
                #endregion

                List<SelectListItem> ContactTypeList = new List<SelectListItem>();
                ContactTypeList.Add(new SelectListItem { Text = "Use default contact", Value = "default" });
                ContactTypeList.Add(new SelectListItem { Text = "Search new contact", Value = "new" });
                ViewBag.ContactTypeList = ContactTypeList;
                if(roadDelegationObject.ArrangementId == 0)
                {
                    Session["actiontype"] = null;
                }

                if(Session["actiontype"] != null)
                {
                    string type = Convert.ToString(Session["actiontype"]);// id will be 10;
                    ViewBag.RetainNotification = roadDelegationObject.RetainNotification;
                    if (type == "Edit")
                    {
                        ViewBag.mode = type;
                        ViewBag.delegId = roadDelegationObject.ArrangementId;
                    }
                    else
                    {
                        if (TempData["Usertype"] != null)
                        {
                            string user = Convert.ToString(TempData["Usertype"]);
                            ViewBag.orginflag = user;
                        }
                        else
                        {
                            ViewBag.orginflag = "";
                        }
                        ViewBag.mode = type;
                        ViewBag.delegId = roadDelegationObject.ArrangementId;
                    }
                }
                else
                {
                    ViewBag.mode = "Create";
                    ViewBag.delegId = 0;
                }



                return View(roadDelegationObject);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/CreateRoadDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region ListOrganisation
        [HttpGet]
        public ActionResult ListOrganisation(int page = 1, int pageSize = 10, int searchFlag = 1, string SearchString = null, string origin = "")
        {
            try
            {
                #region Session Check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                #endregion
                #region Page access check
                if (!PageAccess.GetPageAccess("100002"))
                {
                    return RedirectToAction("Error", "Home");
                }
                #endregion

                if (ModelState.IsValid)
                {
                    #region Paging Part
                    int pageNumber = page;
                    ViewBag.searchFlag = searchFlag;
                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;
                    #endregion

                    if (ViewBag.OrgSearchString == null && SearchString != null)
                    {
                        ViewBag.OrgSearchString = SearchString;
                    }
                    if (SearchString == null)
                    {
                        SearchString = Convert.ToString(ViewBag.OrgSearchString);
                    }
                    else
                    {
                        SearchString = SearchString.Trim();
                        ViewBag.OrgSearchString = SearchString;
                    }

                    ViewBag.origin = origin;
                    List<RoadDelegationOrgSummary> orgListSummary = roadDelegationService.GetRoadDelegationOrganisations(SearchString, page, pageSize, searchFlag);

                    List<RoadOwnershipOrgSummary> orgListSummary1 = roadDelegationService.GetOrganisations(SearchString, page, pageSize, searchFlag);
                    if (orgListSummary != null && orgListSummary.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(orgListSummary[0].TotalRows);
                        IPagedList<RoadDelegationOrgSummary> model = orgListSummary.ToPagedList(page, 10);
                        ViewBag.TotalPages = model.PageCount;
                    }
                    else
                    {
                        ViewBag.TotalCount = 0;
                        ViewBag.TotalPages = 0;
                    }


                    

                    //var OrganisationListAsIPagedList = new StaticPagedList<RoadOwnershipOrgSummary>(orgListSummary, pageNumber, pageSize, ViewBag.TotalCount);
                    return Json(orgListSummary, "application/json", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region EditRoadDelegation        
        public ActionResult EditRoadDelegation(int arrangementId)
        {
            try
            {
                #region Session Check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                {
                    return RedirectToAction("Login", "Account");
                }
                #endregion

                List<SelectListItem> ContactTypeList = new List<SelectListItem>();
                ContactTypeList.Add(new SelectListItem { Text = "Use default contact", Value = "default" });
                ContactTypeList.Add(new SelectListItem { Text = "Search new contact", Value = "new" });
                Session["actiontype"] = "Edit";

                ViewBag.ContactTypeList = ContactTypeList;
                ViewBag.delegId = arrangementId;

                RoadDelegationData roadDelegationData = null;
                //roadDelegationData = roadDelegationService.GetRoadDelegationDetailsWithLinkInfo(arrangementId);
                roadDelegationData= roadDelegationService.GetRoadDelegationDetails(arrangementId);
                ViewBag.mode = "Edit";
                ViewBag.Linklist = roadDelegationData.LinkIdList;
                return RedirectToAction("CreateRoadDelegation", roadDelegationData);

                //return View(roadDelegationData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/EditRoadDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ViewRoadDelegation
        [HttpGet]
        public ActionResult ViewRoadDelegation(int arrangementId, int viewFlag, string orginflag = "")
        {
            try
            {
                #region Session Check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                //if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion
                RoadDelegationData roadDelegationData = null;
                List<SelectListItem> ContactTypeList = new List<SelectListItem>();
                ContactTypeList.Add(new SelectListItem { Text = "Use default contact", Value = "default" });
                ContactTypeList.Add(new SelectListItem { Text = "Search new contact", Value = "new" });
                ViewBag.ContactTypeList = ContactTypeList;
                ViewBag.delegId = arrangementId;
                ViewBag.orginflag = orginflag;
                Session["actiontype"] = "View";
                
                if (viewFlag == 1)
                {
                    roadDelegationData = roadDelegationService.GetRoadDelegationDetailsWithLinkInfo(arrangementId);
                }
                else
                {
                    roadDelegationData = roadDelegationService.GetRoadDelegationDetails(arrangementId);
                }

                if (orginflag == "SOA")
                {
                    TempData["Usertype"] = "SOA";
                    if ((long)SessionInfo.OrganisationId != roadDelegationData.FromOrgId && (long)SessionInfo.OrganisationId != roadDelegationData.ToOrgId)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }

                ViewBag.mode = "View";
                return RedirectToAction("CreateRoadDelegation", roadDelegationData);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/ViewRoadDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region FetchOrgGeoRegion
        /// <summary>
        /// this function will fetch geo region of an organisation input
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public JsonResult FetchOrgGeoRegion(int orgId)
        {
            RoadDelegationOrgSummary roadOrgSummary = roadDelegationService.GetOrganisationGeoRegion(orgId);
            return Json(new { result = roadOrgSummary });
        }
        #endregion
        #region GetRoadDelegationDetails 
        public JsonResult GetRoadDelegationDetails(int arrangementId, int fetchFlag, sdogeometry areaGeom, RoadDelegationSearchParam searchParam, int zoomLevel = 1)
        {
            sdogeometry Geom = new sdogeometry();
            RoadDelegationSearchParam DelegationSearchParam = new RoadDelegationSearchParam();
            List<Domain.RoadNetwork.RoadDelegation.LinkInfo> roadLinkInfo = roadDelegationService.FetchRoadInfoToDisplayOnMap(arrangementId, zoomLevel, fetchFlag, areaGeom, searchParam);
            var jsonResult = Json(roadLinkInfo, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
    }
        #endregion

        #region SaveRoadDelegation
        [HttpPost]
        public ActionResult SaveRoadDelegation(int postFlag, RoadDelegationData roadDelegationObject, List<Domain.RoadNetwork.RoadDelegation.LinkInfo> roadLinkInfo, int len)
        {
            try
            {
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                {
                    return RedirectToAction("Login", "Account");
                }

                if (postFlag == -1)
                {
                    Session["roadDelegationObject"] = roadDelegationObject;
                    Session["linkIdInfo"] = null;
                    return null;
                }
                else
                {
                    if (len != -1)
                    {
                        List<Domain.RoadNetwork.RoadDelegation.LinkInfo> completeList;
                        if (Session["linkIdInfo"] == null)
                        {
                            completeList = new List<Domain.RoadNetwork.RoadDelegation.LinkInfo>();
                        }
                        else
                        {
                            completeList = (List<Domain.RoadNetwork.RoadDelegation.LinkInfo>)Session["linkIdInfo"];
                        }
                        completeList.AddRange(roadLinkInfo);
                        Session["linkIdInfo"] = completeList;

                        if (completeList.Count == len)
                        {
                            RoadDelegationData newRoadDelegObj = (RoadDelegationData)Session["roadDelegationObject"];
                            newRoadDelegObj.LinkInfoList = (List<Domain.RoadNetwork.RoadDelegation.LinkInfo>)Session["linkIdInfo"];
                            bool creatingStatus = roadDelegationService.CreateRoadDelegation(newRoadDelegObj);
                            return Json(new { result = true, value = creatingStatus });
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else // the following portion is for delegate all functionality.
                    {
                        bool creatingStatus = roadDelegationService.CreateRoadDelegation(roadDelegationObject);
                        return Json(new { result = true, value = creatingStatus });
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/SaveRoadDelegation, Exception: {0}", e));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion


        #region IsDelegationAllowed
        public JsonResult IsDelegationAllowed(int orgId)
        {
            bool status = false;

            status = roadDelegationService.IsDelegationAllowed(orgId);
            return Json(new { val = status });
        }

        #endregion

        [HttpPost]
        public ActionResult UpdateRoadDelegation(string roadDelegationData)
        {
            try
            {
                if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                {
                    return RedirectToAction("Login", "Account");
                }
                try
                {
                    bool updatingStatus = roadDelegationService.EditRoadDelegation(roadDelegationData);
                    return Json(new { result = true, value = updatingStatus });
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveRoute, Exception:" + ex);
                    return Json(new { result = false });
                }
            }
            catch (Exception e)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/UpdateRoadDelegation, Exception: {0}", e));
                return RedirectToAction("Error", "Home");
            }
        }


        #region CheckPartialDelegation
        [HttpPost]
        public ActionResult CheckPartialDelegation(int orgId)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                List<DelegationArrangementDetails> delegDetails = roadDelegationService.GetArrangementDetails(orgId);
                return Json(new { delegDetails });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/CheckPartialDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region GetLinksAllowedForDelegation
        public ActionResult GetLinksAllowedForDelegation(List<long> linkIdList, int fromOrgId)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!(PageAccess.GetPageAccess("100001") || PageAccess.GetPageAccess("12005")))
                {
                    return RedirectToAction("Login", "Account");
                }

                List<long> linkIds = roadDelegationService.GetLinksAllowedForDelegation(linkIdList, fromOrgId);
                return Json(new { result = linkIds });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadDelegation/GetLinksAllowedForDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion


    }
}