using STP.Domain;
using STP.Domain.DocumentsAndContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace STP.ServiceAccess.DocumentsAndContents
{
    public interface IInformationService
    {
        /// <summary>
        /// Interface to get InformationModel list
        /// </summary>
        /// <returns></returns>
        List<InformationModel> GetInformationList(int pageNumber, int pageSize, string pageName, string contentType, int userType, int sortOrder, int presetFilter);
        int DeleteInformation(int deletedContactId);
        List<InformationModel> GetEnumValsListByEnumType(string EnumTypeName);
        List<WebContentFile> GetAssociatedFilesByContentId(int CONTENT_ID);
        List<InformationModel> GetPortalContentById(int CONTENT_ID);
        InformationModel GetInformationById(int managedContentId);
        InformationModel ManageInformation(InformationModel infoModel);
        bool ManageInformationFiles(InformationModel webContentFile);
        List<InformationModel> GetDownloadList(int pageNum, int pageSize, string PageName, string ContentType, int UserType, int IsAdmin, string DownloadType);
        InformatinDetail GetAllHotNews(long userTypeId);
        List<InformationModel> GetUniqueInfoList(int pageNum, int pageSize, int portalid, string SearchType);
        List<InformationModel> GetInformationListPortal(int pageNumber, int pageSize, string contentType, long userType);
        List<InformatinDetail> GetHotNewsForAdmin(string SearchType);
        List<LatestNews> GetLatestNews(int portalId, int timeInterval);
    }
}