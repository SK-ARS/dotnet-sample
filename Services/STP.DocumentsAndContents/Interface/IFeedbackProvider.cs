using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.DocumentsAndContents.Interface
{
    interface IFeedbackProvider
    {
        List<FeedbackDomain> GetFeedbackSearchInfo(int pageNumber, int pageSize, string searchtype, int flag, string searchString, int sortOrder, int presetFilter);
        int DeleteFeedbackDetails(int feedBackId);
        FeedbackDomain GetFeedbackInfo(long feedBackId, int openChk);
    }
}
