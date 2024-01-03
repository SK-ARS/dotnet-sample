using STP.DocumentsAndContents.Interface;
using STP.DocumentsAndContents.Persistance;
using STP.Domain;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Providers
{
    public class FeedbackProvider : IFeedbackProvider
    {

        #region
        protected FeedbackProvider()
        {
        }
        internal static FeedbackProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly FeedbackProvider instance = new FeedbackProvider();
        }
        #endregion      

        #region InsertFeedback
        /// <summary>
        /// Store the content of Feedback received from the user in the database
        /// </summary>
        /// <param name="feedbackType">Type of the Feedback</param>
        /// <param name="feedBack">Feedback Content</param>
        /// <returns>Boolean value indicating whether given record is inserted successfully or not</returns>
        public int InsertFeedbackDetails(InsertFeedbackDomain insertFeedbackDomain)
        {
            return FeedbackDAO.InsertFeedbackDetails(insertFeedbackDomain);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchtype"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<FeedbackDomain> GetFeedbackSearchInfo(int pageNumber, int pageSize, string searchtype, int flag, string searchString, int sortOrder, int presetFilter)
        {
            return FeedbackDAO.GetFeedbackSearchInfo(pageNumber, pageSize, flag, searchtype, searchString, sortOrder, presetFilter);
        }

        public int DeleteFeedbackDetails(int feedBackId)
        {
            return FeedbackDAO.DeleteFeedbackInfo(feedBackId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedBackID"></param>
        /// <returns></returns>
        public FeedbackDomain GetFeedbackInfo(long feedBackId, int openChk)
        {
            return FeedbackDAO.GetFeedbackdetails(feedBackId, openChk);
        }
    }

}
