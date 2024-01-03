using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Folder
{
    public class FoldersDomain
    {
        public int FolderId { get; set; }
        public int OrganisationId { get; set; }
        public string FolderName { get; set; }
        public string SearchString { get; set; }
        public int Listcount { get; set; }
        public int RecordCount { get; set; }
        public int? ParentFolderId { get; set; }
    }
    public class InsertFolderParams
    {
        public int ParentId { get; set; }
        public int OrganisationId { get; set; }
        public string FolderName { get; set; }
    }
    public class EditFolderParams
    {
        public int FolderId { get; set; }
        public int OrganisationId { get; set; }
        public string FolderName { get; set; }
    }

    public class AddItemFolderModel
    {
        public int FolderId { get; set; }
        public int OrganisationId { get; set; }
        public long MovementRevisionId { get; set; }
        public long MovementVersionId { get; set; }
        public long NotificationId { get; set; }
        public string MovementType { get; set; }
        public long ProjectId { get; set; }
        public string UserSchema { get; set; }
    }

    public class FolderTreeModel
    {
        public int FolderId { get; set; }
        public int OrganisationId { get; set; }
        public string FolderName { get; set; }
        public int? ParentFolderId { get; set; }
        public IList<FolderTreeModel> Children { get; set; } = new List<FolderTreeModel>();
    }
}