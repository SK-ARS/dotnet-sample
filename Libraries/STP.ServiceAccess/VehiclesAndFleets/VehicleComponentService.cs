using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleGlobalConfig;

namespace STP.ServiceAccess.VehiclesAndFleets
{
    public class VehicleComponentService:IVehicleComponentService
    {
        private readonly HttpClient httpClient;
        public VehicleComponentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public int CheckConfigNameExists(string internalName, int organisationId)
        {
            int configNameCount = 0;
            try
            {                
                string urlParameters = "?vehicleName=" + internalName+ "&organisationId=" + organisationId;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/CheckConfigNameExists{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    configNameCount = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CheckConfigNameExists, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return configNameCount;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CheckConfigNameExists, Exception: {ex}");
                throw;
            }
        }
        public double CreateComponent(ComponentModel componentObj)
        {
            double componentId = 0;
            try
            {

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/CreateComponent", componentObj).Result;
                if (response.IsSuccessStatusCode)
                {
                    componentId = response.Content.ReadAsAsync<double>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateComponent, Exception: {ex}");
                throw;
            }
        }
        public double InsertAppVehicleComponent(ComponentModel componentObj, string userSchema)
        {
            double componentId = 0;
            try
            {
                UpdateVehicleComponentParams updateVehComponentParams = new UpdateVehicleComponentParams
                {
                    ComponentModel = componentObj,
                    UserSchema = userSchema
                }; 
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/InsertAppVehicleComponent", updateVehComponentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    componentId = response.Content.ReadAsAsync<double>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertAppVehicleComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertAppVehicleComponent, Exception: {ex}");
                throw;
            }
        }
        public double InsertVR1VehicleComponent(ComponentModel componentObj, string userSchema= UserSchema.Portal)
        {
            double componentId = 0;
            try
            {
                UpdateVehicleComponentParams updateVehComponentParams = new UpdateVehicleComponentParams
                {
                    ComponentModel = componentObj,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/InsertVR1VehicleComponent", updateVehComponentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    componentId = response.Content.ReadAsAsync<double>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertVR1VehicleComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertVR1VehicleComponent, Exception: {ex}");
                throw;
            }
        }
        public VehicleConfigList CreateConfPosnComponent(VehicleConfigList configPosn)
        {
            VehicleConfigList vehicleConfigList = new VehicleConfigList();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                $"/VehicleComponent/CreateConfPosnComponent", configPosn).Result;
                if (response.IsSuccessStatusCode)
                {
                    vehicleConfigList = response.Content.ReadAsAsync<VehicleConfigList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateConfPosnComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleConfigList;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateConfPosnComponent, Exception: {ex}");
                throw;
            }
        }
        public List<ComponentGridList> GetComponentByOrganisationId(int organisationId, int pageNumber, int pageSize, string componentName, string componentType, string vehicleIntent,int filterFavourites, string userSchema = UserSchema.Portal, int presetFilter=1, int? sortOrder = null)
        {
            List<ComponentGridList> objConfigurationModel = new List<ComponentGridList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetComponentByOrganisationId?organisationId=" + organisationId + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&componentName=" + componentName + "&componentType=" + componentType + "&vehicleIntent=" + vehicleIntent +"&filterFavourites="+filterFavourites+ "&userSchema=" + userSchema + "&presetFilter=" + presetFilter + "&sortOrder=" + sortOrder
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<ComponentGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetComponentByOrganisationId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetComponentByOrganisationId, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public ComponentModel GetVehicleComponent(int componentId)
        {
            ComponentModel componentModel = new ComponentModel();
            try
            {
                string urlParameters = "?componentId=" + componentId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetVehicleComponent{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    componentModel = response.Content.ReadAsAsync<ComponentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetVehicleComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentModel;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetVehicleComponent, Exception: {ex}");
                throw;
            }
        }
        public VehicleConfigList GetConfigForComponent(int componentId)
        {
            VehicleConfigList vehicleConfigList = new VehicleConfigList();
            try
            {
                string urlParameters = "?componentId=" + componentId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetConfigForComponent{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    vehicleConfigList = response.Content.ReadAsAsync<VehicleConfigList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetConfigForComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
              
                return vehicleConfigList;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetConfigForComponent, Exception: {ex}");
                throw;
            }
        }
        public List<VehicleRegistration> GetRegistrationDetails(int compId)
        {
            List<VehicleRegistration> vehicleRegistrations = new List<VehicleRegistration>();
            try
            {
                string urlParameters = "?compId=" + compId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetRegistrationDetails{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    vehicleRegistrations = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRegistrationDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleRegistrations;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRegistrationDetails, Exception: {ex}");
                throw;
            }

        }
        public List<VehicleRegistration> GetVR1RegistrationDetails(int compId, string userSchema)
        {
            List<VehicleRegistration> vehicleRegistrations = new List<VehicleRegistration>();
            try
            {
                string urlParameters = "?componentId=" + compId + "&userSchema=" + userSchema ;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetVR1RegistrationDetails{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    vehicleRegistrations = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetVR1RegistrationDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleRegistrations;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetVR1RegistrationDetails, Exception: {ex}");
                throw;
            }
        }
        public List<VehicleRegistration> GetApplRegistrationDetails(int compId, string userSchema)
        {
            List<VehicleRegistration> vehicleRegistrations = new List<VehicleRegistration>();
            try
            {
                string urlParameters = "?compId=" + compId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetApplRegistrationDetails{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    vehicleRegistrations = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetApplRegistrationDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleRegistrations;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetApplRegistrationDetails, Exception: {ex}");
                throw;
            }
        }
        public int CreateRegistration(int compId, string registrationValue, string fleetId)
        {
            int IdNumber = 0;
            try
            {
                CreateComponentRegistrationParams createCompRegistrationParams = new CreateComponentRegistrationParams
                {
                    ComponentId = compId,
                    RegistrationValue= registrationValue,
                    FleetId= fleetId
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/CreateRegistration", createCompRegistrationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    IdNumber = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateRegistration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return IdNumber;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateRegistration, Exception: {ex}");
                throw;
            }
        }
        public int CreateVR1CompRegistration(int compId, string registrationValue, string fleetId, string userSchema)
        {
            int IdNumber = 0;
            try
            {
                CreateComponentRegistrationParams createCompRegistrationParams = new CreateComponentRegistrationParams
                {
                    ComponentId = compId,
                    RegistrationValue = registrationValue,
                    FleetId = fleetId,
                    UserSchema= userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/CreateVR1CompRegistration", createCompRegistrationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    IdNumber = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateVR1CompRegistration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return IdNumber;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateVR1CompRegistration, Exception: {ex}");
                throw;
            }
        }
        public int CreateAppCompRegistration(int compId, string registrationValue, string fleetId, string userSchema)
        {
            int IdNumber = 0;
            try
            {
                CreateComponentRegistrationParams createCompRegistrationParams = new CreateComponentRegistrationParams
                {
                    ComponentId = compId,
                    RegistrationValue = registrationValue,
                    FleetId = fleetId,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/CreateAppCompRegistration", createCompRegistrationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    IdNumber = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateAppCompRegistration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return IdNumber;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateAppCompRegistration, Exception: {ex}");
                throw;
            }
        }
        public bool UpdateComponent(ComponentModel componentObj)
        {
            bool success = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateComponent", componentObj).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateComponent, Exception: {ex}");
                throw;
            }
        }
        public int CreateVehicleRegFromCompReg(int compId, int vhclId)
        {
            int configNameCount = 0;
            try
            {
                string urlParameters = "?compId=" + compId + "&vhclId=" + vhclId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/CreateVehicleRegFromCompReg{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    configNameCount = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateVehicleRegFromCompReg, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return configNameCount;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateVehicleRegFromCompReg, Exception: {ex}");
                throw;
            }
        }
        public bool UpdateVR1VehComponent(ComponentModel componentObj, string userSchema)
        {
            bool success = false;
            try
            {
                UpdateVehicleComponentParams updateVehComponentParams = new UpdateVehicleComponentParams
                {
                    ComponentModel = componentObj,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateVR1VehComponent", updateVehComponentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateVR1VehComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateVR1VehComponent, Exception: {ex}");
                throw;
            }
        }
        public bool UpdateAppVehComponent(ComponentModel componentObj, string userSchema)
        {
            bool success = false;
            try
            {
                UpdateVehicleComponentParams updateVehComponentParams = new UpdateVehicleComponentParams
                {
                    ComponentModel = componentObj,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateAppVehComponent", updateVehComponentParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateAppVehComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateAppVehComponent, Exception: {ex}");
                throw;
            }
        }
        public ComponentModel GetVR1VehicleComponent(int componentId, string userSchema)
        {
            ComponentModel componentModel = new ComponentModel();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetVR1VehicleComponent{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    componentModel = response.Content.ReadAsAsync<ComponentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetVR1VehicleComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentModel;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetVR1VehicleComponent, Exception: {ex}");
                throw;
            }
        }
        public ComponentModel GetAppVehicleComponent(int componentId, string userSchema)
        {
            ComponentModel componentModel = new ComponentModel();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetAppVehicleComponent{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    componentModel = response.Content.ReadAsAsync<ComponentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetAppVehicleComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentModel;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetAppVehicleComponent, Exception: {ex}");
                throw;
            }
        }
        public void UpdateAxle(Axle axle, int componentId)
        {
            try
            {
                CreateComponentAxleParams createCompAxleParams = new CreateComponentAxleParams
                {
                    Axle = axle,
                    ComponentId = componentId
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateAxle", createCompAxleParams).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateAxle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateAxle, Exception: {ex}");
                throw;
            }
        }
        public void UpdateVR1Axle(Axle axle, int componentId, string userSchema)
        {
            try
            {
                CreateComponentAxleParams createCompAxleParams = new CreateComponentAxleParams
                {
                    Axle = axle,
                    ComponentId = componentId,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateVR1Axle", createCompAxleParams).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateVR1Axle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateVR1Axle, Exception: {ex}");
                throw;
            }
        }
        public void UpdateAppAxle(Axle axle, int componentId, string userSchema)
        {
            try
            {
                CreateComponentAxleParams createCompAxleParams = new CreateComponentAxleParams
                {
                    Axle = axle,
                    ComponentId = componentId,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateAppAxle", createCompAxleParams).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateAppAxle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateAppAxle, Exception: {ex}");
                throw;
            }
        }
        public List<Axle> ListAxle(int componentId)
        {
            List<Axle> axles = new List<Axle>();
            try
            {
                string urlParameters = "?componentId=" + componentId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/ListAxle{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    axles = response.Content.ReadAsAsync<List<Axle>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListAxle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return axles;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListAxle, Exception: {ex}");
                throw;
            }
        }
        public List<Axle> ListVR1vehAxle(int componentId, string userSchema)
        {
            List<Axle> axles = new List<Axle>();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/ListVR1vehAxle{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    axles = response.Content.ReadAsAsync<List<Axle>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListVR1vehAxle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return axles;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListVR1vehAxle, Exception: {ex}");
                throw;
            }
        }
        public List<Axle> ListAppvehAxle(int componentId, string userSchema)
        {
            List<Axle> axles = new List<Axle>();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/ListAppvehAxle{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    axles = response.Content.ReadAsAsync<List<Axle>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListAppvehAxle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return axles;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListAppvehAxle, Exception: {ex}");
                throw;
            }
        }
        public ComponentModel GetRouteComponent(int componentId, string userSchema)
        {
            ComponentModel componentModel = new ComponentModel();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetRouteComponent{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    componentModel = response.Content.ReadAsAsync<ComponentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRouteComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentModel;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRouteComponent, Exception: {ex}");
                throw;
            }
        }
        public List<VehicleRegistration> GetRouteComponentRegistrationDetails(int compId, string userSchema)
        {
            List<VehicleRegistration> vehicleRegistrations = new List<VehicleRegistration>();
            try
            {
                string urlParameters = "?componentId=" + compId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetRouteComponentRegistrationDetails{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    vehicleRegistrations = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRouteComponentRegistrationDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleRegistrations;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRouteComponentRegistrationDetails, Exception: {ex}");
                throw;
            }
        }
        public List<Axle> ListRouteComponentAxle(int componentId, string userSchema)
        {
            List<Axle> axles = new List<Axle>();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/ListRouteComponentAxle{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    axles = response.Content.ReadAsAsync<List<Axle>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListRouteComponentAxle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return axles;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListRouteComponentAxle, Exception: {ex}");
                throw;
            }
        }
        public bool DeleteVehComponent(int componentId)
        {
            bool success = false;
            try
            {
                string urlParameters = "?componentId=" + componentId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/DeleteVehComponent{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteVehComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteVehComponent, Exception: {ex}");
                throw;
            }
        }
        public bool DeleteComponentRegister(int compId, int IdNumber, int flag = 0)
        {
            bool success = false;
            try
            {
                string urlParameters = "?componentId=" + compId + "&IdNumber=" + IdNumber+ "&flag="+ flag;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/DeleteComponentRegister{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteComponentRegister, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteComponentRegister, Exception: {ex}");
                throw;
            }
        }
        public bool DeleteVR1VehComponentRegister(int compId, int IdNumber, string UserSchema)
        {
            bool success = false;
            try
            {
                string urlParameters = "?compId=" + compId + "&IdNumber=" + IdNumber + "&UserSchema=" + UserSchema;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/DeleteVR1VehComponentRegister{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteVR1VehComponentRegister, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteVR1VehComponentRegister, Exception: {ex}");
                throw;
            }
        }
        public bool DeleteAppVehComponentRegister(int compId, int IdNumber, string UserSchema)
        {
            bool success = false;
            try
            {
                string urlParameters = "?compId=" + compId + "&IdNumber=" + IdNumber + "&UserSchema=" + UserSchema;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/DeleteAppVehComponentRegister{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteAppVehComponentRegister, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteAppVehComponentRegister, Exception: {ex}");
                throw;
            }
        }
        public List<uint> VehicleComponentType(int movementClassificationId, string userSchema)
        {
            List<uint> componentId = new List<uint>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/VehicleComponentType?movementClassificationId=" + movementClassificationId+ "&UserSchema="+userSchema).Result;

                if (response.IsSuccessStatusCode)
                {
                    componentId = response.Content.ReadAsAsync<List<uint>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentType, Exception: {ex}");
                throw;
            }
        }
        public List<uint> VehicleSubComponentType(int movementClassificationId, int componentTypeId,string userSchema)
        {
            List<uint> subComponentId = new List<uint>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/VehicleSubComponentType?movementClassificationId=" + movementClassificationId + "&componentTypeId="+ componentTypeId+ "&userSchema="+ userSchema).Result;

                if (response.IsSuccessStatusCode)
                {
                    subComponentId = response.Content.ReadAsAsync<List<uint>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleSubComponentType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return subComponentId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleSubComponentType, Exception: {ex}");
                throw;
            }
        }

        #region Get Movement classification and component parameters

        public List<uint> VehicleComponentMovementClassification(int componentTypeId, int componentSubTypeId)
        {
            List<uint> movementClassificationResult = new List<uint>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                                            $"/VehicleComponent/VehicleComponentMovementClassification?componentTypeId=" + componentTypeId + "&componentSubTypeId=" + componentSubTypeId).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    movementClassificationResult = response.Content.ReadAsAsync<List<uint>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentMovementClassification, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentMovementClassification, Exception: {ex}");
            }
            return movementClassificationResult;
        }

        public VehicleComponentConfiguration VehicleComponentValidation(int componentTypeId, int componentSubTypeId, int movementClassificationId)
        {
            VehicleComponentConfiguration vehicleComponent = new VehicleComponentConfiguration();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                                            $"/VehicleComponent/VehicleComponentConfiguration?componentTypeId=" + componentTypeId + "&componentSubTypeId=" + componentSubTypeId+ "&movementClassificationId="+ movementClassificationId).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    vehicleComponent = response.Content.ReadAsAsync<VehicleComponentConfiguration>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentConfiguration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/VehicleComponentConfiguration, Exception: {ex}");
            }
            return vehicleComponent;
        }

        #endregion

        #region Vehicle workflow TEMP table implementation
        #region Insert component to TEMP table
        public double InsertComponentToTemp(ComponentModel componentModel)
        {
            double componentId = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/InsertComponentToTemp", componentModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    componentId = response.Content.ReadAsAsync<double>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertComponentToTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertComponentToTemp, Exception: {ex}");
            }
            return componentId;
        }
        #endregion
        #region Update component subtype to TEMP table
        public int UpdateComponentSubTypeToTemp(ComponentModel componentModel)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/UpdateComponentSubTypeToTemp", componentModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateComponentSubTypeToTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateComponentSubTypeToTemp, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region Insert registration to TEMP table
        public int CreateRegistrationTemp(int compId, string registrationValue, string fleetId, bool movement, string userSchema)
        {
            int IdNumber = 0;
            try
            {
                CreateComponentRegistrationParams createCompRegistrationParams = new CreateComponentRegistrationParams
                {
                    ComponentId = compId,
                    RegistrationValue = registrationValue,
                    FleetId = fleetId,
                    Movement= movement,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleComponent/CreateRegistrationTemp", createCompRegistrationParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    IdNumber = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateRegistrationTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return IdNumber;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CreateRegistrationTemp, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Insert axle details to TEMP table
        public void InsertAxleDetailsTemp(Axle axle, int componentId, bool movement, string userSchema)
        {
            try
            {
                CreateComponentAxleParams createCompAxleParams = new CreateComponentAxleParams
                {
                    Axle = axle,
                    ComponentId = componentId,
                    Movement= movement,
                    UserSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/InsertAxleDetailsTemp", createCompAxleParams).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertAxleDetailsTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertAxleDetailsTemp, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Update component in TEMP table
        public bool UpdateComponentTemp(ComponentModel componentObj)
        {
            bool success = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateComponentTemp", componentObj).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateComponentTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateComponentTemp, Exception: {ex}");
                throw;
            }
        }

        #endregion
        #region Get component ids from TEMP table
        public List<VehicleConfigList> GetComponentIdTemp(string GUID, int vehicleId, string userSchema)
        {
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetComponentIdTemp?GUID=" + GUID + "&vehicleId=" + vehicleId + "&userSchema=" + userSchema).Result;

                if (response.IsSuccessStatusCode)
                {
                    componentIdList = response.Content.ReadAsAsync<List<VehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetComponentIdTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentIdList;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetComponentIdTemp, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Get component details based on component id and guid from TEMP table
        public ComponentModel GetComponentTemp(int componentId, string GUID, string userSchema)
        {
            ComponentModel componentModel = new ComponentModel();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&GUID=" + GUID + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetComponentTemp{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    componentModel = response.Content.ReadAsAsync<ComponentModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetComponentTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentModel;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetComponentTemp, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Function for getting registartion details from TEMP table
        public List<VehicleRegistration> GetRegistrationTemp(int compId, bool movement, string userSchema)
        {
            List<VehicleRegistration> vehicleRegistrations = new List<VehicleRegistration>();
            try
            {
                string urlParameters = "?compId=" + compId+ "&movement=" + movement + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetRegistrationTemp{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    vehicleRegistrations = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRegistrationTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleRegistrations;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetRegistrationTemp, Exception: {ex}");
                throw;
            }

        }
        #endregion
        #region List of axles from component Temp
        public List<Axle> ListAxleTemp(int componentId, bool movement, string userSchema)
        {
            List<Axle> axles = new List<Axle>();
            try
            {
                string urlParameters = "?componentId=" + componentId + "&movement=" + movement + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/ListAxleTemp{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    axles = response.Content.ReadAsAsync<List<Axle>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListAxleTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return axles;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/ListAxleTemp, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Insert component to vehicle config
        public int InsertComponentConfigPosn(int componentId, int vehicleId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?componentId=" + componentId + "&vehicleId=" + vehicleId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/InsertComponentConfigPosn{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertComponentConfigPosn, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/InsertComponentConfigPosn, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Add to fleet using temp table
        public int AddToFleetTemp(string GUID, int componentId, int vehicleId)
        {
            int result = 0;
            try
            {

                string urlParameters = "?GUID=" + GUID + "&componentId=" + componentId + "&vehicleId=" + vehicleId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/AddToFleetTemp{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/AddToFleetTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/AddToFleetTemp, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Delete component from temp table
        public int DeleteComponentTemp(int componentId)
        {
            int result = 0;
            try
            {               

                string urlParameters = "?componentId=" + componentId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/DeleteComponentTemp{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteComponentTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteComponentTemp, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Delete component in config posn
        public int DeleteComponentConfig(int componentId, int vehicleId, bool movement, string userSchema)
        {
            int result = 0;
            try
            {
                string urlParameters = "?componentId=" + componentId + "&vehicleId=" + vehicleId+ "&movement="+ movement + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/DeleteComponentConfig{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteComponentConfig, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/DeleteComponentConfig, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Get component details based on component id and guid from TEMP table
        public List<ComponentGridList> GetComponentFavourite(int organisationId, int movementId)
        {
            List<ComponentGridList> componentGridLists = new List<ComponentGridList>();
            try
            {

                string urlParameters = "?organisationId=" + organisationId + "&movementId=" + movementId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/GetComponentFavourite{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    componentGridLists = response.Content.ReadAsAsync<List<ComponentGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetComponentFavourite, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return componentGridLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/GetComponentFavourite, Exception: {ex}");
                throw;
            }
        }
        #endregion

        #region Update component in movement TEMP table
        public bool UpdateMovementComponentTemp(ComponentModel componentObj, string userSchema)
        {
            bool success = false;
            try
            {
                UpdateComponentParams updateParams = new UpdateComponentParams()
                {
                    ComponentDetails = componentObj,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleComponent/UpdateMovementComponentTemp", updateParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateMovementComponentTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return success;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateMovementComponentTemp, Exception: {ex}");
                throw;
            }
        }

        #endregion
        #endregion

        public int CheckComponentInternalnameExists(string componentName, int organisationId)
        {
            int compNameCount = 0;
            try
            {
                string urlParameters = "?componentName=" + componentName + "&organisationId=" + organisationId;

                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/CheckComponentInternalnameExists{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    compNameCount = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CheckComponentInternalnameExists, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return compNameCount;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/CheckComponentInternalnameExists, Exception: {ex}");
                throw;
            }
        }

        public int UpdateConventionalTractorAxleCount(int axleCount, int vehicleId, int fromComponentId, int toComponentId, string userSchema)
        {
            int result = 0;
            try
            {
                string urlParameters = $@"?axleCount={axleCount}&vehicleId={vehicleId}&fromComponentId={fromComponentId}&toComponentId={toComponentId}&userSchema={userSchema}";
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleComponent/UpdateConventionalTractorAxleCount{urlParameters}",null).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateConventionalTractorAxleCount, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleComponent/UpdateConventionalTractorAxleCount, Exception: {ex}");
                throw;
            }
        }
    }
}
