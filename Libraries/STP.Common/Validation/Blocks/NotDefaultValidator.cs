#region

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

#endregion

namespace STP.Common.Validation.Blocks
{
    public class NotDefaultValidator<T> : ValueValidator<T>
    {
        public NotDefaultValidator(string messageTemplate, string tag, bool negated)
            : base(messageTemplate, tag, negated)
        {
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get { return string.Empty; }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return string.Empty; }
        }

        protected override void DoValidate(T objectToValidate, object currentTarget, string key,
            ValidationResults validationResults)
        {
            if (objectToValidate.Equals(default(T)))
            {
                validationResults.AddResult(
                    new ValidationResult(
                        string.Format("Field {0} cannot be a default value: {1}", key, objectToValidate), currentTarget,
                        key, Tag, this));
            }
        }
    }
}