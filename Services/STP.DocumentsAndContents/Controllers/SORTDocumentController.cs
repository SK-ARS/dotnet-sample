using STP.Common.Constants;
using STP.Common.Logger;
using STP.DocumentsAndContents.Common;
using STP.DocumentsAndContents.Document;
using STP.DocumentsAndContents.Providers;
using STP.Domain.Applications;
using STP.Domain.DocumentsAndContents;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace STP.DocumentsAndContents.Controllers
{
    public class SortDocumentController : ApiController
    {
        #region GetSpecialOrder
        [HttpGet]
        [Route("SORTDocument/GetSpecialOrder")]
        public IHttpActionResult GetSpecialOrder(string orderId)
        {
            SORTSpecialOrder spOrder;
            try
            {
                spOrder = SORTDocumentProvider.Instance.GetSpecialOrder(orderId);
                return Ok(spOrder);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GetSpecialOrder, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetRouteVehicles
        [HttpGet]
        [Route("SORTDocument/GetRouteVehicles")]
        public IHttpActionResult GetRouteVehicles(int movementVersionId, string vehicleId)
        {
            List<SOCRouteParts> objRouteParts;
            try
            {
                objRouteParts = SORTDocumentProvider.Instance.GetRouteVehicles(movementVersionId, vehicleId);
                return Ok(objRouteParts);
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetSpecialOrderCoverages
        [HttpGet]
        [Route("SORTDocument/GetSpecialOrderCoverages")]
        public IHttpActionResult GetSpecialOrderCoverages(int projectid, int state)
        {
            List<SOCoverageDetails> lstCoverages;
            try
            {
                lstCoverages = SORTDocumentProvider.Instance.GetSpecialOrderCoverages(projectid, state);
                return Ok(lstCoverages);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GetSpecialOrderCoverages, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region ListSortUser
        [HttpGet]
        [Route("SORTDocument/ListSortUser")]
        public IHttpActionResult ListSortUser(long userTypeId, int checkerType = 0)
        {
            List<GetSORTUserList> objSortUser;
            try
            {
                objSortUser = SORTDocumentProvider.Instance.ListSortUser(userTypeId, checkerType);
                return Ok(objSortUser);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/ListSortUser, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetSpecialOrderList
        [HttpGet]
        [Route("SORTDocument/GetSpecialOrderList")]
        public IHttpActionResult GetSpecialOrderList(long projectId)
        {
            List<SORTMovementList> objSpecialOrderList;
            try
            {
                objSpecialOrderList = SORTDocumentProvider.Instance.GetSpecialOrderList(projectId);
                return Ok(objSpecialOrderList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GetSpecialOrderList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region Deletespecialorder
        [HttpDelete]
        [Route("SORTDocument/Deletespecialorder")]
        public IHttpActionResult Deletespecialorder(string orderNo, string userSchema)
        {
            int result;
            try
            {
                result = SORTDocumentProvider.Instance.DeleteSpecialOrder(orderNo, userSchema);
                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }

                else if (result == 0)
                {
                    return Content(HttpStatusCode.OK, result);

                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/Deletespecialorder, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region SaveSortSpecialOrder
        [HttpPost]
        [Route("SORTDocument/SaveSortSpecialOrder")]
        public IHttpActionResult SaveSortSpecialOrder(SpecialOrderParams specialOrderParams)
        {
            string soNumber;
            try
            {
                soNumber = SORTDocumentProvider.Instance.SaveSortSpecialOrder(specialOrderParams.SpecialOrderModel, specialOrderParams.ListCoverages);
                return Ok(soNumber);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/SaveSortSpecialOrder, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region UpdateSortSpecialOrder
        [HttpPost]
        [Route("SORTDocument/UpdateSortSpecialOrder")]
        public IHttpActionResult UpdateSortSpecialOrder(SpecialOrderParams specialOrderParams)
        {
            string soNumber;
            try
            {
                soNumber = SORTDocumentProvider.Instance.UpdateSortSpecialOrder(specialOrderParams.SpecialOrderModel, specialOrderParams.ListCoverages);
                return Ok(soNumber);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/UpdateSortSpecialOrder, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GenrateSODocument
        [HttpPost]
        [Route("SORTDocument/GenrateSODocument")]
        public IHttpActionResult GenrateSODocument(SODocumentParams soDocumentParams)
        {
            byte[] exportByteArrayData;
            try
            {
                exportByteArrayData = SORTDocumentProvider.Instance.GenrateSODocument(soDocumentParams.TemplateType,soDocumentParams.EsdalReferenceNo,soDocumentParams.OrderNumber,soDocumentParams.UserInfo,soDocumentParams.GenerateFlag);
                return Ok(exportByteArrayData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GenrateSODocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GenerateHaulierAgreedRouteDocument

        [HttpGet]
        [Route("SORTDocument/GenerateHaulierAgreedRouteDocument")]
        public IHttpActionResult GenerateHaulierAgreedRouteDocument(string esDALRefNo = "GCS1/25/2", string order_no = "P21/2012", int contactId = 8866,int UserTypeId=0)
        {
            byte[] exportByteArrayData;
            try
            {
                UserInfo userInfo = new UserInfo();
                userInfo.UserTypeId = UserTypeId;
                exportByteArrayData = SORTDocumentProvider.Instance.GenerateHaulierAgreedRouteDocument(esDALRefNo, order_no, contactId, userInfo);
                return Ok(exportByteArrayData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GenrateSODocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region List NE Broken Route Details
        [HttpPost]
        [Route("SORTDocument/ListNEBrokenRouteDetails")]
        public IHttpActionResult ListNEBrokenRouteDetails(NotifRouteImportParams objNotifRouteImportParams)
        {
            List<NotifRouteImport> list = new List<NotifRouteImport>();
            try
            {
                list = SORTDocumentProvider.Instance.ListNEBrokenRouteDetails(objNotifRouteImportParams);
                if(list != null && list.Count > 0)
                 return Ok(list);
                else
                 return Content(HttpStatusCode.NotFound, "no data found");
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetNoofPages
        [HttpGet]
        [Route("SORTDocument/GetNoofPages")]
        public IHttpActionResult GetNoofPages(String outputString)
        {
            int result;
            try
            {
                result = SORTDocumentProvider.Instance.GetNoofPages(outputString);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GetNoofPages, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GenerateFormVR1Document
        [HttpGet]
        [Route("SORTDocument/GenerateFormVR1Document")]
        public IHttpActionResult GenerateFormVR1Document(string haulierMnemonic, string esdalRefNumber, int Version_No, bool generateFlag = true)
        {
            byte[] result;
            try
            {
                GenerateDocument generateDocument = new GenerateDocument();
                result = generateDocument.GenerateFormVR1Document(haulierMnemonic, esdalRefNumber, Version_No, generateFlag);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GenerateFormVR1Document, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region GetNotifICAstatus

        [HttpPost]
        [Route("SORTDocument/GetNotifICAstatus")]
        public IHttpActionResult GetNotifICAstatus(NotificationICAstatusParans notificationICAstatus)
        {
            Dictionary<int, int> result;
            try
            {
                CommonNotifMethods commonNotif = new CommonNotifMethods();

                result = commonNotif.GetNotifICAstatus(notificationICAstatus.XmlaffectedStructures);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GetNotifICAstatus, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion

        #region Commented Code By Mahzeer On 13/07/2023
        /*[HttpPost]
        [Route("SORTDocument/GenerateSOProposalDocument")]
        public IHttpActionResult GenerateSOProposalDocument(SOProposalDocumentParams documentParams)

        {
            int result;
            try
            {
                GenerateDocument generateDocument = new GenerateDocument();
                result = generateDocument.GenerateSOProposalDocument(documentParams.EsdalReferenceNo, documentParams.OrganisationId, documentParams.ContactId, documentParams.DistributionComments, documentParams.VersionId, documentParams.ICAStatusDictionary, documentParams.Esdalreference, documentParams.HaContactDetail, documentParams.Agreedroute, documentParams.UserSchema, documentParams.RoutePlanUnits, documentParams.ProjectStatus, documentParams.VersionNo, documentParams.Moveprint, documentParams.PreVersionDistr, documentParams.SessionInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GenerateSOProposalDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }*/
        #endregion

        #region GenerateAmendmentDocument
        [HttpGet]
        [Route("SORTDocument/GenerateAmendmentDocument")]
        public IHttpActionResult GenerateAmendmentDocument(string SOnumber, Enums.DocumentType doctype, int organisationId, bool generateFlag)

        {
            byte[] result = null;
            try
            {
                GenerateDocument generateDocument = new GenerateDocument();
                result = generateDocument.GenerateAmendmentDocument(SOnumber, doctype, organisationId, generateFlag);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - SORTDocument/GenerateAmendmentDocument, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        #endregion
    }
}
