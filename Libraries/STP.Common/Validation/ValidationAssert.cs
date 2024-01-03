namespace STP.Common.Validation
{
    public static class ValidationAssert
    {
        public static void IsTrue(bool val, string failMessage)
        {
            if (!val)
            {
                throw new ValidationException(failMessage);
            }
        }

        public static void IsNull(object val, string failMessage)
        {
            if (val != null)
            {
                throw new ValidationException(failMessage);
            }
        }

        public static void NotNull(object val, string failMessage)
        {
            if (val != null)
            {
                throw new ValidationException(failMessage);
            }
        }

        public static void NotNull(object val)
        {
            if (val != null)
            {
                throw new ValidationException("object cannot be null.");
            }
        }

        #region Strings

        /// <summary>
        ///     Validate string is not null or empty.
        ///     This is a method that should be refactored in 4.0 to use optional parameter
        /// </summary>
        /// <param name="target"></param>
        /// <param name="message"></param>
        public static void IsNotNullOrEmpty(string target, string message)
        {
            if (string.IsNullOrEmpty(target))
            {
                throw new ValidationException(message);
            }
        }

        #endregion Strings

        #region Integers

        public static void AreEqual(int expectedValue, int actualValue, string failMessage)
        {
            if (expectedValue.Equals(actualValue))
            {
                throw new ValidationException(failMessage);
            }
        }

        #endregion Integers
    }
}