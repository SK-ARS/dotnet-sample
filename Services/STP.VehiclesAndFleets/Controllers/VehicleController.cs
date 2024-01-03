using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.VehiclesAndFleets.Vehicles;
using STP.Domain.VehiclesAndFleets.External;
using STP.ServiceAccess.SecurityAndUsers;
using STP.VehiclesAndFleets.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace STP.VehiclesAndFleets.Controllers
{
    public class VehicleController : ApiController
    {
        private static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];
        private readonly IAuthenticationService authenticationService;
        public VehicleController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpGet]
        [Route("Vehicle/ListVehicles")]
        public IHttpActionResult GetVehicleList(string AuthenticationKey = null, int PageNumber = 0, int PageSize = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                Domain.ExternalAPI.ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey);

                if (authentication.OrganisationId != 0)
                {
                    PageNumber = PageNumber > 0 ? PageNumber : 1;
                    PageSize = PageSize > 0 ? PageSize : 20;

                    VehicleListDetails vehicleListDetails = VehicleExport.Instance.GetVehicleList(authentication.OrganisationId, PageNumber, PageSize);

                    var obj = vehicleListDetails.Vehicles.OrderBy(s => s.Name).ToList();
                    vehicleListDetails.Vehicles = obj;
                    if (vehicleListDetails.Vehicles.Count == 0)
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                    else
                        return Content(HttpStatusCode.OK, vehicleListDetails);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, StatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/GetVehicleList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Vehicle/GetSORTMovVehicle")]
        public IHttpActionResult GetSORTMovVehicle(int partID, string userSchema = UserSchema.Sort)
        {
            try
            {
                List<STP.Domain.VehiclesAndFleets.Configuration.VehicleDetails> vehicleList = ApplicationVehicleProvider.Instance.GetSORTMovVehicle(partID, userSchema);
                return Ok(vehicleList);

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/GetVehicleList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        
        #region DeleteVehicle
        /// <summary>
        ///  Vehicle delete
        /// </summary>
        /// <param name="VehicleId"></param>
        [HttpDelete]
        [Route("Vehicle/DeleteVehicle")]
        public IHttpActionResult DeleteVehicle(int VehicleId = 0, string AuthenticationKey = null)
        {
            if (VehicleId == 0)
                return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

            if (string.IsNullOrWhiteSpace(AuthenticationKey))
                return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

            try
            {
                Domain.ExternalAPI.ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey);

                if (authentication.OrganisationId != 0)
                {
                    int isExist = VehicleExport.Instance.CheckVehicleExists(VehicleId, authentication.OrganisationId);
                    if (isExist > 0)
                    {
                        int count = VehicleExport.Instance.DeleteVehicle(VehicleId);
                        if (count > 0)
                            return Content(HttpStatusCode.OK, ExternalApiStatusMessage.VehicleDelete);
                        else
                            return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
                    }
                    else
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, StatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/DeleteVehicle, Exception: " + ex​​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion        
    }
}