namespace STP.Common.Validation
{
    public static class RegularExpressions
    {
        public static string NumberOnlyValidator = "\\d+";

        public static string EmailAddressValidator = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                                     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                                     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                                     + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    }
}