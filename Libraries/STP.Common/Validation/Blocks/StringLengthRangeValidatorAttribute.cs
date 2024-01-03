#region

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

#endregion

namespace STP.Common.Validation.Blocks
{
    public class StringLengthRangeValidatorAttribute : ValueValidatorAttribute
    {
        private readonly bool _allowDefault = true;
        private readonly bool _allowNull = true;
        private readonly int _maxLength = int.MaxValue;
        private readonly int _minLength = int.MinValue;

        public StringLengthRangeValidatorAttribute(bool allowNull, bool allowDefault, int minLength, int maxLength)
        {
            _allowNull = allowNull;
            _allowDefault = allowDefault;
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            if (targetType == typeof (string))
            {
                return new StringLengthRangeValidator(_allowNull, _allowDefault, _minLength, _maxLength, MessageTemplate,
                    Tag, false);
            }
            else
            {
                throw new ArgumentException("This validator only operates on strings");
            }
        }
    }
}