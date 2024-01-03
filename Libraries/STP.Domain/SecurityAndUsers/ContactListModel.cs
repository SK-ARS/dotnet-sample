using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace STP.Domain.SecurityAndUsers
{
    public class ContactListModel
    {
        public int ContactId { get; set; }  

        public int OnBehalfOfId { get; set; }

       
        public string FirstName { get; set; }

        
        public string SurName { get; set; } 

       
        public string Title { get; set; }

        
        public string AddressLine1 { get; set; } 

        public string AddressLine2 { get; set; } 

        public string AddressLine3 { get; set; } 

        public string AddressLine4 { get; set; } 

        public string AddressLine5 { get; set; } 

        public string PostCode { get; set; } 

        public int CountryId { get; set; }

        public string Country { get; set; }  

         
        public string PhoneNumber { get; set; }  

        public string Extension { get; set; } 

      
        public string Mobile { get; set; } 

       
        public string Fax { get; set; } 

       
        public string Email { get; set; }

        public string Comments { get; set; }  

        public string Initials { get; set; }  

        public int OrganisationId { get; set; } 

        public string Organisation { get; set; } 

        public string Deleted { get; set; } 

        public string ContactType { get; set; } 

        public string OnBehalfOf { get; set; } 

        public string NotificationMethod { get; set; } 

        public string SearchType { get; set; } 
        public string SearchName { get; set; } 

        public Byte[] AffectedParties { get; set; }

        public decimal RecordCount { get; set; } 

        public string OtherOrganisation { get; set; } 

        public Byte[] RevisedParties { get; set; }
        public string Reason { get; set; }


        public string IsPolice { get; set; }

        public int DelegatorsOrganisationId { get; set; }
    }

    public class ContactSearchModel
    {
        public string SearchColumn { get; set; } 
        public string SearchValue { get; set; } 
    }
}
