using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.VehicleAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.VehiclesAndFleets.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleGlobalConfig;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;

namespace STP.VehiclesAndFleets.Controllers
{
    public class VehicleComponentController : ApiController
    {
        private static string LogInstance = ConfigurationManager.AppSettings["Instance"];
        #region Function for checking vehicle exists
        [HttpGet]
        [Route("VehicleComponent/CheckConfigNameExists")]
        public IHttpActionResult CheckConfigNameExists(string vehicleName, int organisationId)
        {
            try
            {
                int count = CreateVehicleComponent.Instance.CheckConfigNameExists(vehicleName, organisationId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CheckConfigNameExists, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Create component in fleet managemnet
        [HttpPost]
        [Route("VehicleComponent/CreateComponent")]
        public IHttpActionResult CreateComponent(ComponentModel componentModel)
        {
            try
            {
                double componentId = 0;
                componentId = CreateVehicleComponent.Instance.CreateComponent(componentModel);
                return Ok(componentId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CreateComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region InsertAppVehicleComponent implementation
        [HttpPost]
        [Route("VehicleComponent/InsertAppVehicleComponent")]
        public IHttpActionResult InsertAppVehicleComponent(UpdateVehicleComponentParams updateVehComponentParams)
        {
            double componentId = 0;
            try
            {
                componentId = CreateVehicleComponent.Instance.InsertAppVehicleComponent(updateVehComponentParams.ComponentModel, updateVehComponentParams.UserSchema);
                return Content(HttpStatusCode.Created, componentId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/InsertAppVehicleComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region InsertVR1VehicleComponent implementation
        [HttpPost]
        [Route("VehicleComponent/InsertVR1VehicleComponent")]
        public IHttpActionResult InsertVR1VehicleComponent(UpdateVehicleComponentParams updateVehComponentParams)
        {
            double componentId = 0;
            try
            {
                componentId = CreateVehicleComponent.Instance.InsertVR1VehicleComponent(updateVehComponentParams.ComponentModel, updateVehComponentParams.UserSchema);
                return Content(HttpStatusCode.Created, componentId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/InsertVR1VehicleComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region For inserting the vehicle and component into vehicle position table with lat and long position as 1
        [HttpPost]
        [Route("VehicleComponent/CreateConfPosnComponent")]
        public IHttpActionResult CreateConfPosnComponent(VehicleConfigList configList)
        {
            try
            {
                VehicleConfigList vehicleConfigList = CreateVehicleComponent.Instance.CreateConfPosnComponent(configList);
                return Ok(vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CreateConfPosnComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Get component based on component id
        [HttpGet]
        [Route("VehicleComponent/GetVehicleComponent")]
        public IHttpActionResult GetVehicleComponent(int componentId)
        {
            try
            {
                ComponentModel componentModel = GetVehicleComponentProvider.Instance.GetVehicleComponent(componentId);
                return Ok(componentModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetVehicleComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function to check configuration already created
        [HttpGet]
        [Route("VehicleComponent/GetConfigForComponent")]
        public IHttpActionResult GetConfigForComponent(int componentId)
        {
            try
            {
                VehicleConfigList vehicleConfigList = GetVehicleComponentProvider.Instance.GetConfigForComponent(componentId);
                return Ok(vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetConfigForComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function for getting registartion details
        [HttpGet]
        [Route("VehicleComponent/GetRegistrationDetails")]
        public IHttpActionResult GetRegistrationDetails(int compId)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = GetComponentRegistration.Instance.GetRegistrationDetails(compId);
                return Ok(vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetRegistrationDetails, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function for getting VR1 registartion details
        [HttpGet]
        [Route("VehicleComponent/GetVR1RegistrationDetails")]
        public IHttpActionResult GetVR1RegistrationDetails(int componentId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = GetComponentRegistration.Instance.GetVR1RegistrationDetails(componentId, userSchema);
                return Ok(vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetVR1RegistrationDetails, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function for getting application registartion details
        [HttpGet]
        [Route("VehicleComponent/GetApplRegistrationDetails")]
        public IHttpActionResult GetApplRegistrationDetails(int componentId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = GetComponentRegistration.Instance.GetApplRegistrationDetails(componentId, userSchema);
                return Ok(vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetApplRegistrationDetails, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function for create registration
        [HttpPost]
        [Route("VehicleComponent/CreateRegistration")]
        public IHttpActionResult CreateRegistration(CreateComponentRegistrationParams createCompRegistrationParams)
        {
            try
            {
                int idNumber = CreateComponentRegistration.Instance.CreateRegistration(createCompRegistrationParams.ComponentId, createCompRegistrationParams.RegistrationValue, createCompRegistrationParams.FleetId);
                return Ok(idNumber);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CreateRegistration, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function for create VR1 registration
        [HttpPost]
        [Route("VehicleComponent/CreateVR1CompRegistration")]
        public IHttpActionResult CreateVR1CompRegistration(CreateComponentRegistrationParams createCompRegistrationParams)
        {
            try
            {
                int idNumber = CreateComponentRegistration.Instance.CreateVR1CompRegistration(createCompRegistrationParams.ComponentId, createCompRegistrationParams.RegistrationValue, createCompRegistrationParams.FleetId, createCompRegistrationParams.UserSchema);
                return Content(HttpStatusCode.Created, idNumber);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CreateVR1CompRegistration, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function for create Application registration
        [HttpPost]
        [Route("VehicleComponent/CreateAppCompRegistration")]
        public IHttpActionResult CreateAppCompRegistration(CreateComponentRegistrationParams createCompRegistrationParams)
        {
            try
            {
                int idNumber = CreateComponentRegistration.Instance.CreateAppCompRegistration(createCompRegistrationParams.ComponentId, createCompRegistrationParams.RegistrationValue, createCompRegistrationParams.FleetId, createCompRegistrationParams.UserSchema);
                return Content(HttpStatusCode.Created, idNumber);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CreateAppCompRegistration, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Update component in fleet managemnet
        [HttpPost]
        [Route("VehicleComponent/UpdateComponent")]
        public IHttpActionResult UpdateComponent(ComponentModel componentModel)
        {
            try
            {
                bool result = UpdateVehicleComponent.Instance.UpdateComponent(componentModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CreateVehicleRegFromCompReg
        [HttpGet]
        [Route("VehicleComponent/CreateVehicleRegFromCompReg")]
        public IHttpActionResult CreateVehicleRegFromCompReg(int componentId, int vehicleId)
        {
            try
            {
                int result = CreateComponentRegistration.Instance.CreateVehicleRegFromCompReg(componentId, vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CreateVehicleRegFromCompReg, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Update VR1 vehicle component in fleet managemnet
        [HttpPost]
        [Route("VehicleComponent/UpdateVR1VehComponent")]
        public IHttpActionResult UpdateVR1VehComponent(UpdateVehicleComponentParams updateVehComponentParams)
        {
            try
            {
                bool result = UpdateVehicleComponent.Instance.UpdateVR1VehComponent(updateVehComponentParams.ComponentModel, updateVehComponentParams.UserSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateVR1VehComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Update VR1 vehicle component in fleet managemnet
        [HttpPost]
        [Route("VehicleComponent/UpdateAppVehComponent")]
        public IHttpActionResult UpdateAppVehComponent(UpdateVehicleComponentParams updateVehComponentParams)
        {
            try
            {
                bool result = UpdateVehicleComponent.Instance.UpdateAppVehComponent(updateVehComponentParams.ComponentModel, updateVehComponentParams.UserSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateAppVehComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Getting component from route vehicle component table for VR1 appln vehicle
        [HttpGet]
        [Route("VehicleComponent/GetVR1VehicleComponent")]
        public IHttpActionResult GetVR1VehicleComponent(int componentId, string userSchema)
        {
            try
            {
                ComponentModel componentModel = GetVehicleComponentProvider.Instance.GetVR1VehicleComponent(componentId, userSchema);
                return Ok(componentModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetVR1VehicleComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Getting component from application component
        [HttpGet]
        [Route("VehicleComponent/GetAppVehicleComponent")]
        public IHttpActionResult GetAppVehicleComponent(int componentId, string userSchema)
        {
            try
            {
                ComponentModel componentModel = GetVehicleComponentProvider.Instance.GetAppVehicleComponent(componentId, userSchema);
                return Ok(componentModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetAppVehicleComponent, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Insert axle details
        [HttpPost]
        [Route("VehicleComponent/UpdateAxle")]
        public IHttpActionResult UpdateAxle(CreateComponentAxleParams createCompAxleParams)
        {
            try
            {
                bool result = false;
                result = UpdateVehicleComponent.Instance.UpdateAxle(createCompAxleParams.Axle, createCompAxleParams.ComponentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateAxle, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Insert VR1 axle details
        [HttpPost]
        [Route("VehicleComponent/UpdateVR1Axle")]
        public IHttpActionResult UpdateVR1Axle(CreateComponentAxleParams createCompAxleParams)
        {
            try
            {
                bool result = false;
                result = UpdateVehicleComponent.Instance.UpdateVR1Axle(createCompAxleParams.Axle, createCompAxleParams.ComponentId, createCompAxleParams.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateVR1Axle, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Insert appl axle details
        [HttpPost]
        [Route("VehicleComponent/UpdateAppAxle")]
        public IHttpActionResult UpdateAppAxle(CreateComponentAxleParams createCompAxleParams)
        {
            try
            {
                bool result = false;
                result = UpdateVehicleComponent.Instance.UpdateAppAxle(createCompAxleParams.Axle, createCompAxleParams.ComponentId, createCompAxleParams.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateAppAxle, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region List of axles from vehicle
        [HttpGet]
        [Route("VehicleComponent/ListAxle")]
        public IHttpActionResult ListAxle(int componentId)
        {
            try
            {
                List<Axle> axles = UpdateVehicleComponent.Instance.ListAxle(componentId);
                return Ok(axles);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/ListAxle, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region List of axles from vr1 app vehicle
        [HttpGet]
        [Route("VehicleComponent/ListVR1vehAxle")]
        public IHttpActionResult ListVR1vehAxle(int componentId, string userSchema)
        {
            try
            {
                List<Axle> axles = UpdateVehicleComponent.Instance.ListVR1vehAxle(componentId, userSchema);
                return Content(HttpStatusCode.OK, axles);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/ListVR1vehAxle, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region List of axles from app vehicle
        [HttpGet]
        [Route("VehicleComponent/ListAppvehAxle")]
        public IHttpActionResult ListAppvehAxle(int componentId, string userSchema)
        {
            try
            {
                List<Axle> axles = UpdateVehicleComponent.Instance.ListAppvehAxle(componentId, userSchema);
                return Content(HttpStatusCode.OK, axles);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/ListAppvehAxle, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Function to obtain component general details
        [HttpGet]
        [Route("VehicleComponent/GetRouteComponent")]
        public IHttpActionResult GetRouteComponent(int componentId, string userSchema)
        {
            try
            {
                ComponentModel componentModel = RouteVehicle.Instance.GetRouteComponent(componentId, userSchema);
                return Ok(componentModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetRouteComponent, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Function to obtain component general details
        [HttpGet]
        [Route("VehicleComponent/GetRouteComponentRegistrationDetails")]
        public IHttpActionResult GetRouteComponentRegistrationDetails(int componentId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = RouteVehicle.Instance.GetRouteComponentRegistrationDetails(componentId, userSchema);
                return Ok(vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetRouteComponentRegistrationDetails, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function to obtain the component axle details
        [HttpGet]
        [Route("VehicleComponent/ListRouteComponentAxle")]
        public IHttpActionResult ListRouteComponentAxle(int componentId, string userSchema)
        {
            try
            {
                List<Axle> axles = RouteVehicle.Instance.ListRouteComponentAxle(componentId, userSchema);
                return Content(HttpStatusCode.OK, axles);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/ListRouteComponentAxle, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete vehicle component in fleet management
        [HttpDelete]
        [Route("VehicleComponent/DeleteVehComponent")]
        public IHttpActionResult DeleteVehComponent(int componentId)
        {
            try
            {
                int result = DeleteVehicleComponent.Instance.DeleteVehComponent(componentId);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/DeleteVehComponent, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete vehicle registration in component
        [HttpDelete]
        [Route("VehicleComponent/DeleteComponentRegister")]
        public IHttpActionResult DeleteComponentRegister(int componentId, int IdNumber, int flag = 0)
        {
            try
            {
                int result = DeleteVehicleComponent.Instance.DeleteComponentRegister(componentId, IdNumber, flag);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/DeleteComponentRegister, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete VR1 application Component Registration
        [HttpDelete]
        [Route("VehicleComponent/DeleteVR1VehComponentRegister")]
        public IHttpActionResult DeleteVR1VehComponentRegister(int componentId, int IdNumber, string UserSchema)
        {
            try
            {
                int result = DeleteVehicleComponent.Instance.DeleteVR1VehComponentRegister(componentId, IdNumber, UserSchema);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/DeleteVR1VehComponentRegister, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete Application Component Registration 
        [HttpDelete]
        [Route("VehicleComponent/DeleteAppVehComponentRegister")]
        public IHttpActionResult DeleteAppVehComponentRegister(int componentId, int idNumber, string userSchema)
        {
            try
            {
                int result = DeleteVehicleComponent.Instance.DeleteAppVehComponentRegister(componentId, idNumber, userSchema);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/DeleteAppVehComponentRegister, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region AssessComponentMovementType 
        [HttpGet]
        [Route("VehicleComponent/AssessMovementType")]
        public IHttpActionResult AssessMovementType(int componentId)
        {
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(VehicleComponentConfiguration));
                VehicleComponentConfiguration vehicleComponentConfiguration;
                using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Configurations/VehicleComponents.xml")))
                {
                    vehicleComponentConfiguration = (VehicleComponentConfiguration)deserializer.Deserialize(reader);
                }

                //get component by componentId
                ComponentModel component = GetVehicleComponentProvider.Instance.GetVehicleComponent(componentId);

                List<uint> movementClassifications = new List<uint>();
                foreach (var vehicleComponentConfig in vehicleComponentConfiguration.VehicleComponents)
                {
                    //bool currentMovementClassFlag = true;
                    foreach (var componentConfig in vehicleComponentConfig.VehicleComponent)
                    {
                        List<string> subTypes = componentConfig.SubComponentTypeid.Split(',').ToList();
                        if (componentConfig.ComponentTypeID == component.ComponentType && subTypes.Contains(component.ComponentSubType.ToString()))
                        {
                            //if((componentConfig.AxleConfiguration == true && component.AxleCount > 0) //to be checked
                            //to be added
                            //(component.ConfigureTyreSpacing == true && componentModel.
                            //    )
                            //{
                            bool currentComponentFlag = true;
                            foreach (var paramNodeConfig in componentConfig.ParamNode)
                            {
                                //currentComponentFlag = true;
                                switch (paramNodeConfig.Items[1])
                                {
                                    case "Number of Axles":
                                        List<string> noOfAxleRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (noOfAxleRange[0] != "" &&
                                            (component.AxleCount < double.Parse(noOfAxleRange[0]) || component.AxleCount > double.Parse(noOfAxleRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Coupling":
                                        if (!(component.CouplingType == (int)CouplingType.None && paramNodeConfig.Items[2].ToString() == "None") &&
                                            !(component.CouplingType == (int)CouplingType.FifthWheel && paramNodeConfig.Items[2].ToString() == "5th Wheel") &&
                                            !(component.CouplingType == (int)CouplingType.Drawbar && paramNodeConfig.Items[2].ToString() == "Drawbar") &&
                                            !(component.CouplingType == (int)CouplingType.TowHitch && paramNodeConfig.Items[2].ToString() == "Tow Hitch"))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Weight":
                                        List<string> weightRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (weightRange[0] != "" &&
                                            (component.GrossWeight < double.Parse(weightRange[0]) || component.GrossWeight > double.Parse(weightRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Maximum Height":
                                        List<string> heightRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (heightRange[0] != "" &&
                                            (component.MaxHeight < double.Parse(heightRange[0]) || component.MaxHeight > double.Parse(heightRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Reducable Height":
                                        List<string> redHeightRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (redHeightRange[0] != "" &&
                                            (component.ReducableHeight < double.Parse(redHeightRange[0]) || component.ReducableHeight > double.Parse(redHeightRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Length":
                                        List<string> lengthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (lengthRange[0] != "" &&
                                            (component.RigidLength < double.Parse(lengthRange[0]) || component.RigidLength > double.Parse(lengthRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Width":
                                        List<string> widthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (widthRange[0] != "" &&
                                            (component.MaxHeight < double.Parse(widthRange[0]) || component.MaxHeight > double.Parse(widthRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Ground Clearance":
                                        List<string> groundClearanceRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (groundClearanceRange[0] != "" &&
                                            (component.GroundClearance < double.Parse(groundClearanceRange[0]) || component.GroundClearance > double.Parse(groundClearanceRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Reduced Ground Clearance":
                                        List<string> redGroundClearanceRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (redGroundClearanceRange[0] != "" &&
                                            (component.RedGroundClearance < double.Parse(redGroundClearanceRange[0]) || component.RedGroundClearance > double.Parse(redGroundClearanceRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Left Overhang":
                                        List<string> leftOverhangRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (leftOverhangRange[0] != "" &&
                                            (component.LeftOverhang < double.Parse(leftOverhangRange[0]) || component.LeftOverhang > double.Parse(leftOverhangRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Right Overhang":
                                        List<string> rightOverhangRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (rightOverhangRange[0] != "" &&
                                            (component.RightOverhang < double.Parse(rightOverhangRange[0]) || component.RightOverhang > double.Parse(rightOverhangRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Front Overhang":
                                        List<string> frontOverhangRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (frontOverhangRange[0] != "" &&
                                            (component.FrontOverhang < double.Parse(frontOverhangRange[0]) || component.FrontOverhang > double.Parse(frontOverhangRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Rear Overhang":
                                        List<string> rearOverhangRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (rearOverhangRange[0] != "" &&
                                            (component.RearOverhang < double.Parse(rearOverhangRange[0]) || component.RearOverhang > double.Parse(rearOverhangRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                    case "Outside Track":
                                        List<string> outsideTrackRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (outsideTrackRange[0] != "" &&
                                            (component.OutsideTrack < double.Parse(outsideTrackRange[0]) || component.OutsideTrack > double.Parse(outsideTrackRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;
                                }
                                if (!currentComponentFlag)
                                {
                                    break;
                                }

                            }
                            //}

                            if (currentComponentFlag)
                            {
                                movementClassifications.Add(vehicleComponentConfig.id);
                                break;
                            }
                        }
                    }
                }
                return Ok(movementClassifications);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/AssessMovementType, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region VehicleComponentMovementClassification
        [HttpGet]
        [Route("VehicleComponent/VehicleComponentMovementClassification")]
        public IHttpActionResult VehicleComponentMovementClassification(int componentTypeId, int componentSubTypeId)
        {
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(VehicleComponentConfiguration));
                VehicleComponentConfiguration vehicleComponentConfiguration;
                using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Configurations/VehicleComponents.xml")))
                {
                    vehicleComponentConfiguration = (VehicleComponentConfiguration)deserializer.Deserialize(reader);
                }

                List<uint> movementClassificationResult = new List<uint>();

                foreach (var movementClassification in vehicleComponentConfiguration.VehicleComponents)
                {
                    List<VehicleComponentConfigurationMovementClassificationIDVehicleComponent> componentTemp = new List<VehicleComponentConfigurationMovementClassificationIDVehicleComponent>();

                    foreach (var vehicleComponent in movementClassification.VehicleComponent)
                    {
                        int typeId = (int)vehicleComponent.ComponentTypeID;
                        List<string> subTypeIds = vehicleComponent.SubComponentTypeid.ToString().Split(',').ToList();

                        foreach (string subTypeId in subTypeIds)
                        {
                            if (int.Parse(subTypeId) == componentSubTypeId && typeId == componentTypeId)
                            {
                                componentTemp.Add(vehicleComponent);
                                break;
                            }
                        }
                    }

                    if (componentTemp.Count > 0)
                    {
                        movementClassificationResult.Add(movementClassification.id);
                    }

                }

                return Ok(movementClassificationResult);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/VehicleComponentMovementClassification, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("VehicleComponent/VehicleComponentType")]
        public IHttpActionResult VehicleComponentType(int movementClassificationId,string userSchema)
        {
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(VehicleComponentConfiguration));
                VehicleComponentConfiguration vehicleComponentConfiguration;
                using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Configurations/VehicleComponents.xml")))
                {
                    vehicleComponentConfiguration = (VehicleComponentConfiguration)deserializer.Deserialize(reader);
                }

                List<uint> componentId = new List<uint>();

                foreach (var movementClassification in vehicleComponentConfiguration.VehicleComponents)
                {
                    if (userSchema == UserSchema.Sort)
                    {
                        if (movementClassification.id == 270002|| movementClassification.id == 270006)
                        {
                            foreach (var component in movementClassification.VehicleComponent)
                            {
                                componentId.Add(component.ComponentTypeID);
                            }
                        }
                    }
                    else
                    {
                        if (movementClassification.id == movementClassificationId)
                        {
                            foreach (var component in movementClassification.VehicleComponent)
                            {
                                componentId.Add(component.ComponentTypeID);
                            }
                        }
                    }
                }

                return Ok(componentId.Distinct());
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/VehicleComponentType, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("VehicleComponent/VehicleSubComponentType")]
        public IHttpActionResult VehicleSubComponentType(int movementClassificationId, int componentTypeId, string userSchema)
        {
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(VehicleComponentConfiguration));
                VehicleComponentConfiguration vehicleComponentConfiguration;
                using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Configurations/VehicleComponents.xml")))
                {
                    vehicleComponentConfiguration = (VehicleComponentConfiguration)deserializer.Deserialize(reader);
                }

                List<uint> subComponentId = new List<uint>();

                foreach (var movementClassification in vehicleComponentConfiguration.VehicleComponents)
                {
                    if (userSchema == UserSchema.Sort)
                    {
                        if (movementClassification.id == 270002 || movementClassification.id == 270006)
                        {
                            foreach (var component in movementClassification.VehicleComponent)
                            {
                                if (component.ComponentTypeID == componentTypeId)
                                {
                                    List<string> subTypeIds = component.SubComponentTypeid.ToString().Split(',').ToList();
                                    subComponentId.AddRange(subTypeIds.Select(uint.Parse).ToList());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (movementClassification.id == movementClassificationId)
                        {
                            foreach (var component in movementClassification.VehicleComponent)
                            {
                                if (component.ComponentTypeID == componentTypeId)
                                {
                                    List<string> subTypeIds = component.SubComponentTypeid.ToString().Split(',').ToList();
                                    subComponentId.AddRange(subTypeIds.Select(uint.Parse).ToList());
                                }
                            }
                        }
                    }
                }

                return Ok(subComponentId.Distinct());
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/VehicleSubComponentType, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region VehicleComponentConfiguration
        [HttpGet]
        [Route("VehicleComponent/VehicleComponentConfiguration")]
        public IHttpActionResult VehicleComponentConfiguration(int componentTypeId, int componentSubTypeId, int movementClassificationId)
        {
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(VehicleComponentConfiguration));
                VehicleComponentConfiguration vehicleComponentConfiguration;
                using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Configurations/VehicleComponents.xml")))
                {
                    vehicleComponentConfiguration = (VehicleComponentConfiguration)deserializer.Deserialize(reader);
                }

                foreach (var movementClassification in vehicleComponentConfiguration.VehicleComponents)
                {
                    if (movementClassificationId == movementClassification.id)
                    {
                        foreach (var vehicleComponent in movementClassification.VehicleComponent)
                        {
                            int typeId = (int)vehicleComponent.ComponentTypeID;
                            List<string> subTypeIds = vehicleComponent.SubComponentTypeid.ToString().Split(',').ToList();

                            foreach (string subTypeId in subTypeIds)
                            {
                                if (int.Parse(subTypeId) == componentSubTypeId && typeId == componentTypeId)
                                {
                                    return Ok(vehicleComponent);
                                }
                            }
                        }
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/VehicleComponentConfiguration, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Vehicle workflow TEMP table implementation

        #region Insert component to TEMP table
        [HttpPost]
        [Route("VehicleComponent/InsertComponentToTemp")]
        public IHttpActionResult InsertComponentToTemp(ComponentModel componentModel)
        {
            try
            {
                double componentId = 0;
                componentId = CreateVehicleComponent.Instance.InsertComponentToTemp(componentModel);
                return Ok(componentId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/InsertComponentToTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Update component subtype to TEMP table
        [HttpPost]
        [Route("VehicleComponent/UpdateComponentSubTypeToTemp")]
        public IHttpActionResult UpdateComponentSubTypeToTemp(ComponentModel componentModel)
        {
            try
            {
                int result = CreateVehicleComponent.Instance.UpdateComponentSubTypeToTemp(componentModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/InsertComponentToTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Insert registration to TEMP table
        [HttpPost]
        [Route("VehicleComponent/CreateRegistrationTemp")]
        public IHttpActionResult CreateRegistrationTemp(CreateComponentRegistrationParams createCompRegistrationParams)
        {
            try
            {
                int idNumber = CreateComponentRegistration.Instance.CreateRegistrationTemp(createCompRegistrationParams.ComponentId, createCompRegistrationParams.RegistrationValue, createCompRegistrationParams.FleetId, createCompRegistrationParams.Movement, createCompRegistrationParams.UserSchema);
                return Ok(idNumber);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CreateRegistrationTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Insert axle details to TEMP table
        [HttpPost]
        [Route("VehicleComponent/InsertAxleDetailsTemp")]
        public IHttpActionResult InsertAxleDetailsTemp(CreateComponentAxleParams createCompAxleParams)
        {
            try
            {
                bool result = false;
                result = UpdateVehicleComponent.Instance.InsertAxleDetailsTemp(createCompAxleParams.Axle, createCompAxleParams.ComponentId, createCompAxleParams.Movement, createCompAxleParams.UserSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/InsertAxleDetailsTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Update component in TEMP table
        [HttpPost]
        [Route("VehicleComponent/UpdateComponentTemp")]
        public IHttpActionResult UpdateComponentTemp(ComponentModel componentModel)
        {
            try
            {
                bool result = UpdateVehicleComponent.Instance.UpdateComponentTemp(componentModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateComponentTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get component ids from TEMP table
        [HttpGet]
        [Route("VehicleComponent/GetComponentIdTemp")]
        public IHttpActionResult GetComponentIdTemp(string GUID, int vehicleId, string userSchema)
        {
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            try
            {
                componentIdList = GetVehicleComponentProvider.Instance.GetComponentIdTemp(GUID, vehicleId, userSchema);
                return Ok(componentIdList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetComponentIdTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Get component based on component id and guid from TEMP table
        [HttpGet]
        [Route("VehicleComponent/GetComponentTemp")]
        public IHttpActionResult GetComponentTemp(int componentId, string GUID, string userSchema)
        {
            try
            {
                ComponentModel componentModel = GetVehicleComponentProvider.Instance.GetComponentTemp(componentId, GUID, userSchema);
                return Ok(componentModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetComponentTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Function for getting registartion details TEMP table
        [HttpGet]
        [Route("VehicleComponent/GetRegistrationTemp")]
        public IHttpActionResult GetRegistrationTemp(int compId, bool movement, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = GetComponentRegistration.Instance.GetRegistrationTemp(compId, movement, userSchema);
                return Ok(vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetRegistrationTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region List of axles from vehicle from TEMP table
        [HttpGet]
        [Route("VehicleComponent/ListAxleTemp")]
        public IHttpActionResult ListAxleTemp(int componentId, bool movement, string userSchema)
        {
            try
            {
                List<Axle> axles = UpdateVehicleComponent.Instance.ListAxleTemp(componentId, movement, userSchema);
                return Ok(axles);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/ListAxleTemp, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Insert component to vehicle config
        [HttpGet]
        [Route("VehicleComponent/InsertComponentConfigPosn")]
        public IHttpActionResult InsertComponentConfigPosn(int componentId, int vehicleId)
        {
            try
            {
                int result = UpdateVehicleComponent.Instance.InsertComponentConfigPosn(componentId, vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/InsertComponentConfigPosn, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  add to fleet using temp table
        [HttpGet]
        [Route("VehicleComponent/AddToFleetTemp")]
        public IHttpActionResult AddToFleetTemp(string GUID, int componentId, int vehicleId)
        {
            try
            {
                int result = UpdateVehicleComponent.Instance.AddToFleetTemp(GUID, componentId, vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/AddToFleetTemp, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete component from temp table
        [HttpDelete]
        [Route("VehicleComponent/DeleteComponentTemp")]
        public IHttpActionResult DeleteComponentTemp(int componentId)
        {
            try
            {
                int result = UpdateVehicleComponent.Instance.DeleteComponentTemp(componentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/DeleteComponentTemp, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete component in config posn
        [HttpDelete]
        [Route("VehicleComponent/DeleteComponentConfig")]
        public IHttpActionResult DeleteComponentConfig(int componentId, int vehicleId, bool movement, string userSchema)
        {
            try
            {
                int result = UpdateVehicleComponent.Instance.DeleteComponentConfig(componentId, vehicleId, movement, userSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/DeleteComponentConfig, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Get component fav
        [HttpGet]
        [Route("VehicleComponent/GetComponentFavourite")]
        public IHttpActionResult GetComponentFavourite(int organisationId, int movementId)
        {
            try
            {
                List < ComponentGridList > componentGridLists = GetVehicleComponentProvider.Instance.GetComponentFavourite(organisationId, movementId);
                return Ok(componentGridLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetComponentFavourite, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Update component in movement TEMP table
        [HttpPost]
        [Route("VehicleComponent/UpdateMovementComponentTemp")]
        public IHttpActionResult UpdateMovementComponentTemp(UpdateComponentParams updateParams)
        {
            try
            {
                bool result = UpdateVehicleComponent.Instance.UpdateMovementComponentTemp(updateParams.ComponentDetails, updateParams.UserSchema);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateMovementComponentTemp, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion

        #region Function for checking component exists
        [HttpGet]
        [Route("VehicleComponent/CheckComponentInternalnameExists")]
        public IHttpActionResult CheckComponentInternalnameExists(string componentName, int organisationId)
        {
            try
            {
                int count = CreateVehicleComponent.Instance.CheckComponentInternalnameExists(componentName, organisationId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/CheckComponentInternalnameExists, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Function to update axle count for semi trailer - conventional tractor
        [HttpPost]
        [Route("VehicleComponent/UpdateConventionalTractorAxleCount")]
        public IHttpActionResult UpdateConventionalTractorAxleCount(int axleCount, int vehicleId, int fromComponentId, int toComponentId, string userSchema = UserSchema.Portal)
        {
            try
            {
                int count = CreateVehicleComponent.Instance.UpdateConventionalTractorAxleCount(axleCount, vehicleId, fromComponentId, toComponentId, userSchema);
                return Ok(count);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/UpdateConventionalTractorAxleCount, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}