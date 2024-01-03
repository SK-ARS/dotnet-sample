using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.DocumentsAndContents;

namespace STP.Web.Controllers
{
    public class ContentsController : Controller
    {
        private readonly IContentsService contentsService;
        // GET: Contents
        public ActionResult Index()
        {
            return View();
        }
        public ContentsController()
        {

        }
        public ContentsController(IContentsService contentsService)
        {
            this.contentsService = contentsService;
        }
        //adding to favourites
        public JsonResult ManageFavourites(int categoryId,int categoryType, int isFavourites)
        {
            int result = 0;
            try
            {
                result = contentsService.ManageFavourites(categoryId, categoryType, isFavourites);   
                return Json(new { success = result });               
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("[{0}] , Contents/ManageFavourites, Exception: {1}", Session.SessionID, ex.Message));
                throw ex;
            }           

        }
    }
}