using STP.Domain.MovementsAndNotifications.Folder;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.MovementsAndNotifications.Providers
{
    internal sealed class FoldersProvider : IFoldersProvider
    {
        #region
        private FoldersProvider()
        {
        }
        internal static FoldersProvider Instance
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
            internal static readonly FoldersProvider instance = new FoldersProvider();
        }
        
        #endregion
        public List<FoldersDomain> GetFoldersSearchInfo(int pageNumber, int pageSize, int organisationId, string searchString)
        {
            return FoldersDAO.GetFoldersSearchInfo(pageNumber, pageSize, organisationId, searchString);
        }
        public int InsertFolderInfo(int organisationId, string folderName, int parentId = 0)
        {
            return FoldersDAO.InsertFolderInfo(organisationId, folderName, parentId);
        }
        public int DeleteFolderInfo(int folderId)
        {
            return FoldersDAO.DeleteFolderInfo(folderId);
        }
        public int EditFolderInfo(int folderId, int organisationId, string folderName)
        {
            return FoldersDAO.EditFolderInfo(folderId, organisationId, folderName);
        }

        public List<FolderTreeModel> GetFolders(int organisationId)
        {
            return FoldersDAO.GetFolders(organisationId);
        }
        public int AddItemToFolder(AddItemFolderModel model)
        {
            return FoldersDAO.AddItemToFolder(model);
        }
        public int RemoveItemsFromFolder(AddItemFolderModel model)
        {
            return FoldersDAO.RemoveItemsFromFolder(model);
        }
        public int MoveFolderToFolder(FolderTreeModel model)
        {
            return FoldersDAO.MoveFolderToFolder(model);
        }
    }
}