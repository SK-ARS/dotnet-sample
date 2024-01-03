using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Controllers
{
    public class BaseController : Controller
    {
        private UserInfo Session_Info = null;

       

        public UserInfo GetSessionUserInfo()
        {

            if (HttpContext != null && HttpContext.Session != null
                && HttpContext.Session["UserInfo"] != null)
            {
                Session_Info = (UserInfo)HttpContext.Session["UserInfo"];
            }
            return Session_Info;

        }

    }
}