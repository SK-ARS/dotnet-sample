#region

using System;

#endregion

namespace STP.Common.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {
        }
    }
}