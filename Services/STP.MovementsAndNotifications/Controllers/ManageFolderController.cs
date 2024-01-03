using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Folder;
using STP.MovementsAndNotifications.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.MovementsAndNotifications.Controllers
{
    public class ManageFolderController : ApiController
    {
        [HttpGet]
        [Route("MovementsFolder/GetFoldersSearchInfo")]
        public IHttpActionResult GetFoldersSearchInfo(int pageNumber, int pageSize, int organisationId, string searchString)
        {
            List<FoldersDomain> folderObj;
            try
            {
                folderObj = FoldersProvider.Instance.GetFoldersSearchInfo(pageNumber, pageSize, organisationId, searchString);
                return Content(HttpStatusCode.OK, folderObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/GetFoldersSearchInfo, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("MovementsFolder/InsertFolderInfo")]
        public IHttpActionResult InsertFolderInfo(InsertFolderParams insertFolderParams)
        {
            try
            {
                int result = FoldersProvider.Instance.InsertFolderInfo(insertFolderParams.OrganisationId, insertFolderParams.FolderName,insertFolderParams.ParentId);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/InsertFolderInfo , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("MovementsFolder/DeleteFolderInfo")]
        public IHttpActionResult DeleteFolderInfo(EditFolderParams editFolderParams)
        {
            try
            {
                int result = FoldersProvider.Instance.DeleteFolderInfo(editFolderParams.FolderId);
                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                }

                else if (result == 0)
                {
                    return Content(HttpStatusCode.OK, result);

                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/DeleteFolderInfo , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("MovementsFolder/EditFolderInfo")]
        public IHttpActionResult EditFolderInfo(EditFolderParams editFolderParams)
        {
            try
            {
                int result = FoldersProvider.Instance.EditFolderInfo(editFolderParams.FolderId, editFolderParams.OrganisationId, editFolderParams.FolderName);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/EditFolderInfo , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("MovementsFolder/GetFolders")]
        public IHttpActionResult GetFolders(int organisationId)
        {
            List<FolderTreeModel> folderObj;
            IList<FolderTreeModel> output = new List<FolderTreeModel>();
            try
            {
                folderObj = FoldersProvider.Instance.GetFolders(organisationId);
                if (folderObj != null)
                    output = FlatToHierarchy(folderObj);
                return Content(HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/GetFolders, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        public IList<FolderTreeModel> FlatToHierarchy(IEnumerable<FolderTreeModel> list, int parentId = 0)
        {
            return (from i in list
                    where i.ParentFolderId == parentId
                    select new FolderTreeModel
                    {
                        FolderId = i.FolderId,
                        ParentFolderId = i.ParentFolderId,
                        FolderName = i.FolderName,
                        OrganisationId = i.OrganisationId,
                        Children = FlatToHierarchy(list, i.FolderId)
                    }).ToList();
        }


        [HttpPost]
        [Route("MovementsFolder/AddItemToFolder")]
        public IHttpActionResult AddItemToFolder(List<AddItemFolderModel> model)
        {
            try
            {
                int result = 0;
                if (model != null && model.Any())
                {
                    foreach (var item in model)
                    {
                        var output = FoldersProvider.Instance.AddItemToFolder(item);
                        if (output == 0)
                            Logger.GetInstance().LogMessage(Log_Priority.DEBUG, Logger.LogInstance + @"MovementsFolder/AddItemToFolder - Unable to insert item> output:" + output + "- folder:" + item.FolderId + "");
                        result = output > 0 ? output : result;
                    }
                }
                if (result < 0)
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InsertionFailed);
                }

                else if (result == 0)
                {
                    return Content(HttpStatusCode.OK, result);

                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/AddItemToFolder , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("MovementsFolder/RemoveItemsFromFolder")]
        public IHttpActionResult RemoveItemsFromFolder(List<AddItemFolderModel> model)
        {
            try
            {
                int result = 0;
                if(model!=null && model.Any())
                {
                    foreach(var item in model)
                    {
                       var output = FoldersProvider.Instance.RemoveItemsFromFolder(item);
                        if (output == 0)
                            Logger.GetInstance().LogMessage(Log_Priority.DEBUG, Logger.LogInstance + @"MovementsFolder/RemoveItemsFromFolder - Unable to remove item> output:" + output + "- folder:" + item.FolderId + "");
                       result = output > 0 ? output : result;
                    }
                }
                if (result < 0)
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
                else if (result == 0)
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/RemoveItemsFromFolder , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("MovementsFolder/MoveFolderToFolder")]
        public IHttpActionResult MoveFolderToFolder(FolderTreeModel model)
        {
            try
            {
                int result = FoldersProvider.Instance.MoveFolderToFolder(model);
                if (result < 0)
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.UpdationFailed);
                else if (result == 0)
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"MovementsFolder/MoveFolderToFolder , Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

    }
}
