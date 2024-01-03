using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.ExternalAPI;
using STP.ServiceAccess.SecurityAndUsers;
using STP.VehiclesAndFleets.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace STP.VehiclesAndFleets.Controllers
{
    public class VehicleExportController : ApiController
    {
        private static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];
        private readonly IAuthenticationService authenticationService;
        public VehicleExportController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("VehicleExport/ExportVehicleList")]
        public IHttpActionResult ExportVehicleList(GetVehicleExportList vehicleExportList)
        {
            try
            {
                var result = VehicleExport.Instance.ExportVehicleList(vehicleExportList);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleExport/ExportVehicleList, Exception: " + ex​​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("VehicleExport/ExportFleetVehicle")]
        public IHttpActionResult ExportFleetVehicle(int VehicleId = 0, string AuthenticationKey = null)
        {
            if (VehicleId == 0)
                return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

            if (string.IsNullOrWhiteSpace(AuthenticationKey))
                return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

            try
            {
                ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey);

                if (authentication.OrganisationId != 0)
                {
                    int isExist = VehicleExport.Instance.CheckVehicleExists(VehicleId, authentication.OrganisationId);
                    if (isExist > 0)
                    {
                        VehicleExportExternal vehicle = VehicleExport.Instance.GetFleetVehicle(VehicleId);
                        return Content(HttpStatusCode.OK, vehicle);
                    }
                    else
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, StatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleExport/ExportFleetVehicle, Exception: " + ex​​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
    }
}
