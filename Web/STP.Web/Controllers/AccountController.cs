#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Xml;
using STP.Business.Filters;
using STP.Domain;
using STP.Common.Constants;
using STP.Domain.SecurityAndUsers;
using STP.DataAccess.Provider;
using STP.Domain.LoggingAndReporting;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.SecurityAndUsers;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.Applications;

using STP.Domain.Communications;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Web.Filters;
using PagedList;
using System.Linq;
using System.Text;
using System.Net.Mail;
using STP.Domain.HelpdeskTools;
using STP.Common.EncryptDecrypt;
using System.Configuration;
using STP.Common.Logger;
using STP.ServiceAccess.CommunicationsInterface;
using System.Text.RegularExpressions;
using STP.Web.Helpers;



#endregion

namespace STP.Web.Controllers
{
    //[Authorize]
    //[InitializeSimpleMembership]
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : Controller
    {
        private readonly IDocumentService documentService;
        private readonly IAuthenticationService authenticationService;
        private readonly ILoggingService loggingService;
        private readonly IMovementsService movementsService;
        private readonly INotificationService notificationService;
        private readonly ISORTApplicationService sortApplicationService;
        private readonly INENNotificationService nenNotificationService;

        private readonly IUserService userService;
        private readonly IReportService reportService;
        private readonly ICommunicationsInterfaceService communicationService;
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public AccountController()
        {
        }

        public AccountController(IDocumentService documentService, IAuthenticationService authenticationService, ILoggingService loggingService, IMovementsService movements, INotificationService notificationService, UserService userService, ISORTApplicationService sortApplicationService, INENNotificationService nenNotificationService, IReportService reportService, ICommunicationsInterfaceService communicationService)
        {
            this.documentService = documentService;
            this.authenticationService = authenticationService;
            this.loggingService = loggingService;
            this.movementsService = movements;
            this.notificationService = notificationService;
            this.userService = userService;
            this.sortApplicationService = sortApplicationService;
            this.nenNotificationService = nenNotificationService;
            this.reportService = reportService;
            this.communicationService = communicationService;
        }

        [AllowAnonymous]
        [NoCache]
        [SessionValidate(Disable = true)]
        [AntiForgeryCookie]
        public ActionResult LogOut()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            UserInfo userInfo = null;
            if (Session["UserInfo"] != null)
            {
                userInfo = (UserInfo)Session["UserInfo"];

                //---------------------------------------------------------------------
                #region movement actions for this action method

                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.MovementActionType = MovementnActionType.user_logs_out;
                movactiontype.ContactName = userInfo.FirstName + " " + userInfo.LastName;
                string ErrMsg = string.Empty;
                string MovementDescription = MovementActions.GetMovementActionString(userInfo, movactiontype, out ErrMsg);
                long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, 0,0,0, userInfo.UserSchema);
                #endregion

                #region sys_events for saving loggout info
                int user_ID = Convert.ToInt32(userInfo.UserId);
                if (userInfo.UserSchema == UserSchema.Portal)
                    movactiontype.SystemEventType = SysEventType.portal_user_logged_out;
                else
                    movactiontype.SystemEventType = SysEventType.sort_user_loggedout;

                string sysEventDescp = System_Events.GetSysEventString(userInfo, movactiontype, out ErrMsg);
                bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, userInfo.UserSchema);
                #endregion
                //---------------------------------------------------------------------

            }

            EncryptionUtility.SetEncryptionKey("");
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.MinValue;
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.MinValue
            });
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Users");
        }
        //
        // GET: /Account/Login

        public ActionResult SORTUserRole()
        {

            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult LoginWithCulture(string lang, string returnUrl)
        {
            var langCookie = new HttpCookie("lang", lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);

            return View("Login");
        }

        //
        // POST: /Account/Login
        [SessionValidate(Disable = true)]
        public ActionResult Login(string returnUrl = null)
        {
            if (returnUrl == null || returnUrl == "")
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return RedirectToAction("Login", "Users", new { returnUrl = Url.Action(returnUrl) });
            }
        }

        public ActionResult RedirectOnLogin()
        {

            return View();
        }

        [UnValidateAntiForgeryToken]
        [SessionValidate(Disable = true)]
        public ActionResult RedirectToLogin()
        {
            return PartialView("PartialView/_RedirectToLogin");
        }

        public ActionResult RefreshToken()
        {
            return PartialView("PartialView/_AntiForgeryToken");
        }

        [HttpPost]
        [NoCacheAttribute]
        [AllowAnonymous]
        [SessionValidate(Disable = true)]
        [AntiForgeryCookie]
        public ActionResult Login(UserInfo userLogin, string returnUrl = null)
        {
            try
            {
                //validate Return Url
                Uri uriResult;
                bool result = Uri.TryCreate(returnUrl, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (result)
                {
                    return Json(new { redirectToUrl = Url.Content("/") });
                }

                if (TempData["Success"] != null)
                {
                    ViewBag.Message = (string)TempData["Success"];
                }
                if (!string.IsNullOrEmpty(userLogin.UserName) && !string.IsNullOrEmpty(userLogin.Password))
                {
                    GenericController gc = new GenericController();
                    var userInfo = authenticationService.GetLoginInfo(userLogin);

                    if (userInfo.UserName == userLogin.UserName)
                    {

                        userLogin.Password = gc.MD5Encryption(userLogin.Password);
                        userInfo.Password = gc.MD5Encryption(userInfo.Password);

                        #region clear all session
                        Session.RemoveAll();
                        #endregion

                        if (userInfo.IsEnabled >= 1)
                        {
                            if (userInfo.Password == userLogin.Password)
                            {
                                Session["ProjectVersion"] = ConfigurationManager.AppSettings["projectVersion"];
                                userInfo.Password = userLogin.Password;

                                if (userInfo.UserTypeId == 696008)
                                {
                                    userInfo.UserSchema = UserSchema.Sort;
                                }
                                Session["UserInfo"] = userInfo;
                                SetMenuPrivelege();
                                NonceHelper.ClearNonceQueue();
                                EncryptionUtility.SetEncryptionKey(userInfo.OrganisationId.ToString());

                                Session["maploadCheckflag"] = false;
                                Session["routedisplayCheckflag"] = false;
                                Session["routeplanCheckflag"] = false;

                                #region Handle session hijacking
                                string browserInfo = Request.Browser.Browser +
                                    Request.Browser.Version +
                                    Request.UserAgent + "~" +
                                    Request.ServerVariables["REMOTE_ADDR"];

                                string sessionValue = Convert.ToString(Session["UserId"]) + "^" +
                                    DateTime.Now.Ticks + "^" +
                                    browserInfo + "^" +
                                    System.Guid.NewGuid();

                                byte[] encodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sessionValue);
                                string encryptedString = System.Convert.ToBase64String(encodeAsBytes);
                                Session["encryptedSession"] = encryptedString;
                                #endregion

                                string daysRemaining = string.Empty;

                                int numberOfDays = 0;
                                int flag = 0;
                                if (userInfo.PasswordStatus != 4)//4 - normal,6- OTP

                                {
                                    flag = 6;
                                }


                                var expiryInfo = authenticationService.GetPasswordExpiryInfo(flag);

                                int notifyPeriod = Convert.ToInt32(expiryInfo.ReminderPeriod);
                                int expiryPeriod = Convert.ToInt32(expiryInfo.PasswordLife); //This is not updated and fixed ?

                                DateTime lastLoginDate = userInfo.LastLogin;
                                if (userInfo.PasswordStatus != 4)//4 - normal,6- OTP
                                {
                                    lastLoginDate = lastLoginDate.AddDays(-365);
                                }
                                DateTime today = DateTime.Now;

                                int dateDifference = today.Subtract(lastLoginDate).Days;

                                numberOfDays = expiryPeriod - dateDifference;

                                Session["showTermsandconditiononly"] = false;
                                Session["showTandConlyAndPass"] = false;
                                Session["UserIDToUpdateTAndC"] = "0";
                                if ((userInfo.LastLogin == DateTime.MinValue || numberOfDays <= 0))//&& userInfo.isTermsAccepted != 1)
                                {
                                    Session["Redirect"] = true;
                                    TempData["RedirectToChangePwd"] = "true";
                                    Session["RedirectToChangePwd"] = true;
                                }
                                else if (userInfo.IsTermsAccepted != 1)//to show T&C only #5728
                                {
                                    Session["Redirect"] = true;
                                    TempData["RedirectToChangePwd"] = "true";
                                    Session["RedirectToChangePwd"] = true;
                                    Session["showTermsandconditiononly"] = true;
                                }

                                if (userInfo.LastLogin != DateTime.MinValue && userInfo.IsTermsAccepted != 1 && numberOfDays <= 0)//To show T&C and changepassword if password reset by helpdesk #5728
                                {
                                    Session["showTandConlyAndPass"] = true;
                                    Session["showTermsandconditiononly"] = false;
                                    Session["UserIDToUpdateTAndC"] = userInfo.UserId;
                                }


                                if (numberOfDays <= notifyPeriod)
                                {
                                    daysRemaining = Convert.ToString(numberOfDays);
                                    TempData["ExpiryDays"] = daysRemaining;
                                }

                                Session["PageSize"] = userInfo.MaxListItem;
                                Session["userTypeId"] = userInfo.UserTypeId;
                                if (userInfo.UserTypeId == 696001 || userInfo.UserTypeId == 696002 || userInfo.UserTypeId == 696007)
                                {
                                    if (userInfo.UserTypeId == 696001 && userInfo.LoggedIn == 2)
                                        Session["Logged_In"] = 1;
                                    else
                                        Session["Logged_In"] = userInfo.LoggedIn;
                                }
                                else
                                {
                                    Session["Logged_In"] = 1;
                                }
                                if (userInfo.UserTypeId == 696001)
                                    Session["ViewMap"] = userInfo.LoggedIn;

                                // ---ForGot Password--OTP checking
                                #region OTP Checking
                                if (userInfo.PasswordStatus != 4)//4 - normal,6- OTP

                                {
                                    //To get Uk time zone
                                    var BritishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

                                    DateTime dt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

                                    DateTime DateTimeInBritishLocal = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, BritishZone);
                                    userInfo.PasswordUpdatedOn = userInfo.PasswordUpdatedOn.AddMinutes(60);

                                    int res2 = DateTime.Compare(DateTimeInBritishLocal, userInfo.PasswordUpdatedOn);

                                    if (dt > userInfo.PasswordUpdatedOn)
                                    {
                                        return Json(new { Errormessage = "OTP Expired." });
                                    }
                                    else if ((bool)Session["RedirectToChangePwd"] && userInfo.IsTermsAccepted == 1)
                                    {
                                        return Json(new { redirectToUrl = Url.Action("ChangePassword", "Users") });
                                    }

                                }
                                #endregion
                                //--------- 

                                FormsAuthentication.SetAuthCookie(userInfo.UserName, false);

                                #region movement actions for this action method
                                UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                                if (Session["UserInfo"] == null)
                                {
                                    return RedirectToAction("Login", "Account");
                                }
                                UserSessionValue = (UserInfo)Session["UserInfo"];
                                #endregion

                                #region sys_events for saving loggin info
                                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                                movactiontype.MovementActionType = MovementnActionType.user_logs_in;
                                string ErrMsg = string.Empty;
                                string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);

                                long res = loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription,0,0,0, UserSessionValue.UserSchema);

                                if (UserSessionValue.UserSchema == UserSchema.Sort)
                                {
                                    movactiontype.SystemEventType = SysEventType.sort_successful_login;

                                }
                                if (UserSessionValue.UserSchema == UserSchema.Portal)
                                {
                                    movactiontype.SystemEventType = SysEventType.portal_successful_login;

                                }
                                string sysEventDescp = System_Events.GetSysEventString(UserSessionValue, movactiontype, out ErrMsg);
                                int user_ID = Convert.ToInt32(UserSessionValue.UserId);

                                bool sysEvntResult = loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, UserSessionValue.UserSchema);
                                if (UserSessionValue.UserSchema == UserSchema.Portal)
                                {
                                    movactiontype.SystemEventType = SysEventType.portal_successful_login;

                                }

                                #endregion
                                //404 error issue after cyber security fixes
                                returnUrl = null;
                                if (returnUrl == null || returnUrl == "")
                                {
                                    if (userInfo.UserTypeId == 696002 || userInfo.UserTypeId == 696007)
                                    {
                                        return Json(new { redirectToUrl = Url.Action("MovementInboxList", "Movements") });
                                    }
                                    else if (userInfo.UserTypeId == 696006)
                                    {
                                        return Json(new { redirectToUrl = Url.Action("Index", "Helpdesk") });

                                    }
                                    else if (userInfo.UserTypeId == 696008)
                                    {

                                        return Json(new { redirectToUrl = Url.Action("SORT", "Home") });
                                    }
                                    else
                                    {
                                        return Json(new { redirectToUrl = Url.Action("Hauliers", "Home") });
                                    }
                                }
                                else
                                {
                                    return Json(new { redirectToUrl = Url.Content(returnUrl) });
                                }
                            }
                            else
                            {
                                Response.Cookies.Clear();
                                return Json(new { Errormessage = "Incorrect Username/Password." });
                            }
                        }
                        else
                        {
                            //is locked
                            Response.Cookies.Clear();
                            return Json(new { Errormessage = "Your account is disabled, please contact helpdesk." });

                        }
                    }
                    else
                    {
                        Response.Cookies.Clear();
                        //invalid username or password
                        if (userInfo.ResponseContent != null && userInfo.ResponseContent.Contains("Unable to establish connection"))
                        {
                            return Json(new { Errormessage = "  Unable to establish connection to Database Server. Please try to login after some time. If the problem persists, contact helpdesk." });
                        }
                        else
                        {
                            return Json(new { Errormessage = "Incorrect Username/Password" });
                        }


                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(userLogin.UserName) && string.IsNullOrEmpty(userLogin.Password))
                    {
                        return Json(new { Errormessage = "Username and Password is required" });

                    }
                    else if (string.IsNullOrEmpty(userLogin.UserName))
                    {
                        return Json(new { Errormessage = "Username is required" });

                    }
                    else
                    {
                        return Json(new { Errormessage = "Password is required" });
                    }
                }

            }
            catch (DBConnectionFailedException)
            {
                ModelState.AddModelError("", "Unable to establish connection to Database Server. Please try to login after some time. If the problem persists, contact helpdesk.");
            }
            catch (Exception ex)
            {
                ex.Message.Equals("The type initializer for 'Nested' threw an exception.");
                ModelState.AddModelError("", "Unable to establish connection to Server. Please retry after some time. If the problem persists, contact helpdesk.");
            }

            return View(userLogin);
        }
        #region set menu privilege
        private void SetMenuPrivelege()
        {
            int userTypeId = 0;
            if (Session["UserInfo"] != null)
            {
                var sessionValues = (UserInfo)Session["UserInfo"];
                userTypeId = sessionValues.UserTypeId;
            }

            MenuAccess menuAccess = new MenuAccess();
            menuAccess.MenuInfo = new List<Menus>();
            menuAccess.SubMenuInfo = new List<SubMenus>();
            menuAccess.MenuAccessInfo = new List<MenuPrivileage>();

            MenuPrivileage objMenuPrivileage = null;

            var menuInfo = authenticationService.GetMenuInfo(userTypeId);
            /*privileage */
            foreach (var menuPrivileage in menuInfo)
            {
                objMenuPrivileage = new MenuPrivileage();
                objMenuPrivileage.MenuId = menuPrivileage.MenuId;
                menuAccess.MenuAccessInfo.Add(objMenuPrivileage);
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Configuration.xml");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);


            XmlNode menuItems = xmlDocument.SelectSingleNode("/Configurable/MenuItems");
            XmlNodeList projectList = menuItems.SelectNodes("Project");

            foreach (XmlNode project in projectList)
            {

                var projectId = project.Attributes.GetNamedItem("id").Value;

                if (projectId == "1")
                {
                    XmlNodeList menuItemsList = project.SelectNodes("MainMenu");

                    foreach (XmlNode mainMenu in menuItemsList)
                    {
                        var menuId = mainMenu.Attributes.GetNamedItem("id").Value;
                        var menuName = mainMenu.Attributes.GetNamedItem("name").Value;

                        Menus objMenus = new Menus();
                        objMenus.MenuId = menuId;
                        objMenus.MenuName = menuName;

                        menuAccess.MenuInfo.Add(objMenus);

                        XmlNodeList subMenuList = mainMenu.SelectNodes("SubMenu");

                        foreach (XmlNode subMenu in subMenuList)
                        {
                            var subId = subMenu.Attributes.GetNamedItem("id").Value;
                            var subName = subMenu.Attributes.GetNamedItem("name").Value;
                            var url = subMenu.Attributes.GetNamedItem("url").Value;

                            SubMenus objSubMenus = new SubMenus();
                            objSubMenus.MainMenuId = menuId;
                            objSubMenus.SubMenuId = subId;
                            objSubMenus.SubMenuName = subName;
                            objSubMenus.Navigation = url;

                            menuAccess.SubMenuInfo.Add(objSubMenus);
                        }
                    }
                }
            }

            Session["MenuAccess"] = menuAccess;
        }
        #endregion set menu privilege

        #region UserPreferences
        public JsonResult UserPreferences()
        {


            int UserId = 0;
            int organisationId = 0;

            string Email = "";
            string FaxNo = "";
            ViewBag.mode = "Get";
            // ViewBag.UserId = UserId;
            if (Session["UserInfo"] != null)
            {
                var sessionValues = (UserInfo)Session["UserInfo"];
                UserId = Convert.ToInt32(sessionValues.UserId);
                organisationId = Convert.ToInt32(sessionValues.OrganisationId);

                Email = sessionValues.Email;
                // ViewBag.MaxListItems = Session["PageSize"];
            }
            //else
            //    return RedirectToAction("Login", "Account");


            UserPreferences userobj = new UserPreferences();

            MailResponse responseMailDetails = notificationService.GetResponseMailDetails(organisationId);
            string ReplyMailPath = responseMailDetails.ReplyMailPdf;



            userobj = userService.GetUserPreferencesById(UserId);





            Byte[] ResponseMsg = responseMailDetails.ReplyMailText;

            Session["PreferenceNotesBLOB"] = responseMailDetails.ReplyMailText;
            Session["PreferenceNotesBLOB"] = ResponseMsg;


            object[] response = new object[2];
            response[0] = responseMailDetails;
            response[1] = userobj;
            return Json(new { result = response });
        }
        #endregion

        #region GetSplitESDAL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ESDALREf"></param>
        /// <returns></returns>
        public List<string> GetSplitESDAL(string ESDALREf = null)
        {
            string[] strArr = new string[3];
            string mnemonic = string.Empty;
            string esdalrefnum = string.Empty;
            string version = string.Empty;
            bool isNotif = ESDALREf.Contains('#');
            string[] esdalRefPro = ESDALREf.Split('/');

            // TODO todo
            if (esdalRefPro.Length > 0)
            {
                strArr[0] = Convert.ToString(esdalRefPro[0]);
                strArr[1] = Convert.ToString(esdalRefPro[1]);
                if (isNotif == true)
                {
                    string[] a = esdalRefPro[2].Split('#');
                    esdalRefPro[2] = a[1];
                }
                strArr[2] = Convert.ToString(esdalRefPro[2].ToUpper().Replace("S", ""));

            }
            List<string> arrlist = new List<string>(strArr);
            return arrlist;
        }
        #endregion


        #region Auto Response 
        public ActionResult AutoResponse()
        {

            int UserId = 0;
            int organisationId = 0;

            string Email = "";
            string FaxNo = "";
            ViewBag.mode = "Get";
            // ViewBag.UserId = UserId;
            if (Session["UserInfo"] != null)
            {
                string result = string.Empty;
                result = (string)Session["PreferenceNotes"];
                if (result != null)
                {
                    ViewBag.NotesforHaulier = result;
                }
                else
                {

                    byte[] ResponseStatus = (byte[])Session["PreferenceNotesBLOB"];
                    string hnresult = string.Empty;
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                    string errormsg = "";
                    hnresult = STP.Common.General.XsltTransformer.Trafo(ResponseStatus, path, out errormsg);
                    var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(hnresult);
                    ViewBag.NotesforHaulier = hnresult;
                }
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                organisationId = (int)SessionInfo.OrganisationId;
                UserId = Convert.ToInt32(SessionInfo.UserId);
                ViewBag.organisationId = organisationId;
                ViewBag.UserId = UserId;
                UserInfo sessionValues = new UserInfo();
                Email = sessionValues.Email;
                // ViewBag.MaxListItems = Session["PageSize"];
            }
            else
                return RedirectToAction("Login", "Account");


            UserPreferences userobj = new UserPreferences();
            int ResponseStatusGlobal = userService.GetAutoResponse(organisationId);
            MailResponse responseMailDetails = notificationService.GetResponseMailDetails(organisationId);
            string ReplyMailPath = responseMailDetails.ReplyMailPdf;
            ViewBag.ReplyMailPath = ReplyMailPath;
            ViewBag.ResponseStatus = ResponseStatusGlobal;
            Session["ResponseStatusGlobal"] = ResponseStatusGlobal;

            userobj = userService.GetUserPreferencesById(UserId);
            ViewBag.MaxListItems = userobj.MaxListItems;
            ViewBag.Fax = userobj.FaxNumber;
            //UserPreferences objUSerPreferences = null;
            //objUSerPreferences = RouteProvider.Instance.GetUserPrefById(UserId);




            //SessionInfo = (UserInfo)Session["UserInfo"];
            //long UserId = Int32.Parse(sessionValues.UserID);
            string userschema = "STP_USER_PREFERENCES";
            Byte[] ResponseMsg = responseMailDetails.ReplyMailText;

            Session["PreferenceNotesBLOB"] = responseMailDetails.ReplyMailText;
            Session["PreferenceNotesBLOB"] = ResponseMsg;
            //Session["PreferenceNotesBLOB"] = ViResponseMsg;

            if(ResponseStatusGlobal==0)
            {
                byte[] ResponseStatus = (byte[])Session["PreferenceNotesBLOB"];
                string hnresult = string.Empty;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                string errormsg = "";
                hnresult = STP.Common.General.XsltTransformer.Trafo(ResponseStatus, path, out errormsg);
                ViewBag.NotesforHaulier = hnresult;
            }
            ViewBag.Email = Email;
            ViewBag.UserId = UserId;
            ViewBag.isXMLattached = userobj.IsXMLAttached;
            // return Json(new { result = flag, ResMsg = ResponseMsg });
            return View("AutoResponse");

        }
        [HttpPost]
        public ActionResult SaveResponseMsgs()
        {
            return Json("File Saved Successfully.");
        }
        [HttpPost]
        public ActionResult SaveResponseMsg()
        {

            #region save PDF
            string PDFPath = "";

            string fpath = "";
            int FldrExst = 0;

            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];

            if (Request.Files.Count > 0)
            {

                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    HttpPostedFileBase file = files[i];

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fpath = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fpath = file.FileName;
                    }

                    //set the file name
                    string Org_name = SessionInfo.OrganisationName.Replace(@" ", "_");
                    Org_name = Org_name.Replace(@"/", "_");
                    string Todays_Date = Convert.ToString(DateTime.Now).Replace(" ", "_");
                    Todays_Date = Todays_Date.Replace(":", "");
                    Todays_Date = Todays_Date.Replace(@"/", "");


                    string fname = Org_name + "_" + Todays_Date + ".pdf";

                    string ImageName = System.IO.Path.GetFileName(fname);

                    string filePath = System.Configuration.ConfigurationManager.AppSettings["PdfPathInServer"];


                    //if there is no such a directory we create one 
                    bool isExists = System.IO.Directory.Exists(filePath);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory(filePath);


                    string filefullPath = Path.Combine(filePath, ImageName);
                    // save PDF in folder
                    file.SaveAs(filefullPath);

                    PDFPath = filefullPath;

                }
            }
            #endregion
            byte[] inbByteVar = (byte[])Session["PreferenceNotesBLOB"];
            int ResStatus = (int)Session["ResStatus"];
            int organisationId = (int)SessionInfo.OrganisationId;
            int UserID = Convert.ToInt32(SessionInfo.UserId);
            try
            {
                var saveResponseMessageParams = new SaveResponseMessageParams()
                {
                    UserId = UserID,
                    AutoResponseId = ResStatus,
                    OrganisationId = organisationId,
                    ResponseMessage = inbByteVar,
                    ResponsePdf = PDFPath
                };
                bool result = sortApplicationService.SaveResponseMessage(saveResponseMessageParams);
                //bool result = SORTApplicationProvider.Instance.SaveResponseMessage(UserId, ResStatus, OrgId, inbByteVar, PDFPath);
                if (result == true)
                {
                    return Json("Data Saved Successfully.");

                }

                else { return Json("No File Saved."); }

            }

            catch (Exception ex)
            {
                return Json("Error While Saving.");

            }

        }


        #region UserPrefer
        [HttpPost]
        public ActionResult UserPrefer(int UserId, string filenull, int ResStatus, int pdfExst, string EmailUpdate = "")
        {

            bool flag = true;
            bool result = false;
            string bytes = "";
            string ResponseMsg = "";
            long OrgId;

            UserPreferences objUserPref = new UserPreferences();
            try
            {
                var sessionValues = (STP.Domain.SecurityAndUsers.UserInfo)Session["UserInfo"];
                if (Session["UserInfo"] != null)
                {
                    UserId = Convert.ToInt32(sessionValues.UserId);
                    OrgId = sessionValues.OrganisationId;

                }
                else
                    return RedirectToAction("Login", "Account");

                //if (MaxListItems == 0)
                //{
                //    MaxListItems = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;
                //}
                //else
                //{
                //    Session["PageSize"] = MaxListItems;
                //}

                byte[] inbByteVar = (byte[])Session["PreferenceNotesBLOB"];




                string auditLogDescp = "";

                if (sessionValues.UserTypeId == 696007)
                {
                    auditLogDescp = "SOA user '" + sessionValues.UserName;

                }
                else
                {
                    auditLogDescp = "Poice user '" + sessionValues.UserName;

                }
                int ResponseStatusGlobal = 0;
                ResponseStatusGlobal = (int)Session["ResponseStatusGlobal"];

                if (ResStatus == 1 && inbByteVar != null)
                {
                    MailResponse responseMailDetails = notificationService.GetResponseMailDetails((int)OrgId);

                    // flag = userService.SetUserPreference(objUserPref, UserId, EmailUpdate, FaxNumber);
                    var saveResponseMessageParams = new SaveResponseMessageParams()
                    {
                        UserId = UserId,
                        AutoResponseId = ResStatus,
                        OrganisationId = OrgId,
                        ResponseMessage = inbByteVar,
                        ResponsePdf = responseMailDetails.ReplyMailPdf
                    };
                    result = sortApplicationService.SaveResponseMessage(saveResponseMessageParams);
                    // result = sortApplicationService.SaveResponseMessage(UserId, ResStatus, OrgId, inbByteVar, responseMailDetails.ReplyMailPdf);
                    if (pdfExst == 0)
                    {

                        var saveAutoResponseParams = new SaveAutoResponseParams()
                        {
                            LogMessage = auditLogDescp,
                            UserId = 1,
                            OrganisationId = sessionValues.organisationId

                        };
                        if (ResponseStatusGlobal == 0)
                        {
                            auditLogDescp = auditLogDescp + "' Enabled the auto response and added response message on " + DateTime.Now.ToString();
                            long auditLogResult = nenNotificationService.SaveNotificationAutoResponseAuditLog(saveAutoResponseParams);

                            //long auditLogResult = nenNotificationService.SaveNotifAutoResponseAuditLog(auditLogDescp, 1, sessionValues.organisationId);

                        }
                        else
                        {
                            string tempStatus = (string)Session["UpdatedNow"];

                            if (tempStatus == "1")
                            {
                                auditLogDescp = auditLogDescp + "' updated response message on " + DateTime.Now.ToString();
                                long auditLogResult = nenNotificationService.SaveNotificationAutoResponseAuditLog(saveAutoResponseParams);

                            }

                        }

                    }

                }
                else if (ResStatus == 0)
                {
                    //  flag = userService.SetUserPreference(objUserPref, UserId, EmailUpdate, FaxNumber);
                    var saveResponseMessageParams = new SaveResponseMessageParams()
                    {
                        UserId = UserId,
                        AutoResponseId = ResStatus,
                        OrganisationId = OrgId,
                        ResponseMessage = inbByteVar,
                        ResponsePdf = bytes
                    };
                    result = sortApplicationService.SaveResponseMessage(saveResponseMessageParams);

                    // result = sortApplicationService.SaveResponseMessage(UserId, ResStatus, OrgId, null, bytes);
                    if (ResponseStatusGlobal != 0)
                    {
                        auditLogDescp = auditLogDescp + "' Disabled the auto response on " + DateTime.Now.ToString();
                        var saveAutoResponseParams = new SaveAutoResponseParams()
                        {
                            LogMessage = auditLogDescp,
                            UserId = 1,
                            OrganisationId = sessionValues.organisationId

                        };
                        long auditLogResult = nenNotificationService.SaveNotificationAutoResponseAuditLog(saveAutoResponseParams);

                        // long auditLogResult = nenNotificationService.SaveNotifAutoResponseAuditLog(auditLogDescp, 1, sessionValues.organisationId);
                    }

                    Session["PreferenceNotesBLOB"] = null;

                }
                else
                {
                    //if (!flag)
                    //    ResponseMsg = "User preferences not saved succesfully.";

                    if (!result)
                    {
                        flag = false;
                        ResponseMsg = "Response message not saved succesfully.";
                    }
                }

                //if (!flag)
                //    ResponseMsg = "User preferences not saved succesfully.";

                if (!result)
                {
                    flag = false;
                    ResponseMsg = "Response message not saved succesfully.";
                }

                if (ResStatus == 1 && inbByteVar == null)
                {
                    ResponseMsg = "Please provide Response message.";
                }
                Session["ResStatus"] = ResStatus;

                if (filenull != "true")
                {
                    Session["PreferenceNotesBLOB"] = null;
                }


                sessionValues.VehicleUnits = objUserPref.VehicleUnits;
                sessionValues.RoutePlanUnits = objUserPref.RouteplanUnit;


            }
            catch (Exception e)
            {

            }
            return Json(new { result = flag, ResMsg = ResponseMsg });

        }
        #endregion

        #region UserPrefer
        [HttpPost]
        public JsonResult SetUserPreference(int VehiUnits = 0, int DriveInstr = 0, string IsEnable = "", string CommMethod = "", int MaxListItems = 10, string EmailUpdate = "", string IsXMLAttached = "")
        {
            int UserId = 0;
            bool flag = false;
            int resultflag = 0;
            bool isemailvalid = false;
            var sessionValues = (STP.Domain.SecurityAndUsers.UserInfo)Session["UserInfo"];
            int portalType = sessionValues.UserTypeId;
            ViewBag.portaltype = portalType;
            if (portalType != 696006)
            {
                isemailvalid = CheckIsMailValid(EmailUpdate);
            }
            if (portalType == 696006)
            {
                resultflag = 1;
                Session["PageSize"] = MaxListItems;
            }
            if (isemailvalid == false && portalType != 696006)
            {
                resultflag = 2;

                return Json(resultflag, JsonRequestBehavior.AllowGet);
            }
            long OrgId;
            UserPreferenceParams objUserPreferenceParams = new UserPreferenceParams();
            UserPreferences objUserPref = new UserPreferences();
            if (portalType != 696006)
            {
                try
                {
                    objUserPref.VehicleUnits = Convert.ToInt32(VehiUnits);
                    objUserPref.RouteplanUnit = Convert.ToInt32(DriveInstr);
                    objUserPref.IsEnable = Convert.ToBoolean(IsEnable);
                    objUserPref.CommonMethod = Convert.ToInt32(CommMethod);
                    objUserPref.MaxListItems = Convert.ToInt32(MaxListItems);
                    objUserPref.IsXMLAttached = Convert.ToBoolean(IsXMLAttached);
                    

                    UserId = Convert.ToInt32(sessionValues.UserId);
                    if (Session["UserInfo"] != null)
                    {
                        UserId = Convert.ToInt32(sessionValues.UserId);
                        OrgId = sessionValues.organisationId;

                    }
                    else
                        return Json(resultflag, JsonRequestBehavior.AllowGet);

                    if (MaxListItems == 0)
                    {
                        MaxListItems = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;


                    }
                    //else
                    //{
                    //    Session["PageSize"] = MaxListItems;
                    //}
                    objUserPreferenceParams.ObjUserPreference = objUserPref;
                    objUserPreferenceParams.UserId = UserId;
                    objUserPreferenceParams.EmailUpdate = EmailUpdate;


                    flag = userService.SetUserPreference(objUserPreferenceParams);

                    if (flag == true)
                    {
                        resultflag = 1;
                        sessionValues.Email = EmailUpdate;
                        sessionValues.VehicleUnits = objUserPref.VehicleUnits;
                        sessionValues.RoutePlanUnits = objUserPref.RouteplanUnit;
                        Session["PageSize"] = MaxListItems;
                    }
                    return Json(resultflag, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {

                }
                return Json(resultflag, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    objUserPref.MaxListItems = Convert.ToInt32(MaxListItems);
                
                UserId = Convert.ToInt32(sessionValues.UserId);
                if (Session["UserInfo"] != null)
                {
                    UserId = Convert.ToInt32(sessionValues.UserId);
                    OrgId = sessionValues.organisationId;

                }
                else
                    return Json(resultflag, JsonRequestBehavior.AllowGet);

                if (MaxListItems == 0)
                {
                    MaxListItems = (Session["PageSize"] != null) ? (int)Session["PageSize"] : 10;


                }
                //else
                //{
                //    Session["PageSize"] = MaxListItems;
                //}
                objUserPreferenceParams.ObjUserPreference = objUserPref;
                objUserPreferenceParams.UserId = UserId;
                objUserPreferenceParams.EmailUpdate = EmailUpdate;


                flag = userService.SetUserPreference(objUserPreferenceParams);

                if (flag == true)
                {
                    resultflag = 1;
                    sessionValues.Email = EmailUpdate;
                    sessionValues.VehicleUnits = objUserPref.VehicleUnits;
                    sessionValues.RoutePlanUnits = objUserPref.RouteplanUnit;
                      objUserPref.MaxListItems = MaxListItems;
                }
                return Json(resultflag, JsonRequestBehavior.AllowGet);
            }
                catch (Exception e)
            {

            }
            return Json(resultflag, JsonRequestBehavior.AllowGet);
        }
    }
        #endregion

        private bool CheckIsMailValid(string email)
        {
            var trimmedEmail = email.Trim();



            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch (FormatException e)
            {
                return false;
            }
        }


        [HttpPost]
        public ActionResult ViewResponseMsg(string fileName, int flag)
        {
            try
            {
                string source = string.Empty;
                string hnresult = string.Empty;
                string FirstSec = string.Empty;

                hnresult = (string)Session["PreferenceNotes"];

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                long UserId = Int32.Parse(SessionInfo.UserId);
                int OrganisationId = (Int32)SessionInfo.OrganisationId;

                //string userschema = "STP_USER_PREFERENCES";
                //Byte[] ResponseStatus = UserRegProvider.Instance.GetMovHaulierNotes(UserId,OrganisationId, userschema);
                MailResponse responseMailDetails = notificationService.GetResponseMailDetails(OrganisationId);
                if (responseMailDetails.ReplyMailText != null)
                {
                    Byte[] ReplyMailText = responseMailDetails.ReplyMailText;
                    string bitString = BitConverter.ToString(ReplyMailText);
                    string utfString = Encoding.UTF8.GetString(ReplyMailText, 0, ReplyMailText.Length);
                    Console.WriteLine(utfString);

                    // ASCII conversion - string from bytes  
                    string asciiString = Encoding.ASCII.GetString(ReplyMailText, 0, ReplyMailText.Length);
                    Console.WriteLine(asciiString);
                    string result = System.Text.Encoding.UTF8.GetString(ReplyMailText);
                }
                if (hnresult == null)
                {
                    ViewBag.NotesforHaulier = hnresult;

                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                    string errormsg = "";
                    hnresult = STP.Common.General.XsltTransformer.Trafo(responseMailDetails.ReplyMailText, path, out errormsg);

                    source = hnresult;
                    string split = "</body>";
                    if (responseMailDetails.ReplyMailPdf != null && responseMailDetails.ReplyMailPdf != "")
                    {
                        FirstSec = source.Substring(0, source.IndexOf(split) + split.Length);

                        string Lastsec = source.Substring(source.IndexOf(split) + split.Length);


                        string Viewpdf = "<p>Please <a href=" + "#" + " " + " onclick=" + "ViewPDF();" + " >click here</a> to see Automated Response PDF.</p>";

                        hnresult = FirstSec;
                    }

                }
                else
                {
                    source = hnresult;
                    string split = "</body>";

                    if (responseMailDetails.ReplyMailPdf != null && responseMailDetails.ReplyMailPdf != "")
                    {
                        FirstSec = source.Substring(0, source.IndexOf(split) + split.Length);

                        string Lastsec = source.Substring(source.IndexOf(split) + split.Length);


                        string Viewpdf = "<p>Please <a href=" + "# " + " " + " onclick=" + "ViewPDF();" + " >click here</a> to see Automated Response PDF.</p>";

                        hnresult = FirstSec;
                    }

                }
                string resultData = hnresult;
                // return Json(new { page = FirstSec, role = flag});
                return Json(resultData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GetHtmlPage, Exception: {0}", ex));
                throw ex;
            }
        }
        public ActionResult ViewResponsePdfDoc(string Pdf)
        {
            try
            {


                int UserId = 0;
                long OrganisationId = 0;
                string ResponseMsg = null;
                var sessionValues = (STP.Domain.SecurityAndUsers.UserInfo)Session["UserInfo"];
                if (Session["UserInfo"] != null)
                {
                    UserId = Convert.ToInt32(sessionValues.UserId);
                    OrganisationId = sessionValues.OrganisationId;
                    ResponseMsg = userService.GetReplyMailPDF(OrganisationId, "STP_USER_PREFERENCES");
                }
                if (!System.IO.File.Exists(ResponseMsg))
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DocumentConsole/ViewResponsePdfDoc, FileName {0} not exist", ResponseMsg));
                    ViewBag.NoDataFound = "1";
                    return View();
                }
                //byte[] toBytes = STP.RouteAssessment.RouteAssessmentDomain.StringExtractor.GenerateInstrPDF(ResponseMsg);

                byte[] bytes = System.IO.File.ReadAllBytes(ResponseMsg);
                if (bytes != null)
                {
                    MemoryStream pdfStream = new MemoryStream();

                    pdfStream.Write(bytes, 0, bytes.Length);

                    pdfStream.Position = 0;

                    return new FileStreamResult(pdfStream, "application/pdf");
                }

                else
                {
                    ViewBag.NoDataFound = "1";
                    return View();
                }

            }
            catch
            {
                return View();

            }
        }

        public FileResult ViewResponsePdf()
        {
            int UserId = 0;
            int OrganisationId = 0;
            string ResponseMsg = null;
            if (Session["UserInfo"] != null)
            {
                var sessionValues = (STP.Domain.SecurityAndUsers.UserInfo)Session["UserInfo"];
                UserId = Convert.ToInt32(sessionValues.UserId);
                OrganisationId = Convert.ToInt32(sessionValues.organisationId);

                ResponseMsg = userService.GetReplyMailPDF(OrganisationId, "STP_USER_PREFERENCES");

            }

            //byte[] FileBytes = (byte[])Session["PreferenceNotesBLOB"];
            return File(ResponseMsg, "application/pdf");
        }


        //ViewResponseMailPdf() will be invoked when user will click on pdf link from email
        [AllowAnonymous]
        [SessionValidate(Disable = true)]
        public ActionResult ViewResponseMailPdf(string FileName = "")
        {
            try
            {
                string PathWithoutFileName = Convert.ToString(ConfigurationManager.AppSettings["PdfPathInServer"]);
                string pdfPath = Path.Combine(PathWithoutFileName, FileName);
                if (!System.IO.File.Exists(pdfPath))
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DocumentConsole/ViewResponseMailPdf, FileName {0} not exist", FileName));
                    ViewBag.NoDataFound = "1";
                    return View();
                }
                byte[] bytes = System.IO.File.ReadAllBytes(pdfPath);
                if (bytes != null)
                {
                    MemoryStream pdfStream = new MemoryStream();

                    pdfStream.Write(bytes, 0, bytes.Length);

                    pdfStream.Position = 0;

                    return new FileStreamResult(pdfStream, "application/pdf");
                }

                else
                {
                    ViewBag.NoDataFound = "1";
                    return View();
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("DocumentConsole/ViewResponseMailPdf, Exception: {0}", ex));
                throw ex;
            }

        }
        #region public ActionResult HaulierNotes()
        public ActionResult ResponseMessage(long VersionId)
        {
            try
            {

                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");
                else
                {
                    string result = string.Empty;
                    result = (string)Session["PreferenceNotes"];
                    if (result != null)
                    {
                        ViewBag.NotesforHaulier = result;
                    }
                    else
                    {

                        byte[] ResponseStatus = (byte[])Session["PreferenceNotesBLOB"];
                        string hnresult = string.Empty;
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\SOGeneral.xslt");
                        string errormsg = "";
                        hnresult = STP.Common.General.XsltTransformer.Trafo(ResponseStatus, path, out errormsg);

                        ViewBag.NotesforHaulier = hnresult;

                    }

                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                //Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Application/HaulierNotes, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion public ActionResult HaulierNotes()
        #endregion
        
        [SessionValidate(Disable = true)]
        public ActionResult ForgetUserName(string userEmail = null)
        {

            if (userEmail != null)
            {
                int result = authenticationService.GetUsername(userEmail);
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return View();

            }
        }
        private static Random random = new Random();
        public static string RandomString()
        {

            const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [SessionValidate(Disable = true)]
        [AllowAnonymous]
        [HttpPost]
       
        public ActionResult ForgetPassword(string userName = null, string email = null)
        {
            if (userName != null)
            {
                string OTP = RandomString();
                GenericController gc = new GenericController();
                string otpEncrypted = gc.MD5Encryption(OTP);
                OTPPasswordUpdation otppasswordUpdation = new OTPPasswordUpdation()
                {
                    UserName = userName,
                    OTPPassword = otpEncrypted,
                    UserEmail = email
                };
                int result1 = authenticationService.GenerateOTP(otppasswordUpdation);
                if (result1 > 0)
                {
                    string loginUl = ConfigurationManager.AppSettings["esdalLoginUrl"];
                    string str = "<body><a href ='" + loginUl + "'>Login</a></body>";
                    string esdalmailid = "mailto:esdalenquiries@nationalhighways.co.uk";
                    string mailid= "<body><a href ='" + esdalmailid + "'>esdalenquiries@nationalhighways.co.uk</a></body>";

                    string mailContent = "Hi " + userName + ",<p>You recently asked us to reset your ESDAL account password.</p><p>The One Time Password (OTP) for login to ESDAL is: " + OTP + "</p><p> Click the link below to sign into your account and choose a new password:</p><p> " + str + "</p><p> This password is only valid for the next 60 minutes. After this, you will need to request a new password.</p> <p>If you did not request a password reset, please contact the ESDAL helpdesk.</p><p>Thanks,</p> ESDAL Helpdesk Team<br /> T : 0300 470 3733 (8am - 6pm Mon - Fri excluding bank holidays)<br /> E : "+ mailid;
                    string subject = "ESDAL Password Reset";
                    string Matter = mailContent;
                    byte[] content = Encoding.ASCII.GetBytes(Matter);
                    byte[] attachment = new byte[0];
                    CommunicationParams communicationParams = new CommunicationParams
                    {
                        UserEmail = email,
                        Subject = subject,
                        Content = content,
                        Attachment = attachment,
                        ESDALReference = null
                    };
                    bool isEmailSend = communicationService.SendGeneralmail(communicationParams);
                    if (!isEmailSend)
                        return Json("0", JsonRequestBehavior.AllowGet);
                    else
                        return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("-3", JsonRequestBehavior.AllowGet);
                } 
            }
            else
            {
                return View();
            }
        }
        [SessionValidate(Disable = true)]
        public ActionResult ForgetPassword()
        {
            return View();
        }


        [AuthorizeUser(Roles = "11001,13003,13004,13005,13006")]
        public ActionResult ManageUsers(int? page, int? pageSize, string SearchString = null, string SearchType = null, bool checkBoxVal = false, string SORT = "", bool SelectButton = false, bool ContactFlag = false, bool ContactdisabFlag = false)
        {
            UserContactSearchItems objUserSearch = new UserContactSearchItems();
            if (Session["g_UserContactSearch"] != null)
                objUserSearch = (UserContactSearchItems)Session["g_UserContactSearch"];

            objUserSearch.DisabledUsersFlag = objUserSearch.DisabledContacts || objUserSearch.DisabledUsers ? 1 : 0;
            objUserSearch.ShowContactsFlag = objUserSearch.DisabledContacts || objUserSearch.ShowContacts ? 1 : 0;
            objUserSearch.DisabledContactsFlag = objUserSearch.DisabledContacts ? 1 : 0;

            if (objUserSearch.SearchType == "1")
                objUserSearch.UserName = objUserSearch.Criteria;
            else if (objUserSearch.SearchType == "2")
                objUserSearch.FirstName = objUserSearch.Criteria;
            else if (objUserSearch.SearchType == "3")
                objUserSearch.SurName = objUserSearch.Criteria;
            else if (objUserSearch.SearchType == "4")
                objUserSearch.OrganisationName = objUserSearch.Criteria;
           
            if (Session["UserInfo"] == null)
                return RedirectToAction("Login", "Account");

            if (TempData["Success"] != null)
            {
                string tempValue = Convert.ToString(TempData["Success"]);
                if (tempValue == "Edit")
                {
                    ViewBag.saveMsg = "User " + TempData["UserName"] + " updated successfully.";
                    ViewBag.action = "Edit";
                }
                else if (tempValue == "EditContact")
                {
                    ViewBag.saveContactMsg = "Contact user " + TempData["UserName"] + " updated successfully.";
                    objUserSearch.ShowContactsFlag = 1;
                    ViewBag.action = "Edit";
                }
                else if (tempValue == "ContactUser")
                {
                    ViewBag.saveContactMsg = "New contact user " + TempData["UserName"] + " saved successfully.";
                    objUserSearch.ShowContactsFlag = 1;
                    ViewBag.action = "New";
                }
                else
                {
                    ViewBag.saveMsg = "New user " + TempData["UserName"] + " created successfully.";
                    ViewBag.action = "New";
                }
            }

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.IsAdmin == 0 && SessionInfo.UserTypeId != 696008)
                return RedirectToAction("Error", "Home");
            int maxlist_item = SessionInfo.MaxListItem;
            if (Session["PageSize"] == null)
                Session["PageSize"] = maxlist_item;
            if (pageSize == null)
                pageSize = ((Session["PageSize"] != null) && (Convert.ToString(Session["PageSize"]) != "0")) ? (int)Session["PageSize"] : 10;
            else
                Session["PageSize"] = pageSize;

            int pageNumber = (page ?? 1);

            objUserSearch.SortOrderValue = objUserSearch.SortOrderValue != null ? (int)objUserSearch.SortOrderValue : 0; //name
            objUserSearch.SortTypeValue = objUserSearch.SortTypeValue != null ? (int)objUserSearch.SortTypeValue : 0; // asc
            ViewBag.SortOrder = objUserSearch.SortOrderValue;
            ViewBag.SortType = objUserSearch.SortTypeValue;
            List<GetUserList> GridListObj = null;
            if (SORT != "SORTSO")
                GridListObj = userService.UserList(Convert.ToString(SessionInfo.UserTypeId), Convert.ToString(SessionInfo.OrganisationId), pageNumber, (int)pageSize, objUserSearch, (int)objUserSearch.SortTypeValue, (int)objUserSearch.SortOrderValue);
            long totalCount = 0;
            if (GridListObj != null && GridListObj.Count > 0)
                totalCount = GridListObj[0].TotalRecordCount;

            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;
            ViewBag.SORT = SORT;
            ViewBag.SelectButton = SelectButton;
            ViewBag.IsHelpDeskAdmin = IsHelpDeskAdmin(SessionInfo.UserTypeId, SessionInfo.IsAdmin);
            ViewBag.TotalCount = totalCount;
            var userPagedList = new StaticPagedList<GetUserList>(GridListObj, pageNumber, (int)pageSize, (int)totalCount);
            ViewBag.UserSearch = objUserSearch;
            return View(userPagedList);
        }

        public bool IsHelpDeskAdmin(int userType, int isAdmin)
        {
            bool isHelpDesk = false;
            if (userType == 696006 && isAdmin == 1)
                isHelpDesk = true;
            return isHelpDesk;
        }

        public ActionResult LoginAsAnotherUser(string user)
        {
            UserInfo tempHelpDeskUser = null;
            if (Session["UserInfo"] != null)
            {
                var sessn = (UserInfo)Session["UserInfo"];
                tempHelpDeskUser = (UserInfo)Session["UserInfo"];
                if (sessn.IsAdmin == 0 && sessn.UserTypeId != 696006)
                    return RedirectToAction("Error", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            UserInfo userInfo = authenticationService.GetAnotherLogin(user);
            #region
            userInfo.HelpdeskLoginAsAnotherUser = true;
            userInfo.HelpdeskUserName = tempHelpDeskUser.UserName;
            userInfo.HelpdeskUserId = tempHelpDeskUser.UserId;
            #endregion
            #region

            if (userInfo.HelpdeskLoginAsAnotherUser)
            {
                SessionLengthModel sessionLengthModel = new SessionLengthModel();
                sessionLengthModel.EventType = 401003;
                sessionLengthModel.StartDate = DateTime.Now;
                sessionLengthModel.LoginId = Convert.ToInt64(userInfo.UserId);
                sessionLengthModel.MachineName = System.Environment.MachineName;
                string helpDeskUserstring = "";// "(Logged in by helpdesk User " + userInfo.HelpdeskUserName + ")";
                sessionLengthModel.Description = "User " + userInfo.UserName + " Logged In" + helpDeskUserstring;
                reportService.LogEvent((int)sessionLengthModel.EventType, (int)sessionLengthModel.LoginId, sessionLengthModel.Description);
            }
            #endregion
            #region clear all session
            Session.RemoveAll();
            #endregion

            if (userInfo.UserTypeId == 696008)
            {
                userInfo.UserSchema = UserSchema.Sort;
            }
            Session["UserInfo"] = userInfo;
            Session["UserTypeId"] = userInfo.UserTypeId;
            Session["ProjectVersion"] = ConfigurationManager.AppSettings["projectVersion"];
            SetMenuPrivelege();

            #region Added for Login from help desk
            EncryptionUtility.SetEncryptionKey(userInfo.OrganisationId.ToString());
            Session["maploadCheckflag"] = false;
            Session["routedisplayCheckflag"] = false;
            Session["routeplanCheckflag"] = false;

            #region Handle session hijacking
            string browserInfo = Request.Browser.Browser +
                Request.Browser.Version +
                Request.UserAgent + "~" +
                Request.ServerVariables["REMOTE_ADDR"];

            string sessionValue = Convert.ToString(Session["UserId"]) + "^" +
                DateTime.Now.Ticks + "^" +
                browserInfo + "^" +
                System.Guid.NewGuid();

            byte[] encodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sessionValue);
            string encryptedString = System.Convert.ToBase64String(encodeAsBytes);
            Session["encryptedSession"] = encryptedString;
            #endregion
            #endregion

            Session["Logged_In"] = 1;
            if (userInfo.UserTypeId == 696001)
                Session["ViewMap"] = 2;

            if (userInfo.UserTypeId == 696001)
            {
                ViewBag.PortalType = Constants.Haulier;
                return Json(new { redirectToUrl = Url.Action("Hauliers", "Home") });
            }
            else if (userInfo.UserTypeId == 696002 || userInfo.UserTypeId == 696007)
            {
                ViewBag.PortalType = userInfo.UserTypeId == 696007 ? Constants.SOA : Constants.Police;
                return Json(new { redirectToUrl = Url.Action("MovementInboxList", "Movements") });
            }
            else if (userInfo.UserTypeId == 696006)
            {
                ViewBag.PortalType = Constants.Admin;
                return Json(new { redirectToUrl = Url.Action("Index", "Helpdesk") });
            }
            else if (userInfo.UserTypeId == 696008)
            {
                ViewBag.PortalType = Constants.SORT;
                return Json(new { redirectToUrl = Url.Action("SORT", "Home") });
            }
            else
            {
                ViewBag.PortalType = Constants.Admin;
                return Json(new { redirectToUrl = Url.Action("Login", "Account") });
            }
        }

        #region public ActionResult CheckAsSOAPoliceHaulier(string ESDALREf = null, long orgid = 0, long contactid = 0, int trsmission_id = 0)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ESDALREf">Esdal reference in active trasaction</param>
        /// <param name="orgid">organisation id of TO organisation</param>
        /// <param name="contactid">To contact ID</param>
        /// <param name="trsmission_id">transaction id in active trasaction</param>
        /// <param name="checkAs">check as SOA,Police,Haulier(check for which button is clicked)</param>
        /// <returns></returns>
        
        [HttpGet]
        public ActionResult CheckAsSOAPoliceHaulier(string ESDALREf = null, long orgid = 0, long contactid = 0, int trsmission_id = 0, string checkAs = null)
        {
            DistributionAlerts DistributionObj = new DistributionAlerts();
            UserInfo Userdetails, userInfo_Redirect, helpdesk_sessn;
            List<string> arrlist = new List<string>();
            string NonEsdalContact = "TRUE";
            string ISDeleted = "TRUE";
            string IsNotification = "false";
            try
            {
                Userdetails = GetUserDetails(ESDALREf, checkAs, orgid, contactid);// storing SOA/POLICE/HAULIER details
                if (Userdetails.UserName == null || Userdetails.UserName == "")
                {
                    Userdetails.IsValidUser = false;
                    ViewBag.NonESDALUSER = NonEsdalContact;
                }
                else if (Userdetails.IsDeleted != 0)
                {
                    Userdetails.IsValidUser = false;
                    ViewBag.ISDeleted = ISDeleted;
                }
                else
                {
                    if (Session["UserInfo"] != null)
                    {
                        helpdesk_sessn = (UserInfo)Session["UserInfo"];
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    userInfo_Redirect = authenticationService.GetAnotherLogin(Userdetails.UserName);
                    #region clear all session
                    Session.RemoveAll();
                    #endregion

                    if (userInfo_Redirect.UserTypeId == 696008)
                    {
                        userInfo_Redirect.UserSchema = UserSchema.Sort;
                    }
                    userInfo_Redirect.HelpdeskRedirect = "true";
                    userInfo_Redirect.HelpdeskUserId = helpdesk_sessn.UserId;
                    userInfo_Redirect.HelpdeskUserName = helpdesk_sessn.UserName;
                    Session["UserInfo"] = userInfo_Redirect;
                    SetMenuPrivelege();

                    if (userInfo_Redirect.UserTypeId == 696007 || userInfo_Redirect.UserTypeId == 696002)
                    {
                        arrlist = GetSplitESDAL(ESDALREf);
                        DistributionObj = documentService.GetSOAPoliceDetails(ESDALREf, trsmission_id);
                        return Json(new { notif_id = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(DistributionObj.NotificationId)), ESDALREf = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(ESDALREf)), Item_Type = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(DistributionObj.ItemType)), Inbox_Id = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(DistributionObj.InboxId)), userType_ID = userInfo_Redirect.UserTypeId }, JsonRequestBehavior.AllowGet);
                    }
                    else if (userInfo_Redirect.UserTypeId == 696001)
                    {
                        bool isNotif = ESDALREf.Contains('#');
                        if (isNotif)
                        {
                            IsNotification = "true";
                            DistributionObj = documentService.GetNotifDetails(ESDALREf, trsmission_id);
                            return Json(new { notif_id = DistributionObj.NotificationId, ESDALREf = DistributionObj.ESDALReference, VehicleCode = DistributionObj.VehicleType, userType_ID = userInfo_Redirect.UserTypeId, IsNotification = IsNotification }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            arrlist = GetSplitESDAL(ESDALREf);
                            DistributionObj = documentService.GetHaulierDetails(arrlist[0], arrlist[1], arrlist[2]);
                            return Json(new { Revision_id = DistributionObj.RevisionId, Veh_purpose = DistributionObj.VehiclePurpose, Version_id = DistributionObj.VersionId, ESDALREf = ESDALREf, Revision_no = DistributionObj.RevisionNo, Version_no = DistributionObj.VersionNo, Project_id = DistributionObj.ProjectId, pageflag = 2, userType_ID = userInfo_Redirect.UserTypeId, IsNotification = IsNotification }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return null;
                //return RedirectToAction("ViewDistributionStatus", "DistributionStatus", new { page = 1, pageSize = 10, nonesdal = ViewBag.NonESDALUSER, ISDeleted = ViewBag.ISDeleted });
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Error Occurred.";
                return RedirectToAction("ViewDistributionStatus", "DistributionStatus", new
                {
                    B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("page=1" +
                        "&pageSize=10" +
                        "&ErrorMessage=" + ViewBag.ErrorMessage)
                });
            }
        }
        #endregion

        #region GetUserDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ESDALREf"></param>
        /// <param name="checkAs">SOA,Police,Haulier</param>
        /// <param name="orgid"></param>
        /// <param name="contactid"></param>
        /// <returns></returns>
        public UserInfo GetUserDetails(string ESDALREf = null, string checkAs = null, long orgid = 0, long contactid = 0)
        {
            UserInfo Userdetails = new UserInfo();
            List<string> arrlist = new List<string>();
            if (checkAs == "Haulier")
            {
                bool isNotif = ESDALREf.Contains('#');
                if (isNotif)
                {
                    Userdetails = documentService.GetUserDetailsForNotification(ESDALREf);
                }
                else
                {
                    arrlist = GetSplitESDAL(ESDALREf);
                    Userdetails = documentService.GetUserDetailsForHaulier(arrlist[0], arrlist[1]);
                }
            }
            else if (checkAs == "SOA" || checkAs == "Police")
            {
                Userdetails = documentService.GetUserName(orgid, contactid);
            }
            return Userdetails;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordInfo changePasswordInfo, int type = 4)//, string existingpassword = null, string password = null
        {
            var sessionValues = (STP.Domain.SecurityAndUsers.UserInfo)Session["UserInfo"];

            changePasswordInfo.UserId = Convert.ToInt32(sessionValues.UserId);
           
            if (type == 6)//Updating old password with new after login with  OTP method.
            {
                GenericController gc = new GenericController();
                changePasswordInfo.OldPassword = sessionValues.Password;
                changePasswordInfo.ExistingPassword = gc.MD5Encryption(changePasswordInfo.ExistingPassword);
                if (changePasswordInfo.OldPassword != changePasswordInfo.ExistingPassword)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //To get Uk time zone
                    var BritishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                    UserInfo userInfo = (UserInfo)Session["UserInfo"];

                    DateTime dt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

                    DateTime DateTimeInBritishLocal = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, BritishZone);
                    userInfo.PasswordUpdatedOn = userInfo.PasswordUpdatedOn.AddMinutes(Convert.ToInt32(60));

                    int res2 = DateTime.Compare(DateTimeInBritishLocal, userInfo.PasswordUpdatedOn);

                    if (dt > userInfo.PasswordUpdatedOn)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        changePasswordInfo.NewPassword = gc.MD5Encryption(changePasswordInfo.NewPassword);
                        int result = authenticationService.UpdateExpiryPassword(changePasswordInfo);

                        if (result > 0)
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            return Json(3, JsonRequestBehavior.AllowGet);

                        }
                    }

                }
            }
            else
            {

                GenericController gc = new GenericController();
                changePasswordInfo.UserId = Convert.ToInt32(sessionValues.UserId);
                changePasswordInfo.ExistingPassword = sessionValues.Password;
                bool showTandConlyAndPass = (bool)Session["showTandConlyAndPass"];
                bool showTermsandconditiononly = (bool)Session["showTermsandconditiononly"];
                if (showTandConlyAndPass && !showTermsandconditiononly)
                {
                    var UserID = Session["UserIDToUpdateTAndC"];
                    SetTermsAndConditions((string)UserID);
                }
                if (sessionValues.LastLogin == DateTime.MinValue)
                {
                    //------------------------------------------------------------
                    #region movement actions for this action method
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.user_accepted_terms_and_cond;
                    string ErrMsg = string.Empty;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    #endregion
                }
                changePasswordInfo.OldPassword = gc.MD5Encryption(changePasswordInfo.OldPassword);
                changePasswordInfo.NewPassword = gc.MD5Encryption(changePasswordInfo.NewPassword);

                if (ModelState.IsValid)
                {

                    if (changePasswordInfo.ExistingPassword != changePasswordInfo.OldPassword)
                    {

                        SecurityQuestion();
                        ModelState.AddModelError("OldPassword", "Wrong Password");

                        return View(changePasswordInfo);
                    }
                    else if (changePasswordInfo.OldPassword == changePasswordInfo.NewPassword)
                    {
                        SecurityQuestion();
                        ModelState.AddModelError("NewPassword", "You cannot reuse the recent password");
                        return View(changePasswordInfo);
                    }
                    if (sessionValues.LastLogin == DateTime.MinValue)
                    {
                        authenticationService.UpdatePassword(changePasswordInfo);
                        SetTermsAndConditions(sessionValues.UserId);
                    }
                    else
                    {
                        authenticationService.UpdateExpiryPassword(changePasswordInfo);
                        SetTermsAndConditions(sessionValues.UserId);
                    }

                    TempData["success"] = "1";
                    Session["RedirectToChangePwd"] = null;

                    #region log event
                    UserInfo SessionInfo = null;
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    string description = "Password changed successfully by " + SessionInfo.UserName;
                    Session["PasswordUpdateFlag"] = "1";
                    sessionValues.Password = changePasswordInfo.NewPassword;
                    reportService.LogEvent(401045, Convert.ToInt32(SessionInfo.UserId), description);
                    #endregion log event

                    if (SessionInfo.UserTypeId == 696002)
                    {
                        return RedirectToAction("MovementInboxList", "Movements");
                    }
                    else if (SessionInfo.UserTypeId == 696007)
                    {
                        return RedirectToAction("MovementInboxList", "Movements");
                    }
                    else if (SessionInfo.UserTypeId == 696006)
                    {
                        return RedirectToAction("Index", "Helpdesk");

                    }
                    else if (SessionInfo.UserTypeId == 696008)
                    {

                        return RedirectToAction("SORT", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Hauliers", "Home");
                    }
                }
                return PartialView("PartialView/_ChangePassword.cshtml", changePasswordInfo);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult UpdateUserPassword(string oldpassword = null, string newpassword = null)
        {
            GenericController gc = new GenericController();

            ChangePasswordInfo changePasswordInfo = new ChangePasswordInfo();
            changePasswordInfo.NewPassword = newpassword;
            changePasswordInfo.OldPassword = oldpassword;

            var sessionValues = (UserInfo)Session["UserInfo"];

            changePasswordInfo.UserId = Convert.ToInt32(sessionValues.UserId);
            changePasswordInfo.ExistingPassword = sessionValues.Password;
            bool showTandConlyAndPass = false;
            if (Session["showTandConlyAndPass"] != null)
            {
                showTandConlyAndPass = (bool)Session["showTandConlyAndPass"];
            }
            //bool showTandConlyAndPass = (bool)Session["showTandConlyAndPass"];
            bool showTermsandconditiononly = false;
            if (Session["showTermsandconditiononly"] != null)
            {
                showTandConlyAndPass = (bool)Session["showTandConlyAndPass"];
            }
            //bool showTermsandconditiononly = (bool)Session["showTermsandconditiononly"];
            if (showTandConlyAndPass && !showTermsandconditiononly)
            {
                var UserID = Session["UserIDToUpdateTAndC"];
                SetTermsAndConditions((string)UserID);
            }
            if (sessionValues.LastLogin == DateTime.MinValue)
            {
                //------------------------------------------------------------
                #region movement actions for this action method
                UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                //movactiontype.movementActionType = MovementnActionType.user_accepted_terms_and_cond;
                string ErrMsg = string.Empty;
                //string MovementDescription = STP.Domain.Movements.MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                #endregion

            }

            changePasswordInfo.OldPassword = gc.MD5Encryption(changePasswordInfo.OldPassword);
            changePasswordInfo.NewPassword = gc.MD5Encryption(changePasswordInfo.NewPassword);
            string result = "Password update failed";
            if (ModelState.IsValid)
            {

                if (changePasswordInfo.ExistingPassword != changePasswordInfo.OldPassword)
                {

                    //SecurityQuestion();

                    ModelState.AddModelError("OldPassword", "Wrong Password");
                    result = "Wrong Password";
                    return Json(result, JsonRequestBehavior.AllowGet);
                    //return (result);
                }
                else if (changePasswordInfo.OldPassword == changePasswordInfo.NewPassword)
                {
                    //SecurityQuestion();

                    ModelState.AddModelError("NewPassword", "You cannot reuse the recent password");
                    result = "You cannot reuse the recent password";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }


                if (sessionValues.LastLogin == DateTime.MinValue)
                {
                    var passwordChanged = authenticationService.UpdatePassword(changePasswordInfo);
                }
                else
                {
                    var passwordChanged = authenticationService.UpdateExpiryPassword(changePasswordInfo);
                }

                #region log event
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                string description = "Password changed successfully by " + SessionInfo.UserName;
                Session["PasswordUpdateFlag"] = "1";
                sessionValues.Password = changePasswordInfo.NewPassword;
                //EventLogProvider.Instance.LogEvent(401045, Convert.ToInt32(SessionInfo.UserID), description);
                #endregion log event
                //int a = 1;
                TempData["success"] = "1";
                Session["RedirectToChangePwd"] = null;
                result = "Success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region SetTermsAndConditions
        public int SetTermsAndConditions(string user_id)
        {
            int result = 0;
            result = authenticationService.SaveTermsAndConditions(user_id);
            return result;
        }
        #endregion



        public ActionResult SetUserSearch(UserContactSearchItems objUserSearch,int? page=null,int pageSize=10)
        {
            Session["g_UserContactSearch"] = objUserSearch;
            return RedirectToAction("ManageUsers",new{ page=page,pageSize=pageSize});
        }

        [AllowAnonymous]
        public ActionResult ChangePassword(int needTerms = 1)
        {
            int ShowonlyNeedTerms = 0;
            int varTandConlyAndPass = 0;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var sessionValues = (UserInfo)Session["UserInfo"];

            bool showTermsandconditiononly = false;
            if (Session["showTermsandconditiononly"] != null)
            {
                showTermsandconditiononly = (bool)Session["showTermsandconditiononly"];
            }


            bool showTandConlyAndPass = false;
            if (Session["showTandConlyAndPass"] != null)
            {
                showTandConlyAndPass = (bool)Session["showTandConlyAndPass"];
            }


            SecurityQuestion();

            if (sessionValues.LastLogin == DateTime.MinValue || showTermsandconditiononly || showTandConlyAndPass)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Configuration.xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);


                XmlNode TermsConditions = xmlDocument.SelectSingleNode("/Configurable");
                XmlNodeList TermsConditionsList = TermsConditions.SelectNodes("TermsConditions");

                foreach (XmlNode Terms in TermsConditionsList)
                {
                    XmlNode acceptedTermIDNode = Terms.SelectSingleNode("acceptedTermID");

                    string acceptedTermID = acceptedTermIDNode.InnerText;

                    XmlNode requiredTermNode = Terms.SelectSingleNode("requiredTerm");

                    string requiredTerm = requiredTermNode.InnerText;

                    XmlNode TermsNode = Terms.SelectSingleNode("Terms");

                    string Term = TermsNode.InnerText;

                    ViewBag.acceptedTermID = acceptedTermID;
                    ViewBag.requiredTerm = requiredTerm;
                    ViewBag.Term = Term;

                }
                if (showTermsandconditiononly && !showTandConlyAndPass)
                {
                    ShowonlyNeedTerms = 1;
                }
                if (showTandConlyAndPass && !showTermsandconditiononly)
                {
                    varTandConlyAndPass = 1;
                }

            }

            ChangePasswordInfo changePasswordInfo = new ChangePasswordInfo();
            changePasswordInfo.UserId = Convert.ToInt32(sessionValues.UserId);
            changePasswordInfo.ExistingPassword = sessionValues.Password;

            ViewBag.NeededTerms = needTerms;
            ViewBag.ShowonlyNeedTerms = ShowonlyNeedTerms;
            ViewBag.varTandConlyAndPass = varTandConlyAndPass;
            return PartialView("PartialView/_ChangePassword");
        }

        private void SecurityQuestion()
        {
            var securityQuestionCollection = authenticationService.GetSecurityQuestion();
            SelectList securityQuestionData = new SelectList(securityQuestionCollection, "QuestionId", "SecurityQuestion");
            ViewBag.SecurityQuestionList = securityQuestionData;
        }

        public JsonResult CheckForPassword(string newPassword, string oldPassword)
        {
            GenericController gc = new GenericController();
            ChangePasswordInfo changePasswordInfo = new ChangePasswordInfo();
            string message = string.Empty;
            string OldPassword = gc.MD5Encryption(oldPassword);
            string NewPassword = gc.MD5Encryption(newPassword);
            changePasswordInfo.NewPassword = NewPassword;
            var sessionValues = (UserInfo)Session["UserInfo"];

            changePasswordInfo.UserId = Convert.ToInt32(sessionValues.UserId);
            int result = 0;

            if (OldPassword != sessionValues.Password && !string.IsNullOrEmpty(oldPassword))
            {
                message = "Wrong password";
            }
            else if (NewPassword == sessionValues.Password && !string.IsNullOrEmpty(newPassword))
            {
                message = "You cannot reuse the current password";
            }
            else
            {
                result = authenticationService.CheckNewPAssword(changePasswordInfo);
                if (result == -1)
                {
                    message = "The new password matches your previously used passwords. Please re-enter a new password.";
                }
                else
                {
                    message = "success";
                    Session["RedirectToChangePwd"] = null;
                }
            }
            return Json(new { result = message });
        }

        public ActionResult TermsAndConditions()
        {
            return PartialView("PartialView/_TermsAndConditions");
        }

        public ActionResult CookiesDetails()
        {
            return PartialView("PartialView/_Cookies");
        }

        public int SetLoginStatus()
        {
            int result = 0;
            int UserID = 0;
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            UserID = Convert.ToInt32(SessionInfo.UserId);
            Session["Logged_In"] = 1;
            result = notificationService.SetLoginStatus(UserID, 0);
            return result;
        }

        public ActionResult ShowTermsandconditiononly()
        {
            GenericController gc = new GenericController();
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            bool showTermsandconditiononly = (bool)Session["showTermsandconditiononly"];

            if (showTermsandconditiononly)
            {
                #region movement actions for this action method
                UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                UserSessionValue = (UserInfo)Session["UserInfo"];
                MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                movactiontype.MovementActionType = MovementnActionType.user_accepted_terms_and_cond;
                string ErrMsg = string.Empty;
                string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                #endregion

                #region log event
                string description = "Terms and conditions saved successfully by " + SessionInfo.UserName;
                reportService.LogEvent(401045, Convert.ToInt32(SessionInfo.UserId), description);
                Session["RedirectToChangePwd"] = null;
                #endregion log event

                SetTermsAndConditions(SessionInfo.UserId);
            }
            if (SessionInfo.UserTypeId == 696002)
            {
                return RedirectToAction("MovementInboxList", "Movements");
            }
            else if (SessionInfo.UserTypeId == 696007)
            {
                return RedirectToAction("MovementInboxList", "Movements");
            }
            else if (SessionInfo.UserTypeId == 696006)
            {
                return RedirectToAction("Index", "Helpdesk");

            }
            else if (SessionInfo.UserTypeId == 696008)
            {

                return RedirectToAction("SORT", "Home");
            }
            else
            {
                return RedirectToAction("Hauliers", "Home");
            }
        }
        [SessionValidate(Disable = true)]
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ValidatePassword(string password)
        {
            var input = password;
            string ErrorMessage = string.Empty;
            bool flagPassword = true;
            //if (string.IsNullOrWhiteSpace(input))
            //{
            //    throw new Exception("Password should not be empty");
            //}

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{6,12}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=[{\]};:<>.?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one lower case letter.";
                flagPassword= false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one upper case letter.";
                flagPassword = false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be lesser than 8 or greater than 15 characters.";
                flagPassword = false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one numeric value.";
                flagPassword = false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one special case character.";
                flagPassword = false;
            }
            else
            {
                flagPassword = true;
            }
            return Json(new { result = flagPassword, message = ErrorMessage });
        }

        public ActionResult SearchUserCriteria(UserContactSearchItems objUserSearch)
        {
            Session["g_UserContactSearch"] = objUserSearch;

            if (objUserSearch.DisabledUsers)
                objUserSearch.DisabledUsersFlag = 1;
            else
                objUserSearch.DisabledUsersFlag = 0;

            if (objUserSearch.ShowContacts)
                objUserSearch.ShowContactsFlag = 1;
            else
                objUserSearch.ShowContactsFlag = 0;

            if (objUserSearch.DisabledContacts)
            {
                objUserSearch.DisabledContactsFlag = 1;
                objUserSearch.DisabledUsersFlag = 1;
                objUserSearch.ShowContactsFlag = 1;
            }
            else
            {
                objUserSearch.DisabledContactsFlag = 0;
            }

            if (objUserSearch.SearchType == "1")
            {
                objUserSearch.UserName = objUserSearch.Criteria;
            }
            else if (objUserSearch.SearchType == "2")
            {
                objUserSearch.FirstName = objUserSearch.Criteria;
            }
            else if (objUserSearch.SearchType == "3")
            {
                objUserSearch.SurName = objUserSearch.Criteria;
            }
            else if (objUserSearch.SearchType == "4")
            {
                objUserSearch.OrganisationName = objUserSearch.Criteria;
            }

            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.IsAdmin == 0 && SessionInfo.UserTypeId != 696008)
            {
                return RedirectToAction("Error", "Home");
            }
            List<GetUserList> GridListObj = null;
            GridListObj = userService.SearchUserCriteria(Convert.ToString(SessionInfo.UserTypeId), Convert.ToString(SessionInfo.OrganisationId), objUserSearch);
            if (objUserSearch.SearchType == "1")
            {
                var userName = GridListObj.Select(x => x.UserName).Distinct();
                return Json(userName, "application/json", JsonRequestBehavior.AllowGet);
            }
            else if (objUserSearch.SearchType == "2")
            {
                var FirstName = GridListObj.Select(x => x.FirstName).Distinct();
                return Json(FirstName, "application/json", JsonRequestBehavior.AllowGet);
            }
            else if (objUserSearch.SearchType == "3")
            {
                var SurName = GridListObj.Select(x => x.SurName).Distinct();
                return Json(SurName, "application/json", JsonRequestBehavior.AllowGet);
            }
            else if (objUserSearch.SearchType == "4")
            {
                var OrganisationName = GridListObj.Select(x => x.OrgName).Distinct();
                return Json(OrganisationName, "application/json", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var DefaultName = GridListObj.Select(x => x.UserName).Distinct();
                return Json(DefaultName, "application/json", JsonRequestBehavior.AllowGet);
            }
        }
    }
}