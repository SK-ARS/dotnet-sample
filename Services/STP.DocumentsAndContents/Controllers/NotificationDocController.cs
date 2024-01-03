using STP.Common.Logger;
using STP.DocumentsAndContents.Providers;
using STP.Domain.DocumentsAndContents;
using STP.Domain.RouteAssessment;
using STP.Domain.SecurityAndUsers;
using System;
using System.Net;
using System.Web.Http;

namespace STP.DocumentsAndContents.Controllers
{
    public class NotificationDocController : ApiController
    {
        #region Get RouteDescription for Haulier Notification document
        [HttpGet]
        [Route("NotificationDocument/GetRouteDescription")]
        public IHttpActionResult GetRouteDescription(int notificationId)
        {
            DrivingInstructionModel structuresDetail;
            try
            {
                structuresDetail = NotificationDocument.Instance.GetRouteDescription(notificationId);
                return Ok(structuresDetail);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetRouteDescription, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region Commented Code By Mahzeer on 13/07/2023
        /*
        [HttpPost]
        [Route("NotificationDocument/AddManageDocument")]
        public IHttpActionResult AddManageDocument(AddManageDocParams addManageDocParams)
        {
            long documentId = 0;
            try
            {
                documentId = NotificationDocument.Instance.AddManageDocument(addManageDocParams.OutboundDocuments, addManageDocParams.UserSchema);
                return Ok(documentId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/AddManageDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("NotificationDocument/GetVehicleUnits")]
        public IHttpActionResult GetVehicleUnits(int contactId, Int32 organisationId)
        {
            int vehicleUnits;
            try
            {
                vehicleUnits = NotificationDocument.Instance.GetVehicleUnits(contactId, organisationId);
                return Ok(vehicleUnits);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetVehicleUnits, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        [HttpPost]
        [Route("NotificationDocument/GenerateMovementAction")]
        public IHttpActionResult GenerateMovementAction(GenerateMovementParams generateMovementParams)
        {
            bool status = false;
            try
            {
                status = NotificationDocument.Instance.GenerateMovementAction(generateMovementParams.UserInfo, generateMovementParams.ESDALReference, generateMovementParams.Movactiontype, generateMovementParams.MovFlagVar, generateMovementParams.NotificationContacts);
                return Ok(status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GenerateMovementAction, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        [HttpPost]
        [Route("NotificationDocument/InsertCollaboration")]
        public IHttpActionResult InsertCollaboration(InsertCollabParams insertCollabParams)
        {
            int isUpdated;
            try
            {
                isUpdated = NotificationDocument.Instance.InsertCollaboration(insertCollabParams.OutboundDocuments, insertCollabParams.DocumentId, insertCollabParams.UserSchema, insertCollabParams.Status);
                return Ok(isUpdated);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/InsertCollaboration, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("NotificationDocument/GetStructuresXMLByESDALReference")]
        public IHttpActionResult GetStructuresXMLByESDALReference(string ESDALReference, string userSchema)
        {
            StructuresModel structureInfo;
            try
            {
                structureInfo = NotificationDocument.Instance.GetStructuresXMLByESDALReference(ESDALReference, userSchema);
                return Ok(structureInfo);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetStructuresXMLByESDALReference, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        */
        #endregion

        #region Get NENRouteDescription
        [HttpGet]
        [Route("NotificationDocument/GetNENRouteDescription")]
        public IHttpActionResult GetNENRouteDescription(long NENInboxId, int organisationId)
        {
            DrivingInstructionModel structuresDetail = new DrivingInstructionModel();
            try
            {
                structuresDetail = NotificationDocument.Instance.GetNENRouteDescription(NENInboxId, organisationId);
                return Ok(structuresDetail);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetNENRouteDescription, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetRouteAssessmentDetails
        [HttpGet]
        [Route("NotificationDocument/GetApiRouteAssessmentDetails")]
        public IHttpActionResult GetApiRouteAssessmentDetails(int notificationID, int organisationId, int isNen)
        {
            RouteAnalysisModel structuresDetail;
            try
            {
                structuresDetail = NotificationDocument.Instance.GetApiRouteAssessmentDetails(notificationID, organisationId, isNen);
                return Content(HttpStatusCode.OK, structuresDetail);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetRouteAssessmentDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetStructuresXML
        [HttpGet]
        [Route("NotificationDocument/GetStructuresXML")]
        public IHttpActionResult GetStructuresXML(int notificationId)
        {
            StructuresModel structuresDetail = new StructuresModel();
            try
            {
                structuresDetail = NotificationDocument.Instance.GetStructuresXML(notificationId);
                return Ok(structuresDetail);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetStructuresXML, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetHaulierNotificationDocument
        [HttpPost]
        [Route("NotificationDocument/GetHaulierNotificationDocument")]
        public IHttpActionResult GetHaulierNotificationDocument(HaulierNotificationParams haulierNotificationParams)
        {
            byte[] byteArrayData;
            try
            {
                byteArrayData = NotificationDocument.Instance.GenerateHaulierNotificationDocument(haulierNotificationParams.NotificationId, haulierNotificationParams.DocumentType, haulierNotificationParams.ContactId, haulierNotificationParams.SessionInfo);
                return Ok(byteArrayData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetHaulierNotificationDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region Get RouteDescription for Haulier Notification document
        [HttpGet]
        [Route("NotificationDocument/GetContactDetails")]
        public IHttpActionResult  GetContactDetails(int contactId)
        {
            ContactModel contactModel;
            try
            {
                contactModel = NotificationDocument.Instance.GetContactDetails(contactId);
                return Ok(contactModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - NotificationDocument/GetContactDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

    }
}
