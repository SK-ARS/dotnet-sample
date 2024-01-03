using PagedList;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.MovementsAndNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using STP.Common.Enums;
using STP.Common.Constants;
using System.Text;
using STP.Domain.RouteAssessment;
using STP.ServiceAccess.RouteAssessment;
using STP.Common.General;
using STP.Common.StringExtractor;
using STP.Domain.Custom;
using STP.ServiceAccess.LoggingAndReporting;

namespace STP.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService contactService;
        private readonly IMovementsService movementsService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly INotificationDocService notificationDocService;
        private readonly ILoggingService loggingService;
        // GET: Contact

        public ContactController()
        {

        }
        public ContactController(IContactService contactService, IMovementsService movementsService, IRouteAssessmentService routeAssessmentService, INotificationDocService notificationDocService, ILoggingService logService)
        {
            this.contactService = contactService;
            this.movementsService = movementsService;
            this.routeAssessmentService = routeAssessmentService;
            this.notificationDocService = notificationDocService;
            this.loggingService = logService;
        }
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get list of authorise movement contact list
        /// </summary>
        /// <param name="analysisID">Analysis Id</param>
        /// <returns></returns>
        public ActionResult AuthoriseMovementShowContactList(int? page, int? pageSize, long analysisID, long NotificationID, string ESDALRefNumber, string Type,int? sortOrder = null, int? sortType = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Contact/AuthoriseMovementShowContactList actionResult method started successfully");

                ESDALRefNumber = !string.IsNullOrWhiteSpace(ESDALRefNumber)?ESDALRefNumber.Replace("~", "#"):ESDALRefNumber;
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                ContactModel contactInfo;

                #region Paging part
                int skipPage = 0;
                int pageNumber = (page ?? 1);

                if (pageSize == null)
                {
                    Session["PageSize"] = Session["PageSize"] == null ? 10 : Session["PageSize"];
                    pageSize = (int)Session["PageSize"];
                }
                else
                    Session["PageSize"] = pageSize;

                sortOrder = sortOrder != null ? sortOrder : 1; //type
                int presetFilter = sortType != null ? (int)sortType : 0; // asc     109
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;
                #endregion

                List<ContactModel> contacts = new List<ContactModel>();
                string userSchema = SessionInfo.UserSchema;
                var itemTypeStatus = (int)EnumExtensionMethods.GetValueFromDescription<ItemTypeStatus>(Type.ToLower());
                if (itemTypeStatus == (int)ItemTypeStatus.proposal || itemTypeStatus == (int)ItemTypeStatus.reproposal || itemTypeStatus == (int)ItemTypeStatus.nolonger_affected
                    || itemTypeStatus == (int)ItemTypeStatus.agreement || itemTypeStatus == (int)ItemTypeStatus.amendment_to_agreement)
                    userSchema = UserSchema.Sort;
                if (itemTypeStatus == (int)ItemTypeStatus.ne_notification_api || itemTypeStatus == (int)ItemTypeStatus.ne_re_notification_api 
                    || itemTypeStatus == (int)ItemTypeStatus.ne_notification || itemTypeStatus == (int)ItemTypeStatus.ne_renotification)
                {
                    contacts = movementsService.GetNENAffectedContactDetails(ESDALRefNumber, UserSchema.Portal);
                }
                else
                {
                    RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, 7, userSchema, sortOrder, presetFilter);
                    string currentAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));
                    var currentAffectedParty = StringExtraction.xmlAffectedPartyDeserializer(currentAffectedParties);
                    foreach (var item in currentAffectedParty.GeneratedAffectedParties.Select(item => item.Contact.Contact.simpleContactRef))
                    {
                        contactInfo = new ContactModel
                        {
                            ContactId = item.ContactId,
                            OrganisationId = item.OrganisationId,
                            Organisation = item.OrganisationName,
                            FullName = item.FullName
                        };
                        contacts.Add(contactInfo);
                    }
                    foreach (var manualAddedParty in currentAffectedParty.ManualAffectedParties.Select(item => item.Contact.Contact.adhocContactRef))
                    {
                        contactInfo = new ContactModel
                        {
                            ContactId = 0,
                            FullName = manualAddedParty.FullName,
                            Organisation = manualAddedParty.OrganisationName,
                        };
                        contacts.Add(contactInfo);
                    }
                }

                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;
                ViewBag.analysisID = analysisID;
                ViewBag.NotificationID = NotificationID;
                ViewBag.ESDALRefNumber = ESDALRefNumber;
                ViewBag.Type = Type;
                ViewBag.TotalCount = contacts.Count;
                skipPage = pageNumber - 1 == 0 ? 0 : (pageNumber - 1) * (int)pageSize;
                contacts = presetFilter == 0 ? contacts.OrderBy(x => x.FullName).ToList() : contacts.OrderByDescending(x => x.FullName).ToList();
                List<ContactModel> contactList = contacts.Skip(skipPage).Take((int)pageSize).ToList();
                var contactPagedList = new StaticPagedList<ContactModel>(contactList, pageNumber, (int)pageSize, ViewBag.TotalCount);

                return View(contactPagedList);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Contact/AuthoriseMovementShowContactList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult ContactList(int? page, int? pageSize, ContactSearchModel contactSearch = null, bool isNotifContact = false, bool isNotifContactSearch = false, bool Iscontactview = false, bool fromAnnotation = false, int? sortOrder = null, int? sortType = null,bool isClear=false)
        {
            try
            {

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("60001"))
                {
                    return RedirectToAction("Error", "Home");
                }

                if(isClear)
                    Session["ContactSearch"]=null;

                if((contactSearch==null || contactSearch.SearchColumn==null) && Session["ContactSearch"]!=null)
                    contactSearch = (ContactSearchModel)Session["ContactSearch"];

                int pageNumber = 0;
                int tempPageCount = 0;
                ViewBag.page = pageSize;
                List<ContactListModel> Contact;

                sortOrder = sortOrder != null ? (int)sortOrder : 1; //type
                int presetFilter = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;
                if (pageSize == null)
                {
                    if (page == null && contactSearch.SearchColumn == null)
                    {
                        //first time page is loaded page parameter will be null and search column will also be null
                        contactSearch.SearchColumn = "0";
                        contactSearch.SearchValue = "";
                        TempData["ContactSearchColumn"] = "0";
                        TempData["ContactSearchValue"] = "";
                    }
                    else if (page == null && contactSearch.SearchColumn != null)
                    {
                        //search button is clicked, page parameter is null and search column is supplied
                        //so save these values in the temp data                

                        TempData["ContactSearchColumn"] = contactSearch.SearchColumn;

                        if (String.IsNullOrEmpty(contactSearch.SearchValue))
                        {
                            TempData["ContactSearchValue"] = "";
                            contactSearch.SearchValue = "";
                        }
                        else
                            TempData["ContactSearchValue"] = contactSearch.SearchValue;
                    }
                    else if (page != null && contactSearch.SearchColumn == null)
                    {
                        //during page number click, the page parameter will not be null but the model will be null
                        //so put the tempdata values back into the  contactSearch

                        contactSearch.SearchColumn = Convert.ToString(TempData["ContactSearchColumn"]);
                        contactSearch.SearchValue = Convert.ToString(TempData["ContactSearchValue"]);
                    }

                }
                else
                {
                    if (!isNotifContactSearch) //Flag to check notification contact search its true when its coming from popupaddressbook.cshtml search else false always
                    {
                        if (contactSearch.SearchColumn != null || contactSearch.SearchValue != null)
                        {
                            TempData["ContactSearchColumn"] = contactSearch.SearchColumn;
                            TempData["ContactSearchValue"] = contactSearch.SearchValue;
                        }
                        if (Convert.ToString(TempData["ContactSearchValue"]) == "")
                        {
                            contactSearch.SearchColumn = "0";
                            contactSearch.SearchValue = "";
                        }
                        else
                        {
                            contactSearch.SearchColumn = Convert.ToString(TempData["ContactSearchColumn"]);
                            contactSearch.SearchValue = Convert.ToString(TempData["ContactSearchValue"]);
                        }
                    }
                    else
                    {
                        if (contactSearch.SearchColumn == null || contactSearch.SearchValue == null)
                        {
                            contactSearch.SearchColumn = "0";
                            contactSearch.SearchValue = "";
                            TempData["ContactSearchColumn"] = "0";
                            TempData["ContactSearchValue"] = "";
                        }

                        else if (contactSearch.SearchColumn == "0")
                        {
                            contactSearch.SearchValue = "";
                        }
                    }
                }


                pageNumber = (page ?? 1);
               
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
                //******#7642**********
                int sortFlag = 0;
                if (SessionInfo.UserSchema == UserSchema.Sort)
                {
                    sortFlag = 1;
                }
                //*********#7642********
                int organisationId = (int)SessionInfo.OrganisationId;
                contactSearch.SearchColumn = contactSearch.SearchColumn != null ? contactSearch.SearchColumn : "0";
                contactSearch.SearchValue = contactSearch.SearchValue != null ? contactSearch.SearchValue : "";
                Contact = contactService.GetContactListSearch(0, pageNumber, (int)pageSize, int.Parse(contactSearch.SearchColumn), contactSearch.SearchValue.TrimEnd(), sortFlag, presetFilter, sortOrder);


                if (Contact != null && Contact.Count != 0)
                {
                    tempPageCount = Convert.ToInt32(Contact[0].RecordCount);
                }
                ViewBag.searchColumn = contactSearch.SearchColumn;
                ViewBag.searchValue = contactSearch.SearchValue;
                ViewBag.TotalCount = tempPageCount;
            
                TempData.Keep("ContactSearchColumn");
                TempData.Keep("ContactSearchValue");

                var contactPageList = new StaticPagedList<ContactListModel>(Contact, pageNumber, (int)pageSize, ViewBag.TotalCount);

                ViewBag.isNotifContact = isNotifContact;

                var fromContactView = 0;
                if (Session["Iscontact"] != null)
                    fromContactView = (int)Session["Iscontact"];
              
                if(fromContactView == 1)
                {
                    Iscontactview = true;
                }
                else
                    Iscontactview = false;
                if (!Iscontactview)
                {
                    TempData["Iscontact"] = "0";
                }
                else
                {
                
                 TempData["Iscontact"] = "1";
                }
                TempData.Keep("Iscontact");
                ViewBag.FromAnnotation = fromAnnotation;
                if(contactSearch!=null && contactSearch.SearchColumn!=null)
                    Session["ContactSearch"] = contactSearch;
                return View("ContactList", contactPageList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Contact/ContactList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// get detail of single contact for displaying in affected contact list
        /// </summary>       
        public ActionResult AffectedContactDetail(int ContactID, int NotifID = 0, int analysisId = 0, bool haulierContact = false, bool NotifyingContact = false, bool fromSort = false, int haulOrgID = 0, int revisionId = 0)
        {
            try
            {
                string contactStatus = null;
                string HaulFullName = "", HaulOrgName = "";
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("60001"))
                {
                    return RedirectToAction("Error", "Home");
                }

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] != null)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                List<AssessmentContacts> contactList = new List<AssessmentContacts>();
                if (!haulierContact)
                {
                    #region
                    //if add yourself as an affected party is selected 
                    if (ContactID == 0 && NotifyingContact == true)
                    {
                        ContactID = (int)SessionInfo.ContactId; // inserting default haulier as a contact from haulier side affected parties page
                        contactList = routeAssessmentService.SaveAndFetchContacts(ContactID, NotifID, SessionInfo.UserSchema);
                    }
                    else if (fromSort == false && ContactID != 0) //conditions where 
                    {
                        contactList = routeAssessmentService.SaveAndFetchContacts(ContactID, NotifID, SessionInfo.UserSchema);
                    }
                    else if (fromSort == true && ContactID != 0) //conditions where 
                    {
                        contactList = routeAssessmentService.SaveAndFetchContacts(ContactID, NotifID, SessionInfo.UserSchema);
                    }
                    else
                    {
                        //do nothing
                    }
                    #endregion

                    if (ContactID == 0 && NotifyingContact == false && fromSort == true)
                    {
                        contactList = routeAssessmentService.FetchContactDetails(haulOrgID, revisionId, SessionInfo.UserSchema);

                        if (contactList != null && contactList.Count != 0)
                        {
                            //Saving haulier users Name in viewbag for storing as a hidden variable
                            HaulFullName = contactList[0].ContactName;
                            HaulOrgName = contactList[0].OrganisationName;
                        }

                    }

                }
                else
                {
                    HaulierContactModel haulierContactDetail = new HaulierContactModel();
                    haulierContactDetail = contactService.GetHaulierContactById(ContactID);

                    AssessmentContacts assessmentContact = new AssessmentContacts();

                    assessmentContact.ContactId = ContactID;
                    assessmentContact.ContactName = haulierContactDetail.Name;
                    assessmentContact.OrganisationName = haulierContactDetail.OrganisationName;
                    assessmentContact.Fax = haulierContactDetail.Fax;
                    assessmentContact.Email = haulierContactDetail.Email;
                    assessmentContact.OrganisationType = "haulier";

                    contactList.Add(assessmentContact);

                }

                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

                string xmlAffectedParties = "";
                string newXmlAffectedParties = "";
                if (objRouteAssessmentModel.AffectedParties != null)
                {
                    xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties));
                }
                newXmlAffectedParties = StringExtraction.XmlAffectedPartyDeserializer(xmlAffectedParties, contactList);

                if (newXmlAffectedParties != "Contact already exist")
                {
                    objRouteAssessmentModel.AffectedParties = StringExtractor.ZipAndBlob(newXmlAffectedParties);

                    try
                    {
                        #region manual_party_added code block to add the information to movements
                        //inserting action for manually added contact on movement action
                        MovementActionIdentifiers movactiontype = null;
                        foreach (AssessmentContacts newContact in contactList)
                        {
                            movactiontype = new MovementActionIdentifiers();
                            movactiontype.MovementActionType = MovementnActionType.manual_party_added;
                            movactiontype.SenderContactName = SessionInfo.FirstName + ' ' + SessionInfo.LastName;
                            movactiontype.OrganisationNameSender = SessionInfo.OrganisationName;
                            movactiontype.ManuallyAddedOrgName = newContact.OrganisationName;
                            movactiontype.ManuallyAddedContName = newContact.ContactName;
                            //notificationDocService.GenerateMovementAction(SessionInfo, "", movactiontype);
                            string MovementDescription = MovementActions.GetMovementActionString(SessionInfo, movactiontype, out string ErrMsg);
                            loggingService.SaveMovementAction(movactiontype.ESDALRef, (int)movactiontype.MovementActionType, MovementDescription, 0, 0, 0, SessionInfo.UserSchema);

                        }
                        #endregion
                    }
                    catch
                    {
                        //do nothing
                    }
                    long status = routeAssessmentService.UpdateAnalysedRoute(objRouteAssessmentModel, analysisId, SessionInfo.UserSchema);
                }
                else if (newXmlAffectedParties == "")
                {
                    contactStatus = newXmlAffectedParties;
                }
                else
                {
                    contactStatus = newXmlAffectedParties;
                }
                int sortAddedContact = 0;
                if (fromSort == true)
                {
                    sortAddedContact = 1;
                }

                var jsonObj = new { NotifID = NotifID, analysisId = analysisId, status = contactStatus, sortAddedContact = sortAddedContact, HaulFullName = HaulFullName, HaulOrgName = HaulOrgName };

                return Json(new { result = jsonObj });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Contact/AffectedContactDetail, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// get detail of signle contact
        /// </summary>       
        public ActionResult ContactDetail(int ContactID, string origin = " ")
        {
            try
            {

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("60001"))
                {
                    return RedirectToAction("Error", "Home");
                }

                ContactModel contact = notificationDocService.GetContactDetails(ContactID);
                if (origin == "routesave" || origin == "outlinesave" || origin == "annotation")
                    return Json(new { result = contact });
                else
                    return View("ContactDetail", contact);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Contact/ContactDetail, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult ContactedPartiesList(int? page, int? pageSize, long analysisID)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Contact/ContactedPartiesList actionResult method started successfully"));

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                MovementContactModel contactInfo = movementsService.GetContactedPartiesDetail(analysisID);

                string xmlAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(contactInfo.AffectedParties));
                var affectedParty = StringExtraction.xmlAffectedPartyDeserializer(xmlAffectedParties);

                List<MovementContactModel> contactList = new List<MovementContactModel>();

                foreach (var genratedAffectedParty in affectedParty.GeneratedAffectedParties)
                {
                    contactInfo = new MovementContactModel
                    {
                        ContactId = genratedAffectedParty.Contact.Contact.simpleContactRef.ContactId,
                        FirstName = genratedAffectedParty.Contact.Contact.simpleContactRef.FullName,
                        Organisation = genratedAffectedParty.Contact.Contact.simpleContactRef.OrganisationName,
                        OrganisationId = genratedAffectedParty.Contact.Contact.simpleContactRef.OrganisationId,
                        ContactType = genratedAffectedParty.IsPolice ? "Police alo" : "SOA",
                        Email = string.Empty,
                        DelegatorsOrganisationName = genratedAffectedParty.OnBehalfOf != null ? genratedAffectedParty.OnBehalfOf.DelegatorsOrganisationName : string.Empty,
                        DelegatorsOrganisationId = genratedAffectedParty.OnBehalfOf != null ? genratedAffectedParty.OnBehalfOf.DelegatorsOrganisationId : 0,
                    };
                    contactList.Add(contactInfo);
                }
                foreach (var manualAddedParty in affectedParty.ManualAffectedParties.Select(item => item.Contact.Contact.adhocContactRef))
                {
                    contactInfo = new MovementContactModel
                    {
                        ContactId = 0,
                        OrganisationId = 0,
                        FirstName = manualAddedParty.FullName,
                        Organisation = manualAddedParty.OrganisationName,
                        Email = manualAddedParty.EmailAddress,
                        ContactType = "Interested party"
                    };
                    contactList.Add(contactInfo);
                }

                int pageNumber = (page ?? 1);

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
                ViewBag.page = pageNumber;

                ViewBag.analysisID = analysisID;

                int totalCount = contactList.Count();

                ViewBag.TotalCount = totalCount;

                var sortedList = contactList.OrderBy(m => m.ContactType).ThenBy(m => m.FirstName).ToList();

                int skipPage = 0;

                if ((pageNumber - 1) == 0)
                {
                    skipPage = 0;
                }
                else
                {
                    skipPage = (pageNumber - 1) * (int)pageSize;
                }

                sortedList = sortedList.Skip(skipPage).Take((int)pageSize).ToList();

                var contactPagedList = new StaticPagedList<MovementContactModel>(sortedList, pageNumber, (int)pageSize, totalCount);

                return PartialView("~/Views/Application/PartialView/ContactedPartiesList.cshtml", contactPagedList);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Contact/ContactedPartiesList, Exception: {0}", ex));
                throw ex;
            }
        }

        //[SessionValidate]
        public ActionResult ContactView()
        {
            try
            {
                if (!PageAccess.GetPageAccess("60001"))
                {
                    return RedirectToAction("Error", "Home");
                }
              
                TempData["Iscontact"] = "1";
                Session["Iscontact"] = 1;
                var contactSearch=new ContactSearchModel();
                if(Session["ContactSearch"]!=null)
                    contactSearch = (ContactSearchModel)Session["ContactSearch"];
                return View(contactSearch);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Contact/ContactView, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

      
    }
}