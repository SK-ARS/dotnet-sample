using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using STP.Domain.VehiclesAndFleets.Configuration;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class VehicleConfiguration
    {
        public VehicleConfiguration()
        {
        }

        public VehicleConfiguration(VehicleConfigurationType vehcConfigType,List<VehicleConfigRow> lstConfigRow,List<IFXProperty> lstIFXProperty)
        {
           vehicleConfigType = vehcConfigType;
           VehicleConfigurationList = lstConfigRow;
           VehicleConfigParamList = lstIFXProperty;
        }

        public VehicleConfiguration(VehicleConfigurationType vehcConfigType, List<IFXProperty> lstIFXProperty)
        {
            vehicleConfigType = new VehicleConfigurationType(vehcConfigType);
            VehicleConfigParamList = ClonePropertyList(lstIFXProperty);
        }

         public VehicleConfiguration(VehicleConfiguration vehcConfiguration)
        {
            vehicleConfigType = new VehicleConfigurationType(vehcConfiguration.vehicleConfigType);
            VehicleConfigParamList = ClonePropertyList(vehcConfiguration.VehicleConfigParamList);
            if (vehcConfiguration.VehicleConfigurationList != null)
            {
                VehicleConfigurationList = ClonePropertyList(vehcConfiguration.VehicleConfigurationList);
            }
        }

         private List<IFXProperty> ClonePropertyList(List<IFXProperty> lstIFXProperty)
         {
             List<IFXProperty> lstIFXProp = new List<IFXProperty>();
             foreach (IFXProperty oProperty in lstIFXProperty)
             {
                 IFXProperty objNewProperty = new IFXProperty(oProperty);
                 lstIFXProp.Add(objNewProperty);
             }
             return lstIFXProp;
         }

         private List<VehicleConfigRow> ClonePropertyList(List<VehicleConfigRow> lstConfigRow)
         {
             List<VehicleConfigRow> lstConfigurationRow = new List<VehicleConfigRow>();
             foreach (VehicleConfigRow oConfigRow in lstConfigRow)
             {
                 VehicleConfigRow objNewConfigRow = new VehicleConfigRow(oConfigRow);
                 lstConfigurationRow.Add(objNewConfigRow);
             }
             return lstConfigurationRow;
         }

         /// <summary>
         /// Creates and returns new Vehicle Configuration Object by fetching data from Component Model
         /// </summary>
         /// <param name="componentModel"></param>
         /// <returns></returns>
         public void UpdateConfigProperties(ConfigurationModel configurationModel)
         {
             foreach (IFXProperty ifxProperty in VehicleConfigParamList)
             {
                 switch (ifxProperty.ParamModel)
                 {
                     case "Formal Name":
                         ifxProperty.ParamValue = configurationModel.FormalName != null ? configurationModel.FormalName : "";
                         break;
                     case "Internal Name":
                         ifxProperty.ParamValue = configurationModel.InternalName != null ? configurationModel.InternalName : "";
                         break;
                     case "Notes":
                         ifxProperty.ParamValue = configurationModel.Description != null ? configurationModel.Description : "";
                         break;
                     case "Length":
                         ifxProperty.ParamValue = Convert.ToString(configurationModel.RigidLength);
                         break;
                     case "Weight":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.GrossWeight);
                         break;
                     case "OverallLength":
                         ifxProperty.ParamValue = Convert.ToString(configurationModel.OverallLength);
                         break;
                     case "Width":
                         ifxProperty.ParamValue = Convert.ToString(configurationModel.Width);
                         break;
                     case "Maximum Height":
                         ifxProperty.ParamValue = Convert.ToString(configurationModel.MaxHeight);
                         break;
                     case "AxleWeight":
                         ifxProperty.ParamValue = Convert.ToString(configurationModel.MaxAxleWeight);
                         break;
                     case "Speed":
                         ifxProperty.ParamValue = Convert.ToString(configurationModel.TravellingSpeed);
                         break;
                     case "Tyre Spacing":
                         ifxProperty.ParamValue = Convert.ToString(configurationModel.TyreSpacing);
                         break;
                    case "Number of Axles":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.AxleCount);
                        break;
                    case "Left Overhang":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.NotifLeftOverhang);
                        break;
                    case "Right Overhang":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.NotifRightOverhang);
                        break;
                    case "Front Overhang":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.NotifFrontOverhang);
                        break;
                    case "Rear Overhang":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.NotifRearOverhang);
                        break;
                    case "Wheelbase":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.WheelBase);
                        break;
                    case "TrainWeight":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.TrainWeight);
                        break;
                    case "Reducable Height":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.ReducedHeight);
                        break;
                    case "Number of Axles for Trailer":
                        ifxProperty.ParamValue = Convert.ToString(configurationModel.TrailerAxleCount);
                        break;
                    default:
                         break;
                 }
             }
         }


         private int ConvertBooleanToInt(string value)
         {
             if (value == "true")
                 return 1;
             else
                 return 0;
         }



         //movementClassification class
         public MovementClassification moveClassification { get; set; }
        //VehicleConfigurationType class
        public VehicleConfigurationType vehicleConfigType { get; set; }
         //list VehicleRegitration class
        public List<VehicleRegistration> VehicleRegList { get; set; }
         //list VehicleConfigRow class
        public List<VehicleConfigRow> VehicleConfigurationList { get; set; }
        public static int MaxTractors = 4;
        public static int MaxTrailers = 4;

        public List<IFXProperty> VehicleConfigParamList { get; set;}       
    }
}
