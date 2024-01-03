using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
    public class ChangePasswordInfo
    {
        public int UserId { get; set; }
        public string ExistingPassword { get; set; }
        [Required(ErrorMessage = "Please enter Old Password.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Please enter New Password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=[{\]};:<>.?,-~`])[A-Za-z\d!@#$%^&*()_+=[{\]};:<>.?,-~`]{6,12}", ErrorMessage = "Password must contain: Minimum 6 and Maximum 12 characters, atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character (except ', \").")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Please enter a password between 6 to 12 characters")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Re-enter New Password.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match. Please try again.")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Please enter a password between 6 to 12 characters")]
        public string ConfirmPassword { get; set; }
        public int? SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        public string AcceptedTerms { get; set; }
        public int? AcceptedTermId { get; set; }
        public int? RequiredTerm { get; set; }
    }
}