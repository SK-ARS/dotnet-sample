using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.SecurityAndUsers
{
    public class FeedBack
    {
        //you are saving total count of records
        public int listCount { get; set; }
        //you are specifying whether to search or fetch the entire list
        public string searchType { get; set; }
        //you are storing the feedback description
        public string description { get; set; }
        //you are specifying the feedback type which is 1 : Complaint ,2 : Suggestion,3 : general Request ,4 : Fault
        public string FeedbackTypeName { get; set; }

        public long feedBackId { get; set; }
        public string searchString { get; set; }

        public int openChk { get; set; }
        public string fullname { get; set; }
        public string showDateTime { get; set; }
        // public string FeedbackType { get; set; }
    }
}
