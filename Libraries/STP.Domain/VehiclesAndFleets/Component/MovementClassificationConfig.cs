using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class MovementClassificationConfig
    {

        public MovementClassificationConfig()
        {
            ListVhclCompType = new List<VehicleComponentType>();
            mapCompType_SubType = new Dictionary<int, List<VehicleCompSubType>>();

            mapVehicleComponent = new Dictionary<string, VehicleComponent>();

            mapVhcConfigID_Type = new Dictionary<int, VehicleConfigurationType>();

            mapVhcConfigID_Configuration = new Dictionary<int, VehicleConfiguration>();
        }


        //MovementClassification class objects
        public MovementClassification MovementClassification { get; set; }
        public VehicleComponentType VehicleComponentType { get; set; }
        //list VehicleCompTypeConfig class
        public List<VehicleComponentType> ListVhclCompType { get; set; }

        public IDictionary<int, List<VehicleCompSubType>> mapCompType_SubType { get; set; }

        public IDictionary<string, VehicleComponent> mapVehicleComponent { get; set; }

        public IDictionary<int, VehicleConfigurationType> mapVhcConfigID_Type { get; set; }

        public IDictionary<int, VehicleConfiguration> mapVhcConfigID_Configuration { get; set; }

        public List<VehicleCompSubType> ListVehicleSubType { get; set; }

        public List<VehicleComponentType> GetListOfVehicleComponent()
        {
            return ListVhclCompType;
        }

        public List<VehicleCompSubType> GetListVehicleSubComponent(int vechicleTypeID)
        {
            return mapCompType_SubType[vechicleTypeID];
        }

        public List<VehicleConfigurationType> GetVehicleConfigList()
        {
            List<VehicleConfigurationType> vhclConfigTypeList = new List<VehicleConfigurationType>();
            var vhclConfigTypeMap = mapVhcConfigID_Type;
            foreach (var vehicleType in vhclConfigTypeMap)
            {
                VehicleConfigurationType vehicleCfgType = new VehicleConfigurationType(vehicleType.Value);
                vhclConfigTypeList.Add(vehicleCfgType);
            }
            return vhclConfigTypeList;
        }



        //Adds Vehicle Cmponent Type to the List
        public void addVehicleCompType(VehicleComponentType vehcCompType)
        {
            var vehCompList = (from s in ListVhclCompType
                               where s.ComponentTypeId == vehcCompType.ComponentTypeId
                               select s).ToList();

            if (vehCompList.Count == 0)
                ListVhclCompType.Add(vehcCompType); //Add if not already added
        }

        public void AddVehicleCompSubType(VehicleComponentType vehcCompType, VehicleCompSubType vehicleSubCompObj)
        {
        }

        internal void AddVehicle(VehicleComponentType vehcCompType, List<VehicleCompSubType> lstVehcCompSubType, List<IFXProperty> lstIFXProperty, bool isConfigAxle, bool isConfigSpacing)
        {
            addVehicleCompType(vehcCompType);
            //Retreive the list of vehicle component sub type id for the component type id
            List<VehicleCompSubType> vehcCompSubTypeLst = GetListVehicleSubType(vehcCompType.ComponentTypeId);
            foreach (var vehclSubType in lstVehcCompSubType)
            {
                vehcCompSubTypeLst.Add(vehclSubType); //add subtype to the list of vehcComp subyypes of the Component Type
                VehicleComponent vechComponentObj = new VehicleComponent(vehcCompType, vehclSubType, lstIFXProperty, isConfigAxle);
                vechComponentObj.IsConfigTyreCentreSpacing = isConfigSpacing;
                //Add vehicle component to the map of Vehicle components
                //Key will be ComponentType_ComponentTypeID as string. That is there will be one vehicle component for 
                //one combination of  ComponentType_ComponentTypeID             
                  AddVehiclComponentToMap(vehcCompType, vehclSubType, vechComponentObj);
            }
        }

        private void AddVehiclComponentToMap(VehicleComponentType vehcCompType, VehicleCompSubType vehclSubType, VehicleComponent vechComponentObj)
        {
            string strvehcCompKey = vehcCompType.ComponentTypeId + "_" + vehclSubType.SubCompType;

            var CompList = (from s in mapVehicleComponent
                            where s.Key == strvehcCompKey
                            select s).ToList();

            if (CompList.Count == 0)
                mapVehicleComponent.Add(new KeyValuePair<string, VehicleComponent>(strvehcCompKey, vechComponentObj));
        }

        public VehicleComponent GetVehicleComponent(int vehcCompTypeID, int vehcCompSubTypeID)
        {
            string strvehcCompKey = vehcCompTypeID + "_" + vehcCompSubTypeID;
            if (!mapVehicleComponent.ContainsKey(strvehcCompKey))
                return null;
            VehicleComponent vehcComponent = mapVehicleComponent[strvehcCompKey];
            if (vehcComponent != null)
            {
                VehicleComponent clonedVehicleComp = new VehicleComponent(vehcComponent);
                vehcComponent = clonedVehicleComp;
            }
            return vehcComponent;
        }


        private List<VehicleCompSubType> GetListVehicleSubType(int vehcCompID)
        {
            if (mapCompType_SubType.ContainsKey(vehcCompID))
            {
                return mapCompType_SubType[vehcCompID];
            }
            else
            {
                List<VehicleCompSubType> vechCompSubTpeLst = new List<VehicleCompSubType>();
                mapCompType_SubType.Add(new KeyValuePair<int, List<VehicleCompSubType>>(vehcCompID, vechCompSubTpeLst));
                return vechCompSubTpeLst;
            }
        }

        public void AddVehicleConfiguration(VehicleConfigurationType vhcConfigType, VehicleConfiguration vhcConfig)
        {
            if (!mapVhcConfigID_Type.ContainsKey(vhcConfigType.ConfigurationTypeId))
            {
                VehicleConfigurationType vehicleConfigType = new VehicleConfigurationType(vhcConfigType);
                mapVhcConfigID_Type.Add(new KeyValuePair<int,VehicleConfigurationType>(vhcConfigType.ConfigurationTypeId, vehicleConfigType));
            }

            if(!mapVhcConfigID_Configuration.ContainsKey(vhcConfigType.ConfigurationTypeId))
            {
                VehicleConfiguration vehicleConfig = new VehicleConfiguration(vhcConfig);
                mapVhcConfigID_Configuration.Add(new KeyValuePair<int, VehicleConfiguration>(vhcConfigType.ConfigurationTypeId, vehicleConfig));
            }
        }

        public VehicleConfiguration GetVehicleConfiguration(int vhcConfigTypeID)
        {
            if (mapVhcConfigID_Configuration.ContainsKey(vhcConfigTypeID))
            {
                return new VehicleConfiguration(mapVhcConfigID_Configuration[vhcConfigTypeID]);
            }
            else
            {
                return null;
            }
        }
    }
}
