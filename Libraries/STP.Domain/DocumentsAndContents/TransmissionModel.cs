using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{    

    
    public class TransmissionModel
    {
        public int ContactID { get; set; } 
        public string FirstName { get; set; } 
        public string SurName { get; set; } 
        public int OrganisationID { get; set; } 
        public string OrganisationName { get; set; } 
        public string Medium { get; set; } 
        public string Fax { get; set; } 

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } 
        public int InboxOnly { get; set; } 
        public DateTime SentOnDate { get; set; } 
        public int TransmissionStatus { get; set; } 
        public int InboxStatusCode { get; set; } 
        public decimal RecordCount { get; set; } 
        public DateTime StatusUpdateTime { get; set; } 
        public string TransmissionStatusName { get; set; }
        public int TransmissionId { get; set; }
        public string FullName { get; set; }
        public int IsManuallyAdded { get; set; }
    }
    
    public class TransmissionModelFilter
    {
        public bool Delivered { get; set; }       
        public bool Failed { get; set; } 
        public bool Pending { get; set; } 
        public bool Sent { get; set; }       
        public bool All { get; set; } 
    }
}