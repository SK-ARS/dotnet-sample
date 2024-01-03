using STP.Common.Validation;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class DispensationGridList
    {
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9!@#\$\^\&\?\*\/\)\(+=._ -]+$", ErrorMessage = "Please enter valid reference number")]
        [Required(ErrorMessage = "The reference number is required")]
        [AlternateValidation(Suggetion = "Please enter a valid reference number")]
        public string DispensationReferenceNo { get; set; }

        [Required(ErrorMessage = "The brief description is required")]
        public string Summary { get; set; }
        [Required(ErrorMessage = "The issued by is required")]
        public string GrantedBy { get; set; }
        [Required(ErrorMessage = "The description is required")]
        public string Description { get; set; }

      //  [MustBeGreaterThanOrEqualAttribute("ToDate", "Start date must be less than or equal to end date")]
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Int32? GrossWeight { get; set; }
        public Int32? SignedAxleWeight { get; set; }
        public double? OverallLength { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double DispensationId { get; set; }
        public int OrganisationId { get; set; }
        public int SelectOrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ListCount { get; set; }
        public int Checkcount { get; set; }
        public int ContactId { get; set; }
        public int OrganisationTypeId { get; set; }
        public string UserTypeName { get; set; }
        public int UserTypeId { get; set; }

        public int RecordCount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }

    public class DispensationSearchItems
    {
        public string Criteria { get; set; }
        public string DRN { get; set; }
        public string Summary { get; set; }
        public string Authority { get; set; }
        public string Description { get; set; }
        public bool Expired { get; set; }
        public string SearchType { get; set; }
        public string SearchName { get; set; } 
        public int SortOrderValue { get; set; }
        public int SortTypeValue { get; set; }
    }

    public class EditDispensationRestriction
    {
        public Int32 GrossCheck { get; set; }
        public Int32 Axle { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}