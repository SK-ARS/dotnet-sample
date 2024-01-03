using STP.Domain;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.DocumentsAndContents
{
    public interface IFeedbackService
    {

     
        int InsertFeedbackDetails(InsertFeedbackDomain objInsertFeedback);
        List<FeedbackDomain> GetFeedbackSearchInfo(int pageNumber, int pageSize, string searchtype, int flag, string searchString, int sortOrder, int presetFilter);
        FeedbackDomain GetFeedbackInfo(long feedBackId, int openChk);
        int DeleteFeedbackDetails(int feedBackId);
    }
}
