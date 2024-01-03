using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using STP.Domain.HelpdeskTools;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.HelpdeskTools;
using STP.Web.Filters;
using STP.Web.WorkflowProvider;

namespace STP.Web.Controllers
{
    public class HolidaysController : Controller
    {
        [AuthorizeUser(Roles = "16000")]
       
            private readonly IHolidaysService HolidaysService;

            public HolidaysController(IHolidaysService HolidaysService)
            {
                this.HolidaysService = HolidaysService;
            }

            // GET: Holidays
            public ActionResult Index()
            {
            return View();
            }

        #region ActionResult HolidayCalender(int flag = 0, string MonthYear = null)
        public ActionResult HolidayCalender(int? page, int? pageSize, int flag = 0, string MonthYear = null, string searchType = null, HolidaysDomain objHolidays = null, int? sortType = null, int? sortOrder = null, bool isClear = false)
        {
            UserInfo SessionInfo;
             
            SessionInfo = (UserInfo)Session["UserInfo"];

            int maxlist_item = (int)Session["PageSize"];

            if (Session["PageSize"] == null)
            {
                Session["PageSize"] = maxlist_item;
            }

            if (pageSize == null)
            {
                
                pageSize = maxlist_item;
            }
            else
            {
                Session["PageSize"] = pageSize;
            }

            ViewBag.pageSize = pageSize;
            ViewBag.page = page;

            int pageNumber = (page ?? 1);

            List<HolidaysDomain> objholiday = new List<HolidaysDomain>();
            List<HolidaysDomain> objholiday1 = new List<HolidaysDomain>();

            string HolidayDateString = "";
            string HolidayDate = "";
            if (isClear)
            {
                Session["g_HolidaySearch"] = isClear ? null : objHolidays;
                objHolidays = new HolidaysDomain();
                Session["g_SearchType"] = null;
                searchType = "0";
            }
            Session["g_HolidaySearch"] = objHolidays;
            if (Session["g_HolidaySearch"] != null)
            {
                objHolidays = (HolidaysDomain)Session["g_HolidaySearch"];
            }

            if (flag == 2 || flag == 0)
            {
                /*if (flag == 2)
                {
                    MonthYear = null;
                    Session["g_MonthYear"] = null;           
                }*/
                if (flag == 0)
                {
                    searchType = (string)Session["g_SearchType"];
                }
                if (Session["g_MonthYear"] != null)
                {
                    MonthYear = (string)Session["g_MonthYear"];

                    flag = 1;
                }
                /*if(MonthYear == null)
                {
                    string currentMonth = DateTime.Now.Month.ToString();
                    string currentYear = DateTime.Now.Year.ToString();
                    MonthYear = currentMonth + "/" + currentYear;

                    flag = 1;
                }*/
               
            }
            int presetFilter = 0;
            int SORTOrder = 1;
            SORTOrder = sortOrder != null ? (int)sortOrder : SORTOrder;
            presetFilter = sortType != null ? (int)sortType : presetFilter; 
            
            ViewBag.sortType = sortType;
            ViewBag.sortOrder = sortOrder;
            if (flag == 1)
            {
                objholiday = HolidaysService.GetAllHolidays((int)pageNumber, (int)pageSize, flag, MonthYear, objHolidays.SearchType, SORTOrder, presetFilter);
                /*if (searchType == null)
                {
                    objholiday = HolidaysService.GetAllHolidays((int)pageNumber, (int)pageSize, flag, MonthYear, "");
                    ViewBag.FirstTime = "FirstTime";
                }
                else
                {

                    objholiday = HolidaysService.GetAllHolidays((int)pageNumber, (int)pageSize, flag, MonthYear, "");
                }*/

                Session["g_MonthYear"] = MonthYear;
                Session["g_SearchType"] = objHolidays.SearchType;
            }
            else
            {

                string strMonth = DateTime.Now.Month.ToString();
                int v = Convert.ToInt32(strMonth);
                strMonth = v < 10 ? "0" + strMonth : strMonth;
                string strYear = DateTime.Now.Year.ToString();
                MonthYear = strMonth + "/" + strYear;
                objholiday = HolidaysService.GetAllHolidays((int)pageNumber, (int)pageSize, 0, MonthYear, objHolidays.SearchType, SORTOrder, presetFilter);
                if (searchType == null)
                {
                    ViewBag.FirstTime = "FirstTime";
                    objholiday = HolidaysService.GetAllHolidays((int)pageNumber, (int)pageSize, 1, MonthYear, searchType, SORTOrder, presetFilter);
                }
                else
                {
                    objholiday = HolidaysService.GetAllHolidays((int)pageNumber, (int)pageSize, 0, MonthYear, objHolidays.SearchType, SORTOrder, presetFilter);
                }

                Session["g_MonthYear"] = MonthYear;
                Session["g_SearchType"] = objHolidays.SearchType;
            }

            //int totalCount = 0;
            if (objholiday.Count() > 0)
            {
                for (int i = 0; i < objholiday.Count(); i++)
                {
                    string[] tempdate = new string[1];
                    tempdate = (objholiday[i].HolidayDate).Split(' ');
                    HolidayDateString = HolidayDateString + tempdate[0].ToString() + ",";
                }
                for (int i = 0; i < objholiday.Count(); i++)
                {
                    string[] tempdateholiday = new string[1];
                    tempdateholiday = (objholiday[i].HighlightedDate).Split(' ');
                    HolidayDate = HolidayDate + tempdateholiday[0].ToString() + ",";
                }
            }
            else
            {
                HolidayDateString = "";
                HolidayDate = "";

            }
            int totalCount = 0;
            if (objholiday != null && objholiday.Count > 0)
            {
                totalCount = objholiday[0].ListCount;
            }
            else
            {
                totalCount = 0;
            }

            ViewBag.holidaydatestring = HolidayDateString;
            ViewBag.searchType = searchType;

            ViewBag.MonthYear = MonthYear;
            ViewBag.HolidayDateArray = HolidayDate;
            var calendarAsIPagedList = new StaticPagedList<HolidaysDomain>(objholiday, (int)pageNumber, (int)pageSize, totalCount);

            //var calendarAsIPagedList = new List<HolidaysDomain>(objholiday1);

            //calendarAsIPagedList = new List<HolidaysDomain>(objholiday1);
            ViewBag.FilterFlag = false;
            if (searchType != null&& searchType!="0")
            {
                ViewBag.FilterFlag = true;
            }

            return View(calendarAsIPagedList);

        }
        #endregion

        #region JsonResult DeleteHolidayDetails(long HOLIDAY_ID, string HolidayDate)
        [HttpPost]
        public JsonResult DeleteHolidayDetails(long holidayId, string holidayDate)
        {
            HolidaysDomain holidayObj = new HolidaysDomain();

            bool result = HolidaysService.DeleteHolidayDetails(holidayId);
            //var jSonVar = new { check = result, HolidayDate = holidayDate };
            
            return Json(new { Success = true });
        }
        #endregion

        #region InsertHolidayDetails(string HolidayDate,string DESCRIPTION)
        [HttpPost]
        public JsonResult InsertHolidayDetails(string HolidayDate, string DESCRIPTION, int COUNTRY_ID)
        {
            HolidaysDomain objHolidays = new HolidaysDomain();
            UserInfo SessionInfo = null;

            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            //if (Session["g_FeedbackSearch"] != null)
            //{
            //    objHolidays = (HolidaysDomain)Session["g_FeedbackSearch"];
            //}

            int result = HolidaysService.InsertHolidayInfo(HolidayDate, DESCRIPTION, COUNTRY_ID);


            var jSonVar = new { check = result, HolidayDate = HolidayDate };

            return Json(new { result = jSonVar });

        }
        #endregion

        #region EditHolidayDetails(long HOLIDAY_ID, string HolidayDate,string DESCRIPTION)
        [HttpPost]
        public JsonResult EditHolidayDetails(long holidayId, string holidayDate, string description, int countryId)
        {
            HolidaysDomain holidayObj = new HolidaysDomain();
            UserInfo SessionInfo = null;

            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }

            //if (Session["g_FeedbackSearch"] != null)
            //{
            //    holidayObj = (HolidaysDomain)Session["g_FeedbackSearch"];
            //}
            int result = HolidaysService.EditholidayInfo(holidayId, holidayDate, description, countryId);


            var jSonVar = new { check = result, holidayDate = holidayDate };

            return Json(new { result = jSonVar });
        }
        #endregion

        #region ActionResult showCalendar()
        public ActionResult showCalendar(string holidaystring, string onchange)
        {
            ViewBag.Holidaystring = holidaystring;
            ViewBag.onchange = onchange;
            return PartialView("showCalendar");
        }
        #endregion
    }
}