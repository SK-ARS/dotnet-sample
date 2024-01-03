using PagedList;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.SecurityAndUsers;
using STP.Web.WorkflowProvider;
using System.Collections.Generic;
using System.Web.Mvc;
using static STP.Domain.Routes.RouteModel;

namespace STP.Business.Controllers
{
    public class AnnotationController : Controller
    {
        private readonly IRoutesService routesService;
        private readonly IUserService userService;

        public AnnotationController(IRoutesService routesService, IUserService userService = null)
        {
            this.routesService = routesService;
            this.userService = userService;
        }

        public ActionResult CreateAnnotation(int editmode = 0)
        {
            RouteAnnotation annot = new RouteAnnotation();
            List<SelectListItem> obj = new List<SelectListItem>();
            obj.Add(new SelectListItem { Text = "Special Manoeuvre", Value = "3" });
            obj.Add(new SelectListItem { Text = "Generic", Value = "1" });
            obj.Add(new SelectListItem { Text = "Caution", Value = "2" });
           
            ViewBag.Annottype = obj;
            ViewBag.editmode = editmode;
            return View("CreateAnnotation", annot);
        }

        public ActionResult viewAnnotation(RouteAnnotation obj)
        {
            RouteAnnotation AM = new RouteAnnotation();

            return View("Annotation",AM);
        }

        public ActionResult GetAnnotationLibraryList(int Id)
        {
            if (Session["UserInfo"] == null)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("Login", "Account")
                });

            }
            //var SessionInfo = (UserInfo)Session["UserInfo"];
            //long OrgId = SessionInfo.organisationId;


            return PartialView("PartialView/_DisplayAnnotationListPopUp");
        }


        public JsonResult SaveAnnotationToLibrary(long annotationType, string annotationText, long structureId = 0)
        {
            if (Session["UserInfo"] == null)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("Login", "Account")
                });

            }
            var SessionInfo = (STP.Domain.SecurityAndUsers.UserInfo)Session["UserInfo"];
            long organisationId = SessionInfo.OrganisationId;
            string userId = SessionInfo.UserId;

            int valueReturn = routesService.SaveAnnottationTextToLibrary(organisationId, userId, annotationType, annotationText, structureId, null);

            if (valueReturn > 0)
                return Json(new { success = true, responseText = "AnnotationText Successfully included in Library" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, responseText = "Annotation text already exists in Library" }, JsonRequestBehavior.AllowGet); ;

        }

        public ActionResult GetAnnotationsFromLibrary(int pageNumber, int pageSize, long annotationType = 0, string annotationText = "", long structureId = 0,int userId=0)
        {
            if (Session["UserInfo"] == null)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("Login", "Account")
                });

            }

            ViewBag.pageSize = pageSize;
            ViewBag.page = pageNumber;
            ViewBag.annotationType = annotationType;
            ViewBag.annotationText = annotationText;
            ViewBag.userId = userId;


            var SessionInfo = (STP.Domain.SecurityAndUsers.UserInfo)Session["UserInfo"];
            long organisationId = SessionInfo.OrganisationId;
            ViewBag.UsersByOrgId = userService.GetUsersByOrgID(organisationId, SessionInfo.UserTypeId);

            List<AnnotationTextLibrary> annotationTextLibraries = routesService.GetAnnottationTextListLibrary(pageNumber, pageSize, organisationId, userId, annotationType, annotationText, structureId, null);


            if (annotationTextLibraries.Count > 0)
            {
                List<AnnotationTextLibrary> annotationTextLibraryRow1 = new List<AnnotationTextLibrary>();
                annotationTextLibraryRow1.Add(annotationTextLibraries[0]);
                foreach (var item in annotationTextLibraryRow1)
                {
                    ViewBag.TotalCount = (int)item.totalRecords;
                }
            }
            else
            {
                ViewBag.TotalCount = 0;
            }

            var annotationTextLibararyPageList = new StaticPagedList<AnnotationTextLibrary>(annotationTextLibraries, pageNumber, (int)pageSize, ViewBag.TotalCount);
            return PartialView("PartialView/_DisplayAnnotationListPopUp", annotationTextLibararyPageList);


        }
    }
}
