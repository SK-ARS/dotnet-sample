using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using System.Web.Security;
using STP.Web.Filters;using STP.Common.EncryptDecrypt;
using STP.Domain.Routes;
using System.Text;
using System.Xml;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Common.Constants;
using STP.Domain.LoggingAndReporting;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.ServiceAccess.Routes;
using STP.Web.General;
using static STP.Domain.Routes.RouteModel;
using STP.Domain.Routes.QAS;
using STP.Domain.MovementsAndNotifications.Movements;
using static STP.Domain.Routes.RouteModelJson;
using Newtonsoft.Json;
using STP.Domain.RouteAssessment;
using STP.Common.General;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.RouteAssessment;
using STP.ServiceAccess.RoadNetwork;
using STP.Domain.Custom;
using System.Text.RegularExpressions;
using STP.ServiceAccess.SecurityAndUsers;
using STP.Common.Enums;

namespace STP.Web.Controllers
{
   
    public class NENNotificationController : Controller
    {

        private readonly INENNotificationService NENNotificationService;
        private readonly IAuditLogService auditLogService;
        private readonly IMovementsService movementsService;
        private readonly ILoggingService loggingService;
        private readonly IVehicleConfigService vehicleconfigService;
        private readonly IRoutesService routesService;
        private readonly IQasService qasService;
        private readonly ISORTApplicationService iSORTApplicationService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly IConstraintService constraintService;
        private readonly INotificationService notificationsService;
        private readonly IOrganizationService organizationService;

        public NENNotificationController(INENNotificationService NENNotificationService, IAuditLogService auditLogService, IMovementsService movementsService, ILoggingService logging, IVehicleConfigService vehicleconfigService, IRoutesService routesService, IQasService qasService, ISORTApplicationService iSORTApplicationService, IRouteAssessmentService routeAssessmentService, IConstraintService constraintService, INotificationService notificationsService, IOrganizationService organizationService)
        {
            this.NENNotificationService = NENNotificationService;
            this.auditLogService = auditLogService;
            this.movementsService = movementsService;
            this.loggingService = logging;
            this.vehicleconfigService = vehicleconfigService;
            this.routesService = routesService;
            this.qasService = qasService;
            this.iSORTApplicationService = iSORTApplicationService;
            this.routeAssessmentService = routeAssessmentService;
            this.constraintService = constraintService;
            this.notificationsService = notificationsService;
            this.organizationService = organizationService;
        }
        // GET: NENNotification
        public ActionResult Index()
        {
            return View();
        }

        #region  NEN AuditLog list
        /// <summary>
        /// NEN AuditLog list
        /// </summary>
        /// <returns></returns>
        public ActionResult NENAuditLog(string ESDAL_ref_num = null, int sortFlag = 0, int pageNumber = 1, int pageSize = 10, int searchNotificationSource = 0, string searchCriteria = "0", int? sortOrder = null, int? sortType = null)
        {
            searchNotificationSource = 0;
            UserInfo SessionInfo = null;
            long org_ID = 0;// To show All organisation's NEN to Helpdesk portal inside Audit log menu
            string searchItem = null;
            int presetFilter = 0;
            ViewBag.sortOrder = sortOrder == null ? null : sortOrder;

            presetFilter = sortType != null ? (int)sortType : presetFilter;
            ViewBag.sortType= presetFilter;
            try
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (SessionInfo.UserTypeId == 696002 || SessionInfo.UserTypeId == 696007)// To show only same organisation's NEN to SOA and Police portal inside Audit log menu
                {
                    org_ID = SessionInfo.OrganisationId;
                }


                if (Session["AuditlogPageSize"] == null)
                {
                    Session["AuditlogPageSize"] = 10;
                }

                if (pageSize == null)
                {
                    pageSize = (int)Session["AuditlogPageSize"];
                }
                else
                {
                    pageSize = (int)Session["AuditlogPageSize"];
                }
                if (pageNumber == 1 && ESDAL_ref_num == null)
                {
                    //first time page is loaded page parameter will be null and search column will also be null
                    ESDAL_ref_num = null;
                    sortFlag = 0;
                    searchCriteria = "0";
                    TempData["ESDAL_ref_num"] = null;
                    TempData["sortFlag"] = 0;
                    TempData["searchCriteria"] = "0";
                }
                else if (pageNumber == 1 && ESDAL_ref_num != null && ESDAL_ref_num != "")
                {
                    //search button is clicked, page parameter is null and search column is supplied
                    //so save these values in the temp data                

                    if (TempData["ESDAL_ref_num"] != null)
                    {
                        sortFlag = Convert.ToInt32(TempData["sortFlag"]);
                        if (searchCriteria == "0")
                            searchCriteria = Convert.ToString(TempData["searchCriteria"]);
                    }
                    else
                    {
                        TempData["ESDAL_ref_num"] = ESDAL_ref_num;
                        if (sortFlag == null || sortFlag == 0)
                        {
                            TempData["sortFlag"] = 0;
                            sortFlag = 0;
                        }
                        else
                            TempData["sortFlag"] = sortFlag;
                        if (String.IsNullOrEmpty(searchCriteria))
                        {
                            TempData["searchCriteria"] = "0";
                            searchCriteria = "0";
                        }
                        else
                            TempData["searchCriteria"] = searchCriteria;
                    }
                }
                else if (pageNumber != 1 && ESDAL_ref_num == null)
                {
                    //during page number click, the page parameter will not be null but the model will be null
                    //so put the tempdata values back into the  contactSearch

                    if (TempData["ESDAL_ref_num"] == null)
                    {
                        ESDAL_ref_num = null;
                    }
                    else
                    {
                        ESDAL_ref_num = Convert.ToString(TempData["ESDAL_ref_num"]);
                    }

                    sortFlag = Convert.ToInt32(TempData["sortFlag"]);
                    searchCriteria = Convert.ToString(TempData["searchCriteria"]);
                }

                if (ESDAL_ref_num != null)
                {
                    ViewBag.SearchESDAL_num = ESDAL_ref_num;
                    searchItem = ESDAL_ref_num;
                }
                else
                {
                    ESDAL_ref_num = ViewBag.SearchESDAL_num;
                }
                if (ESDAL_ref_num == null)
                {
                    sortFlag = 0;
                    searchCriteria = "0";
                }
                ViewBag.sort_Flag = sortFlag;
                ViewBag.page_Number = pageNumber;
                ViewBag.page_Size = pageSize;
                ViewBag.searchItem = searchCriteria;
                List<NENAuditLogList> auditLogObjList;
                searchCriteria = sortFlag == 0 ? "0" : searchCriteria;
                auditLogObjList = loggingService.GetAuditListSearch(searchItem, pageNumber, pageSize, sortFlag, org_ID, searchNotificationSource, searchCriteria, presetFilter, sortOrder);
               // int totalCount = 20000000;
                int totalCount = 0;
                if (auditLogObjList != null && auditLogObjList.Count != 0)
                {
                    //int.TryParse(auditLogObjList[0].RecordCount.ToString(), out tempPageCount);
                    totalCount = Convert.ToInt32(auditLogObjList[0].RecordCount);
                    //auditLogObjList[0].RecordCount = tempPageCount;
                }
                ViewBag.TotalCount = totalCount;
                var AuditLogPageList = new StaticPagedList<NENAuditLogList>(auditLogObjList, pageNumber, (int)pageSize, totalCount);

                return View("NENAuditLog", AuditLogPageList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NENAuditLog, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region Non Esdal Haulier List
        [AuthorizeUser(Roles = "11003")]
        public ActionResult ListNEHaulier(int? page, int? pageSize, string SearchString = null, int Isval = 0, int? sortType=null,int? sortOrder=null, bool isClear = false,bool isSearch=false)
        {
            if (isClear)
            {
                Session["SearchStringNEN"] = null;
                Session["IsvalNEN"] = null;
            }
            if ((string.IsNullOrEmpty(SearchString) && Session["SearchStringNEN"] != null)||
                !isSearch && Session["SearchStringNEN"] != null)
                SearchString = Session["SearchStringNEN"].ToString();
            if (!isSearch&& Session["IsvalNEN"]!=null)
                Isval = Convert.ToInt32(Session["IsvalNEN"]);

            List<NeHaulierList> GridListObj = new List<NeHaulierList>();
            //Verifying session
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            int presetFilter = 0;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (SessionInfo.IsAdmin == 0 && SessionInfo.UserTypeId != 696008)
            {
                return RedirectToAction("Error", "Home");
            }

            if (SearchString == null)
            {
                SearchString = (string)Session["NeUsername"];
            }

            if (pageSize == null)
            {
                pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
            }
            else
            {
                Session["PageSize"] = pageSize;
            }
            presetFilter = sortType != null ? (int)sortType : presetFilter;
            int sortorder = sortOrder != null ? (int)sortOrder : 1;
            int pageNum = (page ?? 1);
            ViewBag.SortType = sortType;
            ViewBag.SortOrder = sortorder;
              GridListObj = NENNotificationService.GetNeHaulier(pageNum, (int)pageSize, SearchString, Isval, presetFilter,(int)sortorder);
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
                /*  ViewBag.OrgSearchString*/
                ViewBag.OrgSearchString = SearchString;
            }


            if (SearchString != null)
            {

                GridListObj = (from s in GridListObj
                               where s.HaulierName.Trim().ToLower().Contains(SearchString.Trim().ToLower())
                               select s).ToList();
            }

            if (pageSize == null)
            {
                pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
            }
            else
            {
                Session["PageSize"] = pageSize;
            }



            int pageNumber = (page ?? 1);
            long totalCount = 0;
            if (GridListObj.Count > 0)
            {
                ViewBag.TotalCount = Convert.ToInt32(GridListObj[0].TotalCount);
                totalCount = GridListObj[0].TotalCount;
            }
            else
            {
                ViewBag.TotalCount = 0;
            }
            if (Isval == 1)
            {
                Isval = 1;
            }
            else
            {
                Isval = 0;
            }
            ViewBag.UserDisableFlag = Isval;
            //Session["OrgSearchString"] = SearchString;


            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;
            ViewBag.OrgSearchString = SearchString;
            var neHaulierPagedList = new StaticPagedList<NeHaulierList>(GridListObj, pageNumber, (int)pageSize, (int)totalCount);

            Session["SearchStringNEN"] = null;
            Session["IsvalNEN"] = null;
            if (!string.IsNullOrEmpty(SearchString))
            {
                Session["SearchStringNEN"] = SearchString;
            }
            Session["IsvalNEN"] = Isval;

            return View(neHaulierPagedList);

        }
        #endregion
        #region Disable/EnableHaulierUser
        //Disabling or Enabling the ne user
        public int EnableUser(string userName="", string authKey = "", long KeyId = 0)
        {
            var sessionValues = (UserInfo)Session["UserInfo"];
            int user_ID = 0;
            string ErrMsg = string.Empty;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();

            int success = NENNotificationService.EnableUser(authKey, KeyId);
            if (authKey == "")
            {
                #region Saving updating user in sys_event
                
                movactiontype.SystemEventType = SysEventType.disabled_user;
                movactiontype.UserName = userName;
                string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);

                #endregion
            }
            else
            {
                #region Saving updating user in sys_event
                movactiontype.SystemEventType = SysEventType.enabled_user;
                movactiontype.UserName = userName;
                string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);
                #endregion
            }

            return success;
        }
        #endregion
        #region HaulierLeftPanel
        public ActionResult SearchNeHaulierPanel()
        {
            return PartialView("SearchNeHaulierPanel");
        }
        #endregion
        #region CreateNewNEHaulier
        public ActionResult CreateNeuser(long AuthKeyId = 0, string mode = "edit")
        {

            List<NeHaulierList> GridListObj = new List<NeHaulierList>();

            GridListObj = NENNotificationService.EditNeUser(AuthKeyId);
            if (AuthKeyId == 0)
            {
                ViewBag.AuthKeyId = 0;    //set 0 for create a new user
                ViewBag.mode = mode;    // for edit/view a user
                return PartialView("CreateNeuser");
            }
            else
            {
                ViewBag.AuthKeyId = GridListObj[0].NeAuthKey;    // for editing a user
                ViewBag.AuthKey = GridListObj[0].AuthenticationKey;    // for editing a user
                ViewBag.mode = mode;    // for edit/view a user
                return PartialView("CreateNeuser", GridListObj[0]);
            }


        }
        #endregion
        #region SaveHaulier
        // Edit/create a new ne user
        public int SaveNeUser(string Haulname = "", string authKey = "", string OrgName = "", long NeLimit = 2000, long KeyId = 0)
        {
            if (authKey != "")
            {
                //authKey = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authKey));       //controller side encryption
                authKey = Helpers.EncryptionUtility.Encrypt(authKey, false);
                authKey = authKey.Substring(0, 20);
                authKey = Regex.Replace(authKey, @"[^0-9a-zA-Z]+", "-");
            }
            var sessionValues = (UserInfo)Session["UserInfo"];
            int user_ID = 0;
            string ErrMsg = string.Empty;
            MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();

            int success = NENNotificationService.SaveNeUser(Haulname, authKey, OrgName, NeLimit, KeyId);
            
            #region Saving updating user in sys_event
            movactiontype.SystemEventType = SysEventType.amended_user;
            movactiontype.UserName = Haulname;
            string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);
            #endregion

            return success;
        }
        #endregion
        #region HaulierNameValidation
        //Check Haulier name already exist
        public int HaulierValid(string haulname = "", string orgname = "")
        {

            int success = NENNotificationService.HaulierValid(haulname, orgname);
            return success;
        }

        #endregion
        #region  Main NE Notification page
        /// <summary>
        /// NE_Notification
        /// </summary>
        /// <returns></returns>
        public ActionResult NE_Notification(string NEN_ID = "", string Notificationid = "", string esdal_ref = "", string route = "", string inboxId = "", string inboxItemStat = "")
        {
            try
            {
                long RuteID = 0;
                NEN_ID = NEN_ID.Replace(" ", "+");
                ViewBag.NEN_ID = Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(NEN_ID));
                Notificationid = Notificationid.Replace(" ", "+");
                long notifId = Convert.ToInt64(MD5EncryptDecrypt.DecryptDetails(Notificationid));
                Session["NOTIF_ID"] = notifId;
                route = route.Replace(" ", "+");
                route = MD5EncryptDecrypt.DecryptDetails(route);

                esdal_ref = esdal_ref.Replace(" ", "+");
                esdal_ref = MD5EncryptDecrypt.DecryptDetails(esdal_ref);

                inboxId = inboxId.Replace(" ", "+");
                inboxId = MD5EncryptDecrypt.DecryptDetails(inboxId);
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                int RSNENID = Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(NEN_ID));
                ViewBag.inboxId = inboxId;
                ViewBag.UserId = SessionInfo.UserId;
                inboxItemStat = inboxItemStat.Replace(" ", "+");
                inboxItemStat = MD5EncryptDecrypt.DecryptDetails(inboxItemStat);
                Session["NENINBOX_ITEM_ID"] = inboxId;
                Session["NEN_ID"] = ViewBag.NEN_ID;

                Session["NENesdal_ref"] = esdal_ref;
                int contactId = 0;
                long DOCUMENT_ID = 0;
                contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
                ViewBag.ContactId = contactId != 0 ? contactId : 0;
                //**********Required code*********
                if (SessionInfo.HelpdeskRedirect != "true"|| SessionInfo.HelpdeskRedirect==null)
                {
                    int editOpenStatusFlag = movementsService.EditInboxItemOpenStatus(Convert.ToInt64(inboxId), (long)SessionInfo.OrganisationId);// modified to include organisation id for updating the inbox status as opened
                }
                //***********************************
                //get document id
                ViewBag.routeType = route;
                ViewBag.NotificationID = notifId;
                ViewBag.EsdalRefNum = esdal_ref;
                DOCUMENT_ID = movementsService.GetDocumentID(esdal_ref, SessionInfo.OrganisationId);
                if (DOCUMENT_ID == 0)
                {
                    DOCUMENT_ID = NENNotificationService.GetNENDocumentIdFromInbox(Convert.ToInt64(MD5EncryptDecrypt.DecryptDetails(NEN_ID)), Convert.ToInt64(inboxId), SessionInfo.OrganisationId); //to fetch document id from inbox_items table for collaboration
                }
                ViewBag.DOCUMENT_ID = DOCUMENT_ID != 0 ? DOCUMENT_ID : 0;
                //get data from blog datatype  
                NENGeneralDetails generalDetails = new NENGeneralDetails();
                generalDetails.NotificationId = notifId;
                generalDetails.NENId = RSNENID;
                FetchHaulierContactDet(generalDetails);
                ViewBag.NENEmail = generalDetails.Email;// movement.EmailAddress;
                //get document id
                if (inboxItemStat == "312014")
                {
                    int Update_Status = 0;
                    Update_Status = NENNotificationService.UpdateInboxTypeFirstTime(Convert.ToInt64(inboxId), SessionInfo.OrganisationId);//NEN R2
                }
                ViewBag.NEN_RouteStatus = NENNotificationService.GetRouteStatus(Convert.ToInt32(inboxId), RSNENID, Convert.ToInt32(SessionInfo.UserId));// Reteriving latest route status for Inbox_item_Id
                int IOrgID = Convert.ToInt32(SessionInfo.OrganisationId);
                RuteID = NENNotificationService.GetNENRouteID(Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(NEN_ID)), Convert.ToInt32(inboxId), IOrgID, 'R');
                ViewBag.NENRute_ID = RuteID;
                //List<AppVehicleConfigList> vehicleconfigurationlist = STP.NEN.Persistance.NENNotificationDAO.GetNEN_VehicleList(RuteID);
                List<AppVehicleConfigList> vehicleconfigurationlist = vehicleconfigService.GetNenVehicleList(RuteID);
                if (vehicleconfigurationlist.Count() > 0)
                {
                    ViewBag.VehicleID = vehicleconfigurationlist[0].VehicleId;
                }
                TempData["EncryptNotiId"] = notifId;
                TempData["EncryptRoute"] = route;
                TempData["EncryptEsdalRef"] = esdal_ref;
                TempData["InBoxId"] = inboxId;
                TempData["NENID"] = Convert.ToInt64(MD5EncryptDecrypt.DecryptDetails(NEN_ID));

                TempData.Keep("EncryptNotiId");
                TempData.Keep("EncryptRoute");
                TempData.Keep("EncryptEsdalRef");
                TempData.Keep("InBoxId");
                TempData.Keep("NENID");
                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();
                ImportedRoutelist = routesService.GetPlannedNenRouteList(Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(NEN_ID)), 1, Convert.ToInt32(inboxId), IOrgID);
                ViewBag.IsreturnLeg = false;
                ViewBag.returnLeg_routeID = 0;
                if (ImportedRoutelist.Count > 0)
                    ViewBag.MainRouteDesc = ImportedRoutelist[0].RouteDescription;
                if (ImportedRoutelist.Count > 1)
                {
                    ViewBag.IsreturnLeg = true;
                    ViewBag.returnLeg_routeID = ImportedRoutelist[1].RouteID;
                    ViewBag.ReturnRouteDesc = ImportedRoutelist[1].RouteDescription;
                }
                //---Below block to check IS_MOST_RECENT Notification------------------
                generalDetails = null;
                generalDetails = NENNotificationService.GetGeneralDetail(Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(NEN_ID)), 0);
                if (generalDetails != null)
                {
                    ViewBag.IS_MOST_RECENT = generalDetails.IsMostRecent;
                    Session["ApplicationRevId"] = generalDetails.ContentRefNo;
                    Session["RouteFlag"] = "3";
                    ViewBag.ContentRefNo= generalDetails.ContentRefNo;
                }
                else
                {
                    ViewBag.IS_MOST_RECENT = 0;
                }
                //---------------------------------------------------------------------

                //Quick links
                int quickRef = movementsService.InsertQuickLinkSOA(Convert.ToInt32(SessionInfo.OrganisationId), Convert.ToInt32(inboxId), Convert.ToInt32(SessionInfo.UserId));

                //-----AUDIT LOGS FOR open NEN notification from movement inbox-----------
                #region-------AUDIT LOGS FOR open NEN notification from movement inbox----------------
                if (SessionInfo.UserName != null)
                {
                    #region AUDIT LOGS FOR Select user for Scrutiny
                    AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                    string ErrMsg = string.Empty;
                    auditLogType.NENID = RSNENID > 0 ? RSNENID : 0;
                    if (SessionInfo.UserSchema == UserSchema.Portal)
                    {
                        auditLogType.AuditActionType = AuditActionType.soauser_opens_nen_notif;
                        auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                        auditLogType.NENNotificationNo = esdal_ref;
                        auditLogType.InboxItemId = inboxId;
                        auditLogType.ESDALNotificationNo = esdal_ref;
                        auditLogType.DateTime = DateTime.Now.ToString();
                        string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                        int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                        long auditLogResult = auditLogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.OrganisationId);
                    }

                    #endregion
                }
                #endregion---------------end--------------------------------
                //------END HERE---------------------------------------------------------

                List<OrganisationUser> UserList = NENNotificationService.GetOrg_UserList(SessionInfo.OrganisationId, SessionInfo.UserTypeId,Convert.ToInt64(inboxId), ViewBag.NEN_ID);
                SelectList GridListObjInfo = new SelectList(UserList, "OrganisationUserId", "OrganisationUserName");
                ViewBag.OrgUserDropDown = GridListObjInfo;
                if ((UserList.Count() > 0) && (UserList[0].ScrutinyUserId > 0))
                {
                    ViewBag.User_id = UserList[0].ScrutinyUserId;
                }
                ViewBag.routeCount = ImportedRoutelist.Count;

                return View("NE_Notification");
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NE_Notification, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
        }
        #endregion

        #region Fetch haulier contact details
        public NENGeneralDetails FetchHaulierContactDet(NENGeneralDetails ObjGeneralDet)
        {
            try
            {
                NotificationGeneralDetails objNotificationGeneralDetails = new NotificationGeneralDetails();

                #region retreive data from inbound xml
                objNotificationGeneralDetails = NENNotificationService.GetNENNotifInboundDet(ObjGeneralDet.NotificationId, ObjGeneralDet.NENId);
                //code to retrieve data from inbound notification file
                if (objNotificationGeneralDetails.InboundNotification != null)
                {
                    Byte[] InboundNotification = objNotificationGeneralDetails.InboundNotification;

                    string InboundNotif = Encoding.UTF8.GetString(InboundNotification, 0, InboundNotification.Length);

                    XmlDocument Doc = new XmlDocument();
                    try
                    {
                        Doc.LoadXml(InboundNotif);
                    }
                    catch (System.Xml.XmlException XE)
                    {
                        //Some data is stored in gzip format, so we need to unzip then load it.
                        InboundNotification = STP.Common.General.XsltTransformer.Trafo(objNotificationGeneralDetails.InboundNotification);

                        InboundNotif = Encoding.UTF8.GetString(InboundNotification, 0, InboundNotification.Length);

                        Doc.LoadXml(@InboundNotif);
                    }
                    //retreive max no of pieces
                    XmlNodeList parentNode = Doc.GetElementsByTagName("LoadDetails");
                    foreach (XmlNode ChildrenNode in parentNode)
                    {
                        try
                        {
                            ObjGeneralDet.MaximamPiecesPerLoad = Convert.ToInt32(ChildrenNode["TotalMoves"].InnerText);
                        }
                        catch
                        {
                            #region MaxPiecesPerMove For vehicles that do not carry a load, for example mobile cranes
                            // A description of the load and the number of movements required.  For vehicles that are moving loads (Special 
                            // Orders, AILs and C&amp;U) the load description and max pieces per move are mandatory, though this is not 
                            // enforced by the schema.  For vehicles that do not carry a load, for example mobile cranes, load description
                            // and max pieces per move should be omitted.  'Total Moves', the number of journeys to be made must always 
                            // be present.
                            ObjGeneralDet.MaximamPiecesPerLoad = 0;
                            #endregion
                        }
                    }
                    //retreive Contact name, Email, Telephone, haulier licence
                    parentNode = Doc.GetElementsByTagName("HaulierDetails");
                    foreach (XmlNode ChildrenNode in parentNode)
                    {
                        try
                        {
                            ObjGeneralDet.HaulierOprLicence = ChildrenNode["Licence"].InnerText;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.HaulierOprLicence = "";
                        }
                        try
                        {
                            ObjGeneralDet.ClientName = ChildrenNode["HaulierContact"].InnerText;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.ClientName = "";
                        }
                        try
                        {
                            ObjGeneralDet.Email = ChildrenNode["EmailAddress"].InnerText;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.Email = "";
                        }
                        try
                        {
                            ObjGeneralDet.Telephone = ChildrenNode["TelephoneNumber"].InnerText;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.Telephone = "";
                        }
                        try
                        {
                            ObjGeneralDet.HaulierOrganisationName = ChildrenNode["HaulierName"].InnerText;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.HaulierOrganisationName = "";
                        }
                    }
                    //Retreive Company address
                    parentNode = Doc.GetElementsByTagName("HaulierAddress");
                    foreach (XmlNode ChildNode in parentNode)
                    {
                        try
                        {

                            ObjGeneralDet.HaulierAddress1 = (parentNode[0]["Line"] != null) == true ? parentNode[0]["Line"].InnerText : string.Empty;
                            ObjGeneralDet.HaulierAddress2 = (parentNode[0]["Line"] != null && parentNode[0]["Line"].NextSibling != null) == true ? parentNode[0]["Line"].NextSibling.InnerText : string.Empty;
                            ObjGeneralDet.HaulierAddress3 = (parentNode[0]["Line"] != null && parentNode[0]["Line"].NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling != null) == true ? parentNode[0]["Line"].NextSibling.NextSibling.InnerText : string.Empty;
                            ObjGeneralDet.HaulierAddress4 = (parentNode[0]["Line"] != null && parentNode[0]["Line"].NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling.NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling.NextSibling.Name == "Line") == true ? parentNode[0]["Line"].NextSibling.NextSibling.NextSibling.InnerText : string.Empty;
                            ObjGeneralDet.HaulierAddress5 = (parentNode[0]["Line"] != null && parentNode[0]["Line"].NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling.NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling.NextSibling.NextSibling != null && parentNode[0]["Line"].NextSibling.NextSibling.NextSibling.NextSibling.Name == "Line") == true ? parentNode[0]["Line"].NextSibling.NextSibling.NextSibling.NextSibling.InnerText : string.Empty;

                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.HaulierAddress = "";
                        }
                    }
                    //retreive OnBehalfOf
                    parentNode = Doc.GetElementsByTagName("InboundNotification");

                    foreach (XmlNode node in parentNode)
                    {
                        try
                        {
                            ObjGeneralDet.OnBehalf = node["OnBehalfOf"] != null ? node["OnBehalfOf"].InnerText : string.Empty;
                        }

                        catch (Exception e)
                        {
                            ObjGeneralDet.OnBehalf = "";
                        }
                        try
                        {
                            ObjGeneralDet.NotesOnEscort = node["NotificationNotesFromHaulier"] != null ? node["NotificationNotesFromHaulier"].InnerText : string.Empty;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.NotesOnEscort = "";
                        }
                        try
                        {
                            ObjGeneralDet.MyReference = node["HauliersReference"] != null ? node["HauliersReference"].InnerText : string.Empty;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.MyReference = "";
                        }
                        try
                        {
                            ObjGeneralDet.OtherContactDetails = node["OtherContactDetails"] != null ? node["OtherContactDetails"].InnerText : string.Empty;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.OtherContactDetails = "";
                        }
                        try
                        {
                            ObjGeneralDet.VSONumber = node["DftReference"] != null ? node["DftReference"].InnerText : string.Empty;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.VSONumber = "";
                        }
                        try
                        {
                            ObjGeneralDet.VR1Number = node["VR1Number"] != null ? node["VR1Number"].InnerText : string.Empty;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.VR1Number = "";
                        }
                        try
                        {
                            ObjGeneralDet.SONumbers = node["SONumber"] != null ? node["SONumber"].InnerText : string.Empty;
                        }
                        catch (Exception e)
                        {
                            ObjGeneralDet.SONumbers = "";
                        }
                    }
                    // Added new code
                    //ObjGeneralDet.IndemnityConfirmation = Doc.GetElementsByTagName("IndemnityConfirmation") == null ? string.Empty : (Doc.GetElementsByTagName("IndemnityConfirmation")[0] == null ? string.Empty : (Doc.GetElementsByTagName("IndemnityConfirmation")[0].InnerText == null || Doc.GetElementsByTagName("IndemnityConfirmation")[0].InnerText == string.Empty ? "No" : (Doc.GetElementsByTagName("IndemnityConfirmation")[0].InnerText == "true" ? "Yes" : "No")));
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/FetchHaulierContactDet, Exception: {0}", ex));
            }
            return ObjGeneralDet;
        }
        #endregion

        public ActionResult NENotificationAssignPopUp(long inboxId = 0, long NenID = 0)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                List<OrganisationUser> UserList = NENNotificationService.GetOrg_UserList(SessionInfo.OrganisationId, SessionInfo.UserTypeId, inboxId, NenID);
                SelectList GridListObjInfo = new SelectList(UserList, "OrganisationUserId", "OrganisationUserName");
                ViewBag.OrgUserDropDown = GridListObjInfo;
                if ((UserList.Count() > 0) && (UserList[0].ScrutinyUserId > 0))
                {
                    ViewBag.User_id = UserList[0].ScrutinyUserId;
                }
                //--------------------
                return PartialView("NENotificationAssignPopUp");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NENotif_leftPanel, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #region  Display NENotification
        /// <summary>
        /// Display_NENotification
        /// </summary>
        /// <returns></returns>
        public ActionResult Display_NENotification(int NEN_ID, int Route_Id, long Notif_Id = 0, long Inbox_ItemID = 0, long NENVehicleID = 0)
        {
            NENGeneralDetails generalDetails = null;
            UserInfo SessionInfo = null;
            try
            {

                SessionInfo = (UserInfo)Session["UserInfo"];
                int IUserID = Convert.ToInt32(SessionInfo.UserId);
                NENHaulierRouteDesc haulierRouteDesc = null;
                haulierRouteDesc = NENNotificationService.GetHualierRouteDesc(NEN_ID, (int)Inbox_ItemID);

               
                generalDetails = NENNotificationService.GetGeneralDetail(NEN_ID, Route_Id);
                if (Notif_Id > 0)
                {
                    generalDetails.NotificationId = Notif_Id;
                    generalDetails.OrganisationId = SessionInfo.OrganisationId;
                    generalDetails.NENId = NEN_ID;
                    if (haulierRouteDesc != null)
                    {
                        generalDetails.DescriptionReturnLeg = haulierRouteDesc.HualierDescriptionReturnLeg;
                    }
                    //FetchHaulierContactDet(generalDetails);
                    if (Inbox_ItemID > 0)
                    {
                        generalDetails.InboxItemId = Inbox_ItemID;
                        FetchRouteDescription(NEN_ID, generalDetails);
                    }
                }
                ViewBag.HauliEmail = generalDetails.Email;
                //SaveAuditLogs(AuditActionType.user_opens_general_tab);
                if (generalDetails.IndemnityConfirmation == 1)
                {
                    generalDetails.Indemnity = "Yes";
                }
                else
                {
                    generalDetails.Indemnity = "No";
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/Display_NENotification, Exception: {0}", ex));
                return PartialView("Display_NENotification", generalDetails);
            }
            return PartialView("Display_NENotification", generalDetails);
        }

        private NENGeneralDetails FetchRouteDescription(int NEN_ID, NENGeneralDetails generalDetails)
        {
            try
            {
                List<NENGeneralDetails> objRouteDescription = new List<NENGeneralDetails>();
                objRouteDescription = NENNotificationService.GetNENRouteDescription(NEN_ID, generalDetails.InboxItemId, generalDetails.OrganisationId);
                if (objRouteDescription.Count() > 0)
                {
                    if (objRouteDescription[0].RoutePartNo == 1)
                    {
                        generalDetails.GRouteDescription = objRouteDescription[0].GRouteDescription;
                        List<NENGeneralDetails> ObjRouteFromTODescp = NENNotificationService.GetRouteFromAndToDescp(objRouteDescription[0].RoutePartIdS);
                        if (ObjRouteFromTODescp.Count() > 0)
                        {
                            if (ObjRouteFromTODescp[0].RoutePartNo == 1)
                            {
                                generalDetails.FromAddress = ObjRouteFromTODescp[0].From; //Route part no 1 description
                            }
                            if (ObjRouteFromTODescp[1].RoutePartNo == 2)
                            {
                                generalDetails.ToAddress = ObjRouteFromTODescp[1].From; //Route part no 2 description
                            }
                        }
                    }
                    if (objRouteDescription.Count() > 1)
                    {
                        if (objRouteDescription[1].RoutePartNo == 2)
                        {
                            generalDetails.DescriptionReturnLeg = objRouteDescription[1].GRouteDescription;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/Display_NENotification, Exception: {0}", ex));
            }
            return generalDetails;
        }
        #endregion

        #region
        public JsonResult GetRouteAssessmentStatus(int InboxId = 0, int USER_ID = 0, int NEN_ID = 0)
        {
            try
            {
                int Route_Status = 0;
                Route_Status = NENNotificationService.GetRouteStatus(InboxId, NEN_ID, USER_ID);
                return Json(new { result = Route_Status });
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/GetRouteAssessmentStatus, Exception: {0}", ex));
                return Json("Error", "Home");
            }


        }
        #endregion

        #region GetBothRouteAssessmentStatus
        public JsonResult GetBothRouteAssessmentStatus(int InboxId = 0, int USER_ID = 0, int NEN_ID = 0)
        {
            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                else
                {
                    RedirectToAction("Account", "LogOut");
                }

                List<NENRouteStatusList> Route_Status = NENNotificationService.GetNENBothRouteStatus(InboxId, NEN_ID, USER_ID, SessionInfo.OrganisationId);
                //----------below code to update latest route details to Notification From and To descr------------------------
                try
                {
                    List<NENGeneralDetails> objRouteDescription = new List<NENGeneralDetails>();
                    objRouteDescription = NENNotificationService.GetNENRouteDescription(NEN_ID, InboxId, SessionInfo.OrganisationId);
                    if (objRouteDescription.Count() > 0)
                    {
                        if (objRouteDescription[0].RoutePartNo == 1)
                        {
                            List<NENGeneralDetails> ObjRouteFromTODescp = NENNotificationService.GetRouteFromAndToDescp(objRouteDescription[0].RoutePartIdS);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/GetBothRouteAssessmentStatus, Exception: {0}", ex));
                }
                //--------------------------------------------------------------------------------------------------------------
                return Json(new { result = Route_Status });
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/GetBothRouteAssessmentStatus, Exception: {0}", ex));
                return Json("Error", "Home");
            }


        }
        #endregion

        #region
        public JsonResult UpdateRouteAssessmentStatus(int InboxId = 0, int USER_ID = 0, int Route_ID = 0, int routeStatus = 0, bool IsmainRoute = true)
        {
            try
            {
                int Update_Status = 0;
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                Update_Status = NENNotificationService.UpdateRouteStatus(InboxId, USER_ID, Route_ID, routeStatus, SessionInfo.OrganisationId);

                if (routeStatus == 911002 || routeStatus == 911010)
                    SaveAuditLogs(AuditActionType.soauser_plan_nen_route, IsmainRoute);
                else if (routeStatus == 911003)
                    SaveAuditLogs(AuditActionType.soauser_planing_error_nen_route, IsmainRoute);
                //else if (routeStatus == 911004)
                //    SaveAuditLogs(AuditActionType.soauser_replan_nen_route);
                else if (routeStatus == 911005)
                    SaveAuditLogs(AuditActionType.soauser_assigned_scru_unplanned, IsmainRoute);

                return Json(new { result = Update_Status });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/UpdateRouteAssessmentStatus, Exception: {0}", ex.Message));
                return Json(new { result = "erro" });
            }
        }
        #endregion

        #region Get Route Points
        public List<RoutePointJson> Get_Route_Points(int PlanRouteID)
        {
            List<RoutePointJson> RLObjjson = new List<RoutePointJson>();
            try
            {
                List<RoutePoint> RLObj = routesService.GetRoutePointsDetails(PlanRouteID);

                string rson = null;

                if (RLObj.Count > 0)
                {
                    rson = JsonConvert.SerializeObject(RLObj);
                    RLObjjson = JsonConvert.DeserializeObject<List<RoutePointJson>>(rson);
                    ViewBag.ImportedRoutelist = RLObjjson;
                }
                Session["RoutePoint"] = RLObjjson;
                //return Json(new { result = RLObjjson });
                return RLObjjson;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/Get_Route_Points, Exception: {0}", ex));
                //return Json(new { result = "error" });
                return RLObjjson;
            }
        }
        #endregion

        public NenRouteModel GetAddressNEN(bool Isretunleg = false)
        {
            int PCount = 0;
            List<RoutePointJson> RLObj = (List<RoutePointJson>)Session["RoutePoint"];
            List<RoutePointJson> ReturnRLObj = null;
            List<ErrorList> Obj_err = new List<ErrorList>();
            List<NonErrorList> Obj_NONerr = new List<NonErrorList>();
            if (RLObj != null)
            {
                 PCount = RLObj.Count;
            }
            int WaypointCount = 0;
            int errorHigh = 174;
            //if (PCount == 0)
            //{
            //    sdogeometry gm ;//= { MDSYS.SDO_GEOMETRY(2001, 27700, MDSYS.SDO_POINT_TYPE(583512.279662547, 209086.477980235), null, null) };
            //    RLObj.Add(new RoutePoint { pointDescr = "4 Grays Cottages, Maldon Road, Hatfield Peverel, CHELMSFORD, CM3 2JP", pointType = 239001, showRoutePoint = 1, pointGeom = {MDSYS.SDO_GEOMETRY(2001, 27700, MDSYS.SDO_POINT_TYPE(583512.279662547, 209086.477980235), null, null) }});
            //    RLObj.Add(new RoutePoint { pointDescr = "Unit 4, Houldings Garage, Colchester Road, Heybridge, MALDON, Essex, CM9 4HH", pointType = 239002, showRoutePoint = 1 });
            //    RLObj.Add(new RoutePoint { pointDescr = "HATFIELD ROAD, CM9 6", pointType = 239003, showRoutePoint = 1 });


            //}
            string moniker = "", searchKeyword = "", errormoniker = "", err_Data = "";
            int j = 1, wayPoint = 1;
            bool QasdifferFlag = true;// Flag set for to identify the haulier provided address and Qas search results are different 
            bool IsStartEnd_error = false;
            List<AddrDetails> addrList = new List<AddrDetails>();
            AddrDetails addDetails = new AddrDetails();
            try
            {

                for (int p = 0; p < PCount; p++)
                {
                    if (RLObj[p].pointType == 239003)
                    {
                        WaypointCount = WaypointCount + 1;
                    }

                }

                int Ipoint = 0;
                for (int i = 0; i < PCount; i++)
                {
                    searchKeyword = RLObj[i].pointDescr;
                    var QasErrorFlag = 0;  // flag created for catching the exception from qas and exclude the insertion steps
                    if (searchKeyword != "" || searchKeyword != null)
                    {
                        if (i != 0)
                            addrList.Clear();
                        if (!validateIsCoordinates(searchKeyword))
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , POST,QAS/Search, keyword: {1}", Session.SessionID, searchKeyword));
                            addrList = qasService.Search(searchKeyword);

                            if (addrList.Count >= 1)
                            {
                                if (addrList.Count == 1)
                                {
                                    if (RLObj[i].pointDescr.Length == addrList[0].AddressLine.Length)
                                    {
                                        QasdifferFlag = false;

                                    }
                                    RLObj[i].pointDescr = addrList[0].AddressLine;   //Route point description set to qas search result instead of Haulier supplied address RM#10459

                                    moniker = addrList[0].Moniker;
                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}] , POST,QAS/GetAddress, moniker: {1}", Session.SessionID, moniker));
                                    try
                                    {
                                        addDetails = qasService.GetAddress(moniker);
                                    }
                                    catch
                                    {
                                        LocationNotFoundErrorIdentifier(RLObj, Obj_err, ref errorHigh, ref errormoniker, j, ref wayPoint, ref IsStartEnd_error, i, err_Data);
                                        QasErrorFlag = 1;
                                    }
                                    if (QasErrorFlag != 1)
                                    {
                                        moniker = "find";

                                        RLObj[i].Easting = addDetails.Easting;
                                        RLObj[i].Northing = addDetails.Northing;
                                        if (RLObj[i].pointType == 239003)
                                        {
                                            Ipoint = 2;
                                            wayPoint++;
                                        }
                                        else if (RLObj[i].pointType == 239001)
                                            Ipoint = 0;
                                        else if (RLObj[i].pointType == 239002)
                                            Ipoint = 1;

                                        Obj_NONerr.Add(new NonErrorList { Point = Ipoint, Northing = addDetails.Northing, Easting = addDetails.Easting, Address = searchKeyword, PointIndex = wayPoint - 1, Moniker = addrList[0].Moniker, DifferentQASSearchFlag = QasdifferFlag });
                                    }
                                }
                                else
                                {
                                    if (errormoniker != "")
                                    {
                                        errorHigh = errorHigh + 16;
                                        errormoniker = errormoniker + " </br>";
                                    }

                                    else
                                        errormoniker = "The route(s) could not be planned due to the following reasons : </br></br>";
                                    if (RLObj[i].pointType == 239001)
                                    {
                                        errormoniker = errormoniker + j + ". Multiple locations found for the start address.";
                                        Obj_err.Add(new ErrorList { Point = "startPoint", AddIndex = 0, Error = "MultipleLoc" });
                                        IsStartEnd_error = true;
                                    }
                                    else if (RLObj[i].pointType == 239002)
                                    {
                                        errormoniker = errormoniker + j + ". Multiple locations found for the end address.";
                                        Obj_err.Add(new ErrorList { Point = "endPoint", AddIndex = 0, Error = "MultipleLoc" });
                                        IsStartEnd_error = true;
                                    }
                                    else if (RLObj[i].pointType == 239003)
                                    {
                                        errormoniker = errormoniker + j + ". Multiple locations found for way point " + wayPoint + ".";
                                        Obj_err.Add(new ErrorList { Point = "wayPoint", AddIndex = wayPoint, Error = "MultipleLoc" });
                                        wayPoint++;
                                    }
                                    j++;
                                    err_Data = err_Data + RLObj[i].pointType;

                                }
                            }

                            else
                            {
                                if (errormoniker != "")
                                {
                                    errorHigh = errorHigh + 16;
                                    errormoniker = errormoniker + " </br>";
                                }
                                else
                                    errormoniker = "The route(s) could not be planned due to the following reasons : </br></br>";
                                if (RLObj[i].pointType == 239001)
                                {
                                    errormoniker = errormoniker + j + ". Location not found for the start address.";
                                    Obj_err.Add(new ErrorList { Point = "startPoint", AddIndex = 0, Error = "Not found" });
                                    IsStartEnd_error = true;
                                }
                                else if (RLObj[i].pointType == 239002)
                                {
                                    errormoniker = errormoniker + j + ". Location not found for the end address.";
                                    Obj_err.Add(new ErrorList { Point = "endPoint", AddIndex = 0, Error = "Not found" });
                                    IsStartEnd_error = true;
                                }
                                else if (RLObj[i].pointType == 239003)
                                {
                                    errormoniker = errormoniker + j + ". Location not found for way point " + wayPoint + ".";
                                    Obj_err.Add(new ErrorList { Point = "wayPoint", AddIndex = wayPoint, Error = "Not found" });
                                    wayPoint++;
                                }

                                j++;
                                err_Data = err_Data + RLObj[i].pointType;

                            }
                        }
                        else
                        {
                            IList<string> Clops = searchKeyword.Split(',').ToList<string>();
                            Clops.Reverse();
                            RLObj[i].Easting = Convert.ToInt64(Clops[0]);//Est
                            RLObj[i].Northing = Convert.ToInt64(Clops[1]);//Nor
                            if (RLObj[i].pointType == 239003)
                            {
                                Ipoint = 2;
                                wayPoint++;
                            }
                            else if (RLObj[i].pointType == 239001)
                                Ipoint = 0;
                            else if (RLObj[i].pointType == 239002)
                                Ipoint = 1;

                            Obj_NONerr.Add(new NonErrorList { Point = Ipoint, Northing = RLObj[i].Northing, Easting = RLObj[i].Easting, Address = searchKeyword, PointIndex = wayPoint - 1 });
                        }

                    }//if data nuul
                    else
                    {
                        if (errormoniker != "")
                        {
                            errormoniker = errormoniker + " </br>";
                            errorHigh = errorHigh + 16;
                        }
                        else
                            errormoniker = "The route(s) could not be planned due to the following reasons : </br></br>";
                        if (RLObj[i].pointType == 239001)
                        {
                            errormoniker = errormoniker + j + ". Address/Postcode is not provided for the start address.";
                            Obj_err.Add(new ErrorList { Point = "startPoint", AddIndex = 0, Error = "No address/postcode" });
                            IsStartEnd_error = true;
                        }
                        else if (RLObj[i].pointType == 239002)
                        {
                            errormoniker = errormoniker + j + ". Address/Postcode is not provided for the end address.";
                            Obj_err.Add(new ErrorList { Point = "endPoint", AddIndex = 0, Error = "No address/postcode" });
                            IsStartEnd_error = true;
                        }
                        else if (RLObj[i].pointType == 239003)
                        {
                            errormoniker = errormoniker + j + ". Address/Postcode is not provided for way point " + wayPoint + ".";
                            Obj_err.Add(new ErrorList { Point = "wayPoint", AddIndex = wayPoint, Error = "No address/postcode" });
                            wayPoint++;
                        }

                        j++;
                        err_Data = err_Data + RLObj[i].pointType;

                    }
                }
                if (errormoniker == "")
                    errormoniker = "find";
                else
                {
                    errorHigh = errorHigh + 16;
                    errormoniker = errormoniker + "***<br/> <br/> Please plan the route(s) from the map correcting the errors listed above.";
                    if (Isretunleg == true)
                    {
                        if (IsStartEnd_error == true)
                        {
                            //errorHigh = errorHigh + 16;
                            // errormoniker = errormoniker + "<br/> Return leg not planned as Start/end of main route has error. Please plan the main route to continue.";
                            IsStartEnd_error = false;
                        }
                        else
                        {
                            ReturnRLObj = RLObj;
                            int po = 0;
                            for (int i = 0; i < PCount; i++)
                            {
                                if (RLObj[po].pointType == 239003)
                                {
                                    RLObj.RemoveAt(po);

                                }
                                else
                                    po++;
                            }
                            IsStartEnd_error = true;
                        }
                    }
                    else { IsStartEnd_error = false; }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/GetAddressNEN, Exception: {0}", ex.Message));
            }
            NenRouteModel nenRouteModel = new NenRouteModel()
            {
                RoutePointJsons = RLObj,
                IsStartEndError = IsStartEnd_error,
                ErrorMoniker = errormoniker,
                ErrorLists = Obj_err,
                NonErrorLists = Obj_NONerr,
                WaypointCount=WaypointCount,
                ErrorHigh=errorHigh
            };

            return nenRouteModel;
            //return Json(new { resultroutePart = RLObj, IsreturnRoutePart = IsStartEnd_error, result = errormoniker, Obj_err = Obj_err, Obj_NONerr = Obj_NONerr, WaypointCount = WaypointCount, errorHigh = errorHigh });
        }

        static bool validateIsCoordinates(string KeyWord)
        {
            bool res = false;
            int n = 0; bool isNumeric;
            IList<string> Clops = KeyWord.Split(',').ToList<string>();
            Clops.Reverse();
            if (Clops.Count == 2)
            {
                isNumeric = int.TryParse(Clops[0].ToString(), out n);
                res = int.TryParse(Clops[1].ToString(), out n);
                if (isNumeric == true && res == true) { res = true; } else { res = false; }
            }
            return res;
        }

        private static void LocationNotFoundErrorIdentifier(List<RoutePointJson> RLObj, List<ErrorList> Obj_err, ref int errorHigh, ref string errormoniker, int j, ref int wayPoint, ref bool IsStartEnd_error, int i, string err_Data)
        {
            if (errormoniker != "")
            {
                errorHigh = errorHigh + 16;
                errormoniker = errormoniker + " </br>";
            }
            else
                errormoniker = "The route(s) could not be planned due to the following reasons : </br></br>";
            if (RLObj[i].pointType == 239001)
            {
                errormoniker = errormoniker + j + ". Location not found for the start address.";
                Obj_err.Add(new ErrorList { Point = "startPoint", AddIndex = 0, Error = "Not found" });
                IsStartEnd_error = true;
            }
            else if (RLObj[i].pointType == 239002)
            {
                errormoniker = errormoniker + j + ". Location not found for the end address.";
                Obj_err.Add(new ErrorList { Point = "endPoint", AddIndex = 0, Error = "Not found" });
                IsStartEnd_error = true;
            }
            else if (RLObj[i].pointType == 239003)
            {
                errormoniker = errormoniker + j + ". Location not found for way point " + wayPoint + ".";
                Obj_err.Add(new ErrorList { Point = "wayPoint", AddIndex = wayPoint, Error = "Not found" });
                wayPoint++;
            }
            j++;
            err_Data = err_Data + RLObj[i].pointType;
        }


        #region Audit log recording for all notifications
        public void SaveAuditLogs(AuditActionType VAuditActionType, bool IsmainRoute = true)
        {
            try
            {
                string EsdalReference = Convert.ToString(Session["NENesdal_ref"]);
                int InboxId = Convert.ToInt32(Session["NENINBOX_ITEM_ID"]);
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                string ErrMsg = string.Empty;
                auditLogType.NENID = Convert.ToInt64(SessionInfo.UserId);
                if (SessionInfo.UserSchema == UserSchema.Portal)
                {
                    string RN = "NEN Route (Return)";
                    if (IsmainRoute == true)
                        RN = "NEN Route";
                    //auditLogType.auditActionType = AuditActionType.soauser_set_user_for_scrutiny;
                    auditLogType.NotificationID = Convert.ToInt32(Session["NOTIF_ID"]);
                    auditLogType.AuditActionType = VAuditActionType;
                    auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                    auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                    auditLogType.NENNotificationNo = EsdalReference;
                    auditLogType.ESDALNotificationNo = EsdalReference;
                    auditLogType.InboxItemId = Convert.ToString(InboxId);
                    auditLogType.NENToScrutinyUser = SessionInfo.UserName;
                    auditLogType.DateTime = DateTime.Now.ToString();
                    auditLogType.NENRouteName = RN;
                    string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                    int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                    long auditLogResult = auditLogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.OrganisationId);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/SaveAuditLogs, Exception: {0}", ex.Message));
            }
        }
        #endregion

        #region Assign user for scrutiny while processing NEN
        public ActionResult NENAssignUserForScrutiny(MovementModel movement)
        {
            long RouteID = 0, returnLeg_routeID = 0;
            bool res = false;
            UserInfo SessionInfo = null;
            try
            {
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    movement.LoginUserId = Convert.ToInt64(SessionInfo.UserId);
                    movement.OrganisationId = Convert.ToInt64(SessionInfo.OrganisationId);
                }
                long LatestRouteStatus = NENNotificationService.GetRouteStatus((int)movement.InboxId, 0, Convert.ToInt32(SessionInfo.UserId));// Reteriving latest route status for Inbox_item_Id, this is main route status.
                long NewStatus = 911005;
                switch (LatestRouteStatus)
                {
                    case 911001://Unplanned
                        NewStatus = 911005;//Assign User For Scrutiny Unplanned Route Status
                        break;
                    case 911002://Planned
                        NewStatus = 911010;//Assign User For Scrutiny planned Route Status
                        break;
                    case 911003://Planning Error
                        NewStatus = 911005;//Assign User For Scrutiny Unplanned Route Status
                        break;
                    case 911004://Replanned
                        NewStatus = 911010;//Assign User For Scrutiny planned Route Status
                        break;
                    case 911005://Assigned for scrutiny unplanned
                        NewStatus = 911005;//Assign User For Scrutiny Unplanned Route Status
                        break;
                    case 911010://Assigned for scrutiny planned
                        NewStatus = 911010;//Assign User For Scrutiny planned Route Status
                        break;
                }
                movement.RouteStatus = NewStatus;
                res = NENNotificationService.SP_SAVE_NEN_USER_FOR_SCRUTINY(movement);
                if (res == true)
                {
                    //-----AUDIT LOGS FOR Select user for Scrutiny-----------
                    #region-------AUDIT LOGS FOR Select user for Scrutiny----------------
                    if (SessionInfo.UserName != null)
                    {
                        #region AUDIT LOGS FOR Select user for Scrutiny
                        AuditLogIdentifiers auditLogType = new AuditLogIdentifiers();
                        string ErrMsg = string.Empty;
                        auditLogType.NENID = movement.NenId > 0 ? movement.NenId : 0;
                        if (SessionInfo.UserSchema == UserSchema.Portal)
                        {
                            auditLogType.HelpDeskUserID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            auditLogType.HelpDeskUsername = SessionInfo.HelpdeskUserName;
                            auditLogType.NENNotificationNo = movement.ESDALReference;
                            auditLogType.RouteStatus = convertStatusMessage(movement.RouteStatus);
                            if (auditLogType.RouteStatus == "Assigned for scrutiny unplanned")
                            {
                                auditLogType.AuditActionType = AuditActionType.soauser_assigned_scru_unplanned;
                                auditLogType.RouteStatus = "Unplanned";
                            }
                            else
                            {
                                auditLogType.AuditActionType = AuditActionType.soauser_set_user_for_scrutiny;
                                auditLogType.RouteStatus = "Planned";
                            }
                            auditLogType.NENFromUser = movement.FromUserName;
                            auditLogType.NENToScrutinyUser = movement.UserName;
                            auditLogType.InboxItemId = Convert.ToString(movement.InboxId);
                            auditLogType.ESDALNotificationNo = movement.ESDALReference;
                            auditLogType.DateTime = DateTime.Now.ToString();
                            string auditLogDescp = AuditLog.GetNENNotifAuditLog(SessionInfo, auditLogType, out ErrMsg);
                            int user_ID = Convert.ToInt32(SessionInfo.HelpdeskUserId);
                            long auditLogResult = auditLogService.SaveNotifAuditLog(auditLogType, auditLogDescp, user_ID, SessionInfo.OrganisationId);
                        }

                        #endregion
                    }
                    #endregion---------------end--------------------------------
                    //------END HERE---------------------------------------------------------
                }

                long NEN_ID = movement.NenId > 0 ? movement.NenId : 0;
                int inboxId = Convert.ToInt32(movement.InboxId);
                List<AppRouteList> ImportedRoutelist = new List<AppRouteList>();
                int IOrgID = Convert.ToInt32(SessionInfo.OrganisationId);
                ImportedRoutelist = routesService.GetPlannedNenRouteList(Convert.ToInt32(NEN_ID), 1, inboxId, IOrgID);
                if (ImportedRoutelist.Count > 1)
                {
                    returnLeg_routeID = ImportedRoutelist[1].RouteID;
                }
                RouteID = NENNotificationService.GetNENRouteID(Convert.ToInt32(NEN_ID), inboxId, IOrgID, 'R');

            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NENAssignUserForScrutiny, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
            return Json(new { result = res, NENRute_ID = RouteID, returnLeg_routeID = returnLeg_routeID });
        }

        private string convertStatusMessage(long statusNo)
        {
            string resultStatus = "";
            try
            {
                switch (statusNo)
                {
                    case 911005:
                        resultStatus = "Assigned for scrutiny unplanned";
                        break;
                    case 911010:
                        resultStatus = "Assigned for scrutiny planned";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultStatus;
        }
        #endregion    }

        public ActionResult hualierRouteInfo(int InboxitemId, int NEN_ID, bool IsReturleg, string routeDesc = "", string fromAddress = "", string toAddress = "")
        {
            ViewBag.IsReturleg = IsReturleg;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            NENHaulierRouteDesc haulierRouteDesc = null;
            haulierRouteDesc = NENNotificationService.GetHualierRouteDesc(NEN_ID, InboxitemId, routeDesc, fromAddress, toAddress);

            return PartialView("hualierRouteInfo", haulierRouteDesc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <param name="Inbox_ID"></param>
        /// <param name="AnalysisId"></param>
        /// <param name="ContentRefNo"></param>
        /// <returns></returns>
        public JsonResult NENRouteAnalysis(string NotificationId = "", int Inbox_ID = 0, int AnalysisId = 0, string ContentRefNo = "", string ESDALRefNo = "")
        {
            var result = false;
            string errorCode = "";
            int orgId = 0;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo == null)
            {
                RedirectToAction("Logout", "Account");
            }
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}], NENNotificationController/NENRouteAnalysis Notification Id={1}", Session.SessionID, NotificationId));

                string errormsg = null, path = null;

                int updated = 0;

                ViewBag.DrivingInstructionError = "";
                int notifId = 0;
                if (NotificationId != "")
                    notifId = Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(NotificationId));

                orgId = (int)SessionInfo.OrganisationId;
                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
                NenAnalysedData nenAnalysedData = new NenAnalysedData(Inbox_ID, orgId, notifId, SessionInfo.UserSchema);
                //List<int> RouteAnalysis = new List<int>();
                bool RouteAnalysis = false;
                try
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("NENNotificationController/NENRouteAnalysis performing route analysis Notification Id={0}", notifId));
                    RouteAnalysis = PerformIndividualRouteAnalysis(notifId, nenAnalysedData, Session.SessionID);
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotificationController/NENRouteAnalysis Exception while performing route analysis Notification Id={0}, Error={1}", notifId, ex.Message));
                    int confirmVal = 1;
                    Session["ConfirmRouteVeh"] = confirmVal;
                    return Json(new { result = false });
                }

                if (!RouteAnalysis)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("NENNotificationController/NENRouteAnalysis route analysis couldent complete Notification Id={0}", notifId));
                    int confirmVal = 0;
                    Session["ConfirmRouteVeh"] = confirmVal;
                    return Json(new { result = false });
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("NENNotificationController/NENRouteAnalysis route analysis completed Notification Id={0}", notifId));
                    #region NENNotificationStructureUpdation
                    FuncNENNotifStructureUpdation(notifId, Inbox_ID);
                    #endregion

                    int confirmVal = 1;
                    Session["ConfirmRouteVeh"] = confirmVal;

                    return Json(new { result = true });
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/RouteAnalysis_SimpleNotif, Exception: {0}", ex));
                int confirmVal = 1;
                Session["ConfirmRouteVeh"] = confirmVal;
                return Json(new { result = false });
            }
        }

        private bool PerformIndividualRouteAnalysis(int notifId, NenAnalysedData nenAnalysedData, string sessionId = "")
        {
            //Norification functionality
            try
            {
                List<RoutePartDetails> routePartDetails = routesService.GetNenPdfRoutesForAnalysis(nenAnalysedData.InboxId, (int)nenAnalysedData.OrganisationId, UserSchema.Portal);
                List<ContactModel> contactModels = organizationService.GetNenAffectedOrganisationDetails(nenAnalysedData.InboxId);
                RouteAssessmentModel routeAssessmentModel = routeAssessmentService.GenerateNenRouteAssessment(routePartDetails, routePartDetails[0].AnalysisId, contactModels, true);
                return true;
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotificationController/PerformRouteAnalysis Error generating route analysis {2} , Notification Id={0}", nenAnalysedData.InboxId, ex.Message));
                return false;
            }
        }
        private bool PerformRouteAnalysis(object state)
        {

            NenAnalysedData nen = (NenAnalysedData)state;
            try
            {
                int updated = routeAssessmentService.UpdatedNenAssessmentDetails(nen.NotificationId, nen.InboxId, nen.AnalysisType, nen.OrganisationId, nen.Schema);//Driving instruction  route description 
                return true;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotificationController/PerformRouteAnalysis Error generating route analysis {2} for {1} , Notification Id={0}", nen.InboxId, ex.Message, nen.getAnalysisType()));
                return false;
            }

        }
        /// <summary>
        /// controller to update the ica status
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <param name="Inbox_ID"></param>
        public void FuncNENNotifStructureUpdation(int NotificationId = 0, int Inbox_ID = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int organisationId = 0;
            if (SessionInfo == null)
            {
                RedirectToAction("Logout", "Account");
            }
            else
            {
                organisationId = (int)SessionInfo.OrganisationId;
            }
            RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
            objRouteAssessmentModel = constraintService.GetNotificationAffectedStructuresConstraint(Inbox_ID, organisationId);

            //save affected constraint and affected structure notifications
            //NotificationController Instance = new NotificationController();
            bool updated = SaveAffectedSrtuctConstrNotif(objRouteAssessmentModel, NotificationId);

            Dictionary<int, int> icaStatusDictionary = null;
            // Code to fetch ContactID based upon UserID
            int contactID = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));
            int icaStatus = 277001;
            if (objRouteAssessmentModel.AffectedStructure != null)
            {
                string xmlaffectedStructures = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure));
                icaStatusDictionary = iSORTApplicationService.GetNotifICAstatus(xmlaffectedStructures);
                if (organisationId > 0) //logged in users organisation is used to verify whether the ICA status of the logged in organisation is to be checked.
                {
                    //checking whether the ica status dictionary is empty or the count is 0 this will help 
                    if (icaStatusDictionary != null && icaStatusDictionary.Count > 0 && icaStatusDictionary.ContainsKey(organisationId))
                    {
                        icaStatus = icaStatusDictionary[organisationId];
                    }
                }
            }

            int result = NENNotificationService.UpdateNENICAStatusInboxItem(Inbox_ID, icaStatus, SessionInfo.OrganisationId);
        }

        #region
        public bool SaveAffectedSrtuctConstrNotif(RouteAssessmentModel objRouteAssessmentModel, int NotificationId)
        {
            bool update = false;

            try
            {
                CommonNotifMethods commonNotif = new CommonNotifMethods();
                AffectedStructConstrParam affectedParam = new AffectedStructConstrParam
                {
                    AffectedConstraints = commonNotif.GetAffectedConstrList(objRouteAssessmentModel.Constraints),
                    AffectedSections = commonNotif.GetAffectedStructList(objRouteAssessmentModel.AffectedStructure),
                    NotificationId = NotificationId
                };

                if (affectedParam.AffectedSections.Count > 0 || affectedParam.AffectedConstraints.Count > 0)
                    update = notificationsService.SaveAffectedNotificationDetails(affectedParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return update;
        }
        #endregion

        #region SOA Reports

        public ActionResult NEN_SOAReport() 
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                return View("NEN_SOAReport");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NEN_SOAReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion


        public ActionResult NEN_HelpdeskReport()//for NEN Submenu
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                return View("NEN_HelpdeskReport");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NEN_HelpdeskReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult NEN_HelpdeskReportHistory(Int32 month = 0, Int32 year = 0, Int32 notificationcategory = 0)//for submit button in Report menu(nen report)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                    ViewBag.NotificationCategory = notificationcategory;
                    NeReport neReport = (NeReport)notificationcategory;
                    string vehicleCat = neReport.GetEnumDescription();
                    int requiresVr1 = 0;
                    if (notificationcategory == (int)NeReport.All_type)
                        requiresVr1 = 2;
                    else if (notificationcategory == (int)NeReport.VR1)
                        requiresVr1 = 1;
                    int vehicleCatCount = !string.IsNullOrWhiteSpace(vehicleCat) ? vehicleCat.Split(',').Length : 0;
                    List<NENHelpdeskReportModel> NENHelpdeskReportList = NENNotificationService.GetNENHelpdeskReportHistory(month, year, vehicleCat, requiresVr1, vehicleCatCount);
                    AlterCategoryColumnName(NENHelpdeskReportList);
                    return PartialView("PartialView/_NEN_HelpdeskReportHistory", NENHelpdeskReportList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NEN_HekpdeskReportHistory, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NEN_HekpdeskReportHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

       
        private void AlterCategoryColumnName(List<NENHelpdeskReportModel> NENHelpdeskReportList)
        {

            for (var i = 0; i < NENHelpdeskReportList.Count; i++)
            {
                switch (Convert.ToString(NENHelpdeskReportList[i].Categories))
                {

                    case "gc001":
                        NENHelpdeskReportList[i].Categories = "Vehicle Special Order";
                        break;
                    case "gc002":
                        NENHelpdeskReportList[i].Categories = "Special Order";
                        break;
                    case "gc003":
                        NENHelpdeskReportList[i].Categories = "STGO CAT 1";
                        break;
                    case "gc004":
                        NENHelpdeskReportList[i].Categories = "STGO CAT 2";
                        break;
                    case "gc005":
                        NENHelpdeskReportList[i].Categories = "STGO CAT 3";
                        break;
                    case "gc006":
                        NENHelpdeskReportList[i].Categories = "Mobile Crane CAT A";
                        break;
                    case "gc007":
                        NENHelpdeskReportList[i].Categories = "Mobile Crane CAT B";
                        break;
                    case "gc008":
                        NENHelpdeskReportList[i].Categories = "Mobile Crane CAT C";
                        break;
                    case "gc009":
                        NENHelpdeskReportList[i].Categories = "STGO Road Recovery";
                        break;
                    case "gc010":
                        NENHelpdeskReportList[i].Categories = "C and U";
                        break;
                    case "gc011":
                        NENHelpdeskReportList[i].Categories = "Tracked";
                        break;
                    case "gc012":
                        NENHelpdeskReportList[i].Categories = "STGO Engineering Plant Wheeled";
                        break;
                    case "gc013":
                        NENHelpdeskReportList[i].Categories = "STGO Engineering Plant Tracked";
                        break;
                    case "vr1 gc003":
                        NENHelpdeskReportList[i].Categories = "VR1 STGO CAT 1";
                        break;
                    case "vr1 gc004":
                        NENHelpdeskReportList[i].Categories = "VR1 STGO CAT 2";
                        break;
                    case "vr1 gc005":
                        NENHelpdeskReportList[i].Categories = "VR1 STGO CAT 3";
                        break;
                    case "vr1 gc009":
                        NENHelpdeskReportList[i].Categories = "VR1 STGO Road Recovery";
                        break;
                    case "vr1 gc012":
                        NENHelpdeskReportList[i].Categories = "VR1 STGO Engineering Plant Wheeled";
                        break;
                    case "vr1 gc013":
                        NENHelpdeskReportList[i].Categories = "VR1 STGO Engineering Plant Tracked";
                        break;
                }
            }
        }

        public ActionResult NENHelpdeskReportExportToCSV(Int32 startMonth, Int32 startYear, Int32 notificationcategory)//eport button in nenreport
        {
            try
            {
                NeReport neReport = (NeReport)notificationcategory;
                string vehicleCat = neReport.GetEnumDescription();
                int requiresVr1 = 0;
                if (notificationcategory == (int)NeReport.All_type)
                    requiresVr1 = 2;
                else if (notificationcategory == (int)NeReport.VR1)
                    requiresVr1 = 1;
                int vehicleCatCount = !string.IsNullOrWhiteSpace(vehicleCat) ? vehicleCat.Split(',').Length : 0;
                List<NENHelpdeskReportModel> NENHelpdeskReportList = NENNotificationService.GetNENHelpdeskReportHistory(startMonth, startYear, vehicleCat, requiresVr1, vehicleCatCount);

                StringBuilder sb = new StringBuilder();

                sb.Append("Categories" + ",");
                sb.Append("NEN Failure" + ",");
                sb.Append("NEN Sent by mail" + ",");
                sb.Append("NEN Sent by api" + ",");
                sb.Append("Total NEN Submitted" + ",");
                string fileName = "NENHelpdeskReport.csv";

                sb.Append("\r\n");

                for (int count = 0; count < NENHelpdeskReportList.Count; count++)
                {
                    AlterCategoryColumnName(NENHelpdeskReportList);
                    sb.Append(NENHelpdeskReportList[count].Categories.Replace(',', ' ') + ",");
                    sb.Append(NENHelpdeskReportList[count].NENFailures + ",");
                    sb.Append(NENHelpdeskReportList[count].NENSentByEmail + ",");
                    sb.Append(NENHelpdeskReportList[count].NENSentByApi + ",");
                    sb.Append(NENHelpdeskReportList[count].TotalNENSubmitted);
                    sb.Append("\r\n");
                }

                string csvContent = sb.ToString();
                return File(new UTF8Encoding().GetBytes(csvContent), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NENHelpdeskReportExportToCSV, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }

        public ActionResult NEN_SOAReportHistory(Int32 month = 0, Int32 year = 0)//for submit button in Report menu(soa report)
        {
            try
            {
                UserInfo SessionInfo = null;
                long org_ID = 0;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    org_ID = SessionInfo.OrganisationId;
                }
                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;

                    List<NENSOAReportModel> NENSOAReportList = NENNotificationService.GetNENSOAReportHistory(month, year, org_ID);
                    return PartialView("PartialView/_NEN_SOAReportHistory", NENSOAReportList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NEN_SOAReportHistory, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NEN_SOAReportHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult NENSOAReportExportToCSV(Int32 startMonth, Int32 startYear)//eport button in nenreport(soa)
        {
            try
            {
                UserInfo SessionInfo = null;
                long org_ID = 0;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    org_ID = SessionInfo.OrganisationId;
                }
                string fileName = "NENSOAReport.csv";
                List<NENSOAReportModel> NENSOAReportList = NENNotificationService.GetNENSOAReportHistory(startMonth, startYear, org_ID);

                StringBuilder sb = new StringBuilder();

                sb.Append("Received" + ",");
                sb.Append("Accepted" + ",");
                sb.Append("Rejected" + ",");
                sb.Append("Sent for further assessment" + ",");
                sb.Append("No action taken");
                sb.Append("\r\n");

                for (int count = 0; count < NENSOAReportList.Count; count++)
                {
                    sb.Append(NENSOAReportList[count].ReportRecieved + ",");
                    sb.Append(NENSOAReportList[count].ReportAccepted + ",");
                    sb.Append(NENSOAReportList[count].ReportRejected + ",");
                    sb.Append(NENSOAReportList[count].SentforFurtherAssessment + ",");
                    sb.Append(NENSOAReportList[count].NoActionTaken);
                    sb.Append("\r\n");
                }

                string csvContent = sb.ToString();
                return File(new System.Text.UTF8Encoding().GetBytes(csvContent), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("NENNotification/NENSOAReportExportToCSV, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
    
        public JsonResult NenAutoRoutePlanning(List<AppRouteList> ImportedRoutelist)
        {
            List<RoutePointJson> routePointJsons = new List<RoutePointJson>();
            List<NenRouteModel> nenRouteModelList = new List<NenRouteModel>();
            try
            {
                if (ImportedRoutelist.Count > 0)
                {
                    for (int i = 0; i < ImportedRoutelist.Count; i++)
                    {
                        routePointJsons= Get_Route_Points((int)ImportedRoutelist[i].RouteID);
                        if (routePointJsons != null)
                            nenRouteModelList.Add(GetAddressNEN(ImportedRoutelist[i].IsReturnRoute));
                    }
                }
                return Json(new { nenRouteModelList = nenRouteModelList });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Notification/NenAutoRoutePlanning, Exception: {0}", ex));
                return null;
            }
        }
    }
}