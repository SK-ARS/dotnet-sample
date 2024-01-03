using STP.Domain.MovementsAndNotifications.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public interface IManageFolderService
    {
        List<FoldersDomain> GetFoldersSearchInfo(int pageNumber, int pageSize, int organisationId, string searchString);
        int InsertFolderInfo(int organisationId, string folderName);
        int DeleteFolderInfo(int folderId);
        int EditFolderInfo(int folderId, int organisationId, string folderName);

    }
}
