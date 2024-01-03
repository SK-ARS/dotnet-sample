using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class FeedbackDomain
    {

        public int ListCount { get; set; }
        public string SearchType { get; set; }
        public string Description { get; set; }
        public string FeedbackTypeName { get; set; }

        public long FeedBackId { get; set; }
        public string SearchString { get; set; }

        public int OpenCheck { get; set; }
        public string FullName { get; set; }
        public string ShowDateTime { get; set; }
        public string ResponseContent { get; set; }

        public long ContactId { get; set; }

    }
    public class InsertFeedbackDomain
    {
        public string FeedbackType { get; set; }
        public string FeedBack { get; set; }
        public string UserId { get; set; }
    }
}