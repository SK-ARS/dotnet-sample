
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;
using STP.Domain.DocumentsAndContents;

namespace STP.DocumentsAndContents.Interface
{
      public   interface IInformation
    {
        List<InformationModel> GetInformationList(int pageNumber, int pageSize, string pageName, string contentType, int userType, int sortOrder, int presetFilter);
        InformationModel ManageInformation(InformationModel informationModel);
        int DeleteInformation(int deletedContentId);
        List<WebContentFile> GetAssociatedFilesByContentId(int CONTENT_ID);
        List<InformationModel> GetEnumValsListByEnumType(string EnumTypeName);
        List<InformationModel> GetPortalContentById(int CONTENT_ID);
        List<InformationModel> GetDownloadList(int pageNum, int pageSize, string PageName, string ContentType, int UserType, int IsAdmin, string DownloadType);
        InformatinDetail GetAllHotNews(long userTypeId);
        List<InformationModel> GetUniqueInfoList(int pageNum, int pageSize, int portalid, string SearchType);
        List<InformationModel> GetInformationListPortal(int pageNumber, int pageSize, string contentType,long userType);
        List<InformatinDetail> GetHotNewsForAdmin(string SearchType);
        List<LatestNews> GetLatestNews(int portalId, int timeInterval);
    }
}
