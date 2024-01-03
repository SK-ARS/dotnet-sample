using System;
using System.Collections.Generic;
using STP.Common.Validation;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class VehicleConfigurationType
    {
        public VehicleConfigurationType(int vehicleConfigId, string vehicleConfigName, RangeValidator<int> noComponents, bool isTractorInfront, int sidebySideRows, List<VehicleComponentType> vhcCompTypes, int maxTrailerCount, int maxTractorCount)
        {
            ConfigurationTypeId = vehicleConfigId;
            ConfigurationName = vehicleConfigName;
            ComponentsRange = noComponents;
            IsTractorInfront = isTractorInfront;
            SideBySideRows   = sidebySideRows;
            MaxTrailerCount = maxTrailerCount;
            MaxTractorCount = maxTractorCount;
            LstVehcCompTypes = new List<VehicleComponentType>(vhcCompTypes);
        }

        public VehicleConfigurationType (VehicleConfigurationType vehicleConfigurationType)
        {
            ConfigurationTypeId = vehicleConfigurationType.ConfigurationTypeId;
            ConfigurationName = vehicleConfigurationType.ConfigurationName;
            ComponentsRange = vehicleConfigurationType.ComponentsRange;
            IsTractorInfront = vehicleConfigurationType.IsTractorInfront;
            MaxTrailerCount = vehicleConfigurationType.MaxTrailerCount;
            MaxTractorCount = vehicleConfigurationType.MaxTractorCount;
            LstVehcCompTypes = new List<VehicleComponentType>(vehicleConfigurationType.LstVehcCompTypes);
        }

        /// <summary>
        /// Configuration type Id
        /// </summary>
        public int ConfigurationTypeId { get; set; }
        /// <summary>
        /// Configuration Name
        /// </summary>
        public string ConfigurationName{ get; set; }
        /// <summary>
        /// Number of Components Range
        /// </summary>
        public RangeValidator<int> ComponentsRange { get; set; } 
        /// <summary>
        /// Whether Tractor is in front
        /// </summary>
        public bool IsTractorInfront{get; set;}
        /// <summary>
        /// Max Side by side rows
        /// </summary>
        public int SideBySideRows { get; set; }
        public int MaxTrailerCount { get; set; }
        public int MaxTractorCount { get; set; }
        public List<VehicleComponentType> LstVehcCompTypes { get; set; }
    }
}
