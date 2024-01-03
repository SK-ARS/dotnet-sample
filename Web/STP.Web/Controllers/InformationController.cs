using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PagedList;
using STP.Common.Logger;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.SignalR;
using STP.Web.Document;
using STP.Web.Filters;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    public class InformationController : Controller
    {
        private const string contentType = "ManagedContentItemTypeEnum";
        private readonly IInformationService informationService;
        private static IHubContext _hubContext;

        public InformationController() { }
        public InformationController(IInformationService informationService)
        {
            this.informationService = informationService;
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<NewsHub>();
        }
        [AuthorizeUser(Roles = "90001")]
        public ActionResult ManageNews(int? page, int? pageSize, int? sortType = null, int? sortOrder = null)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {
                    sortOrder = sortOrder != null && sortOrder != 0 ? (int)sortOrder : 4; //priority
                    int presetFilter = sortType != null ? (int)sortType : 1; //1 - desc  //0 - asc
                    ViewBag.SortOrder = sortOrder;
                    ViewBag.SortType = presetFilter;

                    #region Check Page Request
                    TempData["heading"] = "news";
                    TempData["PageName"] = "news story";
                    TempData["type"] = "NEWS";
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

                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;

                    #endregion

                    List<InformationModel> Informations = informationService.GetInformationList(pageNumber, pageSize.Value, Convert.ToString(TempData["PageName"]), contentType, SessionInfo.UserTypeId, sortOrder.Value, presetFilter);
                    if (Informations.Count > 0)
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    else
                        ViewBag.TotalCount = 0;

                    var InformationList = new StaticPagedList<InformationModel>(Informations, pageNumber, pageSize.Value, ViewBag.TotalCount);

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    #endregion


                    return View(InformationList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/NewsList, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/NewsList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }



        [AuthorizeUser(Roles = "90002")]
        public ActionResult ManageHelpAndInformations(int? page, int? pageSize, int? sortType = null, int? sortOrder = null)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (!PageAccess.GetPageAccess("90002"))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];

                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {

                    #region Check Page Request
                    TempData["heading"] = "help & information";
                    TempData["PageName"] = "info story";
                    TempData["type"] = "INFO";

                    #endregion

                    #region Paging Part
                    int pageNumber = (page ?? 1);

                    if (pageSize == null)
                    {
                        if (Session["PageSize"] == null)
                        {
                            Session["PageSize"] = 10;
                        }
                        pageSize = 10;//(int)Session["PageSize"];
                    }
                    else
                    {
                        Session["PageSize"] = pageSize;
                    }


                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;

                    #endregion

                    int presetFilter = 1;
                    int SORTOrder = 4;
                    SORTOrder = sortOrder != null ? (int)sortOrder : SORTOrder;
                    presetFilter = sortType != null ? (int)sortType : presetFilter;
                    ViewBag.sortType = sortType;
                    ViewBag.sortOrder = SORTOrder;
                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;
                    List<InformationModel> Informations = informationService.GetInformationList(pageNumber, pageSize.Value, Convert.ToString(TempData["PageName"]), contentType, SessionInfo.UserTypeId, SORTOrder, presetFilter);
                    if (Informations.Count > 0)
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    else
                        ViewBag.TotalCount = 0;

                    var InformationList = new StaticPagedList<InformationModel>(Informations, pageNumber, pageSize.Value, ViewBag.TotalCount);

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");

                    #endregion
                    ViewBag.NewsSortOrder = 0;
                    if (sortOrder != null)
                    {
                        ViewBag.NewsSortOrder = sortType;
                    }
                    return View("ManageNews", InformationList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageHelpAndInformations, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageHelpAndInformations, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeUser(Roles = "90003")]
        public ActionResult ManageDocuments(int? page, int? pageSize, int? sortType = null, int? sortOrder = null)
        {
            try
            {

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;

                if (ModelState.IsValid)
                {
                    #region Check Page Request
                    TempData["heading"] = "download";
                    TempData["PageName"] = "download";
                    TempData["type"] = "DOWNLOAD";
                    #endregion

                    #region Paging Part
                    int pageNumber = (page ?? 1);

                    if (pageSize == null)
                    {
                        if (Session["PageSize"] == null)
                        {
                            Session["PageSize"] = 10;
                        }
                        pageSize = 10; ;
                    }
                    else
                    {
                        Session["PageSize"] = pageSize;
                    }

                    int presetFilter = 1;
                    int SORTOrder = 4;
                    SORTOrder = sortOrder != null ? (int)sortOrder : SORTOrder;
                    presetFilter = sortType != null ? (int)sortType : presetFilter;

                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;
                    ViewBag.sortType = sortType;
                    ViewBag.sortOrder = SORTOrder;
                    #endregion

                    List<InformationModel> Informations = informationService.GetInformationList(pageNumber, pageSize.Value, Convert.ToString(TempData["PageName"]), contentType, SessionInfo.UserTypeId, SORTOrder, presetFilter);
                    if (Informations.Count > 0)
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    else
                        ViewBag.TotalCount = 0;

                    var InformationList = new StaticPagedList<InformationModel>(Informations, pageNumber, pageSize.Value, ViewBag.TotalCount);

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    #endregion
                    ViewBag.NewsSortOrder = 0;
                    if (sortOrder != null)
                    {
                        ViewBag.NewsSortOrder = sortType;
                    }
                    return View("ManageNews", InformationList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageDocuments, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageDocuments, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }


        [AuthorizeUser(Roles = "90004")]
        public ActionResult ManageLinks(int? page, int? pageSize, int? sortType = null, int? sortOrder = null)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (!(PageAccess.GetPageAccess("90001") || PageAccess.GetPageAccess("90002") || PageAccess.GetPageAccess("70001") || PageAccess.GetPageAccess("70002")))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {
                    #region Check Page Request
                    TempData["heading"] = "external link";
                    TempData["PageName"] = "link";
                    TempData["type"] = "LINK";
                    #endregion

                    #region Paging Part
                    int pageNumber = (page ?? 1);

                    if (pageSize == null)
                    {
                        if (Session["PageSize"] == null)
                        {
                            Session["PageSize"] = 10;
                        }
                        pageSize = 10;
                    }
                    else
                    {
                        Session["PageSize"] = pageSize;
                    }

                    int presetFilter = 1;
                    int SORTOrder = 4;
                    SORTOrder = sortOrder != null ? (int)sortOrder : SORTOrder;
                    presetFilter = sortType != null ? (int)sortType : presetFilter;

                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;
                    ViewBag.sortType = sortType;
                    ViewBag.sortOrder = SORTOrder;
                    #endregion

                    List<InformationModel> Informations = informationService.GetInformationList(pageNumber, pageSize.Value, Convert.ToString(TempData["PageName"]), contentType, SessionInfo.UserTypeId, SORTOrder, presetFilter);
                    if (Informations.Count > 0)
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    else
                        ViewBag.TotalCount = 0;

                    var InformationList = new StaticPagedList<InformationModel>(Informations, pageNumber, pageSize.Value, ViewBag.TotalCount);

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion
                    ViewBag.NewsSortOrder = 0;
                    if (sortOrder != null)
                    {
                        ViewBag.NewsSortOrder = sortType;
                    }
                    return View("ManageNews", InformationList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageLinks, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageLinks, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeUser(Roles = "70001")]
        public ActionResult NewsOverview(int? page, int? pageSize)
        {

            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (!PageAccess.GetPageAccess("70001"))
                {
                    return RedirectToAction("Error", "Home");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {
                    #region Check Page Request
                    TempData["heading"] = "news";
                    TempData["PageName"] = "news story";
                    TempData["type"] = "NEWS";

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

                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;

                    ViewBag.SearchValue = SessionInfo.UserTypeId;
                    #endregion

                    ViewBag.Heading = "Latest news from National Highways";
                    ViewBag.SubHeading = "Top stories";

                    List<InformationModel> Informations = informationService.GetUniqueInfoList(pageNumber, (int)pageSize, SessionInfo.UserTypeId, "news story"); // portalid

                    if (Informations.Count > 0)
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    else
                        ViewBag.TotalCount = 0;

                    var infoPagedList = new StaticPagedList<InformationModel>(Informations, pageNumber, (int)pageSize, ViewBag.TotalCount);
                    Session["NewNewsCount"] = 0;
                    Session["IsRead"] = true;
                    UpdateIsReadForNewsOpened(0);
                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    #endregion
                    return View(infoPagedList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/NewsOverview, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/NewsOverview, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeUser(Roles = "70001")]
        public ActionResult News()
        {
            UserInfo SessionInfo = null;
            TempData["heading"] = "news";
            TempData["PageName"] = "news story";
            TempData["type"] = "NEWS";
            SessionInfo = (UserInfo)Session["UserInfo"];
            List<InformationModel> Informations = informationService.GetUniqueInfoList(1, 4, SessionInfo.UserTypeId, "news story");
            return PartialView("~/Views/Home/_News.cshtml", Informations);
        }

        [AuthorizeUser(Roles = "70001")]
        public ActionResult HaulierNews()
        {
            UserInfo SessionInfo = null;
            TempData["heading"] = "news";
            TempData["PageName"] = "news story";
            TempData["type"] = "NEWS";
            SessionInfo = (UserInfo)Session["UserInfo"];
            List<InformationModel> Informations = informationService.GetUniqueInfoList(1, 2, SessionInfo.UserTypeId, "news story");
            return PartialView("~/Views/Home/_HaulierNews.cshtml", Informations);
        }

        [AuthorizeUser(Roles = "70004")]
        public ActionResult ViewLinkList(int? page, int? pageSize)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("70003"))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {
                    #region Check Page Request
                    TempData["heading"] = "External links";
                    TempData["PageName"] = "link";
                    TempData["type"] = "link";
                    TempData["button"] = "link";
                    ViewBag.SubHeading = "Useful links provided by National Highways";
                    #endregion

                    #region Paging Part
                    int pageNumber = (page ?? 1);

                    if (pageSize == null)
                    {
                        if (Session["PageSize"] == null)
                        {
                            Session["PageSize"] = 10;
                        }
                        pageSize = 10;
                    }
                    else
                    {
                        Session["PageSize"] = pageSize;
                    }

                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;

                    #endregion

                    List<InformationModel> Informations = informationService.GetDownloadList(pageNumber, (int)pageSize, Convert.ToString(TempData["PageName"]), contentType, SessionInfo.UserTypeId, SessionInfo.IsAdmin, null);
                    if (Informations.Count > 0)
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    else
                        ViewBag.TotalCount = 0;

                    var InformationList = new StaticPagedList<InformationModel>(Informations, pageNumber, (int)pageSize, ViewBag.TotalCount);

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion

                    return View(InformationList);

                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewLinkList, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewLinkList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeUser(Roles = "70002")]
        public ActionResult ViewInformationList(int? page, int? pageSize)
        {

            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("70002"))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {
                    #region Check Page Request
                    TempData["heading"] = "help & information";
                    TempData["PageName"] = "info story";
                    TempData["type"] = "INFO";
                    TempData["button"] = "information";
                    #endregion

                    #region Paging Part
                    int pageNumber = (page ?? 1);

                    if (pageSize == null)
                    {
                        if (Session["PageSize"] == null)
                        {
                            Session["PageSize"] = 10;
                        }
                        pageSize = 10;
                    }
                    else
                    {
                        Session["PageSize"] = pageSize;
                    }

                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;

                    ViewBag.SearchValue = SessionInfo.UserTypeId;
                    #endregion

                    ViewBag.Heading = "Information provided from National Highways";
                    ViewBag.SubHeading = "Detail information";

                    //List<InformationModel> Informations = InformationProvider.Instance.GetUniqueInfoList(pageNumber, (int)pageSize, SessionInfo.userTypeId, "info story"); // portalid

                    List<InformationModel> Informations = informationService.GetUniqueInfoList(pageNumber, (int)pageSize, SessionInfo.UserTypeId, "info story");

                    if (Informations.Count > 0)
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    else
                        ViewBag.TotalCount = 0;

                    var infoPagedList = new StaticPagedList<InformationModel>(Informations, pageNumber, (int)pageSize, ViewBag.TotalCount);
                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion
                    return View("ViewInformationList", infoPagedList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewInformationList, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewInformationList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeUser(Roles = "70001")]
        public ActionResult HotNews()
        {
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            InformationModel Informations = new InformationModel();
            Informations = informationService.GetUniqueInfoList(1, 50, SessionInfo.UserTypeId, "news story").FirstOrDefault(a => a.PriorityName == "hot");
            return PartialView("~/Views/Home/_HotNews.cshtml", Informations);
        }

        public ActionResult HotNewsprompticon(int ContentId = 0)
        {
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            InformationModel Informations = new InformationModel();
            Informations.ContentId = ContentId;
            if (ContentId == 0)
                Informations = informationService.GetUniqueInfoList(1, 50, SessionInfo.UserTypeId, "news story").FirstOrDefault(a => a.PriorityName == "hot");
            try
            {
                #region Session Check

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                //if (!(PageAccess.GetPageAccess("70001") || PageAccess.GetPageAccess("70002")))
                //{
                //    return RedirectToAction("Error", "Home");
                //}
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion

                if (ModelState.IsValid)
                {

                    // Get Main Data
                    InformationModel infoModel = informationService.GetInformationById((int)Informations.ContentId);

                    //Get Associated File List

                    infoModel.AssociatedFiles = informationService.GetAssociatedFilesByContentId((int)Informations.ContentId);

                    Session["IsRead"] = true;
                    UpdateIsReadForNewsOpened(ContentId);

                    ViewBag.NAME = infoModel.Name;
                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion
                    return View(infoModel);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewInformation, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewInformation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public ActionResult ViewInformation(int Id)
        {

            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                //if (!(PageAccess.GetPageAccess("70001") || PageAccess.GetPageAccess("70002")))
                //{
                //    return RedirectToAction("Error", "Home");
                //}
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion

                if (ModelState.IsValid)
                {

                    // Get Main Data
                    InformationModel infoModel = informationService.GetInformationById(Id);

                    //Get Associated File List

                    infoModel.AssociatedFiles = informationService.GetAssociatedFilesByContentId(Id);

                    Session["IsRead"] = true;
                    UpdateIsReadForNewsOpened(Id);
                    ViewBag.NAME = infoModel.Name;
                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion
                    return View(infoModel);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewInformation, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewInformation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        public ActionResult PublishInformation(InformationModel infoModel, IEnumerable<HttpPostedFileBase> associatedFileUpload)
        {
            try
            {
                #region Session Check
                MimeType mimeType = new MimeType();                

                int isValidFile = 0;
                if (associatedFileUpload != null)
                {
                    var allowedContentTypes = new List<string>() { "application/msword", "application/pdf", "video/mpeg", "video/mp4", "application/html","application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
                    var allowedExtensionTypes = new List<string>() { ".doc", ".pdf", ".docx", ".mp4", ".html", ".png", ".jpg", ".jpeg" };//png, jpg, jpeg, bmp, doc, docx, txt ,htm
                    foreach (var item in associatedFileUpload)
                    {
                        if (item != null)
                        {
                            isValidFile = mimeType.ValidateFile(item, allowedContentTypes, allowedExtensionTypes);
                            if (isValidFile > 0)
                            {
                                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, Exception: {0}", "Invalid File"));
                                return RedirectToAction("Error", "Home");
                            }
                        }
                    }
                }


                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!(PageAccess.GetPageAccess("90001") || PageAccess.GetPageAccess("90002")))
                {
                    return RedirectToAction("Error", "Home");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];

                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {
                    string CONTENT_TYPE_NAME = string.Empty;
                    switch (Convert.ToString(TempData["type"]))
                    {
                        case "DOWNLOAD":
                            CONTENT_TYPE_NAME = "download";
                            break;
                        case "LINK":
                            CONTENT_TYPE_NAME = "link";
                            break;
                        case "NEWS":
                            CONTENT_TYPE_NAME = "news story";
                            break;
                        case "INFO":
                            CONTENT_TYPE_NAME = "info story";
                            break;
                        default:
                            CONTENT_TYPE_NAME = "news story";
                            break;
                    }
                    infoModel.ContentTypeName = CONTENT_TYPE_NAME;

                    //get portal list
                    if (infoModel.PortalList != null && infoModel.PortalList.Count > 0)
                    {
                        foreach (CheckBoxList item in infoModel.PortalList)
                        {
                            if (item != null)
                            {
                                if (item.IsSelected)
                                {
                                    infoModel.PortalId += item.CheckBoxId + ",";
                                }
                            }
                        }
                    }
                    else
                        infoModel.PortalId = string.Empty;

                    if (infoModel.PortalId != null && infoModel.PortalId.Length > 0)
                        infoModel.PortalId = infoModel.PortalId.TrimEnd(',');
                    //get portal list end

                    infoModel.ContentTypeNameEnum = contentType;

                    InformationModel informationModelResult = informationService.ManageInformation(infoModel);

                    if (Convert.ToInt32(informationModelResult.ContentId) > 0)
                    {
                        if (infoModel.ContentTypeName == "news story")// && infoModel.ContentId <= 0)
                            _hubContext.Clients.All.AddMessage(informationModelResult.ContentId, JsonConvert.SerializeObject(infoModel));

                        //Save video file to physical folder.
                        if (infoModel.ContentTypeName.Trim().ToLower() == "download")// video file type
                        {

                            try
                            {
                                StringBuilder NewVideofilepath = new StringBuilder();
                                string NewFolderpath = string.Empty;
                                string NewFilepath = string.Empty;
                                if (associatedFileUpload != null && associatedFileUpload.Count() > 2 && (((System.Web.HttpPostedFileBase[])(associatedFileUpload))[2]) != null)
                                {
                                    NewVideofilepath.Append(Convert.ToString(informationModelResult.ContentId) + "\\" + Convert.ToString(informationModelResult.CurrentVerssionID));
                                    NewFolderpath = Path.Combine(ConfigurationManager.AppSettings["HtmlHelperContent"], "Videos", NewVideofilepath.ToString());

                                    if (!System.IO.Directory.Exists(NewFolderpath))
                                    {
                                        System.IO.Directory.CreateDirectory(NewFolderpath);
                                    }
                                    var videofile = ((System.Web.HttpPostedFileBase[])(associatedFileUpload))[2];
                                    if (videofile != null && videofile.FileName != null && videofile.FileName.Length > 0)
                                    {
                                        NewFilepath = NewFolderpath + "\\" + Path.GetFileName(videofile.FileName.Trim());
                                    }

                                    Request.Files[2].SaveAs(NewFilepath);
                                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, ", "File saved successfully." + informationModelResult.FileName));
                                }
                                else
                                {
                                    if (informationModelResult.CurrentVerssionID > 1 && !string.IsNullOrEmpty(informationModelResult.FileName))
                                    {
                                        StringBuilder OldVideofilepath = new StringBuilder();
                                        string OldFolderpath = string.Empty;
                                        string OldFilepath = string.Empty;

                                        NewVideofilepath.Append(Convert.ToString(informationModelResult.ContentId) + "\\" + Convert.ToString(informationModelResult.CurrentVerssionID));
                                        NewFolderpath = Path.Combine(ConfigurationManager.AppSettings["HtmlHelperContent"], "Videos", NewVideofilepath.ToString());
                                        NewFilepath = NewFolderpath + "\\" + informationModelResult.FileName;

                                        OldVideofilepath.Append(Convert.ToString(informationModelResult.ContentId) + "\\" + Convert.ToString(informationModelResult.CurrentVerssionID - 1));
                                        OldFolderpath = Path.Combine(ConfigurationManager.AppSettings["HtmlHelperContent"], "Videos", OldVideofilepath.ToString());
                                        OldFilepath = OldFolderpath + "\\" + informationModelResult.FileName;

                                        if (!System.IO.Directory.Exists(NewFolderpath))
                                        {
                                            System.IO.Directory.CreateDirectory(NewFolderpath);
                                        }
                                        if (System.IO.File.Exists(OldFilepath))
                                        {
                                            System.IO.File.Copy(OldFilepath, NewFilepath);
                                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, ", "File copied successfully." + informationModelResult.FileName));
                                        }
                                        else
                                        {
                                            Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, ", "File not exists." + informationModelResult.FileName));
                                        }
                                    }
                                }
                            }
                            catch (FileLoadException ex)
                            {
                                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, Exception: {0}", ex));
                                return RedirectToAction("Error", "Home");
                            }
                            catch (Exception ex)
                            {
                                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, Exception: {0}", ex));
                                return RedirectToAction("Error", "Home");
                            }

                        }

                        //Save video file to physical folder. End
                        double file_id = 1;
                        if (associatedFileUpload != null)
                        {
                            foreach (var file in associatedFileUpload)
                            {
                                InformationModel webContentFile = new InformationModel();
                                if (file != null && file.FileName != null && file.FileName.Length > 0)
                                {
                                    webContentFile.ContentId = informationModelResult.ContentId;
                                    webContentFile.VersionId = informationModelResult.CurrentVerssionID;
                                    webContentFile.FileName = Path.GetFileName(file.FileName);
                                    if (infoModel.ContentTypeName.Trim().ToLower() == "download")
                                    {
                                        if (file_id == 1)
                                        {
                                            if (TempData["AssoFile2"] != null)
                                            {
                                                TempData.Remove("AssoFile2");
                                            }

                                            webContentFile.FileId = 2;
                                        }
                                        else if (file_id == 2)
                                        {
                                            if (TempData["AssoFile1"] != null)
                                            {
                                                TempData.Remove("AssoFile1");
                                            }

                                            webContentFile.FileId = 1;
                                        }
                                        else if (file_id == 3)//video file
                                        {
                                            if (TempData["AssoFile3"] != null)
                                            {
                                                TempData.Remove("AssoFile3");
                                            }

                                            webContentFile.FileId = 3;
                                        }
                                    }
                                    else
                                    {
                                        if (file_id == 1)
                                        {
                                            webContentFile.FileId = 1;

                                            if (TempData["AssoFile1"] != null)
                                            {
                                                TempData.Remove("AssoFile1");
                                            }
                                        }
                                        else if (file_id == 5)
                                        {
                                            webContentFile.FileId = 2;

                                            if (TempData["AssoFile2"] != null)
                                            {
                                                TempData.Remove("AssoFile2");
                                            }
                                        }
                                        else
                                        {
                                            webContentFile.FileId = Convert.ToDouble(1 + file_id);

                                            if (webContentFile.FileId == 3 && TempData["AssoFile3"] != null)
                                            {
                                                TempData.Remove("AssoFile3");
                                            }

                                            if (webContentFile.FileId == 4 && TempData["AssoFile4"] != null)
                                            {
                                                TempData.Remove("AssoFile4");
                                            }

                                            if (webContentFile.FileId == 5 && TempData["AssoFile5"] != null)
                                            {
                                                TempData.Remove("AssoFile5");
                                            }
                                        }
                                    }
                                    webContentFile.MimeType = file.ContentType;
                                    if (infoModel.ContentTypeName.Trim().ToLower() == "download")// video file type
                                    {
                                        if (webContentFile.FileId != 3)
                                        {
                                            MemoryStream target = new MemoryStream();
                                            file.InputStream.CopyTo(target);
                                            webContentFile.FileContent = target.ToArray();
                                        }
                                    }
                                    else
                                    {
                                        MemoryStream target = new MemoryStream();
                                        file.InputStream.CopyTo(target);
                                        webContentFile.FileContent = target.ToArray();
                                    }
                                    InformationModel uploadedFiles = new InformationModel();     //unused
                                    Boolean result = informationService.ManageInformationFiles(webContentFile);
                                    if (!result)
                                    {
                                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, Exception: {0}", "Error - ManageInformationFiles"));
                                    }
                                }
                                file_id = file_id + 1;
                            }
                        }

                        InformationModel webContentInfo = new InformationModel();

                        List<string> storeRemoveAttachmentInfo = new List<string>();

                        if (infoModel.ContentTypeName == "download")
                        {
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile1"]));
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile2"]));
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile3"]));
                        }
                        else
                        {
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile1"]));
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile2"]));
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile3"]));
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile4"]));
                            storeRemoveAttachmentInfo.Add(Convert.ToString(TempData["AssoFile5"]));
                        }

                        // Add business logic to remove an attachment

                        for (int removeAttachmentNo = 0; removeAttachmentNo < storeRemoveAttachmentInfo.Count; removeAttachmentNo++)
                        {
                            string associatedFileName = "AssoFile";

                            associatedFileName = associatedFileName + (removeAttachmentNo + 1);

                            if (TempData[associatedFileName] != null)
                            {
                                webContentInfo.ContentId = informationModelResult.ContentId;
                                webContentInfo.VersionId = informationModelResult.CurrentVerssionID;

                                webContentInfo.FileName = string.Empty;

                                webContentInfo.FileContent = null;
                                webContentInfo.MimeType = null;

                                webContentInfo.FileId = removeAttachmentNo + 1;

                                informationService.ManageInformationFiles(webContentInfo);

                                TempData.Remove(associatedFileName);
                            }
                        }

                    }
                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion
                    if (infoModel.ContentTypeName == "news story")
                        return RedirectToAction("ManageNews", "Information");
                    else if (infoModel.ContentTypeName == "info story")
                        return RedirectToAction("ManageHelpAndInformations", "Information");
                    else if (infoModel.ContentTypeName == "download")
                        return RedirectToAction("ManageDocuments", "Information");
                    else if (infoModel.ContentTypeName == "link")
                        return RedirectToAction("ManageLinks", "Information");
                    else
                        return RedirectToAction("ManageNews", "Information");
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/PublishInformation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public JsonResult DeleteInformation(int deletedContactId)
        {
            try
            {
                bool isDelete = false;
                if (ModelState.IsValid)
                {
                    #region Session Check
                    UserInfo SessionInfo = null;
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    ViewBag.IsAdmin = SessionInfo.IsAdmin;
                    #endregion

                    string type = Convert.ToString(TempData["type"]);

                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Information/DeleteInformation JsonResult method started successfully, with parameters deletedContactId:{0}", deletedContactId));
                    int result = informationService.DeleteInformation(deletedContactId);
                    if (result < 0)
                    {
                        isDelete = false;
                    }
                    else
                    {
                        isDelete = true;
                    }

                    TempData["type"] = type;

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion


                    return Json(new { Success = isDelete }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DeleteInformation/DeleteInformation, Exception: {0}", "Invalid Model State"));
                    return Json(new { Success = isDelete }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/DeleteInformation, Exception: {0}", ex));
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult RemoveAttachment(string fileType, string fileName)
        {

            if (fileType == "htmlfile" || fileType == "pdffile")
            {
                TempData["AssoFile1"] = fileName;

                TempData.Keep("AssoFile1");
            }
            else if (fileType == "thumbnail" || fileType == "docfile")
            {
                TempData["AssoFile2"] = fileName;
                TempData.Keep("AssoFile2");
            }
            else if (fileType == "file3" || fileType == "video")
            {
                TempData["AssoFile3"] = fileName;
                TempData.Keep("AssoFile3");
            }
            else if (fileType == "file4")
            {
                TempData["AssoFile4"] = fileName;
                TempData.Keep("AssoFile4");
            }
            else if (fileType == "file5")
            {
                TempData["AssoFile5"] = fileName;
                TempData.Keep("AssoFile5");
            }

            return Json(new { Success = fileName }, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(Roles = "70003")]
        public ActionResult ViewDocumentList(int? page, int? pageSize, InformationModelFilter IMF = null)
        {
            try
            {

                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("70003"))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion

                if (IMF.SearchColumn != null && IMF.SearchColumn.ToLower().Trim() == "all")
                {
                    IMF.SearchColumn = null;
                    IMF.SearchValue = null;
                }
                if (pageSize == null)
                {
                    if (page == null && IMF.SearchColumn == null)
                    {
                        //first time page is loaded page parameter will be null and search column will also be null

                        TempData["Column"] = null;

                    }
                    else if (page == null && IMF.SearchColumn != null)
                    {
                        //search button is clicked, page parameter is null and search column is supplied
                        //so save these values in the temp data                

                        TempData["Column"] = IMF.SearchColumn;

                    }
                    else if (page != null && IMF.SearchColumn == null)
                    {
                        //during page number click, the page parameter will not be null but the model will be null
                        //so put the tempdata values back into the  contactSearch

                        if (TempData["Column"] == null)
                        {
                            IMF.SearchColumn = null;
                            IMF.SearchValue = null;
                        }
                        else
                        {
                            IMF.SearchColumn = Convert.ToString(TempData["Column"]);
                        }

                    }

                }
                else
                {
                    if (TempData["Column"] == null)
                    {
                        IMF.SearchColumn = null;
                        IMF.SearchValue = null;
                    }
                    else
                    {
                        IMF.SearchColumn = Convert.ToString(TempData["Column"]);

                    }

                }

                if (ModelState.IsValid)
                {

                    TempData["heading"] = "download";
                    TempData["PageName"] = "download";
                    TempData["type"] = "DOWNLOAD";
                    TempData["button"] = "document";
                    ViewBag.SubHeading = "The downloadable documents below are provided by National Highways";


                    #region Paging Part
                    int pageNumber = (page ?? 1);

                    if (pageSize == null)
                    {
                        if (Session["PageSize"] == null)
                        {
                            Session["PageSize"] = 10;
                        }
                        pageSize = 10;
                    }
                    else
                    {
                        Session["PageSize"] = pageSize;
                    }

                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;

                    #endregion

                    List<InformationModel> Informations;

                    Informations = informationService.GetDownloadList(pageNumber, (int)pageSize, Convert.ToString(TempData["PageName"]), contentType, SessionInfo.UserTypeId, SessionInfo.IsAdmin, IMF.SearchValue);
                    if (Informations.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(Informations[0].TotalRecordCount);
                    }
                    else
                    {
                        ViewBag.TotalCount = 0;
                    }

                    //Code for video file
                    StringBuilder Videofilepath;
                    StringBuilder Folderpath;
                    StringBuilder Filepath;

                    if (Informations.Count > 0)
                    {
                        List<InformationModel> videoFiles = Informations.Where(v => v.VideoFileName != "").ToList();
                        if (videoFiles.Count > 0)
                        {
                            foreach (InformationModel video in videoFiles)
                            {
                                Videofilepath = new StringBuilder();
                                Folderpath = new StringBuilder();
                                Filepath = new StringBuilder();
                                FileInfo fileinfo;

                                Videofilepath.Append(Convert.ToString(video.ContentId) + "\\" + Convert.ToString(video.CurrentVerssionID));
                                Folderpath.Append(ConfigurationManager.AppSettings["HtmlHelperContent"] + "\\Videos\\" + Convert.ToString(Videofilepath));
                                Filepath.Append(Folderpath + "\\" + video.VideoFileName);

                                if (System.IO.File.Exists(Convert.ToString(Filepath)))
                                {
                                    fileinfo = new FileInfo(Convert.ToString(Filepath).Trim());
                                    double fileLength = fileinfo.Length;
                                    if (fileLength > 1024)
                                    {
                                        fileLength = Math.Round(Convert.ToDouble(fileLength / 1024), 2);
                                    }
                                    Informations.Where(v => v.ContentId.Equals(video.ContentId) && v.VideoFileName != "").ToList().ForEach(f => f.VideoFileSize = fileLength);
                                }
                            }
                        }

                    }

                    var InformationList = new StaticPagedList<InformationModel>(Informations, pageNumber, (int)pageSize, ViewBag.TotalCount);

                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    TempData.Keep("Column");


                    string EnumTypeName = "DocumentDownloadTypeEnum";
                    List<InformationModel> DownLoadType = informationService.GetEnumValsListByEnumType(EnumTypeName);
                    List<SelectListItem> DownLoadTypeList = new List<SelectListItem>();
                    SelectListItem BlankItem = new SelectListItem();
                    BlankItem.Text = "All";
                    BlankItem.Value = null;

                    DownLoadTypeList = (from items in DownLoadType
                                        select new SelectListItem
                                        {
                                            Text = items.EnumValuesName.Substring(0, 1).ToUpper() + items.EnumValuesName.Substring(1).ToLower(),
                                            Value = Convert.ToString(items.Code)
                                        }).ToList();

                    DownLoadTypeList.Insert(0, BlankItem);
                    if (TempData["Column"] != null)
                    {
                        DownLoadTypeList.Where(w => w.Text == Convert.ToString(TempData["Column"])).ToList().ForEach(s => s.Selected = true);
                    }
                    TempData.Keep("Column");
                    ViewBag.SearchList = DownLoadTypeList;

                    return View(InformationList);

                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewDownLoadList, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ViewDownLoadList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult SendFile(int FileId, int ContentID)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!(PageAccess.GetPageAccess("90001") || PageAccess.GetPageAccess("90002") || PageAccess.GetPageAccess("70001") || PageAccess.GetPageAccess("70002")))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion
                if (ModelState.IsValid)
                {


                    List<WebContentFile> AssociatedFiles = informationService.GetAssociatedFilesByContentId(ContentID);

                    var model = AssociatedFiles.Where(r => r.FileID.Equals(FileId)).SingleOrDefault();

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    #endregion
                    //Code for video file
                    string FileName = Convert.ToString(model.FileName).Trim();
                    string extension = System.IO.Path.GetExtension(FileName).ToLower().TrimStart('.').ToLower();
                    if (extension == "mpeg" || extension == "wmv" || extension == "mp4" || extension == "avi")
                    {
                        StringBuilder Videofilepath = new StringBuilder();
                        StringBuilder Folderpath = new StringBuilder();
                        StringBuilder Filepath = new StringBuilder();

                        Videofilepath.Append(Convert.ToString(model.ContentID) + "\\" + Convert.ToString(model.VersionID));
                        Folderpath.Append(Path.Combine(ConfigurationManager.AppSettings["VideoVirtualDirectory"], Videofilepath.ToString()));
                        Filepath.Append(Folderpath + "\\" + model.FileName);


                        ViewBag.Source = ConfigurationManager.AppSettings["VideoVirtualDirectory"] + "/" + Videofilepath.ToString() + "/" + model.FileName;
                        ViewBag.Mimetype = model.MimeTypeUpload;
                        ViewBag.Filename = model.FileName;

                        return View("ShowVideo");
                    }
                    else
                    {
                        return File(model.FileContentUpload, model.MimeTypeUpload, model.FileName);
                    }
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/SendFile, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }

            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/SendFile, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult NewsDetails(string mode, int ContentId)
        {
            TempData.Keep("PageName");
            TempData.Keep("type");
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!(PageAccess.GetPageAccess("90001") || PageAccess.GetPageAccess("90002")))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];

                ViewBag.IsAdmin = SessionInfo.IsAdmin;
                #endregion

                TempData.Remove("AssoFile1");
                TempData.Remove("AssoFile2");

                TempData.Remove("AssoFile3");
                TempData.Remove("AssoFile4");
                TempData.Remove("AssoFile5");

                if (ModelState.IsValid)
                {
                    InformationModel infoModel = new InformationModel();
                    if (ModelState.IsValid)
                    {
                        //Common Code

                        double HotNewsContentId = 0;// get hot news content id
                        string HOT_NEWS_NAME = string.Empty;
                        // Get Priority List for Dropdown.
                        string EnumTypeName = "ManagedContentPriorityTypeEnum";
                        List<InformationModel> Priorities = new List<InformationModel>();
                        Priorities = informationService.GetEnumValsListByEnumType(EnumTypeName);
                        List<SelectListItem> priorityList = new List<SelectListItem>();
                        //Low priority
                        priorityList.InsertRange(0, (from items in Priorities
                                                     where items.Code.Equals(691002)
                                                     select new SelectListItem
                                                     {
                                                         Text = items.EnumValuesName.Substring(0, 1).ToUpper() + items.EnumValuesName.Substring(1).ToLower(),
                                                         Value = Convert.ToString(items.Code),
                                                         Selected = false
                                                     }).ToList());

                        priorityList.InsertRange(0, (from items in Priorities
                                                     where items.Code.Equals(691001)
                                                     select new SelectListItem
                                                     {
                                                         Text = items.EnumValuesName.Substring(0, 1).ToUpper() + items.EnumValuesName.Substring(1).ToLower(),
                                                         Value = Convert.ToString(items.Code),
                                                         Selected = true
                                                     }).ToList());

                        if (Convert.ToString(TempData["type"]).ToUpper() == "NEWS")
                        {
                            priorityList.InsertRange(0, (from items in Priorities
                                                         where items.Code.Equals(691003)
                                                         select new SelectListItem
                                                         {
                                                             Text = items.EnumValuesName.Substring(0, 1).ToUpper() + items.EnumValuesName.Substring(1).ToLower(),
                                                             Value = Convert.ToString(items.Code),
                                                             Selected = false
                                                         }).ToList());
                            List<InformatinDetail> informationDetail = informationService.GetHotNewsForAdmin("news story");
                            if (informationDetail.Count > 0)
                            {
                                HotNewsContentId = informationDetail[0].HotNewsContentId;
                                HOT_NEWS_NAME = informationDetail[0].PortalName.Trim();
                            }
                        }
                        priorityList.Where(p => p.Text.ToLower().Equals("high")).Select(s => s.Selected = true);
                        List<InformationModel> Supress = new List<InformationModel>();
                        InformationModel SupressInfo = new InformationModel();
                        SupressInfo.Suppressed = 1;
                        SupressInfo.SuppressedName = "Yes";
                        Supress.Add(SupressInfo);
                        SupressInfo = new InformationModel();
                        SupressInfo.Suppressed = 0;
                        SupressInfo.SuppressedName = "No";
                        Supress.Add(SupressInfo);

                        List<SelectListItem> SupressList = (from items in Supress
                                                            select new SelectListItem
                                                            {
                                                                Text = items.SuppressedName,
                                                                Value = Convert.ToString(items.Suppressed)
                                                            }).ToList();

                        // Get Priority List for Dropdown.
                        List<InformationModel> PortalTypes = informationService.GetEnumValsListByEnumType("PortalTypeEnum");

                        List<CheckBoxList> checkBoxList = new List<CheckBoxList>();
                        foreach (var d in PortalTypes)
                        {
                            CheckBoxList chk = new CheckBoxList();
                            chk.CheckBoxId = d.Code;
                            chk.CheckBoxName = (d.EnumValuesName.ToLower().Substring(0, 1).ToUpper() + d.EnumValuesName.ToLower().Substring(1)).Replace("Cm admin portal", "Helpdesk").Replace("Soa portal", "SOA").Replace("So portal", "SORT").Replace("Police alo portal", "Police").Replace("Hauliers portal", "Haulier");
                            chk.IsSelected = false;
                            checkBoxList.Add(chk);
                        }

                        List<SelectListItem> DownloadTypeList = new List<SelectListItem>();

                        if (Convert.ToString(TempData["type"]).ToUpper() == "DOWNLOAD")
                        {
                            List<InformationModel> DownloadTypes = informationService.GetEnumValsListByEnumType("DocumentDownloadTypeEnum");
                            if (DownloadTypes.Count() > 0)
                            {
                                DownloadTypeList = (from items in DownloadTypes
                                                    select new SelectListItem
                                                    {
                                                        Text = items.EnumValuesName.Substring(0, 1).ToUpper() + items.EnumValuesName.Substring(1).ToLower(),
                                                        Value = Convert.ToString(items.Code)
                                                    }).ToList();
                            }
                            SelectListItem downloadType = new SelectListItem();
                            downloadType.Text = "Please select";
                            downloadType.Value = "0";
                            downloadType.Selected = true;
                            DownloadTypeList.Insert(0, downloadType);
                            //DownloadTypeList
                        }
                        if (mode != null && mode == "Edit")
                        {
                            List<WebContentFile> AssociatedFiles = informationService.GetAssociatedFilesByContentId(ContentId);
                            if (AssociatedFiles.Count > 0)
                            {
                                foreach (WebContentFile assoFile in AssociatedFiles)
                                {
                                    if (assoFile != null)
                                    {
                                        if (Convert.ToString(TempData["type"]).ToLower() == "download")
                                        {
                                            if (assoFile.FileID == 2)
                                            {
                                                ViewBag.MswordFile = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                            }
                                            else if (assoFile.FileID == 1)
                                            {
                                                ViewBag.PdfFile = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                            }
                                            else if (assoFile.FileID == 3)// video file
                                            {
                                                ViewBag.VideoFile = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                            }
                                        }
                                        else
                                        {
                                            if (assoFile.FileID == 1)
                                                ViewBag.AssoFile1 = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                            else if (assoFile.FileID == 2)
                                                ViewBag.AssoFile2 = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                            else if (assoFile.FileID == 3)
                                                ViewBag.AssoFile3 = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                            else if (assoFile.FileID == 4)
                                                ViewBag.AssoFile4 = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                            else if (assoFile.FileID == 5)
                                                ViewBag.AssoFile5 = assoFile.FileName.Length > 27 ? assoFile.FileName.Substring(0, 27) + "..." : assoFile.FileName;
                                        }
                                    }
                                }

                            }

                            // Get Primary data from WEB_PORTAL_CONTENT.
                            List<InformationModel> PortalsContent = informationService.GetPortalContentById(ContentId);

                            if (PortalsContent.Count > 0)
                            {
                                foreach (var item in PortalsContent)
                                {
                                    checkBoxList.Where(w => w.CheckBoxId == item.Code).ToList().ForEach(s => s.IsSelected = true);
                                }
                            }
                            //

                            // Get Main Data
                            infoModel = informationService.GetInformationById(ContentId);
                        }
                        infoModel.PriorityList = priorityList;
                        infoModel.SuppressedList = SupressList;
                        infoModel.PortalList = checkBoxList;

                        infoModel.HotNewsContentId = HotNewsContentId;
                        infoModel.HotNewsName = HOT_NEWS_NAME;

                        if (Convert.ToString(TempData["type"]).ToUpper() == "DOWNLOAD")
                        {
                            infoModel.DownloadTypeList = DownloadTypeList;
                        }
                    }

                    #region Keep temp data
                    TempData.Keep("heading");
                    TempData.Keep("PageName");
                    TempData.Keep("type");
                    TempData.Keep("button");
                    ViewBag.Mode = mode;
                    #endregion

                    return PartialView("PartialView/_NewsDetails", infoModel);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageInformation, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }

            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/ManageInformation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }


        public ActionResult CreateNews(string mode, int ContentId)
        {
            TempData.Keep("PageName");
            TempData.Keep("type");
            return View();
        }

        public ActionResult CreateInformation(string mode, int ContentId)
        {
            TempData.Keep("PageName");
            TempData.Keep("type");
            return View();
        }

        public ActionResult CreateDownload(string mode, int ContentId)
        {
            TempData.Keep("PageName");
            TempData.Keep("type");
            return View();
        }

        public ActionResult CreateLink(string mode, int ContentId)
        {
            TempData.Keep("PageName");
            TempData.Keep("type");
            return View();
        }
        [HttpGet]
        [AuthorizeUser(Roles = "70001")]
        public JsonResult GetLatestNews(int timeInterval, string urlMethod, string urlController)
        {
            List<LatestNews> latestNews = null;
            var existingNews = new List<NewsNotificationModel>();
            try
            {
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                latestNews = informationService.GetLatestNews(SessionInfo.UserTypeId, timeInterval);
                if (latestNews.Count > 0)
                    existingNews = SetNewsNotificationSessionData(latestNews);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Information/GetLatestNews, Exception: {0}", ex));
            }
            var unReadItems = existingNews.Where(x => !x.IsRead);
            return Json(new { result = unReadItems, data = unReadItems!=null && unReadItems.Any() }, JsonRequestBehavior.AllowGet);
        }


        #region--------------------News notification common methods------------------------
        private void UpdateIsReadForNewsOpened(int ContentId)
        {
            if (Session["NewsNotificationExisting"] != null)
            {
                var existingNews = (List<NewsNotificationModel>)Session["NewsNotificationExisting"];
                if (existingNews != null && existingNews.Any())
                {
                    if (ContentId > 0)
                    {
                        foreach (var item in existingNews)
                        {
                            if(item.NewsId == ContentId)
                                item.IsRead = true;
                        }
                        Session["NewsNotificationExisting"] = existingNews;
                    }
                    else//For news listing page
                    {
                        foreach (var item in existingNews)
                        {
                            item.IsRead = true;
                        }
                        Session["NewsNotificationExisting"] = existingNews;
                    }
                }
            }
        }

        private List<NewsNotificationModel> SetNewsNotificationSessionData(List<LatestNews> latestNews)
        {
            List<NewsNotificationModel> existingNews = new List<NewsNotificationModel>();
            if (Session["NewsNotificationExisting"] != null)
                existingNews = (List<NewsNotificationModel>)Session["NewsNotificationExisting"];

            if (latestNews != null)
            {
                for (int i = 0; i < latestNews.Count; i++)
                {
                    var item = latestNews[i];
                    var sameDataExist = existingNews.FirstOrDefault(x => x.NewsId == item.ContentId && x.UploadedDateTime == item.UploadedDateTime && x.IsRead);
                    if (sameDataExist != null)
                        latestNews.Remove(item);
                }
            }

            var newNewsIds = latestNews.Select(x => x.ContentId);
            existingNews = existingNews.Where(x => !newNewsIds.Contains(x.NewsId) && x.IsRead).ToList();

            var existingNewsIds = (existingNews != null && existingNews.Any()) ?
                    existingNews.Select(x => x.NewsId).ToList() : new List<double>();
            var newlyAddedOnly = newNewsIds.Except(existingNewsIds);
            if (newlyAddedOnly != null && newlyAddedOnly.Any())
            {
                latestNews = latestNews.Where(x => newlyAddedOnly.Contains(x.ContentId)).ToList();
                existingNews.AddRange(from item in latestNews
                                      select new NewsNotificationModel { NewsId = item.ContentId, UploadedDateTime = item.UploadedDateTime });
            }
            Session["NewsNotificationExisting"] = existingNews;
            return existingNews;
        }
        #endregion
    }
}