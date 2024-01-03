using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.SecurityAndUsers;
using STP.Web.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace STP.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly ILoggingService loggingService;
        private readonly IAuthenticationService authenticationService;
        public UsersController()
        {
        }
        public UsersController(UserService userService, ILoggingService loggingService, IAuthenticationService authenticationService)
        {
            this.userService = userService;
            this.loggingService = loggingService;
            this.authenticationService = authenticationService;
        }

        [SessionValidate(Disable = true)]
        public ActionResult Login(string returnUrl = null)
        {
            if (Session["UserInfo"] == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                UserInfo userInfo = (UserInfo)Session["UserInfo"];
                if (returnUrl == null || returnUrl == "")
                {
                    if (userInfo.UserTypeId == 696002 || userInfo.UserTypeId == 696007)
                    {
                        return RedirectToAction("MovementInboxList", "Movements");
                    }
                    else if (userInfo.UserTypeId == 696006)
                    {
                        return RedirectToAction("Index", "Helpdesk");
                    }
                    else if (userInfo.UserTypeId == 696008)
                    {
                        return RedirectToAction("SORT", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Hauliers", "Home");
                    }
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }

        public ActionResult Error()
        {
            ViewBag.Message = "Error page.";

            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserRegistration regDet, string mode, int User_id = 0, int Contact_id = 0)
        {
            bool result = false;
            string AssignedRoles = "";
            if (regDet.DataHolder)
            {
                AssignedRoles = AssignedRoles + "226001" + ",";
            }
            if (regDet.NotificationContact)
            {
                AssignedRoles = AssignedRoles + "226002" + ",";
            }
            if (regDet.OfficialContact)
            {
                AssignedRoles = AssignedRoles + "226003" + ",";
            }
            if (regDet.ItContact)
            {
                AssignedRoles = AssignedRoles + "226006" + ",";
            }
            if (regDet.DataOwner)
            {
                AssignedRoles = AssignedRoles + "226008" + ",";
            }
            AssignedRoles = AssignedRoles + "0";

            regDet.Roletype = AssignedRoles;
            UserRegistration userobj = new UserRegistration();
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    if (Contact_id != 0 && Contact_id != null)
                    {
                        regDet.OnlyContact = true;
                    }
                    if (!regDet.OnlyContact)
                    {
                        regDet.ContactPref = null;
                    }
                    GenericController gc = new GenericController();
                    if (regDet.password != null)
                    {
                        regDet.password = gc.MD5Encryption(regDet.password);

                    }
                    regDet.Town = regDet.City;

                    if (regDet.OrgUser == "000")
                    {
                        regDet.OrgUser = string.Empty;
                    }

                    if (regDet.OnlyContact)
                    {
                        regDet.UserName = null;
                        regDet.password = null;
                        regDet.ConfirmPassword = null;
                        regDet.UserType = 0;
                        regDet.IsAdministrator = false;
                    }

                    var sessionValues = (UserInfo)Session["UserInfo"];
                    int user_ID = Convert.ToInt32(sessionValues.UserId);
                    string ErrMsg = string.Empty;
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    if (sessionValues.UserTypeId != 696006)
                    {
                        regDet.OrganisationId = sessionValues.OrganisationId;
                    }

                    if (mode == "Edit")
                    {
                        if (!regDet.OnlyContact)
                        {
                            TempData["Success"] = "Edit";
                            result = true;
                            UserParams updateUserParam = new UserParams()
                            {
                                RegDet = regDet,
                                UserTypeId = sessionValues.UserTypeId,
                                UserId = User_id
                            };
                            userService.UpdateUser(updateUserParam);

                            #region movement actions for this action method
                            UserInfo UserSessionValue = null;
                            if (Session["UserInfo"] == null)
                            {
                                return RedirectToAction("Login", "Account");
                            }
                            UserSessionValue = (UserInfo)Session["UserInfo"];
                            movactiontype.MovementActionType = MovementnActionType.admin_changes_users_detail;
                            movactiontype.HaulierName = regDet.UserName;
                            string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                            loggingService.SaveMovementAction("", (int)movactiontype.MovementActionType, MovementDescription,0,0,0, UserSessionValue.UserSchema);
                            #endregion

                            #region Saving updating user in sys_event
                            movactiontype.SystemEventType = SysEventType.amended_user;
                            movactiontype.UserName = regDet.UserName;
                            string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);
                            #endregion

                            //---------------------------------------------------------------------
                            //Added by Mahzeer (log for resetting password)
                            #region  Resetting password 
                            if (regDet.IsResetPW)
                            {
                                movactiontype.SystemEventType = SysEventType.reset_password;
                                movactiontype.UserName = regDet.UserName;
                                movactiontype.UserId = User_id;
                                sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);
                            }
                            #endregion
                        }
                        else
                        {
                            UserParams updateContactParam = new UserParams()
                            {
                                RegDet = regDet,
                                UserTypeId = sessionValues.UserTypeId,
                                ContactId = Contact_id
                            };

                            userService.UpdateContact(updateContactParam);
                            TempData["Success"] = "EditContact";
                            result = true;
                            #region Saving updating contact in sys_event
                            movactiontype.SystemEventType = SysEventType.amended_contact;
                            movactiontype.ContactName = sessionValues.FirstName + ' ' + sessionValues.LastName;
                            movactiontype.ContactId = sessionValues.ContactId;
                            string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);
                            #endregion
                        }
                    }
                    else
                    {
                        //for user assigned roles
                        UserParams setUserParams = new UserParams()
                        {
                            RegDet = regDet,
                            UserTypeId = sessionValues.UserTypeId

                        };
                        if (!regDet.OnlyContact)
                        {
                            TempData["Success"] = "Create";
                            result = true;
                            userService.SetRegInfo(setUserParams);


                            //----------------------------------------------------------------------
                            #region Saving creating user in sys_event
                            movactiontype.SystemEventType = SysEventType.added_user;
                            movactiontype.ContactName = sessionValues.FirstName + ' ' + sessionValues.LastName;
                            movactiontype.UserName = regDet.UserName;
                            movactiontype.ContactId = sessionValues.ContactId;
                            string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                            loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);
                            #endregion
                            //----------------------------------------------------------------------
                        }
                        else
                        {

                            userobj = userService.SetContactRegInfo(setUserParams);

                            TempData["Success"] = "ContactUser";
                            result = true;
                            if (userobj.FirstName != null && userobj.FirstName != "")
                            {
                                //---------------------------------------------------------------------
                                #region movement actions for this action method
                                UserInfo UserSessionValue = null;
                                if (Session["UserInfo"] == null)
                                {
                                    return RedirectToAction("Login", "Account");
                                }
                                UserSessionValue = (UserInfo)Session["UserInfo"];
                                movactiontype.MovementActionType = MovementnActionType.admin_create_new_contact;
                                movactiontype.ContactName = userobj.FirstName + ' ' + userobj.SurName;
                                string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                                loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription,0,0,0, UserSessionValue.UserSchema);
                                #endregion

                                #region Saving creating user in sys_event
                                movactiontype.SystemEventType = SysEventType.added_contact;
                                movactiontype.ContactName = sessionValues.FirstName + ' ' + sessionValues.LastName;
                                string sysEventDescp = System_Events.GetSysEventString(sessionValues, movactiontype, out ErrMsg);
                                loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, sessionValues.UserSchema);
                                #endregion
                            }
                        }
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            if (!regDet.OnlyContact)
            {
                TempData["UserName"] = regDet.UserName;
            }
            else
            {
                if (Contact_id != 0)
                {
                    TempData["UserName"] = regDet.FirstName;
                }
                else
                {
                    TempData["UserName"] = userobj.FirstName;
                }
            }
            TempData.Keep("UserName");
            TempData.Keep("Success");
            // return RedirectToAction("ManageUsers", "Account");
            return Json(new { result = result, successMsg = TempData["Success"], name = TempData["UserName"] });
        }

        public JsonResult CheckUserNameExists(string UserName, int type, string mode = "Create", string UserId = "")
        {
            decimal isUserNameExists = 0;
            isUserNameExists = userService.GetUserByName(UserName, type, mode, UserId);
            return Json(new { result = isUserNameExists });
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }


        [AllowAnonymous]
        public ActionResult Register(string criteria, string mode, string userID, string contactID)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.mode = mode;
            ViewBag.userID = userID;
            ViewBag.contactID = contactID;

            if (mode == null)
            {
                ViewBag.userID = 0;
                ViewBag.contactID = 0;
            }

            var sessionValues = (UserInfo)Session["UserInfo"];
            int userTypeId = Convert.ToInt32(sessionValues.UserTypeId);

            var selectQuestions = new SecurityQuestionInfo();
            selectQuestions.QuestionId = 0;
            selectQuestions.SecurityQuestion = "Select";

            var securityQuestionList = authenticationService.GetSecurityQuestion();
            securityQuestionList.Add(selectQuestions);
            SelectList securityQuestions = new SelectList(securityQuestionList, "QuestionId", "SecurityQuestion");
            ViewBag.SecurityQuestionList = securityQuestions;

            var UserTypeList = userService.GetUserType();
            var UserType = from s in UserTypeList
                           where s.UserType == userTypeId
                           select s;

            if (userTypeId == 696006)
            {
                SelectList UserTypeInfo = new SelectList(UserTypeList, "UserType", "Name");
                ViewBag.UserTypeListInfo = UserTypeInfo;
            }
            else
            {
                SelectList UserTypeInfo = new SelectList(UserType, "UserType", "Name", userTypeId);
                ViewBag.UserTypeListInfo = UserTypeInfo;
            }

            //Filling Country List Drop Down From database
            var CountryList = userService.GetCountryInfo();
            SelectList CountrySelectList = new SelectList(CountryList, "CountryID", "Country");

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Configuration.xml");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            XmlNode Association = xmlDocument.SelectSingleNode("/Configurable/association");
            XmlNodeList membership = Association.SelectNodes("membership");

            var memddlList = new List<UserRegistration>();
            ViewBag.CountryDropDown = CountrySelectList;
            ///* Select DropDown for Association From XML */
            foreach (XmlNode member in membership)
            {
                var AssoID = member.Attributes.GetNamedItem("value").Value;
                var AssoName = member.InnerText;
                UserRegistration AssoList = new UserRegistration();
                AssoList.AssociationId = Convert.ToInt32(AssoID);
                AssoList.Association = AssoName;
                memddlList.Add(AssoList);
            }
            SelectList AssoMemList = new SelectList(memddlList, "AssociationID", "Association");
            ViewBag.AssoDropDown = AssoMemList;
            return View();
        }

        public ActionResult ViewUserByID(string userTypeId, int UserId = 0, int ContactId = 0)
        {
            var infos = userService.GetUserByID(userTypeId, UserId, ContactId);
            return Json(new { result = infos });
        }

        [HttpPost]
        public JsonResult DeleteUser(int UserId, int deleteVal)
        {
            bool disablingAllowed = true;
            if (deleteVal == 0)
            {
                disablingAllowed = true;
            }
            else
            {
                disablingAllowed = userService.CheckToDisableUser(UserId);
            }
            if (disablingAllowed)
            {
                int result = userService.DeleteUser(UserId, deleteVal);
                if (result > -1)
                {
                    if (deleteVal == 1)
                    {
                        UserInfo UserSessionValue = null;
                        UserSessionValue = (UserInfo)Session["UserInfo"];
                        MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                        string ErrMsg = string.Empty;
                        int user_ID = Convert.ToInt32(UserSessionValue.UserId);
                        #region Saving creating user in sys_event
                        movactiontype.SystemEventType = SysEventType.deleted_users;
                        movactiontype.UserId = user_ID;
                        string sysEventDescp = System_Events.GetSysEventString(UserSessionValue, movactiontype, out ErrMsg);
                        loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, UserSessionValue.UserSchema);
                        #endregion
                    }
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false });
                }
            }
            else
            {
                return Json(new { Success = false });
            }
        }

        [HttpPost]
        public JsonResult DeleteContact(int ContactId, int deleteVal)
        {
            int result = userService.DeleteContact(ContactId, deleteVal);
            if (result > -1)
            {
                if (deleteVal == 1)
                {
                    #region movement actions for this action method
                    UserInfo UserSessionValue = null; //--------object is used for stroing movement actions
                    UserSessionValue = (UserInfo)Session["UserInfo"];
                    MovementActionIdentifiers movactiontype = new MovementActionIdentifiers();
                    movactiontype.MovementActionType = MovementnActionType.admin_deleted_contact_user;
                    movactiontype.ContactId = ContactId;
                    string ErrMsg = string.Empty;
                    string MovementDescription = MovementActions.GetMovementActionString(UserSessionValue, movactiontype, out ErrMsg);
                    loggingService.SaveMovementAction("", (int)movactiontype.MovementActionType, MovementDescription,0,0,0, UserSessionValue.UserSchema);
                    #endregion

                    int user_ID = Convert.ToInt32(UserSessionValue.UserId);
                    #region Saving deleting contact in sys_event
                    movactiontype.SystemEventType = SysEventType.deleted_contacts;
                    movactiontype.ContactId = ContactId;
                    string sysEventDescp = System_Events.GetSysEventString(UserSessionValue, movactiontype, out ErrMsg);
                    loggingService.SaveSysEventsMovement((int)movactiontype.SystemEventType, sysEventDescp, user_ID, UserSessionValue.UserSchema);
                    #endregion
                }
                return Json(new { Success = true });
            }
            else
            {
                return Json(new { Success = false });
            }

        }
        [HttpPost]
        public JsonResult SystemAutoGeneratePassword()
        {
            PasswordGenerator passwordGenerator = new PasswordGenerator();
            string autoPassword = passwordGenerator.Generate();
            GenericController gc = new GenericController();
            string strMd5password = gc.MD5Encryption(autoPassword);
            int iCount = authenticationService.GetPasswords(strMd5password);
            while (iCount > 0)
            {
                autoPassword = passwordGenerator.Generate();
                strMd5password = gc.MD5Encryption(autoPassword);
                iCount = authenticationService.GetPasswords(strMd5password);
            }
            return Json(new { result = autoPassword });
        }
    }
}