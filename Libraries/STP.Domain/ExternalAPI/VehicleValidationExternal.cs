using Newtonsoft.Json;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.NonESDAL;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static STP.Common.Enums.ExternalApiEnums;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;

namespace STP.Domain.ExternalAPI
{
    #region VehicleValidateInputModel
    public class VehicleValidateInputModel
    {
        public Vehicle Vehicle { get; set; }
        public bool IsFleet { get; set; }
        public List<string> RouteNames { get; set; }
        public int VehiclesCount { get; set; }
        public int RoutesCount { get; set; }
        public int PrevVehicleMovementClass { get; set; }
        public int AssessedVehicleMovementClass { get; set; }
        public int AssessedMovementClass { get; set; }
        public bool IsNen { get; set; }
        public bool IsNea { get; set; }
        public int VehiclePos { get; set; }
        public VehicleMovementType PreviousMovementType { get; set; }
        public VehicleMovementType CurrentMovementType { get; set; }

    }
    #endregion

    #region VehicleValidationExternal
    public class VehicleValidationExternal
    {
        public VehicleValidationExternal()
        {
            ValidationFailure = LoadValidationFailure();
        }

        #region Model Decalration 

        #region Validation Failure Message and Dictionary
        public Dictionary<string, string> ValidationFailure { get; set; }
        public Dictionary<string, string> LoadValidationFailure()
        {
            string validationFile = ConfigurationManager.AppSettings["VehicleValidationFailureFile"];
            string path = ConfigurationManager.AppSettings["ValidationFailurePath"] + validationFile;
            if (ConfigurationManager.AppSettings["Envrironment"] == "Debug")
                path = AppDomain.CurrentDomain.RelativeSearchPath + validationFile;
            var vehicleValidationFailure = System.IO.File.ReadAllText(path);
            Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(vehicleValidationFailure);
            return keyValuePairs;
        }
        #endregion

        #region Variable Decalration for Property Length and UnitSystem
        public int DimensionUnitSystem { get; set; }
        public int WeightUnitSystem { get; set; }
        public int SpeedUnitSystem { get; set; }
        public int DimensionLength { get; set; }
        public int WeightLength { get; set; }
        public int SpeedLength { get; set; }
        #endregion

        #region Bool Variable to Identify Movement Class After Vehicle Assesssment using Algorith
        public bool SoMovement { get; set; }//For Special Order
        public bool VsoMovement { get; set; }//For Vehicle Special Order
        public bool WcuMovement { get; set; }//For Wheeled Construction and Use
        public bool StgoMovement { get; set; }// For Stgo Movement Not Require VR1
        public bool Vr1Movement { get; set; }// For Stgo Movement Require VR1
        public bool CraneMovement { get; set; }// For Crane Movement
        public string MovementClassification { get; set; }

        #endregion

        #region Bool Variable to Identify Vehicle Type
        public bool RigidVhcl { get; set; }// Rigid Vehicle
        public bool SemiVehicle { get; set; }// Semi Vehicle
        public bool DrawbarVehicle { get; set; }// Drawbar Vehicle
        public bool RigidAndDrag { get; set; }// Rigid And Drag
        public bool SpmtVehicle { get; set; }// SpmtVehicle
        public bool MobileCraneVehicle { get; set; }// Mobile Crane Vehicle 
        public bool Tracked { get; set; }// Tracked
        public bool RecoveryVhcl { get; set; }//Recovery Vehicle
        public string VehicleTypeName { get; set; }
        #endregion

        #region Bool Variable to Identify Vehicle Component Type
        public bool BallastTractor { get; set; }// Ballast Tractor
        public bool ConventionalTractor { get; set; }// Conventional Tractor
        public bool RigidVehicle { get; set; }// Rigid Vehicle
        public bool TrackedVehicle { get; set; }// Tracked Vehicle
        public bool SemiTrailer { get; set; }// Semi Trailer
        public bool DrawbarTrailer { get; set; }// Drawbar Trailer
        public bool Spmt { get; set; }// Spmt
        public bool MobileCrane { get; set; }// Mobile Crane
        public bool RecoveryVehicle { get; set; }// Recovery Vehicle
        public string ComponentTypeName { get; set; }
        #endregion

        #region BoolVariable to identify API request
        public bool IsNen { get; set; }
        public bool IsNea { get; set; }
        public bool IsFleet { get; set; }

        #endregion

        #region Create Configuration Model for Assessment
        public ConfigurationModel CreateConfigurationModel(Vehicle vehicle)
        {
            int? vehicleType;
            int? movementClass;
            int leadingComponentType;
            var firstComponent = vehicle.VehicleComponents?.OrderBy(x => x.ComponentNumber).FirstOrDefault();
            string componentSubType = firstComponent != null ? firstComponent.ComponentSubType.ToLower() : string.Empty;
            try
            {
                vehicleType = vehicle.VehicleConfiguration != null && vehicle.VehicleConfiguration.VehicleType != null ?
                    (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiVehicleType>(vehicle.VehicleConfiguration.VehicleType.ToLower()) : 0;
                movementClass = vehicle.VehicleConfiguration != null && vehicle.VehicleConfiguration.VehicleType != null ? 
                    (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiMovementClassification>(vehicle.VehicleConfiguration.MovementClassification.ToLower()) : 0;
                leadingComponentType = string.IsNullOrWhiteSpace(componentSubType) ? 0 : (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiComponentSubType>(componentSubType);
            }
            catch
            {
                vehicleType = null;
                movementClass = null;
                leadingComponentType = 0;
            }
            int? axleCount = GetAxleCount(vehicle, vehicleType);
            ConfigurationModel configurationModel = new ConfigurationModel
            {
                VehicleType = vehicleType,
                VehiclePurpose = movementClass,
                LeadingComponentType = leadingComponentType,
                GrossWeight = vehicle.VehicleConfiguration != null && vehicle.VehicleConfiguration.GrossWeight != null ? vehicle.VehicleConfiguration.GrossWeight : 0,
                Width = vehicle.VehicleConfiguration != null && vehicle.VehicleConfiguration.OverallWidth != null ? vehicle.VehicleConfiguration.OverallWidth : 0,
                OverallLength = vehicle.VehicleConfiguration != null && vehicle.VehicleConfiguration.OverallLength != null ? vehicle.VehicleConfiguration.OverallLength : 0,
                RigidLength = vehicle.VehicleConfiguration != null && vehicle.VehicleConfiguration.RigidLength != null ? vehicle.VehicleConfiguration.RigidLength : 0,
                AxleCount = axleCount,
                NotifFrontOverhang = vehicle.VehicleComponents?.Max(x => x.FrontOverhang != null ? x.FrontOverhang : 0),
                NotifRearOverhang = vehicle.VehicleComponents?.Max(x => x.RearOverhang != null ? x.RearOverhang : 0),
                NotifLeftOverhang = vehicle.VehicleComponents?.Max(x => x.LeftOverhang != null ? x.LeftOverhang : 0),
                NotifRightOverhang = vehicle.VehicleComponents?.Max(x => x.RightOverhang != null ? x.RightOverhang : 0),
                MaxAxleWeight = vehicle.VehicleComponents?.Max(x => x.Axles != null ? x.Axles.Max(item => item.AxleWeight != null ? item.AxleWeight : 0) : 0),
                TrailerWeight = vehicle.VehicleComponents != null ? vehicle.VehicleComponents.Where(x => x.ComponentNumber > 1).Sum(x => x.Axles != null ? x.Axles.Sum(item => item.AxleWeight != null ? item.AxleWeight : 0) : 0) : 0,
                WheelBase = vehicle.VehicleComponents?.Sum(x => x.Axles != null ? x.Axles.Sum(item => item.DistanceToNextAxle != null ? item.DistanceToNextAxle : 0) : 0)
            };

            return configurationModel;
        }
        //This function is added to idnetify if any componenets axle count is given as null then the axle count of assessment will be null else it will sum of axle count
        // where as in drawbar it is axle count for which component have maximum axle weight
        private static int? GetAxleCount(Vehicle vehicle, int? vehicleType) 
        {
            int? axleCount = null;
            double? maxGrossWeight = null;
            var isNullAxleCountExist = vehicle.VehicleComponents != null && vehicle.VehicleComponents.Exists(s => s.AxleCount == null);
            if (vehicleType == (int)ExternalApiVehicleType.VT001 || vehicleType == (int)ExternalApiVehicleType.VT008)
            {
                maxGrossWeight = vehicle.VehicleComponents?.Max(x => x.Axles != null ? x.Axles.Sum(s => s.AxleWeight != null ? s.AxleWeight : 0) : 0);
                if (isNullAxleCountExist)
                    axleCount = null;
                else
                    axleCount = vehicle.VehicleComponents?.Find(s => s.Axles != null && (s.Axles.Sum(x => x.AxleWeight) == maxGrossWeight)).AxleCount;
            }
            else
            {
                if (isNullAxleCountExist)
                    axleCount = null;
                else
                    axleCount = vehicle.VehicleComponents?.Sum(item => item.AxleCount);
            }
            return axleCount;
        }
        #endregion

        #endregion

        #region Validate Vehicle Data
        public VehicleImportModel ValidateVehicleData(VehicleValidateInputModel vehicleValidateInputModel, out bool IsVr1)
        {
            int errorCount = 0;
            string errorMsg = ValidationFailure["GeneralVehicleError"] + vehicleValidateInputModel.VehiclePos + "~";
            VehicleImportModel vehicleImportModel = new VehicleImportModel();
            StringBuilder builder = new StringBuilder();
            builder.Append(errorMsg);
            DimensionLength = Convert.ToInt32(ValidationFailure["DimensionLength"]);
            SpeedLength = Convert.ToInt32(ValidationFailure["SpeedLength"]);
            WeightLength = Convert.ToInt32(ValidationFailure["WeightLength"]);
            IsNen = vehicleValidateInputModel.IsNen;
            IsNea = vehicleValidateInputModel.IsNea;
            IsFleet = vehicleValidateInputModel.IsFleet;
            if (vehicleValidateInputModel.AssessedVehicleMovementClass > 0)
                SetMovementClassVariable(vehicleValidateInputModel.AssessedVehicleMovementClass);
            SetVehicleComponentVariable(vehicleValidateInputModel);
            IsVr1 = Vr1Movement;

            #region Unit System
            if (string.IsNullOrWhiteSpace(vehicleValidateInputModel.Vehicle.UnitSystem))
                vehicleImportModel.UnitSystem = ExternalApiWeightUnitSystem.Metric.GetEnumDescription();
            else
            {
                int unitSytem;
                try
                {
                    unitSytem = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiWeightUnitSystem>(vehicleValidateInputModel.Vehicle.UnitSystem.ToLower());
                }
                catch (Exception ex)
                {
                    vehicleImportModel.UnitSystem = ExternalApiWeightUnitSystem.Metric.GetEnumDescription();
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidUnitSystem"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateVehicleConfigData : Invalid Unit System Input:{ vehicleValidateInputModel.Vehicle.UnitSystem} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                }
                vehicleImportModel.UnitSystem = vehicleValidateInputModel.Vehicle.UnitSystem;
            }
            DimensionUnitSystem = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiDimensionUnitSystem>(vehicleImportModel.UnitSystem.ToLower());
            WeightUnitSystem = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiWeightUnitSystem>(vehicleImportModel.UnitSystem.ToLower());
            SpeedUnitSystem = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiDimensionUnitSystem>(vehicleImportModel.UnitSystem.ToLower());
            #endregion

            #region RouteName
            if (!vehicleValidateInputModel.IsFleet)
            {
                if (string.IsNullOrWhiteSpace(vehicleValidateInputModel.Vehicle.RouteName))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["RouteNameNotAvailable"]);
                }
                else
                {
                    var isRouteExist = vehicleValidateInputModel.RouteNames.Exists(x => x == vehicleValidateInputModel.Vehicle.RouteName);
                    if (isRouteExist)
                    {
                        vehicleImportModel.RouteName = vehicleValidateInputModel.Vehicle.RouteName;
                    }
                    else
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["RouteNameNotMatch"]);
                    }
                }
                if (vehicleValidateInputModel.RoutesCount > vehicleValidateInputModel.VehiclesCount)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["VehicleInSufficient"]);
                }
            }
            #endregion

            #region Configuration And Component Validation
            int vehicleType = 0;
            if (vehicleValidateInputModel.Vehicle.VehicleConfiguration != null)
            {
                vehicleImportModel.VehicleConfigDetails = ValidateVehicleConfigData(vehicleValidateInputModel.Vehicle.VehicleConfiguration, vehicleValidateInputModel.PrevVehicleMovementClass, vehicleValidateInputModel.AssessedMovementClass);
                vehicleType = vehicleImportModel.VehicleConfigDetails.VehicleType;
            }
            else
            {
                vehicleImportModel.VehicleConfigDetails = null;
                errorCount++;
                builder.Append(errorCount + ValidationFailure["VehicleConfigDetailsMandate"] + vehicleValidateInputModel.VehiclePos + "~");
            }
            List<VehicleComponentDetails> vehicleComponentsList = new List<VehicleComponentDetails>();
            VehicleComponentDetails vehicleComponentDetails;
            var i = 0;
            if (vehicleValidateInputModel.Vehicle.VehicleComponents != null)
            {
                foreach (var item in vehicleValidateInputModel.Vehicle.VehicleComponents.OrderBy(x => x.ComponentNumber))
                {
                    vehicleComponentDetails = ValidateComponentData(item, vehicleType, i, vehicleValidateInputModel.Vehicle.VehicleComponents.Count);
                    i++;
                    vehicleComponentsList.Add(vehicleComponentDetails);
                }
                vehicleImportModel.VehicleComponentDetails = vehicleComponentsList;
            }
            else
            {
                vehicleImportModel.VehicleComponentDetails = null;
                errorCount++;
                builder.Append(errorCount + ValidationFailure["VehicleComponentsMandate"] + vehicleValidateInputModel.VehiclePos + "~");
            }
            if(vehicleImportModel.VehicleConfigDetails != null && vehicleImportModel.VehicleComponentDetails != null)
                AutoCalculateValue(ref vehicleImportModel);
            #endregion

            #region Movement Type Class check
            if (vehicleValidateInputModel.PreviousMovementType != null && 
                vehicleValidateInputModel.CurrentMovementType.MovementType != vehicleValidateInputModel.PreviousMovementType.MovementType)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["MovementTypeMismatch"]);
            }
            #endregion

            #region Validation Error Message
            if (vehicleImportModel.VehicleConfigDetails != null && vehicleImportModel.VehicleConfigDetails.VehicleConfigError != null)
            {
                errorCount += vehicleImportModel.VehicleConfigDetails.VehicleConfigError.ErrorCount;
                builder.Append(vehicleImportModel.VehicleConfigDetails.VehicleConfigError.ErrorMessage);
            }
            if (vehicleImportModel.VehicleComponentDetails != null)
            {
                foreach (var item in vehicleComponentsList.Select(x => x.VehicleComponentError))
                {
                    if (item != null)
                    {
                        errorCount += item.ErrorCount;
                        builder.Append(item.ErrorMessage);
                    }
                }
            }
            #endregion

            #region Error Count
            if (errorCount > 0)
            {
                string ErrorMessage = builder.ToString();
                List<string> generalDetailsErrorList = ErrorMessage.Split('~').ToList();
                generalDetailsErrorList.RemoveAt(generalDetailsErrorList.Count - 1);
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = string.Join("~", generalDetailsErrorList),
                    ErrorList = generalDetailsErrorList
                };
                vehicleImportModel.VehicleError = validationError;
            }
            #endregion

            return vehicleImportModel;
        }
        #endregion

        #region ValidateVehicleConfigData
        private VehicleConfigDetails ValidateVehicleConfigData(VehicleConfiguration vehicleConfiguration, int previousVehicleMC, int movementClassification)
        {
            int errorCount = 0;
            string ErrorMessage = ValidationFailure["ConfigurationError"] + vehicleConfiguration.Name + "~";
            StringBuilder builder = new StringBuilder();
            builder.Append(ErrorMessage);
            VehicleConfigDetails vehicleConfigDetails = new VehicleConfigDetails();
            Registrations registration;
            
            #region Name
            if (string.IsNullOrWhiteSpace(vehicleConfiguration.Name))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ConfigNameNotFound"]);
            }
            else if (vehicleConfiguration.Name.Trim().Length > 100)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ConfigNameLength"]);
            }
            else
            {
                vehicleConfigDetails.InternalName = vehicleConfiguration.Name.Trim();
                vehicleConfigDetails.FormalName = vehicleConfiguration.Name.Trim();
            }
            #endregion

            #region Movement Classification
            if (string.IsNullOrWhiteSpace(vehicleConfiguration.MovementClassification))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["MovementClassNotFound"]);
            }
            else
            {
                try
                {
                    vehicleConfigDetails.MovementClassification =
                        (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiMovementClassification>(vehicleConfiguration.MovementClassification.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    vehicleConfigDetails.MovementClassification = 0;
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["MovementClassInvalid"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateVehicleConfigData : Invalid Movement Classification Input:{ vehicleConfiguration.MovementClassification} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                }
            }
            #endregion

            #region Check Previous Vehicle Movement Class with Current Vehicle Movement Class
            if (previousVehicleMC > 0 && previousVehicleMC != vehicleConfigDetails.MovementClassification)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["MovementClassMismatch"]);
            }
            #endregion

            #region Check Assessment Vehicle Movement Class with Current Vehicle Class
            if (movementClassification != vehicleConfigDetails.MovementClassification)
            {
                string[] validationErrorList;
                ExternalApiMovementClassification inputMCType = (ExternalApiMovementClassification)vehicleConfigDetails.MovementClassification;
                ExternalApiMovementClassification outputMCType = (ExternalApiMovementClassification)movementClassification;

                string inputMoveClass = inputMCType != 0 ? inputMCType.GetEnumDescription() : ValidationFailure["NoVehicleMovementClass"];
                string outputMoveClass = outputMCType != 0 ? outputMCType.GetEnumDescription() : ValidationFailure["NoVehicleMovementClass"];
                validationErrorList = ValidationFailure["MovementClassError"].Split(';');
                errorCount++;
                builder.Append(errorCount + validationErrorList[0] + outputMoveClass + validationErrorList[1] + inputMoveClass + validationErrorList[2]);
            }
            #endregion

            #region Vehicle Type
            if (string.IsNullOrWhiteSpace(vehicleConfiguration.VehicleType))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["VehicleTypeNotFound"]);
            }
            else
            {
                try
                {
                    vehicleConfigDetails.VehicleType = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiVehicleType>(vehicleConfiguration.VehicleType.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    vehicleConfigDetails.VehicleType = 0;
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["VehicleTypeInvalid"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateVehicleConfigData : Invalid Vehicle Type Input:{ vehicleConfiguration.VehicleType} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                }
            }
            SetVehicleTypeVariable(vehicleConfigDetails.VehicleType);
            #endregion

            #region Notes
            if (!string.IsNullOrWhiteSpace(vehicleConfiguration.Notes) && vehicleConfiguration.Notes.Trim().Length > 200)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ConfigNotesLength"]);
            }
            else
                vehicleConfigDetails.Description = !string.IsNullOrWhiteSpace(vehicleConfiguration.Notes) ? vehicleConfiguration.Notes.Trim() : string.Empty;
            #endregion

            #region Overall Length
            if (vehicleConfiguration.OverallLength > 0)
            {
                vehicleConfiguration.OverallLength = Math.Round((double)vehicleConfiguration.OverallLength, 2);
                if (CalculateLength((double)vehicleConfiguration.OverallLength, DimensionLength))
                {
                    vehicleConfigDetails.OverallLength = vehicleConfiguration.OverallLength;
                    vehicleConfigDetails.LengthUnit = DimensionUnitSystem;
                    vehicleConfigDetails.OverallLengthMtr = GetConvertedValues(DimensionUnitSystem, Math.Round((double)vehicleConfigDetails.OverallLength, 4));
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["OverallLengthSize"]);
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidOverallLength"]);
            }
            #endregion

            #region Rigid Length
            if (vehicleConfiguration.RigidLength > 0)
            {
                vehicleConfiguration.RigidLength = Math.Round((double)vehicleConfiguration.RigidLength, 2);
                if (RigidLengthNotMandate() && vehicleConfiguration.RigidLength != vehicleConfiguration.OverallLength)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["RigidLengthInCorrect"] + MovementClassification + VehicleTypeName + '~');
                }
                else if (!CalculateLength((double)vehicleConfiguration.RigidLength, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["RigidLengthSize"]);
                }
                else if (vehicleConfiguration.RigidLength > vehicleConfiguration.OverallLength)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["GreaterRigidLength"]);
                }
                else
                {
                    vehicleConfigDetails.RigidLength = vehicleConfiguration.RigidLength;
                    vehicleConfigDetails.RigidLengthUnit = DimensionUnitSystem;
                    vehicleConfigDetails.RigidLengthMtr = GetConvertedValues(DimensionUnitSystem, Math.Round((double)vehicleConfigDetails.RigidLength, 4));
                }
            }
            else if (!RigidLengthNotMandate())
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidRigidLength"]);
            }
            #endregion

            #region Width
            if (vehicleConfiguration.OverallWidth > 0)
            {
                vehicleConfiguration.OverallWidth = Math.Round((double)vehicleConfiguration.OverallWidth, 2);
                if (CalculateLength((double)vehicleConfiguration.OverallWidth, DimensionLength))
                {
                    vehicleConfigDetails.Width = vehicleConfiguration.OverallWidth;
                    vehicleConfigDetails.WidthUnit = DimensionUnitSystem;
                    vehicleConfigDetails.WidthMtr = GetConvertedValues(DimensionUnitSystem, Math.Round((double)vehicleConfigDetails.Width, 4));
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["OverallWidthSize"]);
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidOverallWidth"]);
            }
            #endregion

            #region Max Height
            if (vehicleConfiguration.MaxHeight > 0)
            {
                vehicleConfiguration.MaxHeight = Math.Round((double)vehicleConfiguration.MaxHeight, 2);
                if (CalculateLength((double)vehicleConfiguration.MaxHeight, DimensionLength))
                {
                    vehicleConfigDetails.MaxHeight = vehicleConfiguration.MaxHeight;
                    vehicleConfigDetails.MaxHeightUnit = DimensionUnitSystem;
                    vehicleConfigDetails.MaxHeightMtr = GetConvertedValues(DimensionUnitSystem, Math.Round((double)vehicleConfigDetails.MaxHeight, 4));
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["MaxHeightSize"]);
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidMaxHeight"]);
            }
            #endregion

            #region Gross Weight
            if (vehicleConfiguration.GrossWeight > 0)
            {
                vehicleConfiguration.GrossWeight = Math.Round((double)vehicleConfiguration.GrossWeight, 2);
                if (CalculateLength((double)vehicleConfiguration.GrossWeight, WeightLength))
                {
                    vehicleConfigDetails.GrossWeight = vehicleConfiguration.GrossWeight;
                    vehicleConfigDetails.GrossWeightUnit = WeightUnitSystem;
                    vehicleConfigDetails.GrossWeightKg = GetConvertedValues(WeightUnitSystem, Math.Round((double)vehicleConfigDetails.GrossWeight, 4));
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["GrossWeightSize"]);
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidGrossWeight"]);
            }
            #endregion

            #region Speed
            if (vehicleConfiguration.Speed == null)
                vehicleConfigDetails.Speed = vehicleConfiguration.Speed;
            else if (vehicleConfiguration.Speed > 0)
            {
                vehicleConfiguration.Speed = Math.Round((double)vehicleConfiguration.Speed, 2);
                if (CalculateLength((double)vehicleConfiguration.Speed, SpeedLength))
                {
                    vehicleConfigDetails.Speed = vehicleConfiguration.Speed;
                    vehicleConfigDetails.SpeedUnit = SpeedUnitSystem;
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["SpeedSize"]);
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidSpeed"]);
            }
            #endregion

            #region Registration
            if (vehicleConfiguration.VehicleRegistration != null && vehicleConfiguration.VehicleRegistration.Any())
            {
                foreach (var item in vehicleConfiguration.VehicleRegistration)
                {
                    if (item.SerialNumber <= 0)
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["InvalidSerialNumber"]);
                    }
                    if (string.IsNullOrWhiteSpace(item.Registration))
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["InvalidRegistration"]);
                    }
                    registration = new Registrations
                    {
                        FleetId = !string.IsNullOrWhiteSpace(item.FleetId) ? item.FleetId.Trim() : string.Empty,
                        SerialNumber = item.SerialNumber,
                        Registration = !string.IsNullOrWhiteSpace(item.Registration) ? item.Registration.Trim() : string.Empty
                    };
                    vehicleConfigDetails.VehicleRegistration.Add(registration);
                }
            }
            else if (!SoMovement || !Vr1Movement)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["VehicleRegistrationList"]);
            }
            #endregion

            #region Error Count
            if (errorCount > 0)
            {
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = builder.ToString().Trim()
                };
                vehicleConfigDetails.VehicleConfigError = validationError;
            }
            #endregion

            return vehicleConfigDetails;
        }
        #endregion

        #region ValidateComponentData
        private VehicleComponentDetails ValidateComponentData(VehicleComponents vehicleComponents, int vehicleType, int current, int totalCount)
        {
            int errorCount = 0;
            string ErrorMessage = ValidationFailure["ComponentError"] + vehicleComponents.Name + "~";
            StringBuilder builder = new StringBuilder();
            builder.Append(ErrorMessage);
            VehicleComponentDetails vehicleComponentDetails = new VehicleComponentDetails();

            #region Name
            if (string.IsNullOrWhiteSpace(vehicleComponents.Name))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ComponentNameNotFound"]);
            }
            else if (vehicleComponents.Name.Trim().Length > 100)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ComponentNameLength"]);
            }
            else
            {
                vehicleComponentDetails.InternalName = vehicleComponents.Name.Trim();
                vehicleComponentDetails.FormalName = vehicleComponents.Name.Trim();
            }
            #endregion

            #region Description
            if (!string.IsNullOrWhiteSpace(vehicleComponents.Notes) && vehicleComponents.Notes.Trim().Length > 200)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ComponentNotesLength"]);
            }
            else
                vehicleComponentDetails.Description = !string.IsNullOrWhiteSpace(vehicleComponents.Notes) ? vehicleComponents.Notes.Trim() : string.Empty;
            #endregion

            #region Component Type
            if (string.IsNullOrWhiteSpace(vehicleComponents.ComponentType))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ComponentTypeNotFound"]);
            }
            else
            {
                try
                {
                    vehicleComponentDetails.ComponentType = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiComponentType>(vehicleComponents.ComponentType.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    vehicleComponentDetails.ComponentType = 0;
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["ComponentTypeInvalid"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateVehicleComponentData : Invalid Component Type Input:{ vehicleComponents.ComponentType} Exception: { ex.Message } StackTrace { ex.StackTrace}");
                }
            }
            #endregion

            #region First Componenet Validation
            var componentTypeList = new List<int>
        {
            (int)ExternalApiComponentType.CT001,
            (int)ExternalApiComponentType.CT002,
            (int)ExternalApiComponentType.CT003,
            (int)ExternalApiComponentType.CT004,
            (int)ExternalApiComponentType.CT007,
            (int)ExternalApiComponentType.CT008,
            (int)ExternalApiComponentType.CT009,
            (int)ExternalApiComponentType.CT012
        };
            if (current == 0 && !componentTypeList.Contains(vehicleComponentDetails.ComponentType))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InitialComponentError"]);
            }
            #endregion

            #region Component SubType
            if (string.IsNullOrWhiteSpace(vehicleComponents.ComponentSubType))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ComponentSubTypeNotFound"]);
            }
            else
            {
                try
                {
                    vehicleComponentDetails.ComponentSubType = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiComponentSubType>(vehicleComponents.ComponentSubType.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    vehicleComponentDetails.ComponentSubType = 0;
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["ComponentSubTypeInvalid"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateVehicleComponentData : Invalid Component Sub Type Input:{ vehicleComponents.ComponentSubType} Exception: { ex.Message} StackTrace { ex.StackTrace}");
                }
            }
            #endregion

            #region Coupling Type
            if (string.IsNullOrWhiteSpace(vehicleComponents.CouplingType))
                vehicleComponentDetails.CouplingType = (int)ExternalApiCouplingType.None;
            else
            {
                try
                {
                    vehicleComponentDetails.CouplingType = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiCouplingType>(vehicleComponents.CouplingType.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    vehicleComponentDetails.CouplingType = 0;
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["CouplingTypeInvalid"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateVehicleComponentData : Invalid Coupling Type Input:{ vehicleComponents.CouplingType} Exception: { ex.Message} StackTrace { ex.StackTrace}");
                }
            }
            GetCouplingType(vehicleComponentDetails, vehicleComponents.ComponentType, out string message);
                
            if (!string.IsNullOrWhiteSpace(message))
            {
                errorCount++;
                builder.Append(errorCount + message + "~");
            }
            #endregion

            #region Matching check for component type with component sub type and vehicle type
            bool vehicleCompError = false;
            if (vehicleComponentDetails.ComponentType > 0 && vehicleType > 0 && !CheckComponentAndConfigTypeMatch(vehicleComponentDetails.ComponentType, vehicleType, current))
            {
                vehicleCompError = true;
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ComponentTypeMismatch"]);
            }

            if (vehicleComponentDetails.ComponentType > 0 && vehicleComponentDetails.ComponentSubType > 0 && !CheckComponentAndComponentSubTypeMatch(vehicleComponentDetails.ComponentType, vehicleComponentDetails.ComponentSubType))
            {
                vehicleCompError = true;
                errorCount++;
                builder.Append(errorCount + ValidationFailure["ComponentSubTypeMismatch"]);
            }
            #endregion

            #region Function Set Comp bool Variable
            if (!vehicleCompError && vehicleComponentDetails.ComponentType > 0 && vehicleComponentDetails.ComponentSubType > 0)
                SetComponentTypeVariable(vehicleComponentDetails.ComponentType, vehicleComponentDetails.ComponentSubType);
            #endregion

            #region Rear Steerable
            if (string.IsNullOrWhiteSpace(vehicleComponents.RearSteerable))
                vehicleComponentDetails.RearSteerable = (int)ExternalApiBitSystem.No;
            else
            {
                try
                {
                    vehicleComponentDetails.RearSteerable = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiBitSystem>(vehicleComponents.RearSteerable.ToLower().Trim());
                    if (!(SemiTrailer || DrawbarTrailer) && vehicleComponentDetails.RearSteerable != (int)ExternalApiBitSystem.No)
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["RearSteerNotRequired"] + MovementClassification + ComponentTypeName + '~');
                    }
                }
                catch (Exception ex)
                {
                    vehicleComponentDetails.RearSteerable = 0;
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidRearSteer"]);
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"ValidateVehicleComponentData : Invalid Rear Steerable Input:{ vehicleComponents.RearSteerable} Exception: { ex.Message} StackTrace { ex.StackTrace}");
                }
            }

            #endregion

            #region Height
            if (vehicleComponents.Height == null)
            {
                if (ConventionalTractor && Vr1Movement && (IsNea || IsFleet))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidComponentHeight"]);
                }
                else
                    vehicleComponentDetails.MaxHeight = vehicleComponents.Height;
            }
            else if (vehicleComponents.Height > 0)
            {
                vehicleComponents.Height = Math.Round((double)vehicleComponents.Height, 2);
                if (!CalculateLength((double)vehicleComponents.Height, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidComponentHeightSize"]);
                }
                else
                {
                    vehicleComponentDetails.MaxHeight = vehicleComponents.Height;
                    vehicleComponentDetails.MaxHeightUnit = DimensionUnitSystem;
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidComponentHeight"]);
            }
            #endregion

            #region Reducible Height
            if (vehicleComponents.ReducibleHeight == null)
                vehicleComponentDetails.ReducibleHeight = vehicleComponents.ReducibleHeight;
            else if (vehicleComponents.ReducibleHeight > 0)
            {
                vehicleComponents.ReducibleHeight = Math.Round((double)vehicleComponents.ReducibleHeight, 2);
                if (vehicleComponents.ReducibleHeight > vehicleComponents.Height)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["ReducibleHeightGreaterthanHeight"]);
                }
                if (!CalculateLength((double)vehicleComponents.ReducibleHeight, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidComponentRedHeightSize"]);

                }
                else
                {
                    vehicleComponentDetails.ReducibleHeight = vehicleComponents.ReducibleHeight;
                    vehicleComponentDetails.ReducibleHeightUnit = DimensionUnitSystem;
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidComponentRedHeight"]);
            }

            #endregion

            #region  Length validation
            if (vehicleComponents.Length == null)
            {
                vehicleComponentDetails.RigidLength = vehicleComponents.Length;
                    
            }
            else if (vehicleComponents.Length > 0)
            {
                vehicleComponents.Length = Math.Round((double)vehicleComponents.Length, 2);
                if (!CalculateLength((double)vehicleComponents.Length, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidComponentLengthSize"]);
                }
                else
                {
                    vehicleComponentDetails.RigidLength = vehicleComponents.Length;
                    vehicleComponentDetails.RigidLengthUnit = DimensionUnitSystem;
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidComponentLength"]);
            }
            #endregion

            #region Width
            if (vehicleComponents.Width == null)
            {
                if (ConventionalTractor && Vr1Movement && (IsNea || IsFleet))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidComponentWidth"]);
                }
                else
                    vehicleComponentDetails.Width = vehicleComponents.Width;
            }
            else if (vehicleComponents.Width > 0)
            {
                vehicleComponents.Width = Math.Round((double)vehicleComponents.Width, 2);
                if (CalculateLength((double)vehicleComponents.Width, DimensionLength))
                {
                    vehicleComponentDetails.Width = vehicleComponents.Width;
                    vehicleComponentDetails.WidthUnit = DimensionUnitSystem;
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidComponentWidthSize"]);
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidComponentWidth"]);
            }
            #endregion

            #region GrossWeight
            if (vehicleComponents.GrossWeight == null && CheckCompGrossWeightNotMandate())
            {
                vehicleComponentDetails.GrossWeight = null;
            }
            else if (vehicleComponents.GrossWeight > 0)
            {
                if (ConventionalTractor)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["CompGrossWeightNotMandate"] + ComponentTypeName + "~");
                }
                else
                {
                    vehicleComponents.GrossWeight = Math.Round((double)vehicleComponents.GrossWeight, 2);
                    if (CalculateLength((double)vehicleComponents.GrossWeight, WeightLength))
                    {
                        vehicleComponentDetails.GrossWeight = vehicleComponents.GrossWeight;
                        vehicleComponentDetails.GrossWeightUnit = WeightUnitSystem;
                    }
                    else
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["GrossWeightSize"]);
                    }
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidGrossWeight"]);
            }
            #endregion

            #region Left Overhang
            if (vehicleComponents.LeftOverhang == null)
                vehicleComponentDetails.LeftOverhang = vehicleComponents.LeftOverhang;
            else if (vehicleComponents.LeftOverhang > 0)
            {
                vehicleComponents.LeftOverhang = Math.Round((double)vehicleComponents.LeftOverhang, 2);
                if (!CalculateLength((double)vehicleComponents.LeftOverhang, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidLeftOverHangSize"]);
                }
                else
                {
                    vehicleComponentDetails.LeftOverhang = vehicleComponents.LeftOverhang;
                    vehicleComponentDetails.LeftOverhangUnit = DimensionUnitSystem;
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidLeftOverHang"]);
            }
            #endregion

            #region Right Overhang
            if (vehicleComponents.RightOverhang == null)
                vehicleComponentDetails.RightOverhang = vehicleComponents.RightOverhang;
            else if (vehicleComponents.RightOverhang > 0)
            {
                vehicleComponents.RightOverhang = Math.Round((double)vehicleComponents.RightOverhang, 2);
                if (!CalculateLength((double)vehicleComponents.RightOverhang, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidRightOverHangSize"]);
                }
                else
                {
                    vehicleComponentDetails.RightOverhang = vehicleComponents.RightOverhang;
                    vehicleComponentDetails.RightOverhangUnit = DimensionUnitSystem;
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidRightOverHang"]);
            }
            #endregion

            #region Front Overhang
            if (vehicleComponents.FrontOverhang == null)
                vehicleComponentDetails.FrontOverhang = vehicleComponents.FrontOverhang;
            else if (vehicleComponents.FrontOverhang > 0)
            {
                if (current != 0)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["FrontOverHangMandate"]);
                }
                else
                {
                    vehicleComponents.FrontOverhang = Math.Round((double)vehicleComponents.FrontOverhang, 2);
                    if (!CalculateLength((double)vehicleComponents.FrontOverhang, DimensionLength))
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["InvalidFrontOverHangSize"]);
                    }
                    else
                    {
                        vehicleComponentDetails.FrontOverhang = vehicleComponents.FrontOverhang;
                        vehicleComponentDetails.FrontOverhangUnit = DimensionUnitSystem;
                    }
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidFrontOverHang"]);
            }
            #endregion

            #region Rear Overhang
            if (vehicleComponents.RearOverhang == null)
                vehicleComponentDetails.RearOverhang = vehicleComponents.RearOverhang;
            else if (vehicleComponents.RearOverhang > 0)
            {
                if (current != totalCount - 1)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["RearOverHangMandate"]);
                }
                else
                {
                    vehicleComponents.RearOverhang = Math.Round((double)vehicleComponents.RearOverhang, 2);
                    if (CalculateLength((double)vehicleComponents.RearOverhang, DimensionLength))
                    {
                        vehicleComponentDetails.RearOverhang = vehicleComponents.RearOverhang;
                        vehicleComponentDetails.RearOverhangUnit = DimensionUnitSystem;
                    }
                    else
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["InvalidRearOverHangSize"]);
                    }
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidRearOverHang"]);
            }
            #endregion

            #region Outside Track
            if (vehicleComponents.OutsideTrack > 0)
            {
                vehicleComponents.OutsideTrack = Math.Round((double)vehicleComponents.OutsideTrack, 2);
                if (CheckOutsideTrackNotMandate())
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["OutsideTrackNotMandate"] + MovementClassification + ComponentTypeName + '~');
                }
                else if (!CalculateLength((double)vehicleComponents.OutsideTrack, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidOutsideTrackSize"]);

                }
                else
                {
                    vehicleComponentDetails.OutsideTrack = vehicleComponents.OutsideTrack;
                    vehicleComponentDetails.OutsideTrackUnit = DimensionUnitSystem;
                }
            }
            else if (CheckOutSideTrackInvalid() && !IsNen)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidOutsideTrack"]);
            }
            #endregion

            #region Axle count validation
            if (vehicleComponents.AxleCount > 0)
            {
                vehicleComponentDetails.AxleCount = vehicleComponents.AxleCount;
                if (TrackedVehicle)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["AxleCountNotMandate"] + MovementClassification + ComponentTypeName + "~");
                }
            }
            else if (!(TrackedVehicle || WcuMovement))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidAxleCount"]);
            }
            #endregion

            #region Reducible Ground Clearance
            if (vehicleComponents.ReducibleGroundClearance == null)
                vehicleComponentDetails.ReducibleGroundClearance = vehicleComponents.ReducibleGroundClearance;
            else if (vehicleComponents.ReducibleGroundClearance > 0)
            {
                vehicleComponents.ReducibleGroundClearance = Math.Round((double)vehicleComponents.ReducibleGroundClearance, 2);
                if (CheckGCAndRGCNotMandate())
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["ReducibleGroundClearanceNotMandate"] + MovementClassification + ComponentTypeName + "~");
                }
                else if (!CalculateLength((double)vehicleComponents.ReducibleGroundClearance, DimensionLength))
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidReducibleGroundClearanceSize"]);
                }
                else
                {
                    vehicleComponentDetails.ReducibleGroundClearance = vehicleComponents.ReducibleGroundClearance;
                    vehicleComponentDetails.ReducibleGroundClearanceUnit = DimensionUnitSystem;
                }
            }
            else
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidReducibleGroundClearance"]);
            }
            #endregion

            #region Axle Space to following
            if (current < totalCount - 1)
            {
                if (vehicleComponents.AxleSpacingToFollowing == null && ( TrackedVehicle || (WcuMovement && (vehicleComponentDetails.AxleCount == null ||
                    vehicleComponents.Axles == null))))
                    vehicleComponentDetails.AxleSpacingToFollowing = null;
                else if (vehicleComponents.AxleSpacingToFollowing > 0)
                {
                    vehicleComponents.AxleSpacingToFollowing = Math.Round((double)vehicleComponents.AxleSpacingToFollowing, 2);
                    if (!CalculateLength((double)vehicleComponents.AxleSpacingToFollowing, DimensionLength))
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["InvalidAxleSpacingToFollowingSize"]);
                    }
                    else
                        vehicleComponentDetails.AxleSpacingToFollowing = vehicleComponents.AxleSpacingToFollowing;
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidAxleSpacingToFollowing"]);
                }
            }
            else if (current == totalCount - 1) //last component
            {
                if (vehicleComponents.AxleSpacingToFollowing == null)
                    vehicleComponentDetails.AxleSpacingToFollowing = vehicleComponents.AxleSpacingToFollowing;
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidAxleSpacingToFollowing"]);
                }
            }
            vehicleComponentDetails.AxleSpacingToFollowingUnit = DimensionUnitSystem;
            #endregion

            #region Axle config validation
            double? wheelBase = null;
            double? maxAxleWeigth = null;
            double? grossWeight = null;
            bool axleError = false;
            if (vehicleComponents.Axles == null)
            {
                if (WcuMovement || TrackedVehicle)
                    vehicleComponentDetails.Axles = null;
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["AxleDetailsMandate"] + MovementClassification + ComponentTypeName + "~");
                }
            }
            else
            {
                if (vehicleComponentDetails.AxleCount != vehicleComponents.Axles.Count)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["AxleCountMisMatch"]);
                }

                if (vehicleComponents.Axles != null)
                {
                    if (TrackedVehicle)
                    {
                        errorCount++;
                        builder.Append(errorCount + ValidationFailure["AxleDetailsNotMandate"] + MovementClassification + ComponentTypeName + "~");
                    }
                    else
                    {
                        var totalAxleCount = vehicleComponents.Axles.Count;
                        int currentAxleIndex = 1;
                        foreach (var item in vehicleComponents.Axles)
                        {
                            #region Axle Number
                            if (item.AxleNumber < 0 || (item.AxleNumber == 0 && !WcuMovement))
                            {
                                axleError = true;
                                errorCount++;
                                builder.Append(errorCount + ValidationFailure["InvalidAxleNumber"]);
                            }
                            #endregion

                            #region No of Wheels
                            if (item.NoOfWheels < 0 || (item.NoOfWheels == 0 && !WcuMovement))
                            {
                                axleError = true;
                                errorCount++;
                                builder.Append(errorCount + ValidationFailure["InvalidNoOfWheels"] + item.AxleNumber + "~");
                            }
                            #endregion

                            #region Axle Weight
                            if (item.AxleWeight > 0)
                            {
                                item.AxleWeight = Math.Round((double)item.AxleWeight, 2);
                                if (!CalculateLength((double)item.AxleWeight, WeightLength))
                                {
                                    axleError = true;
                                    errorCount++;
                                    builder.Append(errorCount + ValidationFailure["InvalidAxleWeightSize"] + item.AxleNumber + "~");
                                }
                            }
                            else if (!WcuMovement)
                            {
                                axleError = true;
                                errorCount++;
                                builder.Append(errorCount + ValidationFailure["InvalidAxleWeight"] + item.AxleNumber + "~");
                            }
                            #endregion

                            #region Distance To Next Axle
                            if (currentAxleIndex < totalAxleCount)
                            {
                                if (item.DistanceToNextAxle > 0)
                                {
                                    item.DistanceToNextAxle = Math.Round((double)item.DistanceToNextAxle, 2);
                                    if (!CalculateLength((double)item.DistanceToNextAxle, DimensionLength))
                                    {
                                        axleError = true;
                                        errorCount++;
                                        builder.Append(errorCount + ValidationFailure["InvalidDistanceToNextAxleSize"] + item.AxleNumber + "~");
                                    }
                                }
                                else if (!WcuMovement)
                                {
                                    axleError = true;
                                    errorCount++;
                                    builder.Append(errorCount + ValidationFailure["InvalidDistanceToNextAxle"] + item.AxleNumber + "~");
                                }
                            }
                            else if (currentAxleIndex == totalAxleCount && current < totalCount - 1)
                            {
                                if (item.DistanceToNextAxle != null && item.DistanceToNextAxle != vehicleComponents.AxleSpacingToFollowing)
                                {
                                    axleError = true;
                                    errorCount++;
                                    builder.Append(errorCount + ValidationFailure["MismatchInDistanceToNextAxle"]);
                                }
                                else
                                    item.DistanceToNextAxle = vehicleComponentDetails.AxleSpacingToFollowing;
                            }
                            else if (currentAxleIndex == totalAxleCount && current == totalCount - 1 && item.DistanceToNextAxle != null && !WcuMovement)
                            {
                                axleError = true;
                                errorCount++;
                                builder.Append(errorCount + ValidationFailure["InvalidDistanceToNextAxleForLast"]);
                            }
                            #endregion

                            #region Wheel Spacing
                            if (item.WheelSpacing != null)
                            {
                                if (CheckTyreSizeAndSpacingNotmandate())
                                {
                                    errorCount++;
                                    builder.Append(errorCount + ValidationFailure["WheelSpacingNotMandate"] + MovementClassification + ComponentTypeName + "~");
                                }
                                else
                                {
                                    if (item.WheelSpacing.Count != item.NoOfWheels - 1)
                                    {
                                        axleError = true;
                                        errorCount++;
                                        builder.Append(errorCount + ValidationFailure["MismatchInWheelSpacingCount"] + item.AxleNumber + "~");
                                    }
                                    else if (item.WheelSpacing.Exists(x => x == null || x == 0))
                                    {
                                        axleError = true;
                                        errorCount++;
                                        builder.Append(errorCount + ValidationFailure["InvalidWheelSpacing"] + item.AxleNumber + "~");
                                    }
                                }
                            }
                            else if (SoMovement && !IsNen)
                            {
                                axleError = true;
                                errorCount++;
                                builder.Append(errorCount + ValidationFailure["WheelSpacingMandate"] + MovementClassification + ComponentTypeName + "~");
                            }
                            #endregion

                            #region Tyre Size
                            if (string.IsNullOrWhiteSpace(item.TyreSize) && SoMovement && !IsNen)
                            {
                                axleError = true;
                                errorCount++;
                                builder.Append(errorCount + ValidationFailure["TyreSizeMandate"] + MovementClassification + ComponentTypeName + " for Axle "+ item.AxleNumber + "~");
                            }
                            else if (!string.IsNullOrWhiteSpace(item.TyreSize) && CheckTyreSizeAndSpacingNotmandate())
                            {
                                axleError = true;
                                errorCount++;
                                builder.Append(errorCount + ValidationFailure["TyreSizeNoMandate"] + MovementClassification + ComponentTypeName + "~");
                            }
                            #endregion

                            currentAxleIndex++;
                        }
                    }
                }

                #region Create Axle Details
                List<Axles> axleList = new List<Axles>();
                if (!axleError)
                {
                    foreach (var item in vehicleComponents.Axles)
                    {
                        Axles axle = new Axles
                        {
                            AxleNumber = item.AxleNumber,
                            NoOfWheels = item.NoOfWheels,
                            AxleWeight = (double)item.AxleWeight,
                            DistanceToNextAxle = Convert.ToDouble(item.DistanceToNextAxle),
                            TyreSize = item.TyreSize
                        };
                        if (item.WheelSpacing != null)
                            axle.TyreSpacing = string.Join(",", item.WheelSpacing);

                        axleList.Add(axle);
                    }
                    wheelBase = axleList.Sum(x => x.DistanceToNextAxle);
                    maxAxleWeigth = axleList.Max(x => x.AxleWeight);
                    grossWeight = axleList.Sum(x => x.AxleWeight);
                }
                #endregion

                vehicleComponentDetails.Axles = axleList;
            }
            #endregion

            #region ComponentRegistration
            if (vehicleComponents.ComponentRegistrations != null)
            {
                var componentRegList = JsonConvert.SerializeObject(vehicleComponents.ComponentRegistrations);
                vehicleComponentDetails.ComponentRegistration = JsonConvert.DeserializeObject<List<Registrations>>(componentRegList);
            }
            #endregion

            #region Auto Calculation Value
            vehicleComponentDetails.WheelBase = wheelBase;
            vehicleComponentDetails.MaxAxleWeight = maxAxleWeigth;
            vehicleComponentDetails.MaxAxleWeightUnit = DimensionUnitSystem;
            vehicleComponentDetails.WheelBaseUnit = DimensionUnitSystem;
            if(grossWeight != null && grossWeight > vehicleComponentDetails.GrossWeight)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["AxleWeightExceed"]);
            }
            #endregion

            #region Ground Clearance
            if (vehicleComponents.GroundClearance > 0)
            {
                vehicleComponents.GroundClearance = Math.Round((double)vehicleComponents.GroundClearance, 2);
                if (CheckGCAndRGCNotMandate())
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["GroundClearanceNotMandate"] + MovementClassification + ComponentTypeName + '~');
                }
                else if (CalculateLength((double)vehicleComponents.GroundClearance, DimensionLength))
                {
                    vehicleComponentDetails.GroundClearance = vehicleComponents.GroundClearance;
                    vehicleComponentDetails.GroundClearanceUnit = DimensionUnitSystem;
                }
                else
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["InvalidGroundClearanceSize"]);
                }
            }
            else if (CheckGCInavlid(grossWeight) && !IsNen)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["InvalidGroundClearance"]);
            }
            #endregion

            #region Error Count
            if (errorCount > 0)
            {
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = builder.ToString().Trim()
                };
                vehicleComponentDetails.VehicleComponentError = validationError;
            }
            #endregion

            return vehicleComponentDetails;
        }
        
        #endregion

        #region Private Methods

        #region Function to check matching of Component coupling type with somponent sub type
        private static void GetCouplingType(VehicleComponentDetails vehicleComponentDetails, string componentType, out string message)
        {
            var couplingType = ExternalApiCouplingType.None;
            message = string.Empty;
            switch (vehicleComponentDetails.ComponentType)
            {
                case (int)ExternalApiComponentType.CT002:
                case (int)ExternalApiComponentType.CT005:
                case (int)ExternalApiComponentType.CT010:
                    couplingType = ExternalApiCouplingType.FifthWheel;
                    break;
                case (int)ExternalApiComponentType.CT001:
                case (int)ExternalApiComponentType.CT006:
                case (int)ExternalApiComponentType.CT011:
                case (int)ExternalApiComponentType.CT013:
                    couplingType = ExternalApiCouplingType.Drawbar;
                    break;
                case (int)ExternalApiComponentType.CT003:
                case (int)ExternalApiComponentType.CT007:
                case (int)ExternalApiComponentType.CT004:
                case (int)ExternalApiComponentType.CT008:
                case (int)ExternalApiComponentType.CT012:
                    couplingType = ExternalApiCouplingType.None;
                    break;
                case (int)ExternalApiComponentType.CT009:
                    if (vehicleComponentDetails.ComponentSubType == (int)ExternalApiComponentSubType.CST021 ||
                        vehicleComponentDetails.ComponentSubType == (int)ExternalApiComponentSubType.CST025)
                        couplingType = ExternalApiCouplingType.FifthWheel;
                    else if (vehicleComponentDetails.ComponentSubType == (int)ExternalApiComponentSubType.CST024 ||
                        vehicleComponentDetails.ComponentSubType == (int)ExternalApiComponentSubType.CST026)
                        couplingType = ExternalApiCouplingType.Drawbar;
                    else
                        couplingType = ExternalApiCouplingType.None;
                    break;
            }
            if (vehicleComponentDetails.CouplingType != (int)couplingType)
                message = ". For Component " + componentType + ",coupling type should be " + couplingType.GetEnumDescription();
        }
        #endregion

        #region Vehicle Config Calculation Validation with Components
        private void AutoCalculateValue(ref VehicleImportModel vehicleImportModel)
        {
            double? grossWeight = vehicleImportModel.VehicleComponentDetails.Sum(x => x.GrossWeight);
            double? overAllLength = vehicleImportModel.VehicleComponentDetails.Sum(x => x.RigidLength != null ? x.RigidLength : null);
            double? wheelBase = vehicleImportModel.VehicleComponentDetails.Sum(x => x.WheelBase != null ? x.WheelBase : null);

            double? maxAxleWeight = vehicleImportModel.VehicleComponentDetails.Max(x => x.MaxAxleWeight != null ? x.MaxAxleWeight : null);
            double? maxWidth = vehicleImportModel.VehicleComponentDetails.Max(x => x.Width != null ? x.Width : null);
            double? maxRigidLength = vehicleImportModel.VehicleComponentDetails.Max(x => x.RigidLength != null ? x.RigidLength : null);
            double? maxHeight = vehicleImportModel.VehicleComponentDetails.Max(x => x.MaxHeight != null ? x.MaxHeight : null);

            StringBuilder builder = new StringBuilder();
            string errorMessage = vehicleImportModel.VehicleConfigDetails.VehicleConfigError != null ? vehicleImportModel.VehicleConfigDetails.VehicleConfigError.ErrorMessage : string.Empty;
            builder.Append(errorMessage);
            int errorCount = 0;
            builder.Append(ValidationFailure["VehicleCalculationError"] + vehicleImportModel.VehicleConfigDetails.InternalName + "~");

            #region Gross Weight
            if (vehicleImportModel.VehicleConfigDetails.VehicleType == (int)ExternalApiVehicleType.VT001 ||
                vehicleImportModel.VehicleConfigDetails.VehicleType == (int)ExternalApiVehicleType.VT008)
            {
                double? maxGrossWeight = vehicleImportModel.VehicleComponentDetails.Max(x => x.GrossWeight != null ? x.GrossWeight : null);
                wheelBase = vehicleImportModel.VehicleComponentDetails.Find(x => x.GrossWeight == maxGrossWeight).WheelBase;
                if (maxGrossWeight != null && vehicleImportModel.VehicleConfigDetails.GrossWeight != maxGrossWeight)
                {
                    errorCount++;
                    builder.Append(errorCount + ValidationFailure["DrawbarGrossWeightInCorrect"]);
                }
            }
            else if (vehicleImportModel.VehicleConfigDetails.GrossWeight < grossWeight)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["TotalGrossWeightLesser"]);
            }
            #endregion

            #region Width
            if (maxWidth != null && vehicleImportModel.VehicleConfigDetails.Width != maxWidth)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["MismatchInOverAllWidth"]);
            }
            #endregion

            #region Height
            if (maxHeight != null && vehicleImportModel.VehicleConfigDetails.MaxHeight != maxHeight)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["MismatchInMaxHeight"]);
            }
            #endregion

            #region Length
            
            if (overAllLength != null && vehicleImportModel.VehicleConfigDetails.OverallLength < overAllLength)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["GreaterCompLength"]);
            }
            if (wheelBase != null && vehicleImportModel.VehicleConfigDetails.OverallLength < wheelBase)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["WheelBaseGreater"]);
            }

            if (maxRigidLength != null && vehicleImportModel.VehicleConfigDetails.OverallLength < maxRigidLength)
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["GreaterCompRigidLength"]);
            }
            if (maxRigidLength != null && vehicleImportModel.VehicleConfigDetails.RigidLength != maxRigidLength && !(TrackedVehicle || MobileCrane || RecoveryVehicle))
            {
                errorCount++;
                builder.Append(errorCount + ValidationFailure["MismatchInRigidLength"]);
            }
            #endregion

            if (errorCount > 0)
            {
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount + (vehicleImportModel.VehicleConfigDetails.VehicleConfigError != null ? vehicleImportModel.VehicleConfigDetails.VehicleConfigError.ErrorCount : 0),
                    ErrorMessage = builder.ToString()
                };
                vehicleImportModel.VehicleConfigDetails.VehicleConfigError = validationError;
            }
            vehicleImportModel.VehicleConfigDetails.WheelBase = wheelBase;
            vehicleImportModel.VehicleConfigDetails.MaxAxleWeight = maxAxleWeight;
            if (maxAxleWeight != null)
                vehicleImportModel.VehicleConfigDetails.MaxAxleWeightKg = GetConvertedValues(WeightUnitSystem, Math.Round((double)maxAxleWeight, 4));

            vehicleImportModel.VehicleConfigDetails.WheelBaseUnit = DimensionUnitSystem;
            vehicleImportModel.VehicleConfigDetails.MaxAxleWeightUnit = WeightUnitSystem;
        }
        #endregion

        #region Function to check Macthing of component type with Componenent Sub type
        private bool CheckComponentAndComponentSubTypeMatch(int componentType, int componentSubType)
        {
            bool result = false;
            switch (componentType)
            {
                case (int)ExternalApiComponentType.CT001:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST001 || componentSubType == (int)ExternalApiComponentSubType.CST003)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT002:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST002)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT003:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST013)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT004:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST012)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT005:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST004 || componentSubType == (int)ExternalApiComponentSubType.CST005 ||
                            componentSubType == (int)ExternalApiComponentSubType.CST006 || componentSubType == (int)ExternalApiComponentSubType.CST007 ||
                            componentSubType == (int)ExternalApiComponentSubType.CST010 || componentSubType == (int)ExternalApiComponentSubType.CST029)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT006:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST008 || componentSubType == (int)ExternalApiComponentSubType.CST009 ||
                            componentSubType == (int)ExternalApiComponentSubType.CST011)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT007:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST014)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT008:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST019)
                        {
                            result = true;
                        }
                        break;
                    }
                case (int)ExternalApiComponentType.CT009:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST021 || componentSubType == (int)ExternalApiComponentSubType.CST022 ||
                            componentSubType == (int)ExternalApiComponentSubType.CST023 || componentSubType == (int)ExternalApiComponentSubType.CST024)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT010:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST025)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT011:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST026)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT012:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST027)
                            result = true;
                        break;
                    }
                case (int)ExternalApiComponentType.CT013:
                    {
                        if (componentSubType == (int)ExternalApiComponentSubType.CST028)
                            result = true;
                        break;
                    }
                default:
                    result = false;
                    break;
            }
            return result;
        }
        #endregion

        #region Function to check matching of component type with vehicle config type
        
        private bool CheckComponentAndConfigTypeMatch(int componentType, int vehicleType, int current)
        {
            bool result = false;
            InitalizeVehicleTypeVaribale();
            switch (vehicleType)
            {
                case (int)ExternalApiVehicleType.VT001:
                case (int)ExternalApiVehicleType.VT008:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT001 || componentType == (int)ExternalApiComponentType.CT006 ||
                            componentType == (int)ExternalApiComponentType.CT009 || componentType == (int)ExternalApiComponentType.CT011 ||
                            componentType == (int)ExternalApiComponentType.CT013)
                            result = true;
                        DrawbarVehicle = true;
                        VehicleTypeName = ValidationFailure["DrawbarVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT002:
                case (int)ExternalApiVehicleType.VT007:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT002 || componentType == (int)ExternalApiComponentType.CT005 ||
                            componentType == (int)ExternalApiComponentType.CT009 || componentType == (int)ExternalApiComponentType.CT010)
                            result = true;
                        SemiVehicle = true;
                        VehicleTypeName = ValidationFailure["SemiVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT003:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT003 || componentType == (int)ExternalApiComponentType.CT009)
                            result = true;
                        RigidVhcl = true;
                        VehicleTypeName = ValidationFailure["RigidVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT004:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT004 || componentType == (int)ExternalApiComponentType.CT009)
                            result = true;
                        Tracked = true;
                        VehicleTypeName = ValidationFailure["TrackedVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT005:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT007)
                            result = true;
                        SpmtVehicle = true;
                        VehicleTypeName = ValidationFailure["Spmt"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT006:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT002 || componentType == (int)ExternalApiComponentType.CT003 ||
                            componentType == (int)ExternalApiComponentType.CT005 || componentType == (int)ExternalApiComponentType.CT006)
                            result = true;
                        SemiVehicle = true;
                        VehicleTypeName = ValidationFailure["SemiVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT009:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT003 || componentType == (int)ExternalApiComponentType.CT006)
                            result = true;
                        RigidAndDrag = true;
                        VehicleTypeName = ValidationFailure["RigidAndDrag"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT010:
                    {
                        if (componentType == (int)ExternalApiComponentType.CT008)
                            result = true;
                        MobileCraneVehicle = true;
                        VehicleTypeName = ValidationFailure["MobileCrane"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT011:
                    {
                        if (current > 0)
                            result = true;
                        else if (current == 0 && componentType == (int)ExternalApiComponentType.CT012)
                            result = true;
                        RecoveryVhcl = true;
                        VehicleTypeName = ValidationFailure["RecoveryVehicle"];
                        break;
                    }
                default:
                    result = false;
                    break;
            }
            return result;
        }
        #endregion

        #region Function to convert Imperial Systm to metric
        private double GetConvertedValues(int unit, double value)
        {
            switch (unit)
            {
                case (int)ExternalApiDimensionUnitSystem.Imperial:
                    value *= 0.304800610;
                    break;
                case (int)ExternalApiWeightUnitSystem.Imperial:
                    value *= 1.60934;
                    break;
                case (int)ExternalApiSpeedUnitSystem.Imperial:
                    value *= 1.60934;
                    break;
            }
            return value;
        }
        #endregion

        #region Function for Calcualte Length using formula and Regex Matching
        private bool CalculateLength(double value, int length)
        {
            return (int)Math.Floor(Math.Log10(value)) + 1 <= length;
        }
#pragma warning disable S1144 // Unused private types or members should be removed
        private bool CalculateRegexLength(double? value, string regex)
#pragma warning restore S1144 // Unused private types or members should be removed
        {
            return Regex.Match(value.ToString(), regex).Success;
        }
        #endregion

        #region Set Bool Variable
        private void SetMovementClassVariable(int movementClassification)
        {
            InitializeMovementClassVariable();
            switch (movementClassification)
            {
                case (int)ExternalApiMovementClassificationMapping.MC102:
                    WcuMovement = true;
                    MovementClassification = ValidationFailure["WcuMovement"];
                    break;
                case (int)ExternalApiMovementClassificationMapping.MC103:
                case (int)ExternalApiMovementClassificationMapping.MC104:
                case (int)ExternalApiMovementClassificationMapping.MC105:
                case (int)ExternalApiMovementClassificationMapping.MC106:
                case (int)ExternalApiMovementClassificationMapping.MC107:
                case (int)ExternalApiMovementClassificationMapping.MC108:
                case (int)ExternalApiMovementClassificationMapping.MC109:
                case (int)ExternalApiMovementClassificationMapping.MC126:
                case (int)ExternalApiMovementClassificationMapping.MC127:
                case (int)ExternalApiMovementClassificationMapping.MC128:
                case (int)ExternalApiMovementClassificationMapping.MC129:
                case (int)ExternalApiMovementClassificationMapping.MC130:
                case (int)ExternalApiMovementClassificationMapping.MC131:
                case (int)ExternalApiMovementClassificationMapping.MC132:
                case (int)ExternalApiMovementClassificationMapping.MC133:
                case (int)ExternalApiMovementClassificationMapping.MC134:
                case (int)ExternalApiMovementClassificationMapping.MC135:
                case (int)ExternalApiMovementClassificationMapping.MC136:
                case (int)ExternalApiMovementClassificationMapping.MC137:
                case (int)ExternalApiMovementClassificationMapping.MC138:
                case (int)ExternalApiMovementClassificationMapping.MC139:
                case (int)ExternalApiMovementClassificationMapping.MC140:
                case (int)ExternalApiMovementClassificationMapping.MC141:
                case (int)ExternalApiMovementClassificationMapping.MC142:
                case (int)ExternalApiMovementClassificationMapping.MC143:
                case (int)ExternalApiMovementClassificationMapping.MC144:
                case (int)ExternalApiMovementClassificationMapping.MC145:
                case (int)ExternalApiMovementClassificationMapping.MC146:
                case (int)ExternalApiMovementClassificationMapping.MC147:
                case (int)ExternalApiMovementClassificationMapping.MC148:
                case (int)ExternalApiMovementClassificationMapping.MC149:
                case (int)ExternalApiMovementClassificationMapping.MC150:
                case (int)ExternalApiMovementClassificationMapping.MC151:
                case (int)ExternalApiMovementClassificationMapping.MC152:
                case (int)ExternalApiMovementClassificationMapping.MC153:
                case (int)ExternalApiMovementClassificationMapping.MC157:
                    StgoMovement = true;
                    MovementClassification = ValidationFailure["StgoMovement"];
                    break;
                case (int)ExternalApiMovementClassificationMapping.MC110:
                case (int)ExternalApiMovementClassificationMapping.MC111:
                case (int)ExternalApiMovementClassificationMapping.MC112:
                case (int)ExternalApiMovementClassificationMapping.MC156:
                case (int)ExternalApiMovementClassificationMapping.MC158:
                case (int)ExternalApiMovementClassificationMapping.MC159:
                case (int)ExternalApiMovementClassificationMapping.MC160:
                case (int)ExternalApiMovementClassificationMapping.MC161:
                case (int)ExternalApiMovementClassificationMapping.MC162:
                case (int)ExternalApiMovementClassificationMapping.MC163:
                case (int)ExternalApiMovementClassificationMapping.MC164:
                case (int)ExternalApiMovementClassificationMapping.MC165:
                case (int)ExternalApiMovementClassificationMapping.MC166:
                case (int)ExternalApiMovementClassificationMapping.MC167:
                case (int)ExternalApiMovementClassificationMapping.MC168:
                case (int)ExternalApiMovementClassificationMapping.MC169:
                case (int)ExternalApiMovementClassificationMapping.MC170:
                case (int)ExternalApiMovementClassificationMapping.MC171:
                case (int)ExternalApiMovementClassificationMapping.MC172:
                case (int)ExternalApiMovementClassificationMapping.MC173:
                case (int)ExternalApiMovementClassificationMapping.MC174:
                case (int)ExternalApiMovementClassificationMapping.MC175:
                case (int)ExternalApiMovementClassificationMapping.MC176:
                case (int)ExternalApiMovementClassificationMapping.MC177:
                case (int)ExternalApiMovementClassificationMapping.MC178:
                    Vr1Movement = true;
                    MovementClassification = ValidationFailure["Vr1Movement"];
                    break;
                case (int)ExternalApiMovementClassificationMapping.MC118:
                case (int)ExternalApiMovementClassificationMapping.MC119:
                    CraneMovement = true;
                    MovementClassification = ValidationFailure["CAT_A"];
                    break;
                case (int)ExternalApiMovementClassificationMapping.MC120:
                case (int)ExternalApiMovementClassificationMapping.MC121:
                case (int)ExternalApiMovementClassificationMapping.MC122:
                    CraneMovement = true;
                    MovementClassification = ValidationFailure["CAT_B"];
                    break;
                case (int)ExternalApiMovementClassificationMapping.MC123:
                case (int)ExternalApiMovementClassificationMapping.MC124:
                case (int)ExternalApiMovementClassificationMapping.MC125:
                    CraneMovement = true;
                    MovementClassification = ValidationFailure["CAT_C"];
                    break;
                case (int)ExternalApiMovementClassificationMapping.MC114:
                case (int)ExternalApiMovementClassificationMapping.MC116:
                case (int)ExternalApiMovementClassification.MC010://For Fleet Vehicle
                    SoMovement = true;
                    MovementClassification = ValidationFailure["SoMovement"];
                    break;
                case (int)ExternalApiMovementClassificationMapping.MC113:
                    VsoMovement = true;
                    MovementClassification = ValidationFailure["VsoMovement"];
                    break;
            }
        }
        private void InitializeMovementClassVariable()
        {
            WcuMovement = false;
            StgoMovement = false;
            Vr1Movement = false;
            SoMovement = false;
            VsoMovement = false;
            MovementClassification = string.Empty;
        }
        private void InitalizeVehicleTypeVaribale()
        {
            RigidVhcl = false;
            SemiVehicle = false;
            DrawbarVehicle = false;
            RigidAndDrag = false;
            SpmtVehicle = false;
            MobileCraneVehicle = false;
            Tracked = false;
            RecoveryVhcl = false;
            VehicleTypeName = string.Empty;
        }
        private void SetVehicleTypeVariable(int vehicleType)
        {
            InitalizeVehicleTypeVaribale();
            switch (vehicleType)
            {
                case (int)ExternalApiVehicleType.VT001:
                case (int)ExternalApiVehicleType.VT008:
                    {
                        DrawbarVehicle = true;
                        VehicleTypeName = ValidationFailure["DrawbarVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT002:
                case (int)ExternalApiVehicleType.VT007:
                case (int)ExternalApiVehicleType.VT006:
                    {
                        SemiVehicle = true;
                        VehicleTypeName = ValidationFailure["SemiVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT003:
                    {
                        RigidVhcl = true;
                        VehicleTypeName = ValidationFailure["RigidVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT004:
                    {
                        VehicleTypeName = ValidationFailure["TrackedVehicle"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT005:
                    {
                        SpmtVehicle = true;
                        VehicleTypeName = ValidationFailure["Spmt"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT009:
                    {
                        RigidAndDrag = true;
                        VehicleTypeName = ValidationFailure["RigidAndDrag"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT010:
                    {
                        MobileCraneVehicle = true;
                        VehicleTypeName = ValidationFailure["MobileCrane"];
                        break;
                    }
                case (int)ExternalApiVehicleType.VT011:
                    {
                        RecoveryVhcl = true;
                        VehicleTypeName = ValidationFailure["RecoveryVehicle"];
                        break;
                    }
            }
        }
        private void SetComponentTypeVariable(int componentType, int componentSubType)
        {
            InitalizeComponentTypeVaribale();
            switch (componentType)
            {
                case (int)ExternalApiComponentType.CT001:
                    BallastTractor = true;
                    ComponentTypeName = ValidationFailure["BallastTractor"];
                    break;
                case (int)ExternalApiComponentType.CT002:
                    ConventionalTractor = true;
                    ComponentTypeName = ValidationFailure["ConventionalTractor"];
                    break;
                case (int)ExternalApiComponentType.CT003:
                    RigidVehicle = true;
                    ComponentTypeName = ValidationFailure["RigidVehicle"];
                    break;
                case (int)ExternalApiComponentType.CT004:
                    TrackedVehicle = true;
                    ComponentTypeName = ValidationFailure["TrackedVehicle"];
                    break;
                case (int)ExternalApiComponentType.CT005:
                    SemiTrailer = true;
                    ComponentTypeName = ValidationFailure["SemiTrailer"];
                    break;
                case (int)ExternalApiComponentType.CT010:
                    SemiTrailer = true;
                    ComponentTypeName = ValidationFailure["EngPlantSemiTrailer"];
                    break;
                case (int)ExternalApiComponentType.CT006:
                    DrawbarTrailer = true;
                    ComponentTypeName = ValidationFailure["DrawbarTrailer"];
                    break;
                case (int)ExternalApiComponentType.CT011:
                    DrawbarTrailer = true;
                    ComponentTypeName = ValidationFailure["EngPlantDrawbarTrailer"];
                    break;
                case (int)ExternalApiComponentType.CT013:
                    DrawbarTrailer = true;
                    ComponentTypeName = ValidationFailure["GirderSet"];
                    break;
                case (int)ExternalApiComponentType.CT007:
                    Spmt = true;
                    ComponentTypeName = ValidationFailure["Spmt"];
                    break;
                case (int)ExternalApiComponentType.CT008:
                    MobileCrane = true;
                    ComponentTypeName = ValidationFailure["MobileCrane"];
                    break;
                case (int)ExternalApiComponentType.CT009:
                    switch (componentSubType)
                    {
                        case (int)ExternalApiComponentSubType.CST021:
                            ConventionalTractor = true;
                            ComponentTypeName = ValidationFailure["EngPlantConventionalTractor"];
                            break;
                        case (int)ExternalApiComponentSubType.CST022:
                            RigidVehicle = true;
                            ComponentTypeName = ValidationFailure["EngPlantRigidVehicle"];
                            break;
                        case (int)ExternalApiComponentSubType.CST023:
                            TrackedVehicle = true;
                            ComponentTypeName = ValidationFailure["EngPlantTrackedVehicle"];
                            break;
                        case (int)ExternalApiComponentSubType.CST024:
                            BallastTractor = true;
                            ComponentTypeName = ValidationFailure["EngPlantBallastTractor"];
                            break;
                        case (int)ExternalApiComponentSubType.CST025:
                            SemiTrailer = true;
                            ComponentTypeName = ValidationFailure["EngPlantSemiTrailer"];
                            break;
                        case (int)ExternalApiComponentSubType.CST026:
                            DrawbarTrailer = true;
                            ComponentTypeName = ValidationFailure["EngPlantDrawbarTrailer"];
                            break;
                    }
                    break;
                case (int)ExternalApiComponentType.CT012:
                    RecoveryVehicle = true;
                    ComponentTypeName = ValidationFailure["RecoveryVehicle"];
                    break;
            }
        }
        private void InitalizeComponentTypeVaribale()
        {
            BallastTractor = false;
            ConventionalTractor = false;
            RigidVehicle = false;
            TrackedVehicle = false;
            SemiTrailer = false;
            DrawbarTrailer = false;
            Spmt = false;
            MobileCrane = false;
            RecoveryVehicle = false;
            ComponentTypeName = string.Empty;
        }
        private void SetVehicleComponentVariable(VehicleValidateInputModel vehicleValidateInputModel)
        {
            var component = vehicleValidateInputModel.Vehicle.VehicleComponents != null ?
                vehicleValidateInputModel.Vehicle.VehicleComponents.OrderBy(x => x.ComponentNumber).FirstOrDefault() : null;
            if (component != null)
            {
                int compType = 0;
                int compSubType = 0;
                try
                {
                    compType = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiComponentType>(component.ComponentType.ToLower().Trim());
                    compSubType = (int)EnumExtensionMethods.GetValueFromDescription<ExternalApiComponentSubType>(component.ComponentSubType.ToLower().Trim());
                }
                catch
                {
                    compType = 0;
                    compSubType = 0;
                }
                SetComponentTypeVariable(compType, compSubType);
            }
        }
        private bool CheckGCInavlid(double? grossWeight)
        {
            if ((RigidVehicle || Spmt || TrackedVehicle) && SoMovement)
                return true;
            if ((SemiTrailer || DrawbarTrailer) && grossWeight > 150000 && SoMovement)
                return true;
            if (BallastTractor && VsoMovement)
                return true;
            return false;
        }
        private bool CheckGCAndRGCNotMandate()
        {
            return MobileCrane || RecoveryVehicle || ConventionalTractor;
        }
        private bool CheckOutSideTrackInvalid()
        {
            return (RigidVehicle || Spmt || TrackedVehicle || SemiTrailer || DrawbarTrailer) && (SoMovement || VsoMovement) || (BallastTractor && VsoMovement);
        }
        private bool CheckOutsideTrackNotMandate()
        {
            return MobileCrane || (RigidVehicle && WcuMovement) || (BallastTractor && (WcuMovement || StgoMovement)) || ConventionalTractor || RecoveryVehicle;
        }
        private bool CheckTyreSizeAndSpacingNotmandate()
        {
            return WcuMovement || StgoMovement || MobileCrane || RecoveryVehicle || TrackedVehicle;
        }
        private bool RigidLengthNotMandate()
        {
            return MobileCrane || Spmt || TrackedVehicle || RigidVehicle;
        }
        private bool CheckCompGrossWeightNotMandate()
        {
            return ConventionalTractor || TrackedVehicle || (SemiTrailer && !VsoMovement) || RecoveryVehicle;
        }
        #endregion

        #endregion
    }
    #endregion
}
