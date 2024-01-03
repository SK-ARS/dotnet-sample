using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Common.Validation
{
    //Generic Class RangeValidator
    //<T> can be int, float or double
    public class RangeValidator<T>
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="rangeValidator"></param>
        public RangeValidator(RangeValidator<T> rangeValidator)
        {
            PropName = new string(rangeValidator.PropName.ToCharArray());
            this.MinValue = rangeValidator.MinValue;
            this.MaxValue = rangeValidator.MaxValue;
        }

        /// <summary>
        /// Constructor by passing min and max value
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public RangeValidator(T minValue, T maxValue, string propertyName)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            PropName = propertyName;

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RangeValidator()
        {
            
        }

        //Name of the property being validated
        public string PropName { get; set; }
        /// <summary>
        /// Minimum Value of the Range
        /// </summary>
        public T MinValue { get; set; }
        /// <summary>
        /// Maximum Value of the range
        /// </summary>
        public T MaxValue { get; set; }

        //Returns the range as comma separated string
        public string getRangeString()
        {
            return MinValue + "," + MaxValue;
        }
    }
}
