using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using STP.Common.Constants;
using STP.Common.General;
using STP.Common.Logger;
using STP.Common.StringExtractor;
using STP.Domain.Custom;
using STP.Domain.RouteAssessment;
using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using STP.Domain.RouteAssessment.XmlAffectedParties;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Structures;
using STP.Domain.Structures.StructureJSON;
using STP.ServiceAccess.RouteAssessment;
using STP.ServiceAccess.Routes;
using static STP.Domain.Routes.RouteModel;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.Web.WorkflowProvider;
using System.Configuration;
using System.Web.UI.WebControls;
using STP.ServiceAccess.Applications;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.NonESDAL;

namespace STP.Web.Controllers
{
    public class RouteAssessmentController : Controller
    {
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly IRoutesService routeService;
        private readonly INotificationService notificationService;
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        private readonly ISORTApplicationService sortApplicationService;
        public RouteAssessmentController(IRouteAssessmentService routeAssessmentService, IRoutesService routeService, INotificationService notificationService, IApplicationNotificationWorkflowService applicationNotificationWorkflowService, ISORTApplicationService sortApplicationService)
        {
            this.routeAssessmentService = routeAssessmentService;
            this.routeService = routeService;
            this.notificationService = notificationService;
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
            this.sortApplicationService = sortApplicationService;
        }

        #region Code to check the spamming of structure assessment
        /// <summary>
        /// 
        [HttpPost]
        public JsonResult GetStructureAssessmentCount(List<StructuresToAssess> StuctureList, string MovementRefNo = "", int RouteID = 0)
        {
            int success = 0;
            try
            {
                EsdalStructureJson structureJSON = routeAssessmentService.GetStructureAssessmentCount(MovementRefNo, RouteID);

                if (structureJSON.EsdalStructure.Count == 0)
                {
                    return Json(new { success = 0 }); // no records
                }
                else
                {
                    foreach (var existing in structureJSON.EsdalStructure)
                    {
                        foreach (var item in StuctureList)
                        {
                            if (existing.ESRN == item.ESRN && existing.Status == "In Progress")
                            {
                                return Json(new { success = 1 });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetStructureAssessmentCount, Exception: {0}", ex));
                throw ex;
            }
            return Json(new { success = success });
        }
        #endregion

        #region Code for structure api interface
        /// <summary>
        /// 
        [HttpPost]
        public JsonResult PerformStructureAssessment(List<StructuresToAssess> StuctureList, int NotificationID = 0, string MovementRefNo = "", int AnalysisID = 0, int RouteID = 0)
        {
            int sequenceNo;
            var sessionValues = (UserInfo)Session["UserInfo"];
            try
            {
                sequenceNo = routeAssessmentService.PerformAssessment(StuctureList, NotificationID, MovementRefNo, AnalysisID, RouteID);
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Sequence number {0} added by the user {1}", sequenceNo, sessionValues.UserName));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/PerformStructureAssessment, Exception: {0}", ex));
                throw ex;
            }
            return Json(new { sequenceNo = sequenceNo });
        }
        #endregion

        #region Code to check the structure assessment result
        /// <summary>
        /// 
        [HttpPost]
        public JsonResult GetStructureAssessmentResult(string MovementRefNo = "", int RouteID = 0)
        {
            EsdalStructureJson structureJSON = new EsdalStructureJson();
            try
            {
                structureJSON = routeAssessmentService.GetStructureAssessmentCount(MovementRefNo, RouteID);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetStructureAssessmentResult, Exception: {0}", ex));
                throw ex;
            }
            return Json(new { structureJSON });
        }
        #endregion

        #region GetAssessmentResult
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public string GetAssessmentResult(int analysisID, string userSchema = UserSchema.Portal)
        {
            string result = "";
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/GetAssessmentResult actionResult method started successfully, Input Parameters for fetching Assessment result analysisID: {0},userSchema: {1}", analysisID, userSchema));

                string errorCode = "";

                UserInfo SessionInfo = null;
                SessionInfo = (UserInfo)Session["UserInfo"];

                int orgId = 0;

                int userTypeId = SessionInfo.UserTypeId;

                orgId = (int)SessionInfo.OrganisationId;

                result = routeAssessmentService.GetAssessmentResult(analysisID, SessionInfo.UserSchema);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAffectedStructures, Exception: {0}", ex));
                return result;
            }
            return result;
        }
        #endregion

        #region
        /// <summary>
        /// Action to fetch Affected Structure's or Affected Constraints for given Route Part Object
        /// </summary>
        /// <param name="linkIdList"></param>
        /// <param name="CheckFor">0 : Affected Structures , 1 : Constraints and Cautions</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetInstantAnalysis(int routePartId, RoutePart routePart, int CheckFor = 0)
        {

            if (Session["UserInfo"] == null)
            {
                return Json("expire", JsonRequestBehavior.AllowGet);
            }
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];

            List<StructureInfo> structureInfoList = null;
            List<RouteConstraints> routeConstraints = null;

            var analysisInfo = "";

            //code part to check for route-appraisal
            switch (CheckFor)
            {
                case 0:

                    structureInfoList = routeAssessmentService.GetInstantStructureAnalysis(routePartId, routePart, SessionInfo.UserSchema);
                    //return Json(new { result = structureInfoList });
                    var structResult = Json(new { result = structureInfoList }, JsonRequestBehavior.AllowGet);
                    structResult.MaxJsonLength = int.MaxValue;
                    return structResult;
                case 1:
                    routeConstraints = routeAssessmentService.GetInstantConstraintAnalysis(routePartId, routePart, SessionInfo.UserSchema);
                    //return Json(new { result = routeConstraints });
                    var constResult = Json(new { result = routeConstraints }, JsonRequestBehavior.AllowGet);
                    constResult.MaxJsonLength = int.MaxValue;
                    return constResult;
                default:
                    break;
            }

            return Json(new { result = analysisInfo });
        }
        #endregion
        #region GetAffectedConstraintInfoList
        /// <summary>
        /// action to fetch all affected constraints of a route
        /// </summary>
        /// <param name="RouteID"></param>
        /// <param name="routeType">1 : Route part saved route's 0 : for library related route's</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAffectedConstraintInfoList(int routeId, int routeType = 1, bool BSortFlag = false,int analysisId=0)
        {
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];

            List<RouteConstraints> routeConstraints = null;
            if(BSortFlag == true)
            {
                routeConstraints = routeAssessmentService.FetchConstraintList(routeId, routeType, UserSchema.Sort);
            }
            else
            {
                routeConstraints = routeAssessmentService.FetchConstraintList(routeId, routeType, SessionInfo.UserSchema);
            }

            CheckConstraintSuitabilityWithCaution(routeId, analysisId, SessionInfo, routeConstraints);

            var result = Json(new { result = routeConstraints }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        private void CheckConstraintSuitabilityWithCaution(int routeId, int analysisId, UserInfo SessionInfo, List<RouteConstraints> routeConstraints)
        {
            if (routeId > 0 && analysisId > 0)
            {
                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 4, SessionInfo.UserSchema);
                string cautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.Cautions));
                STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions>.XmlDeserialize(cautionsxml);
                newCautions = routeAssessmentService.GetUnsuitableCautions(newCautions, false);
                if (newCautions != null && routeConstraints != null)
                {
                    var newCautionsFiltered = newCautions.AnalysedCautionsPart.Where(x => x.Id == routeId);
                    if (newCautionsFiltered != null && newCautionsFiltered.Any())
                    {
                        List<StructureInfo> unsuitStr = new List<StructureInfo>();
                        foreach (var item in routeConstraints)
                        {
                            var ECRN = item.ConstraintCode;
                            var isConstraintSuitable = item.ConstraintSuitability;
                            if (newCautions != null && newCautions.AnalysedCautionsPart != null && newCautions.AnalysedCautionsPart.Any())
                            {
                                foreach (var itemCaution in newCautionsFiltered)
                                {
                                    if (itemCaution.Caution != null && itemCaution.Caution.Any())
                                    {
                                        var analysedCautionStructures = itemCaution.Caution.Where(x => x.CautionedEntity1 != null && x.CautionedEntity1.AnalysedCautionConstraintStructure != null
                                        && x.CautionedEntity1.AnalysedCautionConstraintStructure.ECRN == ECRN).ToList();
                                        if (analysedCautionStructures != null)
                                        {
                                            //if caution is unsuitable, set constraint as unsuitable
                                            var isUnsuitableCautionExist = analysedCautionStructures.Where(x => x.Vehicle != null &&
                                                            x.Vehicle.Any(y => y.StartsWith("Unsuitable") || y.ToLower() == "unsuitable" )).Any();
                                            if (isUnsuitableCautionExist)
                                                item.ConstraintSuitability = "Unsuitable";
                                            var isNotSpecifiedCautionExist = analysedCautionStructures.Where(x => x.Vehicle != null &&
                                                            x.Vehicle.Any(y => y.StartsWith("Not Specified") || y.ToLower() == "not specified")).Any();
                                            if (isNotSpecifiedCautionExist && isConstraintSuitable.ToLower()!= "unsuitable")
                                                item.ConstraintSuitability = "Not Specified";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region GetAffectedStructureInfoList
        /// <summary>
        /// Action to fetch Affected Structure with Route appraisal
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="routeType">1 : Route part related tables. 0: Library-route related tables</param>
        /// <param name="checkAppAppraisal">true for checking the route appraisal</param>
        /// <param name="sortFlag">
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAffectedStructureInfoList(int routeId, int routeType = 0, bool checkAppAppraisal = false, bool sortFlag = false, int analysisId=0)
        {
            if (Session["UserInfo"] == null)
            {
                return Json("expire", JsonRequestBehavior.AllowGet);
            }
            UserInfo SessionInfo = null;

            SessionInfo = (UserInfo)Session["UserInfo"];
            List<StructureInfo> structureTmpList = null;

            List<StructureInfo> structureInfoList = null;
            //
            //code part to check for route-appraisal
            if (checkAppAppraisal == true)
            {
                if (sortFlag == true)
                {
                    string assessmentFlag = "";
                    if (Session["RouteAssessmentFlag"] != null)
                    {
                        assessmentFlag = Session["RouteAssessmentFlag"].ToString();
                    }
                    if (assessmentFlag == "Completed")
                    {
                        structureInfoList = routeAssessmentService.FetchAffectedStructureInfoList(routeId, routeType, UserSchema.Sort);
                    }
                    else
                    {
                        structureInfoList = routeAssessmentService.FetchAffectedStructureListSO(routeId, routeType, UserSchema.Sort);

                    }
                }
                else
                {
                    string appFlag = "";
                    if (Session["AppFlag"] != null)
                    {
                        appFlag = Session["AppFlag"].ToString();
                    }

                    string assessmentFlag = "";
                    if (Session["RouteAssessmentFlag"] != null)
                    {
                        assessmentFlag = Session["RouteAssessmentFlag"].ToString();
                    }
                    if (appFlag == "SOApp" || appFlag == "VR1App")
                    {
                        if (assessmentFlag == "Completed")
                        {
                            structureInfoList = routeAssessmentService.FetchAffectedStructureInfoList(routeId, routeType, SessionInfo.UserSchema);
                        }
                        else
                        {
                            structureInfoList = routeAssessmentService.FetchAffectedStructureListSO(routeId, routeType, SessionInfo.UserSchema);
                        }
                    }
                    else
                    {
                        if (assessmentFlag == "Completed")
                        {
                            structureInfoList = routeAssessmentService.FetchAffectedStructureInfoList(routeId, routeType, SessionInfo.UserSchema);
                        }
                        else
                        {
                            structureInfoList = routeAssessmentService.FetchAffectedStructureListSO(routeId, routeType, SessionInfo.UserSchema);
                        }
                    }
                }

            }
            else//condition to fetch library route related structure's 
            {
                structureInfoList = routeAssessmentService.AffectedStructureInfoList(routeId);

                try
                {
                    // It is passes 1 for library route for fetching structures in start and end point this is not be modified unless SP's are changed
                    // Route variant variable is ignored and is passed as 0 in this case.
                    structureTmpList = routeAssessmentService.FetchAffectedStructureAtPoints(routeId, 0, SessionInfo.UserSchema, 1);
                    //checking whether the structures are obtained in temporary list object for library route point's
                    //if it is present its added to the already obtained list object and a combined list is obtained
                    if (structureTmpList.Count != 0)
                    {
                        structureInfoList.AddRange(structureTmpList);
                    }
                }
                catch
                {
                    //Exception is ignored
                }

            }

            CheckStructureSuitabilityWithCaution(routeId, analysisId, SessionInfo, structureInfoList);

            //return Json(new { result = structureInfoList });
            var result = Json(new { result = structureInfoList }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        private void CheckStructureSuitabilityWithCaution(int routeId, int analysisId, UserInfo SessionInfo, List<StructureInfo> structureInfoList)
        {
            if (routeId>0 && analysisId > 0)
            {
                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 4, SessionInfo.UserSchema);
                string cautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.Cautions));
                STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions>.XmlDeserialize(cautionsxml);
                newCautions = routeAssessmentService.GetUnsuitableCautions(newCautions, false);
                if (newCautions != null && structureInfoList != null)
                {
                    var newCautionsFiltered = newCautions.AnalysedCautionsPart.Where(x => x.Id == routeId);
                    if (newCautionsFiltered != null && newCautionsFiltered.Any())
                    {
                        List<StructureInfo> unsuitStr = new List<StructureInfo>();
                        foreach (var item in structureInfoList)
                        {
                            var ESRN = item.StructureCode;
                            if (newCautions != null && newCautions.AnalysedCautionsPart != null && newCautions.AnalysedCautionsPart.Any())
                            {
                                foreach (var itemCaution in newCautionsFiltered)
                                {
                                    if (itemCaution.Caution != null && itemCaution.Caution.Any())
                                    {
                                        var analysedCautionStructures = itemCaution.Caution.Where(x => x.CautionedEntity1 != null && x.CautionedEntity1.AnalysedCautionStructureStructure != null
                                        && x.CautionedEntity1.AnalysedCautionStructureStructure.ESRN == ESRN).ToList();
                                        if (analysedCautionStructures != null)
                                        {
                                            //if caution is unsuitable, set structure as unsuitable
                                            var isUnsuitableCautionExist = analysedCautionStructures.Where(x => x.Vehicle != null &&
                                                            x.Vehicle.Any(y => y.StartsWith("Unsuitable")  || y.ToLower() == "unsuitable" )).Any();
                                            if (isUnsuitableCautionExist)
                                            {
                                                if (item.Suitability == null)
                                                {
                                                    item.Suitability = new List<string>();
                                                    item.Suitability.Add("Unsuitable");
                                                }
                                                else
                                                {
                                                    item.Suitability[0] = "Unsuitable";
                                                }
                                            }
                                            var isNotspecifiedCautionExist= analysedCautionStructures.Where(x => x.Vehicle != null &&
                                                             x.Vehicle.Any(y => y.StartsWith("Not Specified") || y.ToLower() == "not specified")).Any();
                                            if (isNotspecifiedCautionExist)
                                            {
                                                if (item.Suitability != null)
                                                {
                                                    if (item.Suitability[0] != "Suitable" && item.Suitability[0] != "Unsuitable")
                                                    {
                                                        item.Suitability[0] = "Not Structure Specified";
                                                    }
                                                    if (item.Suitability[0]=="Suitable")
                                                    {
                                                        item.Suitability[0] = "Not Specified";
                                                    }
                                                }
                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region ListAffectedStructures
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="anal_type"></param>
        /// <param name="ContentRefNo"></param>
        /// <param name="revisionId"></param>
        /// <param name="SORTflag"></param>
        /// <param name="isAuthMove">flag variable to identify the call is coming from authorize movement General</param>
        /// <returns></returns>
        public ActionResult ListAffectedStructures(long analysisID, int anal_type = 3, string ContentRefNo = "", int revisionId = 0, bool SORTflag = false, bool isAuthMove = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int FilterByOrgID = 0, bool ShowAllStruct = false, int AgreedSONotif = 0, bool UnSuitableShowAllStructures = false, bool IsOverbridge = false, bool IsUnderbridge = false, bool IsLevelCrossing = false, bool UnSuitableShowAllCautions = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListAffectedStructures actionResult method started successfully, Input Parameters for fetching affected strucure analysisID: {0},anal_type: {1},ContentRefNo: {2},revisionId: {3},SORTflag: {4},isAuthMove: {5},IsCandidate: {6},versionId: {7},IsVR1: {8},FilterByOrgID: {9},ShowAllStruct: {10},AgreedSONotif: {11}", analysisID, anal_type, ContentRefNo, revisionId, SORTflag, isAuthMove, IsCandidate, versionId, IsVR1, FilterByOrgID, ShowAllStruct, AgreedSONotif));
                ViewBag.AffectedLength = 0;
                string errorCode = "";

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

                int userTypeId = SessionInfo.UserTypeId;

                orgId = (int)SessionInfo.OrganisationId;

                ViewBag.IsChecked = ShowAllStruct;
                ViewBag.analysisId = analysisID;
                ViewBag.UnSuitableShowAllStructures = UnSuitableShowAllStructures;
                ViewBag.UnSuitableShowAllCautions = UnSuitableShowAllCautions;
                ViewBag.IsOverbridge = IsOverbridge;
                ViewBag.IsUnderbridge = IsUnderbridge;
                ViewBag.IsLevelCrossing = IsLevelCrossing;
                ViewBag.FilterByOrgID = FilterByOrgID;
                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

                #region Non candidate route
                if (IsCandidate == false)
                {
                    try
                    {
                        //Affected structures is generated each time 
                        if (ContentRefNo != "" && revisionId == 0)
                        {
                            if (AgreedSONotif == 0)
                            {
                                long verStatus = routeAssessmentService.GetMovementStatus(0, ContentRefNo, SessionInfo.UserSchema);
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get movement status successfully completed verStatus:{0}", verStatus));
                                if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                                {
                                    //do nothing
                                }
                                else
                                {
                                    int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                                }
                            }
                            else
                            {
                                //do nothing.
                            }
                        }
                        //Affected structures is generated each time 
                        else if (ContentRefNo == "" && revisionId != 0) //generating constraints and affected structures for given content reference number and updating in given analysis Id
                        {

                            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                            if (objRouteAssessmentModel.AffectedStructure != null)
                            {
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get drive instructions info successfully completed"));
                            }
                            if (objRouteAssessmentModel.AffectedStructure == null) // the condition is added for SO or VR-1 that already are agreed.
                            {
                                if (SORTflag == false && isAuthMove == false) //condition added to prevent the call from authorize movement updating affected structure's.
                                {
                                    int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                                }
                            }
                        }
                        else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                        }
                        else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == false) // SO Agreed / proposed route Assessment
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get movement status successfully successfully completed verStatus:{0}", verStatus));
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                                if (objRouteAssessmentModel.AffectedStructure != null)
                                {
                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get drive instructions info successfully completed"));
                                }
                                if (objRouteAssessmentModel.AffectedStructure == null) // the condition is added for SO applications that have not been generated.
                                {
                                    int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                                }
                            }
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCode = "ERR#AFS#001"; //Error occured while generating for non candidate route
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Affected structures not generated (Condition's did not match) Candidate route case:{0}", ex));
                    }

                }
                #endregion
                #region Candidate route
                else //if (IsCandidate == true)
                {
                    try
                    {
                        #region Route Assessment Input
                        RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                        inputsForAssessment.AnalysisId = analysisID;

                        inputsForAssessment.ContentReferenceNo = ContentRefNo;

                        inputsForAssessment.IsCanditateRoute = IsCandidate;

                        inputsForAssessment.OrganisationId = orgId;

                        inputsForAssessment.RevisionId = revisionId;
                        #endregion
                        int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update driving instructions
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                    }
                    catch (Exception ex)
                    {
                        errorCode = "ERR#AFS#002"; //Error occured while generating AFS for candidate route
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Affected structures not generated (Condition's did not match) Candidate route case:{0}", ex));
                    }
                }
                #endregion

                //This check is needed because in case of authorize movement showing sort distributed data then route assessment need to be taken from SORT schema
                string schema = SORTflag && isAuthMove ? UserSchema.Sort : SessionInfo.UserSchema;
                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, schema);
                RouteAssessmentModel objRouteAssessmentModelCautions = routeAssessmentService.GetDriveInstructionsinfo(analysisID, 4, SessionInfo.UserSchema);
                if (objRouteAssessmentModel.AffectedStructure != null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get drive instructions info method completed successfully"));
                }

                //if (UnSuitableShowAllStructures == false)
                //    objRouteAssessmentModel.AffectedStructure = GetUnsuitableStructure(objRouteAssessmentModel.AffectedStructure);

                //if (UnSuitableShowAllCautions == false)
                //    objRouteAssessmentModelCautions.Cautions = GetUnsuitableCautions(objRouteAssessmentModelCautions.Cautions);


                ViewBag.IsAuthMove = isAuthMove;
                #region fetching affected structures list from xml to store in structure list object

                //get affectedstructures xml from byte[]
                string affectedStructuresxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure));
                AnalysedStructures newAnalysedStructures = XmlAffectedStructuresDeserializer.XmlAffectedStructuresDeserialize(affectedStructuresxml);

                string cautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModelCautions.Cautions));
                STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions>.XmlDeserialize(cautionsxml);
                if (newAnalysedStructures != null && newAnalysedStructures.AnalysedStructuresPart != null)
                {
                    foreach (var item in newAnalysedStructures.AnalysedStructuresPart)
                    {
                        List<Structure> unsuitStr = new List<Structure>();
                        if (item != null && item.Structure != null && item.Structure.Any())
                        {
                            foreach (var structure in item.Structure)
                            {
                                var ESRN = structure.ESRN;

                                if (newCautions!=null && newCautions.AnalysedCautionsPart != null && newCautions.AnalysedCautionsPart.Any())
                                {
                                    foreach (var itemCaution in newCautions.AnalysedCautionsPart)
                                    {
                                        if (itemCaution.Caution != null && itemCaution.Caution.Any())
                                        {
                                            var analysedCautionStructures = itemCaution.Caution.Where(x => x.CautionedEntity1 != null && x.CautionedEntity1.AnalysedCautionStructureStructure != null
                                            && x.CautionedEntity1.AnalysedCautionStructureStructure.ESRN == ESRN).ToList();
                                            if (analysedCautionStructures != null)
                                            {
                                                structure.AnalysedCautions = analysedCautionStructures;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                List<StructuresToAssess> structAssessList = new List<StructuresToAssess>();
                int routePartNo = 1;
                if (newAnalysedStructures != null && newAnalysedStructures.AnalysedStructuresPart != null)
                {
                    foreach (AnalysedStructuresPart analstrucpart in newAnalysedStructures.AnalysedStructuresPart)
                    {
                        if (analstrucpart != null && analstrucpart.Structure != null)
                        {
                            foreach (Structure struc in analstrucpart.Structure)
                            {
                                if (struc.TraversalType == "underbridge")
                                {
                                    foreach (var org in struc.StructureResponsibility.StructureResponsibleParty)
                                    {
                                        if (org.OrganisationId == orgId)
                                        {
                                            StructuresToAssess structAssess = new StructuresToAssess();
                                            structAssess.ESRN = struc.ESRN;
                                            structAssess.SectionId = (int)struc.StructureSectionId;
                                            structAssess.RouteId = analstrucpart.Id;
                                            structAssess.RoutePartNo = routePartNo;

                                            structAssessList.Add(structAssess);
                                        }
                                    }
                                }
                            }
                        }
                        routePartNo++;
                    }
                }
                ViewBag.StructureAssessList = structAssessList;
                #endregion

                return PartialView("ListAffectedStructures", newAnalysedStructures);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAffectedStructures, Exception: {0}", ex));
                //ViewBag.AffectedStruct = "ERROR 
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListAffectedStructures");
        }
        #endregion

        #region DifferentAffectedStructuresList
        public ActionResult ListOverBridgeStructures(long analysisID, int anal_type = 3, bool IsCandidate = false, int OrganisationID = 0)
        {
            // long analysisID = 580;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListAffectedStructures actionResult method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                RouteAssessmentModel objRouteAssessmentModel;
                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                string result = string.Empty;
                string errormsg = "No data found";
                string path = null;
                if (OrganisationID != 0)
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\MyOverBridgeStruct.xslt");
                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure, path, OrganisationID, 0, out errormsg);
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\OverBridgeStruct.xslt");
                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure, path, out errormsg);
                }
                ViewBag.AffectedStruct = result;
                ViewBag.AffectedLength = result.Length;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAffectedStructures, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListAffectedStructures");
        }


        public ActionResult ListUnderBridgeStructures(long analysisID, int anal_type = 3, bool IsCandidate = false, int OrganisationID = 0)
        {
            //long analysisID = 580;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListAffectedStructures actionResult method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                RouteAssessmentModel objRouteAssessmentModel;
                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                string result = string.Empty;
                string errormsg = "No data found";
                string path = null;
                if (OrganisationID != 0)
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\MyUnderBridgeStruct.xslt");
                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure, path, OrganisationID, 0, out errormsg);
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\UnderBridgeStruct.xslt");
                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure, path, out errormsg);
                }
                ViewBag.AffectedStruct = result;
                ViewBag.AffectedLength = result.Length;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAffectedStructures, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListAffectedStructures");
        }

        public ActionResult ListLevelCrossingStructures(long analysisID, int anal_type = 3, bool IsCandidate = false, int OrganisationID = 0)
        {
            // long analysisID = 580;
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListAffectedStructures actionResult method started successfully"));

                UserInfo SessionInfo = null;
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }

                RouteAssessmentModel objRouteAssessmentModel;
                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                string result = string.Empty;
                string errormsg = "No data found";
                string path = null;
                if (OrganisationID != 0)
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\MyLevelCrossingStruct.xslt");
                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure, path, OrganisationID, 0, out errormsg);
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\LevelCrossingStruct.xslt");
                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure, path, out errormsg);
                }
                ViewBag.AffectedStruct = result;
                ViewBag.AffectedLength = result.Length;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAffectedStructures, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListAffectedStructures");
        }

        #endregion

        #region ListAnnotations
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="anal_type"></param>
        /// <param name="ContentRefNo"></param>
        /// <returns></returns>
        public ActionResult ListAnnotations(long analysisID, int anal_type = 6, string ContentRefNo = "", int revisionId = 0, bool SORTflag = false, bool isAuthMove = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int AgreedSONotif = 0)
        {

            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListAnnotations actionResult method started successfully"));

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

                int userTypeId = SessionInfo.UserTypeId;

                orgId = (int)SessionInfo.organisationId;

                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
                if (IsCandidate == false)
                {
                    //Affected structures is generated each time 
                    if (ContentRefNo != "" && revisionId == 0)
                    {
                        if (AgreedSONotif == 0)
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(0, ContentRefNo, SessionInfo.UserSchema);
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                        else
                        {
                            //do nothing.
                        }
                    }
                    //Affected structures is generated each time 
                    else if (ContentRefNo == "" && revisionId != 0) //generating annotations for given revision id
                    {

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                        if (objRouteAssessmentModel.Annotation == null) // the condition is added for SO or VR-1 that already are agreed.
                        {
                            if (SORTflag == false && isAuthMove == false) //condition added to prevent the call from authorize movement updating affected structure's.
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                    {
                        int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                    }
                    else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == false) // SO Agreed / proposed route Assessment
                    {
                        long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                        if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                        {
                            //do nothing
                        }
                        else
                        {
                            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                            //here you are updating the route  assessment and saving in analysed routes
                            if (objRouteAssessmentModel.Annotation == null) //generating annotations for movement version if its empty
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else
                    {
                        //do nothing
                    }
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                }
                else //if (IsCandidate == true)
                {
                    #region Route Assessment Input
                    RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                    inputsForAssessment.AnalysisId = analysisID;

                    inputsForAssessment.ContentReferenceNo = ContentRefNo;

                    inputsForAssessment.IsCanditateRoute = IsCandidate;

                    inputsForAssessment.OrganisationId = orgId;

                    inputsForAssessment.RevisionId = revisionId;
                    #endregion
                    try
                    {
                        //functionality not yet implemented
                        int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update driving instructions
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                    }
                    catch
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Annotation not generated (Condition's did not match)"));
                    }

                }

                string result = string.Empty;
                string errormsg = "No data found";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\Annotation.xslt");
                result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.Annotation, path, out errormsg);
                ViewBag.annotation = result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAnnotations, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListAnnotations");
        }
        #endregion

        #region List RouteDescription
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="anal_type"></param>
        /// <param name="ContentRefNo"></param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        public ActionResult ListRouteDescription(long analysisID, int anal_type = 2, string ContentRefNo = "", int revisionId = 0, bool IsCandidate = false, int AgreedSONotif = 0, bool SORTFlag = false)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                string schema = SORTFlag ? UserSchema.Sort : SessionInfo.UserSchema;
                string path = null;

                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, schema);

                string result = string.Empty;
                string errormsg = string.Empty;

                //For RM#4311 Change
                string[] separators = { "Split" };
                string resultPart = string.Empty;
                string FinalResult = string.Empty;
                string[] versionSplit = new string[] { };

                //Metric Route description
                if (SessionInfo.RoutePlanUnits == 692001)
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\RouteDescription.xslt");
                }
                //Imperial route description
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\RouteDescriptionImperial.xslt");
                }

                result = XsltTransformer.Trafo(objRouteAssessmentModel.RouteDescription, path, out errormsg);

                //For RM#4311
                if (result != null && result.Length > 0)
                {
                    versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                }

                StringBuilder sb = new StringBuilder();


                foreach (string part in versionSplit)
                {
                    if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
                    {
                        resultPart = part.Replace("Arrive at destination", "<u class='text-highlight'><b>Alternative end # 2:</b></u> <span class='text-color5'>Arrive at destination</span>");
                        sb.Append(resultPart);
                    }
                    else
                    {
                        resultPart = part;
                        sb.Append(resultPart);
                    }
                }

                if (sb.ToString() != null || sb.ToString() != "")
                {
                    FinalResult = sb.ToString();
                }

                ViewBag.RouteDescr = FinalResult;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListRouteDescription, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
            return PartialView("ListRouteDescription");
        }
        #endregion


        #region AffectedRoads
        /// <summary>
        /// controller to generate affected road's related xml and save the same into analysed route's table. 
        /// Controller also checks to session to decide on the user preference to fetch the corresponding xslt
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="notifId"></param>
        /// <param name="revisionId"></param>
        /// <param name="contentRefNo"></param>
        /// <param name="anal_type"></param>
        /// <param name="isAuthMove"></param>
        /// <param name="IsCandidate"></param>
        /// <param name="versionId"></param>
        /// <param name="IsVR1"></param>
        /// <param name="Organisation_ID"></param>//For showing organisation based Affected Roads
        /// <returns></returns>
        public ActionResult AffectedRoads(long analysisID = 0, int notifId = 0, int revisionId = 0, string contentRefNo = "", int anal_type = 8, bool SORTflag = false, bool isAuthMove = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int FilterByOrgID = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/AffectedRoads actionResult method started successfully"));

                UserInfo SessionInfo = null;

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    SessionInfo = (UserInfo)Session["UserInfo"];
                }
                string schema = SORTflag && isAuthMove ? UserSchema.Sort : SessionInfo.UserSchema;
                int orgId = 0;
                string path = null;
                orgId = (int)SessionInfo.OrganisationId;

                RouteAssessmentModel objRouteAssessmentModel;

                if (IsCandidate == false)
                {
                    //for calls that aren't from authorize movement general page new list will be generated
                    if (isAuthMove == false)
                    {
                        if (contentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                        }
                        else if (contentRefNo == "" && revisionId != 0)
                        {
                            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                            //here you are updating the route  assessment and saving in analysed routes
                            if (objRouteAssessmentModel.AffectedRoads == null)
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(contentRefNo, notifId, revisionId, versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                        else if (contentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                        }
                        else if (contentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == false) // SO / Proposed / Agreed assessment
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                                //here you are updating the route  assessment and saving in analysed routes
                                if (objRouteAssessmentModel.AffectedRoads == null) //generating affected roads if its null for SO applications from movement version
                                {
                                    int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                                }
                            }
                        }
                        else if (contentRefNo != "")
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(contentRefNo, notifId, revisionId, versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                        }
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, schema);
                    }
                    else
                    {
                        //This check is needed because in case of authorize movement showing sort distributed data then route assessment need to be taken from SORT schema
                        //string schema = SORTflag && isAuthMove ? UserSchema.Sort : SessionInfo.UserSchema;
                        //here you are fetching the affected road's
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, schema);

                        FilterByOrgID = (int)SessionInfo.OrganisationId; //#3582 SOA's / Police can only view their affected roads
                    }
                }
                else
                {
                    #region Route Assessment Input
                    RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                    inputsForAssessment.AnalysisId = analysisID;

                    inputsForAssessment.ContentReferenceNo = contentRefNo;

                    inputsForAssessment.IsCanditateRoute = IsCandidate;

                    inputsForAssessment.OrganisationId = orgId;

                    inputsForAssessment.RevisionId = revisionId;
                    #endregion
                    //in case of show details call from affected parties
                    if (FilterByOrgID != 0)
                    {
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema); //fetching affected roads
                        //checking if affected roads aren't generated
                        if (objRouteAssessmentModel.AffectedRoads == null)
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update affected roads
                        }
                    }
                    else
                    {
                        //in case of candidate route affected roads are generated each time
                        int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update affected roads
                    }
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema); //fetching affected roads
                }
                string result = string.Empty;
                string errormsg = "No data found";
                //Code to check route planner unit session value to display driving instruction's in Metric or imperial system
                if (SessionInfo.RoutePlanUnits == 692001 && SessionInfo.UserSchema != UserSchema.Sort)
                {
                    if (FilterByOrgID != 0)
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\MyAffectedRoadsMetric.xslt");
                    }
                    else
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedRoadsMetric.xslt");
                    }
                }
                else
                {
                    if (FilterByOrgID != 0)
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\MyAffectedRoadsImperial.xslt");
                    }
                    else
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedRoadsImperial.xslt");
                    }
                }
                if (FilterByOrgID != 0)
                {
                    result = XsltTransformer.Trafo(objRouteAssessmentModel.AffectedRoads, path, FilterByOrgID, 0, out errormsg);
                }
                else
                {
                    result = XsltTransformer.Trafo(objRouteAssessmentModel.AffectedRoads, path, out errormsg);
                }
                ViewBag.AffectedRoads = result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/AffectedRoads, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

            return PartialView("ListAffectedRoads");
        }
        #endregion

        #region ListCautions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="anal_type"></param>
        /// <param name="ContentRefNo"></param>
        /// <param name="revisionId"></param>
        /// <param name="SORTflag"></param>
        /// <returns></returns>
        public ActionResult ListCautions(long analysisID, int anal_type = 4, string ContentRefNo = "", int revisionId = 0, bool SORTflag = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int AgreedSONotif = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListCautions actionResult method started successfully"));

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

                orgId = (int)SessionInfo.organisationId;

                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

                #region Non Candidate route
                if (IsCandidate == false)
                {
                    //Caution's is generated each time
                    if (ContentRefNo != "" && revisionId == 0)
                    {
                        if (AgreedSONotif == 0)
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(0, ContentRefNo, SessionInfo.UserSchema);
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                //function to update the route caution's
                                int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                        else
                        {
                            //do nothing.
                        }
                    }
                    // Cautions is generated each time && objRouteAssessmentModel.cautions == null
                    else if (ContentRefNo == "" && revisionId != 0) //generating constraints and affected structures for given content reference number and updating in given analysis Id
                    {

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                        if (objRouteAssessmentModel.Cautions == null) // the condition is added for SO or VR-1 that already are agreed.
                        {
                            if (SORTflag == false)
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                    {
                        int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                    }
                    else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == false) // SO Agreed / proposed route Assessment
                    {
                        long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                        if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                        {
                            //do nothing
                        }
                        else
                        {
                            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                            //here you are updating the route  assessment and saving in analysed routes
                            if (objRouteAssessmentModel.Cautions == null) //generating analysis if its empty
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                    }
                }
                #endregion

                #region Candidate Route
                else
                {
                    #region Route Assessment Input
                    RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                    inputsForAssessment.AnalysisId = analysisID;

                    inputsForAssessment.ContentReferenceNo = ContentRefNo;

                    inputsForAssessment.IsCanditateRoute = IsCandidate;

                    inputsForAssessment.OrganisationId = orgId;

                    inputsForAssessment.RevisionId = revisionId;
                    #endregion

                    int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update driving instructions

                }
                #endregion


                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                if (objRouteAssessmentModel.Cautions == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Cautions not generated (Condition's did not match)"));
                }

                string result = string.Empty;
                string errormsg = "No data found";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\CautionsMetric.xslt");
                result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.Cautions, path, out errormsg);
                ViewBag.Cautions = result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex.Message);
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListCautions, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListCautions");
        }
        #endregion

        #region ListConstraints
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="anal_type"></param>
        /// <param name="ContentRefNo"></param>
        /// <param name="revisionId"></param>
        /// <param name="SORTflag"></param>
        /// <returns></returns>
        public ActionResult ListConstraints(long analysisID, int anal_type = 5, string ContentRefNo = "", int revisionId = 0, bool SORTflag = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int AgreedSONotif = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListConstraints actionResult method started successfully"));

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

                orgId = (int)SessionInfo.organisationId;

                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel(); ;

                #region Non Candidate route
                if (IsCandidate == false)
                {

                    //Constraints is generated everytime  && objRouteAssessmentModel.constraints == null
                    if (ContentRefNo != "" && revisionId == 0)
                    {
                        if (AgreedSONotif == 0)
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(0, ContentRefNo, SessionInfo.UserSchema);
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                //function to update the route constraint's
                                int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                                //function to fetch the updated route constraints
                            }
                        }
                        else
                        {
                            //do nothing.
                        }
                    }

                    //Constraints is generated everytime && objRouteAssessmentModel.constraints == null 
                    else if (ContentRefNo == "" && revisionId != 0) //generating constraints and affected structures for given content reference number and updating in given analysis Id
                    {
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                        if (objRouteAssessmentModel.Constraints == null) // the condition is added for SO or VR-1 that already are agreed.
                        {
                            if (SORTflag == false)
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                    }
                    else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                    {
                        int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                    }
                    else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == false) // SO Agreed / proposed route Assessment
                    {
                        long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                        if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                        {
                            //do nothing
                        }
                        else
                        {
                            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                            //here you are updating the route  assessment and saving in analysed routes
                            if (objRouteAssessmentModel.Constraints == null) //generating constraints if its null for SO applications from movement version
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            }
                        }
                    }
                }
                #endregion
                #region Candidate route
                else
                {
                    #region Route Assessment Input
                    RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                    inputsForAssessment.AnalysisId = analysisID;

                    inputsForAssessment.ContentReferenceNo = ContentRefNo;

                    inputsForAssessment.IsCanditateRoute = IsCandidate;

                    inputsForAssessment.OrganisationId = orgId;

                    inputsForAssessment.RevisionId = revisionId;
                    #endregion

                    int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update driving instructions
                }
                #endregion

                objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                if (objRouteAssessmentModel.Constraints == null)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Constraints not generated (Condition's did not match)"));
                }

                string result = string.Empty;
                string errormsg = "No data found";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\Constraints.xslt");
                result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.Constraints, path, out errormsg);
                ViewBag.Constraints = result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListConstraints, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListConstraints");
        }
        #endregion

        #region JsonResult GetConstraintId(string ConstraintCode = null)

        public JsonResult GetConstraintId(string ConstraintCode = null)
        {

            decimal result = routeAssessmentService.GetConstraintId(ConstraintCode);


            var jSonVar = new { check = result, ConstraintCode = ConstraintCode };

            return Json(new { result = jSonVar });

        }
        #endregion

        #region ListAffectedParties
        /// <summary>
        ///  function to display Affected Parties
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="revisionId"></param>
        /// <param name="anal_type"></param>
        /// <param name="isAuthMove"></param>
        /// <param name="IsCandidate"></param>
        /// <returns></returns>
        public ActionResult ListAffectedParties(long analysisID = 0, int revisionId = 0, int anal_type = 7, bool isAuthMove = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int AgreedSONotif = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListAffectedParties actionResult method started successfully, Input Parameters for fetching affected parties analysisID: {0},revisionId: {1},anal_type: {2},isAuthMove: {3},IsCandidate: {4},versionId: {5},IsVR1: {6},AgreedSONotif: {7}", analysisID, anal_type, isAuthMove, IsCandidate, versionId, IsVR1, AgreedSONotif));

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
                orgId = (int)SessionInfo.organisationId;

                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();

                if (IsCandidate == false)
                {
                    if (isAuthMove == true)
                    {
                        //here you are fetching the affected parties
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                        if (objRouteAssessmentModel.AffectedParties != null)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get drive instructions info successfully completed"));
                        }
                    }
                    else
                    {
                        //in case of revision calls that aren't from notification , and route is not a candidate 
                        if (revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                        }
                        else if (revisionId == 0 && versionId != 0 && IsVR1 == false) // SO Agreed / proposed route Assessment
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get movement status successfully completed verStatus:{0}", verStatus));
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                            }
                        }
                        else
                        {
                            int updated = routeAssessmentService.updateRouteAssessment("", 0, revisionId, 0, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                        }

                        //here you are fetching the affected road's
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                        if (objRouteAssessmentModel.AffectedRoads != null)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get drive instructions info successfully completed"));
                        }
                    }
                }
                else
                {
                    #region Route Assessment Input
                    RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                    inputsForAssessment.AnalysisId = analysisID;

                    inputsForAssessment.IsCanditateRoute = IsCandidate;

                    inputsForAssessment.OrganisationId = orgId;

                    inputsForAssessment.RevisionId = revisionId;
                    #endregion

                    //fetching affected parties information 
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                    if (objRouteAssessmentModel.AffectedParties != null)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get drive instructions info successfully completed"));
                    }

                    if (objRouteAssessmentModel.AffectedParties == null)
                    {

                        int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update driving instructions
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                        if (objRouteAssessmentModel.AffectedParties != null)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get drive instructions info successfully completed"));
                        }
                    }
                }
                string result = string.Empty;

                string errormsg = "No data found";

                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedParties.xslt");

                result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties, path, out errormsg);

                ViewBag.AffectedParties = result;
                ViewBag.AnalysisId = analysisID;

                return PartialView("ListAffectedParties");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAffectedParties, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region ListNotifAffectedParties
        public ActionResult ListNotifAffectedParties(long analysisId = 0)
        {
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

            RouteAssessmentModel objRouteAssessmentModel;

            objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisId, 7, SessionInfo.UserSchema);

            string result;

            string errormsg;

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedParties.xslt");
            result = Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties, path, out errormsg);

            ViewBag.AffectedParties = result;

            return PartialView("ListAffectedParties");
        }
        #endregion
        #region ListDrivingInstructions
        /// <summary>
        /// controller to generate driving instruction, show existing driving instructions
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="anal_type"></param>
        /// <param name="ContentRefNo"></param>
        /// <param name="revisionId"></param>
        /// <param name="SORTflag"></param>
        /// <param name="GenerateNewInstr"> </param>
        /// <param name="AuthorizeFlagForDI">check driving instruction condition for authorized movement</param>
        /// <returns></returns>
        public ActionResult ListDrivingInstructions(long analysisID, int anal_type = 1, string ContentRefNo = "", int revisionId = 0, bool SORTflag = false, bool GenerateNewInstr = false, bool AuthorizeFlagForDI = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int AgreedSONotif = 0)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions actionResult method started successfully"));
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


                RouteAssessmentModel objRouteAssessmentModel = new RouteAssessmentModel();
                //This check is needed because in case of authorize movement showing sort distributed data then route assessment need to be taken from SORT schema
                string schema = SORTflag && AuthorizeFlagForDI ? UserSchema.Sort : SessionInfo.UserSchema;
                #region Route Assessment Input 
                RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                inputsForAssessment.AnalysisId = analysisID;

                inputsForAssessment.ContentReferenceNo = ContentRefNo;

                inputsForAssessment.IsCanditateRoute = IsCandidate;

                inputsForAssessment.OrganisationId = orgId;

                inputsForAssessment.VersionId = versionId;

                inputsForAssessment.RevisionId = revisionId;
                #endregion
                int i = 1;
                #region Non Candidate route
                if (IsCandidate == false)
                {
                    int updated = 0;
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions VR1-1, Input Parameters for fetching generate deriving instruction analysisID: {0},anal_type: {1},schema: {2}", analysisID, anal_type, schema));
                    //fetching driving instruction
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, schema);

                    //condition to generate entire route assessment
                    if (objRouteAssessmentModel.DriveInst == null)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions VR1-2, Input Parameters for fetching generate deriving instruction DriveInst: {0}", objRouteAssessmentModel.DriveInst != null));
                        //generate in case of notification related route's
                        if (ContentRefNo != "" && analysisID > 0 && revisionId == 0) //generating constraints and affected structures for given content reference number and updating in given analysis Id
                        {
                            updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, i, schema);//To update all Affected structure,Constriants,...                              
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions VR1-3, Input Parameters for fetching generate deriving instruction updated: {0}", updated));
                        }
                        //condition to generate entire route assessment
                        else if (ContentRefNo == "" && analysisID > 0 && revisionId != 0) //generating constraints and affected structures for given content reference number and updating in given analysis Id
                        {
                            updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, i, schema);//To update all Affected structure,Constriants,...
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions VR1-4, Input Parameters for fetching generate deriving instruction updated: {0}", updated));
                        }

                        //fetching the newly generated driving instruction
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, schema);
                        //Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions VR1-5, Input Parameters for fetching generate deriving instruction analysisID: {0},anal_type: {1},schema: {3}", analysisID, anal_type, schema));
                    }
                    else if (AuthorizeFlagForDI)// to generate driving instruction for authorized movement
                    {
                        // The below line is commented for RM #15561 and #15599 . No need to generate driving instructions while listing driving instruction from SOA/Police portal.
                        //updated = RouteAssessmentProvider.Instance.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, i, schema);//To update all Affected structure,Constriants,...
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions VR1-6, Input Parameters for fetching generate deriving instruction AuthorizeFlagForDI: {0}", AuthorizeFlagForDI));
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, schema);
                    }

                    // conditions to show driving instruction print button

                    if (objRouteAssessmentModel.DriveInst != null)
                    {
                        ViewBag.generatePrintFlag = "1";
                    }
                    else
                    {
                        ViewBag.generatePrintFlag = "0";
                    }
                    string result = string.Empty;
                    string errormsg = "No data found";

                    //Code to check route planner unit session value to display driving instruction's in Metric or imperial system
                    if (SessionInfo.RoutePlanUnits == 692001 && SessionInfo.UserSchema != UserSchema.Sort)
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DrivingInstructionsMetric.xslt");
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/ListDrivingInstructions VR1-7, Input Parameters for fetching generate deriving instruction UserSchema: {0}", SessionInfo.UserSchema));
                    }
                    else
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DrivingInstructionsImperial.xslt");
                    }

                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.DriveInst, path, out errormsg);
                    ViewBag.instruction = result;
                }
                #endregion

                #region condition for candidate route
                else
                {
                    //fetching driving instruction
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, Logger.LogInstance + "fetching driving instruction...");
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                    if (objRouteAssessmentModel.DriveInst == null)
                    {
                        int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, i, SessionInfo.UserSchema);//To update all Affected structure,Constriants,...                                                                          // conditions to show driving instruction print button
                        if (i == 1 && updated == 0)
                        {
                            ViewBag.generatePrintFlag = "0";
                        }
                        else
                        {
                            if (i == 1 && updated != 0)
                            {
                                ViewBag.generatePrintFlag = "1";
                            }
                        }

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Driving Instruction not generated (Condition's did not match)"));
                    }

                    string result = string.Empty;
                    string errormsg = "No data found";

                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DrivingInstructionsImperial.xslt");

                    result = STP.Common.General.XsltTransformer.Trafo(objRouteAssessmentModel.DriveInst, path, out errormsg);
                    ViewBag.instruction = result;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListDrivingInstructions, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");

            }
            return PartialView("ListDrivingInstructions");

        }
        #endregion
        #region JsonResult GenerateAllDataFromService(long analysisID, int anal_type = 1, string ContentRefNo = "", int revisionId = 0, int versionId = 0, bool SORTflag = false, string GenerateNewInstr = null, bool AuthorizeFlagForDI = false, bool IsCandidate = false)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="anal_type"></param>
        /// <param name="ContentRefNo"></param>
        /// <param name="revisionId"></param>
        /// <param name="versionId"></param>
        /// <param name="SORTflag"></param>
        /// <param name="GenerateNewInstr"></param>
        /// <param name="AuthorizeFlagForDI"></param>
        /// <param name="IsCandidate"></param>
        /// <param name="IsVR1"></param>
        /// <param name="AgreedSONotif"></param>
        /// <returns></returns>
        public JsonResult GenerateAllDataFromService(long analysisID, int anal_type = 1, string ContentRefNo = "", int revisionId = 0, int versionId = 0, bool SORTflag = false, bool GenerateNewInstr = false, bool AuthorizeFlagForDI = false, bool IsCandidate = false, bool IsVR1 = false, int AgreedSONotif = 0)//Generic function which will generate all 
        {
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("RouteAssessment/GenerateAllDataFromService actionResult method started successfully, Input Parameters for fetching generate deriving instruction L1 analysisID: {0},anal_type: {1},ContentRefNo: {2},revisionId: {3},versionId: {4},SORTflag: {5},GenerateNewInstr: {6},AuthorizeFlagForDI: {7},IsCandidate: {8},IsVR1: {9},AgreedSONotif: {10}", analysisID, anal_type, ContentRefNo, revisionId, versionId, SORTflag, GenerateNewInstr, AuthorizeFlagForDI, IsCandidate, IsVR1, AgreedSONotif));
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                //return RedirectToAction("Login", "Account");
                return null;
            }
            else
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            int orgId = 0;
            orgId = (int)SessionInfo.organisationId;
            string result = null;

            int updated = 0;
            if (IsCandidate == false)
            {
                if (GenerateNewInstr != false)
                {
                    if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == true) // VR - 1 Assessment
                    {
                        updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                    }
                    else if (ContentRefNo == "" && revisionId == 0 && versionId != 0 && IsVR1 == false) // SO Agreed / proposed route Assessment
                    {
                        long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get movement status successfully completed verStatus:{0}", verStatus));
                        if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                        {
                            //do nothing
                            updated = 5; //generated successfully. despite its not generating anything so that it doesn't show any errorneous message.
                        }
                        else
                        {
                            updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                        }
                    }
                    else if (ContentRefNo != "" && revisionId == 0)
                    {
                        if (AgreedSONotif == 0)
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(0, ContentRefNo, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get movement status successfully completed verStatus:{0}", verStatus));
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);//To update all Affected structure,Constriants,...
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                            }
                        }
                        else
                        {
                            updated = 5;//generated successfully. despite its not generating anything so that it doesn't show any errorneous message.
                        }
                    }
                    else
                    {
                        updated = routeAssessmentService.updateRouteAssessment(ContentRefNo, revisionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);//To update all Affected structure,Constriants,...
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                    }
                }
            }
            else
            {
                //this condition is for sort candidate route
                RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                inputsForAssessment.AnalysisId = analysisID;

                inputsForAssessment.ContentReferenceNo = ContentRefNo;

                inputsForAssessment.IsCanditateRoute = IsCandidate;

                inputsForAssessment.OrganisationId = orgId;

                inputsForAssessment.VersionId = versionId;

                inputsForAssessment.RevisionId = revisionId;

                if (GenerateNewInstr != false)
                {
                    updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update all Affected structure,Constriants,...
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                }
            }

            string errorCodeString = "";
            if (anal_type == 1)
            {

                if (updated != 8)
                {
                    switch (updated)
                    {
                        case 1:
                            errorCodeString = "Failed to generate driving instructions, Error Code: ERR#DIS00" + updated.ToString();
                            break;

                        case 2:
                            errorCodeString = "Failed to generate driving instructions, Error Code: ERR#DIS00" + updated.ToString();
                            break;

                        case 3:
                            errorCodeString = "Failed to generate driving instructions, Error Code: ERR#DIS00" + updated.ToString();
                            break;

                        //TCP Client and Network stream cannot be closed
                        case 4:
                            errorCodeString = "Failed to generate driving instructions, Error Code: ERR#DIS00" + updated.ToString();
                            break;
                        case 5:
                            errorCodeString = "Generated successfully ! Code : ERR#DIS00" + updated.ToString();
                            break;
                        case 6:
                            errorCodeString = "Empty route! Please import or create route and then continue...";
                            break;
                        case 7:
                            errorCodeString = "Failed to generate driving instruction, Error Code:ERR#DIS00" + updated.ToString();
                            break;
                    }
                }

            }
            var jSonVar = new { check = updated, anal_type = anal_type, errorcode = errorCodeString };

            return Json(new { result = jSonVar });
        }
        #endregion

        public ActionResult ViewNoteToHaulier(string Note = "", string structureCode = "")
        {
            ViewBag.Heading = "Note To Haulier for " + structureCode;
            ViewBag.Comment = Note;
            return PartialView("ViewAssessmentResult");
        }
        public ActionResult ViewAssessmentComment(string Comment = "", string structureCode = "")
        {
            ViewBag.Heading = "Assessment Comment for " + structureCode;
            ViewBag.Comment = Comment;
            return PartialView("ViewAssessmentResult");
        }

        #region PrintInstructionReport(long analysisID, int anal_type )
        public ActionResult PrintInstructionReport(long analysisID, int anal_type = 1)
        {
            byte[] optionType = null;
            UserInfo userInfo;
            if (Session["UserInfo"] != null)
            {
                userInfo = (UserInfo)Session["UserInfo"];
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            RouteAssessmentModel routeAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, userInfo.UserSchema);
            string path = "";
            string[] separators = { "Split" };
            string[] versionSplit = new string[] { };
            StringBuilder stringBuilder = new StringBuilder();

            switch (anal_type)
            {
                case 1:
                    optionType = routeAssessmentModel.DriveInst;
                    path = userInfo.RoutePlanUnits == 692001
                        ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DrivingInstructionsMetricPdf.xslt")
                        : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DrivingInstructionsImperialPdf.xslt");
                    break;
                case 2:
                    optionType = routeAssessmentModel.RouteDescription;
                    path = userInfo.RoutePlanUnits == 692001
                        ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\RouteDescription.xslt")
                        : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\RouteDescriptionImperial.xslt");
                    break;
                case 3:
                    optionType = routeAssessmentModel.AffectedStructure;
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedStructuresMetric.xslt");

                    break;
                case 4:
                    optionType = routeAssessmentModel.Cautions;
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\CautionsMetric.xslt");
                    break;
                case 5:
                    optionType = routeAssessmentModel.Constraints;
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\Constraints.xslt");
                    break;
                case 6:
                    optionType = routeAssessmentModel.Annotation;
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\Annotation.xslt");
                    break;
                case 7:
                    optionType = routeAssessmentModel.AffectedParties;
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedPartiesPrint.xslt");
                    break;
            }

            //Code to check route planner unit session value to display driving instruction's in Metric or imperial system
            string result = XsltTransformer.Trafo(optionType, path, out string errorMessage);
            if (anal_type == 2)
            {
                if (result != null && result.Length > 0)
                {
                    versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                }


                foreach (string part in versionSplit)
                {
                    string resultPart;
                    if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
                    {
                        resultPart = part.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
                        stringBuilder.Append(resultPart);
                    }
                    else
                    {
                        resultPart = part;
                        stringBuilder.Append(resultPart);
                    }
                }

                if (stringBuilder.ToString() != null || stringBuilder.ToString() != "")
                {
                    result = stringBuilder.ToString();
                }
            }

            byte[] drivingInstructionDocument = anal_type == 7 ? Document.GenerateXML.GeneratePrintablePDF(result) : Document.GenerateXML.GenerateInstrPDF(result);

            if (drivingInstructionDocument != null)
            {
                System.IO.MemoryStream pdfStream = new System.IO.MemoryStream(drivingInstructionDocument);
                return new FileStreamResult(pdfStream, "application/pdf");
            }
            else
            {
                ViewBag.NoDataFound = "1";
                return View();
            }

        }
        #endregion

        #region Route Assessment functionalities

        #region Generate Route Assessment
        public JsonResult GenerateRouteAssessment(GenerateRouteAssessmentModel generateRouteAssessment)
        {
            Session["RouteAssessmentFlag"] = "";
            UserInfo SessionInfo = (UserInfo)Session["UserInfo"]; 
            int orgId = (int)SessionInfo.OrganisationId;
            bool result = false;
            RouteAssessmentModel routeAssessmentModel = new RouteAssessmentModel();
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"-RouteAssessment/GenerateRouteAssessment. Start Time: {DateTime.Now} , AnalysisId:{generateRouteAssessment.AnalysisId}");
                List<RoutePartDetails> routePartDetails = generateRouteAssessment.IsNenApi ?
                    routeService.GetNenApiRoutesForAnalysis(generateRouteAssessment.ContentRefNo, (int)SessionInfo.OrganisationId, SessionInfo.UserSchema) :
                    routeService.GetRouteDetailForAnalysis(generateRouteAssessment.VersionId, generateRouteAssessment.ContentRefNo,
                    generateRouteAssessment.RevisionId, 0, SessionInfo.UserSchema);
                Parallel.Invoke(
                                () => routeAssessmentModel.AffectedStructure = !generateRouteAssessment.NoGenerateAffectedStructures ?
                                routeAssessmentService.GenerateAffectedStructures(routePartDetails, null, 0, SessionInfo.UserSchema) : null,
                                () => routeAssessmentModel.AffectedRoads = !generateRouteAssessment.NoGenerateAffectedRoads ? 
                                routeAssessmentService.GenerateAffectedRoads(routePartDetails, SessionInfo.UserSchema) : null,
                                () => routeAssessmentModel.Annotation = !generateRouteAssessment.NoGenerateAffectedAnnotations ? 
                                routeAssessmentService.GenerateAffectedAnnotation(routePartDetails, SessionInfo.UserSchema) : null,
                                () => routeAssessmentModel.Cautions = !generateRouteAssessment.NoGenerateAffectedCautions ? 
                                routeAssessmentService.GenerateAffectedCautions(routePartDetails, 0, SessionInfo.UserSchema) : null,
                                () => routeAssessmentModel.Constraints = !generateRouteAssessment.NoGenerateAffectedConstraints ? 
                                routeAssessmentService.GenerateAffectedConstraints(routePartDetails, SessionInfo.UserSchema) : null,
                                () => routeAssessmentModel.AffectedParties = !generateRouteAssessment.NoGenerateAffectedParties ? 
                                routeAssessmentService.GenerateAffectedParties(routePartDetails, generateRouteAssessment.NotificationId, orgId, SessionInfo.UserSchema, generateRouteAssessment.VSoType) : null
                               );
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"-RouteAssessment/GenerateRouteAssessment. End Time: {DateTime.Now}, AnalysisId:{generateRouteAssessment.AnalysisId}");
                if (!generateRouteAssessment.NoGenerateAffectedParties)
                {
                    byte[] affectedParties = routeAssessmentService.GetDriveInstructionsinfo(generateRouteAssessment.AnalysisId, 7, SessionInfo.UserSchema).AffectedParties;
                    if (generateRouteAssessment.NotificationId != 0)//Used to Haulier contact as manual affected part by default
                    {
                        List<AssessmentContacts> contactList = new List<AssessmentContacts>();
                        AssessmentContacts assessmentContact = new AssessmentContacts
                        {
                            ContactId = SessionInfo.ContactId,
                            ContactName = SessionInfo.FirstName + " " + SessionInfo.LastName,
                            OrganisationName = SessionInfo.OrganisationName,
                            Fax = null,
                            Email = SessionInfo.Email,
                            OrganisationType = "haulier"
                        };
                        contactList.Add(assessmentContact);
                        routeAssessmentModel.AffectedParties = UpdateManualAddedParties(routeAssessmentModel.AffectedParties, affectedParties, generateRouteAssessment.PreviousContactName, contactList, true);
                    }
                    else
                    {
                        int sortOrgId = Session["SORTOrgID"] != null ? Convert.ToInt32(Session["SORTOrgID"]) : 0;
                        int haulierOrgId = SessionInfo.UserTypeId == Common.Constants.UserType.Sort ? sortOrgId : (int)SessionInfo.OrganisationId;
                        
                        if (generateRouteAssessment.PrevAnalysisId != 0)//Comparing with last Movement Version and last candidate route affected parties
                        {
                            RouteAssessmentModel prevRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(generateRouteAssessment.PrevAnalysisId, 9, SessionInfo.UserSchema);
                            routeAssessmentModel.AffectedParties = ComparePreviousAffectedParties(routeAssessmentModel, prevRouteAssessmentModel);
                        }
                        if (haulierOrgId > 0)
                        {
                            var contactList = routeAssessmentService.FetchContactDetails(haulierOrgId, generateRouteAssessment.AppRevId, SessionInfo.UserSchema);
                            if (contactList.Count > 0)
                                contactList[0].OrganisationType = "haulier";
                            string contactName = contactList.Count > 0 ? contactList[0].ContactName : string.Empty;
                            routeAssessmentModel.AffectedParties = UpdateManualAddedParties(routeAssessmentModel.AffectedParties, affectedParties, contactName, contactList, false);
                        }
                    }
                }
                routeAssessmentService.UpdateAnalysedRoute(routeAssessmentModel, generateRouteAssessment.AnalysisId, SessionInfo.UserSchema);
                routeAssessmentService.GenerateDrivingInstnRouteDesc(routePartDetails, generateRouteAssessment.AnalysisId, SessionInfo.UserSchema);
                //Update ROUTES SET IS_MODIFIED=0 after route assessment for candidate routes
                if (generateRouteAssessment.RevisionId>0 && SessionInfo.UserSchema== UserSchema.Sort)
                    sortApplicationService.UpdateCandIsModified((int)generateRouteAssessment.AnalysisId);

                if (generateRouteAssessment.IsNenApi)
                    UpdateNenApiIcaStatus(routeAssessmentModel.AffectedStructure,generateRouteAssessment.NotificationId,(int)SessionInfo.OrganisationId);

                Session["RouteAssessmentFlag"] = "Completed";
                result = true;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateRouteAssessment, Exception:" + ex);
            }
            return Json(result);
        }

        private void UpdateNenApiIcaStatus(byte[] affectedStructure, long notificationId, int organisationId)
        {
            string xmlaffectedStructures = Encoding.UTF8.GetString(XsltTransformer.Trafo(affectedStructure));
            CommonNotifMethods commonNotif = new CommonNotifMethods();
            Dictionary<int, int> icaStatusDictionary = commonNotif.GetNotifICAstatus(xmlaffectedStructures);
            int suitability = icaStatusDictionary.ContainsKey(organisationId) ? icaStatusDictionary[organisationId] :
                (int)Common.Enums.ExternalApiEnums.ExternalApiSuitability.unknown;
            UpdateNENICAStatusParams updateNENICAStatusParams = new UpdateNENICAStatusParams
            {
                IcaStatus = suitability,
                OrganisationId = organisationId,
                NotificationId = notificationId,
                UserSchema = UserSchema.Portal
            };
            notificationService.UpdateNenApiIcaStatus(updateNENICAStatusParams);
        }
        #endregion

        #region Private Functions
        private static byte[] UpdateManualAddedParties(byte[] currentAffectedParties, byte[] previousAffectedParties, string prevContactName, List<AssessmentContacts> contactList, bool isNotif)
        {
            string xmlCurrentAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(currentAffectedParties));
            var currentAffectedParty = StringExtraction.xmlAffectedPartyDeserializer(xmlCurrentAffectedParties);
            if (previousAffectedParties != null)
            {
                string xmlPreviousAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(previousAffectedParties));
                var previousAffectedParty = StringExtraction.xmlAffectedPartyDeserializer(xmlPreviousAffectedParties);
                var manualAddedParties = previousAffectedParty.ManualAffectedParties.Where(
                            x => x.Contact.Contact.adhocContactRef.OrganisationName != contactList[0].OrganisationName).ToList();
                if (isNotif)
                {
                    currentAffectedParty = StringExtraction.ModifyingDispStatusToInUse(currentAffectedParty, previousAffectedParty);
                    manualAddedParties = previousAffectedParty.ManualAffectedParties.Where(
                            x => x.Contact.Contact.adhocContactRef.OrganisationName != contactList[0].OrganisationName && x.Contact.Contact.adhocContactRef.FullName != prevContactName).ToList();
                }
                currentAffectedParty.ManualAffectedParties = manualAddedParties;
            }
            UpdateHaulierAsManualAddedParty(ref currentAffectedParty, contactList);
            UpdateManualAddedPartiesEmail(currentAffectedParty);
            xmlCurrentAffectedParties = StringExtraction.XmlAffectedPartySerializer(currentAffectedParty);

            return StringExtraction.ZipAndBlob(xmlCurrentAffectedParties);
        }
        private static void UpdateManualAddedPartiesEmail(AffectedPartiesStructure currentAffectedParty)
        {
            var ReceiverMailId = ConfigurationManager.AppSettings["Receiver"];
            var IsLiveSystem = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveSystem"]);
            foreach (var item in currentAffectedParty.ManualAffectedParties.Select( x => x.Contact.Contact.adhocContactRef))
            {
                item.EmailAddress = !IsLiveSystem ? ReceiverMailId : item.EmailAddress; //it has been hard code as a part of RM 25491 need to fetch data from DB
            }
        }
        private static void UpdateHaulierAsManualAddedParty(ref AffectedPartiesStructure currentAffectedParty, List<AssessmentContacts> contactList)
        {
            string updatedXmlAffectedParty = StringExtraction.XmlAffectedPartySerializer(currentAffectedParty);
            string newXmlAffectedParties = StringExtraction.XmlAffectedPartyDeserializer(updatedXmlAffectedParty, contactList, true);
            if (newXmlAffectedParties != "Contact already exist")
                currentAffectedParty = StringExtraction.xmlAffectedPartyDeserializer(newXmlAffectedParties);
        }
        private byte[] ComparePreviousAffectedParties(RouteAssessmentModel currentRouteAssessmentModel, RouteAssessmentModel prevRouteAssessmentModel)
        {
            byte[] affectedParties = null;
            try
            {
                #region Storing Current and Previous AffectedParties
                string currentAffectedParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(currentRouteAssessmentModel.AffectedParties));

                AffectedPartiesStructure currentAffectedParty = StringExtraction.xmlAffectedPartyDeserializer(currentAffectedParties);

                string prevAffectedtdParties = Encoding.UTF8.GetString(XsltTransformer.Trafo(prevRouteAssessmentModel.AffectedParties));

                AffectedPartiesStructure prevAffectedParty = StringExtraction.xmlAffectedPartyDeserializer(prevAffectedtdParties);

                #endregion

                #region Comparting Current and Previous Affected Parties
                //comparing both list of affected parties
                currentAffectedParty = StringExtraction.checkAffectedPartiesDiff(prevAffectedParty, currentAffectedParty);

                currentAffectedParty = StringExtraction.SortMovementAffectedPartyComparison(currentRouteAssessmentModel, prevRouteAssessmentModel, currentAffectedParty);

                UpdateExcludeStatus(currentAffectedParty, prevAffectedParty);
                #endregion

                string latestAffectedParties = StringExtraction.XmlAffectedPartySerializer(currentAffectedParty);

                affectedParties = StringExtractor.ZipAndBlob(latestAffectedParties);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/GenerateAffectedParties/Comaprison, Exception:" + ex);
            }
            return affectedParties;
        }
        private static void UpdateExcludeStatus(AffectedPartiesStructure currentAffectedParty, AffectedPartiesStructure prevAffectedParty)
        {
            if (currentAffectedParty.GeneratedAffectedParties.Count != 0 || currentAffectedParty.GeneratedAffectedParties != null)
            {
                var affectedPartylist = currentAffectedParty.GeneratedAffectedParties;

                if (prevAffectedParty.GeneratedAffectedParties != null || prevAffectedParty.GeneratedAffectedParties.Count != 0)
                {
                    foreach (AffectedPartyStructure tmpAfftdParty in prevAffectedParty.GeneratedAffectedParties)
                    {
                        string tmpOrgName = tmpAfftdParty.Contact.Contact.simpleContactRef.OrganisationName;
                        int tmpContId = tmpAfftdParty.Contact.Contact.simpleContactRef.ContactId;
                        int tmpOrgId = tmpAfftdParty.Contact.Contact.simpleContactRef.OrganisationId;

                        //if current list has a name and organisation from previous list 
                        if (currentAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId)
                           && currentAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.ContactId == tmpContId)
                            && currentAffectedParty.GeneratedAffectedParties.Exists(x => x.Contact.Contact.simpleContactRef.OrganisationName == tmpOrgName)
                            )
                        {
                            //inserting the excluded status
                            (from s in affectedPartylist
                             where (s.Contact.Contact.simpleContactRef.ContactId == tmpContId && s.Contact.Contact.simpleContactRef.OrganisationId == tmpOrgId && s.Contact.Contact.simpleContactRef.OrganisationName == tmpOrgName)
                             select s).ToList().ForEach(s => s.Exclude = tmpAfftdParty.Exclude);
                        }
                    }
                }
                currentAffectedParty.GeneratedAffectedParties = affectedPartylist.OrderBy(t => t.Contact.Contact.simpleContactRef.OrganisationName).ToList();
            }
        }
        #endregion

        #region GetAffectedParties
        public ActionResult GetDrivingInstructions(long analysisID, int anal_type = 1)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                string path = null;

                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                string result = string.Empty;

                //Code to check route planner unit session value to display driving instruction's in Metric or imperial system
                if (SessionInfo.RoutePlanUnits == 692001)
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DrivingInstructionsMetric.xslt");
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\DrivingInstructionsImperial.xslt");
                }

                result = XsltTransformer.Trafo(objRouteAssessmentModel.DriveInst, path, out string errormsg);
                ViewBag.instruction = result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetDrivingInstructions, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
            return PartialView("ListDrivingInstructions");
        }
        #endregion

        #region GetAffectedStructures
        public ActionResult GetAffectedStructures(long analysisID, int anal_type = 3, bool isAuthMove = false, int FilterByOrgID = 0, bool ShowAllStruct = false
            , bool UnSuitableShowAllStructures = false, bool IsOverbridge = false, bool IsUnderbridge = false, bool IsLevelCrossing = false, bool UnSuitableShowAllCautions = false,bool IsPlanMovement=false)
        {
            try
            {
                ViewBag.AffectedLength = 0;

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int orgId = (int)SessionInfo.OrganisationId;
                int userTypeId = SessionInfo.UserTypeId;
                if (userTypeId == STP.Common.Constants.UserType.Sort || !IsPlanMovement)
                {
                    UnSuitableShowAllStructures = !UnSuitableShowAllStructures;
                    UnSuitableShowAllCautions = !UnSuitableShowAllCautions;
                }
                ViewBag.IsPlanMovement = IsPlanMovement;
                string result = string.Empty;
                ViewBag.IsChecked = ShowAllStruct;
                ViewBag.analysisId = analysisID;
                ViewBag.UnSuitableShowAllStructures = UnSuitableShowAllStructures;
                ViewBag.UnSuitableShowAllCautions = UnSuitableShowAllCautions;
                ViewBag.IsOverbridge = IsOverbridge;
                ViewBag.IsUnderbridge = IsUnderbridge;
                ViewBag.IsLevelCrossing = IsLevelCrossing;
                ViewBag.FilterByOrgID = FilterByOrgID;

                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);



                ViewBag.IsAuthMove = isAuthMove;
                #region fetching affected structures list from xml to store in structure list object

                //get affectedstructures xml from byte[]
                string affectedStructuresxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.AffectedStructure));
                AnalysedStructures newAnalysedStructures = XmlAffectedStructuresDeserializer.XmlAffectedStructuresDeserialize(affectedStructuresxml);

                string cautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.Cautions));
                STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions>.XmlDeserialize(cautionsxml);
                routeAssessmentService.GetUnsuitableStructuresWithCautions(UnSuitableShowAllStructures, UnSuitableShowAllCautions, ref newAnalysedStructures, ref newCautions);

                List<StructuresToAssess> structAssessList = new List<StructuresToAssess>();
                int routePartNo = 1;
                foreach (AnalysedStructuresPart analstrucpart in newAnalysedStructures.AnalysedStructuresPart)
                {
                    foreach (Structure struc in analstrucpart.Structure)
                    {
                        if (struc.TraversalType == "underbridge")
                        {
                            foreach (var org in struc.StructureResponsibility.StructureResponsibleParty)
                            {
                                if (org.OrganisationId == orgId)
                                {
                                    StructuresToAssess structAssess = new StructuresToAssess();
                                    structAssess.ESRN = struc.ESRN;
                                    structAssess.SectionId = (int)struc.StructureSectionId;
                                    structAssess.RouteId = analstrucpart.Id;
                                    structAssess.RoutePartNo = routePartNo;
                                    structAssessList.Add(structAssess);
                                }
                            }
                        }
                    }
                    routePartNo++;
                }

                ViewBag.StructureAssessList = structAssessList;
                #endregion
                return PartialView("ListAffectedStructures", newAnalysedStructures);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetAffectedStructures, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region GetAffectedConstraints
        public ActionResult GetAffectedConstraints(long analysisID, int anal_type = 5, bool UnSuitableShowAllConstraint = false, bool UnSuitableShowAllCautions = false, bool IsPlanMovement = false)
        {
            var applicationNotificationManagement = new ApplicationNotificationManagement(applicationNotificationWorkflowService);
            var planMvmt = applicationNotificationManagement.GetPlanMvmtPayload();
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int userTypeId = SessionInfo.UserTypeId;
                if (userTypeId == STP.Common.Constants.UserType.Sort || !IsPlanMovement)
                {
                    UnSuitableShowAllConstraint = !UnSuitableShowAllConstraint;
                    UnSuitableShowAllCautions = !UnSuitableShowAllCautions;
                }
                ViewBag.UnSuitableShowAllConstraint = UnSuitableShowAllConstraint;
                ViewBag.UnSuitableShowAllCautions = UnSuitableShowAllCautions;
                ViewBag.IsPlanMovement = IsPlanMovement;
                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                if (objRouteAssessmentModel.Constraints == null)
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Constraints not generated (Condition's did not match)");



                string constraintsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.Constraints));
                STP.Domain.RouteAssessment.XmlConstraints.AnalysedConstraints newConstraints = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlConstraints.AnalysedConstraints>.XmlDeserialize(constraintsxml);

                string cautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.Cautions));
                STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions newCautions = XmlDeserializerGeneric<STP.Domain.RouteAssessment.XmlAnalysedCautions.AnalysedCautions>.XmlDeserialize(cautionsxml);
                routeAssessmentService.GetUnsuitableConstraintsWithCautions(UnSuitableShowAllConstraint, UnSuitableShowAllCautions, ref newConstraints, ref newCautions);

                return PartialView("ListConstraints", newConstraints);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetAffectedConstraints, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region GetAffectedCautions
        public ActionResult GetAffectedCautions(long analysisID, int anal_type = 4)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                string result = string.Empty;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\CautionsMetric.xslt");
                result = XsltTransformer.Trafo(objRouteAssessmentModel.Cautions, path, out string errormsg);
                ViewBag.Cautions = result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetAffectedCautions, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
            return PartialView("ListCautions");
        }
        #endregion

        #region GetAffectedAnnotations
        public ActionResult GetAffectedAnnotations(long analysisID, int anal_type = 6)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                string result = string.Empty;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\Annotation.xslt");
                result = XsltTransformer.Trafo(objRouteAssessmentModel.Annotation, path, out string errormsg);
                ViewBag.annotation = result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetAffectedAnnotations, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
            return PartialView("ListAnnotations");
        }
        #endregion

        #region GetAffectedParties
        /// <summary>
        ///  function to display Affected Parties
        /// </summary>
        /// <param name="analysisID"></param>
        /// <param name="revisionId"></param>
        /// <param name="anal_type"></param>
        /// <param name="isAuthMove"></param>
        /// <param name="IsCandidate"></param>
        /// <returns></returns>
        public ActionResult GetAffectedParties(long analysisID = 0, int revisionId = 0, int anal_type = 7, bool isAuthMove = false, bool IsCandidate = false, int versionId = 0, bool IsVR1 = false, int AgreedSONotif = 0)
        {
            try
            {
                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];

                int orgId = 0;
                string path = null;
                orgId = SessionInfo.organisationId;

                RouteAssessmentModel objRouteAssessmentModel;

                if (!IsCandidate)
                {
                    if (isAuthMove)
                    {
                        //here you are fetching the affected parties
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                        if (objRouteAssessmentModel.AffectedParties != null)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Get drive instructions info successfully completed");
                        }
                    }
                    else
                    {
                        //in case of revision calls that aren't from notification , and route is not a candidate 
                        if (revisionId == 0 && versionId != 0 && IsVR1) // VR - 1 Assessment
                        {
                            int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                        }
                        else if (revisionId == 0 && versionId != 0 && !IsVR1) // SO Agreed / proposed route Assessment
                        {
                            long verStatus = routeAssessmentService.GetMovementStatus(versionId, null, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get movement status successfully completed verStatus:{0}", verStatus));
                            if (verStatus == 305004 || verStatus == 305005 || verStatus == 305006 || verStatus == 305013 || verStatus == 305014)
                            {
                                //do nothing
                            }
                            else
                            {
                                int updated = routeAssessmentService.updateRouteAssessment(versionId, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                            }
                        }
                        else
                        {
                            int updated = routeAssessmentService.updateRouteAssessment("", 0, revisionId, 0, orgId, (int)analysisID, anal_type, SessionInfo.UserSchema);
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));
                        }

                        //here you are fetching the affected road's
                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                        if (objRouteAssessmentModel.AffectedRoads != null)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Get drive instructions info successfully completed");
                        }
                    }
                }
                else
                {
                    #region Route Assessment Input
                    RouteAssessmentInputs inputsForAssessment = new RouteAssessmentInputs();

                    inputsForAssessment.AnalysisId = analysisID;

                    inputsForAssessment.IsCanditateRoute = IsCandidate;

                    inputsForAssessment.OrganisationId = orgId;

                    inputsForAssessment.RevisionId = revisionId;
                    #endregion

                    //fetching affected parties information 
                    objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                    if (objRouteAssessmentModel.AffectedParties != null)
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Get drive instructions info successfully completed");
                    }

                    if (objRouteAssessmentModel.AffectedParties == null)
                    {

                        int updated = routeAssessmentService.updateRouteAssessment(inputsForAssessment, anal_type, SessionInfo.UserSchema);//To update driving instructions
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Update route assessment successfully completed updated:{0}", updated));

                        objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);
                        if (objRouteAssessmentModel.AffectedParties != null)
                        {
                            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Get drive instructions info successfully completed");
                        }
                    }
                }
                string result = string.Empty;


                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedParties.xslt");

                result = XsltTransformer.Trafo(objRouteAssessmentModel.AffectedParties, path, out string errormsg);

                ViewBag.AffectedParties = result;
                ViewBag.AnalysisId = analysisID;

                return PartialView("ListAffectedParties");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/ListAffectedParties, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region GetAffectedRoads
        public ActionResult GetAffectedRoads(long analysisID = 0, int anal_type = 8, int FilterByOrgID = 0, bool isStructureAffected = false)
        {
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"RouteAssessment/GetAffectedRoads actionResult method started successfully");

                UserInfo SessionInfo = (UserInfo)Session["UserInfo"];
                int userTypeId = SessionInfo.UserTypeId;
                string path = null;

                RouteAssessmentModel objRouteAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(analysisID, anal_type, SessionInfo.UserSchema);

                if (userTypeId == 696007 || userTypeId == 696008 && (!isStructureAffected))
                {
                    FilterByOrgID = SessionInfo.organisationId; //#3582 SOA's / Police can only view their affected roads
                }

                string result = string.Empty;
                //Code to check route planner unit session value to display driving instruction's in Metric or imperial system
                path = SessionInfo.RoutePlanUnits == 692001 && SessionInfo.UserSchema != UserSchema.Sort
                    ? FilterByOrgID != 0
                        ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\MyAffectedRoadsMetric.xslt")
                        : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedRoadsMetric.xslt")
                    : FilterByOrgID != 0
                        ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\MyAffectedRoadsImperial.xslt")
                        : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\XSLT\AffectedRoadsImperial.xslt");

                result = FilterByOrgID != 0
                    ? XsltTransformer.Trafo(objRouteAssessmentModel.AffectedRoads, path, FilterByOrgID, 0, out string errormsg)
                    : XsltTransformer.Trafo(objRouteAssessmentModel.AffectedRoads, path, out errormsg);
                ViewBag.AffectedRoads = result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RouteAssessment/GetAffectedRoads, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

            return PartialView("ListAffectedRoads");
        }
        #endregion

        #endregion
    }
}