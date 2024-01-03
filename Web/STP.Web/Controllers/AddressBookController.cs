using PagedList;
using STP.Common.General;
using STP.Common.Logger;
using STP.Common.StringExtractor;
using STP.Domain.RouteAssessment;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.RouteAssessment;
using STP.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    public class AddressBookController : Controller
    {
        private readonly IContactService contactService;
        private readonly IRouteAssessmentService routeAssessmentService;
        public AddressBookController(IContactService contactService, IRouteAssessmentService routeAssessmentService)
        {
            this.contactService = contactService;
            this.routeAssessmentService = routeAssessmentService;
        }
        [HttpGet]
        [AuthorizeUser(Roles = "60002,60001,60000")]
        public ActionResult Index()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //if (!PageAccess.GetPageAccess("60002"))
            //{
            //    return RedirectToAction("Error", "Home");
            //}
            return View();
        }

        /// <summary>
        /// Create new record or edit old record for Addressbook
        /// </summary>
        [HttpGet]
        [ValidateInput(true)]
        [AuthorizeUser(Roles = "60002,60001,60000")]
        public ActionResult ManageAddressBook(string mode, int HaulierContactId, bool nonEsdalContact = false)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                HaulierContactModel haulierModel = new HaulierContactModel();
                ViewBag.mode = mode;
                ViewBag.NonEsdalContact = nonEsdalContact;
                if (ModelState.IsValid)
                {
                    if (mode != null && mode == "Edit")
                    {
                        haulierModel = contactService.GetHaulierContactById(HaulierContactId);
                    }
                    else
                    {
                        UserInfo SessionInfo = null;
                        SessionInfo = (UserInfo)Session["UserInfo"];
                        haulierModel.OrganisationId = (int)SessionInfo.OrganisationId;
                    }
                }
                return View("ManageAddressBook", haulierModel);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("AddressBook/ManageAddressBook, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [ValidateInput(true)]
        public ActionResult ManageAddressBookDetails(string mode, int HaulierContactId, bool nonEsdalContact = false)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                HaulierContactModel haulierModel = new HaulierContactModel();
                ViewBag.mode = mode;
                ViewBag.NonEsdalContact = nonEsdalContact;
                if (ModelState.IsValid)
                {
                    if (mode != null && mode == "Edit")
                    {
                        haulierModel = contactService.GetHaulierContactById(HaulierContactId);
                    }
                    else
                    {
                        UserInfo SessionInfo = null;
                        SessionInfo = (UserInfo)Session["UserInfo"];
                        haulierModel.OrganisationId = (int)SessionInfo.OrganisationId;
                    }
                }
                return PartialView("PartialView/_ManageAddressBookDetails", haulierModel);
                //return View("ManageAddressBook", haulierModel);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("AddressBook/ManageAddressBook, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeUser(Roles = "60002,60001,60000")]
        public JsonResult UpdateAddress(HaulierContactModel haulierContactModel, bool addToHaul = false, bool isNonEsdalContact = false, int analysisId = 0)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("Login", "Account")
                    });

                }
                var SessionInfo = (UserInfo)Session["UserInfo"];

                if (ModelState.IsValid)
                {
                    if (haulierContactModel.CommunicationMethodName.ToLower() == "fax")
                    {
                        haulierContactModel.Email = string.Empty;
                    }
                    else if (haulierContactModel.CommunicationMethodName.ToLower() == "email html")
                    {
                        haulierContactModel.Fax = string.Empty;
                    }
                    if (isNonEsdalContact)
                    {
                        //code to save to xml

                        List<AssessmentContacts> contactList = new List<AssessmentContacts>();
                        AssessmentContacts assessmentContact = new AssessmentContacts()
                        {
                            ContactName = haulierContactModel.Name,
                            OrganisationName = haulierContactModel.OrganisationName,
                            Fax = haulierContactModel.Fax,
                            Email = haulierContactModel.Email,
                            OrganisationType = "haulier"
                        };
                        contactList.Add(assessmentContact);

                        RouteAssessmentModel objRouteAssessmentModel;

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);
                        if (objRouteAssessmentModel.AffectedParties != null)
                        {
                            string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));

                            string newXmlAffectedParties = routeAssessmentService.XmlAffectedPartyDeserializer(xmlAffectedParties, contactList);

                            if (newXmlAffectedParties != "Contact already exist")
                            {
                                objRouteAssessmentModel.AffectedParties = StringExtractor.ZipAndBlob(newXmlAffectedParties);
                                routeAssessmentService.updateRouteAssessment(objRouteAssessmentModel, analysisId, SessionInfo.UserSchema);
                            }
                            else
                            {
                                return Json(new { Success = true, ContactExists = true });
                            }
                        }

                        if (!addToHaul)
                        {
                            return Json(new { Success = true });
                        }
                    }
                    contactService.ManageHaulierContact(haulierContactModel);
                    return Json(new { Success = true });
                }
                else
                {

                    var validationResults = haulierContactModel.Validate(new ValidationContext(haulierContactModel, null, null));

                    string errorMessage = string.Empty;

                    foreach (var error in validationResults)
                    {
                        foreach (var memberName in error.MemberNames)
                        {
                            errorMessage = error.ErrorMessage;
                        }
                    }

                    return Json(new { ErrorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Addressbook/UpdateAddress, Exception: {0}", ex));
                throw;
            }
        }
        /// <summary>
        /// AddressList ActionResultMethod is used to return the Addresslist view
        /// </summary>
        /// <returns>View</returns>
        public ActionResult AddressList(int? page, int? pageSize, HaulierContactModelSearch HauliercontactSearch = null, bool isNotifContact = false, bool isNotifContactSearch = false, string searchId = null, string searchText = null, int? sortOrder = null, int? sortType = null,bool isClear=false,bool isMainPage=false, bool isPlanMovement = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Addressbook/AddressList actionResult method started successfully");
                if(isClear){
                    Session["searchId"]=null;
                    Session["searchText"]=null;
                }

                if((searchId==null) && Session["searchId"]!=null)
                    searchId = (string)Session["searchId"];
                if((searchText==null) && Session["searchText"]!=null)
                    searchText = (string)Session["searchText"];

                UserInfo SessionInfo = null;
                int tempPageCount = 0;

                sortOrder = sortOrder != null ? (int)sortOrder : 1; //contact name
                int presetFilter = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("60002"))
                {
                    return RedirectToAction("Error", "Home");
                }
                if (searchId != null)
                {
                    HauliercontactSearch.SearchColumn = searchId;
                    HauliercontactSearch.SearchValue = searchText;
                }
                SessionInfo = (UserInfo)Session["UserInfo"];

                int organisationId = (int)SessionInfo.OrganisationId;

                if (pageSize == null)
                {
                    if (page == null && HauliercontactSearch.SearchColumn == null)
                    {
                        //first time page is loaded page parameter will be null and search column will also be null
                        HauliercontactSearch.SearchColumn = null;
                        HauliercontactSearch.SearchValue = "";
                        TempData["HaulierSearchColumn"] = null;
                        TempData["HaulierSearchValue"] = "";
                    }
                    else if (page == null && HauliercontactSearch.SearchColumn != null)
                    {
                        //search button is clicked, page parameter is null and search column is supplied
                        //so save these values in the temp data                

                        TempData["HaulierSearchColumn"] = HauliercontactSearch.SearchColumn;

                        if (String.IsNullOrEmpty(HauliercontactSearch.SearchValue))
                        {
                            TempData["HaulierSearchValue"] = "";
                            HauliercontactSearch.SearchValue = "";
                        }
                        else
                            TempData["HaulierSearchValue"] = HauliercontactSearch.SearchValue;
                    }
                    else if (page != null && HauliercontactSearch.SearchColumn != null)
                    {
                        HauliercontactSearch.SearchColumn = searchId;
                        HauliercontactSearch.SearchValue = searchText;
                        TempData["HaulierSearchValue"] = searchText;
                        TempData["HaulierSearchColumn"] = searchId;

                    }
                    else if (page != null && HauliercontactSearch.SearchColumn == null)
                    {
                        //during page number click, the page parameter will not be null but the model will be null
                        //so put the tempdata values back into the  contactSearch

                        if (TempData["HaulierSearchColumn"] == null)
                        {
                            HauliercontactSearch.SearchColumn = null;
                        }
                        else
                        {
                            HauliercontactSearch.SearchColumn = Convert.ToString(TempData["HaulierSearchColumn"]);
                        }
                        HauliercontactSearch.SearchValue = Convert.ToString(TempData["HaulierSearchValue"]);

                    }
                   
                }
                else
                {
                    if (!isMainPage)
                    {
                        if (!isNotifContactSearch) //Flag to check notification contact search its true when its coming from popupaddressbook.cshtml search else false always
                        {
                            if (Convert.ToString(TempData["HaulierSearchValue"]) == "")
                            {
                                HauliercontactSearch.SearchColumn = null;
                                HauliercontactSearch.SearchValue = "";
                            }
                            else
                            {
                                HauliercontactSearch.SearchColumn = Convert.ToString(TempData["HaulierSearchColumn"]);
                                HauliercontactSearch.SearchValue = Convert.ToString(TempData["HaulierSearchValue"]);
                            }
                        }
                        else
                        {
                            if (HauliercontactSearch.SearchColumn == null || HauliercontactSearch.SearchValue == null)
                            {
                                HauliercontactSearch.SearchColumn = null;
                                HauliercontactSearch.SearchValue = "";
                                TempData["HaulierSearchColumn"] = null;
                                TempData["HaulierSearchValue"] = "";
                            }

                            else if (HauliercontactSearch.SearchColumn == null)
                            {
                                HauliercontactSearch.SearchValue = "";
                            }
                        }
                    }
                }


                //BL method goes here                
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

                ViewBag.UserTypeId = SessionInfo.UserTypeId;

                List<HaulierContactModel> addresslist = contactService.GetHaulierContactList(organisationId, pageNumber, (int)pageSize, HauliercontactSearch.SearchColumn, HauliercontactSearch.SearchValue, presetFilter, sortOrder);

                if (addresslist != null && addresslist.Count != 0)
                {
                    tempPageCount = Convert.ToInt32(addresslist[0].RecordCount);
                }

                ViewBag.TotalCount = tempPageCount;
                TempData.Keep("HaulierSearchColumn");
                TempData.Keep("HaulierSearchValue");

                ViewBag.isNotifContact = isNotifContact;

                var AddressPagedList = new StaticPagedList<HaulierContactModel>(addresslist, pageNumber, (int)pageSize, ViewBag.TotalCount);
                ViewBag.SearchTemp = TempData["HaulierSearchValue"];
                ViewBag.searchColumn = searchId;
                ViewBag.searchValue = searchText;
                if(searchId!=null && searchId!=string.Empty)
                {
                    Session["searchId"]=searchId;
                    Session["searchText"]=searchText;
                }
                ViewBag.IsPlanMovement = isPlanMovement;
                return View("AddressList", AddressPagedList);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Addressbook/AddressList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Get detail of Address
        /// </summary>
        /// <param name="HaulierContactId"></param>
        /// <returns></returns>
        [AuthorizeUser(Roles = "60002,60001,60000")]
        public ActionResult AddressDetail(int HaulierContactId)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                //if (!PageAccess.GetPageAccess("60002"))
                //{
                //    return RedirectToAction("Error", "Home");
                //}
                HaulierContactModel haulierContactDetail = contactService.GetHaulierContactById(HaulierContactId);
                return PartialView("AddressDetail", haulierContactDetail);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Addressbook/AddressDetail, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Delete selected record from Address List
        /// </summary>
        /// <returns>View</returns>
        [HttpPost]
        [AuthorizeUser(Roles = "60002,60001,60000")]
        public ActionResult DeleteAddress(int deletedContactId)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                //if (!PageAccess.GetPageAccess("60002"))
                //{
                //    return RedirectToAction("Error", "Home");
                //}
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("AddressBook/DeleteAddress JsonResult method started successfully, with parameters deletedContactId:{0}", deletedContactId));
                contactService.DeleteHaulierContact(deletedContactId);

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "AddressBook/DeleteAddress JsonResult method completed successfully");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Addressbook/DeleteAddress, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

    }
}