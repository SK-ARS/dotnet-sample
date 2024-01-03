using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{

    //Dependency - Vehicle Configuration in Vehicle and Fleet
    /// <summary>
    /// This model will be used for filter the records of the Transmission
    /// </summary>
    public class TransmissionModelFilter
    {
        public bool Delivered { get; set; } //status is Delivered        
        public bool Failed { get; set; } //status is Failed
        public bool Pending { get; set; } //status is Pending
        public bool Sent { get; set; } //status is Sent        
        public bool All { get; set; } //status is All above selected
    }

    //Dependency - Vehicle Configuration in Vehicle and Fleet
    /// <summary>
    /// Basic model used to display the data of the transmission
    /// </summary>
    public class TransmissionModel
    {
        public int ContactID { get; set; } //The contact ID of the person receiving the transmission
        public string FirstName { get; set; } //The First name of the person receiving the transmission
        public string SurName { get; set; } //The Surname of the person receiving the transmission
        public int OrganisationID { get; set; } //The Organsiation ID of the person receiving the transmission
        public string OrganisationName { get; set; } //The organsiation of the person receiving the transmission
        public string Medium { get; set; } //The medium of transmission - Inbox, Fax or email
        public string Fax { get; set; } //Fax number on which transmission is sent

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } //Email to which the transmission is sent
        public int INBOX_ONLY { get; set; } //Inbox only flag
        public DateTime SentOn { get; set; } //Date and time on which the transmission is sent
        public int TRANSMISSION_STATUS { get; set; } //Status of the transmission
        public int InboxStatusCode { get; set; } //Status of the Inbox
        public decimal recordCount { get; set; } //The total record count fetched from the database, used during paging instead of separate query.
        public DateTime STATUS_UPDATE_TIME { get; set; } //Date and time on which the transmission is sent
        public string TRANSMISSION_STATUS_NAME { get; set; }// Name of transmission status
        public int TRANSMISSION_ID { get; set; }//ID of the transmission
        public string FULL_NAME { get; set; }
        public int IS_MANUALLY_ADDED { get; set; }
    }
}