using System;
using System.Collections.Generic;
using System.Linq;
using STP.Web.Filters;
using System.Web.Mvc;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.ServiceAccess.Structures;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.MovementsAndNotifications;
using System.Text;
using STP.Common.EncryptDecrypt;
using STP.Common.Logger;
using PagedList;
using System.Xml;
using System.IO;
using STP.ServiceAccess.SecurityAndUsers;
using System.Globalization;
using STP.Domain.RoadNetwork.RoadOwnership;
using System.Threading.Tasks;
using STP.Domain.Communications;
using STP.ServiceAccess.CommunicationsInterface;

namespace STP.Web.Controllers
{    
    public class StructuresController : Controller
    {
        private readonly IStructuresService structuresService;
        private readonly IStructureDeligationService structuredeligationService;
        private readonly IApplicationService applicationService;
        private readonly IMovementsService movementsService;
        private readonly IUserService userService;
        private readonly ICommunicationsInterfaceService communicationService;

        public StructuresController(IStructuresService structuresService, IStructureDeligationService structuredeligationService, IApplicationService applicationService, IMovementsService movementsService, IUserService userService, ICommunicationsInterfaceService communicationService)
        {
            this.structuresService = structuresService;
            this.structuredeligationService = structuredeligationService;
            this.applicationService = applicationService;
            this.movementsService = movementsService;
            this.userService = userService;
            this.communicationService = communicationService;
        }

        [AuthorizeUser(Roles = "12000,12002,40002,40003,40004")]
        public ActionResult MyStructures(int? x, int? y, bool IsAgreedAppl = false, string StructureCode = "0",string structId="",string ConstrId="",int assessmentFlag=0)
        {
            ViewBag.X = x;
            ViewBag.Y = y;
            ViewBag.StructureId = structId;
            ViewBag.IsConstrId = ConstrId;
            var ConstraintId = TempData["ConstraintIDViewConst"];
            if (ConstraintId != null)
            {
                ViewBag.ConstraintID = ConstraintId;
                TempData["ConstraintIDViewConst"] = 0;
            }
            else
            {
                ViewBag.ConstraintID = 0;
            }

            TempData.Remove("ConstraintIDViewConst");

            ViewBag.Assessmentflag = assessmentFlag;
            ViewBag.IsAgreedAppl = IsAgreedAppl;
            ViewBag.StructureCode = StructureCode;
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            ViewBag.usrTypeId = SessionInfo.UserTypeId;

            if (SessionInfo.GeoRegion != null && SessionInfo.GeoRegion.OrdinatesArray.Count() >= 4)
            {
                ViewBag.x1 = SessionInfo.GeoRegion.OrdinatesArray[0];
                ViewBag.y1 = SessionInfo.GeoRegion.OrdinatesArray[1];
                ViewBag.x2 = SessionInfo.GeoRegion.OrdinatesArray[2];
                ViewBag.y2 = SessionInfo.GeoRegion.OrdinatesArray[3];
            }
            return View();
        }
        [HttpPost]
        public JsonResult MyStructureInfoList(int otherOrg, int page, int left, int right, int bottom, int top)
        {
            int limit;
            int organisationId;
            List<StructureInfo> structureInfoList = new List<StructureInfo>();
            List<StructureInfo> gStructureInfoList;

            if (page == 0)
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return Json(new { result = "Session out" });
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                if (otherOrg == -1)
                {
                    organisationId = -1;
                    otherOrg = 1;
                }
                else
                {
                    organisationId = (int)SessionInfo.OrganisationId;
                }

                ViewBag.OrgId = organisationId;
                gStructureInfoList = structuresService.MyStructureInfoList(organisationId, otherOrg, left, right, bottom, top);
                Session["gStructureInfoList"] = gStructureInfoList;
                return Json(new { result = gStructureInfoList.Count });
            }

            gStructureInfoList = (List<StructureInfo>)Session["gStructureInfoList"];
            if (gStructureInfoList.Count < (page * 1000))
                limit = ((page - 1) * 1000) + (gStructureInfoList.Count % 1000);
            else
                limit = page * 1000;

            for (int i = (page - 1) * 1000; i < limit; i++)
            {
                structureInfoList.Add(gStructureInfoList[i]);
            }
            return Json(new { result = structureInfoList },
            "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        #region public ActionResult ReviewSummary(int structureId,int arrgid=0)
        public ActionResult ReviewSummary(long structureId)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var SessionInfo = (UserInfo)Session["UserInfo"];

            int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(structureId, SessionInfo.OrganisationId);
            int orgCheck = structuresService.CheckStructureOrganisation((int)SessionInfo.OrganisationId, structureId); // to check the sturcutre belongs to same organisation id 8389 related fix
            if (orgCheck == 0)
            {
                return RedirectToAction("Error", "Home");
            }
            int userValidateFlag = 1;

            List<StructureGeneralDetails> objListStructureGeneralDetails = null;
            List<StructureSectionList> objListStructureSection = null;
            List<AffStructureGeneralDetails> objDetailList = null;

            if (chkValidStructCnt > 0)
            {
                objListStructureGeneralDetails = structuresService.ViewGeneralDetails(structureId);
                objListStructureSection = structuresService.ViewStructureSections(structureId);

                objDetailList = new List<AffStructureGeneralDetails>();

                if (objListStructureGeneralDetails != null && objListStructureGeneralDetails.Count !=0)
                {
                    objDetailList = applicationService.GetStructureDetailList(objListStructureGeneralDetails[0].ESRN, 0);//2nd parameter is section ID used to fetch signed structure c onstraint
                }
            }
            else
            {
                userValidateFlag = 0;
            }
           
            ViewBag.StructureId = structureId;
            ViewBag.ESRN = objListStructureGeneralDetails != null && objListStructureGeneralDetails.Count != 0 ? objListStructureGeneralDetails[0].ESRN : "";

            ViewBag.ListStructureGeneralDetails = objListStructureGeneralDetails;
            ViewBag.ListStructureSections = objListStructureSection;
            ViewBag.ListStructureOwner = objListStructureGeneralDetails;
            ViewBag.structureOwnerchain = objDetailList;
            ViewBag.UserValidateFlag = userValidateFlag;
            return View("ReviewSummary");
        }
        #endregion
        #region public ActionResult StructureReviewSummaryLeftPanel()
        public ActionResult StructureReviewSummaryLeftPanel(string sectionType, string structureId, string sectionId, string structureName, string ESRN)
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];

            #region For document status viewer
            if (SessionInfo.HelpdeskRedirect == "true")
            {
                ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
            }
            #endregion

            ViewBag.sectionType = sectionType;
            ViewBag.StructureId = structureId;
            ViewBag.structureName = structureName;
            ViewBag.ESRN = ESRN;
            ViewBag.sectionId = sectionId;
            
            # region Code added by NetWeb to encrypt StructureId and Structure Name

            string encryptStructureId = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(structureId));
            string encryptStructureName = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(structureName));
            string encryptsectionType = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(sectionType));
            string encryptESRN = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(ESRN));
            string encryptsectionId = MD5EncryptDecrypt.EncryptDetails(Convert.ToString(sectionId));

            ViewBag.EncryptStructureId = encryptStructureId;
            ViewBag.EncryptStructureName = encryptStructureName;
            ViewBag.EncryptSectionType = encryptsectionType;
            ViewBag.EncryptESRN = encryptESRN;
            ViewBag.EncryptSectionId = encryptsectionId;
            # endregion

            return View();
        }
        #endregion
        #region public ActionResult ReviewStructureSectionSummary(int structureId, int sectionId)
        public ActionResult ReviewStructureSectionSummary(int structureId, int sectionId, string sectionType)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
            long OrgID = ObjUserInfo.OrganisationId;
            DimensionConstruction objDimentionConstruction = structuresService.ViewDimensionConstruction(structureId, sectionId);
            List<SpanData> objListSpanData = structuresService.ViewSpanData(structureId, sectionId);
            ImposedConstraints objImposedConstraints = structuresService.ViewimposedConstruction(structureId, sectionId);
            ManageStructureICA objManageStructureICA = structuresService.ViewEnabledICA(structureId, sectionId, OrgID);
            List<SvReserveFactors> objViewSVData = structuresService.ViewSVData(structureId, sectionId);

            ViewBag.DimentionConstruction = objDimentionConstruction;
            ViewBag.ListSpanData = objListSpanData;
            ViewBag.ImposedConstraintas = objImposedConstraints;
            ViewBag.ManageStructureICA = objManageStructureICA;
            ViewBag.ViewSVData = objViewSVData;
            ViewBag.sectionType = sectionType;
            TempData["sectionId"] = sectionId;

            return View();

        }
        #endregion

        #region SVData
        [HttpGet]
        public ActionResult SVData(int StructId, int SectionId, string structName, string ESRN)
        {
            try
            {

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var SessionInfo = (UserInfo)Session["UserInfo"];

                #region For document status viewer
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(StructId, SessionInfo.OrganisationId);
                int orgCheck = structuresService.CheckStructureOrganisation((int)SessionInfo.OrganisationId, (long)StructId); // to check the sturcutre belongs to same organisation id 8389 related fix
                if (orgCheck == 0)
                {
                    return RedirectToAction("Error", "Home");
                }

                SVDataWithLoadModel objSVData = new SVDataWithLoadModel();
                if (chkValidStructCnt > 0)
                {
                    var res = structuresService.GetSVData(StructId, SectionId);
                    foreach (var item in res)
                    {
                        ViewBag.SVDerivation = item.SVDerivation;
                        if (item.VehicleType == 340002)
                        {
                            objSVData.WithSV80 = item.WithLoad;
                            objSVData.WithoutSV80 = item.WithoutLoad;
                        }
                        if (item.VehicleType == 340003)
                        {
                            objSVData.WithSV100 = item.WithLoad;
                            objSVData.WithoutSV100 = item.WithoutLoad;
                        }
                        if (item.VehicleType == 340004)
                        {
                            objSVData.WithSV150 = item.WithLoad;
                            objSVData.WithoutSV150 = item.WithoutLoad;
                        }
                        if (item.VehicleType == 340005)
                        {
                            objSVData.WithSVTrain = item.WithLoad;
                            objSVData.WithoutSVTrain = item.WithoutLoad;
                        }
                        if (item.VehicleType == 340006)
                        {
                            objSVData.WithSVTT = item.WithLoad;
                            objSVData.WithoutSVTT = item.WithoutLoad;
                        }
                    }

                    ViewBag.StructureId = StructId;
                    ViewBag.SectionId = SectionId;
                    ViewBag.structName = structName;
                    ViewBag.ESRN = ESRN;

                    if (SectionId != null)
                    {
                        Session["checkFlag"] = "true";
                        Session["sectionID"] = SectionId;
                    }
                    // ViewBag.msg = 1;
                }
                else
                {
                    objSVData = null;
                }

                return View(objSVData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region StoreDimensionData
        [HttpPost]
        public JsonResult StoreSVData(SVDataWithLoadModel objSVData)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }
            #endregion
            TempData["SVDataWithLoadModelTemp"] = objSVData;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Update SVData
        [HttpPost]
        public JsonResult SVDataJson(SVDataWithLoadModel objsvdata, long StructId, long SectionId, int SVDerivation, string structName, string ESRN)
        {
            List<SVDataList> obj = new List<SVDataList>();
            int VehicleType;
            double? WithLoad;
            double? WithoutLoad;
            //var result=0;          

            if (!string.IsNullOrEmpty(Convert.ToString(TempData["SVDataWithLoadModelTemp"])))
            {
                objsvdata = (SVDataWithLoadModel)TempData["SVDataWithLoadModelTemp"];
            }

            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
            string UserName = ObjUserInfo.UserName;

            VehicleType = 340002;
            if (objsvdata.WithSV80 == null)
            {
                WithLoad = null;
            }
            else
            {
                WithLoad = (double)objsvdata.WithSV80;
            }
            if (objsvdata.WithoutSV80 == null)
            {
                WithoutLoad = null;
            }
            else
            {
                WithoutLoad = (double)objsvdata.WithoutSV80;
            }
            UpdateSVParams objUpdateSVParams = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = SectionId,
                StructId = StructId,
                SVDerivation = SVDerivation,
                UserName = UserName,
                ManualFlag = 0,
                HbWithLoad = null,
                HbWithoutLoad = null

            };
            var result = structuresService.UpdateSVData(objUpdateSVParams);

            VehicleType = 340003;
            if (objsvdata.WithSV100 == null)
            {
                WithLoad = null;
            }
            else
            {
                WithLoad = (double)objsvdata.WithSV100;
            }
            if (objsvdata.WithoutSV100 == null)
            {
                WithoutLoad = null;
            }
            else
            {
                WithoutLoad = (double)objsvdata.WithoutSV100;
            }
            UpdateSVParams objUpdateSVParams1 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = SectionId,
                StructId = StructId,
                SVDerivation = SVDerivation,
                UserName = UserName,
                ManualFlag = 0,
                HbWithLoad = null,
                HbWithoutLoad = null
            };
            var result1 = structuresService.UpdateSVData(objUpdateSVParams1);

            VehicleType = 340004;
            if (objsvdata.WithSV150 == null)
            {
                WithLoad = null;
            }
            else
            {
                WithLoad = (double)objsvdata.WithSV150;
            }
            if (objsvdata.WithoutSV150 == null)
            {
                WithoutLoad = null;
            }
            else
            {
                WithoutLoad = (double)objsvdata.WithoutSV150;
            }
            UpdateSVParams objUpdateSVParams2 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = SectionId,
                StructId = StructId,
                SVDerivation = SVDerivation,
                UserName = UserName,
                ManualFlag = 0,
                HbWithLoad = null,
                HbWithoutLoad = null
            };
            var result2 = structuresService.UpdateSVData(objUpdateSVParams2);

            VehicleType = 340005;
            if (objsvdata.WithSVTrain == null)
            {
                WithLoad = null;
            }
            else
            {
                WithLoad = (double)objsvdata.WithSVTrain;
            }
            if (objsvdata.WithoutSVTrain == null)
            {
                WithoutLoad = null;
            }
            else
            {
                WithoutLoad = (double)objsvdata.WithoutSVTrain;
            }
            UpdateSVParams objUpdateSVParams3 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = SectionId,
                StructId = StructId,
                SVDerivation = SVDerivation,
                UserName = UserName,
                ManualFlag = 0,
                HbWithLoad = null,
                HbWithoutLoad = null
            };
            var result3 = structuresService.UpdateSVData(objUpdateSVParams3);

            VehicleType = 340006;
            if (objsvdata.WithSVTT == null)
            {
                WithLoad = null;
            }
            else
            {
                WithLoad = (double)objsvdata.WithSVTT;
            }
            if (objsvdata.WithoutSVTT == null)
            {
                WithoutLoad = null;
            }
            else
            {
                WithoutLoad = (double)objsvdata.WithoutSVTT;
            }
            UpdateSVParams objUpdateSVParams4 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = SectionId,
                StructId = StructId,
                SVDerivation = SVDerivation,
                UserName = UserName,
                ManualFlag = 0,
                HbWithLoad = null,
                HbWithoutLoad = null
            };
            var result4 = structuresService.UpdateSVData(objUpdateSVParams4);

            ViewBag.msg = "1";
            ViewBag.StructureId = StructId;
            ViewBag.SectionId = SectionId;
            ViewBag.structName = structName;
            ViewBag.ESRN = ESRN;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion


        

        #region public ActionResult StructureList(int? page, int? pageSize, string Mode, string orgid)
        /// <summary>
        /// ActionResult to display Structure List view
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="searchString">search string</param>
        /// <returns>View</returns>
        [AuthorizeUser(Roles = "12001")]
        public ActionResult StructureList(int? page, int? pageSize, string Mode, string orgid, string arrId, string EditOrgId, string flag = null,int ? sortOrder = null, int? sortType = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Structure/StructureList actionResult method started successfully");

                #region Session check
                UserInfo SessionInfo = null;
                int presetFilter = 0;
                //ViewBag.sortOrder = sortOrder == null ? 2 : sortOrder;

                //presetFilter = sortType != null ? (int)sortType : presetFilter;
                //ViewBag.sortType = presetFilter;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion

                #region Page access check
                if (!PageAccess.GetPageAccess("12003"))
                {
                    return RedirectToAction("Error", "Home");
                }
                #endregion

                #region Paging Part
                if(TempData["PageNum"] != null)
                {
                    page = Convert.ToInt32(TempData["PageNum"]);
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

                if (!string.IsNullOrEmpty(Mode) && Mode.Equals("Edit"))
                {
                    Session["StructureMode"] = "Edit";
                    Session["PageNumber"] = 1;
                    if (Session["Page"] != null && page == null)
                    {
                        pageNumber = (int)Session["Page"]; // get the last visited page number from session and update it as current page
                    }
                    else
                    {
                        Session["Page"] = pageNumber;//save last clicked page number                       
                    }
                }
                else
                {
                    TempData.Remove("Mode");
                    TempData.Remove("EditFlag");
                    Session["StructureMode"] = "View";
                    Session["Page"] = 1;
                    if (Session["PageNumber"] != null && page == null)
                    {
                        pageNumber = (int)Session["PageNumber"];
                    }
                    else
                    {
                        Session["PageNumber"] = pageNumber;//save last clicked page number
                    }
                }

                if (Mode == "Edit")
                {
                    TempData["EditFlag"] = "1";
                    //TempData["arrId"] = arrId.ToString();

                }

                TempData.Keep("EditFlag");
                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;

                #endregion

                #region Search criteria
                SearchStructures objSearch = new SearchStructures();
                if (Session["g_StructSearch"] != null)
                {
                    objSearch = (SearchStructures)Session["g_StructSearch"];
                   
                }
               
                    objSearch.sortOrder = sortOrder != null ? (int)sortOrder : 2; //name
                    objSearch.presetFilter = sortType != null ? (int)sortType : 0; // asc
                    ViewBag.SortOrder = objSearch.sortOrder;

                    ViewBag.SortType = objSearch.presetFilter;

                    ViewBag.filterObject = objSearch;

                
                #endregion
                //objSearch.sortOrder = sortOrder;
                //objSearch.presetFilter = presetFilter;

                if (!string.IsNullOrEmpty(arrId))
                    TempData["arrId"] = arrId;

                if (!string.IsNullOrEmpty(EditOrgId))
                    TempData["EditOrgId"] = EditOrgId;

                if (!string.IsNullOrEmpty(orgid))
                    TempData["orgid"] = orgid;

                if (!string.IsNullOrEmpty(Mode))
                    TempData["Mode"] = Mode;

                TempData.Keep("Mode");
                if (!string.IsNullOrEmpty(Convert.ToString(TempData["ModeSearch"])))
                {
                    Mode = Convert.ToString(TempData["ModeSearch"]);
                    pageNumber = 1;
                }

                ViewBag.Mode = Mode;

                List<StructureSummary> summaryObjList = structuresService.GetStructureListSearch(Convert.ToInt32(SessionInfo.OrganisationId), pageNumber, (int)pageSize, objSearch, presetFilter);

                if (summaryObjList.Count > 0)
                {
                    ViewBag.TotalCount = Convert.ToInt32(summaryObjList[0].TotalRecordCount);
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }
                var structureSummaryAsIPagedList = new StaticPagedList<StructureSummary>(summaryObjList, pageNumber, (int)pageSize, ViewBag.TotalCount);
                //check session value and change check change
                if (Session["structureInDelegList"] != null)
                {

                    List<StructureInDelegationList> strucDelListCheck;
                    strucDelListCheck = (List<StructureInDelegationList>)Session["structureInDelegList"];
                    foreach (var item in strucDelListCheck)
                    {
                        structureSummaryAsIPagedList.Where(s => s.StructureId == item.StructureId).ToList().TrueForAll(s => s.StructCodeSelected = true);
                    }
                }
                //
                TempData.Keep("arrId");
                TempData.Keep("EditOrgId");
                TempData.Keep("orgid");
                TempData.Keep("delegationList");
                ViewBag.filterObject = objSearch;
                return View("StructureList", structureSummaryAsIPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/StructureList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion    


       
        #region StructureHistory
        public ActionResult StructureHistory(string structId, string structName, string ESRN, int? sectionID, int? page, int? pageSize=10)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Structures/StructureHistory actionResult method started successfully");

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId =(int) SessionInfo.OrganisationId;

                long lngStructID = !string.IsNullOrEmpty(structId) ? Convert.ToInt64(structId) : 0;
                int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(lngStructID, SessionInfo.OrganisationId);
                var StructHistAsIPagedList = (StaticPagedList<StructureHistoryList>)null;
                int orgCheck = structuresService.CheckStructureOrganisation(organisationId, lngStructID); // to check the sturcutre belongs to same organisation id 8389 related fix
                if (orgCheck == 0)
                {
                    //return RedirectToAction("Error", "Home");
                }
                if (chkValidStructCnt > 0)
                {

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
                    ViewBag.StructName = structName;
                    ViewBag.ESRN = ESRN;
                    ViewBag.StructureId = structId;
                    if (sectionID != null)
                    {
                        Session["checkFlag"] = "true";
                        Session["sectionID"] = sectionID;
                    }
                    int totalCount = structuresService.GetStructureHistoryCount(lngStructID);
                    List<StructureHistoryList> GridListObj = structuresService.GetStructureHistoryById(pageNumber, (int)pageSize, lngStructID);
                    ViewBag.StructId = structId;
                    StructHistAsIPagedList = new StaticPagedList<StructureHistoryList>(GridListObj, pageNumber, (int)pageSize, totalCount);
                }
                return View("StructureHistory",StructHistAsIPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Home/Index, Exception: {0}", ex));
                throw ex;
            }

        }
        #endregion
        #region public ActionResult StructSummaryFilter()
        /// <summary>
        /// Structure summary filter that goes to the left panel
        /// </summary>
        /// <returns>partial view</returns>
        public ActionResult StructSummaryFilter(int? page, int? pageSize,int ? sortOrder = null, int? sortType = null)
        {

            var SessionInfo = (UserInfo)Session["UserInfo"];


            string structType = string.Empty;
            string icaMethod = string.Empty;

            SearchStructures objSearch = new SearchStructures();
            if (Session["g_StructSearch"] != null)
            {
                objSearch = (SearchStructures)Session["g_StructSearch"];
                switch (objSearch.StructureType)
                {
                    case "510001":
                        objSearch.StructureType = "Underbridge";
                        break;

                    case "510002":
                        objSearch.StructureType = "Overbridge";
                        break;

                    case "510003":
                        objSearch.StructureType = "Under and over bridge";
                        break;

                    case "510004":
                        objSearch.StructureType = "Level crossing";
                        break;

                }
                structType = objSearch.StructureType;
                icaMethod = objSearch.ICAMethod;
                
            }
            
            /*method to fill struct type dropdown list*/
            GetStructureTypeDropDown(structType);
            /*Method to fill ICA Method dropdown*/
            GetICAMethod(icaMethod);


            List<DelegationArrangment> objDelegationArrangment = structuredeligationService.viewDelegationArrangment(SessionInfo.OrganisationId);
            ViewBag.delegateName = new SelectList(objDelegationArrangment, "arrangmentID", "name");

            ViewBag.page=page;
            ViewBag.pageSize=pageSize;
            ViewBag.sortOrder=sortOrder;
            ViewBag.sortType=sortType;
            return PartialView(objSearch);
        }
        #endregion        

        #region public ActionResult SaveStructSearch(SearchStruct objSearch)
        public ActionResult SaveStructSearch(SearchStructures objSearch, string Mode = "", int Flag = 0, int? page = null, int? pageSize = null, int? sortOrder = null, int? sortType = null, bool StrcutNotInDel = false)
        {


            if (!string.IsNullOrEmpty(Mode))
                TempData["ModeSearch"] = Mode;
            if (Flag == 1)
            {
                TempData["EditFlag"] = 1;
            }
            TempData["PageNum"] = page != null ? page : 1;
            // return RedirectToAction("StructureList", new { Mode = TempData["ModeSearch"] });
            if (StrcutNotInDel)
            { 
            Session["g_NotDelegatedStructSearch"] = objSearch;
            return RedirectToAction("StructureNotInDelegationList", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                   STP.Web.Helpers.EncryptionUtility.Encrypt("sortOrder=" + sortOrder +
                   "&sortType=" + sortType +
                   "&page=" + page +
                   "&pageSize=" + pageSize+
                   "&Mode="+Mode)
            });
            }
            else
                    {
             Session["g_StructSearch"] = objSearch;
             return RedirectToAction("StructureList", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                    STP.Web.Helpers.EncryptionUtility.Encrypt("sortOrder=" + sortOrder +
                    "&sortType=" + sortType +
                    "&page=" + page +
                    "&pageSize=" + pageSize +
                   "&Mode=" + Mode)
            });
            }
        }
        #endregion

        #region ReviewCautionsList
        public ActionResult ReviewCautionsList(int? page, int? pageSize, long structureId, long sectionId)
        {

            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion

            #region For document status viewer
            if (SessionInfo.HelpdeskRedirect == "true")
            {
                ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
            }
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

            ViewBag.StructureID = structureId ;
            ViewBag.SectionID = sectionId;
            CautionListParams cautionListParams = new CautionListParams
            {
                PageNumber = pageNumber,
                PageSize = (int)pageSize,
                StructureId = structureId,
                SectionId = sectionId
            };

            List<StructureModel> structureList = structuresService.GetCautionList(cautionListParams);
            XmlDocument Doc = new XmlDocument();
            foreach (StructureModel stru in structureList)
            {
                // Date 13 Feb 2015  Ticket No 3706
                if (!string.IsNullOrEmpty(stru.SpecificActionXML))
                {
                    Doc.PreserveWhitespace = true;
                    Doc.LoadXml(stru.SpecificActionXML);
                    XmlNodeList parentNode = Doc.GetElementsByTagName("SpecificAction");
                    if (parentNode.Count > 0)
                    {
                        string text = parentNode[0].InnerXml.Replace("\"_\"", "&nbsp;");
                        text = parentNode[0].InnerXml.Replace("_", "&nbsp;");
                        stru.SpecificAction = text;
                    }
                    else
                    {
                        XmlNodeList parentNodebold = Doc.GetElementsByTagName("caution:SpecificAction");
                        stru.SpecificAction = parentNodebold[0].InnerText;
                    }
                }
            }
            if (structureList.Count > 0)
            {
                ViewBag.TotalCount = Convert.ToInt32(structureList[0].TotalRecordCount);
                IPagedList<StructureModel> model = structureList.ToPagedList(page ?? 1, 10);
                page = model.PageCount;
            }
            else
            {
                ViewBag.TotalCount = 0;
            }

            var struPageList = new StaticPagedList<StructureModel>(structureList, pageNumber, (int)pageSize, ViewBag.TotalCount);
            return PartialView(struPageList);


        }
        #endregion
        #region StructureCautionHistory
        public ActionResult StructureCautionHistory(int? page, int? pageSize, long structureId, long sectionId)
        {
            try
            {
                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (ModelState.IsValid)
                {
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

                    SessionInfo = (UserInfo)Session["UserInfo"];

                    STP.Domain.SecurityAndUsers.UserInfo userInfo = new STP.Domain.SecurityAndUsers.UserInfo();

                    ViewBag.StructureID = structureId;
                    ViewBag.SectionID = sectionId;

                    List<StructureModel> lstStructureModel = structuresService.GetStructureHistory(pageNumber, (int)pageSize, structureId);

                    if (lstStructureModel.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(lstStructureModel[0].TotalRecordCount);

                        IPagedList<StructureModel> model = lstStructureModel.ToPagedList(page ?? 1, 10);

                        page = model.PageCount;

                        ViewBag.TotalPages = model.PageCount;
                    }
                    else
                    {
                        ViewBag.TotalCount = 0;
                        ViewBag.TotalPages = 0;
                    }

                    var structureIPagedList = new StaticPagedList<StructureModel>(lstStructureModel, pageNumber, (int)pageSize, ViewBag.TotalCount);

                    return PartialView("StructureCautionHistory", structureIPagedList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/StructureCautionHistory, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/StructureCautionHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region CreateCaution
        public ActionResult CreateCaution(long structureId, long cautionId, string mode, long sectionId)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion
                #region Check Page Request

                #endregion

                StructureModel StructuremodelBase = new StructureModel();
                StructuremodelBase.Mode = mode;

                StructuremodelBase.StructureId = structureId;
                StructuremodelBase.SectionId = sectionId;

                ViewBag.StructureID = structureId;
                ViewBag.SectionID = sectionId;

                if (mode == "add")
                {
                    if (SessionInfo != null)
                    {
                        StructuremodelBase.StructureId = structureId;
                        StructuremodelBase.SectionId = sectionId;
                        StructuremodelBase.OrganizationName = SessionInfo.OrganisationName;
                        StructuremodelBase.OrganisationId = SessionInfo.OrganisationId;
                    }
                    if (TempData["StructureCode"] != null)
                    {
                        StructuremodelBase.StructureCode = Convert.ToString(TempData["StructureCode"]);
                    }
                    if (TempData["StructureName"] != null)
                    {
                        StructuremodelBase.StructureName = Convert.ToString(TempData["StructureName"]);
                    }
                    StructuremodelBase.SelectedType = ActionType.StandardCaution;
                    StructuremodelBase.CreatorIsContact = false;
                    StructuremodelBase.Height = string.Empty;
                    StructuremodelBase.Width = string.Empty;
                    StructuremodelBase.Length = string.Empty;
                    StructuremodelBase.Speed = string.Empty;
                    TempData["StructuremodelBase"] = StructuremodelBase;
                }
                else if (mode == "edit")//get data and fill
                {
                    StructuremodelBase = structuresService.GetCautionDetails(cautionId);
                    StructuremodelBase.CreatorIsContact = StructuremodelBase.OwnerIsContactFlag;

                    //meters , feet and inches
                    if (SessionInfo.VehicleUnits == 692002)
                    {

                        // By manoj kumar choubey
                        if (StructuremodelBase.MaxHeight != 0)
                        {
                            float MAX_HEIGHT_MTRS_ = StructuremodelBase.MaxHeight;
                            int FeetL = (int)MAX_HEIGHT_MTRS_;
                            float InchL = MAX_HEIGHT_MTRS_ - FeetL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            StructuremodelBase.Height = FeetL + "'" + height + "\"";
                        }
                        else
                        {
                            StructuremodelBase.Height = string.Empty;
                        }
                        if (StructuremodelBase.MaxWidth != 0)
                        {
                            float MAX_WIDTH_MTRS_ = StructuremodelBase.MaxWidth;
                            int widthL = (int)MAX_WIDTH_MTRS_;
                            float InchL = MAX_WIDTH_MTRS_ - widthL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            StructuremodelBase.Width = widthL + "'" + height + "\"";
                        }
                        else
                        {
                            StructuremodelBase.Width = string.Empty;
                        }
                        if (StructuremodelBase.MaxLength != 0)
                        {
                            float MAX_LENGTH_MTRS_ = StructuremodelBase.MaxLength;
                            int lengthL = (int)MAX_LENGTH_MTRS_;
                            float InchL = MAX_LENGTH_MTRS_ - lengthL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            StructuremodelBase.Length = lengthL + "'" + height + "\"";
                        }
                        else
                        {
                            StructuremodelBase.Length = string.Empty;
                        }
                        if (StructuremodelBase.MinSpeed != 0)
                        {
                            StructuremodelBase.Speed = StructuremodelBase.MinSpeed.ToString();
                        }
                        else
                        {
                            StructuremodelBase.Speed = string.Empty;
                        }
                    }
                    else
                    {
                        if (StructuremodelBase.MaxHeightMetres != 0)
                        {
                            StructuremodelBase.Height = Convert.ToString(StructuremodelBase.MaxHeightMetres);
                        }
                        else
                        {
                            StructuremodelBase.Height = string.Empty;
                        }
                        if (StructuremodelBase.MaxWidthMetres != 0)
                        {
                            StructuremodelBase.Width = Convert.ToString(StructuremodelBase.MaxWidthMetres);
                        }
                        else
                        {
                            StructuremodelBase.Width = string.Empty;
                        }
                        if (StructuremodelBase.MaxLengthMetres != 0)
                        {
                            StructuremodelBase.Length = Convert.ToString(StructuremodelBase.MaxLengthMetres);
                        }
                        else
                        {
                            StructuremodelBase.Length = string.Empty;
                        }
                        if (StructuremodelBase.MinSpeedKmph != 0)
                        {
                            StructuremodelBase.Speed = Convert.ToString(StructuremodelBase.MinSpeedKmph);
                        }
                        else
                        {
                            StructuremodelBase.Speed = string.Empty;
                        }
                    }


                    XmlDocument Doc = new XmlDocument();
                    // Date 13 Feb 2015 Ticket No 3706
                    if (!string.IsNullOrEmpty(StructuremodelBase.SpecificAction))
                    {
                        Doc.PreserveWhitespace = true;
                        Doc.LoadXml(StructuremodelBase.SpecificAction);
                        XmlNodeList parentNode = Doc.GetElementsByTagName("SpecificAction");
                        if (parentNode.Count > 0)
                        {
                            string text = parentNode[0].InnerXml.Replace("\"_\"", "&nbsp;");
                            text = parentNode[0].InnerXml.Replace("_", "&nbsp;");
                            string textDescription = text;
                            foreach (XmlNode childrenNode in parentNode)
                            {

                                if (childrenNode.InnerXml.Contains("Bold"))
                                {
                                    StructuremodelBase.Bold = true;
                                    XmlNodeList parentNodeBold = Doc.GetElementsByTagName("Bold");
                                    textDescription = parentNodeBold[0].InnerXml;
                                }
                                if (childrenNode.InnerXml.Contains("Italic"))
                                {
                                    StructuremodelBase.Italic = true;
                                    XmlNodeList parentNodeItalic = Doc.GetElementsByTagName("Italic");
                                    textDescription = parentNodeItalic[0].InnerXml;
                                }
                                if (childrenNode.InnerXml.Contains("Underline"))
                                {
                                    StructuremodelBase.Underline = true;
                                    XmlNodeList parentNodeUnderline = Doc.GetElementsByTagName("Underline");
                                    textDescription = parentNodeUnderline[0].InnerXml;
                                }
                            }
                            StructuremodelBase.DirectionName = textDescription;
                            StructuremodelBase.SelectedType = ActionType.SpecificAction;//#2313
                        }
                        else
                        {
                            XmlNodeList parentNodeStandard = Doc.GetElementsByTagName("caution:SpecificAction");
                            StructuremodelBase.DirectionName = parentNodeStandard[0].InnerText;
                            StructuremodelBase.SelectedType = ActionType.StandardCaution;//#2313
                        }
                    }

                    TempData["StructuremodelBase"] = StructuremodelBase;
                }
                else//get data and fill when it return from CautionAddReport page
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["Structuremodelalter"])))
                    {
                        StructuremodelBase = (StructureModel)TempData["Structuremodelalter"];
                    }
                }
                // Converting kgs to tonnes
                StructuremodelBase.GrossWeight = (double)((StructuremodelBase.MaxGrossWeightKgs != 0) ? StructuremodelBase.MaxGrossWeightKgs / 1000 : 0);
                StructuremodelBase.AxleWeight = (double)((StructuremodelBase.MaxAxleWeightKgs != 0) ? StructuremodelBase.MaxAxleWeightKgs / 1000 : 0);

                ViewBag.Bold = StructuremodelBase.Bold;
                ViewBag.Italic = StructuremodelBase.Italic;
                ViewBag.Underline = StructuremodelBase.Underline;
                ViewBag.UOM = SessionInfo.VehicleUnits;
                ViewBag.mode = mode;
                TempData.Keep("Structuremodelalter");
                TempData.Keep("StructuremodelBase");
                return View(StructuremodelBase);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("CreateCaution/CreateCaution, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region CautionAddReport
        public ActionResult CautionAddReport(long structureId = 0, long sectionId = 0)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                #endregion
                if (ModelState.IsValid)
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    StructureModel Structuremodelalter = new StructureModel();
                    StructureModel StructuremodelBase = new StructureModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["Structuremodelalter"])))
                    {
                        Structuremodelalter = (StructureModel)TempData["Structuremodelalter"];
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["StructuremodelBase"])))
                    {
                        StructuremodelBase = (StructureModel)TempData["StructuremodelBase"];
                    }
                    Dictionary<string, string> changeList = new Dictionary<string, string>();


                    string PreUnspecified = " (previously unspecified)";
                    string unspecified = " unspecified ";
                    string PreSpecified = " (previously ";
                    string closeSpecified = ")";
                    string SetTo = " set to '";
                    string Meters = " m";
                    string FeetInches = " ft/in";
                    string Tonnes = " t";

                    // Identify values.
                    //check if it is for add 
                    if (StructuremodelBase.CautionId == 0)//its for add
                    {
                        ViewBag.CautionId = StructuremodelBase.CautionId;

                        StructuremodelBase.SectionId = sectionId;
                        Structuremodelalter.SectionId = sectionId;

                        if (!string.IsNullOrEmpty(Structuremodelalter.CautionName))
                        {
                            changeList.Add("CAUTION_NAME", "Name" + PreUnspecified + SetTo + Structuremodelalter.CautionName + "'");
                        }
                        if (StructuremodelBase.SelectedType != Structuremodelalter.SelectedType)
                        {
                            changeList.Add("StandardCaution", "Action type" + PreSpecified + StructuremodelBase.SelectedType.ToString() + closeSpecified + SetTo + Structuremodelalter.SelectedType.ToString() + "'");
                        }

                        if (!string.IsNullOrEmpty(Structuremodelalter.DirectionName))
                        {
                            changeList.Add("DIRECTION_NAME", "Action" + PreUnspecified + SetTo + Structuremodelalter.DirectionName + "'");
                        }

                        if (StructuremodelBase.Bold != Structuremodelalter.Bold)
                        {
                            changeList.Add("Bold", "Bold" + PreSpecified + (Convert.ToString(StructuremodelBase.Bold).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.Bold).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (StructuremodelBase.Italic != Structuremodelalter.Italic)
                        {
                            changeList.Add("Italic", "Italic" + PreSpecified + (Convert.ToString(StructuremodelBase.Italic).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.Italic).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (StructuremodelBase.Underline != Structuremodelalter.Underline)
                        {
                            changeList.Add("Underline", "Underline" + PreSpecified + (Convert.ToString(StructuremodelBase.Underline).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.Underline).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (Structuremodelalter.MaxHeightMetres != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MAX_HEIGHT_MTRS", "Height" + PreUnspecified + SetTo + Structuremodelalter.Height + Meters + "'");
                            }
                            else
                            {
                                changeList.Add("MAX_HEIGHT_MTRS", "Height" + PreUnspecified + SetTo + Structuremodelalter.Height + FeetInches + "'");
                            }
                        }

                        if (Structuremodelalter.MaxWidthMetres != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MAX_WIDTH_MTRS", "Width" + PreUnspecified + SetTo + Structuremodelalter.Width + Meters + "'");
                            }
                            else
                            {
                                changeList.Add("MAX_WIDTH_MTRS", "Width" + PreUnspecified + SetTo + Structuremodelalter.Width + FeetInches + "'");
                            }
                        }
                        if (Structuremodelalter.MaxLengthMetres != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MAX_LENGTH_MTRS", "Length" + PreUnspecified + SetTo + Structuremodelalter.Length + Meters + "'");
                            }
                            else
                            {
                                changeList.Add("MAX_LENGTH_MTRS", "Length" + PreUnspecified + SetTo + Structuremodelalter.Length + FeetInches + "'");
                            }
                        }
                        if (Structuremodelalter.GrossWeight != 0)
                        {
                            changeList.Add("MAX_GROSS_WEIGHT_KGS", "Gross weight" + PreUnspecified + SetTo + Structuremodelalter.GrossWeight + Tonnes + "'");
                        }
                        if (Structuremodelalter.AxleWeight != 0)
                        {
                            changeList.Add("MAX_AXLE_WEIGHT_KGS", "Axle weight" + PreUnspecified + SetTo + Structuremodelalter.AxleWeight + Tonnes + "'");
                        }
                        if (Structuremodelalter.MinSpeedKmph != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MIN_SPEED_KPH", "Speed" + PreUnspecified + SetTo + Structuremodelalter.Speed + " kph" + "'");
                            }
                            else
                            {
                                changeList.Add("MIN_SPEED_KPH", "Speed" + PreUnspecified + SetTo + Structuremodelalter.Speed + " mph" + "'");
                            }
                        }
                        if (StructuremodelBase.CreatorIsContact != Structuremodelalter.CreatorIsContact)
                        {
                            changeList.Add("CREATOR_IS_CONTACT", "Creator is contact" + PreSpecified + (Convert.ToString(StructuremodelBase.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                    }
                    else //its for edit caution
                    {
                        ViewBag.CautionId = StructuremodelBase.CautionId;

                        StructuremodelBase.SectionId = sectionId;
                        Structuremodelalter.SectionId = sectionId;

                        //CAUTION_NAME
                        if (Convert.ToString(StructuremodelBase.CautionName == null ? string.Empty : StructuremodelBase.CautionName).Trim() != Convert.ToString(Structuremodelalter.CautionName == null ? string.Empty : Structuremodelalter.CautionName).Trim())
                        {
                            changeList.Add("CAUTION_NAME", "Name" + PreSpecified + ((StructuremodelBase.CautionName == string.Empty || StructuremodelBase.CautionName == null) ? unspecified : StructuremodelBase.CautionName) + closeSpecified + SetTo + ((Structuremodelalter.CautionName == string.Empty || Structuremodelalter.CautionName == null) ? unspecified : Structuremodelalter.CautionName) + "'");
                        }
                        if (StructuremodelBase.SelectedType != Structuremodelalter.SelectedType)
                        {
                            changeList.Add("StandardCaution", "Action type" + PreSpecified + (Convert.ToString(StructuremodelBase.SelectedType) == string.Empty ? unspecified : Convert.ToString(StructuremodelBase.SelectedType)) + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.SelectedType) == string.Empty ? unspecified : Convert.ToString(Structuremodelalter.SelectedType)) + "'");
                        }
                        if (StructuremodelBase.StandardCaution != Structuremodelalter.StandardCaution)
                        {
                            changeList.Add("StandardCaution", "Standard caution" + PreSpecified + (Convert.ToString(StructuremodelBase.StandardCaution).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.StandardCaution).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (!string.IsNullOrEmpty(StructuremodelBase.DirectionName) || !string.IsNullOrEmpty(Structuremodelalter.DirectionName))
                        {
                            if (StructuremodelBase.DirectionName != Structuremodelalter.DirectionName)
                            {
                                changeList.Add("DIRECTION_NAME", "Action" + PreSpecified + (Convert.ToString(StructuremodelBase.DirectionName == null ? string.Empty : StructuremodelBase.DirectionName) == string.Empty ? unspecified : Convert.ToString(StructuremodelBase.DirectionName)) + closeSpecified + SetTo + ((Structuremodelalter.DirectionName == string.Empty || Structuremodelalter.DirectionName == null) ? unspecified : Structuremodelalter.DirectionName) + "'");
                            }
                        }
                        if (StructuremodelBase.Bold != Structuremodelalter.Bold)
                        {
                            changeList.Add("Bold", "Bold" + PreSpecified + (Convert.ToString(StructuremodelBase.Bold).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.Bold).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (StructuremodelBase.Italic != Structuremodelalter.Italic)
                        {
                            changeList.Add("Italic", "Italic" + PreSpecified + (Convert.ToString(StructuremodelBase.Italic).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.Italic).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (StructuremodelBase.Underline != Structuremodelalter.Underline)
                        {
                            changeList.Add("Underline", "Underline" + PreSpecified + (Convert.ToString(StructuremodelBase.Underline).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.Underline).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (!string.IsNullOrEmpty(StructuremodelBase.Height) || !string.IsNullOrEmpty(Convert.ToString(Structuremodelalter.Height).Replace("''", "\"")))
                        {
                            if (Convert.ToString(StructuremodelBase.Height == null ? string.Empty : StructuremodelBase.Height).Trim() != Convert.ToString(Structuremodelalter.Height == null ? string.Empty : Structuremodelalter.Height).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MAX_HEIGHT_MTRS", "Height" + PreSpecified + (StructuremodelBase.Height == string.Empty ? unspecified : StructuremodelBase.Height) + closeSpecified + SetTo + (Structuremodelalter.Height == string.Empty ? unspecified : Structuremodelalter.Height) + Meters + "'");
                                }
                                else
                                {
                                    changeList.Add("MAX_HEIGHT_MTRS", "Height" + PreSpecified + (StructuremodelBase.Height == string.Empty ? unspecified : StructuremodelBase.Height) + closeSpecified + SetTo + (Structuremodelalter.Height == string.Empty ? unspecified : Structuremodelalter.Height) + FeetInches + "'");
                                }
                            }
                            else
                            {
                                Structuremodelalter.MaxHeightMetres = StructuremodelBase.MaxHeightMetres;
                            }
                        }
                        if (!string.IsNullOrEmpty(StructuremodelBase.Width) || !string.IsNullOrEmpty(Convert.ToString(Structuremodelalter.Width).Replace("''", "\"")))
                        {
                            if (Convert.ToString(StructuremodelBase.Width == null ? string.Empty : StructuremodelBase.Width).Trim() != Convert.ToString(Structuremodelalter.Width == null ? string.Empty : Structuremodelalter.Width).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MAX_WIDTH_MTRS", "Width" + PreSpecified + (StructuremodelBase.Width == string.Empty ? unspecified : StructuremodelBase.Width) + closeSpecified + SetTo + (Structuremodelalter.Width == string.Empty ? unspecified : Structuremodelalter.Width) + Meters + "'");
                                }
                                else
                                {
                                    changeList.Add("MAX_WIDTH_MTRS", "Width" + PreSpecified + (StructuremodelBase.Width == string.Empty ? unspecified : StructuremodelBase.Width) + closeSpecified + SetTo + (Structuremodelalter.Width == string.Empty ? unspecified : Structuremodelalter.Width) + FeetInches + "'");
                                }
                            }
                            else
                            {
                                Structuremodelalter.MaxWidthMetres = StructuremodelBase.MaxWidthMetres;
                            }
                        }

                        if (!string.IsNullOrEmpty(StructuremodelBase.Length) || !string.IsNullOrEmpty(Convert.ToString(Structuremodelalter.Length).Replace("''", "\"")))
                        {
                            if (Convert.ToString(StructuremodelBase.Length == null ? string.Empty : StructuremodelBase.Length).Trim() != Convert.ToString(Structuremodelalter.Length == null ? string.Empty : Structuremodelalter.Length).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MAX_LEN_MTRS", "Length" + PreSpecified + (StructuremodelBase.Length == string.Empty ? unspecified : StructuremodelBase.Length) + closeSpecified + SetTo + (Structuremodelalter.Length == string.Empty ? unspecified : Structuremodelalter.Length) + Meters + "'");
                                }
                                else
                                {
                                    changeList.Add("MAX_LEN_MTRS", "Length" + PreSpecified + (StructuremodelBase.Length == string.Empty ? unspecified : StructuremodelBase.Length) + closeSpecified + SetTo + (Structuremodelalter.Length == string.Empty ? unspecified : Structuremodelalter.Length) + FeetInches + "'");
                                }
                            }
                            else
                            {
                                Structuremodelalter.MaxLengthMetres = StructuremodelBase.MaxLengthMetres;
                            }
                        }
                        var maxGrossWeightNew = Structuremodelalter.GrossWeight;
                        if (StructuremodelBase.GrossWeight != 0 || maxGrossWeightNew != 0)
                        {                            
                            if (StructuremodelBase.GrossWeight != maxGrossWeightNew)
                            {
                                changeList.Add("MAX_GROSS_WEIGHT_KGS", "Gross weight" + PreSpecified + Convert.ToString(StructuremodelBase.GrossWeight == 0 ? unspecified : Convert.ToString(StructuremodelBase.GrossWeight)) + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.GrossWeight == 0 ? unspecified : Convert.ToString(Structuremodelalter.GrossWeight))) + Tonnes + "'");
                            }
                        }
                        var maxAxleWeightNew = Structuremodelalter.AxleWeight;
                        if (StructuremodelBase.AxleWeight != 0 || maxAxleWeightNew != 0)
                        {
                            if (StructuremodelBase.AxleWeight != maxAxleWeightNew)
                            {
                                changeList.Add("MAX_AXLE_WEIGHT_KGS", "Axle weight" + PreSpecified + Convert.ToString(StructuremodelBase.AxleWeight == 0 ? unspecified : Convert.ToString(StructuremodelBase.AxleWeight)) + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.AxleWeight == 0 ? unspecified : Convert.ToString(Structuremodelalter.AxleWeight))) + Tonnes + "'");
                            }
                        }
                        if (!string.IsNullOrEmpty(StructuremodelBase.Speed) || !string.IsNullOrEmpty(Structuremodelalter.Speed))
                        {
                            if (Convert.ToString(StructuremodelBase.Speed == null ? string.Empty : StructuremodelBase.Speed).Trim() != Convert.ToString(Structuremodelalter.Speed == null ? string.Empty : Structuremodelalter.Speed).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MIN_SPEED_KPH", "Speed" + PreSpecified + (StructuremodelBase.Speed == string.Empty ? unspecified : StructuremodelBase.Speed) + closeSpecified + SetTo + (Structuremodelalter.Speed == string.Empty ? unspecified : Structuremodelalter.Speed) + " kph" + "'");
                                }
                                else
                                    changeList.Add("MIN_SPEED_KPH", "Speed" + PreSpecified + (StructuremodelBase.Speed == string.Empty ? unspecified : StructuremodelBase.Speed) + closeSpecified + SetTo + (Structuremodelalter.Speed == string.Empty ? unspecified : Structuremodelalter.Speed) + " mph" + "'");
                            }
                        }

                        if (StructuremodelBase.CreatorIsContact != Structuremodelalter.CreatorIsContact)
                        {
                            changeList.Add("CREATOR_IS_CONTACT", "Creator is contact" + PreSpecified + (Convert.ToString(StructuremodelBase.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Structuremodelalter.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                    }

                    if (StructuremodelBase.CautionId != 0)
                    {
                        ViewBag.mode = "edit";
                    }
                    else
                    {
                        ViewBag.mode = "add";
                    }
                    ViewBag.changeListCount = changeList.Count;
                    ViewBag.changeList = changeList;
                    TempData.Keep("Structuremodelalter");
                    TempData.Keep("StructuremodelBase");

                    ViewBag.StructureID = structureId;
                    ViewBag.SectionID = sectionId;

                    if (changeList.Count > 0)
                    {
                        TempData["changeList"] = changeList;
                        TempData.Keep("changeList");
                    }
                    return View();
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/CautionAddReport, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/CautionAddReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region StoreDimensionData
        [HttpPost]
        public JsonResult StoreDimensionData(DimensionConstruction objStrucDim)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }
            #endregion
            TempData["DimensionConstruction"] = objStrucDim;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region StoreMostOnerousFilterData
        [HttpPost]
        public JsonResult StoreMostOnerousFilterData(StructureOnerousSearch SOS)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }
            #endregion
            TempData["SOS"] = SOS;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult ICAVehicleData(ICAVehicleModel objICAVehicleModel)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }
            #endregion
            TempData["ICAVehicleModel"] = objICAVehicleModel;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #region StoreCautionData
        [HttpPost]
        public JsonResult StoreCautionData(StructureModel Structuremodelalter)
        {
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }

            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion
            if (Structuremodelalter.SelectedTypeName == "SpecificAction")
            {
                Structuremodelalter.SelectedType = ActionType.SpecificAction;
                Structuremodelalter.SelectedTypeName = "SpecificAction";

            }
            else
            {
                Structuremodelalter.SelectedType = ActionType.StandardCaution;
                Structuremodelalter.SelectedTypeName = "StandardCaution";
            }
            if (SessionInfo.VehicleUnits == 692001)//Metric
            {
                if (!string.IsNullOrEmpty(Structuremodelalter.Height))
                {
                    Structuremodelalter.MaxHeightMetres = (float)Convert.ToDouble(Structuremodelalter.Height);
                }
                else
                {
                    Structuremodelalter.Height = string.Empty;
                }

                if (!string.IsNullOrEmpty(Structuremodelalter.Width))
                {
                    Structuremodelalter.MaxWidthMetres = (float)Convert.ToDouble(Structuremodelalter.Width);
                }
                else
                {
                    Structuremodelalter.Width = string.Empty;
                }

                if (!string.IsNullOrEmpty(Structuremodelalter.Length))
                {
                    Structuremodelalter.MaxLengthMetres = Structuremodelalter.MaxLengthMetres = (float)Convert.ToDouble(Structuremodelalter.Length);
                }
                else
                {
                    Structuremodelalter.Length = string.Empty;
                }
                if (!string.IsNullOrEmpty(Structuremodelalter.Speed))
                {
                    Structuremodelalter.MinSpeedKmph = (float)Math.Round(Convert.ToDouble(Structuremodelalter.Speed) / (float)0.6213, 2);
                }
                else
                {
                    Structuremodelalter.Speed = string.Empty;
                }
            }
            else
            {//imperial(692002)
                string[] splitFeet;
                float foot;
                float inches;

                if (!string.IsNullOrEmpty(Structuremodelalter.Height))
                {
                    splitFeet = Structuremodelalter.Height.Replace("\"", "''").Split('\'');
                    if (splitFeet.Length > 0 && splitFeet[0] != null)
                    {
                        foot = (float)(Convert.ToDouble(splitFeet[0]));
                        inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                        Structuremodelalter.MaxHeight = (float)Math.Round(foot + inches, 2);
                        Structuremodelalter.MaxHeightMetres = (Structuremodelalter.MaxHeight != 0) ? (float)Math.Round(Structuremodelalter.MaxHeight * (float)0.3048, 2) : 0;
                    }
                }
                else
                {
                    Structuremodelalter.Height = string.Empty;
                }
                if (!string.IsNullOrEmpty(Structuremodelalter.Width))
                {
                    splitFeet = Structuremodelalter.Width.Replace("\"", "''").Split('\'');
                    if (splitFeet.Length > 0 && splitFeet[0] != null)
                    {
                        foot = (float)(Convert.ToDouble(splitFeet[0]));
                        inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                        Structuremodelalter.MaxWidth = (float)Math.Round(foot + inches, 2);
                        Structuremodelalter.MaxWidthMetres = (Structuremodelalter.MaxWidth != 0) ? (float)Math.Round(Structuremodelalter.MaxWidth * (float)0.3048, 2) : 0;
                    }
                }
                else
                {
                    Structuremodelalter.Width = string.Empty;
                }
                if (!string.IsNullOrEmpty(Structuremodelalter.Length))
                {
                    splitFeet = Structuremodelalter.Length.Replace("\"", "''").Split('\'');
                    if (splitFeet.Length > 0 && splitFeet[0] != null)
                    {
                        foot = (float)(Convert.ToDouble(splitFeet[0]));
                        inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                        Structuremodelalter.MaxLength = (float)Math.Round(foot + inches, 2);
                        Structuremodelalter.MaxLengthMetres = Structuremodelalter.MaxLengthMetres = (Structuremodelalter.MaxLength != 0) ? (float)Math.Round(Structuremodelalter.MaxLength * (float)0.3048, 2) : 0;
                    }
                }
                else
                {
                    Structuremodelalter.Length = string.Empty;
                }

                if (!string.IsNullOrEmpty(Structuremodelalter.Speed))
                {
                    Structuremodelalter.MinSpeedKmph = (float)Math.Round(Convert.ToDouble(Structuremodelalter.Speed) / (float)0.6213, 2);
                }
                else
                {
                    Structuremodelalter.Speed = string.Empty;
                }

                Structuremodelalter.OrganisationId = SessionInfo.OrganisationId;
                Structuremodelalter.OrganizationName = SessionInfo.OrganisationName;
            }
            // Converting tonnes to KGS .....
            Structuremodelalter.MaxGrossWeightKgs = (decimal)Structuremodelalter.GrossWeight * 1000;
            Structuremodelalter.MaxGrossWeight = Structuremodelalter.GrossWeight;
            Structuremodelalter.MaxAxleWeightKgs = (decimal)Structuremodelalter.AxleWeight * 1000;
            Structuremodelalter.MaxAxleWeight = Structuremodelalter.AxleWeight;

            TempData["Structuremodelalter"] = Structuremodelalter;
            TempData.Keep("Structuremodelalter");
            TempData.Keep("StructuremodelBase");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        [HttpPost]
        public JsonResult SaveCautions(StructureModel structModel)
        {
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion
            StructureModel structureModelAlter = new StructureModel();
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["Structuremodelalter"])))
            {
                structureModelAlter = (StructureModel)TempData["Structuremodelalter"];
            }
            if (structureModelAlter.CautionId != 0)
            {
                ViewBag.mode = "edit";
                structureModelAlter.Mode = "edit";
            }
            else
            {
                ViewBag.mode = "add";
                structureModelAlter.Mode = "add";
            }

            string specificationStart = "<?xml version='1.0' encoding='UTF-8'?><caution:SpecificAction xmlns:caution='http://www.esdal.com/schemas/core/caution'>".Replace("'", "\"");
            string specificationEnd = "</caution:SpecificAction>";
            string otherSpecification = "<?xml version='1.0' encoding='UTF-8'?><SpecificAction xmlns='http://www.esdal.com/schemas/core/caution'>".Replace("'", "\"");
            string otherSpecificationEnd = "</SpecificAction>";
            string boldSpecStart = "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>".Replace("'", "\"");
            string boldSpecEnd = "</Bold>";

            string italicSpecStart = "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>".Replace("'", "\"");
            string italicSpecEnd = "</Italic>";

            string underlineSpecStart = "<Underline xmlns='http://www.esdal.com/schemas/core/formattedtext'>".Replace("'", "\"");
            string underlineSpecEnd = "</Underline>";

            if (structureModelAlter.SelectedType == ActionType.StandardCaution)//#2313
            {
                structureModelAlter.SpecificAction = specificationStart + structureModelAlter.DirectionName + specificationEnd;
            }
            else
            {
                if (!structureModelAlter.Bold && !structureModelAlter.Italic && !structureModelAlter.Underline)
                {
                    structureModelAlter.SpecificAction = otherSpecification + structureModelAlter.DirectionName + otherSpecificationEnd;
                }
                else
                {
                    StringBuilder xmlAttribute = new StringBuilder();
                    xmlAttribute.Append(otherSpecification);
                    if (structureModelAlter.Bold)
                    {
                        xmlAttribute.Append(boldSpecStart);
                    }
                    if (structureModelAlter.Italic)
                    {
                        xmlAttribute.Append(italicSpecStart);
                    }
                    if (structureModelAlter.Underline)
                    {
                        xmlAttribute.Append(underlineSpecStart);
                    }
                    xmlAttribute.Append(structureModelAlter.DirectionName);
                    if (structureModelAlter.Underline)
                    {
                        xmlAttribute.Append(underlineSpecEnd);
                    }
                    if (structureModelAlter.Italic)
                    {
                        xmlAttribute.Append(italicSpecEnd);
                    }
                    if (structureModelAlter.Bold)
                    {
                        xmlAttribute.Append(boldSpecEnd);
                    }
                    xmlAttribute.Append(otherSpecificationEnd);
                    structureModelAlter.SpecificAction = xmlAttribute.ToString();
                }
            }

            if (structureModelAlter.CreatorIsContact)
            {
                structureModelAlter.OwnerIsContact = Convert.ToInt16(SessionInfo.UserId);
            }



            structureModelAlter.MaxGrossWeightUnit = 240005;
            structureModelAlter.MaxAxleWeightUnit = 240005;

            structureModelAlter.MaxAxleWeightUnit = 208001;
            structureModelAlter.MaxWidthUnit = 208001;
            structureModelAlter.MaxLengthUnit = 208001;
            structureModelAlter.MinSpeedUnit = 229001;
            structureModelAlter.MaxHeightUnit = 208001;

            if (SessionInfo.VehicleUnits == 692002)
            {
                if (!string.IsNullOrEmpty(structureModelAlter.Height))
                {
                    if (structureModelAlter.MaxHeight == 0)
                        structureModelAlter.MaxHeight = (structureModelAlter.MaxHeightMetres != 0) ? (float)Math.Round(structureModelAlter.MaxHeightMetres / (float)0.3048, 2) : structureModelAlter.MaxHeightMetres;
                }
                else
                {
                    structureModelAlter.Height = string.Empty;// if null then set it to empty string.
                }

                if (!string.IsNullOrEmpty(structureModelAlter.Width))
                {
                    if (structureModelAlter.MaxWidth == 0)
                        structureModelAlter.MaxWidth = (structureModelAlter.MaxWidthMetres != 0) ? (float)Math.Round(structureModelAlter.MaxWidthMetres / (float)0.3048, 2) : structureModelAlter.MaxWidthMetres;
                }
                else
                {
                    structureModelAlter.Width = string.Empty;// if null then set it to empty string.
                }
                if (!string.IsNullOrEmpty(structureModelAlter.Length))
                {
                    if (structureModelAlter.MaxLength == 0)
                        structureModelAlter.MaxLength = (structureModelAlter.MaxLengthMetres != 0) ? (float)Math.Round(structureModelAlter.MaxLengthMetres / (float)0.3048, 2) : structureModelAlter.MaxLengthMetres;
                }
                else
                {
                    structureModelAlter.Length = string.Empty;// if null then set it to empty string.
                }
                if (!string.IsNullOrEmpty(structureModelAlter.Speed))
                {
                    if (structureModelAlter.MinSpeed == 0)
                        structureModelAlter.MinSpeed = (float)(Math.Round((structureModelAlter.MinSpeedKmph * (float)0.6213), 2));
                }
                else
                {
                    structureModelAlter.Speed = string.Empty;// if null then set it to empty string.
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(structureModelAlter.Height))
                {
                    if (structureModelAlter.MaxHeight == 0)
                    {
                        structureModelAlter.MaxHeightMetres = (float)(Convert.ToDouble(structureModelAlter.Height));
                        structureModelAlter.MaxHeight = (structureModelAlter.MaxHeightMetres != 0) ? (float)Math.Round(structureModelAlter.MaxHeightMetres / (float)0.3048, 2) : structureModelAlter.MaxHeightMetres;
                    }
                    else
                    {
                        structureModelAlter.MaxHeightMetres = (float)(Convert.ToDouble(structureModelAlter.Height));
                        structureModelAlter.MaxHeight = (structureModelAlter.MaxHeightMetres != 0) ? (float)Math.Round(structureModelAlter.MaxHeightMetres / (float)0.3048, 2) : structureModelAlter.MaxHeightMetres;
                    }


                }
                else
                {
                    structureModelAlter.Height = string.Empty;// if null then set it to empty string.
                }
                if (!string.IsNullOrEmpty(structureModelAlter.Width))
                {
                    if (structureModelAlter.MaxWidth == 0)
                    {
                        structureModelAlter.MaxWidthMetres = (float)(Convert.ToDouble(structureModelAlter.Width));
                        structureModelAlter.MaxWidth = (structureModelAlter.MaxWidthMetres != 0) ? (float)Math.Round(structureModelAlter.MaxWidthMetres / (float)0.3048, 2) : structureModelAlter.MaxWidthMetres;
                    }
                    else
                    {
                        structureModelAlter.MaxWidthMetres = (float)(Convert.ToDouble(structureModelAlter.Width));
                        structureModelAlter.MaxWidth = (structureModelAlter.MaxWidthMetres != 0) ? (float)Math.Round(structureModelAlter.MaxWidthMetres / (float)0.3048, 2) : structureModelAlter.MaxWidthMetres;
                    }
                }
                else
                {
                    structureModelAlter.Width = string.Empty;// if null then set it to empty string.
                }

                if (!string.IsNullOrEmpty(structureModelAlter.Length))
                {
                    if (structureModelAlter.MaxLength == 0)
                    {
                        structureModelAlter.MaxLengthMetres = (float)(Convert.ToDouble(structureModelAlter.Length));
                        structureModelAlter.MaxLength = (structureModelAlter.MaxLengthMetres != 0) ? (float)Math.Round(structureModelAlter.MaxLengthMetres / (float)0.3048, 2) : structureModelAlter.MaxLengthMetres;
                    }
                    else
                    {
                        structureModelAlter.MaxLengthMetres = (float)(Convert.ToDouble(structureModelAlter.Length));
                        structureModelAlter.MaxLength = (structureModelAlter.MaxLengthMetres != 0) ? (float)Math.Round(structureModelAlter.MaxLengthMetres / (float)0.3048, 2) : structureModelAlter.MaxLengthMetres;
                    }
                }
                else
                {
                    structureModelAlter.Length = string.Empty;// if null then set it to empty string.
                }

                if (!string.IsNullOrEmpty(structureModelAlter.Speed))
                {
                    if (structureModelAlter.MinSpeed == 0)
                    {
                        structureModelAlter.MinSpeedKmph = (float)(Convert.ToDouble(structureModelAlter.Speed));
                        structureModelAlter.MinSpeed = (structureModelAlter.MinSpeedKmph != 0) ? (float)Math.Round(structureModelAlter.MinSpeedKmph * (float)0.6213, 2) : structureModelAlter.MinSpeedKmph; //RM#3969 change
                    }
                    else
                    {
                        structureModelAlter.MinSpeedKmph = (float)(Convert.ToDouble(structureModelAlter.Speed));

                    }
                }
                else
                {
                    structureModelAlter.Length = string.Empty;// if null then set it to empty string.
                }

            }

            //---------------------------------------------------------------------------------
            structureModelAlter.OrganisationId = SessionInfo.OrganisationId;
            bool result = structuresService.SaveCautions(structureModelAlter);
            TempData.Remove("StructuremodelBase");
            #region STRUCTURE_LOGS
            var hashChangeList = (dynamic)TempData["changeList"];
            List<StructureLogModel> structureLogsModel = new List<StructureLogModel>();
            StructureLogModel structureLogModel;
            StructureModel struModelalter = (StructureModel)TempData["Structuremodelalter"];

            foreach (System.Collections.Generic.KeyValuePair<string, string> hashVal in hashChangeList)
            {
                structureLogModel = new StructureLogModel();

                structureLogModel.Author = SessionInfo.UserName;
                structureLogModel.Amendment1 = "\"" + struModelalter.StructureName + "\"(" + struModelalter.StructureId + ") Caution:\"" + struModelalter.CautionName + "\"(" + struModelalter.CautionId + ") - " + hashVal.Value;
                structureLogModel.Amendment2 = string.Empty;
                structureLogModel.Amendment3 = string.Empty;
                structureLogModel.StructureId = struModelalter.StructureId;
                structureLogModel.StructureCode = struModelalter.StructureCode;
                structureLogsModel.Add(structureLogModel);
            }

            if (struModelalter.Mode == "add")
            {
                structureLogModel = new StructureLogModel();

                structureLogModel.Author = SessionInfo.UserName;
                structureLogModel.Amendment1 = "\"" + struModelalter.StructureName + "\"(" + struModelalter.StructureId + ") Caution:\"" + struModelalter.CautionName + "\"(" + struModelalter.CautionId + ") - has been added";
                structureLogModel.Amendment2 = string.Empty;
                structureLogModel.Amendment3 = string.Empty;
                structureLogModel.StructureId = struModelalter.StructureId;
                structureLogModel.StructureCode = struModelalter.StructureCode;
                structureLogsModel.Add(structureLogModel);
            }

            structuresService.UpdateStructureLog(structureLogsModel);

            #endregion
            TempData.Remove("Structuremodelalter");
            TempData.Remove("changeList");
            TempData.Remove("StructureCode");
            TempData.Remove("StructureName");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DeleteCaution
        [HttpPost]
        public JsonResult DeleteCaution(long cautionId)
        {
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json("expire", JsonRequestBehavior.AllowGet);
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion

            structuresService.DeleteCaution(cautionId, SessionInfo.UserName);
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region public ActionResult ICAVehicle(int structureId = 0, int sectionId = 0)
        public ActionResult ICAVehicle(string structName = "", string ESRN = "", int structureId = 0, int sectionId = 0, string ICAResult = "", int MovementClassConfig = 0, int ConfigType = 0, int CompNum = 0, int? TractorComponentType = 0, int? TrailerComponentType = 0, ICAVehicleModel  objICAVehicleModel=null)
        {

            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var SessionInfo = (UserInfo)Session["UserInfo"];

                #region For document status viewer
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                ViewBag.MovementClassConfig = new SelectList(GetVehicleClassificationDropDown(), "Id", "Value", ViewBag.MovementType);
                ViewBag.ConfigType = new SelectList(GetAllVehicleType(), "Id", "Value", ViewBag.VhclConfigType);
                ViewBag.TractorComponentType = new SelectList(GetTractorComponentType(), "Id", "Value", ViewBag.TractorType);
                ViewBag.TrailerComponentType = new SelectList(GetTrailerComponentType(), "Id", "Value", ViewBag.TrailerType);
                ViewBag.CompNum = new SelectList(GetComponentNumber(), "Id", "Value", ViewBag.ComponentNum);

                ViewBag.structName = structName;
                ViewBag.ESRN = ESRN;

                ViewBag.structureId = structureId;
                ViewBag.sectionId = sectionId;

                ViewBag.ICAResult = ICAResult;
                int chkValidStructCnt = 0;
                int organisationId = (int)SessionInfo.OrganisationId;


                chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(Convert.ToInt64(structureId),organisationId);

                int orgCheck = structuresService.CheckStructureOrganisation(organisationId, Convert.ToInt64(structureId));
                // to check the sturcutre belongs to same organisation id 8389 related fix                                                                                                            
                //if(orgCheck == 0)
                // return RedirectToAction("Error", "Home"); 
                //}

                objICAVehicleModel = (ICAVehicleModel)TempData["ICAVehicleModel"];
                TempData["TmpchkValidStructCnt"] = 0;
                if (chkValidStructCnt > 0)
                {
                    TempData["TmpchkValidStructCnt"] = chkValidStructCnt;
                }

                //ICAVehicleModel objICAVehicleModel = (ICAVehicleModel)TempData["objICAVehicleModel"];
                //objICAVehicleModel = ViewBag.objICAVehicleModel ;
                return View(objICAVehicleModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region private void GetStructTypeDropDown(string type)
        private void GetStructTypeDropDown(string type)
        {
            List<DropDown> dropdownObjList = new List<DropDown>();
            DropDown dropdownObj = null;
            dropdownObj = new DropDown();
            dropdownObj.Id = 1;
            dropdownObj.Value = "Underbridge";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 2;
            dropdownObj.Value = "Overbridge";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 3;
            dropdownObj.Value = "Under and over bridge";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 4;
            dropdownObj.Value = "Level crossing";
            dropdownObjList.Add(dropdownObj);

            ViewBag.structType = new SelectList(dropdownObjList, "Value", "Value", type);
        }
        #endregion

        #region private static List<string> GetVehicleClassificationDropDown()
        private static List<DropDown> GetVehicleClassificationDropDown()
        {
            List<DropDown> listVhclType = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown()
            {
                Id = 270001,
                Value = "Construction and Use"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 270002,
                Value = "STGO AIL"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 270003,
                Value = "STGO Mobile Crane"
            };
            listVhclType.Add(objDropDown);


            objDropDown = new DropDown()
            {
                Id = 270004,
                Value = "STGO Engineering Plant"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 270005,
                Value = "STGO Road Recovery Operation"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 270006,
                Value = "Special Order"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 270007,
                Value = "Vehicle Special Order"
            };
            listVhclType.Add(objDropDown);

            return listVhclType;
        }
        #endregion

        #region private static List<string> GetAllVehicleType()
        private static List<DropDown> GetAllVehicleType()
        {
            List<DropDown> listVhclType = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown()
            {
                Id = 244002,
                Value = "Semi Trailer"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244001,
                Value = "Drawbar Trailer"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244003,
                Value = "Rigid"
            };
            listVhclType.Add(objDropDown);


            objDropDown = new DropDown()
            {
                Id = 244005,
                Value = "SPMT"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244006,
                Value = "Other Inline"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244007,
                Value = "Side by Side"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 244004,
                Value = "Tracked"
            };
            listVhclType.Add(objDropDown);

            return listVhclType;
        }
        #endregion

        #region private static List<string> GetComponentType()
        private static List<DropDown> GetTractorComponentType()
        {
            List<DropDown> listVhclType = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown()
            {
                Id = 234001,
                Value = "Ballast Tractor"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234002,
                Value = "Conventional Tractor"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234003,
                Value = "Rigid Vehicle"
            };
            listVhclType.Add(objDropDown);

            return listVhclType;
        }
        #endregion

        #region private static List<string> GetTrailerComponentType()
        private static List<DropDown> GetTrailerComponentType()
        {
            List<DropDown> listVhclType = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown()
            {
                Id = 234001,
                Value = "Ballast Tractor"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234002,
                Value = "Conventional Tractor"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234003,
                Value = "Rigid Vehicle"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234005,
                Value = "Semi Trailer"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234006,
                Value = "Drawbar Trailer"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234004,
                Value = "Tracked Vehicle"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 234007,
                Value = "SPMT"
            };
            listVhclType.Add(objDropDown);

            return listVhclType;
        }
        #endregion

        #region private static List<string> GetComponentType()
        private static List<DropDown> GetComponentNumber()
        {
            List<DropDown> listVhclType = new List<DropDown>();
            DropDown objDropDown = null;
            objDropDown = new DropDown()
            {
                Id = 1,
                Value = "1"
            };
            listVhclType.Add(objDropDown);

            objDropDown = new DropDown()
            {
                Id = 2,
                Value = "2"
            };
            listVhclType.Add(objDropDown);

            return listVhclType;
        }
        #endregion


        #region public ActionResult PerformICAVehicle(ICAVehicleModel objICAVehicleModel)
    
        public ActionResult PerformICAVehicle(string structName, string ESRN, ICAVehicleModel objICAVehicleModel, int MovementClassConfig, int ConfigType, int CompNum, int structureId, int sectionId, int? TractorComponentType, int? TrailerComponentType)
        {
            try
            {

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                int orgID = (int)SessionInfo.OrganisationId;
                if (!string.IsNullOrEmpty(Convert.ToString(TempData["ICAVehicleModel"])))
                {

                objICAVehicleModel = (ICAVehicleModel)TempData["ICAVehicleModel"];
                }
                ManageStructureICA objManageStructureICA = null;
                objManageStructureICA = structuresService.GetManageICAUsage(orgID, structureId, sectionId);
                ICAVehicleResult ICAVehicleParams = new ICAVehicleResult()
                {
                    ObjICAVehicleModel = objICAVehicleModel,
                    ObjManageStructureICA = objManageStructureICA,
                    MovementClassConfig = MovementClassConfig,
                    ConfigType = ConfigType,
                    CompNum = CompNum,
                    TractorType = TractorComponentType,
                    TrailerType = TrailerComponentType,
                    StructureId = structureId,
                    SectionId = sectionId,
                    OrgId = orgID
                };
                string result = structuresService.GetICAVehicleResult(ICAVehicleParams);
                TempData["ICAVehicleModel"] = objICAVehicleModel;
                //  return View(result);
                return RedirectToAction("ICAVehicle", "Structures", new
                {
                    B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("structName=" + structName +
                        "&ESRN=" + ESRN +
                        "&structureId=" + structureId +
                        "&sectionId=" + sectionId +
                        "&ICAResult=" + result +
                        "&MovementClassConfig=" + MovementClassConfig +
                        "&ConfigType=" + ConfigType +
                        "&CompNum=" + CompNum +
                        "&TractorComponentType=" + TractorComponentType +
                        "&TrailerComponentType=" + TrailerComponentType +
                        "&objICAVehicleModel=" + objICAVehicleModel)
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ViewCaution
        public ActionResult ViewCaution(long cautionId, long structureId, long sectionId)
        {


            UserInfo SessionInfo = null;

            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            SessionInfo = (UserInfo)Session["UserInfo"];

            int UOM = SessionInfo.VehicleUnits;

            ViewBag.UOM = UOM;

            StructureModel CM = structuresService.GetCautionDetails(cautionId);

            ViewBag.StructureID = structureId;
            ViewBag.SectionID = sectionId;

            XmlDocument Doc = new XmlDocument();

            // Date 13 Feb 2015 Ticket No 3706
            if (!string.IsNullOrEmpty(CM.SpecificAction))
            {
                Doc.PreserveWhitespace = true;
                Doc.LoadXml(CM.SpecificAction);
                XmlNodeList parentNode = Doc.GetElementsByTagName("SpecificAction");
                if (parentNode.Count > 0)
                {
                    string text = parentNode[0].InnerXml.Replace("\"_\"", "&nbsp;");
                    text = parentNode[0].InnerXml.Replace("_", "&nbsp;");
                    CM.SpecificAction = text;
                    CM.SpecificActionB = false;
                }
                else
                {
                    XmlNodeList parentNodebold = Doc.GetElementsByTagName("caution:SpecificAction");
                    CM.SpecificAction = parentNodebold[0].InnerText;
                    CM.SpecificActionB = true;
                }
            }

            if (UOM == 692002)
            {
                CM.MaxHeightFeet = CM.MaxHeight;
                CM.MaxWidthFeet = CM.MaxWidth;
                CM.MaxLengthFeet = CM.MaxLength;
                CM.MinSpeedKmph = (float)CM.MinSpeedKmph;
                CM.MinSpeed = (float)CM.MinSpeed;
            }
            // converting kgs to tonnes
            CM.GrossWeight = (double)((CM.MaxGrossWeightKgs != 0) ? CM.MaxGrossWeightKgs / 1000 : 0);
            CM.AxleWeight = (double)((CM.MaxAxleWeightKgs != 0) ? CM.MaxAxleWeightKgs / 1000 : 0);

            TempData.Keep("OrganisationName");
            TempData.Keep("OWNER_ORG_ID");
            return View(CM);
        }
        #endregion
        #region ViewStructureContactsList
        public ActionResult ViewStructureContactsList(int? page, int? pageSize, long structureId, long cautionId, long sectionId)
        {

            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
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

            ViewBag.CautionID = cautionId;
            ViewBag.StructureID = structureId;
            ViewBag.SectionID = sectionId;

            List<StructureContactModel> structureList = structuresService.GetStructureContactList(pageNumber, (int)pageSize, cautionId);
            
            if(structureList.Count > 0)
            {
                var itemToRemove = structureList.SingleOrDefault(r => r.OrganisationName == "");
                if(itemToRemove != null)
                {
                    structureList.Remove(itemToRemove);
                }                
            }
            if (structureList.Count > 0)
            {
                ViewBag.TotalCount = Convert.ToInt32(structureList[0].TotalRecordCount);
                IPagedList<StructureContactModel> model = structureList.ToPagedList(page ?? 1, 10);
                page = model.PageCount;
            }
            else
            {
                ViewBag.TotalCount = 0;
            }
            var structureContactPageList = new StaticPagedList<StructureContactModel>(structureList, pageNumber, (int)pageSize, ViewBag.TotalCount);
            return View(structureContactPageList);
        }
        #endregion
        #region ManageStructureContact
        public ActionResult ManageStructureContact(long structureId, long cautionId, short contactNo, string mode, long sectionId)
        {
            try
            {
                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion
                #region Check Page Request

                #endregion

                ViewBag.CautionID = cautionId;
                ViewBag.StructureID = structureId;
                ViewBag.SectionID = sectionId;

                #region Fill Country Dropdown
                //Filling Country List Drop Down From XML
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Configuration.xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);

                XmlNode Association = xmlDocument.SelectSingleNode("/Configurable/association");
                XmlNodeList membership = Association.SelectNodes("membership");

                //Filling Country List Drop Down From database
                var CountryList = userService.GetCountryInfo();
                SelectList CountrySelectList = new SelectList(CountryList, "CountryID", "Country");
                ViewBag.CountryDropDown = CountrySelectList;
                #endregion

                if (mode == "add")
                {
                    StructureContactModel structureContactModel = new StructureContactModel();
                    structureContactModel.CautionId = cautionId;
                    structureContactModel.ContactNo = contactNo;
                    return View(structureContactModel);
                }
                else
                {
                    List<StructureContactModel> structureContactList = structuresService.GetStructureContactList(1, 1, cautionId, contactNo);
                    if (structureContactList.Count > 0)
                    {
                        structureContactList[0].CountryName = CountrySelectList.Where(r => r.Value.Equals(Convert.ToString(structureContactList[0].CountryId))).Select(r => r.Text).SingleOrDefault();
                        return View(structureContactList[0]);
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("CreateCaution/ManageStructureContact, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region SaveStructureContact
        [HttpPost]
        public JsonResult SaveStructureContact(StructureContactModel structureContact)
        {
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
                structureContact.ContactId = Convert.ToInt64(SessionInfo.UserId);
                structureContact.RoleType = SessionInfo.UserTypeId;
                structureContact.OrganisationId = SessionInfo.OrganisationId;

                structuresService.SaveStructureContact(structureContact);


                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            #endregion


        }
        #endregion
        #region public ActionResult StructureDimensions()
        public ActionResult StructureDimensions(int structureId = 0, int sectionId = 0, string structureNm = "", string ESRN = "", long strucType = 3)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var SessionInfo = (UserInfo)Session["UserInfo"];

            #region For document status viewer
            if (SessionInfo.HelpdeskRedirect == "true")
            {
                ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
            }
            #endregion

            int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(structureId, SessionInfo.OrganisationId);
            int orgCheck = structuresService.CheckStructureOrganisation((int)SessionInfo.OrganisationId, (long)structureId); // to check the sturcutre belongs to same organisation id 8389 related fix
            if (orgCheck == 0)
            {
                return RedirectToAction("Error", "Home");
            }
            DimensionConstruction objDimentionConstruction = null;
            if (chkValidStructCnt > 0)
            {
                objDimentionConstruction = structuresService.ViewDimensionConstruction(structureId, sectionId);
                List<SpanData> objListSpanData = structuresService.ViewSpanData(structureId, sectionId);
                ViewBag.objListSpanData = objListSpanData;
                ViewBag.structureId = structureId;
                ViewBag.sectionId = sectionId;
                ViewBag.strucType = strucType;

                var objContruType = structuresService.GetSTRUCT_DD(232);
                SelectList objContruTypeList = new SelectList(objContruType, "NAME", "NAME");
                ViewBag.ConstructTypeList = objContruTypeList;

                var objDeckType = structuresService.GetSTRUCT_DD(211);
                SelectList objDeckTypeList = new SelectList(objDeckType, "NAME", "NAME");
                ViewBag.DeckMaterialList = objDeckTypeList;

                var objBearingType = structuresService.GetSTRUCT_DD(206);
                SelectList objBearingTypeList = new SelectList(objBearingType, "NAME", "NAME");
                ViewBag.Bearings1List = objBearingTypeList;

                var objFoundationType = structuresService.GetSTRUCT_DD(227);
                SelectList objFoundationTypeList = new SelectList(objFoundationType, "NAME", "NAME");
                ViewBag.FoundationsList = objFoundationTypeList;

                string ConstType1 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.ConstructionType1))
                {
                    ConstType1 = (from s in objContruTypeList
                                  where s.Value.Trim().ToLower() == objDimentionConstruction.ConstructionType1.Trim().ToLower()
                                  select s.Value.ToString()).FirstOrDefault();
                }
                if (ConstType1 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtConstructionType1))
                {
                    objDimentionConstruction.ConstructionType1 = "Other";
                }

                string ConstType2 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.ConstructionType2))
                {
                    ConstType2 = (from s in objContruTypeList
                                  where s.Value.Trim().ToLower() == objDimentionConstruction.ConstructionType2.Trim().ToLower()
                                  select s.Value.ToString()).FirstOrDefault();

                }
                if (ConstType2 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtConstructionType2))
                {
                    objDimentionConstruction.ConstructionType2 = "Other";
                }

                string ConstType3 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.ConstructionType3))
                {
                    ConstType3 = (from s in objContruTypeList
                                  where s.Value.Trim().ToLower() == objDimentionConstruction.ConstructionType3.Trim().ToLower()
                                  select s.Value.ToString()).FirstOrDefault();

                }
                if (ConstType3 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtConstructionType3))
                {
                    objDimentionConstruction.ConstructionType3 = "Other";
                }

                string DeckMaterial1 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.DeckMaterial1))
                {
                    DeckMaterial1 = (from s in objDeckTypeList
                                     where s.Value.Trim().ToLower() == objDimentionConstruction.DeckMaterial1.Trim().ToLower()
                                     select s.Value.ToString()).FirstOrDefault();
                }
                if (DeckMaterial1 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtDeckMaterial1))
                {
                    objDimentionConstruction.DeckMaterial1 = "Other";
                }

                string DeckMaterial2 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.DeckMaterial1))
                {
                    DeckMaterial2 = (from s in objDeckTypeList
                                     where s.Value.Trim().ToLower() == objDimentionConstruction.DeckMaterial2.Trim().ToLower()
                                     select s.Value.ToString()).FirstOrDefault();

                }
                if (DeckMaterial2 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtDeckMaterial2))
                {
                    objDimentionConstruction.DeckMaterial2 = "Other";
                }

                string DeckMaterial3 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.DeckMaterial3))
                {
                    DeckMaterial3 = (from s in objDeckTypeList
                                     where s.Value.Trim().ToLower() == objDimentionConstruction.DeckMaterial3.Trim().ToLower()
                                     select s.Value.ToString()).FirstOrDefault();

                }
                if (DeckMaterial3 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtDeckMaterial3))
                {
                    objDimentionConstruction.DeckMaterial3 = "Other";
                }

                string BearingType1 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.BearingsType1))
                {
                    BearingType1 = (from s in objBearingTypeList
                                    where s.Value.Trim().ToLower() == objDimentionConstruction.BearingsType1.Trim().ToLower()
                                    select s.Value.ToString()).FirstOrDefault();
                }
                if (BearingType1 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtBearingsType1))
                {
                    objDimentionConstruction.BearingsType1 = "Other";
                }

                string BearingType2 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.BearingsType1))
                {
                    BearingType2 = (from s in objBearingTypeList
                                    where s.Value.Trim().ToLower() == objDimentionConstruction.BearingsType2.Trim().ToLower()
                                    select s.Value.ToString()).FirstOrDefault();

                }
                if (BearingType2 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtBearingsType2))
                {
                    objDimentionConstruction.BearingsType2 = "Other";
                }

                string BearingType3 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.BearingsType3))
                {
                    BearingType3 = (from s in objBearingTypeList
                                    where s.Value.Trim().ToLower() == objDimentionConstruction.BearingsType3.Trim().ToLower()
                                    select s.Value.ToString()).FirstOrDefault();
                }
                if (BearingType3 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtBearingsType3))
                {
                    objDimentionConstruction.BearingsType3 = "Other";
                }

                string FoundationType1 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.FoundationType1))
                {
                    FoundationType1 = (from s in objFoundationTypeList
                                       where s.Value.Trim().ToLower() == objDimentionConstruction.FoundationType1.Trim().ToLower()
                                       select s.Value.ToString()).FirstOrDefault();
                }
                if (FoundationType1 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtFoundationType1))
                {
                    objDimentionConstruction.FoundationType1 = "Other";
                }

                string FoundationType2 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.FoundationType2))
                {
                    FoundationType2 = (from s in objFoundationTypeList
                                       where s.Value.Trim().ToLower() == objDimentionConstruction.FoundationType2.Trim().ToLower()
                                       select s.Value.ToString()).FirstOrDefault();

                }
                if (FoundationType2 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtFoundationType2))
                {
                    objDimentionConstruction.FoundationType2 = "Other";
                }

                string FoundationType3 = null;
                if (!string.IsNullOrEmpty(objDimentionConstruction.FoundationType3))
                {
                    FoundationType3 = (from s in objFoundationTypeList
                                       where s.Value.Trim().ToLower() == objDimentionConstruction.FoundationType3.Trim().ToLower()
                                       select s.Value.ToString()).FirstOrDefault();

                }
                if (FoundationType3 == null && !string.IsNullOrEmpty(objDimentionConstruction.TxtFoundationType3))
                {
                    objDimentionConstruction.FoundationType3 = "Other";
                }
                ViewBag.structName = structureNm;
                ViewBag.ESRN = ESRN;

                if (sectionId != null)
                {
                    Session["checkFlag"] = "true";
                    Session["sectionID"] = sectionId;
                }

                Session["structureNm"] = structureNm;
                Session["ESRN"] = ESRN;
                Session["strucType"] = strucType;
                ViewBag.saveFlag = TempData["saveFlag"];
                ViewBag.saveSpanFlag = TempData["saveSpanFlag"];
            }

            return View(objDimentionConstruction);
        }
        #endregion
        #region Update DIM AND CONSTRAINTS

        [HttpPost]
        public ActionResult EditDimCONSTRAINTS(DimensionConstruction objStrucDim, long structureId = 0, long sectionId = 0)
        {
            bool flag = false;
            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
            objStrucDim.UserName = ObjUserInfo.UserName;
            string a = Request.Form["SpanNumber"];
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["DimensionConstruction"])))
            {
                objStrucDim = (DimensionConstruction)TempData["DimensionConstruction"];
            }
            flag = structuresService.EditDimensionConstraints(objStrucDim, (int)structureId, (int)sectionId);
            TempData["saveFlag"] = flag;
            ViewBag.strucType = Session["strucType"];

            string structureNm = Convert.ToString(Session["structureNm"]);
            string ESRN = Convert.ToString(Session["ESRN"]);
            long strucType = Convert.ToInt64(Session["strucType"]);
            return Json(flag);
        }
        #endregion
        #region public ActionResult StructureSectionSpan()
        public ActionResult StructureSectionSpan(long structureId, long sectionId, long? spanNo, int editSaveFlag = 0, string structureName = "", string ESRN = "", long structureType = 3)
        {
            try
            {

                var objContruType = structuresService.GetSTRUCT_DD(232);
                ViewBag.ConstructTypeList = new SelectList(objContruType, "NAME", "NAME");

                objContruType = structuresService.GetSTRUCT_DD(211);
                ViewBag.DeckMaterialList = new SelectList(objContruType, "NAME", "NAME");

                objContruType = structuresService.GetSTRUCT_DD(206);
                ViewBag.Bearings1List = new SelectList(objContruType, "NAME", "NAME");

                objContruType = structuresService.GetSTRUCT_DD(227);
                ViewBag.FoundationsList = new SelectList(objContruType, "NAME", "NAME");

                objContruType = structuresService.GetSTRUCT_DD(204);
                var filteredList = objContruType.Where(item => item.Name != "level crossing").ToList();
                ViewBag.StructTypeList = new SelectList(filteredList, "NAME", "NAME");

                ViewBag.structureId = structureId;
                ViewBag.sectionId = sectionId;
                ViewBag.EditSaveFlag = editSaveFlag;
                ViewBag.structureName = structureName;
                ViewBag.ESRN = ESRN;
                ViewBag.structureType = structureType;

                SpanData objListSpanData = null;
                if (editSaveFlag == 1)
                {
                    objListSpanData = structuresService.ViewSpanDataByNo(structureId, sectionId, spanNo);
                }

                return PartialView(objListSpanData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region public ActionResult DeleteStructureSpan()
        public JsonResult DeleteStructureSpan(long structureId = 0, long sectionId = 0, long spanNo = 0)
        {
            int deleteFlag = 0;
            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];

            deleteFlag = structuresService.DeleteStructureSpan(structureId, sectionId, spanNo, ObjUserInfo.UserName);
            return Json(deleteFlag);
        }
        #endregion
        #region public ActionResult ValidateSpanPosition()
        public JsonResult ValidateSpanPosition(int structureId, int sectionId, int spanPosition = 0)
        {
            int deleteFlag = 0;
            List<SpanData> objListSpanData = structuresService.ViewSpanData(structureId, sectionId);
            foreach (var item in objListSpanData)
            {
                if (item.Position == spanPosition)
                {
                    deleteFlag = 1;
                }
            }
            return Json(new { result = deleteFlag });
        }
        #endregion
        #region StoreSpanData
        [HttpPost]
        public JsonResult StoreSpanData(SpanData objSpanData)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }
            #endregion
            TempData["SpanData"] = objSpanData;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region public ActionResult SaveStructSectionSpan()
        public ActionResult SaveStructSectionSpan(SpanData objSpanData, long structureId = 0, long sectionId = 0, int editSaveFlag = 0)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["SpanData"])))
            {
                objSpanData = (SpanData)TempData["SpanData"];
            }
            StructureSpanParams SpanParams = new StructureSpanParams
            {

                ObjSpanData = objSpanData,
                StructureId = structureId,
                SectionId = sectionId,
                EditSaveFlag = editSaveFlag
            };
            int saveFlag = 0;
            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
            objSpanData.UserName = ObjUserInfo.UserName;
            string a = Request.Form["SpanNumber"];
            saveFlag = structuresService.SaveStructureSpan(SpanParams);
            TempData["saveSpanFlag"] = saveFlag;
            ViewBag.strucType = Session["strucType"];
            ViewBag.EditSaveFlag = editSaveFlag;
            string structureNm = Convert.ToString(Session["structureNm"]);
            string ESRN = Convert.ToString(Session["ESRN"]);
            long strucType = Convert.ToInt64(Session["strucType"]);
            return Json(saveFlag);

        }
        #endregion
        #region private methods


        #region private void GetICAMethod()
        private void GetICAMethod(string type)
        {
            List<DropDown> dropdownObjList = new List<DropDown>();
            DropDown dropdownObj = null;
            dropdownObj = new DropDown();
            dropdownObj.Id = 1;
            dropdownObj.Value = "Weight Screening";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 2;
            dropdownObj.Value = "SV Screening";
            dropdownObjList.Add(dropdownObj);
            ViewBag.icaMethod = new SelectList(dropdownObjList, "Value", "Value", type);
        }
        #endregion

        #region private void GetICAMethod()
        private void GetStructureTypeDropDown(string type)
        {
            List<DropDown> dropdownObjList = new List<DropDown>();
            DropDown dropdownObj = null;
            dropdownObj = new DropDown();
            dropdownObj.Id = 1;
            dropdownObj.Value = "Underbridge";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 2;
            dropdownObj.Value = "Overbridge";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 3;
            dropdownObj.Value = "Under and over bridge";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 4;
            dropdownObj.Value = "Level crossing";
            dropdownObjList.Add(dropdownObj);

            ViewBag.structType = new SelectList(dropdownObjList, "Value", "Value", type);
        }
        #endregion

        #endregion

        #region public ActionResult ConfigureBandings()
        public ActionResult ConfigureBandings(int structureId, int sectionId, string structureName, string ESRN)
        {

            try
            {
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    Request.RequestContext.RouteData.GetRequiredString("action"); 
                    Request.RequestContext.RouteData.GetRequiredString("controller");

                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];

                #region For document status viewer
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                int organisationId = (int)SessionInfo.OrganisationId;
                
                int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(structureId, SessionInfo.OrganisationId);
                int orgCheck = structuresService.CheckStructureOrganisation(organisationId, (long)structureId); // to check the sturcutre belongs to same organisation id 8389 related fix
                if (orgCheck == 0)
                {
                    return RedirectToAction("Error", "Home");
                }

                ConfigBandModel objConfigBandModel = null;
                if (chkValidStructCnt > 0)
                {
                    ViewBag.organisationId = organisationId;
                    ViewBag.structureID = structureId;
                    ViewBag.sectionID = sectionId;
                    ViewBag.structName = structureName;
                    ViewBag.ESRN = ESRN;
                    objConfigBandModel = new ConfigBandModel();
                    objConfigBandModel = structuresService.GetDefaultBanding(organisationId, structureId, sectionId);
                    objConfigBandModel.IsDefaultWeight = true;
                    objConfigBandModel.IsDefaultSV = true;
                }
                return PartialView(objConfigBandModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateConfigureBandings
        [HttpPost]
        public ActionResult UpdateConfigureBandings(int OrgId, double? OrgMinWeight, double? OrgMaxWeight, double? OrgMinSV, double? OrgMaxSV)
        {
            string UserName = "";
            if (Session["UserInfo"] != null)
            {
                UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
                UserName = ObjUserInfo.UserName;
            }
            SaveDefaultConfigParams configparams = new SaveDefaultConfigParams()
            {
                OrgId = OrgId,
                OrgMinWeight = OrgMinWeight,
                OrgMaxWeight = OrgMaxWeight,
                OrgMinSV = OrgMinSV,
                OrgMaxSV = OrgMaxSV,
                UserName = UserName

            };

            structuresService.UpdateDefaultBanding(configparams);
            ViewBag.msg = "1";
            return Json(new { Result = true });


        }

        #endregion
        #region public ActionResult ManageICAUsage()
        public ActionResult ManageICAUsage(int structureID, int sectionID, string structName, string ESRN)
        {
            try
            {
                // GetStructType("");

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];

                #region For document status viewer
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                int orgID = (int)SessionInfo.OrganisationId;

                int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(structureID, orgID);

                int orgCheck = structuresService.CheckStructureOrganisation((int)SessionInfo.OrganisationId, (long)structureID); // to check the sturcutre belongs to same organisation id 8389 related fix
                if (orgCheck == 0)
                {
                    return RedirectToAction("Error", "Home");
                }

                ManageStructureICA objManageStructureICA = null;
                if (chkValidStructCnt > 0)
                {
                    objManageStructureICA = structuresService.GetManageICAUsage(orgID, structureID, sectionID);
                    ViewBag.StructureId = structureID;
                    ViewBag.SectionId = sectionID;
                    ViewBag.structName = structName;
                    ViewBag.ESRN = ESRN;

                    if (sectionID != null)
                    {
                        Session["checkFlag"] = "true";
                        Session["sectionID"] = sectionID;
                    }
                }
                return View(objManageStructureICA);
               // return Json(new { Success = objManageStructureICA });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region public ActionResult UpdateStructureICAUsage()
        [HttpPost]
        public JsonResult UpdateStructureICAUsage(double? weightScreeningLower, double? weightScreeningUpper, double? svScreeningLower, double? svScreeningUpper
            , int? enableWeightGross, int? enableWeightAxle, int? enableWeightAWR, int? enableWeightOverDistance
            , int? enableSV80, int? enableSV100, int? enableSV150, int? enableSVTrain, int structureID, int sectionID)
        {
            try
            {

                ManageStructureICA objManageStructureICA = new ManageStructureICA();
                objManageStructureICA.CustomWSBandLimitMin = weightScreeningLower;
                objManageStructureICA.CustomWSBandLimitMax = weightScreeningUpper;
                objManageStructureICA.CustomSVBandLimitMin = svScreeningLower;
                objManageStructureICA.CustomSVBandLimitMax = svScreeningUpper;
                objManageStructureICA.EnableGrossWeight = enableWeightGross;
                objManageStructureICA.EnableAxleWeight = enableWeightAxle;
                objManageStructureICA.EnableAWR = enableWeightAWR;
                objManageStructureICA.EnableWeightOverDist = enableWeightOverDistance;
                objManageStructureICA.EnableSV80 = enableSV80;
                objManageStructureICA.EnableSV100 = enableSV100;
                objManageStructureICA.EnableSV150 = enableSV150;
                objManageStructureICA.EnableSVTrain = enableSVTrain;

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                int organisationId = (int)SessionInfo.OrganisationId;
                objManageStructureICA.UserName = SessionInfo.UserName;
                UpdateICAUsageParams ICAUsageparams = new UpdateICAUsageParams
                {
                    ICAUsage = objManageStructureICA,
                    OrganisationId = organisationId,
                    StructureId = structureID,
                    SectionId = sectionID
                };
                bool result = structuresService.UpdateStructureICAUsage(ICAUsageparams);
                // return View();
                ViewBag.StructureId = structureID;
                ViewBag.SectionId = sectionID;
                return Json(new { Success = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// To display the result of finding onerous vehciles
        /// </summary>
        /// <param name="SOS">Model containing the values to be searched</param>
        /// <param name="page">Page being currently viewed</param>
        /// <param name="pageSize">No of records to be displayed</param>
        /// <param name="structureId">Structure ID to filter the records</param>
        /// <returns>Return the view containing the data</returns>
        public ActionResult StructureMostOnerousVehicleList(bool? PageStatus, int? page, int? pageSize, string structureId, string structureName, StructureOnerousSearch SOS)
        {
            try
            {

                #region Session Check

                UserInfo SessionInfo = null;
                int structureNotificationCount = 0;

                CultureInfo ukCulture = new CultureInfo("en-GB");

                DateTime? StartDate = null;
                DateTime? EndDate = null;
                DateTime dtValidate = new DateTime();
                string Status = null;
                int StatusCount = 0;
                int Sort = 1;
                if (!string.IsNullOrEmpty(Convert.ToString(TempData["SOS"])))
                {
                    SOS = (StructureOnerousSearch)TempData["SOS"];
                }
                const string rejectedStatus = "327002,327005,";
                const string notapplicableStatus = "327003,327006,";
                const string applicableStatus = "327001,";

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];

                ViewBag.IsAdmin = SessionInfo.IsAdmin;

                #endregion

                var structureList = (StaticPagedList<STP.Domain.Structures.StructureNotification>)null;
                int chkValidStructCnt = 0;



                if (structureId != null)
                {
                    string SID = structureId;
                    structureId = structureId.Replace(" ", "+");
                    structureId = MD5EncryptDecrypt.DecryptDetails(structureId);

                    if (structureId == "")
                        structureId = SID;

                }

                if (structureName != null)
                {
                    structureName = structureName.Replace(" ", "+");
                    structureName = MD5EncryptDecrypt.DecryptDetails(structureName);
                }

                if (pageSize == null)
                {
                    if (page == null && SOS.SortCriteria == null && SOS.SearchStatus ==null)
                    {
                        //first time page is loaded, "page" parameter will be null and model will also be null                        
                        TempData["Model"] = null;
                    }
                    else if ((page == null && SOS.SortCriteria != null) || (page == null && SOS.SearchStatus != null))
                    {
                        //search button is clicked, page parameter is null and model is supplied
                        //so save these values in the temp data  
                        DateTime dtValidateStart = new DateTime();
                        DateTime dtValidateEnd = new DateTime();
                        CultureInfo uk = new CultureInfo("en-GB");


                        if (SOS.MovementStartDate != null && DateTime.TryParseExact(SOS.MovementStartDate.Trim(), "dd/MM/yyyy", uk.DateTimeFormat, DateTimeStyles.None, out dtValidateStart))
                        {

                            SOS.MovementStartDate = dtValidateStart.ToString("dd/MM/yyyy");
                            TempData["Model"] = SOS;
                        }
                        else
                        {
                            //entered date is invalid, make it blank and update the TempData model for the other values
                            SOS.MovementStartDate = null;
                            TempData["Model"] = SOS;
                        }

                        if (SOS.MovementEndDate != null && DateTime.TryParseExact(SOS.MovementEndDate.Trim(), "dd/MM/yyyy", uk.DateTimeFormat, DateTimeStyles.None, out dtValidateEnd))
                        {

                            SOS.MovementEndDate = dtValidateEnd.ToString("dd/MM/yyyy");
                            TempData["Model"] = SOS;
                        }
                        else
                        {
                            //entered date is invalid, make it blank and update the TempData model for the other values
                            SOS.MovementEndDate = null;
                            TempData["Model"] = SOS;
                        }

                    }
                    else if (page != null && SOS.SortCriteria == null)
                    {
                        //during page number click, the page parameter will not be null but the model will be null
                        //so put the tempdata values back into the model
                        if (TempData["Model"] != null)
                        {
                            SOS = (StructureOnerousSearch)TempData["Model"];
                            if (SOS.MovementStartDate != null)
                            {
                                SOS.MovementStartDate = DateTime.Parse(SOS.MovementStartDate).ToString("dd/MM/yyyy");
                            }
                            if (SOS.MovementEndDate != null)
                            {
                                SOS.MovementEndDate = DateTime.Parse(SOS.MovementEndDate).ToString("dd/MM/yyyy");
                            }
                        }

                    }
                }
                else
                {
                    if (TempData["Model"] != null)
                    {
                        SOS = (StructureOnerousSearch)TempData["Model"];
                        if (SOS.MovementStartDate != null)
                        {
                            SOS.MovementStartDate = DateTime.Parse(SOS.MovementStartDate).ToString("dd/MM/yyyy");
                        }
                        if (SOS.MovementEndDate != null)
                        {
                            SOS.MovementEndDate = DateTime.Parse(SOS.MovementEndDate).ToString("dd/MM/yyyy");
                        }
                    }
                }

                #region Paging Part
                int pageNumber = (page ?? 1);

                if (pageSize == null)
                {
                    if (Session["PageSize"] == null)
                    {
                        Session["PageSize"] = 10;
                        pageSize = (int)Session["PageSize"];
                    }

                }
                else
                {
                    Session["PageSize"] = pageSize;
                }
                pageSize = 10;
                ViewBag.pageSize = pageSize;
                ViewBag.page = pageNumber;

                #endregion

                if (!string.IsNullOrEmpty(structureId))
                {
                    TempData["structureId"] = structureId;
                }
                else
                {
                    structureId = Convert.ToString(TempData["structureId"]);
                }

                if (!string.IsNullOrEmpty(structureName))
                {
                    TempData["structureName"] = structureName;
                }
                else
                {
                    structureName = Convert.ToString(TempData["structureName"]);
                }

                if (!string.IsNullOrEmpty(structureId))
                {
                    chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(Convert.ToInt64(structureId), SessionInfo.OrganisationId);
                }
                else
                {
                    chkValidStructCnt = 0;
                }


                if (chkValidStructCnt > 0)
                {

                    ViewBag.structureId = structureId;

                    if (SOS.StartDateFlag && SOS.MovementStartDate != null)
                    {
                        if (DateTime.TryParseExact(SOS.MovementStartDate.Trim(), "dd/MM/yyyy", ukCulture.DateTimeFormat, DateTimeStyles.None, out dtValidate))
                        {
                            StartDate = DateTime.Parse(SOS.MovementStartDate.Trim(), ukCulture.DateTimeFormat);
                            SOS.MovementStartDate = StartDate.ToString();
                            TempData["Model"] = SOS;
                        }
                        else
                        {
                            //entered date is invalid, make it blank and update the TempData model for the other values
                            SOS.MovementStartDate = null;
                            TempData["Model"] = SOS;
                        }
                    }

                    if (SOS.EndDateFlag && SOS.MovementEndDate != null)
                    {
                        if (DateTime.TryParseExact(SOS.MovementEndDate.Trim(), "dd/MM/yyyy", ukCulture.DateTimeFormat, DateTimeStyles.None, out dtValidate))
                        {
                            EndDate = DateTime.Parse(SOS.MovementEndDate.Trim(), ukCulture.DateTimeFormat);
                            SOS.MovementEndDate = EndDate.ToString();
                            TempData["Model"] = SOS;
                        }
                        else
                        {
                            //entered date is invalid, make it blank and update the TempData model for the other values
                            SOS.MovementEndDate = null;
                            TempData["Model"] = SOS;
                        }
                    }

                    if (PageStatus == true)
                    {
                        TempData["MovementStartDateEnableDisable"] = false;
                        TempData["MovementEndDateEnableDisable"] = true;
                        TempData["PageStatus"] = true;
                    }
                    else
                    {
                        if (SOS.StartDateFlag)
                        {
                            TempData["MovementStartDateEnableDisable"] = false;
                        }
                        else
                        {
                            TempData["MovementStartDateEnableDisable"] = true;
                        }

                        if (SOS.EndDateFlag)
                        {
                            TempData["MovementEndDateEnableDisable"] = false;
                        }
                        else
                        {
                            TempData["MovementEndDateEnableDisable"] = true;
                        }
                    }

                    if (SOS.SearchStatus == "2")
                    {
                        Status = rejectedStatus;
                        StatusCount = 1;
                    }
                    else if (SOS.SearchStatus == "1")
                    {
                        Status = notapplicableStatus;
                        StatusCount = 2;
                    }
                    else if (SOS.SearchStatus == "3")
                    {
                        Status = applicableStatus;
                        StatusCount = 1;
                    }

                    if (SOS.SortCriteria == null || SOS.SortCriteria == "335001")
                    {
                        Sort = 1;
                        ViewBag.ColumnToDisplay = "Gross";
                    }
                    else if (SOS.SortCriteria == "335002")
                    {
                        ViewBag.ColumnToDisplay = "Axle";
                        Sort = 2;
                    }

                    List<STP.Domain.Structures.StructureNotification> structureNotificationList = structuresService.GetAllStructureOnerousVehicles(structureId, pageNumber, (int)pageSize, null, Status, StartDate, EndDate, StatusCount, Sort, SessionInfo.OrganisationId);

                    if (structureNotificationList != null && structureNotificationList.Count != 0)
                    {
                        structureNotificationCount = (int)structureNotificationList[0].CountRec;
                    }

                    ViewBag.TotalCount = structureNotificationCount;

                    TempData.Keep("structureId");
                    TempData.Keep("structureName");

                    TempData.Keep("Model");
                    TempData.Keep("PageStatus");

                    TempData.Keep("MovementStartDateEnableDisable");
                    TempData.Keep("MovementEndDateEnableDisable");

                    structureList = new StaticPagedList<STP.Domain.Structures.StructureNotification>(structureNotificationList, pageNumber, (int)pageSize, ViewBag.TotalCount);

                    ViewBag.StructureName = structureName;
                }

                return PartialView(structureList);

            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/StructureNotificationList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #region SearchMostOnerousVehiclePanel
        [HttpGet]
        public ActionResult SearchMostOnerousVehiclePanel(int structureId, string structureName)
        {
            StructureOnerousSearch search = new StructureOnerousSearch();

            List<SelectListItem> SortList = new List<SelectListItem>();

            //A static list is added since all values of the enum_type are not to be didplayed on the screen.

            SelectListItem si = new SelectListItem();
            si.Text = "Gross weight";
            si.Value = "335001";

            SortList.Add(si);

            si = new SelectListItem();
            si.Text = "Axle weight";
            si.Value = "335002";

            SortList.Add(si);

            if (TempData["Model"] != null)
            {
                StructureOnerousSearch TempModel = new StructureOnerousSearch();
                TempModel = (StructureOnerousSearch)TempData["Model"];
                SortList.Where(w => w.Value == Convert.ToString(TempModel.SortCriteria)).ToList().ForEach(s => s.Selected = true);
            }



            if (TempData["Model"] != null)
            {
                search = (StructureOnerousSearch)TempData["Model"];
                TempData.Keep("Model");
            }

            if (TempData["PageStatus"] != null)
            {
                ViewBag.MovementStartDateEnableDisable = true;
                ViewBag.MovementEndDateEnableDisable = true;
            }
            else
            {

                if (TempData["MovementStartDateEnableDisable"] != null)
                {
                    if ((bool)TempData["MovementStartDateEnableDisable"])
                        ViewBag.MovementStartDateEnableDisable = true;
                    else
                        ViewBag.MovementStartDateEnableDisable = false;
                }

                if (TempData["MovementEndDateEnableDisable"] != null)
                {
                    if ((bool)TempData["MovementEndDateEnableDisable"])
                        ViewBag.MovementEndDateEnableDisable = true;
                    else
                        ViewBag.MovementEndDateEnableDisable = false;
                }
            }

            ViewBag.SearchList = SortList;
            ViewBag.structureId = structureId;
            ViewBag.structureName = structureName;
            return View(search);

        }
        #endregion


        #region public ActionResult GeneralStructure()
        public ActionResult GeneralStructure(int structureID, int? sectionID, string structureNm = "", string ESRN = "")
        {
            try
            {
                // int structureID = 88630;
                List<StructureGeneralDetails> objStructureGeneralDetails = null;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var SessionInfo = (UserInfo)Session["UserInfo"];

                #region For distribution status viewer
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation((long)structureID, SessionInfo.OrganisationId);
                int orgCheck = structuresService.CheckStructureOrganisation((int)SessionInfo.OrganisationId, (long)structureID); // to check the strcutre belongs to same organisation id 8389 related fix
                if (orgCheck == 0)
                {
                    return RedirectToAction("Error", "Home");
                }
                if (chkValidStructCnt > 0)
                {

                    objStructureGeneralDetails = structuresService.ViewGeneralDetails(structureID);

                    string[] arrAltName = null;
                    if (!string.IsNullOrEmpty(objStructureGeneralDetails[0].StructureAlternateNameOne))
                    {
                        arrAltName = objStructureGeneralDetails[0].StructureAlternateNameOne.Split(',');
                        objStructureGeneralDetails[0].StructureAlternateNameOne = Convert.ToString(arrAltName[0]);
                        objStructureGeneralDetails[0].StructureAlternateNameTwo = arrAltName.Length > 1 ? arrAltName[1] : null;// Convert.ToString(arrAltName[1]);
                        objStructureGeneralDetails[0].StructureAlternateNameThree = arrAltName.Length > 2 ? arrAltName[2] : null;// Convert.ToString(arrAltName[3]);
                    }

                    string[] arrType = null;


                    List<StructCategory> dropdownObjCatList = structuresService.GetStructCategory(233);
                    List<StructType> dropdownObjTypeList = structuresService.GetStructType(204);


                    if (objStructureGeneralDetails.Count > 0)
                    {
                        if (objStructureGeneralDetails[0].StructureClass.Trim().ToLower() == "level crossing")
                        {

                            dropdownObjCatList = (from s in dropdownObjCatList
                                                      //where s.OrgName.ToLower().StartsWith(SearchString.ToLower())
                                                  where s.StructureCategory.Trim().ToLower() == "level crossing"
                                                  select s).ToList();

                            dropdownObjTypeList = (from s in dropdownObjTypeList
                                                       //where s.OrgName.ToLower().StartsWith(SearchString.ToLower())
                                                   where s.StrustureType.Trim().ToLower() == "level crossing"
                                                   select s).ToList();
                        }
                        else
                        {
                            dropdownObjCatList = (from s in dropdownObjCatList
                                                      //where s.OrgName.ToLower().StartsWith(SearchString.ToLower())
                                                  where s.StructureCategory.Trim().ToLower() != "level crossing"
                                                  select s).ToList();

                            dropdownObjTypeList = (from s in dropdownObjTypeList
                                                       //where s.OrgName.ToLower().StartsWith(SearchString.ToLower())
                                                   where s.StrustureType.Trim().ToLower() != "level crossing"
                                                   select s).ToList();

                        }
                        TempData["DropDownObjTypeList"] = dropdownObjTypeList;
                        TempData["DropDownObjCatList"] = dropdownObjCatList;

                        string CatCode;
                        if (!string.IsNullOrEmpty(objStructureGeneralDetails[0].Category) && objStructureGeneralDetails[0].Category != "0")
                        {
                            CatCode = (from s in dropdownObjCatList
                                       where s.StructureCategory.Trim().ToLower() == objStructureGeneralDetails[0].Category.Trim().ToLower()
                                       select s.StructureCaegorytId.ToString()).FirstOrDefault();
                            objStructureGeneralDetails[0].CategoryCode = string.IsNullOrEmpty(CatCode) ? "999999999" : CatCode;
                            objStructureGeneralDetails[0].CategoryUserDefined = objStructureGeneralDetails[0].CategoryCode == "999999999" ? objStructureGeneralDetails[0].Category : null;
                        }
                        else
                        {
                            objStructureGeneralDetails[0].Category = "Select Category";
                            objStructureGeneralDetails[0].CategoryCode = "999999998";
                        }

                        string TypeCode = null;
                        if (!string.IsNullOrEmpty(objStructureGeneralDetails[0].Type) && objStructureGeneralDetails[0].Type != "0")
                        {
                            TypeCode = (from s in dropdownObjTypeList
                                        where s.StrustureType.Trim().ToLower() == objStructureGeneralDetails[0].Type.Trim().ToLower()
                                        select s.StructureTypeId.ToString()).FirstOrDefault();
                            objStructureGeneralDetails[0].TypeCode = string.IsNullOrEmpty(TypeCode) ? "999999999" : TypeCode;
                            objStructureGeneralDetails[0].TypeUserDefined = objStructureGeneralDetails[0].TypeCode == "999999999" ? objStructureGeneralDetails[0].Type : null;
                        }
                        else
                        {
                            objStructureGeneralDetails[0].Type = "Select Type";
                            objStructureGeneralDetails[0].TypeCode = "999999998";
                        }

                        string TypeOneCode = null;
                        if (!string.IsNullOrEmpty(objStructureGeneralDetails[0].Type1) && objStructureGeneralDetails[0].Type1 != "0")
                        {
                            TypeOneCode = (from s in dropdownObjTypeList
                                           where s.StrustureType.Trim().ToLower() == objStructureGeneralDetails[0].Type1.Trim().ToLower()
                                           select s.StructureTypeId.ToString()).FirstOrDefault();

                            objStructureGeneralDetails[0].Type1Code = string.IsNullOrEmpty(TypeOneCode) ? "999999999" : TypeOneCode;
                            objStructureGeneralDetails[0].Type1UserDefined = objStructureGeneralDetails[0].Type1Code == "999999999" ? objStructureGeneralDetails[0].Type1 : null;
                        }
                        else
                        {
                            objStructureGeneralDetails[0].Type1 = "Select Type";
                            objStructureGeneralDetails[0].Type1Code = "999999998";
                        }

                        string TypeTwoCode = null;
                        if (!string.IsNullOrEmpty(objStructureGeneralDetails[0].Type2) && objStructureGeneralDetails[0].Type2 != "0")
                        {
                            TypeTwoCode = (from s in dropdownObjTypeList
                                           where s.StrustureType.Trim().ToLower() == objStructureGeneralDetails[0].Type2.Trim().ToLower()
                                           select s.StructureTypeId.ToString()).FirstOrDefault();

                            objStructureGeneralDetails[0].Type2Code = string.IsNullOrEmpty(TypeTwoCode) ? "999999999" : TypeTwoCode;
                            objStructureGeneralDetails[0].Type2UserDefined = objStructureGeneralDetails[0].Type2Code == "999999999" ? objStructureGeneralDetails[0].Type2 : null;
                        }
                        else
                        {
                            objStructureGeneralDetails[0].Type2 = "Select Type";
                            objStructureGeneralDetails[0].Type2Code = "999999998";
                        }
                    }

                    StructCategory objStructCat = new StructCategory();
                    objStructCat.StructureCaegorytId = 999999998;
                    objStructCat.StructureCategory = "Select Category";
                    dropdownObjCatList.Insert(0, objStructCat);

                    if (objStructureGeneralDetails[0].StructureClass.Trim().ToLower() != "level crossing")
                    {
                        StructCategory objStructCat1 = new StructCategory();
                        objStructCat1.StructureCaegorytId = 999999999;
                        objStructCat1.StructureCategory = "User defined";
                        dropdownObjCatList.Insert(dropdownObjCatList.Count, objStructCat1);
                    }

                    ViewBag.structCategory = new SelectList(dropdownObjCatList, "StructureCaegorytId", "StructureCategory");

                    StructType objStructType = new StructType();
                    objStructType.StructureTypeId = 999999998;
                    objStructType.StrustureType = "Select Type";
                    dropdownObjTypeList.Insert(0, objStructType);

                    if (objStructureGeneralDetails[0].StructureClass.Trim().ToLower() != "level crossing")
                    {
                        StructType objStructType1 = new StructType();
                        objStructType1.StructureTypeId = 999999999;
                        objStructType1.StrustureType = "User defined";
                        dropdownObjTypeList.Insert(dropdownObjTypeList.Count, objStructType1);
                    }

                    ViewBag.structType = new SelectList(dropdownObjTypeList, "StructureTypeId", "StrustureType");
                    ViewBag.StructureId = structureID;

                    Session["sectionID"] = sectionID;
                    Session["structureNm"] = structureNm;
                    Session["ESRN"] = ESRN;
                    ViewBag.structName = structureNm;
                    ViewBag.ESRN = ESRN;
                    Session["checkFlag"] = "true";
                    Session["sectionID"] = sectionID;

                    ViewBag.saveFlag = Session["saveFlag"];
                    Session["saveFlag"] = null;
                }
                return View(objStructureGeneralDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region public ActionResult EditStructureGeneralDetails()
        //public ActionResult EditStructureGeneralDetails(string SummaryName, string AltName1, string AltName2, string AltName3, string StructKey, string Description, string Notes, int StructLength, int StructureID, long OrganisationId)
        //{
        [HttpPost]
        public JsonResult EditStructureGeneralDetails(List<StructureGeneralDetails> objStructureGeneralDetails)
        {

            try
            {

                #region Session Check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return Json("expire", JsonRequestBehavior.AllowGet);
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion
             
                var dropdownObjTypeList = TempData["DropDownObjTypeList"] as List<StructType>;
                var dropdownObjCatList = TempData["DropDownObjCatList"] as List<StructCategory>;

                objStructureGeneralDetails[0].OrganisationId = (int)SessionInfo.OrganisationId;

                //------------------------------#6029 Unable to save structure details------------------------------------------------------------------------------
                if (objStructureGeneralDetails[0].StructureAlternateNameOne != null)
                {
                    if (objStructureGeneralDetails[0].StructureAlternateNameOne.Contains("_*") || objStructureGeneralDetails[0].StructureAlternateNameOne.Contains("*_"))
                    {
                        objStructureGeneralDetails[0].StructureAlternateNameOne = objStructureGeneralDetails[0].StructureAlternateNameOne.Replace("_*", "<");
                        objStructureGeneralDetails[0].StructureAlternateNameOne = objStructureGeneralDetails[0].StructureAlternateNameOne.Replace("*_", ">");
                    }
                }
                if (objStructureGeneralDetails[0].StructureAlternateNameTwo != null)
                {
                    if (objStructureGeneralDetails[0].StructureAlternateNameTwo.Contains("_*") || objStructureGeneralDetails[0].StructureAlternateNameTwo.Contains("*_"))
                    {
                        objStructureGeneralDetails[0].StructureAlternateNameTwo = objStructureGeneralDetails[0].StructureAlternateNameTwo.Replace("_*", "<");
                        objStructureGeneralDetails[0].StructureAlternateNameTwo = objStructureGeneralDetails[0].StructureAlternateNameTwo.Replace("*_", ">");
                    }
                }
                if (objStructureGeneralDetails[0].StructureAlternateNameThree != null)
                {
                    if (objStructureGeneralDetails[0].StructureAlternateNameThree.Contains("_*") || objStructureGeneralDetails[0].StructureAlternateNameThree.Contains("*_"))
                    {
                        objStructureGeneralDetails[0].StructureAlternateNameThree = objStructureGeneralDetails[0].StructureAlternateNameThree.Replace("_*", "<");
                        objStructureGeneralDetails[0].StructureAlternateNameThree = objStructureGeneralDetails[0].StructureAlternateNameThree.Replace("*_", ">");
                    }
                }
                //-------------------------------#6029 Unable to save structure details------------------------------------------------------------------------------
                objStructureGeneralDetails[0].StructureAlternateNameOne = !string.IsNullOrEmpty(objStructureGeneralDetails[0].StructureAlternateNameOne) ? objStructureGeneralDetails[0].StructureAlternateNameOne : null;

                objStructureGeneralDetails[0].StructureAlternateNameOne += !string.IsNullOrEmpty(objStructureGeneralDetails[0].StructureAlternateNameTwo) ? "," + objStructureGeneralDetails[0].StructureAlternateNameTwo : null;

                objStructureGeneralDetails[0].StructureAlternateNameOne += !string.IsNullOrEmpty(objStructureGeneralDetails[0].StructureAlternateNameThree) ? "," + objStructureGeneralDetails[0].StructureAlternateNameThree : null;

                if (objStructureGeneralDetails[0].Category != "Select Category")
                {
                    string CatName = null;
                    if (objStructureGeneralDetails[0].Category.ToLower() == "user defined")
                    {
                        objStructureGeneralDetails[0].Category = objStructureGeneralDetails[0].CategoryUserDefined;
                    }
                    if (dropdownObjCatList != null)
                    {
                        CatName = (from s in dropdownObjCatList
                                   where s.StructureCategory.Trim().ToLower() == objStructureGeneralDetails[0].Category.Trim().ToLower()
                                    select s.StructureCategory.ToString()).FirstOrDefault();
                        objStructureGeneralDetails[0].Category = string.IsNullOrEmpty(CatName) ? objStructureGeneralDetails[0].Category : CatName;
                    }
                }
                else if (objStructureGeneralDetails[0].Category == "Select Category")
                {
                    objStructureGeneralDetails[0].Category = null;
                }

                if (objStructureGeneralDetails[0].Type != "Select Type")
                {
                    string TypeName = null;
                    if (objStructureGeneralDetails[0].Type.ToLower() == "user defined")
                    {
                        objStructureGeneralDetails[0].Type = objStructureGeneralDetails[0].TypeUserDefined;
                    }
                    if (dropdownObjTypeList != null)
                    {
                        TypeName = (from s in dropdownObjTypeList
                                    where s.StrustureType.Trim().ToLower() == objStructureGeneralDetails[0].Type.Trim().ToLower()
                                    select s.StrustureType.ToString()).FirstOrDefault();
                        objStructureGeneralDetails[0].Type = string.IsNullOrEmpty(TypeName) ? objStructureGeneralDetails[0].Type : TypeName;
                    }
                }
                else if (objStructureGeneralDetails[0].Type == "Select Type")
                {
                    objStructureGeneralDetails[0].Type = null;
                }


                if (objStructureGeneralDetails[0].Type1 != "Select Type")
                {
                    string TypeName = null;
                    if (objStructureGeneralDetails[0].Type1.ToLower() == "user defined")
                    {
                        objStructureGeneralDetails[0].Type1 = objStructureGeneralDetails[0].Type1UserDefined;
                    }
                    if (dropdownObjTypeList != null)
                    {
                        TypeName = (from s in dropdownObjTypeList
                                    where s.StrustureType.Trim().ToLower() == objStructureGeneralDetails[0].Type1.Trim().ToLower()
                                    select s.StrustureType.ToString()).FirstOrDefault();
                        objStructureGeneralDetails[0].Type1 = string.IsNullOrEmpty(TypeName) ? objStructureGeneralDetails[0].Type1 : TypeName;
                    }
                }
                else if (objStructureGeneralDetails[0].Type1 == "Select Type")
                {
                    objStructureGeneralDetails[0].Type1 = null;
                }

                if (objStructureGeneralDetails[0].Type2 != "Select Type")
                {
                    string TypeName = null;
                    if (objStructureGeneralDetails[0].Type2.ToLower() == "user defined")
                    {
                        objStructureGeneralDetails[0].Type2 = objStructureGeneralDetails[0].Type2UserDefined;
                    }
                    if (dropdownObjTypeList != null)
                    {
                        TypeName = (from s in dropdownObjTypeList
                                    where s.StrustureType.Trim().ToLower() == objStructureGeneralDetails[0].Type2.Trim().ToLower()
                                    select s.StrustureType.ToString()).FirstOrDefault();
                        objStructureGeneralDetails[0].Type2 = string.IsNullOrEmpty(TypeName) ? objStructureGeneralDetails[0].Type2 : TypeName;
                    }                    
                }
                else if (objStructureGeneralDetails[0].Type2 == "Select Type")
                {
                    objStructureGeneralDetails[0].Type2 = null;
                }

                // public ActionResult GeneralStructure(int structureID, int? sectionID, string structureNm = "", string ESRN = "")
                int sectionID = Convert.ToInt32(Session["sectionID"]);
                string structureNm = Convert.ToString(Session["structureNm"]);
                string ESRN = Convert.ToString(Session["ESRN"]);

                UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
                objStructureGeneralDetails[0].UserName = ObjUserInfo.UserName;
                bool saveFlag = structuresService.EditStructureGeneralDetails(objStructureGeneralDetails[0]);//.ediViewGeneralDetails(structureID);EditStructureGeneralDetails
                ViewBag.saveFlag = saveFlag;
                Session["saveFlag"] = saveFlag;
                //return RedirectToAction("GeneralStructure", "Structures", new { structureID = objStructureGeneralDetails[0].StructureId, sectionID = sectionID, structureNm = structureNm, ESRN = ESRN });
                return Json(saveFlag, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SaveHBToSV
        [HttpPost]
        public JsonResult SaveHBToSV(long structureId = 0, long sectionId = 0, double? hbWithLoad = null, double? hbWithoutLoad = null, double? SV80 = null, double? SV100 = null, double? SV150 = null, double? SVTrain = null, double? SVTT = null)
        {
            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
            string UserName = ObjUserInfo.UserName;
            int VehicleType;
            double? WithLoad = null;
            double? WithoutLoad = null;

            // for SV 80 vehicles
            VehicleType = 340002;
            if (SV80 == null)
            {
                WithLoad = null;
                WithoutLoad = null;
            }
            else
            {
                if (hbWithLoad != null)
                {
                    WithLoad = SV80;
                }
                if (hbWithoutLoad != null)
                {
                    WithoutLoad = SV80;
                }
            }
            UpdateSVParams objUpdateSVParams = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = sectionId,
                StructId = structureId,
                SVDerivation = 275002,
                UserName = UserName,
                ManualFlag = 1,
                HbWithLoad = hbWithLoad,
                HbWithoutLoad = hbWithoutLoad

            };
            var result = structuresService.UpdateSVData(objUpdateSVParams);

            // for SV 100 vehicles
            VehicleType = 340003;
            if (SV100 == null)
            {
                WithLoad = null;
                WithoutLoad = null;
            }
            else
            {
                if (hbWithLoad != null)
                {
                    WithLoad = SV100;
                }
                if (hbWithoutLoad != null)
                {
                    WithoutLoad = SV100;
                }
            }
            UpdateSVParams objUpdateSVParams1 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = sectionId,
                StructId = structureId,
                SVDerivation = 275002,
                UserName = UserName,
                ManualFlag = 1,
                HbWithLoad = hbWithLoad,
                HbWithoutLoad = hbWithoutLoad

            };

            var result1 = structuresService.UpdateSVData(objUpdateSVParams1);

            // for SV 150 vehicles
            VehicleType = 340004;
            if (SV150 == null)
            {
                WithLoad = null;
                WithoutLoad = null;
            }
            else
            {
                if (hbWithLoad != null)
                {
                    WithLoad = SV150;
                }
                if (hbWithoutLoad != null)
                {
                    WithoutLoad = SV150;
                }
            }
            UpdateSVParams objUpdateSVParams2 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = sectionId,
                StructId = structureId,
                SVDerivation = 275002,
                UserName = UserName,
                ManualFlag = 1,
                HbWithLoad = hbWithLoad,
                HbWithoutLoad = hbWithoutLoad

            };
            var result2 = structuresService.UpdateSVData(objUpdateSVParams2);

            // for SV Train vehicles
            VehicleType = 340005;
            if (SVTrain == null)
            {
                WithLoad = null;
                WithoutLoad = null;
            }
            else
            {
                if (hbWithLoad != null)
                {
                    WithLoad = SVTrain;
                }
                if (hbWithoutLoad != null)
                {
                    WithoutLoad = SVTrain;
                }
            }
            UpdateSVParams objUpdateSVParams3 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = sectionId,
                StructId = structureId,
                SVDerivation = 275002,
                UserName = UserName,
                ManualFlag = 1,
                HbWithLoad = hbWithLoad,
                HbWithoutLoad = hbWithoutLoad

            };
            var result3 = structuresService.UpdateSVData(objUpdateSVParams3);

            // for SV TT vehicle
            VehicleType = 340006;
            if (SVTT == null)
            {
                WithLoad = null;
                WithoutLoad = null;
            }
            else
            {
                if (hbWithLoad != null)
                {
                    WithLoad = SVTT;
                }
                if (hbWithoutLoad != null)
                {
                    WithoutLoad = SVTT;
                }
            }
            UpdateSVParams objUpdateSVParams4 = new UpdateSVParams
            {
                WithLoad = WithLoad,
                VehicleType = VehicleType,
                WithoutLoad = WithoutLoad,
                SectionId = sectionId,
                StructId = structureId,
                SVDerivation = 275002,
                UserName = UserName,
                ManualFlag = 1,
                HbWithLoad = hbWithLoad,
                HbWithoutLoad = hbWithoutLoad

            };
            var result4 = structuresService.UpdateSVData(objUpdateSVParams4);

            //HB_RATING_WITH_LOAD, HB_RATING_WITHOUT_LOAD
            return Json(new { result = 1 });
        }
        #endregion

        #region public ActionResult StructureImposedContraint()
        //[HttpPost]
        public ActionResult StructureImposedContraint(int structureId = 0, int sectionId = 0, string structureNm = "", string ESRN = "", long strucType = 1)
        {
            try
            {
                ////int deStructId = Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(structureId));
                ////int deSectId = Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(sectionId));
                ////string deStructNm = MD5EncryptDecrypt.DecryptDetails(structureNm);
                ////string deESRN = MD5EncryptDecrypt.DecryptDetails(ESRN);
                ////long deStrucType = Convert.ToInt32(MD5EncryptDecrypt.DecryptDetails(strucType));

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var SessionInfo = (UserInfo)Session["UserInfo"];

                #region For document status viewer
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                int chkValidStructCnt = structuresService.CheckStructureAgainstOrganisation(structureId, SessionInfo.organisationId);
                int orgCheck = structuresService.CheckStructureOrganisation((int)SessionInfo.OrganisationId, (long)structureId); // to check the sturcutre belongs to same organisation id 8389 related fix
                if (orgCheck == 0)
                {
                    return RedirectToAction("Error", "Home");
                }

                ImposedConstraints objImpoConstr = null;
                if (chkValidStructCnt > 0)
                {
                    objImpoConstr = structuresService.ViewimposedConstruction(structureId, sectionId);

                    if (objImpoConstr.SignedHeight.HeightFeet != null)
                    {
                        objImpoConstr.SignedHeight.HeightInches = getAbsoluteInch(objImpoConstr.SignedHeight.HeightFeet);
                        objImpoConstr.SignedHeight.HeightFeet = (int)objImpoConstr.SignedHeight.HeightFeet;
                    }
                    if (objImpoConstr.SignedLength.LengthFeet != null)
                    {
                        objImpoConstr.SignedLength.LengthInches = getAbsoluteInch(objImpoConstr.SignedLength.LengthFeet);
                        objImpoConstr.SignedLength.LengthFeet = (int)objImpoConstr.SignedLength.LengthFeet;
                    }
                    if (objImpoConstr.SignedWidth.WidthFeet != null)
                    {
                        objImpoConstr.SignedWidth.WidthInches = getAbsoluteInch(objImpoConstr.SignedWidth.WidthFeet);
                        objImpoConstr.SignedWidth.WidthFeet = (int)objImpoConstr.SignedWidth.WidthFeet;
                    }
                    // objImpoConstr.Signed_Height.HeightFeet = getDisplayFeet(objImpoConstr.Signed_Height.HeightMeter, objImpoConstr.Signed_Height.HeightFeet);
                    //objImpoConstr.Signed_Height.HeightInches = getDisplayInch(objImpoConstr.Signed_Height.HeightMeter, objImpoConstr.Signed_Height.HeightFeet);
                    //objImpoConstr.Signed_Length.LengthFeet = getDisplayFeet(objImpoConstr.Signed_Length.LengthMeter, objImpoConstr.Signed_Length.LengthFeet);
                    //objImpoConstr.Signed_Length.LengthInches = getDisplayInch(objImpoConstr.Signed_Length.LengthMeter, objImpoConstr.Signed_Length.LengthFeet);
                    //objImpoConstr.Signed_Width.WidthFeet = getDisplayFeet(objImpoConstr.Signed_Width.WidthMeter, objImpoConstr.Signed_Width.WidthFeet);
                    //objImpoConstr.Signed_Width.WidthInches = getDisplayInch(objImpoConstr.Signed_Width.WidthMeter, objImpoConstr.Signed_Width.WidthFeet);

                    ViewBag.structureId = structureId;
                    ViewBag.sectionId = sectionId;
                    ViewBag.strucType = strucType;


                    ViewBag.structName = structureNm;
                    ViewBag.ESRN = ESRN;

                    if (sectionId != null)
                    {
                        Session["checkFlag"] = "true";
                        Session["sectionID"] = sectionId;
                    }

                    Session["structureNm"] = structureNm;
                    Session["ESRN"] = ESRN;
                    Session["strucType"] = strucType;
                    ViewBag.saveFlag = TempData["saveFlag"];
                }
                return View(objImpoConstr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region StoreEditSTRUCT_IMPOSED
        [HttpPost]
        public JsonResult StoreEditSTRUCT_IMPOSED(ImposedConstraints objStrucImpoConst)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }
            #endregion
            TempData["ImposedConstraintsTemp"] = objStrucImpoConst;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EDIT STRUCT IMPOSED
        [HttpPost]
        public JsonResult EditSTRUCT_IMPOSED(ImposedConstraints objStrucImpoConst, long StructId = 0, long SectionId = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(TempData["ImposedConstraintsTemp"])))
                {
                    objStrucImpoConst = (ImposedConstraints)TempData["ImposedConstraintsTemp"];
                }
                bool flag = false;
                double? feet = null, inch = null;
                ViewBag.strucType = Session["strucType"];
                int structType = Convert.ToInt16(Session["strucType"]);
                UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
                objStrucImpoConst.UserName = ObjUserInfo.UserName;
                objStrucImpoConst.OrgId = ObjUserInfo.organisationId;
                if (objStrucImpoConst.SignedHeightStatusBool == true)
                    objStrucImpoConst.SignedHeightStatus = 251001;
                else
                    objStrucImpoConst.SignedHeightStatus = 251002;

                if (objStrucImpoConst.SignedWidthStatusBool == true)
                    objStrucImpoConst.SignedWidthStatus = 251001;
                else
                    objStrucImpoConst.SignedWidthStatus = 251002;

                if (objStrucImpoConst.SignedLengthStatusBool == true)
                    objStrucImpoConst.SignedLengthStatus = 251001;
                else
                    objStrucImpoConst.SignedLengthStatus = 251002;

                if (objStrucImpoConst.SignedGrossWeightStatusBool == true)
                    objStrucImpoConst.SignedGrossWeightStatus = 251001;
                else
                    objStrucImpoConst.SignedGrossWeightStatus = 251002;

                if (objStrucImpoConst.SignedAxcelWeightStatusBool == true)
                    objStrucImpoConst.SignedAxleWeightStatus = 251001;
                else
                    objStrucImpoConst.SignedAxleWeightStatus = 251002;

                //objStrucImpoConst.Signed_Height = new SignedHeight();
                //objStrucImpoConst.Signed_Width = new SignedWidth();

                if ((objStrucImpoConst.SignedHeightRadio != null ? objStrucImpoConst.SignedHeightRadio.Trim() : "") == "NotKnown")
                {
                    objStrucImpoConst.SignedHeight.HeightMeter = null;
                    objStrucImpoConst.SignedHeight.HeightFeet = null;
                }
                else if ((objStrucImpoConst.SignedHeightRadio != null ? objStrucImpoConst.SignedHeightRadio.Trim() : "") == "NoSigned")
                {
                    objStrucImpoConst.SignedHeight.HeightMeter = 0;
                    objStrucImpoConst.SignedHeight.HeightFeet = 0;
                }
                else
                {


                    if (objStrucImpoConst.SignedHeight.HeightFeet != null || objStrucImpoConst.SignedHeight.HeightInches != null)
                    {
                        objStrucImpoConst.SignedHeight.HeightInches = objStrucImpoConst.SignedHeight.HeightInches == null ? 0 : objStrucImpoConst.SignedHeight.HeightInches;
                        feet = objStrucImpoConst.SignedHeight.HeightInches / 12;
                        objStrucImpoConst.SignedHeight.HeightFeet = feet + (objStrucImpoConst.SignedHeight.HeightFeet != null ? objStrucImpoConst.SignedHeight.HeightFeet : 0);
                    }

                }

                if ((objStrucImpoConst.SignedWidthRadio != null ? objStrucImpoConst.SignedWidthRadio.Trim() : "") == "NotKnown")
                {
                    objStrucImpoConst.SignedWidth.WidthMeter = null;
                    objStrucImpoConst.SignedWidth.WidthFeet = null;
                }
                else if ((objStrucImpoConst.SignedWidthRadio != null ? objStrucImpoConst.SignedWidthRadio.Trim() : "") == "NoSigned")
                {
                    objStrucImpoConst.SignedWidth.WidthMeter = 0;
                    objStrucImpoConst.SignedWidth.WidthMeter = 0;
                }
                else
                {
                  
                    if (objStrucImpoConst.SignedWidth.WidthFeet != null || objStrucImpoConst.SignedWidth.WidthInches != null)
                    {
                       
                        objStrucImpoConst.SignedWidth.WidthInches = objStrucImpoConst.SignedWidth.WidthInches == null ? 0 : objStrucImpoConst.SignedWidth.WidthInches;
                        feet = objStrucImpoConst.SignedWidth.WidthInches / 12;
                        objStrucImpoConst.SignedWidth.WidthFeet = feet + (objStrucImpoConst.SignedWidth.WidthFeet != null ? objStrucImpoConst.SignedWidth.WidthFeet : 0);
                    }
                }

                if ((objStrucImpoConst.SignedLengthRadio != null ? objStrucImpoConst.SignedLengthRadio.Trim() : "") == "NotKnown")
                {
                    objStrucImpoConst.SignedLength.LengthMeter = null;
                    objStrucImpoConst.SignedLength.LengthFeet = null;
                }
                else if ((objStrucImpoConst.SignedLengthRadio != null ? objStrucImpoConst.SignedLengthRadio.Trim() : "") == "NoSigned")
                {
                    objStrucImpoConst.SignedLength.LengthMeter = 0;
                    objStrucImpoConst.SignedLength.LengthFeet = 0;
                }
                else
                {
                  
                    if (objStrucImpoConst.SignedLength.LengthFeet != null || objStrucImpoConst.SignedLength.LengthInches != null)
                    {

                        objStrucImpoConst.SignedLength.LengthInches = objStrucImpoConst.SignedLength.LengthInches == null ? 0 : objStrucImpoConst.SignedLength.LengthInches;
                        feet = objStrucImpoConst.SignedLength.LengthInches / 12;
                        objStrucImpoConst.SignedLength.LengthFeet = feet + (objStrucImpoConst.SignedLength.LengthFeet != null ? objStrucImpoConst.SignedLength.LengthFeet : 0);
                    }
                }

                if ((objStrucImpoConst.SignedGrossWeightRadio != null ? objStrucImpoConst.SignedGrossWeightRadio.Trim() : "") == "NotKnown")
                {
                    objStrucImpoConst.SignedGrossWeightObj.GrossWeight = null;
                }
                else if ((objStrucImpoConst.SignedGrossWeightRadio != null ? objStrucImpoConst.SignedGrossWeightRadio.Trim() : "") == "NoSigned")
                {
                    objStrucImpoConst.SignedGrossWeightObj.GrossWeight = 0;
                }

                if ((objStrucImpoConst.SignedAxleWeightRadio != null ? objStrucImpoConst.SignedAxleWeightRadio.Trim() : "") == "NotKnown")
                {
                    objStrucImpoConst.SignedAxleWeight.AxleWeight = null;
                }
                else if ((objStrucImpoConst.SignedAxleWeightRadio != null ? objStrucImpoConst.SignedAxleWeightRadio.Trim() : "") == "NoSigned")
                {
                    objStrucImpoConst.SignedAxleWeight.AxleWeight = 0;
                }
                StuctImposedParams stuctImposedParams = new StuctImposedParams
                {
                    StructImposConstraints = objStrucImpoConst,
                    StructId = (int)StructId,
                    SectionId = (int)SectionId,
                    StructType = structType
                };
                flag = structuresService.GetEditStructureImposed(stuctImposedParams);
                TempData["saveFlag"] = flag;

                string StructName = Convert.ToString(Session["structureNm"]);
                string ESRN = Convert.ToString(Session["ESRN"]);
                return Json(flag, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("StructureImposedContraint", "Structures", new { structureId = StructId, sectionId = SectionId, structureNm = StructName, ESRN = ESRN });
                // return Json(new { value = flag });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region HBToSVConversion
        //[HttpGet]
        public ActionResult HBToSVConversion(long structureId = 0, long sectionId = 0, string structName = "", string ESRN = "",bool performbtn=false)
        {
            double? hbWithLoad, hbWithoutLoad;
            List<double?> lstHBRating = structuresService.GetHBRatings(structureId, sectionId);
            ViewBag.LstHBRating = lstHBRating;

            if (lstHBRating != null && lstHBRating.Any())
            {
                hbWithLoad = lstHBRating[0];
                hbWithoutLoad = lstHBRating[1];
            }
            else
            {
                hbWithLoad = null;
                hbWithoutLoad = null;
            }

            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];

            List<SvReserveFactors> infos = structuresService.GetCalculatedHBToSV(structureId, sectionId, (double?)hbWithLoad, (double?)hbWithoutLoad, 0, ObjUserInfo.UserName);
            ViewBag.LstCalSvReserveFactors = infos;

            List<SVDataList> lstManSVDataListObj = structuresService.GetSVData(structureId, sectionId);
            ViewBag.LstManSVDataListObj = lstManSVDataListObj;

            ViewBag.StructureId = structureId;
            ViewBag.SectionId = sectionId;
            ViewBag.structName = structName;
            ViewBag.ESRN = ESRN;
            ViewBag.performBtnFlag = performbtn;
            return PartialView();
        }
        #endregion

        #region GetCalculatedHBToSV
        [HttpPost]
        public ActionResult GetCalculatedHBToSV(long structureId = 0, long sectionId = 0, double hbWithLoad = 0, double hbWithoutLoad = 0)
        {
            //var infos = new List<SvReserveFactors>();
            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
            List<SvReserveFactors> infos = structuresService.GetCalculatedHBToSV(structureId, sectionId, (double?)hbWithLoad, (double?)hbWithoutLoad, 1, ObjUserInfo.UserName);
            return Json(new { result = infos });
        }
        #endregion

        /// <summary>
        /// Get the inch part in the passed double feet value
        /// </summary>
        /// <param name="feet"></param>
        /// <returns></returns>
        public int getAbsoluteInch(double? feet)
        {
            int nInches = 0;
            if (feet != null)
            {
                int nFeet = (int)feet;
                nInches = (int)((feet - nFeet) * 12);
            }
            return nInches;
        }
        /// <summary>
        /// delete structure contact
        /// </summary>
        /// <param name="StructureId"></param>
        /// <param name="ContactNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteStructureContact(long cautionId, short contactNo)
        {
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json("expire", JsonRequestBehavior.AllowGet);
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion
            structuredeligationService.DeleteStructureContact(contactNo, cautionId);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        #region public ActionResult RoadContact(long linkID)
        [AuthorizeUser(Roles = "13001,12000,12001,12002,12003,12005,40003,40004,100001")] // Added for 8389 regression since this method is used in haulier portal
        public ActionResult RoadContact(long linkID, long length)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var SessionInfo = (UserInfo)Session["UserInfo"];
            List<RoadContactModal> RoadContactList = new List<RoadContactModal>();
            RoadContactList = structuresService.GetRoadContactList(linkID, length, SessionInfo.UserSchema);
            return View(RoadContactList);
        }
        #endregion

        #region MyDelegationArrangement
        /// <summary>
        /// Get delegation records.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchArrangName"></param>
        /// <returns></returns>
        [AuthorizeUser(Roles = "12003")]
        public ActionResult MyDelegationArrangement(int? page, int? pageSize, string searchType, string searchValue,int ? sortOrder = null, int? sortType = null,bool isClear=false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"Structure/MyDelegationArrangement actionResult method started successfully");
                #region Session check
                UserInfo SessionInfo = null;
                int presetFilter = 0;
                sortOrder = sortOrder != null ? (int)sortOrder : 1; //type
                presetFilter = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];


                #endregion

                #region Page access check
                if (!PageAccess.GetPageAccess("12003"))
                {
                    return RedirectToAction("Error", "Home");
                }
                #endregion

                if (isClear)
                {
                    TempData.Remove("searchType");
                    TempData.Remove("searchValue");
                }

                TempData.Remove("EditarrangId");
                TempData.Remove("EditOrganisationId");
                TempData.Remove("EditOrgId");
                TempData.Remove("strucInDelCount");
                TempData.Remove("Mode");
                TempData.Remove("EditFlag");
                #region Seach criteria

                if (!string.IsNullOrEmpty(searchType) && !string.IsNullOrEmpty(searchValue))
                {
                    searchType = Convert.ToString(searchType).Trim();
                    TempData["searchType"] = searchType;

                    searchValue = Convert.ToString(searchValue).Trim();
                    TempData["searchValue"] = searchValue;

                }
                else if (TempData["searchType"] != null || TempData["searchValue"] != null)
                {
                    searchType =TempData["searchType"] != null? Convert.ToString(TempData["searchType"]).Trim():null;
                    searchValue =TempData["searchValue"] != null ? Convert.ToString(TempData["searchValue"]).Trim():null;
                }
                else
                {
                    TempData["searchType"] = null;
                    TempData["searchValue"] = null;
                    if (!string.IsNullOrEmpty(searchType) && !string.IsNullOrEmpty(searchValue))
                    {
                        searchType = Convert.ToString(searchType).Trim();
                        TempData["searchType"] = searchType;

                        searchValue = Convert.ToString(searchValue).Trim();
                        TempData["searchValue"] = searchValue;

                    }
                    else if ((page != null || pageSize != null) && TempData["searchType"] != null && TempData["searchValue"] != null)
                    {
                        searchType = Convert.ToString(TempData["searchType"]).Trim();
                        searchValue = Convert.ToString(TempData["searchValue"]).Trim();
                    }
                    else
                    {
                        TempData["searchType"] = null;
                        TempData["searchValue"] = null;
                    }

                }
                ViewBag.searchtype = searchType;
                ViewBag.searchValue = searchValue;

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

                ViewBag.OrgId = SessionInfo.OrganisationId;

                #endregion

                //clear previous data.
                Session["structureInDelegList"] = null;
                Session["StructureNotDelegated"] = null;
                /*Business logic goes here*/
                TempData["delegationList"] = null;

                List<DelegationList> Delegations = structuredeligationService.GetDelegArrangList(SessionInfo.OrganisationId, pageNumber, (int)pageSize, searchType, searchValue, presetFilter, sortOrder);
                if (Delegations.Count > 0)
                    ViewBag.TotalCount = Convert.ToInt32(Delegations[0].TotalRecordCount);
                else
                    ViewBag.TotalCount = 0;
                var delegarrangAsIPagedList = new StaticPagedList<DelegationList>(Delegations, pageNumber, (int)pageSize, ViewBag.TotalCount);

                #region Keep search criteria
                if (TempData["searchType"] != null && TempData["searchValue"] != null)
                {
                    TempData.Keep("searchType");
                    TempData.Keep("searchValue");
                }

                #endregion
                //
                ViewBag.recordsavemessage = TempData["recordsavemessage"];
                return View("MyDelegationArrangement", delegarrangAsIPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @" - Structure/MyDelegationArrangement,Exception:" + ex);
                return RedirectToAction("Error", "Home");
            }

        }

        #endregion

        #region DeleteDelegationArrangement
        /// <summary>
        /// delete Delegation Arrangement
        /// </summary>
        /// <param name="arrangId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDelegationArrangement(long arrangId)
        {
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json("expire", JsonRequestBehavior.AllowGet);
            }

            else
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            #endregion

            List<StructureContactsList> structureContactList = structuredeligationService.GetStructureContactList(arrangId);

            foreach (StructureContactsList item in structureContactList)
            {

                if (item.OwnerId != SessionInfo.OrganisationId)
                {
                    structuredeligationService.DeleteStructureContact(item);//If arrangement =  A-->B
                }
                else
                {
                    //Have to delete A--B and B---C--structures , if A's arrangId is deleting

                    List<StructureDeleArrList> structureDeleArrList = structuredeligationService.GetStructureDeleArrg(item.StructureCode);
                    foreach (var stritem in structureDeleArrList)
                    {
                        item.ArrangementId = stritem.ArrangementId;
                        structuredeligationService.DeleteStructureContact(item);
                    }

                }


            }

            structuredeligationService.DeleteDelegationArrangement(arrangId);

            return Json(true, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region StructuresInDelegation
        public ActionResult StructuresInDelegation(long arrangId, long organisationId, int OrgFromId, int? page, int? pageSize)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Structure/StructuresInDelegation actionResult method started successfully"));

                UserInfo SessionInfo = null;


                SessionInfo = (UserInfo)Session["UserInfo"];

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

                Session["StructureMode"] = "";

                ViewBag.pageSize = pageSize;
                ViewBag.page = page;
                ViewBag.arrangId = arrangId;
                ViewBag.OrganisationId = organisationId;
                ViewBag.OrgId = OrgFromId;

                if ((int)SessionInfo.OrganisationId != OrgFromId)
                {
                    return RedirectToAction("Error", "Home");
                }


                List<StructureInDelegationList> arrangObjList = structuredeligationService.GetStructuresInDeleg(arrangId, organisationId, pageNumber, (int)pageSize);

                if (arrangObjList.Count > 0)
                {
                    ViewBag.TotalCount = arrangObjList[0].TotalRecordCount;
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }

                var delegarrangAsIPagedList = new StaticPagedList<StructureInDelegationList>(arrangObjList, pageNumber, (int)pageSize, (int)ViewBag.TotalCount);

                return View(delegarrangAsIPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/StructuresInDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ReviewDelegation
        public ActionResult ReviewDelegation(long arrangId, int OrgFromId)
        {
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];

            ViewBag.arrangId = arrangId;

            DelegationList arrangObjList = structuredeligationService.GetArrangement(arrangId, OrgFromId);
            ViewBag.OrgToId = arrangObjList.OrgToId;

            return PartialView("ReviewDelegation", arrangObjList);
        }
        #endregion

       

        #region  StoreDelegationData
        /// <summary>
        /// Store delegation input data
        /// </summary>
        /// <param name="delegationList">DelegationList mode</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StoreDelegationData(DelegationList delegationList)
        {
            if (delegationList.SelectedTypeName == "Yes")
            {
                delegationList.AcceptFailure = 1;
                delegationList.SelectedType = AcceptType.Yes;
            }
            else
            {
                delegationList.AcceptFailure = 0;
                delegationList.SelectedType = AcceptType.No;
            }
            TempData["delegationList"] = delegationList;
            TempData.Keep("delegationList");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// Get organisation list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="SearchString"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrganisationList(int page = 1, int pageSize = 10, string SearchString = null, string save = "false")
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
                #endregion
                #region Page access check
                if (!PageAccess.GetPageAccess("12003"))
                {
                    return RedirectToAction("Error", "Home");
                }
                #endregion


                if (ModelState.IsValid)
                {
                    #region Paging Part                 

                    int pageNumber = (int)page;
                    ViewBag.searchFlag = SearchString;
                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;



                    #endregion

                    List<DelegationList> Organisationlist = null;

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
                        SearchString = SearchString.Trim();
                        ViewBag.OrgSearchString = SearchString;
                    }

                    Organisationlist = structuredeligationService.GetOrganisationList((int)pageNumber, (int)pageSize, SearchString);
                    if (Organisationlist.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(Organisationlist[0].TotalRecordCount);
                        IPagedList<DelegationList> model = Organisationlist.ToPagedList((int)page, 10);
                        ViewBag.TotalPages = model.PageCount;


                    }
                    else
                    {
                        ViewBag.TotalCount = 0;
                        ViewBag.TotalPages = 0;
                    }
                    var OrganisationListAsIPagedList = new StaticPagedList<DelegationList>(Organisationlist, pageNumber, (int)pageSize, ViewBag.TotalCount);

                    return Json(OrganisationListAsIPagedList, "application/json", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/OrganisationList, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/OrganisationList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }


        #region CreateDelegation
        /// <summary>
        /// Return intial create delegation data.
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateDelegation(string Flag = null)
          {
            if (Flag == "Create")
            {
                Session["StructuresPageDetails"] = null;
            }

            //clear previouse search data.
            DelegationStructuresList PaginationStructureList = new DelegationStructuresList();
            Session["g_StructSearch"] = null;
            Session["g_NotDelegatedStructSearch"] = null;
            #region Session check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            #endregion
            try
            {

                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Structure/CreateDelegation actionResult method started successfully"));
                string SelectedTypeDynamic = string.Empty;
                if (TempData["delegationList"] != null)
                {
                    DelegationList ContactSelectedtype = new DelegationList();
                    ContactSelectedtype = (DelegationList)TempData["delegationList"];
                    TempData.Keep("delegationList");
                    SelectedTypeDynamic = ContactSelectedtype.ContactType;
                }

                string edOrgID = (TempData["EditOrgId"] != null) ? TempData["EditOrgId"].ToString() : string.Empty;

                TempData.Keep("EditOrgId");

                ViewBag.OrgId = (string.IsNullOrEmpty(edOrgID)) ? null : edOrgID;

                DelegationList delegationList = new DelegationList();
                List<SelectListItem> ContactTypeList = new List<SelectListItem>();
                SelectListItem selectedItem = new SelectListItem();
                selectedItem.Text = "Use default contact";
                selectedItem.Value = "default";
                if (SelectedTypeDynamic != "")
                {
                    if (SelectedTypeDynamic == "default")
                    {
                        selectedItem.Selected = true;
                    }
                }
                ContactTypeList.Add(selectedItem);

                selectedItem = new SelectListItem();
                selectedItem.Text = "Search new contact";
                selectedItem.Value = "new";
                if (SelectedTypeDynamic != "")
                {
                    if (SelectedTypeDynamic == "new")
                    {
                        selectedItem.Selected = true;
                    }
                }
                ContactTypeList.Add(selectedItem);
                delegationList.SelectedType = AcceptType.Yes;

                delegationList.ContactTypeList = ContactTypeList;
                if (TempData["delegationList"] != null)
                {
                    delegationList = (DelegationList)TempData["delegationList"];
                    TempData.Keep("delegationList");
                    if (delegationList != null)
                    {
                        if (delegationList.ArrangementId > 0)
                        {
                            ViewBag.arrangId = delegationList.ArrangementId;
                        }
                        else
                        {
                            long EditArrId = (TempData["EditarrangId"] != null) ? Convert.ToInt64(TempData["EditarrangId"]) : 0;
                            TempData.Keep("EditarrangId");

                            delegationList.ArrangementId = EditArrId;
                            ViewBag.arrangId = delegationList.ArrangementId;
                        }

                        ViewBag.OrganisationId = (delegationList != null) ? delegationList.OrganisationId : 0;
                    }

                }
                delegationList.ContactTypeList = ContactTypeList;
                int Structpage = 0;
                if (Session["StructuresPageDetails"] != null)
                {
                   

                    DelegationStructuresList PaginationStructureListClone = new DelegationStructuresList();
                        DelegationStructuresList PaginationStructureListClone1 = new DelegationStructuresList();
                        PaginationStructureListClone = (DelegationStructuresList)Session["StructuresPageDetails"];//[StructuresPageDetails]
                                                                                                                  //
                        PaginationStructureList = PaginationStructureListClone;
                        if ((List<StructureInDelegationList>)Session["StructuresMAINList"] != null)
                        {
                            PaginationStructureListClone1.StructuresGridToShow = (List<StructureInDelegationList>)Session["StructuresMAINList"];
                            if (PaginationStructureListClone1.StructuresGridToShow.Count != 0)
                            {
                                PaginationStructureList.StructuresGridToShow = (List<StructureInDelegationList>)Session["StructuresMAINList"];
                            }
                        }
                        PaginationStructureList.CreateDelegationList = delegationList;
                        if(PaginationStructureList.StructuresGridToShow!=null && PaginationStructureList.StructuresGridToShow.Any())
                            Structpage =Convert.ToInt32( PaginationStructureList.StructuresGridToShow[0].TotalRecordCount);
                   

                }
                else
                {
                    PaginationStructureList.CreateDelegationList = delegationList;
                }

                List<DelegationStructuresList> DelegationStructList = new List<DelegationStructuresList>();
                DelegationStructList.Add(PaginationStructureList);
                TempData.Keep("strucInDelCount");
                var DelegationIPagedList = (StaticPagedList<DelegationStructuresList>)null;
                var DelegationIPagedList2 = (StaticPagedList<DelegationStructuresList>)null;
                if (PaginationStructureList.PageNumber > 0 && PaginationStructureList.TotalCount > 0)//if the page number is >0 and there are indeed structures in the delegation.
                {
                    DelegationIPagedList = new StaticPagedList<DelegationStructuresList>(DelegationStructList, PaginationStructureList.PageNumber, PaginationStructureList.StructurePerPageCount, PaginationStructureList.TotalCount);
                  
               
                }
                else
                {
                    DelegationIPagedList = new StaticPagedList<DelegationStructuresList>(DelegationStructList, 1, 1, 1);
                    // TempData["strucInDelCount"] = 0;
                }

               

                if (TempData["PageCount"] != null)
                {
                    if (TempData["PageCount"].ToString() != "0")
                    {
                        ViewBag.TotalCount = TempData["PageCount"].ToString();
                    }
                    else
                    {
                        ViewBag.TotalCount = Structpage.ToString();
                        TempData["PageCount"] = Structpage.ToString();
                    }

                }
                else
                {
                    ViewBag.TotalCount = Structpage.ToString();
                    TempData["PageCount"] = Structpage.ToString();
                }


               
                
                //TempData["delegationList"] = DelegationIPagedList;
                TempData.Keep("delegationList");
                TempData.Keep("PageCount");
                
                return View("CreateDelegation", DelegationIPagedList);//CreateDelegation TestDelegation
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/CreateDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region EditDelegation
        /// <summary>
        /// Return intial create delegation data.
        /// </summary>
        /// <returns></returns>
        public ActionResult EditDelegation(long arrangId, long organisationId, int OrgFromId, int? page=1, int? pageSize=10)

        {
           
          
            List<StructureInDelegationList> structureInDelegList2 = null;
            structureInDelegList2 = (List<StructureInDelegationList>)Session["structureInDelegList"];//Loading 
            //clear previouse search data.   
            DelegationStructuresList PaginationStructureList = new DelegationStructuresList();
            Session["g_StructSearch"] = null;
            Session["g_NotDelegatedStructSearch"] = null;
            #region Session check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion

            #region Page sessions
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
            ViewBag.page = page;
            #endregion

            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Structure/EditDelegation actionResult method started successfully"));



                //int pageNumber = 1;

                //Session["PageSize"] = 10;

                TempData["EditarrangId"] = arrangId;
                TempData["EditOrganisationId"] = organisationId;
                TempData["EditOrgId"] = OrgFromId;

                //RM#3945
                if (arrangId > 0)
                {
                    List<StructureInDelegationList> arrangObjPagingList = new List<StructureInDelegationList>();
                    List<StructureInDelegationList> arrangObjList = new List<StructureInDelegationList>();
                    if (Session["structureInDelegList"] == null)
                    {

                        arrangObjPagingList = structuredeligationService.GetStructuresInDeleg(arrangId, organisationId, pageNumber, (int)pageSize);
                        arrangObjList = structuredeligationService.GetStructuresInDeleg(arrangId, organisationId, 1, Int32.MaxValue);
                    }
                    else
                    {
                        List<StructureInDelegationList> SessionPagingList = (List<StructureInDelegationList>)Session["structureInDelegList"];
                        IPagedList<StructureInDelegationList> StructureFilterByPage = SessionPagingList.ToPagedList((int)pageNumber, (int)pageSize);

                        arrangObjPagingList = (List<StructureInDelegationList>)StructureFilterByPage.ToList();
                        arrangObjList = (List<StructureInDelegationList>)Session["structureInDelegList"];
                    }


                    if (arrangObjPagingList.Count > 0)
                    {
                        PaginationStructureList.TotalCount = Convert.ToInt16(arrangObjList.Count);// Added
                        PaginationStructureList.StructurePerPageCount = Convert.ToInt16(arrangObjPagingList.Count);//arrangObjList.Count;
                    }
                    if (arrangObjList.Count > 0)
                    {

                        TempData["strucPageTotalCount"] = arrangObjList.Count;//arrangObjList[0].TOTAL_RECORD_COUNT;

                        ViewBag.TotalCount = arrangObjList.Count;
                        TempData["strucInDelCount"] = arrangObjList.Count;
                    }
                    else
                    {
                        ViewBag.TotalCount = 0;
                    }

                    var delegarrangAsIPagedList = new StaticPagedList<StructureInDelegationList>(arrangObjList, (int)pageNumber, (int)pageSize, (int)ViewBag.TotalCount);

                    Session["structureInDelegList"] = arrangObjList;

                    IPagedList<StructureInDelegationList> model = arrangObjList.ToPagedList((int)pageNumber, (int)pageSize);
                    PaginationStructureList.PageCount = model.PageCount;

                    PaginationStructureList.ArrangementID = arrangId;//Added
                    PaginationStructureList.PageNumber = Convert.ToInt32(pageNumber); // Added
                    PaginationStructureList.OrganisationId = organisationId; // Added
                    PaginationStructureList.OrgFromId = OrgFromId;// Added

                    PaginationStructureList.StructuresGridToShow = (List<StructureInDelegationList>)Session["structureInDelegList"];//arrangObjPagingList;


                    //Session["structureInDelegationPagingList"] = arrangObjPagingList;

                    PaginationStructureList.StructuresStaticPageList = new StaticPagedList<StructureInDelegationList>(arrangObjPagingList, (int)pageNumber, (int)pageSize, (int)ViewBag.TotalCount);

                }

                string orgid = organisationId.ToString();

                if (!string.IsNullOrEmpty(orgid))
                {
                    TempData["orgid"] = orgid;
                }
                TempData["Mode"] = "Edit";

                TempData["orgID"] = orgid;

                DelegationList delegationList = new DelegationList();
                List<SelectListItem> ContactTypeList = new List<SelectListItem>();
                SelectListItem selectedItem = new SelectListItem();
                selectedItem.Text = "Use default contact";
                selectedItem.Value = "default";
                ContactTypeList.Add(selectedItem);

                selectedItem = new SelectListItem();
                selectedItem.Text = "Search new contact";
                selectedItem.Value = "new";
                ContactTypeList.Add(selectedItem);
                delegationList.SelectedType = AcceptType.Yes;

                delegationList.ContactTypeList = ContactTypeList;

                //==================================================================
                delegationList = structuredeligationService.GetArrangement(arrangId, OrgFromId);
                //==================================================================


                int defaultContactId = movementsService.GetContactDetailsForDefault(Convert.ToInt32(organisationId));
                    
                int contactId = movementsService.GetContactDetails(Convert.ToInt32(SessionInfo.UserId));

                if (defaultContactId == delegationList.ContactId)
                {
                    delegationList.ContactType = "Default";
                }
                else
                {
                    delegationList.ContactType = "new";
                }

                delegationList.ArrangementId = arrangId;

                delegationList.CopyNotification = (delegationList.RetainNotification == 1) ? true : false;
                delegationList.SubDelegation = (delegationList.AllowSubDelegation == 1) ? true : false;
                delegationList.SelectedTypeName = (delegationList.AcceptFailure == 1) ? "Yes" : "No";
                delegationList.SelectedType = (delegationList.AcceptFailure == 1) ? AcceptType.Yes : AcceptType.No;

                ViewBag.arrangId = arrangId;
                ViewBag.OrgToId = delegationList.OrgToId;
                delegationList.OrganisationId = organisationId;

                if (TempData["delegationList"] != null)
                {
                    delegationList = (DelegationList)TempData["delegationList"];
                    TempData.Keep("delegationList");
                }
                else
                {
                    TempData["delegationList"] = delegationList;
                }
                int pagecount = 0;
                if (arrangId > 0)
                {
                    List<StructureInDelegationList> SelectedRecords = new List<StructureInDelegationList>();
                    DelegationStructuresList PaginationStructureListForEdit = new DelegationStructuresList();

                    Session["StructuresPageDetails"] = PaginationStructureList;
                    PaginationStructureListForEdit = (DelegationStructuresList)Session["StructuresPageDetails"];
                    SelectedRecords = PaginationStructureListForEdit.StructuresGridToShow.ToPagedList((int)pageNumber, (int)pageSize).ToList();
                    Session["StructuresMAINList"] = SelectedRecords;
                    pagecount = PaginationStructureListForEdit.StructuresGridToShow.Count();
                }
                else
                {
                    List<StructureInDelegationList> SelectedRecords = new List<StructureInDelegationList>();
                    DelegationStructuresList PaginationStructureListForEdit = new DelegationStructuresList();
                    DelegationStructuresList PaginationStructureListDetails = new DelegationStructuresList();
                    PaginationStructureListForEdit = (DelegationStructuresList)Session["StructuresPageDetails"];
                    PaginationStructureListForEdit.StructuresGridToShow = structureInDelegList2;

                     SelectedRecords = PaginationStructureListForEdit.StructuresGridToShow.ToPagedList((int)pageNumber, (int)pageSize).ToList();
                    

                    PaginationStructureListDetails.StructuresStaticPageList = new StaticPagedList<StructureInDelegationList>(SelectedRecords, (int)pageNumber, (int)pageSize, Convert.ToInt16(PaginationStructureListForEdit.StructuresGridToShow.Count()));
                    IPagedList<StructureInDelegationList> model = SelectedRecords.ToPagedList((int)pageNumber, (int)pageSize);

                    //PaginationStructureListDetails.PageCount = model.PageCount;
                    PaginationStructureListDetails.PageCount = PaginationStructureListForEdit.PageCount;
                    PaginationStructureListDetails.StructuresGridToShow = PaginationStructureListForEdit.StructuresGridToShow;
                    PaginationStructureListDetails.ArrangementID = Convert.ToInt16(arrangId);
                    PaginationStructureListDetails.OrgFromId = Convert.ToInt16(OrgFromId);
                    PaginationStructureListDetails.OrganisationId = Convert.ToInt16(organisationId);
                    PaginationStructureListDetails.TotalCount = Convert.ToInt16(PaginationStructureListForEdit.StructuresGridToShow.Count());
                    PaginationStructureListDetails.StructurePerPageCount = (int)pageSize;
                    PaginationStructureListDetails.PageNumber = (int)pageNumber;
                    pagecount = Convert.ToInt16(PaginationStructureListForEdit.StructuresGridToShow.Count());
                    Session["StructuresPageDetails"] = null;
                    Session["StructuresMAINList"] = null;
                    Session["StructuresPageDetails"] = PaginationStructureListDetails;
                    Session["StructuresMAINList"] = SelectedRecords;
                }
                TempData["PageCount"] = pagecount;
                TempData.Keep("AddStructureFlag");
                return RedirectToAction("CreateDelegation");

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/EditDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Save Deligation
        /// <summary>
        /// Save selected delegation
        /// </summary>
        /// <param name="savedelegation"></param>
        /// <returns></returns>
        /// <summary>
        /// Save selected delegation
        /// </summary>
        /// <param name="savedelegation"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDelegation(DelegationList savedelegation)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    #region Session Check
                    UserInfo SessionInfo = null;
                    if (Session["UserInfo"] == null)
                    {
                        return Json(3, JsonRequestBehavior.AllowGet); //return RedirectToAction("Login", "Account");
                    }
                    SessionInfo = (UserInfo)Session["UserInfo"];
                    Int64 organisationId = Convert.ToInt64(SessionInfo.OrganisationId);
                    #endregion

                    #region Page access check
                    if (!PageAccess.GetPageAccess("12003"))
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                    ////// Delete old structures and contacts
                    //if (savedelegation.ArrangementId > 0)
                    //{

                    //    structuredeligationService.DeleteStructureEdit(savedelegation.ArrangementId);
                    //}
                    ////-----------------------------------------------------------------------------------------

                    //if use default contact use then make contact id null
                    if (savedelegation.ContactType == "default")
                    {
                        if (savedelegation.ContactId != 0)
                        {
                            savedelegation.ContactId = 0;
                        }
                        if (savedelegation.ContactId == 0)// RM#4547
                        {
                            savedelegation.ContactId = movementsService.GetContactDetailsForDefault(Convert.ToInt32(savedelegation.OrganisationId));// RM#4547
                        }

                    }
                    if (savedelegation.SelectedTypeName == "Yes")
                    {
                        savedelegation.AcceptFailure = 1;
                        savedelegation.SelectedType = AcceptType.Yes;
                    }
                    else
                    {
                        savedelegation.AcceptFailure = 0;
                        savedelegation.SelectedType = AcceptType.No;
                    }

                    DelegationList returnvalue = new DelegationList();

                    DelegationList delegationList = new DelegationList();
                    string subDelegationStructures = string.Empty;

                    if (Session["structureInDelegList"] != null)
                    {
                        List<StructureContactsList> structureContactList = new List<StructureContactsList>();
                        List<StructureInDelegationList> structureInDelegatinoList = (List<StructureInDelegationList>)Session["structureInDelegList"];

                        #region To check Structure is already subdeligated.

                        //A-->B---C is allowed.
                        //A--->D, B--->D, C--D .....not allowed.
                        string delgstruct = "";
                        int structureDelegationstatus = 0;
                        foreach (var item in structureInDelegatinoList)
                        {

                            List<StructureDeleArrList> structureDeleArrList = structuredeligationService.GetStructureDeleArrg(item.StructureReference);

                            foreach (var subitem in structureDeleArrList)
                            {
                                if ((structureDeleArrList.Count >= 2 || subitem.FromOrganisation == SessionInfo.OrganisationId) && subitem.ArrangementId != savedelegation.ArrangementId)
                                {
                                    structureDelegationstatus = 1;

                                    delgstruct = string.Join(",", item.StructureReference);

                                }
                                else
                                {

                                    if (subitem.OwnerId == (int)SessionInfo.OrganisationId && subitem.ArrangementId != savedelegation.ArrangementId && subitem.FromOrganisation == SessionInfo.OrganisationId)
                                    {
                                        structureDelegationstatus = 1;

                                        delgstruct = string.Join(",", item.StructureReference);
                                    }
                                    //else
                                    //{
                                    //    //structureDelegationstatus = 0;

                                    //}
                                }
                            }
                            if (structureDeleArrList.Count == 0 && savedelegation.OrganisationId == SessionInfo.OrganisationId)
                            {
                                structureDelegationstatus = 1;

                                delgstruct = string.Join(",", item.StructureReference);
                            }
                        }
                        if (structureDelegationstatus == 1)
                        {
                            subDelegationStructures = delgstruct;
                            subDelegationStructures = subDelegationStructures.Remove(subDelegationStructures.Length - 1);
                            TempData["subDelegationStructures"] = delgstruct;
                            return Json(1, JsonRequestBehavior.AllowGet);// return RedirectToAction("CreateDelegation");

                        }

                        #endregion


                        returnvalue.StructureInDelegations = (List<StructureInDelegationList>)Session["structureInDelegList"];
                        // Store structure Id and Code details in structureContactList
                        Parallel.For(0, returnvalue.StructureInDelegations.Count(), structureCount =>
                        {
                            int structureIdCount = GetCountOfContacts(structureCount, returnvalue, structureContactList);

                            if (structureIdCount == 0)
                            {
                                // Business logic needs to be written for sub delegation starts here
                                int allowSubDelegation = structuredeligationService.CheckSubDelegationList(returnvalue.StructureInDelegations[structureCount].StructureId, SessionInfo.OrganisationId);
                                // Business logic for sub delegation ends here
                                string isStructureSelected = (string)TempData["NewCheckedNotDelegated"];
                                if (allowSubDelegation == 0 && isStructureSelected == "1")
                                {
                                    subDelegationStructures += returnvalue.StructureInDelegations[structureCount].StructureReference + ", ";
                                }

                                AddContactIfDoesNotExist(structureCount, returnvalue, structureContactList, allowSubDelegation);
                            }
                        });


                        if (subDelegationStructures.Length > 0)
                        {
                            subDelegationStructures = subDelegationStructures.Remove(subDelegationStructures.LastIndexOf(","));
                            TempData["subDelegationStructures"] = subDelegationStructures;
                        }

                       



                        if (subDelegationStructures == string.Empty)


                        {
                            //// Delete old structures and contacts
                            if (savedelegation.ArrangementId > 0)
                            {

                                structuredeligationService.DeleteStructureEdit(savedelegation.ArrangementId);
                            }
                            //-----------------------------------------------------------------------------------------


                            returnvalue = structuredeligationService.ManageDelegationArrangement(savedelegation, organisationId);

                            returnvalue.StructureInDelegations = structureInDelegatinoList;

                            bool result = structuredeligationService.ManageStructureDelegation(returnvalue);

                            returnvalue.StructureInContactList = structureContactList;

                            returnvalue.OrganisationId = SessionInfo.OrganisationId;

                            result = structuredeligationService.ManageDelegationStructureContact(returnvalue);

                            #region Delegation Success mail
                            
                            ContactModel cntDetails = null;
                            string structuredetails = "";
                            if (savedelegation.ContactId != 0)
                            {
                               cntDetails = userService.GetContactInformation(savedelegation.ContactId);
                            }
                            if(cntDetails==null)
                            {
                                cntDetails = new ContactModel();
                            }
                            foreach (var item in structureInDelegatinoList)
                            {
                                structuredetails = structuredetails + "<br />" + item.StructureReference + " - " + item.StructureName;
                            }
                            string mailContent = "Dear "+ cntDetails.FullName + ", of " + savedelegation.OrganisationName + " <br /><br />This is to inform you that ESDAL user "+ SessionInfo.UserName + " of "+ SessionInfo.OrganisationName + " has delegated the following structure to you <br />" + structuredetails + "<br /><br /><br /> ESDAL Helpdesk team<br /> Contact : 0300 470 3733(8am - 6pm Mon - Fri excluding bank holidays)<br /><br /> Kind regards,<br />" + SessionInfo.UserName;
                            string subject = "Structure delegation";
                            string Matter = mailContent;
                            byte[] content = Encoding.ASCII.GetBytes(Matter);
                            byte[] attachment = new byte[0];
                            CommunicationParams communicationParams = new CommunicationParams
                            {
                                UserEmail = cntDetails.Email,
                                Subject = subject,
                                Content = content,
                                Attachment = attachment,
                                ESDALReference = null
                            };
                            communicationService.SendGeneralmail(communicationParams);
                            #endregion

                            Session["structureInDelegList"] = null;
                            Session["structureContactList"] = null;

                            TempData["delegationList"] = null;
                            TempData["recordsavemessage"] = "Delegation arrangement saved successfully.";// save message disply on MyDelegationtionArrangement page.
                            TempData["Mode"] = null;
                            TempData["strucInDelCount"] = null;
                            return Json(0, JsonRequestBehavior.AllowGet);//return RedirectToAction("MyDelegationArrangement");

                        }
                        else
                        {
                            return Json(1, JsonRequestBehavior.AllowGet);// return RedirectToAction("CreateDelegation");
                        }
                    }

                    return Json(0, JsonRequestBehavior.AllowGet); //return RedirectToAction("MyDelegationArrangement");
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/SaveDelegation, Exception: {0}", "Invalid Model State"));
                    return Json(2, JsonRequestBehavior.AllowGet); //return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/SaveDelegation, Exception: {0}", ex));
                return Json(2, JsonRequestBehavior.AllowGet); //return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region ViewDelegationArrangDetails
        //[HttpPost]
        public ActionResult ViewDelegationArrangDetails(long arrangId, int orgId)
        {
            var info = structuredeligationService.GetArrangement(arrangId, orgId);
            return Json(new { Result = info });
        }
        #endregion

        private volatile object lockObj = new object();

        private int GetCountOfContacts(int structureCount, DelegationList returnvalue, List<StructureContactsList> structureContactList)
        {
            lock (lockObj)
            {
                return structureContactList.Where(i => i.StructureId == returnvalue.StructureInDelegations[structureCount].StructureId).Count();
            }
        }

        private void AddContactIfDoesNotExist(int structureCount, DelegationList returnvalue, List<StructureContactsList> structureContactList, int allowSubDelegation)
        {
            lock (lockObj)
            {
                structureContactList.Add(new StructureContactsList()
                {
                    StructureId = returnvalue.StructureInDelegations[structureCount].StructureId,
                    StructureCode = returnvalue.StructureInDelegations[structureCount].StructureReference,
                    AllowSubDelegation = allowSubDelegation
                });
            }
        }

        #region public ActionResult FilterSummary(string structName)
        /// <summary>
        /// ActionResult to display filter summary
        /// </summary>
        /// <param name="structName"></param>
        /// <returns>View</returns>
        public ActionResult FilterSummary(string structName)
        {
            return RedirectToAction("NotImplemented", "Home");
            //return View();
        }
        #endregion      

        #region public ActionResult StructNotDelegatedSummaryFilter()
        /// <summary>
        /// Structure summary filter that goes to the left panel
        /// </summary>
        /// <returns>partial view</returns>
        public ActionResult StructNotDelegatedSummaryFilter()
        {

            var SessionInfo = (UserInfo)Session["UserInfo"];


            string structType = string.Empty;
            string ICAMethod = string.Empty;

            SearchStructures objSearch = new SearchStructures();
            if (Session["g_NotDelegatedStructSearch"] != null)
            {
                objSearch = (SearchStructures)Session["g_NotDelegatedStructSearch"];
                switch (objSearch.StructureType)
                {
                    case "510001":
                        objSearch.StructureType = "Underbridge";
                        break;

                    case "510002":
                        objSearch.StructureType = "Overbridge";
                        break;

                    case "510003":
                        objSearch.StructureType = "Under and over bridge";
                        break;

                    case "510004":
                        objSearch.StructureType = "Level crossing";
                        break;

                }
                structType = objSearch.StructureType;
                ICAMethod = objSearch.ICAMethod;
            }
            /*method to fill struct type dropdown list*/
            GetStructTypeDropDown(structType);
            /*Method to fill ICA Method dropdown*/
            GetICAMethod(ICAMethod);


            List<DelegationArrangment> objDelegationArrangment = structuredeligationService.viewDelegationArrangment((long)SessionInfo.OrganisationId);
            ViewBag.delegateName = new SelectList(objDelegationArrangment, "arrangmentID", "name");

            return PartialView(objSearch);
        }
        #endregion

      

        [HttpPost]
        public JsonResult ShowAgreedApplStructureOnMap(string StructureCode)
        {
            List<StructureInfo> structureInfoList = new List<StructureInfo>();
            structureInfoList = structuresService.AgreedAppStructureInfo(StructureCode);
             return Json(new { result = structureInfoList });
         }


        #region public ActionResult SaveNotDelegatedStructSearch(SearchStruct objSearch)
        public ActionResult SaveNotDelegatedStructSearch(SearchStructures objSearch)
        {
            Session["g_NotDelegatedStructSearch"] = objSearch;
            return RedirectToAction("StructureNotInDelegationList");
        }
        #endregion
  [HttpPost]
        public JsonResult AddStructuresNotDelegated(StructureSummary structureSummary, string orgId, string EditOrgId, string arrId, int? page, int? pageSize)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Structure/AddStructuresNotDelegated actionResult method started successfully"));

                #region Session check
                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                {
                    string actionName = Request.RequestContext.RouteData.GetRequiredString("action");
                    string controllerName = Request.RequestContext.RouteData.GetRequiredString("controller");

                    //return View("Login", "Account");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion

                #region Page access check
                if (!PageAccess.GetPageAccess("12003"))
                {
                    //return RedirectToAction("Error", "Home");
                }
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

                StringBuilder structurecodes = new StringBuilder();
                StringBuilder uncheckstructurecodes = new StringBuilder();
                int structurecodecount = 0;
                int OrganisationId = 0;

                StringBuilder UnChekedStructCodes = new StringBuilder();
                #region Find out uncheck checkbox of structure code
                //Find out uncheck checkbox of structure code
                if (!string.IsNullOrEmpty(structureSummary.PreviousStructCodes))
                {
                    if (!string.IsNullOrEmpty(structureSummary.StructCodes))
                    {
                        string[] previousStuctCodes = Convert.ToString(structureSummary.PreviousStructCodes).Trim().Trim(structureSummary.BindBy.ToArray()).Split(structureSummary.BindBy.ToArray());
                        string[] StuctCodes = Convert.ToString(structureSummary.StructCodes).Trim().Trim(structureSummary.BindBy.ToArray()).Split(structureSummary.BindBy.ToArray());
                        if (previousStuctCodes.Length > 0)
                        {
                            bool uncheckStructCode = false;
                            for (int i = 0; i < previousStuctCodes.Length; i++)
                            {
                                for (int j = 0; j < StuctCodes.Length; j++)
                                {
                                    if (previousStuctCodes[i] == StuctCodes[j])
                                    {
                                        uncheckStructCode = true;
                                        break;
                                    }
                                }
                                if (uncheckStructCode == false)
                                {
                                    uncheckstructurecodes.Append(previousStuctCodes[i] + structureSummary.BindBy);
                                }
                                else
                                {
                                    uncheckStructCode = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        uncheckstructurecodes.Append(structureSummary.PreviousStructCodes.Trim(structureSummary.BindBy.ToArray()));
                    }
                }
                //Find out uncheck checkbox of structure code end
                #endregion
                // Get previouse structure code from session
                if (Session["structureInDelegList"] != null)
                {
                    List<StructureInDelegationList> strucDelListPre = new List<StructureInDelegationList>();
                    strucDelListPre = (List<StructureInDelegationList>)Session["structureInDelegList"];
                    foreach (StructureInDelegationList preList in strucDelListPre)
                    {
                        structurecodes.Append(preList.StructureReference + structureSummary.BindBy);
                    }
                }
                // Get previouse structure code from session end
                //Apend current page check structure code
                if (structureSummary != null)
                {
                    if (!string.IsNullOrEmpty(structureSummary.StructCodes))
                    {
                        structurecodes.Append(Convert.ToString(structureSummary.StructCodes).Trim().Trim(structureSummary.BindBy.ToArray()));
                        structurecodecount = Convert.ToString(structurecodes).Split(structureSummary.BindBy.ToArray()).Length;
                        ViewBag.orgId = OrganisationId = Convert.ToInt16(TempData["orgid"]);
                    }
                }
                //Apend current page check structure code end


                StringBuilder finalStrutCode = new StringBuilder();


                #region  Finaly Remove unchecked structure code for all codes
                // Finaly Remove unchecked structure code for all codes
                if (Convert.ToString(uncheckstructurecodes).Length > 0)
                {
                    //remove uncheck code from structurecodes;
                    string[] unCheckCode = Convert.ToString(uncheckstructurecodes).Trim().Trim(structureSummary.BindBy.ToArray()).Split(structureSummary.BindBy.ToArray());
                    string[] FinalStuctCodes = Convert.ToString(structurecodes).Trim().Trim(structureSummary.BindBy.ToArray()).Split(structureSummary.BindBy.ToArray());
                    int lengthCount = FinalStuctCodes.Length;
                    bool uncheckStructCodeFinal = false;


                    for (int i = 0; i < FinalStuctCodes.Length; i++)
                    {
                        for (int j = 0; j < unCheckCode.Length; j++)
                        {
                            if (FinalStuctCodes[i] == unCheckCode[j])
                            {
                                uncheckStructCodeFinal = true;
                                break;
                            }
                        }
                        if (uncheckStructCodeFinal == false)
                        {
                            finalStrutCode.Append(FinalStuctCodes[i] + structureSummary.BindBy);
                        }
                        else
                        {
                            uncheckStructCodeFinal = false;
                        }
                    }
                }
                else
                {
                    finalStrutCode = structurecodes;
                }
                // Finaly Remove unchecked structure code for all codes end
                #endregion
                if (Convert.ToString(finalStrutCode).Trim(structureSummary.BindBy.ToArray()).Trim().Length > 0)
                {
                    structurecodecount = Convert.ToString(finalStrutCode).Split(structureSummary.BindBy.ToArray()).Length;
                }

                Session["structureInDelegList"] = null;
                /*Business logic goes here*/

                List<StructureInDelegationList> structureInDelegList = null; ;

                if (structurecodecount > 0)
                {
                    structureInDelegList = structuredeligationService.GetStructureInDelegationList(pageNumber, pageSize, Convert.ToString(finalStrutCode).Trim(structureSummary.BindBy.ToArray()), Convert.ToInt32(SessionInfo.OrganisationId), structurecodecount);
                    Session["structureInDelegList"] = structureInDelegList;
                    TempData["strucInDelCount"] = structureInDelegList.Count();
                }
                else
                {
                    //Session["structureContactList"] = null;
                    Session["structureInDelegList"] = null;
                    TempData["strucInDelCount"] = 0;
                }
                TempData.Keep("orgid");
                TempData.Keep("strucInDelCount");
                TempData.Keep("delegationList");

                // Start Code to show structures as per pagesize.
                DelegationStructuresList PaginationStructureList = new DelegationStructuresList();
                Session["StructuresPageDetails"] = null;

                if (Convert.ToInt32(arrId) > 0)
                {

                    PaginationStructureList.StructuresStaticPageList = new StaticPagedList<StructureInDelegationList>(structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize), (int)pageNumber, (int)pageSize, Convert.ToInt16(TempData["strucInDelCount"]));

                    IPagedList<StructureInDelegationList> model = structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize);
                    PaginationStructureList.PageCount = model.PageCount;
                    PaginationStructureList.StructuresGridToShow = structureInDelegList;
                    //}
                    PaginationStructureList.ArrangementID = Convert.ToInt16(arrId);
                    PaginationStructureList.OrgFromId = Convert.ToInt16(orgId);
                    PaginationStructureList.OrganisationId = Convert.ToInt16(EditOrgId);
                    PaginationStructureList.TotalCount = Convert.ToInt16(TempData["strucInDelCount"]);
                    PaginationStructureList.StructurePerPageCount = (int)pageSize;
                    PaginationStructureList.PageNumber = (int)pageNumber;

                    Session["StructuresPageDetails"] = PaginationStructureList;
                    Session["StructuresMAINList"] = structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize).ToList();
                }
                else
                {
                    PaginationStructureList.StructuresStaticPageList = new StaticPagedList<StructureInDelegationList>(structureInDelegList, (int)pageNumber, (int)pageSize, Convert.ToInt16(TempData["strucInDelCount"]));
                    IPagedList<StructureInDelegationList> model = structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize);
                    PaginationStructureList.PageCount = model.PageCount;
                    PaginationStructureList.StructuresGridToShow = structureInDelegList;

                    PaginationStructureList.ArrangementID = Convert.ToInt16(arrId);
                    PaginationStructureList.OrgFromId = Convert.ToInt16(orgId);
                    PaginationStructureList.OrganisationId = Convert.ToInt16(EditOrgId);
                    PaginationStructureList.TotalCount = Convert.ToInt16(TempData["strucInDelCount"]);
                    PaginationStructureList.StructurePerPageCount = (int)pageSize;
                    PaginationStructureList.PageNumber = (int)pageNumber;

                    Session["StructuresPageDetails"] = PaginationStructureList;
                    Session["StructuresMAINList"] = structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize).ToList();
                }
                //End


                if (structureSummary.ButtonName == "save")
                {
                    TempData["Mode"] = null;
                }

                if (Session["structureInDelegList"] == null)
                {
                    return Json("invalid", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("valid", JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/AddStructure, Exception: {0}", ex));
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #region public ActionResult StructureNotInDelegationList(int? page, int? pageSize, string Mode, string orgid)
        /// <summary>
        /// ActionResult to display Structure List view
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="searchString">search string</param>
        /// <returns>View</returns>
        [AuthorizeUser(Roles = "12001")]
        public ActionResult StructureNotInDelegationList(int? page, int? pageSize, string Mode, string orgid, string arrId, string EditOrgId, string flag = null, int? sortOrder = null, int? sortType = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Structure/StructureNotInDelegationList actionResult method started successfully"));

                #region Session check
                UserInfo SessionInfo = null;
                Session["StructureMode"] = null;
                if (Session["UserInfo"] == null)
                {


                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion

                #region Page access check
                if (!PageAccess.GetPageAccess("12003"))
                {
                    return RedirectToAction("Error", "Home");
                }
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
               sortOrder = sortOrder != null ? (int)sortOrder : 2; //name
                sortType = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;

                ViewBag.SortType = sortType;

                
                #region Search criteria
                SearchStructures objSearch = new SearchStructures();
                if (Session["g_NotDelegatedStructSearch"] != null)
                {
                    objSearch = (SearchStructures)Session["g_NotDelegatedStructSearch"];
                }
                #endregion
                ViewBag.filterObject = objSearch;
                if (!string.IsNullOrEmpty(arrId))
                    TempData["arrId"] = arrId;

                if (!string.IsNullOrEmpty(EditOrgId))
                    TempData["EditOrgId"] = EditOrgId;

                if (!string.IsNullOrEmpty(orgid))
                    TempData["orgid"] = orgid;

                if (!string.IsNullOrEmpty(Mode))
                    TempData["Mode"] = Mode;

                TempData.Keep("Mode");

                List<StructureSummary> summaryObjList = structuredeligationService.GetNotDelegatedStructureListSearch(Convert.ToInt32(SessionInfo.OrganisationId), pageNumber, (int)pageSize, objSearch, (int)sortOrder,(int) sortType );
                if (summaryObjList.Count > 0)
                {
                    ViewBag.TotalCount = Convert.ToInt32(summaryObjList[0].TotalRecordCount);
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }
                var structureSummaryAsIPagedList = new StaticPagedList<StructureSummary>(summaryObjList, pageNumber, (int)pageSize, ViewBag.TotalCount);
                //check session value and change check change
                if (Session["StructureNotDelegated"] != null)
                {

                    List<StructureInDelegationList> strucDelListCheck = (List<StructureInDelegationList>)Session["StructureNotDelegated"];
                    foreach (var item in strucDelListCheck)
                    {
                        structureSummaryAsIPagedList.Where(s => s.StructureId == item.StructureId).ToList().TrueForAll(s => s.StructCodeSelected = true);
                    }
                }

                TempData.Keep("arrId");
                TempData.Keep("EditOrgId");
                TempData.Keep("orgid");
                TempData.Keep("delegationList");
                return View("StructuresNotInDelegation", structureSummaryAsIPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/StructureList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion


        [HttpPost]
        public JsonResult AddStructure(StructureSummary structureSummary, string orgId, string EditOrgId, string arrId, int? page, int? pageSize, string flag = null)
        {

            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Structure/AddStructure actionResult method started successfully"));

                #region Session check
                UserInfo SessionInfo = null;

                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion

                #region Page access check
                if (!PageAccess.GetPageAccess("12003"))
                {

                }
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
                //TempData["EditFlag"] = "1";//For setting check box in deligation filter;
                //if flag is not delegated then identify the structures that are not delegated by the delegator
                if (flag != null && flag.ToLower() == "notdelegated")
                {
                    ExtractNonDelegatedToSession(structureSummary, SessionInfo);
                }

                int structurecodecount = 0;
                int OrganisationId = 0;

                List<string> previousStuctCodes = new List<string>();//codes from previous session, or page that are checked
                List<string> newCheckedStuctCodes = new List<string>();//newly checked ones
                List<string> uncheckedStructCodes = new List<string>();//ones that are unchecked
                List<string> structureInDelegation = new List<string>();//codes that are already in delegation

                #region Find out uncheck checkbox of structure code
                //Find out uncheck checkbox of structure code identify structure codes that are unchecked to be removed from the delegation list.
                if (!string.IsNullOrEmpty(structureSummary.PreviousStructCodes))//the previous list of checked ones
                {
                    previousStuctCodes = GetStructCodeArray(structureSummary.PreviousStructCodes, Convert.ToChar(structureSummary.BindBy)).ToList();
                    if (!string.IsNullOrEmpty(structureSummary.StructCodes))//if this isnt empty then there are few which are checked, isolate the ones that are unchecked comparing it with previous list.
                    {
                        newCheckedStuctCodes = GetStructCodeArray(structureSummary.StructCodes, Convert.ToChar(structureSummary.BindBy)).ToList();
                        if (previousStuctCodes.Count > 0)//expecting more than one were previously checked
                        {
                            //identify the ones that are unchecked or removed from the list, 
                            //count 0 indicates all are included or new ones are added within the page.
                            uncheckedStructCodes = previousStuctCodes.Where(x => !newCheckedStuctCodes.Contains(x)).ToList();//get the previous ones that arent there in the current list of checked ones
                        }
                    }
                    else
                    {
                        uncheckedStructCodes.AddRange(previousStuctCodes);//add all struct codes that were previously checked but are no longer checked
                    }
                }
                //Find out uncheck checkbox of structure code end
                #endregion
                // Get previouse structure code from session here all delegated structures is fetched
                if (Session["structureInDelegList"] != null)
                {
                    List<StructureInDelegationList> strucDelListPre = new List<StructureInDelegationList>();
                    strucDelListPre = (List<StructureInDelegationList>)Session["structureInDelegList"];
                    structureInDelegation = strucDelListPre.Select(x => x.StructureReference).ToList();
                }
                //Get previouse structure code from session end
                //Apend current page check structure code
                if (structureSummary != null)
                {
                    if (!string.IsNullOrEmpty(structureSummary.StructCodes))//if there are structures checked
                    {
                        if (newCheckedStuctCodes.Count > 0)
                        {
                            AddCheckedToExistingDelList(newCheckedStuctCodes, structureInDelegation);
                        }
                        else
                        {
                            newCheckedStuctCodes = GetStructCodeArray(structureSummary.StructCodes, Convert.ToChar(structureSummary.BindBy)).ToList();
                            AddCheckedToExistingDelList(newCheckedStuctCodes, structureInDelegation);
                        }
                        ViewBag.orgId = OrganisationId = Convert.ToInt16(TempData["orgid"]);
                        //ViewBag.orgId = SessionInfo.OrganisationId;//edited --susmitha
                        //TempData["orgid"]= SessionInfo.OrganisationId;
                    }
                }
                //Apend current page check structure code end


                StringBuilder finalStrutCode = new StringBuilder();


                #region  Finaly Remove unchecked structure code for all codes
                // Finaly Remove unchecked structure code for all codes
                if (uncheckedStructCodes.Count > 0)
                {
                    structureInDelegation.RemoveAll(x => uncheckedStructCodes.Contains(x));
                }
                // Finaly Remove unchecked structure code for all codes end
                #endregion

                TempData["NewCheckedNotDelegated"] = newCheckedStuctCodes.SequenceEqual(structureInDelegation) ? "0" : "1";

                Session["structureInDelegList"] = null;
                /*Business logic goes here*/
                //For delegation search checkbox
                List<StructureInDelegationList> structureInDelegList = null; ;

                if (structureInDelegation.Count > 0)
                {
                    structureInDelegList = structuredeligationService.GetStructureInDelegationList(structureInDelegation.ToArray(), Convert.ToInt32(SessionInfo.OrganisationId));
                    Session["structureInDelegList"] = structureInDelegList;
                    TempData["strucInDelCount"] = structureInDelegList.Count();

                }
                else
                {
                    Session["structureInDelegList"] = null;
                    TempData["strucInDelCount"] = 0;
                }

                TempData.Keep("orgid");
                TempData.Keep("strucInDelCount");
                TempData.Keep("delegationList");
                TempData.Keep("orgid");
                TempData.Keep("EditFlag");

                // Start Code to show structures as per pagesize.
                DelegationStructuresList PaginationStructureList = new DelegationStructuresList();
                Session["StructuresPageDetails"] = null;

                if (Convert.ToInt32(arrId) > 0)
                {

                    PaginationStructureList.StructuresStaticPageList = new StaticPagedList<StructureInDelegationList>(structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize), (int)pageNumber, (int)pageSize, Convert.ToInt16(TempData["strucInDelCount"]));

                    IPagedList<StructureInDelegationList> model = structureInDelegList.ToPagedList(pageNumber, (int)pageSize);
                    PaginationStructureList.PageCount = model.PageCount;
                    PaginationStructureList.StructuresGridToShow = structureInDelegList;

                    PaginationStructureList.ArrangementID = Convert.ToInt16(arrId);
                    PaginationStructureList.OrgFromId = Convert.ToInt16(orgId);
                    PaginationStructureList.OrganisationId = Convert.ToInt16(EditOrgId);
                    PaginationStructureList.TotalCount = Convert.ToInt16(TempData["strucInDelCount"]);
                    PaginationStructureList.StructurePerPageCount = (int)pageSize;
                    PaginationStructureList.PageNumber = pageNumber;

                    Session["StructuresPageDetails"] = PaginationStructureList;
                    Session["StructuresMAINList"] = structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize).ToList();
                }
                else
                {
                    PaginationStructureList.StructuresStaticPageList = new StaticPagedList<StructureInDelegationList>(structureInDelegList, (int)pageNumber, (int)pageSize, Convert.ToInt16(TempData["strucInDelCount"]));
                    IPagedList <StructureInDelegationList> model = structureInDelegList.ToPagedList((int)pageNumber, (int)pageSize);
                    PaginationStructureList.PageCount = model.PageCount;
                    PaginationStructureList.StructuresGridToShow = structureInDelegList;

                    PaginationStructureList.ArrangementID = Convert.ToInt16(arrId);
                    PaginationStructureList.OrgFromId = Convert.ToInt16(orgId);
                    PaginationStructureList.OrganisationId = Convert.ToInt16(EditOrgId);
                    PaginationStructureList.TotalCount = Convert.ToInt16(TempData["strucInDelCount"]);
                    PaginationStructureList.StructurePerPageCount = (int)pageSize;
                    PaginationStructureList.PageNumber = pageNumber;
                    Session["StructuresPageDetails"] = PaginationStructureList;
                    Session["StructuresPageDetails1"] = PaginationStructureList;
                    Session["StructuresMAINList"] = structureInDelegList.ToPagedList(pageNumber, (int)pageSize).ToList();
                }


                if (structureSummary.ButtonName == "save")
                {
                    TempData["Mode"] = null;
                }

                if (Session["structureInDelegList"] == null)
                {
                   
                    return Json("invalid", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["AddStructureFlag"] = "1";
                    return Json("valid", JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/AddStructure, Exception: {0}", ex));
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private static void AddCheckedToExistingDelList(List<string> newCheckedStuctCodes, List<string> structureInDelegation)
        {
            var newCheckedNotInDeleg = newCheckedStuctCodes.Where(x => !structureInDelegation.Contains(x)).ToList();
            structureInDelegation.AddRange(newCheckedNotInDeleg);//add the ones that arent in delegation but are newly checekd
        }

        /// <summary>
        /// function to generate struct array from structure code
        /// </summary>
        /// <param name="structureCodes"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private string[] GetStructCodeArray(string structureCodes, char v)
        {
            try
            {
                return structureCodes.Trim(v).Split(v);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structure/AddStructure, Exception: {0}", ex));
                throw ex;
            }
        }

        private void ExtractNonDelegatedToSession(StructureSummary structureSummary, UserInfo SessionInfo)
        {
            string[] nonDelegatedStructCodes = structureSummary.StructCodes.Split(',');
            List<StructureInDelegationList> SelectedStructureList = structuredeligationService.GetStructureInDelegationList(nonDelegatedStructCodes, (int)SessionInfo.OrganisationId);
            Session["StructureNotDelegated"] = SelectedStructureList;
        }

        #region SearchDelegation
        [HttpGet]
        public ActionResult SearchDelegation()
        {
            if (TempData["searchArrangName"] != null)
            {
                ViewBag.SearchArrangName = TempData["searchArrangName"];
            }

            DelegationList DL = new DelegationList();

            if (TempData["searchType"] != null)
            {
                DL.SearchType = TempData["searchType"].ToString();
                TempData.Keep("searchType");
            }
            if (TempData["searchValue"] != null)
            {
                DL.SearchValue = TempData["searchValue"].ToString();
                TempData.Keep("searchValue");
            }
            return PartialView("SearchDelegation", DL);
        }
        #endregion

        #region GetStructureOwner
        [AuthorizeUser(Roles = "13001,12000,12001,12002,12003,12005,40003,40004,100001")]  // Added for 8389 regression since this method is used in haulier portal
        public ActionResult GetStructureOwner(long StructureId = 0, long OrganisationId = 0)
        {
            int result = 0;
            result = structuresService.GetStructureOwner(StructureId, OrganisationId);
            return Json(new { result });
        }
        #endregion

        #region public ActionResult UnsuitableStructureSummary(long structureId)
        [AuthorizeUser(Roles = "13001,12000,12001,12002,12003,12005,40003,40004,100001")] // Added for 8389 regression since this method is used in haulier portal
        public ActionResult UnsuitableStructureSummary(long structureId, long route_part_id, long section_id, string cont_ref_num = "",bool isRouteAssessment=false)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var SessionInfo = (UserInfo)Session["UserInfo"];
            int userTypeID = SessionInfo.UserTypeId;
            int chkValidStructCnt = 1;
            int userValidateFlag = 1;

            StructureDetailsModel objStructureModel = null;
            List<StructureGeneralDetails> objListStructureGeneralDetails = null;
            List<StructureSectionList> objListStructureSection = null;
            List<StructureSectionList> objUpdatedStructureSection = new List<StructureSectionList>();
            List<AffStructureGeneralDetails> objDetailList = null;

            if (chkValidStructCnt > 0)
            {
                objStructureModel = new StructureDetailsModel();
                objListStructureGeneralDetails = structuresService.ViewGeneralDetails(structureId);
                if (isRouteAssessment)
                {
                    objListStructureSection = structuredeligationService.viewUnsuitableStructSections(structureId, route_part_id, section_id, cont_ref_num);
                    var objStructureSection = objListStructureSection.GroupBy(item => item.SectionId).ToList();

                    var i = 0;
                    foreach (var item in objStructureSection)
                    {
                        var listObj = item.ToList();
                        string VehicleList = "";
                        foreach (var list in listObj)
                        {
                            if (list.AffectFlag != 0)
                            {
                                if (VehicleList != "")
                                {
                                    VehicleList += ", ";
                                }
                                VehicleList += list.VehicleName;
                            }
                        }
                        //var vehicleList
                        objUpdatedStructureSection.Add(listObj[0]);
                        objUpdatedStructureSection[i].VehicleName = VehicleList;
                        i++;
                    }
                    objListStructureSection = objUpdatedStructureSection;
                }

                else
                {
                    objListStructureSection = structuresService.ViewStructureSections(structureId);
                }
                objDetailList = new List<AffStructureGeneralDetails>();

                if (objListStructureGeneralDetails != null)
                {
                    objDetailList = applicationService.GetStructureDetailList(objListStructureGeneralDetails[0].ESRN, 0);//2nd parameter is section ID used to fetch signed structure c onstraint
                }
            }
            else
            {
                userValidateFlag = 0;
            }

            ViewBag.StructureId = structureId; 
            ViewBag.SectionID = section_id;

            ViewBag.ListStructureGeneralDetails = objListStructureGeneralDetails;
            ViewBag.ListStructureSections = objListStructureSection;
            ViewBag.ListStructureOwner = objListStructureGeneralDetails;
            ViewBag.structureOwnerchain = objDetailList;
            ViewBag.UserValidateFlag = userValidateFlag;
            return PartialView();
        }
        #endregion
        #region public ActionResult ReviewSummary(int structureId)
        [AuthorizeUser(Roles = "13001,12000,12001,12002,12003,12005,40003,40004,100001")] // Added for 8389 regression since this method is used in haulier portal
        public ActionResult ReviewSummarypopup(long structureId)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var SessionInfo = (UserInfo)Session["UserInfo"];
            int userTypeID = SessionInfo.UserTypeId;
            int chkValidStructCnt = 1;
            int userValidateFlag = 1;

            StructureDetailsModel objStructureModel = null;
            List<StructureGeneralDetails> objListStructureGeneralDetails = null;
            List<StructureSectionList> objListStructureSection = null;
            List<AffStructureGeneralDetails> objDetailList = null;

            if (chkValidStructCnt > 0)
            {
                objStructureModel = new StructureDetailsModel();
                objListStructureGeneralDetails = structuresService.ViewGeneralDetails(structureId);
                objListStructureSection = structuresService.ViewStructureSections(structureId);

                objDetailList = new List<AffStructureGeneralDetails>();

                if (objListStructureGeneralDetails != null)
                {
                    objDetailList = applicationService.GetStructureDetailList(objListStructureGeneralDetails[0].ESRN, 0);//2nd parameter is section ID used to fetch signed structure c onstraint
                }
            }
            else
            {
                userValidateFlag = 0;
            }

            ViewBag.StructureId = structureId; 

            ViewBag.ListStructureGeneralDetails = objListStructureGeneralDetails;
            ViewBag.ListStructureSections = objListStructureSection;
            ViewBag.ListStructureOwner = objListStructureGeneralDetails;
            ViewBag.structureOwnerchain = objDetailList;
            ViewBag.UserValidateFlag = userValidateFlag;
            return PartialView();
        }
        #endregion


        public ActionResult AffectedstructureSummaryPopup(string structurecode, int sectionId)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var SessionInfo = (UserInfo)Session["UserInfo"];

           
            int userValidateFlag = 1;
            int structureId = 0;
            List<StructureGeneralDetails> objListStructureGeneralDetails = null;
            List<StructureSectionList> objListStructureSection = null;
            List<AffStructureGeneralDetails> objDetailList = null;
            structureId = structuresService.GetStructureId(structurecode);
            
            objListStructureGeneralDetails = structuresService.ViewGeneralDetails(structureId);
            objListStructureSection = structuresService.ViewStructureSections(structureId);
            //objListStructureSection = structuresService.ViewStructureSections(structureId);

            objDetailList = new List<AffStructureGeneralDetails>();

            if (objListStructureGeneralDetails != null && objListStructureGeneralDetails.Count != 0)
            {
                //var sectionId = objListStructureSection != null ? (int)objListStructureSection[0].SectionId : 0;
                objDetailList = applicationService.GetStructureDetailList(objListStructureGeneralDetails[0].ESRN, sectionId);//2nd parameter is section ID used to fetch signed structure c onstraint
            }
            

            ViewBag.SectionId = sectionId;
            ViewBag.StructureId = structureId;
            ViewBag.ESRN = objListStructureGeneralDetails != null && objListStructureGeneralDetails.Count != 0 ? objListStructureGeneralDetails[0].ESRN:"";
            ViewBag.Structurecode = structurecode;
            ViewBag.ListStructureGeneralDetails = objListStructureGeneralDetails;
           // ViewBag.ListStructureSections = objListStructureSection;
            ViewBag.ListStructureOwner = objListStructureGeneralDetails;
            ViewBag.structureOwnerchain = objDetailList;
            ViewBag.UserValidateFlag = userValidateFlag;
            ViewBag.grossweight = objDetailList[0].SignedGrossWeight;
            return PartialView();
        }

        public ActionResult ShowRoadDelegation(int? page, int? pageSize, int? sortOrder = null, int? sortType = null)
        {
            try
            {
                #region Session Check
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (!PageAccess.GetPageAccess("12005"))
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                int presetFilter = 0;
                sortOrder = sortOrder != null ? (int)sortOrder : 1; //type
                 presetFilter = sortType != null ? (int)sortType : 0; // asc
                //ViewBag.sortOrder = sortOrder == null ? null : sortOrder;
                //presetFilter = sortType != null ? (int)sortType : presetFilter;
                ViewBag.sortOrder = sortOrder;
                ViewBag.sortType = presetFilter;
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

                List<RoadDelegationList> onerousVehicleList = structuredeligationService.GetRoadDelegationList(pageNumber, (int)pageSize, SessionInfo.OrganisationId, presetFilter, sortOrder);

                if (onerousVehicleList.Count > 0)
                {
                    ViewBag.TotalCount = onerousVehicleList[0].TotalRecordCount;
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }

                var pagedRoadDelegationList = new StaticPagedList<RoadDelegationList>(onerousVehicleList, pageNumber, (int)pageSize, (int)ViewBag.TotalCount);
                return View("RoadDelegationListSOA", pagedRoadDelegationList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/ShowRoadDelegation, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }





        [AuthorizeUser(Roles = "13001,12000,12001,12002,12003,12005,40003,40004,100001")] // Added for 8389 regression since this method is used in haulier portal
        public ActionResult ReviewStructureSectionImposedConstraints(int structureId, int sectionId, string sectionType)
        {
            
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            UserInfo ObjUserInfo = (UserInfo)Session["UserInfo"];
            ViewBag.userTypeID = ObjUserInfo.UserTypeId;
            long OrgID = ObjUserInfo.organisationId;
            ImposedConstraints objImposedConstraints = structuresService.ViewimposedConstruction(structureId, sectionId);

            ViewBag.ImposedConstraintas = objImposedConstraints;
            TempData["sectionId"] = sectionId;

            return View();

        }
        #region public ActionResult structureContact(int structureId)
        [AuthorizeUser(Roles = "13001,12000,12001,12002,12003,12005,40003,40004,100001")] // Added for 8389 regression since this method is used in haulier portal
        public ActionResult StructureContact(long structureId)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var SessionInfo = (UserInfo)Session["UserInfo"];
            List<StructureContact> structureContactList = new List<StructureContact>();
            structureContactList = structuresService.GetStructureContactListInfo(structureId, SessionInfo.UserSchema);
            return View(structureContactList);
        }

        /// <summary>
        /// Get contact list based on organisation
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="SearchString"></param>
        /// <param name="save"></param>
        /// <returns></returns>
      [HttpGet]
        public ActionResult ContactList(int? page, int? pageSize, string SearchString = null, int? orgID = null, int ContactTypeId = 0, string save = "false")
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
                #endregion

                #region Page access check
                if (!PageAccess.GetPageAccess("12003"))
                {
                    return RedirectToAction("Error", "Home");
                }
                #endregion

                if (ModelState.IsValid)
                {
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

                    if (orgID != null)
                    {
                        TempData["orgID"] = orgID;
                    }
                    List<DelegationList> Contactslist = null;

                    if (ViewBag.ContactSearchString == null && SearchString != null)
                    {
                        ViewBag.ContactSearchString = SearchString;

                    }
                    if (SearchString == null)
                    {
                        SearchString = Convert.ToString(ViewBag.ContactSearchString);
                    }
                    else
                    {
                        SearchString = SearchString.Trim();
                        ViewBag.ContactSearchString = SearchString;
                    }

                    Contactslist = structuredeligationService.GetContactList((int)pageNumber, (int)pageSize, SearchString, Convert.ToInt32(TempData["orgID"]));
                    if (Contactslist.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(Contactslist[0].TotalRecordCount);
                        IPagedList<DelegationList> model = Contactslist.ToPagedList(page ?? 1, 10);
                        page = model.PageCount;
                    }
                    else
                        ViewBag.TotalCount = 0;
                    var ContactslistListAsIPagedList = new StaticPagedList<DelegationList>(Contactslist, pageNumber, (int)pageSize, ViewBag.TotalCount);
                    TempData.Keep("orgID");
                   
                    return Json(ContactslistListAsIPagedList, "application/json", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/ContactList, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Structures/ContactList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
            //return View();
        }
        #endregion

        public ActionResult ViewRouteMapOnBack()
        {
            string result = null;

            if (Session["MovVersionID"] != null)
            {
                if ((string)Session["MovVersionID"] != "0")
                {
                    Session["OnBackIsSORTFlag"] = true;
                }
                else
                {
                    Session["OnBackIsSORTFlag"] = false;
                }
            }
            else if ((string)Session["StructureMode"] == "Edit")
            {
               
                    result = "DelegStructureList";
            }
            else if ((string)Session["StructureMode"] == "View")
            {
              
                result = "StructureList";
            }
           
            else
            {
                result = null;
            }

            return Json(new { result });
        }

    }



}
