using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using STP.Domain.Structures;
using STP.Common.Validation;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class ComponentConfiguration
    {
        public ComponentConfiguration()
        {            
            ListMovementClass = new List<MovementClassification>();
            ListVhclCompType = new List<VehicleComponentType>();
            mapID_MoveClass = new Dictionary<int, MovementClassificationConfig>();
            mapId_VehicleComponent = new Dictionary<int, MovementClassificationConfig>();
            mapId_VehicleCompType = new Dictionary<int, VehicleComponentType>();
            mapId_VehicleCompSubType = new Dictionary<int, VehicleCompSubType>();
            mapID_VehicleConfigType = new Dictionary<int, VehicleConfigurationType>();
            ListConfigClass = new List<VehicleConfigurationType>();
            mapVhcConfigID_Configuration = new Dictionary<int, VehicleConfiguration>();
            mapVhcConfigID_Type = new Dictionary<int, VehicleConfigurationType>();
        }
        
        //list MovementClassificationConfig
        public IDictionary<int, MovementClassificationConfig> mapID_MoveClass { get; set; }
        public IDictionary<int, MovementClassificationConfig> mapId_VehicleComponent { get; set; }
        public List<MovementClassification> ListMovementClass { get; set; }
        public List<VehicleComponentType> ListVhclCompType { get; set; }
        public List<VehicleConfigurationType> ListConfigClass { get; set; }


        public IDictionary<int, VehicleComponentType> mapId_VehicleCompType { get; set; }
        public IDictionary<int, VehicleCompSubType> mapId_VehicleCompSubType { get; set; }
        public IDictionary<int, VehicleConfigurationType> mapID_VehicleConfigType{ get; set; }
        public IDictionary<int, VehicleConfiguration> mapVhcConfigID_Configuration { get; set; }
        public IDictionary<int, VehicleConfigurationType> mapVhcConfigID_Type { get; set; }

        public List<MovementClassification> GetMovementClassification()
        {
            return ListMovementClass;
        }

        public MovementClassificationConfig GetMovementClassificationConfig(int classId)
        {
            return mapID_MoveClass[classId];
        }
        public List<VehicleComponentType> GetListOfVehicleComponent()
        {
            return ListVhclCompType;
        }

        public MovementClassificationConfig GetListOfVehicleComponents(int vehicleId)
        {
            return mapId_VehicleComponent[vehicleId];
        }
        public List<VehicleConfigurationType> GetConfigType()
        {
            return ListConfigClass;
        }

        public bool LoadVehicleConfiguration()
        {
            try
            {
                ReadDataFromXml();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

                       
        }

        /// <summary>
        /// GetAllMovementClass method loads movementcl
        /// </summary>
        /// <param name="xmlDocument"></param>
        private void GetAllMovementClass(XmlDocument xmlDocument)
        {
            XmlNode movementClassNode = xmlDocument.SelectSingleNode("VehicleComponentConfiguration/MovementClassification");
            XmlNodeList movementClassList = movementClassNode.SelectNodes("Classification");
            foreach (XmlNode movementData in movementClassList)
            {                
                int movementId = Convert.ToInt32(movementData.Attributes.GetNamedItem("id").Value);
                string movementName = movementData.InnerText;
                MovementClassification movClassification = new MovementClassification(movementId, movementName);
                ListMovementClass.Add(movClassification);
                
                MovementClassificationConfig movClassConfg = new MovementClassificationConfig();
                movClassConfg.MovementClassification = movClassification;
                mapID_MoveClass.Add(new KeyValuePair<int, MovementClassificationConfig>(movementId, movClassConfg));
            }
        }

        private void GetAllVehicleCompType(XmlDocument xmlDocument)
        {
            XmlNode componentTypeNode = xmlDocument.SelectSingleNode("VehicleComponentConfiguration/VehicleComponentType");
            XmlNodeList ComponentTypeNodeLst = componentTypeNode.SelectNodes("ComponentType");
            foreach (XmlNode componentType in ComponentTypeNodeLst)
            {
                int vechicleCompId = Convert.ToInt32(componentType.Attributes.GetNamedItem("id").Value);
                bool isTractor     = Convert.ToBoolean(componentType.Attributes.GetNamedItem("Tractor").Value);
                string imageName   = componentType.Attributes.GetNamedItem("img").Value;
                string vechicleCompName = componentType.InnerText;
                VehicleComponentType vechicleComp = new VehicleComponentType(vechicleCompId, vechicleCompName, isTractor, imageName);
                ListVhclCompType.Add(vechicleComp);
                mapId_VehicleCompType.Add(new KeyValuePair<int, VehicleComponentType>(vechicleCompId, vechicleComp));
               
                MovementClassificationConfig movClassConfg = new MovementClassificationConfig();
                movClassConfg.VehicleComponentType = vechicleComp;
                mapId_VehicleComponent.Add(new KeyValuePair<int, MovementClassificationConfig>(vechicleCompId, movClassConfg));
            }
        }

        private void GetAllVehicleCompSubType(XmlDocument xmlDocument)
        {
            //Get the Node list of SubComponentType
            XmlNodeList vehicleSubComponents = xmlDocument.SelectNodes("VehicleComponentConfiguration/VehicleSubComponentType/SubComponentType");
            foreach (XmlNode subComponentType in vehicleSubComponents)
            {
                //Get the subcomponenttypeid from attribute
                int vehicleSubCompId = Convert.ToInt32(subComponentType.Attributes.GetNamedItem("id").Value);
                string imageName = subComponentType.SelectSingleNode("image").InnerText;
                string vehicleSubCompName = subComponentType.SelectSingleNode("subcompName").InnerText;
                VehicleCompSubType vehicleSubComp = new VehicleCompSubType(vehicleSubCompId, vehicleSubCompName);
                vehicleSubComp.ImageName     = imageName; //Set Image Name
                vehicleSubComp.axleValidator = GetAxleValidator(subComponentType);

                mapId_VehicleCompSubType.Add(new KeyValuePair<int, VehicleCompSubType>(vehicleSubCompId, vehicleSubComp));
            }
        }
        /// <summary>
        /// Parses the subComponentType node for the axle configuration of the sub component type
        /// </summary>
        /// <param name="subComponentType"></param>
        /// <returns>AxleValidator</returns>
        private AxleValidator GetAxleValidator(XmlNode subComponentType)
        {
            int maxWheels = Convert.ToInt32(subComponentType.SelectSingleNode("AxleConfig/MaxWheels").InnerText);
            int minWeight = Convert.ToInt32(subComponentType.SelectSingleNode("AxleConfig/MinWeight").InnerText);
            int maxWeight = Convert.ToInt32(subComponentType.SelectSingleNode("AxleConfig/MaxWeight").InnerText);
            float minAxleSpacing = (float)Convert.ToDouble(subComponentType.SelectSingleNode("AxleConfig/MinAxleSpacing").InnerText);
            float maxAxleSpacing = (float)Convert.ToDouble(subComponentType.SelectSingleNode("AxleConfig/MaxAxleSpacing").InnerText);
            float? minTyreCentre  = subComponentType.SelectSingleNode("AxleConfig/MinTyreCentre").InnerText!=string.Empty? (float?)Convert.ToDouble(subComponentType.SelectSingleNode("AxleConfig/MinTyreCentre").InnerText):null;
            float? maxTyreCentre = subComponentType.SelectSingleNode("AxleConfig/MaxTyreCentre").InnerText != string.Empty ? (float?)Convert.ToDouble(subComponentType.SelectSingleNode("AxleConfig/MaxTyreCentre").InnerText) : null; 

            AxleValidator axleValidator = new AxleValidator();
            axleValidator.wheels = new RangeValidator<int>(2, maxWheels, "axlewheels");
            axleValidator.weight = new RangeValidator<int>(minWeight, maxWeight, "axleweight");
            axleValidator.axleSpacing = new RangeValidator<float>(minAxleSpacing, maxAxleSpacing, "axlespacing");
            axleValidator.tyreCentreSpacing = new RangeValidator<float?>(minTyreCentre, maxTyreCentre, "tyrecentrespacing");
            return axleValidator;

        }

        private void ReadDataFromXml()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\VehicleComponents.xml");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            //Loads all the available MOvement classifications to the mapID_MoveClass. Movement ID will be the key 
            //and respective MovementClassificationConfig will be the value. Also list of ListMovementClass is updated
            GetAllMovementClass(xmlDocument);

            //Loads all the available vehicle component types to the mapId_VehicleCompType. Vehicle component ID will be the key 
            //and respective VehicleCompType will be the value.
            GetAllVehicleCompType(xmlDocument);

            //Loads all the available vehicle subcomponent types to the mapId_VehicleCompSubType. Vehicle subCompoenet ID will be the key 
            //and respective VehicleCompSubType will be the value.
            GetAllVehicleCompSubType(xmlDocument);

            //Loads movementclassificationConfig details
            LoadMovementClassificationComp(xmlDocument);

            //Load Vehicle Configuration details
           LoadVehicleConfigurationDetails(xmlDocument);

            //Loads LoadComponentClassification details
            LoadComponentClassification(xmlDocument);
        }

        /// <summary>
        /// Loads VehicleConfiguration Details form XML
        /// </summary>
        /// <param name="xmlDocument"></param>
        private void LoadVehicleConfigurationDetails(XmlDocument xmlDocument)
        {
            //First read the max trailers and max tractors
            VehicleConfiguration.MaxTrailers = Convert.ToInt32(xmlDocument.SelectSingleNode("VehicleComponentConfiguration/VehicleConfigurationTypes/MaxTrailersinConfig").InnerText);
            VehicleConfiguration.MaxTractors = Convert.ToInt32(xmlDocument.SelectSingleNode("VehicleComponentConfiguration/VehicleConfigurationTypes/MaxTractorsinConfig").InnerText);

            //Now load vehicle configuration types
            LoadVehicleConfigTypes(xmlDocument);
            //Now Load Vehicle Configurations
            LoadVehicleConfiguration(xmlDocument);
        }

        /// <summary>
        /// Loads the Vehicle Configuration details from the xml
        /// </summary>
        /// <param name="xmlDocument"></param>
        private void LoadVehicleConfiguration(XmlDocument xmlDocument)
        {
            XmlNodeList vehicleConfigNodes = xmlDocument.SelectNodes("VehicleComponentConfiguration/VehicleConfigurations/VehicleConfiguration");
            foreach (XmlNode configNode in vehicleConfigNodes)
            {
                int configType = Convert.ToInt32(configNode.SelectSingleNode("ConfigurationType").InnerText);
                VehicleConfigurationType vhcConfigType = mapID_VehicleConfigType[configType]; //Get the configuration object
                //Get Param node
                List<IFXProperty> lstConfigProperties = GetVehiclePropertyList(configNode);
                //List<int> lstMovementClassID = GetintLstFromNode(configNode, "MovementClassificationID");
                VehicleConfiguration vhcConfig = new VehicleConfiguration(vhcConfigType, lstConfigProperties);
                //foreach (int movClassId in lstMovementClassID)
                //{
                    AddVehicleConfiguration(vhcConfigType, vhcConfig);
                //}
            }
        }
        public void AddVehicleConfiguration(VehicleConfigurationType vhcConfigType, VehicleConfiguration vhcConfig)
        {
            if (!mapVhcConfigID_Type.ContainsKey(vhcConfigType.ConfigurationTypeId))
            {
                VehicleConfigurationType vehicleConfigType = new VehicleConfigurationType(vhcConfigType);
                mapVhcConfigID_Type.Add(new KeyValuePair<int, VehicleConfigurationType>(vhcConfigType.ConfigurationTypeId, vehicleConfigType));
            }

            if (!mapVhcConfigID_Configuration.ContainsKey(vhcConfigType.ConfigurationTypeId))
            {
                VehicleConfiguration vehicleConfig = new VehicleConfiguration(vhcConfig);
                mapVhcConfigID_Configuration.Add(new KeyValuePair<int, VehicleConfiguration>(vhcConfigType.ConfigurationTypeId, vehicleConfig));
            }
        }
        /// <summary>
        /// Parses the child node of the passed xml node and retrives the list of int givens as comma separated values
        /// </summary>
        /// <param name="configNode"></param>
        /// <param name="childnodeName"></param>
        /// <returns></returns>
        private List<int> GetintLstFromNode(XmlNode configNode, string childnodeName)
        {
            List<int> lstIntegers = new List<int>();
            string nodetext = configNode.SelectSingleNode(childnodeName).InnerText;
            string[] intvalues = nodetext.Split(',');
            foreach (string value in intvalues)
            {
                lstIntegers.Add(Convert.ToInt32(value));
            }
            return lstIntegers;
        }


        /// <summary>
        /// Loads configuration types from xml
        /// </summary>
        /// <param name="xmlDocument"></param>
        private void LoadVehicleConfigTypes(XmlDocument xmlDocument)
        {
            XmlNodeList configTypeNodeLst = xmlDocument.SelectNodes("VehicleComponentConfiguration/VehicleConfigurationTypes/Configuration");
            foreach (XmlNode configNode in configTypeNodeLst)
            {
                int nVehicleConfigType = Convert.ToInt32(configNode.SelectSingleNode("ConfigId").InnerText);
                string configName      = configNode.SelectSingleNode("ConfigName").InnerText;
                RangeValidator<int> rangeNumberofComponent = GetIntRangeValidator(configNode, "NumberOfComponents");
                bool isTractorInFront = Convert.ToBoolean(configNode.SelectSingleNode("OnlyTractorInFront").InnerText);
                int nSidebySideRows   = Convert.ToInt32(configNode.SelectSingleNode("SideBySideRows").InnerText);
                int maxTrailerCount = Convert.ToInt32(configNode.SelectSingleNode("MaxTrailerCount").InnerText);
                int maxTractorCount = Convert.ToInt32(configNode.SelectSingleNode("MaxTractorCount").InnerText);
                List<VehicleComponentType> lstCompType = GetVehicleComponentTypes(configNode);
                VehicleConfigurationType vehicleConfigType = new VehicleConfigurationType(nVehicleConfigType, configName, rangeNumberofComponent,
                    isTractorInFront, nSidebySideRows, lstCompType, maxTrailerCount, maxTractorCount);
                ListConfigClass.Add(vehicleConfigType);
                mapID_VehicleConfigType.Add(new KeyValuePair<int, VehicleConfigurationType>(nVehicleConfigType, vehicleConfigType));
            }

        }

        private List<VehicleComponentType> GetVehicleComponentTypes(XmlNode configNode)
        {
            string strComponentTypes = configNode.SelectSingleNode("ComponentType").InnerText;
            string[] strComponentTypeArr = strComponentTypes.Split(',');
            List<VehicleComponentType> lstVehcCompTypes = new List<VehicleComponentType>();
            foreach(string strComponentType in strComponentTypeArr)
            {
                int componentTypeId = Convert.ToInt32(strComponentType);
                VehicleComponentType vehcCompType = mapId_VehicleCompType[componentTypeId];
                lstVehcCompTypes.Add(vehcCompType);
            }
            return lstVehcCompTypes;
        }

        /// <summary>
        /// Reads the parameter node from the xml node and returns range validator
        /// </summary>
        /// <param name="configTypeNodeLst"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private RangeValidator<int> GetIntRangeValidator(XmlNode configNode, string parameter)
        {
            int nMinVal = 1;
            int nMaxVal = 1;
            string strText = configNode.SelectSingleNode(parameter).InnerText;
            string[] range = strText.Split('-');
            if (range.Length == 1)
            {
                nMinVal = nMaxVal = Convert.ToInt32(range[0]);
                
            }
            else if (range.Length == 2)
            {
                nMinVal = Convert.ToInt32(range[0]);
                nMaxVal = Convert.ToInt32(range[1]);
            }
            return new RangeValidator<int>(nMinVal, nMaxVal, parameter);
        }

        private void LoadMovementClassificationComp(XmlDocument xmlDocument)
        {
            foreach (var movement in mapID_MoveClass)
            {               
                LoadVehicleComponent(xmlDocument, movement.Key);
            }
        }

        private void LoadComponentClassification(XmlDocument xmlDocument)
        {
            foreach (var component in mapId_VehicleComponent)
            {
                LoadSubComponents(xmlDocument, component.Key);
            }
        }

        /// <summary>
        /// Loads the vehicle components objects of the passed MovementClassificationID
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="movementClassID"></param>
        public void LoadVehicleComponent(XmlDocument xmlDocument, int movementClassID)
        {
            try
            {
                string queryString = "VehicleComponentConfiguration/VehicleComponents/MovementClassificationID[@id='" + movementClassID + "']/VehicleComponent";
                XmlNodeList lstvehcComponentNodes = xmlDocument.SelectNodes(queryString);
                MovementClassificationConfig movClassConfig = GetMovementClassificationConfig(movementClassID);

                foreach (XmlNode vehicleComponentNode in lstvehcComponentNodes)
                {
                    VehicleComponentType vehcCompType = LoadVehicleComponentType(vehicleComponentNode);
                    bool configurableAxle  = GetConfigurableAxle(vehicleComponentNode);
                    bool configTyreSpacing = GetConfigTyreSpacing(vehicleComponentNode);
                    if (vehcCompType != null)
                    {
                        //OK we got a vehicle component Type
                        List<VehicleCompSubType> lstVehcCompSubType = LoadVehicleComponentSubTypes(vehicleComponentNode);
                        List<IFXProperty> lstIFXProperty = GetVehiclePropertyList(vehicleComponentNode);
                        movClassConfig.AddVehicle(vehcCompType, lstVehcCompSubType, lstIFXProperty, configurableAxle, configTyreSpacing);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
                           
        }

        /// <summary>
        /// Check the Configure tyre spacing field in xml and returns true or false
        /// </summary>
        /// <param name="vehicleComponentNode"></param>
        /// <returns>bool</returns>
        private bool GetConfigTyreSpacing(XmlNode vehicleComponentNode)
        {
            bool isConfigurableAxle = false;
            XmlNode node = vehicleComponentNode.SelectSingleNode("ConfigureTyreSpacing");
            if (node != null)
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                    isConfigurableAxle = Convert.ToBoolean(node.InnerText);
            }
            return isConfigurableAxle;
        }

        /// <summary>
        /// Returns true if axle details need to b econfigurred
        /// </summary>
        /// <param name="vehicleComponentNode"></param>
        /// <returns></returns>
        private bool GetConfigurableAxle(XmlNode vehicleComponentNode)
        {
            bool isConfigurableAxle = false;
            XmlNode node = vehicleComponentNode.SelectSingleNode("AxleConfiguration");
            if (node != null)
            {
                if(!string.IsNullOrEmpty(node.InnerText))
                isConfigurableAxle = Convert.ToBoolean(node.InnerText);
            }
            return isConfigurableAxle;
        }

        private  List<IFXProperty> GetVehiclePropertyList(XmlNode vehicleComponentNode)
        {
            XmlNodeList vehcParamNodeLst = vehicleComponentNode.SelectNodes("ParamNode");
            List<IFXProperty> vehiclePropertyLst = new List<IFXProperty>();
            if (vehcParamNodeLst != null)
            {
                foreach (XmlNode paramNode in vehcParamNodeLst)
                {
                    IFXProperty vehcProperty = GetVehicleProperty(paramNode);
                    if (vehcProperty != null)
                    {
                        vehiclePropertyLst.Add(vehcProperty);
                    }
                }
                
            }
            return vehiclePropertyLst;
        }

        //Create the IFXProperty by reading the vehicle param node
        private IFXProperty GetVehicleProperty(XmlNode paramNode)
        {
            IFXProperty ifxPropertyObj = new IFXProperty();
            ifxPropertyObj.DisplayString = GetParamText(paramNode, "DisplayString");
            ifxPropertyObj.ParamModel    = GetParamText(paramNode, "ParamModel");
            ifxPropertyObj.ParamValue = GetParamText(paramNode, "ParamValue");

            ifxPropertyObj.ParamType = GetParamText(paramNode, "ParamType");
            ifxPropertyObj.ParamMaxLength = GetParamText(paramNode, "ParamMaxLength");
            ifxPropertyObj.StrRange = GetParamText(paramNode, "StrRange");

            ifxPropertyObj.InputType = GetParamText(paramNode, "InputType");
            if (!string.IsNullOrEmpty(GetParamText(paramNode, "IsRequired")))
                ifxPropertyObj.IsRequired = Convert.ToInt32(GetParamText(paramNode, "IsRequired"));
            if (!string.IsNullOrEmpty(GetParamText(paramNode, "ShowText")))
                ifxPropertyObj.ShowText = Convert.ToInt32(GetParamText(paramNode, "ShowText"));

            ifxPropertyObj.ValidRegex = GetParamText(paramNode, "validRegex");

            ifxPropertyObj.DropDownList = GetParamList(paramNode, "DropDownList", "keyValue");
            ifxPropertyObj.TextId = GetParamText(paramNode, "TextId");

            return ifxPropertyObj;
            

        }

        private string GetParamText(XmlNode paramNode, string property)
        {
            string text = "";
            XmlNode propNode = paramNode.SelectSingleNode(property);
            if (propNode != null)
            {
                text = propNode.InnerText;
            }
            return text;
        }

        private List<string> GetParamList(XmlNode paramNode, string property, string innerPropery)
        {
            List<string> parameterStringList=new List<string>();
            XmlNode paramList = paramNode.SelectSingleNode(property);
            if (paramList != null)
            {
                XmlNodeList paramStringList = paramList.SelectNodes(innerPropery);
                foreach (XmlNode paramString in paramStringList)
                {
                    parameterStringList.Add(paramString.InnerText);
                }
            }
            return parameterStringList;

        }


        //Load the vehicle component sub types of the passed vehicle component type for the movement classification id
        private List<VehicleCompSubType> LoadVehicleComponentSubTypes(XmlNode vehicleComponent)
        {
            List<VehicleCompSubType> lstVehcCompSubType = new List<VehicleCompSubType>(); ;
            //Get the component subType of the VehicleComponent which will be in the format <SubComponentTypeid>14,16,9,2</SubComponentTypeid>
            XmlNode node = vehicleComponent.SelectSingleNode("SubComponentTypeid");
            if (node != null)
            {
                string subComponentType = node.InnerText;
                if (!string.IsNullOrEmpty(subComponentType))
                {
                    var subCompStrList = subComponentType.Split(',');
                    foreach (var subCompId in subCompStrList)
                    {
                        int subComponentID = Convert.ToInt32(subCompId);
                        VehicleCompSubType vehicleSubCompObj = mapId_VehicleCompSubType[subComponentID];
                        if (vehicleSubCompObj != null)
                        {
                            lstVehcCompSubType.Add(vehicleSubCompObj);
                        }
                    }
                }
            }
            return lstVehcCompSubType;
        }

        //Parses the passed vehicleComponent xml node for Vehicle Component and adds to the movClassConfig
        private VehicleComponentType LoadVehicleComponentType(XmlNode vehicleComponent)
        {
            VehicleComponentType vehcCompType = null;
            //Get the component Type of the VehicleComponent  <ComponentTypeID>1</ComponentTypeID>
            XmlNode node = vehicleComponent.SelectSingleNode("ComponentTypeID");
            if (node != null)
            {
                string compId = node.InnerText;
                if (!string.IsNullOrEmpty(compId))
                {
                    int componentId = Convert.ToInt32(compId);
                    vehcCompType = mapId_VehicleCompType[componentId];
                }
            }
            return vehcCompType;
        }


        /// <summary>
        /// Loads the vehicle components objects of the passed MovementClassificationID
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="movementClassID"></param>
        public void LoadSubComponents(XmlDocument xmlDocument, int vehicleId)
        {
            try
            {
                //string queryString = "VehicleComponentConfiguration/VehicleComponents/MovementClassificationID/VehicleComponent[@id='234003']";
                string queryString = "VehicleComponentConfiguration/VehicleComponents/MovementClassificationID/VehicleComponent[@id="+ vehicleId +"]";
                XmlNodeList lstvehcComponentNodes = xmlDocument.SelectNodes(queryString);
                MovementClassificationConfig movClassConfig = GetListOfVehicleComponents(vehicleId);

                foreach (XmlNode vehicleComponentNode in lstvehcComponentNodes)
                {
                    VehicleComponentType vehcCompType = LoadVehicleComponentType(vehicleComponentNode);
                    bool configurableAxle = GetConfigurableAxle(vehicleComponentNode);
                    bool configTyreSpacing = GetConfigTyreSpacing(vehicleComponentNode);
                    if (vehcCompType != null)
                    {
                        //OK we got a vehicle component Type
                        List<VehicleCompSubType> lstVehcCompSubType = LoadVehicleComponentSubTypes(vehicleComponentNode);
                        List<IFXProperty> lstIFXProperty = GetVehiclePropertyList(vehicleComponentNode);
                        movClassConfig.AddVehicle(vehcCompType, lstVehcCompSubType, lstIFXProperty, configurableAxle, configTyreSpacing);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
