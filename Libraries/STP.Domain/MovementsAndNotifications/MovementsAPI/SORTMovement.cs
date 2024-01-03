using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.SORTMovementsAPI
{
   public class Movement
    {
        public string ESDALReferenceNumber { get; set; }
        public string MovementType { get; set; }//SO or VR1
        public string CheckingStatus { get; set; }//Application status - Not checked, checked,Checked final
        public string ProjectStatus { get; set; } //Inprogress,Approved, Re-approved, Withadrawn, Declined
                                                 //
        public string Owner { get; set; }
        public string ApplicationDate { get; set; }

        public string FromSummary { get; set; }

        public string ToSummary { get; set; }

        public string DueDate { get; set; }
    }

    public class SORTMovementDetails
    {
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
        public int PageNumber { get; set; }       
        public int PageSize{ get; set; }
        public List<Movement> SORTMovements { get; set; }


    }
}
