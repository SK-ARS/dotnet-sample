using PagedList;
using STP.Common.Logger;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.LoggingAndReporting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService reportService;
        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }
        
        #region SessionLengthReport

        /// <summary>
        /// Get session length history
        /// </summary>
        /// <returns></returns>
        public ActionResult SessionLengthReport()
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("15001"))
                {
                    return RedirectToAction("Error", "Home");
                }

                return View("SessionLengthReport");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/SessionLengthReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SessionLengthHistoryTypeWise
        /// <summary>
        /// Get Session Length history typewise
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult SessionLengthHistoryTypeWise(int month, int year)
        {
            try
            {
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Login", "Account");

                if (!PageAccess.GetPageAccess("15001"))
                    return RedirectToAction("Error", "Home");

                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;

                    List<SessionLengthModel> lstSessionLengthModel = reportService.GetTotalSessionLengthHistoryTypewise(month, year);
                    List<SessionLengthModel> listAvgSessionLength = reportService.GetAvgSessionLength(month, year);
                    List<SessionLengthModel> listPeakHrSessionMonth = reportService.GetPeakHourSessionInMonth(month, year);
                    GetSessionReport(lstSessionLengthModel, listAvgSessionLength, listPeakHrSessionMonth);
                    return PartialView("PartialView/_SessionLengthHistoryTypeWise", lstSessionLengthModel);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/SessionLengthHistoryTypeWise, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/SessionLengthHistoryTypeWise, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Private function for SessionLengthHistoryTypeWise
        private void GetSessionReport(List<SessionLengthModel> listSessionLength, List<SessionLengthModel> listAvgSessionLength, List<SessionLengthModel> listPeakHrSessionMonth)
        {
            int Average = 0;
            int itemCount = 0;
            decimal HaulierAvg = 0;
            decimal PoliceAvg = 0;
            decimal SOAAvg = 0;
            decimal TotalSession = 0;
            foreach (SessionLengthModel sesslen in listSessionLength)
            {
                switch (sesslen.UserType)
                {
                    case "HAULIER":
                        {
                            if (listAvgSessionLength.Count > 0)
                            {
                                sesslen.AvgSessionLength = Convert.ToInt32(FindAvgDuration(696001, listAvgSessionLength));
                                Average += sesslen.AvgSessionLength;
                                itemCount++;
                            }
                            if (listPeakHrSessionMonth.Count > 0)
                            {
                                var item = listPeakHrSessionMonth.Find(i => i.UserType == "HAULIER");

                                if (item != null && sesslen.TotalSessionInMonth != 0)
                                    sesslen.NoofSessionInPeakDay = item.NoofSessionInPeakDay;
                            }
                            HaulierAvg = sesslen.AvgSessionLength * sesslen.TotalSessionInMonth;
                            TotalSession += sesslen.TotalSessionInMonth;
                        }
                        break;
                    case "POLICE":
                        {
                            if (listAvgSessionLength.Count > 0)
                            {
                                sesslen.AvgSessionLength = Convert.ToInt32(FindAvgDuration(696002, listAvgSessionLength));
                                Average += sesslen.AvgSessionLength;
                                itemCount++;
                            }
                            if (listPeakHrSessionMonth.Count > 0)
                            {
                                var item = listPeakHrSessionMonth.Find(i => i.UserType == "POLICE");

                                if (item != null && sesslen.TotalSessionInMonth != 0)
                                    sesslen.NoofSessionInPeakDay = item.NoofSessionInPeakDay;
                            }
                            PoliceAvg = sesslen.AvgSessionLength * sesslen.TotalSessionInMonth;
                            TotalSession += sesslen.TotalSessionInMonth;
                        }
                        break;
                    case "SOA":
                        {
                            if (listAvgSessionLength.Count > 0)
                            {
                                sesslen.AvgSessionLength = Convert.ToInt32(FindAvgDuration(696007, listAvgSessionLength));
                                Average += sesslen.AvgSessionLength;
                                itemCount++;
                            }
                            if (listPeakHrSessionMonth.Count > 0)
                            {
                                var item = listPeakHrSessionMonth.Find(i => i.UserType == "SOA");

                                if (item != null && sesslen.TotalSessionInMonth != 0)
                                    sesslen.NoofSessionInPeakDay = item.NoofSessionInPeakDay;
                            }
                            SOAAvg = sesslen.AvgSessionLength * sesslen.TotalSessionInMonth;
                            TotalSession += sesslen.TotalSessionInMonth;
                        }
                        break;
                    case "ALL":
                        {
                            if (listPeakHrSessionMonth.Count > 0)
                            {
                                var item = listPeakHrSessionMonth.Find(i => i.UserType == "ALL");

                                if (item != null && sesslen.TotalSessionInMonth != 0)
                                    sesslen.NoofSessionInPeakDay = item.NoofSessionInPeakDay;
                            }
                            break;
                        }
                }
            }
            if (TotalSession > 0)
            {
                listSessionLength.Find(l => l.UserType == "ALL").AvgSessionLength = Convert.ToInt32((HaulierAvg + PoliceAvg + SOAAvg) / TotalSession);
            }
            ViewBag.TotalCount = TotalSession;
        }
        private static double FindAvgDuration(int userTypeId, List<SessionLengthModel> list)
        {
            if (list.Count == 0)
                return 0;

            double sumDuration = 0;
            double avgDuration = 0;
            int countElement = 0;
            foreach (SessionLengthModel type in list)
            {
                if ((type.UserTypeID == userTypeId || userTypeId == 0) && type.SessionDuration > 0)
                {
                    sumDuration += type.SessionDuration;
                    countElement++;
                }
            }
            if (countElement > 0)
                avgDuration = sumDuration / countElement;
            return avgDuration;
        }
        #endregion

        #region SessionLengthTypeWiseExportToCSV
        /// <summary>
        /// export report data to csv
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public void SessionLengthTypeWiseExportToCSV(int month, int year)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;

                    List<SessionLengthModel> lstSessionLengthModel = reportService.GetTotalSessionLengthHistoryTypewise(month, year);
                    List<SessionLengthModel> listAvgSessionLength = reportService.GetAvgSessionLength(month, year);
                    List<SessionLengthModel> listPeakHrSessionMonth = reportService.GetPeakHourSessionInMonth(month, year);
                    GetSessionReport(lstSessionLengthModel, listAvgSessionLength, listPeakHrSessionMonth);
                    
                    StringWriter sw = new StringWriter();

                    sw.WriteLine("\"User type\",\"Total sessions in month\",\"Most sessions in one day\",\"Average sessions length unit\",\"No of peak sessions in day\"");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=SessionLengthTypeWiseReport.csv");
                    Response.ContentType = "text/csv";

                    foreach (var sessionLength in lstSessionLengthModel)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"",
                                                   sessionLength.UserType,
                                                   sessionLength.TotalSessionInMonth,
                                                   sessionLength.MostSessionInDay,
                                                   sessionLength.AvgSessionLength,
                                                   sessionLength.NoofSessionInPeakDay));
                    }

                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/SessionLengthTypeWiseExportToCSV, Exception: {0}", ex));
            }
        }
        #endregion

        #region SessionLengthHistory
        /// <summary>
        ///  Get session length history
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public ActionResult SessionLengthHistory(int month, int year, string userType = "ALL")
        {
            try
            { 
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("15001"))
                {
                    return RedirectToAction("Error", "Home");
                }

                if (ModelState.IsValid)
                {

                    ViewBag.Month = month;
                    ViewBag.Year = year;

                    ViewBag.UserType = userType; 
                    List<SessionLengthModel> lstSessionLengthModel = reportService.GetSessionLengthHistory(month, year, userType);

                    ViewBag.TotalCount = lstSessionLengthModel.Count;

                    return PartialView("PartialView/_SessionLengthHistory", lstSessionLengthModel);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/SessionLengthHistory, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/SessionLengthHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region SessionLengthExportToCSV
        /// <summary>
        ///  export report data to csv
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="userType"></param>
        public void SessionLengthExportToCSV(int month, int year, string userType = "ALL")
        {
            try
            {

                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                    ViewBag.UserType = userType;

                    List<SessionLengthModel> lstSessionLengthModel = reportService.GetSessionLengthHistory(month, year, userType);

                    StringWriter sw = new StringWriter();

                    if (userType == "SOA")
                    {
                        sw.WriteLine("\"Day\",\"SOA sessions\"");

                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment;filename=SesionLengthReport.csv");
                        Response.ContentType = "text/csv";

                        foreach (var sessionLength in lstSessionLengthModel)
                        {
                            sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                       sessionLength.StartDate.ToString("dd/MM/yyyy"),
                                                       sessionLength.SOASessionCount));
                        }
                    }
                    else if (userType == "POLICE")
                    {
                        sw.WriteLine("\"Day\",\"Police sessions\"");

                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment;filename=SesionLengthReport.csv");
                        Response.ContentType = "text/csv";

                        foreach (var sessionLength in lstSessionLengthModel)
                        {
                            sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                       sessionLength.StartDate.ToString("dd/MM/yyyy"),
                                                       sessionLength.PoliceSessionCount));
                        }
                    }
                    else if (userType == "HAULIER")
                    {
                        sw.WriteLine("\"Day\",\"Haulier sessions\"");

                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment;filename=SesionLengthReport.csv");
                        Response.ContentType = "text/csv";

                        foreach (var sessionLength in lstSessionLengthModel)
                        {
                            sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                       sessionLength.StartDate.ToString("dd/MM/yyyy"),
                                                       sessionLength.HaulierSessionCount));
                        }
                    }
                    else
                    {
                        sw.WriteLine("\"Day\",\"Police sessions\",\"SOA sessions\",\"Haulier sessions\"");

                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment;filename=SesionLengthReport.csv");
                        Response.ContentType = "text/csv";

                        foreach (var sessionLength in lstSessionLengthModel)
                        {
                            sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                                                       sessionLength.StartDate.ToString("dd/MM/yyyy"),
                                                       sessionLength.PoliceSessionCount,
                                                       sessionLength.SOASessionCount,
                                                       sessionLength.HaulierSessionCount));
                        }
                    }
                    Response.Write(sw.ToString());

                    Response.End();

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/SessionLengthExportToCSV, Exception: {0}", ex));
            }
        }
        #endregion

        #region PeriodicSessionLengthDetails
        /// <summary>
        ///  Get Periodic Session Length history
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="userTypeId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public ActionResult PeriodicSessionLengthDetails(int? page, int? pageSize, int month, int year, int userTypeId, string userType)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("15001"))
                {
                    return RedirectToAction("Error", "Home");
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

                    ViewBag.Month = month;
                    ViewBag.Year = year;
                    ViewBag.UserTypeId = userTypeId;

                    ViewBag.UserType = userType;

                    #endregion

                    
                    List<SessionLengthModel> lstSessionLengthModel = reportService.GetPeriodicSessionLengthHistory(pageNumber, (int)pageSize, month, year, userTypeId);

                    if (lstSessionLengthModel.Count > 0)
                    {
                        ViewBag.TotalCount = Convert.ToInt32(lstSessionLengthModel[0].TotalRecordCount); 
                      
                    }
                    else
                    {
                        ViewBag.TotalCount = 0; 
                    }

                    var sessionLengthIPagedList = new StaticPagedList<SessionLengthModel>(lstSessionLengthModel, pageNumber, (int)pageSize, ViewBag.TotalCount);

                    return PartialView("PartialView/_PeriodicSessionLengthDetails", sessionLengthIPagedList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/PeriodicSessionLengthDetails, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/PeriodicSessionLengthDetails, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region PeriodicSessionLengthExportToCSV
        /// <summary>
        /// export report data to csv
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="userTypeId"></param>
        public void PeriodicSessionLengthExportToCSV(int month, int year, int userTypeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                    ViewBag.UserTypeId = userTypeId;


                    List<SessionLengthModel> lstSessionLengthModel = reportService.GetAllPeriodicSessionLengthHistory(month, year, userTypeId);

                    // Code changes start for showing "No of peak sessions in day"
                    List<SessionLengthModel> listPeakHrSessionMonth = reportService.GetPeakHourSessionInMonthOrganizationWise(month, year);


                    if ((lstSessionLengthModel != null) && (listPeakHrSessionMonth != null))
                    {
                        for (var sessCount = 0; sessCount < lstSessionLengthModel.Count; sessCount++)
                        {
                            var PeakSessionList = (from x in listPeakHrSessionMonth where x.UserTypeID == userTypeId && x.Organisation == lstSessionLengthModel[sessCount].Organisation orderby x.NoofSessionInPeakDay descending select x).ToList();
                            if ((PeakSessionList != null) && (PeakSessionList.Count > 0))
                            {
                                var PeakSessionDetails = PeakSessionList.FirstOrDefault();
                                lstSessionLengthModel[sessCount].NoofSessionInPeakDay = (PeakSessionDetails != null) ? PeakSessionDetails.NoofSessionInPeakDay : 0;
                            }
                        }
                    }
                    // Code End

                    StringWriter sw = new StringWriter();

                    sw.WriteLine("\"Organisation\",\"Total sessions in month\",\"No of peak sessions in day\"");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=PeriodicSesionLengthReport.csv");
                    Response.ContentType = "text/csv";

                    foreach (var sessionLength in lstSessionLengthModel)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"",
                                                   sessionLength.Organisation,
                                                   sessionLength.TotalSessionInMonth,
                                                   sessionLength.NoofSessionInPeakDay));
                    }

                    Response.Write(sw.ToString());

                    Response.End();

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/PeriodicSessionLengthExportToCSV, Exception: {0}", ex));
            }
        }
        #endregion

        #region SaveSessionLengthLog
        /// <summary>
        /// Save session length logs
        /// </summary>
        /// <param name="sessionLengthModel">SessionLengthModel</param>
        /// <returns>return true or false</returns>
        [HttpPost]
        public JsonResult SaveSessionLengthLog(SessionLengthModel sessionLengthModel)
        {
            #region Session Check
            if (Session["UserInfo"] == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                int result = reportService.SaveSessionLengthLog(sessionLengthModel);
                if (result > 0)
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            #endregion


        }
        #endregion

        #region CommunicationReport
        public ActionResult CommunicationReport()
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("15002"))
                {
                    return RedirectToAction("Error", "Home");
                }

                return View("CommunicationReport");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/CommunicationReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region CommunicationHistory
        public ActionResult CommunicationHistory(int startMonth, int startYear)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("15002"))
                {
                    return RedirectToAction("Error", "Home");
                }

                if (ModelState.IsValid)
                {
                    ViewBag.StartMonth = startMonth;
                    ViewBag.StartYear = startYear;

                    List<CommunicationModel> lstCommunicationModel = reportService.GetCommunicationHistory(startMonth, startYear);

                    ViewBag.TotalCount = lstCommunicationModel.Count;

                    return PartialView("PartialView/_CommunicationHistory", lstCommunicationModel);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/CommunicationHistory, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/CommunicationHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region CommunicationExportToCSV
        public void CommunicationExportToCSV(int startMonth, int startYear)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    ViewBag.StartMonth = startMonth;
                    ViewBag.StartYear = startYear;

                    List<CommunicationModel> lstCommunicationModel = reportService.GetCommunicationHistory(startMonth, startYear);

                    StringWriter sw = new StringWriter();

                    sw.WriteLine("\"Day\",\"Email sent\",\"Fax sent\",\"Inbox sent\"");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=CommunicationReport.csv");
                    Response.ContentType = "text/csv";

                    decimal emailSent = 0;
                    decimal faxSent = 0;
                    decimal inboxSent = 0;
                    bool recordStatus = false;
                    foreach (var communication in lstCommunicationModel)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                                                   communication.StartDate.ToString("dd/MM/yyyy"),
                                                   communication.EmailSent,
                                                   communication.FaxSent,
                                                   communication.InboxSent));

                        emailSent = emailSent + communication.EmailSent;
                        faxSent = faxSent + communication.FaxSent;
                        inboxSent = inboxSent + communication.InboxSent;
                        recordStatus = true;
                    }
                    if (recordStatus)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                                                      "Total",
                                                      emailSent,
                                                      faxSent,
                                                      inboxSent));
                    }

                    Response.Write(sw.ToString());

                    Response.End();

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/CommunicationExportToCSV, Exception: {0}", ex));
            }
        }
        #endregion

        #region IndustryLiaisonReport
        public ActionResult IndustryLiaisonReport()
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                return View("IndustryLiaisonReport");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/IndustryLiaisonReport, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region IndustryLiaisonHistory
        public ActionResult IndustryLiaisonHistory(int month, int year)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("15003"))
                {
                    return RedirectToAction("Error", "Home");
                }

                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;

                    IndustryLiaisonModel industryLiaisonModel = reportService.GetIndustryLiaisonHistory(month, year);

                    return PartialView("PartialView/_IndustryLiaisonHistory", industryLiaisonModel);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/IndustryLiaisonHistory, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/IndustryLiaisonHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region IndustryLiaisonExportToCSV
        public void IndustryLiaisonExportToCSV(int startMonth, int startYear)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    IndustryLiaisonModel industryLiaisonModel = reportService.GetIndustryLiaisonHistory(startMonth, startYear);

                    StringWriter sw = new StringWriter();

                    sw.WriteLine("\" \",\" \"");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=IndustryLiaisonReport.csv");
                    Response.ContentType = "text/csv";

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  "Haulier",
                                                  ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                   " No. registered enabled haulier accounts",
                                                   industryLiaisonModel.NoofRegEnabledHauAcc));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  " No. registered haulier organisations",
                                                  industryLiaisonModel.NoofRegHaulierOrganisation));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " No. haulier sessions",
                                                  industryLiaisonModel.NoofHaulierSession));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " Number of planned routes (so)",
                                                  industryLiaisonModel.NoofPlanRouteSO));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                     " Number of planned routes (vr1)",
                                                      industryLiaisonModel.NoofPlanRouteVR1));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                     " Number of planned routes (notification)",
                                                      industryLiaisonModel.NoofPlanRouteNotification));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                               "",
                                                ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "Police ALO",
                                                 ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  " No. registered enabled police/alo accounts",
                                                  industryLiaisonModel.NoofRegEnabledPoliceAcc));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  " No. registered police organisations",
                                                  industryLiaisonModel.NoofRegPoliceOrg));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  " No Police with preference set to online inbox only",
                                                  industryLiaisonModel.NoPolicePrefInboxOnly));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  " No. police alo sessions",
                                                  industryLiaisonModel.NoofPoliceSession));


                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                              "",
                                               ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "SOA",
                                                 ""));


                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " No. registered soa accounts",
                                                 industryLiaisonModel.NoofRegSOAAcc));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " No. registered soa organisations",
                                                  industryLiaisonModel.NoofRegSOAOrg));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " No soa with preference set to online inbox only",
                                                  industryLiaisonModel.NoSOAPrefInboxOnly));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " No. soa sessions",
                                                 industryLiaisonModel.NoofSOASession));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                              "",
                                               ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "Movements",
                                                 ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " No. of applications to SORT submitted (number of unique applications, so irrespective of versions and revisions )",
                                                  ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "  Entered by haulier online into esdal2 (so)",
                                                 industryLiaisonModel.SOAppSubEnterByHauForSO));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "  Entered by haulier online into esdal2 (vr1)",
                                                 industryLiaisonModel.SOAppSubEnterByHauForVR1));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  "  Entered by sort on behalf of the haulier (so)",
                                                  industryLiaisonModel.SOAppSubEnterBySortForSO));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  "  Entered by sort on behalf of the haulier (vr1)",
                                                  industryLiaisonModel.SOAppSubEnterBySortForVR1));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " No. special Orders processed (counted when the agreed/decline/withdraw date falls inside the reporting period)",
                                                 ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  "  Agreed",
                                                  industryLiaisonModel.SOProcssedAgreed));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  "  Declined",
                                                  industryLiaisonModel.SOProcssedDeclined));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                  "  Withdrawn",
                                                  industryLiaisonModel.SOProcssedWithdrawn));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                               " No. of proposed routes and agreed routes sent by sort (so excludes the notifications belonging to a so)",
                                                ""));


                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                "  E-mail",
                                                industryLiaisonModel.NoPropoAgreedSentEmail));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                "  Fax",
                                                 industryLiaisonModel.NoPropoAgreedSentFax));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                "  Inbox",
                                                industryLiaisonModel.NoPropoAgreedSentInbox));


                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                              "",
                                               ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                " Total number of notifications (including re-notifications) related to:",
                                                 ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                   "  Special Orders",
                                                   industryLiaisonModel.SpecialOrders));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "  All STGO notified",
                                                 ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "     STGO CAT 1",
                                                 industryLiaisonModel.STGOCategory1));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "     STGO CAT 2",
                                                 industryLiaisonModel.STGOCategory2));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "     STGO CAT 3",
                                                 industryLiaisonModel.STGOCategory3));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "     Mobile Crane Cat A",
                                                 industryLiaisonModel.MobileCraneCategoryA1));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "     Mobile Crane Cat B",
                                                 industryLiaisonModel.MobileCraneCategoryA2));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "     Mobile Crane Cat C",
                                                 industryLiaisonModel.MobileCraneCategoryA3));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "     STGO Other (like Engineering Plant and Road Recovery Operation)",
                                                 industryLiaisonModel.STGOOtherDetails));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "  C&U",
                                                 industryLiaisonModel.CAndUNotification));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 "  Other (VSO, Tracked, Wheeled construction and use)",
                                                 industryLiaisonModel.OtherNotification));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                                                 " Number of replies to Notifications (= collaboration replies)",
                                                 ""));

                    sw.WriteLine(string.Format("\"{0}\"",
                                                 "      By SOA, plus the total number of SOA notifications (so display " + industryLiaisonModel.TotalNumberOfSOANotificationToShow + " of " + industryLiaisonModel.TotalNumberOfSOANotificationToDisplay + ")"));

                    sw.WriteLine(string.Format("\"{0}\"",
                                                 "      By Police, plus the total number of Police notifications (so display " + industryLiaisonModel.TotalNumberOfPoliceNotificationToShow + " of " + industryLiaisonModel.TotalNumberOfPoliceNotificationToDisplay + ")"));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                             " Number of notifications opened", ""));

                    sw.WriteLine(string.Format("\"{0}\"",
                                                 "      By SOA, plus the total number of SOA notifications (so display " + industryLiaisonModel.TotalNumberOfSOANotificationOpenToShow + " of " + industryLiaisonModel.TotalNumberOfSOANotificationOpenToDisplay + ")"));

                    sw.WriteLine(string.Format("\"{0}\"",
                                                 "      By Police, plus the total number of Police notifications (so display " + industryLiaisonModel.TotalNumberOfPoliceNotificationOpenToShow + " of " + industryLiaisonModel.TotalNumberOfPoliceNotificationOpenToDisplay + ")"));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                             " Total number of notifications sent including re-notifications", ""));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                             "  E-mail", industryLiaisonModel.EmailNotification));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                             "  Fax", industryLiaisonModel.FaxNotification));

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"",
                             "  Inbox", industryLiaisonModel.InBoxNotification));

                    Response.Write(sw.ToString());

                    Response.End();

                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/IndustryLiaisonExportToCSV, Exception: {0}", ex));
            }
        }
        #endregion

        #region ReportPerUser
        public ActionResult ReportPerUser()
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                return View("ReportPerUser");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/ReportPerUser, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ReportPerUserHistory
        public ActionResult ReportPerUserHistory(int month, int year, int userType)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (!PageAccess.GetPageAccess("15003"))
                {
                    return RedirectToAction("Error", "Home");
                }

                if (ModelState.IsValid)
                {
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                    ViewBag.UserType = userType;

                    List<ReportPerUserModel> ReportPerUserList = reportService.GetReportPerUserHistory(month, year, userType);

                    return PartialView("PartialView/__ReportPerUserHistory", ReportPerUserList);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/ReportPerUserHistory, Exception: {0}", "Invalid Model State"));
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/ReportPerUserHistory, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region ReportPerUserExportToCSV
        public ActionResult ReportPerUserExportToCSV(int startMonth, int startYear, int userType)
        {
            try
            {
                string fileName = "ReportPerUser.csv";
                List<ReportPerUserModel> ReportPerUserList = reportService.GetReportPerUserHistory(startMonth, startYear, userType);

                StringBuilder sb = new StringBuilder();

                sb.Append("Organization" + ",");
                sb.Append("Total Sessions" + ",");
                if (userType == 696001)
                {
                    sb.Append("Total Notification Sent" + ",");
                    fileName = "ReportPerHaulier.csv";
                }
                else if (userType == 696002)
                {
                    sb.Append("Total Notification Receive" + ",");
                    fileName = "ReportPerPolice.csv";
                }
                else
                {
                    sb.Append("Total Notification Receive" + ",");
                    fileName = "ReportPerSOA.csv";
                }
                sb.Append("\r\n");

                for (int count = 0; count < ReportPerUserList.Count; count++)
                {
                    sb.Append(ReportPerUserList[count].OrganisationName.Replace(',', ' ') + ",");
                    sb.Append(ReportPerUserList[count].TotalSession + ",");
                    sb.Append(ReportPerUserList[count].TotalNotification + ",");
                    sb.Append("\r\n");
                }

                string csvContent = sb.ToString();
                return File(new System.Text.UTF8Encoding().GetBytes(csvContent), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Report/ReportPerUserExportToCSV, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion
    }
}
 