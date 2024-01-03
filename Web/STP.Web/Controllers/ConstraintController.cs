using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STP.Domain.SecurityAndUsers;
using STP.Domain.RoadNetwork.Constraint;
using PagedList;
using STP.Common.Logger;
using STP.ServiceAccess.RoadNetwork;
using STP.Domain.RouteAssessment;
using System.Xml;
using STP.Domain.DocumentsAndContents;
using STP.ServiceAccess.DocumentsAndContents;
using System.Text;
using NetSdoGeometry;
using System.IO;
using STP.ServiceAccess.SecurityAndUsers;


namespace STP.Web.Controllers
{
    public class ConstraintController : Controller
    {
        // GET: Constraint
        private readonly IConstraintService constraintService;
        private readonly IInformationService informationService;
        private readonly IUserService userService;

        public ConstraintController(IConstraintService constraintService, IInformationService informationService, IUserService userService)
        {
            this.constraintService = constraintService;
            this.informationService = informationService;
            this.userService = userService;
        }
        public ConstraintController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConstraintHistory(int? page, int? pageSize, long ConstraintID, bool flageditmode = true,string from="")
        {
            //int? page = 1;
            //int? pageSize = 20;
            //long ConstraintID = 10009259;
            //bool flageditmode = true;

            try
            {
                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                TempData["ConstraintIDViewConst"] = ConstraintID;
                TempData.Keep("ConstraintIDViewConst");

                //if (ModelState.IsValid)
                //{
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

                    UserInfo userInfo = new UserInfo();

                    ViewBag.ConstraintID = ConstraintID;
                    ViewBag.flageditmode = flageditmode;
                ViewBag.from = from;
                     List<ConstraintModel> lstConstraintModel = constraintService.GetConstraintHistory((int)pageNumber, (int)pageSize, ConstraintID);

                    if (lstConstraintModel.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(lstConstraintModel[0].TotalRecordCount);

                        IPagedList<ConstraintModel> model = lstConstraintModel.ToPagedList(page ?? 1, 10);

                        page = model.PageCount;

                        ViewBag.TotalPages = model.PageCount;
                    }
                    else
                    {
                        ViewBag.TotalCount = 0;
                        ViewBag.TotalPages = 0;
                    }

                    var constraintIPagedList = new StaticPagedList<ConstraintModel>(lstConstraintModel, pageNumber, (int)pageSize, ViewBag.TotalCount);

                    return PartialView("ConstraintHistory", constraintIPagedList);
                //}
                //else
                //{
                //    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ConstraintHistory, Exception: {0}", "Invalid Model State"));
                //    return RedirectToAction("Error", "Home");
                //}
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ConstraintHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult ConstraintHistoryPopUp(int? page, int? pageSize, long ConstraintID, bool flageditmode = true, string from = "")
        {
            //int? page = 1;
            //int? pageSize = 20;
            //long ConstraintID = 10009259;
            //bool flageditmode = true;

            try
            {
                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                TempData["ConstraintIDViewConst"] = ConstraintID;
                TempData.Keep("ConstraintIDViewConst");

                //if (ModelState.IsValid)
                //{
                #region Paging Part
                int pageNumber = (page ?? 1);

                if (pageSize == null)
                {
                    Session["PageSize"] = null;
                    if (Session["PageSize"] == null)
                    {
                        Session["PageSize"] = 5;
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

                UserInfo userInfo = new UserInfo();

                ViewBag.ConstraintID = ConstraintID;
                ViewBag.flageditmode = flageditmode;
                ViewBag.from = from;
                List<ConstraintModel> lstConstraintModel = constraintService.GetConstraintHistory((int)pageNumber, (int)pageSize, ConstraintID);

                if (lstConstraintModel.Count > 0)
                {
                    ViewBag.TotalCount = Convert.ToInt32(lstConstraintModel[0].TotalRecordCount);

                    IPagedList<ConstraintModel> model = lstConstraintModel.ToPagedList(page ?? 1, 10);

                    page = model.PageCount;

                    ViewBag.TotalPages = model.PageCount;
                }
                else
                {
                    ViewBag.TotalCount = 0;
                    ViewBag.TotalPages = 0;
                }

                var constraintIPagedList = new StaticPagedList<ConstraintModel>(lstConstraintModel, pageNumber, (int)pageSize, ViewBag.TotalCount);

                return PartialView("ConstraintHistoryPopUp", constraintIPagedList);
                //}
                //else
                //{
                //    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ConstraintHistory, Exception: {0}", "Invalid Model State"));
                //    return RedirectToAction("Error", "Home");
                //}
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ConstraintHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public JsonResult CheckLinkOwnerShip(List<ConstraintReferences> constRefrences, bool allLinks = true)
        {
            bool res;
            // List<ConstraintReferences> constRefrences11 = null;
            if (Session["UserInfo"] == null)
            {
                return Json("Session timeout");
            }

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            if (SessionInfo.UserTypeId == 696002)
            {
                res = constraintService.CheckLinkOwnerShipForPolice((int)SessionInfo.OrganisationId, constRefrences, allLinks);
            }

            else if (SessionInfo.UserTypeId == 696008)
            {
                return Json(new { Success = true });
            }
            else
                res = constraintService.CheckLinkOwnerShip((int)SessionInfo.OrganisationId, constRefrences, allLinks);
            return Json(new { Success = res });
        }

        #region  SavingConstraint(ConstraintModel CMInput)
        /// <summary>
        /// controller to save constraints
        /// </summary>
        /// <param name="CMInput"></param>
        /// <returns></returns>
        public JsonResult SavingConstraint(ConstraintModel CMInput)
        {
            try
            {
                #region Session check
                UserInfo SessionInfo = null;

                UserInfo userInfo = new UserInfo();

                if (Session["UserInfo"] == null)
                {
                    return Json(new { Success = false });
                }

                SessionInfo = (UserInfo)Session["UserInfo"];

                int userID = Convert.ToInt32(SessionInfo.UserId);

                CMInput.OrganisationName = SessionInfo.OrganisationName;

                CMInput.OrganisationId = SessionInfo.OrganisationId;

                CMInput.UserType = SessionInfo.UserTypeId;
                #endregion

                string constrcode = CMInput.ConstraintCode;

                if (CMInput.ConstraintReferences == null)
                {
                    CMInput.ConstraintReferences = new List<ConstraintReferences>();
                }

                if (CMInput.CautionList == null)
                {
                    CMInput.CautionList = new List<RouteCautions>();
                }

                if (CMInput.ConstraintContact == null)
                {
                    CMInput.ConstraintContact = new List<AssessmentContacts>();
                }
                if (CMInput.ConstraintCode == null)
                {
                    constrcode = createConstraintCode(CMInput);
                    CMInput.ConstraintCode = constrcode;
                }

                CMInput.MaxGrossWeightKgs = (decimal)CMInput.GrossWeight * 1000;
                CMInput.MaxGrossWeight = CMInput.GrossWeight;
                CMInput.MaxAxleWeightKgs = (decimal)CMInput.AxleWeight * 1000;
                CMInput.MaxAxleWeight = CMInput.AxleWeight;

                CMInput.IsNodeConstraint = Convert.ToInt32(CMInput.HdnIsNodeConstraintFlag);

                int UOM = SessionInfo.VehicleUnits;

                ViewBag.UOM = UOM;

                if (SessionInfo.VehicleUnits == 692002)
                {
                    string[] splitFeet;
                    float foot = 0;
                    float inches = 0;
                    float inchtoft = 0;

                    if (!string.IsNullOrEmpty(CMInput.Height))
                    {
                        splitFeet = CMInput.Height.Trim().Replace("\"", "'").Split('\'');
                        if (splitFeet.Length > 0)
                        {
                            if (splitFeet[0] != null && splitFeet[0] != "")
                            {
                                foot = (float)(Convert.ToDouble(splitFeet[0]) * 0.3048);
                                if (splitFeet.Length > 1 && splitFeet[1] != "")
                                    inches = (float)(Convert.ToDouble(splitFeet[1]) * 0.0254);
                                CMInput.MaxHeightMtrs = (float)Math.Round(foot + inches, 2);
                                foot = (float)(Convert.ToDouble(splitFeet[0]));
                                if (splitFeet.Length > 1 && splitFeet[1] != "")
                                    inchtoft = (float)(Convert.ToDouble(splitFeet[1]) * 0.08333);
                                CMInput.MaxHeightFT = (float)Math.Round(foot + inchtoft, 2);
                            }
                        }
                    }
                    else
                    {
                        CMInput.Height = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(CMInput.Width))
                    {
                        splitFeet = CMInput.Width.Trim().Replace("\"", "'").Split('\'');
                        if (splitFeet.Length > 0)
                        {
                            if (splitFeet[0] != null && splitFeet[0] != "")
                            {
                                foot = (float)(Convert.ToDouble(splitFeet[0]) * 0.3048);
                                if (splitFeet.Length > 1 && splitFeet[1] != "")
                                    inches = (float)(Convert.ToDouble(splitFeet[1]) * 0.0254);
                                CMInput.MaxWidthMeters = (float)Math.Round(foot + inches, 2);
                                foot = (float)(Convert.ToDouble(splitFeet[0]));
                                if (splitFeet.Length > 1 && splitFeet[1] != "")
                                    inchtoft = (float)(Convert.ToDouble(splitFeet[1]) * 0.08333);
                                CMInput.MaxWidthFeet = (float)Math.Round(foot + inchtoft, 2);

                            }
                        }
                    }
                    else
                    {
                        CMInput.Width = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(CMInput.Length))
                    {
                        splitFeet = CMInput.Length.Trim().Replace("\"", "'").Split('\'');
                        if (splitFeet.Length > 0)
                        {
                            if (splitFeet[0] != null && splitFeet[0] != "")
                            {
                                foot = (float)(Convert.ToDouble(splitFeet[0]) * 0.3048);
                                if (splitFeet.Length > 1 && splitFeet[1] != "")
                                    inches = (float)(Convert.ToDouble(splitFeet[1]) * 0.0254);
                                CMInput.MaxLengthMeters = (float)Math.Round(foot + inches, 2);
                                foot = (float)(Convert.ToDouble(splitFeet[0]));
                                if (splitFeet.Length > 1 && splitFeet[1] != "")
                                    inchtoft = (float)(Convert.ToDouble(splitFeet[1]) * 0.08333);
                                CMInput.MaxLengthFeet = (float)Math.Round(foot + inchtoft, 2);
                            }
                        }
                    }
                    else
                    {
                        CMInput.Length = string.Empty;
                    }
                }
                else
                {
                    if (CMInput.Height != null && Convert.ToDecimal(CMInput.Height.Trim()) != 0)
                    {
                        CMInput.MaxHeightMtrs = (float)Convert.ToDouble(CMInput.Height.Trim());
                    }

                    if (CMInput.Width != null && Convert.ToDecimal(CMInput.Width.Trim()) != 0)
                    {
                        CMInput.MaxWidthMeters = (float)Convert.ToDouble(CMInput.Width.Trim());
                    }

                    if (CMInput.Length != null && Convert.ToDecimal(CMInput.Length.Trim()) != 0)
                    {
                        CMInput.MaxLengthMeters = (float)Convert.ToDouble(CMInput.Length);
                    }
                }

                long constrId = constraintService.SaveConstraints(CMInput, userID);

                bool result = false;
                if (constrId > 0)
                {
                    result = true;
                }
                TempData["CMI"] = CMInput;
                TempData.Keep("CMI");
                return Json(new { Success = result, ConstraintId = constrId, ConstraintCode = constrcode });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/SavingConstraint, Exception: {0}", ex));
                return Json(new { Success = false });
            }
        }
        private string createConstraintCode(ConstraintModel CM)
        {
            var charString = getRandomChar();
            string constrCode = "0";
            if (CM.ConstraintType == 0)
                CM.ConstraintType = 253000;
            constrCode = "C-" + charString + DateTime.Now.ToString().GetHashCode().ToString("X") + "-" + (CM.ConstraintType - 253000);
            return constrCode;
        }
        private string getRandomChar()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[2];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var charString = new String(stringChars);
            return charString;
        }
        #endregion

        #region StoreCautionData(ConstraintModel Constraintmodelalter)
        /// <summary>
        /// Store caution input data
        /// </summary>
        /// <param name="delegationList">DelegationList mode</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StoreCautionData(ConstraintModel Constraintmodelalter)
        {
            ViewBag.ConstraintID = Constraintmodelalter.ConstraintId;
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json("sessionnull", JsonRequestBehavior.AllowGet);
            }

            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion
            if (Constraintmodelalter.SelectedType == ActionType.SpecificAction)
            {
                Constraintmodelalter.SelectedType = ActionType.SpecificAction;
                Constraintmodelalter.SelectedTypeName = "SpecificAction";

            }
            else
            {
                Constraintmodelalter.SelectedType = ActionType.StandardCaution;
                Constraintmodelalter.SelectedTypeName = "StandardCaution";
            }
            if (SessionInfo.VehicleUnits == 692001)//Metric
            {
                if (!string.IsNullOrEmpty(Constraintmodelalter.Height))
                {
                    Constraintmodelalter.MaxHeightMtrs = (float)Convert.ToDouble(Constraintmodelalter.Height);
                }
                else
                {
                    Constraintmodelalter.Height = string.Empty;
                }

                if (!string.IsNullOrEmpty(Constraintmodelalter.Width))
                {
                    Constraintmodelalter.MaxWidthMeters = (float)Convert.ToDouble(Constraintmodelalter.Width);
                }
                else
                {
                    Constraintmodelalter.Width = string.Empty;
                }

                if (!string.IsNullOrEmpty(Constraintmodelalter.Length))
                {
                    Constraintmodelalter.MaxLengthMeters = Constraintmodelalter.MaxLengthMeters = (float)Convert.ToDouble(Constraintmodelalter.Length);
                }
                else
                {
                    Constraintmodelalter.Length = string.Empty;
                }
                if (!string.IsNullOrEmpty(Constraintmodelalter.Speed))
                {
                    Constraintmodelalter.MinSpeedKph = (float)Math.Round(Convert.ToDouble(Constraintmodelalter.Speed) / (float)0.6213, 2);
                }
                else
                {
                    Constraintmodelalter.Speed = string.Empty;
                }
            }
            else
            {//imperial(692002)
                string[] splitFeet;
                float foot = 0;
                float inches = 0;

                if (!string.IsNullOrEmpty(Constraintmodelalter.Height))
                {
                    splitFeet = Constraintmodelalter.Height.Replace("\"", "''").Split('\'');
                    if (splitFeet.Length > 0)
                    {
                        if (splitFeet[0] != null)
                        {
                            foot = (float)(Convert.ToDouble(splitFeet[0]));
                            inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                            Constraintmodelalter.MaxHeight = (float)Math.Round(foot + inches, 2);
                            Constraintmodelalter.MaxHeightMtrs = (Constraintmodelalter.MaxHeight != 0) ? (float)Math.Round(Constraintmodelalter.MaxHeight * (float)0.3048, 2) : 0;

                        }
                    }
                }
                else
                {
                    Constraintmodelalter.Height = string.Empty;
                }
                if (!string.IsNullOrEmpty(Constraintmodelalter.Width))
                {
                    splitFeet = Constraintmodelalter.Width.Replace("\"", "''").Split('\'');
                    if (splitFeet.Length > 0)
                    {
                        if (splitFeet[0] != null)
                        {
                            foot = (float)(Convert.ToDouble(splitFeet[0]));
                            inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                            Constraintmodelalter.MaxWidth = (float)Math.Round(foot + inches, 2);
                            Constraintmodelalter.MaxWidthMeters = (Constraintmodelalter.MaxWidth != 0) ? (float)Math.Round(Constraintmodelalter.MaxWidth * (float)0.3048, 2) : 0;
                        }
                    }
                }
                else
                {
                    Constraintmodelalter.Width = string.Empty;
                }
                if (!string.IsNullOrEmpty(Constraintmodelalter.Length))
                {
                    splitFeet = Constraintmodelalter.Length.Replace("\"", "''").Split('\'');
                    if (splitFeet.Length > 0)
                    {
                        if (splitFeet[0] != null)
                        {
                            foot = (float)(Convert.ToDouble(splitFeet[0]));
                            inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                            Constraintmodelalter.MaxLength = (float)Math.Round(foot + inches, 2);
                            Constraintmodelalter.MaxLengthMeters = Constraintmodelalter.MaxLengthMeters = (Constraintmodelalter.MaxLength != 0) ? (float)Math.Round(Constraintmodelalter.MaxLength * (float)0.3048, 2) : 0;
                        }
                    }
                }
                else
                {
                    Constraintmodelalter.Length = string.Empty;
                }

                if (!string.IsNullOrEmpty(Constraintmodelalter.Speed))
                {
                    Constraintmodelalter.MinSpeedKph = (float)Math.Round(Convert.ToDouble(Constraintmodelalter.Speed) / (float)0.6213, 2);
                    //Constraintmodelalter.MIN_SPEED_KPH = (float)Math.Round(Convert.ToDouble(Constraintmodelalter.Speed) * (float)0.6213, 2);
                }
                else
                {
                    Constraintmodelalter.Speed = string.Empty;
                }
            }
            // Converting tonnes to KGS .....
            Constraintmodelalter.MaxGrossWeightKgs = (decimal)Constraintmodelalter.GrossWeight * 1000;
            Constraintmodelalter.MaxGrossWeight = Constraintmodelalter.GrossWeight;
            Constraintmodelalter.MaxAxleWeightKgs = (decimal)Constraintmodelalter.AxleWeight * 1000;
            Constraintmodelalter.MaxAxleWeight = Constraintmodelalter.AxleWeight;
            //--------------------------------------------------------------------------

            TempData.Keep("OrganisationName");
            TempData.Keep("OWNER_ORG_ID");
            TempData["Constraintmodelalter"] = Constraintmodelalter;
            TempData.Keep("Constraintmodelalter");
            TempData.Keep("ConstraintmodelBase");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CreateConstraint()

        public ActionResult CreateConstraint(string topolgyType,bool isPartialView=false)
        {
            try
            {

                UserInfo SessionInfo = null;

                UserInfo userInfo = new UserInfo();

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];
                ViewBag.isPartialView = isPartialView;
                if (ModelState.IsValid)
                {
                    int UOM = SessionInfo.VehicleUnits;

                    ViewBag.UOM = UOM;

                    int nTopologyType = 0;
                    if (topolgyType == "point")
                    {
                        nTopologyType = 248001;
                    }
                    else if (topolgyType == "linear")
                    {
                        nTopologyType = 248002;
                    }
                    else
                    {
                        nTopologyType = 248003;
                    }

                    ConstraintModel CM = new ConstraintModel();
                    CM.OrganisationName = SessionInfo.OrganisationName;
                    string EnumTypeName = "ConstraintTypeEnum";
                    List<InformationModel> ConstraintType = new List<InformationModel>();
                    ConstraintType = informationService.GetEnumValsListByEnumType(EnumTypeName);

                    List<SelectListItem> ConstraintTypeList = new List<SelectListItem>();
                    ConstraintTypeList = (from items in ConstraintType
                                          select new SelectListItem
                                          {
                                              Text = Convert.ToString(items.EnumValuesName).Substring(0, 1).ToUpper() + Convert.ToString(items.EnumValuesName).Substring(1).ToLower(),
                                              Value = Convert.ToString(items.Code)
                                          }).ToList();

                    ConstraintTypeList = ConstraintTypeList.OrderBy(m => m.Text).ToList();

                    EnumTypeName = "ConstraintTopologyTypeEnum";
                    List<InformationModel> Direction = new List<InformationModel>();
                    Direction = informationService.GetEnumValsListByEnumType(EnumTypeName);

                    List<SelectListItem> DirectionList = new List<SelectListItem>();

                    DirectionList = (from items in Direction
                                     select new SelectListItem
                                     {
                                         Text = Convert.ToString(items.EnumValuesName).Substring(0, 1).ToUpper() + Convert.ToString(items.EnumValuesName).Substring(1).ToLower(),
                                         Value = Convert.ToString(items.Code),
                                         Selected = false
                                     }).ToList();

                    DirectionList = DirectionList.OrderBy(m => m.Text).ToList();
                    CM.Direction = DirectionList;
                    CM.ConstraintTypeList = ConstraintTypeList;
                    CM.TopologyType = nTopologyType;
                    return View(CM);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/CreateConstraint, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/CreateConstraint, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region  FindLinksOfAreaConstraint(sdogeometry polygonGeometry)
        /// <summary>
        /// controller to find links of area constraints
        /// </summary>
        /// <param name="polygonGeometry"></param>
        /// <returns></returns>
        /// 
        public JsonResult FindLinksOfAreaConstraint(sdogeometry polygonGeometry)
        {
            bool res;
            int userType;

            if (Session["UserInfo"] == null)
            {
                return Json("Session timeout");
            }

            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            if (SessionInfo.UserTypeId == 696002)
                userType = 2;
            else if (SessionInfo.UserTypeId == 696008)
                userType = 0;
            else
                userType = 1;

            res = constraintService.FindLinksOfAreaConstraint(polygonGeometry, (int)SessionInfo.OrganisationId, userType);
            return Json(new { Success = res }); ;
        }
        #endregion

        #region  CautionAddReport(long ConstraintID)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConstraintID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CautionAddReport(long ConstraintID = 0,int? viewCaution=0, long cautionID=0,string description="")
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
                    int UOM = SessionInfo.VehicleUnits;

                    ViewBag.UOM = UOM;
                    ConstraintModel Constraintmodelalter = new ConstraintModel();
                    ConstraintModel ConstraintmodelBase = new ConstraintModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["Constraintmodelalter"])))
                    {
                        Constraintmodelalter = (ConstraintModel)TempData["Constraintmodelalter"];
                        Constraintmodelalter.ConstraintId = ConstraintID;
                        Constraintmodelalter.DirectionName = description;


                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["ConstraintmodelBase"])))
                    {
                        ConstraintmodelBase = (ConstraintModel)TempData["ConstraintmodelBase"];
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
                    if (ConstraintmodelBase.CautionId == 0)//its for add
                    {
                        ViewBag.CautionId = ConstraintmodelBase.CautionId;

                        if (!string.IsNullOrEmpty(Constraintmodelalter.CautionName))
                        {
                            changeList.Add("CAUTION_NAME", "Name" + PreUnspecified + SetTo + Constraintmodelalter.CautionName + "'");
                        }
                        if (ConstraintmodelBase.SelectedType != Constraintmodelalter.SelectedType)
                        {
                            changeList.Add("StandardCaution", "Action type" + PreSpecified + ConstraintmodelBase.SelectedType.ToString() + closeSpecified + SetTo + Constraintmodelalter.SelectedType.ToString() + "'");
                        }

                        if (!string.IsNullOrEmpty(Constraintmodelalter.DirectionName))
                        {
                            changeList.Add("DIRECTION_NAME", "Action" + PreUnspecified + SetTo + Constraintmodelalter.DirectionName + "'");
                        }

                        if (ConstraintmodelBase.Bold != Constraintmodelalter.Bold)
                        {
                            changeList.Add("Bold", "Bold" + PreSpecified + (Convert.ToString(ConstraintmodelBase.Bold).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.Bold).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (ConstraintmodelBase.Italic != Constraintmodelalter.Italic)
                        {
                            changeList.Add("Italic", "Italic" + PreSpecified + (Convert.ToString(ConstraintmodelBase.Italic).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.Italic).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (ConstraintmodelBase.UnderLine != Constraintmodelalter.UnderLine)
                        {
                            changeList.Add("Underline", "Underline" + PreSpecified + (Convert.ToString(ConstraintmodelBase.UnderLine).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.UnderLine).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (Constraintmodelalter.MaxHeightMtrs != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MAX_HEIGHT_MTRS", "Height" + PreUnspecified + SetTo + Constraintmodelalter.Height + Meters + "'");
                            }
                            else
                            {
                                changeList.Add("MAX_HEIGHT_FT", "Height" + PreUnspecified + SetTo + Constraintmodelalter.Height + FeetInches + "'");
                            }
                        }

                        if (Constraintmodelalter.MaxWidthMeters != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MAX_WIDTH_MTRS", "Width" + PreUnspecified + SetTo + Constraintmodelalter.Width + Meters + "'");
                            }
                            else
                            {
                                changeList.Add("MAX_WIDTH_FT", "Width" + PreUnspecified + SetTo + Constraintmodelalter.Width + FeetInches + "'");
                            }
                        }
                        if (Constraintmodelalter.MaxLengthMeters != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MAX_LENGTH_MTRS", "Length" + PreUnspecified + SetTo + Constraintmodelalter.Length + Meters + "'");
                            }
                            else
                            {
                                changeList.Add("MAX_LENGTH_FT", "Length" + PreUnspecified + SetTo + Constraintmodelalter.Length + FeetInches + "'");
                            }
                        }
                       
                        if (Constraintmodelalter.GrossWeight != 0)
                        {
                            changeList.Add("MAX_GROSS_WEIGHT_KGS", "Gross weight" + PreUnspecified + SetTo + Constraintmodelalter.GrossWeight + Tonnes + "'");
                        }
                        if (Constraintmodelalter.AxleWeight != 0)
                        {
                            changeList.Add("MAX_AXLE_WEIGHT_KGS", "Axle weight" + PreUnspecified + SetTo + Constraintmodelalter.AxleWeight + Tonnes + "'");
                        }
                        if (Constraintmodelalter.MinSpeedKph != 0)
                        {
                            if (SessionInfo.VehicleUnits == 692001)
                            {
                                changeList.Add("MIN_SPEED_KPH", "Speed" + PreUnspecified + SetTo + Constraintmodelalter.Speed + " kph" + "'");
                            }
                            else
                            {
                                changeList.Add("MIN_SPEED_KPH", "Speed" + PreUnspecified + SetTo + Constraintmodelalter.Speed + " mph" + "'");
                            }
                        }
                        if (ConstraintmodelBase.CreatorIsContact != Constraintmodelalter.CreatorIsContact)
                        {
                            changeList.Add("CREATOR_IS_CONTACT", "Creator is contact" + PreSpecified + (Convert.ToString(ConstraintmodelBase.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }

                    }
                    else //its for edit caution
                    {
                        ViewBag.CautionId = ConstraintmodelBase.CautionId;
                        //CAUTION_NAME
                        if (Convert.ToString(ConstraintmodelBase.CautionName == null ? string.Empty : ConstraintmodelBase.CautionName).Trim() != Convert.ToString(Constraintmodelalter.CautionName == null ? string.Empty : Constraintmodelalter.CautionName).Trim())
                        {
                            changeList.Add("CAUTION_NAME", "Name" + PreSpecified + ((ConstraintmodelBase.CautionName == string.Empty || ConstraintmodelBase.CautionName == null) ? unspecified : ConstraintmodelBase.CautionName) + closeSpecified + SetTo + ((Constraintmodelalter.CautionName == string.Empty || Constraintmodelalter.CautionName == null) ? unspecified : Constraintmodelalter.CautionName) + "'");
                        }
                        if (ConstraintmodelBase.SelectedType != Constraintmodelalter.SelectedType)
                        {
                            changeList.Add("StandardCaution", "Action type" + PreSpecified + (Convert.ToString(ConstraintmodelBase.SelectedType) == string.Empty ? unspecified : Convert.ToString(ConstraintmodelBase.SelectedType)) + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.SelectedType) == string.Empty ? unspecified : Convert.ToString(Constraintmodelalter.SelectedType)) + "'");
                        }
                        if (ConstraintmodelBase.StandardCaution != Constraintmodelalter.StandardCaution)
                        {
                            changeList.Add("StandardCaution", "Standard caution" + PreSpecified + (Convert.ToString(ConstraintmodelBase.StandardCaution).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.StandardCaution).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (!string.IsNullOrEmpty(ConstraintmodelBase.DirectionName) || !string.IsNullOrEmpty(Constraintmodelalter.DirectionName))
                        {
                            if (ConstraintmodelBase.DirectionName != Constraintmodelalter.DirectionName)
                            {
                                changeList.Add("DIRECTION_NAME", "Action" + PreSpecified + (Convert.ToString(ConstraintmodelBase.DirectionName == null ? string.Empty : ConstraintmodelBase.DirectionName) == string.Empty ? unspecified : Convert.ToString(ConstraintmodelBase.DirectionName)) + closeSpecified + SetTo + ((Constraintmodelalter.DirectionName == string.Empty || Constraintmodelalter.DirectionName == null) ? unspecified : Constraintmodelalter.DirectionName) + "'");
                            }
                        }
                        if (ConstraintmodelBase.Bold != Constraintmodelalter.Bold)
                        {
                            changeList.Add("Bold", "Bold" + PreSpecified + (Convert.ToString(ConstraintmodelBase.Bold).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.Bold).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (ConstraintmodelBase.Italic != Constraintmodelalter.Italic)
                        {
                            changeList.Add("Italic", "Italic" + PreSpecified + (Convert.ToString(ConstraintmodelBase.Italic).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.Italic).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (ConstraintmodelBase.UnderLine != Constraintmodelalter.UnderLine)
                        {
                            changeList.Add("Underline", "Underline" + PreSpecified + (Convert.ToString(ConstraintmodelBase.UnderLine).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.UnderLine).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                        if (!string.IsNullOrEmpty(ConstraintmodelBase.Height) || !string.IsNullOrEmpty(Convert.ToString(Constraintmodelalter.Height).Replace("''", "\"")))
                        {
                            if (Convert.ToString(ConstraintmodelBase.Height == null ? string.Empty : ConstraintmodelBase.Height).Trim() != Convert.ToString(Constraintmodelalter.Height == null ? string.Empty : Constraintmodelalter.Height).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MAX_HEIGHT_MTRS", "Height" + PreSpecified + (ConstraintmodelBase.Height == string.Empty ? unspecified : ConstraintmodelBase.Height) + closeSpecified + SetTo + (Constraintmodelalter.Height == string.Empty ? unspecified : Constraintmodelalter.Height) + Meters + "'");
                                }
                                else
                                {
                                    changeList.Add("MAX_HEIGHT_FT", "Height" + PreSpecified + (ConstraintmodelBase.Height == string.Empty ? unspecified : ConstraintmodelBase.Height) + closeSpecified + SetTo + (Constraintmodelalter.Height == string.Empty ? unspecified : Constraintmodelalter.Height) + FeetInches + "'");
                                }
                            }
                            else
                            {
                                Constraintmodelalter.MaxHeightMtrs = ConstraintmodelBase.MaxHeightMtrs;
                            }
                        }
                        if (!string.IsNullOrEmpty(ConstraintmodelBase.Width) || !string.IsNullOrEmpty(Convert.ToString(Constraintmodelalter.Width).Replace("''", "\"")))
                        {
                            if (Convert.ToString(ConstraintmodelBase.Width == null ? string.Empty : ConstraintmodelBase.Width).Trim() != Convert.ToString(Constraintmodelalter.Width == null ? string.Empty : Constraintmodelalter.Width).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MAX_WIDTH_MTRS", "Width" + PreSpecified + (ConstraintmodelBase.Width == string.Empty ? unspecified : ConstraintmodelBase.Width) + closeSpecified + SetTo + (Constraintmodelalter.Width == string.Empty ? unspecified : Constraintmodelalter.Width) + Meters + "'");
                                }
                                else
                                {
                                    changeList.Add("MAX_WIDTH_FT", "Width" + PreSpecified + (ConstraintmodelBase.Width == string.Empty ? unspecified : ConstraintmodelBase.Width) + closeSpecified + SetTo + (Constraintmodelalter.Width == string.Empty ? unspecified : Constraintmodelalter.Width) + FeetInches + "'");
                                }
                            }
                            else
                            {
                                Constraintmodelalter.MaxWidthMeters = ConstraintmodelBase.MaxWidthMeters;
                            }
                        }

                        if (!string.IsNullOrEmpty(ConstraintmodelBase.Length) || !string.IsNullOrEmpty(Convert.ToString(Constraintmodelalter.Length).Replace("''", "\"")))
                        {
                            if (Convert.ToString(ConstraintmodelBase.Length == null ? string.Empty : ConstraintmodelBase.Length).Trim() != Convert.ToString(Constraintmodelalter.Length == null ? string.Empty : Constraintmodelalter.Length).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MAX_LEN_MTRS", "Length" + PreSpecified + (ConstraintmodelBase.Length == string.Empty ? unspecified : ConstraintmodelBase.Length) + closeSpecified + SetTo + (Constraintmodelalter.Length == string.Empty ? unspecified : Constraintmodelalter.Length) + Meters + "'");
                                }
                                else
                                {
                                    changeList.Add("MAX_LEN_FT", "Length" + PreSpecified + (ConstraintmodelBase.Length == string.Empty ? unspecified : ConstraintmodelBase.Length) + closeSpecified + SetTo + (Constraintmodelalter.Length == string.Empty ? unspecified : Constraintmodelalter.Length) + FeetInches + "'");
                                }
                            }
                            else
                            {
                                Constraintmodelalter.MaxLengthMeters = ConstraintmodelBase.MaxLengthMeters;
                            }
                        }

                        var maxGrossWeightNew = Constraintmodelalter.GrossWeight;
                        if (ConstraintmodelBase.GrossWeight != 0 || maxGrossWeightNew != 0)
                        {                            
                            if (ConstraintmodelBase.GrossWeight != maxGrossWeightNew)
                            {
                                changeList.Add("MAX_GROSS_WEIGHT_KGS", "Gross weight" + PreSpecified + Convert.ToString(ConstraintmodelBase.GrossWeight == 0 ? unspecified : Convert.ToString(ConstraintmodelBase.GrossWeight)) + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.GrossWeight == 0 ? unspecified : Convert.ToString(Constraintmodelalter.GrossWeight))) + Tonnes + "'");
                            }
                        }
                        var maxAxleWeightNew = Constraintmodelalter.AxleWeight;
                        if (ConstraintmodelBase.AxleWeight != 0 || maxAxleWeightNew != 0)
                        {
                            if (ConstraintmodelBase.AxleWeight != maxAxleWeightNew)
                            {
                                changeList.Add("MAX_AXLE_WEIGHT_KGS", "Axle weight" + PreSpecified + Convert.ToString(ConstraintmodelBase.AxleWeight == 0 ? unspecified : Convert.ToString(ConstraintmodelBase.AxleWeight)) + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.AxleWeight == 0 ? unspecified : Convert.ToString(Constraintmodelalter.AxleWeight))) + Tonnes + "'");
                            }
                        }
                        if (!string.IsNullOrEmpty(ConstraintmodelBase.Speed) || !string.IsNullOrEmpty(Constraintmodelalter.Speed))
                        {
                            if (Convert.ToString(ConstraintmodelBase.Speed == null ? string.Empty : ConstraintmodelBase.Speed).Trim() != Convert.ToString(Constraintmodelalter.Speed == null ? string.Empty : Constraintmodelalter.Speed).Trim())
                            {
                                if (SessionInfo.VehicleUnits == 692001)
                                {
                                    changeList.Add("MIN_SPEED_KPH", "Speed" + PreSpecified + (ConstraintmodelBase.Speed == string.Empty ? unspecified : ConstraintmodelBase.Speed) + closeSpecified + SetTo + (Constraintmodelalter.Speed == string.Empty ? unspecified : Constraintmodelalter.Speed) + " kph" + "'");
                                }
                                else
                                    changeList.Add("MIN_SPEED_KPH", "Speed" + PreSpecified + (ConstraintmodelBase.Speed == string.Empty ? unspecified : ConstraintmodelBase.Speed) + closeSpecified + SetTo + (Constraintmodelalter.Speed == string.Empty ? unspecified : Constraintmodelalter.Speed) + " mph" + "'");
                            }
                        }

                        if (ConstraintmodelBase.CreatorIsContact != Constraintmodelalter.CreatorIsContact)
                        {
                            changeList.Add("CREATOR_IS_CONTACT", "Creator is contact" + PreSpecified + (Convert.ToString(ConstraintmodelBase.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + closeSpecified + SetTo + (Convert.ToString(Constraintmodelalter.CreatorIsContact).ToLower() == "true" ? "checked" : "unchecked") + "'");
                        }
                    }


                    Constraintmodelalter.GrossWeight = (double)((Constraintmodelalter.MaxGrossWeightKgs != 0) ? Constraintmodelalter.MaxGrossWeightKgs / 1000 : 0);
                    Constraintmodelalter.AxleWeight = (double)((Constraintmodelalter.MaxAxleWeightKgs != 0) ? Constraintmodelalter.MaxAxleWeightKgs / 1000 : 0);

                    if (ConstraintmodelBase.CautionId != 0)
                    {
                        ViewBag.mode = "edit";
                    }
                    else
                    {
                        ViewBag.mode = "add";
                    }
                    ViewBag.changeListCount = changeList.Count;
                    ViewBag.changeList = changeList;
                    TempData["Constraintmodelalter"] = Constraintmodelalter;
                    TempData.Keep("Constraintmodelalter");
                    TempData.Keep("ConstraintmodelBase");
                    ViewBag.ConstraintID = ConstraintID;
                    ViewBag.viewCaution = viewCaution;
                    if (changeList.Count > 0)
                    {
                        TempData["changeList"] = changeList;
                        TempData.Keep("changeList");
                    }
                    TempData.Keep("OrganisationName");
                    TempData.Keep("OWNER_ORG_ID");
                    return View();
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/CautionAddReport, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/CautionAddReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home"); ;
            }
        }
        #endregion

        /// <summary>
        /// Save constraint contact
        /// </summary>
        /// <param name="constraintContact">ConstraintContactModel</param>
        /// <returns>return true or false</returns>
        [HttpPost]
        public JsonResult SaveConstraintContact(ConstraintContactModel constraintContact)
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
                constraintContact.ContactId = Convert.ToInt64(SessionInfo.UserId);
                constraintContact.RoleType = SessionInfo.UserTypeId;
                constraintContact.OrganisationId = SessionInfo.OrganisationId;

                bool result = constraintService.SaveConstraintContact(constraintContact);

                return Json(new { Success = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }

        #region EditConstraint(int ConstraintID)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConstraintID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditConstraint(int ConstraintID)
        {
            try
            {
                UserInfo SessionInfo = null;

                UserInfo userInfo = new UserInfo();

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                SessionInfo = (UserInfo)Session["UserInfo"];

                if (ModelState.IsValid)
                {
                    int UOM = SessionInfo.VehicleUnits;

                    ViewBag.UOM = UOM;

                    ViewBag.ConstraintID = ConstraintID;

                    ConstraintModel CM = constraintService.GetConstraintDetails(ConstraintID);
                    CM.Length = CM.MaxLengthMeters.ToString();
                    CM.Height = CM.HdnMaxHeightMtrs.ToString();
                    CM.Width = CM.HdnMaxWidthMeters.ToString();
                    CM.HdnMaxGrossWeightKgs =(decimal) CM.GrossWeight;
                    CM.HdnMaxAxleWeightKgs = (decimal)CM.AxleWeight;
                    if (CM.EndDateString == "01/01/0001"|| CM.EndDateString == "01-01-0001")
                    {
                        CM.EndDateString = null;
                        CM.HdnEndDateString = null;
                    }
                    if (CM.StartDateString == "01/01/0001" || CM.StartDateString == "01-01-0001")
                    {
                        CM.StartDateString = null;
                        CM.HdnStartDateString = null;
                    }
                    TempData["CMI"] = CM;
                    TempData.Keep("CMI");

                    // Converting KGS to Tonnes.
                    CM.GrossWeight = (double)((CM.MaxGrossWeightKgs != 0) ? CM.MaxGrossWeightKgs / 1000 : 0);
                    CM.AxleWeight = (double)((CM.MaxAxleWeightKgs != 0) ? CM.MaxAxleWeightKgs / 1000 : 0);
                    //---------------------------------------------------------------------------

                    string EnumTypeName = "ConstraintTypeEnum";
                    List<InformationModel> ConstraintType = new List<InformationModel>();
                    ConstraintType = informationService.GetEnumValsListByEnumType(EnumTypeName);

                    List<SelectListItem> ConstraintTypeList = new List<SelectListItem>();
                    ConstraintTypeList = (from items in ConstraintType
                                          select new SelectListItem
                                          {
                                              Text = Convert.ToString(items.EnumValuesName).Substring(0, 1).ToUpper() + Convert.ToString(items.EnumValuesName).Substring(1).ToLower(),
                                              Value = Convert.ToString(items.Code)
                                          }).ToList();

                    ConstraintTypeList = ConstraintTypeList.OrderBy(m => m.Text).ToList();

                    EnumTypeName = "ConstraintTopologyTypeEnum";

                    CM.ConstraintTypeList = ConstraintTypeList;

                    if (UOM == 692001)
                    {
                        if (CM.MaxHeightMtrs != 0)
                        {
                            CM.HdnHeight = CM.Height = Convert.ToString(CM.MaxHeightMtrs);
                        }

                        if (CM.MaxWidthMeters != 0)
                        {
                            CM.HdnWidth = CM.Width = Convert.ToString(CM.MaxWidthMeters);
                            CM.MaxWidthFeet = 0;
                        }

                        if (CM.MaxLengthMeters != 0)
                        {
                            CM.HdnLength = CM.Length = Convert.ToString(CM.MaxLengthMeters);
                            CM.MaxLengthFeet = 0;
                        }
                    }
                    else
                    {

                        if (CM.MaxHeightFT != 0)
                        {
                            float MAX_HEIGHT_MTRS_ = CM.MaxHeightFT;
                            int FeetL = (int)MAX_HEIGHT_MTRS_;
                            float InchL = MAX_HEIGHT_MTRS_ - FeetL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            CM.HdnHeight = CM.Height = FeetL + "'" + height + "\"";
                        }
                        else
                        {
                            CM.HdnHeight = CM.Height = string.Empty;
                        }
                        if (CM.MaxWidthFeet != 0)
                        {
                            float MAX_WIDTH_MTRS_ = CM.MaxWidthFeet;
                            int widthL = (int)MAX_WIDTH_MTRS_;
                            float InchL = MAX_WIDTH_MTRS_ - widthL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            CM.HdnWidth = CM.Width = widthL + "'" + height + "\"";
                        }
                        else
                        {
                            CM.HdnWidth = CM.Width = string.Empty;
                        }
                        if (CM.MaxLengthFeet != 0)
                        {

                            float MAX_LEN_MTRS_ = CM.MaxLengthFeet;
                            int lengthL = (int)MAX_LEN_MTRS_;
                            float InchL = MAX_LEN_MTRS_ - lengthL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            CM.HdnLength = CM.Length = lengthL + "'" + height + "\"";
                        }
                        else
                        {
                            CM.HdnLength = CM.Length = string.Empty;
                        }
                    }
                    return View(CM);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/EditConstraint, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/EditConstraint, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        public ActionResult ManageConstraintContact(long ConstraintID, short ContactNo, string mode, bool flageditmode = true)
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

                ViewBag.ConstraintID = ConstraintID;
                ViewBag.flageditmode = flageditmode;
                #region Fill Country Dropdown
                //Filling Country List Drop Down From XML
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Configuration.xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);

                //Filling Country List Drop Down From database
                var CountryList = userService.GetCountryInfo();
                SelectList CountrySelectList = new SelectList(CountryList, "CountryID", "Country");

                XmlNode Association = xmlDocument.SelectSingleNode("/Configurable/association");
                XmlNodeList membership = Association.SelectNodes("membership");

                var memddlList = new List<UserRegistration>();
                ViewBag.CountryDropDown = CountrySelectList;
                #endregion

                if (mode == "add")
                {
                    ConstraintContactModel constraintContactModel = new ConstraintContactModel();
                    constraintContactModel.ConstraintId = ConstraintID;
                    constraintContactModel.ContactNo = ContactNo;
                    return View(constraintContactModel);
                }
                else
                {
                    List<ConstraintContactModel> constraintContactList = constraintService.GetConstraintContactList(1, 1, ConstraintID, ContactNo);
                    if (constraintContactList.Count > 0)
                    {
                        constraintContactList[0].CountryName = CountrySelectList.Where(r => r.Value.Equals(Convert.ToString(constraintContactList[0].CountryId))).Select(r => r.Text).SingleOrDefault();
                        return View(constraintContactList[0]);
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ManageConstraintContact, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        /// Delete Caution
        /// </summary>
        /// <param name="CAUTION_ID">Caution id</param>
        /// <returns>ture/false</returns>
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

            int affectedRows = constraintService.DeleteCaution(cautionId, SessionInfo.UserName);
            bool deleteFlag = (affectedRows > 0);
            return Json(deleteFlag, JsonRequestBehavior.AllowGet);
        }

        #region  ConstraintSaveModel(ConstraintModel CMInput)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CMInput"></param>
        /// <returns></returns>
        public JsonResult ConstraintSaveModel(ConstraintModel CMInput)
        {
            try
            {
                #region Session check
                UserInfo SessionInfo = null;

                STP.Domain.SecurityAndUsers.UserInfo userInfo = new STP.Domain.SecurityAndUsers.UserInfo();

                if (Session["UserInfo"] == null)
                {
                    return Json(new { Success = false });
                }

                SessionInfo = (UserInfo)Session["UserInfo"];

                int userID = Convert.ToInt32(SessionInfo.UserId);

                CMInput.OrganisationName = (CMInput.OrganisationName == null || CMInput.OrganisationName == string.Empty ? SessionInfo.OrganisationName : CMInput.OrganisationName);
                CMInput.OrganisationId = SessionInfo.OrganisationId;
                #endregion

                #region
                //Can be removed if not required during editing
                if (CMInput.ConstraintReferences == null)
                {
                    CMInput.ConstraintReferences = new List<ConstraintReferences>();
                }

                if (CMInput.CautionList == null)
                {
                    CMInput.CautionList = new List<RouteCautions>();
                }

                if (CMInput.ConstraintContact == null)
                {
                    CMInput.ConstraintContact = new List<AssessmentContacts>();
                }
                #endregion
                if (SessionInfo.VehicleUnits == 692002)
                {
                    string[] splitFeet;
                    float foot = 0;
                    float inches = 0;

                    if (!string.IsNullOrEmpty(CMInput.Height))
                    {
                        splitFeet = CMInput.Height.Replace("\"", "'").Split('\'');
                        if (splitFeet.Length > 0)
                        {
                            if (splitFeet[0] != null)
                            {
                                foot = (float)(Convert.ToDouble(splitFeet[0]));
                                inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                                CMInput.MaxHeightFT = (float)Math.Round(foot + inches, 2);
                                CMInput.MaxHeightMtrs = (CMInput.MaxHeightFT != 0) ? (float)Math.Round(CMInput.MaxHeightFT * (float)0.3048, 2) : 0;
                            }
                        }
                    }
                    else
                    {
                        CMInput.Height = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(CMInput.Width))
                    {
                        splitFeet = CMInput.Width.Replace("\"", "'").Split('\'');
                        if (splitFeet.Length > 0)
                        {
                            if (splitFeet[0] != null)
                            {
                                foot = (float)(Convert.ToDouble(splitFeet[0]));
                                inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                                CMInput.MaxWidthFeet = (float)Math.Round(foot + inches, 2);
                                CMInput.MaxWidthMeters = (CMInput.MaxWidthFeet != 0) ? (float)Math.Round(CMInput.MaxWidthFeet * (float)0.3048, 2) : 0;
                            }
                        }
                    }
                    else
                    {
                        CMInput.Width = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(CMInput.Length))
                    {
                        splitFeet = CMInput.Length.Replace("\"", "'").Split('\'');
                        if (splitFeet.Length > 0)
                        {
                            if (splitFeet[0] != null)
                            {
                                foot = (float)(Convert.ToDouble(splitFeet[0]));
                                inches = (Convert.ToDouble(splitFeet[1]) > 0) ? (float)(Convert.ToDouble(splitFeet[1]) / 12) : 0;
                                CMInput.MaxLengthFeet = (float)Math.Round(foot + inches, 2);
                                CMInput.MaxLengthMeters = (CMInput.MaxLengthFeet != 0) ? (float)Math.Round(CMInput.MaxLengthFeet * (float)0.3048, 2) : 0;
                            }
                        }
                    }
                    else
                    {
                        CMInput.Length = string.Empty;
                    }

                }
                else
                {

                    if (Convert.ToDecimal(CMInput.Height) != 0)
                    {
                        CMInput.MaxHeightMtrs = (float)Convert.ToDouble(CMInput.Height);

                        CMInput.MaxHeightFT = (CMInput.MaxHeightMtrs != 0) ? (float)Math.Round(CMInput.MaxHeightMtrs / (float)0.3048, 2) : 0;
                    }

                    if (Convert.ToDecimal(CMInput.Width) != 0)
                    {
                        CMInput.MaxWidthMeters = (float)Convert.ToDouble(CMInput.Width);

                        CMInput.MaxWidthFeet = (CMInput.MaxWidthMeters != 0) ? (float)Math.Round(CMInput.MaxWidthMeters / (float)0.3048, 2) : 0;
                    }

                    if (Convert.ToDecimal(CMInput.Length) != 0)
                    {
                        CMInput.MaxLengthMeters = (float)Convert.ToDouble(CMInput.Length);

                        CMInput.MaxLengthFeet = (CMInput.MaxLengthMeters != 0) ? (float)Math.Round(CMInput.MaxLengthMeters / (float)0.3048, 2) : 0;
                    }
                }

                // Converting tonnes to KGS .....
                CMInput.MaxGrossWeightKgs = (decimal)CMInput.GrossWeight * 1000;
                CMInput.MaxGrossWeight = CMInput.GrossWeight;
                CMInput.MaxAxleWeightKgs = (decimal)CMInput.AxleWeight * 1000;
                CMInput.MaxAxleWeight = CMInput.AxleWeight;
                //--------------------------------------------------------------------------

                TempData["CMI"] = CMInput;
                TempData.Keep("CMI");
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ConstraintSaveModel, Exception: {0}", ex));
                return Json(new { Success = false });
            }
        }
        #endregion

        #region ConstraintShowReport()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ConstraintShowReport()
        {
            try
            {
                #region Session check
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion

                #region Page access check
                //Put appropriate page access checking code here
                //if (!PageAccess.GetPageAccess("60001"))
                //{
                //    return RedirectToAction("Error", "Home");
                //}
                #endregion

                ConstraintModel CMInput = new ConstraintModel();

                CMInput = (ConstraintModel)TempData["CMI"];
                TempData.Keep("CMI");

                List<ConstraintReport> constraintList = new List<ConstraintReport>();
                const string previouslyUnspecified = " (previously unspecified) set to '";
                const string preUnchecked = " (previously unchecked) set to 'checked'";
                const string preChecked = " (previously checked) set to 'unchecked'";
                const string OwnerIsContact = "Owner is contact";
                const string IsNodeConstraint = "Is node constraint";


                string enumTypeName = "ConstraintTypeEnum";
                List<InformationModel> constraintType = new List<InformationModel>();
                constraintType = informationService.GetEnumValsListByEnumType(enumTypeName);

                List<SelectListItem> constraintTypeList = new List<SelectListItem>();
                constraintTypeList = (from items in constraintType
                                      select new SelectListItem
                                      {
                                          Text = Convert.ToString(items.EnumValuesName).Substring(0, 1).ToUpper() + Convert.ToString(items.EnumValuesName).Substring(1).ToLower(),
                                          Value = Convert.ToString(items.Code)
                                      }).ToList();



                enumTypeName = "ConstraintTopologyTypeEnum";
                List<InformationModel> direction = new List<InformationModel>();
                direction = informationService.GetEnumValsListByEnumType(enumTypeName);

                List<SelectListItem> directionList = new List<SelectListItem>();

                directionList = (from items in direction
                                 select new SelectListItem
                                 {
                                     Text = Convert.ToString(items.EnumValuesName).Substring(0, 1).ToUpper() + Convert.ToString(items.EnumValuesName).Substring(1).ToLower(),
                                     Value = Convert.ToString(items.Code),
                                     Selected = false
                                 }).ToList();



                var constraintText = constraintTypeList.Where(w => w.Value == Convert.ToString(CMInput.ConstraintTypeId)).Select(w => w.Text).SingleOrDefault();

                var directionText = directionList.Where(w => w.Value == Convert.ToString(CMInput.DirectionId)).Select(w => w.Text).SingleOrDefault();

                if (CMInput.ConstraintName != CMInput.HdnConstraintName)
                {
                    ConstraintReport logText = new ConstraintReport();
                    if (CMInput.HdnConstraintName == "")
                    {
                        logText.DisplayText = "Name " + previouslyUnspecified + (CMInput.ConstraintName == string.Empty ? " unspecified " : CMInput.ConstraintName) + "'";
                    }
                    else
                    {
                        logText.DisplayText = "Name (previously " + (CMInput.HdnConstraintName == string.Empty ? " unspecified " : CMInput.HdnConstraintName) + ") set to '" + (CMInput.ConstraintName == string.Empty ? " unspecified " : CMInput.ConstraintName) + "'";
                    }

                    constraintList.Add(logText);
                }


                if (CMInput.ConstraintTypeId != CMInput.HdnConstraintTypeId)
                {
                    ConstraintReport logText = new ConstraintReport();
                    logText.DisplayText = "Type (previously " + (Convert.ToString(CMInput.ConstraintTypeName).Substring(0, 1).ToUpper() + Convert.ToString(CMInput.ConstraintTypeName).Substring(1).ToLower() == string.Empty ? " unspecified " : Convert.ToString(CMInput.ConstraintTypeName).Substring(0, 1).ToUpper() + Convert.ToString(CMInput.ConstraintTypeName).Substring(1).ToLower()) + ") set to '" + (constraintText == string.Empty ? " unspecified " : constraintText) + "'";
                    constraintList.Add(logText);
                }

                if (CMInput.DirectionId != CMInput.HdnDirectionId)
                {
                    ConstraintReport logText = new ConstraintReport();
                    logText.DisplayText = "Direction (previously " + (Convert.ToString(CMInput.DirectionName).Substring(0, 1).ToUpper() + Convert.ToString(CMInput.DirectionName).Substring(1).ToLower() == string.Empty ? " unspecified " : Convert.ToString(CMInput.DirectionName).Substring(0, 1).ToUpper() + Convert.ToString(CMInput.DirectionName).Substring(1).ToLower()) + ") set to '" + (directionText == string.Empty ? " unspecified " : directionText) + "'";
                    constraintList.Add(logText);
                }

                if (CMInput.StartDateString != CMInput.HdnStartDateString)
                {
                    ConstraintReport logText = new ConstraintReport();
                    if (CMInput.HdnStartDateString == null || CMInput.HdnStartDateString == "")
                    {
                        logText.DisplayText = "Start date " + previouslyUnspecified + (CMInput.StartDateString == string.Empty ? " unspecified " : Convert.ToDateTime(CMInput.StartDateString).ToString("ddd MMM dd yyyy")) + "'";
                    }
                    else
                    {
                        logText.DisplayText = "Start date (previously " + (CMInput.HdnStartDateString == string.Empty ? " unspecified " : Convert.ToDateTime(CMInput.HdnStartDateString).ToString("ddd MMM dd yyyy")) + ") set to '" + (CMInput.StartDateString == string.Empty ? " unspecified " : Convert.ToDateTime(CMInput.StartDateString).ToString("ddd MMM dd yyyy")) + "'";
                    }

                    constraintList.Add(logText);
                }

                if (CMInput.EndDateString != CMInput.HdnEndDateString)
                {
                    ConstraintReport logText = new ConstraintReport();
                    if (CMInput.HdnEndDateString == null || CMInput.HdnEndDateString == "")
                    {
                        logText.DisplayText = "End date " + previouslyUnspecified + (CMInput.EndDateString == string.Empty ? " unspecified " : Convert.ToDateTime(CMInput.EndDateString).ToString("ddd MMM dd yyyy")) + "'";
                    }
                    else
                    {
                        logText.DisplayText = "End date (previously " + (CMInput.HdnEndDateString == string.Empty ? " unspecified " : Convert.ToDateTime(CMInput.HdnEndDateString).ToString("ddd MMM dd yyyy")) + ") set to " + (CMInput.EndDateString == string.Empty ? " unspecified " : Convert.ToDateTime(CMInput.EndDateString).ToString("ddd MMM dd yyyy")) + "'";
                    }

                    constraintList.Add(logText);
                }
                if (SessionInfo.VehicleUnits == 692001)
                {
                    if (CMInput.Height != CMInput.HdnHeight&& CMInput.Height!="0")
                    {
                        ConstraintReport logText = new ConstraintReport();
                        if (string.IsNullOrEmpty(CMInput.HdnHeight))
                        {
                            logText.DisplayText = "Height" + previouslyUnspecified + (CMInput.Height == null || CMInput.Height == string.Empty ? " unspecified " : Convert.ToString(CMInput.Height == null ? string.Empty : CMInput.Height)) + " m'";
                        }
                        else
                        {
                            logText.DisplayText = "Height (previously " + (Convert.ToString(CMInput.HdnHeight == null ? string.Empty : CMInput.HdnHeight) == string.Empty ? " unspecified " : Convert.ToString(CMInput.HdnHeight == null ? string.Empty : CMInput.HdnHeight)) + ") set to '" + (Convert.ToString(CMInput.Height == null ? string.Empty : CMInput.Height) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Height == null ? string.Empty : CMInput.Height)) + " m'";
                        }

                        constraintList.Add(logText);
                    }

                    if (CMInput.Width != CMInput.HdnWidth&& CMInput.Width !="0")
                    {
                        ConstraintReport logText = new ConstraintReport();
                        if (string.IsNullOrEmpty(CMInput.HdnWidth))
                        {
                            logText.DisplayText = "Width" + previouslyUnspecified + (Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width)) + " m'";
                        }
                        else
                        {
                            logText.DisplayText = "Width (previously " + (Convert.ToString(CMInput.HdnWidth == null ? string.Empty : CMInput.HdnWidth) == string.Empty ? " unspecified " : Convert.ToString(CMInput.HdnWidth == null ? string.Empty : CMInput.HdnWidth)) + ") set to '" + (Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width)) + " m'";
                        }

                        constraintList.Add(logText);
                    }

                    if (CMInput.Length != CMInput.HdnLength&& CMInput.Length !="0")
                    {
                        ConstraintReport logText = new ConstraintReport();
                        if (string.IsNullOrEmpty(CMInput.HdnLength))
                        {
                            logText.DisplayText = "Length" + previouslyUnspecified + (Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length)) + " m'";
                        }
                        else
                        {
                            logText.DisplayText = "Length (previously " + (Convert.ToString(CMInput.HdnLength == null ? string.Empty : CMInput.HdnLength) == string.Empty ? " unspecified " : Convert.ToString(CMInput.HdnLength == null ? string.Empty : CMInput.HdnLength)) + ") set to '" + (Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length)) + " m'";
                        }

                        constraintList.Add(logText);
                    }
                }
                else
                {
                    if (Convert.ToString(CMInput.Height).Replace("''", "\"") != Convert.ToString(CMInput.HdnHeight == null ? string.Empty : CMInput.HdnHeight))
                    {
                        ConstraintReport logText = new ConstraintReport();
                        if (string.IsNullOrEmpty(CMInput.HdnHeight))
                        {
                            logText.DisplayText = "Height" + previouslyUnspecified + (Convert.ToString(CMInput.Height == null ? string.Empty : CMInput.Height) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Height == null ? string.Empty : CMInput.Height)) + " ft/in'";
                        }
                        else
                        {
                            logText.DisplayText = "Height (previously " + (Convert.ToString(CMInput.HdnHeight == null ? string.Empty : CMInput.HdnHeight) == string.Empty ? " unspecified " : Convert.ToString(CMInput.HdnHeight == null ? string.Empty : CMInput.HdnHeight)) + ") set to '" + (Convert.ToString(CMInput.Height == null ? string.Empty : CMInput.Height) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Height == null ? string.Empty : CMInput.Height)) + " ft/in'";
                        }

                        constraintList.Add(logText);
                    }

                    if (Convert.ToString(CMInput.Width).Replace("''", "\"") != Convert.ToString(CMInput.HdnWidth == null ? string.Empty : CMInput.HdnWidth))
                    {
                        ConstraintReport logText = new ConstraintReport();
                        if (string.IsNullOrEmpty(CMInput.HdnWidth))
                        {
                            logText.DisplayText = "Width" + previouslyUnspecified + (Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width)) + " ft/in'";
                        }
                        else
                        {
                            logText.DisplayText = "Width (previously " + (Convert.ToString(CMInput.HdnWidth == null ? string.Empty : CMInput.HdnWidth) == string.Empty ? " unspecified " : Convert.ToString(CMInput.HdnWidth == null ? string.Empty : CMInput.HdnWidth)) + ") set to '" + (Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Width == null ? string.Empty : CMInput.Width)) + " ft/in'";
                        }

                        constraintList.Add(logText);
                    }

                    if (Convert.ToString(CMInput.Length).Replace("''", "\"") != Convert.ToString(CMInput.HdnLength == null ? string.Empty : CMInput.HdnLength))
                    {
                        ConstraintReport logText = new ConstraintReport();
                        if (string.IsNullOrEmpty(CMInput.HdnLength))
                        {
                            logText.DisplayText = "Length" + previouslyUnspecified + (Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length)) + " ft/in'";
                        }
                        else
                        {
                            logText.DisplayText = "Length (previously " + (Convert.ToString(CMInput.HdnLength == null ? string.Empty : CMInput.HdnLength) == string.Empty ? " unspecified " : Convert.ToString(CMInput.HdnLength == null ? string.Empty : CMInput.HdnLength)) + ") set to '" + (Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length) == string.Empty ? " unspecified " : Convert.ToString(CMInput.Length == null ? string.Empty : CMInput.Length)) + " ft/in'";
                        }

                        constraintList.Add(logText);
                    }
                }

                if (CMInput.MaxGrossWeightKgs != CMInput.HdnMaxGrossWeightKgs)
                {
                    ConstraintReport logText = new ConstraintReport();
                    if (CMInput.HdnMaxGrossWeightKgs == 0)
                    {
                        logText.DisplayText = "Gross weight" + previouslyUnspecified + (Convert.ToString(CMInput.GrossWeight == 0 ? " unspecified " : Convert.ToString(CMInput.GrossWeight))) + " t'";
                    }
                    else
                    {
                        logText.DisplayText = "Gross weight (previously " + (Convert.ToString(CMInput.HdnMaxGrossWeightKgs == 0 ? " unspecified " : Convert.ToString(CMInput.HdnMaxGrossWeightKgs))) + ") set to '" + (Convert.ToString(CMInput.GrossWeight == 0 ? " unspecified " : Convert.ToString(CMInput.GrossWeight))) + " t'";
                    }

                    constraintList.Add(logText);
                }

                if (CMInput.MaxAxleWeightKgs != CMInput.HdnMaxAxleWeightKgs)
                {
                    ConstraintReport logText = new ConstraintReport();
                    if (CMInput.HdnMaxAxleWeightKgs == 0)
                    {
                        logText.DisplayText = "Axle weight" + previouslyUnspecified + (Convert.ToString(CMInput.AxleWeight == 0 ? " unspecified " : Convert.ToString(CMInput.AxleWeight))) + " t'";
                    }
                    else
                    {
                        logText.DisplayText = "Axle weight (previously " + (Convert.ToString(CMInput.HdnMaxAxleWeightKgs == 0 ? " unspecified " : Convert.ToString(CMInput.HdnMaxAxleWeightKgs))) + ") set to '" + (Convert.ToString(CMInput.AxleWeight == 0 ? " unspecified " : Convert.ToString(CMInput.AxleWeight))) + " t'";
                    }

                    constraintList.Add(logText);
                }

                if (CMInput.OwnerIsContactFlag != CMInput.HdnOwnerIsContactFlag)
                {
                    ConstraintReport logText = new ConstraintReport();
                    if (CMInput.HdnOwnerIsContactFlag == false)
                    {
                        logText.DisplayText = OwnerIsContact + preUnchecked;
                    }
                    else
                    {
                        logText.DisplayText = OwnerIsContact + preChecked;
                    }

                    constraintList.Add(logText);
                }

                if (CMInput.IsNodeConstraintFlag != CMInput.HdnIsNodeConstraintFlag)
                {
                    ConstraintReport logText = new ConstraintReport();
                    if (CMInput.HdnIsNodeConstraintFlag == false)
                    {
                        logText.DisplayText = IsNodeConstraint + preUnchecked;
                    }
                    else
                    {
                        logText.DisplayText = IsNodeConstraint + preChecked;
                    }

                    constraintList.Add(logText);
                }


                ViewBag.ConstraintID = CMInput.ConstraintId;


                if (constraintList.Count == 0)
                {
                    ConstraintReport logText = new ConstraintReport();
                    logText.DisplayText = "No changes to save";
                    constraintList.Add(logText);
                    ViewBag.DisableSaveButton = true;
                }
                else
                {
                    TempData["constraintList"] = constraintList;
                    TempData.Keep("constraintList");
                    ViewBag.DisableSaveButton = false;
                }
                return Json(new { Success = true });

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ConstraintShowReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region CreateCaution(long ConstraintID, long CautionId, string mode)
        /// <summary>
        /// Create caution   -- Manage caution
        /// </summary>
        /// <param name="ConstraintID">Constraint Id</param>
        /// <returns>Save caution</returns>
        public ActionResult CreateCaution(long ConstraintID, long CautionId, string mode, int FirstCautionFlag = 0)
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

                ConstraintModel ConstraintmodelBase = new ConstraintModel();
                ConstraintModel tempConstrModel = null;
                if (TempData["CMI"] != null)
                {
                    tempConstrModel = (ConstraintModel)TempData["CMI"];
                    TempData.Keep("CMI");

                }

                ConstraintmodelBase.Mode = mode;
                ConstraintmodelBase.ConstraintId = ConstraintID;
                ViewBag.ConstraintID = ConstraintID;
                if (mode == "add")
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(TempData["OrganisationName"])))
                    {
                        ConstraintmodelBase.ConstraintId = ConstraintID;
                        ConstraintmodelBase.OrganisationName = Convert.ToString(TempData["OrganisationName"]);
                        ConstraintmodelBase.OwnerOrganisationId = Convert.ToInt64(Convert.ToString(TempData["OWNER_ORG_ID"]));
                        ConstraintmodelBase.ConstraintCode = Convert.ToString(TempData["ConstraintCode"]);
                        ConstraintmodelBase.ConstraintName = Convert.ToString(TempData["ConstraintName"]);
                        TempData.Keep("OrganisationName");
                        TempData.Keep("OWNER_ORG_ID");
                    }
                    else if (tempConstrModel != null)
                    {
                        ConstraintmodelBase.OrganisationName = tempConstrModel.OrganisationName;
                        ConstraintmodelBase.OwnerOrganisationId = tempConstrModel.OwnerOrganisationId;
                        ConstraintmodelBase.ConstraintCode = tempConstrModel.ConstraintCode;
                        ConstraintmodelBase.ConstraintName = tempConstrModel.ConstraintName;
                        TempData["OrganisationName"] = tempConstrModel.OrganisationName;
                        TempData["OWNER_ORG_ID"] = tempConstrModel.OwnerOrganisationId;
                        TempData.Keep("OrganisationName");
                        TempData.Keep("OWNER_ORG_ID");
                    }
                    ConstraintmodelBase.SelectedType = ActionType.StandardCaution;
                    ConstraintmodelBase.CreatorIsContact = false;
                    ConstraintmodelBase.Height = string.Empty;
                    ConstraintmodelBase.Width = string.Empty;
                    ConstraintmodelBase.Length = string.Empty;
                    ConstraintmodelBase.Speed = string.Empty;
                    TempData["ConstraintmodelBase"] = ConstraintmodelBase;
                }
                else if (mode == "edit")//get data and fill
                {
                    ConstraintmodelBase = constraintService.GetCautionDetails(CautionId);
                    ConstraintmodelBase.CreatorIsContact = ConstraintmodelBase.OwnerIsContactFlag;

                    //meters , feet and inches
                    if (SessionInfo.VehicleUnits == 692002)
                    {

                        if (ConstraintmodelBase.MaxHeight != 0)
                        {
                            float MAX_HEIGHT_MTRS_ = ConstraintmodelBase.MaxHeight;
                            int FeetL = (int)MAX_HEIGHT_MTRS_;
                            float InchL = MAX_HEIGHT_MTRS_ - FeetL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            ConstraintmodelBase.Height = FeetL + "'" + height + "\"";
                        }
                        else
                        {
                            ConstraintmodelBase.Height = string.Empty;
                        }
                        if (ConstraintmodelBase.MaxWidth != 0)
                        {
                            float MAX_WIDTH_MTRS_ = ConstraintmodelBase.MaxWidth;
                            int widthL = (int)MAX_WIDTH_MTRS_;
                            float InchL = MAX_WIDTH_MTRS_ - widthL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            ConstraintmodelBase.Width = widthL + "'" + height + "\"";
                        }
                        else
                        {
                            ConstraintmodelBase.Width = string.Empty;
                        }
                        if (ConstraintmodelBase.MaxLength != 0)
                        {
                            float MAX_LENGTH_MTRS_ = ConstraintmodelBase.MaxLength;
                            int lengthL = (int)MAX_LENGTH_MTRS_;
                            float InchL = MAX_LENGTH_MTRS_ - lengthL;
                            int height = 0;
                            InchL = InchL * 12;
                            height = (int)InchL;
                            ConstraintmodelBase.Length = lengthL + "'" + height + "\"";
                        }
                        else
                        {
                            ConstraintmodelBase.Length = string.Empty;
                        }
                        if (ConstraintmodelBase.MinSpeed != 0)
                        {
                            ConstraintmodelBase.Speed = ConstraintmodelBase.MinSpeed.ToString();
                        }
                        else
                        {
                            ConstraintmodelBase.Speed = string.Empty;
                        }

                    }
                    else
                    {
                        if (ConstraintmodelBase.MaxHeightMtrs != 0)
                        {
                            ConstraintmodelBase.Height = Convert.ToString(ConstraintmodelBase.MaxHeightMtrs);
                        }
                        else
                        {
                            ConstraintmodelBase.Height = string.Empty;
                        }
                        if (ConstraintmodelBase.MaxWidthMeters != 0)
                        {
                            ConstraintmodelBase.Width = Convert.ToString(ConstraintmodelBase.MaxWidthMeters);
                        }
                        else
                        {
                            ConstraintmodelBase.Width = string.Empty;
                        }
                        if (ConstraintmodelBase.MaxLengthMeters != 0)
                        {
                            ConstraintmodelBase.Length = Convert.ToString(ConstraintmodelBase.MaxLengthMeters);
                        }
                        else
                        {
                            ConstraintmodelBase.Length = string.Empty;
                        }
                        if (ConstraintmodelBase.MinSpeedKph != 0)
                        {
                            ConstraintmodelBase.Speed = Convert.ToString(ConstraintmodelBase.MinSpeedKph);
                        }
                        else
                        {
                            ConstraintmodelBase.Speed = string.Empty;
                        }
                    }

                    XmlDocument Doc = new XmlDocument();

                    Doc.LoadXml(ConstraintmodelBase.SpecificAction);
                    XmlNodeList parentNode = Doc.GetElementsByTagName("SpecificAction");
                    if (parentNode.Count > 0)
                    {
                        string text = parentNode[0].InnerXml.Replace("\"_\"", "&nbsp;");
                        text=parentNode[0].InnerXml.Replace("_", "&nbsp;");
                        ConstraintmodelBase.DirectionName = text;
                        ConstraintmodelBase.SelectedType = ActionType.SpecificAction;//#2313

                        foreach (XmlNode childrenNode in parentNode)
                        {
                            if (childrenNode.InnerXml.ToLower().Contains("bold"))
                            {
                                ConstraintmodelBase.Bold = true;
                            }
                            if (childrenNode.InnerXml.ToLower().Contains("italic"))
                            {
                                ConstraintmodelBase.Italic = true;
                            }
                            if (childrenNode.InnerXml.ToLower().Contains("underline"))
                            {
                                ConstraintmodelBase.UnderLine = true;
                            }
                        }
                    }
                    else
                    {
                        XmlNodeList parentNodeStandard = Doc.GetElementsByTagName("caution:SpecificAction");
                        ConstraintmodelBase.DirectionName = parentNodeStandard[0].InnerText;
                        ConstraintmodelBase.SelectedType = ActionType.StandardCaution;//#2313
                    }
                    TempData["ConstraintmodelBase"] = ConstraintmodelBase;
                }
                else//get data and fill when it return from CautionAddReport page
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["Constraintmodelalter"])))
                    {
                        ConstraintmodelBase = (ConstraintModel)TempData["Constraintmodelalter"];
                        ConstraintmodelBase.DirectionName = ConstraintmodelBase.DirectionName.Replace("\"_\"", "&nbsp;");
                        ConstraintmodelBase.DirectionName = ConstraintmodelBase.DirectionName.Replace("_", "&nbsp;");

                    }
                }
                if (mode == "edit"|| mode== "add")
                {
                    ConstraintmodelBase.GrossWeight = (double)((ConstraintmodelBase.MaxGrossWeightKgs != 0) ? Normalize(ConstraintmodelBase.MaxGrossWeightKgs) / 1000 : 0);
                    ConstraintmodelBase.AxleWeight = (double)((ConstraintmodelBase.MaxAxleWeightKgs != 0) ? Normalize(ConstraintmodelBase.MaxAxleWeightKgs) / 1000 : 0);
                }

                //-----------------------------------------------------------------------------------------------

                ViewBag.UOM = SessionInfo.VehicleUnits;
                ViewBag.mode = mode;
                TempData.Keep("Constraintmodelalter");
                TempData.Keep("ConstraintmodelBase");
                ViewBag.FirstCautionFlag = FirstCautionFlag;
                return View(ConstraintmodelBase);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/CreateCaution, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        public decimal Normalize(decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }
        /// <summary>
        /// Save cautions
        /// </summary>
        /// <param name="constraintModel">ConstraintModel</param>
        /// <returns>Save cautions</returns>
        [HttpPost]
        public JsonResult SaveCautions(ConstraintModel constraintModel)
        {
            #region Session Check
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            #endregion

            ConstraintModel constraintModelAlter = new ConstraintModel();
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["Constraintmodelalter"])))
            {
                constraintModelAlter = (ConstraintModel)TempData["Constraintmodelalter"];
            }
            if (constraintModelAlter.CautionId != 0)
            {
                ViewBag.mode = "edit";
                constraintModelAlter.Mode = "edit";
            }
            else
            {
                ViewBag.mode = "add";
                constraintModelAlter.Mode = "add";
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

            if (constraintModelAlter.SelectedType == ActionType.StandardCaution)//#2313
            {
                constraintModelAlter.SpecificAction = specificationStart + constraintModelAlter.DirectionName + specificationEnd;
            }
            else
            {
                if (constraintModelAlter.Bold == false && constraintModelAlter.Italic == false && constraintModelAlter.UnderLine == false)
                {
                    constraintModelAlter.SpecificAction = otherSpecification + constraintModelAlter.DirectionName + otherSpecificationEnd;
                }
                else
                {
                    StringBuilder xmlAttribute = new StringBuilder();
                    xmlAttribute.Append(otherSpecification);
                    if (constraintModelAlter.Bold == true)
                    {
                        xmlAttribute.Append(boldSpecStart);
                    }
                    if (constraintModelAlter.Italic == true)
                    {
                        xmlAttribute.Append(italicSpecStart);
                    }
                    if (constraintModelAlter.UnderLine == true)
                    {
                        xmlAttribute.Append(underlineSpecStart);
                    }
                    xmlAttribute.Append(constraintModelAlter.DirectionName);
                    if (constraintModelAlter.UnderLine == true)
                    {
                        xmlAttribute.Append(underlineSpecEnd);
                    }
                    if (constraintModelAlter.Italic == true)
                    {
                        xmlAttribute.Append(italicSpecEnd);
                    }
                    if (constraintModelAlter.Bold == true)
                    {
                        xmlAttribute.Append(boldSpecEnd);
                    }
                    xmlAttribute.Append(otherSpecificationEnd);
                    constraintModelAlter.SpecificAction = xmlAttribute.ToString();
                }
            }

            if (constraintModelAlter.CreatorIsContact)
            {
                constraintModelAlter.OwnerIsContact = Convert.ToInt16(SessionInfo.UserId);
            }

            
            constraintModelAlter.MaxGrossWeightUnit = 240005;
            constraintModelAlter.MaxAxleWeightUnit = 240005;
            

            constraintModelAlter.MaxAxleWeightUnit = 208001;
            constraintModelAlter.MaxWidthUnit = 208001;
            constraintModelAlter.MaxLengthUnit = 208001;
            constraintModelAlter.MinSpeedUnit = 229001;

            if (SessionInfo.VehicleUnits == 692002)//Imperial
            {
                if (!string.IsNullOrEmpty(constraintModelAlter.Height))
                {
                    if (constraintModelAlter.MaxHeight == 0)
                        constraintModelAlter.MaxHeight = (constraintModelAlter.MaxHeightMtrs != 0) ? (float)Math.Round(constraintModelAlter.MaxHeightMtrs / (float)0.3048, 2) : constraintModelAlter.MaxHeightMtrs;
                }
                else
                {
                    constraintModelAlter.Height = string.Empty;// if null then set it to empty string.
                }

                if (!string.IsNullOrEmpty(constraintModelAlter.Width))
                {
                    if (constraintModelAlter.MaxWidth == 0)
                        constraintModelAlter.MaxWidth = (constraintModelAlter.MaxWidthMeters != 0) ? (float)Math.Round(constraintModelAlter.MaxWidthMeters / (float)0.3048, 2) : constraintModelAlter.MaxWidthMeters;
                }
                else
                {
                    constraintModelAlter.Width = string.Empty;// if null then set it to empty string.
                }
                if (!string.IsNullOrEmpty(constraintModelAlter.Length))
                {
                    if (constraintModelAlter.MaxLength == 0)
                        constraintModelAlter.MaxLength = (constraintModelAlter.MaxLengthMeters != 0) ? (float)Math.Round(constraintModelAlter.MaxLengthMeters / (float)0.3048, 2) : constraintModelAlter.MaxLengthMeters;
                }
                else
                {
                    constraintModelAlter.Length = string.Empty;// if null then set it to empty string.
                }
                if (!string.IsNullOrEmpty(constraintModelAlter.Speed))
                {
                    if (constraintModelAlter.MinSpeed == 0)
                    {
                        constraintModelAlter.MinSpeed = (float)(Math.Round((constraintModelAlter.MinSpeedKph * (float)0.6213), 2));
                    }                    
                }
                else
                {
                    constraintModelAlter.Speed = string.Empty;// if null then set it to empty string.
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(constraintModelAlter.Height))
                {
                    if (constraintModelAlter.MaxHeight == 0)
                    {
                        constraintModelAlter.MaxHeightMtrs = (float)(Convert.ToDouble(constraintModelAlter.Height));
                        constraintModelAlter.MaxHeight = (constraintModelAlter.MaxHeightMtrs != 0) ? (float)Math.Round(constraintModelAlter.MaxHeightMtrs / (float)0.3048, 2) : constraintModelAlter.MaxHeightMtrs;
                    }
                    else
                    {
                        constraintModelAlter.MaxHeightMtrs = (float)(Convert.ToDouble(constraintModelAlter.Height));
                        constraintModelAlter.MaxHeight = (constraintModelAlter.MaxHeightMtrs != 0) ? (float)Math.Round(constraintModelAlter.MaxHeightMtrs / (float)0.3048, 2) : constraintModelAlter.MaxHeightMtrs;
                    }
                }
                else
                {
                    constraintModelAlter.Height = string.Empty;// if null then set it to empty string.
                }
                if (!string.IsNullOrEmpty(constraintModelAlter.Width))
                {
                    if (constraintModelAlter.MaxWidth == 0)
                    {
                        constraintModelAlter.MaxWidthMeters = (float)(Convert.ToDouble(constraintModelAlter.Width));
                        constraintModelAlter.MaxWidth = (constraintModelAlter.MaxWidthMeters != 0) ? (float)Math.Round(constraintModelAlter.MaxWidthMeters / (float)0.3048, 2) : constraintModelAlter.MaxWidthMeters;
                    }
                    else
                    {
                        constraintModelAlter.MaxWidthMeters = (float)(Convert.ToDouble(constraintModelAlter.Width));
                        constraintModelAlter.MaxWidth = (constraintModelAlter.MaxWidthMeters != 0) ? (float)Math.Round(constraintModelAlter.MaxWidthMeters / (float)0.3048, 2) : constraintModelAlter.MaxWidthMeters;
                    }
                }
                else
                {
                    constraintModelAlter.Width = string.Empty;// if null then set it to empty string.
                }

                if (!string.IsNullOrEmpty(constraintModelAlter.Length))
                {
                    if (constraintModelAlter.MaxLength == 0)
                    {
                        constraintModelAlter.MaxLengthMeters = (float)(Convert.ToDouble(constraintModelAlter.Length));
                        constraintModelAlter.MaxLength = (constraintModelAlter.MaxLengthMeters != 0) ? (float)Math.Round(constraintModelAlter.MaxLengthMeters / (float)0.3048, 2) : constraintModelAlter.MaxLengthMeters;
                    }
                    else
                    {
                        constraintModelAlter.MaxLengthMeters = (float)(Convert.ToDouble(constraintModelAlter.Length));
                        constraintModelAlter.MaxLength = (constraintModelAlter.MaxLengthMeters != 0) ? (float)Math.Round(constraintModelAlter.MaxLengthMeters / (float)0.3048, 2) : constraintModelAlter.MaxLengthMeters;
                    }
                }
                else
                {
                    constraintModelAlter.Length = string.Empty;// if null then set it to empty string.
                }

                if (!string.IsNullOrEmpty(constraintModelAlter.Speed))
                {
                    if (constraintModelAlter.MinSpeed == 0)
                    {
                        constraintModelAlter.MinSpeedKph = (float)(Convert.ToDouble(constraintModelAlter.Speed));
                        constraintModelAlter.MinSpeed = (constraintModelAlter.MinSpeedKph != 0) ? (float)Math.Round(constraintModelAlter.MinSpeedKph * (float)0.6213,2) : constraintModelAlter.MinSpeedKph;
                    }
                    else
                    {
                        constraintModelAlter.MinSpeedKph = (float)(Convert.ToDouble(constraintModelAlter.Speed));
                    }
                }
                else
                {
                    constraintModelAlter.Speed = string.Empty;// if null then set it to empty string.
                }
            }
            //--------------------------------------------------------------------------------------
            var hashChangeList = (dynamic)TempData["changeList"];
            if (hashChangeList == null)
            {
                TempData["Constraintmodelalter"] = constraintModelAlter;
                TempData.Keep("Constraintmodelalter");
                TempData.Keep("ConstraintmodelBase");
                return Json("No Change", JsonRequestBehavior.AllowGet);
            }
            constraintModelAlter.OwnerOrganisationId = (int)SessionInfo.OrganisationId;
            bool result = constraintService.SaveCautions(constraintModelAlter);

            #region CONSTRAINT_LOGS
            List<ConstraintLogModel> constraintLogsModel = new List<ConstraintLogModel>();
            ConstraintLogModel constraintLogModel;
            ConstraintModel conModelalter = (ConstraintModel)TempData["Constraintmodelalter"];

            foreach (System.Collections.Generic.KeyValuePair<string, string> hashVal in hashChangeList)
            {
                constraintLogModel = new ConstraintLogModel();

                constraintLogModel.Author = SessionInfo.UserName;
                constraintLogModel.Amendment1 = "\"" + conModelalter.ConstraintName + "\"(" + conModelalter.ConstraintId + ") Caution:\"" + conModelalter.CautionName + "\"(" + conModelalter.CautionId + ") - " + hashVal.Value;
                constraintLogModel.Amendment2 = string.Empty;
                constraintLogModel.Amendment3 = string.Empty;
                constraintLogModel.ConstraintCode = conModelalter.ConstraintCode;
                constraintLogsModel.Add(constraintLogModel);
            }

            if (conModelalter.Mode == "add")
            {
                constraintLogModel = new ConstraintLogModel();

                constraintLogModel.Author = SessionInfo.UserName;
                constraintLogModel.Amendment1 = "\"" + conModelalter.ConstraintName + "\"(" + conModelalter.ConstraintId + ") Caution:\"" + conModelalter.CautionName + "\"(" + conModelalter.CautionId + ") - has been added";
                constraintLogModel.Amendment2 = string.Empty;
                constraintLogModel.Amendment3 = string.Empty;
                constraintLogModel.ConstraintCode = conModelalter.ConstraintCode;
                constraintLogsModel.Add(constraintLogModel);
            }
            constraintService.UpdateConstraintLog(constraintLogsModel);
            #endregion

            if (result)
            {
                TempData.Remove("ConstraintmodelBase");
                TempData.Remove("Constraintmodelalter");

            }
            else
            {
                TempData.Keep("Constraintmodelalter");
                TempData.Keep("ConstraintmodelBase");

            }

            TempData.Remove("changeList");
            TempData.Remove("ConstraintCode");
            TempData.Remove("ConstraintName");
            TempData.Keep("OrganisationName");
            TempData.Keep("OWNER_ORG_ID");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region UpdateConstraint()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateConstraint()
        {
            try
            {

                #region Session check
                UserInfo SessionInfo = null;

                STP.Domain.SecurityAndUsers.UserInfo userInfo = new STP.Domain.SecurityAndUsers.UserInfo();

                if (Session["UserInfo"] == null)
                {
                    return Json(new { Success = false });
                }

                SessionInfo = (UserInfo)Session["UserInfo"];

                #endregion

                ConstraintModel CM = new ConstraintModel();

                CM = (ConstraintModel)TempData["CMI"];

                long ConstraintID = 0;
                bool Result = false;
                CM.MaxGrossWeightUnit = 240005;
                CM.MaxAxleWeightUnit = 240005;
                ConstraintID = constraintService.UpdateConstraint(CM, Convert.ToInt32(SessionInfo.UserId));
                #region CONSTRAINT_LOGS

                List<ConstraintReport> constraintList = new List<ConstraintReport>();
                constraintList = (List<ConstraintReport>)TempData["constraintList"];
                List<ConstraintLogModel> constraintLogsModel = new List<ConstraintLogModel>();
                ConstraintLogModel constraintLogModel;
                foreach (ConstraintReport report in constraintList)
                {
                    constraintLogModel = new ConstraintLogModel();

                    constraintLogModel.Author = SessionInfo.UserName;
                    constraintLogModel.Amendment1 = "\"" + CM.ConstraintName + "\"(" + CM.ConstraintId + ") - " + report.DisplayText;
                    constraintLogModel.Amendment2 = string.Empty;
                    constraintLogModel.Amendment3 = string.Empty;
                    constraintLogModel.ConstraintCode = CM.ConstraintCode;

                    constraintLogsModel.Add(constraintLogModel);
                }

                constraintService.UpdateConstraintLog(constraintLogsModel);
                #endregion

                if (ConstraintID > 0)
                {
                TempData.Remove("constraintList");
                    Result = true;
                }
                else
                {
                    TempData.Remove("constraintList");

                }

                return Json(new { Success = Result });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/UpdateConstraint, Exception: {0}", ex));
                return Json(new { Success = false });
            }
        }
        #endregion

        #region  ViewCaution(long cautionID, long constraintID)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cautionID"></param>
        /// <param name="constraintID"></param>
        /// <returns></returns>
        public ActionResult ViewCaution(long cautionID, long constraintID, bool flageditmode = true)
        {

            UserInfo SessionInfo = null;

            UserInfo userInfo = new UserInfo();

            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.flageditmode = flageditmode;
            //Put appropriate page access checking code here
            //if (!PageAccess.GetPageAccess("60001"))
            //{
            //    return RedirectToAction("Error", "Home");
            //}

            SessionInfo = (UserInfo)Session["UserInfo"];

            int UOM = SessionInfo.VehicleUnits;

            ViewBag.UOM = UOM;

            ConstraintModel CM = constraintService.GetCautionDetails(cautionID);
            CM.CreatorIsContact = CM.OwnerIsContactFlag;

            TempData["Constraintmodelalter"] = CM;
            TempData.Keep("Constraintmodelalter");
            ViewBag.ConstraintID = constraintID;
            XmlDocument Doc = new XmlDocument();

            Doc.LoadXml(CM.SpecificAction);
            XmlNodeList parentNode = Doc.GetElementsByTagName("SpecificAction");
            if (parentNode.Count > 0)
            {
                CM.SpecificActionXML = CM.SpecificAction;
                CM.SpecificAction = parentNode[0].InnerText;
                CM.SpecificActionBool = false;
            }
            else
            {
                XmlNodeList parentNodebold = Doc.GetElementsByTagName("caution:SpecificAction");
                CM.SpecificAction = parentNodebold[0].InnerText;
                CM.SpecificActionBool = true;
            }

            if (UOM == 692002)
            {
                CM.MaxHeightFT = (float)CM.MaxHeight;
                CM.MaxWidthFeet = (float)CM.MaxWidth;
                CM.MaxLengthFeet = (float)CM.MaxLength;
                CM.MinSpeedKph = (float)CM.MinSpeedKph;
                CM.MinSpeed = (float)CM.MinSpeed;
            }

            // converting kgs to tonnes
            CM.GrossWeight = (double)((CM.MaxGrossWeightKgs != 0) ? CM.MaxGrossWeightKgs / 1000 : 0);
            CM.AxleWeight = (double)((CM.MaxAxleWeightKgs != 0) ? CM.MaxAxleWeightKgs / 1000 : 0);
            //---------------------------------------------------------------------------

            TempData.Keep("OrganisationName");
            TempData.Keep("OWNER_ORG_ID");
            return View(CM);
        }
        #endregion


        #region  DeleteConstraint(long ConID)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConID"></param>
        /// <returns></returns>
        public JsonResult DeleteConstraint(long ConID)
        {
            try
            {
                #region Session check
                UserInfo SessionInfo = null;

                UserInfo userInfo = new UserInfo();

                if (Session["UserInfo"] == null)
                {
                    return Json(new { Success = false });
                }

                SessionInfo = (UserInfo)Session["UserInfo"];

                #endregion

                long result=constraintService.DeleteConstraint(ConID, SessionInfo.UserName);
                if (result == 0)
                {
                    return Json(new { Success = false });
                }
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/DeleteConstraint, Exception: {0}", ex));
                return Json(new { Success = false });
            }
        }
        #endregion
        #region public ActionResult StructSummaryFilter()
        /// <summary>
        /// Structure summary filter that goes to the left panel
        /// </summary>
        /// <returns>partial view</returns>
        public ActionResult ConstraintSummaryFilter(int? page, int? pageSize,int ? sortOrder = null, int? sortType = null)
        {

            string structType = string.Empty;
            SearchConstraintsFilter objSearch = new SearchConstraintsFilter();
            if (Session["g_ConstSearch"] != null)
            {
                objSearch = (SearchConstraintsFilter)Session["g_ConstSearch"];
                switch (objSearch.ConstraintType)
                {
                    case "510001":
                        objSearch.ConstraintType = "Underbridge";
                        break;

                    case "510002":
                        objSearch.ConstraintType = "Overbridge";
                        break;

                    case "510003":
                        objSearch.ConstraintType = "Under and over bridge";
                        break;

                    case "510004":
                        objSearch.ConstraintType = "Level crossing";
                        break;

                }
                structType = objSearch.ConstraintType;
            }

            /*method to fill struct type dropdown list*/
            GetStructureTypeDropDown(structType);

            ViewBag.page=page;
            ViewBag.pageSize=pageSize;
            ViewBag.sortOrder=sortOrder;
            ViewBag.sortType=sortType;
            return PartialView(objSearch);
        }
        #endregion
        #region private void GetICAMethod()
        private void GetStructureTypeDropDown(string type)
        {
            List<DropDown> dropdownObjList = new List<DropDown>();
            DropDown dropdownObj = null;
            dropdownObj = new DropDown();
            dropdownObj.Id = 1;
            dropdownObj.Value = "Point";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 2;
            dropdownObj.Value = "Line";
            dropdownObjList.Add(dropdownObj);

            dropdownObj = new DropDown();
            dropdownObj.Id = 3;
            dropdownObj.Value = "Area";
            dropdownObjList.Add(dropdownObj);
            ViewBag.structType = new SelectList(dropdownObjList, "Value", "Value", type);
        }
        #endregion
        #region public ActionResult SaveStructSearch(SearchStruct objSearch)
       // public ActionResult SaveConstSearch(SearchConstraintsFilter objSearch, string responsables, bool checkResp = false, string Mode = "", int Flag = 0)
        public ActionResult SaveConstSearch(SearchConstraintsFilter objSearch,  bool checkResp = false, bool IsOwnerContact = false, bool IsValid = false, string Mode = "", int Flag = 0, int? page=null,int? pageSize=null,int? sortOrder=null,int? sortType=null)

        {
            Session["g_ConstSearch"] = objSearch;
            if (!string.IsNullOrEmpty(Mode))
                TempData["ModeSearch"] = Mode;
            if (Flag == 1)
            {
                TempData["EditFlag"] = 1;
            }
            TempData["PageNum"] =page!=null?page: 1;
            
            // return RedirectToAction("StructureList", new { Mode = TempData["ModeSearch"] });
            return RedirectToAction("GetConstraintList", new
            {
                B7vy6imTleYsMr6Nlv7VQ =
                        STP.Web.Helpers.EncryptionUtility.Encrypt("sortOrder=" + sortOrder +
                        "&sortType=" + sortType +
                        "&page=" + page +
                        "&pageSize=" + pageSize)
            });

        }
        #endregion
        #region  ViewConstraint(int ConstraintID)
        /// <summary>
        /// The View constraint will fetch data based on the constraint ID which is the PK of the constraints table
        /// </summary>
        /// <param name="ConstraintID"></param>
        /// <returns></returns>
        public ActionResult ViewConstraint(int ConstraintID, bool flageditmode = true)
        {
            try
            {
                #region Session check

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];
                int userTypeID = SessionInfo.UserTypeId;
                long orgId = SessionInfo.OrganisationId;
                #endregion

                #region For document status viewer
                if (SessionInfo.HelpdeskRedirect == "true")
                {
                    ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
                }
                #endregion

                int UOM = SessionInfo.VehicleUnits;
                ViewBag.UOM = UOM;
                ViewBag.ConstraintID = ConstraintID;
                ViewBag.flageditmode = flageditmode;

                ConstraintModel CM = constraintService.GetConstraintDetails(ConstraintID);
                CM.MaxLengthMeters = CM.HdnMaxLenMeters;
                CM.MaxHeightMtrs = CM.HdnMaxHeightMtrs;
                CM.MaxWidthMeters = CM.HdnMaxWidthMeters;
                CM.HdnMaxGrossWeightKgs = (decimal)CM.GrossWeight;
                CM.HdnMaxAxleWeightKgs = (decimal)CM.AxleWeight;
                // Converting KGS to Tonnes.
                CM.GrossWeight = (double)((CM.MaxGrossWeightKgs != 0) ? CM.MaxGrossWeightKgs / 1000 : 0);
                CM.AxleWeight = (double)((CM.MaxAxleWeightKgs != 0) ? CM.MaxAxleWeightKgs / 1000 : 0);
                //---------------------------------------------------------------------------

                ViewBag.chk_Isdeleted = "1";
                if (CM.IsDeleted == 1)
                {
                    ViewBag.chk_Isdeleted = "0";
                }
                if (CM.ConstraintId == 0)
                {
                    ViewBag.chk_Isdeleted = "0";
                    return View();
                }

                //if (UOM == 692002)
                //{
                //    CM.MAX_HEIGHT_FT = (float) CM.MAX_HEIGHT_MTRS;
                //    CM.MAX_WIDTH_FT = (float)CM.MAX_WIDTH_MTRS;
                //    CM.MAX_LEN_FT = (float)CM.MAX_LEN_MTRS;
                //}

                TempData["OrganisationName"] = CM.OrganisationName;
                TempData["OWNER_ORG_ID"] = CM.OwnerOrganisationId;
                TempData["ConstraintCode"] = CM.ConstraintCode;
                TempData["ConstraintName"] = CM.ConstraintName;

                TempData.Keep("OrganisationName");
                TempData.Keep("OWNER_ORG_ID");
                TempData.Keep("ConstraintCode");
                TempData.Keep("ConstraintName");
                ViewBag.orgId = CM.OwnerOrganisationId;
                if (userTypeID == 696001)
                    return View("HaulierConstraintDetails", CM);
                else
                    return View(CM);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraint/ViewConstraint, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region GetConstraintListForOrg
        [HttpPost]
        public JsonResult GetConstraintListForOrg(int otherOrg, int left, int right, int bottom, int top)
        {
            #region Session Check
            int organisationId;
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return Json(new { result = "session out" });
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
            #endregion
            List<RouteConstraints> constraintList = constraintService.GetConstraintListForOrg(organisationId, SessionInfo.UserSchema, otherOrg, left, right, bottom, top);
            var jsonResult = Json(new { result = constraintList });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        #endregion

        public ActionResult ReviewCautionsList(int? page, int? pageSize, long constraintId, bool flagEditMode = true)
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

            #region Check Page Request

            //Put appropriate page access checking code here
            //if (!PageAccess.GetPageAccess("60001"))
            //{
            //    return RedirectToAction("Error", "Home");
            //}

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

            ConstraintModel constraintObj = constraintService.GetConstraintDetails((int)constraintId);
            constraintObj.HdnMaxGrossWeightKgs = (decimal)constraintObj.GrossWeight;
            constraintObj.HdnMaxAxleWeightKgs = (decimal)constraintObj.AxleWeight;
            ViewBag.pageSize = 3;
            ViewBag.page = pageNumber;

            #endregion
            TempData.Keep("OrganisationName");
            TempData.Keep("OWNER_ORG_ID");
            ViewBag.ConstraintID = constraintId;
            ViewBag.flageditmode = flagEditMode;
            ViewBag.orgId = constraintObj.OwnerOrganisationId;
            TempData["ConstraintCode"] = constraintObj.ConstraintCode;
            TempData.Keep("ConstraintCode");
            List<ConstraintModel> constraintList = constraintService.GetCautionList(pageNumber, 3, constraintId);
            XmlDocument Doc = new XmlDocument();
            foreach (ConstraintModel constrain in constraintList)
            {
                Doc.LoadXml(constrain.SpecificActionXML);
                XmlNodeList parentNode = Doc.GetElementsByTagName("SpecificAction");
                if (parentNode.Count > 0)
                {
                    constrain.SpecificAction = parentNode[0].InnerText;
                }
                else
                {
                    XmlNodeList parentNodebold = Doc.GetElementsByTagName("caution:SpecificAction");
                    constrain.SpecificAction = parentNodebold[0].InnerText;
                }
            }
            if (constraintList.Count > 0)
            {
                ViewBag.TotalCount = Convert.ToInt32(constraintList[0].TotalRecordCount);                
            }
            else
            {
                ViewBag.TotalCount = 0;
            }
            var constraintPageList = new StaticPagedList<ConstraintModel>(constraintList, pageNumber, 3, ViewBag.TotalCount);
            return PartialView("ReviewCautionsList", constraintPageList);
        }
        public ActionResult ReviewContactsList(int? page, int? pageSize, long constraintId, bool flagEditMode = true)
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

            #region Check Page Request

            //Put appropriate page access checking code here
            //if (!PageAccess.GetPageAccess("60001"))
            //{
            //    return RedirectToAction("Error", "Home");
            //}

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

            ConstraintModel CM = constraintService.GetConstraintDetails((int)constraintId);
            CM.HdnMaxGrossWeightKgs = (decimal)CM.GrossWeight;
            CM.HdnMaxAxleWeightKgs = (decimal)CM.AxleWeight;
            ViewBag.pageSize = 3;
            ViewBag.page = pageNumber;

            #endregion

            ViewBag.ConstraintID = constraintId;
            ViewBag.flageditmode = flagEditMode;
            ViewBag.orgId = CM.OwnerOrganisationId;
            List<ConstraintContactModel> contactsList = constraintService.GetConstraintContactList(pageNumber,3, constraintId);

            if (contactsList.Count > 0)
            {
                ViewBag.TotalCount = Convert.ToInt32(contactsList[0].TotalRecordCount);
            }
            else
            {
                ViewBag.TotalCount = 0;
            }
            var contactPageList = new StaticPagedList<ConstraintContactModel>(contactsList, pageNumber, 3, ViewBag.TotalCount);
            return PartialView("ReviewContactsList", contactPageList);
        }

        [HttpPost]
        public JsonResult DeleteContact(long constraintId, short contactNo)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json("expire", JsonRequestBehavior.AllowGet);
            }
            #endregion

            int affectedRows = constraintService.DeleteContact(contactNo, constraintId);
            bool deleteFlag = (affectedRows > 0);
            return Json(deleteFlag, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConstraintList(int? page, int? pageSize, string Mode, string orgid, string arrId, string EditOrgId, string flag = null, int? sortOrder = null, int? sortType = null)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "Constraints/GetConstraintList actionResult method started successfully");

                #region Session check
                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                SessionInfo = (UserInfo)Session["UserInfo"];
                #endregion
                sortOrder = sortOrder != null ? (int)sortOrder : 0; //name
                int presetFilter = sortType != null ? (int)sortType : 0; // asc
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortType = presetFilter;
                if (pageSize == null)


                    #region Paging Part
                    if (TempData["PageNum"] != null)
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
                SearchConstraintsFilter objSearch = new SearchConstraintsFilter();
                objSearch.sortOrder = (int)sortOrder;
                objSearch.presetFilter = presetFilter;
                if (Session["g_ConstSearch"] != null)
                {
                    objSearch = (SearchConstraintsFilter)Session["g_ConstSearch"];
                    
                }
                #endregion
                objSearch.sortOrder = ViewBag.SortOrder;
                objSearch.presetFilter= ViewBag.SortType;
                ViewBag.code = objSearch.ConstraintCode;
                ViewBag.name = objSearch.ConstraintName;
                ViewBag.type = objSearch.ConstraintType;
                ViewBag.ownercontact = objSearch.IsOwnerContact;
                ViewBag.isvalid = objSearch.IsValid;
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

                List<ConstraintModel> summaryObjList = constraintService.GetConstraintList(Convert.ToInt32(SessionInfo.OrganisationId), pageNumber, (int)pageSize, objSearch);

                if (summaryObjList.Count > 0)
                {
                    ViewBag.TotalCount = Convert.ToInt32(summaryObjList[0].TotalRecordCount);
                }
                else
                {
                    ViewBag.TotalCount = 0;
                }
                var structureSummaryAsIPagedList = new StaticPagedList<ConstraintModel>(summaryObjList, pageNumber, (int)pageSize, ViewBag.TotalCount);
                TempData.Keep("arrId");
                TempData.Keep("EditOrgId");
                TempData.Keep("orgid");
                TempData.Keep("delegationList");
                ViewBag.filterObject = objSearch;
                return View("ConstraintList", structureSummaryAsIPagedList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Constraints/GetConstraintList, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
    }
}