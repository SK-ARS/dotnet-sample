using STP.Common.Logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace STP.VehiclesAndFleets.Models.VehicleAPI
{
    public class VehicleValidation
    {
        private static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];
        public VehicleImportModel ValidateVehicleData(Vehicle vehicle)
        {
            VehicleImportModel vehicleImportModel = new VehicleImportModel();
            VehicleConfigDetails vehicleConfigDetails = ValidateVehicleConfigData(vehicle.VehicleConfiguration);
            vehicleImportModel.VehicleConfigDetails = vehicleConfigDetails;
            StringBuilder builder = new StringBuilder();
            if (vehicleConfigDetails.VehicleConfigError != null)
            {
                builder.Append(vehicleConfigDetails.VehicleConfigError.ErrorMessage);
            }
            List<VehicleComponentDetails> vehicleComponentsList = new List<VehicleComponentDetails>();

            if (vehicle.VehicleComponents != null)
            {
                for (int i = 0; i < vehicle.VehicleComponents.Count; i++)
                {
                    VehicleComponentDetails vehicleComponentDetails = ValidateComponentData(vehicle.VehicleComponents[i], vehicleConfigDetails.MovementClassification);
                    if (vehicleComponentDetails.VehicleComponentError != null)
                    {
                        builder.Append(vehicleComponentDetails.VehicleComponentError.ErrorMessage);
                    }
                    vehicleComponentsList.Add(vehicleComponentDetails);
                }
            }
            vehicleImportModel.VehicleComponentDetails = vehicleComponentsList;
            string ErrorMessage = builder.ToString();
            int length = ErrorMessage.Length;
            if (length > 0)
            {
                ValidationError validationError = new ValidationError
                {
                    ErrorMessage = ErrorMessage
                };
                vehicleImportModel.VehicleError = validationError;
            }
            return vehicleImportModel;
        }
        public VehicleConfigDetails ValidateVehicleConfigData(VehicleConfiguration vehicleConfiguration)
        {
            int errorCount = 0;
            string ErrorMessage = "Vehicle Configuration Errors -" + vehicleConfiguration.FormalName + "~";
            int vehicleType = 0;
            int movementClassification = 0;
            VehicleEnumConversions vehicleEnumConversions = new VehicleEnumConversions();
            VehicleConfigDetails vehicleConfigDetails = new VehicleConfigDetails
            {
                OrganisationId = vehicleConfiguration.OrganisationId
            };

            #region Formal Name
            if (String.IsNullOrEmpty(vehicleConfiguration.FormalName))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Vehicle formal name not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle formal name not found");
            }
            else
            {
                if (vehicleConfiguration.FormalName.Length > 150)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Vehicle formal name is too long(maximum is 150 characters)~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle formal name is too long(maximum is 150 characters)");
                }
                else
                {
                    vehicleConfigDetails.FormalName = vehicleConfiguration.FormalName;
                }
            }
            #endregion

            #region Internal Name
            if (String.IsNullOrEmpty(vehicleConfiguration.InternalName))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Vehicle internal name not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle internal name not found");
            }
            else
            {
                if (vehicleConfiguration.InternalName.Length > 100)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Vehicle internal name is too long(maximum is 100 characters)~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle internal name is too long(maximum is 100 characters)");
                }
                else
                {
                    vehicleConfigDetails.InternalName = vehicleConfiguration.InternalName;
                }
            }
            #endregion

            #region Movement Classification
            if (String.IsNullOrEmpty(vehicleConfiguration.MovementClassification))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Movement Classification not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Movement Classification not found");
            }
            else
            {
                vehicleConfigDetails.MovementClassification = vehicleEnumConversions.GetMovementClassificationId(vehicleConfiguration.MovementClassification.ToLower());
                if (vehicleConfigDetails.MovementClassification == 0)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Invalid Movement Classification~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Movement Classification");
                }
                movementClassification = vehicleConfigDetails.MovementClassification;
            }
            #endregion

            #region Vehicle Type
            if (String.IsNullOrEmpty(vehicleConfiguration.VehicleType))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Vehicle Type not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle Type not found");
            }
            else
            {
                vehicleConfigDetails.VehicleType = vehicleEnumConversions.GetVehicleTypeId(vehicleConfiguration.VehicleType.ToLower());
                if (vehicleConfigDetails.VehicleType == 0)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Invalid Vehicle Type~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Vehicle Type");
                }
                vehicleType = vehicleConfigDetails.VehicleType;
            }
            #endregion

            #region Description
            if (vehicleConfiguration.Description.Length > 200)
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Vehicle description is too long(maximum is 200 characters)~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle description is too long(maximum is 200 characters)");
            }
            else
            {
                vehicleConfigDetails.Description = vehicleConfiguration.Description;
            }
            #endregion

            #region Overall Length
            if (vehicleConfiguration.OverallLength > 0)
            {
                if (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005)
                {
                    if ((vehicleType == 244001 || vehicleType == 244002) && (vehicleConfiguration.OverallLength < 10 || vehicleConfiguration.OverallLength > 200))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Overall Length should be in between 10 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Overall Length should be in between 10 and 200");
                    }
                    else if (vehicleType == 244006 && (vehicleConfiguration.OverallLength < 5 || vehicleConfiguration.OverallLength > 200)) // Other Inline
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Overall Length should be in between 5 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Overall Length should be in between 5 and 200");
                    }
                }
                else if (movementClassification == 270006 || movementClassification == 270007)
                {
                    if ((vehicleType == 244001 || vehicleType == 244002) && (vehicleConfiguration.OverallLength < 10 || vehicleConfiguration.OverallLength > 200))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Overall Length should be in between 10 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Overall Length should be in between 10 and 200");
                    }
                    else if ((vehicleType == 244006 || vehicleType == 244007) && (vehicleConfiguration.OverallLength < 5 || vehicleConfiguration.OverallLength > 200)) // Other Inline
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Overall Length should be in between 5 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Overall Length should be in between 5 and 200");
                    }
                }
                vehicleConfigDetails.OverallLength = vehicleConfiguration.OverallLength;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Overall Length~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Overall Length");
            }
            #endregion

            #region Rigid Length
            if (vehicleConfiguration.RigidLength >= 0 || vehicleConfiguration.RigidLength == null)
            {
                bool error = false;
                if (movementClassification == 270002 && vehicleType == 244005 && (vehicleConfiguration.RigidLength < 5 || vehicleConfiguration.RigidLength > 200))
                {
                    error = true;
                }
                else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleType == 244005 || vehicleType == 244007) && (vehicleConfiguration.RigidLength < 5 || vehicleConfiguration.RigidLength > 200))
                {
                    error = true;
                }
                if (error)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 5 and 200~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Rigid Length should be in between 5 and 200");
                }
                vehicleConfigDetails.RigidLength = vehicleConfiguration.RigidLength;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Rigid Length~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Rigid Length");
            }
            #endregion

            #region Width
            if (vehicleConfiguration.Width >= 0 || vehicleConfiguration.Width == null)
            {
                if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005) && vehicleType == 244002 && (vehicleConfiguration.Width < 10 || vehicleConfiguration.Width > 100))
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 10 and 100~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Width should be in between 10 and 100");
                }
                else if (movementClassification == 270006 || movementClassification == 270007)
                {
                    if (vehicleType == 244002 && (vehicleConfiguration.Width < 10 || vehicleConfiguration.Width > 100)) // Semi Trailer
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 10 and 100~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Width should be in between 10 and 100");
                    }
                    else if (vehicleType == 244007 && (vehicleConfiguration.Width < 5 || vehicleConfiguration.Width > 200)) //Other Inline & Side by side
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 5 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Width should be in between 5 and 200");
                    }
                }
                vehicleConfigDetails.Width = vehicleConfiguration.Width;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Width~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Width");
            }
            #endregion

            #region Max Height
            if (vehicleConfiguration.MaxHeight >= 0 || vehicleConfiguration.MaxHeight == null)
            {
                vehicleConfigDetails.MaxHeight = vehicleConfiguration.MaxHeight;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid MaxHeight~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid MaxHeight");
            }
            #endregion

            #region Gross Weight
            if (vehicleConfiguration.GrossWeight >= 0 || vehicleConfiguration.GrossWeight == null)
            {
                if (movementClassification == 270001) //C&U
                {
                    if (vehicleType == 244002 && (vehicleConfiguration.GrossWeight < 3000 || vehicleConfiguration.GrossWeight > 44000)) // Semi Trailer
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 3000 and 44000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Gross Weight should be in between 3000 and 44000");
                    }
                    else if (vehicleType == 244006 && (vehicleConfiguration.GrossWeight < 2000 || vehicleConfiguration.GrossWeight > 10000000)) //Other Inline
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 2000 and 10000000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Gross Weight should be in between 2000 and 10000000");
                    }
                }
                else if (movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005)
                {
                    if (vehicleType == 244002 && (vehicleConfiguration.GrossWeight < 3000 || vehicleConfiguration.GrossWeight > 50000)) // Semi Trailer
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 3000 and 50000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Gross Weight should be in between 3000 and 50000");
                    }
                    else if (vehicleType == 244006 && (vehicleConfiguration.GrossWeight < 2000 || vehicleConfiguration.GrossWeight > 10000000)) //Other Inline
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 2000 and 10000000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Gross Weight should be in between 2000 and 10000000");
                    }
                }
                else if (movementClassification == 270006 || movementClassification == 270007)
                {
                    if (vehicleType == 244002 && (vehicleConfiguration.GrossWeight < 3000 || vehicleConfiguration.GrossWeight > 50000)) // Semi Trailer
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 3000 and 50000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Gross Weight should be in between 3000 and 50000");
                    }
                    else if ((vehicleType == 244006 || vehicleType == 244007) && (vehicleConfiguration.GrossWeight < 2000 || vehicleConfiguration.GrossWeight > 10000000)) //Other Inline & Side by side
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 2000 and 10000000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Gross Weight should be in between 2000 and 10000000");
                    }
                }
                vehicleConfigDetails.GrossWeight = vehicleConfiguration.GrossWeight;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Gross Weight~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Gross Weight");
            }
            #endregion

            #region Max Axle Weight
            if (vehicleConfiguration.MaxAxleWeight >= 0 || vehicleConfiguration.MaxAxleWeight == null)
            {
                vehicleConfigDetails.MaxAxleWeight = vehicleConfiguration.MaxAxleWeight;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Max Axle Weight~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Max Axle Weight");
            }
            #endregion

            #region Wheel Base
            if (vehicleConfiguration.WheelBase >= 0 || vehicleConfiguration.WheelBase == null)
            {
                vehicleConfigDetails.WheelBase = vehicleConfiguration.WheelBase;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Wheel Base~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Wheel Base");
            }
            #endregion

            #region Speed
            if (vehicleConfiguration.Speed >= 0 || vehicleConfiguration.Speed == null)
            {
                if ((movementClassification == 270006) && (vehicleConfiguration.Speed < 1 || vehicleConfiguration.Speed > 60)) //Drawbar & Semi Trailer
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Speed should be in between 1 and 60~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Speed should be in between 1 and 60");
                }
                vehicleConfigDetails.Speed = vehicleConfiguration.Speed;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Speed~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Speed");
            }
            #endregion

            #region Tyre Spacing
            if (vehicleConfiguration.TyreSpacing >= 0 || vehicleConfiguration.TyreSpacing == null)
            {
                if ((movementClassification == 270006 || movementClassification == 270007) && vehicleType == 244007 && (vehicleConfiguration.TyreSpacing < 1 || vehicleConfiguration.TyreSpacing > 100))
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Tyre Spacing should be in between 1 and 100~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Tyre Spacing should be in between 1 and 100");
                }
                vehicleConfigDetails.TyreSpacing = vehicleConfiguration.TyreSpacing;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Tyre Spacing~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Tyre Spacing");
            }
            #endregion

            if (errorCount > 0)
            {
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = ErrorMessage
                };
                vehicleConfigDetails.VehicleConfigError = validationError;
            }
            return vehicleConfigDetails;
        }

        public VehicleComponentDetails ValidateComponentData(VehicleComponents vehicleComponents, int movementClassification)
        {
            int errorCount = 0;
            int componentType = 0;
            string ErrorMessage = "Vehicle Component Errors - " + vehicleComponents.FormalName + "~";
            VehicleComponentDetails vehicleComponentDetails = new VehicleComponentDetails();
            VehicleEnumConversions vehicleEnumConversions = new VehicleEnumConversions();

            #region Formal Name
            if (String.IsNullOrEmpty(vehicleComponents.FormalName))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Component formal name not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Component formal name not found");
            }
            else
            {
                if (vehicleComponents.FormalName.Length > 150)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Vehicle component formal name is too long(maximum is 150 characters)~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle component formal name is too long(maximum is 150 characters)");
                }
                else
                {
                    vehicleComponentDetails.FormalName = vehicleComponents.FormalName;
                }
            }
            #endregion

            #region Internal Name
            if (String.IsNullOrEmpty(vehicleComponents.InternalName))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Component internal name not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Component internal name not found");
            }
            else
            {
                if (vehicleComponents.InternalName.Length > 100)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Vehicle component internal name is too long(maximum is 100 characters)~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle component internal name is too long(maximum is 100 characters)");
                }
                else
                {
                    vehicleComponentDetails.InternalName = vehicleComponents.InternalName;
                }
            }
            #endregion

            #region Component Type
            if (String.IsNullOrEmpty(vehicleComponents.ComponentType))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Component Type not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Component Type not found");
            }
            else
            {
                vehicleComponentDetails.ComponentType = vehicleEnumConversions.GetComponentTypeId(vehicleComponents.ComponentType.ToLower());
                if (vehicleComponentDetails.ComponentType == 0)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Invalid Component Type~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Invalid Component Type");
                }
                componentType = vehicleComponentDetails.ComponentType;
            }
            #endregion

            #region Component SubType
            if (String.IsNullOrEmpty(vehicleComponents.ComponentSubType))
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Component Sub Type not found~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Component Sub Type not found");
            }
            else
            {
                vehicleComponentDetails.ComponentSubType = vehicleEnumConversions.GetComponentSubTypeId(vehicleComponents.ComponentSubType.ToLower());
                if (vehicleComponentDetails.ComponentSubType == 0)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Invalid Component Sub Type~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Invalid Component Sub Type");
                }
            }
            #endregion

            #region Coupling Type
            if (String.IsNullOrEmpty(vehicleComponents.CouplingType))
            {
                if (componentType != 234004)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Coupling Type not found~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Coupling Type not found");
                }
            }
            else
            {
                int couplingType = vehicleEnumConversions.GetCouplingTypetId(vehicleComponents.CouplingType.ToLower());
                if (couplingType == 0)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Invalid Coupling Type~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Invalid Coupling Type");
                }
                else
                {
                    if ((componentType == 234001 || componentType == 234006) && couplingType != 201003 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Coupling Type should be 'Drawbar' for this component type~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Coupling Type should be 'Drawbar' for this component type");
                    }
                    else if ((componentType == 234002 || componentType == 234005) && couplingType != 201002 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Coupling Type should be 'Fifth Wheel' for this component type~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Coupling Type should be 'Fifth Wheel' for this component type");
                    }
                    else if ((componentType == 234003) && couplingType != 201001 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Coupling Type should be 'None' for this component type~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Coupling Type should be 'None' for this component type");
                    }
                    else if (componentType == 234004 && couplingType > 0)
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Invalid Coupling Type~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Invalid Coupling Type");
                    }
                    else if (componentType == 234007 && couplingType != 201001 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Coupling Type should be 'None' for this component type~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Coupling Type should be 'None' for this component type ");
                    }
                }

                vehicleComponentDetails.CouplingType = couplingType;
            }
            #endregion

            #region  Rigid Length validation
            if (vehicleComponents.RigidLength > 0)
            {
                if (componentType == 234001)
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.RigidLength < 2.5 || vehicleComponents.RigidLength > 25))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 2.5 and 25~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 2.5 and 25");
                    }
                    else
                    {
                        vehicleComponentDetails.RigidLength = vehicleComponents.RigidLength;
                    }
                }
                else if (componentType == 234002)
                {
                    if (movementClassification == 270005 && (vehicleComponents.RigidLength < 2.5 || vehicleComponents.RigidLength > 25))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 2.5 and 25~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 2.5 and 25");
                    }
                    else
                    {
                        vehicleComponentDetails.RigidLength = vehicleComponents.RigidLength;
                    }
                }
                else if (componentType == 234003)
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004) && (vehicleComponents.RigidLength < 5 || vehicleComponents.RigidLength > 30))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 5 and 30~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 5 and 30");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.RigidLength < 5 || vehicleComponents.RigidLength > 200))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 5 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 5 and 200");
                    }
                    else
                    {
                        vehicleComponentDetails.RigidLength = vehicleComponents.RigidLength;
                    }
                }
                else if (componentType == 234004)
                {
                    if ((movementClassification == 270007 || movementClassification == 270008) && (vehicleComponents.RigidLength < 2 || vehicleComponents.RigidLength > 200))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 2 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 2 and 200");
                    }
                    else
                    {
                        vehicleComponentDetails.RigidLength = vehicleComponents.RigidLength;
                    }
                }
                else if (componentType == 234005 || componentType == 234006)
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005) && (vehicleComponents.RigidLength < 5 || vehicleComponents.RigidLength > 30))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 5 and 30~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 5 and 30");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.RigidLength < 5 || vehicleComponents.RigidLength > 100))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 5 and 100~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 5 and 100");
                    }
                    else
                    {
                        vehicleComponentDetails.RigidLength = vehicleComponents.RigidLength;
                    }
                }
                else if (componentType == 234007)
                {
                    if (movementClassification == 270002 && (vehicleComponents.RigidLength < 5 || vehicleComponents.RigidLength > 30))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 5 and 30~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 5 and 30");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.RigidLength < 5 || vehicleComponents.RigidLength > 200))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Rigid Length should be in between 5 and 200~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rigid Length should be in between 5 and 200");
                    }
                    else
                    {
                        vehicleComponentDetails.RigidLength = vehicleComponents.RigidLength;
                    }
                }
                else
                {
                    vehicleComponentDetails.RigidLength = vehicleComponents.RigidLength;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Rigid Length~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Rigid Length");
            }
            #endregion 

            #region Width
            if (vehicleComponents.Width > 0)
            {
                if (componentType == 234001) //Ballast Tractor
                {
                    if ((movementClassification == 270001 || movementClassification == 270006) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 5))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 5~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 5");
                    }
                    else if ((movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270007) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 6))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 6~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 6");
                    }
                    else
                    {
                        vehicleComponentDetails.Width = vehicleComponents.Width;
                    }
                }
                else if (componentType == 234002) //Conventional Tractor
                {
                    if (movementClassification == 270006 && (vehicleComponents.Width < 2 || vehicleComponents.Width > 6))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 6~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 6");
                    }
                    else if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270007) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 5))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 5~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 5");
                    }
                    else
                    {
                        vehicleComponentDetails.Width = vehicleComponents.Width;
                    }
                }
                else if (componentType == 234003) //Rigid
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 6.1))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 6.1~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 6.1");
                    }
                    else if (movementClassification == 270003 && (vehicleComponents.Width < 2 || vehicleComponents.Width > 6))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 6~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 6");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 30))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 30~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 30");
                    }
                    else
                    {
                        vehicleComponentDetails.Width = vehicleComponents.Width;
                    }
                }
                else if (componentType == 234005) //Semi Trailer
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 6.1))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 6.1~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 6.1");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 25))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 25~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 25");
                    }
                    else
                    {
                        vehicleComponentDetails.Width = vehicleComponents.Width;
                    }
                }
                else if (componentType == 234006) //Drawbar Trailer
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 6.1))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 6.1~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 6.1");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 30))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 30~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 30");
                    }
                    else
                    {
                        vehicleComponentDetails.Width = vehicleComponents.Width;
                    }
                }
                else if (componentType == 234007) //SPMT
                {
                    if (movementClassification == 270002 && (vehicleComponents.Width < 2 || vehicleComponents.Width > 6.1))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 6.1~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 6.1");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 100))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 100~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 100");
                    }
                    else
                    {
                        vehicleComponentDetails.Width = vehicleComponents.Width;
                    }
                }
                else if (componentType == 234008) //Tracked
                {
                    if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.Width < 2 || vehicleComponents.Width > 50))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Width should be in between 2 and 50~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Width should be in between 2 and 50");
                    }
                    else
                    {
                        vehicleComponentDetails.Width = vehicleComponents.Width;
                    }
                }
                else
                {
                    vehicleComponentDetails.Width = vehicleComponents.Width;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Width~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Width");
            }
            #endregion

            #region MaxHeight
            if (vehicleComponents.MaxHeight > 0)
            {
                if ((componentType == 234001 || componentType == 234002) && (vehicleComponents.MaxHeight < 2 || vehicleComponents.MaxHeight > 6)) //Ballast and Conventional Tractor
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Max Height should be in between 2 and 6~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Max Height should be in between 2 and 6");
                }
                else if ((componentType == 234003 || componentType == 234006 || componentType == 234007) && (vehicleComponents.MaxHeight < 2 || vehicleComponents.MaxHeight > 50)) //Rigid,Drawbar Trailer and SPMT
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Max Height should be in between 2 and 50~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Max Height should be in between 2 and 50");
                }
                else if (componentType == 234005 && (vehicleComponents.MaxHeight < 2 || vehicleComponents.MaxHeight > 25)) //Semi Trailer
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Max Height should be in between 2 and 25~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Max Height should be in between 2 and 25");
                }
                else
                {
                    vehicleComponentDetails.MaxHeight = vehicleComponents.MaxHeight;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Max Height~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Max Height");
            }
            #endregion

            #region Reducible Height
            if (vehicleComponents.ReducibleHeight >= 0 || vehicleComponents.ReducibleHeight == null)
            {
                if ((componentType == 234001 || componentType == 234002) && (vehicleComponents.ReducibleHeight < 2 || vehicleComponents.ReducibleHeight > 6)) //Ballast and Conventional Tractor
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Reducible Height should be in between 2 and 6~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Reducible Height should be in between 2 and 6");
                }
                else if ((componentType == 234003 || componentType == 234006 || componentType == 234007) && (vehicleComponents.ReducibleHeight < 2 || vehicleComponents.ReducibleHeight > 50)) //Rigid,Drawbar Trailer and SPMT
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Reducible Height should be in between 2 and 50~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Reducible Height should be in between 2 and 50");
                }
                else if (componentType == 234005 && (vehicleComponents.ReducibleHeight < 2 || vehicleComponents.ReducibleHeight > 25)) //Semi Trailer
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Reducible Height should be in between 2 and 25~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Reducible Height should be in between 2 and 25");
                }
                else
                {
                    vehicleComponentDetails.ReducibleHeight = vehicleComponents.ReducibleHeight;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Reducible Height~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Reducible Height");
            }
            #endregion

            #region GrossWeight
            if (vehicleComponents.GrossWeight > 0)
            {
                if (componentType == 234001) //Ballast Tractor
                {
                    if (movementClassification == 270001 && (vehicleComponents.GrossWeight < 1500 || vehicleComponents.GrossWeight > 44000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 1500 and 44000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 1500 and 44000");
                    }
                    else if ((movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005) && (vehicleComponents.GrossWeight < 1500 || vehicleComponents.GrossWeight > 150000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 1500 and 150000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 1500 and 150000");
                    }
                    else if (movementClassification == 270006 && (vehicleComponents.GrossWeight < 1500 || vehicleComponents.GrossWeight > 1000000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 1500 and 1000000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 1500 and 1000000");
                    }
                    else if (movementClassification == 270007 && (vehicleComponents.GrossWeight < 1500 || vehicleComponents.GrossWeight > 80000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 1500 and 80000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 1500 and 80000");
                    }
                    else
                    {
                        vehicleComponentDetails.GrossWeight = vehicleComponents.GrossWeight;
                    }
                }
                else if (componentType == 234003) //Rigid
                {
                    if (movementClassification == 270001 && (vehicleComponents.GrossWeight < 1000 || vehicleComponents.GrossWeight > 44000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 1000 and 44000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 1000 and 44000");
                    }
                    else if ((movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004) && (vehicleComponents.GrossWeight < 1000 || vehicleComponents.GrossWeight > 150000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 1000 and 150000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 1000 and 150000");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.GrossWeight < 1500 || vehicleComponents.GrossWeight > 1000000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 1000 and 1000000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 1500 and 1000000");
                    }
                    else
                    {
                        vehicleComponentDetails.GrossWeight = vehicleComponents.GrossWeight;
                    }
                }
                else if (componentType == 234004) //Tracked
                {
                    if ((movementClassification == 270007 || movementClassification == 270008) && (vehicleComponents.GrossWeight < 2000 || vehicleComponents.GrossWeight > 200000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 2000 and 2000000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 2000 and 2000000");
                    }
                    else
                    {
                        vehicleComponentDetails.GrossWeight = vehicleComponents.GrossWeight;
                    }
                }
                else if (componentType == 234007) //SPMT
                {
                    if (movementClassification == 270002 && (vehicleComponents.GrossWeight < 5000 || vehicleComponents.GrossWeight > 150000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 5000 and 150000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 5000 and 150000");
                    }
                    else if ((movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.GrossWeight < 5000 || vehicleComponents.GrossWeight > 10000000))
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". GrossWeight should be in between 5000 and 10000000~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : GrossWeight should be in between 5000 and 10000000");
                    }
                    else
                    {
                        vehicleComponentDetails.GrossWeight = vehicleComponents.GrossWeight;
                    }
                }
                else
                {
                    vehicleComponentDetails.GrossWeight = vehicleComponents.GrossWeight;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid GrossWeight~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid GrossWeight");
            }
            #endregion

            #region Ground Clearance
            if (vehicleComponents.GroundClearance >= 0 || vehicleComponents.GroundClearance == null)
            {
                bool failed = false;
                if (vehicleComponents.GroundClearance > 2)
                {
                    if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        failed = true;
                    }
                    else if ((componentType == 234005 || componentType == 234006) && (movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        failed = true;
                    }
                    else if (componentType == 234003 && (movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        failed = true;
                    }
                    else
                    {
                        vehicleComponentDetails.GroundClearance = vehicleComponents.GroundClearance;
                    }
                    if (failed)
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Ground Clearance should be in between 0 and 2~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Ground Clearance should be in between 0 and 2");
                    }
                }
                else
                {
                    vehicleComponentDetails.GroundClearance = vehicleComponents.GroundClearance;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Ground Clearance~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Ground Clearance");
            }
            #endregion

            #region Reducible Ground Clearance
            if (vehicleComponents.ReducibleGroundClearance >= 0 || vehicleComponents.ReducibleGroundClearance == null)
            {
                bool failed = false;
                if (vehicleComponents.ReducibleGroundClearance > 2)
                {
                    if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        failed = true;
                    }
                    else if ((componentType == 234005 || componentType == 234006) && (movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        failed = true;
                    }
                    else if (componentType == 234003 && (movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007))
                    {
                        failed = true;
                    }
                    else
                    {
                        vehicleComponentDetails.ReducibleGroundClearance = vehicleComponents.ReducibleGroundClearance;
                    }
                    if (failed)
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Reducible Ground Clearance should be in between 0 and 2~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Reducible Ground Clearance should be in between 0 and 2");
                    }
                }
                else
                {
                    vehicleComponentDetails.ReducibleGroundClearance = vehicleComponents.ReducibleGroundClearance;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Ground Reducible Clearance~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Ground Reducible Clearance");
            }
            #endregion

            #region Wheel Base
            if (vehicleComponents.WheelBase >= 0 || vehicleComponents.WheelBase == null)
            {
                bool failed = false;
                if (componentType == 234003 && movementClassification == 270006 && (vehicleComponents.WheelBase < 5 || vehicleComponents.WheelBase > 100))
                {
                    failed = true;
                }
                else if (componentType == 234006 && (movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.WheelBase < 5 || vehicleComponents.WheelBase > 100))
                {
                    failed = true;
                }
                else if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.WheelBase < 5 || vehicleComponents.WheelBase > 200))
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Wheel Base should be in between 5 and 200~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Wheel Base should be in between 5 and 100");
                }
                else
                {
                    vehicleComponentDetails.WheelBase = vehicleComponents.WheelBase;
                }
                if (failed)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Wheel Base should be in between 5 and 100~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Wheel Base should be in between 5 and 100");
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Wheel Base~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Wheel Base");
            }
            #endregion

            #region Outside Track
            if (vehicleComponents.OutsideTrack >= 0 || vehicleComponents.OutsideTrack == null)
            {
                if ((componentType == 234005 || componentType == 234006) && (movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.OutsideTrack < 2 || vehicleComponents.OutsideTrack > 20))
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Outside Track should be in between 2 and 20~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Outside Track should be in between 2 and 20");
                }
                else if (componentType == 234003 && (movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.OutsideTrack < 2 || vehicleComponents.OutsideTrack > 30))
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Outside Track should be in between 2 and 30~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Outside Track should be in between 2 and 30");
                }
                else if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007) && (vehicleComponents.OutsideTrack < 2 || vehicleComponents.OutsideTrack > 200))
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Outside Track should be in between 2 and 200~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Outside Track should be in between 2 and 200");
                }
                else
                {
                    vehicleComponentDetails.OutsideTrack = vehicleComponents.OutsideTrack;
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Outside Track~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Outside Track");
            }
            #endregion

            #region Left Overhang
            if (vehicleComponents.LeftOverhang >= 0 || vehicleComponents.LeftOverhang == null)
            {
                bool failed = false;
                if (componentType == 234006)
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006) && vehicleComponents.LeftOverhang > 10)
                    {
                        failed = true;
                    }
                    else if (movementClassification == 270007 && vehicleComponents.LeftOverhang > 20)
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Left Overhang should be in between 0 and 20~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Left Overhang should be in between 0 and 20");
                    }
                    else
                    {
                        vehicleComponentDetails.LeftOverhang = vehicleComponents.LeftOverhang;
                    }
                }
                else if (componentType == 234003 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.LeftOverhang > 10)
                {
                    failed = true;
                }
                else if (componentType == 234005 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.LeftOverhang > 10)
                {
                    failed = true;
                }
                else if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.LeftOverhang > 30)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Left Overhang should be in between 0 and 30~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Left Overhang should be in between 0 and 30");
                }
                else
                {
                    vehicleComponentDetails.LeftOverhang = vehicleComponents.LeftOverhang;
                }
                if (failed)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Left Overhang should be in between 0 and 10~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Left Overhang should be in between 0 and 10");
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Left Overhang~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Left Overhang");
            }
            #endregion

            #region Right Overhang
            if (vehicleComponents.RightOverhang >= 0 || vehicleComponents.RightOverhang == null)
            {
                bool failed = false;
                if (componentType == 234006)
                {
                    if ((movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006) && vehicleComponents.RightOverhang > 10)
                    {
                        failed = true;
                    }
                    else if (movementClassification == 270007 && vehicleComponents.RightOverhang > 20)
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Right Overhang should be in between 0 and 20~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Right Overhang should be in between 0 and 20");
                    }
                    else
                    {
                        vehicleComponentDetails.RightOverhang = vehicleComponents.RightOverhang;
                    }
                }
                else if (componentType == 234003 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.RightOverhang > 10)
                {
                    failed = true;
                }
                else if (componentType == 234005 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.RightOverhang > 10)
                {
                    failed = true;
                }
                else if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.RightOverhang > 30)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Right Overhang should be in between 0 and 30~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Right Overhang should be in between 0 and 30");
                }
                else
                {
                    vehicleComponentDetails.RightOverhang = vehicleComponents.RightOverhang;
                }
                if (failed)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Right Overhang should be in between 0 and 10~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Right Overhang should be in between 0 and 10");
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Right Overhang~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Right Overhang");
            }
            #endregion

            #region Front Overhang
            if (vehicleComponents.FrontOverhang >= 0 || vehicleComponents.FrontOverhang == null)
            {
                bool failed = false;
                if (componentType == 234006 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006) && vehicleComponents.FrontOverhang > 20)
                {
                    failed = true;
                }
                else if (componentType == 234003 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.FrontOverhang > 20)
                {
                    failed = true;
                }
                else if (componentType == 234005 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.FrontOverhang > 10)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Front Overhang should be in between 0 and 10~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Front Overhang should be in between 0 and 10");
                }
                else if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.FrontOverhang > 30)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Front Overhang should be in between 0 and 30~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Front Overhang should be in between 0 and 30");
                }
                else
                {
                    vehicleComponentDetails.FrontOverhang = vehicleComponents.FrontOverhang;
                }
                if (failed)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Front Overhang should be in between 0 and 20~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Front Overhang should be in between 0 and 20");
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Front Overhang~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Front Overhang");
            }
            #endregion

            #region Rear Overhang
            if (vehicleComponents.RearOverhang >= 0 || vehicleComponents.RearOverhang == null)
            {
                bool failed = false;
                if (componentType == 234006 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006) && vehicleComponents.RearOverhang > 20)
                {
                    failed = true;
                }
                else if (componentType == 234003 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.RearOverhang > 20)
                {
                    failed = true;
                }
                else if (componentType == 234005 && (movementClassification == 270001 || movementClassification == 270002 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.RearOverhang > 20)
                {
                    failed = true;
                }
                else if (componentType == 234007 && (movementClassification == 270002 || movementClassification == 270006 || movementClassification == 270007) && vehicleComponents.RearOverhang > 30)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Rear Overhang should be in between 0 and 30~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rear Overhang should be in between 0 and 30");
                }
                else
                {
                    vehicleComponentDetails.RearOverhang = vehicleComponents.RearOverhang;
                }
                if (failed)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Rear Overhang should be in between 0 and 20~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Rear Overhang should be in between 0 and 20");
                }
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Rear Overhang~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Invalid Rear Overhang");
            }
            #endregion


            if (vehicleComponents.MaxAxleWeight >= 0 || vehicleComponents.MaxAxleWeight == null)
            {
                vehicleComponentDetails.MaxAxleWeight = vehicleComponents.MaxAxleWeight;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Max Axle Weight~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Max Axle Weight");
            }

            if (vehicleComponents.AxleSpacingToFollowing >= 0 || vehicleComponents.AxleSpacingToFollowing == null)
            {
                vehicleComponentDetails.AxleSpacingToFollowing = vehicleComponents.AxleSpacingToFollowing;
            }
            else
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Invalid Axle Spacing To Follow~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Invalid Axle Spacing To Follow");
            }
            #region Description
            if (vehicleComponents.Description.Length > 200)
            {
                errorCount++;
                ErrorMessage = ErrorMessage + errorCount + ". Vehicle component description is too long(maximum is 200 characters)~";
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateVehicleConfigData : {0}", "Error : Vehicle component description is too long(maximum is 200 characters)");
            }
            else
            {
                vehicleComponentDetails.Description = vehicleComponents.Description;
            }
            #endregion
            vehicleComponentDetails.VehiclePurpose = movementClassification;
            vehicleComponentDetails.RearSteerable = vehicleComponents.RearSteerable;

            #region Axle count validation
            if (componentType > 0 && componentType != 234004)
            {
                if (vehicleComponents.AxleCount > 0)
                {
                    if ((componentType == 234001 || componentType == 234002) && (vehicleComponents.AxleCount < 2 || vehicleComponents.AxleCount > 9)) //Ballast and Conventional Tractor
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Axle count should be in between 2 and 9~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Axle count should be in between 2 and 9");
                    }
                    else if ((componentType == 234006 || componentType == 234007) && (vehicleComponents.AxleCount < 2 || vehicleComponents.AxleCount > 99)) //Drawbar and SPMT
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Axle count should be in between 2 and 99~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Axle count should be in between 2 and 99");
                    }
                    else if (componentType == 234005 && vehicleComponents.AxleCount > 99) //Semi Trailer
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Axle count should be in between 1 and 99~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Axle count should be in between 1 and 99");
                    }
                    else if (componentType == 234003 && (vehicleComponents.AxleCount < 2 || vehicleComponents.AxleCount > 20)) //Rigid Vehicle
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Axle count should be in between 2 and 20~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - ValidateComponentData : {0}", "Error : Axle count should be in between 2 and 20");
                    }
                    else
                    {
                        vehicleComponentDetails.AxleCount = vehicleComponents.AxleCount;
                    }
                }
                else
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Invalid Axle Count~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateComponentData : {0}", "Error : Invalid Axle Count");
                }
            }
            #endregion

            #region Axle config validation

            if (vehicleComponents.Axles.Count == 0)
            {
                if (movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Axle configuration not found~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Axle configuration not found");
                }
                else if (movementClassification == 270007 && componentType != 234004)
                {
                    errorCount++;
                    ErrorMessage = ErrorMessage + errorCount + ". Axle configuration not found~";
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Axle configuration not found ");
                }
                else
                {
                    vehicleComponentDetails.Axles =null;
                }
            }

            else
            {
                if (vehicleComponentDetails.AxleCount != vehicleComponents.Axles.Count)
                {
                    if (movementClassification == 270002 || movementClassification == 270003 || movementClassification == 270004 || movementClassification == 270005 || movementClassification == 270006)
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Mismatch in Axle Count~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Mismatch in Axle Count");
                    }
                    else if (movementClassification == 270007 && componentType != 234004)
                    {
                        errorCount++;
                        ErrorMessage = ErrorMessage + errorCount + ". Mismatch in Axle Count~";
                        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, "ValidateVehicleConfigData : {0}", "Error : Mismatch in Axle Count ");
                    }
                }
                List<Axles> axleList = new List<Axles>();
                foreach (var item in vehicleComponents.Axles)
                {
                    Axles axle = new Axles
                    {
                        AxleNumber = item.AxleNumber,
                        NoOfWheels = item.NoOfWheels,
                        AxleWeight = item.AxleWeight,
                        DistanceToNextAxle = item.DistanceToNextAxle,
                        TyreSize = item.TyreSize
                    };
                    if (item.TyreSpacing != null)
                    {
                        axle.TyreSpacing = string.Join(",", item.TyreSpacing);
                    }
                    axleList.Add(axle);
                }
                vehicleComponentDetails.Axles = axleList;
            }
            #endregion

            if (errorCount > 0)
            {
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = ErrorMessage
                };
                vehicleComponentDetails.VehicleComponentError = validationError;
            }
            return vehicleComponentDetails;
        }



        /// <summary>
        /// To validate the vehicle configuration combinations
        /// </summary>
        /// <param name="vehicleCombo"></param>
        /// <param name="movementClassification"></param>
        /// <returns></returns>
        public static bool IsValidVehicleCombo(string vehicleCombo, string movementClassification)
        {
            bool result = false;
            try
            {
                switch (movementClassification)
                {
                    case "CNU":
                        if (CNUCombos.Contains(vehicleCombo)) result = true;
                        break;

                    case "STGOVR1":
                        if (STGOVR1Combos.Contains(vehicleCombo)) result = true;
                        break;

                    case "STGO":
                        if (STGOCombos.Contains(vehicleCombo)) result = true;
                        break;

                    case "SO":
                        if (SOCombos.Contains(vehicleCombo)) result = true;
                        break;

                    case "VSO":
                        if (VSOCombos.Contains(vehicleCombo)) result = true;
                        break;

                    case "TRACKED":
                        if (TRACKEDCombos.Contains(vehicleCombo)) result = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/InsertVehicleDetails, Exception: " + ex​​​​​);
            }
            return result;
        }

        public static readonly IList<String> CNUCombos = new ReadOnlyCollection<string>
           (new List<String>
               {
                    "CNU_CNU_SEMI", "CNU_CNU_DRAWBAR" ,"CNU_CNU_RIGID"
               }
           );

        public static readonly IList<String> STGOVR1Combos = new ReadOnlyCollection<string>
           (new List<String>
                {
                    "STGOVR1_CAT-1_SEMI", "STGOVR1_CAT-1_DRAWBAR" , "STGOVR1_CAT-1_RIGID" , "STGOVR1_CAT-1_SPMT",
                    "STGOVR1_CAT-2_SEMI", "STGOVR1_CAT-2_DRAWBAR" , "STGOVR1_CAT-2_RIGID" , "STGOVR1_CAT-2_SPMT",
                    "STGOVR1_CAT-3_SEMI", "STGOVR1_CAT-3_DRAWBAR" , "STGOVR1_CAT-3_RIGID" , "STGOVR1_CAT-3_SPMT"
                }
           );

        public static readonly IList<String> STGOCombos = new ReadOnlyCollection<string>
          (new List<String>
                {
                    "STGO_CAT-1-CU_SEMI", "STGO_CAT-1-CU_DRAWBAR" , "STGO_CAT-1-CU_RIGID" , "STGO_CAT-1-CU_SPMT",
                    "STGO_CAT-2_SEMI", "STGO_CAT-2_DRAWBAR" , "STGO_CAT-2_RIGID" , "STGO_CAT-2_SPMT",
                    "STGO_CAT-3_SEMI", "STGO_CAT-3_DRAWBAR" , "STGO_CAT-3_RIGID" , "STGO_CAT-3_SPMT",
                    "STGO_CRANE-A_RIGID", "STGO_CRANE-B_RIGID", "STGO_CRANE-C_RIGID",
                    "STGO_ENG-TRACKED_TRACKED","STGO_ENG-WHEELED_SEMI", "STGO_ENG-WHEELED_DRAWBAR" , "STGO_ENG-WHEELED_RIGID" ,
                    "STGO_RECOVERY_SEMI", "STGO_RECOVERY_DRAWBAR"
                }
          );

        public static readonly IList<String> SOCombos = new ReadOnlyCollection<string>
         (new List<String>
                {
                    "SO_SO_SEMI", "SO_SO_DRAWBAR" , "SO_SO_RIGID" , "SO_SO_SPMT",
                }
         );

        public static readonly IList<String> VSOCombos = new ReadOnlyCollection<string>
         (new List<String>
                {
                    "VSO_VSO_SEMI", "VSO_VSO_DRAWBAR" , "VSO_VSO_RIGID" , "VSO_VSO_SPMT",
                }
         );

        public static readonly IList<String> TRACKEDCombos = new ReadOnlyCollection<string>
        (new List<String>
                {
                    "TRACKED_TRACKED_TRACKED"
                }
        );
    }


}