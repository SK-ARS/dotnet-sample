using System;
using System.Web.Mvc;
using STP.Domain.RoutePlannerInterface;
using STP.Domain.SecurityAndUsers;
using STP.ServiceAccess.RoutePlannerInterface;

namespace STP.Business.Controllers
{
    public class RoutePlannerInterfaceController : Controller
    {
        private readonly IRoutePlannerInterfaceService routePlannerInterfaceSevice;
        public RoutePlannerInterfaceController(IRoutePlannerInterfaceService routePlannerSevice)
        {
            this.routePlannerInterfaceSevice = routePlannerSevice;
        }
        bool ismapsessioncheckflag = true;
        bool isrouteplansessioncheckflag = true;
        bool isroutedisplaysessioncheckflag = true;
        private void setMapUsage(int type)
        {
            UserInfo SessionInfo;
            if (Session["UserInfo"] != null)
            {
                SessionInfo = (UserInfo)Session["UserInfo"];
            }
            else
            {
                return;
            }
            int organisationID = (int)SessionInfo.organisationId;
            int userID = Convert.ToInt32(SessionInfo.UserId);
            if (Session["routedisplayCheckflag"] != null)
            {
                isroutedisplaysessioncheckflag = (bool)Session["routedisplayCheckflag"];
            }
            if (Session["maploadCheckflag"] != null)
            {
                ismapsessioncheckflag = (bool)Session["maploadCheckflag"];
            }
            if (Session["routeplanCheckflag"] != null)
            {
                isrouteplansessioncheckflag = (bool)Session["routeplanCheckflag"];
            }
            if (type == 0 && !ismapsessioncheckflag)
            {
                Session["maploadCheckflag"] = true;
            }
            else if (type == 1 && !isrouteplansessioncheckflag && !isroutedisplaysessioncheckflag)
            {
                Session["routeplanCheckflag"] = true;
            }
           // _ = RouteManagerProvider.Instance.SaveMapUsage(userID, organisationID, type);
        }

        #region Not used
        //[System.Web.Mvc.HttpPost]
        //public JsonResult Post(SocketCommunication.Models.RoutePlanner.RouteViaWaypoint routeViaPoint)
        //{
        //    setMapUsage(1);
        //    SocketCommunication.Socket.RoutePlanner.RoutePlannerConnect rpConnect = new SocketCommunication.Socket.RoutePlanner.RoutePlannerConnect();
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("Get Route using via point: StartPoint: {0} EndPoint: {1}",
        //    routeViaPoint.StartPoint, routeViaPoint.EndPoint));
        //    RouteData data = rpConnect.GetRoute(routeViaPoint);
        //    return Json(data);
        //}
        #endregion

        [HttpPost]
        public JsonResult PostEx(RouteViaWaypointEx routeViaPointEx)
        {
            setMapUsage(1);
            RouteData data = routePlannerInterfaceSevice.GetRouteData(routeViaPointEx);
            return Json(data);
        }
        
    }
}
