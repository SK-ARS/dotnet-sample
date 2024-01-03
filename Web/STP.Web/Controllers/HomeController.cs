using PagedList;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.Web.WorkflowProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STP.Web.Filters;

namespace STP.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMovementsService movementsService;
        private readonly IFeedbackService feedbackService;
        private readonly IInformationService informationService;
        readonly PrivacyDisclaimer privacyDisclaimerDetails = new PrivacyDisclaimer();
        public HomeController(IMovementsService movementsService, IFeedbackService feedbackService,IInformationService informationService)
        {
            this.movementsService = movementsService;
            this.feedbackService = feedbackService;
            this.informationService = informationService;
        }
       
        public ActionResult Hauliers(int page=1, int pageSize=5, bool MovementListForSO=false)
        {
            
            if ( (! Convert.ToString(Session["userTypeId"]).Equals("696002")) && (!Convert.ToString(Session["userTypeId"]).Equals("696006")) && (!Convert.ToString(Session["userTypeId"]).Equals("696007")) && (!Convert.ToString(Session["userTypeId"]).Equals("696008")))
            {


                ViewBag.PortalType = Constants.Haulier;

                if (TempData["SetPreferenceMessage"] != null && TempData["SetPreferenceMessage"].ToString() == "1")
                {
                    ViewBag.SetPreferenceMessage = "1";
                }

                ViewBag.DaysToExpire = TempData["ExpiryDays"];
                Session["PreferenceNotesBLOB"] = null;
                Session["PreferenceNotes"] = null;


                if (TempData["success"] != null && TempData["success"].ToString() == "1")
                {
                    ViewBag.saveMsg = "1";
                }

                ViewBag.RedirectToChangePwd = TempData["RedirectToChangePwd"];
                UserInfo SessionInfo = null;

                ViewBag.HelpdeskDistRedirect = "false";


                SessionInfo = (UserInfo)Session["UserInfo"];

                ViewBag.MovementListForSO = MovementListForSO;
                
                TempData["heading"] = "news";
                List<InformationModel> Informations = informationService.GetUniqueInfoList(1, 6, SessionInfo.UserTypeId, "news story");
                return View(Informations);
            }
            else
            {
                return RedirectToAction("LogOut", "Account");
            }

        }

        public ActionResult Police(int page=1, int pageSize=5, int structureID = 0, string structureNm = "", string ESRN = "", bool IsRelatedMov = false)
        {
            if (Convert.ToString(Session["userTypeId"]).Equals("696002"))
            {

           
                  
                ViewBag.PortalType = Constants.Police;
            
            if (TempData["SetPreferenceMessage"] != null && TempData["SetPreferenceMessage"].ToString() == "1")
            {
                ViewBag.SetPreferenceMessage = "1";
            }

            ViewBag.DaysToExpire = TempData["ExpiryDays"];
            Session["PreferenceNotesBLOB"] = null;
            Session["PreferenceNotes"] = null;


            if (TempData["success"] != null && TempData["success"].ToString() == "1")
            {
                ViewBag.saveMsg = "1";
            }

            ViewBag.RedirectToChangePwd = TempData["RedirectToChangePwd"];
            UserInfo SessionInfo = null;
           
                       ViewBag.HelpdeskDistRedirect = "false";
           

            ViewBag.structureID = 0;
            ViewBag.IsRelatedMov = IsRelatedMov;
           
            SessionInfo = (UserInfo)Session["UserInfo"];
           
            int organisationId = (int)SessionInfo.OrganisationId; 
            int userType = SessionInfo.UserTypeId;

            MovementsInboxFilter objMovementsInboxFilter = new MovementsInboxFilter();
            MovementsInboxAdvancedFilter objMovementsInboxAdvancedFilter = new MovementsInboxAdvancedFilter();


            objMovementsInboxFilter.Unopened = true;
            objMovementsInboxFilter.ImminentMovement = true;
            objMovementsInboxAdvancedFilter.SortOrder = 1;


            if (Session["g_moveInboxFilter"] != null)
            {
                objMovementsInboxFilter = (MovementsInboxFilter)Session["g_moveInboxFilter"];
            }
            if (Session["g_moveInboxAdvanceFilter"] != null)
            {
                objMovementsInboxAdvancedFilter = (MovementsInboxAdvancedFilter)Session["g_moveInboxAdvanceFilter"];
            }

            TempData["IsFirstTime"] = "0";
            ViewBag.V_objMovementsInboxFilter = objMovementsInboxFilter;
                GetInboxMovementsParams inboxMovementsParams = new GetInboxMovementsParams
                {
                    OrganisationId = organisationId,
                    PageNumber = page,
                    PageSize = pageSize,
                    UserSchema = SessionInfo.UserSchema,
                    UserId = Convert.ToInt32(SessionInfo.UserId),
                    UserType = userType
                };

                List<MovementsInbox> objMovementInbox = movementsService.GetHomePageMovements(inboxMovementsParams);

                var movementInboxObjPagedList = new StaticPagedList<MovementsInbox>(objMovementInbox, page, pageSize, pageSize);
            
            return View(movementInboxObjPagedList);
            }
            else
            {
                ModelState.AddModelError("", "Your dont have right to access this page.");
                return RedirectToAction("LogOut", "Account");
            }
        }

        public ActionResult SOA(int page = 1, int pageSize = 5, int structureID = 0, string structureNm = "", string ESRN = "", bool IsRelatedMov = false)
        {
            
            if (Convert.ToString(Session["userTypeId"]).Equals("696007"))
            {
                ViewBag.PortalType = Constants.SOA;

            if (TempData["SetPreferenceMessage"] != null && TempData["SetPreferenceMessage"].ToString() == "1")
            {
                ViewBag.SetPreferenceMessage = "1";
            }

            ViewBag.DaysToExpire = TempData["ExpiryDays"];
            Session["PreferenceNotesBLOB"] = null;
            Session["PreferenceNotes"] = null;


            if (TempData["success"] != null && TempData["success"].ToString() == "1")
            {
                ViewBag.saveMsg = "1";
            }

            ViewBag.RedirectToChangePwd = TempData["RedirectToChangePwd"];
            UserInfo SessionInfo = null;
            
                       ViewBag.HelpdeskDistRedirect = "false";
            

            ViewBag.structureID = 0;
            ViewBag.IsRelatedMov = IsRelatedMov;
            
            SessionInfo = (UserInfo)Session["UserInfo"];
            
            int organisationId = (int)SessionInfo.OrganisationId;
            int userType = SessionInfo.UserTypeId;

            MovementsInboxFilter objMovementsInboxFilter = new MovementsInboxFilter();
            MovementsInboxAdvancedFilter objMovementsInboxAdvancedFilter = new MovementsInboxAdvancedFilter();

            objMovementsInboxFilter.Unopened = true;
            objMovementsInboxFilter.ImminentMovement = true;
            objMovementsInboxAdvancedFilter.SortOrder = 1;


            if (Session["g_moveInboxFilter"] != null)
            {
                objMovementsInboxFilter = (MovementsInboxFilter)Session["g_moveInboxFilter"];
            }
            if (Session["g_moveInboxAdvanceFilter"] != null)
            {
                objMovementsInboxAdvancedFilter = (MovementsInboxAdvancedFilter)Session["g_moveInboxAdvanceFilter"];
            }

            TempData["IsFirstTime"] = "0";
            ViewBag.V_objMovementsInboxFilter = objMovementsInboxFilter;
                GetInboxMovementsParams inboxMovementsParams = new GetInboxMovementsParams
                {
                    OrganisationId = organisationId,
                    PageNumber = page,
                    PageSize = pageSize,
                    UserSchema = SessionInfo.UserSchema,
                    UserId = Convert.ToInt32(SessionInfo.UserId),
                    UserType = userType
                };

                List<MovementsInbox> objMovementInbox = movementsService.GetHomePageMovements(inboxMovementsParams);

                var movementInboxObjPagedList = new StaticPagedList<MovementsInbox>(objMovementInbox, page, pageSize, pageSize);
            
            return View(movementInboxObjPagedList);
            }
            else
            {
                return RedirectToAction("LogOut", "Account");
            }
        }

        public ActionResult SORT(int page=1, int pageSize=5)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Home/SORT actionResult method started successfully"));

            
            if (Convert.ToString(Session["userTypeId"]).Equals("696008"))
            {
                ViewBag.PortalType = Constants.SORT;

            if (TempData["SetPreferenceMessage"] != null && TempData["SetPreferenceMessage"].ToString() == "1")
            {
                ViewBag.SetPreferenceMessage = "1";
            }

            ViewBag.DaysToExpire = TempData["ExpiryDays"];
            Session["PreferenceNotesBLOB"] = null;
            Session["PreferenceNotes"] = null;


            if (TempData["success"] != null && TempData["success"].ToString() == "1")
            {
                ViewBag.saveMsg = "1";
            }

            ViewBag.RedirectToChangePwd = TempData["RedirectToChangePwd"];
            UserInfo SessionInfo = null;
            
                       ViewBag.HelpdeskDistRedirect = "false";
            

             SessionInfo = (UserInfo)Session["UserInfo"];
            
            int organisationId = (int)SessionInfo.OrganisationId;
            bool IsCreCandidateOrCreAppl = false;

            
            SORTMovementFilter objSORTMovementFilter = new SORTMovementFilter();
            objSORTMovementFilter.UserID = SessionInfo.UserId;
            objSORTMovementFilter.Unallocated = true;
            objSORTMovementFilter.InProgress = true;
            

            SortAdvancedMovementFilter objSORTMovementFilterAdvanced = new SortAdvancedMovementFilter();
            

            ViewBag.SORTMovementFilter = objSORTMovementFilter;

            List<SORTMovementList> sortMovementObj = null;
            
                sortMovementObj = movementsService.GetSORTMovementList(organisationId, page, pageSize, objSORTMovementFilter, objSORTMovementFilterAdvanced, IsCreCandidateOrCreAppl);
           

            ViewBag.StructureID = null;

            int totalCount = 0;
            if (sortMovementObj.Count > 0)
            {
                totalCount = sortMovementObj[0].TotalRecordCount;
            }
                Session["NewNewsCount"] = 0;
                var movementObjPagedList = new StaticPagedList<SORTMovementList>(sortMovementObj, page, pageSize, totalCount);
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Home/SORT actionResult method completed successfully"));
            return View(movementObjPagedList);
            }
            else
            {
                return RedirectToAction("LogOut", "Account");
            }
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }

        [SessionValidate(Disable = true)]
        public ActionResult Error()
        {
            ViewBag.Message = "Error page.";

            return View();
        }

        [ValidateInput(true)]
        public ActionResult Feedback()
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL,"Home/Feedback actionResult method started successfully");

            return View("Feedback", privacyDisclaimerDetails);
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult SubmitFeedback()
        {
            return View("Feedback", privacyDisclaimerDetails);
        }

        /// <summary>
        /// Insert feedback details
        /// </summary>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult InsertFeedbackDetails(InsertFeedbackDomain objInsertFeedback)
        {
            try
            {

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"Home/InsertFeedbackDetails JsonResult method completed successfully started");
                feedbackService.InsertFeedbackDetails(objInsertFeedback);
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"Home/InsertFeedbackDetails JsonResult method completed successfully");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Home/InsertFeedbackDetails, Exception: {0}", ex));
                throw;
            }
        }

        
    }
}