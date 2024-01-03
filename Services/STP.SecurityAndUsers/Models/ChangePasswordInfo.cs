using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Models
{
    public class ChangePasswordInfo
    {
        public int UserId { get; set; }
        public string ExistingPassword { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        //[Required(ErrorMessage = "Please select question.")]
        public int? SecurityQuestion { get; set; }
        //[Required(ErrorMessage = "Please enter an answer between 2 to 50 characters.")]
        //[StringLength(50, MinimumLength = 2, ErrorMessage = "Please enter an answer between 2 to 50 characters.")]
        public string SecurityAnswer { get; set; }

        public string AcceptedTerms { get; set; }
        public int? acceptedTermID { get; set; }
        public int? requiredTerm { get; set; }
    }
}