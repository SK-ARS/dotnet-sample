using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class CollaborationModel
    {
        public string Title { get; set; } //The title of the person in the collaboration
        public string FirstName { get; set; } //The First name of the person in the collaboration
        public string SurName { get; set; } //The Surname of the person in the collaboration        
        public string OrganisationName { get; set; } //The organsiation of the person in the collaboration
        public string NotificationCode { get; set; } //Notification code in the collaboration
        public DateTime DateAndTime { get; set; } //Date and time in the collaboration
        public string Notes { get; set; } //The notes in the collaboration 
        public decimal RecordCount { get; set; } //The total record count fetched from the database, used during paging instead of separate query.

        public string PhoneNumber { get; set; } //Notification code in the collaboration
        public long DocumentId { get; set; } //Document ID required for acknowledgement of the notes ie to set the acknowledge flag
        public int CollaborationNo { get; set; } //Collaboration No is required for acknowledgement of the notes ie to set the acknowledge flag DocumentID+CollborationNo is PK

        public string ContactDetail { get; set; }

        #region Code added by NetWeb
        public DateTime WHEN { get; set; }
        public string ExternalCollaboratonStatus { get; set; }
        public string ExternalNotes { get; set; }
        public string InternalCollaborationStatus { get; set; }
        public string InternalNotes { get; set; }
        public decimal TotalRecordCount { get; set; }
        #endregion

    }
}