#region

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

#endregion

namespace STP.Common.Validation.Blocks
{
    public class NotDefaultValidatorAttribute : ValueValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            if (targetType == typeof (int))
            {
                return new NotDefaultValidator<int>(MessageTemplate, Tag, false);
            }
            else if (targetType == typeof (decimal))
            {
                return new NotDefaultValidator<decimal>(MessageTemplate, Tag, false);
            }
            else if (targetType == typeof (DateTime))
            {
                return new NotDefaultValidator<DateTime>(MessageTemplate, Tag, false);
            }
            else if (targetType == typeof (double))
            {
                return new NotDefaultValidator<double>(MessageTemplate, Tag, false);
            }
            else if (targetType == typeof (bool))
            {
                return new NotDefaultValidator<bool>(MessageTemplate, Tag, false);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}