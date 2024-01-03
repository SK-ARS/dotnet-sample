using STP.Common.Logger;
using STP.Common.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Applications;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleModel;
using Newtonsoft.Json;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.VehiclesAndFleets
{
   public class VehicleConfigService : IVehicleConfigService
    {
        private readonly HttpClient httpClient;
        public VehicleConfigService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }      
        public  long ImportVehicleFromList(long configId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", int IsCandidate = 0, string VersionType = "A")
        {
            long result = 0;
            try
            {
                ImportVehicleListModel InputParams = new ImportVehicleListModel
                {
                    ApplicationRevisionId = applnRev,
                    ConfigurationId = configId,
                    ContentRefNo = ContentRefNo,
                    IsCandidate = IsCandidate,
                    IsNotif = isNotif,
                    IsVR1 = isVR1,
                    UserSchema = userSchema,
                    VersionType = VersionType
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/ImportVehicleFromList", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt64(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportVehicleFromList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportVehicleFromList, Exception: {ex}");
            }
            return result;
        }
        public  long CopyVehicleFromList(long configId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", int IsCandidate = 0)
        {
            long result = 0;
            try
            {
                ImportVehicleListModel InputParams = new ImportVehicleListModel
                {
                    ApplicationRevisionId = applnRev,
                    ConfigurationId = configId,
                    ContentRefNo = ContentRefNo,
                    IsCandidate = IsCandidate,
                    IsNotif = isNotif,
                    IsVR1 = isVR1,
                    UserSchema = userSchema,
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/CopyVehicleFromList", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt64(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CopyVehicleFromList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CopyVehicleFromList, Exception: {ex}");
            }
            return result;
        }
        public  ConfigurationModel GetRouteConfigInfo(int componentId, string userSchema)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                     + $"/VehicleConfig/GetRouteConfigInfo?componentId=" + componentId + "&userSchema=" + userSchema
                                                                      ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteConfigInfo, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteConfigInfo, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  ConfigurationModel GetRouteConfigInfoForVR1(int componentId, string userSchema, int isEdit = 0)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetRouteConfigInfoForVR1?componentId=" + componentId + "&userSchema=" + userSchema + "&isEdit=" + isEdit
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteConfigInfoForVR1, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteConfigInfoForVR1, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  ConfigurationModel GetNotifVehicleConfiguration(int vhclId, int isSimple)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetNotifVehicleConfiguration?vhclId=" + vhclId + "&isSimple=" + isSimple
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNotifVehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNotifVehicleConfiguration, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  bool CheckNotifValidVehicle(int vhclId)
        {
            bool result = false;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/CheckNotifValidVehicle?vhclId=" + vhclId
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsAsync<bool>().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckNotifValidVehicle, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckNotifValidVehicle, Exception: {ex}");
            }
            return result;
        }
        #region  vehicle insert-Update
        public double CreateConfiguration(NewConfigurationModel configurationModel)
        {
            double result = 0;
            try
            {
                //api call to new service              
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/InsertVehicleConfiguration", configurationModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToDouble(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleConfiguration, Exception: {ex}");
            }
            return result;
        }
        public bool Updatevehicleconfig(NewConfigurationModel configurationModel)
        {
            bool result = false;
            try
            {
                NewConfigurationModel InputParams = new NewConfigurationModel
                {
                    ApplicationRevisionId = configurationModel.ApplicationRevisionId,
                    CandRevisionId = configurationModel.CandRevisionId,
                    ContentRefNo = configurationModel.ContentRefNo,
                    GrossWeight = configurationModel.GrossWeight,
                    GrossWeightKg = configurationModel.GrossWeightKg,
                    GrossWeightUnit = configurationModel.GrossWeightUnit,
                    Length = configurationModel.Length,
                    LengthMtr = configurationModel.LengthMtr,
                    LengthUnit = configurationModel.LengthUnit,
                    MaxAxleWeightKg = configurationModel.MaxAxleWeightKg,
                    MaxAxleWeight = configurationModel.MaxAxleWeight,
                    MaxAxleWeightUnit = configurationModel.MaxAxleWeightUnit,
                    MaxHeight = configurationModel.MaxHeight,
                    MaxHeightMtr = configurationModel.MaxHeightMtr,
                    MaxHeightUnit = configurationModel.MaxHeightUnit,
                    OrganisationId = configurationModel.OrganisationId,
                    PartId = configurationModel.PartId,
                    RedHeightMtr = configurationModel.RedHeightMtr,
                    RigidLength = configurationModel.RigidLength,
                    RevisionId = configurationModel.RevisionId,
                    RigidLengthMtr = configurationModel.RigidLengthMtr,
                    RigidLengthUnit = configurationModel.RigidLengthUnit,
                    RoutePartId = configurationModel.RoutePartId,
                    Speed = configurationModel.Speed,
                    SpeedUnit = configurationModel.SpeedUnit,
                    TyreSpacing = configurationModel.TyreSpacing,
                    TyreSpacingUnit = configurationModel.TyreSpacingUnit,
                    VehicleDesc = configurationModel.VehicleDesc,
                    VehicleId = configurationModel.VehicleId,
                    VehicleIntDesc = configurationModel.VehicleIntDesc,
                    VehicleName = configurationModel.VehicleName,
                    VehiclePurpose = configurationModel.VehiclePurpose,
                    VehicleType = configurationModel.VehicleType,
                    VersionId = configurationModel.VersionId,
                    WheelBase = configurationModel.WheelBase,
                    WheelBaseUnit = configurationModel.WheelBaseUnit,
                    Width = configurationModel.Width,
                    WidthMtr = configurationModel.WidthMtr,
                    WidthUnit = configurationModel.WidthUnit,
                    TractorAxleCount=configurationModel.TractorAxleCount,
                    TrailerAxleCount=configurationModel.TrailerAxleCount
                };
                //api call to new service              
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/UpdateVehicleConfiguration", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateVehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateVehicleConfiguration, Exception: {ex}");
            }
            return result;
        }
        public bool Updateappvehicleconfig(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            try
            {
                NewConfigurationModel InputParams = new NewConfigurationModel
                {
                    ApplicationRevisionId = configurationModel.ApplicationRevisionId,
                    CandRevisionId = configurationModel.CandRevisionId,
                    ContentRefNo = configurationModel.ContentRefNo,
                    GrossWeight = configurationModel.GrossWeight,
                    GrossWeightKg = configurationModel.GrossWeightKg,
                    GrossWeightUnit = configurationModel.GrossWeightUnit,
                    Length = configurationModel.Length,
                    LengthMtr = configurationModel.LengthMtr,
                    LengthUnit = configurationModel.LengthUnit,
                    MaxAxleWeightKg = configurationModel.MaxAxleWeightKg,
                    MaxAxleWeight = configurationModel.MaxAxleWeight,
                    MaxAxleWeightUnit = configurationModel.MaxAxleWeightUnit,
                    MaxHeight = configurationModel.MaxHeight,
                    MaxHeightMtr = configurationModel.MaxHeightMtr,
                    MaxHeightUnit = configurationModel.MaxHeightUnit,
                    OrganisationId = configurationModel.OrganisationId,
                    PartId = configurationModel.PartId,
                    RedHeightMtr = configurationModel.RedHeightMtr,
                    RigidLength = configurationModel.RigidLength,
                    RevisionId = configurationModel.RevisionId,
                    RigidLengthMtr = configurationModel.RigidLengthMtr,
                    RigidLengthUnit = configurationModel.RigidLengthUnit,
                    RoutePartId = configurationModel.RoutePartId,
                    Speed = configurationModel.Speed,
                    SpeedUnit = configurationModel.SpeedUnit,
                    TyreSpacing = configurationModel.TyreSpacing,
                    TyreSpacingUnit = configurationModel.TyreSpacingUnit,
                    VehicleDesc = configurationModel.VehicleDesc,
                    VehicleId = configurationModel.VehicleId,
                    VehicleIntDesc = configurationModel.VehicleIntDesc,
                    VehicleName = configurationModel.VehicleName,
                    VehiclePurpose = configurationModel.VehiclePurpose,
                    VehicleType = configurationModel.VehicleType,
                    VersionId = configurationModel.VersionId,
                    WheelBase = configurationModel.WheelBase,
                    WheelBaseUnit = configurationModel.WheelBaseUnit,
                    Width = configurationModel.Width,
                    WidthMtr = configurationModel.WidthMtr,
                    WidthUnit = configurationModel.WidthUnit
                };
                //api call to new service              
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/UpdateApplVehicleConfiguration?userSchema=" + userSchema, InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateApplVehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateApplVehicleConfiguration, Exception: {ex}");
            }
            return result;
        }
        public NewConfigurationModel InsertVR1VehicleConfiguration(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal)
        {
            NewConfigurationModel result = new NewConfigurationModel();
            try
            {
                NewConfigurationModel InputParams = new NewConfigurationModel
                {
                    ApplicationRevisionId = configurationModel.ApplicationRevisionId,
                    CandRevisionId = configurationModel.CandRevisionId,
                    ContentRefNo = configurationModel.ContentRefNo,
                    GrossWeight = configurationModel.GrossWeight,
                    GrossWeightKg = configurationModel.GrossWeightKg,
                    GrossWeightUnit = configurationModel.GrossWeightUnit,
                    Length = configurationModel.Length,
                    LengthMtr = configurationModel.LengthMtr,
                    LengthUnit = configurationModel.LengthUnit,
                    MaxAxleWeightKg = configurationModel.MaxAxleWeightKg,
                    MaxAxleWeight = configurationModel.MaxAxleWeight,
                    MaxAxleWeightUnit = configurationModel.MaxAxleWeightUnit,
                    MaxHeight = configurationModel.MaxHeight,
                    MaxHeightMtr = configurationModel.MaxHeightMtr,
                    MaxHeightUnit = configurationModel.MaxHeightUnit,
                    OrganisationId = configurationModel.OrganisationId,
                    PartId = configurationModel.PartId,
                    RedHeightMtr = configurationModel.RedHeightMtr,
                    RigidLength = configurationModel.RigidLength,
                    RevisionId = configurationModel.RevisionId,
                    RigidLengthMtr = configurationModel.RigidLengthMtr,
                    RigidLengthUnit = configurationModel.RigidLengthUnit,
                    RoutePartId = configurationModel.RoutePartId,
                    Speed = configurationModel.Speed,
                    SpeedUnit = configurationModel.SpeedUnit,
                    TyreSpacing = configurationModel.TyreSpacing,
                    TyreSpacingUnit = configurationModel.TyreSpacingUnit,
                    VehicleDesc = configurationModel.VehicleDesc,
                    VehicleId = configurationModel.VehicleId,
                    VehicleIntDesc = configurationModel.VehicleIntDesc,
                    VehicleName = configurationModel.VehicleName,
                    VehiclePurpose = configurationModel.VehiclePurpose,
                    VehicleType = configurationModel.VehicleType,
                    VersionId = configurationModel.VersionId,
                    WheelBase = configurationModel.WheelBase,
                    WheelBaseUnit = configurationModel.WheelBaseUnit,
                    Width = configurationModel.Width,
                    WidthMtr = configurationModel.WidthMtr,
                    WidthUnit = configurationModel.WidthUnit,
                    TractorAxleCount=configurationModel.TractorAxleCount,
                    TrailerAxleCount=configurationModel.TrailerAxleCount
                };
                //api call to new service                            
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/InsertVR1VehicleConfiguration?userSchema=" + userSchema, InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<NewConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVR1VehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVR1VehicleConfiguration, Exception: {ex}");
            }
            return result;
        }
        public double InsertApplicationVehicleConfiguration(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal)
        {
            double result = 0;
            try
            {
                NewConfigurationModel InputParams = new NewConfigurationModel
                {
                    ApplicationRevisionId = configurationModel.ApplicationRevisionId,
                    CandRevisionId = configurationModel.CandRevisionId,
                    ContentRefNo = configurationModel.ContentRefNo,
                    GrossWeight = configurationModel.GrossWeight,
                    GrossWeightKg = configurationModel.GrossWeightKg,
                    GrossWeightUnit = configurationModel.GrossWeightUnit,
                    Length = configurationModel.Length,
                    LengthMtr = configurationModel.LengthMtr,
                    LengthUnit = configurationModel.LengthUnit,
                    MaxAxleWeightKg = configurationModel.MaxAxleWeightKg,
                    MaxAxleWeight = configurationModel.MaxAxleWeight,
                    MaxAxleWeightUnit = configurationModel.MaxAxleWeightUnit,
                    MaxHeight = configurationModel.MaxHeight,
                    MaxHeightMtr = configurationModel.MaxHeightMtr,
                    MaxHeightUnit = configurationModel.MaxHeightUnit,
                    OrganisationId = configurationModel.OrganisationId,
                    PartId = configurationModel.PartId,
                    RedHeightMtr = configurationModel.RedHeightMtr,
                    RigidLength = configurationModel.RigidLength,
                    RevisionId = configurationModel.RevisionId,
                    RigidLengthMtr = configurationModel.RigidLengthMtr,
                    RigidLengthUnit = configurationModel.RigidLengthUnit,
                    RoutePartId = configurationModel.RoutePartId,
                    Speed = configurationModel.Speed,
                    SpeedUnit = configurationModel.SpeedUnit,
                    TyreSpacing = configurationModel.TyreSpacing,
                    TyreSpacingUnit = configurationModel.TyreSpacingUnit,
                    VehicleDesc = configurationModel.VehicleDesc,
                    VehicleId = configurationModel.VehicleId,
                    VehicleIntDesc = configurationModel.VehicleIntDesc,
                    VehicleName = configurationModel.VehicleName,
                    VehiclePurpose = configurationModel.VehiclePurpose,
                    VehicleType = configurationModel.VehicleType,
                    VersionId = configurationModel.VersionId,
                    WheelBase = configurationModel.WheelBase,
                    WheelBaseUnit = configurationModel.WheelBaseUnit,
                    Width = configurationModel.Width,
                    WidthMtr = configurationModel.WidthMtr,
                    WidthUnit = configurationModel.WidthUnit
                };
                //api call to new service              
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/InsertApplVehicleConfiguration?userSchema=" + userSchema, InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToDouble(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertApplVehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertApplVehicleConfiguration, Exception: {ex}");
            }
            return result;
        }
        public bool UpdateVR1vehicleconfig(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            try
            {
                NewConfigurationModel InputParams = new NewConfigurationModel
                {
                    ApplicationRevisionId = configurationModel.ApplicationRevisionId,
                    CandRevisionId = configurationModel.CandRevisionId,
                    ContentRefNo = configurationModel.ContentRefNo,
                    GrossWeight = configurationModel.GrossWeight,
                    GrossWeightKg = configurationModel.GrossWeightKg,
                    GrossWeightUnit = configurationModel.GrossWeightUnit,
                    Length = configurationModel.Length,
                    LengthMtr = configurationModel.LengthMtr,
                    LengthUnit = configurationModel.LengthUnit,
                    MaxAxleWeightKg = configurationModel.MaxAxleWeightKg,
                    MaxAxleWeight = configurationModel.MaxAxleWeight,
                    MaxAxleWeightUnit = configurationModel.MaxAxleWeightUnit,
                    MaxHeight = configurationModel.MaxHeight,
                    MaxHeightMtr = configurationModel.MaxHeightMtr,
                    MaxHeightUnit = configurationModel.MaxHeightUnit,
                    OrganisationId = configurationModel.OrganisationId,
                    PartId = configurationModel.PartId,
                    RedHeightMtr = configurationModel.RedHeightMtr,
                    RigidLength = configurationModel.RigidLength,
                    RevisionId = configurationModel.RevisionId,
                    RigidLengthMtr = configurationModel.RigidLengthMtr,
                    RigidLengthUnit = configurationModel.RigidLengthUnit,
                    RoutePartId = configurationModel.RoutePartId,
                    Speed = configurationModel.Speed,
                    SpeedUnit = configurationModel.SpeedUnit,
                    TyreSpacing = configurationModel.TyreSpacing,
                    TyreSpacingUnit = configurationModel.TyreSpacingUnit,
                    VehicleDesc = configurationModel.VehicleDesc,
                    VehicleId = configurationModel.VehicleId,
                    VehicleIntDesc = configurationModel.VehicleIntDesc,
                    VehicleName = configurationModel.VehicleName,
                    VehiclePurpose = configurationModel.VehiclePurpose,
                    VehicleType = configurationModel.VehicleType,
                    VersionId = configurationModel.VersionId,
                    WheelBase = configurationModel.WheelBase,
                    WheelBaseUnit = configurationModel.WheelBaseUnit,
                    Width = configurationModel.Width,
                    WidthMtr = configurationModel.WidthMtr,
                    WidthUnit = configurationModel.WidthUnit,
                    TractorAxleCount = configurationModel.TractorAxleCount,
                    TrailerAxleCount =configurationModel.TrailerAxleCount
                };
                //api call to new service              
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/UpdateVR1VehicleConfiguration?userSchema=" + userSchema, InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateVR1VehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateVR1VehicleConfiguration, Exception: {ex}");
            }
            return result;
        }
        public VehicleConfigList CreateConfigPosn(VehicleConfigList ConfigList)
        {
            VehicleConfigList objResultModel = new VehicleConfigList();
            try
            {
                VehicleConfigList InputParams = new VehicleConfigList
                {
                    VehicleId = ConfigList.VehicleId,
                    ComponentId = ConfigList.ComponentId,
                    LatPosn = ConfigList.LatPosn,
                    LongPosn = ConfigList.LongPosn,
                    SubType = ConfigList.SubType
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/InsertVehicleConfigurationPosition", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objResultModel = response.Content.ReadAsAsync<VehicleConfigList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleConfigurationPosition, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleConfigurationPosition, Exception: {ex}");
            }
            return objResultModel;
        }
        public int CreateVehicleRegistration(int vhclId, string registrationValue, string fleetId)
        {
            int result = 0;
            try
            {
                VehicleRegistrationInputParams InputParams = new VehicleRegistrationInputParams
                {
                    FleetId = fleetId,
                    RegistrationId = registrationValue,
                    VehicleId = vhclId
                };
                //api call to new service             
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/InsertVehicleRegistration", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt16(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleRegistration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleRegistration, Exception: {ex}");
            }
            return result;
        }
        public int SaveVR1VehicleRegistration(int vhclId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                VehicleRegistrationInputParams InputParams = new VehicleRegistrationInputParams
                {
                    FleetId = fleetId,
                    RegistrationId = registrationValue,
                    VehicleId = vhclId,
                    UserSchema = userSchema
                };
                //api call to new service             
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/SaveVR1VehicleRegistrationId", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt16(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVR1VehicleRegistrationId, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVR1VehicleRegistrationId, Exception: {ex}");
            }
            return result;
        }
        public int SaveAppVehicleRegistrationId(int vhclId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                VehicleRegistrationInputParams InputParams = new VehicleRegistrationInputParams
                {
                    FleetId = fleetId,
                    RegistrationId = registrationValue,
                    VehicleId = vhclId,
                    UserSchema = userSchema
                };
                //api call to new service             
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/SaveApplVehicleRegistrationId", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt16(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveApplVehicleRegistrationId, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveApplVehicleRegistrationId, Exception: {ex}");
            }
            return result;
        }
        public bool UpdateVehicleConfigDetails(int configId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false)
        {
            bool result = false;
            try
            {
                SaveConfigurationModel InputParams = new SaveConfigurationModel
                {
                    ApplicationRevisionId = applnRev,
                    ConfigurationId = configId,
                    IsNotif = isNotif,
                    IsVR1 = isVR1,
                    UserSchema = userSchema
                }; 
                //api call to new service             
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/SaveVehicleConfiguration", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVehicleConfiguration, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVehicleConfiguration, Exception: {ex}");
            }
            return result;
        }
        public VehicleConfigList CreateApplVehConfigPosn(VehicleConfigList ConfigList, string userSchema = UserSchema.Portal)
        {
            VehicleConfigList objResultModel = new VehicleConfigList();
            try
            {
                VehicleConfigList InputParams = new VehicleConfigList
                {
                    VehicleId = ConfigList.VehicleId,
                    ComponentId = ConfigList.ComponentId,
                    LatPosn = ConfigList.LatPosn,
                    LongPosn = ConfigList.LongPosn,
                    SubType = ConfigList.SubType
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/CreateApplVehicleConfigPosition?userSchema=" + userSchema, InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objResultModel = response.Content.ReadAsAsync<VehicleConfigList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateApplVehicleConfigPosition, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateApplVehicleConfigPosition, Exception: {ex}");
            }
            return objResultModel;
        }
        public VehicleConfigList CreateAppConfigPosn(VehicleConfigList ConfigList, int isImportFromFleet, string userSchema = UserSchema.Portal)
        {
            VehicleConfigList objResultModel = new VehicleConfigList() ;
            try
            {
                VehicleConfigList InputParams = new VehicleConfigList
                {
                    VehicleId = ConfigList.VehicleId,
                    ComponentId = ConfigList.ComponentId,
                    LatPosn = ConfigList.LatPosn,
                    LongPosn = ConfigList.LongPosn,
                    SubType = ConfigList.SubType
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/CreateApplConfigurationPosition?userSchema=" + userSchema + "&isImportFromFleet=" + isImportFromFleet, InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objResultModel = response.Content.ReadAsAsync<VehicleConfigList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateApplConfigurationPosition, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateApplConfigurationPosition, Exception: {ex}");
            }
            return objResultModel;
        }
        public VehicleConfigList CreateVR1ConfigPosn(VehicleConfigList ConfigList, string userSchema = UserSchema.Portal)
        {
            VehicleConfigList objResultModel = new VehicleConfigList();
            try
            {
                VehicleConfigList InputParams = new VehicleConfigList
                {
                    VehicleId = ConfigList.VehicleId,
                    ComponentId = ConfigList.ComponentId,
                    LatPosn = ConfigList.LatPosn,
                    LongPosn = ConfigList.LongPosn,
                    SubType = ConfigList.SubType
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/CreateVR1ConfigPosition?userSchema=" + userSchema, InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objResultModel = response.Content.ReadAsAsync<VehicleConfigList>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateVR1ConfigPosition, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateVR1ConfigPosition, Exception: {ex}");
            }
            return objResultModel;
        }
        #endregion
        #region AddToFleet
        public int AddComponentToFleet(int componentid, int organisationid)
        {
            int result = 0;
            try
            {
                FleetComponenentModel fleetComponenentModel = new FleetComponenentModel
                {
                    ComponentId = componentid,
                    OrganisationId = organisationid
                };
                 HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/AddComponentToFleet" , fleetComponenentModel).Result;
                //api call to new service
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddComponentToFleet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddComponentToFleet, Exception: {ex}");
            }
            return result;
        }
        public int AddApplicationComponentToFleet(int componentid, int organisationid, int flag, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                FleetComponenentModel fleetComponenentModel = new FleetComponenentModel
                {
                    ComponentId = componentid,
                    OrganisationId = organisationid,
                    Flag  = flag,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/AddApplicationComponentToFleet", fleetComponenentModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddComponentToFleet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddApplicationComponentToFleet, Exception: {ex}");
            }
            return result;
        }
        public int AddVR1ComponentToFleet(int componentid, int organisationid, int flag, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                //api call to new service
                FleetComponenentModel fleetComponenentModel = new FleetComponenentModel
                {
                    ComponentId = componentid,
                    OrganisationId = organisationid,
                    Flag = flag,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/AddVR1ComponentToFleet", fleetComponenentModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddComponentToFleet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddComponentToFleet, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region CHeckNameExists
        public int CheckFormalName(int componentid, int organisationid)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/CheckFormalName?componentId=" + componentid + "&organisationId=" + organisationid
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckFormalName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckFormalName, Exception: {ex}");
            }
            return result;
        }
        public  int CheckVR1FormalName(int componentid, int organisationid, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                                      + $"/VehicleConfig/CheckVR1FormalName?componentId=" + componentid + "&organisationId=" + organisationid + "&userSchema=" + userSchema
                                                                                      ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckVR1FormalName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckVR1FormalName, Exception: {ex}");
            }
            return result;
        }
        public  int CheckAppFormalName(int componentid, int organisationid, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/CheckApp1FormalName?componentId=" + componentid + "&organisationId=" + organisationid + "&userSchema=" + userSchema
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckApp1FormalName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckApp1FormalName, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region Delete Vehicle
        public  bool DeletVehicleRegisterConfiguration(int vehicleid, int IdNumber,bool isMovement=false)
        {
            bool result = false;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/DeletVehicleRegisterConfiguration?vehicleid=" + vehicleid + "&IdNumber=" + IdNumber+ "&isMovement="+ isMovement
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = true;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeletVehicleRegisterConfiguration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeletVehicleRegisterConfiguration, Exception: {ex}");
            }
            return result;
        }
        public  int DeleteVehicleConfigPosn(int vehicleid, int latpos, int longpos)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                        + $"/VehicleConfig/DeleteVehicleConfigPosition?vehicleid=" + vehicleid + "&latpos=" + latpos + "&longpos=" + longpos
                                                                        ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteVehicleConfigPosition, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteVehicleConfigPosition, Exception: {ex}");
            }
            return result;
        }
        public  int DeleteVR1VehicleConfigPosn(int vehicleid, int latpos, int longpos, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/DeleteVR1VehicleConfigPosition?vehicleid=" + vehicleid + "&latpos=" + latpos + "&longpos=" + longpos + "&userSchema=" + userSchema
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteVR1VehicleConfigPosition, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteVR1VehicleConfigPosition, Exception: {ex}");
            }
            return result;
        }
        public  int DeleteApplicationVehicleConfigPosn(int vehicleid, int latpos, int longpos, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                        + $"/VehicleConfig/DeleteApplVehicleConfigPosition?vehicleid=" + vehicleid + "&latpos=" + latpos + "&longpos=" + longpos + "&userSchema=" + userSchema
                                                                        ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteApplVehicleConfigPosition, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteApplVehicleConfigPosition, Exception: {ex}");
            }
            return result;
        }
        public  bool DisableVehicleApi(int vehicleid)//DisableVehicle
        {
            bool result = false;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.DeleteAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                      + $"/VehicleConfig/Disablevehicle?vehicleid=" + vehicleid
                                                                      ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = true;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/Disablevehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/Disablevehicle, Exception: {ex}");
            }
            return result;
        }
        public  bool DeleteVR1RegConfig(int vehicleid, int IdNumber, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.DeleteAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                      + $"/VehicleConfig/DeleteVR1RegConfiguration?vehicleid=" + vehicleid + "&IdNumber=" + IdNumber + "&userSchema=" + userSchema
                                                                      ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = true;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteVR1RegConfiguration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteVR1RegConfiguration, Exception: {ex}");
            }
            return result;
        }
        public  bool DeleteAppRegConfig(int vehicleid, int IdNumber, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.DeleteAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                      + $"/VehicleConfig/DeleteApplRegConfiguration?vehicleId=" + vehicleid + "&IdNumber=" + IdNumber + "&userSchema=" + userSchema
                                                                      ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = true;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteApplRegConfiguration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteApplRegConfiguration, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region GetApi--List
        public  List<VehicleRegistration>GetVehicleRegistrationDetails(int vhclId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> objvehicleconfiglist = new List<VehicleRegistration>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/GetVehicleRegistrationDetails?vehicleId="
                                                                       + vhclId + "&userSchema=" + userSchema
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleRegistrationDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleRegistrationDetails, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public  List<VehicleRegistration>GetVR1VehicleRegistrationDetails(int vhclId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> objvehicleconfiglist = new List<VehicleRegistration>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetVR1VehicleRegistrationDetails?vehicleId="
                                                                         + vhclId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVR1VehicleRegistrationDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVR1VehicleRegistrationDetails, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public  List<VehicleRegistration>GetApplVehicleRegistrationDetails(int vhclId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> objvehicleconfiglist = new List<VehicleRegistration>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                          + $"/VehicleConfig/GetApplVehicleRegistrationDetails?vhclID="
                                                                          + vhclId + "&userSchema=" + userSchema
                                                                          ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetApplVehicleRegistrationDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetApplVehicleRegistrationDetails, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public  List<VehicleConfigList> GetVR1VehicleConfigVhclID(int vhclID, string userSchema)
        {
            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                          + $"/VehicleConfig/GetVR1VehicleConfigVhclID?vehicleId="
                                                                          + vhclID + "&userSchema=" + userSchema
                                                                          ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    listVehclRegObj = response.Content.ReadAsAsync<List<VehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVR1VehicleConfigVhclID, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVR1VehicleConfigVhclID, Exception: {ex}");
            }
            return listVehclRegObj;
        }
        public  List<ComponentGridList> GetAllVR1ComponentByOrganisationId(long componentId, string userSchema)
        {
            List<ComponentGridList> objConfigurationModel = new List<ComponentGridList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetAllVR1ComponentByOrganisationId?componentId=" + componentId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<ComponentGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAllVR1ComponentByOrganisationId, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAllVR1ComponentByOrganisationId, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  List<VehicleConfigList> GetAppVehicleConfigVhclID(int vhclID, string userSchema = UserSchema.Portal)
        {
            List<VehicleConfigList> objConfigurationModel = new List<VehicleConfigList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetApplVehicleConfigVhclID?vehicleId=" + vhclID + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<VehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAppVehicleConfigVhclID, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAppVehicleConfigVhclID, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  List<VehicleConfigList> GetVehicleConfigVhclID(int vehicleId, string userSchema)
        {
            List<VehicleConfigList> objvehicleconfiglist = new List<VehicleConfigList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                        + $"/VehicleConfig/GetVehicleConfigVhclID?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                        ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleConfigVhclID, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleConfigVhclID, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public  List<ComponentGridList> GetAllAppComponentByOrganisationId(long componentId, string userSchema = UserSchema.Portal)
        {
            List<ComponentGridList> objConfigurationModel = new List<ComponentGridList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                          + $"/VehicleConfig/GetAllApplComponentByOrganisationId?componentId=" + componentId + "&userSchema=" + userSchema
                                                                          ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<ComponentGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAllApplComponentByOrganisationId, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAllApplComponentByOrganisationId, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  List<ComponentGridList> GetAllComponentByOrganisationId(int organisationId, long componentId)
        {
            List<ComponentGridList> objConfigurationModel = new List<ComponentGridList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetAllComponentByOrganisationId?OrganisationId=" + organisationId + "&componentId=" + componentId
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<ComponentGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAllComponentByOrganisationId, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetAllComponentByOrganisationId, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  List<ComponentIdModel> GetVR1ListComponentId(int vehicleId, string userSchema = UserSchema.Portal)
        {
            List<ComponentIdModel> objConfigurationModel = new List<ComponentIdModel>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetVR1ListComponentId?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<ComponentIdModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVR1ListComponentId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVR1ListComponentId, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  List<ComponentIdModel> GetAppListComponentId(int vehicleId, string userSchema = UserSchema.Portal)
        {
            List<ComponentIdModel> objConfigurationModel = new List<ComponentIdModel>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetApplListComponentId?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<ComponentIdModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetApplListComponentId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetApplListComponentId, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  List<ComponentIdModel> GetListComponentId(int vehicleId)
        {
            List<ComponentIdModel> objConfigurationModel = new List<ComponentIdModel>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetListComponentId?vehicleId=" + vehicleId
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<List<ComponentIdModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetListComponentId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetListComponentId, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  List<VehicleConfigList> GetRouteVehicleConfigVhclID(int vhclID, string userSchema)
        {
            List<VehicleConfigList> objvehicleconfiglist = new List<VehicleConfigList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetRouteVehicleConfigVhclID?vehicleId=" + vhclID + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteVehicleConfigVhclID, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteVehicleConfigVhclID, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public  long CheckValidVehicleCreated(int vhclID)
        {
            long result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/CheckValidVehicleCreated?vhclID=" + vhclID
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt64(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckValidVehicleCreated, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckValidVehicleCreated, Exception: {ex}");
            }
            return result;
        }
        public  List<VehicleRegistration> GetRouteVehicleRegistrationDetails(int vhclId, string userSchema)
        {
            List<VehicleRegistration> objvehicleconfiglist = new List<VehicleRegistration>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetRouteVehicleRegistrationDetails?vehicleId=" + vhclId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteVehicleRegistrationDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteVehicleRegistrationDetails, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public  List<VehicleConfigurationGridList> GetConfigByOrganisationId(int organisationId, int movtype, int movetype1, string userSchema, int filterFavouritesVehConfig,int presetFilter,int? sortOrder=null)
        {
            List<VehicleConfigurationGridList> objvehicleconfiglist = new List<VehicleConfigurationGridList>();        
            try
            {
                ConfigOrgIDParams inputParams = new ConfigOrgIDParams
                {
                    OrganisationId = organisationId,
                    MovementType1 = movetype1,
                    MovementType = movtype,
                    UserSchema = userSchema,
                    FilterFavouritesVehConfig= filterFavouritesVehConfig,
                    presetFilter=presetFilter,
                    sortOrder=sortOrder
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + 
                                                                            $"/VehicleConfig/GetConfigByOrganisationId", inputParams).Result;
                if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleConfigurationGridList>>().Result;
                    }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigByOrganisationId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigByOrganisationId, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public  ConfigurationModel GetNotifVehicleConfigByID(int VehicleId)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetNotifVehicleConfigByID?VehicleId=" + VehicleId
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNotifVehicleConfigByID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNotifVehicleConfigByID, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  ConfigurationModel GetConfigInfo(int componentId, string userSchema)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();     
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetConfigurationInfo?componentId=" + componentId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigurationInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigurationInfo, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  ConfigurationModel GetConfigInfoApplication(int vehicleId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetConfigInfoApplication?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigInfoApplication, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigInfoApplication, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  ConfigurationModel GetConfigInfoVR1(int componentId, string schematype = UserSchema.Portal)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetConfigurationInfoVR1?componentId=" + componentId + "&schematype=" + schematype
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigInfoVR1, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigInfoVR1, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public  bool CheckWheelWithSumOfAxel(int vhclId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false)
        {
            bool result = false;
            try
            {
                CheckWheelParams inputParams = new CheckWheelParams
                {
                    ApplicationRevisionId = applnRev,
                    VehicleId = vhclId,
                    IsNotif = isNotif,
                    IsVR1 = isVR1,
                    UserSchema = userSchema
                };
                //api call to new service             
                HttpResponseMessage response = httpClient.PostAsJsonAsync( $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/CheckWheelWithSumOfAxel", inputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckWheelWithSumOfAxel, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckWheelWithSumOfAxel, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region Vehicle Managemnet in Application
        #region  To import vehicle from fleet into a SO application
        public long SaveSOApplicationvehicleconfig(int vehicleId, int apprevisionId, int routepartid, string userSchema)
        {
            long result = 0;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&apprevisionId=" + apprevisionId + "&routepartid="+ routepartid +" &userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/SaveSOApplicationvehicleconfig{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveSOApplicationvehicleconfig, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveSOApplicationvehicleconfig, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  To import vehicle from fleet into a VR1 application
        public NewConfigurationModel SaveVR1Applicationvehicleconfig(NewConfigurationModel configurationModel)
        {
            NewConfigurationModel config = new NewConfigurationModel();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleConfig/SaveVR1Applicationvehicleconfig", configurationModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    config = response.Content.ReadAsAsync<NewConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVR1Applicationvehicleconfig, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return config;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVR1Applicationvehicleconfig, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  Applicationvehiclelist
        public List<VehicleDetailSummary> Applicationvehiclelist(int PartID, int FlagSOAppVeh, string RouteType , string userSchema )
        {
            List<VehicleDetailSummary> objComponentModelList = new List<VehicleDetailSummary>();
            try
            {
                string urlParameters = "?PartID=" + PartID + "&FlagSOAppVeh=" + FlagSOAppVeh + "&RouteType=" + RouteType + " &userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/Applicationvehiclelist{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objComponentModelList = response.Content.ReadAsAsync<List<VehicleDetailSummary>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/Applicationvehiclelist, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objComponentModelList;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/Applicationvehiclelist, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  Import vehicle from pre movement in application
        public int ImportApplnVehicleFromPreMove(int vehicleId, int apprevisionId, int routepartid, string userSchema)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&apprevisionId=" + apprevisionId + "&routepartid=" + routepartid + " &userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ImportApplnVehicleFromPreMove{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportApplnVehicleFromPreMove, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportApplnVehicleFromPreMove, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  ImportRouteVehicleToAppVehicle
        public int ImportRouteVehicleToAppVehicle(int vehicleId, int apprevisionId, int routepartid, string userSchema)
        {
            int result = 0;
            try
            {
                ImportVehicleParams importVehicleParams = new ImportVehicleParams()
                {
                    ApplicationRevisionId = apprevisionId,
                    ContentRefNo = "",
                    RoutePartId = routepartid,
                    UserSchema = userSchema,
                    VehicleId = vehicleId,
                    Simple = 0
                };
                //string urlParameters = "?vehicleId=" + vehicleId + "&apprevisionId=" + apprevisionId + "&routepartid=" + routepartid + " &userSchema=" + userSchema;
                //HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                //                                  $"/VehicleConfig/ImportRouteVehicleToAppVehicle{urlParameters}").Result;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleConfig/ImportRouteVehicleToAppVehicle", importVehicleParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportRouteVehicleToAppVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportRouteVehicleToAppVehicle, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  VR1 Application vehicle movemnt list
        public int VR1AppVehicleMovementList(int vehicleId, int apprevisionId, int routepartid, string userSchema)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&apprevisionId=" + apprevisionId + "&routepartid=" + routepartid + " &userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/VR1AppVehicleMovementList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/VR1AppVehicleMovementList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/VR1AppVehicleMovementList, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  SO Application vehicle movemnt list
        public int AppVehicleMovementList(int vehicleId, int apprevisionId, int routepartid, string userSchema)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&apprevisionId=" + apprevisionId + "&routepartid=" + routepartid + " &userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/AppVehicleMovementList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AppVehicleMovementList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AppVehicleMovementList, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  Delete selected vehicleComponent from so application
        public bool DeleteSelectedVehicleComponent(int vehicleId, string userSchema)
        {
            bool result = false;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId  + " &userSchema=" + userSchema;
                //HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                //                                  $"/VehicleConfig/DeleteSelectedVehicleComponent{urlParameters}").Result;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/DeleteSelectedVehicleComponent?vehicleId=" + vehicleId + "&userSchema=" + userSchema 
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                    result = true;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteSelectedVehicleComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteSelectedVehicleComponent, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  Delete selected vehicleComponent from vr1 application
        public bool DeleteSelectedVR1VehicleComponent(int vehicleId, string userSchema)
        {
            bool result = false;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId  + " &userSchema=" + userSchema;
                //HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                //                                  $"/VehicleConfig/DeleteSelectedVR1VehicleComponent{urlParameters}").Result;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                      + $"/VehicleConfig/DeleteSelectedVR1VehicleComponent?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                      ).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteSelectedVR1VehicleComponent, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteSelectedVR1VehicleComponent, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ViewVehicleSummaryByID
        public List<VehicleDetailSummary> ViewVehicleSummaryByID(long rPartId, int vr1, string userSchema)
        {
            List<VehicleDetailSummary> vehicleDetailSummaries = new List<VehicleDetailSummary>();
            try
            {
                string urlParameters = "?rPartId=" + rPartId+ "&vr1=" + vr1 + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ViewVehicleSummaryByID{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    vehicleDetailSummaries = response.Content.ReadAsAsync<List<VehicleDetailSummary>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ViewVehicleSummaryByID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleDetailSummaries;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ViewVehicleSummaryByID, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  CheckFormalNameInApplicationVeh
        public int CheckFormalNameInApplicationVeh(string vehicleName, int organisationId, string userSchema)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vehicleName=" + vehicleName + "&OrganisationId=" + organisationId + " &userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/CheckFormalNameInApplicationVeh{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckFormalNameInApplicationVeh, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckFormalNameInApplicationVeh, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  Add Vehicle To Fleet
        public int AddVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&OrganisationId=" + organisationId + "&flag=" + flag + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/AddVehicleToFleet{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddVehicleToFleet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddVehicleToFleet, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region  Add VR1 Vehicle To Fleet
        public int AddVR1VhclToFleet(int vehicleId, int organisationId, int flag)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&OrganisationId=" + organisationId + "&flag=" + flag;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/AddVR1VhclToFleet{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddVR1VhclToFleet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddVR1VhclToFleet, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Get the list of application vehicle list
        public List<AppVehicleConfigList> AppVehicleConfigList(long apprevisionId, string userSchema)
        {
            List<AppVehicleConfigList> appVehicleConfigLists = new List<AppVehicleConfigList>();
            try
            {
                string urlParameters = "?apprevisionId=" + apprevisionId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/AppVehicleConfigList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<AppVehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AppVehicleConfigList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AppVehicleConfigList, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Get the list of VR1 application vehicle list
        public List<AppVehicleConfigList> AppVehicleConfigListVR1(long routePartId, long versionId, string contentRefNo, string userSchema)
        {
            List<AppVehicleConfigList> appVehicleConfigLists = new List<AppVehicleConfigList>();
            try
            {
                string urlParameters = "?routePartId=" + routePartId + "&versionId=" + versionId + "&contentRefNo=" + contentRefNo + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/AppVehicleConfigListVR1{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<AppVehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AppVehicleConfigListVR1, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AppVehicleConfigListVR1, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Get the list of NEN vehicle list
        public List<AppVehicleConfigList> GetNenVehicleList(long routePartId)
        {
            List<AppVehicleConfigList> appVehicleConfigLists = new List<AppVehicleConfigList>();
            try
            {
                string urlParameters = "?routePartId=" + routePartId ;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/GetNenVehicleList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<AppVehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNenVehicleList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNenVehicleList, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ListVehCompDetails
        public List<ApplVehiclComponents> ListVehCompDetails(int revisionId, string userschema)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                string urlParameters = "?revisionId=" + revisionId+ "&userschema=" + userschema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehCompDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<ApplVehiclComponents>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehCompDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehCompDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ListLengthVehCompDetails
        public List<ApplVehiclComponents> ListLengthVehCompDetails(int revisionId, int vehicleId, string userschema)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                string urlParameters = "?revisionId=" + revisionId + "&vehicleId="+ vehicleId + "&userschema=" + userschema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListLengthVehCompDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<ApplVehiclComponents>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListLengthVehCompDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListLengthVehCompDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Checking vehicle weight against axle weight validation
        public List<ApplVehiclComponents> ListVehWeightDetails(int revisionId, int vehicleId, string userschema, int isVR1)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                string urlParameters = "?revisionId=" + revisionId + "&vehicleId=" + vehicleId + "&userschema=" + userschema + "&isVR1=" + isVR1;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehWeightDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<ApplVehiclComponents>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehWeightDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehWeightDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ListVR1VehCompDetails
        public List<ApplVehiclComponents> ListVR1VehCompDetails(int versionId, string contentref)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                string urlParameters = "?versionId=" + versionId + "&contentref=" + contentref;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVR1VehCompDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<ApplVehiclComponents>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVR1VehCompDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVR1VehCompDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ListLengthVR1VehDetails
        public List<ApplVehiclComponents> ListLengthVR1VehDetails(int versionId, string contentref)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                string urlParameters = "?versionId=" + versionId + "&contentref=" + contentref;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListLengthVR1VehDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigLists = response.Content.ReadAsAsync<List<ApplVehiclComponents>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListLengthVR1VehDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return appVehicleConfigLists;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListLengthVR1VehDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #endregion
        #region Vehicle Managemnet in Notification
        #region ImportFleetRouteVehicle
        public ListRouteVehicleId ImportFleetRouteVehicle(int VehicleID, string ContentRefNo, int simple, int RoutePartId = 0)
        {
            ListRouteVehicleId listRouteVehicleId = new ListRouteVehicleId();
            try
            {
                string urlParameters = "?VehicleID=" + VehicleID + "&ContentRefNo=" + ContentRefNo + "&simple=" + simple + " &RoutePartId=" + RoutePartId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ImportFleetRouteVehicle{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    listRouteVehicleId = response.Content.ReadAsAsync<ListRouteVehicleId>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportFleetRouteVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return listRouteVehicleId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportFleetRouteVehicle, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ImportFleetRouteVehicle
        public long ImportReturnRouteVehicle(int routePartId, string contentRefNo)
        {
            long routePartID = 0;
            try
            {
                string urlParameters = "?routePartId=" + routePartId + "?contentRefNo=" + contentRefNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ImportReturnRouteVehicle{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    routePartID = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportReturnRouteVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return routePartID;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportReturnRouteVehicle, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region UpdateNotifRouteVehicle
        public int UpdateNotifRouteVehicle(NotificationGeneralDetails obj, int RoutePartId, int vehicleUnits)
        {
            int status = 0;
            try
            {
                UpdateNotifRouteVehicleParams updateNotifRouteVehicleParams = new UpdateNotifRouteVehicleParams
                {
                    NotificationGeneralDetails = obj,
                    RoutePartId = RoutePartId,
                    VehicleUnits = vehicleUnits
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleConfig/UpdateNotifRouteVehicle", updateNotifRouteVehicleParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    status = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateNotifRouteVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return status;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateNotifRouteVehicle, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region UpdateNotifRouteVehicle
        public double CreateNotifVehicleConfiguration(NewConfigurationModel configurationModel, string contentRefNo, int isNotif)
        {
            double VehicleId = 0;
            try
            {
                CreateNotifVehicleConfigParams createNotifVehicleConfigParams = new CreateNotifVehicleConfigParams
                {
                    NewConfigurationModel = configurationModel,
                    ContentRefNo = contentRefNo,
                    IsNotif = isNotif
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleConfig/CreateNotifVehicleConfiguration", createNotifVehicleConfigParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    VehicleId = response.Content.ReadAsAsync<double>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateNotifVehicleConfiguration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return VehicleId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateNotifVehicleConfiguration, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region SaveNotifVehicleRegistrationId
        public int SaveNotifVehicleRegistrationId(int vhclId, string registrationValue, string fleetId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vhclId=" + vhclId + "&registrationValue=" + registrationValue + "&fleetId=" + fleetId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/SaveNotifVehicleRegistrationId{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveNotifVehicleRegistrationId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveNotifVehicleRegistrationId, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region SaveNotifVehicleConfiguration
        public int SaveNotifVehicleConfiguration(int vhclId, int compId, int compType)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vhclId=" + vhclId + "&compId=" + compId + "&compType=" + compType;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/SaveNotifVehicleConfiguration{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveNotifVehicleConfiguration, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveNotifVehicleConfiguration, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region SaveNotifAxel
        public bool SaveNotifAxel(AxleDetails axle)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleConfig/SaveNotifAxel", axle).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveNotifAxel, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveNotifAxel, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region UpdateMaxAxleWeight
        public bool UpdateMaxAxleWeight(long vehicleId)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/UpdateMaxAxleWeight",vehicleId).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateMaxAxleWeight, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateMaxAxleWeight, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ListCloneAxelDetails
        public List<AxleDetails> ListCloneAxelDetails(int VehicleID)
        {
            List<AxleDetails> axelDetails = new List<AxleDetails>();
            try
            {
                string urlParameters = "?VehicleID=" + VehicleID;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListCloneAxelDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    axelDetails = response.Content.ReadAsAsync<List<AxleDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListCloneAxelDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return axelDetails;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListCloneAxelDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region UpdateNewAxleDetails
        public bool UpdateNewAxleDetails(AxleDetails axle)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleConfig/UpdateNewAxleDetails", axle).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateNewAxleDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateNewAxleDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region GetNotificationVehicle
        public List<VehicleDetailSummary> GetNotificationVehicle(long partId)
        {
            List<VehicleDetailSummary> vehicleDetailSummaries = new List<VehicleDetailSummary>();
            try
            {
                string urlParameters = "?partId=" + partId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/GetNotificationVehicle{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    vehicleDetailSummaries = response.Content.ReadAsAsync<List<VehicleDetailSummary>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNotificationVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleDetailSummaries;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNotificationVehicle, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region ImportRouteVehicle
        public long ImportRouteVehicle(int routePartID, int vehicleID, string contentRefNo, int simple = 0)
        {
            long routePartId = 0;
            try
            {
                string urlParameters = "?routePartID=" + routePartID + "&vehicleID=" + vehicleID + "&contentRefNo=" + contentRefNo + " &simple=" + simple;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ImportRouteVehicle{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    routePartId = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportRouteVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return routePartId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportRouteVehicle, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking vehicle registraion validation
        public List<NotifVehicleRegistration> ListVehRegDetails(string contentReferenceNo)
        {
            List<NotifVehicleRegistration> objlistveh = new List<NotifVehicleRegistration>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + contentReferenceNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehRegDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleRegistration>> ().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehRegDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehRegDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking vehicle import validation
        public List<NotifVehicleImport> ListVehicleImportDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + contentReferenceNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehicleImportDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehicleImportDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehicleImportDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking vehicle weight against axle weight validation
        public List<NotifVehicleWeight> ListNotiVehWeightDetails(string ContentRefNo)
        {
            List<NotifVehicleWeight> objlistveh = new List<NotifVehicleWeight>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + ContentRefNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListNotiVehWeightDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleWeight>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListNotiVehWeightDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListNotiVehWeightDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking vehicle length validation
        public List<NotifVehicleImport> ListVehicleLengthDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + contentReferenceNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehicleLengthDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehLenDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehLenDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking vehicle gross weight validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleGrossWeightDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + contentReferenceNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehicleGrossWeightDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehicleGrossWeightDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehicleGrossWeightDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking vehicle width validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleWidthDetails(string contentReferenceNo, int reqVR1)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + contentReferenceNo + "&reqVR1=" + reqVR1;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehicleWidthDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehWidthDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehWidthDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking Axle Weight validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleAxleWeightDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + contentReferenceNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehicleAxleWeightDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehicleAxleWeightDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehicleAxleWeightDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region checking Rigid Length validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleRigidLengthDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            try
            {
                string urlParameters = "?contentReferenceNo=" + contentReferenceNo;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/ListVehicleRigidLengthDetails{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objlistveh = response.Content.ReadAsAsync<List<NotifVehicleImport>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehRigidLengthDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return objlistveh;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListVehRigidLengthDetails, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region DeletePrevVehicle
        public bool DeletePrevVehicle(int routePartId)
        {
            bool result = false;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                $"/VehicleConfig/DeletePrevVehicle", routePartId).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeletePrevVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeletePrevVehicle, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #endregion

        #region
        public List<AutoAssessingParams> AutoVehicleConfigType(List<ComponentIdList> componentIds)
        {
            List<AutoAssessingParams> autoAssessingParams = new List<AutoAssessingParams>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                                            $"/VehicleConfig/AutoVehicleConfigType?"+ componentIds).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    autoAssessingParams = response.Content.ReadAsAsync<List<AutoAssessingParams>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AutoVehicleConfigType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AutoVehicleMovementType, Exception: {ex}");
            }
            return autoAssessingParams;
        }

        public List<AutoAssessingMovementType> AutoVehicleMovementType(List<ComponentIdList> componentIds)
        {
            List<AutoAssessingMovementType> autoAssessingParams = new List<AutoAssessingMovementType>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                                            $"/VehicleConfig/AutoVehicleMovementType"+ componentIds).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    autoAssessingParams = response.Content.ReadAsAsync<List<AutoAssessingMovementType>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AutoVehicleMovementType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AutoVehicleMovementType, Exception: {ex}");
            }
            return autoAssessingParams;
        }

        public List<uint> AssessConfigurationType(List<int> componentIds, bool boatMastFlag = false, string userSchema = UserSchema.Portal,int flag=0)
        {
            List<uint> vehicleConfig = new List<uint>();
            try
            {
                ComponentIdParams componentIdParams = new ComponentIdParams()
                {
                    componentIds=componentIds,
                    userSchema= userSchema,
                    boatMastFlag= boatMastFlag,
                    flag= flag
                };
                //api call to new service
                //HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                //                                                            $"/VehicleConfig/AssessConfigurationType?componentIds=" + componentIds + "&userSchema="+ userSchema).Result;
                 HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/AssessConfigurationType", componentIdParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    vehicleConfig = response.Content.ReadAsAsync<List<uint>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AssessConfigurationType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AssessConfigurationType, Exception: {ex}");
            }
            return vehicleConfig;
        }

        public List<uint> AssessMovementClassificationType(List<Int64> componentIds, int configurationType, bool isMovement, string userSchema = UserSchema.Portal)
        {
            List<uint> movementClassifications = new List<uint>();
            try
            {
                AssessMovementTypeParams assessMovementTypeParams = new AssessMovementTypeParams()
                {
                    componentIds = componentIds,
                    configurationType = configurationType,
                    IsMovement=isMovement,
                    userSchema = userSchema
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync
                   ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                    $"/VehicleConfig/AssessMovementClassificationType", assessMovementTypeParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    movementClassifications = response.Content.ReadAsAsync<List<uint>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AssessMovementClassificationType, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AssessMovementClassificationType, Exception: {ex}");
            }
            return movementClassifications;
        }

        #endregion

        public List<VehicleConfigurationGridList> GetSimilarVehicleCombinations(SearchVehicleCombination configDimensions)
        {
            List<VehicleConfigurationGridList> objVehicleConfigList = new List<VehicleConfigurationGridList>();
            try
            {                
                HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/GetSimilarVehicleCombinations", configDimensions).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objVehicleConfigList = response.Content.ReadAsAsync<List<VehicleConfigurationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetSimilarVehicleCombinations, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetSimilarVehicleCombinations, Exception: {ex}");
            }
            return objVehicleConfigList;
        }

        public List<VehicleDetail> GetVehicleConfigByPartID(string ESDALRef, int vr1Vehicle)
        {
            List<VehicleDetail> objVehicleConfigList = new List<VehicleDetail>();
            try
            {
                VehicleConfigParams vehicleConfigParams = new VehicleConfigParams()
                {
                    ESDALRef = ESDALRef,
                    VR1Vehicle = vr1Vehicle
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/GetVehicleConfigByPartID", vehicleConfigParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objVehicleConfigList = response.Content.ReadAsAsync<List<VehicleDetail>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleConfigByPartID, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleConfigByPartID, Exception: {ex}");
            }
            return objVehicleConfigList;
        }
        public List<ComponentGroupingModel> ApplicationcomponentList(int routePartId)
        {
            List<ComponentGroupingModel> objComponentGroupingModel = new List<ComponentGroupingModel>();
            try
            {
                string urlParameter = "?routePartId=" + routePartId;
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/GetVehicleComponentList" + urlParameter).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objComponentGroupingModel = response.Content.ReadAsAsync<List<ComponentGroupingModel>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleComponentList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleComponentList, Exception: {ex}");
            }
            return objComponentGroupingModel;
        }

        #region Movement Vehicle
        public List<MovementVehicleConfig> InsertMovementVehicle(InsertMovementVehicle movementVehicle)
        {
            List<MovementVehicleConfig> movementVehicleConfig = new List<MovementVehicleConfig>();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/InsertMovementVehicle", movementVehicle).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    movementVehicleConfig = response.Content.ReadAsAsync<List<MovementVehicleConfig>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertMovementVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertMovementVehicle, Exception: {ex}");
            }
            return movementVehicleConfig;
        }

        public List<MovementVehicleConfig> SelectMovementVehicle(long movementId, string userSchema)
        {
            List<MovementVehicleConfig> movementVehicleConfig = new List<MovementVehicleConfig>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/SelectMovementVehicle?movementId=" + movementId + "&userSchema=" + userSchema).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    movementVehicleConfig = response.Content.ReadAsAsync<List<MovementVehicleConfig>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"VehicleConfig/SelectMovementVehicle, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"VehicleConfig/SelectMovementVehicle, Exception:" + ex);
            }
            return movementVehicleConfig;
        }

        public ConfigurationModel GetMovementConfigInfo(long vehicleId, string userSchema)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetMovementConfigurationInfo?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementConfigurationInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementConfigurationInfo, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        public List<VehicleConfigList> GetMovementVehicleConfig(long vehicleId, string userSchema)
        {
            List<VehicleConfigList> objvehicleconfiglist = new List<VehicleConfigList>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                        + $"/VehicleConfig/GetMovementVehicleConfig?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                        ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementVehicleConfig, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementVehicleConfig, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public List<VehicleRegistration> GetMovementVehicleRegDetails(long vehicleId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> objvehicleconfiglist = new List<VehicleRegistration>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/GetMovementVehicleRegDetails?vehicleId="
                                                                       + vehicleId + "&userSchema=" + userSchema
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementVehicleRegDetails, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementVehicleRegDetails, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }
        public List<MovementVehicleList> GetRouteVehicleList(long revisionId, long versionId, string cont_Ref_No, string userSchema,int isHistoric=0)
        {
            List<MovementVehicleList> prevMovementVehicleLists = new List<MovementVehicleList>();
            GetRouteVehicleList prevMovementVehicle = new GetRouteVehicleList
            {
                RevisionId=revisionId,
                VersionId = versionId,
                ContentRefNum = cont_Ref_No,
                UserSchema = userSchema,
                IsHistoric=isHistoric
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/GetRouteVehicleList", prevMovementVehicle).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    prevMovementVehicleLists = response.Content.ReadAsAsync<List<MovementVehicleList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteVehicleList, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetRouteVehicleList, Exception: {ex}");
            }
            return prevMovementVehicleLists;
        }
        public bool AssignMovementVehicle(List<VehicleAssignment> vehicleAssignment, long revisionId, long versionId, string contRefNum, string userSchema)
        {
            bool status = false;
            VehicleAssignementParams assignementParams = new VehicleAssignementParams
            {
                VehicleAssignments = vehicleAssignment,
                RevsionId = revisionId,
                VersionId = versionId,
                ContentRefNum = contRefNum,
                UserSchema = userSchema
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/AssignMovementVehicle", assignementParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    status = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AssignMovementVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AssignMovementVehicle, Exception: {ex}");
            }

            return status;
        }

        public bool DeleteMovementVehicle(long movementId, long vehicleId, string userSchema)
        {
            bool status = false;
            DeleteMovementVehicle deleteVehicle = new DeleteMovementVehicle
            {
                MovementId = movementId,
                VehicleId = vehicleId,
                UserSchema = userSchema
            };
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/DeleteMovementVehicle", deleteVehicle).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    status = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteMovementVehicle, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/DeleteMovementVehicle, Exception: {ex}");
            }

            return status;
        }

        public VehicleMovementType AssessMovementType(AssessMoveTypeParams moveTypeParams)
        {
            VehicleMovementType vehicleMovementType = new VehicleMovementType();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/AssessMovementType", moveTypeParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    vehicleMovementType = response.Content.ReadAsAsync<VehicleMovementType>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"VehicleConfig/AssessMovementType, Error:" + (int)response.StatusCode +" - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"VehicleConfig/AssessMovementType, Exception: " + ex);
            }
            return vehicleMovementType;
        }

        #endregion

        public List<VehicleList> GetFavouriteVehicles(int organisationId, int movementId, string userSchema)
        {
            string strUrlParameter = "?organisationId=" + organisationId + "&movementId=" + movementId + "&userSchema=" + userSchema;
            List<VehicleList> result = new List<VehicleList>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/GetFavouriteVehicles{strUrlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<VehicleList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListFavouriteVehicles, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ListFavouriteVehicles, Exception: {ex}");
            }

            return result;
        }

        public List<VehicleDetails> GetSORTMovVehicle(int PartId, string userSchema)
        {
            string strUrlParameter = "?PartId=" + PartId + "&userSchema=" + userSchema;
            List<VehicleDetails> result = new List<VehicleDetails>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/Vehicle/GetSORTMovVehicle{strUrlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<VehicleDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Vehicle/ListFavouriteVehicles, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Vehicle/ListFavouriteVehicles, Exception: {ex}");
            }

            return result;
        }
        public List<VehicleDetails> GetApplVehicle(int PartID, int revisionId, bool IsVRVeh, string userSchema)
        {
            List<VehicleDetails> vehicleDetails = new List<VehicleDetails>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/GetApplVehicle?PartID="
                                                                       + PartID + "&revisionId=" + revisionId + "&IsVRVeh=" + IsVRVeh + "&userSchema=" + userSchema
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    vehicleDetails = response.Content.ReadAsAsync<List<VehicleDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetApplVehicle, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetApplVehicle, Exception: {ex}");
            }
            return vehicleDetails;
        }        
        public List<VehicleDetails> GetVehicleList(long routePartId, string userSchema, int isHistoric = 0)
        {
            List<VehicleDetails> vehicleList = new List<VehicleDetails>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/GetVehicleList?routePartId="
                                                                       + routePartId + "&userSchema=" + userSchema+ "&isHistoric="+ isHistoric
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    vehicleList = response.Content.ReadAsAsync<List<VehicleDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleList, Exception: {ex}");
            }
            return vehicleList;
        }
        #region Vehicle workflow TEMP table implementation
        #region Get vehicle dimensions based on component id in TEMP table
        public ConfigurationModel GetConfigDimensions(string GUID, int configTypeId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetConfigDimensions?GUID=" + GUID + "&configTypeId=" + configTypeId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigDimensions, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigDimensions, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        #endregion
        #region Get vehicle dimensions based on vehicle id 
        public ConfigurationModel GetVehicleDimensions(long VehicleId, int configTypeId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetVehicleDimensions?VehicleId=" + VehicleId + "&configTypeId=" + configTypeId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleDimensions, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetConfigDimensions, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        #endregion
        #region Insert components from TEMP table to component table and insert vehicle config posn
        public bool InsertVehicleConfigPosnTemp(string GUID, int vehicleId, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            try
            {
                //api call to new service
                string urlParameters = "?GUID=" + GUID + "&vehicleId=" + vehicleId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync(
                                            $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" 
                                            + $"/VehicleConfig/InsertVehicleConfigPosnTemp?GUID=" + GUID + "&vehicleId=" + vehicleId + "&userSchema=" + userSchema).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleConfigPosnTemp, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertVehicleConfigPosnTemp, Exception: {ex}");
            }
            return result;
        }
        #endregion
        public ConfigurationModel GetVehicleDetails(int componentId,bool movement, string userSchema)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetVehicleDetails?componentId=" + componentId + "&movement="+ movement + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleDetails, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        #region check formal name in Temp table
        public int CheckFormalNameExistsTemp(int componentId, int organisationId)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/CheckFormalNameExistsTemp?componentId=" + componentId + "&organisationId=" + organisationId
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckFormalNameExistsTemp, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckFormalNameExistsTemp, Exception: {ex}");
            }
            return result;
        }

        #endregion
        #region  Insert config to movement vehicle temp table
        public List<MovementVehicleConfig> InsertConfigurationTemp(NewConfigurationModel configurationModel, string userSchema)
        {
            List<MovementVehicleConfig> movementVehicles = new List<MovementVehicleConfig>();
            try
            {
                CreateConfigurationParams createconfigParams = new CreateConfigurationParams()
                {
                    ConfigurationDetails = configurationModel,
                    UserSchema = userSchema
                };
                //api call to new service              
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/InsertConfigurationTemp", createconfigParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    movementVehicles = response.Content.ReadAsAsync<List<MovementVehicleConfig>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertConfigurationTemp, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertConfigurationTemp, Exception: {ex}");
            }
            return movementVehicles;
        }
        #endregion
        #region  Insert config to movement vehicle temp table
        public int CreateVehicleRegistrationTemp(int vhclId, string registrationValue, string fleetId, string userSchema)
        {
            int result = 0;
            try
            {
                VehicleRegistrationInputParams InputParams = new VehicleRegistrationInputParams
                {
                    FleetId = fleetId,
                    RegistrationId = registrationValue,
                    VehicleId = vhclId,
                    UserSchema = userSchema
                };
                //api call to new service             
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/CreateVehicleRegistrationTemp", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt16(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateVehicleRegistrationTemp, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CreateVehicleRegistrationTemp, Exception: {ex}");
            }
            return result;
        }
        #endregion
        #region Insert vehicle config posn from application
        public bool InsertMovementConfigPosnTemp(string GUID, int vehicleId, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            try
            {
                //api call to new service
                string urlParameters = "?GUID=" + GUID + "&vehicleId=" + vehicleId + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync(
                                            $"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                            + $"/VehicleConfig/InsertMovementConfigPosnTemp?GUID=" + GUID + "&vehicleId=" + vehicleId + "&userSchema=" + userSchema).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertMovementConfigPosnTemp, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/InsertMovementConfigPosnTemp, Exception: {ex}");
            }
            return result;
        }
        #endregion
        public bool UpdateMovementVehicle(NewConfigurationModel configurationModel, string userSchema)
        {
            bool result = false;
            try
            {
                NewConfigurationModel configDetails = new NewConfigurationModel
                {
                    ApplicationRevisionId = configurationModel.ApplicationRevisionId,
                    CandRevisionId = configurationModel.CandRevisionId,
                    ContentRefNo = configurationModel.ContentRefNo,
                    GrossWeight = configurationModel.GrossWeight,
                    GrossWeightKg = configurationModel.GrossWeightKg,
                    GrossWeightUnit = configurationModel.GrossWeightUnit,
                    Length = configurationModel.Length,
                    LengthMtr = configurationModel.LengthMtr,
                    LengthUnit = configurationModel.LengthUnit,
                    MaxAxleWeightKg = configurationModel.MaxAxleWeightKg,
                    MaxAxleWeight = configurationModel.MaxAxleWeight,
                    MaxAxleWeightUnit = configurationModel.MaxAxleWeightUnit,
                    MaxHeight = configurationModel.MaxHeight,
                    MaxHeightMtr = configurationModel.MaxHeightMtr,
                    MaxHeightUnit = configurationModel.MaxHeightUnit,
                    OrganisationId = configurationModel.OrganisationId,
                    PartId = configurationModel.PartId,
                    RedHeightMtr = configurationModel.RedHeightMtr,
                    RigidLength = configurationModel.RigidLength,
                    RevisionId = configurationModel.RevisionId,
                    RigidLengthMtr = configurationModel.RigidLengthMtr,
                    RigidLengthUnit = configurationModel.RigidLengthUnit,
                    RoutePartId = configurationModel.RoutePartId,
                    Speed = configurationModel.Speed,
                    SpeedUnit = configurationModel.SpeedUnit,
                    TyreSpacing = configurationModel.TyreSpacing,
                    TyreSpacingUnit = configurationModel.TyreSpacingUnit,
                    VehicleDesc = configurationModel.VehicleDesc,
                    VehicleId = configurationModel.VehicleId,
                    VehicleIntDesc = configurationModel.VehicleIntDesc,
                    VehicleName = configurationModel.VehicleName,
                    VehiclePurpose = configurationModel.VehiclePurpose,
                    VehicleType = configurationModel.VehicleType,
                    VersionId = configurationModel.VersionId,
                    WheelBase = configurationModel.WheelBase,
                    WheelBaseUnit = configurationModel.WheelBaseUnit,
                    Width = configurationModel.Width,
                    WidthMtr = configurationModel.WidthMtr,
                    WidthUnit = configurationModel.WidthUnit,
                    TractorAxleCount=configurationModel.TractorAxleCount,
                    TrailerAxleCount=configurationModel.TrailerAxleCount
                };
                CreateConfigurationParams updateParams = new CreateConfigurationParams()
                {
                    ConfigurationDetails = configDetails,
                    UserSchema = userSchema
                };
                //api call to new service              
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/UpdateMovementVehicle", updateParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateMovementVehicle, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateMovementVehicle, Exception: {ex}");
            }
            return result;
        }

        public List<VehicleRegistration> GetVehicleRegistrationTemp(int vhclId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> objvehicleconfiglist = new List<VehicleRegistration>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/GetVehicleRegistrationTemp?vehicleId="
                                                                       + vhclId + "&userSchema=" + userSchema
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objvehicleconfiglist = response.Content.ReadAsAsync<List<VehicleRegistration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleRegistrationTemp, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetVehicleRegistrationTemp, Exception: {ex}");
            }
            return objvehicleconfiglist;
        }

        #region  Add Vehicle To Fleet form movement temp
        public int AddMovementVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&OrganisationId=" + organisationId + "&flag=" + flag + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/AddMovementVehicleToFleet{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddMovementVehicleToFleet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddMovementVehicleToFleet, Exception: {ex}");
                throw;
            }
        }
        #endregion
        #region Get vehicle dimensions based on component id in movement TEMP table
        public ConfigurationModel GetMovementConfigDimensions(int vehicleId, string userSchema)
        {
            ConfigurationModel objConfigurationModel = new ConfigurationModel();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                         + $"/VehicleConfig/GetMovementConfigDimensions?vehicleId=" + vehicleId + "&userSchema=" + userSchema
                                                                         ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objConfigurationModel = response.Content.ReadAsAsync<ConfigurationModel>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementConfigDimensions, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetMovementConfigDimensions, Exception: {ex}");
            }
            return objConfigurationModel;
        }
        #endregion

        public int AddMovementComponentToFleet(int componentid, int organisationid, string userSchema)
        {
            int result = 0;
            try
            {
                FleetComponenentModel fleetComponenentModel = new FleetComponenentModel
                {
                    ComponentId = componentid,
                    OrganisationId = organisationid,
                    UserSchema = userSchema
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/AddMovementComponentToFleet", fleetComponenentModel).Result;
                //api call to new service
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddMovementComponentToFleet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddMovementComponentToFleet, Exception: {ex}");
            }
            return result;
        }

        public int MovementCheckFormalNameExists(int componentid, int organisationid, string userSchema)
        {
            int result = 0;
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleConfig/MovementCheckFormalNameExists?componentId=" + componentid + "&organisationId=" + organisationid + "&userSchema=" + userSchema
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/MovementCheckFormalNameExists, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/MovementCheckFormalNameExists, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region SORT Previous/Current Movement VehicleList
        public List<AppVehicleConfigList> GetSortMovementVehicle(long revisionId, int rListType)
        {
            List<AppVehicleConfigList> appVehicleConfigList = new List<AppVehicleConfigList>();
            string urlParameter = "?revisionId=" + revisionId + "&rListType=" + rListType;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + 
                    $"/VehicleConfig/GetSortMovementVehicle{urlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appVehicleConfigList = response.Content.ReadAsAsync<List<AppVehicleConfigList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetSortMovementVehicle, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetSortMovementVehicle, Exception:" + ex);
            }
            return appVehicleConfigList;
        }
        #endregion

        #region SaveNonEsdalVehicle
        public List<long> SaveNonEsdalVehicle(Domain.NonESDAL.NEVehicleImport neVehicleImport)
        {
            List<long> vehicleId = new List<long>();
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                + $"/VehicleImport/InsertNonEsdalVehicle", neVehicleImport).Result;
            if (response.IsSuccessStatusCode)
            {
                vehicleId = response.Content.ReadAsAsync<List<long>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- RouteImport/InsertNERoute, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return vehicleId;
        }
        #endregion

        #region Save All Components
        public long SaveVehicleComponents(VehicleConfigDetail vehicleDetail)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/SaveVehicleComponents", vehicleDetail).Result;
                //api call to new service
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVehicleComponents, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/SaveVehicleComponents, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region Update All Components
        public long UpdateVehicleComponents(VehicleConfigDetail vehicleDetail)
        {
            int result = 0;
            try
            {
                string dt = JsonConvert.SerializeObject(vehicleDetail);
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/UpdateVehicleComponents", vehicleDetail).Result;
                //api call to new service
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateVehicleComponents, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/UpdateVehicleComponents, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region ExportVehicleDetails
        public List<Domain.ExternalAPI.Vehicle> ExportVehicleDetails(Domain.ExternalAPI.GetVehicleExportList vehicleExportList)
        {
            List<Domain.ExternalAPI.Vehicle> vehicleList = new List<Domain.ExternalAPI.Vehicle>();
            try
            {
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                                                                       + $"/VehicleExport/ExportVehicleList", vehicleExportList
                                                                       ).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    vehicleList = response.Content.ReadAsAsync<List<Domain.ExternalAPI.Vehicle>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"VehicleExport/ExportVehicleList, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleExport/ExportVehicleList, Exception: {ex}");
            }
            return vehicleList;
        }
        #endregion

        #region ImportVehicleExternal
        public Domain.ExternalAPI.VehicleImportOutput ImportVehicleExternal(VehicleImportModel vehicleImportModel, long movementId)
        {
            Domain.ExternalAPI.VehicleImportOutput vehicleImportOutput = new Domain.ExternalAPI.VehicleImportOutput();
            vehicleImportModel.MovementId = movementId;
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                + $"/VehicleImport/InsertTempVehicle", vehicleImportModel).Result;
            if (response.IsSuccessStatusCode)
            {
                vehicleImportOutput = response.Content.ReadAsAsync<Domain.ExternalAPI.VehicleImportOutput>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- VehicleImport/InsertTempVehicle, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return vehicleImportOutput;
        }
        #endregion

        #region UpdateTempVehicle
        public bool UpdateTempVehicle(long movementId, int vehicleCategory, long vehicleId)
        {
            bool isSuccess = false;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                + $"/VehicleImport/UpdateTempVehicle?movementId=" + movementId + "&vehicleCategory=" + vehicleCategory + "&vehicleId=" + vehicleId).Result;
            if (response.IsSuccessStatusCode)
            {
                isSuccess = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- VehicleImport/InsertTempVehicle, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return isSuccess;
        }

        #endregion

        #region DeleteTempMovementVehicle
        public bool DeleteTempMovementVehicle(long movementId, string userSchema)
        {
            bool isSuccess = false;
            HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}"
                + $"/VehicleImport/DeleteTempMovementVehicle?movementId=" + movementId + "&userSchema=" + userSchema).Result;
            if (response.IsSuccessStatusCode)
            {
                isSuccess = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- VehicleImport/InsertTempVehicle, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return isSuccess;
        }
        #endregion


        #region Function for add component to fleet
        public long AddComponentToFleetLibrary(List<VehicleComponentModel> componentList)
        {
            int result = 0;
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/AddComponentToFleetLibrary", componentList).Result;
                //api call to new service
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddComponentToFleetLibrary, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AddComponentToFleetLibrary, Exception: {ex}");
            }
            return result;
        }
        #endregion

        public List<VehicleConfigurationGridList> GetFilteredVehicleCombinations(ConfigurationModel configurationModel)
        {
            List<VehicleConfigurationGridList> objVehicleConfigList = new List<VehicleConfigurationGridList>();
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/GetFilteredVehicleCombinations", configurationModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objVehicleConfigList = response.Content.ReadAsAsync<List<VehicleConfigurationGridList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetFilteredVehicleCombinations, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetFilteredVehicleCombinations, Exception: {ex}");
            }
            return objVehicleConfigList;
        }

        public long ImportFleetVehicleToRoute(long configId, string userSchema, int applnRev)
        {
            long result = 0;
            try
            {
                ImportVehicleListModel InputParams = new ImportVehicleListModel
                {
                    ApplicationRevisionId = applnRev,
                    ConfigurationId = configId,
                    UserSchema = userSchema
                };
                //api call to new service
                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" + $"/VehicleConfig/ImportFleetVehicleToRoute", InputParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = Convert.ToInt64(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportFleetVehicleToRoute, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/ImportFleetVehicleToRoute, Exception: {ex}");
            }
            return result;
        }

        public long ChekcVehicleIsValid(long vehicleId, int flag, string userSchema=UserSchema.Portal)
        {
            long result=0;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                     $"/VehicleConfig/ChekcVehicleIsValid?vehicleId=" + vehicleId + "&flag=" + flag + "&userSchema=" + userSchema).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"VehicleConfig/ChekcVehicleIsValid, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"VehicleConfig/ChekcVehicleIsValid, Exception:" + ex);
            }
            return result;
        }

        #region check vehicle validation
        public ImportVehicleValidations CheckVehicleValidations(int vehicleId, string userschema = UserSchema.Portal)
        {
            ImportVehicleValidations vehicleValidations = new ImportVehicleValidations();
            try
            {
                string urlParameters = "?vehicleId=" + vehicleId + "&userschema=" + userschema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                  $"/VehicleConfig/CheckVehicleValidations{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    vehicleValidations = response.Content.ReadAsAsync<ImportVehicleValidations>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckVehicleValidations, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                return vehicleValidations;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/CheckVehicleValidations, Exception: {ex}");
                throw;
            }
        }
        #endregion

        #region AutoFillVehicles
        public List<AutoFillModel> AutoFillVehicles(string vehicleIds, int vehicleCount, string userSchema)
        {
            List<AutoFillModel> autoFillModel = new List<AutoFillModel>();
            try
            {
                string urlParameters = "?vehicleIds=" + vehicleIds + "&vehicleCount=" + vehicleCount + "&userSchema=" + userSchema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                                                      $"/VehicleConfig/AutoFillVehicles{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                    autoFillModel = response.Content.ReadAsAsync<List<AutoFillModel>>().Result;
                else
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AutoFillVehicles, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/AutoFillVehicles, Exception: {ex}");
            }
            return autoFillModel;
        }
        #endregion

        #region GetNenApiVehiclesList
        public List<VehicleConfigration> GetNenApiVehiclesList(long notificationId, long organisationId, string userschema)
        {
            List<VehicleConfigration> objVehicleConfigration = new List<VehicleConfigration>();
            try
            {
                string urlParameters = "?notificationId=" + notificationId + "&organisationId=" + organisationId + "&userschema=" + userschema;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["VehiclesAndFleets"]}" +
                             $"/VehicleConfig/GetNenApiVehiclesList{urlParameters}").Result;
                if (response.IsSuccessStatusCode)
                {
                    objVehicleConfigration = response.Content.ReadAsAsync<List<VehicleConfigration>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNenApiVehiclesList, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfig/GetNenApiVehiclesList, Exception: {ex}");
            }
            return objVehicleConfigration;
        }
        #endregion
    }
}