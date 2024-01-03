using STP.Domain.MovementsAndNotifications.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.MovementsAndNotifications.Interface
{
    public interface IFoldersProvider
    {
        #region GetFoldersSearchInfo(int pageNumber, int pageSize,int organisationId,string searchString);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedBackID"></param>
        /// <returns></returns>
        List<FoldersDomain> GetFoldersSearchInfo(int pageNumber, int pageSize, int organisationId, string searchString);
        #endregion

        #region InsertFolderInfo(int organisationId,string folderName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedBackID"></param>
        /// <returns></returns>
        int InsertFolderInfo(int organisationId, string folderName,int parentId);// string folderName,
        #endregion

        #region DeleteFolderInfo(int folderId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedBackID"></param>
        /// <returns></returns>
        int DeleteFolderInfo(int folderId);// string folderName,
        #endregion
        #region EditFolderInfo(int folderId,int organisationId,string folderName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedBackID"></param>
        /// <returns></returns>
        int EditFolderInfo(int folderId, int organisationId, string folderName);// string folderName,
        #endregion

        List<FolderTreeModel> GetFolders(int organisationId);
    }
}