using System;
using System.Collections.Generic;
using System.Web.Mvc;
using STP.Common.Logger;
using STP.Domain.Routes.QAS;
using STP.ServiceAccess.Routes;

namespace STP.Business.Controllers
{
    public class QASController : Controller
    {
        private readonly IQasService qasService;
        public QASController(IQasService qasService)
        {
            this.qasService = qasService;
        }
        [HttpPost]
        public JsonResult Search(string searchKeyword)
        {
            List<AddrDetails> addrList = new List<AddrDetails>();
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"[{Session.SessionID}] , POST,QAS/Search, keyword: {searchKeyword}");
                addrList = qasService.Search(searchKeyword);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"QAS/Search, Exception: {ex}");
            }
            return Json(addrList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAddress(string moniker)
        {
            AddrDetails addDetails = new AddrDetails();
            try
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, $"[{Session.SessionID}] , POST,QAS/GetAddress, moniker: {moniker}");
                addDetails = qasService.GetAddress(moniker);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"QAS/GetAddress, Exception: {ex}");
            }

            return Json(new {Northing= addDetails.Northing,Easting=addDetails.Easting});
        }
    }
}
