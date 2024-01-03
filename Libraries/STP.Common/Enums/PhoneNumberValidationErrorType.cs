namespace STP.Common.Enums
{
    public enum PhoneNumberValidationErrorType
    {
        
        None = 0,
        
        Valid = 1,
        
        InvalidCharacter = 50,
        
        InvalidCountryCode = 100,
        
        TelephoneNumberBlank = 110,
        
        AreaCodeBlank = 120,
        
        InvalidPhoneType = 140,
        
        PhoneNumberTooShort = 200,
        
        PhoneNumberTooLong = 210,
        
        AreaCodeTooShort = 220,
        
        AreaCodeTooLong = 230,
        
        PhoneBodyTooShort = 240,
        
        PhoneBodyTooLong = 250,
        
        PrefixInvalid = 300
    }
}