#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using STP.Domain;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using STP.Common.Logger;
using STP.Business.Filters;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.Domain.SecurityAndUsers;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;

#endregion

namespace STP.Web.Controllers
{
    public class GenericController : Controller
    {
        private readonly IMovementsService movementsService;
        private readonly IGenericService genericService;
        private readonly IInformationService informationService;
        public GenericController()
        {
        }

        public GenericController(IMovementsService movementsService, IGenericService genericService, IInformationService informationService)
        {
            this.movementsService = movementsService;
            this.genericService = genericService;
            this.informationService = informationService;
        }

        #region Public Methods

        public ActionResult MenuItems()
        {
            
            MenuAccess menuAccess = new MenuAccess();
            UserInfo SessionInfo = null;
            SessionInfo = (UserInfo)Session["UserInfo"];
            if (SessionInfo.HelpdeskRedirect == "true")
            {
                ViewBag.Helpdest_redirect = SessionInfo.HelpdeskRedirect;
            }

            if(Session["MenuAccess"]!=null)
                menuAccess = (MenuAccess)Session["MenuAccess"];
            
            return PartialView("MenuItems",menuAccess);
        }

        public ActionResult Filters()
        {
            return PartialView("Filters");
        }

        //method for uploading image
        [HttpPost]
        public ActionResult Uploadimage(HttpPostedFileBase file)
        {
            var array = new Dictionary<string, string>();
            //check the image is there or not
            if (file != null && file.ContentLength > 0)
            {
                string fileName = file.FileName;

                fileName = fileName.Split('\\').Last();

                string ImageName = System.IO.Path.GetFileName(fileName);
                string filePath = System.Configuration.ConfigurationManager.AppSettings["HtmlHelperImages"];
                //string filePath = Server.MapPath("~/Content/Help/Images");

                //if there is no such a directory we create one 
                bool isExists = System.IO.Directory.Exists(filePath);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(filePath);


                string filefullPath = Path.Combine(filePath, ImageName);
                // save image in folder
                file.SaveAs(filefullPath);

                //array["filelink"] = "/Content/Help/Images/"+fileName;
                array["filelink"] = "/HtmlHelper/Images/" + fileName;
                array["filename"] = fileName;
            }
            
            return Json(array,JsonRequestBehavior.AllowGet);
        }

                
        //method for creating htmlfile
        [HttpPost]
        public ActionResult CreateHtml(FileDetails fileDetails)
        {
            //string filePath = Server.MapPath("~/Content/Help");
            string filePath = System.Configuration.ConfigurationManager.AppSettings["HtmlHelperContent"];
            
            //if there is no such a directory we create one 
            bool isExists = System.IO.Directory.Exists(filePath);
            if (!isExists)
                System.IO.Directory.CreateDirectory(filePath);


            
            var fileText = string.Empty;
            if (fileDetails.Flag == 0)
            {
                //fileText = " <div class='Help_form form_component' id='div_help_builder'><div class='head headgrad head_component' ><div class='dyntitle'>Help</div><span aria-hidden='true' data-icon='&#xe0f3;' id='span-close1' onclick='closehelp()'></span></div><div class='Help_body'>" + fileDetails.FileContent + "</div></div>";
                fileText = " <div class='modal-header pl-5 pr-5 pb-3 pt-6 align-items-center d-flex'><!-- <div > --><div class='col-lg-7 col-sm-7 col-md-7'><span class='text-normal pr-2' style='font-family: lato_light;font-size:25px;float: right;text-align: center;'>HELP</span></div><div class='col-lg-3 col-sm-3 col-md-3'><span  style='cursor: pointer;float: right;width:20;' id='closehelp'><img src=../Content/assets/images/modal-close-icon.svg id='scoll-right-btn' width='20'></span></div> <!-- </div>     --> </div></div><div class='Help_body' id='HelpAlli'>" + fileDetails.FileContent + "</div>";

            }
            else
            {
               // fileText = " <div class='Help_form form_component' id='div_help_builder'><div class='head headgrad head_component' ><div class='dyntitle'>Help</div><span aria-hidden='true' data-icon='&#xe0f3;' id='span-close1' onclick='closehelp_popup()'></span></div><div class='Help_body'>" + fileDetails.FileContent + "</div></div>";
                fileText = " <div class='Help_form form_component' id='div_help_builder'><div class='head headgrad head_component' ><div class='dyntitle'>Help</div><span aria-hidden='true' data-icon='&#xe0f3;' id='span-close1' id='helppopup'></span></div><div class='Help_body'>" + fileDetails.FileContent + "</div></div>";

            }
            string fileName = Path.Combine(filePath, fileDetails.FileName);
            System.IO.File.WriteAllText(fileName, fileText);
            return Json(1);
        }


        //method for fetching html file result
        [HttpPost]
        public ActionResult GetHtmlPage(string fileName,int flag)
        {
            try
            {
                //string filePath = Server.MapPath("~/HtmlHelper/Content/");
                string filePath = System.Configuration.ConfigurationManager.AppSettings["HtmlHelperContent"];
                var fullPath = string.Empty;
                var directory = new DirectoryInfo(filePath);

                //return Json(directory+"-"+fileName);
                //Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GetHtmlPage, Dir path: {0}", directory));

                var url = string.Empty;
                var test = from f in directory.GetFiles() select f;
                var urlContent = (from f in directory.GetFiles()
                                  where f.Name.Contains(fileName)
                                  orderby f.LastWriteTime descending
                                  select f).FirstOrDefault();
                if (urlContent != null)
                {
                    url = urlContent.Name;
                    fullPath = Path.Combine(filePath, url);
                }

                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                    }
                }
                string allines = sb.ToString();

                return Json(new { page = allines, role = flag });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GetHtmlPage, Exception: {0}", ex));
                return Json(new { page = "", role = flag });
               
            }
        }

        //method for showing newsbar
        public ActionResult Newsbar()
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
                /// <summary>
                /// Get hot news for portal
                /// </summary>
                /// <param name="portalid">Portal id</param>
                /// <returns>InformatinDetail</returns>
                //InformatinDetail informationDetail = InformationProvider.Instance.GetHotNewsForPortal(SessionInfo.userTypeId);

                InformatinDetail informationDetail = informationService.GetAllHotNews(SessionInfo.UserTypeId);
                ViewBag.hotNewsName = informationDetail.HotNewsName;
                ViewBag.ContentTypeId = informationDetail.HotNewsContentId;

                //ViewBag.hotnews = informationDetail.HOT_NEWS;
                //ViewBag.userTypeId = SessionInfo.userTypeId;
                return PartialView("Newsbar");
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Generic/Newsbar, Exception: {0}", ex));
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult LeftPanelBack()
        {
            return PartialView();
        }


        public string MD5Encryption(string passwordKey)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwordKey));

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
          return sb.ToString().ToLower();
        }


        #region public ActionResult FirstTimeUser()
        public ActionResult FirstTimeUser()
        {
            return PartialView();
        }
        #endregion public ActionResult FirstTimeUser()

        //method for setup html file url
        #region public ActionResult HelpBuilder(string url)
        public ActionResult HelpBuilder(string url,  int? flag)
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            ViewBag.Username = SessionInfo.UserName;
            ViewBag.PageUrl = url;
            TempData["flag"] = flag;
            ViewBag.flag = flag;
            return View();
        }
        #endregion public ActionResult HelpBuilder(string url)

        
        #region public ActionResult QuickLinks()
        public ActionResult QuickLinks()
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                List<QuickLinks> objQuickLinks = new List<QuickLinks>();
                return PartialView(objQuickLinks);
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            int userId = Convert.ToInt32(SessionInfo.UserId);
            List<QuickLinks> quickLinksObj = genericService.GetQuickLinksList(userId).Take(5).ToList();//new List<QuickLinks>();
            return PartialView(quickLinksObj);
        }
        #endregion

        #region public ActionResult QuickLinksSOA()
        public ActionResult QuickLinksSOA()
        {
            UserInfo SessionInfo = null;
            if (Session["UserInfo"] == null)
            {
                List<QuickLinksSOA> objQuickLinks = new List<QuickLinksSOA>();
                return PartialView(objQuickLinks);
            }
            SessionInfo = (UserInfo)Session["UserInfo"];
            int userId = Convert.ToInt32(SessionInfo.UserId); //20910;// 
            List<QuickLinksSOA> quickLinksObj = movementsService.GetQuickLinksSOAList(userId).Take(5).ToList();
            return PartialView(quickLinksObj);
        }
        #endregion

        #endregion

        #region Private Methods



        #endregion
    }



    


    public static class PageAccess
    {
        public static bool GetPageAccess(string menuId)
        {
            bool validUser = false;
            if (System.Web.HttpContext.Current.Session["MenuAccess"] != null)
            {
                var menuDetails = (MenuAccess)System.Web.HttpContext.Current.Session["MenuAccess"];
                var menuCount = (from s in menuDetails.MenuAccessInfo
                                 where s.MenuId == menuId
                                 select s).Count();
                if (menuCount > 0)
                {
                    validUser = true;
                }
            }
            return validUser;       
        }
    }

    public class FileDetails
    {
        [AllowHtml]
        public string FileContent { get; set; }

        public string FileName { get; set; }

        public int Flag { get; set; }
    }
}
