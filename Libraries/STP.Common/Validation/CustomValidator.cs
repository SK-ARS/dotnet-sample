using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace STP.Common.Validation
{
    public class MustBeGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;

        public MustBeGreaterThanAttribute(string otherProperty, string errorMessage)
            : base(errorMessage)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-GB");

            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(_otherProperty);
            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
            DateTime fromDate = DateTime.Parse(value.ToString());
            DateTime toDate = DateTime.Parse(otherValue.ToString());


            /*string thisDateValue = Convert.ToDateTime(value).ToShortDateString();
            string otherDateValue = Convert.ToDateTime(otherValue).ToShortDateString();

            DateTime fromDate = DateTime.Parse(thisDateValue);
            DateTime toDate = DateTime.Parse(otherDateValue);*/

            int result = toDate.Date.CompareTo(fromDate.Date);

            if (fromDate.Date < toDate.Date)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }

    public class MustBeGreaterThanOrEqualAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;

        public MustBeGreaterThanOrEqualAttribute(string otherProperty, string errorMessage)
            : base(errorMessage)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-GB");

            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(_otherProperty);
            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
            DateTime fromDate = DateTime.Now;//DateTime.Parse(value.ToString());
            DateTime toDate = DateTime.Now;//DateTime.Parse(otherValue.ToString());

            if (value!=null)
            {
                fromDate = DateTime.Parse(value.ToString());
            }
            if (otherValue != null)
            {
                toDate = DateTime.Parse(otherValue.ToString());
            }

            /*string thisDateValue = Convert.ToDateTime(value).ToShortDateString();
            string otherDateValue = Convert.ToDateTime(otherValue).ToShortDateString();

            DateTime fromDate = DateTime.Parse(thisDateValue);
            DateTime toDate = DateTime.Parse(otherDateValue);*/

            int result = toDate.Date.CompareTo(fromDate.Date);

            if (fromDate.Date <= toDate.Date)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }

    public class RequiredFax : ValidationAttribute
    {
         private readonly string _otherProperty;

         public RequiredFax(string otherProperty, string errorMessage)
            : base(errorMessage)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(_otherProperty);
            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
            string _this = Convert.ToString(value);
            string _other = Convert.ToString(otherValue);

            if (_other == "fax" && string.IsNullOrEmpty(_this))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    public class RequiredEmail : ValidationAttribute
    {
        private readonly string _otherProperty;

        public RequiredEmail(string otherProperty, string errorMessage)
            : base(errorMessage)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(_otherProperty);
            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
            string _this = Convert.ToString(value);
            string _other = Convert.ToString(otherValue);

            if (_other == "email" && string.IsNullOrEmpty(_this))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }
}
