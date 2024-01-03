using PagedList;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.SecurityAndUsers;
using STP.Web.Filters;
using STP.Web.Helpers;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace STP.Web.Controllers
{
    public class OrganisationController : Controller
    {
        // GET: Organisation
        private readonly IUserService userService;
        private readonly IOrganizationService organizationService;
        private readonly ILoggingService loggingService;
        private readonly IReportService reportService;
        public OrganisationController(IUserService userService, ILoggingService loggingService, IOrganizationService organizationService, IReportService reportService)
        {
            this.userService = userService;
            this.organizationService = organizationService;
            this.loggingService = loggingService;
            this.reportService = reportService;
        }

        public OrganisationController()
        {

        }

        public ActionResult CreateOrganisation(string mode, string organisationId = "", string sortApplication = "", int revisionId = 0, string sortStatus = "", bool showHaulierCount = false)
        {

            ViewBag.mode = mode;
            ViewBag.SortStatus = sortStatus;
            ViewBag.orgID = organisationId;
            if (sortApplication != null && sortApplication != "")
            {
                ViewBag.sortApp = sortApplication;
                ViewBag.RevisionId = revisionId;
                ViewBag.showHaulCnt = showHaulierCount;
            }

            var OrgTypeList = userService.GetOrganisationTypeList();
            SelectList orgType = new SelectList(OrgTypeList, "OrgTypeCode", "OrgTypeName");
            ViewBag.OrgTypeList = orgType;

            //Filling Country List Drop Down From XML
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Configuration.xml");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            //Filling Country List Drop Down From database
            var CountryList = userService.GetCountryInfo();
            SelectList CountrySelectList = new SelectList(CountryList, "CountryID", "Country");
            ViewBag.CountryDropDown = CountrySelectList;
            Organization orgDet = new Organization();
            if (Session["objOrg"] != null)
            {
                //Code to retain the values in text box
                orgDet = (Organization)Session["objOrg"];
                Session["objOrg"] = null;
                return PartialView("PartialView/_CreateOrganisation", orgDet);
            }
            else
            {
                return PartialView("PartialView/_CreateOrganisation", orgDet);
            }
        }
        public JsonResult CheckOrganisationExists(string OrganisationName, int type, string mode = "Create", string orgID = "")
        {
            decimal isOrganisationExists = organizationService.GetOrganizationByName(OrganisationName, type, mode, orgID);
            return Json(new { result = isOrganisationExists });
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult SaveOrganisation(Organization orgDet, string mode, string orgID = "", string sortApp = "", int RevisionId = 0, string SORTStatus = "")
        {
            //validate authentication key
            string authKey = orgDet.AuthenticationKey;
            if (authKey != null)
            {
                string keyValidator = orgDet.KeyValidator;
                string checksum = EncryptionUtility.Encrypt(authKey, false);
                if (keyValidator != checksum)
                {
                    return Json(new { result = false });
                }
            }

            var sessionValues = (UserInfo)Session["UserInfo"];
            int user_ID = Convert.ToInt32(sessionValues.UserId);
            string ErrMsg = string.Empty;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
            string saveflag = "false";
            int successmsg = 0;
            int savedorgId = 0;
            if (ModelState.IsValid)
            {
                if (mode == "Edit")
                {
                    userService.EditOrganisation(orgID, orgDet);
                    savedorgId = Convert.ToInt32(orgID);
                    //TempData["Success"] = "2";
                    //TempData["OrgName"] = orgDet.OrgName;

                    #region Saving updating orgnisation in sys_event
                    movactiontype.SystemEventType = SysEventType.amended_organisation;
                    movactiontype.OrganisatioName = orgDet.OrgName;
                    string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, Convert.ToInt32(orgID), sessionValues.UserSchema);
                    #endregion

                }
                else
                {
                    savedorgId = organizationService.SaveOrganization(orgDet);
                    if (successmsg != 0)
                    {
                        saveflag = "true";
                    }
                    #region Saving creating orgnisation in sys_event
                    movactiontype.SystemEventType = SysEventType.added_organisation;
                    movactiontype.OrganisatioName = orgDet.OrgName;
                    string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, 0, sessionValues.UserSchema);
                    #endregion

                    //TempData["Success"] = "1";
                    //TempData["OrgName"] = orgDet.OrgName;
                }

            }

            if (mode == "SORTSO" || sortApp == "SORTSO")
            {
                bool isSuccess = false;
                try
                {
                    orgDet.OrgType = "237013";
                    savedorgId = organizationService.SaveOrganization(orgDet);
                    TempData["Success"] = "1";

                    Session["objOrg"] = orgDet;
                    Session["SORTOrgID"] = savedorgId;
                    #region log event
                    UserInfo SessionInfo = null;
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    string description = orgDet.OrgName + " organisation created successfully by " + SessionInfo.UserName;
                    reportService.LogEvent(401032, Convert.ToInt32(SessionInfo.UserId), description);
                    #endregion log event
                    isSuccess = true;
                    return Json(new { result = isSuccess, OrgId = savedorgId });
                }
                catch (Exception)
                {

                    return Json(new { result = isSuccess });
                }
            }
            else
            {
                bool isSuccess = true;
                ViewBag.OrgID = savedorgId;
                return Json(new { result = isSuccess, OrgId = savedorgId });

                // return RedirectToAction("ListOrganisation", "Organisation", new { save = saveflag });
            }
        }
        [AuthorizeUser(Roles = "11002,13003,13004,13005,13006")]
        public ActionResult ListOrganisation(int? page, int? pageSize, string SearchString = null, string save = "false"
            , string SORT = "", string SearchOrganisation = null, int? sortType = null, int? sortOrder = null, bool isClear = false)
        {
            List<OrganizationGridList> GridListObj = new List<OrganizationGridList>();
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.IsAdmin == 0 && SessionInfo.UserTypeId != 696008)
            {
                return RedirectToAction("Error", "Home");
            }

            if (isClear)
            {
                Session["OrgSearchString"] = null;
                Session["OrgSearchCode"] = null;
            }

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

            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;

            if (SearchString == null)
            {
                SearchString = (string)Session["OrgSearchString"];
            }
            if (SearchOrganisation == null)
            {
                SearchOrganisation = (string)Session["OrgSearchCode"];
            }
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

            if (ViewBag.OrgSearchCode == null && SearchOrganisation != null)
            {
                ViewBag.OrgSearchCode = SearchOrganisation;

            }
            if (SearchOrganisation == null)
            {
                SearchOrganisation = Convert.ToString(ViewBag.OrgSearchCode);
            }
            else
            {
                ViewBag.OrgSearchCode = SearchOrganisation;
            }
            sortOrder = sortOrder != null ? (int)sortOrder : 1; //orgid
            int presetFilter = sortType != null ? (int)sortType : 0; // asc
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortType = presetFilter;

            if (SearchString == null && SearchOrganisation == null)
                GridListObj = organizationService.GetOrganizationInformation(pageNumber, (int)pageSize, SessionInfo.UserTypeId, sortOrder.Value, presetFilter);
            else
                GridListObj = organizationService.GetOrganizationInformation(SearchString, pageNumber, (int)pageSize, SessionInfo.UserTypeId, SearchOrganisation, sortOrder.Value, presetFilter);


            if (TempData["Success"] != null && TempData["Success"] == "1")                     //for save org
            {
                ViewBag.saveMsg = "New organisation '" + "" + "'" + TempData["OrgName"] + "'" + "" + "' created successfully.";
            }

            else if (TempData["Success"] != null && TempData["Success"] == "2")                 //for edit org
            {
                ViewBag.saveMsg = "Organisation '" + "" + "'" + TempData["OrgName"] + "'" + "" + "' saved successfully.";
            }

            Session["OrgSearchString"] = SearchString;
            ViewBag.OrgSearchString = SearchString;
            Session["OrgSearchCode"] = SearchOrganisation;
            ViewBag.OrgSearchCode = SearchOrganisation;
            ViewBag.SORT = SORT;
            ViewBag.sortType = sortType;
            ViewBag.sortOrder = sortOrder;
            long totalCount = 0;
            if (GridListObj != null && GridListObj.Count > 0)
            {
                totalCount = GridListObj[0].TotalRecordCount;
            }
            else
            {
                totalCount = 0;
            }
            if (GridListObj.Count > 0)
            {
                ViewBag.TotalCount = Convert.ToInt32(GridListObj[0].TotalRecordCount);
                totalCount = GridListObj[0].TotalRecordCount;
            }
            else
            {
                ViewBag.TotalCount = 0;
            }
            var organisationPagedList = new StaticPagedList<OrganizationGridList>(GridListObj, pageNumber, (int)pageSize, (int)totalCount);
            return PartialView("PartialView/_OrganisationList", organisationPagedList);
        }

        public ActionResult SaveOrganisationSearch(string searchString = null, string SORT = "", string searchOrganisation = null,
            int? sortType = null, int? sortOrder = null,
            int? page = null, int? pageSize = null)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , JsonResult,OrganisationController/SaveOrganisationSearch ", Session.SessionID));

            Session["OrgSearchString"] = searchString;
            Session["OrgSearchCode"] = searchOrganisation;

            return RedirectToAction("ListOrganisation", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("SearchString=" + searchString +
                        "&SORT=" + SORT +
                        "&SearchOrganisation=" + searchOrganisation +
                        "&sortType=" + sortType +
                        "&sortOrder=" + sortOrder +
                        "&page=" + page +
                        "&pageSize=" + pageSize)
            });
        }

        public ActionResult ViewOrganisation(string mode, string organisationId, string sortApplication = "", int revisionId = 0, string sortStatus = "", bool showHaulierCount = false)
        {

            ViewBag.mode = mode;
            ViewBag.SortStatus = sortStatus;
            ViewBag.orgID = organisationId;
            if (sortApplication != null && sortApplication != "")
            {
                ViewBag.sortApp = sortApplication;
                ViewBag.RevisionId = revisionId;
                ViewBag.showHaulCnt = showHaulierCount;
            }

            var OrgTypeList = userService.GetOrganisationTypeList();
            SelectList orgType = new SelectList(OrgTypeList, "OrgTypeCode", "OrgTypeName");
            ViewBag.OrgTypeList = orgType;

            //Filling Country List Drop Down From XML
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Configuration.xml");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            //Filling Country List Drop Down From database
            var CountryList = userService.GetCountryInfo();
            SelectList CountrySelectList = new SelectList(CountryList, "CountryID", "Country");
            ViewBag.CountryDropDown = CountrySelectList;
            Organization orgDet = new Organization();

            return PartialView("PartialView/_ViewExistingOrganisation", orgDet);
        }

        public ActionResult GetHaulierContactByOrgID(int orgId)
        {
            var infos = userService.GetHaulierContactByOrgID(orgId);
            return Json(new { result = infos });
        }

        public ActionResult ViewOrganisationByID(int orgId, int RevisionId = 0)
        {
            var infos = new List<ViewOrganizationByID>();
            Session["SORTOrgID"] = orgId;
            if (RevisionId == 0)
            {
                infos = organizationService.ViewOrganizationByID(orgId);
            }
            else
            {
                infos = organizationService.ViewOrganisationByIDForSORT(RevisionId);
                Session["SORTOrgID"] = (int)infos[0].OrgId;
            }
            return Json(new { result = infos });
        }

        [AuthorizeUser(Roles = "11002,13003,13004,13005,13006")]
        public ActionResult ManageOrganisation()
        {
            return View();
        }
        public JsonResult GenerateAuthenticationKey()
        {
            System.Guid guid = System.Guid.NewGuid();
            string authKey = guid.ToString();
            string checksum = EncryptionUtility.Encrypt(authKey, false);
            return Json(new { result = authKey, validator = checksum });
        }

        public JsonResult GenerateAuthenticationKeyChecksum(string authenticationKey)
        {
            string checksum = EncryptionUtility.Encrypt(authenticationKey, false);
            return Json(new { authenticationKey = authenticationKey, validator = checksum });
        }
    }
}