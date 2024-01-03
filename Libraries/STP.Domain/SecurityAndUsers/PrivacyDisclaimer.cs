using System;
using System.ComponentModel.DataAnnotations;

namespace STP.Domain.SecurityAndUsers
{
    public class PrivacyDisclaimer
    {
        [Required(ErrorMessage = "Description is required")]
        [StringLength(4000, MinimumLength = 15, ErrorMessage = "Minimum 15, maximum 4000 characters")]
        public string Description { get; set; }
    }
}
