using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Domain;
using STP.DocumentsAndContents.Providers;
using STP.DocumentsAndContents.Persistance;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets;
using STP.DocumentsAndContents.Common;
using NotificationXSD;

namespace STP.DocumentsAndContents.Controllers
{
    public class ContentsController : ApiController
    {
       
        
        #region Contactlist
        [HttpGet]
        [Route("Contents/ContactList")]
        public IHttpActionResult ContactList(int organizationId, int pageNumber, int pageSize, int searchCriteria, string searchValue, int sortFlag, int presetFilter, int? sortOrder)
        {
            try
            {
                List<ContactListModel> objContactList = ContactProvider.Instance.GetContactListSearch(organizationId, pageNumber, pageSize, searchCriteria, searchValue, sortFlag,presetFilter,sortOrder);

                return Ok(objContactList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/ContactList Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetHaulierAddressBook
        [HttpGet]
        [Route("Contents/AddressList")]
        public IHttpActionResult AddressList(int organizationId, int pageNumber, int pageSize, string searchCriteria, string searchValue, int presetFilter, int? sortOrder)
        {
            try
            {
                List<HaulierContactModel> objAddressList = HaulierAddressProvider.Instance.GetHaulierContactList(organizationId, pageNumber, pageSize, searchCriteria, searchValue, presetFilter, sortOrder);
                return Ok(objAddressList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/AddressList Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion
        #region InsertFeedback
        [HttpPost]
        [Route("Contents/InsertFeedbackDetails")]
        public IHttpActionResult InsertFeedbackDetails(InsertFeedbackDomain insertFeedbackDomain)
        {

            try
            {
                int rowsAffected = FeedbackProvider.Instance.InsertFeedbackDetails(insertFeedbackDomain);
                if (rowsAffected < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InsertionFailed);
                }
                else if (rowsAffected == 0)
                {
                    return Content(HttpStatusCode.OK, rowsAffected);
                }
                else
                {
                    return Content(HttpStatusCode.Created, rowsAffected);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"InsertFeedbackDetails Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetHaulierContactById
        [HttpGet]
        [Route("Contents/GetHaulierContactById")]
        public IHttpActionResult GetHaulierContactById(double haulierContactId)
        {
            try
            {
                HaulierContactModel objHaulierContactModel = HaulierAddressProvider.Instance.GetHaulierContactById(haulierContactId);
                return Ok(objHaulierContactModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetHaulierContactById Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Save Haulier Contact
        [HttpPost]
        [Route("Contents/ManageHaulierContact")]
        public IHttpActionResult ManageHaulierContact(HaulierContactModel haulierContactModel)
        {
            
            try
            {
              bool result = HaulierAddressProvider.Instance.ManageHaulierContact(haulierContactModel);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/ManageHaulierContact Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeleteHaulierContact
        [HttpDelete]
        [Route("Contents/DeleteHaulierContact")]
        public IHttpActionResult DeleteHaulierContact(int haulierContactId)
        {
            try
            {
                int affectedRows = HaulierAddressProvider.Instance.DeleteHaulierContact(haulierContactId);
                if (affectedRows > 0)
                {
                    return Content(HttpStatusCode.OK, affectedRows);
                }
                else if(affectedRows==0)
                {
                    return Content(HttpStatusCode.OK, affectedRows);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/DeleteHaulierContact Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetVehicleComponentAxles
        [HttpGet]
        [Route("Contents/GetVehicleComponentAxles")]
        public IHttpActionResult GetVehicleComponentAxles(int notificationId, long vehicleId)
        {
            try
            {
                List<VehComponentAxles> objVehComponentAxles = DocumentTransmission.Instance.GetVehicleComponentAxles(notificationId, vehicleId);
                return Ok(objVehComponentAxles);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetVehicleComponentAxles Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAxleWeightListPositions
        [HttpGet]
        [Route("Contents/GetAxleWeightListPositions")]
        public IHttpActionResult GetAxleWeightListPositions(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            try
            {
                NotificationXSD.SummaryAxleStructureAxleWeightListPosition[] sasawlpList = CommonMethods.GetAxleWeightListPosition(vehicleComponentAxlesList);
                return Ok(sasawlpList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleWeightListPositions Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAxleSpacingToFollowListPositionAxleSpacing
        [HttpGet]
        [Route("Contents/GetAxleSpacingToFollowListPositionAxleSpacings")]
        public IHttpActionResult GetAxleSpacingToFollowListPositionAxleSpacings(AxleFollowParams axleFollowParams)
        {
            try
            {
                SummaryAxleStructureAxleSpacingToFollowListPosition[] summaryAxleStructureAxleSpacingListPositions = CommonMethods.GetAxleSpacingToFollowListPositionAxleSpacing(axleFollowParams.vehicleComponentAxlesList, axleFollowParams.firstComponentAxleSpaceToFollow);
                return Ok(summaryAxleStructureAxleSpacingListPositions);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleSpacingToFollowListPositionAxleSpacings Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAxleSpacingListPositionAxleSpacings
        [HttpPost]
        [Route("Contents/GetAxleSpacingListPositionAxleSpacings")]
        public IHttpActionResult GetAxleSpacingListPositionAxleSpacing(List<VehComponentAxles> vehicleComponentAxlesList)
        {
            try
            {
                NotificationXSD.SummaryAxleStructureAxleSpacingListPosition[] summaryAxleStructureAxleSpacingListPositions = CommonMethods.GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
                return Ok(summaryAxleStructureAxleSpacingListPositions);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/GetAxleSpacingListPositionAxleSpacings Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Manage Favourites
        [HttpGet]
        [Route("Contents/ManageFavourites")]
        public IHttpActionResult ManageFavourites(int categoryId, int categoryType, int isFavourite)
        {
            try
            {
                int result = InformationProvider.Instance.ManageFavourites(categoryId, categoryType, isFavourite);
                if (result >= 0)
                {
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError,StatusMessage.UpdationFailed);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Contents/ManageFavouritesHaulierVehicleRoute Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

    }
}
