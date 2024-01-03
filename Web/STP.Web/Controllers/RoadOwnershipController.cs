using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using STP.Common.Logger;
using NetSdoGeometry;
using STP.ServiceAccess.RoadNetwork;
using STP.Web.Controllers;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.Domain.SecurityAndUsers;
using STP.Web.WorkflowProvider;
using Newtonsoft.Json;

namespace STP.Business.Controllers
{
    public class RoadOwnershipController : Controller
    {
        private readonly IRoadOwnershipService roadOwnershipService;

        public RoadOwnershipController(IRoadOwnershipService roadOwnershipService)
        {
            this.roadOwnershipService = roadOwnershipService;
        }

        #region RoadOwnershipMap
        [HttpGet]
        public ActionResult showRoadOwnership()
        {
            //#region Session Check
            //if (Session["UserInfo"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //#endregion
            #region Page access check
            if (!PageAccess.GetPageAccess("100002"))
            {
                return RedirectToAction("Error", "Home");
            }
            #endregion
            RoadOwnershipData roadOwnershipListObj = new RoadOwnershipData();
            return View("RoadOwnershipMap", roadOwnershipListObj);
        }
        #endregion

        #region RoadOwnerList
        public ActionResult SearchRoadOwnershipOrg()
        {
            try
            {
                #region Session Check and page access check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!PageAccess.GetPageAccess("100002"))
                {
                    return RedirectToAction("Login", "Account");
                }
                #endregion
                RoadOwnershipData roadOwnershipListObj = new RoadOwnershipData();
                return View(roadOwnershipListObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region CreateRoadOwnership

        public ActionResult CreateRoadOwnership(RoadOwnershipData roadOwnershipObject, bool allFeilds = false)
        {
            try
            {
                #region Session Check and page access check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!PageAccess.GetPageAccess("100002"))
                {
                    return RedirectToAction("Login", "Account");
                }
                #endregion

                ViewBag.orginFrom = "CreateOwner";
                ViewBag.allFeilds = allFeilds;
                return View(roadOwnershipObject);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/CreateRoadOwnership, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ListRoadContactList
        [HttpGet]
        public ActionResult ListRoadContactList(int page = 1, int pageSize = 10, int searchFlag = 1, string SearchString = null, string origin = "")
        {
            try
            {
                //#region Session Check
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                //#endregion
                #region Page access check
                if (!PageAccess.GetPageAccess("100002"))
                {
                    return RedirectToAction("Error", "Home");
                }
                #endregion

                if (ModelState.IsValid)
                {
                    #region Paging Part
                    int pageNumber = page;
                    ViewBag.searchFlag = searchFlag;
                    ViewBag.pageSize = pageSize;
                    ViewBag.page = pageNumber;
                    #endregion

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

                    ViewBag.origin = origin;
                    List<RoadOwnershipOrgSummary> orgListSummary = roadOwnershipService.GetRoadOwnershipOrganisations(SearchString, page, pageSize, searchFlag);
                    if (orgListSummary != null && orgListSummary.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(orgListSummary[0].TotalRows);
                        IPagedList<RoadOwnershipOrgSummary> model = orgListSummary.ToPagedList(page, 10);
                        ViewBag.TotalPages = model.PageCount;
                    }
                    else
                    {
                        ViewBag.TotalCount = 0;
                        ViewBag.TotalPages = 0;
                    }


                    List<test> objtest = new List<test>(){
                      new test(){ value = "AL", label = "Alabama" },
                      new test(){ value = "AL", label = "Alabama" },
                      new test(){ value = "AL", label = "Alabama" },
                      new test(){ value = "AL", label = "Alabama" }
                  };

                    //var OrganisationListAsIPagedList = new StaticPagedList<RoadOwnershipOrgSummary>(orgListSummary, pageNumber, pageSize, ViewBag.TotalCount);
                    return Json(orgListSummary, "application/json", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region saveRoadOwnership
        public ActionResult saveRoadOwnership(int postFlag, List<OwnerInfo> newOwnerList, List<ArrangementDetails> newManagerDelegationDetails, List<LinkInfo> assignedLinkInfo, List<LinkInfo> unassignedLinkInfo, int length)
        {
            try
            {
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!PageAccess.GetPageAccess("100001"))
                {
                    return RedirectToAction("Login", "Account");
                }

                if (postFlag == 0)
                {
                    Session["newOwnerList"] = newOwnerList;
                    Session["newManagerDelegation"] = newManagerDelegationDetails;
                    Session["assignedLinkInfo"] = null;
                    Session["unassignedLinkInfo"] = null;
                    return Json(new { result = true });
                }
                else if (postFlag == 1)
                {
                    List<LinkInfo> completeList;
                    if (Session["assignedLinkInfo"] == null)
                    {
                        completeList = new List<LinkInfo>();
                    }
                    else
                    {
                        completeList = (List<LinkInfo>)Session["assignedLinkInfo"];
                    }
                    if (assignedLinkInfo == null)
                    {
                        assignedLinkInfo = new List<LinkInfo>();
                    }
                    completeList.AddRange(assignedLinkInfo);
                    Session["assignedLinkInfo"] = completeList;

                    if (completeList.Count == length)
                    {
                        return Json(new { result = true });
                    }
                    else
                    {
                        return Json(new { result = false });
                    }
                }
                else if (postFlag == 2)
                {
                    List<LinkInfo> completeList;
                    if (Session["unassignedLinkInfo"] == null)
                    {
                        completeList = new List<LinkInfo>();
                    }
                    else
                    {
                        completeList = (List<LinkInfo>)Session["unassignedLinkInfo"];
                    }
                    if (unassignedLinkInfo == null)
                    {
                        unassignedLinkInfo = new List<LinkInfo>();
                    }
                   
                    completeList.AddRange(unassignedLinkInfo);
                    
                    Session["unassignedLinkInfo"] = completeList;

                    if (completeList.Count == length)
                    {
                        RoadOwnerShipDetails roadOwnershipDetails = new RoadOwnerShipDetails();
                        roadOwnershipDetails.NewOwnerList = (List<OwnerInfo>)Session["newOwnerList"];
                        roadOwnershipDetails.NewManagerDelegationDetailsList = (List<ArrangementDetails>)Session["newManagerDelegation"];
                        roadOwnershipDetails.AssignedLinkInfoList = (List<LinkInfo>)Session["assignedLinkInfo"];
                        roadOwnershipDetails.UnassignedLinkInfoList = (List<LinkInfo>)Session["unassignedLinkInfo"];

                        bool status = SaveRO(roadOwnershipDetails);
                        return Json(new { result = true, status = status });
                    }
                    else
                    {
                        return Json(new { result = false });
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region RoadOwnerReport
        [HttpPost]
        public ActionResult RoadOwnerReport(List<LinkInfo> assignedLinkInfoArray, List<OwnerInfo> newOwnerList, int page = 1, int pageSize = 5, int assignedLinkInfoCount = 0, bool view = true)
        {
            //#region Session check
            //if (Session["UserInfo"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //#endregion
            ViewBag.pageSize = pageSize;
            List<RoadOwnershipData> rdOwnershipDetailsObj = roadOwnershipService.GetRoadOwnershipDetails(assignedLinkInfoArray, page, pageSize);
            int lstCount = rdOwnershipDetailsObj.Count;
            int ownerLen = newOwnerList.Count;
            int len = pageSize;
            if (!view)
                len = lstCount;
            for (int i = 0; i < len; i++)
            {
                if (i >= lstCount)
                    break;
                for (int j = 0; j < ownerLen; j++)
                {
                    switch (newOwnerList[j].Type)
                    {
                        case 1:
                            rdOwnershipDetailsObj[i].ManagerName = newOwnerList[j].OwnerName;
                            break;
                        case 2:
                            rdOwnershipDetailsObj[i].LocalAuthorityName = newOwnerList[j].OwnerName;
                            break;
                        case 3:
                            rdOwnershipDetailsObj[i].HaMacName = newOwnerList[j].OwnerName;
                            break;
                        case 4:
                            rdOwnershipDetailsObj[i].PoliceName = newOwnerList[j].OwnerName;
                            break;
                    }

                }
            }
            if (newOwnerList.Count > 0)
            {
                ViewBag.ownerCnt = newOwnerList.Count;
                ViewBag.changeFlag = true;
                ViewBag.TotalCount = assignedLinkInfoCount;
            }
            else
            {
                ViewBag.changeFlag = false;
                ViewBag.TotalCount = 0;
            }

            var pagedRoadOwnershipObj = new StaticPagedList<RoadOwnershipData>(rdOwnershipDetailsObj, page, pageSize, assignedLinkInfoCount);
            if (view)
                return View(pagedRoadOwnershipObj);
            else
                return Json(new { pagedRoadOwnershipObj });
        }
        #endregion

        private bool SaveRO(RoadOwnerShipDetails linkIdList)
        {
            RoadOwnerShipDetails rdOwnershipObj = new RoadOwnerShipDetails();
            bool status = false;
            int Recods = linkIdList.AssignedLinkInfoList.Count, x = 0;
            int VRcount = 0, EIndex = 0;
            while (Recods >= 2000)
            {
                x++;
                rdOwnershipObj.AssignedLinkInfoList = null;

                if (Recods != linkIdList.AssignedLinkInfoList.Count)
                {
                    VRcount = EIndex + 1;
                    if (x != 2)
                        EIndex = EIndex + 2000;
                    else EIndex = 2000;
                    rdOwnershipObj.AssignedLinkInfoList = linkIdList.AssignedLinkInfoList.GetRange(VRcount, 2000);
                }
                else
                {
                    EIndex = EIndex + 2001;
                    rdOwnershipObj.AssignedLinkInfoList = linkIdList.AssignedLinkInfoList.GetRange(VRcount, 2001);
                }

                rdOwnershipObj.NewOwnerList = (List<OwnerInfo>)Session["newOwnerList"];
                rdOwnershipObj.NewManagerDelegationDetailsList = (List<ArrangementDetails>)Session["newManagerDelegation"];
                rdOwnershipObj.UnassignedLinkInfoList = (List<LinkInfo>)Session["unassignedLinkInfo"];

                status = roadOwnershipService.SaveRoadOwnership(rdOwnershipObj);
                Recods = Recods - 2000;
            }

            if (Recods > 0)
            {
                rdOwnershipObj.AssignedLinkInfoList = null;
                rdOwnershipObj.AssignedLinkInfoList = linkIdList.AssignedLinkInfoList;
                rdOwnershipObj.NewOwnerList = (List<OwnerInfo>)Session["newOwnerList"];
                rdOwnershipObj.NewManagerDelegationDetailsList = (List<ArrangementDetails>)Session["newManagerDelegation"];
                rdOwnershipObj.UnassignedLinkInfoList = (List<LinkInfo>)Session["unassignedLinkInfo"];
                status = roadOwnershipService.SaveRoadOwnership(rdOwnershipObj);
            }

            return status;
        }

        #region getRoadDelegationDetails

        [HttpPost]
        public ActionResult getRoadDelegationDetails(int roadOwnerID)
        {
            try
            {
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!PageAccess.GetPageAccess("100001"))
                {
                    return RedirectToAction("Login", "Account");
                }
                List<ArrangementDetails> delegDetails = roadOwnershipService.GetDelegationArrangementDetails(roadOwnerID);
                return Json(new { delegDetails });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region GetRoadOwnedDetails
        //<summary>
        //Fetching road ownership information to display on map based on Delegator , delegating or Geometry
        //</summary>
        //<param name="arrangementId"></param>
        //<param name="fetchFlag"></param>
        //<param name="areaGeom"></param>
        //<param name="searchParam"></param>
        //<returns></returns>
        [HttpPost]
        public JsonResult GetRoadOwnedDetails(int organisationId = 0, int fetchFlag = 0, sdogeometry areaGeom = null, int zoomLevel = 1)
        {

            List<LinkInfo> roadLinkInfo = roadOwnershipService.FetchRoadInfoToDisplayOnMap(organisationId, fetchFlag, areaGeom, zoomLevel);
            string rson = JsonConvert.SerializeObject(roadLinkInfo);
            List<LinkInfoJson> LinkinfoJsonVar = JsonConvert.DeserializeObject<List<LinkInfoJson>>(rson);

            var jsonResult = Json(LinkinfoJsonVar, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            }
        #endregion

        #region public ActionResult RoadOwnersContact(long linkID)
        public ActionResult RoadOwnersContact(long linkID, long length, string pageType = null)
        {
            List<RoadContactModal> roadContactsLst;

            //if (Session["UserInfo"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            var SessionInfo = (UserInfo)Session["UserInfo"];

            roadContactsLst = roadOwnershipService.GetRoadOwnerContactList(linkID, length, pageType, SessionInfo.UserSchema);
            return View(roadContactsLst);
        }
        #endregion

        #region getUnassignedRoads
        [HttpPost]
        public ActionResult getUnassignedRoads(List<LinkInfo> linkInfoList, int length)
        {
            try
            {
                //if (Session["UserInfo"] == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                if (!PageAccess.GetPageAccess("100001"))
                {
                    return RedirectToAction("Login", "Account");
                }

                List<LinkInfo> completeList;
                if (Session["linkInfo"] == null)
                {
                    completeList = new List<LinkInfo>();
                }
                else
                {
                    completeList = (List<LinkInfo>)Session["linkInfo"];
                }
                completeList.AddRange(linkInfoList);
                Session["linkInfo"] = completeList;

                if (completeList.Count == length)
                {
                    Session["linkInfo"] = null;

                    List<long> linkIdList = new List<long>();
                    if (completeList.Count > 0)
                    {
                        foreach (LinkInfo linkInfoObj in completeList)
                        {
                            linkIdList.Add(linkInfoObj.LinkId);
                        }
                    }
                    List<long> UnAssignedlinkIds = roadOwnershipService.GetUnassignedLinks(linkIdList);
                    List<LinkInfo> assignedLinkInfo = new List<LinkInfo>(completeList);
                    List<LinkInfo> unAssignedLinkInfo = new List<LinkInfo>(completeList);

                    foreach (long item in UnAssignedlinkIds)
                        assignedLinkInfo.RemoveAll(x => x.LinkId == item);
                    foreach (LinkInfo item in assignedLinkInfo)
                        unAssignedLinkInfo.RemoveAll(x => x.LinkId == item.LinkId);

                    var result = Json(new { status = true, result1 = assignedLinkInfo, result2 = unAssignedLinkInfo }, JsonRequestBehavior.AllowGet);
                    result.MaxJsonLength = int.MaxValue;
                    return result;
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("RoadOwnership/showRoadOwnership, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

    }
    class test
    {
        public string value { get; set; }
        public string label { get; set; }

    }
}
