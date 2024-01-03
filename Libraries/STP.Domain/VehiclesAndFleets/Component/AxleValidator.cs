using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Common.Validation;

namespace STP.Domain.VehicleAndFleets.Component
{
   public class AxleValidator
    {
        
        //Validation range for the number of wheels
        public RangeValidator<int> wheels{get; set;}
        //Validation range for the weight of the axle
        public RangeValidator<int> weight { get; set; }
        //Validation range for the axle spacing
        public RangeValidator<float> axleSpacing { get; set; }
        //Validation range of the tyre centre spacing
        public RangeValidator<float?> tyreCentreSpacing { get; set; }
       //Flag whether tyre centre spacing need to be configured
        public bool IsConfigureTyreCentreSpacing {get; set; }
        public bool IsTyreCentreSpacingRequired { get; set; }
        public bool IsTyreSizeRequired { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AxleValidator()
        {
            wheels = null;
            weight = null;
            axleSpacing = null;
            tyreCentreSpacing = null;
        }

       public AxleValidator(AxleValidator axleValidator)
       {
           if (axleValidator.wheels != null)
               wheels = new RangeValidator<int>(axleValidator.wheels);
           if (axleValidator.weight != null)
               weight = new RangeValidator<int>(axleValidator.weight);
           if (axleValidator.axleSpacing != null)
               axleSpacing = new RangeValidator<float>(axleValidator.axleSpacing);
           if (axleValidator.tyreCentreSpacing != null)
               tyreCentreSpacing = new RangeValidator<float?>(axleValidator.tyreCentreSpacing);
       }
        
       /// <summary>
       /// Static Function returning the tolerance range of the passed total weight of the vehicle component
       /// The total weight of all axles shall be greater than 0.99 of the vehicle weight and 
       /// less than 1.5 times the vehicle weight
       /// </summary>
       /// <param name="vehicleComponentWeight"></param>
       /// <returns>RangeValidator object</returns>
        public static RangeValidator<float> GetAxleToleranceRange(int vehicleComponentWeight)
        {
            RangeValidator<float> axleToleranceValidator = new RangeValidator<float>();
            axleToleranceValidator.PropName = "AxleTolerance";
            axleToleranceValidator.MinValue = (float) (vehicleComponentWeight * 0.99);
            axleToleranceValidator.MaxValue = (float ) (vehicleComponentWeight * 1.5);
            return axleToleranceValidator;
        }
        
    }
}
