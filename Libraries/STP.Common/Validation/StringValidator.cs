#region

using System.Text.RegularExpressions;

#endregion

namespace STP.Common.Validation
{
    public class StringValidator
    {
        // (?=^.{6,14}$)(?=.*?\d)(?=.*?[A-Z]) - between 6-14 characters, at least 1 number, at least one capital letter
        //public const string JavascriptPasswordRegularExpression = @"(?=^.{6,14}$)(?=.*?\\d)(?=.*?[A-Z])";
        public const string PasswordRegularExpression = @"(?=^.{6,14}$)(?=.*?\d)(?=.*?[A-Z])";

        public static void ValidateStandard(string item)
        {
        }

        public static void ValidateMinLength(string item, int minSize)
        {
            if (item.Length < minSize)
                throw new ValidationException(string.Format("This value \"{0}\" must be greater than {1} characters.",
                    item, minSize));
        }

        public static void ValidateMaxLength(string item, int maxSize)
        {
            if (item.Length > maxSize)
                throw new ValidationException(string.Format(
                    "This value \"{0}\" cannot be greater than {1} characters.", item, maxSize));
        }

        public static void ValidateLength(string item, int minSize, int maxSize)
        {
            if (item.Length < minSize)
                throw new ValidationException(string.Format("This value \"{0}\" must be greater than {1} characters.",
                    item, minSize));
            if (item.Length > maxSize)
                throw new ValidationException(string.Format(
                    "This value \"{0}\" cannot be greater than {1} characters.", item, maxSize));
        }

        public static void ValidateAlphaNumeric(string item)
        {
            if (!Regex.IsMatch(item, "^[a-zA-Z0-9 ]*$"))
                throw new ValidationException(string.Format("Value '{0}' must be alphanumeric.", item));
        }

        /// <summary>
        ///     Validates a password according to rules applied in OLPS
        /// </summary>
        /// <param name="item"></param>
        public static void ValidatePassword(string item)
        {
            if (!Regex.IsMatch(item, PasswordRegularExpression))
            {
                throw new ValidationException(
                    "Value must be 6-14 characters long, contain a capital letter, and contain a number.");
            }
        }

        public static void ValidateCommaSpacedAlphaNumberic(string itemNos)
        {
            string[] splitItems = itemNos.Split(',');
            foreach (string itemNo in splitItems)
                ValidateAlphaNumeric(itemNo);
        }

        public static void ValidateNoNumbers(string source)
        {
            if (ContainsNumbers(source))
                throw new ValidationException(string.Format("Value cannot contain numbers.  Was: {0}", source));
        }

        public static void ValidateNotNull(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ValidationException(string.Format("Value cannot be null.  Was: {0}", source));
        }

        public static void ValidateCurrency(string source)
        {
            ValidateNoNumbers(source);
            ValidateLength(source, 3, 3);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Not the fastest implementation but faster than regular expressions
        /// </remarks>
        public static bool ContainsNumbers(string source)
        {
            char[] charArray = source.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if ((charArray[i] >= '0') && (charArray[i] <= '9'))
                    return true;
            }
            return false;
        }
    }
}