#region

using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

#endregion

namespace STP.Common.Enums
{
    /// <summary>
    ///     Creates the Spacing from Enums
    /// </summary>
    public static class EnumDescriptor
    {
        /// <summary>
        ///     Gets Description from EnumType Value
        /// </summary>
        /// <param name="myEnumType"></param>
        /// <returns></returns>
        public static string GetDescriptionOf(object myEnumType)
        {
            // Check if any description is specified to the enum
            string enumDescription = GetDescription(myEnumType);

            //if Not Create Spaceing between 2 Capital Letters from the Enum Value 
            if (string.IsNullOrEmpty(enumDescription))
            {
                // By default, the result is just ToString with
                // a space in front of each capital letter.
                var capitalLetterMatch = new Regex("\\B[A-Z]", RegexOptions.Compiled);
                enumDescription = capitalLetterMatch.Replace(myEnumType.ToString(), " $&");
                MemberInfo[] memberInfo = myEnumType.GetType().GetMember(myEnumType.ToString());
                if (memberInfo.Length == 1)
                {
                    object[] customAttributes = memberInfo[0].GetCustomAttributes(typeof (Description), false);
                    if (customAttributes.Length == 1)
                    {
                        enumDescription = ((Description) customAttributes[0]).text;
                    }
                }
            }
            return enumDescription;
        }

        private static string GetDescription(object myEnumType)
        {
            string retVal;
            try
            {
                FieldInfo fieldInfo = myEnumType.GetType().GetField(myEnumType.ToString());
                var attributes =
                    (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false);
                retVal = attributes.Length > 0 ? attributes[0].Description : string.Empty;
            }
            catch (Exception e)
            {
                retVal = string.Empty;
                throw e;
                //Occurs when we attempt to get description of an enum value that does not exist
            }
            return retVal;
        }

        #region Nested type: Description

        public class Description : Attribute
        {
            public readonly string text;

            public Description(string text)
            {
                this.text = text;
            }
        }

        #endregion
    }
}