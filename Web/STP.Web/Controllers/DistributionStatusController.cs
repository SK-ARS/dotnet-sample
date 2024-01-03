using STP.Common.Constants;
using System.Web.Mvc;
using System.Web.SessionState;
using STP.Web.Filters;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using STP.Domain.HelpdeskTools;
using System.Collections.Generic;
using PagedList;
using System;
using STP.ServiceAccess.HelpdeskTools;
using STP.ServiceAccess.DocumentsAndContents;
using STP.Web.WorkflowProvider;
using System.Text.RegularExpressions;
using STP.Domain.DocumentsAndContents;
using STP.ServiceAccess.CommunicationsInterface;
using STP.Domain.Communications;
using STP.Common.Enums;

namespace STP.Web.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class DistributionStatusController : Controller
    {
        private readonly IDistributionStatusService DistributionStatusService;
        private readonly IDocumentService documentService;
        private readonly ICommunicationsInterfaceService communicationService;

        public DistributionStatusController()
        {

        }
        public DistributionStatusController(IDistributionStatusService DistributionStatusService, IDocumentService documentService,ICommunicationsInterfaceService communicationService)
        {
            this.DistributionStatusService = DistributionStatusService;
            this.documentService = documentService;
            this.communicationService = communicationService;
        }

        #region public ActionResult DistributionStatusDashboard(int? page, int? pageSize, string filterDate)
        [AuthorizeUser(Roles = "14000,14001")]
        public ActionResult DistributionStatusDashboard(bool nonesdal = false, bool ISDeleted = false, string ErrorMessage = null)
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"DistributionStatusController/ViewDistributionStatus actionResult method started successfully");

            DistributionAlerts objDistributionAlert = new DistributionAlerts();
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            int portalType = SessionInfo.UserTypeId;

            if (portalType != 696006 && portalType != 696008)
                return RedirectToAction("Login", "Account");

            int pageNumber = 1;
            int pageSize = 5;
            objDistributionAlert.ShowAlert = "on";
            objDistributionAlert.SearchFlag = 1;
            List<DistributionAlerts> objListDistAlerts = DistributionStatusService.GetDistributionAlert(pageNumber, pageSize, objDistributionAlert, portalType);

            ViewBag.NonESDALUSER = nonesdal;
            ViewBag.Isdeleted = ISDeleted;
            ViewBag.ErrorMessage = ErrorMessage;
            return PartialView(objListDistAlerts);
        }
        #endregion public ActionResult ViewDistributionAlert(int? page, int? pageSize, string filterDate)

        #region public ActionResult ViewDistributionAlert(int? page, int? pageSize, string filterDate)
        [AuthorizeUser(Roles = "14000,14001")]
        public ActionResult ViewDistributionStatus(int? page, int? pageSize, bool nonesdal = false, bool ISDeleted = false, string ErrorMessage = null,int? sortType=null,int? sortOrder=null)
        {
            try
            {
                sortOrder = sortOrder != null ? (int)sortOrder : 1; //date
                int presetFilter = sortType != null ? (int)sortType : 1; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "DistributionStatusController/ViewDistributionStatus actionResult method started successfully");
                ViewBag.movementData = 0;
                if (Session["g_startDate"] != null)
                {
                    ViewBag.Filter_start_Date = (string)Session["g_startDate"];
                }
                if (Session["g_endDate"] != null)
                {
                    ViewBag.Filter_end_Date = (string)Session["g_endDate"];
                }
                if (Session["g_esdalData"] != null)
                {
                    ViewBag.esdalData = (string)Session["g_esdalData"];
                }
                if (Session["g_movementData"] != null)
                {
                    ViewBag.movementData = (string)Session["g_movementData"];
                }
                if (Session["g_showalert"] != null)
                {
                    ViewBag.showalert = (string)Session["g_showalert"];
                }
                if (Session["g_Org_To"] != null)
                {
                    ViewBag.OrgTo = (string)Session["g_Org_To"];
                }
                if (Session["date_filter"] != null)
                {
                    ViewBag.DateFilter = (string)Session["date_filter"];
                }
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                DistributionAlerts objDistributionAlert = new DistributionAlerts();
               
                int portalType = SessionInfo.UserTypeId;

                if (portalType != 696006 && portalType != 696008)
                    return RedirectToAction("Login", "Account");

                //viewbag for pagination
                int pageNumber = page ?? 1;
                if (Session["PageSize"] == null)
                    Session["PageSize"] = 10;

                if (pageSize == null)
                    pageSize = (int)Session["PageSize"];
                else
                    Session["PageSize"] = pageSize;

                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;
                int Flag = 0;
                if (Session["g_startDate"] != null)
                {
                    objDistributionAlert.StartDate = Convert.ToDateTime(Session["g_startDate"]);
                    Flag = 1;
                }
                if (Session["g_endDate"] != null)
                {
                    objDistributionAlert.EndDate = Convert.ToDateTime(Session["g_endDate"]);
                    Flag = 1;
                }
                if (Session["g_esdalData"] != null)
                {
                    objDistributionAlert.ESDALReference = (string)Session["g_esdalData"];
                    Flag = 1;
                }
                if (Session["g_movementData"] != null)
                {
                    objDistributionAlert.MovementData = (string)Session["g_movementData"];
                    Flag = 1;
                }
                if (Session["g_showalert"] != null)
                {
                    objDistributionAlert.ShowAlert = (string)Session["g_showalert"];
                    Flag = 1;
                }
                if (Session["g_Org_To"] != null)
                {
                    objDistributionAlert.ToOrganisationName = (string)Session["g_Org_To"];
                    Flag = 1;
                }

                Session["g_SearchData"] = objDistributionAlert;
                ViewBag.showalert = objDistributionAlert.ShowAlert;
                objDistributionAlert.SearchFlag = Flag;
                List<DistributionAlerts> objListDistAlerts = DistributionStatusService.GetDistributionAlert(pageNumber, (int)pageSize, objDistributionAlert, portalType, presetFilter, sortOrder);

                int totalCount = 0;
                if (objListDistAlerts != null && objListDistAlerts.Count != 0)
                    totalCount = Convert.ToInt32(objListDistAlerts[0].TotalCount);
                ViewBag.TotalCount = totalCount;
                var movementObjPagedList = new StaticPagedList<DistributionAlerts>(objListDistAlerts, pageNumber, (int)pageSize, totalCount);
                ViewBag.DistributionAlert = objDistributionAlert;
                ViewBag.NonESDALUSER = nonesdal;
                ViewBag.Isdeleted = ISDeleted;
                ViewBag.ErrorMessage = ErrorMessage;
                return View(movementObjPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"DistributionStatusController/ViewDistributionStatus Exception :{ex}");
                throw;
            }
        }
        #endregion public ActionResult ViewDistributionAlert(int? page, int? pageSize, string filterDate) 

        #region public ActionResult ViewDistribFilter()
        public ActionResult ViewDistribFilter()
        {
            if (Session["g_startDate"] != null)
            {
                ViewBag.Filter_start_Date = (string)Session["g_startDate"];
            }
            if (Session["g_endDate"] != null)
            {
                ViewBag.Filter_end_Date = (string)Session["g_endDate"];
            }
            if (Session["g_esdalData"] != null)
            {
                ViewBag.esdalData = (string)Session["g_esdalData"];
            }
            if (Session["g_movementData"] != null)
            {
                ViewBag.movementData = (string)Session["g_movementData"];
            }
            if (Session["g_showalert"] != null)
            {
                ViewBag.showalert = (string)Session["g_showalert"];
            }
            if (Session["g_Org_To"] != null)
            {
                ViewBag.OrgTo = (string)Session["g_Org_To"];
            }
            return PartialView();
        }
        #endregion

        #region public ActionResult ViewDistribFilter()
        public ActionResult SetDistribFilter(int pageNum, int pageSizeVal,string esdalData = null, string txt_start_time = null, string txt_end_time = null, string movementData = "0", string showalert = "off", string Org_From = null, string Org_To = null, int? sortOrder = null, int? sortType = null)
        {
            Session["g_startDate"] = txt_start_time;
            Session["g_endDate"] = txt_end_time;
            Session["g_esdalData"] = esdalData;
            Session["g_movementData"] = movementData;
            Session["g_showalert"] = showalert;
            Session["g_Org_To"] = Org_To;
            return RedirectToAction("ViewDistributionStatus", "DistributionStatus", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        Helpers.EncryptionUtility.Encrypt("page=" + pageNum +
                        "&pageSize=" + pageSizeVal +
                        "&sortOrder=" + sortOrder +
                        "&sortType=" + sortType)
            });
        }
        #endregion

        #region ClearHelpdeskFilter()
        public JsonResult ClearHelpdeskFilter()
        {
            Session["g_SearchData"] = null;
            Session["g_startDate"] = null;
            Session["g_endDate"] = null;
            Session["g_esdalData"] = null;
            Session["g_movementData"] = null;
            Session["g_showalert"] = null;
            Session["g_Org_To"] = null;
            Session["date_filter"] = null;
            return Json(new { result = 1 });
        }
        #endregion

        #region public ActionResult Retransmit(int transmissionId)
        [HttpGet]
        public ActionResult Retransmit(int transmissionId, bool distflag = false)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            RetransmitDetails retransmitDetails = documentService.GetRetransmitDetails(transmissionId, SessionInfo.UserSchema);
            ViewBag.TransmissionId = transmissionId;
            ViewBag.distflag = distflag;
            return PartialView(retransmitDetails);
        }
        #endregion public ActionResult Retransmit(int transmissionId)

        #region public ActionResult Retransmit(int transmissionId)
        [HttpPost]
        public ActionResult Retransmit(int transmissionId, string emailId)
        {
            int status = 0;
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
            RetransmitDetails retransmitDetails = documentService.GetRetransmitDetails(transmissionId, SessionInfo.UserSchema);
            retransmitDetails.Email = emailId;
            if (retransmitDetails.efax == "email" || !string.IsNullOrEmpty(retransmitDetails.Email))
            {
                Regex Rgxemail = new Regex(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$");
                if (retransmitDetails.Email == null)
                    return Json(904);
                else if (!Rgxemail.IsMatch(retransmitDetails.Email))
                    return Json(905);
            }
            if (ModelState.IsValid)
            {
                TransmittingDocumentDetails transmittingDetail = documentService.SortSideCheckDoctype(transmissionId, SessionInfo.UserSchema);
                if (SessionInfo.UserSchema == UserSchema.Portal)
                {
                    transmittingDetail.TransmissionId = transmissionId;
                    transmissionId = (int)documentService.GetNewInsertedTransForDist(transmittingDetail);
                }

                NotificationContacts objContact = new NotificationContacts();

                if (transmittingDetail.ContactId != 0)
                {
                    //function that returns contact's details in a string array
                    string[] contactDet = documentService.FetchContactPreference((int)transmittingDetail.ContactId, UserSchema.Portal);
                    objContact.OrganisationId = Convert.ToInt32(contactDet[4]);
                    objContact.ContactId = (int)transmittingDetail.ContactId;
                }
                objContact.Email = retransmitDetails.Email;
                GetRetransmitDocumentParams getRetransmit = new GetRetransmitDocumentParams
                {
                    TransmittingDetail = transmittingDetail,
                    RetransmitDetails = retransmitDetails,
                    TransmissionId = transmissionId,
                    UserInfo = SessionInfo,
                    UserSchema = SessionInfo.UserSchema
                };
                RetransmitEmailgetParams retransmit = documentService.GetRetransmitDocument(getRetransmit);

                bool transmissionStatus = false;

                if (retransmitDetails.efax == "email" || !string.IsNullOrEmpty(retransmitDetails.Email))
                {
                    CommunicationParams communicationParams = new CommunicationParams()
                    {
                        UserEmail = objContact.Email,
                        Subject = "",
                        Content = retransmit.Content,
                        Attachment = retransmit.AttachmentData,
                        ESDALReference = transmittingDetail.EsdalReference,
                        XMLAttach = retransmit.XmlAttached,
                        DocumentTypeName = transmittingDetail.DocumentType,
                        IsImminent = retransmit.IsImminent,
                        DocumentType = 0
                    };
                    transmissionStatus = communicationService.SendGeneralmail(communicationParams);
                }
                else if (retransmitDetails.efax == "fax")
                    transmissionStatus = communicationService.SendFax(objContact, SessionInfo, transmissionId, retransmit.Content);

                SaveDistributionStatusParams saveDistributionStatus;
                if (transmissionStatus)
                {
                    saveDistributionStatus = new SaveDistributionStatusParams
                    {
                        NotificationContacts = objContact,
                        Status = (int)DistibuteStat.with_mail_retransmit_delivered,
                        InboxOnly = 0,
                        EsdalReference = transmittingDetail.EsdalReference,
                        TransmissionId = transmissionId,
                        IsImminent = false,
                        Username = SessionInfo.UserName
                    };
                    status = 1;
                }
                else
                {
                    saveDistributionStatus = new SaveDistributionStatusParams
                    {
                        NotificationContacts = objContact,
                        Status = (int)DistibuteStat.with_mail_retransmit_failed,
                        InboxOnly = 0,
                        EsdalReference = transmittingDetail.EsdalReference,
                        TransmissionId = transmissionId,
                        IsImminent = false,
                        Username = SessionInfo.UserName
                    };
                }
                documentService.SaveDistributionStatus(saveDistributionStatus);
                ViewBag.TransmissionId = transmissionId;
                return Json(status);
            }
            else
            {
                if (retransmitDetails.efax == "fax")
                {
                    Regex Rgxfax = new Regex(@"^(\+)?(?<!\d)\d{12}(?!\d)$");
                    if (retransmitDetails.Fax == null)
                        return Json(901);
                    else if (retransmitDetails.Fax.Length < 12)
                        return Json(902);
                    if (!Rgxfax.IsMatch(retransmitDetails.Fax) || retransmitDetails.Fax.Length > 100)
                        return Json(903);
                    else
                        return PartialView(retransmitDetails);
                }
                else if (retransmitDetails.efax == "email")
                {
                    Regex Rgxemail = new Regex(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$");
                    if (retransmitDetails.Email == null)
                        return Json(904);
                    else if (!Rgxemail.IsMatch(retransmitDetails.Email))
                        return Json(905);
                    else
                        return PartialView(retransmitDetails);
                }
                else
                    return PartialView(retransmitDetails);
            }
        }
        #endregion public ActionResult Retransmit(int transmissionId)
    }
}