using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
    public class HaulierContactModel:IValidatableObject
    {
        public long HaulierContactId { get; set; } 

        public long OrganisationId { get; set; }

      //  [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [StringLength(70, MinimumLength = 2)]
        public string Name { get; set; }


      //  [Required(ErrorMessage = "Organisation Name is required")]
        [DataType(DataType.Text)]
        [StringLength(70, MinimumLength = 2)]
        public string OrganisationName { get; set; }

        //[DataType(DataType.Text)]
        //[RegularExpression(@"^(\+)?(?<!\d)\d{12}(?!\d)$", ErrorMessage = "Please enter a valid Fax number")]
        //[StringLength(100)]
        public string Fax { get; set; }

        //[DataType(DataType.Text)]
        //[StringLength(255)]
        //[RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$", ErrorMessage = "Please enter a valid E-mail")]
        public string Email { get; set; } 

        
        public long CommunicationMethod { get; set; } 

        public string CommunicationMethodName { get; set; } 

        public int CommunicationMethodType { get; set; } 

        public string SearchType { get; set; } 

        public string SearchName { get; set; } 

        public decimal RecordCount { get; set; } 

        /// <summary>
        /// Dynamic validation for Email and Fax
        /// </summary>
        /// <param name="Validate">Validation text is mentioned</param>
        /// <returns>Validation message to the end user</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CommunicationMethodName == "fax")
            {
                var property = new[] { "FAX" };
               
                Regex Rgxfax = new Regex(@"^(\+)?(?<!\d)\d{12}(?!\d)$");
               

                if (Fax == null || Fax == "")
                {
                    yield return new ValidationResult("Fax number is required.", property);
                }
                else if (!Rgxfax.IsMatch(Fax) || Fax.Length < 12)
                {
                    yield return new ValidationResult("Enter 12 digit Fax number.", property);
                }
                else if (!Rgxfax.IsMatch(Fax) || Fax.Length > 100)
                {
                    yield return new ValidationResult("Enter a valid fax number.For ex. +123456789012", property);
                }
            }
            else if (CommunicationMethodName == "email html")
            {
                Regex Rgxemail = new Regex(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$");

                var property = new[] { "EMAIL" };

                if (Email == null || Email == "")
                {
                    yield return new ValidationResult("Email address is required.", property);
                }
                else if (!Rgxemail.IsMatch(Email))
                {
                    yield return new ValidationResult("Enter a valid email address.", property);
                }
            }
        }
    }
    public class HaulierContactModelSearch
    {
        public string SearchColumn { get; set; } 
        public string SearchValue { get; set; }
    }
}
