using PagedList;
using STP.Common.Logger;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.SecurityAndUsers;
using STP.Web.Filters;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    [AuthorizeUser(Roles = "30000,11000")]
    public class DispensationController : Controller
    {
        private readonly IDispensationService dispensationService;
        private readonly IOrganizationService organizationService;
        public DispensationController() { }
        public DispensationController(IDispensationService dispensationService, IOrganizationService organizationService)
        {
            this.dispensationService = dispensationService;
            this.organizationService = organizationService;
        }

        public ActionResult ListDispensation(int? page, int? pageSize, bool expired = false, int Org_Id = 0, bool hideLayout = false, int analysisId = 0, int? sortOrder = null, int? sortType = null)
        {
            //Verifying session
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "List Dispensation actionResult method started successfully");
                UserInfo SessionInfo = null;
                int usertype = 0;
                SessionInfo = (UserInfo)Session["UserInfo"];
                var sessionValues = (UserInfo)Session["UserInfo"];
                usertype = sessionValues.UserTypeId;
                int organisationId = Convert.ToInt32(SessionInfo.OrganisationId);
                int Grantee_ID = 0;
                sortOrder = sortOrder != null ? (int)sortOrder : 0; //DISPENSATION_ID
                int presetFilter = sortType != null ? (int)sortType : 0; // desc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;

                if (hideLayout)
                {
                    Grantee_ID = Org_Id;
                }

                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = 10;
                }

                if (pageSize == null)
                {
                    pageSize = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }
               
                ViewBag.pageSize = pageSize;
                
                ViewBag.expired = expired;
                int pageNumber = (page ?? 1);
                ViewBag.page = pageNumber;
                int totalCount = 0;
                List<DispensationGridList> dispObjList = null;
             
                DispensationSearchItems objDispSearch = new DispensationSearchItems();
                if (Session["g_DispensationSearch"] != null)
                {
                    objDispSearch = (DispensationSearchItems)Session["g_DispensationSearch"];
                }

                int intExpired = 0;

                #region
                if (objDispSearch.Expired)
                {
                    intExpired = 1;
                }

                if (objDispSearch.SearchType == "1")
                {
                    objDispSearch.DRN = objDispSearch.Criteria;
                }
                else if (objDispSearch.SearchType == "2")
                {
                    objDispSearch.Summary = objDispSearch.Criteria;
                }
                else if (objDispSearch.SearchType == "3")
                {
                    objDispSearch.Authority = objDispSearch.Criteria;
                }
                else if (objDispSearch.SearchType == "4")
                {
                    objDispSearch.Description = objDispSearch.Criteria;
                }
               
                if ((objDispSearch.Criteria == null) && (!objDispSearch.Expired))
                {
                    if (hideLayout)
                    {
                        dispObjList = dispensationService.GetAffDispensationInfo(organisationId, Grantee_ID, pageNumber, (int)pageSize, usertype); // List dispensation for Affected parties
                        if (dispObjList.Count != 0)
                        {
                            totalCount = dispObjList[0].RecordCount;
                        }
                    }
                    
                    else
                    {
                        totalCount = dispensationService.GetSummaryListCount(organisationId, usertype); // added usertype for identifying Haulier/SOA/Police
                        dispObjList = dispensationService.GetDispensationInfo(organisationId, pageNumber, (int)pageSize, usertype,presetFilter,sortOrder); // List dispensation
                    }
                }
                else
                {
                    dispObjList = dispensationService.GetDispensationSearchInfo(organisationId, pageNumber, (int)pageSize, objDispSearch.DRN, objDispSearch.Summary, objDispSearch.Authority, objDispSearch.Description, intExpired, 1, usertype,presetFilter,sortOrder);
                    totalCount = dispObjList[0].ListCount;

                    dispObjList = dispensationService.GetDispensationSearchInfo(organisationId, pageNumber, (int)pageSize, objDispSearch.DRN, objDispSearch.Summary, objDispSearch.Authority, objDispSearch.Description, intExpired, 0, usertype,presetFilter,sortOrder); // Search dispensation
                }
                #endregion

                var dispAsIPagedList = new StaticPagedList<DispensationGridList>(dispObjList, pageNumber, (int)pageSize, totalCount);
                ViewBag.DispensationSearchItems = objDispSearch;
                ViewBag.saveflag = TempData["flag"];
                ViewBag.drnnumber = TempData["drn"];
                ViewBag.mode = TempData["mode"];
                ViewBag.hideLayout = hideLayout;
                ViewBag.analysisId = analysisId;
                return PartialView("PartialView/_ListDispensation", dispAsIPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Home/Index, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeUser(Roles = "30000,30001")]
        public ActionResult ManageDispensation(int? page=1, int? sortOrder = null, int? sortType = null)
        {
            
            ViewBag.saveflag = TempData["flag"];
            ViewBag.drnnumber = TempData["drn"];
            ViewBag.mode = TempData["mode"];
            
            if (TempData["AffectedParties"] != null)
                ViewBag.fromAffectedParties = TempData["AffectedParties"];
            else
                ViewBag.fromAffectedParties = false;
            if (page == null && Session["PreviousMovementListPage"] != null)
                    page = (int)Session["PreviousMovementListPage"];
            int pageNumber = (page ?? 1);
            int? pageSize=null;
            UserInfo SessionInfo = null;
                if (page == null && Session["PreviousMovementListPage"] != null)
                    page = (int)Session["PreviousMovementListPage"];
                SessionInfo = (UserInfo)Session["UserInfo"];
            Session["PreviousMovementListPage"] = pageNumber;
            int maxlist_item = SessionInfo.MaxListItem;
            if (Session["PageSize"] == null)
                Session["PageSize"] = maxlist_item;

            if (pageSize == null)
                pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
            else
                Session["PageSize"] = pageSize;

            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortType = sortType;
            return View();
        }
        public ActionResult SetDispensationSearch(DispensationSearchItems objDispSearch, int granterId = 0, bool hideLayout = false,int? page=1, int? pageSize=10)
        {
            Session["g_DispensationSearch"] = objDispSearch;
            return RedirectToAction("ListDispensation", new
                {
                    B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("Org_Id=" + granterId +
                        "&hideLayout=" + hideLayout +
                        "&page="+page+
                        "&pageSize=" + pageSize +
                        "&sortOrder=" + objDispSearch.SortOrderValue +
                        "&sortType=" + objDispSearch.SortTypeValue)
                });
        }
        [HttpGet]
        public ActionResult CreateDispensation(bool hideLayout = false, int Granter_id = 0, string Grantor_name = "", int NotifID = 0, bool fromAffectedParties = false, string mode="")
        {
            try
            {
                ViewBag.exists = false;
                DispensationGridList dispensationGridList = new DispensationGridList();
                ViewBag.hideLayout = hideLayout;
                ViewBag.Granter_id = Granter_id;
                ViewBag.Grantor_name = Grantor_name;
                ViewBag.NotifID = NotifID;
                ViewBag.fromAffectedParties = fromAffectedParties;
                ViewBag.mode = mode;
                ViewBag.ErrorPage = false;
                return View(dispensationGridList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Home/Index, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult OrganisationSummary(int? page, int? pageSize, string SearchString = null, string save = "false")
        {
            try
            {
                int usertype = 0;

                var sessionValues = (UserInfo)Session["UserInfo"];
                usertype = sessionValues.UserTypeId;
                int pageNumber = (page ?? 1);
                int totalCount = 0;

                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = 10;
                }

                if (pageSize == null)
                {
                    pageSize = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;
                }
                else
                {
                    if (pageSize != 0)
                    {
                        Session["PageSize"] = pageSize;
                    }
                }
                List<DispensationGridList> GridListObj = null;
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
                    ViewBag.OrgSearchString = SearchString;
                }
                GridListObj = dispensationService.GetDispOrganisationInfo(SearchString, pageNumber, pageSize.Value, 0, usertype);
                if (GridListObj != null && GridListObj.Count > 0)
                {
                    totalCount = GridListObj[0].ListCount;
                }
                else
                {
                    totalCount = 0;
                }
                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = 10;
                }
                if (pageSize == null || pageSize == 0)
                {
                    pageSize = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;
                }
                else
                {
                    Session["PageSize"] = pageSize;
                }
                if (GridListObj.Count <= 10 || (Convert.ToString(Session["OrgSearchString"]) != SearchString && SearchString != null))
                {
                    pageNumber = 1;
                }
                else
                {
                    pageNumber = (page ?? 1);
                }
                Session["OrgSearchString"] = SearchString;
                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;
                ViewBag.OrgSearchString = SearchString;
                ViewBag.totalCount = totalCount;
                return Json(GridListObj, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateDispensation(DispensationGridList regdisp, string mode, int Granter_ID = 0, bool hideLayout = false, int NotifID = 0, bool fromAffectedParties = false)
        {
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = (int)SessionInfo.OrganisationId;
            regdisp.OrganisationId = organisationId;
            int userTypeID = 0;
            userTypeID = SessionInfo.UserTypeId;
            bool result = false;
            decimal iExists = 0, iOrgExists = 1;
            string moveStart = regdisp.FromDate + " " + DateTime.Now.ToLongTimeString();
            string moveEnd = regdisp.ToDate + " " + DateTime.Now.ToLongTimeString();            
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Dispensation/CreateDispensation FromDate:{0}", moveStart));
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Dispensation/CreateDispensation ToDate:{0}", moveEnd));
            regdisp.ValidFrom = DateTime.ParseExact(moveStart, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            regdisp.ValidTo = DateTime.ParseExact(moveEnd, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Dispensation/CreateDispensation ValidFrom:{0}", regdisp.ValidFrom));
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Dispensation/CreateDispensation ValidTo:{0}", regdisp.ValidTo));

            UpdateDispensationParams dispensationParam = new UpdateDispensationParams()
            {
                RegisterDispensation = regdisp,
                UserTypeId = userTypeID
            };
            if (hideLayout)
            {
                regdisp.SelectOrganisationId = Granter_ID;
            }
            ViewBag.ErrorPage = false;
            ViewBag.OrgUserExists = false;
            ViewBag.exists = false;
            TempData["mode"] = mode;
            ViewBag.mode = mode;
            ViewBag.hidelayout = hideLayout;
            ViewBag.NotifID = NotifID;
            ViewBag.fromAffectedParties = fromAffectedParties;
            iExists = dispensationService.GetDispensationReferenceNumber(regdisp.DispensationReferenceNo, regdisp.OrganisationId, mode, (long)regdisp.DispensationId);
            if ((regdisp.GrantedBy != null))
            {
                iOrgExists = organizationService.GetOrganizationByName(regdisp.GrantedBy, 1, "add", "");
            }
            if (iExists == 1)
            {
                ViewBag.exists = true;
                ViewBag.ErrorPage = true;
                return View("CreateDispensation", regdisp);
            }
            else if (iOrgExists == 0)
            {
                ViewBag.OrgUserExists = true;
                ViewBag.ErrorPage = true;
                return View("CreateDispensation", regdisp);
            }
            
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    ViewBag.exists = false;
                    ViewBag.OrgUserExists = false;
                    if (mode == "Edit")
                    {

                        int upt = dispensationService.UpdateDispensation(dispensationParam);
                        if (upt >= 1)
                            result = true;

                    }

                    else
                    {

                        result = dispensationService.SaveDispensation(dispensationParam);
                    }  
                        TempData["flag"] = result;
                        TempData["drn"] = regdisp.DispensationReferenceNo;
                        TempData["AffectedParties"] = fromAffectedParties;
                        return Json(new { Success = true }); //RedirectToAction("ManageDispensation", "Dispensation");
                    

                }
                else if(mode == "Edit")
                {
                    ViewBag.exists = false;
                    ViewBag.OrgUserExists = false;
                    int upt = dispensationService.UpdateDispensation(dispensationParam);
                    if (upt >= 1)
                        result = true;

                    TempData["flag"] = result;
                    TempData["drn"] = regdisp.DispensationReferenceNo;
                    //return RedirectToAction("ManageDispensation", "Dispensation");
                    return Json(new { Success = true });

                }
                else if(mode == "create")
                {
                    ViewBag.OrgUserExists = false;
                    ViewBag.exists = false;
                    result = dispensationService.SaveDispensation(dispensationParam);
                    //if (!result)
                    //{
                    //    ViewBag.hideContent = false;
                    //    return View("CreateDispensation", regdisp);
                    //}
                    TempData["flag"] = result;
                    TempData["drn"] = regdisp.DispensationReferenceNo;
                    TempData["AffectedParties"] = fromAffectedParties;
                    return Json(new { Success = true }); //Re
                }
                else
                {
                    ViewBag.ErrorPage = true;
                    return View("CreateDispensation", regdisp);
                }                   

                //}
                //else
                //{
                //    ViewBag.ErrorPage = true;
                //    return View("CreateDispensation", regdisp);
                //}
            }

        }
        public ActionResult VehicleRestricationDetails(bool notifFlag = false)
        {
            ViewBag.notifFlag = notifFlag;
            return PartialView("PartialView/_VehicleRestricationDetails");
        }

        public JsonResult DeleteDispensation(int dispId)
        {
            bool isSuccess = false;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Dispensation/DeleteDispensation JsonResult method started successfully, with parameters dispId:{0}", dispId));
                int result = dispensationService.DeleteDispensation(dispId);
                if (result > -1)
                {
                    isSuccess = true;
                }
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Dispensation/DeleteDispensation JsonResult method completed successfully,dispId:{0}", dispId));
                return Json(new { Success = isSuccess });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Dispensation/DeleteDispensation, Exception: {0}", ex));
                return Json(new { Success = isSuccess });
            }
        }
        public ActionResult EditDispensation(int dispId)
        {
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            int userTypeID = 0;
            userTypeID = SessionInfo.UserTypeId;

            ViewBag.mode = "Edit";
            DispensationGridList dispensationGridList = dispensationService.GetDispensationDetailsObjByID(dispId, userTypeID);
            return View("CreateDispensation", dispensationGridList);
        }

        public ActionResult EditDispensationRestriction(bool notifFlag = false)
        {
            ViewBag.notifFlag = notifFlag;
            return PartialView("PartialView/EditDispensationRestriction");
        }


        public ActionResult ViewDispensation(int dispId = 0, string DispRef = "")
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            DispensationGridList dispObjList = new DispensationGridList();
            if (DispRef != "")
            {
                dispObjList = dispensationService.ViewDispensationInfoByDRN(DispRef, SessionInfo.UserTypeId);
            }
            else
            {
                dispObjList = dispensationService.ViewDispensationInfo(dispId, SessionInfo.UserTypeId);
            }
            return PartialView("PartialView/_ViewDispensationDetails", dispObjList);
        }

    }
}