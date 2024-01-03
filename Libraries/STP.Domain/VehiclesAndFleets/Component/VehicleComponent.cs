using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Common.Enums;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class VehicleComponent
    {
        //private int vechicleCompId;
        //private string vechicleCompName;
        //private VehicleComponent vehcComponent;
        //private VehicleComponentType vehcCompType;
       

        public VehicleComponent()
        {
            //vehcCompType = new VehicleComponentType();
            //vehcComponent = new VehicleComponent();            
            // TODO: Complete member initialization
            //this.vechicleCompId = vechicleCompId;
            //this.vechicleCompName = vechicleCompName;
        }

        public VehicleComponent(VehicleComponentType vehcCompType, VehicleCompSubType vehclSubType, List<IFXProperty> lstIFXProperty, bool isConfigAxle)
        {
            vehicleCompType = vehcCompType;
            vehicleComponentSubType = vehclSubType;
            VehicleParamList = lstIFXProperty;
            IsConfigAxle = isConfigAxle;
            IsConfigTyreCentreSpacing = false;
        }

        public VehicleComponent(VehicleComponent vehcComponent)
        {
            vehicleCompType = new VehicleComponentType(vehcComponent.vehicleCompType);
            vehicleComponentSubType = new VehicleCompSubType(vehcComponent.vehicleComponentSubType);
            VehicleParamList = ClonePropertyList(vehcComponent.VehicleParamList);
            IsConfigAxle = vehcComponent.IsConfigAxle;
            IsConfigTyreCentreSpacing = vehcComponent.IsConfigTyreCentreSpacing;
        }

        private List<IFXProperty> ClonePropertyList(List<IFXProperty> lstIFXProperty)
        {
            List<IFXProperty> lstIFXProp = new List<IFXProperty>() ;
            foreach (IFXProperty oProperty in lstIFXProperty)
            {
                IFXProperty objNewProperty = new IFXProperty(oProperty);
                lstIFXProp.Add(objNewProperty);
            }
            return lstIFXProp;
        }

        /// <summary>
        /// Creates and returns new Vehicle Component Object by fetching data from Component Model
        /// </summary>
        /// <param name="componentModel"></param>
        /// <returns></returns>
        public void UpdateVehicleProperties(ComponentModel componentModel)
        {
            foreach (IFXProperty ifxProperty in VehicleParamList)
            {
                switch (ifxProperty.ParamModel)
                {
                    case "Formal Name":
                        ifxProperty.ParamValue = componentModel.FormalName != null ? componentModel.FormalName : "";
                        break;
                    case "Internal Name":
                        ifxProperty.ParamValue = componentModel.IntendedName != null ? componentModel.IntendedName : "";
                        break;
                    case "Notes":
                        ifxProperty.ParamValue = componentModel.Description != null ? componentModel.Description : "";
                        break;
                    case "Component Type":
                        VehiclesAndFleets.VehicleEnums.ComponentSubType componentSubType = (VehiclesAndFleets.VehicleEnums.ComponentSubType)componentModel.ComponentSubType;
                        ifxProperty.ParamValue = componentSubType.GetEnumDescription();
                        break;
                    case "Number of Axles":
                        ifxProperty.ParamValue = Convert.ToInt32(componentModel.AxleCount) != 0 ? Convert.ToString(componentModel.AxleCount) : null;
                        break;
                    case "Maximum Height":
                        ifxProperty.ParamValue = Convert.ToInt32(componentModel.MaxHeight) != 0 ? Convert.ToString(componentModel.MaxHeight) : null;
                        break;
                    case "Reducable Height":
                        ifxProperty.ParamValue = Convert.ToInt32(componentModel.ReducableHeight) != 0 ? Convert.ToString(componentModel.ReducableHeight) : null;
                        break;
                    case "Ground Clearance":
                        ifxProperty.ParamValue = Convert.ToString(componentModel.GroundClearance);
                        break;
                    case "Reduced Ground Clearance":
                        ifxProperty.ParamValue = Convert.ToString(componentModel.RedGroundClearance);
                        break;
                    case "Length":
                        ifxProperty.ParamValue = Convert.ToInt32(componentModel.RigidLength) != 0 ? Convert.ToString(componentModel.RigidLength) : null;
                        break;
                    case "Width":
                        ifxProperty.ParamValue = Convert.ToInt32(componentModel.Width) != 0 ? Convert.ToString(componentModel.Width) : null;
                        break;
                    case "Left Overhang":
                        ifxProperty.ParamValue = Convert.ToString(componentModel.LeftOverhang);
                        break;
                    case "Right Overhang":
                        ifxProperty.ParamValue = Convert.ToString(componentModel.RightOverhang);
                        break;
                    case "Front Overhang":
                        ifxProperty.ParamValue = Convert.ToString(componentModel.FrontOverhang);
                        break;
                    case "Rear Overhang":
                        ifxProperty.ParamValue = Convert.ToString(componentModel.RearOverhang);
                        break;
                    case "Outside Track":
                        ifxProperty.ParamValue = Convert.ToString(componentModel.OutsideTrack);
                        break;
                    case "Wheelbase":
                        ifxProperty.ParamValue = Convert.ToInt32(componentModel.WheelBase) != 0 ? Convert.ToString(componentModel.WheelBase) : null;
                        break;
                    case "conformsCU":
                        ifxProperty.ParamValue = componentModel.StandardCU != 0 ? "true" : "false";
                        break;
                    case "Weight":
                        ifxProperty.ParamValue = Convert.ToInt32(componentModel.GrossWeight) != 0 ? Convert.ToString(componentModel.GrossWeight) : null;
                        break;
                    case "Axle Spacing To Following":
                        ifxProperty.ParamValue = Convert.ToDouble(componentModel.SpacingToFollowing) != 0 ? Convert.ToString(componentModel.SpacingToFollowing) : null;
                        break;
                    case "Rear Steer":
                        ifxProperty.ParamValue = componentModel.IsSteerable != null ? Convert.ToString(componentModel.IsSteerable) : "0";
                        break;
                    case "Coupling":
                        if (componentModel.CouplingType != 0)
                        {
                            VehiclesAndFleets.VehicleEnums.CouplingTypeNew couplingType = (VehiclesAndFleets.VehicleEnums.CouplingTypeNew)componentModel.CouplingType;
                            ifxProperty.ParamValue = couplingType.GetEnumDescription();
                        }
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

        //list VehicleRegitration class
        public List<VehicleRegistration> ListVehicleReg { get; set; }
        public int VehicleComponentId { get; set; }
        public int HaulierId { get; set; }
        //movementClassification class
        public MovementClassification moveClassification { get; set; }
        //vehicleCompType class
        public VehicleComponentType vehicleCompType { get; set; }
        //vehicleSubComp class
        public VehicleCompSubType vehicleComponentSubType { get; set; }
        //list of vehicleParameter
        public List<IFXProperty> VehicleParamList { get; set; }
        //list of Axle class
        public List<Axle> AxleList { get; set; }
        public int ClassificationId { get; set; }
        public bool IsConfigAxle { get; set; }
        /// <summary>
        /// Property determining whether tyre centre spacing need to be configured
        /// </summary>
        public bool IsConfigTyreCentreSpacing { get; set; }
        public double? TravellingSpeed { get; set; }
        public int? TravellingSpeedUnit { get; set; }
    }
}
