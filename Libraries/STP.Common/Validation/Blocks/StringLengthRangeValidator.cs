#region

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

#endregion

namespace STP.Common.Validation.Blocks
{
    public class StringLengthRangeValidator : ValueValidator<string>
    {
        public StringLengthRangeValidator(bool allowNull, bool allowDefault, int minLength, int maxLength,
            string messageTemplate, string tag, bool negated)
            : base(messageTemplate, tag, negated)
        {
            AllowNull = allowNull;
            AllowDefault = allowDefault;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public bool AllowNull { get; set; }

        public bool AllowDefault { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        protected override string DefaultNegatedMessageTemplate
        {
            get { return "true"; }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return "true"; }
        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key,
            ValidationResults validationResults)
        {
            string validationTarget = objectToValidate;
            if (!AllowDefault && validationTarget == string.Empty)
            {
                validationResults.AddResult(
                    new ValidationResult(string.Format("Field {0} cannot be empty: {1}", key, objectToValidate),
                        currentTarget, key, Tag, this));
            }

            if (!AllowNull && validationTarget == null)
            {
                validationResults.AddResult(
                    new ValidationResult(string.Format("Field {0} cannot be null", key, objectToValidate), currentTarget,
                        key, Tag, this));
            }

            if (!string.IsNullOrEmpty(validationTarget))
            {
                if (validationTarget.Length < MinLength || validationTarget.Length > MaxLength)
                {
                    validationResults.AddResult(
                        new ValidationResult(
                            string.Format(
                                "Field {0} length is outside the acceptable range.  Value: {1}, Min: {2}, Min: {3}", key,
                                objectToValidate, MinLength, MaxLength), currentTarget, key, Tag, this));
                }
            }
        }
    }
}