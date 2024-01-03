using AggreedRouteXSD;
using NetSdoGeometry;
using Newtonsoft.Json;
using PagedList;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.General;
using STP.Common.Logger;
using STP.Common.SortHelper;
using STP.Common.StringExtractor;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.HelpdeskTools;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.RouteAssessment;
using STP.Domain.Routes;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.CommunicationsInterface;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.RouteAssessment;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.ServiceAccess.Workflows.SORTSOProcessing;
using STP.ServiceAccess.Workflows.SORTVR1Processing;
using STP.Web.Filters;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using static STP.Domain.Routes.RouteModel;
using UserType = STP.Common.Constants.UserType;

namespace STP.Web.Controllers
{
    public class SORTApplicationController : Controller
    {
        private readonly ISORTDocumentService sortDocumentService;
        private readonly IMovementsService movementsService;
        private readonly INotificationDocService notificationDocService;
        private readonly IDocumentService documentService;
        private readonly ISORTApplicationService sortApplicationService;
        private readonly IRoutesService routeService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly ILoggingService loggingService;
        private readonly IApplicationService applicationService;
        private readonly IVehicleConfigService vehicleconfigService;
        private readonly ISORTSOProcessingService sortSOProcessingService;
        private readonly ICommunicationsInterfaceService communicationService;
        private readonly ISORTVR1ProcessingService sortVR1ProcessingService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;

        public SORTApplicationController(ISORTDocumentService sortDocumentService, IMovementsService movements, INotificationDocService notificationDocService, IDocumentService documentService, ISORTApplicationService sortApplicationService, IRouteAssessmentService routeAssessmentService, ILoggingService loggingService, IApplicationService applicationService, IRoutesService routeService, IVehicleConfigService vehicleconfigService, ICommunicationsInterfaceService communicationService, ISORTSOProcessingService sortSOProcessingService, ISORTVR1ProcessingService sortVR1ProcessingService, IApplicationNotificationWorkflowService applicationNotificationWorkflowService)
        {
            this.sortDocumentService = sortDocumentService;
            this.movementsService = movements;
            this.notificationDocService = notificationDocService;
            this.documentService = documentService;
            this.sortApplicationService = sortApplicationService;
            this.routeAssessmentService = routeAssessmentService;
            this.loggingService = loggingService;
            this.applicationService = applicationService;
            this.routeService = routeService;
            this.vehicleconfigService = vehicleconfigService;
            this.sortSOProcessingService = sortSOProcessingService;
            this.sortVR1ProcessingService = sortVR1ProcessingService;
            this.communicationService = communicationService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
        }
        public static MovementActionIdentifiers movactiontype = null;

        #region SORT Public Methods

        #region public ActionResult MapFilter()
        public ActionResult MapFilter()
        {
            try
            {

                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MapFilterDetail()
        {
            try
            {

                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public ActionResult MapFilter()

        #region SORT Inbox

        #region  public ActionResult SORTInbox(int? page, int? pageSize)
        [AuthorizeUser(Roles = "40003,40004")]
        public ActionResult SORTInbox(int? page, int? pageSize, long? structID, bool IsPrevtMovementsVehicle = false, bool IsPrevtMovementsVehicleRoute = false, bool planMovement = false, int? sortType = null, int? sortOrder = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/SORTInbox actionResult method started successfully");

                UserInfo SessionInfo = null;
                if (page == null && Session["SortPreviousMovementListPage"] != null)
                {
                    page = (int)Session["SortPreviousMovementListPage"];
                }
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!PageAccess.GetPageAccess("13003"))
                {
                    return RedirectToAction("Error", "Home");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                int maxlist_item = SessionInfo.MaxListItem;
                int portalType = SessionInfo.UserTypeId;
                bool IsCreCandidateOrCreAppl = false;
                if (portalType != 696008)
                {
                    return RedirectToAction("Login", "Account");
                }

                //viewbag for pagination
                int pageNumber = (page ?? 1);
                if (Session["sortinboxPageSize"] == null)
                {
                    Session["sortinboxPageSize"] = maxlist_item;
                }

                if (pageSize == null)
                {
                    pageSize = (Session["sortinboxPageSize"] != null) ? (int)Session["sortinboxPageSize"] : 10;
                }
                else
                {
                    Session["sortinboxPageSize"] = pageSize;
                }

                ViewBag.pageSize = pageSize;
                Session["SortPreviousMovementListPage"] = pageNumber;
                ViewBag.page = pageNumber;
                ViewBag.IsPrevtMovementsVehicle = IsPrevtMovementsVehicle;
                ViewBag.IsPrevtMovementsVehicleRoute = IsPrevtMovementsVehicleRoute;
                ViewBag.PlanMovement = planMovement;
                if (IsPrevtMovementsVehicle || IsPrevtMovementsVehicleRoute)
                    IsCreCandidateOrCreAppl = true;
                ViewBag.IsCreCandidateOrCreAppl = IsCreCandidateOrCreAppl;
                SORTMovementFilter objSORTMovementFilter = new SORTMovementFilter();
                if (planMovement)
                {
                    if (Session["movement_SORTFilter"] != null)
                    {
                        objSORTMovementFilter = (SORTMovementFilter)Session["movement_SORTFilter"];
                    }
                }
                else if (IsPrevtMovementsVehicleRoute)
                {
                    if (Session["candidate_SORTFilter"] != null)
                    {
                        objSORTMovementFilter = (SORTMovementFilter)Session["candidate_SORTFilter"];
                    }
                }
                else
                {
                    if (Session["SORTFilter"] != null && (!IsPrevtMovementsVehicle && !IsPrevtMovementsVehicleRoute))
                    {
                        objSORTMovementFilter = (SORTMovementFilter)Session["SORTFilter"];
                    }
                    else
                    {
                        objSORTMovementFilter.ShowMyProjects = false;
                    }
                }

                SortAdvancedMovementFilter objSORTMovementFilterAdvanced = new SortAdvancedMovementFilter();
                if (planMovement)
                {
                    if (Session["movement_SORTFilterAdvanced"] != null)
                    {
                        objSORTMovementFilterAdvanced = (SortAdvancedMovementFilter)Session["movement_SORTFilterAdvanced"];
                    }
                }
                else if (IsPrevtMovementsVehicleRoute)
                {
                    if (Session["candidate_SORTFilterAdvanced"] != null)
                    {
                        objSORTMovementFilterAdvanced = (SortAdvancedMovementFilter)Session["candidate_SORTFilterAdvanced"];
                    }
                }
                else
                {
                    if (Session["SORTFilterAdvanced"] != null)
                    {
                        objSORTMovementFilterAdvanced = (SortAdvancedMovementFilter)Session["SORTFilterAdvanced"];
                    }
                }

                SortMapFilter objSORTMapfilter = new SortMapFilter();
                if (Session["SORTFilterGeom"] != null)
                {
                    sdogeometry geom = (sdogeometry)Session["SORTFilterGeom"];
                    objSORTMapfilter.Geometry = geom;
                    ViewBag.geom = geom;
                }
                if (Session["SORTFilterStructCount"] != null)
                {
                    int StructureCount = (int)Session["SORTFilterStructCount"];
                    objSORTMapfilter.StructureCount = StructureCount;
                    ViewBag.structcount = StructureCount;
                }
                if (Session["SORTFilterStructures"] != null)
                {
                    string structureslist = (string)Session["SORTFilterStructures"];
                    objSORTMapfilter.StructureList = structureslist;
                    ViewBag.structlist= structureslist; 
                }

                objSORTMovementFilter.UserID = SessionInfo.UserId;
                objSORTMovementFilterAdvanced.SORTOrder = sortOrder != null ? (int)sortOrder : 4; //application date
                
                int presetFilter = sortType != null ? (int)sortType : 1; // desc
                ViewBag.SortOrder = objSORTMovementFilterAdvanced.SORTOrder;
                ViewBag.SortType = presetFilter;
                ViewBag.SORTMovementFilter = objSORTMovementFilter;
                ViewBag.SORTAdvancedFilter = objSORTMovementFilterAdvanced;
                List<SORTMovementList> sortMovementObj = null;
                
                if (structID != null)
                {
                    sortMovementObj = movementsService.GetSORTMovementRelatedToStructList(organisationId, pageNumber, (int)pageSize, (long)structID);
                }
                else
                {
                    if (planMovement)
                    {
                        if (new SessionData().E4_AN_MovemenTypeClass != null)
                        {
                            VehicleMovementType movementTypeClass = new SessionData().E4_AN_MovemenTypeClass;
                            if (movementTypeClass.MovementType == (int)MovementType.special_order)
                                objSORTMovementFilterAdvanced.ApplicationType = 1;
                            else
                                objSORTMovementFilterAdvanced.ApplicationType = 2;
                        }
                        else
                            objSORTMovementFilterAdvanced.ApplicationType = 3;
                    }
                    sortMovementObj = movementsService.GetSORTMovementList(organisationId, pageNumber, (int)pageSize, objSORTMovementFilter, objSORTMovementFilterAdvanced, IsCreCandidateOrCreAppl, objSORTMapfilter, planMovement, objSORTMovementFilterAdvanced.SORTOrder, presetFilter);
                }

                ViewBag.StructureID = structID;
                if (!objSORTMovementFilter.ShowMyProjects)
                    ViewBag.ShowMyProjects = "0";
                else
                    ViewBag.ShowMyProjects = "1";

                int totalCount = 0;
                if (sortMovementObj.Count > 0)
                {
                    totalCount = sortMovementObj[0].TotalRecordCount;
                }
                ViewBag.MovementFilter = objSORTMovementFilter;
                ViewBag.MovementsAdvancedFilter = objSORTMovementFilterAdvanced;
                var movementObjPagedList = new StaticPagedList<SORTMovementList>(sortMovementObj, pageNumber, (int)pageSize, totalCount);
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/SORTInbox actionResult method completed successfully");
                return View(movementObjPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/SORTInbox, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion  public ActionResult SORTInbox(int? page, int? pageSize)


        #region public ActionResult SORTMovementInbox()
        public ActionResult SORTMovementInbox()
        {
            try
            {
                //Session["SORTFilter"] = null;
               //Session["SORTFilterAdvanced"] = null;
                Session["SORTFilterGeom"] = null;
                Session["SORTFilterStructures"] = null;
                Session["SORTFilterStructures"] = null;
                Session["movement_SORTFilter"] = null;
                Session["movement_SORTFilterAdvanced"] = null;
                Session["candidate_SORTFilter"] = null;
                Session["candidate_SORTFilterAdvanced"] = null;
                return RedirectToAction("SORTInbox");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public ActionResult SORTMovementInbox()

        public ActionResult MovementsRelatedToStructure(int? page, int? pageSize, long? structID)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Movements/MovementsRelatedToStructure actionResult method started successfully");

                UserInfo SessionInfo = null;
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}

                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                int maxlist_item = SessionInfo.MaxListItem;
                int portalType = SessionInfo.UserTypeId;

                if (portalType != 696008)
                {
                    return RedirectToAction("Login", "Account");
                }

                int pageNumber = (page ?? 1);
                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = maxlist_item;
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
                ViewBag.page = pageNumber;
                ViewBag.StructureID = structID;


                List<SORTMovementList> sortMovementObj = movementsService.GetSORTMovementRelatedToStructList(organisationId, pageNumber, (int)pageSize, (long)structID);

                int totalCount = 0;
                if (sortMovementObj.Count > 0)
                {
                    totalCount = sortMovementObj[0].TotalRecordCount;
                }
                var movementObjPagedList = new StaticPagedList<SORTMovementList>(sortMovementObj, pageNumber, (int)pageSize, totalCount);

                return PartialView(movementObjPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Movements/SORTInbox, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion  public ActionResult MovementsRelatedToStructure(int? page, int? pageSize, long? structID)


        #region public ActionResult SORTInboxFilter()
        public ActionResult SORTInboxFilter()
        {
            try
            {
                SORTMovementFilter sortMovFilter = new SORTMovementFilter();
                if (Session["SORTFilter"] != null)
                {
                    sortMovFilter = (SORTMovementFilter)Session["SORTFilter"];
                }
                return PartialView(sortMovFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public ActionResult SORTInboxFilter()

        #region public ActionResult SORTInboxAdvancedFilter()
        public ActionResult SORTInboxAdvancedFilter(bool IsPrevtMovementsVehicleRoute = false, bool IsPrevtMovementsVehicle = false, bool planMovement = false)
        {
            try
            {
                ViewBag.IsPrevtMovementsVehicleRoute = IsPrevtMovementsVehicleRoute;
                ViewBag.IsPrevtMovementsVehicle = IsPrevtMovementsVehicle;
                GetComparisonDropDown();
                SortAdvancedMovementFilter sortMovFilter = new SortAdvancedMovementFilter();
                if (planMovement == true)
                {
                    if (Session["movement_SORTFilterAdvanced"] != null)
                    {
                        sortMovFilter = (SortAdvancedMovementFilter)Session["movement_SORTFilterAdvanced"];
                    }
                }
                else
                {
                    if (Session["SORTFilterAdvanced"] != null)
                    {
                        sortMovFilter = (SortAdvancedMovementFilter)Session["SORTFilterAdvanced"];
                    }
                }

                GetComparisonDropDown();
                VehicleDimensionDropDown();
                return PartialView(sortMovFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public ActionResult SORTInboxAdvancedFilter()

        #region public JsonResult SetSORTFilter(SORTMovementFilter sortFilter, SortAdvancedMovementFilter sortAdvFilter)
        public ActionResult SetSORTFilter(SORTMovementFilter sortFilter, SortAdvancedMovementFilter sortAdvFilter, bool planMovement = false, bool IsPrevtMovementsVehicleRoute = false, bool IsPrevtMovementsVehicle = false,int? page=null,int? sortOrder=null,int? sortType=null, int pageSize = 10)
        {
            try
            {
                if (planMovement)
                {
                    sortFilter.ShowMyProjects = false;
                    Session["movement_SORTFilter"] = sortFilter;
                }
                else if (IsPrevtMovementsVehicleRoute)
                    Session["candidate_SORTFilter"] = sortFilter;
                else
                    Session["SORTFilter"] = sortFilter;

                if (planMovement)
                    Session["movement_SORTFilterAdvanced"] = sortAdvFilter;
                else if (IsPrevtMovementsVehicleRoute)
                    Session["candidate_SORTFilterAdvanced"] = sortAdvFilter;
                else
                    Session["SORTFilterAdvanced"] = sortAdvFilter;
                Session["SortPreviousMovementListPage"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("SORTInbox", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("page=" + page +
                        "&planMovement=" + planMovement +
                        "&IsPrevtMovementsVehicleRoute=" + IsPrevtMovementsVehicleRoute +
                        "&IsPrevtMovementsVehicle=" + IsPrevtMovementsVehicle +
                        "&sortOrder=" +sortOrder +
                        "&sortType=" + sortType + "&pageSize=" + pageSize)
            });
           
        }
        #endregion public JsonResult SetSORTFilter(SORTMovementFilter sortFilter, SortAdvancedMovementFilter sortAdvFilter)
        #region public ActionResult SavepreferenceNotes()
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavepreferenceNotes(string XmlHaulierNOtes, bool saveorCncl)
        {
            try
            {
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<br>", "<Br></Br>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<body>", "<_body>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<b([^>]*)>", "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<i([^>]*)>", "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<u([^>]*)>", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<ul([^>]*)>", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<li([^>]*)>", "<BulletedText xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<p([^>]*)>", "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<span([^>]*)>", "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<div([^>]*)>", "<Div xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<b>", "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<b >", "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<i>", "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<i >", "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<u>", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<u >", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<ul>", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<li>", "<BulletedText xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<p>", "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<span>", "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<div>", "<Div xmlns='http://www.esdal.com/schemas/core/formattedtext'>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</b>", "</Bold>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</i>", "</Italic>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</u>", "</Underscore>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</ul>", "</Underscore>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<br>", " <Br></Br>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</div>", " </Div>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</p>", " </Para>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</span>", " </Para>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("</li>", "</BulletedText>");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("&nbsp;", " ");
                XmlHaulierNOtes = XmlHaulierNOtes.Replace("<_body>", "<body>");

                XmlHaulierNOtes = "<?xml version='1.0' encoding='UTF-8'?><movementversion:NotesForHaulier xmlns:movementversion='http://www.esdal.com/schemas/common/movementversion'>" + XmlHaulierNOtes + "</movementversion:NotesForHaulier>";

                XmlHaulierNOtes = string.Join(" ", Regex.Split(XmlHaulierNOtes, @"(?:\r\n|\n|\r)"));//to remove \n \r from string


                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    //var bytes = System.Text.Encoding.UTF8.GetBytes(XmlHaulierNOtes);
                    ////getting the compressed blob value of xml for saving
                    bool result = true;
                    if (XmlHaulierNOtes != "" || XmlHaulierNOtes != null || XmlHaulierNOtes != string.Empty)
                    {
                        byte[] inbByteVar = StringExtractor.ZipAndBlob(XmlHaulierNOtes);
                        if (inbByteVar != null)
                        {
                            //Check if it is correct format.
                            string hnresult = string.Empty;
                            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                            string errormsg = "";
                            hnresult = STP.Common.General.XsltTransformer.Trafo(inbByteVar, path, out errormsg);
                            if (saveorCncl == true)
                            {

                            }
                            else
                            {
                                Session["PreferenceNotes"] = hnresult;

                            }

                            if (errormsg == "No Data Found")
                            {
                                result = true;
                                Session["PreferenceNotes"] = hnresult;
                                Session["PreferenceNotesBLOB"] = inbByteVar;
                                Session["UpdatedNow"] = "1";

                            }
                        }
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/SaveHaulierNotes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion public ActionResult SavepreferenceNotes()

        public void SetSearchGeometry(sdogeometry areaGeom)
        {
            try
            {
                Session["SORTFilterGeom"] = areaGeom;
                SortMapFilter objSORTMapfilter = new SortMapFilter();
                if (Session["SORTFilterGeom"] != null)
                {
                    sdogeometry geom = (sdogeometry)Session["SORTFilterGeom"];
                    objSORTMapfilter.Geometry = geom;
                    ViewBag.geom = geom;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region public JsonResult SetSORTFilterAdvanced(SortAdvancedMovementFilter sortFilter)
        public JsonResult SetSORTFilterAdvanced(SortAdvancedMovementFilter sortFilter)
        {
            try
            {
                Session["SORTFilterAdvanced"] = sortFilter;
                return Json(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public JsonResult SetSORTFilterAdvanced(SortAdvancedMovementFilter sortFilter)


        #region public JsonResult ClearInboxAdvancedFilterSORT()
        public JsonResult ClearInboxAdvancedFilterSORT()
        {
            try
            {
                Session["SORTFilter"] = null;
                Session["SORTFilterAdvanced"] = null;
                Session["SORTFilterGeom"] = null;
                Session["SORTFilterStructures"] = null;
                Session["SORTFilterStructures"] = null;
                Session["movement_SORTFilter"] = null;
                Session["movement_SORTFilterAdvanced"] = null;
                Session["candidate_SORTFilter"] = null;
                Session["candidate_SORTFilterAdvanced"] = null;
                return Json(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public JsonResult ClearInboxAdvancedFilterSORT()

        #region public JsonResult ClearLeftInboxFilterSORT()
        public JsonResult ClearLeftInboxFilterSORT()
        {
            try
            {
                Session["SORTFilter"] = null;
                return Json(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public JsonResult ClearLeftInboxFilterSORT()

        #region private void GetComparisonDropDown()
        private void GetComparisonDropDown()
        {
            List<DropDown> objListDropDown = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown();
            objDropDown.Id = 1;
            objDropDown.Value = "less than or equal to";
            objListDropDown.Add(objDropDown);
            objDropDown = new DropDown();
            objDropDown.Id = 0;
            objDropDown.Value = "greater than or equal to";
            objListDropDown.Add(objDropDown);

            List<FilterDropDown> objListFilterDropDown = new List<FilterDropDown>();
            FilterDropDown objFilterDropDown = null;
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = "<=";
            objFilterDropDown.Value = "less than or equal to";
            objListFilterDropDown.Add(objFilterDropDown);
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = ">=";
            objFilterDropDown.Value = "greater than or equal to";
            objListFilterDropDown.Add(objFilterDropDown);
            objFilterDropDown = new FilterDropDown();
            objFilterDropDown.Id = "between";
            objFilterDropDown.Value = "Between";
            objListFilterDropDown.Add(objFilterDropDown);

            int weightCount = 1;
            int widthCount = 1;
            int lengthCount = 1;
            int heightCount = 1;
            int axleCount = 1;
            int operatorCount = 1;

            MovementsAdvancedFilter objMovementsAdvancedFilter = new MovementsAdvancedFilter();
            if (Session["g_AdvancedSearchData"] != null)
            {
                objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["g_AdvancedSearchData"];
                weightCount = objMovementsAdvancedFilter.WeightCount;
                widthCount = objMovementsAdvancedFilter.WidthCount;
                lengthCount = objMovementsAdvancedFilter.LengthCount;
                heightCount = objMovementsAdvancedFilter.HeightCount;
                axleCount = objMovementsAdvancedFilter.AxleCount;
            }

            ViewBag.WeightCount = new SelectList(objListDropDown, "Id", "Value", weightCount);
            ViewBag.WidthCount = new SelectList(objListDropDown, "Id", "Value", widthCount);
            ViewBag.LengthCount = new SelectList(objListDropDown, "Id", "Value", lengthCount);
            ViewBag.HeightCount = new SelectList(objListDropDown, "Id", "Value", heightCount);
            ViewBag.AxleCount = new SelectList(objListDropDown, "Id", "Value", axleCount);
            ViewBag.OperatorCount = new SelectList(objListFilterDropDown, "Id", "Value", operatorCount);
        }
        #endregion

        #region SORTListMovemnets
        //SO=CreateSO, VR1=CreateVR1, ProjectOverView=ViewProj
        // public ActionResult SORTListMovemnets(string SORTStatus = "",string msg="", long? OrgID=null, string tabStatus="")
        // public ActionResult SORTListMovemnets(string SORTStatus = "",string msg="", long? OrgID=null)
        public ActionResult SORTListMovemnets(string hauliermnemonic, string SORTStatus = "", string msg = "", long? OrgID = null, int movementId = 0, int cloneapprevid = 0, int esdalref = 0, int revisionno = 0, int versionno = 0, long revisionId = 0, long versionId = 0, long apprevid = 0, int OrganisationId = 0, int projecid = 0, bool VR1Applciation = false, bool reduceddetailed = false, long? pageflag = 0, bool EditRev = false, int EditFlag = 0, string esdal_history = "", int LatestVer = 0, int arev_no = 0, int arev_Id = 0, int ver_no = 0, string candName = "", int candVersionno = 0, string Owner = "", string WorkStatus = "", string Checker = "", long analysisId = 0, bool IsLastVersion = false, int EnterBySORT = 0, int ViewFlag = 0, int IsHistoric = 0)
    {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"SORTApplication/SORTListMovemnets/Method started successfully");

            ViewBag.IsLastVersion = IsLastVersion;
            ViewBag.CandName = candName;
            ViewBag.CandVersionNo = candVersionno;
            ViewBag.CandAnalysisId = analysisId;
            ViewBag.ViewFlag = ViewFlag;
            Session["movementClassificationId"] = null;
            Session["movementClassificationName"] = null;

            if (projecid == 0)
                ViewBag.isReviseApp = false;
            else
                ViewBag.isReviseApp = true;
            if (Session["UserInfo"] == null)
                return RedirectToAction("Login", "Account");
            if (!PageAccess.GetPageAccess("13004") && !PageAccess.GetPageAccess("13005"))
                return RedirectToAction("Error", "Home");
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            ViewBag.SortUserId = SessionInfo.SortUserId;
            switch (SORTStatus)
            {
                case "CreateSO":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "CreateVR1":
                    ViewBag.SORTStatus = SORTStatus;
                    VR1Applciation = true;//-----added for resolving issue redmine-3944
                    break;
                case "ViewProj":
                    ViewBag.SORTStatus = SORTStatus;
                    Session["SortListMUrl"] = ("/SORTApplication/SORTListMovemnets?SORTStatus=ViewProj&revisionId=" + revisionId + "&versionId=" + versionId + "&hauliermnemonic=" + hauliermnemonic + "&esdalref=" + esdalref + "&revisionno=" + revisionno + "&versionno=" + versionno + "&OrganisationId=" + OrganisationId + "&projecid=" + projecid + "&movementId=" + movementId + "&apprevid=" + apprevid + "&pageflag=" + pageflag + "&Owner=" + Owner).Replace('&', '%');
                    break;
                case "MoveVer":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "RouteVer":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "SpecOrder":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "Revisions":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "CandidateRT":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                default:
                    break;
            }
            //for Status
            if (SORTStatus != "CreateSO" && SORTStatus != "CreateVR1")
            {
                if (!VR1Applciation)
                    ViewBag.SORTViewStatus = "SO";
                else
                    ViewBag.SORTViewStatus = "VR-1";
            }

            ViewBag.msg = msg;
            if (OrgID == null && OrganisationId != 0)
                Session["SORTOrgID"] = OrganisationId;
            else if (OrgID != null && OrganisationId == 0)
                Session["SORTOrgID"] = (int)OrgID;
            else if (OrgID == null && OrganisationId == 0)
                Session["SORTOrgID"] = 0;

            if (SORTStatus == "")
                Session["RouteFlag"] = "2";
            else
                Session["RouteFlag"] = pageflag;
            if (revisionId == 0)
                ViewBag.OrgID = OrgID;
            else
            {
                ViewBag.OrgID = OrganisationId;
                ViewBag.cloneapprevid = cloneapprevid;
                #region saving log action for revising application by SORT User
                if (cloneapprevid != 0)
                {
                    //common function for saving action logs
                    GetDiscriptionToSave(new MovementActionIdentifiers()
                    {
                        MovementActionType = MovementnActionType.sort_desktop_revises_appl
                    }, hauliermnemonic, esdalref, arev_no,projecid,revisionno,versionno);
                }
                #endregion
            }
            if (pageflag == 2)
            {
                SOApplication SORTAppSumObj = applicationService.GetSOGeneralWorkinProcessbyrevisionid(SessionInfo.UserSchema, apprevid, versionId, OrganisationId);
                ViewBag.Application_Status = SORTAppSumObj.ApplicationStatus;
                ViewBag.analysis_id = SORTAppSumObj.AnalysisId;
            }
            if (SORTStatus == "Revisions")
            {
                ViewBag.arev_no = arev_no;
                ViewBag.arev_Id = arev_Id;
                ViewBag.ver_no = ver_no;
            }
            var soGeneralDetails = applicationService.GetSOApplicationTabDetails(apprevid, versionId, SessionInfo.UserSchema, IsHistoric);
            var project_status = applicationService.GetSOGeneralDetails(apprevid, versionId, SessionInfo.UserSchema, IsHistoric);

            ViewBag.Checking_Status = project_status.CheckingStatusCode;
            ViewBag.CheckerId = project_status.CheckerUserId;
            ViewBag.PlannerUserId = project_status.PlannerUserId;
            ViewBag.AppStatusCode = project_status.ApplicationStatusCode;
            ViewBag.IsDistributed = project_status.IsMovDistributed;
            ViewBag.LastSpecialOrderNo = project_status.LastSpecialOrderNo;
            ViewBag.LatestCandidateRouteId = project_status.LastCandidateRouteId;
            ViewBag.LatestCandidateRevisionId = project_status.LastRevisionId;
            ViewBag.LatestCandRevisionNo = project_status.LastRevisionNo;
            ViewBag.Vr1Number = project_status.VR1Number;
            ViewBag.RouteAnalysisId = project_status.RouteAnalysisId;
            ViewBag.SONumber = project_status.SONumber;
            ViewBag.VehicleClass = project_status.VehicleClassification;
            if (project_status.AnalysisId != 0)
            {
                ViewBag.analysis_id = project_status.AnalysisId;
            }
            ViewBag.HAJobFileRef = project_status.HAJobFileReference;
            ViewBag.EnteredBySort = project_status.EnteredBySORT;
            ViewBag.PreVerDistributed = project_status.PreviousVersionDistributed;
            ViewBag.DistributedMovAnalysisId = project_status.DistributedMovAnalysisId;

            if (!EditRev && SORTStatus != "CreateSO" && SORTStatus != "CreateVR1")
            {
                ViewBag.VersionStatus = soGeneralDetails.VersionStatus;
                ViewBag.Proj_Status = project_status.ProjectStatus;
            }
            if (SORTStatus == "MoveVer")
            {
                ViewBag.esdal_history = esdal_history;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");

                string result = XsltTransformer.Trafo(project_status.ApplicationNotesToHA, path, out string errormsg);
                result = Regex.Replace(result, @"<(\/)?(html|body)([^>]*)>", "");
                ViewBag.App_Notes_To_HA = result;
                var Notification_Code = sortApplicationService.GetSORTNotifiCode((int)revisionId);

                ViewBag.Notification_Code = Notification_Code;
                ViewBag.MovLatestVer = LatestVer;
            }
            ViewBag.hauliermnemonic = hauliermnemonic;
            ViewBag.esdalref = esdalref;
            ViewBag.revisionno = revisionno;
            ViewBag.versionno = project_status.VersionNo;
            ViewBag.apprevid = apprevid;
            ViewBag.revisionId = project_status.LastRevisionId;
            ViewBag.movementId = movementId;
            ViewBag.OrganisationId = OrganisationId;
            ViewBag.versionId = project_status.VersionId;
            ViewBag.projecid = projecid;
            ViewBag.Owner = Owner;
            ViewBag.Checker = Checker;
            ViewBag.WorkStatus = WorkStatus;
            ViewBag.EditFlag = EditFlag;
            ViewBag.EnterBySORT = EnterBySORT;

            if (soGeneralDetails.ApplicationRevisionId == 0)
                soGeneralDetails.ApplicationRevisionId = apprevid;

            ViewBag.ApprevisionId = project_status.ApplicationRevId;
            ViewBag.EditRev = EditRev;
            ViewBag.VR1Applciation = VR1Applciation;
            if (pageflag == 3)
                ViewBag.VR1Applciation = true;
            if (apprevid == 0)
                ViewBag.reduceddetailed = reduceddetailed;
            else if (VR1Applciation)
            {
                ApplyForVR1 applyForVR1 = applicationService.GetVR1General(SessionInfo.UserSchema, apprevid, IsHistoric);
                if (applyForVR1.AnalysisId != 0)
                    ViewBag.analysis_id = applyForVR1.AnalysisId;
                ViewBag.reduceddetailed = applyForVR1.ReducedDetails == 0;
                ViewBag.VR1ContentRef = applyForVR1.VR1ContentRefNo;
            }
            else
            {
                ViewBag.reduceddetailed = true;
            }
            Session["pageflag"] = pageflag;

            //code for checking special order no for agreed recleared movements //added by poonam 23-3-15
            string sonum = applicationService.GetSONumberStatus((int)project_status.ProjectId, SessionInfo.UserSchema);
            ViewBag.sonum = sonum;

            return View("SORTListMovemnets", soGeneralDetails);
        }

        private long GetDiscriptionToSave(MovementActionIdentifiers movactiontype, string hauliermnemonic = "", int esdalref = 0, int arev_no = 0,int projectid=0,int revisioNo=0,int versionNo=0)
        {
            long descp_result = 0;
            try
            {
                
                UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                movactiontype.ESDALRef = hauliermnemonic + "/" + esdalref + "-" + arev_no;
                string MovementDescription = "";
                string ErrMsg = string.Empty;
                #region movement actions for this action method

                MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                descp_result = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription,projectid,revisioNo,versionNo, UserSessionValue.UserSchema);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/GetDiscriptionToSave, Exception: {0}", ex));
            }
            return descp_result;
        }
        #endregion

        #region SORTApplicationSummary
        public ActionResult SORTApplicationSummary(string Status, int RevisionId = 0, long versionId = 0, long Project_id = 0, bool Mov_btn_show = false, bool VR1App = false, int Organisation_ID = 0)
        {
            try
            {
                string messg = "SORTApplications/SORTApplicationSummary?Status=" + Status + "RevisionId=" + RevisionId + "versionId=" + versionId + "Project_id=" + Project_id + "Mov_btn_show=" + Mov_btn_show + "VR1App=" + VR1App + "Organisation_ID=" + Organisation_ID;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                string result = "";
                if (Session["UserInfo"] != null)
                    SessionInfo = (UserInfo)Session["UserInfo"];
                else
                    return RedirectToAction("Login", "Account");

                ViewBag.Status = Status;
                SOApplication SORTAppSumObj = applicationService.GetSOGeneralWorkinProcessbyrevisionid(SessionInfo.UserSchema, RevisionId, versionId, Organisation_ID);
                if (VR1App == true)
                {
                    result = sortApplicationService.GetVR1ApprovalDate(Project_id);
                    var VR1Detail = result.Split(',');
                    if (result != "False")
                    {
                        SORTAppSumObj.VR1ApprovalDate = VR1Detail[0];
                        SORTAppSumObj.VR1Number = VR1Detail[1];
                    }
                }
                ViewBag.Project_id = Project_id;
                ViewBag.RevisionId = RevisionId;
                ViewBag.Mov_btn_show = Mov_btn_show;
                ViewBag.VR1App = VR1App;
                if (Status == "MoveVer")
                {
                    try
                    {
                        if (SORTAppSumObj.ESDALReference != null)
                        {
                            string[] esdal2ref = null;
                            esdal2ref = SORTAppSumObj.ESDALReference.Split('-');
                            if (esdal2ref != null)
                            {
                                SORTAppSumObj.ESDALReference = esdal2ref[0].ToString();
                                SORTAppSumObj.ESDALReference += "/S" + SORTAppSumObj.VersionNo;
                            }
                        }
                    }
                    catch { }
                }
                return PartialView("SORTApplicationSummary", SORTAppSumObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SORTApplicationSummary, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SORTProjectOverview
        public ActionResult SORTProjectOverview(String hauliermnemonic = "", int esdalref = 0, int revisionno = 0, int versionno = 0, long Rev_ID = 0, string Checker = "", string OwnerName = "", long ProjectId = 0, bool VR1App = false, int historic = 0)
        {
            try
            {
                Session["RouteAssessmentFlag"] = "";
                UserInfo SessionInfo = null;
                string result = "";
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                SessionInfo = (UserInfo)Session["UserInfo"];


                string messg = "SORTApplications/SORTProjectOverview?hauliermnemonic=" + hauliermnemonic + ", esdalref=" + esdalref + ", revisionno=" + revisionno + ", versionno=" + versionno + ", Rev_ID=" + Rev_ID + ", Checker=" + Checker + ", OwnerName=" + OwnerName + ", ProjectId=" + ProjectId + ", VR1App=" + VR1App;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                SOApplication SORTProjectOverviewobj = null;

                SORTProjectOverviewobj = sortApplicationService.GetProjOverviewDetails(Rev_ID);

                if (SORTProjectOverviewobj.CheckingStatus == "")
                    SORTProjectOverviewobj.CheckingStatus = "Not Checking";

                if (SORTProjectOverviewobj.CheckerName != "" && SORTProjectOverviewobj.CheckerName != null)
                {
                    string checkername = SORTProjectOverviewobj.CheckerName;
                    checkername = checkername.First().ToString().ToUpper() + checkername.Substring(1);
                    SORTProjectOverviewobj.CheckerName = checkername;
                }

                if (SORTProjectOverviewobj.ApplicationStatus == 308002 && SessionInfo.SortUserId == SORTProjectOverviewobj.PlannerUserId && SessionInfo.SortUserId != 0)
                    ViewBag.CandidatePermission = true;
                else
                    ViewBag.CandidatePermission = false;

                long ProjectID = SORTProjectOverviewobj.ProjectId;
                int status = applicationService.GetApplicationStatus(versionno, revisionno, ProjectId, SessionInfo.UserSchema, historic);
                // ViewBag.ProjectID = ProjectID; 
                //int status = 1;
                ViewBag.ApplStatus = status;
                if (hauliermnemonic != "" && esdalref != 0)
                {
                    ViewBag.ESDAL_Reference = SORTProjectOverviewobj.ESDALReference;
                    ViewBag.Version_Status = SORTProjectOverviewobj.VersionStatus;
                }
                ViewBag.hauliermnemonic = hauliermnemonic;
                ViewBag.esdalref = esdalref;
                if (!string.IsNullOrEmpty(OwnerName))
                    OwnerName = OwnerName = Char.ToUpper(OwnerName[0]) + OwnerName.Substring(1);

                ViewBag.OwnerName = OwnerName;
                ViewBag.Checker = Checker;
                ViewBag.PStatus = SORTProjectOverviewobj.ProjectStatus;

                //VR1 Details
                if (VR1App == true)
                {
                    result = sortApplicationService.GetVR1ApprovalDate(ProjectId);
                    var VR1Detail = result.Split(',');
                    if (result != "False")
                    {
                        SORTProjectOverviewobj.VR1ApprovalDate = VR1Detail[0];
                        SORTProjectOverviewobj.VR1Number = VR1Detail[1];
                    }
                }
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.CheckingStatus))
                    SORTProjectOverviewobj.CheckingStatus = Char.ToUpper(SORTProjectOverviewobj.CheckingStatus[0]) + SORTProjectOverviewobj.CheckingStatus.Substring(1);
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.ProjectStatus))
                    SORTProjectOverviewobj.ProjectStatus = Char.ToUpper(SORTProjectOverviewobj.ProjectStatus[0]) + SORTProjectOverviewobj.ProjectStatus.Substring(1);
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.CheckerName))
                    SORTProjectOverviewobj.CheckerName = Char.ToUpper(SORTProjectOverviewobj.CheckerName[0]) + SORTProjectOverviewobj.CheckerName.Substring(1);
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.HaulierName))
                    SORTProjectOverviewobj.HaulierName = Char.ToUpper(SORTProjectOverviewobj.HaulierName[0]) + SORTProjectOverviewobj.HaulierName.Substring(1);

                SORTLatestAppDetails sortprojdetails = sortApplicationService.GetSortProjectDetails(ProjectId);
                ViewBag.AppRevisionId = sortprojdetails.ApplicationRevisionId;
                ViewBag.AppRevisionNo = sortprojdetails.ApplicationRevisionNo;
                ViewBag.VersionId = sortprojdetails.VersionId;
                ViewBag.VersionNo = sortprojdetails.VersionNo;
                ViewBag.MovIdDistributed = sortprojdetails.MovIsDistributed;

                #region SYS_EVENT for SORT project overview
                if (SORTProjectOverviewobj.ProjectId > 0)
                {
                    try
                    {
                        MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                        movactiontype.UserName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                        movactiontype.ESDALRef = Convert.ToString(esdalref);
                        var esdal = Convert.ToString(esdalref).Split('/');
                        movactiontype.HaulierMnemonic = esdal[0];
                        movactiontype.ProjectId = ProjectId;
                        if (VR1App == true)
                        {
                            movactiontype.VR1App = "true";
                        }
                        int user_ID = Convert.ToInt32(SessionInfo.UserId);
                        string ErrMsg = string.Empty;

                        #region sys_events show_sort_project_overview_details
                        movactiontype.SystemEventType = SysEventType.show_sort_project_overview_details;
                        string sysEventDescp = System_Events.GetSysEventString(SessionInfo, movactiontype, out ErrMsg);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, SessionInfo.UserSchema);
                        #endregion
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                #endregion

                return PartialView("SORTProjectOverview", SORTProjectOverviewobj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SORTProjectOverview, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SORTLeftPanel
        public ActionResult SORTLeftPanel(string Display, long pageflag = 1, long Project_ID = 0, int Rev_ID = 0, bool Mov_btn_show = false, string OwnerName = "", int PlannerId = 0, string PrjStatus = "", int CheckingStatus = 0, int AppStatus = 0, long CheckerId = 0, int CandidateId = 0, int MoveVersiono = 0, string VR1Number = "", int Latest_Rev_No = 0, bool VR1APP = false, decimal MovIsDistrbted = 0, string SONumber = "", decimal Enter_BY_SORT = 0)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];


                string messg = "SORTApplications/SORTLeftPanel?Display=" + Display + ", pageflag=" + pageflag + ", Project_ID=" + Project_ID + ", Rev_ID=" + Rev_ID + ", Mov_btn_show=" + Mov_btn_show + ", OwnerName=" + OwnerName + ", PlannerId=" + PlannerId + ", PrjStatus=" + PrjStatus + ", CheckingStatus=" + CheckingStatus + ",AppStatus=" + AppStatus + ",CheckerId=" + CheckerId + ",CandidateId=" + CandidateId + ",MoveVersiono=" + MoveVersiono + ",VR1Number=" + VR1Number + ",Latest_Rev_No=" + Latest_Rev_No + ",VR1APP=" + VR1APP + ",MovIsDistrbted=" + MovIsDistrbted + ",SONumber=" + SONumber + ",Enter_BY_SORT=" + Enter_BY_SORT;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                Session["RouteFlag"] = pageflag;//commentted by ajit
                ViewBag.Display = Display;
                ViewBag.Project_id = Project_ID;
                ViewBag.RevisionId = Rev_ID;
                ViewBag.Mov_btn_show = Mov_btn_show;
                ViewBag.OwnerName = OwnerName;
                ViewBag.PlannerId = PlannerId;
                ViewBag.PrjStatus = PrjStatus;
                ViewBag.CheckingStatus = CheckingStatus;
                ViewBag.AppStatus = AppStatus;
                ViewBag.CheckerId = CheckerId;
                ViewBag.CandidateId = CandidateId;
                ViewBag.MoveVersioNo = MoveVersiono;
                ViewBag.SortVR1Number = VR1Number;
                ViewBag.Latest_Rev_No = Latest_Rev_No;
                ViewBag.VR1APP = VR1APP;
                ViewBag.MovIsDisbted = MovIsDistrbted;
                ViewBag.SONumber = SONumber;
                ViewBag.Enter_BY_SORT = (int)Enter_BY_SORT;

                if (SessionInfo.SORTCanApproveSignVR1 == "1" && AppStatus == 307002 && CheckingStatus == 301003)
                    ViewBag.VR1Approve = true;
                if (SessionInfo.SortUserId == PlannerId && AppStatus == 307016)
                {
                    if (VR1Number != "")
                    {
                        ViewBag.VR1Number = true;
                        ViewBag.VRDocument = false;
                    }
                    else
                    {
                        ViewBag.VRDocument = true;
                        ViewBag.VR1Number = false;
                    }
                }

                return PartialView();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SORTLeftPanel, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ApplSummaryLeftPanel
        public ActionResult ApplSummaryLeftPanel(long Project_ID = 0, int Revision_ID = 0, int PlannerId = 0, int EnterdbySort = 0,bool IsVr1App = false)
        {
            try
            {
                string messg = "SORTApplications/ApplSummaryLeftPanel?Project_ID=" + Project_ID + "Revision_ID=" + Revision_ID + "PlannerId=" + PlannerId + "EnterdbySort=" + EnterdbySort;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                long rev_id = sortApplicationService.GetRevIDFromApplication(Project_ID);
                ViewBag.PlannerId = PlannerId;
                if (Revision_ID == rev_id && !IsVr1App)
                {
                    ViewBag.Show = true;
                }
                else
                {
                    ViewBag.Show = false;
                }
                ViewBag.EnterdbySort = EnterdbySort;
                return PartialView("ApplSummaryLeftPanel");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ApplSummaryLeftPanel, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ProjectOverviewLeftPanel
        public ActionResult ProjectOverviewLeftPanel(string OwnerName = "", int PlannerId = 0, int CheckingStatus = 0, int AppStatus = 0, long Checkerid = 0, int CandidateId = 0, int MovVersionNo = 0, bool IsVR1 = false, string VR1Number = "", decimal MovDistbuted = 0, string SONumber = "", int EnterdbySort = 0, int Projectid = 0, int Revision_ID = 0)
        {
            try
            {
                string messg = "SORTApplications/ProjectOverviewLeftPanel?OwnerName=" + OwnerName + "PlannerId=" + PlannerId + "CheckingStatus=" + CheckingStatus + "AppStatus=" + AppStatus + "Checkerid=" + Checkerid + "CandidateId=" + CandidateId + "MovVersionNo=" + MovVersionNo + "IsVR1=" + IsVR1 + "VR1Number=" + VR1Number + "MovDistbuted=" + MovDistbuted + "SONumber=" + SONumber + "EnterdbySort=" + EnterdbySort;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];

                ViewBag.OwnerName = OwnerName;

                long rev_id = sortApplicationService.GetRevIDFromApplication(Projectid);
                ViewBag.PlannerId = PlannerId;
                if (Revision_ID == rev_id)
                {
                    ViewBag.Show = true;
                }
                else
                {
                    ViewBag.Show = false;
                }
                ViewBag.EnterdbySort = EnterdbySort;


                SortActions sactions = new Common.SortHelper.SortActions();
                sactions.VR1Application = IsVR1;
                //Sort SO Actions.
                if (IsVR1 == false)
                    sactions = CheckingProcess.SortSOActions(sactions, SessionInfo.SortUserId, PlannerId, Checkerid, SessionInfo.SORTAllocateJob, AppStatus, CheckingStatus, MovDistbuted, CandidateId, MovVersionNo, EnterdbySort);
                else if (IsVR1 == true)
                    sactions = CheckingProcess.SortVR1Actions(sactions, AppStatus, CheckingStatus, SessionInfo.SortUserId, PlannerId, Checkerid, SessionInfo.SORTAllocateJob, SessionInfo.SORTCanApproveSignVR1, VR1Number, EnterdbySort);

                return PartialView("ProjectOverviewLeftPanel", sactions);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ProjectOverviewLeftPanel, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region SORTApplRevisions
        public ActionResult SORTApplRevisions(long ProjectID = 0, string hauliermnemonic = "", int esdalref = 0, string Checker = "", string OwnerName = "", long ProjectStatus = 0, bool IsChooseCurrMovmOption = false, bool IsVehicleCurrentMovenet = false)
        {
            try
            {
                string messg = "SORTApplications/SORTApplRevisions?ProjectID=" + ProjectID + ", hauliermnemonic=" + hauliermnemonic + ", esdalref=" + esdalref + ", Checker=" + Checker + ", OwnerName=" + OwnerName + ", ProjectStatus=" + ProjectStatus;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                ViewBag.hauliermnemonic = hauliermnemonic;
                ViewBag.esdalref = esdalref;
                ViewBag.ProjectID = ProjectID;
                ViewBag.OwnerName = OwnerName;
                ViewBag.Checker = Checker;
                ViewBag.IsChooseCurrMovmOption = IsChooseCurrMovmOption;
                ViewBag.IsVehicleCurrentMovenet = IsVehicleCurrentMovenet;
                if (ProjectStatus == 307011)
                    ViewBag.RevisedAppVersion = true;

                List<SORTMovementList> SORTApplRevisionsobj = sortApplicationService.GetHaulierAppRevision(ProjectID);
                if (SORTApplRevisionsobj.Count != 0)
                {
                    ViewBag.LatRev = SORTApplRevisionsobj.Max(c => c.RevisionNo);
                }
                return PartialView("SORTApplRevisions", SORTApplRevisionsobj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SORTApplRevisions, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SORTAppMovementVersion
        public ActionResult SORTAppMovementVersion(long ProjectID = 0, string hauliermnemonic = "", int esdalref = 0, bool IsVR1App = false, string Checker = "", string OwnerName = "", bool IsChooseCurrMovmOption = false, bool IsVehicleCurrentMovenet = false)
        {
            try
            {
                string messg = "SORTApplications/SORTAppMovementVersion?ProjectID=" + ProjectID + ", hauliermnemonic=" + hauliermnemonic + ", esdalref=" + esdalref + ", IsVR1App=" + IsVR1App + ", Checker=" + Checker + ", OwnerName=" + OwnerName;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                ViewBag.hauliermnemonic = hauliermnemonic;
                ViewBag.esdalref = esdalref;
                ViewBag.ProjectID = ProjectID;
                ViewBag.OwnerName = OwnerName;
                ViewBag.Checker = Checker;
                ViewBag.IsVR1App = IsVR1App;
                ViewBag.IsChooseCurrMovmOption = IsChooseCurrMovmOption;
                ViewBag.IsVehicleCurrentMovenet = IsVehicleCurrentMovenet;
                List<SORTMovementList> SORTAppMovementVersionobj = sortApplicationService.GetMovmentVersion(ProjectID);
                if (SORTAppMovementVersionobj.Count != 0)
                {
                    ViewBag.LatVer = SORTAppMovementVersionobj.Max(c => c.VersionNo);
                    ViewBag.LatestVersionId = SORTAppMovementVersionobj.Max(c => c.VersionID);
                    ViewBag.LatestAnalysisId = SORTAppMovementVersionobj.Max(c => c.AnalysisId);
                }
                return PartialView("SORTAppMovementVersion", SORTAppMovementVersionobj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SORTAppMovementVersion, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SORTAppCandidateRouteVersion
        public ActionResult SORTAppCandidateRouteVersion(int AnalysisId, string Prj_Status, string hauliermnemonic, int esdalref, long projectid, bool IsCandPermision, bool IsCurrentMovenet = false, bool IsVehicleCurrentMovenet = false)
        {
            try
            {
                string messg = "SORTApplications/SORTAppCandidateRouteVersion?AnalysisId=" + AnalysisId + ", Prj_Status=" + Prj_Status + ", hauliermnemonic=" + hauliermnemonic + ", esdalref=" + esdalref + ", projectid=" + projectid + ", IsCandPermision=" + IsCandPermision;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                ViewBag.hauliermnemonic = hauliermnemonic;
                ViewBag.esdalref = esdalref;
                ViewBag.HProject_Status = Prj_Status;
                ViewBag.AnalysisId = AnalysisId;
                ViewBag.IsCurrentMovenet = IsCurrentMovenet;
                ViewBag.IsVehicleCurrentMovenet = IsVehicleCurrentMovenet;
                List<CandidateRTModel> SORTAppCandRTDetails = sortApplicationService.GetCandidateRTDetails(projectid);
                List<CandidateRT> lstCandRt = new List<CandidateRT>();

                var CandRTdetails = SORTAppCandRTDetails.OrderBy(c => c.RouteID).GroupBy(c => c.RouteID).ToList();
                foreach (var candrts in CandRTdetails)
                {
                    CandidateRT model = new CandidateRT();
                    model.RouteId = candrts.Select(c => c.RouteID).FirstOrDefault();
                    model.Name = candrts.Select(c => c.Name).FirstOrDefault();
                    model.AnalysisId = candrts.Select(c => c.AnalysisID).FirstOrDefault();
                    model.CandidateDate = candrts.Select(c => c.CandidateDate).FirstOrDefault();
                    foreach (var revisions in candrts)
                    {
                        CRVersion revision = new CRVersion();
                        revision.ReviosionId = revisions.RevisionID;
                        revision.RevisionNo = revisions.RevisionNo;

                        model.Versions.Add(revision);
                    }
                    ViewBag.LastRevision = candrts.Max(c => c.RevisionID);
                    ViewBag.LastVersion = candrts.Max(c => c.RevisionNo);
                    ViewBag.LastCandRouteId = model.RouteId;
                    model.Versions = model.Versions.OrderBy(c => c.RevisionNo).ToList();
                    lstCandRt.Add(model);
                }
                ViewBag.IsCandEditPermision = IsCandPermision;
                return PartialView("SORTAppCandidateRouteVersion", lstCandRt);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SORTAppCandidateRouteVersion, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        //Create Candidate Route
        public ActionResult CreateCandidateRoute(int CandRevisioId = 0)
        {
            try
            {
                string messg = "SORTApplications/CreateCandidateRoute?CandRevisioId=" + CandRevisioId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                ViewBag.LatestCandRevId = CandRevisioId;
                return PartialView();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/CreateCandidateRoute, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        //Create Candidate Route Version
        public ActionResult CreateCandidateRouteVersion()
        {
            try
            {
                string messg = "SORTApplications/CreateCandidateRouteVersion";
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                return PartialView("CreateCandidateRouteVersion");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CreateCandidateRouteVersion, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        //Create new movement version
        public ActionResult CreateMovementVersion(CreateMovementVersionCntrlModel createMovementVersionCntrlModel)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                SessionInfo = (UserInfo)Session["UserInfo"];

                string messg = "SORTApplications/CreateMovementVersion?ProjectId=" + createMovementVersionCntrlModel.ProjectId + ", AppRevisionId=" + createMovementVersionCntrlModel.AppRevisionId + ", AnalysisId=" + createMovementVersionCntrlModel.AnalysisId + ", RouteRevisionId=" + createMovementVersionCntrlModel.RouteRevisionId + ", AppRef=" + createMovementVersionCntrlModel.AppRef + ", MovVersionNo=" + createMovementVersionCntrlModel.MovVersionNo + ", Haulnemonic=" + createMovementVersionCntrlModel.Haulnemonic + ", Esdalrefno=" + createMovementVersionCntrlModel.Esdalrefno;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                //Check affected parties generated.
                long movRevisionId = 0;
                dynamic newMovement = null;
                bool Isliveappication = false;
                int versionno = 0;
                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
                try
                {
                    Isliveappication = Convert.ToBoolean(ConfigurationSettings.AppSettings["LiveApplication"]);
                }
                catch
                {
                    Isliveappication = false;
                }
                if (Isliveappication == true)
                {
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(createMovementVersionCntrlModel.AnalysisId, 9, SessionInfo.UserSchema);
                    if (objRouteAssessmentModel.DriveInst != null && objRouteAssessmentModel.RouteDescription != null && objRouteAssessmentModel.AffectedStructure != null && objRouteAssessmentModel.Cautions != null && objRouteAssessmentModel.Constraints != null && objRouteAssessmentModel.Annotation != null && objRouteAssessmentModel.AffectedParties != null && objRouteAssessmentModel.AffectedRoads != null)
                    {
                        InsertMovementVersionInsertParams insertMovementVersionInsertParams = new InsertMovementVersionInsertParams();
                        insertMovementVersionInsertParams.ProjectID = createMovementVersionCntrlModel.ProjectId;
                        insertMovementVersionInsertParams.ApplicationReference = createMovementVersionCntrlModel.AppRef;
                        insertMovementVersionInsertParams.FromPrevious = 0;
                        insertMovementVersionInsertParams.UserSchema = SessionInfo.UserSchema;
                        newMovement = sortApplicationService.SaveMovementVersion(insertMovementVersionInsertParams);
                        versionno = newMovement.VerNo;
                        //Create movement action of revice movements
                        if (createMovementVersionCntrlModel.MovVersionNo != 0)
                        {
                            string movemntversion = createMovementVersionCntrlModel.Haulnemonic + "/" + createMovementVersionCntrlModel.Esdalrefno + "/S" + createMovementVersionCntrlModel.MovVersionNo;
                            SpecialOrderMovementActions(MovementnActionType.sort_desk_creates_movement_revisions, null, movemntversion, versionno, createMovementVersionCntrlModel.ProjectId, createMovementVersionCntrlModel.lastrevisionno, SessionInfo);
                        }
                    }
                }
                else
                {
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(createMovementVersionCntrlModel.AnalysisId, 7, SessionInfo.UserSchema);
                    if (objRouteAssessmentModel.AffectedParties != null)
                    {
                        InsertMovementVersionInsertParams insertMovementVersionInsertParams = new InsertMovementVersionInsertParams();
                        insertMovementVersionInsertParams.ProjectID = createMovementVersionCntrlModel.ProjectId;
                        insertMovementVersionInsertParams.ApplicationReference = createMovementVersionCntrlModel.AppRef;
                        insertMovementVersionInsertParams.FromPrevious = 0;
                        insertMovementVersionInsertParams.UserSchema = SessionInfo.UserSchema;
                        newMovement = sortApplicationService.SaveMovementVersion(insertMovementVersionInsertParams);
                        versionno = newMovement.VerNo;
                        //Create movement action of revice movements
                        if (createMovementVersionCntrlModel.MovVersionNo != 0)
                        {
                            string movemntversion = createMovementVersionCntrlModel.Haulnemonic + "/" + createMovementVersionCntrlModel.Esdalrefno + "/S" + createMovementVersionCntrlModel.MovVersionNo;
                            SpecialOrderMovementActions(MovementnActionType.sort_desk_creates_movement_revisions, null, movemntversion, versionno, createMovementVersionCntrlModel.ProjectId, createMovementVersionCntrlModel.lastrevisionno, SessionInfo);
                        }
                    }
                }
                var sortSOApplicationManage = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManage.GetSortAppPayload();
                SortAppPayload sortAppPayload = new SortAppPayload
                {
                    ProjectId = createMovementVersionCntrlModel.ProjectId,
                    LastVersionNo = versionno,
                    LastRevisionNo= sortPayload.LastRevisionNo
                };
                if (newMovement != null)
                {
                    
                    long MovVersionId = newMovement.MovVersionId;
                    long MovAnalysisId = newMovement.MovAnalysisId;
                    GenerateMovementRouteAssessment(MovVersionId, MovAnalysisId);
                    var data = new { MVersionId = newMovement.MovVersionId, MAnalysisId = newMovement.MovAnalysisId, IsLiveApp = Isliveappication };
                    var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                    var workflowEsdalReferenceNumber = WorkflowTaskFinder.GenerateEsdalReferenceNumber(createMovementVersionCntrlModel.Haulnemonic, createMovementVersionCntrlModel.Esdalrefno.ToString());
                    if (createMovementVersionCntrlModel.isWorkflow && !createMovementVersionCntrlModel.isVr1Application
                        && sortSOApplicationManagement.CheckIfProcessExit(workflowEsdalReferenceNumber)
                        && WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_CreateProposedMovementVersion, out dynamic workflowPayload) != string.Empty)
                    {
                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Ap_Activity_CreateProposedMovementVersion.ToString());
                        dataPayload.SortAppPayload = sortAppPayload;
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    }
                    return Json(JsonConvert.SerializeObject(data));
                }
                else
                {
                    var data = new { MVersionId = 0, MAnalysisId = 0, IsLiveApp = Isliveappication };
                    return Json(JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/CreateMovementVersion, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        //Save new candidate Route 
        [ValidateInput(false)]
        public ActionResult SaveCandidateRoute(SaveCandidateRouteCntrlModel saveCandidateRouteCntrlModel)
        {
            try
            {
                string messg = "SORTApplications/SaveCandidateRoute?RouteType=" + saveCandidateRouteCntrlModel.RouteType + ", name=" + saveCandidateRouteCntrlModel.name + ", ProjectId=" + saveCandidateRouteCntrlModel.ProjectId + ", AnalysisId=" + saveCandidateRouteCntrlModel.AnalysisId + ", LatestRevId=" + saveCandidateRouteCntrlModel.LatestRevId + ", AppRevisionId=" + saveCandidateRouteCntrlModel.AppRevisionId + ", EsdalRef=" + saveCandidateRouteCntrlModel.EsdalRef;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                long routeId = 0;
                long candanalysisId = 0;
                long versionId = 0;
                long candRevisionId = 0;
                int revisionNo = 0;
                int versionNo = 0;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                if (saveCandidateRouteCntrlModel.ProjectId != 0)
                {
                    //Create new candidate route
                    var candidateRouteInsertParams = new CandidateRouteInsertParams()
                    {
                        CandidateName = saveCandidateRouteCntrlModel.name,
                        ProjectID = saveCandidateRouteCntrlModel.ProjectId
                    };

                    CandidateRouteInsertResponse routedetails = sortApplicationService.SaveCandidateRoute(candidateRouteInsertParams);

                    routeId = routedetails.RouteId;
                    candanalysisId = routedetails.AnalysisId;
                    //create new versions                   
                    var routeRevisionInsertParams = new RouteRevisionInsertParams()
                    {
                        RouteID = routeId,
                        RouteType = saveCandidateRouteCntrlModel.RouteType
                    };
                    RouteRevision rtRevision = sortApplicationService.SaveRouteRevision(routeRevisionInsertParams);
                    revisionNo= rtRevision.RevisionNumber; 
                    versionId = rtRevision.RevisionNumber;
                    candRevisionId = rtRevision.RevisionId;
                    if (saveCandidateRouteCntrlModel.RouteType == "CandidateRT")
                    {
                        //clone the route parts to itself.                       
                        CloneRTPartsInsertParams cloneRTPartsInsertParams = new CloneRTPartsInsertParams()
                        {
                            Flag = 0,
                            OldRevisionID = saveCandidateRouteCntrlModel.LatestRevId,
                            RtRevisionID = rtRevision.RevisionId
                        };

                        sortApplicationService.CloneRouteParts(cloneRTPartsInsertParams);
                    }
                    else if (saveCandidateRouteCntrlModel.RouteType == "NewRoute")
                    {

                        CloneRTPartsInsertParams cloneRTPartsInsertParams = new CloneRTPartsInsertParams()
                        {
                            Flag = 1,
                            OldRevisionID = saveCandidateRouteCntrlModel.AppRevisionId,
                            RtRevisionID = rtRevision.RevisionId
                        };
                        sortApplicationService.CloneRouteParts(cloneRTPartsInsertParams);
                        CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams = new CloneApplicationRTPartsInsertParams();
                        cloneApplicationRTPartsInsertParams.OldRevisionID = saveCandidateRouteCntrlModel.AppRevisionId;
                        cloneApplicationRTPartsInsertParams.RTRevisionID = rtRevision.RevisionId;

                        sortApplicationService.CloneApplicationParts(cloneApplicationRTPartsInsertParams);
                    }
                    else
                    {
                        CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams = new CloneApplicationRTPartsInsertParams();
                        cloneApplicationRTPartsInsertParams.OldRevisionID = saveCandidateRouteCntrlModel.AppRevisionId;
                        cloneApplicationRTPartsInsertParams.RTRevisionID = rtRevision.RevisionId;
                        //Clone Application Parts
                        sortApplicationService.CloneApplicationParts(cloneApplicationRTPartsInsertParams);
                    }
                }

                #region movement actions for this action method
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.MovementActionType = MovementnActionType.user_creates_candidate_ver_from_revised_app_ver;
                string ErrMsg = string.Empty;
                movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                movactiontype.CandVerNo = (int)versionId;
                movactiontype.ESDALRef = saveCandidateRouteCntrlModel.EsdalRef;
                 var sortSOApplicationManagements = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManagements.GetSortAppPayload();
                string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, saveCandidateRouteCntrlModel.ProjectId, saveCandidateRouteCntrlModel.lastrevisionno, saveCandidateRouteCntrlModel.lastversionno, SessionInfo.UserSchema);

                #endregion
                var data = new { analysisid = candanalysisId, routeid = routeId, candRevId = candRevisionId };
                //SORT SO PROCESSING WORKFLOW CREATE CANDIDATE VERSION
                var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                if (saveCandidateRouteCntrlModel.isWorkflow && !saveCandidateRouteCntrlModel.isVr1Application && sortSOApplicationManagement.CheckIfProcessExit(saveCandidateRouteCntrlModel.EsdalReferenceWorkflow) && WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_CreateAndModifyCandidateVersion, out dynamic workflowPayload) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Ap_Activity_CreateAndModifyCandidateVersion.ToString());
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                }
                return Json(JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SaveCandidateRoute, Exception: {0}", ex));
                return Json(null);
            }
        }
        //Save new candidate Route 
        [ValidateInput(false)]
        public ActionResult UpdateCandidateRoute(string name, int ProjectId = 0, long RouteId = 0)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                UpdateCandidateRouteNMInsertParams updateCandidateRouteNMInsertParams = new UpdateCandidateRouteNMInsertParams();
                updateCandidateRouteNMInsertParams.Name = name;
                updateCandidateRouteNMInsertParams.ProjectId = ProjectId;
                updateCandidateRouteNMInsertParams.CandidateRouteId = RouteId;
                //Create new candidate route
                long routeId = sortApplicationService.UpdateCandidateRouteNM(updateCandidateRouteNMInsertParams);
                return Json(routeId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SaveCandidateRoute, Exception: {0}", ex));
                return Json(null);
            }
        }
        public ActionResult SORTAppEditCandiRouteName(long CnRouteID = 0)
        {
            ViewBag.CnRouteID = CnRouteID;
            string CnRouteNM = sortApplicationService.GetCandidateRouteNM(CnRouteID);
            ViewBag.CnRouteNM = CnRouteNM;
            return PartialView("SORTAppEditCandiRouteName");
        }
        public ActionResult UpdateSpecialOrder(int ProjectId = 0, int VersionId = 0, string esdal_ref = null, string sonumber = "")
        {
            int result = 0;
            if (Session["UserInfo"] == null)
                return RedirectToAction("Login", "Account");
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];


            UpdateSpecialOrderInsertParams updateSpecialOrderInsertParams = new UpdateSpecialOrderInsertParams()
            {
                ESDALReference = esdal_ref,
                ProjectId = ProjectId,
                VersionId = VersionId
            };
            result = sortApplicationService.UpdateSpecialOrder(updateSpecialOrderInsertParams);
            if (sonumber != "" && result != 0)
            {
                //Movement action of create special order
                SpecialOrderMovementActions(MovementnActionType.user_reuse_special_order, sonumber, esdal_ref, 0,0,0,SessionInfo);
            }
            return Json(result);
        }
        public ActionResult ViewCandidateCheckerUsers(string AllocationType)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                SessionInfo = (UserInfo)Session["UserInfo"];

                string messg = "SORTApplications/ViewCandidateCheckerUsers?AllocationType=" + AllocationType;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                int checkertype = 0;
                if (AllocationType == "signoff")
                    checkertype = 1;
                else if (AllocationType == "QAChecking")
                    checkertype = 2;

                List<SORTUserList> GridListObj = null;
                GridListObj = sortApplicationService.ListSORTUser(SessionInfo.UserTypeId, checkertype);
                //sortApplicationService.ListSORTUser(SessionInfo.UserTypeId, checkertype);
                SelectList GridListObjInfo = new SelectList(GridListObj, "UserID", "UserName");
                ViewBag.SORTUserListInfo = GridListObjInfo;
                ViewBag.AllocationType = AllocationType;
                return PartialView("CandidateCheckerAllocation");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/ViewCandidateCheckerUsers, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        //Updation for Send for checking and complete checking 
        public ActionResult CheckerUpdation(CheckerUpdationCntrlModel checkerUpdationCntrlModel)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                string messg = "SORTApplications/CheckerUpdation?Projectid=" + checkerUpdationCntrlModel.Projectid + ", Userid=" + checkerUpdationCntrlModel.Userid + ", Status=" + checkerUpdationCntrlModel.Status + ", CandRouteId=" + checkerUpdationCntrlModel.CandRouteId + ", CandRevisionId=" + checkerUpdationCntrlModel.CandRevisionId + ", CandVersiono=" + checkerUpdationCntrlModel.CandVersiono + ", CheckerName=" + checkerUpdationCntrlModel.CheckerName + ", AnalysisId=" + checkerUpdationCntrlModel.AnalysisId + ", AppRef=" + checkerUpdationCntrlModel.AppRef + ", PStatus=" + checkerUpdationCntrlModel.PStatus + ", MovVerNo=" + checkerUpdationCntrlModel.MovVerNo;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                RouteRevision rtRevision = new RouteRevision();
                bool updatestatus = false;
                bool routeassement = true;

                if (checkerUpdationCntrlModel.Status != 301006)
                {
                    if (checkerUpdationCntrlModel.Status == 301003 && checkerUpdationCntrlModel.PStatus == 307002)
                    {
                        if (checkerUpdationCntrlModel.CandRouteId == 0)
                        {
                            var data = new { result = false, analysisid = 0 };
                            return Json(JsonConvert.SerializeObject(data));
                        }
                        else
                        {
                            //create new candidate verison

                            RouteRevisionInsertParams routeRevisionInsertParams = new RouteRevisionInsertParams()
                            {
                                RouteID = checkerUpdationCntrlModel.CandRouteId,
                                RouteType = "Checking"
                            };

                            rtRevision = sortApplicationService.SaveRouteRevision(routeRevisionInsertParams);
                            if (rtRevision.RevisionNumber != 0)
                                checkerUpdationCntrlModel.CandVersiono = rtRevision.RevisionNumber;
                            //clone routes

                            CloneRTPartsInsertParams cloneRTPartsInsertParams = new CloneRTPartsInsertParams()
                            {
                                Flag = 0,
                                OldRevisionID = checkerUpdationCntrlModel.CandRevisionId,
                                RtRevisionID = rtRevision.RevisionId
                            };

                            sortApplicationService.CloneRouteParts(cloneRTPartsInsertParams);
                        }
                    }
                    UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams = new UpdateCheckerDetailsInsertParams()
                    {
                        CheckerId = checkerUpdationCntrlModel.Userid,
                        CheckerStatus = checkerUpdationCntrlModel.Status,
                        ProjectId = checkerUpdationCntrlModel.Projectid
                    };
                    updatestatus = sortApplicationService.CheckerDetailsUpdation(updateCheckerDetailsInsertParams);

                }
                else if (checkerUpdationCntrlModel.Status == 301006)
                {
                    UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams = new UpdateCheckerDetailsInsertParams()
                    {
                        CheckerId = checkerUpdationCntrlModel.Userid,
                        CheckerStatus = checkerUpdationCntrlModel.Status,
                        ProjectId = checkerUpdationCntrlModel.Projectid
                    };
                    updatestatus = sortApplicationService.CheckerDetailsUpdation(updateCheckerDetailsInsertParams);
                }
                //Minor enhancement 9927 updating special order sign data and state as 264003(signed and distributed)
                if (checkerUpdationCntrlModel.Status == 301006)
                {
                    SpecialOrderUpdationInsertParams specialOrderUpdationInsertParams = new SpecialOrderUpdationInsertParams()
                    {
                        ProjectId = checkerUpdationCntrlModel.Projectid,
                        MovmentVersionNumber = checkerUpdationCntrlModel.MovVerNo,
                        Status = checkerUpdationCntrlModel.Status
                    };
                    sortApplicationService.SpecialOrderUpdation(specialOrderUpdationInsertParams);
                }
                var sortSOApplicationManagement1 = new SORTSOApplicationManagement(sortSOProcessingService);
                //var test = sortSOApplicationManagement1.SearchPayloadItem(sortAppPayload);
                //Create movement actions.
                var sortSOApplicationManagements = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManagements.GetSortAppPayload();
                if (updatestatus)
                    CheckingMovementAction(checkerUpdationCntrlModel.Status, checkerUpdationCntrlModel.CandVersiono, checkerUpdationCntrlModel.CheckerName, checkerUpdationCntrlModel.MovVerNo, checkerUpdationCntrlModel.AppRef, checkerUpdationCntrlModel.Projectid, checkerUpdationCntrlModel.lastrevisionno, checkerUpdationCntrlModel.lastversionno, SessionInfo);
                var data1 = new { result = updatestatus, analysisid = rtRevision.NewAnalysisId, RouteAnalysis = routeassement };
                SortAppPayload sortAppPayload = new SortAppPayload
                {
                    ProjectId = checkerUpdationCntrlModel.Projectid,
                    LastRevisionNo = checkerUpdationCntrlModel.lastrevisionno,
                    LastVersionNo = checkerUpdationCntrlModel.lastversionno
                };
                if (checkerUpdationCntrlModel.isWorkflow && !checkerUpdationCntrlModel.isVr1Application)
                {
                    var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                    var esdalReference = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(checkerUpdationCntrlModel.AppRef);
                    if (esdalReference.Length > 0)
                    {
                        var nextWorkflowActivity = WorkflowTaskFinder.SORTSOSetActivityByStatus(checkerUpdationCntrlModel.Status);
                        if (nextWorkflowActivity != WorkflowActivityTypes.Gn_NotDecided && sortSOApplicationManagement.CheckIfProcessExit(esdalReference) && WorkflowTaskFinder.FindNextTask("SAP", nextWorkflowActivity, out dynamic workflowPayload) != string.Empty)
                        {
                            dynamic dataPayload = new ExpandoObject();
                            dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(nextWorkflowActivity.ToString());
                            dataPayload.SortAppPayload = sortAppPayload;
                            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                            {
                                data = dataPayload,
                                workflowData = workflowPayload
                            };
                            sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                        }
                    }
                }
                else if (checkerUpdationCntrlModel.isWorkflow && checkerUpdationCntrlModel.isVr1Application)
                {
                    var sortVR1ApplicationManagement = new SORTVR1ApplicationManagement(sortVR1ProcessingService);
                    var esdalReference = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(checkerUpdationCntrlModel.AppRef, true);
                    if (esdalReference.Length > 0)
                    {
                        var nextWorkflowActivity = WorkflowTaskFinder.SORTVR1SetActivityByStatus(checkerUpdationCntrlModel.Status);
                        if (nextWorkflowActivity != WorkflowActivityTypes.Gn_NotDecided && sortVR1ApplicationManagement.CheckIfProcessExit(esdalReference) && WorkflowTaskFinder.FindNextTask("SVR1P", nextWorkflowActivity, out dynamic workflowPayload) != string.Empty)
                        {
                            dynamic dataPayload = new ExpandoObject();
                            dataPayload.workflowActivityLog = sortVR1ApplicationManagement.SetWorkflowLog(nextWorkflowActivity.ToString());
                            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                            {
                                data = dataPayload,
                                workflowData = workflowPayload
                            };
                            sortVR1ApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                        }
                    }
                }
                return Json(JsonConvert.SerializeObject(data1));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/CheckerUpdation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #region VR Flow
        public ActionResult VR1CheckerUpdation(AllocateVr1CheckerUserCntrlModel allocateVr1CheckerUserCntrlModel)
        {
            try
            {
                string messg = "SORTApplications/VR1CheckerUpdation?Projectid=" + allocateVr1CheckerUserCntrlModel.Projectid + "Userid=" + allocateVr1CheckerUserCntrlModel.Userid + "Status=" + allocateVr1CheckerUserCntrlModel.Status + "CheckerName=" + allocateVr1CheckerUserCntrlModel.CheckerName + "OwnerName=" + allocateVr1CheckerUserCntrlModel.OwnerName + "MovVerNo=" + allocateVr1CheckerUserCntrlModel.MovVerNo + "AppRef=" + allocateVr1CheckerUserCntrlModel.AppRef;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];

                UpdateCheckerDetailsInsertParams updateCheckerDetailsInsertParams = new UpdateCheckerDetailsInsertParams()
                {
                    CheckerId = allocateVr1CheckerUserCntrlModel.Userid,
                    CheckerStatus = allocateVr1CheckerUserCntrlModel.Status,
                    ProjectId = allocateVr1CheckerUserCntrlModel.Projectid
                };
                bool updatestatus = sortApplicationService.CheckerDetailsUpdation(updateCheckerDetailsInsertParams);
                Vr1MovementAction(allocateVr1CheckerUserCntrlModel.Status, allocateVr1CheckerUserCntrlModel.CheckerName, allocateVr1CheckerUserCntrlModel.MovVerNo, allocateVr1CheckerUserCntrlModel.AppRef, allocateVr1CheckerUserCntrlModel.OwnerName, allocateVr1CheckerUserCntrlModel.Projectid, allocateVr1CheckerUserCntrlModel.lastrevisionno, allocateVr1CheckerUserCntrlModel.lastversionno, SessionInfo);
                if (updatestatus && allocateVr1CheckerUserCntrlModel.isWorkflow)
                {
                    var sortVR1ApplicationManagement = new SORTVR1ApplicationManagement(sortVR1ProcessingService);
                    var esdalReference = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(allocateVr1CheckerUserCntrlModel.AppRef, true);
                    if (esdalReference.Length > 0)
                    {
                        var nextWorkflowActivity = WorkflowTaskFinder.SORTVR1SetActivityByStatus(allocateVr1CheckerUserCntrlModel.Status);
                        if (nextWorkflowActivity != WorkflowActivityTypes.Gn_NotDecided && sortVR1ApplicationManagement.CheckIfProcessExit(esdalReference)
                            && WorkflowTaskFinder.FindNextTask("SVR1P", nextWorkflowActivity, out dynamic workflowPayload) != string.Empty)
                        {
                            dynamic dataPayload = new ExpandoObject();
                            dataPayload.workflowActivityLog = sortVR1ApplicationManagement.SetWorkflowLog(nextWorkflowActivity.ToString());
                            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                            {
                                data = dataPayload,
                                workflowData = workflowPayload
                            };
                            sortVR1ApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                        }
                    }
                }
                var data = new { result = updatestatus };

                return Json(JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/VR1CheckerUpdation, Exception: {0}", ex));
                throw ex;
            }
        }

        #endregion
        #endregion SORT Inbox

        #region SORTAllocateUserPopup
        public ActionResult SORTAllocateUser(long projectId = 0, long pln_user_id = 0, int revisionNo = 0)
        {
            try
            {
                string messg = "SORTApplications/SORTAllocateUser?projectId=" + projectId + "pln_user_id=" + pln_user_id + "revisionNo=" + revisionNo;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                var GridListObj = sortApplicationService.ListSORTUser(SessionInfo.UserTypeId);
                SelectList GridListObjInfo = new SelectList(GridListObj, "UserID", "UserName");
                ViewBag.SORTUserListInfo = GridListObjInfo;
                ViewBag.revisionNo = revisionNo;
                if (pln_user_id > 0)
                    ViewBag.planner_user_id = pln_user_id;
                AllocateSORTUserInsertParams allocateSortUserInsertParams = new AllocateSORTUserInsertParams()
                {
                    ProjectID = projectId,
                    RevisionNumber = revisionNo,
                    DueDate = string.Empty,
                    PlannerUserID = pln_user_id
                };
                List<string> a = sortApplicationService.SaveSORTAllocateUser(allocateSortUserInsertParams);
                if (a.Count() != 0)
                {
                    ViewBag.username = a[0];
                    ViewBag.date = a[1];
                    ViewBag.planner_user_id = Convert.ToInt32(a[2]);
                }
                return PartialView("SORTAllocateUser");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SORTAllocateUser, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region GenerateAmendmentDocument
        public ActionResult GenrateAmendDocuments(string SOnumber, Enums.DocumentType doctype)
        {
            try
            {
                string messg = "SORTApplications/GenrateAmendDocuments?SOnumber=" + SOnumber;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                //GenerateDocument doc = new GenerateDocument();
                bool generateFlag = false;
                byte[] docBuffer = sortApplicationService.GenerateAmendmentDocument(SOnumber, doctype, SessionInfo.organisationId, generateFlag);//added generateFlag flag for generate document in order to solve redmine #4959
                if (docBuffer != null)
                {
                    MemoryStream docStream = new MemoryStream(docBuffer);
                    return new FileStreamResult(docStream, "application/msword");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/GenrateAmendDocuments, Exception: {0}", ex));
                return Json(0);

            }
        }
        #endregion

        #region ApplicationWithAndDecline
        public JsonResult ApplicationWithAndDecline(int Project_ID = 0, int flag = 0, string EsdalRef = "",int lastRevisionno=0,int lastversionno=0)
        {
            try
            {
                string messg = "SORTApplications/ApplicationWithAndDecline?Project_ID=" + Project_ID + "flag=" + flag + "EsdalRef=" + EsdalRef;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo UserSessionValue = null; //--------object is used for storing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                movactiontype.ESDALRef = EsdalRef;
                string MovementDescription = "";
                int user_ID = Convert.ToInt32(UserSessionValue.UserId);
                string ErrMsg = string.Empty;
                int result = 0;
                int versionNo = 0;
                SORTAppWithdrawAndDeclineParams sortAppWithdrawAndDeclineParams = new SORTAppWithdrawAndDeclineParams()
                {
                    ProjectId = Project_ID,
                    Flag = flag
                };
                result = sortApplicationService.SORTAppWithdrawandDecline(sortAppWithdrawAndDeclineParams);
                if (result != 0)
                {
                    if (flag == 1)
                    {
                        #region movement actions for this action method
                        movactiontype.MovementActionType = MovementnActionType.sort_desktop_withdraws_appl;
                        #endregion

                        #region sys_events saving for withdrawing SORT application
                        movactiontype.SystemEventType = SysEventType.sort_withdrew_application;
                        #endregion
                    }
                    else if (flag == 2)
                    {
                        #region movement actions for this action method
                        movactiontype.MovementActionType = MovementnActionType.sort_desktop_declines_appl;
                        #endregion

                        #region sys_events saving for declined SORT application
                        movactiontype.SystemEventType = SysEventType.sort_declined_application;
                        movactiontype.OrganisatioName = UserSessionValue.OrganisationName;
                        #endregion
                    }
                    if (flag != 0)
                    {
                        var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                        var sortPayload = sortSOApplicationManagement.GetSortAppPayload();
                        MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                        long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, Project_ID, lastRevisionno, lastversionno, UserSessionValue.UserSchema);

                        //sys_events saving for withdrawing SORT application
                        #region sys_events saving for withdrawing SORT application
                        string sysEventDescp = System_Events.GetSysEventString(UserSessionValue, movactiontype, out ErrMsg);
                        // bool sysEvntResult = STP.Business.Persistence.MovementActionDAO.SaveSysEvents(movactiontype, sysEventDescp, user_ID, UserSessionValue.UserSchema);
                        bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, UserSessionValue.UserSchema);
                        #endregion
                    }
                }
                return Json(new { Success = result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ApplicationWithAndDecline, Exception: {0}", ex));
                return Json(0);
            }
        }
        #endregion

        #region ApplicationUnWithdraw
        public JsonResult ApplicationUnWithdraw(int Project_ID = 0, string EsdalRef = "",int lastrevisionno=0,int lastversionno=0)
        {
            try
            {
                int revisionNo = 0;
                int versionNo = 0;
                string messg = "SORTApplications/ApplicationUnWithdraw?Project_ID=" + Project_ID + "EsdalRef=" + EsdalRef;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo UserSessionValue = null; //--------object is used for storing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                movactiontype.ESDALRef = EsdalRef;
                string MovementDescription = "";
                int user_ID = Convert.ToInt32(UserSessionValue.UserId);
                string ErrMsg = string.Empty;
                int result = 0;
                SORTAppWithdrawAndDeclineParams sortAppWithdrawAndDeclineParams = new SORTAppWithdrawAndDeclineParams()
                {
                    ProjectId = Project_ID,
                    Flag = 0
                };
                result = sortApplicationService.SORTUnwithdraw(sortAppWithdrawAndDeclineParams);
                if (result != 0)
                {

                    #region movement actions for this action method
                    movactiontype.MovementActionType = MovementnActionType.sort_desktop_unwithdraw;
                    #endregion

                    #region sys_events saving for unwithdrawing SORT application
                    movactiontype.SystemEventType = SysEventType.sort_unwithdraw;
                    #endregion

                    var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                    var sortPayload = sortSOApplicationManagement.GetSortAppPayload();
                    MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, Project_ID, lastrevisionno, lastversionno, UserSessionValue.UserSchema);

                    //sys_events saving for unwithdrawing SORT application
                    #region sys_events saving for unwithdrawing SORT application
                    string sysEventDescp = System_Events.GetSysEventString(UserSessionValue, movactiontype, out ErrMsg);
                    bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, UserSessionValue.UserSchema);
                    #endregion

                }
                return Json(new { Success = result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ApplicationUnWithdraw, Exception: {0}", ex));
                return Json(0);
            }
        }
        #endregion

        #region AllocateSORTUser
        public ActionResult AllocateSORTUser(AllocateSORTUserCntrlModel allocateSORTUserCntrlModel)
        {
            try
            {
                int versionNo = 0;
                string messg = "SORTApplications/AllocateSORTUser?projectId=" + allocateSORTUserCntrlModel.projectId + "pln_user_id=" + allocateSORTUserCntrlModel.pln_user_id + "due_date=" + allocateSORTUserCntrlModel.due_date + "revisionNo=" + allocateSORTUserCntrlModel.revisionNo + "DropSort=" + allocateSORTUserCntrlModel.DropSort + "EsdalRef=" + allocateSORTUserCntrlModel.EsdalRef + "ProjectOwner=" + allocateSORTUserCntrlModel.ProjectOwner;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                var sortSOApplicationManagements = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayloads = sortSOApplicationManagements.GetSortAppPayload();
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }
                AllocateSORTUserInsertParams allocateSortUserInsertParams = new AllocateSORTUserInsertParams()
                {
                    ProjectID = allocateSORTUserCntrlModel.projectId,
                    RevisionNumber = allocateSORTUserCntrlModel.revisionNo,
                    DueDate = allocateSORTUserCntrlModel.due_date,
                    PlannerUserID = allocateSORTUserCntrlModel.pln_user_id
                };
                List<string> a = sortApplicationService.SaveSORTAllocateUser(allocateSortUserInsertParams);
                string result = a[0];
                if (result == "1")
                {
                    //---------------------------------------------------------------------

                    #region movement actions for this action method

                    MovementActionIdentifiers movactiontype = null;
                    string ErrMsg = string.Empty;
                    if (allocateSORTUserCntrlModel.ProjectOwner != "")
                    {
                        //Movement action of reallocation
                        movactiontype = new MovementActionIdentifiers();
                        movactiontype.MovementActionType = MovementnActionType.sort_desktop_reallocates_proj;
                        movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                        movactiontype.AllocateUser = allocateSORTUserCntrlModel.ProjectOwner;
                        movactiontype.ReallocateUser = allocateSORTUserCntrlModel.DropSort;
                        movactiontype.ESDALRef = allocateSORTUserCntrlModel.EsdalRef;
                        string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                        long reallcoationres = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, allocateSORTUserCntrlModel.projectId, allocateSORTUserCntrlModel.revisionNo, allocateSORTUserCntrlModel.versionNo, UserSessionValue.UserSchema);

                    }
                    else
                    {
                        //Movement action of allocation
                        movactiontype = new MovementActionIdentifiers();
                        movactiontype.MovementActionType = MovementnActionType.sort_desktop_allocates_proj;
                        movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                        movactiontype.AllocateUser = allocateSORTUserCntrlModel.DropSort;
                        movactiontype.ESDALRef = allocateSORTUserCntrlModel.EsdalRef;
                        string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                        long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, allocateSORTUserCntrlModel.projectId, allocateSORTUserCntrlModel.revisionNo, allocateSORTUserCntrlModel.versionNo, UserSessionValue.UserSchema);
                    }
                    #endregion

                    //---------------------------------------------------------------------
                }
                
                SortAppPayload sortAppPayload = new SortAppPayload
                {
                    ProjectId = allocateSORTUserCntrlModel.projectId,
                    LastRevisionNo = allocateSORTUserCntrlModel.revisionNo,
                    LastVersionNo = allocateSORTUserCntrlModel.versionNo
                };
                //SORT SO PROCESSING WORKFLOW STARTS HERE 
                if (allocateSORTUserCntrlModel.isWorkflow && allocateSORTUserCntrlModel.firstAllocate && !allocateSORTUserCntrlModel.isVr1Application)
                {
                    var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                    var sortPayload = sortSOApplicationManagement.GetSortAppPayload();
                    var esdalWorkflowRefrenceNumber = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(allocateSORTUserCntrlModel.EsdalRef);
                    if (esdalWorkflowRefrenceNumber.Length > 0
                        && !sortSOApplicationManagement.CheckIfProcessExit(esdalWorkflowRefrenceNumber, startProcess: false))
                    {
                        new SORTSOApplicationManagement(sortSOProcessingService).StartWorkflow(UserSessionValue.OrganisationId, UserSessionValue.OrganisationName, esdalWorkflowRefrenceNumber != "", esdalWorkflowRefrenceNumber, allocateSORTUserCntrlModel.projectId);
                        if (WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers, out dynamic workflowPayload, false) != string.Empty)
                        {

                            dynamic dataPayload = new ExpandoObject();
                            dataPayload.allocatedUser = allocateSORTUserCntrlModel.pln_user_id;
                            dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers.ToString());
                            dataPayload.SortAppPayload = sortAppPayload;
                            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                            {
                                data = dataPayload,
                                workflowData = workflowPayload
                            };
                            sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                        }
                    }
                }
                else if (allocateSORTUserCntrlModel.isWorkflow && allocateSORTUserCntrlModel.firstAllocate && allocateSORTUserCntrlModel.isVr1Application)
                {
                    var sortVR1ApplicationManagement = new SORTVR1ApplicationManagement(sortVR1ProcessingService);
                    var sortVR1Payload = sortVR1ApplicationManagement.GetSortAppPayload();
                    var esdalWorkflowRefrenceNumber = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(allocateSORTUserCntrlModel.EsdalRef, true);
                    if (esdalWorkflowRefrenceNumber.Length > 0
                        && !sortVR1ApplicationManagement.CheckIfProcessExit(esdalWorkflowRefrenceNumber, startProcess: false))
                    {
                        new SORTVR1ApplicationManagement(sortVR1ProcessingService).StartWorkflow(UserSessionValue.OrganisationId, UserSessionValue.OrganisationName, esdalWorkflowRefrenceNumber != "", esdalWorkflowRefrenceNumber, allocateSORTUserCntrlModel.projectId);
                        if (WorkflowTaskFinder.FindNextTask("SVR1P", WorkflowActivityTypes.Vr_Activity_AllocateApplication2RoutingOfficers, out dynamic workflowPayload, false) != string.Empty)
                        {

                            dynamic dataPayload = new ExpandoObject();
                            dataPayload.allocatedUser = allocateSORTUserCntrlModel.pln_user_id;
                            dataPayload.workflowActivityLog = sortVR1ApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Vr_Activity_AllocateApplication2RoutingOfficers.ToString());
                            dataPayload.SortAppPayload = sortAppPayload;
                            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                            {
                                data = dataPayload,
                                workflowData = workflowPayload
                            };
                            sortVR1ApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                        }
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/AllocateSORTUser, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region SaveMovementProjDetails
        public ActionResult SaveMovementProjDetails(long projectId = 0, string Mov_Name = "", string From_Add = "", string To_Add = "", string Load = "", string HA_Job_Ref = "")
        {
            try
            {
                string messg = "SORTApplications/SaveMovementProjDetails?projectId=" + projectId + "Mov_Name=" + Mov_Name + "From_Add=" + From_Add + "To_Add=" + To_Add + "Load=" + Load + "HA_Job_Ref=" + HA_Job_Ref;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                SORTMvmtProjectDetailsInsertParams sortMvmtProjectDetailsInsertParams = new SORTMvmtProjectDetailsInsertParams()
                {
                    FromAddress = From_Add,
                    HaulierJobReference = HA_Job_Ref,
                    Load = Load,
                    MovementName = Mov_Name,
                    ProjectId = projectId,
                    ToAddress = To_Add
                };
                int result = sortApplicationService.SaveSORTMovProjDetail(sortMvmtProjectDetailsInsertParams);

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/SaveMovementProjDetails, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region VR1ApprovalSubmit
        public ActionResult VR1ApprovalSubmit(ApproveVr1CntrlModel approveVr1CntrlModel)
        {
            try
            {
                int versionNo = 0;
                string messg = "SORTApplications/VR1ApprovalSubmit?ProjectId=" + approveVr1CntrlModel.ProjectId + "Rev_No=" + approveVr1CntrlModel.Rev_No + "EsdalRef=" + approveVr1CntrlModel.EsdalRef + "VR1_No=" + approveVr1CntrlModel.VR1_No;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action"); //this.ControllerContext.RouteData.Values["action"];
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                int result = 0;
                if (approveVr1CntrlModel.ProjectId != 0)
                {

                    VR1ApprovalInsertParams vr1ApprovalInsertParams = new VR1ApprovalInsertParams()
                    {
                        ProjectID = approveVr1CntrlModel.ProjectId,
                        RevisionNumber = approveVr1CntrlModel.Rev_No
                    };

                    result = sortApplicationService.SaveVR1Approval(vr1ApprovalInsertParams);
                }

                if (result != 0)
                {
                    #region movement actions for this action method
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    MovementActionIdentifiers movactiontype = null;
                    string ErrMsg = string.Empty;

                    //Movement action of allocation
                    movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.vr1_application_approved;
                    movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                    movactiontype.ESDALRef = approveVr1CntrlModel.EsdalRef;
                    movactiontype.VR1GenNum = approveVr1CntrlModel.VR1_No;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, approveVr1CntrlModel.ProjectId, approveVr1CntrlModel.RevisionNo, approveVr1CntrlModel.VersionNo, UserSessionValue.UserSchema);
                    #endregion
                    var sortVR1ApplicationManagement = new SORTVR1ApplicationManagement(sortVR1ProcessingService);
                    var esdalWorkflowRefrenceNumber = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(approveVr1CntrlModel.EsdalRef, true);
                    if (esdalWorkflowRefrenceNumber.Length > 0
                        && sortVR1ApplicationManagement.CheckIfProcessExit(esdalWorkflowRefrenceNumber, startProcess: true)
                        && WorkflowTaskFinder.FindNextTask("SVR1P", WorkflowActivityTypes.Vr_Activity_ApproveVR1, out dynamic workflowPayload, false) != string.Empty)
                    {

                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.workflowActivityLog = sortVR1ApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Vr_Activity_ApproveVR1.ToString());
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        sortVR1ApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    }
                }
                else
                {
                    var sortVR1ApplicationManagement = new SORTVR1ApplicationManagement(sortVR1ProcessingService);
                    var esdalWorkflowRefrenceNumber = WorkflowTaskFinder.WorkflowEsdalReferenceNumberBuilder(approveVr1CntrlModel.EsdalRef, true);
                    if (esdalWorkflowRefrenceNumber.Length > 0
                        && sortVR1ApplicationManagement.CheckIfProcessExit(esdalWorkflowRefrenceNumber, startProcess: true)
                        && WorkflowTaskFinder.FindNextTask("SVR1P", WorkflowActivityTypes.TheEnd, out dynamic workflowPayload, true) != string.Empty)
                    {

                        dynamic dataPayload = new ExpandoObject();
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        sortVR1ApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    }
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/VR1ApprovalSubmit, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        public ActionResult GenerateVR1Number(long ProjectId = 0)
        {
            try
            {
                string messg = "SORTApplications/GenerateVR1Number?ProjectId=" + ProjectId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action"); //this.ControllerContext.RouteData.Values["action"];
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                string result = "";
                if (ProjectId != 0 && SessionInfo.ContactId != 0)
                {
                    VR1NumberInsertParams vR1NumberInsertParams = new VR1NumberInsertParams()
                    {
                        ContactId = SessionInfo.ContactId,
                        ProjectId = ProjectId
                    };
                    result = sortApplicationService.SaveVR1Number(vR1NumberInsertParams);
                }


                return Json(result);
            }
            catch (Exception ex)
            {

                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/GenerateVR1Number, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult GenerateVR1Document(string Haulier_numeric = "", string Esdal_Ref = "", int Mov_Latest_ver = 0)//long ProjectId = 0, string VR1Number = ""
        {
            try
            {
                string messg = "SORTApplications/GenerateVR1Document?Haulier_numeric=" + Haulier_numeric + "Esdal_Ref=" + Esdal_Ref + "Mov_Latest_ver=" + Mov_Latest_ver;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                int result = 0;
                if (Haulier_numeric != "" && Esdal_Ref != "" && Mov_Latest_ver != 0)
                {
                    //Generate vr1 document here.
                    bool generateFlag = false;
                    byte[] docBuffer = sortApplicationService.GenerateFormVR1Document(Haulier_numeric, Esdal_Ref, Mov_Latest_ver, generateFlag);//added generateFlag flag for generate document in order to solve redmine #4959
                    if (docBuffer != null)
                    {
                        result = 1;
                        MemoryStream docStream = new MemoryStream(docBuffer);
                        var docname = new FileStreamResult(docStream, "application/msword");
                        docname.FileDownloadName = Haulier_numeric + "/" + Esdal_Ref + "/" + Mov_Latest_ver + ".doc";
                        return docname;
                    }
                    else
                    {
                        return null; //TODO error handling
                    }
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/GenerateVR1Document, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region MovementAgreeUnagreeWithdraw
        public ActionResult MovementAgreeUnagreeWithdraw(MovementAgreeUnagreeWithdrawCntrlModel movementAgreeUnagreeWithdrawCntrlModel)
        {
            try
            {
                int revisionNo = 0;
                string messg = "SORTApplications/MovementAgreeUnagreeWithdraw?Version_Id=" + movementAgreeUnagreeWithdrawCntrlModel.Version_Id + "flag=" + movementAgreeUnagreeWithdrawCntrlModel.flag + "esdalRef=" + movementAgreeUnagreeWithdrawCntrlModel.esdalRef + "ProjectId=" + movementAgreeUnagreeWithdrawCntrlModel.ProjectId + "AppRef=" + movementAgreeUnagreeWithdrawCntrlModel.AppRef + "AnalysisId=" + movementAgreeUnagreeWithdrawCntrlModel.AnalysisId + "VersionNo=" + movementAgreeUnagreeWithdrawCntrlModel.VersionNo;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.ContactName = UserSessionValue.FirstName + " " + UserSessionValue.LastName;
                movactiontype.ESDALRef = movementAgreeUnagreeWithdrawCntrlModel.esdalRef;
                string MovementDescription = "";
                string ErrMsg = string.Empty;
                var sortSOApplicationManagements = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManagements.GetSortAppPayload();
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action"); //this.ControllerContext.RouteData.Values["action"];
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                int result = 0;
                //Create new movement version for agreed
                if (movementAgreeUnagreeWithdrawCntrlModel.flag == 1)
                {
                    movementAgreeUnagreeWithdrawCntrlModel.Version_Id = 0;
                    InsertMovementVersionInsertParams insertMovementVersionInsertParams = new InsertMovementVersionInsertParams()
                    {
                        ProjectID = movementAgreeUnagreeWithdrawCntrlModel.ProjectId,
                        ApplicationReference = movementAgreeUnagreeWithdrawCntrlModel.AppRef,
                        FromPrevious = 1,
                        UserSchema = UserSessionValue.UserSchema
                    };
                    dynamic newMovement = sortApplicationService.SaveMovementVersion(insertMovementVersionInsertParams);
                    if (newMovement != null)
                        movementAgreeUnagreeWithdrawCntrlModel.Version_Id = newMovement.MovVersionId;
                    if (movementAgreeUnagreeWithdrawCntrlModel.Version_Id != 0)
                    {
                        MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams = new MovementVersionAgreeUnagreeWithInsertParams()
                        {
                            VersionId = movementAgreeUnagreeWithdrawCntrlModel.Version_Id,
                            Flag = movementAgreeUnagreeWithdrawCntrlModel.flag,
                        };
                        result = sortApplicationService.MovementVersionAgreeUnagreeWith(movementVersionAgreeUnagreeWithInsertParams);
                        if (result != 0)
                        {
                            #region movement actions for this action method
                            if (!string.IsNullOrEmpty(movactiontype.ESDALRef))
                            {
                                string[] strsplit = movactiontype.ESDALRef.Split('/');
                                if (strsplit != null && movementAgreeUnagreeWithdrawCntrlModel.VersionNo != 0)
                                {
                                    movementAgreeUnagreeWithdrawCntrlModel.VersionNo++;
                                    movactiontype.ESDALRef = strsplit[0].ToString() + "/" + strsplit[1].ToString() + "/S" + movementAgreeUnagreeWithdrawCntrlModel.VersionNo.ToString();
                                }
                            }
                            movactiontype.MovementActionType = MovementnActionType.user_agrees_movement_version;
                            #endregion
                            MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                            long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, movementAgreeUnagreeWithdrawCntrlModel.ProjectId, movementAgreeUnagreeWithdrawCntrlModel.RevisionNo, movementAgreeUnagreeWithdrawCntrlModel.VersionNo, UserSessionValue.UserSchema);

                        }
                    }
                }
                else
                {
                    MovementVersionAgreeUnagreeWithInsertParams movementVersionAgreeUnagreeWithInsertParams = new MovementVersionAgreeUnagreeWithInsertParams()
                    {
                        VersionId = movementAgreeUnagreeWithdrawCntrlModel.Version_Id,
                        Flag = movementAgreeUnagreeWithdrawCntrlModel.flag,
                    };
                    result = sortApplicationService.MovementVersionAgreeUnagreeWith(movementVersionAgreeUnagreeWithInsertParams);
                    if (result != 0)
                    {
                        if (movementAgreeUnagreeWithdrawCntrlModel.flag == 2)
                        {
                            #region movement actions for this action method
                            movactiontype.MovementActionType = MovementnActionType.user_unagrees_movement_version;
                            #endregion
                        }
                        else if (movementAgreeUnagreeWithdrawCntrlModel.flag == 3)
                        {
                            #region movement actions for this action method
                            movactiontype.MovementActionType = MovementnActionType.sort_withdrawal_request_receipt;
                            #endregion
                        }
                        if (movementAgreeUnagreeWithdrawCntrlModel.flag == 2)
                        {
                            MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                            long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, movementAgreeUnagreeWithdrawCntrlModel.ProjectId, movementAgreeUnagreeWithdrawCntrlModel.RevisionNo, movementAgreeUnagreeWithdrawCntrlModel.VersionNo, UserSessionValue.UserSchema);
                        }
                    }
                }
                SortAppPayload sortAppPayload = new SortAppPayload
                {   
                    ProjectId = movementAgreeUnagreeWithdrawCntrlModel.ProjectId,
                    LastRevisionNo = movementAgreeUnagreeWithdrawCntrlModel.RevisionNo,
                    LastVersionNo = movementAgreeUnagreeWithdrawCntrlModel.VersionNo
                };
                if (result != 0 && movementAgreeUnagreeWithdrawCntrlModel.isWorkflow && !movementAgreeUnagreeWithdrawCntrlModel.isVr1Application)
                {
                    var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                    var sortPayloads = sortSOApplicationManagement.GetSortAppPayload();
                    var esdalReference = movementAgreeUnagreeWithdrawCntrlModel.EsdalReferenceWorkflow;
                    WorkflowActivityTypes workflowActivityTypes = WorkflowActivityTypes.Ap_Activity_AgreeMovement;
                    switch (movementAgreeUnagreeWithdrawCntrlModel.flag)
                    {
                        case 1:
                            workflowActivityTypes = WorkflowActivityTypes.Ap_Activity_AgreeMovement;
                            break;
                        case 2:
                            workflowActivityTypes = WorkflowActivityTypes.Ap_Activity_CreateProposedMovementVersion;
                            break;
                    }
                    if (movementAgreeUnagreeWithdrawCntrlModel.isWorkflow && !movementAgreeUnagreeWithdrawCntrlModel.isVr1Application
                        && sortSOApplicationManagement.CheckIfProcessExit(esdalReference)
                        && WorkflowTaskFinder.FindNextTask("SAP", workflowActivityTypes, out dynamic workflowPayload) != string.Empty)
                    {
                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(workflowActivityTypes.ToString());
                        dataPayload.SortAppPayload = sortAppPayload;
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/MovementAgreeUnagreeWithdraw, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region CheckVehicleForApplication(int PROJ_ID = 0, int Rev_Id=0)
        public ActionResult CheckVehicleForApplication(int PROJ_ID = 0, int Rev_Id = 0)
        {
            try
            {
                string messg = "SORTApplications/CheckVehicleForApplication?Project_id=" + PROJ_ID + "&Rev_Id=" + Rev_Id;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                bool data = sortApplicationService.CheckVehicleOnRoute(PROJ_ID, Rev_Id);
                return Json(data);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CheckVehicleForApplication, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        public ActionResult Acknowledgement(long ProjectId = 0, string EsdalReference = "", int MovVersionNo = 0, int AppRevisionNo = 0, int RevisedBySort = 0, bool IsVr1 = false)
        {
            try
            {
                string messg = "SORTApplications/Acknowledgement?ProjectId=" + ProjectId + "EsdalReference=" + EsdalReference + "MovVersionNo=" + MovVersionNo + "AppRevisionNo=" + AppRevisionNo + "RevisedBySort=" + RevisedBySort + "IsVr1=" + IsVr1;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];
                decimal result = 0;
                if (ProjectId != 0 && EsdalReference != "" && AppRevisionNo != 0)
                {
                    int isSo = 1;
                    if (IsVr1)
                        isSo = 0;
                    UpdateProjectDetailsInsertParams updateProjectDetailsInsertParams = new UpdateProjectDetailsInsertParams()
                    {
                        ProjectId = ProjectId,
                        IsSO = isSo,
                        UserSchema = SessionInfo.UserSchema
                    };
                    result = sortApplicationService.UpdateProjectDetails(updateProjectDetailsInsertParams);
                    if (result == 1)
                    {
                        #region movement actions for this action method

                        MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                        movactiontype.MovementActionType = MovementnActionType.sort_desk_acknowl_proj_revisions;
                        string ErrMsg = string.Empty;
                        movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                        movactiontype.ESDALRef = EsdalReference;
                        movactiontype.MovementVer = MovVersionNo;
                        movactiontype.RevisionNo = AppRevisionNo;
                        movactiontype.ReviseBySort = true;
                        if (RevisedBySort == 1)
                            movactiontype.ReviseBySort = false;

                        string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                        long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, ProjectId, AppRevisionNo, MovVersionNo, SessionInfo.UserSchema);
                        #endregion
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/Acknowledgement, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        //Create new candiadte route vesrion
        public ActionResult CreateCandidateVersion(CreateCandidateVersionCntrlModel createCandidateVersionCntrlModel)
        {
            try
            {
                int projectId = 0;
                int versionNo = 0;
                string messg = "SORTApplications/CreateCandidateVersion?CandRouteId=" + createCandidateVersionCntrlModel.CandRouteId + ", CandRevisionId=" + createCandidateVersionCntrlModel.CandRevisionId + ", CloneType=" + createCandidateVersionCntrlModel.CloneType + ", AppRevId=" + createCandidateVersionCntrlModel.AppRevId + ", EsdalRef=" + createCandidateVersionCntrlModel.EsdalRef;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                RouteRevision rtRevision = new RouteRevision();
                var sortSOApplicationManagements = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManagements.GetSortAppPayload();
                if (createCandidateVersionCntrlModel.CloneType == "application")
                {
                    if (createCandidateVersionCntrlModel.CandRouteId != 0 && createCandidateVersionCntrlModel.AppRevId != 0)
                    {
                        RouteRevisionInsertParams routeRevisionInsertParams = new RouteRevisionInsertParams();
                        routeRevisionInsertParams.RouteID = createCandidateVersionCntrlModel.CandRouteId;
                        routeRevisionInsertParams.RouteType = createCandidateVersionCntrlModel.CloneType;
                        rtRevision = sortApplicationService.SaveRouteRevision(routeRevisionInsertParams);
                        if (rtRevision.RevisionId != 0)
                        {
                            CloneRTPartsInsertParams cloneRTPartsInsertParams = new CloneRTPartsInsertParams();
                            cloneRTPartsInsertParams.OldRevisionID = createCandidateVersionCntrlModel.AppRevId;
                            cloneRTPartsInsertParams.RtRevisionID = rtRevision.RevisionId;
                            cloneRTPartsInsertParams.Flag = 1;
                            sortApplicationService.CloneRouteParts(cloneRTPartsInsertParams);
                            CloneApplicationRTPartsInsertParams cloneApplicationRTPartsInsertParams = new CloneApplicationRTPartsInsertParams();
                            cloneApplicationRTPartsInsertParams.OldRevisionID = createCandidateVersionCntrlModel.AppRevId;
                            cloneApplicationRTPartsInsertParams.RTRevisionID = rtRevision.RevisionId;
                            sortApplicationService.CloneApplicationParts(cloneApplicationRTPartsInsertParams);

                            #region movement actions for this action method
                            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                            movactiontype.MovementActionType = MovementnActionType.user_creates_candidate_ver_from_revised_app_ver;
                            string ErrMsg = string.Empty;
                            movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.CandVerNo = rtRevision.RevisionNumber;
                            movactiontype.ESDALRef = createCandidateVersionCntrlModel.EsdalRef;

                            string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, sortPayload.ProjectId, createCandidateVersionCntrlModel.LastRevisionNo, createCandidateVersionCntrlModel.LastversionNo, SessionInfo.UserSchema);
                            #endregion
                        }
                    }
                }
                else if (createCandidateVersionCntrlModel.CloneType == "lastcandidate")
                {
                    if (createCandidateVersionCntrlModel.CandRouteId != 0 && createCandidateVersionCntrlModel.CandRevisionId != 0)
                    {
                        RouteRevisionInsertParams routeRevisionInsertParams = new RouteRevisionInsertParams();
                        routeRevisionInsertParams.RouteID = createCandidateVersionCntrlModel.CandRouteId;
                        routeRevisionInsertParams.RouteType = createCandidateVersionCntrlModel.CloneType;
                        rtRevision = sortApplicationService.SaveRouteRevision(routeRevisionInsertParams);
                        if (rtRevision.RevisionId != 0)
                        {
                            CloneRTPartsInsertParams cloneRTPartsInsertParams = new CloneRTPartsInsertParams();
                            cloneRTPartsInsertParams.OldRevisionID = createCandidateVersionCntrlModel.CandRevisionId;
                            cloneRTPartsInsertParams.RtRevisionID = rtRevision.RevisionId;
                            cloneRTPartsInsertParams.Flag = 0;
                            sortApplicationService.CloneRouteParts(cloneRTPartsInsertParams);

                            #region movement actions for this action method
                            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                            movactiontype.MovementActionType = MovementnActionType.user_creates_candidate_ver_from_revised_candidate_ver;
                            string ErrMsg = string.Empty;
                            movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.CandVerNo = rtRevision.RevisionNumber;
                            movactiontype.ESDALRef = createCandidateVersionCntrlModel.EsdalRef;

                            string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, sortPayload.ProjectId, createCandidateVersionCntrlModel.LastRevisionNo, createCandidateVersionCntrlModel.LastversionNo, SessionInfo.UserSchema);
                            #endregion
                        }
                    }
                }
                var data = new { newrivisionId = rtRevision.RevisionId, newversionNo = rtRevision.RevisionNumber, analysisid = rtRevision.NewAnalysisId };
                SortAppPayload sortAppPayload = new SortAppPayload
                {
                    ProjectId = sortPayload.ProjectId,
                    LastRevisionNo = createCandidateVersionCntrlModel.LastRevisionNo,
                    LastVersionNo= createCandidateVersionCntrlModel.LastversionNo
                };
                var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                var workflowEsdalReferenceNumber = createCandidateVersionCntrlModel.EsdalReferenceWorkflow;
                if (sortSOApplicationManagement.CheckIfProcessExit(workflowEsdalReferenceNumber) && WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_CreateAndModifyCandidateVersion, out dynamic workflowPayload) != string.Empty)
                {
                    dynamic dataPayload = new ExpandoObject();
                    dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Ap_Activity_CreateAndModifyCandidateVersion.ToString());
                    dataPayload.SortAppPayload = sortAppPayload;
                    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    {
                        data = dataPayload,
                        workflowData = workflowPayload
                    };
                    sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                }
                return Json(JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/CreateCandidateVersion, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult SortGenerelDetails(String hauliermnemonic = "", int esdalref = 0, int revisionno = 0, int versionno = 0, long Rev_ID = 0, string Checker = "", string OwnerName = "", long ProjectId = 0, bool VR1App = false, int historic = 0)
        {
            UserInfo SessionInfo = null;
            SOApplication SORTProjectOverviewobj = null;
            string result = "";
            try
            {

                SessionInfo = (UserInfo)Session["UserInfo"];
                int IUserID = Convert.ToInt32(SessionInfo.UserId);




                SORTProjectOverviewobj = sortApplicationService.GetProjOverviewDetails(Rev_ID);

                if (SORTProjectOverviewobj.CheckingStatus == "")
                    SORTProjectOverviewobj.CheckingStatus = "Not Checking";

                if (SORTProjectOverviewobj.CheckerName != "")
                {
                    string checkername = SORTProjectOverviewobj.CheckerName;
                    checkername = checkername.First().ToString().ToUpper() + checkername.Substring(1);
                    SORTProjectOverviewobj.CheckerName = checkername;
                }

                if (SORTProjectOverviewobj.ApplicationStatus == 308002 && SessionInfo.SortUserId == SORTProjectOverviewobj.PlannerUserId && SessionInfo.SortUserId != 0)
                    ViewBag.CandidatePermission = true;
                else
                    ViewBag.CandidatePermission = false;

                long ProjectID = SORTProjectOverviewobj.ProjectId;
                int status = applicationService.GetApplicationStatus(versionno, revisionno, ProjectId, SessionInfo.UserSchema, historic);

                ViewBag.ApplStatus = status;
                if (hauliermnemonic != "" && esdalref != 0)
                {
                    ViewBag.ESDAL_Reference = SORTProjectOverviewobj.ESDALReference;
                    ViewBag.Version_Status = SORTProjectOverviewobj.VersionStatus;
                }
                ViewBag.hauliermnemonic = hauliermnemonic;
                ViewBag.esdalref = esdalref;
                if (!string.IsNullOrEmpty(OwnerName))
                    OwnerName = OwnerName = Char.ToUpper(OwnerName[0]) + OwnerName.Substring(1);
                OwnerName = OwnerName.Replace("%20", " ");
                ViewBag.OwnerName = OwnerName;
                ViewBag.Checker = Checker;
                ViewBag.PStatus = SORTProjectOverviewobj.ProjectStatus;

                //VR1 Details
                if (VR1App == true)
                {
                    result = sortApplicationService.GetVR1ApprovalDate(ProjectId);
                    var VR1Detail = result.Split(',');
                    if (result != "False")
                    {
                        SORTProjectOverviewobj.VR1ApprovalDate = VR1Detail[0];
                        SORTProjectOverviewobj.VR1Number = VR1Detail[1];
                    }
                }
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.CheckingStatus))
                    SORTProjectOverviewobj.CheckingStatus = Char.ToUpper(SORTProjectOverviewobj.CheckingStatus[0]) + SORTProjectOverviewobj.CheckingStatus.Substring(1);
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.ProjectStatus))
                    SORTProjectOverviewobj.ProjectStatus = Char.ToUpper(SORTProjectOverviewobj.ProjectStatus[0]) + SORTProjectOverviewobj.ProjectStatus.Substring(1);
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.CheckerName))
                    SORTProjectOverviewobj.CheckerName = Char.ToUpper(SORTProjectOverviewobj.CheckerName[0]) + SORTProjectOverviewobj.CheckerName.Substring(1);
                if (!string.IsNullOrEmpty(SORTProjectOverviewobj.HaulierName))
                    SORTProjectOverviewobj.HaulierName = Char.ToUpper(SORTProjectOverviewobj.HaulierName[0]) + SORTProjectOverviewobj.HaulierName.Substring(1);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SortGenerelDetails, Exception: {0}", ex));
                return PartialView("SortGenerelDetails", SORTProjectOverviewobj);
            }
            return PartialView("SortGenerelDetails", SORTProjectOverviewobj);
        }
        //Check all routes are assigned or not
        public ActionResult ChkVehAsgnToAllRoutPrts(int revisionId = 0)
        {
            try
            {
                string messg = "SORTApplications/CheckRouteType?revisionId=" + revisionId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                string result = null;
                if (revisionId != 0)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    bool res = sortApplicationService.GetCandRouteVehicleAssignDetails(revisionId, SessionInfo.UserSchema);
                    if (res)
                        result = "Please assign candidate vehicles to all route parts!";
                    else
                    {
                        List<CRDetails> routedetails = sortApplicationService.GetRouteType(revisionId, SessionInfo.UserSchema);
                        var test = 0;
                        routedetails = routedetails.Where(c => c.SegmentNumber == 0).ToList();
                        if (routedetails.Count != 0)
                        {
                            int rcount = 1;
                            foreach (var rdeatils in routedetails)
                            {
                                if (routedetails.Count != rcount)
                                    result = result + rdeatils.RouteName + ", ";
                                else
                                {
                                    if (routedetails.Count == 1)
                                        result = rdeatils.RouteName + " route is not planned! All routes should be planned before generating driving instructions";
                                    else
                                    {
                                        result = result.Substring(0, result.Length - 1);
                                        result = result + "and " + rdeatils.RouteName + " routes are not planned! All routes should be planned before generating driving instructions";
                                    }
                                }
                                rcount++;
                            }
                        }
                    }
                }
                return Json(new { dataval = result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CheckRouteType, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult CheckAffPartBeforeSending(int AnalysisId = 0, int APP_Rev_ID = 0, bool VR1App = false, string CType = "", long MovAnalysisId = 0)
        {
            try
            {
                string messg = "SORTApplications/CheckAffPartBeforeSending?AnalysisId=" + AnalysisId + "APP_Rev_ID=" + APP_Rev_ID + "VR1App=" + VR1App + "CType=" + CType;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];
                //Check affected parties generated.
                long movRevisionId = 0;
                int isModified = 0;
                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
                if (CType == "SChecking" || CType == "CSChecking")
                {
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(AnalysisId, 9, SessionInfo.UserSchema);
                    isModified = sortApplicationService.CheckCandIsModified(AnalysisId);
                }
                else
                {
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(MovAnalysisId, 9, SessionInfo.UserSchema);
                }
                SOApplication SORTProjectOverviewobj = null;
                SORTProjectOverviewobj = sortApplicationService.GetProjOverviewDetails(APP_Rev_ID);
                if (SORTProjectOverviewobj.MovementName != "" && SORTProjectOverviewobj.HAJobFileReference != "" && SORTProjectOverviewobj.Load != "" && SORTProjectOverviewobj.FromAddress != "" && SORTProjectOverviewobj.ToAddress != "")
                {
                    if (VR1App == false)
                    {
                        if (objRouteAssessmentModel.DriveInst == null)
                            movRevisionId = 3;
                        else if (objRouteAssessmentModel.AffectedStructure == null)
                            movRevisionId = 4;
                        else if (objRouteAssessmentModel.RouteDescription == null)
                            movRevisionId = 5;
                        else if (objRouteAssessmentModel.AffectedParties == null)
                            movRevisionId = 6;
                        else if (objRouteAssessmentModel.AffectedRoads == null)
                            movRevisionId = 7;
                        else
                            movRevisionId = 1;
                    }
                    else
                        movRevisionId = 1;
                }
                else
                {
                    if (!VR1App)
                        movRevisionId = 2;//for project details checking
                    else
                        movRevisionId = 1;
                }
                return Json(movRevisionId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CheckAffPartBeforeSending, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        //view Checking status popup
        public ActionResult UpdateCheckingStatusPopup(int ApprsionId, string AllocationType, int OrganisationId = 0, int VersionId = 0)
        {
            try
            {
                string messg = "SORTApplications/UpdateCheckingStatusPopup?ApprsionId=" + ApprsionId + ", AllocationType=" + AllocationType + ", OrganisationId=" + OrganisationId + ", VersionId=" + VersionId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                SessionInfo = (UserInfo)Session["UserInfo"];

                List<SORTUserList> GridListObj = null;
                if (AllocationType == "checking" || AllocationType == "QAChecking")
                {
                    GridListObj = sortApplicationService.ListSORTUser(SessionInfo.UserTypeId);

                    SelectList GridListObjInfo = new SelectList(GridListObj, "UserID", "UserName");
                    ViewBag.SORTUserListInfo = GridListObjInfo;
                }
                ViewBag.AllocationType = AllocationType;

                //Read application details
                //SOApplication SORTAppSumObj = ApplicationProvider.Instance.GetSOGeneralWorkinProcessbyrevisionid(SessionInfo.userSchema, ApprsionId, VersionId, OrganisationId);

                return PartialView("UpdateCheckingStatusPopup");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/UpdateCheckingStatusPopup, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult SORTSpecialOrderView(long ProjectID = 0)
        {
            try
            {
                string messg = "SORTApplications/SORTSpecialOrderView?ProjectID=" + ProjectID;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                List<SORTMovementList> SORTSpecialOrderViewobj = new List<SORTMovementList>();
                SORTSpecialOrderViewobj = sortDocumentService.GetSpecialOrderList(ProjectID);
                if (SORTSpecialOrderViewobj.Count != 0)
                {
                    var result = SORTSpecialOrderViewobj.OrderByDescending(t => t.SOCreateDate).First();
                    ViewBag.SoNumber = result.OrderNumber;
                }
                return PartialView("SORTSpecialOrderView", SORTSpecialOrderViewobj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SORTSpecialOrderView, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult HaulierDetails(string hauliermnemonic, string SORTStatus = "", string msg = "", long? OrgID = null, int movementId = 0, int cloneapprevid = 0, int esdalref = 0, int revisionno = 0, int versionno = 0, int revisionId = 0, long versionId = 0, int apprevid = 0, int OrganisationId = 0, int projecid = 0, bool VR1Applciation = false, bool reduceddetailed = false, long? pageflag = 0, bool EditRev = false, int EditFlag = 0, string esdal_history = "", int LatestVer = 0, int arev_no = 0, int arev_Id = 0, int ver_no = 0, string candName = "", int candVersionno = 0, string Owner = "", string WorkStatus = "", string Checker = "", long analysisId = 0, bool IsLastVersion = false, int EnterBySORT = 0, int ViewFlag = 0)
        {
            ViewBag.IsLastVersion = IsLastVersion;
            ViewBag.CandName = candName;
            ViewBag.CandVersionNo = candVersionno;
            ViewBag.CandAnalysisId = analysisId;
            ViewBag.ViewFlag = ViewFlag;
            if (projecid == 0)
                ViewBag.isReviseApp = false;
            else
                ViewBag.isReviseApp = true;
            //if (Session["UserInfo"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            if (!PageAccess.GetPageAccess("13004") && !PageAccess.GetPageAccess("13005"))
            {
                return RedirectToAction("Error", "Home");
            }
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            ViewBag.SortUserId = SessionInfo.SortUserId;
            switch (SORTStatus)
            {
                case "CreateSO":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "CreateVR1":
                    ViewBag.SORTStatus = SORTStatus;
                    VR1Applciation = true;//-----added for resolving issue redmine-3944
                    break;
                case "ViewProj":
                    ViewBag.SORTStatus = SORTStatus;
                    Session["SortListMUrl"] = ("/SORTApplication/SORTListMovemnets?SORTStatus=ViewProj&revisionId=" + revisionId + "&versionId=" + versionId + "&hauliermnemonic=" + hauliermnemonic + "&esdalref=" + esdalref + "&revisionno=" + revisionno + "&versionno=" + versionno + "&OrganisationId=" + OrganisationId + "&projecid=" + projecid + "&movementId=" + movementId + "&apprevid=" + apprevid + "&pageflag=" + pageflag + "&Owner=" + Owner).Replace('&', '%');
                    break;
                case "MoveVer":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "RouteVer":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "SpecOrder":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "Revisions":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                case "CandidateRT":
                    ViewBag.SORTStatus = SORTStatus;
                    break;
                default:
                    break;
            }

            //for Status
            if (SORTStatus != "CreateSO" && SORTStatus != "CreateVR1")
            {
                if (!VR1Applciation)
                {
                    ViewBag.SORTViewStatus = "SO";
                }
                else
                {
                    ViewBag.SORTViewStatus = "VR-1";
                }
            }


            ViewBag.msg = msg;


            if (OrgID == null && OrganisationId != 0)
            {
                Session["SORTOrgID"] = (int)OrganisationId;
            }
            else if (OrgID != null && OrganisationId == 0)
            {
                Session["SORTOrgID"] = (int)OrgID;
            }
            else if (OrgID == null && OrganisationId == 0)
            {
                Session["SORTOrgID"] = 0;
            }


            if (SORTStatus == "")
            {
                Session["RouteFlag"] = "2";
            }
            else
            {
                Session["RouteFlag"] = pageflag;
            }
            if (revisionId == 0)
            {
                ViewBag.OrgID = OrgID;
            }
            else
            {
                ViewBag.OrgID = OrganisationId;
                ViewBag.cloneapprevid = cloneapprevid;
                #region saving log action for revising application by SORT User
                if (cloneapprevid != 0)
                {
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.sort_desktop_revises_appl;
                    //common function for saving action logs
                    // long MovDescrip = GetDiscriptionToSave(movactiontype, hauliermnemonic, esdalref, arev_no);
                }
                #endregion
            }
            if (pageflag == 2)
            {
                SOApplication SORTAppSumObj = new SOApplication();
                // SOApplication SORTAppSumObj = ApplicationProvider.Instance.GetSOGeneralWorkinProcessbyrevisionid(SessionInfo.userSchema, apprevid, versionId, OrganisationId);
                ViewBag.Application_Status = SORTAppSumObj.ApplicationStatus;
                ViewBag.analysis_id = SORTAppSumObj.AnalysisId;
            }
            if (SORTStatus == "Revisions")
            {
                ViewBag.arev_no = arev_no;
                ViewBag.arev_Id = arev_Id;
                ViewBag.ver_no = ver_no;
            }
            //var soGeneralDetails = ApplicationProvider.Instance.GetSOApplicationTabDetails(hauliermnemonic, esdalref, revisionno, versionno, SessionInfo.userSchema);
            //  var project_status = ApplicationProvider.Instance.GetSOGeneralDetails(hauliermnemonic, esdalref, revisionno, versionno, SessionInfo.userSchema, OrganisationId);

            //ViewBag.Checking_Status = project_status.CheckingStatus_Code;
            //ViewBag.CheckerId = project_status.CheckerUserId;
            //ViewBag.PlannerUserId = project_status.PlannerUserId;
            //ViewBag.AppStatusCode = project_status.AppStatus_Code;
            //ViewBag.IsDistributed = project_status.MovIsDistributed;
            //ViewBag.LastSpecialOrderNo = project_status.LastSpecialOrderNo;
            //ViewBag.LatestCandidateRouteId = project_status.LastCandidateRouteId;
            //ViewBag.LatestCandidateRevisionId = project_status.LastRevisionId;
            //ViewBag.LatestCandRevisionNo = project_status.LastRevisionNo;
            //ViewBag.Vr1Number = project_status.Vr1Number;
            //ViewBag.RouteAnalysisId = project_status.RouteAnalysisId;
            //ViewBag.SONumber = project_status.SONumber;
            //ViewBag.analysis_id = project_status.AnalysisID;
            //ViewBag.HAJobFileRef = project_status.HA_Job_Reference;
            //ViewBag.EnteredBySort = project_status.Enteredbysort;
            //ViewBag.PreVerDistributed = project_status.PreVerDistributed;
            //ViewBag.DistributedMovAnalysisId = project_status.DistributedMovAnalysisId;

            //if (EditRev == false)
            //{
            //    if (SORTStatus != "CreateSO" && SORTStatus != "CreateVR1")
            //    {
            //        ViewBag.VersionStatus = soGeneralDetails.VersionStatus;
            //        ViewBag.Proj_Status = project_status.Proj_Status;
            //    }
            //}
            //if (SORTStatus == "MoveVer")
            //{
            //    string errormsg = string.Empty;
            //    ViewBag.esdal_history = esdal_history;
            //    string result = string.Empty;
            //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");

            //    result = STP.Common.General.XsltTransformer.Trafo(project_status.App_Notes_To_HA, path, out errormsg);
            //    ViewBag.App_Notes_To_HA = result;

            //    var Notification_Code = SORTApplicationProvider.Instance.GetSORTNotifiCode(revisionId); //revisionId
            //    ViewBag.Notification_Code = Notification_Code;
            //    ViewBag.MovLatestVer = LatestVer;
            //}


            ViewBag.hauliermnemonic = hauliermnemonic;
            ViewBag.esdalref = esdalref;
            ViewBag.revisionno = revisionno;
            // ViewBag.versionno = project_status.VersionNo;
            ViewBag.apprevid = apprevid;
            //  ViewBag.revisionId = project_status.LastRevisionId;
            ViewBag.movementId = movementId;
            ViewBag.OrganisationId = OrganisationId;
            //  ViewBag.versionId = project_status.VersionID;
            ViewBag.projecid = projecid;
            ViewBag.Owner = Owner;
            ViewBag.Checker = Checker;
            ViewBag.WorkStatus = WorkStatus;
            ViewBag.EditFlag = EditFlag;
            ViewBag.EnterBySORT = EnterBySORT;

            //if (soGeneralDetails.ApplicationrevId == 0)
            //{
            //    soGeneralDetails.ApplicationrevId = apprevid;
            //}
            //ViewBag.ApprevisionId = project_status.ApplicationrevId;

            ViewBag.EditRev = EditRev;


            ViewBag.VR1Applciation = VR1Applciation;
            if (pageflag == 3)
                ViewBag.VR1Applciation = true;
            if (apprevid == 0)
            {
                ViewBag.reduceddetailed = reduceddetailed;
            }
            else if (VR1Applciation)
            {

                //ApplyForVR1 applyForVR1 = ApplicationProvider.Instance.GetVR1General(SessionInfo.userSchema, apprevid);
                //ViewBag.analysis_id = applyForVR1.AnalysisId;
                //ViewBag.reduceddetailed = (applyForVR1.ReducedDetails == 0) ? true : false;
                //ViewBag.VR1ContentRef = applyForVR1.VR1ContentRef;
            }
            else
            {
                ViewBag.reduceddetailed = true;
            }
            Session["pageflag"] = pageflag;

            //code for checking special order no for agreed recleared movements //added by poonam 23-3-15
            string sonum = "";
            ViewBag.sonum = null;
            // sonum = ApplicationProvider.Instance.GetSONumberStatus((int)project_status.ProjectID, SessionInfo.userSchema);
            ViewBag.sonum = sonum;
            return PartialView("PartialView/_SORTHaulierDetails");
        }

        #region SORTMovementHistory
        public ActionResult SORTMovementHistory(int? page, int? pageSize, string Haul_num = "", int esdalref = 0, int versionno = 0, long projId = 0, int? sortOrder = null, int? sortType = null)
        {
            try
            {
                string messg = "SORTApplications/SORTMovementHistory?page=" + page + "pageSize=" + pageSize + "Haul_num=" + Haul_num + "esdalref=" + esdalref + "versionno=" + versionno;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                int maxlist_item = (int)SessionInfo.MaxListItem;
                int portalType = (int)SessionInfo.UserTypeId;
                sortOrder = sortOrder != null && sortOrder != 0 ? (int)sortOrder : 2; //date
                sortType = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = sortType;
                if (portalType != 696008)
                {
                    return RedirectToAction("Login", "Account");
                }

                //viewbag for pagination
                int pageNumber = (page ?? 1);
                if (Session["PageSize"] == null)
                {
                    Session["PageSize"] = maxlist_item;
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
                ViewBag.page = pageNumber;
                ViewBag.esdalref = esdalref;
                ViewBag.Haul_num = Haul_num;
                ViewBag.versionno = versionno;
                ViewBag.projId = projId;
                List<MovementHistory> objListDistAlerts = sortApplicationService.GetMovementHistory(pageNumber, (int)pageSize, Haul_num, esdalref, versionno, projId,sortOrder,sortType);

                int totalCount = 0;
                if (objListDistAlerts.Count > 0)
                {
                    totalCount = objListDistAlerts[0].TotalCount;
                }
                ViewBag.TotalCount = totalCount;
                var movementObjPagedList = new StaticPagedList<MovementHistory>(objListDistAlerts, pageNumber, (int)pageSize, totalCount);

                return PartialView("~/Views/SORTApplication/PartialView/SORTMovementHistory.cshtml", movementObjPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SORTMovementHistory, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        #region public ActionResult HaulierNotes()
        public ActionResult HaulierNotes(long VersionId)
        {
            try
            {
                string messg = "SORTApplications/HaulierNotes?VersionId=" + VersionId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                byte[] hnotes = sortApplicationService.GetMovHaulierNotes(VersionId, SessionInfo.UserSchema);
                string result = string.Empty;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                string errormsg = "";
                result = STP.Common.General.XsltTransformer.Trafo(hnotes, path, out errormsg);
                result = Regex.Replace(result, @"<(\/)?(html|body)([^>]*)>", "");
                ViewBag.NotesforHaulier = result;
                return PartialView("~/Views/SORTApplication/PartialView/HaulierNotes.cshtml");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/HaulierNotes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion public ActionResult HaulierNotes()

        #region public ActionResult SaveHaulierNotes()
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveHaulierNotes(long VersionId, string XmlHaulierNOtes)
        {
            try
            {
                XmlHaulierNOtes = string.Join(" ", Regex.Split(XmlHaulierNOtes, @"(?:\r\n|\n|\r)"));//to remove \n \r from string
                XmlHaulierNOtes = Regex.Replace(XmlHaulierNOtes, @"<(\/)?(html|body)([^>]*)>", "");

                //byte[] HaulrNotes = System.Convert.FromBase64String(XmlHaulierNOtes);
                //string strHaulierNotes = System.Text.Encoding.Default.GetString(HaulrNotes);
                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    //var bytes = System.Text.Encoding.UTF8.GetBytes(XmlHaulierNOtes);
                    ////getting the compressed blob value of xml for saving
                    bool result = false;
                    if (XmlHaulierNOtes != "" || XmlHaulierNOtes != null || XmlHaulierNOtes != string.Empty)
                    {
                        byte[] inbByteVar = Common.StringExtractor.StringExtractor.ZipAndBlob(XmlHaulierNOtes);
                        if (inbByteVar != null)
                        {
                            //Check if it is correct format.
                            string hnresult = string.Empty;
                            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                            string errormsg = "";
                            hnresult = STP.Common.General.XsltTransformer.Trafo(inbByteVar, path, out errormsg);
                            if (hnresult != "")
                            {
                                HaulierMovNotesInsertParams haulierMovNotesInsertParams = new HaulierMovNotesInsertParams
                                {
                                    MovementVersionID = VersionId,
                                    HaulierNote = inbByteVar,
                                    UserSchema = SessionInfo.UserSchema
                                };
                                result = sortApplicationService.SaveMovHaulierNotes(haulierMovNotesInsertParams);
                            }
                        }
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string messg = "SORTApplications/SaveHaulierNotes?VersionId=" + VersionId + "XmlHaulierNOtes=" + XmlHaulierNOtes;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, messg + ";");

                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/SaveHaulierNotes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion public ActionResult SaveHaulierNotes()

        #endregion
        public ActionResult ViewProposedReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0, string docType = "", int VersionId = 0)
        {
            try
            {
                string messg = "SORTApplications/ViewProposedReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string userType = "Police";  //by default userTyoe is Police

                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.UserTypeId == UserType.Sort)
                    userType = "Sort";


                if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
                {
                    OrgId = (int)Session["SORTOrgID"];
                    contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                }
                else if (SessionInfo.UserTypeId == UserType.PoliceALO)
                {
                    if (contactId == 0)
                    {
                        contactId = -1;
                    }
                    if (OrgId == 0)
                    {
                        OrgId = -1;
                    }
                }
                #region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                //Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                //DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(trans_id, OrgId, SessionInfo.UserSchema);

                //string xmlInformation = XmlOutboundDoc.XMLDocument;

                ////if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                //if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                //{
                //    userType = XmlOutboundDoc.UserType;
                //}
                #endregion

                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                MovementPrint moveprint = sortApplicationService.GetOrderNoProjectId(VersionId);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserTypePortalType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    UserType = UserTypePortalType,
                    Moveprint = moveprint
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("SORTApplications/ViewProposedReport, data is null {0}", esdalRefno));
                    return null;
                }
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;

                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ProposalForPolice.xslt";

                        //RM#4965
                        List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                        }

                    }
                    else
                    {
                        string[] esdalRefPro = esdalRefno.Split('/');
                        string haulierMnemonic = string.Empty;
                        string esdalrefnum = string.Empty;
                        int versionNo = 0;

                        if (esdalRefPro.Length > 0)

                        {
                            haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                            esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                            versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                        }

                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "Proposal", OrgId);

                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ProposalFaxSOA.xslt";

                        //RM#4965
                        List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }

                        }

                    }
                }

                if (notificationDocument != null)
                {
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ViewProposedReport, Exception: {0}", ex));
                throw ex;
            }
        }

        #region GenrateDocuments
        public ActionResult GenrateDocuments(string esdalRef, string SOnumber, Enums.SOTemplateType doctype)
        {
            try
            {
                string messg = "SORTApplications/GenrateDocuments?esdalRef=" + esdalRef + "SOnumber=" + SOnumber;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                bool generateFlag = false;
                byte[] docBuffer = sortDocumentService.GenrateSODocument(doctype, esdalRef, SOnumber, SessionInfo, generateFlag);//added generateFlag flag for generate document in order to solve redmine #4959
                if (docBuffer != null)
                {
                    MemoryStream docStream = new MemoryStream(docBuffer);
                    var docname = new FileStreamResult(docStream, "application/msword");
                    docname.FileDownloadName = SOnumber + "-" + doctype + ".doc";
                    return docname;
                }
                else
                {
                    return null; //TODO error handling
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/GenrateDocuments, Exception: {0}", ex));
                return Json(0);

            }
        }
        #endregion

        public string ViewSpecialOrderEmail(string xmlInformation, string xsltPath, int ContactId) //RM#4965
        {
            #region For Email View
            UserInfo SessionInfo = null;
            if (System.Web.HttpContext.Current.Session["UserInfo"] == null)
            {
                //return RedirectToAction("Login", "Account");
            }
            else
            {
                SessionInfo = (UserInfo)System.Web.HttpContext.Current.Session["UserInfo"];
            }
            string xmlinfo = xmlInformation;
            string userType = "";
            string documentType = "EMAIL";
            string organisationName = "";
            string HAReference = "";

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\"><Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bus##");
            xmlInformation = xmlInformation.Replace("</Underscore></Bold>", "##bue##");

            xmlInformation = xmlInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#bst#");
            xmlInformation = xmlInformation.Replace("</Bold>", "#be#");

            xmlInformation = xmlInformation.Replace("<Underscore xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##us##");
            xmlInformation = xmlInformation.Replace("<Underscore>", "##us##"); // RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Underscore>", "##ue##");
            xmlInformation = xmlInformation.Replace("<Italic xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##is##");
            xmlInformation = xmlInformation.Replace("<Italic>", "##is##");// RM#5232 changes
            xmlInformation = xmlInformation.Replace("</Italic>", "##ie##");

            xmlInformation = xmlInformation.Replace("<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bs####us##", "<NotesForHaulier xmlns=\"http://www.esdal.com/schemas/core/movement\">##bssbr####us##");

            xmlInformation = xmlInformation.Replace("<Underscore>", "");

            xmlInformation = xmlInformation.Replace("<OnBehalfOf xmlns=\"http://www.esdal.com/schemas/core/movement\"> </OnBehalfOf>", "");
            xmlInformation = xmlInformation.Replace("<OnBehalfOf> </OnBehalfOf>", "");

            xmlInformation = xmlInformation.Replace("<Para xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##ps##");
            xmlInformation = xmlInformation.Replace("</Para>", "##pe##");

            xmlInformation = xmlInformation.Replace("<BulletedText xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "##bts##");
            xmlInformation = xmlInformation.Replace("</BulletedText>", "##bte##");

            //Start For Bug #4319 </Br> tag
            xmlInformation = xmlInformation.Replace("<Br />", "##br##").Replace("<Br></Br>", "##br##");
            //End For Bug #4319 </Br> tag

            StringReader stringReader = new StringReader(xmlInformation);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltPath);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            XsltArgumentList argsList = new XsltArgumentList();

            argsList.AddParam("Contact_ID", "", (int)ContactId);

            if (SessionInfo.VehicleUnits == 692002 && userType == "Police" && xmlInformation.IndexOf("RouteImperial") != -1 && xmlInformation.IndexOf("OutboundNotification") != -1)
            {
                argsList.AddParam("UnitType", "", 692002);
            }
            else
            {
                argsList.AddParam("UnitType", "", SessionInfo.VehicleUnits);
            }

            argsList.AddParam("DocType", "", documentType);
            argsList.AddParam("OrganisationName", "", organisationName);
            argsList.AddParam("HAReferenceNumber", "", HAReference);

            xslt.Transform(xmlReader, argsList, writer, null);

            writer.Close();
            writer = null;

            string Attr = "id=\"hdr_img\"";
            string ImgFilePath = Attr + " src=\"" + System.Configuration.ConfigurationSettings.AppSettings["DocumentImagePath"].ToString() + "\"";

            string outputString = Convert.ToString(sw);

            outputString = outputString.Replace(Attr, ImgFilePath);

            outputString = outputString.Replace("##bus##", "<br/><br/><b><u>");
            outputString = outputString.Replace("##bue##", "</u></b> ");

            outputString = outputString.Replace("##bss##", "<b>");
            outputString = outputString.Replace("##bssbr##", "<b>");

            outputString = outputString.Replace("#bst#", "<b>");
            outputString = outputString.Replace("#be#", "</b>");

            outputString = outputString.Replace("##is##", "<i>");
            outputString = outputString.Replace("##ie##", "</i>");
            outputString = outputString.Replace("##us##", "<u>");
            outputString = outputString.Replace("##ue##", "</u> ");

            outputString = outputString.Replace("&amp;nbsp;", " ");

            outputString = outputString.Replace("##ps##", "<p>");
            outputString = outputString.Replace("##pe##", "</p>");

            outputString = outputString.Replace("##bts##", "<ul><li>");
            outputString = outputString.Replace("##bte##", "</li></ul>");

            //Start For Bug #4319 </Br> tag
            outputString = outputString.Replace("##br##", "<Br />");
            //End For Bug #4319 </Br> tag

            outputString = outputString.Replace("FACSIMILE MESSAGE", "Mail");

            outputString = outputString.Replace("<b />", "");
            outputString = outputString.Replace("#b#", "<b>");

            outputString = outputString.Replace("?", " ");

            int n = sortDocumentService.GetNoofPages(outputString);

            outputString = outputString.Replace("###Noofpages###", n.ToString());

            return outputString;

            #endregion


        }

        #region SORT Affected Parties


        #region public ActionResult AffectedParties(int NotificationId, int analysisId)
        public ActionResult AffectedParties(int NotificationId = 0, int analysisId = 0, int revisionId = 0, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int DistributedMovAnalysisId = 0)
        {
            try
            {
                string messg = "SORTApplications/AffectedParties?NotificationId=" + NotificationId + "analysisId=" + analysisId + "revisionId=" + revisionId + "IsCandidate=" + IsCandidate + "versionId=" + versionId + "IsVR1=" + IsVR1 + "DistributedMovAnalysisId=" + DistributedMovAnalysisId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                int orgId = 0;
                string path = null;
                orgId = (int)SessionInfo.OrganisationId;

                //Saving logged in users Name in viewbag for storing as a hidden variable
                ViewBag.FullName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                ViewBag.OrgName = SessionInfo.OrganisationName;

                RouteAssessmentModel objRouteAssessmentModel = null;

                //functionality for candidate route call
                if (IsCandidate == true)
                {

                    objRouteAssessmentModel = new RouteAssessmentModel();

                    #region Route Assessment Input
                    RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                    inputsForAssessment.AnalysisId = analysisId;

                    inputsForAssessment.IsCanditateRoute = IsCandidate;

                    inputsForAssessment.OrganisationId = orgId;

                    inputsForAssessment.RevisionId = revisionId;
                    #endregion

                    //fetching affected parties information 
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

                    if (objRouteAssessmentModel.AffectedParties == null)
                    {
                        int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, 7, SessionInfo.UserSchema);//To update affected parties for candidate route

                        if (DistributedMovAnalysisId != 0) // in case the affected parties are not generated and there exist a movement version for the movement.
                        {

                            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

                            bool tmpStatus = routeAssessmentService.RetainManualAddedAndCompareForAllVersions(analysisId, DistributedMovAnalysisId, inputsForAssessment, objRouteAssessmentModel.AffectedParties, SessionInfo.UserSchema);
                        }

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);
                    }
                    else
                    {

                        bool tmpStatus = routeAssessmentService.RetainManualAddedAndCompareForAllVersions(analysisId, DistributedMovAnalysisId, inputsForAssessment, objRouteAssessmentModel.AffectedParties, SessionInfo.UserSchema);

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

                        sortApplicationService.UpdateCandIsModified(analysisId);
                    }
                }
                else
                {

                    //fetching affected parties information 
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

                    if (objRouteAssessmentModel.AffectedParties == null)
                    {
                        //in case of revision calls that aren't from notification , and route is not a candidate 
                        if (revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, analysisId, 7, SessionInfo.UserSchema);
                        }
                        else
                        {
                            int updated = routeAssessmentService.updateRouteAssessment("", 0, revisionId, 0, orgId, analysisId, 7, SessionInfo.UserSchema);
                        }

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);
                    }
                    else
                    {
                        bool tmpResult = routeAssessmentService.SortAffectedPartyBasedOnOrganisation(objRouteAssessmentModel, analysisId, SessionInfo.UserSchema);

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);
                    }

                }
                if (IsVR1)
                {
                    ViewBag.IsVR1Sort = true;
                }
                else
                {
                    ViewBag.IsVR1Sort = false;
                }
                ViewBag.NotifId = NotificationId;
                ViewBag.AnalysisId = analysisId;
                return PartialView("AffectedParties");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/AffectedParties, Exception: {0}", ex.StackTrace));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion public ActionResult AffectedParties(int NotificationId, int analysisId)

        #region public ActionResult ExcludeAffectedParty(int contactId, int analysisId, int organisationId, string organisationName)

        public JsonResult ExcludeAffectedParty(int contactId, int analysisId, int organisationId, string organisationName)
        {
            try
            {
                string messg = "SORTApplications/ExcludeAffectedParty?contactId=" + contactId + "analysisId=" + analysisId + "organisationId=" + organisationId + "organisationName=" + organisationName;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                RouteAssessmentModel objRouteAssessmentModel;

                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

                //return View("ManualAddedParties", contactList);

                string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

                string newXmlAffectedParties = xmlAffectedPartyExcludeDeserializer(xmlAffectedParties, contactId, organisationId, organisationName);

                objRouteAssessmentModel.AffectedParties = Common.StringExtractor.StringExtractor.ZipAndBlob(newXmlAffectedParties);

                long status = routeAssessmentService.updateRouteAssessment(objRouteAssessmentModel, analysisId, SessionInfo.UserSchema);
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ExcludeAffectedParty, Exception: {0}", ex));
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }


        #region public static string xmlAffectedPartyDeserializer(string xml, int contactId)
        public static string xmlAffectedPartyExcludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure));

            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = deserializer.Deserialize(xmlReader);

            STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure XmlData = (STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure)obj;

            if (XmlData.ManualAffectedParties == null)
            {
                XmlData.ManualAffectedParties = new List<STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartyStructure>();
            }


            var generatedData = XmlData.GeneratedAffectedParties;

            (from s in generatedData
             where (s.Contact.Contact.simpleContactRef.ContactId == contactId && s.Contact.Contact.simpleContactRef.OrganisationId == organisationId && s.Contact.Contact.simpleContactRef.OrganisationName == organisationName)
             select s).ToList().ForEach(s => s.Exclude = true);

            XmlData.GeneratedAffectedParties = generatedData;

            string newXml = xmlAffectedPartySerializer(XmlData);
            xmlReader.Close();
            return newXml;
        }
        #endregion

        #region  xmlAffectedPartySerializer(AffectedPartiesStructure XmlData)
        /// <summary>
        ///Function to Serialize the Analysed constraint object to constraint xml
        /// </summary>
        public static string xmlAffectedPartySerializer(STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure XmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure));

            StringWriter outStream = new Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, XmlData);
            string str = outStream.ToString();

            return outStream.ToString(); // Output string 
        }
        #endregion

        #region Utf8StringWriter sub class to overide UTF-16 encoding to UTF-8 encoding
        /// <summary>
        /// class to generate UTF8 encoded xml
        /// </summary>
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
        #endregion

        #endregion public ActionResult ExcludeAffectedParty(int contactId, int analysisId, int organisationId, string organisationName)

        #region public ActionResult IncludeAffectedParty(int contactId, int analysisId, int organisationId, string organisationName)

        public JsonResult IncludeAffectedParty(int contactId, int analysisId, int organisationId, string organisationName)
        {
            try
            {
                string messg = "SORTApplications/IncludeAffectedParty?contactId=" + contactId + "analysisId=" + analysisId + "organisationId=" + organisationId + "organisationName=" + organisationName;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                RouteAssessmentModel objRouteAssessmentModel;

                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

                //return View("ManualAddedParties", contactList);

                string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

                string newXmlAffectedParties = xmlAffectedPartyIncludeDeserializer(xmlAffectedParties, contactId, organisationId, organisationName);

                objRouteAssessmentModel.AffectedParties = Common.StringExtractor.StringExtractor.ZipAndBlob(newXmlAffectedParties);

                long status = routeAssessmentService.updateRouteAssessment(objRouteAssessmentModel, analysisId, SessionInfo.UserSchema);
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/IncludeAffectedParty, Exception: {0}", ex));
                return Json(false);
            }
        }

        #region public static string xmlAffectedPartyIncludeDeserializer(string xml, int contactId)
        public static string xmlAffectedPartyIncludeDeserializer(string xml, int contactId, int organisationId, string organisationName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure));

            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = deserializer.Deserialize(xmlReader);

            STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure XmlData = (STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartiesStructure)obj;

            if (XmlData.ManualAffectedParties == null)
            {
                XmlData.ManualAffectedParties = new List<STP.Domain.RouteAssessment.XmlAffectedParties.AffectedPartyStructure>();
            }

            var generatedData = XmlData.GeneratedAffectedParties;

            (from s in generatedData
             where s.Contact.Contact.simpleContactRef.ContactId == contactId && s.Contact.Contact.simpleContactRef.OrganisationId == organisationId && s.Contact.Contact.simpleContactRef.OrganisationName == organisationName
             select s).ToList().ForEach(s => s.Exclude = false);

            XmlData.GeneratedAffectedParties = generatedData;

            string newXml = xmlAffectedPartySerializer(XmlData);
            xmlReader.Close();
            return newXml;
        }
        #endregion


        #endregion public ActionResult IncludeAffectedParty(int contactId, int analysisId, int organisationId, string organisationName)



        public ActionResult ListImportCandidateRouteVehicles(int revisionid, long CheckerId = 0, int CheckerStatus = 0, bool IsCandLastVersion = false, long planneruserId = 0, int appStatusCode = 0, string SONumber = "", int MovVersionNo = 0, decimal IsDistributed = 0)
        {
            try
            {
                string messg = "SORTApplications/ListImportCandidateRouteVehicles?revisionid=" + revisionid + ", CheckerId=" + CheckerId + ", CheckerStatus=" + CheckerStatus + ", IsCandLastVersion=" + IsCandLastVersion + ", planneruserId=" + planneruserId + ", appStatusCode=" + appStatusCode + ", SONumber=" + SONumber + ", MovVersionNo=" + MovVersionNo + ", IsDistributed=" + IsDistributed;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                ViewBag.SORTView = false;
                ViewBag.IsNotif = true;
                ViewBag.ApplRevId = true;
                ViewBag.update = false;
                ViewBag.ContentRefNo = 0;
                ViewBag.MovementId = MovVersionNo;
                Session["movementClassificationId"] = 270006;
                Session["movementClassificationName"] = "Special order";

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];

                bool iseditcandidate = false;
                if (IsCandLastVersion)
                    iseditcandidate = CheckingProcess.EditCandidate(SessionInfo.SortUserId, planneruserId, CheckerId, appStatusCode, CheckerStatus);
                ViewBag.IsCandidateModify = iseditcandidate;
                //if (System.Diagnostics.Debugger.IsAttached)
                //{
                //    ViewBag.IsCandidateModify = true;
                //}

                List<AppVehicleConfigList> vehicleconfigurationlist = new List<AppVehicleConfigList>();
                ViewBag.routecount = 0;


                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();


                ImportedRoutelist = sortApplicationService.CandRouteList(revisionid, SessionInfo.UserSchema);
                ViewBag.routecount = ImportedRoutelist.Count;
                vehicleconfigurationlist = sortApplicationService.CandidateRouteVehicleConfiguration(revisionid, SessionInfo.UserSchema);

                ViewBag.RoutePartsType = new SelectList(ImportedRoutelist, "routeID", "routetype");
                ViewBag.RouteParts = new SelectList(ImportedRoutelist, "routeID", "routeName");
                return PartialView("~/Views/Application/ListImportedVehicleConfiguration.cshtml", vehicleconfigurationlist);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ListImportCandidateRouteVehicles, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion
        /// <summary>
        /// Create Special Order : GET
        /// </summary>
        /// <param name="sedalno"></param>
        /// <param name="ProjectId"></param>
        /// <param name="VersionId"></param>
        /// <param name="SONumber"></param>
        /// <param name="PlannerId"></param>
        /// <param name="ProjectStatus"></param>
        /// <returns></returns>
        public ActionResult CreateSpecialOrder(CreateSpecialOrderCntrlModel createSpecialOrderCntrlModel)
        {
            try
            {
                string messg = "SORTApplications/CreateSpecialOrder?sedalno=" + createSpecialOrderCntrlModel.sedalno + "ProjectId=" + createSpecialOrderCntrlModel.ProjectId + "VersionId=" + createSpecialOrderCntrlModel.VersionId + "SONumber=" + createSpecialOrderCntrlModel.SONumber + "PlannerId=" + createSpecialOrderCntrlModel.PlannerId + "ProjectStatus=" + createSpecialOrderCntrlModel.ProjectStatus;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (Session["UserInfo"] != null)
                {
                    //Get Priveilage sort users
                    List<GetSORTUserList> GridListObj = null;
                    UserInfo SessionInfo = null;
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    GridListObj = sortDocumentService.ListSortUser(SessionInfo.UserTypeId, 1);
                    SelectList GridListObjInfo = new SelectList(GridListObj, "UserName", "UserName");
                    ViewBag.SORTUserListInfo = GridListObjInfo;

                    STP.Domain.Applications.SORTSpecialOrder spOrder = new SORTSpecialOrder();
                    spOrder.ESDALNo = createSpecialOrderCntrlModel.sedalno;
                    spOrder.ProjectID = createSpecialOrderCntrlModel.ProjectId;
                    spOrder.VersionID = createSpecialOrderCntrlModel.VersionId;
                    spOrder.SOCreateDate = DateTime.Now.Date.ToShortDateString();

                    if (createSpecialOrderCntrlModel.SONumber != null)
                    {
                        spOrder = new SORTSpecialOrder();
                        spOrder = sortDocumentService.GetSpecialOrder(createSpecialOrderCntrlModel.SONumber);
                        spOrder.SOCreateDate = spOrder.SOCreateDate.Replace('/', '-');
                        DateTime soCreateDate = DateTime.ParseExact(spOrder.SOCreateDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        spOrder.SOCreateDate = soCreateDate.ToString("dd/MM/yyyy");
                        spOrder.SignDate = spOrder.SignDate.Replace('/', '-');
                        DateTime signDate = DateTime.ParseExact(spOrder.SignDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        spOrder.SignDate = signDate.ToString("dd/MM/yyyy");
                        spOrder.ExpiryDate = spOrder.ExpiryDate.Replace('/', '-');
                        DateTime expiryDate = DateTime.ParseExact(spOrder.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        spOrder.ExpiryDate = expiryDate.ToString("dd/MM/yyyy");
                        ViewBag.iseditflag = 1;
                    }
                    List<SOCRouteParts> routvehicles = sortDocumentService.GetRouteVehicles(createSpecialOrderCntrlModel.VersionId, null);
                    List<SOCoverageDetails> lstSoCoverageDetails = sortDocumentService.GetSpecialOrderCoverages(createSpecialOrderCntrlModel.ProjectId, 264003);

                    var result = routvehicles.GroupBy(c => c.RoutePartId).ToList();
                    ViewBag.RouteCoverageModel = result;
                    List<SOCRoutePartsModel> lstrvmodel = new List<SOCRoutePartsModel>();
                    List<SOCRouteParts> lstRouteVehicles = new List<SOCRouteParts>();

                    foreach (var rResults in result)
                    {
                        SOCRoutePartsModel rvmodel = new SOCRoutePartsModel();

                        rvmodel.RoutePartId = rResults.Select(c => c.RoutePartId).FirstOrDefault();
                        rvmodel.RPName = rResults.Select(c => c.RPName).FirstOrDefault();
                        int vcount = 0;
                        foreach (var vResult in rResults)
                        {
                            vcount++;
                            SOCRouteVehicle vModel = new SOCRouteVehicle();

                            vModel.VehicleId = vResult.VehicleId;
                            vModel.RVName = vResult.RVName;
                            string containts = rvmodel.RoutePartId.ToString() + "," + vModel.VehicleId.ToString();
                            if (lstSoCoverageDetails.Any(c => c.Applicability.Contains(containts)))
                            {

                                vModel.Include = true;
                                if (spOrder.Applicability != null)
                                {
                                    if (spOrder.Applicability.Contains(rvmodel.RoutePartId + "," + vModel.VehicleId))
                                        vModel.isExist = true;
                                    else
                                    {
                                        vModel.isExist = false;
                                        var excovrg = lstSoCoverageDetails.FirstOrDefault(c => c.Applicability.Contains(containts));
                                        vModel.OrderNo = excovrg.OrderNo;
                                    }
                                }
                            }
                            else
                            {
                                vModel.Include = false;
                            }

                            rvmodel.Vehicles.Add(vModel);
                            SOCRouteParts _rvmodel = new SOCRouteParts()
                            {
                                RoutePartId = rvmodel.RoutePartId,
                                RPName = rvmodel.RPName,
                                VehicleId = vModel.VehicleId,
                                RVName = vModel.RVName,
                                Orderno = vModel.OrderNo
                            };
                            if (_rvmodel.RVName.Length > 27)
                                _rvmodel.RVName = _rvmodel.RVName.Substring(0, 27);
                            lstRouteVehicles.Add(_rvmodel);
                        }
                        rvmodel.VCount = vcount;
                        lstrvmodel.Add(rvmodel);
                    }
                    ViewBag.RoutesVehicles = lstRouteVehicles;
                    ViewBag.RVCoverage = lstrvmodel;
                    spOrder.PlannerID = createSpecialOrderCntrlModel.PlannerId;
                    spOrder.ProjectStatus = createSpecialOrderCntrlModel.ProjectStatus;
                    //Get special order counts.                
                    var specialorders = sortDocumentService.GetSpecialOrderList(createSpecialOrderCntrlModel.ProjectId);
                    spOrder.SpecialOrderCount = specialorders.Count;
                    spOrder.State = "work in progress";
                    //var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                    //var workflowEsdalReferenceNumber = createSpecialOrderCntrlModel.EsdalReferenceWorkflow;
                    //if (sortSOApplicationManagement.CheckIfProcessExit(workflowEsdalReferenceNumber) && WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_GenerateSpecialOrderNumber, out dynamic workflowPayload) != string.Empty)
                    //{
                    //    dynamic dataPayload = new ExpandoObject();
                    //    dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Ap_Activity_GenerateSpecialOrderNumber.ToString());
                    //    WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                    //    {
                    //        data = dataPayload,
                    //        workflowData = workflowPayload
                    //    };
                    //    sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    //}
                    ViewBag.EsdalReferenceWorkflow = createSpecialOrderCntrlModel.EsdalReferenceWorkflow;
                    return View(spOrder);

                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CreateSpecialOrder, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Create Special Order : POST
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isEdit"></param>
        /// <param name="RemovedCoverages"></param>
        /// <param name="Statevalue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSpecialOrder(SORTSpecialOrder model, bool isEdit, List<string> RemovedCoverages, int Statevalue, string esdalReferenceWorkflow = "")
        {
            try
            {
                string messg = "SORTApplications/CreateSpecialOrder?isEdit=" + isEdit + "Statevalue=" + Statevalue;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                var sortSOApplicationManage = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManage.GetSortAppPayload();
                int projectId = (int)sortPayload.ProjectId;
                int revisionNo = sortPayload.LastRevisionNo;
                int versionno = sortPayload.LastVersionNo;
                if (Session["UserInfo"] != null)
                {
                    UserInfo UserSessionValue = null;
                    List<GetSORTUserList> GridListObj = null;
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    GridListObj = sortDocumentService.ListSortUser(UserSessionValue.UserTypeId, 1);
                    SelectList GridListObjInfo = new SelectList(GridListObj, "UserName", "UserName");
                    ViewBag.SORTUserListInfo = GridListObjInfo;

                    if (isEdit)
                        model.State = Statevalue.ToString();
                    if (ModelState.IsValid)
                    {
                        if (model.Applicability != null)
                        {
                            model.Applicability = model.Applicability.Replace('#', ',');
                            model.CoverageStatus = 1;
                        }
                        else
                            model.CoverageStatus = 0;
                        if (!isEdit)
                        {
                            model.Year = DateTime.Now.Date.Year;

                            string input = Regex.Replace(model.ESDALNo, "[^0-9]+", string.Empty);
                            Random rnd = new Random(int.Parse(input));

                            //Save new special order.

                            string sonumber = sortDocumentService.SaveSortSpecialOrder(model, RemovedCoverages);
                            if (sonumber != "")
                            {
                                model.SONumber = sonumber;
                                //Movement action of create special order
                                SpecialOrderMovementActions(MovementnActionType.user_creates_special_order, sonumber, model.ESDALNo, versionno, projectId, revisionNo, UserSessionValue);
                                //Movement action of amends special order.
                                if (model.SpecialOrderCount > 0)
                                    SpecialOrderMovementActions(MovementnActionType.user_amends_special_order, sonumber, model.ESDALNo, versionno, projectId, revisionNo, UserSessionValue);
                                model.ESDALNo = null;
                                ModelState.Clear();
                                if (sonumber != "" && sonumber != null)
                                {
                                    var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                                    var workflowEsdalReferenceNumber = esdalReferenceWorkflow;
                                    if (sortSOApplicationManagement.CheckIfProcessExit(workflowEsdalReferenceNumber) && WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_GenerateSpecialOrderNumber, out dynamic workflowPayload) != string.Empty)
                                    {
                                        dynamic dataPayload = new ExpandoObject();
                                        dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Ap_Activity_GenerateSpecialOrderNumber.ToString());
                                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                                        {
                                            data = dataPayload,
                                            workflowData = workflowPayload
                                        };
                                        sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                                    }
                                    return RedirectToAction("ShowSpecialOrder", new
                                    {
                                        B7vy6imTleYsMr6Nlv7VQ =
                                            STP.Web.Helpers.EncryptionUtility.Encrypt("SpecialOrderId=" + sonumber +
                                            "&VersionId=" + model.VersionID +
                                            "&projectId=" + model.ProjectID +
                                            "&ProjectStatus=" + model.ProjectStatus +
                                            "&Projstatus=" + model.ProjectStatus +
                                            "&Plannerid=" + model.PlannerID)
                                    });
                                }
                            }
                        }
                        else
                        {

                            sortDocumentService.UpdateSortSpecialOrder(model, RemovedCoverages);
                            if (Statevalue == 264002)
                                SpecialOrderMovementActions(MovementnActionType.user_revokes_special_order, model.SONumber, model.ESDALNo, versionno, projectId, revisionNo, UserSessionValue);
                            return RedirectToAction("ShowSpecialOrder", new
                            {
                                B7vy6imTleYsMr6Nlv7VQ =
                                    STP.Web.Helpers.EncryptionUtility.Encrypt("SpecialOrderId=" + model.SONumber +
                                    "&VersionId=" + model.VersionID +
                                    "&projectId=" + model.ProjectID +
                                    "&ProjectStatus=" + model.ProjectStatus +
                                    "&Projstatus=" + model.ProjectStatus +
                                    "&Plannerid=" + model.PlannerID)
                            });
                        }
                        //Movement action of revoke special order.
                        if (Statevalue == 264002)
                            SpecialOrderMovementActions(MovementnActionType.user_revokes_special_order, model.SONumber, model.ESDALNo, versionno, projectId, revisionNo, UserSessionValue);
                        ViewBag.SOStatus = 1;
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ModelStatus = false;
                        return View(model);
                    }
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CreateSpecialOrder, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        //Delete special order
        public ActionResult DeleteSpecialOrder(string OrderNo, string EsdalRef)
        {
            try
            {
                string messg = "SORTApplications/DeleteSpecialOrder?OrderNo=" + OrderNo + "EsdalRef=" + EsdalRef;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                UserInfo usersessioninfo = null;
                usersessioninfo = (UserInfo)Session["UserInfo"];
                try
                {
                    bool result = sortDocumentService.Deletespecialorder(OrderNo, usersessioninfo.UserSchema);
                    if (result == true)
                        SpecialOrderMovementActions(MovementnActionType.user_deletes_special_order, OrderNo, EsdalRef,0, 0,0,usersessioninfo);
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/DeleteSpecialOrder, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        /// <summary>
        /// SuggestTemplateSpecialOrder
        /// </summary>
        /// <param name="Vehicle_IDs"></param>
        /// <returns></returns>
        public JsonResult SuggestTemplateSpecialOrder(string[] Vehicle_IDs)
        {
            try
            {
                string messg = "SORTApplications/SuggestTemplateSpecialOrder";
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string result = "";
                string vehicleID = "";
                if (Vehicle_IDs != null)
                {
                    for (int i = 0; i < Vehicle_IDs.Length; i++)
                    {
                        vehicleID += Vehicle_IDs[i] + ',';
                    }
                    vehicleID = vehicleID.TrimEnd(',');
                    List<SOCRouteParts> routvehicles = sortDocumentService.GetRouteVehicles(0, vehicleID);
                    result = routvehicles[0].RecomTemplate;
                }
                else
                {
                    result = "0";
                }
                return Json(new { Success = result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/SuggestTemplateSpecialOrder, Exception: {0}", ex));
                return Json(0);

            }
        }

        public ActionResult ShowSpecialOrder(string SpecialOrderId, int VersionId, int projectId, String hauliermnemonic = "", int esdalref = 0, int versionno = 0, string ProjectStatus = "", int CheckingStatus = 0, int Plannerid = 0, int Checkerid = 0, int Projstatus = 0)
        {
            try
            {
                string messg = "SORTApplications/ShowSpecialOrder?SpecialOrderId=" + SpecialOrderId + "VersionId=" + VersionId + "projectId=" + projectId + "hauliermnemonic=" + hauliermnemonic + "esdalref=" + esdalref + "versionno=" + versionno + "ProjectStatus=" + ProjectStatus + "CheckingStatus=" + CheckingStatus + "Plannerid=" + Plannerid;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (Session["SortListMUrl"] != null)
                    ViewBag.CurrentUrl = Session["SortListMUrl"];
                ViewBag.SOId = SpecialOrderId;

                ViewBag.hauliermnemonic = hauliermnemonic;
                ViewBag.esdalref = esdalref;
                ViewBag.versionno = versionno;

                ViewBag.ProjectStatus = ProjectStatus;
                ViewBag.CheckingStatus = CheckingStatus;
                ViewBag.PlannerId = Plannerid;
                ViewBag.Checkerid = Checkerid;
                ViewBag.VersionId = VersionId;
                ViewBag.Projstatus = Projstatus;
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.SortUserId = SessionInfo.SortUserId;
                if (Session["UserInfo"] != null)
                {
                    STP.Domain.Applications.SORTSpecialOrder spOrder = new SORTSpecialOrder();

                    spOrder.VersionID = VersionId;

                    spOrder = sortDocumentService.GetSpecialOrder(SpecialOrderId);

                    //DateTime createdate = DateTime.ParseExact(spOrder.SOCreateDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //spOrder.SOCreateDate= createdate.ToString("dd-MMM-yyyy");

                    //DateTime signDate = DateTime.ParseExact(spOrder.SignDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //spOrder.SignDate = signDate.ToString("dd-MMM-yyyy");

                    //DateTime expiryDate = DateTime.ParseExact(spOrder.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //spOrder.ExpiryDate = expiryDate.ToString("dd-MMM-yyyy");                    

                    spOrder.State = ToUpperFirstLetter(spOrder.State);

                    List<SOCRouteParts> routvehicles = sortDocumentService.GetRouteVehicles(VersionId, null);
                    List<SOCoverageDetails> lstSoCoverageDetails = sortDocumentService.GetSpecialOrderCoverages(projectId, 0);

                    var result = routvehicles.GroupBy(c => c.RoutePartId).ToList();
                    ViewBag.RouteCoverageModel = result;
                    List<SOCRoutePartsModel> lstrvmodel = new List<SOCRoutePartsModel>();
                    List<SOCRouteParts> lstRouteVehicles = new List<SOCRouteParts>();

                    foreach (var rResults in result)
                    {
                        SOCRoutePartsModel rvmodel = new SOCRoutePartsModel();

                        rvmodel.RoutePartId = rResults.Select(c => c.RoutePartId).FirstOrDefault();
                        rvmodel.RPName = rResults.Select(c => c.RPName).FirstOrDefault();
                        int vcount = 0;
                        foreach (var vResult in rResults)
                        {
                            vcount++;
                            SOCRouteVehicle vModel = new SOCRouteVehicle();

                            vModel.VehicleId = vResult.VehicleId;
                            vModel.RVName = vResult.RVName;
                            string containts = rvmodel.RoutePartId.ToString() + "," + vModel.VehicleId.ToString();
                            if (lstSoCoverageDetails.Any(c => c.Applicability.Contains(containts)))
                            {
                                vModel.Include = true;
                                if (spOrder.Applicability != null)
                                {
                                    if (spOrder.Applicability.Contains(rvmodel.RoutePartId + "," + vModel.VehicleId))
                                        vModel.isExist = true;
                                    else
                                        vModel.isExist = false;
                                }
                            }
                            else
                            {
                                vModel.Include = false;
                            }

                            rvmodel.Vehicles.Add(vModel);
                            SOCRouteParts _rvmodel = new SOCRouteParts()
                            {
                                RoutePartId = rvmodel.RoutePartId,
                                RPName = rvmodel.RPName,
                                VehicleId = vModel.VehicleId,
                                RVName = vModel.RVName
                            };
                            if (_rvmodel.RVName.Length > 27)
                                _rvmodel.RVName = _rvmodel.RVName.Substring(0, 27);
                            lstRouteVehicles.Add(_rvmodel);
                        }
                        rvmodel.VCount = vcount;
                        lstrvmodel.Add(rvmodel);
                    }
                    ViewBag.RoutesVehicles = lstRouteVehicles;
                    ViewBag.RVCoverage = lstrvmodel;

                    return View(spOrder);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ShowSpecialOrder, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #region DistributionComments
        public ActionResult CheckRouteAsseBeforeDistributing(int AnalysisId = 0)
        {
            try
            {
                string messg = "SORTApplications/CheckRouteAsseBeforeDistributing?AnalysisId=" + AnalysisId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];
                //Check affected parties generated.
                long movRevisionId = 0;
                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(AnalysisId, 9, SessionInfo.UserSchema);

                if (objRouteAssessmentModel.DriveInst == null)
                {
                    movRevisionId = 2;
                }
                else if (objRouteAssessmentModel.AffectedStructure == null)
                {
                    movRevisionId = 3;
                }
                else if (objRouteAssessmentModel.RouteDescription == null)
                {
                    movRevisionId = 7;
                }
                else if (objRouteAssessmentModel.AffectedParties == null)
                {
                    movRevisionId = 8;
                }
                else if (objRouteAssessmentModel.AffectedRoads == null)
                {
                    movRevisionId = 9;
                }
                else
                {
                    movRevisionId = 1;
                }

                return Json(movRevisionId);
            }

            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CheckRouteAsseBeforeDistributing, Exception: {0}", ex));
                return Json(null);
            }
        }
        public ActionResult DistributionCommentsPopUp(string EsdalRefNumber = "", string Comments = "", int VersionId = 0)
        {
            try
            {
                string messg = "SORTApplications/DistributionCommentsPopUp?EsdalRefNumber=" + EsdalRefNumber + "Comments=" + Comments + "VersionId=" + VersionId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                DistributionComments distribComments = new DistributionComments { esdalReference = EsdalRefNumber, text = Comments };
                ViewBag.VersionId = VersionId;
                return PartialView(distribComments);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/DistributionCommentsPopUp, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult SaveDistributionComments(DistributionComments distributionComments, SaveDistributionCommentsCntrlModel saveDistributionCommentsCntrlModel)
        {
            try
            {
                int status = 0;
                string messg = "SORTApplications/SaveDistributionComments?EsdalReference=" + saveDistributionCommentsCntrlModel.EsdalReference + "HaulierMnemonic=" + saveDistributionCommentsCntrlModel.HaulierMnemonic + "EsdalRef=" + saveDistributionCommentsCntrlModel.EsdalRef + "VersionNo=" + saveDistributionCommentsCntrlModel.VersionNo + "VersionId=" + saveDistributionCommentsCntrlModel.VersionId + "HaJobFileRef=" + saveDistributionCommentsCntrlModel.HaJobFileRef + "ProjectStatus=" + saveDistributionCommentsCntrlModel.ProjectStatus + "PreVersionDistr=" + saveDistributionCommentsCntrlModel.PreVersionDistr;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                DistributionComments distribComments = new DistributionComments { esdalReference = distributionComments.esdalReference == null ? saveDistributionCommentsCntrlModel.EsdalReference : distributionComments.esdalReference, text = distributionComments.text };

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                saveDistributionCommentsCntrlModel.EsdalReference = saveDistributionCommentsCntrlModel.EsdalReference.Replace("~", "#");
                saveDistributionCommentsCntrlModel.EsdalReference = saveDistributionCommentsCntrlModel.EsdalReference.Replace("#", "/");
                string[] esdalRefPro = saveDistributionCommentsCntrlModel.EsdalReference.Split('/');
                string mnemonic = string.Empty;
                string esdalrefnum = string.Empty;
                string version = string.Empty;
                // TODO todo
                if (esdalRefPro.Length > 0)
                {
                    mnemonic = Convert.ToString(esdalRefPro[0]);
                    esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    version = Convert.ToString(esdalRefPro[2].ToUpper().Replace("S", ""));
                }

                // getting the affected structures for a esdal ref number and haulier mnemonic
                byte[] affectedStructures = sortApplicationService.GetAffectedStructures(0, esdalrefnum, saveDistributionCommentsCntrlModel.HaulierMnemonic, version, SessionInfo.UserSchema);

                Dictionary<int, int> icaStatusDictionary = null;

                if (affectedStructures != null)
                {
                    string xmlaffectedStructures = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedStructures));
                    icaStatusDictionary = sortApplicationService.GetNotifICAstatus(xmlaffectedStructures);
                    CommonNotifMethods commonNotif = new CommonNotifMethods();
                    AffectedStructConstrParam affectedParam = new AffectedStructConstrParam
                    {

                        AffectedSections = commonNotif.GetAffectedStructList(affectedStructures),
                        NotificationId = saveDistributionCommentsCntrlModel.VersionId
                    };
                    bool update = false;
                    if (affectedParam.AffectedSections.Count > 0)
                        update = sortApplicationService.SaveAffectedMovementDetails(affectedParam);

                }

                HAContact haContactDetailObj = applicationService.GetHAContactDetails(SessionInfo.ContactId);

                AggreedRouteXSD.MovementVersionNumberStructure movdetails = new AggreedRouteXSD.MovementVersionNumberStructure
                {
                    CreatedBySort = false,
                    Value = short.Parse(saveDistributionCommentsCntrlModel.VersionNo.ToString())
                };
                AggreedRouteXSD.ESDALReferenceNumberStructure esdal = new AggreedRouteXSD.ESDALReferenceNumberStructure
                {
                    MovementProjectNumber = saveDistributionCommentsCntrlModel.EsdalRef,
                    Mnemonic = saveDistributionCommentsCntrlModel.HaulierMnemonic,
                    MovementVersion = movdetails
                };
                AgreedRouteStructure agreedroute = new AgreedRouteStructure
                {
                    DistributionComments = distribComments.text,
                    JobFileReference = saveDistributionCommentsCntrlModel.HaJobFileRef,
                    SentDateTime = DateTime.Now,
                    ESDALReferenceNumber = esdal
                };

                //GETTING ORDER NO AND PROJECT ID
                MovementPrint moveprint = sortApplicationService.GetOrderNoProjectId(saveDistributionCommentsCntrlModel.VersionId);
                if (moveprint.OrderNumber == "0")
                    moveprint.OrderNumber = "";

                //to delete from quick links                
                sortApplicationService.Deletequicklinks(moveprint.ProjectId);
                var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManagement.GetSortAppPayload();
                SODistributionParams sODistributionParams = new SODistributionParams
                {
                    EsdalReferenceNum = esdalrefnum,
                    DistribComments = distribComments.text,
                    Versionid = saveDistributionCommentsCntrlModel.VersionId,
                    IcaStatusDictionary = icaStatusDictionary,
                    EsdalReference = saveDistributionCommentsCntrlModel.EsdalReference,
                    HaContact = haContactDetailObj,
                    AgreedRoute = agreedroute,
                    ProjectStatus = saveDistributionCommentsCntrlModel.ProjectStatus,
                    VersionNo = saveDistributionCommentsCntrlModel.VersionNo,
                    Moveprint = moveprint,
                    PreVersionDistr = saveDistributionCommentsCntrlModel.PreVersionDistr,
                    ProjectId = saveDistributionCommentsCntrlModel.ProjectId,
                    LastRevNo = saveDistributionCommentsCntrlModel.lastrevisionno,
                    SessionInfo = SessionInfo
                };

                documentService.DistributeSOMovement(sODistributionParams, ref status);

                //Save movement action after route agreed
                //Project is in 'work in progress' status.
                if (status == 1 && (saveDistributionCommentsCntrlModel.ProjectStatus == 307002))
                {
                    var workflowEsdalReferenceNumber = WorkflowTaskFinder.GenerateEsdalReferenceNumber(saveDistributionCommentsCntrlModel.HaulierMnemonic, saveDistributionCommentsCntrlModel.EsdalRef.ToString());
                    if (sortSOApplicationManagement.CheckIfProcessExit(workflowEsdalReferenceNumber) && WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_DistributeMovement, out dynamic workflowPayload) != string.Empty)
                    {
                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.workflowActivityLog = sortSOApplicationManagement.SetWorkflowLog(WorkflowActivityTypes.Ap_Activity_DistributeMovement.ToString());
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        sortSOApplicationManagement.ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    }
                }
                return Json(new { result = status });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SaveDistributionComments, Exception: {0}", ex));
                throw ex;
            }
        }

        #endregion
        public ActionResult ShowCandidateRoutes(int routerevision_id = 0, long CheckerId = 0, int CheckerStatus = 0, bool IsCandLastVersion = false, long planneruserId = 0, int appStatusCode = 0, string SONumber = "")
        {
            try
            {
                string messg = "SORTApplications/ShowCandidateRoutes?routerevision_id=" + routerevision_id + ", CheckerId=" + CheckerId + ", CheckerStatus=" + CheckerStatus + ", IsCandLastVersion=" + IsCandLastVersion + ", planneruserId=" + planneruserId + ", appStatusCode=" + appStatusCode + ", SONumber=" + SONumber;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                bool iseditcandidate = false;
                if (IsCandLastVersion)
                    iseditcandidate = CheckingProcess.EditCandidate(SessionInfo.SortUserId, planneruserId, CheckerId, appStatusCode, CheckerStatus);
                ViewBag.IsCandidateModify = iseditcandidate;

                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();

                if (routerevision_id != 0)//This is for Authorized movemens
                {
                    ImportedRoutelist = sortApplicationService.CandRouteList(routerevision_id, SessionInfo.UserSchema);
                }

                ViewBag.AuthoMov_CONT_REF = "0";
                Session["RouteFlag"] = "4";
                ViewBag.AgreedSO = 0;
                ViewBag.AuthoMov_VersionID = "0";
                try
                {
                    List<CRDetails> routedetails = sortApplicationService.GetRouteType(routerevision_id, SessionInfo.UserSchema);
                    foreach (var routes in ImportedRoutelist)
                    {
                        routes.RouteType = (routedetails.Where(c => c.RoutePartID == routes.RouteID).SingleOrDefault().SegmentNumber == 1) ? "Planned" : "Outline";
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ShowCandidateRoutes, Exception: {0}", ex));

                }
                ViewBag.MovementRouteList = ImportedRoutelist;
                ViewBag.IsCandidateRoute = true;
                ViewBag.VehicleList = new List<AppVehicleConfigList>(); //setting null session;

                ViewBag.NotificationEditFlag = 0;
                ViewBag.IsNotif = false;
                if (CheckerStatus == 305004 || CheckerStatus == 305005 || CheckerStatus == 305006) //added checker status for restricting the route edit on agreed mov
                {
                    ViewBag.NotificationEditFlag = 1;
                }

                return PartialView("~/Views/Routes/MovementRoute.cshtml");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ShowCandidateRoutes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
        }
        public ActionResult SetRouteDetails(AppRouteList route, bool isCandidateModify = false, bool isCandidateRoute = false)
        {
            Session["RouteAssessmentFlag"] = "";
            ViewBag.IsCandidateModify = isCandidateModify;
            if (isCandidateModify == false || isCandidateRoute)
            {
                Session["RouteAssessmentFlag"] = "Completed";
            }

            return PartialView("PartialView/_SortRouteDetails", route);
        }
        public ActionResult SetCandiRouteDetails(AppRouteList route)
        {
            return PartialView("PartialView/_SortCandiRouteDetails", route);
        }
        public ActionResult SortRoutes(long pageflag = 1, long CheckerId = 0, int CheckerStatus = 0, bool IsCandLastVersion = false, long planneruserId = 0, int appStatusCode = 0, int MovVersionNo = 0, decimal IsDistributed = 0, string SONumber = "")
        {
            try
            {
                string messg = "SORTApplications/SortRoutes?pageflag=" + pageflag + ", CheckerId=" + CheckerId + ", CheckerStatus=" + CheckerStatus + ", IsCandLastVersion=" + IsCandLastVersion + ", planneruserId=" + planneruserId + ", appStatusCode=" + appStatusCode + ", MovVersionNo=" + MovVersionNo + ", IsDistributed=" + IsDistributed + ", SONumber=" + SONumber;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                    SessionInfo = (UserInfo)Session["UserInfo"];



                ViewBag.AppStatus = appStatusCode;
                Session["RouteFlag"] = "4";
                Session["pageflag"] = pageflag;

                if (IsCandLastVersion == true)
                {
                    SortActions sactions = new Common.SortHelper.SortActions();
                    sactions = CheckingProcess.SortSOActions(sactions, SessionInfo.SortUserId, planneruserId, CheckerId, SessionInfo.SORTCanAgreeAllSO, appStatusCode, CheckerStatus, IsDistributed, 11, MovVersionNo, 0);
                    ViewBag.NewVersion = sactions.CreateCandidateVersion;
                    ViewBag.IsCandidateRTCreate = sactions.EditCandidateVersion;
                    ViewBag.SendforChecking = sactions.SendforChecking;
                    ViewBag.CompleteChecking = sactions.CompleteChecking;
                    ViewBag.MovementCreation = sactions.CreateMovementVersion;
                    ViewBag.Agreed = sactions.Agree;
                    ViewBag.SendForQAChecking = sactions.SendForQAChecking;
                    ViewBag.CompleteQAChecking = sactions.CompleteQAChecking;
                    ViewBag.SendforSignoff = sactions.SendForSignOff;
                    ViewBag.SignOff = sactions.SignOff;
                    ViewBag.CreateRevisedApplication = sactions.CreateRevisedApplication;
                    return PartialView("~/Views/Application/SoRoute.cshtml");
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SortRoutes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
        }
        public string ToUpperFirstLetter(string source)
        {
            try
            {
                string messg = "SORTApplications/ToUpperFirstLetter?source=" + source;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
                if (string.IsNullOrEmpty(source))
                    return string.Empty;
                char[] letters = source.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                return new string(letters);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ToUpperFirstLetter, Exception: {0}", ex));
                return null;
            }
        }
        public ActionResult MoveSummaryLeftPanel(bool Mov_btn_show = false, string ProjctStatus = "", int CheckingStatus = 0, int PlannerId = 0, bool VR1APP = false, int Enter_BY_SORT = 0)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                    SessionInfo = (UserInfo)Session["UserInfo"];
                else
                    return RedirectToAction("Login", "Account");

                string messg = "SORTApplications/MoveSummaryLeftPanel?Mov_btn_show=" + Mov_btn_show + ", ProjctStatus=" + ProjctStatus + ", CheckingStatus=" + CheckingStatus + ", PlannerId=" + PlannerId + ", VR1APP=" + VR1APP;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                ViewBag.Mov_btn_show = Mov_btn_show;
                ViewBag.PrjStatus = ProjctStatus;
                ViewBag.CheckingStatus = CheckingStatus;
                ViewBag.PlannerId = PlannerId;
                ViewBag.VR1APP = VR1APP;
                ViewBag.Enter_BY_SORT = Enter_BY_SORT;

                return PartialView("~/Views/SORTApplication/PartialView/MoveSummaryLeftPanel.cshtml");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/MoveSummaryLeftPanel, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult ViewAgreedReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0, string docType = "", int VersionId = 0)
        {
            try
            {
                string messg = "SORTApplications/ViewAgreedReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string userType = "Police"; //by default userTyoe is Police

                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.UserTypeId == UserType.Sort)
                    userType = "Sort";
                if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
                {
                    OrgId = (int)Session["SORTOrgID"];
                    contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                }
                else if (SessionInfo.UserTypeId == UserType.PoliceALO)
                {
                    if (contactId == 0)
                    {
                        contactId = -1;
                    }
                    if (OrgId == 0)
                    {
                        OrgId = -1;
                    }
                }

                //#region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                ////Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                //DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(trans_id, OrgId, SessionInfo.UserSchema);

                //string xmlInformation = XmlOutboundDoc.XMLDocument;

                ////if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                //if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                //{
                //    userType = XmlOutboundDoc.UserType;
                //}
                //#endregion

                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                MovementPrint moveprint = sortApplicationService.GetOrderNoProjectId(VersionId);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserTypePortalType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    UserType = UserTypePortalType,
                    Moveprint = moveprint
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("SORTApplications/ViewAgreedReport, data is null {0}", esdalRefno));
                    return null;
                }
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;
                List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);
                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                        }


                    }
                    else
                    {
                        //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                        xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "agreed", OrgId);

                        xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\SpecialOrderAgreedRoute.xslt";
                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                            }
                        }

                    }
                }

                if (notificationDocument != null)
                {

                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");

                    //using (MemoryStream stream = new System.IO.MemoryStream())
                    //{
                    //    StringReader sr = new StringReader(notificationDocument);
                    //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    //    pdfDoc.Open();
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    //    pdfDoc.Close();
                    //    return File(stream.ToArray(), "application/pdf");
                    //}


                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }


            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/ViewAgreedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        public ActionResult ViewReProposedReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0, string docType = "", int VersionId = 0)
        {
            try
            {
                string messg = "SORTApplications/ViewReProposedReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string userType = "Police";  //by default userTyoe is Police
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.UserTypeId == UserType.Sort)
                    userType = "Sort";
                if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
                {
                    OrgId = (int)Session["SORTOrgID"];
                    contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                }
                else if (SessionInfo.UserTypeId == UserType.PoliceALO)
                {
                    if (contactId == 0)
                    {
                        contactId = -1;
                    }
                    if (OrgId == 0)
                    {
                        OrgId = -1;
                    }
                }

                #region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                ////Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                //DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(trans_id, OrgId, SessionInfo.UserSchema);

                //string xmlInformation = XmlOutboundDoc.XMLDocument;

                ////if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                //if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                //{
                //    userType = XmlOutboundDoc.UserType;
                //}
                #endregion

                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                int versionNo = 0;
                string[] esdalRefPro = esdalRefno.Split('/');
                string haulierMnemonic = string.Empty;
                string esdalrefnum = string.Empty;
                if (esdalRefPro.Length > 0)
                {
                    haulierMnemonic = Convert.ToString(esdalRefPro[0]);
                    esdalrefnum = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    versionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                }
                MovementPrint moveprint = sortApplicationService.GetOrderNoProjectId(VersionId);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserTypePortalType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    UserType = UserTypePortalType,
                    Moveprint = moveprint,
                    VersionNo = versionNo
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("SORTApplications/ViewReProposedReport, data is null {0}", esdalRefno));
                    return null;
                }
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;
                TransmittingDocumentDetails transmittingDetail = new TransmittingDocumentDetails();
                transmittingDetail = documentService.SortSideCheckDoctype(trans_id, SessionInfo.UserSchema);
                List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);
                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        if (transmittingDetail.DocumentType == "no longer affected")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalNoLongerFAXPolice.xslt";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalstillaffected_Fax_Police.xslt";
                        }

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                        }


                    }
                    else
                    {
                        if (transmittingDetail.DocumentType == "no longer affected")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                        }
                        else
                        {
                            //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                            xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "Proposal", OrgId);
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalStillAffectedFax.xslt";
                        }
                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                            }
                        }


                    }
                }

                if (notificationDocument != null)
                {
                    //notificationDocument = notificationDocument.Replace("<newdate>", "");
                    //notificationDocument = notificationDocument.Replace("</newdate>", "");
                    //using (MemoryStream stream = new System.IO.MemoryStream())
                    //{
                    //    StringReader sr = new StringReader(notificationDocument);
                    //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    //    pdfDoc.Open();
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    //    pdfDoc.Close();
                    //    return File(stream.ToArray(), "application/pdf");
                    //}
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/ViewReProposedReport, Exception: {0}", ex));
                throw ex;
            }
        }
        public ActionResult ViewAmendmentToAgreementReport(string esdalRefno = "", int trans_id = 0, int OrgId = 0, int contactId = 0, int flagpoint = 0, string docType = "", int VersionId = 0)
        {
            try
            {
                string messg = "SORTApplications/ViewAmendmentToAgreementReport?esdalRefno=" + esdalRefno + "trans_id=" + trans_id + "OrgId=" + OrgId + "contactId=" + contactId;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                string userType = "Police"; //by default userTyoe is Police
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                if (SessionInfo.UserTypeId == UserType.Sort)
                    userType = "Sort";
                if (SessionInfo.UserTypeId == UserType.Sort && OrgId == 0 && contactId == 0)
                {
                    OrgId = (int)Session["SORTOrgID"];
                    contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                }
                else if (SessionInfo.UserTypeId == UserType.PoliceALO)
                {
                    if (contactId == 0)
                    {
                        contactId = -1;
                    }
                    if (OrgId == 0)
                    {
                        OrgId = -1;
                    }
                }

                //#region Function to fetch outbound document/ user type based on input parameters Trans_id and OrgId
                ////Function Call to fetch xml document from outbound document for trans_id aka document id and usertype based on organisation id
                //DocumentInfo XmlOutboundDoc = movementsService.ViewMovementDocument(trans_id, OrgId, SessionInfo.UserSchema);

                //string xmlInformation = XmlOutboundDoc.XMLDocument;

                ////if OrgId is 0 then by default police is selected else based on the output from stored procedure the usertype is considered.
                //if (OrgId != 0 && XmlOutboundDoc.UserType != String.Empty)
                //{
                //    userType = XmlOutboundDoc.UserType;
                //}
                //#endregion
                //>>>>>>>>>>>>>>>>>>>>>>BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                MovementPrint moveprint = sortApplicationService.GetOrderNoProjectId(VersionId);
                var ItemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(docType.ToLower());
                var NotificationType = userType.ToLower() == "police" ? NotificationXSD.NotificationTypeType.police : NotificationXSD.NotificationTypeType.soa;
                var UserTypePortalType = userType.ToLower() == "police" ? Enums.PortalType.POLICE : Enums.PortalType.SOA;
                var documentParams = new SOProposalDocumentParams()
                {
                    ContactId = contactId,
                    EsdalReferenceNo = esdalRefno,
                    SessionInfo = SessionInfo,
                    OrganisationId = Convert.ToInt32(SessionInfo.OrganisationId),
                    ItemTypeStatus = ItemTypeStatus,
                    NotificationType = NotificationType,
                    UserSchema = SessionInfo.UserSchema,
                    UserType = UserTypePortalType,
                    Moveprint = moveprint
                };
                var model = movementsService.GetXmlDataForPrint(documentParams);
                if (model == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.WARNING, string.Format("SORTApplications/ViewAmendmentToAgreementReport, data is null {0}", esdalRefno));
                    return null;
                }
                string xmlInformation = model.ReturnXML;
                //>>>>>>>>>>>>>>>>>>>>>>>>>> END BLOB REMOVAL>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                if (xmlInformation != string.Empty)
                {
                    xmlInformation = xmlInformation.Replace(">?<", ">\u2002<");
                    xmlInformation = xmlInformation.Replace(">?##**##", ">##**##");
                }

                byte[] notificationDocument = null;
                string xsltPath = string.Empty;
                //RM#4965
                List<TransmissionModel> transmissionList = sortDocumentService.GetTransmissionType(trans_id, "310001,310009,310008,310002,310007,310003,310005", 7, UserSchema.Portal);
                TransmittingDocumentDetails transmittingDetail = new TransmittingDocumentDetails();
                transmittingDetail = documentService.SortSideCheckDoctype(trans_id, SessionInfo.UserSchema);
                if (xmlInformation != string.Empty)
                {
                    if (userType == "Police")
                    {
                        if (transmittingDetail.DocumentType == "no longer affected")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\ReProposalNoLongerFAXPolice.xslt";
                        }
                        else
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\RevisedAgreementFaxPolice.xslt";
                        }

                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "PDF", SessionInfo);
                            }
                        }


                    }
                    else
                    {
                        if (transmittingDetail.DocumentType == "no longer affected")
                        {
                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\Re_Proposal_No_Longer_FAX_SOA.xsl";
                        }
                        else
                        {
                            //STP.Document.GenerateDocument genDocument = new Document.GenerateDocument();
                            xmlInformation = documentService.GetLoggedInUserAffectedStructureDetailsByESDALReference(xmlInformation, esdalRefno, SessionInfo, UserSchema.Sort, "agreed", OrgId);

                            xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + "XSLT\\RevisedAgreement.xslt";
                        }
                        if (transmissionList.Any() && !string.IsNullOrEmpty(transmissionList[0].Fax))
                        {
                            notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                        }
                        else
                        {
                            if (flagpoint == 1)
                            {
                                ViewBag.HtmlStr = ViewSpecialOrderEmail(xmlInformation, xsltPath, contactId); //RM#4965                            
                                return View("../DistributionStatus/EmailTransmissionView");
                            }
                            else
                            {
                                notificationDocument = documentService.GeneratePDF(0, 0, xmlInformation, xsltPath, "", 0, contactId, "", false, "", "", SessionInfo.VehicleUnits, "MovementPDF", SessionInfo);
                            }
                        }

                    }
                }

                if (notificationDocument != null)
                {
                    ////System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);
                    //notificationDocument = notificationDocument.Replace("<newdate>", "");
                    //notificationDocument = notificationDocument.Replace("</newdate>", "");
                    ////return new FileStreamResult(pdfStream, "application/pdf");
                    //using (MemoryStream stream = new System.IO.MemoryStream())
                    //{
                    //    StringReader sr = new StringReader(notificationDocument);
                    //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    //    pdfDoc.Open();
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    //    pdfDoc.Close();
                    //    return File(stream.ToArray(), "application/pdf");
                    //}
                    System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(notificationDocument);

                    return new FileStreamResult(pdfStream, "application/pdf");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data is not valid for current movement therefore PDF document will not be generated.');</script>");
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/ViewAmendmentToAgreementReport, Exception: {0}", ex));
                throw ex;
            }
        }
        public ActionResult RelatedMovements(long app_Id = 0, string type = " ")
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SOApplicationRelatedMov soAppRelaMov = sortApplicationService.GetRelatedMovement(app_Id, type);
                return Json(new { result = soAppRelaMov });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }
        public ActionResult CandiVersionVehicleList(int revisionid, long CheckerId = 0, int CheckerStatus = 0, bool IsCandLastVersion = false, long planneruserId = 0, int appStatusCode = 0, string SONumber = "", int MovVersionNo = 0, decimal IsDistributed = 0, char VRLIST_type = 'C', bool IsIsCreateApplication = false)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.IsIsCreateApplication = IsIsCreateApplication;
                ViewBag.VRLIST_type = VRLIST_type;
                List<AppVehicleConfigList> vehicleconfigurationlist = new List<AppVehicleConfigList>();
                vehicleconfigurationlist = sortApplicationService.CandidateRouteVehicleConfiguration(revisionid, SessionInfo.UserSchema, VRLIST_type);

                return PartialView("CandiVersionVehicleList", vehicleconfigurationlist);
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ShowCandidateRoutes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
        }
        public ActionResult CandiVersionRoutesList(int routerevision_id = 0, long CheckerId = 0, int CheckerStatus = 0, bool IsCandLastVersion = false, long planneruserId = 0, int appStatusCode = 0, string SONumber = "", char RList_type = 'C', bool IsIsCreateApplication = false, bool IsRelaStruMov = false)
        {
            try
            {
                string messg = "SORTApplications/CandiVersionRoutesList?routerevision_id=" + routerevision_id + ", CheckerId=" + CheckerId + ", CheckerStatus=" + CheckerStatus + ", IsCandLastVersion=" + IsCandLastVersion + ", planneruserId=" + planneruserId + ", appStatusCode=" + appStatusCode + ", SONumber=" + SONumber;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                bool iseditcandidate = false;
                if (IsCandLastVersion)
                    iseditcandidate = CheckingProcess.EditCandidate(SessionInfo.SortUserId, planneruserId, CheckerId, appStatusCode, CheckerStatus);
                ViewBag.IsCandidateModify = iseditcandidate;
                ViewBag.IsRelaStruMov = IsRelaStruMov;
                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();

                if (routerevision_id != 0)//This is for Authorized movemens
                {
                    ImportedRoutelist = sortApplicationService.CandRouteList(routerevision_id, SessionInfo.UserSchema, RList_type);
                }

                //ViewBag.AuthoMov_RevisionID = "0";
                ViewBag.routerevision_id = routerevision_id;
                if (Session["RouteFlag"] != null && Session["RouteFlag"].ToString() != "2")
                { Session["RouteFlag"] = "4"; }
                ViewBag.AgreedSO = 0;
                ViewBag.AuthoMov_VersionID = "0";
                ViewBag.IsIsCreateApplication = IsIsCreateApplication;
                try
                {
                    List<CRDetails> routedetails = sortApplicationService.GetRouteType(routerevision_id, SessionInfo.UserSchema);
                    foreach (var routes in ImportedRoutelist)
                    {
                        routes.RouteType = (routedetails.Where(c => c.RoutePartID == routes.RouteID).SingleOrDefault().SegmentNumber == 1) ? "planned" : "Outline";
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ShowCandidateRoutes, Exception: {0}", ex));
                }
                return PartialView("CandiVersionRoutesList", ImportedRoutelist);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/ShowCandidateRoutes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
        }
        public JsonResult ImportRouret(int routepartId, int AppRevId, string routeType, int versionid = 0, string contentref = "")
        {
            try
            {
                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                {
                    return Json(0);
                }
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                long result = 0;

                if (routeType == "planned")
                    result = applicationService.IMP_CondidateRoue(routepartId, AppRevId, versionid, contentref, SessionInfo.UserSchema);
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/SaveRouteInAppParts, Exception: {0}", ex));
                return Json(0);

            }
        }
        public ActionResult GetPreviousMovemntList(long projectId = 0, bool isVehicleImport = false, string movmntType = "")
        {
            ViewBag.IsVehicleImport = isVehicleImport;
            List<SORTMovementList> SORTApplRevisionsobj = sortApplicationService.GetHaulierAppRevision(projectId).OrderBy(a => a.RevisionNo).ToList();
            List<SORTMovementList> SORTAppMovementVersionobj = sortApplicationService.GetMovmentVersion(projectId).OrderBy(m => m.VersionNo).ToList();
            List<CandidateRTModel> SORTAppCandRTDetails = sortApplicationService.GetCandidateRTDetails(projectId);
            List<CandidateRT> lstCandRt = new List<CandidateRT>();

            var CandRTdetails = SORTAppCandRTDetails.OrderBy(c => c.RouteID).GroupBy(c => c.RouteID).ToList();
            foreach (var candrts in CandRTdetails)
            {
                CandidateRT model = new CandidateRT
                {
                    RouteId = candrts.Select(c => c.RouteID).FirstOrDefault(),
                    Name = candrts.Select(c => c.Name).FirstOrDefault(),
                    AnalysisId = candrts.Select(c => c.AnalysisID).FirstOrDefault(),
                    CandidateDate = candrts.Select(c => c.CandidateDate).FirstOrDefault(),
                };

                foreach (var revisions in candrts)
                {
                    CRVersion revision = new CRVersion
                    {
                        ReviosionId = revisions.RevisionID,
                        RevisionNo = revisions.RevisionNo,
                    };
                    model.Versions.Add(revision);
                }
                ViewBag.LastRevision = candrts.Max(c => c.RevisionID);
                ViewBag.LastVersion = candrts.Max(c => c.RevisionNo);
                ViewBag.LastCandRouteId = model.RouteId;
                model.Versions = model.Versions.OrderBy(c => c.RevisionNo).ToList();
                lstCandRt.Add(model);
            }
            ViewBag.ApplicationList = SORTApplRevisionsobj;
            ViewBag.MovemenList = SORTAppMovementVersionobj;
            ViewBag.CandidateList = lstCandRt;
            ViewBag.MovementType = movmntType;
            return PartialView("PartialView/_MovementImportVersions");
        }

        #region Private Methods
        private void SpecialOrderMovementActions(MovementnActionType type, string specialorderno = null, string esdalrefno = null,int versionNo=0,int projectid=0,int revisionno=0, UserInfo usersessioninfo = null)
        {
            try
            {
                
                string messg = "SORTApplications/SpecialOrderMovementActions?specialorderno=" + specialorderno + "esdalrefno=" + esdalrefno;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.MovementActionType = type;
                string ErrMsg = string.Empty;
                movactiontype.SpecialOrderNo = specialorderno;
                movactiontype.ContactName = usersessioninfo.FirstName + " " + usersessioninfo.LastName;
                movactiontype.ESDALRef = esdalrefno;

                string MovementDescription = MovementActions.GetMovementActionString(usersessioninfo, movactiontype, out ErrMsg);
                long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, projectid, revisionno, versionNo, usersessioninfo.UserSchema);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/SpecialOrderMovementActions, Exception: {0}", ex));
            }
        }

        //Movement actionsfor checking
        private void CheckingMovementAction(int cstatus, int candversion, string checkername, int mov_ver_no, string esdalref,int projectId,int revisionNo,int versionno,  UserInfo SessionInfo)
        {
            try
            {
                
                string messg = "SORTApplications/CheckingMovementAction?cstatus=" + cstatus + ", candversion=" + candversion + ", checkername=" + checkername + ", mov_ver_no=" + mov_ver_no;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (cstatus != 0 && SessionInfo != null)
                {
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    string ErrMsg = string.Empty;
                    movactiontype.CandVerNo = candversion;
                    movactiontype.ESDALRef = esdalref;
                    string MovementDescription = "";
                    switch (cstatus)
                    {
                        case 301002://For checking
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_sends_candroutever_for_check;
                            movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.CheckerName = checkername;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            break;
                        case 301003://For checked positive
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_candver_from_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = checkername;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription += " The candidate route version has passed checking.";
                            break;
                        case 301004://For checked negative
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_candver_from_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = checkername;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription += " The candidate route version has failed checking.";
                            break;
                        case 301008://For QA checking
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_sends_mov_ver_for_QA_check;
                            movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.CheckerName = checkername;
                            movactiontype.MovementVer = mov_ver_no;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            break;
                        case 301009://For QA checked positive ". The movement version has passed checking."
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_mov_ver_from_QA_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = checkername;
                            movactiontype.MovementVer = mov_ver_no;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription += " The movement version has passed QA checking.";
                            break;
                        case 301010://For QA checked negative ". The movement version has failed checking."
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_mov_ver_from_QA_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = checkername;
                            movactiontype.MovementVer = mov_ver_no;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription += " The movement version has failed QA checking.";
                            break;
                        case 301005://For final checking
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_sends_mov_ver_for_final_check;
                            movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.CheckerName = checkername;
                            movactiontype.MovementVer = mov_ver_no;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            break;
                        case 301006://For final checked positive
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_mov_ver_from_final_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = checkername;
                            movactiontype.MovementVer = mov_ver_no;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription += " The movement version has passed final checking.";
                            break;
                        case 301007://For final checked negative
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_mov_ver_from_final_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = checkername;
                            movactiontype.MovementVer = mov_ver_no;
                            MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription += " The movement version has failed final checking.";
                            break;
                    }
                    if (movactiontype.MovementActionType != null)
                    {
                        long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, projectId,revisionNo, versionno, SessionInfo.UserSchema);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplications/CheckingMovementAction, Exception: {0}", ex));
            }
        }
        private void Vr1MovementAction(int cstatus, string checkername, int mov_ver_no, string esdalref, string OwnerName,int projectId,int revisionno,int versionno, UserInfo SessionInfo)
        {
            try
            {
                
                string messg = "SORTApplications/Vr1MovementAction?cstatus=" + cstatus + "checkername=" + checkername + "mov_ver_no=" + mov_ver_no + "esdalref=" + esdalref + "OwnerName=" + OwnerName;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                if (cstatus != 0 && SessionInfo != null)
                {
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    string ErrMsg = string.Empty;
                    movactiontype.MovementVer = mov_ver_no;
                    movactiontype.ESDALRef = esdalref;
                    string MovementDescription = "";
                    switch (cstatus)
                    {
                        case 301002://For checking
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_sends_candroutever_for_check;
                            movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.CheckerName = checkername;
                            //MovementDescription = STP.Domain.Movements.MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription = "SORT user '" + movactiontype.ContactName + "' sent movement version '" + movactiontype.MovementVer + "' for checking to '" + movactiontype.CheckerName + "'.";
                            break;
                        case 301003://For checked positive
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_candver_from_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = OwnerName;
                            //MovementDescription = STP.Domain.Movements.MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription = "SORT user '" + movactiontype.CheckerName + "' has returned movement version '" + movactiontype.MovementVer + "' from checking to '" + movactiontype.AllocateUser + "'.";
                            MovementDescription += " The movement version has passed checking.";
                            break;
                        case 301004://For checked negative
                            movactiontype.MovementActionType = MovementnActionType.sort_desk_returns_candver_from_check;
                            movactiontype.CheckerName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                            movactiontype.AllocateUser = OwnerName;
                            //MovementDescription = STP.Domain.Movements.MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                            MovementDescription = "SORT user '" + movactiontype.CheckerName + "' has returned movement version '" + movactiontype.MovementVer + "' from checking to '" + movactiontype.AllocateUser + "'.";
                            MovementDescription += " The movement version has failed checking.";
                            break;
                    }
                    if (movactiontype.MovementActionType != null)
                    {
                        long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, projectId, revisionno, versionno, SessionInfo.UserSchema);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/Vr1MovementAction, Exception: {0}", ex));
                throw ex;
            }
        }
        #endregion

        public ActionResult GetPreviousMovementVehicleList(long revisionId, int listType, int flag)
        {
            List<AppVehicleConfigList> vehicleList = vehicleconfigService.GetSortMovementVehicle(revisionId, listType);
            ViewBag.VehicleList = vehicleList;
            ViewBag.Flag = flag;
            if (vehicleList.Count==0)
            {
                return Json(new { flag = true });
            }
            else
            {
                return PartialView("PartialView/_MovementRouteVehicle");
            }
        }
        public ActionResult GetPreviousMovementRouteList(long revisionId, int listType)
        {
            List<AppRouteList> routeList = routeService.GetSortMovementRoute(revisionId, listType);
            ViewBag.RouteList = routeList;
            return PartialView("PartialView/_MovementRouteVehicle");
        }
        
        #region Commented Code by Mahzeer
        /*
        private int GenerateSOProposalDocument(string esdalReferenceNum, string distribComments, int versionid, Dictionary<int, int> icaStatusDictionary, string EsdalReference, HAContact hacontact, AgreedRouteStructure agreedroute, long ProjectStatus = 0, int versionNo = 0, MovementPrint moveprint = null, decimal PreVersionDistr = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int docType;
            string docFileName = string.Empty;
            SODistributeDocumentParams sODistribute;
            sODistribute = documentService.GenerateSODistributeDocument(esdalReferenceNum, (int)SessionInfo.OrganisationId, 0, distribComments, versionid, icaStatusDictionary, EsdalReference, hacontact, agreedroute, SessionInfo.UserSchema, 692001, ProjectStatus, versionNo, moveprint, PreVersionDistr, SessionInfo);

            ProjectStatus = sODistribute.ProjectStatus;
            List<ContactModel> contactList = sODistribute.ContactList;
            int icaStatus = 277001;
            docType = sODistribute.DocType;
            docFileName = sODistribute.DocFileName;
            XMLModel model = null;
            if (sODistribute.XmlModel.ReturnXML != null)
            {
                model = sODistribute.XmlModel;
            }
            else if (sODistribute.ModelStillAfftdSOA.ReturnXML != null)
            {
                model = sODistribute.ModelStillAfftdSOA;
            }
            string xsltPath = sODistribute.XsltPath;

            SOProposalXsltPath proposalXsltPath ;
            #region
            foreach(ContactModel objcontact in contactList)
            {
                if (objcontact.Organisation != null && objcontact.FullName != null && objcontact.Organisation != string.Empty && objcontact.FullName != string.Empty)
                {
                    string[] contactDet = new string[6];
                    if (objcontact.ContactId != 0)
                    {
                        //function that returns contact's details in a string array
                        contactDet = documentService.FetchContactPreference(objcontact.ContactId, UserSchema.Portal);

                        objcontact.OrganisationId = objcontact.OrganisationId == 0 ? Convert.ToInt32(contactDet[4]) : objcontact.OrganisationId; // saving organisation Id into the NotificationContacts object

                        if (icaStatusDictionary != null)
                        {
                            if (icaStatusDictionary.ContainsKey(objcontact.OrganisationId))
                            {
                                icaStatus = icaStatusDictionary[objcontact.OrganisationId];
                            }
                        }
                    }
                    else // condition to fetch manually added parties and send police document
                    {
                        contactDet[3] = "police";// send the police document to any manually added party

                        contactDet[0] = (objcontact.Email != null && objcontact.Email != "") ? "Email" : "Online Inbox Only";

                        contactDet[1] = objcontact.Email;

                        contactDet[2] = objcontact.Fax;
                    }

                    string latestReason = objcontact.Reason;
                    string[] stringSeparators = new string[] { "##**##" };
                    string finalReson = "";

                    try
                    {
                        if (latestReason != null && latestReason != string.Empty && latestReason.IndexOf("##**##") != -1)
                        {
                            string[] reasonArray = latestReason.Split(stringSeparators, StringSplitOptions.None);

                            if (reasonArray.Length > 1)
                            {
                                finalReson = reasonArray[1];
                            }
                        }
                        else if (latestReason != null && latestReason != string.Empty && latestReason.IndexOf("##**##") == -1)
                        {
                            finalReson = latestReason;
                        }
                    }
                    catch
                    {
                        // do nothing
                    }

                    if (contactDet[3] == "soa")
                    {
                        #region check for reason
                        switch (ProjectStatus)
                        {
                            case (int)Common.Enums.ProjectStatus.reproposed: //307004
                                {
                                    if (finalReson == "no longer affected") //no longer affected fax soa
                                    {
                                        docFileName = "ReProposalNolongerAffectedFaxSOA";
                                        docType = (int)ItemDocType.doc007;// 322007
                                        model = sODistribute.ModelNoLongAfftdSOA;
                                    }
                                    else //still affected fax soa
                                    {
                                        docFileName = "ReProposalStillAffectedFaxSOA";
                                        docType = (int)ItemDocType.doc001;//322001
                                        model = sODistribute.ModelStillAfftdSOA;
                                    }
                                    break;
                                }
                            case (int)Common.Enums.ProjectStatus.agreed: //307005
                                {
                                    docFileName = "AgreedRouteSOA";
                                    docType = (int)ItemDocType.doc010;//322010
                                    model = sODistribute.ModelSOA;
                                    break;
                                }
                            case (int)Common.Enums.ProjectStatus.agreed_revised: //307006
                                {
                                    if (finalReson == "no longer affected")//no longer affected fax soa
                                    {
                                        docFileName = "ReProposalNolongerAffectedFaxSOA";
                                        docType = (int)ItemDocType.doc007;//322007
                                        model = sODistribute.ModelNoLongAfftdSOA;
                                    }
                                    else //soa
                                    {
                                        docFileName = "RevisedAgreement";
                                        docType = (int)ItemDocType.doc002;//322002
                                        model = sODistribute.ModelSOA;
                                    }
                                    break;
                                }
                            case (int)Common.Enums.ProjectStatus.agreed_recleared: //307007 //soa
                                {
                                    docFileName = "ReclearedAgreementFaxSOA";
                                    docType = (int)ItemDocType.doc002;//322002
                                    model = sODistribute.ModelSOA;
                                    break;
                                }
                        }
                        proposalXsltPath = documentService.GetSoProposalXsltPath(contactDet[3], ProjectStatus, finalReson);
                        if (proposalXsltPath.XSLTPath != "")
                        {
                            xsltPath = proposalXsltPath.XSLTPath;
                        }
                        #endregion
                    }

                    else if (contactDet[3] == "police")
                    {
                        #region check for reason
                        switch (ProjectStatus)
                        {
                            case (int)Common.Enums.ProjectStatus.reproposed: //307004
                                {
                                    if (finalReson == "no longer affected") //no longer affected fax police
                                    {
                                        docFileName = "ReProposalNolongerAffectedFaxPolice";
                                        docType = (int)ItemDocType.doc007;// 322007
                                        model = sODistribute.ModelNoLongAfftdSOA;
                                    }
                                    else //finalReson == "still affected" || finalReson == "newly affected" || finalReson == "affected by change of route"//still affected fax police
                                    {
                                        docFileName = "ReProposalStillAffectedFaxPolice";
                                        docType = (int)ItemDocType.doc001;//322001
                                        model = sODistribute.ModelStillAfftdSOA;
                                    }
                                    break;
                                }
                            case (int)Common.Enums.ProjectStatus.agreed: //307005
                                {
                                    docFileName = "AgreedRouteSOA";
                                    docType = (int)ItemDocType.doc010;//322010
                                    model = sODistribute.ModelPolice;
                                    break;
                                }
                            case (int)Common.Enums.ProjectStatus.agreed_revised: //307006
                                {
                                    if (finalReson == "no longer affected")//no longer affected fax soa
                                    {
                                        docFileName = "ReProposalNolongerAffectedFaxSOA";
                                        docType = (int)ItemDocType.doc007;//322007
                                        model = sODistribute.ModelNoLongAfftdSOA;
                                    }
                                    else //soa
                                    {
                                        docFileName = "RevisedAgreementPolice";
                                        docType = (int)ItemDocType.doc002;//322002
                                        model = sODistribute.ModelPolice;
                                    }
                                    break;
                                }
                            case (int)Common.Enums.ProjectStatus.agreed_recleared: //307007 //soa
                                {
                                    docFileName = "ReclearedAgreementFaxPolice";
                                    docType = (int)ItemDocType.doc002;//322002
                                    model = sODistribute.ModelPolice;
                                    break;
                                }
                        }
                        proposalXsltPath = documentService.GetSoProposalXsltPath(contactDet[3], ProjectStatus, finalReson);
                        if (proposalXsltPath.XSLTPath != "")
                        {
                            xsltPath = proposalXsltPath.XSLTPath;
                        }
                        #endregion
                    }
                    switch (contactDet[0])
                    {
                        case "Email":
                        case "Online inbox plus email (HTML)":
                            {
                                objcontact.Email = contactDet[1];
                                GenerateEmailParams emailParams = new GenerateEmailParams
                                {
                                    NotificationId = Convert.ToInt32(model.NotificationID),
                                    DocType = docType,
                                    XmlInformation = model.ReturnXML,
                                    FileName = xsltPath,
                                    ESDALReferenceNo = EsdalReference,
                                    Contact = objcontact,
                                    DocumentFileName = docFileName,
                                    IcaStatus = icaStatus,
                                    xmlAttach = 0,
                                    ImminentMovestatus = false,
                                    OrganisationId = objcontact.OrganisationId,
                                    Projectstatus = ProjectStatus
                                };
                                GenerateEmail(emailParams);
                                break;
                            }
                        case "Fax":
                            {
                                objcontact.Fax = contactDet[2];
                                GenerateNotificationPDF(Convert.ToInt32(model.NotificationID), EsdalReference, docType, sODistribute, xsltPath, objcontact, docFileName, icaStatus, false);
                                break;
                            }

                        case "Online Inbox Only":
                            GenerateWord(Convert.ToInt32(model.NotificationID), EsdalReference, docType, sODistribute, xsltPath, objcontact, docFileName, 277001, false);
                            break;
                    }
                }
            }

            if (contactList.Count > 0)
            {
                #region Code part to clone movement details by extracting esdal reference code details
                string Esdalref = EsdalReference;
                Esdalref = Esdalref.Replace("~", "#");
                Esdalref = Esdalref.Replace("-", "/");
                Esdalref = Esdalref.Replace("#", "/");
                string[] esdalRefPro = Esdalref.Split('/');

                MovementCopyDetails moveDetails = new MovementCopyDetails();
                if (esdalRefPro.Length > 0)
                {
                    moveDetails.HaulMnemonic = Convert.ToString(esdalRefPro[0]);
                    moveDetails.ESDALRefNo = Convert.ToString(esdalRefPro[1].ToUpper().Replace("S", ""));
                    moveDetails.VersionNo = Convert.ToInt32(esdalRefPro[2].ToUpper().Replace("S", ""));
                }
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "STP.Document/GenerateDocuments/GenerateSOProposalDocument actionResult method successfully completed");
                //DAO call to copy from sort movement to portal movement 
                byte[] hacontactbytes = GetHAContactDetails(hacontact, agreedroute);
                documentService.CopyMovementSortToPortal(moveDetails, 0, versionid, EsdalReference, hacontactbytes, (int)SessionInfo.OrganisationId, SessionInfo.UserSchema);
                #endregion
            }

            #region movement actions for this action method
            if (contactList.Count != 0)
            {

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.MovementActionType = MovementnActionType.sort_desk_distributes_movement_version;
                string ErrMsg = string.Empty;
                movactiontype.ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                if (ProjectStatus == (int)Common.Enums.ProjectStatus.proposed)
                {
                    movactiontype.ProjectStatus = "PROPOSED";
                }
                else if (ProjectStatus == (int)Common.Enums.ProjectStatus.reproposed)
                {
                    movactiontype.ProjectStatus = "REPROPOSED";
                }
                else if (ProjectStatus == (int)Common.Enums.ProjectStatus.agreed)
                {
                    movactiontype.ProjectStatus = "AGREED";
                }
                else if (ProjectStatus == (int)Common.Enums.ProjectStatus.agreed_revised)
                {
                    movactiontype.ProjectStatus = "AGREED_REVISED";
                }
                else if (ProjectStatus == (int)Common.Enums.ProjectStatus.agreed_recleared)
                {
                    movactiontype.ProjectStatus = "AGREED_RECLEARED";
                }
                string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out ErrMsg);
                var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
                var sortPayload = sortSOApplicationManagement.GetSortAppPayload();
                documentService.SaveMovementActionForDistTrans(movactiontype, MovementDescription, sortPayload.ProjectId, sortPayload.LastRevisionNo, versionNo,SessionInfo.UserSchema);
            }
            #endregion

            return contactList.Count != 0 ? 1 : 0;

            #endregion
        }
        private void GenerateEmail(GenerateEmailParams emailParams)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            emailParams.UserInfo = SessionInfo;
            GenerateEmailgetParams emailgetParams;
            emailgetParams = documentService.GenerateEMAIL(emailParams);
            string logoImageName1 = ConfigurationManager.AppSettings["LogoImagePath"] + "Content/assets/images/logo.png";
            string logoImageName2 = ConfigurationManager.AppSettings["LogoImagePath"] + "Content/assets/images/National Highways Logo.svg";
            string logoImageTag = "<img align=\"left\" width=\"120\" height=\"80\" src=\"" + logoImageName1 + "\" /><img align=\"left\" width=\"120\" height=\"80\" src=\"" + logoImageName2 + "\" />";
            emailgetParams.HtmlContent = emailgetParams.HtmlContent.Replace("<img />", logoImageTag);

            byte[] CompressXMLString = XsltTransformer.CompressData(emailgetParams.AttachmentData);

            NotificationContacts notiContacts = new NotificationContacts();
            notiContacts.ContactId = emailParams.Contact.ContactId;
            notiContacts.ContactName = emailParams.Contact.FullName;
            notiContacts.Email = emailParams.Contact.Email;
            notiContacts.Fax = emailParams.Contact.Fax;
            notiContacts.Reason = emailParams.Contact.Reason;
            notiContacts.ContactPreference = ContactPreference.emailHtml; // contactPreference.onlineInboxOnly; the contact preference is changed to send email from TransmitNotification
            notiContacts.OrganisationId = emailParams.Contact.OrganisationId;
            notiContacts.NotificationId = emailParams.NotificationId; //adding notification id to the notification contact object.
            notiContacts.OrganistationName = emailParams.Contact.Organisation; // organisation name of contact . Needed in case of a manually added contact.
            var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
            var sortPayload = sortSOApplicationManagement.GetSortAppPayload();                                                      // First saving into Outbound documents and outbound documents metadata
            long docid = documentService.SaveDocument(emailParams.NotificationId, emailParams.DocType, emailParams.Contact.OrganisationId, emailParams.ESDALReferenceNo, emailParams.Contact.ContactId, CompressXMLString, "", SessionInfo, notiContacts, sortPayload.ProjectId, sortPayload.LastRevisionNo,sortPayload.LastVersionNo);

            byte[] content = emailParams.xmlAttach == 1
                ? documentService.TransmitNotification(notiContacts, SessionInfo, emailParams.ESDALReferenceNo, emailgetParams.HtmlContent, docid, false, emailgetParams.AttachmentData, emailParams.ImminentMovestatus, emailParams.DocType, sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo)
                : documentService.TransmitNotification(notiContacts, SessionInfo, emailParams.ESDALReferenceNo, emailgetParams.HtmlContent, docid, false, null, emailParams.ImminentMovestatus, emailParams.DocType, sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo);
            long transmissionId = documentService.SaveDistributionStatus(notiContacts, 0, 0, emailParams.ESDALReferenceNo, docid, emailParams.ImminentMovestatus);
            if (transmissionId != 0)
            {
                documentService.InsertTransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, emailParams.ESDALReferenceNo, 1, "", "Email", sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo);
            }
            //mail is ready to be transmitted
            documentService.SaveDistributionStatus(null, 5, 0, null, transmissionId); //updating distribution status with sending status

            bool mailStatus = communicationService.SendGeneralmail(emailParams.Contact.Email, "", content, emailgetParams.AttachmentData, emailParams.ESDALReferenceNo, emailParams.xmlAttach, emailParams.DocumentFileName, emailParams.ImminentMovestatus, emailParams.DocType);

            if (!mailStatus)
            {
                documentService.SaveDistributionStatus(null, 2, 0, null, transmissionId); // updating distribution status with failed status
                documentService.InsertTransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, "", 3, "Mail sending failed", "Email", sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo);
            }
            else
            {
                documentService.SaveDistributionStatus(null, 6, 0, null, transmissionId); //updating distribution status with delivered status
                documentService.TransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, emailParams.ESDALReferenceNo, 2, "", "Email", sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo);
            }

            //condition to check whether the objcontact is null or not
            //the following code part is to save notification in inbox items table additional conditions can be added to prevent from saving into inbox item's
            if (emailParams.Contact != null && emailParams.Contact.OrganisationId != 0) // && ImminentMovestatus == false  REMOVED FOR NEN PROJECT
            {
                //esDALRefNo is passed when docfile name is "SpecialOrderProposal"
                //saving into inbox items table
                if (emailParams.DocumentFileName == "SpecialOrderProposal")
                {
                    emailParams.NotificationId = 0; // Notification id is passed as 0 in case of SpecialOrderProposal
                }
                //finally saving into inbox items
                long inboxItemId;
                //SP return error
                inboxItemId = documentService.SaveInboxItems(emailParams.NotificationId, docid, emailParams.Contact.OrganisationId, emailParams.ESDALReferenceNo, SessionInfo.UserSchema, emailParams.IcaStatus, emailParams.ImminentMovestatus);// ADDED ImminentMovestatus FOR NEN PROJECT

                //updating status of inbox items entry with failed or updated 
                if (inboxItemId != 0)
                {
                    try
                    {
                        MovementActionIdentifiers movaction;
                        //UPDATING Movement history for inbox items
                        #region inbox_item_delivered
                        movaction = new MovementActionIdentifiers();
                        movaction.MovementActionType = MovementnActionType.inbox_item_delivered;
                        movaction.OrganisationNameReceiver = emailParams.Contact.Organisation;
                        movaction.ReciverContactName = emailParams.Contact.FullName;
                        documentService.GenerateMovementAction(SessionInfo, emailParams.ESDALReferenceNo, movaction,0,null, sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo);
                        #endregion
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                else
                {
                    documentService.SaveDistributionStatus(null, 7, 0, null, transmissionId); //updating distribution status with failed status
                }
            }
        }
        private void GenerateNotificationPDF(int NotificationId, string EsdalReference, int docType, SODistributeDocumentParams getParams, string xsltPath, ContactModel objcontact, string docFileName, int icaStatus, bool ImminentState)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            byte[] content;
            XMLModel model = new XMLModel();
            if (getParams.XmlModel.ReturnXML != null)
            {
                model = getParams.XmlModel;
            }
            else if (getParams.ModelStillAfftdSOA.ReturnXML != null)
            {
                model = getParams.ModelStillAfftdSOA;
            }
            else if (getParams.ModelPolice.ReturnXML != null)
            {
                model = getParams.ModelPolice;
            }
            else if (getParams.ModelSOA.ReturnXML != null)
            {
                model = getParams.ModelSOA;
            }
            GenerateEmailgetParams emailgetParams;
            emailgetParams = documentService.GenerateNotificationPDF(NotificationId, docType, model.ReturnXML, xsltPath, EsdalReference, objcontact.OrganisationId, objcontact, docFileName, SessionInfo, icaStatus, false, ImminentState);

            byte[] CompressXMLString = XsltTransformer.CompressData(emailgetParams.AttachmentData);

            NotificationContacts notiContacts = new NotificationContacts();
            notiContacts.ContactId = objcontact.ContactId;
            notiContacts.ContactName = objcontact.FullName;
            notiContacts.Email = objcontact.Email;
            notiContacts.Fax = objcontact.Fax;
            notiContacts.Reason = objcontact.Reason;
            notiContacts.ContactPreference = ContactPreference.fax; // contactPreference.onlineInboxOnly; the contact preference is changed to send email from TransmitNotification
            notiContacts.OrganisationId = objcontact.OrganisationId;
            notiContacts.NotificationId = NotificationId; //adding notification id to the notification contact object.
            notiContacts.OrganistationName = objcontact.Organisation; // organisation name of contact . Needed in case of a manually added contact.

            var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
            var sortPayload = sortSOApplicationManagement.GetSortAppPayload();
            // First saving into Outbound documents and outbound documents metadata
            long docid = documentService.SaveDocument(NotificationId, docType, objcontact.OrganisationId, EsdalReference, objcontact == null ? 0 : objcontact.ContactId, CompressXMLString, "", SessionInfo, notiContacts, sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo);

            content = documentService.TransmitNotification(notiContacts, SessionInfo, EsdalReference, emailgetParams.HtmlContent, docid, false, null, ImminentState, docType);

            long transmissionId = documentService.SaveDistributionStatus(notiContacts, 0, 0, EsdalReference, docid, ImminentState);
            if (transmissionId != 0)
            {
                documentService.InsertTransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, EsdalReference, 1, "", "Email");
            }
            //mail is ready to be transmitted
            documentService.SaveDistributionStatus(null, 5, 0, null, transmissionId); //updating distribution status with sending status
            documentService.InsertTransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, "", 4, "", "Fax");

            //SendFax(objContact, userInfo, transmissionId, content); //function call to send Fax
            bool FaxStatus = communicationService.SendFax(notiContacts, SessionInfo, transmissionId, content);
            if (!FaxStatus)
            {
                documentService.SaveDistributionStatus(null, 2, 0, null, transmissionId);//updating distribution status with failed status
                documentService.InsertTransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, "", 3, "Sending failed", "Fax");
            }
            //else if (FaxStatus == -1)
            //{
            //    DocumentTransmissionDAO.SaveDistributionStatus(null, 2, 0, null, transmissionId); //updating distribution status with failed status
            //    InsertTransmissionInfoToAction(objContact, userInfo, transmissionId, "", 3, "Sending failed ! Service exception occured", "Fax");
            //}
            //condition to check whether the objcontact is null or not
            //the following code part is to save notification in inbox items table additional conditions can be added to prevent from saving into inbox item's
            if (objcontact != null && objcontact.OrganisationId != 0) // && ImminentMovestatus == false  REMOVED FOR NEN PROJECT
            {
                //esDALRefNo is passed when docfile name is "SpecialOrderProposal"
                //saving into inbox items table
                if (docFileName == "SpecialOrderProposal")
                {
                    NotificationId = 0; // Notification id is passed as 0 in case of SpecialOrderProposal
                }
                //finally saving into inbox items
                long inboxItemId = documentService.SaveInboxItems(NotificationId, docid, objcontact.OrganisationId, EsdalReference, SessionInfo.UserSchema, icaStatus, ImminentState);// ADDED ImminentMovestatus FOR NEN PROJECT

                //updating status of inbox items entry with failed or updated 
                if (inboxItemId != 0)
                {
                    try
                    {
                        //UPDATING Movement history for inbox items
                        #region inbox_item_delivered
                        MovementActionIdentifiers movaction = new MovementActionIdentifiers
                        {
                            MovementActionType = MovementnActionType.inbox_item_delivered,
                            OrganisationNameReceiver = objcontact.Organisation,
                            ReciverContactName = objcontact.FullName
                        };
                        documentService.GenerateMovementAction(SessionInfo, EsdalReference, movaction);
                        #endregion
                    }
                    catch
                    {
                        //do nothing
                    }
                    documentService.SaveDistributionStatus(null, 10, 0, null, transmissionId); //updating distribution status with delivered status
                }
                else
                {
                    documentService.SaveDistributionStatus(null, 7, 0, null, transmissionId); //updating distribution status with failed status
                }
            }

        }
        private void GenerateWord(int NotificationId, string EsdalReference, int docType, SODistributeDocumentParams getParams, string xsltPath, ContactModel objcontact, string docFileName, int icaStatus, bool ImminentState, bool generateFlag = false)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            XMLModel model = new XMLModel();
            if (getParams.XmlModel.ReturnXML != null)
            {
                model = getParams.XmlModel;
            }
            else if (getParams.ModelStillAfftdSOA.ReturnXML != null)
            {
                model = getParams.ModelStillAfftdSOA;
            }
            else if (getParams.ModelPolice.ReturnXML != null)
            {
                model = getParams.ModelPolice;
            }
            else if (getParams.ModelSOA.ReturnXML != null)
            {
                model = getParams.ModelSOA;
            }
            GenerateEmailgetParams emailgetParams = documentService.GenerateWord(NotificationId, docType, model.ReturnXML, xsltPath, EsdalReference, SessionInfo.OrganisationId, docFileName, objcontact, SessionInfo, icaStatus, false, ImminentState);

            NotificationContacts notiContacts = new NotificationContacts();
            if (!(docFileName.Contains("2D") || docFileName.Contains("Amendment") || docFileName.Contains("FormVR1")))
            {
                notiContacts.ContactId = objcontact.ContactId;
                notiContacts.ContactName = objcontact.FullName;
                notiContacts.Email = objcontact.Email;
                notiContacts.Fax = objcontact.Fax;
                notiContacts.Reason = objcontact.Reason;
                notiContacts.ContactPreference = ContactPreference.onlineInboxOnly;
                notiContacts.OrganisationId = objcontact.OrganisationId; //adding organisationiD to the object for saving into active transactions folder
                notiContacts.OrganistationName = objcontact.Organisation; // organisation name of contact . Needed in case of a manually added contact.
            }

            byte[] CompressXMLString = XsltTransformer.CompressData(emailgetParams.AttachmentData);
            var sortSOApplicationManagement = new SORTSOApplicationManagement(sortSOProcessingService);
            var sortPayload = sortSOApplicationManagement.GetSortAppPayload();
            //saving into outbound documents and outbound document meta data
            long docid = documentService.SaveDocument(NotificationId, docType, objcontact.OrganisationId, EsdalReference, objcontact == null ? 0 : objcontact.ContactId, CompressXMLString, "", SessionInfo, notiContacts, sortPayload.ProjectId, sortPayload.LastRevisionNo, sortPayload.LastVersionNo);

            bool result = true;
            if (generateFlag)//added generateFlag condition to avoid transmission update for generate Document in order to solve redmine #4959
            {
                //saving into active transactions and distribution status
                documentService.TransmitNotification(notiContacts, SessionInfo, EsdalReference, emailgetParams.HtmlContent, docid, false, null, ImminentState, docType);
            }

            if (!result)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Exceptioned occured during transmission of esdalRef : {0} and Transid : {1}", EsdalReference, docid));
            }

            long transmissionId = documentService.SaveDistributionStatus(notiContacts, 0, 1, EsdalReference, docid, ImminentState);

            if (transmissionId != 0)
            {
                documentService.InsertTransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, EsdalReference, 1, "", "Online inbox");
            }
            //condition to check whether the objcontact is null or not
            //the following code part is to save notification in inbox items table
            if (objcontact != null && objcontact.OrganisationId != 0) // && ImminentMovestatus == false  REMOVED FOR NEN PROJECT
            {
                //esDALRefNo is passed when docfile name is "SpecialOrderProposal"
                //saving into inbox items table
                if (docFileName == "SpecialOrderProposal")
                {
                    NotificationId = 0; // Notification id is passed as 0 in case of SpecialOrderProposal
                }
                long inboxItemId = documentService.SaveInboxItems(NotificationId, docid, objcontact.OrganisationId, EsdalReference, SessionInfo.UserSchema, icaStatus, ImminentState);


                //updating status of inbox items entry with failed or updated 
                if (inboxItemId != 0)
                {
                    try
                    {
                        //UPDATING Movement history for inbox items
                        #region inbox_item_delivered
                        MovementActionIdentifiers movementAction = new MovementActionIdentifiers
                        {
                            MovementActionType = MovementnActionType.inbox_item_delivered,
                            OrganisationNameReceiver = objcontact.Organisation,
                            ReciverContactName = objcontact.FullName
                        };
                        documentService.GenerateMovementAction(SessionInfo, EsdalReference, movementAction);
                        #endregion
                    }
                    catch
                    {
                        //do nothing
                    }
                    documentService.SaveDistributionStatus(null, 10, 0, null, transmissionId); //updating distribution status with delivered status for online only preference
                    documentService.TransmissionInfoToAction(notiContacts, SessionInfo, transmissionId, EsdalReference, 2, "", "Online inbox");
                }
                else
                {
                    documentService.SaveDistributionStatus(null, 11, 0, null, transmissionId); //updating distribution status with failed status for online only preference
                }
            }
            byte[] DocByteArrayData = null;
            if (docFileName.Contains("2D") || docFileName.Contains("Amendment") || docFileName.Contains("FormVR1"))
            {
                DocByteArrayData = Encoding.UTF8.GetBytes(emailgetParams.HtmlContent);
            }
            if (DocByteArrayData != null)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "GenerateWord completed successfully");
            }
        }

        private byte[] GetHAContactDetails(HAContact haContactDetailObj, AgreedRouteStructure agreedroute)
        {
            HAContactStructure objhacontact = new HAContactStructure();

            AggreedRouteXSD.AddressStructure sddrstructure = new AggreedRouteXSD.AddressStructure();
            objhacontact.TelephoneNumber = haContactDetailObj.Telephone;
            objhacontact.EmailAddress = haContactDetailObj.Email;
            objhacontact.Contact = haContactDetailObj.ContactName;
            objhacontact.FaxNumber = haContactDetailObj.Fax;
            string[] Addstru = new string[5];
            Addstru[0] = haContactDetailObj.HAAddress1;
            Addstru[1] = haContactDetailObj.HAAddress2;
            Addstru[2] = haContactDetailObj.HAAddress3;
            Addstru[3] = haContactDetailObj.HAAddress4;
            Addstru[4] = haContactDetailObj.HAAddress5;
            sddrstructure.Line = Addstru;
            objhacontact.Address = sddrstructure;
            switch (haContactDetailObj.Country)
            {
                case "England":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.england;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Wales":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.wales;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Scotland":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.scotland;
                    sddrstructure.CountrySpecified = true;
                    break;

                case "Northern Ireland":
                    sddrstructure.Country = AggreedRouteXSD.CountryType.northernireland;
                    sddrstructure.CountrySpecified = true;
                    break;
            }
            sddrstructure.PostCode = haContactDetailObj.PostCode;
            agreedroute.HAContact = objhacontact;

            XmlSerializer serializer = new XmlSerializer(typeof(AgreedRouteStructure));

            StringWriter outStream = new StringExtractor.Utf8StringWriter();    // The code generates UTF-8 encoded xml string 
            serializer.Serialize(outStream, agreedroute);
            string str = outStream.ToString();

            byte[] hacont = StringExtractor.ZipAndBlob(str);
            return hacont;
        }
        */
        #endregion

        #region public ActionResult GetAffectedParties(int analysisId, int IsVR1)
        public ActionResult GetAffectedParties(int analysisId = 0, bool IsVR1 = false, int revisionId = 0, int haulierOrgId = 0)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.FullName = SessionInfo.FirstName + " " + SessionInfo.LastName;
                ViewBag.OrgName = SessionInfo.OrganisationName;

                if (IsVR1)
                    ViewBag.IsVR1Sort = true;
                else
                    ViewBag.IsVR1Sort = false;

                ViewBag.RevisionId = revisionId;
                ViewBag.HaulierOrgId = haulierOrgId;
                ViewBag.NotifId = 0;
                ViewBag.AnalysisId = analysisId;
                return PartialView("AffectedParties");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/AffectedParties, Exception: {0}", ex.StackTrace));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion 

        public ActionResult CheckRouteAssessment(int AnalysisId = 0, int APP_Rev_ID = 0, bool VR1App = false, long MovAnalysisId = 0)
        {
            try
            {
                string messg = "SORTApplications/CheckRouteAssessment?AnalysisId=" + AnalysisId + "APP_Rev_ID=" + APP_Rev_ID + "VR1App=" + VR1App;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                //Check affected parties generated.
                long movRevisionId = 0;
                int isModified = 0;
                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(AnalysisId, 9, SessionInfo.UserSchema);
                isModified = sortApplicationService.CheckCandIsModified(AnalysisId);

                if (objRouteAssessmentModel.DriveInst == null)
                    movRevisionId = 1;
                else if (objRouteAssessmentModel.AffectedStructure == null)
                    movRevisionId = 1;
                else if (objRouteAssessmentModel.RouteDescription == null)
                    movRevisionId = 1;
                else if (objRouteAssessmentModel.AffectedParties == null)
                    movRevisionId = 1;
                else if (objRouteAssessmentModel.AffectedParties != null && isModified == 1)
                    movRevisionId = 1;
                else if (objRouteAssessmentModel.AffectedRoads == null)
                    movRevisionId = 1;
                else
                    movRevisionId = 2;
                return Json(movRevisionId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("SORTApplication/CheckRouteAssessment, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult ReviseVR1Application(long apprevid = 0, string ESDALRefCode = "", int versionno = 0, int revisionno = 0)
        {
            try
            {
                int versionNo = 0;
                string messg = "SORTApplications/ReviseVR1Application?apprevid=" + apprevid + "ESDALRefCode=" + ESDALRefCode;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;

                ViewBag.organisationId = organisationId;
                ReviseVR1Params reviseVR1Params = new ReviseVR1Params()
                {
                    ApplicationRevisionId = apprevid,
                    UserSchema = SessionInfo.UserSchema
                };

                ApplyForVR1 obj = sortApplicationService.ReviseVR1Application(reviseVR1Params);
                var activityName = WorkflowTaskFinder.GenerateWorkflowActivityName(WorkflowActivityTypes.An_Activity_VehicleDetails);
                var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
                PlanMvmntPayLoad planMvmntPayLoad = new PlanMvmntPayLoad
                {
                    NextActivity = activityName,
                    OrgId = obj.OrganisationId,
                    OrgName = obj.HaulierOrgName,
                    MovementKey = obj.ApplicationRevisionId,
                    RevisionId = obj.ApplicationRevisionId,
                    IsVr1App = true,
                    IsApp = true,
                    IsSortApp = true,
                    VehicleMoveId = obj.MovementId,
                    VersionId = obj.VersionId,
                    IsRevise = true,
                    MovementType = (int)MovementType.vr_1,
                    PrevMovType = (int)MovementType.vr_1,
                    VehicleClass = obj.SubMovementClass
                };
                if (!applicationNotificationManagement.IsThisMovementExist(0, obj.ApplicationRevisionId, out string workflowKey))
                {
                    applicationNotificationManagement.StartWorkflow(planMvmntPayLoad, 2);
                    new SessionData().E4_AN_PlanMovement = planMvmntPayLoad;
                }

                #region movement actions for this action method
                if (SessionInfo.UserSchema == UserSchema.Portal)
                {
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.haulier_revises_application;
                    movactiontype.ESDALRef = ESDALRefCode;
                    string ErrMsg = string.Empty;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription,obj.ProjectId,obj.RevisionNumber,versionNo, UserSessionValue.UserSchema);
                }
                #endregion

                return Json(obj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/ReviseVR1Application, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #region SaveAnnotation
        /// <summary>
        /// Save annotation
        /// </summary>
        /// <param name="plannedRoutePath1"></param>
        /// <returns></returns>
        public ActionResult SaveMovAnnotation(RoutePart plannedRoutePath1, long revisionId, long analysisId, string contentRefNo = "", int versionId = 0)
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
                return RedirectToAction("Login", "Account");
            else
                SessionInfo = (UserInfo)Session["UserInfo"];

            bool result = false;
            if (plannedRoutePath1 != null && plannedRoutePath1.RoutePartDetails.RouteId != 0)
            {
                result = routeService.SaveRouteAnnotation(plannedRoutePath1, 0, SessionInfo.UserSchema);
                RouteAssessmentModel routeAssessmentModel = new RouteAssessmentModel();
                List<RoutePartDetails> routePartDetails = routeService.GetRouteDetailForAnalysis(versionId, contentRefNo, revisionId, 0, SessionInfo.UserSchema);
                routeAssessmentModel.Annotation = routeAssessmentService.GenerateAffectedAnnotation(routePartDetails, SessionInfo.UserSchema);
                routeAssessmentService.UpdateAnalysedRoute(routeAssessmentModel, analysisId, SessionInfo.UserSchema);
                routeAssessmentService.GenerateDrivingInstnRouteDesc(routePartDetails, analysisId, SessionInfo.UserSchema);
            }
            return Json(result);
        }
        #endregion
        private void GenerateMovementRouteAssessment(long versionId = 0, long analysisId = 0)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"SORTApplication/GenerateMovementRouteAssessment actionResult method started successfully");
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            RouteAssessmentModel routeAssessmentModel = new RouteAssessmentModel();
            try
            {
                List<RoutePartDetails> routePartDetails = routeService.GetRouteDetailForAnalysis(versionId, null, 0, 0, SessionInfo.UserSchema);
                routeAssessmentModel.AffectedStructure = routeAssessmentService.GenerateAffectedStructures(routePartDetails, null, 0, SessionInfo.UserSchema);
                //routeAssessmentModel.AffectedRoads = routeAssessmentService.GenerateAffectedRoads(routePartDetails, SessionInfo.UserSchema);
                //routeAssessmentModel.Annotation = routeAssessmentService.GenerateAffectedAnnotation(routePartDetails, SessionInfo.UserSchema);
                //routeAssessmentModel.Cautions = routeAssessmentService.GenerateAffectedCautions(routePartDetails, 0, SessionInfo.UserSchema);
                //routeAssessmentModel.Constraints = routeAssessmentService.GenerateAffectedConstraints(routePartDetails, SessionInfo.UserSchema);
                // routeAssessmentModel.AffectedParties = routeAssessmentService.GenerateAffectedParties(routePartDetails, 0, 0, SessionInfo.UserSchema, (int)VSOType.soapolice);

                routeAssessmentService.UpdateAnalysedRoute(routeAssessmentModel, analysisId, SessionInfo.UserSchema);
                //routeAssessmentService.GenerateDrivingInstnRouteDesc(routePartDetails, analysisId, SessionInfo.UserSchema);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"SORTApplication/GenerateMovementRouteAssessment, Exception:" + ex);
            }
        }

        #region private void VehicleDimensionDropDown()
        private void VehicleDimensionDropDown()
        {
            List<FilterDropDown> objListDropDown = new List<FilterDropDown>();
            FilterDropDown objDropDown = null;
            objDropDown = new FilterDropDown();
            objDropDown.Id = "gross_weight_max_kg";
            objDropDown.Value = "Gross weight";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "width_max_mtr";
            objDropDown.Value = "Width";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "max_height_max_mtr";
            objDropDown.Value = "Height";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "red_height_max_mtr";
            objDropDown.Value = "Reducible height";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "len_max_mtr";
            objDropDown.Value = "Length";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "rigid_len_max_mtr";
            objDropDown.Value = "Rigid length";
            objListDropDown.Add(objDropDown);
            objDropDown = new FilterDropDown();
            objDropDown.Id = "max_axle_weight_max_kg";
            objDropDown.Value = "Max Axle weight";
            objListDropDown.Add(objDropDown);

            int vehicleDimensionCount = 1;

            MovementsAdvancedFilter objMovementsAdvancedFilter = new MovementsAdvancedFilter();
            if (Session["g_AdvancedSearchData"] != null)
            {
                objMovementsAdvancedFilter = (MovementsAdvancedFilter)Session["g_AdvancedSearchData"];
                //weightCount = objMovementsAdvancedFilter.WeightCount;
            }

            ViewBag.VehicleDimensionCount = new SelectList(objListDropDown, "Id", "Value", vehicleDimensionCount);
        }
        #endregion
    }

}